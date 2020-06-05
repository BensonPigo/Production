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
using Excel = Microsoft.Office.Interop.Excel;
namespace Sci.Production.Warehouse
{
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        string StartSPNo, EndSPNo, MDivision, Factory, StartRefno, EndRefno, Color, MT, ST ,WorkNo;
        private bool bulk;
        private bool sample;
        private bool material;
        private bool smtl;
        private bool complete;
        DateTime? BuyerDelivery1, BuyerDelivery2;
        DateTime? ETA1, ETA2;
        DateTime? arriveWH1, arriveWH2;
        string sqlcolumn = @"select
	[M] = o.MDivisionID
	,[Factory] = o.FactoryID
	,[SP#] = psd.id
    ,[Category] = case when o.Category='B'then'Bulk'
						when o.Category='G'then'Garment'
						when o.Category='M'then'Material'
						when o.Category='S'then'Sample'
						when o.Category='T'then'Sample mtl.'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
					end
	,[OrderType] = o.OrderTypeID
	,[WeaveType] = d.WeaveTypeID
    ,[BuyerDelivery]=o.BuyerDelivery
    ,[MaterialComplete] = case when psd.Complete = 1 then 'Y' else '' end
    ,[ETA] = psd.FinalETA
    ,[ArriveWHDate] = stuff((
                    select distinct concat(char(10),isnull(Format(a.date,'yyyy/MM/dd'),'　'))
                    from (
	                    select date = Export.whsearrival
	                    from Export_Detail with (nolock) 
	                    inner join Export with (nolock) on Export.ID = Export_Detail.ID
	                    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2

	                    union all

	                    select date = ts.IssueDate
	                    from TransferIn ts with (nolock) 
	                    inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	                    and ts.Status='Confirmed'
                    )a
                    for xml path('')
	            ),1,1,'')
    ,[WK] = stuff((
	            	select concat(char(10),ID)
	            	from Export_Detail with (nolock) 
	            	where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2 order by Export_Detail.ID
	            	for xml path('')
	            ),1,1,'')
	,[Brand] = o.BrandID
	,[Style] = o.StyleID
	,[Season] = o.SeasonID
	,[Project] = o.ProjectID
	,[Program] = o.ProgramID
	,[Seq1] = psd.SEQ1
	,[Seq2] = psd.SEQ2
	,[Material Type] = psd.FabricType
    ,[stock sp]=psd.StockPOID
    ,[StockSeq1]=psd.StockSeq1
    ,[StockSeq2]=psd.StockSeq2
	,[Refno] = psd.Refno
	,[SCI Refno] = psd.SCIRefno
	,[Description] = d.Description
	,[Color] = CASE WHEN Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' THEN psd.SuppColor 
					ELSE psd.ColorID  
			   END
	,[Size] = psd.SizeSpec
	,[Stock Unit] = psd.StockUnit
	,[Purchase Qty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.Qty)
    ,[Order Qty] = o.Qty
	,[Ship Qty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.ShipQty)
	,[Roll] = fi.Roll
	,[Dyelot] = fi.Dyelot
	,[Stock Type] = case when fi.StockType = 'B' then 'Bulk'
						 when fi.StockType = 'I' then 'Inventory'
						 when fi.StockType = 'O' then 'Scrap'
						 end
	,[In Qty] = round(fi.InQty,2)
	,[Out Qty] = round(fi.OutQty,2)
	,[Adjust Qty] = round(fi.AdjustQty,2)
	,[Balance Qty] = round(fi.InQty,2) - round(fi.OutQty,2) + round(fi.AdjustQty,2)
	,[Location] = f.MtlLocationID
    ,[MCHandle] = isnull(dbo.getPassEmail(o.MCHandle) ,'')
	,[POHandle] = isnull(dbo.getPassEmail(p.POHandle) ,'')
	,[POSMR] = isnull(dbo.getPassEmail(p.POSMR) ,'')     
    ";

        string sqlcolumn_sum = @"select
	[M] = o.MDivisionID
	,[Factory] = o.FactoryID
	,[SP#] = psd.id
	,[OrderType] = o.OrderTypeID
	,[WeaveType] = d.WeaveTypeID
    ,[BuyerDelivery]=o.BuyerDelivery
    ,[ETA] = psd.FinalETA
    ,[ArriveWHDate] = stuff((
                    select distinct concat(char(10),isnull(Format(a.date,'yyyy/MM/dd'),'　'))
                    from (
	                    select date = Export.whsearrival
	                    from Export_Detail with (nolock) 
	                    inner join Export with (nolock) on Export.ID = Export_Detail.ID
	                    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2

	                    union all

	                    select date = ts.IssueDate
	                    from TransferIn ts with (nolock) 
	                    inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	                    and ts.Status='Confirmed'
                    )a
                    for xml path('')
	            ),1,1,'')
    ,[WK] = stuff((
	            	select concat(char(10),ID)
	            	from Export_Detail with (nolock) 
	            	where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2 order by Export_Detail.ID
	            	for xml path('')
	            ),1,1,'')
	,[Brand] = o.BrandID
	,[Style] = o.StyleID
	,[Season] = o.SeasonID
	,[Project] = o.ProjectID
	,[Program] = o.ProgramID
	,[Seq1] = psd.SEQ1
	,[Seq2] = psd.SEQ2
	,[Material Type] = psd.FabricType
    ,[stock sp]=psd.StockPOID
    ,[StockSeq1]=psd.StockSeq1
    ,[StockSeq2]=psd.StockSeq2
	,[Refno] = psd.Refno
	,[SCI Refno] = psd.SCIRefno
    ,[Description] = d.Description
	,[Color] = CASE WHEN Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' THEN psd.SuppColor 
					ELSE psd.ColorID  
			   END
	,[Size] = psd.SizeSpec
	,[Stock Unit] = psd.StockUnit
	,[Purchase Qty] = round(ISNULL(r.RateValue,1) * psd.Qty,2)
    ,[Order Qty] = o.Qty
	,[Ship Qty] = round(ISNULL(r.RateValue,1) * psd.ShipQty,2)
	,[In Qty] = round(mpd.InQty,2)
	,[Out Qty] = round(mpd.OutQty,2)
	,[Adjust Qty] = round(mpd.AdjustQty,2)
	,[Balance Qty] = round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)
	,[Bulk Qty] = (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.LInvQty,2)
	,[Inventory Qty] = round(mpd.LInvQty,2)
	,[Scrap Qty] = round(mpd.LObQty ,2)
	,[Bulk Location] = mpd.ALocation
	,[Inventory Location] = mpd.BLocation
    ,[MCHandle] = isnull(dbo.getPassEmail(o.MCHandle) ,'')
	,[POHandle] = isnull(dbo.getPassEmail(p.POHandle) ,'')
	,[POSMR] = isnull(dbo.getPassEmail(p.POSMR) ,'') 
    ";

        string sql_yyyy = @"select distinct left(CONVERT(CHAR(8),o.SciDelivery, 112),4) as SciYYYY";

        string sql_cnt = @"select count(*) as datacnt";

        int ReportType;
        bool boolCheckQty;
        int data_cnt = 0;
        StringBuilder sqlcmd = new StringBuilder();
        StringBuilder sqlcmd_fin = new StringBuilder();
        DataTable printData;
        DataTable printData_cnt;
        DataTable printData_yyyy;

        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("All", "All");
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessory");
            cmbMaterialType.DataSource = new BindingSource(comboBox1_RowSource, null);
            cmbMaterialType.ValueMember = "Key";
            cmbMaterialType.DisplayMember = "Value";
            cmbMaterialType.SelectedIndex = 0;

            Dictionary<String, String> comboBox2_RowSource = new Dictionary<string, string>();
            comboBox2_RowSource.Add("All", "All");
            comboBox2_RowSource.Add("B", "Bulk");
            comboBox2_RowSource.Add("I", "Inventory");
            comboBox2_RowSource.Add("O", "Scrap");
            cmbStockType.DataSource = new BindingSource(comboBox2_RowSource, null);
            cmbStockType.ValueMember = "Key";
            cmbStockType.DisplayMember = "Value";
            cmbStockType.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            StartSPNo = textStartSP.Text ;
            EndSPNo = textEndSP.Text;
            MDivision = txtMdivision1.Text;
            Factory = txtfactory1.Text;
            StartRefno = textStartRefno.Text;
            EndRefno = textEndRefno.Text;
            Color = textColor.Text;
            MT = cmbMaterialType.SelectedValue.ToString();
            ST = cmbStockType.SelectedValue.ToString();
            ReportType = rdbtnDetail.Checked ? 0 : 1;
            boolCheckQty = checkQty.Checked;
            BuyerDelivery1 = dateBuyerDelivery.Value1;
            BuyerDelivery2 = dateBuyerDelivery.Value2;
            ETA1 = dateETA.Value1;
            ETA2 = dateETA.Value2;
            arriveWH1 = dateArriveDate.Value1;
            arriveWH2 = dateArriveDate.Value2;
            WorkNo = txtWorkNo.Text;

            bulk = checkBulk.Checked;
            sample = checkSample.Checked;
            material = checkMaterial.Checked;
            smtl = checkSMTL.Checked;
            complete = chkComplete.Checked;
            if (MyUtility.Check.Empty(StartSPNo) &&
                MyUtility.Check.Empty(EndSPNo) &&
                !dateETA.HasValue &&
                !dateArriveDate.HasValue &&
                !dateBuyerDelivery.HasValue &&
                MyUtility.Check.Empty(StartRefno) &&
                MyUtility.Check.Empty(EndRefno))
            {
                MyUtility.Msg.WarningBox("<SP#>,<ETA>,<Arrive W/H Date>,<Buyer Delivery>,<Refno> at least one entry is required");
                return false;
            }
            return true;
        }
        
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            sqlcmd.Clear();
            sqlcmd_fin.Clear();

