using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Sci.Data;

namespace Sci.Production.Class
{
    public class UserESignature
    {
        /// <summary>
        /// 取得電子簽章路徑
        /// </summary>
        public static string getESignaturePath()
        {
            // 取得當前Config moudle位置 Dummy or Formal
            string dbName = DBProxy.Current.DefaultModuleName;
            string picPath = MyUtility.GetValue.Lookup(@"select PicPath from System WITH (NOLOCK)") + @"\" + dbName + @"\" + @"ESignature\";
            if (Directory.Exists(picPath) == false)
            {
                try
                {
                    Directory.CreateDirectory(picPath);
                }
                catch (Exception)
                {
                    // MyUtility.Msg.ErrorBox(e.ToString());
                    return null;
                }
            }

            return picPath;
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
        /// 將圖片依照RDLC 元件容器大小,依長寬比例調整Size
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="UserID"></param>
        /// <returns>byte[]</returns>
        public static byte[] getUserESignature(string UserID, int Width, int Height)
        {
            byte[] bytes;
            Image img = getUserESignature(UserID);
            if (img == null)
            {
                return null;
            }

            img = resizeImage(Width, Height, img);
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                bytes = new byte[(int)ms.Length];
                ms.Position = 0;
                ms.Read(bytes, 0, (int)ms.Length);
                ms.Flush();
            }

            return bytes;
        }

        /// <summary>
        /// 將圖片依照RDLC 元件容器大小,依長寬比例調整Size
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="UserID"></param>
        /// <returns>Image</returns>
        public static Image getUserESignature(int Width, int Height, string UserID)
        {
            Image img = getUserESignature(UserID);
            if (img == null)
            {
                return null;
            }

            img = resizeImage(Width, Height, img);

            return img;
        }

        /// <summary>
        /// 將圖片依照RDLC 元件容器大小,依長寬比例調整Size
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="SignatureImage"></param>
        /// <returns>Image</returns>
        private static Image resizeImage(int Width, int Height, Image SignatureImage)
        {
            Image img = SignatureImage;
            if (img == null)
            {
                return null;
            }

            int limitWidth = Width;
            int limitHeight = Height;

            // 建立簽名檔圖片, 圖檔長寬依照比例調整
            int fixWidth = 0, fixHeight = 0;
            int sourWidth = img.Width;
            int sourHeight = img.Height;
            if ((sourWidth * limitHeight) > (sourHeight * limitWidth))
            {
                fixWidth = limitWidth;
                fixHeight = (limitWidth * sourHeight) / sourWidth;
            }
            else
            {
                fixWidth = (sourWidth * limitHeight) / sourHeight;
                fixHeight = limitHeight;
            }

            Bitmap orgBitmap = new Bitmap(img, fixWidth, fixHeight);
            img = orgBitmap as Image;

            #region 合併圖片

            // 建立底圖,長寬同RDLC image元件 size
            int blankWidth = (sourWidth > fixWidth) ? (sourWidth - fixWidth) / 2 : 0;
            int blankHeight = (sourHeight > fixHeight) ? (sourHeight - fixHeight) / 2 : 0;
            Bitmap blankBitmap = new Bitmap(limitWidth, limitHeight, PixelFormat.Format32bppPArgb);
            Image imgblank = blankBitmap;

            // 合併圖片
            Graphics gr = Graphics.FromImage(blankBitmap);
            float x = 0;
            float y = 0;

            // 如果修正後寬= 實際RDLC最大寬度,就調整Y軸放置位置,高度反之亦然
            if (fixWidth == limitWidth)
            {
                y = MyUtility.Convert.GetFloat(Math.Floor(MyUtility.Convert.GetDouble((limitHeight - fixHeight) / 2)));
            }
            else
            {
                x = MyUtility.Convert.GetFloat(Math.Floor(MyUtility.Convert.GetDouble((limitWidth - fixWidth) / 2)));
            }

            gr.DrawImage(orgBitmap, new PointF(x, y));
            img = blankBitmap;

            #endregion

            return img;
        }
    }
}
