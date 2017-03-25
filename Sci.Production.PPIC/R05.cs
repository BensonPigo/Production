using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        DataTable _printData;
        DateTime? _apvDate1, _apvDate2;
        string _reportType, _mDivision, _factory, _reportTypeName;
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "Fabric,Accessory");
            comboBox1.SelectedIndex = 0;
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox2, 1, mDivision);
            comboBox2.Text = Sci.Env.User.Keyword;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboBox3, 1, factory);
            comboBox3.Text = Sci.Env.User.Factory;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("SCI Delivery can't empty!!");
                return false;
            }
            _apvDate1 = dateRange1.Value1;
            _apvDate2 = dateRange1.Value2;
            _reportType = comboBox1.Text == "Fabric" ? "F" : comboBox1.Text == "Accessory" ? "A" : "";
            _mDivision = comboBox2.Text;
            _factory = comboBox3.Text;
            _reportTypeName = comboBox1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
with tmpData as (
	select DISTINCT l.MDivisionID,l.FactoryID,l.POID,ld.Seq1,ld.Seq2,
	l.FabricType,
	[RequestQty] = sum(RequestQty) over(partition by l.MDivisionID,l.FactoryID,l.POID,ld.Seq1,ld.Seq2,l.FabricType) 
	from Lack l WITH (NOLOCK) 
	inner join Lack_Detail ld WITH (NOLOCK) on ld.ID = l.ID	
where l.Type = 'R'"));

            if (!MyUtility.Check.Empty(_apvDate1))
            {
                sqlCmd.Append(string.Format(@" and convert(date,l.ApvDate) >= '{0}'", Convert.ToDateTime(_apvDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(_apvDate2))
            {
                sqlCmd.Append(string.Format(@" and convert(date,l.ApvDate) <= '{0}'", Convert.ToDateTime(_apvDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(_reportType))
            {
                sqlCmd.Append(string.Format(" and l.FabricType = '{0}'", _reportType));
            }

            if (!MyUtility.Check.Empty(_mDivision))
            {
                sqlCmd.Append(string.Format(" and l.MDivisionID = '{0}'", _mDivision));
            }

            if (!MyUtility.Check.Empty(_factory))
            {
                sqlCmd.Append(string.Format(" and l.FactoryID = '{0}'", _factory));
            }

            sqlCmd.Append(@" 
),TMP1 AS( 
	SELECT T.MDivisionID,T.FactoryID,T.POID,T.Seq1,T.Seq2,T.FabricType,RequestQty,
	isnull(psd.Refno,'') as Refno,isnull(f.MtlTypeID,'') as MtlTypeID,
	INQTY = isnull(mpd.InQty,0)+isnull(mpd2.InQty ,0)+ ISnull(mpd7.InQty ,0 ),
	isnull(psd.NETQty,0) as NETQty1,
	isnull(psd2.NETQty,0) as NETQty2,
	isnull(psd.POUnit,'') as POUnit,isnull(psd.StockUnit,'') as StockUnit,isnull(psd2.POUnit,'') as INVPOUnit,
	isnull(psd2.StockUnit,'') as INVStockUnit,
	isnull(i.Qty,0) as StockQty1,
	isnull(i2.Qty,0) as StockQty2,
	isnull(i7.Qty,0) as StockQty71,
	isnull(i72.Qty,0) as StockQty72,
	IIF(T.Seq1 LIKE'7_',1,0) IS7
	FROM tmpData T
	outer apply (select * from PO_Supp_Detail WITH (NOLOCK) where ID = T.POID and SEQ1 = T.Seq1 and SEQ2 = T.Seq2)psd
	outer apply (select * from PO_Supp_Detail WITH (NOLOCK) where ID = T.POID and SEQ1 = psd.OutputSeq1 and SEQ2 = psd.OutputSeq2)psd2
	outer apply (select * from Fabric  WITH (NOLOCK) where SCIRefno = psd.SCIRefno)f
	outer apply (select inqty from MDivisionPoDetail WITH (NOLOCK) where MDivisionId = T.MDivisionID and POID = T.POID and Seq1 = T.Seq1 and Seq2 = T.Seq2) mpd
	outer apply (select sum(InQty) inqty 
		from PO_Supp_Detail psd3 left join MDivisionPoDetail m on m.Seq1 = psd3.SEQ1 and m.Seq2 = psd3.SEQ2
		where m.MDivisionId = T.MDivisionID and m.POID = T.POID and psd3.ID = T.POID and psd3.OutputSeq1 = psd.Seq1 and psd3.OutputSeq2 = psd.SEQ2) mpd2
		outer apply (select inqty from MDivisionPoDetail WITH (NOLOCK) where MDivisionId = T.MDivisionID and POID = T.POID and Seq1 = PSD.OutputSeq1 and Seq2 = PSD.OutputSeq2
	)mpd7
	outer apply (select SUM(Qty)Qty from Invtrans  WITH (NOLOCK) where PoID = T.POID and Seq1 = T.Seq1 and Seq2 = T.Seq2 and Type = 1) i
	outer apply (select SUM(Qty)Qty from Invtrans  WITH (NOLOCK) where InventoryPOID = T.POID and InventorySeq1 = T.Seq1 and InventorySeq2 = T.Seq2 and Type = 4) i2
	outer apply (select SUM(Qty)Qty from Invtrans  WITH (NOLOCK) where PoID = T.POID and Seq1 = PSD.OutputSeq1 and Seq2 = PSD.OutputSeq2 and Type = 1) i7	
	outer apply (select SUM(Qty)Qty from Invtrans  WITH (NOLOCK) where InventoryPOID = T.POID and InventorySeq1 = PSD.OutputSeq1 and InventorySeq2 = PSD.OutputSeq2 and Type = 4) i72
),tmpData2 as (
	select MDivisionID,FactoryID,POID,Seq1,Seq2,Refno,MtlTypeID,RequestQty,
	InQty,
	NETQty1*IIF(POUnit is null or StockUnit is null,1, dbo.getUnitRate(POUnit,StockUnit))+
	NETQty2*IIF(INVPOUnit is null or INVStockUnit is null,1, dbo.getUnitRate(INVPOUnit,INVStockUnit)) as NETQty,
	StockQty1*IIF(POUnit is null or StockUnit is null,1, dbo.getUnitRate(POUnit,StockUnit))+
	StockQty2*IIF(INVPOUnit is null or INVStockUnit is null,1, dbo.getUnitRate(INVPOUnit,INVStockUnit)) +
	IIF(IS7 = 0,0,
	StockQty71*IIF(POUnit is null or StockUnit is null,1, dbo.getUnitRate(POUnit,StockUnit))+
	StockQty72*IIF(INVPOUnit is null or INVStockUnit is null,1, dbo.getUnitRate(INVPOUnit,INVStockUnit)) )
	as StockQty,
	FabricType
	from TMP1
)
select MDivisionID,FactoryID,POID,Seq1,Seq2,Refno,MtlTypeID,InQty,NETQty,StockQty,
[Allowance Qty]=InQty-NETQty-StockQty,
RequestQty
from tmpData2 T
order by MDivisionID,FactoryID,POID,Seq1,Seq2
"
                );

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out _printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(_printData.Rows.Count);

            if (_printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R05_AllowanceConsumptionReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[3, 3] = string.Format("{0}~{1}", MyUtility.Check.Empty(_apvDate1) ? "" : Convert.ToDateTime(_apvDate1).ToString("d"), MyUtility.Check.Empty(_apvDate2) ? "" : Convert.ToDateTime(_apvDate2).ToString("d"));
            worksheet.Cells[4, 3] = _reportTypeName;
            worksheet.Cells[3, 8] = _mDivision;
            worksheet.Cells[4, 8] = _factory;
            worksheet.Cells[3, 12] = DateTime.Today.ToString("d");

            //填內容值
            int intRowsStart = 6;
            object[,] objArray = new object[1, 12];
            foreach (DataRow dr in _printData.Rows)
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
                
                worksheet.Range[String.Format("A{0}:L{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
