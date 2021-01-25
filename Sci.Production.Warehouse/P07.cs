using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P07 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private Ict.Win.UI.DataGridViewNumericBoxColumn Col_ActualW;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Roll;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Dyelot;

        private string UserID = Env.User.UserID;
        private bool isSetZero = false;

        /// <inheritdoc/>
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='A'");
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.di_fabrictype.Add("O", "Other");
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
            this.detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    DataRow tmp = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
                    this.OnDetailGridInsert();
                    DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
                    newrow.ItemArray = tmp.ItemArray;
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[this.detailgrid.RowCount - 1].Cells[0];
                }
            };
            MyUtility.Tool.SetupCombox(this.comboTypeFilter, 2, 1, "ALL,ALL,Fabric,Fabric,Accessory,Accessory");
            this.comboTypeFilter.SelectedIndex = 0;
            this.comboStockType.DataSource = new BindingSource(this.di_stocktype, null);
            this.comboStockType.ValueMember = "Key";
            this.comboStockType.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        public P07(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.di_fabrictype.Add("O", "Other");
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
            this.comboStockType.DataSource = new BindingSource(this.di_stocktype, null);
            this.comboStockType.ValueMember = "Key";
            this.comboStockType.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Third"] = 1;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "A";
            foreach (DataGridViewColumn index in this.detailgrid.Columns)
            {
                index.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.radioEncodeSeq.Checked = true;
            this.radiobySP.Checked = false;
            this.radioPanel1.ReadOnly = true;
            this.Change_record();
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
        protected override bool ClickEditBefore()
        {
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            this.radioPanel1.ReadOnly = true;
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            // 將Type Filter 切換成All
            this.comboTypeFilter.SelectedIndex = 0;

            #region check Columns length
            List<string> listColumnLengthErrMsg = new List<string>();
            Dictionary<string, string> errormsgDir = new Dictionary<string, string>();
            string errorkey;
            foreach (DataRow row in this.DetailDatas)
            {
                // List<string> listRowErrMsg = new List<string>();
                errorkey = string.Format("<SP#> : {0}, <Seq> : {1}", row["Poid"], row["Seq"]);
                if (!errormsgDir.ContainsKey(errorkey))
                {
                    errormsgDir.Add(errorkey, string.Empty);
                }

                // Seq1 varchar(3)
                if (row["Seq1"].ToString().Length > 3)
                {
                    if (!errormsgDir[errorkey].Contains("<SEQ1> length can't be more than 3 Characters."))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<SEQ1> length can't be more than 3 Characters.";
                    }
                }

                // listRowErrMsg.Add("<SEQ1> length can't be more than 3 Characters.");

                // Seq2 varchar(2)
                if (row["Seq2"].ToString().Length > 2)
                {
                    if (!errormsgDir[errorkey].Contains("<SEQ2> length can't be more than 2 Characters."))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<SEQ2> length can't be more than 2 Characters.";
                    }
                }

                // listRowErrMsg.Add("<SEQ2> length can't be more than 2 Characters.");

                // Roll varchar(8)
                if (row["Roll"].ToString().Length > 8)
                {
                    if (!errormsgDir[errorkey].Contains("<Roll> length can't be more than 8 Characters."))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Roll> length can't be more than 8 Characters.";
                    }
                }

                // listRowErrMsg.Add("<Roll> length can't be more than 8 Characters.");

                // Dyelot varchar(8)
                byte[] dyelotTemp = Encoding.Default.GetBytes(row["Dyelot"].ToString());
                if (dyelotTemp.Length > 8)
                {
                    if (!errormsgDir[errorkey].Contains("<Dyelot> length can't be more than 8 Characters."))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Dyelot> length can't be more than 8 Characters.";
                    }
                }

                // listRowErrMsg.Add("<Dyelot> length can't be more than 4 Characters.");

                // ShipQty  numeric(11, 2)
                if (decimal.Parse(row["ShipQty"].ToString()) > 999999999)
                {
                    if (!errormsgDir[errorkey].Contains("<Ship Qty> value can't be more than 999,999,999"))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Ship Qty> value can't be more than 999,999,999";
                    }
                }

                // listRowErrMsg.Add("<Ship Qty> value can't be more than 999,999,999");

                // ActualQty  numeric (11, 2)
                if (decimal.Parse(row["ActualQty"].ToString()) > 999999999)
                {
                    if (!errormsgDir[errorkey].Contains("<Actual Qty> value can't be more than 999,999,999"))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Actual Qty> value can't be more than 999,999,999";
                    }
                }

                // listRowErrMsg.Add("<Actual Qty> value can't be more than 999,999,999");

                // actualWeight numeric (7, 2)
                if (decimal.Parse(row["actualWeight"].ToString()) > 99999)
                {
                    if (!errormsgDir[errorkey].Contains("<Act.(kg)> value can't be more than 99,999"))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Act.(kg)> value can't be more than 99,999";
                    }
                }

                // listRowErrMsg.Add("<Act.(kg)> value can't be more than 99,999");

                // Weight numeric (7, 2)
                if (decimal.Parse(row["Weight"].ToString()) > 99999)
                {
                    if (!errormsgDir[errorkey].Contains("<G.W(kg)> value can't be more than 99,999"))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<G.W(kg)> value can't be more than 99,999";
                    }
                }

                // listRowErrMsg.Add("<G.W(kg)> value can't be more than 99,999");

                // Location varchar(60)
                if (row["Location"].ToString().Length > 60)
                {
                    if (!errormsgDir[errorkey].Contains("<Location> length can't be more than 60 Characters."))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Location> length can't be more than 60 Characters.";
                    }
                }

                // listRowErrMsg.Add("<Location> length can't be more than 60 Characters.");

                // check 相同CombineBarcode, Refno, Color 是否一致
                if (!MyUtility.Check.Empty(row["CombineBarcode"]) &&
                        row["FabricType"].ToString() == "F")
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

            foreach (KeyValuePair<string, string> item in errormsgDir)
            {
                if (item.Value.Length > 0)
                {
                    listColumnLengthErrMsg.Add(item.Key + item.Value);
                }
            }

            if (listColumnLengthErrMsg.Count > 0)
            {
                MyUtility.Msg.WarningBox(listColumnLengthErrMsg.JoinToString(Environment.NewLine + Environment.NewLine));
                return false;
            }
            #endregion

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["invno"]))
            {
                MyUtility.Msg.WarningBox("< Invoice# >  can't be empty!", "Warning");
                this.txtInvoiceNo.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ETA"]))
            {
                MyUtility.Msg.WarningBox("< ETA >  can't be empty!", "Warning");
                this.dateETA.Focus();
                return false;
            }

            #endregion 必輸檢查

            #region 確認此單中，是否存在同【SP#, Seq1, Seq2, Roll】
            string strCheckDuplicateData = @"
select *
from (
    select  Poid
            , Seq1
            , Seq2
            , Roll
            , Dyelot
            , value = count(*)
    from #tmp
    group by Poid, Seq1, Seq2, Roll, Dyelot
) x
where x.value > 1
";
            DualResult resultCheck = MyUtility.Tool.ProcessWithDatatable((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource, null, strCheckDuplicateData, out DataTable dtCheckDuplicateData, "#tmp");
            if (resultCheck)
            {
                if (dtCheckDuplicateData != null && dtCheckDuplicateData.Rows.Count != 0)
                {
                    List<string> listDuplicateData = new List<string>();
                    foreach (DataRow dr in dtCheckDuplicateData.Rows)
                    {
                        listDuplicateData.Add($"<SP#> : {dr["Poid"]}, <Seq1> : {dr["Seq1"]}, <Seq2> : {dr["Seq2"]}, <Roll#> : {dr["Roll"]}, <Dyelot> : {dr["Dyelot"]}");
                    }

                    MyUtility.Msg.WarningBox("SP#, Seq1, Seq2, Roll#, Dyelot cannot be duplicate." + Environment.NewLine + listDuplicateData.JoinToString(Environment.NewLine));
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox(resultCheck.Description);
                return false;
            }
            #endregion

            #region 表身的資料存在Po_Supp_Detail中但是已被Junk，就要跳出訊息告知且不做任何動作
            string sqlchkPSDJunk = $@"
select distinct concat('SP#: ',p.id,', Seq#: ',Ltrim(Rtrim(p.seq1)), '-', p.seq2) as seq
from dbo.PO_Supp_Detail p WITH (NOLOCK) 
inner join #tmp t on p.id =t.poid and p.SEQ1 = t.SEQ1 and p.SEQ2 = t.SEQ2
where p.junk = 1
";

            DualResult dualResult = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, sqlchkPSDJunk, out DataTable junkdt);
            if (!dualResult)
            {
                this.ShowErr(dualResult);
                return false;
            }

            if (junkdt.Rows.Count > 0)
            {
                var v = junkdt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["seq"])).ToList();
                string msgjunk = @"Below item already junk can't be receive.
" + string.Join("\r\n", v);

                MyUtility.Msg.WarningBox(msgjunk);
                return false;
            }
            #endregion

            DateTime arrivePortDate;
            DateTime whseArrival;
            DateTime eTA;
            bool chk;
            string msg;

            if (!MyUtility.Check.Empty(this.dateArrivePortDate.Value) && !MyUtility.Check.Empty(this.CurrentMaintain["WhseArrival"]))
            {
                arrivePortDate = DateTime.Parse(this.dateArrivePortDate.Text); // port
                whseArrival = DateTime.Parse(this.CurrentMaintain["WhseArrival"].ToString()); // warehouse

                // 到倉日不可早於到港日
                if (!(chk = Prgs.CheckArrivedWhseDateWithArrivedPortDate(arrivePortDate, whseArrival, out msg)))
                {
                    MyUtility.Msg.WarningBox(msg);
                    this.dateArriveWHDate.Focus();
                    return false;
                }

                if (DateTime.Compare(whseArrival, arrivePortDate.AddDays(20)) > 0)
                {
                    MyUtility.Msg.WarningBox("Arrive Warehouse date can't be later than arrive port 20 days!!");
                    this.dateArriveWHDate.Focus();
                    return false;
                }
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["WhseArrival"]) && !MyUtility.Check.Empty(this.CurrentMaintain["eta"]))
            {
                eTA = DateTime.Parse(this.CurrentMaintain["eta"].ToString()); // eta
                whseArrival = DateTime.Parse(this.CurrentMaintain["WhseArrival"].ToString()); // warehouse+

                // 到倉日如果早於ETA 3天，則提示窗請USER再確認是否存檔。
                // 到倉日如果晚於ETA 15天，則提示窗請USER再確認是否存檔。
                if (!(chk = Prgs.CheckArrivedWhseDateWithEta(eTA, whseArrival, out msg)))
                {
                    DialogResult dResult = MyUtility.Msg.QuestionBox(msg);
                    if (dResult == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            int intEncodeSeq = 0;
            foreach (DataRow row in this.DetailDatas)
            {
                if ((MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"])) ||
                    row["seq1"].ToString().TrimStart().StartsWith("7") ||
                    MyUtility.Check.Empty(row["ActualQty"]) ||
                    MyUtility.Check.Empty(row["stocktype"]) ||
                    (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"]))))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} ", row["poid"], row["seq1"], row["seq2"]));

                    if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                    {
                        warningmsg.Append(string.Format(@"Seq1 or Seq2 can't be empty ,"));
                    }

                    if (row["seq1"].ToString().TrimStart().StartsWith("7"))
                    {
                        warningmsg.Append(string.Format(@"Seq1 can't start with '7' ,"));
                    }

                    if (MyUtility.Check.Empty(row["ActualQty"]))
                    {
                        warningmsg.Append(string.Format(@"Actual Qty can't be empty ,"));
                    }

                    if (MyUtility.Check.Empty(row["stocktype"]))
                    {
                        warningmsg.Append(string.Format(@"Stock Type can't be empty ,"));
                    }

                    if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                    {
                        warningmsg.Append(string.Format(@"Roll and Dyelot can't be empty ,"));
                    }

                    warningmsg.Append(Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["roll"] = string.Empty;
                    row["dyelot"] = string.Empty;
                }

                // 依照當前排序按順序塞EncodeSeq
                row["EncodeSeq"] = intEncodeSeq;
                intEncodeSeq++;
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            // 收物料時, 要判斷除了自己之外, 是否已存在同SP+Seq+ROLL+Dyelot(Fabric=F, StockType相同),P07 [Receiving_Detail]
            warningmsg.Clear();
            foreach (DataRow row in this.DetailDatas)
            {
                if (row["fabrictype"].ToString().ToUpper() == "F")
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        string sqlcmd = $@"select * from Receiving_Detail where poid = '{row["poid"]}' and seq1 = '{row["seq1"]}' and seq2 = '{row["seq2"]}' and Roll = '{row["Roll"]}' and Dyelot = '{row["Dyelot"]}' and stocktype = '{row["stocktype"]}'";
                        if (MyUtility.Check.Seek(sqlcmd))
                        {
                            warningmsg.Append(string.Format(@"<SP>: {0} <Seq>: {1}-{2}  <ROLL> {3}<DYELOT>{4} exists, cannot be saved!", row["poid"], row["seq1"], row["seq2"], row["Roll"], row["Dyelot"]));
                            warningmsg.Append(Environment.NewLine);
                        }
                    }

                    if (row.RowState == DataRowState.Modified)
                    {
                        if (MyUtility.Convert.GetString(row["poid"]) != MyUtility.Convert.GetString(row["poid", DataRowVersion.Original]) ||
                            MyUtility.Convert.GetString(row["seq1"]) != MyUtility.Convert.GetString(row["seq1", DataRowVersion.Original]) ||
                            MyUtility.Convert.GetString(row["seq2"]) != MyUtility.Convert.GetString(row["seq2", DataRowVersion.Original]) ||
                            MyUtility.Convert.GetString(row["Roll"]) != MyUtility.Convert.GetString(row["Roll", DataRowVersion.Original]) ||
                            MyUtility.Convert.GetString(row["Dyelot"]) != MyUtility.Convert.GetString(row["Dyelot", DataRowVersion.Original]) ||
                            MyUtility.Convert.GetString(row["stocktype"]) != MyUtility.Convert.GetString(row["stocktype", DataRowVersion.Original]))
                        {
                            if (MyUtility.Check.Seek($@"select * from Receiving_Detail where poid = '{row["poid"]}' and seq1 = '{row["seq1"]}' and seq2 = '{row["seq2"]}' and Roll = '{row["Roll"]}' and Dyelot = '{row["Dyelot"]}' and stocktype = '{row["stocktype"]}'"))
                            {
                                warningmsg.Append($@"<SP>: {row["poid"]} <Seq>: {row["seq1"]}-{row["seq2"]}  <ROLL> {row["Roll"]}<DYELOT>{row["Dyelot"]} exists, cannot be saved!");
                                warningmsg.Append(Environment.NewLine);
                            }
                        }
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

            if (!MyUtility.Check.Empty(this.DetailDatas) && this.DetailDatas.Count > 0)
            {
                string cmdd = @"
select  #tmp.*
from #tmp,dbo.po_supp WITH (NOLOCK) ,dbo.supp WITH (NOLOCK) 
where   #tmp.poid = dbo.po_supp.id
        and #tmp.seq1 = dbo.po_supp.seq1
        and dbo.po_supp.suppid = dbo.supp.id
        and dbo.supp.thirdcountry = 1 ";
                MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), "poid,seq1", cmdd, out result, "#tmp");

                if (!MyUtility.Check.Empty(result) && result.Rows.Count > 0)
                {
                    this.CurrentMaintain["third"] = 1;
                }
                else
                {
                    this.CurrentMaintain["third"] = 0;
                }
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "PR", "Receiving", (DateTime)this.CurrentMaintain["ETA"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            foreach (DataGridViewColumn index in this.detailgrid.Columns)
            {
                index.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            this.comboTypeFilter.SelectedIndex = 0;
            this.detailgridbs.Filter = string.Empty;
            base.ClickSaveAfter();

            // 存檔結束後sort by 要回到SP#排序
            this.radiobySP.Checked = true;
            this.GridSortBy(false);
            this.Change_record();
            this.radioPanel1.ReadOnly = false;
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

            if (!(this.CurrentMaintain == null))
            {
                this.dateArrivePortDate.Value = null;
                this.dateDoxRcvDate.Value = null;
                string sqlcmd = $@"select portarrival, docarrival from dbo.export WITH (NOLOCK) where id='{this.CurrentMaintain["exportid"]}'";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow dr, null))
                {
                    if (!MyUtility.Check.Empty(dr["portarrival"]))
                    {
                        this.dateArrivePortDate.Value = DateTime.Parse(dr["portarrival"].ToString());
                    }

                    if (!MyUtility.Check.Empty(dr["docarrival"]))
                    {
                        this.dateDoxRcvDate.Value = DateTime.Parse(dr["docarrival"].ToString());
                    }
                }

                this.dateETA.Enabled = MyUtility.Check.Empty(this.CurrentMaintain["third"]) || this.CurrentMaintain["third"].ToString() == "True";
            }

            #region Status Label

            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            this.radioEncodeSeq.Checked = false;
            this.radiobySP.Checked = true;

            // 排序要回歸SP#
            this.GridSortBy(false);
            this.Change_record();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Color backDefaultColor = this.detailgrid.DefaultCellStyle.BackColor;
            #region SP# Vaild 判斷此sp#存在po中。
            DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["poid"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["invno"]) && !this.txtInvoiceNo.Focused)
                {
                    this.CurrentDetailData["poid"] = string.Empty;
                    MyUtility.Msg.WarningBox("< Invoice# >  can't be empty!", "Warning");
                    this.txtInvoiceNo.Focus();
                    return;
                }

                if (this.EditMode && e.FormattedValue.ToString() != string.Empty)
                {
                    if (MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po WITH (NOLOCK) where id = '{0}')", e.FormattedValue), null))
                    {
                        string sqlorders = string.Format("select category,FactoryID,OrderTypeID from orders WITH (NOLOCK) where id='{0}'", e.FormattedValue);
                        if (MyUtility.Check.Seek(sqlorders, out DataRow dr))
                        {
                            if (MyUtility.Convert.GetString(dr["category"]) == "M")
                            {
                                this.CurrentDetailData["stocktype"] = "I";
                            }
                            else
                            {
                                this.CurrentDetailData["stocktype"] = "B";
                            }

                            this.CurrentDetailData["FactoryID"] = dr["FactoryID"];
                            this.CurrentDetailData["OrderTypeID"] = dr["OrderTypeID"];

                            // 開始檢查FtyInventory
                            string poid = MyUtility.Convert.GetString(this.CurrentDetailData["poid"]);
                            string seq1 = MyUtility.Convert.GetString(this.CurrentDetailData["seq1"]);
                            string seq2 = MyUtility.Convert.GetString(this.CurrentDetailData["seq2"]);
                            string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                            string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                            string fabricType = MyUtility.Convert.GetString(this.CurrentDetailData["fabrictype"]);
                            string stockType = MyUtility.Convert.GetString(this.CurrentDetailData["stockType"]);

                            // 布料，且都有值了才檢查
                            if (fabricType.ToUpper() == "F" && !MyUtility.Check.Empty(poid) && !MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2) && !MyUtility.Check.Empty(roll) && !MyUtility.Check.Empty(dyelot))
                            {
                                bool chkFtyInventory = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                                if (!chkFtyInventory)
                                {
                                    MyUtility.Msg.WarningBox($"The Roll & Dyelot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["poid"] = string.Empty;
                        MyUtility.Msg.WarningBox("SP# is not exist!!", "Data not found");
                        return;
                    }

                    this.CurrentDetailData["poid"] = e.FormattedValue;
                }
            };

            #endregion SP# Vaild 判斷此sp#的cateogry存在 order_tmscost

            #region Seq 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;
                    if (MyUtility.Check.Empty(this.CurrentMaintain["exportid"]))
                    {
                        Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(this.CurrentDetailData["poid"].ToString(), this.CurrentDetailData["seq"].ToString(), "left(p.seq1,1) !='7'");
                        DialogResult result = selepoitem.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        x = selepoitem.GetSelecteds();
                    }
                    else
                    {
                        sqlcmd = $@"
select  e.poid
        , seq = concat (Ltrim (Rtrim (e.seq1)), ' ', e.Seq2)
        , e.Refno
        , [Description] = dbo.getmtldesc(e.poid,e.seq1,e.seq2,2,0)
        , ColorID = Color.Value
        , eta = (SELECT eta from dbo.export WITH (NOLOCK) where id = e.id)
        , M.InQty
        , p.pounit
        , StockUnit = dbo.GetStockUnitBySPSeq (p.id, p.seq1, p.seq2)
        , M.OutQty
        , M.AdjustQty
        , BalanceQty = M.inqty - M.OutQty + M.AdjustQty
        , M.LInvQty
        , p.fabrictype
        , e.seq1
        , e.seq2
from dbo.Export_Detail e WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on e.PoID = p.ID 
                                                and e.Seq1 = p.SEQ1 
                                                and e.Seq2 = p.seq2
left JOIN MDivisionPoDetail M WITH (NOLOCK) ON E.PoID = M.POID 
                                                and e.Seq1 = M.SEQ1 
                                                and e.Seq2 = M.seq2 
LEFT JOIN Fabric f WITH (NOLOCK) ON p.SCIRefNo=f.SCIRefNo
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN p.SuppColor
		 ELSE dbo.GetColorMultipleID(p.BrandID,p.ColorID)
	 END
)Color
where   e.PoID ='{this.CurrentDetailData["poid"]}' 
        and e.id = '{this.CurrentMaintain["exportid"]}'
and p.Junk=0
Order By e.Seq1, e.Seq2, e.Refno";

                        DBProxy.Current.Select(null, sqlcmd, out DataTable poitems);

                        string columns = "Seq,refno,description,colorid,eta,inqty,stockunit,outqty,adjustqty,BalanceQty,linvqty";
                        string heasercap = "Seq,Ref#,Description,Color,ETA,In Qty,Stock Unit,Out Qty,Adqty,Balance,Inventory Qty";
                        string columnwidths = "6,15,25,8,10,6,6,6,6,6,6";
                        SelectItem item = new SelectItem(poitems, columns, columnwidths, this.CurrentDetailData["seq"].ToString(), heasercap)
                        {
                            Width = 1024,
                        };
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        x = item.GetSelecteds();
                    }

                    this.CurrentDetailData["seq"] = x[0]["seq"];
                    this.CurrentDetailData["seq1"] = x[0]["seq1"];
                    this.CurrentDetailData["seq2"] = x[0]["seq2"];
                    this.CurrentDetailData["pounit"] = x[0]["pounit"];
                    this.CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    this.CurrentDetailData["fabrictype"] = x[0]["fabrictype"];
                    this.CurrentDetailData["PoidSeq1"] = this.CurrentDetailData["Poid"].ToString() + x[0]["seq1"];
                    this.CurrentDetailData["PoidSeq"] = this.CurrentDetailData["Poid"].ToString() + x[0]["seq"];
                    this.CurrentDetailData["Refno"] = x[0]["Refno"];
                    this.CurrentDetailData["ColorID"] = x[0]["ColorID"];

                    if ((decimal)this.CurrentDetailData["shipqty"] > 0)
                    {
                        this.Ship_qty_valid((decimal)this.CurrentDetailData["shipqty"]);
                    }

                    this.CurrentDetailData.EndEdit();
                }
            };
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["seq"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                string oldPoidSeq1 = MyUtility.Convert.GetString(this.CurrentDetailData["Poid"]);
                string oldPoidSeq = MyUtility.Convert.GetString(this.CurrentDetailData["Poid"]);
                if (oldvalue == newvalue)
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
                        this.CurrentDetailData["pounit"] = string.Empty;
                        this.CurrentDetailData["stockunit"] = string.Empty;
                        this.CurrentDetailData["fabrictype"] = string.Empty;
                        this.CurrentDetailData["shipqty"] = 0m;
                        this.CurrentDetailData["Actualqty"] = 0m;
                        this.CurrentDetailData["PoidSeq1"] = this.CurrentDetailData["Poid"];
                        this.CurrentDetailData["PoidSeq"] = this.CurrentDetailData["Poid"];
                        this.CurrentDetailData["Refno"] = string.Empty;
                        this.CurrentDetailData["ColorID"] = string.Empty;
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

                        string sqlmcd = string.Format(Prgs.SelePoItemSqlCmd() + @"and p.seq1 ='{2}' and p.seq2 = '{3}' and left(p.seq1, 1) !='7'", this.CurrentDetailData["poid"], Env.User.Keyword, seq[0], seq[1]);
                        if (!MyUtility.Check.Seek(sqlmcd, out DataRow dr))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }
                        else
                        {
                            sqlmcd = $@"select  StockUnit = dbo.GetStockUnitBySPSeq ('{this.CurrentDetailData["poid"]}', '{seq[0]}', '{seq[1]}')";
                            bool unti_result = MyUtility.Check.Seek(sqlmcd, out DataRow dr_StockUnit, null);
                            this.CurrentDetailData["stockunit"] = unti_result ? dr_StockUnit["stockunit"] : dr["stockunit"];
                            this.CurrentDetailData["seq"] = e.FormattedValue;
                            this.CurrentDetailData["seq1"] = seq[0];
                            this.CurrentDetailData["seq2"] = seq[1];
                            this.CurrentDetailData["pounit"] = dr["pounit"];
                            this.CurrentDetailData["fabrictype"] = dr["fabrictype"];
                            this.CurrentDetailData["PoidSeq1"] = this.CurrentDetailData["Poid"] + seq[0];
                            this.CurrentDetailData["PoidSeq"] = this.CurrentDetailData["Poid"].ToString() + e.FormattedValue;
                            this.CurrentDetailData["Refno"] = dr["Refno"];
                            this.CurrentDetailData["ColorID"] = dr["WH_P07_Color"];

                            // 開始檢查FtyInventory
                            string poid = MyUtility.Convert.GetString(this.CurrentDetailData["poid"]);
                            string seq1 = MyUtility.Convert.GetString(this.CurrentDetailData["seq1"]);
                            string seq2 = MyUtility.Convert.GetString(this.CurrentDetailData["seq2"]);
                            string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                            string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                            string fabricType = MyUtility.Convert.GetString(this.CurrentDetailData["fabrictype"]);
                            string stockType = MyUtility.Convert.GetString(this.CurrentDetailData["stockType"]);

                            // 布料，且都有值了才檢查
                            if (fabricType.ToUpper() == "F" && !MyUtility.Check.Empty(poid) && !MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2) && !MyUtility.Check.Empty(roll) && !MyUtility.Check.Empty(dyelot))
                            {
                                bool chkFtyInventory1 = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                                if (!chkFtyInventory1)
                                {
                                    MyUtility.Msg.WarningBox($"The Roll & Dyelot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");

                                    // 未通過驗證 清空欄位
                                    this.CurrentDetailData["seq"] = string.Empty;
                                    this.CurrentDetailData["seq1"] = string.Empty;
                                    this.CurrentDetailData["seq2"] = string.Empty;
                                    this.CurrentDetailData["pounit"] = string.Empty;
                                    this.CurrentDetailData["stockunit"] = string.Empty;
                                    this.CurrentDetailData["fabrictype"] = string.Empty;
                                    this.CurrentDetailData["shipqty"] = 0m;
                                    this.CurrentDetailData["Actualqty"] = 0m;
                                    this.CurrentDetailData["PoidSeq1"] = oldPoidSeq1;
                                    this.CurrentDetailData["PoidSeq"] = oldPoidSeq;
                                    this.CurrentDetailData["Refno"] = string.Empty;
                                    this.CurrentDetailData["ColorID"] = string.Empty;

                                    this.CurrentDetailData.EndEdit();
                                    return;
                                }
                            }

                            if ((decimal)this.CurrentDetailData["shipqty"] > 0)
                            {
                                this.Ship_qty_valid((decimal)this.CurrentDetailData["shipqty"]);
                            }
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            #region Location 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.CurrentDetailData["stocktype"].ToString(), this.CurrentDetailData["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["location"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Location"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = $@"
SELECT  id 
FROM    DBO.MtlLocation WITH (NOLOCK)
WHERE   StockType='{this.CurrentDetailData["stocktype"].ToString()}'
        and junk != '1'";
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                    string[] getLocation = this.CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["location"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };

            #endregion Location 右鍵開窗

            #region Ship Qty Valid

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["shipqty"]);
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                this.Ship_qty_valid((decimal)e.FormattedValue);
            };

            #endregion Ship Qty Valid

            #region In Qty Valid

            DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["ActualQty"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue != null)
                {
                    #region 加總Actualqty到TtlQty
                    this.CurrentDetailData["Actualqty"] = e.FormattedValue;
                    string combineBarcode = MyUtility.Convert.GetString(this.CurrentDetailData["CombineBarcode"]);
                    decimal ttlValue = 0;
                    if (!MyUtility.Check.Empty(combineBarcode))
                    {
                        ttlValue = (decimal)this.DetailDatas.CopyToDataTable().Compute("sum(Actualqty)", $"CombineBarcode = '{combineBarcode}'");
                        foreach (DataRow dr in this.DetailDatas)
                        {
                            if (MyUtility.Check.Empty(dr["Unoriginal"]) &&
                                string.Compare(combineBarcode, dr["CombineBarcode"].ToString()) == 0)
                            {
                                dr["TtlQty"] = ttlValue + $" {dr["pounit"]}";
                                dr.EndEdit();
                            }
                        }
                    }
                    else
                    {
                        this.CurrentDetailData["TtlQty"] = e.FormattedValue + $" {this.CurrentDetailData["pounit"]}";
                    }

                    #endregion

                    if (!MyUtility.Check.Empty(this.CurrentDetailData["pounit"]) && !MyUtility.Check.Empty(this.CurrentDetailData["stockunit"]))
                    {
                        string rate = MyUtility.GetValue.Lookup($@"select RateValue from dbo.View_Unitrate v where v.FROM_U ='{this.CurrentDetailData["pounit"]}' and v.TO_U='{this.CurrentDetailData["stockunit"]}'");
                        this.CurrentDetailData["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
                    }
                }

                this.Change_Color();
            };

            #endregion In Qty Valid

            #region Roll驗證

            Ict.Win.DataGridViewGeneratorTextColumnSettings roll_setting1 = new DataGridViewGeneratorTextColumnSettings();
            roll_setting1.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Roll"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                this.CurrentDetailData["Roll"] = newvalue;

                // 開始檢查FtyInventory
                string poid = MyUtility.Convert.GetString(this.CurrentDetailData["poid"]);
                string seq1 = MyUtility.Convert.GetString(this.CurrentDetailData["seq1"]);
                string seq2 = MyUtility.Convert.GetString(this.CurrentDetailData["seq2"]);
                string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                string fabricType = MyUtility.Convert.GetString(this.CurrentDetailData["fabrictype"]);
                string stockType = MyUtility.Convert.GetString(this.CurrentDetailData["stockType"]);

                // 布料，且都有值了才檢查
                if (fabricType.ToUpper() == "F" && !MyUtility.Check.Empty(poid) && !MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2) && !MyUtility.Check.Empty(roll) && !MyUtility.Check.Empty(dyelot))
                {
                    bool chkFtyInventory1 = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                    if (!chkFtyInventory1)
                    {
                        MyUtility.Msg.WarningBox($"The Roll & Dyelot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");

                        // 未通過驗證 清空欄位
                        this.CurrentDetailData["Roll"] = string.Empty;

                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                }
            };
            #endregion

            #region Dyelot驗證

            Ict.Win.DataGridViewGeneratorTextColumnSettings dyelot_setting1 = new DataGridViewGeneratorTextColumnSettings();

            dyelot_setting1.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Dyelot"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                this.CurrentDetailData["Dyelot"] = newvalue;

                // 開始檢查FtyInventory
                string poid = MyUtility.Convert.GetString(this.CurrentDetailData["poid"]);
                string seq1 = MyUtility.Convert.GetString(this.CurrentDetailData["seq1"]);
                string seq2 = MyUtility.Convert.GetString(this.CurrentDetailData["seq2"]);
                string roll = MyUtility.Convert.GetString(this.CurrentDetailData["roll"]);
                string dyelot = MyUtility.Convert.GetString(this.CurrentDetailData["dyelot"]);
                string fabricType = MyUtility.Convert.GetString(this.CurrentDetailData["fabrictype"]);
                string stockType = MyUtility.Convert.GetString(this.CurrentDetailData["stockType"]);

                // 布料，且都有值了才檢查
                if (fabricType.ToUpper() == "F" && !MyUtility.Check.Empty(poid) && !MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2) && !MyUtility.Check.Empty(roll) && !MyUtility.Check.Empty(dyelot))
                {
                    bool chkFtyInventory1 = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                    if (!chkFtyInventory1)
                    {
                        MyUtility.Msg.WarningBox($"The Roll & Dyelot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");

                        // 未通過驗證 清空欄位
                        this.CurrentDetailData["Dyelot"] = string.Empty;

                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                }
            };
            #endregion

            #region Roll setting
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

            #region Dyelot setting
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

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Seq;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_poid;

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CheckBox("IsSelect", header: string.Empty, width: Widths.AnsiChars(1), iseditable: true, trueValue: true, falseValue: false)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(11), settings: ts4).Get(out cbb_poid) // 1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts).Get(out cbb_Seq) // 2
            .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(9), iseditable: false).Get(out cbb_fabrictype) // 3
            .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 10, settings: ns) // 4
            .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 7) // 5
            .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(7), decimal_places: 2, integer_places: 7).Get(out this.Col_ActualW) // 6
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), settings: roll_setting).Get(out this.col_Roll) // 7
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), settings: dyelot_setting).Get(out this.col_Dyelot) // 8
            .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 10, settings: ns2)
            .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true) // 10
            .Text("TtlQty", header: "Total Qty", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 12
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 13
            .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", width: Widths.AnsiChars(8), iseditable: false).Get(out cbb_stocktype) // 14
            .Text("Location", header: "Location", settings: ts2, iseditingreadonly: false)
            .Text("remark", header: "Remark", iseditingreadonly: false)
            .Text("RefNo", header: "Ref#", iseditingreadonly: true) // 17
            .Text("ColorID", header: "Color", iseditingreadonly: true) // 18
            .Text("FactoryID", header: "Prod. Factory", iseditingreadonly: true) // 19
            .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(15), iseditingreadonly: true) // 20
            .Text("ContainerType", header: "ContainerType & No", width: Widths.AnsiChars(15), iseditingreadonly: true) // 21
            ;
            this.col_Roll.MaxLength = 8;
            this.col_Dyelot.MaxLength = 8;
            cbb_Seq.MaxLength = 6;
            cbb_poid.MaxLength = 13;
            #endregion 欄位設定

            #region Add Column [btnAdd2] 第一欄[+][-]按鈕

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

            cbb_fabrictype.DataSource = new BindingSource(this.di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            #region gridView 事件
            this.Change_Color();
            this.detailgrid.CellClick += this.Detailgrid_CellClick;
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
            this.detailgrid.ColumnHeaderMouseClick += this.Detailgrid_ColumnHeaderMouseClick;
            this.detailgrid.RowsAdded += (s, e) =>
            {
                if (this.EditMode || e.RowIndex < 0)
                {
                    return;
                }

                #region 變色規則，若 stockunit && stocktype != '' 則需變回預設的 Color
                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataGridViewRow dr = this.detailgrid.Rows[index];
                    dr.DefaultCellStyle.BackColor = (MyUtility.Check.Empty(dr.Cells["stockunit"].Value) || MyUtility.Check.Empty(dr.Cells["stocktype"].Value)) ? Color.FromArgb(255, 192, 203) : backDefaultColor;
                    index++;
                }
                #endregion
            };
            #endregion

        }

        private void Change_Color()
        {
            this.Col_ActualW.CellFormatting += (s, e) =>
            {
                if (this.isSetZero)
                {
                    var rowIndex = this.detailgrid.CurrentCell.RowIndex;
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[rowIndex].Cells[0];
                    this.isSetZero = false;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                decimal gw = MyUtility.Convert.GetDecimal(dr["weight"]);
                decimal aw = MyUtility.Convert.GetDecimal(dr["actualweight"]);
                if (dr["fabrictype"].ToString() == "F" && gw > aw)
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
                }
            };
        }

        private void Detailgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.EditMode == false || e.ColumnIndex < 0 || this.detailgrid == null)
            {
                return;
            }

            DataRow pre_row = this.detailgrid.GetDataRow(this.detailgridbs.Position);

            // 要主料才能使用+-按鈕功能
            if (this.detailgrid.Columns[e.ColumnIndex].Name == "btnAdd2" && pre_row["FabricType"].ToString() == "F")
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

                    // 在插入新的row前，將現有資料clickInsert確保為1
                    ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().ForEach(f => f["clickInsert"] = 1);
                    int insertIndex = ((DataTable)this.detailgridbs.DataSource).Rows.IndexOf(this.detailgrid.GetDataRow(e.RowIndex)) + 1;

                    // 新增資料，位置插入在點擊的下一行
                    base.OnDetailGridInsert(insertIndex);

                    // 取得新插入的那筆
                    DataRow newrow = ((DataTable)this.detailgridbs.DataSource).Select($"clickInsert = 0")[0];
                    newrow["fabrictype"] = "F";
                    newrow["Dyelot"] = pre_row["Dyelot"];
                    newrow["Roll"] = pre_row["Roll"];
                    newrow["Unoriginal"] = 1;
                    newrow["Stocktype"] = 'B';
                    newrow["CombineBarcode"] = pre_ComBarcode;
                    newrow["EncodeSeq"] = pre_row["EncodeSeq"];

                    // 新增子項要預設父項的Combine SKU, 這是為避免當排序變更後會亂掉
                    newrow["SortCmbPOID"] = pre_row["SortCmbPOID"];
                    newrow["SortCmbSeq1"] = pre_row["SortCmbSeq1"];
                    newrow["SortCmbSeq2"] = pre_row["SortCmbSeq2"];
                    newrow["SortCmbRoll"] = pre_row["SortCmbRoll"];
                    newrow["SortCmbDyelot"] = pre_row["SortCmbDyelot"];
                    DataGridViewButtonCell next_dgbtn = (DataGridViewButtonCell)this.detailgrid.CurrentRow.Cells["btnAdd2"];
                    next_dgbtn.Value = "-";

                    // 排序要回到EncodeSeq
                    this.GridSortBy(true);
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
            this.detailgrid.ValidateControl();
            string strSort = ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort.ToString();
            if (!this.EditMode)
            {
                this.radiobySP.Checked = false;
                this.radioEncodeSeq.Checked = false;
            }

            if (this.DetailDatas != null)
            {
                if (MyUtility.Check.Empty(((DataTable)this.detailgridbs.DataSource).DefaultView.Sort))
                {
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = $"{strSort}";
                }
            }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void Ship_qty_valid(decimal ship_qty)
        {
            if (this.CurrentDetailData == null)
            {
                return;
            }

            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(this.CurrentDetailData["pounit"]) && !MyUtility.Check.Empty(this.CurrentDetailData["stockunit"]))
                {
                    this.CurrentDetailData["Actualqty"] = ship_qty;
                    string rate = MyUtility.GetValue.Lookup(string.Format(
                        @"
select RateValue 
from dbo.View_Unitrate v
where   v.FROM_U ='{0}' 
        and v.TO_U='{1}'", this.CurrentDetailData["pounit"], this.CurrentDetailData["stockunit"]));
                    this.CurrentDetailData["stockqty"] = MyUtility.Math.Round(decimal.Parse(ship_qty.ToString()) * decimal.Parse(rate), 2);
                    this.CurrentDetailData["shipqty"] = ship_qty;
                    this.CurrentDetailData.EndEdit();
                }
            }

            this.Change_Color();
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

            string upd_MD_2T = string.Empty;
            string upd_MD_8T = string.Empty;
            string upd_Fty_2T = string.Empty;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty, sqlcmd4 = string.Empty;

            DualResult result, result2, result3;

            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["PackingReceive"]))
            {
                MyUtility.Msg.WarningBox("<Packing Receive Date>  can't be empty!", "Warning");
                this.datePLRcvDate.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["whseArrival"]))
            {
                MyUtility.Msg.WarningBox("< Arrive W/H Date >  can't be empty!", "Warning");
                this.dateArriveWHDate.Focus();
                return;
            }
            #endregion

            string sq = string.Empty;
            foreach (DataRow item in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (MyUtility.Convert.GetDecimal(item["stockqty"]) < 0)
                {
                    sq += $@"SP#: {item["poid"]} Seq#: {item["seq"]}-{item["seq"]} Roll#: {item["Roll"]}'s Receiving Qty must not be less than 0 ! 
";
                }
            }

            if (!MyUtility.Check.Empty(sq))
            {
                MyUtility.Msg.WarningBox(sq);
                return;
            }

            // 判斷FtyInventory是否已經存在
            if (!this.ChkFtyInventory_Exists())
            {
                return;
            }

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.StockQty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
        , d.Dyelot
