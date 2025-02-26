using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtMarkerLength 自定義元件
    /// </summary>
    public partial class TxtMarkerLength : Win.UI.TextBox
    {
        /// <inheritdoc/>
        public TxtMarkerLength()
        {
            this.Mask = "00Y00-0/0+0\"";
            this.Size = new System.Drawing.Size(96, 23);
            this.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            // 檢查輸入的數值部分（不包括符號）
            this.Text = this.Text.Replace('_', '0');
            string inputOnly = Regex.Replace(this.Text, "[^0-9]", string.Empty);

            // 如果輸入的內容只包含0，則清空值
            if (!string.IsNullOrEmpty(inputOnly) && inputOnly.Trim('0').Length == 0)
            {
                this.Clear();
                return;
            }

            base.OnValidating(e);
        }

        /// <inheritdoc/>
        public bool HasValue
        {
            get
            {
                // 檢查輸入的數值部分（不包括符號）
                string inputOnly = Regex.Replace(this.Text, "[^0-9]", string.Empty);
                return !string.IsNullOrEmpty(inputOnly);
            }
        }

        /// <inheritdoc/>
        public string FullText
        {
            get
            {
                return this.HasValue ? this.Text : string.Empty;
            }
        }
    }
}
