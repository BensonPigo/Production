using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P22_Print : Win.Tems.PrintForm
    {
        private DateTime? DateCreate1;
        private DateTime? DateCreate2;
        private DateTime? DateApv1;
        private DateTime? DateApv2;
        private string mDivisionID;
        private string facttoryID;
        private string department;
        private DataTable PrintData;

        /// <inheritdoc/>
        public P22_Print()
        {
            this.InitializeComponent();
            this.comboMDivision.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(Sci.Env.User.Keyword);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateCreate.HasValue1 && !this.dateCreate.HasValue2 && !this.dateApv.HasValue1 && !this.dateApv.HasValue2)
            {
                MyUtility.Msg.WarningBox(" Create Dat and Apv. Date can't all empty!!");
                return false;
            }

            this.DateCreate1 = this.dateCreate.Value1;
            this.DateCreate2 = this.dateCreate.Value2;
            this.DateApv1 = this.dateApv.Value1;
            this.DateApv2 = this.dateApv.Value2;
            this.mDivisionID = this.comboMDivision.Text;
            this.facttoryID = this.comboFactory.Text;
            this.department = this.txtDepartment.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;
            List<SqlParameter> para = new List<SqlParameter>();

            sqlCmd = $@"
SELECT a.SewingLineID
    ,a.ID
    ,o.StyleID
    ,a.OrderID
    ,b.Refno
    ,l.Description
    ,a.ApvDate
    ,b.RequestQty
    ,[Type] = Iif(a.Type='L', 'Lacking', 'Replacement')
    ,[Reason] = b.ReplacementLocalItemReasonID +'-'+ reason.Description
    ,b.Remark
FROM ReplacementLocalItem a WITH(NOLOCK) 
INNER JOIN Orders o WITH(NOLOCK) ON a.OrderID = o.ID
INNER JOIN ReplacementLocalItem_Detail b WITH(NOLOCK) ON a.ID = b.ID
INNER JOIN LocalItem l WITH(NOLOCK) ON l.Refno = b.Refno
INNER JOIN ReplacementLocalItemReason reason WITH(NOLOCK) ON reason.ID = b.ReplacementLocalItemReasonID
WHERE 1=1
";
            if (this.DateCreate1.HasValue)
            {
                sqlCmd += $@"AND a.AddDate >= '{this.DateCreate1.Value.ToShortDateString()}'" + Environment.NewLine;
            }

            if (this.DateCreate2.HasValue)
            {
                sqlCmd += $@"AND a.AddDate <= '{this.DateCreate2.Value.AddDays(1).AddSeconds(-1).ToShortDateString()}'" + Environment.NewLine;
            }

            if (this.DateApv1.HasValue)
            {
                sqlCmd += $@"AND a.ApvDate >= '{this.DateApv1.Value.ToShortDateString()}'" + Environment.NewLine;
            }

            if (this.DateApv2.HasValue)
            {
                sqlCmd += $@"AND a.ApvDate <= '{this.DateApv2.Value.AddDays(1).AddSeconds(-1).ToShortDateString()}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.mDivisionID))
            {
                sqlCmd += $@"AND a.MDivisionID = '{this.mDivisionID}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.facttoryID))
            {
                sqlCmd += $@"AND a.FactoryID = '{this.facttoryID}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.department))
            {
                sqlCmd += $@"AND a.Dept = @Department " + Environment.NewLine;
                para.Add(new SqlParameter("@Department", this.department));
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd, para, out this.PrintData);
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
            this.SetCount(this.PrintData.Rows.Count);

            if (this.PrintData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application excelAPP = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_P22_Print.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintData, string.Empty, "PPIC_P22_Print.xltx", 1, false, null, excelAPP); // 將datatable copy to excel

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_P22_Print");
            excelAPP.ActiveWorkbook.SaveAs(strExcelName);
            excelAPP.Visible = true;

            Marshal.ReleaseComObject(excelAPP);
            #endregion

            return true;
        }
    }
}
