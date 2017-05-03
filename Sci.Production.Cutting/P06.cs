using Ict;
using Ict.Win;
using Sci.Production.Class;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Runtime.InteropServices;

namespace Sci.Production.Cutting
{
    public partial class P06 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword; 
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}' and Status !='New'", keyWord);
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
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
            Helper.Controls.Grid.Generator(this.detailgrid)
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
            this.label7.Text = CurrentMaintain["Status"].ToString();
            this.displayRequestedby.Text = PublicPrg.Prgs.GetAddOrEditBy(CurrentMaintain["AddName"]);
            this.detailgrid.AutoResizeColumns();
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.GetValue.Lookup("Status",CurrentMaintain["ID"].ToString(),"MarkerReq","ID")=="New")
            {
                MyUtility.Msg.WarningBox("The Record not yet confirm");
                return false;
            }
            return base.ClickSaveBefore();
        }
        protected override DualResult ClickSavePost()
        {
            DataRow[] dray = ((DataTable)detailgridbs.DataSource).Select("releaseqty <>0");
            if (dray.Length != 0)
            {
                string updateSql = string.Format("Update MarkerReq set Status = 'Sent' where id ='{0}'", CurrentMaintain["ID"]);
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, updateSql)))
                {
                    return upResult;
                }
            }
            else
            {
                string updateSql = string.Format("Update MarkerReq set Status = 'Confirmed' where id ='{0}'", CurrentMaintain["ID"]);
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
            OnDetailEntered();
        }

        private void btnBatchUpdate_Click(object sender, EventArgs e)
        {
            grid.ValidateControl();
            if (MyUtility.Check.Empty(dateReleaseDate.Value)) return;
            string reDate = dateReleaseDate.Text;
            foreach (DataRow dr in DetailDatas)
            {
                dr["releaseDate"] = reDate;
            }
        }

    }
}
