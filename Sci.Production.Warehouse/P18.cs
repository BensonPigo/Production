using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P18 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private ReportViewer viewer;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Roll;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Dyelot;

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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
        }

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

        /// <inheritdoc/>
        protected override DualResult ClickDeletePre()
        {
            string sqlGetFIR_Physical = $@"
                            SELECT  fp.DetailUkey
                            FROM TransferIn_Detail r with (nolock)
                            INNER JOIN PO_Supp_Detail p with (nolock) ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2
                            INNER JOIN FIR f with (nolock) on f.ReceivingID = r.ID AND f.POID = r.PoId AND f.SEQ1 = r.Seq1 AND f.SEQ2 = r.Seq2
                            inner join FIR_Physical fp with (nolock) on fp.ID = f.ID and fp.Roll = r.Roll AND fp.Dyelot = r.Dyelot
                            WHERE r.ID = '{this.CurrentMaintain["ID"]}' AND p.FabricType='F'";

            DataTable dtFIR_Physical;
            DualResult result = DBProxy.Current.Select("Production", sqlGetFIR_Physical, out dtFIR_Physical);

            if (!result)
            {
                return result;
            }

            foreach (DataRow drFIR_Physical in dtFIR_Physical.Rows)
            {
                result = DBProxy.Current.Execute("Production", $"exec dbo.MovePhysicalInspectionToHistory '{drFIR_Physical["DetailUkey"]}', 0, null");
                if (!result)
                {
                    return result;
                }
            }

            return base.ClickDeletePre();
        }

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

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            // 329: WAREHOUSE_P18 Print，資料如果未confirm不能列印。
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print !!", "Warning");
                return false;
            }

            P18_Print from = new P18_Print(this.CurrentMaintain);
            from.ShowDialog();

            return true;
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePre()
        {
            DualResult result = null;

            try
            {
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        string roll = dr["Roll", DataRowVersion.Original].ToString();
                        string dyelot = dr["Dyelot", DataRowVersion.Original].ToString();
                        long transferIn_DetailUkey = MyUtility.Convert.GetLong(dr["Ukey", DataRowVersion.Original]);

                        // Ukey = 0 表示沒有存進資料庫過，直接做下一筆
                        if (MyUtility.Check.Empty(transferIn_DetailUkey))
                        {
                            continue;
                        }

                        // 判斷FabricType = F，才能刪除FIR_Shadebone
                        string sqlCmd = $@"
                                            SELECT  f.ID
                                            FROM TransferIn_Detail r with (nolock)
                                            INNER JOIN PO_Supp_Detail p with (nolock) ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2
                                            INNER JOIN FIR f with (nolock) on f.ReceivingID = r.ID AND f.POID = r.PoId AND f.SEQ1 = r.Seq1 AND f.SEQ2 = r.Seq2 
                                            WHERE r.Ukey = '{transferIn_DetailUkey}' and  p.FabricType='F'
                                            ";

                        string fIR_ID = MyUtility.GetValue.Lookup(sqlCmd, "Production");

                        if (MyUtility.Check.Empty(fIR_ID))
                        {
                            continue;
                        }

                        result = DBProxy.Current.Execute(null, $"DELETE FROM FIR_Shadebone WHERE ID ={fIR_ID} AND Roll='{roll}' AND Dyelot='{dyelot}'");

                        if (!result)
                        {
                            return result;
                        }

                        // 刪除FIR_Physical並記錄FIR_Physical_His
                        string sqlGetFIR_Physical = $@"
                                            SELECT  DetailUkey
                                            FROM FIR_Physical with (nolock)
                                            WHERE ID = '{fIR_ID}' AND Roll='{roll}' AND Dyelot='{dyelot}'
                                            ";
                        DataTable dtFIR_Physical;
                        result = DBProxy.Current.Select("Production", sqlGetFIR_Physical, out dtFIR_Physical);

                        if (!result)
                        {
                            return result;
                        }

                        foreach (DataRow drFIR_Physical in dtFIR_Physical.Rows)
                        {
                            result = DBProxy.Current.Execute("Production", $"exec dbo.MovePhysicalInspectionToHistory '{drFIR_Physical["DetailUkey"]}', 0, null");
                            if (!result)
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

            return base.ClickSavePre();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查
            if (MyUtility.Check.Empty(this.txtFromFactory.Text))
            {
                MyUtility.Msg.WarningBox("From Factory cannot be null! ");
                this.txtFromFactory.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["TransferExportID"]))
            {
                string sqlCheckTransferExport = @"
select 1 from TransferExport with (nolock)
where ID = @ID and Confirm != 1
";
                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@ID", this.CurrentMaintain["TransferExportID"]) };
                bool isTransferExportNotConfirmed = MyUtility.Check.Seek(sqlCheckTransferExport, listPar);
                if (isTransferExportNotConfirmed)
                {
                    MyUtility.Msg.WarningBox("TPE status is not 'Confirm', cannot create transfer in record.");
                    return false;
                }
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append($@"SP#: {row["poid"]} Seq#: {row["seq1"]}-{row["seq2"]} can't be empty" + Environment.NewLine);
                }

                // 檢查Stock Type是否為空、且資料是否正確(B、I)
                if (row["Stocktype"].ToString() != "B" && row["Stocktype"].ToString() != "I")
                {
                    MyUtility.Msg.WarningBox("Detail <Stock Type> can only be [Bulk] or [Inventory]");
                    return false;
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append($@"SP#: {row["poid"]} Seq#: {row["seq1"]}-{row["seq2"]} Roll#:{row["roll"]} Dyelot:{row["dyelot"]} Transfer In Qty can't be empty" + Environment.NewLine);
                }

                if (row["FabricType"].ToString() == "F" && MyUtility.Check.Empty(row["dyelot"]))
                {
                    MyUtility.Msg.WarningBox("Detail <Dyelot> [Fabric Type] is Fabric,can't be empty");
                    return false;
                }

                if (row["FabricType"].ToString() == "F" && MyUtility.Check.Empty(row["roll"]))
                {
                    MyUtility.Msg.WarningBox("Detail <Roll> [Fabric Type] is Fabric,can't be empty");
                    return false;
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
                    if (MyUtility.Check.Seek($@"select * from TransferIn_Detail where poid = '{row["poid"]}' and seq1 = '{row["seq1"]}' and seq2 = '{row["seq2"]}' and Roll = '{row["Roll"]}' and Dyelot = '{row["Dyelot"]}' and stocktype = '{row["stocktype"]}'"))
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
                    if (MyUtility.Check.Seek($@"select * from TransferIn_Detail where poid = '{row["poid"]}' and seq1 = '{row["seq1"]}' and seq2 = '{row["seq2"]}' and Roll = '{row["Roll"]}' and Dyelot = '{row["Dyelot"]}' and stocktype = '{row["stocktype"]}'"))
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "TI", "TransferIn", DateTime.Now);
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

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }

            this.Change_record();
        }

        /// <inheritdoc/>
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
                        sqlcmd = $@"
