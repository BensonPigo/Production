using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Sci.Production.Class;
using Sci.Andy.ExtensionMethods;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_AddNewItem
    /// </summary>
    public partial class P02_AddNewItem : Win.Subs.Input2A
    {
        private DataRow P02_Info_dataRows;
        private bool P02_IsDox;
        private string brandID = string.Empty;
        private string seasonID = string.Empty;

        /// <summary>
        /// P02_AddNewItem
        /// </summary>
        public P02_AddNewItem(DataRow dataRow = null, bool IsDox = false)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, "5,Dox,6,Machine/Parts,7,Mock Up,8,Other Sample,9,Other Material");
            MyUtility.Tool.SetupCombox(this.comboDoxItem, 2, 1, ",,1,C/O,2,Payment doc,3,Other");
            this.P02_Info_dataRows = dataRow;
            this.P02_IsDox = IsDox;
        }

        /// <inheritdoc/>.
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.OperationMode != 2)
            {
                this.txtSPNo.IsSupportEditMode = false;
                this.editDescription.ReadOnly = true;
                this.editDescription.IsSupportEditMode = false;
            }

            Ict.DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, "SELECT distinct Description ,ID FROM ShippingReason where Type= 'OS' order by Description", out DataTable dtReason))
            {
                this.comboReason.DataSource = dtReason;
                this.comboReason.DisplayMember = "Description";
                this.comboReason.ValueMember = "ID";
            }
            else
            {
                this.ShowErr(cbResult);
            }

            this.GetLeaderName();
            this.GetStyle();
        }

        // SP#
        private void TxtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !string.IsNullOrEmpty(this.txtSPNo.Text))
            {
                if (MyUtility.Check.Empty(P02.orderCompanyID))
                {
                    MyUtility.Check.Empty("[Order Company] cannot be empty.");
                    return;
                }

                IList<SqlParameter> cmds = new List<SqlParameter>
                {
                    new SqlParameter("@id", this.txtSPNo.Text),
                    new SqlParameter("@OrderCompanyID", P02.orderCompanyID),
                };
                string sqlCmd = $@"
SELECT Orders.ID, 
       Orders.StyleID, 
       Orders.SeasonID, 
       Orders.BrandID, 
       Orders.SMR, 
       Description = [dbo].[getBOFMtlDesc](Orders.StyleUkey),
       Style.StyleName, 
       Style.Gender,
       Reason.Name,
       PoPrice = iif(isnull(Style_UnitPrice.PoPrice, 0) != 0, Style_UnitPrice.PoPrice, suNotZero.PoPrice)
FROM Orders WITH (NOLOCK)
INNER JOIN Style WITH (NOLOCK) on Orders.StyleID = Style.ID and Orders.BrandID = Style.BrandID and  Orders.SeasonID = Style.SeasonID
LEFT JOIN Reason WITH (NOLOCK) on Reason.ReasonTypeID= 'Style_Apparel_Type' and Reason.ID = Style.ApparelType
LEFT JOIN Style_UnitPrice WITH (NOLOCK) on Style_UnitPrice.StyleUkey = Style.Ukey  And Style_UnitPrice.CountryID = '--'
Outer Apply (
    select top 1 su2.PoPrice
    from Style_UnitPrice su2 WITH (NOLOCK)
    inner Join Style s2 WITH (NOLOCK) on s2.Ukey = su2.StyleUkey
    where s2.ID = Style.ID And su2.CountryID = '--'
    and su2.PoPrice != 0
    Order by s2.EditDate desc ,s2.AddDate desc 
) as suNotZero
WHERE Orders.ID = @id
and Orders.OrderCompanyID = @OrderCompanyID

UNION ALL
SELECT mo.ID, 
       mo.StyleID, 
       mo.SeasonID, 
       mo.BrandID, 
       mo.SMR, 
       Description = [dbo].[getBOFMtlDesc](Style.Ukey),
       Style.StyleName, 
       Style.Gender,
       Reason.Name,
       PoPrice = iif(isnull(Style_UnitPrice.PoPrice, 0) != 0, Style_UnitPrice.PoPrice, suNotZero.PoPrice)
FROM MockupOrder mo WITH (NOLOCK)
INNER JOIN Style WITH (NOLOCK) ON Style.ID = mo.StyleID AND Style.SeasonID = mo.SeasonID AND Style.BrandID = mo.BrandID
LEFT JOIN Reason WITH (NOLOCK) on Reason.ReasonTypeID= 'Style_Apparel_Type' and Reason.ID = Style.ApparelType
LEFT JOIN Style_UnitPrice WITH (NOLOCK) on Style_UnitPrice.StyleUkey = Style.Ukey  And Style_UnitPrice.CountryID = '--'
Outer Apply (
    select top 1 su2.PoPrice
    from Style_UnitPrice su2 WITH (NOLOCK)
    inner Join Style s2 WITH (NOLOCK) on s2.Ukey = su2.StyleUkey
    where s2.ID = Style.ID And su2.CountryID = '--'
    and su2.PoPrice != 0
    Order by s2.EditDate desc ,s2.AddDate desc 
) as suNotZero
WHERE mo.ID = @id
";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable orderData);
                if (!result)
                {
                    this.ShowErr(result);
                    this.ClearDatas();
                    e.Cancel = true;
                    return;
                }

                if (orderData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!!");
                    this.ClearDatas();
                    e.Cancel = true;
                    return;
                }

                if (MyUtility.Check.Empty(P02.orderCompanyID))
                {
                    MyUtility.Check.Empty("[Order Company] cannot be empty.");
                    return;
                }

                this.CurrentData["OrderID"] = orderData.Rows[0]["ID"];
                this.CurrentData["SeasonID"] = orderData.Rows[0]["SeasonID"];
                this.CurrentData["StyleID"] = orderData.Rows[0]["StyleID"];
                this.CurrentData["BrandID"] = orderData.Rows[0]["BrandID"];
                this.CurrentData["Leader"] = orderData.Rows[0]["SMR"];

                if (this.comboCategory.SelectedValue.ToString() == "8")
                {
                    this.CurrentData["Price"] = (decimal)orderData.Rows[0]["PoPrice"].ToDouble();
                    this.CurrentData["Description"] = orderData.Rows[0]["Description"];
                    this.txtGender.Text = orderData.Rows[0]["Gender"].ToString();
                    this.txtAplType.Text = orderData.Rows[0]["Name"].ToString();
                }

                this.txtStyleName.Text = orderData.Rows[0]["StyleName"].ToString();
                this.GetLeaderName();
            }
        }

        private void ClearDatas()
        {
            this.CurrentData["OrderID"] = this.txtSPNo.Text;
            this.CurrentData["SeasonID"] = string.Empty;
            this.CurrentData["StyleID"] = string.Empty;
            this.CurrentData["BrandID"] = string.Empty;
            this.CurrentData["Leader"] = string.Empty;
            this.CurrentData["Description"] = string.Empty;
        }

        private void GetLeaderAndDesc()
        {
            DataRow dr;
            if (MyUtility.Check.Seek(
                string.Format(
                @"select s.BulkSMR,[dbo].[getBOFMtlDesc](s.Ukey) as Description
from Style s WITH (NOLOCK) where s.ID = '{0}' and s.SeasonID = '{1}'",
                this.txtstyle.Text,
                this.txtseason.Text), out dr))
            {
                this.CurrentData["Leader"] = dr["BulkSMR"];
   
            }
        }

        // Season
        private void Txtseason_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtseason.OldValue != this.txtseason.Text && !MyUtility.Check.Empty(this.txtseason.Text) && !MyUtility.Check.Empty(this.txtstyle.Text))
            {
                this.GetLeaderAndDesc();
            }
        }

        // Style
        private void Txtstyle_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtstyle.OldValue != this.txtstyle.Text && !MyUtility.Check.Empty(this.txtstyle.Text) && !MyUtility.Check.Empty(this.txtseason.Text))
            {
                this.GetLeaderAndDesc();
            }
        }

        // CTN No.
        private void TxtCTNNo_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtCTNNo.OldValue != this.txtCTNNo.Text)
            {
                this.CurrentData["CTNNo"] = this.txtCTNNo.Text.Trim();
            }
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            if (!this.P02_IsDox && MyUtility.Convert.GetString(this.P02_Info_dataRows["FreightBy"]).ToUpper() != "HAND")
            {
                if (MyUtility.Check.Empty(this.CurrentData["Qty"]))
                {
                    this.editDescription.Focus();
                    MyUtility.Msg.WarningBox("Qty can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentData["UnitID"]))
                {
                    this.editDescription.Focus();
                    MyUtility.Msg.WarningBox("Unit can't empty!");
                    return false;
                }
            }
            #region 檢查必輸欄位
            if (this.comboCategory.Text == "Other Sample" && MyUtility.Check.Empty(this.CurrentData["StyleID"]))
            {
                this.editDescription.Focus();
                MyUtility.Msg.WarningBox("Style can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Description"]))
            {
                this.editDescription.Focus();
                MyUtility.Msg.WarningBox("Description can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["CTNNo"]))
            {
                this.txtCTNNo.Focus();
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["NW"]))
            {
                this.numNW.Focus();
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Category"]))
            {
                this.comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Receiver"]))
            {
                this.txtReceiver.Focus();
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Leader"]))
            {
                this.txtTeamLeader.Focus();
                MyUtility.Msg.WarningBox("Team Leader can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["BrandID"]))
            {
                this.txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!");
                return false;
            }

            if (MyUtility.Convert.GetString(this.CurrentData["Category"]) == "7")
            {
                if (MyUtility.Check.Empty(this.CurrentData["StyleID"]))
                {
                    this.txtstyle.Focus();
                    MyUtility.Msg.WarningBox("Style can't empty!");
                    return false;
                }
            }

            if (this.comboCategory.Text == "Other Sample" || this.comboCategory.Text == "Other Material" ||
                this.comboDoxItem.Text == "C/O" || this.comboDoxItem.Text == "Payment doc" ||
                 this.comboDoxItem.Text == "Other")
            {
                if (MyUtility.Check.Empty(this.CurrentData["Remark"]))
                {
                    this.editRemark.Focus();
                    MyUtility.Msg.WarningBox("Remark can't empty!");
                    return false;
                }
            }

            if (this.comboCategory.Text == "Dox")
            {
                if (this.comboDoxItem.Text.Empty())
                {
                    this.comboDoxItem.Focus();
                    MyUtility.Msg.WarningBox("Dox can't empty!");
                    return false;
                }
            }

            if (this.comboCategory.Text == "Other Material")
            {
                if (this.txtRefno.Text.Empty())
                {
                    this.txtRefno.Focus();
                    MyUtility.Msg.WarningBox("Please fill in [Refno]!");
                    return false;
                }
            }

            if (this.comboCategory.Text == "Other Sample")
            {
                if (this.comboReason.Text.Empty())
                {
                    this.comboReason.Focus();
                    MyUtility.Msg.WarningBox("Please select [Reason]!");
                    return false;
                }
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.CheckP02Status(this.CurrentData["ID"].ToString()))
            {
                return false;
            }
            #endregion

            // 新增帶值
            if (this.OperationMode == 2)
            {
                DataRow seq;
                if (!MyUtility.Check.Seek(
                    string.Format(
                    @"select RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3) as Seq1
from Express_Detail WITH (NOLOCK) where ID = '{0}' and Seq2 = ''", MyUtility.Convert.GetString(this.CurrentData["ID"])), out seq))
                {
                    MyUtility.Msg.WarningBox("Get seq fail, pls try again");
                    return false;
                }

                this.CurrentData["Seq1"] = string.IsNullOrEmpty(seq["Seq1"].ToString()) ? "001" : seq["Seq1"];
                this.CurrentData["InCharge"] = Env.User.UserID;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnDeleteBefore()
        {
            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.CheckP02Status(this.CurrentData["ID"].ToString()))
            {
                return false;
            }

            return base.OnDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }

            result = DBProxy.Current.Execute(null, $@"update FtyExport set ExpressID = '' where ExpressID = '{MyUtility.Convert.GetString(this.CurrentData["ID"])}' and ID = '{this.CurrentData["DutyNo"]}'");
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update Fty Export list fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // Team Leader
        private void TxtTeamLeader_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1 WITH (NOLOCK) order by ID", "15,30,10,150", this.txtTeamLeader.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtTeamLeader.Text = item.GetSelectedString();
            this.GetLeaderName();
        }

        // Team Leader
        private void TxtTeamLeader_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtTeamLeader.OldValue != this.txtTeamLeader.Text)
            {
                if (!MyUtility.Check.Seek(this.txtTeamLeader.Text, "TPEPass1", "ID"))
                {
                    this.CurrentData["Leader"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Data not found!!");
                    return;
                }
            }
        }

        // Team Leader
        private void TxtTeamLeader_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtTeamLeader.OldValue != this.txtTeamLeader.Text)
            {
                this.GetLeaderName();
            }
        }

        private void GetLeaderName()
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 WITH (NOLOCK) Where id='{0}'", MyUtility.Convert.GetString(this.CurrentData["Leader"]));
            DataRow dr;
            if (MyUtility.Check.Seek(selectSql, out dr))
            {
                this.displayTeamLeader.Text = MyUtility.Convert.GetString(dr["Name"]);
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    this.displayTeamLeader.Text = this.displayTeamLeader.Text + " #" + MyUtility.Convert.GetString(dr["extNo"]);
                }
            }
            else
            {
                this.displayTeamLeader.Text = string.Empty;
            }
        }

        private void GetStyle()
        {
            string sql = $@"
select  s.StyleName, 
        s.Gender, 
        APtype = r.Name, 
        sh.HSCode1,
        PoPrice = iif(isnull(su.PoPrice, 0) != 0, su.PoPrice,suNotZero.PoPrice),
        s.Description 
from Style s with (nolock)
left join Reason r with (nolock) on r.ReasonTypeID= 'Style_Apparel_Type' and r.ID = s.ApparelType
left join Style_HSCode sh with (nolock) on sh.StyleUkey  = s.Ukey
left join Style_UnitPrice su with (nolock) on su.StyleUkey = s.Ukey and su.CountryID = '--'
Outer Apply (
    select top 1 su2.PoPrice
    from Style_UnitPrice su2 with (nolock)
    inner Join style s2 with (nolock) on s2.Ukey = su2.StyleUkey
    where s2.ID = s.ID 
    and su2.CountryID = '--'
    and su2.PoPrice != 0
    order by s2.EditDate desc ,s2.AddDate desc 
) as suNotZero
where (s.BrandID = '{this.txtbrand.Text}' or '' = '{this.txtbrand.Text}')
and s.ID = '{this.txtstyle.Text}' 
and (s.SeasonID = '{this.txtseason.Text}' or '' = '{this.txtseason.Text}')
Order by s.AddDate";

            DataRow dr;
            if (MyUtility.Check.Seek(string.Format(sql), out dr))
            {
                this.txtStyleName.Text = dr["StyleName"].ToString();
            }
            else
            {
                this.CurrentData["HSCode"] = string.Empty;
                this.txtGender.Text = string.Empty;
                this.CurrentData["Description"] = string.Empty;
                this.txtAplType.Text = string.Empty;
                this.CurrentData["Price"] = 0m;
                this.txtStyleName.Text = string.Empty;
            }

            if (this.comboCategory.Text == "Other Sample" && dr != null)
            {
                this.CurrentData["HSCode"] = dr["HSCode1"].ToString();
                this.txtGender.Text = dr["Gender"].ToString();
                this.CurrentData["Description"] = dr["Description"].ToString();
                this.txtAplType.Text = dr["APtype"].ToString();
                this.CurrentData["Price"] = dr["PoPrice"].ToDouble();
            }
            else if (this.comboCategory.Text == "Other Material")
            {
                sql = $@"
select  Type = Case When type = 'A' then 'Accessory'
                    When type = 'F' then 'Fabric'
                    Else ''
                    End,
        Description,
        HSCode,
        MtlTypeID
from Fabric with (nolock)
where Refno = '{this.CurrentData["RefNo"]}'";

                DataRow row;
                if (MyUtility.Check.Seek(string.Format(sql), out row))
                {
                    this.CurrentData["HSCode"] = row["HSCode"].ToString();
                    this.CurrentData["Description"] = row["Description"].ToString();
                    this.txtGender.Text = row["Type"].ToString();
                    this.txtAplType.Text = row["MtlTypeID"].ToString();
                }
            }
        }

        private void ComboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboCategory.SelectedIndex == -1)
            {
                return;
            }

            if (this.comboCategory.SelectedValue.Equals("8") || this.comboCategory.SelectedValue.Equals("9"))
            {
                this.editRemark.WatermarkText = " Please advise the reason of select other sample / other material.";
                this.editRemark.WatermarkColor = SystemColors.GrayText;
                this.txtSPNo.Text = string.Empty;
                this.CurrentData["Category"] = this.comboCategory.SelectedValue;
                this.CurrentData.EndEdit();
            }
            else
            {
                this.editRemark.WatermarkText = string.Empty;
                this.editRemark.WatermarkColor = SystemColors.WindowText;
            }
        }

        private void ComboCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.CurrentData != null)
            {
                this.CurrentData["Category"] = this.comboCategory.SelectedValue;
            }

            if (this.comboCategory.Text == "Dox")
            {
                this.comboDoxItem.Visible = true;
            }
            else
            {
                this.comboDoxItem.Visible = false;
            }

            if (this.comboCategory.Text == "Other Material")
            {
                this.txtRefno.Enabled = true;
                this.labGender.Text = "Type";
                this.labType.Text = "Material Type";
            }
            else
            {
                this.txtRefno.Enabled = false;
                this.labGender.Text = "Gender";
                this.labType.Text = "Apparel Type";
            }

            if (this.comboCategory.Text == "Other Sample")
            {
                this.txtRefno.Visible = false;
                this.comboReason.Visible = true;
                this.labRefno.Text = "Reason";
            }
            else
            {
                this.txtRefno.Visible = true;
                this.comboReason.Visible = false;
                this.labRefno.Text = "Refno";
            }
        }

        private void ComboDoxItem_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboCategory.Text == "Dox")
            {
                if (this.comboDoxItem.Text == "C/O" || this.comboDoxItem.Text == "Payment doc")
                {
                    this.editRemark.WatermarkText = " Please update invoice no.";
                    this.editRemark.WatermarkColor = SystemColors.GrayText;
                }
                else
                {
                    this.editRemark.WatermarkText = string.Empty;
                    this.editRemark.WatermarkColor = SystemColors.WindowText;
                }
            }
        }

        private void TxtRefno_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtRefno.OldValue != this.txtRefno.Text && !MyUtility.Check.Empty(this.txtRefno.Text))
            {
                string sqlcmd = $@"select 1 from Fabric where Refno = '{this.txtRefno.Text}'";
                if (!MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                {
                    MyUtility.Msg.WarningBox($"This Refno：<{this.txtRefno.Text}> does not exist!!");
                    e.Cancel = true;
                    this.CurrentData["HSCode"] = string.Empty;
                    this.CurrentData["Description"] = string.Empty;
                    this.txtGender.Text = string.Empty;
                    this.txtAplType.Text = string.Empty;
                    return;
                }
            }
        }

        private void TxtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode && this.txtRefno.OldValue != this.txtRefno.Text && !MyUtility.Check.Empty(this.txtRefno.Text))
            {
                string sqlcmd = $@"select 1 from Fabric where Refno = '{this.txtRefno.Text}'";
                if (!MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                {
                    MyUtility.Msg.WarningBox($"This Refno：<{this.txtRefno.Text}> does not exist!!");
                    this.txtRefno.Focus();
                    return;
                }
            }

            if (this.EditMode)
            {
                string sqlcmd = "select DISTINCT Refno = (Refno) from Fabric where Junk = 0 order by Refno";

                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "30", this.txtRefno.Text, headercaptions: "Refno");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.txtRefno.Text = item.GetSelectedString();
            }
        }

        private void Txtstyle_Leave(object sender, EventArgs e)
        {
            this.GetStyle();
        }

        private void ComboCategory_Validated(object sender, EventArgs e)
        {
            if (this.CurrentData != null && this.comboCategory.OldValue != this.comboCategory.SelectedValue)
            {
                this.txtSPNo.Text = string.Empty;
                this.CurrentData["StyleID"] = string.Empty;
                this.CurrentData["BrandID"] = string.Empty;
                this.CurrentData["SeasonID"] = string.Empty;
                this.CurrentData["Reason"] = string.Empty;
                this.CurrentData["Remark"] = string.Empty;
                this.CurrentData["Description"] = string.Empty;
                this.CurrentData["Price"] = 0;
                this.CurrentData["CTNNO"] = string.Empty;
                this.CurrentData["NW"] = 0.00;
                this.CurrentData["Qty"] = 0;
                this.CurrentData["UnitID"] = string.Empty;
                this.CurrentData["Leader"] = string.Empty;
                this.CurrentData["Receiver"] = string.Empty;
                this.CurrentData["OrderNumber"] = 0;
                this.CurrentData["Seq1"] = string.Empty;
                this.CurrentData["Seq2"] = string.Empty;
                this.CurrentData["RefNO"] = string.Empty;
                this.CurrentData["OrderID"] = string.Empty;
                this.txtStyleName.Text = string.Empty;
                this.txtGender.Text = string.Empty;
                this.txtAplType.Text = string.Empty;
                this.txtHscode.Text = string.Empty;
                this.displayTeamLeader.Text = string.Empty;
            }
        }

        private void TxtRefno_Validated(object sender, EventArgs e)
        {
            this.GetStyle();
        }
    }
}
