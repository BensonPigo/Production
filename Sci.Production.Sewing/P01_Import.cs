using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P01_Import : Sci.Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;

        /// <inheritdoc/>
        public P01_Import(DataRow drMaster, DataTable detail)
        {
            this.InitializeComponent();
            this.dt_detail = detail;
            this.dr_master = drMaster;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SewingLineID", header: "Line#", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("Inline", header: "Inline Date", width: Widths.AnsiChars(12), iseditingreadonly: true)
                ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
            {
                strWhere += $@" and convert(date,ss.Inline) >= '{Convert.ToDateTime(this.dateInlineDate.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
            {
                strWhere += $@" and convert(date,ss.Inline) <= '{Convert.ToDateTime(this.dateInlineDate.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtLine.Text))
            {
                strWhere += $@" and s.SewingLineID = '{this.txtLine.Text}'";
            }

            string sqlcmd = $@"
select sd.OrderID,sd.Article,sd.ComboType,sd.Color,AutoCreate = 0,QAQty = 0
,od.StyleID,ss.Inline,s.FactoryID,s.SewingLineID
,ttlQAQty = isnull(sewing.ttlQAQty,0)
,ttlQty = isnull(od.ttlQty,0)
,rowno = ROW_NUMBER() over(partition by sd.OrderID order by sd.ID)
into #tmpQty
from SewingOutput s
inner join SewingOutput_Detail sd on s.ID = sd.ID
left join SewingSchedule ss on ss.SewingLineID = s.SewingLineID and ss.OrderID = sd.OrderId
left join Factory f on f.ID = s.FactoryID
outer apply(
	select ttlQAQty = sum(QAQty) 
	from SewingOutput_Detail t
	where t.OrderId = sd.OrderId
	and t.ID = s.ID
) sewing
outer apply(
	select t.StyleID,ttlQty = sum(Qty) 
	from Orders t
	where t.ID = sd.OrderId
	group by t.StyleID
) od
where f.UseAPS = 0
and not exists(
    select 1 from #tmp t
    where t.OrderID = sd.OrderID
)
{strWhere}


select  Selected = 0,id = '',t.*
from #tmpQty t
where ttlQAQty < ttlQty 
and rowno = 1
order by OrderId desc

drop table #tmpQty
";

            DualResult result;
            if (result = MyUtility.Tool.ProcessWithDatatable(this.dt_detail, string.Empty, sqlcmd, out DataTable dt))
            {
                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = dt;
            }
            else
            {
                this.ShowErr(sqlcmd, result);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow dr in dr2)
            {
                dr["ID"] = this.dr_master["ID"];
                dr["QAQty"] = 0;
                dr.AcceptChanges();
                dr.SetAdded();
                this.dt_detail.ImportRow(dr);
            }

            this.Close();
        }
    }
}
