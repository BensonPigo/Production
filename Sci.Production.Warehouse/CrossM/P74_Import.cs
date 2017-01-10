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
        protected DataTable dtBorrow;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
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
            String seq = this.textBox2.Text.Trim();
            String fromSP = this.textBox3.Text.TrimEnd();
            if (seq.Length!=5)
            {
                MyUtility.Msg.WarningBox("Seq need enter 00 00");
                return;
            }
            string seq1 = seq.Substring(0, 2).Trim();
            string seq2 = seq.Substring(3, 2).Trim();

            if (string.IsNullOrWhiteSpace(sp) || string.IsNullOrWhiteSpace(seq) || string.IsNullOrWhiteSpace(fromSP))
            {
                MyUtility.Msg.WarningBox("< To SP# Seq> <From SP#> can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                //建立表頭
                string headSql = string.Format(@"select 
SizeSpec, Refno, ColorID,dbo.getmtldesc(id,seq1,seq2,2,0) as [Description]
from PO_Supp_Detail 
where id = (SELECT distinct poid FROM Orders where poid='{0}' and MDivisionID='{3}') 
and seq1 = '{1}' and seq2 = '{2}'", sp, seq1, seq2, Sci.Env.User.Keyword);

                // 建立可以符合回傳的Cursor
                strSQLCmd.Append(string.Format(@"
                    select 0 as selected ,'' id
                    ,'{5}' as FromMdivisionid
                    , a.id as FromPoId
                    ,a.Seq1 as FromSeq1
                    ,a.Seq2 as FromSeq2
                    ,left(a.seq1+' ',3)+a.Seq2 as fromseq
                    ,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
                    ,a.usedqty as poqty
                    ,iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
                        ff.UsageUnit , 
                        iif(mm.IsExtensionUnit > 0 , 
                            iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
                                ff.UsageUnit , 
                                uu.ExtensionUnit), 
                            ff.UsageUnit)) as StockUnit
                    --,a.stockunit
                    ,left(b.seq1+' ',3)+b.Seq2 as toseq
                    ,0.00 as Qty
                    ,'B' ToStocktype
                    ,'{4}' as tomdivisionid
                    ,b.id topoid
                    ,b.seq1 toseq1
                    ,b.seq2 toseq2
                    ,a.ColorID,a.Refno,a.SizeSpec,a.Remark
                    from dbo.PO_Supp_Detail a 
                    left join dbo.po_supp_detail b on b.Refno = a.Refno and b.SizeSpec = a.SizeSpec 
                        and b.ColorID = a.ColorID and b.BrandId = a.BrandId
                    inner join [dbo].[Fabric] ff on a.SCIRefno= ff.SCIRefno
                    inner join [dbo].[MtlType] mm on mm.ID = ff.MtlTypeID
                    inner join [dbo].[Unit] uu on ff.UsageUnit = uu.ID
                    inner join View_unitrate v on v.FROM_U = a.POUnit
	                    and v.TO_U = (
	                    iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
		                    ff.UsageUnit , 
		                    iif(mm.IsExtensionUnit > 0 , 
			                    iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
				                    ff.UsageUnit , 
				                    uu.ExtensionUnit), 
			                    ff.UsageUnit)))--b.StockUnit
                    Where a.id = (SELECT distinct poid FROM Orders where poid='{0}' and MDivisionID='{5}')
                    and b.id = (SELECT distinct poid FROM Orders where poid='{1}' 
                    and MDivisionID='{4}') and b.seq1 = '{2}' and b.seq2='{3}'"
                , fromSP, sp, seq1, seq2, Sci.Env.User.Keyword, dr_master["mdivisionid"])); // 

                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtBorrow))
                {
                    if (dtBorrow.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    BorrowItemBS.DataSource = dtBorrow;
                    dtBorrow.DefaultView.Sort = "topoid,toseq1,toseq2";

                    DataTable headDt;
                    if (!(result = DBProxy.Current.Select(null, headSql, out headDt)) | headDt.Rows.Count == 0 | dtBorrow.Rows.Count == 0)
                    {
                        displayBoxSizeSpec.Text = "";
                        displayBoxRefno.Text = "";
                        displayBoxColor.Text = "";
                        editBox1.Text = "";
                    }
                    else
                    {
                        displayBoxSizeSpec.Text = headDt.Rows[0].GetValue("SizeSpec").ToString();
                        displayBoxRefno.Text = headDt.Rows[0].GetValue("Refno").ToString();
                        displayBoxColor.Text = headDt.Rows[0].GetValue("ColorID").ToString();
                        editBox1.Text = headDt.Rows[0].GetValue("Description").ToString();
                    }
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                this.HideWaitMessage();               
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = BorrowItemBS;

            #region Qty Validating
            Ict.Win.DataGridViewGeneratorNumericColumnSettings qtyNS = new DataGridViewGeneratorNumericColumnSettings();
            qtyNS.CellValidating += (s, e) =>
            {
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                dr["qty"] = e.FormattedValue;
                dr["Selected"] = ((decimal)e.FormattedValue == 0) ? 0 : 1;      
            };
            #endregion

            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("topoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("toseq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4)) 
                .Text("toseq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) 
                .Numeric("poqty", header: "PO Qty", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) 
                //.EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(16)) 
                .Text("frompoid", header: "From" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("fromseq1", header: "From" + Environment.NewLine + "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4))
                .Text("fromseq2", header: "From" + Environment.NewLine + "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Numeric("qty", header: "Borrow" + Environment.NewLine + "Qty", integer_places: 8, decimal_places: 2, width: Widths.AnsiChars(8), settings: qtyNS) 
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
                DataRow[] findrow = dt_detail.Select(string.Format(@"tomdivisionid = '{0}' and topoid = '{1}'  and toseq1 = '{2}' and toseq2 = '{3}' 
                    and frommdivisionid = '{4}' and frompoid = '{5}' and fromseq1 = '{6}' and fromseq2 = '{7}'"
                    , tmp["toMdivisionid"], tmp["topoid"], tmp["toseq1"], tmp["toseq2"]
                    , tmp["FromMdivisionid"], tmp["Frompoid"], tmp["Fromseq1"], tmp["Fromseq2"]));

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

        //表頭
        private void grid1_SelectionChanged(object sender, EventArgs e)
        {
            if (dtBorrow.Rows.Count > 0 &&  !MyUtility.Check.Empty(grid1.CurrentCell))
            {
                var cRow = ((DataRowView)this.grid1.CurrentRow.DataBoundItem).Row;
                string s1 = cRow["fromseq1"].ToString();
                string s2 = cRow["fromseq2"].ToString();

                DataRow[] dra = dtBorrow.Select(string.Format("fromseq1={0} and fromseq2={1}", s1, s2));
                displayBoxSizeSpec.Text = dra[0]["SizeSpec"].ToString();
                displayBoxRefno.Text = dra[0]["Refno"].ToString();
                displayBoxColor.Text = dra[0]["ColorID"].ToString();
                editBox1.Text = dra[0]["description"].ToString();
            }
        }
    }
}
