using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using Ict;
using Ict.Win;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    public partial class txtmachinetype : Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            SelectItem item = new SelectItem("Select id from Machinetype WITH (NOLOCK) where junk=0", "23", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string tmp = MyUtility.GetValue.Lookup("id", str, "Machinetype", "id");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Machine Type> : {0} not found!!!", str));
                    return;
                }

                string cjunk = MyUtility.GetValue.Lookup("Junk", str, "Machinetype", "id");
                if (cjunk == "True")
                {
                    this.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("Machine Type already junk, you can't choose!!"));
                }
            }
        }

        public txtmachinetype()
        {
            this.Width = 156;
            this.IsSupportSytsemContextMenu = false;
        }
    }

    public class cellmachinetype : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(bool pur)
        {
            // pur 為ture 表示需判斷PurchaseFrom
            cellmachinetype ts = new cellmachinetype();

            ts.EditingMouseDown += (s, e) =>
            {
                // 右鍵彈出功能
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele;

                    sele = new SelectItem("Select id From Machinetype WITH (NOLOCK) where Junk=0", "23", row["machinetypeid"].ToString(), false, ",");

                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };

            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                // Parent form 若是非編輯狀態就 return
                if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["machinetypeid"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                string sql;

                sql = string.Format("Select * from machinetype WITH (NOLOCK) where Junk = 0 and ID = '{0}' ", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql))
                    {
                        row["machinetypeid"] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Machine Type > : {0} not found!!!", newValue));
                        return;
                    }
                }
            };
            return ts;
        }
    }
}