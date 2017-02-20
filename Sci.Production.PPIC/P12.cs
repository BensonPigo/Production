using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P12 : Sci.Win.Tems.Input1
    {
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            numericBox3.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format("select isnull(sum(QAQty),0) as QAQty from SewingOutput_Detail WITH (NOLOCK) where OrderId = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))));
        }
    }
}
