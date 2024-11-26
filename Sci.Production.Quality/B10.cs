using Sci.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            this.ht.Add("Formula3", "Knit = (Total Points × 3600) ÷ (Ticket Length × Cut. Width)\r\nWoven = (Total Points × 3600) ÷ (Act. Yds Inspected × Cut. Width)\r\n");
            this.ht.Add("Formula4", "Linear yard = (Total Points / Act. Yds Inspected ) x 100\r\nSquared yard = (Total Points*3600) / (Act. Yds Inspected * Cut. Width)");

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

            string sqlcmd = $@"
select 1
from FIR_PointRateFormula fp with(nolock)
left join Supp s with(nolock) on s.ID = fp.SuppID
where fp.brandid = '{this.CurrentMaintain["BrandID"]}'
";

            if (MyUtility.Check.Seek(sqlcmd))
            {
                this.btnSuppStandard.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.btnSuppStandard.ForeColor = System.Drawing.Color.Black;
            }

            if (this.CurrentMaintain.Empty())
            {
                this.txtSkewnessFormula.Text = string.Empty;
                this.pictureBox1.ImageLocation = string.Empty;
                return;
            }

            string skewnessOption = this.CurrentMaintain["SkewnessOption"].ToString();

            string formula = string.Empty;
            switch (skewnessOption)
            {
                case "1":
                    formula = this.SkewnessHt["Formula1"].ToString();
                    this.SkewnessOption1.Checked = true;
                    this.pictureBox1.ImageLocation = this.SkewnessHt["Picture1"].ToString();
                    break;
                case "2":
                    formula = this.SkewnessHt["Formula2"].ToString();
                    this.SkewnessOption2.Checked = true;
                    this.pictureBox1.ImageLocation = this.SkewnessHt["Picture2"].ToString();
                    break;
                case "3":
                    formula = this.SkewnessHt["Formula3"].ToString();
                    this.SkewnessOption3.Checked = true;
                    this.pictureBox1.ImageLocation = this.SkewnessHt["Picture3"].ToString();
                    break;
                default:
                    break;
            }

            this.txtSkewnessFormula.Text = formula;
            if (this.EditMode)
            {
                this.radioPanel3.ReadOnly = false;
            }
            else
            {
                this.radioPanel3.ReadOnly = true;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["brandid"]) == "LLL")
            {
                List<string> listGroup = new List<string>() { "A", "B" };
                List<string> listData = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    foreach (var item in listGroup)
                    {
                        string strSQL = $@"select Percentage from FIR_Grade
                                           where BrandID ='LLL' 
                                           and WeaveTypeID = 'KNIT' and Grade = '{item}' 
                                           and InspectionGroup ='{i + 1}'";
                        string strGrade = MyUtility.GetValue.Lookup(strSQL, "Production");
                        listData.Add(strGrade);
                    }
                }

                string strMsg = $@"X = Point Rate per 100 square yds

Group 1：X < {listData[0]} = Grade A; {listData[0]} <= X < {listData[1]} = Grade B; X > {listData[1]} = Grand C 
Group 2：X < {listData[2]} = Grade A; {listData[2]} <= X < {listData[3]} = Grade B; X > {listData[3]} = Grand C 
Group 3：X < {listData[4]} = Grade A; {listData[4]} <= X < {listData[5]} = Grade B; X > {listData[5]} = Grand C 
Group 4：X < {listData[6]} = Grade A; {listData[6]} <= X < {listData[7]} = Grade B; X > {listData[7]} = Grand C 
Group 5：TBC";
                this.txtFacbricGrade.Text = strMsg;
            }
            else if (MyUtility.Convert.GetString(this.CurrentMaintain["brandid"]) == "N.FACE")
            {
                List<string> listGroup = new List<string>() { "A", "B" };
                List<string> listWeaveType = new List<string> { "KNIT", "WOVEN" };
                List<string> listData = new List<string>();
                foreach (var head in listWeaveType)
                {
                    foreach (var item in listGroup)
                    {
                        string strSQL = $@"select Percentage from FIR_Grade
                                           where BrandID ='N.FACE' 
                                           and WeaveTypeID = '{head}' and Grade = '{item}' 
                                           and InspectionGroup =''";
                        string strGrade = MyUtility.GetValue.Lookup(strSQL, "Production");
                        listData.Add(strGrade);
                    }
                }
                string strMsg = $@"X = Point Rate per 100 square yds (Linear yard)
X = Point Rate per 100 square yds2 (Squared yard)
KNIT
X <= {listData[0]} = Grade A; {listData[0]} < X <= {listData[1]} = Grade B; X > {listData[1]} = Grand C 
WOVEN
X <= {listData[2]} = Grade A; {listData[2]} < X <= {listData[3]} = Grade B; X > {listData[3]} = Grand C

";
                this.txtFacbricGrade.Text = strMsg;
            }
            else if (MyUtility.Convert.GetString(this.CurrentMaintain["brandid"]).Trim().ToUpper() == "GYMSHARK")
            {
                List<string> listGroup = new List<string>() { "A", "B" };
                List<string> listData = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    foreach (var item in listGroup)
                    {
                        string strSQL = $@"select Percentage from FIR_Grade
                                           where BrandID ='GYMSHARK' 
                                           and WeaveTypeID = iif({i + 1} in (1, 2),'WOVEN','KNIT') and Grade = '{item}' 
                                           and InspectionGroup = '{i + 1}'";
                        string strGrade = MyUtility.GetValue.Lookup(strSQL, "Production");
                        listData.Add(strGrade);
                    }
                }

                string strMsg = $@"X = Point Rate per 100 square yds
