using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.IE
{
    public partial class B08 : Sci.Win.Tems.Input1
    {
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
        }

        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        protected override void OnEditAfter()
        {
            base.OnEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool OnSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MessageBox.Show("< Employee# > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Name"].ToString()))
            {
                MessageBox.Show("< Nick Name > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["OnBoardDate"].ToString()))
            {
                MessageBox.Show("< Hired on > can not be empty!");
                this.dateBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["SewingLineID"].ToString()))
            {
                MessageBox.Show("< Line > can not be empty!");
                this.txtsewingline1.Focus();
                return false;
            }

            return base.OnSaveBefore();
        }

        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                DataTable MachineGroup;
                Ict.DualResult returnResule = DBProxy.Current.Select("Machine", "select ID,Description from MachineGroup where Junk = 0 order by ID", out MachineGroup);
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(MachineGroup, "ID,Description", "Group ID,Description", "2,60");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }

                string returnData = "";
                IList<DataRow> gridData = item.GetSelecteds();
                foreach (DataRow currentRecord in gridData)
                {
                    returnData = returnData + currentRecord["ID"].ToString() + ",";
                }
                this.textBox3.Text = returnData;
            }
        }
    }
}
