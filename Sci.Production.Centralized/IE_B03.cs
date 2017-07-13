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
    public partial class IE_B03 : Sci.Win.Tems.Input1
    {
        public IE_B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (String.IsNullOrWhiteSpace(CurrentMaintain["id"].ToString()))
            {
                CurrentMaintain["type"] = "LM";
                if (cbResult = DBProxy.Current.Select(null, "select max(id) max_id from IEReason WITH (NOLOCK) where type='LM'", out whseReasonDt))
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
