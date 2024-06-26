using Ict;
using Ict.Win;
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
    }
}