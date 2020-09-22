using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P46_Import : Win.Forms.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtInventory;

        public P46_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string refno = this.txtRef.Text.TrimEnd();
            string color = this.txtColor.Text.TrimEnd();
            string location = this.txtLocation.Text.TrimEnd();
            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                strSQLCmd.Append(@"
select distinct 0 as selected    
        , '' id     
        , Linv.OrderID as poid
		, Linv.Refno
		, Linv.ThreadColorID as Color
         , [Description]=Li.Description
		, Linv.LobQty as QtyBefore
		, 0 as QtyAfter
        , 0 as RemoveQty        
        , Linv.CLocation as CLocation
        , '' reasonid
        , '' reason_nm        
from LocalInventory Linv WITH (NOLOCK) 
outer apply(select Description from LocalItem where refno=Linv.Refno) Li 
outer apply (
	select * from dbo.SplitString(
	(select CLocation from LocalInventory 
	where OrderID=Linv.OrderID  
	and refno=Linv.Refno and ThreadColorID=Linv.ThreadColorID),',') ) lo
where 1=1 and Linv.LobQty>0 ");

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and Linv.OrderID = '{0}' ", sp));
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and Linv.refno = '{0}' ", refno));
                }

                if (!MyUtility.Check.Empty(color))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and Linv.ThreadColorID = '{0}' ", color));
                }

                if (!MyUtility.Check.Empty(location))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and lo.Data='{0}'  ", location));
                }

                this.ShowWaitMessage("Data Loading....");
                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtInventory))
                {
                    if (this.dtInventory.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }
                    else
                    {
                        this.dtInventory.DefaultView.Sort = "poid,Refno,color";
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
            string selectCommand = @"select Name idname,id from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
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
                decimal currentQty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (currentQty >= MyUtility.Convert.GetDecimal(this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["QtyBefore"]))
                    {
                        MyUtility.Msg.WarningBox("Current Qty cannot >= Original Qty!");
                        e.Cancel = true;
                        return;
                    }

                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["qtyafter"] = e.FormattedValue;
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["RemoveQty"] = MyUtility.Convert.GetDecimal(this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue);
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["selected"] = true;
                }
            };
            #endregion
            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        poitems,
                        "ID,Name",
                        "5,150",
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString(),
                        "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = x[0]["id"];
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
and ReasonTypeID='Stock_Remove' AND junk = 0", e.FormattedValue), out dr, null))
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

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) // 1
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 4
                .Text("Color", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(10)) // 4
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 3
                .Numeric("QtyBefore", header: "Original Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 6
                .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6)) // 7
                .Numeric("RemoveQty", header: "Remove Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 8
                .Text("CLocation", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 9
                .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6)) // 10
                .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 11
               ;

            this.gridImport.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Localtion Popup
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
where   StockType='O'
        and junk != '1'"), "10,40", this.txtLocation.Text, "ID,Desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocation.Text = item.GetSelectedString();
        }

        // Localtion Validating
        private void TxtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtLocation.Text.ToString()))
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
    where   StockType='O' 
            and id = '{0}'
            and junk != '1'
)", this.txtLocation.Text), null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }

        // Cancel
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

            dr2 = dtGridBS1.Select("reasonid = '' and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Reason ID of selected row can't be empty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("reasonid <> '' and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format("Poid='{0}' and Refno='{1}' and Color='{2}'", tmp["poid"], tmp["Refno"], tmp["Color"]));
                if (findrow.Length > 0)
                {
                    findrow[0]["Description"] = tmp["Description"].ToString().Trim();
                    findrow[0]["QtyBefore"] = tmp["QtyBefore"].ToString().Trim();
                    findrow[0]["QtyAfter"] = tmp["QtyAfter"].ToString().Trim();
                    findrow[0]["CLocation"] = tmp["CLocation"].ToString().Trim();
                    findrow[0]["reason_nm"] = tmp["reason_nm"].ToString().Trim();
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

        // Update Reason All
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
                item["reasonid"] = reasonid;
                item["reason_nm"] = this.comboReason.Text;
            }
        }
    }
}
