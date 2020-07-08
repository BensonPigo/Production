using System;
using System.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P03_Detail
    /// </summary>
    public partial class P03_Detail : Win.Subs.Input6A
    {
        /// <summary>
        /// P03_Detail
        /// </summary>
        public P03_Detail()
        {
            this.InitializeComponent();
            this.txtuserFtyModifier.TextBox1.ReadOnly = true;
            this.txtuserFtyModifier.TextBox1.IsSupportEditMode = false;
        }

        /// <inheritdoc/>
        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);

            if (!MyUtility.Check.Empty(this.CurrentData["MRLastDate"]))
            {
                this.displayMRLastUpdate.Text = Convert.ToDateTime(this.CurrentData["MRLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                this.displayMRLastUpdate.Text = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.CurrentData["FtyLastDate"]))
            {
                this.displayFtyLastDate.Text = Convert.ToDateTime(this.CurrentData["FtyLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                this.displayFtyLastDate.Text = string.Empty;
            }
        }
    }
}
