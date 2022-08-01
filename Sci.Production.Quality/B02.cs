using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B02 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = "JUNK = 0";
            this.ReloadDatas();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.LoadPicture();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = "JUNK = 0";
                        break;
                    case "1":
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                this.ReloadDatas();
            };
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.LoadPicture();
        }

        private void LoadPicture()
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string sqlcmd = $@"select * from SciPMSFile_Basic_AccessoryDefectImage where AccessoryDefectID = '{this.CurrentMaintain["ID"]}' order by ukey";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return; 
            }

            this.ListDefectImg.Clear();
            int virtualSeqnum = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string virtualSeq = virtualSeqnum.ToString().PadLeft(3, '0');
                this.ListDefectImg.Add(new DefectImg { VirtualSeq = virtualSeq, Img = (byte[])dr["Image"], UpdType = DefectImgUpdType.None, Ukey = MyUtility.Convert.GetLong(dr["Ukey"]) });
                virtualSeqnum++;
            }

            this.SetPicCombox();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
            this.editDescription.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtCode.Focus();
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Description"]))
            {
                this.editDescription.Focus();
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                return false;
            }

            this.SavePricture();
            #endregion
            return base.ClickSaveBefore();
        }

        private List<DefectImg> ListDefectImg = new List<DefectImg>();

        private void BtnUploadDefectPicture_Click(object sender, System.EventArgs e)
        {
            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog
            {
                InitialDirectory = "c:\\", // 預設路徑
                Filter = "Image Files(*.PNG;*.JPG)|*.PNG;*.JPG", // 使用檔名
                FilterIndex = 1,
                RestoreDirectory = true,
                Multiselect = true,
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                int virtualSeqnum = this.ListDefectImg.Count == 0 ? 1 : this.ListDefectImg.Max(s => MyUtility.Convert.GetInt(s.VirtualSeq)) + 1;
                foreach (string fileName in file.FileNames)
                {
                    string virtualSeq = virtualSeqnum.ToString().PadLeft(3, '0');
                    this.ListDefectImg.Add(new DefectImg { VirtualSeq = virtualSeq, Img = File.ReadAllBytes(fileName), UpdType = DefectImgUpdType.Insert });
                    virtualSeqnum++;
                }
            }

            this.SetPicCombox();
        }

        private void BtnRemove_Click(object sender, System.EventArgs e)
        {
            var targetDefectImage = this.ListDefectImg.Where(s => s.VirtualSeq == this.cmbDefectPicture.Text);

            if (targetDefectImage.Any())
            {
                foreach (var defectImg in targetDefectImage)
                {
                    defectImg.UpdType = DefectImgUpdType.Remove;
                }

                this.SetPicCombox();
            }
        }

        private void SetPicCombox()
        {
            MyUtility.Tool.SetupCombox(this.cmbDefectPicture, 1, 1, this.ListDefectImg.Where(s => s.UpdType != DefectImgUpdType.Remove).Select(s => s.VirtualSeq).JoinToString(","));
            this.cmbDefectPicture.SelectedIndex = -1;
            this.cmbDefectPicture.SelectedIndex = 0;
        }

        private void SavePricture()
        {
            foreach (var item in this.ListDefectImg.Where(w => w.Img != null && w.UpdType != DefectImgUpdType.None))
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                string sqlcmd = string.Empty;

                if (item.UpdType == DefectImgUpdType.Insert)
                {
                    paras = new List<SqlParameter> { new SqlParameter($"@Image", item.Img) };
                    sqlcmd = $@"
set XACT_ABORT on
INSERT INTO SciPMSFile_Basic_AccessoryDefectImage([AccessoryDefectID],[Image])VALUES('{this.CurrentMaintain["ID"]}',@Image)
";
                }
                else if (item.UpdType == DefectImgUpdType.Remove && item.Ukey > 0)
                {
                    paras = new List<SqlParameter> { new SqlParameter($"@Ukey", item.Ukey) };
                    sqlcmd = $@"
set XACT_ABORT on
delete SciPMSFile_Basic_AccessoryDefectImage where Ukey = @Ukey
";
                }
                else
                {
                    continue;
                }

                DualResult result = DBProxy.Current.Execute(null, sqlcmd, paras);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }
        }

        private class DefectImg
        {
            public string VirtualSeq { get; set; }

            public byte[] Img { get; set; }

            public DefectImgUpdType UpdType { get; set; }

            public long Ukey { get; set; } = -1;
        }

        private enum DefectImgUpdType
        {
            None,
            Insert,
            Remove,
        }

        private void CmbDefectPicture_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DefectImg defectoicture = this.ListDefectImg.Where(w => w.VirtualSeq == this.cmbDefectPicture.Text).FirstOrDefault();
            if (defectoicture != null && !MyUtility.Check.Empty(defectoicture.Img))
            {
                using (MemoryStream ms = new MemoryStream(defectoicture.Img))
                {
                    this.pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                this.pictureBox1.Image = null;
            }
        }
    }
}
