using Sci.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B10 : Sci.Win.Tems.Input1
    {
        Hashtable ht = new Hashtable();
        public B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            ht.Add("Formula1", "(Total Points / Act. Yds Inspected ) x 100");
            ht.Add("Formula2", "(Total Points × 3600) ÷ (Act. Yds Inspected × Actual Width)");
            InitializeComponent();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtbrand.Text))
            {
                MyUtility.Msg.WarningBox("<Brand> cannot be empty! ");
                this.txtbrand.Focus();
                return false;
            }

            //DataRow dr;
            if (MyUtility.Check.Seek(string.Format("select * from PointRate where brandid='{0}'", this.txtbrand.Text)) && !this.txtbrand.ReadOnly)
            {
                MyUtility.Msg.WarningBox(string.Format("<Brand : {0}> existed, change other one please!", this.txtbrand.Text));
                this.txtbrand.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override Ict.DualResult ClickSave()
        {

            DataRow dr;
            if (MyUtility.Check.Seek(string.Format("select * from PointRate where brandid='{0}'", this.txtbrand.Text), out dr))
            {
                DBProxy.Current.Execute(null, string.Format(@"update PointRate set ID='{0}' where BrandID='{1}'", radioOption1.Checked ? "1" : "2", this.txtbrand.Text));
            }


            return base.ClickSave();

        }

        protected override void ClickLocate()
        {
            base.ClickLocate();
            OnDetailEntered();
        }

        protected override void OnDetailEntered()
        {
            if (CurrentMaintain.Empty())
            {
                this.txtFormula.Text = "";
                return;
            }
            txtFormula.Text = CurrentMaintain["ID"].ToString() == "1" ? ht["Formula1"].ToString() : ht["Formula2"].ToString();

            if (EditMode)
            {
                this.radioPanel1.ReadOnly = false;
            }
            else
            {
                this.radioPanel1.ReadOnly = true;
                this.txtbrand.ReadOnly = true;
            }

            base.OnDetailEntered();
        }

        protected override void ClickNewAfter()
        {
            this.txtbrand.ReadOnly = false;
            radioOption1.Checked = true;
            base.ClickNewAfter();
        }

        protected override bool ClickEditBefore()
        {
            this.txtbrand.ReadOnly = true;
            return base.ClickEditBefore();
        }

        private void radioPanel1_ValueChanged(object sender, EventArgs e)
        {
            txtFormula.Text = radioPanel1.Value == "1" ? ht["Formula1"].ToString() : ht["Formula2"].ToString();

        }
    }
}
