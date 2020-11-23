using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B07 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.CurrentMaintain["CountryID"] = MyUtility.GetValue.Lookup($@"SELECT CountryID FROM Port WHERE ID='{this.CurrentMaintain["PulloutPortID"]}'");
            this.txtcountry.TextBox1.Text = MyUtility.Convert.GetString(this.CurrentMaintain["CountryID"]);

            this.CurrentMaintain["ContinentID"] = MyUtility.GetValue.Lookup($@"select c.Continent from Country c INNER JOIN DropDownList d ON d.Type = 'Continent' and d.ID = c.Continent where c.id='{this.txtcountry.TextBox1.Text}' ");
            this.txtContinent.Text = MyUtility.Convert.GetString(this.CurrentMaintain["ContinentID"]);

            this.CurrentMaintain["ContinentName"] = MyUtility.GetValue.Lookup($@"select d.Name from Country c INNER JOIN DropDownList d ON d.Type = 'Continent' and d.ID = c.Continent where c.id='{this.txtcountry.TextBox1.Text}' ");
            this.displayContinent.Text = MyUtility.Convert.GetString(this.CurrentMaintain["ContinentName"]);

            this.chkIsAirPort.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select AirPort from Port where id='{this.CurrentMaintain["PulloutPortID"]}'"));

            this.chkIsSeaPort.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select SeaPort from Port where id='{this.CurrentMaintain["PulloutPortID"]}'"));

            base.OnDetailEntered();
        }
    }
}
