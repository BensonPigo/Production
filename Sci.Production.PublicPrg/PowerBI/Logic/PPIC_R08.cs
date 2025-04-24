using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class PPIC_R08
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public PPIC_R08()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetPPIC_R08Data(PPIC_R08_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@CDate1", SqlDbType.DateTime) { Value = (object)model.CDate1 ?? DBNull.Value },
                new SqlParameter("@CDate2", SqlDbType.DateTime) { Value = (object)model.CDate2 ?? DBNull.Value },
                new SqlParameter("@ApvDate1", SqlDbType.DateTime) { Value = (object)model.ApvDate1 ?? DBNull.Value },
                new SqlParameter("@ApvDate2", SqlDbType.DateTime) { Value = (object)model.ApvDate2 ?? DBNull.Value },
                new SqlParameter("@Lockdate1", SqlDbType.DateTime) { Value = (object)model.Lockdate1 ?? DBNull.Value },
                new SqlParameter("@Lockdate2", SqlDbType.DateTime) { Value = (object)model.Lockdate2 ?? DBNull.Value },
                new SqlParameter("@Cfmdate1", SqlDbType.DateTime) { Value = (object)model.Cfmdate1 ?? DBNull.Value },
                new SqlParameter("@Cfmdate2", SqlDbType.DateTime) { Value = (object)model.Cfmdate2 ?? DBNull.Value },
                new SqlParameter("@Voucher1", SqlDbType.DateTime) { Value = (object)model.Voucher1 ?? DBNull.Value },
                new SqlParameter("@Voucher2", SqlDbType.DateTime) { Value = (object)model.Voucher2 ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.NVarChar) { Value = (object)model.MDivisionID ?? DBNull.Value },
                new SqlParameter("@FactoryID", SqlDbType.NVarChar) { Value = (object)model.FactoryID ?? DBNull.Value },
                new SqlParameter("@T", SqlDbType.NVarChar) { Value = (object)model.T ?? DBNull.Value },
                new SqlParameter("@Status", SqlDbType.NVarChar) { Value = (object)model.Status ?? DBNull.Value },
                new SqlParameter("@Sharedept", SqlDbType.NVarChar) { Value = (object)model.Sharedept ?? DBNull.Value },
                new SqlParameter("@ReportType", SqlDbType.NVarChar) { Value = (object)model.ReportType ?? DBNull.Value },
                new SqlParameter("@IncludeJunk", SqlDbType.Bit) { Value = model.IncludeJunk },
                new SqlParameter("@IsReplacementReport", SqlDbType.Bit) { Value = model.IsReplacementReport },
                new SqlParameter("@IsPowerBI", SqlDbType.Bit) { Value = model.IsPowerBI },
            };

            #region Where
            string where = string.Empty;
            string whereSummary = string.Empty;
            string whereDetail = string.Empty;

            if (!MyUtility.Check.Empty(model.CDate1))
            {
                where += $@"and rr.CDate >= @CDate1 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.CDate2))
            {
                where += $@"and rr.CDate <= @CDate2 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.ApvDate1))
            {
                where += $@"and rr.ApvDate >= @ApvDate1 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.ApvDate2))
            {
                where += $@"and rr.ApvDate <= @ApvDate2 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Lockdate1))
            {
                where += $@"and rr.LockDate >= @Lockdate1 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Lockdate2))
            {
                where += $@"and rr.LockDate <= @Lockdate2 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Cfmdate1))
            {
                where += $@"and cast(rr.RespDeptConfirmDate as date) >= @Cfmdate1 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Cfmdate2))
            {
                where += $@"and cast(rr.RespDeptConfirmDate as date) <= @Cfmdate2 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Voucher1))
            {
                where += $@"and cast(rr.VoucherDate as date) >= @Voucher1 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Voucher2))
            {
                where += $@"and cast(rr.VoucherDate as date) <= @Voucher2 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                where += $@"and rr.MDivisionID = @MDivisionID " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                where += $@"and rr.FactoryID = @FactoryID " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.T))
            {
                where += $@"and rr.Type = @T " + Environment.NewLine;
            }

            if (model.Status != "ALL")
            {
                where += $@"and rr.Status = @Status " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.Sharedept))
            {
                where += $@"and exists(select 1 from ICR_ResponsibilityDept icr with(nolock) where icr.ID = rr.id and icr.DepartmentID =  @Sharedept) " + Environment.NewLine;
            }

            if (!model.IncludeJunk)
            {
                whereSummary += "and exists (select 1 from ReplacementReport_Detail where Junk = 0 and rr.ID = ID)" + Environment.NewLine;
                whereDetail += "and rrd.Junk = 0" + Environment.NewLine;
            }

            if (model.IsReplacementReport)
            {
                whereSummary += "and rr.Status != 'Auto.Lock' and exists (select 1 from ReplacementReport_Detail where Junk = 0 and rr.ID = ID)" + Environment.NewLine;
                whereDetail += "and rr.Status != 'Auto.Lock' and rrd.Junk = 0" + Environment.NewLine;
            }

            #endregion

            #region sqlcmd 主Table
            StringBuilder sqlCmd = new StringBuilder();
            if (model.ReportType == "Detail List")
            {
                sqlCmd.Append($@"
select
	rr.id,
	Type=IIF(rr.Type = 'F', 'Fabric', 'Accessory'),
	rr.MDivisionID,
	rr.FactoryID,
	rr.POID,
	Style = (select ID from Style s where s.Ukey = o.StyleUkey),
	o.BrandID,
	o.SeasonID,
	rr.Status,
	rr.CDate,
	rr.ApvDate,
	rr.LockDate,
	rr.Responsibility,
	x.ttlestamt,
	rr.RMtlAmt,
	rr.ActFreight,
	rr.EstFreight,
	rr.SurchargeAmt,
    TTLUS = isnull(rr.RMtlAmt,0) + isnull(rr.ActFreight,0) +isnull(rr.EstFreight,0) + isnull(rr.SurchargeAmt,0),
    POHandle = [dbo].[getTPEPass1_ExtNo](PO.POHandle),
    PCSMR = [dbo].[getTPEPass1_ExtNo](PO.PCSMR),
	rr.TransferResponsible,
	rr.TransferNo
{(model.IsPowerBI ? ", [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), [BIInsertDate] = GETDATE() " : string.Empty)}
from ReplacementReport rr WITH (NOLOCK) 
left join Orders o WITH (NOLOCK)  on o.ID = rr.POID
left join PO with(nolock) on PO.ID = rr.POID
outer apply(
	select ttlestamt = SUM(EstReplacementAMT)
	from (
		select EstReplacementAMT = case when rrd.Junk =1 then 0
						else (select top 1 amount from dbo.GetAmountByUnit(po_price.v, x.Qty, psd.POUnit, 4)) * isnull(dbo.getRate('KP', po_stock.v, 'USD', rr.CDate),1)
						end
		from ReplacementReport_Detail rrd with(nolock)
		left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = rr.POID and psd.SEQ1 = rrd.Seq1 and psd.SEQ2 = rrd.Seq2
		left join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
		left join Supp WITH (NOLOCK) on Supp.ID = ps.SuppID
        outer apply (
            select v = case 
						    when psd.seq1 like '7%' then isnull((select v = stock.Price
	                                                             from PO_Supp_Detail stock
	                                                             where	psd.SEQ1 like '7%'
			                                                            and psd.StockPOID = stock.ID
			                                                            and psd.StockSeq1 = stock.SEQ1
			                                                            and psd.StockSeq2 = stock.SEQ2), 0)
						    else psd.Price
					    end
        ) po_price
        outer apply (
            select Qty = iif (rr.Type = 'A', rrd.TotalRequest, rrd.FinalNeedQty)
                         * isnull ((select RateValue = IIF(Denominator = 0,0, Numerator / Denominator) 
                                    from Unit_Rate 
                                    where UnitFrom = rrd.ReplacementUnit 
                                          and UnitTo = psd.POUnit),1)
        )x
        outer apply (
            select v = case 
					        when psd.seq1 like '7%' then isnull((select v = sstock.Currencyid
	                                                            from PO_Supp_Detail stock WITH (NOLOCK)
														        left join PO_Supp pstock WITH (NOLOCK) on pstock.ID = stock.ID and pstock.SEQ1 = stock.SEQ1
														        left join Supp sstock WITH (NOLOCK) on sstock.ID = pstock.SuppID
	                                                            where	psd.SEQ1 like '7%'
			                                                        and psd.StockPOID = stock.ID
			                                                        and psd.StockSeq1 = stock.SEQ1
			                                                        and psd.StockSeq2 = stock.SEQ2), 0)
					        else Supp.Currencyid
				        end
        ) po_stock
		where  rrd.ID = rr.ID
	)x
)x
where 1=1
{where}
{whereSummary}

select
	rr.ID,
	Type = IIF(rr.Type = 'F', 'Fabric', 'Accessory'),
	M = (Select MDivisionID from Factory with(nolock) where ID = rr.FactoryID),
    rr.FactoryID,
	rr.POID,
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	rr.Status,
	rr.CDate,
	rr.ApvDate,
	rr.CompleteDate,
	rr.LockDate,
	Responsibility = (select Name from DropDownList dd with(nolock) where dd.ID = rr.Responsibility and dd.Type = 'Replacement.R'),
    ReplacementReason = isnull((select Reason.Name from Reason where rrd.AfterCutting = Reason.ID and Reason.ReasonTypeID = 'Damage Reason'),''),
	POSMR = [dbo].[getTPEPass1_ExtNo](PO.POSMR),
	POHandle = [dbo].[getTPEPass1_ExtNo](PO.POHandle),
	PCSMR = [dbo].[getTPEPass1_ExtNo](PO.PCSMR),
	PCHandle = [dbo].[getTPEPass1_ExtNo](PO.PCHandle),
	Prepared = [dbo].[getPass1_ExtNo](rr.ApplyName),
	PPICFactorymgr = [dbo].[getPass1_ExtNo](rr.ApvName),
	rr.VoucherID,
	rr.VoucherDate,
	Junk=iif(rrd.Junk=1,'Y',''),
	Seq = iif(isnull(rrd.Seq1,'') = '','',CONCAT(rrd.Seq1,'-',rrd.Seq2)),
	f.MtlTypeID,
	rrd.Refno,
	f.DescDetail,
	rrd.ColorID,
	rrd.EstInQty,
	rrd.ActInQty,
	FinalNeedQty =IIF(rr.Type = 'F', rrd.FinalNeedQty,  rrd.TotalRequest),
    [Unit] = rrd.ReplacementUnit,
	rrd.TotalRequest,
	rrd.AfterCuttingRequest,
    EstReplacementAMT = case when rrd.Junk =1 then 0
						else (select top 1 amount from dbo.GetAmountByUnit(po_price.v, x.Qty, psd.POUnit, 4)) * isnull(dbo.getRate('KP', po_stock.v, 'USD', rr.CDate),1)
                        end,
    psd.POAmt,
    psd.ShipAmt,
	rrd.ResponsibilityReason,
	rrd.Suggested,
	rrd.PurchaseID,
	NewSeq =iif(isnull(rrd.NewSeq1,'') = '','', CONCAT(rrd.NewSeq1,'-',rrd.NewSeq2))
    {(model.IsPowerBI ? ", [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), [BIInsertDate] = GETDATE() " : string.Empty)}
from ReplacementReport rr with(nolock)
inner join Orders o with(nolock) on o.ID = rr.POID
left join ReplacementReport_Detail rrd with(nolock) on rrd.ID = rr.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = rr.POID and psd.SEQ1 = rrd.Seq1 and psd.SEQ2 = rrd.Seq2
outer apply (
    select v = case 
					when psd.seq1 like '7%' then isnull((select v = stock.Price
	                                                    from PO_Supp_Detail stock
	                                                    where	psd.SEQ1 like '7%'
			                                                and psd.StockPOID = stock.ID
			                                                and psd.StockSeq1 = stock.SEQ1
			                                                and psd.StockSeq2 = stock.SEQ2), 0)
					else psd.Price
				end
) po_price
left join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
left join Supp WITH (NOLOCK) on Supp.ID = ps.SuppID
left join PO with(nolock) on PO.ID = rr.POID
left join Fabric f with(nolock) on f.SCIRefno = rrd.SCIRefno
outer apply (
    select Qty = iif (rr.Type = 'A', rrd.TotalRequest, rrd.FinalNeedQty)
                    * isnull ((select RateValue = IIF(Denominator = 0,0, Numerator / Denominator) 
                            from Unit_Rate 
                            where UnitFrom = rrd.ReplacementUnit 
                                    and UnitTo = psd.POUnit),1)
)x
outer apply (
    select v = case 
			        when psd.seq1 like '7%' then isnull((select v = sstock.Currencyid
                                                        from PO_Supp_Detail stock WITH (NOLOCK)
												        left join PO_Supp pstock WITH (NOLOCK) on pstock.ID = stock.ID and pstock.SEQ1 = stock.SEQ1
												        left join Supp sstock WITH (NOLOCK) on sstock.ID = pstock.SuppID
                                                        where	psd.SEQ1 like '7%'
	                                                        and psd.StockPOID = stock.ID
	                                                        and psd.StockSeq1 = stock.SEQ1
	                                                        and psd.StockSeq2 = stock.SEQ2), 0)
			        else Supp.Currencyid
		        end
) po_stock
where 1=1
{where}
{whereDetail}
");
            }
            else
            {
                sqlCmd.Append($@"
select
    [ID] = isnull(rr.ID,''),
    [Type] = IIF(rr.Type = 'F', 'Fabric', 'Accessory'),
    [MDivisionID] = (Select MDivisionID from Factory with(nolock) where ID = rr.FactoryID),
    [FactoryID] = rr.FactoryID,
    [SPNo] = rr.POID,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Status] = rr.Status,
    [Cdate] = rr.CDate,
    [FtyApvDate] = rr.ApvDate,
    [CompleteDate] = rr.CompleteDate,
    [LockDate] = rr.LockDate,
    [Responsibility] = isnull((select Name from DropDownList dd with(nolock) where dd.ID = rr.Responsibility and dd.Type = 'Replacement.R'),''),
	[TtlEstReplacementAMT] = isnull(x.ttlestamt,0),
    [RMtlUS] = isnull(rr.RMtlAmt,0),
    [ActFreightUS] = isnull(rr.ActFreight,0),
    [EstFreightUS] = isnull(rr.EstFreight,0),
    [SurchargeUS] = isnull(rr.SurchargeAmt,0),
    [TotalUS] = isnull(rr.RMtlAmt,0) + isnull(rr.ActFreight,0) +isnull(rr.EstFreight,0) + isnull(rr.SurchargeAmt,0),
    [ResponsibilityFty] = isnull(icr.FactoryID,''),
    [ResponsibilityDept] = isnull(icr.DepartmentID,''),
    [ResponsibilityPercent] = isnull(icr.Percentage,0),
    [ShareAmount] = isnull(icr.Amount,0),
    [VoucherNo] = isnull(rr.VoucherID,''),
    [VoucherDate] = rr.VoucherDate,
    POSMR = isnull([dbo].[getTPEPass1_ExtNo](PO.POSMR),''),
    POHandle = isnull([dbo].[getTPEPass1_ExtNo](PO.POHandle),''),
    PCSMR = isnull([dbo].[getTPEPass1_ExtNo](PO.PCSMR),''),
    PCHandle = isnull([dbo].[getTPEPass1_ExtNo](PO.PCHandle),''),
    Prepared = isnull([dbo].[getPass1_ExtNo](rr.ApplyName),''),
    [PPIC/Factory mgr] = isnull([dbo].[getPass1_ExtNo](rr.ApvName),'')
{(model.IsPowerBI ? ", [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), [BIInsertDate] = GETDATE() " : string.Empty)}
from ReplacementReport rr with(nolock)
inner join Orders o with(nolock) on o.ID = rr.POID
left join PO with(nolock) on PO.ID = rr.POID
left join  ICR_ResponsibilityDept icr with(nolock) on icr.ID = rr.ID
outer apply(
	select ttlestamt = SUM(EstReplacementAMT)
	from (
		select EstReplacementAMT = case when rrd.Junk =1 then 0
						else (select top 1 amount from dbo.GetAmountByUnit(po_price.v, x.Qty, psd.POUnit, 4)) * isnull(dbo.getRate('KP', po_stock.v, 'USD', rr.CDate),1)
						end
		from ReplacementReport_Detail rrd with(nolock)
		left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = rr.POID and psd.SEQ1 = rrd.Seq1 and psd.SEQ2 = rrd.Seq2
        outer apply (
            select v = case 
						    when psd.seq1 like '7%' then isnull((select v = stock.Price
	                                                             from PO_Supp_Detail stock
	                                                             where	psd.SEQ1 like '7%'
			                                                            and psd.StockPOID = stock.ID
			                                                            and psd.StockSeq1 = stock.SEQ1
			                                                            and psd.StockSeq2 = stock.SEQ2), 0)
						    else psd.Price
					    end
        ) po_price
		left join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
		left join Supp WITH (NOLOCK) on Supp.ID = ps.SuppID
        outer apply (
            select Qty = iif (rr.Type = 'A', rrd.TotalRequest, rrd.FinalNeedQty)
                         * isnull ((select RateValue = IIF(Denominator = 0,0, Numerator / Denominator) 
                                    from Unit_Rate 
                                    where UnitFrom = rrd.ReplacementUnit 
                                          and UnitTo = psd.POUnit),1)
        )x
        outer apply (
            select v = case 
					        when psd.seq1 like '7%' then isnull((select v = sstock.Currencyid
	                                                            from PO_Supp_Detail stock WITH (NOLOCK)
														        left join PO_Supp pstock WITH (NOLOCK) on pstock.ID = stock.ID and pstock.SEQ1 = stock.SEQ1
														        left join Supp sstock WITH (NOLOCK) on sstock.ID = pstock.SuppID
	                                                            where	psd.SEQ1 like '7%'
			                                                        and psd.StockPOID = stock.ID
			                                                        and psd.StockSeq1 = stock.SEQ1
			                                                        and psd.StockSeq2 = stock.SEQ2), 0)
					        else Supp.Currencyid
				        end
        ) po_stock
		where  rrd.ID = rr.ID
	)x
)x
where 1=1
{where}
");
            }
            #endregion

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlCmd.ToString(), listPar, out DataTable[] dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTable;
            return resultReport;
        }
    }
}
