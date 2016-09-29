using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;

namespace Sci.Production.PPIC
{
    public partial class B08 : Sci.Win.Tems.Input6
    {
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailGridSetup()
        {
            
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Numeric("Day",header:"Day", iseditingreadonly:true)
                .Numeric("Efficiency", header: "Efficiency (%)",iseditingreadonly: false);
        }

        



    }
}
