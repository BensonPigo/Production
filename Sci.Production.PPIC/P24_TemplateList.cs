using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P24_TemplateList : Sci.Win.Forms.Base
    {
        string strPath;
        private string styleUkey;
        private string style;
        private string season;
        private string brand;
        private string type;
        private string table;

        /// <inheritdoc/>
        public P24_TemplateList(string styleID, string seasonID, string brandID, bool cannew, string type)
        {
            this.InitializeComponent();
            this.style = styleID;
            this.season = seasonID;
            this.brand = brandID;
            this.type = type;
            if (cannew)
            {
                this.btn_New.Enabled = true;
                this.btn_Remove.Enabled = true;
            }
            else
            {
                this.btn_New.Enabled = false;
                this.btn_Remove.Enabled = false;
            }

            switch (type)
            {
                case "Template":
                    this.table = "HandoverATUpload";
                    this.strPath = MyUtility.GetValue.Lookup(@"select HandoverATPath from System");
                    break;
                case "SpecialTools":
                    this.table = "HandoverSpecialToolsUpload";
                    this.strPath = MyUtility.GetValue.Lookup(@"select HandoverSpecialToolsPath from System");
                    break;
                case "FinalPattern":
                    this.table = "FinalPatternUpload";
                    this.strPath = MyUtility.GetValue.Lookup(@"select FinalPatternPath from System");
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || this.gridTemplateList == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.strPath))
            {
                MyUtility.Msg.WarningBox("Path not found.");
                return;
            }

            DataRow dr = this.gridTemplateList.GetDataRow(this.listControlBindingSource1.Position);

            if (!Directory.Exists(this.strPath))
            {
                MyUtility.Msg.WarningBox("Please check the path setting.");
                return;
            }

            DirectoryInfo diInfo = new DirectoryInfo(this.strPath);
            if (!diInfo.Exists)
            {
                MyUtility.Msg.WarningBox("Please check the path setting.");
                return;
            }

            FileInfo[] filelist = this.GetFile(diInfo, dr["SourceFile"].ToString()).ToArray();

            // if (!File.Exists(fullpath))
            if (filelist.Length == 0)
            {
                MyUtility.Msg.WarningBox("File not Exists!");
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "*|*",
                Title = "Save File",
                FileName = dr["SourceFile"].ToString(),
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.Copy(filelist[0].FullName, saveFileDialog1.FileName, true);
                }
                catch (IOException exception)
                {
                    MyUtility.Msg.ErrorBox("Error: Download file fail. Original error: " + exception.Message);
                }
            }
        }

        // 抓出子資料夾中所有符合filename的檔案資訊
        private List<FileInfo> GetFile(DirectoryInfo di, string filename)
        {
            List<FileInfo> listFile = new List<FileInfo>();

            foreach (DirectoryInfo sub_di in di.GetDirectories())
            {
                // Call itself to process any sub directories
                listFile.AddRange(this.GetFile(sub_di, filename));
            }

            listFile.AddRange(di.GetFiles().Where(x => x.EqualString(filename)).ToList());

            return listFile;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridTemplateList)
          .Text("SourceFile", header: "File Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
          .Text("Description", header: "Description", width: Widths.AnsiChars(41), iseditingreadonly: true)
          .Text("CreatedBy", header: "Created by", width: Widths.AnsiChars(30), iseditingreadonly: true)
          ;
            this.styleUkey = MyUtility.GetValue.Lookup($@"select Ukey from Style where ID = '{this.style}' and SeasonID = '{this.season}' and BrandID = 'ADIDAS'");

            switch (this.type)
            {
                case "Template":
                    this.table = "HandoverATUpload";
                    this.strPath = MyUtility.GetValue.Lookup(@"select HandoverATPath from System");
                    break;
                case "SpecialTools":
                    this.table = "HandoverSpecialToolsUpload";
                    this.strPath = MyUtility.GetValue.Lookup(@"select HandoverSpecialToolsPath from System");
                    break;
                case "FinalPattern":
                    this.table = "FinalPatternUpload";
                    this.strPath = MyUtility.GetValue.Lookup(@"select FinalPatternPath from System");
                    break;
                default:
                    break;
            }

            this.Query();
        }

        private void Query()
        {
            string sqlcmd = $@"
select h.* 
,[CreatedBy] = h.AddName+'-' + (select name from pass1 where ID = h.AddName) + ' '+ CONVERT(varchar, h.AddDate)
from {this.table} h
where h.StyleUkey = '{this.styleUkey}'
";
            this.listControlBindingSource1.DataSource = null;
            DualResult result;
            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt)))
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        /// <inheritdoc/>
        private void btn_New_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.strPath))
            {
                MyUtility.Msg.WarningBox("Path not found.");
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open file to upload";
            string fullFileName;
            string onlyFileName;

            // 開窗且有選擇檔案
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                fullFileName = dialog.FileName;
                onlyFileName = dialog.SafeFileName;
            }

            P24_WriteDesc writeDesc = new P24_WriteDesc();
            writeDesc.ShowDialog();
            string desc = writeDesc.strDesc;
            try
            {
                if (Directory.Exists(this.strPath) == false)
                {
                    try
                    {
                        Directory.CreateDirectory(this.strPath);
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Create file fail. Original error: " + exception.Message);
                    }
                }

                File.Copy(fullFileName, Path.Combine(this.strPath, onlyFileName), true);
            }
            catch (IOException exception)
            {
                MyUtility.Msg.ErrorBox("Error: New file fail. Original error: " + exception.Message);
                return;
            }

            string sqlInsert = $@"
insert into {this.table}(StyleUkey,SourceFile,Description,AddName,AddDate)
values('{this.styleUkey}','{onlyFileName}','{desc}','{Env.User.UserID}',GETDATE())
";
            DualResult result;
            if (!(result = DBProxy.Current.Execute(string.Empty, sqlInsert)))
            {
                this.ShowErr(result);
            }

            this.Query();
        }

        /// <inheritdoc/>
        private void btn_Remove_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || this.gridTemplateList == null)
            {
                return;
            }

            DataRow dr = this.gridTemplateList.GetDataRow(this.listControlBindingSource1.Position);
            string fullFileName = Path.Combine(this.strPath, dr["SourceFile"].ToString());
            if (File.Exists(fullFileName))
            {
                try
                {
                    File.Delete(fullFileName);
                    string sqlDelete = $@"delete from {this.table} where Ukey = '{dr["Ukey"]}'";
                    DualResult result;
                    if (!(result = DBProxy.Current.Execute(string.Empty, sqlDelete)))
                    {
                        this.ShowErr(result);
                    }
                }
                catch (IOException exception)
                {
                    MyUtility.Msg.WarningBox("Error: Remove file fail.Original error: " + exception.Message);
                }
            }

            this.Query();
        }
    }
}
