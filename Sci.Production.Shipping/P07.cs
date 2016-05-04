using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P07 : Sci.Win.Tems.Input6
    {
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "1,Wrong Price,2,Wrong Qty,3,Wrong Price & Qty,4,NoPullout");
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format("select *,IIF(NewItem = 1,'Y','') as New from InvAdjust_Qty where ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Q'ty", decimal_places: 0, iseditingreadonly: true)
                .Numeric("OrigQty", header: "Original", decimal_places: 0, iseditingreadonly: true)
                .Numeric("AdjustQty", header: "Change to", decimal_places: 0, iseditingreadonly: true)
                .Numeric("Price", header: "U'Price", decimal_places: 2, iseditingreadonly: true)
                .Text("New", header: "New", width: Widths.AnsiChars(1), iseditingreadonly: true);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            numericBox5.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["OrigSurcharge"]);
            numericBox6.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustSurcharge"]);
            numericBox9.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["OrigCommission"]);
            numericBox10.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustCommission"]);
            numericBox11.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["OrigPulloutAmt"]) + MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["OrigSurcharge"])) + MyUtility.Convert.GetDecimal(CurrentMaintain["OrigAddCharge"]) - MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["OrigPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["OrigCommission"]), 2);
            numericBox12.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustPulloutAmt"]) + MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustSurcharge"])) + MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustAddCharge"]) - MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustPulloutQty"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["AdjustCommission"]), 2);

        }
    }
}
