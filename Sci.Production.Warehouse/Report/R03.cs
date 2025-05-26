using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R03 : Win.Tems.PrintForm
    {
        private string season;
        private string mdivision;
        private string orderby;
        private string spno1;
        private string spno2;
        private string fabrictype;
        private string refno1;
        private string refno2;
        private string style;
        private string country;
        private string supp;
        private string factory;
        private string wkNo1;
        private string wkNo2;
        private string brand;
        private string IncludeJunk;
        private string ExcludeMaterial;
        private string dwr;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DateTime? suppDelivery1;
        private DateTime? suppDelivery2;
        private DateTime? eta1;
        private DateTime? eta2;
        private DateTime? ata1;
        private DateTime? ata2;
        private DataTable printData;

        /// <inheritdoc/>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            this.comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Supplier,SP#");
            this.comboOrderBy.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboDurable, 1, 1, "Include,Exclude,Only");
            this.comboDurable.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateSuppDelivery.Value1) && MyUtility.Check.Empty(this.dateSuppDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateETA.Value1) && MyUtility.Check.Empty(this.dateFinalETA.Value2) &&
                MyUtility.Check.Empty(this.dateFinalETA.Value1) && MyUtility.Check.Empty(this.dateETA.Value2) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)) &&
                (MyUtility.Check.Empty(this.txtRefnoStart.Text) && MyUtility.Check.Empty(this.txtRefnoEnd.Text)) &&
                (MyUtility.Check.Empty(this.txtWKNo1.Text) && MyUtility.Check.Empty(this.txtWKNo2.Text)))
            {
                MyUtility.Msg.WarningBox("< Supp Delivery > & < SCI Delivery > & < ETA > & < FinalETA >& < SP# > & < Refno > & < Wk# > can't be empty!!");
                return false;
            }
            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.suppDelivery1 = this.dateSuppDelivery.Value1;
            this.suppDelivery2 = this.dateSuppDelivery.Value2;
            this.eta1 = this.dateETA.Value1;
            this.eta2 = this.dateETA.Value2;
            this.ata1 = this.dateFinalETA.Value1;
            this.ata2 = this.dateFinalETA.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            this.refno1 = this.txtRefnoStart.Text;
            this.refno2 = this.txtRefnoEnd.Text;
            this.wkNo1 = this.txtWKNo1.Text;
            this.wkNo2 = this.txtWKNo2.Text;
            #endregion

            this.country = this.txtcountry.TextBox1.Text;
            this.supp = this.txtsupplier.TextBox1.Text;
            this.style = this.txtstyle.Text;
            this.season = this.txtseason.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.fabrictype = this.comboFabricType.SelectedValue.ToString();
            this.orderby = this.comboOrderBy.Text;
            this.dwr = this.comboDurable.Text;
            this.brand = this.txtbrand.Text;

            if (this.chkIncludeJunk.Checked)
            {
                this.IncludeJunk = Environment.NewLine;
            }
            else
            {
                this.IncludeJunk = " AND PSD.Junk=0 ";
            }

            if (this.chkExcludeMaterial.Checked)
            {
                this.ExcludeMaterial = " AND o.Category <> 'M' ";
            }
            else
            {
                this.ExcludeMaterial = Environment.NewLine;
            }

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlSeparateByWK = string.Empty;

            #region Separate By WK
            string sqlColSeparateByWK = string.Empty;
            string sqlJoinSeparateByWK = string.Empty;
            if (this.chkSeparateByWK.Checked)
            {
                sqlColSeparateByWK = @"
        ,[WKNo] = exd.ID
		,[WKETA] = ex.Eta
        ,[WKArriveW/HDate] = ex.WhseArrival
		,[WKShipQty] = exd.Qty
		,[WKFoc] = exd.Foc
";
                sqlJoinSeparateByWK = @"
left join Export_Detail exd with (nolock) on exd.POID = psd.id and exd.Seq1 = psd.SEQ1 and exd.Seq2 = psd.SEQ2
left join Export ex with (nolock) on ex.ID = exd.ID
";
            }
            #endregion
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            System.Data.SqlClient.SqlParameter sp_season = new System.Data.SqlClient.SqlParameter();
            sp_season.ParameterName = "@season";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@FactoryID";

            System.Data.SqlClient.SqlParameter sp_brand = new System.Data.SqlClient.SqlParameter();
            sp_brand.ParameterName = "@BrandID";

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter();
            sp_refno1.ParameterName = "@refno1";

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter();
            sp_refno2.ParameterName = "@refno2";

            System.Data.SqlClient.SqlParameter sp_wkno1 = new System.Data.SqlClient.SqlParameter();
            sp_wkno1.ParameterName = "@wkno1";

            System.Data.SqlClient.SqlParameter sp_wkno2 = new System.Data.SqlClient.SqlParameter();
            sp_wkno2.ParameterName = "@wkno2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            string where = string.Empty;

            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                where += $@" and '{Convert.ToDateTime(this.sciDelivery1).ToString("yyyy/MM/dd")}' <= o.SciDelivery ";
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                where += $@" and o.SciDelivery <= '{Convert.ToDateTime(this.sciDelivery2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.suppDelivery1) || !MyUtility.Check.Empty(this.suppDelivery2))
            {
                if (!MyUtility.Check.Empty(this.suppDelivery1))
                {
                   where += $@" and '{Convert.ToDateTime(this.suppDelivery1).ToString("yyyy/MM/dd")}' <= Coalesce(PSD.finaletd, PSD.CFMETD, PSD.SystemETD)";
                }

                if (!MyUtility.Check.Empty(this.suppDelivery2))
                {
                    where += $@" and Coalesce(PSD.finaletd, PSD.CFMETD, PSD.SystemETD) <= '{Convert.ToDateTime(this.suppDelivery2).ToString("yyyy/MM/dd")}'";
                }
            }

            if (!MyUtility.Check.Empty(this.eta1) || !MyUtility.Check.Empty(this.eta2))
            {
                if (!MyUtility.Check.Empty(this.eta1))
                {
                    where += $@" and '{Convert.ToDateTime(this.eta1).ToString("yyyy/MM/dd")}' <= PSD.ETA";
                }

                if (!MyUtility.Check.Empty(this.eta2))
                {
                    where += $@" and PSD.ETA <= '{Convert.ToDateTime(this.eta2).ToString("yyyy/MM/dd")}'";
                }
            }

            if (!MyUtility.Check.Empty(this.ata1) || !MyUtility.Check.Empty(this.ata2))
            {
                if (!MyUtility.Check.Empty(this.ata1))
                {
                    where += $@" and '{Convert.ToDateTime(this.ata1).ToString("yyyy/MM/dd")}' <= PSD.FinalETA";
                }

                if (!MyUtility.Check.Empty(this.ata2))
                {
                    where += $@" and  PSD.FinalETA <= '{Convert.ToDateTime(this.ata2).ToString("yyyy/MM/dd")}'";
                }
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                where += $@" and PSD.id >= '{this.spno1.PadRight(10, '0')}' and PSD.id <= '{this.spno2.PadRight(10, 'Z')}'";
            }

            if (!MyUtility.Check.Empty(this.refno1) && !MyUtility.Check.Empty(this.refno2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                where += $@" and PSD.refno >= '{this.refno1}' and PSD.refno <= '{this.refno2}'";
            }
            else if (!MyUtility.Check.Empty(this.refno1))
            {
                // 只輸入 Refno1
                where += $@" and PSD.refno like '{this.refno1}%'";
            }
            else if (!MyUtility.Check.Empty(this.refno2))
            {
                // 只輸入 Refno2
                where += $@" and PSD.refno like '{this.refno2}%'";
            }

            if (!MyUtility.Check.Empty(this.wkNo1) && !MyUtility.Check.Empty(this.wkNo2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                where += $@" and wk.wkno between '{this.wkNo1}' and '{this.wkNo2}' ";
            }
            else if (!MyUtility.Check.Empty(this.wkNo1))
            {
                // 只輸入 Refno1
                where += $" and wk.wkno like '{this.wkNo1}%'";
            }
            else if (!MyUtility.Check.Empty(this.wkNo2))
            {
                // 只輸入 Refno2
                where += $" and wk.wkno like '{this.wkNo2}%'";
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
--輔料們
--輔料不指定給某個色組
select distinct 
	ob.Id
	,psd.SEQ1
	,ob.SCIRefno 
	,ps.SuppID
	,Article.Article
	,Color.Color
    ,[FromColorCombo] = FromColorCombo.Color
into #tmpAccessory
from PO_Supp_Detail psd WITH (NOLOCK)
inner join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 =psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
inner join Order_BOA ob WITH (NOLOCK) on ob.Id = psd.ID and ob.SCIRefno = psd.SCIRefno
inner join Orders o WITH (NOLOCK) on o.ID = ob.ID
inner join Order_ColorCombo occ WITH (NOLOCK) on occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricPanelCode = ob.FabricPanelCode 
outer apply
(
	select wkno = stuff((
		select concat(char(10), ID)
		from Export_Detail with (nolock) 
		where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
		for xml path('')
	),1,1,'')
) Wk
outer apply 
(
	select Article = stuff((
		select DISTINCT ',' + Article
		from Order_ColorCombo occ WITH (NOLOCK) 
		where occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricPanelCode = ob.FabricPanelCode 
		for xml path('')
	),1,1,'')
) Article
outer apply
(
    select Color = stuff((
        select DISTINCT ',' + ColorID
        from Order_ColorCombo occ WITH (NOLOCK) 
        where occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricPanelCode = ob.FabricPanelCode 
        for xml path('')
    ),1,1,'')
) Color
outer apply 
(
	select Color = stuff((
		select ',' + occ2.ColorID
		from Order_ColorCombo occ WITH (NOLOCK) 
        inner join Order_ColorCombo occ2 WITH (NOLOCK) on occ.ID = occ2.ID and occ.Article = occ2.Article and occ2.PatternPanel = 'FA'
		where occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricPanelCode = ob.FabricPanelCode 
        group by occ2.Article, occ2.ColorID
        order by occ2.Article
		for xml path('')
	),1,1,'')
) FromColorCombo
where not exists (select 1 from Order_BOA_Article oba WITH (NOLOCK) where oba.Order_BoAUkey = ob.Ukey) --表示不指定
{where}
union
--輔料指定給某個色組
select distinct
	ob.Id
	,psd.SEQ1
	,ob.SCIRefno 
	,ps.SuppID
	,[Article] = Article.Value
	,[Color] = Color.Value
    ,[FromColorCombo] = FromColorCombo.Value
from PO_Supp_Detail psd WITH (NOLOCK)
inner join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 =psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
inner join Order_BOA ob WITH (NOLOCK) on psd.ID = ob.ID and ob.SCIRefno = psd.SCIRefno and psd.SEQ1 = ob.Seq1 --這邊紀錄一下，有指定色組的料要串SEQ1，不然會發
inner join Order_BOA_Article oba WITH (NOLOCK) on oba.Order_BoAUkey = ob.Ukey
inner join Orders o WITH (NOLOCK) on o.ID = ob.ID
outer apply
(
	select wkno = stuff((
		select concat(char(10),ID)
		from Export_Detail with (nolock) 
		where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
		for xml path('')
	),1,1,'')
) Wk
outer apply 
(
	select Value = stuff((
		select DISTINCT ',' + Article
		from Order_BOA_Article oba WITH (NOLOCK) 
		where oba.Order_BoAUkey = ob.Ukey
		for xml path('')
	),1,1,'')
) Article
outer apply
(
    select Value = stuff((
        select DISTINCT ',' + occ.ColorID
        from Order_ColorCombo occ WITH (NOLOCK) 
        where occ.id = ob.id and occ.ColorID = psdsc.SpecValue and occ.FabricPanelCode = ob.FabricPanelCode 
        for xml path('')
    ),1,1,'')
) Color
outer apply 
(
	select Value = stuff((
		select ',' + occ.ColorID
		from Order_BOA_Article oba WITH (NOLOCK) 
        inner join Order_ColorCombo occ WITH (NOLOCK) on occ.id = oba.id and occ.Article = oba.Article and occ.PatternPanel = 'FA'
        where oba.Order_BoAUkey = ob.Ukey
        group by occ.Article, occ.ColorID
        order by occ.Article
		for xml path('')
	),1,1,'')
) FromColorCombo
where 1=1
{where}

------------------------------------------------------------------------------
--主料
select distinct 
	ob.Id 
	,ob.SCIRefno 
	,ob.SuppID
	,Article.Article
	,Color.Color
    ,[FromColorCombo] = FromColorCombo.Color
into #tmpFabric
from PO_Supp_Detail psd WITH (NOLOCK) 
inner join Order_BOF ob WITH (NOLOCK) on ob.ID = psd.id and ob.SCIRefno = psd.SCIRefno
inner join Orders o WITH (NOLOCK) on o.ID = ob.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 =psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
inner join Order_ColorCombo occ WITH (NOLOCK) on occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricCode = ob.FabricCode 
outer apply(
	select wkno = stuff((
		select concat(char(10),ID)
		from Export_Detail WITH (NOLOCK)
		where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
		for xml path('')
	),1,1,'')
) Wk
outer apply 
(
	select Article = stuff((
		select DISTINCT ',' + Article
		from Order_ColorCombo occ WITH (NOLOCK) 
		where occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricCode = ob.FabricCode 
		for xml path('')
	),1,1,'')
) Article
outer apply
(
    select Color = stuff((
        select DISTINCT ',' + ColorID
        from Order_ColorCombo occ WITH (NOLOCK) 
        where occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricCode = ob.FabricCode 
        for xml path('')
    ),1,1,'')
) Color
outer apply 
(
	select Color = stuff((
		select ',' + occ2.ColorID
		from Order_ColorCombo occ WITH (NOLOCK) 
        inner join Order_ColorCombo occ2 WITH (NOLOCK) on occ.ID = occ2.ID and occ.Article = occ2.Article and occ2.PatternPanel = 'FA'
		where occ.id = ob.id and occ.ColorID = psdsC.SpecValue and occ.FabricCode = ob.FabricCode 
        group by occ2.Article, occ2.ColorID
        order by occ2.Article
		for xml path('')
	),1,1,'')
) FromColorCombo
where 1=1
{where}

------------------------------------------------------------------
--線
select distinct 
	o.Id
	,psd.SEQ1
	,psd.SEQ2
	,tccd.SCIRefno 
	,ps.SuppID
	,Article.Article
	,Color.Color
    ,[FromColorCombo] = FromColorCombo.Color
into #tmpThread
from PO_Supp_Detail psd WITH (NOLOCK)
Inner Join Orders o WITH (NOLOCK) on o.ID = psd.ID
inner join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 =psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
Inner Join Style_ThreadColorCombo tcc WITH (NOLOCK) on tcc.StyleUkey = o.StyleUkey 
Inner Join dbo.Style_ThreadColorCombo_Detail as tccd WITH (NOLOCK) On tccd.Style_ThreadColorComboUkey = tcc.Ukey and tccd.SCIRefNo = psd.SCIRefno and tccd.ColorID = psdsC.SpecValue
outer apply
(
	select wkno = stuff((
		select concat(char(10), ID)
		from Export_Detail with (nolock) 
		where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
		for xml path('')
	),1,1,'')
) Wk
outer apply 
(
	select Article = stuff((
		select DISTINCT ',' + Article
		from Style_ThreadColorCombo tcc WITH (NOLOCK) 
		Inner Join dbo.Style_ThreadColorCombo_Detail as tccd2 WITH (NOLOCK) On tccd2.Style_ThreadColorComboUkey = tcc.Ukey and colorid = psdsC.SpecValue
		Inner Join Orders o2 WITH (NOLOCK) on o2.styleukey = tcc.StyleUkey 
		where o2.ID = o.ID
		and tccd2.SCIRefNo = tccd.SCIRefNo
		for xml path('')
	),1,1,'')
) Article
outer apply 
(
	select Color = stuff((
		select DISTINCT ',' + tccd2.ColorID 
		from Style_ThreadColorCombo tcc2 WITH (NOLOCK) 
		Inner Join dbo.Style_ThreadColorCombo_Detail as tccd2 with(nolock) On tccd2.Style_ThreadColorComboUkey = tcc2.Ukey and colorid = psdsC.SpecValue
		Inner Join Orders o2 WITH (NOLOCK) on o2.styleukey = tcc2.StyleUkey 
		where o2.ID = o.ID
		and tccd2.SCIRefNo = tccd.SCIRefNo
		for xml path('')
	),1,1,'')
) Color
outer apply 
(
	select Color = stuff((
		select ',' + oc.ColorID 
		from Style_ThreadColorCombo tcc2 WITH (NOLOCK) 
		Inner Join dbo.Style_ThreadColorCombo_Detail as tccd2 with(nolock) On tccd2.Style_ThreadColorComboUkey = tcc2.Ukey and colorid = psdsC.SpecValue
		Inner Join Orders o2 WITH (NOLOCK) on o2.styleukey = tcc2.StyleUkey 
		Inner Join Order_ColorCombo oc WITH (NOLOCK) ON o2.POID = oc.ID AND tccd2.Article = oc.Article and oc.PatternPanel = 'FA'
		where o2.ID = o.ID
		and tccd2.SCIRefNo = tccd.SCIRefNo
        group by oc.Article, oc.ColorID 
        order by oc.Article
		for xml path('')
	),1,1,'')
) FromColorCombo
where 1=1
{where}
------------------------------------------------------------------------------

select  f.MDivisionID
        ,o.FactoryID
        ,[Wkno] = wk.wkno
        ,[Season] = o.SeasonID
        ,PS.id
        ,style = si.StyleID
		,o.BrandID
        ,PSD.FinalETD
		,[ActETA]=PSD.FinalETA
		,[Sup Delivery Rvsd ETA]=PSD.RevisedETA
		,[Category]=o.Category
        ,supp = concat(PS.suppid,'-',S.NameEN )
        ,S.CountryID
        ,[FabricCombo] = EachCons.FabricCombo
        ,PSD.Refno
        ,isnull(psdsS.SpecValue, '')
        ,Fabric.WeaveTypeID
        ,mtl.ProductionType
        ,PSD.SEQ1
        ,PSD.SEQ2
        ,fabrictype = (case PSD.fabrictype 
                        when 'F' then 'Fabric'
                        when 'A' then 'Accessory'
                        when 'O' then 'Other'
						else PSD.FabricType 
						end) + '-' + Fabric.MtlTypeID
		,ds5.string
        ,[Material Color] = iif(Fabric.MtlTypeID in ('EMB Thread', 'SP Thread', 'Thread') 
                , IIF(isnull(PSD.SuppColor,'') = '',dbo.GetColorMultipleID(o.BrandID, psdsC.SpecValue),PSD.SuppColor)
                , dbo.GetColorMultipleID(o.BrandID, psdsC.SpecValue))
		,[Article] = COALESCE(acc.Article, fab.Article, thread.Article)
		,[Color] =  COALESCE(acc.FromColorCombo, fab.FromColorCombo, thread.FromColorCombo)
        ,PSD.Qty
        ,PSD.NETQty
        ,[LossQty] = PSD.NETQty+PSD.LossQty
        ,PSD.ShipQty
        ,PSD.FOC
        ,PSD.ShipFOC
        ,PSD.ApQty
        ,PSD.InputQty
        ,[Scrap Qty]= isnull(MDPD.LObQty,0)
        ,PSD.POUnit
        ,[Complete] = iif(PSD.Complete=1,'Y','N')
        ,OrderList = POSupp_OrderList.Val
        ,MDPD.InQty
        ,PSD.StockUnit
        ,MDPD.OutQty
        ,MDPD.AdjustQty
        ,MDPD.ReturnQty
        ,MDPD.InQty - MDPD.OutQty + MDPD.AdjustQty - MDPD.ReturnQty balance
        ,MDPD.ALocation
        ,MDPD.BLocation
        ,Fabric.InspectionGroup
        ,[FabricType] = case PSD.FabricType 
							when 'F' then FT.F
							when 'A' then FT2.A
						 end
        ,o.KPILETA
        ,[MCHandle] = dbo.GetPass1(o.MCHandle)
        {sqlColSeparateByWK}
from dbo.PO_Supp_Detail PSD WITH (NOLOCK) 
join dbo.PO_Supp PS WITH (NOLOCK) on PSD.id = PS.id and PSD.Seq1 = PS.Seq1
join dbo.Supp S WITH (NOLOCK)  on S.id = PS.SuppID
join dbo.Orders o WITH (NOLOCK)  on o.id = PSD.id
join dbo.Factory f WITH (NOLOCK) on f.id = o.FactoryId
left join dbo.MDivisionPoDetail MDPD WITH (NOLOCK)  on MDPD.POID = PSD.ID and MDPD.Seq1 = PSD.Seq1 and MDPD.Seq2 = PSD.Seq2
left join dbo.Fabric WITH (NOLOCK) on fabric.SciRefno = psd.SciRefno
left join dbo.MtlType mtl WITH (NOLOCK)  on mtl.ID = fabric.MtlTypeID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
{sqlJoinSeparateByWK}
OUTER APPLY(
    SELECT [FabricCombo]=STUFF((
        SELECT 
        (
            SELECT DISTINCT ','+ oec.FabricCombo 
            From Order_EachCons oec
            Inner JOIN Order_BOF BOF with(nolock) ON oec.FabricCode = BOF.FabricCode AND oec.ID = BOF.Id
            Where bof.SCIRefno = psd.SciRefno 
            and bof.ID = o.ID
            FOR XML PATH('')
        ))
     ,1,1,'')
)EachCons
outer apply(select StyleID from dbo.orders WITH (NOLOCK) where id = PS.id) si
outer apply
(
	select Val = 
	stuff((
		select concat(',',OrderID)
        from DBO.PO_Supp_Detail_OrderList WITH (NOLOCK) 
        where id=PSD.id and seq1=PSD.seq1 and seq2 = PSD.SEQ2
        for xml path('')
	 ),1,1,'')
)POSupp_OrderList
outer apply
(
	select F = 
	stuff((
		select concat('/',iif(t3.result='P','Pass',iif(t3.result='F','Fail',t3.Result)))         
        from dbo.AIR t3 WITH (NOLOCK) 
        where t3.POID = PSD.ID and t3.seq1 = PSD.seq1 and t3.seq2 = PSD.seq2
        for xml path('')
	),1,1,'')
)FT
outer apply
(
	select A = 
	stuff((
		select concat('/',x.result )
        from (select result = iif(t2.result='P','Pass',iif(t2.result='F','Fail',t2.Result)) 
                from dbo.FIR t2 WITH (NOLOCK) 
                where t2.POID = PSD.ID and t2.seq1 = PSD.seq1 and t2.seq2 = PSD.seq2) x 
        for xml path('')
	 ),1,1,'')
)FT2
outer apply
(
	SELECT p.SCIRefno
		, p.Refno
		, suppcolor = Concat(iif(ISNULL(p.SuppColor,'') = '', '', p.SuppColor)
							,iif(ISNULL(p.SuppColor,'') != '' and ISNULL(psdsC.SpecValue,'') != '',CHAR(10),'')
						    ,iif(ISNULL(psdsC.SpecValue,'') = '', '', psdsC.SpecValue + ' - ') 
							,c.Name)
		, StockSP = concat(iif(isnull(p.StockPOID,'')='','',p.StockPOID+' ')
						  ,iif(isnull(p.StockSeq1,'')='','',p.StockSeq1+' ')
						  ,p.StockSeq2)
		, po_desc= concat(iif(ISNULL(p.ColorDetail,'') = '', '', 'ColorDetail : ' + p.ColorDetail)
						 ,iif(ISNULL(psdsS.SpecValue,'') = '', '', psdsS.SpecValue + ' ')
						 ,psdsU.SpecValue
						 ,p.Special
						 ,p.Spec
						 ,p.Remark)
		, Spec = iif(stockPO3.Spec is null,p.Spec ,stockPO3.Spec)
		, fabric_detaildesc = f.DescDetail
		, zn.ZipperName
	from dbo.PO_Supp_Detail p WITH (NOLOCK)
	left join fabric f WITH (NOLOCK) on p.SCIRefno = f.SCIRefno
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = p.id and psdsC.seq1 = p.seq1 and psdsC.seq2 = p.seq2 and psdsC.SpecColumnID = 'Color'
    left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = p.id and psdsS.seq1 = p.seq1 and psdsS.seq2 = p.seq2 and psdsS.SpecColumnID = 'Size'
    left join PO_Supp_Detail_Spec psdsU WITH (NOLOCK) on psdsU.ID = p.id and psdsU.seq1 = p.seq1 and psdsU.seq2 = p.seq2 and psdsU.SpecColumnID = 'SizeUnit'
    left join PO_Supp_Detail_Spec psdsZ WITH (NOLOCK) on psdsZ.ID = p.id and psdsZ.seq1 = p.seq1 and psdsZ.seq2 = p.seq2 and psdsZ.SpecColumnID = 'ZipperInsert'
    left join PO_Supp_Detail stockPO3 WITH (NOLOCK) on stockPO3.ID = p.StockPOID and stockPO3.seq1 = p.StockSeq1 and stockPO3.seq2 = p.StockSeq2
    left join PO_Supp_Detail_Spec psdsZstockPO3 WITH (NOLOCK) on psdsZstockPO3.ID = p.StockPOID and psdsZstockPO3.seq1 = p.StockSeq1 and psdsZstockPO3.seq2 = p.StockSeq2 and psdsZstockPO3.SpecColumnID = 'ZipperInsert'
	left join Color c WITH (NOLOCK) on f.BrandID = c.BrandId and psdsC.SpecValue = c.ID 
	outer apply
	(
		Select ZipperName = DropDownList.Name
		From Production.dbo.DropDownList WITH (NOLOCK)
		Where Type = 'Zipper' And ID = iif(psdsZstockPO3.SpecValue is null,psdsZ.SpecValue ,psdsZstockPO3.SpecValue)
	)zn
	WHERE p.ID=PSD.id and p.seq1 = PSD.seq1 and p.seq2 = PSD.seq2
)ds
outer apply
(
	select string = 
	concat(iif(isnull(ds.fabric_detaildesc,'')='','',ds.fabric_detaildesc+CHAR(10))
		  ,iif(isnull(ds.suppcolor,'')='','',ds.suppcolor+CHAR(10))
		  ,replace(ds.po_desc,char(10),'')
		  )
)ds2
outer apply
(
	select string = iif(left(PSD.seq1,1) = '7'
					,concat('**PLS USE STOCK FROM SP#:', ds.StockSP, '**', iif(isnull(ds2.string,'')='', '', CHAR(10) + ds2.string))
					,ds2.string)
)ds3
outer apply(select string=concat(iif(isnull(ds3.string,'')='','',ds3.string+CHAR(10)),IIF(IsNull(ds.ZipperName,'') = '','','Spec:'+ ds.ZipperName+Char(10)),RTrim(ds.Spec)))ds4
outer apply(select string=replace(replace(replace(replace(ds4.string,char(13),char(10)),char(10)+char(10),char(10)),char(10)+char(10),char(10)),char(10)+char(10),char(10)))ds5
outer apply(
select wkno = stuff((
	    select concat(char(10),ID)
	    from Export_Detail with (nolock) 
	    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	    for xml path('')
	),1,1,'')
)Wk
left join #tmpAccessory acc on acc.id = PSD.ID and acc.scirefno = PSD.sciRefno and acc.seq1 = psd.Seq1 and acc.Color = psdsC.SpecValue and acc.SuppID = ps.SuppID and psd.FabricType = 'A' and PSD.SEQ1 not like 'T%' 
left join #tmpFabric fab on fab.id = PSD.ID and fab.Color = psdsC.SpecValue and fab.scirefno = PSD.sciRefno and psd.FabricType = 'F' 
left join #tmpThread thread on thread.id = PSD.ID and thread.scirefno = PSD.sciRefno and thread.SuppID = ps.SuppID and thread.Color = psdsC.SpecValue
Where 1=1
{where}
");

            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(" and o.styleid = @style");
                sp_style.Value = this.style;
                cmds.Add(sp_style);
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(" and o.seasonid = @season");
                sp_season.Value = this.season;
                cmds.Add(sp_season);
            }

            if (!MyUtility.Check.Empty(this.country))
            {
                sqlCmd.Append(string.Format(" and s.countryID = '{0}'", this.country));
            }

            if (!MyUtility.Check.Empty(this.supp))
            {
                sqlCmd.Append(string.Format(" and PS.suppid = '{0}'", this.supp));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and f.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and o.FactoryID = @FactoryID");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(" and o.BrandID = @BrandID");
                sp_brand.Value = this.brand;
                cmds.Add(sp_brand);
            }

            if (!MyUtility.Check.Empty(this.fabrictype))
            {
                sqlCmd.Append(string.Format(@" and PSD.FabricType = '{0}'", this.fabrictype));
            }

            if (this.dwr == "Exclude")
            {
                sqlCmd.Append($@" and fabric.DWR = 0");
            }
            else if (this.dwr == "Only")
            {
                sqlCmd.Append($@" and fabric.DWR = 1");
            }

            if (this.chkWhseClose.Checked)
            {
                sqlCmd.Append(" and o.WhseClose is null");
            }

            sqlCmd.Append(this.IncludeJunk + Environment.NewLine);
            sqlCmd.Append(this.ExcludeMaterial + Environment.NewLine);
            sqlCmd.Append("and f.IsProduceFty = 1" + Environment.NewLine);

            if (this.orderby.ToUpper().TrimEnd() == "SUPPLIER")
            {
                sqlCmd.Append(" ORDER BY PS.SUPPID, PSD.ID, PSD.SEQ1, PSD.SEQ2 ");
            }
            else
            {
                sqlCmd.Append(" ORDER BY PSD.ID, PSD.SEQ1, PSD.SEQ2 ");
            }

            #endregion

            sqlCmd.Append(" drop table #tmpAccessory, #tmpFabric,#tmpThread");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R03.xltx"); // 預先開啟excel app
            this.ShowWaitMessage("Excel Processing...");
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_R03.xltx", objApp);

            // com.TransferArray_Limit = 200000;
            com.ColumnsAutoFit = true;
            com.WriteTable(this.printData, 2);

            if (this.chkSeparateByWK.Checked)
            {
                objApp.Sheets[1].Cells[1, 48].Value = "WK No.";
                objApp.Sheets[1].Cells[1, 49].Value = "WK ETA";
                objApp.Sheets[1].Cells[1, 50].Value = "WK Arrive W/H Date";
                objApp.Sheets[1].Cells[1, 51].Value = "WK ShipQty";
                objApp.Sheets[1].Cells[1, 52].Value = "WK F.O.C";
            }
            else
            {
                for (int colIndex = 52; colIndex >= 48; colIndex--)
                {
                    Excel.Range column = objApp.Columns[colIndex];
                    column.Delete();
                }
            }

            // Excel.Worksheet worksheet = objApp.Sheets[1];

            // for (int i = 1; i <= printData.Rows.Count; i++)
            // {
            //    string str = worksheet.Cells[i + 1, 12].Value;
            //    if (!MyUtility.Check.Empty(str))
            //        worksheet.Cells[i + 1, 12] = str.Trim();
            // }
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R03");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            // Marshal.ReleaseComObject(worksheet);
            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
