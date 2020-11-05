using Sci.Production.Automation;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using static Sci.Production.Shipping.Utility_WebAPI;
using System.Data;
using System;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public class Customs_WebAPI
    {
        /// <inheritdoc/>
        public class ListContract
        {
            /// <inheritdoc/>
            public List<Contract> ContractDt { get; set; }
        }

        /// <inheritdoc/>
        public class Contract
        {
            public string No { get; set; }
        }

        /// <inheritdoc/>
        public class ListCustomsCopyLoad
        {
            /// <inheritdoc/>
            public List<CustomsCopyLoad> CustomsCopyLoadDt { get; set; }
        }

        /// <inheritdoc/>
        public class CustomsCopyLoad
        {
            /// <inheritdoc/>
            public string CustomSP { get; set; }

            /// <inheritdoc/>
            public string Version { get; set; }

            /// <inheritdoc/>
            public DateTime CDate { get; set; }

            /// <inheritdoc/>
            public string Category { get; set; }

            /// <inheritdoc/>
            public string SizeCode { get; set; }

            /// <inheritdoc/>
            public string Article { get; set; }

            /// <inheritdoc/>
            public string SizeGroup { get; set; }
        }

        /// <inheritdoc/>
        public class ListCustomsAllData
        {
            /// <inheritdoc/>
            public DataSet GetCustomsAllDs { get; set; }
        }
    }
}