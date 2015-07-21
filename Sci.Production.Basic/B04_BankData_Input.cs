using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B04_BankData_Input : Sci.Win.Subs.Input6A
    {
        public B04_BankData_Input()
        {
            InitializeComponent();
        }

        protected override bool DoSave()
        {
            if (String.IsNullOrWhiteSpace(this.textBox1.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Account No. > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.textBox3.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Account Name > can not be empty!");
                this.textBox3.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.textBox4.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Bank Name > can not be empty!");
                this.textBox4.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.txtcountry1.TextBox1.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Country > can not be empty!");
                this.txtcountry1.TextBox1.Focus();
                return false;
            }

            return base.DoSave();
        }

        protected override bool OnAcceptChanging(DataRow data)
        {
            data["CountryName"] = MyUtility.GetValue.Lookup("Alias", data["CountryID"].ToString(), "Country", "ID");
            data["CreateBy"] = data["AddName"].ToString().PadRight(10) + ((DateTime)data["AddDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            if (data["EditDate"] != System.DBNull.Value)
            {
                data["EditBy"] = data["EditName"].ToString().PadRight(10) + ((DateTime)data["EditDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            return base.OnAcceptChanging(data);
        }
    }
}
