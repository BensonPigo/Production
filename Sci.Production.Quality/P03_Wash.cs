using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P03_Wash : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;

        public P03_Wash(bool CanEdit, string airID, DataRow mainDr)
        {
            InitializeComponent();
        }
    }
}
