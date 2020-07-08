using System;
using System.Data;
using Ict.Win;

namespace Sci.Production.PublicForm
{
    public partial class PFHis : Sci.Win.Subs.Input4
    {
        private DataRow drOrders;

        public PFHis(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow drOrders)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.drOrders = drOrders;

            DateTime? dtStr = MyUtility.Convert.GetDate(drOrders["CFMDate"]).Value.AddDays(14);
            DateTime? dtLETA = MyUtility.Convert.GetDate(drOrders["LETA"]);

            if (!dtLETA.Empty())
            {
                this.labLockKPILETA.Text = dtStr.Value.ToString("yyyy/MM/dd") + " Lock KPI L/ETA To " + dtLETA.Value.ToString("yyyy/MM/dd");
            }
            else
            {
                this.labLockKPILETA.Text = dtStr.Value.ToString("yyyy/MM/dd") + " Lock KPI L/ETA To ";
            }
        }

        /// <inheritdoc />
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Date("OldSciDelivery", header: "Ori SCI Del.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("NewSciDelivery", header: "New SCI Del.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("LETA", header: "KPI L/ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("PackLETA", header: "Pack L/ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("AddInfo", header: "Create by", width: Widths.AnsiChars(40), iseditingreadonly: true);

            return true;
        }

        /// <inheritdoc />
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            DataTable detail = datas;
            detail.Columns.Add("AddInfo", typeof(string));
            foreach (DataRow row in detail.Rows)
            {
                string create = MyUtility.GetValue.Lookup($@"select id+'-'+Name from TPEpass1 where id = '{row["AddName"]}'");
                row["AddInfo"] = create + " " + row["AddDate"].ToString();
            }
        }
    }
}
