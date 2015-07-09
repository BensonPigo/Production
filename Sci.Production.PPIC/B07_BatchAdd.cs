using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Transactions;
using Sci.Win.Tools;
using Sci.Data;
using Ict;

namespace Sci.Production.PPIC
{
    public partial class B07_BatchAdd : Sci.Win.Subs.Base
    {
        protected DataRow motherData;
        public B07_BatchAdd(DataRow data)
        {
            InitializeComponent();
            this.motherData = data;
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("7", "Monday to Friday");
            comboBox1_RowSource.Add("1", "Monday");
            comboBox1_RowSource.Add("2", "Tuesday");
            comboBox1_RowSource.Add("3", "Wednesday");
            comboBox1_RowSource.Add("4", "Thursday");
            comboBox1_RowSource.Add("5", "Friday");
            comboBox1_RowSource.Add("6", "Saturday");
            comboBox1_RowSource.Add("0", "Sunday");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

            //填預設值
            this.textBox1.Text = this.motherData["SewingLineID"].ToString();
            this.textBox2.Text = this.motherData["SewingLineID"].ToString();
            this.dateRange1.Text1 = Convert.ToDateTime(this.motherData["Date"]).ToString("d");
            this.dateRange1.Text2 = Convert.ToDateTime(this.motherData["Date"]).ToString("d");
            this.comboBox1.SelectedValue = "7";
            this.numericBox1.Text = this.motherData["Hours"].ToString();
            this.checkBox1.Checked = false;
        }

        //Line#按右鍵開窗
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.UI.TextBox sewingLineText = (Sci.Win.UI.TextBox)sender;
            string sql = "Select ID,Description From SewingLine Where FactoryId = '" + Sci.Env.User.Factory + "' order by ID";
            SelectItem item = new SelectItem(sql, "4,15", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            sewingLineText.Text = item.GetSelectedString();
        }

        //Line#檢查值輸入是否正確
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            Sci.Win.UI.TextBox sewingLineText = (Sci.Win.UI.TextBox)sender;
            if (!string.IsNullOrWhiteSpace(sewingLineText.Text) && sewingLineText.Text != sewingLineText.OldValue)
            {
                string selectCommand = string.Format("select ID from SewingLine where FactoryID = '{0}' and ID = '{1}'", Sci.Env.User.Factory, sewingLineText.Text.ToString());
                if (!MyUtility.Check.Seek(selectCommand, null))
                {
                    MessageBox.Show(string.Format("< Sewing Line: {0} > not found!!!", sewingLineText.Text.ToString()));
                    sewingLineText.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //檢查Date不可為空值
            if (string.IsNullOrWhiteSpace(this.dateRange1.Text1))
            {
                MessageBox.Show("< Date > can not be empty!");
                return;
            }

            if (string.IsNullOrWhiteSpace(this.dateRange1.Text2))
            {
                MessageBox.Show("< Date > can not be empty!");
                return;
            }
            
            //先將屬於登入的工廠的SewingLine資料給撈出來
            DataTable sewingLine;
            string sqlCommand = "select ID from SewingLine where FactoryID = '" + Sci.Env.User.Factory + "' and ID >= '" + this.textBox1.Text + "' and ID <= '" + this.textBox2.Text + "' order by ID";
            DualResult returnResult = DBProxy.Current.Select(null, sqlCommand, out sewingLine);
            if (!returnResult)
            {
                MessageBox.Show("Connection fail!\r\nPlease try again.");
                return;
            }

            //組出要新增的資料
            DateTime startDate = Convert.ToDateTime(this.dateRange1.Text1);
            bool doInsert;
            string sqlInsert = "";
            while (startDate <= Convert.ToDateTime(this.dateRange1.Text2))
            {
                doInsert = true;
                switch ((string)this.comboBox1.SelectedValue)
                {
                    case "0":
                        if((int)startDate.DayOfWeek !=0 )
                        {
                            doInsert = false;
                        }
                        break;
                    case "1":
                        if ((int)startDate.DayOfWeek != 1)
                        {
                            doInsert = false;
                        }
                        break;
                    case "2":
                        if ((int)startDate.DayOfWeek != 2)
                        {
                            doInsert = false;
                        }
                        break;
                    case "3":
                        if ((int)startDate.DayOfWeek != 3)
                        {
                            doInsert = false;
                        }
                        break;
                    case "4":
                        if ((int)startDate.DayOfWeek != 4)
                        {
                            doInsert = false;
                        }
                        break;
                    case "5":
                        if ((int)startDate.DayOfWeek != 5)
                        {
                            doInsert = false;
                        }
                        break;
                    case "6":
                        if ((int)startDate.DayOfWeek != 6)
                        {
                            doInsert = false;
                        }
                        break;
                    case "7":
                        if ((int)startDate.DayOfWeek == 0 || (int)startDate.DayOfWeek == 6)
                        {
                            doInsert = false;
                        }
                        break;
                    default:
                        doInsert = false;
                        break;
                }

                if (doInsert)
                {
                    if (sewingLine.Rows.Count > 0)
                    {
                        foreach (DataRow currentRecord in sewingLine.Rows)
                        {
                            sqlCommand = string.Format("select Date from WorkHour where SewingLineID = '{0}' and FactoryID = '{1}' and Date = '{2}'", currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d"));
                            if (!MyUtility.Check.Seek(sqlCommand, null))
                            {
                                sqlInsert = sqlInsert + "Insert into WorkHour (SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)\r\n ";
                                sqlInsert = sqlInsert + string.Format("Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');\r\n", currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d"), this.numericBox1.Text.ToString(), this.checkBox1.Checked, Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            }
                            else
                            {
                                sqlInsert = sqlInsert + string.Format("Update WorkHour set Hours = '{0}', Holiday = '{1}', EditName = '{2}', EditDate = '{3}' where SewingLineID = '{4}' and FactoryID = '{5}' and Date = '{6}';\r\n", this.numericBox1.Text.ToString(),this.checkBox1.Checked,Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d"));
                            }
                        }
                    }
                }
                startDate = startDate.AddDays(1);
            }

            //將資料新增至Table
            DualResult insertReturnResult = Result.True;
            if (!string.IsNullOrWhiteSpace(sqlInsert))
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        insertReturnResult = DBProxy.Current.Execute(null, sqlInsert);
                        if (insertReturnResult)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            MessageBox.Show("Create failed, Pleaes re-try");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
            if (insertReturnResult)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
