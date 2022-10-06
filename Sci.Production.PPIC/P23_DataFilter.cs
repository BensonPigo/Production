using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P23_DataFilter : Sci.Win.Forms.Base
    {
        public static string Season;

        public P23_DataFilter()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // combo Datasource 
            Ict.DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, @"select ID from SeasonSCI where Junk = 0 order by Month desc", out DataTable dtCountry))
            {
                this.comboSeason.DataSource = dtCountry;
                this.comboSeason.DisplayMember = "ID";
                this.comboSeason.ValueMember = "ID";
            }
            else { this.ShowErr(cbResult); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.comboSeason.Text))
            {
                MyUtility.Msg.WarningBox("Season cannot be empty.");
                return;
            }

            Season = this.comboSeason.Text;
        }
    }
}
