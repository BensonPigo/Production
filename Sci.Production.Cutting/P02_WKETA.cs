using Ict.Win;
using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Cutting
{
    public partial class P02_WKETA : Win.Tems.QueryForm
    {
        DataRow CurrentRow;

        public P02_WKETA(DataRow currentRow)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.CurrentRow = currentRow;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.CellClick += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    itemx.WKETA = MyUtility.Convert.GetDate(dr["WKETA"]);
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
            };

            this.grid1.RowsAdded += (s, e) =>
            {
                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataGridViewRow dr = this.grid1.Rows[index];
                    dr.Cells["selected"].ReadOnly = true;
                    index++;
                }
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("WK", header: "WK#", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Date("WKETA", header: "WKETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            ;
            this.Query();
        }

        private void Query()
        {
            string sqlcmd = $@"
select selected  = 0, WK= ED.id , WKETA=E.ETA, ED.Qty From Export_Detail ED 
INNER JOIN Export E ON E.ID = ED.ID
where ED.poid='{this.CurrentRow["ID"]}' and ED.seq1 ='{this.CurrentRow["seq1"]}' and ED.seq2='{this.CurrentRow["seq2"]}'
ORDER BY ETA 
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                if (MyUtility.Convert.GetDate(row["WKETA"]).Equals(MyUtility.Convert.GetDate(this.CurrentRow["WKETA"])))
                {
                    row["selected"] = 1;
                    break;
                }
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }

    public static class itemx
    {
        public static DateTime? WKETA;
    }
}
