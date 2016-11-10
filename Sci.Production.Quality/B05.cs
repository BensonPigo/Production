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
        protected override void OnFormLoaded()
        {          
                    
            base.OnFormLoaded();
        }

        private void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                toolTip1.ToolTipTitle = "Invalid Number Value";
                MyUtility.Msg.InfoBox("The value you entered is not a valid Number. Please change the value.");               
                e.Cancel = true;
                return;

            }
        }       
    

        private void numericBox1_TextChanged(object sender, EventArgs e)
        {
            this.numericBox1.MaxLength = 2;
        }

    }
}
