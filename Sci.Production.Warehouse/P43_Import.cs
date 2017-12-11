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
    public partial class P43_Import : Sci.Win.Forms.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtInventory;

        public P43_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }
        //Find Now
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string seq1 = this.txtSeq1.Text.TrimEnd();
            string seq2 = this.txtSeq2.Text.TrimEnd();
            string refno = this.txtRef.Text.TrimEnd();
            string location = this.txtLocation.Text.TrimEnd();
            string FabricType = this.cmbFabric.SelectedValue.ToString().Trim();
            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!");
                txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(@"
select distinct 0 as selected    
        , '' id     
        , FTI.POID as poid
		,[Seq]= FTI.Seq1+' '+FTI.Seq2
        ,FTI.Seq1
        ,FTI.Seq2
		,FTI.Roll
		,FTI.Dyelot
        ,[Description]=Fa.Description
		,[QtyBefore]= FTI.InQty-FTI.OutQty+FTI.AdjustQty
		,[QtyAfter]= 0 
        ,[AdjustQty]= 0-(FTI.InQty-FTI.OutQty+FTI.AdjustQty)
        ,[StockUnit] = PO3.StockUnit      
        ,[location]= dbo.Getlocation(FTI.Ukey)
        , '' reasonid
        , '' reason_nm
        ,ColorID =dbo.GetColorMultipleID(PO3.BrandId, PO3.ColorID)
from FtyInventory FTI WITH (NOLOCK) 
left join PO_Supp_Detail PO3 on PO3.ID=FTI.POID 
and PO3.SEQ1=FTI.Seq1 and PO3.SEQ2=FTI.Seq2 
outer apply (
	select Description from Fabric where SCIRefno=PO3.SCIRefno
) Fa
outer apply (
	select * from dbo.SplitString(
	(select dbo.Getlocation(FTI.Ukey) 
		),',') ) lo
where 1=1 and FTI.StockType='O' ");

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(string.Format(@" 
        and FTI.Poid = '{0}' ", sp));
                }
                if (!MyUtility.Check.Empty(seq1))
                {
                    strSQLCmd.Append(string.Format(@" 
        and FTI.seq1 = '{0}' ", seq1));
                }
                if (!MyUtility.Check.Empty(seq2))
                {
                    strSQLCmd.Append(string.Format(@" 
        and FTI.seq2 = '{0}' ", seq2));
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append(string.Format(@" 
        and PO3.refno = '{0}' ", refno));
                }
             
                if (!MyUtility.Check.Empty(location))
                {
                    strSQLCmd.Append(string.Format(@" 
        and lo.Data='{0}'  ", location));
                }

                if (!MyUtility.Check.Empty(FabricType))
                {
                    //FabricType=ALL 則不需要判斷
                    if (FabricType !="ALL")
                    {
                        strSQLCmd.Append(string.Format(@" 
        and PO3.FabricType='{0}'  ", FabricType));
                    } 
                }



                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtInventory))
                {
                    if (dtInventory.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    else
                    {
                        dtInventory.DefaultView.Sort = "poid,seq,Roll,Dyelot";
                    }
                    listControlBindingSource1.DataSource = dtInventory;
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                this.HideWaitMessage();
            }
        }
        //Form Load
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- FabricType Combox--
            MyUtility.Tool.SetupCombox(cmbFabric, 3, 1, "ALL,ALL-ALL,F,Fabric,A,Accessory");
            #endregion
            #region -- Reason Combox --
            string selectCommand = @"select Name idname,id from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
            Ict.DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                comboReason.DataSource = dropDownListTable;
                comboReason.DisplayMember = "IDName";
                comboReason.ValueMember = "ID";
            }
            #endregion
            #region -- Current Qty Valid --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                decimal CurrentQty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (CurrentQty < 0)
                    {
                        MyUtility.Msg.WarningBox("Current Qty cannot less 0 !");
                        e.Cancel = true;
                        return;
                    }
                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["qtyafter"] = e.FormattedValue;
                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["AdjustQty"] = MyUtility.Convert.GetDecimal(e.FormattedValue)- MyUtility.Convert.GetDecimal(gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["QtyBefore"]);
                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["selected"] = true;
                }                
            };
            #endregion
            #region -- Reason ID 右鍵開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = "";
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }

                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(poitems
                        , "ID,Name"
                        , "5,150"
                        , gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reasonid"].ToString()
                        , "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = item.GetSelecteds();

                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reasonid"] = x[0]["id"];
                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reasonid"] = "";
                        gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reason_nm"] = "";
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(string.Format(@"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reasonid"] = e.FormattedValue;
                            gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["reason_nm"] = dr["name"];
                        }
                    }
                }
            };
            #endregion

            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   
            .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))     
            .Text("Seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(14))   
            .Text("Roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(14))               
            .Text("Dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) 
            .Numeric("QtyBefore", header: "Original Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) 
            .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))  
            .Numeric("AdjustQty", header: "Adjust Qty",iseditingreadonly:true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))  
            .Text("Location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(6))      
            .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6))   
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))    
            ;

            this.gridImport.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
        }
        //Localtion Popup
        private void txtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format(@"
select  id
        , [Description] 
from    dbo.MtlLocation WITH (NOLOCK) 
where   StockType='O'
        and junk != '1'"), "10,40", txtLocation.Text, "ID,Desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtLocation.Text = item.GetSelectedString();
        }
        //Localtion Validating
        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(txtLocation.Text.ToString())) return;
            if (!MyUtility.Check.Seek(string.Format(@"
select 1 
where exists(
    select * 
    from    dbo.MtlLocation WITH (NOLOCK) 
    where   StockType='O' 
            and id = '{0}'
            and junk != '1'
)", txtLocation.Text), null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }
        //Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

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
                DataRow[] findrow = dt_detail.Select(string.Format("Poid='{0}' and Seq1='{1}' and Seq2='{2}' and Roll='{3}'", tmp["poid"], tmp["Seq1"], tmp["Seq2"], tmp["Roll"]));
                if (findrow.Length > 0)
                {
                    findrow[0]["Dyelot"] = tmp["Dyelot"].ToString().Trim();
                    findrow[0]["Description"] = tmp["Description"].ToString().Trim();
                    findrow[0]["QtyBefore"] = tmp["QtyBefore"].ToString().Trim();
                    findrow[0]["QtyAfter"] = tmp["QtyAfter"].ToString().Trim();
                    findrow[0]["StockUnit"] = tmp["StockUnit"].ToString().Trim();
                    findrow[0]["Location"] = tmp["Location"].ToString().Trim();
                    findrow[0]["reasonid"] = tmp["reasonid"].ToString().Trim();
                    findrow[0]["reason_nm"] = tmp["reason_nm"].ToString().Trim();
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }
            this.Close();
        }
        //Update Reason All
        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            string reasonid = comboReason.SelectedValue.ToString();
            gridImport.ValidateControl();

            if (dtInventory == null || dtInventory.Rows.Count == 0) return;
            DataRow[] drfound = dtInventory.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["reasonid"] = reasonid;
                item["reason_nm"] = comboReason.Text;
            }
        }
    }
}
