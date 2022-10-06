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
    public partial class P23 : Sci.Win.Tems.QueryForm
    {
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // combo Datasource 
            Ict.DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, @"Select ID from Brand where Junk = 0", out DataTable dtCountry))
            {
                this.comboBrand.DataSource = dtCountry;
                this.comboBrand.DisplayMember = "ID";
                this.comboBrand.ValueMember = "ID";
            }
            else { this.ShowErr(cbResult); }
        }

        private void btnDataFilter_Click(object sender, EventArgs e)
        {
            P23_DataFilter frm = new P23_DataFilter();
            frm.ShowDialog(this);
            this.txtSeason.Text = P23_DataFilter.Season;
            this.Find();
        }

        private void Find()
        {
            if (MyUtility.Check.Empty(this.txtStyle.Text) || MyUtility.Check.Empty(this.comboBrand.Text))
            {
                MyUtility.Msg.WarningBox("Style# & Brand cannot be empty.");
                return;
            }
        }
    }
}