            if (ReportType == 0)
            {
                #region 主要sql Detail
                sqlcmd.Append($@" 
from Orders o with (nolock)
inner join PO p with (nolock) on o.id = p.id
inner join PO_Supp_Detail psd with (nolock) on p.id = psd.id
{(!string.IsNullOrEmpty(this.WorkNo) ? $"INNER JOIN Export_Detail ed ON ed.POID=psd.ID AND ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2 AND ed.ID='{this.WorkNo}'" : "")}
left join FtyInventory fi with (nolock) on fi.POID = psd.id and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
left join Fabric WITH (NOLOCK) on psd.SCIRefno = fabric.SCIRefno
outer apply
(
	select MtlLocationID = stuff(
	(
		select concat(',',MtlLocationID)
		from FtyInventory_Detail fid with (nolock) 
		where fid.Ukey = fi.Ukey
		for xml path('')
	),1,1,'')
)f
outer apply
(
	select Description ,WeaveTypeID
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
where 1=1
");
                #endregion
            }
            else
            {
                #region 主要sql summary
                sqlcmd.Append($@"
from Orders o with (nolock)
inner join PO p with (nolock) on o.id = p.id
inner join PO_Supp_Detail psd with (nolock) on p.id = psd.id
{(!string.IsNullOrEmpty(this.WorkNo) ? $"INNER JOIN Export_Detail ed ON ed.POID=psd.ID AND ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2 AND ed.ID='{this.WorkNo}'" : "")}
left join MDivisionPoDetail mpd with (nolock) on mpd.POID = psd.id and mpd.Seq1 = psd.SEQ1 and mpd.seq2 = psd.SEQ2
left join Fabric WITH (NOLOCK) on psd.SCIRefno = fabric.SCIRefno
outer apply
(
	select Description ,WeaveTypeID
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
outer apply
(
	select RateValue
	from Unit_Rate WITH (NOLOCK) 
	where UnitFrom = psd.PoUnit and UnitTo = psd.StockUnit
)r
where 1=1
");
                #endregion
            }

            #region where條件
            if (!MyUtility.Check.Empty(StartSPNo))
            {
                sqlcmd.Append(string.Format(" and psd.id >= '{0}'", StartSPNo));
            }
            if (!MyUtility.Check.Empty(EndSPNo))
            {
                sqlcmd.Append(string.Format(" and (psd.id <= '{0}' or psd.id like '{0}%')", EndSPNo));
            }
            if (!MyUtility.Check.Empty(MDivision))
            {
                sqlcmd.Append(string.Format(" and o.MDivisionID = '{0}'", MDivision));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlcmd.Append(string.Format(" and o.FtyGroup = '{0}'", Factory));
            }
            if (!MyUtility.Check.Empty(StartRefno))
            {
                sqlcmd.Append(string.Format(" and psd.Refno >= '{0}'", StartRefno));
            }
            if (!MyUtility.Check.Empty(EndRefno))
            {
                sqlcmd.Append(string.Format(" and (psd.Refno <= '{0}' or psd.Refno like '{0}%')", EndRefno));
            }
            if (!MyUtility.Check.Empty(Color))
            {
                sqlcmd.Append(string.Format(" and psd.ColorID = '{0}'", Color));
            }

            if (!MyUtility.Check.Empty(MT))
            {
                if (MT != "All")
                {
                    sqlcmd.Append(string.Format(" and psd.FabricType = '{0}'", MT));
                }
            }
            if (!MyUtility.Check.Empty(ST))
            {
                if (ST != "All")
                {
                    if (ReportType == 0)
                    {
                            {
                            sqlcmd.Append(string.Format(" and fi.StockType = '{0}'", ST));
                        }
                        }
                    else
                    {
                        if (ST == "B")
                        {
                            sqlcmd.Append(" and (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.LInvQty,2)>0");
                        }
                        else if (ST == "I")
                        {
                            sqlcmd.Append(" and round(mpd.LInvQty,2) > 0");
                        }
                        else if (ST == "O")
                        {
                            sqlcmd.Append(" and round(mpd.LObQty ,2) > 0");
                        }
                    }
                }
            }
            if (boolCheckQty)
            {
                if (ReportType == 0)
                    sqlcmd.Append(" and (round(fi.InQty,2) - round(fi.OutQty,2) + round(fi.AdjustQty,2))>0");
                else
                    sqlcmd.Append(" and round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)>0");
            }

            if (!MyUtility.Check.Empty(BuyerDelivery1))
            {
                sqlcmd.Append($" and o.BuyerDelivery >='{((DateTime)BuyerDelivery1).ToString("yyyy/MM/dd")}'");
            }

            if (!MyUtility.Check.Empty(BuyerDelivery2))
            {
                sqlcmd.Append($" and o.BuyerDelivery <='{((DateTime)BuyerDelivery2).ToString("yyyy/MM/dd")}'");
            }

            if (!MyUtility.Check.Empty(ETA1))
            {
                sqlcmd.Append($" and psd.FinalETA >='{((DateTime)ETA1).ToString("yyyy/MM/dd")}'");
            }

            if (!MyUtility.Check.Empty(ETA2))
            {
                sqlcmd.Append($" and psd.FinalETA <='{((DateTime)ETA2).ToString("yyyy/MM/dd")}'");
            }

            if (this.bulk || this.sample || this.material || this.smtl) 
            {
                sqlcmd.Append(" and (1=0");
                if (this.bulk)
                {
                    sqlcmd.Append(" or o.Category = 'B'");
                }

                if (this.sample)
                {
                    sqlcmd.Append(" or o.Category = 'S'");
                }

                if (this.material)
                {
                    sqlcmd.Append(" or o.Category = 'M'");
                }

                if (this.smtl)
                {
                    sqlcmd.Append(" or o.Category = 'T'");
                }

                sqlcmd.Append(")");
            }

            if (this.complete)
            {
                sqlcmd.Append(" and psd.Complete = '1'");
            }

            if (!MyUtility.Check.Empty(arriveWH1) && !MyUtility.Check.Empty(arriveWH2))
            {
                sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival between '{((DateTime)arriveWH1).ToString("yyyy/MM/dd")}' and '{((DateTime)arriveWH2).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate between '{((DateTime)arriveWH1).ToString("yyyy/MM/dd")}' and '{((DateTime)arriveWH2).ToString("yyyy/MM/dd")}') 
)
");
            }
            else if (!MyUtility.Check.Empty(arriveWH1))
            {
                sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival >= '{((DateTime)arriveWH1).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate >= '{((DateTime)arriveWH1).ToString("yyyy/MM/dd")}' ) 
)
");
            }
            else if (!MyUtility.Check.Empty(arriveWH2))
            {
                sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival <= '{((DateTime)arriveWH2).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate <= '{((DateTime)arriveWH2).ToString("yyyy/MM/dd")}' ) 
)
");
            }

            #endregion

            #region Get Data
            DualResult result;

            //先抓出資料筆數 大於100萬筆需另外處理(按年出多excel)
            if (result = DBProxy.Current.Select(null, sql_cnt + sqlcmd.ToString(), out printData_cnt))
            {
                data_cnt = (int)printData_cnt.Rows[0]["datacnt"];
             
                return result;
            }

            #endregion
            return Result.True;
        }
        
