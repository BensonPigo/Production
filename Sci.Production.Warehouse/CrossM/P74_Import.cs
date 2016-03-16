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
    public partial class P74_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        DataSet dsTmp;
        protected DataTable dtBorrow;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataRelation relation;
        public P74_Import(DataRow master, DataTable detail)
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
            String seq = this.textBox2.Text.TrimEnd();
            String fromSP = this.textBox3.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp) || string.IsNullOrWhiteSpace(seq) || string.IsNullOrWhiteSpace(fromSP))
            {
                MyUtility.Msg.WarningBox("< To SP# Seq> <From SP#> can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(string.Format(@"select 0 as selected ,'' id
,'{5}' as FromMdivisionid
, a.id as FromPoId
,a.Seq1 as FromSeq1
,a.Seq2 as FromSeq2
,left(a.seq1+' ',3)+a.Seq2 as fromseq
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
,a.stockunit
,left(b.seq1+' ',3)+b.Seq2 as toseq
,0.00 as Qty
,'B' ToStocktype
,'{4}' as tomdivisionid
,b.id topoid
,b.seq1 toseq1
,b.seq2 toseq2
from dbo.PO_Supp_Detail a 
left join dbo.po_supp_detail b on b.Refno = a.Refno and b.SizeSpec = a.SizeSpec 
    and b.ColorID = a.ColorID and b.BrandId = a.BrandId
Where a.id = '{0}' and b.id = '{1}' and b.seq1 = '{2}' and b.seq2='{3}'"
                    , fromSP, sp, seq.Substring(0, 3), seq.Substring(3, 2), Sci.Env.User.Keyword, dr_master["mdivisionid"])); // 

                MyUtility.Msg.WaitWindows("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtBorrow))
                {
                    if (dtBorrow.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    BorrowItemBS.DataSource = dtBorrow;
                    dtBorrow.DefaultView.Sort = "topoid,toseq1,toseq2";
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                MyUtility.Msg.WaitClear();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = BorrowItemBS;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("topoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("toseq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4)) 
                .Text("toseq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) 
                .Numeric("poqty", header: "PO Qty", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) 
                .EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(16)) 
                .Text("frompoid", header: "From" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("fromseq1", header: "From" + Environment.NewLine + "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4))
                .Text("fromseq2", header: "From" + Environment.NewLine + "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Numeric("qty", header: "Borrow" + Environment.NewLine + "Qty", integer_places: 8, decimal_places: 2, width: Widths.AnsiChars(8)) 
               ;

        }

        // Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // SP# Valid

        private void btn_Import_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable dt = (DataTable)BorrowItemBS.DataSource;
            DataRow[] dr2 = dt.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dt.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dt.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format(@"tomdivisionid = '{0}' and topoid = '{1}' and toseq1 = '{2}' and toseq2 = '{3}' 
                        and frommdivisionid = '{4}' and frompoid = '{5}' and fromseq1 = '{6}' and fromseq2 = '{7}'"
                    , tmp["toMdivisionid"], tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["FromMdivisionid"], tmp["Frompoid"], tmp["Fromseq1"], tmp["Fromseq2"]));

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

    }
}
