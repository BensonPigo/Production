using System;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Centralized
{
    public partial class Sewing_B01 : Win.Tems.Input1
    {
        public Sewing_B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type = 'SO'");
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.txtID.ReadOnly = true;
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtDescription.Text))
            {
                MyUtility.Msg.WarningBox("<Description> can not be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            this.CurrentMaintain["Type"] = "SO";

            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            string newID = string.Format("{0:00000}", Convert.ToInt32(MyUtility.GetValue.Lookup("SELECT ISNULL(Max(Cast(ID as int)),0) FROM SewingReason", "ProductionTPE")) + 1);
            this.CurrentMaintain["ID"] = newID;

            base.ClickNewAfter();
        }
    }
}
