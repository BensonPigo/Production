using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R05
    /// </summary>
    public partial class R05 : Win.Tems.PrintForm
    {
        private DataTable _printData;
        private DateTime? _apvDate1;
        private DateTime? _apvDate2;
        private string _reportType;
        private string _mDivision;
        private string _factory;
        private string _reportTypeName;

        /// <summary>
        /// R05
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboReportType, 1, 1, "Fabric,Accessory");
            this.comboReportType.SelectedIndex = 0;
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Env.User.Keyword;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateApvDate.Value1))
            {
                MyUtility.Msg.WarningBox("SCI Delivery can't empty!!");
                return false;
            }

            this._apvDate1 = this.dateApvDate.Value1;
            this._apvDate2 = this.dateApvDate.Value2;
            this._reportType = this.comboReportType.Text == "Fabric" ? "F" : this.comboReportType.Text == "Accessory" ? "A" : string.Empty;
            this._mDivision = this.comboM.Text;
            this._factory = this.comboFactory.Text;
            this._reportTypeName = this.comboReportType.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select distinct 
    l.MDivisionID,l.FactoryID,l.POID,ld.Seq1,ld.Seq2,l.FabricType,
    Refno = isnull(psd.Refno,''),
    NETQty1 = isnull(psd.NETQty,0),
    POUnit = isnull(psd.POUnit,''),
    StockUnit = isnull(psd.StockUnit,''),
    OutputSeq1,OutputSeq2,SCIRefno,
    NETQty2 = isnull(psd2.NETQty,0),
    INVPOUnit = isnull(psd2.POUnit,''),
    INVStockUnit = isnull(psd2.StockUnit,''),
    f.MtlTypeID,
    [INQTY] = isnull(mpd.InQty,0)+isnull(mpd2.InQty ,0)+ISnull(mpd7.InQty ,0 ),
    IS7 = IIF(ld.Seq1 LIKE'7_',1,0),
    c1 = isnull(c1.c1,1),
    c2 = isnull(c2.c2,1)
