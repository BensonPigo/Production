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
using Sci.Data;
using Sci.Win;
using System.Data.SqlClient;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Threading;
using System.Collections;
using System.Xml.Schema;
using System.Net;
using System.Threading.Tasks;
using Sci.Production.Prg;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// Clip subpage class.
    /// </summary>
    public partial class ClipGASA : Sci.Win.Tools.BaseGrid
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, string[]> mappingClipDic;

        private static string CHARs = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="dir"></param>
        ///// <param name="clipDir"></param>
        ///// <returns></returns>
        //internal static DualResult GetClipDirYYYYMM(Sci.Win.SYS.CLIPRow data, out string dir, string clipDir = null)
        //{
        //    clipDir = clipDir ?? PrivUtils.GetClipRootPath(data.TABLENAME, _alianClipConnectionName);
        //    dir = null;
        //    try
        //    {
        //        string yyyymm = null;
        //        yyyymm = data.ADDDATE.ToString("yyyyMM");
        //        dir = Path.Combine(clipDir, yyyymm);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result.F(ex);
        //    }

        //    return Result.True;
        //}

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
        protected ClipGASA()
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
                .Text("PKEY", header: "UKey", width: Widths.AnsiChars(15))
                .Text("ADDDATE", header: "Created by", width: Widths.AnsiChars(25), settings: new DataGridViewGeneratorTextColumnSettings()
                {
                    CellFormatting = (s, e) =>
                    {
                        var view = this.grid.GetData<DataRowView>(e.RowIndex);
                        if (null == view)
                        {
                            return;
                        }

                        var data = (CLIPGASARow)view.Row;

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
            this.btnDownloadAll.Click += this.downloadAll_Click;

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
        public ClipGASA(string tablename, string uid, bool canedit, DataRow dr, string limitedClip = "", string alianClipConnectionName = "", string apiUrlFile = "")
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
        private CLIPGASADataTable _datas;

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

            CLIPGASADataTable datas = null;
            //CLIPDataTable datas = null;
            DateTime? firstcliptime = null;
            string yyyymm = null, fullpath = null;

            if (!(result = GetGASAClips(this.TableName, this.UID, this.LimitedClip, out datas, _alianClipConnectionName)))
            //if (!(result = PrivUtils.GetClips(this.TableName, this.UID, this.LimitedClip, out datas, _alianClipConnectionName)))
            {
                return result;
            }

            DataRow dr = datas.NewRow();

            if (0 < datas.Count)
            {
                firstcliptime = GetFirstClipTime(datas);

                if (firstcliptime.HasValue)
                {
                    yyyymm = firstcliptime.Value.ToString("yyyyMM");
                    fullpath = Path.Combine(this._clipdir, yyyymm);

                    if (!(result = SetClipSize(datas, Path.Combine(this._clipdir, yyyymm))))
                    {
                        Logs.UI.LogErrorByCaller(result);
                    }
                }
            }

            //if (!(result = AsyncHelper.Current.DataLoading(this, () =>
            //{
            //    if (!(result = GetGASAClips(this.TableName, this.UID, this.LimitedClip, out datas, _alianClipConnectionName)))
            //    //if (!(result = PrivUtils.GetClips(this.TableName, this.UID, this.LimitedClip, out datas, _alianClipConnectionName)))
            //    {
            //        return result;
            //    }

            //    if (0 < datas.Count)
            //    {
            //        firstcliptime = GetFirstClipTime(datas);

            //        if (firstcliptime.HasValue)
            //        {
            //            yyyymm = firstcliptime.Value.ToString("yyyyMM");
            //            fullpath = Path.Combine(this._clipdir, yyyymm);

            //            if (!(result = SetClipSize(datas, Path.Combine(this._clipdir, yyyymm))))
            //            {
            //                Logs.UI.LogErrorByCaller(result);
            //            }
            //        }
            //    }

            //    return Result.True;
            //})))
            //{
            //    return result;
            //}

            this._datas = datas;
            ////_firstcliptime = firstcliptime;
            ////_yyyymm = yyyymm;
            ////_fullpath = fullpath;
            this._datas.DefaultView.Sort = "AddDate";
            this.SetGrid(datas);

            return Result.True;
        }

        public DualResult GetGASAClips(string tablename, string uniquekey, out CLIPGASADataTable datas, string alianClipConnectionName = "")
        {
            datas = null;
            if (tablename == null || tablename.Length == 0)
            {
                return Ict.Result.F_ArgNull("tablename");
            }

            if (uniquekey == null)
            {
                return Ict.Result.F_ArgNull("uid");
            }

            try
            {
                DualResult result;
                if (!(result = DBProxy.Current.OpenConnection(alianClipConnectionName, out var connection)))
                {
                    return result;
                }

                using (connection)
                {
                    CLIPGASATableAdapter cLIPTableAdapter = new CLIPGASATableAdapter();
                    cLIPTableAdapter.Connection = connection;
                    DBProxy.Current.Select(alianClipConnectionName, "SELECT 1 FROM sys.columns \r\n                                  WHERE Name = N'Type'\r\n                                  AND Object_ID = Object_ID(N'GASAClip')", out DataTable datas2);
                    if (datas2 != null && datas2.Rows.Count > 0)
                    {
                        datas = cLIPTableAdapter.GetContainType(tablename, uniquekey);
                    }
                    else
                    {
                        datas = cLIPTableAdapter.Gets(tablename, uniquekey);
                    }
                }
            }
            catch (Exception exception)
            {
                return new DualResult(result: false, description: @"進行 IForm 開啟記錄時發生錯誤。",exception: exception);
            }

            return Ict.Result.True;
        }

        public static DualResult GetGASAClips(string tablename, string uniquekey, string limitedClip, out CLIPGASADataTable datas, string alianClipConnectionName = "")
        {
            datas = null;
            if (tablename == null || tablename.Length == 0)
            {
                return Ict.Result.F_ArgNull("tablename");
            }

            if (uniquekey == null)
            {
                return Ict.Result.F_ArgNull("uid");
            }

            CLIPGASADataTable cLIPDataTable = null;
            try
            {
                DualResult result;
                if (!(result = DBProxy.Current.OpenConnection(alianClipConnectionName, out var connection)))
                {
                    return result;
                }

                using (connection)
                {
                    CLIPGASATableAdapter cLIPTableAdapter = new CLIPGASATableAdapter();
                    cLIPTableAdapter.Connection = connection;
                    DBProxy.Current.Select(alianClipConnectionName, "SELECT 1 FROM sys.columns \r\n                                  WHERE Name = N'Type'\r\n                                  AND Object_ID = Object_ID(N'GASAClip')", out DataTable datas2);
                    if (datas2 != null && datas2.Rows.Count > 0)
                    {
                        datas = cLIPTableAdapter.GetContainType(tablename, uniquekey);
                        cLIPDataTable = cLIPTableAdapter.GetContainType(tablename + "#" + limitedClip, uniquekey);
                        foreach (CLIPGASARow row in cLIPDataTable.Rows)
                        {
                            CLIPGASARow cLIPRow2 = datas.NewCLIPRow();
                            cLIPRow2.ItemArray = row.ItemArray;
                            cLIPRow2.TABLENAME = tablename;
                            datas.AddCLIPRow(cLIPRow2);
                        }
                    }
                    else
                    {
                        //tablename = "GASAClip";
                        datas = cLIPTableAdapter.Gets(tablename, uniquekey);
                        cLIPDataTable = cLIPTableAdapter.Gets(tablename + "#" + limitedClip, uniquekey);
                        foreach (CLIPGASARow row2 in cLIPDataTable.Rows)
                        {
                            CLIPGASARow cLIPRow4 = datas.NewCLIPRow();
                            cLIPRow4.ItemArray = row2.ItemArray;
                            cLIPRow4.TABLENAME = tablename;
                            datas.AddCLIPRow(cLIPRow4);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return new DualResult(result: false, description: @"進行 IForm 開啟記錄時發生錯誤。", exception: exception);
            }

            return Ict.Result.True;
        }

        //
        // 摘要:
        //     get file size and set the size information in datarow.
        //
        // 參數:
        //   datas:
        //     DataRows of Clip information.
        //
        //   dir:
        //     Path dir of Clip.
        //
        // 傳回:
        //     result of the method is executed.
        public static DualResult SetClipSize(CLIPGASADataTable datas, string dir)
        {
            if (dir == null || dir.Length == 0)
            {
                return Ict.Result.F_ArgNull("dir");
            }

            if (datas == null || datas.Count == 0)
            {
                return Ict.Result.True;
            }

            if (!Directory.Exists(dir))
            {
                return Ict.Result.True;
            }

            foreach (CLIPGASARow row in datas.Rows)
            {
                string clipFileName = GetClipFileName(row);
                if (clipFileName == null)
                {
                    continue;
                }

                try
                {
                    string fileName = Path.Combine(dir, clipFileName);
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (fileInfo.Exists)
                    {
                        row.SIZE = fileInfo.Length;
                    }
                }
                catch (Exception ex)
                {
                    Logs.UI.LogErrorByCaller("判斷 '{0}' CLIP 檔案大小時發生錯誤。".InvariantFormat(clipFileName), ex);
                }
            }

            return Ict.Result.True;
        }

        //
        // 摘要:
        //     get Clip file name.
        //
        // 參數:
        //   data:
        //     Clip DataRow
        //
        // 傳回:
        //     file name of Clip.
        private static string GetClipFileName(CLIPGASARow data)
        {
            if (data.IsTABLENAMENull() || data.IsSOURCEFILENull())
            {
                return null;
            }

            return data.TABLENAME + data.PKEY + Path.GetExtension(data.SOURCEFILE);
        }

        //
        // 摘要:
        //     get Clip file name.
        //
        // 參數:
        //   data:
        //     Clip DataRow
        //
        //   filename:
        //     file name of Clip.
        //
        // 傳回:
        //     result of the method is executed.
        public static DualResult GetClipFileName(CLIPGASARow data, out string filename)
        {
            filename = null;
            if (data == null)
            {
                return Ict.Result.F_ArgNull("data");
            }

            if (data.IsTABLENAMENull())
            {
                return new DualResult(result: false, description: "尚未設定 TABLENAME。");
            }

            if (data.IsSOURCEFILENull())
            {
                return new DualResult(result: false, description: "尚未設定 SOURCEFILE。");
            }

            filename = data.TABLENAME + data.PKEY + Path.GetExtension(data.SOURCEFILE);
            return Ict.Result.True;
        }

        public static DualResult DeleteClipRow(CLIPGASARow data, string alianClipConnectionName = "")
        {
            if (data == null)
            {
                return Ict.Result.F_ArgNull("data");
            }

            try
            {
                DualResult result;
                if (!(result = DBProxy.Current.OpenConnection(alianClipConnectionName, out var connection)))
                {
                    return result;
                }

                using (connection)
                {
                    CLIPGASATableAdapter cLIPTableAdapter = new CLIPGASATableAdapter();
                    cLIPTableAdapter.Connection = connection;
                    cLIPTableAdapter.Delete(data.PKEY);
                }
            }
            catch (Exception exception)
            {
                return new DualResult(result: false,description: "刪除 CLIP 資料時發生錯誤。",exception: exception);
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// Get first clip time from current collection.
        /// </summary>
        /// <param name="datas">Current collection.</param>
        /// <returns>First datetime.</returns>
        private static DateTime? GetFirstClipTime(IEnumerable<CLIPGASARow> datas)
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

        private List<CLIPGASARow> GetSelectItems()
        {
            return this.grid.SelectedRows.Cast<DataGridViewRow>().Select(row => (CLIPGASARow)((DataRowView)row.DataBoundItem).Row).ToList();
        }

        //
        // 摘要:
        //     get Clip file name.
        //
        // 參數:
        //   data:
        //     Clip DataRow
        //
        // 傳回:
        //     file name of Clip.

        /// <summary>
        /// Get the selected row.
        /// </summary>
        /// <returns>Row of selected.</returns>
        private CLIPGASARow GetSelected()
        {
            return (CLIPGASARow)this.grid.CurrentDataRow;
        }

        /// <summary>
        /// Get file name from specified row.
        /// </summary>
        /// <param name="data">Specified row.</param>
        /// <param name="file">File name string.</param>
        /// <returns>Result of process.</returns>
        private static DualResult GetClipFile(CLIPGASARow data, string clipRootDir, Dictionary<string, string[]> mappingClipDic, out string file)
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
            if (!(result = GetClipFileName(data, out filename)))
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
        private static DualResult GetOlderClipFileName(CLIPGASARow data, string clipDir, Dictionary<string, string[]> mappingClipDic, out string filename)
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
            using (var frm = new ClipGASA01(this.TableName, this.UID, string.Empty, this._clipdir, this.LimitedClip, _alianClipConnectionName, this._dr))
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
                from GASAClip
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
                    lock (FileDownload_UpData.DownloadFileAsync($"{PmsWebAPI.PMSAPApiUri}/api/FileDownload/GetFile", filePath + "\\" + DateTime.Now.ToString("yyyyMM"), fileName, saveFilePath))
                    {
                    }
                }

                #endregion

                this._path = null;
            }
        }

        public static string GetPKeyPre()
        {
            var dtm_sys = DateTime.Now;

            string pkey = string.Empty;
            pkey += CHARs[dtm_sys.Year % 100].ToString() + CHARs[dtm_sys.Month].ToString() + CHARs[dtm_sys.Day].ToString();
            pkey += CHARs[dtm_sys.Hour].ToString();
            pkey += CHARs[dtm_sys.Minute / CHARs.Length].ToString() + CHARs[dtm_sys.Minute % CHARs.Length].ToString();
            pkey += CHARs[dtm_sys.Second / CHARs.Length].ToString() + CHARs[dtm_sys.Second % CHARs.Length].ToString();

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
                if (!(result = DeleteClipRow(data, _alianClipConnectionName)))
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
            if (!(result = GetClipFileName(data, out filename)))
            {
                return;
            }

            file = Path.Combine(clipDirPath, filename);

            result = GetClipFile(data, this._clipdir, this.mappingClipDic, out file);
            if (!MsgHelper.Current.Confirm(this, "是否確認刪除檔案 '{0}'。\n".InvariantFormat(data.SOURCEFILE) + file))
            {
                return;
            }

            if (!(result = DeleteClipRow(data, _alianClipConnectionName)))
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

        void downloadAll_Click(object sender, EventArgs e)
        {
            List<CLIPGASARow> dataList = new List<CLIPGASARow>();
            DataTable dt = (DataTable)this.gridbs.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRowView dr in this.gridbs)
            {
                var data = (CLIPGASARow)dr.Row;
                if (data == null)
                {
                    return;
                }

                dataList.Add(data);
            }

            this.FileDownload(dataList);
        }

        /// <summary>
        /// Download SingleFile
        /// </summary>
        /// <param name="datas"> STS.CLIPRow list </param>
        private void FileDownload(List<CLIPGASARow> datas)
        {
            DualResult result;

            Dictionary<CLIPGASARow, string> files = new Dictionary<CLIPGASARow, string>();
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

                // download 多筆檔案才壓縮
                if (!singleFile)
                {
                    var fileList = new List<string>();
                    foreach (var filePath in files)
                    {
                        string savefile = Path.Combine(savedir, filePath.Key.SOURCEFILE);
                        fileList.Add(savefile);
                    }

                    DateTime date = DateTime.Now;
                    string fileName = Path.Combine(savedir, $"MaterialDocuments_{date.ToString("yyyyMMddHHmmss")}") + ".rar";
                    string[] filesArry = fileList.ToArray();
                    MyUtility.File.RARFile(filesArry, rarOutputFilePath: fileName);

                    List<string> deleteFails = new List<string>();
                    Exception e = null;

                    // 將壓縮後的檔案給刪除掉
                    foreach (var delFile in filesArry)
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
                        string msg = "These files are fail :" +
                            Environment.NewLine +
                            string.Join(Environment.NewLine, deleteFails);
                        result = Result.F(msg, e);
                    }
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

        //        /// <summary>
        ///// 不顯示畫面, 直接夾檔
        ///// </summary>
        ///// <param name="tablename">Table name</param>
        ///// <param name="uniqueID">a unique value in {tablename} </param>
        ///// <param name="userID">who add clip (refer to Pass1.ID )</param>
        ///// <param name="description"> description of files </param>
        ///// <param name="alianClipConnectionName"> alian Clip Connection Name </param>
        ///// <param name="clipFiles">one or more strings for each file (full path)</param>
        ///// <returns>success or not</returns>
        //public static DualResult AddAlianClip(string tablename, string uniqueID, string userID, string description, string alianClipConnectionName, params string[] clipFiles)
        //{
        //    DualResult result = Result.True;
        //    Sci.Win.SYS.CLIPDataTable datas = new Sci.Win.SYS.CLIPDataTable();
        //    List<Sci.Win.SYS.CLIPRow> clip_data = new List<Sci.Win.SYS.CLIPRow>();
        //    for (int i = 0; i < clipFiles.Length; i++)
        //    {
        //        var fileinfo = new FileInfo(clipFiles[i]);
        //        var data = datas.NewCLIPRow();
        //        data.TABLENAME = tablename;
        //        data.UNIQUEKEY = uniqueID;
        //        data.ADDNAME = userID;
        //        data.PKEY = i.ToString("0000000000");
        //        data.DESCRIPTION = description;
        //        data.SOURCEFILE = fileinfo.Name;
        //        data.LOCALFILE = fileinfo.FullName;
        //        data.SIZE = fileinfo.Length;

        //        clip_data.Add(data);
        //    }

        //    result = AddClip(clip_data, alianClipConnectionName: alianClipConnectionName);
        //    datas.Dispose();

        //    return result;
        //}

        ///// <summary>
        ///// 不顯示畫面, 直接夾檔
        ///// </summary>
        ///// <param name="tablename">Table name</param>
        ///// <param name="uniqueID">a unique value in {tablename} </param>
        ///// <param name="userID">who add clip (refer to Pass1.ID )</param>
        ///// <param name="description"> description of files </param>
        ///// <param name="clipFiles">one or more strings for each file (full path)</param>
        ///// <returns>success or not</returns>
        //public static DualResult AddClip(string tablename, string uniqueID, string userID, string description, params string[] clipFiles)
        //{
        //    DualResult result = Result.True;
        //    Sci.Win.SYS.CLIPDataTable datas = new Sci.Win.SYS.CLIPDataTable();
        //    List<Sci.Win.SYS.CLIPRow> clip_data = new List<Sci.Win.SYS.CLIPRow>();
        //    for (int i = 0; i < clipFiles.Length; i++)
        //    {
        //        var fileinfo = new FileInfo(clipFiles[i]);
        //        var data = datas.NewCLIPRow();
        //        data.TABLENAME = tablename;
        //        data.UNIQUEKEY = uniqueID;
        //        data.ADDNAME = userID;
        //        data.PKEY = i.ToString("0000000000");
        //        data.DESCRIPTION = description;
        //        data.SOURCEFILE = fileinfo.Name;
        //        data.LOCALFILE = fileinfo.FullName;
        //        data.SIZE = fileinfo.Length;

        //        clip_data.Add(data);
        //    }

        //    result = AddClip(clip_data);
        //    datas.Dispose();
        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="dir"></param>
        /// <param name="_yyyymm"></param>
        /// <param name="addTime"></param>
        /// <param name="alianClipConnectionName"></param>
        /// <returns>success or not</returns>
        internal static DualResult AddClip(IList<CLIPGASARow> datas, string dir = null, string _yyyymm = null, DateTime? addTime = null, string alianClipConnectionName = "")
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

            if (!(result = AddClips(datas, dir, alianClipConnectionName)))
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

        ///// <summary>
        ///// 不顯示畫面, 直接夾檔
        ///// </summary>
        ///// <param name="tablename">Table name</param>
        ///// <param name="uniqueID">a unique value in {tablename} </param>
        ///// <param name="userID">who add clip (refer to Pass1.ID )</param>
        ///// <param name="description"> description of files </param>
        ///// <param name="clipFiles">one or more strings for each file (full path)</param>
        ///// <returns>success or not</returns>
        //public static DualResult AddLimitedClip(string tablename, string uniqueID, string userID, string description, string limitedClip, params string[] clipFiles)
        //{
        //    DualResult result = Result.True;
        //    Sci.Win.SYS.CLIPDataTable datas = new Sci.Win.SYS.CLIPDataTable();
        //    List<Sci.Win.SYS.CLIPRow> clip_data = new List<Sci.Win.SYS.CLIPRow>();
        //    for (int i = 0; i < clipFiles.Length; i++)
        //    {
        //        var fileinfo = new FileInfo(clipFiles[i]);
        //        var data = datas.NewCLIPRow();
        //        data.TABLENAME = tablename;
        //        data.UNIQUEKEY = uniqueID;
        //        data.ADDNAME = userID;
        //        data.PKEY = i.ToString("0000000000");
        //        data.DESCRIPTION = description;
        //        data.SOURCEFILE = fileinfo.Name;
        //        data.LOCALFILE = fileinfo.FullName;
        //        data.SIZE = fileinfo.Length;

        //        clip_data.Add(data);
        //    }

        //    result = AddLimitedClip(clip_data, limitedClip: limitedClip);
        //    datas.Dispose();
        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="dir"></param>
        /// <param name="_yyyymm"></param>
        /// <param name="addTime"></param>
        ///  <param name="limitedClip"></param>
        /// <returns>success or not</returns>
        internal static DualResult AddLimitedClip(IList<CLIPGASARow> datas, string dir = null, string _yyyymm = null, DateTime? addTime = null, string limitedClip = "")
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

            if (!(result = AddClips(datas, dir, _limitedClip))) { return result; }

            return result;
        }

        private static int RetryAddClips = 0;

        //
        // 摘要:
        //     Get Pk string array.
        //
        // 參數:
        //   count:
        //     Length of string.
        //
        // 傳回:
        //     string array.
        private static string[] GetPKeys(int count)
        {
            string[] array = new string[count];
            string pKeyPre = GetPKeyPre();
            for (int i = 0; i < count; i++)
            {
                array[i] = pKeyPre + i.ToString("00");
            }

            return array;
        }

        //
        // 摘要:
        //     New add some Clip information and clip files.
        //
        // 參數:
        //   datas:
        //     DataRow information of clip
        //
        //   dir:
        //     Path dir of Clip.
        //
        // 傳回:
        //     result of the method is executed.
        public static DualResult AddClips(IList<CLIPGASARow> datas, string dir, string alianClipConnectionName = "")
        {
            RetryAddClips++;
            if (datas == null || datas.Count == 0)
            {
                return Ict.Result.True;
            }

            if (dir == null || dir.Length == 0)
            {
                return Ict.Result.F_ArgNull("dir");
            }

            DualResult result;
            if (!(result = Utils.ChkDir(dir)))
            {
                return result;
            }

            HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                if (!(result = DBProxy.Current.OpenConnection(alianClipConnectionName, out var connection)))
                {
                    return result;
                }

                int num = 0;
                string[] pKeys = GetPKeys(datas.Count);
                foreach (CLIPGASARow data in datas)
                {
                    data.PKEY = pKeys[num];
                    string lOCALFILE = data.LOCALFILE;
                    string clipFileName = GetClipFileName(data);
                    string text = Path.Combine(dir, clipFileName);
                    File.Copy(lOCALFILE, text, overwrite: true);
                    hashSet.Add(text);
                    try
                    {
                        FileInfo fileInfo = new FileInfo(text);
                        fileInfo.IsReadOnly = true;
                    }
                    catch (Exception ex)
                    {
                        Logs.UI.LogErrorByCaller("將 CLIP 上傳檔案設定為唯讀時發生錯誤。", ex);
                    }

                    num++;
                }

                using (connection)
                {
                    SqlTransaction sqlTransaction = connection.BeginTransaction();
                    CLIPGASATableAdapter cLIPTableAdapter = new CLIPGASATableAdapter();
                    cLIPTableAdapter.Connection = connection;
                    cLIPTableAdapter.Transaction = sqlTransaction;
                    foreach (CLIPGASARow data2 in datas)
                    {
                        DBProxy.Current.Select(alianClipConnectionName, "SELECT 1 FROM sys.columns \r\n                                  WHERE Name = N'Type'\r\n                                  AND Object_ID = Object_ID(N'Clip')", out DataTable datas2);
                        if (datas2 != null && datas2.Rows.Count > 0)
                        {
                            cLIPTableAdapter.InsertContainType(data2.TABLENAME, data2.UNIQUEKEY, data2.SOURCEFILE, data2.IsDESCRIPTIONNull() ? null : data2.DESCRIPTION, data2.PKEY, data2.IsADDNAMENull() ? null : data2.ADDNAME, data2.ADDDATE, data2.IsTYPENull() ? null : data2.TYPE);
                        }
                        else
                        {
                            cLIPTableAdapter.Insert(data2.TABLENAME, data2.UNIQUEKEY, data2.SOURCEFILE, data2.IsDESCRIPTIONNull() ? null : data2.DESCRIPTION, data2.PKEY, data2.IsADDNAMENull() ? null : data2.ADDNAME, data2.ADDDATE);
                        }
                    }

                    sqlTransaction.Commit();
                }

                return Ict.Result.True;
            }
            catch (Exception exception)
            {
                result = new DualResult(result: false, description: "新增 CLIP 檔案上傳資料時發生錯誤。",exception: exception);
                foreach (string item in hashSet)
                {
                    try
                    {
                        if (File.Exists(item))
                        {
                            File.SetAttributes(item, FileAttributes.Normal);
                            File.Delete(item);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (RetryAddClips < 5)
                {
                    Thread.Sleep(1500);
                    result = AddClips(datas, dir);
                }
            }

            return result;
        }

        public static DualResult GetClips(string tablename, string uniquekey, string limitedClip, out CLIPGASADataTable datas, string alianClipConnectionName = "")
        {
            datas = null;
            if (tablename == null || tablename.Length == 0)
            {
                return Ict.Result.F_ArgNull("tablename");
            }

            if (uniquekey == null)
            {
                return Ict.Result.F_ArgNull("uid");
            }

            CLIPGASADataTable cLIPDataTable = null;
            try
            {
                DualResult result;
                if (!(result = DBProxy.Current.OpenConnection(alianClipConnectionName, out var connection)))
                {
                    return result;
                }

                using (connection)
                {
                    CLIPGASATableAdapter cLIPTableAdapter = new CLIPGASATableAdapter();
                    cLIPTableAdapter.Connection = connection;
                    DBProxy.Current.Select(alianClipConnectionName, "SELECT 1 FROM sys.columns \r\n                                  WHERE Name = N'Type'\r\n                                  AND Object_ID = Object_ID(N'GASAClip')", out DataTable datas2);
                    if (datas2 != null && datas2.Rows.Count > 0)
                    {
                        datas = cLIPTableAdapter.GetContainType(tablename, uniquekey);
                        cLIPDataTable = cLIPTableAdapter.GetContainType(tablename + "#" + limitedClip, uniquekey);
                        foreach (CLIPGASARow row in cLIPDataTable.Rows)
                        {
                            CLIPGASARow cLIPRow2 = datas.NewCLIPRow();
                            cLIPRow2.ItemArray = row.ItemArray;
                            cLIPRow2.TABLENAME = tablename;
                            datas.AddCLIPRow(cLIPRow2);
                        }
                    }
                    else
                    {
                        datas = cLIPTableAdapter.Gets(tablename, uniquekey);
                        cLIPDataTable = cLIPTableAdapter.Gets(tablename + "#" + limitedClip, uniquekey);
                        foreach (CLIPGASARow row2 in cLIPDataTable.Rows)
                        {
                            CLIPGASARow cLIPRow4 = datas.NewCLIPRow();
                            cLIPRow4.ItemArray = row2.ItemArray;
                            cLIPRow4.TABLENAME = tablename;
                            datas.AddCLIPRow(cLIPRow4);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return new DualResult(result: false,description: "進行 IForm 開啟記錄時發生錯誤。",exception: exception);
            }

            return Ict.Result.True;
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

            CLIPGASADataTable datas = null;
            if (!(result = GetClips(tableName, uniqueID, limitedClip, out datas, alianClipConnectionName))) { return result; }

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

        ///// <summary>
        ///// input 1 隨單號 + tableName刪除其所有的副擋
        ///// </summary>
        ///// <param name="tableName">tableName</param>
        ///// <param name="uniqueID">單號</param>
        ///// <param name="alianClipConnectionName">alian Clip ConnectionName</param>
        ///// <returns>ok or not</returns>
        //public static DualResult DeleteClip(string tableName, string uniqueID)
        //{
        //    DualResult result;

        //    // 1. 刪除擋案
        //    result = DeleteClip_File(tableName, uniqueID);
        //    if (!result)
        //    {
        //        return result;
        //    }

        //    // 2. 先刪除DB記錄
        //    result = DeleteClip_DB(tableName, uniqueID);

        //    return result;
        //}

        ///// <summary>
        ///// input 1 隨單號 + tableName刪除其所有的副擋
        ///// </summary>
        ///// <param name="tableName">tableName</param>
        ///// <param name="uniqueID">單號</param>
        ///// <param name="alianClipConnectionName">alian Clip ConnectionName</param>
        ///// <returns>ok or not</returns>
        //public static DualResult DeleteAlianClip(string tableName, string uniqueID, string alianClipConnectionName)
        //{
        //    DualResult result;

        //    // 1. 刪除擋案
        //    result = DeleteClip_File(tableName, uniqueID, alianClipConnectionName);
        //    if (!result) { return result; }

        //    // 2. 先刪除DB記錄
        //    result = DeleteClip_DB(tableName, uniqueID, alianClipConnectionName);

        //    return result;
        //}

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

    //
    // 摘要:
    //     Represents the strongly named DataTable class.
    [Serializable]
    [XmlSchemaProvider("GetTypedTableSchema")]
    public class CLIPGASADataTable : TypedTableBase<CLIPGASARow>
    {
        private DataColumn columnTABLENAME;

        private DataColumn columnUNIQUEKEY;

        private DataColumn columnSOURCEFILE;

        private DataColumn columnDESCRIPTION;

        private DataColumn columnPKEY;

        private DataColumn columnADDNAME;

        private DataColumn columnADDDATE;

        private DataColumn columnSIZE;

        private DataColumn columnLOCALFILE;

        private DataColumn columnTYPE;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn TABLENAMEColumn => columnTABLENAME;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn UNIQUEKEYColumn => columnUNIQUEKEY;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn SOURCEFILEColumn => columnSOURCEFILE;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn DESCRIPTIONColumn => columnDESCRIPTION;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn PKEYColumn => columnPKEY;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn ADDNAMEColumn => columnADDNAME;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn ADDDATEColumn => columnADDDATE;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn SIZEColumn => columnSIZE;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn LOCALFILEColumn => columnLOCALFILE;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DataColumn TYPEColumn => columnTYPE;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [Browsable(false)]
        public int Count => base.Rows.Count;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public CLIPGASARow this[int index] => (CLIPGASARow)base.Rows[index];

        //[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        //public event CLIPRowChangeEventHandler CLIPRowChanging;

        //[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        //public event CLIPRowChangeEventHandler CLIPRowChanged;

        //[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        //public event CLIPRowChangeEventHandler CLIPRowDeleting;

        //[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        //public event CLIPRowChangeEventHandler CLIPRowDeleted;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public CLIPGASADataTable()
        {
            base.TableName = "GASAClip";
            BeginInit();
            InitClass();
            EndInit();
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        internal CLIPGASADataTable(DataTable table)
        {
            base.TableName = table.TableName;
            if (table.CaseSensitive != table.DataSet.CaseSensitive)
            {
                base.CaseSensitive = table.CaseSensitive;
            }

            if (table.Locale.ToString() != table.DataSet.Locale.ToString())
            {
                base.Locale = table.Locale;
            }

            if (table.Namespace != table.DataSet.Namespace)
            {
                base.Namespace = table.Namespace;
            }

            base.Prefix = table.Prefix;
            base.MinimumCapacity = table.MinimumCapacity;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected CLIPGASADataTable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            InitVars();
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void AddCLIPRow(CLIPGASARow row)
        {
            base.Rows.Add(row);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public CLIPGASARow AddCLIPRow(string TABLENAME, string UNIQUEKEY, string SOURCEFILE, string DESCRIPTION, string PKEY, string ADDNAME, DateTime ADDDATE, long SIZE, string LOCALFILE, string TYPE)
        {
            CLIPGASARow cLIPRow = (CLIPGASARow)NewRow();
            object[] array2 = (cLIPRow.ItemArray = new object[10] { TABLENAME, UNIQUEKEY, SOURCEFILE, DESCRIPTION, PKEY, ADDNAME, ADDDATE, SIZE, LOCALFILE, TYPE });
            base.Rows.Add(cLIPRow);
            return cLIPRow;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public CLIPGASARow FindByPKEY(string PKEY)
        {
            return (CLIPGASARow)base.Rows.Find(new object[1] { PKEY });
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public override DataTable Clone()
        {
            CLIPGASADataTable cLIPDataTable = (CLIPGASADataTable)base.Clone();
            cLIPDataTable.InitVars();
            return cLIPDataTable;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected override DataTable CreateInstance()
        {
            return new CLIPGASADataTable();
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        internal void InitVars()
        {
            columnTABLENAME = base.Columns["TABLENAME"];
            columnUNIQUEKEY = base.Columns["UNIQUEKEY"];
            columnSOURCEFILE = base.Columns["SOURCEFILE"];
            columnDESCRIPTION = base.Columns["DESCRIPTION"];
            columnPKEY = base.Columns["PKEY"];
            columnADDNAME = base.Columns["ADDNAME"];
            columnADDDATE = base.Columns["ADDDATE"];
            columnSIZE = base.Columns["SIZE"];
            columnLOCALFILE = base.Columns["LOCALFILE"];
            columnTYPE = base.Columns["TYPE"];
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private void InitClass()
        {
            columnTABLENAME = new DataColumn("TABLENAME", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnTABLENAME);
            columnUNIQUEKEY = new DataColumn("UNIQUEKEY", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnUNIQUEKEY);
            columnSOURCEFILE = new DataColumn("SOURCEFILE", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnSOURCEFILE);
            columnDESCRIPTION = new DataColumn("DESCRIPTION", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnDESCRIPTION);
            columnPKEY = new DataColumn("PKEY", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnPKEY);
            columnADDNAME = new DataColumn("ADDNAME", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnADDNAME);
            columnADDDATE = new DataColumn("ADDDATE", typeof(DateTime), null, MappingType.Element);
            base.Columns.Add(columnADDDATE);
            columnSIZE = new DataColumn("SIZE", typeof(long), null, MappingType.Element);
            base.Columns.Add(columnSIZE);
            columnLOCALFILE = new DataColumn("LOCALFILE", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnLOCALFILE);
            columnTYPE = new DataColumn("TYPE", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnTYPE);
            base.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[1] { columnPKEY }, isPrimaryKey: true));
            columnPKEY.AllowDBNull = false;
            columnPKEY.Unique = true;
            columnPKEY.MaxLength = 12;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public CLIPGASARow NewCLIPRow()
        {
            return (CLIPGASARow)NewRow();
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new CLIPGASARow(builder);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected override Type GetRowType()
        {
            return typeof(CLIPGASARow);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void RemoveCLIPRow(CLIPGASARow row)
        {
            base.Rows.Remove(row);
        }

    }

    //
    // 摘要:
    //     Represents strongly named DataRow class.
    public class CLIPGASARow : DataRow
    {
        private CLIPGASADataTable tableCLIP;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string TABLENAME
        {
            get
            {
                try
                {
                    return (string)base[tableCLIP.TABLENAMEColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'TABLENAME' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.TABLENAMEColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string UNIQUEKEY
        {
            get
            {
                try
                {
                    return (string)base[tableCLIP.UNIQUEKEYColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'UNIQUEKEY' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.UNIQUEKEYColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string SOURCEFILE
        {
            get
            {
                try
                {
                    return (string)base[tableCLIP.SOURCEFILEColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'SOURCEFILE' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.SOURCEFILEColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string DESCRIPTION
        {
            get
            {
                try
                {
                    return (string)base[tableCLIP.DESCRIPTIONColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'DESCRIPTION' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.DESCRIPTIONColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string PKEY
        {
            get
            {
                return (string)base[tableCLIP.PKEYColumn];
            }
            set
            {
                base[tableCLIP.PKEYColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string ADDNAME
        {
            get
            {
                try
                {
                    return (string)base[tableCLIP.ADDNAMEColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'ADDNAME' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.ADDNAMEColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DateTime ADDDATE
        {
            get
            {
                try
                {
                    return (DateTime)base[tableCLIP.ADDDATEColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'ADDDATE' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.ADDDATEColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public long SIZE
        {
            get
            {
                try
                {
                    return (long)base[tableCLIP.SIZEColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'SIZE' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.SIZEColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string LOCALFILE
        {
            get
            {
                try
                {
                    return (string)base[tableCLIP.LOCALFILEColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'LOCALFILE' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.LOCALFILEColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public string TYPE
        {
            get
            {
                try
                {
                    return (string)base[tableCLIP.TYPEColumn];
                }
                catch (InvalidCastException innerException)
                {
                    throw new StrongTypingException("資料表 'GASAClip' 中資料行 'TYPE' 的值是 DBNull。", innerException);
                }
            }
            set
            {
                base[tableCLIP.TYPEColumn] = value;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        internal CLIPGASARow(DataRowBuilder rb)
            : base(rb)
        {
            tableCLIP = (CLIPGASADataTable)base.Table;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsTABLENAMENull()
        {
            return IsNull(tableCLIP.TABLENAMEColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetTABLENAMENull()
        {
            base[tableCLIP.TABLENAMEColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsUNIQUEKEYNull()
        {
            return IsNull(tableCLIP.UNIQUEKEYColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetUNIQUEKEYNull()
        {
            base[tableCLIP.UNIQUEKEYColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsSOURCEFILENull()
        {
            return IsNull(tableCLIP.SOURCEFILEColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetSOURCEFILENull()
        {
            base[tableCLIP.SOURCEFILEColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsDESCRIPTIONNull()
        {
            return IsNull(tableCLIP.DESCRIPTIONColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetDESCRIPTIONNull()
        {
            base[tableCLIP.DESCRIPTIONColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsADDNAMENull()
        {
            return IsNull(tableCLIP.ADDNAMEColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetADDNAMENull()
        {
            base[tableCLIP.ADDNAMEColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsADDDATENull()
        {
            return IsNull(tableCLIP.ADDDATEColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetADDDATENull()
        {
            base[tableCLIP.ADDDATEColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsSIZENull()
        {
            return IsNull(tableCLIP.SIZEColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetSIZENull()
        {
            base[tableCLIP.SIZEColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsLOCALFILENull()
        {
            return IsNull(tableCLIP.LOCALFILEColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetLOCALFILENull()
        {
            base[tableCLIP.LOCALFILEColumn] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public bool IsTYPENull()
        {
            return IsNull(tableCLIP.TYPEColumn);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public void SetTYPENull()
        {
            base[tableCLIP.TYPEColumn] = Convert.DBNull;
        }
    }

    public class CLIPGASATableAdapter : Component
    {
        private SqlDataAdapter _adapter;

        private SqlConnection _connection;

        private SqlTransaction _transaction;

        private SqlCommand[] _commandCollection;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected internal SqlDataAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    InitAdapter();
                }

                return _adapter;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private void InitConnection()
        {
            _connection = new SqlConnection();
            _connection.ConnectionString = "Data Source=testing\\mis;Initial Catalog=trade;Persist Security Info=True;User ID=scimis;Password=27128299";
        }


        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        internal SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    InitConnection();
                }

                return _connection;
            }
            set
            {
                _connection = value;
                if (Adapter.InsertCommand != null)
                {
                    Adapter.InsertCommand.Connection = value;
                }

                if (Adapter.DeleteCommand != null)
                {
                    Adapter.DeleteCommand.Connection = value;
                }

                if (Adapter.UpdateCommand != null)
                {
                    Adapter.UpdateCommand.Connection = value;
                }

                for (int i = 0; i < CommandCollection.Length; i++)
                {
                    if (CommandCollection[i] != null)
                    {
                        CommandCollection[i].Connection = value;
                    }
                }
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [HelpKeyword("vs.data.TableAdapter")]
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public virtual CLIPGASADataTable GetContainType(string tablename, string uniquekey)
        {
            Adapter.SelectCommand = CommandCollection[1];
            if (tablename == null)
            {
                Adapter.SelectCommand.Parameters[0].Value = DBNull.Value;
            }
            else
            {
                Adapter.SelectCommand.Parameters[0].Value = tablename;
            }

            if (uniquekey == null)
            {
                Adapter.SelectCommand.Parameters[1].Value = DBNull.Value;
            }
            else
            {
                Adapter.SelectCommand.Parameters[1].Value = uniquekey;
            }

            CLIPGASADataTable cLIPDataTable = new CLIPGASADataTable();
            Adapter.Fill(cLIPDataTable);
            return cLIPDataTable;
        }


        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private void InitAdapter()
        {
            _adapter = new SqlDataAdapter();
            DataTableMapping dataTableMapping = new DataTableMapping();
            dataTableMapping.SourceTable = "Table";
            dataTableMapping.DataSetTable = "GASACLIP";
            dataTableMapping.ColumnMappings.Add("TABLENAME", "TABLENAME");
            dataTableMapping.ColumnMappings.Add("UNIQUEKEY", "UNIQUEKEY");
            dataTableMapping.ColumnMappings.Add("SOURCEFILE", "SOURCEFILE");
            dataTableMapping.ColumnMappings.Add("DESCRIPTION", "DESCRIPTION");
            dataTableMapping.ColumnMappings.Add("PKEY", "PKEY");
            dataTableMapping.ColumnMappings.Add("ADDNAME", "ADDNAME");
            dataTableMapping.ColumnMappings.Add("ADDDATE", "ADDDATE");
            dataTableMapping.ColumnMappings.Add("FactoryID", "FactoryID");
            _adapter.TableMappings.Add(dataTableMapping);
            _adapter.DeleteCommand = new SqlCommand();
            _adapter.DeleteCommand.Connection = Connection;
            _adapter.DeleteCommand.CommandText = "DELETE FROM [GASACLIP] WHERE (([PKEY] = @Original_PKEY))";
            _adapter.DeleteCommand.CommandType = CommandType.Text;
            _adapter.DeleteCommand.Parameters.Add(new SqlParameter("@Original_PKEY", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "PKEY", DataRowVersion.Original, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand = new SqlCommand();
            _adapter.InsertCommand.Connection = Connection;
            _adapter.InsertCommand.CommandText = "INSERT INTO GASAClip\r\n                            (TableName, UniqueKey, SourceFile, Description, PKey, AddName, AddDate, FactoryID)\r\nVALUES          (@TABLENAME,@UNIQUEKEY,@SOURCEFILE,@DESCRIPTION,@PKEY,@ADDNAME,@ADDDATE,@FactoryID)";
            _adapter.InsertCommand.CommandType = CommandType.Text;
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@TABLENAME", SqlDbType.VarChar, 50, ParameterDirection.Input, 0, 0, "TableName", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@UNIQUEKEY", SqlDbType.VarChar, 80, ParameterDirection.Input, 0, 0, "UniqueKey", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@SOURCEFILE", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "SourceFile", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar, 60, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@PKEY", SqlDbType.VarChar, 12, ParameterDirection.Input, 0, 0, "PKey", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@ADDNAME", SqlDbType.VarChar, 10, ParameterDirection.Input, 0, 0, "AddName", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@ADDDATE", SqlDbType.DateTime, 8, ParameterDirection.Input, 0, 0, "AddDate", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.InsertCommand.Parameters.Add(new SqlParameter("@FactoryID", SqlDbType.DateTime, 8, ParameterDirection.Input, 0, 0, "FactoryID", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand = new SqlCommand();
            _adapter.UpdateCommand.Connection = Connection;
            _adapter.UpdateCommand.CommandText = @"UPDATE [GASACLIP] SET [TABLENAME] = @TABLENAME, [UNIQUEKEY] = @UNIQUEKEY, [SOURCEFILE] = @SOURCEFILE, [DESCRIPTION] = @DESCRIPTION, [PKEY] = @PKEY, [ADDNAME] = @ADDNAME, [ADDDATE] = @ADDDATE ,[FactoryID] = @FactoryID" +
                "WHERE (([PKEY] = @Original_PKEY))";
            _adapter.UpdateCommand.CommandType = CommandType.Text;
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@TABLENAME", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "TABLENAME", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@UNIQUEKEY", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "UNIQUEKEY", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@SOURCEFILE", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "SOURCEFILE", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "DESCRIPTION", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@PKEY", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "PKEY", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@ADDNAME", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "ADDNAME", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@ADDDATE", SqlDbType.DateTime, 0, ParameterDirection.Input, 0, 0, "ADDDATE", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@FactoryID", SqlDbType.DateTime, 0, ParameterDirection.Input, 0, 0, "FactoryID", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Original_PKEY", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "PKEY", DataRowVersion.Original, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private void InitCommandCollection()
        {
            _commandCollection = new SqlCommand[4];
            _commandCollection[0] = new SqlCommand();
            _commandCollection[0].Connection = Connection;
            _commandCollection[0].CommandText = "SELECT          TABLENAME, UNIQUEKEY, SOURCEFILE, DESCRIPTION, PKEY, ADDNAME, ADDDATE\r\nFROM              GASAClip\r\nWHERE          (TABLENAME LIKE @tablename) AND (UNIQUEKEY = @uniquekey)";
            _commandCollection[0].CommandType = CommandType.Text;
            _commandCollection[0].Parameters.Add(new SqlParameter("@tablename", SqlDbType.VarChar, 50, ParameterDirection.Input, 0, 0, "TABLENAME", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[0].Parameters.Add(new SqlParameter("@uniquekey", SqlDbType.VarChar, 80, ParameterDirection.Input, 0, 0, "UNIQUEKEY", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[1] = new SqlCommand();
            _commandCollection[1].Connection = Connection;
            _commandCollection[1].CommandText = "SELECT          TABLENAME, UNIQUEKEY, SOURCEFILE, DESCRIPTION, PKEY, ADDNAME, ADDDATE,TYPE\r\nFROM              GASAClip\r\nWHERE          (TABLENAME LIKE @tablename) AND (UNIQUEKEY = @uniquekey)";
            _commandCollection[1].CommandType = CommandType.Text;
            _commandCollection[1].Parameters.Add(new SqlParameter("@tablename", SqlDbType.VarChar, 50, ParameterDirection.Input, 0, 0, "TableName", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[1].Parameters.Add(new SqlParameter("@uniquekey", SqlDbType.VarChar, 80, ParameterDirection.Input, 0, 0, "UniqueKey", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2] = new SqlCommand();
            _commandCollection[2].Connection = Connection;
            _commandCollection[2].CommandText = "INSERT INTO Clip\r\n                            (TableName, UniqueKey, SourceFile, Description, PKey, AddName, AddDate, Type)\r\nVALUES          (@TABLENAME,@UNIQUEKEY,@SOURCEFILE,@DESCRIPTION,@PKEY,@ADDNAME,@ADDDATE,@Type)";
            _commandCollection[2].CommandType = CommandType.Text;
            _commandCollection[2].Parameters.Add(new SqlParameter("@TABLENAME", SqlDbType.VarChar, 50, ParameterDirection.Input, 0, 0, "TableName", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2].Parameters.Add(new SqlParameter("@UNIQUEKEY", SqlDbType.VarChar, 80, ParameterDirection.Input, 0, 0, "UniqueKey", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2].Parameters.Add(new SqlParameter("@SOURCEFILE", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "SourceFile", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2].Parameters.Add(new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar, 60, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2].Parameters.Add(new SqlParameter("@PKEY", SqlDbType.VarChar, 12, ParameterDirection.Input, 0, 0, "PKey", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2].Parameters.Add(new SqlParameter("@ADDNAME", SqlDbType.VarChar, 10, ParameterDirection.Input, 0, 0, "AddName", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2].Parameters.Add(new SqlParameter("@ADDDATE", SqlDbType.DateTime, 8, ParameterDirection.Input, 0, 0, "AddDate", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[2].Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar, 3, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            // Cilp PoItem
            _commandCollection[3] = new SqlCommand();
            _commandCollection[3].Connection = Connection;
            _commandCollection[3].CommandText = "SELECT TABLENAME, UNIQUEKEY, SOURCEFILE, DESCRIPTION, PKEY, ADDNAME, ADDDATE FROM Clip WHERE (TABLENAME LIKE @tablename) AND (UNIQUEKEY = @uniquekey)";
            _commandCollection[3].CommandType = CommandType.Text;
            _commandCollection[3].Parameters.Add(new SqlParameter("@tablename", SqlDbType.VarChar, 50, ParameterDirection.Input, 0, 0, "TABLENAME", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));
            _commandCollection[3].Parameters.Add(new SqlParameter("@uniquekey", SqlDbType.VarChar, 80, ParameterDirection.Input, 0, 0, "UNIQUEKEY", DataRowVersion.Current, sourceColumnNullMapping: false, value: null, xmlSchemaCollectionDatabase: "", xmlSchemaCollectionOwningSchema: "", xmlSchemaCollectionName: ""));

        }


        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected SqlCommand[] CommandCollection
        {
            get
            {
                if (_commandCollection == null)
                {
                    InitCommandCollection();
                }

                return _commandCollection;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [HelpKeyword("vs.data.TableAdapter")]
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public virtual CLIPGASADataTable Gets(string tablename, string uniquekey)
        {
            if (tablename == "PoItem")
            {
                Adapter.SelectCommand = CommandCollection[3];

            }
            else
            {
                Adapter.SelectCommand = CommandCollection[0];
            }

            if (tablename == null)
            {
                Adapter.SelectCommand.Parameters[0].Value = DBNull.Value;
            }
            else
            {
                Adapter.SelectCommand.Parameters[0].Value = tablename;
            }

            if (uniquekey == null)
            {
                Adapter.SelectCommand.Parameters[1].Value = DBNull.Value;
            }
            else
            {
                Adapter.SelectCommand.Parameters[1].Value = uniquekey;
            }

            CLIPGASADataTable cLIPDataTable = new CLIPGASADataTable();
            Adapter.Fill(cLIPDataTable);
            return cLIPDataTable;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [HelpKeyword("vs.data.TableAdapter")]
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public virtual int Delete(string Original_PKEY)
        {
            if (Original_PKEY == null)
            {
                throw new ArgumentNullException("Original_PKEY");
            }

            Adapter.DeleteCommand.Parameters[0].Value = Original_PKEY;
            ConnectionState state = Adapter.DeleteCommand.Connection.State;
            if ((Adapter.DeleteCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
            {
                Adapter.DeleteCommand.Connection.Open();
            }

            try
            {
                return Adapter.DeleteCommand.ExecuteNonQuery();
            }
            finally
            {
                if (state == ConnectionState.Closed)
                {
                    Adapter.DeleteCommand.Connection.Close();
                }
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        internal SqlTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
                for (int i = 0; i < CommandCollection.Length; i++)
                {
                    CommandCollection[i].Transaction = _transaction;
                }

                if (Adapter != null && Adapter.DeleteCommand != null)
                {
                    Adapter.DeleteCommand.Transaction = _transaction;
                }

                if (Adapter != null && Adapter.InsertCommand != null)
                {
                    Adapter.InsertCommand.Transaction = _transaction;
                }

                if (Adapter != null && Adapter.UpdateCommand != null)
                {
                    Adapter.UpdateCommand.Transaction = _transaction;
                }
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [HelpKeyword("vs.data.TableAdapter")]
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public virtual int InsertContainType(string TABLENAME, string UNIQUEKEY, string SOURCEFILE, string DESCRIPTION, string PKEY, string ADDNAME, DateTime? ADDDATE, string Type)
        {
            SqlCommand sqlCommand = CommandCollection[2];
            if (TABLENAME == null)
            {
                sqlCommand.Parameters[0].Value = DBNull.Value;
            }
            else
            {
                sqlCommand.Parameters[0].Value = TABLENAME;
            }

            if (UNIQUEKEY == null)
            {
                sqlCommand.Parameters[1].Value = DBNull.Value;
            }
            else
            {
                sqlCommand.Parameters[1].Value = UNIQUEKEY;
            }

            if (SOURCEFILE == null)
            {
                sqlCommand.Parameters[2].Value = DBNull.Value;
            }
            else
            {
                sqlCommand.Parameters[2].Value = SOURCEFILE;
            }

            if (DESCRIPTION == null)
            {
                sqlCommand.Parameters[3].Value = DBNull.Value;
            }
            else
            {
                sqlCommand.Parameters[3].Value = DESCRIPTION;
            }

            if (PKEY == null)
            {
                throw new ArgumentNullException("PKEY");
            }

            sqlCommand.Parameters[4].Value = PKEY;
            if (ADDNAME == null)
            {
                sqlCommand.Parameters[5].Value = DBNull.Value;
            }
            else
            {
                sqlCommand.Parameters[5].Value = ADDNAME;
            }

            if (ADDDATE.HasValue)
            {
                sqlCommand.Parameters[6].Value = ADDDATE.Value;
            }
            else
            {
                sqlCommand.Parameters[6].Value = DBNull.Value;
            }

            if (Type == null)
            {
                sqlCommand.Parameters[7].Value = DBNull.Value;
            }
            else
            {
                sqlCommand.Parameters[7].Value = Type;
            }

            ConnectionState state = sqlCommand.Connection.State;
            if ((sqlCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
            {
                sqlCommand.Connection.Open();
            }

            try
            {
                return sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                if (state == ConnectionState.Closed)
                {
                    sqlCommand.Connection.Close();
                }
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [HelpKeyword("vs.data.TableAdapter")]
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public virtual int Insert(string TABLENAME, string UNIQUEKEY, string SOURCEFILE, string DESCRIPTION, string PKEY, string ADDNAME, DateTime? ADDDATE)
        {
            if (TABLENAME == null)
            {
                Adapter.InsertCommand.Parameters[0].Value = DBNull.Value;
            }
            else
            {
                Adapter.InsertCommand.Parameters[0].Value = TABLENAME;
            }

            if (UNIQUEKEY == null)
            {
                Adapter.InsertCommand.Parameters[1].Value = DBNull.Value;
            }
            else
            {
                Adapter.InsertCommand.Parameters[1].Value = UNIQUEKEY;
            }

            if (SOURCEFILE == null)
            {
                Adapter.InsertCommand.Parameters[2].Value = DBNull.Value;
            }
            else
            {
                Adapter.InsertCommand.Parameters[2].Value = SOURCEFILE;
            }

            if (DESCRIPTION == null)
            {
                Adapter.InsertCommand.Parameters[3].Value = DBNull.Value;
            }
            else
            {
                Adapter.InsertCommand.Parameters[3].Value = DESCRIPTION;
            }

            if (PKEY == null)
            {
                throw new ArgumentNullException("PKEY");
            }

            Adapter.InsertCommand.Parameters[4].Value = PKEY;
            if (ADDNAME == null)
            {
                Adapter.InsertCommand.Parameters[5].Value = DBNull.Value;
            }
            else
            {
                Adapter.InsertCommand.Parameters[5].Value = ADDNAME;
            }

            if (ADDDATE.HasValue)
            {
                Adapter.InsertCommand.Parameters[6].Value = ADDDATE.Value;
            }
            else
            {
                Adapter.InsertCommand.Parameters[6].Value = DBNull.Value;
            }

            ConnectionState state = Adapter.InsertCommand.Connection.State;
            if ((Adapter.InsertCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
            {
                Adapter.InsertCommand.Connection.Open();
            }

            try
            {
                return Adapter.InsertCommand.ExecuteNonQuery();
            }
            finally
            {
                if (state == ConnectionState.Closed)
                {
                    Adapter.InsertCommand.Connection.Close();
                }
            }
        }

    }

}
