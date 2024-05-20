using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class WH_B11 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public WH_B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = "DR";
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Description"]))
            {
                MyUtility.Msg.WarningBox("< Reason > can not be empty!");
                this.txtDesc.Focus();
                return false;
            }

            if (this.IsDetailInserting)
            {
                DualResult result = DBProxy.Current.Select("ProductionTPE", "SELECT MAX(ID) max_id FROM WhseReason WITH (NOLOCK) WHERE Type='DR'", out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                if (dt.Rows.Count == 0 || MyUtility.Check.Empty(dt.Rows[0][0]))
                {
                    this.CurrentMaintain["id"] = "00001";
                }
                else
                {
                    string maxID = MyUtility.Convert.GetString(dt.Rows[0][0]);
                    int newID = int.Parse(maxID) + 1;
                    this.CurrentMaintain["id"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
