using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Sci.Data;
using Ict;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B07_Add
    /// </summary>
    public partial class B07_Add : Sci.Win.Subs.Base
    {
        /// <summary>
        /// B07_Add
        /// </summary>
        public B07_Add()
        {
            this.InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // 檢查Date, Hours不可為空值
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

            if (MyUtility.Check.Empty(this.numHours.Value))
            {
                MyUtility.Msg.WarningBox("< Hours > can not be empty!");
                return;
            }

            // else
            // {
            //    if (this.numericBox1.Text == "0")
            //    {
            //        MyUtility.Msg.WarningBox("< Hours > can not be empty!");
            //        return;
            //    }
            // }

            // 先將屬於登入的工廠的SewingLine資料給撈出來
            DataTable sewingLine;
            string sqlCommand = "select ID from SewingLine WITH (NOLOCK) where FactoryID = '" + Sci.Env.User.Factory + "' order by ID";
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
                if ((int)startDate.DayOfWeek != 0)
                {
                    if (this.checkIncludeSaturday.Value == "False" && (int)startDate.DayOfWeek == 6)
                    {
                        doInsert = false;
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
                                   insertCmds.Add(string.Format(
                                       @"
Insert into WorkHour (SewingLineID,FactoryID,Date,Hours,AddName,AddDate) 
Values('{0}','{1}','{2}','{3}','{4}','{5}');",
                                       currentRecord["ID"].ToString(),
                                       Env.User.Factory,
                                       startDate.ToString("d"),
                                       this.numHours.Text.ToString(),
                                       Sci.Env.User.UserID,
                                       DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                                }
                            }
                        }
                    }
                }

                startDate = startDate.AddDays(1);
            }

            // 將資料新增至Table
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
                            transactionScope.Dispose();
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
    }
}
