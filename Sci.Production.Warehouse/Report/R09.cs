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
            MyUtility.Tool.SetupCombox(this.comboFilterCondition, 1, 1, "Actual Inventory Qty < Taipei system,Inventory In  < Taipei InputQty,");
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
            if (!MyUtility.Check.Empty(this.buyerDelivery1) || !MyUtility.Check.Empty(this.buyerDelivery2))
            {
                string sqlBuyerDelivery = string.Empty;
                if (!MyUtility.Check.Empty(this.buyerDelivery1))
                {
                    sqlBuyerDelivery += string.Format(" '{0}' <= o.BuyerDelivery ", Convert.ToDateTime(this.buyerDelivery1).ToString("d"));
                }

                if (!MyUtility.Check.Empty(this.buyerDelivery2))
                {
                    sqlBuyerDelivery += (MyUtility.Check.Empty(sqlBuyerDelivery) ? string.Empty : " and ") + string.Format(" o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d"));
                }

                sqlCmd.Append(string.Format(
                    @"
;with cte as 
(
	select poid 
    from dbo.orders o WITH (NOLOCK) 
	where {0} 
    group by POID
)
select  distinct 
        FactoryID           = orders.FactoryID,
        SP					= a.POID, 
        SEQ					= concat(a.seq1, ' ', a.seq2),  
		OrderType			= orders.OrderTypeID,
        ETA					= b.ShipETA,
        REF					= A.Refno, 
        MtlType				= iif(A.FabricType='F','Fabric',iif(a.FabricType = 'A','Accessory',a.fabrictype)), 
        PurchaseUnit		= B.StockUnit,
        Color				= B.ColorID, 
		Size				= B.SizeSpec, 
        StockLocation		= C.BLocation, 
        ShipQty				= Round((isnull(B.ShipQty, 0) + isnull(B.ShipFOC, 0)) * v.RateValue, 2),
        ArrivedQty			= isnull(C.InQty, 0), 
        ReleasedQty			= isnull(C.OutQty, 0), 
        AdjustQty			= isnull(C.AdjustQty, 0),
        ReturnQty			= isnull(C.ReturnQty, 0),
        StockInQty			= Round(isnull(InsQty14.qty,0) * v.RateValue, 2) ,
        StockAllocatedQty	= Round((isnull(InsQty25.qty,0) - isnull(InsQty46.qty,0)) * v.RateValue, 2) , 
        StockBalanceQty		= Round(isnull(InsQty14.qty,0) * v.RateValue, 2) -Round((isnull(InsQty25.qty,0) - isnull(InsQty46.qty,0)) * v.RateValue, 2) ,
        InQty				= isnull(x.InQty, 0), 
        OutQty				= isnull(x.OutQty, 0), 
        AdjustQty			= isnull(x.AdjustQty, 0),
        ReturnQty			= isnull(x.ReturnQty, 0),
        BalanceQty			= isnull(x.InQty, 0) - isnull(x.OutQty, 0) + isnull(x.AdjustQty, 0) - isnull(x.ReturnQty, 0)
from cte
inner join Inventory a WITH (NOLOCK) on a.POID = cte.POID 
inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.POID and b.seq1 = a.seq1 and b.seq2 = a.Seq2
inner join MDivisionPoDetail c WITH (NOLOCK) on c.POID = a.POID and c.seq1 = a.seq1 and c.seq2 = a.Seq2
inner join Orders orders on c.poid = orders.id
inner join Factory d WITH (NOLOCK) on orders.FactoryID = d.id
outer apply (select RateValue = dbo.GetUnitRate(b.POUnit, b.StockUnit)) v
outer apply(
	select qty = sum(i.qty)
	from Invtrans i WITH (NOLOCK) 
	where i.inventorypoid = a.POID and i.inventoryseq1 = a.seq1 and i.inventoryseq2 = a.seq2 and i.qty !=0
	and (i.type = '1' or(i.type = '4' and i.qty >0))
)InsQty14
outer apply(
	select qty = sum(i.qty)
	from Invtrans i WITH (NOLOCK) 
	where i.inventorypoid = a.POID and i.inventoryseq1 = a.seq1 and i.inventoryseq2 = a.seq2 and i.qty !=0
	and (i.type = '6' or(i.type = '4' and i.qty < 0))
)InsQty46
outer apply(
	select qty = sum(i.qty)
	from Invtrans i WITH (NOLOCK) 
	where i.inventorypoid = a.POID and i.inventoryseq1 = a.seq1 and i.inventoryseq2 = a.seq2 and i.qty !=0
	and (i.type = '2' or i.type = '5')
)InsQty25
outer apply (
    select  isnull(sum(m.InQty),0.00) InQty
            , isnull(sum(m.OutQty),0.00) OutQty
            , isnull(sum(m.AdjustQty),0.00) AdjustQty 
            , isnull(sum(m.ReturnQty),0.00) ReturnQty 
    from dbo.FtyInventory m WITH (NOLOCK) 
    where   m.POID = a.POID 
            and m.seq1 = a.seq1 
            and m.seq2 = a.seq2 
            and StockType = 'I' 
) x
where (b.InputQty <> 0 or c.LInvQty <> 0) ", sqlBuyerDelivery));
            }
            else
            {
                sqlCmd.Append(string.Format(@"
select  distinct 
        FactoryID           = orders.FactoryID,
        SP					= a.POID, 
        SEQ					= concat(a.seq1, ' ', a.seq2),  
		OrderType			= orders.OrderTypeID,
        ETA					= b.ShipETA,
        REF					= A.Refno, 
        MtlType				= iif(A.FabricType='F','Fabric',iif(a.FabricType = 'A','Accessory',a.fabrictype)), 
        PurchaseUnit		= B.StockUnit,
        Color				= B.ColorID, 
		Size				= B.SizeSpec, 
        StockLocation		= C.BLocation, 
        ShipQty				= Round((isnull(B.ShipQty, 0) + isnull(B.ShipFOC, 0)) * v.RateValue, 2),
        ArrivedQty			= isnull(C.InQty, 0), 
        ReleasedQty			= isnull(C.OutQty, 0), 
        AdjustQty			= isnull(C.AdjustQty, 0),
        ReturnQty			= isnull(C.ReturnQty, 0),
        StockInQty			= Round(isnull(InsQty14.qty,0) * v.RateValue, 2) ,
        StockAllocatedQty	= Round((isnull(InsQty25.qty,0) - isnull(InsQty46.qty,0)) * v.RateValue, 2) , 
        StockBalanceQty		= Round(isnull(InsQty14.qty,0) * v.RateValue, 2) -Round((isnull(InsQty25.qty,0) - isnull(InsQty46.qty,0)) * v.RateValue, 2) ,
        InQty				= isnull(x.InQty, 0), 
        OutQty				= isnull(x.OutQty, 0), 
        AdjustQty			= isnull(x.AdjustQty, 0),
        ReturnQty			= isnull(x.ReturnQty, 0),
        BalanceQty			= isnull(x.InQty, 0) - isnull(x.OutQty, 0) + isnull(x.AdjustQty, 0) - isnull(x.ReturnQty, 0)
from Inventory a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.POID and b.seq1 = a.seq1 and b.seq2 = a.Seq2
inner join MDivisionPoDetail c WITH (NOLOCK) on c.POID = a.POID and c.seq1 = a.seq1 and c.seq2 = a.Seq2 
inner join Orders orders on c.poid = orders.id
inner join Factory d WITH (NOLOCK) on orders.FactoryID = d.id
outer apply (select RateValue = dbo.GetUnitRate(b.POUnit, b.StockUnit)) v
outer apply(
	select qty = sum(i.qty)
	from Invtrans i WITH (NOLOCK) 
	where i.inventorypoid = a.POID and i.inventoryseq1 = a.seq1 and i.inventoryseq2 = a.seq2 and i.qty !=0
	and (i.type = '1' or(i.type = '4' and i.qty >0))
)InsQty14
outer apply(
	select qty = sum(i.qty)
	from Invtrans i WITH (NOLOCK) 
	where i.inventorypoid = a.POID and i.inventoryseq1 = a.seq1 and i.inventoryseq2 = a.seq2 and i.qty !=0
	and (i.type = '6' or(i.type = '4' and i.qty < 0))
)InsQty46
outer apply(
	select qty = sum(i.qty)
	from Invtrans i WITH (NOLOCK) 
	where i.inventorypoid = a.POID and i.inventoryseq1 = a.seq1 and i.inventoryseq2 = a.seq2 and i.qty !=0
	and (i.type = '2' or i.type = '5')
)InsQty25
outer apply (
    select  isnull(sum(m.InQty),0.00) InQty
            , isnull(sum(m.OutQty),0.00) OutQty
            , isnull(sum(m.AdjustQty),0.00) AdjustQty 
            , isnull(sum(m.ReturnQty),0.00) ReturnQty 
    from dbo.FtyInventory m WITH (NOLOCK) 
    where   m.POID = a.POID 
            and m.seq1 = a.seq1 
            and m.seq2 = a.seq2 
            and StockType = 'I' 
) x
where (b.InputQty <> 0 or c.LInvQty <> 0)"));
            }

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.deadline1) || !MyUtility.Check.Empty(this.deadline2))
            {
                if (!MyUtility.Check.Empty(this.deadline1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= a.deadline", Convert.ToDateTime(this.deadline1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.deadline2))
                {
                    sqlCmd.Append(string.Format(@" and a.deadline <= '{0}'", Convert.ToDateTime(this.deadline2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and a.Poid >= @spno1 and a.Poid <= @spno2");
                sp_spno1.Value = this.spno1.PadRight(10, '0');
                sp_spno2.Value = this.spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(this.spno1))
            {
                // 只有 sp1 輸入資料
                sqlCmd.Append(" and a.Poid like @spno1 ");
                sp_spno1.Value = this.spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(this.spno2))
            {
                // 只有 sp2 輸入資料
                sqlCmd.Append(" and a.Poid like @spno2 ");
                sp_spno2.Value = this.spno2 + "%";
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.eta1) || !MyUtility.Check.Empty(this.eta2))
            {
                if (!MyUtility.Check.Empty(this.eta1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= b.ShipEta", Convert.ToDateTime(this.eta1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.eta2))
                {
                    sqlCmd.Append(string.Format(@" and b.ShipEta <= '{0}'", Convert.ToDateTime(this.eta2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and d.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and orders.FactoryID = @Factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.refno1) && !MyUtility.Check.Empty(this.refno2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and b.refno >= @refno1 and b.refno <= @refno2");
                sp_refno1.Value = this.refno1;
                sp_refno2.Value = this.refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(this.refno1))
            {
                // 只輸入 Refno1
                sqlCmd.Append(" and b.refno like @refno1");
                sp_refno1.Value = this.refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(this.refno2))
            {
                // 只輸入 Refno2
                sqlCmd.Append(" and b.refno like @refno2");
                sp_refno2.Value = this.refno2 + "%";
                cmds.Add(sp_refno2);
            }

            if (this.filterIndex == 0)
            {
                // sqlCmd.Append(" and c.linvQty < (B.InputQty - B.OutputQty) * ISNULL(v.RateValue, 1)");
                sqlCmd.Append(" and Round((isnull(B.InputQty, 0) - isnull(B.OutputQty, 0)) * isnull(v.RateValue, 1), 2) > isnull(x.InQty, 0) - isnull(x.OutQty, 0) + isnull(x.AdjustQty, 0) - isnull(x.ReturnQty, 0)");
            }

            if (this.filterIndex == 1)
            {
                sqlCmd.Append(" and x.InQty < Round(B.InputQty * isnull(v.RateValue, 1), 2)");
            }
            #endregion

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
