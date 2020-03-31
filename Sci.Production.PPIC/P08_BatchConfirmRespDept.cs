using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P08_BatchConfirmRespDept : Sci.Win.Tems.QueryForm
    {
        private readonly string Type;
        private DataTable dt1;
        private DataTable dt2;

        /// <inheritdoc/>
        public P08_BatchConfirmRespDept(string type)
        {
            this.InitializeComponent();

            // P08 = F, P09 = A
            this.Type = type;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.comboDropDownList1.SelectedIndex = 2;
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("ID", header: "ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Name", header: "Responsibility", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("TTLUS", header: "Total US$", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
            ;

            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("DepartmentID", header: "Dept.", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Percentage", header: "%", decimal_places: 2, width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Amount", header: "Amt", decimal_places: 2, width: Widths.AnsiChars(7), iseditingreadonly: true)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.dateCreateDate.Value1))
            {
                where += $@"and rr.CDate >='{((DateTime)this.dateCreateDate.Value1).ToString("d")}'" + "\r\n";
            }

            if (!MyUtility.Check.Empty(this.dateCreateDate.Value2))
            {
                where += $@"and rr.CDate <='{((DateTime)this.dateCreateDate.Value2).ToString("d")}'" + "\r\n";
            }

            if (!MyUtility.Check.Empty(this.comboDropDownList1.SelectedValue))
            {
                where += $@"and rr.Responsibility ='{this.comboDropDownList1.SelectedValue}'" + "\r\n";
            }

            if (!MyUtility.Check.Empty(this.dateLock.Value))
            {
                where += $@"and rr.LockDate ='{((DateTime)this.dateLock.Value).ToString("d")}'" + "\r\n";
            }
            #endregion

            string sqlcmd = $@"
select
	Selected=0,
	ID,
	Name = (select Name from DropDownList Where Type = 'Replacement.R' and ID = rr.Responsibility),
	POID,
	TTLUS = isnull(rr.RMtlAmt,0) + isnull(rr.ActFreight,0) +isnull( rr.EstFreight,0) + isnull(rr.SurchargeAmt,0),
    rr.ApplyName,
	EMail=(select EMail from Pass1 where ID = rr.ApplyName)
from ReplacementReport rr with(nolock)
where 1=1
and rr.RespDeptConfirmDate is null 
and exists(select 1 from ICR_ResponsibilityDept icr with(nolock) where icr.id = rr.id)
and rr.Type  = '{this.Type}'
{where}
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dt1.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            this.listControlBindingSource1.DataSource = this.dt1;

            var ids = this.dt1.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["id"])).ToList();
            sqlcmd = $@"
select
	IRD.FactoryID,
	IRD.DepartmentID,
	IRD.Percentage,
	IRD.Amount,
    IRD.ID
from ICR_ResponsibilityDept IRD With(nolock)
where IRD.ID in('{string.Join("','", ids)}')
";
            result = DBProxy.Current.Select(null, sqlcmd, out this.dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource2.DataSource = this.dt2;
            this.DataRowChangeEventArgs();
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (this.dt1 == null || this.dt1.Rows.Count == 0)
            {
                return;
            }

            this.grid1.ValidateControl();
            DataRow[] drs = this.dt1.Select("Selected = 1");
            if (drs.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select datas!");
                return;
            }

            var emaillist = drs.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["EMail"])).Distinct().ToList();
            var ids = drs.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["id"])).ToList();
            string mailto = string.Join(";", emaillist);
            string mailCC = Sci.Env.User.MailAddress;
            string subject = "Replacement Report responsibility update";
            string description = "Pls re-check and update responsibility information." + "\r\n" + string.Join("\r\n", ids);
            var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailCC, subject, string.Empty, description, false, true);
            email.ShowDialog(this);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (this.dt1 == null || this.dt1.Rows.Count ==0)
            {
                return;
            }

            this.grid1.ValidateControl();
            DataRow[] drs = this.dt1.Select("Selected = 1");
            if (drs.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select datas!");
                return;
            }

            var ids = drs.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["id"])).ToList();
            string sqlupdate = $@"
update ReplacementReport
Set RespDeptConfirmDate = getdate(), RespDeptConfirmName = '{Sci.Env.User.UserID}'
where ID in('{string.Join("','", ids)}')
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.Query();
        }

        private void BtnCose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grid1_SelectionChanged(object sender, EventArgs e)
        {
            this.DataRowChangeEventArgs();
        }

        private void DataRowChangeEventArgs()
        {
            DataRow dr = this.grid1.GetDataRow<DataRow>(this.grid1.GetSelectedRowIndex());
            if (dr != null && this.dt2 != null)
            {
                string filter = $"id = '{dr["ID"]}'";
                this.dt2.DefaultView.RowFilter = filter;
            }
        }
    }
}
