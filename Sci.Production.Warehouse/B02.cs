using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Data.SqlClient;
using Sci.Production.PublicPrg;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Warehouse
{
    public partial class B02 : Win.Tems.Input1
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable combos = new DataTable();
            combos.ColumnsStringAdd("Key");
            combos.ColumnsStringAdd("Value");
            combos.Rows.Add(new object[] { "I", "Inventory" });
            combos.Rows.Add(new object[] { "B", "Bulk" });
            combos.Rows.Add(new object[] { "O", "Scrap" });

            // Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            // comboBox1_RowSource.Add("I", "Inventory");
            // comboBox1_RowSource.Add("B", "Bulk");
            this.comboStockType.DataSource = combos; // new BindingSource(comboBox1_RowSource, null);
            this.comboStockType.ValueMember = "Key";
            this.comboStockType.DisplayMember = "Value";

            // 有新增權限的人才可以按這顆按鈕
            bool canNew = Prgs.GetAuthority(Env.User.UserID, "B02. Material Location Index", "CanNew");
            this.btnBatchCreate.Enabled = canNew;
        }

        // 編輯狀態限制
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.comboStockType.ReadOnly = true;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.btnBatchCreate.Enabled = !this.EditMode;
        }

        // 存檔前檢查
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

            this.CurrentMaintain["ID"] = this.CurrentMaintain["ID"].ToString().Trim();
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                if (string.Compare(CurrentMaintain["StockType"].ToString(), "B", true) == 0)
                {
                    DataTable dtMain = CurrentMaintain.Table.Clone();
                    dtMain.ImportRow(CurrentMaintain);
                    Task.Run(() => new Gensong_AutoWHFabric().SentMtlLocationToGensongAutoWHFabric(dtMain))
                   .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
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
