using System;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class Msg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Msg"/> class.
        /// </summary>
        public Msg()
        {
            this.InitializeComponent();
            this.picInfo.Image = System.Drawing.SystemIcons.Information.ToBitmap();
        }

        /// <summary>
        /// Show Msg
        /// </summary>
        /// <param name="msg">msg</param>
        public void Show(string msg)
        {
            this.editBoxMsg.Text = msg;
            this.ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
