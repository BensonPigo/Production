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
    public partial class B01 : Sci.Win.Tems.Input6
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("WhseNo", header: "Warehouse#", width: Widths.AnsiChars(50));
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("< Brand > can not be empty!");
                txtbrand1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Ship Mode > can not be empty!");
                txtshipmode1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Forwarder"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Forwarder > can not be empty!");
                this.txtsubcon1.Focus();
                return false;
            }

            int recordCount = 0;
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["WhseNo"]))
                {
                    dr.Delete();
                    continue;
                }
                recordCount += 1;
            }

            if (recordCount == 0)
            {
                MyUtility.Msg.WarningBox("Details data can't empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
