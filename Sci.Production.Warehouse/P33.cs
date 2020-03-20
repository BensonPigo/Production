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

        P33_Detail subform = new P33_Detail();

        public P33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            this.gridicon.Location = new System.Drawing.Point(btnBreakDown.Location.X + 20, 95); //此gridcon位置會跑掉，需強制設定gridcon位置   
            this.DefaultFilter = $"MDivisionID='{Sci.Env.User.Keyword}' AND Type='E' ";


            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_Summary";           // PK: ID+UKey
            SubGridAlias = "Issue_Detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            KeyField1 = "ID"; //Issue PK
            KeyField2 = "ID"; // Summary FK

            //SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            SubDetailKeyField1 = "id,Ukey";    // second PK
            SubDetailKeyField2 = "id,Issue_SummaryUkey"; // third FK

            DoSubForm = new P33_Detail();
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


            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_summary";           // PK: ID+UKey
            SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            KeyField1 = "ID"; //Issue PK
            KeyField2 = "ID"; // Summary FK

            //SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            SubDetailKeyField1 = "id,Ukey";    // second PK
            SubDetailKeyField2 = "id,Issue_SummaryUkey"; // third FK

            DoSubForm = new P33_Detail();
        }

        #region Form事件
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            //this.labelConfirmed.Visible = MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? false : true;

            labelConfirmed.Text = CurrentMaintain["status"].ToString();

            if (!MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                this.txtOrderID.IsSupportEditMode = false;
                this.txtOrderID.ReadOnly = true;
            }
            else
            {
                this.txtOrderID.IsSupportEditMode = true;
                this.txtOrderID.ReadOnly = false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                this.CurrentMaintain["IssueDate"] = DateTime.Now;
            }

            string OrderID = this.CurrentMaintain["OrderID"].ToString();
            this.displayPOID.Text = MyUtility.GetValue.Lookup($"SELECT POID FROm Orders WHERE ID='{OrderID}' ");
            this.displayLineNo.Text= MyUtility.GetValue.Lookup($@"
SELECT t.sewline + ',' 
FROM(SELECT DISTINCT o.sewline FROM dbo.issue_detail a WITH (nolock) 
INNER JOIN dbo.orders o WITH (nolock) ON a.poid = o.poid  
WHERE o.id = '{OrderID}' AND o.sewline != '') t FOR xml path('')
");

            #region -- matrix breakdown
            RenewData();
            DualResult result;
            if (!(result = matrix_Reload()))
            {
                ShowErr(result);
            }
            #endregion

        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            Ismatrix_Reload = true;
            this.DetailSelectCommand = $@"

SELECT   iis.SCIRefno
        , [Refno]=Refno.Refno
		, iis.SuppColor
		, f.DescDetail
		, [@Qty]= ThreadUsedQtyByBOT.Val
		, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I2 WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I2.id = [IS].Id 
					where I2.type = 'E' and I2.Status = 'Confirmed' 
					and [IS].Poid=iis.POID AND [IS].SCIRefno=iis.SCIRefno AND [IS].SuppColor=iis.SuppColor and i2.[EditDate]<I.AddDate AND i2.ID <> i.ID
				)
		, [IssueQty]=iis.Qty
		, [Use Qty By Stock Unit] = CEILING(Garment.Qty *  ThreadUsedQtyByBOT.Val/ 100 * ISNULL(UnitRate.RateValue,1) ) 
		, [Stock Unit]=StockUnit.StockUnit

		, [Use Unit]='CM'
		, [Use Qty By Use Unit]= (Garment.Qty *  ThreadUsedQtyByBOT.Val  )

		, [Stock Unit Desc.]=StockUnit.Description
		, [OutputQty] = Garment.Qty
		, [Balance(Stock Unit)]= ISNULL( fi.InQty - fi.OutQty + fi.AdjustQty ,0)
		, [Location]=ISNULL(Location.MtlLocationID,'')
        , [POID]=iis.POID
        , i.MDivisionID
        , i.ID
        , iis.Ukey
