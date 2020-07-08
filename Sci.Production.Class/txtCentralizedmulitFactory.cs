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
    public partial class txtCentralizedmulitFactory : Sci.Win.UI.TextBox
    {
        public txtCentralizedmulitFactory()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
        }

        private Control M;

        [Category("Custom Properties")]
        public Control MObjectName
        {
            get { return this.M; }
            set { this.M = value; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            DualResult result = Result.True;
            DataTable FactoryData = new DataTable();
            FactoryData.Columns.Add("Factory", typeof(string));
            DataTable Data;

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
            if (this.M != null && !MyUtility.Check.Empty(this.M.Text))
            {
                List<string> mList = this.M.Text.Split(',').ToList();
                whereM = " where MDivisionID in ('" + string.Join("','", mList) + "')";
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
                    result = DBProxy.Current.SelectByConn(con, sqlcmd, out Data);
                    if (!result)
                    {
                        return;
                    }

                    foreach (DataRow row in Data.Rows)
                    {
                        FactoryData.ImportRow(row);
                    }
                }
            }

            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(FactoryData, "Factory", "Factory", "5", this.Text);
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
