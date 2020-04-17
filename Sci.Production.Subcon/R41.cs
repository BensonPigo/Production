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
        string processLocation;
        DateTime? dateBundle1, dateBundle2, dateBundleScanDate1, dateBundleScanDate2, dateEstCutDate1, dateEstCutDate2, dateBDelivery1, dateBDelivery2, dateSewInLine1, dateSewInLine2;
        public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboload();
            this.comboFactory.setDataSource();
            this.comboRFIDProcessLocation.setDataSource();
            this.comboRFIDProcessLocation.SelectedIndex = 0;
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
            dateBundleScanDate1 = this.dateBundleScanDate.Value1;
            dateBundleScanDate2 = this.dateBundleScanDate.Value2;
            dateEstCutDate1 = this.dateEstCutDate.Value1;
            dateEstCutDate2 = this.dateEstCutDate.Value2;
            dateBDelivery1 = this.dateBDelivery.Value1;
            dateBDelivery2 = this.dateBDelivery.Value2;
            dateSewInLine1 = this.dateSewInLine.Value1;
            dateSewInLine2 = this.dateSewInLine.Value2;
            this.processLocation = this.comboRFIDProcessLocation.Text;
            if (MyUtility.Check.Empty(CutRef1) && MyUtility.Check.Empty(CutRef2) &&
                MyUtility.Check.Empty(SP) &&
                MyUtility.Check.Empty(dateEstCutDate.Value1) && MyUtility.Check.Empty(dateEstCutDate.Value2) &&
                MyUtility.Check.Empty(dateBundleCDate.Value1) && MyUtility.Check.Empty(dateBundleCDate.Value2) &&
                MyUtility.Check.Empty(dateBundleScanDate.Value1) && MyUtility.Check.Empty(dateBundleScanDate.Value2) &&
                MyUtility.Check.Empty(dateSewInLine.Value1) && MyUtility.Check.Empty(dateSewInLine.Value2)
                )
            {
                MyUtility.Msg.WarningBox("[Cut Ref#][SP#][Est. Cutting Date][Bundle CDate][Bundle Scan Date],[Sewing Inline] cannot all empty !!");
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
            StringBuilder sqlWhereWorkOrder = new StringBuilder();
            if (!MyUtility.Check.Empty(SubProcess))
            {
                sqlWhere.Append($@" and s.id in ('{SubProcess.Replace(",", "','")}') ");
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
            if (!MyUtility.Check.Empty(dateBundleScanDate1) && !MyUtility.Check.Empty(dateBundleScanDate2))
            {
                sqlWhere.Append(string.Format(@" and ((convert (date, bio.InComing) >= '{0}' and convert (date, bio.InComing) <= '{1}' ) or (convert (date, bio.OutGoing) >= '{0}' and convert (date, bio.OutGoing) <= '{1}'))", Convert.ToDateTime(dateBundleScanDate1).ToString("d"), Convert.ToDateTime(dateBundleScanDate2).ToString("d")));
            }
            else
            {
                if (!MyUtility.Check.Empty(dateBundleScanDate1))
                {
                    sqlWhere.Append(string.Format(@" and (convert (date, bio.InComing)  >= '{0}' or convert (date, bio.OutGoing) >= '{0}')", Convert.ToDateTime(dateBundleScanDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(dateBundleScanDate2))
                {
                    sqlWhere.Append(string.Format(@" and (convert (date, bio.InComing)  <= '{0}' or convert (date, bio.OutGoing) <= '{0}')", Convert.ToDateTime(dateBundleScanDate2).ToString("d")));
                }
            } 
            if (!MyUtility.Check.Empty(M))
            {
                sqlWhere.Append(string.Format(@" and b.MDivisionid = '{0}'", M));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlWhere.Append(string.Format(@" and o.FtyGroup = '{0}'", Factory));
            }

            if (this.processLocation != "ALL")
            {
                sqlWhere.Append(string.Format(@" and isnull(bio.RFIDProcessLocationID,'') = '{0}'", this.processLocation));
            }

            if (!MyUtility.Check.Empty(dateBDelivery1))
            {
                sqlWhere.Append(string.Format(@" and o.BuyerDelivery >= convert(date,'{0}')", Convert.ToDateTime(dateBDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBDelivery2))
            {
                sqlWhere.Append(string.Format(@" and o.BuyerDelivery <= convert(date,'{0}')", Convert.ToDateTime(dateBDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(dateSewInLine1))
            {
                sqlWhere.Append(string.Format(@" and o.SewInLine >= convert(date,'{0}')", Convert.ToDateTime(dateSewInLine1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateSewInLine2))
            {
                sqlWhere.Append(string.Format(@" and o.SewInLine <= convert(date,'{0}')", Convert.ToDateTime(dateSewInLine2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(dateEstCutDate1))
            {
                sqlWhereWorkOrder.Append(string.Format(@" and w.EstCutDate >= convert(date,'{0}')", Convert.ToDateTime(dateEstCutDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateEstCutDate2))
            {
                sqlWhereWorkOrder.Append(string.Format(@" and w.EstCutDate <= convert(date,'{0}')", Convert.ToDateTime(dateEstCutDate2).ToString("d")));
            }
            #endregion

            #region sqlcmd
            string sqlCmd = string.Empty;
            if (sqlWhereWorkOrder.Length > 0)
            {
                sqlCmd += $@"
select distinct CutRef,MDivisionId
into #tmp_Workorder
from Workorder w
where 1=1
{sqlWhereWorkOrder} and CutRef <> '' and EstCutDate is not null
";
            }

            sqlCmd += $@" 
Select 
    [Bundleno] = bd.BundleNo,
    [RFIDProcessLocationID] = isnull(bio.RFIDProcessLocationID,''),
    [EXCESS] = iif(b.IsEXCESS = 0, '','Y'),
    [Cut Ref#] = isnull(b.CutRef,''),
    [SP#] = b.Orderid,
    [Master SP#] = b.POID,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
	[Category]=o.Category,
	[Program]=o.ProgramID,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Comb] = b.PatternPanel,
    b.Cutno,
	[Fab_Panel Code] = b.FabricPanelCode,
    [Article] = b.Article,
    [Color] = b.ColorId,
    [Line] = b.SewinglineId,
    bio.SewingLineID,
    [Cell] = b.SewingCell,
    [Pattern] = bd.PatternCode,
    [PtnDesc] = bd.PatternDesc,
    [Group] = bd.BundleGroup,
    [Size] = bd.SizeCode,
    [Artwork] = sub.sub,
    [Qty] = bd.Qty,
    [Sub-process] = s.Id,
    [Post Sewing SubProcess]= iif(ps.sub = 1,N'✔',''),
    [No Bundle Card After Subprocess]= iif(nbs.sub= 1,N'✔',''),
    bio.LocationID,
    b.Cdate,
    o.BuyerDelivery,
    o.SewInLine,
    [InComing] = bio.InComing,
    [Out (Time)] = bio.OutGoing,
    [POSupplier] = iif(PoSuppFromOrderID.Value = '',PoSuppFromPOID.Value,PoSuppFromOrderID.Value),
    [AllocatedSubcon]=Stuff((					
					select concat(',',ls.abb)
					from order_tmscost ot
					inner join LocalSupp ls on ls.id = ot.LocalSuppID
					 where ot.id = o.id and ot.ArtworkTypeID=s.ArtworkTypeId 
					for xml path('')
					),1,1,''),
	AvgTime = case  when s.InOutRule = 1 then iif(bio.InComing is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.InComing,''))/24.0,2))
					when s.InOutRule = 2 then iif(bio.OutGoing is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.OutGoing,''))/24.0,2))
					when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then null
					when s.InOutRule = 3 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.InComing,''),isnull(bio.OutGoing,''))/24.0,2))
					when s.InOutRule = 4 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.OutGoing,''),isnull(bio.InComing,''))/24.0,2))
					end,
	TimeRangeFail = case	when s.InOutRule = 1 and bio.InComing is null then 'No Scan'
						when s.InOutRule = 2 and bio.OutGoing is null then 'No Scan'
						when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then 'No Scan'
						when s.InOutRule = 3 and (bio.OutGoing is null or bio.InComing is null) then 'Not Valid'
						when s.InOutRule = 4 and (bio.OutGoing is null or bio.InComing is null) then 'Not Valid'
						else '' end,
	s.InOutRule
	,b.Item
	,bio.PanelNo
	,bio.CutCellID
	,[SpreadingNo] = wk.SpreadingNo
into #result
from Bundle b WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id 
outer apply(
    select s.ID,s.InOutRule,s.ArtworkTypeId
    from SubProcess s
        where exists (
                        select 1 from Bundle_Detail_Art bda
                                where   bda.BundleNo = bd.BundleNo    and
                                        bda.ID = b.ID   and
                                        bda.SubProcessID = s.ID
                        ) or s.IsRFIDDefault = 1
) s
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
outer apply(
	    select sub= stuff((
		    Select distinct concat('+', bda.SubprocessId)
		    from Bundle_Detail_Art bda WITH (NOLOCK) 
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('')
	    ),1,1,'')
) as sub 
outer apply(
    select sub = 1
    from Bundle_Detail_Art bda WITH (NOLOCK) 
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PostSewingSubProcess = 1
    and bda.SubprocessId = s.ID
) as ps
outer apply(
    select sub = 1
    from Bundle_Detail_Art bda WITH (NOLOCK) 
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.NoBundleCardAfterSubprocess = 1
    and bda.SubprocessId = s.ID
) as nbs 
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from ArtworkPO ap with (nolock)
	                                                            inner join ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = b.OrderId 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromOrderID
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' and isnull(PoSuppFromOrderID.Value,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from ArtworkPO ap with (nolock)
	                                                            inner join ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = o.POID 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromPOID
outer apply(
	 select SpreadingNo = stuff((
		    Select distinct concat(',', wo.SpreadingNoID)
		    from WorkOrder wo WITH (NOLOCK) 
		    where   wo.CutRef = b.CutRef 
                    and wo.ID = b.POID
                    and wo.MDivisionID = b.MDivisionID
            and wo.SpreadingNoID is not null
            and wo.SpreadingNoID != ''
		    for xml path('')
	    ),1,1,'')
)wk
";
            if (sqlWhereWorkOrder.Length > 0)
            {
                sqlCmd += " inner join #tmp_Workorder w on b.CutRef = w.CutRef and w.MDivisionId = b.MDivisionid ";
            }

            sqlCmd += $@" where 1=1 {sqlWhere} ";

            string sqlResult = $@"
{sqlCmd}

;with GetCutDateTmp as
(
	select	r.[Cut Ref#],
			r.M,
			[EstCutDate] = MAX(w.EstCutDate),
			[CuttingOutputDate] = MAX(co.cDate)
	from #result r
	inner join WorkOrder w with (nolock) on w.CutRef = r.[Cut Ref#] and w.MDivisionId = r.M and w.id = r.[Master SP#]
	left join CuttingOutput_Detail cod with (nolock) on cod.WorkOrderUkey = w.Ukey
	left join CuttingOutput co  with (nolock) on co.ID = cod.ID
    where r.[Cut Ref#] <> ''
	group by r.[Cut Ref#],r.M
)
select
    r.[Bundleno] ,
    r.[RFIDProcessLocationID],
	r.[EXCESS],
    r.[Cut Ref#] ,
    r.[SP#],
    r.[Master SP#],
    r.[M],
    r.[Factory],
	r.[Category],
	r.[Program],
    r.[Style],
    r.[Season],
    r.[Brand],
    r.[Comb],
    r.Cutno,
	r.[Fab_Panel Code],
    r.[Article],
    r.[Color],
    r.[Line],
    r.SewingLineID,
    r.[Cell],
    r.[Pattern],
    r.[PtnDesc],
    r.[Group],
    r.[Size],
    r.[Artwork],
    r.[Qty],
    r.[Sub-process],
    r.[Post Sewing SubProcess],
    r.[No Bundle Card After Subprocess],
    r.LocationID,
    r.Cdate,
    r.[BuyerDelivery],
    r.[SewInLine],
    r.[InComing],
    r.[Out (Time)],
    r.[POSupplier],
    r.[AllocatedSubcon],
	r.AvgTime,
    [TimeRange] = case	when TimeRangeFail <> '' then TimeRangeFail
                        when AvgTime < 0 then 'Not Valid'
						when AvgTime >= 0 and AvgTime < 1 then '<1'
						when AvgTime >= 1 and AvgTime < 2 then '1-2'
						when AvgTime >= 2 and AvgTime < 3 then '2-3'
						when AvgTime >= 3 and AvgTime < 4 then '3-4'
						when AvgTime >= 4 and AvgTime < 5 then '4-5'
						when AvgTime >= 5 and AvgTime < 10 then '5-10'
						when AvgTime >= 10 and AvgTime < 20 then '10-20'
						when AvgTime >= 20 and AvgTime < 30 then '20-30'
						when AvgTime >= 30 and AvgTime < 40 then '30-40'
						when AvgTime >= 40 and AvgTime < 50 then '40-50'
						when AvgTime >= 50 and AvgTime < 60 then '50-60'
						else '>60' end,
    gcd.EstCutDate,
    gcd.CuttingOutputDate
	,r.Item
	,r.PanelNo
	,r.CutCellID
    ,r.SpreadingNo
from #result r
left join GetCutDateTmp gcd on r.[Cut Ref#] = gcd.[Cut Ref#] and r.M = gcd.M 
order by [Bundleno],[Sub-process],[RFIDProcessLocationID] 

drop table #result
";

            #endregion
            int originalTimeout = DBProxy.Current.DefaultTimeout;
            DBProxy.Current.DefaultTimeout = 1800;
            DataTable groupByDt;
            DualResult result = DBProxy.Current.Select(null, sqlResult, out groupByDt);
            DBProxy.Current.DefaultTimeout = originalTimeout;
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }            

            int ct = groupByDt.Rows.Count;
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
            int num = 100000; 
            int start = 0; 
            DataTable dt = groupByDt.AsEnumerable().Skip(start).Take(num).CopyToDataTable(); 
            while (dt.Rows.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("load {0} records", dt.Rows.Count);

                //do some jobs        
                MyUtility.Excel.CopyToXls(dt, string.Empty, "Subcon_R41_Bundle tracking list (RFID).xltx", 1 + start, false, null, objApp, wSheet: objSheets);
                start += num;
                if (start > ct)
                {
                    break;
                }
                dt = groupByDt.AsEnumerable().Skip(start).Take(num).CopyToDataTable();
            }


            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R41_Bundle tracking list (RFID)");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            if (objSheets != null)
            {
                Marshal.FinalReleaseComObject(objSheets);
                objSheets = null;
            }
            if (objApp != null)
            {
                objApp.Quit();
                Marshal.FinalReleaseComObject(objApp);
                objApp = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            strExcelName.OpenFile();
            #endregion
            return true;
        }
        #endregion
    }
}