from dbo.Receiving_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) + d.StockQty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than stock qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["stockqty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存
            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"
update Receiving 
set status='Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量 mdivisionPoDetail
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
                                  Qty = m.Sum(w => w.Field<decimal>("stockqty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();
            var data_MD_8T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
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
                                  Qty = m.Sum(w => w.Field<decimal>("stockqty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();

            #endregion

            #region -- 更新庫存數量  ftyinventory --

            int mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system")) ? 1 : 0;
            var data_Fty_2T = (from m in this.DetailDatas
                               select new
                               {
                                   poid = m.Field<string>("poid"),
                                   seq1 = m.Field<string>("seq1"),
                                   seq2 = m.Field<string>("seq2"),
                                   stocktype = m.Field<string>("stocktype"),
                                   qty = m.Field<decimal>("stockqty"),
                                   location = m.Field<string>("location"),
                                   roll = m.Field<string>("roll"),
                                   dyelot = m.Field<string>("dyelot"),
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
                barcodeList = Prgs.GetBarcodeNo("FtyInventory_Barcode", "F", distCnt1.Length + count2.Length);
                int cnt = 0;
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

            #region Base on wkno 收料時，需回寫export
            sqlcmd4 = string.Format(
                @"
update dbo.export 
set whsearrival = (select WhseArrival 
                   from dbo.receiving 
                   where id = '{2}')
    , packingarrival = (select PackingReceive 
                        from dbo.receiving 
                        where id='{2}')
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["exportid"], this.CurrentMaintain["id"]);
            #endregion

            #region 更新FIR,AIR資料

            List<SqlParameter> fir_Air_Proce = new List<SqlParameter>
            {
                new SqlParameter("@ID", this.CurrentMaintain["ID"]),
                new SqlParameter("@LoginID", this.UserID),
            };

            if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.insert_Air_Fir", fir_Air_Proce)))
            {
                Exception ex = result.GetException();
                MyUtility.Msg.InfoBox(ex.Message.Substring(ex.Message.IndexOf("Error Message:") + "Error Message:".Length));
                return;
            }
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out DataTable resulttb, "#TmpSource", conn: sqlConn)))
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

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!MyUtility.Check.Empty(this.CurrentMaintain["exportid"]))
                    {
                        if (!(result3 = DBProxy.Current.Execute(null, sqlcmd4)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(sqlcmd4, result);
                            return;
                        }
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
                finally
                {
                    transactionscope.Dispose();
                }
            }

            // AutoWHFabric WebAPI for Gensong
            this.SentToGensong_AutoWHFabric(true);
            this.SentToVstrong_AutoWH_ACC(true);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty, sqlcmd4 = string.Empty;
            DualResult result, result2, result3;

            string upd_MD_2F = string.Empty;
            string upd_MD_8F = string.Empty;
            string upd_Fty_2F = string.Empty;

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.StockQty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
        , d.Dyelot
from dbo.Receiving_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - d.StockQty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than stock qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["stockqty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"
update Receiving 
set status='New'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量 po_supp_detail & ftyinventory
            var data_MD_2F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("poid"),
                                  seq1 = b.Field<string>("seq1"),
                                  seq2 = b.Field<string>("seq2"),
                                  stocktype = b.Field<string>("stocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Stocktype = m.First().Field<string>("stocktype"),
                                  Qty = -m.Sum(w => w.Field<decimal>("stockqty")),
                              }).ToList();

            var data_MD_8F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                              group b by new
                              {
                                  poid = b.Field<string>("poid"),
                                  seq1 = b.Field<string>("seq1"),
                                  seq2 = b.Field<string>("seq2"),
                                  stocktype = b.Field<string>("stocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Stocktype = m.First().Field<string>("stocktype"),
                                  Qty = -m.Sum(w => w.Field<decimal>("stockqty")),
                              }).ToList();

            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("poid"),
                                   seq1 = m.Field<string>("seq1"),
                                   seq2 = m.Field<string>("seq2"),
                                   stocktype = m.Field<string>("stocktype"),
                                   qty = -m.Field<decimal>("stockqty"),
                                   location = m.Field<string>("location"),
                                   roll = m.Field<string>("roll"),
                                   dyelot = m.Field<string>("dyelot"),
                               }).ToList();

            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
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

            sqlcmd4 = string.Format(
                @"
declare @whseArrival date
select top 1 @whseArrival = WhseArrival from dbo.Receiving WITH (NOLOCK) where ExportId = '{1}' and ID <> '{2}' and Status = 'Confirmed'
IF @whseArrival is null
BEGIN
update dbo.export 
set whsearrival = null
    , packingarrival = null
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'
END", Env.User.UserID, this.CurrentMaintain["exportid"], this.CurrentMaintain["id"]);
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region MdivisionPoDetail
                    if (data_MD_2F.Count > 0)
                    {
                        upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (data_MD_8F.Count > 0)
                    {
                        upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F, out resulttb, "#TmpSource")))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }
                    #endregion

                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource")))
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

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!MyUtility.Check.Empty(this.CurrentMaintain["exportid"]))
                    {
                        if (!(result3 = DBProxy.Current.Execute(null, sqlcmd4)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(sqlcmd4, result);
                            return;
                        }
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
            this.SentToGensong_AutoWHFabric(false);
            this.SentToVstrong_AutoWH_ACC(false);
        }

        /// <summary>
        ///  AutoWHFabric WebAPI for Gensong
        /// </summary>
        private void SentToGensong_AutoWHFabric(bool isConfirmed)
        {
            DataTable dtDetail = new DataTable();
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                string sqlGetData = string.Empty;
                sqlGetData = $@"
SELECT [ID] = rd.id
,[InvNo] = r.InvNo
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[ColorID] = Color.Value
,[Roll] = rd.Roll
,[Dyelot] = rd.Dyelot
,[StockUnit] = rd.StockUnit
,[StockQty] = rd.StockQty
,[PoUnit] = rd.PoUnit
,[ShipQty] = rd.ShipQty
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[Ukey] = rd.Ukey
,[IsInspection] = convert(bit, 0)
,[ETA] = r.ETA
,[WhseArrival] = r.WhseArrival
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,[Barcode] = Barcode.value
FROM Production.dbo.Receiving_Detail rd
inner join Production.dbo.Receiving r on rd.id = r.id
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
	and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
left join Production.dbo.FtyInventory f on f.POID = rd.PoId
	and f.Seq1=rd.Seq1 and f.Seq2=rd.Seq2 
	and f.Dyelot = rd.Dyelot and f.Roll = rd.Roll
	and f.StockType = rd.StockType
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN po3.SuppColor
		 ELSE dbo.GetColorMultipleID(po3.BrandID,po3.ColorID)
	 END
)Color
outer apply(
	select value = min(fb.Barcode)
	from FtyInventory_Barcode fb 
	where fb.Ukey = f.Ukey
)Barcode
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='F'
)
and r.id = '{this.CurrentMaintain["id"]}'

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
,[StockUnit] = rd.StockUnit
,[StockQty] = rd.StockQty
,[PoUnit] = rd.PoUnit
,[ShipQty] = rd.ShipQty
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[MtlType] = Fabric.MtlTypeID
,[Ukey] = rd.Ukey
,[ETA] = r.ETA
,[WhseArrival] = r.WhseArrival
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
FROM Production.dbo.Receiving_Detail rd
inner join Production.dbo.Receiving r on rd.id = r.id
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

                Task.Run(() => new Vstrong_AutoWHAccessory().SentReceive_DetailToVstrongAutoWHAccessory(dtDetail))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        // 寫明細撈出的sql command
        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"
