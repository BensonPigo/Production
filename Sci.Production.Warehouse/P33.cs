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

        public P33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            this.DefaultFilter = $"MDivisionID='{Sci.Env.User.Keyword}' AND Type='E' ";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        #region 切換至Detail Tab

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                this.CurrentMaintain["IssueDate"] = DateTime.Now;
            }

            string OrderID = this.CurrentMaintain["OrderID"].ToString();
            this.displayPOID.Text = MyUtility.GetValue.Lookup($"SELECT POID FROm Orders WHERE ID='{OrderID}' ");
            this.displayLineNo.Text = MyUtility.GetValue.Lookup($@"SELECT t.SewLine + ',' 
FROM (
	SELECT DISTINCT o.SewLine 
	FROM dbo.Issue_Detail a WITH (nolock) 
	INNER JOIN dbo.Orders o WITH (nolock) ON a.POID = o.POID  
	WHERE  a.Id = '{CurrentMaintain["ID"]}' AND
	o.SewLine != ''
) t 
FOR xml path('')' ");
        }

        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "" : e.Detail["ID"].ToString();
            string ukey = (e.Detail == null || MyUtility.Check.Empty(e.Detail["ukey"])) ? "0" : e.Detail["ukey"].ToString();

            this.DetailSelectCommand = $@"
SELECT    psd.Refno
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
		, [Output Qty (Garment)]=''
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

            return base.OnSubDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {

            Ict.Win.DataGridViewGeneratorTextColumnSettings Refno = new DataGridViewGeneratorTextColumnSettings();
            Refno.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable bulkItems;
                    string sqlcmd = $@"
 SELECT [Refno]= psd.Refno
 , [Color]=psd.SuppColor
 , [Mtl. Type]=fc.MtlTypeID
 , [Desc]=fc.DescDetail
 , [Stock Unit]=StockUnit.Val
 , [Unit Desc]=StockUnit.Description
 , [Bulk Qty]=''
 , [Inventory Qty]=''
 FROM PO_Supp_Detail psd
 LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	 WHERE psd2.ID ='[表頭][SP#]' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor= psd.SuppColor
 )StockUnit
 OUTER APPLY(
 
 )BulkQty

";
                }
            };
            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("seq", header: "Refno", width: Widths.AnsiChars(6)) 
            .Text("Description", header: "Color", width: Widths.AnsiChars(20), iseditingreadonly: true) 
            .EditText("Colorid", header: "Desc.", width: Widths.AnsiChars(7), iseditingreadonly: true) 
            .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)  
            .Text("SizeUnit", header: "Accu. Issued"+Environment.NewLine+"(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("SizeUnit", header: "Issue Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("SizeUnit", header: "Use Qty" + Environment.NewLine + "By Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeUnit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("SizeUnit", header: "Use Qty" + Environment.NewLine + "By Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeUnit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeUnit", header: "Stock Unit Desc.", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("SizeUnit", header: "Output Qty" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("SizeUnit", header: "Balance" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true) 
            ;
            #endregion 欄位設定
        }
        #endregion

        #region ToolBar事件
        protected override bool ClickNew()
        {
            string TempId = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IT", "Issue", DateTime.Now);

            if (MyUtility.Check.Empty(TempId))
            {
                MyUtility.Msg.WarningBox("Get document ID fail!!");
                return false;
            }

            this.CurrentMaintain["ID"] = TempId;
            this.CurrentMaintain["Type"] = "E";
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;

            return base.ClickNew();
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

            //取得POID
            string POID = MyUtility.GetValue.Lookup($"SELECT POID FROM Orders WHERE ID ='{CurrentOrderID}' ");

            this.displayPOID.Text = POID;



        }

        private void txtOrderID_VisibleChanged(object sender, EventArgs e)
        {
            //•	根據[Issue_Summary表身Grid]規則帶出資料, 如[SP#]為空則清空
        }
        #endregion

        private void Get_Issue_Summary_Grid(string POID)
        {

        }
    }
}
