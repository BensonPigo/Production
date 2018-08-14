using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using Sci.Win.UI;

namespace Sci.Production.Warehouse
{
    public partial class P11_Detail : Sci.Win.Subs.Input8A
    {
        Sci.Win.MatrixHelper _matrix;
        String Orderid;
        public bool combo, isSave;
        bool openFromAutoPick = false;
        public DataRow master
        {
            get;
            set;
        }

        public P11_Detail(bool openFromAutoPick = false)
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_DetailUkey";
            
            this.openFromAutoPick = openFromAutoPick;

            this.btmcont.Controls.Remove(append);
            this.btmcont.Controls.Remove(revise);
            this.btmcont.Controls.Remove(delete);            
        }

        protected override bool OnSave()
        {
            base.OnSave();
            return true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            computeTotalIssueQty();
        }

        protected override void OnDetached()
        {
            this.listControlBindingSource1.DataSource = null;
            base.OnDetached();
        }

        private DualResult matrix_Reload()
        {
            DualResult result;
            DataTable dtIssueBreakdown, dtX, dtY;
            #region -- matrix breakdown setting
            gridBreakDown.DataSource = listControlBindingSource1;
            _matrix = new Sci.Win.MatrixHelper(this, gridBreakDown, listControlBindingSource1); // 建立 Matrix 物件           
            _matrix.XMap.Name = "sizecode";  // 對應到第三表格的 X 欄位名稱
            _matrix.XOrder = "seq";
            _matrix.YMap.Name = "article";  // 對應到第三表格的 Y 欄位名稱
            _matrix
                .SetColDef("qty2", width: Widths.AnsiChars(4))  // 第三表格對應的欄位名稱
                .AddXColDef("sizecode")                             // X 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("ID", header: "OrderID", width: Widths.AnsiChars(16))
                .AddYColDef("total", header: "Total", width: Widths.AnsiChars(8))               
                .AddYColDef("article", header: "Article", width: Widths.AnsiChars(8))  // Y 要顯示的欄位名稱, 可設定多個.
                ;
            _matrix.IsXColEditable = false;  // X 顯示的欄位可否編輯?
            _matrix.IsYColEditable = false;  // Y 顯示的欄位可否編輯?
            #endregion
            if (combo)
            {
                DBProxy.Current.Select(null, string.Format(@"
select  '' as Total
        , o.id
        , oq.article
        , oq.sizecode
        , convert(varchar,oq.qty) as qty2 
from dbo.Orders o WITH (NOLOCK) 
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID 
where o.POID = '{0}'

union all 
select  '' as Total
        , ''
        , 'TTL' 
        , oq.sizecode 
        , convert(varchar,sum(oq.Qty)) qty2 
from dbo.Orders o WITH (NOLOCK) 
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID 
where o.POID = '{0}'
group by sizecode", CurrentDetailData["poid"]), out dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format(@"
select distinct os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.POID='{0}'
order by seq", CurrentDetailData["poid"]), out dtX);
                DBProxy.Current.Select(null, string.Format(@"
select  sum(oq.qty) Total
        , oq.article
        , o.ID 
from dbo.Orders o WITH (NOLOCK) 
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID 
where o.POID = '{0}' 
group by O.ID,Article 

union all
select  sum(oq.qty) Total
        , 'TTL'
        , '' 
from dbo.Orders o WITH (NOLOCK) 
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID 
where o.POID = '{0}'", CurrentDetailData["poid"]), out dtY);
            }
            else
            {
                DBProxy.Current.Select(null, string.Format(@"
select  '' as Total
        , ID
        , article
        , sizecode
        , convert(varchar,qty) as qty2 
from dbo.Order_Qty WITH (NOLOCK) 
where id = '{0}'

union all 
select  '' as Total
        , ''
        , 'TTL' 
        , sizecode 
        , convert(varchar,sum(qty)) qty2 
from dbo.Order_Qty WITH (NOLOCK) 
where id='{0}' 
group by sizecode", Orderid), out dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format(@"
select os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.id = '{0}'
order by seq", Orderid), out dtX);
                DBProxy.Current.Select(null, string.Format(@"
select  sum(qty) Total
        , '{0}' as ID
        , article 
from dbo.Order_Qty WITH (NOLOCK) 
where id = '{0}' 
group by article

union all
select  sum(qty) Total
        , ''
        , 'TTL' 
from dbo.Order_qty WITH (NOLOCK) 
where id='{0}' ", Orderid), out dtY);
            }
            _matrix.Clear();          
            if (!(result = _matrix.Sets(dtIssueBreakdown, dtX, dtY))) return result;  // 如果不是直接由資料庫載入, PR 自行處理資料來源, 再由 matrix.Set() 設定資料.
            ((DataTable)this.listControlBindingSource1.DataSource).Rows[0].Delete();  
            return Result.True;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format(@"
select  p.*
        , concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2) as seq
        , dbo.getmtldesc(p.id, p.seq1, p.seq2, 2, 0) [description]
        , [orderlist] = (select orderid+',' 
                         from (
                              select orderid 
                              from dbo.po_supp_detail_orderlist WITH (NOLOCK) 
                              where   id = p.id 
                                      and seq1 = p.seq1 
                                      and seq2 = p.seq2
                          )t for xml path('')) 
from dbo.po_supp_detail p WITH (NOLOCK) 
where p.id='{0}' and p.seq1='{1}' and p.seq2='{2}'"
                , CurrentDetailData["poid"], CurrentDetailData["seq1"], CurrentDetailData["seq2"]), out dr))
            {
                this.displySeqNo.Text = dr["seq"].ToString();
                this.displyUnit.Text = dr["StockUnit"].ToString();
                this.displyQty.Text = dr["usedqty"].ToString();
                this.displySize.Text = dr["sizespec"].ToString();
                this.displyColorid.Text = dr["colorid"].ToString();
                this.displySpecial.Text = dr["special"].ToString();
                this.editOrderList.Text = dr["orderlist"].ToString();
                this.eb_desc.Text = dr["description"].ToString();
            }

            #region -- matrix breakdown
            Orderid = master["orderid"].ToString();
            DualResult result;
            if (!(result = matrix_Reload()))
            {
                ShowErr(result);
            }

            string sql = string.Empty;
            if (combo)
            {
                sql = $@"
select sizes = stuff((
	select concat(' or sizecode =''',sizecode,'''') 
	from(
        select distinct seq,os.sizecode
        from dbo.Order_SizeCode os WITH(NOLOCK)
        inner join orders o WITH(NOLOCK) on o.POID = os.Id
        inner join dbo.Order_Qty oq WITH(NOLOCK) on o.id = oq.ID and os.SizeCode = oq.SizeCode
        where o.POID = '{CurrentDetailData["poid"]}'
	)a
	for xml path('')
	),1,3,'')
";
            }
            else
            {
                sql = $@"
select sizes = stuff((
	select concat(' or sizecode =''',sizecode,'''') 
	from(
		select distinct seq,os.sizecode
        from dbo.Order_SizeCode os WITH (NOLOCK) 
        inner join orders o WITH (NOLOCK) on o.POID = os.Id
        inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
        where  o.id ='{Orderid}'
	)a
	for xml path('')
	),1,3,'')
";
            }

            string f = MyUtility.GetValue.Lookup(sql);
            this.gridbs.Filter = f;

            #endregion
            #region 重新計算 Total Issue Qty
            computeTotalIssueQty();
            #endregion 
        }
        
        protected override bool OnGridSetup()
        {
            #region issueQtySet
            DataGridViewGeneratorNumericColumnSettings issueQtySet = new DataGridViewGeneratorNumericColumnSettings();
            issueQtySet.CellValidating += (s, e) =>
            {
                grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.FormattedValue;
                if (e.ColumnIndex == 1)
                {
                    computeTotalIssueQty();
                }
            };
            #endregion 
            /*
             * 請注意 如果以後 grid 有追加欄位
             * 1. 請確認 computTotalIssueQty => Cells 所指定的欄位是 QTY
             * 2. 確認 issueQtySet 中 ColumnIndex 判斷的是 QTY 欄位
            */
            Helper.Controls.Grid.Generator(this.grid)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(10), iseditingreadonly: true)  //1
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: issueQtySet)    //2
            ;
            #region computTotalIssueQty
            computeTotalIssueQty();
            #endregion 

            this.grid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }

        private void computeTotalIssueQty()
        {
            /*
             * 請注意 如果以後 grid 有追加欄位
             * 請把確認 Cells 所指定的欄位是 QTY
            */
            decimal totalQty = 0;
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                totalQty += decimal.Parse(grid.Rows[i].Cells[1].Value.ToString());
            }
            this.displayTotalIssueQty.Value = totalQty;
        }

        private void save_Click(object sender, EventArgs e)
        {
            isSave = true;
        }

        private void undo_Click(object sender, EventArgs e)
        {
            isSave = false;
        }
    }
}
