using System;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Planning
{
    /// <summary>
    /// B01
    /// </summary>
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">PlanningB01</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");

            DataRow dr = dt.NewRow();
            dr[0] = "Laser";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Embroidery";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Bonding (Machine)";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Bonding (Hand)";
            dt.Rows.Add(dr);

            this.comboArtworkType.DataSource = dt;
            this.comboArtworkType.DisplayMember = "Value";
            this.comboArtworkType.ValueMember = "Value";
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["unit"] = 1;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["issuedate"].ToString()))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["artworktypeid"].ToString()))
            {
                this.comboArtworkType.Focus();
                MyUtility.Msg.WarningBox("< Artwork Type > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ftysupp"].ToString()))
            {
                this.txtsubconFtySupp.Focus();
                MyUtility.Msg.WarningBox("< Fty / Supp > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["heads"]) && this.CurrentMaintain["artworktypeid"].ToString() == "Embroidery")
            {
                this.numHeads.Focus();
                MyUtility.Msg.WarningBox("< # of Heads > can not be empty!");
                return false;
            }

            if (this.radiobyMonth.Checked == true)
            {
                int yy = DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).Year;
                int mm = DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).Month;
                this.CurrentMaintain["issuedate"] = DateTime.Parse(yy.ToString() + "/" + mm.ToString() + "/1");
            }

            return base.ClickSaveBefore();
        }

        private void ComboArtworkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboArtworkType.SelectedValue == null)
            {
                return;
            }

            switch (this.comboArtworkType.SelectedValue.ToString().TrimEnd())
            {
                case "Bonding (Hand)":
                    this.labelCapacity.Text = "Capacity (Person):";
                    this.label25.Text = "(Calculate by employee)";
                    break;
                case "Embroidery":
                    this.labelHeads.Visible = true;
                    this.numHeads.Visible = true;
                    break;
                default:
                    this.labelCapacity.Text = "Capacity (Unit)";
                    this.label25.Text = "(Calculate by machine)";
                    this.labelHeads.Visible = false;
                    this.numHeads.Visible = false;
                    break;
            }
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            switch (this.CurrentMaintain["artworktypeid"].ToString().TrimEnd())
            {
                case "Bonding (Hand)":
                    this.labelCapacity.Text = "Capacity (Person):";
                    this.label25.Text = "(Calculate by employee)";
                    break;
                case "Embroidery":
                    this.labelHeads.Visible = true;
                    this.numHeads.Visible = true;
                    break;
                default:
                    this.labelCapacity.Text = "Capacity (Unit)";
                    this.label25.Text = "(Calculate by machine)";
                    this.labelHeads.Visible = false;
                    this.numHeads.Visible = false;
                    break;
            }
        }

        private void DateDate_Validated(object sender, EventArgs e)
        {
            if (this.CurrentMaintain["unit"].ToString() == "2")
            {
                int yy = DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).Year;
                int mm = DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).Month;
                this.CurrentMaintain["issuedate"] = DateTime.Parse(yy.ToString() + "/" + mm.ToString() + "/1");
            }
        }

        private void RadioPanel1_Validated(object sender, EventArgs e)
        {
            RadioPanel rdoG = (RadioPanel)sender;
            if (rdoG.Value == "2" && !MyUtility.Check.Empty(this.CurrentMaintain["issuedate"]))
            {
                int yy = DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).Year;
                int mm = DateTime.Parse(this.CurrentMaintain["issuedate"].ToString()).Month;
                this.CurrentMaintain["issuedate"] = DateTime.Parse(yy.ToString() + "/" + mm.ToString() + "/1");
            }
        }
    }
}
