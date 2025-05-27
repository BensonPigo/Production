using Ict;
using Sci.Data;
using Sci.Production.Class.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;

namespace Sci.Production.Class.Commons
{
    /// <summary>
    /// 通用呼叫的Function Class
    /// </summary>
    public static class CommonPrg
    {
        /// <summary>
        /// Open Form by detail
        /// </summary>
        /// <param name="form"> Form </param>
        /// <param name="id"> id </param>
        public static void OpenFormDetail(Form form, string id)
        {
            if (!MyUtility.Check.Empty(id))
            {
                BindingSource gridbs = null;

                var findTab = form.Controls.Find("Tabs", false);
                var findGrid = form.Controls.Find("Grid", true);

                if (findTab.Length > 0 && findGrid.Length > 0)
                {
                    Sci.Win.UI.TabControl tt = (Sci.Win.UI.TabControl)findTab[0];
                    var findReload = form.Controls.Find("reloaddata", true);
                    if (findReload.Length > 0)
                    {
                        tt.SelectedIndex = 0;
                        Button reloadBtn = findReload[0] as Button;
                        reloadBtn.PerformClick();
                    }

                    tt.SelectedIndex = 1;

                    Sci.Win.UI.Grid gg = (Sci.Win.UI.Grid)findGrid[0];
                    gridbs = (BindingSource)gg.DataSource;
                }

                if (gridbs != null)
                {
                    DataTable dt = (DataTable)gridbs.DataSource;
                    int idx = dt.Rows.IndexOf(dt.AsEnumerable().Where(w => w["ID"].ToString() == id).FirstOrDefault());
                    if (idx < 0)
                    {
                        MyUtility.Msg.WarningBox("Unable to locate the correct target. Please manually open the item.");
                    }

                    gridbs.Position = idx;
                }
            }
        }
    }
}
