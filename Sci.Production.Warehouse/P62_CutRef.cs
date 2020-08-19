using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P62_CuttingRef : Sci.Win.Subs.Base
    {
        public Win.Tems.Base P62;
        protected DataRow dr;

        public P62_CuttingRef(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += string.Format(" ({0})", this.dr["cutplanid"]);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(
                $@"
select ct2.*,Article = art.Article 
from CutTapePlan_Detail ct2
inner join Issue i on ct2.ID = i.CutplanID
inner join Issue_Summary ism on ism.Id=i.Id
and ct2.ColorID = ism.Colorid and ct2.Seq1 = ism.seq1 and ct2.Seq2 = ism.seq2
outer apply ( 
	select Article = STUFF((
		Select distinct CONCAT('/ ', tmpa.Article)
		From Order_EachCons_Color tmpB WITH (NOLOCK) 
		left join Order_EachCons_Color_Article tmpA WITH (NOLOCK) on tmpa.Order_EachCons_ColorUkey = tmpb.Ukey
		Where  tmpb.Order_EachConsUkey =ct2.Order_EachConsUkey and tmpb.ColorID = ct2.ColorID
		For XML path('')
	),1,2,'')
) art
where ct2.ID='{this.dr["cutplanid"]}'
order by ct2.FabricCombo,ct2.MarkerName,ct2.Seq1,ct2.Seq2
");

            DataTable selectDataTable1;
            this.P62.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P62.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridCutplanID.IsEditingReadOnly = true;
            this.gridCutplanID.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCutplanID)
           .Text("FabricCombo", header: "Fab Combo", width: Widths.Auto(), iseditingreadonly: true)
           .Text("MarkerName", header: "Mark Name", width: Widths.Auto(), iseditingreadonly: true)
           .Text("SEQ1", header: "SEQ1", width: Widths.Auto(), iseditingreadonly: true)
           .Text("SEQ2", header: "SEQ2", width: Widths.Auto(), iseditingreadonly: true)
           .Text("Article", header: "Article", width: Widths.Auto(), iseditingreadonly: true)
           .Text("Colorid", header: "Color", width: Widths.Auto(), iseditingreadonly: true)
           .Text("direction", header: "Type of Cutting", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("CuttingWidth", header: "Cut Width", width: Widths.Auto(), iseditingreadonly: true)
           .Numeric("Qty", header: "Qty", width: Widths.Auto(), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
           .Numeric("ReleaseQty", header: "Release Qty", width: Widths.Auto(), integer_places: 5, decimal_places: 2)
           .Text("Dyelot", header: "Dyelot", width: Widths.Auto())
           .Text("Refno", header: "Refno", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Numeric("ConsPC", header: "Cons", width: Widths.Auto(), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
           .Text("Remark", header: "Remark", width: Widths.AnsiChars(25))
           ;
        }

        private void BtnClose_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
