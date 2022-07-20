using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// R40
    /// </summary>
    public partial class R40 : Win.Tems.PrintForm
    {
        private string strSp1;
        private string strSp2;
        private DateTime? arriveDateStart;
        private DateTime? arriveDateEnd;
        private string wkStart;
        private string wkEnd;
        private string updateInfo;
        private string status;
        private DataTable[] listResult;
        private DataTable mindDt;

        /// <summary>
        /// R40
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            Dictionary<string, string> updateInfo_source = new Dictionary<string, string>
            {
                { "ALL", "*" },
                { "Receiving Act. (kg)", "0" },
                { "Cut Shadeband", "1" },
                { "Fabric to Lab", "2" },
                { "Checker", "3" },
                { "Scanned by MIND", "4" },
            };
            this.comboUpdateInfo.DataSource = new BindingSource(updateInfo_source, null);
            this.comboUpdateInfo.DisplayMember = "Key";
            this.comboUpdateInfo.ValueMember = "Value";

            this.comboUpdateInfo.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateRangeArriveDate.HasValue &&
                MyUtility.Check.Empty(this.txtSPNoStart.Text) &&
                MyUtility.Check.Empty(this.txtSPNoEnd.Text) &&
                MyUtility.Check.Empty(this.txtWKStart.Text) &&
                MyUtility.Check.Empty(this.txtWKEnd.Text))
            {
                MyUtility.Msg.WarningBox("<Arrive W/H Date>, <SP#>, <WK#> can not be empty");
                return false;
            }

            this.arriveDateStart = this.dateRangeArriveDate.DateBox1.Value;
            this.arriveDateEnd = this.dateRangeArriveDate.DateBox2.Value;
            this.strSp1 = this.txtSPNoStart.Text.Trim();
            this.strSp2 = this.txtSPNoEnd.Text.Trim();
            this.wkStart = this.txtWKStart.Text.Trim();
            this.wkEnd = this.txtWKEnd.Text.Trim();
            this.updateInfo = this.comboUpdateInfo.SelectedValue.ToString().Trim();
            this.status = this.comboStatus.SelectedValue.ToString().Trim();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Set SQL Command & SQLParameter
            string whereReceiving = string.Empty;
            string whereTransferIn = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.strSp1))
            {
                whereReceiving += " and rd.POID >= @Sp1 ";
                whereTransferIn += " and rd.POID >= @Sp1 ";
                listPar.Add(new SqlParameter("@Sp1", this.strSp1));
            }

            if (!MyUtility.Check.Empty(this.strSp2))
            {
                whereReceiving += " and rd.POID <= @Sp2 ";
                whereTransferIn += " and rd.POID <= @Sp2 ";
                listPar.Add(new SqlParameter("@Sp2", this.strSp2));
            }

            if (this.arriveDateStart.HasValue)
            {
                whereReceiving += " and r.WhseArrival >= @arriveDateStart ";
                whereTransferIn += " and r.IssueDate >= @arriveDateStart ";
                listPar.Add(new SqlParameter("@arriveDateStart", this.arriveDateStart));
            }

            if (this.arriveDateEnd.HasValue)
            {
                whereReceiving += " and r.WhseArrival <= @arriveDateEnd ";
                whereTransferIn += " and r.IssueDate <= @arriveDateEnd ";
                listPar.Add(new SqlParameter("@arriveDateEnd", this.arriveDateEnd));
            }

            if (!MyUtility.Check.Empty(this.wkStart))
            {
                whereReceiving += " and r.ExportID >= @wkStart ";
                whereTransferIn += " and 1 = 0 ";
                listPar.Add(new SqlParameter("@wkStart", this.wkStart));
            }

            if (!MyUtility.Check.Empty(this.wkEnd))
            {
                whereReceiving += " and r.ExportID <= @wkEnd ";
                whereTransferIn += " and 1 = 0 ";
                listPar.Add(new SqlParameter("@wkEnd", this.wkEnd));
            }

            string whereReceivingAct = string.Empty;
            string whereCutShadeband = string.Empty;
            string whereFabricLab = string.Empty;
            string whereChecker = string.Empty;
            string whereMind = string.Empty;

            if (this.status == "AlreadyUpdated")
            {
                whereReceivingAct += " and ActualWeight != 0 ";
                whereCutShadeband += " and CutShadebandTime is not null";
                whereFabricLab += " and Fabric2LabTime is not null";
                whereChecker += " and ISNULL(Checker,'') <> ''";
            }

            if (this.status == "NotYetUpdate")
            {
                whereReceivingAct += " and ActualWeight = 0 ";
                whereCutShadeband += " and CutShadebandTime is null";
                whereFabricLab += " and Fabric2LabTime is null";
                whereChecker += " and ISNULL(Checker,'') = ''";
            }

            if (this.status.EqualString("AlreadyScanned"))
            {
                whereMind += " and MINDCheckAddDate is not null";
            }

            if (this.status.EqualString("NotYetScanned"))
            {
                whereMind += " and MINDCheckAddDate is null";
            }

            string strSql = $@"
