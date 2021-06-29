﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B07
    /// </summary>
    public partial class B07 : Win.Tems.Input1
    {
        /// <summary>
        /// B07
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string sqlCommand = "select UseAPS from factory WITH (NOLOCK) where ID = '" + Env.User.Factory + "'";
            string useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "TRUE")
            {
                this.IsSupportDelete = false;
                this.IsSupportEdit = false;
                this.IsSupportNew = false;
            }

            this.InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Env.User.Factory + "'";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.btnBatchEdit.Enabled = this.CurrentMaintain == null ? false : true;
            if (!this.IsSupportNew)
            {
                this.btnBatchEdit.Visible = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayDay.Text = MyUtility.Check.Empty(this.CurrentMaintain["Date"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["Date"]).ToString("dddd");
            this.btnBatchEdit.ForeColor = Color.Blue;
            this.btnBatchEdit.Enabled = this.CurrentMaintain == null ? false : true;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateDate.ReadOnly = true;
            this.txtSewingLine.ReadOnly = true;
            this.txtSewingLine.ReadOnly = false;
        }

        /// <inheritdoc/>
        protected override bool ClickNewBefore()
        {
            B07_Add callNextForm = new B07_Add();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                this.ReloadDatas();
            }

            return false;
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.txtSewingLine.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtSewingLine.ReadOnly = true;
        }

        private void BtnBatchEdit_Click(object sender, EventArgs e)
        {
            B07_BatchAdd callNextForm = new B07_BatchAdd(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                // 紀錄目前畫面資料，Reload Data後，資料要保留在Reload前的那一筆
                DataRow currentData = this.CurrentMaintain;
                this.ReloadDatas();
                IList<DataRow> list = this.DataRows.ToList();
                DataRow dr = list.FirstOrDefault(x => x["FactoryID"].ToString() == currentData["FactoryID"].ToString() && Convert.ToDateTime(x["Date"]).ToString("yyyy/MM/dd") == Convert.ToDateTime(currentData["Date"]).ToString("yyyy/MM/dd") && x["SewingLineID"].ToString() == currentData["SewingLineID"].ToString());
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
