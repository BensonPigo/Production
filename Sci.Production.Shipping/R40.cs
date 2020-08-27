using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R40
    /// </summary>
    public partial class R40 : Win.Tems.PrintForm
    {
        private string contract;
        private string hscode;
        private string nlcode;
        private List<string> FactoryList;
        private bool liguidationonly;
        private DataTable Summary;
        private DataTable OnRoadMaterial;
        private DataTable WHDetail;
        private DataTable WIPDetail;
        private DataTable ProdDetail;
        private DataTable OnRoadProduction;
        private DataTable ScrapDetail;
        private DataTable Outstanding;
        private DataTable WarehouseNotClose;
        private DataTable AlreadySewingOutput;
        private DataTable dtImportEcusData = new DataTable();
        private string strGenerateDate;

        /// <summary>
        /// R40
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.checkLiquidationDataOnly.Checked = true;
            this.dtImportEcusData.Columns.Add("NLCode", typeof(string));
            this.dtImportEcusData.Columns.Add("StockQty", typeof(decimal));
            this.dateGenerate.Value = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.txtContractNo.Text))
            {
                this.txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }

            this.contract = this.txtContractNo.Text;
            this.hscode = this.txtHSCode.Text;
            this.nlcode = this.txtNLCode.Text;
            this.liguidationonly = this.checkLiquidationDataOnly.Checked;
            this.strGenerateDate = ((DateTime)this.dateGenerate.Value).ToString("yyyy/MM/dd");

            #region import Ecus Qty
            DialogResult importResult = DialogResult.Cancel;

            if (!this.liguidationonly)
            {
                importResult = new R40_SelectFileDialog().ShowDialog();
            }

            if (importResult == DialogResult.OK)
            {
                this.dtImportEcusData.Clear();
                string excelFile = MyUtility.File.GetFile("Excel files|*.xls;*.xlsx");

                if (MyUtility.Check.Empty(excelFile))
                {
                    return false;
                }

                Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
                if (excel == null)
                {
                    return false;
                }

                this.ShowWaitMessage("Starting Import EXCEL...");

                excel.Visible = false;
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                int intRowsCount = worksheet.UsedRange.Rows.Count + 6;
                int intRowsStart = 3;
                int intRowsRead = intRowsStart - 1;

                Excel.Range range;
                object[,] objCellArray;

                while (intRowsRead < intRowsCount)
                {
                    intRowsRead++;
                    range = worksheet.Range[string.Format("A{0}:I{0}", intRowsRead)];
                    objCellArray = range.Value;
                    string nLCode = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C"));
                    var stockQty = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "N");
                    if (MyUtility.Check.Empty(nLCode) || MyUtility.Check.Empty(stockQty))
                    {
                        continue;
                    }

                    DataRow drEcusData = this.dtImportEcusData.NewRow();
                    drEcusData["NLCode"] = nLCode;
                    drEcusData["StockQty"] = stockQty;
                    this.dtImportEcusData.Rows.Add(drEcusData);
                }

                excel.Workbooks.Close();
                excel.Quit();
                excel = null;

                this.HideWaitMessage();
            }
            #endregion

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 先取得合約(VNContract)的所有工廠Factory
            // FactoryList
            string sqlcmdF = $@"select FactoryID from VNContract_Factory With(nolock) where VNContractID = '{this.contract}'";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmdF, out dt);
            string whereftys = string.Empty;
            if (!result)
            {
                return result;
            }
            else
            {
                this.FactoryList = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["FactoryID"])).ToList();
                whereftys = "and o.FactoryID in ('" + string.Join("','", this.FactoryList) + "')";
            }
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            /*
             * VNContract_Detail.Qty + sum(import數量) - sum(export數量) + sum(adjust數量)
             * export 在 SQL 帶負數
             * import & export Status = confirm
             * adjust Status != new
             */
            sqlCmd.Append(string.Format(
                @"
DECLARE @contract VARCHAR(15)
		,@mdivision VARCHAR(8)
        ,@GenerateDate date = null
SET @contract = '{0}';
SET @mdivision = '{1}';
SET @GenerateDate = '{2}'

--撈合約資料
select 	HSCode
		,NLCode
		,Qty
		,UnitID 
into #tmpContract
from VNContract_Detail WITH (NOLOCK) 
where ID = @contract

--撈進/出口與調整資料
select 	a.NLCode
		,sum(a.Qty) as Qty 
into #tmpDeclare 
from (
	select 	vid.NLCode
			,Qty = round(vid.Qty,6)
	from VNImportDeclaration vi WITH (NOLOCK) 
	inner join VNImportDeclaration_Detail vid WITH (NOLOCK) on vid.ID = vi.ID
	where vi.VNContractID = @contract and vi.Status = 'Confirmed'
    and (vi.CDate <= @GenerateDate or @GenerateDate is null)

	union all
	select 	vcd.NLCode
			, 0 - round(((vcd.Qty * ved.ExportQty)+(vcd.Qty * ved.ExportQty)* vcd.Waste),6)
	from VNExportDeclaration ve WITH (NOLOCK) 
	inner join VNExportDeclaration_Detail ved WITH (NOLOCK) on ved.ID = ve.ID
	inner join VNConsumption vc WITH (NOLOCK) on vc.VNContractID = ve.VNContractID and ved.CustomSP = vc.CustomSP
	inner join VNConsumption_Detail vcd WITH (NOLOCK) on vcd.ID = vc.ID
    inner join VNContract_Detail vctd on vctd.id=vc.VNContractID and vcd.NLCode=vctd.NLCode
	where ve.VNContractID = @contract and ve.Status = 'Confirmed'
    and (ve.CDate <= @GenerateDate or @GenerateDate is null)

	union all
	select 	vcd.NLCode
			,Qty = round(vcd.Qty,6)
	from VNContractQtyAdjust vc WITH (NOLOCK) 
	inner join VNContractQtyAdjust_Detail vcd WITH (NOLOCK) on vc.ID = vcd.ID
	where vc.VNContractID = @contract and vc.Status != 'New'
    and (vc.CDate <= @GenerateDate or @GenerateDate is null)
) a
group by a.NLCode;",
                this.contract,
                Env.User.Keyword,
                this.strGenerateDate));

            if (this.liguidationonly)
            {
                #region liguidationonly = true
                sqlCmd.Append(@"
select 	isnull(tc.HSCode,'') as HSCode
		,a.NLCode
		,isnull(vcd.DescEN,'') as Description
		,isnull(tc.UnitID,'') as UnitID
		,isnull(tc.Qty,0) + isnull(td.Qty,0) as LiqQty
from (
	select NLCode 
	from #tmpContract 

	union 
	select NLCode 
	from #tmpDeclare
) a
left join #tmpContract tc on a.NLCode = tc.NLCode
left join VNNLCodeDesc vcd WITH (NOLOCK) on a.NLCode = vcd.NLCode
left join #tmpDeclare td on a.NLCode = td.NLCode
where 1 = 1");

                if (!MyUtility.Check.Empty(this.hscode))
                {
                    sqlCmd.Append(string.Format(" and tc.HSCode = '{0}'", this.hscode));
                }

                if (!MyUtility.Check.Empty(this.nlcode))
                {
                    sqlCmd.Append(string.Format(" and a.NLCode = '{0}'", this.nlcode));
                }

                sqlCmd.Append(@"                                                                                                       
order by TRY_CONVERT(int, SUBSTRING(a.NLCode, 3, LEN(a.NLCode))), a.NLCode

drop table #tmpContract;
drop table #tmpDeclare;");
                #endregion
            }
            else
            {
                #region liguidationonly = false
                sqlCmd.Append(
                    $@"
----------------------------------------------------------------
-----------------------資料準備---------------------------------
----------------------------------------------------------------
---- 取得訂單清單 ----
/*  
    訂單包括以下
    1. 訂單尚未關倉
    2. Shortage 訂單
        該種類訂單可能已經關倉
        但實際上總產出有可能大於總出貨（短出）
        因此該類訂單的成衣可能還放在工廠成品倉
    3. FOC 訂單完成 Order_Finish
        FOC 訂單成衣產出後
        基本上會先放在倉庫中
        並且該訂單會先關閉
        日後需要出貨時才會再建立出貨單據
        
    Garment 訂單款式 Categroy 全部都要回推母單才能找到物料展開清單
    因應 G 單的調整
    新增欄位 Origin 保存原始款式 Category 的資訊

    Origin : 顯示用    
    其他 : 系統判斷用（主要是因為 G 單物料展開要回母單找）
*/
select  o.ID
		, o.POID 
        , o.FactoryID
		, o.MDivisionID
        , Category = IIF (o.Category = 'G', fromSP.Category, o.Category)
        , StyleID = IIF (o.Category = 'G', fromSP.StyleID, o.StyleID)
        , BrandID = IIF (o.Category = 'G', fromSP.BrandID, o.BrandID)
        , SeasonID = IIF (o.Category = 'G', fromSP.SeasonID, o.SeasonID)
        , StyleUKey = IIF (o.Category = 'G', fromSP.StyleUKey, o.StyleUKey)
		, OriCategory = o.Category
        , OriStyleID = o.StyleID
		, OriBrandID = o.BrandID
		, OriSeasonID = o.SeasonID
		, OriStyleUKey = o.StyleUKey
        , o.WhseClose
into #tmpOrderList 
from Orders o  WITH (NOLOCK) 
outer apply (
    select top 1
            gmo.Category
            , gmo.StyleID
            , gmo.BrandID
            , gmo.SeasonID
            , gmo.StyleUKey
    from Orders gmo WITH (NOLOCK)
    where   o.Category = 'G'
            and exists (
                select 1
                from Order_Qty_Garment oqg WITH (NOLOCK) 
                where o.id = oqg.id
                      and gmo.ID = oqg.OrderIDFrom
            )
) fromSP
where   o.Category <>''
        and (   
            -- 訂單尚未關倉
            o.WhseClose is null

            -- Bulk, Sample, Garment 訂單雖然已經關單但是短出 Shortage
            or (o.GMTComplete = 'S')

            -- FOC 訂單生產完後會先放在倉庫等日後才會做出貨
            or exists (
                select 1
                from Order_Finish orf With (NoLock)
                where o.id = orf.id
            )

            or o.WhseClose >= @GenerateDate --訂單的關單日在『特定日期（含當天）』之後
        )
        and o.Qty<>0
        and o.LocalOrder = 0 
        and CONVERT(date, o.AddDate) <= @GenerateDate -- 訂單建立日期在『特定日期（含當天）』之前
        {whereftys}


---- 先整理出成衣 - 物料展開資訊 ----
/*    
    撈各Style目前最後的CustomSP
*/
select 	 v.ID
		,v.CustomSP
        , v.StyleID
        , v.BrandID
        , v.Category 
        , v.seasonid 
        , va.Article
        , vs.SizeCode
        , vdd.HSCode
        , vdd.NLCode
        , vdd.RefNo
        , vdd.SCIRefno
        , vdd.FabricType
	    , vdd.LocalItem
        , vdd.Qty
        , vdd.UnitID
        , vdd.StockQty
        , vdd.StockUnit
        , vd.Waste
into #tmpCustomSP
from VNConsumption v WITH (NOLOCK) 
inner join (
	select 	vc.StyleID
			,vc.BrandID
			,vc.Category
            ,vc.sizecode
			,MAX(vc.CustomSP) as CustomSP
	from VNConsumption vc WITH (NOLOCK) 
	where vc.VNContractID = @contract
	group by vc.StyleID,vc.BrandID,vc.Category,vc.sizecode
) vc on vc.CustomSP = v.CustomSP
inner join VNConsumption_Detail vd WITH (NOLOCK) on vd.ID = v.ID
inner join VNConsumption_Detail_Detail vdd WITH (NOLOCK) on vdd.ID = vd.ID 
                                                           and vdd.NLCode = vd.NLCode
inner join VNConsumption_Article va WITH (NOLOCK)on  va.ID = v.ID 
inner join VNConsumption_SizeCode vs WITH (NOLOCK) on vs.ID = v.ID 
where v.VNContractID = @contract


---- 整理出 WIP 成衣產出資訊 ----
/*
    WIP
        Garment 訂單只須關注 Garment 母單
        主要是母單進行生產
        生產完畢後再分配到子單
        因此發料紀錄只有母單才有
        * 子單 : AutoCreate = 1

        WIP 確認尚未關倉的訂單 &&
		關倉日期比 GenerateDate 晚，代表當天還沒有關倉
*/
select sdd.OrderID
		, sdd.ComboType
		, sdd.Article
		, sdd.SizeCode
		, QAQty = sum(sdd.QAQty)
into #tmpSewingOutput_WHNotClose
from #tmpOrderList t
inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = t.id
where   (t.WhseClose is null or t.WhseClose >= @GenerateDate)
        and not exists (
            select 1
            from SewingOutput_Detail sd With (NoLock)
            where sd.Ukey = sdd.SewingOutput_DetailUkey
                  and sd.AutoCreate = 1
        )
        and exists(
			select 1 from SewingOutput s
			where sdd.ID= s.ID
			and s.OutputDate <= @GenerateDate
        )
group by sdd.OrderID, sdd.ComboType, sdd.Article, sdd.SizeCode



----  整理出成衣庫存 - 成衣生產資訊 ----
 /*
    Prod.
        OrderID
        因為要對應出貨因此使用會出貨的訂單號碼
    
    如果訂單為 G 單
    OrderIDFrom 為生產母單
    OrderID 為 Garment 子單
    其他 Category 的訂單只會有 OrderID
*/
select	OrderId = IIF (ASO.OrderID is not null, ASO.OrderID, AGO.OrderIDfrom)
		, ComboType = IIF (ASO.ComboType is not null, ASO.ComboType, AGO.ComboType)
		, Article = IIF (ASO.Article is not null, ASO.Article, AGO.Article)
		, SizeCode = IIF (ASO.SizeCode is not null, ASO.SizeCode, AGO.SizeCode)
		, QAQty = ISNULL (ASO.QAQty, 0) - ISNULL (AGO.QAQty, 0)
into #tmpSewingOutput_InFty
from (
	/*
        AllSewingOuptut	
    */
	select sdd.OrderID
			, sdd.ComboType
			, sdd.Article
			, sdd.SizeCode
			, QAQty = sum(sdd.QAQty)
	from #tmpOrderList t
    inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = t.id
	inner join SewingOutput s WITH (NOLOCK) on s.id=sdd.ID
	where s.OutputDate <= @GenerateDate
	group by sdd.OrderID, sdd.ComboType, sdd.Article, sdd.SizeCode   
) ASO 
full outer join (
    /*
        AssignGarmentOrder
        母單的生產數量會分配到子單
        此處是為了扣除母單已分配的數量
    */
	select 	sdd.OrderIDfrom -- G 單-生產母單
			, sdd.ComboType
			, sdd.Article
			, sdd.SizeCode
			, QAQty = sum (sdd.QAQty)
	from #tmpOrderList t
    inner join SewingOutput_Detail_Detail_Garment sdd WITH (NOLOCK) on sdd.OrderIDfrom = t.id
	inner join SewingOutput s with(nolock) on s.ID=sdd.ID
	where s.OutputDate <= @GenerateDate
	group by sdd.OrderIDfrom, sdd.ComboType, sdd.Article, sdd.SizeCode
) AGO on ASO.OrderId = AGO.OrderIDfrom
		    and ASO.ComboType = AGO.ComboType
		    and ASO.Article = AGO.Article
		    and ASO.SizeCode = AGO.SizeCode

----------------------------------------------------------------
-- 01在途物料(已報關但還在途)(On Road Material Qty新增報表)-----
----------------------------------------------------------------
Declare @EtaRange date = dateadd(day,-31, @GenerateDate)
select * 
into #tmpOnRoadMaterial
from (
	select  [HSCode] = f.HSCode
	        , [NLCode] = f.NLCode
	        , [PoID] = ed.PoID
            , o.FactoryID
	        , [Seq] = ed.Seq1+'-'+ed.Seq2
	        , [Refno] = ed.refno
            , [MaterialType] = dbo.GetMaterialTypeDesc(f.Type)
	        , [Description] = ed.Description
	        , [Qty] =( dbo.getVNUnitTransfer(isnull(f.Type, '')
		                ,dbo.getStockUnit(psd.SciRefno,ed.Suppid)
		                ,isnull(f.CustomsUnit, '')
		                ,(ed.qty+ed.foc)
		                ,isnull(f.Width,0)
		                ,isnull(f.PcsWidth,0)
		                ,isnull(f.PcsLength,0)
		                ,isnull(f.PcsKg,0)
		                ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
			                (select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),
			                (select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit 
			                and TO_U = isnull (f.CustomsUnit,''))),1)
		                ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
			                (select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),
			                (select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit 
			                and TO_U = isnull(f.CustomsUnit,''))),'')
	                    ,default))
	        , [CustomsUnit] = f.CustomsUnit
	        , [OnRaodQty] = dbo.getUnitRate(psd.PoUnit,dbo.getStockUnit(psd.SciRefno,ed.Suppid))*(ed.qty+ed.foc)
	        , [StockUnit] = dbo.getStockUnit(psd.SciRefno,ed.Suppid)				
	from Export e WITH (NOLOCK) 
	inner join Export_Detail ed WITH (NOLOCK) on e.id=ed.id
	inner join VNImportDeclaration vd WITH (NOLOCK) on vd.BLNo=e.BLNo 
	inner join po_supp_detail psd WITH (NOLOCK) on psd.ID = ed.poid 
                                                   and psd.seq1 = ed.seq1 
                                                   and psd.seq2=ed.seq2
	inner join Fabric f WITH (NOLOCK) on f.SciRefno=psd.SciRefno		
    inner join Orders o WITH (NOLOCK) on o.id= ed.poid
	where   vd.VNContractID = @contract
	        and vd.Status='Confirmed'
	        and not exists (
                select 1 
                from Receiving WITH (NOLOCK)
		        where exportID = e.id and status='Confirmed'
				and WhseArrival <= @GenerateDate -- 特定日期沒有倉庫收料紀錄
            )
	        and vd.blno<>''
            and e.Eta between @EtaRange and @GenerateDate --WK#, FtyWK#（物料進口 - 到貨（港）日在『特定日期（含當天）』 的 30 天內）
            and vd.CDate <= @GenerateDate -- 特定日期（含當天）』前已完成進口報關
			and CONVERT(date, e.AddDate) <= @GenerateDate -- 排除資料建立日期在『特定日期（含當天）』後的 WK#, Fty WK# 
            {whereftys}
    
    union all
	
	select distinct
	        [HSCode] = isnull(isnull(f.HSCode,li.HSCode),'')
	        , [NLCode] = isnull(isnull(f.NLCode,li.NLCode),'')
	        , [POID] = fed.PoID
            , o.FactoryID
	        , [Seq] = fed.Seq1+'-'+fed.Seq2
	        , [RefNo] = fed.refno
            , [MaterialType] = iif(f.Type is null,li.Category,dbo.GetMaterialTypeDesc(f.Type))
	        , [Description] = isnull(isnull(f.Description, li.Description),'')
	        , [Qty] =( dbo.getVNUnitTransfer(isnull(li.Category, '')
		                ,StockUnit.unit
		                ,isnull(li.CustomsUnit, '')
		                ,fed.qty * IIF(fed.UnitId = 'CONE',isnull(li.MeterToCone,0),1)
		                ,0
		                ,isnull(li.PcsWidth,0)
		                ,isnull(li.PcsLength,0)
		                ,isnull(li.PcsKg,0)
		                ,isnull(IIF(isnull(li.CustomsUnit, '') = 'M2',
			                (select RateValue from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) and TO_U = 'M'),
			                (select RateValue from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) 
			                and TO_U = isnull (li.CustomsUnit,''))),1)
		                ,isnull(IIF(isnull(li.CustomsUnit, '') = 'M2',
			                (select Rate from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) and TO_U = 'M'),
			                (select Rate from dbo.View_Unitrate where FROM_U = IIF(fed.UnitId = 'CONE','M',fed.UnitId) and TO_U = isnull(f.CustomsUnit,''))),'')
	                    ,isnull(li.Refno,'')))
	        , [CustomsUnit] = isnull(isnull(f.CustomsUnit,li.CustomsUnit),'')
	        , [OnRaodQty] = isnull(UnitRateQty.qty,0)
	        , [StockUnit] = isnull(StockUnit.unit,'')
	from FtyExport fe WITH (NOLOCK)
	inner join FtyExport_Detail fed WITH (NOLOCK) on fe.id = fed.id	
    inner join Orders o WITH (NOLOCK) on o.id = fed.PoID
	left join Fabric f WITH (NOLOCK) on f.SciRefno = fed.SciRefno			
	left join LocalItem li WITH (NOLOCK) on li.Refno = fed.RefNo
	outer apply(
		select unit = iif(fe.type in (2,3),dbo.getStockUnit(fed.SciRefno,fed.SuppID),iif(fe.type in (1,4),li.UnitID,''))
	) StockUnit		
	outer apply(
		select Qty = iif(fe.type in (2,3),dbo.getUnitRate(fed.UnitID,StockUnit.unit)*(fed.qty),
		iif(fe.type in (1,4),fed.qty,0))
	) UnitRateQty							
	where   1=1 
            and exists (
                select 1 
                from VNImportDeclaration WITH (NOLOCK) 
                where (blno=fe.blno or wkno=fe.id) 
		              and blno<>'' 
                      and vncontractid=@contract
                      and CDate <= @GenerateDate -- 特定日期（含當天）』前已完成進口報關
            )	
	        and not exists (
                select 1 
                from Receiving WITH (NOLOCK)
		        where InvNo = fe.InvNo 
                      and status='Confirmed'
                      and WhseArrival <= @GenerateDate -- 特定日期沒有倉庫收料紀錄
            )
	        and not exists (
                select 1 
                from TransferIn WITH (NOLOCK)
		        where InvNo = fe.InvNo 
                      and status='Confirmed'
					  and IssueDate <= @GenerateDate -- 特定日期沒有倉庫收料紀錄
            )
	        and not exists (
                select 1 
                from LocalReceiving WITH (NOLOCK)
		        where InvNo = fe.InvNo 
                    and status='Confirmed'
                    and IssueDate <= @GenerateDate -- 特定日期沒有倉庫收料紀錄
            )
            and fe.PortArrival between @EtaRange and @GenerateDate --WK#, FtyWK#（物料進口 - 到貨（港）日在『特定日期（含當天）』 的 30 天內）
            AND CONVERT(date, fe.AddDate) <= @GenerateDate --排除資料建立日期在『特定日期（含當天）』後的 WK#, Fty WK#  
            {whereftys}
) a				

