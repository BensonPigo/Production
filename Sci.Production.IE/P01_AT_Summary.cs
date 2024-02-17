using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P01_AT_Summary : Win.Subs.Base
    {
        private string IETMSUKEY;
        private bool IsEdit;
        private string strTimeStudyID = string.Empty;
        private DataTable dtDetail;

        /// <inheritdoc/>
        public P01_AT_Summary(string iETMSUkey, ref DataTable dataTable, bool isEdit = false, string strTimeStudy = "")
        {
            this.InitializeComponent();
            this.IETMSUKEY = iETMSUkey;
            this.IsEdit = isEdit;
            this.strTimeStudyID = strTimeStudy;
            this.dtDetail = dataTable;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridIETMS)
            .Text("Draft", header: "Feature Code", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Component", header: "Component", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ShowATGroup", header: "AT Group", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridAT)
            .Text("Component", header: "Component", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("ATGroup", header: "AT Group", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Button("detail", header: "FormulaDetail", onclick: this.Detail_Click);
            this.GetDataSource();
        }

        private void GetDataSource()
        {
            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter("@IETMSUkey", this.IETMSUKEY));
            string sqlcmd_IETMS = @"
            select id.CodeFrom, id.Draft, Component = Reason.Name, id.Pattern_GL_ArtworkUkey, ShowATGroup = isnull(ShowATGroup, '')
            from IETMS_Detail id
            left join Reason on ReasonTypeID = 'IE_Component' and Reason.ID = substring(id.Draft, 6, 2)
            Outer apply (
	            Select ShowATGroup = STUFF((
		            select ',' + gla.Annotation
		            from Pattern_GL_Artwork gla
		            where gla.UKEY in (select data from dbo.SplitString(id.Pattern_GL_ArtworkUkey, ','))
                    order by gla.Annotation
		            for xml path ('')), 1, 1, '' )
            ) getATGroup
            where id.IETMSUkey = @IETMSUkey
            and id.CodeFrom like 'Feature%'
            and id.Draft like '___AT%'
            group by id.CodeFrom, id.Draft, Reason.Name, id.Pattern_GL_ArtworkUkey, ShowATGroup";
            DualResult dualResult = DBProxy.Current.Select(string.Empty, sqlcmd_IETMS, para, out DataTable dt_IETMS);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.gridIETMS.DataSource = dt_IETMS;

            string sqlcmd_AT = @"
            select at.Component
            , getATGroup.ATGroup
            , at.IETMSUkey
            , at.CodeFrom
            from IETMS_AT at
            Outer apply (
	            Select ATGroup = STUFF((
		            select ',' + gla.Annotation
		            from Pattern_GL_Artwork gla
		            where gla.UKEY in (select data from dbo.SplitString(at.Pattern_GL_ArtworkUkey, ','))
		            for xml path ('')), 1, 1, '' )
            ) getATGroup
            where at.IETMSUkey = @IETMSUkey";
            DualResult dualResult_AT = DBProxy.Current.Select(string.Empty, sqlcmd_AT, para, out DataTable dt_AT);
            if (!dualResult_AT)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.gridAT.DataSource = dt_AT;
        }

        private void Detail_Click(object sender, DataGridViewCellEventArgs e)
        {
            var row = this.gridAT.GetCurrentDataRow();
            if (row == null)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            var frm = new P01_Operation_AT(new string[] { "AT" }, ref this.dtDetail, row["IETMSUkey"].ToString(), row["CodeFrom"].ToString(), this.IsEdit, strTimeStudyID: this.strTimeStudyID);
            frm.ShowDialog();
        }
    }
}
