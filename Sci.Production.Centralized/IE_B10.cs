using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class IE_B10 : Win.Tems.Input1
    {
        private string destination_path;
        private Stream fileOpened = null;
        private DialogResult deleteResult1;
        private string IE_B10_TPE_Path = MyUtility.Convert.GetString(ConfigurationManager.AppSettings["IE_B10_TPE_Path"]);

        /// <inheritdoc/>
        public IE_B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = this.IE_B10_TPE_Path;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtID.ReadOnly = !this.IsDetailInserting;

            /*判斷路徑下圖片檔找不到,就將ImageLocation帶空值*/
            if (MyUtility.Check.Empty(this.CurrentMaintain["Picture1"]))
            {
                this.picture1.ImageLocation = string.Empty;
            }
            else if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"])))
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
            else if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"])))
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
            if (this.IsDetailInserting)
            {
                MyUtility.Msg.InfoBox("Please press 'Save' or 'Undo' button before editing pictures.");
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog
            {
                InitialDirectory = "c:\\", // 預設路徑
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.JPEG;*.PNG;*.GIF|All files (*.*)|*.*", // 使用檔名
                FilterIndex = 1,
                RestoreDirectory = true,
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                try
                {
                    if ((this.fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);

                            string newFileName = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

                            // 拿ID 去掉不合法字元，當作新檔名
                            char[] invalidChars = Path.GetInvalidFileNameChars();
                            foreach (char invalidChar in invalidChars)
                            {
                                newFileName = newFileName.Replace(invalidChar.ToString(), string.Empty);
                            }

                            string destination_fileName = $"B10_{newFileName}-1{local_file_type}";

                            File.Copy(local_path_file, this.destination_path + destination_fileName.Trim(), true);

                            // update picture1 path
                            DualResult result = Data.DBProxy.Current.Execute("ProductionTPE", $"update SewingMachineAttachment set Picture1 ='{destination_fileName.Trim()}' where ID=@ID", paras);

                            if (!result)
                            {
                                this.ShowErr(result);
                            }

                            this.picture1.ImageLocation = this.destination_path.Trim() + destination_fileName.Trim();
                            if (this.EditMode)
                            {
                                this.CurrentMaintain["Picture1"] = destination_fileName.Trim();
                            }
                            else
                            {
                                this.CurrentMaintain["Picture1"] = this.destination_path.Trim() + destination_fileName.Trim();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                finally
                {
                    this.RenewData();
                }
            }
        }

        private void BtnPicture1Delete_Click(object sender, EventArgs e)
        {
            if (this.IsDetailInserting)
            {
                MyUtility.Msg.InfoBox("Please press 'Save' or 'Undo' button before editing pictures.");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Picture1"]))
            {
                return;
            }

            this.deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture1 >?", buttons: MessageBoxButtons.YesNo);
            if (this.deleteResult1 == DialogResult.Yes)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

                if (File.Exists(this.destination_path + this.CurrentMaintain["Picture1"].ToString()))
                {
                    try
                    {
                        File.Delete(this.destination_path + this.CurrentMaintain["Picture1"].ToString());
                        this.CurrentMaintain["Picture1"] = string.Empty;
                        this.picture1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                        DualResult result = Data.DBProxy.Current.Execute("ProductionTPE", "update SewingMachineAttachment set Picture1='' where ID=@ID", paras);
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                    finally
                    {
                        this.RenewData();
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture1"] = string.Empty;
                    this.picture1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                    DualResult result = Data.DBProxy.Current.Execute("ProductionTPE", "update SewingMachineAttachment set Picture1='' where ID=@ID", paras);
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
            if (this.IsDetailInserting)
            {
                MyUtility.Msg.InfoBox("Please press 'Save' or 'Undo' button before editing pictures.");
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog
            {
                InitialDirectory = "c:\\", // 預設路徑
                Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*", // 使用檔名
                FilterIndex = 1,
                RestoreDirectory = true,
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                try
                {
                    if ((this.fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);

                            string newFileName = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

                            // 拿ID 去掉不合法字元，當作新檔名
                            char[] invalidChars = Path.GetInvalidFileNameChars();
                            foreach (char invalidChar in invalidChars)
                            {
                                newFileName = newFileName.Replace(invalidChar.ToString(), string.Empty);
                            }

                            string destination_fileName = $"B10_{newFileName}-2{local_file_type}";

                            File.Copy(local_path_file, this.destination_path + destination_fileName.Trim(), true);

                            // update picture2 path
                            DualResult result = Data.DBProxy.Current.Execute("ProductionTPE", $"update SewingMachineAttachment set Picture2 ='{destination_fileName.Trim()}' where ID=@ID", paras);

                            this.picture2.ImageLocation = this.destination_path.Trim() + destination_fileName.Trim();
                            if (this.EditMode)
                            {
                                this.CurrentMaintain["Picture2"] = destination_fileName.Trim();
                            }
                            else
                            {
                                this.CurrentMaintain["Picture2"] = this.destination_path.Trim() + destination_fileName.Trim();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                finally
                {
                    this.RenewData();
                }
            }
        }

        private void BtnPicture2Delete_Click(object sender, EventArgs e)
        {
            if (this.IsDetailInserting)
            {
                MyUtility.Msg.InfoBox("Please press 'Save' or 'Undo' button before editing pictures.");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Picture2"]))
            {
                return;
            }

            this.deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture2 >?", buttons: MessageBoxButtons.YesNo);
            if (this.deleteResult1 == DialogResult.Yes)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

                if (File.Exists(this.destination_path + this.CurrentMaintain["Picture2"].ToString()))
                {
                    try
                    {
                        File.Delete(this.destination_path + this.CurrentMaintain["Picture2"].ToString());
                        this.CurrentMaintain["Picture2"] = string.Empty;
                        this.picture2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                        DualResult result = Data.DBProxy.Current.Execute("ProductionTPE", "update SewingMachineAttachment set Picture2='' where ID=@ID", paras);
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                    finally
                    {
                        this.RenewData();
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture2"] = string.Empty;
                    this.picture2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                    DualResult result = Data.DBProxy.Current.Execute("ProductionTPE", "update SewingMachineAttachment set Picture2='' where ID=@ID", paras);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.InfoBox("<ID> cannot be empty!!");
                return false;
            }

            if (this.IsDetailInserting)
            {
                string newID = $@"{this.CurrentMaintain["ID"]}";

                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("newID", newID));
                string cmd = "select 1 from SewingMachineAttachment with(nolock) where ID=@newID";

                string dbVal = MyUtility.GetValue.Lookup(cmd, paras, connectionName: "ProductionTPE");
                if (!MyUtility.Check.Empty(dbVal))
                {
                    MyUtility.Msg.InfoBox("<ID> is duplicate key.");
                    return false;
                }
                else
                {
                    this.CurrentMaintain["ID"] = newID;
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
