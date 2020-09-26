using Sci.Data;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B10 : Win.Tems.Input1
    {
        private readonly Hashtable ht = new Hashtable();

        public B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.ht.Add("Formula1", "(Total Points / Act. Yds Inspected ) x 100");
            this.ht.Add("Formula2", "(Total Points × 3600) ÷ (Act. Yds Inspected × Actual Width)");
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtbrand.Text))
            {
                MyUtility.Msg.WarningBox("<Brand> cannot be empty! ");
                this.txtbrand.Focus();
                return false;
            }

            // DataRow dr;
            if (MyUtility.Check.Seek(string.Format("select * from PointRate where brandid='{0}'", this.txtbrand.Text)) && !this.txtbrand.ReadOnly)
            {
                MyUtility.Msg.WarningBox(string.Format("<Brand : {0}> existed, change other one please!", this.txtbrand.Text));
                this.txtbrand.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult ClickSave()
        {
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format("select * from PointRate where brandid='{0}'", this.txtbrand.Text), out dr))
            {
                DBProxy.Current.Execute(null, string.Format(@"update PointRate set ID='{0}' where BrandID='{1}'", this.radioOption1.Checked ? "1" : "2", this.txtbrand.Text));
            }

            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override void ClickLocate()
        {
            base.ClickLocate();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            if (this.CurrentMaintain.Empty())
            {
                this.txtFormula.Text = string.Empty;
                return;
            }

            this.txtFormula.Text = this.CurrentMaintain["ID"].ToString() == "1" ? this.ht["Formula1"].ToString() : this.ht["Formula2"].ToString();

            if (this.EditMode)
            {
                this.radioPanel1.ReadOnly = false;
            }
            else
            {
                this.radioPanel1.ReadOnly = true;
                this.txtbrand.ReadOnly = true;
            }

            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.txtbrand.ReadOnly = false;
            this.radioOption1.Checked = true;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.txtbrand.ReadOnly = true;
            return base.ClickEditBefore();
        }

        private void RadioPanel1_ValueChanged(object sender, EventArgs e)
        {
            this.txtFormula.Text = this.radioPanel1.Value == "1" ? this.ht["Formula1"].ToString() : this.ht["Formula2"].ToString();
        }
    }
}
