using System;

namespace Sci.Production.Logistic
{
    public partial class P02_InputDate : Sci.Win.Subs.Base
    {
        private DateTime returnDate;

        public P02_InputDate(string captionName, string lableName)
        {
            this.InitializeComponent();
            this.Text = captionName;
            this.labelReceiveDate.Text = lableName;
        }

        public DateTime ReturnDate
        {
            get
            {
                return this.returnDate;
            }

            set
            {
                this.returnDate = value;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateReceiveDate.Text))
            {
                this.dateReceiveDate.Focus();
                MyUtility.Msg.WarningBox("Date can't be empty!");
                return;
            }

            this.ReturnDate = Convert.ToDateTime(this.dateReceiveDate.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
