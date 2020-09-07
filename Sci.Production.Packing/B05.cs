using Ict;
using Sci.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class B05 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string cmd = $@"
SELECT [Text]=Name,[Value]=ID

FROM DropDownList
WHERE Type ='PMS_ShipMarkCategory' 
";
            DataTable categoryData;
            DualResult result = DBProxy.Current.Select(null, cmd, out categoryData);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.comboCategory.DataSource = new BindingSource(categoryData, null);
            this.comboCategory.ValueMember = "Value";
            this.comboCategory.DisplayMember = "Text";
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!this.EditMode && this.CurrentMaintain != null && this.tabs.SelectedIndex == 1)
            {
                bool junk = MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
                this.toolbar.cmdJunk.Enabled = !junk && this.Perm.Junk;
                this.toolbar.cmdUnJunk.Enabled = junk && this.Perm.Junk;
            }
            else
            {
                this.toolbar.cmdJunk.Enabled = false;
                this.toolbar.cmdUnJunk.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (this.CurrentMaintain["Category"].ToString() == "PIC" && this.EditMode)
            {
                this.checkIsSSCC.ReadOnly = false;
            }
            else
            {
                this.checkIsSSCC.ReadOnly = true;
                this.checkIsSSCC.IsSupportEditMode = false;
            }

            if (this.EditMode)
            {
                this.btnTemplateUpload.Enabled = false;
            }
            else
            {
                this.btnTemplateUpload.Enabled = true;
            }

            bool hasDetail = MyUtility.Check.Seek($"SELECT 1 FROM ShippingMarkType_Detail WHERE ShippingMarkTypeUkey = '{this.CurrentMaintain["Ukey"]}'");
            this.btnTemplateUpload.ForeColor = hasDetail ? Color.Blue : Color.Black;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Category"] = "PIC";
            this.CurrentMaintain["IsSSCC"] = false;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            if (this.CurrentMaintain["Category"].ToString() == "PIC" && this.EditMode)
            {
                this.checkIsSSCC.ReadOnly = false;
            }
            else
            {
                this.checkIsSSCC.ReadOnly = true;
            }

            this.txtbrand.ReadOnly = true;

            this.comboCategory.ReadOnly = true;

            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]) || MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Shipping Mark Type and Brand cannot be empty.");
                return false;
            }

            string cmd = $@"SELECT * FROM ShippingMarkType WHERE BrandID='{this.CurrentMaintain["BrandID"]}' AND ID ='{this.CurrentMaintain["ID"]}' AND Ukey <> {this.CurrentMaintain["UKey"]}";
            bool duplicate = MyUtility.Check.Seek(cmd);
            if (duplicate)
            {
                MyUtility.Msg.WarningBox($"Brand {this.CurrentMaintain["BrandID"]} Shiping Mark Type {this.CurrentMaintain["ID"]} already exists.");
                return false;
            }

            if (this.CurrentMaintain["Category"].ToString() == "PIC")
            {
                cmd = $@"SELECT * FROM ShippingMarkType WHERE BrandID='{this.CurrentMaintain["BrandID"]}' AND Category = 'PIC' AND IsSSCC = 1 AND Ukey <> {this.CurrentMaintain["UKey"]} ";
                DataTable dt;

                DualResult r = DBProxy.Current.Select(null, cmd, out dt);
                if (!r)
                {
                    this.ShowErr(r);
                    return false;
                }

                if (MyUtility.Convert.GetBool(this.CurrentMaintain["IsSSCC"]))
                {
                    // 若該品牌已經有SSCC=1的資料，則詢問，是否用當下的資料，覆蓋已經DB的資料該筆資料設定為SSCC
                    if (dt.Rows.Count > 0)
                    {
                        string otherBrand = dt.Rows[0]["BrandID"].ToString();
                        string otherIDd = dt.Rows[0]["ID"].ToString();

                        DialogResult diaR = MyUtility.Msg.QuestionBox($@"Brand {otherBrand} Shipping Mark Type {otherIDd} already ticked ‘Is SSCC’, system will auto untick ‘Is SSCC’ for Shipping Mark Type {this.CurrentMaintain["ID"]}.
Please check to continue process.");
                        if (diaR == DialogResult.Yes)
                        {
                            cmd = $@"UPDATE ShippingMarkType SET IsSSCC = 0 WHERE BrandID='{otherBrand}' AND ID = '{otherIDd}'AND IsSSCC = 1 ";
                            r = DBProxy.Current.Execute(null, cmd);
                            if (!r)
                            {
                                this.ShowErr(r);
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // 若該品牌沒有SSCC=1的資料，則將該筆資料設定為SSCC
                    if (dt.Rows.Count == 0)
                    {
                        MyUtility.Msg.InfoBox($"Brand {this.CurrentMaintain["BrandID"]} not yet set default, system will auto tick ‘Is SSCC’ for this sticker type.");
                        this.CurrentMaintain["IsSSCC"] = true;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.Reload();
        }

        /// <inheritdoc/>
        public void Reload()
        {
            if (this.CurrentDataRow != null)
            {
                string idIndex = string.Empty;
                if (!MyUtility.Check.Empty(this.CurrentMaintain))
                {
                    if (!MyUtility.Check.Empty(this.CurrentMaintain["Ukey"]))
                    {
                        idIndex = MyUtility.Convert.GetString(this.CurrentMaintain["Ukey"]);
                    }
                }

                this.ReloadDatas();
                this.RenewData();
                if (!MyUtility.Check.Empty(idIndex))
                {
                    this.gridbs.Position = this.gridbs.Find("Ukey", idIndex);
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();

            if (MyUtility.Convert.GetBool(this.CurrentMaintain["IsSSCC"]))
            {
                MyUtility.Msg.WarningBox("Shipping Mark Type is SSCC cannot be junk. ");
                return;
            }

            string sqlcmd = $@"update ShippingMarkType set junk = 1,EditDate= GETDATE(), EditName='{Sci.Env.User.UserID}' where Ukey = {this.CurrentMaintain["Ukey"]}";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            string sqlcmd = $@"update ShippingMarkType set junk = 0,EditDate= GETDATE(), EditName='{Sci.Env.User.UserID}' where Ukey = {this.CurrentMaintain["Ukey"]}";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        private void ComboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentMaintain["Category"] = this.comboCategory.SelectedValue.ToString();
                if (this.comboCategory.SelectedValue.ToString() == "PIC")
                {
                    this.checkIsSSCC.ReadOnly = false;
                    this.chkIsTemplate.ReadOnly = false;
                }

                if (this.comboCategory.SelectedValue.ToString() == "HTML")
                {
                    this.checkIsSSCC.ReadOnly = true;
                    this.chkIsTemplate.ReadOnly = true;
                    this.CurrentMaintain["FromTemplete"] = true;
                }
            }
            else
            {
                this.checkIsSSCC.ReadOnly = true;
                this.chkIsTemplate.ReadOnly = true;
            }

            if (this.IsDetailInserting)
            {
                this.CurrentMaintain["ID"] = string.Empty;
                this.CurrentMaintain["BrandID"] = string.Empty;
                this.CurrentMaintain["IsSSCC"] = false;
            }
        }

        /// <inheritdoc/>
        private void BtnTemplateUpload_Click(object sender, EventArgs e)
        {

        }
    }
}
