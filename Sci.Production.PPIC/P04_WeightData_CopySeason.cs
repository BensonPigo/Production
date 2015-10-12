using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
                if (!MyUtility.Check.Seek(string.Format(@"select s.SeasonID from Style s
where exists (select 1 from Style ss where ss.Ukey = {0} and ss.ID = s.ID and ss.BrandID = s.BrandID)
and exists (select 1 from Style_WeightData sw where sw.StyleUkey = s.Ukey)
and s.SeasonID = '{1}'", styleUkey, textBox1.Text)))
                {
                    MyUtility.Msg.WarningBox("The season has no weight data!");
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
