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
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory order by FTYGroup", out factory);
            factory.Rows.Add(new string[] { "" });
            factory.DefaultView.Sort = "FTYGroup";
            MyUtility.Tool.SetupCombox(comboBox1, 1, factory);
            comboBox1.Text = Sci.Env.User.Factory;
            this.S_radioButton.Checked = true;

        }
        // 驗證輸入條件 必須要有
        protected override bool ValidateInput()
        {
            if (S_radioButton.Checked==false && d_radioButton.Checked==false)
            {
                MyUtility.Msg.InfoBox("<Format> you have to choice one!  ");
                return false;
            }
            id1 = textBox1.Text;
            id2 = textBox2.Text;
            cdate1 = dateBox1.Value;
            cdate2 = dateBox2.Value;
            bdate1 = dateBox3.Value;
            bdate2 = dateBox4.Value;
            factory = comboBox1.Text;
            brand = textBox7.Text;
            return base.ValidateInput();
        }
        // 非同步資料 必須要有
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd_Summary = new StringBuilder();
           
            #region 組撈Summary Data SQL
            sqlCmd_Summary.Append(
@"select 
c.StyleID
,a.OrderID
,iif(c.VasShas=0,'','v') as 'VAS/SHAS'
,c.BrandID
,c.CustPONo
,a.cDate
,a.GarmentOutput
,a.FactoryID
,a.SewingLineID
,case a.shift 
when 'D' then 'night'
when 'N' then 'NIGHT'
when 'O' then 'SUBCON OUT'
when 'I' then 'SUBCON IN'
else '' end as Shift
,a.Team
,c.Qty
,DBO.GETPASS1(a.CFA) as CFA
,case a.Stage when 'I' then 'Comments/Roving'
when 'C' then 'Change Over'
when 'P' then 'Stagger'
when 'R' then 'Re-Stagger'
when 'F' then 'Final'
when 'B' then 'Buyer'
else ''  end as Stage
,a.Result
,a.InspectQty
,a.DefectQty
,iif(a.InspectQty=0,0,round(a.DefectQty/a.InspectQty*100,3)) [SQR]
from dbo.Cfa a inner join dbo.Cfa_Detail b
on b.id = a.ID 
inner join dbo.orders c
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
            if(!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion
            StringBuilder sqlCmd_Detail = new StringBuilder();

            #region 組撈Detail Data SQL
            sqlCmd_Detail.Append(
@"select 
c.FactoryID
,a.OrderID
,iif(c.VasShas=0,'','v') as 'VAS/SHAS'
,c.StyleID
,c.BrandID
,c.CustPONo
,a.cDate
,a.GarmentOutput
,a.SewingLineID
,case a.shift 
when 'D' then 'night'
when 'N' then 'NIGHT'
when 'O' then 'SUBCON OUT'
when 'I' then 'SUBCON IN'
else '' end as Shift
,a.Team
,c.Qty
,DBO.GETPASS1(a.CFA) as CFA
,case a.Stage when 'I' then 'Comments/Roving'
when 'C' then 'Change Over'
when 'P' then 'Stagger'
when 'R' then 'Re-Stagger'
when 'F' then 'Final'
when 'B' then 'Buyer'
else ''  end as Stage
,a.Result
,a.InspectQty
,a.DefectQty
,iif(a.InspectQty=0,0,round(a.DefectQty/a.InspectQty*100,3)) [SQR]
,'' as Area
from dbo.Cfa a inner join dbo.Cfa_Detail b
on b.id = a.ID 
inner join dbo.orders c
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
     
            DualResult result1 = DBProxy.Current.Select(null, sqlCmd_Detail.ToString(), out DetailData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result1.ToString());
                return failResult;

            }
            #endregion
            return Result.True;
        }
        // 產生Excel 必須要有
        protected override bool OnToExcel(Win.ReportDefinition report)
        {   
            this.ShowWaitMessage("Starting Excel");
            if (S_radioButton.Checked)
            {
                // 顯示筆數於PrintForm上Count欄位
                SetCount(SummaryData.Rows.Count);
                if (SummaryData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFM_InlineReport_Summary.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(SummaryData,"","Quality_R21_CFM_InlineReport_Summary.xltx",2,true,null,  excel);
                Microsoft.Office.Interop.Excel.Worksheet excelSheets = excel.ActiveWorkbook.Worksheets[1];// 取得工作表                 

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.EntireRow.AutoFit();

                if (excelSheets != null) Marshal.FinalReleaseComObject(excelSheets);//釋放sheet
                if (excel != null) Marshal.FinalReleaseComObject(excel);
                
            }
            if (d_radioButton.Checked)
            {
                // 顯示筆數於PrintForm上Count欄位
                SetCount(DetailData.Rows.Count);
                if (DetailData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }
                Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R21_CFA_InlineReport_detail.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(DetailData,"", "Quality_R21_CFA_InlineReport_detail.xltx", 2, true, null, excel);
                Microsoft.Office.Interop.Excel.Worksheet excelSheets = excel.ActiveWorkbook.Worksheets[1];// 取得工作表

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.EntireRow.AutoFit();

                 if (excelSheets != null) Marshal.FinalReleaseComObject(excelSheets);//釋放sheet
                 if (excel != null) Marshal.FinalReleaseComObject(excel);     
            }
            
            this.HideWaitMessage();
            return true;
        }
    }
    
}
