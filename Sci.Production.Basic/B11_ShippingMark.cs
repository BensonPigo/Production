﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B11_ShippingMark
    /// </summary>
    public partial class B11_ShippingMark : Sci.Win.Subs.Input1A
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