FROM Issue i 
INNER JOIN Issue_Summary iis ON i.ID= iis.Id
LEFT JOIN Issue_Detail isd ON isd.Issue_SummaryUkey=iis.Ukey
LEFT JOIN FtyInventory fi ON fi.Ukey = isd.FtyInventoryUkey
LEFT JOIN Fabric f ON f.SCIRefno = iis.SCIRefno
OUTER APPLY(
	SELECT DISTINCT Refno
	FROM PO_Supp_Detail psd
	WHERE psd.ID = iis.POID AND psd.SCIRefno = iis.SCIRefno 
	AND psd.SuppColor=iis.SuppColor
)Refno
OUTER APPLY(
	SELECT Val=SUM((SeamLength * Frequency * UseRatio) + Allowance)
	FROM dbo.GetThreadUsedQtyByBOT(iis.POID)
	WHERE SCIRefNo = iis.SCIRefno AND SuppColor = iis.SuppColor
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT   [MtlLocationID] = STUFF(
	(
		SELECT DISTINCT ',' +fid.MtlLocationID 
		FROM Issue_Detail 
		INNER JOIN FtyInventory FI ON FI.POID=Issue_Detail.POID AND FI.Seq1=Issue_Detail.Seq1 AND FI.Seq2=Issue_Detail.Seq2
		INNER JOIN FtyInventory_Detail FID ON FID.Ukey= FI.Ukey
		WHERE Issue_Detail.ID = i.ID AND  FI.StockType='B' AND  fid.MtlLocationID  <> '' AND Issue_Detail.ukey=isd.ukey
		FOR XML PATH('')
	), 1, 1, '') 
)Location
OUTER APPLY(
	SELECT SCIRefNo,SuppColor,[Qty]=SUM(Qty)
	FROM(
		SELECT DISTINCT  O.POID ,t.OrderID , tcd.SCIRefNo, tcd.SuppColor,tcd.Article ,  t.Qty
		From dbo.Orders as o
		INNER JOIN dbo.Style as s On s.Ukey = o.StyleUkey
		INNER JOIN dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
		INNER JOIN dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
        INNER JOIN (
				        SElECT OrderID ,Article ,[Qty]=SUM(Qty)   ----這裡等同於表身下方Grid的總和(不分Size)
				        FROM Issue_Breakdown
				        WHERE ID='{masterID}'
				        GROUP BY OrderID ,Article
			        ) t ON /* t.Article = tcd.Article AND*/ t.OrderID= o.ID
		WHERE tcd.SCIRefNo= iis.SCIRefNo AND tcd.SuppColor = iis.SuppColor 
	)A
	GROUP BY  SCIRefNo, SuppColor
)Garment
OUTER APPLY(
	SELECT TOP 1 psd2.StockUnit ,u.Description
	FROM PO_Supp_Detail psd2
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE psd2.ID = i.OrderId 
	AND psd2.SCIRefno = iis.SCIRefno 
	AND psd2.SuppColor = iis.SuppColor
)StockUnit
OUTER APPLY(
	SELECT RateValue
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate
WHERE i.ID='{masterID}' 
AND iis.SuppColor <> ''
--AND Garment.Qty IS NOT NULL
";

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            sum_subDetail(e.Detail, e.SubDetails);

            DataTable dt;
            foreach (DataRow dr in DetailDatas)
            {
                if (GetSubDetailDatas(dr, out dt))
                {
                    sum_subDetail(dr, dt);
                }
            }

            return base.ConvertSubDetailDatasFromDoSubForm(e);
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
                    string SuppColor =MyUtility.Check.Empty(CurrentDetailData["SuppColor"]) ? string.Empty : CurrentDetailData["SuppColor"].ToString();
                    string Refno = MyUtility.Check.Empty(CurrentDetailData["Refno"]) ? string.Empty : CurrentDetailData["Refno"].ToString();
                    string sqlcmd = $@"
 SELECT   DISTINCT [Refno]= psd.Refno
		 , [SuppColor]=psd.SuppColor
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [Stock Unit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
		 , psd.SCIRefno
INTO #tmp
FROM PO_Supp_Detail psd
LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
LEFT JOIN MtlType m on m.id= fc.MtlTypeID
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
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='B' {(MyUtility.Check.Empty(SuppColor) ? "AND psd2.SuppColor <> ''": $"AND psd2.SuppColor='{SuppColor}'")}
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='I' {(MyUtility.Check.Empty(SuppColor) ? "AND psd2.SuppColor <> ''" : $"AND psd2.SuppColor='{SuppColor}'")}
 )InventoryQty
WHERE psd.ID='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
";
                    if (!MyUtility.Check.Empty(Refno))
                    {
                        sqlcmd += $"AND psd.Refno='{Refno}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.Refno <> '' ";
                    }

                    if (!MyUtility.Check.Empty(SuppColor))
                    {
                        sqlcmd += $"AND psd.SuppColor='{SuppColor}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.SuppColor <> '' ";
                    }

                    sqlcmd += $@"
SELECT [Refno]
		 , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
		 , [BulkQty]=SUM(BulkQty)
		 , [InventoryQty]=SUM(InventoryQty)
         , [SCIRefno]
FROM #tmp
GROUP BY  [Refno]
		 , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
         , [SCIRefno]

DROP TABLE #tmp

";

                    IList<DataRow> selectedDatas;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }


                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(bulkItems
                            , "Refno,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty"
                            , "15,5,10,45,5,15,10,10", CurrentDetailData["Refno"].ToString()
                            , "Refno,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty");
                    selepoitem.Width = 1250;
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    selectedDatas = selepoitem.GetSelecteds();

                    CurrentDetailData["SCIRefno"] = selectedDatas[0]["SCIRefno"];
                    CurrentDetailData["Refno"] = selectedDatas[0]["Refno"];
                    CurrentDetailData["SuppColor"] = selectedDatas[0]["SuppColor"];

                    Refno = CurrentDetailData["Refno"].ToString();
                    SuppColor = CurrentDetailData["suppColor"].ToString();
                    CurrentDetailData["POID"] = this.poid;

                    // 取得預設帶入
                    #region 取得預設帶入
                    sqlcmd = $@"
SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.SuppColor
, f.DescDetail
, [@Qty]=BOT.Val
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].SuppColor=PSD.SuppColor and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit]=0.00
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit]=0.00
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty]=0.00
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.SuppColor=psd.SuppColor
)StockUnit
OUTER APPLY(
	SELECT Val=SUM((SeamLength * Frequency * UseRatio) + Allowance)
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID)
	WHERE SCIRefNo = psd.SCIRefno AND SuppColor = psd.SuppColor
)BOT
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.SuppColor <> ''

AND psd.Refno='{Refno}'
AND psd.SuppColor='{SuppColor}'
";

                    DataRow row;
                    if (!MyUtility.Check.Seek(sqlcmd, out row, null))
                    {
                        MyUtility.Msg.WarningBox("Data not found!", "Refno");
                        return;
                    }

                    CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                    CurrentDetailData["Refno"] = row["Refno"];
                    CurrentDetailData["SuppColor"] = row["SuppColor"];
                    CurrentDetailData["POID"] = row["POID"];
                    CurrentDetailData["DescDetail"] = row["DescDetail"];
                    CurrentDetailData["@Qty"] = row["@Qty"];
                    CurrentDetailData["Use Unit"] = row["Use Unit"];
                    CurrentDetailData["AccuIssued"] = row["AccuIssued"];

                    CurrentDetailData["IssueQty"] = row["IssueQty"];
                    CurrentDetailData["Use Qty By Stock Unit"] = row["Use Qty By Stock Unit"];
                    CurrentDetailData["Stock Unit"] = row["Stock Unit"];
                    CurrentDetailData["Use Qty By Use Unit"] = row["Use Qty By Use Unit"];
                    CurrentDetailData["Use Unit"] = row["Use Unit"];
                    CurrentDetailData["Stock Unit Desc."] = row["Stock Unit Desc."];
                    CurrentDetailData["OutputQty"] = row["OutputQty"];
                    CurrentDetailData["Balance(Stock Unit)"] = row["Balance(Stock Unit)"];
                    CurrentDetailData["Location"] = row["Location"];
                    CurrentDetailData["MDivisionID"] = row["MDivisionID"];

                    #endregion

                    CurrentDetailData.EndEdit();
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
                        CurrentDetailData["SCIRefno"] = "";
                        //CurrentDetailData["SuppColor"] = "";
                        CurrentDetailData["POID"] = "";
                        CurrentDetailData["DescDetail"] = "";
                        CurrentDetailData["@Qty"] = DBNull.Value;
                        CurrentDetailData["Use Unit"] = "";
                        CurrentDetailData["AccuIssued"] = DBNull.Value;

                        CurrentDetailData["IssueQty"] = DBNull.Value;
                        CurrentDetailData["Use Qty By Stock Unit"] = DBNull.Value;
                        CurrentDetailData["Stock Unit"] = "";
                        CurrentDetailData["Use Qty By Use Unit"] = DBNull.Value;
                        CurrentDetailData["Use Unit"] = "";
                        CurrentDetailData["Stock Unit Desc."] = "";
                        CurrentDetailData["OutputQty"] = DBNull.Value;
                        CurrentDetailData["Balance(Stock Unit)"] = DBNull.Value;
                        CurrentDetailData["Location"] = "";
                        CurrentDetailData["MDivisionID"] = "";
                    }
                    else
                    {
                        DataRow row;

                        string suppColor = MyUtility.Check.Empty(CurrentDetailData["SuppColor"]) ? string.Empty : CurrentDetailData["SuppColor"].ToString();

                        string sqlcmd = $@"
SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.SuppColor
, f.DescDetail
, [@Qty]=BOT.Val
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].SuppColor=PSD.SuppColor and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit]=0.00
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit]=0.00
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty]=0.00
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.SuppColor=psd.SuppColor
)StockUnit
OUTER APPLY(
	SELECT Val=SUM((SeamLength * Frequency * UseRatio) + Allowance)
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID)
	WHERE SCIRefNo = psd.SCIRefno AND SuppColor = psd.SuppColor
)BOT
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.SuppColor <> ''

