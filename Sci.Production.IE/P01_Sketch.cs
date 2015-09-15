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

            string sqlCmd = string.Format(@"select s.Picture1,s.Picture2,s1.PicPath
from Style s
left join System s1 on 1=1
where s.ID = '{0}' and s.SeasonID = '{1}' and s.BrandID = '{2}'", masterData["StyleID"].ToString(), masterData["SeasonID"].ToString(), masterData["BrandID"].ToString());
            DataTable styleData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out styleData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Style fail!\r\n"+result.ToString());
                return;
            }
            displayBox1.Value = styleData.Rows[0]["Picture1"].ToString().Trim();
            displayBox2.Value = styleData.Rows[0]["Picture2"].ToString().Trim();
            pictureBox1.ImageLocation = styleData.Rows[0]["PicPath"].ToString().Trim() + styleData.Rows[0]["Picture1"].ToString().Trim();
            pictureBox2.ImageLocation = styleData.Rows[0]["PicPath"].ToString().Trim() + styleData.Rows[0]["Picture2"].ToString().Trim();
        }
    }
}
