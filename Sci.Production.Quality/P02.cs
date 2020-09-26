using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Quality
{
    public partial class P02 : Win.Tems.Input6
    {
        // 宣告Context Menu Item
        private ToolStripMenuItem edit;
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private readonly bool boolFromP02;
        private string find = string.Empty;
        private int index;
        private DataRow[] find_dr;

        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.ContextMenuStrip = this.detailgridmenus;
            this.boolFromP02 = false;
        }

        public P02(string pOID)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("ID = '{0}'", pOID);
            this.InsertDetailGridOnDoubleClick = false;
            this.IsSupportEdit = false;
            this.detailgrid.ContextMenuStrip = this.detailgridmenus;
            this.boolFromP02 = true;
        }

        // 表身額外的資料來源

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            this.find_dr = null;
            this.find = string.Empty;
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmd = string.Format(
                @"
Select a.id
,a.poid
,SEQ1
,SEQ2
,a.ReceivingID
,Refno
,SCIRefno
,Suppid
,C.exportid
,ArriveQty
,InspDeadline
,a.InspQty
,a.RejectQty
,a.Defect
,[DefectDescription] = DefectText.Val
,a.Result
,[Result1]=a.Result
,a.InspDate
,(
	select Pass1.Name from Pass1 WITH (NOLOCK) where a.Inspector = pass1.id
) AS Inspector2,a.Inspector,
a.Remark,a.ReplacementReportID,
a.Status,ReplacementReportID,(seq1+seq2) as seq,
(
    Select weavetypeid from Fabric b WITH (NOLOCK) where b.SCIRefno =a.SCIrefno
) as weavetypeid,
c.whseArrival,
(
    dbo.GetColorMultipleID((select top 1 o.BrandID from orders o where o.POID =a.poid) ,(Select d.colorid from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2))
) as Colorid,
(
    Select d.SizeSpec from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
) as Size,
(
    Select d.StockUnit from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
) as unit,
(
    Select AbbEn From Supp WITH (NOLOCK) Where a.suppid = supp.id
) as SuppEn

From AIR a WITH (NOLOCK) Left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
OUTER APPLY(
	SELECT  [Val]=  STUFF((
	SELECT ', '+ IIF(a.Defect = '' , '' ,ori.Data +'-'+ ISNULL(ad.Description,''))
	FROM [SplitString](a.Defect,'+') ori
	LEFT JOIN AccessoryDefect ad  WITH (NOLOCK) on ad.id = ori.Data
	 FOR XML PATH('')
	 ),1,1,'')

)DefectText
Where a.poid='{0}' order by seq1,seq2  
", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            // Grid 事件屬性: 右鍵跳出新視窗
            DataGridViewGeneratorTextColumnSettings detail = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings detail_Int = new DataGridViewGeneratorNumericColumnSettings();

            detail.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                P02_Detail doForm = new P02_Detail(false, this.CurrentDetailData["ID"].ToString(), this.CurrentMaintain["ID"].ToString());
                doForm.Set(false, this.DetailDatas, this.CurrentDetailData);
                doForm.ShowDialog(this);
                doForm.Close();
                this.RenewData();
            };
            detail_Int.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                P02_Detail doForm = new P02_Detail(false, this.CurrentDetailData["ID"].ToString(), this.CurrentMaintain["ID"].ToString());
                doForm.Set(false, this.DetailDatas, this.CurrentDetailData);
                doForm.ShowDialog(this);
                doForm.Close();
                this.RenewData();
            };

            #region set Grid

            this.detailgrid.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ExportID", header: "WKNO", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(26), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Size", header: "Size", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 11, decimal_places: 2, iseditingreadonly: true)
                .Text("unit", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("InspQty", header: "Inspected Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true, settings: detail_Int)
                .Text("RejectQty", header: "Reject Qty", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("DefectDescription", header: "Defect Type", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: detail)
                .Text("Inspdate", header: "Insp. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("Inspector2", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: detail)
                .Text("ReplacementID", header: "1st ReplacementID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ReceivingID", header: "Receiving ID", width: Widths.AnsiChars(15), iseditingreadonly: true);
            this.detailgrid.Columns["InspQty"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["RejectQty"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["DefectDescription"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["Result"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["Inspdate"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["inspector2"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            #endregion

        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("ModifyDetail", onclick: (s, e) => this.ModifyDetail()).Get(out this.edit);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, ",last two years data");
            if (this.boolFromP02)
            {
                this.ExpressQuery = false;
            }
            else
            {
                this.queryfors.SelectedIndex = 1;
                this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                this.ExpressQuery = true;
            }

            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    case 1:
                        this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.ContextMenuStripSet();
            base.OnDetailEntered();
            this.detailgrid.AutoResizeColumns();

            DataRow queryDr;
            DualResult dResult = PublicPrg.Prgs.QueryQaInspectionHeader(this.CurrentMaintain["ID"].ToString(), out queryDr);
            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            DataTable sciTb;
            string query_cmd = string.Format("select * from Getsci('{0}','{1}')", this.CurrentMaintain["ID"], MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Category"]);
            DBProxy.Current.Select(null, query_cmd, out sciTb);
            if (!dResult)
            {
                this.ShowErr(query_cmd, dResult);
                return;
            }

            // Get scidelivery_box.Text  value
            if (sciTb.Rows.Count > 0)
            {
                if (sciTb.Rows[0]["MinSciDelivery"] == DBNull.Value)
                {
                    // dateEarliestSCIDel.Text = "";
                }
                else
                {
                    // dateEarliestSCIDel.Text = Convert.ToDateTime(sciTb.Rows[0]["MinSciDelivery"]).ToShortDateString();
                }
            }
            else
            {
                // dateEarliestSCIDel.Text = "";
            }

            // 找出Cutinline and MinSciDelivery 比較早的日期
            DateTime? targT = PublicPrg.Prgs.GetTargetLeadTime(MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
            if (targT != null)
            {
                this.dateTargetLeadTime.Text = ((DateTime)targT).ToShortDateString();
            }
            else
            {
                this.dateTargetLeadTime.Text = string.Empty;
            }

            this.displayStyle.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Styleid"].ToString();
            this.displaySeason.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Seasonid"].ToString();
            this.displayBrand.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["brandid"].ToString();
            if (MyUtility.Check.Empty(queryDr))
            {
                this.dateEarliestEstCutDate.Text = string.Empty;
            }
            else
            {
                if (queryDr["cutinline"] == DBNull.Value)
                {
                    this.dateEarliestEstCutDate.Text = string.Empty;
                }
                else
                {
                    this.dateEarliestEstCutDate.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
                }
            }

            this.displayMTLCmlpt.Text = this.CurrentMaintain["Complete"].ToString() == "1" ? "Y" : string.Empty;
            decimal detailRowCount = this.DetailDatas.Count;
            string inspnum = "0";
            DataTable detailTb = (DataTable)this.detailgridbs.DataSource;
            int t = detailTb.Rows.Count;
            for (int i = t - 1; i >= 0; i--)
            {
                if (detailTb.Rows[i]["STATUS"].ToString().ToUpper() == "NEW")
                {
                    detailTb.Rows[i]["Result"] = string.Empty;
                }
            }

            if (detailRowCount != 0)
            {
                if (detailTb.Rows.Count != 0)
                {
                    DataRow[] inspectAry = detailTb.Select("Result<>'' AND STATUS='Confirmed'");

                    if (inspectAry.Length > 0)
                    {
                        inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                    }
                }
            }

            // displayofInspection.Text = inspnum;
            DateTime completedate;
            if (inspnum == "100")
            {
                completedate = (DateTime)detailTb.Compute("Max(InspDate)", string.Empty);

                this.dateCompletionDate.Text = completedate.ToShortDateString();
            }
            else
            {
                this.dateCompletionDate.Text = string.Empty;
            }

            // this.grid.AutoResizeColumns();

            // 判斷Batch Encode是否可用
            // this.btnBatchEncode.Enabled = this.EditMode ? false : detailTb.AsEnumerable().Where(s => !s["Status"].Equals("Confirmed")).Any();
            string strInspAutoLockAcc = MyUtility.GetValue.Lookup("SELECT InspAutoLockAcc FROM System");
            this.chkInspAutoLockAcc.Checked = MyUtility.Convert.GetBool(strInspAutoLockAcc);
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            // 因為表頭是PO不能覆蓋其他資料，必需自行存檔
            // string save_po_cmd = string.Format("update po set AirRemark = '{0}' where id = '{1}';", CurrentMaintain["AiRemark"], CurrentMaintain["ID"]);
            string save_po_cmd = string.Format("update po set AIRRemark = '{0}' where id = '{1}';", this.editRemark.Text.ToString(), this.CurrentMaintain["ID"]);

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, save_po_cmd)))
                    {
                        transactionscope.Dispose();
                        return upResult;
                    }

                    // 更新PO.AIRInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'AIR','{this.CurrentMaintain["ID"]}'")))
                    {
                        transactionscope.Dispose();
                        return upResult;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return Ict.Result.True;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;

            return Ict.Result.True;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)this.detailgridbs.DataSource;

            // 移到指定那筆
            string wk = this.txtLocateforWK.Text;
            string seq1 = this.txtSEQ1.Text;
            string seq2 = this.txtSEQ2.Text;
            string find = string.Empty;
            string find_new = string.Empty;

            if (!MyUtility.Check.Empty(wk))
            {
                find_new = string.Format("Exportid='{0}'", wk);
            }

            if (!MyUtility.Check.Empty(seq1))
            {
                if (!MyUtility.Check.Empty(find_new))
                {
                    find_new = find_new + string.Format(" and SEQ1 = '{0}'", seq1);
                }
                else
                {
                    find_new = string.Format("SEQ1 = '{0}'", seq1);
                }
            }

            if (!MyUtility.Check.Empty(seq2))
            {
                if (!MyUtility.Check.Empty(find_new))
                {
                    find_new = find_new + string.Format(" and SEQ2 = '{0}'", seq2);
                }
                else
                {
                    find_new = string.Format("SEQ2 = '{0}'", seq2);
                }
            }

            if (find != find_new)
            {
                find = find_new;
                this.find_dr = detDtb.Select(find_new);
                if (this.find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Not Found");
                    return;
                }
                else
                {
                    this.index = 0;
                }
            }
            else
            {
                this.index++;
                if (this.index >= this.find_dr.Length)
                {
                    this.index = 0;
                }
            }

            this.detailgridbs.Position = this.DetailDatas.IndexOf(this.find_dr[this.index]);
        }

        private void ModifyDetail()
        {
            if (!this.IsSupportEdit || this.EditMode)
            {
                return;   // 沒編輯權限 OR 編輯模式下 不能跳下一層編輯
            }

            if (MyUtility.Check.Empty(this.CurrentDetailData["ID"].ToString()))
            {
                return;
            }

            string currentID = this.CurrentDetailData["ID"].ToString();
            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            P02_Detail doForm = new P02_Detail(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), this.CurrentMaintain["ID"].ToString());
            doForm.Set(false, this.DetailDatas, this.CurrentDetailData);
            doForm.Text = "Accessory Inspection- SP+SEQ+Detail(Modify)";
            doForm.ShowDialog(this);
            doForm.Close();
            this.RenewData();
            this.OnDetailEntered();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
            this.ContextMenuStripSet();
        }

        private void ContextMenuStripSet()
        {
            var dr = this.CurrentDetailData;
            this.edit.Enabled = true;

            if (dr == null)
            {
                this.edit.Enabled = false;
                return;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            this.ContextMenuStripSet();
            base.OnDetailGridRowChanged();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)this.detailgridbs.DataSource;

            // 移到指定那筆
            string wk = this.txtLocateforWK.Text;
            string seq1 = this.txtSEQ1.Text;
            string seq2 = this.txtSEQ2.Text;
            string find_new = string.Empty;

            if (!MyUtility.Check.Empty(wk))
            {
                find_new = string.Format("Exportid='{0}'", wk);
            }

            if (!MyUtility.Check.Empty(seq1))
            {
                if (!MyUtility.Check.Empty(find_new))
                {
                    find_new = find_new + string.Format(" and SEQ1 = '{0}'", seq1);
                }
                else
                {
                    find_new = string.Format("SEQ1 = '{0}'", seq1);
                }
            }

            if (!MyUtility.Check.Empty(seq2))
            {
                if (!MyUtility.Check.Empty(find_new))
                {
                    find_new = find_new + string.Format(" and SEQ2 = '{0}'", seq2);
                }
                else
                {
                    find_new = string.Format("SEQ2 = '{0}'", seq2);
                }
            }

            if (this.find != find_new)
            {
                this.find = find_new;
                this.find_dr = detDtb.Select(find_new);
                if (this.find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Not Found");
                    return;
                }
                else
                {
                    this.index = 0;
                }
            }
            else
            {
                if (this.find_dr == null)
                {
                    MyUtility.Msg.WarningBox("Not Found");
                    return;
                }

                if (this.find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Not Found");
                    return;
                }

                this.index++;
                if (this.index >= this.find_dr.Length)
                {
                    this.index = 0;
                }
            }

            this.detailgridbs.Position = this.DetailDatas.IndexOf(this.find_dr[this.index]);
        }

        private void BtnBatchEncode_Click(object sender, EventArgs e)
        {
            P02_BatchEncode p02_BatchEncode = new P02_BatchEncode(this.CurrentMaintain["ID"].ToString());
            p02_BatchEncode.ShowDialog();
            this.RenewData();
            this.OnDetailEntered();
        }
    }
}
