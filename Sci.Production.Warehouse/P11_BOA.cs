using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using Sci.Trade.Class.Commons;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P11_BOA : Sci.Win.Subs.Base
    {
        string poid, issueid, cutplanid;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P11_BOA(string _issueid, string _poid, string _cutplanid)
        {
            InitializeComponent();
            poid = _poid;
            issueid = _issueid;
            cutplanid = _cutplanid;
            this.Text += string.Format(" ({0} - {1})", poid,cutplanid);
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
                sqlCmd.Parameters.Add(new SqlParameter("@IssueID", issueid));
                sqlCmd.Parameters.Add(new SqlParameter("@OrderID", issueid));
                sqlCmd.Parameters.Add(new SqlParameter("@POID", poid));
                sqlCmd.Parameters.Add(new SqlParameter("@Order_BOAUkey", "0"));
                sqlCmd.Parameters.Add(new SqlParameter("@TestType", "1"));
                sqlCmd.Parameters.Add(new SqlParameter("@UserID", Env.User.UserID));
                sqlCmd.Parameters.Add(new SqlParameter("@IssueType", "Sewing"));
                sqlCmd.Parameters.Add(new SqlParameter("@MDivisionId", Env.User.Keyword));

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

            //設定Grid1的顯示欄位
            MyUtility.Tool.SetGridFrozen(gridBOA);
            this.gridBOA.IsEditingReadOnly = true; 
            Helper.Controls.Grid.Generator(this.gridBOA)
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
                  .Text("bomfactory", header: "bom factory", width: Widths.AnsiChars(6))
                 .Text("bomcountry", header: "bom country", width: Widths.AnsiChars(15))
                 .Text("bomstyle", header: "bom style", width: Widths.AnsiChars(10))
                  .Text("bomcustcd", header: "bom custcd", width: Widths.AnsiChars(10))
                  .Text("bomarticle", header: "bom article", width: Widths.AnsiChars(10))
                  .Text("bomzipperinsert", header: "bom zipperinsert", width: Widths.AnsiChars(10))
                  .Text("bombuymonth", header: "bom buymonth", width: Widths.AnsiChars(10))
                  .Text("bomcustpono", header: "bom custpono", width: Widths.AnsiChars(10))
                 ;
            gridBOA.Columns[5].Frozen = true;  //Order Qty

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


    }
}
