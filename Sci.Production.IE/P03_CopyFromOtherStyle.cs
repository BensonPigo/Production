using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P03_CopyFromOtherStyle
    /// </summary>
    public partial class P03_CopyFromOtherStyle : Win.Subs.Base
    {
        private DataRow _P03CopyLineMapping;

        /// <summary>
        /// P03CopyLineMapping
        /// </summary>
        public DataRow P03CopyLineMapping
        {
            get
            {
                return this._P03CopyLineMapping;
            }

            set
            {
                this._P03CopyLineMapping = value;
            }
        }

        /// <summary>
        /// P03_CopyFromOtherStyle
        /// </summary>
        public P03_CopyFromOtherStyle()
        {
            this.InitializeComponent();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            string style, season, brand, version;
            style = this.txtstyle.Text;
            season = this.txtseason.Text;
            brand = this.txtbrand.Text;
            version = this.txtLineMappingVersion.Text;

            if (MyUtility.Check.Empty(style))
            {
                MyUtility.Msg.WarningBox("Style# can't empty!!");
                this.txtstyle.Focus();
                return;
            }

            if (MyUtility.Check.Empty(season))
            {
                MyUtility.Msg.WarningBox("Season can't empty!!");
                this.txtseason.Focus();
                return;
            }

            if (MyUtility.Check.Empty(brand))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                this.txtbrand.Focus();
                return;
            }

            if (MyUtility.Check.Empty(version))
            {
                MyUtility.Msg.WarningBox("Line mapping versioncan't empty!!");
                this.txtLineMappingVersion.Focus();
                return;
            }

            // sql參數
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
            string sqlCmd = "select * from LineMapping WITH (NOLOCK) where StyleID = @styleid and SeasonID = @seasonid and BrandID = @brandid and Version = @version";
            DataTable linemap;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out linemap);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                return;
            }
            else
            {
                if (linemap.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
                else
                {
                    this._P03CopyLineMapping = linemap.Rows[0];
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
    }
}
