using Ict;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.PublicForm;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P18_Print : Win.Tems.PrintForm
    {
        private DataRow mainCurrentMaintain;
        private DataTable dtResult;

        /// <inheritdoc/>
        public P18_Print(DataRow drMain)
        {
            /// <inheritdoc/>
            this.InitializeComponent();
            this.mainCurrentMaintain = drMain;
            this.radioPanel.Value = "1";
            MyUtility.Tool.SetupCombox(this.comboPrint, 1, 1, "Sticker,Paper");
            this.comboPrint.Text = "Sticker";
        }

        private void RadioPanel_ValueChanged(object sender, EventArgs e)
        {
            this.ReportResourceNamespace = typeof(P18_PrintData);
            this.ReportResourceAssembly = this.ReportResourceNamespace.Assembly;
            this.ReportResourceName = "P18_Print.rdlc";

            this.IsSupportToPrint = !this.radioP18ExcelImport.Checked;
            this.IsSupportToExcel = this.radioP18ExcelImport.Checked;
            this.comboPrint.Enabled = this.radioQRCodeSticker.Checked;
            this.comboType.Enabled = this.radioQRCodeSticker.Checked;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            // saveDialog.ShowDialog();
            // outpa = saveDialog.FileName;
            // if (outpa.Empty())
            // {

            // return false;
            // }
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DataRow row = this.mainCurrentMaintain;
            string id = row["ID"].ToString();
            string fromFactory = row["FromFtyID"].ToString();
            string remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToShortDateString();

            if (this.radioPanel.Value == "1")
            {
                #region -- 撈表頭資料 --
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", id));
                DataTable dt;
                string cmdd = @"
select  b.name 
from dbo.Transferin  a WITH (NOLOCK) 
inner join dbo.mdivision  b WITH (NOLOCK) on b.id = a.mdivisionid
where   b.id = a.mdivisionid
        and a.id = @ID";
                DualResult result = DBProxy.Current.Select(string.Empty, cmdd, pars, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found!!!");
                }

                // 抓M的EN NAME
                DBProxy.Current.Select(string.Empty, $@"select NameEN from MDivision where ID='{Env.User.Keyword}'", out DataTable dtNAME);
                string rptTitle = dtNAME.Rows[0]["NameEN"].ToString();
                #endregion

                #region -- 撈表身資料 --
                DataTable dtDetail;
                string tmp = @"
select  a.POID
        , a.Seq1 + '-' + a.seq2 as SEQ
        , a.Roll
        , a.Dyelot 
	    , [Description] = IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
			                   AND (b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
			                   AND (b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
			                  , ''
                              , dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))
        , b.StockUnit
	    , a.Qty
        , a.Weight
        , dbo.Getlocation(f.ukey)[Location] 
        , a.ContainerCode
from dbo.TransferIn_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.POID 
                                                and b.SEQ1 = a.Seq1 
                                                and b.SEQ2=a.seq2
inner join FtyInventory f WITH (NOLOCK) on  f.POID = a.poid
		                                    And f.Seq1 = a.seq1
		                                    And f.Seq2 = a.seq2
		                                    And f.Roll =  a.roll
		                                    And f.Dyelot = a.dyelot
		                                    And f.StockType = a.stocktype
where a.id = @ID";
                result = DBProxy.Current.Select(string.Empty, tmp, pars, out dtDetail);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dtDetail == null || dtDetail.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found!!!");
                }

                this.dtResult = dtDetail;

                #endregion

                #region -- 整理表頭資料 --
                e.Report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
                e.Report.ReportParameters.Add(new ReportParameter("ID", id));
                e.Report.ReportParameters.Add(new ReportParameter("FromFtyID", fromFactory));
                e.Report.ReportParameters.Add(new ReportParameter("Remark", remark));
                e.Report.ReportParameters.Add(new ReportParameter("IssueDate", issuedate));
                #endregion

                #region -- 整理表身資料 --

                // 傳 list 資料
                List<P18_PrintData> data = this.dtResult.AsEnumerable()
                    .Select(row1 => new P18_PrintData()
                    {
                        POID = row1["POID"].ToString().Trim(),
                        SEQ = row1["SEQ"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        DYELOT = row1["DYELOT"].ToString().Trim(),
                        DESC = row1["Description"].ToString().Trim(),
                        Unit = row1["StockUnit"].ToString().Trim(),
                        QTY = row1["QTY"].ToString().Trim(),
                        GW = row1["Weight"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                    }).ToList();

                e.Report.ReportDataSource = data;
                #endregion
            }
            else
            {
                string cmd = $@"
select  a.Roll
		, a.Dyelot
		, a.POID
        , a.Seq1 + '-' + a.seq2 as SEQ
		, psd.Refno
		, Color = dbo.GetColorMultipleID(psd.BrandID, isnull(psdsC.SpecValue, ''))
		, ColorName = c.Name
		, f.WeaveTypeID
		, o.BrandID
	    , [Description] = dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0)
		, a.Weight
		, a.ActualWeight
		, a.Qty
		, StockType =  CASE WHEN a.StockType = 'B' THEN 'Bulk'
                            WHEN a.StockType = 'I' THEN 'Inventory'
                            ELSE a.StockType 
                        END
		, a.Location
        , a.ContainerCode
		, a.Remark
from dbo.TransferIn_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.id = a.POID 
                                                and psd.SEQ1 = a.Seq1 
                                                and psd.SEQ2=a.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Color c WITH (NOLOCK) on c.ID = isnull(psdsC.SpecValue, '') AND c.BrandId = psd.BrandId
LEFT JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo=f.SCIRefNo
left join View_WH_Orders o WITH (NOLOCK) on o.ID = a.POID
WHERE a.ID = '{id}'
";

                DualResult result = DBProxy.Current.Select(null, cmd, out this.dtResult);

                if (!result)
                {
                    this.ShowErr(result);
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            if (!this.radioQRCodeSticker.Checked)
            {
                return base.ToPrint();
            }

            if (this.radioQRCodeSticker.Checked && this.mainCurrentMaintain["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, cannot print.");
                return false;
            }

            DataTable dataTable_QRCode = new DataTable();
            this.ValidateInput();
            this.ShowWaitMessage("Loading...");
            DualResult result = this.LoadData_QRCode(out dataTable_QRCode);
            this.HideWaitMessage();
            if (!result)
            {
                this.ShowErr(result);
                return true;
            }

            var barcodeDatas = dataTable_QRCode.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["MINDQRCode"]));

            if (barcodeDatas.Count() == 0)
            {
                MyUtility.Msg.InfoBox("No Data can print");
                return true;
            }

            new WH_Receive_QRCodeSticker(barcodeDatas.CopyToDataTable(), this.comboPrint.Text, this.comboType.Text, "P18").ShowDialog();

            return true;
        }

        private DualResult LoadData_QRCode(out DataTable dt)
        {
            List<SqlParameter> pars = new List<SqlParameter>()
            {
                new SqlParameter("@ID" , this.mainCurrentMaintain["ID"].ToString()),
            };

            string sqlCmd = @"
select [Sel] = 0
	, td.POID
	, [SEQ] = concat(td.Seq1,'-', td.Seq2)
	, [FabricType] = Case psd.FabricType
					When 'F' then 'Fabric'
					When 'A' then 'Accessory'
					When 'O' then 'Other'
					Else psd.FabricType
				end
	, td.Weight
	, td.ActualWeight
	, td.Roll
	, td.Dyelot
	, [StockQty] = td.Qty
	, psd.StockUnit
	, [MINDQRCode] = iif(td.MINDQRCode <> '', 
               td.MINDQRCode,
               (select top 1 case  when    wbt.To_NewBarcodeSeq = '' then wbt.To_NewBarcode
                                   when    wbt.To_NewBarcode = ''  then ''
                                   else    Concat(wbt.To_NewBarcode, '-', wbt.To_NewBarcodeSeq)    end
                from   WHBarcodeTransaction wbt with (nolock)
                where  wbt.TransactionUkey = td.Ukey and
                       wbt.Action = 'Confirm'
                order by CommitTime desc)
           )
	, [Location] = Location.MtlLocationID
	, [RefNo] = psd.RefNo
	, [ColorID] = Color.Value 
	, [FactoryID] = o.FactoryID
    , [SortCmbPOID] = ISNULL(cmb.PoId, td.PoId)
	, [SortCmbSeq1] = ISNULL(cmb.Seq1, td.Seq1)
	, [SortCmbSeq2] = ISNULL(cmb.Seq2, td.Seq2)
	, [SortCmbRoll] = ISNULL(cmb.Roll, td.Roll)
	, [SortCmbDyelot] = ISNULL(cmb.Dyelot, td.Dyelot)
    , td.Unoriginal
    , td.Ukey
    ,StockTypeName = 
        case td.StockType
        when 'b' then 'Bulk'
        when 'i' then 'Inventory'
        when 'o' then 'Scrap'
        end
    ,o.StyleID
    ,WhseArrival = TransferIn.IssueDate
    ,fr.Relaxtime
from TransferIn_Detail td WITH (NOLOCK) 
left join TransferIn WITH (NOLOCK) on TransferIn.id = td.id
left join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = td.POID and  psd.SEQ1 = td.Seq1 and psd.seq2 = td.Seq2 
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Ftyinventory  fi with (nolock) on td.POID = fi.POID and
                                            td.Seq1 = fi.Seq1 and
                                            td.Seq2 = fi.Seq2 and
                                            td.Roll = fi.Roll and
                                            td.Dyelot  = fi.Dyelot and
                                            td.StockType = fi.StockType
left join View_WH_Orders o WITH (NOLOCK) on o.ID = td.PoId
LEFT JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo=f.SCIRefNo
left join Receiving_Detail cmb on  td.Id = cmb.Id
									and td.CombineBarcode = cmb.CombineBarcode
									and cmb.CombineBarcode is not null
									and ISNULL(cmb.Unoriginal,0) = 0
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null, dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')),psd.SuppColor)
		 ELSE dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, ''))
	 END
)Color
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
where td.id = @ID";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlCmd, pars, out dt);
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtResult.Rows.Count); // 顯示筆數

            if (this.dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_P18_Print.xltx", objApp);
            com.UseInnerFormating = false;

            // excel不須顯示ContainerCode
            DataTable dtExcel = this.dtResult.Copy();
            dtExcel.Columns.Remove("ContainerCode");
            com.WriteTable(dtExcel, 2);

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }

        private void ComboPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboPrint.SelectedIndex != -1)
            {
                switch (this.comboPrint.SelectedValue.ToString())
                {
                    case "Paper":
                        this.BindComboTypePaper();
                        break;
                    case "Sticker":
                    default:
                        this.BindComboTypeSticker();
                        break;
                }
            }
            else
            {
                this.BindComboTypeSticker();
            }
        }

        private void BindComboTypeSticker()
        {
            this.comboType.DataSource = null;
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

        private void BindComboTypePaper()
        {
            this.comboType.DataSource = null;
            MyUtility.Tool.SetupCombox(this.comboType, 1, 1, "Horizontal,Straight");
            this.comboType.Text = "Straight";
        }
    }
}
