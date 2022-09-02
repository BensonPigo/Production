using Ict;
using Sci.Data;
using Sci.Win;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
	/// <inheritdoc/>
	public partial class R42 : Win.Tems.PrintForm
	{
		private DataTable dtResult;

		/// <inheritdoc/>
		public R42(ToolStripMenuItem menuitem)
			: base(menuitem)
		{
			this.InitializeComponent();
		}

		/// <inheritdoc/>
		protected override bool ValidateInput()
		{
			if (!this.dateIssue.HasValue && this.txtSP1.Text.Empty() && this.txtSP2.Text.Empty())
			{
				MyUtility.Msg.WarningBox("Issue Date and SP# cannot all be empty.");
				return false;
			}

			return base.ValidateInput();
		}

		/// <inheritdoc/>
		protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
		{
			string sqlWhere = string.Empty;
			List<SqlParameter> listPar = new List<SqlParameter>();

			if (!MyUtility.Check.Empty(this.dateIssue.Value1))
			{
				sqlWhere += $@" and a.IssueDate >= @IssueDate1";
				listPar.Add(new SqlParameter("@IssueDate1", this.dateIssue.Value1));
			}

			if (!MyUtility.Check.Empty(this.dateIssue.Value2))
			{
				sqlWhere += $@" and a.IssueDate <= @IssueDate2";
				listPar.Add(new SqlParameter("@IssueDate2", this.dateIssue.Value2));
			}

			if (!MyUtility.Check.Empty(this.txtSP1.Text))
			{
				sqlWhere += $@" and ad.POID >= @SP1";
				listPar.Add(new SqlParameter("@SP1", this.txtSP1.Text));
			}

			if (!MyUtility.Check.Empty(this.txtSP2.Text))
			{
				sqlWhere += $@" and ad.POID <= @SP2";
				listPar.Add(new SqlParameter("@SP2", this.txtSP2.Text));
			}

			if (!MyUtility.Check.Empty(this.txtMdivision.Text))
			{
				sqlWhere += $@" and wo.MDivisionID = @Mdivision";
				listPar.Add(new SqlParameter("@Mdivision", this.txtMdivision.Text));
			}

			if (!MyUtility.Check.Empty(this.txtFactory.Text))
			{
				sqlWhere += $@" and wo.FtyGroup = @FtyGroup";
				listPar.Add(new SqlParameter("@FtyGroup", this.txtFactory.Text));
			}

			string sqlQuery = $@"
/*
	取出物料清單
	7X 領料項需要額外取得庫存項的資訊
	為了後面判斷收料做準備
*/
select distinct 
		wo.MDivisionID
		, wo.FactoryID
		, wo.BrandID
		, ad.POID
		, ad.Seq1
		, ad.Seq2
		, ad.Roll
		, ad.Dyelot
		, psd.StockPOID
		, psd.StockSeq1
		, psd.StockSeq2
into #MtlList
from Adjust a with(nolock)
inner join Adjust_Detail ad with(nolock) on a.id = ad.id
inner join View_WH_Orders wo with(nolock) on ad.poid = wo.ID
inner join PO_Supp_Detail psd with(nolock) on ad.POID = psd.ID
												and ad.Seq1 = psd.SEQ1
												and ad.Seq2 = psd.SEQ2		
where a.type = 'R'
and a.Status = 'Confirmed'
{sqlWhere}
/*
	取出收料單最早的收進倉庫時間
	7X 領料項需要使用庫存項的 POID, Seq 找
	1. 如果是同工廠領料可以找到庫存項的收料資訊
	2. 如果是跨 System 領料則用領料項 Transfer In 也同樣可以取得
*/
select ReceivingList.POID
		, ReceivingList.Seq1
		, ReceivingList.Seq2
		, ReceivingList.Roll
		, ReceivingList.Dyelot
		, FirstWhseArrival = min(ReceivingList.WhseArrival)
into #ReceivingList
from (
	select rd.PoId
			, rd.Seq1
			, rd.Seq2
			, rd.Roll
			, rd.Dyelot
			, r.WhseArrival
	from Receiving r with(nolock)
	inner join Receiving_Detail rd with(nolock) on r.id = rd.Id
	where r.Status = 'Confirmed'
			and exists (
				select 1
				from #MtlList ml
				where rd.PoId = ml.POID
						and rd.Seq1 = ml.Seq1
						and rd.Seq2 = ml.Seq2
						and rd.Roll = ml.Roll
						and rd.dyelot = ml.Dyelot
			)
	union all
	select tid.PoId
			, tid.Seq1
			, tid.Seq2
			, tid.Roll
			, tid.Dyelot
			, WhseArrival = ti.IssueDate
	from TransferIn ti with(nolock)
	inner join TransferIn_Detail tid with(nolock) on ti.id = tid.Id
	where ti.Status = 'Confirmed'
			and exists (
				select 1
				from #MtlList ml
				where tid.PoId = ml.POID
						and tid.Seq1 = ml.Seq1
						and tid.Seq2 = ml.Seq2
						and tid.Roll = ml.Roll
						and tid.dyelot = ml.Dyelot
			)
	-- 7X 領料項
	union all
	select ml.PoId
			, ml.Seq1
			, ml.Seq2
			, ml.Roll
			, ml.Dyelot
			, WhseArrival = ti.IssueDate
	from #MtlList ml
	inner join TransferIn_Detail tid with(nolock) on ml.StockPOID = tid.POID
														and ml.StockSeq1 = tid.Seq1
														and ml.StockSeq2 = tid.Seq2
	inner join TransferIn ti with(nolock) on tid.id = ti.Id
	where ml.StockPOID != ''
			and ti.Status = 'Confirmed'
	union all
	select ml.PoId
			, ml.Seq1
			, ml.Seq2
			, ml.Roll
			, ml.Dyelot
			, WhseArrival = r.WhseArrival
	from #MtlList ml
	inner join Receiving_Detail rd with(nolock) on ml.StockPOID = rd.POID
														and ml.StockSeq1 = rd.Seq1
														and ml.StockSeq2 = rd.Seq2
	inner join Receiving r with(nolock) on rd.id = r.Id
	where ml.StockPOID != ''
			and r.Status = 'Confirmed'
) ReceivingList
group by ReceivingList.POID
		, ReceivingList.Seq1
		, ReceivingList.Seq2
		, ReceivingList.Roll
		, ReceivingList.Dyelot

/*
	A to C 與 B to C 清單
*/
select ml.POID
		, ml.Seq1
		, ml.Seq2
		, ml.Roll
		, ml.Dyelot
		, LastTrasferScrapDate = max (st.IssueDate)
into #TransferList
from #MtlList ml
inner join SubTransfer_Detail std with(nolock) on ml.POID = std.FromPOID
													and ml.Seq1 = std.FromSeq1
													and ml.Seq2 = std.FromSeq2
													and ml.Roll = std.FromRoll
													and ml.Dyelot = std.FromDyelot
inner join SubTransfer st with(nolock) on std.id = st.Id
where st.Status = 'Confirmed'
		and st.type in ('E', 'D')
group by ml.POID
		, ml.Seq1
		, ml.Seq2
		, ml.Roll
		, ml.Dyelot

/*
	C 倉報廢清單
*/
select ml.POID
		, ml.Seq1
		, ml.Seq2
		, ml.Roll
		, ml.Dyelot
		, a.ID
		, a.IssueDate
		, ad.QtyBefore
		, Reason = Concat (ad.ReasonId, ' ', r.Name)
		, a.EditDate
		, a.EditName
		, rnd = ROW_NUMBER() over (partition by ml.POID, ml.Seq1, ml.Seq2, ml.Roll, ml.Dyelot order by a.IssueDate desc, a.EditDate desc)
into #RemoveList
from #MtlList ml
inner join Adjust_Detail ad with (nolock) on ml.POID = ad.POID
												and ml.Seq1 = ad.Seq1
												and ml.Seq2 = ad.Seq2
												and ml.Roll = ad.Roll
												and ml.Dyelot = ad.Dyelot
inner join Adjust a with(nolock) on ad.id = a.id
left join Reason r with(nolock) on r.ReasonTypeID = 'Stock_Remove'
									and ad.ReasonId = r.ID
where a.Type = 'R'
		and a.status = 'Confirmed'


/*
	最終彙整
*/
select ml.MDivisionID
		, ml.FactoryID
		, ml.POID
		, Seq = Concat (ml.Seq1, ' ', ml.Seq2)
		, ml.Roll
		, ml.Dyelot
		, ml.StockPOID
		, StockSeq = Concat (ml.Seq1, ' ', ml.Seq2)
		, psd.Refno
		, MaterialType = Concat (iif(psd.FabricType='F','Fabric',iif(psd.FabricType='A','Accessory',iif(psd.FabricType='O','Orher',psd.FabricType))), '-', f.MtlTypeID)
		, f.WeaveTypeID
		, Color = IIF(f.MtlTypeID = 'EMB THREAD' OR f.MtlTypeID = 'SP THREAD' OR f.MtlTypeID = 'THREAD' 
                                     ,IIF( psd.SuppColor = '' or psd.SuppColor is null,dbo.GetColorMultipleID(ml.BrandID,psd.ColorID),psd.SuppColor)
                                     ,dbo.GetColorMultipleID(ml.BrandID,psd.ColorID)
                                 )
		, CurrentStock = fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty
		, rl.FirstWhseArrival
		, tl.LastTrasferScrapDate
		, LastDisposeDate = rel.IssueDate
		, R45ID = stuff((	select ',' + ID
									from (	
										select R45.ID	
										from #RemoveList R45
										where  ml.POID = R45.POID
												and ml.Seq1 = R45.Seq1
												and ml.Seq2 = R45.Seq2
												and ml.Roll = R45.Roll
												and ml.Dyelot = R45.Dyelot
									) R45
									for xml path(''))
								, 1, 1, '')
		, R45Reason= stuff((	select ',' + Reason
									from (	
										select R45.Reason	
										from #RemoveList R45
										where  ml.POID = R45.POID
												and ml.Seq1 = R45.Seq1
												and ml.Seq2 = R45.Seq2
												and ml.Roll = R45.Roll
												and ml.Dyelot = R45.Dyelot
									) R45
									for xml path(''))
								, 1, 1, '')
from #MtlList ml
left join FtyInventory fi with(nolock) on ml.POID = fi.POID
											and ml.Seq1 = fi.Seq1
											and ml.Seq2 = fi.Seq2
											and ml.Roll = fi.Roll
											and ml.Dyelot = fi.Dyelot
											and fi.StockType = 'O'
left join PO_Supp_Detail psd with(nolock) on ml.POID = psd.ID
											and ml.Seq1 = psd.SEQ1
											and ml.Seq2 = psd.SEQ2											
left join Fabric f with(nolock) on psd.SCIRefno = f.SCIRefno
left join #ReceivingList rl on ml.POID = rl.PoId
								and ml.Seq1 = rl.Seq1 
								and ml.Seq2 = rl.Seq2
								and ml.Roll = rl.Roll
								and ml.Dyelot = rl.Dyelot
left join #TransferList tl on ml.POID = tl.PoId
								and ml.Seq1 = tl.Seq1
								and ml.Seq2 = tl.Seq2 
								and ml.Roll = tl.Roll
								and ml.Dyelot = tl.Dyelot
left join #RemoveList rel on rel.rnd = 1
								and ml.POID = rel.POID
								and ml.Seq1 = rel.Seq1
								and ml.Seq2 = rel.Seq2
								and ml.Roll = rel.Roll
								and ml.Dyelot = rel.Dyelot
order by ml.MDivisionID, ml.FactoryID, ml.POID, ml.Seq1, ml.Seq2, ml.Roll, ml.Dyelot

drop table #MtlList, #ReceivingList, #TransferList, #RemoveList
";
			return DBProxy.Current.Select(null, sqlQuery, listPar, out this.dtResult);
		}

		/// <inheritdoc/>
		protected override bool OnToExcel(ReportDefinition report)
		{
			this.SetCount(this.dtResult.Rows.Count);
			if (this.dtResult.Rows.Count == 0)
			{
				MyUtility.Msg.InfoBox("Data not found!!");
				return false;
			}

			this.ShowWaitMessage("Excel Processing...");

			string reportName = "Warehouse_R42";

			Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}.xltx"); // 預先開啟excel app
			MyUtility.Excel.CopyToXls(this.dtResult, null, $"{reportName}.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
			Excel.Worksheet worksheet = objApp.Sheets[1];
			worksheet.Rows.AutoFit();
			worksheet.Columns.AutoFit();

			#region Save & Show Excel
			string strExcelName = Class.MicrosoftFile.GetName(reportName);
			objApp.ActiveWorkbook.SaveAs(strExcelName);
			objApp.Quit();
			Marshal.ReleaseComObject(objApp);
			Marshal.ReleaseComObject(worksheet);

			strExcelName.OpenFile();
			#endregion
			this.HideWaitMessage();
			return true;
		}
	}
}
