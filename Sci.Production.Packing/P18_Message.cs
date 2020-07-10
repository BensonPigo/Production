using System.Drawing;

namespace Sci.Production.Packing
{
    public partial class P18_Message : Win.Forms.Base
    {
        /// <summary>
        /// P18_Message
        /// </summary>
        public P18_Message()
        {
            this.InitializeComponent();
            this.txt_msg.BackColor = SystemColors.Control;
            this.txt_msg.ForeColor = Color.Black;
            this.ActiveControl = this.label1;
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="msg">msg</param>
        public void Show(string msg)
        {
            this.txt_msg.Text = msg;
            this.ShowDialog();
        }
    }
}
