using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_B09
    /// </summary>
    public partial class B09 : Win.Tems.Input1
    {
        /// <summary>
        /// B09
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                this.CurrentMaintain["type"] = "PA";
                if (cbResult = DBProxy.Current.Select(null, "select max(id) max_id from PackingReason WITH (NOLOCK) where type='PA'", out whseReasonDt))
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

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }
    }
}
