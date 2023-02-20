using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// TransferExport_StatusHistory
    /// </summary>
    public partial class TK_StatusHistory : Sci.Win.Tems.QueryForm
    {
        private string transferExportID;

        /// <summary>
        /// TransferExport_StatusHistory
        /// </summary>
        /// <param name="transferExportID">transferExportID</param>
        public TK_StatusHistory(string transferExportID)
        {
            this.InitializeComponent();
            this.transferExportID = transferExportID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridTransferExport_StatusHistory)
                .Text("NewStatus", header: "New TPE Status", width: Widths.AnsiChars(23), iseditingreadonly: true)
                .Text("OldStatus", header: "Old TPE Status", width: Widths.AnsiChars(23), iseditingreadonly: true)
                .Text("NewFtyStatus", header: "New Fty Status", width: Widths.AnsiChars(23), iseditingreadonly: true)
                .Text("OldFtyStatus", header: "Old Fty Status", width: Widths.AnsiChars(23), iseditingreadonly: true)
                .DateTime("UpdateDate", header: "Update Date", width: Widths.AnsiChars(20), iseditingreadonly: true);

            this.Query();
        }

        private void Query()
        {
            string sqlGetData = $@"
select  [NewStatus] = dbo.GetTransferExportStatusDesc(NewStatus, 'Status'),
        [OldStatus] = dbo.GetTransferExportStatusDesc(OldStatus, 'Status'),
        [NewFtyStatus] = dbo.GetTransferExportStatusDesc(NewFtyStatus, 'NewFtyStatus'),
        [OldFtyStatus] = dbo.GetTransferExportStatusDesc(OldFtyStatus, 'OldFtyStatus'),
        UpdateDate
from    TransferExport_StatusHistory with (nolock)
where   ID = '{this.transferExportID}'
order by UpdateDate
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridTransferExport_StatusHistory.DataSource = dtResult;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
