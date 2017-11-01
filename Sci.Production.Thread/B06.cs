using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Thread
{
    public partial class B06 : Sci.Win.Tems.QueryForm
    {
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            resetGridData();
            this.grid.IsEditingReadOnly = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Allowance = null;

            Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "Seq", iseditingreadonly: true)
                .Numeric("LowerBound", header: "LowerBound", iseditingreadonly: true)
                .Numeric("UpperBound", header: "UpperBound", iseditingreadonly: true)
                .Numeric("Allowance", header: "Allowance", decimal_places: 2, iseditingreadonly: true).Get(out col_Allowance)
                .EditText("Remark", header: "Remark", iseditingreadonly: true);

            col_Allowance.DecimalZeroize = Ict.Win.UI.NumericBoxDecimalZeroize.Default;
            col_Allowance.Maximum = (decimal)999.99;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resetGridData()
        {
            DataTable gridDt;
            DualResult result = DBProxy.Current.Select(null, "select * from ThreadAllowanceScale", out gridDt);
            if (result)
            {
                this.bindingSource1.DataSource = gridDt;
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
        }
    }
}
