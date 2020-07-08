using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P10_CutRef : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P10;
        protected DataRow dr;

        public P10_CutRef(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += string.Format(" ({0})", this.dr["cutplanid"]);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"select b.POID
,c.SEQ1
,c.seq2
,b.CutRef
,c.FabricCombo
,b.CutNo
,(select x.article+',' from  (select distinct t.Article from dbo.WorkOrder_Distribute t WITH (NOLOCK) where t.WorkOrderUkey = c.Ukey) x for xml path('')) article
,C.Markername
from dbo.Cutplan a WITH (NOLOCK) 
inner join dbo.Cutplan_Detail b WITH (NOLOCK) on b.id= a.ID
inner join dbo.WorkOrder c WITH (NOLOCK) on c.Ukey = b.WorkorderUkey
where a.ID = '{0}'
order by b.POID,c.seq1,c.seq2,c.Cutno

", this.dr["cutplanid"].ToString()));

            DataTable selectDataTable1;
            this.P10.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P10.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridCutplanID.IsEditingReadOnly = true;
            this.gridCutplanID.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCutplanID)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Text("cutref", header: "Cut Ref#", width: Widths.AnsiChars(10))
                 .Text("CutNo", header: "Cut#", width: Widths.AnsiChars(4))
                 .Text("FabricCombo", header: "Comb", width: Widths.AnsiChars(4))
                 .Text("Article", header: "Article", width: Widths.AnsiChars(50))
                 .Text("Markername", header: "Marker name", width: Widths.AnsiChars(13))

                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
