using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Data.SqlClient;
using Sci.Production.PublicPrg;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Warehouse
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable combos = new DataTable();
            combos.ColumnsStringAdd("Key");
            combos.ColumnsStringAdd("Value");
            combos.Rows.Add(new object[] { "I", "Inventory" });
            combos.Rows.Add(new object[] { "B", "Bulk" });
            combos.Rows.Add(new object[] { "O", "Scrap" });
            //Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            //comboBox1_RowSource.Add("I", "Inventory");
            //comboBox1_RowSource.Add("B", "Bulk");
            comboStockType.DataSource = combos;//new BindingSource(comboBox1_RowSource, null);
            comboStockType.ValueMember = "Key";
            comboStockType.DisplayMember = "Value";


            // 有新增權限的人才可以按這顆按鈕
            bool canNew = Prgs.GetAuthority(Sci.Env.User.UserID, "B02. Material Location Index", "CanNew");
            this.btnBatchCreate.Enabled = canNew;
        }

        //編輯狀態限制
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

        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            if (CurrentMaintain["ID"].ToString().IndexOfAny(new char[] {','}, 0) > -1)
            {
                MyUtility.Msg.WarningBox("< Code > can not have ',' !");
                this.txtCode.Focus();
                return false;
            }
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Stocktype"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Stock Type > can not be empty!");
                this.comboStockType.Focus();
                return false;
            }

            if (IsDetailInserting)
            {
                if (!checkCode())
                {
                    return false;
                }
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.txtDescription.Focus();
                return false;
            }

            CurrentMaintain["ID"] = CurrentMaintain["ID"].ToString().Trim();
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                string strID = CurrentMaintain["ID"].ToString();
                Task.Run(() => new Gensong_AutoWHFabric().SentMtlLocationToGensongAutoWHFabric(strID))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private bool checkCode()
        {
            bool check = true;
            string strSQL = @"  select * 
                                from dbo.mtlLocation mtl
                                where   mtl.ID = @ID
                                        and mtl.StockType = @StockType";
            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@ID", CurrentMaintain["ID"].ToString()));
            listPar.Add(new SqlParameter("@StockType", CurrentMaintain["Stocktype"].ToString()));

            DataTable dt;
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQL, listPar, out dt)))
            {
                MyUtility.Msg.WarningBox(result.Description);
                check = false;
            }
            else if (dt != null && dt.Rows.Count > 0)
            {
                MyUtility.Msg.InfoBox(string.Format("This <code> is areadly in {0}", comboStockType.Text.ToString()));
                check = false;
            }
            return check;
        }

        private void btnBatchCreate_Click(object sender, EventArgs e)
        {
            B02_BatchCreate form = new B02_BatchCreate();
            form.ShowDialog();
            ReloadDatas();
        }
    }
}
