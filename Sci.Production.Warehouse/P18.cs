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
    /// <inheritdoc/>
    public partial class P18 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private ReportViewer viewer;
        private bool IsAutomation;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Roll;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Dyelot;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ttlqty;

        /// <inheritdoc/>
        public P18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
            this.viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
            };
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");

            this.Controls.Add(this.viewer);
            this.IsAutomation = UtilityAutomation.IsAutomationEnable;

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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
            string remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            string cmdd = @"
select  b.name 
from dbo.Transferin  a WITH (NOLOCK) 
inner join dbo.mdivision  b WITH (NOLOCK) on b.id = a.mdivisionid
where   b.id = a.mdivisionid
        and a.id = @ID";
            DualResult result = DBProxy.Current.Select(string.Empty, cmdd, pars, out dt);
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
                string.Format(@"select NameEN from MDivision where ID='{0}'", Env.User.Keyword), out dtNAME);
            string rptTitle = dtNAME.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("FromFtyID", fromFactory));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("IssueDate", issuedate));

            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion
            #region -- 撈表身資料 --
            DataTable dtDetail;
            string tmp = @"
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
where a.id = @ID";
            result = DBProxy.Current.Select(string.Empty, tmp, pars, out dtDetail);
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
            Type reportResourceNamespace = typeof(P18_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P18_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report)
            {
                MdiParent = this.MdiParent,
            };
            frm.Show();

            return true;
        }

        // print for SubReport
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

                // check 相同CombineBarcode, Refno, Color 是否一致
                if (!MyUtility.Check.Empty(row["CombineBarcode"]) && row["FabricType"].ToString() == "F")
                {
                    // 取出原始資料
                    DataTable dtOriginal = this.DetailDatas.CopyToDataTable().AsEnumerable().Where(r =>
                        r["FabricType"].ToString() == "F" &&
                        MyUtility.Check.Empty(r["Unoriginal"]) &&
                        r["CombineBarcode"].ToString() == row["CombineBarcode"].ToString())
                    .CopyToDataTable();
                    if (dtOriginal.Rows.Count > 0)
                    {
                        if ((string.Compare(row["Refno"].ToString().Trim(), dtOriginal.Rows[0]["Refno"].ToString().Trim()) != 0 ||
                        string.Compare(row["ColorID"].ToString().Trim(), dtOriginal.Rows[0]["ColorID"].ToString().Trim()) != 0) &&
                        row["FabricType"].ToString() == "F")
                        {
                            MyUtility.Msg.WarningBox("[Refno] & [Color] must be the same in same source data。");
                            return false;
                        }
                    }
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

            // Check FtyInventory 是否已經存在
            if (!this.ChkFtyInventory_Exists())
            {
                return false;
            }

            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "TI", "TransferIn", (DateTime)this.CurrentMaintain["Issuedate"]);
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

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.Change_record();
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

            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }

            this.IsAutomation = UtilityAutomation.IsAutomationEnable;
            this.Change_record();
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
        , p.ColorID
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
        , ColorID=''
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
                        "6,8,20", this.CurrentDetailData["seq"].ToString(), "Seq,Ref#,Description")
                    {
                        Width = 480,
                    };

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
                    this.CurrentDetailData["fabric"] = MyUtility.Check.Empty(x[0]["fabrictype"]) ? string.Empty : x[0]["fabrictype"].ToString().ToUpper() == "F" ? "Fabric" : "Accessory";
                    this.CurrentDetailData["Refno"] = x[0]["Refno"];
                    this.CurrentDetailData["ColorID"] = x[0]["ColorID"];
                    this.CurrentDetailData.EndEdit();
                }
            };

            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                string oldValue = this.CurrentDetailData["seq"].ToString();
                string newValue = e.FormattedValue.ToString();
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
                        string poid = MyUtility.Convert.GetString(this.CurrentDetailData["poid"]);
                        string seq1 = MyUtility.Convert.GetString(this.CurrentDetailData["seq1"]);
                        string seq2 = MyUtility.Convert.GetString(this.CurrentDetailData["seq2"]);
                        string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                        string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                        string fabricType = MyUtility.Convert.GetString(this.CurrentDetailData["fabrictype"]);
                        string stockType = MyUtility.Convert.GetString(this.CurrentDetailData["stockType"]);

                        // 判斷 物料 是否為 布，布料才需要 Roll &Dyelot
                        if (fabricType.ToUpper() == "F" && !MyUtility.Check.Empty(poid) && !MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2) && !MyUtility.Check.Empty(roll) && !MyUtility.Check.Empty(dyelot))
                        {
                            // 判斷 在 FtyInventory 是否存在
                            bool chkFtyInventory = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                            if (!chkFtyInventory)
                            {
                                MyUtility.Msg.WarningBox($"The Roll & Dyelot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");
                                this.CurrentDetailData["seq"] = oldValue;
                                this.CurrentDetailData["seq1"] = oldValue.Split(' ')[0];
                                this.CurrentDetailData["seq2"] = oldValue.Split(' ')[1];
                            }
                        }

                        DualResult result = P18_Utility.CheckDetailSeq(e.FormattedValue.ToString(), this.CurrentMaintain["FromFtyID"].ToString(), this.CurrentDetailData);

                        if (!result)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(result.Description);
                            return;
                        }
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
            #region SP#
            DataGridViewGeneratorTextColumnSettings ts3 = new DataGridViewGeneratorTextColumnSettings();
            ts3.CellValidating += (s, e) =>
            {
                if (this.EditMode == true && string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    this.CurrentDetailData["seq"] = string.Empty;
                    this.CurrentDetailData["seq1"] = string.Empty;
                    this.CurrentDetailData["seq2"] = string.Empty;
                    this.CurrentDetailData["poid"] = string.Empty;
                    this.CurrentDetailData["DataFrom"] = string.Empty;
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

                    string poid = MyUtility.Convert.GetString(this.CurrentDetailData["poid"]);
                    string seq1 = MyUtility.Convert.GetString(this.CurrentDetailData["seq1"]);
                    string seq2 = MyUtility.Convert.GetString(this.CurrentDetailData["seq2"]);
                    string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                    string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                    string fabricType = MyUtility.Convert.GetString(this.CurrentDetailData["fabrictype"]);
                    string stockType = MyUtility.Convert.GetString(this.CurrentDetailData["stockType"]);

                    // 判斷 物料 是否為 布，布料才需要 Roll &Dyelot
                    if (fabricType.ToUpper() == "F")
                    {
                        // 判斷 在 FtyInventory 是否存在
                        bool chkFtyInventory1 = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                        if (!chkFtyInventory1)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox($"The Roll & Dyelot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");
                            return;
                        }
                    }

                    this.CurrentDetailData["seq"] = string.Empty;
                    this.CurrentDetailData["seq1"] = string.Empty;
                    this.CurrentDetailData["seq2"] = string.Empty;
                    this.CurrentDetailData["poid"] = e.FormattedValue;
                    this.CurrentDetailData["DataFrom"] = dataFrom;
                }
            };
            #endregion
            #region Roll

            Ict.Win.DataGridViewGeneratorTextColumnSettings roll_setting = new DataGridViewGeneratorTextColumnSettings();
            roll_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                string combineBarcode = MyUtility.Convert.GetString(this.CurrentDetailData["CombineBarcode"]);

                this.CurrentDetailData["Roll"] = newvalue;
                if (!MyUtility.Check.Empty(combineBarcode))
                {
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        if (string.Compare(dr["CombineBarcode"].ToString(), combineBarcode) == 0)
                        {
                            dr["Roll"] = newvalue;
                        }
                    }
                }
            };

            #endregion
            #region Dyelot

            Ict.Win.DataGridViewGeneratorTextColumnSettings dyelot_setting = new DataGridViewGeneratorTextColumnSettings();

            dyelot_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                string combineBarcode = MyUtility.Convert.GetString(this.CurrentDetailData["CombineBarcode"]);

                this.CurrentDetailData["dyelot"] = newvalue;
                if (!MyUtility.Check.Empty(combineBarcode))
                {
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        if (string.Compare(dr["CombineBarcode"].ToString(), combineBarcode) == 0)
                        {
                            dr["dyelot"] = newvalue;
                        }
                    }
                }
            };
            #endregion
            #region In Qty
            Ict.Win.DataGridViewGeneratorNumericColumnSettings qty_setting = new DataGridViewGeneratorNumericColumnSettings();

            qty_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                decimal newvalue = MyUtility.Convert.GetDecimal(e.FormattedValue);
                this.CurrentDetailData["qty"] = newvalue;
                string combineBarcode = MyUtility.Convert.GetString(this.CurrentDetailData["CombineBarcode"]);
                decimal ttlValue = 0;
                if (!MyUtility.Check.Empty(combineBarcode))
                {
                    ttlValue = (decimal)this.DetailDatas.CopyToDataTable().Compute("sum(qty)", $"CombineBarcode = '{combineBarcode}'");
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        if (MyUtility.Check.Empty(dr["Unoriginal"]) &&
                            string.Compare(combineBarcode, dr["CombineBarcode"].ToString()) == 0)
                        {
                            dr["TtlQty"] = ttlValue + $" {dr["stockunit"]}";
                            dr.EndEdit();
                        }
                    }
                }
                else
                {
                    this.CurrentDetailData["TtlQty"] = e.FormattedValue + $" {this.CurrentDetailData["stockunit"]}";
                }
            };
            #endregion

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), settings: ts3) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts) // 1
            .Text("Fabric", header: "Fabric \r\n Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), settings: roll_setting).Get(out this.col_Roll) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), settings: dyelot_setting).Get(out this.col_Dyelot) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10) // 5
            .Numeric("ActualWeight", header: "Act.(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10) // 5
            .Numeric("qty", header: "In Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, settings: qty_setting) // 6
            .Text("stockunit", header: "Unit", iseditingreadonly: true) // 7
            .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true).Get(out this.col_ttlqty) // 6
            .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), settings: sk).Get(out cbb_stocktype) // 8
            .Text("Location", header: "Location", iseditingreadonly: false, settings: ts2) // 9
            .Text("Remark", header: "Remark", iseditingreadonly: false) // 10
            .Text("RefNo", header: "Ref#", iseditingreadonly: true)
            .Text("ColorID", header: "Color", iseditingreadonly: true)
            ;
            #endregion 欄位設定
            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
            this.col_Roll.MaxLength = 8;
            this.col_Dyelot.MaxLength = 8;

            #region Add Column [btnAdd2]

            DataGridViewButtonColumn col_btnAdd2 = new DataGridViewButtonColumn();
            DataGridViewButtonCell cell = new DataGridViewButtonCell();
            col_btnAdd2.CellTemplate = cell;
            col_btnAdd2.Name = "btnAdd2";
            col_btnAdd2.HeaderText = string.Empty;
            col_btnAdd2.DataPropertyName = "btnAdd2";
            col_btnAdd2.Width = 30;
            this.Change_record();
            this.detailgrid.Columns.Add(col_btnAdd2);
            if (this.detailgrid != null)
            {
                if (this.detailgrid.Columns["btnAdd2"] != null)
                {
                    this.detailgrid.Columns["btnAdd2"].DisplayIndex = 0; // index 0
                }
            }
            #endregion

            this.detailgrid.CellClick += this.Detailgrid_CellClick;
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
            this.detailgrid.ColumnHeaderMouseClick += this.Detailgrid_ColumnHeaderMouseClick;

            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Weight"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ActualWeight"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void Detailgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.EditMode == false || e.ColumnIndex < 0 || this.detailgrid == null)
            {
                return;
            }

            DataRow pre_row = this.detailgrid.GetDataRow(this.detailgridbs.Position);

            // 要主料才能使用+-按鈕功能
            if (this.detailgrid.Columns[e.ColumnIndex].Name == "btnAdd2")
            {
                DataGridViewButtonCell pre_dgbtn = (DataGridViewButtonCell)this.detailgrid.Rows[e.RowIndex].Cells["btnAdd2"];
                DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;
                string maxCombBarcode = dtDetail.Compute("Max(CombineBarcode)", string.Empty).ToString();

                if (MyUtility.Check.Empty(pre_row))
                {
                    return;
                }

                if (pre_dgbtn.Value.ToString() == "+" &&
                    (pre_row["FabricType"].ToString() == "F" || MyUtility.Check.Empty(pre_row["FabricType"])))
                {
                    // 取得CombineBarcode
                    string pre_ComBarcode = pre_row["CombineBarcode"].ToString();
                    if (MyUtility.Check.Empty(maxCombBarcode))
                    {
                        pre_ComBarcode = "1";
                    }
                    else
                    {
                        if (MyUtility.Check.Empty(pre_ComBarcode))
                        {
                            // New Max Value
                            pre_ComBarcode = Prgs.GetNextValue(maxCombBarcode, 1);
                        }
                    }

                    pre_row["CombineBarcode"] = pre_ComBarcode;
                    pre_row.EndEdit();

                    // 新增下一筆資料
                    base.OnDetailGridInsert(this.detailgridbs.Position + 1);

                    DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.CurrentRow.Cells["btnAdd2"].RowIndex);
                    newrow["Dyelot"] = pre_row["Dyelot"];
                    newrow["Roll"] = pre_row["Roll"];
                    newrow["Unoriginal"] = 1;
                    newrow["MDivisionID"] = Env.User.Keyword;
                    newrow["Stocktype"] = 'B';
                    newrow["CombineBarcode"] = pre_ComBarcode;
                    DataGridViewButtonCell next_dgbtn = (DataGridViewButtonCell)this.detailgrid.CurrentRow.Cells["btnAdd2"];
                    next_dgbtn.Value = "-";
                    this.Change_record();
                }
                else if (pre_dgbtn.Value.ToString() == "-")
                {
                    // 刪除該筆資料
                    this.OnDetailGridDelete();
                }
            }
        }

        private void Detailgrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Change_record();
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.EditMode == false)
            {
                return;
            }

            var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            // Unoriginal= true 非原生資料行, Roll,Dyelot不能編輯
            if (!MyUtility.Check.Empty(data["Unoriginal"]))
            {
                this.col_Roll.IsEditingReadOnly = true;
                this.col_Dyelot.IsEditingReadOnly = true;
            }
            else
            {
                this.col_Roll.IsEditingReadOnly = false;
                this.col_Dyelot.IsEditingReadOnly = false;
            }
        }

        private void Change_record()
        {
            if (this.DetailDatas == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            DataTable tmp_dt = this.DetailDatas.CopyToDataTable();
            if (tmp_dt == null)
            {
                return;
            }

            for (int index = 0; index < tmp_dt.Rows.Count; index++)
            {
                // 判斷原生的為+, copy為-
                if (MyUtility.Check.Empty(tmp_dt.Rows[index]["Unoriginal"]))
                {
                    this.detailgrid.Rows[index].Cells["btnAdd2"].Value = "+";
                }
                else
                {
                    this.detailgrid.Rows[index].Cells["btnAdd2"].Value = "-";
                }
            }
        }

        // Confirm

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

            string upd_MD_2T = string.Empty;
            string upd_MD_8T = string.Empty;
            string upd_Fty_2T = string.Empty;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            // Check Roll 是否有重複
            if (!this.ChkFtyInventory_Exists())
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

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "TransferIn_Detail"))
            {
                return;
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
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Stocktype = m.First().Field<string>("stocktype"),
                                  Qty = m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
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
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Stocktype = m.First().Field<string>("stocktype"),
                                  Qty = m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();

            #endregion
            #region -- 更新庫存數量  ftyinventory --

            int mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system")) ? 1 : 0;
            var data_Fty_2T = (from b in this.DetailDatas
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

            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true, mtlAutoLock);
            #endregion 更新庫存數量  ftyinventory

            #region 更新BarCode  Ftyinventory

            List<string> barcodeList = new List<string>();
            DataTable dtCnt = (DataTable)this.detailgridbs.DataSource;

            // distinct CombineBarcode,並排除CombineBarcode = null
            DataRow[] distCnt1 = dtCnt.DefaultView.ToTable(true, "CombineBarcode", "FabricType").Select("FabricType = 'F' and CombineBarcode is not null");
            DataRow[] count2 = dtCnt.Select("FabricType = 'F' and CombineBarcode is null");
            if (distCnt1.Length + count2.Length > 0)
            {
                barcodeList = Prgs.GetBarcodeNo("FtyInventory", "F", distCnt1.Length + count2.Length);
                int cnt = 0;

                // 排序CombineBarcode, 將所有未展開主料置頂
                ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "CombineBarcode";
                foreach (DataRow drDis in this.DetailDatas)
                {
                    if (string.Compare(drDis["FabricType"].ToString(), "F") == 0 && MyUtility.Check.Empty(drDis["Barcode"]))
                    {
                        if (MyUtility.Check.Empty(drDis["CombineBarcode"]))
                        {
                            drDis["Barcode"] = barcodeList[cnt];
                        }
                        else
                        {
                            // 相同CombinBarcode, 則Barcode要寫入一樣的!
                            foreach (var item in this.DetailDatas)
                            {
                                if (string.Compare(drDis["CombineBarcode"].ToString(), item["CombineBarcode"].ToString()) == 0)
                                {
                                    item["Barcode"] = barcodeList[cnt];
                                }
                            }
                        }

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

            List<SqlParameter> fir_Air_Proce = new List<SqlParameter>();
            fir_Air_Proce.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"]));
            fir_Air_Proce.Add(new SqlParameter("@LoginID", Env.User.UserID));

            if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.insert_Air_Fir_TnsfIn", fir_Air_Proce)))
            {
                Exception ex = result.GetException();
                MyUtility.Msg.InfoBox(ex.Message.Substring(ex.Message.IndexOf("Error Message:") + "Error Message:".Length));
                return;
            }
            #endregion

            #region -- Transaction --
            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            try
            {
                using (transactionscope)
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
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    // 更新FtyInventory Barcode
                    if (data_Fty_Barcode.Count >= 1)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V1, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode_V2, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_2T.Count > 0)
                    {
                        upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (data_MD_8T.Count > 0)
                    {
                        upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }
                    #endregion

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                         (DataTable)this.detailgridbs.DataSource, string.Empty, sql_UpdatePO_Supp_Detail, out resulttb, "#tmp", conn: sqlConn)))
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
            }
            catch (Exception ex)
            {
                transactionscope.Dispose();
                this.ShowErr("Commit transaction error.", ex);
            }
            #endregion
            transactionscope.Dispose();
            transactionscope = null;

            // AutoWHFabric WebAPI for Gensong
            this.SentToGensong_AutoWHFabric(true);
            this.SentToVstrong_AutoWH_ACC(true);
        }

        // Unconfirm

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "TransferIn_Detail"))
            {
                return;
            }
            #endregion 檢查庫存項WMSLock

            #region UnConfirmed 先檢查WMS是否傳送成功
            DataTable dtDetail = new DataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                string sqlGetData = string.Empty;
                sqlGetData = $@"
