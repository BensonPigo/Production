using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        string style,season,brand,custcd,zone,mDivision,factory,subProcess;
        bool bulk, sample, material, forecast,hisOrder,artwork,pap,seperate,poCombo;
        DateTime? buyerDlv1, buyerDlv2, sciDlv1, sciDlv2, cutoff1, cutoff2, custRQS1, custRQS2, planDate1, planDate2, orderCfm1, orderCfm2;
        DataTable printData, subprocessColumnName, orderArtworkData;
        decimal stdTMS;
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            
            DataTable zone,mDivision, factory,subprocess;
            DBProxy.Current.Select(null, @"select '' as Zone,'' as Fty union all
select distinct f.Zone,f.Zone+' - '+(select CONCAT(ID,'/') from Factory where Zone = f.Zone for XML path('')) as Fty
from Factory f where Zone <> ''", out zone);
            MyUtility.Tool.SetupCombox(comboBox1, 2, zone);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox2, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox3, 1, factory);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from ArtworkType where ReportDropdown = 1", out subprocess);
            MyUtility.Tool.SetupCombox(comboBox4, 1, subprocess);

            comboBox1.SelectedIndex = 0;
            comboBox2.Text = Sci.Env.User.Keyword;
            comboBox3.Text = Sci.Env.User.Factory;
            comboBox4.SelectedIndex = 0;
            checkBox6.Checked = true;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) &&
                MyUtility.Check.Empty(dateRange2.Value1) && MyUtility.Check.Empty(dateRange2.Value2) &&
                MyUtility.Check.Empty(dateRange3.Value1) && MyUtility.Check.Empty(dateRange3.Value2) &&
                MyUtility.Check.Empty(dateRange4.Value1) && MyUtility.Check.Empty(dateRange4.Value2) &&
                MyUtility.Check.Empty(dateRange5.Value1) && MyUtility.Check.Empty(dateRange5.Value2) &&
                MyUtility.Check.Empty(dateRange6.Value1) && MyUtility.Check.Empty(dateRange6.Value2))
            {
                MyUtility.Msg.WarningBox("All date can't empty!!");
                dateRange1.TextBox1.Focus();
                return false;
            }

            buyerDlv1 = dateRange1.Value1;
            buyerDlv2 = dateRange1.Value2;
            sciDlv1 = dateRange2.Value1;
            sciDlv2 = dateRange2.Value2;
            cutoff1 = dateRange3.Value1;
            cutoff2 = dateRange3.Value2;
            custRQS1 = dateRange4.Value1;
            custRQS2 = dateRange4.Value2;
            planDate1 = dateRange5.Value1;
            planDate2 = dateRange5.Value2;
            orderCfm1 = dateRange6.Value1;
            orderCfm2 = dateRange6.Value2;
           
            zone = MyUtility.Convert.GetString(comboBox1.SelectedValue);
            mDivision = comboBox2.Text;
            factory = comboBox3.Text;
            bulk = checkBox6.Checked;
            sample = checkBox7.Checked;
            material = checkBox8.Checked;
            forecast = checkBox9.Checked;
            subProcess = comboBox4.Text;
            hisOrder = checkBox1.Checked;
            artwork = checkBox2.Checked;
            pap = checkBox3.Checked;
            seperate = checkBox4.Checked;
            poCombo = checkBox5.Checked;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            sqlCmd.Append(@"with tmpOrders
as (
select o.ID,o.MDivisionID,o.FtyGroup,o.FactoryID,o.BuyerDelivery,o.SciDelivery,o.POID,o.CRDDate,o.CFMDate,
o.Dest,o.StyleID,o.SeasonID,o.BrandID,o.ProjectID,o.Customize1,o.BuyMonth,o.CustPONo,o.CustCDID,o.ProgramID,
o.CdCodeID,o.CPU,o.Qty,o.FOCQty,o.LocalOrder,o.PoPrice,o.CMPPrice,o.KPILETA,o.LETA,o.MTLETA,o.SewETA,
o.PackETA,o.MTLComplete,o.SewInLine,o.SewOffLine,o.CutInLine,o.CutOffLine,o.Category,o.PulloutDate,
o.ActPulloutDate,o.SMR,o.MRHandle,o.MCHandle,o.OrigBuyerDelivery,o.DoxType,o.TotalCTN,o.FtyCTN,o.ClogCTN,
o.VasShas,o.TissuePaper,o.MTLExport,o.SewLine,o.ShipModeList,o.PlanDate,o.FirstProduction,o.OrderTypeID,
o.SpecialMark,o.SampleReason,o.InspDate,IIF(o.InspResult='P','Pass',IIF(o.InspResult='F','Fail',''))InspResult
,(o.InspHandle +'-'+ I.Name)InspHandle,o.MnorderApv,o.PulloutComplete,
o.FtyKPI,o.KPIChangeReason,o.EachConsApv,o.Junk,o.StyleUkey,o.CuttingSP,o.RainwearTestPassed,o.BrandFTYCode,
o.CPUFactor,o.ClogLastReceiveDate,o.IsMixMarker,o.GFR
from Orders o
inner join Order_QtyShip oq on o.ID = oq.Id
OUTER APPLY(SELECT Name FROM Pass1 WHERE Pass1.ID=O.InspHandle)I
where 1=1");
            if (!MyUtility.Check.Empty(buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'",Convert.ToDateTime(buyerDlv1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'",Convert.ToDateTime(buyerDlv2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'",Convert.ToDateTime(sciDlv1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'",Convert.ToDateTime(sciDlv2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(cutoff1))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate >= '{0}'",Convert.ToDateTime(cutoff1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(cutoff2))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate <= '{0}'",Convert.ToDateTime(cutoff2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(custRQS1))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate >= '{0}'",Convert.ToDateTime(custRQS1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(custRQS2))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate <= '{0}'",Convert.ToDateTime(custRQS2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate1))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate >= '{0}'",Convert.ToDateTime(planDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate2))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate <= '{0}'",Convert.ToDateTime(planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(orderCfm1))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate >= '{0}'",Convert.ToDateTime(orderCfm1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(orderCfm2))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate <= '{0}'",Convert.ToDateTime(orderCfm2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'",style));
            }
            if (!MyUtility.Check.Empty(season))
            {
                sqlCmd.Append(string.Format(" and o.SeasonID = '{0}'",season));
            }
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'",brand));
            }
            if (!MyUtility.Check.Empty(custcd))
            {
                sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'",custcd));
            }
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'",mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'",factory));
            }
            if (!hisOrder)
            {
                sqlCmd.Append(" and o.Finished = 0");
            }
            if (bulk || sample || material || forecast)
            {
                sqlCmd.Append(" and (1=0");
                if (bulk)
                {
                    sqlCmd.Append(" or o.Category = 'B'");
                }
                if (sample)
                {
                    sqlCmd.Append(" or o.Category = 'S'");
                }
                if (material)
                {
                    sqlCmd.Append(" or o.Category = 'M'");
                }
                if (forecast)
                {
                    sqlCmd.Append(" or o.Category = ''");
                }
                sqlCmd.Append(")");
            }
            sqlCmd.Append(@"
),
tmpFilterZone
as (
select t.* 
from tmpOrders t");
            if (!MyUtility.Check.Empty(zone))
            {
                sqlCmd.Append(string.Format(@"
inner join Factory f on t.FactoryID = f.ID
where f.Zone = '{0}'",zone));
            }

            sqlCmd.Append(@"
),
tmpFilterSubProcess
as (
select t.*
from tmpFilterZone t");
            if (!MyUtility.Check.Empty(subProcess))
            {
                sqlCmd.Append(string.Format(@"
inner join Style_TmsCost st on t.StyleUkey = st.StyleUkey
where st.ArtworkTypeID = '{0}'", subProcess));
            }
            if (poCombo)
            {
                sqlCmd.Append(@"
),
tmpListPoCombo
as (
select * from tmpFilterSubProcess
union
select o.ID,o.MDivisionID,o.FtyGroup,o.FactoryID,o.BuyerDelivery,o.SciDelivery,o.POID,o.CRDDate,o.CFMDate,
o.Dest,o.StyleID,o.SeasonID,o.BrandID,o.ProjectID,o.Customize1,o.BuyMonth,o.CustPONo,o.CustCDID,o.ProgramID,
o.CdCodeID,o.CPU,o.Qty,o.FOCQty,o.LocalOrder,o.PoPrice,o.CMPPrice,o.KPILETA,o.LETA,o.MTLETA,o.SewETA,
o.PackETA,o.MTLComplete,o.SewInLine,o.SewOffLine,o.CutInLine,o.CutOffLine,o.Category,o.PulloutDate,
o.ActPulloutDate,o.SMR,o.MRHandle,o.MCHandle,o.OrigBuyerDelivery,o.DoxType,o.TotalCTN,o.FtyCTN,o.ClogCTN,
o.VasShas,o.TissuePaper,o.MTLExport,o.SewLine,o.ShipModeList,o.PlanDate,o.FirstProduction,o.OrderTypeID,
o.SpecialMark,o.SampleReason,o.InspDate,IIF(o.InspResult='P','Pass',IIF(o.InspResult='F','Fail',''))InspResult,
o.InspHandle,o.MnorderApv,o.PulloutComplete,o.FtyKPI,
o.KPIChangeReason,o.EachConsApv,o.Junk,o.StyleUkey,o.CuttingSP,o.RainwearTestPassed,o.BrandFTYCode,o.CPUFactor,
o.ClogLastReceiveDate,o.IsMixMarker,o.GFR
from Orders o 
where POID in (select distinct POID from tmpFilterSubProcess)
)");
            }
            else
            {
                sqlCmd.Append(@"
),
tmpListPoCombo
as (
select * from tmpFilterSubProcess
)");
            }
            if (seperate)
            {
                sqlCmd.Append(@",
tmpFilterSeperate
as (
select t.ID,t.MDivisionID,t.FtyGroup,t.FactoryID,oq.BuyerDelivery,t.SciDelivery,t.POID,t.CRDDate,t.CFMDate,
t.Dest,t.StyleID,t.SeasonID,t.BrandID,t.ProjectID,t.Customize1,t.BuyMonth,t.CustPONo,t.CustCDID,t.ProgramID,
t.CdCodeID,t.CPU,oq.Qty,t.FOCQty,t.LocalOrder,t.PoPrice,t.CMPPrice,t.KPILETA,t.LETA,t.MTLETA,t.SewETA,
t.PackETA,t.MTLComplete,t.SewInLine,t.SewOffLine,t.CutInLine,t.CutOffLine,t.Category,oq.EstPulloutDate as PulloutDate,
(select MAX(p.PulloutDate) from PackingList p, PackingList_Detail pd where p.ID = pd.ID and pd.OrderID = t.ID and pd.OrderShipmodeSeq = oq.Seq) as ActPulloutDate,
t.SMR,t.MRHandle,t.MCHandle,t.OrigBuyerDelivery,t.DoxType,
isnull((select SUM(pd.CTNQty) from PackingList_Detail pd where pd.OrderID = t.ID and pd.OrderShipmodeSeq = oq.Seq),0) as TotalCTN,
isnull((select SUM(pd.CTNQty) from PackingList_Detail pd where pd.OrderID = t.ID and pd.OrderShipmodeSeq = oq.Seq and pd.TransferDate is null),0) as FtyCTN,
isnull((select SUM(pd.CTNQty) from PackingList_Detail pd where pd.OrderID = t.ID and pd.OrderShipmodeSeq = oq.Seq and pd.ReceiveDate is not null),0) as ClogCTN,
t.VasShas,t.TissuePaper,t.MTLExport,t.SewLine,oq.ShipmodeID as ShipModeList,t.PlanDate,t.FirstProduction,t.OrderTypeID,
t.SpecialMark,t.SampleReason,t.InspDate,t.InspResult,t.InspHandle,t.MnorderApv,t.PulloutComplete,t.FtyKPI,t.KPIChangeReason,t.EachConsApv,
t.Junk,t.StyleUkey,t.CuttingSP,t.RainwearTestPassed,t.BrandFTYCode,t.CPUFactor,oq.Seq,t.ClogLastReceiveDate,t.IsMixMarker,t.GFR
from tmpListPoCombo t
inner join Order_QtyShip oq on t.ID = oq.Id
)

select t.*,isnull(s.ModularParent,'') as ModularParent,isnull(s.CPUAdjusted*100,0) as CPUAdjusted,isnull(c.Alias,'') as DestAlias,
isnull(s.ExpectionForm,'') as ExpectionForm,isnull(s.ExpectionFormRemark,'') as ExpectionFormRemark,isnull(ct.WorkType,'') as WorkType,
ct.FirstCutDate,isnull(p.POSMR,'') as POSMR,isnull(p.POHandle,'') as POHandle,isnull(s.FTYRemark,'') as FTYRemark,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'T'),0) as SewQtyTop,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'B'),0) as SewQtyBottom,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'I'),0) as SewQtyInner,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'O'),0) as SewQtyOuter,
isnull(dbo.getMinCompleteSewQty(t.ID,null,null),0) as TtlSewQty,
isnull((select SUM(Qty) from CuttingOutput_WIP where OrderID = t.ID),0) as CutQty,
isnull((select top 1 Remark from Order_PFHis where ID = t.ID and AddDate = (select Max(AddDate) from Order_PFHis where ID = t.ID)),'') as PFRemark,
dbo.getMinSCIDelivery(t.POID,'') as EarliestSCIDlv,
isnull((select Name from Reason where ReasonTypeID = 'Style_SpecialMark' and ID = t.KPIChangeReason),'') as KPIChangeReasonName,
isnull((select Name from TPEPass1 where Id = t.SMR),'') as SMRName,
isnull((select Name from TPEPass1 where Id = t.MRHandle),'') as MRHandleName,
isnull((select Name from TPEPass1 where Id = p.POSMR),'') as POSMRName,
isnull((select Name from TPEPass1 where Id = p.POHandle),'') as POHandleName,
isnull((select Name from Pass1 where Id = t.MCHandle),'') as MCHandleName,
isnull((select Name from Reason where ReasonTypeID = 'Order_reMakeSample' and ID = t.SampleReason),'') as SampleReasonName,
isnull([dbo].getMTLExport(t.POID,t.MTLExport),'') as MTLExportTimes,
dbo.GetStyleGMTLT(t.BrandID,t.StyleID,t.SeasonID,t.FactoryID) as GMTLT,
dbo.GetSimilarStyleList(t.StyleUkey) as SimilarStyle,
dbo.GetHaveDelaySupp(t.POID) as MTLDelay,
isnull((select SUM(pld.ShipQty) from PackingList pl,PackingList_Detail pld where pl.ID = pld.ID and pl.Type <> 'F' and pld.OrderID = t.ID and pld.OrderShipmodeSeq = t.Seq),0) as PackingQty,
isnull((select SUM(pld.ShipQty) from PackingList pl,PackingList_Detail pld where pl.ID = pld.ID and pl.Type = 'F' and pld.OrderID = t.ID and pld.OrderShipmodeSeq = t.Seq),0) as PackingFOCQty,
isnull((select SUM(pld.ShipQty) from PackingList pl,PackingList_Detail pld where pl.ID = pld.ID and (pl.Type = 'B' or pl.Type = 'S') and pl.INVNo <> '' and pld.OrderID = t.ID and pld.OrderShipmodeSeq = t.Seq),0) as BookingQty,
dbo.getInvAdjQty(t.ID,t.Seq) as InvoiceAdjQty,ct.FirstCutDate,
(select Max(e.WhseArrival) from Export e, Export_Detail ed where e.ID = ed.ID and ed.POID = t.POID) as ArriveWHDate,
(select Min(so.OutputDate) from SewingOutput so, SewingOutput_Detail sod where so.ID = sod.ID and sod.OrderID = t.ID) as FirstOutDate,
(select Max(so.OutputDate) from SewingOutput so, SewingOutput_Detail sod where so.ID = sod.ID and sod.OrderID = t.ID) as LastOutDate,
isnull((select Sum(pod.ShipQty) from Pullout_Detail pod where pod.OrderID = t.ID and pod.OrderShipmodeSeq = t.Seq),0) as PulloutQty,
IIF(t.PulloutComplete = 1,'OK',(select CONVERT(varchar(3),isnull(Count(Distinct ID),0)) from Pullout_Detail where OrderID = t.ID and OrderShipmodeSeq = t.Seq and ShipQty > 0)) as ActPulloutTime,
isnull((select Sum(CTNQty) from PackingList_Detail where OrderID = t.ID and OrderShipmodeSeq = t.Seq),0) as PackingCTN,
isnull((select Sum(CTNQty) from PackingList_Detail where OrderID = t.ID and OrderShipmodeSeq = t.Seq),0) as TotalCTN,
isnull((select Sum(CTNQty) from PackingList_Detail pd where pd.OrderID = t.ID and pd.OrderShipmodeSeq = t.Seq and pd.TransferDate is null),0) as FtyCtn,
isnull((select Sum(CTNQty) from PackingList_Detail pd where pd.OrderID = t.ID and pd.OrderShipmodeSeq = t.Seq and pd.ReceiveDate is not null),0) as ClogCTN,
(Select Max(ReceiveDate) from PackingList_Detail where OrderID = t.ID and OrderShipmodeSeq = t.Seq) as ClogRcvDate,
isnull((select CONCAT(Article,',') from (select distinct Article from Order_QtyShip_Detail where ID = t.ID and Seq = t.Seq) a for xml path('')),'') as Article
from tmpFilterSeperate t
left join Style s on s.Ukey = t.StyleUkey
left join Country c on c.ID = t.Dest
left join Cutting ct on ct.ID = t.CuttingSP
left join PO p on p.ID = t.POID
order by t.ID");
            }
            else
            {
                sqlCmd.Append(@"
select t.*,isnull(s.ModularParent,'') as ModularParent,isnull(s.CPUAdjusted*100,0) as CPUAdjusted,isnull(c.Alias,'') as DestAlias,
isnull(s.ExpectionForm,'') as ExpectionForm,isnull(s.ExpectionFormRemark,'') as ExpectionFormRemark,isnull(ct.WorkType,'') as WorkType,
ct.FirstCutDate,isnull(p.POSMR,'') as POSMR,isnull(p.POHandle,'') as POHandle,isnull(s.FTYRemark,'') as FTYRemark,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'T'),0) as SewQtyTop,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'B'),0) as SewQtyBottom,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'I'),0) as SewQtyInner,
isnull((select SUM(QAQty) from SewingOutput_Detail where OrderId = t.ID and ComboType = 'O'),0) as SewQtyOuter,
isnull(dbo.getMinCompleteSewQty(t.ID,null,null),0) as TtlSewQty,
isnull((select SUM(Qty) from CuttingOutput_WIP where OrderID = t.ID),0) as CutQty,
isnull((select top 1 Remark from Order_PFHis where ID = t.ID and AddDate = (select Max(AddDate) from Order_PFHis where ID = t.ID)),'') as PFRemark,
dbo.getMinSCIDelivery(t.POID,'') as EarliestSCIDlv,
isnull((select Name from Reason where ReasonTypeID = 'Order_BuyerDelivery' and ID = t.KPIChangeReason),'') as KPIChangeReasonName,
isnull((select Name from TPEPass1 where Id = t.SMR),'') as SMRName,
isnull((select Name from TPEPass1 where Id = t.MRHandle),'') as MRHandleName,
isnull((select Name from TPEPass1 where Id = p.POSMR),'') as POSMRName,
isnull((select Name from TPEPass1 where Id = p.POHandle),'') as POHandleName,
isnull((select Name from Pass1 where Id = t.MCHandle),'') as MCHandleName,
isnull((select Name from Reason where ReasonTypeID = 'Order_reMakeSample' and ID = t.SampleReason),'') as SampleReasonName,
isnull((select Name from Reason where ReasonTypeID = 'Style_SpecialMark' and ID = t.SpecialMark),'') as SpecialMarkName,
isnull([dbo].getMTLExport(t.POID,t.MTLExport),'') as MTLExportTimes,
dbo.GetStyleGMTLT(t.BrandID,t.StyleID,t.SeasonID,t.FactoryID) as GMTLT,
dbo.GetSimilarStyleList(t.StyleUkey) as SimilarStyle,
dbo.GetHaveDelaySupp(t.POID) as MTLDelay,
isnull((select SUM(pld.ShipQty) from PackingList pl,PackingList_Detail pld where pl.ID = pld.ID and pl.Type <> 'F' and pld.OrderID = t.ID),0) as PackingQty,
isnull((select SUM(pld.ShipQty) from PackingList pl,PackingList_Detail pld where pl.ID = pld.ID and pl.Type = 'F' and pld.OrderID = t.ID),0) as PackingFOCQty,
isnull((select SUM(pld.ShipQty) from PackingList pl,PackingList_Detail pld where pl.ID = pld.ID and (pl.Type = 'B' or pl.Type = 'S') and pl.INVNo <> '' and pld.OrderID = t.ID),0) as BookingQty,
isnull((select sum(iq.DiffQty) from InvAdjust i, InvAdjust_Qty iq where i.ID = iq.ID and i.OrderID = t.ID),0) as InvoiceAdjQty,ct.FirstCutDate,
(select Max(e.WhseArrival) from Export e, Export_Detail ed where e.ID = ed.ID and ed.POID = t.POID) as ArriveWHDate,
(select Min(so.OutputDate) from SewingOutput so, SewingOutput_Detail sod where so.ID = sod.ID and sod.OrderID = t.ID) as FirstOutDate,
(select Max(so.OutputDate) from SewingOutput so, SewingOutput_Detail sod where so.ID = sod.ID and sod.OrderID = t.ID) as LastOutDate,
isnull((select Sum(pod.ShipQty) from Pullout_Detail pod where pod.OrderID = t.ID),0) as PulloutQty,
dbo.getPulloutComplete(t.ID,t.PulloutComplete) as ActPulloutTime,
isnull((select Sum(CTNQty) from PackingList_Detail where OrderID = t.ID),0) as PackingCTN,
t.TotalCTN,
t.TotalCTN-t.FtyCTN as FtyCtn,
t.ClogCTN,
t.ClogLastReceiveDate as ClogRcvDate,
isnull((select CONCAT(Article,',') from (select distinct Article from Order_Qty where ID = t.ID) a for xml path('')),'') as Article
from tmpListPoCombo t
left join Style s on s.Ukey = t.StyleUkey
left join Country c on c.ID = t.Dest
left join Cutting ct on ct.ID = t.CuttingSP
left join PO p on p.ID = t.POID
order by t.ID");
            }
            #endregion
            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            if (printData.Rows.Count > 0)
            {
                if (artwork || pap)
                {
                    #region 組Subprocess欄位名稱
                    string classify;
                    if (artwork && pap)
                    {
                        classify = "'I','S','A','O','P'";
                    }
                    else if (artwork)
                    {
                        classify = "'I','S','A','O'";
                    }
                    else
                    {
                        classify = "'P'";
                    }
                    sqlCmd.Clear();
                    sqlCmd.Append(string.Format(@"With SubProcess
as (
select *,(ROW_NUMBER() OVER (ORDER BY a.ID, a.ColumnSeq)) as rno from (
SELECT ID,Seq,ArtworkUnit,ProductionUnit,SystemType,Seq+'U1' as FakeID,RTRIM(ID)+' ('+ArtworkUnit+')' as ColumnN, '1' as ColumnSeq FROM ArtworkType
WHERE ArtworkUnit <> '' and Classify in ({0}) and Junk = 0
union all
SELECT ID,Seq,ArtworkUnit,ProductionUnit,SystemType,Seq+'U2' as FakeID,RTRIM(ID)+' ('+IIF(ProductionUnit = 'QTY','Price',ProductionUnit)+')' as ColumnN, '2' as ColumnSeq FROM ArtworkType
WHERE ProductionUnit <> '' and Classify in ({0}) and Junk = 0
union all
SELECT ID,Seq,ArtworkUnit,ProductionUnit,SystemType,Seq+'N' as FakeID,RTRIM(ID) as ColumnN, '3' as ColumnSeq FROM ArtworkType
WHERE ArtworkUnit = '' and ProductionUnit = '' and Classify in ({0}) and Junk = 0
{1}
) a
),
TTL_Subprocess
as (
select 'TTL'+ID as ID,Seq,ArtworkUnit,ProductionUnit,SystemType,'T'+FakeID as FakeID,'TTL_'+ColumnN as ColumnN, ColumnSeq,(ROW_NUMBER() OVER (ORDER BY ID, ColumnSeq))+1000 as rno from SubProcess where ID <> 'PrintSubCon'
)
select ID,Seq,ArtworkUnit,ProductionUnit,SystemType,FakeID,ColumnN,ColumnSeq,(ROW_NUMBER() OVER (ORDER BY a.rno))+{2} as rno from (
select * from SubProcess
union all
SELECT 'TTLTMS' as ID,'' as Seq,'' as ArtworkUnit,'' as ProductionUnit, '' as SystemType,'TTLTMS' as FakeID,'TTL_TMS' as ColumnN,'' as ColumnSeq,'999' as rno
union
select * from TTL_Subprocess) a", classify, (!artwork ? "" : @"union all
SELECT 'PrintSubCon' as ID,'' as Seq,'' as ArtworkUnit,'' as ProductionUnit, '' as SystemType,'9999ZZ' as FakeID,'SubCon' as ColumnN,'999' as ColumnSeq"),
                                                                                                                                                            "112"));
                    #endregion
                    result = DBProxy.Current.Select(null, sqlCmd.ToString(), out subprocessColumnName);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query artworktype fail\r\n" + result.ToString());
                        return failResult;
                    }

                    #region 撈Order Subprocess資料
                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable(subprocessColumnName,
                            "ID,Seq,ArtworkUnit,ProductionUnit,SystemType,FakeID,ColumnN,ColumnSeq,rno",
                            @"with ArtworkData 
as (
select * from #tmp
)
select ot.ID,ot.ArtworkTypeID,ot.ArtworkUnit,at.ProductionUnit,ot.Qty,ot.TMS,ot.Price,
IIF(ot.ArtworkTypeID = 'PRINTING',IIF(ot.InhouseOSP = 'O',(select Abb from LocalSupp where ID = LocalSuppID),ot.LocalSuppID),'') as Supp,
a.rno as AUnitRno, a1.rno as PUnitRno, a2.rno as NRno,
a3.rno as TAUnitRno, a4.rno as TPUnitRno, a5.rno as TNRno  
from Order_TmsCost ot
left join ArtworkType at on at.ID = ot.ArtworkTypeID
left join ArtworkData a on a.FakeID = ot.Seq+'U1' 
left join ArtworkData a1 on a1.FakeID = ot.Seq+'U2'
left join ArtworkData a2 on a2.FakeID = ot.Seq
left join ArtworkData a3 on a3.FakeID = 'T'+ot.Seq+'U1' 
left join ArtworkData a4 on a4.FakeID = 'T'+ot.Seq+'U2'
left join ArtworkData a5 on a5.FakeID = 'T'+ot.Seq", out orderArtworkData);
                    }
                    catch (Exception ex)
                    {
                        DualResult failResult = new DualResult(false, "Query order tms & cost fail\r\n" + ex.ToString());
                    }
                    #endregion
                }
            }
            DBProxy.Current.DefaultTimeout = 0;
            stdTMS = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select StdTMS from System"));
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R03_PPICMasterList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            //填Subprocess欄位名稱
            int lastCol = 113;
            int subConCol = 9999, ttlTMS = 113; //紀錄SubCon與TTL_TMS的欄位
            if (artwork || pap)
            {
                foreach (DataRow dr in subprocessColumnName.Rows)
                {
                    worksheet.Cells[1, MyUtility.Convert.GetInt(dr["rno"])] = MyUtility.Convert.GetString(dr["ColumnN"]);
                    lastCol = MyUtility.Convert.GetInt(dr["rno"]);
                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "SUBCON")
                    {
                        subConCol = MyUtility.Convert.GetInt(dr["rno"]);
                    }
                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "TTL_TMS")
                    {
                        ttlTMS = MyUtility.Convert.GetInt(dr["rno"]);
                    }
                }
            }
            else
            {
                worksheet.Cells[1, 113] = "TTL_TMS";
            }

            //算出Excel的Column的英文位置
            string excelColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(lastCol);

            //填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, lastCol];
           
            foreach (DataRow dr in printData.Rows)
            {
                #region 填固定欄位資料
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["BuyerDelivery"];
                objArray[0, 3] = MyUtility.Check.Empty(dr["BuyerDelivery"]) ? "" : Convert.ToDateTime(dr["BuyerDelivery"]).ToString("yyyyMM");
                objArray[0, 4] = dr["EarliestSCIDlv"];
                objArray[0, 5] = dr["SciDelivery"];
                objArray[0, 6] = dr["CRDDate"];
                objArray[0, 7] = MyUtility.Check.Empty(dr["CRDDate"]) ? "" : Convert.ToDateTime(dr["CRDDate"]).ToString("yyyyMM");
                objArray[0, 8] = MyUtility.Convert.GetDate(dr["BuyerDelivery"]) != MyUtility.Convert.GetDate(dr["CRDDate"]) ? "Y" : "";
                objArray[0, 9] = dr["CFMDate"];
                objArray[0, 10] = MyUtility.Check.Empty(dr["CRDDate"]) || MyUtility.Check.Empty(dr["CFMDate"]) ? 0 : Convert.ToInt32((Convert.ToDateTime(dr["CRDDate"]) - Convert.ToDateTime(dr["CFMDate"])).TotalDays);
                objArray[0, 11] = dr["ID"];
                objArray[0, 12] = dr["Category"];
                objArray[0, 13] = MyUtility.Convert.GetString(dr["Junk"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 14] = dr["DestAlias"];
                objArray[0, 15] = dr["StyleID"];
                objArray[0, 16] = dr["ModularParent"];
                objArray[0, 17] = dr["CPUAdjusted"];
                objArray[0, 18] = dr["SimilarStyle"];
                objArray[0, 19] = dr["SeasonID"];
                objArray[0, 20] = dr["GMTLT"];
                objArray[0, 21] = dr["OrderTypeID"];
                objArray[0, 22] = dr["ProjectID"];
                objArray[0, 23] = dr["Customize1"];
                objArray[0, 24] = dr["BuyMonth"];
                objArray[0, 25] = dr["CustPONo"];
                objArray[0, 26] = MyUtility.Convert.GetString(dr["VasShas"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 27] = dr["MnorderApv"];
                objArray[0, 28] = MyUtility.Convert.GetString(dr["TissuePaper"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 29] = dr["ExpectionForm"];
                objArray[0, 30] = dr["ExpectionFormRemark"];
                objArray[0, 31] = MyUtility.Convert.GetString(dr["GFR"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 32] = dr["BrandID"];
                objArray[0, 33] = dr["CustCDID"];
                objArray[0, 34] = dr["BrandFTYCode"];
                objArray[0, 35] = dr["ProgramID"];
                objArray[0, 36] = dr["CdCodeID"];
                objArray[0, 37] = dr["CPU"];
                objArray[0, 38] = dr["Qty"];
                objArray[0, 39] = dr["FOCQty"];
                objArray[0, 40] = MyUtility.Convert.GetDecimal(dr["CPU"]) * MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPUFactor"]);
                objArray[0, 41] = dr["SewQtyTop"];
                objArray[0, 42] = dr["SewQtyBottom"];
                objArray[0, 43] = dr["SewQtyInner"];
                objArray[0, 44] = dr["SewQtyOuter"];
                objArray[0, 45] = dr["TtlSewQty"];
                objArray[0, 46] = dr["CutQty"];
                objArray[0, 47] = MyUtility.Convert.GetString(dr["WorkType"]) == "1" ? "Y" : "";
                objArray[0, 48] = MyUtility.Convert.GetInt(dr["CutQty"]) >= MyUtility.Convert.GetInt(dr["Qty"]) ? "Y" : "";
                objArray[0, 49] = dr["PackingQty"];
                objArray[0, 50] = dr["PackingFOCQty"];
                objArray[0, 51] = dr["BookingQty"];
                objArray[0, 52] = dr["InvoiceAdjQty"];
                objArray[0, 53] = dr["PoPrice"];
                objArray[0, 54] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["PoPrice"]);
                objArray[0, 55] = MyUtility.Convert.GetString(dr["LocalOrder"]).ToUpper() == "TRUE" ? dr["PoPrice"] : dr["CMPPrice"];
                objArray[0, 56] = dr["KPILETA"];
                objArray[0, 57] = dr["PFRemark"];
                objArray[0, 58] = dr["LETA"];
                objArray[0, 59] = dr["MTLETA"];
                objArray[0, 60] = dr["SewETA"];
                objArray[0, 61] = dr["PackETA"];
                objArray[0, 62] = MyUtility.Convert.GetString(dr["MTLDelay"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 63] = MyUtility.Check.Empty(dr["MTLExport"]) ? dr["MTLExportTimes"] : dr["MTLExport"];
                objArray[0, 64] = MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 65] = dr["ArriveWHDate"];
                objArray[0, 66] = dr["SewInLine"];
                objArray[0, 67] = dr["SewOffLine"];
                objArray[0, 68] = dr["FirstOutDate"];
                objArray[0, 69] = dr["LastOutDate"];
                objArray[0, 70] = dr["EachConsApv"];
                objArray[0, 71] = dr["CutInLine"];
                objArray[0, 72] = dr["CutOffLine"];
                objArray[0, 73] = dr["FirstCutDate"];
                objArray[0, 74] = dr["PulloutDate"];
                objArray[0, 75] = dr["ActPulloutDate"];
                objArray[0, 76] = dr["PulloutQty"];
                objArray[0, 77] = dr["ActPulloutTime"];
                objArray[0, 78] = MyUtility.Convert.GetString(dr["PulloutComplete"]).ToUpper() == "TRUE" ? "OK" : "";
                objArray[0, 79] = dr["FtyKPI"]; //cb
                objArray[0, 80] = dr["KPIChangeReasonName"]; //cc
                objArray[0, 81] = dr["PlanDate"];
                objArray[0, 82] = dr["OrigBuyerDelivery"];
                objArray[0, 83] = dr["SMR"];
                objArray[0, 84] = dr["SMRName"];
                objArray[0, 85] = dr["MRHandle"];
                objArray[0, 86] = dr["MRHandleName"];
                objArray[0, 87] = dr["POSMR"];
                objArray[0, 88] = dr["POSMRName"];
                objArray[0, 89] = dr["POHandle"];
                objArray[0, 90] = dr["POHandleName"];
                objArray[0, 91] = dr["MCHandle"];
                objArray[0, 92] = dr["MCHandleName"];
                objArray[0, 93] = dr["DoxType"];
                objArray[0, 94] = dr["PackingCTN"];
                objArray[0, 95] = dr["TotalCTN"];
                objArray[0, 96] = dr["FtyCtn"];
                objArray[0, 97] = dr["ClogCTN"];
                objArray[0, 98] = dr["ClogRcvDate"];
                objArray[0, 99] = dr["InspDate"];
                objArray[0, 100] = dr["InspResult"];//CW
                objArray[0, 101] = dr["InspHandle"];//CX
                objArray[0, 102] = dr["SewLine"];//CY
                objArray[0, 103] = dr["ShipModeList"];//CZ
                objArray[0, 104] = dr["Article"];//DA
                objArray[0, 105] = dr["SpecialMarkName"];//DB
                objArray[0, 106] = dr["FTYRemark"];//DC
                objArray[0, 107] = dr["SampleReasonName"];//DD
                objArray[0, 108] = MyUtility.Convert.GetString(dr["IsMixMarker"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 109] = dr["CuttingSP"];
                objArray[0, 110] = MyUtility.Convert.GetString(dr["RainwearTestPassed"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[0, 111] = MyUtility.Convert.GetDecimal(dr["CPU"])*stdTMS;
                #endregion
                //先清空Subprocess值
                for (int i = 112; i < lastCol; i++)
                {
                    objArray[0, i] = 0;
                }

                if (artwork || pap)
                {
                    DataRow[] finRow = orderArtworkData.Select(string.Format("ID = '{0}'", MyUtility.Convert.GetString(dr["ID"])));
                    if (finRow.Length > 0)
                    {
                        foreach (DataRow sdr in finRow)
                        {
                            if (!MyUtility.Check.Empty(sdr["AUnitRno"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["AUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }
                            if (!MyUtility.Check.Empty(sdr["PUnitRno"]))
                            {
                                if (MyUtility.Convert.GetString(sdr["ProductionUnit"]).ToUpper() == "TMS")
                                {
                                    objArray[0, MyUtility.Convert.GetInt(sdr["PUnitRno"]) - 1] = sdr["TMS"];
                                }
                                else
                                {
                                    objArray[0, MyUtility.Convert.GetInt(sdr["PUnitRno"]) - 1] = sdr["Price"];
                                }
                            }

                            if (!MyUtility.Check.Empty(sdr["NRno"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["NRno"]) - 1] = MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            //TTL
                            if (!MyUtility.Check.Empty(sdr["TAUnitRno"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["TAUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }
                            if (!MyUtility.Check.Empty(sdr["TPUnitRno"]))
                            {
                                if (MyUtility.Convert.GetString(sdr["ProductionUnit"]).ToUpper() == "TMS")
                                {
                                    objArray[0, MyUtility.Convert.GetInt(sdr["TPUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["TMS"]);
                                }
                                else
                                {
                                    objArray[0, MyUtility.Convert.GetInt(sdr["TPUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Price"]);
                                }
                            }

                            if (!MyUtility.Check.Empty(sdr["TNRno"]))
                            {
                                objArray[0, MyUtility.Convert.GetInt(sdr["TNRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            if (subConCol != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["Supp"]))
                                {
                                    objArray[0, subConCol - 1] = sdr["Supp"];
                                }
                                else
                                {
                                    objArray[0, subConCol - 1] = "";
                                }
                            }
                        }
                    }
                    objArray[0, ttlTMS-1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * stdTMS;
                }
                else
                {
                    objArray[0, 112] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * stdTMS;
                }

                worksheet.Range[String.Format("A{0}:{1}{0}", intRowsStart, excelColEng)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();

            excel.Visible = true;
            return true;
        }

    }
}
