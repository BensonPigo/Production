using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.PPIC
{
    public partial class P08_FreightList : Win.Tems.QueryForm
    {
        private DataRow Master;

        public P08_FreightList(DataRow master)
        {
            this.InitializeComponent();
            this.Master = master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("CloseDate", header: "Close Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("WorkingNo", header: "Working No", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("ShipMode", header: "WK-ShipMode", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("TTLAMT", header: "TTL AMT (USD)", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Exchange", header: "Exchange", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
            .Text("Currency", header: "Currency", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("GW", header: "G.W. (kg)", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Freight", header: "Freight", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Insurance", header: "Insurance", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
            .Text("Payer", header: "Payer", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("HCNo", header: "HC No", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Handle", header: "Handle", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;

            this.Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Query()
        {
            string sqlcmd = $@"
select 
[CloseDate] = e.CloseDate
,[WorkingNo] = es.Id
,[ShipMode] = e.ShipModeID
,[TTLAMT] = ROUND( (es.Freight * Supp.Exchange) + (es.Insurance * Supp.Exchange),2) 
,[Exchange] = Supp.Exchange
,[Currency] = Supp.Currencyid
,[GW] = es.GW
,es.Freight
,es.Insurance
,[Payer] = d.Name
,[HCNo] = es.ShipPlanID
,[Handle] = (select dbo.getTPEPass1_ExtNo(es.Handle))
,[Supplier] = CONCAT(e.Forwarder,'-',Supp.AbbEN)
from Export e
inner join Export_ShareAmount es on es.Id = e.ID
left join DropDownList d on d.ID = e.Payer and d.Type = 'WK_Payer'
outer apply(
	select Exchange = (select rate from dbo.GetCurrencyRate('FX',Currencyid,'USD',e.CloseDate))
	,Currencyid,AbbEN
	from Supp 
	where id = e.Forwarder
) Supp
where es.DutyID = '{this.Master["ID"]}'";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.grid1.DataSource = dt;
        }
    }
}
