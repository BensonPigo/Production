using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using System.Configuration;
using System.Xml.Linq;
using Ict;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtCentralizedmulitFactory
    /// </summary>
    public partial class TxtCentralizedmulitFactory : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtCentralizedmulitFactory"/> class.
        /// </summary>
        public TxtCentralizedmulitFactory()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        /// <summary>
        /// IsProduceFty
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否ProduceFty")]
        public bool IsProduceFty { get; set; } = false;

        /// <summary>
        /// IsJunk
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否Junk")]
        public bool IsJunk { get; set; } = false;

        /// <summary>
        /// Is Add Condition Junk
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否增加Junk條件")]
        public bool IsAddConditionJunk { get; set; } = false;

        /// <summary>
        /// MDivision ID
        /// </summary>
        [Category("Custom Properties")]
        public Control MObjectName { get; set; }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            DualResult result = Ict.Result.True;
            DataTable factoryData = new DataTable();
            factoryData.Columns.Add("Factory", typeof(string));
            DataTable dt;

            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            List<string> strSevers = ConfigurationManager.AppSettings["PMSDBServer"].Split(',').ToList();
            strSevers.Remove("PMSDB_TSR");
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss)).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                MyUtility.Msg.WarningBox("no connection loaded.");
                return;
            }

            string whereM = string.Empty;
            if (this.MObjectName != null && !MyUtility.Check.Empty(this.MObjectName.Text))
            {
                List<string> mList = this.MObjectName.Text.Split(',').ToList();
                whereM = " where MDivisionID in ('" + string.Join("','", mList) + "')";
            }

            if (this.IsProduceFty)
            {
                whereM = whereM.Empty() ? "where IsProduceFty = 1 " : whereM + " and IsProduceFty = 1 ";
            }

            if (this.IsAddConditionJunk)
            {
                if (this.IsJunk)
                {
                    whereM += " and Junk = 1 ";
                }
                else
                {
                    whereM += " and Junk = 0 ";
                }
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
                    string sqlcmd = $@"select distinct Factory=FTYGroup from Factory WITH (NOLOCK) {whereM} order by Factory";
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

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(factoryData, "Factory", "Factory", "5", this.Text);
            DialogResult dialogResult = item.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }
    }
}
