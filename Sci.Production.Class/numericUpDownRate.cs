namespace Sci.Production.Class
{
    /// <summary>
    /// Numeric UpDownRate
    /// </summary>
    public class NumericUpDownRate : Win.UI.NumericUpDown
    {
        /// <inheritdoc/>
        protected override void UpdateEditText()
        {
            this.Text = this.Value.ToString() + "%";
        }
    }
}
