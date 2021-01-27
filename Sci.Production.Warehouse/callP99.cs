using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    class callP99
    {
        public void CallForm(string TransID, string FormName)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is P99)
                {
                    form.Activate();
                    P99 activateForm = (P99)form;
                    activateForm.Initl(false);
                    //activateForm.SetFilter(this.CurrentMaintain["ID"].ToString(), "P07");
                    activateForm.Query();
                    return;
                }
            }

            ToolStripMenuItem p99MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P99. Send To WMS Command Status"))
                            {
                                p99MenuItem = (ToolStripMenuItem)subMenuItem;
                                break;
                            }
                        }
                    }
                }
            }

            //this.callP99 = new P99(p99MenuItem, this.CurrentMaintain["ID"].ToString(), "P07");
            //this.callP99.MdiParent = this.MdiParent;
            //this.callP99.Show();


        }

    }
}
