using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Basic
{
    public partial class B05_SetHoliday : Sci.Win.Subs.Base
    {
        private DateTime reviseDate;
        private int newRecord;

        public B05_SetHoliday(DateTime date)
        {
            InitializeComponent();
            this.textBox1.Text = date.ToString("d");
            reviseDate = date;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlcmd = string.Format("select Name from Holiday WITH (NOLOCK) where HolidayDate='{0}' and FactoryID = '{1}'", this.textBox1.Text, Sci.Env.User.Factory);
            string holidayName = MyUtility.GetValue.Lookup(sqlcmd);

            if (MyUtility.Check.Empty(holidayName))
            {
                this.Text = this.Text + " - New";
                newRecord = 1;
            }
            else
            {
                this.textBox2.Text = holidayName;
                this.Text = this.Text + " - Revise";
                newRecord = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(textBox2.Text))
            {
                if (newRecord == 0)
                {
                    string deleteCmd = string.Format("delete Holiday where HolidayDate = '{0}' and FactoryID = '{1}'", reviseDate.ToString("d"), Sci.Env.User.Factory);
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
                if (newRecord == 0)
                {
                    string updateCmd = string.Format("update Holiday set Name = '{0}' where HolidayDate = '{1}' and FactoryID = '{2}'", textBox2.Text, reviseDate.ToString("d"), Sci.Env.User.Factory);
                    DualResult result = DBProxy.Current.Execute(null, updateCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
                else
                {
                    string insertCmd = string.Format(@"insert into Holiday(FactoryID,HolidayDate,Name,AddName,AddDate)
values ('{0}','{1}','{2}','{3}',GETDATE());", Sci.Env.User.Factory, reviseDate.ToString("d"), textBox2.Text, Sci.Env.User.UserID);
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
