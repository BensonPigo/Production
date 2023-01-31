using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B10
    /// </summary>
    public partial class B10 : Win.Tems.Input1
    {
        private string oldMoldID;
        private string oldMachineMasterGroupID;
        private string oldAttachmentTypeID;
        private string oldMeasurementID;
        private string oldFoldTypeID;
        private string oldSupplier1PartNo;
        private string oldSupplier1BrandID;
        private string oldSupplier2PartNo;
        private string oldSupplier2BrandID;
        private string oldSupplier3PartNo;
        private string oldSupplier3BrandID;
        private Stream fileOpened = null;
        private DialogResult deleteResult1;
        private string destination_path;

        /// <summary>
        /// B10
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK)", null);

        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DualResult result;
            DataTable dt;

            this.oldMoldID = MyUtility.Convert.GetString(this.CurrentMaintain["MoldID"]);
            this.oldMachineMasterGroupID = MyUtility.Convert.GetString(this.CurrentMaintain["MachineMasterGroupID"]);
            this.oldAttachmentTypeID = MyUtility.Convert.GetString(this.CurrentMaintain["AttachmentTypeID"]);
            this.oldMeasurementID = MyUtility.Convert.GetString(this.CurrentMaintain["MeasurementID"]);
            this.oldFoldTypeID = MyUtility.Convert.GetString(this.CurrentMaintain["FoldTypeID"]);
            this.oldSupplier1PartNo = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier1PartNo"]);
            this.oldSupplier1BrandID = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier1BrandID"]);
            this.oldSupplier2PartNo = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier2PartNo"]);
            this.oldSupplier2BrandID = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier2BrandID"]);
            this.oldSupplier3PartNo = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier3PartNo"]);
            this.oldSupplier3BrandID = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier3BrandID"]);
            this.disFoldTypeID.Text = MyUtility.Convert.GetString(this.CurrentMaintain["AttachmentFoldTypeDesc"]);

            string supplier1BrandID = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier1BrandID"]);
            string supplier2BrandID = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier2BrandID"]);
            string supplier3BrandID = MyUtility.Convert.GetString(this.CurrentMaintain["Supplier3BrandID"]);
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@supplier1BrandID", supplier1BrandID),
                new SqlParameter("@supplier2BrandID", supplier2BrandID),
                new SqlParameter("@supplier3BrandID", supplier3BrandID),
            };
            string sqlCmd = $@"Select Name From ExtendServer.Machine.dbo.PartBrand where ID = @supplier1BrandID";

            if (result = DBProxy.Current.Select(null, sqlCmd, parameters, out dt))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string supplier1BrandIDName = MyUtility.Convert.GetString(dt.Rows[0]["Name"]);
                    this.txtSupplier1BrandID.Text = supplier1BrandIDName;
                }
            }
            else
            {
                this.ShowErr(result);
            }

            sqlCmd = $@"Select Name From ExtendServer.Machine.dbo.PartBrand where ID = @supplier2BrandID";
            if (result = DBProxy.Current.Select(null, sqlCmd, parameters, out dt))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string supplier2BrandIDName = MyUtility.Convert.GetString(dt.Rows[0]["Name"]);
                    this.txtSupplier2BrandID.Text = supplier2BrandIDName;
                }
            }
            else
            {
                this.ShowErr(result);
            }

            sqlCmd = $@"Select Name From ExtendServer.Machine.dbo.PartBrand where ID = @Supplier3BrandID";
            if (result = DBProxy.Current.Select(null, sqlCmd, parameters, out dt))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string supplier3BrandIDName = MyUtility.Convert.GetString(dt.Rows[0]["Name"]);
                    this.txtSupplier3BrandID.Text = supplier3BrandIDName;
                }
            }
            else
            {
                this.ShowErr(result);
            }

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

        private void TxtMoldID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT ID FROM Mold WHERE Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "ID", "10", string.Empty, "ID");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["MoldID"] = item.GetSelectedString();
            this.oldMoldID = this.CurrentMaintain["MoldID"].ToString();
        }

        private void TxtMoldID_Validating(object sender, CancelEventArgs e)
        {
            string newMoldID = this.txtMoldID.Text;
            if (this.oldMoldID != newMoldID)
            {
                if (MyUtility.Check.Empty(newMoldID))
                {
                    this.CurrentMaintain["MoldID"] = string.Empty;
                    this.txtMoldID.Text = string.Empty;
                    return;
                }

                string cmd = $@"SELECT ID FROM Mold WHERE ID = @MoldID AND Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@MoldID", newMoldID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["MoldID"] = DBNull.Value;
                    this.txtMoldID.Text = string.Empty;
                    this.oldMoldID = string.Empty;
                    MyUtility.Msg.WarningBox("Data does not exist. Please check [B07. Attachment List].");
                }
                else
                {
                    this.CurrentMaintain["MoldID"] = MyUtility.Convert.GetString(dt.Rows[0]["ID"]);
                    this.txtMoldID.Text = newMoldID;
                    this.oldMoldID = this.CurrentMaintain["MoldID"].ToString();
                }
            }
        }

        private void TxtMachineMasterGroupID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT ID, Description FROM SciMachine_MachineMasterGroup WHERE Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "10,40", "ID,Description", string.Empty, "Machine Master Group");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.CurrentMaintain["MachineMasterGroupID"] = selectedData[0]["ID"].ToString();
            this.disMachineMasterGroupID.Text = selectedData[0]["Description"].ToString();
            this.oldMachineMasterGroupID = this.CurrentMaintain["MachineMasterGroupID"].ToString();
        }

        private void TxtMachineMasterGroupID_Validating(object sender, CancelEventArgs e)
        {
            string newMachineMasterGroupID = this.txtMachineMasterGroupID.Text;
            if (this.oldMachineMasterGroupID != newMachineMasterGroupID)
            {
                if (MyUtility.Check.Empty(newMachineMasterGroupID))
                {
                    this.CurrentMaintain["MachineMasterGroupID"] = string.Empty;
                    this.txtMachineMasterGroupID.Text = string.Empty;
                    return;
                }

                string cmd = $@"SELECT ID, Description FROM SciMachine_MachineMasterGroup WHERE Junk=0 AND ID=@ID";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newMachineMasterGroupID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["MachineMasterGroupID"] = DBNull.Value;
                    this.disMachineMasterGroupID.Text = string.Empty;
                    this.oldMachineMasterGroupID = string.Empty;
                    MyUtility.Msg.WarningBox("<Machine> does not exist. Please check [B12 Machine Master Group].");
                }
                else
                {
                    this.CurrentMaintain["MachineMasterGroupID"] = MyUtility.Convert.GetString(dt.Rows[0]["ID"]);
                    this.disMachineMasterGroupID.Text = MyUtility.Convert.GetString(dt.Rows[0]["Description"]);
                    this.oldMachineMasterGroupID = this.CurrentMaintain["MachineMasterGroupID"].ToString();
                }
            }
        }

        private void TxtAttachmentTypeID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT Type  FROM AttachmentType where Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "20", "Type ", string.Empty, "Type ");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.CurrentMaintain["AttachmentTypeID"] = selectedData[0]["Type"].ToString();
            this.oldAttachmentTypeID = this.CurrentMaintain["AttachmentTypeID"].ToString();
        }

        private void TxtAttachmentTypeID_Validating(object sender, CancelEventArgs e)
        {
            string newAttachmentTypeID = this.txtAttachmentTypeID.Text;
            if (this.oldAttachmentTypeID != newAttachmentTypeID)
            {
                if (MyUtility.Check.Empty(newAttachmentTypeID))
                {
                    this.CurrentMaintain["AttachmentTypeID"] = string.Empty;
                    this.txtAttachmentTypeID.Text = string.Empty;
                    return;
                }

                string cmd = $@"SELECT Type FROM AttachmentType WHERE Type = @Type and Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@Type", newAttachmentTypeID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["AttachmentTypeID"] = DBNull.Value;
                    this.txtAttachmentTypeID.Text = string.Empty;
                    this.oldMachineMasterGroupID = string.Empty;
                    MyUtility.Msg.WarningBox("<Type> does not exist. Please check [B13. Attachment Type].");
                }
                else
                {
                    this.CurrentMaintain["AttachmentTypeID"] = MyUtility.Convert.GetString(dt.Rows[0]["Type"]);
                    this.txtAttachmentTypeID.Text = newAttachmentTypeID;
                    this.oldAttachmentTypeID = this.CurrentMaintain["AttachmentTypeID"].ToString();
                }
            }
        }

        private void TxtMeasurementID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT Measurement FROM AttachmentMeasurement where Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "20", "Measurement", string.Empty, "Measurement");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.CurrentMaintain["MeasurementID"] = selectedData[0]["Measurement"].ToString();
            this.oldAttachmentTypeID = this.CurrentMaintain["MeasurementID"].ToString();
        }

        private void TxtMeasurementID_Validating(object sender, CancelEventArgs e)
        {
            string newMeasurementID = this.txtMeasurementID.Text;
            if (this.oldMeasurementID != newMeasurementID)
            {
                if (MyUtility.Check.Empty(newMeasurementID))
                {
                    this.CurrentMaintain["MeasurementID"] = string.Empty;
                    this.txtMeasurementID.Text = string.Empty;
                    return;
                }

                string cmd = $@"SELECT Measurement FROM AttachmentMeasurement WHERE Measurement = @Measurement and Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@Measurement", newMeasurementID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["MeasurementID"] = DBNull.Value;
                    this.txtMeasurementID.Text = string.Empty;
                    this.oldMeasurementID = string.Empty;
                    MyUtility.Msg.WarningBox("<Measurement> does not exist. Please check [B14. Attachment Measurement].");
                }
                else
                {
                    this.CurrentMaintain["MeasurementID"] = MyUtility.Convert.GetString(dt.Rows[0]["Measurement"]);
                    this.txtMeasurementID.Text = newMeasurementID;
                    this.oldMeasurementID = this.CurrentMaintain["MeasurementID"].ToString();
                }
            }
        }

        private void TxtFoldTypeID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT FoldType,Description FROM AttachmentFoldType where Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "20", "FoldType,Description", string.Empty, "FoldType");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.CurrentMaintain["FoldTypeID"] = selectedData[0]["FoldType"].ToString();
            this.disFoldTypeID.Text = selectedData[0]["Description"].ToString();
            this.oldFoldTypeID = this.CurrentMaintain["FoldTypeID"].ToString();
        }

        private void TxtFoldTypeID_Validating(object sender, CancelEventArgs e)
        {
            string newFoldTypeID = this.txtFoldTypeID.Text;
            if (this.oldFoldTypeID != newFoldTypeID)
            {
                if (MyUtility.Check.Empty(newFoldTypeID))
                {
                    this.CurrentMaintain["FoldTypeID"] = string.Empty;
                    this.txtFoldTypeID.Text = string.Empty;
                    this.disFoldTypeID.Text = string.Empty;
                    return;
                }

                string cmd = $@"SELECT FoldType,Description FROM AttachmentFoldType WHERE FoldType = @FoldType and Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@FoldType", newFoldTypeID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["FoldTypeID"] = DBNull.Value;
                    this.disFoldTypeID.Text = string.Empty;
                    this.oldFoldTypeID = string.Empty;
                    MyUtility.Msg.WarningBox("<Direction/FoldType> does not exist. Please check [B15. Attachment Direction Fold Type].");
                }
                else
                {
                    this.CurrentMaintain["FoldTypeID"] = MyUtility.Convert.GetString(dt.Rows[0]["FoldType"]);
                    this.disFoldTypeID.Text = this.CurrentMaintain["Description"].ToString();
                    this.oldFoldTypeID = this.CurrentMaintain["FoldTypeID"].ToString();
                }
            }
        }

        private void TxtSupplier1BrandID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT ID, Name FROM SciMachine_PartBrand where Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "10,20", "ID,Name", string.Empty, "Supplier Part# - 1 Brand");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.CurrentMaintain["Supplier1BrandID"] = selectedData[0]["ID"].ToString();
            this.oldFoldTypeID = this.CurrentMaintain["Supplier1BrandID"].ToString();

        }

        private void TxtSupplier1BrandID_Validating(object sender, CancelEventArgs e)
        {
            string newSupplier1BrandID = this.txtSupplier1BrandID.Text;
            if (this.oldSupplier1BrandID != newSupplier1BrandID)
            {
                if (MyUtility.Check.Empty(newSupplier1BrandID))
                {
                    this.CurrentMaintain["Supplier1BrandID"] = string.Empty;
                    return;
                }

                string cmd = $@"SELECT ID, Name FROM SciMachine_PartBrand WHERE ID = @ID and Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newSupplier1BrandID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["Supplier1BrandID"] = string.Empty;
                    this.oldSupplier1BrandID = string.Empty;
                    MyUtility.Msg.WarningBox("Data not found !!");
                }
                else
                {
                    this.CurrentMaintain["Supplier1BrandID"] = MyUtility.Convert.GetString(dt.Rows[0]["ID"]);
                    this.oldSupplier1BrandID = this.CurrentMaintain["Supplier1BrandID"].ToString();
                }
            }
        }

        private void TxtSupplier2BrandID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT ID, Name FROM SciMachine_PartBrand where Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "10,20", "ID,Name", string.Empty, "Supplier Part# - 2 Brand");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.CurrentMaintain["Supplier2BrandID"] = selectedData[0]["ID"].ToString();
            this.oldFoldTypeID = this.CurrentMaintain["Supplier2BrandID"].ToString();

        }

        private void TxtSupplier2BrandID_Validating(object sender, CancelEventArgs e)
        {
            string newSupplier2BrandID = this.txtSupplier2BrandID.Text;
            if (this.oldSupplier2BrandID != newSupplier2BrandID)
            {
                if (MyUtility.Check.Empty(newSupplier2BrandID))
                {
                    this.CurrentMaintain["Supplier2BrandID"] = string.Empty;
                    return;
                }

                string cmd = $@"SELECT ID, Name FROM SciMachine_PartBrand WHERE ID = @ID and Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newSupplier2BrandID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["Supplier2BrandID"] = string.Empty;
                    this.oldSupplier2BrandID = string.Empty;
                    MyUtility.Msg.WarningBox("Data not found !!");
                }
                else
                {
                    this.CurrentMaintain["Supplier2BrandID"] = MyUtility.Convert.GetString(dt.Rows[0]["ID"]);
                    this.oldSupplier2BrandID = this.CurrentMaintain["Supplier2BrandID"].ToString();
                }
            }
        }

        private void TxtSupplier3BrandID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT ID, Name FROM SciMachine_PartBrand where Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "10,20", "ID,Name", string.Empty, "Supplier Part# - 3 Brand");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            var selectedData = item.GetSelecteds();
            this.CurrentMaintain["Supplier3BrandID"] = selectedData[0]["ID"].ToString();
            this.oldFoldTypeID = this.CurrentMaintain["Supplier3BrandID"].ToString();
        }

        private void TxtSupplier3BrandID_Validating(object sender, CancelEventArgs e)
        {
            string newSupplier3BrandID = this.txtSupplier3BrandID.Text;
            if (this.oldSupplier3BrandID != newSupplier3BrandID)
            {
                if (MyUtility.Check.Empty(newSupplier3BrandID))
                {
                    this.CurrentMaintain["Supplier3BrandID"] = string.Empty;
                    return;
                }

                string cmd = $@"SELECT ID, Name FROM SciMachine_PartBrand WHERE ID = @ID and Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newSupplier3BrandID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["Supplier3BrandID"] = string.Empty;
                    this.oldSupplier3BrandID = string.Empty;
                    MyUtility.Msg.WarningBox("Data not found !!");
                }
                else
                {
                    this.CurrentMaintain["Supplier3BrandID"] = MyUtility.Convert.GetString(dt.Rows[0]["ID"]);
                    this.oldSupplier3BrandID = this.CurrentMaintain["Supplier3BrandID"].ToString();
                }
            }
        }

        private void BtnPicture1Attach_Click(object sender, EventArgs e)
        {
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
                paras.Add(new SqlParameter("@Ukey", MyUtility.Convert.GetString(this.CurrentMaintain["Ukey"])));
                try
                {
                    if ((this.fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);
                            string destination_fileName = "SewingMachineAttachmentUkey_" + this.CurrentMaintain["Ukey"].ToString().Trim() + "-1" + local_file_type;

                            File.Copy(local_path_file, this.destination_path + destination_fileName, true);

                            // update picture1 path
                            DualResult result = Data.DBProxy.Current.Execute(null, "update SewingMachineAttachment set Picture1 ='" + destination_fileName.Trim() + "' where Ukey=@Ukey", paras);
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
            if (this.deleteResult1 == DialogResult.Yes)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@Ukey", MyUtility.Convert.GetString(this.CurrentMaintain["Ukey"])));
                if (File.Exists(this.destination_path
                    + this.CurrentMaintain["Picture1"].ToString()))
                {
                    try
                    {
                        File.Delete(this.destination_path + this.CurrentMaintain["Picture1"].ToString());
                        this.CurrentMaintain["Picture1"] = string.Empty;
                        this.picture1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                        DualResult result = Data.DBProxy.Current.Execute(null, "update SewingMachineAttachment set Picture1='' where Ukey=@Ukey", paras);
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
                }
                else
                {
                    this.CurrentMaintain["Picture1"] = string.Empty;
                    this.picture1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                    DualResult result = Data.DBProxy.Current.Execute(null, "update SewingMachineAttachment set Picture1='' where Ukey=@Ukey", paras);
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
                paras.Add(new SqlParameter("@Ukey", MyUtility.Convert.GetString(this.CurrentMaintain["Ukey"])));
                try
                {
                    if ((this.fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);
                            string destination_fileName = "SewingMachineAttachmentUkey_" + this.CurrentMaintain["Ukey"] + "-2" + local_file_type;

                            File.Copy(local_path_file, this.destination_path + destination_fileName, true);

                            // update picture2 path
                            DualResult result = Data.DBProxy.Current.Execute(null, "update SewingMachineAttachment set Picture2 ='" + destination_fileName.Trim() + "' where Ukey=@Ukey", paras);
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
            if (this.deleteResult1 == DialogResult.Yes)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@Ukey", MyUtility.Convert.GetString(this.CurrentMaintain["Ukey"])));
                if (File.Exists(this.destination_path + this.CurrentMaintain["Picture2"].ToString()))
                {
                    try
                    {
                        File.Delete(this.destination_path + this.CurrentMaintain["Picture2"].ToString());
                        this.CurrentMaintain["Picture2"] = string.Empty;
                        this.picture2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                        DualResult result = Data.DBProxy.Current.Execute(null, "update SewingMachineAttachment set Picture2='' where Ukey=@Ukey", paras);
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
                }
                else
                {
                    this.CurrentMaintain["Picture2"] = string.Empty;
                    this.picture2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                    DualResult result = Data.DBProxy.Current.Execute(null, "update SewingMachineAttachment set Picture2='' where Ukey=@Ukey", paras);
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["MachineMasterGroupID"]) || MyUtility.Check.Empty(this.CurrentMaintain["AttachmentTypeID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["MeasurementID"]) || MyUtility.Check.Empty(this.CurrentMaintain["FoldTypeID"]))
            {
                MyUtility.Msg.InfoBox("<Machine>,<Type>,<Measurement>,<Direction/Fold Type> cannot be empty!!");
                return false;
            }

            string newID = $@"{this.CurrentMaintain["MachineMasterGroupID"]}-{this.CurrentMaintain["AttachmentTypeID"]}-{this.CurrentMaintain["MeasurementID"]}-{this.CurrentMaintain["FoldTypeID"]}";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("newID", newID));
            string cmd = "select 1 from SewingMachineAttachment with(nolock) where ID=@newID";

            string dbVal = MyUtility.GetValue.Lookup(cmd, paras);
            if (!MyUtility.Check.Empty(dbVal))
            {
                MyUtility.Msg.InfoBox("<Machine>,<Type>,<Measurement>,<Direction/Fold Type> is duplicate key.");
                return false;
            }
            else
            {
                this.CurrentMaintain["ID"] = newID;
            }

            return base.ClickSaveBefore();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            B10_Import callNextForm = new B10_Import();
            callNextForm.ShowDialog(this);
            this.Refresh();
        }
    }
}
