using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_WeightData_CopySeason
    /// </summary>
    public partial class P04_WeightData_CopySeason : Sci.Win.Subs.Base
    {
        private string ppicP04CopySeason;
        private string styleUkey;

        /// <summary>
        /// PPICP04CopySeason
        /// </summary>
        public string PPICP04CopySeason
        {
            get
            {
                return this.ppicP04CopySeason;
            }

            set
            {
                this.ppicP04CopySeason = value;
            }
        }

        /// <summary>
        /// P04_WeightData_CopySeason
        /// </summary>
        /// <param name="styleUkey">string StyleUkey</param>
        public P04_WeightData_CopySeason(string styleUkey)
        {
            this.InitializeComponent();
            this.styleUkey = styleUkey;
        }

        private void TxtFromSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = string.Format(
                @"select s.SeasonID from Style s WITH (NOLOCK) 
where exists (select 1 from Style ss WITH (NOLOCK) where ss.Ukey = {0} and ss.ID = s.ID and ss.BrandID = s.BrandID and ss.SeasonID <> s.SeasonID)
and exists (select 1 from Style_WeightData sw WITH (NOLOCK) where sw.StyleUkey = s.Ukey)", this.styleUkey);
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtFromSeason.Text = item.GetSelectedString();
        }

        private void TxtFromSeason_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtFromSeason.OldValue != this.txtFromSeason.Text)
            {
                // sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleukey", this.styleUkey);
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@seasonid", this.txtFromSeason.Text);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                DataTable styleData;
                string sqlCmd = @"select s.SeasonID from Style s WITH (NOLOCK) 
where exists (select 1 from Style ss WITH (NOLOCK) where ss.Ukey = @styleukey and ss.ID = s.ID and ss.BrandID = s.BrandID)
and exists (select 1 from Style_WeightData sw WITH (NOLOCK) where sw.StyleUkey = s.Ukey)
and s.SeasonID = @seasonid";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleData);

                if (!result || styleData.Rows.Count <= 0)
                {
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("The season has no weight data!");
                    }

                    this.txtFromSeason.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
            }
        }

        // OK
        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.PPICP04CopySeason = this.txtFromSeason.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
