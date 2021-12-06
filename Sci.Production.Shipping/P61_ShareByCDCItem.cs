using Ict.Win;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
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
                .Text("CustomsType", header: "Customs Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CDCCode", header: "CDC Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustomsDescription", header: "Customs Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Numeric("TtlCDCQty", header: "CDC Qty", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
                .Text("CDCUnit", header: "CDC Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OriTtlNetKg", header: "Ori Ttl\r\nN.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
                .Numeric("OriTtlWeightKg", header: "Ori Ttl\r\nG.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
                .Numeric("OriTtlCDCAmount", header: "Ori Ttl CDC\r\nAmount", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
                .Numeric("ActCDCQty", header: "Act. CDC \r\nQty.", width: Widths.AnsiChars(11), decimal_places: 4, integer_places: 5, name: this.nNoEmpty)
                .Numeric("ActTtlNetKg", header: "Act. Ttl\r\nN.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, name: this.nNoEmpty)
                .Numeric("ActTtlWeightKg", header: "Act. Ttl\r\nG.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, name: this.nNoEmpty)
                .Numeric("ActTtlAmount", header: "Act. Ttl\r\nAmount", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, name: this.nNoEmpty)
                .Text("ActHSCode", header: "Act.\r\nHS Code", width: Widths.AnsiChars(14), iseditingreadonly: false, name: this.nNoEmpty)
                ;

            this.grid1.Columns[8].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[9].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[11].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[12].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!Prgs.CheckEmptyColumn((DataTable)this.gridbs.DataSource, Prgs.GetColumnsDataPropertyNameWithTag(this.grid1, this.nNoEmpty)))
            {
                MyUtility.Msg.WarningBox("<Act. CDC Qty>, <Act. Ttl N.W.>, <Act. Ttl G.W.>, <Act. Ttl CDC Amount>, <Act. HS Code> cannot be empty.");
                return;
            }

            P61.KHImportDeclaration_ShareCDCExpense.Clear();
            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                DataRow newdr = P61.KHImportDeclaration_ShareCDCExpense.NewRow();
                newdr["KHCustomsDescriptionCDCName"] = dr["CustomsDescription"];
                newdr["OriTtlNetKg"] = dr["OriTtlNetKg"];
                newdr["OriTtlWeightKg"] = dr["OriTtlWeightKg"];
                newdr["OriTtlCDCAmount"] = dr["OriTtlCDCAmount"];
                newdr["ActCDCQty"] = dr["ActCDCQty"];
                newdr["ActTtlNetKg"] = dr["ActTtlNetKg"];
                newdr["ActTtlWeightKg"] = dr["ActTtlWeightKg"];
                newdr["ActTtlAmount"] = dr["ActTtlAmount"];
                newdr["ActHSCode"] = dr["ActHSCode"];
                P61.KHImportDeclaration_ShareCDCExpense.Rows.Add(newdr);
            }

            DataTable sumDt = ((DataTable)this.gridbs.DataSource).Select($"ct > 1").TryCopyToDataTable((DataTable)this.gridbs.DataSource);
            DataTable sumDtct1 = ((DataTable)this.gridbs.DataSource).Select($"ct = 1").TryCopyToDataTable((DataTable)this.gridbs.DataSource);
            var ct1 = sumDtct1.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["CustomsDescription"])).ToList();
            var ct1Datas = sumDtct1.AsEnumerable()
                .Select(s => new CalData
                {
                    Rn = MyUtility.Convert.GetLong(this.rateDt.Select($"CustomsDescription = '{s["CustomsDescription"]}'")[0]["Rn"]),
                    CustomsType = MyUtility.Convert.GetString(s["CustomsType"]),
                    CustomsDescription = MyUtility.Convert.GetString(s["CustomsDescription"]),
                    ActNetKg = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["ActTtlNetKg"]), 2),
                    ActWeightKg = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["ActTtlWeightKg"]), 2),
                    ActAmount = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["ActTtlAmount"]), 2),
                    ActHSCode = MyUtility.Convert.GetString(s["ActHSCode"]),
                }).ToList();

            string filter = "CustomsType = '{0}' and CustomsDescription = '{1}'";
            var calDatas = this.rateDt.AsEnumerable().Where(w => !ct1.Contains(MyUtility.Convert.GetString(w["CustomsDescription"])))
                .Select(s => new CalData
            {
                Rn = MyUtility.Convert.GetLong(s["rn"]),
                CustomsType = MyUtility.Convert.GetString(s["CustomsType"]),
                CustomsDescription = MyUtility.Convert.GetString(s["CustomsDescription"]),
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

            var groupNew = calDatas
                .GroupBy(g => new { g.CustomsType, g.CustomsDescription })
                .Select(s => new { s.Key.CustomsType, s.Key.CustomsDescription, maxrn = s.Max(m => m.Rn), ActNetKg = s.Sum(su => su.ActNetKg), ActWeightKg = s.Sum(su => su.ActWeightKg), ActAmount = s.Sum(su => su.ActAmount) })
                .ToList();

            var lastDiff = groupNew.Select(s => new
            {
                Rn = s.maxrn,
                s.CustomsType,
                s.CustomsDescription,
                ActNetKg = s.ActNetKg - MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["ActTtlNetKg"]),
                ActWeightKg = s.ActWeightKg - MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["ActTtlWeightKg"]),
                ActAmount = s.ActAmount - MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["ActTtlAmount"]),
            }).ToList();

            foreach (var item in calDatas)
            {
                var lastSamern = lastDiff.Where(w => w.CustomsType == item.CustomsType && w.CustomsDescription == item.CustomsDescription).First();
                if (lastSamern.Rn == item.Rn)
                {
                    item.ActNetKg -= lastSamern.ActNetKg;
                    item.ActWeightKg -= lastSamern.ActWeightKg;
                    item.ActAmount -= lastSamern.ActAmount;
                }
            }

            P61.ShareDt = ListToDataTable.ToDataTable(calDatas);
            P61.ShareDt.Merge(ListToDataTable.ToDataTable(ct1Datas));
            this.Close();
        }

        private class CalData
        {
            public long Rn { get; set; }

            public string CustomsType { get; set; }

            public string CustomsDescription { get; set; }

            public decimal ActNetKg { get; set; }

            public decimal ActWeightKg { get; set; }

            public decimal ActAmount { get; set; }

            public string ActHSCode { get; set; }
        }
    }
}
