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

        private bool isShowDeleteHint = false;

        /// <summary>
        /// CameraDisplay
        /// </summary>
        /// <param name="isShowDeleteHint">isShowDeleteHint</param>
        public CameraDisplay(bool isShowDeleteHint = false)
        {
            this.InitializeComponent();
            this.LastItem = false;
            this.isShowDeleteHint = isShowDeleteHint;
        }

        /// <summary>
        /// SetPictureDisplay
        /// </summary>
        /// <param name="desc">desc</param>
        /// <param name="pkey">pkey</param>
        /// <param name="seq">seq</param>
        /// <param name="bitmap">bitmap</param>
        /// <param name="imgPath">imgPath</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="reSize">reSize</param>
        public void SetPictureDisplay(string desc, string pkey, int seq, Bitmap bitmap, string imgPath, double x, double y, bool reSize)
        {
            this.pictureBox1.Image = Camera_Prg.ResizeImage(Convert.ToInt32(this.pictureBox1.Width * x), Convert.ToInt32((this.pictureBox1.Height + 60) * y), bitmap, reSize);
            this.editDesc.Text = desc;
            this.Pkey = pkey;
            this.Seq = seq;
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

            if (this.isShowDeleteHint)
            {
                DialogResult dialogResult = MyUtility.Msg.QuestionBox("Are you sure you want to delete this picture?");
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            ((Panel)this.Parent).Controls.Remove(this);

            Camera_Prg.MasterSchemas.RemoveAll(a => a.ID == this.Pkey && a.Seq == this.Seq);
        }

        private void EditDesc_Validated(object sender, EventArgs e)
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
