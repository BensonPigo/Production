using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
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
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Class.MailTools;

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
            MyUtility.Tool.SetupCombox(this.comboTypeFilter, 2, 1, "ALL,ALL,Fabric,Fabric,Accessory,Accessory,No QR Code,No QR Code,Threads,Threads");
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
        protected override DualResult ClickDeletePre()
        {
            string sqlGetFIR_Physical = $@"
                            SELECT  fp.DetailUkey
                            FROM Receiving_Detail r with (nolock)
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
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                bool isEncodeSeq = this.radioEncodeSeq.Checked;
                this.GridSortBy(isEncodeSeq);
                this.Change_record();
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
                if (MyUtility.Convert.GetDecimal(row["ShipQty"]) > 999999999)
                {
                    if (!errormsgDir[errorkey].Contains("<Ship Qty> value can't be more than 999,999,999"))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Ship Qty> value can't be more than 999,999,999";
                    }
                }

                // listRowErrMsg.Add("<Ship Qty> value can't be more than 999,999,999");

                // ActualQty  numeric (11, 2)
                if (MyUtility.Convert.GetDecimal(row["ActualQty"]) > 999999999)
                {
                    if (!errormsgDir[errorkey].Contains("<Actual Qty> value can't be more than 999,999,999"))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Actual Qty> value can't be more than 999,999,999";
                    }
                }

                // listRowErrMsg.Add("<Actual Qty> value can't be more than 999,999,999");

                // actualWeight numeric (7, 2)
                if (MyUtility.Convert.GetDecimal(row["actualWeight"]) > 99999)
                {
                    if (!errormsgDir[errorkey].Contains("<Act.(kg)> value can't be more than 99,999"))
                    {
                        errormsgDir[errorkey] += Environment.NewLine + "<Act.(kg)> value can't be more than 99,999";
                    }
                }

                // listRowErrMsg.Add("<Act.(kg)> value can't be more than 99,999");

                // Weight numeric (7, 2)
                if (MyUtility.Convert.GetDecimal(row["Weight"]) > 99999)
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

            #region 檢查是否重複 MINDQRCode

            // 檢查畫面上
            var dupMINDQRCode = this.DetailDatas.AsEnumerable()
                .Where(w => !MyUtility.Check.Empty(w["MINDQRCode"]))
                .GroupBy(g => MyUtility.Convert.GetString(g["MINDQRCode"]))
                .Select(s => new { MINDQRCode = s.Key, ct = s.Count() })
                .Where(w => w.ct > 1).ToList();

            // 檢查DB
            var mINDQRCodes = "'" + this.DetailDatas.AsEnumerable()
                .Where(w => !MyUtility.Check.Empty(w["MINDQRCode"]) && !dupMINDQRCode.Select(s => s.MINDQRCode).Contains(MyUtility.Convert.GetString(w["MINDQRCode"])))
                .Select(s => MyUtility.Convert.GetString(s["MINDQRCode"]))
                .ToList().JoinToString("','") + "'";
            string sqlQR = $@"select r.Invno, rd.MINDQRCode from Receiving_Detail rd inner join Receiving r on r.id = rd.id where r.id <> '{this.CurrentMaintain["id"]}' and rd.MINDQRCode <>'' and rd.MINDQRCode in({mINDQRCodes})";
            DBProxy.Current.Select(null, sqlQR, out DataTable qrDT);
            if (dupMINDQRCode.Count > 0 || qrDT.Rows.Count > 0)
            {
                string msgDupQR = "Below QR Code already exist, cannot save!\r\n";

                foreach (var item in dupMINDQRCode)
                {
                    msgDupQR += MyUtility.Convert.GetString(this.CurrentMaintain["Invno"]) + "," + item.MINDQRCode + "\r\n";
                }

                foreach (DataRow row in qrDT.Rows)
                {
                    msgDupQR += MyUtility.Convert.GetString(row["Invno"]) + "," + MyUtility.Convert.GetString(row["MINDQRCode"]) + "\r\n";
                }

                MyUtility.Msg.WarningBox(msgDupQR);
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

            #region 根據 WK 資料找出 MINDQRCode
            if (!MyUtility.Check.Empty(this.CurrentMaintain["InvNo"]))
            {
                string poIDs = this.DetailDatas.Select(s => $"'{s["POID"]}'").Distinct().JoinToString(",");
                string sqlGetQRCode = $@"
select	[POID] = pl.POID,
		[Seq1] = pl.Seq1,
		[Seq2] = pll.Line,
		Roll = convert(varchar(8), pll.PackageNo),
		Dyelot = convert(varchar(8), pll.BatchNo),
		pll.QRCode
from  Poshippinglist pl with (nolock) 
inner join POShippingList_Line pll WITH (NOLOCK) ON  pll.POShippingList_Ukey = pl.Ukey
where pl.POID in ({poIDs}) and pll.QRCode <> ''
";

                DataTable dtQRCode;
                dualResult = DBProxy.Current.Select(null, sqlGetQRCode, out dtQRCode);
                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                    return false;
                }

                var emptyQRcode = this.DetailDatas.Where(s => MyUtility.Check.Empty(s["MINDQRCode"]));

                if (emptyQRcode.Any() && dtQRCode.Rows.Count > 0)
                {
                    foreach (DataRow drEmptyQRcode in emptyQRcode)
                    {
                        var checkResult = dtQRCode.AsEnumerable()
                             .Where(s => s["POID"].ToString().Trim() == drEmptyQRcode["POID"].ToString() &&
                                         s["Seq1"].ToString().Trim() == drEmptyQRcode["Seq1"].ToString() &&
                                         s["Seq2"].ToString().Trim() == drEmptyQRcode["Seq2"].ToString() &&
                                         s["Roll"].ToString().Trim() == drEmptyQRcode["Roll"].ToString() &&
                                         s["Dyelot"].ToString().Trim() == drEmptyQRcode["Dyelot"].ToString());

                        if (checkResult.Any())
                        {
                            drEmptyQRcode["MINDQRCode"] = checkResult.First()["QRCode"];
                        }
                    }
                }
            }
            #endregion

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
                string sqlcmd = $@"select portarrival, docarrival, Blno from dbo.export WITH (NOLOCK) where id='{this.CurrentMaintain["exportid"]}'";
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

                    this.txtBLAWB.Text = dr["Blno"].ToString();
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
                        if (!MyUtility.Check.Empty(this.CurrentDetailData["seq"]))
                        {
                            string sqlmcd = string.Format(Prgs.SelePoItemSqlCmd() + @"and psd.seq1 ='{2}' and psd.seq2 = '{3}' and left(psd.seq1, 1) !='7'", newvalue, Env.User.Keyword, this.CurrentDetailData["seq1"], this.CurrentDetailData["seq2"]);
                            if (!MyUtility.Check.Seek(sqlmcd, out DataRow dr_reload))
                            {
                                MyUtility.Msg.WarningBox("Data not found!", "SP & Seq");
                                this.CurrentDetailData["seq"] = string.Empty;
                                this.CurrentDetailData["seq1"] = string.Empty;
                                this.CurrentDetailData["seq2"] = string.Empty;
                                this.CurrentDetailData["pounit"] = string.Empty;
                                this.CurrentDetailData["stockunit"] = string.Empty;
                                this.CurrentDetailData["fabrictype"] = string.Empty;
                                this.CurrentDetailData["shipqty"] = 0m;
                                this.CurrentDetailData["Actualqty"] = 0m;
                                this.CurrentDetailData["PoidSeq1"] = string.Empty;
                                this.CurrentDetailData["PoidSeq"] = string.Empty;
                                this.CurrentDetailData["Refno"] = string.Empty;
                                this.CurrentDetailData["ColorID"] = string.Empty;
                            }
                            else
                            {
                                sqlmcd = $@"select StockUnit from PO_Supp_Detail where ID = '{newvalue}' and SEQ1 = '{this.CurrentDetailData["seq1"]}' and SEQ2 = '{this.CurrentDetailData["seq2"]}' ";
                                bool unti_result = MyUtility.Check.Seek(sqlmcd, out DataRow dr_StockUnit, null);
                                this.CurrentDetailData["stockunit"] = unti_result ? dr_StockUnit["stockunit"] : dr_reload["stockunit"];
                                this.CurrentDetailData["pounit"] = dr_reload["pounit"];
                                this.CurrentDetailData["fabrictype"] = dr_reload["fabrictype"];
                                this.CurrentDetailData["Refno"] = dr_reload["Refno"];
                                this.CurrentDetailData["ColorID"] = dr_reload["WH_P07_Color"];
                                this.CurrentDetailData["MtlTypeID"] = dr_reload["MtlTypeID"];
                            }
                        }

                        string sqlorders = string.Format("select category,FactoryID,OrderTypeID from View_WH_Orders WITH (NOLOCK) where id='{0}'", e.FormattedValue);
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
                        Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(this.CurrentDetailData["poid"].ToString(), this.CurrentDetailData["seq"].ToString(), "left(psd.seq1,1) !='7'");
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
        , psd.pounit
        , StockUnit = psd.StockUnit
        , M.OutQty
        , M.AdjustQty
        , M.ReturnQty
        , BalanceQty = M.inqty - M.OutQty + M.AdjustQty - M.ReturnQty
        , M.LInvQty
        , psd.fabrictype
        , e.seq1
        , e.seq2
        , f.MtlTypeID
