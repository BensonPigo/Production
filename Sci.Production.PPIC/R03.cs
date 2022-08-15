﻿using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R03
    /// </summary>
    public partial class R03 : Win.Tems.PrintForm
    {
        private string style;
        private string Article;
        private string season;
        private string brand;
        private string custcd;
        private string zone;
        private string mDivision;
        private string factory;
        private string subProcess;
        private string sp1;
        private string sp2;
        private bool bulk;
        private bool sample;
        private bool material;
        private bool forecast;
        private bool hisOrder;
        private bool artwork;
        private bool pap;
        private bool seperate;
        private bool poCombo;
        private bool garment;
        private bool smtl;
        private bool printingDetail;
        private DateTime? buyerDlv1;
        private DateTime? buyerDlv2;
        private DateTime? sciDlv1;
        private DateTime? sciDlv2;
        private DateTime? cutoff1;
        private DateTime? cutoff2;
        private DateTime? custRQS1;
        private DateTime? custRQS2;
        private DateTime? planDate1;
        private DateTime? planDate2;
        private DateTime? orderCfm1;
        private DateTime? orderCfm2;
        private DataTable printData;
        private DataTable subprocessColumnName;
        private DataTable orderArtworkData;
        private DataTable printingDetailDatas;
        private decimal stdTMS;
        private int subtrue = 0;

        /// <summary>
        /// Subtrue
        /// </summary>
        public int Subtrue
        {
            get
            {
                return this.subtrue;
            }

            set
            {
                this.subtrue = value;
            }
        }

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        /// <param name="type">type</param>
        public R03(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.Text = type == "1" ? "R03. PPIC master list report" : "R031. PPIC master list report (Artwork)";
            this.checkIncludeArtworkdata.Enabled = type != "1";
            this.checkIncludeArtworkdataKindIsPAP.Enabled = type != "1";
            this.checkByCPU.Enabled = type != "1";

            DataTable zone, mDivision, factory, subprocess;
            string strSelectSql = @"select '' as Zone,'' as Fty union all
select distinct f.Zone,f.Zone+' - '+(select CONCAT(ID,'/') from Factory WITH (NOLOCK) where Zone = f.Zone for XML path('')) as Fty
from Factory f WITH (NOLOCK) where Zone <> ''";

            DBProxy.Current.Select(null, strSelectSql, out zone);
            MyUtility.Tool.SetupCombox(this.comboZone, 2, zone);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from ArtworkType WITH (NOLOCK) where ReportDropdown = 1", out subprocess);
            MyUtility.Tool.SetupCombox(this.comboSubProcess, 1, subprocess);

            this.comboZone.SelectedIndex = 0;
            this.comboM.Text = Env.User.Keyword;
            this.comboFactory.Text = Env.User.Factory;
            this.comboSubProcess.SelectedIndex = 0;
            this.checkBulk.Checked = true;

            if (type != "1")
            {
                this.checkIncludeArtworkdata.Checked = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateCutOffDate.Value1) && MyUtility.Check.Empty(this.dateCutOffDate.Value2) &&
                MyUtility.Check.Empty(this.dateCustRQSDate.Value1) && MyUtility.Check.Empty(this.dateCustRQSDate.Value2) &&
                MyUtility.Check.Empty(this.datePlanDate.Value1) && MyUtility.Check.Empty(this.datePlanDate.Value2) &&
                MyUtility.Check.Empty(this.dateOrderCfmDate.Value1) && MyUtility.Check.Empty(this.dateOrderCfmDate.Value2))
            {
                MyUtility.Msg.WarningBox("All date can't empty!!");
                this.dateBuyerDelivery.TextBox1.Focus();
                return false;
            }

            this.buyerDlv1 = this.dateBuyerDelivery.Value1;
            this.buyerDlv2 = this.dateBuyerDelivery.Value2;
            this.sciDlv1 = this.dateSCIDelivery.Value1;
            this.sciDlv2 = this.dateSCIDelivery.Value2;
            this.cutoff1 = this.dateCutOffDate.Value1;
            this.cutoff2 = this.dateCutOffDate.Value2;
            this.custRQS1 = this.dateCustRQSDate.Value1;
            this.custRQS2 = this.dateCustRQSDate.Value2;
            this.planDate1 = this.datePlanDate.Value1;
            this.planDate2 = this.datePlanDate.Value2;
            this.orderCfm1 = this.dateOrderCfmDate.Value1;
            this.orderCfm2 = this.dateOrderCfmDate.Value2;
            this.sp1 = this.txtSp1.Text;
            this.sp2 = this.txtSp2.Text;
            this.style = this.txtstyle.Text.Trim();
            this.Article = this.txtArticle.Text.Trim();
            this.season = this.txtseason.Text.Trim();
            this.brand = this.txtbrand.Text.Trim();
            this.custcd = this.txtcustcd.Text.Trim();

            this.zone = MyUtility.Convert.GetString(this.comboZone.SelectedValue);
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.bulk = this.checkBulk.Checked;
            this.sample = this.checkSample.Checked;
            this.material = this.checkMaterial.Checked;
            this.forecast = this.checkForecast.Checked;
            this.garment = this.checkGarment.Checked;
            this.smtl = this.checkSMTL.Checked;
            this.subProcess = this.comboSubProcess.Text;
            this.hisOrder = this.checkIncludeHistoryOrder.Checked;
            this.artwork = this.checkIncludeArtworkdata.Checked;
            this.pap = this.checkIncludeArtworkdataKindIsPAP.Checked;
            this.seperate = this.checkQtyBDownByShipmode.Checked;
            this.poCombo = this.checkListPOCombo.Checked;

            return base.ValidateInput();
        }

        private StringBuilder Select_cmd(string p_type)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string seperCmd = string.Empty, seperCmdkpi = string.Empty, seperCmdkpi2 = string.Empty;
            string order_QtyShip_Source_InspDate = string.Empty, order_QtyShip_Source_InspResult = string.Empty, order_QtyShip_Source_InspHandle = string.Empty, order_QtyShip_OuterApply = string.Empty;

            this.printingDetail = this.chkPrintingDetail.Checked;
            #region 組SQL
            if (this.seperate && p_type.Equals("ALL"))
            {
                seperCmd = " ,oq.Seq,[IDD] = Format(oq.IDD, 'yyyy/MM/dd')";
            }
            else
            {
                seperCmd = @" ,[IDD] = (SELECT  Stuff((select distinct concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = o.ID FOR XML PATH('')),1,1,'') )";
            }

            seperCmdkpi = this.seperate ? "oq.FtyKPI" : "o.FtyKPI";
            seperCmdkpi2 = this.seperate ? @" left join Order_QtyShip oq WITH (NOLOCK) on o.id=oq.Id" : string.Empty;
            order_QtyShip_Source_InspDate = this.seperate ? "oq.CFAFinalInspectDate " : "QtyShip_InspectDate.Val";
            order_QtyShip_Source_InspResult = this.seperate ? "oq.CFAFinalInspectResult" : "QtyShip_Result.Val";
            order_QtyShip_Source_InspHandle = this.seperate ? "oq.CFAFinalInspectHandle" : "QtyShip_Handle.Val";
            order_QtyShip_OuterApply = this.seperate ? string.Empty : $@"
	OUTER APPLY(
		SELECT [Val]=STUFF((
		    SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		    from Order_QtyShip oq WITH (NOLOCK)
		    WHERE ID = o.id
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_InspectDate
	OUTER APPLY(
		SELECT [Val]=STUFF((
		    SELECT  DISTINCT ','+ CFAFinalInspectResult
		    from Order_QtyShip oq WITH (NOLOCK)
		    WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_Result
	OUTER APPLY(
		SELECT [Val]=STUFF((
		SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
		    from Order_QtyShip oq WITH (NOLOCK)
			LEFT JOIN Pass1 p WITH (NOLOCK) ON oq.CFAFinalInspectHandle = p.ID 
		    WHERE oq.ID = o.id AND CFAFinalInspectHandle <> '' AND CFAFinalInspectHandle IS NOT NULL
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_Handle
";

            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            string wherenoRestrictOrdersDelivery = string.Empty;
            MyUtility.Check.Seek($"select NoRestrictOrdersDelivery from system", out DataRow dr);
            if (!MyUtility.Convert.GetBool(dr["NoRestrictOrdersDelivery"]))
            {
                wherenoRestrictOrdersDelivery = @"
    and (o.IsForecast = 0 or (o.IsForecast = 1 and (o.SciDelivery <= dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),6) or o.BuyerDelivery < dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),0))))";
            }

            // 注意!! 新增欄位也要一併新增在poCombo (搜尋KeyWork: union)
            sqlCmd.Append($@"
with tmpOrders as (
    select DISTINCT o.ID
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
            , s.StyleName
            , o.SeasonID
            , o.BrandID
            , o.ProjectID
            , Kit=(SELECT top 1 c.Kit From CustCD c WITH (NOLOCK) where c.ID=o.CustCDID AND c.BrandID=o.BrandID)
			,[PackingMethod]=d.Name 
            , o.HangerPack
            , o.Customize1
            , o.BuyMonth
            , o.CustPONo
            , o.CustCDID
            , o.ProgramID
			, [NonRevenue]=IIF(o.NonRevenue=1,'Y','N')
            , o.CdCodeID
	        , s.CDCodeNew
            , [ProductType] = r2.Name
		    , [FabricType] = r1.Name
		    , s.Lining
		    , s.Gender
		    , [Construction] = d1.Name
            , o.CPU
            , o.Qty as Qty
            , o.FOCQty
            , o.LocalOrder
            , o.PoPrice
            , o.CMPPrice
            , o.KPILETA
            , o.PFETA
            , o.LETA
            , o.MTLETA
            , o.SewETA
            , o.PackETA
            , o.MTLComplete
            , o.SewInLine
            , o.SewOffLine
            , o.CutInLine
            , o.CutOffLine
            , Category=case when o.Category='B'then'Bulk'
							when o.Category='G'then'Garment'
							when o.Category='M'then'Material'
							when o.Category='S'then'Sample'
							when o.Category='T'then'Sample mtl.'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
						end
            , o.PulloutDate
            , o.ActPulloutDate
            , o.SMR
            , o.MRHandle
            , o.MCHandle
            , o.OrigBuyerDelivery
            , o.DoxType
            , o.TotalCTN
            , PackErrorCtn = isnull(o.PackErrCTN,0)
            , o.FtyCTN
            , o.ClogCTN
            , CFACTN=isnull(o.CFACTN,0)
            , o.VasShas
            , o.TissuePaper
            , [MTLExport]=IIF(o.MTLExport='OK','Y',o.MTLExport)
            , o.SewLine
            , o.ShipModeList
            , o.PlanDate
            , o.FirstProduction
			, o.LastProductionDate
            , o.OrderTypeID
            , o.SpecialMark
            , o.SampleReason
            , InspDate = {order_QtyShip_Source_InspDate}
            , InspResult = {order_QtyShip_Source_InspResult}
            , InspHandle = {order_QtyShip_Source_InspHandle}
            , o.MnorderApv2
            , o.MnorderApv
            , o.PulloutComplete
            , {seperCmdkpi}
            , o.KPIChangeReason
            , o.EachConsApv
            , o.Junk
            , o.StyleUkey
            , o.CuttingSP
            , o.RainwearTestPassed
            , o.BrandFTYCode
            , o.CPUFactor
            , o.ClogLastReceiveDate
			, [IsMixMarker]=  CASE WHEN o.IsMixMarker=0 THEN 'Is Single Marker'
								WHEN o.IsMixMarker=1 THEN 'Is Mix  Marker'		
								WHEN o.IsMixMarker=2 THEN ' Is Mix Marker - SCI'
								ELSE ''
							END
            , o.GFR 
			, isForecast = iif(isnull(o.Category,'')='','1','')
            , [AirFreightByBrand] = IIF(o.AirFreightByBrand='1','Y','')
            , [BuyBack] = iif(exists (select 1 from Order_BuyBack WITH (NOLOCK) where ID = o.ID), 'Y', '')
            , [Cancelled] = case when o.junk = 1 then 
                                 case when o.NeedProduction = 1 then 'Y' 
                                      when o.KeepPanels = 1 then 'K'
                                      else 'N' end
                            else ''
                            end
            , o.Customize2
            , o.KpiMNotice
            , o.KpiEachConsCheck
            , o.LastCTNTransDate
            , o.LastCTNRecdDate
            , o.DryRoomRecdDate
            , o.DryRoomTransDate
            , o.MdRoomScanDate
            , [VasShasCutOffDate] = Format(DATEADD(DAY, -30, iif(GetMinDate.value is null, coalesce(o.BuyerDelivery, o.CRDDate, o.PlanDate, o.OrigBuyerDelivery), GetMinDate.value)), 'yyyy/MM/dd')
            , [StyleSpecialMark] = s.SpecialMark
            {seperCmd}
	        , [SewingMtlComplt]  = isnull(CompltSP.SewingMtlComplt, '')
	        , [PackingMtlComplt] = isnull(CompltSP.PackingMtlComplt, '')
    from Orders o WITH (NOLOCK) 
    left join style s WITH (NOLOCK) on o.styleukey = s.ukey
	left join DropDownList d ON o.CtnType=d.ID AND d.Type='PackingMethod'
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
    {seperCmdkpi2}
    OUTER APPLY(
        SELECT  Name 
        FROM Pass1 WITH (NOLOCK) 
        WHERE Pass1.ID = O.InspHandle
    )I
	outer apply (
		select value = (
			select Min(Date)
			From (Values (o.BuyerDelivery), (o.CRDDate), (o.PlanDate), (o.OrigBuyerDelivery)) as tmp (Date)
			where tmp.Date is not null
		)
	) GetMinDate
    outer apply (
	    select 
		    [PackingMtlComplt] = max([PackingMtlComplt])
		    , [SewingMtlComplt] = max([SewingMtlComplt])
	    from 
	    (
		    select  f.ProductionType
			    , [PackingMtlComplt] = iif(f.ProductionType = 'Packing' and sum(iif(f.ProductionType = 'Packing', 1, 0)) = sum(iif(f.ProductionType = 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
			    , [SewingMtlComplt] = iif(f.ProductionType <> 'Packing' and sum(iif(f.ProductionType <> 'Packing', 1, 0)) = sum(iif(f.ProductionType <> 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
		    from 
		    (
			    select f.ProductionType
				    , psd.Complete
			    from PO_Supp_Detail psd WITH (NOLOCK)
			    inner join PO_Supp_Detail_OrderList psdo WITH (NOLOCK) on psd.ID = psdo.ID and psd.SEQ1 = psdo.SEQ1 and psd.SEQ2 = psdo.SEQ2
			    outer apply (
				    select [ProductionType] = iif(m.ProductionType = 'Packing', 'Packing', 'Sewing')
				    from Fabric f WITH (NOLOCK)
				    left join MtlType m WITH (NOLOCK) on f.MtlTypeID = m.ID
				    where f.SCIRefno = psd.SCIRefno
			    )f  
			    where psdo.OrderID	= o.ID
			    and psd.Junk = 0
		    )f
		    group by f.ProductionType
	    )f
    )CompltSP
{order_QtyShip_OuterApply}    
	outer apply(select oa.Article from Order_article oa WITH (NOLOCK) where oa.id = o.id) a
    where  1=1 {whereIncludeCancelOrder}
    {wherenoRestrictOrdersDelivery}
");
            if (this.seperate)
            {
                if (!MyUtility.Check.Empty(this.buyerDlv1))
                {
                    sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDlv1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.buyerDlv2))
                {
                    sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDlv2).ToString("yyyy/MM/dd")));
                }
            }
            else
            {
                if (!MyUtility.Check.Empty(this.buyerDlv1))
                {
                    sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDlv1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.buyerDlv2))
                {
                    sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDlv2).ToString("yyyy/MM/dd")));
                }
            }

            if (!MyUtility.Check.Empty(this.sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDlv1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDlv2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.cutoff1) || !MyUtility.Check.Empty(this.cutoff2))
            {
                sqlCmd.Append("and o.id in (select id from Order_QtyShip oq2 WITH (NOLOCK) where 1=1");
                if (!MyUtility.Check.Empty(this.cutoff1))
                {
                    sqlCmd.Append(string.Format(" and oq2.SDPDate >= '{0}'", Convert.ToDateTime(this.cutoff1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.cutoff2))
                {
                    sqlCmd.Append(string.Format(" and oq2.SDPDate <= '{0}'", Convert.ToDateTime(this.cutoff2).ToString("yyyy/MM/dd")));
                }

                sqlCmd.Append(")");
            }

            if (!MyUtility.Check.Empty(this.custRQS1))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate >= '{0}'", Convert.ToDateTime(this.custRQS1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.custRQS2))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate <= '{0}'", Convert.ToDateTime(this.custRQS2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.planDate1))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate >= '{0}'", Convert.ToDateTime(this.planDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.planDate2))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate <= '{0}'", Convert.ToDateTime(this.planDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.orderCfm1))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate >= '{0}'", Convert.ToDateTime(this.orderCfm1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.orderCfm2))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate <= '{0}'", Convert.ToDateTime(this.orderCfm2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sp1))
            {
                sqlCmd.Append(string.Format(" and o.id >= '{0}'", this.sp1));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlCmd.Append(string.Format(" and o.id <= '{0}'", this.sp2));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.Article))
            {
                sqlCmd.Append(string.Format(" and (a.Article = '{0}' or (a.Article is null and 1=1))", this.Article));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(string.Format(" and o.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.custcd))
            {
                sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", this.custcd));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
            }

            if (!this.hisOrder)
            {
                sqlCmd.Append(" and o.Finished = 0");
            }

            if ((this.bulk || this.sample || this.material || this.forecast || this.garment || this.smtl) && p_type.Equals("ALL"))
            {
                sqlCmd.Append(" and (1=0");
                if (this.bulk)
                {
                    sqlCmd.Append(" or o.Category = 'B'");
                }

                if (this.sample)
                {
                    sqlCmd.Append(" or o.Category = 'S'");
                }

                if (this.material)
                {
                    sqlCmd.Append(" or o.Category = 'M'");
                }

                if (this.garment)
                {
                    sqlCmd.Append(" or o.Category = 'G'");
                }

                if (this.smtl)
                {
                    sqlCmd.Append(" or o.Category = 'T'");
                }

                // 如果沒勾seperate但有勾forecast的情況，不用將forecast資料另外收
                if (this.forecast && !this.seperate)
                {
                    sqlCmd.Append(" or o.Category = ''");
                }

                sqlCmd.Append(")");
            }

            // forcast 另外出在excel的最下方，因為會與Separate條件衝突，所以另外處理
            if (this.forecast && p_type.Equals("forecast"))
            {
                sqlCmd.Append(" and o.Category = ''");
            }

            sqlCmd.Append(@"
),
tmpFilterZone as (
    select t.* 
    from tmpOrders t");
            if (!MyUtility.Check.Empty(this.zone))
            {
                sqlCmd.Append(string.Format(
                    @"
    inner join Factory f WITH (NOLOCK) on t.FactoryID = f.ID
    where f.Zone = '{0}'", this.zone));
            }

            sqlCmd.Append(@"
), tmpFilterSubProcess as (
    select t.*
    from tmpFilterZone t");
            if (!MyUtility.Check.Empty(this.subProcess))
            {
                sqlCmd.Append(string.Format(
                    @"
    inner join Style_TmsCost st WITH (NOLOCK) on t.StyleUkey = st.StyleUkey
    where st.ArtworkTypeID = '{0}' AND (st.Qty>0 or st.TMS>0 and st.Price>0) ", this.subProcess));
            }

            // 注意! 新增欄位也要一併新增在這!!
            if (this.poCombo)
            {
                if (this.seperate && p_type.Equals("ALL"))
                {
                    seperCmd = " , '' seq, '' IDD ";
                }
                else
                {
                    seperCmd = @" ,[IDD] = (SELECT  Stuff((select distinct concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = o.ID FOR XML PATH('')),1,1,'') )";
                }

                sqlCmd.Append($@"
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
            , s.StyleName
            , o.SeasonID
            , o.BrandID
            , o.ProjectID
            , Kit=(SELECT top 1 c.Kit From CustCD c WITH (NOLOCK) where c.ID=o.CustCDID AND c.BrandID=o.BrandID)
			,[PackingMethod] = d.Name
            , o.HangerPack
            , o.Customize1
            , o.BuyMonth
            , o.CustPONo
            , o.CustCDID
            , o.ProgramID
            , [NonRevenue]=IIF(o.NonRevenue=1,'Y','N')
            , o.CdCodeID
            , s.CDCodeNew
            , [ProductType] = r2.Name
		    , [FabricType] = r1.Name
		    , s.Lining
		    , s.Gender
		    , [Construction] = d1.Name
            , o.CPU
            , o.Qty
            , o.FOCQty
            , o.LocalOrder
            , o.PoPrice
            , o.CMPPrice
            , o.KPILETA
            , o.PFETA
            , o.LETA
            , o.MTLETA
            , o.SewETA
            , o.PackETA
            , o.MTLComplete
            , o.SewInLine
            , o.SewOffLine
            , o.CutInLine
            , o.CutOffLine
            , Category=case when o.Category='B'then'Bulk'
							when o.Category='G'then'Garment'
							when o.Category='M'then'Material'
							when o.Category='S'then'Sample'
							when o.Category='T'then'Sample mtl.'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
						end
            , o.PulloutDate
            , o.ActPulloutDate
            , o.SMR
            , o.MRHandle
            , o.MCHandle
            , o.OrigBuyerDelivery
            , o.DoxType
            , o.TotalCTN
            , PackErrorCtn = isnull(o.PackErrCTN,0)
            , o.FtyCTN
            , o.ClogCTN
            , CFACTN=isnull(o.CFACTN,0)
            , o.VasShas
            , o.TissuePaper
            , o.MTLExport
            , o.SewLine
            , o.ShipModeList
            , o.PlanDate
            , o.FirstProduction
			, o.LastProductionDate
            , o.OrderTypeID
            , o.SpecialMark
            , o.SampleReason
            , InspDate = {order_QtyShip_Source_InspDate}
            , InspResult = {order_QtyShip_Source_InspResult}
            , InspHandle = {order_QtyShip_Source_InspHandle}
            , o.MnorderApv2
            , o.MnorderApv
            , o.PulloutComplete
            , " + seperCmdkpi + @"
            , o.KPIChangeReason
            , o.EachConsApv
            , o.Junk
            , o.StyleUkey
            , o.CuttingSP
            , o.RainwearTestPassed
            , o.BrandFTYCode
            , o.CPUFactor
            , o.ClogLastReceiveDate
			, [IsMixMarker]=  CASE WHEN o.IsMixMarker=0 THEN 'Is Single Marker'
								WHEN o.IsMixMarker=1 THEN 'Is Mix  Marker'		
								WHEN o.IsMixMarker=2 THEN ' Is Mix Marker - SCI'
								ELSE ''
							END
            , o.GFR 
			, isForecast = iif(isnull(o.Category,'')='','1','') 
            , [AirFreightByBrand] = IIF(o.AirFreightByBrand='1','Y','')
            , [BuyBack] = iif(exists (select 1 from Order_BuyBack WITH (NOLOCK) where ID = o.ID), 'Y', '')
            , [Cancelled] = case when o.junk = 1 then 
                                 case when o.NeedProduction = 1 then 'Y' 
                                      when o.KeepPanels = 1 then 'K'
                                      else 'N' end
                            else ''
                            end
            , o.Customize2
            , o.KpiMNotice
            , o.KpiEachConsCheck
            , o.LastCTNTransDate
            , o.LastCTNRecdDate
            , o.DryRoomRecdDate
            , o.DryRoomTransDate
            , o.MdRoomScanDate
            , [VasShasCutOffDate] = Format(DATEADD(DAY, -30, iif(GetMinDate.value	is null, coalesce(o.BuyerDelivery, o.CRDDate, o.PlanDate, o.OrigBuyerDelivery), GetMinDate.value)), 'yyyy/MM/dd')
            , [StyleSpecialMark] = s.SpecialMark
            , [SewingMtlComplt]  = isnull(CompltSP.SewingMtlComplt, '')
            , [PackingMtlComplt] = isnull(CompltSP.PackingMtlComplt, '')
"
            + seperCmd +
    @"from Orders o  WITH (NOLOCK) 
    left join style s WITH (NOLOCK) on o.styleukey = s.ukey
	left join DropDownList d WITH (NOLOCK) ON o.CtnType=d.ID AND d.Type='PackingMethod'
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
    outer apply (
	        select 
		        [PackingMtlComplt] = max([PackingMtlComplt])
		        , [SewingMtlComplt] = max([SewingMtlComplt])
	        from 
	        (
		        select  f.ProductionType
			        , [PackingMtlComplt] = iif(f.ProductionType = 'Packing' and sum(iif(f.ProductionType = 'Packing', 1, 0)) = sum(iif(f.ProductionType = 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
			        , [SewingMtlComplt] = iif(f.ProductionType <> 'Packing' and sum(iif(f.ProductionType <> 'Packing', 1, 0)) = sum(iif(f.ProductionType <> 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
		        from 
		        (
			        select f.ProductionType
				        , psd.Complete
			        from PO_Supp_Detail psd WITH (NOLOCK)
			        inner join PO_Supp_Detail_OrderList psdo WITH (NOLOCK) on psd.ID = psdo.ID and psd.SEQ1 = psdo.SEQ1 and psd.SEQ2 = psdo.SEQ2
			        outer apply (
				        select [ProductionType] = iif(m.ProductionType = 'Packing', 'Packing', 'Sewing')
				        from Fabric f WITH (NOLOCK)
				        left join MtlType m WITH (NOLOCK) on f.MtlTypeID = m.ID
				        where f.SCIRefno = psd.SCIRefno
			        )f  
			        where psdo.OrderID	= o.ID
			        and psd.Junk = 0
		        )f
		        group by f.ProductionType
	        )f
        )CompltSP
   " + seperCmdkpi2 + $@"
    OUTER APPLY (
        SELECT Name 
        FROM Pass1 WITH (NOLOCK) 
        WHERE Pass1.ID=O.InspHandle
    )I
	outer apply (
		select value = (
			select Min(Date)
			From (Values (o.BuyerDelivery), (o.CRDDate), (o.PlanDate), (o.OrigBuyerDelivery)) as tmp (Date)
			where tmp.Date is not null
		)
	) GetMinDate
{order_QtyShip_OuterApply}  
    where o.POID IN (select distinct POID from tmpFilterSubProcess) 
{wherenoRestrictOrdersDelivery}
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

            sqlCmd.Append(@"select * into #tmpListPoCombo from tmpListPoCombo;
                            CREATE NONCLUSTERED INDEX index_tmpListPoCombo_ID ON #tmpListPoCombo(	ID ASC);");

            if (this.seperate && p_type.Equals("ALL"))
            {
                sqlCmd.Append(@"
select pd.OrderID, pd.OrderShipmodeSeq, Sum( pd.CTNQty) PackingCTN ,
	Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) TotalCTN,
	Sum( case when p.Type in ('B', 'L') and pd.TransferDate is null then pd.CTNQty else 0 end) FtyCtn,
	Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end) ClogCTN ,
	Sum(case when p.Type <> 'F'  then pd.ShipQty else 0 end) PackingQty ,
	Sum(case when p.Type = 'F'   then pd.ShipQty else 0 end) PackingFOCQty ,
	Sum(case when p.Type in ('B', 'L') and p.INVNo <> ''  then pd.ShipQty else 0 end) BookingQty ,
	Max (ReceiveDate) ClogRcvDate,
	MAX(p.PulloutDate)  ActPulloutDate
into #tmp_PLDetial
from PackingList_Detail pd WITH (NOLOCK) 
LEFT JOIN PackingList p WITH (NOLOCK) on pd.ID = p.ID 
inner join (select distinct id, seq from #tmpListPoCombo) t on pd.OrderID = t.ID  and pd.OrderShipmodeSeq = t.Seq
group by pd.OrderID, pd.OrderShipmodeSeq 

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
            , t.StyleName
            , t.SeasonID
            , t.BrandID
            , t.ProjectID
			, t.PackingMethod
            , t.HangerPack
            , t.Customize1
            , t.BuyMonth
            , t.CustPONo
            , t.CustCDID
			, t.Kit
            , t.ProgramID
			, t.NonRevenue
            , t.CdCodeID
	        , t.CDCodeNew
	        , t.ProductType
	        , t.FabricType
	        , t.Lining
	        , t.Gender
	        , t.Construction
            , t.CPU
            , oq.Qty as Qty
            , t.FOCQty
            , t.LocalOrder
            , t.PoPrice
            , t.CMPPrice
            , t.KPILETA
            , t.PFETA
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
            , pdm.ActPulloutDate 
            , t.SMR
            , t.MRHandle
            , t.MCHandle
            , t.OrigBuyerDelivery
            , t.DoxType
            , t.VasShas
            , t.TissuePaper
            , t.MTLExport
            , t.SewLine
            , oq.ShipmodeID as ShipModeList
            , t.PlanDate
            , t.FirstProduction
            , t.LastProductionDate
            , t.OrderTypeID
            , t.SpecialMark
            , t.SampleReason
            , t.InspDate
            , t.InspResult
            , t.InspHandle
            , t.MnorderApv2
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
            , [IDD] = Format(oq.IDD, 'yyyy/MM/dd')
            , t.ClogLastReceiveDate
            , t.IsMixMarker
            , t.GFR
            , pdm.PackingQty
			, pdm.PackingFOCQty 
			, pdm.BookingQty
			, pdm.PackingCTN
			, pdm.TotalCTN as  TotalCTN1
			, pdm.FtyCtn as  FtyCtn1
			, pdm.ClogCTN as  ClogCTN1
			, pdm.ClogRcvDate
            , t.PackErrorCtn
            , t.CFACTN
			, t.isForecast
			, t.AirFreightByBrand
            , [BuyBack] = iif(exists (select 1 from Order_BuyBack where ID = t.ID), 'Y', '')
            , t.Cancelled
            , t.Customize2
            , t.KpiMNotice
            , t.KpiEachConsCheck
            , t.LastCTNTransDate
            , t.LastCTNRecdDate
            , t.DryRoomRecdDate
            , t.DryRoomTransDate
            , t.MdRoomScanDate
            , t.VasShasCutOffDate
            , t.StyleSpecialMark
            , t.SewingMtlComplt
            , t.PackingMtlComplt
    into #tmpFilterSeperate
    from #tmpListPoCombo t
    inner join Order_QtyShip oq WITH(NOLOCK) on t.ID = oq.Id and t.Seq = oq.Seq
    left join #tmp_PLDetial pdm on pdm.OrderID = t.ID  and pdm.OrderShipmodeSeq = t.Seq ;

CREATE NONCLUSTERED INDEX index_tmpFilterSeperate ON #tmpFilterSeperate(	ID ASC,seq asc);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_POID ON #tmpFilterSeperate(	POID ASC);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_CuttingSP ON #tmpFilterSeperate(	CuttingSP ASC);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_StyleUkey ON #tmpFilterSeperate(	StyleUkey ASC);


select sod.OrderId,Sum( case when sod.ComboType = 'T'  then sod.QAQty else 0 end) SewQtyTop, 
	Sum( case when sod.ComboType = 'B'  then sod.QAQty else 0 end) SewQtyBottom, 
	Sum( case when sod.ComboType = 'I'  then sod.QAQty else 0 end) SewQtyInner, 
	Sum( case when sod.ComboType = 'O'  then sod.QAQty else 0 end) SewQtyOuter,
	Min (so.OutputDate) FirstOutDate,
	Max (so.OutputDate) LastOutDate
into #tmp_sewDetial
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
inner join (select distinct ID from #tmpFilterSeperate) t on sod.OrderId = t.ID
group by sod.OrderId

select ID, Remark
into #tmp_PFRemark
from (
	select ROW_NUMBER() OVER (PARTITION BY o.ID ORDER BY o.addDate, o.Ukey) r_id
		,o.ID, o.Remark
	from Order_PFHis o WITH (NOLOCK) 
	inner join #tmpFilterSeperate t on o.ID = t.ID 
	where AddDate = (
			select Max(o.AddDate) 
			from Order_PFHis o WITH (NOLOCK) 
			where ID = t.ID
		)   
	group by o.ID, o.Remark ,o.addDate, o.Ukey
)a
where r_id = '1' 

select ed.POID,Max(e.WhseArrival) ArriveWHDate 
into #tmp_ArriveWHDate
from Export e WITH (NOLOCK) 
inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
inner join #tmpFilterSeperate t on ed.POID = t.POID
group by ed.POID 

select StyleUkey ,dbo.GetSimilarStyleList(StyleUkey) GetStyleUkey
into #tmp_StyleUkey
from #tmpFilterSeperate 
group by StyleUkey 

select POID ,dbo.GetHaveDelaySupp(POID) MTLDelay
into #tmp_MTLDelay
from #tmpFilterSeperate 
group by POID 

select pd.OrderID, pd.OrderShipmodeSeq, sum(pd.ShipQty) PulloutQty
into #tmp_PulloutQty
from PackingList_Detail pd WITH (NOLOCK)
inner join #tmpFilterSeperate t on pd.OrderID = t.ID and pd.OrderShipmodeSeq = t.Seq
inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
where p.PulloutID <> ''
group by pd.OrderID, pd.OrderShipmodeSeq

select pd.OrderID, count(distinct p.PulloutID) ActPulloutTime
into #tmp_ActPulloutTime
from PackingList p WITH (NOLOCK)
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join #tmpFilterSeperate t on t.ID = pd.OrderID
where p.PulloutID <> ''
and pd.ShipQty > 0
group by pd.OrderID

select od.ID,od.Seq,od.Article 
into #tmp_Article
from Order_QtyShip_Detail od WITH (NOLOCK) 
inner join #tmpFilterSeperate t on  od.ID = t.ID and od.Seq = t.Seq 
group by od.ID,od.Seq,od.Article 


CREATE NONCLUSTERED INDEX index_tmp_sewDetial ON #tmp_sewDetial(	OrderId ASC);
CREATE NONCLUSTERED INDEX index_tmp_PFRemark ON #tmp_PFRemark(	ID ASC);
CREATE NONCLUSTERED INDEX index_tmp_ArriveWHDate ON #tmp_ArriveWHDate(	PoID ASC);
CREATE NONCLUSTERED INDEX index_tmp_StyleUkey ON #tmp_StyleUkey(	StyleUkey ASC);
CREATE NONCLUSTERED INDEX index_tmp_MTLDelay ON #tmp_MTLDelay(	POID ASC);
CREATE NONCLUSTERED INDEX index_tmp_PulloutQty ON #tmp_PulloutQty(	OrderID ASC, OrderShipmodeSeq);
CREATE NONCLUSTERED INDEX index_tmp_ActPulloutTime ON #tmp_ActPulloutTime(	OrderID ASC);


select  t.ID
            , t.MDivisionID
            , t.FtyGroup
            , t.FactoryID
            , t.BuyerDelivery
            , t.SciDelivery
            , t.POID
            , t.CRDDate
            , t.CFMDate
            , t.Dest
            , t.StyleID
            , t.StyleName
            , t.SeasonID
            , t.BrandID
            , t.ProjectID
			, t.PackingMethod
            , t.HangerPack
            , t.Customize1
            , t.BuyMonth
            , t.CustPONo
            , t.CustCDID
			, t.Kit
            , t.ProgramID
			, t.NonRevenue
            , t.CdCodeID
	        , t.CDCodeNew
	        , t.ProductType
	        , t.FabricType
	        , t.Lining
	        , t.Gender
	        , t.Construction
            , t.CPU
            , t.Qty as Qty
            , t.FOCQty
            , t.LocalOrder
            , t.PoPrice
            , t.CMPPrice
            , t.KPILETA
            , t.PFETA
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
            , t.PulloutDate
            , t.ActPulloutDate 
            , t.SMR
            , t.MRHandle
            , t.MCHandle
            , t.OrigBuyerDelivery
            , t.DoxType
            , t.VasShas
            , t.TissuePaper
            , t.MTLExport
            , t.SewLine
            , t.ShipModeList
            , t.PlanDate
            , t.FirstProduction
            , t.LastProductionDate
            , t.OrderTypeID
            , t.SpecialMark
            , t.SampleReason
            , t.InspDate
            , t.InspResult
            , t.InspHandle
            , t.MnorderApv2
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
            , t.Seq
            , t.[IDD]
            , t.ClogLastReceiveDate
            , t.IsMixMarker
            , t.GFR
            , t.PackingQty
			, t.PackingFOCQty 
			, t.BookingQty
			, t.PackingCTN
			, t.TotalCTN1
			, t.FtyCtn1
			, t.ClogCTN1
			, t.ClogRcvDate
            , t.PackErrorCtn
            , t.CFACTN
			, t.isForecast
			, t.AirFreightByBrand
            , [BuyBack] = iif(exists (select 1 from Order_BuyBack WITH (NOLOCK) where ID = t.ID), 'Y', '')
            , t.Cancelled
            , t.Customize2
            , t.KpiMNotice
            , t.KpiEachConsCheck
        , ModularParent = isnull (s.ModularParent, '')
        , CPUAdjusted = isnull (s.CPUAdjusted * 100, 0)
        , DestAlias = isnull (c.Alias, '')
        , FactoryDisclaimer = isnull (dd.Name, '')
        , FactoryDisclaimerRemark = isnull (s.ExpectionFormRemark, '')
        , ApprovedRejectedDate  = s.ExpectionFormDate
        , WorkType = isnull (ct.WorkType, '')
        , ct.FirstCutDate
        , POSMR = isnull (p.POSMR, '')
        , POHandle = isnull (p.POHandle, '') 
        , PCHandle = isnull (p.PCHandle, '') 
        , FTYRemark = isnull (s.FTYRemark, '')
        , som.SewQtyTop
        , som.SewQtyBottom
        , som.SewQtyInner
        , som.SewQtyOuter
        , TtlSewQty = isnull (dbo.getMinCompleteSewQty (t.ID, null, null) ,0)
        , CutQty = isnull ((select SUM(Qty) 
                            from CuttingOutput_WIP WITH (NOLOCK) 
                            where OrderID = t.ID)
                          , 0)
        , PFRemark = isnull(pf.Remark,'')
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
        , PCHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK)
                                  where Id = p.PCHandle)
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
                                     from Style_SpecialMark sp WITH(NOLOCK) 
                                     where sp.ID = t.[StyleSpecialMark]
                                     and sp.BrandID = t.BrandID
                                     and sp.Junk = 0)
                                    , '') 
        , MTLExportTimes = isnull ([dbo].getMTLExport (t.POID, t.MTLExport), '')
        , GMTLT = dbo.GetGMTLT(t.BrandID, t.StyleID, t.SeasonID, t.FactoryID,t.ID)
        , SimilarStyle = su.GetStyleUkey         
        , MTLDelay = isnull(mt.MTLDelay ,0)
        , InvoiceAdjQty = dbo.getInvAdjQty (t.ID, t.Seq) 
		, FOCAdjQty = dbo.getFOCInvAdjQty (t.ID, t.Seq) 
		, NotFOCAdjQty= dbo.getInvAdjQty (t.ID, t.Seq)-dbo.getFOCInvAdjQty (t.ID, t.Seq) 
        , ct.LastCutDate
        , ArriveWHDate =　aw.ArriveWHDate
        , som.FirstOutDate
        , som.LastOutDate 
        , PulloutQty = isnull(pu.PulloutQty,0)
        , ActPulloutTime = isnull(apu.ActPulloutTime,0) 
        , Article = isnull ((select CONCAT(Article,',') 
                             from #tmp_Article a 
							 where a.ID = t.ID and a.Seq = t.Seq
							 for xml path(''))
                           , '')
        , [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='F')
        , [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='A')
		, t.Cancelled
        , t.Customize2
        , t.KpiMNotice
        , t.KpiEachConsCheck
        , LastCTNTransDate = IIF(isnull(t.FtyCtn1,0) =0 , t.LastCTNTransDate , null)
		, LastCTNRecdDate = IIF(isnull(t.FtyCtn1,0) =0 , t.LastCTNRecdDate , null)
		, DryRoomRecdDate = IIF(isnull(t.FtyCtn1,0) =0 , t.DryRoomRecdDate , null)
		, DryRoomTransDate = IIF(isnull(t.FtyCtn1,0) =0 , t.DryRoomTransDate , null)
		, MdRoomScanDate = IIF(isnull(t.FtyCtn1,0) =0 , t.MdRoomScanDate , null)
        , t.VasShasCutOffDate
        , t.SewingMtlComplt
        , t.PackingMtlComplt
from #tmpFilterSeperate t
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join DropDownList dd WITH (NOLOCK) on dd.Type = 'FactoryDisclaimer' and dd.id = s.ExpectionFormStatus
left join PO p WITH (NOLOCK) on p.ID = t.POID
left join Country c WITH (NOLOCK) on c.ID = t.Dest
left join #tmp_sewDetial som on som.OrderID = t.ID
left join #tmp_PFRemark pf on pf.ID = t.ID
left join #tmp_ArriveWHDate aw on aw.PoID = t.POID
left join #tmp_StyleUkey su on su.StyleUkey = t.StyleUkey 
left join #tmp_MTLDelay mt on mt.POID = t.POID
left join #tmp_PulloutQty pu on pu.OrderID = t.ID and pu.OrderShipmodeSeq = t.Seq
left join #tmp_ActPulloutTime apu on apu.OrderID = t.ID 
order by t.ID;
drop table #tmpListPoCombo;
drop table #tmp_PLDetial,#tmpFilterSeperate,#tmp_sewDetial,#tmp_PFRemark,#tmp_ArriveWHDate,#tmp_StyleUkey,#tmp_MTLDelay,#tmp_PulloutQty,#tmp_ActPulloutTime,#tmp_Article;");
            }
            else
            {
                sqlCmd.Append($@"
select ID, Remark
into #tmp_PFRemark
from (
	select ROW_NUMBER() OVER (PARTITION BY o.ID ORDER BY o.addDate, o.Ukey) r_id
		,o.ID, o.Remark
	from Order_PFHis o WITH (NOLOCK) 
	inner join #tmpListPoCombo t on o.ID = t.ID 
	where AddDate = (
			select Max(o.AddDate) 
			from Order_PFHis o WITH (NOLOCK) 
			where ID = t.ID
		)   
	group by o.ID, o.Remark ,o.addDate, o.Ukey
)a
where r_id = '1'

select StyleUkey ,dbo.GetSimilarStyleList(StyleUkey) GetStyleUkey
into #tmp_StyleUkey
from #tmpListPoCombo
group by StyleUkey 

select POID ,dbo.GetHaveDelaySupp(POID) MTLDelay
into #tmp_MTLDelay
from #tmpListPoCombo
group by POID 		    

select pld.OrderID, SUM(pld.ShipQty) PackingQty
into #tmp_PackingQty
from PackingList pl WITH (NOLOCK) 
inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
inner join #tmpListPoCombo t on pld.OrderID = t.ID
where  pl.Type <> 'F'  
group by pld.OrderID  

select pld.OrderID, SUM(pld.ShipQty) PackingFOCQty 
into #tmp_PackingFOCQty
from PackingList pl WITH (NOLOCK) 
inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
inner join #tmpListPoCombo t on pld.OrderID = t.ID
where pl.Type = 'F' 
group by pld.OrderID 

select pld.OrderID, SUM(pld.ShipQty) BookingQty
into #tmp_BookingQty
from PackingList pl WITH (NOLOCK) 
inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
inner join #tmpListPoCombo t on pld.OrderID = t.ID
where   (pl.Type = 'B' or pl.Type = 'S') 
        and pl.INVNo <> ''  
group by pld.OrderID 
 
select o.ID, o.Article 
into #tmp_Article
from Order_Qty o WITH (NOLOCK) 
inner join #tmpListPoCombo t on o.ID = t.ID
group by o.ID,o.Article 

CREATE NONCLUSTERED INDEX index_tmp_PFRemark ON #tmp_PFRemark(	ID ASC);
CREATE NONCLUSTERED INDEX index_tmp_StyleUkey ON #tmp_StyleUkey(	StyleUkey ASC);
CREATE NONCLUSTERED INDEX index_tmp_MTLDelay ON #tmp_MTLDelay(	POID ASC);
CREATE NONCLUSTERED INDEX index_tmp_PackingQty ON #tmp_PackingQty(	OrderID ASC);
CREATE NONCLUSTERED INDEX index_tmp_PackingFOCQty ON #tmp_PackingFOCQty(	OrderID ASC);
CREATE NONCLUSTERED INDEX index_tmp_BookingQty ON #tmp_BookingQty(	OrderID ASC);

select distinct 
              t.ID
            , t.MDivisionID
            , t.FtyGroup
            , t.FactoryID
            , t.BuyerDelivery
            , t.SciDelivery
            , t.POID
            , t.CRDDate
            , t.CFMDate
            , t.Dest
            , t.StyleID
            , t.StyleName
            , t.SeasonID
            , t.BrandID
            , t.ProjectID
            , t.Kit
			,[PackingMethod] 
            , t.HangerPack
            , t.Customize1
            , t.BuyMonth
            , t.CustPONo
            , t.CustCDID
            , t.ProgramID
			, [NonRevenue]
            , t.CdCodeID
	        , t.CDCodeNew
            , [ProductType]
		    , t. [FabricType]
		    , t.Lining
		    , t.Gender
		    , t.[Construction]
            , t.CPU
            , t.Qty
            , t.FOCQty
            , t.LocalOrder
            , t.PoPrice
            , t.CMPPrice
            , t.KPILETA
            , t.PFETA
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
            , t.PulloutDate
            , t.ActPulloutDate
            , t.SMR
            , t.MRHandle
            , t.MCHandle
            , t.OrigBuyerDelivery
            , t.DoxType
            , t.TotalCTN
            , PackErrorCtn
            , t.FtyCTN
            , t.ClogCTN
            , t.CFACTN
            , t.VasShas
            , t.TissuePaper
            , [MTLExport]
            , t.SewLine
            , t.ShipModeList
            , t.PlanDate
            , t.FirstProduction
			, t.LastProductionDate
            , t.OrderTypeID
            , t.SpecialMark
            , t.SampleReason
            , InspDate 
            , InspResult 
            , InspHandle 
            , t.MnorderApv2
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
            , t.ClogLastReceiveDate
			, [IsMixMarker]
            , t.GFR 
			, isForecast
            , [AirFreightByBrand]
            , [BuyBack]
            , [Cancelled]
            , t.Customize2
            , t.KpiMNotice
            , t.KpiEachConsCheck
            , t.[IDD] 
        , ModularParent = isnull (s.ModularParent, '')  
        , CPUAdjusted = isnull(s.CPUAdjusted * 100, 0)  
        , DestAlias = isnull (c.Alias, '') 
        , FactoryDisclaimer = isnull (dd.Name, '')
        , FactoryDisclaimerRemark = isnull (s.ExpectionFormRemark, '')
        , ApprovedRejectedDate  = s.ExpectionFormDate
        , WorkType = isnull(ct.WorkType,'')
        , ct.FirstCutDate
        , POSMR = isnull (p.POSMR, '')
        , POHandle = isnull (p.POHandle, '') 
        , PCHandle = isnull (p.PCHandle, '') 
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
        , PFRemark = isnull(pf.Remark,'')
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
        , PCHandleName = isnull ((select Name 
                                  from TPEPass1 WITH (NOLOCK)
                                  where Id = p.PCHandle)
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
                                     from Style_SpecialMark sp WITH(NOLOCK) 
                                     where sp.ID = t.[StyleSpecialMark] 
                                     and sp.BrandID = t.BrandID
                                     and sp.Junk = 0)
                                   , '')
        , MTLExportTimes = isnull ([dbo].getMTLExport (t.POID, t.MTLExport), '')
        , GMTLT = dbo.GetGMTLT(t.BrandID, t.StyleID, t.SeasonID, t.FactoryID, t.ID)
        , SimilarStyle = su.GetStyleUkey
        , MTLDelay = isnull(mt.MTLDelay,0)
        , PackingQty = isnull(pa.PackingQty ,0)
        , PackingFOCQty = isnull(paf.PackingFOCQty,0)
        , BookingQty = isnull(bo.BookingQty ,0)
        , InvoiceAdjQty = isnull (i.value, 0)
		, FOCAdjQty = isnull (i2.value, 0)
		, NotFOCAdjQty= isnull (i.value, 0)-isnull (i2.value, 0)
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
        , PulloutQty = isnull ((select sum(pd.ShipQty)
                                from PackingList_Detail pd WITH (NOLOCK)
                                inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                                where p.PulloutID <> ''
                                and pd.OrderID = t.ID)
                              , 0)
        , ActPulloutTime = (select count(distinct p.PulloutID)
                            from PackingList_Detail pd WITH (NOLOCK)
                            inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                            where p.PulloutID <> ''
                            and pd.OrderID = t.ID
                            and pd.ShipQty > 0)
        , PackingCTN = isnull ((select Sum(CTNQty) 
                                from PackingList_Detail WITH (NOLOCK) 
                                where OrderID = t.ID)
                              , 0) 
        , t.TotalCTN
        , FtyCtn = isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0)
        , ClogCTN = isnull(t.ClogCTN,0)
        , ClogRcvDate = t.ClogLastReceiveDate
		, Article = isnull ((select CONCAT(a.Article, ',') 
                             from #tmp_Article a 
							 where a.ID = t.ID
							 for xml path(''))
                           , '') 
        , [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='F')
        , [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='A')
        , t.Customize2
        , t.KpiMNotice
        , t.KpiEachConsCheck
        , [LastCTNTransDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.LastCTNTransDate, null)
        , [LastCTNRecdDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.LastCTNRecdDate, null)
        , [DryRoomRecdDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.DryRoomRecdDate, null)
        , [DryRoomTransDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.DryRoomTransDate, null)
        , [MdRoomScanDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.MdRoomScanDate, null)
        , t.VasShasCutOffDate
        , t.SewingMtlComplt
        , t.PackingMtlComplt
from #tmpListPoCombo t
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join DropDownList dd WITH (NOLOCK) on dd.Type = 'FactoryDisclaimer' and dd.id = s.ExpectionFormStatus
left join PO p WITH (NOLOCK) on p.ID = t.POID
left join Country c WITH (NOLOCK) on c.ID = t.Dest
left join #tmp_PFRemark pf on pf.ID = t.ID
left join #tmp_StyleUkey su on su.StyleUkey = t.StyleUkey 
left join #tmp_MTLDelay mt on mt.POID = t.POID
left join #tmp_PackingQty pa on pa.OrderID = t.ID
left join #tmp_PackingFOCQty paf on paf.OrderID = t.ID
left join #tmp_BookingQty bo on bo.OrderID = t.ID
outer apply(
	select value = sum(iq.DiffQty) 
	from InvAdjust i WITH (NOLOCK) 
	inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID = iq.ID
	where i.OrderID = t.ID
)i
outer apply(
	select value = sum(iq.DiffQty) 
	from InvAdjust i WITH (NOLOCK) 
	inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID = iq.ID
	where i.OrderID = t.ID
    and iq.Price = 0
)i2
order by t.ID;

drop table #tmpListPoCombo,#tmp_PFRemark,#tmp_StyleUkey,#tmp_MTLDelay,#tmp_PackingQty,#tmp_PackingFOCQty,#tmp_BookingQty,#tmp_Article;");
            }
            #endregion

            return sqlCmd;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd;

            // 抓取一般條件資料
            sqlCmd = this.Select_cmd("ALL");
            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            // 抓取forecast資料再merge回主datatable，只有forecast和seperate有勾的時候才做
            if (this.forecast && this.seperate)
            {
                DataTable printData_forecast;
                StringBuilder sqlCmd_forecast = this.Select_cmd("forecast");
                result = DBProxy.Current.Select(null, sqlCmd_forecast.ToString(), out printData_forecast);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                this.printData.Merge(printData_forecast);
            }

            if (this.printData.Rows.Count > 0)
            {
                if (this.artwork || this.pap)
                {
                    #region 組Subprocess欄位名稱
                    string classify;
                    if (this.artwork && this.pap)
                    {
                        classify = "'I','S','A','O','P'";
                    }
                    else if (this.artwork)
                    {
                        classify = "'I','S','A','O'";
                    }
                    else
                    {
                        classify = "'P'";
                    }

                    sqlCmd.Clear();

                    string strUnion = @"
    union all
    SELECT  ID = 'PrintSubCon'
            , Seq = ''
            , ArtworkUnit = '' 
            , ProductionUnit = '' 
            , SystemType = ''
            , FakeID = '9999ZZ'
            , ColumnN = 'POSubCon'
            , ColumnSeq = '998'
	union all
    SELECT  ID = 'PrintSubCon'
            , Seq = ''
            , ArtworkUnit = '' 
            , ProductionUnit = '' 
            , SystemType = ''
            , FakeID = '9999ZZ'
            , ColumnN = 'SubCon'
            , ColumnSeq = '999'";
                    string printingDetailcol = string.Empty;
                    if (this.printingDetail)
                    {
                        printingDetailcol = @"
		union all
		select ID = 'PRINTING'
                , Seq=''
                , ArtworkUnit=''
                , ProductionUnit=''
                , SystemType=''
                , FakeID = ''
                , ColumnN = 'Printing LT'
                , ColumnSeq = -1 

		union all
		select ID = 'PRINTING'
                , Seq=''
                , ArtworkUnit=''
                , ProductionUnit=''
                , SystemType=''
                , FakeID = ''
                , ColumnN = 'InkType/color/size'
                , ColumnSeq = 0 ";
                    }

                    sqlCmd.Append(string.Format(
                        @"
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
        
        union all
        SELECT  ID
                , Seq
                , ArtworkUnit
                , ProductionUnit=p.PUnit
                , SystemType
                , FakeID = Seq + 'U2'
                , ColumnN = RTRIM(ID) + ' ('+IIF(ProductionUnit = 'QTY','Price',p.PUnit)+')'
                , ColumnSeq = '2' 
        FROM ArtworkType WITH (NOLOCK) 
		outer apply(select  PUnit=iif('{3}'='true' and ProductionUnit = 'TMS','CPU',ProductionUnit))p
        WHERE   ProductionUnit <> '' 
                and Classify in ({0}) 
				and ID<> 'PRINTING PPU' 
        
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
        {1}
        {4}
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
            from SubProcess WITH (NOLOCK)
            where ID <> 'PrintSubCon' and ColumnN <> 'Printing LT' and ColumnN <> 'InkType/color/size'
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
    from SubProcess WITH (NOLOCK)

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
) a",
                        classify,
                        !this.artwork ? string.Empty : strUnion,
                        this.lastColA,
                        this.checkByCPU.Checked,
                        printingDetailcol));
                    #endregion
                    result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.subprocessColumnName);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query artworktype fail\r\n" + result.ToString());
                        return failResult;
                    }

                    #region 撈Order Subprocess資料
                    try
                    {
                        string wherenoRestrictOrdersDelivery = string.Empty;
                        MyUtility.Check.Seek($"select NoRestrictOrdersDelivery from system WITH (NOLOCK)", out DataRow dr);
                        if (!MyUtility.Convert.GetBool(dr["NoRestrictOrdersDelivery"]))
                        {
                            wherenoRestrictOrdersDelivery = @"
    and (o.IsForecast = 0 or (o.IsForecast = 1 and (o.SciDelivery <= dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),6) or o.BuyerDelivery < dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),0))))";
                        }

                        StringBuilder sqlcmd_sub = new StringBuilder();
                        sqlcmd_sub.Append($@"
with ArtworkData as (
    select * 
    from #tmp
),
OrderID as(
    select ID from orders O  WITH (NOLOCK)
    where  1=1
{wherenoRestrictOrdersDelivery}
");

                        if (!MyUtility.Check.Empty(this.buyerDlv1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDlv1).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.buyerDlv2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDlv2).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.sciDlv1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDlv1).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.sciDlv2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDlv2).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.cutoff1) || !MyUtility.Check.Empty(this.cutoff2))
                        {
                            sqlCmd.Append("and o.id in (select id from Order_QtyShip oq2 WITH (NOLOCK) where 1=1");
                            if (!MyUtility.Check.Empty(this.cutoff1))
                            {
                                sqlCmd.Append(string.Format(" and oq2.SDPDate >= '{0}'", Convert.ToDateTime(this.cutoff1).ToString("yyyy/MM/dd")));
                            }

                            if (!MyUtility.Check.Empty(this.cutoff2))
                            {
                                sqlCmd.Append(string.Format(" and oq2.SDPDate <= '{0}'", Convert.ToDateTime(this.cutoff2).ToString("yyyy/MM/dd")));
                            }

                            sqlCmd.Append(")");
                        }

                        if (!MyUtility.Check.Empty(this.custRQS1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CRDDate >= '{0}'", Convert.ToDateTime(this.custRQS1).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.custRQS2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CRDDate <= '{0}'", Convert.ToDateTime(this.custRQS2).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.planDate1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.PlanDate >= '{0}'", Convert.ToDateTime(this.planDate1).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.planDate2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.PlanDate <= '{0}'", Convert.ToDateTime(this.planDate2).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.orderCfm1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CFMDate >= '{0}'", Convert.ToDateTime(this.orderCfm1).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.orderCfm2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CFMDate <= '{0}'", Convert.ToDateTime(this.orderCfm2).ToString("yyyy/MM/dd")));
                        }

                        if (!MyUtility.Check.Empty(this.style))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.StyleID = '{0}'", this.style));
                        }

                        if (!MyUtility.Check.Empty(this.season))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SeasonID = '{0}'", this.season));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custcd))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CustCDID = '{0}'", this.custcd));
                        }

                        if (!MyUtility.Check.Empty(this.mDivision))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.MDivisionID = '{0}'", this.mDivision));
                        }

                        if (!MyUtility.Check.Empty(this.factory))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
                        }

                        if (!this.hisOrder)
                        {
                            sqlcmd_sub.Append(" and o.Finished = 0");
                        }

                        if (this.bulk || this.sample || this.material || this.forecast || this.garment || this.smtl)
                        {
                            sqlcmd_sub.Append(" and (1=0");
                            if (this.bulk)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'B'");
                            }

                            if (this.sample)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'S'");
                            }

                            if (this.material)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'M'");
                            }

                            if (this.garment)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'G'");
                            }

                            if (this.smtl)
                            {
                                sqlcmd_sub.Append(" or o.Category = 'T'");
                            }

                            if (this.forecast)
                            {
                                sqlcmd_sub.Append(" or o.Category = ''");
                            }

                            sqlcmd_sub.Append(")");
                        }

                        sqlcmd_sub.Append($@" )
select  ot.ID
        , ot.ArtworkTypeID
        , ot.ArtworkUnit
        , ProductionUnit=p.PUnit
        , isnull(ot.Qty,0) Qty 
        , ot.TMS
        , isnull(ot.Price,0) Price
		, Supp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', (select Abb 
                                                                              from LocalSupp WITH (NOLOCK) 
                                                                              where ID = LocalSuppID)
                                                                           , ot.LocalSuppID)
                                                  , '')
        , PoSupp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', ap.Abb, ot.LocalSuppID) , '')
        , AUnitRno = a.rno 
        , PUnitRno = iif(ot.ArtworkTypeID='PRINTING PPU', a.rno , a1.rno)
        , NRno = a2.rno
        , TAUnitRno = a3.rno
        , TPUnitRno = iif(ot.ArtworkTypeID='PRINTING PPU', a3.rno, a4.rno )
        , TNRno = a5.rno  
