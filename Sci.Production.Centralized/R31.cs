using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class R31 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DataTable dtAllData;
        private StringBuilder sqlWHERE = new StringBuilder();

        /// <inheritdoc/>
        public R31(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlWHERE.Clear();
            #region "Where 條件"
            if (!MyUtility.Check.Empty(this.dateDisposeDate.Value1))
            {
                this.sqlWHERE.Append(string.Format(" and c.DisposeDate >= cast('{0}' as date)", Convert.ToDateTime(this.dateDisposeDate.Value1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateDisposeDate.Value2))
            {
                this.sqlWHERE.Append(string.Format(" and c.DisposeDate <= cast('{0}' as date)", Convert.ToDateTime(this.dateDisposeDate.Value2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this.sqlWHERE.Append(string.Format(" and oqs.BuyerDelivery >= cast('{0}' as date)", Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this.sqlWHERE.Append(string.Format(" and oqs.BuyerDelivery <= cast('{0}' as date)", Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.txtPONoStart.Text))
            {
                this.sqlWHERE.Append(string.Format(" and o.CustPONo >= '{0}'", this.txtPONoStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPONoEnd.Text))
            {
                this.sqlWHERE.Append(string.Format(" and o.CustPONo <= '{0}'", this.txtPONoEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
            {
                this.sqlWHERE.Append(string.Format(" and o.ID >= '{0}'", this.txtSPNoStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
            {
                this.sqlWHERE.Append(string.Format(" and o.ID <= '{0}'", this.txtSPNoEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.txtClogReason.TextBox1.Text))
            {
                this.sqlWHERE.Append(string.Format(" and cr.ID ='{0}'", this.txtClogReason.TextBox1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                this.sqlWHERE.Append(string.Format(" and o.BrandID = '{0}'", this.txtbrand.Text));
            }

            if (!MyUtility.Check.Empty(this.comboM.Text))
            {
                this.sqlWHERE.Append(string.Format(" and o.MDivisionID = '{0}'", this.comboM.Text));
            }

            if (!MyUtility.Check.Empty(this.comboCancel.Text))
            {
                this.sqlWHERE.Append(string.Format(" and o.Junk = '{0}'", this.comboCancel.Text == "Y" ? 1 : 0));
            }
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlcmd = new StringBuilder();
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘

            #region "SQL"
            sqlcmd.Append(
            $@"
            select o.MDivisionID
	            ,o.FactoryID
	            ,[SP] = o.ID
	            ,[PackingListID] = pd.ID
	            ,cd.CTNStartNO
	            ,cd.ID
	            ,c.DisposeDate
	            ,oqs.BuyerDelivery
	            ,o.BrandID
	            ,o.CustPONo
	            ,[TTLPackQty] = sum(pd.ShipQty)
	            ,[CPUPrice] = o.CPU * o.CPUFactor
	            ,[totalCpu] = sum(pd.ShipQty) * o.CPU * o.CPUFactor
	            ,o.PoPrice
	            ,[TotalAmount] = sum(pd.ShipQty) * o.PoPrice
	            ,[Cancelled] = iif(o.Junk=1,'Y','N')
	            ,[IsPulloutcompleted] = case when o.Finished = 1 and o.Qty > podd.ShipQty
								            then 'S'
							                   when o.Finished = 1 and o.Qty = podd.ShipQty
								            then 'Y'
							             else 'N' end

	            ,cr.Description
	            ,c.Remark
	            ,[QtyPerSize] = pp.QtyPerSize
            from ClogGarmentDispose_Detail cd WITH (NOLOCK)
            inner join ClogGarmentDispose c WITH (NOLOCK) on cd.ID =c.ID
            inner join ClogReason cr WITH (NOLOCK) on c.ClogReasonID = cr.ID and cr.Type = 'GD'
            inner join PackingList_Detail pd WITH (NOLOCK) on cd.PackingListID = pd.ID and cd.CTNStartNO = pd.CTNStartNo
            inner join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
            left join Order_QtyShip oqs WITH (NOLOCK) on pd.OrderID = oqs.Id and pd.OrderShipmodeSeq = oqs.Seq
            outer apply(
	            select ShipQty = sum(podd.ShipQty) 
	            from Pullout_Detail_Detail podd WITH (NOLOCK) 
	            inner join Order_Qty oq WITH (NOLOCK) on oq.id=podd.OrderID 
	            and podd.Article= oq.Article and podd.SizeCode=oq.SizeCode
	            where podd.OrderID = o.ID
            )podd
            outer apply(
	            select [QtyPerSize] = (
		            Stuff((
			            select concat('/',SizeCode+':'+ convert(varchar(10),ShipQty))
			            from(
				            select distinct pp.SizeCode,pp.ShipQty
				            from PackingList_Detail pp WITH (NOLOCK)
				            where pp.ID= pd.ID and pp.CTNStartNo= pd.CTNStartNo
			            )s
			            for xml path('')
	            ),1,1,''))
            )pp
            where 1=1 
            {this.sqlWHERE}
            group by o.MDivisionID,o.FactoryID,o.ID,pd.ID,cd.CTNStartNO,cd.ID,c.DisposeDate,oqs.BuyerDelivery
            ,o.BrandID,o.CustPONo,o.CPU,o.CPUFactor,o.PoPrice,o.Junk,cr.Description,c.Remark
            ,o.Finished,o.Qty,podd.ShipQty,[QtyPerSize]");
            #endregion

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            List<string> connectionString = CentralizedClass.AllFactoryConnectionString();

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            foreach (string conString in connectionString)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    DualResult result = DBProxy.Current.SelectByConn(conn, sqlcmd.ToString(), out this.printData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    if (this.printData != null && this.printData.Rows.Count > 0)
                    {
                        if (this.dtAllData == null)
                        {
                            this.dtAllData = this.printData;
                        }
                        else
                        {
                            this.dtAllData.Merge(this.printData);
                        }
                    }
                }
            }

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dtAllData == null || this.dtAllData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dtAllData.Rows.Count);

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\Centrallize_R31.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.dtAllData, string.Empty, "Centrallize_R31.xltx", 1, false, null, excel); // 將datatable copy to excel
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Centrallize_R31");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
