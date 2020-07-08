using System.Data;
using Ict.Win;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B42_Detail
    /// </summary>
    public partial class B42_Detail : Win.Subs.Base
    {
        private DataTable gridData;

        /// <summary>
        /// B42_Detail
        /// </summary>
        /// <param name="gridData">GridData</param>
        public B42_Detail(DataTable gridData)
        {
            this.InitializeComponent();
            this.gridData = gridData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .Text("SuppID", header: "Supp ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 4, iseditingreadonly: true);

            this.listControlBindingSource1.DataSource = this.gridData;
            decimal totalQty = 0;
            if (this.gridData != null)
            {
                foreach (DataRow dr in this.gridData.Rows)
                {
                    totalQty = totalQty + MyUtility.Convert.GetDecimal(dr["Qty"]);
                }
            }

            this.numTotalQty.Value = MyUtility.Math.Round(totalQty, 3);
        }
    }
}
