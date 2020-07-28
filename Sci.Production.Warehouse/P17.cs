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
using Sci.Production.Automation;
using System.Threading.Tasks;

namespace Sci.Production.Warehouse
{
    public partial class P17 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        protected DataTable dtBorrow;
        public P17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);
            //MDivisionID 是 P17 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            //
            detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    DataRow tmp = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    this.OnDetailGridInsert();
                    DataRow newrow = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    newrow.ItemArray = tmp.ItemArray;
                }
            };
        }

        public P17(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["IssueDate"] = DateTime.Now;
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

        //print
        protected override bool ClickPrint()
        {
            //DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("", @"
select NameEn
from MDivision
where id = @MDivision", pars, out dt);
            if (!result) { this.ShowErr(result); }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dt");
                return false;
            }

            string RptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result = DBProxy.Current.Select("", @"
select a.poid [SP]
        ,a.Seq1+'-'+a.Seq2 [SEQ]
        ,a.Roll [ROLL]
        ,a.Dyelot [DYELOT]
        ,IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
			AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
			AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
			,'',dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))[DESCRIPTION]
        ,b.StockUnit [UNIT]
        ,a.Qty [RETURN_QTY]
        ,dbo.Getlocation(fi.ukey) [LOCATION]
        ,[Total]=sum(a.Qty) OVER (PARTITION BY a.POID ,a.Seq1,a.seq2)
from dbo.IssueReturn_Detail a WITH (NOLOCK) 
inner join PO_Supp_Detail b WITH (NOLOCK) on a.poid=b.id and a.Seq1 = b.SEQ1 and a.Seq2 = b.SEQ2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2 and a.Dyelot = fi.Dyelot
    and a.roll = fi.roll and a.stocktype = fi.stocktype
