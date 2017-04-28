using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Planning
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");

            DataRow dr = dt.NewRow();
            dr[0] = "Laser";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Embroidery";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Bonding (Machine)";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Bonding (Hand)";
            dt.Rows.Add(dr);

            this.comboArtworkType.DataSource = dt;
            this.comboArtworkType.DisplayMember = "Value";
            this.comboArtworkType.ValueMember = "Value";
        }
        //新增資料預設
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["unit"] = 1;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }
        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                this.dateDate.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["artworktypeid"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type > can not be empty!");
                this.comboArtworkType.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["ftysupp"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Fty / Supp > can not be empty!");
                this.txtsubconFtySupp.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["heads"]) && CurrentMaintain["artworktypeid"].ToString() == "Embroidery")
            {
                MyUtility.Msg.WarningBox("< # of Heads > can not be empty!");
                this.numHeads.Focus();
                return false;
            }



            if (radiobyMonth.Checked == true)
            {
                int yy = DateTime.Parse(CurrentMaintain["issuedate"].ToString()).Year;
                int mm = DateTime.Parse(CurrentMaintain["issuedate"].ToString()).Month;
                CurrentMaintain["issuedate"] = DateTime.Parse(yy.ToString() + "/" + mm.ToString() + "/1");
            } 



            return base.ClickSaveBefore();
        }

        //combo下拉控制其它物件
        private void comboArtworkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == this.comboArtworkType.SelectedValue) return;
            switch(this.comboArtworkType.SelectedValue.ToString().TrimEnd())
            {
                case "Bonding (Hand)":
                    this.labelCapacity.Text = "Capacity (Person):";
                    this.label25.Text = "(Calculate by employee)";
                    break;
                case "Embroidery":
                    this.labelHeads.Visible = true;
                    this.numHeads.Visible = true;
                    break;
                default:
                    this.labelCapacity.Text = "Capacity (Unit)";
                    this.label25.Text = "(Calculate by machine)";
                    this.labelHeads.Visible = false;
                    this.numHeads.Visible = false;
                    break;

            }

        }
        
        //refresh
       protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            switch (CurrentMaintain["artworktypeid"].ToString().TrimEnd())
            {
                case "Bonding (Hand)":
                    this.labelCapacity.Text = "Capacity (Person):";
                    this.label25.Text = "(Calculate by employee)";
                    break;
                case "Embroidery":
                    this.labelHeads.Visible = true;
                    this.numHeads.Visible = true;
                    break;
                default:
                    this.labelCapacity.Text = "Capacity (Unit)";
                    this.label25.Text = "(Calculate by machine)";
                    this.labelHeads.Visible = false;
                    this.numHeads.Visible = false;
                    break;

            }
        }

       private void dateDate_Validated(object sender, EventArgs e)
        {
            if (CurrentMaintain["unit"].ToString() == "2")
            {
                int yy = DateTime.Parse(CurrentMaintain["issuedate"].ToString()).Year;
                int mm = DateTime.Parse(CurrentMaintain["issuedate"].ToString()).Month;
                CurrentMaintain["issuedate"] = DateTime.Parse(yy.ToString()+"/"+mm.ToString()+"/1");
            }
        }

        private void radioPanel1_Validated(object sender, EventArgs e)
        {
            Sci.Win.UI.RadioPanel rdoG = (RadioPanel)sender;
            if (rdoG.Value == "2" && !MyUtility.Check.Empty(CurrentMaintain["issuedate"]))
            {
                int yy = DateTime.Parse(CurrentMaintain["issuedate"].ToString()).Year;
                int mm = DateTime.Parse(CurrentMaintain["issuedate"].ToString()).Month;
                CurrentMaintain["issuedate"] = DateTime.Parse(yy.ToString() + "/" + mm.ToString() + "/1");
            } 
          
        }


        private void radiobyMonth_CheckedChanged(object sender, EventArgs e)
        {

        }



    }
}
