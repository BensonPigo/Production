using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        DateTime? cdate1;
        DateTime? cdate2;
        DateTime? bdate1;
        DateTime? bdate2;
        string factory;
        string id1;
        string id2;
        string brand;
        DataTable SummaryData;
        DataTable DetailData;

        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            factory.Rows.Add(new string[] { string.Empty });
            factory.DefaultView.Sort = "FTYGroup";
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.radioSummary.Checked = true;
        }

        // 驗證輸入條件 必須要有
        protected override bool ValidateInput()
        {
            if (this.radioSummary.Checked == false && this.radiobyDetail.Checked == false)
            {
                MyUtility.Msg.InfoBox("<Format> you have to choice one!  ");
                return false;
            }

            this.id1 = this.txtSPStart.Text;
            this.id2 = this.txtSPEnd.Text;
            this.cdate1 = this.dateAuditDateStart.Value;
            this.cdate2 = this.dateAuditDateEnd.Value;
            this.bdate1 = this.dateBuyerDeliveryStart.Value;
            this.bdate2 = this.dateBuyerDeliveryEnd.Value;
            this.factory = this.comboFactory.Text;
            this.brand = this.txtBrand.Text;
            return base.ValidateInput();
        }

        // 非同步資料 必須要有
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd_Summary = new StringBuilder();
            if (this.radioSummary.Checked)
            {
                #region 組撈Summary Data SQL
                sqlCmd_Summary.Append(@"
select DISTINCT c.StyleID
       , c.BuyerDelivery 
	   , a.OrderID
	   , [Destination]=ct.Alias
	   , 'VAS/SHAS' = iif(c.VasShas=0,'','v')
	   , c.BrandID
	   , c.CustPONo
	   , a.cDate
	   , [GarmentOutput] = round(a.GarmentOutput/100,2)
	   , a.FactoryID
	   , a.SewingLineID
	   , Shift = case a.shift 
				   when 'D' then 'DAY'
				   when 'N' then 'NIGHT'
				   when 'O' then 'SUBCON OUT'
				   when 'I' then 'SUBCON IN'
				   else '' 
				 end
	   , a.Team
	   , c.Qty
	   , CFA = DBO.GETPASS1(a.CFA)
	   , Stage = case a.Stage 
				   when 'I' then 'Comments/Roving'
				   when 'C' then 'Change Over'
				   when 'P' then 'Stagger'
				   when 'R' then 'Re-Stagger'
				   when 'F' then 'Final'
				   when 'B' then 'Buyer'
				   else ''  
				 end 
	   , [Result]= case a.result 
					   when 'P' then 'Pass'
					   when 'F' then 'Fail'
					   else '' 
				   end 
	   , a.InspectQty
	   , a.DefectQty
	   , [SQR] = iif(a.InspectQty=0,0,round(a.DefectQty/a.InspectQty,3)) 
	   , Remark = a.Remark
from dbo.Cfa a WITH (NOLOCK) 
inner join dbo.orders c WITH (NOLOCK) 
INNEr JOIN Country ct ON ct.ID=c.Dest
on c.id = a.OrderID
where a.Status = 'Confirmed'");
                if (!MyUtility.Check.Empty(this.id1))
                {
                    sqlCmd_Summary.Append(string.Format("and a.OrderID >= '{0}'", this.id1));
                }

                if (!MyUtility.Check.Empty(this.id2))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.OrderID <= '{0}'", this.id2));
                }

                if (!MyUtility.Check.Empty(this.cdate1))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.cDate >= '{0}'", Convert.ToDateTime(this.cdate1).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.cdate2))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.cDate <= '{0}'", Convert.ToDateTime(this.cdate2).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.bdate1))
                {
                    sqlCmd_Summary.Append(string.Format(" and c.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.bdate1).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.bdate2))
                {
                    sqlCmd_Summary.Append(string.Format(" and c.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.bdate2).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.FactoryID = '{0}'", this.factory));
                }

                if (!MyUtility.Check.Empty(this.brand))
                {
                    sqlCmd_Summary.Append(string.Format(" and c.brandID = '{0}'", this.brand));
                }

                DualResult result = DBProxy.Current.Select(null, sqlCmd_Summary.ToString(), out this.SummaryData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion
            }
            else
            {
                StringBuilder sqlCmd_Detail = new StringBuilder();

                #region 組撈Detail Data SQL
                sqlCmd_Detail.Append(
    @"
select DISTINCT
c.FactoryID
,a.OrderID
, [Destination]=ct.Alias
,[VAS/SHAS]= iif(c.VasShas=0,'','v') 
,c.StyleID
, c.BuyerDelivery 
,c.BrandID
,c.CustPONo
,a.cDate
,[GarmentOutput]= round(a.GarmentOutput/100,2)
,a.SewingLineID
,[shift]= 
case a.shift 
when 'D' then 'DAY'
when 'N' then 'NIGHT'
when 'O' then 'SUBCON OUT'
when 'I' then 'SUBCON IN'
else '' end
,a.Team
,c.Qty
,[Cfa]= DBO.GETPASS1(a.CFA)
,[Stage]= 
case a.Stage when 'I' then 'Comments/Roving'
when 'C' then 'Change Over'
when 'P' then 'Stagger'
when 'R' then 'Re-Stagger'
when 'F' then 'Final'
when 'B' then 'Buyer'
else ''  end
,[Result]= 
case a.result when 'P' then 'Pass'
when 'F' then 'Fail'
else '' end 
,a.InspectQty
,a.DefectQty
,[SQR]= iif(a.InspectQty=0,0,round(a.DefectQty/a.InspectQty,3)) 
,[Defect Description]= gd.Description
,[Area]= b.CFAAreaID +' - '+ar.Description
,[No. Of Defect]=b.Qty
,[Remark]= b.Remark
,[Action]= b.Action
from dbo.Cfa a WITH (NOLOCK) 
inner join dbo.Cfa_Detail b WITH (NOLOCK) 
on b.id = a.ID 
inner join dbo.orders c WITH (NOLOCK) 
on c.id = a.OrderID
INNEr JOIN Country ct WITH (NOLOCK)  
ON ct.ID=c.Dest
outer apply(select Description from dbo.GarmentDefectCode a where a.id=b.GarmentDefectCodeID) as gd
outer apply(select description from dbo.cfaarea a where a.id=b.CFAAreaID) as ar
where a.Status = 'Confirmed'");
                if (!MyUtility.Check.Empty(this.id1))
                {
                    sqlCmd_Detail.Append(string.Format("and a.OrderID >= '{0}'", this.id1));
                }

                if (!MyUtility.Check.Empty(this.id2))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.OrderID <= '{0}'", this.id2));
                }

                if (!MyUtility.Check.Empty(this.cdate1))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.cDate >= '{0}'", Convert.ToDateTime(this.cdate1).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.cdate2))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.cDate <= '{0}'", Convert.ToDateTime(this.cdate2).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.bdate1))
                {
                    sqlCmd_Detail.Append(string.Format(" and c.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.bdate1).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.bdate2))
                {
                    sqlCmd_Detail.Append(string.Format(" and c.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.bdate2).ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.FactoryID = '{0}'", this.factory));
                }

                if (!MyUtility.Check.Empty(this.brand))
                {
                    sqlCmd_Detail.Append(string.Format(" and c.brandID = '{0}'", this.brand));
                }

                DualResult result1 = DBProxy.Current.Select(null, sqlCmd_Detail.ToString(), out this.DetailData);
                if (!result1)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result1.ToString());
                    return failResult;
                }
                #endregion
            }

            return Result.True;
        }

        // 產生Excel 必須要有
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.ShowWaitMessage("Starting Excel");
            if (this.radioSummary.Checked)
            {
                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.SummaryData.Rows.Count);
                if (this.SummaryData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_Summary.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.SummaryData, string.Empty, "Quality_R21_CFA_InlineReport_Summary.xltx", 2, false, null, excel);

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.EntireRow.AutoFit();

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R21_CFA_InlineReport_Summary");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);

                strExcelName.OpenFile();
                #endregion
            }

            if (this.radiobyDetail.Checked)
            {
                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.DetailData.Rows.Count);
                if (this.DetailData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                // Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_detail.xltx"); //預先開啟excel app
               // MyUtility.Excel.CopyToXls(DetailData,"", "Quality_R21_CFA_InlineReport_detail.xltx", 2, false, null, excel);
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_detail.xltx");
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_detail.xltx", objApp);
                com.WriteTable(this.DetailData, 3);

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R21_CFA_InlineReport_detail");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                strExcelName.OpenFile();
                #endregion
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
