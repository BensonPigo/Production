using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R43 : Sci.Win.Tems.PrintForm
    {
        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            
        }

        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

       
       

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            return base.OnToExcel(report);
        }

















    }
}
