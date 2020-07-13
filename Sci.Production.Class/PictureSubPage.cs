using System;
using System.Drawing;

namespace Sci.Production.Class
{
    /// <summary>
    /// PictureSubPage
    /// </summary>
    public partial class PictureSubPage : Win.Subs.Base
    {
        /// <summary>
        /// PictureSubPage
        /// </summary>
        /// <param name="img">Image</param>
        public PictureSubPage(Image img)
        {
            this.InitializeComponent();
            this.pbPicture.Image = img;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
