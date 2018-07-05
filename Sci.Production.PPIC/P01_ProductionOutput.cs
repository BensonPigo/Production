using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ProductionOutput
    /// </summary>
    public partial class P01_ProductionOutput : Sci.Win.Subs.Base
    {
        private DataRow masterData;
        private string cuttingWorkType;
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings sewingqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings t = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings b = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings i = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings o = new DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings cuttingqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings loadoutput = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();

        /// <summary>
        /// P01_ProductionOutput
        /// </summary>
        /// <param name="masterData">DataRow MasterData</param>
        public P01_ProductionOutput(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.Text = "Production output - " + MyUtility.Convert.GetString(this.masterData["ID"]);
            this.cuttingWorkType = MyUtility.GetValue.Lookup(string.Format("select WorkType from Cutting WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["CuttingSP"])));
            this.tabPage2.Text = "Cutting output";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 撈Summary資料
            string sqlCmd = string.Format(
                @"select (select Max(s.OutputDate)
		from SewingOutput_Detail sd WITH (NOLOCK) 
		inner join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID
		where sd.OrderId = '{0}') as LastSewingDate,
isnull((dbo.getMinCompleteSewQty('{0}',null,null)),0) as SewingQty,
isnull((select SUM(c.Qty)
	   from Orders o WITH (NOLOCK) 
	   inner join CuttingOutput_WIP c WITH (NOLOCK) on o.ID = c.OrderID
	   where {1}),0) as CutQty", MyUtility.Convert.GetString(this.masterData["ID"]),
            string.Format("o.ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ID"])));
            DataTable summaryQty;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out summaryQty);
            this.dateLastSewingOutputDate.Value = MyUtility.Convert.GetDate(summaryQty.Rows[0]["LastSewingDate"]);
            this.numSewingOrderQty.Value = MyUtility.Convert.GetInt(this.masterData["Qty"]);
            this.numOrderQty.Value = MyUtility.Convert.GetInt(this.masterData["Qty"]);
            this.numOrderQty_L.Value = MyUtility.Convert.GetInt(this.masterData["Qty"]);

            this.sewingqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "S", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.t.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "T", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.b.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "B", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.i.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "I", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.o.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "O", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            // 設定Grid1的顯示欄位
            this.gridSewingOutput.IsEditingReadOnly = true;
            this.gridSewingOutput.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSewingOutput)
                 .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                 .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                 .Numeric("SewQty", header: "Sewing Q'ty", width: Widths.AnsiChars(6), settings: this.sewingqty)
                 .Numeric("T", header: "Top", width: Widths.AnsiChars(6), settings: this.t)
                 .Numeric("B", header: "Bottom", width: Widths.AnsiChars(6), settings: this.b)
                 .Numeric("I", header: "Inner", width: Widths.AnsiChars(6), settings: this.i)
                 .Numeric("O", header: "Outer", width: Widths.AnsiChars(6), settings: this.o);

            #region 控制Column是否可被看見
            this.gridSewingOutput.Columns[4].Visible = false;
            this.gridSewingOutput.Columns[5].Visible = false;
            this.gridSewingOutput.Columns[6].Visible = false;
            this.gridSewingOutput.Columns[7].Visible = false;
            if (MyUtility.Convert.GetString(this.masterData["StyleUnit"]) == "SETS")
            {
                sqlCmd = string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.masterData["StyleUKey"]));
                DataTable styleLocation;
                result = DBProxy.Current.Select(null, sqlCmd, out styleLocation);
                if (styleLocation != null)
                {
                    foreach (DataRow dr in styleLocation.Rows)
                    {
                        if (MyUtility.Convert.GetString(dr["Location"]) == "T")
                        {
                            this.gridSewingOutput.Columns[4].Visible = true;
                        }

                        if (MyUtility.Convert.GetString(dr["Location"]) == "B")
                        {
                            this.gridSewingOutput.Columns[5].Visible = true;
                        }

                        if (MyUtility.Convert.GetString(dr["Location"]) == "I")
                        {
                            this.gridSewingOutput.Columns[6].Visible = true;
                        }

                        if (MyUtility.Convert.GetString(dr["Location"]) == "O")
                        {
                            this.gridSewingOutput.Columns[7].Visible = true;
                        }
                    }
                }
            }
            #endregion

            for (int j = 0; j < this.gridSewingOutput.ColumnCount; j++)
            {
                this.gridSewingOutput.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // 當Article變動時，幣一筆不一樣的資料背景顏色要改變
            this.gridSewingOutput.RowsAdded += (s, e) =>
            {
                DataTable dtData = (DataTable)this.listControlBindingSource1.DataSource;
                for (int j = 0; j < e.RowCount; j++)
                {
                    if (!MyUtility.Check.Empty(dtData.Rows[j]["LastArticle"]) && dtData.Rows[j]["LastArticle"].ToString() != dtData.Rows[j]["Article"].ToString())
                    {
                        this.gridSewingOutput.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                    }
                    else
                    {
                        this.gridSewingOutput.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            };

            this.cuttingqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridCutting.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail(this.cuttingWorkType, MyUtility.Convert.GetString(this.masterData["ID"]), "C", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            // 設定Grid2的顯示欄位
            this.gridCutting.IsEditingReadOnly = true;
            this.gridCutting.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridCutting)
                 .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                 .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                 .Numeric("CutQty", header: "Cutting Q'ty", width: Widths.AnsiChars(6), settings: this.cuttingqty);

            for (int j = 0; j < this.gridCutting.ColumnCount; j++)
            {
                this.gridCutting.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.loadoutput.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "S", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            // 設定Grid3的顯示欄位
            this.gridLoading.IsEditingReadOnly = true;
            this.gridLoading.DataSource = this.listControlBindingSource3;
            this.Helper.Controls.Grid.Generator(this.gridLoading)
                  .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                  .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                  .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                  .Numeric("AccuLoad", header: "Loading Output", width: Widths.AnsiChars(6), settings: this.loadoutput);

            // 撈Sewing Data
            sqlCmd = string.Format(
                @"with SewQty
as (
select oq.Article,oq.SizeCode,oq.Qty,sdd.ComboType,isnull(sum(sdd.QAQty),0) as QAQty
from Orders o WITH (NOLOCK) 
inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = o.ID and sdd.Article = oq.Article and sdd.SizeCode = oq.SizeCode
where o.ID = '{0}'
group by oq.Article,oq.SizeCode,oq.Qty,sdd.ComboType
),
minSewQty
as (
select Article,SizeCode,MIN(QAQty) as QAQty
from SewQty
group by Article,SizeCode
),
PivotData
as (
select *
from SewQty
PIVOT (SUM(QAQty)
FOR ComboType IN ([T],[B],[I],[O])) a
)
select p.*,m.QAQty as SewQty,LAG(p.Article,1,null) OVER (Order by oa.Seq,os.Seq) as LastArticle
from PivotData p
left join minSewQty m on m.Article = p.Article and m.SizeCode = p.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.ID = '{1}' and oa.Article = p.Article
left join Order_SizeCode os WITH (NOLOCK) on os.ID = '{1}' and os.SizeCode = p.SizeCode
order by oa.Seq,os.Seq",
                MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Convert.GetString(this.masterData["POID"]));

            DataTable sewingData;
            result = DBProxy.Current.Select(null, sqlCmd, out sewingData);

            sqlCmd = string.Format(
                @"select oq.Article,oq.SizeCode,oq.Qty,sum(c.Qty) as CutQty
from Orders o WITH (NOLOCK) 
inner join Order_Qty oq WITH (NOLOCK) on oq.id = o.ID
left join CuttingOutput_WIP c WITH (NOLOCK) on c.OrderID = o.ID and c.Article = oq.Article and c.Size = oq.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.ID = o.POID and oa.Article = oq.Article
left join Order_SizeCode os WITH (NOLOCK) on os.ID = o.POID and os.SizeCode = oq.SizeCode
where {0}
group by oq.Article,oq.SizeCode,oq.Qty,oa.Seq,os.Seq
order by oa.Seq,os.Seq", string.Format("o.ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ID"])));

            DataTable cuttingData;
            result = DBProxy.Current.Select(null, sqlCmd, out cuttingData);

            // bug fix:0000294: PPIC_P01_ProductionOutput
            this.numSewingQty.Value = MyUtility.Convert.GetInt(sewingData.Compute("sum(SewQty)", string.Empty));  // Sewing Q'ty
            this.numCuttingQty.Value = MyUtility.Convert.GetDecimal(cuttingData.Compute("sum(CutQty)", string.Empty));  // Cutting Q'ty

            sqlCmd = string.Format(
                @"
declare @SP char(20) = '{0}';
--declare @FactoryID char(10) = 'mai';


/*
*	日期條件 
*		1. InLine & OffLine 都在區間內 
*		2. InLine 在區間內 OffLine 在 End 後 
*		3. InLine 在 Start 前 & OffLine 在 End 後
*/
select	masterID = o.POID
		, orderID = o.ID
		, StyleID
		, o.Finished
		, o.FactoryID
		, o.SewLine
		, o.SewInLine
		, o.SewOffLine
into #tsp
from orders o
inner join Factory f on o.FactoryID = f.ID
where	o.Junk != 1
		-- and o.id = @SP 篩選 OrderID
		and o.id = @SP
		--and not ((o.SewOffLine < @StartDate and o.SewInLine < @EndDate) or (o.SewInLine > @StartDate and o.SewInLine > @EndDate))
		and (o.SewInLine is not null or o.SewInLine != '')
		and (o.SewOffLine is not null or o.SewOffLine != '')	
		-- and f.ID = @FactoryID 篩選 FactoryID	
        --and f.ID = @FactoryID
--order by o.POID, o.ID, o.SewLine

/*
*	依照 Poid，取得製作一件衣服所有需要的 【FabricCode & PatternCode】
*/
select	Poid = o.POID
		, pt.UKey
into #TablePatternUkey
from #tsp p
inner join Orders o on p.orderID = o.ID
inner join Pattern pt on	pt.StyleUkey = o.StyleUkey
							and pt.Status = 'Completed' 
							and pt.EDITdATE = (	SELECT MAX(EditDate) 
												from pattern WITH (NOLOCK) 
												where styleukey = o.StyleUkey
														and Status = 'Completed') 

/*														
*	取得所有 PatternPanel
*	※ C# 中 SubStrin (10) 代表第 11 個字元，SQL 中 SubString (11) 代表第 11 個字元
*
*	FabricPanelCode 只需要取 FabricCode 不為空白
*	Distinct 原因是會有多筆 Article Group
*	PatternCode 只要 Annotation = 空值，則 PatterCode = 'ALLPARTS'	
*/
Select	distinct tpu.Poid
		, PatternCode = case
							when pg.Annotation = '' or pg.Annotation is null then 'ALLPARTS'
							else PatternCode.value
						end
		, F_CODE = a.FabricPanelCode
into #TablePatternCode
from #TablePatternUkey tpu
inner join Pattern_GL pg WITH (NOLOCK) on tpu.UKey = pg.PatternUKEY
inner join Pattern_GL_LectraCode a WITH (NOLOCK) on	 a.PatternUkey = tpu.Ukey
														and a.FabricCode != ''
														and pg.SEQ = a.SEQ
outer apply (
	select value = iif(a.SEQ = '0001', SubString(a.PatternCode, 11, Len(a.PatternCode)), a.PatternCode)
) PatternCode


/*
*	取出的訂單，執行以下判斷流程 (#CutComb 儲存 整件衣服 應該裁剪的部位【須展開至 PatterCode】)
*	Step1 找出同 Article、Comb 的 bundel card 有幾個不同的 artwork bundle
*	Step2 組出 SP# 的剪裁 Comb
*		排除
*			1. Order_BOF.Kind = 0 (表Other) 
*			2. Order_BOF.Kind = 3 (表Polyfill)的資料
*			3. 外裁項 (Order_EachCons.CuttingPiece == 0) 【0 代表非外裁項】
*/
select	distinct #tsp.orderID
		, b.Article
		, FabricCombo = tpc.F_CODE
		, artwork = isnull (stuff ((select '+' + subprocessid
									from (
										select distinct bda.subprocessid
										from Bundle_Detail_Art bda
										where bd.BundleNo = bda.Bundleno
									) k
									for xml path('')
									), 1, 1, '')
							, '')
		, PatternCode = tpc.PatternCode
into #tmpin
from #TablePatternCode tpc
left join Bundle b on b.POID = tpc.Poid
left join Bundle_Detail bd on b.id = bd.id
								and tpc.PatternCode = bd.Patterncode
left join #tsp on b.Orderid = #tsp.orderID
left join WorkOrder wo on b.POID = wo.id
							and b.CutRef = wo.CutRef
left join Order_BOF ob on ob.Id = wo.Id 
							and ob.FabricCode = wo.FabricCode
left join Order_EachCons oec on oec.ID = ob.ID 
								and oec.MarkerName = wo.Markername 
								and oec.FabricCombo = wo.FabricCombo 
								and oec.FabricPanelCode = wo.FabricPanelCode 
								and oec.FabricCode = wo.FabricCode

where ob.Kind not in ('0','3')
	    and oec.CuttingPiece = 0
select	#tsp.masterID
		, #tsp.orderID
		, #tsp.StyleID
		, #cur_artwork.FabricCombo
		, #cur_artwork.PatternCode
		, #cur_artwork.Article
		, #cur_artwork.artwork 
		, #tsp.SewLine
		, #tsp.FactoryID
into #cutcomb
from #tsp
inner join  #tmpin #cur_artwork on #tsp.orderID = #cur_artwork.orderID

/*
*	Step3 取出被 Loader 收下的 Bundle Data
*		條件 SubProcessId = 'Loading'
*		排除
*			1. Order_BOF.Kind = 0 (表Other) 
*			2. Order_BOF.Kind = 3 (表Polyfill)的資料
*			3. 外裁項 【Bundle_Detail 不會出現外裁項的資料】
*		BundleGroup 必須是同 Group 組合才能算一件衣服，因為不同 Group 可能會有色差
*/
select	b.orderid
		, bd.SizeCode
		, cdate2 = cDate.value
		, b.Article
		, bd.BundleGroup
		, FabricCombo = wo.FabricPanelCode
		, PatternCode = bd.Patterncode
		, artwork = isnull(ArtWork.value, '')
		, qty = sum(isnull(bd.qty, 0))
into #cur_bdltrack2
from BundleInOut bio
inner join Bundle_Detail bd on bio.BundleNo = bd.BundleNo
inner join Bundle b on b.id = bd.id
inner join WorkOrder wo on b.POID = wo.id
						   and b.CutRef = wo.CutRef
left join Order_BOF ob on ob.Id = wo.Id 
						  and ob.FabricCode = wo.FabricCode
inner join #tsp on b.Orderid = #tsp.orderID
outer apply (
	select value = stuff ((	select '+' + subprocessid
							from (
								select distinct bda.subprocessid
								from Bundle_Detail_Art bda
								where bd.BundleNo = bda.Bundleno
							) k
							for xml path('')
							), 1, 1, '')
) ArtWork
outer apply(
	select value = CONVERT(char(10), bio.InComing, 120)
) cDate
where	bio.SubProcessId = 'loading'
		and ob.Kind not in ('0','3')		
		--and b.OrderID = @SP 篩選 OrderID
		and b.OrderID = @SP
group by b.orderid, bd.SizeCode, cDate.value, b.Article, wo.FabricPanelCode, ArtWork.value, bd.Patterncode, bd.BundleGroup;

/*
*	根據每一個 OrderID 取得 SewInDate  日期展開至 EndDate
*	這邊會影響到最後計算成衣件數
*/
create table #CBDate (
	OrderID varchar(13)
	, SewDate date
);

Declare rs Cursor For Select orderID, SewInLine from #tsp
Declare @OrderID varchar(20);
Declare @StartInLine date;
Declare @SewInLine date;
Declare @count int = 0;

/*
*	指向 #tsp 第一筆資料
*	@@FETCH_STATUS = 0，代表資料指向成功
*/
Open rs
Fetch Next From rs Into @OrderID, @SewInLine
While @@FETCH_STATUS = 0
Begin	
	select top 1 @StartInLine = iif(cdate2 is null or cdate2 = '', @SewInLine, cdate2)
	from #cur_bdltrack2 
	where #cur_bdltrack2.Orderid = @OrderID
	order by cdate2
	
	;with qa as (
		select	OrderID = @OrderID
				, qdate = @StartInLine

		union all
		select	OrderID = @OrderID
				, DATEADD(day, 1, qdate)
		from qa 
		where	qdate < GETDATE()
	) 
	insert into #CBDate
	select OrderID, qdate
	from qa
	Option (maxrecursion 365);

/*
*	Fetch 指向 #tsp 下一筆資料
*/
	Fetch Next From rs Into @OrderID, @SewInLine
End;
Close rs;
Deallocate rs; 

/*	
*	Step4 已收的bundle資料中找出各 
*			article 
*			size
*			部位
*			artwork
*			加總數量
*	Step5 計算 sp# 上線日至下線日的產出
*	Step6 依條件日期區間，至 sewing 取得 stdqty 加總以及抓取備妥的成衣件數
*/
select	NewCutComb.FactoryID
		, NewCutComb.orderid
		, NewCutComb.StyleID
		, NewCutComb.cdate2
		, NewCutComb.SewLine
		, NewCutComb.Article
		, NewCutComb.SizeCode
		, NewCutComb.BundleGroup
		, NewCutComb.FabricCombo
		, NewCutComb.PatternCode
		, NewCutComb.artwork
		, qty = isnull(#cur_bdltrack2.qty, 0)
		, AccuLoadingQty = sum(isnull(#cur_bdltrack2.qty, 0)) over (partition by NewCutComb.FactoryID, NewCutComb.orderid,  NewCutComb.Article, NewCutComb.SizeCode, NewCutComb.BundleGroup, NewCutComb.FabricCombo, NewCutComb.PatternCode, NewCutComb.artwork
																    order by NewCutComb.cdate2)
into #Min_cut
from (
/*
*	組出每一天，同 OrderID, Artwork, Article, SizeCode 需要的 FabricCombe & PatternCode
*	這邊會在 CutComb 成衣組合中，加入需計算的日期	
*/
	select	distinct #cutcomb.*
			, cdate2 = #CBDate.SewDate
			, SizeCode = SizeCode.value
			, BundleGroup = BundleGroup.value
	from #cutcomb
	outer apply (
		select distinct value = SizeCode
		from #cur_bdltrack2 
		where	#cutcomb.orderID = #cur_bdltrack2.Orderid
	) SizeCode
	outer apply (
		select distinct value = BundleGroup
		from #cur_bdltrack2 
		where	#cutcomb.orderID = #cur_bdltrack2.Orderid
	) BundleGroup
	inner join #CBDate on #cutcomb.orderID = #CBDate.OrderID
) NewCutComb 
left join #cur_bdltrack2 on	NewCutComb.artwork = #cur_bdltrack2.artwork 
							and NewCutComb.FabricCombo = #cur_bdltrack2.FabricCombo
							and NewCutComb.PatternCode = #cur_bdltrack2.PatternCode
							and NewCutComb.orderID = #cur_bdltrack2.orderid
							and NewCutComb.Article = #cur_bdltrack2.Article 
							and NewCutComb.SizeCode = #cur_bdltrack2.SizeCode
                            and NewCutComb.Cdate2 = #cur_bdltrack2.Cdate2	
							and NewCutComb.BundleGroup = #cur_bdltrack2.BundleGroup;						
--order by cdate2, orderid, Article, SizeCode, FabricCombo, PatternCode, artwork

/*
*	準備好要印的資料
*/
select	FactoryID
		, SP
		, StyleID
		, SewingDate = DateAdd(day, 1 ,SewingDate)
		, Line
		, AccuStd
		, Article, SizeCode
		, AccuLoad = AccuLoading
into #print
from (
	select	FactoryID
			, SP = orderID
			, StyleID
			, SewingDate = cdate2
			, Line = SewLine
			, AccuStd = isnull(std.value, 0)
			, Article, SizeCode
			, AccuLoading = sum (isnull(MinLoadingQty, 0))
	from (
/*
*		依照【日期, SP#, Article, Size, Comb, Artwork】取最小數量 (因為每個部位都要有，才能成為一件衣服)
*		判斷 BundleGroup
*			第一次群組判斷最小數量需要加上 BundleGroup
*			第二次群組加總所有成衣數量		
*/
		select FactoryID
				, orderID
				, StyleID
				, cdate2
				, SewLine
				, Article, SizeCode
				, MinLoadingQty = sum (MinLoadingQty)
		from (
			select	FactoryID
					, orderID
					, StyleID
					, cdate2
					, SewLine
					, Article, SizeCode
					, MinLoadingQty = min(AccuLoadingQty)
			from #Min_cut 
			group by FactoryID, orderID	,StyleID, cdate2, SewLine, Article, SizeCode, BundleGroup
		)w
		group by FactoryID, orderID	,StyleID, cdate2, SewLine, Article, SizeCode
	)x 
	outer apply(
		select	value = isnull(( select sum(s.StdQ)
									from dbo.getDailystdq(x.orderid) s)
								, 0)
	) std
	group by FactoryID, orderID, StyleID, cdate2, SewLine, std.value
					, Article, SizeCode
) a

	select	#print.* 
			, BCS = iif(BCS.value >= 100, 100, BCS.value)
	into #la
	from #print
	outer apply (
		select value = ROUND(AccuLoad / iif(AccuStd = 0, 1, AccuStd) * 100, 2)
	) BCS
	where SewingDate = format(GETDATE(),'yyyy-MM-dd')
	order by FactoryID, SP, SewingDate, Line


;with SewQty
as (
select oq.Article,oq.SizeCode,oq.Qty,sdd.ComboType,isnull(sum(sdd.QAQty),0) as QAQty
from Orders o WITH (NOLOCK) 
inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = o.ID and sdd.Article = oq.Article and sdd.SizeCode = oq.SizeCode
where o.ID = @SP
group by oq.Article,oq.SizeCode,oq.Qty,sdd.ComboType
),
minSewQty
as (
select Article,SizeCode,MIN(QAQty) as QAQty
from SewQty
group by Article,SizeCode
)
select p.*
	,AccuLoad = isnull(l.AccuLoad,0)
from SewQty p
left join minSewQty m on m.Article = p.Article and m.SizeCode = p.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.ID = @SP and oa.Article = p.Article
left join Order_SizeCode os WITH (NOLOCK) on os.ID = @SP and os.SizeCode = p.SizeCode
left join #la l on l.SizeCode = p.SizeCode and l.Article = p.Article
order by oa.Seq,os.Seq


--drop table #tsp
--drop table #cutcomb
--drop table #cur_bdltrack2
--drop table #Min_cut
--drop table #TablePatternUkey;
--drop table #TablePatternCode
--drop table #CBDate,#la


drop table #tsp
drop table #cutcomb
drop table #cur_bdltrack2
drop table #Min_cut
drop table #print
drop table #TablePatternUkey;
drop table #TablePatternCode
drop table #CBDate
,#la

", MyUtility.Convert.GetString(this.masterData["ID"]));

            DataTable loadingoutput;
            result = DBProxy.Current.Select(null, sqlCmd, out loadingoutput);
            this.numLoadingQty.Value = MyUtility.Convert.GetInt(loadingoutput.Compute("sum(AccuLoad)", string.Empty));  // Sewing Q'ty

            this.listControlBindingSource1.DataSource = sewingData;
            this.listControlBindingSource2.DataSource = cuttingData;
            this.listControlBindingSource3.DataSource = loadingoutput;
        }

        // Sewing Q'ty
        private void NumSewingQty_DoubleClick(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "A", string.Empty, string.Empty);
            callNextForm.ShowDialog(this);
        }

        private void NumLoadingQty_DoubleClick(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "A", string.Empty, string.Empty);
            callNextForm.ShowDialog(this);
        }
    }
}
