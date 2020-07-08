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
    public partial class txtMachineGroup : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("SELECT DISTINCT MasterPlusGroup FROM Operation WITH (NOLOCK) WHERE MasterPlusGroup <> '' AND Junk=0 ", "23", this.Text, false, ",");
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
                string tmp = MyUtility.GetValue.Lookup($"SELECT DISTINCT MasterPlusGroup FROM Operation WITH (NOLOCK) WHERE MasterPlusGroup = '{str}'");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Machine Group > : {0} not found!!!", str));
                    return;
                }

                bool isJunk = MyUtility.Check.Seek($"SELECT DISTINCT MasterPlusGroup FROM Operation WITH (NOLOCK) WHERE MasterPlusGroup = '{str}' AND Junk=1 ");
                if (isJunk)
                {
                    this.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("Machine Group already junk, you can't choose!!"));
                }
            }
        }

        public txtMachineGroup()
        {
            this.Width = 156;
            this.IsSupportSytsemContextMenu = false;
        }
    }

    public class celltxtMachineGroup : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell()
        {
            ////pur 為ture 表示需判斷PurchaseFrom
            celltxtMachineGroup ts = new celltxtMachineGroup();

            ts.EditingMouseDown += (s, e) =>
            {
                // 右鍵彈出功能
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele;

                    sele = new SelectItem("SELECT DISTINCT MasterPlusGroup FROM Operation WITH (NOLOCK) WHERE MasterPlusGroup <> '' AND Junk=0 ", "23", row["MasterPlusGroup"].ToString(), false, ",");

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
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["MasterPlusGroup"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                string sql;

                sql = string.Format("SELECT 1 FROM Operation WITH (NOLOCK) WHERE Junk=0 AND MasterPlusGroup = '{0}' ", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql))
                    {
                        row["MasterPlusGroup"] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Machine Group > : {0} not found!!!", newValue));
                        return;
                    }
                }
            };
            return ts;
        }
    }
}