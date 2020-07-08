using System;
using System.Data;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B04_BankData_Input
    /// </summary>
    public partial class B04_BankData_Input : Sci.Win.Subs.Input6A
    {
        /// <summary>
        /// B04_BankData_Input
        /// </summary>
        public B04_BankData_Input()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool DoSave()
        {
            if (MyUtility.Check.Empty(this.txtAccountNo.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Account No. > can not be empty!");
                this.txtAccountNo.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.txtAccountName.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Account Name > can not be empty!");
                this.txtAccountName.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.txtBankName.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Bank Name > can not be empty!");
                this.txtBankName.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.txtCountry.TextBox1.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Country > can not be empty!");
                this.txtCountry.TextBox1.Focus();
                return false;
            }

            return base.DoSave();
        }

        /// <inheritdoc/>
        protected override bool OnAcceptChanging(DataRow data)
        {
            data["CountryName"] = MyUtility.GetValue.Lookup("Alias", data["CountryID"].ToString(), "Country", "ID");
            data["CreateBy"] = data["AddName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", data["AddName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)data["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            if (data["EditDate"] != System.DBNull.Value)
            {
                data["EditBy"] = data["EditName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", data["EditName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)data["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }

            return base.OnAcceptChanging(data);
        }
    }
}
