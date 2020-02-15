using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Transactions;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Utility.Excel;
using System.Data.SqlClient;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    public partial class P33 : Sci.Win.Tems.Input8
    {
        StringBuilder sbSizecode, sbSizecode2, strsbIssueBreakDown;
        DataTable dtSizeCode = null, dtIssueBreakDown = null;
        bool Ismatrix_Reload = true; //是否需要重新抓取資料庫資料
        string poid = "";

        public P33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            this.DefaultFilter = $"MDivisionID='{Sci.Env.User.Keyword}' AND Type='E' ";
        }

        public P33(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {

            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        #region Form事件

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                this.CurrentMaintain["IssueDate"] = DateTime.Now;
            }

            string OrderID = this.CurrentMaintain["OrderID"].ToString();
            this.displayPOID.Text = MyUtility.GetValue.Lookup($"SELECT POID FROm Orders WHERE ID='{OrderID}' ");
//            this.displayLineNo.Text = MyUtility.GetValue.Lookup($@"SELECT t.SewLine + ',' 
//FROM (
//	SELECT DISTINCT o.SewLine 
//	FROM dbo.Issue_Detail a WITH (nolock) 
//	INNER JOIN dbo.Orders o WITH (nolock) ON a.POID = o.POID  
//	WHERE  a.Id = '{CurrentMaintain["ID"]}' AND
//	o.SewLine != ''
//) t 
//FOR xml path('')' ");
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
SELECT    psd.SCIRefno
        , psd.Refno
		, iis.SuppColor
		, f.DescDetail
		, [@Qty]=''
		, [AccuIssued]=AccuIssued.Val
		, [IssueQty]=iis.Qty
		, [Use Qty By Stock Unit]=''--'[Output Qty (Garment)] * [@Qty]/100,並轉換為Stock Unit'
		, [StockUnit]=StockUnit.StockUnit
		, [Use Qty By Use Unit]= ''--'[Output Qty (Garment)] * [@Qty]'
		, [Use Unit]='CM'
		, [Stock Unit Desc.]=StockUnit.Description
		, [OutputQty]=''  ----Output Qty (Garment)
		, [Balance (Stock Unit)]= fi.InQty - fi.OutQty + fi.AdjustQty
		, [Location] = ''
FROM Issue i 
INNER JOIN Issue_Summary iis ON i.ID= iis.Id
INNER JOIN Issue_Detail isd ON isd.Issue_SummaryUkey=iis.Ukey
INNER JOIN PO_Supp_Detail psd ON psd.ID = iis.POID
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN FtyInventory fi ON fi.Ukey = isd.FtyInventoryUkey
OUTEr APPLY(
	SELECT [Val] = ISNULL(iis2.Qty,0)
	FROM Issue i2 WITH (NOLOCK) 
	INNER JOIN Issue_Summary iis2 WITH (NOLOCK) ON i2.ID=iis2.ID
	WHERE i2.Type='E' AND i2.Status='Confirmed' AND iis2.POID=iis.Poid AND iis2.SCIRefno=psd.SCIRefno
	AND iis2.SuppColor=iis.SuppColor AND i2.EditDate < i.AddDate
)AccuIssued
OUTER APPLY(
	SELECT TOP 1 PSD.StockUnit ,u.Description
	FROM PO_Supp_Detail psd2
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE psd2.ID = i.OrderId 
	AND psd2.SCIRefno = iis.SCIRefno 
	AND psd2.SuppColor = iis.SuppColor
)StockUnit
WHERE i.ID='PM1IC19110288' -- Head ID
";

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "" : e.Detail["ID"].ToString();
            string ukey = (e.Detail == null || MyUtility.Check.Empty(e.Detail["ukey"])) ? "0" : e.Detail["ukey"].ToString();
            this.SubDetailSelectCommand = string.Format(@"
;with aaa as(
    select  
         a.SizeCode
        , b.Id
        , Issue_DetailUkey =  '{2}'
        , QTY = isnull(b.Qty,0)
        , isvirtual = IIF(b.Qty IS NULL , 1 ,0)
        , seq
    --into #tmp
    from  dbo.Issue_Size b WITH (NOLOCK) 
    inner join dbo.Order_SizeCode a WITH (NOLOCK) on b.SizeCode = a.SizeCode
    outer apply(select poid from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{3}')poid1
    outer apply(select orders.poid from dbo.orders WITH (NOLOCK) left join dbo.Factory on orders.FtyGroup=Factory.ID where orders.id='{4}' and Factory.mdivisionid = '{3}')poid2
    where a.id= iif(isnull(poid1.POID,'')='',poid2.POID,poid1.poid)
    and b.id = '{1}' and b.Issue_DetailUkey = {2}
)"
            , CurrentMaintain["cutplanid"].ToString(), masterID, ukey, Sci.Env.User.Keyword, CurrentMaintain["orderid"].ToString());
            //if (!MyUtility.Check.Empty(CurrentMaintain["cutplanid"]))
            //{
            SubDetailSelectCommand += $@"
,bbb as(
	select distinct os.sizecode,ID = '{CurrentMaintain["orderid"]}',Issue_DetailUkey = '{ukey}',QTY=0,isvirtual = 1,seq
	from dbo.Order_SizeCode os WITH(NOLOCK)
	inner join orders o WITH(NOLOCK) on o.POID = os.Id
	inner join dbo.Order_Qty oq WITH(NOLOCK) on o.id = oq.ID and os.SizeCode = oq.SizeCode
	where o.POID = '{this.poid}' 
	and not exists(select SizeCode from aaa where aaa.SizeCode = os.sizecode)
)
select SizeCode,Id,Issue_DetailUkey,QTY,isvirtual
from(
	select * from aaa
	union all
	select * from bbb
)ccc
order by seq
";
            //}
            //else
            //{
            //    SubDetailSelectCommand += " select SizeCode, Id, Issue_DetailUkey, QTY, isvirtual from aaa order by seq ";
            //}

            return base.OnSubDetailSelectCommandPrepare(e);
        }


        protected override void OnDetailGridSetup()
        {
            #region Refno事件
            Ict.Win.DataGridViewGeneratorTextColumnSettings RefnoSet = new DataGridViewGeneratorTextColumnSettings();
            RefnoSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable bulkItems;
                    string sqlcmd = $@"
 SELECT    [Refno]= psd.Refno
		 , [SuppColor]=psd.SuppColor
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [StockUnit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
 FROM PO_Supp_Detail psd
 LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor= psd.SuppColor
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='B' AND psd2.SuppColor='{CurrentDetailData["SuppColor"]}'
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='I' AND psd2.SuppColor='{CurrentDetailData["SuppColor"]}'
 )InventoryQty
 WHERE psd.ID='{this.poid}' AND psd.SuppColor='{CurrentDetailData["SuppColor"]}'

";
                    IList<DataRow> selectedDatas;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }


                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(bulkItems
                            , "Refno,SuppColor,MtlType,Desc,StockUnit,UnitDesc,BulkQty,InventoryQty"
                            , "15,5,10,45,5,15,10,10", CurrentDetailData["Refno"].ToString()
                            , "Refno,SuppColor,MtlType,Desc,StockUnit,UnitDesc,BulkQty,InventoryQty");
                    selepoitem.Width = 1250;
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    selectedDatas = selepoitem.GetSelecteds();
                    CurrentDetailData["Refno"] = selectedDatas[0]["Refno"];
                }
            };
            RefnoSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["Refno"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["Refno"] = "";
                    }
                    else
                    {
                        DataRow row;

                        if (!MyUtility.Check.Seek($@"
 SELECT    [Refno]= psd.Refno
		 , [SuppColor]=psd.SuppColor
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [StockUnit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
 FROM PO_Supp_Detail psd
 LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor= psd.SuppColor
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='B' AND psd2.SuppColor='{CurrentDetailData["SuppColor"]}'
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='I' AND psd2.SuppColor='{CurrentDetailData["SuppColor"]}'
 )InventoryQty
 WHERE psd.ID='{this.poid}' AND psd.SuppColor='{CurrentDetailData["SuppColor"]}' AND psd.Refno='{e.FormattedValue}'", out row, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Refno");
                            return;
                        }
                        else
                        {
                            CurrentDetailData["Refno"] = e.FormattedValue;
                        }
                    }
                }
            };
            #endregion

            #region Color事件
            Ict.Win.DataGridViewGeneratorTextColumnSettings ColorSet = new DataGridViewGeneratorTextColumnSettings();
            ColorSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable bulkItems;
                    string sqlcmd = $@"
 SELECT    [Refno]= psd.Refno
		 , [SuppColor]=psd.SuppColor
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [StockUnit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
 FROM PO_Supp_Detail psd
 LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor= psd.SuppColor
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='B' AND psd2.Refno='{CurrentDetailData["Refno"]}'
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='I' AND psd2.Refno='{CurrentDetailData["Refno"]}'
 )InventoryQty
 WHERE psd.ID='{this.poid}' AND psd.Refno='{CurrentDetailData["Refno"]}'

