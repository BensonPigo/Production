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
            MyUtility.Tool.SetupCombox(comboCategory, 2, 1, "5,Dox,6,Machine/Parts,7,Mock Up,8,Other Sample,9,Other Material");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (OperationMode != 2)
            {
                txtSPNo.ReadOnly = true;
                txtSPNo.IsSupportEditMode = false;
                editDescription.ReadOnly = true;
                editDescription.IsSupportEditMode = false;
            }
            GetLeaderName();
        }

        //SP#
        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtSPNo.OldValue != txtSPNo.Text)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", txtSPNo.Text);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);

                string sqlCmd = "select Orders.ID from Orders WITH (NOLOCK) ,factory WITH (NOLOCK) where Orders.ID = @id and Orders.FactoryID = Factory.ID and Factory.IsProduceFty = 1";
                DataTable OrderData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrderData);

                if (!result && OrderData.Rows.Count <= 0)
                {
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("SP# not found!!");
                    }
                    CurrentData["OrderID"] = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //SP#
        private void txtSPNo_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtSPNo.OldValue != txtSPNo.Text)
            {
                DataRow OrderData;
                if (MyUtility.Check.Seek(string.Format(@"select SeasonID,StyleID,BrandID,SMR,[dbo].[getBOFMtlDesc](StyleUkey) as Description
from Orders WITH (NOLOCK) where ID = '{0}'", txtSPNo.Text), out OrderData))
                {
                    CurrentData["OrderID"] = txtSPNo.Text;
                    CurrentData["SeasonID"] = OrderData["SeasonID"];
                    CurrentData["StyleID"] = OrderData["StyleID"];
                    CurrentData["BrandID"] = OrderData["BrandID"];
                    CurrentData["Leader"] = OrderData["SMR"];
                    if (MyUtility.Check.Empty(CurrentData["Description"]))
                    {
                        CurrentData["Description"] = OrderData["Description"];
                    }
                }
                else
                {
                    CurrentData["OrderID"] = txtSPNo.Text;
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
from Style s WITH (NOLOCK) where s.ID = '{0}' and s.SeasonID = '{1}'", txtstyle.Text, txtseason.Text), out dr))
            {
                CurrentData["Leader"] = dr["BulkSMR"];
                CurrentData["Description"] = dr["Description"];
            }
        }

        //Season
        private void txtseason_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtseason.OldValue != txtseason.Text && !MyUtility.Check.Empty(txtseason.Text) && !MyUtility.Check.Empty(txtstyle.Text))
            {
                GetLeaderAndDesc();
            }
        }

        //Style
        private void txtstyle_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtstyle.OldValue != txtstyle.Text && !MyUtility.Check.Empty(txtstyle.Text) && !MyUtility.Check.Empty(txtseason.Text))
            {
                GetLeaderAndDesc();
            }
        }

        //CTN No.
        private void txtCTNNo_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtCTNNo.OldValue != txtCTNNo.Text)
            {
                CurrentData["CTNNo"] = txtCTNNo.Text.Trim();
            }
        }

        protected override bool OnSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentData["Description"]))
            {
                editDescription.Focus();
                MyUtility.Msg.WarningBox("Description can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["CTNNo"]))
            {
                txtCTNNo.Focus();
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["NW"]))
            {
                numNW.Focus();
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Category"]))
            {
                comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Receiver"]))
            {
                txtReceiver.Focus();
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Leader"]))
            {
                txtTeamLeader.Focus();
                MyUtility.Msg.WarningBox("Team Leader can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["BrandID"]))
            {
                txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!");
                return false;
            }

            if (MyUtility.Convert.GetString(CurrentData["Category"]) == "7")
            {
                if (MyUtility.Check.Empty(CurrentData["StyleID"]))
                {
                    txtstyle.Focus();
                    MyUtility.Msg.WarningBox("Style can't empty!");
                    return false;
                }
            }
            #endregion

            //新增帶值
            if (OperationMode == 2)
            {
                DataRow Seq;
                if (!MyUtility.Check.Seek(string.Format(@"select RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3) as Seq1
from Express_Detail WITH (NOLOCK) where ID = '{0}' and Seq2 = ''", MyUtility.Convert.GetString(CurrentData["ID"])), out Seq))
                {
                    MyUtility.Msg.WarningBox("Get seq fail, pls try again");
                    return false;
                }
                CurrentData["Seq1"] = Seq["Seq1"];
                CurrentData["InCharge"] = Sci.Env.User.UserID;
            }

            return true;
        }

        protected override DualResult OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        protected override DualResult OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        //Team Leader
        private void txtTeamLeader_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1 WITH (NOLOCK) order by ID", "15,30,10,150", txtTeamLeader.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtTeamLeader.Text = item.GetSelectedString();
        }

        //Team Leader
        private void txtTeamLeader_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtTeamLeader.OldValue != txtTeamLeader.Text)
            {
                if (!MyUtility.Check.Seek(txtTeamLeader.Text, "TPEPass1", "ID"))
                {
                    CurrentData["Leader"] = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Data not found!!");
                    return;
                }
            }
        }

        //Team Leader
        private void txtTeamLeader_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtTeamLeader.OldValue != txtTeamLeader.Text)
            {
                GetLeaderName();
            }
        }

        private void GetLeaderName()
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 WITH (NOLOCK) Where id='{0}'", MyUtility.Convert.GetString(CurrentData["Leader"]));
            DataRow dr;
            if (MyUtility.Check.Seek(selectSql, out dr))
            {
                displayTeamLeader.Text = MyUtility.Convert.GetString(dr["Name"]);
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    displayTeamLeader.Text = this.displayTeamLeader.Text + " #" + MyUtility.Convert.GetString(dr["extNo"]);
                }
            }
            else
            {
                displayTeamLeader.Text = "";
            }
        }
    }
}
