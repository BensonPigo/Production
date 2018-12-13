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
            DualResult Result;

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
            SubProcess = this.txtsubprocess.Text;
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

            #region Append畫面上的條件
            StringBuilder sqlWhere = new StringBuilder();
            if (!MyUtility.Check.Empty(SubProcess))
            {
                sqlWhere.Append($@" and (s.id in ('{SubProcess.Replace(",", "','")}') or '{SubProcess}'='')");
            }
            if (!MyUtility.Check.Empty(CutRef1))
            {
                sqlWhere.Append(string.Format(@" and b.CutRef >= '{0}' ", CutRef1));
            }
            if (!MyUtility.Check.Empty(CutRef2))
            {
                sqlWhere.Append(string.Format(@" and b.CutRef <= '{0}' ", CutRef2));
            }
            if (!MyUtility.Check.Empty(SP))
            {
                sqlWhere.Append(string.Format(@" and b.Orderid = '{0}'", SP));
            }
            if (!MyUtility.Check.Empty(dateBundle1))
            {
                sqlWhere.Append(string.Format(@" and b.Cdate >= '{0}'", Convert.ToDateTime(dateBundle1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBundle2))
            {
                sqlWhere.Append(string.Format(@" and b.Cdate <= '{0}'", Convert.ToDateTime(dateBundle2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(M))
            {
                sqlWhere.Append(string.Format(@" and b.MDivisionid = '{0}'", M));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlWhere.Append(string.Format(@" and o.FtyGroup = '{0}'", Factory));
            }
            #endregion

            #region sqlcmd
            string sqlCmd = $@"
select  MDivisionID,
        ProcessId, 
	    [InOutType] = iif(sum(cast(type as int)) > 2 ,3,sum(cast(type as int))) 
into #SubProcessInOutType
from (select distinct  MDivisionID,ProcessId,Type from RFIDReader) a group by MDivisionID,ProcessId

Select 
    [Bundleno] = bd.BundleNo,
    [Cut Ref#] = isnull(b.CutRef,''),
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
    bio.LocationID,
    b.Cdate,
    [InComing] = bio.InComing,
    [Out (Time)] = bio.OutGoing,
	AvgTime = case when spio.InOutType = 1 then iif(bio.InComing is null, '', Cast(round(abs(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.InComing,'')))/24.0,2) as varchar))
					when spio.InOutType = 2 then iif(bio.OutGoing is null, '', Cast(round(abs(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.OutGoing,'')))/24.0,2) as varchar))
					when spio.InOutType = 3 then iif(bio.OutGoing is null or bio.InComing is null, '', Cast(round(abs(Datediff(Hour,isnull(bio.InComing,''),isnull(bio.OutGoing,'')))/24.0,2) as varchar))
					else  '--' end
into #result
from Bundle b WITH (NOLOCK) 
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
outer apply(
    select s.ID
    from SubProcess s
        where exists (
                        select 1 from Bundle_Detail_Art bda
                                where   bda.BundleNo = bd.BundleNo    and
                                        bda.ID = b.ID   and
                                        bda.SubProcessID = s.ID
                        ) or s.IsRFIDDefault = 1
) s
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
left join #SubProcessInOutType spio on s.ID = spio.ProcessId and b.MDivisionID = spio.MDivisionID
outer apply(
	    select sub= stuff((
		    Select distinct concat('+', bda.SubprocessId)
		    from Bundle_Detail_Art bda WITH (NOLOCK) 
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('')
	    ),1,1,'')
) as sub
where 1=1 {sqlWhere}";

            string sqlResult = $@"
{sqlCmd}

;with GetCutDateTmp as
(
	select	r.[Cut Ref#],
			r.M,
			[EstCutDate] = MAX(w.EstCutDate),
			[CuttingOutputDate] = MAX(co.cDate)
	from #result r
	inner join WorkOrder w with (nolock) on w.CutRef = r.[Cut Ref#] and w.MDivisionId = r.M
	left join CuttingOutput_Detail cod with (nolock) on cod.WorkOrderUkey = w.Ukey
	left join CuttingOutput co  with (nolock) on co.ID = cod.ID
    where r.[Cut Ref#] <> ''
	group by r.[Cut Ref#],r.M
)
select
    r.[Bundleno] ,
    r.[Cut Ref#] ,
    r.[SP#],
    r.[Master SP#],
    r.[M],
    r.[Factory],
    r.[Style],
    r.[Season],
    r.[Brand],
    r.[Comb],
    r.Cutno,
	r.[Fab_Panel Code],
    r.[Article],
    r.[Color],
    r.[Line],
    r.[Cell],
    r.[Pattern],
    r.[PtnDesc],
    r.[Group],
    r.[Size],
    r.[Artwork],
    r.[Qty],
    r.[Sub-process],
    r.LocationID,
    r.Cdate,
    r.[InComing],
    r.[Out (Time)],
	r.AvgTime,
    gcd.EstCutDate,
    gcd.CuttingOutputDate
from #result r
left join GetCutDateTmp gcd on r.[Cut Ref#] = gcd.[Cut Ref#] and r.M = gcd.M 
order by [Bundleno],[Cut Ref#],[SP#],[Style],[Season],[Brand],[Article],[Color],[Line],[Cell],[Pattern],[PtnDesc],[Group],[Size],[Out (Time)] desc,[InComing] desc

drop table #SubProcessInOutType,#result
";

            #endregion

            string cmdct = $@"
{sqlCmd}

select ct = count(1)
from #result

drop table #SubProcessInOutType,#result
";
            DataTable groupByDt;
            DualResult result = DBProxy.Current.Select(null, cmdct, out groupByDt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }
            var groupByLinq = groupByDt.AsEnumerable();
            int ct = groupByLinq.Sum(s => (int)s["ct"]);
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
                cm.CommandText = sqlResult;
                cm.CommandTimeout = 900;
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
                        
            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R41_Bundle tracking list (RFID)");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
        #endregion
    }
}
