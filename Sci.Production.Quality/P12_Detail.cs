using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Win.Tools;
using Sci.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Sci.Production.PublicPrg;

namespace Sci.Production.Quality
{
    public partial class P12_Detail : Win.Subs.Input4
    {
        private DataRow masterDr;
        private DataRow Detaildr;
        private string reportNo;
        private string id;
        private bool isSee = false;
        private string status;

        public P12_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, string status)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.id = id;
            this.isSee = true;
            this.reportNo = keyvalue2;
            this.status = status;
        }

        protected override DualResult OnRequery()
        {
            #region 表頭設定
            MyUtility.Check.Seek($"select * from MockupOven WITH (NOLOCK) where ID = '{this.id}'", out this.masterDr);
            MyUtility.Check.Seek($"select * from MockupOven_Detail WITH (NOLOCK) where ReportNo = '{this.reportNo}'", out this.Detaildr);

            this.displayStyleID.Text = this.masterDr["StyleID"].ToString();
            this.displaySeasonID.Text = this.masterDr["SeasonID"].ToString();
            this.displayBrandID.Text = this.masterDr["BrandID"].ToString();
            this.displayArticle.Text = this.masterDr["Article"].ToString();
            if (MyUtility.Check.Empty(this.Detaildr))
            {
                this.txtCombineStyle.Text = string.Empty;
                this.displayNo.Text = string.Empty;
                this.displayReportNo.Text = string.Empty;
                this.dateBoxSubmitDate.Value = null;
                this.dateBoxReceivedDate.Value = null;
                this.dateBoxReleasedDate.Value = null;
                this.displayResult.Text = string.Empty;
                this.txtTechnician.Text = string.Empty;
                this.txtMR.Text = string.Empty;
                this.numTestTemp.Value = 70;
                this.numTestTime.Value = 48;
                this.numAPT.Value = 0;
                this.numAFT.Value = 0;
                this.numCT.Value = 0;
                this.numP.Value = 0;
                this.numT.Value = 0;
                this.num2Pr.Value = 0;
                this.num2Pnr.Value = 0;
                this.txtPOff.Text = string.Empty;
            }
            else
            {
                this.txtCombineStyle.Text = this.Detaildr["CombineStyle"].ToString();
                this.displayNo.Text = this.Detaildr["NO"].ToString();
                this.displayReportNo.Text = this.Detaildr["ReportNo"].ToString();
                this.dateBoxSubmitDate.Value = MyUtility.Convert.GetDate(this.Detaildr["SubmitDate"]);
                this.dateBoxReceivedDate.Value = MyUtility.Convert.GetDate(this.Detaildr["ReceivedDate"]);
                this.dateBoxReleasedDate.Value = MyUtility.Convert.GetDate(this.Detaildr["ReleasedDate"]);
                this.displayResult.Text = this.Detaildr["Result"].ToString();
                this.txtTechnician.Textbox1_text = this.Detaildr["Technician"].ToString();
                this.txtMR.Textbox1_text = this.Detaildr["MR"].ToString();
                this.numTestTemp.Value = MyUtility.Convert.GetDecimal(this.Detaildr["TestTemperature"]);
                this.numTestTime.Value = MyUtility.Convert.GetDecimal(this.Detaildr["TestTime"]);
                this.numAPT.Value = MyUtility.Convert.GetDecimal(this.Detaildr["HTPlate"]);
                this.numAFT.Value = MyUtility.Convert.GetDecimal(this.Detaildr["HTFlim"]);
                this.numCT.Value = MyUtility.Convert.GetDecimal(this.Detaildr["HTCoolingTime"]);
                this.numP.Value = MyUtility.Convert.GetDecimal(this.Detaildr["HTPressure"]);
                this.numT.Value = MyUtility.Convert.GetDecimal(this.Detaildr["HTTime"]);
                this.num2Pr.Value = MyUtility.Convert.GetDecimal(this.Detaildr["HT2ndPressreversed"]);
                this.num2Pnr.Value = MyUtility.Convert.GetDecimal(this.Detaildr["HT2ndPressnoreverse"]);
                this.txtPOff.Text = this.Detaildr["HTPellOff"].ToString();
            }

            #endregion

            return base.OnRequery();
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("LastUpdate", typeof(string));
            datas.Columns.Add("ArtworkColorName", typeof(string));
            datas.Columns.Add("FabricColorName", typeof(string));
            #region 跑迴圈丟值進去
            foreach (DataRow dr in datas.Rows)
            {
                if (MyUtility.Check.Empty(dr["EditName"]))
                {
                    dr["LastUpdate"] = MyUtility.GetValue.Lookup("Name", dr["AddName"].ToString(), "Pass1", "ID") + " - " + dr["AddDate"].ToString();
                }
                else
                {
                    dr["LastUpdate"] = MyUtility.GetValue.Lookup("Name", dr["EditName"].ToString(), "Pass1", "ID") + " - " + dr["EditDate"].ToString();
                }

                // 跑回圈將ArtworkColor,FabricColor 拆開後在串color 取得colorname塞入表身
                string colorName = string.Empty;
                string[] drArry = dr["ArtworkColor"].ToString().Split(';');
                foreach (var item in drArry)
                {
                    colorName += MyUtility.GetValue.Lookup($"select Name from Color WITH (NOLOCK) where ID = '{item}'  and BrandID =  '{this.masterDr["BrandID"]}'") + ",";
                }

                dr["ArtworkColorName"] = colorName.Substring(0, colorName.Length - 1);

                string FabName = string.Empty;
                string[] drFab = dr["FabricColor"].ToString().Split(';');
                foreach (var item in drFab)
                {
                    FabName += MyUtility.GetValue.Lookup($"select Name from Color WITH (NOLOCK) where ID = '{item}'  and BrandID =  '{this.masterDr["BrandID"]}'") + ",";
                }

                dr["FabricColorName"] = FabName.Substring(0, FabName.Length - 1);
            }
            #endregion
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.btnPDF.Enabled = !this.EditMode;
            this.btnSendMR.Enabled = !this.EditMode;
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.isSee)
            {
                this.btnPDF.Enabled = !this.EditMode;
                this.btnSendMR.Enabled = !this.EditMode;
            }
        }

        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings ResulCell = Prgs.cellResult.GetGridCell();
            #region Artwork event
            DataGridViewGeneratorTextColumnSettings ts_artwork = new DataGridViewGeneratorTextColumnSettings();
            ts_artwork.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = $"Select distinct ArtworkTypeID from Style_Artwork WITH (NOLOCK) where StyleUkey = (select ukey from style where ID = '{this.masterDr["StyleID"]}' and BrandID = '{this.masterDr["BrandID"]}' and SeasonID = '{this.masterDr["SeasonID"]}')";
                    SelectItem item = new SelectItem(item_cmd, string.Empty, string.Empty);

                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ArtworkTypeID"] = item.GetSelecteds()[0]["ArtworkTypeID"].ToString();
                    dr.EndEdit();
                }
            };

            ts_artwork.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                if (!MyUtility.Check.Seek($"Select 1 from Style_Artwork WITH (NOLOCK) where ArtworkTypeID = '{e.FormattedValue}' and StyleUkey = (select ukey from style where ID = '{this.masterDr["StyleID"]}' and BrandID = '{this.masterDr["BrandID"]}' and SeasonID = '{this.masterDr["SeasonID"]}')"))
                {
                    MyUtility.Msg.WarningBox("Artwork not found!");
                    e.Cancel = true;
                    return;
                }
            };
            #endregion
            #region Artwork Color event
            DataGridViewGeneratorTextColumnSettings ts_artworkColor = new DataGridViewGeneratorTextColumnSettings();
            ts_artworkColor.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = $"Select BrandID,ID,Name from Color WITH (NOLOCK) where BrandID =  '{this.masterDr["BrandID"]}'";
                    SelectItem2 item = new SelectItem2(item_cmd, string.Empty, string.Empty, string.Empty, null, "ID");

                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ArtworkColor"] = item.GetSelectedString().ToString().Replace(",", ";");
                    string colorName = string.Empty;
                    if (item.GetSelecteds().Count > 0)
                    {
                        foreach (DataRow its in item.GetSelecteds())
                        {
                            colorName += its["Name"] + ",";
                        }

                        if (colorName.Length > 0)
                        {
                            dr["ArtworkColorName"] = colorName.Substring(0, colorName.Length - 1);
                        }
                    }

                    dr.EndEdit();
                }
            };

            ts_artworkColor.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string[] drArry = e.FormattedValue.ToString().Split(',');
                string colorID = string.Empty;
                foreach (var item in drArry)
                {
                    colorID += MyUtility.GetValue.Lookup($"select ID from Color WITH (NOLOCK) where Name = '{item}'  and BrandID =  '{this.masterDr["BrandID"]}' ") + ";";

                    if (MyUtility.Check.Empty(colorID))
                    {
                        MyUtility.Msg.WarningBox("Artwork Color not found!");
                        e.Cancel = true;
                        return;
                    }
                }

                dr["ArtworkColor"] = colorID.Substring(0, colorID.Length - 1);
                dr.EndEdit();
            };
            #endregion

            #region Fabric Color event
            DataGridViewGeneratorTextColumnSettings ts_fabricColor = new DataGridViewGeneratorTextColumnSettings();
            ts_fabricColor.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = $"Select BrandID,ID,Name from Color WITH (NOLOCK) where BrandID =  '{this.masterDr["BrandID"]}'";
                    SelectItem2 item = new SelectItem2(item_cmd, string.Empty, string.Empty, string.Empty, null, "ID");

                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["FabricColor"] = item.GetSelectedString().ToString().Replace(",", ";");
                    string colorName = string.Empty;
                    if (item.GetSelecteds().Count > 0)
                    {
                        foreach (DataRow its in item.GetSelecteds())
                        {
                            colorName += its["Name"] + ",";
                        }

                        if (colorName.Length > 0)
                        {
                            dr["FabricColorName"] = colorName.Substring(0, colorName.Length - 1);
                        }
                    }

                    dr.EndEdit();
                }
            };

            ts_fabricColor.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string[] drArry = e.FormattedValue.ToString().Split(',');
                string colorID = string.Empty;
                foreach (var item in drArry)
                {
                    colorID += MyUtility.GetValue.Lookup($"select ID from Color WITH (NOLOCK) where Name = '{item}'  and BrandID =  '{this.masterDr["BrandID"]}' ") + ";";

                    if (MyUtility.Check.Empty(colorID))
                    {
                        MyUtility.Msg.WarningBox("Fabric Color not found!");
                        e.Cancel = true;
                        return;
                    }
                }

                dr["FabricColor"] = colorID.Substring(0, colorID.Length - 1);
                dr.EndEdit();
            };
            #endregion
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("ArtworkTypeID", "Artwork", width: Widths.AnsiChars(17), settings: ts_artwork)
            .Text("Typeofprint", "Typeofprint", width: Widths.AnsiChars(17))
            .Text("Design", "Design", width: Widths.AnsiChars(15), iseditingreadonly: false)
            .Text("ArtworkColorName", "Artwork Color", width: Widths.AnsiChars(18), settings: ts_artworkColor)
            .Text("FabricRefNo", "Fabric Ref No.", width: Widths.AnsiChars(17))
            .Text("FabricColorName", "Fabric Color", width: Widths.AnsiChars(18), settings: ts_fabricColor)
            .Text("Result", "Result", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: ResulCell)
            .EditText("Remark", "Remark", width: Widths.AnsiChars(15))
            .Text("LastUpdate", "Last Update", width: Widths.AnsiChars(28), iseditingreadonly: true);

            return true;
        }

        private void txtCombineStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string item_cmd = "select ID,SeasonID,Description,BrandID from Style WITH (NOLOCK) where Junk = 0 order by ID";
            SelectItem2 item = new SelectItem2(item_cmd, string.Empty, string.Empty, string.Empty);
            DialogResult dresult = item.ShowDialog();
            if (dresult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCombineStyle.Text = item.GetSelectedString().Replace(",", "/");
        }

        protected override bool OnSaveBefore()
        {
            if (this.status.Equals("New"))
            {
                // 取reporyID
                string country = MyUtility.GetValue.Lookup("select top 1 CountryID from Factory");
                this.KeyValue2 = MyUtility.GetValue.GetID(country + "OV", "MockupOven_Detail", DateTime.Today, 2, "ReportNo", null);
                this.reportNo = this.KeyValue2;
            }

            return base.OnSaveBefore();
        }

        protected override DualResult OnSavePost()
        {
            DualResult execute_result;
            List<SqlParameter> sql_par = new List<SqlParameter>();
            string submitDate = MyUtility.Check.Empty(this.dateBoxSubmitDate.Value) ? "null" : "'" + this.dateBoxSubmitDate.Text + "'";
            string receivedDate = MyUtility.Check.Empty(this.dateBoxReceivedDate.Value) ? "null" : "'" + this.dateBoxReceivedDate.Text + "'";
            string releasedDate = MyUtility.Check.Empty(this.dateBoxReleasedDate.Value) ? "null" : "'" + this.dateBoxReleasedDate.Text + "'";
            string sql_cmd = string.Empty;
            if (this.status.Equals("New"))
            {
                // 取No
                int no = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($"select isnull(max(No),0) + 1 from MockupOven_Detail WITH (NOLOCK) where ID = '{this.id}'"));

                // insert MockupOven_Detail
                sql_cmd = $@"
insert into MockupOven_Detail(ID,ReportNo,No,SubmitDate,CombineStyle,Result,ReceivedDate,ReleasedDate,Technician,MR,AddDate,AddName,TestTemperature,TestTime,
                                            HTPlate,HTFlim,HTCoolingTime,HTPressure,HTTime,HT2ndPressreversed,HT2ndPressnoreverse,HTPellOff) 
values('{this.id}','{this.reportNo}',{no},{submitDate},'{this.txtCombineStyle.Text}',@Result,{receivedDate},{releasedDate},'{this.txtTechnician.TextBox1.Text}','{this.txtMR.TextBox1.Text}',GETDATE(),@USERID,@TestTemperature,@TestTime,
            '{this.numAPT.Value}', '{this.numAFT.Value}','{this.numCT.Value}','{this.numP.Value}','{this.numT.Value}','{this.num2Pr.Value}','{this.num2Pnr.Value}','{this.txtPOff.Text}');";

                this.status = "Edit";
            }
            else if (this.status.Equals("Edit"))
            {
                sql_cmd = $@"
update MockupOven_Detail 
    set CombineStyle = '{this.txtCombineStyle.Text}',SubmitDate = {submitDate},ReceivedDate = {receivedDate}, ReleasedDate = {releasedDate}, Result = @Result, Technician = '{this.txtTechnician.TextBox1.Text}' ,MR = '{this.txtMR.TextBox1.Text}',EditName = @UserID,EditDate = GETDATE(),TestTemperature = @TestTemperature,TestTime = @TestTime,
    HTPlate= '{this.numAPT.Value}', HTFlim='{this.numAFT.Value}',HTCoolingTime='{this.numCT.Value}',HTPressure='{this.numP.Value}',HTTime='{this.numT.Value}',
    HT2ndPressreversed='{this.num2Pr.Value}',HT2ndPressnoreverse='{this.num2Pnr.Value}',HTPellOff='{this.txtPOff.Text}'
where ReportNo = '{this.reportNo}';";
            }

            // 取Result
            var group_result = ((DataTable)this.gridbs.DataSource).AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).GroupBy(s => s["Result"]).Select(group => new { Result = group.Key, count = group.Count() });
            string result = string.Empty;
            if (group_result.Count() > 1)
            {
                result = "Fail";
            }
            else if (group_result.Count() == 1)
            {
                if (group_result.First().Result.Equals("Pass"))
                {
                    result = "Pass";
                }
                else if (group_result.First().Result.Equals("Fail"))
                {
                    result = "Fail";
                }
            }

            sql_par.AddRange(new List<SqlParameter>()
                {
                    new SqlParameter("@Result", result),
                    new SqlParameter("@UserID", Env.User.UserID),
                    new SqlParameter("@TestTemperature", this.numTestTemp.Value),
                    new SqlParameter("@TestTime", this.numTestTime.Value),
                });

            string upd_master = $@"update MockupOven set ReceivedDate = mdReceivedDate ,ReleasedDate = mdReleasedDate
 from (select max(ReceivedDate) mdReceivedDate, max(ReleasedDate) mdReleasedDate from MockupOven_Detail where id = '{this.id}' ) md
 where MockupOven.id ='{this.id}';";
            execute_result = DBProxy.Current.Execute(null, sql_cmd + upd_master, sql_par);
            if (!execute_result)
            {
                return execute_result;
            }

            return base.OnSavePost();
        }

        private void txtCombineStyle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSendMR_Click(object sender, EventArgs e)
        {
            string pdf_path = this.CreatePDF();
            this.HideWaitMessage();
            if (pdf_path.Equals(string.Empty))
            {
                MyUtility.Msg.WarningBox("Create PDF fail");
                return;
            }

            string mailto = MyUtility.GetValue.Lookup("Email", this.txtMR.TextBox1.Text, "Pass1", "ID");
            string mailcc = Env.User.MailAddress;
            string subject = "Mockup Oven Test – ReportNo:" + this.reportNo + @" – Style#: " + this.masterDr["StyleID"].ToString();
            string content = "Attachment is Mockup Oven Test– ReportNo:" + this.reportNo + " detail data";
            var email = new MailTo(Env.Cfg.MailFrom, mailto, mailcc, subject, pdf_path, content.ToString(), false, true);
            email.ShowDialog(this);
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            string pdf_path = this.CreatePDF();
            this.HideWaitMessage();
            if (pdf_path.Equals(string.Empty))
            {
                MyUtility.Msg.WarningBox("Create PDF fail");
                return;
            }
            else if (pdf_path.Equals("1"))
            {
                MyUtility.Msg.WarningBox("Detail no data");
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(pdf_path);
            Process.Start(startInfo);
        }

        protected override void OnInsertPrepare(DataRow data)
        {
            base.OnInsertPrepare(data);
            data["Result"] = "Pass";
        }

        private string CreatePDF()
        {
            bool haveHT = ((DataTable)this.gridbs.DataSource).AsEnumerable().Any(r => MyUtility.Convert.GetString(r["ArtworkTypeID"]).EqualString("HEAT TRANSFER"));
            this.ShowWaitMessage("PDF Processing...");
            DataTable gridData = (DataTable)this.gridbs.DataSource;
            if (gridData.Rows.Count == 0)
            {
                return "1";
            }

            string file = haveHT ? "Quality_P12_Detail_Report2" : "Quality_P12_Detail_Report";
            int haveHTrow = haveHT ? 6 : 0;
            string sql_cmd = string.Empty;
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + file + ".xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            // 設定表頭資料
            worksheet.Cells[4, 2] = this.displayReportNo.Text;
            worksheet.Cells[5, 2] = this.masterDr["T1Subcon"].ToString() + "-" + MyUtility.GetValue.Lookup("Abb", this.masterDr["T1Subcon"].ToString(), "LocalSupp", "ID");
            worksheet.Cells[6, 2] = this.masterDr["T2Supplier"].ToString() + "-" + MyUtility.GetValue.Lookup(
                $"select Abb from LocalSupp WITH (NOLOCK) where  Junk =  0  and ID = '{this.masterDr["T2Supplier"].ToString()}'   union all select [Abb] = AbbEN from Supp WITH (NOLOCK) where  Junk =  0 and ID = '{this.masterDr["T2Supplier"].ToString()}' ",
                "Production");
            worksheet.Cells[7, 2] = this.masterDr["BrandID"].ToString();
            worksheet.Cells[8, 2] = $"5.14 color migration test({this.numTestTemp.Text} degree @ {this.numTestTime.Text} hours)";
            worksheet.Cells[4, 6] = MyUtility.Check.Empty(this.dateBoxReleasedDate.Value) ? string.Empty : this.dateBoxReleasedDate.Text;
            worksheet.Cells[5, 6] = MyUtility.Check.Empty(this.dateBoxSubmitDate.Value) ? string.Empty : this.dateBoxSubmitDate.Text;
            worksheet.Cells[6, 6] = this.masterDr["SeasonID"].ToString();
            if (haveHT)
            {
                worksheet.Cells[10, 2] = this.Detaildr["HTPlate"].ToString();
                worksheet.Cells[11, 2] = this.Detaildr["HTFlim"].ToString();
                worksheet.Cells[12, 2] = this.Detaildr["HTTime"].ToString();
                worksheet.Cells[13, 2] = this.Detaildr["HTPressure"].ToString();
                worksheet.Cells[10, 6] = this.Detaildr["HTPellOff"].ToString();
                worksheet.Cells[11, 6] = this.Detaildr["HT2ndPressnoreverse"].ToString();
                worksheet.Cells[12, 6] = this.Detaildr["HT2ndPressreversed"].ToString();
                worksheet.Cells[13, 6] = this.Detaildr["HTCoolingTime"].ToString();
            }

            // 插入圖片與Technician名字
            sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
from Technician t WITH (NOLOCK)
inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
outer apply (select PicPath from system) s 
where t.ID = '{this.txtTechnician.TextBox1.Text}'";
            DataRow drTechnicianInfo;
            string technicianName = string.Empty;
            string picSource = string.Empty;
            Image img = null;
            Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[12 + haveHTrow, 2];

            if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
            {
                technicianName = drTechnicianInfo["name"].ToString();
                picSource = drTechnicianInfo["SignaturePic"].ToString();
            }

            worksheet.Cells[13 + haveHTrow, 2] = technicianName;

            if (!MyUtility.Check.Empty(picSource))
            {
                if (File.Exists(picSource))
                {
                    img = Image.FromFile(picSource);
                    worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cell.Left, cell.Top, 100, 24);
                }
            }

            #region 表身資料

            // 插入表格資料
            string styleNo = MyUtility.Check.Empty(this.txtCombineStyle.Text) ? this.masterDr["StyleID"].ToString() : this.masterDr["StyleID"].ToString() + "/ " + this.txtCombineStyle.Text.Replace("/", "/ ");
            string refColor = string.Empty;
            string printArtwork = string.Empty;

            // 插入多的row
            if (gridData.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range($"A{10 + haveHTrow}:G{10 + haveHTrow}", Type.Missing).EntireRow;
                for (int i = 1; i < gridData.Rows.Count; i++)
                {
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    worksheet.get_Range(string.Format("E{0}:G{0}", MyUtility.Convert.GetString(10 + haveHTrow + i - 1))).Merge(false);
                }

                Marshal.ReleaseComObject(rngToInsert);
            }

            // 塞進資料
            int start_row = 10 + haveHTrow;
            foreach (DataRow dr in gridData.Rows)
            {
                string remark = dr["Remark"].ToString();
                string fabric = MyUtility.Check.Empty(dr["FabricColorName"]) ? dr["FabricRefNo"].ToString() : dr["FabricRefNo"].ToString() + " - " + dr["FabricColorName"].ToString();
                string Artwork = MyUtility.Check.Empty(dr["ArtworkTypeID"]) ? dr["Design"] + " - " + dr["ArtworkColorName"].ToString() : dr["ArtworkTypeID"].ToString() + "/" + dr["Design"] + " - " + dr["ArtworkColorName"].ToString();

                worksheet.Cells[start_row, 1] = styleNo;
                worksheet.Cells[start_row, 2] = fabric;
                worksheet.Cells[start_row, 3] = Artwork;
                worksheet.Cells[start_row, 4] = dr["Result"].ToString();
                worksheet.Cells[start_row, 5] = dr["Remark"].ToString();
                worksheet.Rows[start_row].Font.Bold = false;
                worksheet.Rows[start_row].WrapText = true;
                worksheet.Rows[start_row].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // 合併儲存格無法AutoFit()因此要自己算高度
                if (fabric.Length > remark.Length || Artwork.Length > remark.Length)
                {
                    worksheet.Rows[start_row].AutoFit();
                }
                else
                {
                    worksheet.Range[$"E{start_row}", $"E{start_row}"].RowHeight = ((remark.Length / 20) + 1) * 16.5;
                }

                start_row++;
            }
            #endregion

            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;
            strFileName = Class.MicrosoftFile.GetName(file);
            strPDFFileName = Class.MicrosoftFile.GetName(file, Class.PDFFileNameExtension.PDF);
            objApp.ActiveWorkbook.SaveAs(strFileName);
            objApp.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);

            if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
            {
                return strPDFFileName;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
