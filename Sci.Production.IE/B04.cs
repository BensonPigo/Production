using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override void ClickNewAfter()
        {

            this.txtID.ReadOnly = false;
            this.txtDescription.ReadOnly = false;
            base.ClickNewAfter();
        }

        protected override void ClickEditAfter()
        {
            this.txtID.ReadOnly = true;
            this.txtDescription.ReadOnly = false;
            base.ClickEditAfter();
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< ID > can not be empty!");
                this.txtID.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.txtDescription.Focus();
                return false;
            }
            this.CurrentMaintain["Type"] = "CP";

            #endregion
            return base.ClickSaveBefore();
        }
    }
}
