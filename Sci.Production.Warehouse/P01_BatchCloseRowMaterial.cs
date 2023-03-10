using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P01_BatchCloseRowMaterial : Win.Subs.Base
    {
        private DataTable dtBatch;
        private string DataType;
        private Func<string, bool> whP01_CheckBarcodeEmpty;

        /// <inheritdoc/>
        public P01_BatchCloseRowMaterial(string dataType, Func<string, bool> whP01_CheckBarcodeEmpty)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, ",All,B,Bulk,S,Sample,M,Material");
            this.comboCategory.SelectedIndex = 0;
            this.DataType = dataType;
            this.whP01_CheckBarcodeEmpty = whP01_CheckBarcodeEmpty;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.QueryData(false);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public void QueryData(bool autoQuery)
        {
            DateTime? pulloutdate1, pulloutdate2, buyerDelivery1, buyerDelivery2;
            StringBuilder strSQLCmd = new StringBuilder();
            pulloutdate1 = this.datePullOutDate.Value1;
            pulloutdate2 = this.datePullOutDate.Value2;
            buyerDelivery1 = this.dateBuyerDelivery.Value1;
            buyerDelivery2 = this.dateBuyerDelivery.Value2;
            string sp1 = this.txtSPNoStart.Text.TrimEnd();
            string sp2 = this.txtSPNoEnd.Text.TrimEnd();
            string category = this.comboCategory.SelectedValue.ToString();
            string style = this.txtstyle.Text;
            string brand = this.txtbrand.Text;
            string factory = this.txtmfactory.Text;

            if (!autoQuery &&
                MyUtility.Check.Empty(this.datePullOutDate.Value1) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) || MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Pullout Date > & < Buyer Delivery > & < SP# > can't be empty!!");
                return;
            }

            strSQLCmd.Append(string.Format(
                @"
with cte_order as(
    select distinct poid 
    from dbo.orders WITH (NOLOCK) 
    LEFT JOIN dbo.Factory on orders.FtyGroup=Factory.ID 
    where orders.Finished=1 and Orders.WhseClose is null and Factory.MDivisionID = '{0}'", Env.User.Keyword));

            if (!MyUtility.Check.Empty(pulloutdate1))
            {
                strSQLCmd.Append(string.Format(
                    @" and orders.ActPulloutDate between '{0}' and '{1}'",
                    Convert.ToDateTime(pulloutdate1).ToString("yyyy/MM/dd"), Convert.ToDateTime(pulloutdate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                strSQLCmd.Append(string.Format(
                    @" and BuyerDelivery between '{0}' and '{1}'",
                    Convert.ToDateTime(buyerDelivery1).ToString("yyyy/MM/dd"), Convert.ToDateTime(buyerDelivery2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(sp1) || !MyUtility.Check.Empty(sp2))
            {
                strSQLCmd.Append(string.Format(@" and poid between '{0}' and '{1}' ", sp1, sp2));
            }

            if (!MyUtility.Check.Empty(style))
            {
                strSQLCmd.Append(string.Format(@" and styleid = '{0}' ", style));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                strSQLCmd.Append(string.Format(@" and brandid = '{0}' ", brand));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                strSQLCmd.Append(string.Format(@" and factoryid = '{0}' ", factory));
            }

            string categorySql = string.Empty;
            if (!MyUtility.Check.Empty(category))
            {
                categorySql = string.Format(@" and category = '{0}' ", category);
            }

            strSQLCmd.Append(string.Format(
                @"
    EXCEPT
	select temp.POID 
    from ( 
        select distinct poid 
        from dbo.orders WITH (NOLOCK) 
        LEFT JOIN dbo.Factory on orders.FtyGroup=Factory.ID
        where orders.Finished=0 and Orders.WhseClose is null and Factory.MDivisionID = '{0}'
	) temp
)

select * into #cte_temp 
from cte_order

select  fty.POID
INTO #LockList
from #cte_temp cte 
left join FtyInventory fty WITH (NOLOCK) on cte.POID=fty.POID 
where fty.Lock=1  AND fty.StockType = 'B'

select  0 Selected
        , m.poid
    ,x.FactoryID
    ,Category =case when x.Category='B'then 'Bulk'
			        when x.Category='M'then 'Material'
			        when x.Category='O'then 'Other'
			        when x.Category='S'then 'Sample'
			        end
    ,x.OrderTypeID
    ,x.StyleID
    ,x.BrandID
    ,x.BuyerDelivery
    ,m.ActPulloutDate
    ,m.ppicClose
    ,dbo.getPOComboList(m.poid,m.poid) [PoCombo] 
    ,[MCHandle] = dbo.getPass1_ExtNo((select MCHandle from orders where id=m.POID))
    ,x.WhseClose
	,[Lock]= Cast( IIF(EXISTS( select 1 from #LockList l where l.POID = m.poID) , 1,0) as bit)
from (
    select  a.POID
            ,max(a.ActPulloutDate) ActPulloutDate
            , max(a.gmtclose) ppicClose
    from dbo.orders a WITH (NOLOCK) 
    LEFT JOIN dbo.Factory f on a.FtyGroup=f.ID
    inner join #cte_temp  b on b.POID = a.POID
    where  f.MDivisionID = '{0}' and a.Finished=1 and a.WhseClose is null 
    group by a.poid
) m
cross apply (
    select a1.* 
    from dbo.orders a1 WITH (NOLOCK) 
    LEFT JOIN dbo.Factory on a1.FtyGroup=Factory.ID 
    where a1.id=m.POID and Factory.MDivisionID = '{0}' {1} 
) x
order by m.POID
Drop table #cte_temp;", Env.User.Keyword, categorySql));

            this.ShowWaitMessage("Data Loading....");

            DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtBatch))
            {
                if (this.dtBatch.Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtBatch;
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }

            this.HideWaitMessage();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorCheckBoxColumnSettings col_chk = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_chk.CellEditable += (s, e) =>
            {
                if (MyUtility.Check.Empty(this.listControlBindingSource1))
                {
                    return;
                }

                DataRow dr = this.gridBatchCloseRowMaterial.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetBool(dr["Lock"]))
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.gridBatchCloseRowMaterial.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchCloseRowMaterial.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchCloseRowMaterial)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_chk)
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 1
                .Text("factoryid", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 1
                .Text("category", header: "Category", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 4
                .Text("OrderTypeID", header: "Order Type", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 4
                .Text("styleid", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 3
                .Text("brandid", header: "Brand", iseditingreadonly: true) // 5
                .Date("buyerdelivery", header: "Buyer Delivery", iseditingreadonly: true) // 5
                .Date("ActPulloutDate", header: "Last Pullout Date", iseditingreadonly: true) // 5
                .Date("ppicclose", header: "Last PPIC Close", iseditingreadonly: true) // 5
                .EditText("pocombo", header: "PO Combo", iseditingreadonly: true, width: Widths.AnsiChars(25))
                .Text("MCHandle", header: "MC Handle", iseditingreadonly: true, width: Widths.AnsiChars(30))
                .CheckBox("Lock", header: "Material Lock", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0) // 0
               ; // 8
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBatchCloseRMTL_Click(object sender, EventArgs e)
        {
            // listControlBindingSource1.EndEdit();
            this.gridBatchCloseRowMaterial.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to close this R/Mtl?");
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            List<string> lockPOID = new List<string>();

            foreach (DataRow tmp in dr2)
            {
                this.ShowWaitMessage(string.Format("Closing R/Mtl of {0}.", tmp["poid"]));
                bool existsFtyInventoryLock = MyUtility.Check.Seek($"select 1 from FtyInventory with (nolock) where POID = '{tmp["poid"]}' and StockType='B' and Lock = 1");

                // 檢查 Barcode不可為空
                bool checkBarcodeEmptyResult = this.whP01_CheckBarcodeEmpty(tmp["poid"].ToString());
                if (!checkBarcodeEmptyResult)
                {
                    this.HideWaitMessage();
                    return;
                }

                if (existsFtyInventoryLock)
                {
                    lockPOID.Add(MyUtility.Convert.GetString(tmp["poid"]));
                    continue;
                }

                if (lockPOID.Count > 0)
                {
                    continue;
                }

                string subTransferId = MyUtility.GetValue.GetID(Env.User.Keyword + "AC", "SubTransfer", DateTime.Now);
                if (MyUtility.Check.Empty(subTransferId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return;
                }

                DualResult result;
                #region store procedure parameters

                DataTable dtSubTransfer_Detail = new DataTable();
                Exception errMsg = null;
                using (TransactionScope transactionscope = new TransactionScope())
                {
                    try
                    {
                        List<SqlParameter> sqlPar = new List<SqlParameter>();
                        sqlPar.Add(new SqlParameter("@poid", tmp["POID"].ToString()));
                        sqlPar.Add(new SqlParameter("@MDivisionid", Env.User.Keyword));
                        sqlPar.Add(new SqlParameter("@factoryid", Env.User.UserID));
                        sqlPar.Add(new SqlParameter("@loginid", Env.User.UserID));
                        sqlPar.Add(new SqlParameter("@NewID", subTransferId));
                        sqlPar.Add(new SqlParameter("@FirstClose", true));
                        #endregion
                        if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_WarehouseClose", sqlPar)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = DBProxy.Current.Select(null, $"select * from SubTransfer_Detail where id = '{subTransferId}'", out dtSubTransfer_Detail)))
                        {
                            throw result.GetException();
                        }

                        // 上方 Auto Create P25 Confrim 後, 寫入新的 BarCode
                        if (!(result = Prgs.UpdateWH_Barcode(true, dtSubTransfer_Detail, "P25", out bool fromNewBarcode)))
                        {
                            throw result.GetException();
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }

                if (!MyUtility.Check.Empty(errMsg))
                {
                    this.ShowErr(errMsg);
                    return;
                }

                // SubTransfer_Detail
                if (dtSubTransfer_Detail.Rows.Count > 0)
                {
                    Gensong_AutoWHFabric.Sent(false, dtSubTransfer_Detail, "P25", EnumStatus.New, EnumStatus.Confirm);
                    Vstrong_AutoWHAccessory.Sent(false, dtSubTransfer_Detail, "P25", EnumStatus.New, EnumStatus.Confirm);
                }
            }

            // WHClose 批次 只要傳一次
            DataTable dtFilter = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(x => x["Selected"].EqualDecimal(1)).CopyToDataTable();
            DataTable dtMain = dtFilter.DefaultView.ToTable(true, "POID", "WhseClose");
            Gensong_AutoWHFabric.SentWHClose(false, dtMain);
            Vstrong_AutoWHAccessory.SentWHClose(false, dtMain);

            if (lockPOID.Count > 0)
            {
                this.HideWaitMessage();
                string msg = "Material locked. Can not close." + Environment.NewLine + lockPOID.JoinToString(Environment.NewLine);
                MyUtility.Msg.ErrorBox(msg);
                return;
            }

            MyUtility.Msg.InfoBox("Finish closing R/Mtl!!");
            this.HideWaitMessage();

            this.QueryData(true);
        }

        private void BtnToEexcel_Click(object sender, EventArgs e)
        {
            string cmd = @"select [SP#]=poid,[Factory]=FactoryID
,Category,[OrderType]=OrderTypeID,Style=StyleID,Brand=BrandID,[BuyerDelivery]=BuyerDelivery,[Last Pullout Date]=ActPulloutDate
,[Last PPIC Close] = ppicClose,[PO Combo] = PoCombo

from #tmp";
            DataTable printDatatable;

            if (this.dtBatch != null && this.dtBatch.Rows.Count > 0)
            {
                MyUtility.Tool.ProcessWithDatatable(this.dtBatch, string.Empty, cmd, out printDatatable, "#Tmp");
                Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(printDatatable);
                sdExcel.Save(Class.MicrosoftFile.GetName("Warehouse_P01_BatchCloseRowMaterial"));
            }
        }

        private void GridBatchCloseRowMaterial_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.gridBatchCloseRowMaterial.ValidateControl();
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                if (dt != null || dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (!MyUtility.Convert.GetBool(item["Lock"]))
                        {
                            bool old = MyUtility.Convert.GetBool(item["Selected"]);
                            item["Selected"] = !old;
                        }
                        else
                        {
                            item["Selected"] = false;
                        }

                        item.EndEdit();
                    }
                }
            }
        }
    }
}
