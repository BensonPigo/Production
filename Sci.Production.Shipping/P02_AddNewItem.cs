using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

namespace Sci.Production.Shipping
{
    public partial class P02_AddNewItem : Sci.Win.Subs.Input2A
    {

        public P02_AddNewItem()
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "5,Dox,6,Machine/Parts,7,Mock Up,8,Other Sample,9,Other Material");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (OperationMode != 2)
            {
                textBox1.ReadOnly = true;
                textBox1.IsSupportEditMode = false;
                editBox1.ReadOnly = true;
                editBox1.IsSupportEditMode = false;
            }
            GetLeaderName();
        }

        //SP#
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && textBox1.OldValue != textBox1.Text)
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from Orders where ID = '{0}'", textBox1.Text)))
                {
                    MyUtility.Msg.WarningBox("SP# not found!!");
                    CurrentData["OrderID"] = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //SP#
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (EditMode && textBox1.OldValue != textBox1.Text)
            {
                DataRow OrderData;
                if (MyUtility.Check.Seek(string.Format(@"select SeasonID,StyleID,BrandID,SMR,[dbo].[getBOFMtlDesc](StyleUkey) as Description
from Orders where ID = '{0}'", textBox1.Text), out OrderData))
                {
                    CurrentData["OrderID"] = textBox1.Text;
                    CurrentData["SeasonID"] = OrderData["SeasonID"].ToString();
                    CurrentData["StyleID"] = OrderData["StyleID"].ToString();
                    CurrentData["BrandID"] = OrderData["BrandID"].ToString();
                    CurrentData["Leader"] = OrderData["SMR"].ToString();
                    if (MyUtility.Check.Empty(CurrentData["Description"]))
                    {
                        CurrentData["Description"] = OrderData["Description"].ToString();
                    }
                }
                else
                {
                    CurrentData["OrderID"] = textBox1.Text;
                    CurrentData["SeasonID"] = "";
                    CurrentData["StyleID"] = "";
                    CurrentData["BrandID"] = "";
                    CurrentData["Leader"] = "";
                }
            }
        }

        private void GetLeaderAndDesc()
        {
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format(@"select s.BulkSMR,[dbo].[getBOFMtlDesc](s.Ukey) as Description
from Style s where s.ID = '{0}' and s.SeasonID = '{1}'",txtstyle1.Text,txtseason1.Text),out dr))
            {
                CurrentData["Leader"] = dr["BulkSMR"].ToString();
                CurrentData["Description"] = dr["Description"].ToString();
            }
        }

        //Season
        private void txtseason1_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtseason1.OldValue != txtseason1.Text && !MyUtility.Check.Empty(txtseason1.Text) && !MyUtility.Check.Empty(txtstyle1.Text))
            {
                GetLeaderAndDesc();
            }
        }

        //Style
        private void txtstyle1_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtstyle1.OldValue != txtstyle1.Text && !MyUtility.Check.Empty(txtstyle1.Text) && !MyUtility.Check.Empty(txtseason1.Text))
            {
                GetLeaderAndDesc();
            }
        }

        //CTN No.
        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (EditMode && textBox2.OldValue != textBox2.Text)
            {
                CurrentData["CTNNo"] = textBox2.Text.Trim();
            }
        }

        protected override bool OnSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentData["Description"]))
            {
                MyUtility.Msg.WarningBox("Description can't empty!");
                editBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["CTNNo"]))
            {
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                textBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["NW"]))
            {
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                numericBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Category"]))
            {
                MyUtility.Msg.WarningBox("Category can't empty!");
                comboBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Receiver"]))
            {
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                textBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Leader"]))
            {
                MyUtility.Msg.WarningBox("Team Leader can't empty!");
                textBox4.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!");
                txtbrand1.Focus();
                return false;
            }

            if (CurrentData["Category"].ToString() == "7")
            {
                if (MyUtility.Check.Empty(CurrentData["StyleID"]))
                {
                    MyUtility.Msg.WarningBox("Style can't empty!");
                    txtstyle1.Focus();
                    return false;
                }
            }
            #endregion

            //新增帶值
            if (OperationMode == 2)
            {
                DataRow Seq;
                if (!MyUtility.Check.Seek(string.Format(@"select RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3) as Seq1
from Express_Detail where ID = '{0}' and Seq2 = ''", CurrentData["ID"].ToString()), out Seq))
                {
                    MyUtility.Msg.WarningBox("Get seq fail, pls try again");
                    return false;
                }
                CurrentData["Seq1"] = Seq["Seq1"].ToString();
                CurrentData["InCharge"] = Sci.Env.User.UserID;
            }

            return true;
        }

        protected override DualResult OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(CurrentData["ID"].ToString()));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        protected override bool OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(CurrentData["ID"].ToString()));
            if (!result)
            {
                MyUtility.Msg.WarningBox("Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return false;
            }
            return true;
        }

        //Team Leader
        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1 order by ID", "15,30,10,150", textBox4.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox4.Text = item.GetSelectedString();
        }

        //Team Leader
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && textBox4.OldValue != textBox4.Text)
            {
                if (!MyUtility.Check.Seek(textBox4.Text, "TPEPass1", "ID"))
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    CurrentData["Leader"] = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //Team Leader
        private void textBox4_Validated(object sender, EventArgs e)
        {
            if (EditMode && textBox4.OldValue != textBox4.Text)
            {
                GetLeaderName();
            }
        }

        private void GetLeaderName()
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 Where id='{0}'", CurrentData["Leader"].ToString());
            DataRow dr;
            if (MyUtility.Check.Seek(selectSql, out dr))
            {
                displayBox2.Text = MyUtility.Check.Empty(dr["extNo"]) ? "" : dr["Name"].ToString();
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    displayBox2.Text = this.displayBox2.Text + " #" + dr["extNo"].ToString();
                }
            }
            else
            {
                displayBox2.Text = "";
            }
        }
    }
}
