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

namespace Sci.Production.PPIC
{
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        DataTable _printData;
        DateTime? _sciDate1, _sciDate2;
        string _mDivision, _orderType, _factory, _category;
        int _excludeReplacement, _complection;
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            comboM.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(comboOrderType, 1, 1, "Bulk,Sample,Bulk+Sample,Material");
            comboOrderType.Text = "Bulk+Sample";
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory; 
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("SCI Delivery can't empty!!");
                return false;
            }
            _sciDate1 = dateSCIDelivery.Value1;
            _sciDate2 = dateSCIDelivery.Value2;
            _mDivision = comboM.Text;
            _orderType = comboOrderType.SelectedIndex == -1 ? "" : comboOrderType.SelectedIndex == 0 ? "B" : comboOrderType.SelectedIndex == 1 ? "S" : comboOrderType.SelectedIndex == 2 ? "BS" : "M";
            _excludeReplacement = checkExcludedReplacementItem.Checked ? 1 : 0;
            _complection = checkPOMaterialCompletion.Checked ? 1 : 0;
            _factory = comboFactory.Text;
            _category = comboOrderType.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
with tmpPO as (
    select ps.ID,ps.SEQ1,Qty,psd.ETA,s.ThirdCountry,isnull(s.AbbEN,'') as SuppAbb
    from PO_Supp ps WITH (NOLOCK) 
    inner join (
        select a.ID,a.SEQ1,sum(a.Qty) as Qty,max(a.ETA) as ETA 
        from 
        (
            select psd.ID,psd.SEQ1,psd.SEQ2,psd.FabricType,psd.ETA,
            IIF(psd.FabricType = 'F',psd.Qty-isnull((select sum(ed.Qty) from Export_Detail ed WITH (NOLOCK) where ed.PoID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2),0),0) as Qty
            from PO_Supp_Detail psd WITH (NOLOCK) 
            where psd.Junk = 0
            and psd.Complete = 0
        {0}
        ) a
        group by a.ID,a.SEQ1
    ) psd on psd.ID = ps.ID and psd.SEQ1 = ps.SEQ1
    left join Supp s WITH (NOLOCK) on s.ID = ps.SuppID
),PrepareData1 as (
    select ID,ThirdCountry,SEQ1+'-'+SuppAbb as Seq,
    IIF(Qty > 0,IIF(ETA is null,'',CONVERT(VARCHAR(2),Month(ETA))+'/'+CONVERT(VARCHAR(2),DAY(ETA)))+'-'+CONVERT(VARCHAR(10),Qty)+isnull((select top 1 psd.POUnit from PO_Supp_Detail psd WITH (NOLOCK) 
    where psd.ID = tmpPO.ID and psd.SEQ1 = tmpPO.SEQ1 and psd.FabricType = 'F' and psd.Junk = 0 and psd.Complete = 0 and psd.POUnit <> '' {0}),''),'') as Qty
    from tmpPO
),PrepareData2 as (
    select ID,ThirdCountry,Seq+IIF(Qty = '','','('+Qty+')') as Seq
    from PrepareData1
)
select o.ID,o.StyleID,
Category = 
    (CASE WHEN o.Category = 'B' THEN 'Bulk'
        WHEN o.Category = 'S' THEN 'Sample'
        WHEN o.Category = 'O' THEN 'Other'
        WHEN o.Category = 'M' THEN 'Material'
        END)
