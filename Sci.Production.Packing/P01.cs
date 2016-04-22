using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        public P01(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            this.Text = Type == "1" ? "P01. Packing Master List" : "P011. Packing Master List (History)";
            this.DefaultFilter = Type == "1" ? string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND GMTClose is null", Sci.Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND GMTClose is not null", Sci.Env.User.Keyword);
            txtcountry1.TextBox1.ReadOnly = true;
            txtcountry1.TextBox1.IsSupportEditMode = false;
            txtuser1.TextBox1.ReadOnly = true;
            txtuser1.TextBox1.IsSupportEditMode = false;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            displayBox6.Value = MyUtility.GetValue.Lookup(string.Format("select Description from style where Ukey = {0}", CurrentMaintain["StyleUkey"].ToString()));
            displayBox7.Value = MyUtility.GetValue.Lookup(string.Format("select [dbo].getPOComboList('{0}','{1}') as PoList from Orders where ID = '{0}'", CurrentMaintain["ID"].ToString(), CurrentMaintain["POID"].ToString()));
            button1.Enabled = CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["CtnType"]) == "2" && !EditMode;
            //按鈕變色
            if (MyUtility.Convert.GetString(CurrentMaintain["CtnType"]) == "2")
            {
                button1.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyCTN where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            }
            button2.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button3.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;
            button4.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail where OrderID = '{0}' and ReceiveDate is not null", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button5.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail where PoID = '{0}'", CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
            button6.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_CTNData where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            bool gmtClose = !MyUtility.Check.Empty(CurrentMaintain["GMTClose"]);
            button7.Visible = gmtClose;
            if (gmtClose)
            {
                button7.ForeColor = MyUtility.Check.Seek(string.Format("select ID from OverrunGMT where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            }
        }

        //b'down
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyCTN callNextForm = new Sci.Production.PPIC.P01_QtyCTN(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Quantity breakdown
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(displayBox7.Value));
            callNextForm.ShowDialog(this);
        }

        //Packing method
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["Packing"].ToString(), "Packing method", false, null);
            callNextForm.ShowDialog(this);
        }

        //Carton Status
        private void button4_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(CurrentMaintain["ID"].ToString(), false);
            callNextForm.ShowDialog(this);
        }

        //Material import
        private void button5_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_MTLImport callNextForm = new Sci.Production.PPIC.P01_MTLImport(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Carton Booking
        private void button6_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P01_CTNData callNextForm = new Sci.Production.Packing.P01_CTNData(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Overrun garment record
        private void button7_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P01_OverrunGMTRecord callNextForm = new Sci.Production.Packing.P01_OverrunGMTRecord(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }
    }
}
