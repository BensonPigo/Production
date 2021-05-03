using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using Sci.Win.Tools;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P17 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        /// <inheritdoc/>
        private ReportViewer viewer;

        /// <inheritdoc/>
        private DataTable dtBorrow;

        /// <inheritdoc/>
        public P17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(this.viewer);

            // MDivisionID 是 P17 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
            this.detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    DataRow tmp = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
                    this.OnDetailGridInsert();
                    DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
                    newrow.ItemArray = tmp.ItemArray;
                }
            };
        }

        /// <inheritdoc/>
        public P17(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        // print

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string confirmTime = MyUtility.Convert.GetDate(row["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            string cmdText = @"
select NameEn
from MDivision
where id = @MDivision";
            DualResult result = DBProxy.Current.Select(string.Empty, cmdText, pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dt");
                return false;
            }

            string rptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            report.ReportParameters.Add(new ReportParameter("confirmTime", confirmTime));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;

            string cmd = @"
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
        ,a.Location
        ,[Total]=sum(a.Qty) OVER (PARTITION BY a.POID ,a.Seq1,a.seq2)
from dbo.IssueReturn_Detail a WITH (NOLOCK) 
inner join PO_Supp_Detail b WITH (NOLOCK) on a.poid=b.id and a.Seq1 = b.SEQ1 and a.Seq2 = b.SEQ2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2 and a.Dyelot = fi.Dyelot
    and a.roll = fi.roll and a.stocktype = fi.stocktype
where a.id= @ID";

            result = DBProxy.Current.Select(string.Empty, cmd, pars, out dd);
            if (!result)
            {
                this.ShowErr(result);
            }

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
                    Total = row1["Total"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC

            // DualResult result;
            Type reportResourceNamespace = typeof(P17_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P17_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report)
            {
                MdiParent = this.MdiParent,
            };
            frm.Show();

            return true;

            // return base.ClickPrint();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Return Qty can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["ftyinventoryukey"]))
                {
                    string stocktype = string.Empty;
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

                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} doesn't exist in warehouse {5}",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"], stocktype) + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "RR", "IssueReturn", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            string strSort = ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort;
            ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = string.Empty;
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["Stocktype"] = 'B';
            this.CurrentDetailData["MdivisionID"] = Env.User.Keyword;
            ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = strSort;
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            #region Seq 右鍵開窗
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    IList<DataRow> x;

                    Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(this.CurrentDetailData["poid"].ToString(), this.CurrentDetailData["seq"].ToString(), "f.MDivisionID = '{1}'", false);
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = selepoitem.GetSelecteds();

                    this.CurrentDetailData["seq"] = x[0]["seq"];
                    this.CurrentDetailData["seq1"] = x[0]["seq1"];
                    this.CurrentDetailData["seq2"] = x[0]["seq2"];
                    this.CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    this.CurrentDetailData["Description"] = x[0]["Description"];
                    string tmp = MyUtility.GetValue.Lookup(
                            string.Format(
                                @"
select isnull(ukey,0) ukey 
from dbo.ftyinventory WITH (NOLOCK) 
inner join Orders on ftyinventory.poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.id
where Factory.MDivisionID = '{0}' and ftyinventory.poid='{1}' and ftyinventory.seq1='{2}' and ftyinventory.seq2='{3}' 
    and ftyinventory.stocktype='{4}' and ftyinventory.roll='{5}' ",
                                Env.User.Keyword, this.CurrentDetailData["poid"], x[0]["seq1"], x[0]["seq2"], this.CurrentDetailData["stocktype"], string.Empty));
                    if (!MyUtility.Check.Empty(tmp))
                    {
                        this.CurrentDetailData["ftyinventoryukey"] = tmp;
                    }

                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region -- Seq valied --
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr;
                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["seq"] = string.Empty;
                        this.CurrentDetailData["seq1"] = string.Empty;
                        this.CurrentDetailData["seq2"] = string.Empty;
                        this.CurrentDetailData["Roll"] = string.Empty;
                        this.CurrentDetailData["Dyelot"] = string.Empty;
                        this.CurrentDetailData["stockunit"] = string.Empty;
                        this.CurrentDetailData["Description"] = string.Empty;
                    }
                    else
                    {
                        // check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (seq.Length < 2)
                        {
                             e.Cancel = true;
                             MyUtility.Msg.WarningBox("Data not found!", "Seq");
                             return;
                        }

                        string x = Prgs.SelePoItemSqlCmd(false);
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            Prgs.SelePoItemSqlCmd(false) +
                                    @" and f.MDivisionID = '{1}' and p.seq1 ='{2}' and p.seq2 = '{3}'", this.CurrentDetailData["poid"], Env.User.Keyword, seq[0], seq[1]), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }

                        this.CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                        this.CurrentDetailData["seq1"] = seq[0];
                        this.CurrentDetailData["seq2"] = seq[1];
                        this.CurrentDetailData["Roll"] = string.Empty;
                        this.CurrentDetailData["Dyelot"] = string.Empty;
                        this.CurrentDetailData["stockunit"] = dr["stockunit"];
                        this.CurrentDetailData["Description"] = dr["description"];

                        string tmp = MyUtility.GetValue.Lookup(
                            string.Format(
                                @"
select isnull(ukey,0) ukey 
from dbo.ftyinventory WITH (NOLOCK) 
inner join Orders on ftyinventory.poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.id
where Factory.MDivisionID = '{0}' and ftyinventory.poid='{1}' and ftyinventory.seq1='{2}' and ftyinventory.seq2='{3}' 
    and ftyinventory.stocktype='{4}' and ftyinventory.roll='{5}'
", Env.User.Keyword, this.CurrentDetailData["poid"], seq[0], seq[1], this.CurrentDetailData["stocktype"], string.Empty));
                        if (!MyUtility.Check.Empty(tmp))
                        {
                            this.CurrentDetailData["ftyinventoryukey"] = tmp;
                        }
                    }
                }
            };
            #endregion Seq 右鍵開窗
            DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
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

                    if (e.Button == MouseButtons.Right && e.RowIndex != -1)
                    {
                        string sqlcmd = string.Format(
                            @"SELECT  a.poid
                                                                ,a.seq1
                                                                ,a.seq2
                                                                ,a.roll
                                                                ,a.dyelot
                                                                ,inqty - outqty + adjustqty - ReturnQty balance
                                                                ,a.ukey
                                                        FROM dbo.ftyinventory a WITH (NOLOCK) 
                                                        WHERE   stocktype = 'B'
                                                            AND poID ='{0}'
                                                            AND seq1= '{1}' 
                                                            AND seq2= '{2}' 
                                                        order by poid,seq1,seq2,Roll", dr["poid"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString());
                        Win.Tools.SelectItem item
                            = new Win.Tools.SelectItem(sqlcmd, "13,4,3,10,5,10,0", dr["roll"].ToString(), "SP#,Seq1,Seq2,Roll,Dyelot,Balance,")
                            {
                                Width = 600,
                            };
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }

                        IList<DataRow> selectedData = item.GetSelecteds();
                        if (selectedData.Count > 0)
                        {
                            dr["roll"] = selectedData[0]["roll"].ToString();
                            dr["dyelot"] = selectedData[0]["dyelot"].ToString();
                            dr["ftyinventoryukey"] = selectedData[0]["ukey"].ToString();
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
                    if (oldValue.Equals(newValue))
                    {
                        return;
                    }

                    string sqlcmd = string.Format(
                        @"SELECT  dyelot
                                                            ,dbo.Getlocation(a.ukey) as [location]
                                                            ,ukey
                                                        FROM dbo.ftyinventory a WITH (NOLOCK) 
                                                        WHERE   stocktype = 'B'
                                                            AND poID ='{0}'
                                                            AND seq1= '{1}' 
                                                            AND seq2= '{2}' 
                                                            AND roll= '{3}'
                                                       ", dr["poid"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString(), e.FormattedValue);

                    DualResult result;
                    if (result = DBProxy.Current.Select(null, sqlcmd, out this.dtBorrow))
                    {
                        if (this.dtBorrow.Rows.Count > 0)
                        {
                            // 有此ROLL
                            dr["dyelot"] = this.dtBorrow.Rows[0][0].ToString();
                            dr["Roll"] = e.FormattedValue;
                            dr["Location"] = this.dtBorrow.Rows[0][1].ToString();
                            dr["ftyinventoryukey"] = this.dtBorrow.Rows[0][2].ToString();
                            dr.EndEdit();
                        }
                        else
                        {
                            e.Cancel = true;
                            this.CurrentDetailData["roll"] = string.Empty;
                            MyUtility.Msg.WarningBox("Data not found!", "Roll#");
                            return;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["roll"] = string.Empty;
                        MyUtility.Msg.WarningBox("Data not found!", "Roll#");
                        return;
                    }
                }
            };
            #endregion

            DataGridViewGeneratorTextColumnSettings location_Col = new DataGridViewGeneratorTextColumnSettings();
            #region Location右鍵開窗

            location_Col.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (e.Button == MouseButtons.Right && e.RowIndex != -1)
                    {
                        SelectItem2 selectItem2 = Prgs.SelectLocation("B", MyUtility.Convert.GetString(dr["Location"]));

                        selectItem2.ShowDialog();
                        if (selectItem2.DialogResult == DialogResult.OK)
                        {
                            dr["Location"] = selectItem2.GetSelecteds().Select(o => MyUtility.Convert.GetString(o["ID"])).JoinToString(",");
                        }

                        dr.EndEdit();
                    }
                }
            };
            #endregion

            #region Location驗證
            location_Col.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = dr["Location"].ToString();
                    string newValue = e.FormattedValue.ToString().Split(',').ToList().Where(o => !MyUtility.Check.Empty(o)).Distinct().JoinToString(",");
                    if (oldValue.Equals(newValue))
                    {
                        return;
                    }

                    string notLocationExistsList = newValue.Split(',').ToList().Where(o => !Prgs.CheckLocationExists("B", o)).JoinToString(",");

                    if (!MyUtility.Check.Empty(notLocationExistsList))
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"Location<{notLocationExistsList}> not Found");
                        return;
                    }
                    else
                    {
                        dr["Location"] = newValue;
                        dr.EndEdit();
                    }
                }
            };
            #endregion

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), checkMDivisionID: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts) // 1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6),  settings: ts4) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
            .Numeric("qty", header: "Return Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
            .Text("Location", header: "Bulk Location", settings: location_Col) // 7
            ;
            #endregion 欄位設定
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = string.Empty;
            string sqlupd2_B = string.Empty;

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P17"))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "IssueReturn_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (dt != null)
            {
                if (UtilityAutomation.IsAutomationEnable && !Prgs.ChkWMSCompleteTime(dt, "IssueReturn_Detail"))
                {
                    return;
                }
            }

            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,f.OutQty,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.dyelot = f.dyelot
where isnull(f.OutQty,0) < d.Qty and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} Outqty is {5}, Return Qty can not higher than {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"], tmp["OutQty"]);
                    }

                    MyUtility.Msg.WarningBox(ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update IssueReturn set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B = Prgs.UpdateMPoDetail(4, null, true);

            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             Location = m.Field<string>("location"),
                             qty = -m.Field<decimal>("qty"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);

            #endregion

            #region 更新庫存位置  ftyinventory_detail.MtlLocationID

            // 增加判斷條件WMSLock=0,才能更新MtlLocation
            DataTable locationTable;
            sqlcmd = $@"
Select d.poid ,d.seq1 ,d.seq2 ,d.Roll ,d.Dyelot ,d.StockType ,[ToLocation]=d.Location
from dbo.IssueReturn_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=0 and f.WMSLock=0 AND d.Id = '{this.CurrentMaintain["id"]}'";
            DBProxy.Current.Select(null, sqlcmd, out locationTable);

            var data_Fty_26F = (from b in locationTable.AsEnumerable()
                                select new
                                {
                                    poid = b.Field<string>("poid"),
                                    seq1 = b.Field<string>("seq1"),
                                    seq2 = b.Field<string>("seq2"),
                                    stocktype = b.Field<string>("stocktype"),
                                    toLocation = b.Field<string>("ToLocation"),
                                    roll = b.Field<string>("roll"),
                                    dyelot = b.Field<string>("dyelot"),
                                }).ToList();

            string upd_Fty_26F = Prgs.UpdateFtyInventory_IO(27, null, false);

            #endregion

            #region 更新BarCode  Ftyinventory
            List<string> barcodeList = new List<string>();
            DataTable dtCnt = (DataTable)this.detailgridbs.DataSource;

            // distinct CombineBarcode,並排除CombineBarcode = null
            DataRow[] count2 = dtCnt.Select("FabricType = 'F' and Barcode = ''");
            if (count2.Length > 0)
            {
                barcodeList = Prgs.GetBarcodeNo("FtyInventory_Barcode", "F", count2.Length);
                int cnt = 0;
                ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "Barcode";
                foreach (DataRow drDis in this.DetailDatas)
                {
                    if (string.Compare(drDis["FabricType"].ToString(), "F") == 0 && MyUtility.Check.Empty(drDis["Barcode"]))
                    {
                        drDis["Barcode"] = barcodeList[cnt];
                        cnt++;
                    }
                }
            }

            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;
            var data_Fty_Barcode = (from m in this.DetailDatas.AsEnumerable().Where(s => s["FabricType"].ToString() == "F")
                                    select new
                                    {
                                        TransactionID = m.Field<string>("ID"),
                                        poid = m.Field<string>("poid"),
                                        seq1 = m.Field<string>("seq1"),
                                        seq2 = m.Field<string>("seq2"),
                                        stocktype = m.Field<string>("stocktype"),
                                        roll = m.Field<string>("roll"),
                                        dyelot = m.Field<string>("dyelot"),
                                        Barcode = m.Field<string>("Barcode"),
                                    }).ToList();

            upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, true);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, true);
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb,
                           "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_26F, string.Empty, upd_Fty_26F, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    // 更新FtyInventory Barcode
                    if (data_Fty_Barcode.Count >= 1)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V1, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V2, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    #region -- 更新庫存數量 MDivisionPoDetail --
                    var data_MD_0T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "B")
                                      group b by new
                                      {
                                          poid = b.Field<string>("poid").Trim(),
                                          seq1 = b.Field<string>("seq1").Trim(),
                                          seq2 = b.Field<string>("seq2").Trim(),
                                          stocktype = b.Field<string>("stocktype").Trim(),
                                      }
                                        into m
                                      select new Prgs_POSuppDetailData
                                      {
                                          Poid = m.First().Field<string>("poid"),
                                          Seq1 = m.First().Field<string>("seq1"),
                                          Seq2 = m.First().Field<string>("seq2"),
                                          Stocktype = m.First().Field<string>("stocktype"),
                                          Qty = m.Sum(w => w.Field<decimal>("qty")),
                                          Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                                      }).ToList();

                    string upd_MD_0T = Prgs.UpdateMPoDetail(0, data_MD_0T, true);

                    #endregion

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_0T, string.Empty, upd_MD_0T, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

            // AutoWHACC WebAPI for Vstrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                Task.Run(() => new Vstrong_AutoWHAccessory().SentIssueReturn_Detail_New(dtMain, "New"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }

            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                Task.Run(() => new Gensong_AutoWHFabric().SentIssueReturn_Detail_New(dtMain, "New"))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            string sqlupd2_FIO = string.Empty;
            string sqlupd2_B = string.Empty;

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion
            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "IssueReturn_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) 
