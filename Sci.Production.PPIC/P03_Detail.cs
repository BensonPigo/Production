using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P03_Detail : Sci.Win.Subs.Input6A
    {
        public P03_Detail()
        {
            InitializeComponent();

            txtuser2.TextBox1.ReadOnly = true;
            txtuser2.TextBox1.IsSupportEditMode = false;
        }
    }
}
