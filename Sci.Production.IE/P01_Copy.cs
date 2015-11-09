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

namespace Sci.Production.IE
{
    public partial class P01_Copy : Sci.Win.Subs.Base
    {
        private DataRow masterData;
        public DataTable P01CopyStyleData;
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
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@id";
                sp1.Value = textBox1.Text;
                sp2.ParameterName = "@seasonid";
                sp2.Value = txtseason1.Text;

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                DataTable styleBrand;
                string sqlCmd = "select BrandID from Style where ID = @id and SeasonID = @seasonid";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleBrand);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("SQL connection fail!!\r\n"+result.ToString());
                    return;
                }

                if (styleBrand.Rows.Count > 0)
                {
                    textBox2.Text = MyUtility.Convert.GetString(styleBrand.Rows[0]["BrandID"]);
                }
                else
                {
                    textBox2.Text = "";
                }
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
            #region sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = textBox1.Text;
            sp2.ParameterName = "@seasonid";
            sp2.Value = txtseason1.Text;
            sp3.ParameterName = "@brandid";
            sp3.Value = textBox2.Text;
            sp4.ParameterName = "@location";
            sp4.Value = comboBox1.SelectedValue;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            #endregion
            string sqlCmd = @"select s.ID, s.SeasonID, s.BrandID,sl.Location
from Style s
inner join Style_Location sl on s.Ukey = sl.StyleUkey
where s.ID = @id and s.SeasonID = @seasonid and s.BrandID = @brandid and sl.Location = @location";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out P01CopyStyleData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL connection fail!!\r\n" + result.ToString());
                return;
            }
            if (P01CopyStyleData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not exist!!");
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
