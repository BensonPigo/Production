﻿using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R11
    /// </summary>
    public partial class R11 : Win.Tems.PrintForm
    {
        private string factory;
        private string mDivision;
        private DateTime? CDate1;
        private DateTime? CDate2;
        private string sp1;
        private string sp2;
        private DataTable printData;

        /// <summary>
        /// R11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(this.comboM.Text);
            this.comboM.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if ((!this.dateRangeCDate.Value1.HasValue || !this.dateRangeCDate.Value2.HasValue) &&
                (MyUtility.Check.Empty(this.txtSP1.Text) && MyUtility.Check.Empty(this.txtSP2.Text)))
            {
                MyUtility.Msg.WarningBox("Date, Affected Sp# can't empty!!");
                return false;
            }

            this.CDate1 = this.dateRangeCDate.Value1;
            this.CDate2 = this.dateRangeCDate.Value2;
            this.sp1 = this.txtSP1.Text;
            this.sp2 = this.txtSP2.Text;
            this.factory = this.comboFactory.Text;
            this.mDivision = this.comboM.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            DualResult failResult;

            if (this.CDate1.HasValue)
            {
                sqlWhere.Append(string.Format(" and s.CreateDate >= '{0}'", this.CDate1.Value.ToString("yyyyMMdd")));
            }

            if (this.CDate2.HasValue)
            {
                sqlWhere.Append(string.Format(" and s.CreateDate <= '{0}'", this.CDate2.Value.ToString("yyyyMMdd")));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlWhere.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlWhere.Append(string.Format(" and f.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.sp1) || !MyUtility.Check.Empty(this.sp2))
            {
                if (!MyUtility.Check.Empty(this.sp1) && !MyUtility.Check.Empty(this.sp2))
                {
                    sqlWhere.Append(string.Format(
                        @" and ((sd.FromOrderID = '{0}' or sd.ToOrderID = '{0}')
                                or (sd.FromOrderID = '{1}' or sd.ToOrderID = '{1}'))",
                        this.sp1,
                        this.sp2));
                }
                else if (!MyUtility.Check.Empty(this.sp1))
                {
                    sqlWhere.Append(string.Format(" and (sd.FromOrderID = '{0}' or sd.ToOrderID = '{0}')", this.sp1));
                }
                else if (!MyUtility.Check.Empty(this.sp2))
                {
                    sqlWhere.Append(string.Format(" and (sd.FromOrderID = '{0}' or sd.ToOrderID = '{0}')", this.sp2));
                }
            }

            sqlCmd.Append(string.Format(
                @"
select distinct s.ID
	,s.CreateDate
	,f.MDivisionID
	,s.FactoryID
	,sd.FromOrderID
	,sd.FromComboType
	,sd.Article
	,sd.SizeCode
	,sd.ToOrderID
	,sd.ToComboType
    ,sd.ToArticle
    ,sd.ToSizeCode
	,sd.TransferQty
	,s.Remark
from SewingOutputTransfer s
inner join Factory f on s.FactoryID = f.ID
inner join SewingOutputTransfer_detail sd on s.ID =sd.ID
where 1=1
{0}",
                sqlWhere.ToString()));

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                failResult = new DualResult(false, "Query sewing output data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Sewing_R11";
            string strXltName = Env.Cfg.XltPathDir + "\\" + excelFile + ".xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(strXltName); // 開excelapp
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, excelFile + ".xltx", 1, false, null, excelApp, false, null, false);

            #region Save Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelFile);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();
            return true;
        }
    }
}
