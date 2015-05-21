using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string sqlCommand = "select UseAPS from factory where ID = '" + Sci.Env.User.Factory + "'";
            string useAPS = myUtility.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "TRUE")
            {
                IsSupportDelete = false;
                IsSupportEdit = false;
                IsSupportNew = false;
            }
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayBox2.Text = Convert.ToDateTime(CurrentMaintain["Date"]).ToString("dddd");
            this.button1.ForeColor = Color.Blue;
        }

        protected override void OnEditAfter()
        {
            base.OnEditAfter();
            this.dateBox1.ReadOnly = true;
            this.txtsewingline1.ReadOnly = true;
        }

        protected override bool OnNewBefore()
        {
            Sci.Production.PPIC.B07_Add callNextForm = new Sci.Production.PPIC.B07_Add();
            callNextForm.ShowDialog(this);
            ReloadDatas();
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.B07_BatchAdd callNextForm = new Sci.Production.PPIC.B07_BatchAdd(CurrentMaintain);
            callNextForm.ShowDialog(this);

            //紀錄目前畫面資料，Reload Data後，資料要保留在Reload前的那一筆
            DataRow currentData = CurrentMaintain;
            ReloadDatas();
            IList<DataRow> list = DataRows;
            int count = 0;
            foreach (DataRow dr in list)
            {
                if (dr["FactoryID"].ToString() == currentData["FactoryID"].ToString() && Convert.ToDateTime(dr["Date"]).ToString("d") == Convert.ToDateTime(currentData["Date"]).ToString("d") && dr["SewingLineID"].ToString() == currentData["SewingLineID"].ToString())
                {
                    break;
                }
                count++;
            }
            this.gridbs.Position = count;
        }
    }
}
