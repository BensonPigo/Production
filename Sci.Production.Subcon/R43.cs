using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R43 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string SubProcess, M;
        DateTime? dateBundleReceive1, dateBundleReceive2;

        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboload();
        }

        private void comboload()
        {
            DataTable dtSubprocessID;
            DualResult Result;
            if (Result = DBProxy.Current.Select(null, "select 'ALL' as id,1 union select id,2 from Subprocess where Junk = 0 ",
                out dtSubprocessID))
            {
                this.comboSubProcess.DataSource = dtSubprocessID;
                this.comboSubProcess.DisplayMember = "ID";
            }
            else { ShowErr(Result); }

            DataTable dtM;
            if (Result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory", out dtM))
            {
                this.comboM.DataSource = dtM;
                this.comboM.DisplayMember = "ID";
            }
            else { ShowErr(Result); }
        }

        #region ToExcel三步驟
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBundleReceive.Value1) && MyUtility.Check.Empty(dateBundleReceive.Value2))
            {
                MyUtility.Msg.WarningBox("Bundel receive date can't empty!!");
                return false;
            }

            SubProcess = comboSubProcess.Text;
            M = comboM.Text;
            dateBundleReceive1 = dateBundleReceive.Value1;
            dateBundleReceive2 = dateBundleReceive.Value2;
            return base.ValidateInput();
        }
        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region sqlcmd
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select
            [Factory] = b.MDivisionid,
            [SPNo] = b.Orderid,
            [Style] = o.StyleID,
            [Season] = o.SeasonID,
            [Brand] = o.BrandID,
            [Sub-process] = bio.SubProcessId,
            [Receive Qty] =sum(bd.qty),
            [Release Qty] = (select sum(bd.qty)  where (bio.InComing-bio.OutGoing) <= s.BCSDate),
            [BCS] = sum(bd.qty) /(select sum(bd.qty)  where (bio.InComing-bio.OutGoing) <= s.BCSDate)

            from Bundle b
            inner join Bundle_Detail bd on bd.Id = b.Id
            inner join Bundle_Detail_Art bda on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
            inner join orders o on o.Id = b.OrderId
            left join BundleInOut bio on bio.Bundleno = bd.Bundleno
            left join SubProcess s on s.Id = bio.SubprocessId

            where 1=1
            ");
            #endregion
            #region Append畫面上的條件
            IList<SqlParameter> cmds = new List<SqlParameter>();
            if (!MyUtility.Check.Empty(SubProcess))
            {
                sqlCmd.Append(@" and (bio.SubprocessId = @SubProcess or @SubProcess = 'ALL')");
                cmds.Add(new SqlParameter("@SubProcess", SubProcess));
            }
            if (!MyUtility.Check.Empty(dateBundleReceive1))
            {
                sqlCmd.Append(@" and bio.InComing between @BundleReceive1 and @BundleReceive2 ");
                cmds.Add(new SqlParameter("@BundleReceive1", Convert.ToDateTime(dateBundleReceive1).ToString("d")));
                cmds.Add(new SqlParameter("@BundleReceive2", Convert.ToDateTime(dateBundleReceive2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(M))
            {
                sqlCmd.Append(string.Format(@" and b.MDivisionid = @Factory"));
                cmds.Add(new SqlParameter("@Factory", M));
            }
            #endregion
            #region group order
            sqlCmd.Append(@"
            group by 
            b.MDivisionid,b.Orderid,o.StyleID,o.SeasonID,o.BrandID,bio.SubProcessId,bio.InComing,bio.OutGoing,s.BCSDate
            order by 
            [Factory],[SPNo],[Style],[Season],[Brand],[Sub-process],[Receive Qty],[Release Qty],[BCS]");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
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
            SetCount(printData.Rows.Count);
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            //預先開啟excel app
            Microsoft.Office.Interop.Excel.Application objApp
                = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R43_Sub-process BCS report (RFID).xltx");
            // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R43_Sub-process BCS report (RFID).xltx", 1, true, null, objApp);
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
        #endregion
    }
}
