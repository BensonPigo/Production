using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P11_AutoPick_Detail : Win.Subs.Base
    {
        private P11_AutoPick P11Autopick = null;
        private Win.MatrixHelper _matrix;
        private bool combo;
        private string poid;
        private string orderid;
        private DataTable dt;
        private int DataRowIndex;
        private Win.UI.ListControlBindingSource gridbs = new Win.UI.ListControlBindingSource();

        /// <inheritdoc/>
        public P11_AutoPick_Detail(bool combo, string poid, string orderid, DataTable dt, int dataRowIndex, P11_AutoPick p11Autopick)
        {
            this.combo = combo; // 上面GRID使用
            this.poid = poid; // 上面GRID使用
            this.orderid = orderid; // 上面GRID使用
            this.dt = dt;
            this.DataRowIndex = dataRowIndex;
            this.P11Autopick = p11Autopick;
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public void SetRightGrid(DataTable datas)
        {
            if (datas != null)
            {
                this.gridbs.DataSource = null;
            }

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

        /// <inheritdoc/>
        public void SetDisplayBox(string strpoid, string strseq1, string strseq2)
        {
            string sqlcmd = string.Format(
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
                strpoid,
                strseq1,
                strseq2);

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
        }

        private DataTable dtIssueBreakdown;
        private DataTable dtX;
        private DataTable dtY;

        /// <inheritdoc/>
        public void SetTopGrid()
        {
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
            if (this.combo)
            {
                string sqlcmd = string.Format(
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
group by sizecode", this.poid);
                DBProxy.Current.Select(null, sqlcmd, out this.dtIssueBreakdown);

                sqlcmd = string.Format(
                    @"
select distinct os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.POID='{0}'
order by seq", this.poid);
                DBProxy.Current.Select(null, sqlcmd, out this.dtX);

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
where o.POID = '{0}'", this.poid);
                DBProxy.Current.Select(null, sqlcmd, out this.dtY);
            }
            else
            {
                string sqlcmd = string.Format(
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
group by sizecode", this.orderid);
                DBProxy.Current.Select(null, sqlcmd, out this.dtIssueBreakdown);

                sqlcmd = string.Format(
                    @"
select os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.id = '{0}'
order by seq", this.orderid);
                DBProxy.Current.Select(null, sqlcmd, out this.dtX);

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
where id='{0}' ", this.orderid);
                DBProxy.Current.Select(null, sqlcmd, out this.dtY);
            }

            this._matrix.Clear();
            this._matrix.Sets(this.dtIssueBreakdown, this.dtX, this.dtY);
            ((DataTable)this.listControlBindingSource1.DataSource).Rows[0].Delete();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridAutoPickDetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridAutoPickDetail.DataSource = this.gridbs;
            #region issueQtySet
            DataGridViewGeneratorNumericColumnSettings issueQtySet = new DataGridViewGeneratorNumericColumnSettings();
            issueQtySet.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridAutoPickDetail.GetDataRow<DataRow>(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr["Diffqty"] = MyUtility.Convert.GetDecimal(dr["Autopickqty"]) - MyUtility.Convert.GetDecimal(dr["Qty"]);
                dr.EndEdit();
                this.ComputeTotalQty();
            };
            #endregion
            this.Helper.Controls.Grid.Generator(this.gridAutoPickDetail)
                .Text("sizecode", header: "SizeCode", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("Autopickqty", header: "Autopick qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("qty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: issueQtySet)
                .Numeric("Diffqty", header: "Diff. Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                ;
            #region computTotalIssueQty
            this.ComputeTotalQty();
            #endregion
            this.gridAutoPickDetail.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private bool isSaved = false;

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.SetRightGrid(this.P11Autopick.GetAutoDetailDataTable(this.DataRowIndex));
            DataRow tmpDt = this.P11Autopick.GetAutoDetailDataRow(this.DataRowIndex);
            this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

            // DataRowIndex//要改變原本表單的資料 要現在的索引
            this.P11Autopick.Sum_subDetail(this.P11Autopick.GetNeedChangeDataRow(this.DataRowIndex), this.P11Autopick.GetAutoDetailDataTable(this.DataRowIndex));

            this.P11Autopick.BOA_PO.AcceptChanges();
            this.isSaved = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.P11Autopick.BOA_PO.RejectChanges();
            this.Close();
        }

        private void P11_AutoPick_Detail_Load(object sender, EventArgs e)
        {
            this.SetTopGrid();

            this.SetRightGrid(this.P11Autopick.GetAutoDetailDataTable(this.DataRowIndex));

            DataRow tmpDt = this.P11Autopick.GetAutoDetailDataRow(this.DataRowIndex);
            this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());
        }

        // 下一筆
        private void Button1_Click(object sender, EventArgs e)
        {
            this.DataRowIndex++;
            if (this.DataRowIndex > this.dt.Rows.Count - 1)
            {
                this.DataRowIndex = this.dt.Rows.Count - 1;
                MyUtility.Msg.WarningBox("Last data already.");
            }
            else
            {
                this.SetRightGrid(this.P11Autopick.GetAutoDetailDataTable(this.DataRowIndex));
                DataRow tmpDt = this.P11Autopick.GetAutoDetailDataRow(this.DataRowIndex);
                this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

                // DataRowIndex = DataRowIndex - 1;//要改變原本表單的資料 要退回上一筆索引
                this.P11Autopick.Sum_subDetail(this.P11Autopick.GetNeedChangeDataRow(this.DataRowIndex - 1), this.P11Autopick.GetAutoDetailDataTable(this.DataRowIndex - 1));
            }
            #region 重新計算 Total Issue Qty
            this.ComputeTotalQty();
            #endregion
        }

        // 上一筆
        private void Button2_Click(object sender, EventArgs e)
        {
            this.DataRowIndex--; // 上一筆索引-1

            if (this.DataRowIndex == -1)
            {
                this.DataRowIndex = 0;
                MyUtility.Msg.WarningBox("First data already.");
            }
            else
            {
                this.SetRightGrid(this.P11Autopick.GetAutoDetailDataTable(this.DataRowIndex));
                DataRow tmpDt = this.P11Autopick.GetAutoDetailDataRow(this.DataRowIndex);
                this.SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

                // DataRowIndex = DataRowIndex +1;//要改變原本表單的資料 要退回上一筆索引
                this.P11Autopick.Sum_subDetail(this.P11Autopick.GetNeedChangeDataRow(this.DataRowIndex + 1), this.P11Autopick.GetAutoDetailDataTable(this.DataRowIndex + 1));
            }
            #region 重新計算 Total Issue Qty
            this.ComputeTotalQty();
            #endregion
        }

        private void P11_AutoPick_Detail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.isSaved)
            {
                this.P11Autopick.BOA_PO.RejectChanges();
                this.P11Autopick.DictionaryDatasRejectChanges();
            }

            this.listControlBindingSource1.DataSource = null;
            this.gridbs.DataSource = null;
        }

        private void ComputeTotalQty()
        {
            this.displayTotalIssueQty.Value = MyUtility.Convert.GetDecimal(((DataTable)this.gridbs.DataSource).Compute("sum(Qty)", string.Empty));
            this.displayDiffqty.Value = MyUtility.Convert.GetDecimal(((DataTable)this.gridbs.DataSource).Compute("sum(Diffqty)", string.Empty));
        }
    }
}
