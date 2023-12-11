﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B62 : Sci.Win.Tems.Input6
    {
        /// <inheritdoc/>
        public B62(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        private string GetRefNoSqlCmd()
        {
            string sqlcmd = @"
select Refno, SCIRefno ,Description
from view_KHImportItem 
where 1=1
";
            switch (this.comboCustomsType.Text)
            {
                case "Fabric":
                    sqlcmd += Environment.NewLine + "and CustomsType in ('Fabric', 'Accessory')";
                    break;
                case "Accessory":
                    sqlcmd += Environment.NewLine + "and CustomsType in ('Fabric', 'Accessory')";
                    break;
                case "Machine":
                    sqlcmd += Environment.NewLine + "and CustomsType in ('Machine')";
                    break;
                case "Chemical":
                    sqlcmd += Environment.NewLine + "and CustomsType in ('Chemical')";
                    break;
            }

            return sqlcmd;
        }

        private void TxtCustomsDesc_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode)
            {
                string sqlcmd = $@"select CDCCode,CDCName,CDCUnit from KHCustomsDescription where junk=0 and CustomsType = '{this.CurrentMaintain["CustomsType"]}'";
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, string.Empty, this.CurrentMaintain["KHCustomsDescriptionCDCName"].ToString());
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["KHCustomsDescriptionCDCName"] = item.GetSelecteds()[0]["CDCName"];
                this.CurrentMaintain["CDCCode"] = item.GetSelecteds()[0]["CDCCode"];
                this.txtCDCUnit.Text = item.GetSelecteds()[0]["CDCUnit"].ToString();
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings col_Port = new DataGridViewGeneratorTextColumnSettings();
            col_Port.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null || !this.EditMode)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"select ID, Name from Port where CountryID ='KH' and Junk =0 ";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "15,20", this.CurrentDetailData["Port"].ToString(), "ID, Name");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    dr["Port"] = item.GetSelecteds()[0]["ID"];
                    dr.EndEdit();
                }
            };

            col_Port.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null || !this.EditMode || e.FormattedValue.Empty())
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string sqlcmd = $@"select ID, Name from Port where CountryID ='KH' and id ='{dr["Port"]}' and Junk =0 ";

                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox("No this port.");
                    e.Cancel = true;
                    return;
                }

                dr["Port"] = e.FormattedValue;
                dr.EndEdit();
            };

            base.OnDetailGridSetup();
            #region 欄位設定

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Port", header: "Port", width: Widths.AnsiChars(12), iseditingreadonly: false, settings: col_Port)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(14), iseditingreadonly: false)
                ;
            #endregion

        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.txtCustomsDesc.BackColor = Color.FromArgb(183, 227, 255);
            this.txtCustomsDesc.ForeColor = Color.FromArgb(0, 0, 255);
            base.OnDetailEntered();

            if (!MyUtility.Check.Empty(this.CurrentMaintain))
            {
                string sqlcmd = $@"select CDCUnit from KHCustomsDescription where junk=0 and CustomsType = '{this.CurrentMaintain["CustomsType"]}'";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                {
                    this.txtCDCUnit.Text = dr["CDCUnit"].ToString();
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["RefNo"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["KHCustomsDescriptionCDCName"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["CDCUnitPrice"]))
            {
                MyUtility.Msg.WarningBox(@"< Ref#>, < Customer Description >, <CDC Unit Price(USD)>cannot be empty.");
                return false;
            }

            string sqlcmd = $@"select 1 from KHCustomsItem where RefNo='{this.CurrentMaintain["RefNo"]}' and ukey != '{this.CurrentMaintain["ukey"]}'";
            if (MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox($@"Save failed. [Refno#]: {this.CurrentMaintain["RefNo"]} has already existed.");
                return false;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                // Port HSCode不可為空值
                if (MyUtility.Check.Empty(dr["Port"]) || MyUtility.Check.Empty(dr["HSCode"]))
                {
                    MyUtility.Msg.WarningBox("[Port] and [HS Code] cannot be empty!");
                    return false;
                }

                // 判斷Port,HSCOde是否重複
                DataRow[] drCheck = ((DataTable)this.detailgridbs.DataSource).Select($@"Port = '{dr["Port"]}' and HSCode = '{dr["HSCode"]}'");
                if (drCheck.Length >= 2)
                {
                    MyUtility.Msg.WarningBox(@"Save failed. Multiple same [Port] and [HS Code]");
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.txtCustomsDesc.BackColor = Color.White;
            this.txtCustomsDesc.ForeColor = Color.Red;
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.txtCustomsDesc.BackColor = Color.White;
            this.txtCustomsDesc.ForeColor = Color.Red;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();

            if (this.DialogResult == DialogResult.OK)
            {
                this.txtCustomsDesc.BackColor = Color.FromArgb(183, 227, 255);
                this.txtCustomsDesc.ForeColor = Color.FromArgb(0, 0, 255);
            }
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            this.txtCustomsDesc.BackColor = Color.FromArgb(183, 227, 255);
            this.txtCustomsDesc.ForeColor = Color.FromArgb(0, 0, 255);
            base.ClickSaveAfter();
        }

        private void TxtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["CustomsType"]))
            {
                MyUtility.Msg.InfoBox("Please select [Customs Type] first!");
                this.comboCustomsType.Select();
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode)
            {
                string sqlcmd = this.GetRefNoSqlCmd();
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "20,25,50", this.txtRefno.Text, "Refno,SCIRefno,Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["Refno"] = item.GetSelecteds()[0]["SCIRefno"];
                this.CurrentMaintain["Description"] = item.GetSelecteds()[0]["Description"];
                this.txtRefno.Text = item.GetSelecteds()[0]["Refno"].ToString();
                this.CurrentMaintain["vk_Refno"] = item.GetSelecteds()[0]["Refno"].ToString();
            }
        }

        private void TxtRefno_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtRefno.Text))
            {
                this.CurrentMaintain["vk_Refno"] = string.Empty;
                this.CurrentMaintain["Refno"] = string.Empty;
            }

            if (this.CurrentMaintain["vk_Refno"].ToString() == this.txtRefno.Text)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CustomsType"]))
            {
                MyUtility.Msg.InfoBox("Please select [Customs Type] first!");
                this.txtSCIRefno.Text = string.Empty;
                this.comboCustomsType.Select();
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode && !MyUtility.Check.Empty(this.txtRefno.Text))
            {
                string sqlcmd = this.GetRefNoSqlCmd();
                sqlcmd += Environment.NewLine + $" and Refno= '{this.txtRefno.Text}'";

                if (!MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                {
                    MyUtility.Msg.WarningBox("Cannot find this [Refno].");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    this.CurrentMaintain["vk_Refno"] = dr["Refno"].ToString();
                    this.CurrentMaintain["Refno"] = dr["SCIRefno"].ToString();
                    this.CurrentMaintain["Description"] = dr["Description"];
                    this.CurrentMaintain.EndEdit();
                }
            }
        }
    }
}
