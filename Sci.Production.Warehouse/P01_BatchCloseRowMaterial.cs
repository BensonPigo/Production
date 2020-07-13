using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P01_BatchCloseRowMaterial : Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable[] dtBatch;

        public P01_BatchCloseRowMaterial()
        {
            this.InitializeComponent();
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, ",All,B,Bulk,S,Sample,M,Material");
            this.comboCategory.SelectedIndex = 0;
        }

        public P01_BatchCloseRowMaterial(DataRow master, DataTable detail)
            : this()
        {
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            this.QueryData(false);
        }

        public void QueryData(bool AutoQuery)
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

            if (!AutoQuery &&
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
                    Convert.ToDateTime(pulloutdate1).ToString("d"), Convert.ToDateTime(pulloutdate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                strSQLCmd.Append(string.Format(
                    @" and BuyerDelivery between '{0}' and '{1}'",
                    Convert.ToDateTime(buyerDelivery1).ToString("d"), Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sp1) || !MyUtility.Check.Empty(sp2))
            {
                strSQLCmd.Append(string.Format(@" and poid between '{0}' and '{1}' ", sp1, sp2));

                // strSQLCmd.Append(string.Format(@" and id between '{0}' and '{1}' ", sp1, sp2));
                // strSQLCmd.Append(string.Format(@" and id between '{0}' and '{1}' ", sp1, sp2.PadLeft(13, 'Z')));
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
        ,fty.Seq1
        ,fty.Seq2 
from #cte_temp cte 
left join FtyInventory fty WITH (NOLOCK) on cte.POID=fty.POID 
where fty.Lock=1 and StockType='B'

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
from (
    select  a.POID
            ,max(a.ActPulloutDate) ActPulloutDate
            , max(a.gmtclose) ppicClose
    from dbo.orders a WITH (NOLOCK) 
    LEFT JOIN dbo.Factory f on a.FtyGroup=f.ID
    inner join (
        select poid from #cte_temp 
		EXCEPT
		select distinct fty.POID 
        from #cte_temp cte 
		left join FtyInventory fty WITH (NOLOCK) on cte.POID=fty.POID 
		where fty.Lock=1 and StockType='B'
	) b on b.POID = a.POID
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
                if (this.dtBatch[1].Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtBatch[1];
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }

            this.HideWaitMessage();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchCloseRowMaterial.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchCloseRowMaterial.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchCloseRowMaterial)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
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
               ; // 8
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBatchCloseRMTL_Click(object sender, EventArgs e)
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

            foreach (DataRow tmp in dr2)
            {
                this.ShowWaitMessage(string.Format("Closing R/Mtl of {0}.", tmp["poid"]));
                DualResult result;
                #region store procedure parameters
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter();
                sp_StocktakingID.ParameterName = "@poid";
                sp_StocktakingID.Value = tmp["poid"].ToString();
                cmds.Add(sp_StocktakingID);
                System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
                sp_mdivision.ParameterName = "@MDivisionid";
                sp_mdivision.Value = Env.User.Keyword;
                cmds.Add(sp_mdivision);
                System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
                sp_factory.ParameterName = "@factoryid";
                sp_factory.Value = Env.User.Factory;
                cmds.Add(sp_factory);
                System.Data.SqlClient.SqlParameter sp_loginid = new System.Data.SqlClient.SqlParameter();
                sp_loginid.ParameterName = "@loginid";
                sp_loginid.Value = Env.User.UserID;
                cmds.Add(sp_loginid);
                #endregion
                if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_WarehouseClose", cmds)))
                {
                    // MyUtility.Msg.WarningBox(result.Messages[1].ToString());
                    Exception ex = result.GetException();
                    MyUtility.Msg.WarningBox(ex.Message);

                    // return;
                }
            }

            // this.QueryData();
            MyUtility.Msg.InfoBox("Finish closing R/Mtl!!");
            this.HideWaitMessage();

            this.QueryData(true);
        }

        private void btnToEexcel_Click(object sender, EventArgs e)
        {
            string cmd = @"select [SP#]=poid,[Factory]=FactoryID
,Category,[OrderType]=OrderTypeID,Style=StyleID,Brand=BrandID,[BuyerDelivery]=BuyerDelivery,[Last Pullout Date]=ActPulloutDate
,[Last PPIC Close] = ppicClose,[PO Combo] = PoCombo

from #tmp";
            DataTable printDatatable;

            if (this.dtBatch != null && this.dtBatch[1].Rows.Count > 0)
            {
                MyUtility.Tool.ProcessWithDatatable(this.dtBatch[1], string.Empty, cmd, out printDatatable, "#Tmp");
                Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(printDatatable);
                sdExcel.Save(Class.MicrosoftFile.GetName("Warehouse_P01_BatchCloseRowMaterial"));
            }
        }
    }
}
