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
    /// <summary>
    /// CustomQuestionBox
    /// </summary>
    public partial class CustomQuestionBox : Form
    {
        /// <summary>
        /// CustomQuestionBox
        /// </summary>
        /// <param name="msg">msg</param>
        /// <param name="title">title</param>
        /// <param name="buttonOKText">buttonOKText</param>
        /// <param name="buttonCancelText">buttonCancelText</param>
        public CustomQuestionBox(string msg, string title, string buttonOKText, string buttonCancelText)
        {
            this.InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.lblMsg.Text = msg;
            this.Text = title;
            this.btnOK.Text = buttonOKText;
            this.btnCancel.Text = buttonCancelText;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ShowDialog
        /// </summary>
        /// <param name="msg">msg</param>
        /// <param name="title">title</param>
        /// <param name="buttonOKText">buttonOKText</param>
        /// <param name="buttonCancelText">buttonCancelText</param>
        /// <returns>DialogResult</returns>
        public static DialogResult ShowDialog(string msg, string title, string buttonOKText, string buttonCancelText)
        {
            return new CustomQuestionBox(msg, title, buttonOKText, buttonCancelText).ShowDialog();
        }
    }
}
