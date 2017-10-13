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
using System.Runtime.InteropServices;
using System.Linq;

namespace Sci.Production.PPIC
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        string style,season,brand,custcd;
        string zone, mDivision, factory, subProcess;
        bool bulk, sample, material, forecast,hisOrder,artwork,pap,seperate,poCombo;
        DateTime? buyerDlv1, buyerDlv2, sciDlv1, sciDlv2, cutoff1, cutoff2, custRQS1, custRQS2, planDate1, planDate2, orderCfm1, orderCfm2;
        DataTable printData, subprocessColumnName,  orderArtworkData;
        decimal stdTMS; int subtrue = 0;
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            
            DataTable zone,mDivision, factory,subprocess;
            DBProxy.Current.Select(null, @"select '' as Zone,'' as Fty union all
select distinct f.Zone,f.Zone+' - '+(select CONCAT(ID,'/') from Factory WITH (NOLOCK) where Zone = f.Zone for XML path('')) as Fty
from Factory f WITH (NOLOCK) where Zone <> ''", out zone);
            MyUtility.Tool.SetupCombox(comboZone, 2, zone);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from ArtworkType WITH (NOLOCK) where ReportDropdown = 1", out subprocess);
            MyUtility.Tool.SetupCombox(comboSubProcess, 1, subprocess);

            comboZone.SelectedIndex = 0;
            comboM.Text = Sci.Env.User.Keyword;
            comboFactory.Text = Sci.Env.User.Factory;
            comboSubProcess.SelectedIndex = 0;
            checkBulk.Checked = true;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBuyerDelivery.Value1) && MyUtility.Check.Empty(dateBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(dateSCIDelivery.Value1) && MyUtility.Check.Empty(dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(dateCutOffDate.Value1) && MyUtility.Check.Empty(dateCutOffDate.Value2) &&
                MyUtility.Check.Empty(dateCustRQSDate.Value1) && MyUtility.Check.Empty(dateCustRQSDate.Value2) &&
                MyUtility.Check.Empty(datePlanDate.Value1) && MyUtility.Check.Empty(datePlanDate.Value2) &&
                MyUtility.Check.Empty(dateOrderCfmDate.Value1) && MyUtility.Check.Empty(dateOrderCfmDate.Value2))
            {
                MyUtility.Msg.WarningBox("All date can't empty!!");
                dateBuyerDelivery.TextBox1.Focus();
                return false;
            }

            buyerDlv1 = dateBuyerDelivery.Value1;
            buyerDlv2 = dateBuyerDelivery.Value2;
            sciDlv1 = dateSCIDelivery.Value1;
            sciDlv2 = dateSCIDelivery.Value2;
            cutoff1 = dateCutOffDate.Value1;
            cutoff2 = dateCutOffDate.Value2;
            custRQS1 = dateCustRQSDate.Value1;
            custRQS2 = dateCustRQSDate.Value2;
            planDate1 = datePlanDate.Value1;
            planDate2 = datePlanDate.Value2;
            orderCfm1 = dateOrderCfmDate.Value1;
            orderCfm2 = dateOrderCfmDate.Value2;
            style = txtstyle.Text.Trim();
            season = txtseason.Text.Trim();
            brand = txtbrand.Text.Trim();
            custcd = txtcustcd.Text.Trim();

            zone = MyUtility.Convert.GetString(comboZone.SelectedValue);
            mDivision = comboM.Text;
            factory = comboFactory.Text;
            bulk = checkBulk.Checked;
            sample = checkSample.Checked;
            material = checkMaterial.Checked;
            forecast = checkForecast.Checked;
            subProcess = comboSubProcess.Text;
            hisOrder = checkIncludeHistoryOrder.Checked;
            artwork = checkIncludeArtworkdata.Checked;
            pap = checkIncludeArtworkdataKindIsPAP.Checked;
            seperate = checkQtyBDownByShipmode.Checked;
            poCombo = checkListPOCombo.Checked;

            return base.ValidateInput();
        }


        private StringBuilder select_cmd(string p_type)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string seperCmd = "";
            #region 組SQL
            if (seperate && p_type.Equals("ALL"))
            {
                seperCmd = " ,oq.Seq";
            }
            sqlCmd.Append(@"
with tmpOrders as (
    select  o.ID
            , o.MDivisionID
            , o.FtyGroup
            , o.FactoryID
            , o.BuyerDelivery
            , o.SciDelivery
            , o.POID
            , o.CRDDate
            , o.CFMDate
            , o.Dest
            , o.StyleID
            , o.SeasonID
            , o.BrandID
            , o.ProjectID
            , o.Customize1
            , o.BuyMonth
            , o.CustPONo
            , o.CustCDID
            , o.ProgramID
            , o.CdCodeID
            , o.CPU
            , o.Qty
            , o.FOCQty
            , o.LocalOrder
            , o.PoPrice
            , o.CMPPrice
            , o.KPILETA
            , o.LETA
            , o.MTLETA
            , o.SewETA
            , o.PackETA
            , o.MTLComplete
            , o.SewInLine
            , o.SewOffLine
            , o.CutInLine
            , o.CutOffLine
            , o.Category
            , o.PulloutDate
            , o.ActPulloutDate
            , o.SMR
            , o.MRHandle
            , o.MCHandle
            , o.OrigBuyerDelivery
            , o.DoxType
            , o.TotalCTN
            , o.FtyCTN
            , o.ClogCTN
            , o.VasShas
            , o.TissuePaper
            , o.MTLExport
            , o.SewLine
            , o.ShipModeList
            , o.PlanDate
            , o.FirstProduction
            , o.OrderTypeID
            , o.SpecialMark
            , o.SampleReason
            , o.InspDate
            , InspResult = IIF(o.InspResult='P','Pass',IIF(o.InspResult='F','Fail',''))
            , InspHandle = (o.InspHandle +'-'+ I.Name)
            , o.MnorderApv
            , o.PulloutComplete
            , o.FtyKPI
            , o.KPIChangeReason
            , o.EachConsApv
            , o.Junk
            , o.StyleUkey
            , o.CuttingSP
            , o.RainwearTestPassed
            , o.BrandFTYCode
            , o.CPUFactor
            , o.ClogLastReceiveDate
            , o.IsMixMarker
            , o.GFR "
            + seperCmd +
    @" from Orders o WITH (NOLOCK) 
    left join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
    OUTER APPLY(
        SELECT  Name 
        FROM Pass1 WITH (NOLOCK) 
        WHERE Pass1.ID = O.InspHandle
    )I
    where o.junk=0 ");
            if (!MyUtility.Check.Empty(buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDlv1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDlv2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDlv1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDlv2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(cutoff1))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate >= '{0}'", Convert.ToDateTime(cutoff1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(cutoff2))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate <= '{0}'", Convert.ToDateTime(cutoff2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(custRQS1))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate >= '{0}'", Convert.ToDateTime(custRQS1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(custRQS2))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate <= '{0}'", Convert.ToDateTime(custRQS2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate1))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate >= '{0}'", Convert.ToDateTime(planDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate2))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate <= '{0}'", Convert.ToDateTime(planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(orderCfm1))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate >= '{0}'", Convert.ToDateTime(orderCfm1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(orderCfm2))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate <= '{0}'", Convert.ToDateTime(orderCfm2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", style));
            }
            if (!MyUtility.Check.Empty(season))
            {
                sqlCmd.Append(string.Format(" and o.SeasonID = '{0}'", season));
            }
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }
            if (!MyUtility.Check.Empty(custcd))
            {
                sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", custcd));
            }
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
            }
            if (!hisOrder)
            {
                sqlCmd.Append(" and o.Finished = 0");
            }
            if ((bulk || sample || material || forecast) && p_type.Equals("ALL"))
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
                //如果沒勾seperate但有勾forecast的情況，不用將forecast資料另外收
                if (forecast && !seperate)
                {
                    sqlCmd.Append(" or o.Category = ''");
                }
                sqlCmd.Append(")");
            }

            //forcast 另外出在excel的最下方，因為會與Separate條件衝突，所以另外處理
            if (forecast && p_type.Equals("forecast"))
            {
                sqlCmd.Append(" and o.Category = ''");
            }

            sqlCmd.Append(@"
),
tmpFilterZone as (
    select t.* 
    from tmpOrders t");
            if (!MyUtility.Check.Empty(zone))
            {
                sqlCmd.Append(string.Format(@"
    inner join Factory f WITH (NOLOCK) on t.FactoryID = f.ID
    where f.Zone = '{0}'", zone));
            }

            sqlCmd.Append(@"
), tmpFilterSubProcess as (
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
                if (seperate && p_type.Equals("ALL"))
                {
                    seperCmd = " , '' seq ";
                }
                sqlCmd.Append(@"
), tmpListPoCombo as (
    select * 
    from tmpFilterSubProcess

    union
    select  o.ID
            , o.MDivisionID
            , o.FtyGroup
            , o.FactoryID
            , o.BuyerDelivery
            , o.SciDelivery
            , O.POID
            , o.CRDDate
            , o.CFMDate
            , o.Dest
            , o.StyleID
            , o.SeasonID
            , o.BrandID
            , o.ProjectID
            , o.Customize1
            , o.BuyMonth
            , o.CustPONo
            , o.CustCDID
            , o.ProgramID
            , o.CdCodeID
            , o.CPU
            , o.Qty
            , o.FOCQty
            , o.LocalOrder
            , o.PoPrice
            , o.CMPPrice
            , o.KPILETA
            , o.LETA
            , o.MTLETA
            , o.SewETA
            , o.PackETA
            , o.MTLComplete
            , o.SewInLine
            , o.SewOffLine
            , o.CutInLine
            , o.CutOffLine
            , o.Category
            , o.PulloutDate
            , o.ActPulloutDate
            , o.SMR
            , o.MRHandle
            , o.MCHandle
            , o.OrigBuyerDelivery
            , o.DoxType
            , o.TotalCTN
            , o.FtyCTN
            , o.ClogCTN
            , o.VasShas
            , o.TissuePaper
            , o.MTLExport
            , o.SewLine
            , o.ShipModeList
            , o.PlanDate
            , o.FirstProduction
            , o.OrderTypeID
            , o.SpecialMark
            , o.SampleReason
            , o.InspDate
            , InspResult = IIF(o.InspResult='P','Pass',IIF(o.InspResult='F','Fail',''))
            , InspHandle = (o.InspHandle +'-'+ I.Name)
            , o.MnorderApv
            , o.PulloutComplete
            , o.FtyKPI
            , o.KPIChangeReason
            , o.EachConsApv
            , o.Junk
            , o.StyleUkey
            , o.CuttingSP
            , o.RainwearTestPassed
            , o.BrandFTYCode
            , o.CPUFactor
            , o.ClogLastReceiveDate
            , o.IsMixMarker
            , o.GFR "
            + seperCmd +
    @"from Orders o  WITH (NOLOCK) 
    OUTER APPLY (
        SELECT Name 
        FROM Pass1 WITH (NOLOCK) 
        WHERE Pass1.ID=O.InspHandle
    )I
    where POID IN (select distinct POID from tmpFilterSubProcess)
)");
            }
            else
            {
                sqlCmd.Append(@"
), tmpListPoCombo as (
    select * 
    from tmpFilterSubProcess
)");
            }
            if (seperate && p_type.Equals("ALL"))
            {
                sqlCmd.Append(@"
, tmpFilterSeperate as (
    select  t.ID
            , t.MDivisionID
            , t.FtyGroup
            , t.FactoryID
            , oq.BuyerDelivery
            , t.SciDelivery
            , t.POID
            , t.CRDDate
            , t.CFMDate
            , t.Dest
            , t.StyleID
            , t.SeasonID
            , t.BrandID
            , t.ProjectID
            , t.Customize1
            , t.BuyMonth
            , t.CustPONo
            , t.CustCDID
            , t.ProgramID
            , t.CdCodeID
            , t.CPU
            , oq.Qty
            , t.FOCQty
            , t.LocalOrder
            , t.PoPrice
            , t.CMPPrice
            , t.KPILETA
            , t.LETA
            , t.MTLETA
            , t.SewETA
            , t.PackETA
            , t.MTLComplete
            , t.SewInLine
            , t.SewOffLine
            , t.CutInLine
            , t.CutOffLine
            , t.Category
            , PulloutDate = oq.EstPulloutDate
            , ActPulloutDate = (select MAX(p.PulloutDate) 
                                from PackingList p WITH (NOLOCK) 
                                inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
                                where pd.OrderID = t.ID and pd.OrderShipmodeSeq = oq.Seq)
            , t.SMR
            , t.MRHandle
            , t.MCHandle
            , t.OrigBuyerDelivery
            , t.DoxType
            , TotalCTN = isnull ((select SUM (pd.CTNQty) 
                                  from PackingList_Detail pd WITH (NOLOCK) 
                                  LEFT JOIN PackingList p on pd.ID=p.ID 
                                  where     pd.OrderID = t.ID 
                                            and pd.OrderShipmodeSeq = oq.Seq 
                                            and p.Type in ('B','L'))
                                , 0)
            , FtyCTN = isnull ((select SUM(pd.CTNQty) 
                                from PackingList_Detail pd WITH (NOLOCK) 
                                LEFT JOIN PackingList p on pd.ID = p.ID 
                                where   pd.OrderID = t.ID 
                                        and pd.OrderShipmodeSeq = oq.Seq 
                                        and pd.TransferDate is null 
                                        and p.Type in ('B','L'))
                              , 0)
            , ClogCTN = isnull ((select SUM(pd.CTNQty) 
                                 from PackingList_Detail pd WITH (NOLOCK) 
                                 LEFT JOIN PackingList p on pd.ID=p.ID 
                                 where  pd.OrderID = t.ID 
                                        and pd.OrderShipmodeSeq = oq.Seq 
                                        and pd.ReceiveDate is not null 
                                        and p.Type in ('B','L'))
                                , 0)
            , t.VasShas
            , t.TissuePaper
            , t.MTLExport
            , t.SewLine
            , oq.ShipmodeID as ShipModeList
            , t.PlanDate
            , t.FirstProduction
            , t.OrderTypeID
            , t.SpecialMark
            , t.SampleReason
            , t.InspDate
            , t.InspResult
            , t.InspHandle
            , t.MnorderApv
            , t.PulloutComplete
            , t.FtyKPI
            , t.KPIChangeReason
            , t.EachConsApv
            , t.Junk
            , t.StyleUkey
            , t.CuttingSP
            , t.RainwearTestPassed
            , t.BrandFTYCode
            , t.CPUFactor
            , oq.Seq
            , t.ClogLastReceiveDate
            , t.IsMixMarker
            , t.GFR
    from tmpListPoCombo t
    inner join Order_QtyShip oq WITH (NOLOCK) on t.ID = oq.Id and t.Seq=oq.Seq
)
select  t.* 
        , ModularParent = isnull (s.ModularParent, '')
        , CPUAdjusted = isnull (s.CPUAdjusted * 100, 0)
        , DestAlias = isnull (c.Alias, '')
        , ExpectionForm = isnull (s.ExpectionForm, '')
        , ExpectionFormRemark = isnull (s.ExpectionFormRemark, '')
        , WorkType = isnull (ct.WorkType, '')
        , ct.FirstCutDate
        , POSMR = isnull (p.POSMR, '')
        , POHandle = isnull (p.POHandle, '') 
        , FTYRemark = isnull (s.FTYRemark, '')
        , SewQtyTop = isnull ((select SUM(QAQty) 
                               from SewingOutput_Detail WITH (NOLOCK) 
                               where    OrderId = t.ID 
                                        and ComboType = 'T')
                              , 0) 
        , SewQtyBottom = isnull ((select SUM(QAQty) 
                                  from SewingOutput_Detail WITH (NOLOCK) 
                                  where OrderId = t.ID 
                                        and ComboType = 'B')
                                , 0)
        , SewQtyInner = isnull ((select SUM(QAQty) 
                                 from SewingOutput_Detail WITH (NOLOCK) 
                                 where  OrderId = t.ID 
                                        and ComboType = 'I')
                                , 0)
        , SewQtyOuter = isnull ((select SUM(QAQty) 
                                 from SewingOutput_Detail WITH (NOLOCK) 
                                 where  OrderId = t.ID 
                                        and ComboType = 'O')
                                , 0)
        , TtlSewQty = isnull (dbo.getMinCompleteSewQty (t.ID, null, null) ,0)
        , CutQty = isnull ((select SUM(Qty) 
                            from CuttingOutput_WIP WITH (NOLOCK) 
                            where OrderID = t.ID)
                          , 0)
        , PFRemark = isnull ((select top 1 Remark 
                              from Order_PFHis WITH (NOLOCK) 
                              where ID = t.ID 
                                    and AddDate = (select Max(AddDate) 
                                                   from Order_PFHis WITH (NOLOCK) 
                                                   where ID = t.ID))
                            , '')
        , EarliestSCIDlv =dbo.getMinSCIDelivery(t.POID,'')
        , KPIChangeReasonName = isnull ((select Name 
                                         from Reason WITH (NOLOCK) 
                                         where  ReasonTypeID = 'Order_BuyerDelivery' 
                                                and ID = t.KPIChangeReason)
                                       , '')
        , SMRName = isnull ((select Name 
                             from TPEPass1 WITH (NOLOCK) 
                             where Id = t.SMR)
                           , '')
        , MRHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK) 
                                  where Id = t.MRHandle)
                                , '')
        , POSMRName = isnull ((select Name 
                               from TPEPass1 WITH (NOLOCK) 
                               where Id = p.POSMR)
                             , '')
        , POHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK)
                                  where Id = p.POHandle)
                                , '')
        , MCHandleName = isnull ((select Name 
                                  from Pass1 WITH (NOLOCK) 
                                  where Id = t.MCHandle)
                                , '')
        , SampleReasonName = isnull ((select Name 
                                      from Reason WITH (NOLOCK) 
                                      where ReasonTypeID = 'Order_reMakeSample' 
                                            and ID = t.SampleReason)
                                    , '')
        , SpecialMarkName = isnull ((select Name 
                                     from Reason WITH (NOLOCK) 
                                     where ReasonTypeID = 'Style_SpecialMark' 
                                           and ID = t.SpecialMark)
                                    , '') 
        , MTLExportTimes = isnull ([dbo].getMTLExport (t.POID, t.MTLExport), '')
        , GMTLT = dbo.GetStyleGMTLT (t.BrandID, t.StyleID, t.SeasonID, t.FactoryID)
        , SimilarStyle = dbo.GetSimilarStyleList (t.StyleUkey)
        , MTLDelay = dbo.GetHaveDelaySupp (t.POID)
        , PackingQty = isnull ((select SUM(pld.ShipQty) 
                                from PackingList pl WITH (NOLOCK) 
                                inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
                                where   pl.Type <> 'F' 
                                        and pld.OrderID = t.ID 
                                        and pld.OrderShipmodeSeq = t.Seq)
                              , 0)
        , PackingFOCQty = isnull ((select SUM(pld.ShipQty) 
                                   from PackingList pl WITH (NOLOCK) 
                                   inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID 
                                   where    pl.Type = 'F' 
                                            and pld.OrderID = t.ID 
                                            and pld.OrderShipmodeSeq = t.Seq)
                                 , 0)
        , BookingQty = isnull ((select SUM(pld.ShipQty) 
                                from PackingList pl WITH (NOLOCK) 
                                inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
                                where   (pl.Type = 'B' or pl.Type = 'S') 
                                        and pl.INVNo <> '' 
                                        and pld.OrderID = t.ID 
                                        and pld.OrderShipmodeSeq = t.Seq)
                               , 0)
        , InvoiceAdjQty = dbo.getInvAdjQty (t.ID, t.Seq) 
        , ct.LastCutDate
        , ArriveWHDate = (select Max(e.WhseArrival) 
                          from Export e WITH (NOLOCK) 
                          inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
                          where ed.POID = t.POID)
        , FirstOutDate = (select Min (so.OutputDate) 
                          from SewingOutput so WITH (NOLOCK) 
                          inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                          where sod.OrderID = t.ID) 
        , LastOutDate = (select Max (so.OutputDate) 
                         from SewingOutput so WITH (NOLOCK) 
                         inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                         where sod.OrderID = t.ID)
        , PulloutQty = isnull ((select Sum (pod.ShipQty) 
                                from Pullout_Detail pod WITH (NOLOCK) 
                                where pod.OrderID = t.ID 
                                      and pod.OrderShipmodeSeq = t.Seq)
                              , 0)
        , ActPulloutTime = (select Count (Distinct ID) 
                            from Pullout_Detail WITH (NOLOCK) 
                            where   OrderID = t.ID 
                                    and ShipQty > 0) 
        , PackingCTN = isnull ((select Sum(CTNQty) 
                                from PackingList_Detail WITH (NOLOCK) 
                                where   OrderID = t.ID 
                                        and OrderShipmodeSeq = t.Seq)
                              , 0)
        , TotalCTN = isnull ((select Sum(pd.CTNQty) 
                              from PackingList_Detail pd WITH (NOLOCK) 
                              LEFT JOIN PackingList p on pd.ID = p.ID 
                              where pd.OrderID = t.ID 
                                    and pd.OrderShipmodeSeq = t.Seq 
                                    and p.Type in ('B', 'L'))
                            , 0)
        , FtyCtn = isnull ((select Sum(pd.CTNQty) 
                            from PackingList_Detail pd WITH (NOLOCK) 
                            LEFT JOIN PackingList p on pd.ID=p.ID 
                            where   pd.OrderID = t.ID 
                                    and pd.OrderShipmodeSeq = t.Seq 
                                    and p.Type in ('B','L') 
                                    and pd.TransferDate is null)
                          , 0)
        , ClogCTN = isnull ((select Sum(pd.CTNQty) 
                             from PackingList_Detail pd WITH (NOLOCK) 
                             LEFT JOIN PackingList p on pd.ID = p.ID 
                             where  pd.OrderID = t.ID 
                                    and pd.OrderShipmodeSeq = t.Seq 
                                    and p.Type in ('B', 'L') 
                                    and pd.ReceiveDate is not null)
                           , 0) 
        , ClogRcvDate = (Select Max (ReceiveDate) 
                         from PackingList_Detail WITH (NOLOCK) 
                         where  OrderID = t.ID 
                                and OrderShipmodeSeq = t.Seq)
        , Article = isnull ((select CONCAT(Article,',') 
                             from (select distinct Article 
                                   from Order_QtyShip_Detail WITH (NOLOCK) 
                                   where ID = t.ID and Seq = t.Seq
                             ) a for xml path(''))
                           , '')