AND psd.Refno='{e.FormattedValue}'
";

                        if (!MyUtility.Check.Empty(suppColor))
                        {
                            sqlcmd += $"AND psd.SuppColor='{suppColor}' ";
                        }
                        else
                        {
                            sqlcmd += $"AND psd.SuppColor <> '' ";
                        }


                        if (!MyUtility.Check.Seek(sqlcmd, out row, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Refno");
                            return;
                        }
                        else
                        {
                            if (MyUtility.Check.Empty(suppColor))
                            {
                                CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                                CurrentDetailData["Refno"] = e.FormattedValue;
                                //CurrentDetailData["DescDetail"] = row["DescDetail"];
                                CurrentDetailData["POID"] = this.poid;
                            }
                            else
                            {
                                CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                                CurrentDetailData["Refno"] = row["Refno"];
                                CurrentDetailData["SuppColor"] = row["SuppColor"];
                                CurrentDetailData["POID"] = row["POID"];
                                CurrentDetailData["DescDetail"] = row["DescDetail"];
                                CurrentDetailData["@Qty"] = row["@Qty"];
                                CurrentDetailData["Use Unit"] = row["Use Unit"];
                                CurrentDetailData["AccuIssued"] = row["AccuIssued"];

                                CurrentDetailData["IssueQty"] = row["IssueQty"];
                                CurrentDetailData["Use Qty By Stock Unit"] = row["Use Qty By Stock Unit"];
                                CurrentDetailData["Stock Unit"] = row["Stock Unit"];
                                CurrentDetailData["Use Qty By Use Unit"] = row["Use Qty By Use Unit"];
                                CurrentDetailData["Use Unit"] = row["Use Unit"];
                                CurrentDetailData["Stock Unit Desc."] = row["Stock Unit Desc."];
                                CurrentDetailData["OutputQty"] = row["OutputQty"];
                                CurrentDetailData["Balance(Stock Unit)"] = row["Balance(Stock Unit)"];
                                CurrentDetailData["Location"] = row["Location"];
                                CurrentDetailData["MDivisionID"] = row["MDivisionID"];
                            }

                            CurrentDetailData.EndEdit();
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
                    string Refno = MyUtility.Check.Empty(CurrentDetailData["Refno"]) ? string.Empty : CurrentDetailData["Refno"].ToString();
                    string SuppColor = MyUtility.Check.Empty(CurrentDetailData["SuppColor"]) ? string.Empty : CurrentDetailData["SuppColor"].ToString();
                    string sqlcmd = $@"
 SELECT  DISTINCT  [Refno]= psd.Refno
		 , [SuppColor]=psd.SuppColor
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [Stock Unit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
         , psd.SCIRefno
INTO #tmp
 FROM PO_Supp_Detail psd
 LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
LEFT JOIN MtlType m on m.id= fc.MtlTypeID
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
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='B' {(MyUtility.Check.Empty(Refno) ? "AND psd2.Refno <> ''" : $"AND psd2.Refno='{Refno}'")}
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.SuppColor=psd.SuppColor AND F.StockType='I' {(MyUtility.Check.Empty(Refno) ? "AND psd2.Refno <> ''" : $"AND psd2.Refno='{Refno}'")}
 )InventoryQty
 WHERE psd.ID='{this.poid}'
 AND m.IsThread=1 
AND psd.FabricType ='A'
AND psd.SuppColor <> ''
";
                    if (!MyUtility.Check.Empty(Refno))
                    {
                        sqlcmd += $"AND psd.Refno='{Refno}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.Refno <> '' ";
                    }

                    if (!MyUtility.Check.Empty(SuppColor))
                    {
                        sqlcmd += $"AND psd.SuppColor='{SuppColor}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.SuppColor <> '' ";
                    }

                    sqlcmd += $@"
SELECT [Refno]
		 , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
		 , [BulkQty]=SUM(BulkQty)
		 , [InventoryQty]=SUM(InventoryQty)
         , [SCIRefno]
FROM #tmp
GROUP BY  [Refno]
		 , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
         , [SCIRefno]

DROP TABLE #tmp

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
                            , "Refno,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty"
                            , "15,5,10,45,5,15,10,10", CurrentDetailData["Refno"].ToString()
                            , "Refno,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty");
                    selepoitem.Width = 1250;
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    selectedDatas = selepoitem.GetSelecteds();

                    CurrentDetailData["SCIRefno"] = selectedDatas[0]["SCIRefno"];
                    CurrentDetailData["Refno"] = selectedDatas[0]["Refno"];
                    CurrentDetailData["SuppColor"] = selectedDatas[0]["SuppColor"];

                    Refno = CurrentDetailData["Refno"].ToString();
                    SuppColor = CurrentDetailData["SuppColor"].ToString();
                    CurrentDetailData["POID"] = this.poid;

                    // 取得預設帶入
                    #region 取得預設帶入
                    sqlcmd = $@"
SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.SuppColor
, f.DescDetail
, [@Qty]=BOT.Val
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].SuppColor=PSD.SuppColor and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit]=0.00
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit]=0.00
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty]=0.00
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.SuppColor=psd.SuppColor
)StockUnit
OUTER APPLY(
	SELECT Val=SUM((SeamLength * Frequency * UseRatio) + Allowance)
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID)
	WHERE SCIRefNo = psd.SCIRefno AND SuppColor = psd.SuppColor
)BOT
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.SuppColor <> ''

