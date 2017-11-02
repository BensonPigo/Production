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
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R08
    /// </summary>
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        private DataTable _printData;
        private DateTime? _cdate1;
        private DateTime? _cdate2;
        private DateTime? _apvdate1;
        private DateTime? _apvdate2;
        private string _mDivision;
        private string _factory;
        private string _type;
        private string _typedesc;

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            MyUtility.Tool.SetupCombox(this.comboType, 1, 1, "Fabric,Accessory,");
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Sci.Env.User.Keyword;
            this.comboType.SelectedIndex = 0;
            this.comboFactory.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this._cdate1 = this.dateCreateDate.Value1;
            this._cdate2 = this.dateCreateDate.Value2;
            this._apvdate1 = this.dateApvDate.Value1;
            this._apvdate2 = this.dateApvDate.Value2;
            this._mDivision = this.comboM.Text;
            this._type = this.comboType.SelectedIndex == -1 || this.comboType.SelectedIndex == 2 ? string.Empty : this.comboType.SelectedIndex == 0 ? "F" : "A";
            this._factory = this.comboFactory.Text;
            this._typedesc = this.comboType.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region sqlcmd 主table
            sqlCmd.Append(@"
select r.ID,r.CDate,r.ApvDate,r.POID,r.MDivisionID,r.FactoryID,o.StyleID,
[Seq] = CONCAT(rd.Seq1,'-',rd.Seq2),
[Type] = IIF(r.Type='F','Fabric','Accessory'),
f.MtlTypeID,rd.Refno,
f.DescDetail,rd.ColorID,rd.EstInQty,rd.ActInQty,rd.TotalRequest,rd.AfterCuttingRequest,
[Responsibility] = 
	CASE WHEN rd.Responsibility = 'M' THEN 'Mill'
		 WHEN rd.Responsibility = 'S' THEN 'Subcon in Local'
		 WHEN rd.Responsibility = 'F' THEN 'Factory'
		 WHEN rd.Responsibility = 'T' THEN 'SCI dep. (purchase / s. mrs / sample room)'
	END,
rd.ResponsibilityReason,rd.Suggested,
[POSMR] = dbo.[getTPEPass1_ExtNo](p.POSMR),
[Prepare] = dbo.[getPass1_ExtNo](r.ApplyName)
from ReplacementReport r WITH (NOLOCK) 
inner join ReplacementReport_Detail rd WITH (NOLOCK) on rd.ID = r.ID
left join Orders o WITH (NOLOCK) on o.ID = r.POID
left join Fabric f WITH (NOLOCK) on f.SCIRefno = rd.SCIRefno
left join PO p WITH (NOLOCK) on p.ID = r.POID
where 1=1");
            #endregion
            #region 使用者輸入條件
            if (!MyUtility.Check.Empty(this._cdate1))
            {
                sqlCmd.Append(string.Format(" and r.CDate >= '{0}'", Convert.ToDateTime(this._cdate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this._cdate2))
            {
                sqlCmd.Append(string.Format(" and r.CDate <= '{0}'", Convert.ToDateTime(this._cdate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this._apvdate1))
            {
                sqlCmd.Append(string.Format(" and r.ApvDate >= '{0}'", Convert.ToDateTime(this._apvdate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this._apvdate2))
            {
                sqlCmd.Append(string.Format(" and r.ApvDate <= '{0}'", Convert.ToDateTime(this._apvdate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this._mDivision))
            {
                sqlCmd.Append(string.Format(" and r.MDivisionID = '{0}'", this._mDivision));
            }

            if (!MyUtility.Check.Empty(this._factory))
            {
                sqlCmd.Append(string.Format(" and r.FactoryID = '{0}'", this._factory));
            }

            if (!MyUtility.Check.Empty(this._type))
            {
                sqlCmd.Append(string.Format(" and r.Type = '{0}'", this._type));
            }
            #endregion
            sqlCmd.Append(" order by r.ID,Seq");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this._printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
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
            Microsoft.Office.Interop.Excel.Application excel
                = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R08_ReplacementReportList.xltx"); // 預先開啟excel app
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 表頭
            string strfactory = string.Empty;
            if (!MyUtility.Check.Empty(this.comboFactory.Text))
            {
                strfactory = this.comboFactory.Text;
            }
            else
            {
            strfactory = Sci.Env.User.Factory;
            }

            worksheet.Cells[1, 1] = MyUtility.GetValue.Lookup("NameEN", strfactory, "Factory", "ID", "Production");
            worksheet.Cells[3, 2] = string.Format("{0}~{1}", MyUtility.Check.Empty(this._cdate1) ? string.Empty : Convert.ToDateTime(this._cdate1).ToString("d"), MyUtility.Check.Empty(this._cdate2) ? string.Empty : Convert.ToDateTime(this._cdate2).ToString("d"));
            worksheet.Cells[3, 5] = string.Format("{0}~{1}", MyUtility.Check.Empty(this._apvdate1) ? string.Empty : Convert.ToDateTime(this._apvdate1).ToString("d"), MyUtility.Check.Empty(this._apvdate2) ? string.Empty : Convert.ToDateTime(this._apvdate2).ToString("d"));
            worksheet.Cells[3, 7] = "M: " + this._mDivision;
            worksheet.Cells[3, 9] = this._factory;
            worksheet.Cells[3, 11] = this._typedesc;

            // 填內容值
            int intRowsStart = 5;
            object[,] objArray = new object[1, 22];
            foreach (DataRow dr in this._printData.Rows)
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
                worksheet.Range[string.Format("A{0}:V{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R08_ReplacementReportList");
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
