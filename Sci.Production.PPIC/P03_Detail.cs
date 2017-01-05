using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P03_Detail : Sci.Win.Subs.Input6A
    {
        public P03_Detail()
        {
            InitializeComponent();

            txtuser2.TextBox1.ReadOnly = true;
            txtuser2.TextBox1.IsSupportEditMode = false;
        }

        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);

            if (!MyUtility.Check.Empty(CurrentData["MRLastDate"]))
                this.display_MRLastUpdate.Text = Convert.ToDateTime(CurrentData["MRLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            else
                this.display_MRLastUpdate.Text = "";

            if (!MyUtility.Check.Empty(CurrentData["FtyLastDate"]))
                this.display_FtyLastDate.Text = Convert.ToDateTime(CurrentData["FtyLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            else
                this.display_FtyLastDate.Text = "";

        } 
    }
}
