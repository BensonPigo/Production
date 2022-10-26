using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R44 : Win.Tems.PrintForm
    {
        private DataTable[] dts;
        private List<SqlParameter> listPar = new List<SqlParameter>();
        private string sqlcmd;

        /// <inheritdoc/>
        public R44(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboBoxTransferType, 2, 1, "0,ALL,1,Transfer in,2,Transfer Out");
            this.comboBoxFTYStatus.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateTWSendDate.HasValue &&
                !this.dateETA.HasValue &&
                this.txtTK1.Text.Empty() &&
                this.txtTK2.Text.Empty())
            {
                MyUtility.Msg.WarningBox("TW Send Date, ETA, TK cannot all be empty.");
                return false;
            }

            this.listPar.Clear();
            this.sqlcmd = string.Empty;
            string sqlWhere = string.Empty;
            if (this.dateTWSendDate.HasValue1)
            {
                sqlWhere += $"\r\nand cast(te.SendDate as Date) between '{(DateTime)this.dateTWSendDate.Value1:yyyy/MM/dd}' and '{(DateTime)this.dateTWSendDate.Value2:yyyy/MM/dd}'";
            }

            if (this.dateETA.HasValue1)
            {
                sqlWhere += $"\r\nand te.ETA between '{(DateTime)this.dateETA.Value1:yyyy/MM/dd}' and '{(DateTime)this.dateETA.Value2:yyyy/MM/dd}'";
            }

            if (!MyUtility.Check.Empty(this.txtTK1.Text) && !MyUtility.Check.Empty(this.txtTK2.Text))
            {
                sqlWhere += $"\r\nand te.ID between @ID1 and @ID2";
                this.listPar.Add(new SqlParameter("@ID1", this.txtTK1.Text));
                this.listPar.Add(new SqlParameter("@ID2", this.txtTK2.Text));
            }
            else if (!MyUtility.Check.Empty(this.txtTK1.Text))
            {
                sqlWhere += $"\r\nand te.ID = @ID1";
                this.listPar.Add(new SqlParameter("@ID1", this.txtTK1.Text));
            }
            else if (!MyUtility.Check.Empty(this.txtTK2.Text))
            {
                sqlWhere += $"\r\nand te.ID = @ID2";
                this.listPar.Add(new SqlParameter("@ID2", this.txtTK2.Text));
            }

            if (!MyUtility.Check.Empty(this.comboDropDownList1.SelectedValue))
            {
                sqlWhere += $"\r\nand ted.FabricType in ({this.comboDropDownList1.SelectedValue})";
            }

            if (!this.comboBoxFTYStatus.Text.Equals("ALL"))
            {
                sqlWhere += $"\r\nand te.FtyStatus = '{this.comboBoxFTYStatus.Text}'";
            }

            if (this.chkExcludeJunk.Checked)
            {
                sqlWhere += $"\r\nand te.Junk = 0";
            }

            string where2 = string.Empty;
            if (!MyUtility.Check.Empty(this.txtMdivision1.Text))
            {
                where2 += $" and f.MDivisionID = '{this.txtMdivision1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                where2 += $" and f.FTYGroup = '{this.txtfactory1.Text}'";
            }

            string wherein = $"\r\nand exists(select 1 from Factory f with(nolock) where f.id = te.FactoryID {where2})";
            string whereout = $"\r\nand exists(select 1 from Factory f with(nolock) where f.id = te.FromFactoryID {where2})";

            // Summary
            if (this.radioSummary.Checked)
            {
                // Trans in
                if (this.comboBoxTransferType.SelectedValue.EqualString("0") || this.comboBoxTransferType.SelectedValue.EqualString("1"))
                {
                    this.sqlcmd += $@"
select [TK No.] = te.ID
		, [TW Send Date] = te.SendDate
		, [ETA] = te.Eta
		, [From Factory] = te.FromFactoryID
		, [To Factory] = te.FactoryID
		, [Consignee] = te.Consignee
		, [ShipModeID] = te.ShipModeID
		, [TPE Status] = case 
							when te.Junk = 1 then 'Junk' 
							when te.Confirm = 1 then 'Confirm' 
							when te.Sent = 1 then 'Sent' 
							else 'New' 
						  end
		, [Fty Status] = te.FtyStatus
		, [P/L Rcv Date] = te.PackingArrival
		, [Arrive Port Date] = te.PortArrival
		, [Arrive W/H Date] = te.WhseArrival
		, [Dox Rcv Date] = te.DocArrival
		, [From SP#] = ted.InventoryPOID
		, [From SEQ] = CONCAT (ted.InventorySeq1, ' ', ted.InventorySeq2)
		, [To SP#] = ted.PoID
		, [To SEQ] = CONCAT (ted.Seq1, ' ', ted.Seq2)
		, [Suppier] = ted.SuppID
		, [Desc] = ted.Description
		, [Material Type] = Concat(
                case ted.FabricType
                when 'F' then 'Fabric'
                when 'A' then 'Accessory'
                when 'O' then 'Other'
                else ted.FabricType
                end
                , '-' +  f.MtlTypeID)
		, [Color] = psd.ColorID
		, [Size] = psd.SizeSpec
		, [Stock Unit] = psd.StockUnit
		, [Po Q'ty] = dbo.GetUnitQty (ted.UnitID, psd.StockUnit, ted.PoQty)
		, [Export Q'ty] = TranserOut.Qty
		, [In Q'ty] = TransferIn.Qty
		, [Balance] = isnull(TranserOut.Qty, 0) - isnull(TransferIn.Qty, 0)
		, [Reason] = ted.TransferExportReason
		, [Reason Desc] = wr.Description
		, [N.W.(kg)] = ted.NetKg
		, [G.W.(kg)] = ted.WeightKg
		, [CBM] = ted.CBM
from TransferExport te with (nolock)
Left join TransferExport_Detail ted with (nolock) on te.ID = ted.ID
left join Fabric f with (nolock) on ted.SCIRefno = f.SCIRefno
left join WhseReason wr with (nolock) on wr.Type = 'TE'
											and ted.TransferExportReason = wr.ID
left join PO_Supp_Detail psd with (nolock) on	ted.POID = psd.ID  
												and ted.Seq1 = psd.SEQ1 
												and ted.Seq2 = psd.SEQ2
outer apply (
	select Qty = SUM (round(dbo.GetUnitQty(tedc.StockUnitID, psd.StockUnit, tedc.StockQty),2))
	from TransferExport_Detail_Carton tedc
	left join PO_Supp_Detail psd with (nolock) on tedc.PoID = psd.ID
													and tedc.Seq1 = psd.SEQ1
													and tedc.Seq2 = psd.SEQ2
	where ted.Ukey = tedc.TransferExport_DetailUkey
) TranserOut
outer apply (
	select Qty = SUM (tid.Qty)
	from TransferIn ti
	inner join TransferIn_Detail tid on ti.Id = tid.ID
	where ti.TransferExportID = te.Id
			and tid.POID = ted.PoID
			and tid.Seq1 = ted.Seq1
			and tid.Seq2 = ted.Seq2
			and ti.Status = 'Confirmed'
) TransferIn
where te.TransferType = 'Transfer In'
{wherein}
{sqlWhere}
order by ted.id,ted.Ukey
";
                }

                // Trans out
                if (this.comboBoxTransferType.SelectedValue.EqualString("0") || this.comboBoxTransferType.SelectedValue.EqualString("2"))
                {
                    this.sqlcmd += $@"
select [TK No.] = te.ID
		, [TW Send Date] = te.SendDate
		, [ETA] = te.Eta
		, [From Factory] = te.FromFactoryID
		, [To Factory] = te.FactoryID
		, [Consignee] = te.Consignee
		, [ShipModeID] = te.ShipModeID
		, [TPE Status] = case 
							when te.Junk = 1 then 'Junk' 
							when te.Confirm = 1 then 'Confirm' 
							when te.Sent = 1 then 'Sent' 
							else 'New' 
						  end
		, [Fty Status] = te.FtyStatus
		, [From SP#] = ted.InventoryPOID
		, [From SEQ] = CONCAT (ted.InventorySeq1, ' ', ted.InventorySeq2)
		, [To SP#] = ted.PoID
		, [To SEQ] = CONCAT (ted.Seq1, ' ', ted.Seq2)
		, [Suppier] = ted.SuppID
		, [Desc] = ted.Description
		, [Material Type] = Concat(
                case ted.FabricType
                when 'F' then 'Fabric'
                when 'A' then 'Accessory'
                when 'O' then 'Other'
                else ted.FabricType
                end
                , '-' +  f.MtlTypeID)
		, [Color] = psdinv.ColorID
		, [Size] = psdinv.SizeSpec
		, [Stock Unit] = psdinv.StockUnit
		, [Po Q'ty]
		, [Export Q'ty] = TranserOut.Qty
		, [Balance] = isnull([Po Q'ty], 0) - isnull(TranserOut.Qty, 0)
		, [Reason] = ted.TransferExportReason
		, [Reason Desc] = wr.Description
		, [N.W.(kg)] = ted.NetKg
		, [G.W.(kg)] = ted.WeightKg
		, [CBM] = ted.CBM
		, [WK#] = WK.ExportId
from TransferExport te with (nolock)
Left join TransferExport_Detail ted with (nolock) on te.ID = ted.ID
left join Fabric f with (nolock) on ted.SCIRefno = f.SCIRefno
left join WhseReason wr with (nolock) on wr.Type = 'TE'
											and ted.TransferExportReason = wr.ID
left join PO_Supp_Detail psdInv with (nolock) on	ted.InventoryPOID = psdInv.ID  
													and ted.InventorySeq1 = psdInv.SEQ1 
													and ted.InventorySeq2 = psdinv.SEQ2
outer apply(
	select ExportId = Stuff((
		select concat(',',ExportId)
		from (
				select 	distinct r.ExportId
				from TransferExport_Detail_Carton tedc with (nolock)
				inner join Receiving_Detail rd with (nolock) on rd.PoId = ted.InventoryPOID 
																and rd.Seq1 = ted.InventorySeq1
																and rd.Seq2 = ted.InventorySeq2 
																and rd.Roll = tedc.Carton
																and rd.Dyelot = tedc.LotNo
				inner join Receiving r with (nolock) on rd.Id = r.Id
				where ted.Ukey = tedc.TransferExport_DetailUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) WK
outer apply (
	select Qty = SUM (tedc.StockQty)   
	from TransferExport_Detail_Carton tedc
	where ted.Ukey = tedc.TransferExport_DetailUkey
) TranserOut
outer apply (select [Po Q'ty] = dbo.GetUnitQty (ted.UnitID, psdinv.StockUnit, ted.PoQty))StockUnitQty
where te.TransferType = 'Transfer Out'
{whereout}
{sqlWhere}
order by ted.id,ted.Ukey
";
                }
            }

            // Detail
            if (this.radioDetail.Checked)
            {
                // Trans in
                if (this.comboBoxTransferType.SelectedValue.EqualString("0") || this.comboBoxTransferType.SelectedValue.EqualString("1"))
                {
                    this.sqlcmd += $@"
 select distinct
          te.ID
		, te.SendDate
		, te.Eta
		, te.FromFactoryID
		, te.FactoryID
		, te.Consignee
		, te.ShipModeID
		, te.FtyStatus
into #tmp
from TransferExport te with (nolock)
Left join TransferExport_Detail ted with (nolock) on te.ID = ted.ID
where te.TransferType = 'Transfer In'
{wherein}
{sqlWhere}

--TransferExport_Detail有,TransferIn_Detail沒有
select [TK No.] = te.ID
	, [TW Send Date] = te.SendDate
	, [ETA] = te.Eta
	, [From Factory] = te.FromFactoryID
	, [To Factory] = te.FactoryID
	, [Consignee] = te.Consignee
	, [ShipModeID] = te.ShipModeID
	, [Fty Status] = te.FtyStatus
	, [From SP#] = ted.InventoryPOID
	, [From SEQ] = CONCAT (ted.InventorySeq1, ' ', ted.InventorySeq2)
	, [To SP#] = ted.PoID
	, [To SEQ] = CONCAT (ted.Seq1, ' ', ted.Seq2)
	, [Suppier] = ted.SuppID
	, [Desc] = ted.Description
	, [Material Type] = Concat(
            case ted.FabricType
            when 'F' then 'Fabric'
            when 'A' then 'Accessory'
            when 'O' then 'Other'
            else ted.FabricType
            end
            , '-' +  f.MtlTypeID)
	, [REF#] = ted.Refno
	, [Roll] = tedc.Carton
	, [Dyelot] = tedc.LotNo
	, [Stock Type] = case tid.StockType
						when 'b' then 'Bulk'
						when 'i' then 'Inventory'
						when 'o' then 'Scrap'
					end
	, [Color] = psd.ColorID
	, [Size] = psd.SizeSpec
	, [Stock Unit] = psd.StockUnit
	, [Po Q'ty] = dbo.GetUnitQty (ted.UnitID, psd.StockUnit, ted.PoQty)
	, [Export Q'ty]
	, [In Q'ty] = tid.Qty
	, [Balance] = isnull([Export Q'ty], 0) - isnull(tid.Qty, 0)
from #tmp te with (nolock)
Left join TransferExport_Detail ted with (nolock) on te.ID = ted.ID
left join Fabric f with (nolock) on ted.SCIRefno = f.SCIRefno
left join TransferExport_Detail_Carton tedc with (nolock) on ted.Ukey = tedc.TransferExport_DetailUkey
left join PO_Supp_Detail psd with (nolock) on ted.POID = psd.ID and ted.Seq1 = psd.SEQ1 and ted.Seq2 = psd.SEQ2
outer apply (select [Export Q'ty] = dbo.GetUnitQty (tedc.StockUnitID, psd.StockUnit, tedc.StockQty))StockUnitQty
outer apply (
	select tid.Qty,tid.StockType,ti.Id
	from TransferIn ti
	inner join TransferIn_Detail tid on ti.Id = tid.ID
	where ti.TransferExportID = te.Id
	and tid.POID = ted.PoID
	and tid.Seq1 = ted.Seq1
	and tid.Seq2 = ted.Seq2
	and tid.Roll = tedc.Carton
	and tid.Dyelot = tedc.LotNo
	and ti.Status = 'Confirmed'
) tid

union all
    
select [TK No.] = te.ID
	, [TW Send Date] = te.SendDate
	, [ETA] = te.Eta
	, [From Factory] = te.FromFactoryID
	, [To Factory] = te.FactoryID
	, [Consignee] = te.Consignee
	, [ShipModeID] = te.ShipModeID
	, [Fty Status] = te.FtyStatus
	, [From SP#] = ted.InventoryPOID
	, [From SEQ] = CONCAT (ted.InventorySeq1, ' ', ted.InventorySeq2)
	, [To SP#] = tid.PoID
	, [To SEQ] = CONCAT (tid.Seq1, ' ', tid.Seq2)
	, [Suppier] = ted.SuppID
	, [Desc] = dbo.getMtlDesc(tid.Poid,tid.SEQ1,tid.SEQ2,2,0)
	, [Material Type] = Concat(
            case psd.FabricType
            when 'F' then 'Fabric'
            when 'A' then 'Accessory'
            when 'O' then 'Other'
            else ted.FabricType
            end
            , '-' +  f.MtlTypeID)
	, [REF#] = ted.Refno
	, [Roll] = tid.Roll
	, [Dyelot] = tid.Dyelot
	, [Stock Type] = case tid.StockType
						when 'b' then 'Bulk'
						when 'i' then 'Inventory'
						when 'o' then 'Scrap'
					end
	, [Color] = psd.ColorID
	, [Size] = psd.SizeSpec
	, [Stock Unit] = psd.StockUnit
	, [Po Q'ty] = dbo.GetUnitQty (ted.UnitID, psd.StockUnit, ted.PoQty)
	, [Export Q'ty]
	, [In Q'ty] = tid.Qty
	, [Balance] = isnull([Export Q'ty], 0) - isnull(tid.Qty, 0)
from #tmp te
inner join TransferIn ti with (nolock) on ti.TransferExportID = te.ID
inner join TransferIn_Detail tid on ti.Id = tid.ID
left join PO_Supp_Detail psd with (nolock) on tid.POID = psd.ID and tid.Seq1 = psd.SEQ1 and tid.Seq2 = psd.SEQ2
left join Fabric f with (nolock) on psd.SCIRefno = f.SCIRefno
outer apply(
	select ted.*,[Export Q'ty]
	from TransferExport_Detail ted with (nolock)
	left join TransferExport_Detail_Carton tedc with (nolock) on ted.Ukey = tedc.TransferExport_DetailUkey
	outer apply (select [Export Q'ty] = dbo.GetUnitQty (tedc.StockUnitID, psd.StockUnit, tedc.StockQty))StockUnitQty
	where te.ID = ted.ID
	and tid.POID = ted.PoID
	and tid.Seq1 = ted.Seq1
	and tid.Seq2 = ted.Seq2
	and tid.Roll = tedc.Carton
	and tid.Dyelot = tedc.LotNo
)ted
where ti.Status = 'Confirmed'
and ted.ID is null
order by te.id

drop table #tmp
";
                }

                // Trans out
                if (this.comboBoxTransferType.SelectedValue.EqualString("0") || this.comboBoxTransferType.SelectedValue.EqualString("2"))
                {
                    this.sqlcmd += $@"
select [TK No.] = te.ID
		, [TW Send Date] = te.SendDate
		, [ETA] = te.Eta
		, [From Factory] = te.FromFactoryID
		, [To Factory] = te.FactoryID
		, [Consignee] = te.Consignee
		, [ShipModeID] = te.ShipModeID
		, [Fty Status] = te.FtyStatus
		, [WH Send Date] = te.FtySendDate
		, [Fty Shipping Confirm Date] = te.FtyConfirmDate
		, [From SP#] = ted.InventoryPOID
		, [From SEQ] = CONCAT (ted.InventorySeq1, ' ', ted.InventorySeq2)
		, [To SP#] = ted.PoID
		, [To SEQ] = CONCAT (ted.Seq1, ' ', ted.Seq2)
		, [Suppier] = ted.SuppID
		, [Desc] = ted.Description
		, [Material Type] = Concat(
                case ted.FabricType
                when 'F' then 'Fabric'
                when 'A' then 'Accessory'
                when 'O' then 'Other'
                else ted.FabricType
                end
                , '-' +  f.MtlTypeID)
		, [REF#] = ted.Refno
		, [Roll] = tedc.Carton
		, [Dyelot] = tedc.LotNo
		, [Stock Type] = case tod.StockType
							when 'b' then 'Bulk'
							when 'i' then 'Inventory'
							when 'o' then 'Scrap'
						  end
		, [Color] = psdinv.ColorID
		, [Size] = psdinv.SizeSpec
		, [Stock Unit] = psdinv.StockUnit
		, [Po Q'ty]
		, [Export Q'ty] = tedc.StockQty
		, [Balance] = isnull([Po Q'ty], 0) - isnull(TtlTranserOut.Qty, 0) --此欄位是(Po總項 - out總項)
		, [Need check] = IIF (tedc.TransferExport_DetailUkey is null, 'Y', '')
		, [WK#] = WK.ExportId
		, [Issue Date] = tfo.IssueDate
from TransferExport te with (nolock)
Left join TransferExport_Detail ted with (nolock) on te.ID = ted.ID
left join Fabric f with (nolock) on ted.SCIRefno = f.SCIRefno
left join TransferExport_Detail_Carton tedc with (nolock) on ted.Ukey = tedc.TransferExport_DetailUkey
left join PO_Supp_Detail psdInv with (nolock) on ted.InventoryPOID = psdInv.ID  
												 and ted.InventorySeq1 = psdInv.SEQ1 
												 and ted.InventorySeq2 = psdinv.SEQ2
left join TransferOut_Detail tod with (nolock) on tedc.TransferExport_DetailUkey = tod.TransferExport_DetailUkey
													and tedc.Carton = tod.Roll
													and tedc.LotNo = tod.Dyelot
left join TransferOut tfo with (nolock) on tod.ID = tfo.Id
outer apply (select [Po Q'ty] = dbo.GetUnitQty (ted.UnitID, psdinv.StockUnit, ted.PoQty))StockUnitQty
outer apply(
	select ExportId = Stuff((
		select concat(',',ExportId)
		from (
				select 	distinct r.ExportId
				from TransferExport_Detail_Carton tedc with (nolock)
				inner join Receiving_Detail rd with (nolock) on rd.PoId = ted.InventoryPOID 
																and rd.Seq1 = ted.InventorySeq1
																and rd.Seq2 = ted.InventorySeq2 
																and rd.Roll = tedc.Carton
																and rd.Dyelot = tedc.LotNo
				inner join Receiving r with (nolock) on rd.Id = r.Id
				where ted.Ukey = tedc.TransferExport_DetailUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) WK
outer apply (
	select Qty = SUM (tedc.StockQty)
	from TransferExport_Detail_Carton tedc
	where ted.Ukey = tedc.TransferExport_DetailUkey
) TtlTranserOut
where te.TransferType = 'Transfer Out'
{whereout}
{sqlWhere}
order by ted.id,ted.Ukey
";
                }
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd, this.listPar, out this.dts);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            int dataCount = 0;
            foreach (DataTable dt in this.dts)
            {
                dataCount += dt.Rows.Count;
            }

            this.SetCount(dataCount);
            if (dataCount == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");

            if (this.radioSummary.Checked)
            {
                switch (this.comboBoxTransferType.SelectedValue.ToString())
                {
                    case "0":
                        this.ProcessExcel(this.dts[0], "Warehouse_R44_SummaryTransin");
                        this.ProcessExcel(this.dts[1], "Warehouse_R44_SummaryTransout");
                        break;
                    case "1":
                        this.ProcessExcel(this.dts[0], "Warehouse_R44_SummaryTransin");
                        break;
                    case "2":
                        this.ProcessExcel(this.dts[0], "Warehouse_R44_SummaryTransout");
                        break;
                }
            }

            if (this.radioDetail.Checked)
            {
                switch (this.comboBoxTransferType.SelectedValue.ToString())
                {
                    case "0":
                        this.ProcessExcel(this.dts[0], "Warehouse_R44_DetailTransin");
                        this.ProcessExcel(this.dts[1], "Warehouse_R44_DetailTransout");
                        break;
                    case "1":
                        this.ProcessExcel(this.dts[0], "Warehouse_R44_DetailTransin");
                        break;
                    case "2":
                        this.ProcessExcel(this.dts[0], "Warehouse_R44_DetailTransout");
                        break;
                }
            }

            this.HideWaitMessage();
            return true;
        }

        private void ProcessExcel(DataTable dt, string fileName)
        {
            if (dt.Rows.Count == 0)
            {
                return;
            }

            Excel.Application excelapp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + fileName + ".xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, null, fileName + ".xltx", 1, showExcel: false, showSaveMsg: false, excelApp: excelapp);
            string excelfile = Class.MicrosoftFile.GetName(fileName);
            excelapp.ActiveWorkbook.SaveAs(excelfile);
            excelapp.Visible = true;
            Marshal.ReleaseComObject(excelapp);
        }
    }
}
