using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class MsgGrid : MsgGridPrg
    {
        public MsgGrid(DataTable dt, string msg) : base(dt, msg)
        {
        }
    }
}
