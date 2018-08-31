using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    public partial class msg : Form
    {
        public msg()
        {
            InitializeComponent();
            this.picInfo.Image = System.Drawing.SystemIcons.Information.ToBitmap();
        }

        public void Show(string msg)
        {
            this.editBoxMsg.Text = msg;
            this.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
