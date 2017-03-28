using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P04_LocalTransaction : Sci.Win.Subs.Base
    {
        DataRow dataRow;
        DataTable dataTable;
        List<SqlParameter> sqlPar = new List<SqlParameter>();

        public P04_LocalTransaction(DataRow dataRow)
        {
            InitializeComponent();
            this.dataRow = dataRow;

            sqlPar.Add(new SqlParameter("@Poid", dataRow["sp"].ToString()));
            sqlPar.Add(new SqlParameter("@Refno", dataRow["refno"].ToString()));
            sqlPar.Add(new SqlParameter("@ColorID", dataRow["threadColor"].ToString()));
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region SQL Command
            string sql = @"
select 	s.date
		, s.transactionID
		, s.Name
		, s.arrivedQty
		, s.releasedQty
		, [balance] = sum(arrivedQty -releasedQty) Over (order by s.date, s.Name)
		, s.remark
From (
	SELECT	[date] = a.IssueDate
			,[transactionID] = a.ID
			,[Name] ='P60.Local PO Receiving - incoming' 
			,[arrivedQty] = b.Qty 
			,[releasedQty] = '0.00' 
			,[remark] = a.Remark 
	FROM LocalReceiving A
	INNER JOIN LocalReceiving_Detail B ON  A.ID=B.Id 
	WHERE b.OrderId like @Poid and b.Refno = @Refno and b.ThreadColorID = @ColorID

	Union all

	SELECT 	[date] = a.IssueDate
			,[transactionID] = a.ID
			,[Name] ='P61.Issue Local Item' 
			,[arrivedQty] = '0.00' 
			,[releasedQty] = b.Qty
			,[remark] = a.Remark 
	FROM localissue a 
	inner join localissue_Detail b ON a.id=b.id
	WHERE b.OrderId like @Poid and b.Refno = @Refno and b.ThreadColorID = @ColorID
) s	
order by s.date, s.Name             
";
            #endregion 
            this.ShowWaitMessage("Data Loading....");
            #region SQL Data Loading....
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sql, sqlPar, out dataTable))
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!");
                }
                else
                {
                    listControlBindingSource1.DataSource = dataTable;
                    string arrivedQty = dataTable.Compute("sum(arrivedQty)", null).ToString();
                    string releasedQty = dataTable.Compute("sum(releasedQty)", null).ToString();
                    string balance = (Convert.ToDecimal(arrivedQty) - Convert.ToDecimal(releasedQty)).ToString();

                    this.numArrived.Value = (arrivedQty.Empty()) ? decimal.Parse(arrivedQty) : 0;
                    this.numReleaced.Value = (releasedQty.Empty()) ? decimal.Parse(releasedQty) : 0;
                    this.numBalance.Value = (balance.Empty()) ? decimal.Parse(balance) : 0;
                }
            }
            else
            {
                ShowErr(sql, result);
            }
            #endregion 
            this.HideWaitMessage();
            #region Set Grid
            Helper.Controls.Grid.Generator(this.grid1)
               .Text("date", header: "Date", iseditingreadonly: true, width: Widths.AnsiChars(13))
               .Text("transaction", header: "Transaction#", iseditingreadonly: true, width: Widths.AnsiChars(10))
               .Text("name", header: "Name", iseditingreadonly: true, width: Widths.AnsiChars(10))
               .Numeric("arrivedQty", header: "Arrived Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Numeric("releasedQty", header: "Released Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Numeric("balance", header: "Balance", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Text("remark", header: "Remark", iseditingreadonly: true, width: Widths.AnsiChars(10));
            #endregion 
        }

        private void btnReCalculate_Click(object sender, EventArgs e)
        {
            #region SQL Command
            string sql = @"
update LocalInventory 
set InQty= isNull(s.InQty,0) 
	, outqty= isNull(s.OutQty,0)
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
		WHERE b.OrderId=LocalInventory.OrderID and b.Refno=LocalInventory.Refno and b.ThreadColorID=LocalInventory.ThreadColorID

		Union all

		SELECT	b.OrderId
				,b.Refno
				,b.ThreadColorID
				,[Arrived Qty]= '0.00' 
				,[Released Qty]= b.Qty
		FROM localissue a 
		inner join localissue_Detail b ON a.id=b.id
		WHERE b.OrderId=LocalInventory.OrderID and b.Refno=LocalInventory.Refno and b.ThreadColorID=LocalInventory.ThreadColorID
	) s
)s
WHERE OrderId = @Poid and Refno = @Refno and ThreadColorID = @ColorID
";
            #endregion
            this.ShowWaitMessage("Data Loading....");
            #region SQL Data Loading....
            Ict.DualResult result;
            if (result = DBProxy.Current.Execute(null, sql, sqlPar))
            {
                MyUtility.Msg.InfoBox("Finished");
            }
            else
            {
                ShowErr(sql, result);
            }
            #endregion
            this.HideWaitMessage();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P04_LocalTransaction.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dataTable, "", "Warehouse_P04_LocalTransaction.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();
                objApp.Visible = true;

                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
                if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet
                this.HideWaitMessage();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }    
}
