using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static Ict.Win.Design.DateTimeConverter;

namespace Sci.Production.PPIC
{
    public partial class P24_Critical_Operations : Sci.Win.Forms.Base
    {
        string strPath;
        string strStyleID;

        public P24_Critical_Operations(string StyleID)
        {
            this.InitializeComponent();
            this.strStyleID = StyleID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || this.gridCritical_Operations == null || this.gridCritical_Operations.Rows.Count <= 0)
            {
                return;
            }

            DataRow dr = this.gridCritical_Operations.GetDataRow(this.listControlBindingSource1.Position);
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

            FileInfo[] filelist = this.GetFile(diInfo, dr["FileName"].ToString()).ToArray();

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
                FileName = dr["FileName"].ToString(),
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

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridCritical_Operations)
          .Text("FileName", header: "File Name", width: Widths.AnsiChars(50), iseditingreadonly: true)
          ;
            this.gridCritical_Operations.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.gridCritical_Operations.DefaultCellStyle.Font = new Font(this.gridCritical_Operations.DefaultCellStyle.Font.Name, 14);
            this.Query();
        }

        private void Query()
        {
            string[] dirs;
            string sqlcmd = $@"
select CriticalOperationPath from System
";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drSystem))
            {
                this.strPath = Path.Combine(drSystem["CriticalOperationPath"].ToString(), this.strStyleID);
            }
            else
            {
                this.strPath = string.Empty;
                return;
            }

            DataTable dtGrid = new DataTable();
            dtGrid.Columns.Add("FileName", typeof(string));

            if (!MyUtility.Check.Empty(this.strPath))
            {
                if (Directory.Exists(this.strPath) == true)
                {
                    //try
                    //{
                    //    Directory.CreateDirectory(this.strPath);
                    //}
                    //catch (IOException exception)
                    //{
                    //    MyUtility.Msg.ErrorBox("Error: Create file fail. Original error: " + exception.Message);
                    //}

                    dirs = Directory.GetFileSystemEntries(this.strPath);
                    foreach (string item in dirs)
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["FileName"] = Path.GetFileName(item.ToString());
                        dtGrid.Rows.Add(dr);
                    }
                }
            }

            this.listControlBindingSource1.DataSource = dtGrid;
        }
    }
}
