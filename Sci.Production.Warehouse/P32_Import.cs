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

namespace Sci.Production.Warehouse
{
    public partial class P32_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtBorrow;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P32_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.textBox1.Text.TrimEnd();
            String seq = this.textBox2.Text.TrimEnd();


            if (string.IsNullOrWhiteSpace(sp) || string.IsNullOrWhiteSpace(seq))
            {
                MyUtility.Msg.WarningBox("< Return to SP# & Seq>  can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(string.Format(@"select 0 as selected ,'' id
,c.ukey as FromFtyinventoryUkey
,bd.Tomdivisionid as FromMdivisionId
,bd.ToPoid as FromPoId
,bd.ToSeq1 as FromSeq1
,bd.ToSeq2 as FromSeq2
,left(bd.ToSeq1+' ',3)+bd.ToSeq2 as fromseq
,c.dyelot as FromDyelot
,c.roll as FromRoll
,c.StockType as FromStocktype
,c.InQty - c.OutQty + c.AdjustQty balance
,0.00 as qty
,bd.FromMdivisionid as toMdivisionid
,bd.FromStocktype  as tostocktype
,bd.FromPoId as topoid
,bd.FromSeq1 as toseq1
,bd.FromSeq2 as toseq2
,c.roll as toRoll
,c.dyelot as toDyelot
,left(bd.FromSeq1+' ',3)+bd.FromSeq2 as toseq
,(select mtllocationid +',' from (select mtllocationid from ftyinventory_detail where ukey = c.ukey)t for xml path('')) as location
,dbo.getMtlDesc(bd.topoid,bd.toseq1,bd.toseq2,2,0) as [description]
,p.StockUnit
,p.FabricType
from dbo.BorrowBack_Detail as bd inner join ftyinventory c 
    on bd.topoid = c.poid and bd.toseq1 = c.seq1 and bd.toseq2 = c.seq2 and bd.tomdivisionid = c.mdivisionid
left join PO_Supp_Detail p on p.ID= bd.ToPoid and p.SEQ1 = bd.ToSeq1 and p.SEQ2 = bd.ToSeq2
where bd.id='{0}' and bd.FromPoId = '{1}' and bd.FromSeq1 = '{2}' and bd.FromSeq2 = '{3}' 
and c.lock = 0 and c.inqty-c.OutQty+c.AdjustQty > 0 and c.stocktype !='O'"
                    , dr_master["BorrowID"], sp, seq.Substring(0, 3), seq.Substring(3, 2))); // 

