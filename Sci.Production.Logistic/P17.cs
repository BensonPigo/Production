using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Packing;
using Sci.Production.Prg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class P17 : Win.Tems.QueryForm
    {
        private string upd_sql_barcode = string.Empty;
        private int intTmpNo = 0;
        private bool UseAutoScanPack = false;
        private DataTable dtHade;
        private DataTable dtDetail;
        private P09_IDX_CTRL IDX;

        /// <inheritdoc/>
        public P17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.UseAutoScanPack = MyUtility.Check.Seek("select 1 from system where UseAutoScanPack = 1");
            this.txtScanCartonSP.Focus();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.gridScanDetail.DataSource = this.scanDetailBS;
            this.Helper.Controls.Grid.Generator(this.gridScanDetail)
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(15))
                .Text("Barcode", header: "Hangtag Barcode", width: Widths.AnsiChars(15))
                .Numeric("QtyPerCTN", header: "Qty")
                .Numeric("ScanQty", header: "Scan Qty")
                ;
            this.txtDest.TextBox1.ReadOnly = true;
        }

        private void TabControlScanArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            int tabControlIndex = tabControl.SelectedIndex;

            if (tabControlIndex == 0)
            {
                this.txtScanCartonSP.Focus();
            }
            else if (tabControlIndex == 1)
            {
                this.txtScanEAN.Focus();
            }
        }

        private string GetSicCtnNo()
        {
            string strSciCtnNo = string.Empty;
            if (MyUtility.Check.Empty(this.txtScanCartonSP.Text))
            {
                this.txtScanCartonSP.Text = string.Empty;
                return string.Empty;
            }

            if (this.txtScanCartonSP.Text.Length >= 13)
            {
                int iPath = this.txtScanCartonSP.Text.Length >= 16 ? this.txtScanCartonSP.Text.Length : 13;
                string ctnStartNo = this.txtScanCartonSP.Text.Length < 16 ? this.txtScanCartonSP.Text.Substring(13, this.txtScanCartonSP.Text.Length - 13).TrimStart('^') : string.Empty;
                string sciCtnNo = this.txtScanCartonSP.Text.Substring(0, iPath);
                string sqlWhere = this.txtScanCartonSP.Text.Length < 16 ? $@"PLD.ID ='{sciCtnNo}' and pld.CTNStartNo = '{ctnStartNo}'" : $@"pld.SCICtnNo = '{sciCtnNo}'";
                strSciCtnNo = MyUtility.GetValue.Lookup($@"select SciCtnNo from PackingList_Detail pld WHERE  {sqlWhere}");
            }

            return strSciCtnNo;
        }

        private void TxtScanCartonSP_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtScanCartonSP.Text))
            {
                this.txtScanCartonSP.Text = string.Empty;
                return;
            }

            string oldValue = this.txtScanCartonSP.OldValue;

            // 檢查是否有正在掃packing未刷完
            if (!MyUtility.Check.Empty(oldValue) && this.numBoxScanQty.Value > 0)
            {
                if (MyUtility.Msg.InfoBox("Do you want to change carton No.?", buttons: MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (!this.LackingClose())
                    {
                        this.txtScanCartonSP.Text = oldValue;
                        return;
                    }
                }
            }

            //// 檢查字元
            string packListSciCtnNo = MyUtility.GetValue.Lookup($@"select 1 from PackingList_Detail pld WHERE pld.SciCtnNo ='{this.GetSicCtnNo()}'");
            if (MyUtility.Check.Empty(packListSciCtnNo))
            {
                AutoClosingMessageBox.Show($"This carton No.({this.txtScanCartonSP.Text}) does not exist.", "Warning", 3000);
                e.Cancel = true;
                this.ClearAll("ALL");
                return;
            }

            string strSQL_CFA = $@"
            SELECT 1
            FROM PackingList_Detail pld
            inner join TransferToCFA CFA on pld.id = cfa.PackingListID
							            and pld.CTNStartNo = CFA.CTNStartNo
            WHERE pld.SCICtnNo in(CFA.SCICtnNo) AND pld.SciCtnNo ='{this.GetSicCtnNo()}'";
            string cfaSciCtnNo = MyUtility.GetValue.Lookup(strSQL_CFA);

            if (MyUtility.Check.Empty(cfaSciCtnNo) && !MyUtility.Check.Empty(packListSciCtnNo))
            {
                MyUtility.Msg.InfoBox("This carton No. has not been transferred to CFA. Cannot be scan & pack.", buttons: MessageBoxButtons.OK);
                e.Cancel = true;
                this.ClearAll("ALL");
                return;
            }

            // 換箱清空更新barcode字串
            this.upd_sql_barcode = string.Empty;
            this.intTmpNo = 0;

            this.LoadingData(this.GetSicCtnNo());

            if (this.dtDetail.Rows.Count == 0)
            {
                AutoClosingMessageBox.Show($"This carton No.({this.txtScanCartonSP.Text}) does not exist.", "Warning", 3000);
                e.Cancel = true;
                this.ClearAll("ALL");
                return;
            }

            int cnt_selectCarton = this.LoadSelectCarton();
            if (cnt_selectCarton == 1)
            {
                // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
                if (Convert.ToInt16(this.dtDetail.Compute("Sum(ScanQty)", null)) > 0)
                {
                    if (MyUtility.Msg.InfoBox("This carton had been scanned, are you sure you want to rescan again?", buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DualResult dualResult = this.ClearScanQty(this.dtDetail.Select(), "ALL");
                        if (!dualResult)
                        {
                            this.ShowErr(dualResult);
                            return;
                        }
                    }
                    else
                    {
                        this.ClearAll("ALL");
                        e.Cancel = true;
                        return;
                    }
                }
            }

            this.Tab_Focus("EAN"); // 跳Tab跳至掃描流程
        }

        private void LoadingData(string sciCtnNo)
        {
            this.dtHade = null;
            this.dtDetail = null;
            string sqlcmd = $@"
            select distinct
            pd.ID,
            pd.CTNStartNo,
            pd.OrderID,
            o.CustPoNo,
            pd.Article,
            pd.Color,
            pd.SizeCode  ,
            pd.QtyPerCTN,
            ScanQty = pd.ClogScanQty,
            pd.ScanEditDate,
            pd.ScanName,
            pd.barcode,
            p.BrandID,
            o.StyleID,
            os.Seq,
            pd.Ukey,
            [PKseq] = pd.Seq,
            o.Dest,
            isnull(pd.ActCTNWeight,0) as ActCTNWeight
            ,p.Remark
	        ,pd.Ukey
	        ,[IsFirstTimeScan] = Cast(1 as bit)
            ,o.CustCDID
            ,pd.SciCtnNo
            ,[MDStatus] = IIF(pd.ScanPackMDDate is null, '1st MD', '2rd MD')
            from PackingList_Detail pd WITH (NOLOCK)
            inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
            inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
            left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode 
            where p.Type in ('B','L') and pd.SCICtnNo = '{sciCtnNo}'";
            DualResult result_Head = DBProxy.Current.Select(null, sqlcmd, out this.dtHade);
            if (!result_Head)
            {
                this.ShowErr(result_Head);
            }

            string sum_sql = $@"
            DECLARE @PackingID  varchar(20)
            SELECT @PackingID = ID FROM PackingList_Detail WHERE SCICtnNo = '{sciCtnNo}'                                                

            SELECT 
            TtlCartons= isnull(sum(pld.CTNQty),0)
            ,TtlQty = isnull(sum(pld.ShipQty),0)
            ,TtlPackedCartons = isnull(sum(case when pld.ClogScanQty > 0 and pld.CTNQty>0 then 1 else 0 end),0)
            ,TtlPackQty = isnull(sum(pld.ClogScanQty),0)
            FROM PackingList p
            INNER join PackingList_Detail pld with(NOLOCK) on p.id = pld.id
            inner join TransferToCFA CFA on pld.id = cfa.PackingListID
					            and pld.CTNStartNo = CFA.CTNStartNo
            WHERE 
            pld.SCICtnNo in(CFA.SCICtnNo) AND 
            p.Type in ('B','L') AND 
            pld.ID = @PackingID";
            DataRow dr_sum;
            MyUtility.Check.Seek(sum_sql, out dr_sum);

            #region 左邊 Head
            this.displayPackID.Text = MyUtility.Convert.GetString(this.dtHade.Rows[0]["ID"]);
            this.displayCtnNo.Text = MyUtility.Convert.GetString(this.dtHade.Rows[0]["CTNStartNo"]);
            this.displaySPNo.Text = MyUtility.Convert.GetString(this.dtHade.Rows[0]["OrderID"]);
            this.displayPoNo.Text = MyUtility.Convert.GetString(this.dtHade.Rows[0]["CustPoNo"]);
            this.displayBrand.Text = MyUtility.Convert.GetString(this.dtHade.Rows[0]["BrandID"]);
            this.displayStyle.Text = MyUtility.Convert.GetString(this.dtHade.Rows[0]["StyleID"]);
            this.txtDest.TextBox1.Text = MyUtility.Convert.GetString(this.dtHade.Rows[0]["Dest"]);
            #endregion 左邊 Head
            #region 右邊 Head

            this.numBoxttlCatons.Text = dr_sum["TtlCartons"].ToString();
            this.numBoxttlQty.Text = dr_sum["TtlQty"].ToString();
            this.numBoxPackedCartons.Text = dr_sum["TtlPackedCartons"].ToString();
            this.numBoxttlPackQty.Text = ((int)dr_sum["TtlPackQty"]).ToString();
            this.numBoxRemainCartons.Text = ((int)dr_sum["TtlCartons"] - (int)dr_sum["TtlPackedCartons"]).ToString();
            this.numBoxRemainQty.Text = ((int)dr_sum["TtlQty"] - (int)dr_sum["TtlPackQty"]).ToString();

            #endregion

            string sqlcmd_Detail = $@"
            SELECT
            pd.ID,
            pd.CTNStartNo,
            pd.OrderID,
            o.CustPoNo,
            pd.Article,
            pd.Color,
            pd.SizeCode  ,
            pd.QtyPerCTN,
            ScanQty = pd.ClogScanQty,
            pd.ScanEditDate,
            pd.ScanName,
            pd.barcode,
            p.BrandID,
            o.StyleID,
            os.Seq,
            pd.Ukey,
            [PKseq] = pd.Seq,
            o.Dest,
            isnull(pd.ActCTNWeight,0) as ActCTNWeight
            ,p.Remark
	        ,pd.Ukey
	        ,[IsFirstTimeScan] = Cast(1 as bit)
            ,o.CustCDID
            ,pd.SCICtnNo
            FROM PackingList p
            inner join  PackingList_Detail pd with(NOLOCK) on p.id = pd.id
            left join TransferToCFA CFA on pd.id = CFA.PackingListID 
							            and pd.CTNStartNo = CFA.CTNStartNo
            inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
            left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode 
            WHERE pd.SCICtnNo in(CFA.SCICtnNo) and p.Type in ('B','L')
            and pd.SCICtnNo = '{sciCtnNo}'";

            DualResult result_Detail = DBProxy.Current.Select(null, sqlcmd_Detail, out this.dtDetail);
            if (!result_Detail)
            {
                this.ShowErr(result_Detail);
            }

            this.scanDetailBS.DataSource = this.dtDetail;

            if (this.dtDetail != null)
            {
                this.numBoxScanTtlQty.Value = this.dtDetail.AsEnumerable().Sum(s => (int)s["QtyPerCTN"]);
                this.numBoxScanQty.Value = this.dtDetail.AsEnumerable().Sum(s => (short)s["ScanQty"]);
            }
        }

        private void Tab_Focus(string type)
        {
            if (type.Equals("EAN"))
            {
                this.tabControlScanArea.SelectedTab = this.tabControlScanArea.TabPages[1];
                this.txtScanEAN.Focus();
                this.numBoxScanQty.Value = 0;

                if (this.UseAutoScanPack)
                {
                    if (this.IDX != null)
                    {
                        this.IDX = null;
                    }

                    P09_IDX_CTRL iDX = new P09_IDX_CTRL();
                    if (iDX.IdxCall(1, "8:?", 4))
                    {
                        this.IDX = iDX;
                    }
                }

            }
            else if (type.Equals("CARTON"))
            {
                this.tabControlScanArea.SelectedTab = this.tabControlScanArea.TabPages[0];
                this.txtScanCartonSP.Focus();
            }
        }

        private bool LackingClose()
        {
            if (this.numBoxScanQty.Value != this.numBoxScanTtlQty.Value &&
                this.numBoxScanQty.Value > 0)
            {
                P18_MessageBox questionBox = new P18_MessageBox();
                DialogResult resultLacking = questionBox.ShowDialog();
                if (resultLacking == DialogResult.Yes)
                {
                    this.UpdateLackingStatus();
                    return true;
                }
                else if (resultLacking == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearAll(string type)
        {
            if (type.Equals("ALL"))
            {
                this.txtScanCartonSP.Text = string.Empty;
                this.numBoxScanQty.Text = string.Empty;
                this.numBoxScanTtlQty.Text = string.Empty;
                this.displayPackID.Text = string.Empty;
                this.displayCtnNo.Text = string.Empty;
                this.displaySPNo.Text = string.Empty;
                this.displayPoNo.Text = string.Empty;
                this.displayBrand.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
                this.txtDest.TextBox1.Text = string.Empty;
                this.numBoxttlCatons.Text = string.Empty;
                this.numBoxttlQty.Text = string.Empty;
                this.numBoxPackedCartons.Text = string.Empty;
                this.numBoxttlPackQty.Text = string.Empty;
                this.numBoxRemainCartons.Text = string.Empty;
                this.numBoxRemainQty.Text = string.Empty;
                this.scanDetailBS.DataSource = null;
            }
            else if (type.Equals("SCAN"))
            {
                this.numBoxScanQty.Text = string.Empty;
                this.numBoxScanTtlQty.Text = string.Empty;
                this.displayPackID.Text = string.Empty;
                this.displayCtnNo.Text = string.Empty;
                this.displaySPNo.Text = string.Empty;
                this.displayPoNo.Text = string.Empty;
                this.displayBrand.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
                this.numBoxttlCatons.Text = string.Empty;
                this.numBoxttlQty.Text = string.Empty;
                this.numBoxPackedCartons.Text = string.Empty;
                this.numBoxttlPackQty.Text = string.Empty;
                this.numBoxRemainCartons.Text = string.Empty;
                this.numBoxRemainQty.Text = string.Empty;
                this.txtDest.TextBox1.Text = string.Empty;
                this.scanDetailBS.DataSource = null;
            }
        }

        private void TxtScanEAN_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtScanEAN.Text))
            {
                return;
            }

            if (this.scanDetailBS.DataSource == null)
            {
                return;
            }

            if (this.IsNotInitialedIDX_CTRL())
            {
                return;
            }

            int barcode_pos = this.scanDetailBS.Find("Barcode", this.txtScanEAN.Text);

            // 沒有BarCode狀況下
            if (barcode_pos == -1)
            {
                // 如果不存在，代表一定是第一次掃描，則找出Barcode還空著的Row填進去
                int no_barcode_cnt = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Where(s => MyUtility.Check.Empty(s["Barcode"])).Count();
                if (no_barcode_cnt == 0)
                {
                    P18_Message msg = new P18_Message();

                    // 送回 沒有的barcode
                    if (this.UseAutoScanPack)
                    {
                        this.IDX.IdxCall(254, "a:" + this.txtScanEAN.Text.Trim(), ("a:" + this.txtScanEAN.Text.Trim()).Length);
                    }

                    msg.Show($"<{this.txtScanEAN.Text}> Invalid barcode !!");
                    this.txtScanEAN.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    // 有Barcode還空著的Row，若筆數大於一筆，則跳出視窗給User選填
                    DataTable no_barcode_dt = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Where(s => MyUtility.Check.Empty(s["Barcode"])).CopyToDataTable();
                    DataRow no_barcode_dr = no_barcode_dt.NewRow();
                    if (no_barcode_dt.Rows.Count > 1)
                    {
                        // 有空的barcode就開窗
                        SelectItem sele = new SelectItem(no_barcode_dt, "Article,Color,SizeCode", "8,6,8", string.Empty, headercaptions: "Colorway,Color,Size");
                        DialogResult result = sele.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            this.txtScanEAN.Text = string.Empty;
                            e.Cancel = true;
                            if (this.UseAutoScanPack)
                            {
                                this.IDX.IdxCall(254, "a:" + this.txtScanEAN.Text.Trim(), ("a:" + this.txtScanEAN.Text.Trim()).Length);
                            }

                            return;
                        }

                        no_barcode_dr = sele.GetSelecteds()[0];
                    }
                    else
                    {
                        no_barcode_dr = no_barcode_dt.Rows[0];
                    }

                    this.upd_sql_barcode += this.Update_barcodestring(no_barcode_dr);
                    foreach (DataRow dr in ((DataTable)this.scanDetailBS.DataSource).Rows)
                    {
                        if (dr["Article"].Equals(no_barcode_dr["Article"]) && dr["SizeCode"].Equals(no_barcode_dr["SizeCode"]))
                        {
                            dr["Barcode"] = this.txtScanEAN.Text;

                           // dr["ScanQty"] = (short)dr["ScanQty"] + 1;

                            // 變更是否為第一次掃描的標記
                            dr["IsFirstTimeScan"] = false;
                            this.UpdScanQty((long)dr["Ukey"], (string)dr["Barcode"]);
                            break;
                        }
                    }

                    if (this.UseAutoScanPack)
                    {
                        this.IDX.IdxCall(254, "A:" + this.txtScanEAN.Text.Trim() + "=" + no_barcode_dr["QtyPerCtn"].ToString().Trim(), ("A:" + this.txtScanEAN.Text.Trim() + "=" + no_barcode_dr["QtyPerCtn"].ToString().Trim()).Length);
                    }
                }
            }
            else
            {
                this.scanDetailBS.Position = barcode_pos;
                DataRowView cur_dr = (DataRowView)this.scanDetailBS.Current;
                int scanQty = (short)cur_dr["ScanQty"];
                int qtyPerCTN = (int)cur_dr["QtyPerCTN"];

                // 判斷該Barcode是否為第一次掃描，是的話傳送指令避免停下
                bool isFirstTimeScan = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Where(s => MyUtility.Convert.GetString(s["Barcode"]) == this.txtScanEAN.Text.Trim() && MyUtility.Convert.GetBool(s["IsFirstTimeScan"])).Any();

                if (isFirstTimeScan && this.UseAutoScanPack)
                {
                    this.IDX.IdxCall(254, "A:" + this.txtScanEAN.Text.Trim() + "=" + cur_dr["QtyPerCtn"].ToString().Trim(), ("A:" + this.txtScanEAN.Text.Trim() + "=" + cur_dr["QtyPerCtn"].ToString().Trim()).Length);

                    // 變更是否為第一次掃描的標記
                    cur_dr["IsFirstTimeScan"] = false;
                }

                if (scanQty >= qtyPerCTN)
                {
                    // 此barcode已足夠,或超過 送回
                    if (this.UseAutoScanPack)
                    {
                        this.IDX.IdxCall(254, "a:" + this.txtScanEAN.Text.Trim(), ("a:" + this.txtScanEAN.Text.Trim()).Length);
                    }

                    AutoClosingMessageBox.Show($"This Size scan is complete,can not scan again!!", "Warning", 3000);
                    this.txtScanEAN.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    // cur_dr["ScanQty"] = (short)cur_dr["ScanQty"] + 1;
                    this.UpdScanQty((long)cur_dr["Ukey"]);
                }
            }

            // this.scanDetailBS.ResetCurrentItem();
            this.scanDetailBS.ResetBindings(true);

            // 計算scanQty
            this.numBoxScanQty.Value = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Sum(s => (short)s["ScanQty"]);
            this.txtScanEAN.Text = string.Empty;

            if (this.numBoxScanQty.Value == this.numBoxScanTtlQty.Value)
            {
                if (!MyUtility.Check.Empty(this.upd_sql_barcode))
                {
                    DataTable dtUpdateID;
                    DualResult sql_result = DBProxy.Current.Select(null, this.upd_sql_barcode, out dtUpdateID);
                    if (!sql_result)
                    {
                        this.ShowErr(sql_result);
                        return;
                    }
                }

                string upd_sql = $@"
                update PackingList_Detail 
                set ClogScanQty = QtyPerCTN 
                , ClogScanDate = GETDATE()
                , ScanName = '{Env.User.UserID}'   
                , ClogLackingQty = 0
                where id = '{MyUtility.Convert.GetString(this.dtHade.Rows[0]["ID"])}' 
                and CTNStartNo = '{MyUtility.Convert.GetString(this.dtHade.Rows[0]["CTNStartNo"])}' 

                ";
                DualResult sql_result1 = DBProxy.Current.Execute(null, upd_sql);
                if (!sql_result1)
                {
                    this.ShowErr(sql_result1);
                    return;
                }

                this.InsertClog();
                this.AfterCompleteScanCarton();
                this.Tab_Focus("CARTON");
            }

            e.Cancel = true;
        }

        private void UpdScanQty(long ukey, string barcode = "")
        {
            foreach (DataRow dr in this.dtDetail.Rows)
            {
                if (dr["Ukey"].Equals(ukey))
                {
                    dr["ScanQty"] = (short)dr["ScanQty"] + 1;
                    if (MyUtility.Check.Empty(dr["Barcode"]) && !MyUtility.Check.Empty(barcode))
                    {
                        dr["Barcode"] = barcode;
                    }
                }
            }
        }

        private string Update_barcodestring(DataRow no_barcode_dr)
        {
            if (!MyUtility.Check.Empty(this.upd_sql_barcode))
            {
                this.intTmpNo += 1;
            }

            return $@"
            --先將需要update的key取出，避免update過久lock整個table
            select distinct a.Article,a.Color,a.SizeCode,o.StyleUkey
            into #tmpNeedUpdateGroup{this.intTmpNo}
            from PackingList_Detail a
            inner join Orders o ON o.ID = a.OrderID
            where   a.ID ='{MyUtility.Convert.GetString(this.dtHade.Rows[0]["ID"])}'
                    AND a.CTNStartNo =  '{MyUtility.Convert.GetString(this.dtHade.Rows[0]["CTNStartNo"])}'
                    AND a.Article =  '{no_barcode_dr["Article"]}'
                    and a.SizeCode=  '{no_barcode_dr["SizeCode"]}'

            select  pd.Ukey
            into #tmpNeedUpdPackUkeys{this.intTmpNo}
            from PackingList_Detail pd with (nolock)
            inner join Orders o  with (nolock) ON o.ID = pd.OrderID
            where exists(select 1 from #tmpNeedUpdateGroup{this.intTmpNo} t 
                                  where t.Article = pd.Article     and
                                        t.Color = pd.Color         and
                                        t.SizeCode = pd.SizeCode   and
                                        t.StyleUkey = o.StyleUkey)

            UPDATE pd
            SET BarCode = '{this.txtScanEAN.Text}'
            from  PackingList_Detail pd
            where pd.Ukey in (select Ukey from #tmpNeedUpdPackUkeys{this.intTmpNo})

            drop table #tmpNeedUpdateGroup{this.intTmpNo}, #tmpNeedUpdPackUkeys{this.intTmpNo}

            ";
        }

        private bool IsNotInitialedIDX_CTRL()
        {
            if (this.UseAutoScanPack && this.IDX == null)
            {
                MyUtility.Msg.WarningBox("Please enter Paircode first.");
                return true;
            }

            return false;
        }

        private void InsertClog()
        {
            string insert_sql = $@"
            DECLARE @InsertUkey bigint
            INSERT INTO ClogScanPack
            (
                [MDivisionID]
                ,[PackingListID]
                ,[CTNStartNo]
                ,[SCICtnNo]
                ,[ScanQty]
                ,[LackingQty]
                ,[AddName]
                ,[AddDate]
            )
            VALUES
            (
	                '{Env.User.Keyword}'
                ,'{MyUtility.Convert.GetString(this.dtHade.Rows[0]["ID"])}'
                ,'{MyUtility.Convert.GetString(this.dtHade.Rows[0]["CTNStartNo"])}'
                ,'{MyUtility.Convert.GetString(this.dtHade.Rows[0]["SCICtnNo"])}'
                , {MyUtility.Convert.GetDecimal(this.numBoxScanQty.Value)}
                , {MyUtility.Convert.GetDecimal(this.numBoxScanTtlQty.Value) - MyUtility.Convert.GetDecimal(this.numBoxScanQty.Value)}
                , '{Env.User.UserID}'
                , GETDATE()
            ) 
            SELECT @InsertUkey = SCOPE_IDENTITY()
            ";

            foreach (DataRow row in this.dtDetail.Rows)
            {
                insert_sql += $@"
                INSERT INTO ClogScanPack_Detail
                (
                    [ClogScanPackUkey]
                  ,[OrderID]
                  ,[Article]
                  ,[SizeCode]
                  ,[ScanQty]
                  ,[LackingQty]
                )
                VALUES
                (
                    @InsertUkey
                    ,'{MyUtility.Convert.GetString(row["OrderID"])}'
                    ,'{MyUtility.Convert.GetString(row["Article"])}'
                    ,'{MyUtility.Convert.GetString(row["SizeCode"])}'
                    ,{MyUtility.Convert.GetInt(row["ScanQty"])}
                    ,{MyUtility.Convert.GetInt(row["QtyPerCTN"]) - MyUtility.Convert.GetInt(row["ScanQty"])}
                )
                ";
            }

            DualResult dual = DBProxy.Current.Execute(null, insert_sql);
            if (!dual)
            {
                this.ShowErr(dual);
            }
        }

        private void UpdateLackingStatus()
        {
            DualResult sql_result;
            DataTable dt = (DataTable)this.scanDetailBS.DataSource;
            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0)
            {
                return;
            }

            // 計算scanQty
            this.numBoxScanQty.Value = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Sum(s => (short)s["ScanQty"]);

            // 如果掃描數量> 0,則 update PackingList_Detail
            if (this.numBoxScanQty.Value > 0)
            {
                string upd_sql = @"
                Create table #tmpUpdatedID
	                (
		                ID varchar(13) null
	                )
                ";

                foreach (DataRow dr in dt.Rows)
                {
                    upd_sql += $@"
                    update PackingList_Detail 
                    set   
                     ClogScanQty = {MyUtility.Convert.GetDecimal(dr["ScanQty"])}
                    ,ClogLackingQty = {MyUtility.Convert.GetDecimal(dr["QtyPerCTN"]) - MyUtility.Convert.GetDecimal(dr["ScanQty"])} 
                    ,ClogScanDate = GETDATE()
                    ,ClogScanName = '{Env.User.UserID}'
                    output	inserted.ID
                    into #tmpUpdatedID
                    where Ukey={dr["Ukey"]}

                    ";
                }

                upd_sql += @"select distinct ID from #tmpUpdatedID
                drop table #tmpUpdatedID";
                DataTable dtUpdateID;
                sql_result = DBProxy.Current.Select(null, upd_sql, out dtUpdateID);
                if (!sql_result)
                {
                    this.ShowErr(sql_result);
                    return;
                }

                this.InsertClog();

                MyUtility.Msg.InfoBox("successfully!!");
            }
            else
            {
                // 讓遊標停留在原地
                this.txtScanEAN.Select();
            }
        }

        private void AfterCompleteScanCarton()
        {
            // 回壓DataTable

            // 檢查下方carton列表是否都掃完
            int carton_complete = this.dtDetail.AsEnumerable().Where(s => (short)s["ScanQty"] != (int)s["QtyPerCTN"]).Count();
            if (carton_complete == 0)
            {
                this.ClearAll("ALL");
            }
            else
            {
                this.ClearAll("SCAN");
                this.LoadSelectCarton();
            }
        }

        private int LoadSelectCarton()
        {
            if (this.dtDetail == null)
            {
                return 0;
            }

            var list_selectCarton = (from c in this.dtDetail.AsEnumerable()
                                                          group c by new
                                                          {
                                                              ID = c.Field<string>("ID"),
                                                              CTNStartNo = c.Field<string>("CTNStartNo"),
                                                              CustPoNo = c.Field<string>("CustPoNo"),
                                                              BrandID = c.Field<string>("BrandID"),
                                                              StyleID = c.Field<string>("StyleID"),
                                                              Dest = c.Field<string>("Dest"),
                                                              Remark = c.Field<string>("Remark"),
                                                          }).ToList();
            return list_selectCarton.Count;
        }

        private DualResult ClearScanQty(DataRow[] tmp, string clearType)
        {
            DualResult result1 = new DualResult(true);
            int oriTtlScanQty = tmp.Sum(o => Convert.ToInt32(o["ScanQty"]));

            string packingListID = tmp[0]["ID"].ToString();
            string cTNStartNo = tmp[0]["CTNStartNo"].ToString();

            if (tmp.Length == 0)
            {
                result1 = new DualResult(false, new BaseResult.MessageInfo("ClearScanQty Error"));

                return result1;
            }

            foreach (DataRow dr in tmp)
            {
                dr["ScanQty"] = 0;
            }

            if (clearType.Equals("ALL"))
            {
                string upd_sql = $@"
                UPDATE PackingList_Detail 
                SET ClogScanQty = 0
                WHERE ID = '{packingListID}' AND CTNStartNo = '{cTNStartNo}' 
";
                result1 = DBProxy.Current.Execute(null, upd_sql);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        result1 = DBProxy.Current.Execute(null, upd_sql);

                        if (result1)
                        {
                            transactionScope.Complete();
                            transactionScope.Dispose();
                        }
                        else
                        {
                            transactionScope.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                    }
                }

                // 重新從DB撈取下方Grid資料
                this.LoadingData(this.GetSicCtnNo());
                this.LoadSelectCarton();
            }

            return new DualResult(result1);
        }

        private void BtnLacking_Click(object sender, EventArgs e)
        {
            if (this.numBoxScanTtlQty.Value > 0 && this.numBoxScanTtlQty.Value != this.numBoxScanQty.Value)
            {
                if (MyUtility.Msg.InfoBox("Scan & Pack has not ended yet, \r\nare you sure you want to record lacking Qty ?", buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.UpdateLackingStatus();
                    this.ClearAll("ALL");
                    this.Tab_Focus("CARTON");
                }
                else
                {
                    this.txtScanEAN.Select();
                    return;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!this.LackingClose())
            {
                e.Cancel = true;
            }

            base.OnFormClosing(e);
        }
    }
}