AND psd.Refno='{Refno}'
AND psd.SuppColor='{SuppColor}'
";

                    DataRow row;
                    if (!MyUtility.Check.Seek(sqlcmd, out row, null))
                    {
                        MyUtility.Msg.WarningBox("Data not found!", "Refno");
                        return;
                    }

                    CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                    CurrentDetailData["Refno"] = row["Refno"];
                    CurrentDetailData["SuppColor"] = row["SuppColor"];
                    CurrentDetailData["POID"] = row["POID"];
                    CurrentDetailData["DescDetail"] = row["DescDetail"];
                    CurrentDetailData["@Qty"] = row["@Qty"];
                    CurrentDetailData["Use Unit"] = row["Use Unit"];
                    CurrentDetailData["AccuIssued"] = row["AccuIssued"];

                    CurrentDetailData["IssueQty"] = row["IssueQty"];
                    CurrentDetailData["Use Qty By Stock Unit"] = row["Use Qty By Stock Unit"];
                    CurrentDetailData["Stock Unit"] = row["Stock Unit"];
                    CurrentDetailData["Use Qty By Use Unit"] = row["Use Qty By Use Unit"];
                    CurrentDetailData["Use Unit"] = row["Use Unit"];
                    CurrentDetailData["Stock Unit Desc."] = row["Stock Unit Desc."];
                    CurrentDetailData["OutputQty"] = row["OutputQty"];
                    CurrentDetailData["Balance(Stock Unit)"] = row["Balance(Stock Unit)"];
                    CurrentDetailData["Location"] = row["Location"];
                    CurrentDetailData["MDivisionID"] = row["MDivisionID"];

                    #endregion
                    CurrentDetailData.EndEdit();
                }
            };
            ColorSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["SuppColor"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        //CurrentDetailData["Refno"] = "";
                        CurrentDetailData["SCIRefno"] = "";
                        CurrentDetailData["SuppColor"] = "";
                        CurrentDetailData["POID"] = "";
                        CurrentDetailData["DescDetail"] = "";
                        CurrentDetailData["@Qty"] = DBNull.Value;
                        CurrentDetailData["Use Unit"] = "";
                        CurrentDetailData["AccuIssued"] = DBNull.Value;

                        CurrentDetailData["IssueQty"] = DBNull.Value;
                        CurrentDetailData["Use Qty By Stock Unit"] = DBNull.Value;
                        CurrentDetailData["Stock Unit"] = "";
                        CurrentDetailData["Use Qty By Use Unit"] = DBNull.Value;
                        CurrentDetailData["Use Unit"] = "";
                        CurrentDetailData["Stock Unit Desc."] = "";
                        CurrentDetailData["OutputQty"] = DBNull.Value;
                        CurrentDetailData["Balance(Stock Unit)"] = DBNull.Value;
                        CurrentDetailData["Location"] = "";
                        CurrentDetailData["MDivisionID"] = "";
                    }
                    else
                    {
                        DataRow row;

                        string Refno = MyUtility.Check.Empty(CurrentDetailData["Refno"]) ? string.Empty : CurrentDetailData["Refno"].ToString();

                        string sqlcmd = $@"

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.SuppColor
, f.DescDetail
, [@Qty]=BOT.Val
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].SuppColor=PSD.SuppColor and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit]=0.00
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit]=0.00
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty]=0.00
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.SuppColor=psd.SuppColor
)StockUnit
OUTER APPLY(
	SELECT Val=SUM((SeamLength * Frequency * UseRatio) + Allowance)
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID)
	WHERE SCIRefNo = psd.SCIRefno AND SuppColor = psd.SuppColor
)BOT
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.Refno <> ''
AND psd.SuppColor='{e.FormattedValue}'

