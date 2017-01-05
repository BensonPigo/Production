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

        protected override void ClickNewAfter()
        {
            textBox1.ReadOnly = false;
            base.ClickNewAfter();
        }

        protected override void ClickEditAfter()
        {
            textBox1.ReadOnly = true;
            base.ClickEditAfter();
        }

        protected override void ClickSaveAfter()
        {
            textBox1.ReadOnly = true;
            base.ClickSaveAfter();
        }

        protected override void ClickUndo()
        {
            textBox1.ReadOnly = true;
            base.ClickUndo();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(textBox1.Text.Trim()) || MyUtility.Check.Empty(textBox3.Text.Trim()))
            {
                MyUtility.Msg.InfoBox("ID and Show SEQ can not empty");
                return false;
            }

            return base.ClickSaveBefore();            
        }

    }
}
