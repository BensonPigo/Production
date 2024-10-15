using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class B05 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (this.EditMode == true)
            {
                this.gridDetail.IsEditingReadOnly = false;
            }
            else
            {
                this.gridDetail.IsEditingReadOnly = true;
            }

            string sql = string.Format("select * from [MachineType_ThreadRatio] where ID='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sql, this.ConnectionName))
            {
                this.btnThreadRatio.ForeColor = Color.Blue;
            }
            else
            {
                this.btnThreadRatio.ForeColor = DefaultForeColor;
            }

            string sqlQuery = $@"
select * 
from dbo.MachineType_Detail
where ID = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out DataTable dt);
            if (result == false)
            {
                this.ShowErr(result);
                this.listControlBindingSource1.DataSource = null;
                return;
            }
            else
            {
                this.listControlBindingSource1.DataSource = dt;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Set Grid
            this.gridDetail.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
             .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(13), iseditingreadonly: true)
             .CheckBox("IsSubprocess", header: "Subprocess", width: Widths.AnsiChars(15), iseditable: false, trueValue: true, falseValue: false)
             .CheckBox("IsNonSewingLine", header: "Non-Sewing Line", width: Widths.AnsiChars(17), iseditable: false, trueValue: true, falseValue: false)
             .CheckBox("IsNotShownInP01", header: "Not shown in P01", width: Widths.AnsiChars(17), iseditable: false, trueValue: true, falseValue: false)
             .CheckBox("IsNotShownInP03", header: "Not shown in P03", width: Widths.AnsiChars(17), iseditable: false, trueValue: true, falseValue: false)
             .CheckBox("IsNotShownInP05", header: "Not shown in P05", width: Widths.AnsiChars(17), iseditable: false, trueValue: true, falseValue: false)
             .CheckBox("IsNotShownInP06", header: "Not shown in P06", width: Widths.AnsiChars(17), iseditable: false, trueValue: true, falseValue: false)
            ;
        }

        private void BtnThreadRatio_Click(object sender, EventArgs e)
        {
            B05_ThreadRatio callNextForm = new B05_ThreadRatio(this.CurrentMaintain["ID"].ToString());
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
