using Ict;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using Sci.Production.IE;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P24 : Win.Tems.QueryForm
    {
        private Form frontForm;
        private System.Windows.Forms.Button btnCurrentPage;
        private bool canNew;

        private enum Status
        {
            hasStyle = 1,
            noStyle = 2,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="P01"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P24(ToolStripMenuItem menuitem)
             : base(menuitem)
        {
            this.InitializeComponent();
            this.canNew = Prgs.GetAuthority(Env.User.UserID, "P24. Query Handover List", "CanNew");
            this.EditMode = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            //EndlineInfo.Shift = null;
            //EndlineInfo.P01_CanConfirm = Prgs.GetAuthority(Env.User.UserID, "P01. Sintex End-line E-Inspection system", "CanConfirm");
        }

        public P24(string styleID, string seasonID, string brandID, ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtStyle.Text = styleID;
            this.txtSeason.Text = seasonID;
            this.comboBrand.Text = brandID;
            this.canNew = Prgs.GetAuthority(Env.User.UserID, "P24. Query Handover List", "CanNew");
            this.Init_Layout();
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        /// <inheritdoc/>
        public void P24Call(string styleID, string seasonID, string brandID)
        {
            this.txtStyle.Text = styleID;
            this.txtSeason.Text = seasonID;
            this.comboBrand.Text = brandID;
            this.canNew = Prgs.GetAuthority(Env.User.UserID, "P24. Query Handover List", "CanNew");
            this.Init_Layout();
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void Init_Layout()
        {
            this.EditMode = true;

            this.SetBottomInfo();
            this.SetMenuClick(this.left_btn_Sketch, new P24_Sketch(this.txtStyle.Text, this.txtSeason.Text, this.comboBrand.Text));
            this.SetMenuClick(this.left_btn_FinalPatternAndMarkerList, null); // 先放空白
            this.SetMenuClick(this.left_btn_OperationsBreakdown, new P24_Operations_Breakdown(this.txtStyle.Text, this.txtSeason.Text, this.comboBrand.Text));
            this.SetMenuClick(this.left_btn_CriticalOperations, new P24_Critical_Operations(this.txtStyle.Text));
            this.SetMenuClick(this.left_btn_TemplateAutoTemplateList, new P24_TemplateList(this.txtStyle.Text, this.txtSeason.Text, this.comboBrand.Text, this.canNew, "Template"));
            this.SetMenuClick(this.left_btn_SkillMatrix, null); // 先放空白
            this.SetMenuClick(this.left_btn_SpecialTools, new P24_TemplateList(this.txtStyle.Text, this.txtSeason.Text, this.comboBrand.Text, this.canNew, "SpecialTools"));
        }

        private void StatusChange(Status status)
        {
            switch (status)
            {
                case Status.noStyle:
                    this.left_btn_Sketch.Enabled = false;
                    this.left_btn_AD.Enabled = false;
                    this.left_btn_FinalPatternAndMarkerList.Enabled = false;
                    this.left_btn_OperationsBreakdown.Enabled = false;
                    this.left_btn_CriticalOperations.Enabled = false;
                    this.left_btn_TemplateAutoTemplateList.Enabled = false;
                    this.left_btn_SkillMatrix.Enabled = false;
                    this.left_btn_LineLayoutMachine.Enabled = false;
                    this.left_btn_SpecialTools.Enabled = false;
                    break;
                case Status.hasStyle:
                    this.left_btn_Sketch.Enabled = true;
                    this.left_btn_AD.Enabled = true;
                    this.left_btn_FinalPatternAndMarkerList.Enabled = true;
                    this.left_btn_OperationsBreakdown.Enabled = true;
                    this.left_btn_CriticalOperations.Enabled = true;
                    this.left_btn_TemplateAutoTemplateList.Enabled = true;
                    this.left_btn_SkillMatrix.Enabled = true;
                    this.left_btn_LineLayoutMachine.Enabled = true;
                    this.left_btn_SpecialTools.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set Bottom Info
        /// </summary>
        public void SetBottomInfo()
        {
            if (MyUtility.Check.Empty(this.txtSeason.Text) ||
                MyUtility.Check.Empty(this.txtStyle.Text)  ||
                MyUtility.Check.Empty(this.comboBrand.Text))
            {
                this.StatusChange(Status.noStyle);
            }
            else
            {
                this.StatusChange(Status.hasStyle);
            }

            //this.left_btn_Sketch.PerformClick();
        }

        private void SetMenuClick(System.Windows.Forms.Button setBtn, Form calledForm)
        {
            if (calledForm == null)
            {
                return;
            }

            if (!this.CheckParm())
            {
                return;
            }

            setBtn.Click -= null;
            setBtn.Click += (s, e) =>
            {
                if (this.splitContainerMain.Panel2.Controls.Contains(calledForm))
                {
                    if (calledForm == this.frontForm)
                    {
                        return;
                    }

                    // calledForm.BringToFront();
                    calledForm.Visible = true;
                    if (this.frontForm != null)
                    {
                        this.frontForm.Visible = false;
                    }

                    this.frontForm = calledForm;
                }
                else
                {
                    calledForm.TopLevel = false;
                    this.splitContainerMain.Panel2.Controls.Add(calledForm);
                    calledForm.FormBorderStyle = FormBorderStyle.None;
                    calledForm.Dock = DockStyle.Fill;
                    calledForm.Show();
                    calledForm.Visible = true;

                    // calledForm.BringToFront();
                    if (this.frontForm != null)
                    {
                        this.frontForm.Visible = false;
                    }

                    this.frontForm = calledForm;
                }

                if (this.btnCurrentPage != null)
                {
                    //this.btnCurrentPage.BackColor = this.colorMenuDefault;
                }

                //setBtn.BackColor = this.colorMenuCurrentPage;
                this.btnCurrentPage = setBtn;
            };
        }

        private void SplitContainerMain_Paint(object sender, PaintEventArgs e)
        {
            if (sender is SplitContainer s)
            {
                int top = 0;
                int bottom = s.Height;
                int left = s.SplitterDistance - 2;
                Pen line = new Pen(Color.Gray)
                {
                    Width = 5,
                };
                e.Graphics.DrawLine(line, left, top, left, bottom);
            }
        }

        private void Left_btn_Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void P01_Show_ResizeEnd(object sender, EventArgs e)
        {
            this.splitContainerMain.Refresh();
        }

        private void P01_Show_FormLoaded(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSeason.Text) ||
             MyUtility.Check.Empty(this.txtStyle.Text) ||
             MyUtility.Check.Empty(this.comboBrand.Text))
            {
                this.StatusChange(Status.noStyle);
            }
            else
            {
                this.StatusChange(Status.hasStyle);
            }

            // combo Datasource
            Ict.DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, @"Select ID from Brand where Junk = 0", out System.Data.DataTable dtCountry))
            {
                this.comboBrand.DataSource = dtCountry;
                this.comboBrand.DisplayMember = "ID";
                this.comboBrand.ValueMember = "ID";
            }
            else
            {
                this.ShowErr(cbResult);
            }

            this.comboBrand.Text = "ADIDAS";
        }

        private System.Data.DataTable GetDtByComboID(string poid)
        {
            string cmd = @"select distinct id as OrderComboID from Orders
outer apply (select 1 as cnt from Orders tmp where tmp.OrderComboID = Orders.ID) cnt
WHERE Orders.ordercomboid = @POID and Orders.ordercomboid <> ''";

            DualResult res = DBProxy.Current.Select("Production", cmd, new List<SqlParameter> { new SqlParameter("@POID", poid) }, out System.Data.DataTable dt);

            if (res && dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        private void Left_btn_AD_Click(object sender, EventArgs e)
        {
            if (!this.CheckParm())
            {
                return;
            }

            string path;

            if (MyUtility.Check.Seek($@"select top 1 Path, IsDirectOpenFile from ADPath where BrandID='{this.comboBrand.Text}' and SeasonID='{this.txtSeason}' ", out DataRow dr, "ManufacturingExecution"))
            {
                path = dr["path"].ToString();
            }
            else
            {
                MyUtility.Msg.WarningBox("No Data.");
                return;
            }

            if (MyUtility.Check.Seek($"select top 1 Path, IsDirectOpenFile from ADPath where BrandID='{this.comboBrand.Text}' and SeasonID='{this.txtSeason}'", "ManufacturingExecution"))
            {
                this.OpenFile(path, this.txtStyle.Text, "AD");
            }
            else
            {
                this.OpenFile(path, "AD");
            }
        }

        private void OpenFile(string path, string filename, string type)
        {
            //if (!Directory.Exists(path))
            //{
            //    MyUtility.Msg.WarningBox("Please check the path setting.");
            //    return;
            //}

            //DirectoryInfo diInfo = new DirectoryInfo(path);

            //if (!diInfo.Exists)
            //{
            //    P01_MsgShow callmsg = new P01_MsgShow(path + $" {CodeMsg.Not_exists}", type);
            //    callmsg.ShowDialog();
            //    return;
            //}

            //// 排除excel 暫存檔(~$)
            //FileInfo[] arr;
            //if (type.EqualString("AD"))
            //{
            //    arr = this.GetADFile(diInfo, filename).ToArray();
            //}
            //else
            //{
            //    arr = diInfo.GetFiles($@"*{filename}*" + " td print.pdf").Where(file => !file.StrStartsWith("~$")).ToArray();
            //}

            //this.SortAsFile(ref arr);
            //if (arr.Length == 0)
            //{
            //    P01_MsgShow callmsg = new P01_MsgShow(CodeMsg.File_not_exists, type);
            //    callmsg.ShowDialog();
            //    return;
            //}
            //else
            //{
            //    Process.Start(arr[0].FullName);
            //}
        }

        private void OpenFile(string path, string type)
        {
            if (!Directory.Exists(path))
            {
                MyUtility.Msg.WarningBox("Please check the path setting.");
                return;
            }

            DirectoryInfo diInfo = new DirectoryInfo(path);

            if (!diInfo.Exists)
            {
                MyUtility.Msg.WarningBox("File doesn't exists.");
                return;
            }

            Process.Start(path);
        }

        // 抓出子資料夾中所有符合filename的檔案資訊
        private List<FileInfo> GetADFile(DirectoryInfo di, string filename)
        {
            List<FileInfo> listFile = new List<FileInfo>();

            foreach (DirectoryInfo sub_di in di.GetDirectories())
            {
                // Call itself to process any sub directories
                listFile.AddRange(this.GetADFile(sub_di, filename));
            }

            listFile.AddRange(di.GetFiles($@"*{filename}_*.pdf").Where(file => !file.StrStartsWith("~$")).ToList());
            return listFile;
        }

        // Sort by LastWriteTime Desc
        private void SortAsFile(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, (x, y) => { return y.LastWriteTime.CompareTo(x.LastWriteTime); });
        }

        private void P01_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = this.splitContainerMain.Panel2.Controls.Count - 1; i >= 0; i--)
            {
                var formControls = this.splitContainerMain.Panel2.Controls[i];

                if (formControls.GetType().BaseType.FullName == "Sci.Win.Forms.Base")
                {
                    ((Win.Forms.Base)formControls).Close();
                }

                if (formControls.GetType().BaseType.FullName == "Sci.Win.Tems.QueryForm")
                {
                    ((Win.Tems.QueryForm)formControls).Close();
                }
            }
        }

        private void labelTop_Click(object sender, EventArgs e)
        {

        }

        private void txtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item;
            string sqlCmd = "select ID, SeasonID, BrandID from Style where BrandID = 'ADIDAS' and LocalStyle = 0";
            item = new Win.Tools.SelectItem(sqlCmd, "16,8,10@660,400", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtStyle.Text = item.GetSelectedString();
        }

        private void txtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item;
            string sqlCmd = @"
select s.SeasonID from
(select Distinct SeasonID from Style s where BrandID = 'ADIDAS' and LocalStyle = 0) s
left join SeasonSCI  se on s.SeasonID = se.ID
order by Month desc";
            item = new Win.Tools.SelectItem(sqlCmd, "16@460,300", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            this.Init_Layout();
        }

        private bool CheckParm()
        {
            if (MyUtility.Check.Empty(this.txtStyle.Text) ||
             MyUtility.Check.Empty(this.txtSeason.Text) ||
             MyUtility.Check.Empty(this.comboBrand.Text))
            {
                MyUtility.Msg.WarningBox("Style# & Season & Brand cannot be empty.");
                return false;
            }

            return true;
        }

        private void btnSketch()
        {
            this.SetMenuClick(this.left_btn_Sketch, new P24_Sketch(this.txtStyle.Text, this.txtSeason.Text, this.comboBrand.Text));
            this.StatusChange(Status.hasStyle);
        }

        private void left_btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void left_btn_LineLayoutMachine_Click(object sender, EventArgs e)
        {
            IE.P03 callNextForm = new IE.P03(this.txtStyle.Text, this.comboBrand.Text, this.txtSeason.Text, isReadOnly: true);
            callNextForm.ShowDialog(this);
        }
    }
}