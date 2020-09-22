using Sci.Win.UI;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public class MergeRowGrid : Grid
    {
        /// <inheritdoc/>
        protected override void InitLayout()
        {
            base.InitLayout();
        }

        /// <inheritdoc/>
        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }

            if (this.Columns[e.ColumnIndex].DataPropertyName.Equals("selected") && e.RowIndex == -1)
            {
                foreach (DataGridViewRow dr in this.Rows)
                {
                    if (dr.Cells[0].GetType().BaseType.Name == "DataGridViewCheckBoxCell")
                    {
                        DataGridViewCheckBoxCell drChk = (DataGridViewCheckBoxCell)dr.Cells[0];
                        drChk.Value = drChk.Value.ToString() == "Y" ? string.Empty : "Y";
                    }
                }

                this.RefreshEdit();
                return;
            }
            else
            {
                base.OnCellClick(e);
            }
        }
    }
}