select  a.id
        , a.PoId
        , a.Seq1
        , a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Poid + concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as PoidSeq
        , a.Poid + Ltrim(Rtrim(a.seq1)) as PoidSeq1
        , (select p1.FabricType from PO_Supp_Detail p1 WITH (NOLOCK) where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as fabrictype
        , [RefNo]=p.RefNo
		, [ColorID]=Color.Value
        , a.shipqty
        , a.Weight
        , a.ActualWeight
        , a.Roll
        , a.Dyelot
        , a.ActualQty
		, TtlQty = convert(varchar(20),
			iif(a.CombineBarcode is null , a.ActualQty, 
				iif(a.Unoriginal is  null , ttlQty.value, null))) +' '+ a.PoUnit
        , a.PoUnit
        , a.StockQty
        , a.StockUnit
        , a.StockType
        , a.Location
        , a.remark
        , a.ukey
        ,o.FactoryID
        ,o.OrderTypeID
		,b.ExportId
		, [ContainerType]= Container.Val
        ,Barcode = isnull(ft.barcode,'')
		,a.CombineBarcode
        ,a.Unoriginal 
        ,a.EncodeSeq
        ,[SortCmbPOID] = ISNULL(cmb.PoId,a.PoId)
		,[SortCmbSeq1] = ISNULL(cmb.Seq1,a.Seq1)
		,[SortCmbSeq2] = ISNULL(cmb.Seq2,a.Seq2)
		,[SortCmbRoll] = ISNULL(cmb.Roll,a.Roll)
		,[SortCmbDyelot] = ISNULL(cmb.Dyelot,a.Dyelot)
        ,clickInsert = 1 -- 用來處理按+插入列狀況
        ,[IsSelect] = cast(0 as bit)
from dbo.Receiving_Detail a WITH (NOLOCK) 
INNER JOIN Receiving b WITH (NOLOCK) ON a.id= b.Id
left join orders o WITH (NOLOCK) on o.id = a.PoId
LEFT JOIN PO_Supp_Detail p  WITH (NOLOCK) ON p.ID=a.PoId AND p.SEQ1=a.Seq1 AND p.SEQ2 = a.Seq2
LEFT JOIN Fabric f WITH (NOLOCK) ON p.SCIRefNo=f.SCIRefNo
left join Receiving_Detail cmb on  a.Id = cmb.Id
									and a.CombineBarcode = cmb.CombineBarcode
									and cmb.CombineBarcode is not null
									and ISNULL(cmb.Unoriginal,0) = 0
left join FtyInventory ft on ft.POID = a.PoId
                            and ft.Seq1 = a.Seq1 
                            and ft.Seq2 = a.Seq2
                            and ft.StockType = a.StockType 
                            and ft.Roll =a.Roll 
                            and ft.Dyelot = a.Dyelot
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN p.SuppColor
		 ELSE dbo.GetColorMultipleID(o.BrandID,p.ColorID)
	 END
)Color
OUTER APPLY(
	SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_Detail_Ukey IN (
			SELECT Ukey
			FROM Export_Detail ed
			WHERE ed.ID = b.ExportId
		)
		AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
		FOR XML PATH('')
	),1,1,'')
)Container
outer apply(
	select value = sum(t.ActualQty)
	from Receiving_Detail t WITH (NOLOCK) 
	where t.ID=a.ID
	and t.CombineBarcode=a.CombineBarcode
	and t.CombineBarcode is not null
)ttlQty
Where a.id = '{0}'
order by a.EncodeSeq, SortCmbPOID, SortCmbSeq1, SortCmbSeq2, SortCmbRoll, SortCmbDyelot, Unoriginal, a.POID, a.Seq1, a.Seq2, a.Roll, a.Dyelot
", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        private void TxtInvoiceNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtInvoiceNo.Text != this.txtInvoiceNo.OldValue)
            {
                for (int i = 0; i < ((DataTable)this.detailgridbs.DataSource).Rows.Count;)
                {
                    DataRow drDetail = ((DataTable)this.detailgridbs.DataSource).Rows[i];
                    if (drDetail.RowState == DataRowState.Deleted)
                    {
                        i++;
                        continue;
                    }

                    // 清空表身資料
                    drDetail.Delete();
                }

                this.CurrentMaintain["invno"] = this.txtInvoiceNo.Text;
                this.CurrentMaintain["ETA"] = DBNull.Value;
                this.CurrentMaintain["WhseArrival"] = DBNull.Value;
                this.dateDoxRcvDate.Value = null;
                this.dateArrivePortDate.Value = null;
                string sqlcmd = $@"
select
    packingarrival,
    whsearrival,
    eta,
    PortArrival,
    DocArrival 
from dbo.export WITH (NOLOCK) 
where id = '{this.txtInvoiceNo.Text}'
";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow dr, null))
                {
                    if (!MyUtility.Check.Empty(dr["portarrival"]))
                    {
                        this.dateArrivePortDate.Value = DateTime.Parse(dr["portarrival"].ToString());
                    }

                    this.CurrentMaintain["exportid"] = this.CurrentMaintain["invno"];
                    this.CurrentMaintain["PackingReceive"] = dr["packingarrival"];
                    this.CurrentMaintain["WhseArrival"] = dr["WhseArrival"];
                    if (!MyUtility.Check.Empty(dr["DocArrival"]))
                    {
                        this.dateDoxRcvDate.Value = DateTime.Parse(dr["DocArrival"].ToString());
                    }

                    this.CurrentMaintain["ETA"] = dr["ETA"];
                    this.CurrentMaintain["third"] = 0;
                    this.dateETA.Enabled = false;
                    string selCom = $@"
select
	a.id
	, a.poid
	, a.seq1
	, a.seq2
	, a.UnitId
	, Weight = iif(pll.GW is not null, pll.GW, a.WeightKg)
	, ActualWeight = iif(pll.GW is not null, pll.GW, a.NetKg)
	, stocktype = iif(c.category='M','I','B')
	, b.POUnit 
	, StockUnit = dbo.GetStockUnitBySPSeq (b.id, b.seq1, b.seq2)
	, b.FabricType
	, seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
	, stockqty = round(x.qty * v.RateValue,2)
	, shipqty = x.qty
	, Actualqty = x.qty
	, Roll = convert(varchar(8), isnull(pll.PackageNo, ''))
	, Dyelot = convert(varchar(8), isnull(pll.BatchNo, ''))
	, remark = ''
	, location = ''
	, b.Refno
	, [ColorID] = Color.Value
	, c.FactoryID
	, c.OrderTypeID
	, [ContainerType] = Container.Val
	, pll.QRCode -- b.FabricType = 'F'  相同 QRCode 其中一筆為 [+] 剩下為 [-]
    , clickInsert = 1
	, DRQ = IIF(isnull(pll.QRCode, '') = '', Null, DENSE_RANK() over(order by pll.QRCode))
into #tmp
from dbo.Export_Detail a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on a.PoID= b.id   
                                                 and a.Seq1 = b.SEQ1    
                                                 and a.Seq2 = b.SEQ2    
inner join orders c WITH (NOLOCK) on c.id = a.poid
inner join View_unitrate v on v.FROM_U = b.POUnit and v.TO_U=dbo.GetStockUnitBySPSeq (b.id, b.seq1, b.seq2)
LEFT JOIN Fabric WITH (NOLOCK) ON b.SCIRefNo=Fabric.SCIRefNo
left join POShippingList_Line pll WITH (NOLOCK) ON pll.Export_Detail_Ukey = a.Ukey and b.FabricType = 'F'
outer apply(select qty = iif(b.FabricType = 'F' and pll.Export_Detail_Ukey is not null, pll.ShipQty + pll.FOC, a.Qty + a.Foc))x
OUTER APPLY(
	SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_Detail_Ukey = a.Ukey
		FOR XML PATH('')
	),1,1,'')
)Container
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN b.SuppColor
		 ELSE dbo.GetColorMultipleID(b.BrandID,b.ColorID)
	 END
)Color
where a.id='{this.CurrentMaintain["exportid"]}'

