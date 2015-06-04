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
using Ict.Win;

namespace Sci.Production.Class
{
    public partial class txtcloglocation : Sci.Win.UI.TextBox
    {
        public txtcloglocation()
        {
            this.Size = new System.Drawing.Size(80, 23);
            this.IsSupportSytsemContextMenu = false;
            
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {            
            base.OnPopUp(e);
            if (e.IsHandled) return;

            if (this.ReadOnly == false)
            {
                string sql = "select ID,Description from ClogLocation order by ID";
                DataTable tbClogLocation;
                DBProxy.Current.Select("Production", sql, out tbClogLocation);
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbClogLocation, "ID,Description", "10,40", this.Text, "ID,Description");

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = item.GetSelectedString();
                e.IsHandled = true;
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!myUtility.Empty(str) && str != this.OldValue)
            {
                if (myUtility.Seek(str, "ClogLocation", "id") == false)
                {
                    MessageBox.Show(string.Format("< ClogLocation : {0} > not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
