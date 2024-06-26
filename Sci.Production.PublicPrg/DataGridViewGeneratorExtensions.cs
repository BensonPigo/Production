using Ict;
using Ict.Win;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static class DataGridViewGeneratorExtensions
    {
        /// <summary>
        /// MarkerLength Cell 格式化
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator MarkerLength(
            this IDataGridViewGenerator generator,
            string propertyName,
            string header,
            string name,
            IWidth width,
            Func<DataRow, bool> canEditData,
            string propertyName2 = "MarkerLength")
        {
            DataGridViewGeneratorMaskedTextColumnSettings settings = new DataGridViewGeneratorMaskedTextColumnSettings();

            settings.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string newValue = Prgs.SetMarkerLengthMaskString(e.FormattedValue.ToString());
                string oldValue = MyUtility.Convert.GetString(dr[propertyName]);
                if (!canEditData(dr) || newValue == oldValue)
                {
                    return;
                }

                dr[propertyName] = newValue;
                dr[propertyName2] = newValue;
                dr.EndEdit();
            };

            return generator.MaskedText(propertyName, "00Y00-0/0+0\"", header, name, width, settings);
        }

        /// <summary>
        /// 所有子單 OrderID For WorkOrder Grid使用
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator WorkOrderSP(
            this IDataGridViewGenerator generator,
            string propertyName,
            string header,
            IWidth width,
            Func<string> getWorkType,
            Func<DataRow, bool> canEditData)
        {
            DataGridViewGeneratorTextColumnSettings settings = new DataGridViewGeneratorTextColumnSettings();
            settings.EditingMouseDown += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (!canEditData(dr) || getWorkType() == "1" || e.Button != MouseButtons.Right)
                {
                    return;
                }

                DualResult result = Prgs.GetAllOrderID(dr["ID"].ToString(), out DataTable dt);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return;
                }

                // 只取 ID 去重複
                DataTable dtID = dt.DefaultView.ToTable(true, "ID");

                SelectItem sele = new SelectItem(dtID, "ID", "20", dr[propertyName].ToString(), false, ",", "SP#");
                if (sele.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = sele.GetSelectedString();
            };
            settings.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string newValue = e.FormattedValue.ToString();
                string oldValue = MyUtility.Convert.GetString(dr[propertyName]);
                if (!canEditData(dr) || getWorkType() == "1" || newValue == oldValue)
                {
                    return;
                }

                DualResult result = Prgs.GetAllOrderID(dr["ID"].ToString(), out DataTable dt);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return;
                }

                if (dt.Select($"ID = '{newValue}'").Length == 0)
                {
                    dr["OrderID"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"<SP> : {newValue} data not found!");
                }
                else
                {
                    dr["OrderID"] = newValue;
                }

                dr.EndEdit();
            };

            return generator.Text(propertyName, header, null, width, settings);
        }

        /// <summary>
        /// 所有子單 OrderID For WorkOrder Grid使用
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator EstCutDate(
            this IDataGridViewGenerator generator,
            string propertyName,
            string header,
            IWidth width,
            Func<DataRow, bool> canEditData)
        {
            DataGridViewGeneratorDateColumnSettings settings = new DataGridViewGeneratorDateColumnSettings();
            settings.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (!canEditData(dr) || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                string newValue = e.FormattedValue.ToString();
                string oldValue = MyUtility.Convert.GetString(dr[propertyName]);
                if (newValue == oldValue)
                {
                    return;
                }

                if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(e.FormattedValue)) > 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Est. Cut Date] cannot be less than today");
                }
            };

            return generator.Date(propertyName, header, null, width, settings);
        }
    }
}