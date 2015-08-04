using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtmultifactory : Sci.Win.UI.TextBox
    {
        public txtmultifactory()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        protected override void OnPopUp(Win.UI.TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sqlWhere = "select ID from Factory where Junk = 0 order by ID";
            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sqlWhere, "Factory", "10", this.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }

    }
}
