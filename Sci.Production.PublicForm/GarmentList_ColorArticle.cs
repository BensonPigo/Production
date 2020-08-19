using Ict;
using Ict.Win;
using System;
using System.Data;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class GarmentList_ColorArticle : Win.Subs.Base
    {
        private string styleukey;
        private string sukey;
        private string cid;
        private string FC;

        /// <summary>
        /// Initializes a new instance of the <see cref="GarmentList_ColorArticle"/> class.
        /// </summary>
        /// <param name="ukey">ukey</param>
        /// <param name="styukey">styukey</param>
        /// <param name="id">id</param>
        /// <param name="f">F</param>
        public GarmentList_ColorArticle(string ukey, string styukey, string id, string f)
        {
            this.InitializeComponent();
            this.sukey = ukey;
            this.cid = id;
            this.FC = f;
            this.styleukey = styukey;
            this.Requery();
            this.GridSetup();
        }

        private void Requery()
        {
            string sqlcmd = string.Format(
                @"
Select *
from Pattern_GL_Article a WITH (NOLOCK),(SELECT DISTINCT PatternPanel FROM Order_ColorCombo WHERE Id='{1}' and FabricPanelCode='{2}')b
            Where a.PatternUkey = '{0}'",
                this.sukey,
                this.cid,
                this.FC);
            DataTable gridtb;
            DualResult sqldr = DBProxy.Current.Select(null, sqlcmd, out gridtb);
            if (!sqldr)
            {
                this.ShowErr(sqlcmd, sqldr);
            }

            this.gridColorArticle.DataSource = gridtb;
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridColorArticle)
                .Text("ArticleGroup", header: "Article Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeRange", header: "Size Range", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("PatternPanel", header: "Pattern Panel Combo", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
