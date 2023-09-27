﻿using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using Ict;
using Ict.Win;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtMachineGroup
    /// </summary>
    public partial class TxtMachineGroup : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtMachineGroup"/> class.
        /// </summary>
        public TxtMachineGroup()
        {
            this.Width = 156;
            this.IsSupportSytsemContextMenu = false;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            SelectItem item = new SelectItem("SELECT DISTINCT MasterPlusGroup FROM Operation WITH (NOLOCK) WHERE MasterPlusGroup <> '' AND Junk=0 ", "23", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public class CelltxtMachineGroup : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static CelltxtMachineGroup GetGridCell()
            {
                ////pur 為ture 表示需判斷PurchaseFrom
                CelltxtMachineGroup ts = new CelltxtMachineGroup();

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

                        string sqlMachineGroup = " SELECT DISTINCT MasterPlusGroup FROM Operation WITH (NOLOCK) WHERE MasterPlusGroup <> '' AND Junk=0 ";

                        if (row.Table.Columns.IndexOf("MachineTypeID") != -1)
                        {
                            if (MyUtility.Check.Empty(row["MachineTypeID"]))
                            {
                                MyUtility.Msg.WarningBox("Please input ST/MC first.");
                                return;
                            }

                            sqlMachineGroup += $" and MachineTypeID = '{row["MachineTypeID"]}'";
                        }

                        sele = new SelectItem(sqlMachineGroup, "23", row["MasterPlusGroup"].ToString(), false, ",");

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
                    string oldValue = row["MasterPlusGroup"].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
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
}