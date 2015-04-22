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
    public partial class txtbuyer : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Buyer.id,NameEN", "10,50", this.Text, false, ",");
            // select id, NameEN from buyer where junk = 0
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
                if (myUtility.Seek(str, "Buyer", "id") == false)
                {
                    MessageBox.Show(string.Format("< Buyer : {0} > not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }
        
        public txtbuyer()
        {
            //this._Alias = "Buyer";
            //this._Tag = "ID";
            //this.HelpColumnWidths = "10,50";
            //this.HelpRecordSource = "Buyer.id,NameEN";
            //this.Size = new System.Drawing.Size(100, 22);

        }
    }
}
