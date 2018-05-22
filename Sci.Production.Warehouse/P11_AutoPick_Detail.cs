using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Threading;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P11_AutoPick_Detail : Sci.Win.Subs.Base
    {
        DataTable dt_detail;
        
        //
        Sci.Production.Warehouse.P11_AutoPick P11Autopick = null;
        Sci.Win.MatrixHelper _matrix;
        bool combo;
        string poid;
        string orderid;
        DataTable dt;
        int DataRowIndex;
        int DataColumIndex;
        public Dictionary<DataRow, DataTable> dictionaryDatas = new Dictionary<DataRow, DataTable>();
        //
        protected Sci.Win.UI.ListControlBindingSource gridbs = new Win.UI.ListControlBindingSource();

        protected DataTable dtArtwork;

        public P11_AutoPick_Detail(bool _combo, string _poid, string _orderid, DataTable _dt, int _DataRowIndex, int _DataColumIndex, Sci.Production.Warehouse.P11_AutoPick _P11Autopick)
        {
            combo = _combo;//上面GRID使用
            poid = _poid;//上面GRID使用
            orderid = _orderid;//上面GRID使用
            dt = _dt;
            DataRowIndex = _DataRowIndex;
            DataColumIndex = _DataColumIndex;
            P11Autopick = _P11Autopick;
            InitializeComponent();

        }

        public void SetRightGrid(DataTable datas)
        {
            if (null != datas)
            { 
                gridbs.DataSource = null;
            }
            //datas.AcceptChanges();
            string[] sary = new string[dtX.Rows.Count];
            for (int i = 0; i < dtX.Rows.Count; i++)
            {
                sary[i] = "sizecode = '" + MyUtility.Convert.GetString(dtX.Rows[i]["sizecode"])+"'";
            }
            string sc = string.Join(" or ", sary) ;
            DataTable datas2 = datas.Select(sc, "seq").CopyToDataTable();
            dt_detail = datas2;
            gridbs.DataSource = datas2;
            gridbs.MoveFirst();
        }
       
        public void SetDisplayBox(string strpoid,string strseq1,string strseq2)
        {
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format(@"
select  p.*
        , concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2) as seq
        , dbo.getmtldesc(p.id, p.seq1, p.seq2, 2,0) [description]
        , (select orderid+',' from (select orderid 
                                    from dbo.po_supp_detail_orderlist WITH (NOLOCK) 
                                    where   id = p.id 
                                            and seq1 = p.seq1 
                                            and seq2 = p.seq2)t for xml path('')) [orderlist]
from dbo.po_supp_detail p WITH (NOLOCK) 
where id='{0}' and seq1='{1}' and seq2='{2}'"
                , strpoid, strseq1, strseq2), out dr))
            {
                this.displySeqNo.Text = dr["seq"].ToString();
                this.displyUnit.Text = dr["StockUnit"].ToString();
                this.displyQty.Text = dr["usedqty"].ToString();
                this.displySize.Text = dr["sizespec"].ToString();
                this.displyColorid.Text = dr["colorid"].ToString();
                this.displySpecial.Text = dr["special"].ToString();
                this.editOrderList.Text = dr["orderlist"].ToString();
                this.eb_desc.Text = dr["description"].ToString();
                //Rate = Convert.ToDecimal(dr["RATE"].ToString());
            }
        }
        
        DataTable dtIssueBreakdown, dtX, dtY;
        public void SetTopGrid()
        {
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
group by sizecode", poid), out dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format(@"
select distinct os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.POID='{0}'
order by seq", poid), out dtX);
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
where o.POID = '{0}'", poid), out dtY);
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
group by sizecode", orderid), out dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format(@"
select os.* 
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.id = (
            select poid 
            from dbo.orders WITH (NOLOCK) 
            where id='{0}'
      ) 
order by seq", orderid), out dtX);
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
where id='{0}' ", orderid), out dtY);
            }
            _matrix.Clear();
            _matrix.Sets(dtIssueBreakdown, dtX, dtY);
            ((DataTable)this.listControlBindingSource1.DataSource).Rows[0].Delete();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            this.gridAutoPickDetail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridAutoPickDetail.DataSource = gridbs;
            #region issueQtySet
            DataGridViewGeneratorNumericColumnSettings issueQtySet = new DataGridViewGeneratorNumericColumnSettings();
            issueQtySet.CellValidating += (s, e) =>
            {
                gridAutoPickDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.FormattedValue;
                if (e.ColumnIndex == 1)
                {
                    computeTotalIssueQty();
                }
            };
            #endregion 
            /*
             * 請注意 如果以後 gridAutoPickDetail 有追加欄位
             * 1. 請確認 computTotalIssueQty => Cells 所指定的欄位是 QTY
             * 2. 確認 issueQtySet 中 ColumnIndex 判斷的是 QTY 欄位
            */
            Helper.Controls.Grid.Generator(this.gridAutoPickDetail)
                .Text("sizecode", header: "SizeCode", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("qty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: issueQtySet)
                ;
            #region computTotalIssueQty
            computeTotalIssueQty();
            #endregion 
            this.gridAutoPickDetail.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink; 
        }
        bool isSaved = false;
        private void btnSave_Click(object sender, EventArgs e)
        {
            
                SetRightGrid(P11Autopick.getAutoDetailDataTable(DataRowIndex));
                DataRow tmpDt = P11Autopick.getAutoDetailDataRow(DataRowIndex);
                SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

                //DataRowIndex//要改變原本表單的資料 要現在的索引
                P11Autopick.sum_subDetail(P11Autopick.getNeedChangeDataRow(DataRowIndex), P11Autopick.getAutoDetailDataTable(DataRowIndex));         

            //
            P11Autopick.BOA_PO.AcceptChanges();
            isSaved = true;
            this.Close();
               
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            P11Autopick.BOA_PO.RejectChanges();
            this.Close();
        }

        private void P11_AutoPick_Detail_Load(object sender, EventArgs e)
        {
            SetTopGrid();

            SetRightGrid(P11Autopick.getAutoDetailDataTable(DataRowIndex));

            DataRow tmpDt = P11Autopick.getAutoDetailDataRow(DataRowIndex);
            SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());           
        }

        //下一筆
        private void button1_Click(object sender, EventArgs e)
        {
            DataRowIndex = DataRowIndex + 1;
            if (DataRowIndex > dt.Rows.Count - 1)
            {
                DataRowIndex = dt.Rows.Count - 1;
                MyUtility.Msg.WarningBox("Last data already.");

            }
            else {
                SetRightGrid(P11Autopick.getAutoDetailDataTable(DataRowIndex));
                DataRow tmpDt = P11Autopick.getAutoDetailDataRow(DataRowIndex);
                SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());

                //DataRowIndex = DataRowIndex - 1;//要改變原本表單的資料 要退回上一筆索引
                P11Autopick.sum_subDetail(P11Autopick.getNeedChangeDataRow(DataRowIndex -1), P11Autopick.getAutoDetailDataTable(DataRowIndex-1));
            }
            #region 重新計算 Total Issue Qty
            computeTotalIssueQty();
            #endregion 
        }

        //上一筆
        private void button2_Click(object sender, EventArgs e)
        {
            DataRowIndex = DataRowIndex - 1; //上一筆索引-1

            if (DataRowIndex == -1)
            {
                DataRowIndex = 0;
                MyUtility.Msg.WarningBox("First data already.");

            }
            else {
                SetRightGrid(P11Autopick.getAutoDetailDataTable(DataRowIndex));
                DataRow tmpDt = P11Autopick.getAutoDetailDataRow(DataRowIndex);
                SetDisplayBox(tmpDt["Poid"].ToString(), tmpDt["seq1"].ToString(), tmpDt["seq2"].ToString());
                
                //DataRowIndex = DataRowIndex +1;//要改變原本表單的資料 要退回上一筆索引
                P11Autopick.sum_subDetail(P11Autopick.getNeedChangeDataRow(DataRowIndex +1), P11Autopick.getAutoDetailDataTable(DataRowIndex +1));                
            }
            #region 重新計算 Total Issue Qty
            computeTotalIssueQty();
            #endregion 
        }

        private void P11_AutoPick_Detail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved) {
                P11Autopick.BOA_PO.RejectChanges();
                P11Autopick.dictionaryDatasRejectChanges();
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
            for (int i = 0; i < gridAutoPickDetail.Rows.Count; i++)
            {
                totalQty += decimal.Parse(gridAutoPickDetail.Rows[i].Cells[1].Value.ToString());
            }
            this.displayTotalIssueQty.Value = totalQty;
        }
    }
}
