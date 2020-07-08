namespace Sci.Production.Class
{
    public class numericUpDownRate : Sci.Win.UI.NumericUpDown
    {
        protected override void UpdateEditText()
        {
            this.Text = this.Value.ToString() + "%";
        }
    }
}
