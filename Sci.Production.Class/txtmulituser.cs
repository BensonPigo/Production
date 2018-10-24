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
using Sci.Production.Class;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class txtmulituser : Sci.Win.UI._UserControl
    {
        public txtmulituser()
        {            
            InitializeComponent();
            
        }
        private string myUsername = null;
        private string mulitUsername = null;

        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        public string textbox1_text
        {
            set {
                textBox1.Text = value;

                mulitUsername = string.Empty;
                string[] getUserId = this.TextBox1.Text.Split(',').Distinct().ToArray();
                foreach (var UserID in getUserId)
                {
                    Sci.Production.Class.Commons.UserPrg.GetName(UserID, out myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                    mulitUsername += "," + myUsername;
                }

                this.DisplayBox1.Text = (mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = mulitUsername.Substring(1) : this.DisplayBox1.Text = mulitUsername.Substring(1);
            }
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            set 
            {               
                this.textBox1.Text = value;
                if (!Env.DesignTime)
                {
                    mulitUsername = string.Empty;
                    string[] getUserId = this.TextBox1.Text.Split(',').Distinct().ToArray();
                    foreach (var UserID in getUserId)
                    {
                        Sci.Production.Class.Commons.UserPrg.GetName(UserID, out myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                        mulitUsername += "," + myUsername;
                    }

                    this.DisplayBox1.Text = (mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = mulitUsername.Substring(1) : this.DisplayBox1.Text = mulitUsername.Substring(1);
                }
            }
            get { return textBox1.Text; }
        }

        [Bindable(true)]
        public string DisplayBox1Binding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            // base.OnValidating(e);
            string textValue = this.textBox1.Text;
            mulitUsername = string.Empty;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                string[] getUserId = textValue.Split(',').Distinct().ToArray();
                foreach (var UserID in getUserId)
                {
                    if (!MyUtility.Check.Seek(UserID, "Pass1", "ID"))
                    {
                        string alltrimData = UserID.Trim();
                        bool isUserExtNo = MyUtility.Check.Seek(alltrimData, "Pass1", "ExtNo");
                        DataTable dtName;
                        string selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where Name like '%{0}%' order by ID", UserID.Trim());
                        var resultName = DBProxy.Current.Select(null, selectCommand, out dtName);

                        //if (isUserName | isUserExtNo)
                        if ((resultName && dtName.Rows.Count > 0) | isUserExtNo)
                        {
                            DataTable selectTable;
                            if (dtName.Rows.Count > 0)
                            {
                                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dtName, "ID,Name,ExtNo,Factory", "10,22,5,40", this.textBox1.Text);
                                item.Size = new System.Drawing.Size(828, 509);
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel)
                                {
                                    this.textBox1.Text = "";
                                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                                    return;
                                }
                                this.textBox1.Text = item.GetSelectedString();
                            }
                            else
                            {
                                selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where ExtNo = '{0}' order by ID", UserID.Trim());
                                DBProxy.Current.Select(null, selectCommand, out selectTable);
                                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(selectTable, "ID,Name,ExtNo,Factory", "10,22,5,40", this.textBox1.Text);
                                item.Size = new System.Drawing.Size(828, 509);
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel)
                                {
                                    this.textBox1.Text = "";
                                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                                    return;
                                }
                                this.textBox1.Text = item.GetSelectedString();
                            }
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", UserID));
                            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                            this.DisplayBox1.Text = "";
                            return;
                        }
                    }
                    Sci.Production.Class.Commons.UserPrg.GetName(UserID, out myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                    mulitUsername += "," + myUsername;
                }
               
            }
            
            // 強制把binding的Text寫到DataRow
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            this.DisplayBox1.Text = (mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = mulitUsername.Substring(1) : this.DisplayBox1.Text = mulitUsername.Substring(1);
        }
        
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2("select ID, Name, ExtNo, replace(Factory,' ','')factory from Pass1 WITH (NOLOCK) where Resign is null order by ID", "", this.textBox1.Text);
            item.Size = new System.Drawing.Size(828, 509);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();

            mulitUsername = string.Empty;
            string[] getUserId = item.GetSelectedString().Split(',').Distinct().ToArray();
            foreach (var UserID in getUserId)
            {
                Sci.Production.Class.Commons.UserPrg.GetName(UserID, out myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                mulitUsername += "," + myUsername;
            }

            this.DisplayBox1.Text = (mulitUsername.Substring(0, 1) == ",") ? this.DisplayBox1.Text = mulitUsername.Substring(1) : this.DisplayBox1.Text = mulitUsername.Substring(1);
        }
    }
}
