﻿using System;
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
    public partial class P01_Sketch : Sci.Win.Subs.Base
    {
        private DataRow masterData;
        public P01_Sketch(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            
            sp1.ParameterName = "@styleid";
            sp1.Value = masterData["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = masterData["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = masterData["BrandID"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);

            string sqlCmd = @"select s.Picture1,s.Picture2,s1.PicPath
from Style s WITH (NOLOCK) 
left join System s1 WITH (NOLOCK) on 1=1
where s.ID = @styleid and s.SeasonID = @seasonid and s.BrandID = @brandid";
            DataTable styleData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Style fail!\r\n"+result.ToString());
                return;
            }
            if (styleData.Rows.Count > 0)
            {
                displayPicture1.Value = styleData.Rows[0]["Picture1"].ToString().Trim();
                displayPicture2.Value = styleData.Rows[0]["Picture2"].ToString().Trim();

                /*判斷路徑下圖片檔找不到,就將ImageLocation帶空值*/
                if (MyUtility.Check.Empty(styleData.Rows[0]["Picture1"].ToString().Trim())) picture1.ImageLocation = "";
                else
                {
                    if (File.Exists(styleData.Rows[0]["PicPath"].ToString().Trim() + styleData.Rows[0]["Picture1"].ToString().Trim()))
                    {
                        picture1.ImageLocation = styleData.Rows[0]["PicPath"].ToString().Trim() + styleData.Rows[0]["Picture1"].ToString().Trim();
                    }
                    else
                    {
                        picture1.ImageLocation = "";
                    }
                }
                if (MyUtility.Check.Empty(styleData.Rows[0]["Picture2"].ToString().Trim())) picture2.ImageLocation = "";
                else
                {
                    if (File.Exists(styleData.Rows[0]["PicPath"].ToString().Trim() + styleData.Rows[0]["Picture2"].ToString().Trim()))
                    {
                        picture2.ImageLocation = styleData.Rows[0]["PicPath"].ToString().Trim() + styleData.Rows[0]["Picture2"].ToString().Trim();
                    }
                    else
                    {
                        picture2.ImageLocation = "";
                    }
                }             
            }
            //Image images = Image.FromFile(@"D:\images.jpg");
            //pictureBox1.BackgroundImage = images;
        }
    }
}
