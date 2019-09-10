using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    public partial class R18 : Sci.Win.Tems.PrintForm
    {
        public R18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }
    }
}
