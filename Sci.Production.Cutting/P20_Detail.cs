using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P20_Detail : Sci.Win.Subs.Input8A
    {

        public P20_Detail()
        {
            
            InitializeComponent();
            
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            viewText();
        }
        private void viewText()
        {
            editSPNo.Text = CurrentDetailData["OrderID"].ToString();
            displayCutRefNo.Text = CurrentDetailData["Cutref"].ToString();
            displayCutNo.Text = CurrentDetailData["cutno"].ToString();
            displayColor.Text = CurrentDetailData["colorid"].ToString();
            displayCons.Text = CurrentDetailData["cons"].ToString();
            displayMarkerName.Text = CurrentDetailData["MarkerName"].ToString();
            displayMarkerLength.Text = CurrentDetailData["MarkerLength"].ToString();
            displayFabricCombo.Text = CurrentDetailData["FabricCombo"].ToString();
            displayFabricPanelCode.Text = CurrentDetailData["FabricPanelCode"].ToString();
        }
        protected override void ToNext()
        {
            base.ToNext();
            viewText();

        }
        protected override void ToPrev()
        {
            base.ToPrev();
            viewText();
        }

        protected override bool OnGridSetup()
        {
            #region set grid
            //this.grid.ReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10),iseditingreadonly: true)
            .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(10), integer_places: 6,iseditingreadonly: true);
            #endregion
            return true;
        }

        private void P20_Detail_Load(object sender, EventArgs e)
        {
            viewText();
        }

    }
}
