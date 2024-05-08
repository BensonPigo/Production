using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class B08_Operation : Win.Subs.Base
    {
        private DataTable dt;

        /// <inheritdoc/>
        public B08_Operation(DataTable dataTable)
        {
            this.InitializeComponent();
            this.dt = dataTable;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .Text("ST_MC_Type", header: "ST/MC Type", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Motion", header: "Motion", width: Widths.AnsiChars(35), iseditingreadonly: true)
            .Text("Group_Header", header: "Group Header", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Part", header: "Part", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Attachment", header: "Attachment", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Effi_3_year", header: "Effi %", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
            this.listControlBindingSource1.DataSource = this.dt;
        }
    }
}
