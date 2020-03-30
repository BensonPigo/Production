using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;

namespace Sci.Production.PublicPrg
{

    public static partial class Prgs
    {
        #region BundleCardCheckSubprocess
        /// <summary>
        /// BundleCardCheckSubprocess(string[] ann, string patterncode,DataTable artTb, out bool lallpart)
        /// </summary>
        /// <param name="ann"></param>
        /// <param name="patterncode"></param>
        /// <param name="artTb"></param>
        /// <param name="lallpartas"></param>
        /// <returns>string</returns>
        public static string BundleCardCheckSubprocess(string[] ann, string patterncode, DataTable artTb, out bool lallpart)
        {
            //artTb 是給前Form 使用同Garment List 的PatternCode 與Subrpocess
            string art = "";
            lallpart = true; //是不是All part
            for (int i = 0; i < ann.Length; i++) //寫入判斷是否存在Subprocess
            {
                string[] ann2 = ann[i].ToString().Split(' '); //剖析Annotation
                if (ann2.Length > 0)
                {
                    #region 有分開字元需剖析
                    for (int j = 0; j < ann2.Length; j++)
                    {
                        if (MyUtility.Check.Seek(ann2[j], "subprocess", "Id"))
                        {
                            lallpart = false;
                            //Artwork 相同的也要顯示, ex: HT+HT
                            //if (art.IndexOf(ann2[j]) == -1)
                            //{
                            DataRow[] existdr = artTb.Select(string.Format("PatternCode ='{0}' and Subprocessid ='{1}'", patterncode, ann2[j]));
                            if (existdr.Length == 0)
                            {
                                DataRow ndr_art = artTb.NewRow();
                                ndr_art["PatternCode"] = patterncode;
                                ndr_art["SubProcessid"] = ann2[j];
                                artTb.Rows.Add(ndr_art);
                            }
                            if (art == "") art = ann2[j];
                            else art = art.Trim() + "+" + ann2[j];
                            //}
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 無分開字元
                    if (MyUtility.Check.Seek(ann[i], "subprocess", "Id"))
                    {
                        lallpart = false;
                        if (art.IndexOf(ann[i]) == -1)
                        {
                            DataRow[] existdr = artTb.Select(string.Format("PatternCode ='{0}' and Subprocessid ='{1}'", patterncode, ann[i]));
                            if (existdr.Length == 0) //表示無在ArtTable 內
                            {
                                DataRow ndr_art = artTb.NewRow();
                                ndr_art["PatternCode"] = patterncode;
                                ndr_art["SubProcessid"] = ann[i];
                                artTb.Rows.Add(ndr_art);
                            }
                            if (art == "") art = ann[i];
                            else art = art.Trim() + "+" + ann[i];
                        }
                    }
                    #endregion
                }
            }
            return art;
        }
        #endregion;

        #region 均分數量 EX:10均分4份→3,3,2,2
        public static void AverageNumeric(DataRow[] dr, string columnName = "Qty", int TotalNumeric = 0, bool deleteZero = false)
        {
            if (dr.Count() == 0) return;
            int rowCount = dr.Count();
            int eachqty = TotalNumeric / rowCount;
            int modqty = TotalNumeric % rowCount; //剩餘數

            if (modqty == 0)
            {
                foreach (DataRow dr2 in dr)
                {
                    dr2[columnName] = eachqty;
                }
            }
            else
            {
                foreach (DataRow dr2 in dr)
                {
                    if (eachqty != 0)
                    {
                        if (modqty > 0) dr2[columnName] = eachqty + 1;//每組分配一個Qty 當分配完表示沒了
                        else dr2[columnName] = eachqty;
                        modqty--; //剩餘數一定小於rowcount所以會有筆數沒有拿到
                    }
                    else
                    {
                        // 這處理資料筆數小於總數. EX:3筆資料,總數只有2
                        if (modqty > 0)
                        {
                            dr2[columnName] = 1;
                            modqty--;
                        }
                        else
                        {
                            if (deleteZero)
                            {
                                dr2.Delete();
                            }
                            else
                            {
                                dr2[columnName] = 0;
                            }

                        }
                    }
                }
            }
        }
        #endregion
    }
}