using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.IO;

namespace Sci.Production.IE
{
    public partial class B09 : Sci.Win.Tems.Input1
    {
        private Stream fileOpened = null;
        private DialogResult deleteResult1;
        private MessageBoxButtons buttons = MessageBoxButtons.YesNo;

        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayBox3.Text = myUtility.Lookup("Description", this.displayBox2.Text.ToString(), "MachineType","ID");
            string selectCommand = string.Format("select Name from Reason where ReasonTypeID = 'OperationType' and ID = '{0}'", this.displayBox7.Text.ToString());
            this.displayBox8.Text = myUtility.Lookup(selectCommand,null);
            selectCommand = string.Format("select Name from Reason where ReasonTypeID = 'CostCenter' and ID = '{0}'", this.displayBox10.Text.ToString());
            this.displayBox9.Text = myUtility.Lookup(selectCommand, null);
            selectCommand = string.Format("select Name from Reason where ReasonTypeID = 'Section' and ID = '{0}'", this.displayBox12.Text.ToString());
            this.displayBox11.Text = myUtility.Lookup(selectCommand, null);
            this.numericBox8.Text = myUtility.Lookup("MachineAllow", this.displayBox2.Text.ToString(), "MachineType", "ID");
            this.numericBox9.Text = myUtility.Lookup("ManAllow", this.displayBox2.Text.ToString(), "MachineType", "ID");
            this.pictureBox1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
            this.pictureBox2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\"; //預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; //使用檔名
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
                            string destination_path = myUtility.Lookup("select PicPath from System", null);
                            string destination_fileName = (this.CurrentMaintain["UKey"].ToString()).Trim() + "-1" + local_file_type;

                            System.IO.File.Copy(local_path_file, destination_path + destination_fileName, true);

                            //update picture1 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture1 ='" + destination_path.Trim() + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]); ;
                            this.CurrentMaintain["Picture1"] = destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deleteResult1 = MessageBox.Show("Are you sure delete the < Picture1 >?", "Warning", buttons);
            if (deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.IO.File.Exists(CurrentMaintain["Picture1"].ToString()))
                {
                    try
                    {
                        System.IO.File.Delete(CurrentMaintain["Picture1"].ToString());
                        this.CurrentMaintain["Picture1"] = string.Empty;
                        this.pictureBox1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture1='' where UKey=" + this.CurrentMaintain["UKey"]);
                    }
                    catch (System.IO.IOException exception)
                    {
                        MessageBox.Show("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture1"] = string.Empty;
                    this.pictureBox1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture1='' where UKey=" + this.CurrentMaintain["UKey"]);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\"; //預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; //使用檔名
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
                            string destination_path = myUtility.Lookup("select PicPath from System", null);
                            string destination_fileName = this.CurrentMaintain["UKey"] + "-2" + local_file_type;

                            System.IO.File.Copy(local_path_file, destination_path + destination_fileName, true);

                            //update picture2 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture2 ='" + destination_path.Trim() + destination_fileName.Trim() + "' where Ukey='" + this.CurrentMaintain["UKey"] + "'"); ;
                            this.CurrentMaintain["Picture2"] = destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deleteResult1 = MessageBox.Show("Are you sure delete the < Picture2 >?", "Warning", buttons);
            if (deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.IO.File.Exists(CurrentMaintain["Picture2"].ToString()))
                {
                    try
                    {
                        System.IO.File.Delete(CurrentMaintain["Picture2"].ToString());
                        this.CurrentMaintain["Picture2"] = string.Empty;
                        this.pictureBox2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture2='' where UKey='" + this.CurrentMaintain["UKey"] + "'");
                    }
                    catch (System.IO.IOException exception)
                    {
                        MessageBox.Show("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture2"] = string.Empty;
                    this.pictureBox2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Operation set Picture2='' where UKey='" + this.CurrentMaintain["UKey"] + "'");
                }
            }
        }
    }
}
