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
    public partial class P03_CopyFromOtherStyle : Sci.Win.Subs.Base
    {
        public DataRow P03CopyLineMapping;

        public P03_CopyFromOtherStyle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string style, season, brand, version;
            style = txtstyle1.Text;
            season = txtseason1.Text;
            brand = txtbrand1.Text;
            version = textBox1.Text;

            if (MyUtility.Check.Empty(style))
            {
                MyUtility.Msg.WarningBox("Style# can't empty!!");
                txtstyle1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(season))
            {
                MyUtility.Msg.WarningBox("Season can't empty!!");
                txtseason1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(brand))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                txtbrand1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(version))
            {
                MyUtility.Msg.WarningBox("Line mapping versioncan't empty!!");
                textBox1.Focus();
                return;
            }

            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@styleid";
            sp1.Value = style;
            sp2.ParameterName = "@seasonid";
            sp2.Value = season;
            sp3.ParameterName = "@brandid";
            sp3.Value = brand;
            sp4.ParameterName = "@version";
            sp4.Value = version;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            string sqlCmd = "select * from LineMapping where StyleID = @styleid and SeasonID = @seasonid and BrandID = @brandid and Version = @version";
            DataTable Linemap;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out Linemap);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                return;
            }
            else
            {
                if (Linemap.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
                else
                {
                    P03CopyLineMapping = Linemap.Rows[0];
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
    }
}
