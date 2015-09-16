using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Miscellaneous
{
    public partial class B03 : Sci.Win.Tems.Input1
    {
        private Stream fileOpened = null;
        private DialogResult deleteResult1;
        private MessageBoxButtons buttons = MessageBoxButtons.YesNo;
        string clippath = MyUtility.GetValue.Lookup("Select clippath from system", null);
        string pic = "";
        private string DelepicPath1;

        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "PurchaseFrom = 'T'";
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (MyUtility.Check.Empty(CurrentMaintain["Pic"])) this.pictureBox1.ImageLocation = "";
            else this.pictureBox1.ImageLocation = clippath + this.CurrentMaintain["Pic"].ToString();
            if ((decimal)CurrentMaintain["InspLeadTime"]==0)
            {
                numericBox1.Text = MyUtility.GetValue.Lookup("Select MiscInspdate from System");
            }
            else
            {
                numericBox1.Text = CurrentMaintain["InspLeadTime"].ToString();
            }

        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.numericBox1.Text = CurrentMaintain["InspleadTime"].ToString();
            pic = CurrentMaintain["PIC"].ToString();
        }

        private void numericBox1_Validated(object sender, EventArgs e)
        {

            if (numericBox1.Text != CurrentMaintain["InspleadTime"].ToString() && numericBox1.Text != "0")
            {
                CurrentMaintain["InspleadTime"] = numericBox1.Text;
            }
        }
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            if (pic != CurrentMaintain["Pic"].ToString())
            {
                DelepicPath1 = clippath + pic;
                File.Delete(DelepicPath1);
            }
        }
        private void button10_Click(object sender, EventArgs e)
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
                            string extension = Path.GetExtension(file.FileName);
                            string ukey = MyUtility.GetValue.GetUKey("Misc", "ID");
                            string fileName = ukey + extension;
                            string copyPath = clippath + ukey + extension;

                            File.Copy(file.FileName, copyPath, true);
                            this.CurrentMaintain["Pic"] = fileName; //寫入資料
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.WarningBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture1 >?", "Warning", buttons);
            if (deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                this.CurrentMaintain["Pic"] = string.Empty; //清空Table 資料

                this.pictureBox1.ImageLocation = this.CurrentMaintain["Pic"].ToString();
            }
        }
    }
}
