using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <summary>
    /// Model for the IE_R22 report
    /// </summary>
    public class IE_R22_ViewModel
    {
        /// <summary>
        /// Indicates if the model is for PowerBI
        /// </summary>
        public bool IsPowerBI { get; set; }

        /// <summary>
        /// Start date of the deadline
        /// </summary>
        public DateTime? DeadlineStart { get; set; }

        /// <summary>
        /// End date of the deadline
        /// </summary>
        public DateTime? DeadlineEnd { get; set; }

        /// <summary>
        /// Start date of the inline process
        /// </summary>
        public DateTime? InlineStart { get; set; }

        /// <summary>
        /// End date of the inline process
        /// </summary>
        public DateTime? InlineEnd { get; set; }

        /// <summary>
        /// Order ID 
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// Style ID
        /// </summary>
        public string StyleID { get; set; }

        /// <summary>
        /// Type of the product
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Category of the product
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// SewingLine cell
        /// </summary>
        public string SewingCell { get; set; }

        /// <summary>
        /// Responsible department
        /// </summary>
        public string ResponseDep { get; set; }

        /// <summary>
        /// Factory ID
        /// </summary>
        public string FactoryID { get; set; }

        /// <summary>
        /// Is Outstanding
        /// </summary>
        public bool IsOutstanding { get; set; }
    }
}
