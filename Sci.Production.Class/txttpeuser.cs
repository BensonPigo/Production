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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox2; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.DisplayBox DisplayBox2
        {
            get { return this.displayBox2; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string DisplayBox2Binding
        {
            set { this.displayBox2.Text = value; }
            get { return this.displayBox2.Text; }
        }

        private void displayBox1_TextChanged(object sender, EventArgs e)
        {
            string name = myUtility.Lookup("Name", this.displayBox1.Text.ToString(), "TPEPass1", "ID");
            string extno = myUtility.Lookup("Ext_No", this.displayBox1.Text.ToString(), "TPEPass1", "ID");
            this.displayBox2.Text = name;
            if (!string.IsNullOrWhiteSpace(extno)) { this.displayBox2.Text = name + " #" + extno; }
            
        }
    }
}
