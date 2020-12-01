using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Production.Class;
using System.Linq;
using Ict.Win;
using System.Linq.Dynamic;
using Sci.Win.Tools;
using System.Transactions;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P18
    /// </summary>
    public partial class P18 : Win.Tems.QueryForm
    {
        private DataTable dt_scanDetail;
        private SelectCartonDetail selecedPK;
        private P09_IDX_CTRL IDX;
        private bool UseAutoScanPack = false;
        private string PackingListID = string.Empty;
        private string CTNStarNo = string.Empty;

        /// <summary>
        /// P18
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.UseAutoScanPack = MyUtility.Check.Seek("select 1 from system where UseAutoScanPack = 1");
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.numBoxScanQty.ForeColor = Color.Red;
            this.gridSelectCartonDetail.DataSource = this.selcartonBS;
            this.Helper.Controls.Grid.Generator(this.gridSelectCartonDetail)
                .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15))
                .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(17))
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(15))
                .Text("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(12))
                .Text("ScanQty", header: "Scanned Qty", width: Widths.AnsiChars(12))
                .Text("PassName", header: "Scanned by", width: Widths.AnsiChars(12))
                .Text("ActCTNWeight", header: "Actual CTN# Weight", width: Widths.AnsiChars(12));

            this.gridScanDetail.DataSource = this.scanDetailBS;
            this.Helper.Controls.Grid.Generator(this.gridScanDetail)
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(15))
                .Numeric("QtyPerCTN", header: "Qty")
                .Text("Barcode", header: "Hangtag Barcode", width: Widths.AnsiChars(15))
                .Numeric("ScanQty", header: "Scan Qty");
            this.Tab_Focus("CARTON");

            this.txtDest.TextBox1.ReadOnly = true;
        }

        private void TxtScanCartonSP_Validating(object sender, CancelEventArgs e)
        {
            DualResult result;
            this.PackingListID = string.Empty;
            this.CTNStarNo = string.Empty;

            if (MyUtility.Check.Empty(this.txtScanCartonSP.Text))
            {
                return;
            }

            string oldValue = this.txtScanCartonSP.OldValue;

            // 檢查是否有正在掃packing未刷完
            if (!MyUtility.Check.Empty(oldValue) && this.numBoxScanQty.Value > 0)
            {
                if (MyUtility.Msg.InfoBox("Do you want to change CTN#?", buttons: MessageBoxButtons.YesNo) == DialogResult.No)
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

            if (this.txtScanCartonSP.Text.Length > 13)
            {
                this.PackingListID = this.txtScanCartonSP.Text.Substring(0, 13);
                this.CTNStarNo = this.txtScanCartonSP.Text.Substring(13, this.txtScanCartonSP.Text.Length - 13);
            }

            this.upd_sql_barcode = string.Empty; // 換箱清空更新barcode字串
            this.ClearAll("SCAN");
            #region 檢查是否有資料，三個角度

            // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
            // 2.=Orders.ID
            // 3.=Orders.CustPoNo
            string[] aLLwhere = new string[]
            {
                this.txtScanCartonSP.Text.Length > 13 ? $" and  pd.ID = '{this.PackingListID}' and  pd.CTNStartNo = '{this.CTNStarNo}'" : " and 1=0 ",
                $" and  pd.ID = '{this.txtScanCartonSP.Text}'",
                $@" and o.ID = '{this.txtScanCartonSP.Text}' or o.CustPoNo = '{this.txtScanCartonSP.Text}'",
                $@" and pd.CustCTN = '{this.txtScanCartonSP.Text}'",
            };

            string scanDetail_sql = $@"select distinct
                                           pd.ID,
                                           pd.CTNStartNo  ,
                                           pd.OrderID,
                                           o.CustPoNo ,
                                           pd.Article    ,
                                           pd.Color,
                                           pd.SizeCode  ,
                                           pd.QtyPerCTN,
                                           ScanQty = pd.ScanQty,
                                           pd.ScanEditDate,
                                           pd.ScanName,
                                           pd.barcode,
                                           p.BrandID,
                                           o.StyleID,
                                           os.Seq,
                                           pd.Ukey,
                                           [PKseq] = pd.Seq,
                                           o.Dest,
                                           isnull(pd.ActCTNWeight,0) as ActCTNWeight, 
                                           isnull(iif(ps.name is null, convert(nvarchar(10),pd.ScanEditDate,112), ps.name+'-'+convert(nvarchar(10),pd.ScanEditDate,120)),'') as PassName
                                           ,p.Remark
										   ,pd.Ukey
										   ,[IsFirstTimeScan] = Cast(1 as bit)
                                from PackingList_Detail pd WITH (NOLOCK)
                                inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                                inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
                                left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode 
                                left join pass1 ps WITH (NOLOCK) on pd.ScanName = ps.id
                                where p.Type in ('B','L') ";

            foreach (string where in aLLwhere)
            {
                result = DBProxy.Current.Select(null, scanDetail_sql + where, out this.dt_scanDetail);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (this.dt_scanDetail.Rows.Count > 0)
                {
                    break;
                }
            }

            if (this.dt_scanDetail.Rows.Count == 0)
            {
                AutoClosingMessageBox.Show($"<{this.txtScanCartonSP.Text}> Invalid CTN#!!", "Warning", 3000);
                e.Cancel = true;
                return;
            }

            // 產生comboPKFilter資料
            List<string> srcPKFilter = new List<string>() { string.Empty };
            srcPKFilter.AddRange(this.dt_scanDetail.AsEnumerable().Select(s => s["ID"].ToString()).Distinct().ToList());
            this.comboPKFilter.DataSource = srcPKFilter;

            // 產生select Carton資料
            int cnt_selectCarton = this.LoadSelectCarton();

            if (cnt_selectCarton == 1)
            {
                // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
                if (Convert.ToInt16(this.dt_scanDetail.Compute("Sum(ScanQty)", null)) > 0)
                {
                    if (MyUtility.Msg.InfoBox("This carton had been scanned, are you sure you want to rescan again?", buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (!(result = this.ClearScanQty(this.dt_scanDetail.Select(), "ALL")))
                        {
                            this.ShowErr(result);
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

                if (this.UseAutoScanPack)
                {
                    foreach (DataRow dr in this.dt_scanDetail.Rows)
                    {
                        dr["barcode"] = DBNull.Value;
                    }

                    DBProxy.Current.Execute(null, $"update PackingList_Detail set barcode = null where ID = '{this.PackingListID}' and  CTNStartNo = '{this.CTNStarNo}'  ");
                }

                DualResult result_load = this.LoadScanDetail(0);
                if (!result_load)
                {
                    this.ShowErr(result_load);
                }
            }
            #endregion
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

        private void Tab_Focus(string type)
        {
            if (type.Equals("EAN"))
            {
                this.tabControlScanArea.SelectedTab = this.tabControlScanArea.TabPages[1];
                this.txtScanEAN.Focus();
                this.numBoxScanQty.Value = 0;
                if (this.scanDetailBS.DataSource != null)
                {
                    this.numBoxScanTtlQty.Value = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Sum(s => (int)s["QtyPerCTN"]);
                }

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

        private DualResult LoadScanDetail(int rowidx)
        {
            DualResult result = new DualResult(true);
            SelectCartonDetail dr = (SelectCartonDetail)this.gridSelectCartonDetail.GetData(rowidx);

            if (this.selecedPK != null && this.numBoxScanQty.Value > 0)
            {
                // 這邊加try catch 是為了ISP20191449 補充說明的bug 2 當user殺生問題可以keep當時情況
                try
                {
                    if (dr.ID == this.selecedPK.ID && dr.CTNStartNo == this.selecedPK.CTNStartNo && dr.Article == this.selecedPK.Article)
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    return new DualResult(false, ex);
                }

                if (!this.LackingClose())
                {
                    return result;
                }
            }

            DataRow[] dr_scanDetail = this.dt_scanDetail.Select($"ID = '{dr.ID}' and CTNStartNo = '{dr.CTNStartNo}' ");
            if (dr_scanDetail.Where(s => (short)s["ScanQty"] != (int)s["QtyPerCTN"]).Count() == 0)
            {
                if (MyUtility.Msg.InfoBox("This carton had been scanned, are you sure you want to rescan again?", buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (!(result = this.ClearScanQty(dr_scanDetail, "ALL")))
                    {
                        return result;
                    }

                    // 由於Form上面的DataTable已經更新，因此要重新抓取
                    dr = (SelectCartonDetail)this.gridSelectCartonDetail.GetData(rowidx);
                }
                else
                {
                    this.Tab_Focus("EAN");
                    return result;
                }
            }

            // 確認該箱都有設定Barcode，只要有缺少，就清空該箱Barcode
            if (this.UseAutoScanPack && MyUtility.Check.Seek($"SELECT 1 FROM PackingList_Detail WHERE ID='{MyUtility.Convert.GetString(dr.ID)}' AND CTNStartNo='{MyUtility.Convert.GetString(dr.CTNStartNo)}' AND (Barcode = '' OR Barcode IS NULL) "))
            {
                foreach (DataRow seledr in this.dt_scanDetail.Rows)
                {
                    seledr["Barcode"] = DBNull.Value;
                }

                DBProxy.Current.Execute(null, $"update PackingList_Detail set barcode = null where ID = '{MyUtility.Convert.GetString(dr.ID)}' AND CTNStartNo='{MyUtility.Convert.GetString(dr.CTNStartNo)}' ");
            }

            this.scanDetailBS.DataSource = dr_scanDetail.OrderBy(s => s["Article"]).ThenBy(s => s["Seq"]).CopyToDataTable();
            this.LoadHeadData(dr);
            this.Tab_Focus("EAN");

            return result;
        }

        private void GridSelectCartonDetail_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            this.LoadSelectCarton();
            DualResult result = this.LoadScanDetail(e.RowIndex);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.upd_sql_barcode = string.Empty; // 換箱清空更新barcode字串
        }

        /// <summary>
        /// 清空scan區資料(連同db)
        /// </summary>
        /// <param name="tmp">tmp</param>
        /// <param name="clearType">clearType</param>
        /// <returns>result</returns>
        private DualResult ClearScanQty(DataRow[] tmp, string clearType)
        {
            DualResult result1 = new DualResult(true);
            DualResult result2 = new DualResult(true);
            int oriTtlScanQty = tmp.Sum(o => Convert.ToInt32(o["ScanQty"]));

            string packingListID = tmp[0]["ID"].ToString();
            string orderID = tmp[0]["OrderID"].ToString();
            string cTNStartNo = tmp[0]["CTNStartNo"].ToString();
            string scanName = tmp[0]["ScanName"].ToString();

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
SET ScanQty = 0 ,ScanEditDate = NULL , ActCTNWeight = 0
WHERE ID = '{packingListID}' AND CTNStartNo = '{cTNStartNo}' ";

                string insertCmds = $@"

INSERT INTO [dbo].[PackingScan_History]
           ([MDivisionID]
           ,[PackingListID]
           ,[OrderID]
           ,[CTNStartNo]
           ,[SCICtnNo]
           ,[DeleteFrom]
           ,[ScanQty]
           ,[ScanEditDate]
           ,[ScanName]
           ,[AddName]
           ,[AddDate]
           ,[LackingQty])
     VALUES
           ('{Env.User.Keyword}'
           ,'{packingListID}'
           ,'{orderID}'
           ,'{cTNStartNo}'
           ,(SELECt TOP 1 SCICtnNo FROm PackingList_Detail WHERE ID = '{packingListID}' AND CTNStartNo='{cTNStartNo}')
           ,'Packing P18'
           ,{oriTtlScanQty}
           ,'{Convert.ToDateTime(tmp[0]["ScanEditDate"]).ToAppDateTimeFormatString()}'
           ,'{scanName}'
           ,'{Env.User.UserID}'
           ,GETDATE()
           ,(
                ISNULL(  (SELECT SUM(pd.ShipQty) FROM PackingList_Detail pd WHERE pd.ID='{packingListID}' AND pd.CTNStartNo='{cTNStartNo}') ,0)
                - 
                {oriTtlScanQty}
            ) ----LackingQty計算規則詳見：ISP20191801
            )
";

                result1 = DBProxy.Current.Execute(null, upd_sql);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        result2 = DBProxy.Current.Execute(null, insertCmds);
                        result1 = DBProxy.Current.Execute(null, upd_sql);

                        if (result1 && result2)
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
                this.Reset();
                this.LoadSelectCarton();
            }

            return new DualResult(result1 && result2);
        }

        /// <summary>
        /// 重新載入Select表身資料
        /// </summary>
        /// <returns>int</returns>
        private int LoadSelectCarton()
        {
            if (this.dt_scanDetail == null)
            {
                return 0;
            }

            List<SelectCartonDetail> list_selectCarton = (from c in this.dt_scanDetail.AsEnumerable()
                                                          group c by new
                                                          {
                                                              ID = c.Field<string>("ID"),
                                                              CTNStartNo = c.Field<string>("CTNStartNo"),
                                                              CustPoNo = c.Field<string>("CustPoNo"),
                                                              BrandID = c.Field<string>("BrandID"),
                                                              StyleID = c.Field<string>("StyleID"),
                                                              Dest = c.Field<string>("Dest"),
                                                              Remark = c.Field<string>("Remark"),
                                                          }
                                                          into g
                                                          select new SelectCartonDetail
                                                          {
                                                              ID = g.Key.ID,
                                                              CTNStartNo = g.Key.CTNStartNo,
                                                              CustPoNo = g.Key.CustPoNo,
                                                              BrandId = g.Key.BrandID,
                                                              StyleId = g.Key.StyleID,
                                                              Dest = g.Key.Dest,
                                                              Remark = g.Key.Remark,
                                                              OrderID = string.Join("/", g.Select(st => st.Field<string>("OrderID")).Distinct().ToArray()),
                                                              Article = string.Join("/", g.OrderBy(o => o.Field<string>("Article")).ThenBy(o => o.Field<string>("SizeCode")).ThenBy(o => o.Field<long>("Ukey")).Select(st => st.Field<string>("Article")).ToArray()),
                                                              SizeCode = string.Join("/", g.OrderBy(o => o.Field<string>("Article")).ThenBy(o => o.Field<string>("SizeCode")).ThenBy(o => o.Field<long>("Ukey")).Select(st => st.Field<string>("SizeCode")).ToArray()),
                                                              QtyPerCTN = string.Join("/", g.OrderBy(o => o.Field<string>("Article")).ThenBy(o => o.Field<string>("SizeCode")).ThenBy(o => o.Field<long>("Ukey")).Select(st => st.Field<int>("QtyPerCTN").ToString()).ToArray()),
                                                              ScanQty = string.Join("/", g.OrderBy(o => o.Field<string>("Article")).ThenBy(o => o.Field<string>("SizeCode")).ThenBy(o => o.Field<long>("Ukey")).Select(st => st.Field<short>("ScanQty").ToString()).ToArray()),
                                                              TtlScanQty = g.Sum(st => st.Field<short>("ScanQty")),
                                                              TtlQtyPerCTN = g.Sum(st => st.Field<int>("QtyPerCTN")),
                                                              PKseq = g.Max(st => st.Field<string>("PKseq")),
                                                              PassName = g.Select(st => st.Field<string>("PassName")).FirstOrDefault(), // 一箱只會有一個掃描人員，因此抓第一筆就好
                                                              ActCTNWeight = g.Max(st => st.Field<decimal>("ActCTNWeight")),
                                                          }).OrderBy(s => s.ID).ThenBy(s => s.PKseq).ToList();
            string default_where = " 1 = 1 ";

            // Only not yet scan complete checkbox
            if (this.chkBoxNotScan.Checked)
            {
                default_where += " and TtlScanQty <> TtlQtyPerCTN ";
            }

            // Packing No Filter combobox
            if (!MyUtility.Check.Empty(this.comboPKFilter.SelectedValue))
            {
                default_where += $" and ID = \"{this.comboPKFilter.SelectedValue}\"";
            }

            var selectCartonFilterResult = list_selectCarton.Where(default_where);

            if (selectCartonFilterResult.Any())
            {
                this.selcartonBS.DataSource = list_selectCarton.Where(default_where);
            }
            else
            {
                this.selcartonBS.DataSource = null;
            }

            var queryTotal = from c in list_selectCarton
                             group c by c.ID into g
                             select new { totalWeight = g.Sum(x => x.ActCTNWeight) };
            foreach (var item in queryTotal)
            {
                this.txtTotalWeight.Text = MyUtility.Convert.GetString(item.totalWeight);
            }

            return list_selectCarton.Count;
        }

        private void LoadHeadData(SelectCartonDetail dr)
        {
            string sum_sql = $@"select TtlCartons= sum(CTNQty),
                                       TtlQty = sum(ShipQty),
                                       TtlPackedCartons = sum(case when ScanQty > 0 and CTNQty>0 then 1 else 0 end),
                                       TtlPackQty = sum(ScanQty)
                                from PackingList_Detail
                                where id = '{dr.ID}'";
            DataRow dr_sum;
            MyUtility.Check.Seek(sum_sql, out dr_sum);

            this.displayPackID.Text = dr.ID;
            this.displayCtnNo.Text = dr.CTNStartNo;
            this.displaySPNo.Text = dr.OrderID;
            this.displayPoNo.Text = dr.CustPoNo;
            this.displayBrand.Text = dr.BrandId;
            this.displayStyle.Text = dr.StyleId;
            this.txtDest.TextBox1.Text = dr.Dest;
            this.numWeight.Text = MyUtility.Convert.GetString(dr.ActCTNWeight);

            this.numBoxttlCatons.Text = dr_sum["TtlCartons"].ToString();
            this.numBoxttlQty.Text = dr_sum["TtlQty"].ToString();
            this.numBoxPackedCartons.Text = dr_sum["TtlPackedCartons"].ToString();
            this.numBoxttlPackQty.Text = ((int)dr_sum["TtlPackQty"] + this.numBoxScanQty.Value).ToString();
            this.numBoxRemainCartons.Text = ((int)dr_sum["TtlCartons"] - (int)dr_sum["TtlPackedCartons"]).ToString();
            this.numBoxRemainQty.Text = ((int)dr_sum["TtlQty"] - (int)dr_sum["TtlPackQty"] - this.numBoxScanQty.Value).ToString();

            this.selecedPK = dr;

            this.lbCustomize1.Text = MyUtility.GetValue.Lookup($"select Customize1 from Brand with(nolock) where id = '{dr.BrandId}'");
            this.lbCustomize2.Text = MyUtility.GetValue.Lookup($"select Customize2 from Brand with(nolock) where id = '{dr.BrandId}'");
            this.lbCustomize3.Text = MyUtility.GetValue.Lookup($"select Customize3 from Brand with(nolock) where id = '{dr.BrandId}'");
            this.displayCustomize1.Text = MyUtility.GetValue.Lookup($"select Customize1 from orders with(nolock) where id = '{dr.OrderID}'");
            this.displayCustomize2.Text = MyUtility.GetValue.Lookup($"select Customize2 from orders with(nolock) where id = '{dr.OrderID}'");
            this.displayCustomize3.Text = MyUtility.GetValue.Lookup($"select Customize3 from orders with(nolock) where id = '{dr.OrderID}'");
            this.displayKIT.Text = MyUtility.GetValue.Lookup($@"
SELECT TOP 1 cc.Kit
FROM Orders o 
LEFT JOIN CustCD cc ON o.BrandID=cc.BrandID AND o.CustCDID=cc.ID
WHERE o.ID='{dr.OrderID}'");
            this.boxPackingRemark.Text = dr.Remark;
            this.chkVasShas.Checked = MyUtility.Convert.GetBool(
                MyUtility.GetValue.Lookup($"SELECT VasShas FROM Orders WHERE ID = '{dr.OrderID}'")
                );
        }

        private void ChkBoxNotScan_CheckedChanged(object sender, EventArgs e)
        {
            this.LoadSelectCarton();
        }

        private void ComboPKFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadSelectCarton();
        }

        private void TxtQuickSelCTN_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.gridSelectCartonDetail.RowCount > 0)
            {
                var obj = this.selcartonBS.List.OfType<SelectCartonDetail>().ToList().Find(f => f.CTNStartNo.Equals(this.txtQuickSelCTN.Text));
                var pos = this.selcartonBS.IndexOf(obj);
                this.selcartonBS.Position = pos;
            }
        }

        private string upd_sql_barcode = string.Empty;

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

            DualResult sql_result;

            // 判斷輸入的Barcode，有沒有存在gridScanDetail
            int barcode_pos = this.scanDetailBS.Find("Barcode", this.txtScanEAN.Text);

            // 不存在
            if (barcode_pos == -1)
            {
                // 如果不存在，代表一定是第一次掃描，則找出Barcode還空著的Row填進去
                int no_barcode_cnt = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Where(s => MyUtility.Check.Empty(s["Barcode"])).Count();

                // 沒有Barcode還空著的Row，代表操作有錯誤，回傳退回指令
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

                    this.upd_sql_barcode += $@"
update PackingList_Detail
set BarCode = '{this.txtScanEAN.Text}'
where PackingList_Detail.Article 
=  '{no_barcode_dr["Article"]}'
and PackingList_Detail.SizeCode
=  '{no_barcode_dr["SizeCode"]}'
and PackingList_Detail.ID = '{this.selecedPK.ID}'
and PackingList_Detail.CTNStartNo = '{this.selecedPK.CTNStartNo}'
";
                    foreach (DataRow dr in ((DataTable)this.scanDetailBS.DataSource).Rows)
                    {
                        if (dr["Article"].Equals(no_barcode_dr["Article"]) && dr["SizeCode"].Equals(no_barcode_dr["SizeCode"]))
                        {
                            dr["Barcode"] = this.txtScanEAN.Text;
                            dr["ScanQty"] = (short)dr["ScanQty"] + 1;

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
                    cur_dr["ScanQty"] = (short)cur_dr["ScanQty"] + 1;
                    this.UpdScanQty((long)cur_dr["Ukey"]);
                }
            }

            // this.scanDetailBS.ResetCurrentItem();
            this.scanDetailBS.ResetBindings(true);

            // 計算scanQty
            this.numBoxScanQty.Value = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Sum(s => (short)s["ScanQty"]);

            this.txtScanEAN.Text = string.Empty;

            // 如果都掃完 update PackingList_Detail
            if (this.numBoxScanQty.Value == this.numBoxScanTtlQty.Value)
            {
                if (!MyUtility.Check.Empty(this.upd_sql_barcode))
                {
                    if (!(sql_result = DBProxy.Current.Execute(null, this.upd_sql_barcode)))
                    {
                        this.ShowErr(sql_result);
                        return;
                    }
                }

                bool isNeedShowWeightInputWindow = this.chk_AutoCheckWeight.Checked && MyUtility.Check.Empty(this.numWeight.Value);

                if (isNeedShowWeightInputWindow)
                {
                    P18_InputWeight p18_InputWeight = new P18_InputWeight();
                    p18_InputWeight.ShowDialog();
                    this.numWeight.Value = p18_InputWeight.ActWeight;
                    this.numWeight.ValidateControl();
                }

                string upd_sql = $@"
update PackingList_Detail 
set ScanQty = QtyPerCTN 
, ScanEditDate = GETDATE()
, ScanName = '{Env.User.UserID}'   
, Lacking = 0
, ActCTNWeight = {this.numWeight.Value}
where id = '{this.selecedPK.ID}' 
and CTNStartNo = '{this.selecedPK.CTNStartNo}' ";
                sql_result = DBProxy.Current.Execute(null, upd_sql);
                if (!sql_result)
                {
                    this.ShowErr(sql_result);
                    return;
                }

                this.AfterCompleteScanCarton();
            }
            else
            {
                // 讓遊標停留在原地
                e.Cancel = true;
            }
        }

        private void UpdScanQty(long ukey, string barcode = "")
        {
            foreach (DataRow dr in this.dt_scanDetail.Rows)
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
                this.numWeight.Text = string.Empty;

                this.numBoxttlCatons.Text = string.Empty;
                this.numBoxttlQty.Text = string.Empty;
                this.numBoxPackedCartons.Text = string.Empty;
                this.numBoxttlPackQty.Text = string.Empty;
                this.numBoxRemainCartons.Text = string.Empty;
                this.numBoxRemainQty.Text = string.Empty;
                this.scanDetailBS.DataSource = null;
                this.selcartonBS.DataSource = null;
                this.selecedPK = null;
                if (this.dt_scanDetail != null)
                {
                    this.dt_scanDetail.Clear();
                }

                this.comboPKFilter.DataSource = null;
                this.Tab_Focus("CARTON");
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
                this.txtDest.TextBox1.Text = string.Empty;
                this.numWeight.Text = string.Empty;

                this.numBoxttlCatons.Text = string.Empty;
                this.numBoxttlQty.Text = string.Empty;
                this.numBoxPackedCartons.Text = string.Empty;
                this.numBoxttlPackQty.Text = string.Empty;
                this.numBoxRemainCartons.Text = string.Empty;
                this.numBoxRemainQty.Text = string.Empty;
                this.scanDetailBS.DataSource = null;
                this.selecedPK = null;
            }
        }

        // 修改Actual CTN# Weight值時存檔
        private void NumWeight_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(((TextBox)sender).Text.ToString()))
            {
                return;
            }

            if (this.selecedPK != null)
            {
                if (!MyUtility.Check.Empty(this.selecedPK.ID) && !MyUtility.Check.Empty(this.selecedPK.CTNStartNo) && !MyUtility.Check.Empty(this.selecedPK.Article))
                {
                    DataRow[] dt_scanDetailrow = this.dt_scanDetail.Select($"ID = '{this.selecedPK.ID}' and CTNStartNo = '{this.selecedPK.CTNStartNo}'");
                    foreach (DataRow dr in dt_scanDetailrow)
                    {
                        dr["ActCTNWeight"] = this.numWeight.Text;
                    }

                    this.LoadSelectCarton();
                }
            }
        }

        private class SelectCartonDetail
        {
            private string iD;
            private string cTNStartNo;
            private string custPoNo;
            private string article;
            private string orderID;
            private string sizeCode;
            private string qtyPerCTN;
            private string scanQty;
            private string brandId;
            private string styleId;
            private int ttlScanQty;
            private int ttlQtyPerCTN;
            private string pKseq;
            private string dest;
            private string passName;
            private decimal actCTNWeight;

            private string _remark;

            public string ID
            {
                get
                {
                    return this.iD;
                }

                set
                {
                    this.iD = value;
                }
            }

            public string CTNStartNo
            {
                get
                {
                    return this.cTNStartNo;
                }

                set
                {
                    this.cTNStartNo = value;
                }
            }

            public string CustPoNo
            {
                get
                {
                    return this.custPoNo;
                }

                set
                {
                    this.custPoNo = value;
                }
            }

            public string Article
            {
                get
                {
                    return this.article;
                }

                set
                {
                    this.article = value;
                }
            }

            public string OrderID
            {
                get
                {
                    return this.orderID;
                }

                set
                {
                    this.orderID = value;
                }
            }

            public string SizeCode
            {
                get
                {
                    return this.sizeCode;
                }

                set
                {
                    this.sizeCode = value;
                }
            }

            public string QtyPerCTN
            {
                get
                {
                    return this.qtyPerCTN;
                }

                set
                {
                    this.qtyPerCTN = value;
                }
            }

            public string ScanQty
            {
                get
                {
                    return this.scanQty;
                }

                set
                {
                    this.scanQty = value;
                }
            }

            public string BrandId
            {
                get
                {
                    return this.brandId;
                }

                set
                {
                    this.brandId = value;
                }
            }

            public string StyleId
            {
                get
                {
                    return this.styleId;
                }

                set
                {
                    this.styleId = value;
                }
            }

            public int TtlScanQty
            {
                get
                {
                    return this.ttlScanQty;
                }

                set
                {
                    this.ttlScanQty = value;
                }
            }

            public string PKseq
            {
                get
                {
                    return this.pKseq;
                }

                set
                {
                    this.pKseq = value;
                }
            }

            public string Dest
            {
                get
                {
                    return this.dest;
                }

                set
                {
                    this.dest = value;
                }
            }

            public int TtlQtyPerCTN
            {
                get
                {
                    return this.ttlQtyPerCTN;
                }

                set
                {
                    this.ttlQtyPerCTN = value;
                }
            }

            public string PassName
            {
                get
                {
                    return this.passName;
                }

                set
                {
                    this.passName = value;
                }
            }

            public decimal ActCTNWeight
            {
                get
                {
                    return this.actCTNWeight;
                }

                set
                {
                    this.actCTNWeight = value;
                }
            }

            public string Remark
            {
                get
                {
                    return this._remark;
                }

                set
                {
                    this._remark = value;
                }
            }
        }

        private void BtnLacking_Click(object sender, EventArgs e)
        {
            this.UpdateLackingStatus();
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
                string upd_sql = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    upd_sql += $@"
update PackingList_Detail 
set   
ScanQty = {(MyUtility.Check.Empty(dr["ScanQty"]) ? "0" : dr["ScanQty"])} 
, ScanEditDate = GETDATE()
, ScanName = '{Env.User.UserID}'   
, Lacking = 1
, BarCode = '{dr["Barcode"]}'
where Ukey={dr["Ukey"]}
";
                }

                sql_result = DBProxy.Current.Execute(null, upd_sql);
                if (!sql_result)
                {
                    this.ShowErr(sql_result);
                    return;
                }

                this.AfterCompleteScanCarton();

                MyUtility.Msg.InfoBox("Lacking successfully!!");
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
            DataRow drPassName;
            string passName = string.Empty;

            // 掃描完成後要重新撈一次ScanName，存到Form的DataTable
            string scanName = string.Empty;

            string sql = $@"
select  ScanName, isnull(iif(ps.name is null, convert(nvarchar(10),pd.ScanEditDate,112), ps.name+'-'+convert(nvarchar(10),pd.ScanEditDate,120)),'') as PassName,
        pd.ScanEditDate
from PackingList_Detail pd
left join pass1 ps WITH (NOLOCK) on pd.ScanName = ps.id
where pd.id = '{this.selecedPK.ID}' 
and pd.CTNStartNo = '{this.selecedPK.CTNStartNo}'
";

            if (MyUtility.Check.Seek(sql, out drPassName))
            {
                passName = MyUtility.Convert.GetString(drPassName["PassName"]);
                scanName = MyUtility.Convert.GetString(drPassName["ScanName"]);
            }

            DataRow[] dt_scanDetailrow = this.dt_scanDetail.Select($"ID = '{this.selecedPK.ID}' and CTNStartNo = '{this.selecedPK.CTNStartNo}'");
            foreach (DataRow dr in dt_scanDetailrow)
            {
                dr["ScanName"] = scanName;
                dr["PassName"] = passName;
                dr["ScanEditDate"] = drPassName["ScanEditDate"];
            }

            // 檢查下方carton列表是否都掃完
            int carton_complete = this.dt_scanDetail.AsEnumerable().Where(s => (short)s["ScanQty"] != (int)s["QtyPerCTN"]).Count();
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

        /// <inheritdoc/>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!this.LackingClose())
            {
                e.Cancel = true;
            }

            base.OnFormClosing(e);
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
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void Reset()
        {
            DualResult result;
            this.PackingListID = string.Empty;
            this.CTNStarNo = string.Empty;

            if (MyUtility.Check.Empty(this.txtScanCartonSP.Text))
            {
                return;
            }

            if (this.txtScanCartonSP.Text.Length > 13)
            {
                this.PackingListID = this.txtScanCartonSP.Text.Substring(0, 13);
                this.CTNStarNo = this.txtScanCartonSP.Text.Substring(13, this.txtScanCartonSP.Text.Length - 13);
            }

            this.upd_sql_barcode = string.Empty; // 換箱清空更新barcode字串
            this.ClearAll("SCAN");

            if (this.txtScanCartonSP.Text.Length > 13)
            {
                this.PackingListID = this.txtScanCartonSP.Text.Substring(0, 13);
                this.CTNStarNo = this.txtScanCartonSP.Text.Substring(13, this.txtScanCartonSP.Text.Length - 13);
            }

            this.upd_sql_barcode = string.Empty; // 換箱清空更新barcode字串
            this.ClearAll("SCAN");

            #region 檢查是否有資料，三個角度

            // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
            // 2.=Orders.ID
            // 3.=Orders.CustPoNo
            string[] aLLwhere = new string[]
            {
                this.txtScanCartonSP.Text.Length > 13 ? $" and  pd.ID = '{this.PackingListID}' and  pd.CTNStartNo = '{this.CTNStarNo}'" : " and 1=0 ",
                $" and  pd.ID = '{this.txtScanCartonSP.Text}'",
                $@" and o.ID = '{this.txtScanCartonSP.Text}' or o.CustPoNo = '{this.txtScanCartonSP.Text}'",
                $@" and pd.CustCTN = '{this.txtScanCartonSP.Text}'",
            };

            string scanDetail_sql = $@"select distinct
                                           pd.ID,
                                           pd.CTNStartNo  ,
                                           pd.OrderID,
                                           o.CustPoNo ,
                                           pd.Article    ,
                                           pd.Color,
                                           pd.SizeCode  ,
                                           pd.QtyPerCTN,
                                           ScanQty = pd.ScanQty,
                                           pd.ScanEditDate,
                                           pd.ScanName,
                                           pd.barcode,
                                           p.BrandID,
                                           o.StyleID,
                                           os.Seq,
                                           pd.Ukey,
                                           [PKseq] = pd.Seq,
                                           o.Dest,
                                           isnull(pd.ActCTNWeight,0) as ActCTNWeight, 
                                           isnull(iif(ps.name is null, convert(nvarchar(10),pd.ScanEditDate,112), ps.name+'-'+convert(nvarchar(10),pd.ScanEditDate,120)),'') as PassName
                                           ,p.Remark
										   ,pd.Ukey
                                from PackingList_Detail pd WITH (NOLOCK)
                                inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                                inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
                                left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode 
                                left join pass1 ps WITH (NOLOCK) on pd.ScanName = ps.id
                                where p.Type in ('B','L') ";

            foreach (string where in aLLwhere)
            {
                result = DBProxy.Current.Select(null, scanDetail_sql + where, out this.dt_scanDetail);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (this.dt_scanDetail.Rows.Count > 0)
                {
                    break;
                }
            }

            if (this.dt_scanDetail.Rows.Count == 0)
            {
                AutoClosingMessageBox.Show($"<{this.txtScanCartonSP.Text}> Invalid CTN#!!", "Warning", 3000);
                return;
            }

            // 產生comboPKFilter資料
            List<string> srcPKFilter = new List<string>() { string.Empty };
            srcPKFilter.AddRange(this.dt_scanDetail.AsEnumerable().Select(s => s["ID"].ToString()).Distinct().ToList());
            this.comboPKFilter.DataSource = srcPKFilter;

            // 產生select Carton資料
            int cnt_selectCarton = this.LoadSelectCarton();

            if (cnt_selectCarton == 1)
            {
                // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
                if (Convert.ToInt16(this.dt_scanDetail.Compute("Sum(ScanQty)", null)) > 0)
                {
                    if (MyUtility.Msg.InfoBox("This carton had been scanned, are you sure you want to rescan again?", buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (!(result = this.ClearScanQty(this.dt_scanDetail.Select(), "ALL")))
                        {
                            this.ShowErr(result);
                            return;
                        }
                    }
                    else
                    {
                        this.ClearAll("ALL");
                        return;
                    }
                }

                if (this.UseAutoScanPack)
                {
                    foreach (DataRow dr in this.dt_scanDetail.Rows)
                    {
                        dr["barcode"] = DBNull.Value;
                    }

                    DBProxy.Current.Execute(null, $"update PackingList_Detail set barcode = null where ID = '{this.PackingListID}' and  CTNStartNo = '{this.CTNStarNo}'  ");
                }

                DualResult result_load = this.LoadScanDetail(0);
                if (!result_load)
                {
                    this.ShowErr(result_load);
                }
            }
            #endregion
        }

        private void TxtScanEAN_Leave(object sender, EventArgs e)
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

            DualResult sql_result;
            int barcode_pos = this.scanDetailBS.Find("Barcode", this.txtScanEAN.Text);

            // 無Barcode
            if (barcode_pos == -1)
            {
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
                    return;
                }
                else
                {
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

                    this.upd_sql_barcode += $@"
update PackingList_Detail
set BarCode = '{this.txtScanEAN.Text}'
where PackingList_Detail.Article 
=  '{no_barcode_dr["Article"]}'
and PackingList_Detail.SizeCode
=  '{no_barcode_dr["SizeCode"]}'
and PackingList_Detail.ID = '{this.selecedPK.ID}'
and PackingList_Detail.CTNStartNo = '{this.selecedPK.CTNStartNo}'
";
                    foreach (DataRow dr in ((DataTable)this.scanDetailBS.DataSource).Rows)
                    {
                        if (dr["Article"].Equals(no_barcode_dr["Article"]) && dr["SizeCode"].Equals(no_barcode_dr["SizeCode"]))
                        {
                            dr["Barcode"] = this.txtScanEAN.Text;
                            dr["ScanQty"] = (short)dr["ScanQty"] + 1;
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
                if (scanQty >= qtyPerCTN)
                {
                    // 此barcode已足夠,或超過 送回
                    if (this.UseAutoScanPack)
                    {
                        this.IDX.IdxCall(254, "a:" + this.txtScanEAN.Text.Trim(), ("a:" + this.txtScanEAN.Text.Trim()).Length);
                    }

                    AutoClosingMessageBox.Show($"This Size scan is complete,can not scan again!!", "Warning", 3000);
                    this.txtScanEAN.Text = string.Empty;
                    return;
                }
                else
                {
                    cur_dr["ScanQty"] = (short)cur_dr["ScanQty"] + 1;
                    this.UpdScanQty((long)cur_dr["Ukey"]);
                }
            }

            // this.scanDetailBS.ResetCurrentItem();
            this.scanDetailBS.ResetBindings(true);

            // 計算scanQty
            this.numBoxScanQty.Value = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Sum(s => (short)s["ScanQty"]);

            this.txtScanEAN.Text = string.Empty;

            // 如果都掃完 update PackingList_Detail
            if (this.numBoxScanQty.Value == this.numBoxScanTtlQty.Value)
            {
                if (!MyUtility.Check.Empty(this.upd_sql_barcode))
                {
                    if (!(sql_result = DBProxy.Current.Execute(null, this.upd_sql_barcode)))
                    {
                        this.ShowErr(sql_result);
                        return;
                    }
                }

                bool isNeedShowWeightInputWindow = this.chk_AutoCheckWeight.Checked && MyUtility.Check.Empty(this.numWeight.Value);

                if (isNeedShowWeightInputWindow)
                {
                    P18_InputWeight p18_InputWeight = new P18_InputWeight();
                    p18_InputWeight.ShowDialog();
                    this.numWeight.Value = p18_InputWeight.ActWeight;
                    this.numWeight.ValidateControl();
                }

                string upd_sql = $@"
update PackingList_Detail 
set ScanQty = QtyPerCTN 
, ScanEditDate = GETDATE()
, ScanName = '{Env.User.UserID}'   
, Lacking = 0
, ActCTNWeight = {this.numWeight.Value}
where id = '{this.selecedPK.ID}' 
and CTNStartNo = '{this.selecedPK.CTNStartNo}' ";
                sql_result = DBProxy.Current.Execute(null, upd_sql);
                if (!sql_result)
                {
                    this.ShowErr(sql_result);
                    return;
                }

                this.AfterCompleteScanCarton();
            }
            else
            {
                // 讓遊標停留在原地
                this.txtScanEAN.Focus();
            }
        }
    }
}
