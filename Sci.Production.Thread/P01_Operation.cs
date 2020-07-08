using Ict.Win;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P01_Operation
    /// </summary>
    public partial class P01_Operation : Win.Subs.Input8A
    {
        /// <summary>
        /// P01_Operation
        /// </summary>
        public P01_Operation()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            #region set grid
            this.Helper.Controls.Grid.Generator(this.grid)
           .Text("Operationid", header: "Operationid", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("DescEN", header: "Thread Comb Desc", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Numeric("Seamlength", header: "Seam Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
           .Numeric("Frequency", header: "Frequency", width: Widths.AnsiChars(6), integer_places: 4, decimal_places: 2, iseditingreadonly: true);
            #endregion
            return true;
        }
    }
}
