using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R04
    /// </summary>
    public partial class R04 : Win.Tems.PrintForm
    {

        private string inline1;
        private string inline2;
        private List<string> mcs;
        private List<string> operations;
        private string factory;
        private string brand;
        private List<string> seasons;
        private bool version;
        private DataTable printData;

        /// <summary>
        /// R04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateInlineDate.HasValue1 || !this.dateInlineDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Inline Date> first!!");
                return false;
            }

            if (this.txtmulitMachineType1.Text.Empty())
            {
                MyUtility.Msg.InfoBox("Please input <M/C> first!!");
                return false;
            }

            this.inline1 = this.dateInlineDate.Value1.Value.ToString("yyyyMMdd");
            this.inline2 = this.dateInlineDate.Value2.Value.ToString("yyyyMMdd");
            this.mcs = this.txtmulitMachineType1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.operations = this.txtmulitOperation1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.factory = this.txtfactory1.Text;
            this.brand = this.txtbrand1.Text;
            this.seasons = this.txtmultiSeason1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.version = this.chkLatestVersion.Checked;

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
            paras.Add(new SqlParameter("@inline1", this.inline1));
            paras.Add(new SqlParameter("@inline2", this.inline2));

            sqlCmd.Append(string.Format(
                $@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
	, l.Version
	, l.FactoryID
	, ld.OriNO
	, ld.No
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
	, ld.GSD
	, ld.Cycle
	, [diff] = iif(isnull(ld.Cycle,0) = 0, 0, 1 - (ld.GSD / ld.Cycle))
from LineMapping l WITH (NOLOCK) 
inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID = ld.ID
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
outer apply (
	select [Version] = max(Version)
	from LineMapping l2 WITH (NOLOCK) 
    where l.StyleID = l2.StyleID and l.SeasonID = l2.SeasonID and l.BrandID = l2.BrandID
)lMax
where EXISTS (
	select 1
	from SewingSchedule s WITH (NOLOCK)
	left join Orders o WITH (NOLOCK) on s.OrderID = o.ID
	where s.Inline >= @inline1 
    and s.Inline <= @inline2 
    and o.StyleID=l.StyleID and o.SeasonID=l.SeasonID and o.BrandID = l.BrandID
)
and ld.MachineTypeID in ('{string.Join("','", this.mcs)}')" + Environment.NewLine));

            if (!MyUtility.Check.Empty(this.factory))
            {
                paras.Add(new SqlParameter("@FactoryID", this.factory));
                sqlCmd.Append("And l.FactoryID = @FactoryID" + Environment.NewLine);
            }

            if (this.operations.Count > 0)
            {
                sqlCmd.Append($"And ld.OperationID in ('{string.Join("','", this.operations)}')" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                paras.Add(new SqlParameter("@BrandID", this.brand));
                sqlCmd.Append("And l.BrandID = @BrandID" + Environment.NewLine);
            }

            if (this.seasons.Count > 0)
            {
                sqlCmd.Append($"And l.SeasonID in ('{string.Join("','", this.seasons)}')" + Environment.NewLine);
            }

            if (this.version)
            {
                sqlCmd.Append("And l.Version = lMax.Version" + Environment.NewLine);
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
            string strXltName = Env.Cfg.XltPathDir + "\\IE_R04.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(strXltName); // 預先開啟excel app
            if (objApp == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "IE_R04.xltx", 2, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            objApp.Cells.EntireRow.AutoFit();
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
