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
    public partial class B20 : Sci.Win.Tems.Input1
    {
        public B20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();           
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }
    }
}