and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than return qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region UnConfirmed 先檢查WMS是否傳送成功
            DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                if (!Vstrong_AutoWHAccessory.SentIssueReturn_Detail_delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }

            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                if (!Gensong_AutoWHFabric.SentIssueReturn_Detail_delete(dtDetail, "UnConfirmed"))
                {
                    return;
                }
            }
            #endregion

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update IssueReturn set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new
                       {
                           poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);

            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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

            #region 刪除Barcode
            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;
            var data_Fty_Barcode = (from m in this.DetailDatas.AsEnumerable().Where(s => s["FabricType"].ToString() == "F")
                                    select new
                                    {
                                        TransactionID = m.Field<string>("ID"),
                                        poid = m.Field<string>("poid"),
                                        seq1 = m.Field<string>("seq1"),
                                        seq2 = m.Field<string>("seq2"),
                                        stocktype = m.Field<string>("stocktype"),
                                        roll = m.Field<string>("roll"),
                                        dyelot = m.Field<string>("dyelot"),
                                        Barcode = m.Field<string>("Barcode"),
                                    }).ToList();

            upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, false);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, false);

            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B, out resulttb,
                           "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    // 更新FtyInventory Barcode
                    if (data_Fty_Barcode.Count >= 1)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V1, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V2, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            // AutoWHFabric WebAPI for Gensong
            // this.SentToGensong_AutoWHFabric();
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,p1.StockUnit
,a.Qty
,a.StockType
,a.ftyinventoryukey
,a.Location
,a.ukey
,Barcode = isnull(FI.barcode,'')
,fabrictype = isnull(p1.fabrictype,'')
from dbo.IssueReturn_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.Poid = FI.Poid and a.Seq1 = FI.Seq1 and a.Seq2 = FI.Seq2 and a.Dyelot = FI.Dyelot
    and a.Roll = FI.Roll and a.StockType = FI.StockType
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P17_AccumulatedQty(this.CurrentMaintain)
            {
                P17 = this,
            };
            frm.ShowDialog(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void TxtTransfer_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.txtTransfer.Text) && this.txtTransfer.Text != this.txtTransfer.OldValue)
            {
                ((DataTable)this.detailgridbs.DataSource).Rows.Clear();  // 清空表身資料

                DataRow dr;
                DataTable dt;
                if (!MyUtility.Check.Seek(
                    string.Format(
                    "select 1 where exists(select * from dbo.issue WITH (NOLOCK) where id='{0}' and status !='New')",
                    this.txtTransfer.Text), out dr, null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Transfer# is not found!!");
                    this.txtTransfer.Text = string.Empty;
                    return;
                }
                else
                {
                    DBProxy.Current.Select(null, string.Format(
                        @"select a.poid,a.seq1,a.seq2,a.Qty,a.StockType
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
", this.txtTransfer.Text), out dt);
                    foreach (var item in dt.ToList())
                    {
                        // DetailDatas.(item);
                        ((DataTable)this.detailgridbs.DataSource).ImportRow(item);
                    }
                }
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("poid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P17_ExcelImport(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }
    }
}