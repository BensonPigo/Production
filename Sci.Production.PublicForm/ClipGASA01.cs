using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Production.Class.Command;
using Sci.Production.Class;
using System.Data.SqlClient;
using Sci.Production.Prg.Entity;
using Sci.Data;
using static Ict.Win.UI.DataGridView;
using Ict.Win.Defs;
using System.Diagnostics;
using Sci.Production.Prg;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// Clip01 subpage Class.
    /// </summary>
    public partial class ClipGASA01 : Sci.Win.Tools.BaseGrid
    {
        static string CHARs = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// grid column of filename.
        /// </summary>
        private DataGridViewColumn col_file;

        /// <summary>
        /// Open file dialog object.
        /// </summary>
        private OpenFileDialog dialog;

        /// <summary>
        /// Login user information.
        /// </summary>
        private IUserInfo _sysuser;

        /// <summary>
        /// Clip directory string.
        /// </summary>
        private string _clipdir;

        /// <summary>
        /// DataTable of data source.
        /// </summary>
        private CLIPGASADataTable _datas;

        /// <summary>
        /// new add data.
        /// </summary>
        private IList<CLIPGASARow> _inserteds;

        /// <summary>
        /// File path of user.
        /// </summary>
        private string _openpath;

        /// <summary>
        /// LimitedClip
        /// </summary>
        private string _limitedClip;

        /// <summary>
        /// AlianClipConnectionName
        /// </summary>
        private string _alianClipConnectionName;

        //// properties

        /// <summary>
        /// Field of table name.
        /// </summary>
        private string _tablename;

        /// <summary>
        /// Property of table name.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TableName
        {
            get { return this._tablename ?? string.Empty; }
        }

        /// <summary>
        /// Field of user code.
        /// </summary>
        private string _uid;

        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UID
        {
            get { return this._uid ?? string.Empty; }
        }

        /// <inheritdoc />
        internal IList<CLIPGASARow> Inserteds
        {
            get { return this._inserteds; }
        }

        /// <inheritdoc />
        protected ClipGASA01()
        {
            this.InitializeComponent();
            this.Text = "Add Clip File";
            this.grid.IsEditable = true;
            this.grid.IsEditingReadOnly = false;

            var colFile = new DataGridViewGeneratorTextColumnSettings()
            {
                CellMouseClick = this.UI_grid_CellMouseClick,
                EditingMouseDown = this.UI_grid_EditCellMouseClick,
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .Serial(header: "No.")
                .Text("LOCALFILE", header: "File Name", iseditingreadonly: true, settings: colFile).Get(out this.col_file)
                .Text("DESCRIPTION", header: "Description")
                ;

            this.save.Click += this.UI_btnSave_Click;
            this.close.Click += this.UI_btnClose_Click;
        }

        private DataRow _dr;

        /// <inheritdoc />
        public ClipGASA01(string tablename, string uid, string yyyymm, string path, string limitedClip, string alianClipConnectionName, DataRow dr)
            : this()
        {
            this._tablename = tablename;
            this._uid = uid;
            this._clipdir = path;
            this._limitedClip = limitedClip;
            this._alianClipConnectionName = alianClipConnectionName;
            this._dr = dr;
        }

        /// <inheritdoc />
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DualResult result;

            if (!Env.DesignTime)
            {
                if (!(result = this.Initialize()))
                {
                    this.ShowErr(result);

                    this.UI_btnClose_Click(null, null);
                    return;
                }

                this._datas = new CLIPGASADataTable();
                for (int i = 0; i < 10; ++i)
                {
                    var data = this._datas.NewCLIPRow();
                    data.TABLENAME = this.TableName;
                    data.UNIQUEKEY = this.UID;
                    data.ADDNAME = this._sysuser != null ? this._sysuser.UserID : null;
                    data.PKEY = i.ToString("0000000000");
                    data.LOCALFILE = null;

                    this._datas.AddCLIPRow(data);
                }

                this.SetGrid(this._datas);
            }

            int freeWidth = this.grid.Width - this.grid.RowHeadersWidth - this.grid.Columns[0].Width - 5;
            this.grid.Columns[1].Width = (int)(freeWidth * 0.5);
            this.grid.Columns[2].Width = (int)(freeWidth * 0.5);
        }

        /// <inheritdoc />
        protected override void OnFormDispose()
        {
            if (this.dialog != null)
            {
                this.dialog.Dispose();
            }

            base.OnFormDispose();
        }

        private void UI_grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            if (e.RowIndex == -1)
            {
                return;
            }

            if (this.col_file.Index == e.ColumnIndex)
            {
                var view = this.grid.GetData<DataRowView>(e.RowIndex);
                if (view == null)
                {
                    return;
                }

                var data = (CLIPGASARow)view.Row;

                try
                {
                    if (this.dialog == null)
                    {
                        this.dialog = new OpenFileDialog();
                    }

                    if (this.dialog.ShowDialog() == DialogResult.OK)
                    {
                        var fileinfo = new FileInfo(this.dialog.FileName);
                        long size = fileinfo.Length;

                        data.SOURCEFILE = fileinfo.Name;
                        data.LOCALFILE = fileinfo.FullName;
                        data.SIZE = size;

                        this._openpath = fileinfo.DirectoryName;
                    }
                }
                catch (Exception ex)
                {
                    this.ShowErr("選取並設定上傳檔案時發生錯誤。", ex);
                }
            }
        }

        private void UI_grid_EditCellMouseClick(object sender, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            if (e.RowIndex == -1)
            {
                return;
            }

            if (this.col_file.Index == e.ColumnIndex)
            {
                var view = this.grid.GetData<DataRowView>(e.RowIndex);
                if (view == null)
                {
                    return;
                }

                var data = (CLIPGASARow)view.Row;

                try
                {
                    if (this.dialog == null)
                    {
                        this.dialog = new OpenFileDialog();
                    }

                    if (this.dialog.ShowDialog() == DialogResult.OK)
                    {
                        var fileinfo = new FileInfo(this.dialog.FileName);
                        long size = fileinfo.Length;

                        data.SOURCEFILE = fileinfo.Name;
                        data.LOCALFILE = fileinfo.FullName;
                        data.SIZE = size;

                        this._openpath = fileinfo.DirectoryName;
                    }
                }
                catch (Exception ex)
                {
                    this.ShowErr("選取並設定上傳檔案時發生錯誤。", ex);
                }
            }
        }

        /// <summary>
        /// The Save button event method.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        private void UI_btnSave_Click(object sender, EventArgs e)
        {
            DualResult result;
            this.grid.EndEdit();

            var datas = new List<CLIPGASARow>();

            foreach (var it in this._datas)
            {
                if (it.IsLOCALFILENull() || it.LOCALFILE.Length == 0)
                {
                    continue;
                }

                datas.Add(it);
            }

            if (datas.Count == 0)
            {
                this.ShowInfo("請選取上傳檔案。");
                return;
            }

            if (datas.Any(row =>
                string.IsNullOrWhiteSpace(row.Field<string>("SOURCEFILE")) == false &&
                string.IsNullOrWhiteSpace(row.Field<string>("DESCRIPTION")) == true))
            {
                this.ShowInfo("Please input [Description] for each File.");
                return;
            }

            DataTable dtSourceFileMaxLen;
            Sci.Data.DBProxy.Current.Select(this._alianClipConnectionName, @"
            SELECT CHARACTER_MAXIMUM_LENGTH 
            FROM INFORMATION_SCHEMA.Columns 
            Where Table_Name = 'GASAClip' and COLUMN_Name='SourceFile'", out dtSourceFileMaxLen);

            int sourceFileLen = 100;

            if (dtSourceFileMaxLen != null && dtSourceFileMaxLen.Rows.Count > 0)
            {
                sourceFileLen = (int)dtSourceFileMaxLen.AsEnumerable().Select(r => r["CHARACTER_MAXIMUM_LENGTH"]).FirstOrDefault();
            }

            var lenCheck = datas.Where(r => r.SOURCEFILE.ToString().Length > sourceFileLen).Select(r => r.SOURCEFILE).ToList();

            if (lenCheck.Count() > 0)
            {
                this.ShowErr($"File Name can not more than {sourceFileLen} characters");
                return;
            }

            int ix = 0;
            string[] pkeys = this.GetPKeys(datas.Count);
            var dtm_sys = DateTime.Now;
            string yyyymm = dtm_sys.ToString("yyyyMM");
            string dir = Path.Combine(this._clipdir, yyyymm);

            foreach (var it in datas)
            {
                it.PKEY = pkeys[ix];
                var localfile = it.LOCALFILE;
                string newFileName = this._tablename + pkeys[ix] + Path.GetExtension(it.LOCALFILE);
                string filename = this.GetClipFileName(it);
                string saveFilePath = dir;

                // 限制檔案大小
                var fileInfo = new FileInfo(it.LOCALFILE);
                if (fileInfo.Length > 15 * 1024 * 1024)
                {
                    MyUtility.Msg.WarningBox("File size cannot exceed 15 MB limit!");
                    return;
                }

                // call API上傳檔案到Trade
                lock (FileDownload_UpData.UploadFile($"{PmsWebAPI.PMSAPApiUri}/api/FileUpload/PostFile", saveFilePath, newFileName, it.LOCALFILE))
                {
                }

                ++ix;
            }

            if (this._tablename == "UASentReport")
            {
                foreach (var it in datas)
                {
                    this._dr["TestReport"] = MyUtility.Convert.GetDate(dtm_sys).ToYYYYMMDD();

                    string addUpdate = string.Empty;

                    string sql = $@"

                    DECLARE @OutputTbl TABLE (ID bigint)
                    IF EXISTS(select 1 FROM dbo.UASentReport WHERE BrandRefno = '{this._dr["BrandRefno"]}' and ColorID = '{this._dr["ColorID"]}' and SuppID = '{this._dr["SuppID"]}' and DocumentName = '{this._dr["BasicDocumentName"]}' and BrandID = '{this._dr["BasicBrandID"]}')
                    Begin
                        Update dbo.UASentReport SET  TestReport = getdate(), EditName = '{Env.User.UserID}' ,EditDate = getdate() {this._dr["updateCol_Where"]} 
                        output inserted.Ukey into @OutputTbl
                        WHERE BrandRefno = '{this._dr["BrandRefno"]}' and ColorID = '{this._dr["ColorID"]}' and SuppID = '{this._dr["SuppID"]}' and DocumentName = '{this._dr["BasicDocumentName"]}' and BrandID = '{this._dr["BasicBrandID"]}'
                    End
                    Else
                     Begin
                        Insert Into dbo.[UASentReport]
                         (
                           [BrandRefno]
                          ,[ColorID]
                          ,[SuppID]
                          ,[TestReport]
                          ,[TestReportTestDate]
                          ,[AddDate]
                          ,[AddName]
                          ,[DocumentName]
                          ,[BrandID]
                          ,[TestSeasonID]
                          ,[DueSeason]
                          ,[DueDate]
                          ,[UniqueKey]
                        )
                        output inserted.Ukey into @OutputTbl
                        Values(
                          '{this._dr["BrandRefno"]}'
                         ,'{this._dr["ColorID"]}'
                         ,'{this._dr["SuppID"]}'
                         ,getdate()
                         ,{(MyUtility.Check.Empty(this._dr["TestReportTestDate"]) ? "null" : $"'{this._dr["TestReportTestDate"]}'")}
                         ,getdate()
                         ,'{Env.User.UserID}'
                         ,'{this._dr["BasicDocumentName"]}'
                         ,'{this._dr["BasicBrandID"]}'
                         ,'{this._dr["TestSeasonID"]}'
                         ,'{this._dr["DueSeason"]}'
                         ,{(MyUtility.Check.Empty(this._dr["DueDate"]) ? "null" : $"'{this._dr["DueDate"]}'")}
                         ,'{this._dr["BrandRefno"]}'+'_'+'{this._dr["ColorID"]}'+'_'+'{this._dr["SuppID"]}'+'_'+ '{this._dr["BasicDocumentName"]}'+'_'+'{this._dr["BasicBrandID"]}'
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
                    SELECT @ClipPkey, 'UASentReport', '{this._dr["BrandRefno"]}'+'_'+'{this._dr["ColorID"]}'+'_'+'{this._dr["SuppID"]}'+'_'+ '{this._dr["BasicDocumentName"]}'+'_'+'{this._dr["BasicBrandID"]}', @sourceFileName, 'File Upload', @UserID, getdate(),'{this._dr["FactoryID"]}'
                     FROM @OutputTbl

                    select UniqueKey from {it.TABLENAME}
                    where ukey in (SELECT TOP 1 ID FROM @OutputTbl)";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("ClipPkey", it.PKEY),
                        new SqlParameter("tableName", it.TABLENAME),
                        new SqlParameter("sourceFileName", it.SOURCEFILE),
                    };
                    DataTable dtUkey = new DataTable();
                    var result2 = DBProxy.Current.Select(null, sql, plis, out dtUkey);
                    if (!result2)
                    {
                        this.ShowErr(result2);
                        return;
                    }

                    this._dr["UniqueKey"] = dtUkey.Rows[0][0];
                }
            }
            else
            {
                foreach (var it in datas)
                {
                    string addUpdate = string.Empty;

                    if (this._dr["UniqueKey"].Empty())
                    {
                        this._dr["AddName"] = Env.User.UserID;
                        this._dr["AddDate"] = dtm_sys;
                    }
                    else
                    {
                        this._dr["EditName"] = Env.User.UserID;
                        this._dr["EditDate"] = dtm_sys;
                    }

                    this._dr["ReportDate"] = MyUtility.Convert.GetDate(dtm_sys).ToYYYYMMDD();

                    string sql = $@"

DECLARE @OutputTbl TABLE (ID bigint)
if (@tableName = 'NewSentReport')
begin
    IF EXISTS(select 1 FROM dbo.NewSentReport WHERE ExportID = '{this._dr["ExportID"]}' and PoID = '{this._dr["PoID"]}' and Seq1 = '{this._dr["Seq1"]}' and Seq2 = '{this._dr["Seq2"]}' and DocumentName = '{this._dr["BasicDocumentName"]}' and BrandID = '{this._dr["BasicBrandID"]}')
    Begin
        Update dbo.NewSentReport SET ReportDate = getdate(), EditName = '{Env.User.UserID}' ,EditDate = getdate() 
        output inserted.Ukey into @OutputTbl
        WHERE ExportID = '{this._dr["ExportID"]}' and PoID = '{this._dr["PoID"]}' and Seq1 = '{this._dr["Seq1"]}' and Seq2 = '{this._dr["Seq2"]}' and DocumentName = '{this._dr["BasicDocumentName"]}' and BrandID = '{this._dr["BasicBrandID"]}'
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
          '{this._dr["ExportID"]}'
         ,'{this._dr["PoID"]}'
         ,'{this._dr["Seq1"]}'
         ,'{this._dr["Seq2"]}'
         ,getdate()
         ,getdate()
         ,'{Env.User.UserID}'
         ,'{this._dr["BasicDocumentName"]}'
         ,'{this._dr["BasicBrandID"]}'
         ,'{this._dr["ExportID"]}'+'_'+'{this._dr["PoID"]}'+'_'+'{this._dr["Seq1"]}'+'_'+'{this._dr["Seq2"]}'+'_'+'{this._dr["BasicDocumentName"]}'+'_'+'{this._dr["BasicBrandID"]}'
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
    SELECT @ClipPkey, @tableName, '{this._dr["ExportID"]}'+'_'+'{this._dr["PoID"]}'+'_'+'{this._dr["Seq1"]}'+'_'+'{this._dr["Seq2"]}'+'_'+'{this._dr["BasicDocumentName"]}'+'_'+'{this._dr["BasicBrandID"]}', @sourceFileName, 'File Upload', @UserID, getdate(),'{this._dr["FactoryID"]}'
    FROM @OutputTbl
END
ELSE
BEGIN
    IF EXISTS(select 1 FROM dbo.ExportRefnoSentReport WHERE ExportID = '{this._dr["ExportID"]}' and BrandRefno = '{this._dr["BrandRefno"]}' and ColorID = '{this._dr["ColorID"]}' and DocumentName = '{this._dr["BasicDocumentName"]}' and BrandID = '{this._dr["BasicBrandID"]}')
    Begin
        Update dbo.ExportRefnoSentReport SET ReportDate = getdate(), EditName = '{Env.User.UserID}' ,EditDate = getdate() 
        output inserted.Ukey into @OutputTbl
        WHERE ExportID = '{this._dr["ExportID"]}' and BrandRefno = '{this._dr["BrandRefno"]}' and ColorID = '{this._dr["ColorID"]}' and DocumentName = '{this._dr["BasicDocumentName"]}' and BrandID = '{this._dr["BasicBrandID"]}'
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
          '{this._dr["ExportID"]}'
         ,'{this._dr["BrandRefno"]}'
         ,'{this._dr["ColorID"]}'
         ,getdate()
         ,getdate()
         ,'{Env.User.UserID}'
         ,'{this._dr["BasicDocumentName"]}'
         ,'{this._dr["BasicBrandID"]}'
         ,'{this._dr["ExportID"]}'+'_'+'{this._dr["BrandRefno"]}'+'_'+'{this._dr["ColorID"]}'+'_'+'{this._dr["BasicDocumentName"]}'+'_'+'{this._dr["BasicBrandID"]}'
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
    SELECT @ClipPkey, @tableName, '{this._dr["ExportID"]}'+'_'+'{this._dr["BrandRefno"]}'+'_'+'{this._dr["ColorID"]}'+'_'+'{this._dr["BasicDocumentName"]}'+'_'+'{this._dr["BasicBrandID"]}', @sourceFileName, 'File Upload', @UserID, getdate(),'{this._dr["FactoryID"]}'
    FROM @OutputTbl
END

select UniqueKey from {it.TABLENAME}
where ukey in (SELECT TOP 1 ID FROM @OutputTbl)

";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("ClipPkey", it.PKEY),
                        new SqlParameter("tableName", it.TABLENAME),
                        new SqlParameter("sourceFileName", it.SOURCEFILE),
                    };
                    DataTable dtUkey = new DataTable();
                    var result1 = DBProxy.Current.Select(string.Empty, sql, plis, out dtUkey);
                    if (!result1)
                    {
                        this.ShowErr(result1);
                        return;
                    }

                    this._dr["UniqueKey"] = dtUkey.Rows[0][0];
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// The Close button event method.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        private void UI_btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Initialize the default value.
        /// </summary>
        /// <returns>Result of process.</returns>
        private DualResult Initialize()
        {
            DualResult result;

            this._sysuser = Env.User;

            if (this.TableName.Length == 0)
            {
                return new DualResult(false, "尚未指定 TableName。");
            }

            return Result.True;
        }

        private string GetClipFileName(CLIPGASARow data)
        {
            if (data.IsTABLENAMENull() || data.IsSOURCEFILENull())
            {
                return null;
            }

            return data.TABLENAME + data.PKEY + Path.GetExtension(data.SOURCEFILE);
        }

        private string[] GetPKeys(int count)
        {
            string[] pkeys = new string[count];
            var pkey = this.GetPKeyPre();

            for (int i = 0; i < count; ++i)
            {
                pkeys[i] = pkey + i.ToString("00");
            }

            return pkeys;
        }

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
    }
}