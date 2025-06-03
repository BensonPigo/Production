using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_ItemSeqFiles : Sci.Win.Tems.QueryForm
    {
        private string strPoid;

        /// <inheritdoc/>
        public P03_ItemSeqFiles(string poid)
        {
            this.InitializeComponent();
            this.strPoid = poid;
            this.Requery();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("@", header: "@", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SuppName", header: "Supplier", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("SuppID", header: "SuppID", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Button("...", header: "File", width: Widths.AnsiChars(5), onclick: this.ClickClip);
        }

        /// <inheritdoc/>
        private void ClickClip(object sender, EventArgs e)
        {
            var row = this.grid.GetDataRow(this.listControlBindingSource1.Position);
            if (row == null)
            {
                return;
            }

            var id = row["UniqueKey"].ToString();
            if (id.IsNullOrWhiteSpace())
            {
                return;
            }

            string sqlcmd = $@"select 
            [FileName] = TableName + PKey,
            SourceFile,
            AddDate
            from Clip
            where TableName = 'PoItem' and 
            UniqueKey = '{id}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
            }

            string filePath = MyUtility.GetValue.Lookup("select [path] from CustomizedClipPath where TableName = 'PoItem'");

            // 組ClipPath
            string clippath = MyUtility.GetValue.Lookup($"select ClipPath from System");
            foreach (DataRow dataRow in dt.Rows)
            {
                string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                string saveFilePath = Path.Combine(clippath, yyyyMM);
                string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                lock (FileDownload_UpData.DownloadFileAsync($"{PmsWebAPI.PMSAPApiUri}/api/FileDownload/GetFile", filePath + "\\" + yyyyMM, fileName, saveFilePath))
                {
                }
            }

            using (var dlg = new PublicForm.ClipGASA("PoItem", id, false, row, apiUrlFile: $"{PmsWebAPI.PMSAPApiUri}/api/FileDelete/RemoveFile"))
            {
                dlg.ShowDialog();

                foreach (DataRow dataRow in dt.Rows)
                {
                    string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                    string saveFilePath = Path.Combine(clippath, yyyyMM);
                    string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                    string deleteFile = Path.Combine(saveFilePath, fileName);
                }
            }
        }

        private void Requery()
        {
            string sqlcmd = $@"
            select
            [@] = isnull(IsFile.val,''),
            [POID] = wh.POID, 
            [Seq1] = ps.Seq1,
            [SuppName] = iif(s.AbbCH = s.AbbEN, s.AbbEN,s.AbbCH+'/'+ s.AbbEN),
            [SuppID] = ps.SuppID, 
            [ThirdCountry] =s.ThirdCountry,
            [IsJunk] = IsJunk.val,
            [UniqueKey] = ps.ID+ps.SEQ1
            from View_WH_Orders wh
            inner join PO_Supp ps on ps.id = wh.POID
            inner join Supp s on s.id = ps.SuppID
            OUTER APPLY
            (
	            Select top 1 val = iif(c.PKey is NOT null and c.UniqueKey is NOT null, '@','')
	            FROM Clip c
	            WHERE c.UniqueKey = ps.ID+ps.SEQ1
            )IsFile
            OUTER APPLY
            (
	            SELECT
	            Id,
	            seq1,
	            val = CASE WHEN COUNT(1) = SUM(CASE WHEN Junk = 1 THEN 1 ELSE 0 END) THEN 1 ELSE 0 END
	            FROM PO_Supp_Detail psd
	            WHERE psd.id = ps.id AND psd.seq1 = ps.SEQ1
	            GROUP BY Id, seq1
	            HAVING COUNT(1) > 0
            )IsJunk
            where wh.ID ='{this.strPoid}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridtb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = gridtb;
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var rows = this.grid.Rows.Cast<DataGridViewRow>();

            var coloredRows = rows.Select(gridDr =>
            {
                DataRow dr = this.grid.GetDataRow(gridDr.Index);
                bool isThirdCountry = MyUtility.Convert.GetBool(dr["ThirdCountry"]);
                string isJunkValue = MyUtility.Convert.GetString(dr["IsJunk"]);

                Color backgroundColor = Color.Empty;

                if (isThirdCountry)
                {
                    backgroundColor = Color.FromArgb(220, 140, 255);
                }

                if (isJunkValue == "1")
                {
                    backgroundColor = Color.FromArgb(190, 190, 190);
                }

                return new
                {
                    Row = gridDr,
                    BackgroundColor = backgroundColor,
                };
            }).ToList();

            coloredRows.ForEach(coloredRow =>
            {
                coloredRow.Row.DefaultCellStyle.BackColor = coloredRow.BackgroundColor;
            });
        }
    }
}
