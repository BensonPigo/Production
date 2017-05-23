﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtThreadCombination.ReadOnly = true;
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                txtThreadCombination.Focus();
                MyUtility.Msg.WarningBox("<Thread Combination> can not be empty.");
                return false;
            }
            return base.ClickSaveBefore();
        }
    }
}
