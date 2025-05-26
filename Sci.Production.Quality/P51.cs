using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.IO;
using Sci.Win.Tools;
using System.Data.SqlClient;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using System.Runtime.CompilerServices;
using Sci.Production.Prg;

namespace Sci.Production.Quality
{
    /// <summary>
    /// Material Document upload by Shipment
    /// </summary>
    internal partial class P51 : Sci.Win.Tems.QueryForm
    {
        private DataTable dtMain;
        private DataRow drBasic;
        private Ict.Win.UI.DataGridViewTextBoxColumn colPOID;
        private Ict.Win.UI.DataGridViewTextBoxColumn colSeq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn colSeq2;
        private Ict.Win.UI.DataGridViewTextBoxColumn colPINO;
        private Ict.Win.UI.DataGridViewTextBoxColumn colInvNO;
        private Ict.Win.UI.DataGridViewTextBoxColumn colWKold;
        static string CHARs = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Material Document upload by Shipment
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P51(ToolStripMenuItem menuitem)
           : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.GetMaterialDocument();
        }

        private void GetMaterialDocument()
        {
            DataTable dt;
            string sql = $"Select distinct DocumentName From MaterialDocument WHERE FileRule in (4,5) and junk = 0";
            var result = DBProxy.Current.Select(string.Empty, sql, out dt);
            if (result && dt.Rows.Count > 0)
            {
                this.cboDocumentname.DataSource = dt;
                this.cboDocumentname.ValueMember = "DocumentName";
                this.cboDocumentname.DisplayMember = "DocumentName";
                this.cboDocumentname.SelectedIndex = 0;
            }
            else
            {
                this.ShowWarning("[Material Document Platform] is not exists!");
                return;
            }
        }

