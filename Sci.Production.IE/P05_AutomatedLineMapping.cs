using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_AutomatedLineMapping
    /// </summary>
    public class P05_AutomatedLineMapping
    {
        private string styleID;
        private string brandID;
        private string seasonID;
        private string comboType;

        /// <summary>
        /// P05_AutomatedLineMapping
        /// </summary>
        /// <param name="styleID">styleID</param>
        /// <param name="brandID">brandID</param>
        /// <param name="seasonID">seasonID</param>
        /// <param name="comboType">comboType</param>
        public P05_AutomatedLineMapping(string styleID, string brandID, string seasonID, string comboType)
        {
            this.styleID = styleID;
            this.brandID = brandID;
            this.seasonID = seasonID;
            this.comboType = comboType;
        }
    }
}
