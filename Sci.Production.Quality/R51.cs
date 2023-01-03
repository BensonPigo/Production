﻿using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R51 : Win.Tems.PrintForm
    {
        private readonly List<SqlParameter> Parameters = new List<SqlParameter>();
        private readonly StringBuilder Sqlcmd = new StringBuilder();
        private string sqlCol;
        private DataTable PrintData;
        private DataTable CustomColumnDt;

        /// <inheritdoc/>
        public R51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex();
            this.comboFactory1.SetDataSource();
            MyUtility.Tool.SetupCombox(this.comboShift, 1, 1, ",Day,Night");
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Parameters.Clear();
            this.Sqlcmd.Clear();
            this.sqlCol = string.Empty;
            this.CustomColumnDt = null;

            if (this.dateInspectionDate.Value1.Empty() && this.txtSP.Text.Empty())
            {
                MyUtility.Msg.WarningBox("<Inspection Date>, <SP#> can not all empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtsubprocess.Text))
            {
                MyUtility.Msg.WarningBox(" <SubProcess> can not empty!");
                return false;
            }

            string subProcessCondition = this.txtsubprocess.Text.Split(',').Select(s => $"'{s}'").JoinToString(",");
            string formatCol;
            string formatCol2 = string.Empty;
            string formatCol3 = string.Empty;
            string formatJoin;
            string s_d = $@"
outer apply(select ttlSecond_RD = sum(DATEDIFF(Second, StartResolveDate, EndResolveDate)) from SubProInsRecord_ResponseTeam where EndResolveDate is not null and SubProInsRecordUkey = SR.Ukey)ttlSecond_RD
outer apply(
	select SubProResponseTeamID = STUFF((
		select CONCAT(',', SubProResponseTeamID)
		from SubProInsRecord_ResponseTeam
		where SubProInsRecordUkey = SR.Ukey
		order by SubProResponseTeamID
		for xml path('')
	),1,1,'')
)SubProResponseTeamID
";

            this.sqlCol = $@"select AssignColumn,DisplayName,SubProcessID from SubProCustomColumn where SubProcessID in ({subProcessCondition}) order by AssignColumn";
            if (this.radioSummary.Checked)
            {
                formatJoin = @"outer apply (select [val] = sum(SRD.DefectQty)
                                            from SubProInsRecord_Defect SRD WITH(NOLOCK)
		                                    where SR.Ukey=SRD.SubProInsRecordUkey ) DefectQty" + s_d;
                formatCol = "DefectQty.val,";
            }
            else if (this.radioDetail_DefectType.Checked)
            {
                formatJoin = @"
left join SubProMachine m on SR.Machine = m.ID and SR.FactoryID = m.FactoryID and SR.SubProcessID = m.SubProcessID
left join SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey" + s_d;
                formatCol = @"m.Serial,
[Junk] = iif(m.Junk = 1, 'Y', 'N'),
m.Description,
SRD.DefectCode,                                
SRD.DefectQty,";
            }
            else
            {
                formatJoin = @"left join SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey
left join SubProInsRecord_ResponseTeam SRR on SRR.SubProInsRecordUkey = SR.Ukey
outer apply(select ttlSecond_RD = DATEDIFF(Second, StartResolveDate, EndResolveDate))ttlSecond_RD";
                formatCol = @"  SRD.DefectCode,
                                SRD.DefectQty,";
                formatCol2 = $@"SRR.StartResolveDate,
    SRR.EndResolveDate,
";
            }

            #region where
            StringBuilder sqlwhere1 = new StringBuilder();
            StringBuilder sqlwhere2 = new StringBuilder();
            StringBuilder declare = new StringBuilder();
            if (!this.dateInspectionDate.Value1.Empty())
            {
                sqlwhere1.Append("\r\nand SR.InspectionDate between @InspectionDate1 and @InspectionDate2");
                sqlwhere2.Append("\r\nand SR.InspectionDate between @InspectionDate1 and @InspectionDate2");
                this.Parameters.Add(new SqlParameter("@InspectionDate1p", SqlDbType.Date) { Value = this.dateInspectionDate.Value1 });
                this.Parameters.Add(new SqlParameter("@InspectionDate2p", SqlDbType.Date) { Value = this.dateInspectionDate.Value2 });
                declare.Append("\r\ndeclare @InspectionDate1 Date = @InspectionDate1p\r\ndeclare @InspectionDate2 Date = @InspectionDate2p");
            }

            if (!this.txtSP.Text.Empty())
            {
                sqlwhere1.Append("\r\nand B.OrderID = @SP");
                sqlwhere2.Append("\r\nand BR.OrderID = @SP");
                this.Parameters.Add(new SqlParameter("@SPp", SqlDbType.VarChar, 13) { Value = this.txtSP.Text });
                declare.Append("\r\ndeclare @SP varchar(13) = @SPp");
            }

            if (!this.txtstyle1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand O.StyleID= @Style");
                sqlwhere2.Append("\r\nand O.StyleID= @Style");
                this.Parameters.Add(new SqlParameter("@Stylep", SqlDbType.VarChar, 15) { Value = this.txtstyle1.Text });
                declare.Append("\r\ndeclare @Style varchar(15) = @Stylep");
            }

            sqlwhere1.Append($"\r\nand SR.SubProcessID in ({subProcessCondition})");
            sqlwhere2.Append($"\r\nand SR.SubProcessID in ({subProcessCondition})");

            if (!this.comboMDivision1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand B.MDivisionID= @M");
                sqlwhere2.Append("\r\nand BR.MDivisionID= @M");
                this.Parameters.Add(new SqlParameter("@Mp", SqlDbType.VarChar, 8) { Value = this.comboMDivision1.Text });
                declare.Append("\r\ndeclare @M varchar(8) = @Mp");
            }

            if (!this.comboFactory1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand SR.FactoryID = @F");
                sqlwhere2.Append("\r\nand SR.FactoryID = @F");
                this.Parameters.Add(new SqlParameter("@Fp", SqlDbType.VarChar, 8) { Value = this.comboFactory1.Text });
                declare.Append("\r\ndeclare @F varchar(8) = @Fp");
            }

            if (!this.comboShift.Text.Empty())
            {
                sqlwhere1.Append("\r\nand SR.Shift = @Shift");
                sqlwhere2.Append("\r\nand SR.Shift = @Shift");
                this.Parameters.Add(new SqlParameter("@Shiftp", SqlDbType.VarChar, 5) { Value = this.comboShift.Text });
                declare.Append("\r\ndeclare @Shift varchar(5) = @Shiftp");
            }
            #endregion
            this.Sqlcmd.Append(declare);
            this.Sqlcmd.Append($@"
select
    SR.FactoryID,
    SR.SubProLocationID,
	SR.InspectionDate,
    O.SewInLine,
	B.Sewinglineid,
    SR.Shift,
	[RFT] = iif(isnull(BD.Qty, 0) = 0, 0, round((isnull(BD.Qty, 0)- isnull(SR.RejectQty, 0)) / Cast(BD.Qty as float),2)),
	SR.SubProcessID,
	SR.BundleNo,
    [Artwork] = Artwork.val,
	B.OrderID,
    Country.Alias,
    o.BuyerDelivery,
	BD.BundleGroup,
    o.SeasonID,
	O.styleID,
	B.Colorid,
	BD.SizeCode,
    BD.PatternDesc,
    B.Item,
	BD.Qty,
	SR.RejectQty,
	SR.Machine,
	{formatCol}
	Inspector = (SELECT CONCAT(a.ID, ':', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = SR.AddName),
	SR.Remark,
    AddDate2 = SR.AddDate,
    SR.RepairedDatetime,
	RepairedTime = iif(RepairedDatetime is null, null, ttlSecond),
    {formatCol2}
	ResolveTime = iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	SubProResponseTeamID
    ,CustomColumn1
into #tmp
from SubProInsRecord SR WITH (NOLOCK)
Left join Bundle_Detail BD WITH (NOLOCK) on SR.BundleNo=BD.BundleNo
Left join Bundle B WITH (NOLOCK) on BD.ID=B.ID
Left join Orders O WITH (NOLOCK) on B.OrderID=O.ID
left join Country on Country.ID = o.Dest
outer apply(SELECT val =  Stuff((select distinct concat( '+',SubprocessId)   
                                    from Bundle_Detail_Art bda with (nolock) 
                                    where bda.Bundleno = BD.Bundleno
                                    FOR XML PATH('')),1,1,'') ) Artwork
{formatJoin}
outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
Where 1=1
");
            this.Sqlcmd.Append(sqlwhere1);
            this.Sqlcmd.Append($@"
UNION

select
    SR.FactoryID,
    SR.SubProLocationID,
	SR.InspectionDate,
    O.SewInLine,
    BR.Sewinglineid,
    SR.Shift,
	[RFT] = iif(isnull(BRD.Qty, 0) = 0, 0, round((isnull(BRD.Qty, 0)- isnull(SR.RejectQty, 0)) / Cast(BRD.Qty as float),2)),
	SR.SubProcessID,
	SR.BundleNo,
    [Artwork] = Artwork.val,
	BR.OrderID,
    Country.Alias,
    o.BuyerDelivery,
	BRD.BundleGroup,
    o.SeasonID,
	O.styleID,
	BR.Colorid,
	BRD.SizeCode,
    BRD.PatternDesc,
    BR.Item,
	BRD.Qty,
	SR.RejectQty,
	SR.Machine,
	{formatCol}
	Inspector = (SELECT CONCAT(a.ID, ':', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = SR.AddName),
	SR.Remark,
    AddDate2 = SR.AddDate,
    SR.RepairedDatetime,
	iif(RepairedDatetime is null, null, ttlSecond),
    {formatCol2}
	iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	SubProResponseTeamID
    ,CustomColumn1--自定義欄位, 在最後一個若有變動,則輸出Excel部分也要一起改
from SubProInsRecord SR WITH (NOLOCK)
Left join BundleReplacement_Detail BRD WITH (NOLOCK) on SR.BundleNo=BRD.BundleNo
Left join BundleReplacement BR WITH (NOLOCK) on BRD.ID=BR.ID
Left join Orders O WITH (NOLOCK) on BR.OrderID=O.ID
left join Country on Country.ID = o.Dest
outer apply(SELECT val =  Stuff((select distinct concat( '+',SubprocessId)   
                                    from Bundle_Detail_Art bda with (nolock) 
                                    where bda.Bundleno = SR.BundleNo
                                    FOR XML PATH('')),1,1,'') ) Artwork
{formatJoin}
outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
Where 1=1
");
            this.Sqlcmd.Append(sqlwhere2);
            this.Sqlcmd.Append(@"
select *, BundleNoCT = COUNT(1) over(partition by t.BundleNo)
into #tmp2
from #tmp t

select *
from #tmp2 t
where BundleNoCT = 1--綁包/補料都沒有,在第一段union會合併成一筆
or (BundleNoCT > 1 and isnull(t.Orderid, '') <> '')--綁包/補料其中一個有

drop table #tmp,#tmp2
");

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (!this.sqlCol.Empty())
            {
                DualResult result = DBProxy.Current.Select(null, this.sqlCol, out this.CustomColumnDt);
                if (!result)
                {
                    return result;
                }
            }

            return DBProxy.Current.Select(null, this.Sqlcmd.ToString(), this.Parameters, out this.PrintData);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.PrintData.Rows.Count);

            if (this.PrintData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.PrintData.Columns.Remove("BundleNoCT");

            string filename = this.radioSummary.Checked ? "Quality_R51_Summary.xltx" : this.radioDetail_DefectType.Checked ? "Quality_R51_Detail_DefectType.xltx" : "Quality_R51_Detail_Responseteam.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app

            Excel.Workbook xlWb = excelApp.ActiveWorkbook as Excel.Workbook;
            Excel.Worksheet xlSht = xlWb.Sheets[1];

            var listSubProcess = this.PrintData.AsEnumerable().Select(s => s["SubProcessID"].ToString()).Distinct();
            int sheetCnt = 2;
            int customColumn = this.PrintData.Columns.Count; // 自定義欄位最後一欄

            foreach (string subprocessID in listSubProcess)
            {
                xlSht.Copy(Type.Missing, xlWb.Sheets[1]);

                DataTable dtSubprocess = this.PrintData.AsEnumerable().Where(s => s["SubProcessID"].ToString() == subprocessID).CopyToDataTable();

                MyUtility.Excel.CopyToXls(dtSubprocess, string.Empty, filename, 2, false, null, excelApp, wSheet: excelApp.Sheets[sheetCnt]);
                xlWb.Sheets[sheetCnt].Name = subprocessID;

                if (this.CustomColumnDt != null)
                {
                    var custColumnNames = this.CustomColumnDt.AsEnumerable().Where(s => s["SubProcessID"].ToString() == subprocessID);
                    foreach (DataRow dr in custColumnNames)
                    {
                        xlWb.Sheets[sheetCnt].Cells[1, customColumn] = dr["DisplayName"];
                        customColumn++;
                    }

                    customColumn = this.PrintData.Columns.Count;
                }
            }

            excelApp.DisplayAlerts = false;
            xlSht.Delete();

            excelApp.Visible = true;
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(filename);

            xlWb.SaveAs(strExcelName);

            Marshal.ReleaseComObject(xlSht);
            Marshal.ReleaseComObject(xlWb);
            Marshal.ReleaseComObject(excelApp);
            #endregion
            return true;
        }

        private void TxtShiftTime_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!TimeSpan.TryParse(((Sci.Win.UI.TextBox)sender).Text, out TimeSpan _))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Incorrect time format");
            }
        }
    }
}
