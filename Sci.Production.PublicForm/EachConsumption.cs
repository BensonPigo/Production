using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;

#pragma warning disable 0414
namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class EachConsumption : Win.Subs.Input4Plus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EachConsumption"/> class.
        /// </summary>
        public EachConsumption()
        {
            this.InitializeComponent();
        }

        private DataTable sizetb;
        private string _isCuttingPiece;
        private int _cuttingPiece;

        /// <summary>
        /// inner class.
        /// </summary>
        private class SizeQtyInfo
        {
            public DataRow Master { get; set; } = null;

            public DataTable SizeQty { get; set; } = null;
        }

        // ITableSchema _sizeQtyTableschema;
        // int _isSizeQtyGridCurrentChanging=0;

        /// <summary>
        /// Internal used.
        /// </summary>
        private IDictionary<DataRow, SizeQtyInfo> _sizeQtyInfos = new Dictionary<DataRow, SizeQtyInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EachConsumption"/> class.
        /// </summary>
        /// <param name="canedit">Can Edit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        /// <param name="cuttingPiece">Cutting Piece</param>
        /// <param name="switchToWorkorder">Switch To Workorder</param>
        /// <param name="canSwitch">Can Switch</param>
        public EachConsumption(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, bool cuttingPiece, bool switchToWorkorder, bool canSwitch)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Text = "Each Consumption - (" + keyvalue1 + ")";
            this.btnSwitchtoWorkOrder.Visible = canSwitch;
            this._isCuttingPiece = "1";
            this._cuttingPiece = 0;
            if (cuttingPiece)
            {
                this._isCuttingPiece = "2";
                this._cuttingPiece = 1;
            }

            bool editSwitch2Order = Prgs.GetAuthority(Env.User.UserID, "P01.Cutting Master List", "CanEdit");
            if (editSwitch2Order && switchToWorkorder)
            {
                this.btnSwitchtoWorkOrder.Enabled = true;
            }
            else
            {
                this.btnSwitchtoWorkOrder.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery(out DataTable datas)
        {
            string sqlCmd = $@"
SELECT
    oec.*
   ,EachConsSource =
        CASE
            WHEN o.EachConsSource = 'O' THEN 'Original'
            WHEN o.EachConsSource = 'P' THEN 'Prophet'
            WHEN o.EachConsSource = '' THEN ''
        END
   ,oecp.PatternPanel
   ,ForArticle = oec.Article
   ,EC_Size.TotalQty
   ,FabricDesc = CONCAT(RTRIM(Fabric.Refno), '-', Fabric.Description)
   ,FabricWidth = Fabric.Width
   ,type2 =
        CASE
            WHEN oec.TYPE = '0' THEN ''
            WHEN oec.TYPE = 'C' THEN 'Cutting Piece'
            WHEN oec.TYPE = '1' THEN 'Cutting Piece'
            WHEN oec.TYPE = 'T' THEN 'TAPE'
            WHEN oec.TYPE = '2' THEN 'TAPE'
        END
   ,createby2 = CONCAT(oec.AddName, ' ', FORMAT(oec.AddDate, 'yyyy/MM/dd HH:mm:ss'))
   ,editby2 = CONCAT(oec.EditName, ' ', FORMAT(oec.EditDate, 'yyyy/MM/dd HH:mm:ss'))
   ,FabricKind = CONCAT(bof.kind, ':', DD.Name)
   ,MarkerTypeName = DropDownList.Name
FROM dbo.Order_EachCons oec WITH (NOLOCK)
INNER JOIN dbo.Orders o WITH (NOLOCK) ON oec.ID = o.ID
OUTER APPLY (
    SELECT PatternPanel = STUFF((
        SELECT DISTINCT ',' + PatternPanel
        FROM dbo.Order_EachCons_PatternPanel oecp WITH (NOLOCK)
        WHERE oecp.Order_EachConsUkey = oec.Ukey
        FOR XML PATH ('')),1,1,'')
) AS oecp
OUTER APPLY (
    SELECT TotalQty = SUM(Qty)
    FROM dbo.Order_EachCons_SizeQty WITH (NOLOCK)
    WHERE oec.Ukey = Order_EachConsUkey
) AS EC_Size
LEFT JOIN dbo.Order_BOF bof WITH (NOLOCK) ON bof.ID = oec.ID AND bof.FabricCode = oec.FabricCode
LEFT JOIN dbo.Fabric WITH (NOLOCK) ON Fabric.SCIRefno = bof.SCIRefno
LEFT JOIN DropDownList DD ON DD.ID = bof.Kind AND DD.Type = 'FabricKind'
LEFT JOIN DropDownList WITH (NOLOCK) ON DropDownList.ID = oec.MarkerType AND DropDownList.Type = 'MarkerType' 
WHERE oec.ID = '{this.KeyValue1}'
ORDER BY oec.Seq
";
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out datas)))
            {
                return result;
            }

            sqlCmd = string.Format("Select * from Order_EachCons_SizeQty WITH (NOLOCK) where id = '{0}'", this.KeyValue1);
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out this.sizetb)))
            {
                return result;
            }

            this.gridSizeQty.DataSource = this.sizetb;
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Markername", header: "Marker\r\n name", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("FabricCombo", header: "Fabric\r\n Combo", width: Widths.AnsiChars(2), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ColorID", header: "Color ID", width: Widths.AnsiChars(8))
                .Text("SizeList", header: "Ratio", width: Widths.AnsiChars(16))
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.Numeric(6), decimal_places: 0)
                .Numeric("Layer", header: "Layer", width: Widths.Numeric(5), decimal_places: 0)
                .Numeric("CutQty", header: "Cut Q'ty", width: Widths.Numeric(6), decimal_places: 0)
                .Numeric("Variance", header: "Variance", width: Widths.Numeric(6), decimal_places: 0)
                .Numeric("YDS", header: "Cons.(YDS)", width: Widths.Numeric(6),  decimal_places: 2);

            this.Helper.Controls.Grid.Generator(this.gridSizeQty)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5))
                .Numeric("Qty", header: "Qty", width: Widths.Numeric(4), decimal_places: 0);
            this.gridSizeQty.Font = new Font("Arial", 9);
            return true;
        }

        /// <inheritdoc/>
        protected override void OnGridRowChanged()
        {
            if (MyUtility.Check.Empty(this.CurrentData))
            {
                return;
            }

            this.sizetb.DefaultView.RowFilter = string.Format("Order_EachConsUkey = '{0}'", this.CurrentData["ukey"]);
            base.OnGridRowChanged();
            DataRow row = this.grid.GetDataRow(this.gridbs.Position);
            if (row == null)
            {
                return;
            }

            DataRow dr;
            if (MyUtility.Check.Seek(
                string.Format(
                @"
select Article = 
stuff( ( select distinct concat(',', Article )
from Order_EachCons_Article  
where id='{0}' and Order_EachConsUkey='{1}' 
for xml path('')),1,1,'')  ",
                row["id"],
                row["Ukey"]),
                out dr))
            {
                this.editForArticle.Text = dr["Article"].ToString();
            }
            else
            {
                this.editForArticle.Text = string.Empty;
            }

            this.detailgrid.AutoResizeColumns();
        }

        private void BtnDetail_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData;
            if (dr == null)
            {
                return;
            }

            var frm = new EachConsumption_Detail(false, dr["ID"].ToString(), dr["ukey"].ToString(), this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex())["colorid"].ToString());
            frm.ShowDialog(this);
        }

        private void BtnDownloadIdList_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData;
            if (dr == null)
            {
                return;
            }

            var frm = new EachConsumption_DownloadIdList(false, dr["ID"].ToString(), null, null);
            frm.ShowDialog(this);
        }

        private void BtnSwitchtoWorkOrder_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData;
            if (dr == null)
            {
                return;
            }

            var frm = new EachConsumption_SwitchWorkOrder(dr["ID"].ToString());
            frm.ShowDialog(this);
        }
    }
}
