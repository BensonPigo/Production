using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P01_ShippingMark : Sci.Win.Subs.Input1A
    {
        public P01_ShippingMark(bool canedit, DataRow data) : base(canedit, data)
        {
            InitializeComponent();
            this.Text = "Shipping Mark-(" + data["id"].ToString().Trim() + ")";
        }
    }
}
