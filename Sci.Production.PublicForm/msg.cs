using System;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    public partial class msg : Form
    {
        public msg()
        {
            this.InitializeComponent();
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
