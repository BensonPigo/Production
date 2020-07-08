using Ict;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtMtlLocation : Sci.Win.UI.TextBox
    {
        private string stockTypeFilte = string.Empty;

        /// <summary>
        /// 篩選 StockType
        /// </summary>
        [Category("Custom Properties")]
        [Description(
@"篩選 StockType 
EX : 只需要 Type = C, 'C'
            Type = B or C, 'B,C'")]
        public string StockTypeFilte
        {
            get { return this.stockTypeFilte; }
            set { this.stockTypeFilte = value; }
        }

        public txtMtlLocation()
        {
            this.InitializeComponent();
        }

        public txtMtlLocation(IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
        }

        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (this.Text.Empty() != true)
            {
                #region SQLParameter
                List<SqlParameter> listSQLPara = new List<SqlParameter>();
                listSQLPara.Add(new SqlParameter("@checkLocation", this.Text));
                #endregion
                #region SQL Filte
                Dictionary<string, string> dicSQLFilte = new Dictionary<string, string>();
                string strStockType = this.stockTypeFilte.Split(',').Where(row => !row.Empty()).Select(row => row = string.Format("'{0}'", row)).JoinToString(",");
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

        private void txtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            #region SQL Filte
            Dictionary<string, string> dicSQLFilte = new Dictionary<string, string>();
            string strStockType = this.stockTypeFilte.Split(',').Where(row => !row.Empty()).Select(row => row = string.Format("'{0}'", row)).JoinToString(",");
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
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(strSQLCmd, "15,5", this.Text);
            item.Size = new System.Drawing.Size(400, 530);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }
    }

    public class cellMtlLocation : Ict.Win.DataGridViewGeneratorTextColumnSettings
    {
        public static Ict.Win.DataGridViewGeneratorTextColumnSettings GetGridCell(string stockTypeFilte)
        {
            cellMtlLocation ts = new cellMtlLocation();
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
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
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
ORDER BY ID, StockType", "15,5", row["tolocation"].ToString(), false, ",");

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
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["tolocation"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

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
