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
    public partial class P41 : Sci.Win.Tems.QueryForm
    {
        public P41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;

            //this.comboSubPorcess.set
        }
    }
}
