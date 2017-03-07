using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P03_ExcelImport : Sci.Win.Subs.Base
    {
        DataTable detailData;
        public P03_ExcelImport(DataTable DetailData)
        {
            InitializeComponent();
            detailData = DetailData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }
    }
}
