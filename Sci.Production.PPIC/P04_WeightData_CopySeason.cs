using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P04_WeightData_CopySeason : Sci.Win.Subs.Base
    {
        public string PPICP04CopySeason;
        private string styleUkey;
        public P04_WeightData_CopySeason(string StyleUkey)
        {
            InitializeComponent();
            styleUkey = StyleUkey;
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = string.Format(@"select s.SeasonID from Style s
where exists (select 1 from Style ss where ss.Ukey = {0} and ss.ID = s.ID and ss.BrandID = s.BrandID and ss.SeasonID <> s.SeasonID)
and exists (select 1 from Style_WeightData sw where sw.StyleUkey = s.Ukey)", styleUkey);
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.OldValue != textBox1.Text)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleukey", styleUkey);
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@seasonid", textBox1.Text);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                DataTable StyleData;
                string sqlCmd = @"select s.SeasonID from Style s
where exists (select 1 from Style ss where ss.Ukey = @styleukey and ss.ID = s.ID and ss.BrandID = s.BrandID)
and exists (select 1 from Style_WeightData sw where sw.StyleUkey = s.Ukey)
and s.SeasonID = @seasonid";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out StyleData);

                if (!result || StyleData.Rows.Count <= 0)
                {
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("The season has no weight data!");
                    }
                    textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //OK
        private void button1_Click(object sender, EventArgs e)
        {
            PPICP04CopySeason = textBox1.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