----------------------------------------------------------------
------------ 02 料倉(AB)( W/House Qty Detail) ------------------
----------------------------------------------------------------
--撈今天 W/House資料
select * 
into #tmpWHQty1
from (
	select  [HSCode] = isnull(f.HSCode,'')
	        , [NLCode] = isnull(f.NLCode,'')
	        , [POID] = o.POID
            , o.FactoryID
	        , [Seq] = (fi.Seq1+'-'+fi.Seq2)
	        , [Refno] = psd.Refno
            , [MaterialType] = dbo.GetMaterialTypeDesc(f.Type)
	        , [Description] = f.Description
	        , [Roll] = fi.Roll
	        , [Dyelot] = fi.Dyelot
	        , [StockType] = fi.StockType
	        , [Location] = isnull((select CONCAT(fid.MtlLocationID,',') 
		                            from FtyInventory_Detail fid WITH (NOLOCK) 
		                            where fid.Ukey = fi.UKey 
		                            for xml path(''))
		                           ,'')
	        ,[Qty] = IIF(fi.InQty-fi.OutQty+fi.AdjustQty != 0, dbo.getVNUnitTransfer(
			        isnull(f.Type, '')
			        ,psd.StockUnit
			        ,isnull(f.CustomsUnit, '')
			        ,(fi.InQty-fi.OutQty+fi.AdjustQty)
			        ,isnull(f.Width,0)
			        ,isnull(f.PcsWidth,0)
			        ,isnull(f.PcsLength,0)
			        ,isnull(f.PcsKg,0)
			        ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
				        (select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
			        ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
				        (select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),'')
                        ,default)
			        , 0)
	        , [W/House Unit] = f.CustomsUnit
	        , [W/House Qty(Stock Unit)] = fi.InQty-fi.OutQty+fi.AdjustQty
	        , [Stock Unit] = psd.StockUnit
	from FtyInventory fi WITH (NOLOCK)  --EDIT
    inner join Orders o WITH (NOLOCK) on o.id= fi.POID
	left join PO_Supp_Detail psd WITH (NOLOCK) on fi.POID = psd.ID 
                                                  and psd.SEQ1 = fi.Seq1 
                                                  and psd.SEQ2 = fi.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
	where (fi.StockType = 'B' or fi.StockType = 'I')		 	
    and fi.InQty-fi.OutQty+fi.AdjustQty != 0
          {whereftys}
    
    union all

    select  HSCode
            , NLCode
            , POID
            , FactoryID
            , Seq
            , RefNo
            , MaterialType
            , Description
            , Roll
            , Dyelot
            , StockType
            , Location
            , Qty=sum(Qty)
            , [W/House Unit]
            , [W/House Qty(Usage Unit)]=sum([W/House Qty(Usage Unit)])
            , [Customs Unit]
    from(
        select  [HSCode] = isnull(li.HSCode,'') 
	            , [NLCode] = isnull(li.NLCode,'') 
	            , [POID] = o.POID
                , o.FactoryID
	            , [Seq] = ''
	            , [RefNo] = l.Refno
                , [MaterialType] = dbo.GetMaterialTypeDesc(li.Category)
	            , [Description] = li.Description
	            , [Roll] = '' 
	            , [Dyelot] = '' 
	            , [StockType] = 'B'
	            , [Location] = '' 
	            , [Qty] = IIF(l.InQty-l.OutQty+l.AdjustQty != 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
		                    ,l.UnitId
		                    ,li.CustomsUnit
		                    ,(l.InQty-l.OutQty+l.AdjustQty)
		                    ,0
		                    ,li.PcsWidth
		                    ,li.PcsLength
		                    ,li.PcsKg
		                    ,isnull(IIF(li.CustomsUnit = 'M2',
			                    (select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			                    ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),1)
		                    ,isnull(IIF(li.CustomsUnit = 'M2',
			                    (select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			                    ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),'')
                                ,li.Refno)
		                    ,0)
	            , [W/House Unit] = li.CustomsUnit
	            , [W/House Qty(Usage Unit)] =l.InQty-l.OutQty+l.AdjustQty
	            , [Customs Unit] = l.UnitId
        from LocalInventory l WITH (NOLOCK) 	
        inner join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
        inner join Orders o WITH (NOLOCK) on o.id= l.OrderID
        where 1=1
		and l.InQty-l.OutQty+l.AdjustQty != 0
        {whereftys}
    )x
	group by HSCode,NLCode,POID,FactoryID,Seq,RefNo,MaterialType,Description,Roll,Dyelot,StockType,Location,[W/House Unit],[Customs Unit]
) a

/*特定日期區間資料*/
select * 
into #tmpWHQty2
from (
	select  [HSCode] = isnull(f.HSCode,'')
	        , [NLCode] = isnull(f.NLCode,'')
	        , [POID] = o.POID
            , o.FactoryID
	        , [Seq] = (fi.Seq1+'-'+fi.Seq2)
	        , [Refno] = psd.Refno
            , [MaterialType] = dbo.GetMaterialTypeDesc(f.Type)
	        , [Description] = f.Description
	        , [Roll] = fi.Roll
	        , [Dyelot] = fi.Dyelot
	        , [StockType] = fi.StockType
	        , [Location] = isnull((select CONCAT(fid.MtlLocationID,',') 
		                            from FtyInventory_Detail fid WITH (NOLOCK) 
		                            where fid.Ukey = fi.UKey 
		                            for xml path(''))
		                           ,'')
	        ,[Qty] = IIF(WH_Issue.Qty+WH07_08.Qty+WH15_16.Qty+WH17.Qty+WH18.Qty+WH19.Qty+WH34_35.Qty+WH37.Qty+WHBorrowBack_Plus.Qty+WHBorrowBack_Reduce.Qty+WHSubTransfer_Plus.Qty+WHSubTransfer_Reduce.Qty != 0, dbo.getVNUnitTransfer(
			        isnull(f.Type, '')
			        ,psd.StockUnit
			        ,isnull(f.CustomsUnit, '')
			        ,WH_Issue.Qty+WH07_08.Qty+WH15_16.Qty+WH17.Qty+WH18.Qty+WH19.Qty+WH34_35.Qty+WH37.Qty+WHBorrowBack_Plus.Qty+WHBorrowBack_Reduce.Qty+WHSubTransfer_Plus.Qty+WHSubTransfer_Reduce.Qty
			        ,isnull(f.Width,0)
			        ,isnull(f.PcsWidth,0)
			        ,isnull(f.PcsLength,0)
			        ,isnull(f.PcsKg,0)
			        ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
				        (select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
			        ,isnull(IIF(isnull(f.CustomsUnit, '') = 'M2',
				        (select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),'')
                        ,default)
			        , 0)
	        , [W/House Unit] = f.CustomsUnit
	        , [W/House Qty(Stock Unit)] = WH_Issue.Qty+WH07_08.Qty+WH15_16.Qty+WH17.Qty+WH18.Qty+WH19.Qty+WH34_35.Qty+WH37.Qty+WHBorrowBack_Plus.Qty+WHBorrowBack_Reduce.Qty+WHSubTransfer_Plus.Qty+WHSubTransfer_Reduce.Qty
	        , [Stock Unit] = psd.StockUnit
	from FtyInventory fi WITH (NOLOCK)  --EDIT
    inner join Orders o WITH (NOLOCK) on o.id= fi.POID
	left join PO_Supp_Detail psd WITH (NOLOCK) on fi.POID = psd.ID 
                                                  and psd.SEQ1 = fi.Seq1 
                                                  and psd.SEQ2 = fi.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
	outer apply(
		select Qty = - isnull(sum(b.StockQty),0) 
		from Receiving a
		inner join Receiving_Detail b on a.Id=b.Id
		where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.WhseArrival > @GenerateDate and a.WhseArrival <= GETDATE() --特定日期到 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
		and a.Type in ('A','B')
	)WH07_08
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from ReturnReceipt a
		inner join ReturnReceipt_Detail b on a.id=b.id
		where b.POID = fi.POID and b.Seq1=fi.Seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期到 A, B 倉有收發紀錄的訂單
		and a.Status = 'Confirmed'
	)WH37
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from Issue a
		inner join Issue_Detail b on a.Id=b.Id
		where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期到 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
		and a.Type in ('A','B','C','D','E','I')
	)WH_Issue
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from Issuelack a
		inner join Issuelack_Detail b on a.Id=b.Id
		where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Type='R' and a.FabricType in ('F','A')
		and a.Status !='New'
	)WH15_16
	outer apply(
		select Qty = - isnull(sum(b.Qty),0) 
		from IssueReturn a
		inner join IssueReturn_Detail b on a.Id=b.Id
		where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
	)WH17
	outer apply(
		select Qty = - isnull(sum(b.QtyAfter) - sum(b.QtyBefore),0) 
		from Adjust a
		inner join Adjust_Detail b on a.Id=b.Id
		where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
		and a.Type in ('A','B')
	)WH34_35
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from SubTransfer a
		inner join SubTransfer_Detail b on a.Id=b.Id
		where b.FromFtyInventoryUkey = fi.Ukey and b.FromPOID = fi.POID and b.FromSeq1=fi.seq1 and b.FromSeq2=fi.Seq2
		and b.FromStockType = fi.StockType and b.FromRoll = fi.Roll and b.FromDyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
		and a.Type in ('E','D')
	)WHSubTransfer_Plus
	outer apply(
		select Qty = - isnull(sum(b.Qty),0) 
		from SubTransfer a
		inner join SubTransfer_Detail b on a.Id=b.Id
		where b.ToPOID = fi.POID and b.ToSeq1=fi.seq1 and b.ToSeq2=fi.Seq2
		and b.ToStockType = fi.StockType and b.ToRoll = fi.Roll and b.ToDyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
		and a.Type in ('C')
	)WHSubTransfer_Reduce
	outer apply(
		select Qty = - isnull(sum(b.Qty),0) 
		from TransferIn a
		inner join TransferIn_Detail b on a.Id=b.Id
		where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
	)WH18
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from TransferOut a
		inner join TransferOut_Detail b on a.Id=b.Id
		where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
		and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
	)WH19
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from BorrowBack a
		inner join BorrowBack_Detail b on a.Id=b.Id
		where b.FromPoId = fi.POID and b.FromSeq1=fi.seq1 and b.FromSeq2=fi.Seq2
		and b.FromStockType = fi.StockType and b.FromRoll = fi.Roll and b.FromDyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
		and a.Type in ('A','B')
	)WHBorrowBack_Plus
	outer apply(
		select Qty = - isnull(sum(b.Qty),0) 
		from BorrowBack a
		inner join BorrowBack_Detail b on a.Id=b.Id
		where b.ToPoId = fi.POID and b.ToSeq1=fi.seq1 and b.ToSeq2=fi.Seq2
		and b.ToStockType = fi.StockType and b.ToRoll = fi.Roll and b.ToDyelot = fi.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
		and a.Status='Confirmed'
		and a.Type in ('A','B')
	)WHBorrowBack_Reduce

	where (fi.StockType = 'B' or fi.StockType = 'I')
    and WH_Issue.Qty+WH07_08.Qty+WH15_16.Qty+WH17.Qty+WH18.Qty+WH19.Qty+WH34_35.Qty+WH37.Qty+WHBorrowBack_Plus.Qty+WHBorrowBack_Reduce.Qty+WHSubTransfer_Plus.Qty+WHSubTransfer_Reduce.Qty != 0
		  and exists (
			select 1 from Receiving a
			inner join Receiving_Detail b on a.Id=b.Id
			where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.WhseArrival > @GenerateDate and a.WhseArrival <= GETDATE() --特定日期到 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			union all
			select 1 from ReturnReceipt a
			inner join ReturnReceipt_Detail b on a.id=b.id
			where b.POID = fi.POID and b.Seq1=fi.Seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期到 A, B 倉有收發紀錄的訂單
			and a.Status = 'Confirmed'
			union all
			select 1 from Issue a
			inner join Issue_Detail b on a.Id=b.Id
			where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期到 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			and a.Type in ('A','B','C','D','E','I')
			union all
			select 1 from Issuelack a
			inner join Issuelack_Detail b on a.Id=b.Id
			where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Type='R' and a.FabricType in ('F','A')
			and a.Status !='New'
			union all
			select 1 from IssueReturn a
			inner join IssueReturn_Detail b on a.Id=b.Id
			where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			union all
			select 1 from Adjust a
			inner join Adjust_Detail b on a.Id=b.Id
			where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			and a.Type in ('A','B')
			union all
			select 1 from SubTransfer a
			inner join SubTransfer_Detail b on a.Id=b.Id
			where b.FromFtyInventoryUkey = fi.Ukey and b.FromPOID = fi.POID and b.FromSeq1=fi.seq1 and b.FromSeq2=fi.Seq2
			and b.FromStockType = fi.StockType and b.FromRoll = fi.Roll and b.FromDyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			and a.Type in ('E','D')
            union all
			select 1 
			from SubTransfer a
			inner join SubTransfer_Detail b on a.Id=b.Id
			where b.ToPOID = fi.POID and b.ToSeq1=fi.seq1 and b.ToSeq2=fi.Seq2
			and b.ToStockType = fi.StockType and b.ToRoll = fi.Roll and b.ToDyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			and a.Type in ('C')
			union all
			select 1 from TransferIn a
			inner join TransferIn_Detail b on a.Id=b.Id
			where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			union all
			select 1 from TransferOut a
			inner join TransferOut_Detail b on a.Id=b.Id
			where b.PoId = fi.POID and b.Seq1=fi.seq1 and b.Seq2=fi.Seq2
			and b.StockType = fi.StockType and b.Roll = fi.Roll and b.Dyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			union all
			select 1 from BorrowBack a
			inner join BorrowBack_Detail b on a.Id=b.Id
			where b.FromPoId = fi.POID and b.FromSeq1=fi.seq1 and b.FromSeq2=fi.Seq2
			and b.FromStockType = fi.StockType and b.FromRoll = fi.Roll and b.FromDyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			and a.Type in ('A','B')
			union all
			select 1 from BorrowBack a
			inner join BorrowBack_Detail b on a.Id=b.Id
			where b.ToPoId = fi.POID and b.ToSeq1=fi.seq1 and b.ToSeq2=fi.Seq2
			and b.ToStockType = fi.StockType and b.ToRoll = fi.Roll and b.ToDyelot = fi.Dyelot
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status='Confirmed'
			and a.Type in ('A','B')
		  )
        {whereftys}
		 
    
    union all

    select  HSCode
            , NLCode
            , POID
            , FactoryID
            , Seq
            , RefNo
            , MaterialType
            , Description
            , Roll
            , Dyelot
            , StockType
            , Location
            , Qty=sum(Qty)
            , [W/House Unit]
            , [W/House Qty(Usage Unit)]=sum([W/House Qty(Usage Unit)])
            , [Customs Unit]
    from(
        select  [HSCode] = isnull(li.HSCode,'') 
	            , [NLCode] = isnull(li.NLCode,'') 
	            , [POID] = o.POID
                , o.FactoryID
	            , [Seq] = ''
	            , [RefNo] = l.Refno
                , [MaterialType] = dbo.GetMaterialTypeDesc(li.Category)
	            , [Description] = li.Description
	            , [Roll] = '' 
	            , [Dyelot] = '' 
	            , [StockType] = 'B'
	            , [Location] = '' 
	            , [Qty] = IIF(WH39.Qty+WH47.Qty+WH60.Qty+WH61.Qty != 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
		                    ,l.UnitId
		                    ,li.CustomsUnit
		                    ,(WH39.Qty+WH47.Qty+WH60.Qty+WH61.Qty)
		                    ,0
		                    ,li.PcsWidth
		                    ,li.PcsLength
		                    ,li.PcsKg
		                    ,isnull(IIF(li.CustomsUnit = 'M2',
			                    (select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			                    ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),1)
		                    ,isnull(IIF(li.CustomsUnit = 'M2',
			                    (select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			                    ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),'')
                                ,li.Refno)
		                    ,0)
	            , [W/House Unit] = li.CustomsUnit
	            , [W/House Qty(Usage Unit)] = WH39.Qty+WH47.Qty+WH60.Qty+WH61.Qty
	            , [Customs Unit] = l.UnitId
        from LocalInventory l WITH (NOLOCK) 	
        inner join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
        inner join Orders o WITH (NOLOCK) on o.id= l.OrderID
		outer apply(
			select Qty = - ISNULL(sum(b.Qty),0)
			from LocalReceiving a
			inner join LocalReceiving_Detail b on a.Id=b.Id
			where b.OrderId = o.ID
			and b.Refno = l.Refno and b.ThreadColorID = l.ThreadColorID
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status = 'Confirmed'
		)WH60
		outer apply(
			select Qty = ISNULL(sum(b.Qty),0)
			from LocalIssue a
			inner join LocalIssue_Detail b on a.Id=b.Id
			where b.OrderID = o.ID
			and b.Refno = l.Refno and b.ThreadColorID = l.ThreadColorID
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status = 'Confirmed'
		)WH61
		outer apply(
			select Qty = - ISNULL(sum(b.QtyAfter) - sum(b.QtyBefore),0)
			from AdjustLocal a
			inner join AdjustLocal_Detail b on a.Id=b.Id
			where b.POID = l.OrderID
			and b.Refno = l.Refno and b.Color = l.ThreadColorID
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status = 'Confirmed'
			and a.Type='A'
		)WH39
		outer apply(
			select Qty = ISNULL(sum(b.Qty),0)
			from SubTransferLocal a
			inner join SubTransferLocal_Detail b on a.Id=b.Id
			where b.POID = l.OrderID
			and b.Refno = l.Refno and b.Color = l.ThreadColorID
			and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status = 'Confirmed'
			and a.Type='D'
		)WH47
        where WH39.Qty+WH47.Qty+WH60.Qty+WH61.Qty != 0
			  and exists(
				select 1 from LocalReceiving a
				inner join LocalReceiving_Detail b on a.Id=b.Id
				where b.OrderId = o.ID
				and b.Refno = l.Refno and b.ThreadColorID = l.ThreadColorID
				and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
				and a.Status = 'Confirmed'
				union all
				select 1 from LocalIssue a
				inner join LocalIssue_Detail b on a.Id=b.Id
				where b.OrderID = o.ID
				and b.Refno = l.Refno and b.ThreadColorID = l.ThreadColorID
				and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
				and a.Status = 'Confirmed'
				union all
				select 1 from AdjustLocal a
				inner join AdjustLocal_Detail b on a.Id=b.Id
				where b.POID = l.OrderID
				and b.Refno = l.Refno and b.Color = l.ThreadColorID
				and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
				and a.Status = 'Confirmed'
				and a.Type='A'
				union all
				select 1 from SubTransferLocal a
				inner join SubTransferLocal_Detail b on a.Id=b.Id
				where b.POID = l.OrderID
				and b.Refno = l.Refno and b.Color = l.ThreadColorID
				and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() --特定日期 A, B 倉有收發紀錄的訂單
				and a.Status = 'Confirmed'
				and a.Type='D'
			  )
            {whereftys}
    )x
	group by HSCode,NLCode,POID,FactoryID,Seq,RefNo,MaterialType,Description,Roll,Dyelot,StockType,Location,[W/House Unit],[Customs Unit]
) a

