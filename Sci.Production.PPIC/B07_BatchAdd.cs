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
            string sql = "Select ID,Description From SewingLine WITH (NOLOCK) Where FactoryId = '" + Sci.Env.User.Factory + "' order by ID";
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
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", Sci.Env.User.Factory);
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@sewinglineid", sewingLineText.Text.ToString());

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);

                string selectCommand = "select ID from SewingLine WITH (NOLOCK) where FactoryID = @factoryid and ID = @sewinglineid";
                DataTable SewingData;
                DualResult result = DBProxy.Current.Select(null, selectCommand, cmds, out SewingData);
                if (!result || SewingData.Rows.Count <= 0)
                {
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", sewingLineText.Text.ToString()));
                    }
                    sewingLineText.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //檢查Date不可為空值
            if (MyUtility.Check.Empty(this.dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.dateRange1.Value2))
            {
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                return;
            }

            //先將屬於登入的工廠的SewingLine資料給撈出來
            DataTable sewingLine;
            string sqlCommand = "select ID from SewingLine WITH (NOLOCK) where FactoryID = '" + Sci.Env.User.Factory + "' and ID >= '" + this.textBox1.Text + "' and ID <= '" + this.textBox2.Text + "' order by ID";
            DualResult returnResult = DBProxy.Current.Select(null, sqlCommand, out sewingLine);
            if (!returnResult)
            {
                MyUtility.Msg.WarningBox("Connection fail!\r\nPlease try again.");
                return;
            }

            //組出要新增的資料
            DateTime startDate = Convert.ToDateTime(this.dateRange1.Text1);
            bool doInsert;
            IList<string> insertCmds = new List<string>();
            while (startDate <= Convert.ToDateTime(this.dateRange1.Text2))
            {
                doInsert = true;
                switch ((string)this.comboBox1.SelectedValue)
                {
                    case "0":
                        if ((int)startDate.DayOfWeek != 0)
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
                            sqlCommand = string.Format("select Date from WorkHour WITH (NOLOCK) where SewingLineID = '{0}' and FactoryID = '{1}' and Date = '{2}'", currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d"));
                            if (!MyUtility.Check.Seek(sqlCommand, null))
                            {
                                insertCmds.Add(string.Format(@"Insert into WorkHour (SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
Values('{0}','{1}','{2}','{3}','{4}','{5}',GETDATE());", currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d"), this.numericBox1.Text.ToString(), this.checkBox1.Checked, Sci.Env.User.UserID));
                            }
                            else
                            {
                                insertCmds.Add(string.Format("Update WorkHour set Hours = '{0}', Holiday = '{1}', EditName = '{2}', EditDate = GETDATE() where SewingLineID = '{3}' and FactoryID = '{4}' and Date = '{5}';", this.numericBox1.Text.ToString(), this.checkBox1.Checked, Sci.Env.User.UserID, currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d")));
                            }
                        }
                    }
                }
                startDate = startDate.AddDays(1);
            }

            //將資料新增至Table
            DualResult insertReturnResult = Result.True;
            if (insertCmds.Count > 0)
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        insertReturnResult = DBProxy.Current.Executes(null, insertCmds);
                        if (insertReturnResult)
                        {
                            transactionScope.Complete();
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
        }

        //It's a holiday
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                numericBox1.Value = 0;
                numericBox1.ReadOnly = true;
            }
            else
            {
                numericBox1.ReadOnly = false;
            }
        }
    }
}
