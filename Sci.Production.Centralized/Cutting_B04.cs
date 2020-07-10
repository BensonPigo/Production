using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Cutting_B04
    /// </summary>
    public partial class Cutting_B04 : Win.Tems.Input1
    {
        /// <summary>
        /// Cutting_B04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Cutting_B04(ToolStripMenuItem menuitem)
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
                this.CurrentMaintain["type"] = "RC";
                if (cbResult = DBProxy.Current.Select(null, "select max(id) max_id from CutReason WITH (NOLOCK) where type='RC'", out whseReasonDt))
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