,o.SeasonID,o.SewInLine,o.LETA,o.KPILETA,o.BuyerDelivery,o.SciDelivery,
o.BrandID,o.CPU,o.SewETA,o.PackETA,o.MDivisionID,o.FactoryID,dbo.getPass1(o.LocalMR) as LocalMR,
dbo.getTPEPass1(o.MCHandle) as MCHandle,dbo.getTPEPass1(o.MRHandle) as MRHandle,
dbo.getTPEPass1(o.SMR) as SMR,dbo.getTPEPass1(p.POSMR) as POSMR,
dbo.getTPEPass1(p.POHandle) as POHandle,o.Qty,o.CPU*o.Qty*o.CPUFactor as tCPU,o.MTLComplete,
isnull((select CONCAT(Seq,';') from PrepareData2 where ID = o.POID and ThirdCountry = 0 for XML PATH('')),'') as SeqNo,
isnull((select CONCAT(Seq,';') from PrepareData2 where ID = o.POID and ThirdCountry = 1 for XML PATH('')),'') as Seq3rd
from Orders o WITH (NOLOCK) 
left join PO p WITH (NOLOCK) on p.ID = o.POID
where 1=1", _excludeReplacement == 1?"and psd.SEQ1 not between '50' and '69'":""));

            if (!MyUtility.Check.Empty(_sciDate1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(_sciDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(_sciDate2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(_sciDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(_mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", _mDivision));
            }

            if (!MyUtility.Check.Empty(_factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", _factory));
            }

            if (!MyUtility.Check.Empty(_orderType))
            {
                if (_orderType == "B")
                {
                    sqlCmd.Append(" and o.Category = 'B'");
                }
                else if (_orderType == "S")
                {
                    sqlCmd.Append(" and o.Category = 'S'");
                }
                else if (_orderType == "M")
                {
                    sqlCmd.Append(" and o.Category = 'M'");
                }
                else
                {
                    sqlCmd.Append(" and (o.Category = 'B' or o.Category ='S')");
                }
            }

            sqlCmd.Append(" order by o.ID");

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
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R06_MonthlyMaterialCompletion.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[2, 2] = string.Format("{0}~{1}", MyUtility.Check.Empty(_sciDate1) ? "" : Convert.ToDateTime(_sciDate1).ToString("d"), MyUtility.Check.Empty(_sciDate2) ? "" : Convert.ToDateTime(_sciDate2).ToString("d"));
            
            worksheet.Cells[2, 6] = _mDivision;
            worksheet.Cells[2, 8] = _factory;
            worksheet.Cells[2, 10] = _category;
            worksheet.Cells[2, 13] = _excludeReplacement == 1 ? "True" : "False";
            worksheet.Cells[2, 15] = _complection == 1 ? "True" : "False";

            //填內容值
            int intRowsStart = 4;
            object[,] objArray = new object[1, 27];
            foreach (DataRow dr in _printData.Rows)
            {
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["Category"];
                objArray[0, 3] = dr["SeasonID"];
                objArray[0, 4] = dr["SewInLine"];
                objArray[0, 5] = dr["LETA"];
                objArray[0, 6] = dr["KPILETA"];
                objArray[0, 7] = dr["BuyerDelivery"];
                objArray[0, 8] = dr["SciDelivery"];
                objArray[0, 9] = dr["BrandID"];
                objArray[0, 10] = dr["CPU"];
                objArray[0, 11] = dr["SeqNo"];
                objArray[0, 12] = dr["Seq3rd"];
                objArray[0, 13] = dr["SewETA"];
                objArray[0, 14] = dr["PackETA"];
                objArray[0, 15] = dr["MDivisionID"];
                objArray[0, 16] = dr["FactoryID"];
                objArray[0, 17] = dr["LocalMR"];
                objArray[0, 18] = dr["MCHandle"];
                objArray[0, 19] = dr["MRHandle"];
                objArray[0, 20] = dr["SMR"];
                objArray[0, 21] = dr["POHandle"];
                objArray[0, 22] = dr["POSMR"];
                objArray[0, 23] = dr["Qty"];
                objArray[0, 24] = dr["tCPU"];
                objArray[0, 25] = _complection == 1 && MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : MyUtility.Check.Empty(dr["SeqNo"]) && MyUtility.Check.Empty(dr["Seq3rd"]) ? "Y" : "N";
                objArray[0, 26] = MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "FALSE" ? "" : "Y"; ;
                worksheet.Range[String.Format("A{0}:AA{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R06_MonthlyMaterialCompletion");
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
