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

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R22 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private List<SqlParameter> listSqlPara = new List<SqlParameter>();
        private string sqlWhere = string.Empty;

        /// <inheritdoc/>
        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlWhere = string.Empty;
            this.listSqlPara.Clear();

            if (MyUtility.Check.Empty(this.txtseason.Text))
            {
                MyUtility.Msg.WarningBox("<Season> cannot be empty!");
                this.txtseason.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                this.listSqlPara.Add(new SqlParameter("@BrandID", this.txtbrand.Text));
                this.sqlWhere += " and (ss.MasterBrandID = @BrandID or ss.ChildrenBrandID = @BrandID)";
            }

            if (!MyUtility.Check.Empty(this.txtseason.Text))
            {
                this.listSqlPara.Add(new SqlParameter("@Season", this.txtseason.Text));
                this.sqlWhere += " and s.SeasonID = @Season";
            }

            if (!MyUtility.Check.Empty(this.txtstyle.Text))
            {
                this.listSqlPara.Add(new SqlParameter("@StyleID", this.txtstyle.Text));
                this.sqlWhere += " and (ss.MasterStyleID = @StyleID or ss.ChildrenStyleID = @StyleID)";
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
select 
ss.*,
s.SeasonID,
[Createby] = dbo.getTPEPass1_ExtNo(ss.AddName),
[CreateDate] = ss.AddDate,
[Edit by] = dbo.getTPEPass1_ExtNo(ss.EditName),
[Edit Date] = ss.EditDate,
[Rowcnt] = ROW_NUMBER() over(PARTITION BY ss.MasterStyleID,ss.MasterBrandID order by ss.MasterStyleID,ss.MasterBrandID)
into #tmp
from Style_SimilarStyle ss with(nolock)
inner join Style s with(nolock) on s.BrandID = ss.MasterBrandID and s.ID = ss.MasterStyleID
where 1=1
{this.sqlWhere}

select * from (
    --Master
	select [StyleGroup] = MasterStyleID,
	[SimilarStyle] = MasterStyleID,
	[SimilarStyleBrand] = MasterBrandID,
	[Createby] = '',
	[CreateDate] = null,
	[Edit by] = '',
	[Edit Date] = null
	from #tmp where [Rowcnt] =1
union 
	-- Children
	select [StyleGroup] = MasterStyleID,
	[SimilarStyle] = ChildrenStyleID,
	[SimilarStyleBrand] = ChildrenBrandID,
	[Createby],
	[CreateDate],
	[Edit by],
	[Edit Date]
	from #tmp
	where MasterStyleID!=ChildrenStyleID
) a
order by [StyleGroup],a.Createby,[SimilarStyle],[SimilarStyleBrand]

drop table #tmp
");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), listSqlPara, out this.printData);
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
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing...");
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R22.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, null, "PPIC_R22.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R22");
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
