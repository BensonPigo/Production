using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using Ict;

namespace Sci.Production.Basic
{
    public partial class B02_MailTo : Sci.Win.Subs.Input4
    {
        public B02_MailTo(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            DoForm = new B02_MailTo_Detail();
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "Code", width: Widths.AnsiChars(3))
                .Text("Description", header: "Description", width: Widths.AnsiChars(30))
                .Text("Subject", header: "Subject", width: Widths.AnsiChars(30))
                .EditText("ToAddress", header: "Mail to", width: Widths.AnsiChars(30))
                .EditText("CcAddress", header: "C.C.", width: Widths.AnsiChars(20))
                .EditText("Content", header: "Content", width: Widths.AnsiChars(60));

            return true;
        }
    }
}
