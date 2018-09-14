using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Sci.Data;

namespace Sci.Production.Class
{
    public class UserESignature
    {
        public static int LimitWidth = 550;
        public static int LimitHeight = 300;
        /// <summary>
        /// 取得電子簽章路徑
        /// </summary>
        public static string getESignaturePath()
        {
            // 取得當前Config moudle位置 Dummy or Formal
            string dbName = DBProxy.Current.DefaultModuleName;
            string PicPath = MyUtility.GetValue.Lookup(@"select PicPath from System WITH (NOLOCK)") + @"\" + dbName + @"\" + @"ESignature\";
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

            if (File.Exists(FilePath))
            {
                Image img = null;
                using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    img = new Bitmap(Image.FromStream(stream));
                }

                return img;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 系統在匯出 RDLC 時,自動放大圖片或縮小
        /// </summary>
        /// <param name="UserID"></param>
        public static byte[] getRDLCUserESignature(string UserID)
        {
            byte[] bytes;
            Image img =getUserESignature(UserID);
            if (img == null)
            {
                return null;
            }
            int fixWidth = 0, fixHeight = 0;
            int sourWidth = img.Width;
            int sourHeight = img.Height;
            // 原始圖檔任一邊大於縮放極限,就依比例調整長寬
            if (sourWidth > LimitWidth || sourHeight > LimitHeight)
            {
                if ((sourWidth * LimitHeight) > (sourHeight * LimitWidth))
                {
                    fixWidth = LimitWidth;
                    fixHeight = (LimitWidth * sourHeight) / sourWidth;
                }
                else
                {
                    fixWidth = (sourWidth * LimitHeight) / sourHeight;
                    fixHeight = LimitHeight;
                }
            }
            else
            {
                fixWidth = sourWidth;
                fixHeight = sourHeight;
            }
            Bitmap finalBitmap = new Bitmap(img, fixWidth, fixHeight);
          

            using (MemoryStream ms = new MemoryStream())
            {
                finalBitmap.Save(ms, ImageFormat.Png);
                bytes = new byte[(int)ms.Length];
                ms.Position = 0;
                ms.Read(bytes, 0, (int)ms.Length);
                ms.Flush();
            }
            return bytes;
        }

        /// <summary>
        /// 依照UserID 取得對應的ESignature圖檔路徑位置
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string getByteImagePath(string UserID)
        {
            string path = getESignaturePath() + MyUtility.GetValue.Lookup($@"select ESignature from Pass1 where id='{UserID}'");

            return path;
        }
    }
}
