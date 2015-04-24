using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B11_ShippingMark : Sci.Win.Subs.Input1A
    {
        public B11_ShippingMark(bool canedit, DataRow data)
            : base(canedit, data)
        {
            InitializeComponent();
            this.Text = "Shipping Mark-(" + data["id"].ToString().Trim() + ")";
        }
    }
}
