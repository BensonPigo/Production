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
        #region GetItemDesc
        /// <summary>
        /// GetItemDesc()
        /// </summary>
        /// <param name="String Category"></param>
        /// <param name="String Refno"></param>
        /// <returns>String Desc</returns>
        public static string GetItemDesc(string category, string refno)
        {
            string desc = myUtility.Lookup(string.Format(@"
                    select description from localitem where refno = '{0}' and category = '{1}'",refno,category));
            string[] descs = desc.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (descs.Length == 0)
                return "";
            else
                return descs[0];
        }
        #endregion

        
    }
    
}