select 
HSCode = isnull(a.HSCode,b.HSCode)
, NLCode = isnull(a.NLCode,b.NLCode)
, POID = isnull(a.POID,b.POID)
, FactoryID = isnull(a.FactoryID,b.FactoryID)
, Seq = isnull(a.Seq,b.Seq)
, Refno = isnull(a.Refno,b.Refno)
, MaterialType = isnull(a.MaterialType,b.MaterialType)
, Description = isnull(a.Description,b.Description)
, Roll = isnull(a.Roll,b.Roll)
, Dyelot = isnull(a.Dyelot,b.Dyelot)
, StockType = isnull(a.StockType,b.StockType)
, Location = isnull(a.Location,b.Location)
, [Qty] = sum(isnull(a.Qty,0) + isnull(b.Qty,0))
, [W/House Unit] = isnull(a.[W/House Unit],b.[W/House Unit])
, [W/House Qty(Stock Unit)] = sum(isnull(a.[W/House Qty(Stock Unit)],0) + isnull(b.[W/House Qty(Stock Unit)],0))
, [Stock Unit] = isnull(a.[Stock Unit],b.[Stock Unit])
into #tmpWHQty
from #tmpWHQty1 a
full outer join #tmpWHQty2 b on a.POID = b.POID
and a.FactoryID = b.FactoryID and a.Seq=b.Seq 
and a.Roll = b.Roll and a.Dyelot = b.Dyelot
and a.Refno = b.Refno and a.MaterialType = b.MaterialType 
and a.HSCode = b.HSCode and a.NLCode = b.NLCode
and a.StockType = b.StockType and a.Location = b.Location
and a.[Stock Unit] = b.[Stock Unit]
where isnull(a.[W/House Qty(Stock Unit)],0) + isnull(b.[W/House Qty(Stock Unit)],0) != 0
group by isnull(a.HSCode,b.HSCode),isnull(a.NLCode,b.NLCode),isnull(a.POID,b.POID),isnull(a.FactoryID,b.FactoryID),isnull(a.Seq,b.Seq)
,isnull(a.Refno,b.Refno),isnull(a.MaterialType,b.MaterialType),isnull(a.Description,b.Description),isnull(a.Roll,b.Roll),isnull(a.Dyelot,b.Dyelot)
,isnull(a.StockType,b.StockType),isnull(a.Location,b.Location), isnull(a.[W/House Unit],b.[W/House Unit]),isnull(a.[Stock Unit],b.[Stock Unit])

