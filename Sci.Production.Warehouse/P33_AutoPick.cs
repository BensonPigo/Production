using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sci.Production.Warehouse
{
    public partial class P33_AutoPick : Sci.Win.Subs.Base
    {
        StringBuilder sbSizecode;
        string poid, issueid, orderid;
        public DataTable BOA, BOA_Orderlist, BOA_PO, BOA_PO_Size, dtIssueBreakDown;
        public DataRow[] importRows;
        public List<IssueQtyBreakdown> _IssueQtyBreakdownList = new List<IssueQtyBreakdown>();
        bool combo;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public Dictionary<DataRow, DataTable> dictionaryDatas = new Dictionary<DataRow, DataTable>();
        public P33_AutoPick(string _issueid, string _poid, string _orderid, DataTable _dtIssueBreakDown, StringBuilder _sbSizecode, bool _combo, List<IssueQtyBreakdown> IssueQtyBreakdownList)
        {
            InitializeComponent();
            poid = _poid;
            issueid = _issueid;
            //cutplanid = _cutplanid;
            orderid = _orderid;
            dtIssueBreakDown = _dtIssueBreakDown;
            sbSizecode = _sbSizecode;
            combo = _combo;
            this._IssueQtyBreakdownList = IssueQtyBreakdownList;
            this.Text += string.Format(" ({0})", poid);

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (dtIssueBreakDown == null)
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            Decimal sum = 0;
            foreach (DataRow dr in dtIssueBreakDown.Rows)
            {
                foreach (DataColumn dc in dtIssueBreakDown.Columns)
                {
                    if (Object.ReferenceEquals(sum.GetType(), dr[dc].GetType()))
                        sum += (Decimal)dr[dc];
                }
            }

            if (sum == 0)
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            SqlConnection sqlConnection = null;
            DataSet dataSet = new DataSet();
            DataTable result = null;

            BOA = null;
            BOA_Orderlist = null;
            BOA_PO = null;
            BOA_PO_Size = null;

            // 整理出#tmp傳進 SQL 使用
            DataTable IssueBreakDown_Dt = new DataTable();
            IssueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "OrderID", DataType = typeof(string) });
            IssueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            IssueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "Qty", DataType = typeof(int) });

            foreach (var model in _IssueQtyBreakdownList)
            {
                if (model.Qty > 0)
                {
                    DataRow newDr = IssueBreakDown_Dt.NewRow();
                    newDr["OrderID"] = model.OrderID;
                    newDr["Article"] = model.Article;
                    newDr["Qty"] = model.Qty;

                    IssueBreakDown_Dt.Rows.Add(newDr);
                }
            }

            string sqlcmd;

            #region SQL
            sqlcmd = $@"

SELECt Article,[Qty]=SUM(Qty) 
INTO #tmp_sumQty
FROm #tmp
GROUP BY Article


--------------取得哪些要打勾--------------
---- 從Issue breakdown的Article，找到包含在哪些物料裡面
---- 傳入OrderID
Select distinct tcd.SCIRefNo, tcd.SuppColor ,tcd.Article 
INTO #step1
From dbo.Orders as o
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )


----取得這些物料的項次號，然後取得Order List，如果NULL就直接打勾不用判斷後續
---- 傳入OrderID
SELECT DISTINCT  pso.OrderID,a.SCIRefNo,a.SuppColor
INTO #step2
FROM PO_Supp_Detail a
LEFT JOIN  PO_Supp_Detail_OrderList pso ON a.ID = pso.ID AND a.SEQ1 = pso.SEQ1 AND a.SEQ2 = pso.SEQ2
WHERE a.ID = '{this.poid}'
AND EXISTS(
	SELECT * FROM #step1
	WHERE SCIRefNo = a.SCIRefNo AND SuppColor = a.SuppColor
)

SELECT DISTINCT SCIRefno,SuppColor
INTO #SelectList1
FROM #step2 
WHERE OrderID IS NULL 