where a.id= @ID", pars, out dd);
            if (!result) { this.ShowErr(result); }

            if (dd == null || dd.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dd");
                return false;
            }

            // 傳 list 資料            
            List<P17_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P17_PrintData()
                {
                    SP = row1["SP"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    ROLL = row1["ROLL"].ToString().Trim(),
                    DYELOT = row1["DYELOT"].ToString().Trim(),
                    DESCRIPTION = row1["DESCRIPTION"].ToString().Trim(),
                    UNIT = row1["UNIT"].ToString().Trim(),
                    RETURN_QTY = row1["RETURN_QTY"].ToString().Trim(),
                    LOCATION = row1["LOCATION"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P17_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P17_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;


            //return base.ClickPrint();
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
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty"
                        , row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }
        

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Return Qty can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["ftyinventoryukey"]))
                {
                    string stocktype="";
                    switch (row["stocktype"].ToString())
                    {
                        case "I":
                            stocktype = "Inventory";
                            break;
                        case "B":
                            stocktype = "Bulk";
                            break;
                        default:
                            
                            break;
                    }

                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} doesn't exist in warehouse {5}"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"], stocktype) + Environment.NewLine);
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "RR", "IssueReturn", (DateTime)CurrentMaintain["Issuedate"]);
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
            CurrentDetailData["Stocktype"] = 'B';
            CurrentDetailData["MdivisionID"] = Sci.Env.User.Keyword;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            #region Seq 右鍵開窗
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    IList<DataRow> x;

                    Sci.Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(CurrentDetailData["poid"].ToString(), CurrentDetailData["seq"].ToString(), "f.MDivisionID = '{1}'", false);
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = selepoitem.GetSelecteds();

                    CurrentDetailData["seq"] = x[0]["seq"];
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    CurrentDetailData["Description"] = x[0]["Description"];
                    string tmp = MyUtility.GetValue.Lookup(
                            string.Format(@"
select isnull(ukey,0) ukey 
from dbo.ftyinventory WITH (NOLOCK) 
inner join Orders on ftyinventory.poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.id
where Factory.MDivisionID = '{0}' and ftyinventory.poid='{1}' and ftyinventory.seq1='{2}' and ftyinventory.seq2='{3}' 
    and ftyinventory.stocktype='{4}' and ftyinventory.roll='{5}' "
                            , Sci.Env.User.Keyword, CurrentDetailData["poid"], x[0]["seq1"], x[0]["seq2"], CurrentDetailData["stocktype"], ""));
                    if (!MyUtility.Check.Empty(tmp)) CurrentDetailData["ftyinventoryukey"] = tmp;
                    CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region -- Seq valied --
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                DataRow dr;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["seq"] = "";
                        CurrentDetailData["seq1"] = "";
                        CurrentDetailData["seq2"] = "";
                        CurrentDetailData["Roll"] = "";
                        CurrentDetailData["Dyelot"] = "";
                        CurrentDetailData["stockunit"] = "";
                        CurrentDetailData["Description"] = "";
                    }
                    else
                    {
                        //check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (seq.Length < 2)
                        {
                             e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");    
                            return;
                        }
                        string x = Prgs.selePoItemSqlCmd(false);
                        if (!MyUtility.Check.Seek(string.Format(Prgs.selePoItemSqlCmd(false) +
                                    @" and f.MDivisionID = '{1}' and p.seq1 ='{2}' and p.seq2 = '{3}'", CurrentDetailData["poid"], Sci.Env.User.Keyword, seq[0], seq[1]), out dr, null))
                        {
                            e.Cancel = true; 
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");  
                            return;
                        }

                        CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                        CurrentDetailData["seq1"] = seq[0];
                        CurrentDetailData["seq2"] = seq[1];
                        CurrentDetailData["Roll"] = "";
                        CurrentDetailData["Dyelot"] = "";
                        CurrentDetailData["stockunit"] = dr["stockunit"];
                        CurrentDetailData["Description"] = dr["description"];
                        
                        string tmp = MyUtility.GetValue.Lookup(
                            string.Format(@"
select isnull(ukey,0) ukey 
from dbo.ftyinventory WITH (NOLOCK) 
inner join Orders on ftyinventory.poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.id
where Factory.MDivisionID = '{0}' and ftyinventory.poid='{1}' and ftyinventory.seq1='{2}' and ftyinventory.seq2='{3}' 
    and ftyinventory.stocktype='{4}' and ftyinventory.roll='{5}'
", Sci.Env.User.Keyword, CurrentDetailData["poid"], seq[0], seq[1], CurrentDetailData["stocktype"], ""));
                        if (!MyUtility.Check.Empty(tmp)) CurrentDetailData["ftyinventoryukey"] = tmp;
                    }
                }
            };
            #endregion Seq 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            #region Roll#右鍵開窗
            
            ts4.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["POID"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill SP# first");
                        return;
                    }
                    if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
                    {

                        string sqlcmd = string.Format(@"SELECT  a.poid
                                                                ,a.seq1
                                                                ,a.seq2
                                                                ,a.roll
                                                                ,a.dyelot
                                                                ,inqty - outqty + adjustqty balance
                                                                ,a.ukey
                                                        FROM dbo.ftyinventory a WITH (NOLOCK) 
                                                        WHERE   stocktype = 'B'
                                                            AND poID ='{0}'
                                                            AND seq1= '{1}' 
                                                            AND seq2= '{2}' 
                                                        order by poid,seq1,seq2,Roll", dr["poid"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString());
                        Sci.Win.Tools.SelectItem item
                            = new Sci.Win.Tools.SelectItem(sqlcmd, "13,4,3,10,5,10,0", dr["roll"].ToString(), "SP#,Seq1,Seq2,Roll,Dyelot,Balance,");
                        item.Width = 600;
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        IList<DataRow> selectedData = item.GetSelecteds();
                        if (selectedData.Count > 0)
                        {
                            dr["roll"] = (selectedData[0])["roll"].ToString();
                            dr["dyelot"] = (selectedData[0])["dyelot"].ToString();
                            dr["ftyinventoryukey"] = (selectedData[0])["ukey"].ToString();
                        }
                    }
                }
            };
            #endregion
            #region -- Roll Vaild --
            ts4.CellValidating += (s, e) =>
            {
                
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = dr["Roll"].ToString();
                    string newValue = e.FormattedValue.ToString();
                    if (oldValue.Equals(newValue)) return;
                    string sqlcmd = string.Format(@"SELECT  dyelot
                                                            ,dbo.Getlocation(a.ukey) as [location]
                                                            ,ukey
                                                        FROM dbo.ftyinventory a WITH (NOLOCK) 
                                                        WHERE   stocktype = 'B'
                                                            AND poID ='{0}'
                                                            AND seq1= '{1}' 
                                                            AND seq2= '{2}' 
                                                            AND roll= '{3}'
                                                       ", dr["poid"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString(), e.FormattedValue);
                   
                    Ict.DualResult result;
                    if (result = DBProxy.Current.Select(null, sqlcmd, out dtBorrow))
                    {
                        if (dtBorrow.Rows.Count > 0)
                        {
                            //有此ROLL
                            dr["dyelot"] = dtBorrow.Rows[0][0].ToString();
                            dr["Roll"] = e.FormattedValue;
                            dr["Location"] = dtBorrow.Rows[0][1].ToString();
                            dr["ftyinventoryukey"] = dtBorrow.Rows[0][2].ToString();
                            dr.EndEdit();
                        }
                        else {
                            e.Cancel = true;
                            CurrentDetailData["roll"] = "";
                            MyUtility.Msg.WarningBox("Data not found!", "Roll#");
                            return;
                        }
                    }
                    else {
                        
                        e.Cancel = true;
                        CurrentDetailData["roll"] = "";
                        MyUtility.Msg.WarningBox("Data not found!", "Roll#");
                        return;
                    }
                                       
                }
            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
                .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), CheckMDivisionID: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6),settings:ts)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6),  settings: ts4)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //5
            .Numeric("qty", header: "Return Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
            .Text("Location", header: "Bulk Location", iseditingreadonly: true)    //7
            ;     //
            #endregion 欄位設定
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = "";
            string sqlupd2_B = "";

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,f.OutQty,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.dyelot = f.dyelot
where isnull(f.OutQty,0) < d.Qty and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} Outqty is {5}, Return Qty can not higher than {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"], tmp["OutQty"]);
                    }
                    MyUtility.Msg.WarningBox(ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update IssueReturn set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();
            sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);

            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             Location = m.Field<string>("location"),
                             qty = - (m.Field<decimal>("qty")),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb
                           , "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            // AutoWHFabric WebAPI for Gensong
            SentToGensong_AutoWHFabric();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            string sqlupd2_FIO = "";
            string sqlupd2_B = "";

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than return qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update IssueReturn set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);

            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = m.Field<decimal>("qty"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb
                           , "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            // AutoWHFabric WebAPI for Gensong
            SentToGensong_AutoWHFabric();
        }

        /// <summary>
        ///  AutoWHFabric WebAPI for Gensong
        /// </summary>
        private void SentToGensong_AutoWHFabric()
        {   
            if (true) return;// 暫未開放

            DataTable dtDetail = new DataTable();
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                string sqlGetData = string.Empty;
                sqlGetData = $@"
SELECT [ID] = ird.id
,[InvNo] = ''
,[PoId] = ird.Poid
,[Seq1] = ird.Seq1
,[Seq2] = ird.Seq2
,[Refno] = po3.Refno
,[ColorID] = po3.ColorID
,[Roll] = ird.Roll
,[Dyelot] = ird.Dyelot
,[StockUnit] = dbo.GetStockUnitBySPSeq(ird.POID,ird.Seq1,ird.Seq2)
,[StockQty] = ird.Qty
,[PoUnit] = po3.PoUnit
,[ShipQty] = convert(numeric(11,2), 0)
,[Weight] = convert(numeric(7,2), 0)
,[StockType] = ird.StockType
,[Ukey] = ird.Ukey
,[IsInspection] = convert(bit, 0)
,Junk = case when ir.Status = 'Confirmed' then convert(bit, 0) else convert(bit, 1) end
FROM Production.dbo.IssueReturn_Detail ird
inner join Production.dbo.IssueReturn ir on ird.id = ir.id
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= ird.PoId 
	and po3.SEQ1 = ird.Seq1 and po3.SEQ2 = ird.Seq2
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = ird.Poid and seq1 = ird.seq1 and seq2 = ird.seq2 
	and FabricType='F'
)
and ir.id = '{CurrentMaintain["id"]}'
";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    ShowErr(drResult);
                }

                Task.Run(() => new Gensong_AutoWHFabric().SentReceive_DetailToGensongAutoWHFabric(dtDetail))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,p1.StockUnit
