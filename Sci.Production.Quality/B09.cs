using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B09 : Win.Tems.Input1
    {
        private string destination_path; // 放圖檔的路徑
        private bool attach_flag = false;

        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK) ", null);
            this.pictureBoxSignature.SizeMode = PictureBoxSizeMode.Zoom;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.chkJunk.ForeColor = Color.Blue;
            this.chkGarmentTest.ForeColor = Color.Blue;
            this.chkP10.ForeColor = Color.Blue;
            this.chkP11.ForeColor = Color.Blue;
            this.chkP12.ForeColor = Color.Blue;
            this.chkP13.ForeColor = Color.Blue;
            this.txtID.SetReadOnly(false);
            if (this.EditMode == true)
            {
                this.btnAttach.Enabled = true;
                this.btnDelete.Enabled = true;
            }
            else
            {
                this.btnAttach.Enabled = false;
                this.btnDelete.Enabled = false;
            }

            /*判斷路徑下圖片檔找不到,就將ImageLocation帶空值*/
            if (MyUtility.Check.Empty(this.CurrentMaintain["SignaturePic"]))
            {
                this.pictureBoxSignature.ImageLocation = string.Empty;
            }
            else
            {
                if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["SignaturePic"])))
                {
                    try
                    {
                        this.pictureBoxSignature.ImageLocation = this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["SignaturePic"]);
                    }
                    catch (Exception e)
                    {
                        MyUtility.Msg.WarningBox("Picture1 process error. Please check it !!" + Environment.NewLine + e.ToString());
                        this.pictureBoxSignature.ImageLocation = string.Empty;
                    }
                }
                else
                {
                    this.pictureBoxSignature.ImageLocation = string.Empty;
                }
            }
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.SetReadOnly(true);
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                string local_path_file = file.FileName;
                this.pictureBoxSignature.ImageLocation = MyUtility.Convert.GetString(local_path_file);
                this.attach_flag = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the <Signature Picture>?", buttons: MessageBoxButtons.YesNo);
            if (deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                this.pictureBoxSignature.ImageLocation = string.Empty;
                this.attach_flag = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            // 依照attach_flag判斷新增編輯資料時是否有上傳圖檔
            if (this.attach_flag == true)
            {
                // 如果this.pictureBoxSignature.ImageLocation是空的表示刪除
                if (MyUtility.Check.Empty(this.pictureBoxSignature.ImageLocation) && !MyUtility.Check.Empty(this.CurrentMaintain["SignaturePic"]))
                {
                    // 清掉存放路徑的檔案
                    try
                    {
                        System.IO.File.Delete(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["SignaturePic"]));
                        this.CurrentMaintain["SignaturePic"] = string.Empty;
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else if (!MyUtility.Check.Empty(this.pictureBoxSignature.ImageLocation))
                {
                    string local_path_file = this.pictureBoxSignature.ImageLocation;
                    string local_file_type = Path.GetExtension(local_path_file);
                    string destination_fileName = "QA_B09_" + MyUtility.Convert.GetString(this.CurrentMaintain["ID"]).Trim() + local_file_type;
                    try
                    {
                        System.IO.File.Copy(local_path_file, this.destination_path + destination_fileName, true);
                        this.CurrentMaintain["SignaturePic"] = destination_fileName.Trim();
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: update file fail. Original error: " + exception.Message);
                    }
                }

                this.attach_flag = false;
            }

            return base.ClickSaveBefore();
        }
    }
}
