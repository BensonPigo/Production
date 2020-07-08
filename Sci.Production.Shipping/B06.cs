using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B06 : Sci.Win.Tems.QueryForm
    {
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("ExportPort", header: "Loading Port", width: Widths.AnsiChars(20))
            .Text("ExportCountry", header: "Origin", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("ImportCountry", header: "Destination", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Vessel", header: "Vessel Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
            ;
        }

        private void Query()
        {
            string sqlcmd = "select * from Door2DoorDelivery";

            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }
    }
}
