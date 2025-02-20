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
using static Ict.AsyncWorker;

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
        private List<int> selectedRowIndices = new List<int>();

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
        /// GridRowDrag 建構子 (建立拖曳功能)
        /// </summary>
        /// <param name="tarGrid">tarGrid</param>
        /// <param name="afterRowDragDo">afterRowDragDo</param>
        /// <param name="beforeRowDragDo">beforeRowDragDo</param>
        /// <param name="enableDragCell">enableDragCell</param>
        /// <param name="excludeDragCols">excludeDragCols</param>
        /// <param name="mutiSelect">mutiSelect</param>
        public GridRowDrag(Grid tarGrid, Action<DataRow> afterRowDragDo = null, Action<DataRow> beforeRowDragDo = null, bool enableDragCell = true, List<string> excludeDragCols = null, bool mutiSelect = false)
        {
            this.mainGrid = tarGrid;
            this.mainGrid.AllowDrop = true;
            this.backgroundScrollMainGrid.WorkerSupportsCancellation = true;
            this.mainGrid.MultiSelect = mutiSelect;

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

            this.backgroundScrollMainGrid.ProgressChanged += this.BackgroundScrollMainGrid_ProgressChanged;

            this.backgroundScrollMainGrid.WorkerReportsProgress = true;

            this.afterRowDragDo = afterRowDragDo;
            this.beforeRowDragDo = beforeRowDragDo;

            this.enableDragCell = enableDragCell;

            if (excludeDragCols != null)
            {
                this.excludeDragCols = excludeDragCols;
            }
        }

        private void BackgroundScrollMainGrid_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.mainGrid.FirstDisplayedScrollingRowIndex = e.ProgressPercentage;
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
                    newIndex = this.GetScrollIndex();
                }
                else if (this.scrollDirection == ScrollDirection.Down)
                {
                    newIndex = this.GetScrollIndex();
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

                this.backgroundScrollMainGrid.ReportProgress(newIndex);
            }
        }

        private int GetScrollIndex()
        {
            int newIndex = this.mainGrid.FirstDisplayedScrollingRowIndex;
            while (newIndex >= 0 && newIndex < this.mainGrid.RowCount)
            {
                if (this.scrollDirection == ScrollDirection.Up)
                {
                    newIndex = newIndex - 1;
                }
                else if (this.scrollDirection == ScrollDirection.Down)
                {
                    newIndex = newIndex + 1;
                }
                else
                {
                    return newIndex;
                }

                if (newIndex >= 0 && newIndex < this.mainGrid.RowCount &&
                    this.mainGrid.Rows[newIndex].Visible)
                {
                    return newIndex;
                }
            }

            return newIndex;
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
                    // 儲存目前選取的資料列索引
                    this.selectedRowIndices.Clear();
                    foreach (DataGridViewRow row in this.mainGrid.SelectedRows)
                    {
                        this.selectedRowIndices.Add(row.Index);
                    }

                    if (!this.selectedRowIndices.Contains(rowIndex))
                    {
                        this.selectedRowIndices.Add(rowIndex);
                    }

                    // 在拖曳操作開始前，恢復選取的資料列
                    this.mainGrid.BeginInvoke(new Action(() =>
                    {
                        this.mainGrid.ClearSelection();

                        // 恢復選取狀態
                        foreach (int index in this.selectedRowIndices)
                        {
                            if (index >= 0 && index < this.mainGrid.Rows.Count)
                            {
                                this.mainGrid.Rows[index].Selected = true;
                            }
                        }

                        // 開始拖曳操作
                        this.scrollDirection = ScrollDirection.None;
                        if (!this.backgroundScrollMainGrid.IsBusy)
                        {
                            this.backgroundScrollMainGrid.RunWorkerAsync();
                        }

                        List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
                        foreach (int index in this.selectedRowIndices)
                        {
                            selectedRows.Add(this.mainGrid.Rows[index]);
                        }

                        this.mainGrid.DoDragDrop(selectedRows, DragDropEffects.Move);
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
            if (e.Data.GetDataPresent(typeof(List<DataGridViewRow>)))
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
            if (e.Data.GetDataPresent(typeof(List<DataGridViewRow>)) && targetRowIndex != -1)
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
            if (e.Data.GetDataPresent(typeof(List<DataGridViewRow>)))
            {
                // 取得目標行的索引
                Point clientPoint = this.mainGrid.PointToClient(new Point(e.X, e.Y));
                int targetGridRowIndex = this.mainGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (targetGridRowIndex >= 0)
                {
                    int tarDataRowIndex = this.DataMain.Rows.IndexOf(this.mainGrid.GetDataRow(targetGridRowIndex));

                    // 取得被拖曳的資料列
                    List<DataGridViewRow> draggedRows = (List<DataGridViewRow>)e.Data.GetData(typeof(List<DataGridViewRow>));
                    List<int> draggedRowIndices = draggedRows.Select(row => this.DataMain.Rows.IndexOf(this.mainGrid.GetDataRow(row.Index))).ToList();
                    this.finalDragIndex = -1;
                    foreach (DataRow dataRow in draggedRows.OrderBy(s => s.Index).Select(s => this.mainGrid.GetDataRow(s.Index)))
                    {
                        // 更新資料綁定
                        DataRow newRow = this.MoveDataRow(this.DataMain, dataRow, tarDataRowIndex);

                        tarDataRowIndex = this.DataMain.Rows.IndexOf(newRow) + 1;

                        if (this.finalDragIndex == -1)
                        {
                            this.finalDragIndex = this.DataMain.Rows.IndexOf(newRow);
                        }

                        if (this.afterRowDragDo != null)
                        {
                            this.afterRowDragDo(newRow);
                        }
                    }
                }

                // 重置拖曳相關變數
                this.dragImage?.Dispose();
                this.dragImage = null;
                this.mainGrid.BeginInvoke(new Action(() =>
                {
                    this.mainGrid.Refresh();
                }));
            }
        }

        private DataRow MoveDataRow(DataTable dataTable, DataRow sourceRow, int targetIndex)
        {
            if (dataTable.Rows.Count < 2)
            {
                return null;
            }

            DataRow newRow = dataTable.NewRow();
            DataRowState oriRowState = sourceRow.RowState;

            newRow.ItemArray = oriRowState == DataRowState.Modified ? this.GetRowItemArrayByVersion(sourceRow, DataRowVersion.Original) : sourceRow.ItemArray;
            object[] currentRowData = sourceRow.ItemArray;

            sourceRow.Delete();

            // 插入資料列至目標位置
            dataTable.Rows.InsertAt(newRow, targetIndex);

            // 從 DataTable 中移除資料列
            dataTable.Rows.Remove(sourceRow);

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
