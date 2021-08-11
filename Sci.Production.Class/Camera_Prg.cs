using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sci.Data;
using System.IO;
using System.Drawing;
using Ict;
using System.Drawing.Imaging;
using Sci.Win.Tools;
using System.Threading;
using System.Data;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public class Camera_Prg
    {
        /// <summary>
        /// 取得相機照片路徑
        /// </summary>
        /// <returns>路徑</returns>
        public static string GetCameraPath(string OldyyyyMM)
        {
            // 取得當前Config moudle位置 Dummy or Formal
            string dbName = DBProxy.Current.DefaultModuleName;
            string picPath = MyUtility.GetValue.Lookup(@"select ClipPath from Production..System WITH (NOLOCK)");
            string yyyyMM = MyUtility.GetValue.Lookup("select Now = FORMAT(GETDATE(),'yyyyMM')");
            if (!MyUtility.Check.Empty(OldyyyyMM))
            {
                yyyyMM = OldyyyyMM;
            }

            picPath = Path.Combine(picPath, yyyyMM);

            if (Directory.Exists(picPath) == false)
            {
                try
                {
                    Directory.CreateDirectory(picPath);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return picPath;
        }

        /// <summary>
        /// 將圖片依照元件容器大小,依長寬比例調整Size
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="img">camera Image</param>
        /// <param name="resize">resize</param>
        /// <returns>Image</returns>
        public static Image ResizeImage(int width, int height, Image img, bool resize)
        {
            if (img == null)
            {
                return null;
            }

            int limitWidth = width;
            int limitHeight = height;

            // 圖檔長寬依照比例調整
            int fixWidth;
            int fixHeight;
            int sourWidth = img.Width;
            int sourHeight = img.Height;
            if (resize == false)
            {
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
            }
            else
            {
                fixWidth = width;
                fixHeight = height;
            }

            Bitmap orgBitmap = new Bitmap(img, fixWidth, fixHeight);
            img = orgBitmap as Image;

            #region 合併圖片

            // 建立底圖,長寬同image元件 size
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

        /// <summary>
        /// 將照片存進Clip
        /// </summary>
        /// <param name="key">組成照片的key word ex: {SP#}_{Article}_Endline_{Inspection_Detail.UEKY}_XXX.png</param>
        /// <param name="CameraID">暫存檔案的key ID</param>
        /// <param name="TableName">Table Name</param>
        /// <param name="UniqueID">UniqueID</param>
        /// <returns>DualResult</returns>
        public static DualResult AddClip(string key, string CameraID, string TableName, string UniqueID)
        {
            DualResult result = new DualResult(true);
            List<Endline_Camera_Schema> sc = Camera_Prg.MasterSchemas.Where(t => t.ID == CameraID).OrderBy(o => o.Seq).ToList();

            int ix = 0;
            string[] pkeys = GetPKeys(sc.Count);
            string sqlcmd = string.Empty;
            string seq = "00000";

            string sqlSeq = $@"
select [MaxSeq] = SUBSTRING(split.Data,0 ,LEN(split.Data) - 3) -- 去除.png
from (
	select [MaxSourceFile] = max( c.SourceFile) 
	from ManufacturingExecution.dbo.Clip c
	where TableName = '{TableName}'
	and UniqueKey = '{UniqueID}'
) a
outer apply(
	select data 
	from Production.dbo.SplitString(a.MaxSourceFile,'_')
	where no='3'
)split
where (a.MaxSourceFile != '' or a.MaxSourceFile is not null)
";
            if (MyUtility.Check.Seek(sqlSeq, out DataRow drSeq, "ManufacturingExecution"))
            {
                seq = drSeq["MaxSeq"].ToString();
            }

            foreach (var item in sc)
            {
                // 先將照片存到指定位置
                Bitmap image = item.image;
                seq = GetNextValue(seq, 1);
                string path = Path.Combine(Camera_Prg.GetCameraPath(string.Empty), key + "_" + seq + ".png");
                image.Save(path, ImageFormat.Png);

                sqlcmd += $@" 
insert into Clip(PKey,TableName,UniqueKey,SourceFile,Description,AddName,AddDate)
values('{pkeys[ix]}','{TableName}','{UniqueID}','{key + "_" + seq + ".png"}','{item.desc.ToString()}','{Sci.Env.User.UserID}',GETDATE()) 
";

                ix++;
            }

            if (!MyUtility.Check.Empty(sqlcmd))
            {
                if (!(result = DBProxy.Current.Execute(string.Empty, sqlcmd)))
                {
                    return result;
                }
            }

            return result;
        }

        static string CHARs = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Get PK string without prefix.
        /// </summary>
        /// <returns>string without prefix.</returns>
        private static string GetPKeyPre()
        {
            var dtm_sys = DateTime.Now;

            string pkey = string.Empty;
            pkey += CHARs[dtm_sys.Year % 100].ToString() + CHARs[dtm_sys.Month].ToString() + CHARs[dtm_sys.Day].ToString();
            pkey += CHARs[dtm_sys.Hour].ToString();
            pkey += CHARs[dtm_sys.Minute / CHARs.Length].ToString() + CHARs[dtm_sys.Minute % CHARs.Length].ToString();
            pkey += CHARs[dtm_sys.Second / CHARs.Length].ToString() + CHARs[dtm_sys.Second % CHARs.Length].ToString();
            pkey += CHARs[dtm_sys.Millisecond / CHARs.Length].ToString() + CHARs[dtm_sys.Millisecond % CHARs.Length].ToString();

            return pkey;
        }

        /// <summary>
        /// 取得多筆Clip.Pkey
        /// </summary>
        /// <param name="count">count</param>
        /// <returns>string[]</returns>
        internal static string[] GetPKeys(int count)
        {
            string[] pkeys = new string[count];
            var pkey = GetPKeyPre();

            for (int i = 0; i < count; ++i)
            {
                pkeys[i] = pkey + i.ToString("00");
            }

            return pkeys;
        }

        /// <summary>
        /// 取得下一筆流水號
        /// </summary>
        /// <param name="strValue">上一筆流水號</param>
        /// <param name="sequenceMode">1</param>
        /// <returns>string</returns>
        public static string GetNextValue(string strValue, int sequenceMode)
        {
            char[] charValue = strValue.ToArray();
            int sequenceValue = 0;
            string returnValue = string.Empty;
            int charAscii = 0;

            if (sequenceMode == 1)
            {
                // 當第一個字為字母
                if (Convert.ToInt32(charValue[0]) >= 65 && Convert.ToInt32(charValue[0]) <= 90)
                {
                    sequenceValue = Convert.ToInt32(strValue.Substring(1));

                    // 進位處理
                    if (((sequenceValue + 1).ToString().Length > sequenceValue.ToString().Length) && string.IsNullOrWhiteSpace(strValue.Substring(1).Replace("9", string.Empty)))
                    {
                        charAscii = Convert.ToInt32(charValue[0]);
                        if (charAscii + 1 > 90)
                        {
                            return strValue;
                        }
                        else
                        {
                            sequenceValue = 1;
                            if (charAscii == 72 || charAscii == 78)
                            {
                                // I or O略過
                                charValue[0] = Convert.ToChar(charAscii + 2);
                            }
                            else
                            {
                                charValue[0] = Convert.ToChar(charAscii + 1);
                            }
                        }
                    }
                    else
                    {
                        sequenceValue = sequenceValue + 1;
                    }

                    returnValue = charValue[0] + sequenceValue.ToString().PadLeft(strValue.Length - 1, '0');
                }
                else
                {
                    sequenceValue = Convert.ToInt32(strValue);

                    // 進位處理
                    if (((sequenceValue + 1).ToString().Length > sequenceValue.ToString().Length) && string.IsNullOrWhiteSpace(strValue.Replace("9", string.Empty)))
                    {
                        sequenceValue = 1;
                        charValue[0] = 'A';
                        returnValue = charValue[0] + sequenceValue.ToString().PadLeft(strValue.Length - 1, '0');
                    }
                    else
                    {
                        sequenceValue = sequenceValue + 1;
                        returnValue = sequenceValue.ToString().PadLeft(strValue.Length, '0');
                    }
                }
            }
            else
            {
                for (int i = charValue.Length - 1; i >= 0; i--)
                {
                    charAscii = Convert.ToInt32(charValue[i]);

                    if (charAscii == 57)
                    { // 遇9跳A
                        charValue[i] = 'A';
                        break;
                    }

                    if (charAscii == 72 || charAscii == 78)
                    {
                        // I or O略過
                        charValue[i] = Convert.ToChar(charAscii + 2);
                        break;
                    }

                    if (charAscii == 90)
                    {
                        // 當字母為Z
                        if (i > 0)
                        {
                            charValue[i] = '0';
                            continue;
                        }
                        else
                        {
                            return strValue;    // 超出最大上限ZZZ...., 返回原值
                        }
                    }

                    charValue[i] = Convert.ToChar(charAscii + 1);
                    break;
                }

                returnValue = new string(charValue);
            }

            return returnValue;
        }

        /// <summary>
        /// 全域變數
        /// </summary>
        public static List<Endline_Camera_Schema> MasterSchemas = new List<Endline_Camera_Schema>();
    }

    /// <inheritdoc/>
    public class Endline_Camera_Schema
    {
        /// <inheritdoc/>
        public string ID { get; set; }

        /// <inheritdoc/>
        public int Seq { get; set; }

        /// <inheritdoc/>
        public Bitmap image { get; set; }

        /// <inheritdoc/>
        public string desc { get; set; }

        public string FabricdefectID { get; set; }

        public string Pkey { get; set; }

        public string imgPath { get; set; }
    }
}
