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
        string SubProcess, M, Factory;
        DateTime? dateBundleReceive1, dateBundleReceive2;

        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboload();
            this.comboFactory.setDataSource();
        }

        private void comboload()
        {
            DataTable dtSubprocessID;
            DualResult Result;
            if (Result = DBProxy.Current.Select(null, "select 'ALL' as id,1 union select id,2 from Subprocess WITH (NOLOCK) where Junk = 0 ",
                out dtSubprocessID))
            {
                this.comboSubProcess.DataSource = dtSubprocessID;
                this.comboSubProcess.DisplayMember = "ID";
            }
            else { ShowErr(Result); }

            DataTable dtM;
            if (Result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory WITH (NOLOCK) ", out dtM))
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
            if (MyUtility.Check.Empty(dateBundleReceiveDate.Value1) && MyUtility.Check.Empty(dateBundleReceiveDate.Value2))
            {
                MyUtility.Msg.WarningBox("Bundel receive date can't empty!!");
                return false;
            }

            SubProcess = comboSubProcess.Text;
            M = comboM.Text;
            Factory = comboFactory.Text;
            dateBundleReceive1 = dateBundleReceiveDate.Value1;
            dateBundleReceive2 = dateBundleReceiveDate.Value2;
            return base.ValidateInput();
        }

        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 畫面上的條件
            IList<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(new SqlParameter("@SubProcess", SubProcess));
            cmds.Add(new SqlParameter("@BundleReceive1", Convert.ToDateTime(dateBundleReceive1).ToString("d")));
            cmds.Add(new SqlParameter("@BundleReceive2", Convert.ToDateTime(dateBundleReceive2).ToString("d") + " 23:59:59"));
            cmds.Add(new SqlParameter("@M", M));
            cmds.Add(new SqlParameter("@Factory", Factory));

            Dictionary<string, string> listDicFilte = new Dictionary<string, string>();
            listDicFilte.Add("SubProcess", (!MyUtility.Check.Empty(SubProcess)) ? "and (bio.SubprocessId = @SubProcess or @SubProcess = 'ALL')" : "");
            listDicFilte.Add("BundleReceive1", (!MyUtility.Check.Empty(dateBundleReceive1)) ? "and bio.InComing >= @BundleReceive1" : "");
            listDicFilte.Add("BundleReceive2", (!MyUtility.Check.Empty(dateBundleReceive2)) ? "and bio.InComing <= @BundleReceive2 " : "");
            listDicFilte.Add("M", (!MyUtility.Check.Empty(M)) ? "and b.MDivisionid = @M" : "");
            listDicFilte.Add("Factory", (!MyUtility.Check.Empty(Factory)) ? "and o.FtyGroup = @Factory" : "");            
            #endregion
            #region sqlcmd
            string sqlCmd = String.Format(@"
select DetailData.M
       , DetailData.Factory
       , DetailData.SPNo
       , DetailData.Style
       , DetailData.Season
       , DetailData.Brand
       , DetailData.[Sub-Process]
       , [Receive Qty] = sum (DetailData.[Receive Qty])
       , [Release Qty] = sum (DetailData.[Release Qty])
       , BCS = Round ((sum (DetailData.[Receive Qty]) / sum (DetailData.[Release Qty]) * 100), 2)
from (
    select [M] = b.MDivisionid
           , [Factory] = o.FtyGroup
           , [SPNo] = b.Orderid
           , [Style] = o.StyleID
           , [Season] = o.SeasonID
           , [Brand] = o.BrandID
           , [Sub-process] = bio.SubProcessId
           , [Receive Qty] = sum(bd.qty)
           , [Release Qty] = (select sum(bd.qty)  
                              where (bio.InComing-bio.OutGoing) <= s.BCSDate)
    from Bundle b WITH (NOLOCK) 
    inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
    inner join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id 
                                                      and bda.Bundleno = bd.Bundleno
    inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
    left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno = bd.Bundleno
    left join SubProcess s WITH (NOLOCK) on s.Id = bio.SubprocessId
    where 1=1
          -- SubProcess --
          {0}  
          -- BundleReceive1 --
          {1}  
          -- BundleReceive2 --
          {2}  
          -- M --
          {3}  
          -- Factory --
          {4}  
    group by b.MDivisionid, o.FtyGroup, b.Orderid, o.StyleID, o.SeasonID, o.BrandID, bio.SubProcessId, bio.InComing, bio.OutGoing, s.BCSDate    
) DetailData
group by [M], [Factory], [SPNo], [Style], [Season], [Brand], [Sub-process]
order by [M], [Factory], [SPNo], [Style], [Season], [Brand], [Sub-process], [Receive Qty], [Release Qty], [BCS]", listDicFilte["SubProcess"]
                                                                                                                , listDicFilte["BundleReceive1"]
                                                                                                                , listDicFilte["BundleReceive2"]
                                                                                                                , listDicFilte["M"]
                                                                                                                , listDicFilte["Factory"]);
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
            return true;
        }
        #endregion
    }
}
