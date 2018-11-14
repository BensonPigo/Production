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
using System.Configuration;

namespace Sci.Production.Quality
{
    public partial class P09 : Sci.Win.Tems.QueryForm
    {
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            Env.Cfg.FtpServerIP = "ftp.sportscity.com.tw";
            Env.Cfg.FtpServerAccount = "insp_rpt";
            Env.Cfg.FtpServerPassword = "rpt_insp";
        }

        private DataTable dt1;
        private DataTable dt2;
        private string Filepath = @"TO-ALL\MMC\for testing only\Jeff\";

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            #region tabPage1
            #region settings Event

            DataGridViewGeneratorDateColumnSettings Inspection = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorDateColumnSettings Test = new DataGridViewGeneratorDateColumnSettings();
            Inspection.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (!MyUtility.Check.Empty(e.Value))
                {
                    e.CellStyle.Font = new Font("Ariel", 10, FontStyle.Underline);
                    e.CellStyle.ForeColor = Color.Blue;
                }
            };
            Test.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (!MyUtility.Check.Empty(e.Value))
                {
                    e.CellStyle.Font = new Font("Ariel", 10, FontStyle.Underline);
                    e.CellStyle.ForeColor = Color.Blue;
                }
            };
            // 帶出grade
            DataGridViewGeneratorNumericColumnSettings T2IY = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings T2DP = new DataGridViewGeneratorNumericColumnSettings();
            T2IY.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                dr["T2InspYds"] = e.FormattedValue;
                dr.EndEdit();
                T2Validating(s, e);
            };
            T2DP.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                dr["T2DefectPoint"] = e.FormattedValue;
                dr.EndEdit();
                T2Validating(s, e);
            };
            #endregion settings Event
            #region Set_grid1 Columns
            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("selected", header: "", trueValue: 1, falseValue: 0, iseditable: true)
            .Text("ID", header: "WK#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Date("LastEta", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("PoID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("seq", header: "Seq#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("AbbEN", header: "Supp Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Date("InspectionReport", header: "Inspection Report\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPEInspectionReport", header: "Inspection Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: Inspection)
            .Date("TestReport", header: "Test Report\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPETestReport", header: "Test Report\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: Test)
            .Date("ContinuityCard", header: "Continuity Card\r\nFty Received Date", width: Widths.AnsiChars(10)) // W (Pink)
            .Date("TPEContinuityCard", header: "Continuity Card\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("FirstDyelot", header: "1st Bulk Dyelot\r\nFty Received Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("TPEFirstDyelot", header: "1st Bulk Dyelot\r\nSupp Sent Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
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

        private string Filename(DataRow dr, string type)
        {
            List<string> cols = new List<string>();
            if (!MyUtility.Check.Empty(dr["ID"])) cols.Add(MyUtility.Convert.GetString(dr["ID"]));
            if (!MyUtility.Check.Empty(dr["PoID"])) cols.Add(MyUtility.Convert.GetString(dr["PoID"]));
            if (!MyUtility.Check.Empty(dr["seq"])) cols.Add(MyUtility.Convert.GetString(dr["seq"]));
            if (!MyUtility.Check.Empty(dr["Refno"])) cols.Add(MyUtility.Convert.GetString(dr["Refno"]));
            if (!MyUtility.Check.Empty(dr["ColorID"])) cols.Add(MyUtility.Convert.GetString(dr["ColorID"]));

            string fp = string.Join("-", cols) + type;
            return fp;
        }

        // 子目錄
        private string Filedic(DataRow dr)
        {
            string fp = Filepath + MyUtility.Convert.GetString(dr["AbbEN"]) + " " + MyUtility.Convert.GetString(dr["SuppID"]) + @"\";
            return fp;
        }

        private void T2Validating(object s, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
            if (null == dr) return;
            decimal PointRate = 0;
            if (MyUtility.Convert.GetDecimal(dr["T2InspYds"]).EqualDecimal(0))
            {
                PointRate = 0;
            }
            else
            {
                PointRate = MyUtility.Convert.GetDecimal(dr["T2DefectPoint"]) / MyUtility.Convert.GetDecimal(dr["T2InspYds"]) * 100;
            }
            string sqlWEAVETYPEID = $@"
SELECT isnull(MIN(grade),'')
FROM FIR_Grade WITH (NOLOCK) 
WHERE WEAVETYPEID = (
	SELECT WeaveTypeId 
	FROM Fabric F
	LEFT JOIN PO_Supp_Detail  PSD ON PSD.SCIRefno = F.SCIRefno
	WHERE PSD.ID='{dr["poid"]}' AND PSD.SEQ1 ='{dr["seq1"]}' AND PSD.SEQ2 ='{dr["seq2"]}'
) 
AND PERCENTAGE >= IIF({PointRate} > 100, 100, {PointRate} )
";
            dr["T2Grade"] = MyUtility.GetValue.Lookup(sqlWEAVETYPEID);
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
                sqlwheres.Add(" o.CustPONo = @po ");
            }

            if (sqlwheres.Count > 0)
            {
                sqlwhere = "where " + string.Join(" and ", sqlwheres);
            }
            #endregion Where

            #region Sqlcmd
            string sqlcmd = $@"
select selected = cast(0 as bit),
	FileExistI= cast(0 as bit),
	FileExistT= cast(0 as bit),
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
    ed.seq1,
    ed.seq2,
	ed.Ukey
from Export_Detail ed with(nolock)
left join Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
left join PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
left join FirstDyelot fd with(nolock) on fd.Refno = psd.Refno and fd.ColorID = psd.ColorID and fd.SuppID = ps.SuppID
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
and psd.FabricType = 'F'
order by ed.id,ed.PoID,ed.Seq1,ed.Seq2
";
            #endregion Sqlcmd
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listSQLParameter, out dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            dt1.AcceptChanges();
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
    t.EditName='{Sci.Env.User.UserID}',
    t.EditDate = getdate()	
when not matched by target then 
insert([Export_DetailUkey]
,[InspectionReport],[TestReport],[ContinuityCard],[T2InspYds],[T2DefectPoint],[T2Grade],[EditName],[EditDate])
VALUES(s.ukey,s.InspectionReport,s.TestReport,s.ContinuityCard,isnull(s.T2InspYds,0),isnull(s.T2DefectPoint,0),isnull(s.T2Grade,''),'{Sci.Env.User.UserID}',getdate())
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
            Page1_Query();
        }

        TransferPms transferPMS = new TransferPms();
        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            DualResult result;
            IList<MyUtility.FTP.FtpFile> ftpDir = new List<MyUtility.FTP.FtpFile>();
            result = MyUtility.FTP.FTP_GetFileList(this.Filepath, out ftpDir); //確認根目錄能正常取得
            if (!result)
            {
                MyUtility.Msg.WarningBox("For ftp://ftp.sportscity.con.tw no access, Please find Local IT assistance to open.");
                return;
            }

            if (dt1==null || dt1.Select("selected = 1").Length ==0)
            {
                MyUtility.Msg.WarningBox("No datas selected.");
                return;
            }
            contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
        }

        private void Savefile(string type)
        {
            this.grid1.ValidateControl();
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.Filter = "All Files|*";
            //saveFileDialog1.Title = "Save File";
            //saveFileDialog1.FileName = "multiple files"+ type;
            //List<string> files = new List<string>();
            Dictionary<string, string> filesDic = new Dictionary<string, string>();
            DataRow[] drs = dt1.Select("selected = 1");
            foreach (DataRow dr in drs)
            {
                string filepath = Filedic(dr); // 取得根目錄+子目錄

                DualResult result;
                IList<string> ftpDir = new List<string>();
                result = MyUtility.FTP.FTP_GetFileList(filepath, out ftpDir);
                if (!result)
                {
                    continue;
                }

                if (ftpDir.Count > 0)
                {
                    string filename = Filename(dr, type);
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
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    DualResult result;
            //    foreach (var file in filesDic)
            //    {
            //        result = MyUtility.FTP.FTP_Download(file.Value + @"\" + file.Key, Path.GetDirectoryName(saveFileDialog1.FileName) + @"\" + file.Key);
            //        if (!result)
            //        {
            //            this.ShowErr(result);
            //        }
            //    }
            //}
        }

        private void inspectionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Savefile("-Inspection");
        }

        private void testReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Savefile("-test");
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
                sqlwheres.Add(" ps.SuppID = @SuppID ");
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                listSQLParameter.Add(new SqlParameter("@Refno", this.txtRefno.Text));
                sqlwheres.Add(" psd.Refno = @Refno ");
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                listSQLParameter.Add(new SqlParameter("@ColorID", this.txtColor.Text));
                sqlwheres.Add(" psd.ColorID = @ColorID ");
            }

            if (sqlwheres.Count > 0)
            {
                sqlwhere = string.Join(" and ", sqlwheres);
            }
            #endregion Where
            #region Sqlcmd
            string sqlcmd = $@"
select distinct
	ps.SuppID,
	s.AbbEN,
	psd.Refno,
	psd.ColorID,
	fd.FirstDyelot  FirstDyelot,
	fd.TPEFirstDyelot as TPEFirstDyelot,
	psd.Refno
from Po_Supp_Detail psd with(nolock)
left join Po_Supp ps on ps.ID= psd.id and ps.SEQ1 = psd.seq1
left join Supp s with(nolock) on s.ID = ps.SuppID
left join FirstDyelot fd on fd.Refno = psd.Refno and fd.ColorID = psd.ColorID and fd.SuppID = ps.SuppID 
where   ps.seq1 not like '7%'  and 
{sqlwhere}
and psd.FabricType = 'F'
order by ps.SuppID
";
            #endregion Sqlcmd
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listSQLParameter, out dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt2.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found.");
            }
            this.listControlBindingSource2.DataSource = dt2;
            dt2.AcceptChanges();
        }

        private void btnQuery2_Click(object sender, EventArgs e)
        {
            Page2_Query();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            this.grid2.ValidateControl();
            if (dt2 == null || dt2.Rows.Count == 0)
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
merge FirstDyelot t
using #tmp s
on t.Refno = s.Refno and t.SuppID = s.SuppID and t.ColorID = s.ColorID
when matched then update set 
	FirstDyelot=s.FirstDyelot,
	EditName = '{Sci.Env.User.UserID}',
	EditDate = GETDATE()
when not matched by target then 
insert([Refno],[SuppID],[ColorID],[FirstDyelot],[EditName],[EditDate])
VALUES(s.[Refno],s.[SuppID],s.[ColorID],s.[FirstDyelot],'{Sci.Env.User.UserID}',GETDATE())
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
            Page2_Query();
        }
        #endregion#Tab_Page2

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataRow drSystem;
            if (MyUtility.Check.Seek("select * from system", out drSystem))
            {
                Sci.Env.Cfg.FtpServerIP = drSystem["FtpIP"].ToString().Trim();
                Sci.Env.Cfg.FtpServerAccount = drSystem["FtpID"].ToString().Trim();
                Sci.Env.Cfg.FtpServerPassword = drSystem["FtpPwd"].ToString().Trim();
            }
            this.Close();
        }
    }
}
