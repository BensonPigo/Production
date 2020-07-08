using Ict.Win;
using System;

namespace Sci.Production.Cutting
{
    public partial class P20_Detail : Sci.Win.Subs.Input8A
    {
        public P20_Detail()
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.viewText();
        }

        private void viewText()
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

        protected override void ToNext()
        {
            base.ToNext();
            this.viewText();
        }

        protected override void ToPrev()
        {
            base.ToPrev();
            this.viewText();
        }

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
            this.viewText();
        }
    }
}
