using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        private string destination_path; // 放的路徑
        private bool Upload_flag = false;
        string Destination_fileName;

        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select ShippingMarkPath from System WITH (NOLOCK) ", null);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnDownload.Enabled = !MyUtility.Check.Empty(this.CurrentMaintain["FileName"]);
        }

        private void TxtCTNRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select RefNo  from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' ", null, this.txtCTNRefno.Text);

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
                        System.IO.File.Copy(from_file_path, fbd.SelectedPath + @"\" + MyUtility.Convert.GetString(this.CurrentMaintain["FileName"]), true);
                    }
                    catch (System.IO.IOException exception)
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

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CustCD"]))
            {
                MyUtility.Msg.WarningBox("CustCD can not empty!");
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
                    System.IO.File.Copy(local_path_file, destination, true);
                    this.CurrentMaintain["FileName"] = this.Destination_fileName.Trim();
                }
                catch (System.IO.IOException exception)
                {
                    MyUtility.Msg.ErrorBox("Error: update file fail. Original error: " + exception.Message);
                    return new DualResult(false,exception);
                }

                this.Upload_flag = false;
            }

            return base.ClickSavePost();
        }

        protected override DualResult ClickDelete()
        {
            string fileName = this.CurrentMaintain["FileName"].ToString();

            // 清掉存放路徑的檔案
            try
            {
                string destination = Path.Combine(this.destination_path, fileName);
                System.IO.File.Delete(destination);
            }
            catch (System.IO.IOException exception)
            {
                MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
            }

            return base.ClickDelete();
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
    }
}
