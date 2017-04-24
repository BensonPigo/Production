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

            txtuserFtyModifier.TextBox1.ReadOnly = true;
            txtuserFtyModifier.TextBox1.IsSupportEditMode = false;
        }

        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);

            if (!MyUtility.Check.Empty(CurrentData["MRLastDate"]))
                this.displayMRLastUpdate.Text = Convert.ToDateTime(CurrentData["MRLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            else
                this.displayMRLastUpdate.Text = "";

            if (!MyUtility.Check.Empty(CurrentData["FtyLastDate"]))
                this.displayFtyLastDate.Text = Convert.ToDateTime(CurrentData["FtyLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            else
                this.displayFtyLastDate.Text = "";

        } 
    }
}
