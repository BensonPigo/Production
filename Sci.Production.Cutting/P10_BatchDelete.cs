﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P10_BatchDelete : Sci.Win.Subs.Base
    {
        private DataTable dtQuery;
        public P10_BatchDelete()
        {
            InitializeComponent();
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            QueryData();
        }

        private void QueryData()
        {
            DateTime? AddDate1, AddDate2;
            AddDate1 = dateAddDate.Value1;
            AddDate2 = dateAddDate.Value2;
            if ((MyUtility.Check.Empty(AddDate1) && MyUtility.Check.Empty(AddDate2)) &&
                MyUtility.Check.Empty(this.txtCutRef.Text) &&
                MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                MyUtility.Msg.WarningBox("SP#, CutRef add AddDate cannot all be empty.");
                return;
            }

            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(AddDate1))
            {
                sqlwhere += $@" and convert(date, b.adddate)  between '{Convert.ToDateTime(AddDate1).ToString("d")}' and '{Convert.ToDateTime(AddDate2).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(this.txtCutRef.Text))
            {
                sqlwhere += $@" and b.CutRef = '{this.txtCutRef.Text}'"; ;
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlwhere += $@" and b.OrderID = '{this.txtSPNo.Text}'";
            }

            string sqlcmd = $@"
select 
[Selected]=0
,b.ID
,b.POID,b.MDivisionid
,[Factory] = o.FtyGroup
,o.StyleID
,b.CutRef,b.Orderid,b.Sizecode,b.Colorid,b.Article,b.PatternPanel,b.Cutno,b.Cdate
,[EstCutDate] = estdate.estcutdate
,b.Sewinglineid,b.Item,b.SewingCell,b.Ratio,b.PrintDate,b.oldid,b.AddDate,b.EditDate
,[Excess] = iif(b.IsExcess=1,'Y','') 
from bundle b
left join Orders o on b.Orderid=o.ID
outer apply
(
	Select MAX(estcutdate) as estcutdate
	from workorder WITH (NOLOCK) where workorder.id = b.POID and workorder.cutref = b.CutRef
	and workorder.MDivisionID = b.MDivisionID
) as estdate
where 1=1
{sqlwhere}
";
            this.ShowWaitMessage("Data Loading....");
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtQuery))
            {
                if (dtQuery.Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
                listControlBindingSource1.DataSource = dtQuery;
            }
            else { ShowErr(sqlcmd, result); }
            this.HideWaitMessage();

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchDelete.IsEditingReadOnly = false;
            this.gridBatchDelete.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridBatchDelete)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("id", header: "ID", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("POID", header: "POID", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .Text("MDivisionid", header: "M", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(17))
                .Text("CutRef", header: "Cut Ref#", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("Sizecode", header: "Size", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("Colorid", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("PatternPanel", header: "PatternPanel", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Cutno", header: "Cut#", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Date("Cdate", header: "Create Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Date("EstCutDate", header: "Est. cut date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Sewinglineid", header: "Line#", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Item", header: "Item", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("SewingCell", header: "SewingCell", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Ratio", header: "Ratio", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Date("PrintDate", header: "PrintDate", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("oldid", header: "oldid", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .DateTime("AddDate", header: "Add Date", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .DateTime("EditDate", header: "Edit Date", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("Excess", header: "Excess", iseditingreadonly: true, width: Widths.AnsiChars(6));
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            gridBatchDelete.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DualResult result;
            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to delete these Bundle Card?");
            if (dResult.ToString().ToUpper() == "NO") return;
            string sqldelete = string.Empty;
            this.ShowWaitMessage("Data Process....");
            foreach (DataRow dr in dr2)
            {
                sqldelete += string.Format(@"
delete from bundle where id = '{0}';
delete from Bundle_Detail where id = '{0}';
delete from Bundle_Detail_Art where id = '{0}';
delete from Bundle_Detail_AllPart where id = '{0}';
delete from Bundle_Detail_qty where id = '{0}';
", dr["ID"]);
            }

            if (!(result = DBProxy.Current.Execute("", sqldelete)))
            {
                ShowErr(result);
                this.HideWaitMessage();
                return;
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Finish batch delete!!");
            QueryData();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
