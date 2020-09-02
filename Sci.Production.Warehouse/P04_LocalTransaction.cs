using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P04_LocalTransaction : Win.Subs.Base
    {
        private DataRow dataRow;
        private DataTable dataTable;
        private List<SqlParameter> sqlPar = new List<SqlParameter>();

        public P04_LocalTransaction(DataRow dataRow, string from_program)
            : base()
        {
            this.InitializeComponent();
            this.dataRow = dataRow;

            if (from_program.Equals("P03"))
            {
                this.sqlPar.Add(new SqlParameter("@Poid", dataRow["id"].ToString().Trim()));
                this.sqlPar.Add(new SqlParameter("@Refno", dataRow["refno"].ToString().Trim()));
                this.sqlPar.Add(new SqlParameter("@ColorID", dataRow["ColorID"].ToString().Trim()));
            }
            else
            {
                this.sqlPar.Add(new SqlParameter("@Poid", dataRow["sp"].ToString().Trim()));
                this.sqlPar.Add(new SqlParameter("@Refno", dataRow["refno"].ToString().Trim()));
                this.sqlPar.Add(new SqlParameter("@ColorID", dataRow["threadColor"].ToString().Trim()));
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.LoadData();
            #region Set Grid
            this.Helper.Controls.Grid.Generator(this.gridLocalTransaction)
               .Text("date", header: "Date", iseditingreadonly: true, width: Widths.AnsiChars(13))
               .Text("transactionID", header: "Transaction#", iseditingreadonly: true, width: Widths.AnsiChars(25))
               .Text("name", header: "Name", iseditingreadonly: true, width: Widths.AnsiChars(30))
               .Numeric("arrivedQty", header: "Arrived Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Numeric("releasedQty", header: "Released Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Numeric("balance", header: "Balance", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Text("remark", header: "Remark", iseditingreadonly: true, width: Widths.AnsiChars(10));
            #endregion
        }

        private void LoadData()
        {
            #region SQL Command
            string sql = @"
select 	s.date
		, s.transactionID
		, s.Name
		, s.arrivedQty
		, s.releasedQty
        , balance = 0
		, s.remark
From (
	SELECT	[date] = a.IssueDate
			,[transactionID] = a.ID
			,[Name] ='P60.Local PO Receiving - incoming' 
			,[arrivedQty] = b.Qty
			,[releasedQty] = '0.00' 
			,[remark] = isnull(a.Remark, '')
	FROM LocalReceiving A
	INNER JOIN LocalReceiving_Detail B ON  A.ID=B.Id 
	WHERE b.OrderId = @Poid and b.Refno = @Refno and b.ThreadColorID = @ColorID 
        and a.Status = 'CONFIRMED'

	Union all

	SELECT 	[date] = a.IssueDate
			,[transactionID] = a.ID
			,[Name] ='P61.Issue Local Item' 
			,[arrivedQty] = '0.00' 
			,[releasedQty] = b.Qty
			,[remark] = isnull(a.Remark, '')
	FROM localissue a 
	inner join localissue_Detail b ON a.id=b.id
	WHERE b.OrderId = @Poid and b.Refno = @Refno and b.ThreadColorID = @ColorID
        and a.Status = 'CONFIRMED'

    Union all

	SELECT 	[date] = a.IssueDate
			,[transactionID] = a.ID
			,[Name] ='P39. Adjust Bulk Qty (Local)' 
			,[arrivedQty] = iif(QtyAfter > QtyBefore,QtyAfter - QtyBefore , 0)
			,[releasedQty] = iif(QtyBefore > QtyAfter,QtyBefore - QtyAfter , 0)
			,[remark] = isnull(a.Remark, '')
	FROM AdjustLocal  a 
	inner join AdjustLocal_Detail b ON a.id=b.id
	WHERE b.POID = @Poid and b.Refno = @Refno and b.Color = @ColorID and a.Type = 'A'
        and a.Status = 'CONFIRMED'

    Union all

	SELECT 	[date] = a.IssueDate
			,[transactionID] = a.ID
			,[Name] ='P47. Transfer Bulk to Scrap (A2C)(Local)' 
			,[arrivedQty] = 0
			,[releasedQty] =  b.Qty
			,[remark] = isnull(a.Remark, '')
	FROM SubTransferlocal  a 
	inner join SubTransferlocal_Detail b ON a.id=b.id
	WHERE b.POID = @Poid and b.Refno = @Refno and b.Color = @ColorID
        and a.Status = 'CONFIRMED'
) s	
order by s.date, s.Name, arrivedQty, releasedQty        
";
            #endregion
            this.ShowWaitMessage("Data Loading....");
            #region SQL Data Loading....
            DualResult result;
            if (result = DBProxy.Current.Select(null, sql, this.sqlPar, out this.dataTable))
            {
                if (this.dataTable == null || this.dataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!");
                    this.Close();
                }
                else
                {
                    #region compute BalanceQty
                    for (int i = 0; i < this.dataTable.Rows.Count; i++)
                    {
                        DataRow dr = this.dataTable.Rows[i];
                        if (i == 0)
                        {
                            dr["balance"] = Convert.ToDecimal(dr["arrivedQty"]) - Convert.ToDecimal(dr["releasedQty"]);
                        }
                        else
                        {
                            DataRow drFront = this.dataTable.Rows[i - 1];
                            dr["balance"] = Convert.ToDecimal(drFront["balance"]) + Convert.ToDecimal(dr["arrivedQty"]) - Convert.ToDecimal(dr["releasedQty"]);
                        }
                    }
                    #endregion
                    this.listControlBindingSource1.DataSource = this.dataTable;
                    string arrivedQty = this.dataTable.Compute("sum(arrivedQty)", null).ToString();
                    string releasedQty = this.dataTable.Compute("sum(releasedQty)", null).ToString();
                    string balance = (Convert.ToDecimal(arrivedQty) - Convert.ToDecimal(releasedQty)).ToString();

                    this.numArrived.Value = arrivedQty.Empty() ? 0 : decimal.Parse(arrivedQty);
                    this.numReleaced.Value = releasedQty.Empty() ? 0 : decimal.Parse(releasedQty);
                    this.numBalance.Value = balance.Empty() ? 0 : decimal.Parse(balance);
                }
            }
            else
            {
                this.ShowErr(sql, result);
            }
            #endregion
            this.HideWaitMessage();
        }

        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            #region SQL Command
            string sql = @"
update LocalInventory 
set InQty= isNull(s.InQty,0) 
	, outqty= isNull(s.OutQty,0)
    , AdjustQty = isNull(adj.Adjqty,0)
from LocalInventory
outer apply(
	SELECT	[InQty]=sum([Arrived Qty]),
			[OutQty]=sum([Released Qty])
	From (
		SELECT 
			b.OrderId
			,b.Refno
			,b.ThreadColorID
			,[Arrived Qty]=b.Qty 
			,[Released Qty]= '0.00' 
		FROM LocalReceiving A
		INNER JOIN LocalReceiving_Detail B ON  A.ID=B.Id 
		WHERE b.OrderId = LocalInventory.OrderID and b.Refno = LocalInventory.Refno and b.ThreadColorID = LocalInventory.ThreadColorID
            and a.Status = 'CONFIRMED' 
		Union all

		SELECT	b.OrderId
				,b.Refno
				,b.ThreadColorID
				,[Arrived Qty]= '0.00' 
				,[Released Qty]= b.Qty
		FROM localissue a 
		inner join localissue_Detail b ON a.id=b.id
		WHERE b.OrderId = LocalInventory.OrderID and b.Refno = LocalInventory.Refno and b.ThreadColorID = LocalInventory.ThreadColorID
            and a.Status = 'CONFIRMED'

        Union all

	    SELECT  [OrderId] = b.POID
				,b.Refno
				,[ThreadColorID] = b.Color
	    		,[Arrived Qty] = 0
	    		,[Released Qty] =  b.Qty
	    FROM SubTransferlocal  a 
	    inner join SubTransferlocal_Detail b ON a.id=b.id
	    WHERE b.POID = LocalInventory.OrderID and b.Refno = LocalInventory.Refno and b.Color = LocalInventory.ThreadColorID
            and a.Status = 'CONFIRMED'
	) s
)s
outer apply (
SELECT 	[Adjqty] = sum(isnull(QtyAfter,0) - isnull(QtyBefore,0))
	    FROM AdjustLocal  a 
	    inner join AdjustLocal_Detail b ON a.id=b.id
	    WHERE b.POID = LocalInventory.OrderID and b.Refno = LocalInventory.Refno and b.Color = LocalInventory.ThreadColorID and a.Type = 'A'
            and a.Status = 'CONFIRMED'
) adj
WHERE OrderId = @Poid and Refno = @Refno and ThreadColorID = @ColorID
";
            #endregion
            this.ShowWaitMessage("Data Loading....");
            #region SQL Data Loading....
            DualResult result;
            if (result = DBProxy.Current.Execute(null, sql, this.sqlPar))
            {
                MyUtility.Msg.InfoBox("Finished");
            }
            else
            {
                this.ShowErr(sql, result);
            }
            #endregion
            this.HideWaitMessage();
            this.LoadData();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            if (this.dataTable != null && this.dataTable.Rows.Count > 0)
            {
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P04_LocalTransaction.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dataTable, string.Empty, "Warehouse_P04_LocalTransaction.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P04_LocalTransaction");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
