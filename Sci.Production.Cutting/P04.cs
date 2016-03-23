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

namespace Sci.Production.Cutting
{
    public partial class P04 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword; 
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.*,e.FabricCombo,e.seq1,e.seq2,e.FabricCode,e.SCIRefno,
            (
                Select distinct Article+'/ ' 
			    From dbo.WorkOrder_Distribute b
			    Where b.workorderukey = a.WorkOrderUkey and b.article!=''
                For XML path('')
            ) as article,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
                From WorkOrder_SizeRatio c
                Where  c.WorkOrderUkey =a.WorkOrderUkey 
                
                For XML path('')
            ) as SizeCode,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty*e.layer)+', ' 
                From WorkOrder_SizeRatio c 
                Where  c.WorkOrderUkey =a.WorkOrderUkey and c.WorkOrderUkey = e.Ukey
               
                For XML path('')
            ) as CutQty,
            (
                Select PatternPanel+'+ ' 
                From WorkOrder_PatternPanel c
                Where c.WorkOrderUkey =a.WorkOrderUkey 
                For XML path('')
            ) as PatternPanel    
            From Cutplan_Detail a, WorkOrder e
            where a.id = '{0}' and a.WorkOrderUkey = e.Ukey
            ", masterID);
            this.DetailSelectCommand = cmdsql;            
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Sewinglineid", header: "Line#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
            .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Fabriccode", header: "Fabric Code", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(10),iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(15));
            this.detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override bool ClickDeleteBefore()
        {
            #region 判斷Encode 不可,MarkerReqid 存在
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not delete.");
                return false;
            }
            #endregion

            return base.ClickDeleteBefore();
        }
        protected override DualResult ClickDeletePost()
        {
            #region 清空WorkOrder 的Cutplanid
            string clearCutplanidSql = string.Format("Update WorkOrder set cutplanid ='' where cutplanid ='{0}'", CurrentMaintain["ID"]);
            #endregion
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, clearCutplanidSql)))
            {
                return upResult;
            }
            return base.ClickDeletePost();
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            #region 建立Cutplan_Detail_Cons 資料
            DataTable detailTb = (DataTable)detailgridbs.DataSource;
            string insert_cons = string.Format(
                        @"insert into Cutplan_Detail_Cons(id,poid,seq1,seq2,cons) 
                        select a.id,a.poid,b.seq1,b.seq2,sum(a.cons) as tt 
                        from Cutplan_Detail a,workorder b  
                        where a.id='{0}' and a.workorderukey = b.Ukey 
                        group by a.id,a.poid,b.seq1,b.seq2", CurrentMaintain["ID"]);
            #endregion
            string insertmk = "";
            string insert_mark2 = "";
            #region 建立Bulk request 
                
            #region ID
            string keyword = keyWord + "MK";
            string reqid = MyUtility.GetValue.GetID(keyword, "MarkerReq");
            if (string.IsNullOrWhiteSpace(reqid))
            {
                return;
            }
           #endregion
            insertmk = string.Format(
            @"Insert into MarkerReq
            (id,estctdate,mDivisionid,CutCellid,Status,Cutplanid,AddName,AddDate) 
            values('{0}','{1}','{2}','{3}','New','{4}','{5}',getdate()));",
            reqid,CurrentMaintain["estcutdate"],CurrentMaintain["mDivisionid"],
            CurrentMaintain["cutcellid"],CurrentMaintain["ID"],loginID);

            #region 表身
            string marker2sql = string.Format(
            @"Select b.Orderid,b.MarkerName,sum(b.Layer) as layer,
            b.MarkerNo,b.fabricCombo,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
                From WorkOrder_SizeRatio c
                Where a.WorkOrderUkey =c.WorkOrderUkey            
                For XML path('')
            ) as SizeRatio
            From Cutplan_Detail a, WorkOrder b 
            Where a.workorderukey = b.ukey and a.id = '{0}'
			Group by b.Orderid,b.MarkerName,b.MarkerNo,
                b.fabricCombo,a.WorkOrderUkey", CurrentMaintain["ID"]);
            #endregion
            DataTable markerTb;
                   
            DualResult dResult = DBProxy.Current.Select(null, marker2sql, out markerTb);
            if(dResult)
            {
                foreach (DataRow dr in markerTb.Rows)
                {
                    insert_mark2 = insert_mark2 + string.Format(
                    @"Insert into MarkerReq_Detail      
                    (ID,OrderID,SizeRatio,MarkerName,Layer,FabricCombo,MarkerNo) 
                    Values('{0}','{1}','{2}','{3}',{4},'{5}','{6}');",
                        reqid,dr["OrderID"],dr["SizeRatio"],dr["MarkerName"],
                        dr["Layer"],dr["FabricCombo"],dr["MarkerNo"]);
                }
            }
            else
            {
                ShowErr(marker2sql,dResult);
                return;
            }
                
                
            

            #endregion

            #region update Master
            string updSql = string.Format("update Cutplan set Status = 'Confirmed', editdate = getdate(), editname = '{0}' Where id='{1}'", loginID, CurrentMaintain["ID"]);
            #endregion
            #region transaction
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, insert_cons)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    if (insertmk != "")
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insertmk)))
                        {
                            _transactionscope.Dispose();
                            return;
                        }
                    }
                    if (insert_mark2 != "")
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insert_mark2)))
                        {
                            _transactionscope.Dispose();
                            return;
                        }
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
            #endregion
            
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 有Marker Req 不可Unconfirm
            if (MyUtility.Check.Empty(CurrentMaintain["markerreqid"]))
            {
                MyUtility.Msg.WarningBox("The record already create Marker request, you can not Unconfirm.");
                return;
            }
            #endregion
            #region 有IssueFabric 不可Uncomfirm
            DataTable queryIssueFabric;
            string Query = string.Format("Select * from Issue Where Cutplanid ='{0}'", CurrentMaintain["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, Query, out queryIssueFabric);
            if (dResult)
            {
                if (queryIssueFabric.Rows.Count != 0)
                {
                    MyUtility.Msg.WarningBox("The record already issued fabric, you can not Unconfirm.");
                    return;
                }
            }
            else
            {
                ShowErr(Query, dResult);
                return;
            }
            #endregion
            string updSql = string.Format("Delete cutplan_Detail_Cons where id ='{1}';update Cutplan set Status = 'New', editdate = getdate(), editname = '{0}' Where id='{1}'", loginID, CurrentMaintain["ID"]);
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        protected override bool ClickNew()
        {
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P04_Import();
            frm.ShowDialog(this);
            return true;
        }
        protected override bool ClickEditBefore()
        {
            #region 判斷Encode 不可,MarkerReqid 存在
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not modify.");
                return false;
            }
            #endregion
            return base.ClickEditBefore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P04_Import();
            frm.ShowDialog(this);
        }
    }
}
