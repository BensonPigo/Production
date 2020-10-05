using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class B07 : Sci.Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataTable dt;

            string cmd = $@"select *  from ShippingMarkTemplate_DefineColumn";

            DBProxy.Current.Select(null, cmd, out dt);
            this.grid.DataSource = dt;

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "ID", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CheckBox("FromPMS", header: "FromPMS", width: Widths.AnsiChars(6), iseditable: false, trueValue: 1, falseValue: 0)
                .Text("Desc", header: "Description", width: Widths.AnsiChars(50), iseditingreadonly: true)
                ;

        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
