using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Win.Tools;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B01
    /// </summary>
    public partial class B01 : Win.Tems.Input6
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings col_ShipMode = new DataGridViewGeneratorTextColumnSettings();
            col_ShipMode.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    string sqlitem = "Select distinct ID from ShipMode WITH (NOLOCK) where Junk = 0";
                    SelectItem item = new SelectItem(sqlitem, "20", dr["ShipModeID"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ShipModeID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            col_ShipMode.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["ShipModeID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode || e.RowIndex == -1 || oldvalue.Equals(newvalue))
                {
                    return;
                }

                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    string cmd = "Select ID from ShipMode WITH (NOLOCK) where Junk = 0 and ID = @ShipModeID";
                    List<SqlParameter> spam = new List<SqlParameter>
                    {
                        new SqlParameter("@ShipModeID", e.FormattedValue),
                    };

                    if (!MyUtility.Check.Seek(cmd, spam))
                    {
                        dr["ShipModeID"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"<Ship Mode: {e.FormattedValue}> data not found!");
                    }
                    else
                    {
                        dr["ShipModeID"] = e.FormattedValue;
                    }

                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorTextColumnSettings col_LoadingType = new DataGridViewGeneratorTextColumnSettings();
            col_LoadingType.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    string sqlitem = "Select distinct LoadingType from ShipMode WITH (NOLOCK) where Junk = 0 and LoadingType !=''";
                    SelectItem2 item = new SelectItem2(sqlitem, headercaptions: "Loading Type", columnwidths: "20", defaults: string.Empty);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["LoadingType"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(30), settings: col_ShipMode)
                .Text("LoadingType", header: "Loading Type", width: Widths.AnsiChars(30), iseditingreadonly: true, settings: col_LoadingType)
                ;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["WhseCode"]))
            {
                this.txtWhseCode.Focus();
                MyUtility.Msg.WarningBox("< Port/WH Code > cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["WhseName"].ToString()))
            {
                this.txtWhseName.Focus();
                MyUtility.Msg.WarningBox("< Name > cannot be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Address"].ToString()))
            {
                this.editAddress.Focus();
                MyUtility.Msg.WarningBox("< Address > cannot be empty!");
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Details data can't empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void editPOCombo_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                string sqlWhere = "SELECT Id,NameCH,NameEN FROM Production.dbo.Brand WITH (NOLOCK) WHERE Junk=0  ORDER BY Id";
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, headercaptions: "Id", columnwidths: "10,29,35", defaults: this.editBrandID.Text)
                {
                    Size = new System.Drawing.Size(810, 666),
                };

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.editBrandID.Text = item.GetSelectedString();
            }
        }

        private void editBrandID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string str = this.editBrandID.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.editBrandID.OldValue)
            {
                string[] str_multi = str.Split(',');
                if (str_multi.Length > 1)
                {
                    string err_brand = string.Empty;
                    foreach (string chk_str in str_multi)
                    {
                        if (MyUtility.Check.Seek(chk_str, "Brand", "id", "Production") == false)
                        {
                            err_brand += "," + chk_str;
                        }
                    }

                    if (!err_brand.Equals(string.Empty))
                    {
                        this.editBrandID.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Brand : {0} > not found!!!", err_brand.Substring(1)));
                        return;
                    }
                }
                else
                {
                    if (MyUtility.Check.Seek(str, "Brand", "id", "Production") == false)
                    {
                        this.editBrandID.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Brand : {0} > not found!!!", str));
                        return;
                    }
                }
            }
        }
    }
}