select *,
	Unoriginal_0 =  IIF(isnull(QRCode, '') <> '' and COUNT(1) over(partition by DRQ) > 1,
							row_number() over(partition by DRQ order by poid, seq1, seq2, FabricType) - 1,
							null),
	DRQ_0 = IIF(isnull(QRCode, '') <> '' and COUNT(1) over(partition by DRQ) > 1,DRQ,null)
into #tmp2
from #tmp

select t.*,
	Unoriginal =  CAST(IIF(Unoriginal_0 = 0, Null, Unoriginal_0) as bit), -- 次項目為1, 主項或其它為 Null, 不能為 0
	CombineBarcode_0 = IIF(DRQ_0 is not null and COUNT(1) over(partition by DRQ_0) > 1,
							DENSE_RANK() over(order by DRQ_0)
							,null),
	TtlQty = convert(varchar(20),
		iif(t.Unoriginal_0 is null , t.ActualQty, 
			iif(t.Unoriginal_0 = 0 , x.qty, null))) +' '+ PoUnit
into #tmp3
from #tmp2 t
outer apply(
	select qty = SUM(t2.Actualqty)
	from #tmp2 t2
	where t2.QRCode = t.QRCode
)x

select *, CombineBarcode_num = IIF(min(CombineBarcode_0) over() >1, CombineBarcode_0 - 1, CombineBarcode_0)
into #tmp4
from #tmp3

