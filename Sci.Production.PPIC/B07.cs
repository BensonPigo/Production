using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string sqlCommand = "select UseAPS from factory where ID = '" + Sci.Env.User.Factory + "'";
            string useAPS = myUtility.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "TRUE")
            {
                IsSupportDelete = false;
                IsSupportEdit = false;
                IsSupportNew = false;
            }
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayBox2.Text = Convert.ToDateTime(CurrentMaintain["Date"]).ToString("dddd");
            this.button1.ForeColor = Color.Blue;
        }

        protected override void OnEditAfter()
        {
            base.OnEditAfter();
            this.dateBox1.ReadOnly = true;
            this.txtsewingline1.ReadOnly = true;
        }

        protected override bool OnNewBefore()
        {
            Sci.Production.PPIC.B07_Add callNextForm = new Sci.Production.PPIC.B07_Add(CurrentMaintain);
            callNextForm.ShowDialog(this);
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.B07_BatchAdd callNextForm = new Sci.Production.PPIC.B07_BatchAdd(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
