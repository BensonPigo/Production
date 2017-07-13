using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Centralized
{
    public partial class Packing_B01 : Sci.Win.Tems.Input1
    {
        public Packing_B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ClickSaveBefore()
        {
            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (String.IsNullOrWhiteSpace(CurrentMaintain["id"].ToString()))
            {
                CurrentMaintain["type"] = "OG";
                if (cbResult = DBProxy.Current.Select(null, "select max(id) max_id from PackingReason WITH (NOLOCK) where type='OG'", out whseReasonDt))
                {
                    string id = whseReasonDt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    { CurrentMaintain["id"] = "00001"; }
                    else
                    {
                        int newID = int.Parse(id) + 1;
                        CurrentMaintain["id"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                    }
                }
                else { ShowErr(cbResult); }

            }
            return base.ClickSaveBefore();
        }
    }
}
