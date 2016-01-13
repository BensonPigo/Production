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
        private string keyWord = Sci.Env.User.Keyword;

        public P01(ToolStripMenuItem menuitem,string Type)
            : base(menuitem)
        {
            this.Text = Type == "1" ? "P01. Cutting Master List" : "P011. Cutting Master List (History)";
            this.DefaultFilter = Type == "1" ? string.Format("MDivisionID = '{0}' AND Finished = 0", keyWord) : string.Format("MDivisionID = '{0}' AND Finished = 1", keyWord);
            if (Type != "1") this.button4.Enabled = false;
            InitializeComponent();
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataRow orderdr;
            if (MyUtility.Check.Seek(String.Format("Select * from Orders where id='{0}'", CurrentMaintain["ID"]), out orderdr))
            {
                displayBox2.Text = orderdr["Styleid"].ToString();
                displayBox4.Text = orderdr["Seasonid"].ToString();
                displayBox7.Text = orderdr["Category"].ToString();
                displayBox3.Text = orderdr["Projectid"].ToString();
                displayBox5.Text = orderdr["styleUnit"].ToString();
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
                    default :
                        displayBox8.Text = "";
                        break;
                }

            #region Cutinline,Cutoffline 是減System.Cutday計算
            int cutday = Convert.ToInt16(MyUtility.GetValue.Lookup(String.Format("Select cutday from System")));
            if (CurrentMaintain["sewinline"] == DBNull.Value) dateBox1.Value = null;
            else dateBox1.Value = Convert.ToDateTime(CurrentMaintain["sewinline"]).AddDays(cutday);

            if (CurrentMaintain["sewoffline"] == DBNull.Value) dateBox2.Value = null;
            else dateBox2.Value = Convert.ToDateTime(CurrentMaintain["sewoffline"]).AddDays(cutday);

            #endregion
            #region 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T欄位值
            DataTable OrdersData;
            string sqlCmd;
            sqlCmd = string.Format(@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList,
isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'') as CuttingList
from Orders o where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OrdersData);
            if (result)
            {
                if (OrdersData.Rows.Count > 0)
                {
                    editBox1.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["PoList"]);
                    editBox2.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["CuttingList"]);
                }
                else
                {
                    editBox1.Text = "";
                    editBox2.Text = "";
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!!" + result.ToString());
                editBox1.Text = "";
                editBox2.Text = "";
            }
            #endregion
            #region sum FOC & OrderQty
            sqlCmd = string.Format("Select isnull(sum(Qty),0) as Qty , isnull(sum(FOCQty),0) as FOC from Orders where CuttingSp = '{0}'", CurrentMaintain["ID"]);
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
            #region color change
            button1.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_MarkerList", "ID") == true ? Color.Blue : Color.Black;
            button2.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_EachCons", "ID") == true ? Color.Blue : Color.Black;
            button3.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Bundle", "CuttingId") == true ? Color.Blue : Color.Black;
            button4.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "WorkOrder", "ID") == true ? Color.Blue : Color.Black;
            button5.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_Qty", "ID") == true ? Color.Blue : Color.Black;

            button7.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "WorkOrder", "ID") == true ? Color.Blue : Color.Black;
            button8.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "WorkOrder", "ID") == true ? Color.Blue : Color.Black;
            button9.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Bundle", "CuttingId") == true ? Color.Blue : Color.Black;
            button10.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "FIR", "OrderID") == true ? Color.Blue : Color.Black;
            button11.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Bundle", "CuttingId") == true ? Color.Blue : Color.Black;
            button12.ForeColor = MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_ColorCombo", "ID") == true ? Color.Blue : Color.Black;

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Cutting.P01_MarkerList callNextForm =
    new Sci.Production.Cutting.P01_MarkerList(false, CurrentMaintain["ID"].ToString(), null, null, "Order_Markerlist", this.CurrentMaintain);
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.Cutting.P01_BundleCard callNextForm =
    new Sci.Production.Cutting.P01_BundleCard(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }
    }
}
