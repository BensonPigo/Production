using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txttpeuser : UserControl
    {
        public txttpeuser()
        {
            InitializeComponent();
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox2; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox2
        {
            get { return this.displayBox2; }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        [Bindable(true)]
        public string DisplayBox2Binding
        {
            set { this.displayBox2.Text = value; }
            get { return this.displayBox2.Text; }
        }

        private void displayBox1_TextChanged(object sender, EventArgs e)
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 Where id='{0}'", this.displayBox1.Text.ToString());
            DataRow dr;
            if (MyUtility.Check.Seek(selectSql, out dr))
            {
                this.displayBox2.Text = MyUtility.Check.Empty(dr["extNo"]) ? "" : dr["Name"].ToString();
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    this.displayBox2.Text = this.displayBox2.Text + " #" + dr["extNo"].ToString();
                }
            }
            else
            {
                this.displayBox2.Text = "";
            }
        }
    }
}