drop table #tmpWHQty1,#tmpWHQty2

----------------------------------------------------------------
---------------- 08 WIP - 未WH關單------------------------------
----------------------------------------------------------------
/*特定日期區間*/
select * 
into #tmpIssueQty1
from (
    --台北採購的物料
	select  [HSCode] = isnull(f.HSCode,'')
	        , [NLCode] = isnull(f.NLCode,'')
            , t.FactoryID
	        , [ID] = t.ID
	        , [POID] = t.POID
	        , [Qty] = IIF((WH_Issue.Qty+WH15_16.Qty+WH17.Qty) != 0,dbo.getVNUnitTransfer(isnull(f.Type,'')
		                ,psd.StockUnit
		                ,isnull(f.CustomsUnit,'')
		                ,(WH_Issue.Qty+WH15_16.Qty+WH17.Qty)
		                ,isnull(f.Width,0)
		                ,isnull(f.PcsWidth,0)
		                ,isnull(f.PcsLength,0)
		                ,isnull(f.PcsKg,0)
		                ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',
			                (select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
			                ,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
		                ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',
			                (select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
			                ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),'')
                            ,default)
		                ,0)
            , f.Refno
            , [MaterialType] = dbo.GetMaterialTypeDesc(f.Type)
            , f.Description
            , [CustomsUnit] = f.CustomsUnit
            , [StockQty] = WH_Issue.Qty+WH15_16.Qty+WH17.Qty
            , [StockUnit] = psd.StockUnit
            , [StyleID] = t.StyleID
            , [Color] = isnull(c.Name,'')
	from #tmpOrderList t
	inner join MDivisionPoDetail mdp WITH (NOLOCK) on mdp.POID = t.ID 
	inner join PO_Supp_Detail psd WITH (NOLOCK) on mdp.POID = psd.ID 
                                                   and psd.SEQ1 = mdp.Seq1 
                                                   and psd.SEQ2 = mdp.Seq2
	left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
    left join Color c WITH (NOLOCK) on psd.BrandID = c.BrandID 
                                       and psd.ColorID = c.ID
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from Issue a
		inner join Issue_Detail b on a.Id=b.Id
		where b.PoId = mdp.POID and b.Seq1=mdp.seq1 and b.Seq2=mdp.Seq2
		and a.IssueDate <= @GenerateDate
		and a.Status='Confirmed'
		and a.Type in ('A','B','C','D','E','I')
	)WH_Issue
	outer apply(
		select Qty = isnull(sum(b.Qty),0) 
		from Issuelack a
		inner join Issuelack_Detail b on a.Id = b.Id
		where b.POID = psd.ID and b.Seq1 = psd.SEQ1 and b.Seq2 = psd.SEQ2
		and IssueDate <= @GenerateDate
		and a.Type='R' and a.FabricType in ('F','A')
		and a.Status !='New'
	)WH15_16
	outer apply(
		select Qty = - isnull(sum(b.Qty),0) 
		from IssueReturn a
		inner join IssueReturn_Detail b on a.Id = b.Id
		where b.POID = psd.ID and b.Seq1 = psd.SEQ1 and b.Seq2 = psd.SEQ2
		and IssueDate <= @GenerateDate
		and a.status ='Confirmed'
	)WH17
    where (t.WhseClose is null or t.WhseClose >= @GenerateDate)
	and exists(
		select 1 from Issue a
		inner join Issue_Detail b on a.Id = b.Id
		where b.POID = psd.ID and b.Seq1 = psd.SEQ1 and b.Seq2 = psd.SEQ2
		and IssueDate <= @GenerateDate
		and a.status = 'Confirmed'
		and a.Type in ('A','B','C','D','E','I')
		union all
		select 1 from Issuelack a
		inner join Issuelack_Detail b on a.Id = b.Id
		where b.POID = psd.ID and b.Seq1 = psd.SEQ1 and b.Seq2 = psd.SEQ2
		and IssueDate <= @GenerateDate
		and a.Type='R' and a.FabricType in ('F','A')
		and a.Status !='New'
		union all
		select 1 from IssueReturn a
		inner join IssueReturn_Detail b on a.Id = b.Id
		where b.POID = psd.ID and b.Seq1 = psd.SEQ1 and b.Seq2 = psd.SEQ2
		and IssueDate <= @GenerateDate
		and a.status ='Confirmed'
	)
    
    union all
    
    --工廠採購的物料
	select  [HSCode] = isnull(li.HSCode,'')
	        , [NLCode] = isnull(li.NLCode,'')
            , t.FactoryID
	        , [ID] = t.ID
	        , [POID] = t.POID
	        , [Qty] = IIF(WH61.Qty != 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
		                ,l.UnitId
		                ,isnull(li.CustomsUnit,'')
		                ,WH61.Qty
		                ,0
		                ,isnull(li.PcsWidth,0)
		                ,isnull(li.PcsLength,0)
		                ,isnull(li.PcsKg,0)
		                ,isnull(IIF(isnull(li.CustomsUnit,'') = 'M2',
			                (select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			                ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = isnull(li.CustomsUnit,''))),1)
		                ,isnull(IIF(isnull(li.CustomsUnit,'') = 'M2',
			                (select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
			                ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = isnull(li.CustomsUnit,''))),'')
                            ,li.Refno)
		                ,0)
            , li.Refno
            , [MaterialType] = dbo.GetMaterialTypeDesc(li.Category)
            , li.Description
            , [CustomsUnit] = li.CustomsUnit
            , [StockQty] = WH61.Qty
            , [StockUnit] = li.UnitId
            , [StyleID] = t.StyleID
            , [Color] = ''
	from #tmpOrderList t
	inner join LocalInventory l WITH (NOLOCK) on t.ID = l.OrderID 
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
	outer apply(
			select Qty = ISNULL(sum(b.Qty),0)
			from LocalIssue a
			inner join LocalIssue_Detail b on a.Id=b.Id
			where b.OrderID = l.OrderID
			and b.Refno = l.Refno and b.ThreadColorID = l.ThreadColorID
			and a.IssueDate <= @GenerateDate --特定日期 A, B 倉有收發紀錄的訂單
			and a.Status = 'Confirmed'
	)WH61
    where (t.WhseClose is null or t.WhseClose >= @GenerateDate)
	and exists(
		select * from LocalIssue a
		inner join LocalIssue_Detail b on a.Id = b.Id
		where b.OrderID = t.ID and b.Refno = l.Refno
		and IssueDate <= @GenerateDate
		and a.status = 'Confirmed'
	)
) a

select 
[HSCode] = a.HSCode
, [NLCode] = a.NLCode
, a.FactoryID
, [ID] = a.ID
, [POID] = a.POID
, [Qty] = sum(a.Qty) 
, a.Refno
, [MaterialType] = a.MaterialType
, a.Description
, [CustomsUnit] = a.CustomsUnit
, [StockQty] = sum(a.StockQty)
, [StockUnit] = a.StockUnit
, [StyleID] = a.StyleID
, [Color] = a.Color
into #tmpIssueQty
from #tmpIssueQty1 a
where a.StockQty  != 0
group by a.id, a.POID, a.FactoryID, a.Refno, a.Color, a.Description, a.NLCode, a.CustomsUnit, a.StockUnit,a.MaterialType,a.HSCode,a.StyleID

drop Table #tmpIssueQty1
----------------------------------------------------------------
-------- 09 WIP - 未WH關單 已SewingOutput數量 ------------------
----------------------------------------------------------------
---- 訂單尚未關倉 - 已Sewing數量 ----
select sdd.OrderId
		, t.POID
		, t.FactoryID
		, sdd.ComboType
		, sdd.Article
		, sdd.SizeCode
		, sdd.QAQty
		, v.HSCode
		, v.NLCode
		, v.UnitID
		, [Qty] = (ol_rate.value/100*sdd.QAQty)* (v.Qty * (1 + v.Waste))
		, v.CustomSP
        , v.Refno
        , [MaterialType] = iif(v.FabricType = 'L', li.Category,dbo.GetMaterialTypeDesc(v.FabricType))
        , [Description] = case 
                                when v.LocalItem = 1 then (select Description from LocalItem with (nolock) where Refno = v.Refno)
                                when v.FabricType = 'Misc' then (select Description from SciMachine_Misc with (nolock) where ID = v.Refno)
                                else (select Description from Fabric with (nolock) where SCIRefno = v.SCIRefno) 
                          end
        ,[CustomsUnit] = v.UnitID
        ,[StockQty] = (ol_rate.value/100*sdd.QAQty)* (v.StockQty * (1 + v.Waste))
        ,[StockUnit] = v.StockUnit
