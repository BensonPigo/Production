using Ict;
using Sci.Data;
using Sci.ManufacturingExecution.Class;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.ManufacturingExecution.Class.Controls
{
    /// <inheritdoc/>
    public partial class CameraDisplay_ShowOnly : UserControl
    {
        /// <summary>
        /// Bool Last Item
        /// </summary>
        public bool LastItem { get; set; }

        /// <summary>
        /// 識別該元件的Pkey
        /// </summary>
        public string Pkey { get; set; }

        /// <summary>
        /// 相同ID的不同序號
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InspectionCameraDefect"/> class.
        /// </summary>
        public CameraDisplay_ShowOnly()
        {
            this.InitializeComponent();
            this.LastItem = false;
        }

        public void SetPictureDisplay(string desc, string Pkey, Bitmap bitmap, string imgPath, double x, double y, bool reSize)
        {
            this.pictureBox1.Image = Camera_Prg.ResizeImage(Convert.ToInt32(this.pictureBox1.Width * x), Convert.ToInt32((this.pictureBox1.Height + 60) * y), bitmap, reSize);
            this.Pkey = Pkey;
            this.ImagePath = imgPath;
        }

        private void CameraDisplay_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = e.ClipRectangle;
            if (this.LastItem)
            {
                r.Height -= 1;
            }

            r.Width -= 1;
        }
    }
}
