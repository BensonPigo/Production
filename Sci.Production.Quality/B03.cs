using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class B03 : Sci.Win.Tems.Input1
    {
        DualResult result;
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
           // this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Scale Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            #endregion
            return base.ClickSaveBefore();
        }
    }
}
