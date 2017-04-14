﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = string.Format("MDivisionID = '{0}'",Sci.Env.User.Keyword);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //按鈕Shipping Mark變色
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Factory_TMS", "ID") ||
                MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Factory_WorkHour", "ID"))
            {
                this.btnCapacityWorkday.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCapacityWorkday.ForeColor = Color.Black;
            }
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["NameEN"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Name > can not be empty!");
                this.txtName.Focus();
                return false;
            }
            
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;//MDivisionID為登入的ID

            return base.ClickSaveBefore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B01_CapacityWorkDay callNextForm = new Sci.Production.Basic.B01_CapacityWorkDay(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
