using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class WatermarkEditBox : Sci.Win.UI.EditBox
    {
        private string _watermarkText;
        private Color _watermarkColor;

        /// <inheritdoc/>
        [Browsable(true)]
        [Category("Watermark")]
        [Description("The watermark text to display when the textbox is empty.")]
        public string WatermarkText
        {
            get { return this._watermarkText; }
            set { this._watermarkText = value; this.Invalidate(); }
        }

        /// <inheritdoc/>
        [Browsable(true)]
        [Category("Watermark")]
        [Description("The color of the watermark text.")]
        public Color WatermarkColor
        {
            get
            {
                return this._watermarkColor;
            }

            set
            {
                this._watermarkColor = value;
                this.Invalidate();
            }
        }

        private const int WM_PAINT = 0xF;

        /// <inheritdoc/>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT && string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.WatermarkText))
            {
                using (var g = Graphics.FromHwnd(this.Handle))
                {
                    var brush = new SolidBrush(this.WatermarkColor);
                    g.DrawString(this.WatermarkText, this.Font, brush, new PointF(0, 0));
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Invalidate();
        }
    }
}
