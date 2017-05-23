﻿using System;
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
    public partial class B02 : Sci.Win.Tems.Input1
    {         
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;         
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查 
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                this.txtCode.Focus();
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                return false;      
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
            {
                this.editDescription.Focus();
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                return false;
            }

          
            #endregion
            return base.ClickSaveBefore();
        }
    }
}
