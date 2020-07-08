using Ict.Win;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B02_MailTo
    /// </summary>
    public partial class B02_MailTo : Win.Subs.Input4
    {
        /// <summary>
        /// B02_MailTo
        /// </summary>
        /// <param name="canedit">可編輯</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        public B02_MailTo(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.DoForm = new B02_MailTo_Detail();
        }

        /// <summary>
        /// OnGridSetup
        /// </summary>
        /// <returns>bool</returns>
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
