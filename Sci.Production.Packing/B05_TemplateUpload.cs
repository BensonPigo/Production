using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class B05_TemplateUpload : Win.Subs.Input4
    {
        private DataTable sizesAll;
        private bool canEdit = false;
        private string ShippingMarkTypeUkey;

        /// <inheritdoc/>
        public B05_TemplateUpload(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.canEdit = canedit;
            this.ShippingMarkTypeUkey = keyvalue1;

            #region ComboBox
            DualResult result;
            string cmd = $@"
SELECT [StickerSizeID]='' ,[SIze]='' 
UNION
SELECT [StickerSizeID]=ID, SIze 
FROM StickerSize WITH (NOLOCK) 
";
            result = DBProxy.Current.Select(null, cmd, out this.sizesAll);
            if (!result)
            {
                this.ShowErr(result);
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorComboBoxColumnSettings col_StickerSize = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_TemplateName = new DataGridViewGeneratorTextColumnSettings();

            col_TemplateName.MaxLength = 30;

            // sideComboCell
            Dictionary<long, string> size = new Dictionary<long, string>();

            foreach (DataRow dr in this.sizesAll.Rows)
            {
                size.Add(MyUtility.Convert.GetLong(dr["StickerSizeID"]), MyUtility.Convert.GetString(dr["SIze"]));
            }

            col_StickerSize.DataSource = new BindingSource(size, null);
            col_StickerSize.ValueMember = "Key";
            col_StickerSize.DisplayMember = "Value";

            // 避免選了Junk
            col_StickerSize.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow currentRow = this.grid.GetDataRow(e.RowIndex);
                    int currentStickerSizeID = MyUtility.Convert.GetInt(e.FormattedValue);
                    DataRow dr;
                    string cmd = $@"
SELECT Size
FROM StickerSize WITH (NOLOCK) 
WHERE ID = '{currentStickerSizeID}'
AND Junk = 1
";
                    if (MyUtility.Check.Seek(cmd, out dr))
                    {
                        MyUtility.Msg.InfoBox($"Sticker Size {dr["Size"]} has junked !!");
                        currentRow["StickerSizeID"] = 0;
                        e.FormattedValue = 0;
                    }
                    else
                    {
                        currentRow["StickerSizeID"] = currentStickerSizeID;
                    }

                    currentRow.EndEdit();
                }
            };
            this.Helper.Controls.Grid.Generator(this.grid)
                .ComboBox("StickerSizeID", header: "Mark Size", width: Widths.AnsiChars(20), settings: col_StickerSize)
                .Text("TemplateName", header: "Template Name", width: Widths.AnsiChars(60),iseditingreadonly:true)
                .Button("Upload", null, header: "Upload", width: Widths.AnsiChars(5), onclick: this.BtnUpload)
            ;

            return true;
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();

            this.save.Enabled = this.canEdit && !this.EditMode;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            DataTable dt = (DataTable)((BindingSource)this.grid.DataSource).DataSource;
            if (dt.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted && MyUtility.Check.Empty(o["StickerSizeID"])).Any())
            {
                MyUtility.Msg.WarningBox($"Mark Size can't be empty.");
                return false;
            }

            return base.OnSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            return base.OnSavePost();
        }

        private void BtnUpload(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string targetPath = MyUtility.GetValue.Lookup("SELECT ShippingMarkTemplatePath FROM System");

            if (MyUtility.Check.Empty(targetPath))
            {
                MyUtility.Msg.WarningBox("Please set <Shipping Mark Template Path> first.");
                return;
            }

            if (!System.IO.Directory.Exists(targetPath))
            {
                MyUtility.Msg.WarningBox("Path not exists.");
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "All Files(*.tff;)|*.tff"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = file.SafeFileName;
                    string sourcePath = file.FileName;
                    string toPath = targetPath + file.SafeFileName;

                    // 複製範本至指定路徑
                    System.IO.File.Copy(sourcePath, toPath, true);

                    this.CurrentData["TemplateName"] = fileName;
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex);
                }

            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.revise.Visible = false;
        }
    }
}
