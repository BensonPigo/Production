using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P11_AutoPick_Detail : Sci.Win.Subs.Base
    {
        DataTable dt_detail;

        Sci.Production.Warehouse.P11_AutoPick P11Autopick = null;
        Sci.Win.MatrixHelper _matrix;
        bool combo;
        string poid;
        string orderid;
        DataTable dt;
        int DataRowIndex;
        int DataColumIndex;
        public Dictionary<DataRow, DataTable> dictionaryDatas = new Dictionary<DataRow, DataTable>();
        protected Sci.Win.UI.ListControlBindingSource gridbs = new Win.UI.ListControlBindingSource();

        protected DataTable dtArtwork;

        public P11_AutoPick_Detail(bool _combo, string _poid, string _orderid, DataTable _dt, int _DataRowIndex, int _DataColumIndex, Sci.Production.Warehouse.P11_AutoPick _P11Autopick)
        {
            this.combo = _combo; // 上面GRID使用
            this.poid = _poid; // 上面GRID使用
            this.orderid = _orderid; // 上面GRID使用
            this.dt = _dt;
            this.DataRowIndex = _DataRowIndex;
            this.DataColumIndex = _DataColumIndex;
            this.P11Autopick = _P11Autopick;
            this.InitializeComponent();
        }

        public void SetRightGrid(DataTable datas)
        {
            if (datas != null)
            {
                this.gridbs.DataSource = null;
            }

            // datas.AcceptChanges();
            this.dt_detail = datas;
            this.gridbs.DataSource = datas;

            string[] sary = new string[this.dtX.Rows.Count];
            for (int i = 0; i < this.dtX.Rows.Count; i++)
            {
                sary[i] = "sizecode = '" + MyUtility.Convert.GetString(this.dtX.Rows[i]["sizecode"]) + "'";
            }

            string sc = string.Join(" or ", sary);

            this.gridbs.Filter = sc;
            this.gridbs.MoveFirst();
        }

        public void SetDisplayBox(string strpoid, string strseq1, string strseq2)
        {
            DataRow dr;
            if (MyUtility.Check.Seek(
                string.Format(
                @"
select  p.*
        , concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2) as seq
        , dbo.getmtldesc(p.id, p.seq1, p.seq2, 2,0) [description]
        , (select orderid+',' from (select orderid 
                                    from dbo.po_supp_detail_orderlist WITH (NOLOCK) 
                                    where   id = p.id 
                                            and seq1 = p.seq1 
                                            and seq2 = p.seq2)t for xml path('')) [orderlist]
from dbo.po_supp_detail p WITH (NOLOCK) 
where id='{0}' and seq1='{1}' and seq2='{2}'",
                strpoid, strseq1, strseq2), out dr))
            {
                this.displySeqNo.Text = dr["seq"].ToString();
                this.displyUnit.Text = dr["StockUnit"].ToString();
                this.displyQty.Text = dr["usedqty"].ToString();
                this.displySize.Text = dr["sizespec"].ToString();
                this.displyColorid.Text = dr["colorid"].ToString();
                this.displySpecial.Text = dr["special"].ToString();
                this.editOrderList.Text = dr["orderlist"].ToString();
                this.eb_desc.Text = dr["description"].ToString();

                // Rate = Convert.ToDecimal(dr["RATE"].ToString());
            }
        }

        DataTable dtIssueBreakdown;
        DataTable dtX;
        DataTable dtY;

        public void SetTopGrid()
        {
            #region -- matrix breakdown setting
            this.gridBreakDown.DataSource = this.listControlBindingSource1;
            this._matrix = new Sci.Win.MatrixHelper(this, this.gridBreakDown, this.listControlBindingSource1); // 建立 Matrix 物件
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
            if (this.combo)
            {
                DBProxy.Current.Select(null, string.Format(
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
group by sizecode", this.poid), out this.dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format(
                    @"
select distinct os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.POID='{0}'
order by seq", this.poid), out this.dtX);
                DBProxy.Current.Select(null, string.Format(
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
where o.POID = '{0}'", this.poid), out this.dtY);
            }
            else
            {
                DBProxy.Current.Select(null, string.Format(
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
group by sizecode", this.orderid), out this.dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format(
                    @"
select os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.id = '{0}'
order by seq", this.orderid), out this.dtX);
                DBProxy.Current.Select(null, string.Format(
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
where id='{0}' ", this.orderid), out this.dtY);
            }

            this._matrix.Clear();
            this._matrix.Sets(this.dtIssueBreakdown, this.dtX, this.dtY);
            ((DataTable)this.listControlBindingSource1.DataSource).Rows[0].Delete();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridAutoPickDetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridAutoPickDetail.DataSource = this.gridbs;
            #region issueQtySet
            DataGridViewGeneratorNumericColumnSettings issueQtySet = new DataGridViewGeneratorNumericColumnSettings();
            issueQtySet.CellValidating += (s, e) =>
            {
                this.gridAutoPickDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.FormattedValue;
                if (e.ColumnIndex == 1)
                {
                    this.computeTotalIssueQty();
                }
            };
            #endregion
            /*
             * 請注意 如果以後 gridAutoPickDetail 有追加欄位
             * 1. 請確認 computTotalIssueQty => Cells 所指定的欄位是 QTY
             * 2. 確認 issueQtySet 中 ColumnIndex 判斷的是 QTY 欄位
            */
            this.Helper.Controls.Grid.Generator(this.gridAutoPickDetail)
                .Text("sizecode", header: "SizeCode", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("qty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: issueQtySet)
                ;
            #region computTotalIssueQty
            this.computeTotalIssueQty();
            #endregion
            this.gridAutoPickDetail.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        bool isSaved = false;

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SetRightGrid(this.P11Autopick.getAutoDetailDataTable(this.DataRowIndex));
            DataRow tmpDt = this.P11Autopick.getAutoDetailDataRow(this.DataRowIndex);
            this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

            // DataRowIndex//要改變原本表單的資料 要現在的索引
            this.P11Autopick.sum_subDetail(this.P11Autopick.getNeedChangeDataRow(this.DataRowIndex), this.P11Autopick.getAutoDetailDataTable(this.DataRowIndex));

            this.P11Autopick.BOA_PO.AcceptChanges();
            this.isSaved = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.P11Autopick.BOA_PO.RejectChanges();
            this.Close();
        }

        private void P11_AutoPick_Detail_Load(object sender, EventArgs e)
        {
            this.SetTopGrid();

            this.SetRightGrid(this.P11Autopick.getAutoDetailDataTable(this.DataRowIndex));

            DataRow tmpDt = this.P11Autopick.getAutoDetailDataRow(this.DataRowIndex);
            this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());
        }

        // 下一筆
        private void button1_Click(object sender, EventArgs e)
        {
            this.DataRowIndex = this.DataRowIndex + 1;
            if (this.DataRowIndex > this.dt.Rows.Count - 1)
            {
                this.DataRowIndex = this.dt.Rows.Count - 1;
                MyUtility.Msg.WarningBox("Last data already.");
            }
            else
            {
                this.SetRightGrid(this.P11Autopick.getAutoDetailDataTable(this.DataRowIndex));
                DataRow tmpDt = this.P11Autopick.getAutoDetailDataRow(this.DataRowIndex);
                this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

                // DataRowIndex = DataRowIndex - 1;//要改變原本表單的資料 要退回上一筆索引
                this.P11Autopick.sum_subDetail(this.P11Autopick.getNeedChangeDataRow(this.DataRowIndex - 1), this.P11Autopick.getAutoDetailDataTable(this.DataRowIndex - 1));
            }
            #region 重新計算 Total Issue Qty
            this.computeTotalIssueQty();
            #endregion
        }

        // 上一筆
        private void button2_Click(object sender, EventArgs e)
        {
            this.DataRowIndex = this.DataRowIndex - 1; // 上一筆索引-1

            if (this.DataRowIndex == -1)
            {
                this.DataRowIndex = 0;
                MyUtility.Msg.WarningBox("First data already.");
            }
            else
            {
                this.SetRightGrid(this.P11Autopick.getAutoDetailDataTable(this.DataRowIndex));
                DataRow tmpDt = this.P11Autopick.getAutoDetailDataRow(this.DataRowIndex);
                this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

                // DataRowIndex = DataRowIndex +1;//要改變原本表單的資料 要退回上一筆索引
                this.P11Autopick.sum_subDetail(this.P11Autopick.getNeedChangeDataRow(this.DataRowIndex + 1), this.P11Autopick.getAutoDetailDataTable(this.DataRowIndex + 1));
            }
            #region 重新計算 Total Issue Qty
            this.computeTotalIssueQty();
            #endregion
        }

        private void P11_AutoPick_Detail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.isSaved)
            {
                this.P11Autopick.BOA_PO.RejectChanges();
                this.P11Autopick.dictionaryDatasRejectChanges();
            }

            this.listControlBindingSource1.DataSource = null;
            this.gridbs.DataSource = null;
        }

        private void computeTotalIssueQty()
        {
            /*
             * 請注意 如果以後 gridAutoPickDetail 有追加欄位
             * 請把確認 Cells 所指定的欄位是 QTY
            */
            decimal totalQty = 0;
            for (int i = 0; i < this.gridAutoPickDetail.Rows.Count; i++)
            {
                totalQty += decimal.Parse(this.gridAutoPickDetail.Rows[i].Cells[1].Value.ToString());
            }

            this.displayTotalIssueQty.Value = totalQty;
        }
    }
}
