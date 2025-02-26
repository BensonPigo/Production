using System;
using System.ComponentModel;

namespace Sci.Production.Class
{
    /// <summary>
    /// DateEstCutDate
    /// </summary>
    public partial class DateEstCutDate : Win.UI.DateBox
    {
        /// <inheritdoc/>
        public DateEstCutDate()
        {
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.Value) && this.OldValue != this.Value)
            {
                if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(this.Value)) > 0)
                {
                    this.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Est. Cut Date] cannot be less than today");
                }
            }
        }
    }
}
