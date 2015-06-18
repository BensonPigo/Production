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
        private Sci.Win.Forms.Base myForm;
        public txttpeuser()
        {
            InitializeComponent();
            myForm = (Sci.Win.Forms.Base)this.FindForm();
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
            if (myForm.EditMode == false)
            {
                string selectSql = string.Format("Select Name from TPEPass1 Where id='{0}'", this.displayBox1.Text.ToString());
                string name = myUtility.Lookup(selectSql, "Production");
                selectSql = string.Format("Select Ext_No from TPEPass1 Where id='{0}'", this.displayBox1.Text.ToString());
                string extNo = myUtility.Lookup(selectSql, "Production");
                this.displayBox2.Text = name;
                if (!string.IsNullOrWhiteSpace(extNo)) { this.displayBox2.Text = name + " #" + extNo; }
            }        
        }
    }
}
