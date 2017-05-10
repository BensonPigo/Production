using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    public partial class P09 : Sci.Win.Tems.QueryForm
    {        
        protected bool canUnConfirm;
        protected DataTable PackID;
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            checkOnlyNotYetScanComplete.Checked = true;
            canUnConfirm = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P09. Scan && Pack", "CanUnConfirm");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            txtcountryDestination.TextBox1.ReadOnly = true;
            btnUncomplete.Enabled = false;
            btnStartToScan.Enabled = false;

            //Grid設定
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridDetail)
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

            //按Header沒有排序功能
            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //SP#
        private void txtSP_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(txtSP.Text) && txtSP.OldValue != txtSP.Text)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", txtSP.Text);
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
                        MyUtility.Msg.WarningBox("Sql connection fail.\r\n"+result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("SP# not found!!");
                    }
                    txtSP.Text = "";
                    //清除所有欄位資料
                    displayStyle.Value = "";
                    displaySeason.Value = "";
                    displayBrand.Value = "";
                    displayBuyer.Value = "";
                    displayM.Value = "";
                    displayOrder.Value = "";
                    displayPONo.Value = "";
                    displayFactory.Value = "";
                    txtcountryDestination.TextBox1.Text = "";
                    numOrderQty.Value = 0;
                    if (PackID != null)
                    {
                        PackID.Clear();
                        MyUtility.Tool.SetupCombox(comboPackingNoFilter, 1, PackID);
                        comboPackingNoFilter.SelectedIndex = -1;
                    }
                    txtQuickSelectCTN.Text = "";
                    btnUncomplete.Enabled = false;
                    btnStartToScan.Enabled = false;
                    if ((DataTable)listControlBindingSource1.DataSource != null)
                    {
                        ((DataTable)listControlBindingSource1.DataSource).Clear();
                    }
                    return;
                }
            }
        }

        //SP#
        private void txtSP_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(txtSP.Text) && (txtSP.OldValue != txtSP.Text))
            {
                DataRow OrdersData;
                string sqlCmd = string.Format("select StyleID,SeasonID,BrandID,Dest,Qty,Customize1,CustPONo,FtyGroup,isnull((select BuyerID from Brand WITH (NOLOCK) where ID =  Orders.BrandID),'') as BuyerID from Orders WITH (NOLOCK) where ID = '{0}'", txtSP.Text);
                MyUtility.Check.Seek(sqlCmd, out OrdersData);
                displayStyle.Value = MyUtility.Convert.GetString(OrdersData["StyleID"]);
                displaySeason.Value = MyUtility.Convert.GetString(OrdersData["SeasonID"]);
                displayBrand.Value = MyUtility.Convert.GetString(OrdersData["BrandID"]);
                displayBuyer.Value = MyUtility.Convert.GetString(OrdersData["BuyerID"]);
                displayM.Value = Sci.Env.User.Keyword;
                displayOrder.Value = MyUtility.Convert.GetString(OrdersData["Customize1"]);
                displayPONo.Value = MyUtility.Convert.GetString(OrdersData["CustPONo"]);
                displayFactory.Value = MyUtility.Convert.GetString(OrdersData["FtyGroup"]);
                txtcountryDestination.TextBox1.Text = MyUtility.Convert.GetString(OrdersData["Dest"]);
                numOrderQty.Value = MyUtility.Convert.GetInt(OrdersData["Qty"]);

                sqlCmd = string.Format(@"select ID,CTNStartNo,ShipModeID,ShipQty,NotYetScan,
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
order by ID,Seq", txtSP.Text);
                DataTable gridData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Sql connection fail.\r\n"+result.ToString());
                }
                listControlBindingSource1.DataSource = gridData;


                sqlCmd = string.Format("select distinct ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}'", txtSP.Text);
                result = DBProxy.Current.Select(null, sqlCmd, out PackID);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Query Packing ID fail.\r\n" + result.ToString());
                }

                //當Grid有資料時Start to scan按鈕才可以按
                if (gridData == null)
                {
                    btnStartToScan.Enabled = false;
                }
                else
                {
                    btnStartToScan.Enabled = true;
                }
                MyUtility.Tool.SetupCombox(comboPackingNoFilter, 1, PackID);
                comboPackingNoFilter.SelectedIndex = -1;

                SetFilter(checkOnlyNotYetScanComplete.Checked);
            }
        }

        private void SetFilter(bool checkBox)
        {
            if ((DataTable)listControlBindingSource1.DataSource == null)
            {
                return;
            }
            
            string packID;
            if (comboPackingNoFilter.SelectedIndex == -1)
            {
                packID = "";
            }
            else
            {
                packID = comboPackingNoFilter.SelectedValue.ToString();
            }

            //清空Quick Select CTN#欄位值
            txtQuickSelectCTN.Text = "";
            
            if (checkBox)
            {
                if (MyUtility.Check.Empty(packID))
                {
                    ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = "NotYetScan = 1";
                }
                else
                {
                    ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = string.Format("NotYetScan = 1 and ID = '{0}'", packID);
                }
            }
            else
            {
                if (MyUtility.Check.Empty(packID))
                {
                    ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = "";
                }
                else
                {
                    ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = string.Format("ID = '{0}'", packID);
                }
            }
        }

        //Only not yet scan complete
        private void checkOnlyNotYetScanComplete_CheckedChanged(object sender, EventArgs e)
        {
            SetFilter(checkOnlyNotYetScanComplete.Checked);

        }

        //Packing No Filter
        private void comboPackingNoFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilter(checkOnlyNotYetScanComplete.Checked);
        }

        //Quick Select CTN#
        private void txtQuickSelectCTN_TextChanged(object sender, EventArgs e)
        {
            int pos = -1;
            bool hadFound = false;
            if (!MyUtility.Check.Empty(txtQuickSelectCTN.Text))
            {
                foreach (DataRowView dr in listControlBindingSource1)
                {
                    pos++;
                    if (MyUtility.Convert.GetString(dr["CTNStartNo"]).StartsWith(txtQuickSelectCTN.Text))
                    {
                        hadFound = true;
                        break;
                    }
                }

                //找完全等於的資料就可以用Find()
                //pos = listControlBindingSource1.Find("CTNStartNo", textBox2.Text);
                if (!hadFound)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    txtQuickSelectCTN.Text = "";
                }
                else
                {
                    this.listControlBindingSource1.Position = pos;
                }
            }
        }

        private void ControlButtonEnable(int rowIndex)
        {
            DataRow dr = gridDetail.GetDataRow(rowIndex);
            btnUncomplete.Enabled = canUnConfirm && MyUtility.Convert.GetString(dr["NotYetScan"]) == "0";
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Uncomplete
        private void btnUncomplete_Click(object sender, EventArgs e)
        {
            //問是否要做Uncomplete，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("This carton had been scanned, are you sure you want to < Uncomplete >?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            DataRow dr = gridDetail.GetDataRow(gridDetail.GetSelectedRowIndex());
            string updateCmd = string.Format("update PackingList_Detail set ScanQty = 0, Barcode = '', ScanEditDate = GETDATE() where ID = '{0}' and CTNStartNo = '{1}';",MyUtility.Convert.GetString(dr["ID"]),MyUtility.Convert.GetString(dr["CTNStartNo"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Uncomplete fail!!\r\n" + result.ToString());
            }
            else
            {
                dr["ScanQty"] = "0";
                dr["Barcode"] = "";
                dr["NotYetScan"] = 1;
            }
        }
        
        //Start to Scan
        private void btnStartToScan_Click(object sender, EventArgs e)
        {
            if (gridDetail.GetTable() == null || gridDetail.GetTable().Rows.Count == 0)
            {
                return;
            }
            DataRow dr = gridDetail.GetDataRow(gridDetail.GetSelectedRowIndex());            
            if (dr != null && MyUtility.Convert.GetString(dr["NotYetScan"]) == "0")
            {
                MyUtility.Msg.WarningBox("This carton had been scanned, so can't scan again!!");
                return;
            }
            P09_IDX_CTRL IDX = new P09_IDX_CTRL();
            IDX.IdxCall(1, "8:?", 4);
            Sci.Production.Packing.P09_StartToScan callNextForm = new Sci.Production.Packing.P09_StartToScan(dr);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
            }
        }

        //改變Grid's Row時，要檢查是否可以做Uncomplete
        private void grid1_RowSelecting(object sender, Ict.Win.UI.DataGridViewRowSelectingEventArgs e)
        {
            ControlButtonEnable(e.RowIndex);
        }
    }
}
