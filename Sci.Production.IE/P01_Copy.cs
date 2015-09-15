using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class P01_Copy : Sci.Win.Subs.Base
    {
        private DataRow masterData;
        public DataRow P01CopyStyleData;
        public P01_Copy(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "T,B,I,O");
            textBox1.Text = masterData["StyleID"].ToString();
            txtseason1.Text = masterData["SeasonID"].ToString();
            textBox2.Text = masterData["BrandID"].ToString();
            comboBox1.Text = masterData["ComboType"].ToString();
        }

        //Style
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string selectCommand;
            selectCommand = "select ID,SeasonID,Description,BrandID from Style where Junk = 0 order by ID";
            
            item = new Sci.Win.Tools.SelectItem(selectCommand, "16,10,50,8", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            IList<DataRow> selectedData = item.GetSelecteds();
            textBox1.Text = item.GetSelectedString();
            txtseason1.Text = (selectedData[0])["SeasonID"].ToString();
            textBox2.Text = (selectedData[0])["BrandID"].ToString();
        }

        //Style
        private void textBox1_Validated(object sender, EventArgs e)
        {
            GetBrand();
        }

        //Brand
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Brand WHERE Junk=0  ORDER BY Id";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "10,50,50", this.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox2.Text = item.GetSelectedString();
        }

        //Season
        private void txtseason1_Validated(object sender, EventArgs e)
        {
            GetBrand();
        }

        private void GetBrand()
        {
            if (!MyUtility.Check.Empty(textBox1.Text) && !MyUtility.Check.Empty(txtseason1.Text))
            {
                textBox2.Text = MyUtility.GetValue.Lookup(string.Format("select BrandID from Style where ID = '{0}' and SeasonID = '{1}'", textBox1.Text, txtseason1.Text));
            }
        }

        //OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(textBox1.Text))
            {
                MyUtility.Msg.WarningBox("Style can't empty!");
                textBox1.Focus();
                return;
            }

            if (MyUtility.Check.Empty(txtseason1.Text))
            {
                MyUtility.Msg.WarningBox("Season can't empty!");
                txtseason1.Focus();
                return;
            }

            if (MyUtility.Check.Empty(textBox2.Text))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!");
                textBox2.Focus();
                return;
            }

            if (MyUtility.Check.Empty(comboBox1.SelectedValue))
            {
                MyUtility.Msg.WarningBox("ComboType can't empty!");
                comboBox1.Focus();
                return;
            }

            //檢查輸入的資料是否存在
            string sqlCmd = string.Format(@"select s.ID, s.SeasonID, s.BrandID,sl.Location
from Style s
inner join Style_Location sl on s.Ukey = sl.StyleUkey
where s.ID = '{0}' and s.SeasonID = '{1}' and s.BrandID = '{2}' and sl.Location = '{3}'", textBox1.Text, txtseason1.Text, textBox2.Text, comboBox1.SelectedValue);
            if (!MyUtility.Check.Seek(sqlCmd, out P01CopyStyleData))
            {
                MyUtility.Msg.WarningBox("Data not exist!!");
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
