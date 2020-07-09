using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txt TPE user
    /// </summary>
    public partial class Txttpeuser : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txttpeuser"/> class.
        /// </summary>
        public Txttpeuser()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox2 { get; private set; }

        /// <inheritdoc/>
        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.DisplayBox1.Text; }
            set { this.DisplayBox1.Text = value; }
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string DisplayBox2Binding
        {
            get { return this.DisplayBox2.Text; }
            set { this.DisplayBox2.Text = value; }
        }

        private void DisplayBox1_TextChanged(object sender, EventArgs e)
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 WITH (NOLOCK) Where id='{0}'", this.DisplayBox1.Text.ToString());
            DataRow dr;
            if (MyUtility.Check.Seek(selectSql, out dr, connectionName: "Production"))
            {
                this.DisplayBox2.Text = MyUtility.Check.Empty(dr["extNo"]) ? string.Empty : dr["Name"].ToString();
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    this.DisplayBox2.Text = this.DisplayBox2.Text + " #" + dr["extNo"].ToString();
                }
            }
            else
            {
                this.DisplayBox2.Text = string.Empty;
            }
        }

        private void DisplayBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql;
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Production.dbo.TPEPass1 WITH (NOLOCK) 
                    where id = @id";
            sqlpar.Add(new SqlParameter("@id", this.DisplayBox1.Text.ToString()));

            UserData ud = new UserData(sql, sqlpar);

            if (ud.ErrMsg == null)
            {
                ud.ShowDialog();
            }
            else
            {
                MyUtility.Msg.ErrorBox(ud.ErrMsg);
            }
        }
    }
}
