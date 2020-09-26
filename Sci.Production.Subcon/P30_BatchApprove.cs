using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P30_BatchApprove : Win.Forms.Base
    {
        private Action delegateAct;
        private DataTable LocalPOs;
        private DataTable LocalPO_Details;

        public P30_BatchApprove(Action reload)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.delegateAct = reload;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridHead.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridHead)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false) // .Get(out col_chk)
                .Text("ID", header: "PO#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Factory", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Date("IssueDate", header: "Issue Date", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Currency", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 4, iseditingreadonly: true)
                .Numeric("VatRate", header: "Vat Rate (%)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                .Numeric("TotalAmount", header: "Total Amount", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 4, iseditingreadonly: true)
                ;

            this.gridBody.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridBody)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Style", header: "Style", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("ColorShade", header: "Color Shade", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Unit", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 4, iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 4, iseditingreadonly: true)
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

            this.Query();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Empty;
            DualResult result;

            if (this.LocalPOs == null || this.LocalPOs.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first.");
                return;
            }

            if (this.LocalPOs.AsEnumerable().Where(o => (int)o["Selected"] == 1).Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first.");
                return;
            }

            // 取得勾選資料並檢查，得到確定是Locked的資料
            DataTable selectedList = this.LocalPOs.AsEnumerable().Where(o => (int)o["Selected"] == 1).CopyToDataTable();
            DataTable realLockedList = this.CheckSelectedData(selectedList);

            if (realLockedList.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data.");
                return;
            }

            List<string> idList = realLockedList.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

            sqlCmd = $" UPDATE LocalPO SET Status='Approved', ApvName='{Env.User.UserID}',ApvDate=GETDATE() WHERE ID IN ('{idList.JoinToString("','")}')";

            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlCmd)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlCmd, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    this.Query();
                    this.delegateAct();
                    MyUtility.Msg.InfoBox("Sucessful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Empty;
            DualResult result;
            DataTable prictData;

            try
            {
                // 取得Locked資料
                if (this.LocalPOs == null || this.LocalPOs.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("No Data!");
                    return;
                }

                this.ShowWaitMessage("Data Loading...");

                // 取得勾選資料並檢查，得到確定是Locked的資料
                DataTable realLockedList = this.CheckSelectedData(this.LocalPOs);

                List<string> idList = realLockedList.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

                #region 組合SQL

                sqlCmd = $@"
                        --1. 先處理標準價問題
                        --根據表頭LocalPO的ID，整理出Category、POID、OrderID
                        SELECT DISTINCT [ArtworkTypeID]=a.Category  ,[POID]=ad.POID  ,[OrderId]=ad.OrderID 
                        INTO #tmp_AllOrders 
                        FROM LocalPO a 
                        INNER JOIN LocalPO_Detail ad ON a.ID=ad.ID
                        WHERE a.ID IN( '{idList.JoinToString("','")}')

                        --計算採購價總合
                        SELECT   [Category] = t.ArtworkTypeID  
                                ,[OrderID] = o.id
                                ,[stdPrice] = IIF(Standard.order_qty=0,0, round(Standard.order_amt/Standard.order_qty,4) )
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
	                        SELECT stdPrice FROM #StdPriceTable WHERE OrderID=ld.OrderId AND Category = l.Category
                        )std
                        WHERE l.ID IN( '{idList.JoinToString("','")}')

                        DROP TABLE #tmp_AllOrders, #StdPriceTable";

                #endregion

                result = DBProxy.Current.Select(null, sqlCmd, out prictData);
                if (!result)
                {
                    this.ShowErr(result);
                }

                MyUtility.Excel.CopyToXls(prictData, string.Empty, "Subcon_P30_LockList.xltx", 1);

                this.HideWaitMessage();
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        #region 自訂事件

        private void Query()
        {
            DataSet localPOs_And_Details = null;
            this.LocalPOs = null;
            this.LocalPO_Details = null;
            string sqlCmd = string.Empty;

            #region 組合SQL
            sqlCmd = $@"
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
                 if (!SQL.Selects(string.Empty, sqlCmd, out localPOs_And_Details))
                {
                    MyUtility.Msg.WarningBox(sqlCmd, "Query error!!");
                    return;
                }

                 if (localPOs_And_Details.Tables.Count == 0)
                {
                    return;
                }

                // 將Table從DataSet切分出來
                 this.LocalPOs = localPOs_And_Details.Tables[0];
                 this.LocalPOs.TableName = "Master";

                 this.LocalPO_Details = localPOs_And_Details.Tables[1];
                 this.LocalPO_Details.TableName = "Detail";

                // 建立Relation，連動兩個Grid
                 DataRelation relation = new DataRelation(
                     "LocalPORelation",
                     new DataColumn[] { this.LocalPOs.Columns["ID"] },
                     new DataColumn[] { this.LocalPO_Details.Columns["ID"] });

                 localPOs_And_Details.Relations.Add(relation);

                // DataSource必須Reset
                 if (this.listControlBindingSource_Master.DataSource != null)
                {
                    this.listControlBindingSource_Master.DataSource = null;
                }

                 if (this.listControlBindingSource_Detail.DataSource != null)
                {
                    this.listControlBindingSource_Detail.DataSource = null;
                }

                 this.listControlBindingSource_Master.DataSource = localPOs_And_Details;
                 this.listControlBindingSource_Master.DataMember = "Master";

                 this.listControlBindingSource_Detail.DataSource = this.listControlBindingSource_Master;
                 this.listControlBindingSource_Detail.DataMember = "LocalPORelation";

                 this.gridHead.AutoResizeColumns();
                 this.gridBody.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private DataTable CheckSelectedData(DataTable selectedList)
        {
            string sqlCmd = string.Empty;
            DataTable realLockedList;

            List<string> idList = selectedList.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

            sqlCmd = $" SELECT ID FROm LocalPO WHERE Status='Locked' AND ID IN ('{idList.JoinToString("','")}')";

            // Form並不是最新的狀態，避免Form上的Status與DB歧異，因此回去撈出真正Locked的LocalPO
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out realLockedList);
            if (!result)
            {
                this.ShowErr(result);
                return null;
            }

            return realLockedList;
        }
        #endregion

    }
}
