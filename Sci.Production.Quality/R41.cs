using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R41 : Sci.Win.Tems.PrintForm
    {
          public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            print.Enabled = false;
        }
          string Brand;
          string Year;
          protected override bool ValidateInput()
          {
              Brand = combo_Brand.Text.ToString();
              Year = combo_Year.Text.ToString();
              return base.ValidateInput();
          }


          protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
          {
              return base.OnAsyncDataLoad(e);
          }

          protected override bool OnToExcel(Win.ReportDefinition report)
          {
              return true;
          }

    }
}
