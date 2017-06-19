using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            radioOption1.Checked = true;

        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtbrand.Text))
            {
               MyUtility.Msg.WarningBox("<Brand> cannot be empty! ");
               this.txtbrand.Focus();
               return false;
            }
            return base.ClickSaveBefore();
        }
        protected override Ict.DualResult ClickSave()
        {
            
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format("select * from SkewnessOption where brandid='{0}'", this.txtbrand.Text), out dr))
            {
                DBProxy.Current.Execute(null, string.Format(@"update SkewnessOption set OptionID='{0}' where BrandID='{1}'", radioOption1.Checked ? "1" : "2", this.txtbrand.Text));
            }
            //else
            //{
            //    DBProxy.Current.Execute(null, string.Format(@"insert into SkewnessOption (Brandid,OptionID,Junk,AddName,AddDate) values('{0}','{1}','{2}','{3}','{4}') ", this.txtbrand.Text, radioOption1.Checked ? "1" : "2",this.checkJunk.Checked,Sci.Env.User,DateTime.Now));
            //}

            return base.ClickSave();
            
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //按鈕Shipping Mark變色
            if (MyUtility.Convert.GetString(CurrentMaintain["OptionID"]) == "1")
            {
                radioOption1.Checked = true;
                txtFormula.Text = "100 × [ 2 × ( AC - BD ) / ( AC + BD ) ]";
                pictureBox1.ImageLocation = @".\Resources\QA_Skewness1.png";
            }
            else if (MyUtility.Convert.GetString(CurrentMaintain["OptionID"]) == "2")
            {
                radioOption2.Checked = true;
                txtFormula.Text = "100 × [ ( AA’ + DD’ ) / ( AB + CD ) ]";
                pictureBox1.ImageLocation = @".\Resources\QA_Skewness2.png";
            }
            if (EditMode)
            {
                this.radioOption1.ReadOnly = false;
            }
            else
            {
                this.radioOption1.ReadOnly = true;
                this.txtbrand.ReadOnly = true;
            }    

        }
        protected override bool ClickNewBefore()
        {
           // CurrentMaintain["OptionID"] = radioOption1.Checked ? "1" : "2";
            return base.ClickNewBefore();
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
        private void radioOption1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOption1.Checked)
            {
                txtFormula.Text = "100 × [ 2 × ( AC - BD ) / ( AC + BD ) ]";
                pictureBox1.ImageLocation = @".\Resources\QA_Skewness1.png";
            }
            else
            {
                txtFormula.Text = "100 × [ ( AA’ + DD’ ) / ( AB + CD ) ]";
                pictureBox1.ImageLocation = @".\Resources\QA_Skewness2.png";
            }
        }
    }
}
