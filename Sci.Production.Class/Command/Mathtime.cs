using System;
using System.Globalization;

namespace Sci.Production.Class.Command
{
    /// <inheritdoc/>
    public static class Mathtime
    {
        /// <summary>
        /// Converts the given object to a string in the time format "HH:mm".
        /// If conversion fails, returns "00:00".
        /// </summary>
        /// <param name="sender">The object to be converted.</param>
        /// <returns>A string representing the time in "HH:mm" format, or "00:00" if conversion fails.</returns>
        public static string ToTimeFormat(this object sender)
        {
            try
            {
                if (sender != null)
                {
                    string input = sender.ToString();

                    // Handle case where input is in the format "HHmm" (e.g., "0100" to "01:00")
                    if (input.Length == 4 && int.TryParse(input, out _))
                    {
                        input = input.Insert(2, ":");
                    }

                    if (TimeSpan.TryParseExact(input, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan time))
                    {
                        return time.ToString(@"hh\:mm");
                    }
                }
            }
            catch
            {
                // Ignore any parsing exceptions and return default value.
            }

            return "00:00";
        }
    }
}
