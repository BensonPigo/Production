using Ict;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// AutoLineMappingGridSyncScroll
    /// </summary>
    internal class AutoLineMappingGridSyncScroll
    {
        private Grid gridMain;
        private Grid gridSub;
        private string syncColName;

        /// <summary>
        /// GridSyncScroll
        /// </summary>
        /// <param name="gridMain">gridMain</param>
        /// <param name="gridSub">gridSub</param>
        /// <param name="syncColName">syncColName</param>
        public AutoLineMappingGridSyncScroll(Grid gridMain, Grid gridSub, string syncColName)
        {
            this.gridMain = gridMain;
            this.gridSub = gridSub;
            this.syncColName = syncColName;

            this.gridMain.Scroll += this.GridMain_Scroll;
            this.gridSub.Scroll += this.GridSub_Scroll;
        }

        private void GridSub_Scroll(object sender, ScrollEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;

            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll)
            {
                return;
            }

            //if (this.tabDetail.SelectedIndex != 0)
            //{
            //    return;
            //}

            string scrollNo = sourceGrid.Rows[sourceGrid.FirstDisplayedScrollingRowIndex].Cells[this.syncColName].Value.ToString();
            this.ScrollLineMapping(scrollNo);
        }

        private void GridMain_Scroll(object sender, ScrollEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;

            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll)
            {
                return;
            }

            //if (this.tabDetail.SelectedIndex != 0)
            //{
            //    return;
            //}

            bool isScrollDown = e.NewValue > e.OldValue;

            string scrollNo = sourceGrid.Rows[sourceGrid.FirstDisplayedScrollingRowIndex].Cells[this.syncColName].Value.ToString();
            string oldScrollNo = sourceGrid.Rows[e.OldValue].Cells[this.syncColName].Value.ToString();

            if (isScrollDown && scrollNo == oldScrollNo)
            {
                scrollNo = (MyUtility.Convert.GetInt(scrollNo) + 1).ToString().PadLeft(2, '0');
            }

            this.ScrollLineMapping(scrollNo);
        }

        private void ScrollLineMapping(string scrollToNo)
        {

            DataTable dtMain = this.gridMain.DataSource.GetType() == typeof(ListControlBindingSource) ? (DataTable)((ListControlBindingSource)this.gridMain.DataSource).DataSource : (DataTable)this.gridMain.DataSource;
            DataTable dtSub = this.gridSub.DataSource.GetType() == typeof(ListControlBindingSource) ? (DataTable)((ListControlBindingSource)this.gridSub.DataSource).DataSource : (DataTable)this.gridSub.DataSource;

            this.gridMain.FirstDisplayedScrollingRowIndex = this.gridMain.GetRowIndexByDataRow(dtMain.AsEnumerable().Where(s => s[this.syncColName].ToString() == scrollToNo).First());
            this.gridSub.FirstDisplayedScrollingRowIndex = this.gridSub.GetRowIndexByDataRow(dtSub.AsEnumerable().Where(s => s[this.syncColName].ToString() == scrollToNo).First());
        }

    }
}
