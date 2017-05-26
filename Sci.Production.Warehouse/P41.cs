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
            this.checkEachCons.Checked = true;
            this.checkEmptyMtlETA.Checked = true;
            gridEmbAppliqueQuery.RowPostPaint += (s, e) =>
            {
                //DataGridViewRow dvr = detailgrid.Rows[e.RowIndex];
                //DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                DataRow dr = gridEmbAppliqueQuery.GetDataRow(e.RowIndex);
                if (gridEmbAppliqueQuery.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                int i = e.RowIndex;
                if (MyUtility.Check.Empty(dr["EachConsApv"]))
                {
                    gridEmbAppliqueQuery.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 192);
                }
            };


            //設定Grid1的顯示欄位
            this.gridEmbAppliqueQuery.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridEmbAppliqueQuery.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridEmbAppliqueQuery)
                .Text("FactoryId", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Text("EachConsApv", header: "Each Cons.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ETA", header: "Mtl. ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("FstSewinline", header: "1st. Sewing" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Special", header: "Special", width: Widths.AnsiChars(25), iseditingreadonly: true)
                 .Text("SizeSpec", header: "SizeSpec", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Date("SCIdlv", header: "SCI Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Date("BuyerDlv", header: "Buyer Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("Refno", header: "Ref#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                  .Text("color", header: "Color Desc", width: Widths.AnsiChars(13), iseditingreadonly: true)
                  ;
        }

        private void btnQuery_Click(object sender, EventArgs e)
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
            eachchk = checkEachCons.Checked;
            mtletachk = checkEmptyMtlETA.Checked;

            if (dateSewingInline.Value1 != null) sewinline_b = this.dateSewingInline.Text1;
            if (dateSewingInline.Value2 != null) { sewinline_e = this.dateSewingInline.Text2; }

            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }

            if (dateBuyerDelivery.Value1 != null) buyerdlv_b = this.dateBuyerDelivery.Text1;
            if (dateBuyerDelivery.Value2 != null) { buyerdlv_e = this.dateBuyerDelivery.Text2; }

            if ((sewinline_b == null && sewinline_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (buyerdlv_b == null && buyerdlv_e == null))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > or < SCI Delivery > or < Fist Inline Date > can't be empty!!");
                dateSCIDelivery.Focus1();
                return;
            }

            string sqlcmd
                = string.Format(@"select B.FactoryID,c.POID,concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2) as seq,a.EachConsApv
,(SELECT MAX(FinalETA) FROM 
	(SELECT PO_SUPP_DETAIL.FinalETA FROM PO_Supp_Detail WITH (NOLOCK) 
		WHERE PO_Supp_Detail.ID = B.ID 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	UNION ALL
	SELECT B1.FinalETA FROM PO_Supp_Detail a1 WITH (NOLOCK) , PO_Supp_Detail b1 WITH (NOLOCK) 
		WHERE a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND a1.StockPOID = b1.ID and a1.StockSeq1 = b1.SEQ1 and a1.StockSeq2 = b1.SEQ2
	) tmp) as ETA
	,MIN(a.SewInLine) as FstSewinline
    ,b.Special
    ,b.SizeSpec
	,Qty = round(dbo.GetUnitQty(b.POUnit, b.StockUnit, b.Qty), (select unit.Round from unit WITH (NOLOCK) where id = b.StockUnit)) 
    ,min(a.SciDelivery) SCIdlv
    ,min(a.BuyerDelivery) BuyerDlv
    ,B.Refno
    ,a.BrandID
    ,(select color.Name from color WITH (NOLOCK) where color.id = b.ColorID and color.BrandId = a.brandid ) as color
	,b.stockunit	
    ,B.SEQ1
    ,B.SEQ2
    ,c.TapeInline,c.TapeOffline		    	
from dbo.orders a WITH (NOLOCK) inner join dbo.po_supp_detail b WITH (NOLOCK) on a.poid = b.id
inner join dbo.cuttingtape_detail c WITH (NOLOCK) on c.mdivisionid = '{0}' and c.poid = b.id and c.seq1 = b.seq1 and c.seq2 = b.seq2
WHERE A.IsForecast = 0 AND A.Junk = 0 AND A.LocalOrder = 0
AND (B.Special LIKE ('%EMB-APPLIQUE%') or B.Special LIKE ('%EMB APPLIQUE%'))", Sci.Env.User.Keyword);
            if (!(MyUtility.Check.Empty(sciDelivery_b)))
            { sqlcmd += string.Format(@" and a.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sewinline_b)))
            { sqlcmd += string.Format(@" and a.sewinline between '{0}' and '{1}'", sewinline_b, sewinline_e); }
            if (!(string.IsNullOrWhiteSpace(buyerdlv_b)))
            {
                sqlcmd += string.Format(@" and a.BuyerDelivery between '{0}' and '{1}'", buyerdlv_b, buyerdlv_e);
            }
            sqlcmd += "GROUP BY c.mdivisionid,c.POID,a.EachConsApv,B.Special,B.Qty,B.SizeSpec,B.Refno,B.SEQ1,B.SEQ2,c.TapeInline,c.TapeOffline,B.ID,B.ColorID,b.SCIRefno,a.brandid,b.POUnit,b.stockunit,B.FactoryID";
            //20161220 CheckBox 選項用 checkBoxs_Status() 取代
//            if (eachchk && mtletachk) sqlcmd += @" having EachConsApv is not null and (SELECT MAX(FinalETA) FROM 
//	(SELECT B1.FinalETA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
//		WHERE a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
//		AND a1.StockPOID = b1.ID and a1.StockSeq1 = b1.SEQ1 and a1.StockSeq2 = b1.SEQ2
//	) tmp) is not null";
//            else
//            {
//                if (eachchk) sqlcmd += " having EachConsApv is not null";
//                if (mtletachk) sqlcmd += @" having (SELECT MAX(FinalETA) FROM 
//	(SELECT B1.FinalETA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
//		WHERE a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
//		AND a1.StockPOID = b1.ID and a1.StockSeq1 = b1.SEQ1 and a1.StockSeq2 = b1.SEQ2
//	) tmp) is not null";
//            }
            sqlcmd += @" ORDER BY c.mdivisionid,c.POID";
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtData))
            {
                if (dtData.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtData;
                checkBoxs_Status();
            }
            else
            {
                ShowErr(sqlcmd, result);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("poid", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.DataSource == null) return;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            dt.DefaultView.RowFilter = listControlBindingSource1.Filter;
            dt = dt.DefaultView.ToTable();
            dt.Columns.Remove("stockunit");
            dt.Columns.Remove("SEQ1");
            dt.Columns.Remove("SEQ2");
            dt.Columns.Remove("TapeInline");
            dt.Columns.Remove("TapeOffline");
            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data!!");
                return;
            }
            Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(dt);
            sdExcel.Save();
            //string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            //SaveFileDialog dlg = new SaveFileDialog();
            //dlg.RestoreDirectory = true;
            //dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            //dlg.Title = "Save as Excel File";
            //dlg.FileName = "P41_EmbApplique_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            //dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            //// Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            //// Get the selected file name and CopyToXls
            //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            //{
            //    //DualResult result = MyUtility.Excel.CopyToXls(dt, dlg.FileName);
            //    //if (result) { MyUtility.Excel.XlsAutoFit(dlg.FileName); }   //XlsAutoFit(dlg.FileName, "MMDR030.xltx", 12);
            //    //else { MyUtility.Msg.WarningBox(result.ToMessages().ToString(), "Warning"); }
            //}
            //else
            //{
            //    return;
            //}
        }

        private void checkEachCons_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxs_Status();
        }

        private void checkEmptyMtlETA_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxs_Status();
        }

        private void checkBoxs_Status()
        {
            if (listControlBindingSource1.DataSource == null)
                return;
            string formatStr = "";
            if (checkEachCons.Checked) formatStr += "EachConsApv is not null ";
            if (checkEmptyMtlETA.Checked)
                formatStr += (formatStr.EqualString("")) ? "ETA is not null" : "and ETA is not null";

            listControlBindingSource1.Filter = string.Format(formatStr);
        }
    }
}
