using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Shipping
{
    public partial class P15_Containers : Sci.Win.Tems.Base
    {
        private string _ExportId;

        public P15_Containers(string ExportId)
        {
            this.InitializeComponent();
            this._ExportId = ExportId;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataTable dt;

            #region SQL 貨櫃詳細資料
            string cmd = $@"
-- 貨櫃詳細資料
select	e.ID
		, ec.Type
		, container = concat(ec.Container,'(',e.ID,')' )
		, ec.WeightKg
		, ec.CartonQty
from dbo.Export as e
inner join dbo.Export_Container as ec on ec.ID = e.ID
where e.ID = '{this._ExportId}'
ORDER BY ec.Seq
";
            #endregion
            DualResult result = DBProxy.Current.Select(null, cmd, out dt);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Type", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("container", header: "Container", iseditingreadonly: true, width: Widths.AnsiChars(25))
                .Numeric("WeightKg", header: "G.W.", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(10))
                .Numeric("CartonQty", header: "Ttl Packages", iseditingreadonly: true, decimal_places: 0, width: Widths.AnsiChars(5))
                ;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
