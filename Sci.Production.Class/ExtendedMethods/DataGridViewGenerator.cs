using Ict;
using Ict.Win;
using Sci.Production.Class.Command;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public static class DataGridViewGenerator
    {
        /// <summary>
        /// TimeSpan HHmm 格式
        /// MaskedText 00:00 其中 e.FormattedValue 不會有冒號, 例如顯示 12:30 的值是 1230
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator TimeSpanHHmm(this IDataGridViewGenerator generator, string propertyName, string header)
        {
            DataGridViewGeneratorMaskedTextColumnSettings settings = new DataGridViewGeneratorMaskedTextColumnSettings();
            settings.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string inputValue = e.FormattedValue.ToString();
                if (MyUtility.Check.Empty(inputValue))
                {
                    return;
                }

                // 使用 ToTimeFormat 方法轉換為 HHmm 格式, 自動補 0
                string formatValue = inputValue.ToTimeFormatCell();
                if (formatValue == string.Empty)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Input time must be between 00:00 and 23:59.");
                }

                dr[propertyName] = formatValue;
            };

            return generator.MaskedText(propertyName, "00:00", header, width: Widths.AnsiChars(8), settings: settings);
        }
    }
}
