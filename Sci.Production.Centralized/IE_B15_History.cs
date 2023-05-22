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

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B15_History
    /// </summary>
    public partial class IE_B15_History : Sci.Win.Tems.QueryForm
    {
        private long automatedLineMappingConditionSettingUkey;

        /// <summary>
        /// IE_B15_History
        /// </summary>
        /// <param name="automatedLineMappingConditionSettingUkey">automatedLineMappingConditionSettingUkey</param>
        public IE_B15_History(long automatedLineMappingConditionSettingUkey)
        {
            this.InitializeComponent();
            this.automatedLineMappingConditionSettingUkey = automatedLineMappingConditionSettingUkey;
            this.gridHistory.DataSource = this.bindingGridHistory;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridHistory)
                .Text("HisType", header: "Condition No", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OldValue", header: "Old Value", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("NewValue", header: "New Value", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ModifyBy", header: "Modify By", width: Widths.AnsiChars(35), iseditingreadonly: true);
            string pass1Source = DBProxy.Current.DefaultModuleName.Contains("testing") ? "Production.dbo.pass1" : "tradedb.dbo.pass1";
            string sqlGetHistory = $@"
select  als.HisType,
        als.OldValue,
        als.NewValue,
        [ModifyBy] = als.AddName + '-' + p.Name + ' ' + Format(als.AddDate,  'yyyy/MM/dd HH:mm:ss')
from AutomatedLineMappingConditionSetting_History als with (nolock)
left join {pass1Source} p with (nolock) on als.AddName = p.ID
where AutomatedLineMappingConditionSettingUkey = '{this.automatedLineMappingConditionSettingUkey}'
order by als.AddDate desc
";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select("ProductionTPE", sqlGetHistory, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                this.Close();
                return;
            }

            this.bindingGridHistory.DataSource = dtResult;
            this.comboBoxFilter.Add(string.Empty, string.Empty);
            foreach (string hisType in dtResult.AsEnumerable().Select(s => s["HisType"].ToString()).Distinct().OrderBy(s => s))
            {
                this.comboBoxFilter.Add(hisType, hisType);
            }

            this.comboBoxFilter.SelectedIndex = 0;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.bindingGridHistory.DataSource == null)
            {
                return;
            }

            this.bindingGridHistory.Filter = MyUtility.Check.Empty(this.comboBoxFilter.Text) ? string.Empty : $"HisType = '{this.comboBoxFilter.Text}'";
        }
    }
}
