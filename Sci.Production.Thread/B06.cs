using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// B06
    /// </summary>
    public partial class B06 : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// B06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.ResetGridData();
            this.grid.IsEditingReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Allowance = null;

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "Seq", iseditingreadonly: true)
                .Numeric("LowerBound", header: "LowerBound", iseditingreadonly: true)
                .Numeric("UpperBound", header: "UpperBound", iseditingreadonly: true)
                .Numeric("Allowance", header: "Allowance", decimal_places: 2, iseditingreadonly: true).Get(out col_Allowance)
                .EditText("Remark", header: "Remark", iseditingreadonly: true);

            col_Allowance.DecimalZeroize = Ict.Win.UI.NumericBoxDecimalZeroize.Default;
            col_Allowance.Maximum = (decimal)999.99;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetGridData()
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
