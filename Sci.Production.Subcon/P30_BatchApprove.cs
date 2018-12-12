using Ict;
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

            if (RealLockedList.Rows.Count==0)
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
                        INNER JOIN Orders o On o.ID=lp.OrderId
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
