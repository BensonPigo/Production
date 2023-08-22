using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class R09 : Win.Tems.PrintForm
    {
        private string date1;
        private string date2;
        private string strOperator;
        private string strSM;
        private string strStyle;
        private DataTable printData;

        /// <inheritdoc/>
        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dtAddEdit.HasValue1 && !this.dtAddEdit.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Buyer Delivery>.");
                return false;
            }

            this.date1 = this.dtAddEdit.Value1.Value.ToString("yyyyMMdd");
            this.date2 = this.dtAddEdit.Value2.Value.ToString("yyyyMMdd");
            this.strOperator = this.txtOperatorID.Text;
            this.strSM = this.txtST_MC_Type.Text;
            this.strStyle = this.txtStyle.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;
            string sqlWhere = string.Empty;
            List<SqlParameter> listParameter = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.date1))
            {
                listParameter.Add(new SqlParameter("@date1", this.date1));
                listParameter.Add(new SqlParameter("@date2", this.date2));
                sqlWhere += $@" and ((lm.AddDate between @date1 and @date2) or(lm.EditDate between @date1 and @date2))";
            }

            if (!MyUtility.Check.Empty(this.strOperator))
            {
                listParameter.Add(new SqlParameter("@OperatorID", this.strOperator));
                sqlWhere += $@" and e.ID = @OperatorID";
            }

            if (!MyUtility.Check.Empty(this.strSM))
            {
                listParameter.Add(new SqlParameter("@STMC", this.strSM));
                sqlWhere += $@" and lmd.MachineTypeID = @STMC";
            }

            if (!MyUtility.Check.Empty(this.strStyle))
            {
                listParameter.Add(new SqlParameter("@Style", this.strStyle));
                sqlWhere += $@" and lm.StyleID = @Style";
            }

            if (this.chkVersion.Checked)
            {
                sqlWhere += $@"
                 and lm.Version = (select MAX(l.Version)
	                               from LineMapping l
	                               where l.StyleUKey = lm.StyleUKey
	                               and l.FactoryID = lm.FactoryID
	                               and l.Phase = lm.Phase
	                               and l.SewingLineID = lm.SewingLineID
	                               group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID)";
            }

            sqlCmd = $@"
            select
            [Factory] = lm.FactoryID
            , [OperatorID] = e.ID
            , [OperatorName] = iif(e.Junk = 1 , e.[Name], iif(e.LastName + ',' + e.FirstName <> ',',e.LastName + ',' + e.FirstName,''))
            , [Style] = lm.StyleID
            , [Season] = lm.SeasonID
            , [Brand] = lm.BrandID
            , [ComboType] = lm.ComboType
            , [Version] = lm.[Version]
            , [Phase] = lm.Phase
            , [Line] = lm.SewingLineID
            , [Team] = lm.Team
            , [NO] = lmd.[No]
            , [OperationCode] = lmd.OperationID
            , [ST/CM Type] = lmd.MachineTypeID
            , [MachineGroup] = lmd.MasterPlusGroup
            , [Operation] = o.DescEN
            , [Motion] = Motion.val
            , [Shape] = Shape.val
            , [Attachment] = lmd.Attachment
            , [Tmplate] = lmd.Template
            , [GSDTime] = lmd.GSD
            , [Cycle Time] = lmd.Cycle
            from LineMapping_Detail lmd
            LEFT JOIN LineMapping lm WITH(NOLOCK) on lm.ID = lmd.ID
            LEFT JOIN Employee e WITH(NOLOCK) on lmd.EmployeeID = e.ID
            LEFT JOIN Operation o WITH(NOLOCK) on o.ID = lmd.OperationID
            OUTER APPLY
            (
	            select val = stuff((select distinct concat(',',Name)
                from OperationRef a
                inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
                where a.CodeType = '00007' and a.id = o.id for xml path('') ),1,1,'')
            )Motion
            OUTER APPLY
            (
	            select val = stuff((select distinct concat(',',Name)
                from OperationRef a
                inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
                where a.CodeType = '00008' and a.id = o.id for xml path('') ),1,1,'')
            )Shape
            Where 1=1 and (e.ID is not null or e.id <> '')
            and e.junk = 0
            {sqlWhere}
            ORDER by OperationCode,Style,Season,Brand,Version";

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), listParameter, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            this.ShowWaitMessage("Excel Processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\IE_R09.xltx"); // 預先開啟excel app

            MyUtility.Excel.CopyToXls(this.printData, null, "IE_R09.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_R09");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
