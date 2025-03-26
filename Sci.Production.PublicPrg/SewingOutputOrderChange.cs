using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Sci.Production.Prg
{
    /// <summary>
    /// SewingOutputOrderChange
    /// </summary>
    public class SewingOutputOrderChange
    {

        private DataRow CurrentMaintain;
        private DataTable dtDetail;
        private string loginUser;
        private bool isErrorReturnImmediate;
        private AbstractDBProxyPMS proxyPMS;

        /// <summary>
        /// SewingOutputOrderChange
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="userID">userID</param>
        /// <param name="proxyPMS">proxyPMS</param>
        /// <param name="isErrorReturnImmediate">isErrorReturnImmediate</param>
        public SewingOutputOrderChange(DataRow currentMaintain, DataTable dtDetail, string userID, AbstractDBProxyPMS proxyPMS, bool isErrorReturnImmediate = true)
        {
            this.CurrentMaintain = currentMaintain;
            this.dtDetail = dtDetail;
            this.loginUser = userID;
            this.isErrorReturnImmediate = isErrorReturnImmediate;
            this.proxyPMS = proxyPMS;
            if (!this.dtDetail.Columns.Contains("ErrMsg"))
            {
                this.dtDetail.Columns.Add(new DataColumn("ErrMsg", typeof(string)));
            }
        }

        /// <summary>
        /// Confirm
        /// </summary>
        /// <param name="isNeedUpdateDetail">isNeedUpdateDetail</param>
        /// <returns>DualResult</returns>
        public DualResult Confirm(bool isNeedUpdateDetail = true)
        {
            DualResult result;
            IList<DataRow> detailDatas = this.dtDetail.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).ToList();

            result = this.CheckTransferQty(detailDatas);
            if (!result)
            {
                return result;
            }

            result = this.CheckOrderQtyUpperlimit(detailDatas);
            if (!result)
            {
                return result;
            }

            try
            {
                TransactionOptions tOpt = default(TransactionOptions);
                tOpt.Timeout = new TimeSpan(0, 30, 0);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, tOpt))
                {
                    result = this.UpdateSewingOutputTransfer(isNeedUpdateDetail);
                    if (!result)
                    {
                        scope.Dispose();
                        return result;
                    }

                    result = this.TransferQty();
                    if (!result)
                    {
                        scope.Dispose();
                        return result;
                    }

                    result = this.UpdateMESInspection(detailDatas);
                    if (!result)
                    {
                        scope.Dispose();
                        return result;
                    }

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                return new DualResult(false, e);
            }

            return new DualResult(true);
        }

        private DualResult TransferQty()
        {
            #region 準備分配資料與計算 轉走Qty, 轉至Qty
            string sqlcmd = $@"
-- 準備要尋找轉出來源<From> SP,ComboType,Artile,SizeCode組合，以及將要轉移總數
select t.FromOrderID,t.FromComboType,t.Article,t.SizeCode
into #tmpFromG
from #tmp t
group by t.FromOrderID,t.FromComboType,t.Article,t.SizeCode

--準備可轉移數資料, 依據<From> ID, SP,ComboType, Artile, SizeCode
SELECT so.OutputDate,sodd.*,
	CanTransQty = isnull(sodd.QAQty,0) - isnull(G.QAQty,0), -- 此筆Sdd剩下可轉移數
	TransferredQty = 0 -- 用來記錄轉走Qty
into #tmpByIDCanTransQty
FROM #tmpFromG t
INNER JOIN SewingOutput_Detail_Detail sodd with(nolock) on -- 找要轉出的資料(必定有)
	sodd.OrderId = t.FromOrderID and sodd.ComboType = t.FromComboType
	and sodd.Article = t.Article and sodd.SizeCode = t.SizeCode
inner join SewingOutput so with(nolock) on so.id = sodd.ID and so.FactoryID = '{this.CurrentMaintain["FactoryID"]}'
outer apply(
	select QAQty=sum(soddg.QAQty) -- 找G單(虛數量) 有可能多筆
	from SewingOutput_Detail_Detail_Garment soddg with(nolock) 
	where soddg.ID = sodd.ID -- 同SewingOutput單下
	and soddg.OrderIDfrom = sodd.OrderId and soddg.ComboType = sodd.ComboType
	and soddg.Article = sodd.Article and soddg.SizeCode = sodd.SizeCode
)G
where  isnull(sodd.QAQty,0) - isnull(G.QAQty,0) > 0

--By From & To 所有欄組合 加總需轉移數 (表身可輸入同樣組合多筆)
select t.FromOrderID,t.FromComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode,TransferQty=sum(t.TransferQty)
into #tmpFromToG
from #tmp t
group by t.FromOrderID,t.FromComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode

-- 可轉移數資料, 再依據<To 4欄位> 資料展開-- WillTransferQty最後要用來更新sdd, 或是新增sdd,sd用
select t.OutputDate,t.ID,t.OrderId,t.ComboType,t.Article,t.SizeCode,t.SewingOutput_DetailUKey,
	t2.ToOrderID,t2.ToComboType,t2.ToArticle,t2.ToSizeCode,
	WillTransferQty = 0 -- 用來紀錄轉進Qty
into #tmpUp
from #tmpByIDCanTransQty t
inner join #tmpFromToG t2 on t2.FromOrderID = t.OrderId and t2.FromComboType = t.ComboType and t2.Article = t.Article and t2.SizeCode = t.SizeCode

select*from #tmpByIDCanTransQty t order by t.OutputDate,t.ID,t.OrderId,t.ComboType,t.Article,t.SizeCode
select*from #tmpFromToG t  order by t.FromOrderID,t.FromComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode
select*from #tmpUp t order by t.OutputDate,t.ID,t.OrderId,t.ComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode

--檢查OrderId在Order_Location是否有資料，沒資料就補 此處只檢查轉To SP，From SP是P01已有資料，P01存檔時檢查 ※Sewing_P01
DECLARE CUR_SewingOutput_Detail CURSOR FOR 
    Select distinct orderid = ToOrderID from #tmpUp t
declare @orderid varchar(13) 
OPEN CUR_SewingOutput_Detail   
FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid 
WHILE @@FETCH_STATUS = 0 
BEGIN
    exec dbo.Ins_OrderLocation @orderid
FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid
END
CLOSE CUR_SewingOutput_Detail
DEALLOCATE CUR_SewingOutput_Detail

drop table #tmp,#tmpFromG,#tmpByIDCanTransQty,#tmpFromToG,#tmpUp
";
            DataTable[] dt;
            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = this.proxyPMS.ProcessWithDatatable("Production", this.dtDetail, string.Empty, sqlcmd, out dt);
            if (!result)
            {
                return result;
            }

            try
            {
                // 開始分配計算
                // dt[0]的TransferredQty是紀錄轉出數，要回到轉出來源將此數量扣除
                // dt[2]的WillTransferQty是紀錄轉入目標數量，若目標已有這筆組合，則加上這數量
                foreach (DataRow r2 in dt[2].Rows)
                {
                    DataRow[] dr0s = dt[0].Select($@"ID = '{r2["ID"]}' and OutputDate='{r2["OutputDate"]}' and OrderId='{r2["OrderId"]}' and ComboType='{r2["ComboType"]}' and Article = '{r2["Article"]}' and SizeCode='{r2["SizeCode"]}'"); // 必有, dt2是dt0為主展開
                    DataRow[] dr1s = dt[1].Select($@"FromOrderID='{r2["OrderId"]}' and FromComboType='{r2["ComboType"]}' and Article = '{r2["Article"]}' and SizeCode='{r2["SizeCode"]}' and ToOrderID='{r2["ToOrderID"]}' and ToComboType='{r2["ToComboType"]}' and ToArticle='{r2["ToArticle"]}' and ToSizeCode='{r2["ToSizeCode"]}'");

                    // 準備資料已Group過, 只會找到對應一筆, 或沒有
                    if (dr1s.Count() > 0)
                    {
                        if (MyUtility.Convert.GetInt(dr0s[0]["CanTransQty"]) >= MyUtility.Convert.GetInt(dr1s[0]["TransferQty"]))
                        {
                            r2["WillTransferQty"] = dr1s[0]["TransferQty"]; // 紀錄此筆將轉Qty. 增到sdd, 或<加>到已有sdd 的數量
                            dr0s[0]["CanTransQty"] = MyUtility.Convert.GetInt(dr0s[0]["CanTransQty"]) - MyUtility.Convert.GetInt(r2["WillTransferQty"]); // 還剩餘數
                            dr1s[0]["TransferQty"] = 0; // 紀錄還須轉移數
                        }
                        else
                        {
                            r2["WillTransferQty"] = dr0s[0]["CanTransQty"]; // 紀錄此筆將轉Qty. 增到sdd, 或<加>到已有sdd 的數量
                            dr1s[0]["TransferQty"] = MyUtility.Convert.GetInt(dr1s[0]["TransferQty"]) - MyUtility.Convert.GetInt(r2["WillTransferQty"]); // 還須轉移數
                            dr0s[0]["CanTransQty"] = 0; // 還剩餘數
                        }

                        dr0s[0]["TransferredQty"] = MyUtility.Convert.GetInt(dr0s[0]["TransferredQty"]) + MyUtility.Convert.GetInt(r2["WillTransferQty"]); // 紀錄轉走數
                        dr0s[0].EndEdit();
                        dr1s[0].EndEdit();
                        r2.EndEdit();
                    }
                }
            }
            catch (Exception e)
            {
                return new DualResult(false, e);
            }

            DataTable fromDt = dt[0];
            DataTable toDt = dt[2];
            DataTable dto;
            #endregion

            #region 更新/寫入SewingOutput第3層，第2層 在分配完數量後

            // 回找到Form SP 對應 sdd 更新 QAQty = QAQty - 轉走數。 PS:不是剩餘數(QAQty-G單數)
            sqlcmd = $@"
select t.ID, t.OrderId, t.ComboType, t.Article, t.SizeCode, TransferredQty = sum(t.TransferredQty)
into #tmpupdate
from #tmp t
group by t.ID, t.OrderId, t.ComboType, t.Article, t.SizeCode

update sodd set
	sodd.QAQty = sodd.QAQty - t.TransferredQty --減去轉走數
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID
	and sodd.OrderId = t.OrderId and sodd.ComboType = t.ComboType and sodd.Article = t.Article and sodd.SizeCode = t.SizeCode
where t.TransferredQty > 0

update sod set
    sod.AutoSplit=1
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID
	and sodd.OrderId = t.OrderId and sodd.ComboType = t.ComboType and sodd.Article = t.Article and sodd.SizeCode = t.SizeCode
inner join SewingOutput_Detail sod on sodd.SewingOutput_DetailUKey = sod.Ukey
where t.TransferredQty > 0 -- 找到有更新第3層對應第2層更新
";
            result = this.proxyPMS.ProcessWithDatatable("Production", fromDt, string.Empty, sqlcmd, out dto);
            if (!result)
            {
                return result;
            }

            // 找to SP 對應 sdd, 更新/新增
            sqlcmd = $@"
--更新已有第3層sdd
select t.ID, t.ToOrderID, t.ToComboType, t.ToArticle, t.ToSizeCode, WillTransferQty = sum(t.WillTransferQty)
into #tmpupdate
from #tmp t
group by t.ID, t.ToOrderID, t.ToComboType, t.ToArticle, t.ToSizeCode

update sodd set
	sodd.QAQty = sodd.QAQty + t.WillTransferQty -- 已有的數量加上轉移數
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
where t.WillTransferQty > 0

--寫入第2層sd有,但第3層sdd未有
insert into SewingOutput_Detail_Detail(ID, SewingOutput_DetailUKey, OrderId, ComboType, Article, SizeCode, QAQty, OldDetailKey)
select t.ID,
	SewingOutput_DetailUKey=sod.UKey,
	t.ToOrderID,
    t.ToComboType,
    t.ToArticle,
    t.ToSizeCode,
    WillTransferQty=sum(t.WillTransferQty),
	OldDetailKey=''
from #tmp t
inner join SewingOutput_Detail sod with(nolock) on sod.id = t.ID and sod.OrderId = t.ToOrderID and sod.ComboType = t.ToComboType and sod.Article = t.ToArticle
left join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
where sodd.id is null
and t.WillTransferQty > 0
group by t.ID, sod.UKey, t.ToOrderID, t.ToComboType, t.ToArticle, t.ToSizeCode

--第2層也沒有的資料
--先寫第2層
Declare @SewingOutput_Detail table(
	[ID] [varchar](13) NOT NULL,
	[OrderId] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL,
	[Article] [varchar](8) NULL,
	[Color] [varchar](6) NULL,
	[TMS] [int] NULL,
	[HourlyStandardOutput] [int] NULL,
	[WorkHour] [numeric](6, 3) NOT NULL,
	[UKey] [bigint] NOT NULL,
	[QAQty] [int] NULL,
	[DefectQty] [int] NULL,
	[InlineQty] [int] NULL,
	[OldDetailKey] [varchar](13) NULL,
	[AutoCreate] [bit] NULL,
	[SewingReasonID] [varchar](5) NOT NULL,
	[Remark] [nvarchar](1000) NULL,
	[ImportFromDQS] [bit] NOT NULL,
    [AutoSplit] [bit] NULL
)
INSERT INTO [dbo].[SewingOutput_Detail]
([ID],[OrderId],[ComboType],[Article],[Color],[TMS],[HourlyStandardOutput],[WorkHour],[QAQty],[DefectQty],[InlineQty],[OldDetailKey],[AutoCreate],[SewingReasonID],[Remark],[ImportFromDQS],[AutoSplit])
OUTPUT INSERTED.ID
    , INSERTED.OrderId
    , INSERTED.ComboType
    , INSERTED.Article
    , INSERTED.Color
    , INSERTED.TMS
    , INSERTED.HourlyStandardOutput
    , INSERTED.WorkHour
    , INSERTED.UKey
    , INSERTED.QAQty
    , INSERTED.DefectQty
    , INSERTED.InlineQty
    , INSERTED.OldDetailKey
    , INSERTED.AutoCreate
    , INSERTED.SewingReasonID
    , INSERTED.Remark
    , INSERTED.ImportFromDQS
    , INSERTED.AutoSplit
INTO @SewingOutput_Detail  -- 取得寫入的資料,ukey欄位
select t.ID,t.ToOrderID,t.ToComboType,t.ToArticle,
	Color = (select top 1 ColorID from View_OrderFAColor where Id = t.ToOrderID and Article = t.ToArticle),--※Sewing_P01
	TMS = isnull(Round(o.CPU * o.CPUFactor * (r.rate / 100) * (select StdTMS from System WITH (NOLOCK)), 0),0),--※Sewing_P01
	HourlyStandardOutput = isnull(h.StandardOutput,0),
	WorkHour = 0,-- 先給0下方再重算
	[QAQty] = sum(t.WillTransferQty),--第3層加總
	[DefectQty] = 0, -- P11不處理DefectQty
	[InlineQty] = 0, -- P11不處理InlineQty
	OldDetailKey = '',
	[AutoCreate] = 0,
	[SewingReasonID] = '',
	[Remark] = '',
	[ImportFromDQS] = 0,
    AutoSplit = 1
from #tmp t
inner join orders o WITH (NOLOCK) on o.id = t.ToOrderID
inner join SewingOutput so WITH (NOLOCK) on so.ID = t.ID
left join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
outer apply(select Rate = isnull([dbo].GetOrderLocation_Rate(t.ToOrderID ,t.ComboType), ([dbo].GetStyleLocation_Rate(o.StyleUkey, t.ComboType))))r--※Sewing_P01
outer apply(
    select top 1 StandardOutput
    from SewingSchedule s WITH (NOLOCK)
    where s.OrderID = t.ToOrderID and s.ComboType = t.ToComboType and s.SewingLineID = so.SewingLineID
)h--※Sewing_P01
where sodd.id is null
and not exists(select 1 from SewingOutput_Detail sod with(nolock) where sod.id = t.ID and sod.OrderId = t.ToOrderID and sod.ComboType = t.ToComboType and sod.Article = t.ToArticle)
and t.WillTransferQty > 0
group by t.ID,t.ToOrderID,t.ToComboType,t.ToArticle,o.CPU,o.CPUFactor,r.Rate,h.StandardOutput

--再寫第3層
INSERT INTO [dbo].[SewingOutput_Detail_Detail]
([ID],[SewingOutput_DetailUKey],[OrderId],[ComboType],[Article],[SizeCode],[QAQty],[OldDetailKey])
select s.ID,s.UKey,s.OrderId,s.ComboType,s.Article,t.ToSizeCode,QAQty=sum(t.WillTransferQty),''
from #tmp t
inner join @SewingOutput_Detail s on s.ID = t.ID and s.OrderId = t.ToOrderID and s.ComboType = t.ToComboType and s.Article = t.ToArticle
where t.WillTransferQty > 0
group by s.ID,s.UKey,s.OrderId,s.ComboType,s.Article,t.ToSizeCode

--以上第3層都寫入後
update sod set
    sod.AutoSplit=1
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd with(nolock) on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
inner join SewingOutput_Detail sod with(nolock) on sodd.SewingOutput_DetailUKey = sod.Ukey
where t.WillTransferQty > 0 -- 找到有更新/新增第3層, 對應第2層

--更新表頭
update so set
    so.EditName ='{this.loginUser}',
    so.EditDate = GetDate(),
    so.ReDailyTransferDate = GetDate()    
from SewingOutput so
where so.id in(select distinct id from #tmp t where t.WillTransferQty > 0 )

--刪除為0的第3層，等上面更新完再執行
delete SewingOutput_Detail_Detail where QAQty=0 and id in(select distinct id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料

--重算第2層 QAQty ※Sewing_P01
update SD set SD.QAQty = SDD.SDD_Qty
from  SewingOutput_Detail SD WITH (NOLOCK)
outer apply 
( 
    select isnull(SUM(SDD.QAQty),0) as SDD_Qty from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.ID=SD.ID and SDD.SewingOutput_DetailUKey=SD.UKey 
) as SDD 
where SD.QAQty <> SDD.SDD_Qty and SD.ID in (select distinct t.id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料

--重算表頭TMS, Efficiency (在重算完 第2層 QAQty 之後) ※Sewing_P01
select id, tms = cast(TMS as numeric(24,10)) * QAQty / sum(QAQty) over(partition by id), QAQty
into #tmpdTms
from SewingOutput_Detail sod with(nolock)
where sod.AutoCreate <> 1
and sod.id in (select distinct id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料

select id,TMS = round(Sum(tms), 0), QAQty = sum(QAQty) into #tmp_upTms from #tmpdTms group by id
--不更新表頭QAQty，因為正確狀況下總數不變動
update so set
    TMS = t.TMS,
    Efficiency = iif(t.TMS * so.ManHour = 0, 0, cast(t.QAQty as numeric(24,10))/ (3600 / t.TMS * so.ManHour) * 100)
from #tmp_upTms t
inner join SewingOutput so with(nolock) on so.id = t.id

--重算第2層 WorkHour 撈需要重算資料 (在重算完 第2層 QAQty 之後) ※Sewing_P01
select ID,
    sumQaqty = sum(isnull(QAQty,0) * isnull(TMS,0)),
    RecCnt = count(1)
from SewingOutput_Detail sod with(nolock)
where sod.AutoCreate <> 1
and sod.id in (select distinct id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料
group by ID
";
            DataTable sumQaQty;
            result = this.proxyPMS.ProcessWithDatatable("Production", toDt, string.Empty, sqlcmd, out sumQaQty);
            if (!result)
            {
                return result;
            }
            #endregion

            #region 重新計算 Cumulate
            sqlcmd = $@"
DECLARE @Line as varchar(5)
	, @FactoryID as varchar(8) 
	, @OutputDate as date 


DECLARE cursorSewingOutput 
CURSOR FOR
	select s.SewingLineID, s.FactoryID, s.OutputDate
    from SewingOutput s
    where s.id in(select distinct id from #tmp t where t.WillTransferQty > 0 )
	group by s.SewingLineID, s.FactoryID, s.OutputDate
	order by s.OutputDate

OPEN cursorSewingOutput
FETCH NEXT FROM cursorSewingOutput INTO @Line, @FactoryID, @OutputDate

WHILE @@FETCH_STATUS = 0
BEGIN
    exec RecalculateCumulateDay @Line, @FactoryID, @OutputDate

    FETCH NEXT FROM cursorSewingOutput INTO @Line, @FactoryID, @OutputDate
END


CLOSE cursorSewingOutput
DEALLOCATE cursorSewingOutput
";
            result = this.proxyPMS.ProcessWithDatatable("Production", toDt, string.Empty, sqlcmd, out DataTable dataTable);
            if (!result)
            {
                return result;
            }
            #endregion

            #region 重新計算 Inline Category
            sqlcmd = $@"
DECLARE @ID varchar(13)

DECLARE cursorInlineCategory
CURSOR FOR
	select distinct s.ID
    from SewingOutput s
    where s.id in(select distinct id from #tmp t where t.WillTransferQty > 0 )

OPEN cursorInlineCategory
FETCH NEXT FROM cursorInlineCategory INTO @ID

WHILE @@FETCH_STATUS = 0
BEGIN
    exec RecalculateSewingInlineCategory @ID

    FETCH NEXT FROM cursorInlineCategory INTO @ID
END


CLOSE cursorInlineCategory
DEALLOCATE cursorInlineCategory
";
            result = this.proxyPMS.ProcessWithDatatable("Production", toDt, string.Empty, sqlcmd, out dataTable);
            if (!result)
            {
                return result;
            }
            #endregion

            #region 重算第2層 WorkHour (在重算完 第2層 QAQty 之後) ※Sewing_P01
            foreach (DataRow item in sumQaQty.Rows)
            {
                int recCnt = MyUtility.Convert.GetInt(item["RecCnt"]);
                decimal ttlQaqty = MyUtility.Convert.GetDecimal(item["sumQaqty"]);
                decimal subSum = 0;
                sqlcmd = $@"select * from SewingOutput sod with(nolock) where id = '{item["id"]}'";
                DataTable dtid;
                result = this.proxyPMS.Select("Production", sqlcmd, null, out dtid);
                if (!result)
                {
                    return result;
                }

                decimal workHour = MyUtility.Convert.GetDecimal(dtid.Rows[0]["WorkHour"]); // 取得表頭 WorkHour 總數
                sqlcmd = $@"select * from SewingOutput_Detail sod with(nolock) where AutoCreate <> 1 and id = '{item["id"]}'";
                result = this.proxyPMS.Select("Production", sqlcmd, null, out dtid);
                if (!result)
                {
                    return result;
                }

                foreach (DataRow dr in dtid.Rows)
                {
                    recCnt = recCnt - 1;
                    if (recCnt == 0)
                    {
                        dr["WorkHour"] = workHour - subSum;
                    }
                    else
                    {
                        dr["WorkHour"] = ttlQaqty == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["QAQty"]) * MyUtility.Convert.GetDecimal(dr["TMS"]) / ttlQaqty * workHour, 3);
                    }

                    subSum = subSum + MyUtility.Convert.GetDecimal(dr["WorkHour"]);
                }

                // 重算好的WorkHour寫回 SewingOutput_Detail
                sqlcmd = $@"
update sod set
    WorkHour = t.WorkHour
from SewingOutput_Detail sod with(nolock)
inner join #tmp t on t.[UKey] = sod.[UKey]
";
                result = this.proxyPMS.ProcessWithDatatable("Production", dtid, string.Empty, sqlcmd, out dto);
                if (!result)
                {
                    return result;
                }
            }
            #endregion

            DBProxy.Current.DefaultTimeout = 300;

            return new DualResult(true);
        }

        private DualResult UpdateMESInspection(IList<DataRow> detailDatas)
        {
            string sqlcmd = string.Empty;
            int idx = 0;

            // 跑 P11 表身迴圈，在MES inspection一筆資料是一件，更新是用 Top (TransferQty) 數量。一筆轉移就只會有一段update
            foreach (DataRow item in detailDatas)
            {
                sqlcmd += $@"
select top {item["TransferQty"]} ID, AddDate
INTO #tmp{idx}
from Inspection with(nolock)
where Status in ('Pass','Fixed') and OrderId = '{item["FromOrderID"]}' and Location = '{item["FromComboType"]}'
and Article='{item["Article"]}' and Size='{item["SizeCode"]}'
order by AddDate 

update Inspection set 
	OrderId = '{item["ToOrderID"]}',
	Location = '{item["ToComboType"]}', 
	Article = '{item["ToArticle"]}', 
	Size = '{item["ToSizeCode"]}',  
	StyleUkey = {item["styleUkey"]}, 
	SewingOutputTransfer_DetailUkey = {item["Ukey"]}
from Inspection
WHERE ID IN (
    SELECT ID FROM #tmp{idx}
)
;
DROP TABLE #tmp{idx}
;

";
                idx++;
            }

            DualResult result = this.proxyPMS.Execute(sqlcmd, "ManufacturingExecution");
            if (!result)
            {
                return result;
            }

            return new DualResult(true);
        }

        private DualResult UpdateSewingOutputTransfer(bool isNeedUpdateDetail)
        {
            string sqlcmd = $@"
update SewingOutputTransfer set
    Status ='Confirmed',
    EditDate =GetDate(),
    EditName = '{this.loginUser}'
where id = '{this.CurrentMaintain["ID"]}'
    
";
            if (isNeedUpdateDetail)
            {
                sqlcmd += @"
update sotd set
    sotd.FromQty = t.DisplayFromQty,
    sotd.FromSewingQty  = t.DisplayFromSewingQty,
    sotd.ToQty = t.DisplayToQty,
    sotd.ToSewingQty = t.DisplayToSewingQty
from SewingOutputTransfer_Detail sotd
inner join #tmp t on t.ukey = sotd.ukey and sotd.id = t.id
";
            }

            DataTable dt;
            DualResult result;
            if (isNeedUpdateDetail)
            {
                result = this.proxyPMS.ProcessWithDatatable("Production", this.dtDetail, string.Empty, sqlcmd, out dt);
            }
            else
            {
                result = this.proxyPMS.Execute(sqlcmd, "Production");
            }

            if (!result)
            {
                return result;
            }

            return new DualResult(true);
        }

        /// <summary>
        /// BeforeSaveCheck
        /// </summary>
        /// <param name="errorDetailPosition">errorDetailPosition</param>
        /// <returns>DualResult</returns>
        public DualResult BeforeSaveCheck(out int errorDetailPosition)
        {
            errorDetailPosition = -1;
            IList<DataRow> detailDatas = this.dtDetail.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).ToList();
            DualResult result;

            string errMsg = string.Empty;
            for (int i = 0; i < detailDatas.Count; i++)
            {
                if (MyUtility.Check.Empty(detailDatas[i]["FromOrderID"]))
                {
                    errMsg = "From SP# can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["FromComboType"]))
                {
                    errMsg = "* can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["Article"]))
                {
                    errMsg = "Article can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["SizeCode"]))
                {
                    errMsg = "Size can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["ToOrderID"]))
                {
                    errMsg = "To SP# can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["ToComboType"]))
                {
                    errMsg = "* can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["ToArticle"]))
                {
                    errMsg = "Article can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["ToSizeCode"]))
                {
                    errMsg = "Size can't empty!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Check.Empty(detailDatas[i]["TransferQty"]) || MyUtility.Convert.GetInt(detailDatas[i]["TransferQty"]) == 0)
                {
                    errMsg = "The transfer qty can't be 0!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                if (MyUtility.Convert.GetString(detailDatas[i]["FromOrderID"]) == MyUtility.Convert.GetString(detailDatas[i]["ToOrderID"]))
                {
                    errMsg = "From SP# cannot be the same as To SP#";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }

                #region 檢查：款式是否相同，以及可轉出數量是否足夠

                string fromOrderID = MyUtility.Convert.GetString(detailDatas[i]["FromOrderID"]);
                string toOrderID = MyUtility.Convert.GetString(detailDatas[i]["ToOrderID"]);
                string fromComboType = MyUtility.Convert.GetString(detailDatas[i]["FromComboType"]);
                string fromArticle = MyUtility.Convert.GetString(detailDatas[i]["Article"]);

                string fromSizeCode = MyUtility.Convert.GetString(detailDatas[i]["SizeCode"]);

                string cmd = $@"
select 1
from Orders o
where o.ID='{fromOrderID}'
AND EXISTS(
	select 1 
	from Orders
	where ID='{toOrderID}' AND StyleID = o.StyleID
)
";

                // 判斷From To SP是否同款
                bool isSameStyle = this.proxyPMS.Seek(cmd, "Production");

                if (isSameStyle)
                {
                    // 同款式的話，上鎖+未上鎖 >= 轉移數量即可
                    cmd = $@"
select 1
from SewingOutput s
INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
INNER JOIN SewingOutput_Detail_Detail sdd ON sdd.SewingOutput_DetailUKey = sd.UKey
WHERE sd.OrderId='{fromOrderID}' AND sdd.ComboType='{fromComboType}' AND sdd.Article='{fromArticle}' AND sdd.SizeCode='{fromSizeCode}'
HAVING SUM(sdd.QAQty) >= '{detailDatas[i]["TransferQty"]}'
";
                }
                else
                {
                    // 不同款的話，只能從未上鎖的轉出，因此只需判斷未上鎖數量
                    cmd = $@"
select 1
from SewingOutput s
INNER JOIN SewingOutput_Detail sd ON s.ID = sd.ID
INNER JOIN SewingOutput_Detail_Detail sdd ON sdd.SewingOutput_DetailUKey = sd.UKey
WHERE sd.OrderId='{fromOrderID}' AND s.Status != 'Locked' AND sdd.ComboType='{fromComboType}' AND sdd.Article='{fromArticle}' AND sdd.SizeCode='{fromSizeCode}'
HAVING SUM(sdd.QAQty) >= '{detailDatas[i]["TransferQty"]}'
";
                }

                if (!this.proxyPMS.Seek(cmd, "Production"))
                {
                    errMsg = "Some [From SP#] sewing output already locked cannot transfer to other style!";
                    if (this.isErrorReturnImmediate)
                    {
                        errorDetailPosition = i;
                        return new DualResult(false, errMsg);
                    }

                    detailDatas[i]["ErrMsg"] += errMsg + Environment.NewLine;
                }
                #endregion
            }

            result = this.CheckTransferQty(detailDatas);
            if (!result)
            {
                return result;
            }

            result = this.CheckOrderQtyUpperlimit(detailDatas);
            if (!result)
            {
                return result;
            }

            return new DualResult(true);
        }

        private DualResult CheckTransferQty(IList<DataRow> detailDatas)
        {
            string errorMsg = string.Empty;

            // FromOrderID,FromComboType,Article,SizeCode組合判斷TransferQty(移轉數)不可大於FromSewingQty(來源數)
            var listCheckDataBase = detailDatas
                                .Where(s => s.RowState != DataRowState.Deleted)
                                .GroupBy(s => new
                                {
                                    FromOrderID = s["FromOrderID"].ToString(),
                                    FromComboType = s["FromComboType"].ToString(),
                                    Article = s["Article"].ToString(),
                                    SizeCode = s["SizeCode"].ToString(),
                                    PackingQty = MyUtility.Convert.GetInt(s["PackingQty"]),
                                    DisplayFromSewingQty = MyUtility.Convert.GetInt(s["DisplayFromSewingQty"]),
                                })
                                .Select(g => new
                                {
                                    g.Key.FromOrderID,
                                    g.Key.FromComboType,
                                    g.Key.Article,
                                    g.Key.SizeCode,
                                    g.Key.PackingQty,
                                    g.Key.DisplayFromSewingQty,
                                    TransferQty = g.Sum(s => MyUtility.Convert.GetInt(s["TransferQty"])),
                                    DetailRows = g,
                                });

            var listSewingQtyOver = listCheckDataBase.Where(w => w.TransferQty > w.DisplayFromSewingQty);
            if (listSewingQtyOver.Any())
            {
                if (this.isErrorReturnImmediate)
                {
                    errorMsg = "<Transfer Qty> can not more than <From SP# Sewing Output Qty>!" + Environment.NewLine +
                                  listSewingQtyOver.Select(s => $"From SP#: {s.FromOrderID}, {s.FromComboType}, Article:{s.Article}, Size:{s.SizeCode}").JoinToString(Environment.NewLine);
                    return new DualResult(false, errorMsg);
                }

                foreach (var itemSewingQtyOver in listSewingQtyOver)
                {
                    foreach (DataRow dr in itemSewingQtyOver.DetailRows)
                    {
                        dr["ErrMsg"] += "<Transfer Qty> can not more than <From SP# Sewing Output Qty>!" + Environment.NewLine;
                    }
                }
            }

            var listSewingPackQtyOver = listCheckDataBase.Where(w => w.TransferQty > (w.DisplayFromSewingQty - w.PackingQty));
            if (listSewingPackQtyOver.Any())
            {
                if (this.isErrorReturnImmediate)
                {
                    errorMsg = "<Transfer Qty> can not more than <From SP# Packing Qty>!" + Environment.NewLine +
                                  listSewingPackQtyOver.Select(s => $"From SP#: {s.FromOrderID}, {s.FromComboType}, Article:{s.Article}, Size:{s.SizeCode}, Packing Qty:{s.PackingQty.ToString()}").JoinToString(Environment.NewLine);
                    return new DualResult(false, errorMsg);
                }

                foreach (var itemSewingPackQtyOver in listSewingPackQtyOver)
                {
                    foreach (DataRow dr in itemSewingPackQtyOver.DetailRows)
                    {
                        dr["ErrMsg"] += "<Transfer Qty> can not more than <From SP# Packing Qty>!" + Environment.NewLine;
                    }
                }
            }

            return new DualResult(true);
        }

        private DualResult CheckOrderQtyUpperlimit(IList<DataRow> detailDatas)
        {
            // ToOrderID,ToComboType,ToArticle,ToSizeCode 組合判斷 TransferQty(移轉數)加DisplayToSewingQty(已報產出數) 不可大於 OrderQtyUpperlimit(實際上限)
            var list = detailDatas.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            var list2 = list.GroupBy(s => new
            {
                ToOrderID = s["ToOrderID"].ToString(),
                ToComboType = s["ToComboType"].ToString(),
                ToArticle = s["ToArticle"].ToString(),
                ToSizeCode = s["ToSizeCode"].ToString(),
                DisplayToSewingQty = MyUtility.Convert.GetInt(s["DisplayToSewingQty"]),
                OrderQtyUpperlimit = MyUtility.Convert.GetInt(s["OrderQtyUpperlimit"]),
            })
            .Select(g => new
            {
                g.Key.ToOrderID,
                g.Key.ToComboType,
                g.Key.ToArticle,
                g.Key.ToSizeCode,
                g.Key.DisplayToSewingQty,
                g.Key.OrderQtyUpperlimit,
                TransferQty = g.Sum(s => MyUtility.Convert.GetInt(s["TransferQty"])),
                DetailRows = g,
            }).Where(w => w.TransferQty + w.DisplayToSewingQty > w.OrderQtyUpperlimit).ToList();

            if (list2.Count > 0)
            {
                string msg = "< Transfer Qty > + < To SP# Sewing Output Qty > can not more than < To SP# Qty >!";
                foreach (var item in list2)
                {
                    msg += $"\r\nTo SP#: {item.ToOrderID}, {item.ToComboType}, Article:{item.ToArticle}, Size:{item.ToSizeCode}";
                }

                if (this.isErrorReturnImmediate)
                {
                    return new DualResult(false, msg);
                }

                foreach (var item in list2)
                {
                    foreach (DataRow dr in item.DetailRows)
                    {
                        dr["ErrMsg"] += "< Transfer Qty > + < To SP# Sewing Output Qty > can not more than < To SP# Qty >!" + Environment.NewLine;
                    }
                }
            }

            return new DualResult(true);
        }
    }
}
