using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Sci.Production.PPIC
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string sqlCommand = "select UseAPS from factory where ID = '" + Sci.Env.User.Factory + "'";
            string useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "TRUE")
            {
                IsSupportDelete = false;
                IsSupportEdit = false;
                IsSupportNew = false;
            }
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            button1.Enabled = CurrentMaintain == null ? false : true;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayBox2.Text = MyUtility.Check.Empty(CurrentMaintain["Date"])?"": Convert.ToDateTime(CurrentMaintain["Date"]).ToString("dddd");
            this.button1.ForeColor = Color.Blue;
            button1.Enabled = CurrentMaintain == null ? false : true;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateBox1.ReadOnly = true;
            this.txtsewingline1.ReadOnly = true;
            this.txtsewingline1.ReadOnly = false;
        }

        protected override bool ClickNewBefore()
        {
            Sci.Production.PPIC.B07_Add callNextForm = new Sci.Production.PPIC.B07_Add();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ReloadDatas();
            }
            return false;
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.txtsewingline1.ReadOnly = true;
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtsewingline1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.B07_BatchAdd callNextForm = new Sci.Production.PPIC.B07_BatchAdd(CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //紀錄目前畫面資料，Reload Data後，資料要保留在Reload前的那一筆
                DataRow currentData = CurrentMaintain;
                ReloadDatas();
                IList<DataRow> list = DataRows.ToList<DataRow>();
                DataRow dr = list.FirstOrDefault <DataRow>(x => x["FactoryID"].ToString() == currentData["FactoryID"].ToString() && Convert.ToDateTime(x["Date"]).ToString("d") == Convert.ToDateTime(currentData["Date"]).ToString("d") && x["SewingLineID"].ToString() == currentData["SewingLineID"].ToString());
                if (dr != null)
                {
                    int pos = list.IndexOf(dr);
                    if (pos != -1)
                    {
                        this.gridbs.Position = pos;
                    }
                }
            }
        }
    }
}