";


                        if (!MyUtility.Check.Empty(Refno))
                        {
                            sqlcmd += $"AND psd.Refno='{Refno}' ";
                        }
                        else
                        {
                            sqlcmd += $"AND psd.Refno <> '' ";
                        }

                        if (!MyUtility.Check.Seek(sqlcmd, out row, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "SuppColor");
                            return;
                        }
                        else
                        {
                            if (MyUtility.Check.Empty(Refno))
                            {
                                CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                                CurrentDetailData["SuppColor"] = e.FormattedValue;
                                //CurrentDetailData["DescDetail"] = row["DescDetail"];
                                CurrentDetailData["POID"] = this.poid;
                            }
                            else
                            {
                                CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                                CurrentDetailData["Refno"] = row["Refno"];
                                CurrentDetailData["SuppColor"] = row["SuppColor"];
                                CurrentDetailData["POID"] = row["POID"];
                                CurrentDetailData["DescDetail"] = row["DescDetail"];
                                CurrentDetailData["@Qty"] = row["@Qty"];
                                CurrentDetailData["Use Unit"] = row["Use Unit"];
                                CurrentDetailData["AccuIssued"] = row["AccuIssued"];

                                CurrentDetailData["IssueQty"] = row["IssueQty"];
                                CurrentDetailData["Use Qty By Stock Unit"] = row["Use Qty By Stock Unit"];
                                CurrentDetailData["Stock Unit"] = row["Stock Unit"];
                                CurrentDetailData["Use Qty By Use Unit"] = row["Use Qty By Use Unit"];
                                CurrentDetailData["Use Unit"] = row["Use Unit"];
                                CurrentDetailData["Stock Unit Desc."] = row["Stock Unit Desc."];
                                CurrentDetailData["OutputQty"] = row["OutputQty"];
                                CurrentDetailData["Balance(Stock Unit)"] = row["Balance(Stock Unit)"];
                                CurrentDetailData["Location"] = row["Location"];
                                CurrentDetailData["MDivisionID"] = row["MDivisionID"];
                            }
                            CurrentDetailData.EndEdit();
                        }
                    }
                }
            };
            #endregion

            #region issue Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings issueQty = new DataGridViewGeneratorNumericColumnSettings();
            issueQty.CellMouseDoubleClick += (s, e) =>
            {
                if (dtIssueBreakDown == null)
                {
                    MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                    return;
                }

                DoSubForm.IsSupportUpdate = false;
                OpenSubDetailPage();

                DataTable FinalSubDt;
                GetSubDetailDatas(out FinalSubDt);

                DataTable detail = (DataTable)detailgridbs.DataSource;
                foreach (DataRow dr in detail.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        GetSubDetailDatas(dr, out FinalSubDt);

                        dr["Qty"]= Math.Round(FinalSubDt.AsEnumerable()
                                                    .Where(row => row.RowState!=DataRowState.Deleted && !MyUtility.Check.Empty(row["Qty"]))
                                                    .Sum(row => Convert.ToDouble(row["Qty"].ToString()))
                                                    , 2);
                        dr["IssueQty"] = Math.Round(FinalSubDt.AsEnumerable()
                                                    .Where(row => row.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(row["Qty"]))
                                                    .Sum(row => Convert.ToDouble(row["Qty"].ToString()))
                                                    , 2);

                    }
                }
            };
            #endregion

            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), settings: RefnoSet) 
            .Text("SuppColor", header: "Color", width: Widths.AnsiChars(7),  settings: ColorSet) 
            .EditText("DescDetail", header: "Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true) 
            .Numeric("@Qty", header: "@Qty", width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10, iseditingreadonly: true)  
            .Numeric("AccuIssued", header: "Accu. Issued"+Environment.NewLine+"(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("IssueQty", header: "Issue Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6),decimal_places:2 , settings: issueQty, iseditingreadonly: true)
            .Numeric("Use Qty By Stock Unit", header: "Use Qty" + Environment.NewLine + "By Stock Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Text("Stock Unit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Use Qty By Use Unit", header: "Use Qty" + Environment.NewLine + "By Use Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Text("Use Unit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Stock Unit Desc.", header: "Stock Unit Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("OutputQty", header: "Output Qty" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Numeric("Balance(Stock Unit)", header: "Balance" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true) 
            ;
            #endregion 欄位設定


            #region 可編輯欄位變色
            this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["SuppColor"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["IssueQty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion 可編輯欄位變色
        }

        protected override void OpenSubDetailPage()
        {
            base.OpenSubDetailPage();
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
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "E";
            CurrentMaintain["issuedate"] = DateTime.Now;
            CurrentMaintain["combo"] = 0;
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
            DataTable result = null;


            if(((DataTable)detailgridbs.DataSource).AsEnumerable().Where(o=>o.RowState != DataRowState.Deleted).Count() == 0)
            {
                MyUtility.Msg.InfoBox("Detail can't be empty !!");
                return false;
            }

            #region 檢查不可為空

            string OrderID = this.CurrentMaintain["OrderID"].ToString();
            string IssueDate =MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]) ? string.Empty : this.CurrentMaintain["IssueDate"].ToString();

            if (MyUtility.Check.Empty(OrderID) || MyUtility.Check.Empty(IssueDate))
            {
                MyUtility.Msg.InfoBox("[SP#] , [Issue Date] can't be empty !!");
                return false;
            }

            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["IssueQty"]) || MyUtility.Check.Empty(dr["SCIRefNo"]) || MyUtility.Check.Empty(dr["SuppColor"]))
                {
                    MyUtility.Msg.InfoBox("[RefNo] , [Color] , [Issue Qty] can't be empty !!");
                    return false;
                }
            }
            #endregion

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IC", "Issue", (DateTime)CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;

                //assign 給detail table ID
                DataTable tmp = (DataTable)detailgridbs.DataSource;

                foreach (DataRow row in tmp.Rows)
                {
                    row.SetField("ID", tmpId);
                }

            }



            if (dtSizeCode != null && dtSizeCode.Rows.Count != 0)
            {
                if (checkByCombo.Checked == false)
                {
                    foreach (DataRow data in dtIssueBreakDown.ToList())
                    {
                        if (data.ItemArray[0].ToString() != txtOrderID.Text)
                            dtIssueBreakDown.Rows.Remove(data);
                    }
                }
                string sqlcmd;
                sqlcmd = string.Format(@";delete from dbo.issue_breakdown where id='{0}'
;WITH UNPIVOT_1
AS
(
SELECT * FROM #tmp
UNPIVOT
(
QTY
FOR SIZECODE IN ({1})
)
AS PVT
)
MERGE INTO DBO.ISSUE_BREAKDOWN T
USING UnPivot_1 S
ON T.ID = '{0}' AND T.ORDERID= S.OrderID AND T.ARTICLE = S.ARTICLE AND T.SIZECODE = S.SIZECODE
WHEN MATCHED THEN
UPDATE
SET QTY = S.QTY
WHEN NOT MATCHED THEN
INSERT (ID,ORDERID,ARTICLE,SIZECODE,QTY)
VALUES ('{0}',S.OrderID,S.ARTICLE,S.SIZECODE,S.QTY)
;delete from dbo.issue_breakdown where id='{0}' and qty = 0; ", CurrentMaintain["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1));

                string aaa = sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1).Replace("[", "").Replace("]", "");

                ProcessWithDatatable2(dtIssueBreakDown, "OrderID,Article," + aaa
                    , sqlcmd, out result, "#tmp");
            }

            // 取BarcodeNo
            IList<DataRow> listSubDetail = new List<DataRow>();
            DataTable dtTmp;
            foreach (DataRow dr in this.DetailDatas)
            {
                this.GetSubDetailDatas(dr, out dtTmp);

                foreach (DataRow subDr in dtTmp.Rows)
                {
                    listSubDetail.Add(subDr);
                }
            }

            DualResult resultBarcodeNo = Prgs.FillIssueDetailBarcodeNo(listSubDetail);

            if (!resultBarcodeNo)
            {
                return ShowErr(resultBarcodeNo);
            }

            //將Issue_Detail的數量更新Issue_Summary
            DataTable subDetail;
            DataTable detail = (DataTable)detailgridbs.DataSource;
            foreach (DataRow detailRow in detail.Rows)
            {
                if (detailRow.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                this.GetSubDetailDatas(detailRow, out subDetail);
                if (subDetail.Rows.Count == 0)
                {
                    detailRow["Qty"] = 0;
                    detailRow["IssueQty"] = 0;
                }
                else
                {
                    decimal detailQty = subDetail.AsEnumerable().Sum(s => s.RowState != DataRowState.Deleted ? (decimal)s["Qty"] : 0);
                    detailRow["IssueQty"] = detailQty;
                    detailRow["Qty"] = detailQty;
                }
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickDeleteAfter()
        {
            base.ClickDeleteAfter();
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return;
            }

            var dr = this.CurrentMaintain;
            if (null == dr) return;


            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = "";
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 檢查庫存項lock
            sqlcmd = $@"

SELECT   psd.Refno
		,psd.SuppColor
		,d.Seq1
		,d.seq2
FROM Issue i
INNER JOIN Issue_Summary s ON i.ID = s.ID 
INNER JOIN Issue_Detail d ON s.id=d.id AND s.Ukey = d.Issue_SummaryUkey
INNER JOIN FtyInventory f ON f.POID=s.Poid AND f.Seq1=d.Seq1 AND f.Seq2=d.Seq2
INNER JOIN PO_Supp_Detail psd ON psd.ID = s.Poid AND psd.SCIRefno = s.SCIRefno AND psd.SCIRefno = s.SCIRefno AND psd.SEQ1=d.Seq1 AND psd.Seq2=d.Seq2
WHERE i.Id = '{CurrentMaintain["id"]}' AND  f.lock = 1 
";

            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    //foreach (DataRow tmp in datacheck.Rows)
                    //{
                    //    ids += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} is locked!!" + Environment.NewLine;
                    //}
                    //MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");

                    var m = MyUtility.Msg.ShowMsgGrid(datacheck, "The following Thread has been Locked. can't confirm!!", "Material Locked");

                    m.Width = 850;
                    m.grid1.Columns[0].Width = 300;
                    m.grid1.Columns[1].Width = 100;
                    m.TopMost = true;
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"


SELECT   psd.Refno
		,psd.SuppColor
		,d.Seq1
		,d.seq2
		,[BulkQty]=isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
		,[IssueQty]=ISNULL(d.Qty ,0)
		,[Balance]=isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty
FROM Issue i
INNER JOIN Issue_Summary s ON i.ID = s.ID 
INNER JOIN Issue_Detail d ON s.id=d.id AND s.Ukey = d.Issue_SummaryUkey
INNER JOIN FtyInventory f ON f.POID=s.Poid AND f.Seq1=d.Seq1 AND f.Seq2=d.Seq2
INNER JOIN PO_Supp_Detail psd ON psd.ID = s.Poid AND psd.SCIRefno = s.SCIRefno AND psd.SCIRefno = s.SCIRefno AND psd.SEQ1=d.Seq1 AND psd.Seq2=d.Seq2
WHERE i.Id = '{0}'
AND(isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - ISNULL(d.Qty, 0)) < 0
", CurrentMaintain["id"]);

            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    //foreach (DataRow tmp in datacheck.Rows)
                    //{
                    //    ids += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} is less than issue Qty: {tmp["qty"]}" + Environment.NewLine;
                    //}
                    //MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");

                    var m = MyUtility.Msg.ShowMsgGrid(datacheck, "The following bulk stock is insufficient, can't confirm!!", "Balacne Qty is not enough");

                    m.Width = 850;
                    m.grid1.Columns[0].Width = 300;
                    m.grid1.Columns[1].Width = 100;
                    m.TopMost = true;
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"
update Issue 
set status = 'Confirmed'
    , ApvName = '{0}' 
    , ApvDate  = GETDATE()
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            #region -- 更新mdivisionpodetail B倉數 --
            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion
            #endregion


            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithDatatable
                        (datacheck, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
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
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            string sqlupd2_FIO = "";
            StringBuilder sqlupd2_B = new StringBuilder();


            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='New', ApvName = '' , ApvDate  = NULL, editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料


            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }

            var bsfio = (from m in datacheck.AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -(m.Field<decimal>("qty")),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = -(m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject
                        (bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
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

        }

        protected override bool ClickPrint()
        {
            labelConfirmed.Text = CurrentMaintain["status"].ToString();
            if (labelConfirmed.Text.ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            //--------------------------------------------------------------------------------------------//

            DataRow issue = this.CurrentMaintain;

            //上方欄位
            string ID = issue["ID"].ToString();
            string IssueDate = Convert.ToDateTime(issue["IssueDate"]).ToString("yyyy/MM/dd");
            string OrderID = this.txtOrderID.Text;
            string Line = this.displayLineNo.Text;
            string Remark = issue["Remark"].ToString();
            string poID = this.displayPOID.Text;

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", ID));
            pars.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
            pars.Add(new SqlParameter("@OrderID", OrderID));

            #region Title
            DataTable dt;
            DBProxy.Current.Select("", @"
select NameEN 
from MDivision 
where id = @MDivision", pars, out dt);
            string RptTitle = dt.Rows[0]["NameEN"].ToString();

            #endregion

            #region Body

            DataTable dtBody;
            DualResult result;

            string cmd = $@"
SELECT f.Refno
		,iis.SCIRefno 
		,iis.SuppColor 
		,[Seq]=iid.Seq1 +'-'+iid.Seq2
		,[Desc]=f.DescDetail
		,[Issue_Detail_Qty]=Cast( iid.Qty as int)
		,[Issue_Summary_Qty]=Cast( iis.Qty as int)
		,[Unit]=Unit.StockUnit
		,[UnitDesc]=Unit.Description
		,[Location]=ftd.MtlLocationID
INTO #tmp
FROM Issue_Summary iis WITH(NOLOCK)
INNER JOIN Issue_Detail iid WITH(NOLOCK) ON iis.Id = iid.Id AND iis.Ukey = iid.Issue_SummaryUkey
INNER JOIN Fabric f WITH(NOLOCK) ON f.SCIRefno = iis.SCIRefno
LEFT JOIN FtyInventory_Detail ftd WITH(NOLOCK) ON ftd.Ukey= iid.FtyInventoryUkey
OUTER APPLY(
	SELECT TOP 1 PSD.StockUnit  ,u.Description
	FROM PO_Supp_Detail PSD 
	INNER JOIN Unit u ON u.ID = PSD.StockUnit
	WHERE PSD.ID ='{poID}' AND PSD.SCIRefno=iis.SCIRefno AND PSD.SuppColor = iis.SuppColor
)Unit
WHERE iis.ID='{ID}'


SELECT [Refno]
/*
[Refno]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND SuppColor=t.SuppColor
					ORDER BY Seq
				), '' ,Refno )*/
		,SuppColor
		,Seq
		,[Desc]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND SuppColor=t.SuppColor
					ORDER BY Seq
				), '' ,t.[Desc] )
		,Issue_Detail_Qty
		,[Issue_Summary_Qty]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND SuppColor=t.SuppColor
					ORDER BY Seq
				) OR Issue_Detail_Qty = Issue_Summary_Qty, '' ,'= '+Cast(t.Issue_Summary_Qty as char))
		
		,[Unit]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND SuppColor=t.SuppColor
					ORDER BY Seq
				), '' , t.Unit)
		
		,[UnitDesc]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND SuppColor=t.SuppColor
					ORDER BY Seq
				), '' , t.UnitDesc)
		,[Location]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND SuppColor=t.SuppColor
					ORDER BY Seq
				), '' , t.Location)
