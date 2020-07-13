using Ict;
using Sci.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class B02 : Win.Tems.Input1
    {
        private string destination_path; // 放的路徑
        private bool Upload_flag = false;
        string Destination_fileName;
        private Hashtable ht = new Hashtable();
        DataTable sizes;
        DataTable sizesAll;

        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select ShippingMarkPath from System WITH (NOLOCK) ", null);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Resources\");
            if (this.ht.Count == 0)
            {
                this.ht.Add("Picture1", path + "CTN.jpg");
                this.pictureBox1.ImageLocation = this.ht["Picture1"].ToString();
            }

            #region ComboBox
            DualResult result;
            string cmd = $@"
SELECT [ID]='' ,[SIze]='' 
UNION
SELECT ID, SIze 
FROM StickerSize WITH (NOLOCK) 
where junk <> 1";
            result = DBProxy.Current.Select(null, cmd, out this.sizes);
            if (!result)
            {
                this.ShowErr(result);
            }

            cmd = $@"
SELECT [ID]='' ,[SIze]='' 
UNION
SELECT ID, SIze 
FROM StickerSize WITH (NOLOCK) 
";
            result = DBProxy.Current.Select(null, cmd, out this.sizesAll);
            if (!result)
            {
                this.ShowErr(result);
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.ComboPressing2DataSource();
        }

        private void ComboPressing2DataSource()
        {
            if (this.comboStickerSize != null && this.CurrentMaintain != null)
            {
                if (this.EditMode && this.sizes != null)
                {
                    MyUtility.Tool.SetupCombox(this.comboStickerSize, 1, this.sizes);
                    this.comboStickerSize.DisplayMember = "Size";
                }

                if (!this.EditMode && this.sizesAll != null)
                {
                    MyUtility.Tool.SetupCombox(this.comboStickerSize, 1, this.sizesAll);
                    this.comboStickerSize.DisplayMember = "Size";
                }

                this.comboStickerSize.SelectedValue = this.CurrentMaintain["StickerSizeID"];
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnDownload.Enabled = !MyUtility.Check.Empty(this.CurrentMaintain["FileName"]);
            this.toolbar.cmdJunk.Enabled = !MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
            this.ComboPressing2DataSource();
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!this.EditMode && this.CurrentMaintain != null && this.tabs.SelectedIndex == 1)
            {
                bool junk = MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
                this.toolbar.cmdJunk.Enabled = !junk && this.Perm.Junk;
                this.toolbar.cmdUnJunk.Enabled = junk && this.Perm.Junk;
            }
            else
            {
                this.toolbar.cmdJunk.Enabled = false;
                this.toolbar.cmdUnJunk.Enabled = false;
            }
        }

        private void TxtCTNRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select RefNo  from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' ", null, this.txtCTNRefno.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCTNRefno.Text = item.GetSelectedString();
        }

        private void TxtCTNRefno_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtCTNRefno.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtCTNRefno.OldValue)
            {
                if (!MyUtility.Check.Seek($"Select 1 from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' and RefNo = '{textValue}'"))
                {
                    this.txtCTNRefno.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< RefNo : {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            string from_file_path = Path.Combine(this.destination_path, MyUtility.Convert.GetString(this.CurrentMaintain["FileName"]));

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    try
                    {
                        File.Copy(from_file_path, fbd.SelectedPath + @"\" + MyUtility.Convert.GetString(this.CurrentMaintain["FileName"]), true);
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Download file fail. Original error: " + exception.Message);
                    }
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.destination_path))
            {
                MyUtility.Msg.WarningBox("ShippingMarkPath not set!");
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "HTML Files(*.HTML)|*.HTML|All files (*.*)|*.*"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                string local_path_file = file.FileName;
                this.displayFileName.Tag = MyUtility.Convert.GetString(local_path_file);
                this.displayFileName.Text = MyUtility.Convert.GetString(local_path_file);
                this.Upload_flag = true;

                this.GetFilenNme(local_path_file);
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CTNRefno"]))
            {
                MyUtility.Msg.WarningBox("CTNRefno can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Side"]))
            {
                MyUtility.Msg.WarningBox("Side can not empty!");
                return false;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["FileName"]).Length > 40)
            {
                MyUtility.Msg.WarningBox("The length of file name is 40 words, please rename file name and upload again!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            // Upload_flag
            if (this.Upload_flag)
            {
                string local_path_file = this.displayFileName.Tag.ToString();
                string local_file_type = Path.GetExtension(local_path_file);

                this.GetFilenNme(local_path_file);

                try
                {
                    string destination = Path.Combine(this.destination_path, this.Destination_fileName);
                    File.Copy(local_path_file, destination, true);
                    this.CurrentMaintain["FileName"] = this.Destination_fileName.Trim();
                }
                catch (IOException exception)
                {
                    MyUtility.Msg.ErrorBox("Error: update file fail. Original error: " + exception.Message);
                    return new DualResult(false, exception);
                }

                this.Upload_flag = false;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {
            string fileName = this.CurrentMaintain["FileName"].ToString();

            // 清掉存放路徑的檔案
            try
            {
                string destination = Path.Combine(this.destination_path, fileName);
                File.Delete(destination);
            }
            catch (IOException exception)
            {
                MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
            }

            return base.ClickDelete();
        }

        /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            string sqlcmd = $@"update ShippingMarkStamp set junk = 1 
where BrandID = '{this.CurrentMaintain["BrandID"]}'
and CTNRefno = '{this.CurrentMaintain["CTNRefno"]}'
and Side = '{this.CurrentMaintain["Side"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            string sqlcmd = $@"update ShippingMarkStamp set junk = 0
where BrandID = '{this.CurrentMaintain["BrandID"]}'
and CTNRefno = '{this.CurrentMaintain["CTNRefno"]}'
and Side = '{this.CurrentMaintain["Side"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        private void GetFilenNme(string from_path_file)
        {
            string local_file_type = Path.GetExtension(from_path_file);
            string filename = Path.GetFileName(from_path_file);
            this.Destination_fileName = Path.GetFileName(from_path_file);
            if (this.Destination_fileName.Length > 40)
            {
                MyUtility.Msg.WarningBox("The length of file name is 40 words, please rename file name and upload again!");
                this.CurrentMaintain["FileName"] = string.Empty;
                this.Destination_fileName = string.Empty;
                this.displayFileName.Tag = string.Empty;
                this.Upload_flag = false;
                return;
            }

            this.CurrentMaintain["FileName"] = this.Destination_fileName;
        }

        private void ComboStickerSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboStickerSize.SelectedIndex == -1)
            {
                return;
            }

            DataTable dt;
            DualResult result;
            Int64 id = Convert.ToInt64(this.comboStickerSize.SelectedValue);
            string cmd = "SELECT  Size ,Width,Length FROM StickerSize WITH(NOLOCK) WHERE ID=@ID";
            List<SqlParameter> paras = new List<SqlParameter>();

            paras.Add(new SqlParameter("@ID", id));

            result = DBProxy.Current.Select(null, cmd, paras, out dt);
            if (result)
            {
                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    this.CurrentMaintain["StampLength"] = Convert.ToInt32(dt.Rows[0]["Length"]);
                    this.CurrentMaintain["StampWidth"] = Convert.ToInt32(dt.Rows[0]["Width"]);
                }
            }
            else
            {
                this.ShowErr(result);
            }

            if (this.CurrentMaintain != null && this.EditMode)
            {
                this.CurrentMaintain["StickerSizeID"] = id;
            }
        }
    }
}
