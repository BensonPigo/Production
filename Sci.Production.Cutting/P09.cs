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

        private void BtnImportMarker_Click(object sender, EventArgs e)
        {
            string id = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

            string sqlcmd = $@"
select top 1 s.SizeGroup, s.PatternNo, oe.markerNo, s.ID, p.Version
from Order_EachCons oe 
inner join dbo.SMNotice s on oe.SMNoticeID = s.ID
inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
inner join Pattern p with(nolock)on p.id = sd.id
where oe.ID = '{id}'
and sd.PhaseID = 'Bulk'
and p.Status='Completed'
order by p.EditDate desc
";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drSMNotice))
            {
                string styleUkey = MyUtility.GetValue.Lookup($@"select o.StyleUkey from Orders o where o.id = '{id}'");
                var form = new P02_ImportML(styleUkey, id, drSMNotice, (DataTable)this.detailgridbs.DataSource);
                form.ShowDialog();
            }
            else
            {
                MyUtility.Msg.InfoBox("Not found SMNotice Datas"); // 正常不會發生這狀況
            }
        }
    }
}