SELECT DISTINCT a.SCIRefno,a.SuppColor--,b.* 
INTO #SelectList2
FROM #step2 a
INNER JOIN Order_Article b ON a.OrderID = b.id
WHERE OrderID IS NOT NULL 

--SELECT * FROM #SelectList1
--SELECT * FROM #SelectList2

--------------取得哪些要打勾--------------


SELECT  DISTINCT 
		[Selected] = IIF(   EXISTS(SELECT 1 FROM #SelectList1 WHERE SCIRefno =psd.SCIRefno AND SuppColor=psd.SuppColor) OR
							EXISTS(SELECT 1 FROM #SelectList2 WHERE SCIRefno =psd.SCIRefno AND SuppColor=psd.SuppColor)
						,1 ,0)
		, psd.SCIRefno 
        , psd.Refno
		, psd.SuppColor
		, f.DescDetail
		, [@Qty]= ThreadUsedQtyByBOT.Val
		, [Use Qty By Stock Unit] = CEILING (ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ThreadUsedQtyByBOT.Val/ 100 * ISNULL(UnitRate.RateValue,1) )--並轉換為Stock Unit
		, [Stock Unit]=StockUnit.StockUnit
		, [Use Qty By Use Unit]= (ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ThreadUsedQtyByBOT.Val  )
		, [Use Unit]='CM'
		, [Stock Unit Desc.]=StockUnit.Description
		, [Output Qty(Garment)] = ISNULL(ThreadUsedQtyByBOT.Qty,0)
		, [Bulk Balance(Stock Unit)] = 0
        , [POID]=psd.ID
		, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I2 WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I2.id = [IS].Id 
					where I2.type = 'E' and I2.Status = 'Confirmed' 
					and [IS].Poid=psd.ID AND [IS].SCIRefno=psd.SCIRefno AND [IS].SuppColor=psd.SuppColor and i2.[EditDate]<GETDATE()
				)
INTO #final
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
OUTER APPLY(
	SELECT TOP 1 a.StockUnit ,u.Description
	FROM PO_Supp_Detail a
	INNER JOIN Unit u ON a.StockUnit = u.ID
	WHERE a.ID =psd.ID	AND a.SCIRefno=psd.SCIRefno AND a.SuppColor=psd.SuppColor
)StockUnit
OUTER APPLY(
	SELECT RateValue
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate
OUTER APPLY(
	SELECT SCIRefNo
		,SuppColor
		,[Val]=SUM(((SeamLength  * Frequency * UseRatio ) +  (Allowance * Segment) )) 
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  SuppColor= psd.SuppColor AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID) g
	WHERE SCIRefNo= psd.SCIRefNo AND SuppColor = psd.SuppColor  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND SuppColor = psd.SuppColor 
	)
	GROUP BY SCIRefNo,SuppColor , Article
)ThreadUsedQtyByBOT
WHERE psd.ID='{this.poid}'
AND psd.FabricType ='A'
AND EXISTS(
	SELECT 1
	FROM Fabric f
	INNER JOIN MtlType m on m.id= f.MtlTypeID
	WHERE f.SCIRefno = psd.SCIRefNo
	AND m.IsThread=1 
)
AND psd.SuppColor <> ''

SELECT  [Selected] 
		, SCIRefno 
        , Refno
		, SuppColor
		, DescDetail
		, [@Qty] 
		, [Use Qty By Stock Unit]
		, [Stock Unit]
		, [Use Qty By Use Unit]
		, [Use Unit]
		, [Stock Unit Desc.]
		, [Output Qty(Garment)]
		, [Bulk Balance(Stock Unit)] = Balance.Qty
        , [POID]
		, [AccuIssued]
FROM #final t
OUTER APPLY(
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND psd.SuppColor=t.SuppColor AND psd.ID='{this.poid}'
)Balance 


DROP TABLE #step1,#step2 ,#SelectList1 ,#SelectList2 ,#final,#tmp,#tmp_sumQty