";
                    IList<DataRow> selectedDatas;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }


                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(bulkItems
                            //, "Type,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,inqty,outqty,adjustqty,ukey"
                            , "Refno,SuppColor,MtlType,Desc,StockUnit,UnitDesc,BulkQty,InventoryQty"
                            , "15,5,10,45,5,15,10,10", CurrentDetailData["Refno"].ToString()
                            , "Refno,SuppColor,MtlType,Desc,StockUnit,UnitDesc,BulkQty,InventoryQty");
                    selepoitem.Width = 1250;
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    selectedDatas = selepoitem.GetSelecteds();
                    CurrentDetailData["SuppColor"] = selectedDatas[0]["SuppColor"];
                }
            };
            ColorSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["SuppColor"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["SuppColor"] = "";
                    }
                    else
                    {
                        DataRow row;

                        if (!MyUtility.Check.Seek($@"
 SELECT    [Refno]= psd.Refno
		 , [SuppColor]=psd.SuppColor
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [StockUnit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
 FROM PO_Supp_Detail psd
 LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor= psd.SuppColor
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='B' AND psd2.SuppColor='{e.FormattedValue}'
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='I' AND psd2.SuppColor='{e.FormattedValue}'
 )InventoryQty
 WHERE psd.ID='{this.poid}' AND psd.Refno='{CurrentDetailData["Refno"]}'  AND psd.SuppColor='{e.FormattedValue}'", out row, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "SuppColor");
                            return;
                        }
                        else
                        {
                            CurrentDetailData["SuppColor"] = e.FormattedValue;
                        }
                    }
                }
            };
            #endregion

            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), settings: RefnoSet) 
            .Text("SuppColor", header: "Color", width: Widths.AnsiChars(15),  settings: ColorSet) 
            .EditText("Desc.", header: "Desc.", width: Widths.AnsiChars(7), iseditingreadonly: true) 
            .Numeric("@Qty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)  
            .Text("Accu. Issued", header: "Accu. Issued"+Environment.NewLine+"(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("SizeUnit", header: "Issue Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("SizeUnit", header: "Use Qty" + Environment.NewLine + "By Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Stock Unit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Use Qty", header: "Use Qty" + Environment.NewLine + "By Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Use Unit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Stock Unit Desc.", header: "Stock Unit Desc.", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("OutputQty", header: "Output Qty" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Balance", header: "Balance" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true) 
            ;
            #endregion 欄位設定


            #region 可編輯欄位變色
            this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["SuppColor"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion 可編輯欄位變色
        }


        #endregion

        #region ToolBar事件

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            string TempId = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IT", "Issue", DateTime.Now);

            if (MyUtility.Check.Empty(TempId))
            {
                MyUtility.Msg.WarningBox("Get document ID fail!!");
                return;
            }
            //CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            //CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            //CurrentMaintain["Status"] = "New";
            //CurrentMaintain["Type"] = "B";
            //CurrentMaintain["issuedate"] = DateTime.Now;
            //CurrentMaintain["combo"] = 0;
            dtIssueBreakDown = null;
            gridIssueBreakDown.DataSource = null;
            txtOrderID.IsSupportEditMode = true;
            //txtRequest.IsSupportEditMode = true;
            //txtOrderID.ReadOnly = false;
            //txtRequest.ReadOnly = false;
        }
        protected override bool ClickEditBefore()
        {
            string Status = this.CurrentMaintain["Status"].ToString();
            if (Status != "New")
            {
                MyUtility.Msg.InfoBox("The record status is not new, can't modify !!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查不可為空

            string OrderID = this.CurrentMaintain["OrderID"].ToString();
            string IssueDate =MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]) ? string.Empty : this.CurrentMaintain["IssueDate"].ToString();

            if (MyUtility.Check.Empty(OrderID) || MyUtility.Check.Empty(IssueDate))
            {
                MyUtility.Msg.InfoBox("[SP#] , [Issue Date] can't be empty !!");
                return false;
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                //dr["PPICReasonID"] = string.Empty;
                //dr["PPICReasonDesc"] = string.Empty;
            }
            #endregion

            return base.ClickSaveBefore();
        }
        #endregion

        #region 控制項事件

        private void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            string CurrentOrderID = this.txtOrderID.Text;

            if (MyUtility.Check.Empty(CurrentOrderID))
            {
                this.displayPOID.Text = "";
                this.poid = "";
                return;
            }

            #region 防呆

            if (!MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID ='{CurrentOrderID}' "))
            {
                MyUtility.Msg.InfoBox($"<{CurrentOrderID}> not found !!");
                this.txtOrderID.Focus();
                return;
            }
            if (MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID ='{CurrentOrderID}' AND Junk=1"))
            {
                MyUtility.Msg.InfoBox($"<{CurrentOrderID}> Already Junked !!");
                this.txtOrderID.Focus();
                return;
            }
            string CurrentMDivisionID = MyUtility.GetValue.Lookup($"SELECT MDivisionID FROM Orders WHERE  ID ='{CurrentOrderID}' ");

            if (CurrentMDivisionID != Sci.Env.User.Keyword)
            {
                MyUtility.Msg.InfoBox($"<{CurrentOrderID}> M is {CurrentMDivisionID}. not the same login M. can't release !!");
                this.txtOrderID.Focus();
                return;
            }
            #endregion

            //取得POID
            string POID = MyUtility.GetValue.Lookup($"SELECT POID FROM Orders WHERE ID ='{CurrentOrderID}' ");

            this.displayPOID.Text = POID;
            this.poid = POID;
        }


        private void txtOrderID_Validated(object sender, EventArgs e)
        {
            string CurrentOrderID = this.txtOrderID.Text;

            //取得POID
            string POID = MyUtility.GetValue.Lookup($"SELECT POID FROM Orders WHERE ID ='{CurrentOrderID}' ");

            if (MyUtility.Check.Empty(POID))
            {
                dtIssueBreakDown = null;
                gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in DetailDatas)
                {
                    //刪除SubDetail資料
                    ((DataTable)detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                }
                return;
            }

            // 根據SP#，帶出這張訂單會用到的線材資訊(線的種類以及顏色)
            DualResult result = Detail_Reload();

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            Ismatrix_Reload = true;
            IssueBreakDown_Reload();
            detailgridbs.Position = 0;
            detailgrid.Focus();
            detailgrid.CurrentCell = detailgrid[10, 0];
            detailgrid.BeginEdit(true);
        }

        private void btnBreakDown_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["OrderId"]))
            {
                MyUtility.Msg.WarningBox("Please key-in Order ID first!!");
                return;
            }

            var frm = new Sci.Production.Warehouse.P33_IssueBreakDown(CurrentMaintain, dtIssueBreakDown, dtSizeCode);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }
        #endregion


        private void checkByCombo_CheckedChanged(object sender, EventArgs e)
        {
            if (dtIssueBreakDown == null) return;
            if (checkByCombo.Checked)
            {
                dtIssueBreakDown.DefaultView.RowFilter = string.Format("");
            }
            else
            {
                dtIssueBreakDown.DefaultView.RowFilter = string.Format("OrderID='{0}'", this.txtOrderID.Text);
            }
            string sql = string.Empty;

            if (CurrentMaintain == null)
            {
                return;
            }
            if (EditMode)
            {
                if (checkByCombo.Checked)
                {
                    sql = $@"
select distinct seq,os.sizecode
from dbo.Order_SizeCode os WITH(NOLOCK)
inner join orders o WITH(NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH(NOLOCK) on o.id = oq.ID and os.SizeCode = oq.SizeCode
where o.POID = '{this.poid}'
";
                }
                else
                {
                    sql = $@"
select distinct seq,os.sizecode
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.id ='{CurrentMaintain["orderid"]}'
";
                }
                DataTable sizecodeDt;
                DBProxy.Current.Select(null, sql, out sizecodeDt);

                foreach (DataRow dr in DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        DataTable subDt;
                        GetSubDetailDatas(dr, out subDt);
                        foreach (DataRow subdr in subDt.Rows)
                        {
                            if (!sizecodeDt.AsEnumerable().Any(r => r["Sizecode"].ToString() == MyUtility.Convert.GetString(subdr["sizecode"])))
                            {
                                subdr["Qty"] = 0;
                            }
                        }
                        //dr["OutputQty"] = string.Join(", ",
                        //        subDt.AsEnumerable()
                        //             .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                        //             .Select(row => row["SizeCode"].ToString() + "*" + Convert.ToDecimal(row["qty"]).ToString("0.00"))
                        //    );
                        //dr["@Qty"] = Math.Round(subDt.AsEnumerable()
                        //                            .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                        //                            .Sum(row => Convert.ToDouble(row["Qty"].ToString()))
                        //                            , 2);

                    }
                }
            }
        }

        private void Get_Issue_Breakdown_Grid(string POID)
        {

        }

        private DualResult Detail_Reload()
        {
            foreach (DataRow dr in DetailDatas)
            {
                //刪除SubDetail資料
                ((DataTable)detailgridbs.DataSource).Rows.Remove(dr);
                dr.Delete();
            }

            DataTable subData;
            DualResult result;

            string POID = MyUtility.GetValue.Lookup($"SELECT POID FROM Orders WHERE ID ='{ this.txtOrderID.Text}' ");

            // 回採購單找資料
            string sql = $@"
SELECT  psd.SCIRefno, psd.Refno, psd.SuppColor
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
WHERE psd.id ='{POID}' AND m.IsThread=1 AND psd.FabricType ='A'
ORDER BY psd.SCIRefno,psd.SuppColor

";
            result = DBProxy.Current.Select(null, sql, out subData);

            if (subData.Rows.Count == 0)
            {
                txtOrderID.Text = "";
                return Result.F("No PO Data !");
            }

            foreach (DataRow dr in subData.Rows)
            {
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                DataRow ndr = detailDt.NewRow();

                ndr["SCIRefno"] = dr["SCIRefno"];
                ndr["Refno"] = dr["Refno"];
                ndr["SuppColor"] = dr["SuppColor"];

                detailDt.Rows.Add(ndr);


                DataTable sizeRange, subDetails;
                if (GetSubDetailDatas(ndr, out subDetails))
                {
                    sql = $@"
select  a.SizeCode
        , b.Id
        , b.Issue_DetailUkey
        , isnull(b.Qty,0) QTY 
from dbo.Order_SizeCode a WITH (NOLOCK) 
left join dbo.Issue_Size b WITH (NOLOCK) on b.SizeCode = a.SizeCode 
                                            and b.id = '{CurrentMaintain["id"]}'
                                            --and b.Issue_DetailUkey = {ndr["ukey"]}
where   a.id = '{POID}' 
order by Seq 
";
                    DBProxy.Current.Select(null, sql, out sizeRange);
                    if (sizeRange == null)
                        continue;

                    foreach (DataRow drr in sizeRange.Rows)
                    {
                        drr.AcceptChanges();
                        drr.SetAdded();
                        subDetails.ImportRow(drr);
                    }
                }

            }

            return Result.True;
        }


        private DualResult IssueBreakDown_Reload()
        {
            if (EditMode == true && Ismatrix_Reload == false)
                return Result.True;

            Ismatrix_Reload = false;
            string sqlcmd;
            StringBuilder sbIssueBreakDown;
            DualResult result;

            string OrderID = txtOrderID.Text;

            // 取得該訂單Sizecode
            sqlcmd = $@"select sizecode from dbo.order_sizecode WITH (NOLOCK) 
where id = (select poid from dbo.orders WITH (NOLOCK) where id='{OrderID}') order by seq";

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dtSizeCode)))
            {
                ShowErr(sqlcmd, result);
                return Result.True;
            }
            if (dtSizeCode.Rows.Count == 0)
            {
                dtIssueBreakDown = null;
                gridIssueBreakDown.DataSource = null;
                return Result.True;
            }


            sbSizecode = new StringBuilder();
            sbSizecode2 = new StringBuilder();
            sbSizecode.Clear();
            sbSizecode2.Clear();
            for (int i = 0; i < dtSizeCode.Rows.Count; i++)
            {
                sbSizecode.Append($@"[{dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()}],");
                sbSizecode2.Append($@"{dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()},");
            }
            sbIssueBreakDown = new StringBuilder();
            //sbIssueBreakDown.Append(string.Format(@";with Bdown as 
            //(select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a WITH (NOLOCK) 
            //inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
            //where b.POID=(select poid from dbo.orders WITH (NOLOCK) where id = '{0}')
            //)
            //,Issue_Bdown as
            //(
            //	select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
            //	from Bdown full outer join (select * from dbo.Issue_Breakdown WITH (NOLOCK) where id='{1}') ib
            //	on Bdown.orderid = ib.OrderID and Bdown.Article = ib.Article and Bdown.SizeCode = ib.SizeCode
            //)
            //select * from Issue_Bdown
            //pivot
            //(
            //	sum(qty)
            //	for sizecode in ({2})
            //)as pvt
            //order by [OrderID],[Article]", OrderID, CurrentMaintain["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));//.Replace("[", "[_")

            sbIssueBreakDown.Append($@"
;with Bdown as 
(
    select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a WITH (NOLOCK) 
    inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
    where b.POID=( select poid from dbo.orders WITH (NOLOCK) where id = '{OrderID}' )
)
,Issue_Bdown as
(
    select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
    from Bdown full 
    outer join (select * from dbo.Issue_Breakdown WITH (NOLOCK) 
    where id='{CurrentMaintain["id"]}') ib
    on Bdown.orderid = ib.OrderID and Bdown.Article = ib.Article and Bdown.SizeCode = ib.SizeCode
)
select * from Issue_Bdown
pivot
(
    sum(qty)
    for sizecode in ({sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)})
)as pvt
order by [OrderID],[Article]
");
            strsbIssueBreakDown = sbIssueBreakDown;//多加一個變數來接 不改變欄位
            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out dtIssueBreakDown)))
            {
                ShowErr(sqlcmd, result);
                return Result.True;
            }

            gridIssueBreakDown.AutoGenerateColumns = true;
            gridIssueBreakDownBS.DataSource = dtIssueBreakDown;
            gridIssueBreakDown.DataSource = gridIssueBreakDownBS;
            gridIssueBreakDown.IsEditingReadOnly = true;
            gridIssueBreakDown.ReadOnly = true;

            checkByCombo_CheckedChanged(null, null);

            return Result.True;
        }

    }
}
