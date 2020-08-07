using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R05 : Win.Tems.PrintForm
    {
        private DateTime? Issuedate1;
        private DateTime? Issuedate2;
        private string SP1;
        private string SP2;
        private string mdivisionid;
        private string factory;
        private string materialType;
        private int type;
        private DataTable printData;

        /// <summary>
        /// Initializes a new instance of the <see cref="R05"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision1.Text = Env.User.Keyword;

            DualResult result;
            string sqlM = @" 
                        select [fabrictype] = '', [fabrictypeName] = 'All'
                        union
                        SELECT distinct fabrictype
                                ,[fabrictypeName] = case fabrictype
                                    when 'F' then 'Fabric' 
                                    when 'A' then 'Accessory'
                                end 
                        FROM Po_supp_detail WITH (NOLOCK) 
                        where fabrictype !='O' AND fabrictype !=''";
            result = DBProxy.Current.Select(string.Empty, sqlM, out DataTable material);
            if (!result)
            {
                this.ShowErr(result);
            }
            else
            {
                this.comboMaterialType.DataSource = material;
                this.comboMaterialType.ValueMember = "fabrictype";
                this.comboMaterialType.DisplayMember = "fabrictypeName";
                this.comboMaterialType.SelectedIndex = 0;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Issuedate1 = this.dateIssue.Value1;
            this.Issuedate2 = this.dateIssue.Value2;
            this.SP1 = this.txtSP1.Text;
            this.SP2 = this.txtSP2.Text;
            this.mdivisionid = this.txtMdivision1.Text;
            this.factory = this.txtfactory1.Text;
            this.type = this.radioTransferIn.Checked ? 0 : 1;
            this.materialType = this.comboMaterialType.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            if (this.radioDetail.Checked)
            {
                sqlCmd = this.SQLCmd_Detail();
            }
            else
            {
                sqlCmd = this.SQLCmd_Summary();
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        private StringBuilder SQLCmd_Detail()
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (this.type == 0)
            {
                sqlCmd.Append(@"
select
	t.id,t.MDivisionID,t.FromFtyID,t.FactoryID,t.IssueDate,t.Status
	,td.POID, o.StyleID, p.Refno, p.ColorID, p.SizeSpec
	,td.Seq1,td.Seq2,td.Roll,td.Dyelot
	,[StockType] = case when td.StockType = 'B' then 'Bulk'
	        when td.StockType = 'I' then 'Inventory'
	        end
	,td.Qty
	,[Unit] = dbo.GetStockUnitBySPSeq (td.poid, td.seq1, td.seq2)
	,t.Remark
	,Description = dbo.getMtlDesc(td.POID,td.seq1,td.seq2,2,0)
from TransferIn t with(nolock)
inner join TransferIn_Detail td with(nolock) on td.id = t.id
left join Po_Supp_Detail p WITH (NOLOCK) on td.poid = p.id and td.seq1 = p.seq1 and td.seq2 = p.seq2
left join Orders o with(nolock) on o.id = td.poid
where 1=1
");
                if (!MyUtility.Check.Empty(this.Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", this.Issuedate1.Value.ToShortDateString(), this.Issuedate2.Value.ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", this.SP1));
                }

                if (!MyUtility.Check.Empty(this.SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <='{0}'", this.SP2));
                }

                if (!MyUtility.Check.Empty(this.mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", this.mdivisionid));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", this.factory));
                }

                if (!MyUtility.Check.Empty(this.materialType))
                {
                    sqlCmd.Append(string.Format(@" and p.FabricType  = '{0}'", this.materialType));
                }
            }
            else
            {
                sqlCmd.Append(@"
select
	t.id,t.MDivisionID,t.FactoryID,t.ToMDivisionid,t.IssueDate,t.Status
	,td.POID, o.StyleID, p.Refno, p.ColorID, p.SizeSpec
	,td.Seq1, td.Seq2, td.Roll, td.Dyelot
	,[StockType] = case when td.StockType = 'B' then 'Bulk'
	        when td.StockType = 'I' then 'Inventory'
	        end
	,td.Qty
	,[Unit] = dbo.GetStockUnitBySPSeq (td.poid, td.seq1, td.seq2)
	,t.Remark
	,Description = dbo.getMtlDesc(td.POID, td.seq1, td.seq2, 2, 0)
from TransferOut t with(nolock)
inner join TransferOut_Detail td with(nolock) on td.id = t.id
left join Po_Supp_Detail p WITH (NOLOCK) on td.poid = p.id and td.seq1 = p.seq1 and td.seq2 = p.seq2
left join Orders o with(nolock) on o.id = td.poid
where 1=1
");
                if (!MyUtility.Check.Empty(this.Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", this.Issuedate1.Value.ToShortDateString(), this.Issuedate2.Value.ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", this.SP1));
                }

                if (!MyUtility.Check.Empty(this.SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <= '{0}'", this.SP2));
                }

                if (!MyUtility.Check.Empty(this.mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", this.mdivisionid));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", this.factory));
                }

                if (!MyUtility.Check.Empty(this.materialType))
                {
                    sqlCmd.Append(string.Format(@" and p.FabricType  = '{0}'", this.materialType));
                }
            }

            return sqlCmd;
        }

        private StringBuilder SQLCmd_Summary()
        {
            StringBuilder sqlCmd = new StringBuilder();

            if (this.type == 0)
            {
                sqlCmd.Append(@"
select distinct
	t.id,t.MDivisionID,t.FromFtyID,t.FactoryID,t.IssueDate,t.Status
	,td.POID, o.StyleID, p.Refno, p.ColorID, p.SizeSpec
	,td.Seq1, td.Seq2
	,[StockType] = case when td.StockType = 'B' then 'Bulk'
	        when td.StockType = 'I' then 'Inventory'
	        end
	,tds.Qty
	,[Unit] = dbo.GetStockUnitBySPSeq (td.poid, td.seq1, td.seq2)
	,t.Remark
	,Description = dbo.getMtlDesc(td.POID, td.seq1,td.seq2,2,0)
from TransferIn t with(nolock)
inner join TransferIn_Detail td with(nolock) on td.id = t.id
left join Po_Supp_Detail p WITH (NOLOCK) on td.poid = p.id and td.seq1 = p.seq1 and td.seq2 = p.seq2
left join Orders o with(nolock) on o.id = td.poid 
outer apply (
	select [Qty] = sum(Qty)
	from TransferIn_Detail ttd with(nolock)
	where ttd.id = td.id
	and ttd.StockType = td.StockType
	and ttd.POID = td.POID
	and ttd.seq1 = td.seq1 
	and ttd.seq2 = td.Seq2
	group by ttd.id, ttd.StockType, ttd.poid, ttd.seq1, ttd.seq2
)tds
where 1=1
");
                if (!MyUtility.Check.Empty(this.Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", this.Issuedate1.Value.ToShortDateString(), this.Issuedate2.Value.ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", this.SP1));
                }

                if (!MyUtility.Check.Empty(this.SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <='{0}'", this.SP2));
                }

                if (!MyUtility.Check.Empty(this.mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", this.mdivisionid));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", this.factory));
                }

                if (!MyUtility.Check.Empty(this.materialType))
                {
                    sqlCmd.Append(string.Format(@" and p.FabricType  = '{0}'", this.materialType));
                }
            }
            else
            {
                sqlCmd.Append(@"
select distinct
	t.id,t.MDivisionID,t.FactoryID,t.ToMDivisionid,t.IssueDate,t.Status
	,td.POID, o.StyleID, p.Refno, p.ColorID, p.SizeSpec
	,td.Seq1, td.Seq2
	,[StockType] = case when td.StockType = 'B' then 'Bulk'
	        when td.StockType = 'I' then 'Inventory'
	        end
	,tds.Qty
	,[Unit] = dbo.GetStockUnitBySPSeq (td.poid, td.seq1, td.seq2)
	,t.Remark
	,Description = dbo.getMtlDesc(td.POID,td.seq1,td.seq2,2,0)
from TransferOut t with(nolock)
inner join TransferOut_Detail td with(nolock) on td.id = t.id
left join Po_Supp_Detail p WITH (NOLOCK) on td.poid = p.id and td.seq1 = p.seq1 and td.seq2 = p.seq2
left join Orders o with(nolock) on o.id = td.poid 
outer apply (
	select [Qty] = sum(Qty)
	from TransferOut_Detail ttd with(nolock)
	where ttd.id = td.id
	and ttd.StockType = td.StockType
	and ttd.POID = td.POID
	and ttd.seq1 = td.seq1 
	and ttd.seq2 = td.Seq2
	group by ttd.id, ttd.StockType, ttd.poid, ttd.seq1, ttd.seq2
)tds
where 1=1
");
                if (!MyUtility.Check.Empty(this.Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", this.Issuedate1.Value.ToShortDateString(), this.Issuedate2.Value.ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", this.SP1));
                }

                if (!MyUtility.Check.Empty(this.SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <= '{0}'", this.SP2));
                }

                if (!MyUtility.Check.Empty(this.mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", this.mdivisionid));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", this.factory));
                }

                if (!MyUtility.Check.Empty(this.materialType))
                {
                    sqlCmd.Append(string.Format(@" and p.FabricType  = '{0}'", this.materialType));
                }
            }

            return sqlCmd;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string fileName = this.radioDetail.Checked ? "Warehouse_R05_Detail.xltx" : "Warehouse_R05_Summary.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + fileName); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 5] = this.type == 0 ? "Arrive W/H Date" : "Issue Date";
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, fileName, 1, true, null, objApp);      // 將datatable copy to excel
            Marshal.ReleaseComObject(objSheets);
            return true;
        }
    }
}
