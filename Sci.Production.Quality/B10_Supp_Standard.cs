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

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B10_Supp_Standard : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public B10_Supp_Standard(string brandID)
        {
            this.BrandID = brandID;
            this.InitializeComponent();
            this.Text = $"QA B10. Supp Standard List ({this.BrandID})";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Supp", header: "Supp", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("WeaveTypeID", header: "Weave Type", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("Formula", header: "Formula", width: Widths.AnsiChars(50), iseditingreadonly: true)
            ;
        }

        private void Query()
        {
            string sqlcmd = $@"
select Supp = fp.SuppID + '-' + s.AbbEN
,fp.* 
from FIR_PointRateFormula fp with(nolock)
left join Supp s with(nolock) on s.ID = fp.SuppID
where fp.brandid = '{this.BrandID}'
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }
    }
}
