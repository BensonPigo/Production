using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P01_ExpectionFormRemark : Sci.Win.Forms.Base
    {
        private string _ExpectionFormRemark;
        public P01_ExpectionFormRemark(string ExpectionFormRemark)
        {
            _ExpectionFormRemark = ExpectionFormRemark;
            InitializeComponent();
        }
        protected override void OnFormLoaded()
        {
            displayBox1.Text = _ExpectionFormRemark;
            base.OnFormLoaded();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