from dbo.Export_Detail e WITH (NOLOCK) 
left join dbo.PO_Supp_Detail psd WITH (NOLOCK) on e.PoID = psd.ID and e.Seq1 = psd.SEQ1 and e.Seq2 = psd.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left JOIN MDivisionPoDetail M WITH (NOLOCK) ON E.PoID = M.POID 
                                                and e.Seq1 = M.SEQ1 
                                                and e.Seq2 = M.seq2 
LEFT JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo=f.SCIRefNo
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null,dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, '')),psd.SuppColor)
		 ELSE dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, ''))
	 END
)Color
where   e.PoID ='{this.CurrentDetailData["poid"]}' 
        and e.id = '{this.CurrentMaintain["exportid"]}'
and psd.Junk=0
Order By e.Seq1, e.Seq2, e.Refno";

                        DBProxy.Current.Select(null, sqlcmd, out DataTable poitems);

                        string columns = "Seq,refno,description,colorid,eta,inqty,stockunit,outqty,adjustqty,ReturnQty,BalanceQty,linvqty";
                        string heasercap = "Seq,Ref#,Description,Color,ETA,In Qty,Stock Unit,Out Qty,Adqty,Return,Balance,Inventory Qty";
                        string columnwidths = "6,15,25,8,10,6,6,6,6,6,6,6";
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
                    this.CurrentDetailData["MtlTypeID"] = x[0]["MtlTypeID"];

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

                        string sqlmcd = string.Format(Prgs.SelePoItemSqlCmd() + @"and psd.seq1 ='{2}' and psd.seq2 = '{3}' and left(psd.seq1, 1) !='7'", this.CurrentDetailData["poid"], Env.User.Keyword, seq[0], seq[1]);
                        if (!MyUtility.Check.Seek(sqlmcd, out DataRow dr))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }
                        else
                        {
                            sqlmcd = $@"select StockUnit from PO_Supp_Detail where ID = '{this.CurrentDetailData["poid"]}' and SEQ1 = '{seq[0]}' and SEQ2 = '{seq[1]}' ";
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
                            this.CurrentDetailData["MtlTypeID"] = dr["MtlTypeID"];

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
                        this.CurrentDetailData["stockqty"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(e.FormattedValue) * MyUtility.Convert.GetDecimal(rate), 2);
                    }
                }

                this.Change_Color();
            };

            #endregion In Qty Valid

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
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CheckBox("IsSelect", header: string.Empty, width: Widths.AnsiChars(1), iseditable: true, trueValue: true, falseValue: false)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(11), settings: ts4).Get(out cbb_poid) // 1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts).Get(out cbb_Seq) // 2
            .ComboBox("fabrictype", header: "Material" + Environment.NewLine + "Type", width: Widths.AnsiChars(9), iseditable: false).Get(out cbb_fabrictype) // 3
            .Text("NameEN", header: "Order Company", width: Widths.AnsiChars(25), iseditingreadonly: true)
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
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            .Text("remark", header: "Remark", iseditingreadonly: false)
            .Text("RefNo", header: "Ref#", iseditingreadonly: true) // 17
            .Text("ColorID", header: "Color", iseditingreadonly: true) // 18
            .Text("FactoryID", header: "Prod. Factory", iseditingreadonly: true) // 19
            .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(15), iseditingreadonly: true) // 20
            .Text("ContainerType", header: "ContainerType & No", width: Widths.AnsiChars(15), iseditingreadonly: true) // 21
            .Text("MINDQRCode", header: "MIND QR Code", width: Widths.AnsiChars(30))
            .Text("MINDChecker", header: "Checker", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .DateTime("CheckDate", header: "Check Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("FullRoll", header: "Full Roll", width: Widths.AnsiChars(25))
            .Text("FullDyelot", header: "Full Dyelot", width: Widths.AnsiChars(25))
            ;
            this.detailgrid.Columns["MINDQRCode"].DefaultCellStyle.BackColor = Color.Pink;

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;

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
                    string rate = MyUtility.GetValue.Lookup($@"
select RateValue from dbo.View_Unitrate v where v.FROM_U ='{this.CurrentDetailData["pounit"]}' and v.TO_U='{this.CurrentDetailData["stockunit"]}'");
                    this.CurrentDetailData["stockqty"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(ship_qty) * MyUtility.Convert.GetDecimal(rate), 2);
                    this.CurrentDetailData["shipqty"] = ship_qty;
                    this.CurrentDetailData.EndEdit();
                }
            }

            this.Change_Color();
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

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
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

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            string sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.StockQty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        , d.Dyelot
from dbo.Receiving_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0) + d.StockQty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than stock qty: {tmp["stockqty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion 檢查負數庫存

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
            #endregion 更新庫存數量  ftyinventory

            #region Base on wkno 收料時，需回寫export
            string sqlcmd4 = string.Format(
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
where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["exportid"],
                this.CurrentMaintain["id"]);
            #endregion

            #region 更新FIR,AIR資料

            List<SqlParameter> fir_Air_Proce = new List<SqlParameter>
            {
                new SqlParameter("@ID", this.CurrentMaintain["ID"]),
                new SqlParameter("@LoginID", Env.User.UserID),
            };

            if (!(result = DBProxy.Current.Select(string.Empty, " exec dbo.insert_Air_Fir @ID,@LoginID", fir_Air_Proce, out DataTable[] airfirids)))
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
and ID NOT IN(select ID from FIR_Laboratory)
";
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
WHERE a.id in ({airDeleteIDs})
and NOT EXISTS(
    select 1 from AIR_Laboratory b
    where a.ID = b.ID AND a.POID=b.POID AND a.Seq1=b.Seq1 AND a.Seq2=b.Seq2    
)
";
                }

                result = DBProxy.Current.Execute(null, cmd);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion

            #region 檢查MINDQRCode是否有在其他單子重複，有重複就update成空白, where 拆開來是因為效能(有index但有時候無效)
            string sqlCheckMINDQRCode = $@"
update rd set rd.MINDQRCode = ''
from Receiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from Receiving_Detail rd2 with (nolock) where rd2.ID <> rd.ID and rd2.MINDQRCode = rd.MINDQRCode)

