using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B25 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboBox1, 2, 1, "A,Accessory,F,Fabric");
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["AOS_InspQtyOption"]) == 1 &&
                MyUtility.Convert.GetInt(this.CurrentMaintain["InspectedPercentage"]) == 0)
            {
                MyUtility.Msg.WarningBox("<Inspected %> can not be empty or 0");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
