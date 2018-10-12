using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R03
    /// </summary>
    public partial class R03 : Sci.Win.Tems.PrintForm
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
            this.comboM.Text = Sci.Env.User.Keyword;
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.comboSubProcess.SelectedIndex = 0;
            this.checkBulk.Checked = true;
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
            #region 組SQL
            if (this.seperate && p_type.Equals("ALL"))
            {
                seperCmd = " ,oq.Seq";
            }

            seperCmdkpi = this.seperate ? "oq.FtyKPI" : "o.FtyKPI";
            seperCmdkpi2 = this.seperate ? " left join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id" : string.Empty;
            sqlCmd.Append(@"
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
            , o.Customize1
            , o.BuyMonth
            , o.CustPONo
            , o.CustCDID
            , o.ProgramID
            , o.CdCodeID
            , o.CPU
            , iif ((o.junk = 0 or o.junk is null), o.Qty,0) as Qty
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
            , DryCTN=isnull(o.DryCTN,0)
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
            , o.OrderTypeID
            , o.SpecialMark
            , o.SampleReason
            , o.InspDate
            , InspResult = IIF(o.InspResult='P','Pass',IIF(o.InspResult='F','Fail',''))
            , InspHandle = (o.InspHandle +'-'+ I.Name)
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
            , o.IsMixMarker
            , o.GFR "
            + seperCmd +
    @" from Orders o WITH (NOLOCK) 
   left join style s WITH (NOLOCK) on o.styleukey = s.ukey
   " + seperCmdkpi2 + @"
    OUTER APPLY(
        SELECT  Name 
        FROM Pass1 WITH (NOLOCK) 
        WHERE Pass1.ID = O.InspHandle
    )I
	outer apply(select oa.Article from Order_article oa WITH (NOLOCK) where oa.id = o.id)a
    where  1=1 ");
            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.cutoff1) || !MyUtility.Check.Empty(this.cutoff2))
            {
                sqlCmd.Append("and o.id in (select id from Order_QtyShip oq2 WITH (NOLOCK) where 1=1");
                if (!MyUtility.Check.Empty(this.cutoff1))
                {
                    sqlCmd.Append(string.Format(" and oq2.SDPDate >= '{0}'", Convert.ToDateTime(this.cutoff1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.cutoff2))
                {
                    sqlCmd.Append(string.Format(" and oq2.SDPDate <= '{0}'", Convert.ToDateTime(this.cutoff2).ToString("d")));
                }

                sqlCmd.Append(")");
            }

            if (!MyUtility.Check.Empty(this.custRQS1))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate >= '{0}'", Convert.ToDateTime(this.custRQS1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.custRQS2))
            {
                sqlCmd.Append(string.Format(" and o.CRDDate <= '{0}'", Convert.ToDateTime(this.custRQS2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.planDate1))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate >= '{0}'", Convert.ToDateTime(this.planDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.planDate2))
            {
                sqlCmd.Append(string.Format(" and o.PlanDate <= '{0}'", Convert.ToDateTime(this.planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.orderCfm1))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate >= '{0}'", Convert.ToDateTime(this.orderCfm1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.orderCfm2))
            {
                sqlCmd.Append(string.Format(" and o.CFMDate <= '{0}'", Convert.ToDateTime(this.orderCfm2).ToString("d")));
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
    inner join Style_TmsCost st on t.StyleUkey = st.StyleUkey
    where st.ArtworkTypeID = '{0}' AND (st.Qty>0 or st.TMS>0 and st.Price>0) ", this.subProcess));
            }

            if (this.poCombo)
            {
                if (this.seperate && p_type.Equals("ALL"))
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
            , s.StyleName
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
            , DryCTN=isnull(o.DryCTN,0)
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
            , o.OrderTypeID
            , o.SpecialMark
            , o.SampleReason
            , o.InspDate
            , InspResult = IIF(o.InspResult='P','Pass',IIF(o.InspResult='F','Fail',''))
            , InspHandle = (o.InspHandle +'-'+ I.Name)
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
            , o.IsMixMarker
            , o.GFR "
            + seperCmd +
    @"from Orders o  WITH (NOLOCK) 
    left join style s WITH (NOLOCK) on o.styleukey = s.ukey
   " + seperCmdkpi2 + @"
    OUTER APPLY (
        SELECT Name 
        FROM Pass1 WITH (NOLOCK) 
        WHERE Pass1.ID=O.InspHandle
    )I
    where o.POID IN (select distinct POID from tmpFilterSubProcess) 
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
            , t.DryCTN
            , t.CFACTN
    into #tmpFilterSeperate
    from #tmpListPoCombo t
    inner join Order_QtyShip oq WITH(NOLOCK) on t.ID = oq.Id and t.Seq = oq.Seq
    outer apply( select Sum( pd.CTNQty) PackingCTN ,
					Sum( case when p.Type in ('B', 'L') then pd.CTNQty else 0 end) TotalCTN,
					Sum( case when p.Type in ('B', 'L') and pd.TransferDate is null then pd.CTNQty else 0 end) FtyCtn,
					Sum(case when p.Type in ('B', 'L') and pd.ReceiveDate is not null then pd.CTNQty else 0 end) ClogCTN ,
					Sum(case when p.Type <> 'F'  then pd.ShipQty else 0 end) PackingQty ,
					Sum(case when p.Type = 'F'   then pd.ShipQty else 0 end) PackingFOCQty ,
					Sum(case when p.Type in ('B', 'L') and p.INVNo <> ''  then pd.ShipQty else 0 end) BookingQty ,
					Max (ReceiveDate) ClogRcvDate,
					MAX(p.PulloutDate)  ActPulloutDate
					from PackingList_Detail pd WITH (NOLOCK) 
                             LEFT JOIN PackingList p on pd.ID = p.ID 
                             where  pd.OrderID = t.ID 
                                    and pd.OrderShipmodeSeq = t.Seq   )  pdm;

CREATE NONCLUSTERED INDEX index_tmpFilterSeperate ON #tmpFilterSeperate(	ID ASC,seq asc);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_POID ON #tmpFilterSeperate(	POID ASC);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_CuttingSP ON #tmpFilterSeperate(	CuttingSP ASC);
CREATE NONCLUSTERED INDEX index_tmpFilterSeperate_StyleUkey ON #tmpFilterSeperate(	StyleUkey ASC);


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
        , som.SewQtyTop
        , som.SewQtyBottom
        , som.SewQtyInner
        , som.SewQtyOuter
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
        , InvoiceAdjQty = dbo.getInvAdjQty (t.ID, t.Seq) 
        , ct.LastCutDate
        , ArriveWHDate = (select Max(e.WhseArrival) 
                          from Export e WITH (NOLOCK) 
                          inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
                          where ed.POID = t.POID)
        , som.FirstOutDate
        , som.LastOutDate 
        , PulloutQty = isnull ((select Sum (pod.ShipQty) 
                                from Pullout_Detail pod WITH (NOLOCK) 
                                where pod.OrderID = t.ID 
                                      and pod.OrderShipmodeSeq = t.Seq)
                              , 0)
        , ActPulloutTime = (select Count (Distinct ID) 
                            from Pullout_Detail WITH (NOLOCK) 
                            where   OrderID = t.ID 
                                    and ShipQty > 0) 
        , Article = isnull ((select CONCAT(Article,',') 
                             from (select distinct Article 
                                   from Order_QtyShip_Detail WITH (NOLOCK) 
                                   where ID = t.ID and Seq = t.Seq
                             ) a for xml path(''))
                           , '')
, [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail where id=p.ID  and FabricType='F')
, [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail where id=p.ID  and FabricType='A')
from #tmpFilterSeperate t
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join PO p WITH (NOLOCK) on p.ID = t.POID
left join Country c WITH (NOLOCK) on c.ID = t.Dest
outer apply(select Sum( case when sod.ComboType = 'T'  then sod.QAQty else 0 end) SewQtyTop, 
				   Sum( case when sod.ComboType = 'B'  then sod.QAQty else 0 end) SewQtyBottom, 
				   Sum( case when sod.ComboType = 'I'  then sod.QAQty else 0 end) SewQtyInner, 
				   Sum( case when sod.ComboType = 'O'  then sod.QAQty else 0 end) SewQtyOuter,
				   Min (so.OutputDate) FirstOutDate,
				   Max (so.OutputDate) LastOutDate
					from SewingOutput so WITH (NOLOCK) 
                    inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
                    where sod.OrderID = t.ID) som
order by t.ID;
drop table #tmpListPoCombo;
drop table #tmpFilterSeperate;");
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
from #tmpListPoCombo t
left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
left join PO p WITH (NOLOCK) on p.ID = t.POID
left join Country c WITH (NOLOCK) on c.ID = t.Dest
order by t.ID;
drop table #tmpListPoCombo;");
            }
            #endregion

            return sqlCmd;
     }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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
            , ColumnN = 'SubCon'
            , ColumnSeq = '999'";

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
) a",
                        classify,
                        !this.artwork ? string.Empty : strUnion,
                        this.lastColA));
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
                        StringBuilder sqlcmd_sub = new StringBuilder();
                        sqlcmd_sub.Append(@"
with ArtworkData as (
    select * 
    from #tmp
),
OrderID as(
    select ID from orders O  WITH (NOLOCK)  where  1=1 ");

                        if (!MyUtility.Check.Empty(this.buyerDlv1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDlv1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.buyerDlv2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDlv2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.sciDlv1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDlv1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.sciDlv2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDlv2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.cutoff1) || !MyUtility.Check.Empty(this.cutoff2))
                        {
                            sqlCmd.Append("and o.id in (select id from Order_QtyShip oq2 WITH (NOLOCK) where 1=1");
                            if (!MyUtility.Check.Empty(this.cutoff1))
                            {
                                sqlCmd.Append(string.Format(" and oq2.SDPDate >= '{0}'", Convert.ToDateTime(this.cutoff1).ToString("d")));
                            }

                            if (!MyUtility.Check.Empty(this.cutoff2))
                            {
                                sqlCmd.Append(string.Format(" and oq2.SDPDate <= '{0}'", Convert.ToDateTime(this.cutoff2).ToString("d")));
                            }

                            sqlCmd.Append(")");
                        }

                        if (!MyUtility.Check.Empty(this.custRQS1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CRDDate >= '{0}'", Convert.ToDateTime(this.custRQS1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.custRQS2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CRDDate <= '{0}'", Convert.ToDateTime(this.custRQS2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.planDate1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.PlanDate >= '{0}'", Convert.ToDateTime(this.planDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.planDate2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.PlanDate <= '{0}'", Convert.ToDateTime(this.planDate2).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.orderCfm1))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CFMDate >= '{0}'", Convert.ToDateTime(this.orderCfm1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.orderCfm2))
                        {
                            sqlcmd_sub.Append(string.Format(" and o.CFMDate <= '{0}'", Convert.ToDateTime(this.orderCfm2).ToString("d")));
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
            }

            DBProxy.Current.DefaultTimeout = 0;
            this.stdTMS = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select StdTMS from System WITH (NOLOCK) "));
            return Result.True;
        }

        private int lastColA = 119; // 最後一欄 , 有新增欄位要改這
        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_R03_PPICMasterList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            int lastCol = lastColA;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Name = "PPIC_Master_List";
            excel.Visible = true;
            // 填Subprocess欄位名稱
            int subConCol = 9999, ttlTMS = lastCol + 1; // 紀錄SubCon與TTL_TMS的欄位
            string excelColEng = string.Empty;
            if (this.artwork || this.pap)
            {
                foreach (DataRow dr in this.subprocessColumnName.Rows)
                {
                    worksheet.Cells[1, MyUtility.Convert.GetInt(dr["rno"])] = MyUtility.Convert.GetString(dr["ColumnN"]);
                    lastCol = MyUtility.Convert.GetInt(dr["rno"]);
                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "SUBCON")
                    {
                        subConCol = MyUtility.Convert.GetInt(dr["rno"]);
                         this.Subtrue = 1;
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
            object[,] objArray = new object[this.printData.Rows.Count, lastCol + 1];

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
                                  where record.Field<string>("ArtworkTypeID") == this.subProcess
                                           && (record.Field<decimal>("Price") > 0 || record.Field<decimal>("Qty") > 0)
                                  select record;
                    if (records.Count() == 0)
                    {
                        continue;
                    }

                    records = null;
                }

                #region 填固定欄位資料
                objArray[intRowsStart, 0] = dr["MDivisionID"];
                objArray[intRowsStart, 1] = dr["FactoryID"];
                objArray[intRowsStart, 2] = dr["BuyerDelivery"];
                objArray[intRowsStart, 3] = MyUtility.Check.Empty(dr["BuyerDelivery"]) ? string.Empty : Convert.ToDateTime(dr["BuyerDelivery"]).ToString("yyyyMM");
                objArray[intRowsStart, 4] = dr["EarliestSCIDlv"];
                objArray[intRowsStart, 5] = dr["SciDelivery"];
                objArray[intRowsStart, 6] = dr["CRDDate"];
                objArray[intRowsStart, 7] = MyUtility.Check.Empty(dr["CRDDate"]) ? string.Empty : Convert.ToDateTime(dr["CRDDate"]).ToString("yyyyMM");
                objArray[intRowsStart, 8] = MyUtility.Convert.GetDate(dr["BuyerDelivery"]) != MyUtility.Convert.GetDate(dr["CRDDate"]) ? "Y" : string.Empty;
                objArray[intRowsStart, 9] = dr["CFMDate"];
                objArray[intRowsStart, 10] = MyUtility.Check.Empty(dr["CRDDate"]) || MyUtility.Check.Empty(dr["CFMDate"]) ? 0 : Convert.ToInt32((Convert.ToDateTime(dr["CRDDate"]) - Convert.ToDateTime(dr["CFMDate"])).TotalDays);
                objArray[intRowsStart, 11] = dr["ID"];
                objArray[intRowsStart, 12] = dr["Category"];
                objArray[intRowsStart, 13] = MyUtility.Convert.GetString(dr["Junk"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 14] = dr["DestAlias"];
                objArray[intRowsStart, 15] = dr["StyleID"];
                objArray[intRowsStart, 16] = dr["StyleName"];
                objArray[intRowsStart, 17] = dr["ModularParent"];
                objArray[intRowsStart, 18] = dr["CPUAdjusted"];
                objArray[intRowsStart, 19] = dr["SimilarStyle"];
                objArray[intRowsStart, 20] = dr["SeasonID"];
                objArray[intRowsStart, 21] = dr["GMTLT"];
                objArray[intRowsStart, 22] = dr["OrderTypeID"];
                objArray[intRowsStart, 23] = dr["ProjectID"];
                objArray[intRowsStart, 24] = dr["Customize1"];
                objArray[intRowsStart, 25] = dr["BuyMonth"];
                objArray[intRowsStart, 26] = dr["CustPONo"];
                objArray[intRowsStart, 27] = MyUtility.Convert.GetString(dr["VasShas"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 28] = dr["MnorderApv2"];
                objArray[intRowsStart, 29] = dr["MnorderApv"];
                objArray[intRowsStart, 30] = MyUtility.Convert.GetString(dr["TissuePaper"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 31] = dr["ExpectionForm"];
                objArray[intRowsStart, 32] = dr["ExpectionFormRemark"];
                objArray[intRowsStart, 33] = MyUtility.Convert.GetString(dr["GFR"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 34] = dr["BrandID"];
                objArray[intRowsStart, 35] = dr["CustCDID"];
                objArray[intRowsStart, 36] = dr["BrandFTYCode"];
                objArray[intRowsStart, 37] = dr["ProgramID"];
                objArray[intRowsStart, 38] = dr["CdCodeID"];
                objArray[intRowsStart, 39] = dr["CPU"];
                objArray[intRowsStart, 40] = dr["Qty"];
                objArray[intRowsStart, 41] = dr["FOCQty"];
                objArray[intRowsStart, 42] = MyUtility.Convert.GetDecimal(dr["CPU"]) * MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPUFactor"]);
                objArray[intRowsStart, 43] = dr["SewQtyTop"];
                objArray[intRowsStart, 44] = dr["SewQtyBottom"];
                objArray[intRowsStart, 45] = dr["SewQtyInner"];
                objArray[intRowsStart, 46] = dr["SewQtyOuter"];
                objArray[intRowsStart, 47] = dr["TtlSewQty"];
                objArray[intRowsStart, 48] = dr["CutQty"];
                objArray[intRowsStart, 49] = MyUtility.Convert.GetString(dr["WorkType"]) == "1" ? "Y" : string.Empty;
                objArray[intRowsStart, 50] = MyUtility.Convert.GetDecimal(dr["CutQty"]) >= MyUtility.Convert.GetDecimal(dr["Qty"]) ? "Y" : string.Empty;
                objArray[intRowsStart, 51] = dr["PackingQty"];
                objArray[intRowsStart, 52] = dr["PackingFOCQty"];
                objArray[intRowsStart, 53] = dr["BookingQty"];
                objArray[intRowsStart, 54] = dr["InvoiceAdjQty"];
                objArray[intRowsStart, 55] = dr["PoPrice"];
                objArray[intRowsStart, 56] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["PoPrice"]);
                objArray[intRowsStart, 57] = MyUtility.Convert.GetString(dr["LocalOrder"]).ToUpper() == "TRUE" ? dr["PoPrice"] : dr["CMPPrice"];
                objArray[intRowsStart, 58] = dr["KPILETA"];  // BE
                objArray[intRowsStart, 59] = dr["PFRemark"]; // BF
                objArray[intRowsStart, 60] = dr["LETA"];  // BG
                objArray[intRowsStart, 61] = dr["MTLETA"];  // BH
                objArray[intRowsStart, 62] = dr["Fab_ETA"];  // BI
                objArray[intRowsStart, 63] = dr["Acc_ETA"];  // BJ
                objArray[intRowsStart, 64] = dr["SewETA"];  // BK
                objArray[intRowsStart, 65] = dr["PackETA"];  // BL
                objArray[intRowsStart, 66] = MyUtility.Convert.GetString(dr["MTLDelay"]).ToUpper() == "TRUE" ? "Y" : string.Empty; // BM
                objArray[intRowsStart, 67] = MyUtility.Check.Empty(dr["MTLExport"]) ? dr["MTLExportTimes"] : dr["MTLExport"];
                objArray[intRowsStart, 68] = MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper();   // MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "";
                objArray[intRowsStart, 69] = dr["ArriveWHDate"];
                objArray[intRowsStart, 70] = dr["SewInLine"];
                objArray[intRowsStart, 71] = dr["SewOffLine"];
                objArray[intRowsStart, 72] = dr["FirstOutDate"];
                objArray[intRowsStart, 73] = dr["LastOutDate"];
                objArray[intRowsStart, 74] = dr["EachConsApv"];
                objArray[intRowsStart, 75] = dr["CutInLine"];
                objArray[intRowsStart, 76] = dr["CutOffLine"];
                objArray[intRowsStart, 77] = dr["FirstCutDate"];
                objArray[intRowsStart, 78] = dr["LastCutDate"];
                objArray[intRowsStart, 79] = dr["PulloutDate"];
                objArray[intRowsStart, 80] = dr["ActPulloutDate"];
                objArray[intRowsStart, 81] = dr["PulloutQty"];
                objArray[intRowsStart, 82] = dr["ActPulloutTime"];
                objArray[intRowsStart, 83] = MyUtility.Convert.GetString(dr["PulloutComplete"]).ToUpper() == "TRUE" ? "OK" : string.Empty;
                objArray[intRowsStart, 84] = dr["FtyKPI"];
                kPIChangeReasonName = dr["KPIChangeReason"].ToString().Trim() + "-" + dr["KPIChangeReasonName"].ToString().Trim();
                objArray[intRowsStart, 85] = !MyUtility.Check.Empty(dr["KPIChangeReason"]) ? kPIChangeReasonName : string.Empty; // cc
                objArray[intRowsStart, 86] = dr["PlanDate"];
                objArray[intRowsStart, 87] = dr["OrigBuyerDelivery"];
                objArray[intRowsStart, 88] = dr["SMR"];
                objArray[intRowsStart, 89] = dr["SMRName"];
                objArray[intRowsStart, 90] = dr["MRHandle"];
                objArray[intRowsStart, 91] = dr["MRHandleName"];
                objArray[intRowsStart, 92] = dr["POSMR"];
                objArray[intRowsStart, 93] = dr["POSMRName"];
                objArray[intRowsStart, 94] = dr["POHandle"];
                objArray[intRowsStart, 95] = dr["POHandleName"];
                objArray[intRowsStart, 96] = dr["MCHandle"];
                objArray[intRowsStart, 97] = dr["MCHandleName"];
                objArray[intRowsStart, 98] = dr["DoxType"];
                objArray[intRowsStart, 99] = dr["PackingCTN"];
                objArray[intRowsStart, 100] = dr["TotalCTN1"];
                objArray[intRowsStart, 101] = dr["DryCTN"];
                objArray[intRowsStart, 102] = dr["FtyCtn1"];
                objArray[intRowsStart, 103] = dr["ClogCTN1"];
                objArray[intRowsStart, 104] = dr["CFACTN"];
                objArray[intRowsStart, 105] = dr["ClogRcvDate"];
                objArray[intRowsStart, 106] = dr["InspDate"];
                objArray[intRowsStart, 107] = dr["InspResult"];
                objArray[intRowsStart, 108] = dr["InspHandle"];
                objArray[intRowsStart, 109] = dr["SewLine"];
                objArray[intRowsStart, 110] = dr["ShipModeList"];
                objArray[intRowsStart, 111] = dr["Article"];
                objArray[intRowsStart, 112] = dr["SpecialMarkName"];
                objArray[intRowsStart, 113] = dr["FTYRemark"];
                objArray[intRowsStart, 114] = dr["SampleReasonName"];
                objArray[intRowsStart, 115] = MyUtility.Convert.GetString(dr["IsMixMarker"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 116] = dr["CuttingSP"];
                objArray[intRowsStart, 117] = MyUtility.Convert.GetString(dr["RainwearTestPassed"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 118] = MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;
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

                            if (subConCol != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["Supp"]))
                                {
                                    objArray[intRowsStart, subConCol - 1] = sdr["Supp"];
                                }
                            }
                        }
                    }

                    objArray[intRowsStart, ttlTMS - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;
                }
                else
                {
                    objArray[intRowsStart, ttlTMS - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;
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
                        }
                    }
                }
            }

            worksheet.Range[string.Format("A{0}:{1}{0}", 1, excelColEng)].AutoFilter(1); // 篩選
            worksheet.Range[string.Format("A{0}:{1}{0}", 1, excelColEng)].Interior.Color = Color.FromArgb(191, 191, 191); // 底色
            worksheet.Range[string.Format("A{0}:{1}{2}", 2, excelColEng, intRowsStart + 1)].Value2 = objArray;
            this.Subtrue = 0;

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(intRowsStart);

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
