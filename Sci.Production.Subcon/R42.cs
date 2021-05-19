﻿using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class R42 : Win.Tems.PrintForm
    {
        private StringBuilder sqlCmd;
        private string SubProcess;
        private string SP;
        private string M;
        private string Factory;
        private string CutRef1;
        private string CutRef2;
        private string processLocation;
        private DateTime? dateBundle1;
        private DateTime? dateBundle2;
        private DateTime? dateBundleTransDate1;
        private DateTime? dateBundleTransDate2;

        /// <inheritdoc/>
        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Comboload();
            this.comboFactory.SetDataSource();
            this.comboRFIDProcessLocation.SetDataSource();
            this.comboRFIDProcessLocation.SelectedIndex = 0;
        }

        // string date = "";
        private void Comboload()
        {
            DualResult result;
            if (result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory WITH (NOLOCK) ", out DataTable dtfactory))
            {
                this.comboM.DataSource = dtfactory;
                this.comboM.DisplayMember = "ID";
            }
            else
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBundleCDate.Value1) && MyUtility.Check.Empty(this.dateBundleCDate.Value2) &&
                MyUtility.Check.Empty(this.dateBundleTransDate.Value1) && MyUtility.Check.Empty(this.dateBundleTransDate.Value2))
            {
                MyUtility.Msg.WarningBox("Bundel CDate or Bundle Trans date can't empty!!");
                return false;
            }

            this.SubProcess = this.txtsubprocess.Text;
            this.SP = this.txtSPNo.Text;
            this.M = this.comboM.Text;
            this.Factory = this.comboFactory.Text;
            this.CutRef1 = this.txtCutRefStart.Text;
            this.CutRef2 = this.txtCutRefEnd.Text;
            this.dateBundle1 = this.dateBundleCDate.Value1;
            this.dateBundle2 = this.dateBundleCDate.Value2;
            this.dateBundleTransDate1 = this.dateBundleTransDate.Value1;
            this.dateBundleTransDate2 = this.dateBundleTransDate.Value2;
            this.processLocation = this.comboRFIDProcessLocation.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.sqlCmd = new StringBuilder();

            // 效能: 看起來多餘的寫法, SP#分兩個欄位撈, 存入#tmp再組起來, 直接組起來要花4倍時間
            // 因為BundleTransfer 的table太肥，如果有用到這個條件則修改寫法
            if (this.dateBundleTransDate1 == null && this.dateBundleTransDate2 == null)
            {
                #region sqlcmd

                this.sqlCmd.Append(@"
select PostSewingSubProcess,NoBundleCardAfterSubprocess,SubprocessId,Bundleno
into #tmp_Bundle_Detail_Art 
from Bundle_Detail_Art WITH (NOLOCK) 
where PostSewingSubProcess = 1 or NoBundleCardAfterSubprocess = 1


Select
    [Bundle#] = bt.BundleNo,
    [RFIDProcessLocationID] = bt.RFIDProcessLocationID,
	[FabricKind] = FabricKind.val,
    [Cut Ref#] = b.CutRef,
    [SP#] = dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo order by OrderID for XML RAW)),
    [Master SP#] = b.POID,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Comb] = b.PatternPanel,
    [Cutno] = b.Cutno,
    [Article] = b.Article,
    [Color] = b.ColorId,
    [Line] = b.SewinglineId,
	bt.SewingLineID,
    [Cell] = b.SewingCell,
    [Pattern] = bd.PatternCode,
    [PtnDesc] = bd.PatternDesc,
    [Group] = bd.BundleGroup,
    [Size] = bd.SizeCode,
    [Qty] = bd.Qty,
    [RFID Reader] = bt.RFIDReaderId,
    [Sub-process] = bt.SubprocessId,
    [Post Sewing SubProcess]= iif(ps.sub = 1,N'✔',''),
    [No Bundle Card After Subprocess]= iif(nbs.sub= 1,N'✔',''),
    [Type] = case when bt.Type = '1' then 'IN'
			        when bt.Type = '2' then 'Out'
			        when bt.Type = '3' then 'In/Out'
			        when bt.Type = '4' then 'Out/In' end,
    [TagId] = bt.TagId,
    [TransferDate] = CAST(TransferDate AS DATE),
    [TransferTime] = TransferDate,
    bt.LocationID
    ,b.item
	,bt.PanelNo
	,CutCellID
into #tmp
from BundleTransfer bt WITH (NOLOCK, Index(BundleTransferDate))
left join Bundle_Detail bd WITH (NOLOCK) on bt.BundleNo = bd.BundleNo
left join Bundle b WITH (NOLOCK) on bd.Id = b.Id
left join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
outer apply(
    select sub = 1
    from #tmp_Bundle_Detail_Art bda WITH (NOLOCK) 
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PostSewingSubProcess = 1
    and bda.SubprocessId = bt.SubprocessId
) as ps
outer apply(
    select sub = 1
    from #tmp_Bundle_Detail_Art bda WITH (NOLOCK) 
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.NoBundleCardAfterSubprocess = 1
    and bda.SubprocessId = bt.SubprocessId
) as nbs 
outer apply(
	SELECT top 1 [val] = DD.id + '-' + DD.NAME
	FROM dropdownlist DD 
	where exists (
		SELECT 1 
		FROM order_colorcombo OCC WITH (NOLOCK)
		INNER JOIN order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		AND OCC.id = b.poid
		AND OCC.patternpanel = b.patternpanel
		AND DD.id = OB.kind 
	) 
    AND DD.[type] = 'FabricKind'
)FabricKind
where 1=1
            ");
                #endregion
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(this.SubProcess))
                {
                    this.sqlCmd.Append($@" and (bt.SubprocessId in ('{this.SubProcess.Replace(",", "','")}') or '{this.SubProcess}'='')");
                }

                if (!MyUtility.Check.Empty(this.CutRef1) && (!MyUtility.Check.Empty(this.CutRef1)))
                {
                    this.sqlCmd.Append(string.Format(@" and b.CutRef between '{0}' and '{1}'", this.CutRef1, this.CutRef2));
                }

                if (!MyUtility.Check.Empty(this.SP))
                {
                    this.sqlCmd.Append(string.Format(@" and exists(select 1 from Bundle_Detail_Order with(nolock) where bundleNo = bd.bundleNo and Orderid= '{0}')", this.SP));
                }

                if (!MyUtility.Check.Empty(this.dateBundle1))
                {
                    this.sqlCmd.Append(string.Format(@" and b.Cdate >= '{0}'", Convert.ToDateTime(this.dateBundle1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateBundle2))
                {
                    this.sqlCmd.Append(string.Format(@" and b.Cdate <= '{0}'", Convert.ToDateTime(this.dateBundle2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateBundleTransDate1))
                {
                    this.sqlCmd.Append(string.Format(@" and bt.TransferDate >= '{0}'", Convert.ToDateTime(this.dateBundleTransDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateBundleTransDate2))
                {
                    // TransferDate 是 datetime, 直接用日期做判斷的話要加一天才不會漏掉最後一天的資料
                    this.sqlCmd.Append(string.Format(@" and bt.TransferDate <= '{0}'", Convert.ToDateTime(((DateTime)this.dateBundleTransDate2).AddDays(1)).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.M))
                {
                    this.sqlCmd.Append(string.Format(@" and b.MDivisionid = '{0}'", this.M));
                }

                if (!MyUtility.Check.Empty(this.Factory))
                {
                    this.sqlCmd.Append(string.Format(@" and o.FtyGroup = '{0}'", this.Factory));
                }

                if (this.processLocation != "ALL")
                {
                    this.sqlCmd.Append(string.Format(@" and bt.RFIDProcessLocationID = '{0}'", this.processLocation));
                }
                #endregion
            }
            else
            {
                this.sqlCmd.Append($@"
select PostSewingSubProcess,NoBundleCardAfterSubprocess,SubprocessId,Bundleno
into #tmp_Bundle_Detail_Art 
from Bundle_Detail_Art WITH (NOLOCK) 
where PostSewingSubProcess = 1 or NoBundleCardAfterSubprocess = 1

--Replace1

Select 
    [Bundle#] = bt.BundleNo,
    [RFIDProcessLocationID] = bt.RFIDProcessLocationID,
    [FabricKind] = FabricKind.val,
    [Cut Ref#] = b.CutRef,
    [SP#] = dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo order by OrderID for XML RAW)),
    [Master SP#] = b.POID,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Comb] = b.PatternPanel,
    [Cutno] = b.Cutno,
    [Article] = b.Article,
    [Color] = b.ColorId,
    [Line] = b.SewinglineId,
    bt.SewingLineID,
    [Cell] = b.SewingCell,
    [Pattern] = bd.PatternCode,
    [PtnDesc] = bd.PatternDesc,
    [Group] = bd.BundleGroup,
    [Size] = bd.SizeCode,
    [Qty] = bd.Qty,
    [RFID Reader] = bt.RFIDReaderId,
    [Sub-process] = bt.SubprocessId,
    [Post Sewing SubProcess]= iif(ps.sub = 1,N'✔',''),
    [No Bundle Card After Subprocess]= iif(nbs.sub= 1,N'✔',''),
    [Type] = case when bt.Type = '1' then 'IN'
			        when bt.Type = '2' then 'Out'
			        when bt.Type = '3' then 'In/Out'
			        when bt.Type = '4' then 'Out/In' end,
    [TagId] = bt.TagId,
    [TransferDate] = CAST(bt.TransferDate AS DATE),
    [TransferTime] = bt.TransferDate,
    bt.LocationID
    ,b.item
    ,bt.PanelNo
    ,bt.CutCellID
into #tmp
from BundleTransfer bt WITH (NOLOCK, Index(BundleTransferDate))
inner join Bundle_Detail bd WITH (NOLOCK) on bd.BundleNo = bt.BundleNo
left join Bundle b WITH (NOLOCK) on b.id = bd.id
left join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
outer apply(
    select sub = 1
    from #tmp_Bundle_Detail_Art bda WITH (NOLOCK) 
    where bda.Bundleno = bt.Bundleno and bda.PostSewingSubProcess = 1
    and bda.SubprocessId = bt.SubprocessId
) as ps
outer apply(
    select sub = 1
    from #tmp_Bundle_Detail_Art bda WITH (NOLOCK) 
    where bda.Bundleno = bt.Bundleno and bda.NoBundleCardAfterSubprocess = 1
    and bda.SubprocessId = bt.SubprocessId
) as nbs 
outer apply(
	SELECT top 1 [val] = DD.id + '-' + DD.NAME
	FROM dropdownlist DD 
	where exists (
		SELECT 1 
		FROM order_colorcombo OCC WITH (NOLOCK)
		INNER JOIN order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		AND OCC.id = b.poid
		AND OCC.patternpanel = b.patternpanel
		AND DD.id = OB.kind 
	)
    AND DD.[type] = 'FabricKind'
)FabricKind
where 1=1
");
                if (!MyUtility.Check.Empty(this.SubProcess))
                {
                    this.sqlCmd.Append($@" and (bt.SubprocessId in ('{this.SubProcess.Replace(",", "','")}') or '{this.SubProcess}'='')" + Environment.NewLine);
                }

                if (this.processLocation != "ALL")
                {
                    this.sqlCmd.Append($@" and bt.RFIDProcessLocationID = '{this.processLocation}'" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(this.dateBundleTransDate1))
                {
                    this.sqlCmd.Append($@" and bt.TransferDate >= '{Convert.ToDateTime(this.dateBundleTransDate1).ToString("d")}'" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(this.dateBundleTransDate2))
                {
                    // TransferDate 是 datetime, 直接用日期做判斷的話要加一天才不會漏掉最後一天的資料
                    this.sqlCmd.Append($@" and bt.TransferDate <= '{Convert.ToDateTime(((DateTime)this.dateBundleTransDate2).AddDays(1)).ToString("d")}'" + Environment.NewLine + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(this.CutRef1) && (!MyUtility.Check.Empty(this.CutRef1)))
                {
                    this.sqlCmd.Append(string.Format(@" and b.CutRef between '{0}' and '{1}'", this.CutRef1, this.CutRef2));
                }

                if (!MyUtility.Check.Empty(this.SP))
                {
                    this.sqlCmd.Append(string.Format(@" and exists(select 1 from Bundle_Detail_Order with(nolock) where bundleNo = bd.bundleNo and Orderid= '{0}')", this.SP));
                }

                if (!MyUtility.Check.Empty(this.dateBundle1))
                {
                    this.sqlCmd.Append(string.Format(@" and b.Cdate >= '{0}'", Convert.ToDateTime(this.dateBundle1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateBundle2))
                {
                    this.sqlCmd.Append(string.Format(@" and b.Cdate <= '{0}'", Convert.ToDateTime(this.dateBundle2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.M))
                {
                    this.sqlCmd.Append(string.Format(@" and b.MDivisionid = '{0}'", this.M));
                }

                if (!MyUtility.Check.Empty(this.Factory))
                {
                    this.sqlCmd.Append(string.Format(@" and o.FtyGroup = '{0}'", this.Factory));
                }
            }

            this.sqlCmd.Append(@"
select [Bundle#],[RFIDProcessLocationID],[FabricKind],[Cut Ref#],
	[SP#],
	[Master SP#],[M],[Factory],[Style],[Season],[Brand],[Comb],[Cutno],[Article],[Color],[Line],SewingLineID,
	[Cell],[Pattern],[PtnDesc],[Group],[Size],[Qty],[RFID Reader],[Sub-process],[Post Sewing SubProcess],
	[No Bundle Card After Subprocess],[Type],[TagId],[TransferDate],[TransferTime],LocationID,item,PanelNo,CutCellID
from #tmp

drop table #tmp
");
            DBProxy.Current.DefaultTimeout = 3600;  // 加長時間為30分鐘，避免timeout
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R42_Bundle Transaction detail (RFID).xltx"); // 預先開啟excel app
            decimal excelMaxrow = 1000000;

            Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[2];
            worksheet1.Copy(worksheetn);

            int sheet = 1;

            // 因為一次載入太多筆資料到DataTable 會造成程式佔用大量記憶體，改為每1萬筆載入一次並貼在excel上
            #region 分段抓取資料填入excel
            this.ShowLoadingText($"Data Loading , please wait …");
            DataTable tmpDatas = new DataTable();
            DBProxy.Current.OpenConnection(this.ConnectionName, out SqlConnection conn);
            var cmd = new SqlCommand(this.sqlCmd.ToString(), conn)
            {
                CommandTimeout = 3000,
            };
            var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            int loadCounts = 0;
            int loadCounts2 = 0;
            int eachCopy = 100000;
            using (conn)
            using (reader)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    tmpDatas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }

                while (reader.Read())
                {
                    object[] items = new object[reader.FieldCount];
                    reader.GetValues(items);
                    tmpDatas.LoadDataRow(items, true);
                    loadCounts++;
                    loadCounts2++;
                    if (loadCounts % eachCopy == 0)
                    {
                        this.ShowLoadingText($"Data Loading – {loadCounts} , please wait …");
                        MyUtility.Excel.CopyToXls(tmpDatas, string.Empty, "Subcon_R42_Bundle Transaction detail (RFID).xltx", loadCounts2 - (eachCopy - 1), false, null, objApp, wSheet: objApp.Sheets[sheet]); // 將datatable copy to excel

                        this.DataTableClearAll(tmpDatas);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            tmpDatas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                        }

                        if (loadCounts % excelMaxrow == 0)
                        {
                            Microsoft.Office.Interop.Excel.Worksheet worksheetA = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[sheet + 1];
                            Microsoft.Office.Interop.Excel.Worksheet worksheetB = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[sheet + 2];
                            worksheetA.Copy(worksheetB);
                            sheet++;
                            loadCounts2 = 0;
                        }
                    }
                }

                if (loadCounts > 0)
                {
                    MyUtility.Excel.CopyToXls(tmpDatas, string.Empty, "Subcon_R42_Bundle Transaction detail (RFID).xltx", loadCounts2 - (loadCounts2 % eachCopy) + 1, false, null, objApp, wSheet: objApp.Sheets[sheet]); // 將datatable copy to excel
                    this.DataTableClearAll(tmpDatas);
                }
                else
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    this.HideLoadingText();
                    return false;
                }
            }

            this.SetCount((long)loadCounts);
            objApp.DisplayAlerts = false;
            ((Microsoft.Office.Interop.Excel.Worksheet)objApp.Sheets[sheet + 1]).Delete();
            ((Microsoft.Office.Interop.Excel.Worksheet)objApp.Sheets[1]).Select();
            objApp.DisplayAlerts = true;
            this.HideLoadingText();
            #endregion

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subcon_R42_BundleTransactiondetail(RFID)");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void DataTableClearAll(DataTable target)
        {
            target.Rows.Clear();
            target.Constraints.Clear();
            target.Columns.Clear();
            target.ExtendedProperties.Clear();
            target.ChildRelations.Clear();
            target.ParentRelations.Clear();
        }
    }
}
