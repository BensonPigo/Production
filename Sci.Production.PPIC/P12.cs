using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P12 : Sci.Win.Tems.Input1
    {
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            numOutputQty.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format("select isnull(sum(QAQty),0) as QAQty from SewingOutput_Detail WITH (NOLOCK) where OrderId = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))));
        }

        private void P12_FormLoaded(object sender, EventArgs e)
        {
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
        }
    }
}
