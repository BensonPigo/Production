using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Basic
{
    public partial class B14 : Sci.Win.Tems.Input1
    {
        public B14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.label9.Text = "Subprocess BCS\r\nLead Time";
            this.label10.Text = "Std. L/T(Day) b4\r\n1st Cut date base\r\non SubProcess";

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("I", "InHouse");
            comboBox1_RowSource.Add("O", "OSP");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlCommand = string.Format("select (select cast(rtrim(ID) as nvarchar) +',' from MachineType where ArtworkTypeID = '{0}' or ArtworkTypeDetail = '{0}' for XML Path('')) as MatchTypeID", CurrentMaintain["ID"]);
            Ict.DualResult returnResult;
            DataTable machineTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, sqlCommand, out machineTable))
            {
                this.editBox1.Text = machineTable.Rows[0]["MatchTypeID"].ToString();
            }
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtdropdownlist1.ReadOnly = true;
            this.editBox1.ReadOnly = true;
            this.checkBox1.ReadOnly = true;
            this.checkBox2.ReadOnly = true;
            this.checkBox3.ReadOnly = true;
            this.checkBox4.ReadOnly = true;
            this.checkBox5.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {

            if (String.IsNullOrWhiteSpace(CurrentMaintain["InhouseOSP"].ToString()))
            {
                MyUtility.Msg.WarningBox("< InHouse/OSP > can not be empty!");
                this.comboBox1.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B14_Machine callNextForm = new Sci.Production.Basic.B14_Machine(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
