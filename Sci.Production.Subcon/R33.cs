using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R33 : Sci.Win.Tems.PrintForm
    {
        public R33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.print.Visible = false;
        }
    }
}
