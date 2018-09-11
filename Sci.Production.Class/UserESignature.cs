using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Sci.Production.Class
{
    public class UserESignature
    {
        int LimitWidth = 100;
        int LimitHeight = 25;
        /// <summary>
        /// 取得電子簽章路徑
        /// </summary>
        public static string getESignaturePath()
        {
            string PicPath = MyUtility.GetValue.Lookup(@"select PicPath from System WITH (NOLOCK)") + "ESignature\\";
            if (!Directory.Exists(PicPath))
            {
                Directory.CreateDirectory(PicPath);
            }

            return PicPath;
        }

        /// <summary>
        /// 用於Pass1 取得原始圖檔
        /// </summary>
        /// <param name="UserID"></param>
        public static Image getUserESignature(string UserID)
        {
            string FilePath = getESignaturePath() + MyUtility.GetValue.Lookup($@"select ESignature from Pass1 where id='{UserID}'");
            Image img = Image.FromFile(FilePath);
            return img;
        }

        /// <summary>
        /// 系統在匯出 RDLC 時,自動放大圖片或縮小
        /// </summary>
        /// <param name="UserID"></param>
        public static Image getRDLCUserESignature(string UserID)
        {
            Image img =getUserESignature(UserID);
            int orgW = img.Width;
            int orgH = img.Height;
            //if (orgW > orgH)
            //{
            //    img.   
            //}
            //else
            //{

            //}
            return img;
        }
    }
}
