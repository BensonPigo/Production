using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class P03_CopyFromOtherStyle : Sci.Win.Subs.Base
    {
        public DataRow P03CopyLineMapping;
        public P03_CopyFromOtherStyle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string style, season, brand, version;
            style = txtstyle1.Text;
            season = txtseason1.Text;
            brand = txtbrand1.Text;
            version = textBox1.Text;

            if (MyUtility.Check.Empty(style))
            {
                MyUtility.Msg.WarningBox("Style# can't empty!!");
                txtstyle1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(season))
            {
                MyUtility.Msg.WarningBox("Season can't empty!!");
                txtseason1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(brand))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                txtbrand1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(version))
            {
                MyUtility.Msg.WarningBox("Line mapping versioncan't empty!!");
                textBox1.Focus();
                return;
            }

            if (!MyUtility.Check.Seek(string.Format("select * from LineMapping where StyleID = '{0}' and SeasonID = '{1}' and BrandID = '{2}' and Version = {3}", style, season, brand, version), out P03CopyLineMapping))
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
