using Ict;
using Ict.Win;
using Sci.Data;
using System.Data;

namespace Sci.Production.Shipping
{
    public partial class P02_History : Win.Subs.Base
    {
        private string HCNo;

        public P02_History(string hCNo)
        {
            this.InitializeComponent();
            this.HCNo = hCNo;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid.IsEditingReadOnly = true;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("NewValue", header: "Status", width: Widths.AnsiChars(15))
                .Text("OldValue", header: "Old Status", width: Widths.AnsiChars(15))
                .Text("Update", header: "Update", width: Widths.AnsiChars(30));

            string sqlCmd = string.Format(
                @"select eh.OldValue, eh.NewValue, [Update] = eh.AddName + '-' + p.Name + ' ' + FORMAT(eh.AddDate, 'yyyy/MM/dd HH:mm:ss')
from Express_History eh
left join pass1 p on eh.AddName = p.ID
where eh.ID = '{0}'
order by eh.AddDate", this.HCNo);

            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }
    }
}
