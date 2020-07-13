using Ict;
using Sci.Win.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtMtlLocation
    /// </summary>
    public partial class TxtMtlLocation : Win.UI.TextBox
    {
        /// <summary>
        /// 篩選 StockType
        /// </summary>
        [Category("Custom Properties")]
        [Description(
@"篩選 StockType 
EX : 只需要 Type = C, 'C'
            Type = B or C, 'B,C'")]
        public string StockTypeFilte { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtMtlLocation"/> class.
        /// </summary>
        public TxtMtlLocation()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtMtlLocation"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public TxtMtlLocation(IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
        }

        private void TxtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (this.Text.Empty() != true)
            {
                #region SQLParameter
                List<SqlParameter> listSQLPara = new List<SqlParameter>();
                listSQLPara.Add(new SqlParameter("@checkLocation", this.Text));
                #endregion
                #region SQL Filte
                Dictionary<string, string> dicSQLFilte = new Dictionary<string, string>();
                string strStockType = this.StockTypeFilte.Split(',').Where(row => !row.Empty()).Select(row => row = string.Format("'{0}'", row)).JoinToString(",");
                dicSQLFilte.Add("StockType", strStockType.Empty() ? string.Empty : string.Format("and StockType in ({0})", strStockType));
                #endregion
                #region SQL Commend
                string strSQLCmd = string.Format(
                    @"
SELECT ID
	   , StockType 
FROM MtlLocation 
WHERE Junk = 0 
      -- StockType --
	  {0}
      and ID = @checkLocation", dicSQLFilte["StockType"]);
                #endregion
                if (!MyUtility.Check.Seek(strSQLCmd, listSQLPara))
                {
                    MyUtility.Msg.InfoBox("Data not found.");
                    e.Cancel = true;
                }
            }
        }

        private void TxtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            #region SQL Filte
            Dictionary<string, string> dicSQLFilte = new Dictionary<string, string>();
            string strStockType = this.StockTypeFilte.Split(',').Where(row => !row.Empty()).Select(row => row = string.Format("'{0}'", row)).JoinToString(",");
            dicSQLFilte.Add("StockType", strStockType.Empty() ? string.Empty : string.Format("and StockType in ({0})", strStockType));
            #endregion
            #region SQL Commend
            string strSQLCmd = string.Format(
                @"
SELECT ID
	   , StockType 
FROM MtlLocation 
WHERE Junk = 0 
      -- StockType --
	  {0}
ORDER BY ID, StockType", dicSQLFilte["StockType"]);
            #endregion
            SelectItem item = new SelectItem(strSQLCmd, "15,5", this.Text);
            item.Size = new System.Drawing.Size(400, 530);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        public class CellMtlLocation : Ict.Win.DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <param name="stockTypeFilte">stockType Filte</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static Ict.Win.DataGridViewGeneratorTextColumnSettings GetGridCell(string stockTypeFilte)
            {
                CellMtlLocation ts = new CellMtlLocation();
                string strstockTypeFilte = stockTypeFilte;

                #region SQL Filte
                Dictionary<string, string> dicSQLFilte = new Dictionary<string, string>();
                string strStockType = strstockTypeFilte.Split(',').Where(row => !row.Empty()).Select(row => row = string.Format("'{0}'", row)).JoinToString(",");
                dicSQLFilte.Add("StockType", strStockType.Empty() ? string.Empty : $@"and StockType in ({strStockType})");
                #endregion

                ts.EditingMouseDown += (s, e) =>
                {
                    // 右鍵彈出功能
                    if (e.Button == MouseButtons.Right)
                    {
                        DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                        // Parent form 若是非編輯狀態就 return
                        if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                        {
                            return;
                        }

                        DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                        SelectItem sele;
                        sele = new SelectItem(
                            $@"
SELECT ID
       , StockType
FROM MtlLocation
WHERE Junk = 0
      -- StockType--

      {dicSQLFilte["StockType"]}
ORDER BY ID, StockType", "15,5",
                            row["tolocation"].ToString(),
                            false,
                            ",");

                        DialogResult result = sele.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        e.EditingControl.Text = sele.GetSelectedString();
                        row["tolocation"] = sele.GetSelectedString();
                    }
                };

                // 正確性檢查
                ts.CellValidating += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    // 右鍵彈出功能
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row["tolocation"].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                    List<SqlParameter> listSQLPara = new List<SqlParameter>();
                    listSQLPara.Add(new SqlParameter("@checkLocation", newValue));
                    string strSQLCmd = string.Format(
                        @"
SELECT ID
	   , StockType 
FROM MtlLocation 
WHERE Junk = 0 
      -- StockType --
	  {0}
      and ID = @checkLocation", dicSQLFilte["StockType"]);
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        if (!MyUtility.Check.Seek(strSQLCmd, listSQLPara))
                        {
                            MyUtility.Msg.InfoBox("Data not found.");
                            e.Cancel = true;
                        }
                    }
                };
                return ts;
            }
        }
    }
}
