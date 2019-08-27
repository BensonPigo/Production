using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B09 : Sci.Win.Tems.Input1
    {
        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["DM300"] = DBNull.Value;
        }

        protected override bool ClickSaveBefore()
        {
            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.Lookup(@"
select MAXID = case when max(DM300) >= 0 then max(DM300) +1 else 0 end
from FinishingProcess");
                this.CurrentMaintain["DM300"] = id;
            }

            return base.ClickSaveBefore();
        }
    }
}
