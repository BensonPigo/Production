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
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        DateTime? cdate1,cdate2,bdate1,bdate2;
        string factory, id1, id2, brand;
        DataTable SummaryData,DetailData;

        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            factory.Rows.Add(new string[] { "" });
            factory.DefaultView.Sort = "FTYGroup";
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            this.radioSummary.Checked = true;

        }

        // 驗證輸入條件 必須要有
        protected override bool ValidateInput()
        {
            if (radioSummary.Checked==false && radiobyDetail.Checked==false)
            {
                MyUtility.Msg.InfoBox("<Format> you have to choice one!  ");
                return false;
            }
            id1 = txtSPStart.Text;
            id2 = txtSPEnd.Text;
            cdate1 = dateAuditDateStart.Value;
            cdate2 = dateAuditDateEnd.Value;
            bdate1 = dateBuyerDeliveryStart.Value;
            bdate2 = dateBuyerDeliveryEnd.Value;
            factory = comboFactory.Text;
            brand = txtBrand.Text;
            return base.ValidateInput();
        }

        // 非同步資料 必須要有
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd_Summary = new StringBuilder();
            if (radioSummary.Checked)
            {
                #region 組撈Summary Data SQL
                sqlCmd_Summary.Append(@"
select DISTINCT c.StyleID
	   , a.OrderID
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
on c.id = a.OrderID
where a.Status = 'Confirmed'");
                if (!MyUtility.Check.Empty(id1))
                {
                    sqlCmd_Summary.Append(string.Format("and a.OrderID >= '{0}'", id1));
                }
                if (!MyUtility.Check.Empty(id2))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.OrderID <= '{0}'", id2));
                }
                if (!MyUtility.Check.Empty(cdate1))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.cDate >= '{0}'", Convert.ToDateTime(cdate1).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(cdate2))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.cDate <= '{0}'", Convert.ToDateTime(cdate2).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(bdate1))
                {
                    sqlCmd_Summary.Append(string.Format(" and c.BuyerDelivery >= '{0}'", Convert.ToDateTime(bdate1).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(bdate2))
                {
                    sqlCmd_Summary.Append(string.Format(" and c.BuyerDelivery <= '{0}'", Convert.ToDateTime(bdate2).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(factory))
                {
                    sqlCmd_Summary.Append(string.Format(" and a.FactoryID = '{0}'", factory));
                }
                if (!MyUtility.Check.Empty(brand))
                {
                    sqlCmd_Summary.Append(string.Format(" and c.brandID = '{0}'", brand));
                }

                DualResult result = DBProxy.Current.Select(null, sqlCmd_Summary.ToString(), out SummaryData);
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
,[VAS/SHAS]= iif(c.VasShas=0,'','v') 
,c.StyleID
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
outer apply(select Description from dbo.GarmentDefectCode a where a.id=b.GarmentDefectCodeID) as gd
outer apply(select description from dbo.cfaarea a where a.id=b.CFAAreaID) as ar
where a.Status = 'Confirmed'");
                if (!MyUtility.Check.Empty(id1))
                {
                    sqlCmd_Detail.Append(string.Format("and a.OrderID >= '{0}'", id1));
                }
                if (!MyUtility.Check.Empty(id2))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.OrderID <= '{0}'", id2));
                }
                if (!MyUtility.Check.Empty(cdate1))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.cDate >= '{0}'", Convert.ToDateTime(cdate1).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(cdate2))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.cDate <= '{0}'", Convert.ToDateTime(cdate2).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(bdate1))
                {
                    sqlCmd_Detail.Append(string.Format(" and c.BuyerDelivery >= '{0}'", Convert.ToDateTime(bdate1).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(bdate2))
                {
                    sqlCmd_Detail.Append(string.Format(" and c.BuyerDelivery <= '{0}'", Convert.ToDateTime(bdate2).ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(factory))
                {
                    sqlCmd_Detail.Append(string.Format(" and a.FactoryID = '{0}'", factory));
                }
                if (!MyUtility.Check.Empty(brand))
                {
                    sqlCmd_Detail.Append(string.Format(" and c.brandID = '{0}'", brand));
                }

                DualResult result1 = DBProxy.Current.Select(null, sqlCmd_Detail.ToString(), out DetailData);
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
            if (radioSummary.Checked)
            {
                // 顯示筆數於PrintForm上Count欄位
                SetCount(SummaryData.Rows.Count);
                if (SummaryData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_Summary.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(SummaryData, "", "Quality_R21_CFA_InlineReport_Summary.xltx", 2, false, null, excel);            

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
            if (radiobyDetail.Checked)
            {
                // 顯示筆數於PrintForm上Count欄位
                SetCount(DetailData.Rows.Count);
                if (DetailData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }         
                //Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_detail.xltx"); //預先開啟excel app
               // MyUtility.Excel.CopyToXls(DetailData,"", "Quality_R21_CFA_InlineReport_detail.xltx", 2, false, null, excel);
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_detail.xltx"); 
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_detail.xltx", objApp);
                com.WriteTable(DetailData, 3);

               
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
