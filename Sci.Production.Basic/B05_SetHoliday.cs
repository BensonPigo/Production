using System;
using Ict;
using Sci.Data;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B05_SetHoliday
    /// </summary>
    public partial class B05_SetHoliday : Win.Subs.Base
    {
        private DateTime reviseDate;
        private int newRecord;

        /// <summary>
        /// B05_SetHoliday
        /// </summary>
        /// <param name="date">date</param>
        public B05_SetHoliday(DateTime date)
        {
            this.InitializeComponent();
            this.txtDate.Text = date.ToString("d");
            this.reviseDate = date;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlcmd = string.Format("select Name from Holiday WITH (NOLOCK) where HolidayDate='{0}' and FactoryID = '{1}'", this.txtDate.Text, Sci.Env.User.Factory);
            string holidayName = MyUtility.GetValue.Lookup(sqlcmd);

            if (MyUtility.Check.Empty(holidayName))
            {
                this.Text = this.Text + " - New";
                this.newRecord = 1;
            }
            else
            {
                this.txtDescription.Text = holidayName;
                this.Text = this.Text + " - Revise";
                this.newRecord = 0;
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtDescription.Text))
            {
                if (this.newRecord == 0)
                {
                    string deleteCmd = string.Format("delete Holiday where HolidayDate = '{0}' and FactoryID = '{1}'", this.reviseDate.ToString("d"), Sci.Env.User.Factory);
                    DualResult result = DBProxy.Current.Execute(null, deleteCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Delete data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
            else
            {
                if (this.newRecord == 0)
                {
                    string updateCmd = string.Format("update Holiday set Name = '{0}' where HolidayDate = '{1}' and FactoryID = '{2}'", this.txtDescription.Text, this.reviseDate.ToString("d"), Sci.Env.User.Factory);
                    DualResult result = DBProxy.Current.Execute(null, updateCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
                else
                {
                    string insertCmd = string.Format(
                        @"insert into Holiday(FactoryID,HolidayDate,Name,AddName,AddDate)
values ('{0}','{1}','{2}','{3}',GETDATE());",
                        Env.User.Factory,
                        this.reviseDate.ToString("d"),
                        this.txtDescription.Text,
                        Env.User.UserID);
                    DualResult result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Insert data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }
    }
}
