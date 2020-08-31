using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Subcon
{
    public partial class P05_BatchApprove : Win.Subs.Base
    {
        Action delegateAct;
        private bool boolDeptApv;
        private bool canConfrim;
        private bool canCheck;
        Func<string, string> sqlGetBuyBackDeduction;
        public P05_BatchApprove(Action reload, Func<string, string> SqlGetBuyBackDeduction)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.delegateAct = reload;

            // 檢查是否擁有Confirm or Check權限
            this.canConfrim = Prgs.GetAuthority(Env.User.UserID, "P05. Sub-con Requisition", "CanConfirm");
            this.canCheck = Prgs.GetAuthority(Env.User.UserID, "P05. Sub-con Requisition", "CanCheck");

            this.boolDeptApv = true;
            this.Authority();
            this.sqlGetBuyBackDeduction = SqlGetBuyBackDeduction;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();

            this.gridArtworkReq.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridArtworkReq.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridArtworkReq)
                 .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                 .Text("ID", header: "Req#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("FactoryId", header: "Factory", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Date("ReqDate", header: "Req Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("LocalSuppID", header: "Supplier", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("DeptApvName", header: "Dept. Mgr Apv", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("MgApvName", header: "Mg Mgr Apv", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            for (int i = 0; i < this.gridArtworkReq.Columns.Count; i++)
            {
                this.gridArtworkReq.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            this.gridArtworkReqDetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridArtworkReqDetail.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridArtworkReqDetail)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("StyleID", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("ReqQty", header: "Req Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("PatternCode", header: "Curpart ID", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("QtyGarment", header: "Qty/GMT", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 ;

            for (int i = 0; i < this.gridArtworkReqDetail.Columns.Count; i++)
            {
                this.gridArtworkReqDetail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;
            DataSet ds = null;
            DataRelation relation;
            string sqlCmd = string.Empty;
            string filter = string.Empty;
            if (this.boolDeptApv)
            {
                filter = @" and Status = 'New'";
            }
            else
            {
                filter = @" and Status = 'Locked'";
            }

            sqlCmd = $@"
select 0 Selected
    ,ID
	,FactoryId
	,ReqDate
	,ArtworkTypeID
	,LocalSuppID
	,[DeptApvName] = (select name from Pass1 where id = DeptApvName)
	,[MgApvName] = (select name from pass1 where id = MgApvName)
	,Remark
    ,[Status],Exceed
from ArtworkReq
where 1=1
{filter}

select a.ID
	,ad.OrderID  
	,o.StyleID 
	,ad.ReqQty
	,ad.ArtworkId
	,ad.Stitch
	,ad.PatternCode
    ,ad.PatternDesc
	,ad.QtyGarment
    ,[Status],Exceed
from ArtworkReq a
inner join ArtworkReq_Detail ad on a.ID = ad.ID 
left join Orders o on ad.OrderID = o.ID
where 1=1
{filter}
                      ";

            if (!SQL.Selects(string.Empty, sqlCmd, out ds))
            {
                MyUtility.Msg.WarningBox(sqlCmd, "DB error!!");
                return;
            }

            relation = new DataRelation(
                "rel1",
                new DataColumn[] { ds.Tables[0].Columns["ID"] },
                new DataColumn[] { ds.Tables[1].Columns["ID"] });

            ds.Relations.Add(relation);
            ds.Tables[0].TableName = "Master";
            ds.Tables[1].TableName = "Detail";

            this.listControlBindingSource1.DataSource = ds;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";
            this.gridArtworkReq.AutoResizeColumns();
            this.gridArtworkReqDetail.AutoResizeColumns();
            this.Authority();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            this.gridArtworkReq.ValidateControl();
            DataSet ds = (DataSet)this.listControlBindingSource1.DataSource;
            DataTable dt = ds.Tables["Master"];
            DataTable dt2 = ds.Tables["Detail"];
            DualResult result;
            string sqlcmd = string.Empty;
            string strStatus;

            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0)
            {
                return;
            }

            var query = dt.AsEnumerable().Where(x => x["Selected"].EqualString("1")).Select(x => x.Field<string>("ID"));
            if (query.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first.", "Warning");
                return;
            }

            DataRow[] drSelect = dt.Select(" Selected = 1");
            foreach (DataRow dr in drSelect)
            {
                // Status = New
                if (this.boolDeptApv)
                {
                    // 判斷irregular Reason沒寫不能存檔
                    DataTable dtDetail = dt2.AsEnumerable().Where(x => x["ID"].EqualString(dr["id"].ToString())).CopyToDataTable();
                    var IrregularQtyReason = new P05_IrregularQtyReason(dr["ID"].ToString(), dr, dtDetail, this.sqlGetBuyBackDeduction);

                    DataTable dtIrregular = IrregularQtyReason.Check_Irregular_Qty();
                    if (dtIrregular != null)
                    {
                        bool isReasonEmpty = dtIrregular.AsEnumerable().Any(s => MyUtility.Check.Empty(s["SubconReasonID"]));
                        if (isReasonEmpty)
                        {
                            MyUtility.Msg.WarningBox($@"<Req#: {dr["ID"]}> Irregular Qty Reason cannot be empty!");
                            return;
                        }
                    }

                    if (MyUtility.Check.Empty(dr["Exceed"]))
                    {
                        strStatus = "Approved";
                    }
                    else
                    {
                        strStatus = "Locked";
                    }

                    sqlcmd += $@"
update ArtworkReq 
set [Status] = '{strStatus}'
, DeptApvName = '{Env.User.UserID}'
, DeptApvDate = GETDATE()
, editname = '{Env.User.UserID}'
, editdate=GETDATE()  
where ID = '{dr["ID"]}' ";
                }

                // Status = Checked
                else
                {
                    sqlcmd += $@"
update ArtworkReq 
set [Status] = 'Approved'
, MgApvName = '{Env.User.UserID}'
, MgApvDate = GETDATE()
, editname = '{Env.User.UserID}'
, editdate=GETDATE()  
where ID = '{dr["ID"]}' ";
                }
            }

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            this.Query();
            MyUtility.Msg.InfoBox("Successful.");
            this.delegateAct();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable printData;
            string sqlCmd = string.Format(@"
select aq.MDivisionID as [M]
	,aq.FactoryId as [Factory]
	,aq.ID as [P/O #]
    ,aq.ReqDate as [Date]
	,aq.LocalSuppID as [Supplier]
	,aq.ArtworkTypeID as [ArtworkType]
	,aqd.OrderID as [SP#]
	,o.SewInLine as [Sewing Inline]
	,o.SciDelivery as [SCI Delivery]
	,o.StyleID as [Style#]
	,aqd.ArtworkId as [Pattern]
	,aqd.PatternDesc as [Cutparts]
    ,aqd.ReqQty as [Q'ty]
	,aq.Remark 
from Artworkreq aq
inner join Artworkreq_Detail aqd on aq.id = aqd.id
left join orders o on aqd.OrderID = o.ID
where 
(
    (aq.Status = 'Locked' and Exceed = 1) 
or 
    (aq.Status = 'New' and Exceed = 0)
)
order by aq.ID, aqd.OrderID 
");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            MyUtility.Excel.CopyToXls(printData, string.Empty, "Subcon_P05_BatchApprove.xltx");
        }

        private void Authority()
        {
            if ((this.canConfrim & this.canCheck) || Env.User.IsAdmin)
            {
                if (this.boolDeptApv)
                {
                    this.linkLabelDept.Enabled = false;
                    this.linkLabelMg.Enabled = true;
                }
                else
                {
                    this.linkLabelDept.Enabled = true;
                    this.linkLabelMg.Enabled = false;
                }
            }
            else
            {
                this.boolDeptApv = this.canCheck ? true : false;
            }
        }

        private void LinkLabelDept_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.boolDeptApv = true;
            this.Query();
        }

        private void LinkLabelMg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.boolDeptApv = false;
            this.Query();
        }
    }
}
