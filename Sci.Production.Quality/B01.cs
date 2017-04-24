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
            this.txtCode.ReadOnly = true;         
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查 
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;      
            }
            if (MyUtility.Check.Empty(CurrentMaintain["DescriptionEN"]))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.editDescription.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("< Type > can not be empty!");
                this.txttype.Focus();
                return false;
            }
          
            #endregion
            return base.ClickSaveBefore();
        }
    }
}
