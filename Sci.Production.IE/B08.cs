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
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //新增Import From Barcode按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Import From Excel";
            btn.Click += new EventHandler(btn_Click);
            browsetop.Controls.Add(btn);
            btn.Size = new Size(165, 30);//預設是(80,30)
        }

        //Import From Barcode按鈕的Click事件
        private void btn_Click(object sender, EventArgs e)
        {
            
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("< Factory > can not be empty!");
                txtmfactory1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Employee# > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("< Nick Name > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["OnBoardDate"]))
            {
                MyUtility.Msg.WarningBox("< Hired on > can not be empty!");
                this.dateBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("< Line > can not be empty!");
                this.txtsewingline1.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                DataTable MachineGroup;
                Ict.DualResult returnResule = DBProxy.Current.Select("Machine", "select ID,Description from MachineGroup where Junk = 0 order by ID", out MachineGroup);
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(MachineGroup, "ID,Description", "Group ID,Description", "2,60", textBox3.Text);

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
