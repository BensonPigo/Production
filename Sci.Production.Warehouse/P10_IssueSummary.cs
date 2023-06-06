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

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10_IssueSummary : Win.Subs.Base
    {
        private string strid;

        /// <inheritdoc/>
        public P10_IssueSummary(string id)
        {
            this.InitializeComponent();
            this.strid = id;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridArtworkSummary.IsEditingReadOnly = true;
            this.gridArtworkSummary.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridArtworkSummary)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("sumQty", header: "Ttl Issue Qty", decimal_places: 2, iseditingreadonly: true);

            string sqlCmd = $@"
            select
            poid,
            seq1,
            seq2,
            [sumQty] = Sum (Qty)
            from Issue_Detail
            where id = '{this.strid}'
            group by poid,seq1,seq2";
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Artwork fail\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
