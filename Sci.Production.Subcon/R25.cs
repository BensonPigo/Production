using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R25 : Sci.Win.Tems.PrintForm
    {
        public R25()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }
        protected override bool ValidateInput()
        {
           // DateTime?=

            

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            
            return base.OnAsyncDataLoad(e);
        }


    }
}
