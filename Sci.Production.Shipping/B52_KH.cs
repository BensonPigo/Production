using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B52_KH : Sci.Win.Tems.Input6
    {
        public B52_KH()
        {
            InitializeComponent();
        }

        private void TxtRefno_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["CustomsType"]))
            {
                MyUtility.Msg.InfoBox("Please select [Customs Type] first!");
                this.comboCustomsType.Select();
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode && !MyUtility.Check.Empty(this.CurrentMaintain["Refno"]))
            {
                DataRow dr;
                string sqlcmd = this.GetRefNoSqlCmd();
                sqlcmd += Environment.NewLine + $" and RefNo= '{this.CurrentMaintain["Refno"]}'";

                if (!MyUtility.Check.Seek(sqlcmd, out dr))
                {
                    MyUtility.Msg.WarningBox("Cannot find this [Ref#].");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    this.CurrentMaintain["Refno"] = dr["Refno"];
                    this.CurrentMaintain["Description"] = dr["Description"];
                }
            }
        }

        private string GetRefNoSqlCmd()
        {
            string sqlcmd = @"
select  Refno , Description
from view_KHImportItem ";
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
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "Refno ,Description", "20,35", this.CurrentMaintain["Refno"].ToString(), "Refno,Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["Refno"] = item.GetSelecteds()[0]["Refno"];
                this.CurrentMaintain["Description"] = item.GetSelecteds()[0]["Description"];
            }
        }

        private void TxtCustomsDesc_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode)
            {
                string sqlcmd = @"select ID,CDCUnit from KHCustomsDescription where junk=0 ";
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "ID,CDCUnit", "20,35", this.CurrentMaintain["KHCustomsDescriptionID"].ToString(), "KHCustomsDescriptionID ,CDCUnit");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["KHCustomsDescriptionID"] = item.GetSelecteds()[0]["ID"];
                this.txtCDCUnit.Text = item.GetSelecteds()[0]["CDCUnit"].ToString();
            }
        }

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
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "ID, Name", "20,35", this.CurrentDetailData["Port"].ToString(), "ID, Name");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Port"] = item.GetSelecteds()[0]["ID"];
                }
            };

            col_Port.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null || !this.EditMode || e.FormattedValue.Empty())
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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
                .Text("Port", header: "Port", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: col_Port)
                .Numeric("Ratio", header: "1 CDC Unit : Purchase Unit", width: Widths.AnsiChars(28), decimal_places: 4, integer_places: 8, iseditingreadonly: false)
                ;
            #endregion


        }
    }
}
