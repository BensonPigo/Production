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
        #region GetExcelEnglishColumnName
        /// <summary>
        /// GetExcelEnglishColumnName(int)
        /// </summary>
        /// <param name="column"></param>
        /// <returns>string</returns>
        public static string GetExcelEnglishColumnName(int column)
        {
            string strReturn = "";

            int iQuotient = column / 26;//商數
            int iRemainder = column % 26;//餘數

            if (iRemainder == 0)
                iQuotient--;  // 剛好整除的時候，商數要減一

            if (iQuotient > 0)
                strReturn = Convert.ToChar(64 + iQuotient).ToString();//A 65 利用ASCII做轉換

            if (iRemainder == 0)
                strReturn += "Z";
            else
                strReturn += Convert.ToChar(64 + iRemainder).ToString();    //A 65 利用ASCII做轉換

            return strReturn;
        }
        #endregion;
    }
}