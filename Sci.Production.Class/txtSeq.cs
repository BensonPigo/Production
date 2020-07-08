using System;
using System.ComponentModel;

namespace Sci.Production.Class
{
    public partial class txtSeq : Sci.Win.UI._UserControl
    {
        public txtSeq()
        {
            this.InitializeComponent();
        }

        [Bindable(true)]
        public string seq1
        {
            get { return this.textSeq1.Text.ToString().Trim(); }
            set { this.textSeq1.Text = value.Trim(); }
        }

        [Bindable(true)]
        public string seq2
        {
            get { return this.textSeq2.Text.ToString().Trim(); }
            set { this.textSeq2.Text = value.Trim(); }
        }

        public string getSeq()
        {
            return this.seq1 + " " + this.seq2;
        }

        public bool checkSeq1Empty()
        {
            return MyUtility.Check.Empty(this.seq1);
        }

        public bool checkSeq2Empty()
        {
            return MyUtility.Check.Empty(this.seq2);
        }

        public bool checkEmpty(bool showErrMsg = true)
        {
            if ((MyUtility.Check.Empty(this.seq1) & MyUtility.Check.Empty(this.seq2)) & showErrMsg)
            {
                MyUtility.Msg.WarningBox("Seq' mask need enter 00-00");
            }

            return MyUtility.Check.Empty(this.seq1) & MyUtility.Check.Empty(this.seq2);
        }

        public void txtSeq_ReadOnly(bool ReadOnly)
        {
            this.textSeq1.ReadOnly = ReadOnly;
            this.textSeq2.ReadOnly = ReadOnly;
        }

        private void txtSeq_Leave(object sender, EventArgs e)
        {
            // Seq1 is Empty & Seq2 isn't Empty
            if (MyUtility.Check.Empty(this.seq1) & !MyUtility.Check.Empty(this.seq2))
            {
                MyUtility.Msg.WarningBox("When Seq2 isn't Empty, Seq1 can't be Empty", "Seq");
                this.textSeq1.Focus();
                return;
            }

            // mantis# : 4260
            // Seq1 isn't Empty & Seq2 is Empty
            // if (!MyUtility.Check.Empty(seq1) & MyUtility.Check.Empty(seq2))
            // {
            //    MyUtility.Msg.WarningBox("When Seq1 isn't Empty, Seq2 can't be Empty", "Seq");
            //    textSeq2.Focus();
            //    return;
            // }
        }
    }
}
