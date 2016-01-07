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
using Sci;
using Ict;
using Sci.Win;
using Ict.Win;
using Sci.Win.Tools;


namespace Sci.Production.Class
{

    public partial class txtthreadcolor : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select id,description from threadcolor where junk=0", "23,40", this.Text, false, ",");
            //
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string tmp = MyUtility.GetValue.Lookup("id", str, "threadcolor", "id");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Thread Color> : {0} not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
                string cjunk = MyUtility.GetValue.Lookup("Junk", str, "threadcolor", "id");
                if (cjunk == "True")
                {
                    MyUtility.Msg.WarningBox(string.Format("Thread Color already junk, you can't choose!!"));
                    this.Text = "";
                }
            }
        }
        public txtthreadcolor()
        {
            this.Width = 90;
            this.IsSupportSytsemContextMenu = false;
        }
    }
}