        protected override bool OnToExcel(Win.ReportDefinition report)
        {

            #region check printData
            if (data_cnt == 0 )
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(data_cnt);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            DualResult result;
            string reportname = "";
            if (ReportType == 0)
            {
                sqlcmd_fin.Append(sqlcolumn + sqlcmd.ToString());
                reportname = "Warehouse_R21_Detail.xltx";
            }
            else
            {
                sqlcmd_fin.Append(sqlcolumn_sum + sqlcmd.ToString());
                reportname = "Warehouse_R21_Summary.xltx";
            }
                

            if (data_cnt > 1000000)
            {
                string sqlcmd_dtail;
                Excel.Application objApp;
                Excel.Worksheet tmpsheep = null;
                Sci.Utility.Report.ExcelCOM com;
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName((ReportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary");
                int sheet_cnt = 1;
                int split_cnt = 500000;
                result = DBProxy.Current.Select(null, sql_yyyy + sqlcmd, out printData_yyyy);
                if (!result ) {
                    this.HideWaitMessage();
                    MyUtility.Msg.ErrorBox(result.Messages.ToString());
                    return result;
                }
                string[] exl_name = new string[printData_yyyy.Rows.Count];
                DataTable tmpTb;
                for (int i = 0; i < printData_yyyy.Rows.Count; i++)
                {
                    sqlcmd_dtail = sqlcmd_fin.ToString() + string.Format("  and o.SciDelivery >= '{0}' and  o.SciDelivery < = '{1}' ", printData_yyyy.Rows[i][0].ToString() + "0101", printData_yyyy.Rows[i][0].ToString() + "1231");
                    strExcelName = Sci.Production.Class.MicrosoftFile.GetName((ReportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary" + printData_yyyy.Rows[i][0].ToString());
                    exl_name[i] = strExcelName;
                    com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\" + reportname, null);
                    objApp = com.ExcelApp;
                    com.TransferArray_Limit = 10000;
                    com.ColumnsAutoFit = false;
                    sheet_cnt = 1;

                    if (DBProxy.Current.Select(null, sqlcmd_dtail, out printData))
                    {
                        //如果筆數超過split_cnt再拆一次sheet
                        if (printData.Rows.Count > split_cnt)
                        {
                            int max_sheet_cnt = (int)Math.Floor((decimal)(printData.Rows.Count / split_cnt));
                            for (int j = 0; j <= max_sheet_cnt; j++)
                            {
                                if (j < max_sheet_cnt) {
                                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]).Copy(objApp.Workbooks[1].Worksheets[sheet_cnt]);
                                }
                                tmpsheep = ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]);
                                tmpsheep.Name = printData_yyyy.Rows[i][0].ToString() + "-" + (j + 1).ToString();
                                ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Sheets[sheet_cnt]).Select();
                                tmpTb = printData.AsEnumerable().Skip(j * split_cnt).Take(split_cnt).CopyToDataTable();

                                DualResult ok = com.WriteTable(tmpTb, 2);

                                sheet_cnt++;
                                if (tmpTb != null)
                                {
                                    tmpTb.Rows.Clear();
                                    tmpTb.Constraints.Clear();
                                    tmpTb.Columns.Clear();
                                    tmpTb.ExtendedProperties.Clear();
                                    tmpTb.ChildRelations.Clear();
                                    tmpTb.ParentRelations.Clear();
                                    tmpTb.Dispose();

                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    GC.Collect();
                                }
                            }
                        }
                        else
                        {
                            //複製sheet
                            tmpsheep = ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]);
                            tmpsheep.Name = printData_yyyy.Rows[i][0].ToString();
                            ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Sheets[sheet_cnt]).Select();
                            com.WriteTable(printData, 2);
                            sheet_cnt++;
                        }

