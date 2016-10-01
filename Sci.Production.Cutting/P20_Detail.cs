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
    public partial class P20_Detail : Sci.Win.Subs.Input8A
    {

        public P20_Detail()
        {
            
            InitializeComponent();
            
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            viewText();
        }
        private void viewText()
        {
            editBox1.Text = CurrentDetailData["OrderID"].ToString();
            displayBox1.Text = CurrentDetailData["Cutref"].ToString();
            displayBox2.Text = CurrentDetailData["cutno"].ToString();
            displayBox3.Text = CurrentDetailData["colorid"].ToString();
            displayBox4.Text = CurrentDetailData["cons"].ToString();
            displayBox5.Text = CurrentDetailData["MarkerName"].ToString();
            displayBox6.Text = CurrentDetailData["MarkerLength"].ToString();
            displayBox7.Text = CurrentDetailData["FabricCombo"].ToString();
            displayBox8.Text = CurrentDetailData["PatternPanel"].ToString();
        }
        protected override void ToNext()
        {
            base.ToNext();
            viewText();

        }
        protected override void ToPrev()
        {
            base.ToPrev();
            viewText();
        }

        protected override bool OnGridSetup()
        {
            #region set grid
            //this.grid.ReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10),iseditingreadonly: true)
            .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(10), integer_places: 6,iseditingreadonly: true);
            #endregion
            return true;
        }

        private void P20_Detail_Load(object sender, EventArgs e)
        {
            viewText();
        }

    }
}
