using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P24_Operations_Breakdown : Sci.Win.Forms.Base
    {
        private string style;
        private string season;
        private string brand;

        public P24_Operations_Breakdown(string styleID, string seasonID, string brandID)
        {
            InitializeComponent();
            this.style = styleID;
            this.season = seasonID;
            this.brand = brandID;
        }

        private void btn_StandardGSDList_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"select Ukey from Style where BrandID = '{this.brand}' and ID = '{this.style}' and SeasonID = '{this.season}'";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drStyle))
            {
                PublicForm.StdGSDList callNextForm = new PublicForm.StdGSDList(MyUtility.Convert.GetLong(drStyle["UKey"]));
                callNextForm.ShowDialog(this);
            }
        }

        private void btn_FactoryGSDList_Click(object sender, EventArgs e)
        {
            IE.P01 callNextForm = new IE.P01(this.style, this.brand, this.season, string.Empty, isReadOnly: true);
            callNextForm.ShowDialog(this);
        }
    }
}
