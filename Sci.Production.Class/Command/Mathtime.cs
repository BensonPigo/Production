using System;
using System.Globalization;

namespace Sci.Production.Class.Command
{
    /// <inheritdoc/>
    public static class Mathtime
    {
        /// <summary>
        /// For grid masktext column, time format "HH:mm".
        /// </summary>
        /// <param name="sender">The object to be converted.</param>
        /// <inheritdoc/>
        public static string ToTimeFormat(this object sender)
        {
            if (sender == null)
            {
                return null;
            }

            string input = sender.ToString();
            input = input.Replace(":", string.Empty); // 不一定有冒號, 先去除冒號
            string format4num = input.ReplaceZeroHHmm().Insert(2, ":");
            if (TimeSpan.TryParseExact(format4num, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan time))
            {
                return time.ToString(@"hh\:mm");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// For grid masktext column, time format "HHmm".
        /// </summary>
        /// <param name="sender">The object to be converted.</param>
        /// <inheritdoc/>
        public static string ToTimeFormatCell(this object sender)
        {
            if (sender == null)
            {
                return null;
            }

            string input = sender.ToString();
            input = input.Replace(":", string.Empty); // 不一定有冒號, 先去除冒號
            string replaceZero = input.ReplaceZeroHHmm();
            string format4num = replaceZero.Insert(2, ":");
            if (TimeSpan.TryParseExact(format4num, @"hh\:mm", CultureInfo.InvariantCulture, out _))
            {
                return replaceZero;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 將空白符號替換為 '0'，並將字串補足至 4 位數。
        /// </summary>
        /// <param name="input">要處理的字串。</param>
        /// <returns>替換空白並補齊至 4 位數的字串。</returns>
        public static string ReplaceZeroHHmm(this string input)
        {
            input = input.Replace(" ", "0");
            return input.PadRight(4, '0');
        }
    }
}
