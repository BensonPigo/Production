using Ict;
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
                        MyUtility.Msg.ErrorBox("Error: update file fail. Original error: " + exception.Message);
                    }
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
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
            // Upload_flag
            if (this.Upload_flag)
            {
                // 如果是空的表示刪除
                if (MyUtility.Check.Empty(this.displayFileName.Tag.ToString()))
                {
                    // 清掉存放路徑的檔案
                    try
                    {
                        System.IO.File.Delete(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["FileName"]));
                    }
                    catch (System.IO.IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
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
                    }
                }

                this.Upload_flag = false;
            }

            return base.ClickSaveBefore();
        }

        private void GetFilenNme(string from_path_file)
        {
            string local_file_type = Path.GetExtension(from_path_file);
            string brandIDCustCD = MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]).Trim() + MyUtility.Convert.GetString(this.CurrentMaintain["CustCD"]).Trim();
            string brandIDCustCDFileName = MyUtility.GetValue.Lookup($"select max(FileName) from ShippingMarkStamp with(nolock) where FileName like '{brandIDCustCD}%'");
            int num = 1;
            if (!MyUtility.Check.Empty(brandIDCustCDFileName))
            {
                num = MyUtility.Convert.GetInt(brandIDCustCDFileName.Substring(brandIDCustCDFileName.Length - 8, 3)) + 1; // 取檔案名末3碼數值, .html有5碼
            }

            this.Destination_fileName = brandIDCustCD + "_" + num.ToString().PadLeft(3, '0') + local_file_type;
            this.CurrentMaintain["FileName"] = this.Destination_fileName.Trim();
        }
    }
}
