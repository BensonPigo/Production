using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ShippingMark
    /// </summary>
    public partial class P01_ShippingMark : Sci.Win.Subs.Input1A
    {
        /// <summary>
        /// P01_ShippingMark
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="data">DataRow data</param>
        public P01_ShippingMark(bool canedit, DataRow data)
            : base(canedit, data)
        {
            this.InitializeComponent();
            this.Text = "Shipping Mark-(" + data["id"].ToString().Trim() + ")";
        }
    }
}
