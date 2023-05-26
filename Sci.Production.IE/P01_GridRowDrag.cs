using Ict.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01
    {
        private Image dragImage; // 截取的圖片
        private Point mouseLocation;
        private int finalDragIndex = -1;

        private void InitialGridRowDragEvent()
        {
            this.detailgrid.AllowDrop = true;

            // 處理拖曳開始事件
            this.detailgrid.MouseDown += this.DataGridView_MouseDown;

            // 處理拖曳放下事件
            this.detailgrid.DragDrop += this.DataGridView_DragDrop;

            // 處理拖曳進入事件
            this.detailgrid.DragEnter += this.DataGridView_DragEnter;

            // 設定拖曳樣式
            this.detailgrid.DragOver += this.DataGridView_DragOver;

            this.detailgrid.DragLeave += this.DataGridView_DragLeave;

            this.detailgrid.Paint += this.DataGridView_Paint;
        }

        private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.detailgrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell)
            {
                // 取得被拖曳的資料列的索引
                int rowIndex = this.detailgrid.HitTest(e.X, e.Y).RowIndex;

                // 截取拖移資料列的圖片
                using (Bitmap bitmap = new Bitmap(this.detailgrid.Width, this.detailgrid.Height))
                {
                    this.detailgrid.DrawToBitmap(bitmap, this.detailgrid.ClientRectangle);
                    Rectangle rowBounds = this.detailgrid.GetRowDisplayRectangle(rowIndex, true);
                    this.dragImage = bitmap.Clone(rowBounds, bitmap.PixelFormat);

                    this.mouseLocation = e.Location;
                }

                if (rowIndex >= 0)
                {
                    // 開始拖曳操作
                    this.detailgrid.DoDragDrop(this.detailgrid.Rows[rowIndex], DragDropEffects.Move);
                }
            }
        }

        private void DataGridView_DragEnter(object sender, DragEventArgs e)
        {
            // 確保拖曳的是資料列
            if (e.Data.GetDataPresent("Ict.Win.UI.DataGridView+Row"))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None; // 禁用非資料列的拖曳
            }
        }

        private void DataGridView_DragOver(object sender, DragEventArgs e)
        {
            // 取得滑鼠的位置
            Point clientPoint = this.detailgrid.PointToClient(new Point(e.X, e.Y));

            // 取得目標行的索引
            int targetRowIndex = this.detailgrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // 設定拖曳效果
            if (e.Data.GetDataPresent("Ict.Win.UI.DataGridView+Row") && targetRowIndex != -1)
            {
                e.Effect = DragDropEffects.Move;
                this.mouseLocation = clientPoint;
                this.detailgrid.Invalidate();
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void DataGridView_DragDrop(object sender, DragEventArgs e)
        {
            // 確保拖曳的是資料列
            if (e.Data.GetDataPresent("Ict.Win.UI.DataGridView+Row"))
            {
                // 取得目標行的索引
                Point clientPoint = this.detailgrid.PointToClient(new Point(e.X, e.Y));
                int targetRowIndex = this.detailgrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (targetRowIndex >= 0)
                {
                    // 取得被拖曳的資料列
                    DataGridViewRow draggedRow = (DataGridViewRow)e.Data.GetData("Ict.Win.UI.DataGridView+Row");

                    // 判斷被拖曳的資料列是否與目標行相同，若相同則不進行任何操作
                    if (draggedRow.Index != targetRowIndex)
                    {
                        // 更新資料綁定的資料
                        DataTable dtSource = (DataTable)this.detailgridbs.DataSource;
                        this.MoveDataRow(dtSource, draggedRow.Index, targetRowIndex);
                        this.finalDragIndex = targetRowIndex;
                    }
                }

                // 重設拖移相關變數
                this.dragImage = null;
            }
        }

        private void MoveDataRow(DataTable dataTable, int sourceIndex, int targetIndex)
        {
            if (dataTable.Rows.Count < 2)
            {
                return;
            }

            DataRow dataRow = dataTable.Rows[sourceIndex];
            DataRow newRow = dataTable.NewRow();
            newRow.ItemArray = dataRow.ItemArray;

            // 從 DataTable 中移除資料列
            dataTable.Rows.Remove(dataRow);

            // 插入資料列至目標位置
            dataTable.Rows.InsertAt(newRow, targetIndex);
        }

        private void DataGridView_DragLeave(object sender, EventArgs e)
        {
            // 清除截取的圖片
            this.dragImage?.Dispose();
            this.dragImage = null;
        }

        private void DataGridView_Paint(object sender, PaintEventArgs e)
        {
            // 繪製拖移的圖片
            if (this.dragImage != null)
            {
                e.Graphics.DrawImage(this.dragImage, this.mouseLocation);
            }

            if (this.finalDragIndex >= 0)
            {
                this.detailgrid.ClearSelection();
                this.detailgrid.Rows[this.finalDragIndex].Selected = true;
                this.finalDragIndex = -1;
            }
        }
    }
}
