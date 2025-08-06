using System.Data;
using System.Collections.Generic;
using System.Transactions;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System;

using Ict;

using Sci.Production.Class;
using Sci.Production.Class.Commons;
using Sci.Win.Tools;
using Sci.Win.UI;

using Check = Sci.MyUtility.Check;
using Msg = Sci.MyUtility.Msg;
using SciConvert = Sci.MyUtility.Convert;
using Sci.Data;
using Sci.Andy.ExtensionMethods;
using static Sci.Production.Class.CrossSystemCommons.APICommon;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Xml.Linq;

namespace Sci.Production.Class.Commons
{
    /// <inheritdoc />
    public class AccountPayable
    {
        /// <summary>
        /// status_Junked
        /// </summary>
        public const string status_Junked = "Junked";

        /// <summary>
        /// status_Approved
        /// </summary>
        public const string status_Approved = "Approved";

        /// <summary>
        /// status_Sent
        /// </summary>
        public const string status_Sent = "Sent";

        /// <summary>
        /// status_Sent
        /// </summary>
        public const string status_MMSApv = "MMS Apv";

        /// <summary>
        /// status_New
        /// </summary>
        public const string status_New = "New";

        /// <summary>
        /// status_SMRConfirm
        /// </summary>
        public const string status_SMRConfirm = "SMR Confirm";

        /// <summary>
        /// status_LeaderConfirm
        /// </summary>
        public const string status_LeaderConfirm = "Leader Confirm";

        /// <summary>
        /// status_Recall
        /// </summary>
        public const string status_Recall = "Recall";

        /// <summary>
        /// status_Junked_Remark
        /// </summary>
        public const string status_Junked_Remark = "update status from New to Junked.";

        /// <summary>
        /// status_Approved_Remark
        /// </summary>
        public const string status_Approved_Remark = "update status from Sent to Approved.";

        /// <summary>
        /// status_Unapproved_Remark
        /// </summary>
        public const string status_Unapproved_Remark = "update status from Approved to Sent.";

        /// <summary>
        /// status_Sent_Remark
        /// </summary>
        public const string status_Sent_Remark = "update status from New to Sent.";

        /// <summary>
        /// status_Recall_Remark
        /// </summary>
        public const string status_Recall_Remark = "update status from Sent to New.";

        /// <summary>
        /// status_ReasonType_Recall
        /// </summary>
        public const string status_ReasonType_Recall = "UnApprove";

        /// <summary>
        /// status_ReasonType_unApproved
        /// </summary>
        public const string status_ReasonType_unApproved = "UnApprove";

        /// <summary>
        /// 顯示字 - Received
        /// </summary>
        public const string Label_Received = "Received";

        /// <summary>
        /// 顯示字 - Return
        /// </summary>
        public const string Label_Return = "Return";

        /// <summary>
        /// Sent後發送Email給SMR的設定檔ID (船務用)
        /// </summary>
        public const string MailToSentForShipping = "087";

        /// <summary>
        /// Sent後發送Email給SMR的設定檔ID
        /// </summary>
        public const string MailTo_Sent = "094";

        /// <summary>
        /// SMR Confirm後發送Email給Leader的設定檔ID
        /// </summary>
        public const string MailTo_SMRConfirm = "095";

        /// <summary>
        /// SMR Confirm後發送Email給Finance的設定檔ID
        /// </summary>
        public const string MailTo_SMRConfirmToFinance = "096";

        /// <summary>
        /// Leader Confirm後發送Email給Finance的設定檔ID
        /// </summary>
        public const string MailTo_LeaderConfirm = "097";

