using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P01_BatchCloseRowMaterial : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtBatch;

        public P01_BatchCloseRowMaterial()
        {
            InitializeComponent();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            MyUtility.Tool.SetupCombox(cbbCategory, 2, 1, ",All,B,Bulk,S,Sample,M,Material");
            cbbCategory.SelectedIndex = 0;

        }
        public P01_BatchCloseRowMaterial(DataRow master, DataTable detail)
            : this()
        {
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            QueryData();
        }

        private void QueryData()
        {
            DateTime? pulloutdate1, pulloutdate2, buyerDelivery1, buyerDelivery2;
            StringBuilder strSQLCmd = new StringBuilder();
            pulloutdate1 = PullOutdateRange.Value1;
            pulloutdate2 = PullOutdateRange.Value2;
            buyerDelivery1 = BuyerDeliverydateRange.Value1;
            buyerDelivery2 = BuyerDeliverydateRange.Value2;
            String sp1 = this.txtSP1.Text.TrimEnd();
            String sp2 = this.txtSP2.Text.TrimEnd();
            String category = this.cbbCategory.SelectedValue.ToString();
            String style = txtstyle1.Text;
            String brand = txtbrand1.Text;
            String factory = txtmfactory1.Text;

            if (MyUtility.Check.Empty(PullOutdateRange.Value1) &&
                MyUtility.Check.Empty(BuyerDeliverydateRange.Value1) &&
                (MyUtility.Check.Empty(txtSP1.Text) || MyUtility.Check.Empty(txtSP2.Text)))
            {
                MyUtility.Msg.WarningBox("< Pullout Date > & < Buyer Delivery > & < SP# > can't be empty!!");
                return;
            }

            strSQLCmd.Append(string.Format(@"with cte_order as
(
	select distinct poid from dbo.orders 
	where orders.Finished=1 and Orders.WhseClose is null and MDivisionID = '{0}'
", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(pulloutdate1))
            {
                strSQLCmd.Append(string.Format(@" and pulloutdate between '{0}' and '{1}'"
                , Convert.ToDateTime(pulloutdate1).ToString("d"), Convert.ToDateTime(pulloutdate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                strSQLCmd.Append(string.Format(@" and BuyerDelivery between '{0}' and '{1}'"
                , Convert.ToDateTime(buyerDelivery1).ToString("d"), Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sp1) || !MyUtility.Check.Empty(sp2))
            {
                strSQLCmd.Append(string.Format(@" and id between '{0}' and '{1}' ", sp1, sp2.PadLeft(13, 'Z')));
            }
            if (!MyUtility.Check.Empty(category))
            {
                strSQLCmd.Append(string.Format(@" and category = '{0}' ", category));
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

            strSQLCmd.Append(string.Format(@"
                )
                select 0 Selected
                , m.poid
                ,x.FactoryID,x.Category,x.StyleID,x.BrandID,x.BuyerDelivery,m.ActPulloutDate,m.ppicClose
                ,dbo.getPOComboList(m.poid,m.poid) [PoCombo] 
                from (
                    select a.POID
                    ,max(a.ActPulloutDate) ActPulloutDate, max(a.gmtclose) ppicClose
                    from dbo.orders a 
                    inner join cte_order b on b.POID = a.POID
                    where  a.MDivisionID = '{0}' and a.Finished=1 and a.WhseClose is null 
                    group by a.poid
                ) m
                cross apply (select * from dbo.orders a1 where a1.id=m.POID) x
                order by m.POID", Sci.Env.User.Keyword
            ));

            MyUtility.Msg.WaitWindows("Data Loading....");

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtBatch))
            {
                if (dtBatch.Rows.Count == 0) { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtBatch;
                //dtFtyinventory.DefaultView.Sort = "fabrictype,poid,seq1,seq2,location,dyelot,roll";
            }
            else { ShowErr(strSQLCmd.ToString(), result); }
            MyUtility.Msg.WaitClear();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
                .Text("factoryid", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(8)) //1
                .Text("category", header: "Category", iseditingreadonly: true, width: Widths.AnsiChars(8)) //4
                .Text("styleid", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(20)) //3
                .Text("brandid", header: "Brand", iseditingreadonly: true)      //5
                .Date("buyerdelivery", header: "Buyer Delivery", iseditingreadonly: true)      //5
                .Date("buyerdelivery", header: "Last Pullout Date", iseditingreadonly: true)      //5
                .Date("ppicclose", header: "Last PPIC Close", iseditingreadonly: true)      //5
               .EditText("pocombo", header: "PO Combo", iseditingreadonly: true, width: Widths.AnsiChars(25))
               ; //8

            // 全選
            checkBox1.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetCheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };

            // 全不選
            checkBox2.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetUncheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
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
                MyUtility.Msg.WaitWindows(string.Format("Closing R/Mtl of {0}.", tmp["poid"]));
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
            }
            //this.QueryData();
            MyUtility.Msg.InfoBox("Finish closing R/Mtl!!");
            MyUtility.Msg.WaitClear();
        }

        private void btnToEexcel_Click(object sender, EventArgs e)
        {
            if (dtBatch != null && dtBatch.Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(dtBatch, "");
            }
        }
    }
}