select*,CombineBarcode = IIF(CombineBarcode_num > 9,char(CombineBarcode_num + 55),convert(varchar(1), CombineBarcode_num)) -- CombineBarcode 編碼為1,2,3~9,A,B,C~Z
into #tmp5
from #tmp4

select *
from #tmp5
order by isnull(CombineBarcode, 'ZZ'), Unoriginal_0, poid, seq1, seq2, FabricType

drop table #tmp,#tmp2,#tmp3,#tmp4,#tmp5
";
                    DualResult result = DBProxy.Current.Select(null, selCom, out DataTable dt);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Export Data not found!!");
                        return;
                    }

                    foreach (DataRow item in dt.Rows)
                    {
                        item.SetAdded();
                        ((DataTable)this.detailgridbs.DataSource).ImportRow(item);
                    }
                }
                else
                {
                    this.CurrentMaintain["exportid"] = string.Empty;
                    this.CurrentMaintain["third"] = 1;
                    this.dateETA.Enabled = true;
                }

                this.Change_record();
            }
        }

        private void BtDeleteAllDetail_Click(object sender, EventArgs e)
        {
            List<DataRow> listCanDeleteDetail;

            switch (this.comboTypeFilter.Text)
            {
                case "Fabric":
                    listCanDeleteDetail = this.DetailDatas.Where(s => s["fabrictype"].ToString() == "F").ToList();
                    break;
                case "Accessory":
                    listCanDeleteDetail = this.DetailDatas.Where(s => s["fabrictype"].ToString() == "A").ToList();
                    break;
                default:
                    listCanDeleteDetail = this.DetailDatas.ToList();
                    break;
            }

            foreach (DataRow item in listCanDeleteDetail)
            {
                item.Delete();
            }
        }

        private void BtAccumulated_Click(object sender, EventArgs e)
        {
            var frm = new P07_AccumulatedQty(this.CurrentMaintain)
            {
                P07 = this,
            };
            frm.ShowDialog(this);
        }

        private void ComboTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboTypeFilter.SelectedIndex)
            {
                case 0:
                    this.detailgridbs.Filter = string.Empty;
                    this.btnDeleteAll.Text = "Delete all";
                    break;
                case 1:
                    this.detailgridbs.Filter = "fabrictype ='F'";
                    this.btnDeleteAll.Text = "Delete all fabric";
                    break;
                case 2:
                    this.detailgridbs.Filter = "fabrictype ='A'";
                    this.btnDeleteAll.Text = "Delete all acc.";
                    break;
            }

            this.Change_record();
        }

        private void BtModifyRollDyelot_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.InfoBox("Please modify data directly!!");
                return;
            }

            // 此功能只需顯示FabricType=F 資料,不須顯示副料
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, @"select * from #tmp where fabrictype='F'", out DataTable dt)))
            {
                this.ShowErr(result);
                return;
            }

            var frm = new P07_ModifyRollDyelot(dt, this.CurrentMaintain["id"].ToString(), this.GridAlias);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void BtFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index;

            // 判斷 Poid
            if (this.txtSeq1.CheckEmpty(showErrMsg: false))
            {
                index = this.detailgridbs.Find("PoId", this.txtLocateForSP.Text.TrimEnd());
            }

            // 判斷 Poid + Seq1
            else if (this.txtSeq1.CheckSeq2Empty())
            {
                index = this.detailgridbs.Find("PoIdSeq1", this.txtLocateForSP.Text.TrimEnd() + this.txtSeq1.Seq1);
            }

            // 判斷 Poid + Seq1 + Seq2
            else
            {
                index = this.detailgridbs.Find("PoIdSeq", this.txtLocateForSP.Text.TrimEnd() + this.txtSeq1.GetSeq());
            }

            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void BtImportFromExcel_Click(object sender, EventArgs e)
        {
            if (this.txtInvoiceNo.Text == string.Empty)
            {
                MyUtility.Msg.WarningBox("Invoice# Can't Null");
            }
            else
            {
                P07_ExcelImport callNextForm = new P07_ExcelImport(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
                callNextForm.ShowDialog(this);
            }

            this.Change_record();
        }

        private void BtDownloadSample_Click(object sender, EventArgs e)
        {
            // 呼叫執行檔絕對路徑
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);

            // 執行檔上一層絕對路徑
            // string xltpath = dir.Parent.FullName.ToString();
            // Microsoft.Office.Interop.Excel._Application ObjApp = MyUtility.Excel.ConnectExcel(xltpath + "\\xlt\\Warehouse_P07_ImportExcelFormat.xltx");
            // ObjApp.Visible = true;
            string strXltName = Env.Cfg.XltPathDir + "\\Warehouse_P07_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            DataTable details = (DataTable)this.detailgridbs.DataSource;
            List<string> poidList = details.AsEnumerable()
                .Select(row => row["poid"].ToString().TrimEnd())
                .Distinct()
                .ToList();

            P07_Print p = new P07_Print(poidList)
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            // p.CurrentDataRow = this.CurrentDataRow;
            p.ShowDialog();
            return true;
        }

        private void BtPrintSticker_Click(object sender, EventArgs e)
        {
            P07_Sticker s = new P07_Sticker(this.CurrentDataRow);
            s.ShowDialog();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            foreach (DataGridViewColumn index in this.detailgrid.Columns)
            {
                index.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.radioEncodeSeq.Checked = true;
            this.radiobySP.Checked = false;

            ((DataTable)this.detailgridbs.DataSource).AcceptChanges();
            base.ClickEditAfter();
            this.Change_record();
        }

        /// <summary>
        /// 刪除Receiving_Detail同時，如果FabricType=F，同時刪除FIR_Shadebone
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSave()
        {
            DualResult result = null;

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < ((DataTable)this.detailgridbs.DataSource).Rows.Count;)
                    {
                        DataRow dr = ((DataTable)this.detailgridbs.DataSource).Rows[i];

                        if (dr.RowState == DataRowState.Deleted)
                        {
                            string roll = dr["Roll", DataRowVersion.Original].ToString();
                            string dyelot = dr["Dyelot", DataRowVersion.Original].ToString();

                            // 判斷FabricType = F，才能刪除FIR_Shadebone
                            string sqlCmd = $@"
                                            SELECT  r.*,p.FabricType
                                            FROM Receiving_Detail r
                                            INNER JOIN PO_Supp_Detail p ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2 
                                            WHERE r.ID='{this.CurrentMaintain["ID"]}' AND Roll='{roll}' AND Dyelot='{dyelot}'  AND p.FabricType='F'
                                            ";
                            DBProxy.Current.Select(null, sqlCmd, out DataTable tmpdt);

                            if (tmpdt.Rows.Count > 0)
                            {
                                string fIR_ID = MyUtility.GetValue.Lookup($"select TOP 1 f.id from dbo.Receiving_Detail r INNER JOIN FIR f ON f.ReceivingID=r.ID AND f.POID=r.PoId AND f.SEQ1=r.Seq1 AND f.SEQ2=r.Seq2 WHERE r.id = '{this.CurrentMaintain["ID"]}' AND r.Roll='{roll}' AND r.Dyelot='{dyelot}'");

                                result = DBProxy.Current.Execute(null, $"DELETE FROM FIR_Shadebone WHERE ID ={fIR_ID} AND Roll='{roll}' AND Dyelot='{dyelot}'");

                                if (!result)
                                {
                                    break;
                                }
                            }

                            dr.Delete();
                        }

                        i++;
                    }

                    result = base.ClickSave();
                    if (result)
                    {
                        transactionScope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr(ex);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            foreach (DataGridViewColumn index in this.detailgrid.Columns)
            {
                index.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            base.ClickUndo();

            this.comboTypeFilter.SelectedIndex = 0;
            this.detailgridbs.Filter = string.Empty;
            this.radioPanel1.ReadOnly = this.EditMode ? true : false;
        }

        /// <inheritdoc/>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.EditMode)
            {
                switch (keyData)
                {
                    case Keys.Tab:
                        var currentCell = this.detailgrid.CurrentCell;
                        if (!currentCell.Empty())
                        {
                            var columnIndex = currentCell.ColumnIndex;
                            if (columnIndex == 8)
                            {
                                this.detailgrid.CurrentCell = this.detailgrid.Rows[currentCell.RowIndex].Cells[13];
                            }

                            if (columnIndex == 14)
                            {
                                int maxRow = this.detailgrid.Rows.Count;
                                if (currentCell.RowIndex + 1 < maxRow)
                                {
                                    this.detailgrid.CurrentCell = this.detailgrid.Rows[currentCell.RowIndex + 1].Cells[0];
                                }
                                else
                                {
                                    this.detailgrid.CurrentCell = this.detailgrid.Rows[0].Cells[0];
                                }

                                this.isSetZero = true;
                            }
                        }

                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool ChkFtyInventory_Exists()
        {
            List<string> listMsg = new List<string>();
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
                MyUtility.Msg.WarningBox(listMsg.JoinToString(string.Empty).TrimStart());
                return false;
            }

            return true;
        }

        private void BtUpdateWeight_Click(object sender, EventArgs e)
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
            newrow["poid"] = lastRow["poid"];
            newrow["seq1"] = lastRow["seq1"];
            newrow["seq2"] = lastRow["seq2"];
            newrow["seq"] = lastRow["seq"];
            newrow["poidseq"] = lastRow["poidseq"];
            newrow["poidseq1"] = lastRow["poidseq1"];
            newrow["fabrictype"] = lastRow["fabrictype"];
            newrow["shipqty"] = lastRow["shipqty"];
            newrow["ActualQty"] = lastRow["ActualQty"];
            newrow["stockqty"] = lastRow["stockqty"];
            newrow["weight"] = lastRow["weight"];
            newrow["pounit"] = lastRow["pounit"];
            newrow["stockunit"] = lastRow["stockunit"];
            newrow["Stocktype"] = lastRow["Stocktype"];
            newrow["Location"] = lastRow["Location"];

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

                    // 新增下一筆資料
                    DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.CurrentRow.Cells["btnAdd2"].RowIndex);
                    newrow["Dyelot"] = pre_row["Dyelot"];
                    newrow["Roll"] = pre_row["Roll"];
                    newrow["Unoriginal"] = 1;
                    newrow["Stocktype"] = 'B';
                    newrow["CombineBarcode"] = pre_ComBarcode;
                    newrow["EncodeSeq"] = pre_row["EncodeSeq"];

                    // 新增子項要預設父項的Combine SKU, 這是為避免當排序變更後會亂掉
                    newrow["SortCmbPOID"] = pre_row["SortCmbPOID"];
                    newrow["SortCmbSeq1"] = pre_row["SortCmbSeq1"];
                    newrow["SortCmbSeq2"] = pre_row["SortCmbSeq2"];
                    newrow["SortCmbRoll"] = pre_row["SortCmbRoll"];
                    newrow["SortCmbDyelot"] = pre_row["SortCmbDyelot"];
                    DataGridViewButtonCell next_dgbtn = (DataGridViewButtonCell)this.detailgrid.CurrentRow.Cells["btnAdd2"];
                    next_dgbtn.Value = "-";

                    // 排序要回到EncodeSeq
                    this.GridSortBy(true);
                }
            }

            this.Change_record();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            // 移除父項時子項也要一起刪除
            if (!MyUtility.Check.Empty(this.CurrentDetailData["CombineBarcode"]) &&
                (MyUtility.Convert.GetBool(this.CurrentDetailData["Unoriginal"]) == false))
            {
                var needDeleteChildItems = this.DetailDatas.Where(s => s["CombineBarcode"].ToString() == this.CurrentDetailData["CombineBarcode"].ToString() &&
                                                                       MyUtility.Convert.GetBool(s["Unoriginal"]));

                if (needDeleteChildItems.Any())
                {
                    foreach (DataRow dr in needDeleteChildItems)
                    {
                        dr.Delete();
                    }
                }
            }

            base.OnDetailGridDelete();
        }

        private void RadioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (this.detailgridbs.DataSource != null)
            {
                if (this.radioPanel1.Value == "1")
                {
                    // SP#, Seq, Roll, Dyelot
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = @"SortCmbPOID, SortCmbSeq1, SortCmbSeq2, SortCmbRoll, SortCmbDyelot, Unoriginal, POID, Seq1, Seq2, Roll, Dyelot ";
                }
                else
                {
                    // 使用OnDetailSelectCommandPrepare預設的排序(Encode Seq)
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = string.Empty;
                }

                this.Change_record();
            }
        }

        private void GridSortBy(bool isEncodeSeq)
        {
            if (isEncodeSeq)
            {
                ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = string.Empty;
            }
            else
            {
                ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = @"SortCmbPOID, SortCmbSeq1, SortCmbSeq2, SortCmbRoll, SortCmbDyelot, Unoriginal, POID, Seq1, Seq2, Roll, Dyelot ";
            }
        }

        private void BtnUpdateLocation_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            List<DataRow> dataRows = this.DetailDatas.Where(x => x.Field<bool>("IsSelect") &&
                                              x.Field<string>("StockType").EqualString(this.comboStockType.SelectedValue))
                                        .ToList();
            foreach (DataRow dr in dataRows)
            {
                dr["Location"] = this.txtMtlLocation1.Text;
            }
        }

        private void ComboStockType_SelectedValueChanged(object sender, EventArgs e)
        {
            this.txtMtlLocation1.Text = string.Empty;
        }
    }
}