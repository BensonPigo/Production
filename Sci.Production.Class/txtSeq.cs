using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtSeq : Sci.Win.UI._UserControl
    {
        public txtSeq()
        {
            InitializeComponent();
        }

        [Bindable(true)]
        public string seq1
        {
            set { this.textSeq1.Text = value.Trim(); }
            get { return textSeq1.Text.ToString().Trim(); }
        }

        [Bindable(true)]
        public string seq2
        {
            set { this.textSeq2.Text = value.Trim(); }
            get { return textSeq2.Text.ToString().Trim(); }
        }

        public string getSeq()
        {
            return seq1 + " " + seq2;
        }

        public bool checkEmpty(bool showErrMsg = true)
        {
            if ((MyUtility.Check.Empty(seq1) | MyUtility.Check.Empty(seq2)) & showErrMsg)
                MyUtility.Msg.WarningBox("Seq' mask need enter 00-00");

            return MyUtility.Check.Empty(seq1) | MyUtility.Check.Empty(seq2);
        }


        private void txtSeq_Leave(object sender, EventArgs e)
        {
            //Seq1 is Empty & Seq2 isn't Empty
            if (MyUtility.Check.Empty(seq1) & !MyUtility.Check.Empty(seq2))
            {
                MyUtility.Msg.WarningBox("When Seq2 isn't Empty, Seq1 can't be Empty", "Seq");
                textSeq1.Focus();
                return;
            }

            //Seq1 isn't Empty & Seq2 is Empty
            if (!MyUtility.Check.Empty(seq1) & MyUtility.Check.Empty(seq2))
            {
                MyUtility.Msg.WarningBox("When Seq1 isn't Empty, Seq2 can't be Empty", "Seq");
                textSeq2.Focus();
                return;
            }
        }
    }
}
