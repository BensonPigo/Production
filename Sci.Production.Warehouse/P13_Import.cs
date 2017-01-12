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
    public partial class P13_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtArtwork;

        public P13_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.textBox1.Text.TrimEnd();
            string seq1 = txtSeq1.seq1;
            string seq2 = txtSeq1.seq2;
            string seq = seq1 + " " + seq2;

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(string.Format(@"select 0 as selected ,'' id, c.mdivisionid,a.id as PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.FabricType
,a.stockunit
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
,c.Roll
,c.Dyelot
,0.00 as Qty
,'B' StockType
,c.ukey as ftyinventoryukey
,stuff((select ',' + cast(mtllocationid as varchar) from (select mtllocationid from ftyinventory_detail where ukey = c.ukey)t for xml path('')), 1, 1, '') as location
,c.inqty-c.outqty + c.adjustqty as balance
from dbo.PO_Supp_Detail a 
inner join dbo.ftyinventory c on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B'
Where a.id = '{0}' and c.lock = 0 and c.inqty-c.outqty + c.adjustqty > 0 and c.mdivisionid='{1}'", sp, Sci.Env.User.Keyword)); // 
                if (!MyUtility.Check.Empty(seq))
                {
                    strSQLCmd.Append(string.Format(@" and a.seq1 = '{0}' and a.seq2='{1}'", seq1, seq2));
                }

                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtArtwork;
                    dtArtwork.DefaultView.Sort = "seq1,seq2,location,dyelot,balance desc";
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
            this.displayBox1.Value = localPrice.ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

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

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //2
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .Numeric("balance", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10) //6
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns)  //7
               .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)); //8

            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll ='{3}'and dyelot='{4}'"
                    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

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

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            string seq1 = txtSeq1.seq1;
            string seq2 = txtSeq1.seq2;
            string seq = seq1 + " " + seq2;

            if (MyUtility.Check.Empty(sp)) return;

            if (MyUtility.Check.Empty(seq))
            {
                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po_supp_detail where id ='{0}')"
                    , sp), null))
                {
                    MyUtility.Msg.WarningBox("SP# is not found!!");
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from po_supp_detail where id ='{0}' 
                        and seq1 = '{1}' and seq2 = '{2}')", sp, seq1, seq2), null))
                {
                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                    e.Cancel = true;
                    return;
                }
            }

        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            if (MyUtility.Check.Empty(sp) || MyUtility.Check.Empty(txtSeq1.getSeq())) return;

            string seq1 = txtSeq1.seq1;
            string seq2 = txtSeq1.seq2;

            if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from po_supp_detail where id ='{0}' 
                        and seq1 = '{1}' and seq2 = '{2}')", sp, seq1, seq2), null))
            {
                MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                e.Cancel = true;
                return;
            }

        }
    }
}