select * into #tmpResult
 from (
		select
			[ReceivingID] = r.ID
		    ,r.ExportID
			,[ArriveDate] = r.WhseArrival
		    ,rd.PoId
			,[Seq] = rd.Seq1 + ' ' + rd.Seq2
			,o.BrandID
			,psd.refno
			,fb.WeaveTypeID
			,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' 
						, IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, psd.ColorID) , psd.SuppColor)
						, psd.ColorID)
			,rd.Roll
		    ,rd.Dyelot
			,rd.StockQty
			,StockType = isnull (ddl.Name, rd.StockType)
			,rd.Location
			,rd.Weight
			,rd.ActualWeight
			,[CutShadebandTime]=cutTime.CutTime
		    ,cutTime.CutBy
			,rd.Fabric2LabBy
			,rd.Fabric2LabTime
            ,rd.Checker
		from  Receiving r with (nolock)
		inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
		inner join Orders o with (nolock) on o.ID = rd.POID 
		inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
        inner join PO_Supp ps with (nolock) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
		inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
        left join DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Pms_StockType'
                                                    and REPLACE(ddl.ID,'''','') = rd.StockType
		OUTER APPLY(
		    SELECT  fs.CutTime,fs.CutBy
		    FROM FIR f
		    INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
		    WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
		) cutTime
        where   psd.FabricType ='F' {whereReceiving}
		union all
		SELECT 
			[ReceivingID] = r.ID
		    ,[ExportID] = ''
			,[ArriveDate] = r.IssueDate
		    ,rd.PoId
			,[Seq] = rd.Seq1 + ' ' + rd.Seq2
			,o.BrandID
			,psd.refno
			,fb.WeaveTypeID
			,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' 
						, IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, psd.ColorID) , psd.SuppColor)
						, psd.ColorID)
			,rd.Roll
		    ,rd.Dyelot
			,[StockQty] = rd.Qty
			,StockType = isnull (ddl.Name, rd.StockType)
			,rd.Location
			,rd.Weight
			,rd.ActualWeight
			,[CutShadebandTime]=cutTime.CutTime
		    ,cutTime.CutBy
			,rd.Fabric2LabBy
			,rd.Fabric2LabTime
            ,rd.Checker
		FROM TransferIn r with (nolock)
		INNER JOIN TransferIn_Detail rd with (nolock) ON r.ID = rd.ID
		INNER JOIN Orders o with (nolock) ON o.ID = rd.POID
		INNER JOIN PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
        inner join PO_Supp ps with (nolock) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
		INNER JOIN Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
        left join DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Pms_StockType'
                                                    and REPLACE(ddl.ID,'''','') = rd.StockType
		OUTER APPLY(
		    SELECT  fs.CutTime,fs.CutBy
		    FROM FIR f
		    INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
		    WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
		)cutTime
        where   psd.FabricType ='F' {whereTransferIn}
	) a

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,BrandID
		,refno
		,WeaveTypeID
		,Color
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,ActualWeight
from #tmpResult
where 1 = 1 {whereReceivingAct}

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,BrandID
		,refno
		,WeaveTypeID
		,Color
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,CutShadebandTime
		,CutBy
from #tmpResult
where 1 = 1 {whereCutShadeband}

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,BrandID
		,refno
		,WeaveTypeID
		,Color
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,Fabric2LabTime
		,Fabric2LabBy
from #tmpResult
where 1 = 1 {whereFabricLab}

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,BrandID
		,refno
		,WeaveTypeID
		,Color
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,Checker
from #tmpResult
where 1 = 1 {whereChecker}

drop table #tmpResult
";
            DualResult result;
            if (this.updateInfo == "*" || this.updateInfo == "4")
            {
                string sqlmind = $@"
select
	[ReceivingID] = r.ID
	,r.ExportID
	,[ArriveDate] = r.WhseArrival
	,rd.PoId
    ,rd.seq1
    ,rd.seq2
	,[Seq] = rd.Seq1 + ' ' + rd.Seq2
	,o.BrandID
	,psd.refno
	,fb.WeaveTypeID
	,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' 
				, IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, psd.ColorID) , psd.SuppColor)
				, psd.ColorID)
	,rd.Roll
	,rd.Dyelot
	,rd.StockQty
	,StockType = isnull (ddl.Name, rd.StockType)
	,rd.Location
	,rd.Weight
	,rd.ActualWeight
	,[CutShadebandTime]=cutTime.CutTime
	,cutTime.CutBy
	,rd.Fabric2LabBy
	,rd.Fabric2LabTime
    ,rd.Checker
    ,IsQRCodeCreatedByPMS = iif (dbo.IsQRCodeCreatedByPMS(rd.MINDQRCode) = 1, 'Create from PMS', '')
    ,rd.MINDChecker
    ,rd.QRCode_PrintDate
    ,rd.MINDCheckAddDate
    ,rd.MINDCheckEditDate
    ,AbbEN = (select Supp.AbbEN from Supp with (nolock) where Supp.id =ps.SuppID)
    ,rdStockType = rd.StockType
