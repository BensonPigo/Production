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
            DateTime? lastTime = (DateTime?)this.CurrentData["MRLastDate"];
            string MRLastUpdate = lastTime == null ? "" : ((DateTime)lastTime).ToString("yyyy/MM/dd HH:mm:ss");
            this.display_MRLastUpdate.Text = MRLastUpdate;

            DateTime? lastTime2 = (DateTime?)this.CurrentData["FtyLastDate"];
            string FtyLastupdate = lastTime2 == null ? "" : ((DateTime)lastTime2).ToString("yyyy/MM/dd HH:mm:ss");
            this.display_FtyLastDate.Text = FtyLastupdate;
        } 
    }
}
