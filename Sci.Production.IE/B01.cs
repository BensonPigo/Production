using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "A,All,N,New,R,Repeat");
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["UseFor"] = "A";
            CurrentMaintain["BaseOn"] = 1;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
            textBox4.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["Code"]))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            //移除BrandID必填，避免無BrandID的舊資料無法更新
            //if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            //{
            //    MyUtility.Msg.WarningBox("< Brand > can not be empty!");
            //    textBox4.Focus();
            //    return false;
            //}

            if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
            {
                MyUtility.Msg.WarningBox("< Activities > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["UseFor"]))
            {
                MyUtility.Msg.WarningBox("< New/Repeat > can not be empty!");
                this.comboBox1.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }

        //Brand
        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Brand WHERE Junk=0  ORDER BY Id";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "10,50,50", textBox4.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox4.Text = item.GetSelectedString();
        }
    }
}
