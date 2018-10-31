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
using System.Net;
using System.Net.NetworkInformation;
using System.IO;

namespace Sci.Production.Quality
{
    public partial class P09 : Sci.Win.Tems.QueryForm
    {
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        private DataTable dt1;
        private DataTable dt2;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            #region tabPage1
            #region settings Event
            // 帶出grade
            DataGridViewGeneratorNumericColumnSettings T2IY = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings T2DP = new DataGridViewGeneratorNumericColumnSettings();
            T2IY.CellValidating += (s, e) =>
            {
                T2Validating(s, e);
            };
            T2DP.CellValidating += (s, e) =>
            {
                T2Validating(s, e);
            };
            #endregion settings Event
            #region Set_grid1 Columns
            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("selected", header: "", trueValue: 1, falseValue: 0, iseditable: true)
            .Text("ID", header: "WK#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Date("LastEta", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("PoID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("seq", header: "Seq#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("AbbEN", header: "Supp Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Date("InspectionReport", header: "Inspection Report\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPEInspectionReport", header: "Inspection Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("TestReport", header: "Test Report\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPETestReport", header: "Test Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("ContinuityCard", header: "Continuity Card\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPEContinuityCard", header: "Continuity Card\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("FirstDyelot", header: "1st Bulk Dyelot\r\nFty Received Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("TPEFirstDyelot", header: "1st Bulk Dyelot\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("T2InspYds", header: "T2 Inspected Yards", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: T2IY) // W
            .Numeric("T2DefectPoint", header: "T2 Defect Points", width: Widths.AnsiChars(8), integer_places: 5, settings: T2DP) // W
            .Text("Grade", header: "Grade", width: Widths.AnsiChars(8), iseditingreadonly: true)
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
            #region Set_grid2 Columns
            this.grid2.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid2)
            .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("AbbEN", header: "Supp Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Date("FirstDyelot", header: "1st Bulk Dyelot\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPEFirstDyelot", header: "1st Bulk Dyelot\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
            #endregion Set_grid2 Columns
            #region Color
            this.grid2.Columns["FirstDyelot"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion Color
            #endregion tabPage2
        }
        #region Tab_Page1
        private void T2Validating(object s, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
            if (null == dr) return;
            decimal PointRate = MyUtility.Convert.GetDecimal(dr["T2DefectPoint"]) / MyUtility.Convert.GetDecimal(dr["T2InspYds"]) * 100;
            string sqlWEAVETYPEID = $@"
SELECT MIN(grade)
FROM FIR_Grade WITH (NOLOCK) 
WHERE WEAVETYPEID = (
	SELECT WeaveTypeId 
	FROM Fabric F
	LEFT JOIN PO_Supp_Detail  PSD ON PSD.SCIRefno = F.SCIRefno
	WHERE PSD.ID='{dr["id"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'
) 
AND PERCENTAGE >= IIF({PointRate} > 100, 100, {PointRate} )
";
            dr["Grade"] = MyUtility.GetValue.Lookup(sqlWEAVETYPEID);
            dr.EndEdit();
        }

        private void Page1_Query()
        {
            // 檢查[表頭][ETA+SP#+PO#] 如果全為空請跳出訊息並return
            if (MyUtility.Check.Empty(this.dateRange1.Value1) && MyUtility.Check.Empty(this.dateRange1.Value1) && MyUtility.Check.Empty(this.txtsp.Text) && MyUtility.Check.Empty(this.txtpo.Text))
            {
                MyUtility.Msg.WarningBox("Please select ETA or SP# or PO# at least one field entry.");
                return;
            }

            this.listControlBindingSource1.DataSource = null;

            #region Where
            string sqlwhere = string.Empty;
            List<string> sqlwheres = new List<string>();
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            if (!MyUtility.Check.Empty(this.dateRange1.Value1) && !MyUtility.Check.Empty(this.dateRange1.Value1))
            {
                listSQLParameter.Add(new SqlParameter("@ETA1", this.dateRange1.Value1));
                listSQLParameter.Add(new SqlParameter("@ETA2", this.dateRange1.Value2));
                sqlwheres.Add(" ed.LastEta between @ETA1 and @ETA2 ");
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                listSQLParameter.Add(new SqlParameter("@sp", this.txtsp.Text));
                sqlwheres.Add(" ed.PoID = @sp ");
            }

            if (!MyUtility.Check.Empty(this.txtSeq.seq1))
            {
                listSQLParameter.Add(new SqlParameter("@seq1", this.txtSeq.seq1));
                sqlwheres.Add(" ed.Seq1 = @seq1 ");
            }

            if (!MyUtility.Check.Empty(this.txtSeq.seq2))
            {
                listSQLParameter.Add(new SqlParameter("@seq2", this.txtSeq.seq2));
                sqlwheres.Add(" ed.Seq2 = @seq2 ");
            }

            if (!MyUtility.Check.Empty(this.txtpo.Text))
            {
                listSQLParameter.Add(new SqlParameter("@po", this.txtpo.Text));
                sqlwheres.Add(" and o.CustPONo = @po ");
            }

            if (sqlwheres.Count > 0)
            {
                sqlwhere = "where " + string.Join(" and ", sqlwheres);
            }
            #endregion Where

            #region Sqlcmd
            string sqlcmd = $@"
select selected = 0,
	ed.id,
	ed.LastEta,
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
	sr.TPETestReport,
	sr.ContinuityCard,
	sr.TPEContinuityCard,
	fd.FirstDyelot,
	fd.TPEFirstDyelot,
	sr.T2InspYds,
	sr.T2DefectPoint,
	sr.T2Grade,
	a.T1InspectedYards,
	b.T1DefectPoints,
	sr.Export_DetailUkey
from Export_Detail ed with(nolock)
left join Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
left join FirstDyelot fd with(nolock) on fd.SCIRefno = psd.SCIRefno
left join PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
left join orders o with(nolock) on o.id = ed.PoID
left join Supp with(nolock) on Supp.ID = ps.SuppID
left join SentReport sr with(nolock) on sr.Export_DetailUkey = ed.Ukey
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
order by ed.id,ed.PoID,ed.Seq1,ed.Seq2
";
            #endregion Sqlcmd
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listSQLParameter, out dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt1;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.Page1_Query();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            if (dt1 == null || dt1.Rows.Count == 0)
            {
                return;
            }

            if (dt1.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).ToList().Count == 0)
            {
                MyUtility.Msg.WarningBox("No data changes.");
                return;
            }

            DataTable changedt = dt1.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).CopyToDataTable();
            string sqlupdate = $@"
update t set
	InspectionReport=s.InspectionReport,
	TestReport=s.TestReport,
	ContinuityCard=s.ContinuityCard,
	T2InspYds=s.T2InspYds,
	T2DefectPoint=s.T2DefectPoint,
	T2Grade=s.T2Grade,
    EditName='{Sci.Env.User.UserID}',
    EditDate = getdate()
from SentReport t
inner join #tmp s on t.Export_DetailUkey = s.Export_DetailUkey
";
            DataTable odt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(changedt, string.Empty, sqlupdate, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            MyUtility.Msg.InfoBox("Success!");
            Page1_Query();
        }

        TransferPms transferPMS = new TransferPms();
        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"\\evamgr\ftp\FACTORY\TO-ALL\MMC\for testing only\"))
            {
                MyUtility.Msg.WarningBox("Please select ETA or SP# or PO# at least one field entry.");
                return;
            }

            if (File.Exists(@"Z:\PMS\NewPMS_2016\imp_ChangeKPILETARequest2.sql"))
            {

            }
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
                sqlwheres.Add(" fd.SuppID = @SuppID ");
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                listSQLParameter.Add(new SqlParameter("@Refno", this.txtRefno.Text));
                sqlwheres.Add(" psd.Refno = @Refno ");
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                listSQLParameter.Add(new SqlParameter("@ColorID", this.txtColor.Text));
                sqlwheres.Add(" fd.ColorID = @ColorID ");
            }

            if (sqlwheres.Count > 0)
            {
                sqlwhere = "where " + string.Join(" and ", sqlwheres);
            }
            #endregion Where
            #region Sqlcmd
            string sqlcmd = $@"
select
	fd.SuppID,
	Supp.AbbEN,
	psd.Refno,
	fd.ColorID,
	fd.FirstDyelot,
	fd.TPEFirstDyelot,
	fd.SCIRefno
from FirstDyelot fd with(nolock)
left join Supp with(nolock) on Supp.ID = fd.SuppID
left join Po_Supp_Detail psd with(nolock) on psd.SCIRefno = fd.SCIRefno
{sqlwhere}
order by fd.SuppID
";
            #endregion Sqlcmd
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listSQLParameter, out dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource2.DataSource = dt2;
        }

        private void btnQuery2_Click(object sender, EventArgs e)
        {
            Page2_Query();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            this.grid2.ValidateControl();
            if (dt1 == null || dt1.Rows.Count == 0)
            {
                return;
            }

            if (dt2.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).ToList().Count == 0)
            {
                MyUtility.Msg.WarningBox("No data changes.");
                return;
            }

            DataTable changedt = dt2.AsEnumerable().Where(r => r.RowState == DataRowState.Modified).CopyToDataTable();
            string sqlupdate = $@"
update t set 
	FirstDyelot=s.FirstDyelot,
	EditName = '{Sci.Env.User.UserID}',
	EditDate = GETDATE()
from FirstDyelot t
inner join #tmp s on t.SCIRefno = s.SCIRefno
";
            DataTable odt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(changedt, string.Empty, sqlupdate, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            MyUtility.Msg.InfoBox("Success!");
            Page2_Query();
        }
        #endregion#Tab_Page2

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
