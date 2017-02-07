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
    public partial class R42 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DataTable printData2;

        string SubProcess, SP, Factory,CutRef1, CutRef2;
        DateTime?  dateBundle1, dateBundle2, dateBundleTransDate1, dateBundleTransDate2;
        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboload();
        }
        //string date = "";
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

            DataTable dtfactory;
            if (Result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory", out dtfactory))
            {
                this.comboM.DataSource = dtfactory;
                this.comboM.DisplayMember = "ID";
            }
            else { ShowErr(Result); }
        }

        #region ToExcel3步驟
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBundle.Value1) && MyUtility.Check.Empty(dateBundle.Value2) &&
                MyUtility.Check.Empty(dateBundleTransDate.Value1) && MyUtility.Check.Empty(dateBundleTransDate.Value2))
            {
                MyUtility.Msg.WarningBox("Bundel CDate or Bundle Trans date can't empty!!");
                return false;
            }
            SubProcess = comboSubProcess.Text;
            SP = textSP.Text;
            Factory = comboM.Text;
            CutRef1 = textCutRef_Start.Text;
            CutRef2 = textCutRef_End.Text;
            dateBundle1 = dateBundle.Value1;
            dateBundle2 = dateBundle.Value2;
            dateBundleTransDate1 = dateBundleTransDate.Value1;
            dateBundleTransDate2 = dateBundleTransDate.Value2;
            return base.ValidateInput();
        }
        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region sqlcmd
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"Select distinct
            [Bundle#] = bd.BundleNo,
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
            [RFID Reader] = bt.RFIDReaderId,
            [Sub-process] = bt.SubprocessId,
            [Type] = case when bt.Type = '1' then 'IN'
			              when bt.Type = '2' then 'Out'
			              when bt.Type = '3' then 'In/Out' end,
            [TagId] = bt.TagId,
            [TransferDate] = bt.TransferDate
            --CAST ( bt.TransferDate AS DATE) AS TransferDate

            from Bundle b
            inner join Bundle_Detail bd on bd.Id = b.Id
            inner join orders o on o.Id = b.OrderId
            left join BundleTransfer bt on bt.BundleNo = bd.BundleNo
            outer apply(
	             select sub= (
		             Select distinct concat('+', bda.SubprocessId)
		             from Bundle_Detail_Art bda 
		             where bda.Bundleno = bd.Bundleno
		             for xml path('')
	             )
            ) as sub
            where 1=1
            ");
            #endregion
            #region Append畫面上的條件
            if (!MyUtility.Check.Empty(SubProcess))
            {
                sqlCmd.Append(string.Format(@" and (bt.SubprocessId = '{0}' or '{0}' = 'ALL') ", SubProcess));
            }
            if (!MyUtility.Check.Empty(CutRef1)&&(!MyUtility.Check.Empty(CutRef1))) 
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
            if (!MyUtility.Check.Empty(dateBundleTransDate1))
            {
                sqlCmd.Append(string.Format(@" and b.Cdate between '{0}' and '{1}'",
                    Convert.ToDateTime(dateBundleTransDate1).ToString("d"), Convert.ToDateTime(dateBundleTransDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlCmd.Append(string.Format(@" and b.MDivisionid = '{0}'", Factory));
            }
            #endregion

            sqlCmd.Append(@"order by [Bundle#],[Cut Ref#],[SP#],[Style],[Season],[Brand],[Article],[Color],[Line],[Cell],[Pattern],[PtnDesc],[Group],[Size]");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            
            printData2 = printData.Clone();
            printData2.Columns["TransferDate"].DataType = typeof(string);
            
            foreach (DataRow rd in printData.Rows)
            {
                object[] itemarr = rd.ItemArray;
                itemarr[itemarr.Length - 1] = (rd["TransferDate"].ToString() == "") ? "" : Convert.ToDateTime(rd["TransferDate"]).ToShortDateString();

                printData2.LoadDataRow(itemarr, true);
            }
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
                = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R42_Bundle Transaction detail (RFID).xltx");
            // 將datatable copy to excel
         
            
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            MyUtility.Excel.CopyToXls(printData2, "", "Subcon_R42_Bundle Transaction detail (RFID).xltx", 1, true, null, objApp);

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
        #endregion
    }
}
