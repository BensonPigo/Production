using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Warehouse
{
    public partial class P34_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtInventory;

        public P34_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.txtSPNo.Text.TrimEnd();
            String refno = this.txtRef.Text.TrimEnd();
            String location = this.txtLocation.Text.TrimEnd();
            string fabrictype = txtdropdownlistFabricType.SelectedValue.ToString();

            if (string.IsNullOrWhiteSpace(sp) && string.IsNullOrWhiteSpace(refno) && string.IsNullOrWhiteSpace(location))
            {
                MyUtility.Msg.WarningBox("< SP# > < Ref# > < Location > can't be empty!!");
                txtSPNo.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(string.Format(@"
select  0 as selected 
        , '' id
        , c.PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
        , c.Roll
        , c.Dyelot
        , c.inqty-c.outqty + c.adjustqty as QtyBefore
        , 0.00 as QtyAfter
        , dbo.Getlocation(c.ukey) as location
        , '' reasonid
        , '' reason_nm
        , a.FabricType
        , a.stockunit
        , c.stockType
        , c.ukey as ftyinventoryukey
        , ColorID =dbo.GetColorMultipleID(a.BrandId, a.ColorID)
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'I'
inner join dbo.factory f WITH (NOLOCK) on a.FactoryID=f.id
Where   c.lock = 0 
        and f.mdivisionid = '{0}'", Sci.Env.User.Keyword));

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(string.Format(@" 
        and a.id = '{0}' ", sp));
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append(string.Format(@" 
        and a.refno = '{0}' ", refno));
                }

                if (!MyUtility.Check.Empty(location))
                {
                    strSQLCmd.Append(string.Format(@" 
        and c.ukey in ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where mtllocationid = '{0}') ", location));
                }

                if (!txtSeq.checkSeq1Empty())
                {
                    strSQLCmd.Append(string.Format(@"
        and a.seq1 = '{0}'", txtSeq.seq1));
                }
                if (!txtSeq.checkSeq2Empty())
                {
                    strSQLCmd.Append(string.Format(@" 
        and a.seq2 = '{0}'", txtSeq.seq2));
                }

                switch (fabrictype)
                {
                    case "ALL":
                        break;
                    case "F":
                        strSQLCmd.Append(@" 
        And a.fabrictype = 'F'");
                        break;
                    case "A":
                        strSQLCmd.Append(@" 
        And a.fabrictype = 'A'");
                        break;
                }


                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtInventory))
                {
                    if (dtInventory.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    else
                    {
                        dtInventory.Columns.Add("adjustqty", typeof(decimal));
                        dtInventory.Columns["adjustqty"].Expression = "qtyafter - qtybefore";
                        dtInventory.DefaultView.Sort = "seq1,seq2,location,dyelot";
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
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["qtyafter"] = e.FormattedValue;
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
            .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6))
            .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20))
            .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6))
            .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("QtyBefore", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
            .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))
            .Numeric("adjustqty", header: "Adjust Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
            .Text("location", header: "Bulk Location", iseditingreadonly: true, width: Widths.AnsiChars(6))
            .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6))
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
            ;

            this.gridImport.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
        }
        //Close
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

            dr2 = dtGridBS1.Select("adjustqty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Adjust Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("reasonid = '' and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Reason ID of selected row can't be empty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("adjustqty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("ftyinventoryukey = {0}", tmp["ftyinventoryukey"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["qtybefore"] = tmp["qtybefore"];
                    findrow[0]["qtyafter"] = tmp["qtyafter"];
                    findrow[0]["adjustqty"] = tmp["adjustqty"];
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

        //SP# Valid
        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
//            string sp = textBox1.Text.TrimEnd();

//            if (MyUtility.Check.Empty(sp)) return;

//            if (txtSeq1.checkEmpty(showErrMsg: false))
//            {
//                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from MdivisionPoDetail WITH (NOLOCK) where poid ='{0}')",sp), null))
//                {
//                    MyUtility.Msg.WarningBox("SP# is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }
//            else
//            {
//                if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from MdivisionPoDetail WITH (NOLOCK) where poid ='{0}' 
//                        and seq1 = '{1}' and seq2 = '{2}')", sp, txtSeq1.seq1, txtSeq1.seq2), null))
//                {
//                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }

        }

        //Update All
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

        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (txtLocation.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"
select 1 
where exists(
    select * 
    from    dbo.MtlLocation WITH (NOLOCK) 
    where   StockType='I' 
            and id = '{0}'
            and junk != '1'
)", txtLocation.Text), null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }
        //Location  右鍵
        private void txtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format(@"
select  id
        , [Description] 
from    dbo.MtlLocation WITH (NOLOCK) 
where   StockType='I'
        and junk != '1'"), "10,40", txtLocation.Text, "ID,Desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtLocation.Text = item.GetSelectedString();
        }
    }
}
