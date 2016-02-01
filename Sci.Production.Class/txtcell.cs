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


namespace Sci.Production.Class
{
  
    public partial class txtCell : Sci.Win.UI.TextBox
    {
        private string fty = "";
        private string where = "Where junk = 0";
        [Category("Custom Properties")]
        public string FactoryId
        {
            set { fty = value; }
            get { return fty; }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;
            if (!string.IsNullOrWhiteSpace(fty))
            {
                where = where + string.Format(" Factoryid = '{0}'", fty);
            }
            sql = "select id from CutCell"+where ;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "2", this.Text, false, ",");
            
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
                string tmp = MyUtility.GetValue.Lookup("id", fty + str, "Cutcell", "factoryid+id");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Cut Cell> : {0} not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    string cJunk = MyUtility.GetValue.Lookup("Junk", fty + str, "CutCell", "factoryid+id");
                    if (cJunk == "True")
                    {
                        MyUtility.Msg.WarningBox(string.Format("Cut Cell already junk, you can't choose!!"));
                        this.Text = "";
                    }

                }
            }
        }
        public txtCell()
        {
            this.Width = 30;
        }

    }
}