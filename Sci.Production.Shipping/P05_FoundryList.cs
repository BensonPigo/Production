using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P05_FoundryList : Sci.Win.Subs.Base
    {
        private string GMTBookingID;
        private string ShipModeID;

        public P05_FoundryList(string _GMTBookingID,string _ShipModeID)
        {
            InitializeComponent();
            this.GMTBookingID = _GMTBookingID;
            this.ShipModeID = _ShipModeID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("FactoryGroup", header: "Fty. Group", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("CBM", header: "CBM", width: Widths.AnsiChars(11), decimal_places: 3, iseditingreadonly: true)
            .Numeric("Ratio", header: "Ratio", decimal_places: 0, width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;

            this.Query();
        }

        private void Query()
        {
            string cmd = string.Empty;

            cmd = $@"
select FactoryGroup
,CBM = iif('SEA' = '{this.ShipModeID}', CBM, GW)
,Ratio
from GarmentInvoice_Foundry 
where InvoiceNo = '{this.GMTBookingID}'
order by FactoryGroup
";
            DataTable gridData;
            if (DBProxy.Current.Select(null, cmd, out gridData))
            {
                this.listControlBindingSource1.DataSource = gridData;
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query data fail!!");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
