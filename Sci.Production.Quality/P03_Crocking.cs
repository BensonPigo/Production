using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P03_Crocking : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;

        public P03_Crocking(bool CanEdit, string airID, DataRow mainDr)
        {
            InitializeComponent();
        }
    }
}