into #tmpSPNotCloseSewingOutput
from #tmpOrderList t
outer apply (
    select OrderId = sdd.OrderID
		    , sdd.ComboType
		    , sdd.Article
		    , sdd.SizeCode
		    , sdd.QAQty
    from #tmpSewingOutput_WHNotClose sdd
    where sdd.OrderID = t.ID
) sdd
outer apply(
	select value = dbo.GetOrderLocation_Rate(t.id, sdd.ComboType)
) ol_rate	
inner join #tmpCustomSP v on t.StyleID = v.StyleID
					         and t.BrandID = v.BrandID
					         and t.Category = v.Category
					         and t.SeasonID = v.SeasonID
					         and sdd.SizeCode = v.SizeCode
					         and sdd.Article = v.Article
left join LocalItem li with (nolock) on li.RefNo = v.RefNo
where (t.WhseClose is null or t.WhseClose >= @GenerateDate)
----------------------------------------------------------------
-------------03 WIP在製品(WIP Qty Detail) ----------------------
----------------------------------------------------------------
--組WIP明細
--先處理發料部分, 若要group by時其他欄位一樣,只有StockUnit不一樣, 則先轉換其中一個單位再sum起來
select 	
	POID	, HSCode	, NLCode	, Qty 	, Refno	, MaterialType	, Description	, CustomsUnit	, StockQty	, StockUnit	, FactoryID
	,rn=ROW_NUMBER()over(order by ID)
into #tmpIssueQty_forwip
from #tmpIssueQty 

select
	POID	, HSCode	, NLCode	, Qty 	, Refno	, MaterialType	, t2.Description	, CustomsUnit
	, StockQty=
		case when t.StockUnit <> t2.StockUnit and t.StockUnit like '%cone%' then
				StockQty * 
				isnull((select RateValue from dbo.View_Unitrate where FROM_U = t.StockUnit and TO_U = 'M')*
				(select RateValue from dbo.View_Unitrate where FROM_U = 'M' and TO_U = t2.StockUnit),1)
			when t.StockUnit <> t2.StockUnit then
				StockQty * 
				isnull((select RateValue from dbo.View_Unitrate where FROM_U = t.StockUnit and TO_U = t2.StockUnit),1)
			else
				StockQty
		end
	, StockUnit = t2.StockUnit
	, FactoryID	
into #tmpIssueQty_forwip2
from #tmpIssueQty_forwip t
outer apply(
	select top 1 t2.StockUnit,t2.Description 
	from #tmpIssueQty_forwip t2
	where 1=1
	and t.POID			= t2.POID
	and t.HSCode		= t2.HSCode
	and t.NLCode		= t2.NLCode
	and t.Refno			= t2.Refno
	and t.MaterialType	= t2.MaterialType
	and t.CustomsUnit	= t2.CustomsUnit
	and t.FactoryID		= t2.FactoryID
	order by t2.rn
)t2

select 	POID	, HSCode	, NLCode	, Qty     , Refno    , MaterialType    , Description    , CustomsUnit    , StockQty    , StockUnit    , FactoryID
	,rn=ROW_NUMBER()over(order by POID)
into #tmpSPNotCloseSewingOutput_forwip
from #tmpSPNotCloseSewingOutput 

select 	POID	, HSCode	, NLCode	, Qty     , Refno    , MaterialType    , t2.Description    , CustomsUnit
	, StockQty=
		case when t.StockUnit <> t2.StockUnit and t.StockUnit like '%cone%' then
				StockQty * 
				isnull((select RateValue from dbo.View_Unitrate where FROM_U = t.StockUnit and TO_U = 'M')*
				(select RateValue from dbo.View_Unitrate where FROM_U = 'M' and TO_U = t2.StockUnit),1)
			when t.StockUnit <> t2.StockUnit then
				StockQty * 
				isnull((select RateValue from dbo.View_Unitrate where FROM_U = t.StockUnit and TO_U = t2.StockUnit),1)
			else
				StockQty
		end
	, t2.StockUnit    , FactoryID
	,rn=ROW_NUMBER()over(order by POID)
into #tmpSPNotCloseSewingOutput_forwip2
from #tmpSPNotCloseSewingOutput_forwip t
outer apply(
	select top 1 t2.StockUnit,t2.Description 
	from #tmpSPNotCloseSewingOutput_forwip t2
	where 1=1
	and t.POID			= t2.POID
	and t.HSCode		= t2.HSCode
	and t.NLCode		= t2.NLCode
	and t.Refno			= t2.Refno
	and t.MaterialType	= t2.MaterialType
	and t.CustomsUnit	= t2.CustomsUnit
	and t.FactoryID		= t2.FactoryID
	order by t2.rn
)t2

