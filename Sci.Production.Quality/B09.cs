using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B09 : Sci.Win.Tems.Input1
    {
        private string destination_path; // 放圖檔的路徑

        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK) ", null);
            this.pictureBoxSignature.SizeMode = PictureBoxSizeMode.Zoom;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtID.SetReadOnly(false);
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
            Stream fileOpened = null;

            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);
                            string destination_fileName = "QA_B09_" + MyUtility.Convert.GetString(this.CurrentMaintain["ID"]).Trim() + local_file_type;

                            System.IO.File.Copy(local_path_file, this.destination_path + destination_fileName, true);

                            // update picture1 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Technician set SignaturePic ='" + destination_fileName.Trim() + "' where ID ='" + this.CurrentMaintain["ID"] + "'");
                            this.CurrentMaintain["SignaturePic"] = destination_fileName.Trim();
                            this.pictureBoxSignature.ImageLocation = MyUtility.Convert.GetString(this.destination_path.Trim() + destination_fileName.Trim());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the <Signature Picture>?", buttons: MessageBoxButtons.YesNo);
            if (deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.IO.File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["SignaturePic"])))
                {
                    try
                    {
                        System.IO.File.Delete(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["SignaturePic"]));
                        this.CurrentMaintain["SignaturePic"] = string.Empty;
                        this.pictureBoxSignature.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["SignaturePic"]);
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Technician set SignaturePic='' where ID='{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    catch (System.IO.IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
                {
                    this.CurrentMaintain["SignaturePic"] = string.Empty;
                    this.pictureBoxSignature.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["SignaturePic"]);
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Technician set SignaturePic='' where ID='{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }
    }
}
