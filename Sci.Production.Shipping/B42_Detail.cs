using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class B42_Detail : Sci.Win.Subs.Base
    {
        DataTable gridData;
        public B42_Detail(DataTable GridData)
        {
            InitializeComponent();
            gridData = GridData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(grid1)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .Text("SuppID", header: "Supp ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 4, iseditingreadonly: true);

            listControlBindingSource1.DataSource = gridData;
            decimal totalQty = 0;
            if (gridData != null)
            {
                foreach (DataRow dr in gridData.Rows)
                {
                    totalQty = totalQty + MyUtility.Convert.GetDecimal(dr["Qty"]);
                }
            }
            numericBox1.Value = MyUtility.Math.Round(totalQty,3);
        }
    }
}
