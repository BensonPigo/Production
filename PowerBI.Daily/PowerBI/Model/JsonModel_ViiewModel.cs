using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBI.Daily.PowerBI.Model
{
    /// <inheritdoc/>
    public class JsonModel<T>
    {
        /// <inheritdoc/>
        public List<T> ResultDt { get; set; }
    }
}
