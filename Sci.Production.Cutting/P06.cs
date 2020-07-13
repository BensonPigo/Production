using Ict;
using Ict.Win;

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Cutting
{
    public partial class P06 : Win.Tems.Input6
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;

        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}' and Status !='New'", this.keyWord);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("(select CT.FactoryID from Cutplan CP left join Cutting CT on CP.CuttingID=CT.ID where CP.ID = MarkerReq.Cutplanid) = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.*,
            (
                Select PatternPanel+'+ ' 
                From WorkOrder_PatternPanel c WITH (NOLOCK) 
                Where c.WorkOrderUkey =a.WorkOrderUkey 
                For XML path('')
            ) as PatternPanel,
            (
                Select cuttingwidth from Order_EachCons b WITH (NOLOCK) , WorkOrder e WITH (NOLOCK) 
                where e.Order_EachconsUkey = b.Ukey and a.WorkOrderUkey = e.Ukey  
            ) as cuttingwidth,
            o.styleid,o.seasonid
            From MarkerReq_Detail a WITH (NOLOCK) left join Orders o WITH (NOLOCK) on a.orderid=o.id
            where a.id = '{0}'
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Styleid", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seasonid", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Markerno", header: "Flow No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "MarkerName", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("FabricCombo", header: "FabricCombo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("CuttingWidth", header: "Cutting Width", width: Widths.AnsiChars(8), iseditingreadonly: true)
             .Numeric("ReqQty", header: "# of Copies", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
             .Numeric("ReleaseQty", header: "# of Release", width: Widths.AnsiChars(5), integer_places: 8)
             .Date("ReleaseDate", header: "Release Date", width: Widths.AnsiChars(10));
            this.detailgrid.Columns["ReleaseQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ReleaseDate"].DefaultCellStyle.BackColor = Color.Pink;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label7.Text = this.CurrentMaintain["Status"].ToString();
            this.displayRequestedby.Text = PublicPrg.Prgs.GetAddOrEditBy(this.CurrentMaintain["AddName"]);
            this.detailgrid.AutoResizeColumns();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.GetValue.Lookup("Status", this.CurrentMaintain["ID"].ToString(), "MarkerReq", "ID") == "New")
            {
                MyUtility.Msg.WarningBox("The Record not yet confirm");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            DataRow[] dray = ((DataTable)this.detailgridbs.DataSource).Select("releaseqty <>0");
            if (dray.Length != 0)
            {
                string updateSql = string.Format("Update MarkerReq set Status = 'Sent' where id ='{0}'", this.CurrentMaintain["ID"]);
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, updateSql)))
                {
                    return upResult;
                }
            }
            else
            {
                string updateSql = string.Format("Update MarkerReq set Status = 'Confirmed' where id ='{0}'", this.CurrentMaintain["ID"]);
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, updateSql)))
                {
                    return upResult;
                }
            }

            return base.ClickSavePost();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.OnDetailEntered();
        }

        private void btnBatchUpdate_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            if (MyUtility.Check.Empty(this.dateReleaseDate.Value))
            {
                return;
            }

            string reDate = this.dateReleaseDate.Text;
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["releaseDate"] = reDate;
            }
        }
    }
}
