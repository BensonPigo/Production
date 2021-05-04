using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class R09 : Win.Tems.PrintForm
    {
        // string    mdivision, factory, orderby, spno1, spno2, refno1, refno2;
        private string mdivision;

        // string    mdivision, factory, orderby, spno1, spno2, refno1, refno2;
        private string factory;

        // string    mdivision, factory, orderby, spno1, spno2, refno1, refno2;
        private string spno1;

        // string    mdivision, factory, orderby, spno1, spno2, refno1, refno2;
        private string spno2;

        // string    mdivision, factory, orderby, spno1, spno2, refno1, refno2;
        private string refno1;

        // string    mdivision, factory, orderby, spno1, spno2, refno1, refno2;
        private string refno2;
        private DateTime? deadline1;
        private DateTime? deadline2;
        private DateTime? buyerDelivery1;
        private DateTime? buyerDelivery2;
        private DateTime? eta1;
        private DateTime? eta2;
        private DataTable printData;
        private int filterIndex;

        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            this.txtMdivision.Text = Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboFilterCondition, 1, 1, "Actual Inventory Qty < Taipei system,Actual Inventory Qty ≠ Taipei system,Inventory In  < Taipei InputQty,");
            this.comboFilterCondition.SelectedIndex = 0;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDeadLine.Value1) && MyUtility.Check.Empty(this.dateDeadLine.Value2) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateInventoryETA.Value1) && MyUtility.Check.Empty(this.dateInventoryETA.Value2) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Dead Line > & < Buyer Delivery > & < SP# > & < ETA > can't be empty!!");
                return false;
            }

            this.deadline1 = this.dateDeadLine.Value1;
            this.deadline2 = this.dateDeadLine.Value2;
            this.buyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.buyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            this.eta1 = this.dateInventoryETA.Value1;
            this.eta2 = this.dateInventoryETA.Value2;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.refno1 = this.txtRefnoStart.Text;
            this.refno2 = this.txtRefnoEnd.Text;
            this.filterIndex = this.comboFilterCondition.SelectedIndex;

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@spno1",
            };

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@spno2",
            };

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@MDivision",
            };

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@Factory",
            };

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@refno1",
            };

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@refno2",
            };

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();

            string whereStart = string.Empty;
            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                whereStart += "and" + string.Format(" '{0}' <= o.BuyerDelivery ", Convert.ToDateTime(this.buyerDelivery1).ToString("d"));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                whereStart += "and" + string.Format(" o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d"));
            }

            if (!MyUtility.Check.Empty(this.deadline1) || !MyUtility.Check.Empty(this.deadline2))
            {
                whereStart += " and exists(select 1 from Inventory inv with (nolock) where inv.POID = o.POID ";
                if (!MyUtility.Check.Empty(this.deadline1))
                {
                    whereStart += string.Format(@" and '{0}' <= inv.deadline", Convert.ToDateTime(this.deadline1).ToString("d"));
                }

                if (!MyUtility.Check.Empty(this.deadline2))
                {
                    whereStart += string.Format(@" and inv.deadline <= '{0}'", Convert.ToDateTime(this.deadline2).ToString("d"));
                }

                whereStart += ")";
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                whereStart += " and o.Poid >= @spno1 and o.Poid <= @spno2";
                sp_spno1.Value = this.spno1.PadRight(10, '0');
                sp_spno2.Value = this.spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(this.spno1))
            {
                // 只有 sp1 輸入資料
                whereStart += " and o.Poid like @spno1 ";
                sp_spno1.Value = this.spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(this.spno2))
            {
                // 只有 sp2 輸入資料
                whereStart += " and o.Poid like @spno2 ";
                sp_spno2.Value = this.spno2 + "%";
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                whereStart += " and f.mdivisionid = @MDivision";
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                whereStart += " and o.FactoryID = @Factory";
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            string whereFinal = string.Empty;
            if (!MyUtility.Check.Empty(this.eta1) || !MyUtility.Check.Empty(this.eta2))
            {
                if (!MyUtility.Check.Empty(this.eta1))
                {
                    whereFinal += string.Format(@" and '{0}' <= psd.ShipEta", Convert.ToDateTime(this.eta1).ToString("d"));
                }

                if (!MyUtility.Check.Empty(this.eta2))
                {
                    whereFinal += string.Format(@" and psd.ShipEta <= '{0}'", Convert.ToDateTime(this.eta2).ToString("d"));
                }
            }

            if (!MyUtility.Check.Empty(this.refno1) && !MyUtility.Check.Empty(this.refno2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                whereFinal += " and psd.refno >= @refno1 and psd.refno <= @refno2";
                sp_refno1.Value = this.refno1;
                sp_refno2.Value = this.refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(this.refno1))
            {
                // 只輸入 Refno1
                whereFinal += " and psd.refno like @refno1";
                sp_refno1.Value = this.refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(this.refno2))
            {
                // 只輸入 Refno2
                whereFinal += " and psd.refno like @refno2";
                sp_refno2.Value = this.refno2 + "%";
                cmds.Add(sp_refno2);
            }

            if (this.filterIndex == 0)
            {
                whereFinal += " and sl.FtyBalance  < sl.TPEBalance";
            }

            if (this.filterIndex == 1)
            {
                whereFinal += " and sl.FtyBalance  <> sl.TPEBalance";
            }

            if (this.filterIndex == 2)
            {
                whereFinal += " and mpd.InQty < Round(psd.InputQty * isnull(v.RateValue, 1), 2)";
            }

            sqlCmd.Append($@"
declare @stock table 
(
	POID varchar(13)
)

insert into @stock
select poid 
    from dbo.orders o WITH (NOLOCK) 
    inner join Factory f WITH (NOLOCK) on o.FactoryID = f.id
	where 1 = 1 {whereStart}
    group by POID

/*
	TPE Current Stock
*/
select	TPEInQty.FactoryID
		, TPEInQty.POID
		, TPEInQty.Seq1
		, TPEInQty.Seq2 
		, Qty = Sum(round(TPEInQty.Qty * v.RateValue, 2))
into #TPEIn
from (
	-- Type 1, 4 --
	select FactoryID = f.ID
			, POID = inv.InventoryPOID
			, Seq1 = inv.InventorySeq1
			, Seq2 = inv.InventorySeq2
			, inv.Type
			, inv.Qty
	from @stock s
	inner join Invtrans inv on s.POID = inv.InventoryPOID
	inner join Factory f on inv.FactoryID = f.ID
	where	( 
				inv.Type = 1 
				or (inv.Type = 4 and Qty > 0)
			)
			and f.IsProduceFty = 1
	union all

	-- Type 3 --
	select FactoryID = f.ID
			, POID = inv.InventoryPOID
			, Seq1 = inv.InventorySeq1
			, Seq2 = inv.InventorySeq2
			, inv.Type
			, inv.Qty
	from @stock s
	inner join Invtrans inv on s.POID = inv.InventoryPOID
	inner join Factory f on inv.TransferFactory = f.ID
	where	inv.Type = 3
			and f.IsProduceFty = 1
) TPEInQty
left join PO_Supp_Detail psd on TPEInQty.POID = psd.ID
								and TPEInQty.Seq1 = psd.SEQ1
								and TPEInQty.Seq2 = psd.SEQ2
outer apply (
	select RateValue = dbo.GetUnitRate(psd.POUnit, psd.StockUnit)
) v
group by TPEInQty.FactoryID, TPEInQty.POID, TPEInQty.Seq1, TPEInQty.Seq2 

select	TPEAllocatedQty.FactoryID
		, TPEAllocatedQty.POID
		, TPEAllocatedQty.Seq1
		, TPEAllocatedQty.Seq2 
		, Qty = Sum (round(TPEAllocatedQty.Qty * v.RateValue, 2))
into #TPEAllocated
from (
	-- Type 2, 3, 5 --
	select FactoryID = f.ID
			, POID = inv.InventoryPOID
			, Seq1 = inv.InventorySeq1
			, Seq2 = inv.InventorySeq2
			, inv.Type
			, inv.Qty
	from @stock s
	inner join Invtrans inv on s.POID = inv.InventoryPOID
	inner join Factory f on inv.FactoryID = f.ID
	where	inv.Type in ('2', '3', '5')
			and f.IsProduceFty = 1
	union all

	-- Type 4, 6 --
	select FactoryID = f.ID
			, POID = inv.InventoryPOID
			, Seq1 = inv.InventorySeq1
			, Seq2 = inv.InventorySeq2
			, inv.Type
			, Qty = 0 - inv.Qty
	from @stock s
	inner join Invtrans inv on s.POID = inv.InventoryPOID
	inner join Factory f on inv.FactoryID = f.ID
	where	(
				inv.Type = 6 
				or (inv.Type = 4 and Qty < 0)
			)
			and f.IsProduceFty = 1
) TPEAllocatedQty
left join PO_Supp_Detail psd on TPEAllocatedQty.POID = psd.ID
								and TPEAllocatedQty.Seq1 = psd.SEQ1
								and TPEAllocatedQty.Seq2 = psd.SEQ2
outer apply (
	select RateValue = dbo.GetUnitRate(psd.POUnit, psd.StockUnit)
) v
group by TPEAllocatedQty.FactoryID, TPEAllocatedQty.POID, TPEAllocatedQty.Seq1, TPEAllocatedQty.Seq2 

select FactoryID = iif (ti.FactoryID is not null, ti.FactoryID, ta.FactoryID)
		, POID = iif (ti.FactoryID is not null, ti.POID, ta.POID)
		, Seq1 = iif (ti.FactoryID is not null, ti.Seq1, ta.Seq1)
		, Seq2 = iif (ti.FactoryID is not null, ti.Seq2, ta.Seq2)
		, InQty = isnull (ti.Qty, 0)
		, AllocatedQty = isnull (ta.Qty, 0)
into #TPECurrentStock
from #TPEIn ti
full outer join #TPEAllocated ta on ti.FactoryID = ta.FactoryID
									and ti.POID = ta.POID
									and ti.Seq1 = ta.Seq1
									and ti.Seq2 = ta.Seq2

------------------------------------------------------------------
/*
	Factory Current Stock
*/

select	FactoryID = o.FactoryID
		, fi.POID
		, fi.Seq1
		, fi.Seq2
		, InQty = Sum (InQty)
		, OutQty = Sum (OutQty)
		, AdjustQty = Sum (AdjustQty)
		, ReturnQty = Sum (ReturnQty)
into #FtyCurrentStock
from @stock s
inner join FtyInventory fi on s.POID = fi.POID
inner join Orders o on fi.POID = o.ID
where fi.StockType = 'I'
group by o.FactoryID, fi.POID, fi.Seq1, fi.Seq2


------------------------------------------------------------------
/*
	Comparison
*/

select  FactoryID = iif (tcs.FactoryID is not null, tcs.FactoryID, fcs.FactoryID)
		, POID = iif (tcs.FactoryID is not null, tcs.POID, fcs.POID)
		, Seq1 = iif (tcs.FactoryID is not null, tcs.Seq1, fcs.Seq1)
		, Seq2 = iif (tcs.FactoryID is not null, tcs.Seq2, fcs.Seq2)
		, TPEInQty = isnull (tcs.InQty, 0)
		, TPEAllocatedQty = isnull (tcs.AllocatedQty, 0)
		, TPEBalance = isnull (tcs.InQty, 0) - isnull (tcs.AllocatedQty, 0)
		, FtyInQty = isnull (fcs.InQty, 0)
		, FtyOutQty = isnull (fcs.OutQty, 0)
		, FtyAdjustQty = isnull (fcs.AdjustQty, 0)
		, FtyReturnQty = isnull (fcs.ReturnQty, 0)
		, FtyBalance = isnull (fcs.InQty, 0) - isnull (fcs.OutQty, 0) + isnull (fcs.AdjustQty, 0) - isnull (fcs.ReturnQty, 0)
into #StockList
from #TPECurrentStock tcs
full outer join #FtyCurrentStock fcs on tcs.FactoryID = fcs.FactoryID
										and tcs.POID = fcs.POID
										and tcs.Seq1 = fcs.Seq1
										and tcs.Seq2 = fcs.Seq2
------------------------------------------------------------------
/*
	Final
*/

select	FactoryID = sl.FactoryID
		, SP = sl.POID
		, Seq = Concat (sl.Seq1, ' ', sl.Seq2)
		, OrderType = o.OrderTypeID
		, ETA = psd.ShipETA
		, REF = psd.Refno
		, MtlType = iif(psd.FabricType='F','Fabric',iif(psd.FabricType = 'A','Accessory',psd.fabrictype))
		, PurchaseUnit = psd.StockUnit
		, Color = psd.ColorID
		, Size = psd.SizeSpec
		, StockLocation = isnull (mpd.BLocation, '')
		, ShipQty = Round((isnull(psd.ShipQty, 0) + isnull(psd.ShipFOC, 0)) * v.RateValue, 2)
		, ArrivedQty = isnull (mpd.InQty, 0)
		, ReleaseQty = isnull (mpd.OutQty, 0)
		, AdjustQty = isnull (mpd.AdjustQty, 0)
		, ReturnQty = isnull (mpd.ReturnQty, 0)
		, StockInQty = sl.TPEInQty 
		, StockAllocatedQty = sl.TPEAllocatedQty 
		, StockBalance = sl.TPEBalance 
		, InQty = sl.FtyInQty
		, OutQty = sl.FtyOutQty
		, AdjustQty = sl.FtyAdjustQty
		, ReturnQty = sl.FtyReturnQty
		, BalanceQty = sl.FtyBalance
from #StockList sl
left join PO_Supp_Detail psd on sl.POID = psd.ID
								and sl.Seq1 = psd.SEQ1
								and sl.Seq2 = psd.SEQ2
left join Orders o on sl.POID = o.ID
left join MDivisionPoDetail mpd on sl.POID = mpd.POID
									and sl.Seq1 = mpd.Seq1
									and sl.Seq2 = mpd.Seq2
outer apply (
	select RateValue = dbo.GetUnitRate(psd.POUnit, psd.StockUnit)
) v
where   (sl.TPEBalance <> 0 or sl.TPEInQty <> 0 or sl.FtyBalance <> 0) 
        {whereFinal}
order by sl.POID, sl.Seq1, sl.Seq2
------------------------------------------------------------------
drop table #TPEIn, #TPEAllocated, #TPECurrentStock, #FtyCurrentStock, #StockList

");

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

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R09.xltx", 3);
            return true;
        }
    }
}