, [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail where id=p.ID  and FabricType='F')
, [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail where id=p.ID  and FabricType='A')
from tmpFilterSeperate t
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join Country c WITH (NOLOCK) on c.ID = t.Dest
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join PO p WITH (NOLOCK) on p.ID = t.POID
order by t.ID");
            }
            else
            {
                sqlCmd.Append(@"
select distinct t.*
        , ModularParent = isnull (s.ModularParent, '')  
        , CPUAdjusted = isnull(s.CPUAdjusted * 100, 0)  
        , DestAlias = isnull (c.Alias, '') 
        , ExpectionForm = isnull (s.ExpectionForm, '')  
        , ExpectionFormRemark = isnull (s.ExpectionFormRemark, '')  
        , WorkType = isnull(ct.WorkType,'')
        , ct.FirstCutDate
        , POSMR = isnull (p.POSMR, '')
        , POHandle = isnull (p.POHandle, '') 
        , FTYRemark = isnull (s.FTYRemark, '')
        , SewQtyTop = isnull ((select SUM(QAQty) 
                               from SewingOutput_Detail WITH (NOLOCK) 
                               where OrderId = t.ID 
                                     and ComboType = 'T')
                             , 0)
        , SewQtyBottom = isnull ((select SUM(QAQty) 
                                  from SewingOutput_Detail WITH (NOLOCK) 
                                  where OrderId = t.ID 
                                        and ComboType = 'B')
                                , 0)
        , SewQtyInner = isnull ((select SUM(QAQty) 
                                 from SewingOutput_Detail WITH (NOLOCK) 
                                 where OrderId = t.ID 
                                       and ComboType = 'I')
                               , 0) 
        , SewQtyOuter = isnull ((select SUM(QAQty) 
                                 from SewingOutput_Detail WITH (NOLOCK) 
                                 where OrderId = t.ID 
                                       and ComboType = 'O')
                               , 0)
        , TtlSewQty = isnull (dbo.getMinCompleteSewQty (t.ID, null, null), 0)
        , CutQty = isnull ((select SUM(Qty) 
                            from CuttingOutput_WIP WITH (NOLOCK) 
                            where OrderID = t.ID)
                          , 0)
        , PFRemark = isnull ((select top 1 Remark 
                              from Order_PFHis WITH (NOLOCK) 
                              where ID = t.ID 
                                    and AddDate = (select Max(AddDate) 
                                                   from Order_PFHis WITH (NOLOCK) 
                                                   where ID = t.ID))
                            , '') 
        , EarliestSCIDlv = dbo.getMinSCIDelivery (t.POID, '')
        , KPIChangeReasonName = isnull ((select Name 
                                         from Reason WITH (NOLOCK)  
                                         where  ReasonTypeID = 'Order_BuyerDelivery' 
                                                and ID = t.KPIChangeReason)
                                        , '')
        , SMRName = isnull ((select Name 
                             from TPEPass1 WITH (NOLOCK) 
                             where Id = t.SMR)
                            , '')
        , MRHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK) 
                                  where Id = t.MRHandle)
                                , '')
        , POSMRName = isnull ((select Name 
                               from TPEPass1 WITH (NOLOCK) 
                               where Id = p.POSMR)
                             , '')
        , POHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK) 
                                  where Id = p.POHandle)
                                , '')
        , MCHandleName = isnull ((select Name 
                                  from Pass1 WITH (NOLOCK) 
                                  where Id = t.MCHandle)
                                , '')
        , SampleReasonName = isnull ((select Name 
                                      from Reason WITH (NOLOCK) 
                                      where ReasonTypeID = 'Order_reMakeSample' 
                                            and ID = t.SampleReason)
                                    , '') 
        , SpecialMarkName = isnull ((select Name 
                                     from Reason WITH (NOLOCK) 
                                     where  ReasonTypeID = 'Style_SpecialMark' 
                                            and ID = t.SpecialMark)
                                   , '')
        , MTLExportTimes = isnull ([dbo].getMTLExport (t.POID, t.MTLExport), '')
        , GMTLT = dbo.GetStyleGMTLT (t.BrandID, t.StyleID, t.SeasonID, t.FactoryID)
        , SimilarStyle = dbo.GetSimilarStyleList (t.StyleUkey)
        , MTLDelay = dbo.GetHaveDelaySupp (t.POID)
        , PackingQty = isnull ((select SUM(pld.ShipQty) 
                                from PackingList pl WITH (NOLOCK) 
                                inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
                                where  pl.Type <> 'F' 
                                       and pld.OrderID = t.ID)
                              , 0)
        , PackingFOCQty = isnull ((select SUM(pld.ShipQty) 
                                   from PackingList pl WITH (NOLOCK) 
                                   inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
                                   where pl.Type = 'F' 
                                         and pld.OrderID = t.ID)
                                 , 0)
        , BookingQty = isnull ((select SUM(pld.ShipQty) 
                                from PackingList pl WITH (NOLOCK) 
                                inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
                                where   (pl.Type = 'B' or pl.Type = 'S') 
                                        and pl.INVNo <> '' 
                                        and pld.OrderID = t.ID)
                              , 0)
        , InvoiceAdjQty = isnull ((select sum(iq.DiffQty) 
                                   from InvAdjust i WITH (NOLOCK) 
                                   inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID = iq.ID
                                   where i.OrderID = t.ID)
                                 , 0)
        , ct.LastCutDate
        , ArriveWHDate = (select Max(e.WhseArrival) 
                          from Export e WITH (NOLOCK) 
                          inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID 
                          where ed.POID = t.POID) 
        , FirstOutDate = (select Min(so.OutputDate) 
                          from SewingOutput so WITH (NOLOCK) 
                          inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                          where sod.OrderID = t.ID) 
        , LastOutDate = (select Max(so.OutputDate) 
                         from SewingOutput so WITH (NOLOCK) 
                         inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                         where sod.OrderID = t.ID)
        , PulloutQty = isnull ((select Sum(pod.ShipQty) 
                                from Pullout_Detail pod WITH (NOLOCK) 
                                where pod.OrderID = t.ID)
                              , 0)
        , ActPulloutTime = (select Count(Distinct ID) 
                            from Pullout_Detail WITH (NOLOCK) 
                            where   OrderID=t.ID 
                                    and ShipQty > 0)
        , PackingCTN = isnull ((select Sum(CTNQty) 
                                from PackingList_Detail WITH (NOLOCK) 
                                where OrderID = t.ID)
                              , 0) 
        , t.TotalCTN
        , FtyCtn = isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0)
        , ClogCTN = isnull(t.ClogCTN,0)
        , ClogRcvDate = t.ClogLastReceiveDate
        , Article = isnull ((select CONCAT(Article, ',') 
                             from (select distinct Article 
                                   from Order_Qty WITH (NOLOCK) 
                                   where ID = t.ID
                             ) a for xml path(''))
                           , '') 
