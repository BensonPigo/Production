﻿using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_Scrap : Win.Subs.Base
    {
        private DataRow dr;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public P03_Scrap(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += string.Format(" ({0}-{1}- {2})", this.dr["id"].ToString(),
this.dr["seq1"].ToString(),
this.dr["seq2"].ToString());
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 表頭
            string selectCommand1 = string.Format(
                @"
select *
,[balance] = sum(inqty-outqty+adjustQty)  over (order by issuedate,id,name)
from 
(
select a.EditDate ,a.issuedate 
, a.id
,case type  when 'E' then 'P24. Transfer Inventory to Scrap (B2C)' 
			when 'D' then 'P25. Transfer Bulk to Scrap (A2C)' 
			when 'C' then 'P36. Transfer Scrap to Inventory (C2B)'  end name
,inqty = case type when 'E' then sum(b.Qty)  
				   when 'D' then sum(b.Qty)
				   else 0 end 
,outqty=  case type when 'C' then sum(b.Qty) else 0 end
,adjustQty = 0
from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
where  Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}'  
and a.id = b.id
and a.Status='Confirmed'
and type in ('C','D','E')
group by a.id, frompoid,a.EditDate,a.Type ,a.issuedate 

union all

select 	a.EditDate , a.issuedate
, a.id
,  name = Case type 
when 'O' then 'P43. Adjust Scrap Qty' 
when 'R' then 'P45. Remove from Scrap Whse' end
,inqty = 0 
,outqty = 0
,adjustQty = sum(QtyAfter - QtyBefore) 
from Adjust a WITH (NOLOCK) , Adjust_Detail b WITH (NOLOCK)        
where  poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id
and a.Status='Confirmed'
and type in ('O','R')
group by a.id, poid,a.EditDate,type,a.issuedate 
) a
group by IssueDate,EditDate,id,name,inqty,outqty,adjustQty
order by IssueDate,EditDate,name",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString(), Env.User.Keyword);
#endregion

            #region DetailGrid
            string sqlDetail = string.Format(
                @"
select *
,[balance] = sum(inqty-outqty+adjustQty)  over  (order by issuedate,EditDate, roll,dyelot,inqty)
from 
(
	select Roll=b.FromRoll
	,Dyelot= b.FromDyelot
	,StockType = b.FromStockType
	,inqty = case type when 'E' then (b.Qty)  
					   when 'D' then (b.Qty)
					   else 0 end 
	,outqty=  case type when 'C' then (b.Qty) else 0 end
	,adjustQty = 0
	,Location  = ToLocation.location
	,ContainerCode = ToContainerCode.ContainerCode
    ,a.EditDate , a.issuedate 
	from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
outer apply(
    select  location = stuff(
        (
            select ',' + x.location								
            from(select distinct location = sd.ToLocation 
            from SubTransfer_Detail sd
            where Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}' 
            and sd.FromDyelot = b.FromDyelot and sd.FromRoll = b.FromRoll and sd.FromStockType = b.FromStockType
        )x			
    for xml path('')),1,1,'') 
) ToLocation
outer apply(
    select  ContainerCode = stuff(
        (
            select ',' + x.ContainerCode 								
            from(select distinct ContainerCode  = sd.ToContainerCode 
            from SubTransfer_Detail sd
            where Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}' 
            and sd.FromDyelot = b.FromDyelot and sd.FromRoll = b.FromRoll and sd.FromStockType = b.FromStockType
        )x			
    for xml path('')),1,1,'') 
) ToContainerCode
	where  Frompoid='{0}' and Fromseq1 = '{1}'and FromSeq2 = '{2}'  
	and a.id = b.id
    and a.Status='Confirmed'
    and type in ('C','D','E')

	union all

	select 	b.Roll
	, b.Dyelot
	, b.StockType
	,inqty = 0 
	,outqty = 0
	,adjustQty = (QtyAfter - QtyBefore) 
	,Location  = ''
	,fi.ContainerCode
    ,a.EditDate , a.issuedate
	from Adjust a WITH (NOLOCK) , Adjust_Detail b WITH (NOLOCK)         
	left join FtyInventory fi on fi.POID = b.POID
		and fi.Seq1 = b.Seq1 and fi.Seq2 = b.Seq2
		and fi.Roll = b.Roll and fi.Dyelot = b.Dyelot
		and fi.StockType = b.StockType
	where  b.poid='{0}' and b.seq1 = '{1}'and b.seq2 = '{2}'  and a.id = b.id
    and a.Status='Confirmed'
    and type in ('O','R')
) a order by issuedate,EditDate
",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString(), Env.User.Keyword);
            #endregion

            DataTable selectDataTable1, selectDataTable2;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }

            DualResult selectResult2 = DBProxy.Current.Select(null, sqlDetail, out selectDataTable2);
            if (selectResult2 == false)
            {
                this.ShowErr(sqlDetail, selectResult2);
            }

            this.bindingSource1.DataSource = selectDataTable1;
            this.bindingSource2.DataSource = selectDataTable2;
            Ict.Win.UI.DataGridViewTextBoxColumn col_ContainerCode;

            // 設定gridScrapList的顯示欄位
            this.gridScrapList.IsEditingReadOnly = true;
            this.gridScrapList.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridScrapList)
                 .Text("issuedate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("ID", header: "Transaction ID", width: Widths.AnsiChars(14))
                 .Text("Name", header: "Name", width: Widths.AnsiChars(35))
                 .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("OutQty", header: "Out Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("adjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 ;

            this.grid_detail.IsEditingReadOnly = true;
            this.grid_detail.DataSource = this.bindingSource2;
            this.Helper.Controls.Grid.Generator(this.grid_detail)
                 .Text("roll", header: "Roll#", width: Widths.AnsiChars(8))
                 .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                 .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("OutQty", header: "Out Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("adjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("Location", header: "To Location", width: Widths.AnsiChars(15))
                 .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out col_ContainerCode)
                 ;

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            col_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
