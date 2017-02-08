using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(txtID.Text.Trim()) || MyUtility.Check.Empty(txtShowSeq.Text.Trim()))
            {
                MyUtility.Msg.InfoBox("'ID' and 'Show Seq' can not empty");
                return false;
            }
            return base.ClickSaveBefore();            
        }
    }
}
