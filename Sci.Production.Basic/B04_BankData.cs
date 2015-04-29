using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;

namespace Sci.Production.Basic
{
    public partial class B04_BankData : Sci.Win.Subs.Input4
    {
        public B04_BankData(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            DoForm = new B04_BankData_Input();
            this.Text = "Bank data (" + this.KeyValue1.Trim() + ")";
        }

        protected override bool OnGridSetup()
        {
            this.grid.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("IsDefault", header: "Default", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("AccountNo", header: "Account No.", width: Widths.AnsiChars(20))
                .Text("SWIFTCode", header: "SWIFT", width: Widths.AnsiChars(11))
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(20))
                .Text("BankName", header: "Bank Name", width: Widths.AnsiChars(20))
                .Text("CountryID", header: "Country", width: Widths.AnsiChars(2))
                .Text("CountryName", header: "Country Name", width: Widths.AnsiChars(10),iseditable:false)
                .Text("City", header: "City", width: Widths.AnsiChars(20))
                .Text("MidBankName", header: "Intermediary Bank", width: Widths.AnsiChars(20))
                .Text("MidSWIFTCode", header: "Intermediary Bank-SWIFT Code", width: Widths.AnsiChars(11))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
                .Text("AddName", header: "Create by", width: Widths.AnsiChars(10), iseditable: false)
                .DateTime("AddDate", header: "Create at", iseditable: false)
                .Text("EditName", header: "Edit by", width: Widths.AnsiChars(10), iseditable: false)
                .DateTime("EditDate", header: "Edit at", iseditable: false);
            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("CountryName");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["CountryName"] = myUtility.Lookup("NameEN", gridData["CountryID"].ToString(), "Country", "ID");
            }
        }

        protected override bool DoSave()
        {
            int defaultCount = 0;

            foreach (DataRow gridData in Datas)
            {
                if (string.IsNullOrWhiteSpace(gridData["AccountNo"].ToString()))
                {
                    MessageBox.Show("The field < Account No. > can not be empty!");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(gridData["AccountName"].ToString()))
                {
                    MessageBox.Show("The field < Account Name > can not be empty!");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(gridData["BankName"].ToString()))
                {
                    MessageBox.Show("The field < Bank Name > can not be empty!");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(gridData["CountryID"].ToString()))
                {
                    MessageBox.Show("The field < Country > can not be empty!");
                    return false;
                }


                if (gridData["IsDefault"].ToString() == "True" )
                {
                    defaultCount = defaultCount + 1;
                }
            }

            if (defaultCount != 1)
            {
                MessageBox.Show("The field < Default > can only collude and select one!");
                return false;
            }

            return base.DoSave();
        }
    }
}
