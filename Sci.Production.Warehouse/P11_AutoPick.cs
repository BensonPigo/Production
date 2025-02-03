using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class.Commons;
using Sci.Production.PPIC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P11_AutoPick : Win.Subs.Base
    {
        private StringBuilder sbSizecode;
        private string poid;
        private string issueid;
        private string orderid;
        private DataTable BOA_PO_Size;
        private DataTable dtIssueBreakDown;
        private bool combo;
        private string strLabel;
        private string strPacking;
        private DataRow drMnOrder;

        /// <inheritdoc/>
        public DataTable BOA_PO { get; set; }

        /// <inheritdoc/>
        public Dictionary<DataRow, DataTable> DictionaryDatas { get; set; }

        /// <inheritdoc/>
        public P11_AutoPick(string issueid, string poid, string orderid, DataTable dtIssueBreakDown, StringBuilder sbSizecode, bool combo)
        {
            this.InitializeComponent();
            this.poid = poid;
            this.issueid = issueid;
            this.orderid = orderid;
            this.dtIssueBreakDown = dtIssueBreakDown;
            this.sbSizecode = sbSizecode;
            this.combo = combo;
            this.Text += string.Format(" ({0})", this.poid);

            this.strLabel = MyUtility.GetValue.Lookup($"select Label from Orders with(nolock) where id='{this.orderid}'");
            this.strPacking = MyUtility.GetValue.Lookup($"select Packing from Orders with(nolock) where id='{this.orderid}'");
            MyUtility.Check.Seek($@"select SMnorderApv,MnorderApv,* from Orders with(nolock) where id='{this.orderid}'", out this.drMnOrder);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region 按鈕變色
            this.btnHangtag.ForeColor = !MyUtility.Check.Empty(this.strLabel) ? Color.Blue : Color.Black;
            this.btnPackingMethod.ForeColor = !MyUtility.Check.Empty(this.strPacking) ? Color.Blue : Color.Black;
            if (this.drMnOrder != null)
            {
                this.btnMNnotice.ForeColor = (!MyUtility.Check.Empty(this.drMnOrder["MnorderApv"]) || !MyUtility.Check.Empty(this.drMnOrder["SMnorderApv"])) ? Color.Blue : Color.Black;
            }
            #endregion

            if (this.dtIssueBreakDown == null)
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            decimal sum = 0;
            foreach (DataRow dr in this.dtIssueBreakDown.Rows)
            {
                foreach (DataColumn dc in this.dtIssueBreakDown.Columns)
                {
                    if (ReferenceEquals(sum.GetType(), dr[dc].GetType()))
                    {
                        sum += (decimal)dr[dc];
                    }
                }
            }

            if (sum == 0)
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            DataSet dataSet = new DataSet();

            DataTable[] result = null;

            this.BOA_PO = null;
            this.BOA_PO_Size = null;

            string sqlcmd = string.Format(
                @"
;WITH UNPIVOT_1 AS (
    SELECT * 
    FROM #tmp
    UNPIVOT
    (
        QTY FOR SIZECODE IN ({0})
    ) AS PVT
) 
select *
into #tmp2
from UNPIVOT_1 ;

alter table #tmp2 alter column OrderID varchar(13)
alter table #tmp2 alter column SizeCode varchar(8)
alter table #tmp2 alter column Article varchar(8)

delete from #tmp2 where qty = 0;

declare @count as int;
create Table #Tmp_Order_Qty
(  
    ID Varchar(13)
    , FactoryID Varchar(8)
    , CustCDID Varchar(16)
    , ZipperInsert Varchar(5)
    , CustPONo VarChar(30)
    , BuyMonth VarChar(16)
    , CountryID VarChar(2)
    , StyleID Varchar(15)
	, Article VarChar(8)
    , SizeSeq VarChar(2)
    , SizeCode VarChar(8)
    , Qty Numeric(6,0)
    , OriQty Numeric(6,0)
);

Insert Into #Tmp_Order_Qty
Select  Orders.ID
        , Orders.FactoryID
        , Orders.CustCDID
        , CustCD.ZipperInsert
        , Orders.CustPONo
        , Orders.BuyMonth
        , Factory.CountryID
        , Orders.StyleID
	    , Order_Article.Article
        , Order_SizeCode.Seq
        , Order_SizeCode.SizeCode
		, IsNull(#tmp2.Qty, 0) Qty
		, oq.OriQty
From dbo.Orders with(nolock)
Left Join dbo.Order_SizeCode with(nolock) On Order_SizeCode.ID = Orders.POID
Left Join dbo.Order_Article with(nolock) On Order_Article.ID = Orders.ID
Left Join #tmp2 On #tmp2.OrderID = Orders.ID
			       And #tmp2.SizeCode = Order_SizeCode.SizeCode
			       And #tmp2.Article = Order_Article.Article
left join Order_Qty oq with(nolock) on oq.id = Orders.ID and oq.Article = Order_Article.Article and oq.SizeCode = Order_SizeCode.SizeCode
Left Join dbo.CustCD with(nolock) On CustCD.BrandID = Orders.BrandID
			            And CustCD.ID = Orders.CustCDID
Left Join dbo.Factory with(nolock) On Factory.ID = Orders.FactoryID
Where   Orders.POID = '{3}'
		And Orders.Junk = 0

select  @count = count(1) 
from #Tmp_Order_Qty;
	
if @count = 0
    begin
	Insert Into #Tmp_Order_Qty
	Select  Orders.ID
            , Orders.FactoryID
            , Orders.CustCDID
            , CustCD.ZipperInsert
			, Orders.CustPONo
            , Orders.BuyMonth
            , Factory.CountryID
            , Orders.StyleID
			, Order_Article.Article
            , Order_SizeCode.Seq
            , Order_SizeCode.SizeCode
		    , IsNull(Order_Qty.Qty, 0) Qty
			, Order_Qty.OriQty
	From dbo.Orders with(nolock)
	Left Join dbo.Order_SizeCode with(nolock) On Order_SizeCode.ID = Orders.POID
	Left Join dbo.Order_Article with(nolock) On Order_Article.ID = Orders.ID
	Left Join dbo.Order_Qty with(nolock) On Order_Qty.ID = Orders.ID
		                       And Order_Qty.SizeCode = Order_SizeCode.SizeCode
			                   And Order_Qty.Article = Order_Article.Article
	Left Join dbo.CustCD with(nolock) On CustCD.BrandID = Orders.BrandID
			                And CustCD.ID = Orders.CustCDID
	Left Join dbo.Factory with(nolock) On Factory.ID = Orders.FactoryID
	Where   Orders.POID = '{3}'
	        And Orders.Junk = 0
		    AND Order_Qty.ID = '{2}'
		 
	select @count = count(1) 
    from #Tmp_Order_Qty;
		
    if @count = 0
		begin
		Insert Into #Tmp_Order_Qty
		Select  Orders.ID
                , Orders.FactoryID
                , Orders.CustCDID
                , CustCD.ZipperInsert
				, Orders.CustPONo
                , Orders.BuyMonth
                , Factory.CountryID
                , Orders.StyleID
				, Order_Article.Article
                , Order_SizeCode.Seq
                , Order_SizeCode.SizeCode
				, IsNull(Order_Qty.Qty, 0) Qty
			    , Order_Qty.OriQty
		From dbo.Orders with(nolock) 
		Left Join dbo.Order_SizeCode with(nolock) On Order_SizeCode.ID = Orders.POID
		Left Join dbo.Order_Article with(nolock) On Order_Article.ID = Orders.ID
		Left Join dbo.Order_Qty with(nolock) On Order_Qty.ID = Orders.ID
				                   And Order_Qty.SizeCode = Order_SizeCode.SizeCode
				                   And Order_Qty.Article = Order_Article.Article
		Left Join dbo.CustCD with(nolock) On CustCD.BrandID = Orders.BrandID
				                And CustCD.ID = Orders.CustCDID
		Left Join dbo.Factory with(nolock) On Factory.ID = Orders.FactoryID
		Where   Orders.POID = '{3}'
			    And Orders.Junk = 0			 
	end
end

Create Table #Tmp_BoaExpend
(   ExpendUkey BigInt Identity(1,1) Not Null, 
    ID Varchar(13),
    Order_BOAUkey BigInt
	, RefNo VarChar(36)
    , SCIRefNo VarChar(30)
    , Article VarChar(8)
    , ColorID VarChar(6)
    , SuppColor NVarChar(Max)
	, SizeCode VarChar(8)
    , SizeSpec VarChar(15)
    , SizeUnit VarChar(8)
    , Remark NVarChar(Max)
	, OrderQty Numeric(6,0)
    --, Price Numeric(12,4)--pms does not use this column
    , UsageQty Numeric(11,2)
    , UsageUnit VarChar(8)
    , SysUsageQty  Numeric(11,2)
	, BomZipperInsert VarChar(5)
    , BomCustPONo VarChar(30)
    , Keyword VarChar(Max)
    , Keyword_Original VarChar(Max)
    , Keyword_xml VarChar(Max), OrderList nvarchar(max)
    , ColorDesc nvarchar(150), Special nvarchar(max)
	, BomTypeColorID varchar(50)
    , BomTypeSize varchar(50)
    , BomTypeSizeUnit varchar(50)
    , BomTypeZipperInsert varchar(50)
    , BomTypeArticle varchar(50)
    , BomTypeCOO varchar(50)
	, BomTypeGender varchar(50)
    , BomTypeCustomerSize varchar(50)
    , BomTypeDecLabelSize varchar(50)
    , BomTypeBrandFactoryCode varchar(50)
    , BomTypeStyle varchar(50)
	, BomTypeStyleLocation varchar(50)
    , BomTypeSeason varchar(50)
    , BomTypeCareCode varchar(50)
    , BomTypeCustomerPO varchar(50)
    , BomTypeBuyMonth varchar(50)
    , BomTypeBuyerDlvMonth varchar(50)
	, Primary Key (ExpendUkey)
	, Index Idx_ID NonClustered (ID, Order_BOAUkey, ColorID) -- table index
);

--依日期分新舊使用BoaExpend還有procedure usp_BoaByIssueBreakDown 有用到
if  (select CFMDate from orders with(nolock) where id = '{3}') < '2022/08/01'
begin
    Exec dbo.BoaExpend '{3}', {4}, {5}, '{6}',0,1;
end
else
BEGIN   
    Exec dbo.BoaExpend_New '{3}', {4}, {5}, '{6}',0,1;
END

--BoAExpend SizeSpec 與 Po_Supp_Detail SizeSpec 意義不同，因此比對時 Po_Supp_Detail 也需要展開
select	distinct psd.id as [poid]
        , psd.seq1
        , psd.seq2
        , psd.Refno
        , psd.SCIRefno
        , dbo.getMtlDesc(psd.id, psd.seq1, psd.seq2,2,0) [description]
        , psd.Spec
        , psd.Special
        , psd.Remark
        , IIF ( psd.UsedQty = 0.0000, 0, psd.UsedQty ) as UsedQty  
        , NetQty = Round(dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.NetQty), 2)
        , psd.StockUnit
        , f.BomTypeCalculate
        , ColorMultipleID = isnull(dbo.GetColorMultipleID(psd.BrandId, isnull(bomType.Color, '')), '')
        , f.MTLTYPEID
        , CompareBoAExpandSeq1 = case 
                                    when psd.Seq1 like '7%' then psd.OutputSeq1
                                    else psd.Seq1
                                 end
		, [Garment Size]=dbo.GetGarmentSizeByOrderIDSeq(psd.id ,psd.SEQ1 ,psd.SEQ2)
        ,bomType.*
into #tmpPO_supp_detail
from dbo.PO_Supp_Detail as psd WITH (NOLOCK) 
inner join dbo.Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
inner join dbo.MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID
inner join orders o With(NoLock) on psd.id = o.id
OUTER APPLY(SELECT * FROM dbo.GetPo3Spec_Null(psd.id,psd.seq1,psd.seq2))bomType
where   psd.id = '{3}' 
        and psd.FabricType = 'A' 
        and m.IssueType = '{7}'
        and psd.junk != 1

--計算 FtyInventory 庫存量
--因為 #tmpPo_Supp_Detail 有用 Order_BoA 展開，把多餘的先做排除，再尋找 【庫存量】
--【需求量】計算放 SQL 外，在記錄 output 時順便計算
----計算的原則是【每一個 SizeCode 先四捨五入後再加總】
--※顯示時，SizeSpec = Po_Supp_Detail.SizeSpec
--※計算時，SizeSpec = #TmpPo_Supp_Detail.SizeSpec
select  x.*
        , [balanceqty] = isnull(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty, 0)
        , ftyinventoryukey = Fty.Ukey 
        , Fty.StockType
        , Fty.Roll
        , Fty.Dyelot
from (
    select  distinct 0 as [Selected]
            , '' as id
            , b.Refno
            , b.poid
            , b.seq1
            , b.seq2
            , b.[description]
            , ColorID = b.Color
            , SizeSpec = size
            , b.SCIRefno
            , b.Spec
            , b.Special
            , b.Remark
            , b.UsedQty
            , b.NetQty
            , OrderListShow.OrderList
            , b.StockUnit
            , Qty = 0.00
            , concat (Ltrim (Rtrim (b.seq1)), ' ', b.seq2) as seq
            , b.ColorMultipleID
            , b.MTLTYPEID
            , checkOrderListEmpty = checkOrderListEmpty.value
            , Orderlist_chk = psdo.Orderid
			, b.[Garment Size]
            , b.SizeUnit
    from #tmpPO_supp_detail b
    outer apply (
        select value = case count (1)
                            when 0 then 'Y'
                            else 'N'
                       end
        from dbo.po_supp_detail_orderlist psdo with (NOLOCK)
        where psdo.id = b.poid
	          and psdo.seq1 = b.seq1
	          and psdo.seq2 = b.seq2
    ) checkOrderListEmpty
    left join dbo.po_supp_detail_orderlist psdo with (NOLOCK) on psdo.ID = b.poid 
                                                                 and psdo.seq1 = b.seq1
                                                                 and psdo.seq2 = b.seq2
                                                                 and psdo.orderid = '{2}'    
    left join PO_Supp_Detail_OrderList_Show OrderListShow with (NOLOCK) On OrderListShow.ID = b.poid
                                                            and OrderListShow.Seq1 = b.Seq1
                                                            and OrderListShow.Seq2 = b.Seq2
) x
left join dbo.FtyInventory Fty with(NoLock) on Fty.poid = x.poid
                                                and Fty.seq1 = x.seq1 
											    and Fty.seq2 = x.seq2
											    and Fty.StockType = 'B' 
											    and Fty.Roll = ''
order by x.poid, x.seq1, x.seq2, x.scirefno, x.ColorID, x.SizeSpec, x.Special;

--因為 #tmpPo_Supp_Detail 有用 Order_BoA 展開
--計算數量時，必須根據 Poid, Seq, SizeCode 群組
SELECT b.POID, b.SEQ1, b.SEQ2, boa.SizeCode
    , qty = Round(SUM(boa.UsageQty * dbo.GetUnitRate(boa.UsageUnit, b.StockUnit)) , 2)
INTO #tmpLast
FROM #tmpPO_supp_detail b
OUTER APPLY (
    SELECT t.SizeCode, t.UsageQty, t.UsageUnit
    FROM #Tmp_BoaExpend t
    INNER JOIN Order_BOA ob with (NOLOCK) ON t.Order_BOAUkey = ob.Ukey
    WHERE b.SCIRefno = t.SciRefno 
    AND b.POID = t.ID 
    AND b.CompareBoAExpandSeq1 = ob.Seq1
    AND dbo.ConditionIncludeNull(b.Article         , t.BomTypeArticle         ) = 1
    AND dbo.ConditionIncludeNull(b.BrandFactoryCode, t.BomTypeBrandFactoryCode) = 1
    AND dbo.ConditionIncludeNull(b.CareCode        , t.BomTypeCareCode        ) = 1
    AND dbo.ConditionIncludeNull(b.Color           , t.BomTypeColorID         ) = 1
    AND dbo.ConditionIncludeNull(b.COO             , t.BomTypeCOO             ) = 1
    AND dbo.ConditionIncludeNull(b.CustomerPO      , t.BomTypeCustomerPO      ) = 1
    AND dbo.ConditionIncludeNull(b.CustomerSize    , t.BomTypeCustomerSize    ) = 1
    AND dbo.ConditionIncludeNull(b.DecLabelSize    , t.BomTypeDecLabelSize    ) = 1
    AND dbo.ConditionIncludeNull(b.Gender          , t.BomTypeGender          ) = 1
    AND dbo.ConditionIncludeNull(b.Season          , t.BomTypeSeason          ) = 1
    AND dbo.ConditionIncludeNull(b.Size            , t.BomTypeSize            ) = 1
    AND dbo.ConditionIncludeNull(b.Style           , t.BomTypeStyle           ) = 1
    AND dbo.ConditionIncludeNull(b.StyleLocation   , t.BomTypeStyleLocation   ) = 1
    AND dbo.ConditionIncludeNull(b.ZipperInsert    , t.BomTypeZipperInsert    ) = 1
    AND dbo.ConditionIncludeNull(b.BuyMonth        , t.BomTypeBuyMonth        ) = 1
    AND dbo.ConditionIncludeNull(b.BuyerDlvMonth   , t.BomTypeBuyerDlvMonth   ) = 1
) boa
GROUP BY b.POID, b.SEQ1, b.SEQ2, boa.SizeCode

SELECT
    z.*
    , Autopickqty = isnull (t.qty, 0)
    , qty = ISNULL(t.qty, 0)
    , Diffqty = 0.0--之後用庫存總數去分配時再計算
FROM(
    SELECT x.*, os.SizeCode, os.Seq
    FROM(SELECT DISTINCT POID, SEQ1, SEQ2 FROM #tmpPO_supp_detail)x
    CROSS JOIN Order_SizeCode os WITH(NOLOCK)--要去展開所有 SizeCode, 提供手動填寫需求
    WHERE os.Id = '{3}'
)z
LEFT JOIN #tmpLast t ON t.SizeCode = z.SizeCode
                    AND t.POID = z.POID
                    AND t.SEQ1 = z.SEQ1
                    AND t.SEQ2 = z.SEQ2
order by z.SEQ1,z.SEQ2,z.Seq
", this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1),
                this.issueid,
                this.orderid,
                this.poid,
                0,
                1,
                Env.User.UserID,
                "Sewing",
                Env.User.Keyword); // .Replace("[", "[_")

            // 呼叫procedure，取得BOA展開結果
            try
            {
                DBProxy.Current.DefaultTimeout = 3600;
                string sizecodes = this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1).Replace("[", string.Empty).Replace("]", string.Empty);
                var rESULT = MyUtility.Tool.ProcessWithDatatable(this.dtIssueBreakDown, "OrderID,Article," + sizecodes, sqlcmd, out result, "#tmp");
                if (!rESULT)
                {
                    this.ShowErr(rESULT);
                    return;
                }

                if (result.Length > 0)
                {
                    dataSet.Tables.AddRange(result);
                    this.BOA_PO = result[2];
                    this.BOA_PO.DefaultView.Sort = "poid,seq1,seq2";
                    this.BOA_PO_Size = result[3];
                    this.BOA_PO.ColumnsStringAdd("Output");
                    this.BOA_PO.ColumnsStringAdd("OutputAutoPick");
                    this.BOA_PO.ColumnsStringAdd("AutoPickqty");

                    DataRelation relation = new DataRelation(
                        "rel1",
                        new DataColumn[] { this.BOA_PO.Columns["Poid"], this.BOA_PO.Columns["seq1"], this.BOA_PO.Columns["seq2"] },
                        new DataColumn[] { this.BOA_PO_Size.Columns["Poid"], this.BOA_PO_Size.Columns["seq1"], this.BOA_PO_Size.Columns["seq2"] });
                    dataSet.Relations.Add(relation);

                    foreach (DataRow dr in this.BOA_PO.Rows)
                    {
                        dr["ColorID"] = dr["ColorMultipleID"];
                        DataTable tmp = new DataTable();
                        tmp.ColumnsStringAdd("Poid");
                        tmp.ColumnsStringAdd("seq1");
                        tmp.ColumnsStringAdd("seq2");
                        tmp.ColumnsStringAdd("seq");
                        tmp.ColumnsStringAdd("sizecode");
                        tmp.ColumnsStringAdd("Garment Size");
                        tmp.ColumnsDecimalAdd("Autopickqty");
                        tmp.ColumnsDecimalAdd("qty");
                        tmp.ColumnsDecimalAdd("Diffqty");
                        decimal balqty = MyUtility.Convert.GetDecimal(dr["balanceqty"]); // 庫存總數
                        var drs = dr.GetChildRows(relation);
                        if (drs.Count() > 0)
                        {
                            // Qty 在這邊計算原因：細項中每一個 SizeCode 的數量都有做四捨五入
                            // 若在組 SQL 時就先將數量做加總，四捨五入後的結果會有差異
                            #region 計算每一個項的 Output & Qty
                            string output = string.Empty;
                            string output_ori = string.Empty;
                            decimal totalQty = drs.AsEnumerable().Sum(s => MyUtility.Convert.GetDecimal(s["qty"]));
                            decimal totalQty_ori = totalQty;
                            totalQty = totalQty > balqty ? balqty : totalQty;
                            decimal totalQty2 = totalQty;
                            foreach (DataRow dr2 in drs)
                            {
                                if (MyUtility.Convert.GetDecimal(dr2["qty"]) > 0)
                                {
                                    output_ori += dr2["sizecode"].ToString() + "*" + Convert.ToDecimal(dr2["qty"]).ToString("0.00") + ", ";
                                }

                                if (totalQty < MyUtility.Convert.GetDecimal(dr2["qty"]))
                                {
                                    dr2["qty"] = totalQty;
                                }

                                if (MyUtility.Convert.GetDecimal(dr2["qty"]) > 0)
                                {
                                    output += dr2["sizecode"].ToString() + "*" + Convert.ToDecimal(dr2["qty"]).ToString("0.00") + ", ";
                                }

                                dr2["diffqty"] = MyUtility.Convert.GetDecimal(dr2["AutoPickqty"]) - MyUtility.Convert.GetDecimal(dr2["qty"]);
                                tmp.ImportRow(dr2);
                                totalQty -= MyUtility.Convert.GetDecimal(dr2["qty"]);
                            }

                            dr["Output"] = output;
                            dr["OutputAutoPick"] = output_ori;
                            dr["Qty"] = Math.Round(totalQty2, 2);
                            dr["AutoPickqty"] = Math.Round(totalQty_ori, 2);
                            #endregion
                        }

                        /*
                         * 1. 若數量有大於零才勾選
                         * 2. MTLTYPEID不為THREAD,CARTON才勾選
                         * 3. 表頭 orderid 需存在 orderlist(po_supp_detail_orderlist)中才勾選
                         *    若 orderlist 為空，表示全部都要帶出
                         */
                        if (Convert.ToDouble(dr["qty"]) != 0
                            && Convert.ToDouble(dr["balanceqty"]) > 0
                            && dr["MTLTYPEID"].ToString().Equals("THREAD") == false
                            && dr["MTLTYPEID"].ToString().Equals("CARTON") == false
                            && (dr["Orderlist_chk"] != DBNull.Value
                                || dr["checkOrderListEmpty"].EqualString("Y")))
                        {
                            dr["Selected"] = 1;
                        }
                        else
                        {
                            // 沒有default 打勾的話output要清掉因為在BOA裡面 所以會被帶出來可是實際上我不需要這些物料 所以不用計算
                            dr["Output"] = string.Empty;
                            dr["Qty"] = 0;

                            // 連同detail資料也顯示0
                            if (tmp.Rows.Count > 0 && Convert.ToDouble(dr["qty"]) > 0)
                            {
                                tmp.AsEnumerable().ToList().ForEach(row => row["qty"] = 0);
                            }
                        }

                        if (tmp.Rows.Count > 0)
                        {
                            this.DictionaryDatas.Add(dr, tmp);
                        }
                        else
                        {
                            this.DictionaryDatas.Add(dr, new DataTable());
                        }
                    }

                    this.BOA_PO.Columns.Remove("ColorMultipleID");
                    var tmp2 = this.DictionaryDatas.Count;
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.GetBaseException().ToString(), "Error");
            }
            finally
            {
                dataSet.Dispose();
            }

            DBProxy.Current.DefaultTimeout = 300;
            this.listControlBindingSource1.DataSource = this.BOA_PO;
            this.gridAutoPick.DataSource = this.listControlBindingSource1;

            this.gridAutoPick.AutoResizeColumns();

            #region --Pick Qty 開窗--
            DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            ns.CellMouseDoubleClick += (s, e) =>
            {
                this.BOA_PO.AcceptChanges(); // 先做初始設定，再透過Detail來控制UNDO OR SAVE
                var dr = this.gridAutoPick.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                var frm = new P11_AutoPick_Detail(this.combo, this.poid, this.orderid, this.BOA_PO, e.RowIndex, this);
                this.DictionaryDatasAcceptChanges();
                frm.ShowDialog(this);
            };
            #endregion
            DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                var dr = this.gridAutoPick.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null || Convert.ToDecimal(e.FormattedValue) == Convert.ToDecimal(dr["qty"]))
                {
                    return;
                }

                dr["qty"] = e.FormattedValue;
                if (Convert.ToDecimal(dr["qty"]) > 0 && Convert.ToDecimal(dr["Balanceqty"]) >= Convert.ToDecimal(dr["qty"]))
                {
                    dr["Selected"] = 1;
                }
                else
                {
                    dr["Selected"] = 0;
                }
            };

            #region --設定Grid1的顯示欄位--

            this.gridAutoPick.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridAutoPick.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAutoPick)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(23), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("colorid", header: "Color ID", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("sizespec", header: "SizeSpec", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Garment Size", header: "Garment\r\nSize", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
                .Numeric("qty", header: "Pick Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, settings: ns2)
                .Text("Output", header: "Pick Output", width: Widths.AnsiChars(10), settings: ns)
                .Text("Balanceqty", header: "Bulk Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("AutoPickqty", header: "AutoPick \r\n Calculation \r\n Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                .Text("OutputAutoPick", header: "AutoPick \r\n Calculation \r\n Output", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("suppcolor", header: "Supp Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("sizecode", header: "SizeCode", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("sizeunit", header: "Size Unit", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("usageqty", header: "Usage Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("usageunit", header: "Usage Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;
            this.gridAutoPick.Columns["qty"].Frozen = true;  // Qty
            this.gridAutoPick.Columns["Output"].DefaultCellStyle.BackColor = Color.Pink;   // Qty
            #endregion

        }

        /// <inheritdoc/>
        public void Sum_subDetail(DataRow target, DataTable source)
        {
            DataTable tmpDt = source;
            DataRow dr = target;
            if (tmpDt != null)
            {
                var output = string.Empty;
                decimal sumQTY = 0;
                foreach (DataRow dr2 in tmpDt.ToList())
                {
                    if (Convert.ToDecimal(dr2["qty"]) != 0)
                    {
                        output += dr2["sizecode"].ToString() + "*" + Convert.ToDecimal(dr2["qty"]).ToString("0.00") + ", ";
                        sumQTY += Convert.ToDecimal(dr2["qty"]);
                    }
                }

                dr["Output"] = output;
                dr["qty"] = Math.Round(sumQTY, 2);
            }

            if (Convert.ToDecimal(dr["qty"]) > 0)
            {
                dr["Selected"] = 1;
            }
            else
            {
                dr["Selected"] = 0;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPick_Click(object sender, EventArgs e)
        {
            if (this.BOA_PO == null || this.gridAutoPick.DataSource == null)
            {
                return;
            }

            this.gridAutoPick.ValidateControl();
            DataRow[] dr2 = this.BOA_PO.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select rows first!", "Warnning");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        /// <inheritdoc/>
        public DataTable GetAutoDetailDataTable(int rowIndex)
        {
            DataTable tmpDt = this.DictionaryDatas[this.gridAutoPick.GetDataRow(rowIndex)];
            return tmpDt;
        }

        /// <inheritdoc/>
        public DataRow GetAutoDetailDataRow(int rowIndex)
        {
            DataRow tmpDt = this.gridAutoPick.GetDataRow<DataRow>(rowIndex);
            return tmpDt;
        }

        /// <inheritdoc/>
        public DataRow GetNeedChangeDataRow(int rowIndex)
        {
            DataRow tmpDt = this.gridAutoPick.GetDataRow<DataRow>(rowIndex);
            return tmpDt;
        }

        /// <inheritdoc/>
        public void DictionaryDatasRejectChanges()
        {
            var d = this.DictionaryDatas.AsEnumerable().ToList();
            for (int i = 0; i < d.Count; i++)
            {
                d[i].Value.RejectChanges();
            }

            return;
        }

        /// <inheritdoc/>
        public void DictionaryDatasAcceptChanges()
        {
            // 批次RejectChanges
            var d = this.DictionaryDatas.AsEnumerable().ToList();
            for (int i = 0; i < d.Count; i++)
            {
                d[i].Value.AcceptChanges();
            }

            return;
        }

        private void BtnMNnotice_Click(object sender, EventArgs e)
        {
            if (this.drMnOrder != null)
            {
                if (MyUtility.Check.Empty(this.drMnOrder["SMnorderApv"]) && MyUtility.Check.Empty(this.drMnOrder["MnorderApv"]))
                {
                    MyUtility.Msg.WarningBox("M/Notice did not approve yet, you cannot print M/Notice.");
                    return;
                }

                if (MyUtility.Check.Empty(this.drMnOrder["SMnorderApv"]))
                {
                    var frm = new P01_MNoticePrint(null, this.orderid);
                    frm.ShowDialog(this);
                    return;
                }
                else
                {
                    SMNoticePrg.PrintSMNotice(this.poid, SMNoticePrg.EnuPrintSMType.Order);
                }
            }
        }

        private void BtnHangtag_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.strLabel))
            {
                Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(this.strLabel, "Label & Hangtag", false, null);
                callNextForm.ShowDialog(this);
            }
        }

        private void BtnPackingMethod_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.strPacking))
            {
                Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(this.strPacking, "Packing Method", false, null);
                callNextForm.ShowDialog(this);
            }
        }
    }
}