                        if (printData != null)
                        {
                            printData.Rows.Clear();
                            printData.Constraints.Clear();
                            printData.Columns.Clear();
                            printData.ExtendedProperties.Clear();
                            printData.ChildRelations.Clear();
                            printData.ParentRelations.Clear();
                            printData.Dispose();

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();
                        }
                    }
                    //刪除多餘sheet
                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt + 1]).Delete();
                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]).Delete();
                    objApp.ActiveWorkbook.SaveAs(strExcelName);
                    if (tmpsheep != null) {
                        Marshal.ReleaseComObject(tmpsheep);
                    }
                    for (int f = 1; f <= objApp.Workbooks[1].Worksheets.Count; f++)
                    {
                        Excel.Worksheet sheet = objApp.Workbooks[1].Worksheets.Item[f];
                        if (sheet != null) Marshal.ReleaseComObject(sheet);
                    }
                    objApp.Workbooks[1].Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                
                Marshal.ReleaseComObject(tmpsheep);
                foreach (string filrstr in exl_name)
                {
                    filrstr.OpenFile();
                }
            }
            else
            {
                result = DBProxy.Current.Select(null,  sqlcmd_fin.ToString(), out printData);
                if (!result)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.ErrorBox(result.Messages.ToString());
                    return result;
                }
                
                Sci.Utility.Report.ExcelCOM com =  new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\" + reportname, null); 
                Excel.Application objApp = com.ExcelApp;
                //MyUtility.Excel.CopyToXls(printData, "", reportname, 1, showExcel: false, excelApp: objApp);
                com.WriteTable(printData, 2);
                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName((ReportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary");
                for (int f = 1; f <= objApp.Workbooks[1].Worksheets.Count; f++)
                {
                    Excel.Worksheet sheet = objApp.Workbooks[1].Worksheets.Item[f];
                    sheet.UsedRange.Rows.AutoFit();
                    if (sheet != null) Marshal.ReleaseComObject(sheet);
                }

                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Workbooks[1].Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
               
                strExcelName.OpenFile();
            }


            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }

    }
}