        /// <summary>
        /// GetAccTypeName
        /// </summary>
        /// <param name="apID">apID</param>
        /// <returns>string</returns>
        public static string GetAccTypeName(string apID)
        {
            switch (apID.ToUpper().SafeLefts(2))
            {
                case DocNo.TTBefore: return "Account Payable(T/T Before)";
                case DocNo.POAP: return "Account Payable(Purchase)";
                case DocNo.AP: return "Account Payable(Material)";
                case DocNo.Expense: return "Account Payable(Expense)";
                case DocNo.PartAP: return "Account Payable(Mms)";
                case DocNo.PartTT: return "Account Payable(Mms Payment Before)";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// sql update invoice to ExportDetail's payInv
        /// </summary>
        public const string sql_Update_ExportDetail_PayInv =
@"
update e
    set e.FormXPayINV = a.invoiceNo
from dbo.Export_Detail as e
inner join {0} as a on {1}

";

        /// <inheritdoc />
        public static string GetSqlCmd_AP_UpdateExportDetail_PayInv(string innerJoinTable, string joinOn)
        {
            var creteria = Check.Empty(joinOn)
                    ? " a.ID = @ID and a.Export_DetailUkey = e.Ukey "
                    : joinOn;
            return string.Format(
                sql_Update_ExportDetail_PayInv,
                innerJoinTable,
                creteria);
        }

        /// <inheritdoc />
        /// 若表身InvoiceNo,InvoiceDate與資料不符,更新回相關欄位
        public static string GetSqlCmd_AP_UpdateArtworkTest_InvData(string apID)
        {
            return $@"update atInvoice set atInvoice.InvoiceNo = apd.InvoiceNo, atInvoice.InvoiceDate = apd.InvoiceDate
                from dbo.ArtworkTest_Invoice as atInvoice
                inner join AP_Detail as apd on apd.POID = atInvoice.ID and apd.Seq2 = atInvoice.Seq
                Where apd.ID = @ID";
        }

        /// <summary>
        /// sql CheckAPQty_byWKQty
        /// </summary>
        public const string sql_Check_APQty_byWkQty =
@"
select * into #fails from(
	select wk2.ID , wk2.PoID, wk2.Seq1, wk2.Seq2,wk2.Qty as wk_Qty, IsNull(AlreadyAp.ApQty,0) as had_ApQty , t.Qty 
	from(
		select
			t.Export_DetailUkey ,Sum(t.Qty) as Qty
		from {0} as t
		Group by t.Export_DetailUkey 
	) as t
	left join dbo.Export_Detail as wk2 on wk2.Ukey = t.Export_DetailUkey
	outer apply (
		select sum(Qty) as ApQty
		from dbo.AP_Detail as apd, dbo.AP as ap
		where Export_DetailUkey = wk2.Ukey and ap.ID = apd.ID and ap.Status != 'J'
	) as AlreadyAp
	where wk2.Qty - IsNull(AlreadyAp.ApQty,0) - t.Qty < 0
) as errors

if exists( select 1 from #fails )
begin 
	select * from #fails
	return 	
end
";

        /// <inheritdoc />
        public static string GetSqlCmd_Check_APQty_byWKQty(string sourceTable)
        {
            return string.Format(sql_Check_APQty_byWkQty, sourceTable);
        }

        /// <summary>
        /// sql Check AP Qty by PO Qty/ShipQty (TTBefore/Purchase請款數不能超過採購數或是已出貨數)
        /// </summary>
        public const string sql_Check_ApQty_byPOQty_forAddItem =
@"
select * into #fails from(
    select s.POID,s.Seq1,s.Seq2,po3.Qty as PO_Qty,po3.ShipQty,s.Qty ,ap.ApQty as Had_AP_Qty
    from (
	    select s.POID,s.Seq1,s.Seq2,Sum(s.Qty) as Qty
	    from {0} as s
	    group by s.POID,s.Seq1,s.Seq2 
    ) as s
    outer apply dbo.GetApQty_AllStatus(s.POID,s.Seq1,s.Seq2,'') as ap
    left join dbo.PO_Supp_Detail as po3 on po3.ID = s.poid and po3.Seq1= s.Seq1 and po3.Seq2 = s.Seq2
    where isNull(iif(po3.Qty > po3.ShipQty, po3.Qty, po3.ShipQty),0) - s.Qty - isNull(ap.ApQty,0) < 0
) as errors

if exists( select 1 from #fails )
begin 
	select * from #fails
	return 	
end

";

        /// <inheritdoc />
        public const string sql_Check_ApQty_byPOQty_forReviseItem =
@"
select * into #fails2 from(
    select s.POID,s.Seq1,s.Seq2,po3.Qty as PO_Qty,po3.ShipQty,s.Qty ,hadTT.ApQty as Had_TT_Qty
    from (
	    select s.POID,s.Seq1,s.Seq2,Sum(s.Qty) as Qty
	    from {0} as s
	    group by s.POID,s.Seq1,s.Seq2 
    ) as s
    outer apply dbo.GetApQty_AllStatus(s.POID,s.Seq1,s.Seq2,'{1}') as hadTT
    inner join dbo.PO_Supp_Detail as po3 on po3.ID = s.poid and po3.Seq1= s.Seq1 and po3.Seq2 = s.Seq2 --2020/07/02 [IST20201103] inner join 排除空白POID項與非PO的單
    where isNull(iif(po3.Qty > po3.ShipQty, po3.Qty, po3.ShipQty),0) - s.Qty - isNull(hadTT.ApQty,0) < 0
) as errors

if exists( select 1 from #fails2 )
begin 
	select * from #fails2
	return 	
end
";

        /// <inheritdoc />
        public static string GetSqlCmd_Check_ApQty_byPOQty(string sourceTable, bool isRevise, string apID)
        {
            return isRevise
                ? string.Format(sql_Check_ApQty_byPOQty_forReviseItem, sourceTable, apID)
                : string.Format(sql_Check_ApQty_byPOQty_forAddItem, sourceTable);
        }

        public const string sql_Check_ApQty_byMmsPOQty_forAddItem =
@"
select * into #fails from(
    select s.POID,s.Seq1,s.Seq2,po3.Qty as PO_Qty,po3.ShipQty,s.Qty ,ap.ApQty as Had_AP_Qty
    from (
	    select s.POID,s.Seq1,s.Seq2,Sum(s.Qty) as Qty
	    from {0} as s
	    group by s.POID,s.Seq1,s.Seq2 
    ) as s
    outer apply dbo.GetApQty_AllStatus(s.POID,s.Seq1,s.Seq2,'') as ap
    left join dbo.MmsPO_Detail as po3 on po3.ID = s.poid and po3.Seq1= s.Seq1 and po3.Seq2 = s.Seq2
    where isNull(iif(po3.Qty > po3.ShipQty, po3.Qty, po3.ShipQty),0) - s.Qty - isNull(ap.ApQty,0) < 0
) as errors

if exists( select 1 from #fails )
begin 
	select * from #fails
	return 	
end

";

        /// <inheritdoc />
        public const string sql_Check_ApQty_byMmsPOQty_forReviseItem =
@"
select * into #fails2 from(
    select s.POID,s.Seq1,s.Seq2,po3.Qty as PO_Qty,po3.ShipQty,s.Qty ,hadTT.ApQty as Had_TT_Qty
    from (
	    select s.POID,s.Seq1,s.Seq2,Sum(s.Qty) as Qty
	    from {0} as s
	    group by s.POID,s.Seq1,s.Seq2 
    ) as s
    outer apply dbo.GetApQty_AllStatus(s.POID,s.Seq1,s.Seq2,'{1}') as hadTT
    left join dbo.MmsPO_Detail as po3 on po3.ID = s.poid and po3.Seq1= s.Seq1 and po3.Seq2 = s.Seq2 and po3.Junk=0
    where isNull(iif(po3.Qty > po3.ShipQty, po3.Qty, po3.ShipQty),0) - s.Qty - isNull(hadTT.ApQty,0) < 0
) as errors

if exists( select 1 from #fails2 )
begin 
	select * from #fails2
	return 	
end
";

        /// <inheritdoc />
        public static string GetSqlCmd_Check_ApQty_byMmsPOQty(string sourceTable, bool isRevise, string apID)
        {
            return isRevise
                ? string.Format(sql_Check_ApQty_byMmsPOQty_forReviseItem, sourceTable, apID)
                : string.Format(sql_Check_ApQty_byMmsPOQty_forAddItem, sourceTable);
        }

        /// <summary>
        /// sql Check mapping of ReasonID , SouceID is correct
        /// </summary>
        public const string sql_Check_AP_NotShipped =
@"
select * into #NotShip from (
	select POID+'-'+Seq1+'-'+Seq2 as NotShippedItem
	from {0} as s
	where isNull(s.reasonid,'') = ''
		and -- 空白採購單號或沒有出貨記錄
		( isNull(s.POID,'') = '' or (
			not exists(select 1 from dbo.Export_Detail as e where e.PoID = s.POID and e.Seq1 = s.Seq1 and e.Seq2 = s.Seq2)
			and not exists(select 1 from dbo.PO_Receiving as e where e.ID = s.POID and e.Seq1 = s.Seq1 and e.Seq2 = s.Seq2)
		))
) as s
if exists(select 1 from #NotShip)
begin
	select * from #NotShip
	return 
end

";

        /// <summary>
        /// 空白採購單號或沒有出貨記錄的採購自行請款AP項目需要填原因
        /// </summary>
        /// <inheritdoc />
        public static string GetSqlCmd_Check_AP_NotShipped(string sourceTable)
        {
            return string.Format(sql_Check_AP_NotShipped, sourceTable);
        }

        /// <summary>
        /// sql Check mapping of ReasonID , SouceID is correct
        /// </summary>
        public const string sql_Check_AP_Reason_Source =
@"
select 
	t.id,t.NameCH,t.SourceID,t.FailReason into #failSource
from(
	select r.id,r.NameCH,SourceID,r.FailReason
	,iif( r.ValidDB = 1 and ( exists(select 1 from dbo.Debit as db where db.ID = s.SourceID and db.Status != 'J')),0,1) as notPassDB
	,iif( r.ValidICR = 1 and ( exists(select 1 from dbo.ICR as i where i.ID = s.SourceID and i.Status != 'J')),0,1) as notPassICR
	,iif( r.ValidSD = 1 and ( exists(select 1 from dbo.SuppDebit as sd where sd.ID = s.SourceID and sd.Status != 'J')),0,1) as notPassSD
	,iif( r.ValidWK = 1 and ( exists(select 1 from dbo.Export as wk where wk.ID = s.SourceID and wk.Junk != 1)),0,1) as notPassWK
	from {0} as s
	left join dbo.APReason as r on r.ID = s.ReasonID 
	where s.ReasonID > '' and s.SourceID > ''
		and ( r.ValidDB = 1 or r.ValidICR = 1 or r.ValidSD = 1 or r.ValidWK = 1 )
) as t where t.notPassDB = 1 and t.notPassICR = 1 and t.notPassSD = 1 and t.notPassWK = 1

if exists( select 1 from #failSource )
begin 
	select * from #failSource
	return 	
end
";

        /// <inheritdoc />
        public static string GetSqlCmd_Check_AP_Reason_Source(string sourceTable)
        {
            return string.Format(sql_Check_AP_Reason_Source, sourceTable);
        }

        /// <summary>
        /// sql Check AP Detail will dup in ExportID + PoID + Seq1 + Seq2 + InvoiceNo
        /// </summary>
        public const string sql_Check_ApDetail_Duplicate =
@"
select t.* into #failSource2
from(
	select s.ExportID+' / '+s.PoID+'-'+s.Seq1+'-'+s.Seq2+' / '+s.InvoiceNo as AP_Detail_Dup
	from {0} as s
	where exists(select 1 from dbo.Ap_Detail where ID= '{1}' and Export_DetailUkey = s.Export_DetailUkey and invoiceNo = s.invoiceNo)
) as t 

if exists( select 1 from #failSource2 )
begin 
	select * from #failSource2
	return 	
end
";

        /// <inheritdoc />
        public static string GetSqlCmd_Check_ApDetail_Duplicate(string sourceTable, string apID)
        {
            return string.Format(sql_Check_ApDetail_Duplicate, sourceTable, apID);
        }

        /// <summary>
        /// sql Check TTBefore Detail will dup in PoID + Seq1+ Seq2
        /// </summary>
        public const string sql_Check_TTBeforeDetail_Duplicate =
@"
select t.* into #failSource3
from(
	select s.PoID+'-'+s.Seq1+'-'+s.Seq2 as TTBefore_Detail_Dup ,td.ID 
	from {0} as s
    left join dbo.TTBefore_Detail as td on td.PoID = s.poid and td.Seq1 = s.Seq1 and td.Seq2 = s.Seq2
	where exists(select 1 from dbo.TTBefore_Detail where POID = s.POID and Seq1 = s.Seq1 and Seq2 = s.Seq2)
) as t 

if exists( select 1 from #failSource3 )
begin 
	select * from #failSource3
	return 	
end
";

        /// <inheritdoc />
        public static string GetSqlCmd_Check_TTBeforeDetail_Duplicate(string sourceTable)
        {
            return string.Format(sql_Check_TTBeforeDetail_Duplicate, sourceTable);
        }

        /// <inheritdoc />
        public static void Warning_AP_UpdateFailed(Sci.Win.Forms.Base form, DataTable table)
        {
            MsgGridForm alert;
            if (table.Columns.Contains("failSource"))
            {
                alert = new MsgGridForm(table, caption: "Error");
            }
            else if (table.Columns.Contains("failReason") && table.Rows[0]["failReason"].StrStartsWith("DB/ICR/SD#"))
            {
                alert = new MsgGridForm(table, string.Empty);
            }
            else if (table.Columns.Contains("NotShippedItem"))
            {
                alert = new MsgGridForm(table, " [Reason] can't be empty for these not shipped item .", "Error");
                alert.grid1.Columns[0].Width = 260;
                alert.grid1.Columns[0].HeaderText = "PO item not shipped";
            }
            else if (table.Columns.Contains("TTBefore_Detail_Dup"))
            {
                alert = new MsgGridForm(table, " These PO item are already exists in Account Payable .", "Error");
                alert.grid1.Columns[0].Width = 160;
                alert.grid1.Columns[0].HeaderText = "PO item ";
                alert.grid1.Columns[1].Width = 130;
                alert.grid1.Columns[1].HeaderText = "AP #";
            }
            else if (table.Columns.Contains("AP_Detail_Dup"))
            {
                alert = new MsgGridForm(table, " These PO item are already exists in Account Payable .", "Error");
                alert.grid1.Columns[0].Width = 120;
                alert.grid1.Columns[0].HeaderText = "Duplicate items ";
            }
            else
            {
                alert = new MsgGridForm(table, " These items' [AP Qty] will be greater than [Po Qty/Ship Qty] after updated.", "Error");
            }

            UIClassPrg.SetGrid_HeaderBorderStyle(alert.grid1);
            alert.Show(form);
        }

        /// <summary>
        /// Check Surcharge before update from PO / MmsPO
        /// </summary>
        public const string sql_Check_Surcharge_BeforePOUpdate =
@"
if exists(
    select 1
    from {0} as ps
    inner join dbo.Surcharge_Detail as su2 on su2.{1} = ps.Ukey
    inner join dbo.Surcharge as su1 on su1.ID = su2.ID
    where su1.Status <> 'New' and su2.Amount <> ps.Amount
)
begin
    select su1.ID, {2}, ps.Description
	from {0} as ps
	inner join dbo.Surcharge_Detail as su2 on su2.{1} = ps.Ukey
	inner join dbo.Surcharge as su1 on su1.ID = su2.ID
	where su1.Status <> 'New' and su2.Amount <> ps.Amount
    return
end
";

        /// <summary>
        /// to check account payable status , only be acceptable for Account payable not found or status is New
        /// </summary>
        /// <param name="tmpTableName"> ex:  "#tmpTable" </param>
        /// <param name="sourceTable"> "PO_Surcharge" or "MmsPO_Surcharge"</param>
        /// <inheritdoc />
        public static string GetSqlCmd_Check_SurchargeStatus_BeforePOUpdate(string tmpTableName, string sourceTable)
        {
            return string.Format(
                sql_Check_Surcharge_BeforePOUpdate,
                tmpTableName,
                sourceTable.EqualString("PO_Surcharge") ? "PO_Surcharge_Ukey" : "MmsPO_Surcharge_Ukey",
                sourceTable.EqualString("PO_Surcharge") ? "ps.Seq1, '' as Seq2" : "ps.Seq1,ps.Seq2");
        }

        /// <inheritdoc />
        public static void Warning_Surcharge_UpdateFailed(Sci.Win.Forms.Base form, DataTable errors)
        {
            MsgGridForm alert = new MsgGridForm(errors, "can't Modify these item since already existed in Account Payable(Surcharge), and Status is not New.", "Error");
            alert.grid1.Columns[0].Width = 130;
            alert.grid1.Columns[0].HeaderText = "Surcharge #";
            alert.grid1.Columns[1].Width = 50;
            alert.grid1.Columns[1].HeaderText = "Seq#1";
            alert.grid1.Columns[2].Width = 50;
            alert.grid1.Columns[2].HeaderText = "Seq#2";
            alert.grid1.Columns[3].Width = 260;
            alert.grid1.Columns[3].HeaderText = "Surcharge Description";
            UIClassPrg.SetGrid_HeaderBorderStyle(alert.grid1);
            alert.Show(form);
        }

        /// <summary>
        /// update Surcharge Amount from PO / MmsPO
        /// </summary>
        // 2017/06/09 Modify by shan 調整為將Tax的加總為合計同Invoice的金額後乘以稅額四依幣別精準度四捨五入
        // 2017.11.01 modify by Edward 調整By SurchangeID做Group by後Update
        public const string sql_Update_Surcharge_Amount_FromPO =
@"
update t
	set t.Amount = ps.Amount, 
        t.EditDate = GetDate(),
        t.EditName = '{2}'
from dbo.Surcharge_Detail as t
inner join {0} as ps on t.{1} = ps.Ukey

update dbo.Surcharge
	set  Amount = result.detailTotalAmount
	, TAX = result.detailTotalTaxAmount
	, TaxRate = result.companytax , EditDate = GetDate() , EditName = '{2}'
	-- Payby = 1 要更新台幣的NTAmt
	, NTAmt = result.NTAmt
from (
        select sum(round(sumDetail.Amount,cur.Exact)) as detailTotalAmount
            ,  sum(round(sumDetail.Amount * com.TaxRate / 100, cur.Exact)) as detailTotalTaxAmount
            ,  com.TaxRate as companytax
	        -- Payby = 1 要更新台幣的NTAmt
	        ,sum(iif( a.Payby = 1
				        ,Round( ( isNull(sumDetail.Amount,0)+isNull(round(sumDetail.Amount * com.TaxRate / 100, cur.Exact),0) )
				        	 * a.Exchange , curPay.Exact)
				        , a.NTAmt 
				)) as NTAmt
            , a.ID surchargeID
        from dbo.Surcharge as a
        inner join(
    	    select su1.ID
    	    from {0} as ps , dbo.Surcharge as su1 ,dbo.Surcharge_Detail as su2
    	    where su2.{1} = ps.Ukey and su1.ID = su2.ID and su1.Status = 'New' 
    	    group by su1.ID
        ) as s on s.ID = a.ID
        outer apply (
	        select sum(Amount) as Amount
	        from dbo.Surcharge_Detail 
	        where Surcharge_Detail.ID = a.ID
	        group by InvoiceNo
        ) as detail
        outer apply(select isnull(detail.amount,0) as Amount) as sumDetail
        cross apply(
	        select top 1 c.TaxRate
	        from dbo.Company_Tax as c
	        where c.ID = a.CompanyID and a.CDate>= c.StartDate and (c.EndDate >= a.CDate or c.EndDate is null )
        ) as com
        left join dbo.Currency as cur on cur.ID = a.CurrencyID 
        left join dbo.Currency as curPay on curPay.ID = a.P_Currency
		group by com.TaxRate, a.ID
) as result
where id=result.surchargeID
";

        /// <summary>
        /// update PO_Surcharg.Amount to Surcharge_Detail.Amount while Account Payable status is New
        /// </summary>
        /// <param name="tmpTableName"> ex:  "#tmpTable" </param>
        /// <param name="sourceTable"> "PO_Surcharge" or "MmsPO_Surcharge"</param>
        /// <inheritdoc />
        public static string GetSqlCmd_Check_Surcharge_Amount_FromPO(string tmpTableName, string sourceTable)
        {
            return string.Format(
                sql_Update_Surcharge_Amount_FromPO,
                tmpTableName,
                sourceTable.EqualString("PO_Surcharge") ? "PO_Surcharge_Ukey" : "MmsPO_Surcharge_Ukey",
                Env.User.UserID);
        }

        /// <summary>
        /// sql update Ap's Amount Tax TaxRate NTAmt
        /// </summary>
        // 2017/06/09 Modify by shan 調整為將Tax的加總為合計同Invoice的金額後乘以稅額四依幣別精準度四捨五入
        // 2020/07/21 [IST20201290] 調整Tax先依InvoiceNo Group by四捨五入加總後，再加總四捨五入一次，避免跟SubTotal中Excel明細加總有落差
        public const string sql_Update_Ap_Amount = @"
If object_id('tempdb..#tmp') is not null
Drop table #tmp

Select Amount = Round(Sum(result.detailTotalAmount), result.Exact)
	, TAX = Round(Sum(result.detailTotalTaxAmount), result.Exact)
	, TaxRate = result.companytax 
	-- Payby = 1 要更新台幣的NTAmt
	, NTAmt = result.NTAmt
Into #tmp
From dbo.{0}
Outer apply (
	Select detailTotalAmount = Round(Sum(round(sumDetail.Amount,cur.Exact)) - isnull(GetDiscountAmt.discountAmt,0), cur.Exact) 
        , detailTotalTaxAmount = Round((Sum(round(sumDetail.Amount,cur.Exact)) - isnull(GetDiscountAmt.discountAmt,0)) * getTaxRate.TaxRate / 100, cur.Exact)
        , companytax = min(getTaxRate.TaxRate)
	    , NTAmt = iif(a.Payby = 1 -- Payby = 1 要更新台幣的NTAmt
                , Round(((Sum(Round(sumDetail.Amount,cur.Exact)) - GetDiscountAmt.discountAmt) 
                            + isNull(Round((sum(round(sumDetail.Amount, cur.Exact)) - GetDiscountAmt.discountAmt) * getTaxRate.TaxRate / 100, cur.Exact), 0)) * a.Exchange , curPay.Exact)
				, a.NTAmt 
			)
        , cur.Exact
    From dbo.{0} as a
    join supp on a.SUPPID = supp.ID
    Outer apply(
        Select Amount = isNull(sum(ap2.Amount),0)
            , ap2.InvoiceNo
        From dbo.{0}_Detail as ap2
        Where ap2.ID = @ID 
        Group by InvoiceNo
    ) as detail
    Cross apply (select isnull(detail.amount, 0) as Amount) as sumDetail
    Cross apply (
        Select top 1 c.TaxRate
        From dbo.Company_Tax as c
        Where c.ID = a.CompanyID and a.CDate >= c.StartDate and (c.EndDate >= a.CDate or c.EndDate is null )
    ) as com
    outer apply(select TaxRate = iif(supp.NeedTax = 1, com.TaxRate, 0)) as getTaxRate    
	Outer apply (
        Select discountAmt = sum(DiscountAmount) 
        From AP_Discount 
        Where APID = @ID And InvoiceNo = detail.InvoiceNo
    ) GetDiscountAmt
    Left join dbo.Currency as cur on cur.ID = a.CurrencyID 
    Left join dbo.Currency as curPay on curPay.ID = a.P_Currency
    Where a.ID = @ID 
	Group by a.ID
        , GetDiscountAmt.discountAmt
        , getTaxRate.TaxRate
        , a.Payby
        , curPay.Exact
        , cur.Exact
        , a.Exchange
        , a.NTAmt 
        , detail.InvoiceNo
        , cur.Exact
) as result
Where id = @ID 
Group by result.companytax, result.companytax, result.Exact, result.NTAmt

Update dbo.{0}
Set Amount = t.Amount
    , Tax = t.Tax
    , TaxRate = t.TaxRate 
    , EditDate = GetDate()
    , EditName = @userID
From #tmp t
Where ID = @ID
";

        /// <summary>
        /// sql update Expense's Amount Tax TaxRate NTAmt
        /// </summary>
        public const string sql_Update_Expense_Amount =
@"
update dbo.{0}
	set  Amount = result.detailTotalAmount
	, TAX = result.detailTotalTaxAmount
	, TaxRate = result.companytax , EditDate = GetDate() , EditName = @userID
	-- Payby = 1 要更新台幣的NTAmt
	, NTAmt = result.NTAmt
from (
        select sum(round(sumDetail.Amount, cur.Exact)) as detailTotalAmount
            ,  sum(round(sumDetail.Amount * getTaxRate.TaxRate / 100, cur.Exact)) as detailTotalTaxAmount
            ,  min(getTaxRate.TaxRate)  as companytax
	        -- Payby = 1 要更新台幣的NTAmt
	        ,sum(iif( a.Payby = 1
				        ,Round( ( isNull(sumDetail.Amount,0)+isNull(round(sumDetail.Amount * getTaxRate.TaxRate / 100, cur.Exact),0) )
				        	 * a.Exchange , curPay.Exact)
				        , a.NTAmt 
				)) as NTAmt
        from dbo.{0} as a
        join supp on a.SUPPID = supp.ID
        outer apply(
        	select isNull(sum(ap2.Amount),0) as Amount
        	from dbo.{0}_Detail as ap2
        	where ap2.ID = @ID 
            group by InvoiceNo
        ) as detail
        outer apply(select isnull(detail.amount,0) as Amount) as sumDetail
        cross apply(
        	select top 1 
                    case when (a.type = '01' or a.Type  ='08' or a.Type ='10') and a.CurrencyID = 'CNY' Then 6
						 when a.Type in ('06','07') Then 0 --Type = 06 or 07 為免稅項目
						 else c.TaxRate end as TaxRate
        	from dbo.Company_Tax as c
        	where c.ID = a.CompanyID and a.CDate>= c.StartDate and (c.EndDate >= a.CDate or c.EndDate is null )
        ) as com
        outer apply(select TaxRate = iif(supp.NeedTax = 1, com.TaxRate, 0)) as getTaxRate    
        left join dbo.Currency as cur on cur.ID = a.CurrencyID 
        left join dbo.Currency as curPay on curPay.ID = a.P_Currency
        where a.ID = @ID 
        group by a.type, a.CurrencyID
       -- having min(a.amount) <> sum(sumDetail.amount) or min(a.taxrate) <> min(com.taxrate)
    ) as result
where id=@ID 
";

        /// <summary>
        /// sql update DomesticAP's Amount Tax TaxRate NTAmt
        /// </summary>
        public const string sql_Update_DomesticAP_Amount =
@"
Update dbo.DomesticAP
	Set Amount = result.detailTotalAmount
	, TAX = result.detailTotalTaxAmount
	, TaxRate = result.companytax 
    , EditDate = GetDate() 
    , EditName = @userID
from (
        Select Sum(Round(sumDetail.Amount, cur.Exact)) as detailTotalAmount
             , Sum(Round(sumDetail.Amount * getTaxRate.TaxRate / 100, cur.Exact)) as detailTotalTaxAmount
             , min(getTaxRate.TaxRate)  as companytax
        From dbo.DomesticAP as a
        join supp on a.SUPPID = supp.ID
        Outer apply(
        	select isNull(sum(ap2.Amount),0) as Amount
        	from dbo.DomesticAP_Detail as ap2
        	where ap2.ID = @ID 
            group by InvoiceNo
        ) as detail
        Outer apply(Select Isnull(detail.amount,0) as Amount) as sumDetail
        Cross apply(
        	Select top 1 c.TaxRate
        	From dbo.Company_Tax as c
        	Where c.ID = a.CompanyID and a.CDate >= c.StartDate and (c.EndDate >= a.CDate or c.EndDate is null )
        ) as com
        outer apply(select TaxRate = iif(supp.NeedTax = 1, com.TaxRate, 0)) as getTaxRate    
        Left Join dbo.Currency as cur on cur.ID = a.CurrencyID 
        where a.ID = @ID
        group by a.CurrencyID
    ) as result
Where id = @ID 
";

        /// <inheritdoc />
        // 2017/06/09 Modify by shan 調整為將Tax的加總為合計同Invoice的金額後乘以稅額四依幣別精準度四捨五入
        public const string sql_Update_TTBeforeMms_Amount =
@"
update dbo.TTBeforeMms
	set  Amount = detailTotalAmount 
		,TAX = detailTotalTaxAmount
		,TaxRate = companytax, EditDate = GetDate(), EditName = @userID	
from (
        select sum(round(sumDetail.Amount,cur.Exact)) as detailTotalAmount
	        ,  sum(round(sumDetail.Amount * getTaxRate.TaxRate / 100, cur.Exact)) as detailTotalTaxAmount
	        ,  min(getTaxRate.TaxRate) as companytax
	    from dbo.TTBeforeMms as a
        join supp on a.SUPPID = supp.ID
        outer apply(
                    select isNull(sum(ap2.InvoiceAmount),0) as Amount
                    from dbo.TTBeforeMms_Detail as ap2
                    where ap2.ID = @ID
                    group by InvoiceNo
        ) as detail
        outer apply(select isnull(detail.amount,0) as Amount) as sumDetail
        cross apply(
                    select top 1 c.TaxRate
                    from dbo.Company_Tax as c
                    where c.ID = a.CompanyID and a.CDate>= c.StartDate and (c.EndDate >= a.CDate or c.EndDate is null )
        ) as com
        outer apply(select TaxRate = iif(supp.NeedTax = 1, com.TaxRate, 0)) as getTaxRate        
        left join dbo.Currency as cur on cur.ID = a.CurrencyID 
        where a.ID = @ID
        --having min(a.amount) <> sum(sumDetail.amount) or min(a.taxrate) <> min(com.taxrate)
    ) as result
where id = @ID
";

        /// <inheritdoc />
        // 2019/7/4 [IST20190569] 國內快遞依重量分攤金額
        public const string sql_Update_DomesticAP_Share = @"
-- Add DomesticAP_Share
-- By明細DE#的Amount做金額分攤
select ap.ID
, APDetailUkey = ap2.Ukey
, DEDetailUkey = ded.Ukey
, PayDepartment = ded.PayDepartment
, ap2.Amount
, FinanceDepartmentID = dep.FinanceDepartmentID
, AccountID = iif(Isnull(f.ID, '') <> '', f.Acc1269, dep.ExpressAccountID)
, ded.Weight
, getWeight.sumWeight
, shareAmount = Floor(ap2.Amount * (ded.Weight / getWeight.sumWeight))
into #tmp
From DomesticAP_Detail ap2 
Left join DomesticAP ap on ap.ID = ap2.ID
Left join DomesticExpress_Detail ded on ap2.DomesticExpressID = ded.ID
Left join HRDepartment dep on dep.DepartmentID = ded.PayDepartment
Left join Factory f on f.ID = ded.PayDepartment
outer apply (
	Select sumWeight = sum(Weight)
	From DomesticAP_Detail t1
	Left join DomesticExpress_Detail t2 On t1.DomesticExpressID = t2.ID
	Where t1.Ukey = ap2.Ukey
) getWeight
Where ap2.ID = @ID and (@apDetailUkey = '' or ap2.Ukey = @apDetailUkey)

Insert into DomesticAP_Share (DomesticAP_Detail_Ukey, DomesticExpress_Detail_Ukey, PayDepartment, Amount, FinanceDepartmentID, AccountID, AddName, AddDate)
select tmp.APDetailUkey
, tmp.DEDetailUkey
, tmp.PayDepartment
, finalAmount = iif (tmp.DEDetailUkey = GetMax.DEDetailUkey, shareAmount + Amount - GetSumShareAmount.Value, shareAmount) --若有Amount分攤下來有尾差,將剩餘數加至明細最重的那筆金額
, isnull(tmp.FinanceDepartmentID,'')
, isnull(tmp.AccountID,'')
, AddName = @userID
, AddDate = @editDate
From #tmp tmp
outer apply (
	select Value = sum(ShareAmount)
	From #tmp t1
	Where t1.APDetailUkey = tmp.APDetailUkey
) GetSumShareAmount

--取Weight最重的Ukey
outer apply (
	select top 1 DEDetailUkey
	From #tmp t2
	Where t2.APDetailUkey = tmp.APDetailUkey
	Order by t2.Weight desc
) GetMax";

        /// <inheritdoc />
        // 重新計算AP.DiffTax
        public const string sql_Update_AP_DiffTax = @"
update AP set DiffTax = isnull(AP_Detail.SumDiffTax, 0)
from (
    select ID, SumDiffTax = Sum(a.DiffTax)
    from (
        select ap2.ID
        , InvoiceNo
        , DiffTax = Round((Sum(AP_Detail.Amount) + Sum(DiffAmt)) * ap2.Taxrate / 100, c.exact)
        from AP ap2
		Left join AP_Detail on ap2.ID = AP_Detail.ID
        Left join Currency c on c.ID = ap2.CurrencyID
        Where ap2.ID = @ID
        group by ap2.ID, InvoiceNo, ap2.Taxrate, c.Exact
    ) a
    group by a.id
) AP_Detail
where AP.ID = AP_Detail.ID and AP.ID = @ID";

        /// <inheritdoc />
        public static bool update_AP_AmountTax(string apID, string sourceTable, SqlConnection conn = null)
        {
            if (conn == null)
            {
                if (!SQL.GetConnection(out conn))
                {
                    return false;
                }
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));

            // TTBeforeMms, Expense, DomesticAP 獨立算法
            // 其他 AP, TTBefore, Expense 等table的算法都一樣
            string cmd_Update = string.Empty;
            switch (sourceTable)
            {
                case "TTBeforeMms":
                    cmd_Update = sql_Update_TTBeforeMms_Amount;
                    break;
                case "Expense":
                    cmd_Update = string.Format(sql_Update_Expense_Amount, sourceTable);
                    break;
                case "DomesticAP":
                    cmd_Update = sql_Update_DomesticAP_Amount;
                    break;
                default:
                    cmd_Update = string.Format(sql_Update_Ap_Amount, sourceTable);
                    break;
            }

            return SQL.Execute(conn, cmd_Update, pars);
        }

        /// <inheritdoc />
        public static bool update_AP_Company(string apID, string sourceTable,DataTable updateDt, SqlConnection conn = null)
        {
            string cmd_Update = $"update {sourceTable} set OrderCompany = @OrderCompany, CompanyID = @CompanyID where ID = @ID ";
            if (conn == null)
            {
                if (!SQL.GetConnection(out conn))
                {
                    return false;
                }
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@OrderCompany", updateDt.Rows[0]["OrderCompany"].ToString()));
            pars.Add(new SqlParameter("@CompanyID", updateDt.Rows[0]["CompanyID"].ToString()));
            pars.Add(new SqlParameter("@ID", apID));

            return SQL.Execute(conn, cmd_Update, pars);
        }

        /// <inheritdoc />
        public static bool update_AP_TaxRate(string apID, string sourceTable, SqlConnection conn = null)
        {
            if (conn == null)
            {
                if (!SQL.GetConnection(out conn))
                {
                    return false;
                }
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));

            string cmd_Update = $@"
update dbo.{sourceTable} set 
TAX = isnull(result.detailTotalTaxAmount,0)
, TaxRate = isnull(result.TaxRate,0)
from (
	select round((sum(round(sumDetail.Amount,cur.Exact)) - isnull(GetDiscountAmt.discountAmt,0)) * com.TaxRate / 100, cur.Exact) as detailTotalTaxAmount
            , com.TaxRate
	from dbo.{sourceTable} a
	outer apply (
		select top 1 iif((a.type = '01' and a.CurrencyID = 'CNY'),6, c.TaxRate) as TaxRate
		from dbo.Company_Tax as c
		outer apply (select max(InvoiceDate) as InvoiceDate from dbo.{sourceTable}_detail where id = @ID) maxDate
		where c.ID = a.CompanyID and maxDate.InvoiceDate>= c.StartDate and (c.EndDate >= maxDate.InvoiceDate or c.EndDate is null )
	) com
    outer apply(
        select isNull(sum(ap2.Amount),0) as Amount
        from dbo.{sourceTable}_Detail as ap2
        where ap2.ID = @ID
        group by InvoiceNo
    ) as detail
    outer apply(select isnull(detail.amount,0) as Amount) as sumDetail
    left join dbo.Currency as cur on cur.ID = a.CurrencyID 
	outer apply (Select discountAmt = sum(DiscountAmount) From AP_Discount Where APID = @ID) GetDiscountAmt
    where a.ID = @ID
    group by com.TaxRate, GetDiscountAmt.discountAmt,cur.Exact
) as result
where id=@ID";
            return SQL.Execute(conn, cmd_Update, pars);
        }

        /// <inheritdoc />
        public static bool update_AP_Remark(string apID, string remark, string sourceTable, SqlConnection conn = null)
        {
            string updateCmd = string.Format(
@" update dbo.{0}
    set Remark = @Remark ,editName = @userID , editDate = @editDate
    where ID = @ID
", sourceTable);
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));
            pars.Add(new SqlParameter("@editDate", DateTime.Now));
            pars.Add(new SqlParameter("@Remark", remark));

            TransactionScope scope = new TransactionScope();
            if (!SQL.Execute(string.Empty, updateCmd, pars))
            {
                scope.Dispose();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            return true;
        }

        #region -- Invoice #, Invoice Date重複的相關檢查

        /// <inheritdoc />
        public const string sql_Check_InvoiceDuplicate = @"exec dbo.CheckInvoiceDuplicate @ID,@sourceTable";

        /// <inheritdoc />
        public const string sql_CheckInvoiceDuplicate_TTBefore = @"select * from dbo.CheckInvoiceDuplicate_TTBefore(@ID)";

        /// <inheritdoc />
        public static string GetSqlCmd_Check_InvoiceDup(string sourceTable)
        {
            // 此procedure 執行後會產生一個 table出來
            return sql_Check_InvoiceDuplicate;
        }

        /// <inheritdoc />
        public static void Warning_InvoiceDuplicate(string apID, string sourceTable)
        {
            string sqlCmd = GetSqlCmd_Check_InvoiceDup(sourceTable);
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@sourceTable", sourceTable));

            DataTable data;
            if (!SQL.Select(SQL.queryConn, sqlCmd, out data, pars))
            {
                return;
            }

            // 2017/05/12 Modify by shan 狀況訊息分開，讓使用者方便查看
            var invNoDup = data.AsEnumerable().Where(row => Check.isTrue(row["dupInvNo"]));
            var invDateDup = data.AsEnumerable().Where(row => Check.isTrue(row["dupInvDate"]));
            if ((invNoDup.Count() != 0) || (invDateDup.Count() != 0))
            {
                StringBuilder sb = new StringBuilder();
                if (invNoDup.Count() != 0)
                {
                    sb.AppendLine().Append("Invno:");
                    foreach (DataRow row in invNoDup)
                    {
                        sb.AppendLine().Append(row["InvoiceNo"].ToString());
                    }

                    sb.AppendLine().Append("已出現在其他請款單中!!");
                }

                if (invDateDup.Count() != 0)
                {
                    // 不同的狀態訊息，中間隔一行，為了美觀
                    if (invNoDup.Count() != 0)
                    {
                        sb.AppendLine().Append(string.Empty);
                    }

                    sb.AppendLine().Append("Invno:");
                    foreach (DataRow row in invDateDup)
                    {
                        sb.AppendLine().Append(row["InvoiceNo"].ToString());
                    }

                    sb.AppendLine().Append("有不同的[Invoice Date]!!");
                }

                EditMemo memo = new EditMemo(sb.ToString(), "Information", false, null);
                memo.ShowDialog();
            }
        }

        #endregion

        #region -- Account Payable主畫面顯示相關

        /// <inheritdoc />
        public static DateTime Get_CDate_4_NewAP()
        {
            DateTime cDate = DateTime.Today;
            return cDate.Day > 5 ? cDate : cDate.GetFirstDayOfMonth().AddDays(-1);
        }

        /// <inheritdoc />
        public static DateTime Get_BaseDate_4_AP()
        {
            return DateTime.Today.Day >= 25
                    ? DateTime.Now.GetFirstDayOfMonth().AddMonths(1).AddDays(-1)
                    : DateTime.Now.GetFirstDayOfMonth().AddDays(-1)
                    ;
        }

        #endregion

        /// <summary>
        /// company 選飛雁 , 幣別非TWD 的話 exchange不能= 0
        /// </summary>
        /// <inheritdoc />
        public static bool Check_ApDetail_Need_Exchange(object companyID, object currencyID, object shipterm)
        {
            decimal company = SciConvert.GetDecimal(companyID);
            string shiptermValue = SciConvert.GetString(shipterm);

            // string country = MyUtility.GetValue.Lookup("Select CountryID from Supp where id = '" + supplierID + "'");
            return company == 1 && !Check.EqualString("TWD", currencyID.ToString()) && shiptermValue.IsOneOfThe("FOR", "EXW");
        }

        /// <inheritdoc />
        public static bool Check_ApDetail_Exchange(IEnumerable<DataRow> data, object companyID, object currencyID, object shipterm, string sourceTable = "")
        {
            if (!Check_ApDetail_Need_Exchange(companyID, currencyID, shipterm))
            {
                return true;
            }

            var exchangeErr = data.Where(row => Check.Empty(row["exchange"]) || decimal.Parse(row["exchange"].ToString()) == 0);
            if (exchangeErr.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Exchange> can not be empty !");
                if (sourceTable.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                {
                    // 費用請款沒有小項
                    foreach (DataRow row in exchangeErr)
                    {
                        sb.AppendLine().Append(row["ExportID"]);
                    }
                }
                else if (sourceTable.Equals("Surcharge", StringComparison.OrdinalIgnoreCase))
                {
                    Msg.WarningBox(sb.ToString());
                    return false;
                }
                else
                {
                    bool has_ExportId = exchangeErr.First().Table.Columns.Contains("Exportid");
                    foreach (DataRow row in exchangeErr)
                    {
                        if (row.Table.Columns["Seq1"] != null && row.Table.Columns["Seq2"] != null)
                        {
                            sb.AppendLine()
                                    .Append(has_ExportId ? row["Exportid"].ToString() + "-" : string.Empty)
                                    .Append(row["poid"])
                                    .Append("-").Append(row["Seq1"])
                                    .Append("-").Append(row["Seq2"])
                                    ;
                        }
                        else
                        {
                            sb.AppendLine()
                                .Append(has_ExportId ? row["Exportid"].ToString() + "-" : string.Empty)
                                .Append(row["poid"])
                                ;
                        }
                    }
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public static bool Check_ApDetail_Amt(IEnumerable<DataRow> data, string sourceTable = "")
        {
            var qtyErr = data.Where(row => MyUtility.Convert.GetDecimal(row["Qty"]) != MyUtility.Convert.GetDecimal(row["POQty"]));
            if (qtyErr.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("以下SP# 計算出來的Amount與採購單不符，請確認!");

                foreach (DataRow row in qtyErr)
                {
                    if (row.Table.Columns["Seq1"] != null && row.Table.Columns["Seq2"] != null)
                    {
                        sb.AppendLine()
                                .Append(row["poid"])
                                .Append("-").Append(row["Seq1"])
                                .Append("-").Append(row["Seq2"])
                                ;
                    }
                    else
                    {
                        sb.AppendLine()
                            .Append(row["poid"])
                            ;
                    }
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// invoice date 年度有誤
        /// </summary>
        /// <inheritdoc />
        public static bool Check_ApDetail_SourceID(IEnumerable<DataRow> data)
        {
            var errReason = data.Where(row => !APReasonPrg.ValidReason(row["reasonID"], row["SourceID"]));
            if (errReason.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("These relation# are incorrect for its reason ");
                foreach (DataRow row in errReason)
                {
                    APReason reason = APReasonPrg.GetReason(row["reasonID"]);
                    sb.AppendLine().Append(row["POID"])
                        .Append("-").Append(row["Seq1"])
                        .Append("-").Append(row["Seq2"])
                        .Append(",  Reason : ").Append(reason.NameCH)
                        .Append(",  Error : ").Append(reason.FailReason);
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// invoice date 年度有誤
        /// </summary>
        /// <inheritdoc />
        public static bool Check_ApDetail_InvoiceDate_Year(IEnumerable<DataRow> data, string invoiceDateColumn, string sourceTable)
        {
            var errYear = data.AsEnumerable().Where(row => !Check.Empty(row[invoiceDateColumn])
                    && (Convert.ToDateTime(row[invoiceDateColumn])).MonthInRangeOf(DateTime.Today, -120, 120));
            if (errYear.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Year of ")
                    .Append(invoiceDateColumn.StartsWith("inv", StringComparison.OrdinalIgnoreCase) ? "Invoic Date" : "PI Date")
                    .Append(" is incorrect ");
                if (sourceTable.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                {
                    // 費用請款沒有小項
                    foreach (DataRow row in errYear)
                    {
                        sb.AppendLine().Append(row["ExportID"])
                            .Append("  : ").Append(row[invoiceDateColumn].ToString());
                    }
                }
                else if (sourceTable.Equals("Surcharge", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (DataRow row in errYear)
                    {
                        sb.AppendLine().Append(row[invoiceDateColumn].ToString());
                    }
                }
                else
                {
                    foreach (DataRow row in errYear)
                    {
                        sb.AppendLine().Append(row["POID"])
                            .Append("-").Append(row["Seq1"])
                            .Append("-").Append(row["Seq2"])
                            .Append("  : ").Append(row[invoiceDateColumn].ToString());
                    }
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查 Surcharge detail的reasonID不能是空的
        /// </summary>
        /// <inheritdoc />
        public static bool Check_SurchargeDetail_Reason_EmptyError(IEnumerable<DataRow> data)
        {
            if (data.Any(row => Check.Empty(row["ReasonID"])))
            {
                var errEmptyReason = data.Where(row => Check.Empty(row["ReasonID"]));
                StringBuilder sb = new StringBuilder();
                sb.Append("ReasonID can't be empty ");

                bool hasSeq1 = errEmptyReason.Any(row => row.Table.Columns.Contains("Seq1"));
                if (hasSeq1)
                {
                    foreach (DataRow row in errEmptyReason)
                    {
                        sb.AppendLine().Append(row["POID"])
                            .Append("-").Append(row["Seq1"]);
                    }
                }
                else
                {
                    foreach (DataRow row in errEmptyReason)
                    {
                        sb.AppendLine().Append(row["POID"]);
                    }
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// invoice No /Date 空白的錯誤
        /// </summary>
        /// <inheritdoc />
        public static bool Check_ApDetail_Invoice_EmptyError(IEnumerable<DataRow> data, string sourceTable)
        {
            string errDesc = sourceTable.InList("AP", "Expense", "Surcharge", "DomesticAP")
                    ? "Invoice # / Invoice date"
                    : "PI # / PI date";
            var errEmptyInv = data.Where(row => Check.Empty(row["InvoiceNo"]) || Check.Empty(row["invoiceDate"]));
            if (errEmptyInv.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(errDesc).Append(" can't be empty ");
                if (sourceTable.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                {
                    // 費用請款沒有小項
                    foreach (DataRow row in errEmptyInv)
                    {
                        sb.AppendLine().Append(row["ExportID"]);
                    }
                }
                else if (sourceTable.Equals("Surcharge", StringComparison.OrdinalIgnoreCase))
                {
                    bool hasSeq1 = errEmptyInv.Any(row => row.Table.Columns.Contains("Seq1"));
                    if (hasSeq1)
                    {
                        foreach (DataRow row in errEmptyInv)
                        {
                            if (row.Table.Columns["Seq1"] != null)
                            {
                                sb.AppendLine().Append(row["POID"])
                                    .Append("-").Append(row["Seq1"]);
                            }
                            else
                            {
                                sb.AppendLine().Append(row["POID"]);
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow row in errEmptyInv)
                        {
                            sb.AppendLine().Append(row["POID"]);
                        }
                    }
                }
                else if (sourceTable.Equals("DomesticAP", StringComparison.OrdinalIgnoreCase))
                {
                    // 費用請款沒有小項
                    foreach (DataRow row in errEmptyInv)
                    {
                        sb.AppendLine().Append(row["DomesticExpressID"]);
                    }
                }
                else
                {
                    foreach (DataRow row in errEmptyInv)
                    {
                        if (row.Table.Columns["Seq1"] != null && row.Table.Columns["Seq2"] != null)
                        {
                            sb.AppendLine().Append(row["POID"])
                                .Append("-").Append(row["Seq1"])
                                .Append("-").Append(row["Seq2"]);
                        }
                        else
                        {
                            sb.AppendLine().Append(row["POID"]);
                        }
                    }
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 費用請款 Amount  空白的錯誤
        /// </summary>
        /// <inheritdoc />
        public static bool Check_ExpenseDetail_Amount_Error(IEnumerable<DataRow> data, string sourceTable)
        {
            var errEmptyInv = data.Where(row => Check.Empty(row["Amount"]));
            if (errEmptyInv.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Amount can't be empty ");
                foreach (DataRow row in errEmptyInv)
                {
                    if (sourceTable.EqualString("DomesticAP"))
                    {
                        sb.AppendLine().Append(row["DomesticExpressID"]);
                    }
                    else
                    {
                        sb.AppendLine().Append(row["ExportID"]);
                    }
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Qty 是 0 的錯誤
        /// </summary>
        /// <inheritdoc />
        public static bool Check_ApDetail_QtyEmpty(IEnumerable<DataRow> data)
        {
            var errQty = data.Where(row => Check.Empty(row["Qty"]));
            if (errQty.Count() != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Qty should not be Empty ");
                foreach (DataRow row in errQty)
                {
                    sb.AppendLine().Append(row["POID"])
                        .Append("-").Append(row["Seq1"])
                        .Append("-").Append(row["Seq2"]);
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查資料集裡面的 exportid,poid,seq1,seq2,invoiceNo 組合是否有重複
        /// </summary>
        /// <returns>true 代表正確, 沒重複</returns>
        /// <inheritdoc />
        public static bool Check_ApDetail_Unique_MtlAp(IEnumerable<DataRow> data)
        {
            var dup = (from row in data
                       group row by new
                       {
                           wk = row["ExportID"].ToString().TrimEnd(),
                           po = row["POID"].ToString().TrimEnd(),
                           seq1 = row["Seq1"].ToString().TrimEnd(),
                           seq2 = row["Seq2"].ToString().TrimEnd(),
                           inv = row["invoiceNo"].ToString().TrimEnd()
                       }

into gp
                       select new
                       {
                           gp.Key.wk,
                           gp.Key.po,
                           gp.Key.seq1,
                           gp.Key.seq2,
                           gp.Key.inv,
                           Count = gp.Count()
                       }).Where(g => g.Count > 1);
            if (dup.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("These items are duplicate in WK #,PO item, Invoice #:");
                foreach (var a in dup)
                {
                    sb.AppendLine().Append(a.wk)
                        .Append(",").Append(a.po).Append("-").Append(a.seq1).Append("-").Append(a.seq2)
                        .Append(",").Append(a.inv)
                        .Append("  x ").Append(a.Count).Append(string.Empty);
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查資料集裡面的 poid,seq1,seq2 組合是否有重複
        /// </summary>
        /// <returns>true 代表正確, 沒重複</returns>
        /// <inheritdoc />
        public static bool Check_ApDetail_Unique_TTBefore(IEnumerable<DataRow> data)
        {
            var dup = (from row in data
                       group row by new
                       {
                           po = row["POID"].ToString().TrimEnd(),
                           seq1 = row["Seq1"].ToString().TrimEnd(),
                           seq2 = row["Seq2"].ToString().TrimEnd()
                       }

into gp
                       select new
                       {
                           gp.Key.po,
                           gp.Key.seq1,
                           gp.Key.seq2,
                           Count = gp.Count()
                       }).Where(g => g.Count > 1);
            if (dup.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("These items are duplicate in PO item :");
                foreach (var a in dup)
                {
                    sb.AppendLine()
                        .Append(",").Append(a.po).Append("-").Append(a.seq1).Append("-").Append(a.seq2)
                        .Append("  x ").Append(a.Count).Append(string.Empty);
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查 ExportID是否重複
        /// </summary>
        /// <param name="data">檢查目標</param>
        /// <param name="isCheckInvoiceNo">檢查時是否要包含InvoiceNo</param>
        /// <returns>True:Pass;False:重複了</returns>
        public static bool Check_ApDetail_Unique_Expense(IEnumerable<DataRow> data)
        {
            var dup = (from row in data
                       group row by new
                       {
                           ExportID = row["ExportID"].ToString().TrimEnd(),
                           InvNo = row["InvoiceNo"].ToString()
                       }

                       into gp
                       select new
                       {
                           gp.Key.ExportID,
                           gp.Key.InvNo,
                           Count = gp.Count()
                       }).Where(g => g.Count > 1);
            if (dup.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();

                // 2024/02/22 IST20240105 Alex 改為全需判斷Invoice#
                sb.Append("These items are duplicate in WK# / HC#, Invoice#  :");
                foreach (var a in dup)
                {
                    sb.AppendLine()
                        .Append(",").Append(a.ExportID)
                        .Append(",").Append(a.InvNo)
                        .Append("  x ").Append(a.Count).Append(string.Empty);
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查 DomesticExpressID 是否重複
        /// </summary>
        /// <param name="data">檢查目標</param>
        /// <returns>True:Pass;False:重複了</returns>
        public static bool Check_ApDetail_Unique_DomesticAP(IEnumerable<DataRow> data)
        {
            var dup = (from row in data
                       group row by new
                       {
                           DomesticExpressID = row["DomesticExpressID"].ToString().TrimEnd(),
                       }

                       into gp
                       select new
                       {
                           gp.Key.DomesticExpressID,
                           Count = gp.Count()
                       }).Where(g => g.Count > 1);
            if (dup.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("These items are duplicate in DE#:");
                foreach (var a in dup)
                {
                    sb.AppendLine()
                        .Append(",").Append(a.DomesticExpressID)
                        .Append("  x ").Append(a.Count).Append(string.Empty);
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 1.檢查 空白POID的 Seq1是否有重複
        /// <para>2.檢查有POID的項目, PO_Surcharge_Ukey / MmsPO_Surcharge_Ukey是否重複</para>
        /// </summary>
        /// <inheritdoc />
        public static bool Check_ApDetail_Unique_Surcharge(IEnumerable<DataRow> data)
        {
            // 1.檢查 空白POID的 Seq1是否有重複
            var dup = (from row in data.Where(row => Check.Empty(row["POID"]))
                       group row by new
                       {
                           Seq1 = row["Seq1"].ToString().TrimEnd()
                       }

into gp
                       select new
                       {
                           gp.Key.Seq1,
                           Count = gp.Count()
                       }).Where(g => g.Count > 1);

            // 2.檢查有POID的項目, PO_Surcharge_Ukey / MmsPO_Surcharge_Ukey是否重複
            var dup2 = (from row in data.Where(row => !Check.Empty(row["POID"]))
                        group row by new
                        {
                            Ukey = Check.Empty(row["PO_Surcharge_Ukey"])
                                ? SciConvert.GetLong(row["MmsPO_Surcharge_Ukey"])
                                : SciConvert.GetLong(row["PO_Surcharge_Ukey"]),
                            POID = row["POID"].ToString().TrimEnd(),
                            Seq1 = row["Seq1"].ToString().TrimEnd()
                        }

into gp
                        select new
                        {
                            gp.Key.POID,
                            gp.Key.Seq1,
                            Count = gp.Count()
                        }).Where(g => g.Count > 1);
            if (dup.Any() || dup2.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("These items are duplicate in PO# + Seq  :");
                foreach (var a in dup)
                {
                    sb.AppendLine()
                        .Append("Blank PO # -").Append(a.Seq1)
                        .Append("  x ").Append(a.Count);
                }

                foreach (var a in dup2)
                {
                    sb.AppendLine()
                        .Append(a.POID).Append("-").Append(a.Seq1)
                        .Append("  x ").Append(a.Count);
                }

                Msg.WarningBox(sb.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查是否有Discount
        /// </summary>
        /// <param name="data">檢查目標</param>
        /// <param name="apID">APID</param>
        /// <returns>True/False</returns>
        public static bool Check_ApDetail_Discount(IEnumerable<DataRow> data, string apID)
        {
            foreach (DataRow dr in data)
            {
                string invNo_Old = string.Empty;
                if (dr["InvoiceNo_old"].ToString() != dr["InvoiceNo"].ToString())
                {
                    invNo_Old = dr["InvoiceNo_old"].ToString();
                }
                else
                {
                    continue;
                }

                if (DBProxy.Current.SeekEx("Select 1 From AP_Discount Where APID = @ID and InvoiceNo = @InvoiceNo", "ID", apID, "InvoiceNo", invNo_Old).ExtendedData != null)
                {
                    MyUtility.Msg.WarningBox($"{invNo_Old} 有Discount，不可修改Invoice #!");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 檢查是否有LC
        /// </summary>
        /// <param name="data">檢查目標</param>
        /// <param name="apID">APID</param>
        /// <returns>True/False</returns>
        public static bool Check_ApDetail_LC(DataTable data, string apID, string sourceTable)
        {
            string sql = string.Empty;

            // AP
            if (sourceTable == "AP")
            {
                sql = @"with apd as(
select tmp.ID
, tmp.POID
, tmp.Seq1
, tmp.Seq2
, tmp.Price
, Amount = sum(tmp.Amount)
From #tmp tmp
Group by tmp.ID, tmp.POID, tmp.Seq1, tmp.Seq2, tmp.Price
)
Select LCID = lad.ID, lad.POID, lad.Seq1, lad.Seq2
From LC_Application_Detail lad
Left join LC_Application la on lad.ID = la.ID
inner join apd on lad.SourceID = apd.ID and lad.POID = apd.POID and lad.Seq1 = apd.Seq1 and lad.Seq2 = apd.Seq2
Where apd.Amount != lad.Amount
and la.Status != 'New'";
            }

            // Surcharge
            else
            {
                sql = @"
with apd as(
select tmp.ID
, tmp.POID
, tmp.Seq1
, Amount = sum(tmp.Amount)
From #tmp tmp
Group by tmp.ID, tmp.POID, tmp.Seq1
)

Select LCID = lad.ID, lad.POID, lad.Seq1
From LC_Application_Detail lad
Left join LC_Application la on lad.ID = la.ID
inner join apd on lad.SourceID = apd.ID and lad.POID = apd.POID and lad.Seq1 = apd.Seq1
Where apd.Amount != lad.Amount
and la.Status != 'New'";
            }

            DataTable dt;
            var res = MyUtility.Tool.ProcessWithDatatable(data, string.Empty, sql, out dt);

            if (!res)
            {
                MyUtility.Msg.WarningBox("Check LC Amount failed!");
                return false;
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                string lcid = string.Join(",", dt.AsEnumerable().Select(r => r.Field<string>("LCID")));
                string poid = string.Empty;
                if (sourceTable == "AP")
                {
                    poid = string.Join(Environment.NewLine, dt.AsEnumerable().Select(r => r.Field<string>("POID") + "-" + r.Field<string>("Seq1") + "-" + r.Field<string>("Seq2")));
                }
                else
                {
                    poid = string.Join(Environment.NewLine, dt.AsEnumerable().Select(r => r.Field<string>("POID") + "-" + r.Field<string>("Seq1")));
                }

                MyUtility.Msg.WarningBox($"{poid + Environment.NewLine}已有{lcid}且狀態不是New，不能調整金額!");
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public static bool Junk_AP(string apID, string sourceTable, bool fromP61 = false)
        {
            if (Msg.QuestionBox("Are you sure to Cancel it ?") == DialogResult.No)
            {
                return false;
            }

            string cmd = string.Empty;

            if (fromP61)
            {
                cmd = $@"Delete inv
                           FROM ArtworkTest_Invoice inv
                           inner join {sourceTable}_Detail d on inv.id = d.poid and inv.Seq = d.Seq2
                           where d.ID = @ID and d.poid like 'AX%'";
            }

            var sqlUpdateTax = string.Empty;
            if (!sourceTable.EqualString("NP"))
            {
                sqlUpdateTax = ", TAX = 0";
            }

            string sqlCmd = $@"               
{cmd}

delete from dbo.{sourceTable}_Detail where ID = @ID

update dbo.{sourceTable} 
set status = @Status 
    , editName = @userID 
    , editDate = @editDate
    , Amount = 0 
    {sqlUpdateTax}
where ID = @ID
";
            if ((sourceTable == "TTBefore") || (sourceTable == "TTBeforeMms") || (sourceTable == "Expense"))
            {
                string type = string.Empty;
                if (sourceTable == "TTBeforeMms")
                {
                    type = "PT";
                }
                else if (sourceTable == "TTBefore")
                {
                    type = apID.Substring(0, 2); // Type增加了OP,改取APID前兩碼
                    // type = "PP";
                }
                else
                {
                    type = "WE";
                }

                TradeHisPrg.insertHistory("Export", sourceTable, type, apID, "New", "Junked", string.Empty, string.Empty, string.Empty);
            }
            else if (sourceTable.EqualString("NP"))
            {
                TradeHisPrg.insertHistory("Mms", sourceTable, apID.Substring(0, 2), apID, "New", "Junked", string.Empty, string.Empty, string.Empty);
            }
            else if (sourceTable.EqualString("DomesticAP"))
            {
                TradeHisPrg.insertHistory("Purchase", sourceTable, apID.Substring(0, 2), apID, "New", "Junked", string.Empty, string.Empty, string.Empty);
            }
            else
            {
                TradeHisPrg.insertHistory("Export", sourceTable, apID.Substring(0, 2), apID, "New", "Junked", string.Empty, string.Empty, string.Empty);
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));
            pars.Add(new SqlParameter("@editDate", DateTime.Now));
            pars.Add(new SqlParameter("@Status", AccountPayable.status_Junked));

            TransactionScope scope = new TransactionScope();
            if (!SQL.Execute(string.Empty, sqlCmd, pars))
            {
                scope.Dispose();
                return false;
            }

            scope.Complete();
            scope.Dispose();

            return true;
        }

        /// <summary>
        /// Recall
        /// </summary>
        /// <inheritdoc />
        public static bool Recall_AP(string apID, string reasonID, string reasonRemark, string sourceTable)
        {
            string updateCmd = string.Format(
@"
update dbo.{0}
    set status = @Status ,status_upd = @editDate
        , editName = @userID , editDate = @editDate
    where ID = @ID
", sourceTable);
            if ((sourceTable == "TTBefore") || (sourceTable == "TTBeforeMms"))
            {
                string type = string.Empty;
                if (sourceTable == "TTBeforeMms")
                {
                    type = "PT";
                }
                else
                {
                    type = apID.Substring(0, 2); // Type增加了OP,改取APID前兩碼
                    // type = "PP";
                }

                TradeHisPrg.insertHistory("Export", sourceTable, type, apID, "Sent", "New", reasonRemark, AccountPayable.status_ReasonType_Recall, reasonID);
            }
            else if (sourceTable.EqualString("NP"))
            {
                TradeHisPrg.insertHistory("Mms", sourceTable, apID.Substring(0, 2), apID, "Sent", "New", reasonRemark, AccountPayable.status_ReasonType_Recall, reasonID);
            }
            else
            {
                TradeHisPrg.insertHistory("Export", sourceTable, apID.Substring(0, 2), apID, "Sent", "New", reasonRemark, AccountPayable.status_ReasonType_Recall, reasonID);
            }

            // Recall Matertial AP , Mms AP (After付款) 的話需將Export那邊的InvoiceNo 改回空白
            if (apID.StartsWith(DocNo.AP, StringComparison.OrdinalIgnoreCase)
                || apID.StartsWith(DocNo.PartAP, StringComparison.OrdinalIgnoreCase))
            {
                updateCmd =
@"
update e
	set e.FormXPayINV = ''
from dbo.Export_Detail as e
inner join dbo.AP_Detail as a on a.ID = @ID and a.Export_DetailUkey = e.Ukey
"
+ Environment.NewLine + updateCmd;
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));
            pars.Add(new SqlParameter("@editDate", DateTime.Now));
            pars.Add(new SqlParameter("@Status", AccountPayable.status_New));
            pars.Add(new SqlParameter("@ReasonTypeID", AccountPayable.status_ReasonType_Recall));
            pars.Add(new SqlParameter("@ReasonID", reasonID));
            pars.Add(new SqlParameter("@Remark", reasonRemark));

            TransactionScope scope = new TransactionScope();
            if (!SQL.Execute(string.Empty, updateCmd, pars))
            {
                scope.Dispose();
                return false;
            }

            scope.Complete();
            scope.Dispose();

            return true;
        }

        /// <summary>
        /// Sent
        /// </summary>
        /// <inheritdoc />
        public static bool Send_AP(string apID, string sourceTable)
        {
            string updateCmd = string.Format(
                @"
update dbo.{0} 
    set status = @Status ,status_upd = @editDate
        , editName = @userID , editDate = @editDate
    where ID = @ID
", sourceTable);

            if ((sourceTable == "TTBefore") || (sourceTable == "TTBeforeMms"))
            {
                string type = string.Empty;
                if (sourceTable == "TTBeforeMms")
                {
                    type = "PT";
                }
                else
                {
                    type = apID.Substring(0, 2); // Type增加了OP,改取APID前兩碼
                    // type = "PP";
                }

                TradeHisPrg.insertHistory("Export", sourceTable, type, apID, "New", "Sent", string.Empty, string.Empty, string.Empty);
            }
            else if (sourceTable.EqualString("NP"))
            {
                TradeHisPrg.insertHistory("Mms", sourceTable, apID.Substring(0, 2), apID, "New", "Sent", string.Empty, string.Empty, string.Empty);
            }
            else
            {
                TradeHisPrg.insertHistory("Export", sourceTable, apID.Substring(0, 2), apID, "New", "Sent", string.Empty, string.Empty, string.Empty);
            }

            // Sent Matertial AP , Mms AP (After付款) 的話需將Export那邊的InvoiceNo update
            if (apID.StartsWith(DocNo.AP, StringComparison.OrdinalIgnoreCase)
                || apID.StartsWith(DocNo.PartAP, StringComparison.OrdinalIgnoreCase))
            {
                updateCmd = AccountPayable.GetSqlCmd_AP_UpdateExportDetail_PayInv("dbo.AP_Detail", " a.ID = @ID and a.Export_DetailUkey = e.Ukey")
                            + Environment.NewLine + updateCmd;
            }

            if (apID.StartsWith(DocNo.POAP, StringComparison.OrdinalIgnoreCase))
            {
                updateCmd = AccountPayable.GetSqlCmd_AP_UpdateArtworkTest_InvData(apID) + Environment.NewLine + updateCmd;
            }

            List<SqlCommandText> updateList = new List<SqlCommandText>();
            if (sourceTable.EqualString("AP"))
            {
                // Send時，確保OriAmount 都有填上值
                string apDetailsql = $@"update AP_Detail Set OriAmount = iif(AP_Detail.OriAmount = 0, s.Amount,s.OriAmount)
FROM (
select Ukey,Amount,OriAmount From AP_Detail
) s
where s.Ukey = AP_Detail.Ukey and AP_Detail.ID = @ID";

                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", apID));
                updateList.Add(new SqlCommandText(apDetailsql, paras));

                // Send時，計算Tax填入ActTax
                string sql = $@"
update AP set ActTax = DiffTax
where AP.ID = @ID";

                List<SqlParameter> para = new List<SqlParameter>();
                para.Add(new SqlParameter("@ID", apID));
                para.Add(new SqlParameter("@userID", Env.User.UserID));
                para.Add(new SqlParameter("@editDate", DateTime.Now));
                updateList.Add(new SqlCommandText(sql, para));
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));
            pars.Add(new SqlParameter("@editDate", DateTime.Now));
            pars.Add(new SqlParameter("@Status", AccountPayable.status_Sent));

            updateList.Add(new SqlCommandText(updateCmd, pars));
            TransactionScope scope = new TransactionScope();
            var result = DBProxy.Current.Executes(string.Empty, updateList);
            if (!result)
            {
                scope.Dispose();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            return true;
        }

        /// <inheritdoc />
        public static bool SendRecall_Expense(string apID, bool isSend, string reasonID, string reasonRemark, Sci.Win.Forms.Base form)
        {
            string updateCmd = @"
update t
	set t.Status = @WK_Status , t.StatusUpdate = getDate()
from dbo.Export_ShareAmount as t
inner join (
	select com.SubExportId
	from dbo.Expense as ex
	left join dbo.ExpenseType as et on ex.Type = et.Id
	left join dbo.Expense_Detail as ex2 on ex2.ID = ex.ID
	outer apply dbo.GetExportCombo(ex2.ExportID,'01') as com
	inner join dbo.Export_ShareAmount as s on s.ID = com.SubExportId
	where ex.ID = @ID and et.IsFreigth = 'Y'  -- ExpenseType會設定是否需回寫 Status
) as s on s.SubExportId = t.ID

update dbo.Expense 
    set status = @Status ,StatusUpdate = @editDate
        , editName = @userID , editDate = @editDate
    where ID = @ID
";
            if (isSend)
            {
                TradeHisPrg.insertHistory("Export", "Expense", "WE", apID, "New", "Sent", string.Empty, string.Empty, string.Empty);
            }
            else
            {
                TradeHisPrg.insertHistory("Export", "Expense", "WE", apID, "Sent", "New", reasonRemark, AccountPayable.status_ReasonType_Recall, reasonID);
            }

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));
            pars.Add(new SqlParameter("@editDate", DateTime.Now));
            pars.Add(new SqlParameter("@Status", isSend ? AccountPayable.status_Sent : AccountPayable.status_New));
            pars.Add(new SqlParameter("@ReasonTypeID", isSend ? string.Empty : AccountPayable.status_ReasonType_Recall));
            pars.Add(new SqlParameter("@ReasonID", isSend ? string.Empty : reasonID));
            pars.Add(new SqlParameter("@Remark", isSend ? string.Empty : reasonRemark));
            pars.Add(new SqlParameter("@WK_Status", isSend ? ExportPrg.status_ShareFreight_Approved : ExportPrg.status_ShareFreight_New));

            TransactionScope scope = new TransactionScope();
            bool ok = isSend
                ? Send_Expense(updateCmd, pars, form)
                : Recall_Expense(updateCmd, pars);

            if (!ok)
            {
                scope.Dispose();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            return true;
        }

        /// <inheritdoc />
        private static bool Send_Expense(string sqlCmd, List<SqlParameter> pars, Sci.Win.Forms.Base form)
        {
            // Send 需額外檢查金額跟WK不一致的問題
            // 2018/05/08 [IST20180526] modify by Anderson 因為運費可以拆開申請,所以將同ExportID申請的金額加總起來後比對
            sqlCmd = @"
Select * 
From (
	Select ex2.ExportID
	    , Amount = Sum(ex2.Amount)
	    , WK_Amount = wkAmt.WK_Amount
    From dbo.Expense as ex
    Left join dbo.Expense_Detail as ex2 on ex2.ID = ex.ID
    Left join dbo.ExpenseType as et on ex.Type = et.Id
    Outer apply (
        Select WK_Amount = case ex.Type
		    When '01' then Sum(getSubWK.Freight)
		    When '02' then Sum(getSubWK.Insurance)
            When '03' then Sum(getSubWK.TruckFee)
            When '04' then Sum(getSubWK.CustomsFee)
		    Else 0 
		    End
        From dbo.GetExportCombo(ex2.ExportID, ex.Type) e
	    Outer apply (
		    Select Freight, Insurance, TruckFee, CustomsFee
		    From dbo.Export
		    Where ID = e.SubExportId
		    Union
		    Select Freight, Insurance, TruckFee, CustomsFee
		    From dbo.TransferExport
		    Where ID = e.SubExportId
	    ) getSubWK
    ) as wkAmt
    Where ex.ID = @ID And et.IsFreigth = 'Y'
    Group by ex2.ExportID, wkAmt.WK_Amount
) as s 
Where s.Amount <> s.WK_Amount 
"
                + Environment.NewLine + sqlCmd;
            DataTable fails;
            if (!SQL.Select(string.Empty, sqlCmd, out fails, pars))
            {
                return false;
            }

            if (fails != null && fails.Rows.Count != 0)
            {
                MsgGridForm alert;
                alert = new MsgGridForm(fails, "請款金額與WK不符,請檢查!", "Error");
                alert.grid1.Columns[0].Width = 160;
                alert.grid1.Columns[0].HeaderText = "WK #";
                alert.grid1.Columns[1].Width = 130;
                alert.grid1.Columns[1].HeaderText = "AP Amount";
                alert.grid1.Columns[2].Width = 130;
                alert.grid1.Columns[2].HeaderText = "WK Amount";
                UIClassPrg.SetGrid_HeaderBorderStyle(alert.grid1);
                alert.Show(form);
                return false;
            }

            #region 更新WK商港服務費、推廣費
            var type = DBProxy.Current.LookupEx<string>("Select Type From Expense Where ID = @ID", "ID", pars[0].Value).ExtendedData;
            string sql = string.Empty;
            if (type == "06")
            {
                sql = @"
update wk
set wk.HarborServiceFee = e2.Amount
From Export as wk
inner join(
    select ExportID,Amount from Expense_Detail
    where ID = @ID
) as e2
on e2.ExportID = wk.ID";
            }
            else if (type == "07")
            {
                sql = @"
update wk
set wk.PromotionFee = e2.Amount
From Export as wk
inner join(
    select ExportID,Amount from Expense_Detail
    where ID = @ID
) as e2
on e2.ExportID = wk.ID";
            }

            if (sql != string.Empty)
            {
                bool updateOk = SQL.Execute(SQL.queryConn, sql, pars);
                if (!updateOk)
                {
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <inheritdoc />
        private static bool Recall_Expense(string sqlCmd, List<SqlParameter> pars)
        {
            return SQL.Execute(string.Empty, sqlCmd, pars);
        }

        /// <inheritdoc />
        public static bool Send_DomesticAP(string apID)
        {
            string updateCmd = @"
Update de
	set de.Amount = apData.Amount, de.PayDate = apData.CDate, de.Remark = apData.Remark, de.EditDate = @editDate, de.EditName = @userID
From dbo.DomesticExpress de
inner join (
	Select ap2.DomesticExpressID, ap2.Amount, ap.CDate, ap2.Remark
	From dbo.DomesticAP ap
	Left join dbo.DomesticAP_Detail ap2 on ap.ID = ap2.ID
	Where ap.ID = @ID
) as apData on apData.DomesticExpressID = de.ID

Update dbo.DomesticAP 
Set status = @Status, StatusUpdate = @editDate, editName = @userID , editDate = @editDate
Where ID = @ID
" + sql_Update_DomesticAP_Share;

            TradeHisPrg.insertHistory("Purchase", "DomesticAP", "DP", apID, "New", "Sent", string.Empty, string.Empty, string.Empty);

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@apDetailUkey", string.Empty));
            pars.Add(new SqlParameter("@Status", AccountPayable.status_Sent));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));
            pars.Add(new SqlParameter("@editDate", DateTime.Now));

            TransactionScope scope = new TransactionScope();

            bool ok = SQL.Execute(SQL.queryConn, updateCmd, pars);

            if (!ok)
            {
                scope.Dispose();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            return true;
        }

        /// <inheritdoc />
        public static bool Recall_DomesticAP(string apID, string reasonRemark, string reasonID)
        {
            string updateCmd = @"

Delete DomesticAP_Share where DomesticExpress_Detail_Ukey in (
select ded.Ukey
From DomesticAP_Detail ap2
Left join DomesticExpress_Detail ded on ap2.DomesticExpressID = ded.ID
Where ap2.ID = @ID
)

Update dbo.DomesticAP 
Set status = @Status, StatusUpdate = @editDate, editName = @userID , editDate = @editDate
Where ID = @ID
";
            TradeHisPrg.insertHistory("Purchase", "DomesticAP", "DP", apID, "Sent", "New", reasonRemark, AccountPayable.status_ReasonType_Recall, reasonID);

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@Status", AccountPayable.status_New));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));
            pars.Add(new SqlParameter("@editDate", DateTime.Now));

            TransactionScope scope = new TransactionScope();

            bool ok = SQL.Execute(SQL.queryConn, updateCmd, pars);

            if (!ok)
            {
                scope.Dispose();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            return true;
        }

        /// <summary>
        /// 更新 AP_Detail的OriAmount，將Amount更新至OriAmount。
        /// </summary>
        /// <param name="apID">apID</param>
        /// <inheritdoc />
        public static bool Revise_ApDetail_OriAmount(string apID)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            string tsql =
                @"
update a set 
	 a.OriAmount = a.Amount 
from dbo.AP_Detail as a
where a.ID = @ID
";
            TransactionScope scope = new TransactionScope();
            SqlConnection conn;
            SQL.GetConnection(out conn);
            bool updateOk = SQL.Execute(conn, tsql, pars);
            if (!updateOk)
            {
                scope.Dispose();
                conn.Close();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            conn.Close();
            return true;
        }

        /// <summary>
        /// 1.從採購單 update price / unit
        /// <para>2.更新 AP_Detail的Amount </para>
        /// <para>3.重算 AP 的Amount </para>
        /// <para>4.寫入log 到AP_Revise</para>
        /// </summary>
        /// <param name="apID">apID</param>
        /// <param name="sourceTable">"AP" ,"TTBefore" , ... </param>
        /// <inheritdoc />
        public static bool Revise_ApDetail_FromPO(string apID, string sourceTable)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));

            string update_AP_DiffTax = $@"
IF @@ROWCOUNT > 0
BEGIN
    {AccountPayable.sql_Update_AP_DiffTax}
END
";

            string tsql = string.Format(
                @"
update a set a.Price = p.Price
, a.UnitID = p.POUnit
, a.Amount = cast(amt.Amount as numeric(11,4))
output inserted.ID {1}, inserted.POID, inserted.Seq1, inserted.Seq2, deleted.Price, inserted.Price, deleted.UnitID, inserted.UnitID, GETDATE(), @userID
into dbo.AP_Revise (APID {2}, POID, Seq1, Seq2, Old_Price, New_Price, Old_Unit, New_Unit, AddDate, AddName)	
from dbo.{0}_Detail as a
inner join dbo.PO_Supp_Detail as p on p.ID = a.POID and p.SEQ1 = a.Seq1 and p.SEQ2 = a.Seq2
inner join dbo.{0} as Ap1 on Ap1.ID = @ID 
left join dbo.Currency as cur on cur.ID = Ap1.CurrencyID 
outer apply dbo.GetAmountByUnit(p.Price, a.Qty, p.PoUnit, cur.Exact) as amt
where a.ID = @ID and (a.Price <> p.Price or a.UnitID <> p.POUnit)

{3}
",
                sourceTable,
                sourceTable.EqualString("TTBefore") ? string.Empty : ", inserted.ExportID",
                sourceTable.EqualString("TTBefore") ? string.Empty : ", ExportID",
                sourceTable.EqualString("TTBefore") ? string.Empty : update_AP_DiffTax).ToString();
            TransactionScope scope = new TransactionScope();
            SqlConnection conn;
            SQL.GetConnection(out conn);
            bool updateOk = SQL.Execute(conn, tsql, pars)
                && AccountPayable.update_AP_AmountTax(apID, sourceTable, conn);
            if (!updateOk)
            {
                scope.Dispose();
                conn.Close();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            conn.Close();
            return true;
        }

        /// <summary>
        /// 1.從Mms採購單 update price / unit
        /// <para>2.更新 AP_Detail的Amount </para>
        /// <para>3.重算 AP 的Amount </para>
        /// <para>4.寫入log 到AP_Revise</para>
        /// </summary>
        /// <inheritdoc />
        public static bool Revise_ApDetail_FromMms(string apID, string sourceTable)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            pars.Add(new SqlParameter("@userID", Env.User.UserID));

            string tsql = string.Format(
@"
update a set a.Price = p.Price
, a.UnitID = p.UnitID
, a.Amount = cast(amt.Amount as numeric(11,4))
output inserted.ID {1} , inserted.POID, inserted.Seq1, inserted.Seq2, deleted.Price, inserted.Price, deleted.UnitID, inserted.UnitID, GETDATE(), @userID
into dbo.AP_Revise (APID {2}, POID, Seq1, Seq2, Old_Price, New_Price, Old_Unit, New_Unit, AddDate, AddName)	
from dbo.{0}_Detail as a
inner join dbo.MmsPO_Detail as p on p.ID = a.POID and p.SEQ1 = a.Seq1 and p.SEQ2 = a.Seq2
inner join dbo.{0} as Ap1 on Ap1.ID = @ID 
left join dbo.Currency as cur on cur.ID = Ap1.CurrencyID 
outer apply dbo.GetAmountByUnit(p.Price,a.Qty, p.UnitID, cur.Exact) as amt
where a.ID = @ID and (a.Price <> p.Price or a.UnitID <> p.UnitID)
",
            sourceTable,
            sourceTable.EqualString("TTBefore") ? string.Empty : ", inserted.ExportID",
            sourceTable.EqualString("TTBefore") ? string.Empty : ", ExportID").ToString();

            TransactionScope scope = new TransactionScope();
            SqlConnection conn;
            SQL.GetConnection(out conn);
            bool updateOk = SQL.Execute(conn, tsql, pars)
                && AccountPayable.update_AP_AmountTax(apID, sourceTable, conn);
            if (!updateOk)
            {
                scope.Dispose();
                conn.Close();
                return false;
            }

            scope.Complete();
            scope.Dispose();
            conn.Close();
            return true;
        }

        /// <inheritdoc />
        public static void ImportDups(Dictionary<string, DupInvoice> output, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                DupInvoice dup = new DupInvoice(row);
                if (!output.ContainsKey(dup.invoiceID) && !Check.Empty(dup.invoiceID))
                {
                    output.Add(dup.invoiceID, dup);
                }
            }
        }

        /// <inheritdoc />
        public static string GetSqlCmd_ExpenseType_CheckFields(string type, string exportAlias, string checkValue)
        {
            switch (type)
            {
                case ExportPrg.typeExpense_Freight:
                case ExportPrg.typeExpense_FtyCourierFee:
                    return exportAlias + ".Forwarder = " + checkValue;
                case ExportPrg.typeExpense_Insurance:
                    return exportAlias + ".Insurer = " + checkValue;
                case ExportPrg.typeExpense_Trailer:
                    return "(" + exportAlias + ".Trailer1 = " + checkValue + " or " + exportAlias + ".Trailer2 = " + checkValue + ")";
                case ExportPrg.typeExpense_Broker:
                    return exportAlias + ".Broker = " + checkValue;
                case ExportPrg.typeExpense_PrepaidFtyImportFee:
                case ExportPrg.typeExpense_ImportPrepaidFtyImportFee:
                case ExportPrg.typeExpense_OTFee:
                case ExportPrg.typeExpense_ImportOTFee:
                    return exportAlias + ".FtyBroker = " + checkValue;
                case ExportPrg.typeExpense_FtyTruckFee:
                case ExportPrg.typeExpense_ImportFtyTruckFee:
                    return exportAlias + ".FtyTrucker = " + checkValue;
                default: return string.Empty;
            }
        }

        /// <inheritdoc />
        public static string GetSqlCmd_ExpenseType_AmountField(string type, string exportAlias, string asAlias)
        {
            var tmpAlias = !asAlias.IsNullOrWhiteSpace() ? $"{asAlias} = " : string.Empty;

            switch (type)
            {
                case ExportPrg.typeExpense_Freight: return $"{tmpAlias}{exportAlias}.Freight";
                case ExportPrg.typeExpense_Insurance: return $"{tmpAlias}{exportAlias}.Insurance";
                case ExportPrg.typeExpense_Trailer: return $"{tmpAlias}{exportAlias}.TruckFee";
                case ExportPrg.typeExpense_Broker: return $"{tmpAlias}{exportAlias}.CustomsFee";
                case ExportPrg.typeExpense_HarborServise: return $"{tmpAlias}{exportAlias}.HarborServiceFee";
                case ExportPrg.typeExpense_Promotion: return $"{tmpAlias}{exportAlias}.PromotionFee";
                case ExportPrg.typeExpense_PrepaidFtyImportFee:
                case ExportPrg.typeExpense_ImportPrepaidFtyImportFee:
                    return $"{tmpAlias}{exportAlias}.PrepaidFtyImportFee";
                case ExportPrg.typeExpense_OTFee:
                case ExportPrg.typeExpense_ImportOTFee:
                    return $"{tmpAlias}{exportAlias}.OTFee";
                case ExportPrg.typeExpense_FtyTruckFee:
                case ExportPrg.typeExpense_ImportFtyTruckFee:
                    return $"{tmpAlias}{exportAlias}.FtyTruckFee";
                default: return "0 as " + (asAlias.IsNullOrWhiteSpace() ? "AAA" : asAlias);
            }
        }

        /// <inheritdoc />
        public static string GetSqlCmd_ExpenseType_CombineID(string type)
        {
            switch (type)
            {
                case "02": return "01";
                default: return type;
            }
        }

        /// <summary>
        /// 限制中文, 全形文字, 空白鍵
        /// </summary>
        /// <inheritdoc />
        public static void Check_InvoiceNo(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                if (!new System.Text.RegularExpressions.Regex("^[A-Za-z0-9\x21-\x7e]+$").IsMatch(e.KeyChar.ToString()))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 判斷是否在其他請款單中有重複的InvoiceNo，用於觸發Validating時
        /// </summary>
        /// <inheritdoc />
        /// 2017.07.24 Add by Eden For Finance AccountPayable Modify Form
        public static void Check_DupInvoiceNo(string apID, string invoiceNo)
        {
            if (invoiceNo == string.Empty)
            {
                return;
            }

            DataTable result = null;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@InvoiceNo", invoiceNo));
            pars.Add(new SqlParameter("@ID", apID));
            string sqlCmd =
@"
SELECT 1 FROM AP_Detail WHERE InvoiceNo = @InvoiceNo AND ID <> @ID
UNION
SELECT 1 FROM TTBefore_Detail WHERE InvoiceNo = @InvoiceNo AND ID <> @ID
UNION
SELECT 1 FROM TTBeforeMms_Detail WHERE InvoiceNo = @InvoiceNo AND ID <> @ID
UNION
SELECT 1 FROM Expense_Detail WHERE InvoiceNo = @InvoiceNo AND ID <> @ID
UNION
SELECT 1 FROM Surcharge_Detail WHERE InvoiceNo = @InvoiceNo AND ID <> @ID
UNION
SELECT 1 FROM DomesticAP_Detail WHERE InvoiceNo = @InvoiceNo AND ID <> @ID";

            if (!SQL.Select(SQL.queryConn, sqlCmd, out result, pars))
            {
                return;
            }

            if (result.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Invoice:{0} 已經出現在其他請款單中!", invoiceNo));
            }
        }

        /// <summary>
        /// 判斷同InvoiceNo是否有不同的InvoiceDate，用於觸發Validating時
        /// </summary>
        /// <inheritdoc />
        /// 2017.07.24 Add by Eden For Finance AccountPayable Modify Form
        public static void Check_DupInvoiceDate(string invoiceNo, string invoiceDate, string ukey, string sourceTable)
        {
            if (invoiceNo == string.Empty)
            {
                return;
            }

            DataTable result = null;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@InvoiceNo", invoiceNo));
            pars.Add(new SqlParameter("@InvoiceDate", invoiceDate));
            pars.Add(new SqlParameter("@Ukey", ukey));
            string sqlCmd = string.Format(
@"SELECT 1 FROM AP_Detail WHERE InvoiceNo = @InvoiceNo AND InvoiceDate <> @InvoiceDate " + (sourceTable == "AP" ? "And Ukey <> @Ukey" : string.Empty) +
@"
UNION
SELECT 1 FROM TTBefore_Detail WHERE InvoiceNo = @InvoiceNo AND InvoiceDate <> @InvoiceDate " + (sourceTable == "TTBefore" ? "And Ukey <> @Ukey" : string.Empty) +
@"
UNION
SELECT 1 FROM TTBeforeMms_Detail WHERE InvoiceNo = @InvoiceNo AND InvoiceDate <> @InvoiceDate " + (sourceTable == "TTBeforeMms" ? "And Ukey <> @Ukey" : string.Empty) +
@"
UNION
SELECT 1 FROM Expense_Detail WHERE InvoiceNo = @InvoiceNo AND InvoiceDate <> @InvoiceDate " + (sourceTable == "Expense" ? "And Ukey <> @Ukey" : string.Empty) +
@"
UNION
SELECT 1 FROM Surcharge_Detail WHERE InvoiceNo = @InvoiceNo AND InvoiceDate <> @InvoiceDate " + (sourceTable == "Surcharge" ? "And Ukey <> @Ukey" : string.Empty) +
@"
UNION
SELECT 1 FROM DomesticAP_Detail WHERE InvoiceNo = @InvoiceNo AND InvoiceDate <> @InvoiceDate " + (sourceTable == "DomesticAP" ? "And Ukey <> @Ukey" : string.Empty),
                            invoiceNo,
                            invoiceDate,
                            ukey);

            if (!SQL.Select(SQL.queryConn, sqlCmd, out result, pars))
            {
                return;
            }

            if (result.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Invno:{0} 有不同的[Invoice Date]", invoiceNo));
            }
        }

        /// <summary>
        /// 判斷AP_Detail的Invoice Date是否有跨兩個稅率設定
        /// </summary>
        /// <inheritdoc />
        public static bool Check_TaxRate(string apID, string sourceTable)
        {
            DataTable result = null;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            string sqlCmd = $@"
select count(distinct com.TaxRate) count from {sourceTable} a
left join {sourceTable}_Detail d on a.ID = d.ID
outer apply (
		select iif((a.Type = '01' and a.CurrencyID = 'CNY'),6, c.TaxRate) as TaxRate
        from dbo.Company_Tax as c
        where c.ID = a.CompanyID and d.InvoiceDate>= c.StartDate and (c.EndDate >= d.InvoiceDate or c.EndDate is null )
) com
where a.id = @ID
";
            if (!SQL.Select(string.Empty, sqlCmd, out result, pars))
            {
                return false;
            }

            if (result != null && result.Rows.Count != 0)
            {
                if (Convert.ToDecimal(result.Rows[0]["count"]) > 1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 取得Company對應稅率
        /// </summary>
        /// <inheritdoc />
        public static List<decimal> GetCompanyTax(int companyID)
        {
            DataTable dt = DBProxy.Current.SelectEx($"select TaxRate from Company_Tax where ID = @CompanyID order by EndDate desc", "CompanyID", companyID).ExtendedData;
            List<decimal> results = dt.Rows.OfType<DataRow>().Select(dr => dr.Field<decimal>("TaxRate")).ToList();

            return results;
        }

        /// <summary>
        /// 當公司別是4.惠中、5.惠興、6.貴州的時候，系統的稅額允許被修改。
        /// </summary>
        /// <inheritdoc />
        public static bool GetCompanyTaxEnable(int companyID)
        {
            switch (companyID)
            {
                case 4:
                case 5:
                case 6:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// RelationID_Validating
        /// </summary>
        /// <param name="reasonID">reasonID</param>
        /// <param name="relationID">relationID</param>
        /// <returns>string</returns>
        public static string RelationID_Validating(string reasonID, string relationID)
        {
            string sqlCmd = string.Empty;
            string message = string.Empty;
            switch (reasonID)
            {
                case "01":
                case "02":
                    sqlCmd = string.Format(
                        @"SELECT 1 FROM ICR WHERE ID='{0}' AND Status!='Cancel'
                        UNION ALL
                        SELECT 1 FROM SuppDebit WHERE ID='{0}' AND Status!='Junked'
                        UNION ALL
                        SELECT 1 FROM Debit WHERE ID='{0}' AND Status!='Junked'",
                        relationID);
                    message = "Data not found. Please input ICR#、DBC# or SD# and status not <Junk>";
                    break;
                case "04":
                    sqlCmd = string.Format(@"SELECT 1 FROM Export WHERE ID='{0}' and Junk = 0", relationID);
                    message = "WK# not found!!!";
                    break;
                case "07":
                    sqlCmd = string.Format(
                                      @"select 1 from SuppDebit as sd where sd.id = '{0}' and sd.Status <> 'Junked'
                                         union all
                                         select 1 from Debit as d where d.id = '{0}' and d.Status <> 'Junked'", relationID);
                    message = "Data not found. Please input DBC# or SD# and check status not <Junk>";
                    break;
                case "10":
                case "12":
                    sqlCmd = string.Format(@"SELECT 1 FROM Factory WHERE ID='{0}' and Junk = 0", relationID);
                    message = "Factory not found!!!";
                    break;
                case "11":
                    sqlCmd = string.Format(@"SELECT 1 FROM TTBefore WHERE Type = 'PP' and ID='{0}' and Status != 'Junked'", relationID);
                    message = "Can not find PP#!!!";
                    break;
            }

            DataTable result = null;

            if ((sqlCmd.Length == 0) || (!SQL.Select(SQL.queryConn, sqlCmd, out result)))
            {
                return string.Empty;
            }

            if (result.Rows.Count > 0)
            {
                return string.Empty;
            }

            return message;
        }

        /// <summary>
        /// 更新Discount Amount
        /// </summary>
        /// <param name="apID">請款單號</param>
        /// <param name="conn">Connection</param>
        /// <returns>執行結果</returns>
        public static DualResult UpdateDiscountAmount(string apID, SqlConnection conn = null)
        {
            if (conn == null)
            {
                if (!SQL.GetConnection(out conn))
                {
                    return new DualResult(false, "Get connection fail.");
                }
            }

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("APID", apID)
            };

            var sql = @"
Update AP_Discount
Set DiscountAmount = t.DiscountAmount
	, BalanceAmount = t.BalanceAmount
From (
	Select apdc.APID, apdc.InvoiceNo, apdc.DiscountPercent
		, DiscountAmount = Round(Sum(apd.Amount) * apdc.DiscountPercent / 100, 0)
		, BalanceAmount = Sum(apd.Amount) - Round(Sum(apd.Amount) *  apdc.DiscountPercent / 100, 0)
	From ap_discount apdc
	Left join AP on apdc.APID = AP.ID
	Left join AP_Detail apd on apd.ID = AP.ID And apd.InvoiceNo = apdc.InvoiceNo
	Where apdc.APID = @APID
	Group by apdc.APID, apdc.InvoiceNo, apdc.DiscountPercent, AP.Amount
) t
Where AP_Discount.APID = t.APID And AP_Discount.InvoiceNo = t.InvoiceNo
";
            var result = SQL.Execute(conn, sql, paras);
            if (!result)
            {
                return new DualResult(false, "Update Discount detail fail.");
            }

            return Result.True;
        }

        /// <summary>
        /// 更新Invoice
        /// </summary>
        /// <param name="apID">請款單號</param>
        /// <param name="Data">修改後的資料</param>
        /// <param name="conn">Connection</param>
        /// <returns>執行結果</returns>
        public static DualResult UpdateInvoice(string apID, DataTable data, SqlConnection conn = null, bool neednum = false)
        {
            DataTable ck = new DataTable();

            if (conn == null)
            {
                if (!SQL.GetConnection(out conn))
                {
                    return new DualResult(false, "Get connection fail.");
                }
            }

            foreach (DataRow row in data.Rows)
            {
                decimal diffAmt = 0;
                if (neednum == true)
                {
                    diffAmt = DBProxy.Current.LookupEx<decimal>($"select DiffAmount from AP_Invoice with (nolock) where InvoiceNO = '{row["invoiceNo_Old"].ToString()}'").ExtendedData;
                }

                bool isFoc = DBProxy.Current.LookupEx<bool>($@"Select top 1 isFoc From AP_Invoice where APID = @ID and InvoiceNo = @invNO", "ID", apID, "invNO", row["invoiceNo_Old"].ToString()).ExtendedData;
                var paras = new List<SqlParameter>()
                {
                new SqlParameter("@APID", apID),
                new SqlParameter("@InvoiceNo_Old", row["invoiceNo_Old"].ToString()),
                new SqlParameter("@InvoiceNo", row["InvoiceNo"].ToString()),
                new SqlParameter("@InvoiceDate", row["InvoiceDate"].ToDateTime().ToShortDateString()),
                new SqlParameter("@InvoiceDate_Old", row["InvoiceDate_Old"].ToDateTime().ToShortDateString()),
                new SqlParameter("@diffAmt", diffAmt),
                new SqlParameter("@EditName", Env.User.UserID),
                new SqlParameter("@IsFoc", isFoc)
                };

                if (row["InvoiceNo"].ToString() != row["InvoiceNo_Old"].ToString() || row["InvoiceDate_Old"].ToString() != row["InvoiceDate"].ToString())
                {
                    string sql = $@"
/*
Declare @APID as Varchar(15) = '{apID}'
Declare @InvoiceNo_Old as Varchar(15) = '{row["invoiceNo_Old"].ToString()}'
Declare @InvoiceNo as Varchar(15) = '{row["InvoiceNo"].ToString()}'
Declare @InvoiceDate as Varchar(15) = '{row["InvoiceDate"].ToDateTime().ToShortDateString()}'
Declare @InvoiceDate_Old as Varchar(15) = '{row["InvoiceDate_Old"].ToDateTime().ToShortDateString()}'
Declare @diffAmt as Varchar(15) = '{diffAmt}'
Declare @EditName as Varchar(15) = '{Env.User.UserID}'
Declare @IsFoc as Varchar(15) = '{isFoc}'
*/

                         Delete AP_Invoice
                         Where APID = @APID
                         And InvoiceNo = @InvoiceNo
                         And InvoiceDate = @InvoiceDate

                         Insert into AP_Invoice(APID, InvoiceNo, DiffAmount, InvoiceDate, EditName, EditDate, IsFoc)
                         Values (@APID, @InvoiceNo, @diffAmt, @InvoiceDate, @EditName, GetDate(), @IsFoc)

                         Update AP_Invoice 
                         Set DiffAmount = @diffAmt
                            , EditName = @EditName
                            , EditDate = GetDate()
                         Where APID = @APID 
                         And InvoiceNo = @InvoiceNo_Old 
                         And InvoiceDate = @InvoiceDate_Old

                         Delete AP_Invoice
                         where AP_Invoice.APID = @APID
                         And not exists (Select 1 
                                         From AP_Detail ad
                                         Where ad.ID = AP_Invoice.APID
                                         And ad.InvoiceNo = AP_Invoice.InvoiceNo
                                         And ad.InvoiceDate = AP_Invoice.InvoiceDate)
                    ";

                    var result = DBProxy.Current.Execute(null, sql, paras);
                    if (!result)
                    {
                        return new DualResult(false, "Update Invoice fail.");
                    }
                }
            }

            return Result.True;
        }

        /// <summary>
        /// 更新LC表頭表身Amount
        /// </summary>
        /// <param name="apID">請款單號</param>
        /// <param name="sourceTable">AP/Surcharge</param>
        /// <param name="conn">Connection</param>
        /// <returns>執行結果</returns>
        public static DualResult UpdateLCAmount(string apID, string sourceTable, SqlConnection conn = null)
        {
            if (conn == null)
            {
                if (!SQL.GetConnection(out conn))
                {
                    return new DualResult(false, "Get connection fail.");
                }
            }

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("APID", apID)
            };

            string sql = string.Empty;

            // AP
            if (sourceTable == "AP")
            {
                sql = @"
DECLARE @LCUpdateList TABLE (LCID varchar(13));

with apd as(
	Select apd.ID
, apd.POID
, apd.Seq1
, apd.Seq2
, Qty = sum(apd.Qty)
, apd.Price
, Amount = sum(apd.Amount)
From AP_Detail apd
where id = @APID
Group by apd.ID, apd.POID, apd.Seq1, apd.Seq2, apd.Price
)
Update lad set Amount = apd.Amount
OutPut la.ID into @LCUpdateList
From LC_Application_Detail lad
Left join LC_Application la on lad.ID = la.ID
inner join apd on lad.SourceID = apd.ID and lad.POID = apd.POID and lad.Seq1 = apd.Seq1 and lad.Seq2 = apd.Seq2
Where apd.Amount != lad.Amount
and la.Status = 'New'";
            }

            // Surcharge
            else
            {
                sql = @"
DECLARE @LCUpdateList TABLE (LCID varchar(13));

with apd as(
	Select apd.ID
, apd.POID
, apd.Seq1
, Amount = sum(apd.Amount)
From Surcharge_Detail apd
where id = @APID
Group by apd.ID, apd.POID, apd.Seq1
)
Update lad set Amount = apd.Amount
OutPut la.ID into @LCUpdateList
From LC_Application_Detail lad
Left join LC_Application la on lad.ID = la.ID
inner join apd on lad.SourceID = apd.ID and lad.POID = apd.POID and lad.Seq1 = apd.Seq1
Where apd.Amount != lad.Amount
and la.Status = 'New'";
            }

            sql +=

@"Update LC_Application Set TotalAmount = Source.Amount
From (
	Select la.ID, Amount = sum(lad.Amount)
	From LC_Application la
	Left join LC_Application_Detail lad on la.ID = lad.ID
	Where la.ID in (select LCID from @LCUpdateList)
	Group by la.ID
) Source
Where LC_Application.ID = source.ID
";
            var result = SQL.Execute(conn, sql, paras);
            if (!result)
            {
                return new DualResult(false, "Update LC detail fail.");
            }

            return Result.True;
        }

        /// <summary>
        /// 收單紀錄
        /// </summary>
        /// <param name="apID">apID</param>
        public static void ShowScanHis(string apID)
        {
            DataTable dt = new DataTable();
            string sqlCmd = @"
SELECT [Scan Date]=ScanDate
        ,[Scan Handle] = ScanHandle.IdAndNameAndExt
        ,[Receive Handle]=ReceiveHandle.IdAndNameAndExt
        --,ReturnHandle.IdAndNameAndExt
        ,[Return Date]=ReturnDate
        ,[Return Reason] =Reason.Name
        ,[Return Remark]=ReturnRemark
	    ,[Modify Request] = iif(Isnull(ModifyReq.NameAndExt, '') = '', '', Concat(ModifyReq.NameAndExt, '-', Convert(varchar(20), ModifyReqDate, 120)))
FROM AP_Scan
LEFT JOIN Reason on AP_Scan.ReturnReason = Reason.ID and ReasonTypeID = 'AP_Return'
Left Join GetName ScanHandle on ScanHandle.ID = AP_Scan.ScanHandle
Left Join GetName ReceiveHandle on ReceiveHandle.ID = AP_Scan.ReceiveHandle
Left Join GetName ReturnHandle on ReturnHandle.ID = AP_Scan.ReturnHandle
Left Join GetName ModifyReq on ModifyReq.ID = AP_Scan.ModifyReqHandle
WHERE AP_Scan.ID =@ID";
            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter("@ID", apID));
            var result = SQL.Select(SQL.queryConn, sqlCmd, out dt, para);
            if (!result || dt.Rows.Count == 0)
            {
                Msg.WarningBox("No Data !");
                return;
            }
            else
            {
                Forms.JustGrid form = new Forms.JustGrid("收單紀錄", dt);
                form.Show();
            }
        }

        /// <summary>
        /// 修改通知/收單通知 寄信
        /// </summary>
        /// <param name="btnText">修改通知/收單通知</param>
        /// <param name="row">row</param>
        public static void ModifyReqSendMail(string btnText, DataRow row)
        {
            string mailFrom = Env.User.MailAddress;
            string mailTo = "Accounting@sportscity.com.tw";
            string mailCc = Env.User.MailAddress;
            string subject = string.Empty;
            string desc = string.Empty;

            string apID = row["ID"].ToString();
            string suppID = row["SuppID"].ToString();
            string abbCH = SuppPrg.GetSupp(suppID).AbbCH;

            if (btnText == "修改通知")
            {
                subject = $"修改通知-請退件-{suppID}-{abbCH}-{apID}";
                desc = $"[{apID}]需修改資料, 今日下班前請協助退件, 謝謝";

                using (var dlg = new MailTo(mailFrom, mailTo, mailCc, subject, string.Empty, desc, false, true))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (!dlg.SendMailResult)
                        {
                            Msg.ErrorBox(dlg.SendMailResult.ToString());
                            return;
                        }
                        else
                        {
                            string sqlCmd = @"
update scan set ModifyReqHandle = @UserID, ModifyReqDate = GETDATE()
from AP_Scan scan with(nolock)
INNER JOIN
(
    SELECT TOP 1 Ukey FROM AP_Scan WITH (NOLOCK) WHERE ID = @ID	ORDER BY ScanDate DESC
)chk on scan.Ukey = chk.Ukey
";
                            List<SqlParameter> paras = new List<SqlParameter>();
                            paras.Add(new SqlParameter("ID", apID));
                            paras.Add(new SqlParameter("UserID", Env.User.UserID));

                            DualResult result;
                            if (!(result = DBProxy.Current.Execute(null, sqlCmd, paras)))
                            {
                                MyUtility.Msg.ErrorBox(result.ToString());
                                return;
                            }
                        }

                        Msg.InfoBox("Send mail sucess!!", "Information");
                    }
                }
            }

            if (btnText == "收單通知")
            {
                subject = $"收單通知-{suppID}-{abbCH}-{apID}-請款金額不變,未重印帳單";
                desc = $"[{apID}]請款金額不變, 沒有重新列印帳單, 請確認收單, 謝謝";

                // 抓最後一筆收單紀錄的退件人員
                var returnHandle = DBProxy.Current.LookupEx<string>("Select ReturnHandle From AP_Scan Where ID = @ID Order By ScanDate Desc", "ID", apID).ExtendedData;
                if (!VFP.Empty(returnHandle))
                {
                    mailTo = UserPrg.GetUser(returnHandle).email;
                }

                using (var dlg = new MailTo(mailFrom, mailTo, mailCc, subject, string.Empty, desc, false, true))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (!dlg.SendMailResult)
                        {
                            Msg.ErrorBox(dlg.SendMailResult.ToString());
                            return;
                        }

                        Msg.InfoBox("Send mail sucess!!", "Information");
                    }
                }
            }
        }

        /// <summary>
        /// 取得收單狀態
        /// </summary>
        /// <param name="apID">apID</param>
        /// <returns>Status</returns>
        public static string GetModifyReqStatus(string apID)
        {
            string status = string.Empty;

            DataTable dt = DBProxy.Current.SelectEx($"Select Top 1 * From AP_Scan Where ID = @ID Order By ScanDate Desc", "ID", apID).ExtendedData;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (VFP.Empty(dt.Rows[0]["ReturnDate"]))
                {
                    status = Label_Received;
                }
                else
                {
                    status = Label_Return;
                }
            }

            return status;
        }

        /// <summary>
        /// 檢查InvoiceDate不可在請款日期(create date) 前2年或往後3個月內
        /// </summary>
        /// <param name="data">要檢查的資料</param>
        /// <param name="apID">請款單號</param>
        /// <param name="tableName">請款單在DB中的TableName</param>
        /// <returns>True:Pass; False:Fail</returns>
        public static bool CheckInvoiceDate(IEnumerable<DataRow> data, string apID, string tableName)
        {
            var checkFieldName = tableName.EqualString("LC_Application") ? "PIDate" : "InvoiceDate";

            var sql = $@"Select AddDate From {tableName} Where ID = @ID";
            var result = DBProxy.Current.LookupEx<DateTime>(sql, "ID", apID);
            var addDate = result.ExtendedData;

            if (data.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted).Any(x => MyUtility.Convert.GetDate(x[checkFieldName]) < addDate.AddYears(-2) || MyUtility.Convert.GetDate(x[checkFieldName]) > addDate.AddMonths(3)))
            {
                MyUtility.Msg.WarningBox($"[{checkFieldName}] Shoud be between {addDate.AddYears(-2).ToString("yyyy/MM/dd")} and {addDate.AddMonths(3).ToString("yyyy/MM/dd")}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查OrderCompany與Company是否相同
        /// </summary>
        /// <param name="data">要檢查的資料</param>
        /// <returns>True:Pass; False:Fail</returns>
        public static bool CheckCompany(IEnumerable<DataRow> data)
        {
            DataRow firstRow = data.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted).FirstOrDefault<DataRow>();
            string companyID = firstRow["CompanyID"].ToString();
            string orderCompany = firstRow["OrderCompany"].ToString();
            if (data.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted
            && x["CompanyID"].ToString() == companyID
            && x["OrderCompany"].ToString() == orderCompany).ToList().Count != data.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted).ToList().Count)
            {
                MyUtility.Msg.WarningBox("<Order Company> and <Company> has different setting! Please check!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 若已經有明細資料，檢查OrderCompany與Company是否相同
        /// </summary>
        /// <param name="data">要檢查的資料</param>
        /// <param name="apID">請款單號</param>
        /// <param name="tableName">請款單在DB中的TableName</param>
        /// <returns>True:Pass; False:Fail</returns>
        public static bool CheckAPCompany(IEnumerable<DataRow> data, string apID, string tableName)
        {
            string sql = $"select M.* from {tableName} M join {tableName}_Detail D on M.ID = D.ID where M.ID = @ID";
            var result = DBProxy.Current.SeekEx(sql, "ID", apID);
            var dr = result.ExtendedData;
            if (dr != null)
            {
                if (data.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted).Any(x => x["CompanyID"].ToString() != dr["CompanyID"].ToString() || x["OrderCompany"].ToString() != dr["OrderCompany"].ToString()))
                {
                    MyUtility.Msg.WarningBox("<Order Company> and <Company> is different from AP!");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 檢查InvoiceDate不可在請款日期(create date) 前2年或往後3個月內
        /// </summary>
        /// <param name="invoiceDate">Invoice Date</param>
        /// <param name="apID">請款單號</param>
        /// <param name="tableName">請款單在DB中的TableName</param>
        /// <returns>True:Pass; False:Fail</returns>
        public static bool CheckInvoiceDate(DateTime invoiceDate, string apID, string tableName)
        {
            var checkFieldName = tableName.EqualString("LC_Application") ? "PIDate" : "InvoiceDate";

            var sql = $@"Select AddDate From {tableName} Where ID = @ID";
            var result = DBProxy.Current.LookupEx<DateTime>(sql, "ID", apID);
            var addDate = result.ExtendedData;

            if (invoiceDate < addDate.AddYears(-2) || invoiceDate > addDate.AddMonths(3))
            {
                MyUtility.Msg.WarningBox($"[{checkFieldName}] Shoud be between {addDate.AddYears(-2).ToString("yyyy/MM/dd")} and {addDate.AddMonths(3).ToString("yyyy/MM/dd")}");
                return false;
            }

            return true;
        }

        public static decimal GetSameWeekAmount(string apID)
        {
            return DBProxy.Current.LookupEx<decimal?>(
                @"
select sum(GetSameWeekData.Amount_USD) Amt
from TTBefore tt
Left join pass1 handleName on handleName.ID = tt.HANDLE
Left join pass1 smrName on smrName.ID = handleName.Supervisor
outer apply (
	select WeekStart = DATEADD(dd, -(DATEPART(dw, tt.EstAPdate)-1), tt.EstAPdate)
		, WeekEnd = DATEADD(dd, 7-(DATEPART(dw, tt.EstAPdate)), tt.EstAPdate)
) sameWeek
Cross apply (
	select tt1.ID, Amount_USD = Round(tt1.Amount * Cur.Rate, 2)
	from TTBefore tt1
	Left join pass1 on pass1.ID = tt1.HANDLE
	Left join pass1 smr on smr.ID = pass1.Supervisor
	outer apply dbo.GetCurrencyRate('FX', tt1.CurrencyID, 'USD', tt1.EstAPdate) Cur
	where tt1.SUPPID = tt.SUPPID
	and tt1.Payterm = tt.Payterm
	and tt1.EstAPdate between sameWeek.WeekStart and sameWeek.WeekEnd
	and smr.DepartmentID = smrName.DepartmentID
	and tt1.OrderCompany = tt.OrderCompany
	and tt1.Status <> 'Junked'
) GetSameWeekData
where tt.ID = @ID",
                "ID",
                apID).ExtendedData.GetValueOrDefault(0);
        }

        /// <summary>
        /// 檢查Surcharge是否有TT Before的共用憑證
        /// </summary>
        /// <param name="apID">apID</param>
        /// <returns>bool</returns>
        public static bool CheckSameInvoiceNo(string apID)
        {
            var sqlcmd = @"
select distinct t.ID
from Surcharge s
inner join Surcharge_Detail sd on s.ID = sd.ID
inner join TTBefore_Detail td on sd.InvoiceNo = td.InvoiceNO and sd.InvoiceDate = td.InvoiceDate
inner join TTBefore t on td.ID = t.ID and s.SuppID = t.SuppID
where s.ID = @ID";
            return DBProxy.Current.SeekEx(sqlcmd, "ID", apID).ExtendedData != null;
        }

        public static DualDisposableResult<DataTable> GetPrePaidData(string apID)
        {
            DataTable tmp;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", apID));
            #region sqlCmd
            string sqlCmd = @"
Declare @LocalID varchar(13) = @ID

Select Distinct APID = AP.ID
    , APAmount = AP.Amount + getTax.APTax
    , NPID = NP.ID
    , NPAmount = NP.Amount
    , NP.AddDate
    , NP.Status
From AP
Inner join NP on NP.APID = AP.ID
Left join Supp s on s.ID = AP.SuppID
Left join Currency c on c.ID = s.CurrencyID
Left join Company_Tax ct on ct.ID = s.CompanyID And AP.CDate Between ct.StartDate And ct.EndDate
Outer apply (
    Select APTax = Round(ct.TaxRate * AP.Amount / 100, c.Exact)
) getTax
Where NP.Status != 'Junked' and AP.ID = @LocalID
Union all
Select Distinct APID = sur.ID
    , APAmount = sur.Amount + getTax.APTax
    , NPID = NP.ID
    , NPAmount = NP.Amount
    , NP.AddDate
    , NP.Status
From Surcharge sur
Inner join NP on NP.APID = sur.ID
Left join Supp s on s.ID = sur.SuppID
Left join Currency c on c.ID = s.CurrencyID
Left join Company_Tax ct on ct.ID = s.CompanyID And sur.CDate Between ct.StartDate And ct.EndDate
Outer apply (
    Select APTax = Round(ct.TaxRate * sur.Amount / 100, c.Exact)
) getTax
Where NP.Status != 'Junked' and sur.ID = @LocalID
";
            #endregion

            var result = DBProxy.Current.SelectEx(sqlCmd, "ID", apID);

            return result;
        }

        public static DataRow GetLCApvName(string apID)
        {
            var sql = @"
Select Top 1 SMRConfirmName = Isnull(lc.ApproveName, '')
	, SMRConfirmDate = lc.ApproveDate
	, LeaderConfirmName = Isnull(lc.MgApproveName, '')
	, LeaderConfirmDate = lc.MgApproveDate
From TTBefore_Detail ttd
Left join LC_Application_Detail lcad on lcad.ID = ttd.SourceID
Left join LC_Request_Detail lcrd on lcrd.ID = lcad.SourceID And lcad.POID = lcrd.POID And lcad.Seq1 = lcrd.Seq1 And lcad.Seq2 = lcrd.Seq2
Left join LC_Request lc on lc.ID = lcrd.ID
Where ttd.ID = @ID 
    And (Isnull(lc.MgApproveName, '') <> '' Or Isnull(lc.ApproveName, '') <> '')
Order by lc.ID
";
            return DBProxy.Current.SeekEx(sql, "ID", apID).ExtendedData;
        }

        public static string SetAccRemark(DataRow row)
        {
            List<string> list = new List<string>();
            var qtyOld = 0m;
            var qtyNew = 0m;
            if (row.Table.Columns.Contains("Qty"))
            {
                qtyOld = SciConvert.GetDecimal(row["Qty", DataRowVersion.Original]);
                qtyNew = SciConvert.GetDecimal(row["Qty"]);
            }

            var amountOld = SciConvert.GetDecimal(row["Amount", DataRowVersion.Original]);
            var amountNew = SciConvert.GetDecimal(row["Amount"]);
            var invoiceNoOld = row["InvoiceNo", DataRowVersion.Original].ToString();
            var invoiceNoNew = row["InvoiceNo"].ToString();
            var invoiceDateOld = SciConvert.GetDate(row["InvoiceDate", DataRowVersion.Original]).ToStringEx("yyyy/MM/dd");
            var invoiceDateNew = SciConvert.GetDate(row["InvoiceDate"]).ToStringEx("yyyy/MM/dd");
            var accRemarkOld = row["AccRemark"].ToString();

            if (qtyOld != qtyNew)
            {
                list.Add("Qty(Original):" + qtyOld);
            }

            if (amountOld != amountNew)
            {
                list.Add("Amount(Original):" + amountOld);
            }

            if (invoiceNoOld != invoiceNoNew)
            {
                list.Add("發票號碼(Original):" + invoiceNoOld);
            }

            if (invoiceDateOld != invoiceDateNew)
            {
                list.Add("發票日期(Original):" + invoiceDateOld);
            }

            if (list.Count == 0)
            {
                return accRemarkOld;
            }

            return string.IsNullOrWhiteSpace(accRemarkOld)
                ? string.Join(";", list)
                : accRemarkOld + ", " + string.Join(";", list);
        }
    }

    /// <inheritdoc />
    public class ExpenseType
    {
        static ExpenseType()
        {
            Load();
        }

        /// <inheritdoc />
        public static Dictionary<string, ExpenseType> types = new Dictionary<string, ExpenseType>(StringComparer.OrdinalIgnoreCase);

        /// <inheritdoc />
        public static ExpenseType Type_Freight;

        /// <inheritdoc />
        public static ExpenseType Type_Insure;

        /// <inheritdoc />
        public static ExpenseType Type_Truck;

        /// <inheritdoc />
        public static ExpenseType Type_Broker;

        /// <inheritdoc />
        public static ExpenseType Type_FactoryExpress;

        /// <inheritdoc />
        public static void Load()
        {
            DataTable data;
            if (!SQL.Select(string.Empty, "select * from dbo.ExpenseType", out data))
            {
                return;
            }

            foreach (DataRow row in data.Rows)
            {
                string typeid = row["ID"].ToString().TrimEnd();
                types.Add(typeid, new ExpenseType(row));
            }

            Type_Freight = types["01"];
            Type_Insure = types["02"];
            Type_Truck = types["03"];
            Type_Broker = types["04"];
            Type_FactoryExpress = types["05"];
        }

        /// <inheritdoc />
        public string ID = null;

        /// <inheritdoc />
        public string AccountID = null;

        /// <inheritdoc />
        public string AccountID2 = null;

        /// <inheritdoc />
        public string SourceType = null;

        /// <inheritdoc />
        public bool isFreight = false;

        /// <inheritdoc />
        public string CombineID = null;

        /// <inheritdoc />
        public string CheckField1 = null;

        /// <inheritdoc />
        public string CheckField2 = null;

        /// <inheritdoc />
        public bool isDefault = false;

        /// <inheritdoc />
        public string WriteBackField = null;

        /// <inheritdoc />
        public ExpenseType(DataRow row)
        {
            this.ID = row["ID"].ToString().TrimEnd();
            this.AccountID = row["AccountID"].ToString().TrimEnd();
            this.AccountID2 = row["AccountID2"].ToString().TrimEnd();
            this.SourceType = row["SourceType"].ToString().TrimEnd();
            this.isFreight = !Check.Empty(row["isFreight"]);
            this.CombineID = row["CombineID"].ToString().TrimEnd();
            this.CheckField1 = row["CheckField1"].ToString().TrimEnd();
            this.CheckField2 = row["CheckField2"].ToString().TrimEnd();
            this.isDefault = !Check.Empty(row["isDefault"]);
            this.WriteBackField = row["WriteBackField"].ToString().TrimEnd();
        }

        /// <inheritdoc />
        public ExpenseType()
        {
        }
    }

    /// <summary>
    /// 搭配 dbo.CheckInvoiceDuplicate(@ID) , dbo.CheckInvoiceDuplicate_TTBefore(@ID)
    /// 將DataRow轉 class
    /// </summary>
    public class DupInvoice
    {
        /// <inheritdoc />
        public string invoiceID;

        /// <inheritdoc />
        public bool dupInvoiceDate;

        /// <inheritdoc />
        public bool dupInvoiceNo;

        /// <inheritdoc />
        public DupInvoice(DataRow row)
        {
            this.invoiceID = row["InvoiceNo"].ToString().TrimEnd();
            this.dupInvoiceDate = Check.isTrue(row["dupInvDate"]);
            this.dupInvoiceNo = Check.isTrue(row["dupInvNo"]);
        }
    }

    public class AccRemarkSetting
    {
        public decimal QtyOld { get; set; } = 0m;

        public decimal QtyNew { get; set; } = 0m;

        public decimal AmountOld { get; set; } = 0m;

        public decimal AmountNew { get; set; } = 0m;

        public string InvoiceNoOld { get; set; } = string.Empty;

        public string invoiceNoNew { get; set; } = string.Empty;

        public string InvoiceDateOld { get; set; } = string.Empty;

        public string InvoiceDateNew { get; set; } = string.Empty;

        public string AccRemarkOld { get; set; } = string.Empty;

        /// <summary>
        /// 建構式
        /// </summary>
        public AccRemarkSetting()
        {

        }

        /// <summary>
        /// 初始設定
        /// </summary>
        /// <param name="row">DataRow</param>
        public AccRemarkSetting(DataRow row)
        {
            this.QtyOld = SciConvert.GetDecimal(row["Qty", DataRowVersion.Original]);
            this.QtyNew = SciConvert.GetDecimal(row["Qty"]);
            this.AmountOld = SciConvert.GetDecimal(row["Amount", DataRowVersion.Original]);
            this.AmountNew = SciConvert.GetDecimal(row["Amount"]);
            this.InvoiceNoOld = row["InvoiceNo", DataRowVersion.Original].ToString();
            this.invoiceNoNew = row["InvoiceNo"].ToString();
            this.InvoiceDateOld = SciConvert.GetDate(row["InvoiceDate", DataRowVersion.Original]).ToStringEx("yyyy/MM/dd");
            this.InvoiceDateNew = SciConvert.GetDate(row["InvoiceDate"]).ToStringEx("yyyy/MM/dd");
            this.AccRemarkOld = row["AccRemark"].ToString();
        }

        public static string SetAccRemark(DataRow row)
        {
            var setting = new AccRemarkSetting(row);
            return SetAccRemark(setting);
        }

        public static string SetAccRemark(AccRemarkSetting setting)
        {
            List<string> list = new List<string>();

            if (setting.QtyOld != setting.QtyNew)
            {
                list.Add("Qty(Original):" + setting.QtyOld);
            }

            if (setting.AmountOld != setting.AmountNew)
            {
                list.Add("Amount(Original):" + setting.AmountOld);
            }

            if (setting.InvoiceNoOld != setting.invoiceNoNew)
            {
                list.Add("發票號碼(Original):" + setting.InvoiceNoOld);
            }

            if (setting.InvoiceDateOld != setting.InvoiceDateNew)
            {
                list.Add("發票日期(Original):" + setting.InvoiceDateOld);
            }

            if (list.Count == 0)
            {
                return string.Empty;
            }

            return string.IsNullOrWhiteSpace(setting.AccRemarkOld)
                ? string.Join(";", list)
                : setting.AccRemarkOld + ", " + string.Join(";", list);
        }
    }
}