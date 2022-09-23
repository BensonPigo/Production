using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// ShippingMemo
    /// </summary>
    public partial class ShippingMemo : Win.Subs.Base
    {
        /// <summary>
        /// ShippingMemoType
        /// </summary>
        public enum ShippingMemoType
        {
            Export_ShippingMemo = 1,
            FtyExport_ShippingMemo = 2,
            GMTBooking_ShippingMemo = 3,
        }

        private ShippingMemoType shippingMemoType;
        private string id;

        /// <summary>
        /// ShippingMemo
        /// </summary>
        /// <param name="shippingMemoType">shippingMemoType</param>
        /// <param name="ID">ID</param>
        public ShippingMemo(ShippingMemoType shippingMemoType, string ID)
        {
            this.InitializeComponent();

            this.shippingMemoType = shippingMemoType;
            this.id = ID;

            switch (shippingMemoType)
            {
                case ShippingMemoType.Export_ShippingMemo:
                    this.Text = "P03. Shipping Memo";
                    break;
                case ShippingMemoType.FtyExport_ShippingMemo:
                    this.Text = "P04. Shipping Memo";
                    break;
                case ShippingMemoType.GMTBooking_ShippingMemo:
                    this.Text = "P05. Shipping Memo";
                    break;
                default:
                    break;
            }

            this.EditMode = false;
            this.btnSave.Text = "Edit";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridShippingMemo)
                .CheckBox("ShippingExpense", header: "Shipping Expense", trueValue: 1, falseValue: 0, iseditable: true)
                .Text("Subject", header: "Subject", width: Widths.AnsiChars(13), iseditingreadonly: false)
                .Text("AddName", header: "Create by", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("AddDate", header: "Create Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("EditName", header: "Edit by", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.gridShippingMemo.Columns["ShippingExpense"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridShippingMemo.Columns["Subject"].DefaultCellStyle.BackColor = Color.Pink;
            this.Query();
        }

        /// <summary>
        /// IsDataExists
        /// </summary>
        /// <param name="shippingMemoType">shippingMemoType</param>
        /// <param name="ID">ID</param>
        /// <returns>bool</returns>
        public static bool IsDataExists(ShippingMemoType shippingMemoType, string ID)
        {
            return MyUtility.Check.Seek($"select 1 from {shippingMemoType} sm with (nolock)where   ID = '{ID}'");
        }

        private void Query()
        {
            string sqlGetData = $@"
select  sm.Ukey
        ,sm.ID
        ,[ShippingExpense] = cast(sm.ShippingExpense as int)
        ,sm.Subject
        ,sm.Description
        ,[AddName] = (select ID + '-' + Name from Pass1 with (nolock) where ID = sm.AddName)
        ,sm.AddDate
        ,[EditName] = (select ID + '-' + Name from Pass1 with (nolock) where ID = sm.EditName)
        ,sm.EditDate
from    {this.shippingMemoType} sm with (nolock)
where   ID = '{this.id}'
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridShippingMemo.DataSource = dtResult;
        }

        private void GridShippingMemo_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridShippingMemo.SelectedRows.Count == 0)
            {
                return;
            }

            this.editDesc.Text = this.gridShippingMemo.GetDataRow(this.gridShippingMemo.GetSelectedRowIndex())["Description"].ToString();
        }

        private void EditDesc_Validating(object sender, CancelEventArgs e)
        {
            if (this.gridShippingMemo.SelectedRows.Count == 0)
            {
                return;
            }

            DataRow drSelected = this.gridShippingMemo.GetDataRow(this.gridShippingMemo.GetSelectedRowIndex());
            if (this.editDesc.Text == drSelected["Description"].ToString())
            {
                return;
            }

            drSelected["Description"] = this.editDesc.Text;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Text == "Edit")
            {
                this.EditMode = true;
                this.btnSave.Text = "Save";
            }

            DataTable dtShippingMemo = (DataTable)this.gridShippingMemo.DataSource;
            string sqlUpdateShippingMemo = string.Empty;

            foreach (DataRow dr in dtShippingMemo.Rows)
            {
                switch (dr.RowState)
                {

                    case DataRowState.Detached:
                        break;
                    case DataRowState.Unchanged:
                        break;
                    case DataRowState.Added:
                        sqlUpdateShippingMemo += $@"
insert into {this.shippingMemoType}(ID, ShippingExpense, Subject, Description, AddName, AddDate)
            values('{this.id}', {dr["ShippingExpense"]}, '{dr["Subject"]}', '{dr["Description"]}', '{Env.User.UserID}', getdate())
";
                        break;
                    case DataRowState.Deleted:
                        sqlUpdateShippingMemo += $@"
delete {this.shippingMemoType} where Ukey = '{dr["UKey", DataRowVersion.Original]}'
";
                        break;
                    case DataRowState.Modified:
                        sqlUpdateShippingMemo += $@"
update  {this.shippingMemoType} set ShippingExpense = {dr["ShippingExpense"]},
                                    Subject = '{dr["Subject"]}',
                                    Description = '{dr["Description"]}',
                                    EditName = '{Env.User.UserID}',
                                    EditDate = getdate()
where Ukey = '{dr["UKey"]}'
";
                        break;
                    default:
                        break;
                }
            }

            if (!MyUtility.Check.Empty(sqlUpdateShippingMemo))
            {
                DualResult result = DBProxy.Current.Execute(null, sqlUpdateShippingMemo);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.EditMode = false;
                this.btnSave.Text = "Edit";
                this.Query();
            }
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            DataTable dtSource = (DataTable)this.gridShippingMemo.DataSource;
            DataRow drNewRow = dtSource.NewRow();
            drNewRow["ShippingExpense"] = false;
            dtSource.Rows.Add(drNewRow);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.gridShippingMemo.SelectedRows.Count == 0)
            {
                return;
            }

            this.gridShippingMemo.GetDataRow(this.gridShippingMemo.GetSelectedRowIndex()).Delete();
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            this.EditMode = false;
            this.btnSave.Text = "Edit";
            this.Query();
        }
    }
}