SELECT 
 [ID] = rd.id
,[InvNo] = r.InvNo
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[StockUnit] = dbo.GetStockUnitBySPSeq(rd.POID,rd.Seq1,rd.Seq2)
,[StockQty] = rd.Qty
,[PoUnit] = ''
,[ShipQty] = 0.00
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[MtlType] = Fabric.MtlTypeID
,[Ukey] = rd.Ukey
,[ETA] = null
,[WhseArrival] = r.IssueDate
,[Status] = 'Delete'
FROM Production.dbo.TransferIn_Detail rd
inner join Production.dbo.TransferIn r on rd.id = r.id
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
	and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
left join Production.dbo.FtyInventory f on f.POID = rd.PoId
	and f.Seq1=rd.Seq1 and f.Seq2=rd.Seq2 
	and f.Dyelot = rd.Dyelot and f.Roll = rd.Roll
	and f.StockType = rd.StockType
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='A'
)
and r.id = '{this.CurrentMaintain["id"]}'
";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                if (!Vstrong_AutoWHAccessory.SentReceive_Detail_Delete(dtDetail, "P08"))
                {
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
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Stocktype = m.First().Field<string>("stocktype"),
                                  Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
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
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Stocktype = m.First().Field<string>("stocktype"),
                                  Qty = -m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
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

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(this.CurrentMaintain["id"].ToString(), "TransferIn_Detail"))
            {
                return;
            }
            #endregion

            #region -- Transaction --
            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (transactionscope)
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
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_2F.Count > 0)
                    {
                        upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (data_MD_8F.Count > 0)
                    {
                        upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }
                    #endregion

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
            #endregion
            transactionscope.Dispose();
            transactionscope = null;
        }

        /// <summary>
        ///  AutoWHFabric WebAPI for Gensong
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void SentToGensong_AutoWHFabric(bool isConfirmed)
        {
            DataTable dtDetail = new DataTable();
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                string sqlGetData = string.Empty;
                sqlGetData = $@"
SELECT [ID] = td.id
,[InvNo] = isnull(t.InvNo,'')
,[PoId] = td.Poid
,[Seq1] = td.Seq1
,[Seq2] = td.Seq2
,[Refno] = po3.Refno
,[ColorID] = po3.ColorID
,[Roll] = td.Roll
,[Dyelot] = td.Dyelot
,[StockUnit] = dbo.GetStockUnitBySPSeq(td.POID,td.Seq1,td.Seq2)
,[StockQty] = td.Qty
,[PoUnit] = po3.PoUnit
,[ShipQty] = td.Qty
,[Weight] = td.Weight
,[StockType] = td.StockType
,[Ukey] = td.Ukey
,[IsInspection] = convert(bit, 0)
,[ETA] = null
,[WhseArrival] = t.IssueDate
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,[Barcode] = Barcode.value
FROM Production.dbo.TransferIn_Detail td
inner join Production.dbo.TransferIn t on td.id = t.id
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= td.PoId 
	and po3.SEQ1=td.Seq1 and po3.SEQ2=td.Seq2
outer apply(
	select value = min(fb.Barcode)
	from FtyInventory_Barcode fb 
	inner join FtyInventory f on f.Ukey = fb.Ukey
	where f.POID = td.POID
	and f.Seq1 = td.Seq1 and f.Seq2= td.Seq2
	and f.Roll = td.Roll and f.Dyelot = td.Dyelot
	and f.StockType = td.StockType
)Barcode
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = td.Poid and seq1=td.seq1 and seq2=td.seq2 
	and FabricType='F'
)
and t.id = '{this.CurrentMaintain["id"]}'
";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                Task.Run(() => new Gensong_AutoWHFabric().SentReceive_DetailToGensongAutoWHFabric(dtDetail))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <summary>
        ///  AutoWH Acc WebAPI for Vstrong
        /// </summary>
        private void SentToVstrong_AutoWH_ACC(bool isConfirmed)
        {
            DataTable dtDetail = new DataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                string sqlGetData = string.Empty;
                sqlGetData = $@"
SELECT 
 [ID] = rd.id
,[InvNo] = r.InvNo
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[StockUnit] = dbo.GetStockUnitBySPSeq(rd.POID,rd.Seq1,rd.Seq2)
,[StockQty] = rd.Qty
,[PoUnit] = ''
,[ShipQty] = 0.00
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[MtlType] = Fabric.MtlTypeID
,[Ukey] = rd.Ukey
,[ETA] = null
,[WhseArrival] = r.IssueDate
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
FROM Production.dbo.TransferIn_Detail rd
inner join Production.dbo.TransferIn r on rd.id = r.id
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
	and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
left join Production.dbo.FtyInventory f on f.POID = rd.PoId
	and f.Seq1=rd.Seq1 and f.Seq2=rd.Seq2 
	and f.Dyelot = rd.Dyelot and f.Roll = rd.Roll
	and f.StockType = rd.StockType
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='A'
)
and r.id = '{this.CurrentMaintain["id"]}'
";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                Task.Run(() => new Vstrong_AutoWHAccessory().SentReceive_Detail_New(dtDetail, "P18"))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
        , TtlQty = convert(varchar(20),
			iif(a.CombineBarcode is null , a.Qty, 
				iif(a.Unoriginal is null , ttlQty.value, null))) +' '+ dbo.GetStockUnitBySPSeq (a.poid, a.seq1, a.seq2)
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
        , p.Refno
		, [ColorID] = Color.Value
        ,[Barcode] = isnull(Barcode.value,'')
        ,a.CombineBarcode
        ,a.Unoriginal 
        ,[ActualWeight] = isnull(a.ActualWeight, 0)
        ,a.MDivisionID
