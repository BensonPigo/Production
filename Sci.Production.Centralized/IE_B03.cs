using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B03
    /// </summary>
    public partial class IE_B03 : Win.Tems.Input1
    {
        /// <summary>
        /// IE_B03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public IE_B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        // 存檔前檢查

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                this.CurrentMaintain["type"] = "LM";
                if (cbResult = DBProxy.Current.Select(null, "select max(id) max_id from IEReason WITH (NOLOCK) where type='LM'", out whseReasonDt))
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
