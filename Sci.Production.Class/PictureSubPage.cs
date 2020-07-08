using System;
using System.Drawing;

namespace Sci.Production.Class
{
    public partial class PictureSubPage : Sci.Win.Subs.Base
    {
        public PictureSubPage(Image img)
        {
            this.InitializeComponent();
            this.pbPicture.Image = img;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
