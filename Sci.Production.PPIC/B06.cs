using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B06 : Sci.Win.Tems.Input1
    {
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string sqlCommand = "select UseAPS from factory WITH (NOLOCK) where ID = '" + Sci.Env.User.Factory + "'";
            string useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "TRUE")
            {
                IsSupportCopy = false;
                IsSupportDelete = false;
                IsSupportEdit = false;
                IsSupportNew = false;
            }

            //string sqlCommand2 = "select IsSampleRoom from factory where ID = '" + Sci.Env.User.Factory + "'";
            //string IsSampleRoom = MyUtility.GetValue.Lookup(sqlCommand2, null);
            //if (IsSampleRoom == "False")
            //{
            //    IsSupportCopy = false;
            //    IsSupportDelete = false;
            //    IsSupportEdit = false;
            //    IsSupportNew = false;
            //}

            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            txtCell1.FactoryId = Sci.Env.User.Factory;
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Line# > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.textBox2.Focus();
                return false;
            }
   
            return base.ClickSaveBefore();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            //當輸入的值只有一個位元且介於0~9時，自動在此值前面補數字’0’
            if (!string.IsNullOrWhiteSpace(this.textBox1.Text) && this.textBox1.Text.Trim().Length == 1)
            {
                char idValue = Convert.ToChar(this.textBox1.Text.Trim().Substring(0, 1));
                if (Convert.ToInt32(idValue) >= 48 && Convert.ToInt32(idValue) <= 57)
                {
                    CurrentMaintain["ID"] = "0" + this.textBox1.Text.Trim();
                }
            }
        }
    }
}
