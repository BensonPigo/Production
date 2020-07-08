using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P09
    /// </summary>
    public partial class P09 : Win.Tems.QueryForm
    {
        private bool canUnConfirm;
        private DataTable PackID;

        /// <summary>
        /// P09
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.checkOnlyNotYetScanComplete.Checked = true;
            this.canUnConfirm = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P09. Scan && Pack", "CanUnConfirm");
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtcountryDestination.TextBox1.ReadOnly = true;
            this.btnUncomplete.Enabled = false;
            this.btnStartToScan.Enabled = false;

            // Grid設定
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(15))
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(8))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15))
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(10))
                .Text("Color", header: "Color", width: Widths.AnsiChars(10))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Text("QtyPerCTN", header: "PC/CTN", width: Widths.AnsiChars(5))
                .Numeric("ShipQty", header: "Qty")
                .Text("ScanQty", header: "PC/Ctn Scanned", width: Widths.AnsiChars(8))
                .Text("Barcode", header: "Ref. Barcode", width: Widths.AnsiChars(13));

            // 按Header沒有排序功能
            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        // SP#
        private void TxtSP_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtSP.Text) && this.txtSP.OldValue != this.txtSP.Text)
            {
                // sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", this.txtSP.Text);
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisioinid", Sci.Env.User.Keyword);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);

                string sqlCmd = "select ID from Orders WITH (NOLOCK) where MDivisionID = @mdivisioinid and ID = @id";
                DataTable ordersData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out ordersData);
                if (!result || ordersData.Rows.Count <= 0)
                {
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail.\r\n" + result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("SP# not found!!");
                    }

                    this.txtSP.Text = string.Empty;

                    // 清除所有欄位資料
                    this.displayStyle.Value = string.Empty;
                    this.displaySeason.Value = string.Empty;
                    this.displayBrand.Value = string.Empty;
                    this.displayBuyer.Value = string.Empty;
                    this.displayM.Value = string.Empty;
                    this.displayOrder.Value = string.Empty;
                    this.displayPONo.Value = string.Empty;
                    this.displayFactory.Value = string.Empty;
                    this.txtcountryDestination.TextBox1.Text = string.Empty;
                    this.numOrderQty.Value = 0;
                    if (this.PackID != null)
                    {
                        this.PackID.Clear();
                        MyUtility.Tool.SetupCombox(this.comboPackingNoFilter, 1, this.PackID);
                        this.comboPackingNoFilter.SelectedIndex = -1;
                    }

                    this.txtQuickSelectCTN.Text = string.Empty;
                    this.btnUncomplete.Enabled = false;
                    this.btnStartToScan.Enabled = false;
                    if ((DataTable)this.listControlBindingSource1.DataSource != null)
                    {
                        ((DataTable)this.listControlBindingSource1.DataSource).Clear();
                    }

                    return;
                }
            }
        }

        // SP#
        private void TxtSP_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtSP.Text) && (this.txtSP.OldValue != this.txtSP.Text))
            {
                DataRow ordersData;
                string sqlCmd = string.Format("select StyleID,SeasonID,BrandID,Dest,Qty,Customize1,CustPONo,FtyGroup,isnull((select BuyerID from Brand WITH (NOLOCK) where ID =  Orders.BrandID),'') as BuyerID from Orders WITH (NOLOCK) where ID = '{0}'", this.txtSP.Text);
                MyUtility.Check.Seek(sqlCmd, out ordersData);
                this.displayStyle.Value = MyUtility.Convert.GetString(ordersData["StyleID"]);
                this.displaySeason.Value = MyUtility.Convert.GetString(ordersData["SeasonID"]);
                this.displayBrand.Value = MyUtility.Convert.GetString(ordersData["BrandID"]);
                this.displayBuyer.Value = MyUtility.Convert.GetString(ordersData["BuyerID"]);
                this.displayM.Value = Sci.Env.User.Keyword;
                this.displayOrder.Value = MyUtility.Convert.GetString(ordersData["Customize1"]);
                this.displayPONo.Value = MyUtility.Convert.GetString(ordersData["CustPONo"]);
                this.displayFactory.Value = MyUtility.Convert.GetString(ordersData["FtyGroup"]);
                this.txtcountryDestination.TextBox1.Text = MyUtility.Convert.GetString(ordersData["Dest"]);
                this.numOrderQty.Value = MyUtility.Convert.GetInt(ordersData["Qty"]);

                sqlCmd = string.Format(
                    @"select ID,CTNStartNo,ShipModeID,ShipQty,NotYetScan,
SUBSTRING(OrderID,1,LEN(OrderID)-1) as OrderID,
SUBSTRING(Article,1,LEN(Article)-1) as Article,
SUBSTRING(Color,1,LEN(Color)-1) as Color,
SUBSTRING(SizeCode,1,LEN(SizeCode)-1) as SizeCode,
SUBSTRING(QtyPerCTN,1,LEN(QtyPerCTN)-1) as QtyPerCTN,
SUBSTRING(Barcode,1,LEN(Barcode)-1) as Barcode,
SUBSTRING(ScanQty,1,LEN(ScanQty)-1) as ScanQty
from (select ID,CTNStartNo,ShipModeID,min(Seq) as Seq,NotYetScan,ShipQty,OrderID,Article,Color,SizeCode,QtyPerCTN,Barcode,ScanQty 
	  from (select pd.ID,pd.CTNStartNo,p.ShipModeID,Seq,iif(Barcode = '' or Barcode is null, 1,0) as NotYetScan,
				   (select isnull(sum(ShipQty),0) from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and CTNStartNo = pd.CTNStartNo) as ShipQty,
				   (select OrderID+'/' from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and OrderID = pd.OrderID and CTNStartNo = pd.CTNStartNo for xml path('')) as OrderID,
				   (select Article+'/' from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and OrderID = pd.OrderID and CTNStartNo = pd.CTNStartNo for xml path('')) as Article,
				   (select Color+'/' from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and OrderID = pd.OrderID and CTNStartNo = pd.CTNStartNo for xml path('')) as Color,
				   (select SizeCode+'/' from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and OrderID = pd.OrderID and CTNStartNo = pd.CTNStartNo for xml path('')) as SizeCode,
				   (select CONCAT(isnull(QtyPerCTN,0),'/') from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and OrderID = pd.OrderID and CTNStartNo = pd.CTNStartNo for xml path('')) as QtyPerCTN,
				   (select Barcode+'/' from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and OrderID = pd.OrderID and CTNStartNo = pd.CTNStartNo for xml path('')) as Barcode,
				   (select CONCAT(isnull(ScanQty,0),'/') from PackingList_Detail WITH (NOLOCK) where ID = pd.ID and OrderID = pd.OrderID and CTNStartNo = pd.CTNStartNo for xml path('')) as ScanQty
			from PackingList_Detail pd WITH (NOLOCK) 
			left join PackingList p WITH (NOLOCK) on p.ID = pd.ID 
			where pd.OrderID = '{0}' and pd.QtyPerCTN > 0 and p.Type = 'B') b
	  group by ID,CTNStartNo,ShipModeID,NotYetScan,ShipQty,OrderID,Article,Color,SizeCode,QtyPerCTN,Barcode,ScanQty) a
order by ID,Seq", this.txtSP.Text);
                DataTable gridData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Sql connection fail.\r\n" + result.ToString());
                }

                this.listControlBindingSource1.DataSource = gridData;

                sqlCmd = string.Format("select distinct ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}'", this.txtSP.Text);
                result = DBProxy.Current.Select(null, sqlCmd, out this.PackID);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Query Packing ID fail.\r\n" + result.ToString());
                }

                // 當Grid有資料時Start to scan按鈕才可以按
                if (gridData == null)
                {
                    this.btnStartToScan.Enabled = false;
                }
                else
                {
                    this.btnStartToScan.Enabled = true;
                }

                MyUtility.Tool.SetupCombox(this.comboPackingNoFilter, 1, this.PackID);
                this.comboPackingNoFilter.SelectedIndex = -1;

                this.SetFilter(this.checkOnlyNotYetScanComplete.Checked);
            }
        }

        private void SetFilter(bool checkBox)
        {
            if ((DataTable)this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            string packID;
            if (this.comboPackingNoFilter.SelectedIndex == -1)
            {
                packID = string.Empty;
            }
            else
            {
                packID = this.comboPackingNoFilter.SelectedValue.ToString();
            }

            // 清空Quick Select CTN#欄位值
            this.txtQuickSelectCTN.Text = string.Empty;

            if (checkBox)
            {
                if (MyUtility.Check.Empty(packID))
                {
                    ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = "NotYetScan = 1";
                }
                else
                {
                    ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = string.Format("NotYetScan = 1 and ID = '{0}'", packID);
                }
            }
            else
            {
                if (MyUtility.Check.Empty(packID))
                {
                    ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = string.Format("ID = '{0}'", packID);
                }
            }
        }

        // Only not yet scan complete
        private void CheckOnlyNotYetScanComplete_CheckedChanged(object sender, EventArgs e)
        {
            this.SetFilter(this.checkOnlyNotYetScanComplete.Checked);
        }

        // Packing No Filter
        private void ComboPackingNoFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetFilter(this.checkOnlyNotYetScanComplete.Checked);
        }

        // Quick Select CTN#
        private void TxtQuickSelectCTN_TextChanged(object sender, EventArgs e)
        {
            int pos = -1;
            bool hadFound = false;
            if (!MyUtility.Check.Empty(this.txtQuickSelectCTN.Text))
            {
                foreach (DataRowView dr in this.listControlBindingSource1)
                {
                    pos++;
                    if (MyUtility.Convert.GetString(dr["CTNStartNo"]).StartsWith(this.txtQuickSelectCTN.Text))
                    {
                        hadFound = true;
                        break;
                    }
                }

                // 找完全等於的資料就可以用Find()
                // pos = listControlBindingSource1.Find("CTNStartNo", textBox2.Text);
                if (!hadFound)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    this.txtQuickSelectCTN.Text = string.Empty;
                }
                else
                {
                    this.listControlBindingSource1.Position = pos;
                }
            }
        }

        private void ControlButtonEnable(int rowIndex)
        {
            DataRow dr = this.gridDetail.GetDataRow(rowIndex);
            this.btnUncomplete.Enabled = this.canUnConfirm && MyUtility.Convert.GetString(dr["NotYetScan"]) == "0";
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Uncomplete
        private void BtnUncomplete_Click(object sender, EventArgs e)
        {
            // 問是否要做Uncomplete，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("This carton had been scanned, are you sure you want to < Uncomplete >?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            DataRow dr = this.gridDetail.GetDataRow(this.gridDetail.GetSelectedRowIndex());
            string updateCmd = string.Format("update PackingList_Detail set ScanQty = 0, Barcode = '', ScanEditDate = GETDATE() where ID = '{0}' and CTNStartNo = '{1}';", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["CTNStartNo"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Uncomplete fail!!\r\n" + result.ToString());
            }
            else
            {
                dr["ScanQty"] = "0";
                dr["Barcode"] = string.Empty;
                dr["NotYetScan"] = 1;
                this.btnUncomplete.Enabled = false;
            }
        }

        // Start to Scan
        private void BtnStartToScan_Click(object sender, EventArgs e)
        {
            if (this.gridDetail.GetTable() == null || this.gridDetail.GetTable().Rows.Count == 0)
            {
                return;
            }

            DataRow dr = this.gridDetail.GetDataRow(this.gridDetail.GetSelectedRowIndex());
            if (dr != null && MyUtility.Convert.GetString(dr["NotYetScan"]) == "0")
            {
                MyUtility.Msg.WarningBox("This carton had been scanned, so can't scan again!!");
                return;
            }

            P09_IDX_CTRL iDX = new P09_IDX_CTRL();
            if (iDX.IdxCall(1, "8:?", 4))
            {
                P09_StartToScan callNextForm = new P09_StartToScan(dr, iDX);
                DialogResult result = callNextForm.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.btnUncomplete.Enabled = this.canUnConfirm && MyUtility.Convert.GetString(dr["NotYetScan"]) == "0";
                }
            }
        }

        // 改變Grid's Row時，要檢查是否可以做Uncomplete
        private void Grid1_RowSelecting(object sender, Ict.Win.UI.DataGridViewRowSelectingEventArgs e)
        {
            this.ControlButtonEnable(e.RowIndex);
        }
    }
}
