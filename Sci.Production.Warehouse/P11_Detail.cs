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

namespace Sci.Production.Warehouse
{
    public partial class P11_Detail : Sci.Win.Subs.Input8A
    {
        Sci.Win.MatrixHelper _matrix;
        String Orderid;

        public DataRow master
        {
            get;
            set;
        }

        public P11_Detail()
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_DetailUkey";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        private DualResult matrix_Reload()
        {
            DualResult result;
            DataTable dtIssueBreakdown, dtX, dtY;
            #region -- matrix breakdown setting
            Sci.Win.UI.ListControlBindingSource gridbsBreakdown = new Sci.Win.UI.ListControlBindingSource();
            gridBreakDown.DataSource = gridbsBreakdown;

            _matrix = new Sci.Win.MatrixHelper(this, gridBreakDown, gridbsBreakdown); // 建立 Matrix 物件
            //_matrix.XTableName = "issue_breakdown";  // X 軸表格名稱
            //_matrix.YTableName = "issue_breakdown";  // Y 軸表格名稱
            //_matrix.TableName = "issue_breakdown";   // 第三表格名稱

            _matrix.XMap.Name = "sizecode";  // 對應到第三表格的 X 欄位名稱
            _matrix.XOrder = "seq";
            //_matrix.XMap.MapName = "SIZECODE";//ignore
            _matrix.YMap.Name = "article";  // 對應到第三表格的 Y 欄位名稱
            //_matrix.YMap.MapName = "SIZEITEM";//ignore

            //_matrix.XUniqueKey = "sizecode"; // X 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.
            //_matrix.YUniqueKey = "orderid,article"; // Y 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.

            _matrix
                .SetColDef("qty2", width: Widths.AnsiChars(4))  // 第三表格對應的欄位名稱
                .AddXColDef("sizecode")                             // X 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("total", header: "Total", width: Widths.AnsiChars(13))
                .AddYColDef("article", header: "Article", width: Widths.AnsiChars(8))  // Y 要顯示的欄位名稱, 可設定多個.
                //.AddYColDef("DESCRIPTION", header: "Description", width: Widths.UnicodeChars(15))
                ;

            _matrix.IsXColEditable = false;  // X 顯示的欄位可否編輯?
            _matrix.IsYColEditable = false;  // Y 顯示的欄位可否編輯?

            //_matrix.AutoField = "ID";   //映對到第三表格的欄位名稱
            //_matrix.AutoXField = "ID";  //映對到 X 表格的欄位名稱
            //_matrix.AutoYField = "ID";  //映對到 Y 表格的欄位名稱


            #endregion
            DBProxy.Current.Select(null, string.Format(@"select '' as Total, article,sizecode,convert(varchar,qty) as qty2 from dbo.Order_Qty where id = '{0}'
                                                        union all 
                                                        select '' as Total,'TTL' ,sizecode ,convert(varchar,sum(qty)) qty2 from dbo.Order_Qty where id='{0}' 
                                                        group by sizecode", Orderid), out dtIssueBreakdown);
            DBProxy.Current.Select(null, string.Format("select * from dbo.Order_SizeCode where id = (select poid from dbo.orders where id='{0}') order by seq", Orderid), out dtX);
            DBProxy.Current.Select(null, string.Format(@"select sum(qty) Total,article from dbo.Order_Qty where id = '{0}' group by article
                                                         union all
                                                         select sum(qty) Total,'TTL' from dbo.Order_qty where id='{0}' ",Orderid), out dtY);
            _matrix.Clear();

            if (!(result = _matrix.Sets(dtIssueBreakdown, dtX, dtY))) return result;  // 如果不是直接由資料庫載入, PR 自行處理資料來源, 再由 matrix.Set() 設定資料.

            return Result.True;
        }

        //protected override void OnSubDetailInsert(int index = -1)
        //{
        //    var frm = new Sci.Production.Warehouse.P10_Detail_Detail(CurrentDetailData, (DataTable)gridbs.DataSource);
        //    frm.ShowDialog(this);
        //    //base.OnSubDetailInsert(index);
        //    //CurrentSubDetailData["Issue_SummaryUkey"] = 0;
        //}

        protected override void OnAttached()
        {
            base.OnAttached();
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format(@"select *,left(seq1+'   ',3)+seq2 as seq,dbo.getmtldesc(id,seq1,seq2,2,0) [description]
,(select orderid+',' from (select orderid from dbo.po_supp_detail_orderlist where id=po_supp_detail.id and seq1=po_supp_detail.seq1 and seq2=po_supp_detail.seq2)t for xml path('')) [orderlist] 
from dbo.po_supp_detail where id='{0}' and seq1='{1}' and seq2='{2}'"
                , CurrentDetailData["poid"], CurrentDetailData["seq1"], CurrentDetailData["seq2"]), out dr))
            {
                this.dis_seq.Text = dr["seq"].ToString();
                this.dis_sizeunit.Text = dr["sizeunit"].ToString();
                this.dis_usedqty.Text = dr["usedqty"].ToString();
                this.dis_sizespec.Text = dr["sizespec"].ToString();
                this.dis_colorid.Text = dr["colorid"].ToString();
                this.dis_special.Text = dr["special"].ToString();
                this.eb_orderlist.Text = dr["orderlist"].ToString();
                this.eb_desc.Text = dr["description"].ToString();
            }

            #region -- matrix breakdown
            Orderid = master["orderid"].ToString();
            DualResult result;
            if (!(result = matrix_Reload()))
            {
                ShowErr(result);
            }
            #endregion
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                //.Text("id", header: "id", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
                //.Numeric("Issue_SummaryUkey", header: "Issue_SummaryUkey", width: Widths.AnsiChars(8), integer_places: 10)    //6
            .Text("Issue_detailUkey", header: "Ukey", width: Widths.AnsiChars(8), iseditingreadonly: true)    //0
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(10), iseditingreadonly: true)  //1
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8)    //2
            ;     //

            this.grid.Columns[2].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }
    }
}
