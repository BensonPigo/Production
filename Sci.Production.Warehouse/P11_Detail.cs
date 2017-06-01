﻿using System;
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
        Decimal Rate;
        public bool combo;
        bool openFromAutoPick = false;
        public DataRow master
        {
            get;
            set;
        }
        public DataRow parentData;

        public P11_Detail(bool openFromAutoPick = false)
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_DetailUkey";
            this.openFromAutoPick = openFromAutoPick;
            
        }
        protected override bool OnSave()
        {
            if (!openFromAutoPick)
                return base.OnSave();
            else 
            {
                return true;  
            }
        }
        protected override void OnSaveAfter()
        {
            
            base.OnSaveAfter();
            parentData["output"] = string.Join(" , ",
                    this.CurrentSubDetailDatas
                    .AsEnumerable()
                    .Where(row=> !MyUtility.Check.Empty(row["Qty"]) )
                    .Select(row=> row["SizeCode"].ToString()+"*"+row["Qty"].ToString())
                
                );
            Decimal usedqty;
            if (Convert.ToDecimal(displyQty.Text)!=0) 
            {
                usedqty = Convert.ToDecimal(displyQty.Text);
            }
            else 
            {
                usedqty = 1;
            }

            parentData["qty"] = Math.Round(this.CurrentSubDetailDatas
                                            .AsEnumerable()
                                            .Where(row=> !MyUtility.Check.Empty(row["Qty"]) )
                                            .Sum(row => Convert.ToDecimal(row["Qty"].ToString())) * usedqty * Rate
                                            , 2);
            //將需新增的資料狀態更改為新增
            foreach (DataRow temp in CurrentSubDetailDatas.Rows)
            {
                if (temp["isvirtual"].ToString() == "1" && Convert.ToDecimal(temp["QTY"].ToString()) > 0)
                {
                    temp.AcceptChanges();
                    temp.SetAdded();
                }
            }
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
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
            //_matrix.XTableName = "issue_breakdown";  // X 軸表格名稱
            //_matrix.YTableName = "issue_breakdown";  // Y 軸表格名稱
            //_matrix.TableName = "issue_breakdown";   // 第三表格名稱

            _matrix.XMap.Name = "sizecode";  // 對應到第三表格的 X 欄位名稱
            _matrix.XOrder = "seq";
            //_matrix.XMap.MapName = "SIZECODE";//ignore
            _matrix.YMap.Name = "ID";  // 對應到第三表格的 Y 欄位名稱
            //_matrix.YMap.MapName = "SIZEITEM";//ignore

            //_matrix.XUniqueKey = "sizecode"; // X 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.
            //_matrix.YUniqueKey = "orderid,article"; // Y 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.

            _matrix
                .SetColDef("qty2", width: Widths.AnsiChars(4))  // 第三表格對應的欄位名稱
                .AddXColDef("sizecode")                             // X 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("ID", header: "OrderID", width: Widths.AnsiChars(16))
                .AddYColDef("total", header: "Total", width: Widths.AnsiChars(8))               
                .AddYColDef("article", header: "Article", width: Widths.AnsiChars(8))  // Y 要顯示的欄位名稱, 可設定多個.
                //.AddYColDef("DESCRIPTION", header: "Description", width: Widths.UnicodeChars(15))
                ;
            _matrix.IsXColEditable = false;  // X 顯示的欄位可否編輯?
            _matrix.IsYColEditable = false;  // Y 顯示的欄位可否編輯?

            //_matrix.AutoField = "ID";   //映對到第三表格的欄位名稱
            //_matrix.AutoXField = "ID";  //映對到 X 表格的欄位名稱
            //_matrix.AutoYField = "ID";  //映對到 Y 表格的欄位名稱


            #endregion
            if (combo)
            {
                DBProxy.Current.Select(null, string.Format(@"select '' as Total,o.id, oq.article,oq.sizecode,convert(varchar,oq.qty) as qty2 from dbo.Orders o WITH (NOLOCK) inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID where o.POID = '{0}'
                                                        union all 
                                                        select '' as Total,'','TTL' ,oq.sizecode ,convert(varchar,sum(oq.Qty)) qty2 from dbo.Orders o WITH (NOLOCK) inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID where o.POID = '{0}'
                                                        group by sizecode", CurrentDetailData["poid"]), out dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format("select * from dbo.Order_SizeCode WITH (NOLOCK) where id = (select poid from dbo.orders WITH (NOLOCK) where id='{0}') order by seq", CurrentDetailData["poid"]), out dtX);
                DBProxy.Current.Select(null, string.Format(@"select sum(oq.qty) Total,oq.article,o.ID from dbo.Orders o WITH (NOLOCK) inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID where o.POID = '{0}' group by O.ID,Article 
                                                         union all
                                                         select sum(oq.qty) Total,'TTL','' from dbo.Orders o WITH (NOLOCK) inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID where o.POID = '{0}'", CurrentDetailData["poid"]), out dtY);
            }
            else
            {
                DBProxy.Current.Select(null, string.Format(@"select '' as Total,ID, article,sizecode,convert(varchar,qty) as qty2 from dbo.Order_Qty WITH (NOLOCK) where id = '{0}'
                                                        union all 
                                                        select '' as Total,'','TTL' ,sizecode ,convert(varchar,sum(qty)) qty2 from dbo.Order_Qty WITH (NOLOCK) where id='{0}' 
                                                        group by sizecode", Orderid), out dtIssueBreakdown);
                DBProxy.Current.Select(null, string.Format("select * from dbo.Order_SizeCode WITH (NOLOCK) where id = (select poid from dbo.orders WITH (NOLOCK) where id='{0}') order by seq", Orderid), out dtX);
                DBProxy.Current.Select(null, string.Format(@"select sum(qty) Total,'{0}' as ID,article from dbo.Order_Qty WITH (NOLOCK) where id = '{0}' group by article
                                                         union all
                                                         select sum(qty) Total,'','TTL' from dbo.Order_qty WITH (NOLOCK) where id='{0}' ", Orderid), out dtY);
            }
            _matrix.Clear();          
            if (!(result = _matrix.Sets(dtIssueBreakdown, dtX, dtY))) return result;  // 如果不是直接由資料庫載入, PR 自行處理資料來源, 再由 matrix.Set() 設定資料.
            ((DataTable)this.listControlBindingSource1.DataSource).Rows[0].Delete();  
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
            if (MyUtility.Check.Seek(string.Format(@"
select  *
        , concat(Ltrim(Rtrim(seq1)), ' ', seq2) as seq
        , dbo.getmtldesc(id,seq1,seq2,2,0) [description]
        , (select orderid+',' from (select orderid 
                                    from dbo.po_supp_detail_orderlist WITH (NOLOCK) 
                                    where   id=po_supp_detail.id 
                                            and seq1=po_supp_detail.seq1 
                                            and seq2=po_supp_detail.seq2)t for xml path('')) [orderlist]
        , dbo.GetUnitRate(po_supp_detail.POUnit, po_supp_detail.StockUnit) RATE 
from dbo.po_supp_detail WITH (NOLOCK) 
where id='{0}' and seq1='{1}' and seq2='{2}'"
                , CurrentDetailData["poid"], CurrentDetailData["seq1"], CurrentDetailData["seq2"]), out dr))
            {
                this.displySeqNo.Text = dr["seq"].ToString();
                this.displyUnit.Text = dr["sizeunit"].ToString();
                this.displyQty.Text = dr["usedqty"].ToString();
                this.displySize.Text = dr["sizespec"].ToString();
                this.displyColorid.Text = dr["colorid"].ToString();
                this.displySpecial.Text = dr["special"].ToString();
                this.editOrderList.Text = dr["orderlist"].ToString();
                this.eb_desc.Text = dr["description"].ToString();
                Rate = Convert.ToDecimal(dr["RATE"].ToString());
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
            //.Text("Issue_detailUkey", header: "Ukey", width: Widths.AnsiChars(8), iseditingreadonly: true)    //0
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(10), iseditingreadonly: true)  //1
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8)    //2
            ;     //

            this.grid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }

    }
}
