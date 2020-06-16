using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.Automation;

namespace Sci.Production.Warehouse
{
    public partial class P01_BatchCloseRowMaterial : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable [] dtBatch;

        public P01_BatchCloseRowMaterial()
        {
            InitializeComponent();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            MyUtility.Tool.SetupCombox(comboCategory, 2, 1, ",All,B,Bulk,S,Sample,M,Material");
            comboCategory.SelectedIndex = 0;

        }

        public P01_BatchCloseRowMaterial(DataRow master, DataTable detail)
            : this()
        {
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            QueryData(false);
        }

        public void QueryData(bool AutoQuery)
        {
            DateTime? pulloutdate1, pulloutdate2, buyerDelivery1, buyerDelivery2;
            StringBuilder strSQLCmd = new StringBuilder();
            pulloutdate1 = datePullOutDate.Value1;
            pulloutdate2 = datePullOutDate.Value2;
            buyerDelivery1 = dateBuyerDelivery.Value1;
            buyerDelivery2 = dateBuyerDelivery.Value2;
            String sp1 = this.txtSPNoStart.Text.TrimEnd();
            String sp2 = this.txtSPNoEnd.Text.TrimEnd();
            String category = this.comboCategory.SelectedValue.ToString();
            String style = txtstyle.Text;
            String brand = txtbrand.Text;
            String factory = txtmfactory.Text;

            if (!AutoQuery &&
                MyUtility.Check.Empty(datePullOutDate.Value1) &&
                MyUtility.Check.Empty(dateBuyerDelivery.Value1) &&
                (MyUtility.Check.Empty(txtSPNoStart.Text) || MyUtility.Check.Empty(txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Pullout Date > & < Buyer Delivery > & < SP# > can't be empty!!");
                return;
            }

            strSQLCmd.Append(string.Format(@"
with cte_order as(
    select distinct poid 
    from dbo.orders WITH (NOLOCK) 
    LEFT JOIN dbo.Factory on orders.FtyGroup=Factory.ID 
    where orders.Finished=1 and Orders.WhseClose is null and Factory.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(pulloutdate1))
            {
                strSQLCmd.Append(string.Format(@" and orders.ActPulloutDate between '{0}' and '{1}'"
                , Convert.ToDateTime(pulloutdate1).ToString("d"), Convert.ToDateTime(pulloutdate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                strSQLCmd.Append(string.Format(@" and BuyerDelivery between '{0}' and '{1}'"
                , Convert.ToDateTime(buyerDelivery1).ToString("d"), Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sp1) || !MyUtility.Check.Empty(sp2))
            {
                strSQLCmd.Append(string.Format(@" and poid between '{0}' and '{1}' ", sp1, sp2));
                //strSQLCmd.Append(string.Format(@" and id between '{0}' and '{1}' ", sp1, sp2));
                //strSQLCmd.Append(string.Format(@" and id between '{0}' and '{1}' ", sp1, sp2.PadLeft(13, 'Z')));
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

            string categorySql = "";
            if (!MyUtility.Check.Empty(category))
            {
                categorySql = (string.Format(@" and category = '{0}' ", category));
            }

            strSQLCmd.Append(string.Format(@"
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
Drop table #cte_temp;", Sci.Env.User.Keyword, categorySql));

            this.ShowWaitMessage("Data Loading....");

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtBatch))
            {
                if (dtBatch[1].Rows.Count == 0) {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
                listControlBindingSource1.DataSource = dtBatch[1];
            }
            else { ShowErr(strSQLCmd.ToString(), result); }
            this.HideWaitMessage();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchCloseRowMaterial.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridBatchCloseRowMaterial.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridBatchCloseRowMaterial)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
                .Text("factoryid", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(8)) //1
                .Text("category", header: "Category", iseditingreadonly: true, width: Widths.AnsiChars(8)) //4
                .Text("OrderTypeID", header: "Order Type", iseditingreadonly: true, width: Widths.AnsiChars(15)) //4
                .Text("styleid", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(20)) //3
                .Text("brandid", header: "Brand", iseditingreadonly: true)      //5
                .Date("buyerdelivery", header: "Buyer Delivery", iseditingreadonly: true)      //5
                .Date("ActPulloutDate", header: "Last Pullout Date", iseditingreadonly: true)      //5
                .Date("ppicclose", header: "Last PPIC Close", iseditingreadonly: true)      //5
                .EditText("pocombo", header: "PO Combo", iseditingreadonly: true, width: Widths.AnsiChars(25))
                .Text("MCHandle", header: "MC Handle", iseditingreadonly: true, width: Widths.AnsiChars(30))
               ; //8
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBatchCloseRMTL_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            gridBatchCloseRowMaterial.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to close this R/Mtl?");
            if (dResult.ToString().ToUpper() == "NO") return;

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
                sp_mdivision.Value = Sci.Env.User.Keyword;
                cmds.Add(sp_mdivision);
                System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
                sp_factory.ParameterName = "@factoryid";
                sp_factory.Value = Sci.Env.User.Factory;
                cmds.Add(sp_factory);
                System.Data.SqlClient.SqlParameter sp_loginid = new System.Data.SqlClient.SqlParameter();
                sp_loginid.ParameterName = "@loginid";
                sp_loginid.Value = Sci.Env.User.UserID;
                cmds.Add(sp_loginid);
                #endregion
                if (!(result = DBProxy.Current.ExecuteSP("", "dbo.usp_WarehouseClose", cmds)))
                {
                    //MyUtility.Msg.WarningBox(result.Messages[1].ToString()); 
                    Exception ex = result.GetException();
                    MyUtility.Msg.WarningBox(ex.Message);
                    //return;
                }

                #region Sent WHClose to Gensong
                if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                {
                    string strPOID = tmp["poid"].ToString();
                    Task.Run(() => new Gensong_AutoWHFabric().SentWHCloseToGensongAutoWHFabric(strPOID))
                   .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }
                #endregion
            }
            //this.QueryData();
            MyUtility.Msg.InfoBox("Finish closing R/Mtl!!");
            this.HideWaitMessage();

            QueryData(true);
        }

        private void btnToEexcel_Click(object sender, EventArgs e)
        {
            string cmd = @"select [SP#]=poid,[Factory]=FactoryID
,Category,[OrderType]=OrderTypeID,Style=StyleID,Brand=BrandID,[BuyerDelivery]=BuyerDelivery,[Last Pullout Date]=ActPulloutDate
,[Last PPIC Close] = ppicClose,[PO Combo] = PoCombo

from #tmp";
            DataTable printDatatable; 

            if (dtBatch != null && dtBatch[1].Rows.Count > 0)
            {
                MyUtility.Tool.ProcessWithDatatable(dtBatch[1], "",cmd , out printDatatable, "#Tmp");
                Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(printDatatable);
                sdExcel.Save(Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P01_BatchCloseRowMaterial"));
            }
        }
    }
}
