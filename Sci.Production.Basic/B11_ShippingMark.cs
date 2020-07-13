using System.Data;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B11_ShippingMark
    /// </summary>
    public partial class B11_ShippingMark : Win.Subs.Input1A
    {
        /// <summary>
        /// B11_ShippingMark
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="data">data</param>
        public B11_ShippingMark(bool canedit, DataRow data)
            : base(canedit, data)
        {
            this.InitializeComponent();
            this.Text = "Shipping Mark-(" + data["id"].ToString().Trim() + ")";
            this.edit.Visible = false;
            this.save.Visible = false;
        }
    }
}