select  poid = psd.ID 
        , seq = concat(Ltrim(Rtrim(psd.seq1)), ' ', psd.seq2)
        , psd.seq1
        , psd.seq2
        , psd.Refno
        , ColorID = isnull(psdsC.SpecValue, '')
        , Description = (select f.DescDetail from fabric f WITH (NOLOCK) where f.SCIRefno = psd.scirefno) 
        , psd.scirefno
        , psd.FabricType
        , stockunit = psd.StockUnit
from dbo.Po_Supp_Detail psd WITH (NOLOCK) 
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
where psd.ID ='{this.CurrentDetailData["poid"]}'
";
                    }
                    else
                    {
                        sqlcmd = $@"
select  poid = I.InventoryPOID 
        , seq = concat(Ltrim(Rtrim(I.InventorySeq1)), ' ', I.InventorySeq2)
        , seq1 = I.InventorySeq1
        , seq2 = I.InventorySeq2
        , I.Refno
        , ColorID=''
        , Description = ''
        , I.FabricType
        , psd.StockUnit
from dbo.Invtrans I WITH (NOLOCK) 
left join Po_Supp_Detail psd WITH (NOLOCK) on I.InventoryPOID = psd.id and I.InventorySeq1 = psd.seq1 and I.InventorySeq2 = psd.seq2
where I.InventoryPOID ='{this.CurrentDetailData["poid"]}'
and I.type = '3'
and I.FactoryID = '{this.CurrentMaintain["FromFtyID"]}'
";
                    }

                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt, "Seq,refno,description", "6,8,20", this.CurrentDetailData["seq"].ToString(), "Seq,Ref#,Description")
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
                    string seq = MyUtility.Convert.GetString(this.CurrentDetailData["seq"]);
                    if (!MyUtility.Check.Empty(seq))
                    {
                        DualResult result = P18_Utility.CheckDetailSeq(seq, this.CurrentMaintain["FromFtyID"].ToString(), this.CurrentDetailData);
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox(result.Description);
                            this.CurrentDetailData["seq"] = string.Empty;
                            this.CurrentDetailData["seq1"] = string.Empty;
                            this.CurrentDetailData["seq2"] = string.Empty;
                            this.CurrentDetailData["stockunit"] = string.Empty;
                            this.CurrentDetailData["Description"] = string.Empty;
                            this.CurrentDetailData["fabrictype"] = string.Empty;
                            return;
                        }
                    }

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
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;

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
            .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grop", width: Widths.AnsiChars(13), iseditingreadonly: false)
            .ComboBox("Stocktype", header: "Stock Type", width: Widths.AnsiChars(8), settings: sk).Get(out cbb_stocktype) // 8
            .Text("Location", header: "Location", iseditingreadonly: false, settings: ts2) // 9
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            .Text("Remark", header: "Remark", iseditingreadonly: false) // 10
            .Text("RefNo", header: "Ref#", iseditingreadonly: true)
            .Text("ColorID", header: "Color", iseditingreadonly: true)
            .Text("MINDQRCode", header: "MIND QR Code", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("MINDChecker", header: "Checker", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .DateTime("CheckDate", header: "Check Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
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

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("Arrive WH Date cannot be empty");
                this.dateIssueDate.Focus();
                return;
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;
            DataTable datacheck;

            // Check Roll 是否有重複
            if (!this.ChkFtyInventory_Exists())
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region -- 檢查庫存項lock --
            string sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        ,f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   f.lock=1 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
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
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        ,f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0) + d.Qty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than In qty: {tmp["qty"]}" + Environment.NewLine;
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
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            #endregion

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

            string upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true, mtlAutoLock);
            #endregion 更新庫存數量  ftyinventory

            #region -- ftyinventory Tone --

            var data_Fty_Tone = (from b in this.DetailDatas
                               select new
                               {
                                   poid = b.Field<string>("poid"),
                                   seq1 = b.Field<string>("seq1"),
                                   seq2 = b.Field<string>("seq2"),
                                   stocktype = b.Field<string>("stocktype"),
                                   FabricType = b.Field<string>("FabricType"),
                                   Tone = b.Field<string>("Tone"),
                                   roll = b.Field<string>("roll"),
                                   dyelot = b.Field<string>("dyelot"),
                               }).ToList();

            string upd_Fty_Tone = Prgs.UpdateFtyInventory_IO(66, null, true, mtlAutoLock);
            #endregion ftyinventory Tone

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

            #region 檢查MINDQRCode是否有在其他單子重複，有重複就update成空白, where 拆開來是因為效能(有index但有時候無效)
            string sqlCheckMINDQRCode = $@"
update rd set rd.MINDQRCode = ''
from TransferIn_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from TransferIn_Detail rd2 with (nolock) where rd2.ID <> rd.ID and rd2.MINDQRCode = rd.MINDQRCode)

