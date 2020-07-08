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
    public partial class P18 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;

        public P18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            this.viewer = new ReportViewer();
            this.viewer.Dock = DockStyle.Fill;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");

            this.Controls.Add(this.viewer);

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

        public P18(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
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
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            if (this.CurrentMaintain["Status"].EqualString("Confirmed"))
            {
                this.dateIssueDate.ReadOnly = true;
                this.txtFromFactory.ReadOnly = true;
                this.editRemark.ReadOnly = true;
                this.btnClearQtyIsEmpty.Enabled = false;
                this.gridicon.Enabled = false;
            }
        }

        // print
        protected override bool ClickPrint()
        {
            // 329: WAREHOUSE_P18 Print，資料如果未confirm不能列印。
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print !!", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string fromFactory = row["FromFtyID"].ToString();
            string Remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(string.Empty, @"
select  b.name 
from dbo.Transferin  a WITH (NOLOCK) 
inner join dbo.mdivision  b WITH (NOLOCK) on b.id = a.mdivisionid
where   b.id = a.mdivisionid
        and a.id = @ID", pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dt");
                return false;
            }

            // 抓M的EN NAME
            DataTable dtNAME;
            DBProxy.Current.Select(
                string.Empty,
                string.Format(@"select NameEN from MDivision where ID='{0}'", Sci.Env.User.Keyword), out dtNAME);
            string RptTitle = dtNAME.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("FromFtyID", fromFactory));
            report.ReportParameters.Add(new ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new ReportParameter("IssueDate", issuedate));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion
            #region -- 撈表身資料 --
            DataTable dtDetail;
            result = DBProxy.Current.Select(string.Empty, @"
select  a.POID
        , a.Seq1 + '-' + a.seq2 as SEQ
        , a.Roll
        , a.Dyelot 
	    , [Description] = IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
			                   AND (b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
			                   AND (b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
			                  , ''
                              , dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))
        , StockUnit = dbo.GetStockUnitBySpSeq (a.poid, a.seq1, a.seq2)
	    , a.Qty
        , a.Weight
        , dbo.Getlocation(f.ukey)[Location] 
from dbo.TransferIn_detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.POID 
                                                and b.SEQ1 = a.Seq1 
                                                and b.SEQ2=a.seq2
inner join FtyInventory f WITH (NOLOCK) on  f.POID = a.poid
		                                    And f.Seq1 = a.seq1
		                                    And f.Seq2 = a.seq2
		                                    And f.Roll =  a.roll
		                                    And f.Dyelot = a.dyelot
		                                    And f.StockType = a.stocktype
where a.id = @ID", pars, out dtDetail);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dtDetail");
                return false;
            }

            // 傳 list 資料
            List<P18_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P18_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    DYELOT = row1["DYELOT"].ToString().Trim(),
                    DESC = row1["Description"].ToString().Trim(),
                    Unit = row1["StockUnit"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    GW = row1["Weight"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type ReportResourceNamespace = typeof(P18_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P18_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.txtFromFactory.Text))
            {
                MyUtility.Msg.WarningBox("From Factory cannot be null! ");
                this.txtFromFactory.Focus();
                return false;
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }

                // 檢查Stock Type是否為空、且資料是否正確(B、I)
                if (row["Stocktype"].ToString() != "B" && row["Stocktype"].ToString() != "I")
                {
                    MyUtility.Msg.WarningBox("Detail <Stock Type> can only be [Bulk] or [Inventory]");
                    return false;
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Transfer In Qty can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
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
            #endregion 必輸檢查

            // 非 Fabric 的物料，移除 Roll & Dyelot
            foreach (DataRow row in this.DetailDatas)
            {
                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["Roll"] = string.Empty;
                    row["Dyelot"] = string.Empty;
                }
            }

            // 收物料時, 要判斷除了自己之外, 是否已存在同SP+Seq+ROLL+Dyelot(Fabric=F, StockType相同),P18 [TransferIn_Detail]
            warningmsg.Clear();
            foreach (DataRow row in this.DetailDatas)
            {
                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    continue;
                }

                if (row.RowState == DataRowState.Added)
                {
                    if (MyUtility.Check.Seek(string.Format(
                        @"select * from TransferIn_Detail where poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and Roll = '{3}' and Dyelot = '{4}' and stocktype = '{5}'",
                        row["poid"], row["seq1"], row["seq2"], row["Roll"], row["Dyelot"], row["stocktype"])))
                    {
                        warningmsg.Append(string.Format(@"<SP>: {0} <Seq>: {1}-{2}  <ROLL> {3}<DYELOT>{4} exists, cannot be saved!", row["poid"], row["seq1"], row["seq2"], row["Roll"], row["Dyelot"]));
                        warningmsg.Append(Environment.NewLine);
                    }

                    continue;
                }

                bool isTransferIn_DetailKewordChanged = MyUtility.Convert.GetString(row["poid"]) != MyUtility.Convert.GetString(row["poid", DataRowVersion.Original]) ||
                        MyUtility.Convert.GetString(row["seq1"]) != MyUtility.Convert.GetString(row["seq1", DataRowVersion.Original]) ||
                        MyUtility.Convert.GetString(row["seq2"]) != MyUtility.Convert.GetString(row["seq2", DataRowVersion.Original]) ||
                        MyUtility.Convert.GetString(row["Roll"]) != MyUtility.Convert.GetString(row["Roll", DataRowVersion.Original]) ||
                        MyUtility.Convert.GetString(row["Dyelot"]) != MyUtility.Convert.GetString(row["Dyelot", DataRowVersion.Original]) ||
                        MyUtility.Convert.GetString(row["stocktype"]) != MyUtility.Convert.GetString(row["stocktype", DataRowVersion.Original]);
                if (row.RowState == DataRowState.Modified && isTransferIn_DetailKewordChanged)
                {
                    if (MyUtility.Check.Seek(string.Format(
                        @"select * from TransferIn_Detail where poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and Roll = '{3}' and Dyelot = '{4}' and stocktype = '{5}'",
                        row["poid"], row["seq1"], row["seq2"], row["Roll"], row["Dyelot"], row["stocktype"])))
                    {
                        warningmsg.Append(string.Format(@"<SP>: {0} <Seq>: {1}-{2}  <ROLL> {3}<DYELOT>{4} exists, cannot be saved!", row["poid"], row["seq1"], row["seq2"], row["Roll"], row["Dyelot"]));
                        warningmsg.Append(Environment.NewLine);
                    }
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            // Check Roll 是否有重複
            if (!this.checkRoll())
            {
                return false;
            }

            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "TI", "TransferIn", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (this.CurrentMaintain["status"].EqualString("Confirmed"))
            {
                this.toolbar.cmdEdit.Enabled = false;
            }
            else
            {
                this.toolbar.cmdEdit.Enabled = true;
            }

            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentDetailData["Stocktype"] = 'B';
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region -- Seq 右鍵開窗 --

            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    IList<DataRow> x;

                    DataTable dt;
                    string sqlcmd;
                    if (this.CurrentDetailData["DataFrom"].Equals("Po_Supp_Detail"))
                    {
                        sqlcmd = string.Format(
                            @"
select  poid = p.ID 
        , seq = concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2)
        , p.seq1
        , p.seq2
        , p.Refno
        , Description = (select f.DescDetail from fabric f WITH (NOLOCK) where f.SCIRefno = p.scirefno) 
        , p.scirefno
        , p.FabricType
        , stockunit = dbo.GetStockUnitBySPSeq (p.ID, p.seq1, p.seq2)
from dbo.Po_Supp_Detail p WITH (NOLOCK) 
where p.ID ='{0}'", this.CurrentDetailData["poid"].ToString());
                    }
                    else
                    {
                        sqlcmd = string.Format(
                            @"
select  poid = I.InventoryPOID 
        , seq = concat(Ltrim(Rtrim(I.InventorySeq1)), ' ', I.InventorySeq2)
        , seq1 = I.InventorySeq1
        , seq2 = I.InventorySeq2
        , I.Refno
        , Description = ''
        , I.FabricType
        , stockunit = dbo.GetStockUnitBySPSeq (I.InventoryPOID, I.InventorySeq1, I.InventorySeq2)
from dbo.Invtrans I WITH (NOLOCK) 
where I.InventoryPOID ='{0}' and I.type = '3' and FactoryID = '{1}'", this.CurrentDetailData["poid"].ToString(), this.CurrentMaintain["FromFtyID"]);
                    }

                    DBProxy.Current.Select(null, sqlcmd, out dt);

                    Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(
                        dt,
                        "Seq,refno,description",
                        "6,8,20", this.CurrentDetailData["seq"].ToString(), "Seq,Ref#,Description");
                    selepoitem.Width = 480;

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
                    this.CurrentDetailData["fabrictype"] = x[0]["fabrictype"];
                    this.CurrentDetailData.EndEdit();
                }
            };

            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["seq"] = string.Empty;
                        this.CurrentDetailData["seq1"] = string.Empty;
                        this.CurrentDetailData["seq2"] = string.Empty;

                        // CurrentDetailData["Roll"] = "";
                        // CurrentDetailData["Dyelot"] = "";
                        this.CurrentDetailData["stockunit"] = string.Empty;
                        this.CurrentDetailData["Description"] = string.Empty;
                        this.CurrentDetailData["fabrictype"] = string.Empty;
                    }
                    else
                    {
                        DualResult result = P18_Utility.CheckDetailSeq(e.FormattedValue.ToString(), this.CurrentMaintain["FromFtyID"].ToString(), this.CurrentDetailData);

                        if (!result)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(result.Description);
                            return;
                        }

                        // CurrentDetailData["Roll"] = "";
                        // CurrentDetailData["Dyelot"] = "";
                    }
                }
            };
            #endregion Seq 右鍵開窗
            #region StockType
            DataGridViewGeneratorComboBoxColumnSettings sk = new DataGridViewGeneratorComboBoxColumnSettings();
            sk.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    // 去除錯誤的Location將正確的Location填回
                    string newLocation = string.Empty;
                    DualResult result = P18_Utility.CheckDetailStockTypeLocation(e.FormattedValue.ToString(), this.CurrentDetailData["Location"].ToString(), out newLocation);
                    if (!result)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(result.Description);
                    }

                    this.CurrentDetailData["stocktype"] = e.FormattedValue;
                    this.CurrentDetailData["Location"] = newLocation;
                }
            };
            #endregion
            #region -- Location 右鍵開窗 --

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.CurrentDetailData["stocktype"].ToString(), this.CurrentDetailData["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    // CurrentDetailData["location"] = item.GetSelectedString();
                    this.detailgrid.GetDataRow(e.RowIndex)["location"] = item.GetSelectedString();
                    this.detailgrid.GetDataRow(e.RowIndex).EndEdit();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    // 去除錯誤的Location將正確的Location填回
                    string newLocation = string.Empty;
                    DualResult result = P18_Utility.CheckDetailStockTypeLocation(this.CurrentDetailData["stocktype"].ToString(), e.FormattedValue.ToString(), out newLocation);
                    if (!result)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(result.Description);
                    }

                    this.CurrentDetailData["Location"] = newLocation;
                }
            };
            #endregion Location 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts3 = new DataGridViewGeneratorTextColumnSettings();
            ts3.CellValidating += (s, e) =>
            {
                if (this.EditMode == true && string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    this.CurrentDetailData["seq"] = string.Empty;
                    this.CurrentDetailData["seq1"] = string.Empty;
                    this.CurrentDetailData["seq2"] = string.Empty;

                    // this.CurrentDetailData["roll"] = "";
                    // this.CurrentDetailData["dyelot"] = "";
                    this.CurrentDetailData["poid"] = string.Empty;
                    this.CurrentDetailData["DataFrom"] = string.Empty;

                    // this.CurrentDetailData["qty"] = 0;
                    return;
                }

                if (this.EditMode == true && string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["poid"].ToString()) != 0)
                {
                    string dataFrom = "Po_Supp_Detail";

                    DualResult checkResult = P18_Utility.CheckDetailPOID(e.FormattedValue.ToString(), this.CurrentMaintain["FromFtyID"].ToString(), out dataFrom);

                    if (!checkResult)
                    {
                        MyUtility.Msg.WarningBox(checkResult.Description, e.FormattedValue.ToString());
                        return;
                    }

                    this.CurrentDetailData["seq"] = string.Empty;
                    this.CurrentDetailData["seq1"] = string.Empty;
                    this.CurrentDetailData["seq2"] = string.Empty;

                    // this.CurrentDetailData["roll"] = "";
                    // this.CurrentDetailData["dyelot"] = "";
                    this.CurrentDetailData["poid"] = e.FormattedValue;
                    this.CurrentDetailData["DataFrom"] = dataFrom;
                }
            };

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Roll;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Dyelot;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), settings: ts3) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts) // 1
            .Text("Fabric", header: "Fabric \r\n Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6)).Get(out cbb_Roll) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8)).Get(out cbb_Dyelot) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10) // 5
            .Numeric("qty", header: "In Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10) // 6
            .Text("stockunit", header: "Unit", iseditingreadonly: true) // 7
            .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), settings: sk).Get(out cbb_stocktype) // 8
            .Text("Location", header: "Location", iseditingreadonly: false, settings: ts2) // 9
            .Text("Remark", header: "Remark", iseditingreadonly: false) // 10
            ;
            #endregion 欄位設定
            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
            cbb_Roll.MaxLength = 8;
            cbb_Dyelot.MaxLength = 8;

            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Weight"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string upd_MD_2T = string.Empty;
            string upd_MD_8T = string.Empty;
            string upd_Fty_2T = string.Empty;

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            // Check Roll 是否有重複
            if (!this.checkRoll())
            {
                return;
            }

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0),f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   f.lock=1 
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0),f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) + d.Qty < 0) 
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than In qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查不存在的po_supp_detail資料，並新增
            sqlcmd = string.Format(
                @"
Select  distinct d.poid
        , d.seq1
        , d.seq2
from dbo.TransferIn_Detail d WITH (NOLOCK) 
left join dbo.PO_Supp_Detail f WITH (NOLOCK) on d.PoId = f.Id
                                                and d.Seq1 = f.Seq1
                                                and d.Seq2 = f.seq2
where   d.Id = '{0}' 
        and f.id is null", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"
update TransferIn 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新庫存數量 MDivisionPoDetail --
            var data_MD_2T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
                                  poid = m.First().Field<string>("poid"),
                                  seq1 = m.First().Field<string>("seq1"),
                                  seq2 = m.First().Field<string>("seq2"),
                                  stocktype = m.First().Field<string>("stocktype"),
                                  qty = m.Sum(w => w.Field<decimal>("qty")),
                                  location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();
            var data_MD_8T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                              group b by new
                              {
                                  poid = b.Field<string>("poid").Trim(),
                                  seq1 = b.Field<string>("seq1").Trim(),
                                  seq2 = b.Field<string>("seq2").Trim(),
                                  stocktype = b.Field<string>("stocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("poid"),
                                  seq1 = m.First().Field<string>("seq1"),
                                  seq2 = m.First().Field<string>("seq2"),
                                  stocktype = m.First().Field<string>("stocktype"),
                                  qty = m.Sum(w => w.Field<decimal>("qty")),
                                  location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();

            #endregion
            #region -- 更新庫存數量  ftyinventory --
            DataTable newDt = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dtr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                string[] dtrLocation = dtr["Location"].ToString().Split(',');
                dtrLocation = dtrLocation.Distinct().ToArray();

                if (dtrLocation.Length == 1)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["Location"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            int MtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system")) ? 1 : 0;
            var data_Fty_2T = (from b in newDt.AsEnumerable()
                               select new
                               {
                                   poid = b.Field<string>("poid"),
                                   seq1 = b.Field<string>("seq1"),
                                   seq2 = b.Field<string>("seq2"),
                                   stocktype = b.Field<string>("stocktype"),
                                   qty = b.Field<decimal>("qty"),
                                   location = b.Field<string>("location"),
                                   roll = b.Field<string>("roll"),
                                   dyelot = b.Field<string>("dyelot"),
                               }).ToList();

            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true, MtlAutoLock);
            #endregion 更新庫存數量  ftyinventory

            #region 更新 Po_Supp_Detail StockUnit
            string sql_UpdatePO_Supp_Detail = @";
alter table #Tmp alter column poid varchar(20)
alter table #Tmp alter column seq1 varchar(3)
alter table #Tmp alter column seq2 varchar(3)
alter table #Tmp alter column StockUnit varchar(20)

select  distinct poid
        , seq1
        , seq2
        , StockUnit 
into #tmpD 
from #Tmp

merge dbo.PO_Supp_Detail as target
using #tmpD as src on   target.ID = src.poid 
                        and target.seq1 = src.seq1 
                        and target.seq2 =src.seq2 
when matched then
    update
    set target.StockUnit = src.StockUnit;
";
            #endregion

            #region 更新FIR,AIR資料

            List<SqlParameter> Fir_Air_Proce = new List<SqlParameter>();
            Fir_Air_Proce.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"]));
            Fir_Air_Proce.Add(new SqlParameter("@LoginID", Sci.Env.User.UserID));

            if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.insert_Air_Fir_TnsfIn", Fir_Air_Proce)))
            {
                Exception ex = result.GetException();
                MyUtility.Msg.InfoBox(ex.Message.Substring(ex.Message.IndexOf("Error Message:") + "Error Message:".Length));
                return;
            }
            #endregion

            #region -- Transaction --
            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            try
            {
                using (_transactionscope)
                using (sqlConn)
                {
                    /*
                        * 先更新 FtyInventory 後更新 MDivisionPoDetail
                        * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                        * 因為要在同一 SqlConnection 之下執行
                        */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_2T.Count > 0)
                    {
                        upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (data_MD_8T.Count > 0)
                    {
                        upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }
                    #endregion

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                         (DataTable)this.detailgridbs.DataSource, string.Empty, sql_UpdatePO_Supp_Detail, out resulttb, "#tmp", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
            }
            catch (Exception ex)
            {
                _transactionscope.Dispose();
                this.ShowErr("Commit transaction error.", ex);
            }
            #endregion
            _transactionscope.Dispose();
            _transactionscope = null;
        }

        // Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            string upd_MD_2F = string.Empty;
            string upd_MD_8F = string.Empty;
            string upd_Fty_2F = string.Empty;

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

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0),f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   f.lock=1 
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0),f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - d.Qty < 0) 
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3}  Dyelot#: {6}'s balance: {4} is less than In qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"
update TransferIn 
set status = 'New'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新庫存數量 MDivisionPoDetail --
            var data_MD_2F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
                                  poid = m.First().Field<string>("poid"),
                                  seq1 = m.First().Field<string>("seq1"),
                                  seq2 = m.First().Field<string>("seq2"),
                                  stocktype = m.First().Field<string>("stocktype"),
                                  qty = -m.Sum(w => w.Field<decimal>("qty")),
                                  location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();
            var data_MD_8F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                              group b by new
                              {
                                  poid = b.Field<string>("poid").Trim(),
                                  seq1 = b.Field<string>("seq1").Trim(),
                                  seq2 = b.Field<string>("seq2").Trim(),
                                  stocktype = b.Field<string>("stocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("poid"),
                                  seq1 = m.First().Field<string>("seq1"),
                                  seq2 = m.First().Field<string>("seq2"),
                                  stocktype = m.First().Field<string>("stocktype"),
                                  qty = -m.Sum(w => w.Field<decimal>("qty")),
                                  location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();

            #endregion
            #region -- 更新庫存數量  ftyinventory --

            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("poid"),
                                   seq1 = m.Field<string>("seq1"),
                                   seq2 = m.Field<string>("seq2"),
                                   stocktype = m.Field<string>("stocktype"),
                                   qty = -m.Field<decimal>("qty"),
                                   roll = m.Field<string>("roll"),
                                   dyelot = m.Field<string>("dyelot"),
                               }).ToList();
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region -- Transaction --
            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (_transactionscope)
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_2F.Count > 0)
                    {
                        upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (data_MD_8F.Count > 0)
                    {
                        upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }
                    #endregion

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion
            _transactionscope.Dispose();
            _transactionscope = null;
        }

        // 寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string fromFty = (e.Master == null) ? string.Empty : e.Master["FromFtyID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  a.id
        , a.PoId
        , a.Seq1
        , a.Seq2
        , seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
        , a.Roll
        , a.Dyelot
        , [Description] = dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0)
        , StockUnit = dbo.GetStockUnitBySPSeq (a.poid, a.seq1, a.seq2)
        , a.Qty
        , a.StockType
        , a.location
        , a.ukey
        , FabricType = isnull(p.FabricType,I.FabricType)
        , DataFrom = iif(p.FabricType is null,'Invtrans','Po_Supp_Detail')
		,a.Weight
		,a.Remark
        ,[Fabric] = case when p.FabricType = 'F' then 'Fabric' 
                             when p.FabricType = 'A' then 'Accessory'
                        else '' end
