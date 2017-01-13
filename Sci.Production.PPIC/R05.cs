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
        DataTable printData;
        DateTime? apvDate1, apvDate2;
        string reportType, mDivision, factory, reportTypeName;
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "Fabric,Accessory");
            comboBox1.SelectedIndex = 0;
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox2, 1, mDivision);
            comboBox2.Text = Sci.Env.User.Keyword;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
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
            apvDate1 = dateRange1.Value1;
            apvDate2 = dateRange1.Value2;
            reportType = comboBox1.Text == "Fabric" ? "F" : comboBox1.Text == "Accessory" ? "A" : "";
            mDivision = comboBox2.Text;
            factory = comboBox3.Text;
            reportTypeName = comboBox1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"with tmpData
as (
select l.MDivisionID,l.FactoryID,l.POID,ld.Seq1,ld.Seq2,isnull(psd.Refno,'') as Refno,isnull(f.MtlTypeID,'') as MtlTypeID,
ld.RequestQty,isnull(mpd.InQty,0)+isnull(mpd2.InQty ,0) as InQty,isnull(psd.NETQty,0) as NETQty1,isnull(psd2.NETQty,0) as NETQty2,
isnull(psd.POUnit,'') as POUnit,isnull(psd.StockUnit,'') as StockUnit,isnull(psd2.POUnit,'') as INVPOUnit,
isnull(psd2.StockUnit,'') as INVStockUnit,
sum(isnull(i.Qty,0)) as StockQty1,sum(isnull(i2.Qty,0)) as StockQty2
from Lack l
inner join Lack_Detail ld on ld.ID = l.ID
left join PO_Supp_Detail psd on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
left join Fabric f on f.SCIRefno = psd.SCIRefno
left join MDivisionPoDetail mpd on mpd.MDivisionId = l.MDivisionID and mpd.POID = l.POID and mpd.Seq1 = ld.Seq1 and mpd.Seq2 = ld.Seq2
left join PO_Supp_Detail psd2 on psd2.ID = l.POID and psd2.SEQ1 = psd.OutputSeq1 and psd2.SEQ2 = psd.OutputSeq2
left join MDivisionPoDetail mpd2 on mpd2.MDivisionId = l.MDivisionID and mpd2.POID = l.POID and mpd2.Seq1 = psd.OutputSeq1 and mpd2.Seq2 = psd.OutputSeq2
left join Invtrans i on i.PoID = l.POID and i.Seq1 = ld.Seq1 and i.Seq2 = ld.Seq2 and i.Type = 1
left join Invtrans i2 on i2.InventoryPOID = l.POID and i2.InventorySeq1 = ld.Seq1 and i2.InventorySeq2 = ld.Seq2 and i2.Type = 4
where l.ApvDate between '{0}' and '{1}'
and l.Type = 'R'", Convert.ToDateTime(apvDate1).ToString("d"), Convert.ToDateTime(apvDate2).ToString("d")));

            if (!MyUtility.Check.Empty(reportType))
            {
                sqlCmd.Append(string.Format(" and l.FabricType = '{0}'", reportType));
            }

            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and l.MDivisionID = '{0}'", mDivision));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and l.FactoryID = '{0}'", factory));
            }

            sqlCmd.Append(@" group by l.MDivisionID,l.FactoryID,l.POID,ld.Seq1,ld.Seq2,isnull(psd.Refno,''),isnull(f.MtlTypeID,''),ld.RequestQty,
isnull(mpd.InQty,0)+isnull(mpd2.InQty ,0),isnull(psd.NETQty,0),isnull(psd2.NETQty,0),
isnull(psd.POUnit,''),isnull(psd.StockUnit,''),isnull(psd2.POUnit,''),isnull(psd2.StockUnit,'')
),
tmpData2
as (
select MDivisionID,FactoryID,POID,Seq1,Seq2,Refno,MtlTypeID,RequestQty,InQty,
NETQty1*IIF(POUnit is null or StockUnit is null,1, dbo.getUnitRate(POUnit,StockUnit))+
NETQty2*IIF(INVPOUnit is null or INVStockUnit is null,1, dbo.getUnitRate(INVPOUnit,INVStockUnit)) as NETQty,
StockQty1*IIF(POUnit is null or StockUnit is null,1, dbo.getUnitRate(POUnit,StockUnit))+
StockQty2*IIF(INVPOUnit is null or INVStockUnit is null,1, dbo.getUnitRate(INVPOUnit,INVStockUnit)) as StockQty
from tmpData)
select MDivisionID,FactoryID,POID,Seq1,Seq2,Refno,MtlTypeID,Sum(RequestQty) as RequestQty,InQty,NETQty,StockQty
from tmpData2
group by MDivisionID,FactoryID,POID,Seq1,Seq2,Refno,MtlTypeID,InQty,NETQty,StockQty
order by MDivisionID,FactoryID,POID,Seq1,Seq2");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R05_AllowanceConsumptionReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[3, 3] = string.Format("{0}~{1}", MyUtility.Check.Empty(apvDate1) ? "" : Convert.ToDateTime(apvDate1).ToString("d"), MyUtility.Check.Empty(apvDate2) ? "" : Convert.ToDateTime(apvDate2).ToString("d"));
            worksheet.Cells[4, 3] = reportTypeName;
            worksheet.Cells[3, 8] = mDivision;
            worksheet.Cells[4, 8] = factory;
            worksheet.Cells[3, 12] = DateTime.Today.ToString("d");

            //填內容值
            int intRowsStart = 6;
            object[,] objArray = new object[1, 12];
            foreach (DataRow dr in printData.Rows)
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
