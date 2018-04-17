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
    public partial class R41 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string SubProcess, SP, M, Factory, CutRef1, CutRef2;
        DateTime? dateBundle1, dateBundle2;
        public R41(ToolStripMenuItem menuitem)
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

        #region ToExcel3步驟
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            SubProcess = this.comboSubProcess.Text;
            SP = this.txtSPNo.Text;
            M = this.comboM.Text;
            Factory = this.comboFactory.Text;
            CutRef1 = this.txtCutRefStart.Text;
            CutRef2 = this.txtCutRefEnd.Text;
            dateBundle1 = this.dateBundleCDate.Value1;
            dateBundle2 = this.dateBundleCDate.Value2;
            if (MyUtility.Check.Empty(CutRef1) && MyUtility.Check.Empty(CutRef2) &&
                MyUtility.Check.Empty(SP) &&
                MyUtility.Check.Empty(dateBundleCDate.Value1) && MyUtility.Check.Empty(dateBundleCDate.Value2))
            {
                MyUtility.Msg.WarningBox("[Cut Ref#][SP#][Bundle CDate] can not all empty !!");
                return false;
            }
            return base.ValidateInput();
        }

        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {

            #region sqlcmd
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
Select DISTINCT
    [Bundleno] = bd.BundleNo,
    [Cut Ref#] = b.CutRef,
    [SP#] = b.Orderid,
    [Master SP#] = b.POID,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Comb] = b.PatternPanel,
    b.Cutno,
	[Fab_Panel Code] = b.FabricPanelCode,
    [Article] = b.Article,
    [Color] = b.ColorId,
    [Line] = b.SewinglineId,
    [Cell] = b.SewingCell,
    [Pattern] = bd.PatternCode,
    [PtnDesc] = bd.PatternDesc,
    [Group] = bd.BundleGroup,
    [Size] = bd.SizeCode,
    [Artwork] = sub.sub,
    [Qty] = bd.Qty,
    [Sub-process] = s.Id,
    [InComing] = bio.InComing,
    [Out (Time)] = bio.OutGoing

from Bundle b WITH (NOLOCK) 
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
outer apply(
	    select sub= stuff((
		    Select distinct concat('+', bda.SubprocessId)
		    from Bundle_Detail_Art bda WITH (NOLOCK) 
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('')
	    ),1,1,'')
) as sub

where 1=1
            ");
            #endregion
            #region Append畫面上的條件
            if (!MyUtility.Check.Empty(SubProcess))
            {
                sqlCmd.Append(string.Format(@" and (s.Id = '{0}' or '{0}' = 'ALL') ", SubProcess));
            }
            if (!MyUtility.Check.Empty(CutRef1))
            {
                sqlCmd.Append(string.Format(@" and b.CutRef >= '{0}' ", CutRef1));
            }
            if (!MyUtility.Check.Empty(CutRef2))
            {
                sqlCmd.Append(string.Format(@" and b.CutRef <= '{0}' ", CutRef2));
            }
            if (!MyUtility.Check.Empty(SP))
            {
                sqlCmd.Append(string.Format(@" and b.Orderid = '{0}'", SP));
            }
            if (!MyUtility.Check.Empty(dateBundle1))
            {
                sqlCmd.Append(string.Format(@" and b.Cdate >= '{0}'", Convert.ToDateTime(dateBundle1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBundle2))
            {
                sqlCmd.Append(string.Format(@" and b.Cdate <= '{0}'", Convert.ToDateTime(dateBundle2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(M))
            {
                sqlCmd.Append(string.Format(@" and b.MDivisionid = '{0}'", M));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlCmd.Append(string.Format(@" and o.FtyGroup = '{0}'", Factory));
            }
            #endregion

            
            string cmdct = string.Format("select count(*) ct from ({0})aaa", sqlCmd.ToString());
            int ct = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(cmdct));
            sqlCmd.Append(@"order by [Bundleno],[Cut Ref#],[SP#],[Style],[Season],[Brand],[Article],[Color],[Line],[Cell],[Pattern],[PtnDesc],[Group],[Size],[Out (Time)] desc,[InComing] desc");
            string cmd1 = sqlCmd.ToString();
            SetCount(ct);
            if (ct <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            if (ct > 1000000)
            {
                MyUtility.Msg.WarningBox("The number of data more than one million, please use more condition !!");
                return false;
            }
            //預先開啟excel app
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R41_Bundle tracking list (RFID).xltx");

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            int num = 200000;

            using (var cn = new SqlConnection(Env.Cfg.GetConnection("", DBProxy.Current.DefaultModuleName).ConnectionString))
            using (var cm = cn.CreateCommand())
            {
                cm.CommandText = cmd1;
                var adp = new System.Data.SqlClient.SqlDataAdapter(cm);
                var cnt = 0;
                var start = 0;
                using (var ds = new DataSet())
                {
                    while ((cnt = adp.Fill(ds, start, num, "Bundle_Detail")) > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("load {0} records", cnt);

                        //do some jobs                       
                        MyUtility.Excel.CopyToXls(ds.Tables[0], "", "Subcon_R41_Bundle tracking list (RFID).xltx", 1+ start, false, null, objApp, wSheet: objSheets);
                        
                        start += num;

                        //if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                        ds.Tables[0].Dispose();
                        ds.Tables.Clear();
                    }
                }
            }
            //if (Cpage > 0)
            //{
            //    objApp.ActiveWorkbook.Worksheets[Cpage].Columns.AutoFit();//這頁需要重新調整欄寬                
            //}

            Marshal.ReleaseComObject(objSheets);
            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R41_Bundle tracking list (RFID)");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
        #endregion
    }
}
