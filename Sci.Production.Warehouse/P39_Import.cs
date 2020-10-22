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

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P39_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable dtInventory;

        /// <inheritdoc/>
        public P39_Import(DataRow master, DataTable detail)
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

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSP.Focus();
                return;
            }
            else
            {
                #region 建立可以符合回傳的Cursor
                strSQLCmd.Append(string.Format(@"
select selected = 0
    ,id = ''
    ,PoId = LI.OrderID
    ,LI.Refno
    ,Color = LI.ThreadColorID
    ,L.Description
    ,QtyBefore = LI.InQty-LI.OutQty+LI.AdjustQty
    ,QtyAfter = 0
    ,AdjustLocalqty = 0 - (LI.InQty-LI.OutQty+LI.AdjustQty)
    ,LI.ALocation
    ,ReasonId = ''
    ,reason_nm = ''
    ,StockType = 'B'
from LocalInventory LI WITH (NOLOCK)
left join LocalItem L WITH (NOLOCK) on L.RefNo = LI.Refno
where 1=1"));
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
            }
        }

        // Form Load

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Reason Combox --
            string selectCommand = @"select Name idname,id from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
            DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                this.comboReason.DataSource = dropDownListTable;
                this.comboReason.DisplayMember = "IDName";
                this.comboReason.ValueMember = "ID";
            }
            #endregion
            #region -- Current Qty Valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                decimal currentQty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                this.gridImport.GetDataRow(e.RowIndex)["QtyAfter"] = currentQty;
                this.gridImport.GetDataRow(e.RowIndex)["AdjustLocalqty"] = currentQty - MyUtility.Convert.GetDecimal(this.gridImport.GetDataRow(e.RowIndex)["QtyBefore"]);
            };
            #endregion
            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(poitems, "ID,Name", "5,150",
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString(), "ID,Name");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> x = item.GetSelecteds();
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonId"] = x[0]["id"];
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = string.Empty;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = e.FormattedValue;
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = dr["name"];
                        }
                    }
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
                .Numeric("QtyBefore", header: "Original Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 5
                .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6)) // 6
                .Numeric("AdjustLocalqty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 7
                .Text("ALocation", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 8
                .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6)) // 9
                .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 10
               ;
            #endregion
            #region 可編輯欄位的顏色
            this.gridImport.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
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

            dr2 = dtGridBS1.Select("AdjustLocalqty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Adjust Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("ReasonId = '' and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Reason ID of selected row can't be empty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format("PoId = '{0}' and Refno = '{1}' and Color = '{2}' ", tmp["PoId"], tmp["Refno"], tmp["Color"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["Description"] = tmp["Description"];
                    findrow[0]["QtyBefore"] = tmp["QtyBefore"];
                    findrow[0]["QtyAfter"] = tmp["QtyAfter"];
                    findrow[0]["AdjustLocalqty"] = tmp["AdjustLocalqty"];
                    findrow[0]["ALocation"] = tmp["ALocation"];
                    findrow[0]["ReasonId"] = tmp["ReasonId"];
                    findrow[0]["reason_nm"] = tmp["reason_nm"];
                    findrow[0]["StockType"] = tmp["StockType"];
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

        // Update All
        private void BtnUpdateAll_Click(object sender, EventArgs e)
        {
            string reasonid = this.comboReason.SelectedValue.ToString();
            this.gridImport.ValidateControl();

            if (this.dtInventory == null || this.dtInventory.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = this.dtInventory.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["ReasonId"] = reasonid;
                item["reason_nm"] = this.comboReason.Text;
            }
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

        // Close
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