from dbo.TransferIn_Detail a WITH (NOLOCK) 
left join Po_Supp_Detail p WITH (NOLOCK)  on a.poid = p.id
                              and a.seq1 = p.seq1
                              and a.seq2 = p.seq2
LEFT JOIN Fabric f WITH (NOLOCK) ON p.SCIRefNo=f.SCIRefNo
outer apply ( 
    select top 1 FabricType 
    from Invtrans I WITH (NOLOCK)  
    where a.poid = I.InventoryPOID and a.seq1 = I.InventorySeq1 and a.seq2 = I.InventorySeq2 
    and I.FactoryID = '{1}' and I.type = '3' 
) I
outer apply(
	select value = sum(Qty)
	from TransferIn_Detail t WITH (NOLOCK) 
	where t.ID=a.ID
	and t.CombineBarcode=a.CombineBarcode
	and t.CombineBarcode is not null
)ttlQty
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN p.SuppColor
		 ELSE dbo.GetColorMultipleID(p.BrandID,p.ColorID)
	 END
)Color
outer apply(
	select value = ft.barcode
	from FtyInventory ft
	where ft.POID = a.PoId
	and ft.Seq1 = a.Seq1 and ft.Seq2 = a.Seq2
	and ft.StockType = a.StockType 
	and ft.Roll =a.Roll and ft.Dyelot = a.Dyelot
)Barcode
Where a.id = '{0}'
order by a.CombineBarcode,a.Unoriginal,a.POID,a.Seq1,a.Seq2
", masterID, fromFty);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Accumulated Form
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P18_AccumulatedQty(this.CurrentMaintain)
            {
                P18 = this,
            };
            frm.ShowDialog(this);
        }

        // Find
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
            var frm = new P18_ExcelImport(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
            this.Change_record();
        }

        private void TxtFromFactory_Validating(object sender, CancelEventArgs e)
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

        private void TxtFromFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

        private bool ChkFtyInventory_Exists()
        {
            // 判斷是否已經收過此種布料SP#,SEQ,Roll不能重複收
            List<string> listMsg = new List<string>();
            List<string> listDyelot = new List<string>();
            foreach (DataRow row in this.DetailDatas)
            {
                string poid = MyUtility.Convert.GetString(row["poid"]);
                string seq1 = MyUtility.Convert.GetString(row["seq1"]);
                string seq2 = MyUtility.Convert.GetString(row["seq2"]);
                string roll = MyUtility.Convert.GetString(row["roll"]);
                string dyelot = MyUtility.Convert.GetString(row["dyelot"]);
                string fabricType = MyUtility.Convert.GetString(row["fabrictype"]);
                string stockType = MyUtility.Convert.GetString(row["stockType"]);

                // 判斷 物料 是否為 布，布料才需要 Roll &Dyelot
                if (fabricType.ToUpper() == "F")
                {
                    // 判斷 在 FtyInventory 是否存在
                    bool chkFtyInventory = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                    if (!chkFtyInventory)
                    {
                        listMsg.Add($"The Roll & Dyelot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");
                    }
                }
            }

            if (listMsg.Count > 0)
            {
                DialogResult dr = MyUtility.Msg.WarningBox(listMsg.JoinToString(string.Empty).TrimStart());
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

            // GridView button顯示+
            DataGridViewButtonCell next_dgbtn = (DataGridViewButtonCell)this.detailgrid.CurrentRow.Cells["btnAdd2"];
            next_dgbtn.Value = "+";
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);

            if (this.EditMode == false || this.DetailDatas == null || this.DetailDatas.Count <= 0)
            {
                return;
            }

            this.CurrentDetailData["MDivisionID"] = Env.User.Keyword;
            this.CurrentDetailData["Stocktype"] = 'B';

            // 新增後確認前一筆有資料才做下個動作
            DataRow pre_row = this.detailgrid.GetDataRow(this.detailgridbs.Position + 1);
            if (pre_row != null)
            {
                DataGridViewButtonCell pre_dgbtn = (DataGridViewButtonCell)this.detailgrid.Rows[this.detailgridbs.Position + 1].Cells["btnAdd2"];
                DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;
                if (dtDetail == null || dtDetail.Rows.Count <= 0)
                {
                    return;
                }

                string maxCombBarcode = dtDetail.Compute("Max(CombineBarcode)", string.Empty).ToString();

                if (pre_dgbtn.Value.ToString() == "-")
                {
                    // 取得CombineBarcode
                    string pre_ComBarcode = pre_row["CombineBarcode"].ToString();
                    if (MyUtility.Check.Empty(maxCombBarcode))
                    {
                        pre_ComBarcode = "1";
                    }
                    else
                    {
                        if (MyUtility.Check.Empty(pre_ComBarcode))
                        {
                            // New Max Value
                            pre_ComBarcode = Prgs.GetNextValue(maxCombBarcode, 1);
                        }
                    }

                    pre_row["CombineBarcode"] = pre_ComBarcode;
                    pre_row.EndEdit();
                    DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.CurrentRow.Cells["btnAdd2"].RowIndex);
                    newrow["Dyelot"] = pre_row["Dyelot"];
                    newrow["Roll"] = pre_row["Roll"];
                    newrow["Unoriginal"] = 1;
                    newrow["MDivisionID"] = Env.User.Keyword;
                    newrow["Stocktype"] = 'B';
                    newrow["CombineBarcode"] = pre_ComBarcode;
                    DataGridViewButtonCell next_dgbtn = (DataGridViewButtonCell)this.detailgrid.CurrentRow.Cells["btnAdd2"];
                    next_dgbtn.Value = "-";
                }
            }

            this.Change_record();
        }

        private void BtnModifyRollDyelot_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.InfoBox("Please modify data directly!!");
                return;
            }

            // 此功能只需顯示FabricType=F 資料,不須顯示副料
            DataTable dt;
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, @"select * from #tmp where fabrictype='F'", out dt)))
            {
                this.ShowErr(result);
                return;
            }

            var frm = new P07_ModifyRollDyelot(dt, this.CurrentMaintain["id"].ToString(), this.GridAlias);
            frm.ShowDialog(this);
            this.RenewData();
            this.Change_record();
        }

        private void BtnUpdateWeight_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.InfoBox("Please modify data directly!!");
                return;
            }

            var frm = new P07_UpdateWeight(this.detailgridbs.DataSource, this.CurrentMaintain["id"].ToString(), this.GridAlias);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // 呼叫P99
        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P18", this);
        }
    }
}