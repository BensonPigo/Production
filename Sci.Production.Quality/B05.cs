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
       
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            
        }

        protected override bool ClickNew()
        {
            return base.ClickNew();
        
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }
        protected override bool ClickNewBefore()
        {
           
            this.textBox1.ReadOnly = true;
            this.numericBox1.ReadOnly = true;
            return base.ClickNewBefore();
        }

    }
}
