using Sci.Data;
using Sci.Production.Class;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class B09 : Win.Tems.Input1
    {
        private bool IsoldBrandID = true;
        private bool IsoldSeason = true;
        private bool IsoldRefno = true;

        /// <inheritdoc/>
        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.IsoldBrandID = true;
            this.IsoldSeason = true;
            this.IsoldRefno = true;
        }

        private void TxtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = $@"
            Select distinct Refno from Fabric where junk = 0 and Type = 'F'";

            DataTable dt = DBProxy.Current.SelectEx(sql).ExtendedData;
            using (var dlg = new SelectItem(dt, "Refno", "Refno", "10", this.txtRefno.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtRefno.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item;
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0 order by ID desc";
            item = new Win.Tools.SelectItem(sqlCmd, "11", this.Text)
            {
                Width = 300,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.IsoldBrandID && this.IsoldSeason && this.IsoldRefno)
            {
                return true;
            }

            string strRefno = this.txtRefno.Text;
            string strBrandID = this.txtBrand.Text;
            string strSeason = this.txtSeason.Text;
            string sqlcmd = $@"Select 1 From FabricNotch Where Refno = '{strRefno}' AND BrandID = '{strBrandID}' AND SeasonId = '{strSeason}'";

            if (MyUtility.GetValue.Lookup(sqlcmd) == "1")
            {
                MyUtility.Msg.WarningBox("Already exist same Refno,BrandID,SeasonID data,please check.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtSeason_Validated(object sender, EventArgs e)
        {
            this.IsoldSeason = !MyUtility.Check.Empty(this.txtSeason.OldValue) ? this.txtSeason.OldValue == this.txtSeason.Text : true;
        }

        private void TxtRefno_Validated(object sender, EventArgs e)
        {
            this.IsoldRefno = !MyUtility.Check.Empty(this.txtRefno.OldValue) ? this.txtRefno.OldValue == this.txtRefno.Text : true;
        }

        private void TxtBrand_Validating(object sender, CancelEventArgs e)
        {
            this.IsoldBrandID = !MyUtility.Check.Empty(this.txtBrand.OldValue) ? this.txtBrand.OldValue == this.txtBrand.Text : true;
        }
    }
}
