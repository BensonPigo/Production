using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.IE
{
    /// <summary>
    /// Copy From GSD_CIPF
    /// </summary>
    public static class P03CIPFinfo
    {
        /// <summary>
        /// Cutting
        /// </summary>
        public static bool Cutting { get; set; } = false;

        /// <summary>
        /// Inspection
        /// </summary>
        public static bool Inspection { get; set; } = false;

        /// <summary>
        /// Pressing
        /// </summary>
        public static bool Pressing { get; set; } = false;

        /// <summary>
        /// Packing
        /// </summary>
        public static bool Packing { get; set; } = false;
    }
}
