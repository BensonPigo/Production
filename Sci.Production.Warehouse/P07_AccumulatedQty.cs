using System;
using System.Data;
using System.Drawing;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P07_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P07;
        protected DataRow dr;

        public P07_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string strSqlCmd = string.Empty;
            if (MyUtility.Check.Empty(this.dr["ExportID"]))
            {
                strSqlCmd = $@"
select poid,seq1,seq2
,[shipqty] = dbo.GetUnitQty(PoUnit,dbo.GetStockUnitBySPSeq(poid,seq1,seq2),sum(shipqty))
,[received] = sum(accu_rcv) 
,[receiving] = sum(rcv) 
,description
,Foc = sum(Foc)
from (
    select a.PoId,a.Seq1,a.Seq2,0 as shipqty,0 as accu_rcv,sum(a.StockQty) as rcv
        ,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] ,a.POUnit ,Foc = 0
    from dbo.Receiving_Detail a WITH (NOLOCK) 
    where id='{this.dr["id"]}' 
    group by a.PoId,a.Seq1,a.Seq2,a.POUnit

    union all              

    select a.id poid,a.Seq1,a.seq2,a.Qty as shipqty,0 as accu_rcv,0 as rcv
        ,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [description],a.POUnit,a.Foc
    from dbo.PO_Supp_Detail a WITH (NOLOCK) 
    ,(select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail WITH (NOLOCK) where id='{this.dr["id"]}') c 
    where a.id = c.poid and a.seq1 = c.seq1 and a.seq2 = c.seq2
) tmp
group by poid,seq1,seq2,description,POUnit";
            }
            else
            {
                strSqlCmd = $@"

select ed.poid,ed.seq1,ed.seq2
,[shipqty] = dbo.GetUnitQty(rd.PoUnit,dbo.GetStockUnitBySPSeq(ed.poid,ed.seq1,ed.seq2),ed.Qty) 
,[received] = sum(accu_rcv) 
,[receiving] = sum(rcv)
,rd.description
,Foc = ed.Foc
from Export_Detail ed
inner join dbo.PO_Supp_Detail pod WITH (NOLOCK) on pod.ID= ed.POid   
                                                 and pod.Seq1 = ed.SEQ1    
                                                 and pod.Seq2 = ed.SEQ2  
outer apply(
	select a.PoId,a.Seq1,a.Seq2,0 as accu_rcv,sum(a.StockQty) as rcv
		,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] ,a.POUnit 
    from dbo.Receiving_Detail a WITH (NOLOCK) 
	where 1=1
	and a.Id='{this.dr["id"]}' 
	and a.PoId=ed.PoID and a.Seq1=ed.Seq1 and a.Seq2=ed.Seq2
	group by a.PoId,a.Seq1,a.Seq2,a.POUnit

    union all

    select a.PoId,a.Seq1,a.Seq2,sum(a.StockQty) as accu_rcv ,0 as rcv
        ,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description],a.POUnit
    from dbo.Receiving_Detail a WITH (NOLOCK) 
    ,dbo.Receiving b WITH (NOLOCK)
    where b.id!='{this.dr["id"]}' and b.Status='Confirmed' and a.id=b.id and b.ExportId = ed.ID
    and a.PoId=ed.poid and a.seq1 = ed.seq1 and a.seq2 = ed.seq2 
    group by a.PoId,a.Seq1,a.Seq2,a.POUnit
)rd

where ed.id='{this.dr["ExportID"]}'
group by ed.poid,ed.seq1,ed.seq2,rd.description,rd.PoUnit,ed.Qty,ed.Foc
order by PoId,Seq1,Seq2
";
            }

            DataTable selectDataTable1;
            this.P07.ShowWaitMessage("Data loading...");
            DBProxy.Current.DefaultTimeout = 1200;
            DualResult selectResult1 = DBProxy.Current.Select(null, strSqlCmd.ToString(), out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(strSqlCmd.ToString(), selectResult1);
            }

            DBProxy.Current.DefaultTimeout = 0;
            this.P07.HideWaitMessage();
            selectDataTable1.ColumnsDecimalAdd("variance", 0m, "received+receiving-shipqty-foc");
            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("shipQty", header: "Ship Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("Foc", header: "F.O.C", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("received", header: "Accu. Rcvd.", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("receiving", header: "Rcvd. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("variance", header: "Variance", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
            this.Grid_Filter();
            this.ChangeColor();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Grid_Filter()
        {
            string filter = string.Empty;

            switch (this.chkRcvdQty.Checked)
            {
                case true:
                    if (MyUtility.Check.Empty(this.gridAccumulatedQty))
                    {
                        break;
                    }

                    filter = $@"receiving > 0";
                    ((DataTable)this.bindingSource1.DataSource).DefaultView.RowFilter = filter;
                    break;

                case false:
                    if (MyUtility.Check.Empty(this.gridAccumulatedQty))
                    {
                        break;
                    }

                    filter = string.Empty;
                    ((DataTable)this.bindingSource1.DataSource).DefaultView.RowFilter = filter;
                    break;
            }
        }

        private void ChangeColor()
        {
            for (int index = 0; index < this.gridAccumulatedQty.Rows.Count; index++)
            {
                DataRow dr = this.gridAccumulatedQty.GetDataRow(index);
                if (this.gridAccumulatedQty.Rows.Count <= index || index < 0)
                {
                    return;
                }

                int i = index;

                if (MyUtility.Check.Empty(dr["receiving"]))
                {
                    this.gridAccumulatedQty.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                }
                else
                {
                    this.gridAccumulatedQty.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void ChkRcvdQty_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
            this.ChangeColor();
        }
    }
}
