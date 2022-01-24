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
        private List<Endline_Camera_Schema> schemas;
        private string strType;

        private double oldWidth;
        private double oldHeight;

        public Camera_ShowNew(string formName, List<Endline_Camera_Schema> src, string Type)
        {
            this.InitializeComponent();
            this.oldWidth = this.Width;
            this.oldHeight = this.Height;
            this.strType = Type;

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

            double x = (this.Width / this.oldWidth);
            double y = (this.Height / this.oldHeight);
            this.panel1.Controls.Clear();

            foreach (var item in this.schemas)
            {
                switch (this.strType)
                {
                    case "ShowOnly":
                        CameraDisplay_ShowOnly picItemShow = new CameraDisplay_ShowOnly();
                        picItemShow.SetPictureDisplay(item.desc, item.Pkey, item.image, item.imgPath, x, y, resize);
                        picItemShow.Dock = DockStyle.Top;
                        this.panel1.Controls.Add(picItemShow);
                        break;
                    default:
                        CameraDisplay picItem = new CameraDisplay();
                        picItem.SetPictureDisplay(item.desc, item.Pkey, item.image, item.imgPath, x, y, resize);
                        picItem.Dock = DockStyle.Top;
                        this.panel1.Controls.Add(picItem);
                        break;
                }

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
