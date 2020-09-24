using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Transactions;
using Sci.Win.Tools;
using Sci.Data;
using Ict;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B07_BatchAdd
    /// </summary>
    public partial class B07_BatchAdd : Win.Subs.Base
    {
        private DataRow motherData;

        /// <summary>
        /// MotherData
        /// </summary>
        protected DataRow MotherData
        {
            get
            {
                return this.motherData;
            }

            set
            {
                this.motherData = value;
            }
        }

        /// <summary>
        /// B07_BatchAdd
        /// </summary>
        /// <param name="data">DataRow data</param>
        public B07_BatchAdd(DataRow data)
        {
            this.InitializeComponent();
            this.MotherData = data;
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("7", "Monday to Friday");
            comboBox1_RowSource.Add("1", "Monday");
            comboBox1_RowSource.Add("2", "Tuesday");
            comboBox1_RowSource.Add("3", "Wednesday");
            comboBox1_RowSource.Add("4", "Thursday");
            comboBox1_RowSource.Add("5", "Friday");
            comboBox1_RowSource.Add("6", "Saturday");
            comboBox1_RowSource.Add("0", "Sunday");
            this.comboWeekDay.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboWeekDay.ValueMember = "Key";
            this.comboWeekDay.DisplayMember = "Value";

            // 填預設值
            this.txtLineNoStart.Text = this.MotherData["SewingLineID"].ToString();
            this.txtLineNoEnd.Text = this.MotherData["SewingLineID"].ToString();
            this.dateDate.Text1 = Convert.ToDateTime(this.MotherData["Date"]).ToString("d");
            this.dateDate.Text2 = Convert.ToDateTime(this.MotherData["Date"]).ToString("d");
            this.comboWeekDay.SelectedValue = "7";
            this.numHours.Text = this.MotherData["Hours"].ToString();
            this.checkItsAHoliday.Checked = false;
        }

        // Line#按右鍵開窗
        private void TxtLineNoStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.UI.TextBox sewingLineText = (Win.UI.TextBox)sender;
            string sql = "Select ID,Description From SewingLine WITH (NOLOCK) Where FactoryId = '" + Env.User.Factory + "' order by ID";
            SelectItem item = new SelectItem(sql, "4,15", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            sewingLineText.Text = item.GetSelectedString();
        }

        // Line#檢查值輸入是否正確
        private void TxtLineNoStart_Validating(object sender, CancelEventArgs e)
        {
            Win.UI.TextBox sewingLineText = (Win.UI.TextBox)sender;
            if (!string.IsNullOrWhiteSpace(sewingLineText.Text) && sewingLineText.Text != sewingLineText.OldValue)
            {
                // sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", Env.User.Factory);
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@sewinglineid", sewingLineText.Text.ToString());

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);

                string selectCommand = "select ID from SewingLine WITH (NOLOCK) where FactoryID = @factoryid and ID = @sewinglineid";
                DataTable sewingData;
                DualResult result = DBProxy.Current.Select(null, selectCommand, cmds, out sewingData);
                if (!result || sewingData.Rows.Count <= 0)
                {
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", sewingLineText.Text.ToString()));
                    }

                    sewingLineText.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // 檢查Date不可為空值
            if (MyUtility.Check.Empty(this.dateDate.Value1))
            {
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.dateDate.Value2))
            {
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                return;
            }

            // 先將屬於登入的工廠的SewingLine資料給撈出來
            DataTable sewingLine;
            string sqlCommand = "select ID from SewingLine WITH (NOLOCK) where FactoryID = '" + Env.User.Factory + "' and ID >= '" + this.txtLineNoStart.Text + "' and ID <= '" + this.txtLineNoEnd.Text + "' order by ID";
            DualResult returnResult = DBProxy.Current.Select(null, sqlCommand, out sewingLine);
            if (!returnResult)
            {
                MyUtility.Msg.WarningBox("Connection fail!\r\nPlease try again.");
                return;
            }

            // 組出要新增的資料
            DateTime startDate = Convert.ToDateTime(this.dateDate.Text1);
            bool doInsert;
            IList<string> insertCmds = new List<string>();
            while (startDate <= Convert.ToDateTime(this.dateDate.Text2))
            {
                doInsert = true;
                switch ((string)this.comboWeekDay.SelectedValue)
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
                            sqlCommand = string.Format("select Date from WorkHour WITH (NOLOCK) where SewingLineID = '{0}' and FactoryID = '{1}' and Date = '{2}'", currentRecord["ID"].ToString(), Env.User.Factory, startDate.ToString("d"));
                            if (!MyUtility.Check.Seek(sqlCommand, null))
                            {
                                insertCmds.Add(string.Format(
                                    @"
Insert into WorkHour (SewingLineID,FactoryID,Date,Hours,Holiday,AddName,AddDate)
Values('{0}','{1}','{2}','{3}','{4}','{5}',GETDATE());",
                                    currentRecord["ID"].ToString(),
                                    Env.User.Factory,
                                    startDate.ToString("d"),
                                    this.numHours.Text.ToString(),
                                    this.checkItsAHoliday.Checked,
                                    Env.User.UserID));
                            }
                            else
                            {
                                insertCmds.Add(string.Format("Update WorkHour set Hours = '{0}', Holiday = '{1}', EditName = '{2}', EditDate = GETDATE() where SewingLineID = '{3}' and FactoryID = '{4}' and Date = '{5}';", this.numHours.Text.ToString(), this.checkItsAHoliday.Checked, Env.User.UserID, currentRecord["ID"].ToString(), Env.User.Factory, startDate.ToString("d")));
                            }
                        }
                    }
                }

                startDate = startDate.AddDays(1);
            }

            // 將資料新增至Table
            DualResult insertReturnResult = Ict.Result.True;
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
                            this.DialogResult = DialogResult.OK;
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
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
        }

        // It's a holiday
        private void CheckItsAHoliday_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkItsAHoliday.Checked)
            {
                this.numHours.Value = 0;
                this.numHours.ReadOnly = true;
            }
            else
            {
                this.numHours.ReadOnly = false;
            }
        }
    }
}
