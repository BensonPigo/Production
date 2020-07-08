using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R06
    /// </summary>
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? date1;
        private DateTime? date2;
        private string m;
        private string factory;
        private string brand;
        private string sp_from;
        private string sp_to;

        /// <summary>
        /// R06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.dateOutput.Value1;
            this.date2 = this.dateOutput.Value2;
            this.m = this.txtMdivision1.Text;
            this.factory = this.txtfactory.Text;
            this.brand = this.txtbrand.Text;
            this.sp_from = this.txtsp_from.Text;
            this.sp_to = this.txtsp_to.Text;

            if (MyUtility.Check.Empty(this.date1) && MyUtility.Check.Empty(this.date2) && MyUtility.Check.Empty(this.sp_from) && MyUtility.Check.Empty(this.sp_to))
            {
                MyUtility.Msg.WarningBox("OutputDate and Order can't be empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select s.OutputDate,s.SewingLineID as SewingLine,sdd.OrderId,sdd.ComboType,sdd.Article,sdd.SizeCode as Size,sum(sdd.QAQty) as 'QA Output' 
from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sdd.SewingOutput_DetailUKey=sd.UKey 
inner join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID 
where 1=1
                "));

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.m))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.m));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.sp_from))
            {
                sqlCmd.Append(string.Format(" and sdd.OrderId >= '{0}'", this.sp_from));
            }

            if (!MyUtility.Check.Empty(this.sp_to))
            {
                sqlCmd.Append(string.Format(" and sdd.OrderId <= '{0}'", this.sp_to));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and (select brandid from orders WITH (NOLOCK) where id = sdd.orderid) = '{0}'", this.brand));
            }

            sqlCmd.Append(string.Format(@"
group by s.OutputDate,s.SewingLineID,sdd.OrderId,sdd.ComboType,sdd.Article,sdd.SizeCode 
order by s.OutputDate,s.SewingLineID,sdd.OrderId,sdd.ComboType,sdd.Article,sdd.SizeCode 
                "));

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                return new DualResult(false, result.ToString());
            }

            return Result.True;
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
            string excelFile = "Sewing_R06.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
