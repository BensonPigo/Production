using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Andy;
using Ict.Win;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P31_FTYRegularSendingScheduleHistory
    /// </summary>
    internal partial class P02_FTYRegularSendingScheduleHistory : Sci.Win.Subs.Base
    {
        private string regionCode;
        private string toAlias;

        /// <summary>
        /// P31_FTYRegularSendingScheduleHistory
        /// </summary>
        /// <param name="regioncode">RegionCode</param>
        /// <param name="toalias">toalias</param>
        public P02_FTYRegularSendingScheduleHistory(string regioncode, string toalias)
        {
            this.InitializeComponent();
            this.regionCode = regioncode;
            this.toAlias = toalias;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.InitialData();
            this.SetupGrid();
        }

        private void InitialData()
        {
            var dt = mySqlClient.Select(null, $"select * from FactoryExpress_SendingScheduleHistory where RegionCode = '{this.regionCode}' and ToAlias = '{this.toAlias}' order by BeginDate");
            dt.ColumnsStringAdd("CreateBy");
            dt.ColumnsStringAdd("EditBy");
            dt.AsEnumerable().ToList().ForEach(r =>
            {
                r["CreateBy"] = Sci.Production.Class.Commons.UserPrg.GetName(r["AddName"], Sci.Production.Class.Commons.UserPrg.NameType.idAndNameAndExt);
                r["EditBy"] = Sci.Production.Class.Commons.UserPrg.GetName(r["EditName"], Sci.Production.Class.Commons.UserPrg.NameType.idAndNameAndExt);
            });
            this.grid1.DataSource = dt;
        }

        private void SetupGrid()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Country", header: "Country", width: Widths.AnsiChars(3))
                .Text("RegionCode", header: "Region Code", width: Widths.AnsiChars(5))
                .Text("ToAlias", header: "To", width: Widths.AnsiChars(10))
                .CheckBox("SUN", header: "SUN", width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .CheckBox("MON", header: "MON", width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .CheckBox("TUE", header: "TUE", width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .CheckBox("WED", header: "WED", width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .CheckBox("THU", header: "THU", width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .CheckBox("FRI", header: "FRI", width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .CheckBox("SAT", header: "SAT", width: Widths.AnsiChars(3), trueValue: true, falseValue: false)
                .Date("BeginDate", header: "Begin Date", width: Widths.AnsiChars(10))
                .Date("EndDate", header: "End Date", width: Widths.AnsiChars(10))
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(35))
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(15));
            this.grid1.IsEditingReadOnly = true;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
