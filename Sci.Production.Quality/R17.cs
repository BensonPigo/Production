using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R17 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private string Excelfile;

        /// <inheritdoc/>
        public R17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateETA.Value1) && MyUtility.Check.Empty(this.dateArrWHDate.Value1) && MyUtility.Check.Empty(this.dateInsp.Value1))
            {
                MyUtility.Msg.InfoBox("Please fill in the required fields.");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult r = new DualResult(true);
            string cmd = string.Empty;

            cmd = $@"
 select
          ItemID = psd.ID + '_' + CAST(ROW_NUMBER() OVER (PARTITION BY psd.ID ORDER BY psd.ID) AS varchar(10))
        , 'Fabric' AS 'Product Line ID'
        , 'Fabric' AS 'Product Line Name'
        , 'Fabric' AS 'Product Category ID'
        , 'Fabric' AS 'Product Category Name'
        , 'lululemon' AS 'Brand ID'
        , 'lululemon'  AS 'Brand Name'
        , psd.Refno AS 'Style ID'
        , o.StyleID AS 'Style Name'
        , psds.SpecValue as 'Color'
        , [Size] = CAST(isnull(fb.WeightM2, 0) AS varchar(999)) 
            + ' x ' + CAST(isnull(fb.Width, 0) AS varchar(999)) 
            + ' in x ' + CAST(isnull(rec_val.ActualQty, 0) AS varchar(999)) + ' yd'
        , '' as 'Case Pack'
        , psdQty.Qty as 'Order Quantity'
        , '' as 'Available Quantity'
 from po_supp_detail psd
  inner join orders o on o.ID =psd.id
  left join Receiving_Detail rd on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
  left join Receiving r on  r.ID = rd.ID
  left join fir f on f.POID =psd.ID and f.SEQ1 =psd.SEQ1 and f.SEQ2 = psd.SEQ2
  left join PO_Supp_Detail_Spec psds on psds.ID =psd.id and psds.Seq1 =psd.SEQ1 and psds.Seq2 =psd.SEQ2 and psds.SpecColumnID ='Color'
  left join Fabric fb on fb.SCIRefno =psd.SCIRefno
  Outer Apply (Select Qty = Sum(psd2.Qty) 
  			   from Po_supp_detail psd2
			   left join PO_Supp_Detail_Spec psds2 on psds2.ID =psd2.id and psds2.Seq1 =psd2.SEQ1 and psds2.Seq2 =psd2.SEQ2 and psds2.SpecColumnID ='Color'
			   Where psd2.ID = psd.ID 
			   and psd2.Refno = psd.RefNo
			   and psds.SpecValue = psds2.SpecValue
               and psd2.Seq1 = psd.Seq1
               and psd2.Seq2 = psd.Seq2
) psdQty
  outer apply (select Max(rd2.ActualQty) as ActualQty
                from po_supp_detail psd2
				inner join Receiving_Detail rd2 on rd2.PoId = psd2.ID and rd2.Seq1 = psd2.SEQ1 and rd2.Seq2 = psd2.SEQ2
				where 1=1
                  and psd2.ID = psd.ID 
                  and psd2.Refno = psd.Refno
                  and psd2.Seq1 = psd.Seq1
                  and psd2.Seq2 = psd.Seq2
) as rec_val
where 1=1
and psd.Junk = 0
and o.BrandID = 'LLL'
and psd.FabricType = 'F'
";
            if (!MyUtility.Check.Empty(this.dateETA.Value1))
            {
                cmd += $@"AND psd.ETA >='{this.dateETA.Value1.Value.ToString("yyyy-MM-dd")}' and psd.ETA <= '{this.dateETA.Value2.Value.ToString("yyyy-MM-dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateArrWHDate.Value1))
            {
                cmd += $@"AND r.WhseArrival >= '{this.dateArrWHDate.Value1.Value.ToString("yyyy-MM-dd")}' and r.WhseArrival <= '{this.dateArrWHDate.Value2.Value.ToString("yyyy-MM-dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateInsp.Value1))
            {
                cmd += $@"AND f.PhysicalDate >= '{this.dateInsp.Value1.Value.ToString("yyyy-MM-dd")}' and f.PhysicalDate <= '{this.dateInsp.Value2.Value.ToString("yyyy-MM-dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                cmd += $@"AND psd.ID >= '{this.txtSP1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                cmd += $@"AND psd.ID <= '{this.txtSP2.Text}'";
            }

            cmd += @"Group by psd.ID, psd.Refno,  o.StyleID, psds.SpecValue, fb.WeightM2, fb.Width, rec_val.ActualQty, psdQty.Qty
                     Order by psd.ID";

            r = DBProxy.Current.Select(string.Empty, cmd, out this.printData);
            if (!r)
            {
                this.ShowErr(r);
                return r;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string xltx = "Quality_R17.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltx, 1, true, null, objApp); // 將datatable copy to excel
            return true;
        }
    }
}