update rd set rd.MINDQRCode = ''
from Receiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where wht.Action = 'Confirm' and [Function] = 'P07' and wht.TransactionID <> rd.ID and wht.To_NewBarcode = rd.MINDQRCode)
update rd set rd.MINDQRCode = ''
from Receiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P07' and wht.From_OldBarcode = rd.MINDQRCode)
update rd set rd.MINDQRCode = ''
from Receiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P07' and wht.From_NewBarcode = rd.MINDQRCode)
update rd set rd.MINDQRCode = ''
from Receiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P07' and wht.To_OldBarcode = rd.MINDQRCode)
update rd set rd.MINDQRCode = ''
from Receiving_Detail rd
where ID = '{this.CurrentMaintain["ID"]}'
and exists(select 1 from WHBarcodeTransaction wht with (nolock) where [Function] != 'P07' and wht.To_NewBarcode = rd.MINDQRCode)
";
            #endregion
            DBProxy.Current.DefaultTimeout = 900;  // 加長時間為15分鐘，避免timeout
            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 15, 0)))
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        /*
                         * 先更新 FtyInventory 後更新 MDivisionPoDetail
                         * 所有 MDivisionPoDetail 資料都在 Transaction 中更新
                         * 因為要在同一 SqlConnection 之下執行
                         */
                        // FtyInventory 庫存
                        string upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true, mtlAutoLock);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out DataTable resulttb, "#TmpSource", conn: sqlConn)))
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

                        if (!(result = DBProxy.Current.Execute(null, $"update Receiving set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        if (!MyUtility.Check.Empty(this.CurrentMaintain["exportid"]))
                        {
                            if (!(result = DBProxy.Current.Execute(null, sqlcmd4)))
                            {
                                throw result.GetException();
                            }
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            DBProxy.Current.DefaultTimeout = 300;  // 恢復時間為5分鐘
            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);

            #region FD, RR, LR 寄信 (QKPI - Exception)

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@mainID", this.CurrentMaintain["ID"]),
            };

            string sqlcmdQKPI = @"
declare @ID varchar(15) = @mainID

select * from (

    -- FD
    select distinct
    o.FtyGroup
    , Style = s.ID
    ,[Brand] = s.BrandID
    ,[Season] = s.SeasonID
    ,[Remark] = REPLACE('The FD was failed due to {Style.ExceptionFormRemark}.','{Style.ExceptionFormRemark}',s.ExpectionFormRemark)
    from Style s 
    inner join Orders o on s.Ukey = o.StyleUkey
    inner join Receiving_Detail rd on rd.PoId = o.ID
    where s.ExpectionFormStatus in ('A','R')
    and rd.Id = @ID

    union 
    -- RR
    select distinct
    o.FtyGroup
    ,[Style] = s.ID
    ,[Brand] = s.BrandID
    ,[Season] = s.SeasonID
    ,[Remark] = Replace(REPLACE('The RR under {Refno} was failed due to {RR Remark}.','{Refno}',sr.Refno),'{RR Remark}',sr.RRRemark)
    from Receiving_Detail rd
    inner join PO_Supp_Detail psd on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
    inner join PO_Supp p1 on p1.ID = psd.ID and psd.SEQ1 = p1.SEQ1
    inner join Orders o on psd.ID = o.ID
    inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
    inner join Style s on o.StyleUkey = s.Ukey
    inner join Style_RRLR_Report sr on sr.StyleUkey = s.Ukey and sr.SuppID = p1.SuppID and sr.Refno = psd.Refno and sr.ColorID = isnull(psdsC.SpecValue, '')
    where sr.RR= 1
    and rd.Id=@ID


    union 

    -- LR 
    select distinct
    o.FtyGroup
    ,[Style] = s.ID
    ,[Brand] = s.BrandID
    ,[Season] = s.SeasonID
    ,[Remark] = Replace('The LR under {Refno} was failed.','{Refno}',sr.Refno)
    from Receiving_Detail rd
    inner join PO_Supp_Detail psd on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
    inner join PO_Supp p1 on p1.ID = psd.ID and psd.SEQ1 = p1.SEQ1
    inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
    inner join Orders o on psd.ID = o.ID
    inner join Style s on o.StyleUkey = s.Ukey
    inner join Style_RRLR_Report sr on sr.StyleUkey = s.Ukey and sr.SuppID = p1.SuppID and sr.Refno = psd.Refno and sr.ColorID = isnull(psdsC.SpecValue, '')
    where rd.Id=@ID
    and sr.LR= 1

) a
order by Style,Brand,Season,Remark

select * from MailGroup where Code= '104'

select id,Description,ToAddress,CcAddress
,[Subject] = REPLACE(Subject,'{0}',@ID)
,Content
from MailTo where ID ='104'

";
            DualResult result1;
            if (!(result1 = DBProxy.Current.Select(null, sqlcmdQKPI, sqlParameters, out DataTable[] dts)))
            {
                this.ShowErr(result1);
                return;
            }

            DataTable dtAll = dts[0];
            DataTable dtMailGroup = dts[1];
            DataTable dtMailTo = dts[2];
            foreach (DataRow drGroup in dtMailGroup.Rows)
            {
                string toAddress = string.Empty;
                string ccAddress = string.Empty;
                bool dataExists = false;
                string content = Class.MailTools.HtmlStyle() + Environment.NewLine + "<h3>Please do further actions.</h4>" + Environment.NewLine;

                // FD
                DataRow[] filterFD = dtAll.Select($"FtyGroup = '{drGroup["FactoryID"]}'");
                if (filterFD.Length > 0)
                {
                    DataTable dt = filterFD.CopyToDataTable();
                    dt.Columns.Remove("FtyGroup");
                    content += Class.MailTools.DataTableChangeHtml_WithOutStyleHtml(dt);
                    dataExists = true;
                }

                if (dataExists && MyUtility.Check.Empty(drGroup["ToAddress"]) == false)
                {
                    toAddress += drGroup["ToAddress"].ToString() + ";" + dtMailTo.Rows[0]["ToAddress"];
                    ccAddress += drGroup["CCAddress"].ToString() + ";" + dtMailTo.Rows[0]["CcAddress"];

                    SendMail_Request request = new SendMail_Request()
                    {
                        To = toAddress,
                        CC = ccAddress,
                        Subject = dtMailTo.Rows[0]["Subject"].ToString(),
                        Body = "<html>" + Environment.NewLine + content + Environment.NewLine + "</html>",
                    };

                    SendMail(request);
                }
            }

            #endregion

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

            #region 檢查負數庫存
            string sqlcmd = string.Format(
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll
        , d.StockQty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        , d.Dyelot
from dbo.Receiving_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on   d.PoId = f.PoId
                                            and d.Seq1 = f.Seq1
                                            and d.Seq2 = f.seq2
                                            and d.StockType = f.StockType
                                            and d.Roll = f.Roll
                                            and d.Dyelot = f.Dyelot
where   (isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0) - d.StockQty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than stock qty: {tmp["stockqty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion 檢查負數庫存

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "Receiving_Detail"))
            {
                return;
            }
            #endregion

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
            #endregion

            string sqlcmd4 = string.Format(
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
END", Env.User.UserID,
                this.CurrentMaintain["exportid"],
                this.CurrentMaintain["id"]);

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 15, 0)))
            {
                try
                {
                    string deleteFIR_Shadebone = $@"
delete fs
from Receiving_Detail sd with(nolock)
inner join PO_Supp_Detail psd with(nolock) on psd.ID = sd.PoId and psd.SEQ1 = sd.Seq1 and psd.SEQ2 = sd.Seq2
inner join FIR f with (nolock) on sd.id = f.ReceivingID and sd.PoId = F.POID and sd.Seq1 = F.SEQ1 and sd.Seq2 = F.SEQ2
inner join FIR_Shadebone fs with (nolock) on f.id = fs.ID
where sd.id = '{this.CurrentMaintain["ID"]}'
";
                    if (!(result = DBProxy.Current.Execute(null, deleteFIR_Shadebone)))
                    {
                        throw result.GetException();
                    }

                    DataTable resulttb;
                    #region MdivisionPoDetail
                    string upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false);
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (data_MD_8F.Count > 0)
                    {
                        string upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F, out resulttb, "#TmpSource")))
                        {
                            throw result.GetException();
                        }
                    }
                    #endregion

                    // FtyInventory 庫存
                    string upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $@"update Receiving set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!MyUtility.Check.Empty(this.CurrentMaintain["exportid"]))
                    {
                        if (!(result = DBProxy.Current.Execute(null, sqlcmd4)))
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

            this.DetailSelectCommand = string.Format(
                @"
select  a.id
        , a.PoId
        , a.Seq1
        , a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Poid + concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as PoidSeq
        , a.Poid + Ltrim(Rtrim(a.seq1)) as PoidSeq1
        , (select p1.FabricType from PO_Supp_Detail p1 WITH (NOLOCK) where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as fabrictype
        , [RefNo]=psd.RefNo
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
        , a.ContainerCode
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
        ,[MINDQRCode] = case when b.Status = 'New' then a.MINDQRCode
                             when b.Status = 'Confirmed' and a.MINDQRCode <> '' then a.MINDQRCode
                             else ( select top 1 case  when    wbt.To_NewBarcodeSeq = '' then wbt.To_NewBarcode
                                                       when    wbt.To_NewBarcode = ''  then ''
                                                       else    Concat(wbt.To_NewBarcode, '-', wbt.To_NewBarcodeSeq)    end
                                    from   WHBarcodeTransaction wbt with (nolock)
                                    where  wbt.TransactionUkey = a.Ukey and
                                           wbt.Action = 'Confirm'
                                    order by wbt.CommitTime desc) end
        ,MINDChecker = a.MINDChecker+'-'+(select name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = a.MINDChecker)
        ,CheckDate= IIF(a.MINDCheckEditDate IS NULL, a.MINDCheckAddDate,a.MINDCheckEditDate)
        ,a.FullRoll
        ,a.FullDyelot
        ,a.CompleteTime
        ,a.SentToWMS 
        ,f.MtlTypeID
        ,a.ExportDetailUkey
        ,Company.NameEN
from dbo.Receiving_Detail a WITH (NOLOCK) 
INNER JOIN Receiving b WITH (NOLOCK) ON a.id= b.Id
left join View_WH_Orders o WITH (NOLOCK) on o.id = a.PoId
left join Orders on Orders.ID = o.ID
left join Company on Company.ID = Orders.OrderCompanyID
LEFT JOIN PO_Supp_Detail psd  WITH (NOLOCK) ON psd.ID=a.PoId AND psd.SEQ1=a.Seq1 AND psd.SEQ2 = a.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
LEFT JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo=f.SCIRefNo
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
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null, dbo.GetColorMultipleID(o.BrandID,isnull(psdsC.SpecValue, '')), psd.SuppColor)
		 ELSE dbo.GetColorMultipleID(o.BrandID,isnull(psdsC.SpecValue, ''))
	 END
)Color
OUTER APPLY(
	SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
		FROM Export_ShipAdvice_Container esc
        WHERE esc.Export_DetailUkey = a.ExportDetailUkey
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
select *
into #tmp
from (
	select
		a.Ukey
		, a.id
		, a.poid
		, a.seq1
		, a.seq2
		, a.UnitId
		, Weight = isnull(pll.GW, isnull(edc.WeightKg, a.WeightKg))
		, ActualWeight = isnull(pll.GW, isnull(edc.NetKg, a.NetKg))
		, stocktype = iif(c.category='M','I','B')
		, psd.POUnit 
		, StockUnit = psd.StockUnit
		, psd.FabricType
		, seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
		, stockqty = round(x.qty * v.RateValue,2)
		, shipqty = x.qty
		, Actualqty = x.qty
		, Roll = convert(varchar(8), isnull(pll.PackageNo, isnull(edc.Carton, '')))
		, FullRoll = convert(varchar(50), isnull(pll.PackageNo, isnull(edc.Carton, '')))
		, Dyelot = convert(varchar(8), isnull(pll.BatchNo, isnull(edc.LotNo, '')))
		, FullDyelot = convert(varchar(50), isnull(pll.BatchNo, isnull(edc.LotNo, '')))
		, remark = ''
		, location = ''
		, psd.Refno
		, [ColorID] = Color.Value
		, c.FactoryID
		, c.OrderTypeID
		, [ContainerType] = Container.Val
		, [QRCode] = pll.QRCode -- psd.FabricType = 'F'  相同 QRCode 其中一筆為 [+] 剩下為 [-]
		, [MINDQRCode] = iif(isnull(pll.BatchNo, '') = '' or isnull(pll.PackageNo, '') = '', '', pll.QRCode)
		, clickInsert = 1
		, DRQ = IIF(isnull(pll.QRCode, '') = '', Null, DENSE_RANK() over(order by pll.QRCode))
        , Fabric.MtlTypeID
        , ExportDetailUkey = a.Ukey
	from dbo.Export_Detail a WITH (NOLOCK) 
	inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on a.PoID= psd.id   
													 and a.Seq1 = psd.SEQ1    
													 and a.Seq2 = psd.SEQ2
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	inner join View_WH_Orders c WITH (NOLOCK) on c.id = a.poid
	inner join View_unitrate v on v.FROM_U = psd.POUnit and v.TO_U = psd.StockUnit
	left join Fabric WITH (NOLOCK) ON psd.SCIRefNo=Fabric.SCIRefNo
	left join Export_Detail_Carton edc with (nolock) on a.Ukey = edc.Export_DetailUkey
	left join Poshippinglist pl with (nolock) on pl.POID = a.POID and pl.Seq1 = a.Seq1
	left join POShippingList_Line pll WITH (NOLOCK) ON  pll.POShippingList_Ukey = pl.Ukey and 
														pll.Line = a.Seq2 and
														pll.PackageNo = isnull(edc.Carton, pll.PackageNo) and
														pll.BatchNo = isnull(edc.LotNo, pll.BatchNo)
	outer apply(select qty = isnull(pll.ShipQty + pll.FOC, isnull(edc.Qty + edc.Foc, a.Qty + a.Foc)) )x
	OUTER APPLY(
		SELECT [Val] = STUFF((
			SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
			FROM Export_ShipAdvice_Container esc
			WHERE esc.Export_DetailUkey = a.Ukey
			FOR XML PATH('')
		),1,1,'')
	)Container
	OUTER APPLY(
	 SELECT [Value]=
		 CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null,dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, '')),psd.SuppColor)
			 ELSE dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, ''))
		 END
	)Color
	where psd.FabricType = 'F'
	and a.id = '{this.CurrentMaintain["exportid"]}'
	union 

	select
		a.Ukey
		, a.id
		, a.poid
		, a.seq1
		, a.seq2
		, a.UnitId
		, Weight = isnull(edc.WeightKg, a.WeightKg)
		, ActualWeight = isnull(edc.NetKg, a.NetKg)
		, stocktype = iif(c.category='M','I','B')
		, psd.POUnit 
		, StockUnit = psd.StockUnit
		, psd.FabricType
		, seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
		, stockqty = round(x.qty * v.RateValue,2)
		, shipqty = x.qty
		, Actualqty = x.qty
		, Roll = ''
		, FullRoll = ''
		, Dyelot = ''
		, FullDyelot = ''
		, remark = ''
		, location = ''
		, psd.Refno
		, [ColorID] = Color.Value
		, c.FactoryID
		, c.OrderTypeID
		, [ContainerType] = Container.Val
		, [QRCode] = ''
		, [MINDQRCode] = ''
		, clickInsert = 1
		, DRQ = ''
        , Fabric.MtlTypeID
        , ExportDetailUkey = a.Ukey
	from dbo.Export_Detail a WITH (NOLOCK) 
	inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on a.PoID= psd.id   
													 and a.Seq1 = psd.SEQ1    
													 and a.Seq2 = psd.SEQ2
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	inner join View_WH_Orders c WITH (NOLOCK) on c.id = a.poid
	inner join View_unitrate v on v.FROM_U = psd.POUnit and v.TO_U = psd.StockUnit
	left join Fabric WITH (NOLOCK) ON psd.SCIRefNo=Fabric.SCIRefNo
	outer apply (
		select edc.Export_DetailUkey, edc.Id, edc.PoID, edc.Seq1, edc.Seq2
			, Qty = SUM(edc.Qty)
			, Foc = SUM(edc.Foc)
			, NetKg = SUM(edc.NetKg)
			, WeightKg = SUM(edc.WeightKg)
		from Export_Detail_Carton edc
		where Export_DetailUkey = a.Ukey
		group by edc.Export_DetailUkey, edc.Id, edc.PoID, edc.Seq1, edc.Seq2
	) edc
	outer apply(select qty = isnull(edc.Qty + edc.Foc, a.Qty + a.Foc))x
	OUTER APPLY(
		SELECT [Val] = STUFF((
			SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
			FROM Export_ShipAdvice_Container esc
			WHERE esc.Export_DetailUkey = a.Ukey
			FOR XML PATH('')
		),1,1,'')
	)Container
	OUTER APPLY(
	 SELECT [Value]=
		 CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null,dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, '')),psd.SuppColor)
			 ELSE dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, ''))
		 END
	)Color
	where psd.FabricType = 'A'
	and a.id = '{this.CurrentMaintain["exportid"]}'
)a
order by a.Ukey

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
                case "No QR Code":
                    listCanDeleteDetail = this.DetailDatas.Where(s => MyUtility.Check.Empty(s["MINDQRCode"])).ToList();
                    break;
                case "Threads":
                    listCanDeleteDetail = this.DetailDatas.Where(s => s["MtlTypeID"].ToString() == "EMB THREAD" || s["MtlTypeID"].ToString() == "SP THREAD" || s["MtlTypeID"].ToString() == "THREAD").ToList();
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
                case 3:
                    this.detailgridbs.Filter = "MINDQRCode = '' or MINDQRCode is null";
                    this.btnDeleteAll.Text = "Delete All No QR Code";
                    break;
                case 4:
                    this.detailgridbs.Filter = "MtlTypeID  in ('EMB THREAD', 'SP THREAD', 'THREAD')";
                    this.btnDeleteAll.Text = "Delete All Threads";
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
            bool isEncodeSeq = this.radioEncodeSeq.Checked;
            this.GridSortBy(isEncodeSeq);
            this.Change_record();
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

            P07_Print p = new P07_Print(poidList, this.CurrentMaintain)
            {
                CurrentDataRow = this.CurrentMaintain,
            };

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

            try
            {
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        string roll = dr["Roll", DataRowVersion.Original].ToString();
                        string dyelot = dr["Dyelot", DataRowVersion.Original].ToString();
                        long receiving_DetailUkey = MyUtility.Convert.GetLong(dr["Ukey", DataRowVersion.Original]);

                        // Ukey = 0 表示沒有存進資料庫過，直接做下一筆
                        if (MyUtility.Check.Empty(receiving_DetailUkey))
                        {
                            continue;
                        }

                        // 判斷FabricType = F，才能刪除FIR_Shadebone
                        string sqlCmd = $@"
SELECT  f.ID
FROM Receiving_Detail r
INNER JOIN PO_Supp_Detail p with (nolock) ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2
INNER JOIN FIR f with (nolock) on f.ReceivingID = r.ID AND f.POID = r.PoId AND f.SEQ1 = r.Seq1 AND f.SEQ2 = r.Seq2 
WHERE r.Ukey = '{receiving_DetailUkey}' and p.FabricType='F'
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

                result = base.ClickSave();
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
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
            bool isEncodeSeq = this.radioEncodeSeq.Checked;
            this.GridSortBy(isEncodeSeq);
            this.Change_record();
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
            newrow["MtlTypeID"] = lastRow["MtlTypeID"];

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
            if (this.detailgridbs != null && this.detailgridbs.DataSource != null)
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
                dr.EndEdit();
            }
        }

        private void ComboStockType_SelectedValueChanged(object sender, EventArgs e)
        {
            this.txtMtlLocation1.Text = string.Empty;
            this.txtMtlLocation1.StockTypeFilte = this.comboStockType.SelectedValue.ToString();
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
        }

        private void BtnPrintAccessorySticker_Click(object sender, EventArgs e)
        {
            P07_PrintAccessorySticker s = new P07_PrintAccessorySticker(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["ExportId"].ToString());
            s.ShowDialog();
        }
    }
}