FROM #tmp t

DROP TABLE #tmp
";
            result = DBProxy.Current.Select("", cmd, out dtBody);
            #endregion

            #region RDLC
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", ID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("IssueDate", IssueDate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("OrderID", OrderID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Line", Line));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            //report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("poID", poID));


            List<P33_PrintData> PrintDatas = dtBody.AsEnumerable().Select(o => new P33_PrintData()
            {
                RefNo = o["RefNo"].ToString().Trim(),
                Color = o["SuppColor"].ToString().Trim(),
                Seq = o["Seq"].ToString().Trim(),
                Desc = o["Desc"].ToString().Trim(),
                Issue_Detail_Qty = o["Issue_Detail_Qty"].ToString().Trim(),
                Issue_Summary_Qty = o["Issue_Summary_Qty"].ToString().Trim(),
                Unit = o["Unit"].ToString().Trim(),
                UnitDesc = o["UnitDesc"].ToString().Trim(),
                Location = o["Location"].ToString().Trim()
            }).ToList();

            report.ReportDataSource = PrintDatas;


            Type ReportResourceNamespace = typeof(P33_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P33_Print.rdlc";


            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;


            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();
            #endregion

            return base.ClickPrint();   
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
                this.txtOrderID.Text = string.Empty;
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

                this.displayLineNo.Text = string.Empty;
                return;
            }

            // 根據SP#，帶出這張訂單會用到的線材資訊(線的種類以及顏色)
            DualResult result = Detail_Reload();

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.displayLineNo.Text = MyUtility.GetValue.Lookup($@"
SELECT t.sewline + ',' 
FROM(SELECT DISTINCT o.sewline FROM dbo.issue_detail a WITH (nolock) 
INNER JOIN dbo.orders o WITH (nolock) ON a.poid = o.poid  
WHERE o.id = '{CurrentOrderID}'  AND o.sewline != '') t FOR xml path('')
");

            Ismatrix_Reload = true;
            IssueBreakDown_Reload();
            detailgridbs.Position = 0;
            detailgrid.Focus();
            detailgrid.CurrentCell = detailgrid[0, 0];
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

            this.HideNullColumn(gridIssueBreakDown);
        }


        private void btnAutoPick_Click(object sender, EventArgs e)
        {

            if (dtIssueBreakDown == null)
            {
                return;
            }
            List<IssueQtyBreakdown> modelList = new List<IssueQtyBreakdown>();

            //檢查是否有勾選Combo，處理傳入AutoPick資料篩選
            if (!checkByCombo.Checked && dtIssueBreakDown != null)
            {
                foreach (DataRow tempRow in dtIssueBreakDown.Rows)
                {
                    if (tempRow["OrderID"].ToString() != txtOrderID.Text.ToString())
                    {
                        foreach (DataColumn tempColumn in dtIssueBreakDown.Columns)
                        {
                            if ("Decimal" == tempRow[tempColumn].GetType().Name)
                                tempRow[tempColumn] = 0;
                        }
                    }
                }
            }

            var tmp = dtIssueBreakDown.AsEnumerable().Select(o => new { OrderID = o["OrderID"].ToString(), Article = o["Article"].ToString() }).ToList();

            foreach (var obj in tmp)
            {

                foreach (DataRow tempRow in dtIssueBreakDown.Rows)
                {
                    if (tempRow["OrderID"].ToString() == obj.OrderID && tempRow["Article"].ToString() == obj.Article)
                    {
                        IssueQtyBreakdown m = new IssueQtyBreakdown()
                        {
                            OrderID = obj.OrderID,
                            Article = obj.Article
                        };

                        int totalQty = 0;
                        foreach (DataColumn col in dtIssueBreakDown.Columns)
                        {
                            if ("Decimal" == tempRow[col].GetType().Name)
                            {
                                totalQty += Convert.ToInt32(tempRow[col]);
                            }
                        }
                        m.Qty = totalQty;
                        modelList.Add(m);
                    }
                }
            }

            var frm = new Sci.Production.Warehouse.P33_AutoPick(CurrentMaintain["id"].ToString(), this.poid, txtOrderID.Text.ToString(), dtIssueBreakDown, sbSizecode, checkByCombo.Checked , modelList);
            DialogResult result = frm.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DataTable _detail, _subDetail;
                _detail = (DataTable)detailgridbs.DataSource;


                //刪除表身重新匯入
                foreach (DataRow del in DetailDatas)
                {
                    del.Delete();
                }

                foreach (DataRow key in frm.importRows)
                {
                    string POID = key["POID"].ToString();
                    string SCIRefno = key["SCIRefno"].ToString();
                    string SuppColor = key["SuppColor"].ToString();
                    string Refno = key["Refno"].ToString();
                    string DescDetail = key["DescDetail"].ToString();
                    string @Qty = key["@Qty"].ToString();
                    string UseQtyByStockUnit = key["Use Qty By Stock Unit"].ToString();
                    string StockUnit = key["Stock Unit"].ToString();
                    string UseQtyByUseUnit = key["Use Qty By Use Unit"].ToString();
                    string UseUnit = key["Use Unit"].ToString();
                    string StockUnitDesc = key["Stock Unit Desc."].ToString();
                    string OutputQty = key["Output Qty(Garment)"].ToString();
                    decimal balance = (decimal)key["Bulk Balance(Stock Unit)"];
                    //string FtyInventoryUkey = key["FtyInventoryUkey"].ToString();
                    string AccuIssued = key["AccuIssued"].ToString();

                    DataRow nRow = _detail.NewRow();
                    nRow["ID"] = CurrentMaintain["ID"];

                    nRow["POID"] = POID;
                    nRow["SCIRefno"] = SCIRefno;
                    nRow["Refno"] = Refno;
                    nRow["SuppColor"] = SuppColor;
                    nRow["DescDetail"] = DescDetail;
                    nRow["@Qty"] =Convert.ToDecimal(Qty);
                    nRow["Use Qty By Stock Unit"] = Convert.ToDecimal(UseQtyByStockUnit);
                    nRow["Stock Unit"] = StockUnit;
                    nRow["Use Qty By Use Unit"] = Convert.ToDecimal(UseQtyByUseUnit);
                    nRow["Use Unit"] = UseUnit;
                    nRow["Stock Unit Desc."] = StockUnitDesc;
                    nRow["OutputQty"] = OutputQty;
                    nRow["Balance(Stock Unit)"] = balance;
                    nRow["AccuIssued"] = AccuIssued;

                    if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
                    {
                        //nRow["AccuIssued"] = 0.00;
                    }
                    else
                    {
                        AccuIssued = MyUtility.GetValue.Lookup($@"
select isnull(sum([IS].qty),0)
from dbo.issue I WITH (NOLOCK) 
inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
where I.type = 'E' and I.Status = 'Confirmed' 
and [IS].Poid='{POID}' AND [IS].SCIRefno='{SCIRefno}' AND [IS].SuppColor='{SuppColor}' and i.[EditDate]<'{Convert.ToDateTime(CurrentMaintain["AddDate"]).ToShortDateString()}'
");

                        nRow["AccuIssued"] = Convert.ToDecimal(AccuIssued);
                    }

                    _detail.Rows.Add(nRow);
                    decimal totalQty = 0;
                    if (GetSubDetailDatas(_detail.Rows[_detail.Rows.Count - 1], out _subDetail))
                    {
                        List<DataRow> issuedList = PublicPrg.Prgs.Thread_AutoPick(key, Convert.ToDecimal(AccuIssued));
                        foreach (var issued in issuedList)
                        {
                            totalQty += (decimal)issued["Qty"];
                            issued.AcceptChanges();
                            issued.SetAdded();
                            _subDetail.ImportRow(issued);
                        }
                        sum_subDetail(_detail.Rows[_detail.Rows.Count - 1], _subDetail);
                    }
                    //_subDetail.AcceptChanges();
                    _detail.Rows[_detail.Rows.Count - 1]["IssueQty"] = totalQty;
                    _detail.Rows[_detail.Rows.Count - 1]["Qty"] = totalQty;
                }

                detailgrid.SelectRowToNext();
                detailgrid.SelectRowToPrev();
            }

        }

        #endregion

        #region 自訂事件
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

            string POID = this.poid;

            // 回採購單找資料
            string sql = $@"

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.SuppColor
, f.DescDetail
, [@Qty]=BOT.Val
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].SuppColor=PSD.SuppColor and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit]=0.00
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit]=0.00
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty]=0.00
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.SuppColor=psd.SuppColor
)StockUnit
OUTER APPLY(
	SELECT Val=SUM((SeamLength * Frequency * UseRatio) + Allowance)
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID)
	WHERE SCIRefNo = psd.SCIRefno AND SuppColor = psd.SuppColor
)BOT
WHERE psd.id ='{POID}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.SuppColor <> ''
ORDER BY psd.SCIRefno,psd.SuppColor

