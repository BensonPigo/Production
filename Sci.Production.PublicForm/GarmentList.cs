using Ict;
using Ict.Win;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class GarmentList : Win.Subs.Base
    {
        private readonly string styleyukey;
        private readonly string id;
        private readonly string patternukey;
        private readonly DataTable articleGroupDT;

        /// <summary>
        /// Initializes a new instance of the <see cref="GarmentList"/> class.
        /// </summary>
        /// <param name="ukey">ukey</param>
        /// <param name="cid">cid</param>
        /// <param name="cutref">cutref</param>
        /// <param name="sizeList">sizeList</param>
        public GarmentList(string ukey, string cid, string cutref, List<string> sizeList = null)
        {
            this.InitializeComponent();
            this.styleyukey = ukey;
            this.id = cid;
            string sizes = "''";
            if (sizeList != null)
            {
                sizes = "'" + string.Join("','", sizeList) + "'";
            }

            this.patternukey = Prgs.GetPatternUkey(cutref, cid, sizes);
            Prgs.GetGarmentListTable(cutref, cid, sizes, out DataTable garmentTb, out this.articleGroupDT);
            this.gridGarment.DataSource = garmentTb;
            this.GridSetup();
            this.gridGarment.AutoResizeColumns();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridGarment)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("nLocation", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternCode", header: "Cutpart ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Artwork", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .CheckBox("Main", header: "Main", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: false)
                .Text("Alone", header: "Alone", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Pair", header: "Pair", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("DV", header: "DV", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("F_CODE", header: "F_Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Remarks", header: "Remarks", width: Widths.AnsiChars(15), iseditingreadonly: true);
            foreach (DataRow dr in this.articleGroupDT.Rows)
            {
                string header = dr["ArticleGroup"].ToString().Trim();
                this.Helper.Controls.Grid.Generator(this.gridGarment)
                 .Text(header, header: header, width: Widths.AnsiChars(8), iseditingreadonly: true);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnArticleForFCode_Click(object sender, EventArgs e)
        {
            int idx = this.gridGarment.GetSelectedRowIndex();

            GarmentList_ColorArticle callNextForm =
new GarmentList_ColorArticle(this.patternukey, this.styleyukey, this.id, ((DataTable)this.gridGarment.DataSource).Rows[idx][11].ToString());
            callNextForm.ShowDialog(this);
        }
    }
}