,a.Qty
,a.StockType
,a.ftyinventoryukey
,dbo.Getlocation(fi.ukey) location
,a.ukey
from dbo.IssueReturn_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.Poid = FI.Poid and a.Seq1 = FI.Seq1 and a.Seq2 = FI.Seq2 and a.Dyelot = FI.Dyelot
    and a.Roll = FI.Roll and a.StockType = FI.StockType
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void btnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());

        }

        private void btnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P17_AccumulatedQty(CurrentMaintain);
            frm.P17 = this;
            frm.ShowDialog(this);
        }

        private void txtTransfer_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;

            if (!MyUtility.Check.Empty(txtTransfer.Text) && txtTransfer.Text != txtTransfer.OldValue)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料

                DataRow dr;
                DataTable dt;
                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from dbo.issue WITH (NOLOCK) where id='{0}' and status !='New')"
                    , txtTransfer.Text), out dr, null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Transfer# is not found!!");
                    this.txtTransfer.Text = string.Empty;
                    return;
                }
                else
                {
                    DBProxy.Current.Select(null, string.Format(@"select a.poid,a.seq1,a.seq2,a.Qty,a.StockType
,b.StockUnit
, concat(Ltrim(Rtrim(a.seq1)), ' ', a.seq2) as seq
, a.Roll as roll
, a.Dyelot as dyelot
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.ftyinventoryukey
from dbo.Issue_Detail a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on a.PoID= b.id and a.Seq1 = b.SEQ1 and a.Seq2 = b.SEQ2
LEFT JOIN Orders o ON o.ID = a.POID
where a.id='{0}'
AND o.Category <> 'A'
", txtTransfer.Text), out dt);
                    foreach (var item in dt.ToList())
                    {
                        //DetailDatas.(item);
                        ((DataTable)detailgridbs.DataSource).ImportRow(item);
                    }
                }

            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P17_ExcelImport(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }
    }
}