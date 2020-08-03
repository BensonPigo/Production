using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R43 : Win.Tems.PrintForm
    {
        DataTable printData;
        string SubProcess;
        string M;
        string Factory;
        DateTime? dateBundleReceive1;
        DateTime? dateBundleReceive2;

        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboload();
            this.comboFactory.SetDataSource();
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
            else
            {
                this.ShowErr(Result);
            }

            DataTable dtM;
            if (Result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory WITH (NOLOCK) ", out dtM))
            {
                this.comboM.DataSource = dtM;
                this.comboM.DisplayMember = "ID";
            }
            else
            {
                this.ShowErr(Result);
            }
        }

        #region ToExcel三步驟

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBundleReceiveDate.Value1) && MyUtility.Check.Empty(this.dateBundleReceiveDate.Value2))
            {
                MyUtility.Msg.WarningBox("Bundel receive date can't empty!!");
                return false;
            }

            this.SubProcess = this.comboSubProcess.Text;
            this.M = this.comboM.Text;
            this.Factory = this.comboFactory.Text;
            this.dateBundleReceive1 = this.dateBundleReceiveDate.Value1;
            this.dateBundleReceive2 = this.dateBundleReceiveDate.Value2;
            return base.ValidateInput();
        }

        // 非同步讀取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 畫面上的條件
            IList<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(new SqlParameter("@SubProcess", this.SubProcess));
            cmds.Add(new SqlParameter("@BundleReceive1", Convert.ToDateTime(this.dateBundleReceive1).ToString("d")));
            cmds.Add(new SqlParameter("@BundleReceive2", Convert.ToDateTime(this.dateBundleReceive2).ToString("d")));
            cmds.Add(new SqlParameter("@M", this.M));
            cmds.Add(new SqlParameter("@Factory", this.Factory));

            Dictionary<string, string> listDicFilte = new Dictionary<string, string>();
            listDicFilte.Add("SubProcess", (!MyUtility.Check.Empty(this.SubProcess)) ? "and (bio.SubprocessId = @SubProcess or @SubProcess = 'ALL')" : string.Empty);
            listDicFilte.Add("BundleReceive1", (!MyUtility.Check.Empty(this.dateBundleReceive1)) ? "and convert(date, bio.InComing) >= @BundleReceive1" : string.Empty);
            listDicFilte.Add("BundleReceive2", (!MyUtility.Check.Empty(this.dateBundleReceive2)) ? "and convert(date, bio.InComing) <= @BundleReceive2 " : string.Empty);
            listDicFilte.Add("M", (!MyUtility.Check.Empty(this.M)) ? "and b.MDivisionid = @M" : string.Empty);
            listDicFilte.Add("Factory", (!MyUtility.Check.Empty(this.Factory)) ? "and o.FtyGroup = @Factory" : string.Empty);
            #endregion
            #region sqlcmd
            string sqlCmd = string.Format(
                @"
select DetailData.M
       , DetailData.Factory
       , DetailData.SPNo
       , DetailData.Style
       , DetailData.Season
       , DetailData.Brand
       , DetailData.[Sub-Process]
       , [Receive Qty] = sum (DetailData.[Receive Qty])
       , [Release Qty] = sum (DetailData.[Release Qty])
       , BCS = Round ((sum (DetailData.[Release Qty]) / sum (DetailData.[Receive Qty]) * 100), 2)
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
                              where DATEDIFF(day,bio.InComing,bio.OutGoing)  <= s.BCSDate)
    from Bundle b WITH (NOLOCK) 
    inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
    inner join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
    inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
    left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno = bd.Bundleno
    left join SubProcess s WITH (NOLOCK) on s.Id = bio.SubprocessId
    where 1=1 and isnull(bio.RFIDProcessLocationID,'') = ''
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
order by [M], [Factory], [SPNo], [Style], [Season], [Brand], [Sub-process], [Receive Qty], [Release Qty], [BCS]", listDicFilte["SubProcess"],
                listDicFilte["BundleReceive1"],
                listDicFilte["BundleReceive2"],
                listDicFilte["M"],
                listDicFilte["Factory"]);
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Application objApp
                = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R43_Sub-process BCS report (RFID).xltx");

            // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R43_Sub-process BCS report (RFID).xltx", 1, true, null, objApp);
            return true;
        }
        #endregion
    }
}
