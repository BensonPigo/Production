using Sci.ManufacturingExecution.Class.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class Camera_ShowNew : Sci.Win.Forms.Base
    {
        private string key;
        private List<Endline_Camera_Schema> schemas;

        private double oldWidth;
        private double oldHeight;

        public Camera_ShowNew(string formName, string strKey, List<Endline_Camera_Schema> src)
        {
            InitializeComponent();
            oldWidth = this.Width;
            oldHeight = this.Height;

            this.key = strKey;
            this.schemas = src;
            this.Text = formName;
            this.Reload(false);
        }

        private void Reload(bool resize)
        {
            if (this.schemas == null || this.schemas.Count == 0)
            {
                return;
            }

            List<Endline_Camera_Schema> tempShow = this.schemas.Where(t => t.ID == this.key).ToList();
            if (tempShow.Count == 0)
            {
                return;
            }

            double x = (this.Width / oldWidth);
            double y = (this.Height / oldHeight);
            this.panel1.Controls.Clear();

            foreach (var item in tempShow)
            {
                CameraDisplay picItem = new CameraDisplay();
                picItem.SetPictureDisplay(item.desc, item.Pkey, item.image, item.imgPath, x, y, resize);
                picItem.Dock = DockStyle.Top;
                this.panel1.Controls.Add(picItem);
                this.panel1.Refresh();
            }
        }

        // 此按鈕只單純用來close
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Camera_ShowNew_Resize(object sender, EventArgs e)
        {
            this.Reload(true);
        }
    }
}
