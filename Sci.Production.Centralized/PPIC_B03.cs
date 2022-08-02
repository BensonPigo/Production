using Ict;
using Ict.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class PPIC_B03 : Sci.Win.Tems.Input1
    {
        private DataTable dt = new DataTable();

        /// <inheritdoc/>
        public PPIC_B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"exists (select 1 from Trade.dbo.Factory where ID = MailGroup.FactoryID)";
            this.dt.Columns.Add("ToAddress", typeof(string));
            this.listControlBindingSource1.DataSource = this.dt;
            this.GridSetup();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorTextColumnSettings toAddress = new DataGridViewGeneratorTextColumnSettings();
            toAddress.CellValidating += (s, e) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridMailto.GetDataRow(e.RowIndex);
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!regex.IsMatch(e.FormattedValue.ToString()))
                {
                    dr["ToAddress"] = string.Empty;
                    dr.EndEdit();
                    MyUtility.Msg.WarningBox("Invalid email !!");
                    return;
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridMailto)
                .Text("ToAddress", header: "Mail", settings: toAddress)
                ;
            this.gridMailto.Columns[0].Width = this.gridMailto.Width - 5;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.gridMailto.IsEditingReadOnly = !this.EditMode;
            this.dt.Clear();
            foreach (string toAddress in MyUtility.Convert.GetString(this.CurrentMaintain["ToAddress"]).Split(';'))
            {
                DataRow newdr = this.dt.NewRow();
                newdr["ToAddress"] = toAddress;
                this.dt.Rows.Add(newdr);
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Code"] = "P30";
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.txtFactory.ReadOnly = true;
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtFactory.Text))
            {
                MyUtility.Msg.WarningBox("Factory can't empty");
                return false;
            }

            if (this.IsDetailInserting)
            {
                string sqlcmd = $@"select 1 from MailGroup where FactoryID = '{this.CurrentMaintain["FactoryID"]}' and Code = 'P30'";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox("<Factory> is duplicate in database. ");
                    return false;
                }
            }

            this.CurrentMaintain["ToAddress"] = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable()
                .Where(w => !MyUtility.Check.Empty(w["ToAddress"]))
                .Select(s => MyUtility.Convert.GetString(s["ToAddress"]).ToLower())
                .Distinct()
                .JoinToString(";");
            return base.ClickSaveBefore();
        }

        private void GridIcon1_AppendClick(object sender, EventArgs e)
        {
            this.dt.Rows.Add();
        }

        private void GridIcon1_InsertClick(object sender, EventArgs e)
        {
            this.dt.AcceptChanges();
            DataRow newrow = this.dt.NewRow();
            this.dt.Rows.InsertAt(newrow, this.gridMailto.GetSelectedRowIndex());
        }

        private void GridIcon1_RemoveClick(object sender, EventArgs e)
        {
            if (this.gridMailto.CurrentDataRow != null)
            {
                this.gridMailto.CurrentDataRow.Delete();
            }
        }

        private void BtnBatchImport_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"select ID, Name, Email from Pass1 where EMail like'%@%' and Resign is null";
            SelectItem2 item = new SelectItem2(sqlcmd, string.Empty, string.Empty);
            item.Width = 777;
            if (item.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            foreach (DataRow selectitem in item.GetSelecteds())
            {
                if (this.dt.Select($"ToAddress = '{selectitem["Email"]}'").Length == 0)
                {
                    DataRow newdr = this.dt.NewRow();
                    newdr["ToAddress"] = selectitem["Email"];
                    this.dt.Rows.Add(newdr);
                }
            }
        }

        private void TxtFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = @"
SELECT FtyZone as Factory 
FROM TradeDB.Trade.dbo.Factory 
WHERE JUNK=0 
AND ISNULL(FtyZone,'') != ''
GROUP BY CountryID, FtyZone
ORDER BY charindex(CountryID,'PH,VN,KH,CN,TW') ,FtyZone
";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "8", this.txtFactory.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtFactory.Text = item.GetSelectedString();
            this.txtFactory.ValidateControl();
        }

        private void TxtFactory_Validating(object sender, CancelEventArgs e)
        {
            string fty = this.txtFactory.Text;
            string sqlcmd = $@"
SELECT FtyZone as Factory 
FROM TradeDB.Trade.dbo.Factory 
WHERE JUNK=0 
and FtyZone = '{fty}'
AND ISNULL(FtyZone,'') != ''
";

            if (!MyUtility.Check.Seek(sqlcmd))
            {
                this.txtFactory.Text = string.Empty;
                e.Cancel = true;

                MyUtility.Msg.WarningBox($"<Factory : {fty}> not found!");
            }
        }
    }
}
