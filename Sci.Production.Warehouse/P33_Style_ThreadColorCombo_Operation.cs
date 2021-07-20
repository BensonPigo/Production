using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33_Style_ThreadColorCombo_Operation : Sci.Win.Subs.Base
    {
        private string style_ThreadColorCombo_HistoryUkey = string.Empty;

        /// <inheritdoc/>
        public P33_Style_ThreadColorCombo_Operation(string style_ThreadColorCombo_HistoryUkey)
        {
            this.InitializeComponent();
            this.style_ThreadColorCombo_HistoryUkey = style_ThreadColorCombo_HistoryUkey;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string cmd = $@"
select   a.Operationid
        , b.DescEN
        , a.SeamLength  
        , a.Frequency
from Style_ThreadColorCombo_History_Operation a WITH (NOLOCK)
INNER JOIN Operation b ON b.Id = a.OperationId 
where Style_ThreadColorCombo_HistoryUkey = '{this.style_ThreadColorCombo_HistoryUkey}'
";

            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            this.listControlBindingSource1.DataSource = dt;

            this.Helper.Controls.Grid.Generator(this.grid1)
           .Text("Operationid", header: "Operation ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("DescEN", header: "Description", width: Widths.AnsiChars(45), iseditingreadonly: true)
           .Numeric("SeamLength", header: "SeamLength", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Numeric("Frequency", header: "Frequency", width: Widths.AnsiChars(15), iseditingreadonly: true);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
