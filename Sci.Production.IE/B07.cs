using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.labelDescriptionEnglish.Text = "Description\r\n(English)";
            this.labelDescriptionChinese.Text = "Description\r\n(Chinese)";

            MyUtility.Tool.SetupCombox(comboType, 2, 1, "A,Accoessory,W,Workmanship");
        }
    }
}
