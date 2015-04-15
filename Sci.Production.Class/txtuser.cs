using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtuser : UserControl
    {
        public txtuser()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string TextBox1Binding
        {
            set { this.textBox1.Text = value; }
            get { return textBox1.Text; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string str = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            {
                if (!myUtility.Seek(str, "Pass1", "ID"))
                {
                    bool IsUserName = myUtility.Seek(str, "Pass1", "Name");
                    bool IsUserExtNo = myUtility.Seek(str, "Pass1", "Ext_No");

                    if (IsUserName | IsUserExtNo)
                    {
                        if (IsUserName)
                        {
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Pass1.ID,Name,Ext_No,Factory", "15,30,10,150", this.textBox1.Text);
                            //select ID, Name, Ext_No, Factory from Pass1 where Name = str
                            DialogResult result = item.ShowDialog();
                            if (result == DialogResult.Cancel) { return; }
                            this.textBox1.Text = item.GetSelectedString();
                        }
                        else
                        {
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Pass1.ID,Name,Ext_No,Factory", "15,30,10,150", this.textBox1.Text);
                            //select ID, Name, Ext_No, Factory from Pass1 where Ext_No = str
                            DialogResult result = item.ShowDialog();
                            if (result == DialogResult.Cancel) { return; }
                            this.textBox1.Text = item.GetSelectedString();
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("< User Id: {0} > not found!!!", str));
                        this.textBox1.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string name = myUtility.Lookup("Name", this.textBox1.Text.ToString(), "Pass1", "ID");
            string extno = myUtility.Lookup("Ext_No", this.textBox1.Text.ToString(), "Pass1", "ID");
            this.displayBox1.Text = name + extno;
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Pass1.ID,Name,Ext_No,Factory", "15,30,10,150", this.textBox1.Text);
            //select ID, Name, Ext_No, Factory from Pass1 where Resign is null
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
        }
    }
}
