using Ict.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P02_PatternPanel : Sci.Win.Subs.Input4
    {
        public P02_PatternPanel(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3,DataTable layerTb)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
        }
        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2))
                .Text("LectraCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2));

            return true;
        }
    }
}
