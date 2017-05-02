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
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P31_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtBorrow;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P31_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.txtToSP.Text.TrimEnd();
            String fromSP = this.txtBorrowFromSP.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp) || txtSeq.checkEmpty(showErrMsg: false) || string.IsNullOrWhiteSpace(fromSP))
            {
                MyUtility.Msg.WarningBox("< To SP# Seq> <From SP#> can't be empty!!");
                txtToSP.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(string.Format(@"

select  selected = 0
        ,id = '' 
        ,FromRoll = c.Roll 
        ,FromDyelot = c.Dyelot 
        ,fromftyinventoryukey = c.ukey  
        ,FromPoId = a.id 
        ,FromSeq1 = a.Seq1 
        ,FromSeq2 = a.Seq2 
        ,FromFactoryID = orders.FactoryID
        ,fromseq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) 
        ,[Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) 
        ,a.stockunit
        ,fromstocktype = c.StockType 
        ,balance = c.InQty - c.OutQty + c.AdjustQty 
        ,location = dbo.Getlocation(c.ukey)
        ,toseq = concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2) 
        ,toroll = iif(toSP.Roll is not null, toSP.Roll, c.Roll)
        ,todyelot = iif(toSP.Roll is not null, toSP.Dyelot, c.Dyelot)
        ,Qty = 0.00 
        ,ToStocktype = 'B' 
        ,topoid = b.id 
        ,toseq1 = b.seq1 
        ,toseq2 = b.seq2 
        ,toFactoryID = (select FactoryID from Orders where b.id = Orders.id)
        ,a.fabrictype
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
inner join Orders on c.poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.ID
left join dbo.po_supp_detail b WITH (NOLOCK) on b.Refno = a.Refno and b.SizeSpec = a.SizeSpec and b.ColorID = a.ColorID and b.BrandId = a.BrandId
outer apply(
    select	Top 1 Roll
		    , Dyelot
    from FtyInventory
    where POID = b.id
	    and Seq1 = b.seq1
	    and Seq2 = b.seq2
        and Roll = c.Roll
) as toSP
Where a.id = '{0}' and b.id = '{1}' and b.seq1 = '{2}' and b.seq2='{3}' and Factory.MDivisionID = '{4}'
and  c.lock = 0 and c.inqty-c.outqty + c.adjustqty > 0 ", fromSP, sp, txtSeq.seq1, txtSeq.seq2, Sci.Env.User.Keyword)); // 

                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtBorrow))
                {
                    if (dtBorrow.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtBorrow;
                    dtBorrow.DefaultView.Sort = "fromseq1,fromseq2,location,fromdyelot,balance desc";
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                this.HideWaitMessage();
            }
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayTotalQty.Value = localPrice.ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["selected"] = true;
                        this.sum_checkedqty();
                    }
                };

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (gridImport.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }
                    dr.EndEdit();
                    this.sum_checkedqty();
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .Text("fromseq", header: "From" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) //4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)    //6
                .Numeric("balance", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10) //7
                .Text("location", header: "From Location", iseditingreadonly: true)      //8
                .Text("toseq", header: "To" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //9
                .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)) //10
                .Numeric("qty", header: "Issue" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns)  //11
               ;

            this.gridImport.Columns["toroll"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            StringBuilder warningmsg = new StringBuilder();

            gridImport.ValidateControl();
            gridImport.EndEdit();

            if (MyUtility.Check.Empty(dtBorrow) || dtBorrow.Rows.Count == 0) return;

            DataRow[] dr2 = dtBorrow.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtBorrow.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtBorrow.Select("qty <> 0 and Selected = 1");
            foreach (DataRow row in dr2)
            {
                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["toroll"]) || MyUtility.Check.Empty(row["todyelot"])))
                {
                    warningmsg.Append(string.Format(@"To SP#: {0} To Seq#: {1}-{2} To Roll#:{3} To Dyelot:{4} Roll and Dyelot can't be empty"
                        , row["topoid"], row["toseq1"], row["toseq2"], row["toroll"], row["todyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = "";
                    row["todyelot"] = "";
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["fromftyinventoryukey"].EqualString(tmp["fromftyinventoryukey"])
                                       && row["topoid"].EqualString(tmp["topoid"].ToString()) && row["toseq1"].EqualString(tmp["toseq1"])
                                       && row["toseq2"].EqualString(tmp["toseq2"].ToString()) && row["toroll"].EqualString(tmp["toroll"])
                                       && row["todyelot"].EqualString(tmp["todyelot"]) && row["tostocktype"].EqualString(tmp["tostocktype"])).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
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

        // To SP# Valid
        private void txtToSP_Validating(object sender, CancelEventArgs e)
        {
//            string sp = textBox1.Text.TrimEnd();

//            DataRow tmp;

//            if (MyUtility.Check.Empty(sp)) return;

//            if (txtSeq1.checkEmpty(showErrMsg: false))
//            {
//                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po_supp_detail WITH (NOLOCK) where id ='{0}')"
//                    , sp), null))
//                {
//                    MyUtility.Msg.WarningBox("SP# is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }
//            else
//            {
//                if (!MyUtility.Check.Seek(string.Format(@"select sizespec,refno,colorid,dbo.getmtldesc(id,seq1,seq2,2,0) as [description]
//                        from po_supp_detail WITH (NOLOCK) where id ='{0}' and seq1 = '{1}' and seq2 = '{2}'", sp, txtSeq1.seq1, txtSeq1.seq2), out tmp, null))
//                {
//                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//                else
//                {
//                    this.displayBox2.Value = tmp["sizespec"];
//                    this.displayBox3.Value = tmp["refno"];
//                    this.displayBox4.Value = tmp["colorid"];
//                    this.editBox1.Text = tmp["description"].ToString();
//                }
//            }

        }
    }
}
