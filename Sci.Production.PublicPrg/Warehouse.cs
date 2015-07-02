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
        #region GetMtlDesc
        /// <summary>
        /// GetMtlDesc()
        /// </summary>
        /// <param name="String Poid"></param>
        /// <param name="String Seq1"></param>
        /// <param name="String Seq2"></param>
        /// <param name="Int ReturnFormat"></param>
        /// <param name="Bool Repeat"></param>
        /// <returns>String Desc</returns>
        public static string GetMtlDesc(string Poid, string seq1, string seq2, int ReturnFormat, bool Repeat = false)
        {
            string rtn="";
            DataRow dr;
            MyUtility.Check.Seek(string.Format(@"select StockPOID,scirefno,SuppColor,colorid,ColorDetail,sizespec
                                            ,special,SizeUnit,remark from po_supp_detail where id ='{0}' and seq1 = '{1}' and seq2 = '{2}'"
                                            , Poid, seq1, seq2), out dr);
            switch (ReturnFormat)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    if (seq1.Substring(0,1) ==  "7")
                    {
                        rtn = "**PLS USE STOCK FROM SP#:" 
                            + dr["StockPOID"].ToString()
                            + "**" + Environment.NewLine;
                    }

                    if (!Repeat && !MyUtility.Check.Empty(dr["scirefno"]))
                    {
                        string DescDetail = MyUtility.GetValue.Lookup(string.Format(@"
                                        select DescDetail from fabric where SCIRefno = '{0}'", dr["scirefno"]));

                        string[] descs = DescDetail.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        if (descs.Length > 0)
                        {
                            rtn = rtn + descs[0];
                        }
                    }
                    break;

                default:
                    break;
            }

            if (Repeat || ReturnFormat == 2 || ReturnFormat==4)
            {
                rtn += dr["SuppColor"].ToString().TrimEnd();
                string colorid = dr["colorid"].ToString();
                string[] colors = colorid.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (colors.Length > 0)
                {
                    for (int i = 0; i < colors.Length; i++)
                    {
                        rtn = rtn + colors[i] + "-" + MyUtility.GetValue.Lookup(string.Format(@"select name from color,orders 
                                                                                                    where color.brandid = orders.brandid 
                                                                                                            and orders.brandid= '{0}' 
                                                                                                            and color.id = '{1}'", Poid, colors[i].ToString()));
                    }
                }
                if (!MyUtility.Check.Empty(dr["ColorDetail"].ToString())) { rtn += dr["ColorDetail"].ToString() + Environment.NewLine; }
                if (!MyUtility.Check.Empty(dr["sizespec"].ToString())) { rtn += dr["sizespec"].ToString() + Environment.NewLine; }
                if (!MyUtility.Check.Empty(dr["SizeUnit"].ToString())) { rtn += dr["SizeUnit"].ToString() + Environment.NewLine; }
                if (!MyUtility.Check.Empty(dr["Special"].ToString())) { rtn += dr["Special"].ToString() + Environment.NewLine; }
                if (!MyUtility.Check.Empty(dr["Remark"].ToString())) { rtn += dr["Remark"].ToString() + Environment.NewLine; }

            }

            
            //mReturn = mReturn + IIF(EMPTY(ALLTRIM(PO3.Old_Desc)), '', 'Color Detail:' + ALLTRIM(PO3.Old_Desc) + CHR(13))
            //mReturn = mReturn + IIF(EMPTY(ALLTRIM(PO3.SizeSpec)), '', IIF(AT(PO3.ColorID, ',') = 0, ',' + ALLTRIM(PO3.SizeSpec), CHR(10) + ALLTRIM(PO3.SizeSpec)))
            //mReturn = mReturn + IIF(EMPTY(ALLTRIM(PO3.SizeUnit)), '', ' ' + ALLTRIM(PO3.SizeUnit) + CHR(13))
            //mReturn = mReturn + IIF(EMPTY(ALLTRIM(PO3.Special)), '', ALLTRIM(PO3.Special) + CHR(13))
            //mReturn = mReturn + IIF(EMPTY(ALLTRIM(PO3.Spec)), '', ALLTRIM(PO3.Spec) + CHR(13))
            //mReturn = mReturn + IIF(EMPTY(ALLTRIM(PO3.Remark)), '', ALLTRIM(PO3.Remark) + CHR(13))
            return rtn;
        }
        #endregion

        
    }
    
}
