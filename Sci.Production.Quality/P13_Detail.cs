using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Win.Tools;
using Sci.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Sci.Production.PublicPrg;

namespace Sci.Production.Quality
{
    public partial class P13_Detail : Sci.Win.Subs.Input4
    {
        private DataRow masterDr;
        private string reportNo;
        private string id;
        private bool isSee = false;
        private string status;
        public P13_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, string status)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.id = id;
            isSee = true;
            this.reportNo = keyvalue2;
            this.status = status;
            MyUtility.Tool.SetupCombox(this.comboTestingMethod, 2, 1,
                "a. 5 cycles continuous wash at 60 degree followed by 5 cycles continuous wash at 60 degree in standard domestic washing machine and tumble dry after the 10th cycle,a. 5 cycles continuous wash at 60 degree followed by 5 cycles continuous wash at 60 degree in standard domestic washing machine and tumble dry after the 10th cycle," +
                "b. 5 cycles continuous wash at 40 degree followed by 5 cycles continuous wash at 60 degree in standard domestic washing machine and tumble dry after the 10th cycle,b. 5 cycles continuous wash at 40 degree followed by 5 cycles continuous wash at 60 degree in standard domestic washing machine and tumble dry after the 10th cycle," +
                "c. 5 cycles continuous wash at 40 degree followed by 5 cycles continuous wash at 40 degree in standard domestic washing machine and tumble dry after the 10th cycle,c. 5 cycles continuous wash at 40 degree followed by 5 cycles continuous wash at 40 degree in standard domestic washing machine and tumble dry after the 10th cycle," +
                "d. 5 cycles continuous wash at 30 degree followed by 5 cycles continuous wash at 30 degree in standard domestic washing machine and tumble dry after the 10th cycle,d. 5 cycles continuous wash at 30 degree followed by 5 cycles continuous wash at 30 degree in standard domestic washing machine and tumble dry after the 10th cycle");

