using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class B17 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = "JUNK = 0";
            this.ReloadDatas();

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = $"JUNK = 0";
                        break;
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                this.ReloadDatas();
            };
        }

        private void TxtDept_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem("SELECT DISTINCT Dept FROM Employee WHERE Dept IS NOT NULL AND Dept <> ''", "35", this.txtDept.Text);

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["Dept"] = item.GetSelectedString();
            }
        }

        private void TxtPosition_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem("SELECT DISTINCT Position FROM Employee WHERE Position IS NOT NULL AND Position <> ''", "35", this.txtDept.Text);

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["Position"] = item.GetSelectedString();
            }
        }
    }
}
