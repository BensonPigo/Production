using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Warehouse
{
    public partial class P02_Cartondetail : Win.Forms.Base
    {
        private string Export_DetailUkey;

        public P02_Cartondetail(string export_DetailUkey)
        {
            this.InitializeComponent();
            this.Export_DetailUkey = export_DetailUkey;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string sqlcmd = $@"
select 
	edc.poid,
	seq = concat(edc.seq1,'-',edc.seq2),
	Supp = (ed.SuppID+'-'+s.AbbEN) ,
	edc.Carton,	edc.LotNo,	edc.Qty,	edc.Foc,	edc.NetKg,	edc.WeightKg
from [Export_Detail_Carton] edc WITH (NOLOCK)
inner join Export_Detail ed WITH (NOLOCK) on ed.ukey = edc.Export_DetailUkey
left join Supp s WITH (NOLOCK) on s.id = ed.SuppID 
where Export_DetailUkey = '{this.Export_DetailUkey}'
order by edc.poid,edc.seq1,edc.seq2,edc.Carton";
            DataTable dt;
            DualResult result;
            result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Poid", header: "PO#", width: Widths.AnsiChars(16))
            .Text("seq", header: "Seq", width: Widths.AnsiChars(8))
            .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20))
            .Text("Carton", header: "C/No", width: Widths.AnsiChars(20))
            .Text("LotNo", header: "LotNo", width: Widths.AnsiChars(10))
            .Text("Qty", header: "ShipQty", width: Widths.AnsiChars(6))
            .Text("Foc", header: "ShipFoc", width: Widths.AnsiChars(10))
            .Text("NetKg", header: "NW", width: Widths.AnsiChars(6))
            .Text("WeightKg", header: "GW", width: Widths.AnsiChars(6))
            ;
            this.grid1.AutoResizeColumns();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