update rd set rd.MINDQRCode = ''
from TransferIn_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where wht.Action = 'Confirm' and [Function] = 'P18' and wht.TransactionID <> rd.ID and wht.To_NewBarcode = rd.MINDQRCode)

update rd set rd.MINDQRCode = ''
from TransferIn_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P18' and wht.From_OldBarcode = rd.MINDQRCode)

update rd set rd.MINDQRCode = ''
from TransferIn_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P18' and wht.From_NewBarcode = rd.MINDQRCode)

update rd set rd.MINDQRCode = ''
from TransferIn_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P18' and wht.To_OldBarcode = rd.MINDQRCode)

update rd set rd.MINDQRCode = ''
from TransferIn_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P18' and wht.To_NewBarcode = rd.MINDQRCode)
";
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        #region 更新FIR,AIR資料 & 寫入PMSFile 在 insert_Air_Fir 內

                        List<SqlParameter> fir_Air_Proce = new List<SqlParameter>
            {
                new SqlParameter("@ID", this.CurrentMaintain["ID"]),
                new SqlParameter("@LoginID", Env.User.UserID),
            };
                        if (!(result = DBProxy.Current.SelectByConn(sqlConn, " exec dbo.insert_Air_Fir_TnsfIn @ID,@LoginID", fir_Air_Proce, out DataTable[] airfirids)))
                        {
                            this.ShowErr(result);
                            return;
                        }

                        if (airfirids[0].Rows.Count > 0 || airfirids[1].Rows.Count > 0)
                        {
                            // 寫入PMSFile
                            string cmd = @"SET XACT_ABORT ON
";
                            var firinsertlist = airfirids[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["id"]));
                            if (firinsertlist.Any())
                            {
                                string firInsertIDs = firinsertlist.Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().JoinToString(",");
                                cmd += $@"
INSERT INTO SciPMSFile_FIR_Laboratory (ID)
select ID from FIR_Laboratory t WITH(NOLOCK) where id in ({firInsertIDs})
and not exists (select 1 from SciPMSFile_FIR_Laboratory s (NOLOCK) where s.ID = t.ID )
";
                            }

                            var firDeletelist = airfirids[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["deID"]));
                            if (firDeletelist.Any())
                            {
                                string firDeleteIDs = firDeletelist.Select(s => MyUtility.Convert.GetString(s["deID"])).Distinct().JoinToString(",");
                                cmd += $@"
Delete SciPMSFile_FIR_Laboratory where id in ({firDeleteIDs})
and ID NOT IN(select ID from FIR_Laboratory)";
                            }

                            var airinsertlist = airfirids[1].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["id"]));
                            if (airinsertlist.Any())
                            {
                                string airInsertIDs = airinsertlist.Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().JoinToString(",");
                                cmd += $@"
INSERT INTO SciPMSFile_AIR_Laboratory (ID,POID,SEQ1,SEQ2)
select  ID,POID,SEQ1,SEQ2 from AIR_Laboratory t WITH(NOLOCK) where id in ({airInsertIDs})
and not exists (select 1 from SciPMSFile_AIR_Laboratory s WITH(NOLOCK) where s.ID = t.ID AND s.POID = t.POID AND s.SEQ1 = t.SEQ1 AND s.SEQ2 = t.SEQ2 )
";
                            }

                            var airDeletelist = airfirids[1].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["deID"]));
                            if (airDeletelist.Any())
                            {
                                string airDeleteIDs = airDeletelist.Select(s => MyUtility.Convert.GetString(s["deID"])).Distinct().JoinToString(",");
                                cmd += $@"
Delete a 
from SciPMSFile_AIR_Laboratory a
where id in ({airDeleteIDs})
and NOT EXISTS(select 1 from AIR_Laboratory b    where a.ID = b.ID AND a.POID=b.POID AND a.Seq1=b.Seq1 AND a.Seq2=b.Seq2)
";
                            }

                            result = DBProxy.Current.ExecuteByConn(sqlConn, cmd);
                            if (!result)
                            {
                                this.ShowErr(result);
                                return;
                            }
                        }
                        #endregion

                        /*
                            * 先更新 FtyInventory 後更新 MDivisionPoDetail
                            * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                            * 因為要在同一 SqlConnection 之下執行
                            */
                        // FtyInventory 庫存
                        DataTable resulttb;
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // FtyInventory Tone一定要在更新庫存後面執行
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Tone, string.Empty, upd_Fty_Tone, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // 檢查MINDQRCode是否有在其他單子重複，有的畫清空，UpdateWH_Barcode會重編新的
                        result = DBProxy.Current.ExecuteByConn(sqlConn, sqlCheckMINDQRCode);
                        if (!result)
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        #region MDivisionPoDetail
                        if (data_MD_2T.Count > 0)
                        {
                            string upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);
                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                            {
                                throw result.GetException();
                            }
                        }

                        if (data_MD_8T.Count > 0)
                        {
                            string upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);
                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                            {
                                throw result.GetException();
                            }
                        }
                        #endregion

                        if (!(result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, sql_UpdatePO_Supp_Detail, out resulttb, "#tmp", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // 更新表頭
                        if (MyUtility.Check.Seek($"select 1 from TransferIn where ID = '{this.CurrentMaintain["ID"]}' and Status = 'Confirmed'"))
                        {
                            throw result.GetException();
                        }

                        if (!(result = DBProxy.Current.Execute(null, $"update TransferIn set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        string issueDate = ((DateTime)this.CurrentMaintain["IssueDate"]).ToString("yyyy/MM/dd");
                        sqlcmd = $@"update FtyExport set WhseArrival = '{issueDate}' where INVNo = '{this.CurrentMaintain["INVNo"]}'";
                        if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                        {
                            throw result.GetException();
                        }

                        if (!MyUtility.Check.Empty(this.CurrentMaintain["TransferExportID"]))
                        {
                            string sqlUpdateTransferExport = @" update TransferExport set WhseArrival = @WhseArrival, PackingArrival = @PackingArrival where ID = @TransferExportID";
                            List<SqlParameter> listParTransferExport = new List<SqlParameter>()
                        {
                            new SqlParameter("@WhseArrival", this.CurrentMaintain["IssueDate"]),
                            new SqlParameter("@PackingArrival", this.CurrentMaintain["PackingArrival"]),
                            new SqlParameter("@TransferExportID", this.CurrentMaintain["TransferExportID"]),
                        };

                            result = DBProxy.Current.Execute(null, sqlUpdateTransferExport, listParTransferExport);
                            if (!result)
                            {
                                transactionscope.Dispose();
                                this.ShowErr(result);
                                return;
                            }
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            if (!Prgs.CheckShadebandResult(this.Name, this.CurrentMaintain["ID"].ToString()))
            {
                return;
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            DataTable datacheck;
            string ids = string.Empty;

            #region -- 檢查庫存項lock --
            string sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        ,f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   f.lock=1 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "TransferIn_Detail"))
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
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        ,f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0) - d.Qty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]}  Dyelot#: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than In qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

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
            #endregion 更新庫存數量  ftyinventory

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        string deleteFIR_Shadebone = $@"
delete fs
from TransferIn_Detail sd with(nolock)
inner join PO_Supp_Detail psd with(nolock) on psd.ID = sd.PoId and psd.SEQ1 = sd.Seq1 and psd.SEQ2 = sd.Seq2
inner join FIR f with (nolock) on sd.id = f.ReceivingID and sd.PoId = F.POID and sd.Seq1 = F.SEQ1 and sd.Seq2 = F.SEQ2
inner join FIR_Shadebone fs with (nolock) on f.id = fs.ID
where sd.id = '{this.CurrentMaintain["ID"]}'
";
                        if (!(result = DBProxy.Current.Execute(null, deleteFIR_Shadebone)))
                        {
                            throw result.GetException();
                        }

                        /*
                         * 先更新 FtyInventory 後更新 MDivisionPoDetail
                         * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                         * 因為要在同一 SqlConnection 之下執行
                         */

                        // FtyInventory 庫存
                        DataTable resulttb;
                        string upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        #region MDivisionPoDetail
                        if (data_MD_2F.Count > 0)
                        {
                            string upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);
                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                            {
                                throw result.GetException();
                            }
                        }

                        if (data_MD_8F.Count > 0)
                        {
                            string upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn);
                            if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F, out resulttb, "#TmpSource", conn: sqlConn)))
                            {
                                throw result.GetException();
                            }
                        }
                        #endregion

                        if (MyUtility.Check.Seek($"select 1 from TransferIn where ID = '{this.CurrentMaintain["ID"]}' and Status = 'New'"))
                        {
                            throw result.GetException();
                        }

                        if (!(result = DBProxy.Current.Execute(null, $@"update TransferIn set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        if (!MyUtility.Check.Empty(this.CurrentMaintain["TransferExportID"]))
                        {
                            string sqlUpdateTransferExport = @" update TransferExport set WhseArrival = null, PackingArrival = null where ID = @TransferExportID";
                            List<SqlParameter> listParTransferExport = new List<SqlParameter>() { new SqlParameter("@TransferExportID", this.CurrentMaintain["TransferExportID"]) };
                            if (!(result = DBProxy.Current.Execute(null, sqlUpdateTransferExport, listParTransferExport)))
                            {
                                throw result.GetException();
                            }
                        }

                        // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                        Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string fromFty = (e.Master == null) ? string.Empty : e.Master["FromFtyID"].ToString();
            this.DetailSelectCommand = $@"
select  a.id
    , a.PoId
    , a.Seq1
    , a.Seq2
    , seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
    , a.Roll
    , a.Dyelot
    , [Description] = dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0)
    , StockUnit = psd.StockUnit
    , a.Qty
    , TtlQty = convert(varchar(20),
		iif(a.CombineBarcode is null , a.Qty, 
			iif(a.Unoriginal is null , ttlQty.value, null))) +' '+ psd.StockUnit
    , a.StockType
    , a.location
    , a.ContainerCode
    , a.ukey
    , FabricType = isnull(psd.FabricType,I.FabricType)
    , DataFrom = iif(psd.FabricType is null,'Invtrans','Po_Supp_Detail')
	,a.Weight
	,a.Remark
    ,[Fabric] = case when psd.FabricType = 'F' then 'Fabric' 
                            when psd.FabricType = 'A' then 'Accessory'
                    else '' end
    , psd.Refno
	, [ColorID] = Color.Value
    ,[Barcode] = isnull(Barcode.value,'')
    ,a.CombineBarcode
    ,a.Unoriginal 
    ,[ActualWeight] = isnull(a.ActualWeight, 0)
    ,a.MDivisionID
    ,a.CompleteTime
    ,a.SentToWMS
    ,[Tone] = a.Tone
    ,[MINDQRCode] = case when b.Status = 'New' then a.MINDQRCode
                            when b.Status = 'Confirmed' and a.MINDQRCode <> '' then a.MINDQRCode
                            else ( select top 1 case  when    wbt.To_NewBarcodeSeq = '' then wbt.To_NewBarcode
                                                    when    wbt.To_NewBarcode = ''  then ''
                                                    else    Concat(wbt.To_NewBarcode, '-', wbt.To_NewBarcodeSeq)    end
                                from   WHBarcodeTransaction wbt with (nolock)
                                where  wbt.TransactionUkey = a.Ukey and
                                        wbt.Action = 'Confirm'
                                order by wbt.CommitTime desc) end
    ,[MINDChecker] = a.MINDChecker+'-'+(select name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = a.MINDChecker)
    ,[CheckDate] = IIF(a.MINDCheckEditDate IS NULL, a.MINDCheckAddDate,a.MINDCheckEditDate)
from dbo.TransferIn_Detail a WITH (NOLOCK) 
inner join TransferIn b WITH (NOLOCK) on a.id = b.id
left join Po_Supp_Detail psd WITH (NOLOCK)  on a.poid = psd.id and a.seq1 = psd.seq1 and a.seq2 = psd.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
LEFT JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo=f.SCIRefNo
outer apply ( 
    select top 1 FabricType 
    from Invtrans I WITH (NOLOCK)  
    where a.poid = I.InventoryPOID and a.seq1 = I.InventorySeq1 and a.seq2 = I.InventorySeq2 
    and I.FactoryID = '{fromFty}' and I.type = '3' 
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
	    CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF( isnull(psd.SuppColor,'') = '',dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, '')),psd.SuppColor)
		    ELSE dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, ''))
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
Where a.id = '{masterID}'
order by a.CombineBarcode,a.Unoriginal,a.POID,a.Seq1,a.Seq2
            ";
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P18_AccumulatedQty(this.CurrentMaintain)
            {
                P18 = this,
            };
            frm.ShowDialog(this);
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
            newrow["Tone"] = lastRow["Tone"];

            // GridView button顯示+
            DataGridViewButtonCell next_dgbtn = (DataGridViewButtonCell)this.detailgrid.CurrentRow.Cells["btnAdd2"];
            next_dgbtn.Value = "+";
        }

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

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
        }

        private void TxtTransferExportID_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtTransferExportID.Text == this.CurrentMaintain["TransferExportID"].ToString())
            {
                return;
            }

            if (MyUtility.Check.Empty(this.txtTransferExportID.Text))
            {
                ((DataTable)this.detailgridbs.DataSource).Rows.Clear();
                return;
            }

            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@ID", this.txtTransferExportID.Text) };
            string sqlGetTrasnferExport = $@"
        select  ted.POID,
            [Seq] = concat(Ltrim(Rtrim(ted.Seq1)), ' ', ted.Seq2),
            ted.Seq1,
            ted.Seq2,
            [Roll] = tdc.Roll,
            [Dyelot] = tdc.LotNo,
            [StockType] = 'B',
            [Qty] = isnull(dbo.GetUnitQty(tdc.StockUnitID, psd.StockUnit, sum(isnull(tdc.StockQty, 0))), 0),
            ted.FabricType,
            [Fabric] = case when ted.FabricType = 'F' then 'Fabric' 
                                 when ted.FabricType = 'A' then 'Accessory'
                            else '' end,
            [Description] = dbo.getMtlDesc(ted.poid, ted.seq1, ted.seq2,2,0),
            psd.StockUnit,
            psd.Refno,
            [ColorID] = Color.Value,
            [Tone] =tdc.Tone,
            [MINDQRCode] = tdc.MINDQRCode
        from    TransferExport te with (nolock)
        inner join TransferExport_Detail ted with (nolock) on ted.ID = te.ID 
        inner join TransferExport_Detail_Carton tdc with (nolock) on ted.Ukey = tdc.TransferExport_DetailUkey
        left join Po_Supp_Detail psd with (nolock) on psd.ID = ted.POID and psd.Seq1 = ted.Seq1 and psd.Seq2 = ted.Seq2
        left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
        left JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo = f.SCIRefNo
        OUTER APPLY(
         SELECT [Value]=
	         CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF( isnull(psd.SuppColor,'') = '',dbo.GetColorMultipleID(psd.BrandID, isnull(psdsC.SpecValue, '')),psd.SuppColor)
		         ELSE dbo.GetColorMultipleID(psd.BrandID, isnull(psdsC.SpecValue, ''))
	         END
        )Color
        where   te.ID = @ID and
		        te.Confirm  = 1 and
		        exists(select 1 from Factory f with (nolock) 
		                where f.ID = te.FactoryID and 
		                      f.IsproduceFty = 1 and 
		                      f.MDivisionID  = '{Env.User.Keyword}' ) and
		        not exists(select 1 from TransferIn tf with (nolock) where tf.TransferExportID = te.ID and tf.ID <> '{this.CurrentMaintain["ID"]}') and
                tdc.StockQty > 0
        group by ted.POID, ted.Seq1, ted.Seq2, tdc.Roll, tdc.LotNo, ted.FabricType, psd.StockUnit, psd.Refno, Color.Value, tdc.StockUnitID, psd.StockUnit,Tone, tdc.MINDQRCode
        ";
            DualResult result = DBProxy.Current.Select(null, sqlGetTrasnferExport, listPar, out DataTable dtTrasnferExport);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            ((DataTable)this.detailgridbs.DataSource).Rows.Clear();

            if (dtTrasnferExport.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Transfer WK# not found");
                this.CurrentMaintain["TransferExportID"] = string.Empty;
                return;
            }

            DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;

            foreach (DataRow drImport in dtTrasnferExport.Rows)
            {
                DataRow drNew = dtDetail.NewRow();

                drNew["POID"] = drImport["POID"];
                drNew["Seq"] = drImport["Seq"];
                drNew["Seq1"] = drImport["Seq1"];
                drNew["Seq2"] = drImport["Seq2"];
                drNew["Roll"] = drImport["Roll"];
                drNew["Dyelot"] = drImport["Dyelot"];
                drNew["StockType"] = drImport["StockType"];
                drNew["Qty"] = drImport["Qty"];
                drNew["FabricType"] = drImport["FabricType"];
                drNew["Fabric"] = drImport["Fabric"];
                drNew["Description"] = drImport["Description"];
                drNew["StockUnit"] = drImport["StockUnit"];
                drNew["Refno"] = drImport["Refno"];
                drNew["ColorID"] = drImport["ColorID"];
                drNew["Tone"] = drImport["Tone"];
                drNew["MINDQRCode"] = drImport["MINDQRCode"];

                dtDetail.Rows.Add(drNew);
            }

            this.Change_record();
        }
    }
}