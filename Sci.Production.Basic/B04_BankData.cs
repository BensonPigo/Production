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
        }

        protected override bool OnGridSetup()
        {
            this.grid.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("IsDefault", header: "Default", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("AccountNo", header: "Account No.", width: Widths.AnsiChars(30))
                .Text("SWIFTCode", header: "SWIFT", width: Widths.AnsiChars(11))
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(30))
                .Text("BankName", header: "Bank Name", width: Widths.AnsiChars(30))
                .Text("CountryID", header: "Country", width: Widths.AnsiChars(2))
                .Text("CountryName", header: "Country Name", width: Widths.AnsiChars(10),iseditable:false)
                .Text("City", header: "City", width: Widths.AnsiChars(20))
                .Text("MidBankName", header: "Intermediary Bank", width: Widths.AnsiChars(30))
                .Text("MidSWIFTCode", header: "Intermediary Bank-SWIFT Code", width: Widths.AnsiChars(11))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
                .Text("AddName", header: "Create by", width: Widths.AnsiChars(10))
                .DateTime("AddDate", header: "Create at")
                .Text("EditName", header: "Edit by", width: Widths.AnsiChars(10))
                .DateTime("EditDate", header: "Edit at");
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
    }
}
