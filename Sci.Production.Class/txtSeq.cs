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
    }
}
