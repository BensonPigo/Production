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
    public partial class P41 : Sci.Win.Tems.QueryForm
    {
        public P41(ToolStripMenuItem menuitem)
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

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("FactoryID", header: "Fac", width: Widths.AnsiChars(5),  iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Text("EachConsApv", header: "Each Cons.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ETA", header: "Mtl. ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("fstSewinline", header: "1st. Sewing" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Special", header: "Special", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("SizeSpec", header: "SizeSpec", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
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
                = @"select a.FactoryID,a.POID,a.EachConsApv
,(SELECT MAX(ATA) FROM 
	(SELECT PO_SUPP_DETAIL.ATA FROM PO_Supp_Detail 
		WHERE PO_Supp_Detail.ID = B.ID AND PO_Supp_Detail.DetailType ='A' 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	UNION ALL
	SELECT B1.ATA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
		WHERE a1.ID = B.ID AND a1.DetailType ='B' AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND LEFT(a1.StockPOID,13) = b1.ID and SUBSTRING(a1.StockPOID,14,3) = b1.SEQ1 and right(a1.StockPOID,2) = b1.SEQ2
	) tmp) as ETA
	,MIN(a.SewInLine) as fstSewinline
    ,b.Special
	,round(cast(b.Qty as float)* (select unit_rate.rate from unit_rate where Unit_Rate.UnitFrom = b.POUnit and Unit_Rate.UnitTo = b.StockUnit)
			,(select unit.Round from unit where id = b.StockUnit)) as qty
	,b.stockunit
	,b.SizeSpec,B.Refno,B.SEQ1,B.SEQ2,B.TapeInline,B.TapeOffline
	,min(a.SciDelivery) fstSCIdlv
	,min(a.BuyerDelivery) fstBuyerDlv
	,(select color.Name from color where color.id = b.ColorID and color.BrandId = a.brandid ) as color
from orders a inner join PO_Artwork b on a.poid = b.id
WHERE A.IsForecast = 0 AND A.Junk = 0 AND A.LocalOrder = 0
AND B.SEQ1 = 'A1'
AND B.Special LIKE ('%EMB APPLIQUE%')";
            if (!(MyUtility.Check.Empty(sciDelivery_b)))
            { sqlcmd += string.Format(@" and a.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sewinline_b)))
            { sqlcmd += string.Format(@" and a.sewinline between '{0}' and '{1}'", sewinline_b, sewinline_e); }
            if (!(string.IsNullOrWhiteSpace(buyerdlv_b)))
            {
                sqlcmd += string.Format(@" and a.BuyerDelivery between '{0}' and '{1}'", buyerdlv_b, buyerdlv_e);
            }
            sqlcmd += "GROUP BY a.FactoryID,a.POID,a.EachConsApv,B.Special,B.Qty,B.SizeSpec,B.Refno,B.SEQ1,B.SEQ2,B.TapeInline,B.TapeOffline,B.ID,B.ColorID,b.SCIRefno,a.brandid,b.POUnit,b.stockunit";
            if (eachchk && mtletachk) sqlcmd += @" having EachConsApv is not null and (SELECT MAX(ATA) FROM 
	(SELECT PO_SUPP_DETAIL.ATA FROM PO_Supp_Detail 
		WHERE PO_Supp_Detail.ID = B.ID AND PO_Supp_Detail.DetailType ='A' 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	UNION ALL
	SELECT B1.ATA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
		WHERE a1.ID = B.ID AND a1.DetailType ='B' AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND LEFT(a1.StockPOID,13) = b1.ID and SUBSTRING(a1.StockPOID,14,3) = b1.SEQ1 and right(a1.StockPOID,2) = b1.SEQ2
	) tmp) is not null";
            else
            {
                if (eachchk) sqlcmd += " having EachConsApv is not null";
                if (mtletachk) sqlcmd += @" having (SELECT MAX(ATA) FROM 
	(SELECT PO_SUPP_DETAIL.ATA FROM PO_Supp_Detail 
		WHERE PO_Supp_Detail.ID = B.ID AND PO_Supp_Detail.DetailType ='A' 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	UNION ALL
	SELECT B1.ATA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
		WHERE a1.ID = B.ID AND a1.DetailType ='B' AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND LEFT(a1.StockPOID,13) = b1.ID and SUBSTRING(a1.StockPOID,14,3) = b1.SEQ1 and right(a1.StockPOID,2) = b1.SEQ2
	) tmp) is not null";
            }
            sqlcmd += @" ORDER BY A.FactoryID,A.POID";
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0) return;
            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            dlg.FileName = "P41_EmbApplique_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            // Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            // Get the selected file name and CopyToXls
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                DualResult result = MyUtility.Excel.CopyToXls(dt, dlg.FileName);
                if (result) { MyUtility.Excel.XlsAutoFit(dlg.FileName); }   //XlsAutoFit(dlg.FileName, "MMDR030.xlt", 12);
                else { MyUtility.Msg.WarningBox(result.ToMessages().ToString(), "Warning"); }
            }
            else
            {
                return;
            }
        }
    }
}
