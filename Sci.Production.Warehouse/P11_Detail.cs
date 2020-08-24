using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P11_Detail : Win.Subs.Input8A
    {
        private Win.MatrixHelper _matrix;
        private string Orderid;

        /// <inheritdoc/>
        public bool Combo { get; set; }

        /// <inheritdoc/>
        public bool IsSave { get; set; }

        /// <inheritdoc/>
        public DataRow Master { get; set; }

        /// <inheritdoc/>
        public P11_Detail()
        {
            this.InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_DetailUkey";

            this.btmcont.Controls.Remove(this.append);
            this.btmcont.Controls.Remove(this.revise);
            this.btmcont.Controls.Remove(this.delete);
        }

        /// <inheritdoc/>
        protected override bool OnSave()
        {
            base.OnSave();
            return true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.ComputeTotalQty();
        }

        /// <inheritdoc/>
        protected override void OnDetached()
        {
            this.listControlBindingSource1.DataSource = null;
            base.OnDetached();
        }

        private DualResult Matrix_Reload()
        {
            DualResult result;
            DataTable dtIssueBreakdown, dtX, dtY;
            #region -- matrix breakdown setting
            this.gridBreakDown.DataSource = this.listControlBindingSource1;
            this._matrix = new Win.MatrixHelper(this, this.gridBreakDown, this.listControlBindingSource1); // 建立 Matrix 物件
            this._matrix.XMap.Name = "sizecode";  // 對應到第三表格的 X 欄位名稱
            this._matrix.XOrder = "seq";
            this._matrix.YMap.Name = "article";  // 對應到第三表格的 Y 欄位名稱
            this._matrix
                .SetColDef("qty2", width: Widths.AnsiChars(4)) // 第三表格對應的欄位名稱
                .AddXColDef("sizecode") // X 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("ID", header: "OrderID", width: Widths.AnsiChars(16))
                .AddYColDef("total", header: "Total", width: Widths.AnsiChars(8))
                .AddYColDef("article", header: "Article", width: Widths.AnsiChars(8)) // Y 要顯示的欄位名稱, 可設定多個.
                ;
            this._matrix.IsXColEditable = false;  // X 顯示的欄位可否編輯?
            this._matrix.IsYColEditable = false;  // Y 顯示的欄位可否編輯?
            #endregion

            string sqlcmd;
            if (this.Combo)
            {
                sqlcmd = string.Format(
                    @"
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
group by sizecode", this.CurrentDetailData["poid"]);
                DBProxy.Current.Select(null, sqlcmd, out dtIssueBreakdown);

                sqlcmd = string.Format(
                    @"
select distinct os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.POID='{0}'
order by seq", this.CurrentDetailData["poid"]);
                DBProxy.Current.Select(null, sqlcmd, out dtX);

                sqlcmd = string.Format(
                    @"
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
where o.POID = '{0}'", this.CurrentDetailData["poid"]);
                DBProxy.Current.Select(null, sqlcmd, out dtY);
            }
            else
            {
                sqlcmd = string.Format(
                    @"
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
group by sizecode", this.Orderid);
                DBProxy.Current.Select(null, sqlcmd, out dtIssueBreakdown);

                sqlcmd = string.Format(
                    @"
select os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.id = '{0}'
order by seq", this.Orderid);
                DBProxy.Current.Select(null, sqlcmd, out dtX);

                sqlcmd = string.Format(
                    @"
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
where id='{0}' ", this.Orderid);
                DBProxy.Current.Select(null, sqlcmd, out dtY);
            }

            this._matrix.Clear();
            if (!(result = this._matrix.Sets(dtIssueBreakdown, dtX, dtY)))
            {
                return result;  // 如果不是直接由資料庫載入, PR 自行處理資料來源, 再由 matrix.Set() 設定資料.
            }

            ((DataTable)this.listControlBindingSource1.DataSource).Rows[0].Delete();
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            string sqlcmd = $@"
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
where p.id='{this.CurrentDetailData["poid"]}' and p.seq1='{this.CurrentDetailData["seq1"]}' and p.seq2='{this.CurrentDetailData["seq2"]}'";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
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
            this.Orderid = this.Master["orderid"].ToString();
            DualResult result;
            if (!(result = this.Matrix_Reload()))
            {
                this.ShowErr(result);
            }

            string sql;
            if (this.Combo)
            {
                sql = $@"
select sizes = stuff((
	select concat(' or sizecode =''',sizecode,'''') 
	from(
        select distinct seq,os.sizecode
        from dbo.Order_SizeCode os WITH(NOLOCK)
        inner join orders o WITH(NOLOCK) on o.POID = os.Id
        inner join dbo.Order_Qty oq WITH(NOLOCK) on o.id = oq.ID and os.SizeCode = oq.SizeCode
        where o.POID = '{this.CurrentDetailData["poid"]}'
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
        where  o.id ='{this.Orderid}'
	)a
	for xml path('')
	),1,3,'')
";
            }

            string f = MyUtility.GetValue.Lookup(sql);
            this.gridbs.Filter = f;

            #endregion

            this.ComputeTotalQty();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            #region issueQtySet
            DataGridViewGeneratorNumericColumnSettings issueQtySet = new DataGridViewGeneratorNumericColumnSettings();
            issueQtySet.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr["Diffqty"] = MyUtility.Convert.GetDecimal(dr["Autopickqty"]) - MyUtility.Convert.GetDecimal(dr["Qty"]);
                dr.EndEdit();
                this.ComputeTotalQty();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Autopickqty", header: "Autopick qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: issueQtySet)
                .Numeric("Diffqty", header: "Diff. Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                ;

            this.ComputeTotalQty();

            this.grid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }

        private void ComputeTotalQty()
        {
            if (this.gridbs.DataSource != null)
            {
                this.displayTotalIssueQty.Value = MyUtility.Convert.GetDecimal(((DataTable)this.gridbs.DataSource).Compute("sum(Qty)", string.Empty));
                this.displayDiffqty.Value = MyUtility.Convert.GetDecimal(((DataTable)this.gridbs.DataSource).Compute("sum(Diffqty)", string.Empty));
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.IsSave = true;
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            this.IsSave = false;
        }
    }
}
