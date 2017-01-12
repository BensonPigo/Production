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
    public partial class txtSeq : Sci.Win.UI._UserControl
    {
        public txtSeq()
        {
            InitializeComponent();
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            set { this.textSeq1.Text = value; }
            get { return textSeq1.Text; }
        }

        [Bindable(true)]
        public string TextBox2Binding
        {
            set { this.textSeq2.Text = value; }
            get { return textSeq2.Text; }
        }
    }
}
