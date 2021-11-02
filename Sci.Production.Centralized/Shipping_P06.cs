using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static PmsWebApiUtility20.WebApiTool;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Shipping_P06
    /// </summary>
    public partial class Shipping_P06 : Win.Tems.QueryForm
    {
        /// <summary>
        /// Shipping_P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Shipping_P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.txtShippingReason.Type = "PL";
            this.txtShippingReason.LinkDB = "ProductionTPE";
            this.comboRegion.ValueMember = "Key";
            this.comboRegion.DisplayMember = "Value";
            this.comboRegion.DataSource = new BindingSource(StaticEntity.LoginRegionList, null);
            this.EditMode = true;
        }

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.comboRegion.Text))
            {
                MyUtility.Msg.WarningBox("Please fill <Region>");
                return;
            }

            if (MyUtility.Check.Empty(this.txtPulloutID.Text))
            {
                MyUtility.Msg.WarningBox("Please fill <PullOut ID>");
                return;
            }

            if (MyUtility.Check.Empty(this.txtShippingReason.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("Please fill <Reason>");
                return;
            }

            WebApiBaseResult webApiBaseResult;
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add("PulloutID", this.txtPulloutID.Text);

            string apiUrl = this.APIUrl;

            if (MyUtility.Check.Empty(apiUrl))
            {
                MyUtility.Msg.WarningBox($"<Region>{this.comboRegion.Text} WebAPI Url not found, Please notify MIS");
                return;
            }

            string sqlInsertShippingHistory = $@"
insert into ShippingHistory(ID, MDivisionID, Type, ReasonTypeID, ReasonID, Remark, AddName, AddDate)
            values(@ID, @MDivisionID, 'PullOutUnlock', 'PL', @ReasonID, @Remark, '{Env.User.UserID}', GetDate())
";
            List<SqlParameter> sqlPar = new List<SqlParameter>();
            sqlPar.Add(new SqlParameter("@ID", this.txtPulloutID.Text));
            sqlPar.Add(new SqlParameter("@MDivisionID", this.comboRegion.Text));
            sqlPar.Add(new SqlParameter("@ReasonID", this.txtShippingReason.TextBox1.Text));
            sqlPar.Add(new SqlParameter("@Remark", this.txtRemark.Text));

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result = DBProxy.Current.Execute(this.LinkDB, sqlInsertShippingHistory, sqlPar);

                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, "api/Shipping/PulloutUnlock", string.Empty, 300, requestHeaders);
                    if (!webApiBaseResult.isSuccess)
                    {
                        transactionScope.Dispose();
                        if (webApiBaseResult.webApiResponseStatus == WebApiResponseStatus.WebApiReturnFail)
                        {
                            MyUtility.Msg.WarningBox(webApiBaseResult.responseContent);
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(webApiBaseResult.exception.ToString());
                        }

                        return;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Successfully unlocked!!");
        }

        private string APIUrl
        {
            get
            {
                string getUrlSql = $"select URL from SystemWebAPIURL with (nolock) where SystemName = '{this.comboRegion.Text.Replace("PH1", "PHI")}' and Environment = {"'{0}'"} and junk = 0";
                string resultUrl = string.Empty;
                if (DBProxy.Current.DefaultModuleName.Contains("PMSDB"))
                {
                    resultUrl = MyUtility.GetValue.Lookup(string.Format(getUrlSql, "Formal"));
                }
                else
                {
                    resultUrl = MyUtility.GetValue.Lookup(string.Format(getUrlSql, "Testing"));
                }

#if DEBUG
                resultUrl = MyUtility.GetValue.Lookup(string.Format(getUrlSql, "Testing"));
#endif

                return resultUrl;
            }
        }

        private string LinkDB
        {
            get
            {
                string linkDB = "ProductionTPE";

#if DEBUG
                linkDB = "Production";
#endif
                return linkDB;
            }
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            new Shipping_P06_History().ShowDialog();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
