using Ict;
using Microsoft.Office.Core;
using Sci.Data;
//using Sci.Production.Class;
//using Sci.Trade.Class.Commons;
using Sci.Utility.Excel;
using Sci.Utility.Report;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Planning
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        private DataTable dtPrint;
        private SaveXltReportCls sxc;
        private string program = string.Empty;
        private string artworkType = string.Empty;

        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.txtMuiltSeason.Text = string.Empty;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        protected override bool ValidateInput()
        {
            if (this.txtbrand.Text == string.Empty)
            {
                MyUtility.Msg.ErrorBox("Brand can't be blank");
                return false;
            }

            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            try
            {
                string where = string.Empty;

                if (!this.txtbrand.Text.Empty())
                {
                    where += $" and s.BrandID in ('{this.txtbrand.Text.Replace(",", "','")}')";
                }

                string seasons = string.Join("','", this.txtMuiltSeason.Text.Split(','));

                if (this.txtMuiltSeason.Text != string.Empty)
                {
                    where += $" and s.SeasonID in ('{seasons}')";
                }

                if (!this.txtStyle.Text.Empty())
                {
                    where += $" and s.ID = '{this.txtStyle.Text}'";
                }

                if (this.artworkType != string.Empty)
                {
                    where += $@" and sa.ArtworkTypeID In ({"'" + string.Join("','", this.editArtworkType.Text.Split(',')) + "'"})";
                }

                if (this.program != string.Empty)
                {
                    where += $@" and s.ProgramID In ({"'" + string.Join("','", this.editProgram.Text.Split(',')) + "'"})";
                }

                if (!this.txtCOO.Text.Empty())
                {
                    where += $" and s.COO in ({"'" + string.Join("','", this.txtCOO.Text.Split(',')) + "'"})";
                }

                if (this.UI_ckbExcludeDevOptionStyle.Checked)
                {
                    where += " and s.DevOption = 0";
                }

                if (this.UI_ckbExcludeLocalStyle.Checked)
                {
                    where += " and s.LocalStyle = 0";
                }

                var sqlcmd = $@"
Select [Brand]         = s.BrandID
    , [Season]         = s.SeasonID
    , [Style#]         = s.ID
    , [Program]        = s.ProgramID
    , [COO]            = s.COO
    , [Region]         = s.DevRegion
    , [Artwork]        = sa.ArtworkID
    , [Artwork Description] = sa.ArtworkName
    , [Artwork Type]   = sa.ArtworkTypeID
    , [Article]        = sa.Article
    , [Cut part]       = sa.PatternCode
    , [Apply#]         = sa.SMNoticeID
    , [Ver.]           = sa.PatternVersion
    , [Description]    = sa.PatternDesc
    , [Annotation]     = sa.PatternAnnotation
    , [Qty]            = sa.Qty
    , [TMS]            = sa.Tms
    , [Price]          = sa.Price
    , [Cost]           = sa.Cost
    , [Print Type]     = sa.PrintType
    , [Ink Type]       = sa.InkType
    , [Ink Type PPU]   = sa.InkTypePPU
    , [Colors]         = sa.Colors
    , [Length(cm)]     = sa.Length
    , [Width(cm)]      = sa.Width
    , [Anti-Migration] = iif(sa.AntiMigration = 1, 'Y', '')
    , [Cut Part Size]  = sa.PatternCodeSize
    , [PPU]  = sa.PPU
    , [Remark]         = sa.Remark
    , [Create by]      = AddName.IdAndNameAndExt
    , [Create Date]    = sa.AddDate
    , [Edit by]        = EditName.IdAndNameAndExt
    , [Edit Date]      = sa.EditDate
    , s.Picture1
    , s.Picture2
    , ArtworkPicture = sa.Picture
From Style_Artwork as sa with(nolock)
Left join Style s with(nolock) on sa.StyleUkey = s.Ukey
Outer apply (Select IdAndNameAndExt From dbo.GetName Where ID = sa.AddName) as AddName
Outer apply (Select IdAndNameAndExt From dbo.GetName Where ID = sa.EditName) as EditName
where s.Junk = 0 and s.LocalStyle = 0
{where}
Order by s.BrandID, s.SeasonID
";
                result = DBProxy.Current.Select(null, sqlcmd, out this.dtPrint);
                if (!result)
                {
                    return result;
                }

                if (this.dtPrint != null && this.dtPrint.Rows.Count > 0)
                {
                    // 顯示筆數
                    this.SetCount(this.dtPrint.Rows.Count);
                    return this.TransferToExcel();
                }
                else
                {
                    return new DualResult(false, "Data not found.");
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }
        }

        private DualResult TransferToExcel()
        {
            try
            {
                string xltPath = Path.Combine(Env.Cfg.XltPathDir, "Style-R15.Style Artwork.xltx");
                this.sxc = new SaveXltReportCls(xltPath, keepApp: true);
                SaveXltReportCls.XltRptTable xrt = new SaveXltReportCls.XltRptTable(this.dtPrint);

                Excel.Worksheet wks = this.sxc.ExcelApp.Sheets[1];
                wks.Cells[2, 1].Value = "##tbl";

                xrt.ShowHeader = false;
                this.sxc.DicDatas.Add("##tbl", xrt);
                this.sxc.Save();

                wks.UsedRange.VerticalAlignment = Excel.Constants.xlCenter;

                if (this.CkbPictures.Checked)
                {
                    for (int i = 0; i < this.dtPrint.Rows.Count; i++)
                    {
                        DataRow row = this.dtPrint.Rows[i];

                        #region picture1
                        if (!row["Picture1"].Empty())
                        {
                            var imgPath = Path.Combine(TradeSystem.Get(TradeSystem.Columns.PicturePath), row["Picture1"].ToString());
                            if (System.IO.File.Exists(imgPath))
                            {
                                using (var img = Image.FromFile(imgPath))
                                {
                                    var colName = MyExcelPrg.GetExcelColumnName(this.dtPrint.Columns["Picture1"].Ordinal + 1);
                                    var colIndex = MyExcelPrg.GetExcelColumnIndex(colName);
                                    this.SetImageCell(img, imgPath, i, colIndex, wks);
                                }
                            }
                        }
                        #endregion

                        #region picture2
                        if (!row["Picture2"].Empty())
                        {
                            var imgPath = Path.Combine(TradeSystem.Get(TradeSystem.Columns.PicturePath), row["Picture2"].ToString());
                            if (System.IO.File.Exists(imgPath))
                            {
                                using (var img = Image.FromFile(imgPath))
                                {
                                    var colName = MyExcelPrg.GetExcelColumnName(this.dtPrint.Columns["Picture2"].Ordinal + 1);
                                    var colIndex = MyExcelPrg.GetExcelColumnIndex(colName);
                                    this.SetImageCell(img, imgPath, i, colIndex, wks);
                                }
                            }
                        }
                        #endregion

                        #region ArtworkPicture
                        if (!row["ArtworkPicture"].Empty())
                        {
                            var imgPath = Path.Combine(TradeSystem.Get(TradeSystem.Columns.ArtworkPicturePath), row["ArtworkPicture"].ToString());
                            if (System.IO.File.Exists(imgPath))
                            {
                                using (var img = Image.FromFile(imgPath))
                                {
                                    var colName = MyExcelPrg.GetExcelColumnName(this.dtPrint.Columns["ArtworkPicture"].Ordinal + 1);
                                    var colIndex = MyExcelPrg.GetExcelColumnIndex(colName);
                                    this.SetImageCell(img, imgPath, i, colIndex, wks);
                                }
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    // 移除末三欄，圖片欄位
                    int colCnt = wks.UsedRange.Columns.Count;
                    wks.GetRange(MyExcelPrg.GetExcelColumnName(colCnt - 0) + "1").EntireColumn.Delete(); // Artwork Picture
                    wks.GetRange(MyExcelPrg.GetExcelColumnName(colCnt - 1) + "1").EntireColumn.Delete(); // Style Picture 2
                    wks.GetRange(MyExcelPrg.GetExcelColumnName(colCnt - 2) + "1").EntireColumn.Delete(); // Style Picture 1
                }

                this.sxc.FinishSave(); // 存檔再打開

                return Result.True;
            }
            catch (Exception ex)
            {
                if (this.sxc.ExcelApp != null)
                {
                    this.sxc.ExcelApp.DisplayAlerts = false;
                    this.sxc.ExcelApp.Quit();
                }

                return new DualResult(false, "Export excel error.", ex);
            }
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            return true;
        }

        private void EditProgram_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string sql = "select ID, BrandID from Program Order by ID";
            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sql, "ID,Brand", "15,10", string.Empty, null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.program = string.Empty;
            IList<DataRow> selectedData = item.GetSelecteds();
            string program = string.Empty;
            for (int i = 0; i < selectedData.Count; i++)
            {
                if (i == 0)
                {
                    program = selectedData[i].GetValue("ID").ToString().Trim();
                    this.program += selectedData[i].GetValue("ID").ToString().Trim();
                }
                else
                {
                    program += "," + selectedData[i].GetValue("ID").ToString().Trim();
                    this.program += "," + selectedData[i].GetValue("ID").ToString().Trim();
                }
            }

            if (program == string.Empty)
            {
                this.program = string.Empty;
            }

            this.editProgram.Text = program;
        }

        private void EditArtworkType_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string sql = "select ID, SEQ, Classify from ArtworkType where (isArtwork=1 or useArtwork=1) Order by SEQ";
            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sql, "ID, SEQ, Classify", "15,5,3", string.Empty, null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.artworkType = string.Empty;
            IList<DataRow> selectedData = item.GetSelecteds();
            string artworkType = string.Empty;
            for (int i = 0; i < selectedData.Count; i++)
            {
                if (i == 0)
                {
                    artworkType = selectedData[i].GetValue("ID").ToString().Trim();
                    this.artworkType += selectedData[i].GetValue("ID").ToString().Trim();
                }
                else
                {
                    artworkType += "," + selectedData[i].GetValue("ID").ToString().Trim();
                    this.artworkType += "," + selectedData[i].GetValue("ID").ToString().Trim();
                }
            }

            if (artworkType == string.Empty)
            {
                this.artworkType = string.Empty;
            }

            this.editArtworkType.Text = artworkType;
        }

        private void SetImageCell(Image img, string imgPath, int rowIndex, int colIndex, Excel.Worksheet wks)
        {
            int max = img.Width > img.Height ? img.Width : img.Height;
            decimal ratio = 0m;
            if (max < 100)
            {
                ratio = 1;
            }
            else
            {
                ratio = 100m / max;
            }

            var imgWidth = (float)(img.Width * ratio);
            var imgHeight = (float)(img.Height * ratio);

            var cell = wks.Cells[rowIndex + 2, colIndex];
            var cellWidth = cell.Width;
            if (cellWidth < imgWidth)
            {
                cellWidth = cellWidth + imgWidth;
            }

            // 置中距離計算
            float leftDiff = (135 - imgWidth) / 2;
            float topDiff = (100 - imgHeight) / 2;
            wks.Rows[rowIndex + 2].RowHeight = 100;
            wks.Columns[colIndex].ColumnWidth = 25;

            wks.Shapes.AddPicture(imgPath, MsoTriState.msoFalse, MsoTriState.msoTrue, cell.Left + leftDiff, cell.Top + topDiff, imgWidth, imgHeight);
            cell.Value = string.Empty;
        }

        private void TxtMuiltSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string brand = string.Join("','", this.txtbrand.Text.Split(','));
            SelectItem2 item = new SelectItem2($"SELECT ID FROM Season WHERE BrandID IN ('{brand}') AND Junk = 0 GROUP BY ID", "ID", "10", this.txtMuiltSeason.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            if (selectedData.Count == 0)
            {
                this.txtMuiltSeason.Text = string.Empty;
            }
            else
            {
                this.txtMuiltSeason.Text = string.Join(
                    ",",
                        selectedData.Select(row => row["ID"].ToString())
                              .OrderBy(id => id));
            }
        }

        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            var sqlCmd = this.GetSqlCommand();

            using (var dlg = new Sci.Win.Tools.SelectItem(sqlCmd, "20,10,10,50,5", this.Text, "Style#,Season,Brand,Description,CD#"))
            {
                if (dlg.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                this.txtStyle.Text = dlg.GetSelectedString();
            }
        }

        private void TxtStyle_Validating(object sender, CancelEventArgs e)
        {
            var strStyle = this.txtStyle.Text;
            if (strStyle == string.Empty)
            {
                return;
            }

            var sqlCmd = this.GetSqlCommand(strStyle);

            using (var dr = DBProxy.Current.SelectEx(sqlCmd, "id", strStyle))
            {
                if (dr == false)
                {
                    e.Cancel = true;
                    MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                    return;
                }

                var dt = dr.ExtendedData;
                if (dt.Rows.Count == 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.ErrorBox(string.Format("< Style : {0} > not found!!!", strStyle), "Error");
                    return;
                }

                DataRow selectedRow;
                if (dt.Rows.Count == 1)
                {
                    selectedRow = dt.Rows[0];
                }
                else
                {
                    using (var dlg = new SelectItem(sqlCmd, "20,10,10,50,5", this.Text, "Style#,Season,Brand,Description,CD#"))
                    {
                        if (dlg.ShowDialog() != DialogResult.OK)
                        {
                            e.Cancel = true;
                            return;
                        }

                        selectedRow = dlg.GetSelecteds().First();
                    }
                }
            }
        }

        private string GetSqlCommand(string iID = null)
        {
            string sqlWhere = "Where Junk = 0 and LocalStyle = 0";
            string sqlCmd = string.Empty;
            string brand = string.Join("','", this.txtbrand.Text.Split(','));
            string seasons = string.Join("','", this.txtMuiltSeason.Text.Split(','));

            // 帶出登入者的特殊權限: AllCustomer
            bool isAllCustomer = AuthPrg.hasSpecialAuth("CUST");

            // 若非Admin或All Customer就僅帶出該登入者能使用的Brand
            if (!Env.User.IsAdmin && !isAllCustomer)
            {
                sqlWhere += " And BrandID In (Select BrandID From PASS_AuthBrand Where ID = '" + Env.User.UserID + "')";
            }

            if (!MyUtility.Check.Empty(this.txtbrand) && !MyUtility.Check.Empty(this.txtbrand.Text.Trim()))
            {
                sqlWhere += $" And BrandID in ('{brand}')";
            }

            if (!MyUtility.Check.Empty(this.txtMuiltSeason) && !MyUtility.Check.Empty(this.txtMuiltSeason.Text.Trim()))
            {
                sqlWhere += $" And SeasonID  in ('{seasons}')";
            }

            if (!MyUtility.Check.Empty(iID))
            {
                sqlWhere += " And ID = @id";
            }

            sqlCmd = "Select ID, SeasonID, BrandID, Description, CdCodeID From Style " + sqlWhere + " Order by Planning, BrandID, ID, SeasonID";

            return sqlCmd;
        }
    }
}
