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
    public partial class P18 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;

        public P18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");

            Controls.Add(viewer);

            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);

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

        public P18(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["IssueDate"] = DateTime.Now;
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

        //print
        protected override bool ClickPrint()
        {
            //329: WAREHOUSE_P18 Print，資料如果未confirm不能列印。
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print !!", "Warning");
                return false;
            }

            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string fromFactory = row["FromFtyID"].ToString();
            string Remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select    
            b.name 
            from dbo.Transferin  a 
            inner join dbo.mdivision  b 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string RptTitle = dt.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FromFtyID", fromFactory));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("IssueDate", issuedate));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion
            #region -- 撈表身資料 --
            DataTable dtDetail;
            result = DBProxy.Current.Select("",
            @"select  a.POID,a.Seq1+'-'+a.seq2 as SEQ,a.Roll,a.Dyelot 
	        ,dbo.Getmtldesc(a.poid, a.seq1, a.seq2,2,0) [Description]
            ,b.StockUnit
	        ,a.Qty
            ,dbo.Getlocation(f.ukey)[Location] 
            from dbo.TransferIn_detail a
            left join dbo.PO_Supp_Detail b
            on 
            b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
			inner join FtyInventory f
			on f.MDivisionID= a.MDivisionID and f.POID = a.poid
			And f.Seq1 = a.seq1
			And f.Seq2 = a.seq2
			And f.Roll =  a.roll
			And f.Dyelot = a.dyelot
			And f.StockType = a.stocktype
             where a.id= @ID", pars, out dtDetail);
            if (!result) { this.ShowErr(result); }
       
            // 傳 list 資料            
            List<P18_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P18_PrintData()
                {
                    POID = row1["POID"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    Roll = row1["Roll"].ToString(),
                    DYELOT = row1["DYELOT"].ToString(),
                    DESC = row1["Description"].ToString(),
                    Unit = row1["StockUnit"].ToString(),
                    QTY = row1["QTY"].ToString(),
                    Location = row1["Location"].ToString()
                }).ToList();

            report.ReportDataSource = data;
             #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P18_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P18_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;
        }

        // print for SubReport
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
            if (MyUtility.Check.Empty(this.textBox2.Text))
            {
                MyUtility.Msg.WarningBox("From Factory cannot be null! ");
                this.textBox2.Focus();
                return false;
            }

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
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Transfer In Qty can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
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
            #endregion 必輸檢查
            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "TI", "TransferIn", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }
            #endregion

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
            CurrentDetailData["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentDetailData["Stocktype"] = 'B';
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region -- Seq 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    IList<DataRow> x;

                    DataTable dt;
                    string sqlcmd = string.Format(@"select p.POID poid
,concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2) as seq
,p.seq1
,p.seq2
, p.Refno
, (select f.DescDetail from fabric f where f.SCIRefno = p.scirefno) as Description 
,p.scirefno
from dbo.Inventory p
where POID ='{0}'", CurrentDetailData["poid"].ToString());
                    DBProxy.Current.Select(null, sqlcmd, out dt);

                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt
                                    , "Seq,refno,description"
                                    , "6,8,20", CurrentDetailData["seq"].ToString(), "Seq,Ref#,Description");
                    selepoitem.Width = 480;

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = selepoitem.GetSelecteds();

                    CurrentDetailData["seq"] = x[0]["seq"];
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    //CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    CurrentDetailData["Description"] = x[0]["Description"];
                    //CurrentDetailData["fabrictype"] = x[0]["fabrictype"];

                }
            };

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
                        //CurrentDetailData["fabrictype"] = "";
                    }
                    else
                    {
                        //check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (seq.Length < 2)
                        {
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            e.Cancel = true;
                            return;
                        }

                        if (!MyUtility.Check.Seek(string.Format(@"select pounit, stockunit,fabrictype,qty,scirefno, dbo.getmtldesc(id,seq1,seq2,2,0) as [description] from po_supp_detail
where id = '{0}' and seq1 ='{1}'and seq2 = '{2}'", CurrentDetailData["poid"], seq[0], seq[1]), out dr, null))
                        {
                            if (!MyUtility.Check.Seek(string.Format(@"select p.POID poid
,left(p.seq1+' ',3)+p.seq2 as seq
,p.seq1
,p.seq2
, p.Refno
, (select f.DescDetail from fabric f where f.SCIRefno = p.scirefno) as Description 
,p.scirefno
from dbo.Inventory p
where poid = '{0}' and seq1 ='{1}'and seq2 = '{2}' and factoryid='{3}'", CurrentDetailData["poid"]
                                                                       , e.FormattedValue.ToString().PadRight(5).Substring(0, 3)
                                             , e.FormattedValue.ToString().PadRight(5).Substring(3, 2)
                                             , CurrentMaintain["fromftyid"]), out dr, null))
                            {
                                MyUtility.Msg.WarningBox("Data not found!", "Seq");
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                                CurrentDetailData["seq1"] = seq[0];
                                CurrentDetailData["seq2"] = seq[1];
                                CurrentDetailData["Roll"] = "";
                                CurrentDetailData["Dyelot"] = "";
                                //CurrentDetailData["stockunit"] = dr["stockunit"];
                                CurrentDetailData["Description"] = dr["description"];
                            }
                        }
                        else
                        {
                            CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                            CurrentDetailData["seq1"] = seq[0];
                            CurrentDetailData["seq2"] = seq[1];
                            CurrentDetailData["Roll"] = "";
                            CurrentDetailData["Dyelot"] = "";
                            CurrentDetailData["stockunit"] = dr["stockunit"];
                            CurrentDetailData["Description"] = dr["description"];
                        }
                    }
                }
            };
            #endregion Seq 右鍵開窗
            #region StockType
            Ict.Win.DataGridViewGeneratorComboBoxColumnSettings sk = new DataGridViewGeneratorComboBoxColumnSettings();
            sk.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["stocktype"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", CurrentDetailData["stocktype"].ToString(), Sci.Env.User.Keyword);
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    CurrentDetailData["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            #region -- Location 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(CurrentDetailData["stocktype"].ToString(), CurrentDetailData["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    //CurrentDetailData["location"] = item.GetSelectedString();
                    detailgrid.GetDataRow(e.RowIndex)["location"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", CurrentDetailData["stocktype"].ToString(), Sci.Env.User.Keyword);
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    CurrentDetailData["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts3 = new DataGridViewGeneratorTextColumnSettings();
            ts3.CellValidating += (s, e) =>
            {
                if (this.EditMode == true && String.Compare(e.FormattedValue.ToString(), CurrentDetailData["poid"].ToString()) != 0)
                {
                    if (!MyUtility.Check.Seek(string.Format(@"select POID from Inventory where POID = '{0}'", e.FormattedValue)))
                    {
                        this.CurrentDetailData["poid"] = CurrentDetailData["poid"];
                        MyUtility.Msg.WarningBox("Data not found!", e.FormattedValue.ToString());
                        return;
                    }
                    this.CurrentDetailData["seq"] = "";
                    this.CurrentDetailData["seq1"] = "";
                    this.CurrentDetailData["seq2"] = "";
                    this.CurrentDetailData["roll"] = "";
                    this.CurrentDetailData["dyelot"] = "";
                    this.CurrentDetailData["poid"] = e.FormattedValue;
                }
            };


            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), settings: ts3)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6))  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6))  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //5
            .Numeric("qty", header: "In Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)    //6
            .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), settings:sk).Get(out cbb_stocktype)    //7
            .Text("Location", header: "Location", iseditingreadonly: false, settings: ts2)    //8
            ;     //
            #endregion 欄位設定
            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            detailgrid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            string sqlupd2_B = "";
            string sqlupd2_BI = "";
            String sqlupd2_FIO = "";

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.TransferIn_Detail d inner join FtyInventory f
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.TransferIn_Detail d left join FtyInventory f
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than In qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查不存在的po_supp_detail資料，並新增
            sqlcmd = string.Format(@"Select distinct d.poid,d.seq1,d.seq2
from dbo.TransferIn_Detail d left join dbo.PO_Supp_Detail f
on d.PoId = f.Id
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
where d.Id = '{0}' and f.id is null", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }

        //這段條件 已經改了 不是舊的條件
            //else
            //{
            //    if (datacheck.Rows.Count > 0)
            //    {
//                    sqlupd2.Append(string.Format(@"insert into po_supp_detail 
//(
//	id,seq1,seq2
//	,scirefno
//	,StockUnit
//	,POUnit
//	,FinalETA
//	,ColorID
//    ,brandid
//	,sizespec
//    ,sizeunit
//	,BomArticle
//	,BomBuymonth
//	,BomCountry
//	,BomCustCD
//	,BomCustPONo
//	,BomFactory
//	,BomStyle
//	,BomZipperInsert
//	,width
//    ,stockpoid
//    ,stockseq1
//    ,stockseq2
//	,fabrictype
//	,addname
//    ,adddate
//) 
//Select distinct t.*
//,ISNULL((select Fabric.Type from dbo.Fabric where fabric.SCIRefno = t.SCIRefno),'') as fabrictype
//,'{1}'
//,GETDATE()
//from dbo.TransferIn_Detail d 
//left join (select i.POID,i.Seq1,i.Seq2,i.scirefno,dbo.getstockunit(b.SCIRefno,s.suppid) as stockunit
//					,i.UnitID pounit,i.ETA,ir.ColorID,i.brandid,ir.SizeSpec,ir.SizeUnit,ir.BomArticle,ir.BomBuymonth
//					,ir.BomCountry,ir.BomCustCD,ir.BomCustPONo,ir.BomFactory,ir.BomStyle,ir.BomZipperInsert
//					,ir.Width,poid stockpoid,seq1 stockseq1,seq2 stockseq2
//			from dbo.Inventory i inner join dbo.Inventoryrefno ir on i.InventoryRefnoID = ir.id
//            left join dbo.PO_Supp_Detail b on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
//            left join dbo.PO_Supp as s on s.ID = b.ID and s.Seq1 = b.SEQ1
//) t 
//	on t.POID = d.poid and t.Seq1 = d.seq1 and t.seq2 = d.seq2
//where d.Id = '{0}' and t.POID is not null;" + Environment.NewLine,CurrentMaintain["id"],Env.User.UserID));
            //    }
            //}

            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update TransferIn set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新庫存數量 MDivisionPoDetail -- 
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid").Trim(),
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype").Trim()
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                       }).ToList();
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid").Trim(),
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                        select new Prgs_POSuppDetailData
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                       }).ToList();

            if (bs1.Count > 0)
                sqlupd2_B = Prgs.UpdateMPoDetail(2, bs1, true);
            if (bs1I.Count > 0)
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, true);
            #endregion
            #region -- 更新庫存數量  ftyinventory --
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量  ftyinventory

            #region -- Transaction --
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, "", sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (!(result = MyUtility.Tool.ProcessWithDatatable
                        ((DataTable)detailgridbs.DataSource, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
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
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            string sqlupd2_B = "";
            string sqlupd2_BI = "";
            String sqlupd2_FIO = "";

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.TransferIn_Detail d inner join FtyInventory f
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.TransferIn_Detail d left join FtyInventory f
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than In qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update TransferIn set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新庫存數量 MDivisionPoDetail --
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid").Trim(),
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype").Trim()
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty"))),
                           location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                       }).ToList();
            var bs1I = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                        group b by new
                        {
                            mdivisionid = b.Field<string>("mdivisionid").Trim(),
                            poid = b.Field<string>("poid").Trim(),
                            seq1 = b.Field<string>("seq1").Trim(),
                            seq2 = b.Field<string>("seq2").Trim(),
                            stocktype = b.Field<string>("stocktype")
                        } into m
                        select new Prgs_POSuppDetailData
                        {
                            mdivisionid = m.First().Field<string>("mdivisionid"),
                            poid = m.First().Field<string>("poid"),
                            seq1 = m.First().Field<string>("seq1"),
                            seq2 = m.First().Field<string>("seq2"),
                            stocktype = m.First().Field<string>("stocktype"),
                            qty = - (m.Sum(w => w.Field<decimal>("qty"))),
                            location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                        }).ToList();

            if (bs1.Count > 0)
                sqlupd2_B = Prgs.UpdateMPoDetail(2, bs1, false);
            if (bs1I.Count > 0)
                sqlupd2_BI = Prgs.UpdateMPoDetail(8, bs1I, false);

            #endregion
            #region -- 更新庫存數量  ftyinventory --

            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             mdivisionid = m.Field<string>("mdivisionid"),
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -(m.Field<decimal>("qty")),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region -- Transaction --
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (bs1.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (bs1I.Count > 0)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1I, "", sqlupd2_BI, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
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
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.mdivisionid,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,p1.StockUnit
,a.Qty
,a.StockType
,a.location
,a.ukey
from dbo.TransferIn_Detail a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
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
            var frm = new Sci.Production.Warehouse.P13_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Form
        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P18_AccumulatedQty(CurrentMaintain);
            frm.P18 = this;
            frm.ShowDialog(this);
        }

        // Find
        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P18_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Seek(string.Format(@"select * from scifty where id='{0}'", this.textBox2.Text)))
            {
                MyUtility.Msg.WarningBox("From Factory : " + textBox2.Text + " not found!");
                this.textBox2.Text = "";
                this.textBox2.Focus();
                this.textBox2.Select();
            }
        }
   

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
         if (e.Button==System.Windows.Forms.MouseButtons.Right)
            {
                string cmd = "select ID from scifty where mdivisionid<>'' and Junk<>1 order by MDivisionID,ID ";
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(cmd, "6", this.textBox2.ToString());
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.textBox2.Text = item.GetSelectedString();
            }
        }

    }
}