using Ict.Win;
using System.Data;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P05_Detail : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;
        private string ID;

        public P05_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            maindr = mainDr;
            ID = id.Trim();
        }
        protected override Ict.DualResult OnRequery()
        {
            //encode_btn.Text = MyUtility.Convert.GetBool(maindr[""]
            return base.OnRequery();
        }
        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("group", header: "Group", width: Widths.AnsiChars(8))
                .Text("SEQ", header: "SEQ#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(10))
                .Text("Changescale", header: "Color Change Scale", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("StainingScale", header: "Color Staining Scale", width: Widths.AnsiChars(10))
                .Text("Result", header: "Result", width: Widths.AnsiChars(16))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(50), iseditingreadonly: true);
            return base.OnGridSetup();
        }
    }
}
