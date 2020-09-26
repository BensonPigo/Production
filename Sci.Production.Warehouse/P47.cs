using Ict;
using Ict.Win;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P47 : Win.Tems.Input6
    {
        public P47(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "Type='D'";
        }

        // 表身資料SQL Command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select 
SL2.id,
SL2.POID,
SL2.Refno,
SL2.Color,
SL2.MDivisionID,
LI.Description,
LI.UnitID,
SL2.Qty,
SL2.FromLocation,
SL2.ToLocation,
SL2.Ukey
from SubTransferLocal_Detail SL2
left join LocalItem LI on SL2.Refno=li.RefNo
where SL2.ID='{0}'
 ", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // 表身資料設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("id", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 0
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true) // 1
            .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true) // 2
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 3
            .Text("UnitID", header: "Unit", iseditingreadonly: true) // 4
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 5
            .Text("FromLocation", header: "From Bulk Location", iseditingreadonly: true) // 6
            .Text("ToLocation", header: "To Scrap Location", iseditingreadonly: true) // 7
            ;
            #endregion 欄位設定
        }
    }
}
