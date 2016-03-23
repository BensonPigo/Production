using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P02_BatchAssignCellCutDate : Sci.Win.Subs.Base
    {
        private DataTable curTb;
        private DataTable detailTb;
        public P02_BatchAssignCellCutDate(DataTable cursor)
        {
            InitializeComponent();
            txtcell.FactoryId = Sci.Env.User.Factory;
            txtCell2.FactoryId = Sci.Env.User.Factory;
            detailTb = cursor;
            curTb = cursor.Copy();
            curTb.Columns.Add("Sel", typeof(bool));

            gridsetup();
        }
        private void gridsetup()
        {
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示

            Helper.Controls.Grid.Generator(this.grid1)
             .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
             .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
             .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5), iseditingreadonly: true)
             .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("Cutcellid", header: "Cell", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
             .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
             .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
             .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.grid1.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[6].DefaultCellStyle.ForeColor = Color.Red;
            this.grid1.Columns[16].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[16].DefaultCellStyle.ForeColor = Color.Red;
        }

        private void filter_button_Click(object sender, EventArgs e)
        {
            string sp = SP_textbox.Text;
            string article = article_textbox.Text;
            string markername = markername_textbox.Text;
            string sizecode = sizecode_textbox.Text;
            string cutcell = txtcell.Text;
            string fabriccombo = fabriccombo_textbox.Text;
            string estcutdate = estcutdate_textbox1.Text.ToString();
            string filter="(cutref is null or cutref = '') and (cutplanid is null or cutplanid = '') ";
            if (!MyUtility.Check.Empty(sp)) filter = filter + string.Format(" and OrderID ='{0}'", sp);
            if (!MyUtility.Check.Empty(article)) filter = filter + string.Format(" and article like '%{0}%'", article);
            if (!MyUtility.Check.Empty(markername)) filter = filter + string.Format(" and markername ='{0}'", markername);
            if (!MyUtility.Check.Empty(sizecode)) filter = filter + string.Format(" and sizecode like '%{0}%'", sizecode);
            if (!MyUtility.Check.Empty(cutcell)) filter = filter + string.Format(" and cutcellid ='{0}'", cutcell);
            if (!MyUtility.Check.Empty(fabriccombo)) filter = filter + string.Format(" and fabriccombo ='{0}'", fabriccombo);
            if (!MyUtility.Check.Empty(cutno_numericbox.Value)) filter = filter + string.Format(" and cutno ={0}", cutno_numericbox.Value);
            if (!MyUtility.Check.Empty(estcutdate_textbox1.Value)) filter = filter + string.Format(" and estcutdate ='{0}'", estcutdate);
            if (only_checkBox.Value == "True") filter = filter + " and estcutdate is null ";
            curTb.DefaultView.RowFilter=filter;
            grid1.DataSource = curTb;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void batchcutcell_button_Click(object sender, EventArgs e)
        {
            string cell = txtCell2.Text;
            foreach (DataRow dr in curTb.Rows)
            {
                if(dr["Sel"].ToString()=="True")
                {
                    DataRow[] detaildr = detailTb.Select(string.Format("Ukey = '{0}'",dr["Ukey"]));
                    detaildr[0]["Cutcellid"] = cell;
                    dr["Cutcellid"] = cell;
                }
            }

        }

        private void batchestcutdate_button_Click(object sender, EventArgs e)
        {
            string cdate =""; 
                if(!MyUtility.Check.Empty(estcutdate_textbox2.Value))
                {
                    cdate = estcutdate_textbox2.Text;
                };
            foreach (DataRow dr in curTb.Rows)
            {
                if (dr["Sel"].ToString() == "True")
                {
                    DataRow[] detaildr = detailTb.Select(string.Format("Ukey = '{0}'", dr["Ukey"]));
                    if (cdate != "")
                    {
                        detaildr[0]["estcutdate"] = cdate;
                        dr["estcutdate"] = cdate;
                    }
                    else
                    {
                        detaildr[0]["estcutdate"] = DBNull.Value;
                        dr["estcutdate"] = DBNull.Value;
                    }
                }
            }
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
