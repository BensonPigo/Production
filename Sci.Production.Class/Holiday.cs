using System;
using System.Reflection;

namespace Sci.Production.Class
{
    /// <summary>
    /// Holiday
    /// </summary>
    public partial class Holiday : Win.UI._UserControl
    {
        /// <summary>
        /// Today
        /// </summary>
        public DateTime Today { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Holiday"/> class.
        /// </summary>
        public Holiday()
        {
            this.InitializeComponent();

            // 讓 tableLayoutPanel 更新時不會閃爍
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(this.tableLayoutPanel1, true, null);
        }
    }
}
