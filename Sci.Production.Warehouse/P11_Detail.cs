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
        public P11_Detail()
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_DetailUkey";
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
