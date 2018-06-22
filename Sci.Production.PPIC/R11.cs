using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;

namespace Sci.Production.PPIC
{
    public partial class R11 : Sci.Win.Tems.PrintForm
    {
        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return base.OnAsyncDataLoad(e);
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            return base.OnToExcel(report);
        }
    }
}
