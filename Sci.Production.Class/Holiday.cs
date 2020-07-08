using System;
using System.Reflection;

namespace Sci.Production.Class
{
    public partial class Holiday : Sci.Win.UI._UserControl
    {
        public DateTime Today;

        public Holiday()
        {
            this.InitializeComponent();

            // 讓 tableLayoutPanel 更新時不會閃爍
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(this.tableLayoutPanel1, true, null);
        }
    }
}