select  [HSCode] = IIF(tw.HSCode is not null, tw.HSCode, ti.HSCode)
        , [NLCode] = IIF(tw.NLCode is not null, tw.NLCode, ti.NLCode)
        , [POID] = IIF(tw.POID is not null, tw.POID, ti.POID)
        , FactoryID=IIF(tw.FactoryID is not null, tw.FactoryID, ti.FactoryID)
        , [Refno] = IIF(tw.Refno is not null, tw.Refno, ti.Refno)   
        , [MaterialType] = IIF(tw.MaterialType is not null, tw.MaterialType, ti.MaterialType)   
        , [Description] = IIF(tw.Description is not null, tw.Description, ti.Description)   
        , [Qty] = isnull(ti.Qty,0) - isnull(tw.Qty,0)
        , [CustomsUnit] = IIF(tw.CustomsUnit is not null, tw.CustomsUnit, ti.CustomsUnit)
        , [StockQty] =
			case when tw.StockUnit is not null and ti.StockUnit like '%cone%'  then -- 資料以 Already Sewing Output(#tmpSPNotCloseSewingOutput) 為主(單位)
					isnull(ti.StockQty,0)*
					isnull((select RateValue from dbo.View_Unitrate where FROM_U = ti.StockUnit and TO_U = 'M')*
					(select RateValue from dbo.View_Unitrate where FROM_U = 'M' and TO_U = tw.StockUnit),1)
					-
					isnull(tw.StockQty,0)
				when tw.StockUnit is not null then
					isnull(ti.StockQty,0)*
					isnull((select RateValue from dbo.View_Unitrate where FROM_U = ti.StockUnit and TO_U = tw.StockUnit),1)
					-
					isnull(tw.StockQty,0)					
				when tw.StockUnit is null and tw.StockUnit like '%cone%' then
					isnull(ti.StockQty,0)
					-
					isnull(tw.StockQty,0)*
					isnull((select RateValue from dbo.View_Unitrate where FROM_U = tw.StockUnit and TO_U = 'M')*
					(select RateValue from dbo.View_Unitrate where FROM_U = 'M' and TO_U = ti.StockUnit),1)
				else
					isnull(ti.StockQty,0)
					-
					isnull(tw.StockQty,0)*
					isnull((select RateValue from dbo.View_Unitrate where FROM_U = tw.StockUnit and TO_U = ti.StockUnit),1)

			end
        , [StockUnit] = IIF(tw.StockUnit is not null,tw.StockUnit,ti.StockUnit)
into #tmpWIPDetail
from (
	select 	POID
			, HSCode
			, NLCode
			, SUM(Qty) as Qty 
            , Refno
            , MaterialType
            , Description
            , CustomsUnit
            , SUM(StockQty) as StockQty
            , StockUnit
            , FactoryID
	from #tmpIssueQty_forwip2 
	group by POID,HSCode,NLCode,Refno,MaterialType,Description,CustomsUnit,StockUnit,FactoryID
) ti
full outer 
join (
	select 	POID
			, HSCode
			, NLCode
			, SUM(Qty) as Qty 
            , Refno
            , MaterialType
            , Description
            , CustomsUnit
            , SUM(StockQty) as StockQty
            , StockUnit
            , FactoryID
	from #tmpSPNotCloseSewingOutput_forwip2 
	group by POID,HSCode,NLCode,Refno,MaterialType,Description,CustomsUnit,StockUnit,FactoryID
) tw on tw.POID = ti.POID 
     and ti.HSCode = tw.HSCode 
     and tw.NLCode = ti.NLCode 
     and tw.Refno = ti.Refno 
     and tw.MaterialType = ti.MaterialType 
     and ti.CustomsUnit = tw.CustomsUnit 
     and ti.FactoryID = tw.FactoryID
order by IIF(tw.POID is null,ti.POID,tw.POID)

----------------------------------------------------------------
--------- 04 Production成品庫存(Prod. Qty Detail) --------------
----------------------------------------------------------------
/*
    成品庫存數 = 產線產出 - 工廠出貨 + 台北調整出貨 - 工廠成衣數量調整 - 工廠報廢成衣

    Clog Garment Dispose 必須排除 Reason 00003 Transfer to new packing
    因為以前的訂單再轉 Shipmode 的時候都是直接再建立新的 Packing
    不會特別去刪除舊的 Packing
    因此工廠會將舊的 Packing 使用 Reason 00003 將其移除
*/
select tp.id 
        , tp.StyleID
        , tp.BrandID
        , tp.SeasonID
        , tp.Category
	    , [Article] = sdd.Article
	    , [SizeCode] = sdd.SizeCode
        , [ComboType] = sdd.ComboType
	    , [SewQty] = sdd.QaQty
	    , [PullQty] = isnull(PulloutDD.ShipQty, 0) + isnull (InvAdjustQ.DiffQty,0)
	    , [GMTAdjustQty] = isnull(AdjustGMT.gmtQty,0) + isnull (ClogDispose.DisposeQty, 0)
        , GarmentStock = GarmentStock.Qty
        , tp.FactoryID
into #tmpPreProdQty
from #tmpOrderList tp
left join #tmpSewingOutput_InFty sdd on sdd.OrderID = tp.ID
outer apply(
	select isnull(Sum(pdd.ShipQty),0) as ShipQty
	from Pullout_Detail_Detail pdd WITH (NOLOCK)
	inner join Pullout p with(nolock) on p.ID = pdd.ID
	where pdd.OrderID = tp.ID
            and pdd.Article = sdd.Article 
            and pdd.SizeCode = sdd.SizeCode
			and p.PulloutDate <= @GenerateDate
            and p.status !='New'
) PulloutDD
outer apply(
	select isnull(Sum(iaq.DiffQty),0) as DiffQty
	from InvAdjust ia WITH (NOLOCK)
	inner join InvAdjust_qty iaq WITH (NOLOCK) on ia.id=iaq.id 		
	where ia.OrderID = sdd.OrderId 
            and iaq.Article = sdd.Article 
            and iaq.SizeCode = sdd.SizeCode
			and ia.IssueDate <= @GenerateDate
) InvAdjustQ
outer apply(
	select isnull(Sum(agd.qty),0) as gmtQty
	from AdjustGMT ag WITH (NOLOCK)
	inner join AdjustGMT_Detail agd WITH (NOLOCK) on ag.id=agd.id
	where ag.Status='Confirmed'
		    and agd.orderid = sdd.orderid 
            and agd.Article = sdd.Article 
            and agd.SizeCode = sdd.SizeCode
			and ag.IssueDate <= @GenerateDate
) AdjustGMT	
outer apply (
    select	DisposeQty = isnull (sum (pld.ShipQty), 0)
    from ClogGarmentDispose cgd with (NoLock)
    inner join ClogGarmentDispose_Detail cgdd with (NoLock) on cgd.ID = cgdd.ID
    inner join PackingList_Detail pld with (NoLock) on cgdd.PackingListID = pld.ID
													    and cgdd.CTNStartNO = pld.CTNStartNO
    where cgd.ClogReasonID != '00003'
            and cgd.Status = 'Confirmed'
	        and sdd.OrderID = pld.OrderID
	        and sdd.Article = pld.Article
	        and sdd.SizeCode = pld.SizeCode
			and cgd.DisposeDate <= @GenerateDate
) ClogDispose
outer apply (
    select qty = sdd.QaQty - (isnull(PulloutDD.ShipQty, 0) + isnull (InvAdjustQ.DiffQty,0)) - (isnull(AdjustGMT.gmtQty,0) + isnull (ClogDispose.DisposeQty, 0))
) GarmentStock
where GarmentStock.Qty != 0

select  [HSCode] = v.HSCode
        , [NLCode] = v.NLCode
        , [SP#] = tpq.ID
        , tpq.FactoryID
        , [Refno] = v.Refno
        , [MaterialType] = iif(v.FabricType = 'L', li.Category,dbo.GetMaterialTypeDesc(v.FabricType))
        , [Description] = case   
                            when v.LocalItem = 1 then (select Description from LocalItem with (nolock) where Refno = v.Refno)
                            when v.FabricType = 'Misc' then (select Description from SciMachine_Misc with (nolock) where ID = v.Refno)
                            else (select Description from Fabric with (nolock) where SCIRefno = v.SCIRefno) 
                          end
        , [Custom SP#] = v.CustomSP
        , [Article] = tpq.Article
        , [SizeCode] = tpq.SizeCode
        , [ComboType] = tpq.ComboType
        , [SewQty] = tpq.SewQty
        , [PullOutQty] = tpq.PullQty
        , [GMTAdjustQty] = tpq.GMTAdjustQty
        , [Qty]  = (ol_rate.value / 100 * tpq.GarmentStock) * (v.Qty * (1 + v.Waste))
        , [Customs Unit] = v.UnitID	
        , [StockQty] = (ol_rate.value / 100 * tpq.GarmentStock) * (v.StockQty * (1 + v.Waste))
        , [StockUnit] = v.StockUnit
into #tmpProdQty
from #tmpPreProdQty tpq
outer apply (
	select value = dbo.GetOrderLocation_Rate(tpq.id, tpq.ComboType)
) ol_rate
inner join #tmpCustomSP v on tpq.StyleID = v.StyleID
					         and tpq.BrandID = v.BrandID
					         and tpq.Category = v.Category
					         and tpq.SeasonID = v.SeasonID
					         and tpq.SizeCode = v.SizeCode
					         and tpq.Article = v.Article
left join LocalItem li with (nolock) on li.RefNo = v.RefNo

----------------------------------------------------------------
----- 05 在途成品(已報關但還沒出貨) (On Road Product Qty)-------
----------------------------------------------------------------

-- 取得Shipping P41有資料, 但Pullout沒資料 (或ShipQty=0)
select * 
into #tmpPull
from (
    select a.ID 
    from VNExportDeclaration a
    where not exists (
                select 1 
                from pullout_detail pd
				inner join Pullout p on p.ID=pd.ID
	            where invno=a.invno
				and p.PulloutDate <= @GenerateDate -- 『特定日期（含當天）』前成衣尚未出貨
				and p.Status !='New'
    )

    union 

    select a.ID 
    from VNExportDeclaration a
    where exists (
        select 1 
        from pullout_detail pd
		inner join Pullout p on p.ID=pd.ID
	    where invno = a.invno 
        and shipqty = 0
		and p.PulloutDate <= @GenerateDate -- 『特定日期（含當天）』前成衣尚未出貨
		and p.Status !='New'
    )
) a

-- 取得最大值customsp
select max(vdd.customsp)customsp,vd.id,vc.sizecode
into #tmpmax
from VNExportDeclaration vd WITH (NOLOCK)
inner join VNExportDeclaration_Detail vdd WITH (NOLOCK) on vd.id=vdd.id
inner join VNConsumption vc WITH (NOLOCK) on vc.StyleID = vdd.StyleID 
                                                and vc.BrandID=vdd.BrandID
		                                        and vc.SeasonID=vdd.SeasonID 
                                                and vc.category=vdd.category 
		                                        and vc.sizecode=vdd.sizecode 
                                                and vc.customsp=vdd.customsp
where vd.VNContractID=@contract
group by vd.id,vc.sizecode

select [SP#] = vdd.OrderId
        , o.FactoryID
        , [Refno] = vcdd.Refno
        , [MaterialType] = iif(vcdd.FabricType = 'L', li.Category,dbo.GetMaterialTypeDesc(vcdd.FabricType))
        , [Description] =   case 
                                when vcdd.LocalItem = 1 then (select Description from LocalItem with (nolock) where Refno = vcdd.Refno)
                                when vcdd.FabricType = 'Misc' then (select Description from SciMachine_Misc with (nolock) where ID = vcdd.Refno)
                                else (select Description from Fabric with (nolock) where SCIRefno = vcdd.SCIRefno) 
                            end
        , [CustomSP] = vdd.customsp
        , [Article] = vdd.Article
        , [SizeCode] = vdd.SizeCode
        , [NLCode] = vcd.NlCode
        , [HSCode] = vcd.HSCode
        , [Qty] = vdds.ExportQty * (vcdd.Qty * (1+vcd.Waste))
        , [Unit] = vcdd.UnitID
        , [StockQty] =  vdds.ExportQty * (vcdd.StockQty * (1+vcd.Waste))
        , [StockUnit] = vcdd.StockUnit
INTO #OnRoadProductQty
from VNExportDeclaration vd WITH (NOLOCK)
inner join VNExportDeclaration_Detail vdd WITH (NOLOCK) on vd.id=vdd.id
inner join VNConsumption vc on vc.StyleID = vdd.StyleID 
                                and vc.BrandID=vdd.BrandID
							    and vc.SeasonID=vdd.SeasonID 
                                and vc.category=vdd.category 
							    and vc.sizecode=vdd.sizecode 
                                and vc.customsp=vdd.customsp
inner join VNConsumption_Detail vcd WITH (NOLOCK) on vcd.ID=vc.ID
inner join VNConsumption_Detail_Detail vcdd WITH (NOLOCK) on vcd.ID= vcdd.ID 
                                                             and vcd.NLCode = vcdd.NLCode
inner join orders o WITH (NOLOCK) on o.id=vdd.OrderId
left join LocalItem li with (nolock) on li.RefNo = vcdd.RefNo
outer apply (
	select sum(ExportQty) as ExportQty 
	from VNExportDeclaration_Detail WITH (NOLOCK)
	where id = vdd.id 
          and Article = vdd.Article 
          and sizecode=vdd.sizecode
) vdds
inner join #tmpmax tm on vd.id = tm.id 
                         and tm.customsp = vdd.customsp 
                         and vdd.sizecode=tm.sizecode
where   vd.status='Confirmed'
        and vd.VNContractID=@contract
        and exists(
	        select 1 
            from #tmpPull where ID = vd.id
        )
		and vd.CDate <= @GenerateDate --『特定日期（含當天）』前完成出口報關
        {whereftys}


----------------------------------------------------------------
------------- 06 料倉(C) (Scrap Qty Detail ---------------------
----------------------------------------------------------------
--撈Scrap資料
select * 
into #tmpScrapQty1
from (
	select  [HSCode] = isnull(f.HSCode,'')
	        , [NLCode] = isnull(f.NLCode,'')
	        , [POID] = ft.POID
            , o.FactoryID
	        , [Seq] = (ft.Seq1+'-'+ft.Seq2)
	        , [Refno] = psd.Refno	
            , [MaterialType] = dbo.GetMaterialTypeDesc(f.Type)
	        , [Description] = isnull(f.Description,'')
	        , [Roll] = ft.Roll
	        , [Dyelot] = ft.Dyelot
	        , [StockType] = ft.StockType
	        , [Location] = ftd.MtlLocationID		
	        , [Qty] = IIF(ft.InQty-ft.OutQty+ft.AdjustQty != 0,dbo.getVNUnitTransfer(isnull(f.Type,'')
			        ,psd.StockUnit
			        ,isnull(f.CustomsUnit,'')
			        ,ft.InQty-ft.OutQty+ft.AdjustQty
			        ,isnull(f.Width,0)
			        ,isnull(f.PcsWidth,0)
			        ,isnull(f.PcsLength,0)
			        ,isnull(f.PcsKg,0)
			        ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',
				        (select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
			        ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2'
				        ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),'')
                        ,default),0)
	        , [CustomsUnit] = isnull(f.CustomsUnit,'')
	        , [ScrapQty] = ft.InQty-ft.OutQty+ft.AdjustQty
	        , [StockUnit] = psd.StockUnit
	from FtyInventory ft WITH (NOLOCK) 
	left join FtyInventory_detail ftd WITH (NOLOCK) on ft.ukey=ftd.ukey	
	inner join PO_Supp_Detail psd WITH (NOLOCK) on ft.POID = psd.ID 
                                                    and psd.SEQ1 = ft.Seq1 
                                                    and psd.SEQ2 = ft.Seq2
	inner join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
    inner join orders o WITH (NOLOCK) on o.id=ft.POID
	where 1=1 
            and ft.StockType='O'
			and ft.InQty-ft.OutQty+ft.AdjustQty != 0
            {whereftys}

    union all
	
    select  [HSCode] = isnull(li.HSCode,'')
	        , [NLCode] = isnull(li.NLCode,'')
	        , [POID] = l.OrderID
            , o.FactoryID
	        , [Seq] = ''
	        , [Refno] = l.Refno	
            , [MaterialType] = dbo.GetMaterialTypeDesc(li.Category)
	        , [Description] = isnull(li.Description,'')
	        , [Roll] = ''
	        , [Dyelot] = ''
	        , [StockType] = 'O'
	        , [Location] = l.CLocation		
	        , [Qty] = IIF(l.LobQty != 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
			        ,l.UnitId,li.CustomsUnit
			        ,l.LobQty
			        ,0
			        ,li.PcsWidth
			        ,li.PcsLength
			        ,li.PcsKg
			        ,isnull(IIF(li.CustomsUnit = 'M2',
				        (select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
				        ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),1)
			        ,isnull(IIF(li.CustomsUnit = 'M2',
				        (select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
				        ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),'')
                        ,li.Refno),0)
	        , [CustomsUnit] = isnull(li.CustomsUnit,'')
	        , [ScrapQty] = l.LobQty
	        , [StockUnit] = l.UnitID
	from LocalInventory l WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = l.OrderID
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo
	where 1=1
	and l.LobQty != 0
    {whereftys}
) a

/*取得區間資料*/
select * 
into #tmpScrapQty2
from (
	select  [HSCode] = isnull(f.HSCode,'')
	        , [NLCode] = isnull(f.NLCode,'')
	        , [POID] = ft.POID
            , o.FactoryID
	        , [Seq] = (ft.Seq1+'-'+ft.Seq2)
	        , [Refno] = psd.Refno	
            , [MaterialType] = dbo.GetMaterialTypeDesc(f.Type)
	        , [Description] = isnull(f.Description,'')
	        , [Roll] = ft.Roll
	        , [Dyelot] = ft.Dyelot
	        , [StockType] = ft.StockType
	        , [Location] = ftd.MtlLocationID		
	        , [Qty] = IIF((WH43.Qty + WH24_25.Qty + WH36.Qty + WH45.Qty) != 0,dbo.getVNUnitTransfer(isnull(f.Type,'')
			        ,psd.StockUnit
			        ,isnull(f.CustomsUnit,'')
			        ,WH43.Qty + WH24_25.Qty + WH36.Qty + WH45.Qty
			        ,isnull(f.Width,0)
			        ,isnull(f.PcsWidth,0)
			        ,isnull(f.PcsLength,0)
			        ,isnull(f.PcsKg,0)
			        ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2',
				        (select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),1)
			        ,isnull(IIF(isnull(f.CustomsUnit,'') = 'M2'
				        ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M')
				        ,(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = isnull(f.CustomsUnit,''))),'')
                        ,default),0)
	        , [CustomsUnit] = isnull(f.CustomsUnit,'')
	        , [ScrapQty] = WH43.Qty + WH24_25.Qty + WH36.Qty + WH45.Qty
	        , [StockUnit] = psd.StockUnit
	from FtyInventory ft WITH (NOLOCK) 
	left join FtyInventory_detail ftd WITH (NOLOCK) on ft.ukey=ftd.ukey	
	inner join PO_Supp_Detail psd WITH (NOLOCK) on ft.POID = psd.ID 
                                                    and psd.SEQ1 = ft.Seq1 
                                                    and psd.SEQ2 = ft.Seq2
	inner join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
    inner join orders o WITH (NOLOCK) on o.id=ft.POID
	outer apply(
		select [Qty]= - isnull(sum(b.QtyAfter-b.QtyBefore),0) 
		from Adjust a
		inner join Adjust_Detail b on a.ID=b.ID
		where b.FtyInventoryUkey = ft.Ukey
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
		and a.Type in ('O')
		and a.Status = 'Confirmed'		
	)WH43
	outer apply(
		select [Qty]= isnull(sum(b.QtyAfter-b.QtyBefore) ,0)
		from Adjust a
		inner join Adjust_Detail b on a.ID=b.ID
		where b.FtyInventoryUkey = ft.Ukey
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
		and a.Type in ('R')
		and a.Status = 'Confirmed'		
	)WH45
	outer apply(
		select [Qty] = - isnull(sum(b.Qty),0) from SubTransfer a
		inner join SubTransfer_Detail b on a.ID=b.ID
		where b.ToPOID = ft.POID and b.ToSeq1=ft.seq1 and b.ToSeq2=ft.Seq2
		and b.ToStockType = ft.StockType and b.ToRoll = ft.Roll and b.ToDyelot = ft.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
		and a.Status = 'Confirmed'
		and a.Type in ('D','E')
	)WH24_25
	outer apply(
		select [Qty] = isnull(sum(b.Qty),0) from SubTransfer a
		inner join SubTransfer_Detail b on a.ID=b.ID
		where b.FromPOID = ft.POID and b.FromSeq1=ft.seq1 and b.FromSeq2=ft.Seq2
		and b.FromStockType = ft.StockType and b.FromRoll = ft.Roll and b.FromDyelot = ft.Dyelot
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
		and a.Status = 'Confirmed'
		and a.Type in ('C')
	)WH36
	where 1=1 
            and ft.StockType='O'
            and (WH43.Qty + WH24_25.Qty + WH36.Qty + WH45.Qty) != 0
			and exists(
				select 1 from Adjust a
				inner join Adjust_Detail b on a.ID=b.ID
				where b.FtyInventoryUkey = ft.Ukey
				and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
				and a.Type in ('O','R')
				and a.Status = 'Confirmed'
				union all
				select 1 from SubTransfer a
				inner join SubTransfer_Detail b on a.ID=b.ID
                where b.ToPOID = ft.POID and b.ToSeq1=ft.seq1 and b.ToSeq2=ft.Seq2
		        and b.ToStockType = ft.StockType and b.ToRoll = ft.Roll and b.ToDyelot = ft.Dyelot
		        and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
				and a.Status = 'Confirmed'
				and a.Type in ('C','D','E')
			)
            {whereftys}

    union all
	
    select  [HSCode] = isnull(li.HSCode,'')
	        , [NLCode] = isnull(li.NLCode,'')
	        , [POID] = l.OrderID
            , o.FactoryID
	        , [Seq] = ''
	        , [Refno] = l.Refno	
            , [MaterialType] = dbo.GetMaterialTypeDesc(li.Category)
	        , [Description] = isnull(li.Description,'')
	        , [Roll] = ''
	        , [Dyelot] = ''
	        , [StockType] = 'O'
	        , [Location] = l.CLocation		
	        , [Qty] = IIF((WH36.Qty + wh44.Qty + WH46.Qty) != 0,dbo.getVNUnitTransfer(isnull(li.Category,'')
			        ,l.UnitId,li.CustomsUnit
			        ,WH36.Qty + wh44.Qty + WH46.Qty
			        ,0
			        ,li.PcsWidth
			        ,li.PcsLength
			        ,li.PcsKg
			        ,isnull(IIF(li.CustomsUnit = 'M2',
				        (select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
				        ,(select RateValue from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),1)
			        ,isnull(IIF(li.CustomsUnit = 'M2',
				        (select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = 'M')
				        ,(select Rate from dbo.View_Unitrate where FROM_U = IIF(l.UnitId = 'CONE','M',l.UnitId) and TO_U = li.CustomsUnit)),'')
                        ,li.Refno),0)
	        , [CustomsUnit] = isnull(li.CustomsUnit,'')
	        , [ScrapQty] = WH36.Qty + wh44.Qty + WH46.Qty
	        , [StockUnit] = l.UnitID
	from LocalInventory l WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = l.OrderID
	left join LocalItem li WITH (NOLOCK) on l.Refno = li.RefNo	
	outer apply(
		select  [Qty] = isnull(sum(b.Qty ),0) 
		from SubTransferLocal a
		inner join SubTransferLocal_Detail b on a.ID=b.ID
		where b.PoId = l.OrderID and b.Refno = l.Refno and b.Color = l.ThreadColorID 
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
		and a.Status = 'Confirmed'
		and a.Type='D'
	)WH36
	outer apply(
		select [Qty] = - isnull(sum(b.QtyAfter) - sum(b.QtyBefore),0)
		from AdjustLocal a
		inner join AdjustLocal_Detail b on a.ID=b.ID
		where b.PoId = l.OrderID and b.Refno = l.Refno and b.Color = l.ThreadColorID 
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
		and a.Status = 'Confirmed'
		and a.Type in ('C')
	)WH44
	outer apply(
		select [Qty] = isnull(sum(b.QtyAfter) - sum(b.QtyBefore),0)
		from AdjustLocal a
		inner join AdjustLocal_Detail b on a.ID=b.ID
		where b.PoId = l.OrderID and b.Refno = l.Refno and b.Color = l.ThreadColorID 
		and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
		and a.Status = 'Confirmed'
		and a.Type in ('R')
	)WH46
	where 1=1
	      and (WH36.Qty + wh44.Qty + WH46.Qty) != 0	
		  and exists(
				select 1 from SubTransferLocal a
				inner join SubTransferLocal_Detail b on a.ID=b.ID
				where b.PoId = l.OrderID and b.Refno = l.Refno and b.Color = l.ThreadColorID 
				and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
				and a.Status = 'Confirmed'
				and a.Type='D'
				union all 
				select 1 from AdjustLocal a
				inner join AdjustLocal_Detail b on a.ID=b.ID
				where b.PoId = l.OrderID and b.Refno = l.Refno and b.Color = l.ThreadColorID 
				and a.IssueDate > @GenerateDate and a.IssueDate <= GETDATE() -- 特定日期到今天 C 倉有收發紀錄的訂單
				and a.Status = 'Confirmed'
				and a.Type in ('R','C')
			)
            {whereftys}
) a

select 
HSCode = isnull(a.HSCode,b.HSCode)
, NLCode = isnull(a.NLCode,b.NLCode)
, POID = isnull(a.POID,b.POID)
, FactoryID = isnull(a.FactoryID,b.FactoryID)
, Seq = isnull(a.Seq,b.Seq)
, Refno = isnull(a.Refno,b.Refno)
, MaterialType = isnull(a.MaterialType,b.MaterialType)
, [Description] = isnull(a.Description,b.Description)
, Roll = isnull(a.Roll,b.Roll)
, Dyelot = isnull(a.Dyelot,b.Dyelot)
, StockType = isnull(a.StockType,b.StockType)
, Location = isnull(a.Location,b.Location)
, [Qty] = sum(isnull(a.Qty,0) + isnull(b.Qty,0))
, CustomsUnit = isnull(a.CustomsUnit ,b.CustomsUnit)
, [ScrapQty] = sum(isnull(a.ScrapQty,0) + isnull(b.ScrapQty,0))
, StockUnit = isnull(a.StockUnit,b.StockUnit)
into #tmpScrapQty
from #tmpScrapQty1 a
full outer join #tmpScrapQty2 b on a.POID = b.POID
and a.FactoryID = b.FactoryID and a.Seq=b.Seq 
and a.Roll = b.Roll and a.Dyelot = b.Dyelot
and a.Refno = b.Refno  and a.MaterialType = b.MaterialType
and a.StockType = b.StockType and a.Location = b.Location
and a.CustomsUnit = b.CustomsUnit and a.StockUnit = b.StockUnit
where isnull(a.ScrapQty,0) + isnull(b.ScrapQty,0) != 0
group by isnull(a.HSCode,b.HSCode),isnull(a.NLCode,b.NLCode),isnull(a.POID,b.POID),isnull(a.FactoryID,b.FactoryID),isnull(a.Seq,b.Seq)
,isnull(a.Refno,b.Refno),isnull(a.MaterialType,b.MaterialType),isnull(a.Description,b.Description),isnull(a.Roll,b.Roll),isnull(a.Dyelot,b.Dyelot)
,isnull(a.StockType,b.StockType),isnull(a.Location,b.Location),isnull(a.CustomsUnit ,b.CustomsUnit),isnull(a.StockUnit,b.StockUnit)

drop table #tmpScrapQty1,#tmpScrapQty2
----------------------------------------------------------------
----------------- 07 Outstanding List --------------------------
----------------------------------------------------------------
/*
    撈出 Sewing Output 對應不到VNConsumption的資料

    WIP 與 Prod. 差別
        WIP : 將所有尚未關倉的產出全部列出
        Prod. : 只列出尚有庫存的訂單

    因此 2 邊資料都必須判斷
*/
select  o.ID
		, o.POID 
        , o.FactoryID
		, o.MDivisionID
        , Category = IIF (o.Category = 'G', fromSP.Category, o.Category)
        , StyleID = IIF (o.Category = 'G', fromSP.StyleID, o.StyleID)
        , BrandID = IIF (o.Category = 'G', fromSP.BrandID, o.BrandID)
        , SeasonID = IIF (o.Category = 'G', fromSP.SeasonID, o.SeasonID)
        , StyleUKey = IIF (o.Category = 'G', fromSP.StyleUKey, o.StyleUKey)
		, OriCategory = o.Category
        , OriStyleID = o.StyleID
		, OriBrandID = o.BrandID
		, OriSeasonID = o.SeasonID
		, OriStyleUKey = o.StyleUKey
        , o.WhseClose
into #tmpOrderListAll 
from Orders o  WITH (NOLOCK) 
outer apply (
    select top 1
            gmo.Category
            , gmo.StyleID
            , gmo.BrandID
            , gmo.SeasonID
            , gmo.StyleUKey
    from Orders gmo WITH (NOLOCK)
    where   o.Category = 'G'
            and exists (
                select 1
                from Order_Qty_Garment oqg WITH (NOLOCK) 
                where o.id = oqg.id
                      and gmo.ID = oqg.OrderIDFrom
            )
			and gmo.WhseClose >= @GenerateDate
) fromSP
where   o.Category <>''
        and (   
            ---- 訂單尚未關倉
            o.WhseClose is null

            -- Bulk, Sample, Garment 訂單雖然已經關單但是短出 Shortage
            or (o.GMTComplete = 'S')

            -- FOC 訂單生產完後會先放在倉庫等日後才會做出貨
            or exists (
                select 1
                from Order_Finish orf With (NoLock)
                where o.id = orf.id
            )
			or o.WhseClose >= @GenerateDate --訂單的關單日在『特定日期（含當天）』之後
        )
        and o.Qty<>0
        and o.LocalOrder = 0 

select  t.ID
		, t.FactoryID
		, t.OriStyleID
		, t.OriBrandID
		, t.OriSeasonID
		, sdd.Article 
		, sdd.Sizecode
        , sdd.ComboType
		, sdd.WIPQaQty
        , sdd.GarmentStock
into  #tmpOutstanding 
from #tmpOrderListAll t
outer apply (
    select  Article = iif (tpq.Article is not null, tpq.Article, tsow.Article)
            , SizeCode = iif (tpq.SizeCode is not null, tpq.SizeCode, tsow.SizeCode)
            , ComboType = iif (tpq.ComboType is not null, tpq.ComboType, tsow.ComboType)
            , WIPQaQty = isnull (tsow.QaQty, 0)
            , GarmentStock = isnull (tpq.GarmentStock, 0)
    from (
        select tpq.Article
                , tpq.SizeCode
                , tpq.ComboType
                , tpq.GarmentStock
        from #tmpPreProdQty tpq
        where t.id = tpq.id
    ) tpq
    full outer join (
        select tsow.Article
                , tsow.SizeCode
                , tsow.ComboType
                , tsow.QaQty
        from #tmpSewingOutput_WHNotClose tsow
        where t.id = tsow.OrderID
    ) tsow on tpq.ComboType = tsow.ComboType 
              and tpq.Article = tsow.Article
              and tpq.SizeCode = tsow.SizeCode
) sdd
where   not exists (
            select 1
            from #tmpCustomSP v
            where t.StyleID = v.StyleID
				    and t.BrandID = v.BrandID
				    and t.Category = v.Category
				    and t.SeasonID = v.SeasonID
				    and sdd.SizeCode = v.SizeCode
				    and sdd.Article = v.Article
        )

----------------------------------------------------------------
------------------------整理出Summary --------------------------
----------------------------------------------------------------
select  [HSCode] = isnull(tc.HSCode,'')
        , [NLCode] =  a.NLCode
        , [Description] = isnull(vcd.DescEN,'')
        , [UnitID] = isnull(tc.UnitID,'')
        , [EcusQty] = isnull(cq.StockQty,0)
        , [LiqQty] = isnull(tc.Qty,0) + isnull(td.Qty,0) --調整與勾選Liquidation data only相同
        , [OnRoadMaterialQty] = isnull(orm.Qty,0)
        , [WHQty] = isnull(tw.Qty,0)
        , [WIPQty] = isnull(ti.Qty,0)
        , [ProdQty] = isnull(tp.Qty,0)
        , [OnRoadProductQty] = isnull(orp.Qty,0)
        , [ScrapQty] = isnull(ts.Qty,0)
        , [Total] = isnull(orm.Qty,0) + isnull(tw.Qty,0) + isnull(ti.Qty,0) + isnull(tp.Qty,0) - isnull(orp.Qty,0) + isnull(ts.Qty,0)
from (
	select 	NLCode 
	from #tmpContract 

	union 
	select	NLCode 
	from #tmpDeclare 

	--1)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpOnRoadMaterial
	) t 
	--2)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpWHQty
	) t 
	--3)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpWIPDetail
	) t 
	--4)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpProdQty
	) t 
	--5)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #OnRoadProductQty
	) t 
	--6)
	union 
	select 	NLCode 
	from (
		select 	Distinct NLCode 
		from #tmpScrapQty
	) t
) a
left join #tmpContract tc on a.NLCode = tc.NLCode
left join VNNLCodeDesc vcd WITH (NOLOCK) on a.NLCode = vcd.NLCode
left join #tmpDeclare td on a.NLCode = td.NLCode
left join #CusQty cq on a.NLCode = cq.NLCode
left join (
	select 	NLCode
			,SUM(Qty) as Qty 
	from #tmpOnRoadMaterial 
	group by NLCode
) orm on a.NLCode = orm.NLCode
left join (
	select 	NLCode
			,SUM(Qty) as Qty 
	from #tmpWHQty 
	group by NLCode
) tw on a.NLCode = tw.NLCode
left join (
	select 	NLCode
			,SUM(Qty) as Qty 
	from #tmpWIPDetail 
    where Qty != 0
	group by NLCode
) ti on a.NLCode = ti.NLCode
left join (
	select 	NLCode
			,sum(Qty) as Qty 
	from #tmpProdQty 
	group by NLCode
) tp on a.NLCode = tp.NLCode
left join (
	select 	NLCode
			,sum(Qty) as Qty 
	from #OnRoadProductQty 
	group by NLCode
) orp on a.NLCode = orp.NLCode
left join (
	select 	NLCode
			,sum(Qty) as Qty 
	from #tmpScrapQty 
	group by NLCode
) ts on a.NLCode = ts.NLCode
where 1 = 1 ");

                if (!MyUtility.Check.Empty(this.hscode))
                {
                    sqlCmd.Append(string.Format(" and tc.HSCode = '{0}'", this.hscode));
                }

                if (!MyUtility.Check.Empty(this.nlcode))
                {
                    sqlCmd.Append(string.Format(" and a.NLCode = '{0}'", this.nlcode));
                }

                sqlCmd.Append(string.Format(
                    @"                                                                                                       
order by TRY_CONVERT(int, SUBSTRING(a.NLCode, 3, LEN(a.NLCode))), a.NLCode

--1)在途物料
select * 
from #tmpOnRoadMaterial 
where Qty != 0  {0} {1} 
order by POID,Seq

--2)W/H明細
select * 
from #tmpWHQty 
where Qty != 0 {0} {1} 
order by POID,Seq

