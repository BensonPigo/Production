using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict.Win.UI;
using System.Reflection;
using Sci.Win;

namespace Sci.Production.Class
{
    public partial class Holiday : Sci.Win.UI._UserControl
    {
        public DateTime Today;

        public Holiday()
        {
            InitializeComponent();
            // 讓 tableLayoutPanel 更新時不會閃爍
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(tableLayoutPanel1, true, null);
        }
    }
}
