using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B14
    /// </summary>
    public partial class B14 : Win.Tems.Input1
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
            DualResult result;
            DataTable dt = new DataTable();
            string sqlCmd;

            #region Machine Button Color
            sqlCmd = string.Format(
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

            result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.editMachineID.Text = dt.Rows[0]["MatchTypeID"].ToString();
            #endregion

            #region Machine ID

            // 根據Code 第一碼決定顯示內容
            sqlCmd = string.Format(
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

            result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt.Rows.Count > 0)
            {
                this.btnMachine.ForeColor = Color.Blue;
            }
            else
            {
                this.btnMachine.ForeColor = Color.Black;
            }
            #endregion

            #region checkbox color
            sqlCmd = string.Format("select * from ArtworkType_FTY where ArtworkTypeID = '{0}'", this.CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.chkIsShowinIEP01.Checked = dt.AsEnumerable().Where(x => x.Field<bool>("IsShowinIEP01").Equals(true)).Any();
            var showinIEP03 = dt.AsEnumerable().Where(x => x.Field<bool>("IsShowinIEP03").Equals(true));
            this.chkIsShowinIEP03.Checked = showinIEP03.Any();
            var sewingline = dt.AsEnumerable().Where(x => x.Field<bool>("IsSewingline").Equals(true));
            this.chkIsSewingline.Checked = sewingline.Any();

            this.txtCentralizedmulitFactoryIEP03.Text = string.Empty;
            if (showinIEP03.Any())
            {
                this.txtCentralizedmulitFactoryIEP03.Text = string.Join(",", showinIEP03.Select(x => x.Field<string>("FactoryID")).ToArray());
            }

            this.txtCentralizedmulitFactorySewingline.Text = string.Empty;
            if (sewingline.Any())
            {
                this.txtCentralizedmulitFactorySewingline.Text = string.Join(",", sewingline.Select(x => x.Field<string>("FactoryID")).ToArray());
            }
            #endregion
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
            B14_Machine callNextForm = new B14_Machine(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
