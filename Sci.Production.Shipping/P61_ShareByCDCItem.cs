﻿using Ict.Win;
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
                .Text("CDCName", header: "Customs Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Numeric("TtlCDCQty", header: "CDC Qty", width: Widths.AnsiChars(9), decimal_places: 2, iseditingreadonly: true)
                .Text("CDCUnit", header: "CDC Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OriTtlNetKg", header: "Ori Ttl\r\nN.W.", width: Widths.AnsiChars(11), decimal_places: 2, iseditingreadonly: true)
                .Numeric("OriTtlWeightKg", header: "Ori Ttl\r\nG.W.", width: Widths.AnsiChars(11), decimal_places: 2, iseditingreadonly: true)
                .Numeric("OriTtlCDCAmount", header: "Ori Ttl CDC\r\nAmount", width: Widths.AnsiChars(11), decimal_places: 2, iseditingreadonly: true)
                .Numeric("ActCDCQty", header: "Act. CDC \r\nQty.", width: Widths.AnsiChars(11), decimal_places: 4, name: this.nNoEmpty)
                .Numeric("ActTtlNetKg", header: "Act. Ttl\r\nN.W.", width: Widths.AnsiChars(11), decimal_places: 2, name: this.nNoEmpty)
                .Numeric("ActTtlWeightKg", header: "Act. Ttl\r\nG.W.", width: Widths.AnsiChars(11), decimal_places: 2, name: this.nNoEmpty)
                .Numeric("ActTtlAmount", header: "Act. Ttl\r\nAmount", width: Widths.AnsiChars(11), decimal_places: 2, name: this.nNoEmpty)
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

            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                if (MyUtility.Convert.GetDecimal(dr["OriTtlNetKg"]) >= 10000000)
                {
                    MyUtility.Msg.WarningBox("<Ori Ttl N.W.> can't over than 10,000,000");
                    return;
                }

                if (MyUtility.Convert.GetDecimal(dr["OriTtlWeightKg"]) >= 10000000)
                {
                    MyUtility.Msg.WarningBox("<Ori Ttl G.W.> can't over than 10,000,000");
                    return;
                }

                if (MyUtility.Convert.GetDecimal(dr["OriTtlCDCAmount"]) >= 100000000)
                {
                    MyUtility.Msg.WarningBox("<Ori Ttl CDC Amount> can't over than 100,000,000");
                    return;
                }

                if (MyUtility.Convert.GetDecimal(dr["ActTtlNetKg"]) >= 100000)
                {
                    MyUtility.Msg.WarningBox("<Act. Ttl N.W.> can't over than 100,000");
                    return;
                }

                if (MyUtility.Convert.GetDecimal(dr["ActTtlWeightKg"]) >= 100000)
                {
                    MyUtility.Msg.WarningBox("<Act. Ttl G.W.> can't over than 100,000");
                    return;
                }

                if (MyUtility.Convert.GetDecimal(dr["ActTtlAmount"]) >= 100000000)
                {
                    MyUtility.Msg.WarningBox("<Act. Ttl Amount> can't over than 100,000,000");
                    return;
                }

                if (MyUtility.Convert.GetDecimal(dr["ActCDCQty"]) >= 100000000)
                {
                    MyUtility.Msg.WarningBox("<Act. CDC Qty.> can't over than 100,000,000");
                    return;
                }

                if (MyUtility.Convert.GetString(dr["ActHSCode"]).Length > 14)
                {
                    MyUtility.Msg.WarningBox("<Act.HS Code> length can't over than 14");
                    return;
                }
            }

            P61.KHImportDeclaration_ShareCDCExpense.Clear();
            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                DataRow newdr = P61.KHImportDeclaration_ShareCDCExpense.NewRow();
                newdr["KHCustomsDescriptionCDCName"] = dr["CDCName"];
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
            var ct1 = sumDtct1.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["CDCName"])).ToList();
            var ct1Datas = sumDtct1.AsEnumerable()
                .Select(s => new CalData
                {
                    Rn = MyUtility.Convert.GetLong(this.rateDt.Select($"CDCName = '{s["CDCName"]}'")[0]["Rn"]),
                    CustomsType = MyUtility.Convert.GetString(s["CustomsType"]),
                    CDCName = MyUtility.Convert.GetString(s["CDCName"]),
                    ActNetKg = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["ActTtlNetKg"]), 2),
                    ActWeightKg = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["ActTtlWeightKg"]), 2),
                    ActAmount = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["ActTtlAmount"]), 2),
                    ActHSCode = MyUtility.Convert.GetString(s["ActHSCode"]),
                }).ToList();

            string filter = "CustomsType = '{0}' and CDCName = '{1}'";
            var calDatas = this.rateDt.AsEnumerable().Where(w => !ct1.Contains(MyUtility.Convert.GetString(w["CDCName"])))
                .Select(s => new CalData
            {
                Rn = MyUtility.Convert.GetLong(s["rn"]),
                CustomsType = MyUtility.Convert.GetString(s["CustomsType"]),
                    CDCName = MyUtility.Convert.GetString(s["CDCName"]),
                ActNetKg = MyUtility.Math.Round(
                    MyUtility.Convert.GetDecimal(s["RateNetKg"])
                    * MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CDCName"].ToString()))[0]["ActTtlNetKg"]), 2),
                ActWeightKg = MyUtility.Math.Round(
                    MyUtility.Convert.GetDecimal(s["RateWeightKg"])
                    * MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CDCName"].ToString()))[0]["ActTtlWeightKg"]), 2),
                ActAmount = MyUtility.Math.Round(
                    MyUtility.Convert.GetDecimal(s["RateCDCAmount"])
                    * MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CDCName"].ToString()))[0]["ActTtlAmount"]), 2),
                ActHSCode = MyUtility.Convert.GetString(sumDt.Select(string.Format(filter, s["CustomsType"].ToString(), s["CDCName"].ToString()))[0]["ActHSCode"]),
            }).ToList();

            var groupNew = calDatas
                .GroupBy(g => new { g.CustomsType, g.CDCName })
                .Select(s => new { s.Key.CustomsType, s.Key.CDCName, maxrn = s.Max(m => m.Rn), ActNetKg = s.Sum(su => su.ActNetKg), ActWeightKg = s.Sum(su => su.ActWeightKg), ActAmount = s.Sum(su => su.ActAmount) })
                .ToList();

            var lastDiff = groupNew.Select(s => new
            {
                Rn = s.maxrn,
                s.CustomsType,
                s.CDCName,
                ActNetKg = s.ActNetKg - MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CDCName))[0]["ActTtlNetKg"]),
                ActWeightKg = s.ActWeightKg - MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CDCName))[0]["ActTtlWeightKg"]),
                ActAmount = s.ActAmount - MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CDCName))[0]["ActTtlAmount"]),
            }).ToList();

            foreach (var item in calDatas)
            {
                var lastSamern = lastDiff.Where(w => w.CustomsType == item.CustomsType && w.CDCName == item.CDCName).First();
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

            public string CDCName { get; set; }

            public decimal ActNetKg { get; set; }

            public decimal ActWeightKg { get; set; }

            public decimal ActAmount { get; set; }

            public string ActHSCode { get; set; }
        }
    }
}
