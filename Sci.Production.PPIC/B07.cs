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
            string sqlCommand = "select UseAPS from factory WITH (NOLOCK) where ID = '" + Sci.Env.User.Factory + "'";
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
            btnBatchEdit.Enabled = CurrentMaintain == null ? false : true;
            if (!IsSupportNew)
            {
                btnBatchEdit.Visible = false;
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayDay.Text = MyUtility.Check.Empty(CurrentMaintain["Date"])?"": Convert.ToDateTime(CurrentMaintain["Date"]).ToString("dddd");
            this.btnBatchEdit.ForeColor = Color.Blue;
            btnBatchEdit.Enabled = CurrentMaintain == null ? false : true;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateDate.ReadOnly = true;
            this.txtSewingLine.ReadOnly = true;
            this.txtSewingLine.ReadOnly = false;
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
            this.txtSewingLine.ReadOnly = true;
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtSewingLine.ReadOnly = true;
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
