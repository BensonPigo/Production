using Ict;
using Sci.Data;
using Sci.Production.Class.Command;
using Sci.Production.PublicForm;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P07_Print : Win.Tems.PrintForm
    {
        private DataTable dt;
        private string id;
        private string Date1;
        private string Date2;
        private string ETA;
        private string Invoice;
        private string Wk;
        private string FTYID;
        private string rptTitle;
        private DataRow curMain;

        /// <inheritdoc/>
        public P07_Print(List<string> polist, DataRow currentMain)
        {
            this.InitializeComponent();
            this.CheckControlEnable();
            this.poidList = polist;
            this.curMain = currentMain;
            DataTable dtPMS_FabricQRCode_LabelSize;
            DualResult result = DBProxy.Current.Select(null, "select ID, Name from dropdownlist where Type = 'PMS_Fab_LabSize' order by Seq", out dtPMS_FabricQRCode_LabelSize);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.comboType.DisplayMember = "Name";
            this.comboType.ValueMember = "ID";
            this.comboType.DataSource = dtPMS_FabricQRCode_LabelSize;

            this.comboType.SelectedValue = MyUtility.GetValue.Lookup("select PMS_FabricQRCode_LabelSize from system");
        }

        private List<string> poidList;

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.ReportResourceName == "P07_Report2.rdlc")
            {
                if (!MyUtility.Check.Empty(this.txtSPNo.Text) && !this.poidList.Contains(this.txtSPNo.Text.TrimEnd(), StringComparer.OrdinalIgnoreCase))
                {
                    MyUtility.Msg.ErrorBox("SP# is not found.");
                    return false;
                }
            }

            this.id = this.CurrentDataRow["ID"].ToString();

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            this.Date1 = MyUtility.Check.Empty(row["PackingReceive"]) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(row["PackingReceive"])).ToShortDateString();
            this.Date2 = MyUtility.Check.Empty(row["WhseArrival"]) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(row["WhseArrival"])).ToShortDateString();
            this.ETA = MyUtility.Check.Empty(row["ETA"]) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(row["ETA"])).ToShortDateString();
            this.Invoice = row["invno"].ToString();
            this.Wk = row["exportid"].ToString();
            this.FTYID = row["Mdivisionid"].ToString();

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", this.id));

            DataTable dtTitle;
            string cmdd =
                @"select m.nameEN
             from  dbo.Receiving r WITH (NOLOCK) 
             left join dbo.MDivision m WITH (NOLOCK) 
             on m.id = r.MDivisionID 
             where m.id = r.MDivisionID
             and r.id = @ID";
            DualResult titleResult = DBProxy.Current.Select(
                string.Empty, cmdd, pars, out dtTitle);
            if (!titleResult)
            {
                this.ShowErr(titleResult);
            }

            string strBLNO = MyUtility.GetValue.Lookup($@"Select Blno from dbo.export WITH (NOLOCK) where id='{this.Wk}'");
            this.rptTitle = dtTitle.Rows[0]["nameEN"].ToString();
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", this.rptTitle));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ETA", this.ETA));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", this.Invoice));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Wk", this.Wk));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FTYID", this.FTYID));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Blno", strBLNO.IsNullOrWhiteSpace() ? " " : strBLNO));
            DualResult result = this.LoadData();
            if (!result)
            {
                return result;
            }

            if (this.ReportResourceName == "P07_Report2.rdlc")
            {
                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date2", this.Date2));
                List<P07_PrintData> data = this.dt.AsEnumerable()
                                .Select(row1 => new P07_PrintData()
                                {
                                    Roll = row1["Roll"].ToString(),
                                    Dyelot = row1["dyelot"].ToString(),
                                    POID = row1["PoId"].ToString(),
                                    SEQ = row1["SEQ"].ToString(),
                                    BrandID = row1["BrandID"].ToString(),
                                    Desc = row1["Desc"].ToString().TrimEnd(new char[] { '\n', '\r' }),
                                    ShipQty = row1["ShipQty"].ToString(),
                                    Pounit = row1["pounit"].ToString(),
                                    StockQty = row1["StockQty"].ToString(),
                                    StockUnit = row1["StockUnit"].ToString(),
                                    SubStockQty = row1["SubStockQty"].ToString(),
                                    SubQty = row1["SubQty"].ToString(),
                                    Remark = row1["Remark"].ToString(),
                                }).ToList();

                e.Report.ReportDataSource = data;
            }
            else
            {
                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date1", this.Date1));
                List<P07_PrintData> data = this.dt.AsEnumerable()
                               .Select(row1 => new P07_PrintData()
                               {
                                   POID = row1["PoId"].ToString(),
                                   SEQ = row1["SEQ"].ToString(),
                                   BrandID = row1["BrandID"].ToString(),
                                   Roll = row1["Roll"].ToString(),
                                   Desc = row1["Desc"].ToString().TrimEnd(new char[] { '\n', '\r' }),
                                   ShipQty = row1["ShipQty"].ToString(),
                                   Pounit = row1["pounit"].ToString(),
                                   GW = row1["Weight"].ToString(),
                                   AW = row1["ActualWeight"].ToString(),
                                   Vaniance = row1["Vaniance"].ToString(),
                                   SubQty = row1["SubQty"].ToString(),
                                   SubGW = row1["SubGW"].ToString(),
                                   SubAW = row1["SubAW"].ToString(),
                                   SubVaniance = row1["SubVaniance"].ToString(),
                                   Remark = row1["Remark"].ToString(),
                               }).ToList();
                e.Report.ReportDataSource = data;
            }

            return Ict.Result.True;
        }

        private DualResult LoadData()
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", SqlDbType.VarChar, size: this.id.Length) { Value = this.id });

            string sql = @"
