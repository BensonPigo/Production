using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P11_BOA : Win.Subs.Base
    {
        string poid;
        string issueid;
        string cutplanid;
        string orderid;

       // Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P11_BOA(string _issueid, string _poid, string _cutplanid, string _orderid)
        {
            this.InitializeComponent();
            this.poid = _poid;
            this.issueid = _issueid;
            this.cutplanid = _cutplanid;
            this.orderid = _orderid;
            this.Text += string.Format(" ({0} - {1})", this.poid, this.cutplanid);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable BOA, BOA_Orderlist, BOA_PO;
            SqlConnection sqlConnection = null;
            SqlCommand sqlCmd = null;
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = null;

            BOA = null;
            BOA_Orderlist = null;
            BOA_PO = null;

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

                // sqlCmd.Parameters.Add(new SqlParameter("@MDivisionId", Env.User.Keyword));
                sqlCmd.CommandTimeout = 300;
                sqlDataAdapter = new SqlDataAdapter(sqlCmd);

                sqlDataAdapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)
                {
                    BOA = dataSet.Tables[0];
                    BOA_Orderlist = dataSet.Tables[1];
                    BOA_PO = dataSet.Tables[2];
                    BOA_PO.DefaultView.Sort = "qty desc,scirefno,poid,seq1,seq2";
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

            this.gridBOA.DataSource = BOA;

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

                  // .Text("bomfactory", header: "bom factory", width: Widths.AnsiChars(6))
                 // .Text("bomcountry", header: "bom country", width: Widths.AnsiChars(15))
                 // .Text("bomstyle", header: "bom style", width: Widths.AnsiChars(10))
                  // .Text("bomcustcd", header: "bom custcd", width: Widths.AnsiChars(10))
                  // .Text("bomarticle", header: "bom article", width: Widths.AnsiChars(10))
                  // .Text("bomzipperinsert", header: "bom zipperinsert", width: Widths.AnsiChars(10))
                  // .Text("bombuymonth", header: "bom buymonth", width: Widths.AnsiChars(10))
                  // .Text("bomcustpono", header: "bom custpono", width: Widths.AnsiChars(10))
                 ;
            this.gridBOA.Columns[5].Frozen = true;  // Order Qty
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
