using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class PictureSubPage : Sci.Win.Subs.Base
    {
        public PictureSubPage(Image img)
        {
            InitializeComponent();
            this.pbPicture.Image = img;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
