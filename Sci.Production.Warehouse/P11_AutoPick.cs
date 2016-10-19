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
    public partial class P11_AutoPick : Sci.Win.Subs.Base
    {
        string poid, issueid, cutplanid;
        public DataTable BOA, BOA_Orderlist, BOA_PO, BOA_PO_Size;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public Dictionary<DataRow, DataTable> dictionaryDatas = new Dictionary<DataRow, DataTable>();
        public P11_AutoPick(string _issueid, string _poid, string _cutplanid)
        {
            InitializeComponent();
            poid = _poid;
            issueid = _issueid;
            cutplanid = _cutplanid;
            this.Text += string.Format(" ({0})", poid);
            gridBOA.RowPostPaint += (s, e) =>
            {
                DataTable dtSource = (DataTable)listControlBindingSource1.DataSource;

                DataRow dr = gridBOA.GetDataRow(e.RowIndex);
                bool exists = dtSource
                    .AsEnumerable()
                    .Any(dataRow =>
                    {
                        return string.Compare(dataRow.Field<string>("scirefno"), dr.Field<string>("scirefno"), true) == 0 &&
                               string.Compare(dataRow.Field<string>("colorid"), dr.Field<string>("colorid"), true) == 0 &&
                               string.Compare(dataRow.Field<string>("sizespec"), dr.Field<string>("sizespec"), true) == 0;
                    });

                if (exists)
                {
                    gridBOA.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                }

            };
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //POPrg.BOAExpend(poid, "0", 1, out BOA, out BOA_Orderlist);
            SqlConnection sqlConnection = null;
            SqlCommand sqlCmd = null;
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = null;

            BOA = null;
            BOA_Orderlist = null;
            BOA_PO = null;
            BOA_PO_Size = null;

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
                sqlCmd.CommandTimeout = 300;
                sqlDataAdapter = new SqlDataAdapter(sqlCmd);

                sqlDataAdapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)
                {
                    BOA = dataSet.Tables[0];
                    BOA_Orderlist = dataSet.Tables[1];
                    BOA_PO = dataSet.Tables[2];
                    BOA_PO.DefaultView.Sort = "qty desc,scirefno,poid,seq1,seq2";
                    BOA_PO_Size = dataSet.Tables[3];

                    DataRelation relation = new DataRelation("rel1"
                    , new DataColumn[] { BOA_PO.Columns["Poid"], BOA_PO.Columns["seq1"], BOA_PO.Columns["seq2"] }
                    , new DataColumn[] { BOA_PO_Size.Columns["Poid"], BOA_PO_Size.Columns["seq1"], BOA_PO_Size.Columns["seq2"] }
                    );
                    dataSet.Relations.Add(relation);

                    foreach (DataRow dr in BOA_PO.Rows)
                    {

                        DataTable tmp = new DataTable();
                        tmp.ColumnsStringAdd("Poid");
                        tmp.ColumnsStringAdd("seq1");
                        tmp.ColumnsStringAdd("seq2");
                        tmp.ColumnsStringAdd("seq");
                        tmp.ColumnsStringAdd("sizecode");
                        tmp.ColumnsDecimalAdd("qty");
                        tmp.ColumnsDecimalAdd("ori_qty");

                        var drs = dr.GetChildRows(relation);
                        if (drs.Count() > 0)
                        {
                            foreach (DataRow dr2 in drs)
                            {
                                tmp.ImportRow(dr2);
                            }
                        }

                        tmp.AcceptChanges();

                        if (tmp.Rows.Count > 0)
                        {
                            dictionaryDatas.Add(dr, tmp);
                        }
                        else
                        {
                            dictionaryDatas.Add(dr, new DataTable());
                        }
                    }
                    var tmp2 = dictionaryDatas.Count;
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

            this.listControlBindingSource1.DataSource = BOA_PO;
            this.grid1.DataSource = listControlBindingSource1;

            this.grid1.AutoResizeColumns();
            this.gridBOA.DataSource = BOA;
            Helper.Controls.Grid.Generator(this.gridBOA)

               .Text("ID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
               .Text("RefNo", header: "RefNo", iseditingreadonly: true, width: Widths.AnsiChars(13)) //2
               .Text("SCIRefNo", header: "SCIRefNo", iseditingreadonly: true, width: Widths.AnsiChars(17)) //2
                .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                 .Text("ColorID", header: "ColorID", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                  .Text("SizeCode", header: "SizeCode", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                    .Text("SizeSpec", header: "SizeSpec", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                       .Text("OrderQty", header: "OrderQty", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                       .Text("UsageQty", header: "UsageQty", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                       .Text("UsageUnit", header: "UsageUnit", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                       .Text("SysUsageQty", header: "SysUsageQty", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2

              // .Numeric("", header: "SizeSpec", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //3

               ;
            // this.gridBOA.AutoGenerateColumns = true;
            // this.gridBOA.AutoResizeColumns();


            #region --Pick Qty 開窗--
            Ict.Win.DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            ns.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P11_AutoPick_Detail();
                DataTable tmpDt = dictionaryDatas[grid1.GetDataRow(e.RowIndex)];
                //DataTable _clone = tmpDt.Copy();
                frm.SetGrid(tmpDt);
                DialogResult DResult = frm.ShowDialog(this);

                if (DResult == DialogResult.OK)
                    sum_subDetail(dr, tmpDt);
            };
            #endregion

            #region --設定Grid1的顯示欄位--
            MyUtility.Tool.SetGridFrozen(grid1);

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Text("scirefno", header: "SCI Refno", width: Widths.AnsiChars(23))
                 .Text("colorid", header: "Color ID", width: Widths.AnsiChars(7))
                 .Text("sizespec", header: "SizeSpec", width: Widths.AnsiChars(6))
                 .Text("qty", header: "Pick Qty", width: Widths.AnsiChars(10), settings: ns)
                 .Text("Balanceqty", header: "Bulk Qty", width: Widths.AnsiChars(10))
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
            grid1.Columns[7].Frozen = true;  //Qty
            grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;   //Qty
            #endregion

        }

        static void sum_subDetail(DataRow target, DataTable source)
        {
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataRow[] dr2 = BOA_PO.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = BOA_PO.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Pick Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = BOA_PO.Select("qty > balanceqty and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Pick Qty of selected row can't over Bulk Qty!", "Warning");
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
