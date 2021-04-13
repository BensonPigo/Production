using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P01 : Win.Tems.Input1
    {
        private string StyleUkey;
        private string keyWord = Env.User.Keyword;
        private string histype;

        /// <summary>
        /// Initializes a new instance of the <see cref="P01"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        /// <param name="type">Type</param>
        public P01(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.histype = type;
            if (type == "1")
            {
                this.Text = "P01. Cutting Master List";
                this.DefaultFilter = string.Format("MDivisionID = '{0}' AND Finished = 0", this.keyWord);
            }
            else
            {
                this.Text = "P011. Cutting Master List(History)";
                this.DefaultFilter = string.Format("MDivisionID = '{0}' AND Finished = 1", this.keyWord);
                this.IsSupportEdit = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region from Orders Base
            if (MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["ID"]), out DataRow orderdr))
            {
                this.displayStyleNo.Text = orderdr["Styleid"].ToString();
                this.displaySeason.Text = orderdr["Seasonid"].ToString();
                this.displayCategory.Text = orderdr["Category"].ToString();
                this.displayProject.Text = orderdr["Projectid"].ToString();
                this.displayOrderQty.Text = orderdr["styleUnit"].ToString();
                this.StyleUkey = orderdr["StyleUkey"].ToString();
                if (orderdr["eachconsapv"] == DBNull.Value)
                {
                    this.dateEachConsApvDate.Value = null;
                }
                else
                {
                    this.dateEachConsApvDate.Value = Convert.ToDateTime(orderdr["eachconsapv"]);
                }

                if (orderdr["LETA"] == DBNull.Value)
                {
                    this.dateSewingScheduleLastMtlETA.Value = null;
                }
                else
                {
                    this.dateSewingScheduleLastMtlETA.Value = Convert.ToDateTime(orderdr["LETA"]);
                }

                if (orderdr["MTLETA"] == DBNull.Value)
                {
                    this.dateSewingScheduleRMtlETA.Value = null;
                }
                else
                {
                    this.dateSewingScheduleRMtlETA.Value = Convert.ToDateTime(orderdr["MTLETA"]);
                }
            }
            else
            {
                this.displayStyleNo.Text = string.Empty;
                this.displaySeason.Text = string.Empty;
                this.displayCategory.Text = string.Empty;
                this.displayProject.Text = string.Empty;
                this.displayOrderQty.Text = string.Empty;
                this.dateEachConsApvDate.Value = null;
                this.dateSewingScheduleLastMtlETA.Value = null;
                this.dateSewingScheduleRMtlETA.Value = null;
            }

            // Switch to WorkOrder
            switch (this.CurrentMaintain["Worktype"].ToString())
            {
                case "1":
                    this.displaySwitchtoWorkOrder.Text = "By Combination";
                    break;
                case "2":
                    this.displaySwitchtoWorkOrder.Text = "By PO";
                    break;
                default:
                    this.displaySwitchtoWorkOrder.Text = string.Empty;
                    break;
            }

            #endregion
            #region from Orders 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T欄位值
            #endregion
            #region from Orders 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T欄位值
            string sqlCmd;
            sqlCmd = string.Format(
                @"
select 
    PoList = isnull([dbo].getPOComboList(o.ID,o.POID),''),
    CuttingList = isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'')
from Orders o WITH (NOLOCK) where ID = '{0}'",
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable ordersData);
            if (result)
            {
                if (ordersData.Rows.Count > 0)
                {
                    this.editPOCombo.Text = MyUtility.Convert.GetString(ordersData.Rows[0]["PoList"].ToString().Replace(" ", string.Empty));
                    this.editCuttingCombo.Text = MyUtility.Convert.GetString(ordersData.Rows[0]["CuttingList"].ToString().Replace(" ", string.Empty));
                }
                else
                {
                    this.editPOCombo.Text = string.Empty;
                    this.editCuttingCombo.Text = string.Empty;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!! " + result.ToString());
                this.editPOCombo.Text = string.Empty;
                this.editCuttingCombo.Text = string.Empty;
            }
            #endregion
            #region from Orders sum FOC & OrderQty
            sqlCmd = string.Format(
                @"
Select
    Qty = isnull(sum(Qty),0),
    FOC = isnull(sum(FOCQty),0)
from Orders WITH (NOLOCK) 
where CuttingSp = '{0}'",
                this.CurrentMaintain["ID"]);
            if (MyUtility.Check.Seek(sqlCmd, out orderdr))
            {
                this.numFOCQty.Value = Convert.ToDecimal(orderdr["FOC"]);
                this.numOrderQty.Value = Convert.ToDecimal(orderdr["Qty"]);
            }
            else
            {
                this.numFOCQty.Value = 0;
                this.numOrderQty.Value = 0;
            }
            #endregion
            #region from System Cutinline,Cutoffline 是減System.Cutday計算
            int cutday = Convert.ToInt16(MyUtility.GetValue.Lookup(string.Format("Select cutday from System WITH (NOLOCK)")));
            if (this.CurrentMaintain["sewinline"] == DBNull.Value)
            {
                this.dateCuttingInLine.Value = null;
            }
            else
            {
                this.dateCuttingInLine.Value = Convert.ToDateTime(this.CurrentMaintain["sewinline"]).AddDays(-cutday);
            }

            if (this.CurrentMaintain["sewoffline"] == DBNull.Value)
            {
                this.dateSewingScheduleCuttingOffLine.Value = null;
            }
            else
            {
                this.dateSewingScheduleCuttingOffLine.Value = Convert.ToDateTime(this.CurrentMaintain["sewoffline"]).AddDays(-cutday);
            }
            #endregion
            #region button1 color change
            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Order_MarkerList", "ID"))
            {
                this.btnMarkerList.ForeColor = Color.Blue;
            }
            else
            {
                this.btnMarkerList.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Order_EachCons", "ID"))
            {
                this.btnEachCons.ForeColor = Color.Blue;
            }
            else
            {
                this.btnEachCons.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Order_Qty", "ID"))
            {
                this.btnQuantitybreakdown.ForeColor = Color.Blue;
            }
            else
            {
                this.btnQuantitybreakdown.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Order_ColorCombo", "ID"))
            {
                this.btnColorCombo.ForeColor = Color.Blue;
            }
            else
            {
                this.btnColorCombo.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Bundle", "POID"))
            {
                this.btnBundleCard.ForeColor = Color.Blue;
            }
            else
            {
                this.btnBundleCard.ForeColor = Color.Black;
            }

            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.CurrentMaintain["ID"].ToString(), "Orders", "ID");
            string patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{this.CurrentMaintain["ID"]}','','','{ukey}','')s";
            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            if (patternukey != string.Empty)
            {
                this.btnGarmentList.ForeColor = Color.Blue;
            }
            else
            {
                this.btnGarmentList.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "WorkOrder", "ID"))
            {
                this.btnCutPartsCheckSummary.ForeColor = Color.Blue;
                this.btnCutPartsCheck.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCutPartsCheckSummary.ForeColor = Color.Black;
                this.btnCutPartsCheck.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Empty(this.StyleUkey))
            {
                this.btnProductionkit.ForeColor = Color.Black;
            }
            else
            {
                this.btnProductionkit.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", this.StyleUkey)) ? Color.Blue : Color.Black;
            }
            #endregion
        }

        // Marker List
        private void BtnMarkerList_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new P01_MarkerList(false, this.CurrentMaintain["ID"].ToString(), null, null, "Order_Markerlist", this.CurrentMaintain);
            frm.ShowDialog(this);
        }

        // Each Cons.
        private void BtnEachCons_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new PublicForm.EachConsumption(false, this.CurrentMaintain["id"].ToString(), null, null, false, true, true);
            frm.ShowDialog(this);
            this.RenewData();
            this.OnDetailEntered();
        }

        // Button Bundle Card
        private void BtnBundleCard_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new P01_BundleCard(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["MDivisionID"].ToString());
            frm.ShowDialog(this);
        }

        // Cutpart Check
        private void BtnCutPartsCheck_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new P01_Cutpartcheck(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["WorkType"].ToString());
            frm.ShowDialog(this);
        }

        // Cutpart Check Summary
        private void BtnCutPartsCheckSummary_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new P01_Cutpartchecksummary(this.CurrentMaintain["ID"].ToString());
            frm.ShowDialog(this);
        }

        // Quantity breakdown
        private void BtnQuantitybreakdown_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.editPOCombo.Text);
            frm.ShowDialog(this);
        }

        // ColorComb
        private void BtnColorCombo_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.CurrentMaintain["ID"].ToString(), "Orders", "ID");
            var frm = new PublicForm.ColorCombination(this.CurrentMaintain["ID"].ToString(), ukey);
            frm.ShowDialog(this);
        }

        // ProductionKit
        private void BtnProductionkit_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new PPIC.P01_ProductionKit(false, this.StyleUkey, null, null, null);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }

        // Garment List
        private void BtnGarmentList_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.CurrentMaintain["ID"].ToString(), "Orders", "ID");
            var frm = new PublicForm.GarmentList(ukey, this.CurrentMaintain["ID"].ToString(), null);
            frm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            string id = this.CurrentMaintain["ID"].ToString();
            if (this.tabs.SelectedIndex == 1)
            {
                var frm = new P01_Print_OrderList(id, this.histype == "1" ? 0 : 1);
                frm.ShowDialog();
            }

            return base.ClickPrint();
        }

        private void P01_FormLoaded(object sender, EventArgs e)
        {
            // base.OnFormLoaded();
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
        }
    }
}
