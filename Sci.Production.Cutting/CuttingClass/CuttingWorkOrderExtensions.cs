using Ict;
using Ict.Win;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public static class DataGridViewGeneratorExtensions_WorkOrder
    {
        /// <summary>
        /// WKETA Grid Column Event
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator WorkOrderWKETA(
            this IDataGridViewGenerator generator,
            string propertyName,
            string header,
            IWidth width,
            bool iseditingreadonly,
            Func<DataRow, bool> canEditData)
        {
            DataGridViewGeneratorDateColumnSettings settings = new DataGridViewGeneratorDateColumnSettings();
            settings.EditingMouseDown += (s, e) =>
            {
                System.Windows.Forms.DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (!canEditData(dr) || e.Button != MouseButtons.Right)
                {
                    return;
                }

                P02_WKETA item = new P02_WKETA(dr);
                DialogResult result = item.ShowDialog();
                switch (result)
                {
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Yes:
                        dr[propertyName] = Itemx.WKETA;
                        break;
                    case DialogResult.No:
                        dr[propertyName] = DBNull.Value;
                        break;
                }

                dr.EndEdit();
            };

            return generator.Date(propertyName, header, null, width, settings, iseditingreadonly: iseditingreadonly);
        }

        /// <summary>
        /// 所有子單 OrderID For WorkOrder Grid使用
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator MarkerNo(
            this IDataGridViewGenerator generator,
            string propertyName,
            string header,
            IWidth width,
            Func<DataRow, bool> canEditData)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();
            settings.EditingMouseDown += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (!canEditData(dr) || e.Button != MouseButtons.Right)
                {
                    return;
                }

                SelectItem selectItem = CuttingWorkOrder.PopupMarkerNo(dr["ID"].ToString(), dr[propertyName].ToString());
                if (selectItem == null)
                {
                    return;
                }

                e.EditingControl.Text = selectItem.GetSelectedString();
            };
            settings.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string newValue = e.FormattedValue.ToString();
                string oldValue = MyUtility.Convert.GetString(dr[propertyName]);
                if (!canEditData(dr) || newValue == oldValue)
                {
                    return;
                }

                if (!CuttingWorkOrder.ValidatingMarkerNo(dr["ID"].ToString(), e.FormattedValue.ToString()))
                {
                    dr[propertyName] = string.Empty;
                    e.Cancel = true;
                }

                dr[propertyName] = e.FormattedValue;
                dr.EndEdit();
            };

            return generator.Text(propertyName, header, null, width, settings);
        }

        /// <summary>
        /// P02 SEQ, P09 CutNo
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator NumericNull(
            this IDataGridViewGenerator generator,
            string propertyName,
            string header,
            IWidth width,
            Func<DataRow, bool> canEditData)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();
            settings.EditingKeyPress += (s, e) =>
            {
                // 限制只能輸入數字
                string input = e.EditingControl.Text;
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // 阻止非數字字符的輸入
                }
            };

            settings.CellValidating += (s, e) =>
             {
                 DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                 DataRow dr = grid.GetDataRow(e.RowIndex);
                 if (!canEditData(dr))
                 {
                     return;
                 }

                 if (MyUtility.Convert.GetString(e.FormattedValue) == string.Empty)
                 {
                     dr[propertyName] = DBNull.Value;
                 }
                 else
                 {
                     dr[propertyName] = e.FormattedValue;
                 }

                 dr.EndEdit();
             };

            return generator.Text(propertyName, header, null, width, settings);
        }
    }
}