using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections;       // file使用Hashtable時，必須引入這個命名空間

namespace Sci.Production.Quality
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        Hashtable ht = new Hashtable();

        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            // HashTabe add key,Value
            this.ht.Add("Formula1", "100 × [ 2 × ( AC - BD ) / ( AC + BD ) ]");
            this.ht.Add("Formula2", "100 × [ ( AA’ + DD’ ) / ( AB + CD ) ]");

            // 抓取當下.exe執行位置路徑 同抓取Excle範本檔路徑
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Resources\");
            this.ht.Add("Picture1", path + "QA_Skewness1.png");
            this.ht.Add("Picture2", path + "QA_Skewness2.png");

            this.InitializeComponent();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtbrand.Text))
            {
               MyUtility.Msg.WarningBox("<Brand> cannot be empty! ");
               this.txtbrand.Focus();
               return false;
            }

            // DataRow dr;
            if (MyUtility.Check.Seek(string.Format("select * from SkewnessOption where brandid='{0}'", this.txtbrand.Text)) && !this.txtbrand.ReadOnly)
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
            if (MyUtility.Check.Seek(string.Format("select * from SkewnessOption where brandid='{0}'", this.txtbrand.Text), out dr))
            {
                DBProxy.Current.Execute(null, string.Format(@"update SkewnessOption set ID='{0}' where BrandID='{1}'", this.radioOption1.Checked ? "1" : "2", this.txtbrand.Text));
            }

            return base.ClickSave();
        }

        protected override void ClickLocate()
        {
            base.ClickLocate();
            this.OnDetailEntered();
        }

        protected override void OnDetailEntered()
        {
            if (this.CurrentMaintain.Empty())
            {
                this.txtFormula.Text = string.Empty;
                this.pictureBox1.ImageLocation = string.Empty;
                return;
            }

            this.txtFormula.Text = this.CurrentMaintain["ID"].ToString() == "1" ? this.ht["Formula1"].ToString() : this.ht["Formula2"].ToString();
            this.pictureBox1.ImageLocation = this.CurrentMaintain["ID"].ToString() == "1" ? this.ht["Picture1"].ToString() : this.ht["Picture2"].ToString();

            if (this.EditMode)
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
            this.radioOption1.Checked = true;
            base.ClickNewAfter();
        }

        protected override bool ClickEditBefore()
        {
            this.txtbrand.ReadOnly = true;
            return base.ClickEditBefore();
        }

        private void radioOption1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioOption1.Checked)
            {
                this.txtFormula.Text = this.ht["Formula1"].ToString();
                this.pictureBox1.ImageLocation = this.ht["Picture1"].ToString();
            }
            else
            {
                this.txtFormula.Text = this.ht["Formula2"].ToString();
                this.pictureBox1.ImageLocation = this.ht["Picture2"].ToString();
            }
        }
    }
}
