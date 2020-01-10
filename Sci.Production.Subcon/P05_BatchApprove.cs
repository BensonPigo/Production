﻿using System;
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
using Sci.Production.PublicPrg;

namespace Sci.Production.Subcon
{
    public partial class P05_BatchApprove : Sci.Win.Subs.Base
    {
        Action delegateAct;
        private bool boolDeptApv;
        private bool canConfrim;
        private bool canCheck;
        public P05_BatchApprove(Action reload)
        {
            InitializeComponent();
            this.EditMode = true;
            this.delegateAct = reload;
            // 檢查是否擁有Confirm or Check權限
            canConfrim = Prgs.GetAuthority(Sci.Env.User.UserID, "P05. Sub-con Requisition", "CanConfirm");
            canCheck = Prgs.GetAuthority(Sci.Env.User.UserID, "P05. Sub-con Requisition", "CanCheck");

            boolDeptApv = true;
            Authority();
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
            string filter = string.Empty;
            if (boolDeptApv)
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
            Authority();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            this.gridArtworkReq.ValidateControl();
            DataSet ds = (DataSet)listControlBindingSource1.DataSource;
            DataTable dt = ds.Tables["Master"];
            DataTable dt2 = ds.Tables["Detail"];
            DualResult result;
            string sqlcmd = string.Empty;
            string strStatus;

            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0) return; 

            var query = dt.AsEnumerable().Where(x => x["Selected"].EqualString("1")).Select(x => x.Field<string>("ID"));
            if (query.Count() == 0) {
                MyUtility.Msg.WarningBox("Please select data first.", "Warning");
                return;
            }

            DataRow[] drSelect = dt.Select(" Selected = 1");
            foreach (DataRow dr in drSelect)
            {
                // Status = New
                if (boolDeptApv)
                {
                    // 判斷irregular Reason沒寫不能存檔
                    DataTable dtDetail = dt2.AsEnumerable().Where(x => x["ID"].EqualString(dr["id"].ToString())).CopyToDataTable();
                    var IrregularQtyReason = new Sci.Production.Subcon.P05_IrregularQtyReason(dr["ID"].ToString(), dr, dtDetail);

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
           

            if (!(result = DBProxy.Current.Execute(null, sqlcmd))) {
                ShowErr(sqlcmd, result);
                return;
            }

            Query();
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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_P05_BatchApprove.xltx");
        }

        private void Authority()
        {
            if ((canConfrim & canCheck) || Env.User.IsAdmin)
            {
                if (boolDeptApv)
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
                boolDeptApv = canCheck ? true : false;
            }
        }

        private void LinkLabelDept_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            boolDeptApv = true;
            Query();
        }

        private void LinkLabelMg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            boolDeptApv = false;
            Query();
        }
    }
}
