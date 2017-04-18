﻿using System;
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
            this.labelSubprocessBCSLeadTime.Text = "Subprocess BCS\r\nLead Time";
            this.labelStdLTDayb41stCutDateBaseOnSubProcess.Text = "Std. L/T(Day) b4\r\n1st Cut date base\r\non SubProcess";

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("I", "InHouse");
            comboBox1_RowSource.Add("O", "OSP");
            comboInHouseOSP.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboInHouseOSP.ValueMember = "Key";
            comboInHouseOSP.DisplayMember = "Value";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlCommand = string.Format("select (select cast(rtrim(ID) as nvarchar) +',' from MachineType WITH (NOLOCK) where ArtworkTypeID = '{0}' or ArtworkTypeDetail = '{0}' for XML Path('')) as MatchTypeID", CurrentMaintain["ID"]);
            Ict.DualResult returnResult;
            DataTable machineTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, sqlCommand, out machineTable))
            {
                this.editMachineID.Text = machineTable.Rows[0]["MatchTypeID"].ToString();
            }
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtDropdownlistClassify.ReadOnly = true;
            this.editMachineID.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
            this.checkIsTMS.ReadOnly = true;
            this.checkIsPrice.ReadOnly = true;
            this.checkIsArtWork.ReadOnly = true;
            this.checkIsttlTMS.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {

            if (MyUtility.Check.Empty(CurrentMaintain["InhouseOSP"]))
            {
                MyUtility.Msg.WarningBox("< InHouse/OSP > can not be empty!");
                this.comboInHouseOSP.Focus();
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
