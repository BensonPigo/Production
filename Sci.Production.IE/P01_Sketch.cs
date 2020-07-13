using System.Collections.Generic;
using System.Data;
using Ict;
using Sci.Data;
using System.IO;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_Sketch
    /// </summary>
    public partial class P01_Sketch : Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P01_Sketch
        /// </summary>
        /// <param name="masterData">MasterData</param>
        public P01_Sketch(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();

            sp1.ParameterName = "@styleid";
            sp1.Value = this.masterData["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = this.masterData["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = this.masterData["BrandID"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);

            string sqlCmd = @"select s.Picture1,s.Picture2,s1.StyleSketch
from Style s WITH (NOLOCK) 
left join System s1 WITH (NOLOCK) on 1=1
where s.ID = @styleid and s.SeasonID = @seasonid and s.BrandID = @brandid";
            DataTable styleData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Style fail!\r\n" + result.ToString());
                return;
            }

            if (styleData.Rows.Count > 0)
            {
                this.displayPicture1.Value = styleData.Rows[0]["Picture1"].ToString().Trim();
                this.displayPicture2.Value = styleData.Rows[0]["Picture2"].ToString().Trim();

                /*判斷路徑下圖片檔找不到,就將ImageLocation帶空值*/
                if (MyUtility.Check.Empty(styleData.Rows[0]["Picture1"].ToString().Trim()))
                {
                    this.picture1.ImageLocation = string.Empty;
                }
                else
                {
                    if (File.Exists(styleData.Rows[0]["StyleSketch"].ToString().Trim() + styleData.Rows[0]["Picture1"].ToString().Trim()))
                    {
                        this.picture1.ImageLocation = styleData.Rows[0]["StyleSketch"].ToString().Trim() + styleData.Rows[0]["Picture1"].ToString().Trim();
                    }
                    else
                    {
                        this.picture1.ImageLocation = string.Empty;
                    }
                }

                if (MyUtility.Check.Empty(styleData.Rows[0]["Picture2"].ToString().Trim()))
                {
                    this.picture2.ImageLocation = string.Empty;
                }
                else
                {
                    if (File.Exists(styleData.Rows[0]["StyleSketch"].ToString().Trim() + styleData.Rows[0]["Picture2"].ToString().Trim()))
                    {
                        this.picture2.ImageLocation = styleData.Rows[0]["StyleSketch"].ToString().Trim() + styleData.Rows[0]["Picture2"].ToString().Trim();
                    }
                    else
                    {
                        this.picture2.ImageLocation = string.Empty;
                    }
                }
            }

            // Image images = Image.FromFile(@"D:\images.jpg");
            // pictureBox1.BackgroundImage = images;
        }
    }
}
