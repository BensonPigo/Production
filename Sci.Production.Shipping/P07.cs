using System.Windows.Forms;
using Ict.Win;
using Ict;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P07
    /// </summary>
    public partial class P07 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// P07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboReason, 2, 1, "1,Wrong Price,2,Wrong Qty,3,Wrong Price & Qty,4,NoPullout");
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format("select *,IIF(NewItem = 1,'Y','') as New from InvAdjust_Qty WITH (NOLOCK) where ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Q'ty", decimal_places: 0, iseditingreadonly: true)
                .Numeric("OrigQty", header: "Original", decimal_places: 0, iseditingreadonly: true)
                .Numeric("AdjustQty", header: "Change to", decimal_places: 0, iseditingreadonly: true)
                .Numeric("Price", header: "U'Price", decimal_places: 2, iseditingreadonly: true)
                .Text("New", header: "New", width: Widths.AnsiChars(1), iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.numOriginalSurchargeAmt.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigSurcharge"]);
            this.numChangetoSurchargeAmt.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustSurcharge"]);
            this.numOriginalCommAmt.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigCommission"]);
            this.numChangetoCommAmt.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustCommission"]);
            this.numOriginalTotalAmt.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigPulloutAmt"]) + MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigSurcharge"])) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigAddCharge"]) - MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["OrigCommission"]), 2);
            this.numChangetoTotalAmt.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustPulloutAmt"]) + MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustSurcharge"])) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustAddCharge"]) - MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["AdjustCommission"]), 2);
        }
    }
}
