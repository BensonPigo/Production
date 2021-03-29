using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P03
    /// </summary>
    public partial class P03 : Win.Tems.QueryForm
    {
        private DataGridViewGeneratorDateColumnSettings rcvDate = new DataGridViewGeneratorDateColumnSettings();
        private DataGridViewGeneratorTextColumnSettings ftyRemark = new DataGridViewGeneratorTextColumnSettings();
        private bool needSave = false;
        private bool alreadySave = false;
        private DataTable gridData;

        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.QueryData();

            this.ftyRemark.CharacterCasing = CharacterCasing.Normal;
            this.rcvDate.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridProductionKitsConfirm.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (Convert.ToDateTime(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddDays(180) || Convert.ToDateTime(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-180)))
                {
                    dr["ReceiveDate"] = DBNull.Value;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< FTY MR Rcv date > is invalid, it exceeds +/-180 days!!");
                    return;
                }
            };

            this.gridProductionKitsConfirm.DataSource = this.listControlBindingSource1;
            this.gridProductionKitsConfirm.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridProductionKitsConfirm)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .EditText("Article", header: "Colorway", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Text("ReasonName", header: "Doc", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Date("SendDate", header: "TW Send date", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("ReceiveDate", header: "FTY MR Rcv date", width: Widths.AnsiChars(8), settings: this.rcvDate)
                .Text("FtyRemark", header: "Remark for factory", width: Widths.AnsiChars(15), settings: this.ftyRemark)
                .Date("ProvideDate", header: "Provide date", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Delivery", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("MRName", header: "MR", width: Widths.AnsiChars(38), iseditingreadonly: true)
                .Text("SMRName", header: "SMR", width: Widths.AnsiChars(38), iseditingreadonly: true)
                .Text("POHandleName", header: "PO Handle", width: Widths.AnsiChars(38), iseditingreadonly: true)
                .Text("POSMRName", header: "PO SMR", width: Widths.AnsiChars(38), iseditingreadonly: true)
                .DateTime("MRLastDate", header: "MR Last Updated", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;

            this.gridProductionKitsConfirm.Columns["ReceiveDate"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridProductionKitsConfirm.Columns["FtyRemark"].DefaultCellStyle.BackColor = Color.LightYellow;
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.gridProductionKitsConfirm.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    this.needSave = true;
                    break;
                }
            }

            if (this.needSave && !this.alreadySave)
            {
                DialogResult buttonResult = MyUtility.Msg.QuestionBox("Do you want to save data?", "Confirm", MessageBoxButtons.OKCancel);
                if (buttonResult == DialogResult.OK)
                {
                    if (!this.SaveData())
                    {
                        return;
                    }
                }

                this.needSave = false;
            }

            this.QueryData();
        }

        private void QueryData()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"select sp.*,s.ID as StyleID,s.SeasonID,
(select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'ProductionKits' and ID = sp.DOC) as ReasonName,
isnull((sp.MRHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.MRHandle)),sp.MRHandle) as MRName,
isnull((sp.SMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.SMR)),sp.SMR) as SMRName,
isnull((sp.PoHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.PoHandle)),sp.PoHandle) as POHandleName,
isnull((sp.POSMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.POSMR)),sp.POSMR) as POSMRName,
iif(sp.IsPF = 1,'Y','N') as CPF
,sp.MRLastDate
from Style_ProductionKits sp WITH (NOLOCK) 
left join Style s WITH (NOLOCK) on s.Ukey = sp.StyleUkey
where sp.ReceiveDate is null and sp.SendDate is not null and ReasonID=''
and sp.ProductionKitsGroup='{Env.User.Keyword}'
");

            if (!MyUtility.Check.Empty(this.txtStyleNo.Text))
            {
               sqlCmd.Append(string.Format(" and s.ID = '{0}'", this.txtStyleNo.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSeason.Text))
            {
               sqlCmd.Append(string.Format(" and s.SeasonID = '{0}'", this.txtSeason.Text));
            }

            if (!MyUtility.Check.Empty(this.dateSendDate.Value))
            {
               sqlCmd.Append(string.Format(" and sp.SendDate = '{0}'", Convert.ToDateTime(this.dateSendDate.Value).ToString("d")));
            }

            sqlCmd.Append(@" order by FactoryID, StyleID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);

            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.alreadySave = false;
        }

        // Batch update
        private void BtnBatchUpdate_Click(object sender, EventArgs e)
        {
           foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(this.dateFactoryReceiveDate.Value))
                {
                    dr["ReceiveDate"] = DBNull.Value;
                }
                else
                {
                    dr["ReceiveDate"] = this.dateFactoryReceiveDate.Value;
                }
            }
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.SaveData();
        }

        private bool SaveData()
        {
            IList<string> updateCmds = new List<string>();
            this.gridProductionKitsConfirm.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            StringBuilder cmds = new StringBuilder();
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    cmds.Clear();
                    cmds.Append(string.Format(@"update Style_ProductionKits set ReceiveDate = {0}, FtyRemark = '{1}'", MyUtility.Check.Empty(dr["ReceiveDate"]) ? "null" : "'" + Convert.ToDateTime(dr["ReceiveDate"]).ToString("d") + "'", dr["FtyRemark"].ToString()));
                    if (!MyUtility.Check.Empty(dr["ReceiveDate"]))
                    {
                        cmds.Append(string.Format(@", FtyHandle = '{0}', FtyLastDate = GETDATE()", Env.User.UserID));
                    }

                    cmds.Append(string.Format(" where Ukey = {0}", dr["UKey"].ToString()));

                    updateCmds.Add(cmds.ToString());
                }
            }

            if (updateCmds.Count != 0)
            {
                DualResult result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Save Fail!" + result.ToString());
                    return false;
                }
            }

            this.alreadySave = true;
            return true;
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // View Detail
        private void BtnViewDetail_Click(object sender, EventArgs e)
        {
            this.gridProductionKitsConfirm.ValidateControl();
            P03_Detail doForm = new P03_Detail();
            doForm.Set(false, ((DataTable)this.listControlBindingSource1.DataSource).ToList(), this.gridProductionKitsConfirm.GetDataRow(this.gridProductionKitsConfirm.GetSelectedRowIndex()));
            doForm.ShowDialog(this);
        }

        // [send Date]改變時，同步更新GRID資料
        private void DateSendDate_ValueChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateSendDate.Value))
            {
                this.listControlBindingSource1.Filter = string.Empty;
            }
            else
            {
                this.listControlBindingSource1.Filter = string.Format("SendDate='{0}'", this.dateSendDate.Text);
            }
        }
    }
}
