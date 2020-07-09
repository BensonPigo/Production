using System;
using System.ComponentModel;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtSeq
    /// </summary>
    public partial class TxtSeq : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtSeq"/> class.
        /// </summary>
        public TxtSeq()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string Seq1
        {
            get { return this.textSeq1.Text.ToString().Trim(); }
            set { this.textSeq1.Text = value.Trim(); }
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string Seq2
        {
            get { return this.textSeq2.Text.ToString().Trim(); }
            set { this.textSeq2.Text = value.Trim(); }
        }

        /// <summary>
        /// Seq1 + Seq2
        /// </summary>
        /// <returns>string</returns>
        public string GetSeq()
        {
            return this.Seq1 + " " + this.Seq2;
        }

        /// <summary>
        /// Check Seq1 Empty
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckSeq1Empty()
        {
            return MyUtility.Check.Empty(this.Seq1);
        }

        /// <summary>
        /// Check Seq2 Empty
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckSeq2Empty()
        {
            return MyUtility.Check.Empty(this.Seq2);
        }

        /// <summary>
        /// Check Empty
        /// </summary>
        /// <param name="showErrMsg">Error Msg</param>
        /// <returns>bool</returns>
        public bool CheckEmpty(bool showErrMsg = true)
        {
            if ((MyUtility.Check.Empty(this.Seq1) & MyUtility.Check.Empty(this.Seq2)) & showErrMsg)
            {
                MyUtility.Msg.WarningBox("Seq' mask need enter 00-00");
            }

            return MyUtility.Check.Empty(this.Seq1) & MyUtility.Check.Empty(this.Seq2);
        }

        /// <summary>
        /// TxtSeq set ReadOnly
        /// </summary>
        /// <param name="readOnly">readOnly</param>
        public void TxtSeq_ReadOnly(bool readOnly)
        {
            this.textSeq1.ReadOnly = readOnly;
            this.textSeq2.ReadOnly = readOnly;
        }

        private void TxtSeq_Leave(object sender, EventArgs e)
        {
            // Seq1 is Empty & Seq2 isn't Empty
            if (MyUtility.Check.Empty(this.Seq1) & !MyUtility.Check.Empty(this.Seq2))
            {
                MyUtility.Msg.WarningBox("When Seq2 isn't Empty, Seq1 can't be Empty", "Seq");
                this.textSeq1.Focus();
                return;
            }
        }
    }
}
