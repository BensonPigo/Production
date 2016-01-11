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
            string excelColEng;
            if (column <= 26)
            {
                excelColEng = MyUtility.Convert.GetString(Convert.ToChar(column + 64));
            }
            else
            {
                if ((column + 64) % 26 == 0)
                {
                    excelColEng = MyUtility.Convert.GetString(Convert.ToChar((int)((column - 1) / 26) + 64)) + 'Z';
                }
                else
                {
                    excelColEng = MyUtility.Convert.GetString(Convert.ToChar((int)((column) / 26) + 64)) + MyUtility.Convert.GetString(Convert.ToChar(column - ((int)(column / 26) * 26) + 64));
                }
            }
            return excelColEng;
        }
        #endregion;
    }
}