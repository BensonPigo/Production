﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P30_BatchApprove : Sci.Win.Forms.Base
    {
        Action delegateAct;
        DataTable LocalPOs, LocalPO_Details;

        public P30_BatchApprove(Action reload)
        {
            InitializeComponent();
            this.EditMode = true;
            this.delegateAct = reload;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridHead.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridHead)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false)//.Get(out col_chk)
                .Text("ID", header: "PO#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Factory", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Date("IssueDate", header: "Issue Date", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Currency", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("VatRate", header: "Vat Rate (%)", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                .Numeric("TotalAmount", header: "Total Amount", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;

            this.gridBody.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridBody)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Style", header: "Style", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("ColorShade", header: "Color Shade", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Unit", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                .Text("RequestID", header: "RequestID", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;


            for (int i = 0; i < this.gridHead.Columns.Count; i++)
            {
                this.gridHead.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }


            for (int i = 0; i < this.gridBody.Columns.Count; i++)
            {
                this.gridBody.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            Query();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Empty;
            DualResult result;

            if (LocalPOs == null || this.LocalPOs.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first.");
                return;
            }

            if (LocalPOs.AsEnumerable().Where(o => (int)o["Selected"] == 1).Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first.");
                return;
            }

            //取得勾選資料並檢查，得到確定是Locked的資料
            DataTable SelectedList = LocalPOs.AsEnumerable().Where(o => (int)o["Selected"] == 1).CopyToDataTable();
            DataTable RealLockedList = CheckSelectedData(SelectedList);

            if (RealLockedList.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data.");
                return;
            }

            List<string> IdList = RealLockedList.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

            sqlCmd = $" UPDATE LocalPO SET Status='Approved', ApvName='{Env.User.UserID}',ApvDate=GETDATE() WHERE ID IN ('{IdList.JoinToString("','")}')";

            using (TransactionScope _transactionscope = new TransactionScope())
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlCmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlCmd, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();

                    Query();

                    MyUtility.Msg.InfoBox("Sucessful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Empty;
            DualResult result;
            DataTable PrictData;

            try
            {
                //取得Locked資料
                if (LocalPOs == null || this.LocalPOs.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("No Data!");
                    return;
                }


                this.ShowWaitMessage("Data Loading...");

                //取得勾選資料並檢查，得到確定是Locked的資料
                DataTable RealLockedList = CheckSelectedData(LocalPOs);

                List<string> IdList = RealLockedList.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

                #region 組合SQL

                sqlCmd = $@"
                        --1. 先處理標準價問題
                        --根據表頭LocalPO的ID，整理出Category、POID、OrderID
                        SELECT DISTINCT [ArtworkTypeID]=a.Category  ,[POID]=ad.POID  ,[OrderId]=ad.OrderID 
                        INTO #tmp_AllOrders 
                        FROM LocalPO a 
                        INNER JOIN LocalPO_Detail ad ON a.ID=ad.ID
                        WHERE a.ID IN( '{IdList.JoinToString("','")}')



                        --列出採購價的清單（尚未總和）
                        SELECT  ap.ID
		                        ,ap.Category
		                        ,Orders.POID
		                        ,[OID]=apd.OrderId
		                        ,apd.Qty * apd.Price * dbo.getRate('FX',ap.CurrencyID,'USD',ap.issuedate) PO_amt
                        INTO #total_PO
                        from LocalPO ap WITH (NOLOCK) 
                        INNER JOIN LocalPO_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
                        INNER JOIN  Orders WITH (NOLOCK) on orders.id = apd.orderid
                        WHERE  EXiSTS  ( 
				                        SELECT ArtworkTypeID,POID 
				                        FROM #tmp_AllOrders 
				                        WHERE ArtworkTypeID= ap.Category  AND POID=Orders.POID)


                        --計算採購價總合
                        SELECT 
                                o.BrandID 
                                ,o.StyleID  
                                ,t.ArtworkTypeID  
                                ,t.POID  
                                ,[OrderID]=o.id
                                ,[stdPrice]=IIF(Standard.order_qty=0,0, round(Standard.order_amt/Standard.order_qty,4) )
                        INTO #StdPriceTable
                        FROM #tmp_AllOrders t
                        INNER JOIN Orders o WITH (NOLOCK) on o.id = t.OrderId
                        OUTER APPLY(--標準價
	                                SELECT orders.POID
	                                ,sum(orders.qty) order_qty        --實際訂單數量
	                                ,sum(orders.qty*Price) order_amt  --外發成本
	                                FROM orders WITH (NOLOCK) 
	                                INNER JOIN Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	                                WHERE POID= t.POID                   --相同母單
			                        AND ArtworkTypeID= t.ArtworkTypeID   --相同加工
	                                GROUP BY orders.poid,ArtworkTypeID
                        ) Standard



                        --2.開始產生報表需要的欄位
                        SELECT 
                                 [Factory]=l.FactoryID
                                ,[OriginalFactory]=o.FactoryID
                                ,[LPONO]=l.Id
                                ,[MasterSP]=o.POID
                                ,[SP]=ld.OrderId
                                ,[Style]=o.StyleID
                                ,[SciDelivery]=o.SciDelivery
                                ,[BuyerDelivery]=o.BuyerDelivery
                                ,[SewInLine]=o.SewInLine
                                ,[Buyer]=ld.BuyerID
                                ,[Season]=o.SeasonID
                                ,[Category]=l.Category
                                ,[Supp]=l.LocalSuppID
                                ,[IssueDate]=l.IssueDate
                                ,[Delivery]=ld.Delivery
                                ,[Ref# (Code)]=ld.Refno
                                ,[ColorShade]=ld.ThreadColorID
                                ,[Description]=li.Description
                                ,[PoQty]=ld.Qty
                                ,[Unit]=ld.UnitId
                                ,[Currency]=l.CurrencyId
                                ,[Price]= round(ld.Price,4)
                                ,[StdPrice]= round(std.stdPrice,4) 
                                ,[Amount]=round(ld.qty * ld.Price,4) 
                                ,[VatRate(%)]=round(l.VatRate,2)
                                ,[VatAmt]=round((ld.Qty * ld.Price) * l.VatRate / 100 , 4 )
                                ,[Total]= round((ld.qty * ld.Price) + ((ld.Qty * ld.Price) * l.VatRate / 100),4)  --Total = Amount + Vat Amt
                                ,[RequestID]=ld.RequestID
                                ,[Remark]=ld.Remark

                        FROM LocalPO l
                        INNER JOIN LocalPO_Detail ld ON l.Id=ld.Id
                        LEFT JOIN Orders o ON o.ID= ld.OrderId
                        LEFT JOIN LocalItem li ON li.RefNo=ld.RefNo
                        OUTER APPLY(
	                        SELECT stdPrice FROM #StdPriceTable WHERE OrderID=ld.OrderId
                        )std
                        WHERE l.ID IN( '{IdList.JoinToString("','")}')

                        DROP TABLE #tmp_AllOrders  ,#total_PO,#StdPriceTable";

                #endregion

                result = DBProxy.Current.Select(null, sqlCmd, out PrictData);
                if (!result)
                {
                    ShowErr(result);
                }


                MyUtility.Excel.CopyToXls(PrictData, string.Empty, "Subcon_P30_LockList.xltx", 1);

                this.HideWaitMessage();
            }
            catch (Exception ex)
            {
                ShowErr(ex);
            }


        }

        protected override void OnFormDispose()
        {
            base.OnFormDispose();

            //避免使用者沒按Close直接按 XX 關閉不會Reload
            this.delegateAct();
        }

        #region 自訂事件

        private void Query()
        {
            DataSet LocalPOs_And_Details = null;
            this.LocalPOs = null;
            this.LocalPO_Details = null;
            string sqlCmd = string.Empty;

            #region 組合SQL
            sqlCmd = @"
                        --表頭
                        SELECT 
                             [Selected]=0
                            ,[ID]=ID
                            ,[Factory]=FactoryID
                            ,[IssueDate]=IssueDate
                            ,[Category]=Category
                            ,[Supplier]=LocalSuppID
                            ,[Currency]=CurrencyID
                            ,[Amount]=Amount
                            ,[VatRate]=VatRate
                            ,[TotalAmount]=Amount + Vat
                        INTO #LocalPOs
                        FROm LocalPO
                        WHERE Status ='Locked'

                        SELECT * FROM #LocalPOs

                        --表身
                        SELECT 
                                 [ID]=lp.ID
                                ,[OrderId]=lp.OrderId
                                ,[Style]=o.StyleID
                                ,[Refno]=lp.Refno
                                ,[ColorShade]=lp.ThreadColorID
                                ,[Qty]=lp.Qty
                                ,[Unit]=lp.UnitId
                                ,[Price]=lp.Price
                                ,[Amount]=lp.Price * lp.Qty
                                ,[RequestID]=lp.RequestID
                                ,[Remark]=lp.Remark
                        FROm LocalPO l
                        INNER JOIN LocalPO_Detail lp ON l.ID=lp.ID
                        LEFT JOIN Orders o On o.ID=lp.OrderId
                        WHERE     l.Status ='Locked'
                              AND lp.ID IN( SELECT ID FROM #LocalPOs)

                        DROP TABLE #LocalPOs";
            #endregion

            try
            {

                if (!SQL.Selects("", sqlCmd, out LocalPOs_And_Details))
                {
                    MyUtility.Msg.WarningBox(sqlCmd, "Query error!!");
                    return;
                }

                if (LocalPOs_And_Details.Tables.Count == 0)
                {
                    return;
                }

                //將Table從DataSet切分出來
                LocalPOs = LocalPOs_And_Details.Tables[0];
                LocalPOs.TableName = "Master";

                LocalPO_Details = LocalPOs_And_Details.Tables[1];
                LocalPO_Details.TableName = "Detail";

                //建立Relation，連動兩個Grid
                DataRelation relation = new DataRelation("LocalPORelation"
                                                         , new DataColumn[] { LocalPOs.Columns["ID"] }
                                                         , new DataColumn[] { LocalPO_Details.Columns["ID"] });

                LocalPOs_And_Details.Relations.Add(relation);

                //DataSource必須Reset
                if (listControlBindingSource_Master.DataSource != null)
                {
                    listControlBindingSource_Master.DataSource = null;
                }
                if (listControlBindingSource_Detail.DataSource != null)
                {
                    listControlBindingSource_Detail.DataSource = null;
                }

                listControlBindingSource_Master.DataSource = LocalPOs_And_Details;
                listControlBindingSource_Master.DataMember = "Master";

                listControlBindingSource_Detail.DataSource = listControlBindingSource_Master;
                listControlBindingSource_Detail.DataMember = "LocalPORelation";

                this.gridHead.AutoResizeColumns();
                this.gridBody.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                ShowErr(ex);
            }

        }

        private DataTable CheckSelectedData(DataTable SelectedList)
        {
            string sqlCmd = string.Empty;
            DataTable RealLockedList;

            List<string> IdList = SelectedList.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

            sqlCmd = $" SELECT ID FROm LocalPO WHERE Status='Locked' AND ID IN ('{IdList.JoinToString("','")}')";

            //Form並不是最新的狀態，避免Form上的Status與DB歧異，因此回去撈出真正Locked的LocalPO
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out RealLockedList);
            if (!result)
            {
                ShowErr(result);
                return null;
            }
            return RealLockedList;
        }
        #endregion

    }
}
