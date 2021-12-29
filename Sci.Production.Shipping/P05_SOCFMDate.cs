﻿using Ict;
using Sci.Data;
using System;
using System.ComponentModel;
using System.Data;
using System.Transactions;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P05_SOCFMDate : Win.Subs.Base
    {
        private DataRow dr;
        public DateTime? SOCFMDate;
        public string newSOCFMDateCmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="P05_SOCFMDate"/> class.
        /// </summary>
        /// <param name="data">DataRow</param>
        public P05_SOCFMDate(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateSOCfmDate.Value))
            {
                MyUtility.Msg.WarningBox("S/O CFM Date cannot be empty!");
                return;
            }

            // S/O Cnfm不可早於FCR Date
            if (!MyUtility.Check.Empty(this.dr["FCRDate"]) && DateTime.Compare(this.dateSOCfmDate.Value.Value, (DateTime)this.dr["FCRDate"]) < 0)
            {
                MyUtility.Msg.WarningBox("<S/O Cnfm> cannot be earlier than <FCR Date> !");
                return;
            }

            if (!MyUtility.Check.Empty(this.dr["ID"]))
            {
                #region Confirm
                bool firstCFM = !MyUtility.Check.Seek(string.Format("select ID from GMTBooking_History WITH (NOLOCK) where ID = '{0}' and HisType = '{1}'", MyUtility.Convert.GetString(this.dr["ID"]), "SOCFMDate"));
                string insertCmd = string.Format(
                    @"insert into GMTBooking_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}',GETDATE())",
                    MyUtility.Convert.GetString(this.dr["ID"]),
                    "SOCFMDate",
                    firstCFM ? string.Empty : "CFM",
                    "Un CFM",
                    Env.User.UserID);

                string updateCmd =
                    $@"
update GMTBooking set SOCFMDate = '{((DateTime)this.dateSOCfmDate.Value).ToString("yyyy/MM/dd")}' 
where ID = '{MyUtility.Convert.GetString(this.dr["ID"])}';
update PackingList set GMTBookingLock = 'Y' 
where INVNo = '{MyUtility.Convert.GetString(this.dr["ID"])}'";

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, insertCmd);
                        DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                        if (result && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
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
                #endregion
            }
            else
            {
                // Click New時還沒有GNTBooking ID，因此要把資料記錄下來，外面Save的時候一併存入
                this.SOCFMDate = this.dateSOCfmDate.Value;
                this.newSOCFMDateCmd = $@"
insert into GMTBooking_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('@@GMTBookinID@@', 'SOCFMDate', '', 'CFM', '{Env.User.UserID}', GETDATE())
;
update PackingList set GMTBookingLock = 'Y' 
where INVNo = '@@GMTBookinID@@'
";
            }

            this.Close();
        }

        private void DateSOCfmDate_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateSOCfmDate.Value) ||
                MyUtility.Check.Empty(this.dr["FCRDate"]))
            {
                return;
            }

            if (DateTime.Compare((DateTime)this.dr["FCRDate"], (DateTime)this.dateSOCfmDate.Value) < 0)
            {
                MyUtility.Msg.WarningBox("[S/O Cfm Date] can not later than FCR Date");
                e.Cancel = true;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
