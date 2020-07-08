using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P21_BatchConfirmRespDept
    /// </summary>
    public partial class P21_BatchConfirmRespDept : Sci.Win.Forms.Base
    {
        private DataTable dtICR;
        private List<DataRow> listICR_ResponsibilityDept;

        /// <inheritdoc/>
        public P21_BatchConfirmRespDept(bool isReCal = false)
        {
            this.InitializeComponent();
            this.EditMode = true;
            if (isReCal)
            {
                this.btnReCalculate.Visible = true;
                this.btnReject.Visible = false;
                this.btnConfirm.Visible = false;
                this.Text = "Batch Re-Calculate Responsibility Dept. Amt";
            }
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridICR)
                .CheckBox("select", iseditable: true, trueValue: 1, falseValue: 0)
                .Text("ID", "ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("TotalUS", header: "Total US$", width: Widths.AnsiChars(15), iseditingreadonly: true, decimal_places: 2, integer_places: 12)
                ;

            this.Helper.Controls.Grid.Generator(this.gridICR_ResponsibilityDept)
                .Text("FactoryID", "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("DepartmentID", "Dept.", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .Numeric("Percentage", header: "%", width: Widths.AnsiChars(10), iseditingreadonly: true, decimal_places: 0, integer_places: 10)
                .Numeric("Amount", header: "Amt", width: Widths.AnsiChars(13), iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                ;
        }

        private void Query()
        {
            this.gridICR.DataSource = null;
            this.gridICR_ResponsibilityDept.DataSource = null;
            string sqlGetData = string.Empty;
            string sqlWhere = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (this.dateRangeConfirm.HasValue1)
            {
                sqlWhere += " and ICR.CfmDate >= @RespDeptConfirmDateFrom ";
                listPar.Add(new SqlParameter("@RespDeptConfirmDateFrom", this.dateRangeConfirm.DateBox1.Value));
            }

            if (this.dateRangeConfirm.HasValue2)
            {
                sqlWhere += " and ICR.CfmDate <= @RespDeptConfirmDateTo ";
                listPar.Add(new SqlParameter("@RespDeptConfirmDateTo", this.dateRangeConfirm.DateBox2.Value));
            }

            sqlGetData = $@"
select  [select] = 0,
        ICR.ID,
        ICR.OrderID,
        [TotalUS] = ICR.RMtlAmtUSD + ICR.ActFreightUSD + ICR.OtherAmtUSD,
        [Email] = isnull(p.EMail,tp.EMail),
        [TTlAmt] = isnull(i3.Amt,0)
into    #tmp
from ICR with (nolock)
left join Pass1 p with (nolock) on ICR.EditName = p.ID
left join TpePass1 tp with (nolock) on ICR.EditName = tp.ID
outer apply(
	select [Amt] = sum(ISNULL(Amount,0))
	from ICR_ResponsibilityDept 
	where id=ICR.id
)i3

where   RespDeptConfirmDate is null {sqlWhere}

select t.* from #tmp t where exists (select 1 from ICR_ResponsibilityDept ird with (nolock) where t.ID = ird.ID)

select  ID,
        FactoryID,
        DepartmentID,
        Percentage,
        Amount
from ICR_ResponsibilityDept
where   ID in (select ID from #tmp)

";

            DataTable[] dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, listPar, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listICR_ResponsibilityDept = dtResult[1].AsEnumerable().ToList();
            this.dtICR = dtResult[0];

            this.gridICR.DataSource = this.dtICR;
        }

        private void GridICR_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridICR.SelectedRows.Count == 0)
            {
                return;
            }

            string id = this.gridICR.SelectedRows[0].Cells["ID"].Value.ToString();
            var resultICR_ResponsibilityDept = this.listICR_ResponsibilityDept.Where(s => s["ID"].ToString() == id);

            if (resultICR_ResponsibilityDept.Any())
            {
                this.gridICR_ResponsibilityDept.DataSource = resultICR_ResponsibilityDept.CopyToDataTable();
            }
            else
            {
                this.gridICR_ResponsibilityDept.DataSource = null;
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (this.dtICR == null)
            {
                return;
            }

            var listSelectedICR = this.dtICR.AsEnumerable().Where(s => (int)s["select"] == 1);

            if (!listSelectedICR.Any())
            {
                MyUtility.Msg.WarningBox("Please select at least one item");
                return;
            }

            string mailTo = listSelectedICR.Select(s => s["Email"].ToString()).Distinct().JoinToString(";");
            string mailCC = Env.User.MailAddress;
            string subject = "ICR responsibility update";
            string content = $@"
Pls re-check and update responsibility information.
{listSelectedICR.Select(s => s["ID"].ToString()).JoinToString(Environment.NewLine)}
";
            var email = new MailTo(Sci.Env.Cfg.MailFrom, mailTo, mailCC, subject, string.Empty, content, false, true);
            email.ShowDialog(this);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (this.dtICR == null)
            {
                return;
            }

            var listSelectedICR = this.dtICR.AsEnumerable().Where(s => (int)s["select"] == 1);

            if (!listSelectedICR.Any())
            {
                MyUtility.Msg.WarningBox("Please select at least one item");
                return;
            }

            if (listSelectedICR.Where(w => MyUtility.Convert.GetDecimal(w["TotalUS"]) != MyUtility.Convert.GetDecimal(w["TTLAMT"])).Any())
            {
                MyUtility.Msg.WarningBox("Total department shared amount must equal to Total US$.");
                return;
            }

            string updateICR_ID = listSelectedICR.Select(s => "'" + s["ID"].ToString() + "'").JoinToString(",");
            string updSQL = $@"
update ICR set RespDeptConfirmDate = getdate(), RespDeptConfirmName = '{Env.User.UserID}'
where ID in ({updateICR_ID})

";
            DualResult result = DBProxy.Current.Execute(null, updSQL);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Confirm Complete!!");
            this.Query();
        }

        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            if (this.dtICR == null)
            {
                return;
            }

            var listSelectedICR = this.dtICR.AsEnumerable().Where(s => (int)s["select"] == 1);

            if (!listSelectedICR.Any())
            {
                MyUtility.Msg.WarningBox("Please select at least one item");
                return;
            }

            foreach (DataRow item in listSelectedICR)
            {
                decimal ttlus = MyUtility.Convert.GetDecimal(item["TotalUS"]);
                DataRow[] tmp2 = ((DataTable)this.gridICR_ResponsibilityDept.DataSource).Select($"id = '{item["id"]}'");
                if (tmp2.Length > 0)
                {
                    decimal ttlamt = 0;
                    foreach (var item2 in tmp2)
                    {
                        decimal amt = Math.Round(
                            ttlus
                            * (MyUtility.Convert.GetDecimal(item2["Percentage"])
                            / 100), 2);
                        item2["Amount"] = amt;
                        ttlamt += amt;
                    }

                    if (ttlus != ttlamt)
                    {
                        tmp2[tmp2.Length - 1]["Amount"] = MyUtility.Convert.GetDecimal(tmp2[tmp2.Length - 1]["Amount"]) + (ttlus - ttlamt);
                    }
                }
            }

            var ids = listSelectedICR.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["id"])).ToList();
            DataTable tmpdt2 = ((DataTable)this.gridICR_ResponsibilityDept.DataSource).Select($"ID in('{string.Join("','", ids)}')").CopyToDataTable();
            string sqlcmd = $@"
update IRD set Amount = t.Amount
from ICR_ResponsibilityDept IRD
inner join #tmp t on t.id = IRD.id and t.FactoryID = IRD.FactoryID and t.DepartmentID = IRD.DepartmentID
";
            DataTable odt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(tmpdt2, string.Empty, sqlcmd, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.Query();
        }
    }
}
