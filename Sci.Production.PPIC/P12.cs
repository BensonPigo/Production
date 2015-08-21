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
            this.DefaultFilter = "FTYGroup = '" + Sci.Env.User.Factory + "'";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            numericBox3.Value = Convert.ToDecimal(MyUtility.GetValue.Lookup(string.Format("select isnull(sum(QAQty),0) as QAQty from SewingOutput_Detail where OrderId = '{0}'",CurrentMaintain["ID"].ToString())));
        }
    }
}
