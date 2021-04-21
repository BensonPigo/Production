using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B24 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public B24(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Name", header: "Inspection Levels", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("LotSize", header: "LotSize", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("SampleSize", header: "SampleSize", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
select 
	a.InspectionLevels,
	d.Name,
	LotSize = Concat (Format(LotSize_Start, '#,0'), ' to ', Format(LotSize_End, '#,0')),
	a.SampleSize
from AcceptableQualityLevels a
left join DropDownList d on d.ID = a.InspectionLevels and d.Type = 'PMS_QA_AQL'
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
