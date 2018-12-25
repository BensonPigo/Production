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
    /// <summary>
    /// B14
    /// </summary>
    public partial class B14 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.labelSubprocessBCSLeadTime.Text = "Subprocess BCS\r\nLead Time";
            this.labelStdLTDayb41stCutDateBaseOnSubProcess.Text = "Std. L/T(Day) b4\r\n1st Cut date base\r\non SubProcess";

            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("I", "InHouse");
            comboBox1_RowSource.Add("O", "OSP");
            this.comboInHouseOSP.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboInHouseOSP.ValueMember = "Key";
            this.comboInHouseOSP.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlCommand = string.Format(
               @"
select (
	select concat( ',',cast(rtrim(m.ID) as nvarchar))
	FROM MachineType m
	INNER JOIN ArtworkType a ON m.ArtworkTypeID=a.ID
	where  A.Seq LIKE '1%'AND m.ArtworkTypeID = '{0}' 
	for XML Path('')
) as MatchTypeID
INTO #tmp

SELECt [MatchTypeID]=STUFF( MatchTypeID,1,1,'')
FROM #tmp

DROP TABLE #tmp
",
               this.CurrentMaintain["ID"]);

            Ict.DualResult returnResult;
            DataTable machineTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, sqlCommand, out machineTable))
            {
                this.editMachineID.Text = machineTable.Rows[0]["MatchTypeID"].ToString();
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["InhouseOSP"]))
            {
                MyUtility.Msg.WarningBox("< InHouse/OSP > can not be empty!");
                this.comboInHouseOSP.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void BtnMachine_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B14_Machine callNextForm = new Sci.Production.Basic.B14_Machine(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
