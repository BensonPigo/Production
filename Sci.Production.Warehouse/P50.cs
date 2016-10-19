using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    public partial class P50 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("Type='F' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            //
        }

        public P50(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='F' and id='{0}'", transID);
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MyUtility.Tool.SetupCombox(cbbStockType, 2, 1, "B,Bulk,I,Inventory");
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "F";
            CurrentMaintain["IssueDate"] = DateTime.Now;
            CurrentMaintain["stocktype"] = "B";
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox3.Focus();
                return false;
            }


            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty"
                        , row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "WSF", "StockTaking", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .Text("Location", header: "Book Location", iseditingreadonly: true)    //4
            .Numeric("qtybefore", header: "Book Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //5
            .Numeric("qtyafter", header: "Actual Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
            .Numeric("variance", header: "Variance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
            .Text("refno", header: "Ref#", iseditingreadonly: true)    //8
            .Text("Colorid", header: "Color", iseditingreadonly: true)    //9
            .Text("stockunit", header: "Stock Unit", iseditingreadonly: true)    //10
            .ComboBox("FabricType", header: "Fabric Type", iseditable: false).Get(out cbb_fabrictype)    //11
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //12
            
            ;     //
            #endregion 欄位設定
            cbb_fabrictype.DataSource = new BindingSource(di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;
            DualResult result;
            #region store procedure parameters
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter();
            sp_StocktakingID.ParameterName = "@StocktakingID";
            sp_StocktakingID.Value = dr["id"].ToString();
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
            if (!(result = DBProxy.Current.ExecuteSP("", "dbo.usp_StocktakingEncode",cmds)))
            {
                //MyUtility.Msg.WarningBox(result.Messages[1].ToString()); 
                Exception ex = result.GetException();
                MyUtility.Msg.WarningBox(ex.Message);
                return;
            }
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.MDivisionID
,a.PoId,a.Seq1,a.Seq2
,left(a.seq1+' ',3)+a.Seq2 as seq
,a.Roll
,a.Dyelot
,(select t.MtlLocationID+',' from (select mtllocationid from dbo.ftyinventory_detail fd where fd.Ukey = a.FtyInventoryUkey) t 
	for xml path('')) location
,a.QtyBefore
,a.QtyAfter
,a.QtyAfter - a.QtyBefore as variance
,a.StockType
,p1.Refno
,p1.colorid
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.ukey
,a.ftyinventoryukey
from dbo.StockTaking_detail as a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
            
        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P50_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void cbbStockType_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(cbbStockType.SelectedValue) && cbbStockType.SelectedValue != cbbStockType.OldValue)
            {
                if (detailgridbs.DataSource != null)
                {
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
                }
            }
        }

        protected override bool ClickPrint()
        {
            P50_Print p = new P50_Print(this.CurrentDataRow);
            p.ShowDialog();
            
            
            return true;

        }
       
    }
}