using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P24_Sketch : Sci.Win.Forms.Base
    {
        private string destination_path; // 放圖檔的路徑
        private string style;
        private string season;
        private string brand;
        private string picture1;
        private string picture2;

        public P24_Sketch(string StyleID, string Season, string Brand)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select StyleSketch from System WITH (NOLOCK) ", null);
            this.style = StyleID;
            this.season = Season;
            this.brand = Brand;
            this.comboPictureSize1.SetDataSource(this.pictureBox1);
            this.comboPictureSize2.SetDataSource(this.pictureBox2);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlcmd = $@"
select Picture1,Picture2 from Style
where ID = '{this.style}' and SeasonID = '{this.season}' and BrandID = '{this.brand}'";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drStyle))
            {
                this.picture1 = drStyle["Picture1"].ToString();
                this.picture2 = drStyle["Picture2"].ToString();
            }
            else
            {
                this.picture1 = string.Empty;
                this.picture2 = string.Empty;
            }

            /*判斷路徑下圖片檔找不到,就將ImageLocation帶空值*/
            if (MyUtility.Check.Empty(this.picture1))
            {
                this.pictureBox1.ImageLocation = string.Empty;
            }
            else
            {
                if (File.Exists(Path.Combine(this.destination_path, this.picture1)))
                {
                    try
                    {
                        this.pictureBox1.ImageLocation = Path.Combine(this.destination_path, this.picture1);
                    }
                    catch (Exception e)
                    {
                        MyUtility.Msg.WarningBox("Picture1 process error. Please check it !!" + Environment.NewLine + e.ToString());
                        this.pictureBox1.ImageLocation = string.Empty;
                    }
                }
                else
                {
                    this.pictureBox1.ImageLocation = string.Empty;
                }
            }

            if (MyUtility.Check.Empty(this.picture2))
            {
                this.pictureBox2.ImageLocation = string.Empty;
            }
            else
            {
                if (File.Exists(Path.Combine(this.destination_path, this.picture2)))
                {
                    try
                    {
                        this.pictureBox2.ImageLocation = Path.Combine(this.destination_path, this.picture2);
                    }
                    catch (Exception e)
                    {
                        MyUtility.Msg.WarningBox("Picture2 process error. Please check it !!" + Environment.NewLine + e.ToString());
                        this.pictureBox2.ImageLocation = string.Empty;
                    }
                }
                else
                {
                    this.pictureBox2.ImageLocation = string.Empty;
                }
            }
        }
    }
}
