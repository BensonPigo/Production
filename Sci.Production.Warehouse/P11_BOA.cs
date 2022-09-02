﻿using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P11_BOA : Win.Subs.Base
    {
        private string poid;
        private string issueid;
        private string orderid;

        /// <inheritdoc/>
        public P11_BOA(string issueid, string poid, string cutplanid, string orderid)
        {
            this.InitializeComponent();
            this.poid = poid;
            this.issueid = issueid;
            this.orderid = orderid;
            this.Text += $" ({this.poid} - {cutplanid})";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable bOA = null;
            SqlConnection sqlConnection = null;
            SqlCommand sqlCmd = null;
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = null;

            // 呼叫procedure，取得BOA展開結果
            try
            {
                DBProxy.Current.OpenConnection(null, out sqlConnection);
                sqlCmd = new SqlCommand("[dbo].[usp_BoaByIssueBreakDown]", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter("@IssueID", this.issueid));
                sqlCmd.Parameters.Add(new SqlParameter("@OrderID", this.orderid));
                sqlCmd.Parameters.Add(new SqlParameter("@POID", this.poid));
                sqlCmd.Parameters.Add(new SqlParameter("@Order_BOAUkey", "0"));
                sqlCmd.Parameters.Add(new SqlParameter("@TestType", "1"));
                sqlCmd.Parameters.Add(new SqlParameter("@UserID", Env.User.UserID));
                sqlCmd.Parameters.Add(new SqlParameter("@IssueType", "Sewing"));
                sqlCmd.CommandTimeout = 300;
                sqlDataAdapter = new SqlDataAdapter(sqlCmd);
                sqlDataAdapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)
                {
                    bOA = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.GetBaseException().ToString(), "Error");
            }
            finally
            {
                sqlCmd.Dispose();
                sqlDataAdapter.Dispose();
                dataSet.Dispose();
                sqlConnection.Close();
            }

            this.gridBOA.DataSource = bOA;

            // 設定Grid1的顯示欄位
            this.gridBOA.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridBOA)
                .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                .Text("scirefno", header: "SCI Refno", width: Widths.AnsiChars(26))
                .Text("article", header: "Article", width: Widths.AnsiChars(10))
                .Text("colorid", header: "Color ID", width: Widths.AnsiChars(10))
                .Text("sizespec", header: "SizeSpec", width: Widths.AnsiChars(6))
                .Text("orderqty", header: "Order Qty", width: Widths.AnsiChars(10))
                .Text("suppcolor", header: "Supp Color", width: Widths.AnsiChars(10))
                .Text("sizecode", header: "SizeCode", width: Widths.AnsiChars(6))
                .Text("sizeunit", header: "Size Unit", width: Widths.AnsiChars(15))
                .Text("remark", header: "Remark", width: Widths.AnsiChars(10))
                .Text("usageqty", header: "Usage Qty", width: Widths.AnsiChars(10))
                .Text("usageunit", header: "Usage Unit", width: Widths.AnsiChars(10))
                ;
            this.gridBOA.Columns["orderqty"].Frozen = true;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
