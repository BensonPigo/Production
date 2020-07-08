using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
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
IF (SELECT COUNT( m.ArtworkTypeID)FROm MachineType m 
	INNER JOIN ArtworkType a On m.ArtworkTypeID=a.ID
	WHERE A.Seq LIKE '1%' AND m.ArtworkTypeID='{0}' ) 
	> 0
BEGIN
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
END
ELSE
BEGIN
    select (
	    select cast(rtrim(ID) as nvarchar) +',' 
	    from MachineType MT WITH (NOLOCK) LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON MT.ID=ATD.MachineTypeID
	    where ATD.ArtworkTypeID = '{0}' for XML Path('')
    ) as MatchTypeID
END

",
    this.CurrentMaintain["ID"]);

            Ict.DualResult returnResult;
            DataTable machineTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, sqlCommand, out machineTable))
            {
                this.editMachineID.Text = machineTable.Rows[0]["MatchTypeID"].ToString();
            }

            // 根據Code 第一碼決定顯示內容
            string selectCommand = string.Format(
                                    @"
IF (SELECT COUNT( m.ArtworkTypeID)FROm MachineType m 
	INNER JOIN ArtworkType a On m.ArtworkTypeID=a.ID
	WHERE A.Seq LIKE '1%' AND m.ArtworkTypeID='{0}' ) 
	> 0
BEGIN
        select m.ID,Description
	    FROM MachineType m
	    INNER JOIN ArtworkType a ON m.ArtworkTypeID=a.ID
	    where  A.Seq LIKE '1%'AND m.ArtworkTypeID = '{0}' 
END
ELSE
BEGIN
    select ID,Description 
    from MachineType MT WITH (NOLOCK) LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON MT.ID=ATD.MachineTypeID
    where ATD.ArtworkTypeID = '{0}'
END

",
                                    this.CurrentMaintain["ID"].ToString());

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand, out selectDataTable);

            if (selectResult1)
            {
                if (selectDataTable.Rows.Count > 0)
                {
                    this.btnMachine.ForeColor = Color.Blue;
                }
                else
                {
                    this.btnMachine.ForeColor = Color.Black;
                }
            }
            else
            {
                this.ShowErr(selectResult1);
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
            this.checkIsPrintToCMP.ReadOnly = true;
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
