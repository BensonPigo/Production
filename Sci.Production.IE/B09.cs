using System;
using System.Windows.Forms;
using Ict;
using System.IO;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B09
    /// </summary>
    public partial class B09 : Sci.Win.Tems.Input1
    {
        private Stream fileOpened = null;
        private DialogResult deleteResult1;
        private string destination_path;

        /// <summary>
        /// B09
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK)", null);
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayMachine.Text = MyUtility.GetValue.Lookup("Description", this.displayMachineTypeID.Text.ToString(), "MachineType", "ID");
            string selectCommand = string.Format("select Name from IESELECTCODE WITH (NOLOCK) where ID = '{0}' and  type='00001'", this.displayOperationType.Text.ToString());
            this.displayOperationType1.Text = MyUtility.GetValue.Lookup(selectCommand, null);
            selectCommand = string.Format("select Name from IESELECTCODE WITH (NOLOCK) where  ID = '{0}' and  type='00002'", this.displayCostCenter.Text.ToString());
            this.displayCostCenter1.Text = MyUtility.GetValue.Lookup(selectCommand, null);
            selectCommand = string.Format("select Name from IESELECTCODE WITH (NOLOCK) where ID = '{0}' and  type='00003'", this.displaySection.Text.ToString());
            this.displaySection1.Text = MyUtility.GetValue.Lookup(selectCommand, null);
            this.numMachineAllowance.Text = MyUtility.GetValue.Lookup("MachineAllow", this.displayMachineTypeID.Text.ToString(), "MachineType", "ID");
            this.numManualAllowance.Text = MyUtility.GetValue.Lookup("ManAllow", this.displayMachineTypeID.Text.ToString(), "MachineType", "ID");

            /*判斷路徑下圖片檔找不到,就將ImageLocation帶空值*/
            if (MyUtility.Check.Empty(this.CurrentMaintain["Picture1"]))
            {
                this.picture1.ImageLocation = string.Empty;
            }
            else
                if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"])))
            {
                this.picture1.ImageLocation = this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"]);
            }
            else
            {
                this.picture1.ImageLocation = string.Empty;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Picture2"]))
            {
                this.picture2.ImageLocation = string.Empty;
            }
            else
                if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"])))
            {
                this.picture2.ImageLocation = this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"]);
            }
            else
            {
                this.picture2.ImageLocation = string.Empty;
            }
        }

        private void BtnPicture1Attach_Click(object sender, EventArgs e)
        {
            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((this.fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);
                            string destination_fileName = this.CurrentMaintain["UKey"].ToString().Trim() + "-1" + local_file_type;

                            System.IO.File.Copy(local_path_file, this.destination_path + destination_fileName, true);

                            // update picture1 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture1 ='" + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]);
                            this.CurrentMaintain["Picture1"] = this.destination_path.Trim() + destination_fileName.Trim();
                            this.picture1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void BtnPicture1Delete_Click(object sender, EventArgs e)
        {
            this.deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture1 >?", buttons: MessageBoxButtons.YesNo);
            if (this.deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.IO.File.Exists(this.destination_path
                    + this.CurrentMaintain["Picture1"].ToString()))
                {
                    try
                    {
                        System.IO.File.Delete(this.destination_path + this.CurrentMaintain["Picture1"].ToString());
                        this.CurrentMaintain["Picture1"] = string.Empty;
                        this.picture1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Operation set Picture1='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
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
                    this.CurrentMaintain["Picture1"] = string.Empty;
                    this.picture1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Operation set Picture1='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }

        private void BtnPicture2Attach_Click(object sender, EventArgs e)
        {
            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((this.fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);
                            string destination_fileName = this.CurrentMaintain["UKey"] + "-2" + local_file_type;

                            System.IO.File.Copy(local_path_file, this.destination_path + destination_fileName, true);

                            // update picture2 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture2 ='" + destination_fileName.Trim() + "' where Ukey='" + this.CurrentMaintain["UKey"] + "'");
                            this.CurrentMaintain["Picture2"] = this.destination_path.Trim() + destination_fileName.Trim();
                            this.picture2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void BtnPicture2Delete_Click(object sender, EventArgs e)
        {
            this.deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture2 >?", buttons: MessageBoxButtons.YesNo);
            if (this.deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.IO.File.Exists(this.destination_path + this.CurrentMaintain["Picture2"].ToString()))
                {
                    try
                    {
                        System.IO.File.Delete(this.destination_path + this.CurrentMaintain["Picture2"].ToString());
                        this.CurrentMaintain["Picture2"] = string.Empty;
                        this.picture2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Operation set Picture2='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
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
                    this.CurrentMaintain["Picture2"] = string.Empty;
                    this.picture2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Operation set Picture2='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
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
