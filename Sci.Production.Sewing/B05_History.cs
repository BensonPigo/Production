using Ict;
using Ict.Win;
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

namespace Sci.Production.Sewing
{
    /// <summary>
    /// B05_History
    /// </summary>
    public partial class B05_History : Sci.Win.Tems.QueryForm
    {
        private long sewingOutputEfficiencyUkey;

        /// <summary>
        /// B05_History
        /// </summary>
        /// <param name="sewingOutputEfficiencyUkey">sewingOutputEfficiencyUkey</param>
        public B05_History(long sewingOutputEfficiencyUkey)
        {
            this.InitializeComponent();
            this.sewingOutputEfficiencyUkey = sewingOutputEfficiencyUkey;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridSewingOutputEfficiency_History)
               .DateTime("AddDate", header: "Date", iseditingreadonly: true)
               .Text("Actioner", header: "Actioner", width: Widths.AnsiChars(25), iseditingreadonly: true)
               .Text("Action", header: "Action", iseditingreadonly: true)
               .Text("OldValue", header: "Old Value", iseditingreadonly: true)
               .Text("NewValue", header: "New Value", iseditingreadonly: true);

            this.Query();
        }

        private void Query()
        {
            string sqlGetData = $@"
select  sh.AddDate,
        [Actioner] = sh.AddName + '-' + p.Name,
        sh.Action,
        sh.OldValue,
        sh.NewValue
from SewingOutputEfficiency_History sh with (nolock)
left join pass1 p with (nolock) on p.ID = sh.AddName
where SewingOutputEfficiencyUkey = '{this.sewingOutputEfficiencyUkey}'
";

            DataTable dtSewingOutputEfficiency_History = new DataTable();

            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtSewingOutputEfficiency_History);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridSewingOutputEfficiency_History.DataSource = dtSewingOutputEfficiency_History;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
