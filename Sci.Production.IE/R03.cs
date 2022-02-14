using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R03
    /// </summary>
    public partial class R03 : Win.Tems.PrintForm
    {
        private string factory;
        private string style;
        private string season;
        private string toolType;
        private string version;
        private DataTable printData;

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboToolType, 2, 1, "0,,1,Attachment,2,Template");
            MyUtility.Tool.SetupCombox(this.comboVersion, 2, 1, "0,,1,Latest Version");
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateInlineDate.HasValue1 && !this.dateInlineDate.HasValue2 && !this.dateSewingDate.HasValue1 && !this.dateSewingDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Inline Date> or <Sewing Date> first!!");
                return false;
            }

            this.factory = this.txtfactory.Text;
            this.style = this.txtstyle.Text;
            this.season = this.txtseason.Text;
            this.toolType = MyUtility.Convert.GetString(this.comboToolType.SelectedValue);
            this.version = MyUtility.Convert.GetString(this.comboVersion.SelectedValue);

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paras = new List<SqlParameter>();

            sqlCmd.Append(
                @"
select l.FactoryID
	, l.StyleID
	, l.ComboType
	, l.SeasonID
	, l.BrandID
	, l.Version
	, ld.No
	, ld.MachineTypeID
	, [Machine Group] = op.MasterPlusGroup
	, [Operation] = IIF(ISNULL(op.DescEN,'') = '', ld.OperationID, op.DescEN)
	, ld.Attachment
	, ld.Template
	, [Latest Version] = lMax.Version
	, [Create By] = l.AddName + '-' + pAdd.Name
	, l.AddDate
	, [Edit By] = l.EditName + '-' + pEdt.Name
	, l.EditDate
from LineMapping l WITH (NOLOCK)
left join Pass1 pAdd WITH (NOLOCK) on l.AddName = pAdd.ID
left join Pass1 pEdt WITH (NOLOCK) on l.EditName = pEdt.ID
inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID = ld.ID
left join Operation op WITH (NOLOCK) on ld.OperationID = op.ID
outer apply (
	select [Version] = max(Version),FactoryID
	from LineMapping l2 WITH (NOLOCK) 
    where l.StyleUkey = l2.StyleUkey and l.FactoryID = l2.FactoryID
    group by FactoryID
)lMax");
            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateSewing = string.Empty;

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateSewing += $@"and s.Inline >= '{this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd")}'";
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateSewing += $@"and s.Inline <= '{this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd")}'";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1) && !MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateSewing += $@"
AND 
( 
	(Cast(s.Inline as Date) >= convert(varchar(10), '{this.dateSewingDate.Value1.Value.ToString("yyyy-MM-dd")}', 120) AND Cast(s.Inline as Date) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy-MM-dd")}' )
    OR
    (Cast(s.Offline as Date) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy-MM-dd")}' AND Cast(s.Offline as Date) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy-MM-dd")}')
)";
                }

                sqlCmd.Append($@"
where EXISTS (
	select 1
	from SewingSchedule s WITH (NOLOCK)
	left join Orders o WITH (NOLOCK) on s.OrderID = o.ID
	where 1=1
    {dateSewing}
    and o.StyleID=l.StyleID and o.SeasonID=l.SeasonID and o.BrandID = l.BrandID and s.FactoryID = l.FactoryID
)" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                paras.Add(new SqlParameter("@FactoryID", this.factory));
                sqlCmd.Append("And l.FactoryID = @FactoryID" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                paras.Add(new SqlParameter("@StyleID", this.style));
                sqlCmd.Append("And l.StyleID = @StyleID" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                paras.Add(new SqlParameter("@SeasonID", this.season));
                sqlCmd.Append("And l.SeasonID = @SeasonID" + Environment.NewLine);
            }

            switch (MyUtility.Convert.GetInt(this.toolType))
            {
                case 1:
                    sqlCmd.Append("And ld.Attachment <> ''" + Environment.NewLine);
                    break;
                case 2:
                    sqlCmd.Append("And ld.Template <> ''" + Environment.NewLine);
                    break;
                default:
                    sqlCmd.Append("And (ld.Attachment <> '' or ld.Template <> '')" + Environment.NewLine);
                    break;
            }

            if (MyUtility.Convert.GetInt(this.version) > 0)
            {
                sqlCmd.Append("And l.Version = lMax.Version and l.FactoryID = lMax.FactoryID" + Environment.NewLine);
            }

            sqlCmd.Append("Order by l.FactoryID, l.StyleID, l.BrandID, l.Version, ld.NO " + Environment.NewLine);

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), paras, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <summary>
        /// OnToExcel 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
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
            string strXltName = Env.Cfg.XltPathDir + "\\IE_R03.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(strXltName); // 預先開啟excel app
            if (objApp == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "IE_R03.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            objApp.Cells.EntireRow.AutoFit();
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
