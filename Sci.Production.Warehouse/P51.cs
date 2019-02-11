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

namespace Sci.Production.Warehouse
{
    public partial class P51 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            di_fabrictype.Add("O", "Other");
        }

        public P51(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("id='{0}'", transID);
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
            MyUtility.Tool.SetupCombox(comboStockType, 2, 1, "B,Bulk,I,Inventory");
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["IssueDate"] = DateTime.Now;
            CurrentMaintain["stocktype"] = "B";
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
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
                dateIssueDate.Focus();
                return false;
            }


            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["poid"]) || MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Seq1 or Seq2 can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["stocktype"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Stock Type can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Roll and Dyelot can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["roll"] = "";
                    row["dyelot"] = "";
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }



            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "SB", "StockTaking", (DateTime)CurrentMaintain["Issuedate"]);
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
            CurrentDetailData["stocktype"] = CurrentMaintain["stocktype"].ToString();
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            DataRow dr;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            #region Seq 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    //string sqlcmd = "";
                    IList<DataRow> x;
                   
                        Sci.Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(CurrentDetailData["poid"].ToString(), CurrentDetailData["seq"].ToString(), "f.MDivisionID = '{1}'");
                        DialogResult result = selepoitem.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        x = selepoitem.GetSelecteds();

                    CurrentDetailData["seq"] = x[0]["seq"];
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    CurrentDetailData["fabrictype"] = x[0]["fabrictype"];
                    CurrentDetailData["colorid"] = x[0]["colorid"];
                    CurrentDetailData["refno"] = x[0]["refno"];
                    CurrentDetailData["qtybefore"] = 0m;
                    CurrentDetailData["qtyafter"] = 0m;
                    CurrentDetailData["ftyinventoryukey"] = 0;
                    CurrentDetailData["roll"] = "";
                    CurrentDetailData["dyelot"] = "";
                    CurrentDetailData["Location"] = "";
                    CurrentDetailData["description"] = "";
                    CurrentDetailData.EndEdit();
                }
            };
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["seq"] = "";
                        CurrentDetailData["seq1"] = "";
                        CurrentDetailData["seq2"] = "";
                        CurrentDetailData["stockunit"] = "";
                        CurrentDetailData["fabrictype"] = "";
                        CurrentDetailData["colorid"] = "";
                        CurrentDetailData["refno"] = "";
                        CurrentDetailData["qtybefore"] = 0m;
                        CurrentDetailData["qtyafter"] = 0m;
                        CurrentDetailData["ftyinventoryukey"] = 0;
                        CurrentDetailData["roll"] = "";
                        CurrentDetailData["dyelot"] = "";
                        CurrentDetailData["Location"] = "";
                        CurrentDetailData["description"] = "";
                    }
                    else
                    {
                        //check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' });
                        if (seq.Length < 2)
                        {
                            e.Cancel = true;  
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            
                            return;
                        }

                        if (!MyUtility.Check.Seek(string.Format(Prgs.selePoItemSqlCmd +
                                    @"and f.MDivisionID = '{1}' and p.seq1 ='{2}' and p.seq2 = '{3}'", CurrentDetailData["poid"], Sci.Env.User.Keyword, seq[0], seq[1]), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq"); 
                            return;
                        }
                        else
                        {
                            CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                            CurrentDetailData["seq1"] = seq[0];
                            CurrentDetailData["seq2"] = seq[1];
                            CurrentDetailData["stockunit"] = dr["stockunit"];
                            CurrentDetailData["fabrictype"] = dr["fabrictype"];
                            CurrentDetailData["colorid"] = dr["colorid"];
                            CurrentDetailData["refno"] = dr["refno"];
                            CurrentDetailData["qtybefore"] = 0m;
                            CurrentDetailData["qtyafter"] = 0m;
                            CurrentDetailData["ftyinventoryukey"] = 0;
                            CurrentDetailData["roll"] = "";
                            CurrentDetailData["dyelot"] = "";
                            CurrentDetailData["Location"] = "";
                            CurrentDetailData["description"] = "";
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            #region roll# 右鍵開窗 & valid
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right 
                    && !MyUtility.Check.Empty(CurrentDetailData["poid"]) 
                    && !MyUtility.Check.Empty(CurrentDetailData["seq"]) )
                {
                    //bug fix:364: WAREHOUSE_P51_Warehouse Backward Stocktaking
                    if (MyUtility.Check.Empty(CurrentDetailData["stocktype"])) CurrentDetailData["stocktype"] = CurrentMaintain["stocktype"].ToString();

                    string sqlcmd = string.Format(@"select a.ukey,a.roll,a.dyelot,inqty-a.outqty+a.adjustqty qty 
                                 ,dbo.Getlocation(a.ukey) as location 
                                 ,dbo.getmtldesc('{0}','{1}','{2}',2,0) as [description]
                                        from dbo.ftyinventory a WITH (NOLOCK) 
                                        where poid='{0}' and seq1='{1}' and seq2='{2}' 
                                        and stocktype='{3}' and lock =0", CurrentDetailData["poid"]
                                                                        ,CurrentDetailData["seq1"]
                                                                        ,CurrentDetailData["seq2"]
                                                                        ,CurrentDetailData["stocktype"]);
                    IList<DataRow> x;
                    Sci.Win.Tools.SelectItem selepoitem2 = new Win.Tools.SelectItem(sqlcmd
                        , "Ukey,Roll,Dyelot,Balance,Location", CurrentDetailData["roll"].ToString());

                    DialogResult result = selepoitem2.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = selepoitem2.GetSelecteds();

                    CurrentDetailData["ftyinventoryukey"] = x[0]["ukey"];
                    CurrentDetailData["roll"] = x[0]["roll"];
                    CurrentDetailData["dyelot"] = x[0]["dyelot"];
                    CurrentDetailData["qtybefore"] = x[0]["qty"];
                    CurrentDetailData["qtyafter"] = 0m;
                    CurrentDetailData["Location"] = x[0]["Location"];
                    CurrentDetailData["description"] = x[0]["description"];
                }
            };
            ts2.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (MyUtility.Check.Empty(CurrentDetailData["poid"]) || MyUtility.Check.Empty(CurrentDetailData["seq"]))
                {
                    MyUtility.Msg.WarningBox("Please fill < SP# > , < Seq > first!", "Warning");
                    return;
                }
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["roll"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["ftyinventoryukey"] = 0;
                        CurrentDetailData["roll"] = "";
                        CurrentDetailData["dyelot"] = "";
                        CurrentDetailData["qtybefore"] = 0m;
                        CurrentDetailData["qtyafter"] = 0m;
                        CurrentDetailData["Location"] = "";
                        CurrentDetailData["description"] = "";
                    }
                    else
                    {
                        //bug fix:364: WAREHOUSE_P51_Warehouse Backward Stocktaking
                        if (MyUtility.Check.Empty(CurrentDetailData["stocktype"])) CurrentDetailData["stocktype"] = CurrentMaintain["stocktype"].ToString();

                        if (!MyUtility.Check.Seek(string.Format(@"select a.ukey,a.roll,a.dyelot,a.inqty-a.outqty+a.adjustqty qty
                                 ,dbo.Getlocation(a.ukey) as location 
                                 ,dbo.getmtldesc('{0}','{1}','{2}',2,0) as [description] 
                                        from dbo.ftyinventory a WITH (NOLOCK) where poid='{0}' and seq1='{1}' and seq2='{2}' 
                                        and stocktype='{3}' and roll='{4}' and lock =0", CurrentDetailData["poid"]
                                                                        ,CurrentDetailData["seq1"]
                                                                        ,CurrentDetailData["seq2"]
                                                                        ,CurrentDetailData["stocktype"]
                                                                        , e.FormattedValue.ToString()), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found! or Item is lock!!", "Roll#");
                            return;
                        }
                        else
                        {
                            CurrentDetailData["ftyinventoryukey"] = dr["ukey"];
                            CurrentDetailData["roll"] = e.FormattedValue;
                            CurrentDetailData["dyelot"] = dr["dyelot"];
                            CurrentDetailData["qtybefore"] = dr["qty"];
                            CurrentDetailData["qtyafter"] = 0m;
                            CurrentDetailData["Location"] = dr["Location"];
                            CurrentDetailData["description"] = dr["description"];
                        }
                    }
                }
            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: false, alignment: null, CheckMDivisionID:true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: false, settings:ts)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: false, settings: ts2)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)  //3
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
            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return;
            }

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
            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@Factoryid";
            sp_factory.Value = Sci.Env.User.Factory;
            cmds.Add(sp_factory);
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
          
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id
,a.PoId,a.Seq1,a.Seq2
,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,dbo.Getlocation(fi.ukey) location
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
from dbo.StockTaking_detail as a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void comboStockType_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(comboStockType.SelectedValue) && comboStockType.SelectedValue != comboStockType.OldValue)
            {
                if (detailgridbs.DataSource != null && DetailDatas.Count > 0)
                {
                    foreach (DataRow dr in DetailDatas)
                    {
                        dr.Delete();
                    }                 
                }
            }
        }

        protected override bool ClickPrint()
        {
            P51_Print p = new P51_Print();
            p.CurrentDataRow = this.CurrentDataRow;
            p.ShowDialog();

            return true;
        }
    }
}