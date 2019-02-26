using Sci.Win.Tems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class B41 : Input1
    {
        public B41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtLocation.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("<Location> can not be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
