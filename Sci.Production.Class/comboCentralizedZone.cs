using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using System.Configuration;
using System.Xml.Linq;
using Ict;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboFactory
    /// </summary>
    public partial class ComboCentralizedZone : Win.UI.ComboBox
    {
        /// <summary>
        /// ComboFactory
        /// </summary>
        public ComboCentralizedZone()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// ComboFactory
        /// </summary>
        /// <param name="container">container</param>
        public ComboCentralizedZone(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// SetDefalutIndex
        /// </summary>
        /// <param name="defalutValue">defalutValue</param>
        public void SetDefalutIndex(string defalutValue = null)
        {
            DualResult result = Ict.Result.True;
            DataTable zoneData = new DataTable();
            zoneData.Columns.Add("zone", typeof(string));
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

            // 將所有Zone資料合併起來
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];

                // 跨資料庫連線，將所需資料存到TempTable，再給不同資料庫使用
                SqlConnection con;
                using (con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlcmd = $@"select Zone=''  union all select distinct Zone from FACTORY sf where 1=1 and sf.junk=0 and sf.Zone <> '' Order by Zone";
                    result = DBProxy.Current.SelectByConn(con, sqlcmd, out dt);
                    if (!result)
                    {
                        return;
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        zoneData.ImportRow(row);
                    }
                }
            }

            var zonelist = zoneData.AsEnumerable().Select(s => new { zone = MyUtility.Convert.GetString(s["zone"]) }).Distinct().OrderBy(o => o.zone).ToList();

            // 第一筆加入空白
            this.DataSource = zonelist;
            this.ValueMember = "zone";
            this.DisplayMember = "zone";

            // this.SelectedValue = (defalutValue == null) ? "" : defalutValue;
        }
    }
}
