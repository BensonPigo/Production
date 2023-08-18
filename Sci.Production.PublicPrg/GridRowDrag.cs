using Ict;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Prg
{
    /// <summary>
    /// GridRowDrag
    /// </summary>
    public class GridRowDrag
    {
        /// <summary>
        /// 自動scroll方向
        /// </summary>
        enum ScrollDirection
        {
            None,
            Up,
            Down,
        }

        private ScrollDirection scrollDirection = ScrollDirection.None;
        private Image dragImage; // 截取的圖片
        private Point mouseLocation;
        private int finalDragIndex = -1;
        private Grid mainGrid;
        private Action<DataRow> afterRowDragDo;
        private Action<DataRow> beforeRowDragDo;
        private bool enableDragCell;
        private List<string> excludeDragCols = new List<string>();
        private BackgroundWorker backgroundScrollMainGrid = new BackgroundWorker();

        private DataTable DataMain
        {
            get
            {
                if (this.mainGrid.DataSource.GetType() == typeof(ListControlBindingSource))
                {
                    object bsDataSource = ((ListControlBindingSource)this.mainGrid.DataSource).DataSource;

                    return (DataTable)bsDataSource;
                }
                else
                {
                    return (DataTable)this.mainGrid.DataSource;
                }
            }
        }

        /// <summary>
        /// GridRowDrag
        /// </summary>
        /// <param name="tarGrid">tarGrid</param>
        /// <param name="afterRowDragDo">afterRowDragDo</param>
        /// <param name="beforeRowDragDo">beforeRowDragDo</param>
        /// <param name="enableDragCell">enableDragCell</param>
        /// <param name="excludeDragCols">excludeDragCols</param>
        public GridRowDrag(Grid tarGrid, Action<DataRow> afterRowDragDo = null, Action<DataRow> beforeRowDragDo = null, bool enableDragCell = true, List<string> excludeDragCols = null)
        {
            this.mainGrid = tarGrid;
            this.mainGrid.AllowDrop = true;
            this.backgroundScrollMainGrid.WorkerSupportsCancellation = true;
            // 處理拖曳開始事件
            this.mainGrid.MouseDown += this.DataGridView_MouseDown;

            // 處理拖曳放下事件
            this.mainGrid.DragDrop += this.DataGridView_DragDrop;

            // 處理拖曳進入事件
            this.mainGrid.DragEnter += this.DataGridView_DragEnter;

            // 設定拖曳樣式
            this.mainGrid.DragOver += this.DataGridView_DragOver;

            this.mainGrid.DragLeave += this.DataGridView_DragLeave;

            this.mainGrid.Paint += this.DataGridView_Paint;

            this.backgroundScrollMainGrid.DoWork += this.BackgroundScrollMainGrid_DoWork;

            this.afterRowDragDo = afterRowDragDo;
            this.beforeRowDragDo = beforeRowDragDo;

            this.enableDragCell = enableDragCell;

            if (excludeDragCols != null)
            {
                this.excludeDragCols = excludeDragCols;
            }
        }

        private void BackgroundScrollMainGrid_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                Thread.Sleep(80);

                if (worker.CancellationPending)
                {
                    return;
                }

                int newIndex = 0;
                if (this.scrollDirection == ScrollDirection.Up)
                {
                    newIndex = this.mainGrid.FirstDisplayedScrollingRowIndex - 1;
                }
                else if (this.scrollDirection == ScrollDirection.Down)
                {
                    newIndex = this.mainGrid.FirstDisplayedScrollingRowIndex + 1;
                }
                else
                {
                    continue;
                }

                if ((newIndex < 0 && this.scrollDirection == ScrollDirection.Up) || 
                    (this.IsGridScrolledToBottom(this.mainGrid) && this.scrollDirection == ScrollDirection.Down))
                {
                    continue;
                }

                this.mainGrid.BeginInvoke(new Action(() =>
                {
                    this.mainGrid.FirstDisplayedScrollingRowIndex = newIndex;
                }));
            }
        }

        private bool IsGridScrolledToBottom(DataGridView grid)
        {
            if (grid.Rows.Count == 0)
            {
                return false;
            }

            int lastVisibleRowIndex = grid.DisplayedRowCount(false) + grid.FirstDisplayedScrollingRowIndex - 1;
            return lastVisibleRowIndex >= grid.Rows.Count - 1;
        }

        private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left &&
                ((this.mainGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell && this.enableDragCell) ||
                 this.mainGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.RowHeader) &&
                this.mainGrid.AllowDrop)
            {
                int colIdex = this.mainGrid.HitTest(e.X, e.Y).ColumnIndex;
                if (colIdex >= 0 &&
                    this.excludeDragCols.Contains(this.mainGrid.Columns[colIdex].DataPropertyName))
                {
                    return;
                }

                // 取得被拖曳的資料列的索引
                int rowIndex = this.mainGrid.HitTest(e.X, e.Y).RowIndex;

                // 截取拖移資料列的圖片
                using (Bitmap bitmap = new Bitmap(this.mainGrid.Width, this.mainGrid.Height))
                {
                    this.mainGrid.DrawToBitmap(bitmap, this.mainGrid.ClientRectangle);
                    Rectangle rowBounds = this.mainGrid.GetRowDisplayRectangle(rowIndex, true);
                    this.dragImage = bitmap.Clone(rowBounds, bitmap.PixelFormat);

                    // 縮小圖片
                    int newWidth = MyUtility.Convert.GetInt(this.dragImage.Width / 1.2);  // 設定新的寬度
                    int newHeight = MyUtility.Convert.GetInt(this.dragImage.Height / 1.2);  // 設定新的高度
                    this.dragImage = new Bitmap(this.dragImage, newWidth, newHeight);

                    this.mouseLocation = e.Location;
                }

                if (rowIndex >= 0)
                {
                    // 開始拖曳操作
                    this.mainGrid.BeginInvoke(new Action(() =>
                    {
                        this.scrollDirection = ScrollDirection.None;
                        if (!this.backgroundScrollMainGrid.IsBusy)
                        {
                            this.backgroundScrollMainGrid.RunWorkerAsync();
                        }

                        this.mainGrid.DoDragDrop(this.mainGrid.Rows[rowIndex], DragDropEffects.Move);
                        this.scrollDirection = ScrollDirection.None;
                        if (this.backgroundScrollMainGrid.IsBusy)
                        {
                            this.backgroundScrollMainGrid.CancelAsync();
                        }

                        this.dragImage?.Dispose();
                        this.dragImage = null;
                        this.mainGrid.Refresh();
                    }));
                }
            }
        }

        private void DataGridView_DragEnter(object sender, DragEventArgs e)
        {
            this.scrollDirection = ScrollDirection.None;

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
            Point clientPoint = this.mainGrid.PointToClient(new Point(e.X, e.Y));

            // 取得目標行的索引
            int targetRowIndex = this.mainGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // 設定拖曳效果
            if (e.Data.GetDataPresent("Ict.Win.UI.DataGridView+Row") && targetRowIndex != -1)
            {
                e.Effect = DragDropEffects.Move;
                this.mouseLocation = clientPoint;
                this.mainGrid.Invalidate();
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
                Point clientPoint = this.mainGrid.PointToClient(new Point(e.X, e.Y));
                int targetGridRowIndex = this.mainGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (targetGridRowIndex >= 0)
                {
                    int tarDataRowIndex = this.DataMain.Rows.IndexOf(this.mainGrid.GetDataRow(targetGridRowIndex));

                    // 取得被拖曳的資料列
                    DataGridViewRow draggedRow = (DataGridViewRow)e.Data.GetData("Ict.Win.UI.DataGridView+Row");
                    int draggedRowIndex = this.DataMain.Rows.IndexOf(this.mainGrid.GetDataRow(draggedRow.Index));

                    // 判斷被拖曳的資料列是否與目標行相同，若相同則不進行任何操作
                    if (draggedRowIndex != tarDataRowIndex)
                    {
                        if (this.beforeRowDragDo != null)
                        {
                            this.beforeRowDragDo(this.mainGrid.GetDataRow(draggedRow.Index));
                        }

                        // 更新資料綁定的資料
                        DataRow newRow = this.MoveDataRow(this.DataMain, draggedRowIndex, tarDataRowIndex);
                        this.finalDragIndex = targetGridRowIndex;
                        if (this.afterRowDragDo != null)
                        {
                            this.afterRowDragDo(newRow);
                        }
                    }
                }

                // 重設拖移相關變數
                this.dragImage?.Dispose();
                this.dragImage = null;
                this.mainGrid.BeginInvoke(new Action(() =>
                {
                    this.mainGrid.Refresh();
                }));
            }
        }

        private DataRow MoveDataRow(DataTable dataTable, int sourceIndex, int targetIndex)
        {
            if (dataTable.Rows.Count < 2)
            {
                return null;
            }

            DataRow dataRow = dataTable.Rows[sourceIndex];
            DataRow newRow = dataTable.NewRow();
            DataRowState oriRowState = dataRow.RowState;

            newRow.ItemArray = oriRowState == DataRowState.Modified ? this.GetRowItemArrayByVersion(dataRow, DataRowVersion.Original) : dataRow.ItemArray;
            object[] currentRowData = dataRow.ItemArray;

            // 從 DataTable 中移除資料列
            dataTable.Rows.Remove(dataRow);

            // 插入資料列至目標位置
            dataTable.Rows.InsertAt(newRow, targetIndex);
            if (oriRowState == DataRowState.Unchanged)
            {
                newRow.AcceptChanges();
            }
            else if (oriRowState == DataRowState.Modified)
            {
                newRow.AcceptChanges();
                newRow.ItemArray = currentRowData;
            }

            return newRow;
        }

        private object[] GetRowItemArrayByVersion(DataRow srcRow, DataRowVersion dataRowVersion)
        {
            object[] resultItemArray = new object[srcRow.Table.Columns.Count];
            int colIndex = 0;
            foreach (DataColumn col in srcRow.Table.Columns)
            {
                resultItemArray[colIndex] = srcRow[col.ColumnName, dataRowVersion];
                colIndex++;
            }

            return resultItemArray;
        }

        private void DataGridView_DragLeave(object sender, EventArgs e)
        {
            Point mousePosition = this.mainGrid.PointToClient(Cursor.Position);
            Rectangle gridBounds = this.mainGrid.Bounds;

            if (mousePosition.Y < gridBounds.Top + 50)
            {
                this.scrollDirection = ScrollDirection.Up;
            }
            else if (mousePosition.Y > gridBounds.Bottom - 50)
            {
                this.scrollDirection = ScrollDirection.Down;
            }

            if (!this.backgroundScrollMainGrid.IsBusy)
            {
                this.backgroundScrollMainGrid.RunWorkerAsync();
            }
        }

        private void DataGridView_Paint(object sender, PaintEventArgs e)
        {
            // 繪製拖移的圖片
            if (this.dragImage != null && this.scrollDirection == ScrollDirection.None)
            {
                e.Graphics.DrawImage(this.dragImage, this.mouseLocation);
            }

            if (this.finalDragIndex >= 0)
            {
                this.mainGrid.ClearSelection();
                this.mainGrid.SelectRowTo(this.finalDragIndex);
                this.finalDragIndex = -1;
            }
        }
    }
}
