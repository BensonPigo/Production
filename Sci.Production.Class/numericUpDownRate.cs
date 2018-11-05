using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Class
{
    public class numericUpDownRate : Sci.Win.UI.NumericUpDown
    {
        protected override void UpdateEditText()
        {
            this.Text = this.Value.ToString() + "%";
        }
    }
}
