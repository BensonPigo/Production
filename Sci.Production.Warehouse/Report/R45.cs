using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R45 : Win.Tems.PrintForm
    {
        private List<string> sqlWherelist;
        private List<SqlParameter> lisSqlParameter;
        private string strSQLWhere = string.Empty;
        private DataTable printTable;

        /// <inheritdoc/>
        public R45(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strSQLWhere = string.Empty;
            this.sqlWherelist = new List<string>();
            this.lisSqlParameter = new List<SqlParameter>();

            if (MyUtility.Check.Empty(this.txtSP1.Text) &&
                MyUtility.Check.Empty(this.txtSP2.Text) &&
                MyUtility.Check.Empty(this.dateTransfertoSubconDate.Value1) &&
                MyUtility.Check.Empty(this.dateTransfertoSubconDate.Value2) &&
                MyUtility.Check.Empty(this.dateSubConReturnDate.Value1) &&
                MyUtility.Check.Empty(this.dateSubConReturnDate.Value2))
            {
                MyUtility.Msg.WarningBox("SP#, Transfer to Sub con Date and Sub con Return Date cannot all be empty.");
                return false;
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text) &&
                !MyUtility.Check.Empty(this.txtSP2.Text))
            {
                this.sqlWherelist.Add("f.POID >= @SP1 and f.POID <= @SP2");
                this.lisSqlParameter.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                this.lisSqlParameter.Add(new SqlParameter("@SP2", this.txtSP2.Text));
            }

            if (!MyUtility.Check.Empty(this.dateTransfertoSubconDate.Value1) &&
                !MyUtility.Check.Empty(this.dateTransfertoSubconDate.Value2))
            {
                this.sqlWherelist.Add("t.TransferOutDate between @dateTransfer1 and @dateTransfer2");
                this.lisSqlParameter.Add(new SqlParameter("@dateTransfer1", this.dateTransfertoSubconDate.Value1));
                this.lisSqlParameter.Add(new SqlParameter("@dateTransfer2", this.dateTransfertoSubconDate.Value2));
            }

            if (!MyUtility.Check.Empty(this.dateSubConReturnDate.Value1) &&
                !MyUtility.Check.Empty(this.dateSubConReturnDate.Value2))
            {
                this.sqlWherelist.Add("s.ReturnDate between @dateSubcon1 and @dateSubcon2");
                this.lisSqlParameter.Add(new SqlParameter("@dateSubcon1", this.dateSubConReturnDate.Value1));
                this.lisSqlParameter.Add(new SqlParameter("@dateSubcon2", this.dateSubConReturnDate.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtSeq.Seq1) &&
                !MyUtility.Check.Empty(this.txtSeq.Seq2))
            {
                this.sqlWherelist.Add("f.Seq1 = @Seq1 and f.Seq2 = @Seq2");
                this.lisSqlParameter.Add(new SqlParameter("@Seq1", this.txtSeq.Seq1));
                this.lisSqlParameter.Add(new SqlParameter("@Seq2", this.txtSeq.Seq2));
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                this.sqlWherelist.Add("psd.Refno = @Refno");
                this.lisSqlParameter.Add(new SqlParameter("@Refno", this.txtRefno.Text));
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                this.sqlWherelist.Add("o.MDivisionID = @MDivisionID");
                this.lisSqlParameter.Add(new SqlParameter("@MDivisionID", this.txtMdivision.Text));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                this.sqlWherelist.Add("o.FtyGroup = @FtyGroup");
                this.lisSqlParameter.Add(new SqlParameter("@FtyGroup", this.txtfactory.Text));
            }

            this.strSQLWhere = string.Join(" and ", this.sqlWherelist);
            if (this.sqlWherelist.Count != 0)
            {
                if (this.radioSummary.Checked)
                {
                    this.strSQLWhere = " where psd.FabricType = 'F' and " + this.strSQLWhere;
                }
                else
                {
                    this.strSQLWhere = " where f.StockType ='B' and psd.FabricType = 'F' and " + this.strSQLWhere;
                }
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            if (this.radioSummary.Checked)
            {
                sqlcmd = $@"select 
                            [SP#] =f.POID
                            ,[Seq#] = Concat (f.Seq1, ' ', f.Seq2)
                            ,psd.Refno
                            ,psd.StockUnit
                            ,[ReceivingQty] = isnull( rdQty.ActualQty,0) + isnull(tidQty.Qty,0)
                            ,[TransferToSubconStatus] = t.Status
                            ,[SubconReturnStatus] = s.Status
                            into #tmp
                            from FtyInventory f with(nolock) 
                            left join PO_Supp_Detail psd with(nolock) on f.POID = psd.ID and
											                             f.Seq1 = psd.SEQ1 and
											                             f.Seq2 = psd.SEQ2
                            left join TransferToSubcon_Detail td with(nolock) on f.POID = td.POID and
													                              f.Seq1 = td.Seq1 and
													                              f.Seq2 = td.Seq2 and
													                              f.Roll = td.Roll and
													                              f.Dyelot = td.Dyelot and
													                              f.StockType = td.StockType
                            left join TransferToSubcon t with(nolock) on t.ID = td.ID
                                                                         and t.Subcon = 'GMT Wash'
                                                                         and t.Status = 'Confirmed'
                            left join Orders o with(nolock) on f.POID = o.ID
                            left join SubconReturn_Detail sd with(nolock) on td.Ukey = sd.TransferToSubcon_DetailUkey
                            left join SubconReturn s with(nolock) on s.id = sd.ID
                                                                     and s.Status = 'Confirmed'
                                                                     and t.Subcon = s.Subcon
                            outer apply
                            (
	                            select rd.ActualQty
	                            from Receiving_Detail rd with(nolock)
	                            inner join Receiving r with(nolock) on rd.Id = r.id
	                            where r.Type = 'A' and
		                                td.POID = rd.PoId and
		                                td.Seq1 = rd.Seq1 and 
		                                td.Seq2 = rd.Seq2 and 
		                                td.Roll = rd.Roll and 
		                                td.Dyelot = rd.Dyelot 
                            )rdQty
                            outer apply
                            (
	                            select tid.Qty
	                            from TransferIn_Detail tid with(nolock)
	                            where  td.POID = tid.PoId and 
		                                td.Seq1 = tid.Seq1 and 
		                                td.Seq2 = tid.Seq2 and
		                                td.Roll = tid.Roll and 
		                                td.Dyelot = tid.Dyelot 
                            )tidQty
                            {this.strSQLWhere}

                            select 
                            [SP#]
                            ,[Seq#]
                            ,[Refno]
                            ,[StockUnit]
                            ,[ReceivingQty] = sum([ReceivingQty])
                            ,[Transfer out - GMT Wash] = sum(IIF([TransferToSubconStatus] = 'Confirmed',[ReceivingQty],0))
                            ,[Return - GMT Wash] = sum(IIF([SubconReturnStatus] = 'Confirmed',[ReceivingQty],0))
                            ,[Balance - GMT Wash] = cast(sum(IIF([TransferToSubconStatus] = 'Confirmed',[ReceivingQty],0)) as float) - cast(sum(IIF([SubconReturnStatus] = 'Confirmed',[ReceivingQty],0)) as float)
                            from #tmp 
                            group by [SP#],[Seq#],[Refno],[StockUnit]
                            order by [SP#],[Seq#]
                            drop table #tmp";
            }
            else
            {
                sqlcmd = $@"select 
                            [SP#] =f.POID
                            ,[Seq#] = Concat (f.Seq1, ' ', f.Seq2)
                            ,psd.Refno
                            ,f.Roll
                            ,f.Dyelot
                            ,psd.StockUnit
                            ,[ReceivingQty] = isnull( rdQty.ActualQty,0) + isnull(tidQty.Qty,0)
                            ,[TransferToSubconStatus] = t.Status
                            ,[SubconReturnStatus] = s.Status
                            ,t.TransferOutDate
                            ,s.ReturnDate
                            ,[GMTWash] = GMTWash.val
                            into #tmp
                            from FtyInventory f with(nolock) 
                            left join PO_Supp_Detail psd with(nolock) on f.POID = psd.ID and
											                             f.Seq1 = psd.SEQ1 and
											                             f.Seq2 = psd.SEQ2
                            left join TransferToSubcon_Detail td with(nolock) on f.POID = td.POID and
													                              f.Seq1 = td.Seq1 and
													                              f.Seq2 = td.Seq2 and
													                              f.Roll = td.Roll and
													                              f.Dyelot = td.Dyelot and
													                              f.StockType = td.StockType
                            left join TransferToSubcon t with(nolock) on t.ID = td.ID 
                                                                         and t.Subcon = 'GMT Wash'
                                                                         and t.Status = 'Confirmed'
                            left join Orders o with(nolock) on f.POID = o.ID
                            left join SubconReturn_Detail sd with(nolock) on td.Ukey = sd.TransferToSubcon_DetailUkey
                            left join SubconReturn s with(nolock) on s.id = sd.ID
                                                                     and s.Status = 'Confirmed'
                                                                     and t.Subcon = s.Subcon
                            outer apply
                            (
	                            select rd.ActualQty
	                            from Receiving_Detail rd with(nolock)
	                            inner join Receiving r with(nolock) on rd.Id = r.id
	                            where r.Type = 'A' and
		                                td.POID = rd.PoId and
		                                td.Seq1 = rd.Seq1 and 
		                                td.Seq2 = rd.Seq2 and 
		                                td.Roll = rd.Roll and 
		                                td.Dyelot = rd.Dyelot 
                            )rdQty
                            outer apply
                            (
	                            select tid.Qty
	                            from TransferIn_Detail tid with(nolock)
	                            where  td.POID = tid.PoId and 
		                                td.Seq1 = tid.Seq1 and 
		                                td.Seq2 = tid.Seq2 and
		                                td.Roll = tid.Roll and 
		                                td.Dyelot = tid.Dyelot 
                            )tidQty

                            outer apply
                            (
	                            select 
	                             val = td.POID
	                             from TransferToSubcon_Detail td with(nolock)
	                             where td.POID = f.PoId and 
		                               td.Seq1 = f.Seq1 and 
		                               td.Seq2 = f.Seq2 and
		                               td.Roll = f.Roll and 
		                               td.Dyelot = f.Dyelot and
		                               td.StockType = f.StockType
                            )GMTWash
                            {this.strSQLWhere}
                            group by f.POID,f.Seq1,f.Seq2,rdQty.ActualQty,tidQty.Qty,t.Status,s.Status,psd.Refno,psd.StockUnit,f.Roll,f.Dyelot,t.TransferOutDate,s.ReturnDate,f.StockType,GMTWash.val

                            select 
                            [SP#]
                            ,[Seq#]
                            ,[Refno]
                            ,[Roll]
                            ,[Dyelot]
                            ,[StockUnit]
                            ,[ReceivingQty] 
                            ,[GMTWash_B] = case when [SubconReturnStatus] = 'Confirmed' then 'Done'
					                            when [TransferToSubconStatus] = 'Confirmed' then 'Ongoing'
					                            else '' end
                            ,[TransferOutDate]
                            ,[ReturnDate]
                            from #tmp 
                            group by [SP#],[Seq#],[Refno],[StockUnit],[Roll],[Dyelot],[ReceivingQty],[TransferOutDate],[ReturnDate],[GMTWash],[TransferToSubconStatus],[SubconReturnStatus]
                            order by [SP#],[Seq#],[Roll],[Dyelot]
                            drop table #tmp";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, this.lisSqlParameter, out this.printTable);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string excelName = this.radioSummary.Checked ? "Warehouse_R45_Summary" : "Warehouse_R45_Detail";
            int excelRow = this.radioSummary.Checked ? 1 : 1;

            this.SetCount(this.printTable.Rows.Count);
            if (this.printTable == null || this.printTable.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + $"\\{excelName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printTable, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: excelRow, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);

            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;

            workbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion

            return true;
        }
    }
}
