using System;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_TaipeiInventory : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable selectDataTable1;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public P03_TaipeiInventory(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.comboSortBy.SelectedIndex = 0;
            this.Text += string.Format(" ({0}-{1}- {2})", this.dr["id"].ToString(),
this.dr["seq1"].ToString(),
this.dr["seq2"].ToString());
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(
                @"
SELECT  *, 
        sum(TMP.inqty - TMP.Allocated) over ( order by ID,SEQ,sum(TMP.inqty - TMP.Allocated) desc ) as [balance]
		,[balanceQty] = sum(TMP.[StockInQty] - TMP.[StockAllocatedQty]) over ( order by ID,SEQ,sum(TMP.[StockInQty] - TMP.[StockAllocatedQty]) desc)
FROM (
    SELECT  inv.ID
            ,inv.Type
            ,case inv.Type 
			    when '1' then '1:Input'
			    when '2' then '2:Output'
			    when '3' then '3:Transfer In'
			    when '4' then '4:Adjust'
			    when '5' then '5:Obsolescene'
			    when '6' then '6:Return'
			 end as typename 
            , inv.ConfirmDate
            , Round(dbo.GetUnitQty(inv.UnitID, po.StockUnit, isnull(inv.Qty, 0.00)), 4) inqty
            , 0 Allocated
            , TPEPASS1.ID+'-'+TPEPASS1.NAME ConfirmHandle
            , po.StockUnit
            , concat(inv.seq70poid, '-', inv.seq70seq1, '-', inv.seq70seq2) as seq70
            , [UseFactory] = inv.TransferFactory
            , case inv.type 
                when '3' then inv.TransferFactory 
                else inv.FactoryID 
              end as factoryid
            , invtransreason.ReasonEN
            , case inv.type 
                when '3' then '2' 
                else '1' 
              end AS SEQ
            , inv.remark
            ,SpecificDestination = iif( aa.SpecificDestination> 1 ,1,0)
            , inv.ukey
			,inv.UnitID
			,[StockInQty] = isnull(inv.Qty, 0.00)
			,[StockAllocatedQty] = 0
    FROM InvTrans Inv WITH (NOLOCK) 
    inner join Po_Supp_Detail po on inv.InventoryPoid = po.id and inv.InventorySeq1 = po.seq1 and inv.InventorySeq2 = po.seq2
    left join invtransReason WITH (NOLOCK) on inv.reasonid = invtransreason.id
    LEFT JOIN TPEPASS1 WITH (NOLOCK) ON inv.ConfirmHandle = TPEPASS1.ID
    outer apply (
		select COUNT(cn.SpecificDestination) as SpecificDestination
		from View_WH_Orders wh WITH (NOLOCK) 
		inner join Country cn WITH (NOLOCK) on wh.Dest = cn.ID
		where inv.seq70POID = wh.POID 
              and cn.SpecificDestination = 1 
	) aa

    WHERE   inv.InventoryPOID ='{0}'
            and inv.InventorySeq1 = '{1}'
            and inv.InventorySeq2 = '{2}' 
			and inv.Type in ('1', '3', '6')             
                                                                         
    union
    SELECT  inv.ID
            , inv.Type
            , case inv.Type 
			    when '1' then '1:Input'
			    when '2' then '2:Output'
			    when '3' then '3:Transfer Out'
			    when '4' then '4:Adjust'
			    when '5' then '5:Obsolescene'
			    when '6' then '6:Return'
			  end as typename
            , inv.ConfirmDate
            , 0 inqty
            , Round(dbo.GetUnitQty(inv.UnitID, StockUnit, isnull(inv.Qty, 0.00)), 4) Allocated
            , TPEPASS1.ID+'-'+TPEPASS1.NAME ConfirmHandle
            , po.StockUnit
            , concat(inv.seq70poid, '-', inv.seq70seq1, '-', inv.seq70seq2) as seq70
            , [UseFactory] = inv.TransferFactory
            , case inv.type 
                when '3' then inv.FactoryID 
                else inv.FactoryID 
              end as FactoryID
            , invtransreason.ReasonEN
            , case inv.type 
                when '3' then '1' 
                else '2' 
              end AS SEQ
            , inv.remark
            ,SpecificDestination = iif( aa.SpecificDestination> 1 ,1,0)
            , inv.ukey
			,inv.UnitID
			,[StockInQty] = 0
			,[StockAllocatedQty] = isnull(inv.Qty, 0.00)
    FROM InvTrans inv WITH (NOLOCK) 
	left join View_WH_Orders wh  on inv.seq70poid = wh.ID
	left join Country cn on wh.Dest = cn.ID
    inner join Po_Supp_Detail po on inv.InventoryPoid = po.id and inv.InventorySeq1 = po.seq1 and inv.InventorySeq2 = po.seq2
    left join invtransReason WITH (NOLOCK) on inv.reasonid = invtransreason.id
	LEFT JOIN TPEPASS1 WITH (NOLOCK) ON inv.ConfirmHandle = TPEPASS1.ID
    outer apply (
		select COUNT(cn.SpecificDestination) as SpecificDestination
		from View_WH_Orders wh WITH (NOLOCK) 
		inner join Country cn WITH (NOLOCK) on wh.Dest = cn.ID
		where inv.seq70POID = wh.POID 
              and cn.SpecificDestination = 1 
	) aa


    WHERE   inv.InventoryPOID ='{0}'
            and inv.InventorySeq1 = '{1}'
            and inv.InventorySeq2 = '{2}'
			and inv.type in ('2', '3', '5')    

    union 
    SELECT  inv.ID
            , inv.Type
            , case inv.Type 
			    when '1' then '1:Input'
			    when '2' then '2:Output'
			    when '3' then '3:Transfer Out'
			    when '4' then '4:Adjust'
			    when '5' then '5:Obsolescene'
			    when '6' then '6:Return'
			  end as typename
            , inv.ConfirmDate
            , Round(dbo.GetUnitQty(inv.UnitID, po.StockUnit, iif(inv.Qty >= 0, inv.Qty, 0)), 4) inqty
            , Round(dbo.GetUnitQty(inv.UnitID, po.StockUnit, iif(inv.Qty < 0, -inv.Qty, 0)), 4) Allocated
            , TPEPASS1.ID+'-'+TPEPASS1.NAME ConfirmHandle
            , po.StockUnit
            , concat(inv.seq70poid, '-', inv.seq70seq1, '-', inv.seq70seq2) as seq70
            , [UseFactory] = inv.TransferFactory
            , case inv.type 
                when '3' then inv.FactoryID 
                else inv.FactoryID 
              end as FactoryID
            , invtransreason.ReasonEN
            , case inv.type 
                when '3' then '1' 
                else '2' 
              end AS SEQ
            , inv.remark
            ,SpecificDestination = iif( aa.SpecificDestination> 1 ,1,0)
            , inv.ukey
			,inv.UnitID
			,[StockInQty] = iif(inv.Qty >= 0, inv.Qty, 0)
			,[StockAllocatedQty] =  iif(inv.Qty < 0, -inv.Qty, 0)
    FROM InvTrans inv WITH (NOLOCK) 
	left join View_WH_Orders wh  on inv.seq70poid = wh.ID
	left join Country cn on wh.Dest = cn.ID
    inner join Po_Supp_Detail po on inv.InventoryPoid = po.id and inv.InventorySeq1 = po.seq1 and inv.InventorySeq2 = po.seq2
    left join invtransReason WITH (NOLOCK) on inv.reasonid = invtransreason.id
	LEFT JOIN TPEPASS1 WITH (NOLOCK) ON inv.ConfirmHandle = TPEPASS1.ID
    outer apply (
		select COUNT(cn.SpecificDestination) as SpecificDestination
		from View_WH_Orders wh WITH (NOLOCK) 
		inner join Country cn WITH (NOLOCK) on wh.Dest = cn.ID
		where inv.seq70POID = wh.POID 
              and cn.SpecificDestination = 1 
	) aa

    WHERE   inv.InventoryPOID ='{0}'
            and inv.InventorySeq1 = '{1}'
            and inv.InventorySeq2 = '{2}'
			and inv.type in ('4')    
) TMP 
GROUP BY    TMP.ID, TMP.TYPE, TMP.typename, TMP.ConfirmDate, TMP.ConfirmHandle, TMP.factoryid, TMP.seq70
            , TMP.ReasonEN, TMP.SEQ, TMP.inqty, TMP.Allocated, Tmp.remark, Tmp.ukey, Tmp.UseFactory,Tmp.SpecificDestination,Tmp.StockUnit
            ,tmp.UnitID,TMP.StockInQty,TMP.StockAllocatedQty",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString());

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out this.selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }
            else
            {
                string remark = string.Empty;
                foreach (DataRow dr2 in this.selectDataTable1.Rows)
                {
                    if (!MyUtility.Check.Empty(dr2["remark"].ToString()))
                    {
                        remark += dr2["remark"].ToString().TrimEnd() + Environment.NewLine;
                    }
                }

                this.editRemark.Text = remark;
            }

            this.bindingSource1.DataSource = this.selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridTaipeiInventoryList.IsEditingReadOnly = true;
            this.gridTaipeiInventoryList.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridTaipeiInventoryList)
                .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                .Text("factoryid", header: "Factory", width: Widths.AnsiChars(8))
                .Text("typeName", header: "Type", width: Widths.AnsiChars(13))
                .Date("confirmdate", header: "Date", width: Widths.AnsiChars(10))
                .Text("confirmhandle", header: "Handle", width: Widths.AnsiChars(20))
                .Text("StockUnit", header: "StockUnit", width: Widths.AnsiChars(6))
                .Numeric("inqty", header: "Stock In Qty\r\n( Stock Unit)", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 4)
                .Numeric("Allocated", header: "Stock Allocated Qty\r\n( Stock Unit)", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 4)
                .Numeric("balance", header: "Balance Qty\r\n( Stock Unit)", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 4)
                .Text("seq70", header: "Use for SP#", width: Widths.AnsiChars(20))
                .Text("UseFactory", header: "Use for Factory", width: Widths.AnsiChars(6))
                .CheckBox("SpecificDestination", header: "Specific Destination", width: Widths.AnsiChars(26))
                .Text("ReasonEN", header: "Reason", width: Widths.AnsiChars(60))
                .Text("UnitID", header: "PO Unit", width: Widths.AnsiChars(6))
                .Numeric("StockInQty", header: "Stock In Qty\r\n( PO Unit )", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2)
                .Numeric("StockAllocatedQty", header: "Stock Allocated Qty\r\n( PO Unit )", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2)
                .Numeric("balanceQty", header: "Balance Qty\r\n( PO Unit)", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2)
                ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void ComboSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboSortBy.SelectedIndex)
            {
                case 0:
                    if (MyUtility.Check.Empty(this.selectDataTable1))
                    {
                        break;
                    }

                    this.selectDataTable1.DefaultView.Sort = "confirmdate , id";
                    break;
                case 1:
                    if (MyUtility.Check.Empty(this.selectDataTable1))
                    {
                        break;
                    }

                    this.selectDataTable1.DefaultView.Sort = "type , id";
                    break;

                default:
                    break;
            }
        }
    }
}
