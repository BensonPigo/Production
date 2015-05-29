using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Transactions;
using Sci.Data;
using Ict;

namespace Sci.Production.PPIC
{
    public partial class B07_Add : Sci.Win.Subs.Base
    {
        public B07_Add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //檢查Date, Hours不可為空值
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

            if (string.IsNullOrWhiteSpace(this.numericBox1.Text))
            {
                MessageBox.Show("< Hours > can not be empty!");
                return;
            }
            else
            {
                if (this.numericBox1.Text == "0")
                {
                    MessageBox.Show("< Hours > can not be empty!");
                    return;
                }
            }

            //先將屬於登入的工廠的SewingLine資料給撈出來
            DataTable sewingLine;
            string sqlCommand = "select ID from SewingLine where FactoryID = '" + Sci.Env.User.Factory + "' order by ID";
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
                if ((int)startDate.DayOfWeek != 0)
                {
                    if (this.checkBox1.Value == "False" && (int)startDate.DayOfWeek == 6)
                    {
                        doInsert = false;
                    }

                    if (doInsert)
                    {
                        if(sewingLine.Rows.Count > 0)
                        {
                            foreach (DataRow currentRecord in sewingLine.Rows)
                            {
                              sqlCommand = string.Format("select Date from WorkHour where SewingLineID = '{0}' and FactoryID = '{1}' and Date = '{2}'", currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d"));
                              if (!myUtility.Seek(sqlCommand, null))
                               {
                                    sqlInsert = sqlInsert + "Insert into WorkHour (SewingLineID,FactoryID,Date,Hours,AddName,AddDate)\r\n ";
                                    sqlInsert = sqlInsert + string.Format("Values('{0}','{1}','{2}','{3}','{4}','{5}');\r\n", currentRecord["ID"].ToString(), Sci.Env.User.Factory, startDate.ToString("d"),this.numericBox1.Text.ToString(),Sci.Env.User.UserID,DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                }
                            }
                        }
                    }
                }
                startDate = startDate.AddDays(1);
            }

            //將資料新增至Table
            DualResult insertReturnResult = Result.True;
            if(!string.IsNullOrWhiteSpace(sqlInsert))
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