";
            #endregion

            try
            {
                //SqlConnection conn;
                DBProxy.Current.OpenConnection(null, out sqlConnection);
                var dualResult = MyUtility.Tool.ProcessWithDatatable(IssueBreakDown_Dt, string.Empty, sqlcmd, out result, "#tmp", conn: sqlConnection);

                if (!dualResult) ShowErr(dualResult);
                if (!dualResult) return;

                BOA_PO = result;
                this.listControlBindingSource1.DataSource = result;

            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.GetBaseException().ToString(), "Error");

            }
            finally
            {
                dataSet.Dispose();
                sqlConnection.Close();
            }

            this.gridAutoPick.DataSource = listControlBindingSource1;

            this.gridAutoPick.AutoResizeColumns();



            Ict.Win.DataGridViewGeneratorNumericColumnSettings Qty = new DataGridViewGeneratorNumericColumnSettings();

            Qty.CellMouseDoubleClick += (s, e) =>
            {
                DataTable detail = (DataTable)listControlBindingSource1.DataSource;
                DataRow currentRow = detail.Rows[e.RowIndex];

                string SCIRefNo = currentRow["SCIRefNo"].ToString();
                string SuppColor = currentRow["SuppColor"].ToString();
                List<string> Articles = _IssueQtyBreakdownList.Where(o => o.Qty > 0).Select(o => o.Article).Distinct().ToList();
                string cmd = $@"

SELECT Article, [Qty]=SUM(((SeamLength  * Frequency * UseRatio ) + (Allowance *Segment))) 
FROM dbo.GetThreadUsedQtyByBOT('{this.poid}')
WHERE SCIRefNo='{SCIRefNo}' 
AND SuppColor='{SuppColor}'
AND Article IN ('{Articles.JoinToString("','")}')
GROUP BY Article

";

                DataTable dt;
                DualResult dualResult = DBProxy.Current.Select(null, cmd, out dt);
                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                    return;
                }

                MyUtility.Msg.ShowMsgGrid_LockScreen(dt, caption: $"@Qty by Article");
            };

            #region --設定Grid1的顯示欄位--

            this.gridAutoPick.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridAutoPick.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridAutoPick)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                 .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("SuppColor", header: "SuppColor", width: Widths.AnsiChars(7), iseditingreadonly: true)
                 .Text("DescDetail", header: "Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("@Qty", header: "@Qty", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true, settings: Qty)
                 .Numeric("Use Qty By Stock Unit", header: "Use Qty\r\nBy Stock Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                 .Text("Stock Unit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("Use Qty By Use Unit", header: "Use Qty\r\nBy Use Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                 .Text("Use Unit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("Stock Unit Desc.", header: "Stock Unit Desc.", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("Output Qty(Garment)", header: "Output Qty\r\n(Garment)", width: Widths.AnsiChars(6), decimal_places: 0, iseditingreadonly: true)
                 .Text("Bulk Balance(Stock Unit)", header: "Bulk Balance\r\n(Stock Unit)", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;
            gridAutoPick.Columns["Selected"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        public void sum_subDetail(DataRow target, DataTable source)
        {
            DataTable tmpDt = source;
            DataRow dr = target;
            if (tmpDt != null)
            {
                var Output = "";
                Decimal SumQTY = 0;
                foreach (DataRow dr2 in tmpDt.ToList())
                {
                    if (Convert.ToDecimal(dr2["qty"]) != 0)
                    {
                        Output += dr2["sizecode"].ToString() + "*" + Convert.ToDecimal(dr2["qty"]).ToString("0.00") + ", ";
                        SumQTY += Convert.ToDecimal(dr2["qty"]);
                    }
                }
                dr["Output"] = Output;
                dr["qty"] = Math.Round(SumQTY, 2);
            }
            if (Convert.ToDecimal(dr["qty"]) > 0) dr["Selected"] = 1;
            else dr["Selected"] = 0;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            gridAutoPick.ValidateControl();
            DataRow[] dr2 = BOA_PO.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select rows first!", "Warnning");
                return;
            }
            importRows = dr2;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public static void ProcessWithDatatable2(DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result, string temptablename = "#tmp")
        {
            result = null;
            StringBuilder sb = new StringBuilder();
            if (temptablename.TrimStart().StartsWith("#"))
            {
                sb.Append(string.Format("create table {0} (", temptablename));
            }
            else
            {
                sb.Append(string.Format("create table #{0} (", temptablename));
            }
            string[] cols = tmp_columns.Split(',');
            for (int i = 0; i < cols.Length; i++)
            {
                if (MyUtility.Check.Empty(cols[i])) continue;
                switch (Type.GetTypeCode(source.Columns[cols[i]].DataType))
                {
                    case TypeCode.Boolean:
                        sb.Append(string.Format("[{0}] bit", cols[i]));
                        break;

                    case TypeCode.Char:
                        sb.Append(string.Format("[{0}] varchar(1)", cols[i]));
                        break;

                    case TypeCode.DateTime:
                        sb.Append(string.Format("[{0}] datetime", cols[i]));
                        break;

                    case TypeCode.Decimal:
                        sb.Append(string.Format("[{0}] numeric(24,8)", cols[i]));
                        break;

                    case TypeCode.Int32:
                        sb.Append(string.Format("[{0}] int", cols[i]));
                        break;

                    case TypeCode.String:
                        sb.Append(string.Format("[{0}] varchar(max)", cols[i]));
                        break;

                    case TypeCode.Int64:
                        sb.Append(string.Format("[{0}] bigint", cols[i]));
                        break;
                    default:
                        break;
                }
                if (i < cols.Length - 1) { sb.Append(","); }
            }
            sb.Append(")");

            System.Data.SqlClient.SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }
                using (System.Data.SqlClient.SqlBulkCopy bulkcopy = new System.Data.SqlClient.SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = 60;
                    if (temptablename.TrimStart().StartsWith("#"))
                    {
                        bulkcopy.DestinationTableName = temptablename.Trim();
                    }
                    else
                    {
                        bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
                    }

                    for (int i = 0; i < cols.Length; i++)
                    {
                        bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
                    }
                    bulkcopy.WriteToServer(source);
                    bulkcopy.Close();
                }
                result2 = DBProxy.Current.SelectByConn(conn, sqlcmd, out result);
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable getAutoDetailDataTable(int RowIndex)
        {
            DataTable tmpDt = dictionaryDatas[gridAutoPick.GetDataRow(RowIndex)];
            return tmpDt;
        }

        public DataRow getAutoDetailDataRow(int RowIndex)
        {
            DataRow tmpDt = this.gridAutoPick.GetDataRow<DataRow>(RowIndex);
            return tmpDt;
        }

        public DataRow getNeedChangeDataRow(int RowIndex)
        {
            DataRow tmpDt = this.gridAutoPick.GetDataRow<DataRow>(RowIndex);
            return tmpDt;
        }

        public void dictionaryDatasRejectChanges()
        {
            var d = dictionaryDatas.AsEnumerable().ToList();
            for (int i = 0; i < d.Count; i++)
            {
                d[i].Value.RejectChanges();
            }
            return;
            ////批次RejectChanges
            //foreach (KeyValuePair<DataRow, DataTable> item in dictionaryDatas)
            //{
            //    item.Value.RejectChanges();
            //}
        }

        public void dictionaryDatasAcceptChanges()
        {
            //批次RejectChanges
            var d = dictionaryDatas.AsEnumerable().ToList();
            for (int i = 0; i < d.Count; i++)
            {
                d[i].Value.AcceptChanges();
            }
            return;
            //foreach (KeyValuePair<DataRow, DataTable> item in dictionaryDatas)
            //{
            //    item.Value.AcceptChanges();
            //}
        }
    }
}
