using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        private string keyWord = Sci.Env.User.Keyword;

        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = string.Format("mDivisionid = '{0}'", keyWord);
            InitializeComponent();
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["mDivisionid"] = keyWord;
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            textCellNo.ReadOnly = true;
        }
    }
}
