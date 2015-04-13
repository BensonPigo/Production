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
    public partial class txtshipterm : Sci.Win.UI.TextBox
    {
        public txtshipterm()
        {
            //InitializeComponent();
            #region PopUp
            this.PopUp += (s, e) =>
            {
                //Sci.Win.UI.TextBox textBox = (Sci.Win.UI.TextBox) s;
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Shipterm.id,Description", "10,150", this.Text, false, ",");
                // SELECT Id,Description FROM Shipterm order by id
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = item.GetSelectedString();
            };
            #endregion

            #region Validating
            this.Validating += (s, e) =>
            {
                string str = this.Text;
                if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
                {
                    string tmp = myUtility.Lookup("id", str, "Shipterm", "id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Ship Term : {0} > not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        this.Text = tmp;
                    }

                }
            };
            #endregion
        }


    }
}
