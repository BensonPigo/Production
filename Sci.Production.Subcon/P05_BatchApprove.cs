using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
namespace Sci.Production.Subcon
{
    public partial class P05_BatchApprove : Sci.Win.Subs.Base
    {
        Action delegateAct;
        public P05_BatchApprove(Action reload)
        {
            InitializeComponent();
            this.EditMode = true;
            this.delegateAct = reload;
        }

        protected override void OnFormLoaded()
        {

            base.OnFormLoaded();
            Query();

            this.gridArtworkReq.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridArtworkReq.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridArtworkReq)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                 .Text("ID", header: "Req#", width: Widths.AnsiChars(15), iseditingreadonly :true)
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

            this.gridArtworkReqDetail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridArtworkReqDetail.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.gridArtworkReqDetail)                 
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
            Query();
        }

        private void Query()
        {
            listControlBindingSource1.DataSource = null;
            listControlBindingSource2.DataSource = null;
            DataSet ds = null;
            DataRelation relation;
            string sqlCmd = string.Empty; 

            sqlCmd = string.Format(@"
select 0 Selected
    ,ID
	,FactoryId
	,ReqDate
	,ArtworkTypeID
	,LocalSuppID
	,[DeptApvName] = (select name from Pass1 where id = DeptApvName)
	,[MgApvName] = (select name from pass1 where id = MgApvName)
	,Remark
from ArtworkReq
where (
    ([Status] = 'Locked' and Exceed = 1) 
or 
    ([Status] = 'New' and Exceed = 0)
)


select a.ID
	,ad.OrderID  
	,o.StyleID 
	,ad.ReqQty
	,ad.ArtworkId
	,ad.Stitch
	,ad.PatternCode
	,ad.QtyGarment
from ArtworkReq a
inner join ArtworkReq_Detail ad on a.ID = ad.ID 
left join Orders o on ad.OrderID = o.ID
where (
([Status] = 'Locked' and Exceed = 1) 
or 
([Status] = 'New' and Exceed = 0)
)

                      ");

            if (!SQL.Selects("", sqlCmd, out ds)){
                MyUtility.Msg.WarningBox(sqlCmd, "DB error!!");
                return;
            }

            relation = new DataRelation("rel1"
                , new DataColumn[] { ds.Tables[0].Columns["ID"] }
                , new DataColumn[] { ds.Tables[1].Columns["ID"] }
           );

            ds.Relations.Add(relation);
            ds.Tables[0].TableName = "Master";
            ds.Tables[1].TableName = "Detail";

            listControlBindingSource1.DataSource = ds;
            listControlBindingSource1.DataMember = "Master";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";
            this.gridArtworkReq.AutoResizeColumns();
            this.gridArtworkReqDetail.AutoResizeColumns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)listControlBindingSource1.DataSource;
            DataTable dt = ds.Tables["Master"];
            DualResult result;
            string sqlcmd = string.Empty;

            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0) return; 

            var query = dt.AsEnumerable().Where(x => x["Selected"].EqualString("1")).Select(x => x.Field<string>("ID"));
            if (query.Count() == 0) {
                MyUtility.Msg.WarningBox("Please select data first.", "Warning");
                return;
            }

            sqlcmd = string.Format(@"update ArtworkReq 
                    set [Status]='Approved', MgApvName='{0}', MgApvDate=GETDATE(), editname='{0}', editdate=GETDATE()  
                    where ID in ('{1}') ", Env.User.UserID, string.Join("','", query.ToList()));

            if (!(result = DBProxy.Current.Execute(null, sqlcmd))) {
                ShowErr(sqlcmd, result);
                return;
            }

            Query();
            MyUtility.Msg.InfoBox("Sucessful");
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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_P05_BatchApprove.xltx");
        }
    }
}