            this.comboTestingMethod.DrawMode = DrawMode.OwnerDrawVariable;
            this.comboTestingMethod.DrawItem += new DrawItemEventHandler(comboBox2_DrawItem);
            this.comboTestingMethod.MeasureItem += new MeasureItemEventHandler(comboBox2_MeasureItem);
            this.comboTestingMethod.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
            this.comboTestingMethod.SelectedIndex = 0;
        }

        private void comboBox2_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            ComboxBoxEx cbox = (ComboxBoxEx)sender;
            e.ItemHeight = 46;
            e.ItemWidth = cbox.DropDownWidth;

            cbox.ItemHeights.Add(e.ItemHeight);
        }

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboxBoxEx cbox = (ComboxBoxEx)sender;
            System.Collections.DictionaryEntry item = (System.Collections.DictionaryEntry)cbox.Items[e.Index];
            string txt = item.Key.ToString();

            e.DrawBackground();
            if (this.EditMode)
            {
                e.Graphics.DrawString(txt, cbox.Font, System.Drawing.Brushes.Red, new RectangleF(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height));
            }
            else
            {
                e.Graphics.DrawString(txt, cbox.Font, System.Drawing.Brushes.Blue, new RectangleF(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height));
            }
            e.Graphics.DrawLine(new Pen(Color.LightGray), e.Bounds.X, e.Bounds.Top + e.Bounds.Height - 1, e.Bounds.Width, e.Bounds.Top + e.Bounds.Height - 1);
            e.DrawFocusRectangle();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboxBoxEx cbox = (ComboxBoxEx)sender;
            if (cbox.SelectedItem == null) return;

            System.Collections.DictionaryEntry item = (System.Collections.DictionaryEntry)cbox.SelectedItem;
            SetComboBoxHeight(this.comboTestingMethod.Handle, 50);
            //label1.Text = item["id"].ToString();
        }

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
        private const Int32 CB_SETITEMHEIGHT = 0x153;

        private void SetComboBoxHeight(IntPtr comboBoxHandle, Int32 comboBoxDesiredHeight)
        {
            SendMessage(comboBoxHandle, CB_SETITEMHEIGHT, -1, comboBoxDesiredHeight);
        }

        protected override DualResult OnRequery()
        {
            #region 表頭設定
            MyUtility.Check.Seek($"select * from MockupWash WITH (NOLOCK) where ID = '{this.id}'", out this.masterDr);
            DataRow Detaildr;
            MyUtility.Check.Seek($"select * from MockupWash_Detail WITH (NOLOCK) where ReportNo = '{this.reportNo}'", out Detaildr);

            this.displayStyleID.Text = this.masterDr["StyleID"].ToString();
            this.displaySeasonID.Text = this.masterDr["SeasonID"].ToString();
            this.displayBrandID.Text = this.masterDr["BrandID"].ToString();
            this.displayArticle.Text = this.masterDr["Article"].ToString();
            if (MyUtility.Check.Empty(Detaildr))
            {
                this.txtCombineStyle.Text = "";
                this.displayNo.Text = "";
                this.displayReportNo.Text = "";
                this.dateBoxSubmitDate.Value = null;
                this.dateBoxReceivedDate.Value = null;
                this.dateBoxReleasedDate.Value = null;
                this.displayResult.Text = "";
                this.txtTechnician.Text = "";
                this.txtMR.Text = "";
            }
            else
            {
                this.txtCombineStyle.Text = Detaildr["CombineStyle"].ToString();
                this.displayNo.Text = Detaildr["NO"].ToString();
                this.displayReportNo.Text = Detaildr["ReportNo"].ToString();
                this.dateBoxSubmitDate.Value = MyUtility.Convert.GetDate(Detaildr["SubmitDate"]);
                this.dateBoxReceivedDate.Value = MyUtility.Convert.GetDate(Detaildr["ReceivedDate"]);
                this.dateBoxReleasedDate.Value = MyUtility.Convert.GetDate(Detaildr["ReleasedDate"]);
                this.displayResult.Text = Detaildr["Result"].ToString();
                this.txtTechnician.textbox1_text = Detaildr["Technician"].ToString();
                this.txtMR.textbox1_text = Detaildr["MR"].ToString();
                this.comboTestingMethod.SelectedValue = Detaildr["TestingMethod"].ToString();
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
                dr["LastUpdate"] = MyUtility.GetValue.Lookup("Name",dr["EditName"].ToString(),"Pass1","ID") + " - " + dr["EditDate"].ToString();
                dr["ArtworkColorName"] = MyUtility.GetValue.Lookup($"select Name from Color WITH (NOLOCK) where ID = '{dr["ArtworkColor"].ToString()}'  and BrandID =  '{this.masterDr["BrandID"]}'");
                dr["FabricColorName"] = MyUtility.GetValue.Lookup($"select Name from Color WITH (NOLOCK) where ID = '{dr["FabricColor"].ToString()}'  and BrandID =  '{this.masterDr["BrandID"]}'");
            }
            #endregion
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            SetComboBoxHeight(this.comboTestingMethod.Handle, 44);
            this.btnPDF.Enabled = !this.EditMode;
            this.btnSendMR.Enabled = !this.EditMode;
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (isSee)
            {
                SetComboBoxHeight(this.comboTestingMethod.Handle, 44);
                this.btnPDF.Enabled = !this.EditMode;
                this.btnSendMR.Enabled = !this.EditMode;

            }
         
        }

        protected override bool OnGridSetup()
        {

            DataGridViewGeneratorTextColumnSettings ResulCell = Sci.Production.PublicPrg.Prgs.cellResult.GetGridCell();
            #region Artwork event
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts_artwork = new DataGridViewGeneratorTextColumnSettings();
            ts_artwork.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = $"Select distinct ArtworkTypeID from Style_Artwork WITH (NOLOCK) where StyleUkey = (select ukey from style where ID = '{this.masterDr["StyleID"]}' and BrandID = '{this.masterDr["BrandID"]}' and SeasonID = '{this.masterDr["SeasonID"]}')";
                    SelectItem item = new SelectItem(item_cmd, "", "");

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
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts_artworkColor = new DataGridViewGeneratorTextColumnSettings();
            ts_artworkColor.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = $"Select BrandID,ID,Name from Color WITH (NOLOCK) where BrandID =  '{this.masterDr["BrandID"]}'";
                    SelectItem item = new SelectItem(item_cmd, "", "");

                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["ArtworkColor"] = item.GetSelecteds()[0]["ID"].ToString();
                    dr["ArtworkColorName"] = item.GetSelecteds()[0]["Name"].ToString();
                    dr.EndEdit();
                }

            };

            ts_artworkColor.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string colorID = MyUtility.GetValue.Lookup($"select ID from Color WITH (NOLOCK) where Name = '{e.FormattedValue}'  and BrandID =  '{this.masterDr["BrandID"]}' ");

                if (MyUtility.Check.Empty(colorID))
                {
                    MyUtility.Msg.WarningBox("Artwork Color not found!");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    dr["ArtworkColor"] = colorID;
                }

            };
            #endregion

            #region Fabric Color event
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts_fabricColor = new DataGridViewGeneratorTextColumnSettings();
            ts_fabricColor.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = $"Select BrandID,ID,Name from Color WITH (NOLOCK) where BrandID =  '{this.masterDr["BrandID"]}'";
                    SelectItem item = new SelectItem(item_cmd, "", "");

                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["FabricColor"] = item.GetSelecteds()[0]["ID"].ToString();
                    dr["FabricColorName"] = item.GetSelecteds()[0]["Name"].ToString();
                    dr.EndEdit();
                }

            };

            ts_fabricColor.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string colorID = MyUtility.GetValue.Lookup($"select ID from Color WITH (NOLOCK) where Name = '{e.FormattedValue}'  and BrandID =  '{this.masterDr["BrandID"]}' ");

                if (MyUtility.Check.Empty(colorID))
                {
                    MyUtility.Msg.WarningBox("Fabric Color not found!");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    dr["FabricColor"] = colorID;
                }

            };
            #endregion
            Helper.Controls.Grid.Generator(this.grid)
            .Text("ArtworkTypeID", "Artwork", width: Widths.AnsiChars(17),settings: ts_artwork)
            .Text("ArtworkColorName", "Artwork Color", width: Widths.AnsiChars(18),settings: ts_artworkColor)
            .Text("FabricRefNo", "Fabric Ref No.", width: Widths.AnsiChars(17))
            .Text("FabricColorName", "Fabric Color", width: Widths.AnsiChars(18), settings: ts_fabricColor)
            .Text("Result", "Result", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: ResulCell)
            .EditText("Remark", "Remark", width: Widths.AnsiChars(15))
            .Text("LastUpdate", "Last Update", width: Widths.AnsiChars(28),iseditingreadonly: true);
            
            return true;
        }

        private void txtCombineStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string item_cmd = "select ID,SeasonID,Description,BrandID from Style WITH (NOLOCK) where Junk = 0 order by ID";
            SelectItem2 item = new SelectItem2(item_cmd,"", "","");
            DialogResult dresult = item.ShowDialog();
            if (dresult == DialogResult.Cancel)
            {
                return;
            }

            txtCombineStyle.Text = item.GetSelectedString().Replace(",","/");
        }

        protected override bool OnSaveBefore()
        {
            if (this.status.Equals("New"))
            {
                //取reporyID
                string country = MyUtility.GetValue.Lookup("select top 1 CountryID from Factory");
                this.KeyValue2 = MyUtility.GetValue.GetID(country + "WA", "MockupWash_Detail", DateTime.Today, 2, "ReportNo", null);
                this.reportNo = this.KeyValue2;
            }
            return base.OnSaveBefore();
        }

        protected override DualResult OnSavePost()
        {
            DualResult execute_result;
            List<SqlParameter> sql_par = new List<SqlParameter>();
            string submitDate = MyUtility.Check.Empty(this.dateBoxSubmitDate.Value) ? "null" : "'"  + this.dateBoxSubmitDate.Text + "'";
            string receivedDate = MyUtility.Check.Empty(this.dateBoxReceivedDate.Value) ? "null" : "'" + this.dateBoxReceivedDate.Text + "'";
            string releasedDate = MyUtility.Check.Empty(this.dateBoxReleasedDate.Value) ? "null" : "'" + this.dateBoxReleasedDate.Text + "'";
            string sql_cmd = string.Empty;
            if (this.status.Equals("New"))
            {
                //取No
                int no = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($"select isnull(max(No),0) + 1 from MockupWash_Detail WITH (NOLOCK) where ID = '{this.id}'"));

                //insert MockupWash_Detail
                sql_cmd = $@"insert into MockupWash_Detail(ID,ReportNo,No,SubmitDate,CombineStyle,Result,ReceivedDate,ReleasedDate,Technician,MR,AddDate,AddName,TestingMethod) 
                                                                    values('{this.id}','{this.reportNo}',{no},{submitDate},'{this.txtCombineStyle.Text}',@Result,{receivedDate},{releasedDate},'{this.txtTechnician.TextBox1.Text}','{this.txtMR.TextBox1.Text}',GETDATE(),@USERID,@TestingMethod);";

                this.status = "Edit";
            }
            else if (this.status.Equals("Edit"))
            {
                sql_cmd = $@"update MockupWash_Detail set CombineStyle = '{this.txtCombineStyle.Text}',SubmitDate = {submitDate},ReceivedDate = {receivedDate}, ReleasedDate = {releasedDate}, Result = @Result, Technician = '{this.txtTechnician.TextBox1.Text}' ,MR = '{this.txtMR.TextBox1.Text}',
                            EditName = @UserID,EditDate = GETDATE(),TestingMethod = @TestingMethod
                            where ReportNo = '{this.reportNo}';";
            }

            //取Result
            var group_result = ((DataTable)gridbs.DataSource).AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).GroupBy(s => s["Result"]).Select(group => new { Result = group.Key, count = group.Count() });
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
            }

            sql_par.AddRange( new List<SqlParameter>()
                {
                    new SqlParameter("@Result",result),
                    new SqlParameter("@UserID",Env.User.UserID),
                    new SqlParameter("@TestingMethod",this.comboTestingMethod.SelectedValue)
                });

            string upd_master = $@"update MockupWash set ReceivedDate = mdReceivedDate ,ReleasedDate = mdReleasedDate
 from (select max(ReceivedDate) mdReceivedDate, max(ReleasedDate) mdReleasedDate from MockupWash_Detail where id = '{this.id}' ) md
 where MockupWash.id ='{this.id}';";
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
            string pdf_path = CreatePDF();
            this.HideWaitMessage();
            if (pdf_path.Equals(string.Empty))
            {
                MyUtility.Msg.WarningBox("Create PDF fail");
                return;
            }
            string mailto = MyUtility.GetValue.Lookup("Email",this.txtMR.TextBox1.Text,"Pass1","ID");
            string mailcc = Env.User.MailAddress;
            string subject = "Mockup Wash Test – ReportNo:" + this.reportNo;
            string content = "Attachment is Mockup Wash Test– ReportNo:" + this.reportNo + " detail data";
            var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailcc, subject, pdf_path, content.ToString(), false, true);
            email.ShowDialog(this);
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            string pdf_path = CreatePDF();
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

        private string CreatePDF()
        {
            this.ShowWaitMessage("PDF Processing...");
            DataTable gridData = (DataTable)gridbs.DataSource;
            if (gridData.Rows.Count == 0)
            {
                return "1";
            }

            string sql_cmd = string.Empty;
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_P13_Detail_Report.xltx");
            objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            //設定表頭資料
            worksheet.Cells[4,2] = this.displayReportNo.Text;
            worksheet.Cells[5,2] = this.masterDr["T1Subcon"].ToString();
            worksheet.Cells[6,2] = this.masterDr["T2Supplier"].ToString();
            worksheet.Cells[7,2] = this.comboTestingMethod.SelectedValue;
            worksheet.Cells[4,6] = MyUtility.Check.Empty(this.dateBoxReleasedDate.Value) ? string.Empty : this.dateBoxReleasedDate.Text;
            worksheet.Cells[5,6] = MyUtility.Check.Empty(this.dateBoxSubmitDate.Value) ? string.Empty : this.dateBoxSubmitDate.Text;
            worksheet.Cells[6,6] = MyUtility.Check.Empty(this.dateBoxReceivedDate.Value) ? string.Empty : this.dateBoxReceivedDate.Text;
            worksheet.Cells[7,6] = this.masterDr["SeasonID"].ToString();

            //插入圖片與Technician名字
            sql_cmd = $@"select [name] = p.name + ' / LAB TECHNICIAN',[SignaturePic] = s.PicPath + t.SignaturePic
from Technician t WITH (NOLOCK)
inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
outer apply (select PicPath from system) s 
where t.ID = '{this.txtTechnician.TextBox1.Text}'";
            DataRow drTechnicianInfo;
            string technicianName = string.Empty;
            string picSource = string.Empty;
            Image img = null;
            Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[12, 2];

            if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
            {
                technicianName = drTechnicianInfo["name"].ToString();
                picSource = drTechnicianInfo["SignaturePic"].ToString();
            }

            worksheet.Cells[13, 2] = technicianName;

            if (!MyUtility.Check.Empty(picSource))
            {
                if (File.Exists(picSource))
                {
                    img = Image.FromFile(picSource);
                    worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cell.Left, cell.Top, 100, 24);
                }
            }

            #region 表身資料
            //插入表格資料
            string styleNo = MyUtility.Check.Empty(this.txtCombineStyle.Text) ? this.masterDr["StyleID"].ToString() : this.masterDr["StyleID"].ToString() + "/ " + this.txtCombineStyle.Text.Replace("/","/ ");
            string refColor = string.Empty;
            string printArtwork = string.Empty;

            //插入多的row
            if (gridData.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A10:G10", Type.Missing).EntireRow;
                for (int i = 1; i < gridData.Rows.Count; i++)
                {
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                }
                Marshal.ReleaseComObject(rngToInsert);
            }
            //塞進資料
            int start_row = 10;
            foreach (DataRow dr in gridData.Rows)
            {
                worksheet.Cells[start_row, 1] = styleNo;
                worksheet.Cells[start_row, 2] = MyUtility.Check.Empty(dr["FabricColorName"]) ? dr["FabricRefNo"].ToString() : dr["FabricRefNo"].ToString() + "_ " + dr["FabricColorName"].ToString(); 
                worksheet.Cells[start_row, 3] = MyUtility.Check.Empty(dr["ArtworkColorName"]) ? dr["ArtworkTypeID"].ToString() : dr["ArtworkTypeID"].ToString() + "_ " + dr["ArtworkColorName"].ToString(); 
                worksheet.Cells[start_row, 4] = dr["Result"].ToString();
                worksheet.Cells[start_row, 5] = dr["Remark"].ToString();
                worksheet.Rows[start_row].Font.Bold = false;
                worksheet.Rows[start_row].WrapText = true;
                worksheet.Rows[start_row].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                start_row++;
            }
            #endregion

            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;
            strFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P13_Detail_Report");
            strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P13_Detail_Report", Sci.Production.Class.PDFFileNameExtension.PDF);
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
                return "";
            }
        }
    }

    public partial class ComboxBoxEx : Sci.Win.UI.ComboBox
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner 
            public int Top;         // y position of upper-left corner 
            public int Right;       // x position of lower-right corner 
            public int Bottom;      // y position of lower-right corner 
        }

        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_NOOWNERZORDER = 0x0200;

        public const int WM_CTLCOLORLISTBOX = 0x0134;

        private int _hwndDropDown = 0;

        internal List<int> ItemHeights = new List<int>();

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CTLCOLORLISTBOX)
            {
                if (_hwndDropDown == 0)
                {
                    _hwndDropDown = m.LParam.ToInt32();

                    RECT r;
                    GetWindowRect((IntPtr)_hwndDropDown, out r);

                    int newHeight = 0;
                    int n = (Items.Count > MaxDropDownItems) ? MaxDropDownItems : Items.Count;
                    for (int i = 0; i < n; i++)
                    {
                        newHeight += ItemHeights[i];
                    }
                    newHeight += 5; //to stop scrollbars showing

                    SetWindowPos((IntPtr)_hwndDropDown, IntPtr.Zero,
                        r.Left,
                                 r.Top,
                                 DropDownWidth,
                                 newHeight,
                                 SWP_FRAMECHANGED |
                                     SWP_NOACTIVATE |
                                     SWP_NOZORDER |
                                     SWP_NOOWNERZORDER);
                }
            }

            base.WndProc(ref m);
        }
        
        protected override void OnDropDownClosed(EventArgs e)
        {
            _hwndDropDown = 0;
            base.OnDropDownClosed(e);
        }
    }
}
