using Ict.Win;

namespace Sci.Production.Cutting
{
    public partial class P08_MailTo : Sci.Win.Subs.Input4
    {
        public P08_MailTo(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.DoForm = new P08_MailTo_Detail();
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("ID", header: "Code", width: Widths.AnsiChars(3))
            .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditable: false)
            .Text("Subject", header: "Subject", width: Widths.AnsiChars(30))
            .EditText("ToAddress", header: "Mail to", width: Widths.AnsiChars(30))
            .EditText("CcAddress", header: "C.C.", width: Widths.AnsiChars(20))
            .EditText("Content", header: "Content", width: Widths.AnsiChars(60));

            return true;
        }
    }
}