--3)WIP明細
select * 
from #tmpWIPDetail 
where Qty != 0 {0} {1} 
order by POID

--4)Prod明細
select * 
from #tmpProdQty 
where Qty != 0 {0} {1} 
order by SP#, Article, SizeCode, ComboType

--5)在途成品
select * 
from #OnRoadProductQty 
where Qty != 0 {0} {1} 
order by SP#,Article,SizeCode

--6)Scrap明細
select * 
from #tmpScrapQty 
where Qty != 0 {0} {1} 
order by POID,Seq

--7)Outstanding List 
select * 
from #tmpOutstanding 
where WIPQaQty != 0 or GarmentStock != 0
order by ID, Article, SizeCode, ComboType

-- 8) 未WH關單
select StyleID
        , POID
        , FactoryID
        , Refno
        , Color
        , Description
        , NLCode
        , Qty = sum (Qty)
        , CustomsUnit
        , StockQty = sum (StockQty)
        , StockUnit
from #tmpIssueQty
where Qty != 0  {0} {1} 
group by StyleID, POID, FactoryID, Refno, Color, Description, NLCode, CustomsUnit, StockUnit
order by POID

-- 9) 已SewingOutput數量
select	HSCode
		, NLCode
		, OrderID
		, FactoryID
        , Article
        , SizeCode
        , ComboType
		, Refno
		, MaterialType
		, Description
		, Qty
		, CustomsUnit
		, StockQty
		, StockUnit