select  
	[Sel] = 0
    ,R.Roll
	,R.Dyelot
	,R.PoId
	,R.Seq1+'-'+R.Seq2 AS SEQ
	,[RefNo]=psd.RefNo
	, [ColorID]=Color.Value 
	,f.WeaveTypeID
	,o.BrandID
	,IIF((psd.ID = lag(psd.ID,1,'')over (order by psd.ID,psd.seq1,psd.seq2)  
			AND (psd.seq1 = lag(psd.seq1,1,'')over (order by psd.ID,psd.seq1,psd.seq2))  
			AND(psd.seq2 = lag(psd.seq2,1,'')over (order by psd.ID,psd.seq1,psd.seq2))) 
				,'',dbo.getMtlDesc(R.poid,R.seq1,R.seq2,2,0))[Desc]            
	,R.ShipQty
	,R.pounit
	,R.StockQty
	,R.StockUnit
	,r.ActualQty
	,[QtyVaniance]=R.ShipQty-R.ActualQty
	,R.Weight
	,R.ActualWeight
	,[Vaniance]=R.ActualWeight - R.Weight 
	,[SubQty]=sum(R.ShipQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
	,[SubGW]=sum(R.Weight) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 ) 
	,[SubAW]=sum(R.ActualWeight) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
	,[SubStockQty]=sum(R.StockQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
	,[TotalReceivingQty]=IIF((psd.ID = lag(psd.ID,1,'')over (order by psd.ID,psd.seq1,psd.seq2)  
			AND (psd.seq1 = lag(psd.seq1,1,'')over (order by psd.ID,psd.seq1,psd.seq2))  
			AND(psd.seq2 = lag(psd.seq2,1,'')over (order by psd.ID,psd.seq1,psd.seq2))) 
				,null,sum(R.StockQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 ))
	,[SubVaniance]=R.ShipQty - R.ActualQty
	,R.Remark		
    ,ColorName = c.Name
    ,[MINDQRCode] = iif(R.MINDQRCode <> '', 
                        R.MINDQRCode,
                        (select top 1 case  when    wbt.To_NewBarcodeSeq = '' then wbt.To_NewBarcode
                                            when    wbt.To_NewBarcode = ''  then ''
                                            else    Concat(wbt.To_NewBarcode, '-', wbt.To_NewBarcodeSeq)    end
                         from   WHBarcodeTransaction wbt with (nolock)
                         where  wbt.TransactionUkey = R.Ukey and
                                wbt.Action = 'Confirm'
                         order by CommitTime desc)
                    )
    ,[SuppColor] = isnull(psdsC.SpecValue, '')
    ,R.ActualQty
    ,[FabricType] = case when psd.FabricType = 'F' then 'Fabric'
                         when psd.FabricType = 'A' then 'Accessory'
                         else 'Other' end
    ,TtlQty = convert(varchar(20),
			iif(r.CombineBarcode is null , r.ActualQty, 
				iif(r.Unoriginal is  null , ttlQty.value, null))) +' '+ R.PoUnit
    ,[Location] = Location.MtlLocationID
    ,fp.Inspector
    ,[InspDate] = Format(fp.InspDate, 'yyyy/MM/dd')
    ,o.FactoryID
    ,[FirRemark] = fp.Remark
    ,[SortCmbPOID] = ISNULL(cmb.PoId,R.PoId)
	,[SortCmbSeq1] = ISNULL(cmb.Seq1,R.Seq1)
	,[SortCmbSeq2] = ISNULL(cmb.Seq2,R.Seq2)
	,[SortCmbRoll] = ISNULL(cmb.Roll,R.Roll)
	,[SortCmbDyelot] = ISNULL(cmb.Dyelot,R.Dyelot)
    ,R.Unoriginal
    ,R.Ukey
    ,StockTypeName = 
        case R.StockType
        when 'b' then 'Bulk'
        when 'i' then 'Inventory'
        when 'o' then 'Scrap'
        end
    ,o.StyleID
    ,Receiving.WhseArrival
    ,[Article] = isnull(psdsA.SpecValue, '')
    ,[Size] = isnull(psdsS.SpecValue, '')
    ,fr.Relaxtime
from dbo.Receiving_Detail R WITH (NOLOCK) 
left join Receiving WITH (NOLOCK) on Receiving.id = R.id
LEFT join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = R.POID and  psd.SEQ1 = R.Seq1 and psd.seq2 = R.Seq2 
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsA WITH (NOLOCK) on psdsA.ID = psd.id and psdsA.seq1 = psd.seq1 and psdsA.seq2 = psd.seq2 and psdsA.SpecColumnID = 'Article'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join Ftyinventory  fi with (nolock) on    R.POID = fi.POID and
                                               R.Seq1 = fi.Seq1 and
                                               R.Seq2 = fi.Seq2 and
                                               R.Roll = fi.Roll and
                                               R.Dyelot  = fi.Dyelot and
                                               R.StockType = fi.StockType
left join View_WH_Orders o WITH (NOLOCK) on o.ID = r.PoId
LEFT JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo=f.SCIRefNo
LEFT JOIN color c on c.id = isnull(psdsC.SpecValue, '') and c.BrandId = psd.BrandId 
left join FIR with (nolock) on  FIR.ReceivingID = r.ID and 
                                FIR.POID = r.POID and 
                                FIR.Seq1 = r.Seq1 and 
                                FIR.Seq2 = r.Seq2
left join FIR_Physical fp with (nolock) on  fp.ID = FIR.ID and
                                            fp.Roll = r.Roll and
                                            fp.Dyelot = r.Dyelot
left join Receiving_Detail cmb on  R.Id = cmb.Id
									and R.CombineBarcode = cmb.CombineBarcode
									and cmb.CombineBarcode is not null
									and ISNULL(cmb.Unoriginal,0) = 0
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null, dbo.GetColorMultipleID(o.BrandID,isnull(psdsC.SpecValue, '')),psd.SuppColor)
		 ELSE dbo.GetColorMultipleID(o.BrandID,isnull(psdsC.SpecValue, ''))
	 END
)Color
outer apply(
	select value = sum(t.ActualQty)
	from Receiving_Detail t WITH (NOLOCK) 
	where t.ID=r.ID
	and t.CombineBarcode=r.CombineBarcode
	and t.CombineBarcode is not null
)ttlQty
OUTER APPLY(
	    SELECT [MtlLocationID] = STUFF(
			    (
			    SELECT DISTINCT IIF(fid.MtlLocationID IS NULL OR fid.MtlLocationID = '' ,'' , ','+fid.MtlLocationID)
			    FROM FtyInventory_Detail fid
			    WHERE fid.Ukey = fi.Ukey
			    FOR XML PATH('') )
			    , 1, 1, '')
    )Location