";
            result = DBProxy.Current.Select(null, sql, out subData);

            if (subData.Rows.Count == 0)
            {
                txtOrderID.Text = "";
                return Result.F("No Issue Thread Data !");
            }

            foreach (DataRow dr in subData.Rows)
            {
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                if (detailDt!=null)
                {
                    DataRow ndr = detailDt.NewRow();

                    ndr["SCIRefno"] = dr["SCIRefno"];
                    ndr["Refno"] = dr["Refno"];
                    ndr["SuppColor"] = dr["SuppColor"];
                    ndr["POID"] = dr["POID"];
                    ndr["DescDetail"] = dr["DescDetail"];
                    ndr["@Qty"] = dr["@Qty"];
                    ndr["Use Unit"] = dr["Use Unit"];
                    ndr["AccuIssued"] = dr["AccuIssued"];

                    ndr["IssueQty"] = dr["IssueQty"];
                    ndr["Use Qty By Stock Unit"] = dr["Use Qty By Stock Unit"];
                    ndr["Stock Unit"] = dr["Stock Unit"];
                    ndr["Use Qty By Use Unit"] = dr["Use Qty By Use Unit"];
                    ndr["Use Unit"] = dr["Use Unit"];
                    ndr["Stock Unit Desc."] = dr["Stock Unit Desc."];
                    ndr["OutputQty"] = dr["OutputQty"]; 
                    ndr["Balance(Stock Unit)"] = dr["Balance(Stock Unit)"];
                    ndr["Location"] = dr["Location"];
                    ndr["MDivisionID"] = dr["MDivisionID"];
                    detailDt.Rows.Add(ndr);
                }

            }

            return Result.True;
        }

        private DualResult matrix_Reload()
        {
            if (EditMode == true && Ismatrix_Reload == false)
                return Result.True;

            Ismatrix_Reload = false;
            string sqlcmd;
            StringBuilder sbIssueBreakDown;
            DualResult result;

            string OrderID = txtOrderID.Text;

            sqlcmd = string.Format(@"select sizecode from dbo.order_sizecode WITH (NOLOCK) 
where id = (select poid from dbo.orders WITH (NOLOCK) where id='{0}') order by seq", OrderID);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dtSizeCode)))
            {
                ShowErr(sqlcmd, result);
                return Result.True;
            }
            if (dtSizeCode.Rows.Count == 0)
            {
                //MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", CurrentDataRow["orderid"]));               
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
                sbSizecode.Append(string.Format(@"[{0}],", dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
                sbSizecode2.Append(string.Format(@"{0},", dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }
            sbIssueBreakDown = new StringBuilder();
            sbIssueBreakDown.Append(string.Format(@";with Bdown as 
            (select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a WITH (NOLOCK) 
            inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
            where b.POID=(select poid from dbo.orders WITH (NOLOCK) where id = '{0}')
            )
            ,Issue_Bdown as
            (
            	select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
            	from Bdown full outer join (select * from dbo.Issue_Breakdown WITH (NOLOCK) where id='{1}') ib
            	on Bdown.orderid = ib.OrderID and Bdown.Article = ib.Article and Bdown.SizeCode = ib.SizeCode
            )
            select * from Issue_Bdown
            pivot
            (
            	sum(qty)
            	for sizecode in ({2})
            )as pvt
            order by [OrderID],[Article]", OrderID, CurrentMaintain["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));//.Replace("[", "[_")
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

        private void HideNullColumn(Win.UI.Grid grid)
        {

            List<string> nullCol = new List<string>();
            foreach (DataGridViewColumn Column in grid.Columns)
            {
                Column.Visible = true;
                int rowCount = 0;
                int nullCount = 0;
                string ColumnName = Column.Name;
                if (ColumnName != "Selected" && ColumnName != "Article" && ColumnName != "OrderID")
                {
                    foreach (DataGridViewRow Row in grid.Rows)
                    {
                        string val = Row.Cells[ColumnName].Value.ToString();
                        if (MyUtility.Check.Empty(val))
                        {
                            nullCount++;
                        }
                        rowCount++;
                    }
                    if (rowCount == nullCount)
                    {
                        nullCol.Add(ColumnName);
                    }
                }
            }

            foreach (var col in nullCol)
            {
                grid.Columns[col].Visible = false;
            }
        }

        public static void ProcessWithDatatable2(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp")
        {
            result = null;
            StringBuilder sb = new StringBuilder();
            if (temptablename.TrimStart().StartsWith("#"))
            {
                sb.Append(string.Format("create table {0} (", temptablename));
            }
            else
            {
                sb.Append(string.Format("create table #{0} (", temptablename));
            }
            string[] cols = tmp_columns.Split(',');
            for (int i = 0; i < cols.Length; i++)
            {
                if (MyUtility.Check.Empty(cols[i])) continue;
                switch (Type.GetTypeCode(source.Columns[cols[i]].DataType))
                {
                    case TypeCode.Boolean:
                        sb.Append(string.Format("[{0}] bit", cols[i]));
                        break;

                    case TypeCode.Char:
                        sb.Append(string.Format("[{0}] varchar(1)", cols[i]));
                        break;

                    case TypeCode.DateTime:
                        sb.Append(string.Format("[{0}] datetime", cols[i]));
                        break;

                    case TypeCode.Decimal:
                        sb.Append(string.Format("[{0}] numeric(24,8)", cols[i]));
                        break;

                    case TypeCode.Int32:
                        sb.Append(string.Format("[{0}] int", cols[i]));
                        break;

                    case TypeCode.String:
                        sb.Append(string.Format("[{0}] varchar(max)", cols[i]));
                        break;

                    case TypeCode.Int64:
                        sb.Append(string.Format("[{0}] bigint", cols[i]));
                        break;
                    default:
                        break;
                }
                if (i < cols.Length - 1) { sb.Append(","); }
            }
            sb.Append(")");

            System.Data.SqlClient.SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }
                using (System.Data.SqlClient.SqlBulkCopy bulkcopy = new System.Data.SqlClient.SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = 60;
                    if (temptablename.TrimStart().StartsWith("#"))
                    {
                        bulkcopy.DestinationTableName = temptablename.Trim();
                    }
                    else
                    {
                        bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
                    }

                    for (int i = 0; i < cols.Length; i++)
                    {
                        bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
                    }
                    bulkcopy.WriteToServer(source);
                    bulkcopy.Close();
                }
                result2 = DBProxy.Current.SelectByConn(conn, sqlcmd, out result);
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        static void sum_subDetail(DataRow target, DataTable source)
        {

            target["Qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("Qty"));

        }
        #endregion
    }

    public class IssueQtyBreakdown
    {
        public string OrderID { get; set; }

        public string Article { get; set; }

        public int Qty { get; set; }
    }
}
