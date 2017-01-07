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
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P11_IssueBreakDown : Sci.Win.Subs.Base
    {
        DataRow Master;
        DataTable dtQtyBreakDown, DtIssueBreakDown, DtSizeCode;
        StringBuilder sbSizecode, sbSizecode2, strsbIssueBreakDown;//LEO 10/04多加一個變數
      // StringBuilder strsbIssueBreakDown;//LEO 10/04多加一個變數
        public P11_IssueBreakDown()
        {
            InitializeComponent();
            this.EditMode = true;
        }

        public P11_IssueBreakDown(DataRow _master, DataTable _dtIssueBreakDown, DataTable _dtSizeCode)
            : this()
        {
            DtSizeCode = _dtSizeCode;
            DtIssueBreakDown = _dtIssueBreakDown;
            Master = _master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder sbQtyBreakDown;
            DualResult result;

//            sqlcmd = string.Format(@"select sizecode from dbo.order_sizecode 
//where id = (select poid from dbo.orders where id='{0}') order by seq", Master["orderid"]);

//            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DtSizeCode)))
//            {
//                ShowErr(sqlcmd, result);
//                return;
//            }
            if (DtSizeCode==null || DtSizeCode.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", Master["orderid"]));
                return;
            }

            sbSizecode = new StringBuilder();
            //sbSizecode2 = new StringBuilder();
            sbSizecode.Clear();
            //sbSizecode2.Clear();
            for (int i = 0; i < DtSizeCode.Rows.Count; i++)
            {
                sbSizecode.Append(string.Format(@"[{0}],", DtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
                //sbSizecode2.Append(string.Format(@"{0},", DtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }

            sbQtyBreakDown = new StringBuilder();
            sbQtyBreakDown.Append(string.Format(@";with Bdown as 
(select a.ID [OrderID],a.Article,a.SizeCode,a.Qty from dbo.order_qty a
inner join dbo.orders b on b.id = a.id
where b.POID=(select poid from dbo.orders where id = '{0}')
)
select * from Bdown
pivot
(
	sum(qty)
	for sizecode in ({1})
)as pvt
order by [OrderID],[Article]", Master["orderid"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));//.Replace("[", "[_")
            if (!(result = DBProxy.Current.Select(null, sbQtyBreakDown.ToString(), out dtQtyBreakDown)))
            {
                ShowErr(sbQtyBreakDown.ToString(), result);
                return;
            }
            gridQtyBreakDown.AutoGenerateColumns = true;
            gridQtyBreakDownBS.DataSource = dtQtyBreakDown;
            gridQtyBreakDown.DataSource = gridQtyBreakDownBS;
            gridQtyBreakDown.IsEditingReadOnly = true;
            gridQtyBreakDown.ReadOnly = true;
            if (gridQtyBreakDown.ColumnCount > 1) gridQtyBreakDown.Columns[1].Frozen = true;

            //sbIssueBreakDown = new StringBuilder();
//            sbIssueBreakDown.Append(string.Format(@";with Bdown as 
//(select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a
//inner join dbo.orders b on b.id = a.id
//where b.POID=(select poid from dbo.orders where id = '{0}')
//)
//,Issue_Bdown as
//(
//	select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
//	from Bdown full outer join (select * from dbo.Issue_Breakdown where id='{1}') ib
//	on Bdown.orderid = ib.OrderID and Bdown.Article = ib.Article and Bdown.SizeCode = ib.SizeCode
//)
//select * from Issue_Bdown
//pivot
//(
//	sum(qty)
//	for sizecode in ({2})
//)as pvt
//order by [OrderID],[Article]", Master["orderid"], Master["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));//.Replace("[", "[_")
//            strsbIssueBreakDown = sbIssueBreakDown;//多加一個變數來接 不改變欄位
//            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out dtIssueBreakDown)))
//            {
//                ShowErr(sqlcmd, result);
//                return;
//            }
            gridIssueBreakDown.AutoGenerateColumns = true;
            gridIssueBreakDownBS.DataSource = DtIssueBreakDown;
            gridIssueBreakDown.DataSource = gridIssueBreakDownBS;
            gridIssueBreakDown.IsEditingReadOnly = false;
            if (gridIssueBreakDown.ColumnCount > 0) gridIssueBreakDown.Columns[0].ReadOnly = true;
            if (gridIssueBreakDown.ColumnCount > 1) gridIssueBreakDown.Columns[1].ReadOnly = true;
            if (gridIssueBreakDown.ColumnCount > 1) gridIssueBreakDown.Columns[1].Frozen = true;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {           
            if (!this.gridIssueBreakDown.ValidateControl()) { return; }
            this.Dispose();
            return;
//            //DtIssueBreakDown = (DataTable)this.gridIssueBreakDown.DataSource;
//            DataTable result = null;
//            string sqlcmd;
//            sqlcmd = string.Format(@";WITH UNPIVOT_1
//AS
//(
//SELECT * FROM #tmp
//UNPIVOT
//(
//QTY
//FOR SIZECODE IN ({1})
//)
//AS PVT
//)
//MERGE INTO DBO.ISSUE_BREAKDOWN T
//USING UnPivot_1 S
//ON T.ID = '{0}' AND T.ORDERID= S.OrderID AND T.ARTICLE = S.ARTICLE AND T.SIZECODE = S.SIZECODE
//WHEN MATCHED THEN
//UPDATE
//SET QTY = S.QTY
//WHEN NOT MATCHED THEN
//INSERT (ID,ORDERID,ARTICLE,SIZECODE,QTY)
//VALUES ('{0}',S.OrderID,S.ARTICLE,S.SIZECODE,S.QTY)
//;delete from dbo.issue_breakdown where id='{0}' and qty = 0; ", Master["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1));//.Replace("[", "[_")

//            string aaa = sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1).Replace("[", "").Replace("]", "");//.Replace("[", "").Replace("]", "")


            
////            sqlcmd = string.Format(string.Format(@";WITH UNPIVOT_1
////AS
////(
////SELECT * FROM #tmp
////UNPIVOT
////(
////QTY
////FOR SIZECODE IN ({1})
////)
////AS PVT
////)SELECT * FROM UNPIVOT_1;", Master["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));



//            ProcessWithDatatable2(DtIssueBreakDown, "OrderID,Article," + aaa
//                , sqlcmd, out result, "#tmp");
//            MyUtility.Msg.InfoBox("Save completed!!");
//            this.Dispose();
        }

        //public static void ProcessWithDatatable2(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp")
        //{
        //    result = null;
        //    StringBuilder sb = new StringBuilder();
        //    if (temptablename.TrimStart().StartsWith("#"))
        //    {
        //        sb.Append(string.Format("create table {0} (", temptablename));
        //    }
        //    else
        //    {
        //        sb.Append(string.Format("create table #{0} (", temptablename));
        //    }
        //    string[] cols = tmp_columns.Split(',');
        //    for (int i = 0; i < cols.Length; i++)
        //    {
        //        if (MyUtility.Check.Empty(cols[i])) continue;
        //        switch (Type.GetTypeCode(source.Columns[cols[i]].DataType))
        //        {
        //            case TypeCode.Boolean:
        //                sb.Append(string.Format("[{0}] bit", cols[i]));
        //                break;

        //            case TypeCode.Char:
        //                sb.Append(string.Format("[{0}] varchar(1)", cols[i]));
        //                break;

        //            case TypeCode.DateTime:
        //                sb.Append(string.Format("[{0}] datetime", cols[i]));
        //                break;

        //            case TypeCode.Decimal:
        //                sb.Append(string.Format("[{0}] numeric(24,8)", cols[i]));
        //                break;

        //            case TypeCode.Int32:
        //                sb.Append(string.Format("[{0}] int", cols[i]));
        //                break;

        //            case TypeCode.String:
        //                sb.Append(string.Format("[{0}] varchar(max)", cols[i]));
        //                break;

        //            case TypeCode.Int64:
        //                sb.Append(string.Format("[{0}] bigint", cols[i]));
        //                break;
        //            default:
        //                break;
        //        }
        //        if (i < cols.Length - 1) { sb.Append(","); }
        //    }
        //    sb.Append(")");

        //    System.Data.SqlClient.SqlConnection conn;
        //    DBProxy.Current.OpenConnection(null, out conn);

        //    try
        //    {
        //        DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
        //        if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }
        //        using (System.Data.SqlClient.SqlBulkCopy bulkcopy = new System.Data.SqlClient.SqlBulkCopy(conn))
        //        {
        //            bulkcopy.BulkCopyTimeout = 60;
        //            if (temptablename.TrimStart().StartsWith("#"))
        //            {
        //                bulkcopy.DestinationTableName = temptablename.Trim();
        //            }
        //            else
        //            {
        //                bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
        //            }

        //            for (int i = 0; i < cols.Length; i++)
        //            {
        //                bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
        //            }
        //            bulkcopy.WriteToServer(source);
        //            bulkcopy.Close();
        //        }
        //        result2 = DBProxy.Current.SelectByConn(conn, sqlcmd, out result);
        //        if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        private void gridIssueBreakDown_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            var dataRow = this.gridIssueBreakDown.GetDataRow(this.gridIssueBreakDownBS.Position);
            if (e.ColumnIndex > 1 && null != dataRow) 
            { 
                string col_name = this.gridIssueBreakDown.Columns[e.ColumnIndex].DataPropertyName;
                if (dataRow[col_name].Empty()) dataRow[col_name] = 0;
            }
        }

        private void gridIssueBreakDown_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            var dataRow = this.gridIssueBreakDown.GetDataRow(this.gridIssueBreakDownBS.Position);
            if (e.ColumnIndex > 1 && null != dataRow)
            {
                string col_name = this.gridIssueBreakDown.Columns[e.ColumnIndex].DataPropertyName;
                if (dataRow[col_name].Empty()) dataRow[col_name] = 0;
            }
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (dtQtyBreakDown == null || DtIssueBreakDown==null) return;
            ((DataTable)gridIssueBreakDownBS.DataSource).Rows.Clear();
            foreach (DataRow tmprow in dtQtyBreakDown.Rows)
            {
                ((DataTable)gridIssueBreakDownBS.DataSource).ImportRow(tmprow);
            }
        }

    }
}
