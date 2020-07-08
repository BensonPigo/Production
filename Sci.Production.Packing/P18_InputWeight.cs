using System;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P18_InputWeight
    /// </summary>
    public partial class P18_InputWeight : Sci.Win.Forms.Base
    {
        /// <summary>
        /// actWeight
        /// </summary>
        public decimal? ActWeight { get; set; }

        /// <summary>
        /// P18_InputWeight
        /// </summary>
        public P18_InputWeight()
        {
            this.InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.ActWeight = 0;
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            this.ActWeight = this.numWeight.Value.HasValue ? this.numWeight.Value : 0;
            this.DialogResult = DialogResult.OK;
        }
    }
}
