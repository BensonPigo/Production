using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P27_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtInventory;

        public P27_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSP.Text.TrimEnd();
            string refno = this.txtRef.Text.TrimEnd();
            string color = this.txtColor.Text.TrimEnd();
            string location = this.txtLocation.Text.TrimEnd();
            string stockType = MyUtility.Convert.GetString(this.dr_master["StockType"]);

            #region 判斷畫面條件, 至少其中一個有填
            if (string.IsNullOrWhiteSpace(sp) && string.IsNullOrWhiteSpace(refno) && string.IsNullOrWhiteSpace(color) && string.IsNullOrWhiteSpace(location))
            {
                MyUtility.Msg.WarningBox("<SP#> <Ref#> <Color> <Location> can’t be all empty!");
                this.txtSP.Focus();
                return;
            }
            #endregion

            #region  query sql command
            strSQLCmd.Append(string.Format(
                @"
select selected = 0
    ,id = ''
    ,PoId = LI.OrderID
    ,LI.Refno
    ,Color = LI.ThreadColorID
    ,L.Description
    ,Qty = case when '{0}' = 'B'then LI.InQty-LI.OutQty+LI.AdjustQty
				when '{0}' = 'O'then LI.LobQty
		   End
	,FromLocation = case when '{0}' = 'B'then LI.ALocation
						 when '{0}' = 'O'then LI.CLocation
					End
	,ToLocation = ''
from LocalInventory LI WITH (NOLOCK)
left join LocalItem L WITH (NOLOCK) on L.RefNo = LI.Refno
where (case when '{0}' = 'B'then LI.InQty-LI.OutQty+LI.AdjustQty 
		    when '{0}' = 'O'then LI.LobQty	End)>0", stockType));
            #endregion

            #region sql搜尋條件
            if (!MyUtility.Check.Empty(sp))
            {
                strSQLCmd.Append(string.Format(@" and LI.OrderID = '{0}' ", sp));
            }

            if (!MyUtility.Check.Empty(refno))
            {
                strSQLCmd.Append(string.Format(@" and LI.Refno = '{0}' ", refno));
            }

            if (!MyUtility.Check.Empty(color))
            {
                strSQLCmd.Append(string.Format(@" and LI.ThreadColorID = '{0}' ", color));
            }

            if (!MyUtility.Check.Empty(location))
            {
                strSQLCmd.Append(string.Format(@" and '{0}' in (select data from dbo.SplitString(LI.ALocation,','))", location));
            }
            #endregion

            #region Execute
            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtInventory))
            {
                if (this.dtInventory.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtInventory;
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }

            this.HideWaitMessage();
            #endregion
        }

        // Form Load

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region -- ToLocation 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = string.Format(@"
select id
       , [Description] 
from   dbo.MtlLocation WITH (NOLOCK) 
where  StockType='O' and junk != '1'");

                    DataRow dr = this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex());
                    Win.Tools.SelectItem2 selectSubcons = new Win.Tools.SelectItem2(sqlcmd, "ID,Desc", "13,30", dr["ToLocation"].ToString(), null, null, null);

                    DialogResult result = selectSubcons.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ToLocation"] = selectSubcons.GetSelectedString();

                    if (!MyUtility.Check.Empty(dr["ToLocation"]))
                    {
                        dr["ToLocation"] = string.Join(",", selectSubcons.GetSelectedList().ToArray());
                    }
                    else
                    {
                        dr["ToLocation"] = string.Empty;
                    }

                    selectSubcons.Empty();
                }
            };
            #endregion
            #region -- 欄位設定 --
            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("PoId", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) // 1
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(18)) // 2
                .Text("Color", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 3
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 4
                .Numeric("Qty", header: "Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 5
                .EditText("FromLocation", header: "FromLocation", iseditingreadonly: true, width: Widths.AnsiChars(10)) // 6
                .EditText("ToLocation", header: "ToLocation", iseditingreadonly: true, width: Widths.AnsiChars(20), settings: ts) // 7
               ;
            #endregion
            #region 可編輯欄位的顏色
            this.gridImport.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format("PoId = '{0}' and Refno = '{1}' and Color = '{2}' ", tmp["PoId"], tmp["Refno"], tmp["Color"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["Description"] = tmp["Description"];
                    findrow[0]["Qty"] = tmp["Qty"];
                    findrow[0]["FromLocation"] = tmp["FromLocation"];
                    findrow[0]["ToLocation"] = tmp["ToLocation"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        private void TxtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtLocation.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                @"
select 1 
where exists(
    select * 
    from    dbo.MtlLocation WITH (NOLOCK) 
    where   StockType='B' 
            and id = '{0}'
            and junk != '1'
)", this.txtLocation.Text), null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }

        // Location  右鍵
        private void TxtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                string.Format(@"
select  id
        , [Description] 
from    dbo.MtlLocation WITH (NOLOCK) 
where   StockType='B'
        and junk != '1'"), "13,50", this.txtLocation.Text, "ID,Desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocation.Text = item.GetSelectedString();
        }

        private void TxtToLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string sqlcmd = string.Format(@"
select id
       , [Description] 
from   dbo.MtlLocation WITH (NOLOCK) 
where  StockType='B' and junk != '1'");

            Win.Tools.SelectItem2 selectSubcons = new Win.Tools.SelectItem2(sqlcmd, "ID,Desc", "13,30", this.txtToLocation.Text, null, null, null);

            DialogResult result = selectSubcons.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtToLocation.Text = selectSubcons.GetSelectedString();
            if (!MyUtility.Check.Empty(this.txtToLocation.Text))
            {
                this.txtToLocation.Text = string.Join(",", selectSubcons.GetSelectedList().ToArray());
            }
            else
            {
                this.txtToLocation.Text = string.Empty;
            }

            selectSubcons.Empty();
        }

        private void BtnUpdateAll_Click(object sender, EventArgs e)
        {
            string toLocation = this.txtToLocation.Text;

            if (this.dtInventory == null || this.dtInventory.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = this.dtInventory.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["ToLocation"] = toLocation;
            }
        }

        // Close
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