into #tmpMind
from  Receiving r with (nolock)
inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
inner join Orders o with (nolock) on o.ID = rd.POID 
inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
inner join PO_Supp ps with (nolock) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
left join DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Pms_StockType'
                                            and REPLACE(ddl.ID,'''','') = rd.StockType
OUTER APPLY(
	SELECT  fs.CutTime,fs.CutBy
	FROM FIR f
	INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
	WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
) cutTime
where   psd.FabricType ='F' and r.Type = 'A'
{whereReceiving}

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,BrandID
		,refno
		,WeaveTypeID
		,Color
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
        ,ActualWeight
        ,IsQRCodeCreatedByPMS
        ,LastP26RemarkData
        ,MINDChecker
        ,QRCode_PrintDate
        ,MINDCheckAddDate
        ,MINDCheckEditDate
        ,AbbEN
from #tmpMind rd
OUTER APPLY(
    select top 1 LastP26RemarkData =  lt.Remark
	FROM LocationTrans lt
	INNER JOIN LocationTrans_detail ltd ON lt.ID=ltd.ID
    where lt.Status='Confirmed'
    and ltd.poid = rd.poid and ltd.seq1 = rd.seq1 and ltd.seq2 = rd.seq2  AND ltd.Roll = rd.Roll and ltd.Dyelot = rd.Dyelot and ltd.StockType = rd.rdStockType
    order by EditDate desc
)p26
where 1 = 1 {whereMind}
drop table #tmpMind
";
                result = DBProxy.Current.Select(null, sqlmind, listPar, out this.mindDt);
                if (!result)
                {
                    return result;
                }
            }
            #endregion

            if (this.updateInfo != "4")
            {
                return DBProxy.Current.Select(null, strSql, listPar, out this.listResult);
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            int dataCnt = 0;

            if (this.updateInfo == "*")
            {
                dataCnt = this.listResult.Sum(s => s.Rows.Count) + this.mindDt.Rows.Count;
            }
            else if (this.updateInfo != "4")
            {
                int dataNum = MyUtility.Convert.GetInt(this.updateInfo);
                dataCnt = this.listResult[dataNum].Rows.Count;
            }
            else
            {
                dataCnt = this.mindDt.Rows.Count;
            }

            this.SetCount(dataCnt);
            if (dataCnt == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            if (this.updateInfo != "4")
            {
                int serReport = 0;
                foreach (DataTable dataTable in this.listResult)
                {
                    string excelName = string.Empty;

                    if (this.updateInfo != "*" && this.updateInfo != serReport.ToString())
                    {
                        serReport++;
                        continue;
                    }

                    switch (serReport)
                    {
                        case 0:
                            excelName = "Warehouse_R40_ReceivingActkg";
                            break;
                        case 1:
                            excelName = "Warehouse_R40_CutShadeband";
                            break;
                        case 2:
                            excelName = "Warehouse_R40_FabrictoLab";
                            break;
                        case 3:
                            excelName = "Warehouse_R40_Checker";
                            break;
                        default:
                            continue;
                    }

                    Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app
                    if (dataTable.Rows.Count > 0)
                    {
                        MyUtility.Excel.CopyToXls(dataTable, null, $"{excelName}.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                    }

                    Excel.Worksheet worksheet = objApp.Sheets[1];
                    worksheet.Rows.AutoFit();
                    worksheet.Columns.AutoFit();

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName(excelName);
                    objApp.ActiveWorkbook.SaveAs(strExcelName);
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(worksheet);

                    strExcelName.OpenFile();
                    serReport++;
                    #endregion
                }
            }

            if (this.updateInfo == "*" || this.updateInfo == "4")
            {
                string excelName = "Warehouse_R40_ScannedbyMIND";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app
                if (this.mindDt.Rows.Count > 0)
                {
                    MyUtility.Excel.CopyToXls(this.mindDt, null, $"{excelName}.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                }

                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName(excelName);
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }

            this.HideWaitMessage();
            return true;
        }

        private void ComboUpdateInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Status下拉選單
            Dictionary<string, string> status_source = new Dictionary<string, string>
            {
                { "ALL", "All" },
                { "Already updated", "AlreadyUpdated" },
                { "Not yet Update", "NotYetUpdate" },
            };

            // Status下拉選單
            Dictionary<string, string> status_source_MIND = new Dictionary<string, string>
            {
                { "ALL", "All" },
                { "Already Scanned", "AlreadyScanned" },
                { "Not yet Scanned", "NotYetScanned" },
            };

            this.comboStatus.DataSource = null;
            if (this.comboUpdateInfo.SelectedValue.ToString() == "4")
            {
                this.comboStatus.DataSource = new BindingSource(status_source_MIND, null);
            }
            else
            {
                this.comboStatus.DataSource = new BindingSource(status_source, null);
            }

            this.comboStatus.DisplayMember = "Key";
            this.comboStatus.ValueMember = "Value";
            this.comboStatus.SelectedIndex = 0;
        }
    }
}
