using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Packing_B09
    /// </summary>
    public partial class Packing_B10 : Win.Tems.Input1
    {
        /// <summary>
        /// B09
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Packing_B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Type"] = "MD";
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.txtDescription.ReadOnly = true;
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.txtDescription.ReadOnly = true;
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                this.CurrentMaintain["Type"] = "MD";
                if (cbResult = DBProxy.Current.Select("ProductionTPE", "select max(id) max_id from PackingReason WITH (NOLOCK) where type='MD'", out whseReasonDt))
                {
                    string id = whseReasonDt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        this.CurrentMaintain["id"] = "00001";
                    }
                    else
                    {
                        int newID = int.Parse(id) + 1;
                        this.CurrentMaintain["id"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                    }
                }
                else
                {
                    this.ShowErr(cbResult);
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
