using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P52 : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P12"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P52(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            MyUtility.Tool.SetupCombox(this.comboMaterialType, 2, 1, @"F,Fabric,A,Accessory");
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataSet materialSet = null;
        private MaterialtableDataSet materialSet2 = null;

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetupGrid_Material();
            this.SetupGrid_Document();
            this.SetupGrid_Report(string.Empty);
        }

        private void SetupGrid_Material()
        {
            this.Helper.Controls.Grid.Generator(this.grid_Material)
               .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
               .Text("Season", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("supplier", header: "Supplier", width: Widths.AnsiChars(17), iseditingreadonly: true)
               .Text("Refno", header: "Refno", width: Widths.AnsiChars(17), iseditingreadonly: true)
               .Text("BrandRefno", header: "BrandRefno", width: Widths.AnsiChars(17), iseditingreadonly: true)
               .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(11), iseditingreadonly: true, integer_places: 8, decimal_places: 2)
               ;

            this.SetGrid_HeaderBorderStyle(this.grid_Material, false);
        }

        private void SetupGrid_Document()
        {
            this.Helper.Controls.Grid.Generator(this.grid_Document)
                .Text("DocumentName", header: "Document Name", width: Widths.AnsiChars(18), iseditingreadonly: true)
                ;

            this.SetGrid_HeaderBorderStyle(this.grid_Document, false);
        }

        private void SetupGrid_Report(string fileRule)
        {
            this.grid_Report.Columns.Clear();
            switch (fileRule)
            {
                case "1":
                case "2":
                    this.grid_Report.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.grid_Report)
                .Button("...", header: "File", width: Widths.AnsiChars(8), onclick: this.ClickClip)
                .Date("TestReport", header: "Upload date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("TestReportTestDate", header: "Test Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("TestSeasonID", header: "Test Season", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("DueDate", header: "Due Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("FTYReceivedReport", header: "FTY Received Date", width: Widths.AnsiChars(13), iseditingreadonly: false, iseditable: true)
                .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                ;
                    break;
                case "3":
                    this.grid_Report.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.grid_Report)
                    .Text("DocSeason", header: "Doc Season", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Numeric("Period", header: "Period", width: Widths.AnsiChars(15), integer_places: 3, decimal_places: 0, iseditingreadonly: true)
                    .Text("ReceivedRemark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                    .Text("TestDocFactoryGroup", header: "FTY ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("FirstDyelot", header: "Sent Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("AWBno", header: "AWBno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("FTYReceivedReport", header: "FTY Received Date", width: Widths.AnsiChars(13), iseditingreadonly: false, iseditable: true)
                    .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                ;
                    break;
                case "4":
                case "5":
                    this.grid_Report.IsEditingReadOnly = false;
                    this.Helper.Controls.Grid.Generator(this.grid_Report)
                    .Button("...", header: "File", width: Widths.AnsiChars(8), onclick: this.ClickClip)
                    .Date("ReportDate", header: "Upload date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("AWBno", header: "AWBno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Text("ExportID", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("FTYReceivedReport", header: "FTY Received Date", width: Widths.AnsiChars(13), iseditingreadonly: false, iseditable: true)
                    .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
                    .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                    ;
                    break;
            }

            this.SetGrid_HeaderBorderStyle(this.grid_Report, false);
        }

        private void SetGrid_HeaderBorderStyle(Sci.Win.UI.Grid grid, bool withAlternateRowStyle = true)
        {
            grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;

            grid.RowHeadersVisible = true;
            grid.RowHeadersWidth = 22;
            grid.RowTemplate.Height = 19; // 小於19 的話會導致 checkBox column 不見

            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = System.Drawing.Color.DimGray;
            headerStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.WhiteSmoke;
            headerStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            headerStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            headerStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        private void ClickClip(object sender, EventArgs e)
        {
            var row = this.grid_Report.GetCurrentDataRow();
            if (row == null)
            {
                return;
            }

            var id = row["Ukey"].ToString();
            if (id.IsNullOrWhiteSpace())
            {
                return;
            }

            string tableName = string.Empty;

            switch (row["FileRule"].ToString())
            {
                case "1":
                case "2":
                    tableName = "UASentReport";
                    break;
                case "4":
                    tableName = "NewSentReport";
                    break;
                case "5":
                    tableName = "ExportRefnoSentReport";
                    break;
            }

            string sqlcmd = $@"select 
            [FileName] = TableName + PKey,
            SourceFile
            from Clip
            where TableName = '{tableName}' and 
            UniqueKey = '{id}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
            }

            List<string> list = new List<string>();
            string filePath = MyUtility.GetValue.Lookup($"select [path] from CustomizedClipPath where TableName = '{tableName}'");

            // 組ClipPath
            string clippath = MyUtility.GetValue.Lookup($"select ClipPath from System");
            string saveFilePath = clippath + "\\" + DateTime.Now.ToString("yyyyMM");

            foreach (DataRow dataRow in dt.Rows)
            {
                string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                lock (FileDownload_UpData.DownloadFileAsync("http://pmsap.sportscity.com.tw:16888/api/FileDownload/GetFile", filePath + "\\" + DateTime.Now.ToString("yyyyMM"), fileName, saveFilePath))
                {
                }
            }

            using (var dlg = new Clip(tableName, id, true))
            {
                dlg.EditMode = false;
                dlg.ShowDialog();

                foreach (DataRow dataRow in dt.Rows)
                {
                    string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                    string deleteFile = Path.Combine(saveFilePath, fileName);
                    if (File.Exists(deleteFile))
                    {
                        File.Delete(deleteFile);
                    }
                }
            }
        }

        private MaterialtableDataSet LoadMaterialtableData()
        {
            #region 必輸入條件
            if (MyUtility.Check.Empty(this.txtSP.Text) &&
                MyUtility.Check.Empty(this.txtSeq1.Text) &&
                MyUtility.Check.Empty(this.txtseq2.Text) &&
                MyUtility.Check.Empty(this.txtSeason.Text) &&
                MyUtility.Check.Empty(this.txtBrand.Text) &&
                MyUtility.Check.Empty(this.txtSupplier.Text) &&
                MyUtility.Check.Empty(this.txtRefno.Text) &&
                MyUtility.Check.Empty(this.txtColor.Text) &&
                MyUtility.Check.Empty(this.dateETA.Value1) &&
                MyUtility.Check.Empty(this.dateATA.Value1))
            {
                MyUtility.Msg.WarningBox("Please input any one filter before query!");
                this.txtSP.Select();
                return null;
            }
            #endregion
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                where += $@" and o.id = '{this.txtSP.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSeq1.Text))
            {
                where += $@" and p3.Seq1 = '{this.txtSeq1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtseq2.Text))
            {
                where += $@" and p3.Seq2 = '{this.txtseq2.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSeason.Text))
            {
                where += $@" and o.SeasonID = '{this.txtSeason.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $@" and o.BrandID = '{this.txtBrand.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSupplier.Text))
            {
                where += $@" and su.ID = '{this.txtSupplier.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                where += $@" and p3.Refno = '{this.txtRefno.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                where += $@" and psds.SpecValue = '{this.txtColor.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.dateETA.Value1) || !MyUtility.Check.Empty(this.dateETA.Value2))
            {
                where += $@" 
--Export ETA
and exists (
select 1 from Export_Detail ed
inner join Export on Export.id = ed.ID and Export.Confirm = 1
where ed.PoID = p3.id and ed.Seq1 = p3.Seq1 and ed.Seq2 = p3.Seq2 
and Export.Eta between '{((DateTime)this.dateETA.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateETA.Value2).ToString("yyyy/MM/dd")}') 
";
            }

            if (!MyUtility.Check.Empty(this.dateATA.Value1) || !MyUtility.Check.Empty(this.dateATA.Value2))
            {
                where += $@" 
--Export ATA
and exists (
select 1 from Export_Detail ed
inner join Export on Export.id = ed.ID and Export.Confirm = 1
where ed.PoID = p3.id and ed.Seq1 = p3.Seq1 and ed.Seq2 = p3.Seq2 
and Export.WhseArrival between '{((DateTime)this.dateATA.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateATA.Value2).ToString("yyyy/MM/dd")}') 
";
            }

            if (!MyUtility.Check.Empty(this.comboMaterialType.SelectedValue.ToString()))
            {
                where += $@"
and  f.Type = '{this.comboMaterialType.SelectedValue.ToString()}'
";
            }

            #endregion
            string sqlcmd = $@"
use Production
IF OBJECT_ID('tempdb..#POList') IS NOT NULL
    DROP TABLE #POList

Select distinct
o.FactoryID,
       MtltypeId,
	   POID = p3.ID,
       Seq = p3.Seq1 + '-' + p3.Seq2,
	   Season = o.SeasonID,	   
       p3.Seq1,
       p3.Seq2,
       p2.SuppID,
       supplier = IIF(Isnull(su.AbbEN, '') = '', su.ID, Concat(su.ID, '-', su.AbbEN)),
       FinalETD=IsNull(p3.CfmETD, p3.SystemETD),
       p3.Refno,
       Color = psds.SpecValue,
       p3.Qty,
       p3.ShipQty,
       p3.ShipETA,
       o.ProgramID,
       f.Type,
       Season.Month,
       o.BrandID,
       Category = ddl.Name,
       f.BrandRefno
into #POList
From Orders o with(nolock)
inner Join dbo.PO_Supp p2 with(nolock) on p2.ID = o.ID
inner Join dbo.PO_Supp_Detail p3 with(nolock) on p3.ID = p2.ID and p3.Seq1 = p2.SEQ1 
 	and IsNull(P3.Junk, 0) = 0 --作廢不顯示
	and IsNull(p3.Qty, 0) != 0 --數量為0不顯示
left join PO_Supp_Detail_spec  psds on psds.id = p3.ID and psds.Seq1 = p3.SEQ1 and psds.Seq2 = p3.SEQ2 and psds.SpecColumnID = 'Color'
inner Join dbo.Supp su with(nolock) on su.ID = p2.SuppID
inner join dbo.Fabric f with(nolock) on p3.SciRefno = f.SciRefno
inner JOIN Season WITH (NOLOCK) on o.SeasonID = Season.ID and o.BrandID = Season.BrandID
LEFT JOIN DropDownList ddl WITH (NOLOCK) on ddl.type ='Category' and o.Category = ddl.ID
where 1=1
{where}


select * from #POList
Order by POID,Seq

Select distinct md.DocumentName, md.FileRule, po.Seq, po.POID
FROM MaterialDocument md
inner join #POList po on md.BrandID = po.BrandID
Outer apply ( 
	SELECT value = STUFF((SELECT CONCAT(',', MtlType.MtltypeId) 
	FROM MaterialDocument_MtlType MtlType WITH (NOLOCK)
	WHERE MtlType.DocumentName = md.DocumentName and MtlType.BrandID = md.BrandID 
	FOR XML PATH (''))
	, 1, 1, '')
) MtlType
Outer apply ( 
	SELECT value = STUFF((SELECT CONCAT(',', supp.SuppID) 
	FROM MaterialDocument_Supplier supp WITH (NOLOCK)
	WHERE supp.DocumentName = md.DocumentName AND supp.BrandID = md.BrandID 
	FOR XML PATH ('')), 1, 1, '')
) supp
WHERE md.FabricType = po.Type 
and po.Month >= (select month From Season where id =md.ActiveSeason and BrandID = md.BrandID)
and (isnull(md.EndSeason,'') = '' or po.Month <= (select month From Season where id =md.EndSeason and BrandID = md.BrandID))
and (isnull(md.ExcludeProgram,'') = '' or po.ProgramID not in (select data from splitstring(md.ExcludeProgram,',')))
and (md.ExcludeReplace = 0 or po.seq1 < '50' or po.seq1 > '69')
and (md.ExcludeStock = 0 or po.seq1 < '70')
and (isnull(md.MtlTypeClude,'') = '' or isnull(MtlType.value,'') = '' 
    or (md.MtlTypeClude = 'E' and po.MtltypeId not in (select data from splitstring(MtlType.value, ',')))
    or (md.MtlTypeClude = 'I' and po.MtltypeId in (select data from splitstring(MtlType.value, ',')))
    )
and (isnull(md.SupplierClude,'') = '' or isnull(supp.value,'') = ''
    or (md.SupplierClude = 'E' and po.SuppID not in (select data from splitstring(supp.value, ',')))
    or (md.SupplierClude = 'I' and po.SuppID in (select data from splitstring(supp.value, ',')))
    )
and (po.Category in (select data from splitstring(md.Category,',')))
IF OBJECT_ID('tempdb..#POList') IS NOT NULL
DROP TABLE #POList

";
            DataSet ds;
            if (!SQL.Selects(SQL.queryConn, sqlcmd, out ds))
            {
                return null;
            }

            var tmpSet = new MaterialtableDataSet();
            var dt1 = ds.Tables[0];
            var dt2 = ds.Tables[1];
            ds.Tables.Remove(dt1);
            ds.Tables.Remove(dt2);
            dt1.TableName = "PO";
            dt2.TableName = "Document";
            tmpSet.Tables.Add(dt1);
            tmpSet.Tables.Add(dt2);

            tmpSet.RelationDocument = new DataRelation(
             "DocSeq",
             new DataColumn[] { tmpSet.PO.Columns["POID"], tmpSet.PO.Columns["seq"] },
             new DataColumn[] { tmpSet.Document.Columns["POID"], tmpSet.Document.Columns["seq"] });
            return tmpSet;
        }

        private void Find()
        {
            if (this.materialSet != null)
            {
                this.bs_Material.DataSource = null;
                this.bs_Document.DataSource = null;
                this.bs_Report.DataSource = null;
                this.materialSet.Dispose();
                this.materialSet = null;
            }

            var dataLoaded = this.LoadMaterialtableData();

            if (dataLoaded == null)
            {
                return;
            }

            if (dataLoaded.PO.Rows.Count == 0)
            {
                this.bs_Material.DataSource = null;
                this.bs_Document.DataSource = null;
                this.bs_Report.DataSource = null;
            }
            else
            {
                this.materialSet2 = dataLoaded;
                this.materialSet = dataLoaded;
                this.bs_Material.DataMember = "PO";
                this.bs_Material.DataSource = dataLoaded;
                this.bs_Document.DataMember = dataLoaded.RelationDocument.RelationName;
                this.bs_Document.DataSource = this.bs_Material;
                this.bs_Document_PositionChanged(null, null);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.bs_Report.DataSource == null)
            {
                return;
            }

            DataTable dt = (DataTable)this.bs_Report.DataSource;
            DualResult result;
            string updSql = string.Empty;

            var row = this.grid_Document.GetDataRow(this.bs_Document.Position);
            if (dt.Rows.Count > 0)
            {
                switch (row["FileRule"].ToString())
                {
                    case "1":
                    case "2":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from UASentReport t
inner join #tmp s on s.Ukey = t.Ukey
";
                        break;
                    case "3":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from FirstDyelot t
inner join #tmp s on s.Ukey = t.Ukey
";
                        break;
                    case "4":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from NewSentReport t
inner join #tmp s on s.Ukey = t.Ukey
";
                        break;
                    case "5":
                        updSql = $@"
update t
set FTYReceivedReport = s.FTYReceivedReport
from ExportRefnoSentReport t
inner join #tmp s on s.Ukey = t.Ukey
";
                        break;
                    default:
                        break;
                }

                result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, updSql, out DataTable odt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            this.Find();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private string DocumentName;

        private void bs_Document_PositionChanged(object sender, EventArgs e)
        {
            if (this.bs_Document.Current == null)
            {
                this.bs_Report.DataSource = null;
                return;
            }

            if (this.bs_Document.Position == -1)
            {
                this.bs_Report.DataSource = null;
                return;
            }

            var row = this.grid_Document.GetDataRow(this.bs_Document.Position);
            if (row == null)
            {
                this.DocumentName = string.Empty;
                this.bs_Report.DataSource = null;
                return;
            }

            var mainrow = this.grid_Material.GetDataRow(this.bs_Material.Position);
            if (mainrow == null)
            {
                this.bs_Report.DataSource = null;
                return;
            }

            this.SetupGrid_Report(row["FileRule"].ToString());
            List<SqlParameter> parmes = new List<SqlParameter>();
            string sql = string.Empty;
            switch (row["FileRule"].ToString())
            {
                case "1":
                    sql = @"
Select sr.Ukey  
       ,TestReportTestDate = CONVERT(VARCHAR(10), sr.TestReportTestDate, 23)
	   ,TestReport = CONVERT(VARCHAR(10), sr.TestReport, 23) 
       ,TestSeasonID
       ,DueSeason
       ,DueDate
       ,FTYReceivedReport
       ,sr.AddName
       ,sr.AddDate
       ,sr.EditName
       ,sr.EditDate
       ,FileRule = '1'
FROM UASentReport sr
WHERE  sr.SuppID in (select top 1 SuppGroup FROM BrandRelation where SuppID = @SuppID)  
and (sr.BrandRefno = @BrandRefno  or sr.BrandRefno = @Refno)
and sr.BrandID = @BrandID and sr.DocumentName = @DocumentName
                    ";
                    parmes.Add(new SqlParameter("@SuppID", mainrow["SuppID"]));
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@BrandID", mainrow["BrandID"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    break;
                case "2":
                    sql = @"
Select sr.Ukey  
       ,TestReportTestDate = CONVERT(VARCHAR(10), sr.TestReportTestDate, 23)
	   ,TestReport = CONVERT(VARCHAR(10), sr.TestReport, 23) 
       ,TestSeasonID
       ,DueSeason
       ,DueDate
       ,FTYReceivedReport
       ,sr.AddName
       ,sr.AddDate
       ,sr.EditName
       ,sr.EditDate
       ,FileRule = '2'
FROM UASentReport sr
WHERE  sr.SuppID in (select top 1 SuppGroup FROM BrandRelation where SuppID = @SuppID)  
and (sr.BrandRefno = @BrandRefno  or sr.BrandRefno = @Refno)
and sr.ColorID = @ColorID and sr.BrandID = @BrandID and sr.DocumentName = @DocumentName 
                    ";
                    parmes.Add(new SqlParameter("@SuppID", mainrow["SuppID"]));
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@BrandID", mainrow["BrandID"]));
                    parmes.Add(new SqlParameter("@ColorID", mainrow["Color"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    break;
                case "3":
                    sql = @"
Select DocSeason = SeasonID
       ,Period 
       ,TestDocFactoryGroup
       ,FTYReceivedReport
       ,ReceivedRemark
       ,FirstDyelot
       ,AddName
       ,AddDate
       ,EditName
       ,EditDate
       ,FileRule = '3'
FROM dbo.FirstDyelot 
WHERE SuppID in (select top 1 SuppGroup FROM BrandRelation where SuppID = @SuppID) 
and (BrandRefno = @BrandRefno  or BrandRefno = @Refno)
and ColorID = @ColorID and BrandID = @BrandID and DocumentName = @DocumentName
Order by SeasonID desc";
                    parmes.Add(new SqlParameter("@SuppID", mainrow["SuppID"]));
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@BrandID", mainrow["BrandID"]));
                    parmes.Add(new SqlParameter("@ColorID", mainrow["Color"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    break;
                case "4":
                    sql = @"
Select Ukey
       ,ReportDate = CONVERT(VARCHAR(10), ReportDate, 23)
       ,AWBNO 
       ,ExportID
       ,FTYReceivedReport
       ,AddName
       ,AddDate
       ,EditName
       ,EditDate
       ,FileRule = '4'
FROM NewSentReport 
WHERE PoID = @PoID and Seq1 = @Seq1 and Seq2 = @Seq2 and BrandID = @BrandID and DocumentName = @DocumentName
                    ";
                    parmes.Add(new SqlParameter("@POID", mainrow["POID"]));
                    parmes.Add(new SqlParameter("@Seq1", mainrow["Seq1"]));
                    parmes.Add(new SqlParameter("@Seq2", mainrow["Seq2"]));
                    parmes.Add(new SqlParameter("@BrandID", mainrow["BrandID"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    break;
                case "5":
                    sql = @"
Select  sr.Ukey
       ,ReportDate = CONVERT(VARCHAR(10), sr.ReportDate, 23)
       ,sr.AWBNO 
       ,FTYReceivedReport
       ,sr.ExportID
       ,sr.AddName
       ,sr.AddDate
       ,sr.EditName
       ,sr.EditDate
       ,FileRule = '5'
FROM ExportRefnoSentReport sr
INNER JOIN Export e on sr.ExportID = e.ID
WHERE sr.ColorID = @ColorID 
and (sr.BrandRefno = @BrandRefno  or sr.BrandRefno = @Refno)
and sr.BrandID = @BrandID and sr.DocumentName = @DocumentName
and exists(select 1 FROM Export_Detail ed WITH (NOLOCK) inner join Fabric f2 WITH (NOLOCK) on f2.SCIRefno = ed.SCIRefNo where ed.ID = e.ID and ed.POID = @POID and f2.BrandRefno = sr.BrandRefno)
                    ";
                    parmes.Add(new SqlParameter("@BrandRefno", mainrow["BrandRefno"]));
                    parmes.Add(new SqlParameter("@Refno", mainrow["Refno"]));
                    parmes.Add(new SqlParameter("@POID", mainrow["POID"]));
                    parmes.Add(new SqlParameter("@ColorID", mainrow["Color"]));
                    parmes.Add(new SqlParameter("@BrandID", mainrow["BrandID"]));
                    parmes.Add(new SqlParameter("@DocumentName", row["DocumentName"]));
                    break;
            }

            DataTable dt;
            if (!SQL.Select(string.Empty, sql, out dt, parmes))
            {
                return;
            }

            this.bs_Report.DataSource = dt;
        }
    }

    public class MaterialtableDataSet : DataSet
    {
        private DataRelation _RelationDocument = null;

        /// <summary>
        /// 由採購資料找DocumentName
        /// </summary>
        public DataRelation RelationDocument
        {
            get
            {
                return this._RelationDocument;
            }

            set
            {
                this._RelationDocument = value;
                this.Relations.Add(value);
            }
        }

        private DataRelation _RelationDocumentReport;

        /// <summary>
        /// 從採購資料找ReportData
        /// </summary>
        public DataRelation RelationDocumentReport
        {
            get
            {
                return this._RelationDocumentReport;
            }

            set
            {
                this._RelationDocumentReport = value;
                this.Relations.Add(value);
            }
        }

        /// <inheritdoc />
        public DataTable PO
        {
            get
            {
                return this.Tables["PO"];
            }
        }

        /// <inheritdoc />
        public DataTable Document
        {
            get
            {
                return this.Tables["Document"];
            }
        }

        /// <inheritdoc />
        public DataTable Report
        {
            get
            {
                return this.Tables["Report"];
            }
        }

        /// <summary>
        /// use po data to lookup dataRow
        /// </summary>
        public ILookup<string, DataRow> LookupMaterialDataRow { get; set; } = null;
    }
}
