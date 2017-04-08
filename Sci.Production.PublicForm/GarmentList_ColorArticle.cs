using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    
    public partial class GarmentList_ColorArticle : Sci.Win.Subs.Base
    {
        private string styleukey;
        private string sukey;
        private string cid;
        private string FC;
        public GarmentList_ColorArticle(string ukey,string styukey,string id,string F)
        {
            InitializeComponent();
            sukey = ukey;
            cid = id;
            FC = F;
            styleukey = styukey;
            requery();
            gridSetup();

        }
        private void requery()
        {
            string sqlcmd = String.Format(@"
Select *
from Pattern_GL_Article a WITH (NOLOCK),(SELECT DISTINCT PatternPanel FROM Order_ColorCombo WHERE Id='{1}' and FabricPanelCode='{2}')b
            Where a.PatternUkey = '{0}'", sukey,cid,FC);
            DataTable gridtb;
            DualResult sqldr = DBProxy.Current.Select(null, sqlcmd, out gridtb);
            if(!sqldr)
            {
                ShowErr(sqlcmd,sqldr);
            }
            grid1.DataSource = gridtb;
        }
        private void gridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("ArticleGroup", header: "Article Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeRange", header: "Size Range", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("PatternPanel", header: "Pattern Panel Combo", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