LEFT JOIN [SciMES_RefnoRelaxtime] rr WITH (NOLOCK) ON rr.Refno = psd.Refno
LEFT JOIN [SciMES_FabricRelaxation] fr WITH (NOLOCK) ON rr.FabricRelaxationID = fr.ID
where R.id = @ID
";

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                pars.Add(new SqlParameter("@poid", this.txtSPNo.Text));
                sql += " and R.Poid = @poid";
            }

            sql += @"
order by R.EncodeSeq, SortCmbPOID, SortCmbSeq1, SortCmbSeq2, SortCmbRoll, SortCmbDyelot, R.Unoriginal, R.POID, R.Seq1, R.Seq2, R.Roll, R.Dyelot
";

            DualResult result = DBProxy.Current.Select(
                string.Empty,
                sql,
                pars,
                out this.dt);

            return result;
        }

        /// <inheritdoc/>
        public DataRow CurrentDataRow { get; set; }

        private void RadioGroup1_ValueChanged(object sender, EventArgs e)
        {
            this.ReportResourceNamespace = typeof(P07_PrintData);
            this.ReportResourceAssembly = this.ReportResourceNamespace.Assembly;
            this.ReportResourceName = this.radioPanel1.Value == this.radioPLRcvReport.Value ? "P07_Report1.rdlc" : "P07_Report2.rdlc";
            this.CheckControlEnable();
        }

        private void RadioPLRcvReport_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckControlEnable();
        }

        private void CheckControlEnable()
        {
            this.txtSPNo.Enabled = this.radioArriveWHReport.Checked;
            this.comboType.Enabled = this.radioQRCodeSticker.Checked;
            this.toexcel.Enabled = !this.radioQRCodeSticker.Checked;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.ReportResourceName == "P07_Report2.rdlc")
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P07_ArriveWearhouseReport.xltx"); // 預先開啟excel app
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                int nRow = 7;

                objSheets.Cells[1, 1] = this.rptTitle;
                objSheets.Cells[3, 1] = this.Date2;
                objSheets.Cells[4, 1] = "ETA:" + this.ETA;
                objSheets.Cells[5, 1] = "Invoice#:" + this.Invoice + "   From FTY ID:" + this.FTYID;
                objSheets.Cells[5, 14] = "WK#:" + this.Wk;
                foreach (DataRow dr in this.dt.Rows)
                {
                    objSheets.Cells[nRow, 1] = dr["Roll"].ToString();
                    objSheets.Cells[nRow, 2] = dr["Dyelot"].ToString();
                    objSheets.Cells[nRow, 3] = dr["PoId"].ToString();
                    objSheets.Cells[nRow, 4] = dr["SEQ"].ToString();
                    objSheets.Cells[nRow, 5] = dr["Refno"].ToString();
                    objSheets.Cells[nRow, 6] = dr["Article"].ToString();
                    objSheets.Cells[nRow, 7] = dr["ColorID"].ToString();
                    objSheets.Cells[nRow, 8] = dr["ColorName"].ToString();
                    objSheets.Cells[nRow, 9] = dr["Size"].ToString();
                    objSheets.Cells[nRow, 10] = dr["WeaveTypeID"].ToString();
                    objSheets.Cells[nRow, 11] = dr["BrandID"].ToString();
                    objSheets.Cells[nRow, 12] = dr["Desc"].ToString();
                    objSheets.Cells[nRow, 13] = dr["Weight"].ToString();
                    objSheets.Cells[nRow, 14] = dr["ShipQty"].ToString() + " " + dr["POUnit"].ToString();
                    objSheets.Cells[nRow, 15] = dr["ActualQty"].ToString() + " " + dr["POUnit"].ToString();
                    objSheets.Cells[nRow, 16] = dr["StockQty"].ToString() + " " + dr["StockUnit"].ToString();
                    objSheets.Cells[nRow, 17] = MyUtility.Check.Empty(dr["TotalReceivingQty"]) ?
                        string.Empty : dr["TotalReceivingQty"].ToString() + " " + dr["POUnit"].ToString();
                    objSheets.Cells[nRow, 18] = dr["QtyVaniance"].ToString();
                    objSheets.Cells[nRow, 19] = dr["Remark"].ToString();
                    nRow++;
                }

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P07_ArriveWearhouseReport");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P07_PackingListReveivingReport.xltx"); // 預先開啟excel app
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                int nRow = 7;

                objSheets.Cells[1, 1] = this.rptTitle;
                objSheets.Cells[3, 1] = this.Date1;
                objSheets.Cells[4, 1] = "ETA:" + this.ETA;
                objSheets.Cells[5, 1] = "Invoice#:" + this.Invoice + "   From FTY ID:" + this.FTYID;
                objSheets.Cells[5, 10] = "WK#:" + this.Wk;
                foreach (DataRow dr in this.dt.Rows)
                {
                    objSheets.Cells[nRow, 1] = dr["Roll"].ToString();
                    objSheets.Cells[nRow, 2] = dr["Dyelot"].ToString();
                    objSheets.Cells[nRow, 3] = dr["PoId"].ToString();
                    objSheets.Cells[nRow, 4] = dr["SEQ"].ToString();
                    objSheets.Cells[nRow, 5] = dr["RefNo"].ToString();
                    objSheets.Cells[nRow, 6] = dr["BrandID"].ToString();
                    objSheets.Cells[nRow, 7] = dr["Desc"].ToString();
                    objSheets.Cells[nRow, 8] = dr["ShipQty"].ToString() + " " + dr["pounit"].ToString();
                    objSheets.Cells[nRow, 9] = dr["ActualQty"].ToString() + " " + dr["pounit"].ToString();
                    objSheets.Cells[nRow, 10] = dr["StockQty"].ToString() + " " + dr["StockUnit"].ToString();
                    objSheets.Cells[nRow, 11] = dr["Weight"].ToString();
                    objSheets.Cells[nRow, 12] = dr["ActualWeight"].ToString();
                    objSheets.Cells[nRow, 13] = dr["QtyVaniance"].ToString();
                    objSheets.Cells[nRow, 14] = dr["Remark"].ToString();
                    nRow++;
                }

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P07_PackingListReveivingReport");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            if (!this.radioQRCodeSticker.Checked)
            {
                return base.ToPrint();
            }

            if (this.radioQRCodeSticker.Checked && this.curMain["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, cannot print.");
                return false;
            }

            this.ValidateInput();
            this.ShowWaitMessage("Loading...");
            DualResult result = this.LoadData();
            this.HideWaitMessage();
            if (!result)
            {
                this.ShowErr(result);
                return true;
            }

            var barcodeDatas = this.dt.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["MINDQRCode"]));

            if (barcodeDatas.Count() == 0)
            {
                MyUtility.Msg.InfoBox("No Data can print");
                return true;
            }

            new WH_Receive_QRCodeSticker(barcodeDatas.CopyToDataTable(), this.comboType.Text, "P07").ShowDialog();

            return true;
        }
    }
}
