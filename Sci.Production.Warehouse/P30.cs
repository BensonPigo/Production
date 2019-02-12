using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P30 : Sci.Win.Tems.QueryForm
    {
        private DataSet ds;
        public P30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ds = null;
            listControlBindingSource1.DataSource = null;
            listControlBindingSource2.DataSource = null;

            #region Filter 表頭條件

            int selectindex = comboCategory.SelectedIndex;
            int selectindex2 = comboFabricType.SelectedIndex;
            string strCfmDate1 = null;
            string strCfmDate2 = null;
            string strSP = this.txtSP.Text;
            string strFactory = this.txtmfactory.Text;

            if (dateCfmDate.Value1 != null && dateCfmDate.Value2 != null)
            {
                strCfmDate1 = this.dateCfmDate.Text1;
                strCfmDate2 = this.dateCfmDate.Text2;
            }

            if (MyUtility.Check.Empty(strSP) && 
                MyUtility.Check.Empty(strCfmDate1) && 
                MyUtility.Check.Empty(strCfmDate2))
            {
                MyUtility.Msg.WarningBox("<SP#> , <Cfm Date> cannot be empty!");
                return;
            }
            #endregion

            StringBuilder sqlcmd = new StringBuilder();
            #region -- sql command --

            sqlcmd.Append($@"");
            #endregion


            
        }
    }
}
