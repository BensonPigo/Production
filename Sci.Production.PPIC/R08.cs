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
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? cdate1, cdate2, apvdate1, apvdate2;
        string mDivision, factory, type, typedesc;
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "Fabric,Accessory,");
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox3, 1, factory);
            comboBox1.Text = Sci.Env.User.Keyword;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = -1;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            cdate1 = dateRange1.Value1;
            cdate2 = dateRange1.Value2;
            apvdate1 = dateRange2.Value1;
            apvdate2 = dateRange2.Value2;
            mDivision = comboBox1.Text;
            type = comboBox2.SelectedIndex == -1 || comboBox2.SelectedIndex == 2 ? "" : comboBox2.SelectedIndex == 0 ? "F":"A";
            factory = comboBox3.Text;
            typedesc = comboBox2.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select r.ID,r.CDate,r.ApvDate,r.POID,r.MDivisionID,r.FactoryID,isnull(o.StyleID,'') as StyleID,
rd.Seq1+'-'+rd.Seq2 as Seq,IIF(r.Type='F','Fabric','Accessory') as Type,isnull(f.MtlTypeID,'') as MtlTypeID,
rd.Refno,isnull(f.DescDetail,'') as DescDetail,rd.ColorID,rd.EstInQty,rd.ActInQty,rd.TotalRequest,
rd.AfterCuttingRequest,IIF(rd.Responsibility='M','Mill',IIF(rd.Responsibility = 'S','Subcon in Local',IIF(rd.Responsibility = 'F','Factory',IIF(rd.Responsibility = 'T','SCI dep. (purchase / s. mrs / sample room)','')))) as Responsibility,
rd.ResponsibilityReason,rd.Suggested,IIF(p.POSMR is null,'',dbo.getTPEPass1(p.POSMR)) as POSMR,
dbo.getPass1(r.ApplyName) as Prepare
from ReplacementReport r
inner join ReplacementReport_Detail rd on rd.ID = r.ID
left join Orders o on o.ID = r.POID
left join Fabric f on f.SCIRefno = rd.SCIRefno
left join PO p on p.ID = r.POID
where 1=1");

            if (!MyUtility.Check.Empty(cdate1))
            {
                sqlCmd.Append(string.Format(" and r.CDate >= '{0}'", Convert.ToDateTime(cdate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(cdate2))
            {
                sqlCmd.Append(string.Format(" and r.CDate <= '{0}'", Convert.ToDateTime(cdate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(apvdate1))
            {
                sqlCmd.Append(string.Format(" and r.ApvDate >= '{0}'", Convert.ToDateTime(apvdate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(apvdate2))
            {
                sqlCmd.Append(string.Format(" and r.ApvDate <= '{0}'", Convert.ToDateTime(apvdate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and r.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and r.FactoryID = '{0}'", factory));
            }
            if (!MyUtility.Check.Empty(type))
            {
                sqlCmd.Append(string.Format(" and r.Type = '{0}'", type));
            }
            sqlCmd.Append(" order by r.ID,Seq");

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
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R08_ReplacementReportList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[2, 2] = string.Format("{0}~{1}", MyUtility.Check.Empty(cdate1) ? "" : Convert.ToDateTime(cdate1).ToString("d"),MyUtility.Check.Empty(cdate2) ? "" : Convert.ToDateTime(cdate2).ToString("d"));
            worksheet.Cells[2, 6] = string.Format("{0}~{1}", MyUtility.Check.Empty(apvdate1) ? "" : Convert.ToDateTime(apvdate1).ToString("d"), MyUtility.Check.Empty(apvdate2) ? "" : Convert.ToDateTime(apvdate2).ToString("d"));
            worksheet.Cells[2, 10] = mDivision;
            worksheet.Cells[2, 12] = factory;
            worksheet.Cells[2, 14] = typedesc;

            //填內容值
            int intRowsStart = 4;
            object[,] objArray = new object[1, 22];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["CDate"];
                objArray[0, 2] = dr["ApvDate"];
                objArray[0, 3] = dr["POID"];
                objArray[0, 4] = dr["MDivisionID"];
                objArray[0, 5] = dr["FactoryID"];
                objArray[0, 6] = dr["StyleID"];
                objArray[0, 7] = dr["Seq"];
                objArray[0, 8] = dr["Type"];
                objArray[0, 9] = dr["MtlTypeID"];
                objArray[0, 10] = dr["Refno"];
                objArray[0, 11] = dr["DescDetail"];
                objArray[0, 12] = dr["ColorID"];
                objArray[0, 13] = dr["EstInQty"];
                objArray[0, 14] = dr["ActInQty"];
                objArray[0, 15] = dr["TotalRequest"];
                objArray[0, 16] = dr["AfterCuttingRequest"];
                objArray[0, 17] = dr["Responsibility"];
                objArray[0, 18] = dr["ResponsibilityReason"];
                objArray[0, 19] = dr["Suggested"];
                objArray[0, 20] = dr["POSMR"];
                objArray[0, 21] = dr["Prepare"];
                worksheet.Range[String.Format("A{0}:V{0}", intRowsStart)].Value2 = objArray;
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
