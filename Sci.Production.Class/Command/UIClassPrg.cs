﻿using Ict.Win;
using Sci.Production.Class.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci;
using Msg = Sci.MyUtility.Msg;
using System.Drawing;
using BrightIdeasSoftware;
using System.Runtime.InteropServices;
using System.Data;


namespace Sci.Production.Class
{
    public class UIClassPrg
    {

        [DllImport("user32")]
        static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);
        public enum Effect { Roll, Slide, Center, Blend }

        public static void Animate(Control ctl, Effect effect, int msec, int angle)
        {
            int flags = effmap[(int)effect];
            if (ctl.Visible) { flags |= 0x10000; angle += 180; }
            else
            {
                if (ctl.TopLevelControl == ctl) flags |= 0x20000;
                else if (effect == Effect.Blend) throw new ArgumentException();
            }
            flags |= dirmap[(angle % 360) / 45];
            bool ok = AnimateWindow(ctl.Handle, msec, flags);
            if (!ok) throw new Exception("Animation failed");
            ctl.Visible = !ctl.Visible;
        }

        private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
        private static int[] effmap = { 0, 0x40000, 0x10, 0x80000 };

        public static void SetGrid_HeaderBorderStyle(Sci.Win.UI.Grid grid, bool withAlternateRowStyle = true)
        {
            
            grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;

            grid.RowHeadersVisible = true;
            grid.RowHeadersWidth = 20;
            grid.RowTemplate.Height = 19; // 小於19 的話會導致 checkBox column 不見

            System.Windows.Forms.DataGridViewCellStyle HeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            HeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            HeaderStyle.BackColor = System.Drawing.Color.DimGray;
            HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            HeaderStyle.ForeColor = System.Drawing.Color.WhiteSmoke;
            HeaderStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            HeaderStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            HeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle = HeaderStyle;
            //grid.SelectionMode = DataGridViewSelectionMode.CellSelect;

            //Color defColor = grid.DefaultCellStyle.BackColor;
            //System.Windows.Forms.DataGridViewCellStyle alternatingRowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            //alternatingRowStyle.BackColor = Color.FromArgb(defColor.R - 10, defColor.G , defColor.B - 10);
            //alternatingRowStyle.BackColor = System.Drawing.Color.LightGray;
            //if (withAlternateRowStyle)
            //    grid.AlternatingRowsDefaultCellStyle = alternatingRowStyle;
            
        }


        private static BrightIdeasSoftware.HeaderFormatStyle headerFormatStyle1 = Get_ListViewFormatStyle1();
        private static BrightIdeasSoftware.HeaderFormatStyle Get_ListViewFormatStyle1(){
            BrightIdeasSoftware.HeaderStateStyle headerStateStyle4 = new BrightIdeasSoftware.HeaderStateStyle();
            BrightIdeasSoftware.HeaderStateStyle headerStateStyle5 = new BrightIdeasSoftware.HeaderStateStyle();
            BrightIdeasSoftware.HeaderStateStyle headerStateStyle6 = new BrightIdeasSoftware.HeaderStateStyle();
            BrightIdeasSoftware.HeaderFormatStyle headerFormatStyle1 = new BrightIdeasSoftware.HeaderFormatStyle();
            // 
            // headerFormatStyle1
            // 
            headerStateStyle4.BackColor = System.Drawing.Color.Gray;
            headerStateStyle4.ForeColor = System.Drawing.Color.White;
            headerStateStyle4.FrameColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            headerStateStyle4.FrameWidth = 2F;
            headerFormatStyle1.Hot = headerStateStyle4;
            headerStateStyle5.BackColor = System.Drawing.Color.DimGray;
            headerStateStyle5.ForeColor = System.Drawing.Color.White;
            headerFormatStyle1.Normal = headerStateStyle5;
            headerFormatStyle1.Pressed = headerStateStyle6;
            return headerFormatStyle1;
        }
        //private static HotItemStyle hotItemStyle1;
        private static HotItemStyle Get_ListViewHotItemStyle1(){
            HotItemStyle hotItemStyle1 = new BrightIdeasSoftware.HotItemStyle();
            hotItemStyle1.BackColor = System.Drawing.Color.PeachPuff;
            hotItemStyle1.ForeColor = System.Drawing.Color.MediumBlue;

            return hotItemStyle1;
        }
        public static void SetListView_Style1(ObjectListView listV)
        {
            listV.AllowColumnReorder = true;
            listV.FullRowSelect = true;
            listV.HideSelection = true;
            listV.ShowCommandMenuOnRightClick = true;
            listV.UseFilterIndicator = true;
            listV.UseFiltering = true;
            listV.UseHotItem = false;
            
            // Header Style
            listV.HeaderFormatStyle = UIClassPrg.headerFormatStyle1;
            // HotItemStyle
            //listV.HotItemStyle = UIClassPrg.hotItemStyle1;
            // AlternateRowBackColor
            Color defColor = listV.BackColor;
            System.Windows.Forms.DataGridViewCellStyle alternatingRowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            alternatingRowStyle.BackColor = Color.FromArgb(defColor.R - 10, defColor.G, defColor.B - 10);

            listV.UseAlternatingBackColors = true;
            listV.AlternateRowBackColor = alternatingRowStyle.BackColor;

            //
            listV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            listV.GridLines = true;
        }

