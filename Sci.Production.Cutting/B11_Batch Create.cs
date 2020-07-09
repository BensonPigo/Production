using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class B11_Batch_Create : Win.Tems.QueryForm
    {
        public B11_Batch_Create()
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        private DataTable Dt1;
        private DataTable Dt2;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();

            string sqlcmd = B11.DefaultQuerySql();
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource2.DataSource = this.Dt2;
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
            .Text("ID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("StyleName", header: "Style Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;

            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(4), maximum: 255)
            .Text("SubProcessID", header: "SubProcess", width: Widths.AnsiChars(24), iseditingreadonly: true)
            .Text("ArtworkTypeId", header: "Artwork Type", width: Widths.AnsiChars(35), iseditingreadonly: true)
            ;
            this.grid2.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            this.listControlBindingSource1.DataSource = null;
            if (MyUtility.Check.Empty(this.txtstyle1.Text) && MyUtility.Check.Empty(this.txtseason1.Text) && MyUtility.Check.Empty(this.txtbrand1.Text))
            {
                MyUtility.Msg.WarningBox("<Brand>,<Style>,<Season> can't be empty!!");
                return;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtstyle1.Text))
            {
                where += "\r\n" + $@" and s.id = '{this.txtstyle1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtseason1.Text))
            {
                where += "\r\n" + $@" and s.SeasonID = '{this.txtseason1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtbrand1.Text))
            {
                where += "\r\n" + $@" and s.BrandID = '{this.txtbrand1.Text}'";
            }

            string sqlcmd = $@"
select selected=0,s.id,s.StyleName,s.SeasonID,s.BrandID,s.Ukey
from style s
where 1=1
and s.ApvDate is not null
and not exists(select 1 from SubProcessSeq where StyleUkey = s.Ukey)
and s.Junk=0
{where}
Order by s.ApvDate desc
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.Dt1.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            this.listControlBindingSource1.DataSource = this.Dt1;
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            string sqlcmd = B11.B10_QuerySql();
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource2.DataSource = this.Dt2;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            this.grid2.ValidateControl();
            if (this.Dt1 == null)
            {
                return;
            }

            DataRow[] drs = this.Dt1.Select("selected = 1");
            if (drs.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select datas first!");
                return;
            }

            DataTable dt = drs.CopyToDataTable();
            string sqlcmd = $@"
select msg=concat('<Style#:',ID, ',Season:',seasonID,',Brand:',brandID,'> already exists!!')
from #tmp t
where exists(select 1 from SubProcessSeq where Styleukey = t.Ukey)
";
            DataTable dto;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlcmd, out dto);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dto.Rows.Count > 0)
            {
                List<string> list = dto.AsEnumerable().Select(s => new { msg = MyUtility.Convert.GetString(s["msg"]) }).Select(s => s.msg).ToList();
                MyUtility.Msg.WarningBox(string.Join("\r\n", list));
                return;
            }

            // ↓Cross join
            var x = dt.AsEnumerable().SelectMany(
                        t1 => this.Dt2.AsEnumerable()
                            .Select(t2 => new
                            {
                                StyleUkey = MyUtility.Convert.GetLong(t1["Ukey"]),
                                SubProcessID = MyUtility.Convert.GetString(t2["SubProcessID"]),
                                Seq = MyUtility.Convert.GetString(t2["Seq"]),
                            }))
                        .ToList();

            sqlcmd = $@"
INSERT INTO [dbo].[SubProcessSeq]([StyleUkey],[AddName],[AddDate])
select  distinct t.StyleUkey, '{Env.User.UserID}',  getdate()
from #tmp t

INSERT INTO [dbo].[SubProcessSeq_Detail]([StyleUkey],[SubProcessID],[Seq])
select StyleUkey,SubProcessID,Seq
from #tmp t
where Seq <> 0
";

            result = MyUtility.Tool.ProcessWithObject(x, string.Empty, sqlcmd, out dto);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Complete!");
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
