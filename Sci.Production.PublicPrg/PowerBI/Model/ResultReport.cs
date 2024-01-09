using Ict;
using System.Data;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class ResultReport
    {
        /// <inheritdoc/>
        public DualResult Result { get; set; }

        /// <inheritdoc/>
        public DataTable Dt { get; set; }

        /// <inheritdoc/>
        public DataTable[] DtArr { get; set; }

        public int IntValue { get; set; }
    }
}
