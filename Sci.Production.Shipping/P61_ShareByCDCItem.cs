using Ict.Win;
using Sci.Production.PublicPrg;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P61_ShareByCDCItem : Win.Tems.QueryForm
    {
        private readonly string nNoEmpty = "_CantEmpty";
        private readonly string nPink = "_Pink";
        private readonly DataTable rateDt;

        /// <inheritdoc/>
        public P61_ShareByCDCItem(DataTable dt, DataTable rateDt)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.gridbs.DataSource = dt.Copy();
            this.rateDt = rateDt.Copy();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("CustomsType", header: "Customs Type", width: Widths.AnsiChars(10), iseditingreadonly: true, name: this.nPink)
                .Text("CDCCode", header: "CDC Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustomsDescription", header: "Customs Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("CDCUnit", header: "CDC Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OriTtlNetKg", header: "Ori Ttl\r\nN.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
                .Numeric("OriTtlWeightKg", header: "Ori Ttl\r\nG.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
                .Numeric("OriTtlCDCAmount", header: "Ori Ttl CDC\r\nAmount", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
                .Numeric("ActTtlNetKg", header: "Act. Ttl\r\nN.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, name: this.nNoEmpty + this.nPink)
                .Numeric("ActTtlWeightKg", header: "Act. Ttl\r\nG.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, name: this.nNoEmpty + this.nPink)
                .Numeric("ActTtlAmount", header: "Act. Ttl\r\nAmount", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, name: this.nNoEmpty + this.nPink)
                .Text("ActHSCode", header: "Act.\r\nHS Code", width: Widths.AnsiChars(14), iseditingreadonly: false, name: this.nNoEmpty + this.nPink)
                ;

            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[8].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[9].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!Prgs.CheckEmptyColumn((DataTable)this.gridbs.DataSource, Prgs.GetColumnsDataPropertyNameWithTag(this.grid1, this.nNoEmpty)))
            {
                MyUtility.Msg.WarningBox("<Act. Ttl N.W.>, <Act. Ttl G.W.>, <Act. Ttl CDC Amount>, <Act. HS Code> cannot be empty.");
                return;
            }

            DataTable sumDt = (DataTable)this.gridbs.DataSource;
            string filter = "CustomsType = '{0}' and CustomsDescription = '{1}'";
            var calDatas = this.rateDt.AsEnumerable().Select(s => new
            {
                rn = MyUtility.Convert.GetLong(s["rn"]),
                ActNetKg = MyUtility.Math.Round(
                    MyUtility.Convert.GetDecimal(s["RateNetKg"])
                    * MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CustomsDescription"].ToString()))[0]["ActTtlNetKg"]), 2),
                ActWeightKg = MyUtility.Math.Round(
                    MyUtility.Convert.GetDecimal(s["RateWeightKg"])
                    * MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CustomsDescription"].ToString()))[0]["ActTtlWeightKg"]), 2),
                ActAmount = MyUtility.Math.Round(
                    MyUtility.Convert.GetDecimal(s["RateCDCAmount"])
                    * MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CustomsDescription"].ToString()))[0]["ActTtlAmount"]), 2),
                ActHSCode = MyUtility.Convert.GetString(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CustomsDescription"].ToString()))[0]["ActHSCode"]),
            }).ToList();
            P61.ShareDt = PublicPrg.ListToDataTable.ToDataTable(calDatas);
            this.Close();
        }
    }
}
