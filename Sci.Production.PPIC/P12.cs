using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P12
    /// </summary>
    public partial class P12 : Win.Tems.Input1
    {
        /// <summary>
        /// P12
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "'";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.numOutputQty.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format("select isnull(sum(QAQty),0) as QAQty from SewingOutput_Detail WITH (NOLOCK) where OrderId = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))));
        }

        private void P12_FormLoaded(object sender, EventArgs e)
        {
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
        }
    }
}