into #tmpData
from Lack l WITH (NOLOCK) 
inner join Lack_Detail ld WITH (NOLOCK) on ld.ID = l.ID	
outer apply (
	select Refno,NETQty,POUnit,StockUnit,OutputSeq1,OutputSeq2,SCIRefno
	from PO_Supp_Detail WITH (NOLOCK) 
	where ID = l.POID and SEQ1 = ld.Seq1 and SEQ2 = ld.Seq2
)psd
outer apply (
	select NETQty,POUnit,StockUnit 
	from PO_Supp_Detail WITH (NOLOCK) 
	where ID = L.POID and SEQ1 = psd.OutputSeq1 and SEQ2 = psd.OutputSeq2
)psd2
outer apply (select MtlTypeID from Fabric  WITH (NOLOCK) where SCIRefno = psd.SCIRefno)f
outer apply (
	select m.inqty 
	from MDivisionPoDetail m WITH (NOLOCK) 
    inner join Orders o WITH (NOLOCK) on m.POID=o.ID  
    inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup
	where f.MDivisionId = L.MDivisionID and m.POID = L.POID and Seq1 = LD.Seq1 and Seq2 = LD.Seq2
) mpd
outer apply (
	select sum(InQty) inqty 
	from PO_Supp_Detail psd3 
    INNER join MDivisionPoDetail m on m.Seq1 = psd3.SEQ1 and m.Seq2 = psd3.SEQ2
	where psd3.ID = L.POID and psd3.OutputSeq1 = LD.Seq1 and psd3.OutputSeq2 = LD.SEQ2
	and m.POID = L.POID  
) mpd2
outer apply (
	select inqty 
	from MDivisionPoDetail m WITH (NOLOCK) 
    inner join Orders o WITH (NOLOCK) on m.POID=o.ID  
    inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup
	where f.MDivisionId = L.MDivisionID and m.POID = L.POID and Seq1 = PSD.OutputSeq1 and Seq2 = PSD.OutputSeq2
)mpd7
outer apply(select RateValue c1 from View_Unitrate WITH (NOLOCK) where FROM_U = psd.POUnit and TO_U = psd.StockUnit) c1
outer apply(select RateValue c2 from View_Unitrate WITH (NOLOCK) where FROM_U = psd2.POUnit and TO_U = psd2.StockUnit) c2
where l.Type = 'R'"));

            if (!MyUtility.Check.Empty(this._apvDate1))
            {
                sqlCmd.Append(string.Format(@" and convert(date,l.ApvDate) >= '{0}'", Convert.ToDateTime(this._apvDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this._apvDate2))
            {
                sqlCmd.Append(string.Format(@" and convert(date,l.ApvDate) <= '{0}'", Convert.ToDateTime(this._apvDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this._reportType))
            {
                sqlCmd.Append(string.Format(" and l.FabricType = '{0}'", this._reportType));
            }

            if (!MyUtility.Check.Empty(this._mDivision))
            {
                sqlCmd.Append(string.Format(" and l.MDivisionID = '{0}'", this._mDivision));
            }

            if (!MyUtility.Check.Empty(this._factory))
            {
                sqlCmd.Append(string.Format(" and l.FactoryID = '{0}'", this._factory));
            }

            sqlCmd.Append(@" 
;with s1 as(
select T.MDivisionID,T.FactoryID,T.POID,T.Seq1,T.Seq2,StockQty1 = isnull(sum(i.qty) over(partition by MDivisionID,FactoryID,FabricType,POID,Seq1,Seq2,i.type),0)
	from #tmpData T	outer apply(select Qty,Type from Invtrans  WITH (NOLOCK) where PoID = T.POID and Seq1 = T.Seq1 and Seq2 = T.Seq2 and Type = '1') i
),s2 as(
select T.MDivisionID,T.FactoryID,T.POID,T.Seq1,T.Seq2,StockQty2 = isnull(sum(i2.qty) over(partition by MDivisionID,FactoryID,FabricType,POID,Seq1,Seq2,i2.type),0)
	from #tmpData T	outer apply(select Qty,Type from Invtrans  WITH (NOLOCK) where InventoryPOID = T.POID and InventorySeq1 = T.Seq1 and InventorySeq2 = T.Seq2 and Type = '4') i2
),s71 as(
select T.MDivisionID,T.FactoryID,T.POID,T.Seq1,T.Seq2,StockQty71 = isnull(sum(i7.qty) over(partition by MDivisionID,FactoryID,FabricType,POID,OutputSeq1,OutputSeq2,i7.type),0)
	from #tmpData T	outer apply(select Qty,Type from Invtrans  WITH (NOLOCK) where PoID = T.POID and Seq1 = T.OutputSeq1 and Seq2 = T.OutputSeq2 and Type = '1') i7	
),s72 as(
select T.MDivisionID,T.FactoryID,T.POID,T.Seq1,T.Seq2,StockQty72 = isnull(sum(i72.qty) over(partition by MDivisionID,FactoryID,FabricType,POID,OutputSeq1,OutputSeq2,i72.type),0)
	from #tmpData T	outer apply(select Qty,Type from Invtrans  WITH (NOLOCK) where InventoryPOID = T.POID and InventorySeq1 = T.OutputSeq1 and InventorySeq2 = T.OutputSeq2 and Type = '4') i72
),RequestQty as(
	select  T.MDivisionID,T.FactoryID,T.POID,T.Seq1,T.Seq2,RequestQty = isnull(sum(RequestQty) over(partition by MDivisionID,FactoryID,FabricType,POID,Seq1,Seq2),0)
	from #tmpData T 
	OUTER APPLY(
		SELECT  RequestQty	FROM Lack L	inner join Lack_Detail ld WITH (NOLOCK) on ld.ID = l.ID
		WHERE l.Type = 'R' and l.FabricType = T.FabricType	and l.MDivisionID = T.MDivisionID and l.FactoryID = T.FactoryID	AND POID = T.POID AND SEQ1 = T.Seq1 AND SEQ2 = T.Seq2
	)R
), AllTMP AS(
	select DISTINCT 
	T.*,S1.StockQty1,S2.StockQty2,S71.StockQty71,S72.StockQty72,RequestQty
	from #tmpData T
	outer apply(SELECT StockQty1 FROM S1 WHERE MDivisionID = T.MDivisionID AND FactoryID = T.FactoryID AND POID = T.POID AND Seq1 = T.Seq1 AND Seq2 = T.Seq2)S1
	outer apply(SELECT StockQty2 FROM S2 WHERE MDivisionID = T.MDivisionID AND FactoryID = T.FactoryID AND POID = T.POID AND Seq1 = T.Seq1 AND Seq2 = T.Seq2)S2
	outer apply(SELECT StockQty71 FROM S71 WHERE MDivisionID = T.MDivisionID AND FactoryID = T.FactoryID AND POID = T.POID AND Seq1 = T.Seq1 AND Seq2 = T.Seq2)S71
	outer apply(SELECT StockQty72 FROM S72 WHERE MDivisionID = T.MDivisionID AND FactoryID = T.FactoryID AND POID = T.POID AND Seq1 = T.Seq1 AND Seq2 = T.Seq2)S72
	outer apply(SELECT RequestQty FROM RequestQty WHERE MDivisionID = T.MDivisionID AND FactoryID = T.FactoryID AND POID = T.POID AND Seq1 = T.Seq1 AND Seq2 = T.Seq2)R	
)
select 
	MDivisionID,FactoryID,POID,Seq1,Seq2,Refno,MtlTypeID,InQty,
	[NETQty] = NETQty1*c1 + NETQty2*c2,
	[StockQty] = StockQty1 * c1 + StockQty2 * c2 + IIF(IS7 = 0,0,StockQty71 * c1 + StockQty72 * c2),
	[Allowance Qty] = InQty - (NETQty1*c1 + NETQty2*c2) - (StockQty1 * c1 + StockQty2 * c2 + IIF(IS7 = 0,0,StockQty71 * c1 + StockQty72 * c2)),
	RequestQty
from AllTMP
order by MDivisionID,FactoryID,POID,Seq1,Seq2
drop table #tmpData");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this._printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData.Rows.Count);

            if (this._printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\PPIC_R05_AllowanceConsumptionReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[3, 3] = string.Format("{0}~{1}", MyUtility.Check.Empty(this._apvDate1) ? string.Empty : Convert.ToDateTime(this._apvDate1).ToString("d"), MyUtility.Check.Empty(this._apvDate2) ? string.Empty : Convert.ToDateTime(this._apvDate2).ToString("d"));
            worksheet.Cells[4, 3] = this._reportTypeName;
            worksheet.Cells[3, 8] = this._mDivision;
            worksheet.Cells[4, 8] = this._factory;
            worksheet.Cells[3, 12] = DateTime.Today.ToString("d");

            // 填內容值
            int intRowsStart = 6;
            object[,] objArray = new object[1, 12];
            foreach (DataRow dr in this._printData.Rows)
            {
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["POID"];
                objArray[0, 3] = MyUtility.Convert.GetString(dr["Seq1"]) + '-' + MyUtility.Convert.GetString(dr["Seq2"]);
                objArray[0, 4] = dr["Refno"];
                objArray[0, 5] = dr["MtlTypeID"];
                objArray[0, 6] = dr["InQty"];
                objArray[0, 7] = dr["NETQty"];
                objArray[0, 8] = dr["StockQty"];
                objArray[0, 9] = MyUtility.Convert.GetDecimal(dr["InQty"]) - MyUtility.Convert.GetDecimal(dr["NETQty"]) - MyUtility.Convert.GetDecimal(dr["StockQty"]);
                objArray[0, 10] = dr["RequestQty"];
                objArray[0, 11] = MyUtility.Convert.GetDecimal(dr["RequestQty"]) > MyUtility.Convert.GetDecimal(dr["InQty"]) - MyUtility.Convert.GetDecimal(dr["NETQty"]) - MyUtility.Convert.GetDecimal(dr["StockQty"]) ? "FAIL" : "PASS";

                worksheet.Range[string.Format("A{0}:L{0}", intRowsStart)].Value2 = objArray;
                worksheet.Range[string.Format("A{0}:L{0}", intRowsStart)].Borders.Weight = 2; // 設定全框線
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R05_AllowanceConsumptionReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
