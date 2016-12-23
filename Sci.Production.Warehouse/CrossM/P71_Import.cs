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
    public partial class P71_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        DataSet dsTmp;
        protected DataTable dtBorrow;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataRelation relation;
        public P71_Import(DataRow master, DataTable detail)
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

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor
                #region -- Sql Command --
                strSQLCmd.Append(string.Format(@"
;with cte as (
select '' as id,o.MDivisionID ToMdivisionid,pd.ID topoid ,pd.seq1 toseq1,pd.seq2 toseq2,left(pd.seq1+'   ',3)+pd.seq2 toseq,pd.POUnit,pd.StockUnit,pd.Qty*u.RateValue poqty
	,dbo.getMtlDesc(pd.ID,seq1,seq2,2,0) as [description]
	,o1.MDivisionID fromMdivisionid,pd.StockPOID frompoid,pd.StockSeq1 fromseq1,pd.StockSeq2 fromseq2,left(pd.stockseq1+'   ',3)+pd.stockseq2 fromseq
	,pd.Qty*u.Rate as qty
from dbo.PO_Supp_Detail pd 
inner join dbo.orders o on o.id = pd.id and o.MDivisionID ='{0}'
inner join dbo.View_Unitrate u on u.FROM_U = POUnit and u.TO_U = StockUnit
inner join dbo.Orders o1 on o1.id = pd.StockPOID and o1.MDivisionID ='{1}'
where pd.id = @poid
)
select 0 as selected ,* from cte 
", Sci.Env.User.Keyword, dr_master["MDivisionID"]));
                #endregion
                System.Data.SqlClient.SqlParameter sqlp1 = new System.Data.SqlClient.SqlParameter();
                sqlp1.ParameterName = "@poid";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
                sqlp1.Value = sp;
                paras.Add(sqlp1);

                MyUtility.Msg.WaitWindows("Data Loading....");

                if (!SQL.Selects("", strSQLCmd.ToString(), out dsTmp, paras)) { return; }
                DataTable TaipeiOutput = dsTmp.Tables[0];
                dsTmp.Tables[0].TableName = "TaipeiOutput";

                TaipeiOutputBS.DataSource = TaipeiOutput;

                //myFilter();

                MyUtility.Msg.WaitClear();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = TaipeiOutputBS;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("topoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("toseq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4)) 
                .Text("toseq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) 
                .Numeric("poqty", header: "PO Qty", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) 
                .EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(16)) 
                .Text("frompoid", header: "Inventory" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) 
                .Text("fromseq1", header: "Inventory" + Environment.NewLine + "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4)) 
                .Text("fromseq2", header: "Inventory" + Environment.NewLine + "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Numeric("qty", header: "Request" + Environment.NewLine + "Qty", integer_places: 8, decimal_places: 2, width: Widths.AnsiChars(8)) 
               ;

        }

        // Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // SP# Valid
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();

            if (MyUtility.Check.Empty(sp)) return;

            if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po_supp_detail where id ='{0}')"
                , sp), null))
            {
                MyUtility.Msg.WarningBox("SP# is not found!!");
                e.Cancel = true;
                return;
            }

        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable dt = (DataTable)TaipeiOutputBS.DataSource;
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
