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
            print.Enabled = false;
           
           
        }
        string Brand;
        string Year;
        string Month;
        protected override bool ValidateInput()
        {
            Brand = comboBox_brand.SelectedItem.ToString();
            Year = comboBox_year.SelectedItem.ToString();
            Month = comboBox_month.SelectedItem.ToString();
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return base.OnAsyncDataLoad(e);
        }
       

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            return base.OnToExcel(report);
        }

















    }
}
