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
    public partial class P02_PatternPanel : Sci.Win.Subs.Input8A
    {
        public P02_PatternPanel()
        {
            InitializeComponent();
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            prev.Visible = false;
            next.Visible = false;
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
              .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2))
              .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2));
            return true;
        }
    }
}
