using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        string StyleUkey;
        private string keyWord = Sci.Env.User.Keyword;
        private string histype;
        public P01(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            this.histype = Type;
            if (Type == "1")
            {
                this.Text = "P01. Cutting Master List";
                this.DefaultFilter = string.Format("MDivisionID = '{0}' AND Finished = 0", keyWord);
            }
            else
            {
                this.Text = "P011. Cutting Master List(History)";
                this.DefaultFilter = string.Format("MDivisionID = '{0}' AND Finished = 1", keyWord);
                this.IsSupportEdit = false;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region from Orders Base
            DataRow orderdr;
            if (MyUtility.Check.Seek(String.Format("Select * from Orders WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"]), out orderdr))
            {
                displayStyleNo.Text = orderdr["Styleid"].ToString();
                displaySeason.Text = orderdr["Seasonid"].ToString();
                displayCategory.Text = orderdr["Category"].ToString();
                displayProject.Text = orderdr["Projectid"].ToString();
                displayOrderQty.Text = orderdr["styleUnit"].ToString();
                StyleUkey = orderdr["StyleUkey"].ToString();
                if (orderdr["eachconsapv"] == DBNull.Value) dateEachConsApvDate.Value = null;
                else dateEachConsApvDate.Value = Convert.ToDateTime(orderdr["eachconsapv"]);
                if (orderdr["LETA"] == DBNull.Value) dateSewingScheduleLastMtlETA.Value = null;
                else dateSewingScheduleLastMtlETA.Value = Convert.ToDateTime(orderdr["LETA"]);
                if (orderdr["MTLETA"] == DBNull.Value) dateSewingScheduleRMtlETA.Value = null;
                else dateSewingScheduleRMtlETA.Value = Convert.ToDateTime(orderdr["MTLETA"]);
            }
            else
            {
                displayStyleNo.Text = "";
                displaySeason.Text = "";
                displayCategory.Text = "";
                displayProject.Text = "";
                displayOrderQty.Text = "";
                dateEachConsApvDate.Value = null;
                dateSewingScheduleLastMtlETA.Value = null;
                dateSewingScheduleRMtlETA.Value = null;
            }
            // Switch to WorkOrder
            switch (CurrentMaintain["Worktype"].ToString())
            {
                case "1":
                    displaySwitchtoWorkOrder.Text = "By Combination";
                    break;
                case "2":
                    displaySwitchtoWorkOrder.Text = "By PO";
                    break;
                default:
                    displaySwitchtoWorkOrder.Text = "";
                    break;
            }
            #endregion
            #region from Orders 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T欄位值
            DataTable OrdersData;
            string sqlCmd;
            sqlCmd = string.Format(@"
select 
    PoList = isnull([dbo].getPOComboList(o.ID,o.POID),''),
    CuttingList = isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'')
from Orders o WITH (NOLOCK) where ID = '{0}'"
                , MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OrdersData);
            if (result)
            {
                if (OrdersData.Rows.Count > 0)
                {
                    editPOCombo.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["PoList"].ToString().Replace(" ", ""));
                    editCuttingCombo.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["CuttingList"].ToString().Replace(" ", ""));
                }
                else
                {
                    editPOCombo.Text = "";
                    editCuttingCombo.Text = "";
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!! " + result.ToString());
                editPOCombo.Text = "";
                editCuttingCombo.Text = "";
            }
            #endregion
            #region from Orders sum FOC & OrderQty
            sqlCmd = string.Format(@"
Select
    Qty = isnull(sum(Qty),0),
    FOC = isnull(sum(FOCQty),0)
from Orders WITH (NOLOCK) 
where CuttingSp = '{0}'"
                , CurrentMaintain["ID"]);
            if (MyUtility.Check.Seek(sqlCmd, out orderdr))
            {
                numFOCQty.Value = Convert.ToDecimal(orderdr["FOC"]);
                numOrderQty.Value = Convert.ToDecimal(orderdr["Qty"]);
            }
            else
            {
                numFOCQty.Value = 0;
                numOrderQty.Value = 0;
            }
            #endregion
            #region from System Cutinline,Cutoffline 是減System.Cutday計算
            int cutday = Convert.ToInt16(MyUtility.GetValue.Lookup(String.Format("Select cutday from System WITH (NOLOCK)")));
            if (CurrentMaintain["sewinline"] == DBNull.Value) dateCuttingInLine.Value = null;
            else dateCuttingInLine.Value = Convert.ToDateTime(CurrentMaintain["sewinline"]).AddDays(-cutday);
            if (CurrentMaintain["sewoffline"] == DBNull.Value) dateSewingScheduleCuttingOffLine.Value = null;
            else dateSewingScheduleCuttingOffLine.Value = Convert.ToDateTime(CurrentMaintain["sewoffline"]).AddDays(-cutday);
            #endregion
            #region button1 color change
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_MarkerList", "ID")) btnMarkerList.ForeColor = Color.Blue;
            else btnMarkerList.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_EachCons", "ID")) btnEachCons.ForeColor = Color.Blue;
            else btnEachCons.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_Qty", "ID")) btnQuantitybreakdown.ForeColor = Color.Blue;
            else btnQuantitybreakdown.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_ColorCombo", "ID")) btnColorCombo.ForeColor = Color.Blue;
            else btnColorCombo.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Bundle", "POID")) btnBundleCard.ForeColor = Color.Blue;
            else btnBundleCard.ForeColor = Color.Black;

            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["ID"].ToString(), "Orders", "ID");
            string patidsql = String.Format(@"
SELECT ukey 
FROM [Production].[dbo].[Pattern] WITH (NOLOCK) 
WHERE STYLEUKEY = '{0}' and Status = 'Completed' 
AND EDITDATE = (SELECT MAX(EditDate) from pattern WITH (NOLOCK) where styleukey = '{0}' and Status = 'Completed')"
                , ukey);
            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            if (patternukey != "") btnGarmentList.ForeColor = Color.Blue;
            else btnGarmentList.ForeColor = Color.Black;  

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "WorkOrder", "ID"))
            {
                btnCutPartsCheckSummary.ForeColor = Color.Blue;
                btnCutPartsCheck.ForeColor = Color.Blue;
            }
            else
            {
                btnCutPartsCheckSummary.ForeColor = Color.Black;
                btnCutPartsCheck.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Empty(StyleUkey)) btnProductionkit.ForeColor = Color.Black;
            else
                btnProductionkit.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", StyleUkey)) ? Color.Blue : Color.Black;
            #endregion
        }

        //Marker List
        private void btnMarkerList_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_MarkerList(false, CurrentMaintain["ID"].ToString(), null, null, "Order_Markerlist", this.CurrentMaintain);
            frm.ShowDialog(this);
        }
        //Each Cons.
        private void btnEachCons_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.PublicForm.EachConsumption(false, CurrentMaintain["id"].ToString(), null, null, false, true,true);
            frm.ShowDialog(this);
            this.RenewData();
            this.OnDetailEntered();
        }

        //Button Bundle Card
        private void btnBundleCard_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_BundleCard(CurrentMaintain["ID"].ToString());
            frm.ShowDialog(this);
        }
        //Cutpart Check
        private void btnCutPartsCheck_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_Cutpartcheck(CurrentMaintain["ID"].ToString(), CurrentMaintain["WorkType"].ToString());
            frm.ShowDialog(this);
        }
        //Cutpart Check Summary
        private void btnCutPartsCheckSummary_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_Cutpartchecksummary(CurrentMaintain["ID"].ToString());
            frm.ShowDialog(this);
        }
        //Quantity breakdown
        private void btnQuantitybreakdown_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]), editPOCombo.Text);
            frm.ShowDialog(this);
        }
        //ColorComb
        private void btnColorCombo_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["ID"].ToString(), "Orders", "ID");
            var frm = new Sci.Production.PublicForm.ColorCombination(CurrentMaintain["ID"].ToString(), ukey);
            frm.ShowDialog(this);
        }
        //ProductionKit
        private void btnProductionkit_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.PPIC.P01_ProductionKit(false, StyleUkey, null, null, null);
            frm.ShowDialog(this);
            OnDetailEntered();
        }
        //Garment List
        private void btnGarmentList_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["ID"].ToString(), "Orders", "ID");
            var frm = new Sci.Production.PublicForm.GarmentList(ukey, CurrentMaintain["ID"].ToString(),null);
            frm.ShowDialog(this);
        }

        protected override bool ClickPrint()
        {
            string ID = this.CurrentMaintain["ID"].ToString();
            if (tabs.SelectedIndex == 1)
            {
                var frm = new P01_Print_OrderList(ID, this.histype == "1" ? 0 : 1);
                frm.ShowDialog();
            }
            return base.ClickPrint();
        }

        private void P01_FormLoaded(object sender, EventArgs e)
        {
           // base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
        }
    }
}
