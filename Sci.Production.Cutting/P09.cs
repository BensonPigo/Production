using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P09 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
        }
    }
}
