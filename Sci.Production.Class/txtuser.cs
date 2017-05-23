﻿using System;
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
    public partial class txtuser : Sci.Win.UI._UserControl
    {
        public txtuser()
        {            
            InitializeComponent();
            
        }
        private string myUsername = null;

        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this.textBox1; }
        }

        public Sci.Win.UI.DisplayBox DisplayBox1
        {
            get { return this.displayBox1; }
        }

        [Bindable(true)]
        public string TextBox1Binding
        {
            set 
            {               
                this.textBox1.Text = value;
                if (!Env.DesignTime)
                {
                    if (this.textBox1.Text == "" || MyUtility.Check.Empty(this.textBox1.Text))
                    {
                        return;
                    }
                    
                    Sci.Production.Class.Commons.UserPrg.GetName(this.TextBox1.Text, out myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                    this.DisplayBox1.Text = myUsername;
           
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
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "Pass1", "ID"))
                {
                    string alltrimData = textValue.Trim();
                    //bool isUserName = MyUtility.Check.Seek(alltrimData, "Pass1", "Name");
                    bool isUserExtNo = MyUtility.Check.Seek(alltrimData, "Pass1", "ExtNo");
                    DataTable dtName;
                    string selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where Name like '%{0}%' order by ID", textValue.Trim());
                    var resultName= DBProxy.Current.Select(null, selectCommand, out dtName);

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
                            selectCommand = string.Format("select ID, Name, ExtNo, REPLACE(Factory,' ','') Factory from Pass1 WITH (NOLOCK) where ExtNo = '{0}' order by ID", textValue.Trim());
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
                        MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textValue));
                        this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                        return;
                    }
                }
            }
            
            // 強制把binding的Text寫到DataRow
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());

            string selectSql = string.Format("Select Name,ExtNo from Pass1 WITH (NOLOCK) where id = '{0}'", this.textBox1.Text.ToString());
            DataTable data;
            var result = DBProxy.Current.Select(null, selectSql, out data);
            string name = "";
            string extNo = "";
            if (result && data.Rows.Count > 0)
            {
                name = data.Rows[0]["name"].ToString();
                extNo = data.Rows[0]["extNo"].ToString();
            }

            if (!string.IsNullOrWhiteSpace(extNo) || !string.IsNullOrWhiteSpace(name))
            {
                this.displayBox1.Text = name + " #" + extNo;
            }
            else
            {
                this.displayBox1.Text = "";
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                this.textBox1.Text = "";
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {            
            
               if (this.textBox1.ReadOnly || this.DataBindings.Count == 0)
               {
                   string selectSql = string.Format("Select Name,ExtNo from Pass1 WITH (NOLOCK) where id = '{0}'", this.textBox1.Text.ToString());
                   DataTable data;
                   var result = DBProxy.Current.Select(null, selectSql, out data);
                   string name = "";
                   string extNo = "";
                   if (result && data.Rows.Count>0) {
                       name = data.Rows[0]["name"].ToString();
                       extNo = data.Rows[0]["extNo"].ToString();
                   }
                   if (!string.IsNullOrWhiteSpace(extNo) || !string.IsNullOrWhiteSpace(name))
                   {
                       this.displayBox1.Text = name + " #" + extNo;
                   }
                   else
                   {
                       this.displayBox1.Text = "";
                   }
                   if (string.IsNullOrWhiteSpace(name))
                   {
                       this.textBox1.Text = "";
                   }
               }
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || textBox1.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID, Name, ExtNo, replace(Factory,' ','')factory from Pass1 WITH (NOLOCK) where Resign is null order by ID", "10,22,5,40", this.textBox1.Text);
            item.Size = new System.Drawing.Size(828, 509);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.textBox1.Text = item.GetSelectedString();
            this.displayBox1.Text = item.GetSelecteds()[0]["Name"].ToString().TrimEnd()+" #"+ item.GetSelecteds()[0]["EXTNO"].ToString().TrimEnd();
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string sql;
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Pass1 WITH (NOLOCK) 
                    where id = @id";
            sqlpar.Add(new SqlParameter("@id", textBox1.Text.ToString()));

            userData ud = new userData(sql, sqlpar);

            if (ud.errMsg == null)
                ud.ShowDialog();
            else
                MyUtility.Msg.ErrorBox(ud.errMsg);
        }
    }
}
