using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    class P99_CallForm
    {
        public static bool CallForm(string TransID, string FormName, Form thisForm)
        {
            P99 callP99 = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form is P99)
                {
                    form.Activate();
                    P99 activateForm = (P99)form;
                    activateForm.Initl(false);
                    activateForm.SetFilter(TransID, FormName);
                    activateForm.Query();
                    return true;
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

            callP99 = new P99(p99MenuItem, TransID, FormName);
            callP99.MdiParent = thisForm.MdiParent;
            callP99.Show();

            return true;
        }

    }
}
