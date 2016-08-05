using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R26 : Sci.Win.Tems.PrintForm
    {
        public R26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {

            return base.OnAsyncDataLoad(e);
        }
        //按件觸發
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            return base.OnToExcel(report);
        }
    }
}