from dbo.TransferIn_Detail a WITH (NOLOCK) 
left join Po_Supp_Detail p WITH (NOLOCK)  on a.poid = p.id
                              and a.seq1 = p.seq1
                              and a.seq2 = p.seq2
outer apply ( select top 1 FabricType from Invtrans I WITH (NOLOCK)  
             where a.poid = I.InventoryPOID and a.seq1 = I.InventorySeq1 and a.seq2 = I.InventorySeq2 and I.FactoryID = '{1}' and I.type = '3' ) I
Where a.id = '{0}'", masterID, fromFty);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void btnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Accumulated Form
        private void btnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P18_AccumulatedQty(this.CurrentMaintain);
            frm.P18 = this;
            frm.ShowDialog(this);
        }

        // Find
        private void btnFind_Click(object sender, EventArgs e)
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new P18_ExcelImport(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void txtFromFactory_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFromFactory.Text))
            {
                return;
            }

            if (!MyUtility.Check.Seek(string.Format(@"select * from scifty WITH (NOLOCK) where id='{0}'", this.txtFromFactory.Text)))
            {
                this.txtFromFactory.Text = string.Empty;
                MyUtility.Msg.WarningBox("From Factory : " + this.txtFromFactory.Text + " not found!");
                this.txtFromFactory.Focus();
                this.txtFromFactory.Select();
            }
        }

        private void txtFromFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string cmd = "select ID from scifty WITH (NOLOCK) where mdivisionid<>'' and Junk<>1 order by MDivisionID,ID ";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(cmd, "6", this.txtFromFactory.ToString());
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtFromFactory.Text = item.GetSelectedString();
        }

        /// <summary>
        /// 確認 SP# & Seq 是否已經有重複的 Roll
        /// </summary>
        /// <returns>bool</returns>
        private bool checkRoll()
        {
            // 判斷是否已經收過此種布料SP#,SEQ,Roll不能重複收
            List<string> listMsg = new List<string>();
            List<string> listDyelot = new List<string>();
            foreach (DataRow row in this.DetailDatas)
            {
                DualResult result = P18_Utility.CheckRollExists(this.CurrentMaintain["id"].ToString(), row);

                if (!result)
                {
                    listMsg.Add(result.Description);
                }
            }

            if (listMsg.Count > 0)
            {
                DialogResult Dr = MyUtility.Msg.WarningBox(listMsg.JoinToString(string.Empty).TrimStart());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 表身新增資料,會將上一筆資料copy並填入新增的資料列裡
        /// </summary>
        protected override void OnDetailGridAppendClick()
        {
            base.OnDetailGridAppendClick();
            DataRow lastRow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex() - 1);
            if (MyUtility.Check.Empty(lastRow))
            {
                return;
            }

            DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.CurrentRow.Cells[1].RowIndex);
            newrow["id"] = lastRow["id"];
            newrow["poid"] = lastRow["poid"];
            newrow["seq1"] = lastRow["seq1"];
            newrow["seq2"] = lastRow["seq2"];
            newrow["seq"] = lastRow["seq"];
            newrow["Dyelot"] = lastRow["Dyelot"];
            newrow["Description"] = lastRow["Description"];
            newrow["StockUnit"] = lastRow["StockUnit"];
            newrow["Qty"] = lastRow["Qty"];
            newrow["StockType"] = lastRow["StockType"];
            newrow["Location"] = lastRow["Location"];
            newrow["FabricType"] = lastRow["FabricType"];
            newrow["DataFrom"] = lastRow["DataFrom"];
        }
    }
}