WOVEN
Group 1：X <= {listData[0]} = Grade A; {listData[0]} < X <= {listData[1]} = Grade B; X > {listData[1]} = Grade C 
Group 2：X <= {listData[2]} = Grade A; {listData[2]} < X <= {listData[3]} = Grade B; X > {listData[3]} = Grade C
KNIT
Group 3：X <= {listData[4]} = Grade A; {listData[4]} < X <= {listData[5]} = Grade B; X > {listData[5]} = Grade C 
Group 4：X <= {listData[6]} = Grade A; {listData[6]} < X <= {listData[7]} = Grade B; X > {listData[7]} = Grade C
";
                this.txtFacbricGrade.Text = strMsg;
            }
            else
            {
                List<string> listGroup = new List<string>() { "A", "B" };
                List<string> listWeaveType = new List<string> { "KNIT", "WOVEN" };
                List<string> listData = new List<string>();

                var isBrandID = MyUtility.Check.Seek($"select Percentage from FIR_Grade where BrandID ='{MyUtility.Convert.GetString(this.CurrentMaintain["brandid"])}'", out DataRow dataRow, "Production");
                var strBrandID = isBrandID ? MyUtility.Convert.GetString(this.CurrentMaintain["brandid"]) : string.Empty;

                foreach (var head in listWeaveType)
                {
                    foreach (var item in listGroup)
                    {
                        string strSQL = $@"select Percentage from FIR_Grade
                                           where BrandID ='{strBrandID}'
                                           and WeaveTypeID = '{head}' 
                                           and Grade = '{item}'
                                           and InspectionGroup  = ''
";
                        string strGrade = MyUtility.GetValue.Lookup(strSQL, "Production");
                        listData.Add(strGrade);
                    }
                }

                string strMsg = $@"X = Point Rate per 100 square yds
KINT
X < {listData[0]} = Grade A; {listData[0]} <= X < {listData[1]} = Grade B; X > {listData[1]} = Grand C 
WOVEN
X < {listData[2]} = Grade A; {listData[2]}<= X < {listData[3]} = Grade B; X > {listData[3]} = Grand C";
                this.txtFacbricGrade.Text = strMsg;
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
            string strFormula = string.Empty;
            switch (this.radioPanel1.Value)
            {
                case "1":
                    strFormula = this.ht["Formula1"].ToString();
                    break;
                case "2":
                    strFormula = this.ht["Formula2"].ToString();
                    break;
                case "3":
                    strFormula = this.ht["Formula3"].ToString();
                    break;
                case "4":
                    strFormula = this.ht["Formula4"].ToString();
                    break;
                default:
                    break;
            }

            this.txtFormula.Text = strFormula;
        }

        private void BtnMoistureStandardList_Click(object sender, EventArgs e)
        {
            var frm = new B10_MoistureStandardList(MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]));
            frm.ShowDialog();
        }

        private void RadioPanel3_ValueChanged(object sender, EventArgs e)
        {
            string formula = string.Empty;
            switch (this.radioPanel3.Value)
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

        private void btnSuppStandard_Click(object sender, EventArgs e)
        {
            var frm = new B10_Supp_Standard(MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]));
            frm.ShowDialog();
        }
    }
}
