﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        private string keyword = Sci.Env.User.Keyword;
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            string defaultfilter = string.Format("MDivisionid = '{0}' ", keyword);
            this.DefaultFilter = defaultfilter;
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtThreadLocation.ReadOnly = true;
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["mDivisionid"] = keyword;
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                txtThreadLocation.Focus();
                MyUtility.Msg.WarningBox("<Thread Location> can not be empty.");
                return false;
            }
            return base.ClickSaveBefore();
        }
    }
}
