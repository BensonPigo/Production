using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Cutting
{
    public partial class P01_MarkerList : Sci.Win.Subs.Input4Plus
    {
        DataRow MasterData;

        public P01_MarkerList(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string tablename, DataRow MasterData)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();

            this.DetailGridAlias = "Order_MarkerList_SizeQty";
            this.DetailKeyField = "Order_MarkerListUkey";
            this.MasterData = MasterData;
        }

        protected override DualResult OnRequery(out DataTable datas)
        {
            datas = null;
            string sqlCmd;

            sqlCmd = string.Format(
                @"
Select mark.*
,OE.Article ForArticle
,b.Description,b.Width as FabricWidth
,OFC.PatternPanel PatternPanel
,concat(rtrim(mark.MarkerUpdateName),' ', format(mark.MarkerUpdate,'yyyy/MM/dd HH:mm:ss')) MarkerUpdate2
,concat(mark.AddName,' ', format(mark.AddDate,'yyyy/MM/dd HH:mm:ss')) createby2
,concat(mark.EditName,' ', format(mark.EditDate,'yyyy/MM/dd HH:mm:ss')) editby2
from dbo.Order_MarkerList mark WITH (NOLOCK) 
left join dbo.Order_BOF a WITH (NOLOCK) on mark.Id = a.Id and mark.FabricCode = a.FabricCode  
left join dbo.Fabric b WITH (NOLOCK) on b.SCIRefno = a.SCIRefno  
left join Order_EachCons OE WITH (NOLOCK) 
    on mark.Id=OE.Id and mark.MarkerNo=OE.MarkerNo and mark.MarkerName=OE.MarkerName 
    and mark.FabricCode=OE.FabricCode and mark.FabricCombo=OE.FabricCombo 
    and mark.FabricPanelCode=OE.FabricPanelCode 
left join Order_FabricCode OFC WITH (NOLOCK) 
    on OFC.Id=mark.Id and OFC.FabricPanelCode=mark.FabricPanelCode and OFC.FabricCode=mark.FabricCode
Where mark.id ='{0}' order by mark.Seq",
                this.KeyValue1);

            Ict.DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out datas)))
            {
                return result;
            }

            return Result.True;
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Markername", header: "Marker" + Environment.NewLine + "name", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("FabricCombo", header: "Fabric" + Environment.NewLine + "Combo", width: Widths.AnsiChars(2), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5))
                .Numeric("Qty", header: "Qty", width: Widths.Numeric(6), maximum: 999999, minimum: 0, decimal_places: 0);
            return true;
        }

        protected override void OnGridRowChanged()
        {
            base.OnGridRowChanged();
            this.SumSizeQty();
        }

        protected override void OnDetailGridRowChanged()
        {
            base.OnDetailGridRowChanged();
            this.SumSizeQty();
        }

        private void SumSizeQty()
        {
            // 計算每個Marker 的總裁剪訂單數
            DataTable detailGrid = (DataTable)this.detailgridbs.DataSource;
            int rowid = this.grid.GetSelectedRowIndex();
            DataRowView dr = this.grid.GetData<DataRowView>(rowid);
            if (dr != null && detailGrid != null)
            {
                object totalSizeQty = detailGrid.Compute("sum(Qty)", string.Empty);
                this.displayTotal.Value = totalSizeQty.ToString();
            }
        }
    }
}
