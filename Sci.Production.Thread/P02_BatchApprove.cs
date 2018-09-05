using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P02_BatchApprove
    /// </summary>
    public partial class P02_BatchApprove : Sci.Win.Subs.Base
    {
        /// <summary>
        /// P02_BatchApprove
        /// </summary>
        public P02_BatchApprove()
        {
            this.InitializeComponent();
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchApprove.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridBatchApprove)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("MDivisionid", header: "MDivisionid", iseditingreadonly: true)
            .Text("Status", header: "Status", iseditingreadonly: true)
            .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("BrandID", header: "Brand", iseditingreadonly: true)
            .Text("FactoryID", header: "Factory", iseditingreadonly: true)
            .Text("StyleID", header: "Style", iseditingreadonly: true)
            .Text("SeasonID", header: "Season", iseditingreadonly: true)
            .Date("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
            .Date("SewInLine", header: "SewInLine", iseditingreadonly: true)
            .Date("EstBookDate", header: "EstBookDate", iseditingreadonly: true)
            .Text("AddName", header: "AddName", iseditingreadonly: true)
            .Text("EditName", header: "EditName", iseditingreadonly: true)
            ;
        }

        private void BtntoExcel_Click(object sender, EventArgs e)
        {
            DataTable gridData = ((DataTable)this.listControlBindingSource1.DataSource).Copy();

            if (gridData == null || gridData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            gridData.Columns.Remove("Selected");

            MyUtility.Excel.CopyToXls(gridData, string.Empty, xltfile: "Thread_P02_BatchApprove.xltx", headerRow: 2);
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.QueryData();
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            List<string> orderlist = new List<string>();
            foreach (DataRow item in ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = '1'"))
            {
                orderlist.Add($"'{MyUtility.Convert.GetString(item["OrderID"])}'");
            }

            string orderids = string.Join(",", orderlist);
            if (MyUtility.Check.Empty(orderids))
            {
                MyUtility.Msg.WarningBox("No data selected!");
                return;
            }

            string updSql = $"update ThreadRequisition set Status = 'Approved' ,editname='{Sci.Env.User.UserID}', editdate = GETDATE() where orderid in({orderids})";

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                    this.QueryData();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }

        private void QueryData()
        {
            this.listControlBindingSource1.DataSource = null;

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtbrand1.Text)) where += $" and tr.BrandID = '{this.txtbrand1.Text}'";
            if (!MyUtility.Check.Empty(this.txtstyle1.Text)) where += $" and tr.StyleID = '{this.txtstyle1.Text}'";
            if (!MyUtility.Check.Empty(this.txtfactory1.Text)) where += $" and tr.FactoryID = '{this.txtfactory1.Text}'";
            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1)) where += $" and o.SciDelivery between '{this.dateSCIDelivery.Text1}' and '{this.dateSCIDelivery.Text2}'";
            if (!MyUtility.Check.Empty(this.dateSewingInLine.Value1)) where += $" and o.SewInLine between '{this.dateSewingInLine.Text1}' and '{this.dateSewingInLine.Text2}'";
            if (!MyUtility.Check.Empty(this.dateEstBooking.Value1)) where += $" and tr.EstBookDate between '{this.dateEstBooking.Text1}' and '{this.dateEstBooking.Text2}'";
            if (!MyUtility.Check.Empty(this.dateEstArrived.Value1)) where += $" and tr.EstArriveDate between '{this.dateEstArrived.Text1}' and '{this.dateEstArrived.Text2}'";
            string sqlcmd = $@"
select 
	selected = 0,
	tr.MDivisionid,
	tr.Status,
	tr.OrderID,
	tr.BrandID,
	tr.FactoryID,
	tr.StyleID,
	tr.SeasonID,
	SciDelivery = o.SciDelivery,
	SewInLine = o.SewInLine,
	tr.EstBookDate,
	tr.AddName,
	tr.EditName
from ThreadRequisition tr
left join orders o with(nolock) on o.id = tr.OrderID
where tr.Status <> 'Approved' and tr.MDivisionid = '{Sci.Env.User.Keyword}'
{where}
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
