using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Ict;
using Ict.Win;
using Sci.Win.Tools;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Prg.Entity;
using Ict.Win.Defs;
using Sci.Data;
using Sci.Win;
using System.Data.SqlClient;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// Clip subpage class.
    /// </summary>
    public partial class Clip : Sci.Win.Tools.BaseGrid
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, string[]> mappingClipDic;

        private string CHARs = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dir"></param>
        /// <param name="clipDir"></param>
        /// <returns></returns>
        internal static DualResult GetClipDirYYYYMM(Sci.Win.SYS.CLIPRow data, out string dir, string clipDir = null)
        {
            clipDir = clipDir ?? PrivUtils.GetClipRootPath(data.TABLENAME, _alianClipConnectionName);
            dir = null;
            try
            {
                string yyyymm = null;
                yyyymm = data.ADDDATE.ToString("yyyyMM");
                dir = Path.Combine(clipDir, yyyymm);
            }
            catch (Exception ex)
            {
                return Result.F(ex);
            }

            return Result.True;
        }

        private static Dictionary<string, string[]> FillMappingClipDic(string alianClipConnectionName = "")
        {
            Dictionary<string, string[]> mapping = null;
            DataTable table;
            DualResult result = Sci.Data.DBProxy.Current.Select(_alianClipConnectionName, "Select * from ClipMapping", out table);
            mapping = table.AsEnumerable()
                .GroupBy(r => r["NewTableName"].ToString().TrimEnd().ToUpper())
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(subRows => subRows["OldTableName"].ToString()).ToArray(),
                    StringComparer.OrdinalIgnoreCase)
                ;
            return mapping;
        }

        /// <summary>
        /// Constructor of Clip.
        /// </summary>
        protected Clip()
        {
            this.InitializeComponent();

            this.grid.MultiSelect = true;

            this.Text = "Clip"; 
            this.grid.IsEditable = true;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("SOURCEFILE", header: "File Name", width: Widths.AnsiChars(35))
                .Text("DESCRIPTION", header: "Description", width: Widths.UnicodeChars(25), settings: new DataGridViewGeneratorTextColumnSettings()
                {
                    CharacterCasing = CharacterCasing.Normal,
                })
                // .Numeric("SIZE", header: "Size", integer_places: 10)
                .Text("PKEY", header: "UKey", width: Widths.AnsiChars(10))
                .Text("ADDDATE", header: "Created by", width: Widths.AnsiChars(25), settings: new DataGridViewGeneratorTextColumnSettings()
                {
                    CellFormatting = (s, e) =>
                    {
                        var view = this.grid.GetData<DataRowView>(e.RowIndex);
                        if (null == view)
                        {
                            return;
                        }

                        var data = (Sci.Win.SYS.CLIPRow)view.Row;

                        string value = !data.IsADDNAMENull() ? data.ADDNAME : string.Empty;
                        if (!data.IsADDDATENull())
                        {
                            if (0 < value.Length)
                            {
                                value += " ";
                            }

                            value += data.ADDDATE.ToString("yyyy/MM/dd HH:mm");
                        }

                        e.FormattingApplied = true;
                        e.Value = value;
                    },
                });

            this.IsEnableChangeEditMode = false;

            this.nem.Click += this.nem_Click;
            this.remove.Click += this.remove_Click;

            this.openfile.Click += this.openfile_Click;
            this.download.Click += this.download_Click;
            this.mailto.Click += this.mailto_Click;
            this.close.Click += this.close_Click;

            this.grid.CellMouseDoubleClick += this.Grid_CellMouseDoubleClick;
            ;
        }

        private DataRow _dr;

        /// <summary>
        /// Constructor of Clip with parameters.
        /// </summary>
        /// <param name="tablename">Table name.</param>
        /// <param name="uid">Usercode.</param>
        /// <param name="canedit">Flag of CanEdit.</param>
        /// <param name="limitedClip">Limited Clip</param>
        /// <param name="alianClipConnectionName">alian Clip ConnectionName</param>
        public Clip(string tablename, string uid, bool canedit, DataRow dr, string limitedClip = "", string alianClipConnectionName = "", string apiUrlFile = "")
            : this()
        {
            this._tablename = tablename;
            this._uid = uid;
            _limitedClip = limitedClip;
            _alianClipConnectionName = alianClipConnectionName;
            this.mappingClipDic = FillMappingClipDic(_alianClipConnectionName);
            this._dr = dr;
            this.EditMode = canedit;
            this._ApiUrlFile = apiUrlFile;
        }

        private void Grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (MouseButtons.Right == e.Button)
            {
                return;
            }

            if (-1 == e.RowIndex)
            {
                return;
            }

            this.openfile_Click(sender, e);
        }

        /// <summary>
        /// On form loaded method.
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DualResult result;

            if (!Env.DesignTime)
            {
                this.SetButton();

                this._clipdir = PrivUtils.GetClipRootPath(this._tablename, _alianClipConnectionName);

                this.loc.Text = this._clipdir;

                if (!(result = this.Load()))
                {
                    this.ShowErr(result);

                    this.close_Click(null, null);
                    return;
                }
            }
        }

        /// <summary>
        /// On form dispose method.
        /// </summary>
        protected override void OnFormDispose()
        {
            if (null != this.savefiledialog)
            {
                this.savefiledialog.Dispose();
            }

            base.OnFormDispose();
        }

        /// <summary>
        /// Flag of button is enable.
        /// </summary>
        protected bool? _buttonenable;

        /// <summary>
        /// Field of Clip path.
        /// </summary>
        protected string _clipdir;

        /// <summary>
        /// Data collection of gird.
        /// </summary>
        private Sci.Win.SYS.CLIPDataTable _datas;

        /////// <summary>
        /////// Field of first clip time.
        /////// </summary>
        ////protected DateTime? _firstcliptime;

        /////// <summary>
        /////// Clip path string.
        /////// </summary>
        ////protected string _yyyymm;

        /////// <summary>
        /////// Clip FullPath string. ( clipPath / _yyyymm)
        /////// </summary>
        ////private string _fullpath;

        /// <summary>
        /// Save dialog object.
        /// </summary>
        private SaveFileDialog savefiledialog;

        /// <summary>
        /// Field of path.
        /// </summary>
        string _path;

        #region 屬性

        /// <summary>
        /// Field of table name.
        /// </summary>
        private string _tablename;

        /// <summary>
        /// API Url + Path + FileName.
        /// </summary>
        private string _ApiUrlFile;

        /// <summary>
        /// Property of table name.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TableName { get { return this._tablename ?? string.Empty; } }

        /// <summary>
        /// Field of usercode.
        /// </summary>
        private string _uid;

        /// <summary>
        /// Property of Usercode.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UID { get { return this._uid ?? string.Empty; } }

        /// <summary>
        /// Field of limitedClip.
        /// </summary>
        private static string _limitedClip;

        /// <summary>
        /// Property of limitedClip
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LimitedClip { get { return _limitedClip ?? string.Empty; } }

        /// <summary>
        /// Field of AlianClipConnectionName
        /// </summary>
        private static string _alianClipConnectionName;

        /// <summary>
        /// Property of AlianClipConnectionName
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AlianClipConnectionName { get { return _alianClipConnectionName ?? string.Empty; } }

        /////// <summary>
        /////// Property of first clip time.
        /////// </summary>
        ////[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ////public DateTime? FirstClipTime { get { return _firstcliptime; } }

        /////// <summary>
        /////// Property of YYYYMM
        /////// </summary>
        ////[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ////public string YYYYMM { get { return _yyyymm ?? ""; } }

        /////// <summary>
        /////// Property of FullPath.
        /////// </summary>
        ////[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ////public string FullPath { get { return _fullpath ?? ""; } }
        #endregion
        #region 應用函式
        /// <summary>
        /// Set buttons status.
        /// </summary>
        private void SetButton()
        {
            this.nem.Enabled = this.EditMode;

            if (0 < this.gridbs.Count)
            {
                this._buttonenable = true;

                this.remove.Enabled = this.EditMode;
                this.openfile.Enabled = true;
                this.download.Enabled = true;
                // this.mailto.Enabled = true;
            }
            else
            {
                this._buttonenable = false;

                this.remove.Enabled = false;
                this.openfile.Enabled = false;
                this.download.Enabled = false;
                // this.mailto.Enabled = false;
            }
        }

        /// <summary>
        /// Load data method.
        /// </summary>
        /// <returns></returns>
        private DualResult Load()
        {
            DualResult result;

            Sci.Win.SYS.CLIPDataTable datas = null;
            DateTime? firstcliptime = null;
            string yyyymm = null, fullpath = null;
            if (!(result = AsyncHelper.Current.DataLoading(this, () =>
            {
                if (!(result = PrivUtils.GetClips(this.TableName, this.UID, this.LimitedClip, out datas, _alianClipConnectionName)))
                {
                    return result;
                }

                if (0 < datas.Count)
                {
                    firstcliptime = GetFirstClipTime(datas);

                    if (firstcliptime.HasValue)
                    {
                        yyyymm = firstcliptime.Value.ToString("yyyyMM");
                        fullpath = Path.Combine(this._clipdir, yyyymm);

                        if (!(result = PrivUtils.SetClipSize(datas, Path.Combine(this._clipdir, yyyymm))))
                        {
                            Logs.UI.LogErrorByCaller(result);
                        }
                    }
                }

                return Result.True;
            })))
            {
                return result;
            }

            this._datas = datas;
            ////_firstcliptime = firstcliptime;
            ////_yyyymm = yyyymm;
            ////_fullpath = fullpath;
            this._datas.DefaultView.Sort = "AddDate";
            this.SetGrid(datas);

            return Result.True;
        }

        /// <summary>
        /// Get first clip time from current collection.
        /// </summary>
        /// <param name="datas">Current collection.</param>
        /// <returns>First datetime.</returns>
        private static DateTime? GetFirstClipTime(IEnumerable<Sci.Win.SYS.CLIPRow> datas)
        {
            DateTime? firstcliptime = null;

            foreach (var it in datas)
            {
                if (it.IsADDDATENull())
                {
                    continue;
                }

                if (!firstcliptime.HasValue || firstcliptime > it.ADDDATE)
                {
                    firstcliptime = it.ADDDATE;
                }
            }

            return firstcliptime;
        }

        private List<Sci.Win.SYS.CLIPRow> GetSelectItems()
        {
            return this.grid.SelectedRows.Cast<DataGridViewRow>().Select(row => (Sci.Win.SYS.CLIPRow)((DataRowView)row.DataBoundItem).Row).ToList();
        }

        /// <summary>
        /// Get the selected row.
        /// </summary>
        /// <returns>Row of selected.</returns>
        private Sci.Win.SYS.CLIPRow GetSelected()
        {
            return (Sci.Win.SYS.CLIPRow)this.grid.CurrentDataRow;
        }

        /// <summary>
        /// Get file name from specified row.
        /// </summary>
        /// <param name="data">Specified row.</param>
        /// <param name="file">File name string.</param>
        /// <returns>Result of process.</returns>
        private static DualResult GetClipFile(Sci.Win.SYS.CLIPRow data, string clipRootDir, Dictionary<string, string[]> mappingClipDic, out string file)
        {
            file = null;
            DualResult result;

            if (clipRootDir == null || clipRootDir.Length == 0)
            {
                return new DualResult(false, "無法識別 CLIP 存放資料夾。");
            }
            else if (data["AddDate"] == null || data["AddDate"] is DBNull)
            {
                return new DualResult(false, "無法識別 CLIP 存放日期的資料夾。");
            }

            string yyyyMM = ((DateTime)data["AddDate"]).ToString("yyyyMM");
            string clipDirPath = Path.Combine(clipRootDir, yyyyMM);
            string filename;
            string appConfigRoot = Path.Combine(Env.Cfg.ClipDir, yyyyMM);

            // 先用新的方法找
            if (!(result = PrivUtils.GetClipFileName(data, out filename)))
            {
                return result;
            }

            file = Path.Combine(clipDirPath, filename);
            if (File.Exists(file))
            {
                return Result.True;
            }

            // 找不到用舊的找
            string oldFileName;
            if (!(result = GetOlderClipFileName(data, clipDirPath, mappingClipDic, out oldFileName)))
            {
                if (clipRootDir != appConfigRoot)
                {
                    file = Path.Combine(appConfigRoot, filename);
                    if (File.Exists(file))
                    {
                        return Result.True;
                    }

                    if (!(result = GetOlderClipFileName(data, appConfigRoot, mappingClipDic, out oldFileName)))
                    {
                        // return result;
                        return new DualResult(false, "File not found ! \n\n" + filename + "\n" + file);
                    }
                    else
                    {
                        file = oldFileName;
                    }
                }

                // return result;
                return new DualResult(false, "File not found ! \n\n" + filename + "\n" + file);
            }
            else
            {
                file = oldFileName;
            }

            return Result.True;
        }

        /// <summary>
        /// 取得舊版的 Clip 檔名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clipDir"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static DualResult GetOlderClipFileName(Sci.Win.SYS.CLIPRow data, string clipDir, Dictionary<string, string[]> mappingClipDic, out string filename)
        {
            var result = Result.True;
            string newFileName = filename = data.TABLENAME.ToUpper() + "_" + data.PKEY + Path.GetExtension(data.SOURCEFILE);

            string[] oldTables;
            string tablename;
            if (!mappingClipDic.TryGetValue(data.TABLENAME.ToUpper(), out oldTables))
            {
                ////tablename = data.TABLENAME.ToUpper();
                ////filename = tablename + "_" + data.PKEY + Path.GetExtension(data.SOURCEFILE);
                ////string file = Path.Combine(clipDir, filename);
                ////if (File.Exists(file))
                ////{
                ////    return Result.True;
                ////}
                filename = Path.Combine(clipDir, filename);
                if (File.Exists(filename))
                {
                    return Result.True;
                }
                else
                {
                    filename = string.Empty;
                }

                result = Result.F("File not found !", newFileName);
            }
            else
            {
                foreach (var table in oldTables)
                {
                    filename = table + "_" + data.PKEY + Path.GetExtension(data.SOURCEFILE);
                    filename = Path.Combine(clipDir, filename);
                    if (File.Exists(filename))
                    {
                        return Result.True;
                    }
                }

                var notFounds = newFileName + Environment.NewLine + string.Join(Environment.NewLine, oldTables);
                result = Result.F("File not found !", notFounds);
            }

            return result;
        }

        /// <summary>
        /// On grid row changed method.
        /// </summary>
        /// <param name="rowindex">index of row.</param>
        protected override void OnGridRowChanged(int rowindex)
        {
            base.OnGridRowChanged(rowindex);
            if (this._buttonenable != (-1 != rowindex))
            {
                this.SetButton();
            }
        }

        /// <summary>
        /// On grid row mouse double click method.
        /// </summary>
        /// <param name="rowindex">index of row.</param>
        protected override void OnGridRowDoubleClick(int rowindex)
        {
            base.OnGridRowDoubleClick(rowindex);
            this.openfile.PerformClick();
        }
        #endregion

        /// <summary>
        /// New add button is pressed.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        void nem_Click(object sender, EventArgs e)
        {
            using (var frm = new Clip01(this.TableName, this.UID, string.Empty, this._clipdir, this.LimitedClip, _alianClipConnectionName, this._dr))
            {
                frm.ShowDialog();
                this.Load();

                #region 把檔案從trade重新抓下來
                string filePath = MyUtility.GetValue.Lookup($"select [path] from CustomizedClipPath where TableName = '{this._tablename}'");

                // 組ClipPath
                string clippath = MyUtility.GetValue.Lookup($"select ClipPath from System");
                string saveFilePath = clippath + "\\" + DateTime.Now.ToString("yyyyMM");

                string sqlcmd = $@"select 
                [FileName] = TableName + PKey,
                SourceFile
                from Clip
                where TableName = '{this._tablename}' and 
                UniqueKey = '{this._uid}'";
                DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                if (!dualResult)
                {
                    MyUtility.Msg.WarningBox(dualResult.ToString());
                }

                // 先刪除原本下載的檔案
                foreach (DataRow dataRow in dt.Rows)
                {
                    string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                    string deleteFile = Path.Combine(saveFilePath, fileName);
                    if (File.Exists(deleteFile))
                    {
                        File.Delete(deleteFile);
                    }
                }

                // 再下載所有檔案
                foreach (DataRow dataRow in dt.Rows)
                {
                    string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                    lock (FileDownload_UpData.DownloadFileAsync("http://pmsap.sportscity.com.tw:16888/api/FileDownload/GetFile", filePath + "\\" + DateTime.Now.ToString("yyyyMM"), fileName, saveFilePath))
                    {
                    }
                }

                #endregion

                this._path = null;
            }
        }

        private string GetPKeyPre()
        {
            var dtm_sys = DateTime.Now;

            string pkey = string.Empty;
            pkey += this.CHARs[dtm_sys.Year % 100].ToString() + this.CHARs[dtm_sys.Month].ToString() + this.CHARs[dtm_sys.Day].ToString();
            pkey += this.CHARs[dtm_sys.Hour].ToString();
            pkey += this.CHARs[dtm_sys.Minute / this.CHARs.Length].ToString() + this.CHARs[dtm_sys.Minute % this.CHARs.Length].ToString();
            pkey += this.CHARs[dtm_sys.Second / this.CHARs.Length].ToString() + this.CHARs[dtm_sys.Second % this.CHARs.Length].ToString();

            return pkey;
        }

        /// <summary>
        /// Remove button is pressed.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        void remove_Click(object sender, EventArgs e)
        {
            DualResult result;

            var data = this.GetSelected();
            if (null == data)
            {
                return;
            }

            if (data.IsSOURCEFILENull() || this._clipdir.Length == 0 || this._clipdir == null)
            {
                if (!(result = PrivUtils.DeleteClipRow(data, _alianClipConnectionName)))
                {
                    this.ShowErr(result);
                    return;
                }

                this._datas.RemoveCLIPRow(data);
                return;
            }

            string file = string.Empty;

            IList<string> manager;
            if (!(result = PrivUtils.GetManeger(data.ADDNAME, out manager)))
            {
                this.ShowErr(result);
                return;
            }

            // 只有自己、主管、Admin可以刪除檔案
            if (!manager.Contains(Env.User.UserID) && !Env.User.IsAdmin)
            {
                this.ShowErr($"Can not delete this file, addName is {data.ADDNAME}.");
                return;
            }

            string yyyyMM = ((DateTime)data["AddDate"]).ToString("yyyyMM");
            string clipDirPath = Path.Combine(this._clipdir, yyyyMM);
            string appConfigRoot = Path.Combine(Env.Cfg.ClipDir, yyyyMM);
            string filename;

            // 先用新的方法找
            if (!(result = PrivUtils.GetClipFileName(data, out filename)))
            {
                return;
            }

            file = Path.Combine(clipDirPath, filename);

            result = GetClipFile(data, this._clipdir, this.mappingClipDic, out file);
            if (!MsgHelper.Current.Confirm(this, "是否確認刪除檔案 '{0}'。\n".InvariantFormat(data.SOURCEFILE) + file))
            {
                return;
            }

            if (!(result = PrivUtils.DeleteClipRow(data, _alianClipConnectionName)))
            {
                this.ShowErr(result);
            }

            this._datas.RemoveCLIPRow(data);

            // 原本的的 PrivUtils.DeleteClip 有 Bug. 改成在這裡刪除
            if (result.Result)
            {
                try
                {
                    // call API 刪除在Trade上的檔案
                    lock (FileDownload_UpData.DeleteFileAsync(this._ApiUrlFile, clipDirPath, filename))
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("檔案刪除失敗!!! \n" + file + "\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Open file button is pressed.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        void openfile_Click(object sender, EventArgs e)
        {
            DualResult result;

            var data = this.GetSelected();
            if (data == null)
            {
                return;
            }

            string file;
            if (!(result = GetClipFile(data, this._clipdir, this.mappingClipDic, out file)))
            {
                this.ShowErr(result);
                return;
            }

            if (!(result = Utils.OpenFile(file)))
            {
                this.ShowErr(result);
            }
        }
        /// <summary>
        /// Download button is pressed.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        void download_Click(object sender, EventArgs e)
        {
            var data = this.GetSelectItems();
            if (data == null)
            {
                return;
            }

            this.FileDownload(data);
        }

        /// <summary>
        /// Download SingleFile
        /// </summary>
        /// <param name="datas"> STS.CLIPRow list </param>
        private void FileDownload(List<Sci.Win.SYS.CLIPRow> datas)
        {
            DualResult result;

            Dictionary<Sci.Win.SYS.CLIPRow, string> files = new Dictionary<Sci.Win.SYS.CLIPRow, string>();
            foreach (var data in datas)
            {
                string file;
                if (!(result = GetClipFile(data, this._clipdir, this.mappingClipDic, out file)))
                {
                    this.ShowErr(result);
                    return;
                }

                files.Add(data, file);
            }

            try
            {
                var sysuser = Env.User;
                if (this._path == null)
                {
                    if (sysuser != null)
                    {
                        string path;
                        if (!(result = PrivUtils.GetClipPath(sysuser.UserID, out path, _alianClipConnectionName)))
                        {
                            Logs.UI.LogErrorByCaller("取得 CLIPPATH 時發生錯誤。", result);
                        }
                        else
                        {
                            this._path = path ?? string.Empty;
                        }
                    }
                }

                bool singleFile = datas.Count == 1;
                var savedir = string.Empty;
                string fileExistsStr = string.Empty;

                if (singleFile)
                {
                    if (this.savefiledialog == null)
                    {
                        this.savefiledialog = new SaveFileDialog();
                    }

                    if (this._path != null && this._path.Length > 0)
                    {
                        this.savefiledialog.FileName = Path.Combine(this._path, datas[0].SOURCEFILE);
                    }
                    else
                    {
                        this.savefiledialog.FileName = datas[0].SOURCEFILE;
                    }

                    // user電腦可能設定隱藏副檔名, 會導致 saveFileDialog 沒有副檔名選單, 存了之後擋案也沒副檔名
                    // dialog.FileName = filename;
                    this.savefiledialog.SupportMultiDottedExtensions = true;
                    this.savefiledialog.Filter = "All files (*.*)|*.*";

                    this.savefiledialog.DefaultExt = System.IO.Path.GetExtension(datas[0].SOURCEFILE);
                    if (this.savefiledialog.DefaultExt.Length > 0)
                    {
                        this.savefiledialog.AddExtension = true;
                        this.savefiledialog.Filter = this.savefiledialog.DefaultExt + " files (*." + this.savefiledialog.DefaultExt + ")|*." + this.savefiledialog.DefaultExt + "|" + this.savefiledialog.Filter;
                        this.savefiledialog.FilterIndex = 0;
                    }

                    if (this.savefiledialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    string savefile = this.savefiledialog.FileName;

                    savedir = Path.GetDirectoryName(savefile);
                    if (!Directory.Exists(savedir))
                    {
                        Directory.CreateDirectory(savedir);
                    }

                    if (File.Exists(savefile))
                    {
                        fileExistsStr += $"{savefile}{Environment.NewLine}";
                    }
                    else
                    {
                        File.Copy(files[datas[0]], savefile, true);
                    }
                }
                else
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();

                    if (this._path != null && this._path.Length > 0)
                    {
                        fbd.SelectedPath = this._path;
                    }

                    if (fbd.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    savedir = fbd.SelectedPath;
                    if (!Directory.Exists(savedir))
                    {
                        Directory.CreateDirectory(savedir);
                    }

                    foreach (var item in files)
                    {
                        string savefile = Path.Combine(savedir, item.Key.SOURCEFILE);
                        if (File.Exists(savefile))
                        {
                            fileExistsStr += $"{savefile}{Environment.NewLine}";
                        }
                        else
                        {
                            File.Copy(item.Value, savefile, true);
                        }
                    }
                }

                if (fileExistsStr != string.Empty)
                {
                    MessageBox.Show($"Files already exists:{Environment.NewLine}" + fileExistsStr, "Exists file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (savedir != this._path)
                {
                    this._path = savedir;

                    if (sysuser != null)
                    {
                        if (!(result = PrivUtils.SetClipPath(sysuser.UserID, this._path, _alianClipConnectionName)))
                        {
                            Logs.UI.LogErrorByCaller("設定 CLIPPATH 時發生錯誤。", result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErr("進行 CLIP 檔案下載處理時發生錯誤。", ex);
            }
        }

        /// <summary>
        /// MailTo button is pressed.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        void mailto_Click(object sender, EventArgs e)
        {
            DualResult result;

            var data = this.GetSelected();
            if (null == data)
            {
                return;
            }

            string file;
            if (!(result = GetClipFile(data, this._clipdir, this.mappingClipDic, out file)))
            {
                this.ShowErr(result);
                return;
            }

            var frm = new MailTo(Sci.Env.Cfg.MailFrom, data.SOURCEFILE, file, data.IsDESCRIPTIONNull() ? null : data.DESCRIPTION);
            frm.ShowDialog(this);
        }

        /// <summary>
        /// Close button is pressed.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Event Args information.</param>
        void close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

                /// <summary>
        /// 不顯示畫面, 直接夾檔
        /// </summary>
        /// <param name="tablename">Table name</param>
        /// <param name="uniqueID">a unique value in {tablename} </param>
        /// <param name="userID">who add clip (refer to Pass1.ID )</param>
        /// <param name="description"> description of files </param>
        /// <param name="alianClipConnectionName"> alian Clip Connection Name </param>
        /// <param name="clipFiles">one or more strings for each file (full path)</param>
        /// <returns>success or not</returns>
        public static DualResult AddAlianClip(string tablename, string uniqueID, string userID, string description, string alianClipConnectionName, params string[] clipFiles)
        {
            DualResult result = Result.True;
            Sci.Win.SYS.CLIPDataTable datas = new Sci.Win.SYS.CLIPDataTable();
            List<Sci.Win.SYS.CLIPRow> clip_data = new List<Sci.Win.SYS.CLIPRow>();
            for (int i = 0; i < clipFiles.Length; i++)
            {
                var fileinfo = new FileInfo(clipFiles[i]);
                var data = datas.NewCLIPRow();
                data.TABLENAME = tablename;
                data.UNIQUEKEY = uniqueID;
                data.ADDNAME = userID;
                data.PKEY = i.ToString("0000000000");
                data.DESCRIPTION = description;
                data.SOURCEFILE = fileinfo.Name;
                data.LOCALFILE = fileinfo.FullName;
                data.SIZE = fileinfo.Length;

                clip_data.Add(data);
            }

            result = AddClip(clip_data, alianClipConnectionName: alianClipConnectionName);
            datas.Dispose();

            return result;
        }

        /// <summary>
        /// 不顯示畫面, 直接夾檔
        /// </summary>
        /// <param name="tablename">Table name</param>
        /// <param name="uniqueID">a unique value in {tablename} </param>
        /// <param name="userID">who add clip (refer to Pass1.ID )</param>
        /// <param name="description"> description of files </param>
        /// <param name="clipFiles">one or more strings for each file (full path)</param>
        /// <returns>success or not</returns>
        public static DualResult AddClip(string tablename, string uniqueID, string userID, string description, params string[] clipFiles)
        {
            DualResult result = Result.True;
            Sci.Win.SYS.CLIPDataTable datas = new Sci.Win.SYS.CLIPDataTable();
            List<Sci.Win.SYS.CLIPRow> clip_data = new List<Sci.Win.SYS.CLIPRow>();
            for (int i = 0; i < clipFiles.Length; i++)
            {
                var fileinfo = new FileInfo(clipFiles[i]);
                var data = datas.NewCLIPRow();
                data.TABLENAME = tablename;
                data.UNIQUEKEY = uniqueID;
                data.ADDNAME = userID;
                data.PKEY = i.ToString("0000000000");
                data.DESCRIPTION = description;
                data.SOURCEFILE = fileinfo.Name;
                data.LOCALFILE = fileinfo.FullName;
                data.SIZE = fileinfo.Length;

                clip_data.Add(data);
            }

            result = AddClip(clip_data);
            datas.Dispose();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="dir"></param>
        /// <param name="_yyyymm"></param>
        /// <param name="addTime"></param>
        /// <param name="alianClipConnectionName"></param>
        /// <returns>success or not</returns>
        internal static DualResult AddClip(IList<Sci.Win.SYS.CLIPRow> datas, string dir = null, string _yyyymm = null, DateTime? addTime = null, string alianClipConnectionName = "")
        {
            DualResult result = Result.True;
            var dtm_sys = addTime ?? DateTime.Now;
            foreach (var it in datas)
            {
                it.ADDDATE = dtm_sys;
            }

            string yyyymm = _yyyymm;
            if (yyyymm == null || yyyymm.Length == 0)
            {
                yyyymm = dtm_sys.ToString("yyyyMM");
            }

            if (dir == null || dir.Length == 0)
            {
                string clipDir = null;
                if (!(result = PrivUtils.GetClipDir(out clipDir)))
                {
                    return result;
                }

                dir = Path.Combine(clipDir, yyyymm);
            }

            if (!(result = PrivUtils.AddClips(datas, dir, alianClipConnectionName)))
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="dir"></param>
        /// <param name="_yyyymm"></param>
        /// <param name="addTime"></param>
        /// <param name="alianClipConnectionName"></param>
        /// <returns>success or not</returns>
        
        /// <summary>
        /// 不顯示畫面, 直接夾檔
        /// </summary>
        /// <param name="tablename">Table name</param>
        /// <param name="uniqueID">a unique value in {tablename} </param>
        /// <param name="userID">who add clip (refer to Pass1.ID )</param>
        /// <param name="description"> description of files </param>
        /// <param name="clipFiles">one or more strings for each file (full path)</param>
        /// <returns>success or not</returns>
        public static DualResult AddLimitedClip(string tablename, string uniqueID, string userID, string description, string limitedClip, params string[] clipFiles)
        {
            DualResult result = Result.True;
            Sci.Win.SYS.CLIPDataTable datas = new Sci.Win.SYS.CLIPDataTable();
            List<Sci.Win.SYS.CLIPRow> clip_data = new List<Sci.Win.SYS.CLIPRow>();
            for (int i = 0; i < clipFiles.Length; i++)
            {
                var fileinfo = new FileInfo(clipFiles[i]);
                var data = datas.NewCLIPRow();
                data.TABLENAME = tablename;
                data.UNIQUEKEY = uniqueID;
                data.ADDNAME = userID;
                data.PKEY = i.ToString("0000000000");
                data.DESCRIPTION = description;
                data.SOURCEFILE = fileinfo.Name;
                data.LOCALFILE = fileinfo.FullName;
                data.SIZE = fileinfo.Length;

                clip_data.Add(data);
            }

            result = AddLimitedClip(clip_data, limitedClip: limitedClip);
            datas.Dispose();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="dir"></param>
        /// <param name="_yyyymm"></param>
        /// <param name="addTime"></param>
        ///  <param name="limitedClip"></param>
        /// <returns>success or not</returns>
        internal static DualResult AddLimitedClip(IList<Sci.Win.SYS.CLIPRow> datas, string dir = null, string _yyyymm = null, DateTime? addTime = null, string limitedClip = "")
        {
            DualResult result = Result.True;
            var dtm_sys = addTime ?? DateTime.Now;
            foreach (var it in datas)
            {
                it.ADDDATE = dtm_sys;
            }

            string yyyymm = _yyyymm;
            if (yyyymm == null || yyyymm.Length == 0)
            {
                yyyymm = dtm_sys.ToString("yyyyMM");
            }

            if (dir == null || dir.Length == 0)
            {
                string clipDir = null;
                if (!(result = PrivUtils.GetClipDir(out clipDir)))
                {
                    return result;
                }

                dir = Path.Combine(clipDir, yyyymm);
            }

            if (!(result = PrivUtils.AddClips(datas, dir, _limitedClip))) { return result; }

            return result;
        }

        /// <summary>
        /// 列出所有的附檔
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="uniqueID"></param>
        /// <param name="files"></param>
        /// <param name="alianClipConnectionName"></param>
        /// <returns></returns>
        public static DualResult GetAllClipFiles(string tableName, string uniqueID, out string[] files, string alianClipConnectionName = "")
        {
            files = null;
            var result = Result.True;
            string clipRootDir = null;

            Sci.Win.SYS.CLIPDataTable datas = null;
            if (!(result = PrivUtils.GetClips(tableName, uniqueID, out datas, alianClipConnectionName))) { return result; }

            clipRootDir = PrivUtils.GetClipRootPath(tableName, alianClipConnectionName);

            var allClips = new List<string>();
            var missClips = new List<string>();
            try
            {
                var mappingClipDic = FillMappingClipDic(alianClipConnectionName);
                foreach (var data in datas)
                {
                    string fileFullPath;
                    if (GetClipFile(data, clipRootDir, mappingClipDic, out fileFullPath))
                    {
                        allClips.Add(fileFullPath);
                    }
                    else
                    {
                        missClips.Add(data.SOURCEFILE);
                    }
                }

                files = allClips.ToArray();
            }
            catch (Exception ex)
            {
                return Result.F(ex);
            }

            if (missClips.Count != 0)
            {
                string miss = Environment.NewLine + string.Join(Environment.NewLine, missClips);
                result = Result.F("These files are not found..." + miss);
            }

            return result;
        }

        /// <summary>
        /// 列出所有的附檔
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="uniqueID"></param>
        /// <param name="limitedClip"></param>
        /// <param name="files"></param>
        /// <param name="alianClipConnectionName"></param>
        /// <returns></returns>
        public static DualResult GetAllClipFiles(string tableName, string uniqueID, string limitedClip, out string[] files, string alianClipConnectionName = "")
        {
            files = null;
            var result = Result.True;
            string clipRootDir = null;

            Sci.Win.SYS.CLIPDataTable datas = null;
            if (!(result = PrivUtils.GetClips(tableName, uniqueID, limitedClip, out datas, alianClipConnectionName))) { return result; }

            clipRootDir = PrivUtils.GetClipRootPath(tableName, alianClipConnectionName);

            var allClips = new List<string>();
            var missClips = new List<string>();
            try
            {
                var mappingClipDic = FillMappingClipDic(alianClipConnectionName);
                foreach (var data in datas)
                {
                    string fileFullPath;
                    if (GetClipFile(data, clipRootDir, mappingClipDic, out fileFullPath))
                    {
                        allClips.Add(fileFullPath);
                    }
                    else
                    {
                        missClips.Add(data.SOURCEFILE);
                    }
                }

                files = allClips.ToArray();
            }
            catch (Exception ex)
            {
                return Result.F(ex);
            }

            if (missClips.Count != 0)
            {
                string miss = Environment.NewLine + string.Join(Environment.NewLine, missClips);
                result = Result.F("These files are not found..." + miss);
            }

            return result;
        }

        /// <summary>
        /// input 1 隨單號 + tableName刪除其所有的副擋
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="uniqueID">單號</param>
        /// <param name="alianClipConnectionName">alian Clip ConnectionName</param>
        /// <returns>ok or not</returns>
        public static DualResult DeleteClip(string tableName, string uniqueID)
        {
            DualResult result;

            // 1. 刪除擋案
            result = DeleteClip_File(tableName, uniqueID);
            if (!result)
            {
                return result;
            }

            // 2. 先刪除DB記錄
            result = DeleteClip_DB(tableName, uniqueID);

            return result;
        }

        /// <summary>
        /// input 1 隨單號 + tableName刪除其所有的副擋
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="uniqueID">單號</param>
        /// <param name="alianClipConnectionName">alian Clip ConnectionName</param>
        /// <returns>ok or not</returns>
        public static DualResult DeleteAlianClip(string tableName, string uniqueID, string alianClipConnectionName)
        {
            DualResult result;

            // 1. 刪除擋案
            result = DeleteClip_File(tableName, uniqueID, alianClipConnectionName);
            if (!result) { return result; }

            // 2. 先刪除DB記錄
            result = DeleteClip_DB(tableName, uniqueID, alianClipConnectionName);

            return result;
        }

        private static DualResult DeleteClip_File(string tableName, string uniqueID, string alianClipConnectionName = "")
        {
            string[] files = null;
            var result = GetAllClipFiles(tableName, uniqueID, "%", out files, alianClipConnectionName);

            // 1. 刪除擋案
            if (files == null || files.Length == 0)
            {
                return Result.True;
            }

            // result 是錯誤, 但files有檔案, 代表可能部份檔案找不到, 仍可繼續刪
            if (!result && !(files == null && files.Length > 0))
            {
                result = Result.True;
            }

            List<string> deleteFails = new List<string>();
            Exception e = null;
            foreach (var delFile in files)
            {
                try
                {
                    if (File.Exists(delFile))
                    {
                        var fileinfo = new FileInfo(delFile);
                        fileinfo.IsReadOnly = false;
                        File.Delete(delFile);

                        if (File.Exists(delFile))
                        {
                            // reCheck...
                            deleteFails.Add(delFile);
                        }
                    }

                }
                catch (Exception ex)
                {
                    e = ex;
                    deleteFails.Add(delFile);
                }
            }

            if (deleteFails.Count > 0)
            {
                string msg = "Fail to delete these files :" +
                    Environment.NewLine +
                    string.Join(Environment.NewLine, deleteFails);
                result = Result.F(msg, e);
            }

            return result;
        }

        private static DualResult DeleteClip_DB(string tableName, string uniqueID, string alianClipConnectionName = "")
        {
            string cmd = "delete from dbo.Clip where tableName like @tableName and uniqueKey = @uniqueID";
            var pars = new List<System.Data.SqlClient.SqlParameter>();
            pars.Add(new System.Data.SqlClient.SqlParameter("@tableName", tableName));
            pars.Add(new System.Data.SqlClient.SqlParameter("@uniqueID", uniqueID));
            var result = Sci.Data.DBProxy.Current.Execute(alianClipConnectionName, cmd, pars);

            if (result)
            {
                pars = new List<System.Data.SqlClient.SqlParameter>();
                pars.Add(new System.Data.SqlClient.SqlParameter("@tableName", tableName + "#%"));
                pars.Add(new System.Data.SqlClient.SqlParameter("@uniqueID", uniqueID));
                result = Sci.Data.DBProxy.Current.Execute(alianClipConnectionName, cmd, pars);
            }

            return result;
        }
    }
}