, [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail where id=p.ID  and FabricType='F')
, [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail where id=p.ID  and FabricType='A')
from tmpListPoCombo t
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join Country c WITH (NOLOCK) on c.ID = t.Dest
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join PO p WITH (NOLOCK) on p.ID = t.POID
order by t.ID");
            }
            #endregion

            return sqlCmd;

     }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd;
            //抓取一般條件資料
            sqlCmd = select_cmd("ALL");
            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            //抓取forecast資料再merge回主datatable，只有forecast和seperate有勾的時候才做
            if (forecast && seperate)
            {
                DataTable printData_forecast;
                StringBuilder sqlCmd_forecast = select_cmd("forecast");
                result = DBProxy.Current.Select(null, sqlCmd_forecast.ToString(), out printData_forecast);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
                printData.Merge(printData_forecast);
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
                    sqlCmd.Append(string.Format(@"
With SubProcess  as (
    select  *
            , rno = (ROW_NUMBER() OVER (ORDER BY a.ID, a.ColumnSeq)) 
    from (
        SELECT  ID
                , Seq
                , ArtworkUnit
                , ProductionUnit
                , SystemType
                , FakeID = Seq + 'U1'
                , ColumnN = RTRIM(ID) + ' ('+ArtworkUnit+')'
                , ColumnSeq = '1'
        FROM ArtworkType WITH (NOLOCK) 
        WHERE   ArtworkUnit <> '' 
                and Classify in ({0}) 
                and Junk = 0
        
        union all
        SELECT  ID
                , Seq
                , ArtworkUnit
                , ProductionUnit
                , SystemType
                , FakeID = Seq + 'U2'
                , ColumnN = RTRIM(ID) + ' ('+IIF(ProductionUnit = 'QTY','Price',ProductionUnit)+')'
                , ColumnSeq = '2' 
        FROM ArtworkType WITH (NOLOCK) 
        WHERE   ProductionUnit <> '' 
                and Classify in ({0}) 
                and Junk = 0
        
        union all
        SELECT  ID
                , Seq
                , ArtworkUnit
                , ProductionUnit
                , SystemType
                , FakeID = Seq + 'N'
                , ColumnN = RTRIM(ID)
                , ColumnSeq = '3'
        FROM ArtworkType WITH (NOLOCK) 
        WHERE   ArtworkUnit = '' 
                and ProductionUnit = '' 
                and Classify in ({0}) 
                and Junk = 0
        {1}
    ) a
), TTL_Subprocess as (
    select  ID = 'TTL' + ID 
            , Seq
            , ArtworkUnit
            , ProductionUnit
            , SystemType
            , FakeID = 'T' + FakeID
            , ColumnN = 'TTL_' + ColumnN
            , ColumnSeq
            , rno = (ROW_NUMBER() OVER (ORDER BY ID, ColumnSeq)) + 1000
            from SubProcess 
            where ID <> 'PrintSubCon'
)
select  ID
        , Seq
        , ArtworkUnit
        , ProductionUnit
        , SystemType
        , FakeID
        , ColumnN
        , ColumnSeq
        , rno = (ROW_NUMBER() OVER (ORDER BY a.rno)) + {2}
from (
    select * 
    from SubProcess

    union all
    SELECT  ID = 'TTLTMS'
            , Seq = ''
            , ArtworkUnit = '' 
            , ProductionUnit = '' 
            , SystemType = '' 
            , FakeID = 'TTLTMS'
            , FakeID = 'TTL_TMS'
            , ColumnSeq = '' 
            , rno = '999'
    union
    select * 
    from TTL_Subprocess
) a"
                        , classify, (!artwork ? "" : @"
    union all
    SELECT  ID = 'PrintSubCon'
            , Seq = ''
            , ArtworkUnit = '' 
            , ProductionUnit = '' 
            , SystemType = ''
            , FakeID = '9999ZZ'
            , ColumnN = 'SubCon'
            , ColumnSeq = '999'"),
                                                                                                                                                            "115"));
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
                        StringBuilder sqlcmd_sub = new StringBuilder();
                        sqlcmd_sub.Append(@"
with ArtworkData as (
    select * 
    from #tmp
),
OrderID as(
    select ID from orders O  WITH (NOLOCK)  where  o.junk=0 "
);

                        if (!MyUtility.Check.Empty(buyerDlv1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDlv1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(buyerDlv2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDlv2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(sciDlv1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDlv1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(sciDlv2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDlv2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(cutoff1))
                        {
                            sqlcmd_sub.Append(string.Format(" and oq.SDPDate >= '{0}'", Convert.ToDateTime(cutoff1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(cutoff2))
                        {
                            sqlcmd_sub.Append(string.Format(" and oq.SDPDate <= '{0}'", Convert.ToDateTime(cutoff2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(custRQS1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CRDDate >= '{0}'", Convert.ToDateTime(custRQS1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(custRQS2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CRDDate <= '{0}'", Convert.ToDateTime(custRQS2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(planDate1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.PlanDate >= '{0}'", Convert.ToDateTime(planDate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(planDate2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.PlanDate <= '{0}'", Convert.ToDateTime(planDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(orderCfm1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CFMDate >= '{0}'", Convert.ToDateTime(orderCfm1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(orderCfm2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CFMDate <= '{0}'", Convert.ToDateTime(orderCfm2).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(style))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.StyleID = '{0}'", style));
                        }
                        if (!MyUtility.Check.Empty(season))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SeasonID = '{0}'", season));
                        }
                        if (!MyUtility.Check.Empty(brand))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BrandID = '{0}'", brand));
                        }
                        if (!MyUtility.Check.Empty(custcd))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CustCDID = '{0}'", custcd));
                        }
                        if (!MyUtility.Check.Empty(mDivision))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));
                        }
                        if (!MyUtility.Check.Empty(factory))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
                        }
                        if (!hisOrder)
                        {
                            sqlcmd_sub.Append(" and o.Finished = 0");
                        }
                        if ((bulk || sample || material || forecast))
                        {
                            sqlcmd_sub.Append(" and (1=0");
                            if (bulk)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'B'");
                            }
                            if (sample)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'S'");
                            }
                            if (material)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'M'");
                            }
                            if (forecast)
                            {
                                sqlcmd_sub.Append(" or o.Category = ''");
                            }
                            sqlcmd_sub.Append(")");
                        }

                        sqlcmd_sub.Append(@" )
select  ot.ID
        , ot.ArtworkTypeID
        , ot.ArtworkUnit
        , at.ProductionUnit
        , isnull(ot.Qty,0) Qty 
        , ot.TMS
        , isnull(ot.Price,0) Price
        , Supp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', (select Abb 
                                                                              from LocalSupp WITH (NOLOCK) 
                                                                              where ID = LocalSuppID)
                                                                           , ot.LocalSuppID)
                                                  , '')
        , AUnitRno = a.rno 
        , PUnitRno = a1.rno
        , NRno = a2.rno
        , TAUnitRno = a3.rno
        , TPUnitRno = a4.rno 
        , TNRno = a5.rno  
from Order_TmsCost ot WITH (NOLOCK) 
left join ArtworkType at WITH (NOLOCK) on at.ID = ot.ArtworkTypeID
left join ArtworkData a on a.FakeID = ot.Seq+'U1' 
left join ArtworkData a1 on a1.FakeID = ot.Seq+'U2'
left join ArtworkData a2 on a2.FakeID = ot.Seq
left join ArtworkData a3 on a3.FakeID = 'T'+ot.Seq+'U1' 
left join ArtworkData a4 on a4.FakeID = 'T'+ot.Seq+'U2'
left join ArtworkData a5 on a5.FakeID = 'T'+ot.Seq where exists (select id from OrderID where ot.ID = OrderID.ID )");
                        MyUtility.Tool.ProcessWithDatatable(subprocessColumnName,
                            "ID,Seq,ArtworkUnit,ProductionUnit,SystemType,FakeID,ColumnN,ColumnSeq,rno", sqlcmd_sub.ToString()
                            , out orderArtworkData);

       

                    }
                    catch (Exception ex)
                    {
                        DualResult failResult = new DualResult(false, "Query order tms & cost fail\r\n" + ex.ToString());
                    }
                    #endregion
                }
            }
            DBProxy.Current.DefaultTimeout = 0;
            stdTMS = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select StdTMS from System WITH (NOLOCK) "));
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
          

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            this.ShowWaitMessage("Excel Processing...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R03_PPICMasterList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Name = "PPIC_Master_List";

            //填Subprocess欄位名稱
            int lastCol = 115;
            int subConCol = 9999, ttlTMS = 115; //紀錄SubCon與TTL_TMS的欄位
            if (artwork || pap)
            {
                foreach (DataRow dr in subprocessColumnName.Rows)
                {
                    worksheet.Cells[1, MyUtility.Convert.GetInt(dr["rno"])] = MyUtility.Convert.GetString(dr["ColumnN"]);
                    lastCol = MyUtility.Convert.GetInt(dr["rno"]);
                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "SUBCON")
                    {
                        subConCol = MyUtility.Convert.GetInt(dr["rno"]);
                         subtrue = 1;
                    }
                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "TTL_TMS")
                    {
                        ttlTMS = MyUtility.Convert.GetInt(dr["rno"]);
                    }
                }
            }
            else
            {
                worksheet.Cells[1, 114] = "TTL_TMS";
            }

            //算出Excel的Column的英文位置
            string excelColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(lastCol);

            //填內容值
            int intRowsStart = 0;
            object[,] objArray = new object[printData.Rows.Count, lastCol];
           
            string KPIChangeReasonName;  //CLOUMN[CC]:dr["KPIChangeReason"]+dr["KPIChangeReasonName"]
                                         //Dictionary<string, DataRow> tmp_a = orderArtworkData.AsEnumerable().ToDictionary<DataRow, string, DataRow>(r => r["ID"].ToString(),r => r);

            if (orderArtworkData == null) {
                orderArtworkData = new DataTable();
                orderArtworkData.ColumnsStringAdd("ID");
            }
            var lookupID = orderArtworkData.AsEnumerable().ToLookup(row => row["ID"].ToString());
            
            


            excel.Cells.EntireColumn.AutoFit(); //所有列最適列高
            foreach (DataRow dr in printData.Rows)
            {
                //EMBROIDERY 如果Qty price都是0該筆資料不show
                if ( orderArtworkData.Rows.Count > 0 && !MyUtility.Check.Empty( subProcess)) {
                    //DataRow[] find_subprocess = orderArtworkData.Select(string.Format("ID = '{0}' and ArtworkTypeID = '{1}' and (Price > 0 or Qty > 0)", MyUtility.Convert.GetString(dr["ID"]), subProcess));
                    var records = from record in lookupID[MyUtility.Convert.GetString(dr["ID"])]
                                  where record.Field<string>("ArtworkTypeID") == subProcess
                                           && (record.Field<decimal>("Price") > 0  || record.Field<decimal>("Qty") > 0)
                                  select record;
                    if (records.Count() == 0 )
                    {
                        continue;
                    }
                    records = null;
                }
               

                #region 填固定欄位資料
                objArray[intRowsStart, 0] = dr["MDivisionID"];
                objArray[intRowsStart, 1] = dr["FactoryID"];
                objArray[intRowsStart, 2] = dr["BuyerDelivery"];
                objArray[intRowsStart, 3] = MyUtility.Check.Empty(dr["BuyerDelivery"]) ? "" : Convert.ToDateTime(dr["BuyerDelivery"]).ToString("yyyyMM");
                objArray[intRowsStart, 4] = dr["EarliestSCIDlv"];
                objArray[intRowsStart, 5] = dr["SciDelivery"];
                objArray[intRowsStart, 6] = dr["CRDDate"];
                objArray[intRowsStart, 7] = MyUtility.Check.Empty(dr["CRDDate"]) ? "" : Convert.ToDateTime(dr["CRDDate"]).ToString("yyyyMM");
                objArray[intRowsStart, 8] = MyUtility.Convert.GetDate(dr["BuyerDelivery"]) != MyUtility.Convert.GetDate(dr["CRDDate"]) ? "Y" : "";
                objArray[intRowsStart, 9] = dr["CFMDate"];
                objArray[intRowsStart, 10] = MyUtility.Check.Empty(dr["CRDDate"]) || MyUtility.Check.Empty(dr["CFMDate"]) ? 0 : Convert.ToInt32((Convert.ToDateTime(dr["CRDDate"]) - Convert.ToDateTime(dr["CFMDate"])).TotalDays);
                objArray[intRowsStart, 11] = dr["ID"];
                objArray[intRowsStart, 12] = dr["Category"];
                objArray[intRowsStart, 13] = MyUtility.Convert.GetString(dr["Junk"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 14] = dr["DestAlias"];
                objArray[intRowsStart, 15] = dr["StyleID"];
                objArray[intRowsStart, 16] = dr["ModularParent"];
                objArray[intRowsStart, 17] = dr["CPUAdjusted"];
                objArray[intRowsStart, 18] = dr["SimilarStyle"];
                objArray[intRowsStart, 19] = dr["SeasonID"];
                objArray[intRowsStart, 20] = dr["GMTLT"];
                objArray[intRowsStart, 21] = dr["OrderTypeID"];
                objArray[intRowsStart, 22] = dr["ProjectID"];
                objArray[intRowsStart, 23] = dr["Customize1"];
                objArray[intRowsStart, 24] = dr["BuyMonth"];
                objArray[intRowsStart, 25] = dr["CustPONo"];
                objArray[intRowsStart, 26] = MyUtility.Convert.GetString(dr["VasShas"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 27] = dr["MnorderApv"];
                objArray[intRowsStart, 28] = MyUtility.Convert.GetString(dr["TissuePaper"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 29] = dr["ExpectionForm"];
                objArray[intRowsStart, 30] = dr["ExpectionFormRemark"];
                objArray[intRowsStart, 31] = MyUtility.Convert.GetString(dr["GFR"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 32] = dr["BrandID"];
                objArray[intRowsStart, 33] = dr["CustCDID"];
                objArray[intRowsStart, 34] = dr["BrandFTYCode"];
                objArray[intRowsStart, 35] = dr["ProgramID"];
                objArray[intRowsStart, 36] = dr["CdCodeID"];
                objArray[intRowsStart, 37] = dr["CPU"];
                objArray[intRowsStart, 38] = dr["Qty"];
                objArray[intRowsStart, 39] = dr["FOCQty"];
                objArray[intRowsStart, 40] = MyUtility.Convert.GetDecimal(dr["CPU"]) * MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPUFactor"]);
                objArray[intRowsStart, 41] = dr["SewQtyTop"];
                objArray[intRowsStart, 42] = dr["SewQtyBottom"];
                objArray[intRowsStart, 43] = dr["SewQtyInner"];
                objArray[intRowsStart, 44] = dr["SewQtyOuter"];
                objArray[intRowsStart, 45] = dr["TtlSewQty"];
                objArray[intRowsStart, 46] = dr["CutQty"];
                objArray[intRowsStart, 47] = MyUtility.Convert.GetString(dr["WorkType"]) == "1" ? "Y" : "";
                objArray[intRowsStart, 48] = MyUtility.Convert.GetDecimal(dr["CutQty"]) >= MyUtility.Convert.GetDecimal(dr["Qty"]) ? "Y" : "";
                objArray[intRowsStart, 49] = dr["PackingQty"];
                objArray[intRowsStart, 50] = dr["PackingFOCQty"];
                objArray[intRowsStart, 51] = dr["BookingQty"];
                objArray[intRowsStart, 52] = dr["InvoiceAdjQty"];
                objArray[intRowsStart, 53] = dr["PoPrice"];
                objArray[intRowsStart, 54] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["PoPrice"]);
                objArray[intRowsStart, 55] = MyUtility.Convert.GetString(dr["LocalOrder"]).ToUpper() == "TRUE" ? dr["PoPrice"] : dr["CMPPrice"];
                objArray[intRowsStart, 56] = dr["KPILETA"];  //BE
                objArray[intRowsStart, 57] = dr["PFRemark"];//BF
                objArray[intRowsStart, 58] = dr["LETA"];  //BG
                objArray[intRowsStart, 59] = dr["MTLETA"];  //BH
                objArray[intRowsStart, 60] = dr["Fab_ETA"];  //BI
                objArray[intRowsStart, 61] = dr["Acc_ETA"];  //BJ
                objArray[intRowsStart, 62] = dr["SewETA"];  //BK
                objArray[intRowsStart, 63] = dr["PackETA"];  //BL
                objArray[intRowsStart, 64] = MyUtility.Convert.GetString(dr["MTLDelay"]).ToUpper() == "TRUE" ? "Y" : ""; //BM
                objArray[intRowsStart, 65] = MyUtility.Check.Empty(dr["MTLExport"]) ? dr["MTLExportTimes"] : dr["MTLExport"];
                objArray[intRowsStart, 66] = MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper();   //MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 67] = dr["ArriveWHDate"];
                objArray[intRowsStart, 68] = dr["SewInLine"];
                objArray[intRowsStart, 69] = dr["SewOffLine"];
                objArray[intRowsStart, 70] = dr["FirstOutDate"];
                objArray[intRowsStart, 71] = dr["LastOutDate"];
                objArray[intRowsStart, 72] = dr["EachConsApv"];
                objArray[intRowsStart, 73] = dr["CutInLine"];
                objArray[intRowsStart, 74] = dr["CutOffLine"];
                objArray[intRowsStart, 75] = dr["FirstCutDate"];
                objArray[intRowsStart, 76] = dr["LastCutDate"];
                objArray[intRowsStart, 77] = dr["PulloutDate"];
                objArray[intRowsStart, 78] = dr["ActPulloutDate"];
                objArray[intRowsStart, 79] = dr["PulloutQty"];
                objArray[intRowsStart, 80] = dr["ActPulloutTime"];
                objArray[intRowsStart, 81] = MyUtility.Convert.GetString(dr["PulloutComplete"]).ToUpper() == "TRUE" ? "OK" : "";
                objArray[intRowsStart, 82] = dr["FtyKPI"]; 
                KPIChangeReasonName = dr["KPIChangeReason"].ToString().Trim() + "-" + dr["KPIChangeReasonName"].ToString().Trim();
                objArray[intRowsStart, 83] = !MyUtility.Check.Empty(dr["KPIChangeReason"]) ? KPIChangeReasonName : ""; //cc
                objArray[intRowsStart, 84] = dr["PlanDate"];
                objArray[intRowsStart, 85] = dr["OrigBuyerDelivery"];
                objArray[intRowsStart, 86] = dr["SMR"];
                objArray[intRowsStart, 87] = dr["SMRName"];
                objArray[intRowsStart, 88] = dr["MRHandle"];
                objArray[intRowsStart, 89] = dr["MRHandleName"];
                objArray[intRowsStart, 90] = dr["POSMR"];
                objArray[intRowsStart, 91] = dr["POSMRName"];
                objArray[intRowsStart, 92] = dr["POHandle"];
                objArray[intRowsStart, 93] = dr["POHandleName"];
                objArray[intRowsStart, 94] = dr["MCHandle"];
                objArray[intRowsStart, 95] = dr["MCHandleName"];
                objArray[intRowsStart, 96] = dr["DoxType"];
                objArray[intRowsStart, 97] = dr["PackingCTN"];
                objArray[intRowsStart, 98] = dr["TotalCTN1"];
                objArray[intRowsStart, 99] = dr["FtyCtn1"];
                objArray[intRowsStart, 100] = dr["ClogCTN1"];
                objArray[intRowsStart, 101] = dr["ClogRcvDate"];
                objArray[intRowsStart, 102] = dr["InspDate"];
                objArray[intRowsStart, 103] = dr["InspResult"];
                objArray[intRowsStart, 104] = dr["InspHandle"];
                objArray[intRowsStart, 105] = dr["SewLine"];
                objArray[intRowsStart, 106] = dr["ShipModeList"];
                objArray[intRowsStart, 107] = dr["Article"];
                objArray[intRowsStart, 108] = dr["SpecialMarkName"];
                objArray[intRowsStart, 109] = dr["FTYRemark"];
                objArray[intRowsStart, 110] = dr["SampleReasonName"];
                objArray[intRowsStart, 111] = MyUtility.Convert.GetString(dr["IsMixMarker"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 112] = dr["CuttingSP"];
                objArray[intRowsStart, 113] = MyUtility.Convert.GetString(dr["RainwearTestPassed"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 114] = MyUtility.Convert.GetDecimal(dr["CPU"])*stdTMS;
                #endregion
                //先清空Subprocess值
                //for (int i = 115; i < lastCol; i++)
                //{
                //    objArray[intRowsStart, i] = 0;
                //    if (subtrue == 1)
                //    {
                //       objArray[intRowsStart, subConCol - 1] = "";

                //    }

                //}

                if (artwork || pap)
                {

                    //DataRow[] finRow = orderArtworkData.Select(string.Format("ID = '{0}'", MyUtility.Convert.GetString(dr["ID"])));
                    var finRow = lookupID[dr["ID"].ToString()];
                    //var finRow = from record in orderArtworkData.AsEnumerable()
                    //             where record.Field<string>("ID") == MyUtility.Convert.GetString(dr["ID"])
                    //             select record;

                    // if (finRow.Length > 0)
                    if (finRow.Count() > 0)
                    {
                        foreach (DataRow sdr in finRow)
                        {
                            if (!MyUtility.Check.Empty(sdr["AUnitRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["AUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }
                            if (!MyUtility.Check.Empty(sdr["PUnitRno"]))
                            {
                                if (MyUtility.Convert.GetString(sdr["ProductionUnit"]).ToUpper() == "TMS")
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["PUnitRno"]) - 1] = sdr["TMS"];
                                }
                                else
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["PUnitRno"]) - 1] = sdr["Price"];
                                }
                            }

                            if (!MyUtility.Check.Empty(sdr["NRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["NRno"]) - 1] = MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            //TTL
                            if (!MyUtility.Check.Empty(sdr["TAUnitRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TAUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }
                            if (!MyUtility.Check.Empty(sdr["TPUnitRno"]))
                            {
                                if (MyUtility.Convert.GetString(sdr["ProductionUnit"]).ToUpper() == "TMS")
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TPUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["TMS"]);
                                }
                                else
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TPUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Price"]);
                                }
                            }

                            if (!MyUtility.Check.Empty(sdr["TNRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TNRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            if (subConCol != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["Supp"]))
                                {
                                    objArray[intRowsStart, subConCol - 1] = sdr["Supp"];

                                }
                            }
                        }
                    }

                    objArray[intRowsStart, ttlTMS - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * stdTMS;
                }

                else
                {
                    objArray[intRowsStart, 114] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * stdTMS;
                }


                intRowsStart++;


            }

            if (intRowsStart == 0)
            {
                Microsoft.Office.Interop.Excel.Workbook workbook_close = excel.ActiveWorkbook;
                workbook_close.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook_close);
                this.HideWaitMessage();
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            //空值給0
            if (artwork || pap)
            {
                for (int j = 0; j < intRowsStart; j++)
                {
                    for (int i = 115; i < lastCol; i++)
                    {
                        if (objArray[j, i] == null)
                        {
                            objArray[j, i] = 0;
                        }

                    }
                }
            }
          

            worksheet.Range[String.Format("A{0}:{1}{0}", 1, excelColEng)].AutoFilter(1); //篩選
            worksheet.Range[String.Format("A{0}:{1}{0}", 1, excelColEng)].Interior.Color = Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191))))); //底色
            worksheet.Range[String.Format("A{0}:{1}{2}", 2, excelColEng, intRowsStart + 1)].Value2 = objArray;
            subtrue = 0;

            // 顯示筆數於PrintForm上Count欄位
            SetCount(intRowsStart);


            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R03_PPICMasterList");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            this.HideWaitMessage();
            #endregion
            return true;
        }
    }
}
