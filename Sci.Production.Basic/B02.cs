using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Sci.Production.Basic
{
    public partial class B02 : Sci.Win.Tems.Input7
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            button1.ForeColor = MyUtility.GetValue.Lookup("select isnull(count(ID),0) from MailTo WITH (NOLOCK) ") == "0" ? Color.Black : Color.Blue;
        }

        //取Sketch目錄的路徑
        private void button2_Click(object sender, EventArgs e)
        {
            string dir = getDir();
            if (!MyUtility.Check.Empty(dir))
            {
                textBox8.Text = dir;
            }
        }

        //取Clip目錄的路徑
        private void button3_Click(object sender, EventArgs e)
        {
            string dir = getDir();
            if (!MyUtility.Check.Empty(dir))
            {
                textBox9.Text = dir;
            }
        }

        private string getDir()
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            return path.SelectedPath;
        }

        //Mail To
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B02_MailTo callNextForm = new Sci.Production.Basic.B02_MailTo(this.IsSupportEdit, null, null, null);
            callNextForm.ShowDialog(this);
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtuser1.TextBox1.Select();
        }
    }
}
