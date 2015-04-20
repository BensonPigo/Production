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
        private string cfty = "";
        
        [Category("Custom Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string cFactoryid
        {
            set { cfty = value; }
            get { return cfty; }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("CutCell.id", "30", this.Text, false, ",");
            //select id from CutCell where factoryid = cfty and !junk
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string tmp = myUtility.Lookup("id", cfty + str, "Cutcell", "factoryid+id");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    MessageBox.Show(string.Format("< Cut Cell> : {0} not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
                else
                {
                    string cJunk = myUtility.Lookup("Junk", cfty + str, "CutCell", "factoryid+id");
                    if (cJunk == "True")
                    {
                        MessageBox.Show(string.Format("Cut Cell already junk, you can't choose!!"));
                        this.Text = "";
                    }

                }
            }
        }
        public txtCell()
        {
            this.Width = 50;
        }

    }
}