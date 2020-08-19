using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Sewing_P11 : Win.Tems.QueryForm
    {
        private string Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sewing_P11"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public Sewing_P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtCentralizedmulitM1.Text = Env.User.Keyword;
            this.txtCentralizedmulitFactory1.Text = Env.User.Factory;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btnlock_Click(object sender, EventArgs e)
        {
            string m = this.txtCentralizedmulitM1.Text;
            string fty = this.txtCentralizedmulitFactory1.Text;
            if (MyUtility.Check.Empty(m) && MyUtility.Check.Empty(fty))
            {
                MyUtility.Msg.WarningBox("Please choose <M> or <Factory>");
                return;
            }

            if (MyUtility.Check.Empty(this.dateLock.Value))
            {
                MyUtility.Msg.WarningBox($"Please set <{this.Type} Date >");
                return;
            }

            DateTime date = (DateTime)this.dateLock.Value;

            if (((DateTime)this.dateLock.Value).MonthGreaterThan(DateTime.Now))
            {
                MyUtility.Msg.WarningBox($"Set <{this.Type} Date> before {DateTime.Now.ToString("yyyy/MM")}");
                return;
            }

            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            List<string> strSevers = new List<string>();
            if (DBProxy.Current.DefaultModuleName.Contains("PMSDB"))
            {
                strSevers = ConfigurationManager.AppSettings["PMSDBServer"].Split(',').ToList();
                strSevers.Remove("PMSDB_TSR");
                strSevers.Remove("PMSDB_NAI");
            }
            else
            {
                strSevers = ConfigurationManager.AppSettings["TestingServer"].Split(',').ToList();
                strSevers.Remove("testing_TSR");
                strSevers.Remove("testing_NAI");
            }

            DataTable ftyServerDatas = new DataTable();
            ftyServerDatas.Columns.Add("Factory", typeof(string));
            ftyServerDatas.Columns.Add("nowConnection", typeof(string));

            if (MyUtility.Check.Empty(fty))
            {
                foreach (string ss in strSevers)
                {
                    var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss)).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();

                    string whereM = string.Empty;
                    List<string> mList = this.txtCentralizedmulitM1.Text.Split(',').ToList();
                    whereM = " where MDivisionID in ('" + string.Join("','", mList) + "')";
                    DualResult result = Ict.Result.True;
                    SqlConnection con;
                    using (con = new SqlConnection(connections))
                    {
                        con.Open();
                        string sqlcmd = $@"select distinct Factory=FTYGroup,nowConnection='{ss}' from Factory WITH (NOLOCK) {whereM} order by Factory";
                        result = DBProxy.Current.SelectByConn(con, sqlcmd, out DataTable data);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return;
                        }

                        foreach (DataRow row in data.Rows)
                        {
                            ftyServerDatas.ImportRow(row);
                        }
                    }
                }
            }
            else
            {
                foreach (string ss in strSevers)
                {
                    var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss)).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();

                    string whereF = string.Empty;
                    List<string> fList = this.txtCentralizedmulitFactory1.Text.Split(',').ToList();
                    whereF = " where FtyGroup in ('" + string.Join("','", fList) + "')";
                    DualResult result = Ict.Result.True;
                    SqlConnection con;
                    using (con = new SqlConnection(connections))
                    {
                        con.Open();
                        string sqlcmd = $@"select distinct Factory=FTYGroup,nowConnection='{ss}' from Factory WITH (NOLOCK) {whereF} order by Factory";
                        result = DBProxy.Current.SelectByConn(con, sqlcmd, out DataTable data);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return;
                        }

                        foreach (DataRow row in data.Rows)
                        {
                            ftyServerDatas.ImportRow(row);
                        }
                    }
                }
            }

            this.msgList.Clear();
            if (this.rdbtnLock.Checked)
            {
                // call [httppost]: Website:16888/api/Sewing/LockSewingMonthly
                foreach (DataRow row in ftyServerDatas.Rows)
                {
                    string nowConnection = MyUtility.Convert.GetString(row["nowConnection"]); // EX:testing_PH1
                    string factory = MyUtility.Convert.GetString(row["factory"]);
                    this.APILock(factory, date, Env.User.UserID, "LockSewingMonthly", nowConnection);
                }
            }
            else if (this.rdbtnUnlock.Checked)
            {
                // call [httppost]: Website:16888/api/Sewing/UnlockSewingMonthly
                foreach (DataRow row in ftyServerDatas.Rows)
                {
                    string nowConnection = MyUtility.Convert.GetString(row["nowConnection"]); // EX:testing_PH1
                    string factory = MyUtility.Convert.GetString(row["factory"]);
                    this.APILock(factory, date, Env.User.UserID, "UnlockSewingMonthly", nowConnection);
                }
            }

            if (this.msgList.Count > 0)
            {
                MyUtility.Msg.WarningBox(string.Join("\r\n", this.msgList));
            }
        }

        /// <summary>
        /// API Data
        /// </summary>
        public class APIData
        {
            /// <summary>
            /// API Message
            /// </summary>
            public string Message { get; set; }
        }

        private void APILock(string factory, DateTime lockDate, string userID, string api, string nowConnection)
        {
            try
            {
                string apiParemeter = string.Empty;
                apiParemeter = $"?Factory={factory}&LockDate={lockDate.ToString("yyyy/MM/dd")}&UserID={userID}";

                XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
                string connections = docx.Descendants("modules").Elements()
                    .Where(y => y.FirstAttribute.Value.EqualString(nowConnection)).Descendants("connectionStrings").Elements()
                    .Where(x => x.FirstAttribute.Value.Contains("PMSSewingAPIuri")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(connections + api + apiParemeter, null).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                var jason = JsonConvert.DeserializeObject<APIData>(responseBody);
                this.msgList.Add(factory + ":" + jason.Message);
            }
            catch (Exception ex)
            {
                this.msgList.Add(factory + ":" + ex.Message);
                return;
            }
        }

        private List<string> msgList = new List<string>();

        private void RdbtnLock_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbtnLock.Checked)
            {
                this.btnlock.Text = "Lock";
                this.lbDate.Text = "Lock Date";
                this.Type = "Lock";
            }
            else if (this.rdbtnUnlock.Checked)
            {
                this.btnlock.Text = "Unlock";
                this.lbDate.Text = "Unlock Date";
                this.Type = "Unlock";
            }
        }

        private void TxtCentralizedmulitM1_TextChanged(object sender, EventArgs e)
        {
            this.txtCentralizedmulitFactory1.Text = string.Empty;
        }
    }
}
