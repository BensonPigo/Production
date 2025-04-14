using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P01_MarkerList : Win.Subs.Input4Plus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P01_MarkerList"/> class.
        /// </summary>
        /// <param name="canedit">Can Edit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        /// <param name="tablename">Table Name</param>
        /// <param name="masterData">MasterData</param>
        public P01_MarkerList(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string tablename, DataRow masterData)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();

            this.DetailGridAlias = "Order_MarkerList_SizeQty";
            this.DetailKeyField = "Order_MarkerListUkey";
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery(out DataTable datas)
        {
            string sqlCmd = $@"
SELECT
    mark.*
   ,ForArticle = oec.Article
   ,b.Description
   ,FabricWidth = b.Width
   ,PatternPanel = ofc.PatternPanel
   ,MarkerUpdate2 = CONCAT(RTRIM(mark.MarkerUpdateName), ' ', FORMAT(mark.MarkerUpdate, 'yyyy/MM/dd HH:mm:ss'))
   ,createby2 = CONCAT(mark.AddName, ' ', FORMAT(mark.AddDate, 'yyyy/MM/dd HH:mm:ss'))
   ,editby2 = CONCAT(mark.EditName, ' ', FORMAT(mark.EditDate, 'yyyy/MM/dd HH:mm:ss'))
   ,MarkerTypeName = DropDownList.Name
FROM Order_MarkerList mark WITH (NOLOCK)
LEFT JOIN Order_BOF obof WITH (NOLOCK) ON mark.ID = obof.ID AND mark.FabricCode = obof.FabricCode
LEFT JOIN dbo.Fabric b WITH (NOLOCK) ON b.SCIRefno = obof.SCIRefno
LEFT JOIN Order_EachCons oec WITH (NOLOCK)
    ON mark.ID = oec.ID
        AND mark.MarkerNo = oec.MarkerNo
        AND mark.MarkerName = oec.MarkerName
        AND mark.FabricCode = oec.FabricCode
        AND mark.FabricCombo = oec.FabricCombo
        AND mark.FabricPanelCode = oec.FabricPanelCode
LEFT JOIN Order_FabricCode ofc WITH (NOLOCK)
    ON ofc.ID = mark.ID
        AND ofc.FabricPanelCode = mark.FabricPanelCode
        AND ofc.FabricCode = mark.FabricCode
LEFT JOIN DropDownList WITH (NOLOCK) ON DropDownList.ID = mark.MarkerType AND DropDownList.Type = 'MarkerType'         
WHERE mark.ID = '{this.KeyValue1}'
ORDER BY mark.Seq
";
            return DBProxy.Current.Select(null, sqlCmd, out datas);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void OnGridRowChanged()
        {
            base.OnGridRowChanged();
            this.SumSizeQty();
        }

        /// <inheritdoc/>
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