from Order_TmsCost ot WITH (NOLOCK) 
left join ArtworkType at WITH (NOLOCK) on at.ID = ot.ArtworkTypeID
left join ArtworkData a on a.FakeID = ot.Seq+'U1' 
left join ArtworkData a1 on a1.FakeID = ot.Seq+'U2'
left join ArtworkData a2 on a2.FakeID = ot.Seq
left join ArtworkData a3 on a3.FakeID = 'T'+ot.Seq+'U1' 
left join ArtworkData a4 on a4.FakeID = 'T'+ot.Seq+'U2'
left join ArtworkData a5 on a5.FakeID = 'T'+ot.Seq 
outer apply(select  PUnit=iif('{this.checkByCPU.Checked}'='true' and at.ProductionUnit = 'TMS','CPU',at.ProductionUnit))p
outer apply(
	select Abb = Stuff((
			select distinct concat( ',', l.Abb)   
			from ArtworkPO ap WITH (NOLOCK)
			inner join ArtworkPO_Detail apd WITH (NOLOCK) on ap.ID = apd.ID
			left join LocalSupp l on ap.LocalSuppID = l.ID
			where ap.ArtworkTypeID = 'PRINTING'
			and apd.OrderID = ot.ID
		FOR XML PATH('')),1,1,'') 
)ap
where exists (select id from OrderID WITH (NOLOCK) where ot.ID = OrderID.ID )");
                        MyUtility.Tool.ProcessWithDatatable(
                            this.subprocessColumnName,
                            "ID,Seq,ArtworkUnit,ProductionUnit,SystemType,FakeID,ColumnN,ColumnSeq,rno",
                            sqlcmd_sub.ToString(),
                            out this.orderArtworkData);
                    }
                    catch (Exception ex)
                    {
                        DualResult failResult = new DualResult(false, "Query order tms & cost fail\r\n" + ex.ToString());
                    }
                    #endregion
                }

                if (this.printingDetail)
                {
                    string sqlprintingDetail = $@"
SELECT distinct
	oa.[ID]
	,[InkTypecolorsize] = concat(oa.InkType,'/',oa.Colors,' ','/',IIF(s.SmallLogo = 1,'Small logo','Big logo'))
	,PrintingLT = cast(plt.LeadTime + plt.AddLeadTime as float)
into #tmp2
FROM Order_Artwork oa WITH (NOLOCK)
outer apply(select SmallLogo = iif(oa.Width >= (select SmallLogoCM from system WITH (NOLOCK)) or oa.Length >= (select SmallLogoCM from system WITH (NOLOCK)), 0, 1) )s
inner join orders o WITH (NOLOCK) on o.id = oa.id
outer apply(select tmpRTL = IIF(o.Cpu = 0, 0, s.SewlineAvgCPU  / o.Cpu)  from System s WITH (NOLOCK))tr
outer apply(select RTLQty = iif(o.Qty < tmpRTL, o.Qty, tmpRTL))r
outer apply(select Colors = iif(isnull(oa.Colors,'')='', 0, oa.Colors))c
outer apply(select ex = iif(exists(select 1 from PrintLeadTime plt WITH (NOLOCK) where plt.InkType = oa.InkType), 1, 0))e
outer apply(select * from PrintLeadTime plt WITH (NOLOCK) where plt.InkType = oa.InkType and plt.SmallLogo = s.SmallLogo 
	and r.RTLQty between plt.RTLQtyLowerBound and plt.RTLQtyUpperBound
	and c.Colors between plt.ColorsLowerBound and plt.ColorsUpperBound
)pEx
outer apply(select * from PrintLeadTime plt WITH (NOLOCK) where plt.SmallLogo = s.SmallLogo and plt.IsDefault = 1
	and r.RTLQty between plt.RTLQtyLowerBound and plt.RTLQtyUpperBound
	and c.Colors between plt.ColorsLowerBound and plt.ColorsUpperBound
)pNEx
outer apply(
	select
		InkType = IIF(e.ex = 1, pEx.InkType, pnEx.InkType),
		LeadTime = IIF(e.ex = 1, pEx.LeadTime, pnEx.LeadTime),
		AddLeadTime = IIF(e.ex = 1, pEx.AddLeadTime, pnEx.AddLeadTime)
)plt
where oa.id in (select t.id from #tmp t)
and oa.ArtworkTypeID = 'Printing'

select *,rn = ROW_NUMBER() over(order by PrintingLT desc) into #tmp3 from #tmp2

select t.id,a.PrintingLT,b.InkTypecolorsize
from #tmp t
outer apply(
	select PrintingLT =   STUFF((
		select concat(',', t2.PrintingLT)
		from #tmp3 t2
		where t2.ID = t.id
		order by rn
		for xml path('')
	),1,1,'')
)a
outer apply(
	select [InkTypecolorsize] = STUFF((
		select concat(',', t2.[InkTypecolorsize])
		from #tmp3 t2
		where t2.ID = t.id
		order by rn
		for xml path('')
	),1,1,'')
)b

drop table #tmp,#tmp2,#tmp3
";
                    result = MyUtility.Tool.ProcessWithDatatable(this.printData, "id", sqlprintingDetail, out this.printingDetailDatas);
                    if (!result)
                    {
                        return result;
                    }
                }
            }

            DBProxy.Current.DefaultTimeout = 0;
            this.stdTMS = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select StdTMS from System WITH (NOLOCK) "));
            return Ict.Result.True;
        }

        // 最後一欄 , 有新增欄位要改這
        // 注意!新增欄位也要新增到StandardReport_Detail(Customized)。
        private int lastColA = 152;

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string strXltName = Env.Cfg.XltPathDir + "\\PPIC_R03_PPICMasterList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            int lastCol = this.lastColA;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Name = "PPIC_Master_List";

            // excel.Visible = true;

            // 填Subprocess欄位名稱
            int poSubConCol = 9999, subConCol = 9999, ttlTMS = lastCol + 1; // 紀錄SubCon與TTL_TMS的欄位
            int printingDetailCol = 9999;
            string excelColEng = string.Empty;
            if (this.artwork || this.pap)
            {
                foreach (DataRow dr in this.subprocessColumnName.Rows)
                {
                    worksheet.Cells[1, MyUtility.Convert.GetInt(dr["rno"])] = MyUtility.Convert.GetString(dr["ColumnN"]);
                    lastCol = MyUtility.Convert.GetInt(dr["rno"]);

                    if (this.printingDetail && MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "PRINTING LT")
                    {
                        printingDetailCol = MyUtility.Convert.GetInt(dr["rno"]);
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "POSUBCON")
                    {
                        poSubConCol = MyUtility.Convert.GetInt(dr["rno"]);
                        this.Subtrue = 1;
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "SUBCON")
                    {
                        subConCol = MyUtility.Convert.GetInt(dr["rno"]);
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "TTL_TMS")
                    {
                        ttlTMS = MyUtility.Convert.GetInt(dr["rno"]);
                    }
                }

                // 算出Excel的Column的英文位置
                excelColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(lastCol);
            }
            else
            {
                worksheet.Cells[1, ttlTMS] = "TTL_TMS";

                // 算出Excel的Column的英文位置
                excelColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(lastCol + 1);
            }

            // 填內容值
            int intRowsStart = 0;
            int maxRow = 0;
            int tRow = 10000;
            object[,] objArray = new object[tRow + 1, lastCol + 1];

            string kPIChangeReasonName;  // CLOUMN[CC]:dr["KPIChangeReason"]+dr["KPIChangeReasonName"]

            // Dictionary<string, DataRow> tmp_a = orderArtworkData.AsEnumerable().ToDictionary<DataRow, string, DataRow>(r => r["ID"].ToString(),r => r);
            if (this.orderArtworkData == null)
            {
                this.orderArtworkData = new DataTable();
                this.orderArtworkData.ColumnsStringAdd("ID");
            }

            var lookupID = this.orderArtworkData.AsEnumerable().ToLookup(row => row["ID"].ToString());
            excel.Cells.EntireColumn.AutoFit(); // 所有列最適列高
            foreach (DataRow dr in this.printData.Rows)
            {
                // EMBROIDERY 如果Qty price都是0該筆資料不show
                if (this.orderArtworkData.Rows.Count > 0 && !MyUtility.Check.Empty(this.subProcess))
                {
                    // DataRow[] find_subprocess = orderArtworkData.Select(string.Format("ID = '{0}' and ArtworkTypeID = '{1}' and (Price > 0 or Qty > 0)", MyUtility.Convert.GetString(dr["ID"]), subProcess));
                    var records = from record in lookupID[MyUtility.Convert.GetString(dr["ID"])]
                                  where record.Field<string>("ArtworkTypeID").ToUpper() == this.subProcess.ToUpper()
                                           && (record.Field<decimal>("Price") > 0 || record.Field<decimal>("Qty") > 0)
                                  select record;
                    if (records.Count() == 0)
                    {
                        continue;
                    }

                    records = null;
                }

                string key = Convert.ToDateTime(dr["SciDelivery"]).ToString("yyyyMM");
                if (Convert.ToDateTime(dr["SciDelivery"]).Day <= 7)
                {
                    key = Convert.ToDateTime(dr["SciDelivery"]).AddMonths(-1).ToString("yyyyMM");
                }

                #region 填固定欄位資料
                kPIChangeReasonName = dr["KPIChangeReason"].ToString().Trim() + "-" + dr["KPIChangeReasonName"].ToString().Trim();

                objArray[intRowsStart, 0] = dr["MDivisionID"];
                objArray[intRowsStart, 1] = dr["FactoryID"];
                objArray[intRowsStart, 2] = dr["BuyerDelivery"];
                objArray[intRowsStart, 3] = MyUtility.Check.Empty(dr["BuyerDelivery"]) ? string.Empty : Convert.ToDateTime(dr["BuyerDelivery"]).ToString("yyyyMM");
                objArray[intRowsStart, 4] = dr["EarliestSCIDlv"];
                objArray[intRowsStart, 5] = dr["SciDelivery"];
                objArray[intRowsStart, 6] = key;
                objArray[intRowsStart, 7] = dr["IDD"];
                objArray[intRowsStart, 8] = dr["CRDDate"];
                objArray[intRowsStart, 9] = MyUtility.Check.Empty(dr["CRDDate"]) ? string.Empty : Convert.ToDateTime(dr["CRDDate"]).ToString("yyyyMM");
                objArray[intRowsStart, 10] = MyUtility.Convert.GetDate(dr["BuyerDelivery"]) != MyUtility.Convert.GetDate(dr["CRDDate"]) ? "Y" : string.Empty;
                objArray[intRowsStart, 11] = dr["CFMDate"];
                objArray[intRowsStart, 12] = MyUtility.Check.Empty(dr["CRDDate"]) || MyUtility.Check.Empty(dr["CFMDate"]) ? 0 : Convert.ToInt32((Convert.ToDateTime(dr["CRDDate"]) - Convert.ToDateTime(dr["CFMDate"])).TotalDays);
                objArray[intRowsStart, 13] = dr["ID"];
                objArray[intRowsStart, 14] = dr["Category"];
                objArray[intRowsStart, 15] = MyUtility.Check.Empty(dr["isForecast"]) ? string.Empty : dr["BuyMonth"];
                objArray[intRowsStart, 16] = dr["BuyBack"];
                objArray[intRowsStart, 17] = MyUtility.Convert.GetString(dr["Junk"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 18] = dr["Cancelled"];
                objArray[intRowsStart, 19] = dr["DestAlias"];
                objArray[intRowsStart, 20] = dr["StyleID"];
                objArray[intRowsStart, 21] = dr["StyleName"];
                objArray[intRowsStart, 22] = dr["ModularParent"];
                objArray[intRowsStart, 23] = dr["CPUAdjusted"];
                objArray[intRowsStart, 24] = dr["SimilarStyle"];
                objArray[intRowsStart, 25] = dr["SeasonID"];
                objArray[intRowsStart, 26] = dr["GMTLT"];
                objArray[intRowsStart, 27] = dr["OrderTypeID"];
                objArray[intRowsStart, 28] = dr["ProjectID"];
                objArray[intRowsStart, 29] = dr["PackingMethod"];
                objArray[intRowsStart, 30] = dr["HangerPack"];
                objArray[intRowsStart, 31] = dr["Customize1"];
                objArray[intRowsStart, 32] = MyUtility.Check.Empty(dr["isForecast"]) ? dr["BuyMonth"] : string.Empty;
                objArray[intRowsStart, 33] = dr["CustPONo"];
                objArray[intRowsStart, 34] = MyUtility.Convert.GetString(dr["VasShas"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 35] = dr["MnorderApv2"];
                objArray[intRowsStart, 36] = dr["VasShasCutOffDate"];
                objArray[intRowsStart, 37] = dr["MnorderApv"];
                objArray[intRowsStart, 38] = dr["KpiMNotice"];
                objArray[intRowsStart, 39] = MyUtility.Convert.GetString(dr["TissuePaper"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 40] = dr["AirFreightByBrand"];
                objArray[intRowsStart, 41] = dr["FactoryDisclaimer"];
                objArray[intRowsStart, 42] = dr["FactoryDisclaimerRemark"];
                objArray[intRowsStart, 43] = dr["ApprovedRejectedDate"];
                objArray[intRowsStart, 44] = MyUtility.Convert.GetString(dr["GFR"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 45] = dr["BrandID"];
                objArray[intRowsStart, 46] = dr["CustCDID"];
                objArray[intRowsStart, 47] = dr["Kit"];
                objArray[intRowsStart, 48] = dr["BrandFTYCode"];
                objArray[intRowsStart, 49] = dr["ProgramID"];
                objArray[intRowsStart, 50] = dr["NonRevenue"];
                objArray[intRowsStart, 51] = dr["CdCodeID"];
                objArray[intRowsStart, 52] = dr["CDCodeNew"];
                objArray[intRowsStart, 53] = dr["ProductType"];
                objArray[intRowsStart, 54] = dr["FabricType"];
                objArray[intRowsStart, 55] = dr["Lining"];
                objArray[intRowsStart, 56] = dr["Gender"];
                objArray[intRowsStart, 57] = dr["Construction"];
                objArray[intRowsStart, 58] = dr["CPU"];
                objArray[intRowsStart, 59] = dr["Qty"];
                objArray[intRowsStart, 60] = dr["FOCQty"];
                objArray[intRowsStart, 61] = MyUtility.Convert.GetDecimal(dr["CPU"]) * MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPUFactor"]);
                objArray[intRowsStart, 62] = dr["SewQtyTop"];
                objArray[intRowsStart, 63] = dr["SewQtyBottom"];
                objArray[intRowsStart, 64] = dr["SewQtyInner"];
                objArray[intRowsStart, 65] = dr["SewQtyOuter"];
                objArray[intRowsStart, 66] = dr["TtlSewQty"];
                objArray[intRowsStart, 67] = dr["CutQty"];
                objArray[intRowsStart, 68] = MyUtility.Convert.GetString(dr["WorkType"]) == "1" ? "Y" : string.Empty;
                objArray[intRowsStart, 69] = MyUtility.Convert.GetDecimal(dr["CutQty"]) >= MyUtility.Convert.GetDecimal(dr["Qty"]) ? "Y" : string.Empty;
                objArray[intRowsStart, 70] = dr["PackingQty"];
                objArray[intRowsStart, 71] = dr["PackingFOCQty"];
                objArray[intRowsStart, 72] = dr["BookingQty"];
                objArray[intRowsStart, 73] = dr["FOCAdjQty"];
                objArray[intRowsStart, 74] = dr["NotFOCAdjQty"];
                objArray[intRowsStart, 75] = dr["PoPrice"];
                objArray[intRowsStart, 76] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["PoPrice"]);
                objArray[intRowsStart, 77] = dr["KPILETA"];  // BG
                objArray[intRowsStart, 78] = dr["PFETA"];
                objArray[intRowsStart, 79] = dr["PFRemark"];
                objArray[intRowsStart, 80] = dr["LETA"];
                objArray[intRowsStart, 81] = dr["MTLETA"];
                objArray[intRowsStart, 82] = dr["Fab_ETA"];
                objArray[intRowsStart, 83] = dr["Acc_ETA"];
                objArray[intRowsStart, 84] = dr["SewingMtlComplt"];
                objArray[intRowsStart, 85] = dr["PackingMtlComplt"];

                objArray[intRowsStart, 86] = dr["SewETA"];
                objArray[intRowsStart, 87] = dr["PackETA"];
                objArray[intRowsStart, 88] = MyUtility.Convert.GetString(dr["MTLDelay"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 89] = MyUtility.Check.Empty(dr["MTLExport"]) ? dr["MTLExportTimes"] : dr["MTLExport"];
                objArray[intRowsStart, 90] = MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "N";
                objArray[intRowsStart, 91] = dr["ArriveWHDate"];
                objArray[intRowsStart, 92] = dr["SewInLine"];
                objArray[intRowsStart, 93] = dr["SewOffLine"];
                objArray[intRowsStart, 94] = dr["FirstOutDate"];
                objArray[intRowsStart, 95] = dr["LastOutDate"]; // CA
                objArray[intRowsStart, 96] = dr["FirstProduction"]; // CB
                objArray[intRowsStart, 97] = dr["LastProductionDate"]; // CC
                objArray[intRowsStart, 98] = dr["EachConsApv"];
                objArray[intRowsStart, 99] = dr["KpiEachConsCheck"];
                objArray[intRowsStart, 100] = dr["CutInLine"];
                objArray[intRowsStart, 101] = dr["CutOffLine"];
                objArray[intRowsStart, 102] = dr["FirstCutDate"];
                objArray[intRowsStart, 103] = dr["LastCutDate"];
                objArray[intRowsStart, 104] = dr["PulloutDate"];
                objArray[intRowsStart, 105] = dr["ActPulloutDate"];
                objArray[intRowsStart, 106] = dr["PulloutQty"];
                objArray[intRowsStart, 107] = dr["ActPulloutTime"];
                objArray[intRowsStart, 108] = MyUtility.Convert.GetString(dr["PulloutComplete"]).ToUpper() == "TRUE" ? "OK" : string.Empty;
                objArray[intRowsStart, 109] = dr["FtyKPI"];
                objArray[intRowsStart, 110] = !MyUtility.Check.Empty(dr["KPIChangeReason"]) ? kPIChangeReasonName : string.Empty; // cc
                objArray[intRowsStart, 111] = dr["PlanDate"];
                objArray[intRowsStart, 112] = dr["OrigBuyerDelivery"];
                objArray[intRowsStart, 113] = dr["SMR"];
                objArray[intRowsStart, 114] = dr["SMRName"];
                objArray[intRowsStart, 115] = dr["MRHandle"];
                objArray[intRowsStart, 116] = dr["MRHandleName"];
                objArray[intRowsStart, 117] = dr["POSMR"];
                objArray[intRowsStart, 118] = dr["POSMRName"];
                objArray[intRowsStart, 119] = dr["POHandle"];
                objArray[intRowsStart, 120] = dr["POHandleName"];
                objArray[intRowsStart, 121] = dr["PCHandle"];
                objArray[intRowsStart, 122] = dr["PCHandleName"];
                objArray[intRowsStart, 123] = dr["MCHandle"];
                objArray[intRowsStart, 124] = dr["MCHandleName"];
                objArray[intRowsStart, 125] = dr["DoxType"];
                objArray[intRowsStart, 126] = dr["PackingCTN"];
                objArray[intRowsStart, 127] = dr["TotalCTN1"];
                objArray[intRowsStart, 128] = dr["PackErrorCtn"];
                objArray[intRowsStart, 129] = dr["FtyCtn1"];
                objArray[intRowsStart, 130] = dr["ClogCTN1"];
                objArray[intRowsStart, 131] = dr["CFACTN"];
                objArray[intRowsStart, 132] = dr["ClogRcvDate"];
                objArray[intRowsStart, 133] = dr["InspDate"];
                objArray[intRowsStart, 134] = dr["InspResult"];
                objArray[intRowsStart, 135] = dr["InspHandle"];
                objArray[intRowsStart, 136] = dr["SewLine"];
                objArray[intRowsStart, 137] = dr["ShipModeList"];
                objArray[intRowsStart, 138] = dr["Customize2"];
                objArray[intRowsStart, 139] = dr["Article"];
                objArray[intRowsStart, 140] = dr["SpecialMarkName"];
                objArray[intRowsStart, 141] = dr["FTYRemark"];
                objArray[intRowsStart, 142] = dr["SampleReasonName"];
                objArray[intRowsStart, 143] = dr["IsMixMarker"];
                objArray[intRowsStart, 144] = dr["CuttingSP"];
                objArray[intRowsStart, 145] = MyUtility.Convert.GetString(dr["RainwearTestPassed"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 146] = MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;
                objArray[intRowsStart, 147] = dr["LastCTNTransDate"];
                objArray[intRowsStart, 148] = dr["LastCTNRecdDate"];
                objArray[intRowsStart, 149] = dr["DryRoomRecdDate"];
                objArray[intRowsStart, 150] = dr["DryRoomTransDate"];
                objArray[intRowsStart, 151] = dr["MdRoomScanDate"];
                #endregion

                if (this.artwork || this.pap)
                {
                    var finRow = lookupID[dr["ID"].ToString()];
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

                            // TTL
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

                            if (poSubConCol != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["PoSupp"]))
                                {
                                    objArray[intRowsStart, poSubConCol - 1] = sdr["PoSupp"];
                                }

                                if (MyUtility.Check.Empty(objArray[intRowsStart, poSubConCol - 1]))
                                {
                                    objArray[intRowsStart, poSubConCol - 1] = string.Empty;
                                }
                            }

                            if (subConCol != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["Supp"]))
                                {
                                    objArray[intRowsStart, subConCol - 1] = sdr["Supp"];
                                }

                                if (MyUtility.Check.Empty(objArray[intRowsStart, subConCol - 1]))
                                {
                                    objArray[intRowsStart, subConCol - 1] = string.Empty;
                                }
                            }
                        }
                    }

                    objArray[intRowsStart, ttlTMS - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;

                    if (this.printingDetail)
                    {
                        DataRow pdr = this.printingDetailDatas.Select($"ID = '{dr["ID"]}'").FirstOrDefault();
                        if (pdr != null)
                        {
                            objArray[intRowsStart, printingDetailCol - 1] = pdr["PrintingLT"];
                            objArray[intRowsStart, printingDetailCol] = pdr["InkTypecolorsize"];
                        }
                    }
                }
                else
                {
                    objArray[intRowsStart, ttlTMS - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;
                }

                // 每一萬筆資料就先塞入excel並清空array
                switch (maxRow % tRow)
                {
                    case 0:
                        if (maxRow == 0)
                        {
                            intRowsStart++;
                            break;
                        }

                        // 空值給0
                        if (this.artwork || this.pap)
                        {
                            for (int j = 0; j < intRowsStart; j++)
                            {
                                for (int i = this.lastColA; i < lastCol; i++)
                                {
                                    if (objArray[j, i] == null)
                                    {
                                        objArray[j, i] = 0;
                                        if (i == poSubConCol - 1 || i == subConCol - 1)
                                        {
                                            objArray[j, i] = string.Empty;
                                        }
                                    }
                                }
                            }
                        }

                        worksheet.Range[string.Format("A{0}:{1}{2}", maxRow / tRow == 1 ? 2 : maxRow - tRow + 3, excelColEng, maxRow + 2)].Value2 = objArray;
                        intRowsStart = 0;
                        objArray = new object[tRow + 1, lastCol + 1];
                        break;
                    default:
                        intRowsStart++;
                        break;
                }

                maxRow++;
            }

            if (maxRow == 0)
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

            // 空值給0
            if (this.artwork || this.pap)
            {
                for (int j = 0; j < intRowsStart; j++)
                {
                    for (int i = this.lastColA; i < lastCol; i++)
                    {
                        if (objArray[j, i] == null)
                        {
                            objArray[j, i] = 0;
                            if (i == poSubConCol - 1 || i == subConCol - 1)
                            {
                                objArray[j, i] = string.Empty;
                            }
                        }
                    }
                }
            }

            worksheet.Range[string.Format("A{0}:{1}{0}", 1, excelColEng)].AutoFilter(1); // 篩選
            worksheet.Range[string.Format("A{0}:{1}{0}", 1, excelColEng)].Interior.Color = Color.FromArgb(191, 191, 191); // 底色
            worksheet.Range[string.Format("A{0}:{1}{2}", maxRow < tRow ? 2 : (maxRow / tRow * tRow) + 3, excelColEng, maxRow + 2)].Value2 = objArray;
            this.Subtrue = 0;
            this.CreateCustomizedExcel(ref worksheet);

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(maxRow);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R03_PPICMasterList");
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

        private void CheckIncludeArtworkdata_CheckedChanged(object sender, EventArgs e)
        {
            this.chkPrintingDetail.Enabled = this.checkIncludeArtworkdata.Checked;
            if (!this.checkIncludeArtworkdata.Checked)
            {
                this.chkPrintingDetail.Checked = false;
            }
        }
    }
}
