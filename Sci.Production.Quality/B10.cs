using Sci.Data;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B10 : Win.Tems.Input1
    {
        private readonly Hashtable ht = new Hashtable();
        private readonly Hashtable SkewnessHt = new Hashtable();

        /// <inheritdoc/>
        public B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.ht.Add("Formula1", "(Total Points / Act. Yds Inspected ) x 100");
            this.ht.Add("Formula2", "(Total Points × 3600) ÷ (Act. Yds Inspected × Actual Width)");

            // HashTabe add key,Value
            this.SkewnessHt.Add("Formula1", "100 × [ 2 × ( AC - BD ) / ( AC + BD ) ]");
            this.SkewnessHt.Add("Formula2", "100 × [ ( AA’ + DD’ ) / ( AB + CD ) ]");
            this.SkewnessHt.Add("Formula3", "100 * ( AA’ / AB )");

            // 抓取當下.exe執行位置路徑 同抓取Excle範本檔路徑
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Resources\");
            this.SkewnessHt.Add("Picture1", path + "QA_Skewness1.png");
            this.SkewnessHt.Add("Picture2", path + "QA_Skewness2.png");
            this.SkewnessHt.Add("Picture3", path + "QA_Skewness3.png");

            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("<Brand> cannot be empty! ");
                this.txtbrand.Focus();
                return false;
            }

            if (this.IsDetailInserting)
            {
                if (MyUtility.Check.Seek($"select * from QABrandSetting where brandid='{this.CurrentMaintain["BrandID"]}'"))
                {
                    MyUtility.Msg.WarningBox($"<Brand : {this.CurrentMaintain["BrandID"]}> existed, change other one please!");
                    this.txtbrand.Focus();
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickLocate()
        {
            base.ClickLocate();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            if (this.CurrentMaintain.Empty())
            {
                this.txtFormula.Text = string.Empty;
                return;
            }

            if (this.EditMode)
            {
                this.radioPanel1.ReadOnly = false;
            }
            else
            {
                this.radioPanel1.ReadOnly = true;
                this.txtbrand.ReadOnly = true;
            }

            if (MyUtility.Check.Seek($"select * from Brand_QAMoistureStandardList where brandid='{this.CurrentMaintain["BrandID"]}'"))
            {
                this.btnMoistureStandardList.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.btnMoistureStandardList.ForeColor = System.Drawing.Color.Black;
            }

            if (this.CurrentMaintain.Empty())
            {
                this.txtSkewnessFormula.Text = string.Empty;
                this.pictureBox1.ImageLocation = string.Empty;
                return;
            }

            this.txtSkewnessFormula.Text = this.CurrentMaintain["ID"].ToString() == "1" ? this.SkewnessHt["Formula1"].ToString() : this.SkewnessHt["Formula2"].ToString();
            this.pictureBox1.ImageLocation = this.CurrentMaintain["ID"].ToString() == "1" ? this.SkewnessHt["Picture1"].ToString() : this.SkewnessHt["Picture2"].ToString();

            if (this.EditMode)
            {
                this.radioPanel3.ReadOnly = false;
            }
            else
            {
                this.radioPanel3.ReadOnly = true;
            }

            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.txtbrand.ReadOnly = false;
            this.radioOption1.Checked = true;
            this.radioForWetDry.Checked = true;
            this.SkewnessOption1.Checked = true;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.txtbrand.ReadOnly = true;
            return base.ClickEditBefore();
        }

        private void RadioPanel1_ValueChanged(object sender, EventArgs e)
        {
            this.txtFormula.Text = this.radioPanel1.Value == "1" ? this.ht["Formula1"].ToString() : this.ht["Formula2"].ToString();
        }

        private void BtnMoistureStandardList_Click(object sender, EventArgs e)
        {
            var frm = new B10_MoistureStandardList(MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]));
            frm.ShowDialog();
        }

        private void RadioPanel3_ValueChanged(object sender, EventArgs e)
        {
            string formula = string.Empty;
            switch (this.radioPanel1.Value)
            {
                case "1":
                    formula = this.SkewnessHt["Formula1"].ToString();
                    this.pictureBox1.ImageLocation = this.SkewnessHt["Picture1"].ToString();
                    break;
                case "2":
                    formula = this.SkewnessHt["Formula2"].ToString();
                    this.pictureBox1.ImageLocation = this.SkewnessHt["Picture2"].ToString();
                    break;
                case "3":
                    formula = this.SkewnessHt["Formula3"].ToString();
                    this.pictureBox1.ImageLocation = this.SkewnessHt["Picture3"].ToString();
                    break;
                default:
                    break;
            }

            this.txtSkewnessFormula.Text = formula;
        }
    }
}
