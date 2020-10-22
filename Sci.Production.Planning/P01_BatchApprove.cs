using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Planning
{
    /// <summary>
    /// P01_BatchApprove
    /// </summary>
    public partial class P01_BatchApprove : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// P01_BatchApprove
        /// </summary>
        public P01_BatchApprove()
        {
            this.InitializeComponent();
            this.dateApproveDate.Enabled = false;
            this.btnUnApprove.Enabled = false;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchApprove.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchApprove.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchApprove)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("factoryid", header: "Factory", iseditingreadonly: true)
            .Text("id", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Date("scidelivery", header: "SCI Delivery", iseditingreadonly: true)
            .Text("artworktypeid", header: "Artwork Type", iseditingreadonly: true)
            .Text("localsuppid", header: "Supplier", iseditingreadonly: true)
            .Text("SupplierName", header: "Supplier Name", iseditingreadonly: true)
            .Date("ArtworkInLine", header: "Sub Process Inline", iseditingreadonly: true)
            .DateTime("ApvDate", header: "Approve Date", iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMdd)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.QueryData(true, true);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckOnlyAprrovedData_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                this.dateApproveDate.Enabled = true;
                this.btnApprove.Enabled = false;
                this.btnUnApprove.Enabled = true;
                this.dateApproveDate.TextBox1.Value = DateTime.Now;
                this.dateApproveDate.TextBox2.Value = DateTime.Now;
            }
            else
            {
                this.dateApproveDate.Enabled = false;
                this.dateApproveDate.TextBox1.Value = null;
                this.dateApproveDate.TextBox2.Value = null;
                this.btnApprove.Enabled = true;
                this.btnUnApprove.Enabled = false;
            }
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridBatchApprove.ValidateControl();

            DataTable dtImport = (DataTable)this.listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtImport.Select("Selected = 1 ");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to Approve data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            IList<SqlCommandText> updateCmds = new List<SqlCommandText>();

            for (int i = 0; i < dr2.Length; i++)
            {
                SqlCommandText tmp = new SqlCommandText(string.Format(@"update order_tmscost set apvdate = getdate() , apvname = '{0}'  where id = '{1}' and artworktypeid = '{2}';", Env.User.UserID, dr2[i]["id"], dr2[i]["artworktypeid"]), null);
                updateCmds.Add(tmp);
            }

            DualResult result;
            if (!(result = DBProxy.Current.Executes(null, updateCmds)))
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Approve data successful.");
            this.QueryData(false, false);
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            MyUtility.Excel.CopyToXls(dt, string.Empty, "Planning_P01_BatchApprove.xltx", 1);
        }

        private void BtnUnApprove_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridBatchApprove.ValidateControl();

            DataTable dtImport = (DataTable)this.listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtImport.Select("Selected = 1 ");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to UnApprove data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            IList<SqlCommandText> updateCmds = new List<SqlCommandText>();

            for (int i = 0; i < dr2.Length; i++)
            {
                SqlCommandText tmp = new SqlCommandText(string.Format(@"update order_tmscost set apvdate = null , apvname = '' where id = '{1}' and artworktypeid = '{2}';", Env.User.UserID, dr2[i]["id"], dr2[i]["artworktypeid"]), null);
                updateCmds.Add(tmp);
            }

            DualResult result;
            if (!(result = DBProxy.Current.Executes(null, updateCmds)))
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("UnApprove data successful.");
            this.QueryData(false, false);
        }

        private void QueryData(bool checkSelect, bool ap)
        {
            string sp_b = this.txtSPNoStart.Text;
            string sp_e = this.txtSPNoEnd.Text;
            string factory = this.txtfactory.Text;
            string artworktype = this.txtartworktype_ftyArtworkType.Text;
            bool chkApprove = this.checkOnlyAprrovedData.Checked;

            string inline_b, inline_e, sewinline_b, sewinline_e, delivery_b, delivery_e, approve_b, approve_e;
            inline_b = null;
            inline_e = null;
            sewinline_b = null;
            sewinline_e = null;
            delivery_b = null;
            delivery_e = null;
            approve_b = null;
            approve_e = null;

            if (this.dateSubprocessInline.Value1 != null)
            {
                inline_b = this.dateSubprocessInline.Text1;
            }

            if (this.dateSubprocessInline.Value2 != null)
            {
                inline_e = this.dateSubprocessInline.Text2;
            }

            if (this.dateRangeSewInLine.Value1 != null)
            {
                sewinline_b = this.dateRangeSewInLine.Text1;
            }

            if (this.dateRangeSewInLine.Value2 != null)
            {
                sewinline_e = this.dateRangeSewInLine.Text2;
            }

            if (this.dateSCIDelivery.Value1 != null)
            {
                delivery_b = this.dateSCIDelivery.Text1;
            }

            if (this.dateSCIDelivery.Value2 != null)
            {
                delivery_e = this.dateSCIDelivery.Text2;
            }

            if (this.dateApproveDate.Value1 != null)
            {
                approve_b = this.dateApproveDate.Text1;
            }

            if (this.dateApproveDate.Value2 != null)
            {
                approve_e = this.dateApproveDate.Text2;
            }

            if (chkApprove && (MyUtility.Check.Empty(approve_b) || MyUtility.Check.Empty(approve_e)))
            {
                MyUtility.Msg.WarningBox("Approve date can't be empty", "Warning");
                return;
            }

            if ((MyUtility.Check.Empty(inline_b) && MyUtility.Check.Empty(inline_e)) &&
                (MyUtility.Check.Empty(sewinline_b) && MyUtility.Check.Empty(sewinline_e)) &&
                (MyUtility.Check.Empty(delivery_b) && MyUtility.Check.Empty(delivery_e)) &&
                MyUtility.Check.Empty(sp_b) && MyUtility.Check.Empty(sp_e) &&
                MyUtility.Check.Empty(artworktype))
            {
                this.txtSPNoStart.Focus();
                MyUtility.Msg.WarningBox("< Inline Date > or < SewInline Date > or < SCI Delivery > or < SP# > or < Artwork Type > can't be empty!!");
                return;
            }
            else
            {
                string strSQLCmd = string.Format(
                    @"
select  {0} as Selected
        ,ods.FactoryID
        ,ot.ID
        ,ot.ArtworkTypeID
        ,ot.LocalSuppID
        ,ls.Abb SupplierName
        ,ot.ArtworkInLine
        ,ods.SciDelivery
        ,ot.ApvDate
from Order_TmsCost ot WITH (NOLOCK)
inner join orders ods WITH (NOLOCK) on ot.ID = ods.ID
inner join LocalSupp ls WITH (NOLOCK) on ls.id = ot.LocalSuppID
inner join dbo.factory WITH (NOLOCK) on factory.id = ods.factoryid
where ods.finished=0 and ods.isforecast = 0 and factory.IsProduceFty = 1
and (ods.category = 'B' or ods.category = 'S')
and ods.qty > 0 and (ot.qty > 0 or ot.tms > 0) ",
                    checkSelect ? 1 : 0);
                if (!chkApprove)
                {
                    strSQLCmd += " and  ot.apvdate is null";
                }

                if (!MyUtility.Check.Empty(sp_b))
                {
                    strSQLCmd += " and  ot.id  between @sp1 and  @sp2";
                }

                if (!MyUtility.Check.Empty(inline_b))
                {
                    strSQLCmd += string.Format(" and ot.artworkoffline >= '{0}'", Convert.ToDateTime(inline_b).ToString("d"));
                }

                if (!MyUtility.Check.Empty(inline_e))
                {
                    strSQLCmd += string.Format(" and ot.artworkinline <= '{0}'", Convert.ToDateTime(inline_e).ToString("d"));
                }

                if (!MyUtility.Check.Empty(sewinline_b))
                {
                    strSQLCmd += string.Format(" and ods.sewoffline >= '{0}'", Convert.ToDateTime(sewinline_b).ToString("d"));
                }

                if (!MyUtility.Check.Empty(sewinline_e))
                {
                    strSQLCmd += string.Format(" and ods.sewinline <= '{0}'", Convert.ToDateTime(sewinline_e).ToString("d"));
                }

                if (!MyUtility.Check.Empty(artworktype))
                {
                    strSQLCmd += " and ot.artworktypeid = @artworktypeid";
                }

                if (!MyUtility.Check.Empty(delivery_b))
                {
                    strSQLCmd += string.Format(" and ods.sciDelivery >= '{0}'", Convert.ToDateTime(delivery_b).ToString("d"));
                }

                if (!MyUtility.Check.Empty(delivery_e))
                {
                    strSQLCmd += string.Format(" and ods.sciDelivery <= '{0}'", Convert.ToDateTime(delivery_e).ToString("d"));
                }

                if (!MyUtility.Check.Empty(approve_b))
                {
                    strSQLCmd += string.Format(" and ot.apvdate >= '{0}'", Convert.ToDateTime(approve_b).ToString("d"));
                }

                if (!MyUtility.Check.Empty(approve_e))
                {
                    strSQLCmd += string.Format(" and ot.apvdate <= '{0}'", Convert.ToDateTime(approve_e).ToString("d"));
                }

                if (!MyUtility.Check.Empty(factory))
                {
                    strSQLCmd += " and ods.factoryid = @factoryid";
                }

                strSQLCmd += @" order by ods.FactoryID
,ot.ID
,ot.ArtworkTypeID
,ot.LocalSuppID,ot.ArtworkInLine,ods.SciDelivery";

                #region 準備sql參數資料
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@sp1",
                    Value = sp_b.TrimEnd(),
                };

                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@sp2",
                    Value = sp_e.TrimEnd(),
                };

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@artworktypeid",
                    Value = artworktype,
                };

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@factoryid",
                    Value = factory,
                };

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                cmds.Add(sp3);
                cmds.Add(sp4);
                #endregion

                DataTable dtOT;

                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, cmds, out dtOT))
                {
                    if (dtOT.Rows.Count == 0 && ap)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }

                    this.listControlBindingSource1.DataSource = dtOT;
                }
                else
                {
                    this.ShowErr(strSQLCmd, result);
                }
            }

            this.gridBatchApprove.AutoResizeColumns();
        }
    }
}
