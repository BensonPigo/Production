using System.Data;
using System.Windows.Forms;
using Ict;

namespace Sci
{
    /// <summary>
    /// CartonRefnoCommon
    /// </summary>
    public class CartonRefnoCommon
    {
        /// <inheritdoc/>
        public static void EditingMouseDown(object sender, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            Win.UI.Grid g = (Win.UI.Grid)((DataGridViewColumn)sender).DataGridView;
            Win.Forms.Base frm = (Win.Forms.Base)g.FindForm();
            if (frm.EditMode)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                        Win.Tools.SelectItem item = new Win.Tools.SelectItem("select RefNo,Description,STR(CtnLength,8,4)+'*'+STR(CtnWidth,8,4)+'*'+STR(CtnHeight,8,4) as Dim from LocalItem where Category = 'CARTON' and Junk = 0 order by RefNo", "10,25,25", dr["RefNo"].ToString().Trim());
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }

                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            }
        }
    }
}