        /// <summary>
        ///  請在 form 的Constructor執行 , 放置在 InitializeComponent() 之後跑
        /// </summary>
        /// <param name="inputForm"></param>
        public static void SetDefaultFilter_OnBrandID_ByUser(Sci.Win.Tems.Input1 inputForm)
        {
            bool getAll = AuthPrg.hasSpecialAuth("CUST ");
            if (getAll)  {
                inputForm.DefaultFilter = "";
            } else { 
                inputForm.DefaultFilter = " Brandid in (Select  BrandId from dbo.PASS3 Where Id ='" + Env.User.UserID + "')"; 
            }

        }
        public static void ColumnHeaderMouseClick_Frozen(DataGridView grid,int columnIndex){
            DataGridViewColumn gridColumn = grid.Columns[columnIndex];
            bool isFrozen_Original = gridColumn.Frozen;
            if (isFrozen_Original)
            {
                //解凍結
                grid.Columns[0].Frozen = false;
            }
            else
            {
                gridColumn.Frozen = true; ;// !isFrozen_Original;                         
            }
            Msg.WaitWindows("Column (" + gridColumn.HeaderText + ") is " + (isFrozen_Original ? "not Frozen" : "Frozen"));
        }
        public static void ColumnHeaderMouseClick_Frozen(object sender, DataGridViewCellMouseEventArgs e){
            if (e.Button == MouseButtons.Right)
            {
                DataGridView grid = (DataGridView)sender;
                DataGridViewColumn gridColumn = grid.Columns[e.ColumnIndex];
                bool isFrozen_Original = gridColumn.Frozen;
                if (isFrozen_Original)
                {
                    //解凍結
                    grid.Columns[0].Frozen = false;
                }
                else
                {
                    gridColumn.Frozen = true; ;// !isFrozen_Original;                         
                }
                Msg.WaitWindows("Column (" + gridColumn.HeaderText + ") is " + (isFrozen_Original ? "not Frozen" : "Frozen"));

            }
        }
        public static void RowHeaderMouseClick_Frozen(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView grid = (DataGridView)sender;
                DataGridViewRow gridRow = grid.Rows[e.RowIndex];
                bool isFrozen_Original = gridRow.Frozen;
                if (isFrozen_Original)
                {
                    //解凍結
                    grid.Rows[0].Frozen = false;
                }
                else
                {
                    gridRow.Frozen = true; // !isFrozen_Original;
                    gridRow.HeaderCell.Value = (e.RowIndex + 1).ToString();
                }
                Msg.WaitWindows("Row # (" + (e.RowIndex + 1).ToString() + ") is " + (isFrozen_Original ? "not Frozen" : "Frozen"));
            }
        }

        public static void SetGridFrozenFunction(DataGridView grid){
            grid.ColumnHeaderMouseClick += ColumnHeaderMouseClick_Frozen;
            grid.RowHeaderMouseClick += RowHeaderMouseClick_Frozen;
            
        }

        /// <summary>
        /// 設定當輸入數值時,自動補零傳回文字.
        /// </summary>
        /// <param name="maxlenght"></param>
        /// <returns></returns>
        public static DataGridViewGeneratorTextColumnSettings InputMaskCell(int maxlenght)
        {
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.MaxLength = (int)maxlenght;
            ts.CellFormatting += (s, e) =>
            {
                e.Value = e.Value.ToString().PadLeft(maxlenght, '0');
                e.FormattingApplied = true;
            };
            ts.InputRestrict = Ict.Win.UI.TextBoxInputsRestrict.Digit;

            return ts;
        }

        /// <summary>
        /// 當文字型態控制只允許輸入數值.
        /// </summary>
        /// <returns></returns>
        public static DataGridViewGeneratorTextColumnSettings InputMaskCell()
        {
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.InputRestrict = Ict.Win.UI.TextBoxInputsRestrict.Digit;
            return ts;
        }

        //依State寫入 Add,Edit 
        public static void  ModifyRecords(DataRow row)
        {
            if (row.RowState == DataRowState.Added)
            {
                row["AddName"] = Env.User.UserID;
                row["AddDate"] = DateTime.Now;
                row["EditName"] = "";
                row["EditDate"] = DBNull.Value;
            }
            else if (row.RowState == DataRowState.Modified)
            {
                row["EditName"] = Env.User.UserID;
                row["EditDate"] = DateTime.Now;
            }
        }

        /// <summary>
        /// 畫分隔線在 Split Container兩個Panel中間可拉動的Bar上
        /// </summary>
        /// <param name="splitCont"></param>
        public static void SetStyle_SplitContainer(SplitContainer splitCont)
        {
            splitCont.Paint += UIClassPrg.SplitContainer_Paint;
        }
        /// <summary>
        /// 畫分隔線在 Split Container兩個Panel中間可拉動的Bar上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SplitContainer_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as SplitContainer;
            //paint the three dots'
            Point[] points = new Point[5];
            var w = control.Width;
            var h = control.Height;
            var d = control.SplitterDistance;
            var sW = control.SplitterWidth;

            //calculate the position of the points'
            if (control.Orientation == Orientation.Horizontal)
            {
                points[0] = new Point((w / 2), d + (sW / 2));
                points[1] = new Point(points[0].X - 5, points[0].Y);
                points[2] = new Point(points[0].X + 5, points[0].Y);
                points[3] = new Point(points[0].X - 11, points[0].Y);
                points[4] = new Point(points[0].X + 11, points[0].Y);
            }
            else
            {
                points[0] = new Point(d + (sW / 2), (h / 2));
                points[1] = new Point(points[0].X, points[0].Y - 5);
                points[2] = new Point(points[0].X, points[0].Y + 5);
                points[3] = new Point(points[0].X, points[0].Y - 11);
                points[4] = new Point(points[0].X, points[0].Y + 11);
            }
            /*
            // 畫線
            var lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            lineShape1.BorderWidth = 2;
            lineShape1.BorderColor = System.Drawing.Color.SkyBlue;
            Rectangle rect = new Rectangle(new Point((w / 2) - 30, d + (sW / 2) - 1), new Size(66, 2));

            e.Graphics.FillRectangle(SystemBrushes.ButtonShadow, rect);
            */
            // 畫點
            foreach (Point p in points)
            {
                p.Offset(-2, -2);
                e.Graphics.FillEllipse(SystemBrushes.ControlDarkDark,
                    new Rectangle(p, new Size(4, 3)));

                /*
                p.Offset(1, 1);
                e.Graphics.FillEllipse(SystemBrushes.ControlLight,
                    new Rectangle(p, new Size(3, 3)));*/
            }
        }

        /// <summary>
        /// 設定Grid 可編輯欄位的Header 顏色
        /// </summary>
        /// <param name="dataGridViewColumn"></param>
        public static void SetGridColumn_EditableStyle(DataGridViewColumn dataGridViewColumn)
        {
            dataGridViewColumn.HeaderCell.Style.BackColor = VFPColor.Blue_30_90_230;
            dataGridViewColumn.DefaultCellStyle.BackColor = VFPColor.Blue_183_227_255;
            dataGridViewColumn.DefaultCellStyle.ForeColor = VFPColor.Red_255_0_0;
            dataGridViewColumn.DefaultCellStyle.SelectionBackColor = VFPColor.Blue_183_227_255;
            dataGridViewColumn.DefaultCellStyle.SelectionForeColor = VFPColor.Red_255_0_0;
        }
    }
}
