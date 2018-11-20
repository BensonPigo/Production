using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;

namespace Sci.Production.Class
{
    public partial class txtsubprocess : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "Select ID From Subprocess WITH (NOLOCK) where junk=0";

            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: null, defaultValueColumn: null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();

            this.ValidateText();
        }
    }
}
