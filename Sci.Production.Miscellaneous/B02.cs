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
    public partial class B02 : Sci.Win.Tems.Input1
    {
        private Stream fileOpened = null;
        private DialogResult deleteResult1;
        private MessageBoxButtons buttons = MessageBoxButtons.YesNo;
        string clippath = MyUtility.GetValue.Lookup("Select picpath from system", null);
        string pic = "";
        private string DelepicPath1;

        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "PurchaseFrom = 'L'";
            Dictionary<String, String> comboBox1_RowSource2 = new Dictionary<string, string>();
            comboBox1_RowSource2.Add("Maintenance", "Maintenance");
            comboBox1_RowSource2.Add("General Affair", "General Affair");
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource2, null);
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (MyUtility.Check.Empty(CurrentMaintain["Pic"])) this.pictureBox1.ImageLocation = "";
            else this.pictureBox1.ImageLocation = clippath + this.CurrentMaintain["Pic"].ToString();

            if (MyUtility.Check.Empty(CurrentMaintain["InspLeadTime"]) || CurrentMaintain["InspLeadTime"].ToString() =="0")
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
            this.textBox1.ReadOnly = true;
            this.numericBox1.Text = CurrentMaintain["InspleadTime"].ToString();
            pic = CurrentMaintain["Pic"].ToString();
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["PurchaseFrom"] = "L";
            CurrentMaintain["PurchaseType"] = "General Affair";
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("<ID> can not be empty.");
                textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["MiscBrandid"]))
            {
                MyUtility.Msg.WarningBox("<Brand> can not be empty.");
                txtmiscbrand1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Suppid"]))
            {
                MyUtility.Msg.WarningBox("<Supplier> can not be empty.");
                txtsubcon1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Unitid"]))
            {
                MyUtility.Msg.WarningBox("<Unit> can not be empty.");
                txtmmsunit1.Focus();
                return false;
            }
            return base.ClickSaveBefore();
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
        private void txtsubcon1_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(txtsubcon1.TextBox1.Text))
            {
                string currencyId = MyUtility.GetValue.Lookup("currencyId",CurrentMaintain["Suppid"].ToString(),"LocalSupp","id");
                CurrentMaintain["Currencyid"] = currencyId;
            }
        }

        private void numericBox1_Validated(object sender, EventArgs e)
        {

            if (numericBox1.Text != CurrentMaintain["InspleadTime"].ToString() && numericBox1.Text != "0")
            {
                CurrentMaintain["InspleadTime"] = numericBox1.Text;
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
