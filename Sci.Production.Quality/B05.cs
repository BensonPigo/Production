using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class B05 : Sci.Win.Tems.Input1
    {
        ToolTip toolTip1 = new ToolTip();
       
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            
        }
        private void numWeight_TextChanged(object sender, EventArgs e)
        {
            this.numWeight.MaxLength = 2;
        }

    }
}
