using Ict.Win;
using System;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P20_Detail : Win.Subs.Input8A
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P20_Detail"/> class.
        /// </summary>
        public P20_Detail()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.ViewText();
        }

        private void ViewText()
        {
            this.editSPNo.Text = this.CurrentDetailData["OrderID"].ToString();
            this.displayCutRefNo.Text = this.CurrentDetailData["Cutref"].ToString();
            this.displayCutNo.Text = this.CurrentDetailData["cutno"].ToString();
            this.displayColor.Text = this.CurrentDetailData["colorid"].ToString();
            this.displayCons.Text = this.CurrentDetailData["cons"].ToString();
            this.displayMarkerName.Text = this.CurrentDetailData["MarkerName"].ToString();
            this.displayMarkerLength.Text = this.CurrentDetailData["MarkerLength"].ToString();
            this.displayFabricCombo.Text = this.CurrentDetailData["FabricCombo"].ToString();
            this.displayFabricPanelCode.Text = this.CurrentDetailData["FabricPanelCode"].ToString();
        }

        /// <inheritdoc/>
        protected override void ToNext()
        {
            base.ToNext();
            this.ViewText();
        }

        /// <inheritdoc/>
        protected override void ToPrev()
        {
            base.ToPrev();
            this.ViewText();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            #region set grid
            // this.grid.ReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(10), integer_places: 6, iseditingreadonly: true);
            #endregion
            return true;
        }

        private void P20_Detail_Load(object sender, EventArgs e)
        {
            this.ViewText();
        }
    }
}
