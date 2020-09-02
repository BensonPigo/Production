using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Sci.DB;

namespace Sci.Production.Quality
{
    public partial class P09 : Win.Tems.QueryForm
    {
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            Env.Cfg.FtpServerIP = "ftp.sportscity.com.tw";
            Env.Cfg.FtpServerAccount = "insp_rpt";
            Env.Cfg.FtpServerPassword = "rpt_insp";
            this.displayBoxapvSeasonNull.BackColor = Color.FromArgb(190, 190, 190);
            this.displayBox1.BackColor = Color.Yellow;
        }

        private DataTable dt1;
        private DataTable dt2;
        private readonly string Filepath = @"MMC\";
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ApprovedSeason;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_FirstDyelot;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_TestReportCheckClima;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            #region tabPage1
            #region settings Event

            DataGridViewGeneratorTextColumnSettings Refno = new DataGridViewGeneratorTextColumnSettings();
            Refno.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Convert.GetInt(dr["bitRefnoColor"]) == 1)
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }
            };
            DataGridViewGeneratorDateColumnSettings Inspection = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorDateColumnSettings Test = new DataGridViewGeneratorDateColumnSettings();
            Inspection.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!MyUtility.Check.Empty(e.Value))
                {
                    e.CellStyle.Font = new Font("Ariel", 10, FontStyle.Underline);
                    e.CellStyle.ForeColor = Color.Blue;
                }
            };
            Test.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!MyUtility.Check.Empty(e.Value))
                {
                    e.CellStyle.Font = new Font("Ariel", 10, FontStyle.Underline);
                    e.CellStyle.ForeColor = Color.Blue;
                }
            };

            // 帶出grade
            DataGridViewGeneratorNumericColumnSettings T2IY = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings T2DP = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings col_CheckClima = new DataGridViewGeneratorCheckBoxColumnSettings();

            T2IY.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                dr["T2InspYds"] = e.FormattedValue;
                dr.EndEdit();
                this.T2Validating(s, e);
            };
            T2DP.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                dr["T2DefectPoint"] = e.FormattedValue;
                dr.EndEdit();
                this.T2Validating(s, e);
            };

            col_CheckClima.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["Clima"]))
                {
                    e.IsEditable = false;
                }
            };
            #endregion settings Event
            #region Set_grid1 Columns
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("selected", header: string.Empty, trueValue: 1, falseValue: 0, iseditable: true)
            .Text("ID", header: "WK#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("InvoiceNo", header: "Invoice#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Date("WhseArrival", header: "ATA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("ETA", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("PoID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("seq", header: "Seq#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("AbbEN", header: "Supp Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: Refno)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Date("InspectionReport", header: "Inspection Report\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPEInspectionReport", header: "Inspection Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: Inspection)
            .Date("TestReport", header: "Test Report\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .CheckBox("TestReportCheckClima", header: "Test Report\r\n Check Clima", trueValue: 1, falseValue: 0, iseditable: true, settings: col_CheckClima).Get(out this.col_TestReportCheckClima)
            .Date("TPETestReport", header: "Test Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: Test)
            .Date("ContinuityCard", header: "Continuity Card\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPEContinuityCard", header: "Continuity Card\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("AWBNo", header: "Continuity Card\r\nAWB#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Date("FirstDyelot", header: "1st Bulk Dyelot\r\nFty Received Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("TPEFirstDyelot", header: "1st Bulk Dyelot\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("T2InspYds", header: "T2 Inspected Yards", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: T2IY) // W
            .Numeric("T2DefectPoint", header: "T2 Defect Points", width: Widths.AnsiChars(8), integer_places: 5, settings: T2DP) // W
            .Text("T2Grade", header: "Grade", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("T1InspectedYards", header: "T1 Inspected Yards", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("T1DefectPoints", header: "T1 Defect Points", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;
            #endregion Set_grid1 Columns
            #region Color
            this.grid1.Columns["InspectionReport"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["TestReport"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["ContinuityCard"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["T2InspYds"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["T2DefectPoint"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion Color
            #endregion tabPage1_grid1
            #region tabPage2
            #region settings Event
            DataGridViewGeneratorDateColumnSettings FirstDyelot = new DataGridViewGeneratorDateColumnSettings();

            FirstDyelot.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.grid2.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["SeasonSCIID"]) && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    MyUtility.Msg.WarningBox("Approved Seanson is empty, you can't enter the data , please reference 1st Bulk Dyelot Supp Sent Date column information。");
                    dr["FirstDyelot"] = DBNull.Value;
                    dr.EndEdit();
                }
            };

            #endregion
            #region Set_grid2 Columns
            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("AbbEN", header: "Supp Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SeasonSCIID", header: "Approved Season", width: Widths.AnsiChars(8), iseditingreadonly: true).Get(out this.col_ApprovedSeason)
            .Numeric("Period", header: "Period", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Date("FirstDyelot", header: "1st Bulk Dyelot\r\nFty Received Date", width: Widths.AnsiChars(10), settings: FirstDyelot).Get(out this.col_FirstDyelot) // W (Pink)
            .Text("TPEFirstDyelot", header: "1st Bulk Dyelot\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
            #endregion Set_grid2 Columns
            #region Color
            this.grid2.Columns["FirstDyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.Change_Color();
            this.grid2.RowEnter += this.Grid2_RowEnter;
            #endregion Color
            #endregion tabPage2
        }

        private void Grid2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var data = ((DataRowView)this.grid2.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            bool canEdit = this.CheckPage2_Row_CanEdit(data["TPEFirstDyelot"].ToString());

            if (canEdit)
            {
                this.col_FirstDyelot.IsEditingReadOnly = false;
            }
            else
            {
                this.col_FirstDyelot.IsEditingReadOnly = true;
            }
        }

        private void Change_Color()
        {
            this.col_ApprovedSeason.CellFormatting += (s, e) =>
             {
                 if (e.RowIndex == -1)
                 {
                     return;
                 }

                 DataRow dr = this.grid2.GetDataRow(e.RowIndex);
                 if (dr == null)
                 {
                     return;
                 }

                 bool canEdit = this.CheckPage2_Row_CanEdit(dr["TPEFirstDyelot"].ToString());
                 if (!canEdit)
                 {
                     this.grid2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                 }
             };

            this.col_TestReportCheckClima.CellFormatting += (s, e) =>
           {
               if (e.RowIndex == -1)
               {
                   return;
               }

               DataRow dr = this.grid1.GetDataRow(e.RowIndex);
               if (dr == null)
               {
                   return;
               }

               if (MyUtility.Check.Empty(dr["Clima"]))
               {
                   e.CellStyle.BackColor = Color.White;
               }
               else
               {
                   e.CellStyle.BackColor = Color.Pink;
               }
           };
        }
        #region Tab_Page1

        private bool CheckPage2_Row_CanEdit(string tPEFirstDyelot)
        {
            if (MyUtility.Check.Empty(tPEFirstDyelot) ||
               tPEFirstDyelot.Equals("RIB no need frist dye lot"))
            {
                return false;
            }

            return true;
        }

        private string Filename(DataRow dr, string type)
        {
            List<string> cols = new List<string>();
            if (!MyUtility.Check.Empty(dr["ID"]))
            {
                cols.Add(MyUtility.Convert.GetString(dr["ID"]));
            }

            if (!MyUtility.Check.Empty(dr["PoID"]))
            {
                cols.Add(MyUtility.Convert.GetString(dr["PoID"]));
            }

            if (!MyUtility.Check.Empty(dr["seq"]))
            {
                cols.Add(MyUtility.Convert.GetString(dr["seq"]));
            }

            if (!MyUtility.Check.Empty(dr["Refno"]))
            {
                cols.Add(MyUtility.Convert.GetString(dr["Refno"]));
            }

            if (!MyUtility.Check.Empty(dr["ColorID"]))
            {
                cols.Add(MyUtility.Convert.GetString(dr["ColorID"]));
            }

            string fp = string.Join("-", cols) + type;
            return fp;
        }

        // 子目錄
        private string Filedic(DataRow dr)
        {
            string fp = this.Filepath + MyUtility.Convert.GetString(dr["AbbEN"]) + " " + MyUtility.Convert.GetString(dr["SuppID"]) + @"\";
            return fp;
        }

        private void T2Validating(object s, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
            if (dr == null)
            {
                return;
            }

            decimal PointRate = 0;
            if (MyUtility.Convert.GetDecimal(dr["T2InspYds"]).EqualDecimal(0))
            {
                PointRate = 0;
            }
            else
            {
                PointRate = MyUtility.Convert.GetDecimal(dr["T2DefectPoint"]) / MyUtility.Convert.GetDecimal(dr["T2InspYds"]) * 100;
            }

            string BrandID = dr["BrandID"].ToString();

            string sqlWEAVETYPEID = $@"

---- 1. 取得預設的布種的等級
SELECT [Grade]=MIN(Grade)
INTO #default
FROM FIR_Grade f WITH (NOLOCK) 
WHERE f.WeaveTypeID= (
	SELECT WeaveTypeId 
	FROM Fabric F
	LEFT JOIN PO_Supp_Detail  PSD ON PSD.SCIRefno = F.SCIRefno
	WHERE PSD.ID='{dr["poid"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'
) 
AND PERCENTAGE >= IIF({PointRate} > 100, 100, {PointRate} )
AND BrandID=''

---- 2. 取得該品牌布種的等級
SELECT [Grade]=MIN(Grade)
INTO #withBrandID
FROM FIR_Grade f WITH (NOLOCK) 
WHERE f.WeaveTypeID= (
	SELECT WeaveTypeId 
	FROM Fabric F
	LEFT JOIN PO_Supp_Detail  PSD ON PSD.SCIRefno = F.SCIRefno
	WHERE PSD.ID='{dr["poid"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'
) 
AND PERCENTAGE >= IIF({PointRate} > 100, 100, {PointRate} )
AND BrandID='{BrandID}'

---- 若該品牌有另外設定等級，就用該設定，不然用預設（主索引鍵是WeaveTypeID + Percentage + BrandID，因此不會找到多筆預設的Grade）
SELECT ISNULL(Brand.Grade, ISNULL((SELECT Grade FROM #default),'') ) 
FROM #withBrandID brand

DROP TABLE #default,#withBrandID

";

            dr["T2Grade"] = MyUtility.GetValue.Lookup(sqlWEAVETYPEID);
            dr.EndEdit();
        }

        private void Page1_Query()
        {
            // 檢查[表頭][ETA+SP#+PO#] 如果全為空請跳出訊息並return
            if (MyUtility.Check.Empty(this.dateATA.Value1) && MyUtility.Check.Empty(this.dateATA.Value2) && MyUtility.Check.Empty(this.dateRangeETA.Value1) && MyUtility.Check.Empty(this.dateRangeETA.Value1) && MyUtility.Check.Empty(this.txtsp.Text) && MyUtility.Check.Empty(this.txtpo.Text))
            {
                MyUtility.Msg.WarningBox("Please select ATA or ETA or SP# or PO# at least one field entry.");
                return;
            }

            this.listControlBindingSource1.DataSource = null;

            #region Where
            string sqlwhere = string.Empty;
            List<string> sqlwheres = new List<string>();
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            if (!MyUtility.Check.Empty(this.dateRangeETA.Value1) && !MyUtility.Check.Empty(this.dateRangeETA.Value2))
            {
                listSQLParameter.Add(new SqlParameter("@ETA1", this.dateRangeETA.Value1));
                listSQLParameter.Add(new SqlParameter("@ETA2", this.dateRangeETA.Value2));
                sqlwheres.Add(" Export.ETA between @ETA1 and @ETA2 ");
            }

            if (!MyUtility.Check.Empty(this.dateATA.Value1) && !MyUtility.Check.Empty(this.dateATA.Value2))
            {
                listSQLParameter.Add(new SqlParameter("@ATA1", this.dateATA.Value1));
                listSQLParameter.Add(new SqlParameter("@ATA2", this.dateATA.Value2));
                sqlwheres.Add(" Export.WhseArrival between @ATA1 and @ATA2 ");
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                listSQLParameter.Add(new SqlParameter("@sp", this.txtsp.Text));
                sqlwheres.Add(" ed.PoID = @sp ");
            }

            if (!MyUtility.Check.Empty(this.txtSeq.Seq1))
            {
                listSQLParameter.Add(new SqlParameter("@seq1", this.txtSeq.Seq1));
                sqlwheres.Add(" ed.Seq1 = @seq1 ");
            }

            if (!MyUtility.Check.Empty(this.txtSeq.Seq2))
            {
                listSQLParameter.Add(new SqlParameter("@seq2", this.txtSeq.Seq2));
                sqlwheres.Add(" ed.Seq2 = @seq2 ");
            }

            if (!MyUtility.Check.Empty(this.txtpo.Text))
            {
                listSQLParameter.Add(new SqlParameter("@po", this.txtpo.Text));
                sqlwheres.Add(" o.CustPONo = @po ");
            }

            if (sqlwheres.Count > 0)
            {
                sqlwhere = "where " + string.Join(" and ", sqlwheres);
            }
            #endregion Where

            #region Sqlcmd
            string sqlcmd = $@"
Select RowNo = ROW_NUMBER() OVER(ORDER by Month), ID 
Into #probablySeasonList
From SeasonSCI

select distinct
    selected = cast(0 as bit),
	FileExistI= cast(0 as bit),
	FileExistT= cast(0 as bit),
	ed.id,
    ed.InvoiceNo,
	Export.ETA,
    Export.WhseArrival,
	ed.PoID,
	seq=ed.seq1+'-'+ed.seq2,
	ps.SuppID,
	Supp.AbbEN,
	psd.Refno,
	psd.ColorID,
	Qty = isnull(ed.Qty,0) + isnull(ed.Foc,0),
	sr.InspectionReport,
	sr.TPEInspectionReport,
	sr.TestReport,
    sr.TestReportCheckClima,
	sr.TPETestReport,
	sr.ContinuityCard,
	sr.TPEContinuityCard,
	FirstDyelot.FirstDyelot,
	TPEFirstDyelot = IIF(FirstDyelot.TPEFirstDyelot is null and f.RibItem = 1
                        ,'RIB no need first dye lot'
                        ,IIF(FirstDyelot.SeasonSCIID is null
                                ,'Still not received and under pushing T2. Please contact with PR if you need L/G first.'
                                ,format(FirstDyelot.TPEFirstDyelot,'yyyy/MM/dd')
                            )
                    ),
	sr.T2InspYds,
	sr.T2DefectPoint,
	sr.T2Grade,
	a.T1InspectedYards,
	b.T1DefectPoints,
    ed.seq1,
    ed.seq2,
	ed.Ukey,
    o.BrandID,
    f.Clima,
    sr.AWBNo,
    [bitRefnoColor] = case when f.Clima = 1 then ROW_NUMBER() over(partition by f.Clima, ps.SuppID, psd.Refno, psd.ColorID, Format(Export.CloseDate,'yyyyMM') order by Export.CloseDate) else 0 end
from Export_Detail ed with(nolock)
inner join Export with(nolock) on Export.id = ed.id and Export.Confirm = 1
inner join orders o with(nolock) on o.id = ed.PoID
left join SentReport sr with(nolock) on sr.Export_DetailUkey = ed.Ukey
left join Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
left join PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
left join Supp with(nolock) on Supp.ID = ps.SuppID
left join Season s with(nolock) on s.ID=o.SeasonID and s.BrandID = o.BrandID
left join Factory fty with (nolock) on fty.ID = Export.Consignee
left join Fabric f with(nolock) on f.SCIRefno =psd.SCIRefno
Left join #probablySeasonList seasonSCI on seasonSCI.ID = s.SeasonSCIID
OUTER APPLY(
	Select Top 1 FirstDyelot,TPEFirstDyelot,SeasonSCIID
	From dbo.FirstDyelot fd
	Inner join #probablySeasonList season on fd.SeasonSCIID = season.ID
	WHERE fd.Refno = psd.Refno and fd.ColorID = psd.ColorID and fd.SuppID = ps.SuppID and fd.TestDocFactoryGroup = fty.TestDocFactoryGroup
		And seasonSCI.RowNo >= season.RowNo
	Order by season.RowNo Desc
)FirstDyelot
outer apply(
	select T1InspectedYards=sum(fp.ActualYds)
	from fir f
	left join FIR_Physical fp on fp.id=f.id
	left join Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)a
outer apply(
	select  T1DefectPoints = sum(fp.TotalPoint)
	from fir f
	left join FIR_Physical fp on fp.id=f.id
	left join Receiving r on r.id= f.ReceivingID
	where r.InvNo=ed.ID and f.POID=ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 =ed.Seq2
)b
{sqlwhere}
and psd.FabricType = 'F'
and (ed.qty + ed.Foc)>0
and o.Category in('B','M')

order by ed.id,ed.PoID,ed.Seq1,ed.Seq2

DROP TABLE #probablySeasonList
";
            #endregion Sqlcmd
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listSQLParameter, out this.dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.dt1.AcceptChanges();
            this.listControlBindingSource1.DataSource = this.dt1;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Page1_Query();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            if (this.dt1 == null || this.dt1.Rows.Count == 0)
            {
                return;
            }

            if (this.dt1.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).ToList().Count == 0)
            {
                MyUtility.Msg.WarningBox("No data changes.");
                return;
            }

            DataTable changedt = this.dt1.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).CopyToDataTable();

            string sqlupdate = $@"
merge SentReport t
using #tmp s
on t.Export_DetailUkey = s.ukey
when matched then update set 
	t.InspectionReport=s.InspectionReport,
	t.TestReport=s.TestReport,
	t.ContinuityCard=s.ContinuityCard,
	t.T2InspYds=isnull(s.T2InspYds,0),
	t.T2DefectPoint=isnull(s.T2DefectPoint,0),
	t.T2Grade=isnull(s.T2Grade,''),
    t.EditName='{Env.User.UserID}',
    t.EditDate = getdate()	,
    t.TestReportCheckClima = isnull(s.TestReportCheckClima,0)
when not matched by target then 
insert([Export_DetailUkey]
,[InspectionReport],[TestReport],[ContinuityCard],[T2InspYds],[T2DefectPoint],[T2Grade],[EditName],[EditDate],TestReportCheckClima)
VALUES(s.ukey,s.InspectionReport,s.TestReport,s.ContinuityCard,isnull(s.T2InspYds,0),isnull(s.T2DefectPoint,0),isnull(s.T2Grade,''),'{Env.User.UserID}',getdate(), isnull(s.TestReportCheckClima,0))
;
";
            DataTable odt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(changedt, string.Empty, sqlupdate, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Success!");
            this.Page1_Query();
        }

        readonly TransferPms transferPMS = new TransferPms();

        private void BtnDownloadFile_Click(object sender, EventArgs e)
        {
            DualResult result;
            IList<MyUtility.FTP.FtpFile> ftpDir = new List<MyUtility.FTP.FtpFile>();
            result = MyUtility.FTP.FTP_GetFileList(this.Filepath, out ftpDir); // 確認根目錄能正常取得
            if (!result)
            {
                MyUtility.Msg.WarningBox("For ftp://ftp.sportscity.con.tw no access, Please find Local IT assistance to open.");
                return;
            }

            if (this.dt1 == null || this.dt1.Select("selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("No datas selected.");
                return;
            }

            this.contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
        }

        private void Savefile(string type)
        {
            this.grid1.ValidateControl();

            // SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            // saveFileDialog1.Filter = "All Files|*";
            // saveFileDialog1.Title = "Save File";
            // saveFileDialog1.FileName = "multiple files"+ type;
            // List<string> files = new List<string>();
            Dictionary<string, string> filesDic = new Dictionary<string, string>();
            DataRow[] drs = this.dt1.Select("selected = 1");
            foreach (DataRow dr in drs)
            {
                string filepath = this.Filedic(dr); // 取得根目錄+子目錄

                DualResult result;
                IList<string> ftpDir = new List<string>();
                result = MyUtility.FTP.FTP_GetFileList(filepath, out ftpDir);
                if (!result)
                {
                    continue;
                }

                if (ftpDir.Count > 0)
                {
                    string filename = this.Filename(dr, type);
                    string[] fs = ftpDir.Where(r => r.ToUpper().Contains(filename.ToUpper())).ToArray();
                    foreach (string item in fs)
                    {
                        filesDic.Add(item, filepath);
                    }
                }
            }

            if (filesDic.Count == 0)
            {
                MyUtility.Msg.WarningBox("No files exists.");
                return;
            }

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    DualResult dresult;
                    foreach (var file in filesDic)
                    {
                        dresult = MyUtility.FTP.FTP_Download(file.Value + @"\" + file.Key, fbd.SelectedPath + @"\" + file.Key);
                        if (!dresult)
                        {
                            this.ShowErr(dresult);
                        }
                    }
                }
            }

            MyUtility.Msg.InfoBox("Success");

            // if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            // {
            //    DualResult result;
            //    foreach (var file in filesDic)
            //    {
            //        result = MyUtility.FTP.FTP_Download(file.Value + @"\" + file.Key, Path.GetDirectoryName(saveFileDialog1.FileName) + @"\" + file.Key);
            //        if (!result)
            //        {
            //            this.ShowErr(result);
            //        }
            //    }
            // }
        }

        private void InspectionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Savefile("-inspection");
        }

        private void TestReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Savefile("-test");
        }
        #endregion Tab_Page1

        #region #Tab_Page2
        private void Page2_Query()
        {
            // 檢查[表頭][ETA+SP#+PO#] 如果全為空請跳出訊息並return
            if (MyUtility.Check.Empty(this.txtsupplier1.TextBox1.Text) && MyUtility.Check.Empty(this.txtRefno.Text) && MyUtility.Check.Empty(this.txtColor.Text))
            {
                MyUtility.Msg.WarningBox("Please select Supplier or Refno or Color at least one field entry.");
                return;
            }

            this.listControlBindingSource2.DataSource = null;
            #region Where
            string sqlwhere = string.Empty;
            List<string> sqlwheres = new List<string>();
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            if (!MyUtility.Check.Empty(this.txtsupplier1.TextBox1.Text))
            {
                listSQLParameter.Add(new SqlParameter("@SuppID", this.txtsupplier1.TextBox1.Text));
                sqlwheres.Add(" (a.SuppID = @SuppID or b.Suppid = @SuppID) ");
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                listSQLParameter.Add(new SqlParameter("@Refno", this.txtRefno.Text));
                sqlwheres.Add(" (a.Refno = @Refno or b.Refno = @Refno)");
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                listSQLParameter.Add(new SqlParameter("@ColorID", this.txtColor.Text));
                sqlwheres.Add(" (a.ColorID = @ColorID or b.ColorID = @ColorID) ");
            }

            if (sqlwheres.Count > 0)
            {
                sqlwhere = string.Join(" and ", sqlwheres);
            }
            #endregion Where
            #region Sqlcmd
            string sqlcmd = $@"
select distinct
    Export.Consignee,
	ps.SuppID,
	Supp.AbbEN,
	psd.Refno,
	psd.ColorID,
	o.SeasonID,
    fd.SeasonSCIID,
    fd.Period,
	fd.FirstDyelot  FirstDyelot,
	TPEFirstDyelot = IIF(fd.TPEFirstDyelot is null and RibItem = 1
                    ,'RIB no need first dye lot'
                    ,IIF(fd.SeasonSCIID is null
                            ,'Still not received and under pushing T2. Please contact with PR if you need L/G first.'
                            ,format(fd.TPEFirstDyelot,'yyyy/MM/dd')
                        )
                )
into #tmp
from Export_Detail ed with(nolock)
inner join Export with(nolock) on Export.id = ed.id and Export.Confirm = 1
inner join orders o with(nolock) on o.id = ed.PoID
left join Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
left join PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
left join Supp with(nolock) on Supp.ID = ps.SuppID
left join Season s with(nolock) on s.ID=o.SeasonID and s.BrandID = o.BrandID
left join Factory fty with (nolock) on fty.ID = Export.Consignee
left outer join FirstDyelot fd with(nolock) on fd.Refno = psd.Refno and fd.ColorID = psd.ColorID and fd.SuppID = ps.SuppID and fd.TestDocFactoryGroup = fty.TestDocFactoryGroup 
left join Fabric f with(nolock) on f.SCIRefno =psd.SCIRefno
where   ps.seq1 not like '7%' 
and psd.FabricType = 'F'
and (ed.qty + ed.Foc)>0
and o.Category in('B','M')

select 
fty.TestDocFactoryGroup
,[suppid] = iif(a.SuppID is null, b.SuppID,a.Suppid)
,[AbbEN] = iif(a.AbbEN is null, (select abben from supp where id=b.suppid), a.abben)
,[Refno] = iif(a.Refno is null ,b.Refno,a.refno)
,[ColorID] = iif(a.ColorID is null , b.ColorID, a.colorid)
,[SeasonID] = a.SeasonID
,[SeasonSCIID] = iif(a.SeasonSCIID is null,b.SeasonSCIID,a.SeasonSCIID)
,[Period] = iif(a.Period is null, b.Period , a.Period)
,[FirstDyelot] = iif(a.FirstDyelot is null, b.FirstDyelot, a.FirstDyelot)
,a.[TPEFirstDyelot]
from #tmp a
inner join Factory fty with (nolock) on fty.ID = a.Consignee
full join FirstDyelot b on fty.TestDocFactoryGroup = b.TestDocFactoryGroup
and a.Refno=b.Refno and a.suppid=b.suppid and a.colorid=b.ColorID 
where 1=1 and 
{sqlwhere}
Order by  SuppID, Refno, ColorID, a.SeasonSCIID

drop table #tmp
";
            #endregion Sqlcmd
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listSQLParameter, out this.dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dt2.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found.");
            }

            this.listControlBindingSource2.DataSource = this.dt2;
            this.dt2.AcceptChanges();
            this.grid2.AutoResizeColumns();
        }

        private void BtnQuery2_Click(object sender, EventArgs e)
        {
            this.Page2_Query();
        }

        private void BtnSave2_Click(object sender, EventArgs e)
        {
            this.grid2.ValidateControl();
            if (this.dt2 == null || this.dt2.Rows.Count == 0)
            {
                return;
            }

            if (this.dt2.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).ToList().Count == 0)
            {
                MyUtility.Msg.WarningBox("No data changes.");
                return;
            }

            DataTable changedt = this.dt2.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).Distinct().CopyToDataTable();
            string sqlupdate = $@"
update t
	set FirstDyelot= s.FirstDyelot,
		EditName = '{Env.User.UserID}',
		EditDate = GETDATE()
from FirstDyelot t
inner join
(
	select TestDocFactoryGroup, Suppid, Refno, ColorID, SeasonSCIID, max(convert(date,FirstDyelot)) as FirstDyelot 
	from #tmp
	group by  TestDocFactoryGroup,Suppid,Refno,ColorID,SeasonSCIID
) s
on t.Refno = s.Refno and t.SuppID = s.SuppID and t.ColorID = s.ColorID and t.TestDocFactoryGroup = s.TestDocFactoryGroup and t.SeasonSCIID = s.SeasonSCIID
;
";
            DataTable odt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(changedt, string.Empty, sqlupdate, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Success!");
            this.Page2_Query();
        }
        #endregion#Tab_Page2

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DataRow drSystem;
            if (MyUtility.Check.Seek("select * from system", out drSystem))
            {
                Env.Cfg.FtpServerIP = drSystem["FtpIP"].ToString().Trim();
                Env.Cfg.FtpServerAccount = drSystem["FtpID"].ToString().Trim();
                Env.Cfg.FtpServerPassword = drSystem["FtpPwd"].ToString().Trim();
            }

            this.Close();
        }
    }
}