        private void GridSetup()
        {
            this.UI_grid.IsEditingReadOnly = false;
            this.UI_grid.AutoGenerateColumns = false;
            this.Helper.Controls.Grid.Generator(this.UI_grid)
               .CheckBox("sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: true, falseValue: false)
               .Text("ExportID", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Date("ETA", header: "ETA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("POID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out this.colPOID)
               .Text("Seq1", header: "#1", width: Widths.AnsiChars(5), iseditingreadonly: true).Get(out this.colSeq1)
               .Text("Seq2", header: "#2", width: Widths.AnsiChars(5), iseditingreadonly: true).Get(out this.colSeq2)
               .Text("FactoryID", header: "FTY", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("SuppGroup", header: "Supp Group", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("BrandRefNo", header: "BrandRefNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("ColorID", header: "Color", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("ColorDesc", header: "Color Desc", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Button("...", header: "File", width: Widths.AnsiChars(8), onclick: this.ClickClip)
               .Date("ReportDate", header: "Upload date", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: this.GetUploadDateCell())
               .Text("AWBno", header: "AWBno", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: this.GetAWBnoCell())
               .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true)
               .Numeric("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true)
               .Numeric("ShipFOC", header: "Ship FOC", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true)
               .Text("Pino", header: "PI#", width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out this.colPINO)
               .Text("InvNo", header: "Inv#", width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out this.colInvNO)
               .Text("ExportIDOld", header: "WK# old", width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out this.colWKold)
               .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
               ;

            this.UI_grid.DataSource = this.gridBS;
            this.UI_grid.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.UI_grid_RowPrePaint);
        }

        private DataGridViewGeneratorDateColumnSettings GetUploadDateCell()
        {
            DataGridViewGeneratorDateColumnSettings ts = new DataGridViewGeneratorDateColumnSettings();
            string whereStr = string.Empty;

            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow r = grid.GetDataRow<DataRow>(e.RowIndex);

                var newValue = e.FormattedValue;
                var oldValue = MyUtility.Convert.GetDate(r["ReportDate"]);

                if (!MyUtility.Check.Empty(newValue) && !newValue.EqualString(oldValue))
                {
                    if (!MyUtility.Convert.GetBool(r["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        e.Cancel = true;
                        r["ReportDate"] = DBNull.Value;
                        return;
                    }

                    bool isByPO = this.drBasic["FileRule"].ToString() == "4";
                    string tableName = string.Empty;
                    if (isByPO)
                    {
                        tableName = "NewSentReport";
                    }
                    else
                    {
                        tableName = "ExportRefnoSentReport";
                    }

                    DateTime? reportDate = MyUtility.Convert.GetDate(newValue);

                    bool isBetween2000And2099 = reportDate.Value.Year >= 2000 && reportDate.Value.Year <= 2099;

                    if (isBetween2000And2099 == false)
                    {
                        MyUtility.Msg.WarningBox("Date shoule be between 2000 ~ 2099");
                        e.Cancel = true;
                        return;
                    }

                    r["ReportDate"] = MyUtility.Convert.GetDate(newValue).ToYYYYMMDD();
                    if (r["AddDate"].Empty())
                    {
                        r["AddDate"] = DateTime.Now;
                        r["AddName"] = Env.User.UserID;
                    }
                    else
                    {
                        r["EditDate"] = DateTime.Now;
                        r["EditName"] = Env.User.UserID;
                    }

                    string sql = $@"
if (@tableName = 'NewSentReport')
begin
    IF EXISTS(select 1 FROM dbo.NewSentReport WHERE ExportID = '{r["ExportID"]}' and PoID = '{r["PoID"]}' and Seq1 = '{r["Seq1"]}' and Seq2 = '{r["Seq2"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}')
    Begin
        Update dbo.NewSentReport SET ReportDate = '{r["ReportDate"]}', EditName = '{Env.User.UserID}' ,EditDate = getdate() 
        WHERE ExportID = '{r["ExportID"]}' and PoID = '{r["PoID"]}' and Seq1 = '{r["Seq1"]}' and Seq2 = '{r["Seq2"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}'
    End
    Else
     Begin
        Insert Into dbo.[NewSentReport]
         (
           [ExportID]
          ,[PoID]
          ,[Seq1]
          ,[Seq2]
          ,[ReportDate]
          ,[AddDate]
          ,[AddName]
          ,[DocumentName]
          ,[BrandID]
          ,[UniqueKey]
        )
        Values(
          '{r["ExportID"]}'
         ,'{r["PoID"]}'
         ,'{r["Seq1"]}'
         ,'{r["Seq2"]}'
         ,'{r["ReportDate"]}'
         ,getdate()
         ,'{Env.User.UserID}'
         ,'{this.drBasic["DocumentName"]}'
         ,'{this.drBasic["BrandID"]}'
         ,'{r["ExportID"]}'+'_'+'{r["PoID"]}'+'_'+'{r["Seq1"]}'+'_'+'{r["Seq2"]}'+'_'+'{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}'
        )
     End
END
ELSE
BEGIN
    IF EXISTS(select 1 FROM dbo.ExportRefnoSentReport WHERE ExportID = '{r["ExportID"]}' and BrandRefno = '{r["BrandRefno"]}' and ColorID = '{r["ColorID"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}')
    Begin
        Update dbo.ExportRefnoSentReport SET ReportDate = '{r["ReportDate"]}', EditName = '{Env.User.UserID}' ,EditDate = getdate() 
        WHERE ExportID = '{r["ExportID"]}' and BrandRefno = '{r["BrandRefno"]}' and ColorID = '{r["ColorID"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}'
    End
    Else
     Begin
        Insert Into dbo.[ExportRefnoSentReport]
         (
           [ExportID]
          ,[BrandRefno]
          ,[ColorID]
          ,[ReportDate]
          ,[AddDate]
          ,[AddName]
          ,[DocumentName]
          ,[BrandID]
          ,[UniqueKey]
        )
        Values(
          '{r["ExportID"]}'
         ,'{r["BrandRefno"]}'
         ,'{r["ColorID"]}'
         ,'{r["ReportDate"]}'
         ,getdate()
         ,'{Env.User.UserID}'
         ,'{this.drBasic["DocumentName"]}'
         ,'{this.drBasic["BrandID"]}'
         ,'{r["ExportID"]}'+'_'+'{r["BrandRefno"]}'+'_'+'{r["ColorID"]}'+'_'+'{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}'
        )
     End  
END

";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("tableName", tableName),
                    };
                    var result = DBProxy.Current.Execute(string.Empty, sql, plis);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
            };

            return ts;
        }

        private void UI_grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataRow drMainGrid = this.UI_grid.GetDataRow(e.RowIndex);
            DataGridViewRow dgvMainGrid = this.UI_grid.Rows[e.RowIndex];
            if (drMainGrid == null || dgvMainGrid == null)
            {
                return;
            }

            dgvMainGrid.Cells["AWBno"].Style.BackColor = VFPColor.Blue_183_227_255;
            dgvMainGrid.Cells["ReportDate"].Style.BackColor = VFPColor.Blue_183_227_255;
        }

        private DataGridViewGeneratorTextColumnSettings GetAWBnoCell()
        {
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                var newValue = e.FormattedValue;
                var oldValue = row["AWBno"].ToString();

                if (!MyUtility.Check.Empty(newValue) && !newValue.EqualString(oldValue))
                {
                    if (!MyUtility.Convert.GetBool(row["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        e.Cancel = true;
                        row["AWBno"] = DBNull.Value;
                        return;
                    }

                    if (row["ReportDate"] == DBNull.Value)
                    {
                        MyUtility.Msg.ErrorBox("Please update [Upload date] first!", "Error");
                        e.Cancel = true;
                        row["AWBno"] = DBNull.Value;
                        return;
                    }

                    bool isGASA = this.drBasic["FileRule"].ToString() == "4";
                    string sql = string.Empty;
                    if (isGASA)
                    {
                        sql = $@"

Update NewSentReport SET  AWBNO = @updateData, EditName = @UserID ,EditDate = getdate() WHERE ExportID = @ExportID and POID = @POID and Seq1 = @Seq1 and Seq2 = @Seq2 and DocumentName = @DocumentName and BrandID = @BrandID

";
                        List<SqlParameter> plis = new List<SqlParameter>()
                    {
                         new SqlParameter("ExportID", row["ExportID"]),
                         new SqlParameter("POID", row["POID"]),
                         new SqlParameter("Seq1", row["Seq1"]),
                         new SqlParameter("Seq2", row["Seq2"]),
                         new SqlParameter("UserID", Env.User.UserID),
                         new SqlParameter("updateData", newValue),
                         new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                         new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                        var result = DBProxy.Current.Execute(string.Empty, sql, plis);
                        if (!result)
                        {
                            this.ShowErr(result);
                            e.Cancel = true;
                            row["AWBno"] = DBNull.Value;
                            return;
                        }
                    }
                    else
                    {
                        sql = $@"

Update ExportRefnoSentReport SET  AWBNO = @updateData, EditName = @UserID ,EditDate = getdate() WHERE  ExportID = @ExportID and BrandRefno = @BrandRefno and ColorID = @ColorID and DocumentName = @DocumentName and BrandID = @BrandID

";
                        List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("ExportID", row["ExportID"]),
                        new SqlParameter("BrandRefno", row["BrandRefno"]),
                        new SqlParameter("ColorID", row["ColorID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("updateData", newValue),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                        var result = DBProxy.Current.Execute(string.Empty, sql, plis);
                        if (!result)
                        {
                            this.ShowErr(result);
                            e.Cancel = true;
                            row["AWBno"] = DBNull.Value;
                            return;
                        }
                    }

                    row["AWBno"] = newValue;
                    row["EditDate"] = DateTime.Now;
                    row["EditName"] = Env.User.UserID;
                }
            };

            return ts;
        }

        /// <inheritdoc/>
        private void ClickClip(object sender, EventArgs e)
        {
            var row = this.UI_grid.GetCurrentDataRow();
            if (row == null)
            {
                return;
            }

            var id = row["UniqueKey"].ToString();
            if (id.IsNullOrWhiteSpace())
            {
                return;
            }

            string tableName = string.Empty;
            bool isGASA = this.drBasic["FileRule"].ToString() == "4";
            if (isGASA)
            {
                tableName = "NewSentReport";
            }
            else
            {
                tableName = "ExportRefnoSentReport";
            }

            string sqlcmd = $@"select 
            [FileName] = TableName + PKey,
            SourceFile,
            AddDate
            from GASAClip
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

            foreach (DataRow dataRow in dt.Rows)
            {
                string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                string saveFilePath = Path.Combine(clippath, yyyyMM);
                string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                lock (FileDownload_UpData.DownloadFileAsync($"{PmsWebAPI.PMSAPApiUri}/api/FileDownload/GetFile", filePath + "\\" + yyyyMM, fileName, saveFilePath))
                {
                }
            }

            #region 傳遞額外的參數
            if (!row.Table.Columns.Contains("BasicBrandID"))
            {
                row.Table.Columns.Add("BasicBrandID", typeof(string));
            }

            if (!row.Table.Columns.Contains("BasicDocumentName"))
            {
                row.Table.Columns.Add("BasicDocumentName", typeof(string));
            }

            var now = DateTime.Now;

            if (row["UniqueKey"].Empty())
            {
                row["AddName"] = Env.User.UserID;
                row["AddDate"] = now;
            }
            else
            {
                row["EditName"] = Env.User.UserID;
                row["EditDate"] = now;
            }

            row["BasicBrandID"] = this.drBasic["BrandID"];
            row["BasicDocumentName"] = this.drBasic["DocumentName"];
            #endregion

            bool isEnable = MyUtility.Check.Empty(row["canModify"]) ? false : true;
            using (var dlg = new PublicForm.ClipGASA(tableName, id, isEnable, row, apiUrlFile: $"{PmsWebAPI.PMSAPApiUri}/api/FileDelete/RemoveFile"))
            {
                dlg.ShowDialog();

                foreach (DataRow dataRow in dt.Rows)
                {
                    string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                    string saveFilePath = Path.Combine(clippath, yyyyMM);
                    string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                    string deleteFile = Path.Combine(saveFilePath, fileName);
                    if (File.Exists(deleteFile))
                    {
                        File.Delete(deleteFile);
                    }
                }
            }
        }

        private void Btn_NewSearch_Click(object sender, EventArgs e)
        {
            this.txtBrand1.Text = string.Empty;
            this.dateETA.Value1 = null;
            this.dateETA.Value2 = null;
            this.txtWKID.Text = string.Empty;
            this.txtBrandRefno.Text = string.Empty;
            this.txtMultiSupplier1.Text = string.Empty;
            this.txtRefno.Text = string.Empty;
            this.txtUser1.TextBox1.Text = string.Empty;
            this.txtColor.Text = string.Empty;
            this.txtStyle1.Text = string.Empty;
            this.txtPINO.Text = string.Empty;
            this.chkUploadRecord.Checked = false;
            this.chkNonValidDoc.Checked = false;
        }

        private void Btn_Find_Click(object sender, EventArgs e)
        {
            var conditions = new StringBuilder(@" 1=1");
            List<SqlParameter> parmes = new List<SqlParameter>();
            if (this.cboDocumentname.Text == string.Empty || this.txtBrand1.Text == string.Empty)
            {
                this.ShowWarning("[Document Name] and [Brand] can't be empty!");
                return;
            }

            string cmd = $@"
            Select m.* ,MtlType = MtlType.value,WeaveType = WeaveType.value,supplier = supp.value
            From MaterialDocument m
            Outer apply ( SELECT
		            value = STUFF((SELECT
				            CONCAT(',', MtlType.MtltypeId)
			            FROM MaterialDocument_MtlType MtlType WITH (NOLOCK)
			            WHERE MtlType.DocumentName = m.DocumentName and MtlType.BrandID = m.BrandID
			            FOR XML PATH (''))
		            , 1, 1, '')
            ) MtlType
			Outer apply ( SELECT
		            value = STUFF((SELECT
				            CONCAT(',', WeaveType.WeaveTypeId)
			            FROM MaterialDocument_WeaveType WeaveType WITH (NOLOCK)
			            WHERE WeaveType.DocumentName = m.DocumentName and WeaveType.BrandID = m.BrandID
			            FOR XML PATH (''))
		            , 1, 1, '')
            ) WeaveType
            Outer apply ( SELECT
		            value = STUFF((SELECT
				            CONCAT(',', supp.SuppID)
			            FROM MaterialDocument_Supplier supp WITH (NOLOCK)
			            WHERE supp.DocumentName = m.DocumentName
			            AND supp.BrandID = m.BrandID
			            FOR XML PATH (''))
		            , 1, 1, '')
            ) supp
            Where documentName = @documentName and brandID = @brandID and m.FileRule in ('4','5') and m.junk = 0";

            var res = DBProxy.Current.SeekEx(cmd, "documentName", this.cboDocumentname.Text, "brandID", this.txtBrand1.Text);
            if (!res)
            {
                this.ShowErr(res.InnerResult);
                return;
            }

            if (res.ExtendedData == null)
            {
                MyUtility.Msg.ErrorBox("找不到基本檔設定!");
                return;
            }

            this.drBasic = res.ExtendedData;

            bool isByPO = this.drBasic["FileRule"].ToString() == "4";
            this.colPOID.Visible = isByPO;
            this.colSeq1.Visible = isByPO;
            this.colSeq2.Visible = isByPO;
            this.colPINO.Visible = isByPO;
            this.colInvNO.Visible = isByPO;
            this.colWKold.Visible = isByPO;

            string join = string.Empty;
            if (isByPO)
            {
                join = "LEFT JOIN dbo.NewSentReport sr WITH (NOLOCK) on sr.ExportID = main.ExportID  and sr.PoID = main.PoID and sr.Seq1 = main.Seq1 and sr.Seq2 = main.Seq2 and sr.BrandID = main.BrandID and sr.DocumentName = @DocumentName";
            }
            else
            {
                join = "LEFT JOIN dbo.ExportRefnoSentReport sr WITH (NOLOCK) on sr.ExportID = main.ExportID  and sr.BrandRefno = main.BrandRefno and sr.ColorID = main.ColorID and sr.BrandID = main.BrandID and sr.DocumentName = @DocumentName";
            }

            // 預設關聯庫存項
            var getpo3_sql = @"
LEFT JOIN PO_Supp_Detail stockPO3 with (nolock)
    on iif(po3.StockPOID >'', 1, 0) = 1
    and stockPO3.ID =  po3.StockPOID
	and stockPO3.Seq1 = po3.StockSeq1
	and stockPO3.Seq2 = po3.StockSeq2	        
Outer APPLY (
	Select ID = COALESCE(stockPO3.ID, po3.ID)
    , SCIRefno = COALESCE(stockPO3.SCIRefno, po3.SCIRefno)
    , Seq1 = COALESCE(stockPO3.Seq1, po3.Seq1)
    , Seq2 = COALESCE(stockPO3.Seq2, po3.Seq2)
    , Junk = COALESCE(stockPO3.Junk, po3.Junk)
    , Qty = COALESCE(stockPO3.Qty, po3.Qty)
    , Foc = COALESCE(stockPO3.Foc, po3.Foc)
) getpo3";

            #region 基本檔設定
            conditions.AppendLine($" and f.type='{this.drBasic["FabricType"]}'");
            conditions.AppendLine($" and f.BrandRefNo <> ''");

            parmes.Add(new SqlParameter() { ParameterName = "@documentName", SqlDbType = SqlDbType.VarChar, Size = 100, Value = this.cboDocumentname.Text });
            parmes.Add(new SqlParameter() { ParameterName = "@brandID", SqlDbType = SqlDbType.VarChar, Size = 8, Value = this.txtBrand1.Text });
            parmes.Add(new SqlParameter("@FileRule", this.drBasic["FileRule"]));

            // 排除庫存項
            if (MyUtility.Convert.GetBool(this.drBasic["ExcludeStock"]))
            {
                conditions.AppendLine($" and (po3.seq1 < '70' or po3.seq1 > '79')");
                getpo3_sql = @"
Outer APPLY (
	Select ID = po3.ID
    , SCIRefno = po3.SCIRefno
    , Seq1 = po3.Seq1
    , Seq2 = po3.Seq2
    , Junk = po3.Junk
    , Qty = po3.Qty
    , Foc = po3.Foc
) getpo3";
            }
            #endregion

            if (this.txtWKID.Text != string.Empty)
            {
                conditions.AppendLine(" And (ed.ID = @SearchWK or ed.ExportIDOld = @SearchWK)");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchWK", SqlDbType = SqlDbType.VarChar, Size = 13, Value = this.txtWKID.Text });
            }

            if (this.txtSeason1.Text != string.Empty)
            {
                conditions.AppendLine(" And o.SeasonID = @SeasonID");
                parmes.Add(new SqlParameter() { ParameterName = "@SeasonID", SqlDbType = SqlDbType.VarChar, Size = 10, Value = this.txtSeason1.Text });
            }

            if (this.dateETA.HasValue1)
            {
                conditions.AppendLine(" And e.ETA >= @Eta1");
                parmes.Add(new SqlParameter("@Eta1", this.dateETA.Value1));
            }

            if (this.dateETA.HasValue2)
            {
                conditions.AppendLine(" And e.ETA <= @Eta2");
                parmes.Add(new SqlParameter("@Eta2", this.dateETA.Value2));
            }

            if (this.txtRefno.Text != string.Empty)
            {
                conditions.AppendLine(" And f.Refno = @SearchRefno");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchRefno", SqlDbType = SqlDbType.VarChar, Size = 20, Value = this.txtRefno.Text });
            }

            if (this.txtBrandRefno.Text != string.Empty)
            {
                conditions.AppendLine(" And f.BrandRefNo = @SearchBrandRefno");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchBrandRefno", SqlDbType = SqlDbType.VarChar, Size = 50, Value = this.txtBrandRefno.Text });
            }

            if (this.txtColor.Text != string.Empty)
            {
                conditions.AppendLine(" And Color.ID = @SearchColor");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchColor", SqlDbType = SqlDbType.VarChar, Size = 6, Value = this.txtColor.Text });
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                conditions.AppendLine(" And  o.FactoryID = @FactoryID");
                parmes.Add(new SqlParameter() { ParameterName = "@FactoryID", SqlDbType = SqlDbType.VarChar, Size = 6, Value = this.txtfactory.Text });
            }

            if (this.txtStyle1.Text != string.Empty)
            {
                conditions.AppendLine(" And s.ID = @SearchStyle");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchStyle", SqlDbType = SqlDbType.VarChar, Size = 15, Value = this.txtStyle1.Text });
            }

            if (this.txtPINO.Text != string.Empty)
            {
                conditions.AppendLine(" And ed.PINO = @SearchPINO");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchPINO", SqlDbType = SqlDbType.VarChar, Size = 25, Value = this.txtPINO.Text });
            }

            conditions.AppendLine($" and Season.Month >= (select month From Season where id ='{this.drBasic["ActiveSeason"]}' and BrandID = '{this.drBasic["brandID"]}')");
            if (!this.drBasic["EndSeason"].Empty())
            {
                conditions.AppendLine($" and Season.Month <= (select month From Season where id ='{this.drBasic["EndSeason"]}' and BrandID = '{this.drBasic["brandID"]}')");
            }

            if (!this.drBasic["ExcludeProgram"].Empty())
            {
                conditions.AppendLine($" and o.ProgramID not in (select data from splitstring('{this.drBasic["ExcludeProgram"]}',','))");
            }

            if (MyUtility.Convert.GetBool(this.drBasic["ExcludeReplace"]))
            {
                conditions.AppendLine($" and (po3.seq1 < '50' or po3.seq1 > '69')");
            }

            if (MyUtility.Convert.GetBool(this.drBasic["ExcludeStock"]))
            {
                conditions.AppendLine($" and po3.seq1 < '70'");
            }

            if (this.drBasic["FabricType"].ToString() == "A" && !this.drBasic["MtlTypeClude"].Empty() && !this.drBasic["MtlType"].Empty())
            {
                string not = this.drBasic["MtlTypeClude"].ToString() == "E" ? "not" : string.Empty;
                conditions.AppendLine($" and f.MtltypeId {not} in (select data from splitstring('{this.drBasic["MtlType"]}', ','))");
            }
            else if (this.drBasic["FabricType"].ToString() == "F" && !this.drBasic["MtlTypeClude"].Empty() && !this.drBasic["WeaveType"].Empty())
            {
                string not = this.drBasic["MtlTypeClude"].ToString() == "E" ? "not" : string.Empty;
                conditions.AppendLine($" and f.WeaveTypeId {not} in (select data from splitstring('{this.drBasic["WeaveType"]}', ','))");
            }

            if (!this.drBasic["SupplierClude"].Empty() && !this.drBasic["supplier"].Empty())
            {
                string not = this.drBasic["SupplierClude"].ToString() == "E" ? "not" : string.Empty;
                conditions.AppendLine($" and s2.ID {not} in (select data from splitstring('{this.drBasic["supplier"]}', ','))");
            }

            string category = DBProxy.Current.LookupEx<string>(
                @"
 SELECT
	 value = STUFF((SELECT
			 CONCAT(',', ID)
		 FROM DropDownList WITH (NOLOCK)
		 WHERE type ='Category'
		 and NAME in(select data from splitstring(@Category,','))
		 FOR XML PATH (''))
	 , 1, 1, '')",
                "Category",
                this.drBasic["Category"]).ExtendedData;

            conditions.AppendLine($" and o.Category in (select data from splitstring('{category}',','))");

            if (this.txtMultiSupplier1.Text != string.Empty)
            {
                conditions.AppendLine(" and s2.ID in (select data from splitstring(@supp,','))");
                parmes.Add(new SqlParameter("@supp", this.txtMultiSupplier1.Text));
            }

            if (this.txtUser1.TextBox1.Text != string.Empty)
            {
                conditions.AppendLine(" And po.PCHandle = @pchandle");
                parmes.Add(new SqlParameter() { ParameterName = "@pchandle", SqlDbType = SqlDbType.VarChar, Size = 10, Value = this.txtUser1.TextBox1.Text });
            }

            string sql = $@"
           select distinct 
            CAST(0 AS BIT) sel     
           ,main.ExportID
           ,main.ETA
           ,main.Qty
           ,main.ShipQty
           ,main.ShipFOC
           ,main.ExportIDOld
           ,main.SuppGroup
           ,main.SuppID
           ,main.Supplier
           ,main.RefNo
           ,main.BrandRefNo
           ,main.Pino
           ,main.InvNo
           ,main.ColorID
           ,main.ColorDesc
           ,main.POID
           ,main.Seq1
           ,main.Seq2           
           ,AWBNO = sr.AWBNO
	       ,ReportDate = CONVERT(VARCHAR(10), sr.ReportDate, 23)
           ,sr.AddName
           ,sr.AddDate
           ,sr.EditName
           ,sr.EditDate
           ,sr.Ukey
           ,sr.UniqueKey
           ,main.canModify
           ,FactoryID = main.FtyGroup
         FROM (
                    Select
                   ed.ID as ExportID
                   ,Convert(varchar(10), e.Eta, 23) as ETA
		           ,Qty = SUM(IIF(IsNull(po3.StockPOID, '') = '' , po3.Qty, getpo3.Qty))
		           ,ShipQty = SUM(ed.Qty)
		           ,ShipFOC = SUM(ed.FOC)
                   ,SuppGroup = Concat(s2.ID, '-', s2.AbbEN)
                   ,SuppID = iif({this.drBasic["FileRule"]} = 5, s2.ID, Supp.ID)
		           ,Supplier = iif({this.drBasic["FileRule"]} = 5, IIF(Isnull(s2.AbbEN, '') = '', s2.ID, Concat(s2.ID, '-', s2.AbbEN)), IIF(Isnull(Supp.AbbEN, '') = '', Supp.ID, Concat(Supp.ID, '-', Supp.AbbEN)))  
                   ,RefNo = min(f.RefNo)
		           ,f.BrandRefNo
                   ,ColorID = Color.ID
                   ,ColorDesc = Color.Name
                   ,o.BrandID
                   ,POID = iif({this.drBasic["FileRule"]} = 5, '', ed.POID)
                   ,Seq1 = iif({this.drBasic["FileRule"]} = 5, '', ed.Seq1)
                   ,Seq2 = iif({this.drBasic["FileRule"]} = 5, '', ed.Seq2)
		           ,Pino = iif({this.drBasic["FileRule"]} = 5, '', ed.Pino)
		           ,InvNo = iif({this.drBasic["FileRule"]} = 5, '', e.InvNo)
		           ,ExportIDOld = iif({this.drBasic["FileRule"]} = 5, '', ed.ExportIDOld)
                   ,canModify = CAST(iif((chkNoRes.value is null and '{this.drBasic["Responsibility"]}' = 'F') or chkNoRes.value = 'F', 1, 0) AS BIT)
                   ,o.FtyGroup
                 from  PO_Supp_Detail po3 WITH (NOLOCK)
                 {getpo3_sql}
                 INNER JOIN Orders o with(nolock) on o.ID = IIF(IsNull(po3.StockPOID, '') = '' , po3.ID, getpo3.ID)
                 inner join Fabric f WITH (NOLOCK) on f.SCIRefno = IIF(IsNull(po3.StockPOID, '') = '' , po3.SCIRefno, getpo3.SCIRefno)
                 inner join PO_Supp po2 WITH (NOLOCK) on po2.ID = o.ID and po2.Seq1 = IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq1, getpo3.Seq1)
                 inner join PO WITH (NOLOCK) on po.ID = po2.ID 
                 INNER JOIN Season WITH (NOLOCK) on o.SeasonID = Season.ID and o.BrandID = Season.BrandID
		         INNER JOIN Style s WITH(NOLOCK) on s.Ukey = o.StyleUkey		 
		         INNER join Export_Detail ed WITH (NOLOCK) on ed.PoID = o.ID and ed.Seq1 = po2.Seq1 and ed.Seq2 = IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq2, getpo3.Seq2)
		         INNER join Export e WITH (NOLOCK) on e.ID = ed.ID   
                 Inner Join Supp WITH (NOLOCK) on po2.SuppID = Supp.ID
                 Inner Join BrandRelation as bs WITH (NOLOCK) ON bs.BrandID = o.BrandID and bs.SuppID = supp.ID
                 Inner Join Supp s2 WITH (NOLOCK) on bs.SuppGroup = s2.ID
                 Outer Apply(
                      SELECT Color FROM GetPo3Spec(IIF(IsNull(po3.StockPOID, '') = '' , po3.ID, getpo3.ID),IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq1, getpo3.Seq1),IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq2, getpo3.Seq2)) po3Spec
                 )po3Spec
                 LEFT JOIN  Color WITH (NOLOCK) ON Color.BrandId = o.BrandID AND Color.ID = po3Spec.Color
                 Outer apply(
                    select top 1 value = Responsibility FROM MaterialDocument_Responsbility where DocumentName = @documentName and BrandID = @brandID and SuppID = s2.ID
                )chkNoRes
                where po2.SuppID <> 'FTY'
                    and getpo3.Junk  = 0     
                    and (getpo3.Qty > 0 or getpo3.Foc > 0)
                    and o.BrandID = @brandID
	                and s.DevOption = 0
                    and f.BrandRefNo <> ''
                    and {conditions}
                GROUP BY ed.ID,e.Eta,iif({this.drBasic["FileRule"]} = 5, s2.ID, Supp.ID),iif({this.drBasic["FileRule"]} = 5, s2.AbbEN, Supp.AbbEN),f.BrandRefNo,Color.ID,Color.Name,o.BrandID,iif({this.drBasic["FileRule"]} = 5, '', ed.ExportIDOld),iif({this.drBasic["FileRule"]} = 5, '', e.InvNo),iif({this.drBasic["FileRule"]} = 5, '', ed.Pino),iif({this.drBasic["FileRule"]} = 5, '', ed.POID),iif({this.drBasic["FileRule"]} = 5, '', ed.Seq1),iif({this.drBasic["FileRule"]} = 5, '', ed.Seq2),chkNoRes.value,o.FtyGroup,s2.ID, s2.AbbEN
        )main
        {join}
        Where 1 = 1
        {(this.chkUploadRecord.Checked ? "and sr.ExportID <> ''" : string.Empty)}    
        {(this.chkNonValidDoc.Checked ? "and sr.ExportID is null" : string.Empty)}       
         Order By main.BrandRefno,main.ColorID
    ";

            DataTable dt = new DataTable();
            var result = DBProxy.Current.Select(string.Empty, sql, parmes, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                this.ShowInfo("No Data");
            }

            this.gridBS.DataSource = dt;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridBS.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("no data!");
                return;
            }

            // 13:[Upload date] 14:[AWBNO]
            if (!this.UI_grid.CurrentCell.ColumnIndex.IsOneOfThe(13, 14))
            {
                MyUtility.Msg.WarningBox("Please focus on [Upload date]/[AWBNO]!", "Error");
                return;
            }

            if (!dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel")).Any())
            {
                MyUtility.Msg.ErrorBox("No data selected!");
                return;
            }

            string updateData = this.txtUpdate.Text;
            bool isUpdateAwbNo = false;
            if (this.UI_grid.CurrentCell.ColumnIndex == 14)
            {
                isUpdateAwbNo = true;
            }
            else
            {
                DateTime tm;
                if (!DateTime.TryParse(updateData, out tm))
                {
                    MyUtility.Msg.ErrorBox(string.Format(" {0} isn't a valid date value!", updateData), "Error");
                    return;
                }
            }

            if (dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && w["ReportDate"].Empty()).Any())
            {
                MyUtility.Msg.ErrorBox("Please update [Upload date] first!");
                return;
            }

            var canModifySupp = dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && !w.Field<bool>("canModify")).Select(row => row["SuppID"].ToString()).Distinct()
                                        .ToList()
                                        .JoinToString(",");
            if (!canModifySupp.Empty())
            {
                MyUtility.Msg.ErrorBox(canModifySupp + " can't modify!");
                return;
            }

            dt.ExtNotDeletedRowsForeach(r =>
            {
                if (MyUtility.Convert.GetBool(r["sel"]))
                {
                    string sql = string.Empty;
                    bool isGASA = this.drBasic["FileRule"].ToString() == "4";

                    if (isGASA)
                    {
                        sql = $@"

Update NewSentReport SET  {(isUpdateAwbNo ? "AWBNO" : "ReportDate")} = @updateData, EditName = @UserID ,EditDate = getdate() WHERE ExportID = @ExportID and POID = @POID and Seq1 = @Seq1 and Seq2 = @Seq2 and DocumentName = @DocumentName and BrandID = @BrandID

";
                        List<SqlParameter> plis = new List<SqlParameter>()
                    {
                         new SqlParameter("ExportID", r["ExportID"]),
                         new SqlParameter("POID", r["POID"]),
                         new SqlParameter("Seq1", r["Seq1"]),
                         new SqlParameter("Seq2", r["Seq2"]),
                         new SqlParameter("UserID", Env.User.UserID),
                         new SqlParameter("updateData", updateData),
                         new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                         new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                        DBProxy.Current.Execute(string.Empty, sql, plis);
                    }
                    else
                    {
                        sql = $@"

Update ExportRefnoSentReport SET  {(isUpdateAwbNo ? "AWBNO" : "ReportDate")} = @updateData, EditName = @UserID ,EditDate = getdate() WHERE  ExportID = @ExportID and BrandRefno = @BrandRefno and ColorID = @ColorID and DocumentName = @DocumentName and BrandID = @BrandID

";
                        List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("ExportID", r["ExportID"]),
                        new SqlParameter("BrandRefno", r["BrandRefno"]),
                        new SqlParameter("ColorID", r["ColorID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("updateData", updateData),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                        DBProxy.Current.Execute(string.Empty, sql, plis);
                    }

                    if (isUpdateAwbNo)
                    {
                        r["AWBno"] = updateData;
                    }
                    else
                    {
                        r["ReportDate"] = MyUtility.Convert.GetDate(updateData).ToYYYYMMDD();
                    }

                    r["EditDate"] = DateTime.Now;
                    r["EditName"] = Env.User.UserID;
                }
            });

            dt.AcceptChanges();
        }

        private void BtnFileUpload_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridBS.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("no data!");
                return;
            }

            if (!dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel")).Any())
            {
                MyUtility.Msg.ErrorBox("No data selected!");
                return;
            }

            var canModifySupp = dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && !w.Field<bool>("canModify")).Select(row => row["SuppID"].ToString()).Distinct()
                                       .ToList()
                                       .JoinToString(",");
            if (!canModifySupp.Empty())
            {
                MyUtility.Msg.ErrorBox(canModifySupp + " can't modify!");
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog ofdFileName = ProductionEnv.GetOpenFileDialog();
            ofdFileName.Multiselect = true;
            if (ofdFileName.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(ofdFileName.FileName))
                {
                    this.ShowErr("Import File is not exist");
                    return;
                }
            }
            else
            {
                return;
            }

            string tableName = string.Empty;
            bool isByPO = this.drBasic["FileRule"].ToString() == "4";
            if (isByPO)
            {
                tableName = "NewSentReport";
            }
            else
            {
                tableName = "ExportRefnoSentReport";
            }

            string saveFilePath = DBProxy.Current.LookupEx<string>($@"select path From CustomizedClipPath WHERE TableName ='{tableName}'").ExtendedData;
            DateTime now = DateTime.Now;
            saveFilePath = Path.Combine(saveFilePath, now.ToString("yyyyMM"));
            List<string> pkeys = new List<string>();
            string[] files = ofdFileName.FileNames;
            int count = 0;
            foreach (string file in files)
            {
                string pkey = this.GetPKeyPre() + count.ToString().PadLeft(2, '0');
                string newFileName = tableName + pkey + Path.GetExtension(ofdFileName.FileName);
                string filenamme = Path.GetFileName(file);
                pkeys.Add(pkey + "-" + filenamme);

                count++;

                // 限制檔案大小
                var fileInfo = new FileInfo(ofdFileName.FileName);
                if (fileInfo.Length > 15 * 1024 * 1024)
                {
                    MyUtility.Msg.WarningBox("File size cannot exceed 15 MB limit!");
                    return;
                }

                // call API上傳檔案到Trade
                lock (FileDownload_UpData.UploadFile($"{PmsWebAPI.PMSAPApiUri}/api/FileUpload/PostFile", saveFilePath, newFileName, ofdFileName.FileName))
                {
                }
            }

            dt.ExtNotDeletedRowsForeach(r =>
            {
                if (MyUtility.Convert.GetBool(r["sel"]))
                {
                    string addUpdate = string.Empty;

                    if (r["UniqueKey"].Empty())
                    {
                        r["AddName"] = Env.User.UserID;
                        r["AddDate"] = now;
                    }
                    else
                    {
                        r["EditName"] = Env.User.UserID;
                        r["EditDate"] = now;
                    }

                    r["ReportDate"] = MyUtility.Convert.GetDate(now).ToYYYYMMDD();

                    string sql = $@"

DECLARE @OutputTbl TABLE (ID bigint)
if (@tableName = 'NewSentReport')
begin
    IF EXISTS(select 1 FROM dbo.NewSentReport WHERE ExportID = '{r["ExportID"]}' and PoID = '{r["PoID"]}' and Seq1 = '{r["Seq1"]}' and Seq2 = '{r["Seq2"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}')
    Begin
        Update dbo.NewSentReport SET ReportDate = getdate(), EditName = '{Env.User.UserID}' ,EditDate = getdate() 
        output inserted.Ukey into @OutputTbl
        WHERE ExportID = '{r["ExportID"]}' and PoID = '{r["PoID"]}' and Seq1 = '{r["Seq1"]}' and Seq2 = '{r["Seq2"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}'
    End
    Else
     Begin
        Insert Into dbo.[NewSentReport]
         (
           [ExportID]
          ,[PoID]
          ,[Seq1]
          ,[Seq2]
          ,[ReportDate]
          ,[AddDate]
          ,[AddName]
          ,[DocumentName]
          ,[BrandID]
          ,[UniqueKey]
        )
        output inserted.Ukey into @OutputTbl
        Values(
          '{r["ExportID"]}'
         ,'{r["PoID"]}'
         ,'{r["Seq1"]}'
         ,'{r["Seq2"]}'
         ,getdate()
         ,getdate()
         ,'{Env.User.UserID}'
         ,'{this.drBasic["DocumentName"]}'
         ,'{this.drBasic["BrandID"]}'
         ,'{r["ExportID"]}'+'_'+'{r["PoID"]}'+'_'+'{r["Seq1"]}'+'_'+'{r["Seq2"]}'+'_'+'{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}'
        )
     End
    
    INSERT INTO GASAClip (
        [PKey]
        ,[TableName]
        ,[UniqueKey]
        ,[SourceFile]
        ,[Description]
        ,[AddName]
        ,[AddDate]
        ,[FactoryID])
    SELECT files.Pkey, @tableName, '{r["ExportID"]}'+'_'+'{r["PoID"]}'+'_'+'{r["Seq1"]}'+'_'+'{r["Seq2"]}'+'_'+'{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}', files.FileName, 'File Upload', @UserID, getdate(), '{r["FactoryID"]}'
    FROM @OutputTbl
    Outer Apply(
        select [Pkey] = SUBSTRING(Data,0,11),FileName = SUBSTRING(Data,12,len(Data)-11) from splitstring(@ClipPkey,'?')
    )files
END
ELSE
BEGIN
    IF EXISTS(select 1 FROM dbo.ExportRefnoSentReport WHERE ExportID = '{r["ExportID"]}' and BrandRefno = '{r["BrandRefno"]}' and ColorID = '{r["ColorID"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}')
    Begin
        Update dbo.ExportRefnoSentReport SET ReportDate = getdate(), EditName = '{Env.User.UserID}' ,EditDate = getdate() 
        output inserted.Ukey into @OutputTbl
        WHERE ExportID = '{r["ExportID"]}' and BrandRefno = '{r["BrandRefno"]}' and ColorID = '{r["ColorID"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}'
    End
    Else
     Begin
        Insert Into dbo.[ExportRefnoSentReport]
         (
           [ExportID]
          ,[BrandRefno]
          ,[ColorID]
          ,[ReportDate]
          ,[AddDate]
          ,[AddName]
          ,[DocumentName]
          ,[BrandID]
          ,[UniqueKey]
        )
        output inserted.Ukey into @OutputTbl
        Values(
          '{r["ExportID"]}'
         ,'{r["BrandRefno"]}'
         ,'{r["ColorID"]}'
         ,getdate()
         ,getdate()
         ,'{Env.User.UserID}'
         ,'{this.drBasic["DocumentName"]}'
         ,'{this.drBasic["BrandID"]}'
         ,'{r["ExportID"]}'+'_'+'{r["BrandRefno"]}'+'_'+'{r["ColorID"]}'+'_'+'{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}'
        )
     End
    
     INSERT INTO GASAClip (
        [PKey]
        ,[TableName]
        ,[UniqueKey]
        ,[SourceFile]
        ,[Description]
        ,[AddName]
        ,[AddDate]
        ,[FactoryID])
    SELECT files.Pkey, @tableName, '{r["ExportID"]}'+'_'+'{r["BrandRefno"]}'+'_'+'{r["ColorID"]}'+'_'+'{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}', files.FileName, 'File Upload', @UserID, getdate(), '{r["FactoryID"]}'
    FROM @OutputTbl
    Outer Apply(
        select [Pkey] = SUBSTRING(Data,0,11),FileName = SUBSTRING(Data,12,len(Data)-11) from splitstring(@ClipPkey,'?')
    )files
END

select UniqueKey from {tableName}
where ukey in (SELECT TOP 1 ID FROM @OutputTbl)

";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("ClipPkey", string.Join("?", pkeys)),
                        new SqlParameter("tableName", tableName),
                    };
                    DataTable dtUkey = new DataTable();
                    var result = DBProxy.Current.Select(string.Empty, sql, plis, out dtUkey);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    r["UniqueKey"] = dtUkey.Rows[0][0];
                }
            });

            dt.AcceptChanges();
            MyUtility.Msg.InfoBox("Update Success!");
        }

        /// <summary>
        /// Get PK string without prefix.
        /// </summary>
        /// <returns>string without prefix.</returns>
        private string GetPKeyPre()
        {
            var dtm_sys = DateTime.Now;

            string pkey = string.Empty;
            pkey += CHARs[dtm_sys.Year % 100].ToString() + CHARs[dtm_sys.Month].ToString() + CHARs[dtm_sys.Day].ToString();
            pkey += CHARs[dtm_sys.Hour].ToString();
            pkey += CHARs[dtm_sys.Minute / CHARs.Length].ToString() + CHARs[dtm_sys.Minute % CHARs.Length].ToString();
            pkey += CHARs[dtm_sys.Second / CHARs.Length].ToString() + CHARs[dtm_sys.Second % CHARs.Length].ToString();

            return pkey;
        }

        private void CboDocumentname_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> parmes = new List<SqlParameter>();
            parmes.Add(new SqlParameter("@Documentname", this.cboDocumentname.Text));
            string sql = "select BrandID From MaterialDocument WHERE Documentname = @Documentname and FileRule in ('4','5') and junk = 0";
            var result = DBProxy.Current.Select(string.Empty, sql, parmes, out dt);
            if (result && dt.Rows.Count == 1)
            {
                this.txtBrand1.Text = dt.Rows[0]["BrandID"].ToString();
            }
            else
            {
                this.txtBrand1.Text = string.Empty;
            }
        }
    }
}
