using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P42 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;

        }

        protected override void OnFormLoaded()
        {
            
            base.OnFormLoaded();
            this.checkBox1.Checked = true;
            this.checkBox2.Checked = true;
            dateBox1.Value = DateTime.Now;
            dateBox2.Value = DateTime.Now;
            grid1.RowPostPaint += (s, e) =>
            {
                //DataGridViewRow dvr = detailgrid.Rows[e.RowIndex];
                //DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                if (grid1.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                int i = e.RowIndex;
                if (MyUtility.Check.Empty(dr["EachConsApv"]))
                {
                    grid1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 192);
                }
            };

            MyUtility.Tool.SetGridFrozen(this.grid1);

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            ts1.CellMouseDoubleClick += (s, e) =>
            {
                DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                string colnm = this.grid1.Columns[e.ColumnIndex].DataPropertyName;
                string expression = colnm + " = '" + ddr[colnm].ToString().TrimEnd() + "'";
                DataRow[] drfound = dt.Select(expression);

                foreach (var item in drfound)
                {
                    item["selected"] = true;
                }
            };

            this.grid1.CellValueChanged += (s, e) =>
            {
                if (grid1.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    this.sum_checkedqty();
                }
            };
            Ict.Win.DataGridViewGeneratorDateColumnSettings ts2 = new DataGridViewGeneratorDateColumnSettings();
            ts2.CellValidating += (s, e) =>
            {

                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["tapeoffline"])) return;
                    if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["tapeoffline"]) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Inline can't be late than Offline!!");
                    }
                }
            };

            Ict.Win.DataGridViewGeneratorDateColumnSettings ts3 = new DataGridViewGeneratorDateColumnSettings();
            ts3.CellValidating += (s, e) =>
            {

                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["tapeinline"])) return;
                    if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["tapeinline"]) < 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Offline can't be earlier than inline!!");
                    }
                }
            };

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("MdivisionID", header: "M", width: Widths.AnsiChars(5), settings: ts1, iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                 .Text("EachConsApv", header: "Each Cons.", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Text("ETA", header: "Mtl. ETA", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Text("fstSewinline", header: "1st. Sewing" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Text("cutType", header: "Type of Cutting", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                 .Text("cutwidth", header: "Cut Width", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Date("TapeInline", header: "Inline", width: Widths.AnsiChars(10),settings:ts2)
                  .Date("TapeOffline", header: "Offline", width: Widths.AnsiChars(10), settings: ts3)
                  .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                  .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                  .Date("fstSCIdlv", header: "SCI Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Date("fstBuyerDlv", header: "Buyer Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("Refno", header: "Ref#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                  .Text("color", header: "Color Desc", width: Widths.AnsiChars(13), iseditingreadonly: true)
                  ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtData;
            string sewinline_b, sewinline_e, sciDelivery_b, sciDelivery_e, buyerdlv_b, buyerdlv_e;
            sewinline_b = null;
            sewinline_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            buyerdlv_b = null;
            buyerdlv_e = null;
            bool eachchk, mtletachk;
            eachchk = checkBox1.Checked;
            mtletachk = checkBox2.Checked;

            if (dateRange1.Value1 != null) sewinline_b = this.dateRange1.Text1;
            if (dateRange1.Value2 != null) { sewinline_e = this.dateRange1.Text2; }

            if (dateRange2.Value1 != null) sciDelivery_b = this.dateRange2.Text1;
            if (dateRange2.Value2 != null) { sciDelivery_e = this.dateRange2.Text2; }

            if (dateRange3.Value1 != null) buyerdlv_b = this.dateRange3.Text1;
            if (dateRange3.Value2 != null) { buyerdlv_e = this.dateRange3.Text2; }

            if ((sewinline_b == null && sewinline_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (buyerdlv_b == null && buyerdlv_e == null))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > or < SCI Delivery > or < Fist Inline Date > can't be empty!!");
                dateRange2.Focus1();
                return;
            }

            string sqlcmd
                = string.Format(@"select 0 as Selected,c.MdivisionId,c.POID,a.EachConsApv
,(select max(FinalETA) from 
	(select po_supp_detail.FinalETA from PO_Supp_Detail 
		WHERE PO_Supp_Detail.ID = B.ID 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	union all
	select b1.FinalETA from PO_Supp_Detail a1, PO_Supp_Detail b1
		where a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND a1.StockPOID = b1.ID and a1.Stockseq1 = b1.SEQ1 and a1.StockSeq2 = b1.SEQ2
	) tmp) as ETA
	,MIN(a.SewInLine) as FstSewinline
    ,b.Special AS cutType
	,round(cast(b.Qty as float)
        * (iif(b.POUnit=b.StockUnit,1,(select unit_rate.rate from unit_rate where Unit_Rate.UnitFrom = b.POUnit and Unit_Rate.UnitTo = b.StockUnit)))
		,(select unit.Round from unit where id = b.StockUnit)) as qty
	,b.stockunit
	,b.SizeSpec cutwidth,B.Refno,B.SEQ1,B.SEQ2
    ,c.TapeInline,c.TapeOffline
	,min(a.SciDelivery) FstSCIdlv
	,min(a.BuyerDelivery) FstBuyerDlv
	,(select color.Name from color where color.id = b.ColorID and color.BrandId = a.brandid ) as color
from orders a inner join Po_supp_detail b on a.poid = b.id
inner join dbo.cuttingtape_detail c on c.mdivisionid = '{0}' and c.poid = b.id and c.seq1 = b.seq1 and c.seq2 = b.seq2
WHERE A.IsForecast = 0 AND A.Junk = 0 AND A.LocalOrder = 0
AND B.SEQ1 = 'A1'
AND ((B.Special NOT LIKE ('%DIE CUT%')) and B.Special is not null)", Sci.Env.User.Keyword);
            if (!(MyUtility.Check.Empty(sciDelivery_b)))
            { sqlcmd += string.Format(@" and a.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sewinline_b)))
            { sqlcmd += string.Format(@" and a.sewinline between '{0}' and '{1}'", sewinline_b, sewinline_e); }
            if (!(string.IsNullOrWhiteSpace(buyerdlv_b)))
            {
                sqlcmd += string.Format(@" and a.BuyerDelivery between '{0}' and '{1}'", buyerdlv_b, buyerdlv_e);
            }
            sqlcmd += "GROUP BY c.MdivisionID,c.POID,a.EachConsApv,B.Special,B.Qty,B.SizeSpec,B.Refno,B.SEQ1,B.SEQ2,c.TapeInline,c.TapeOffline,B.ID,B.ColorID,b.SCIRefno,a.brandid,b.POUnit,b.stockunit";
            if (eachchk && mtletachk) sqlcmd += @" having EachConsApv is not null and (SELECT MAX(FinalETA) FROM 
	(SELECT PO_SUPP_DETAIL.FinalETA FROM PO_Supp_Detail 
		WHERE PO_Supp_Detail.ID = B.ID 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	UNION ALL
	SELECT B1.FinalETA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
		WHERE a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND a1.StockPOID = b1.ID and a1.Stockseq1 = b1.SEQ1 and a1.Stockseq2 = b1.SEQ2
	) tmp) is not null";
            else
            {
                if (eachchk) sqlcmd += " having EachConsApv is not null";
                if (mtletachk) sqlcmd += @" having (SELECT MAX(FinalETA) FROM 
	(SELECT PO_SUPP_DETAIL.FinalETA FROM PO_Supp_Detail 
		WHERE PO_Supp_Detail.ID = B.ID 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	UNION ALL
	SELECT B1.FinalETA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
		WHERE a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND a1.StockPOID = b1.ID and a1.Stockseq1 = b1.SEQ1 and a1.Stockseq2 = b1.SEQ2
	) tmp) is not null";
            }
            sqlcmd += @" ORDER BY c.MdivisionID,c.POID";
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtData))
            {
                if (dtData.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtData;
            }
            else
            {
                ShowErr(sqlcmd, result);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DualResult result;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] find;
            find = dt.Select("Selected = 1");
            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to save it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;

            string sqlcmd = "";
            
            foreach (DataRow item in find)
            {

                sqlcmd += @"update cuttingtape_detail ";
                if (MyUtility.Check.Empty(item["TapeInline"]))
                {sqlcmd += "set tapeinline = null ";}
                else
                {sqlcmd += string.Format(@"set tapeinline ='{0}'", ((DateTime)item["tapeinline"]).ToShortDateString());}
                if (MyUtility.Check.Empty(item["TapeOffline"]))
                {sqlcmd += ",tapeoffline = null ";}
                else
                { sqlcmd += string.Format(@",tapeoffline = '{0}'", ((DateTime)item["TapeOffline"]).ToShortDateString()); }

                sqlcmd += string.Format(@" where poid ='{0}' and seq1 = '{1}' and seq2 = '{2}' and mdivisionid='{3}';"
                                                        , item["POID"]
                                                        , item["seq1"]
                                                        , item["seq2"]
                                                        , item["mdivisionid"]);
                
            }
            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
            {
                MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
                return;
            }
            else { MyUtility.Msg.InfoBox("Save successful!!"); }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                if (null != dateBox1.Value)
                {
                    item["tapeinline"] = dateBox1.Value;
                }
                else
                    item["tapeinline"] = DBNull.Value;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");
            foreach (var item in drfound)
            {
                if (null != dateBox2.Value)
                {
                    item["tapeoffline"] = dateBox2.Value;
                }
                else
                item["tapeoffline"] = DBNull.Value;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayBox1.Value = localPrice.ToString();
        }
    }
}
