using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class txttpeuser : Sci.Win.UI._UserControl
    {
        public txttpeuser()
        {
            this.InitializeComponent();
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox2
        {
            get { return this.displayBox2; }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.displayBox1.Text; }
            set { this.displayBox1.Text = value; }
        }

        [Bindable(true)]
        public string DisplayBox2Binding
        {
            get { return this.displayBox2.Text; }
            set { this.displayBox2.Text = value; }
        }

        private void displayBox1_TextChanged(object sender, EventArgs e)
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 WITH (NOLOCK) Where id='{0}'", this.displayBox1.Text.ToString());
            DataRow dr;
            if (MyUtility.Check.Seek(selectSql, out dr, connectionName: "Production"))
            {
                this.displayBox2.Text = MyUtility.Check.Empty(dr["extNo"]) ? string.Empty : dr["Name"].ToString();
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    this.displayBox2.Text = this.displayBox2.Text + " #" + dr["extNo"].ToString();
                }
            }
            else
            {
                this.displayBox2.Text = string.Empty;
            }
        }

        private void displayBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql;
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Production.dbo.TPEPass1 WITH (NOLOCK) 
                    where id = @id";
            sqlpar.Add(new SqlParameter("@id", this.displayBox1.Text.ToString()));

            userData ud = new userData(sql, sqlpar);

            if (ud.errMsg == null)
            {
                ud.ShowDialog();
            }
            else
            {
                MyUtility.Msg.ErrorBox(ud.errMsg);
            }
        }
    }
}
