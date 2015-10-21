using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        public P01(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            this.Text = Type == "1" ? "P01. Clog Master List" : "P011. Clog Master List (History)";
            this.DefaultFilter = Type == "1" ? string.Format("FtyGroup = '{0}' AND IsForecast = 0 AND Finished = 0", Sci.Env.User.Factory) : string.Format("FtyGroup = '{0}' AND IsForecast = 0 AND Finished = 1", Sci.Env.User.Factory);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataRow brandData;
            displayBox5.Value = MyUtility.GetValue.Lookup(string.Format("select Description from style where Ukey = {0}",CurrentMaintain["StyleUkey"].ToString()));
            displayBox10.Value = MyUtility.GetValue.Lookup(string.Format("select [dbo].getPOComboList('{0}','{1}') as PoList from Orders where ID = '{0}'",CurrentMaintain["ID"].ToString(),CurrentMaintain["POID"].ToString()));
            if (MyUtility.Check.Seek(string.Format("select ID,Customize1,Customize2,Customize3 from Brand where ID = '{0}'", CurrentMaintain["BrandID"].ToString()), out brandData))
            {
                label19.Text = brandData["Customize1"].ToString();
                label20.Text = brandData["Customize2"].ToString();
                label21.Text = brandData["Customize3"].ToString();
            }
            else
            {
                label19.Text = "";
                label20.Text = "";
                label21.Text = "";
            }
            displayBox18.Value = CurrentMaintain["PulloutComplete"].ToString().ToUpper() == "TRUE" ? "OK" : MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct ID) as CntID from Pullout_Detail where OrderID = '{0}' and ShipQty > 0", CurrentMaintain["ID"].ToString()));
            displayBox19.Value = CurrentMaintain["InspResult"].ToString() == "P" ? "Pass" : CurrentMaintain["InspResult"].ToString() == "F" ? "Fail" : "";

            numericBox7.Value = Convert.ToInt32(CurrentMaintain["FtyCTN"]) - Convert.ToInt32(CurrentMaintain["ClogCTN"]);
            numericBox8.Value = Convert.ToInt32(CurrentMaintain["TotalCTN"]) - Convert.ToInt32(CurrentMaintain["FtyCTN"]);
            numericBox9.Value = Convert.ToDecimal(CurrentMaintain["TotalCTN"]) == 0 ? 0 : MyUtility.Math.Round(Convert.ToDecimal(CurrentMaintain["ClogCTN"]) / Convert.ToDecimal(CurrentMaintain["TotalCTN"]) * 100, 2);

            //按鈕變色
            bool haveOrder_Qty = MyUtility.Check.Seek(string.Format("select ID from Order_Qty where ID = '{0}'", CurrentMaintain["ID"].ToString()));
            button1.ForeColor = haveOrder_Qty ? Color.Blue : Color.Black;
            button2.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button3.ForeColor = haveOrder_Qty ? Color.Blue : Color.Black;
            button4.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail where OrderID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button5.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_CTNData where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button6.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail where OrderID = '{0}' and ClogReceiveID <> ''", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button7.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;
            button8.Enabled = !MyUtility.Check.Empty(CurrentMaintain["CMPQDate"]);
            if (button8.Enabled)
            {
                button8.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkFront"]) || !MyUtility.Check.Empty(CurrentMaintain["Label"]) || haveOrder_Qty || MyUtility.Check.Seek(string.Format("select ID from Order_Article where ID = '{0}'", CurrentMaintain["ID"].ToString())) || MyUtility.Check.Seek(string.Format("select ID from Order_SizeCode where ID = '{0}'", CurrentMaintain["POID"].ToString())) || MyUtility.Check.Seek(string.Format("select ID from Order_ColorCombo where ID = '{0}'", CurrentMaintain["POID"].ToString())) || MyUtility.Check.Seek(string.Format("select ID from Orders where POID = '{0}' and ID != '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            }
            else
            {
                button8.ForeColor = Color.Black;
            }
            button9.ForeColor = MyUtility.Check.Seek(string.Format("select ID from FIR where POID = '{0}'", CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
            button10.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Cfa where OrderID = '{0}'", CurrentMaintain["ID"].ToString())) || MyUtility.Check.Seek(string.Format("select OrderID from Rft where OrderID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button11.ForeColor = MyUtility.Check.Seek(string.Format("select ID from AIR where POID = '{0}'", CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
        }

        //Quantity breakdown
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //Q'ty b'down by shipmode
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Production output
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //Garment Export
        private void button4_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Carton Size
        private void button5_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P01_CTNData callNextForm = new Sci.Production.Packing.P01_CTNData(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Carton Status
        private void button6_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(CurrentMaintain["ID"].ToString(),true);
            callNextForm.ShowDialog(this);
        }

        //Order remark
        private void button7_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["OrderRemark"].ToString(), "Order Remark", false, null);
            callNextForm.ShowDialog(this);
        }

        //CMPQ sheet
        private void button8_Click(object sender, EventArgs e)
        {

        }

        //Fabric inspection list
        private void button9_Click(object sender, EventArgs e)
        {
            //等QA -> P01開發完成後呼叫
            //Sci.Production.QA.P01 callNextForm = new Sci.Production.QA.P01(CurrentMaintain["POID"].ToString());
            //callNextForm.ShowDialog(this);
        }

        //CFA && RFT list
        private void button10_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P01_CFAAndRFTList callNextForm = new Sci.Production.Logistic.P01_CFAAndRFTList(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Accessory inspection list
        private void button11_Click(object sender, EventArgs e)
        {
            //等QA -> P02開發完成後呼叫
            //Sci.Production.QA.P02 callNextForm = new Sci.Production.QA.P02(CurrentMaintain["POID"].ToString());
            //callNextForm.ShowDialog(this);
        }
    }
}
