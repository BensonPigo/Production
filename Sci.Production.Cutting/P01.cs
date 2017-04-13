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
                
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region from Orders Base
            DataRow orderdr;
            if (MyUtility.Check.Seek(String.Format("Select * from Orders WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"]), out orderdr))
            {
                displayBox2.Text = orderdr["Styleid"].ToString();
                displayBox4.Text = orderdr["Seasonid"].ToString();
                displayBox7.Text = orderdr["Category"].ToString();
                displayBox3.Text = orderdr["Projectid"].ToString();
                displayBox5.Text = orderdr["styleUnit"].ToString();
                StyleUkey = orderdr["StyleUkey"].ToString();
                if (orderdr["eachconsapv"] == DBNull.Value) dateBox11.Value = null;
                else dateBox11.Value = Convert.ToDateTime(orderdr["eachconsapv"]);
                if (orderdr["LETA"] == DBNull.Value) dateBox3.Value = null;
                else dateBox3.Value = Convert.ToDateTime(orderdr["LETA"]);
                if (orderdr["MTLETA"] == DBNull.Value) dateBox4.Value = null;
                else dateBox4.Value = Convert.ToDateTime(orderdr["MTLETA"]);
            }
            else
            {
                displayBox2.Text = "";
                displayBox4.Text = "";
                displayBox7.Text = "";
                displayBox3.Text = "";
                displayBox5.Text = "";
                dateBox11.Value = null;
                dateBox3.Value = null;
                dateBox4.Value = null;
            }
            //Switch to WordOrder
            switch (CurrentMaintain["Worktype"].ToString())
            {
                case "1":
                    displayBox8.Text = "By Combination";
                    break;
                case "2":
                    displayBox8.Text = "By PO";
                    break;
                default:
                    displayBox8.Text = "";
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
                    editBox1.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["PoList"].ToString().Replace(" ", ""));
                    editBox2.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["CuttingList"].ToString().Replace(" ", ""));
                }
                else
                {
                    editBox1.Text = "";
                    editBox2.Text = "";
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!! " + result.ToString());
                editBox1.Text = "";
                editBox2.Text = "";
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
                numericBox1.Value = Convert.ToDecimal(orderdr["FOC"]);
                numericBox2.Value = Convert.ToDecimal(orderdr["Qty"]);
            }
            else
            {
                numericBox1.Value = 0;
                numericBox2.Value = 0;
            }
            #endregion
            #region from System Cutinline,Cutoffline 是減System.Cutday計算
            int cutday = Convert.ToInt16(MyUtility.GetValue.Lookup(String.Format("Select cutday from System WITH (NOLOCK)")));
            if (CurrentMaintain["sewinline"] == DBNull.Value) dateBox1.Value = null;
            else dateBox1.Value = Convert.ToDateTime(CurrentMaintain["sewinline"]).AddDays(-cutday);
            if (CurrentMaintain["sewoffline"] == DBNull.Value) dateBox2.Value = null;
            else dateBox2.Value = Convert.ToDateTime(CurrentMaintain["sewoffline"]).AddDays(-cutday);
            #endregion
            #region button1 color change
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_MarkerList", "ID")) button1.ForeColor = Color.Blue;
            else button1.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_EachCons", "ID")) button2.ForeColor = Color.Blue;
            else button2.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_Qty", "ID")) button5.ForeColor = Color.Blue;
            else button5.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_ColorCombo", "ID")) button12.ForeColor = Color.Blue;
            else button12.ForeColor = Color.Black;

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Bundle", "POID")) button3.ForeColor = Color.Blue;
            else button3.ForeColor = Color.Black;

            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["ID"].ToString(), "Orders", "ID");
            string patidsql = String.Format(@"
SELECT ukey 
FROM [Production].[dbo].[Pattern] WITH (NOLOCK) 
WHERE STYLEUKEY = '{0}' and Status = 'Completed' 
AND EDITDATE = (SELECT MAX(EditDate) from pattern WITH (NOLOCK) where styleukey = '{0}' and Status = 'Completed')"
                , ukey);
            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            if (patternukey != "") button9.ForeColor = Color.Blue;
            else button9.ForeColor = Color.Black;  

            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "WorkOrder", "ID"))
            {
                button7.ForeColor = Color.Blue;
                button8.ForeColor = Color.Blue;
            }
            else
            {
                button7.ForeColor = Color.Black;
                button8.ForeColor = Color.Black;
            }

            if (MyUtility.Check.Empty(StyleUkey)) button11.ForeColor = Color.Black;
            else
                button11.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", StyleUkey)) ? Color.Blue : Color.Black;
            #endregion
        }

        //Marker List
        private void button1_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_MarkerList(false, CurrentMaintain["ID"].ToString(), null, null, "Order_Markerlist", this.CurrentMaintain);
            frm.ShowDialog(this);
        }
        //Each Cons.
        private void button2_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.PublicForm.EachConsumption(false, CurrentMaintain["id"].ToString(), null, null, false, true,true);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }
        //Button Bundle Card
        private void button3_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_BundleCard(CurrentMaintain["ID"].ToString());
            frm.ShowDialog(this);
        }
        //Cutpart Check
        private void button8_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_Cutpartcheck(CurrentMaintain["ID"].ToString(), CurrentMaintain["WorkType"].ToString());
            frm.ShowDialog(this);
        }
        //Cutpart Check Summary
        private void button7_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.Cutting.P01_Cutpartchecksummary(CurrentMaintain["ID"].ToString());
            frm.ShowDialog(this);
        }
        //Quantity breakdown
        private void button5_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]), editBox1.Text);
            frm.ShowDialog(this);
        }
        //ColorComb
        private void button12_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["ID"].ToString(), "Orders", "ID");
            var frm = new Sci.Production.PublicForm.ColorCombination(CurrentMaintain["ID"].ToString(), ukey);
            frm.ShowDialog(this);
        }
        //ProductionKit
        private void button11_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.PPIC.P01_ProductionKit(true, StyleUkey, null, null, null);
            frm.ShowDialog(this);
            OnDetailEntered();
        }
        //Garment List
        private void button9_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["ID"].ToString(), "Orders", "ID");
            var frm = new Sci.Production.PublicForm.GarmentList(ukey, CurrentMaintain["ID"].ToString());
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
    }
}
