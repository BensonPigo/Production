using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using System.Configuration;
using System.Xml.Linq;
using Ict;
using System.Data.SqlClient;
using System.ComponentModel;
using Sci.Win.Tools;
using Sci.Production.Prg;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtCentralizedmulitM
    /// </summary>
    public partial class TxtCentralizedmulitM : Win.UI.TextBox
    {

        /// <summary>
        /// IsSingleSelect
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否單選")]
        public bool IsSingleSelect { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtCentralizedmulitM"/> class.
        /// </summary>
        public TxtCentralizedmulitM()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            DualResult result = Ict.Result.True;
            DataTable factoryData = new DataTable();
            factoryData.Columns.Add("M", typeof(string));
            DataTable dt;
            List<string> connectionString = CentralizedClass.AllFactoryConnectionString(excludeModules: "PMSDB_TSR");

            if (connectionString == null || connectionString.Count == 0)
            {
                MyUtility.Msg.WarningBox("no connection loaded.");
                return;
            }

            // 將所有工廠的資料合併起來
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];

                // 跨資料庫連線，將所需資料存到TempTable，再給不同資料庫使用
                SqlConnection con;
                using (con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlcmd = $@"select M=ID from MDivision ";
                    result = DBProxy.Current.SelectByConn(con, sqlcmd, out dt);
                    if (!result)
                    {
                        return;
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        factoryData.ImportRow(row);
                    }
                }
            }

            Win.Tools.BaseSelectItem item;

            if (this.IsSingleSelect)
            {
                item = new Win.Tools.SelectItem(factoryData, "M", "M", "5", this.Text);
            }
            else
            {
                item = new Win.Tools.SelectItem2(factoryData, "M", "M", "5", this.Text);
            }

            DialogResult dialogResult = item.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = this.IsSingleSelect ? ((SelectItem)item).GetSelectedString() : ((SelectItem2)item).GetSelectedString();
            this.ValidateText();
        }
    }
}
