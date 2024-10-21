using Ict;
using Ict.Win;
using Sci.Production.Class.Command;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public static class DataGridViewGenerator
    {
        /// <summary>
        /// TimeSpanHHmm 格式化
        /// </summary>
        /// <inheritdoc/>
        public static IDataGridViewGenerator TimeSpanHHmm(
            this IDataGridViewGenerator generator, string propertyName, string header)
        {
            DataGridViewGeneratorMaskedTextColumnSettings settings = new DataGridViewGeneratorMaskedTextColumnSettings();
            settings.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                string inputValue = e.FormattedValue.ToString();
                if (MyUtility.Check.Empty(inputValue))
                {
                    return;
                }

                // 使用 ToTimeFormat 方法轉換為 HH:mm 格式，若格式錯誤則回傳 "00:00"
                string formattedTime = inputValue.ToTimeFormat();

                // 如果轉換後的時間為 "00:00" 並且原始輸入不為 "0000"，則視為無效輸入
                if (formattedTime == "00:00" && inputValue != "0000")
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Input time must be between 00:00 and 23:59.");
                }
            };

            return generator.MaskedText(propertyName, "00:00", header, width: Widths.AnsiChars(8), settings: settings);
        }
    }
}