from #tmpSPNotCloseSewingOutput
where Qty != 0  {0} {1} 
order by OrderID, Article, SizeCode, ComboType

drop table  #tmpContract
            , #tmpDeclare
            , #tmpOrderList
            , #tmpOrderListAll
            , #tmpCustomSP
            , #tmpSewingOutput_WHNotClose
            , #tmpSewingOutput_InFty
            , #tmpOnRoadMaterial
            , #tmpWHQty
            , #tmpIssueQty
            , #tmpSPNotCloseSewingOutput
            , #tmpWIPDetail
            , #tmpPreProdQty
            , #tmpProdQty
            , #tmpPull
            , #tmpmax
            , #OnRoadProductQty
            , #tmpScrapQty
            , #tmpOutstanding 
			, #tmpIssueQty_forwip
			,#tmpIssueQty_forwip2
			,#tmpSPNotCloseSewingOutput_forwip
			,#tmpSPNotCloseSewingOutput_forwip2
			
", MyUtility.Check.Empty(this.hscode) ? string.Empty : string.Format("and HSCode = '{0}'", this.hscode),
                    MyUtility.Check.Empty(this.nlcode) ? string.Empty : string.Format("and NLCode = '{0}'", this.nlcode)));
                #endregion
            }
            #endregion

            DataTable[] allData;
            DBProxy.Current.DefaultTimeout = 12000;  // 加長時間為 2 小時，避免timeout

            DualResult queryResult = MyUtility.Tool.ProcessWithDatatable(this.dtImportEcusData, string.Empty, sqlCmd.ToString(), out allData, temptablename: "#CusQty");
            if (!queryResult)
            {
                return queryResult;
            }

            this.Summary = allData[0];

            if (!this.liguidationonly)
            {
                this.OnRoadMaterial = allData[1];
                this.WHDetail = allData[2];
                this.WIPDetail = allData[3];
                this.ProdDetail = allData[4];
                this.OnRoadProduction = allData[5];
                this.ScrapDetail = allData[6];
                this.Outstanding = allData[7];
                this.WarehouseNotClose = allData[8];
                this.AlreadySewingOutput = allData[9];
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.Summary.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.Summary.Rows.Count + (this.liguidationonly ? 0 : this.OnRoadMaterial.Rows.Count + this.WHDetail.Rows.Count + this.WIPDetail.Rows.Count + this.ProdDetail.Rows.Count + this.ScrapDetail.Rows.Count + this.OnRoadProduction.Rows.Count + this.Outstanding.Rows.Count));

            this.ShowWaitMessage("Starting EXCEL...");
            this.ShowWaitMessage("Starting EXCEL...Summary");
            string filename = string.Empty;
            string ftys = string.Join(",", this.FactoryList);
            if (!this.liguidationonly)
            {
                filename = "Shipping_R40_Summary.xltx";
                Excel.Application excelSummary = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                Utility.Report.ExcelCOM comSummary = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelSummary);
                comSummary.ColumnsAutoFit = true;
                comSummary.WriteTable(this.Summary, 3);

                Excel.Worksheet worksheetSummary = excelSummary.ActiveWorkbook.Worksheets[1];   // 取得工作表
                worksheetSummary.Cells[1, 1] = "Summary-" + this.contract + "(" + ftys + ")";
                this.SaveExcelwithName(excelSummary, "Summary");

                if (this.OnRoadMaterial.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...OnRoadMaterial List");
                    filename = "Shipping_R40_OnRoadMaterial.xltx";
                    Excel.Application excelOnRoadMaterial = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comOnRoadMaterial = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelOnRoadMaterial);
                    comOnRoadMaterial.ColumnsAutoFit = true;
                    comOnRoadMaterial.WriteTable(this.OnRoadMaterial, 3);

                    Excel.Worksheet worksheetOnRoadMaterial = excelOnRoadMaterial.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetOnRoadMaterial.Cells[1, 1] = "On Road Material-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelOnRoadMaterial, "On Road Material");
                }

                if (this.WHDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...WHouse Qty Detail");
                    filename = "Shipping_R40_WHQtyDetail.xltx";
                    Excel.Application excelWHDetail = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comWHDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelWHDetail);
                    comWHDetail.ColumnsAutoFit = true;
                    comWHDetail.WriteTable(this.WHDetail, 3);

                    Excel.Worksheet worksheetWHDetail = excelWHDetail.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetWHDetail.Cells[1, 1] = "WHouse Qty Detail-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelWHDetail, "WHouse Qty Detail");
                }

                if (this.WIPDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...WIP Qty Detail");
                    filename = "Shipping_R40_WIPQtyDetail.xltx";
                    Excel.Application excelWIP = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comWIP = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelWIP);
                    comWIP.ColumnsAutoFit = true;
                    comWIP.WriteTable(this.WIPDetail, 3);

                    Excel.Worksheet worksheetWIP = excelWIP.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetWIP.Cells[1, 1] = "WIP Qty Detail-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelWIP, "WIP Qty Detail");
                }

                if (this.ProdDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Prod. Qty Detail");
                    filename = "Shipping_R40_ProdQtyDetail.xltx";
                    Excel.Application excelProdDetail = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comProdDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelProdDetail);
                    comProdDetail.ColumnsAutoFit = true;
                    comProdDetail.WriteTable(this.ProdDetail, 3);

                    Excel.Worksheet worksheetProdDetail = excelProdDetail.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetProdDetail.Cells[1, 1] = "Prod. Qty Detail-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelProdDetail, "Prod. Qty Detail");
                }

                if (this.ScrapDetail.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Scrap Qty Detail");
                    filename = "Shipping_R40_ScrapQtyDetail.xltx";
                    Excel.Application excelScrapDetail = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comScrapDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelScrapDetail);
                    comScrapDetail.ColumnsAutoFit = true;
                    comScrapDetail.WriteTable(this.ScrapDetail, 3);

                    Excel.Worksheet worksheetScrapDetail = excelScrapDetail.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetScrapDetail.Cells[1, 1] = "Scrap Qty Detail-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelScrapDetail, "Scrap Qty Detail");
                }

                if (this.OnRoadProduction.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...OnRoadProduction List");
                    filename = "Shipping_R40_OnRoadProduction.xltx";
                    Excel.Application excelOnRoadProduction = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comOnRoadProduction = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelOnRoadProduction);
                    comOnRoadProduction.ColumnsAutoFit = true;
                    comOnRoadProduction.WriteTable(this.OnRoadProduction, 3);

                    Excel.Worksheet worksheetOnRoadProduction = excelOnRoadProduction.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetOnRoadProduction.Cells[1, 1] = "On Road Production-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelOnRoadProduction, "On Road Production");
                }

                if (this.Outstanding.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Outstanding");
                    filename = "Shipping_R40_OutStanding.xltx";
                    Excel.Application excelOutstanding = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comOutstanding = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelOutstanding);
                    comOutstanding.ColumnsAutoFit = true;
                    comOutstanding.WriteTable(this.Outstanding, 3);

                    Excel.Worksheet worksheetOutstanding = excelOutstanding.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetOutstanding.Cells[1, 1] = "OutStanding-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelOutstanding, "OutStanding");
                }

                if (this.WarehouseNotClose.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Warehouse Not Close List");
                    filename = "Shipping_R40_WHNotClose.xltx";
                    Excel.Application excelWHNotClose = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comWHNotClose = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelWHNotClose);
                    comWHNotClose.ColumnsAutoFit = true;
                    comWHNotClose.WriteTable(this.WarehouseNotClose, 3);

                    Excel.Worksheet worksheetWHNotClose = excelWHNotClose.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetWHNotClose.Cells[1, 1] = "Warehouse Not Close-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelWHNotClose, "Warehouse Not Close");
                }

                if (this.AlreadySewingOutput.Rows.Count > 0)
                {
                    this.ShowWaitMessage("Starting EXCEL...Already SewingOutput List");
                    filename = "Shipping_R40_AlreadySewingOutput.xltx";
                    Excel.Application excelAlreadySewing = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                    Utility.Report.ExcelCOM comAlreadySewing = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelAlreadySewing);
                    comAlreadySewing.ColumnsAutoFit = true;
                    comAlreadySewing.WriteTable(this.AlreadySewingOutput, 3);

                    Excel.Worksheet worksheetAlreadySewing = excelAlreadySewing.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    worksheetAlreadySewing.Cells[1, 1] = "Already SewingOutput-" + this.contract + "(" + ftys + ")";
                    this.SaveExcelwithName(excelAlreadySewing, "Already SewingOutput");
                }
            }
            else
            {
                filename = "Shipping_R40_Summary(Only Liquidation).xltx";
                Excel.Application excelSummary = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                Utility.Report.ExcelCOM comSummary = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelSummary);
                comSummary.ColumnsAutoFit = true;
                comSummary.WriteTable(this.Summary, 3);

                Excel.Worksheet worksheetSummary = excelSummary.ActiveWorkbook.Worksheets[1];   // 取得工作表
                worksheetSummary.Cells[1, 1] = "Summary-" + this.contract + "(" + ftys + ")";
                this.SaveExcelwithName(excelSummary, "Summary");
            }

            this.HideWaitMessage();
            return true;
        }

        private void SaveExcelwithName(Excel.Application excelapp, string filename)
        {
            string strExcelName = Class.MicrosoftFile.GetName(filename);
            Excel.Workbook workbook = excelapp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelapp.Quit();
            strExcelName.OpenFile();
        }

        private void TxtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"select id,startdate,EndDate from [Production].[dbo].[VNContract]", "20,10,10", this.Text, false, ",", headercaptions: "Contract No, Start Date, End Date");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtContractNo.Text = item.GetSelectedString();
        }

        private void DateGenerate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateGenerate.Value))
            {
                MyUtility.Msg.WarningBox("Generate date cannot be empty");
                this.dateGenerate.Value = DateTime.Now;
                return;
            }

            // Generate日期不可晚於今天
            if (DateTime.Compare((DateTime)this.dateGenerate.Value, DateTime.Now.Date) > 0)
            {
                MyUtility.Msg.WarningBox("Generate date cannot later than Today!");
                this.dateGenerate.Value = DateTime.Now;
            }
        }
    }
}