                MyUtility.Msg.WaitWindows("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtBorrow))
                {
                    if (dtBorrow.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtBorrow;
                    dtBorrow.DefaultView.Sort = "fromseq1,fromseq2,location,fromdyelot,balance desc";
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                MyUtility.Msg.WaitClear();
            }
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayBox1.Value = localPrice.ToString();
        }

        //Form Load
        protected override void OnFormLoaded()
        {
            string sqlcmd;
            base.OnFormLoaded();

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellMouseDoubleClick += (s, e) =>
            {
                this.textBox1.Text = ((DataTable)listControlBindingSource2.DataSource).Rows[e.RowIndex]["FromPoId"].ToString();
                this.textBox2.Text = ((DataTable)listControlBindingSource2.DataSource).Rows[e.RowIndex]["FromSeq"].ToString();
                CheckAndShowInfo(((DataTable)listControlBindingSource2.DataSource).Rows[e.RowIndex]["FromPoId"].ToString()
                    , ((DataTable)listControlBindingSource2.DataSource).Rows[e.RowIndex]["FromSeq1"].ToString()
                    , ((DataTable)listControlBindingSource2.DataSource).Rows[e.RowIndex]["FromSeq2"].ToString());
            };
            this.grid2.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                .Text("FromPoId", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13), settings: ts) //0
                .Text("fromseq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(2), settings: ts) //1
                .Text("fromseq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(2), settings: ts) //2
                .ComboBox("fromstock", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false, width: Widths.AnsiChars(6)).Get(out cbb_stocktype)    //3
                .Numeric("qty", header: "Borrow" + Environment.NewLine + "Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //4
                .Numeric("returnqty", header: "Accu." + Environment.NewLine + "Return", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //5
                .Numeric("balance", header: "Balance", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //6
               ;
            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            #region -- 依借料單號搜尋內容 --
            sqlcmd = string.Format(@";with cte1
as
(
	select bd.FromPoId,bd.FromSeq1,bd.FromSeq2,bd.FromStocktype,sum(qty)qty from borrowback_detail bd where id='{0}'
	group by bd.FromPoId,bd.FromSeq1,bd.FromSeq2,bd.FromStocktype
),
cte2
as
(
	select bd.ToPoid,bd.ToSeq1,bd.ToSeq2,bd.ToStocktype,sum(qty)qty 
	from borrowback b inner join borrowback_detail bd on b.Id= bd.ID 
	where b.BorrowId='{0}' and b.Status = 'Confirmed'
	group by bd.ToPoid,bd.ToSeq1,bd.ToSeq2,bd.ToStocktype
)
select cte1.FromPoId,cte1.FromSeq1,cte1.FromSeq2,left(cte1.FromSeq1+' ',3)+cte1.FromSeq2 as fromseq,cte1.FromStocktype,cte1.qty
,isnull(cte2.qty,0.00) as returnqty, cte1.qty - isnull(cte2.qty,0.00) as balance 
from cte1 
left join cte2 on cte2.ToPoid = cte1.FromPoId and cte2.ToSeq1 = cte1.FromSeq1 and cte2.ToSeq2 =  cte1.FromSeq2 and cte2.ToStocktype = cte1.FromStocktype;
", dr_master["BorrowId"]);
            DataTable datas;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out datas);

            listControlBindingSource2.DataSource = datas;

            #endregion

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["selected"] = true;
                        this.sum_checkedqty();
                    }
                };

            this.grid1.CellValueChanged += (s, e) =>
            {
                if (grid1.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    this.sum_checkedqty();
                }
            };

            //Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)//0
                .Text("frompoid", header: "From" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
                .Text("fromseq", header: "From" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .Text("location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(10)) //5
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) //6
                .Text("StockUnit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4))      //7
                .Text("toseq", header: "To" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //8
                .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)) //9
                .Numeric("balance", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //10
                .Numeric("qty", header: "Issue" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))  //11
                .ComboBox("Tostocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)    //12
                ;

            this.grid1.Columns[9].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[11].DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            // 全選
            checkBox1.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetCheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };

            // 全不選
            checkBox2.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetUncheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };
        }

        //Close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Import
        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder warningmsg = new StringBuilder();

            grid1.ValidateControl();
            grid1.EndEdit();

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

                if (decimal.Parse(row["balance"].ToString()) < decimal.Parse(row["qty"].ToString()))
                {
                    warningmsg.Append(string.Format(@"From SP#: {0} From Seq#: {1}-{2} From Roll#:{3} From Dyelot:{4} Issue Qty can't over Stock Qty!"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }

            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(
                    string.Format(@"fromFtyinventoryukey = {0}
                and ToMdivisionId ='{1}' and topoid = '{2}' and toseq1 = '{3}' and toseq2 = '{4}' and toroll ='{5}'and todyelot='{6}' and tostocktype='{7}' "
                    , tmp["fromFtyinventoryukey"]
                    , tmp["tomdivisionid"], tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["todyelot"], tmp["tostocktype"]));

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

        //SP# Valid
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            string seq = textBox2.Text.PadRight(5, ' ');

            if (MyUtility.Check.Empty(sp))
            {
                this.displayBox2.Value = "";
                this.displayBox3.Value = "";
                this.displayBox4.Value = "";
                this.editBox1.Text = "";
                return;
            }


            if (MyUtility.Check.Empty(textBox2.Text.TrimEnd()))
            {
                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from dbo.mdivisionpodetail where poid ='{0}' and mdivisionid='{1}')"
                    , sp, Sci.Env.User.Keyword), null))
                {
                    MyUtility.Msg.WarningBox("SP# is not found!!");
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                e.Cancel = CheckAndShowInfo(sp, seq.Substring(0, 3), seq.Substring(3, 2));
            }

        }

        private bool CheckAndShowInfo(string sp, string seq1, string seq2)
        {
            this.displayBox2.Value = "";
            this.displayBox3.Value = "";
            this.displayBox4.Value = "";
            this.editBox1.Text = "";

            DataRow tmp;
            if (!MyUtility.Check.Seek(string.Format(@"select sizespec,refno,colorid,dbo.getmtldesc(id,seq1,seq2,2,0) as [description]
                        from po_supp_detail where id ='{0}' and seq1 = '{1}' and seq2 = '{2}'", sp, seq1, seq2), out tmp, null))
            {
                MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                return true;
            }
            else
            {
                this.displayBox2.Value = tmp["sizespec"];
                this.displayBox3.Value = tmp["refno"];
                this.displayBox4.Value = tmp["colorid"];
                this.editBox1.Text = tmp["description"].ToString();
                return false;
            }
        }
        //Seq Vaild
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            if (MyUtility.Check.Empty(sp) || MyUtility.Check.Empty(textBox2.Text.TrimEnd()))
            {
                this.displayBox2.Value = "";
                this.displayBox3.Value = "";
                this.displayBox4.Value = "";
                this.editBox1.Text = "";
                return;
            }
            string seq = textBox2.Text.PadRight(5, ' ');

            e.Cancel = CheckAndShowInfo(sp, seq.Substring(0, 3), seq.Substring(3, 2));

        }
    }
}
