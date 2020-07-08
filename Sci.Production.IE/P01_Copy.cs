using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_Copy
    /// </summary>
    public partial class P01_Copy : Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P01CopyStyleData
        /// </summary>
        // public DataTable P01CopyStyleData;
        private DataTable _P01CopyStyleData;

        /// <summary>
        /// P01CopyStyleData
        /// </summary>
        public DataTable P01CopyStyleData
        {
            get
            {
                return this._P01CopyStyleData;
            }

            set
            {
                this._P01CopyStyleData = value;
            }
        }

        /// <summary>
        /// P01_Copy
        /// </summary>
        /// <param name="masterData">MasterData</param>
        public P01_Copy(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            MyUtility.Tool.SetupCombox(this.comboStyle, 1, 1, "T,B,I,O");
            this.txtStyle.Text = this.masterData["StyleID"].ToString();
            this.txtseason.Text = this.masterData["SeasonID"].ToString();
            this.txtBrand.Text = this.masterData["BrandID"].ToString();
            this.comboStyle.Text = this.masterData["ComboType"].ToString();
        }

        // Style
        private void TxtStyle_PopUp1(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item;
            string selectCommand;
            selectCommand = "select ID,SeasonID,Description,BrandID from Style WITH (NOLOCK) where Junk = 0 order by ID";

            item = new Win.Tools.SelectItem(selectCommand, "14,6,50,12", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            this.txtStyle.Text = item.GetSelectedString();
            this.txtseason.Text = selectedData[0]["SeasonID"].ToString();
            this.txtBrand.Text = selectedData[0]["BrandID"].ToString();
        }

        // Style
        private void TxtStyle_Validated1(object sender, EventArgs e)
        {
            this.GetBrand();
        }

        // Brand
        private void TxtBrand_PopUp1(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Brand WITH (NOLOCK) WHERE Junk=0  ORDER BY Id";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, "10,40,40", this.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtBrand.Text = item.GetSelectedString();
        }

        // Season
        private void Txtseason_Validated1(object sender, EventArgs e)
        {
            this.GetBrand();
        }

        private void GetBrand()
        {
            if (!MyUtility.Check.Empty(this.txtStyle.Text) && !MyUtility.Check.Empty(this.txtseason.Text))
            {
                // sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@id";
                sp1.Value = this.txtStyle.Text;
                sp2.ParameterName = "@seasonid";
                sp2.Value = this.txtseason.Text;

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                DataTable styleBrand;
                string sqlCmd = "select BrandID from Style WITH (NOLOCK) where ID = @id and SeasonID = @seasonid";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleBrand);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("SQL connection fail!!\r\n" + result.ToString());
                    return;
                }

                if (styleBrand.Rows.Count > 0)
                {
                    this.txtBrand.Text = MyUtility.Convert.GetString(styleBrand.Rows[0]["BrandID"]);
                }
                else
                {
                    this.txtBrand.Text = string.Empty;
                }
            }
        }

        // OK
        private void BtnOK_Click1(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtStyle.Text))
            {
                MyUtility.Msg.WarningBox("Style can't empty!");
                this.txtStyle.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtseason.Text))
            {
                MyUtility.Msg.WarningBox("Season can't empty!");
                this.txtseason.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtBrand.Text))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!");
                this.txtBrand.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.comboStyle.SelectedValue))
            {
                MyUtility.Msg.WarningBox("ComboType can't empty!");
                this.comboStyle.Focus();
                return;
            }

            // 檢查輸入的資料是否存在
            #region sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.txtStyle.Text;
            sp2.ParameterName = "@seasonid";
            sp2.Value = this.txtseason.Text;
            sp3.ParameterName = "@brandid";
            sp3.Value = this.txtBrand.Text;
            sp4.ParameterName = "@location";
            sp4.Value = this.comboStyle.SelectedValue;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            #endregion
            DataTable p01CopyStyleData_bak;
            string sqlCmd = @"select s.ID, s.SeasonID, s.BrandID,sl.Location
from Style s WITH (NOLOCK) 
inner join Style_Location sl WITH (NOLOCK) on s.Ukey = sl.StyleUkey
where s.ID = @id and s.SeasonID = @seasonid and s.BrandID = @brandid and sl.Location = @location";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out p01CopyStyleData_bak);
            this._P01CopyStyleData = p01CopyStyleData_bak.Copy();
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL connection fail!!\r\n" + result.ToString());
                return;
            }

            if (this.P01CopyStyleData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not exist!!");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
