using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtMtlLocation : Sci.Win.UI.TextBox
    {
        private string stockTypeFilte = "";

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
            set { this.stockTypeFilte = value; }
            get { return this.stockTypeFilte; }
        }

        public txtMtlLocation()
        {
            InitializeComponent();
        }

        public txtMtlLocation(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
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
                string strStockType = stockTypeFilte.Split(',').Where(row => !row.Empty()).Select(row => row = string.Format("'{0}'", row)).JoinToString(",");
                dicSQLFilte.Add("StockType", (strStockType.Empty()) ? "" : string.Format("and StockType in ({0})", strStockType));
                #endregion
                #region SQL Commend
                string strSQLCmd = string.Format(@"
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
            string strStockType = stockTypeFilte.Split(',').Where(row => !row.Empty()).Select(row => row = string.Format("'{0}'", row)).JoinToString(",");
            dicSQLFilte.Add("StockType", (strStockType.Empty()) ? "" : string.Format("and StockType in ({0})", strStockType));
            #endregion
            #region SQL Commend
            string strSQLCmd = string.Format(@"
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
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
