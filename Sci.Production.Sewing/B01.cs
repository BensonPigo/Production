using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type = 'SO'");
        }

        protected override void OnDetailEntered()
        {
            txtID.ReadOnly = true;
            txtDescription.ReadOnly = true;
            chkJunk.ReadOnly = true;
            base.OnDetailEntered();
        }
    }
}
