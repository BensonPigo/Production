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
        private string cutid;
        public GarmentList_ColorArticle(string cID,string styukey)
        {
            InitializeComponent();
            cutid = cID;
            styleukey = styukey;
            requery();
            gridSetup();

        }
        private void requery()
        {
            string sqlcmd = String.Format(
            @"Select a.*
            from Pattern_GL_Article a
            Where a.PatternUkey = '{0}'",cutid);
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
                .Text("Remark", header: "Comb", width: Widths.AnsiChars(20), iseditingreadonly: true);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
