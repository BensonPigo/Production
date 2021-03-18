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
    /// IE_R07
    /// </summary>
    public partial class R07 : Win.Tems.PrintForm
    {

        private string date1;
        private string date2;
        private List<string> mcs;
        private List<string> operations;
        private bool version;
        private bool bolFtyGSD;
        private DataTable printData;

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R07(ToolStripMenuItem menuitem)
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
                MyUtility.Msg.InfoBox("Please input <Add/ Edit Date> first!!");
                return false;
            }

            if (this.txtmulitOperation1.Text.Empty())
            {
                MyUtility.Msg.InfoBox("Please input <Code Type> first!!");
                return false;
            }

            this.date1 = this.dateInlineDate.Value1.Value.ToString("yyyyMMdd");
            this.date2 = this.dateInlineDate.Value2.Value.ToString("yyyyMMdd");
            this.mcs = this.txtmulitMachineType1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.operations = this.txtmulitOperation1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.version = this.chkLatestVersion.Checked;
            this.bolFtyGSD = this.radioFtyGSD.Checked;

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
            paras.Add(new SqlParameter("@date1", this.date1));
            paras.Add(new SqlParameter("@date2", this.date2));

            if (this.bolFtyGSD)
            {
                sqlCmd = this.FtyGSDCmd();
            }
            else
            {
                sqlCmd = this.LineMappingCmd();
            }

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
            string strXltName = this.bolFtyGSD ? "IE_R07_FtyGSD.xltx" : "IE_R07_LineMapping.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + @"\" + strXltName); // 預先開啟excel app
            if (objApp == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, strXltName, 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            objApp.Cells.EntireRow.AutoFit();
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }

        private StringBuilder FtyGSDCmd()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(
                 $@"
select t.*
into #tmp_TimeStudy
from TimeStudy t WITH (NOLOCK)
inner join TimeStudy_Detail td WITH (NOLOCK) on t.ID = td.ID
outer apply (
	select [Version] = max(Version)
	from TimeStudy t2 WITH (NOLOCK) 
    where t.StyleID = t2.StyleID and t.SeasonID = t2.SeasonID and t.BrandID = t2.BrandID
)tMax
where (t.AddDate between @date1 and @date2
	or t.EditDate between @date1 and @date2)
And td.OperationID in ('{string.Join("', '", this.operations)}')
");

            if (this.mcs.Count > 0)
            {
                sqlCmd.Append($"And td.MachineTypeID in ('{string.Join("','", this.mcs)}')" + Environment.NewLine);
            }

            if (this.version)
            {
                sqlCmd.Append("And t.Version = tMax.Version" + Environment.NewLine);
            }

            sqlCmd.Append(
                $@"
select distinct m.StyleID, m.SeasonID, m.BrandID, m.ComboType, m.TimeStudyVersion, m.FactoryID 
into #tmp_LineMapping
from #tmp_TimeStudy t
left join LineMapping m on m.StyleID = t.StyleID 
	  and m.SeasonID = t.SeasonID 
	  and m.BrandID = t.BrandID 
	  and m.ComboType = t.ComboType
	  and m.TimeStudyVersion  = t.Version 
where m.FactoryID  is not null

select distinct td.OperationID
	, t.BrandID
	, t.StyleID
	, t.ComboType
	, t.SeasonID
	, t.Phase
	, t.Version
	, [Factory] = Stuff((select distinct concat('/',m.FactoryID)
				 		from #tmp_LineMapping m 
				 		where m.StyleID = t.StyleID 
				 		and m.SeasonID = t.SeasonID 
				 		and m.BrandID = t.BrandID 
				 		and m.ComboType = t.ComboType 
				 		and m.TimeStudyVersion  = t.Version 
				  FOR XML PATH('')) ,1,1,'') 
	, t.Status
	, t.AddName
	, t.AddDate
	, t.EditName
	, t.EditDate	
from #tmp_TimeStudy t
inner join TimeStudy_Detail td WITH (NOLOCK) on t.ID = td.ID 
                                           and td.OperationID in ('{string.Join("', '", this.operations)}')
");

            return sqlCmd;
        }

        private StringBuilder LineMappingCmd()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(
                 $@"
select distinct ld.OperationID
	, l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
	, l.Version
	, l.FactoryID
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
from LineMapping l WITH (NOLOCK)
inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID = ld.ID
outer apply (
	select [Version] = max(Version)
	from LineMapping l2 WITH (NOLOCK)
    where l.StyleID = l2.StyleID and l.SeasonID = l2.SeasonID and l.BrandID = l2.BrandID
)lMax
where (l.AddDate between @date1 and @date2
	or l.EditDate between @date1 and @date2)
And ld.OperationID in ('{string.Join("', '", this.operations)}')
");

            if (this.mcs.Count > 0)
            {
                sqlCmd.Append($"And ld.MachineTypeID in ('{string.Join("','", this.mcs)}')" + Environment.NewLine);
            }

            if (this.version)
            {
                sqlCmd.Append("And l.Version = lMax.Version" + Environment.NewLine);
            }

            return sqlCmd;
        }
    }
}
