using Ict.Win;
using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Sci.Data;
using System.Transactions;
using System.Data.SqlClient;

namespace Sci.Production.Shipping
{
    public partial class P14_BatchUpdate : Sci.Win.Forms.Base
    {
        private DataTable dtBatchUpdate = new DataTable();

        public P14_BatchUpdate()
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.gridBatchUpdate.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region FtyReceiveDate
            DataGridViewGeneratorDateColumnSettings dsFtyReceiveDate = new DataGridViewGeneratorDateColumnSettings();
            dsFtyReceiveDate.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow curRow = this.gridBatchUpdate.GetDataRow(e.RowIndex);

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    curRow["FtyReceiveName"] = string.Empty;
                    curRow["FtyReceiveDate"] = DBNull.Value;
                    return;
                }

                DateTime ftyReceiveDate = (DateTime)e.FormattedValue;

                if (ftyReceiveDate < (DateTime)curRow["TPEReceiveDate"])
                {
                    e.FormattedValue = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("<Fty Receive Date> cannot be earlier than <TPE Receive Date>");
                    return;
                }

                curRow["FtyReceiveDate"] = ftyReceiveDate;
                curRow["FtyReceiveName"] = Env.User.UserID;
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridBatchUpdate)
                .CheckBox("selected", trueValue: 1, falseValue: 0, iseditable: true)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("InvoiceNo", header: "Invoice#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("FormName", header: "Form Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("FormNo", header: "Form#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("FtyReceiveDate", header: "Fty Receive Date", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: dsFtyReceiveDate)
                .Text("FtyReceiveName", header: "Fty Receive Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .EditText("FtyRemark", header: "Fty Remark", width: Widths.AnsiChars(25), iseditingreadonly: false)
                .Date("TPEReceiveDate", header: "TPE Receive Date", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.gridBatchUpdate.Columns["FtyReceiveDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchUpdate.Columns["FtyRemark"].DefaultCellStyle.BackColor = Color.Pink;

            this.GetData();
        }

        private void GetData()
        {
            string sqlGetData;
            string where = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSupplier.Text))
            {
                where += $" and md.SuppID = '{this.txtSupplier.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtInvNo.Text))
            {
                where += $" and md.InvoiceNo = '{this.txtInvNo.Text}' ";
            }

            sqlGetData = $@"
select
[selected] = 0,
md.Ukey            ,
md.ID              ,
md.SuppID          ,
md.InvoiceNo       ,
md.FormType        ,
md.FormNo          ,
md.TPEReceiveDate  ,
md.TPERemark       ,
md.Junk            ,
md.FtyReceiveDate  ,
md.FtyReceiveName  ,
md.FtyRemark       ,
md.TPEAddName      ,
md.TPEAddDate      ,
md.TPEEditName     ,
md.TPEEditDate     ,
md.FtyEditName     ,
md.FtyEditDate,
[FormName] = ft.Name
from MtlCertificate_Detail md with (nolock)
left join FormType ft with (nolock) on ft.ID = md.FormType
where md.TpeReceiveDate is not null  and FtyReceiveDate is null
{where}
";
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out this.dtBatchUpdate);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridBatchUpdate.DataSource = this.dtBatchUpdate;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.GetData();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var listSelected = this.dtBatchUpdate.AsEnumerable().Where(s => (int)s["selected"] == 1);

            if (!listSelected.Any())
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
            }

            var hasFtyReceiveDateEmpty = listSelected.Any(s => MyUtility.Check.Empty(s["FtyReceiveDate"]));
            if (hasFtyReceiveDateEmpty)
            {
                MyUtility.Msg.WarningBox("<Fty Receive Date> cannot be empty");
                return;
            }

            string sqlUpdate;
            DualResult result;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (DataRow dr in listSelected)
                    {
                        sqlUpdate = $@" update MtlCertificate_Detail set   FtyEditName = '{Env.User.UserID}', 
                                                                    FtyEditDate  = getdate(),
                                                                    FtyReceiveName = '{Env.User.UserID}',
                                                                    FtyReceiveDate = @FtyReceiveDate,
                                                                    FtyRemark = '{dr["FtyRemark"]}'
                                        where Ukey = {dr["Ukey"]}
";
                        result = DBProxy.Current.Execute(null, sqlUpdate, new List<SqlParameter>() { new SqlParameter("@FtyReceiveDate", dr["FtyReceiveDate"]) });
                        if (!result)
                        {
                            transaction.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    // 更新表頭LastEdit
                    var listMtlCertificateID = listSelected.Select(s => s["ID"].ToString()).Distinct();
                    foreach (string mtlCertificateID in listMtlCertificateID)
                    {
                        sqlUpdate = $@"update MtlCertificate set EditDate = getdate(),EditName = '{Env.User.UserID}'";
                        result = DBProxy.Current.Execute(null, sqlUpdate);
                        if (!result)
                        {
                            transaction.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    transaction.Complete();
                    transaction.Dispose();
                    MyUtility.Msg.InfoBox("Saved successfully!");
                    this.GetData();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnUpdateGridFtyRecieveDate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateFtyReceive.Text))
            {
                MyUtility.Msg.WarningBox("Left Date field can not be empty");
                return;
            }

            var listSelected = this.dtBatchUpdate.AsEnumerable().Where(s => (int)s["selected"] == 1);

            if (!listSelected.Any())
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
            }

            string errorMsg = string.Empty;
            foreach (DataRow dr in listSelected)
            {
                if (this.dateFtyReceive.Value < (DateTime)dr["TPEReceiveDate"])
                {
                    errorMsg += $"<Invoice#>{dr["InvoiceNo"]}" + Environment.NewLine;
                }
                else
                {
                    dr["FtyReceiveDate"] = this.dateFtyReceive.Value;
                    dr["FtyReceiveName"] = Env.User.UserID;
                }
            }

            if (!MyUtility.Check.Empty(errorMsg))
            {
                MyUtility.Msg.WarningBox($@"Please Check below Data,<Fty Receive Date> cannot be earlier than <TPE Receive Date>
{errorMsg}");
                return;
            }
        }
    }
}
