using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Shipping_B07 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public Shipping_B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }


        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.EditMode = true;
            this.txtcountry.TextBox1.Text = MyUtility.GetValue.Lookup($@"SELECT CountryID FROM Port WHERE ID='{this.CurrentMaintain["PortID"]}'");

            base.OnDetailEntered();
        }
    }
}