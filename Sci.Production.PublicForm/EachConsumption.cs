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
    public partial class EachConsumption : Win.Subs.Input4Plus
    {
        public EachConsumption()
        {
            this.InitializeComponent();
        }

        private DataTable sizetb;
        private String _isCuttingPiece;
        private int _cuttingPiece;

        /// <summary>
        /// inner class.
        /// </summary>
        class SizeQtyInfo
        {
            public DataRow master = null;
            public DataTable sizeQty = null;
        }

        // ITableSchema _sizeQtyTableschema;
        // int _isSizeQtyGridCurrentChanging=0;

        /// <summary>
        /// Internal used.
        /// </summary>
        IDictionary<DataRow, SizeQtyInfo> _sizeQtyInfos = new Dictionary<DataRow, SizeQtyInfo>();

        /// <summary>
        /// Internal used.
        /// </summary>
        // SizeQtyInfo _sizeQtyAttached;
        // private bool isSizeQtyGridCurrentChanging { get { return 0 < _isSizeQtyGridCurrentChanging; } }
       // private bool IsSizeQtyAttached { get { return null != _sizeQtyAttached; } }
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

            bool EditSwitch2Order = Prgs.GetAuthority(Sci.Env.User.UserID, "P01.Cutting Master List", "CanEdit");
            if (EditSwitch2Order && switchToWorkorder)
            {
                this.btnSwitchtoWorkOrder.Enabled = true;
            }
            else
            {
                this.btnSwitchtoWorkOrder.Enabled = false;
            }
        }

        protected override DualResult OnRequery(out DataTable datas)
        {
            // return base.OnRequery();
            datas = null;
            string sqlCmd = string.Format(
@"Select a.*, 
case
	when b.EachConsSource='O' then 'Original'
	when b.EachConsSource='P' then 'Prophet'
	when b.EachConsSource='' then ''
END
AS EachConsSource,

d.PatternPanel, 

a.Article as ForArticle,

EC_Size.TotalQty , 
concat(RTrim(Fabric.Refno),'-',Fabric.Description) as FabricDesc,
Fabric.Width as FabricWidth 
,type2 = (case when  a.TYPE = '0' then ''
	when a.TYPE = 'C' then 'Cutting Piece'
    when a.TYPE = '1' then 'Cutting Piece'
	when a.TYPE = 'T' then 'TAPE'
    when a.TYPE = '2' then 'TAPE'
	end) 
,createby2 = concat(a.AddName,' ' ,FORMAT(a.AddDate,'yyyy/MM/dd HH:mm:ss'))
,editby2 = concat(a.EditName,' ' , FORMAT(a.EditDate,'yyyy/MM/dd HH:mm:ss'))
,FabricKind = concat(bof.kind , ':', DD.Name)

From dbo.Order_EachCons a WITH (NOLOCK) 
Left Join dbo.Orders b WITH (NOLOCK) On a.ID = b.ID  

outer apply(
	select (
		select distinct PatternPanel+',' 
		From dbo.Order_EachCons_PatternPanel as tmp WITH (NOLOCK) 
		Where tmp.Order_EachConsUkey = a.Ukey
		for XML path('')
		)
	as PatternPanel
) d

outer apply(select sum(Qty) as TotalQty from  dbo.Order_EachCons_SizeQty WITH (NOLOCK) where  a.Ukey = Order_EachConsUkey  ) as EC_Size 
left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = a.Id and bof.FabricCode = a.FabricCode
left join dbo.Fabric WITH (NOLOCK) on Fabric.SCIRefno = bof.SCIRefno
left join DropDownList DD on DD.ID=bof.Kind and DD.Type='FabricKind' 
Where a.ID = '{0}' Order by a.Seq", this.KeyValue1);
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
            return Result.True;
        }

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
for xml path('')),1,1,'')  ", row["id"], row["Ukey"]), out dr))
            {
                this.editForArticle.Text = dr["Article"].ToString();
            }
            else
            {
                this.editForArticle.Text = string.Empty;
            }

            this.detailgrid.AutoResizeColumns();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData;
            if (dr == null)
            {
                return;
            }

            var frm = new EachConsumption_Detail(false, dr["ID"].ToString(), dr["ukey"].ToString(), this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex())["colorid"].ToString());
            frm.ShowDialog(this);
        }

        private void btnDownloadIdList_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData;
            if (dr == null)
            {
                return;
            }

            var frm = new EachConsumption_DownloadIdList(false, dr["ID"].ToString(), null, null);
            frm.ShowDialog(this);
        }

        private void btnSwitchtoWorkOrder_Click(object sender, EventArgs e)
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
