using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        DateTime? LastDate; DateTime? ArrDate; DateTime? SCIDate; DateTime? SewDate; DateTime? Est;
        string sp1; string sp2; string Sea; string Brand; string Ref; string Cate; string Supp;
        string Over;
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable ORS = null;
            string sqlm = (@"select ID FROM DBO.MDivision");
            DBProxy.Current.Select("", sqlm, out ORS);
            ORS.Rows.Add(new string[] { "" });
            ORS.DefaultView.Sort = "ID";
            this.Over_comboBox.DataSource = ORS;
            this.Over_comboBox.ValueMember = "ID";
            this.Over_comboBox.DisplayMember = "ID";
            this.Over_comboBox.SelectedIndex = 0;
       
            print.Enabled = false;
        }

       
    }
}
