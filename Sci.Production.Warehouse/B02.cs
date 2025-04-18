using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class B02 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable dtStockType = new DataTable();
            dtStockType.ColumnsStringAdd("Key");
            dtStockType.ColumnsStringAdd("Value");
            dtStockType.Rows.Add(new object[] { "I", "Inventory" });
            dtStockType.Rows.Add(new object[] { "B", "Bulk" });
            dtStockType.Rows.Add(new object[] { "O", "Scrap" });

            this.comboStockType.DataSource = dtStockType;
            this.comboStockType.ValueMember = "Key";
            this.comboStockType.DisplayMember = "Value";

            DataTable dtLocationType = new DataTable();
            dtLocationType.ColumnsStringAdd("Key");
            dtLocationType.ColumnsStringAdd("Value");
            dtLocationType.Rows.Add(new object[] { "Fabric", "Fabric" });
            dtLocationType.Rows.Add(new object[] { "Accessory", "Accessory" });

            this.comboLocationType.DataSource = dtLocationType;
            this.comboLocationType.ValueMember = "Key";
            this.comboLocationType.DisplayMember = "Value";

            // 有新增權限的人才可以按這顆按鈕
            bool canNew = Prgs.GetAuthority(Env.User.UserID, "B02. Material Location Index", "CanNew");
            this.btnBatchCreate.Enabled = canNew;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (Prgs.IsAutomation() || Automation.UtilityAutomation.IsAutomationEnable)
            {
                this.chkIsWMS.Visible = true;
            }
            else
            {
                this.chkIsWMS.Visible = false;
            }
        }

        // 編輯狀態限制

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.comboStockType.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.btnBatchCreate.Enabled = !this.EditMode;
        }

        /// <inheritdoc/>
        protected override bool ClickCopy()
        {
            return base.ClickCopy();
        }

        // 存檔前檢查

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.CurrentMaintain["ID"].ToString().IndexOfAny(new char[] { ',' }, 0) > -1)
            {
                MyUtility.Msg.WarningBox("< Code > can not have ',' !");
                this.txtCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (this.chkIsWMS.Checked)
            {
                string strid = this.txtCode.Text;
                if (!MyUtility.Check.Empty(strid))
                {
                    bool hasWMS_ = false;
                    if (strid.Length >= 4)
                    {
                        hasWMS_ = strid.Substring(0, 4) == "WMS_";
                    }

                    if (!hasWMS_)
                    {
                        MyUtility.Msg.WarningBox("WMS Location must input \"WMS_\" front.");
                        return false;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Stocktype"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Stock Type > can not be empty!");
                this.comboStockType.Focus();
                return false;
            }

            if (this.IsDetailInserting)
            {
                if (!this.CheckCode())
                {
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.txtDescription.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["LocationType"].ToString()))
            {
                if (this.CurrentMaintain["Description"].ToString().ToUpper().Contains("FAC") || this.CurrentMaintain["Description"].ToString().ToUpper().Contains("FABRIC"))
                {
                    this.CurrentMaintain["LocationType"] = "Fabric";
                }
                else
                {
                    this.CurrentMaintain["LocationType"] = "Accessory";
                }
            }

            this.CurrentMaintain["ID"] = this.CurrentMaintain["ID"].ToString().Trim();
            return base.ClickSaveBefore();
        }

        private bool CheckCode()
        {
            bool check = true;
            string strSQL = @"  select * 
                                from dbo.mtlLocation mtl
                                where   mtl.ID = @ID
                                        and mtl.StockType = @StockType";
            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()));
            listPar.Add(new SqlParameter("@StockType", this.CurrentMaintain["Stocktype"].ToString()));

            DataTable dt;
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQL, listPar, out dt)))
            {
                MyUtility.Msg.WarningBox(result.Description);
                check = false;
            }
            else if (dt != null && dt.Rows.Count > 0)
            {
                MyUtility.Msg.InfoBox(string.Format("This <code> is areadly in {0}", this.comboStockType.Text.ToString()));
                check = false;
            }

            return check;
        }

        private void BtnBatchCreate_Click(object sender, EventArgs e)
        {
            B02_BatchCreate form = new B02_BatchCreate();
            form.ShowDialog();
            this.ReloadDatas();
        }
    }
}
