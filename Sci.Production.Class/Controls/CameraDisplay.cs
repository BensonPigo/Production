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
    public partial class CameraDisplay : UserControl
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
        /// 識別該元件的Seq
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 相同ID的不同序號
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InspectionCameraDefect"/> class.
        /// </summary>
        public CameraDisplay()
        {
            this.InitializeComponent();
            this.LastItem = false;
        }

        public void SetPictureDisplay(string desc, string Pkey, int Seq, Bitmap bitmap, string imgPath, double x, double y, bool reSize)
        {
            this.pictureBox1.Image = Camera_Prg.ResizeImage(Convert.ToInt32(this.pictureBox1.Width * x), Convert.ToInt32((this.pictureBox1.Height + 60) * y), bitmap, reSize);
            this.editDesc.Text = desc;
            this.Pkey = Pkey;
            this.Seq = Seq;
            this.ImagePath = imgPath;

            // 隱藏split分隔線
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer2.SplitterWidth = 1;
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

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (Camera_Prg.MasterSchemas == null || Camera_Prg.MasterSchemas.Count == 0)
            {
                return;
            }

            ((Panel)this.Parent).Controls.Remove(this);

            Camera_Prg.MasterSchemas.RemoveAll(a => a.ID == this.Pkey && a.Seq == this.Seq);
        }

        private void editDesc_Validated(object sender, EventArgs e)
        {
            if (Camera_Prg.MasterSchemas == null || Camera_Prg.MasterSchemas.Count == 0)
            {
                return;
            }

            List<Endline_Camera_Schema> tempShow = Camera_Prg.MasterSchemas.Where(t => t.ID == this.Pkey && t.Seq == this.Seq).ToList();
            foreach (var item in tempShow)
            {
                item.desc = this.editDesc.Text;
            }

            string sqlcmdEdit = $@"
update SciPMSFile_FIR_Physical_Defect_RealtimeImage
set Description = '{this.editDesc.Text}'
where FIRPhysicalDefectRealtimeID = '{this.Pkey}' and Seq = '{this.Seq}'
";
            DBProxy.Current.Execute(string.Empty, sqlcmdEdit);
        }
    }
}
