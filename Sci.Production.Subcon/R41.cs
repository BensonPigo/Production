using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R41 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string SubProcess, SP, M, CutRef1, CutRef2;
        DateTime?  dateBundle1, dateBundle2;
        public R41(ToolStripMenuItem menuitem)
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

        #region ToExcel3步驟
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBundle.Value1) && MyUtility.Check.Empty(dateBundle.Value2))
            {
                MyUtility.Msg.WarningBox("Bundel CDate can't empty!!");
                return false;
            }
            SubProcess = comboSubProcess.Text;
            SP = textSP.Text;
            M = comboM.Text;
            CutRef1 = textCutRef_Start.Text;
            CutRef2 = textCutRef_End.Text;
            dateBundle1 = dateBundle.Value1;
            dateBundle2 = dateBundle.Value2;
            return base.ValidateInput();
        }
        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region sqlcmd
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"Select distinct
            [Bundleno] = bd.BundleNo,
            [Cut Ref#] = b.CutRef,
            [SP#] = b.Orderid,
            [Master SP#] = b.POID,
            [Factory] = b.MDivisionid,
            [Style] = o.StyleID,
            [Season] = o.SeasonID,
            [Brand] = o.BrandID,
            [Comb] = bd.Patterncode,
            [Article] = b.Article,
            [Color] = b.ColorId,
            [Line] = b.SewinglineId,
            [Cell] = b.SewingCell,
            [Pattern] = bd.PatternCode,
            [PtnDesc] = bd.PatternDesc,
            [Group] = bd.BundleGroup,
            [Size] = bd.SizeCode,
            [Artwork] = stuff(sub.sub,1,1,''),
            [Qty] = bd.Qty,
            [Sub-process] = s.Id,
            [InComing] = bio.InComing,
            [Out (Time)] = bio.OutGoing

            from Bundle b
            inner join Bundle_Detail bd on bd.Id = b.Id
            inner join Bundle_Detail_Art bda on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
            inner join orders o on o.Id = b.OrderId
            inner join SubProcess s on s.Id = bda.SubprocessId 
            left join BundleInOut bio on bio.Bundleno=bd.Bundleno and bio.SubProcessId = bda.SubprocessId
            outer apply(
	             select sub= (
		             Select distinct concat('+', bda.SubprocessId)
		             from Bundle_Detail_Art bda 
		             where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		             for xml path('')
	             )
            ) as sub

            where 1=1
            and (s.IsRFIDDefault = '1' or s.Id = bda.SubprocessId) 
            ");
            #endregion
            #region Append畫面上的條件
            if (!MyUtility.Check.Empty(SubProcess))
            {
                sqlCmd.Append(string.Format(@" and (s.Id = '{0}' or '{0}' = 'ALL') ", SubProcess));
            }
            if (!MyUtility.Check.Empty(CutRef1) && (!MyUtility.Check.Empty(CutRef2)))
            {
                sqlCmd.Append(string.Format(@" and b.CutRef between '{0}' and '{1}'", CutRef1, CutRef2));
            }
            if (!MyUtility.Check.Empty(SP))
            {
                sqlCmd.Append(string.Format(@" and b.Orderid = '{0}'", SP));
            }
            if (!MyUtility.Check.Empty(dateBundle1))
            {
                sqlCmd.Append(string.Format(@" and b.Cdate between '{0}' and '{1}'",
                    Convert.ToDateTime(dateBundle1).ToString("d"), Convert.ToDateTime(dateBundle2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(M))
            {
                sqlCmd.Append(string.Format(@" and b.MDivisionid = '{0}'", M));
            }
            #endregion

            sqlCmd.Append(@"order by [Bundleno],[Cut Ref#],[SP#],[Style],[Season],[Brand],[Article],[Color],[Line],[Cell],[Pattern],[PtnDesc],[Group],[Size]");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);

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
                = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R41_Bundle tracking list (RFID).xltx");
            // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R41_Bundle tracking list (RFID).xltx", 1, true, null, objApp);
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
        #endregion
    }
}
