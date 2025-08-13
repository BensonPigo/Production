using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Class;
using Sci.Production.Prg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P18
    /// </summary>
    public partial class P18 : Win.Tems.QueryForm
    {
        private enum ScanStatusResult
        {
            Success = 0,
            Error_NotFound = 1,
        }

        private DataTable dt_scanDetail;
        private SelectCartonDetail selecedPK;
        private bool UseAutoScanPack = false;
        private string PackingListID = string.Empty;
        private string CTNStarNo = string.Empty;
        private bool Boolfirst;
        private string MachineID = string.Empty;
        private RfidFX7500Reader rfidReader = new RfidFX7500Reader();
        private DataTable dt_epcData = new DataTable();

        public static System.Windows.Forms.Timer timer;

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
            this.Boolfirst = true;
            this.comboMDMachineID.DataSource = new List<string>();
            this.rfidReader = new RfidFX7500Reader(this.HandleRFIDScanResult, this);
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            #region combo Data Source設定
            DataTable dtMDCalibrationList;
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, @"
select row = 0, MachineID = '',operator = ''
union all
select row = ROW_NUMBER () over(order by MachineID), MachineID , operator
from MDMachineBasic
Where Junk = 0
", out dtMDCalibrationList))
            {
                this.comboMDMachineID.DataSource = dtMDCalibrationList;
                this.comboMDMachineID.DisplayMember = "MachineID";
                this.comboMDMachineID.ValueMember = "MachineID";
            }
            else
            {
                this.ShowErr(result);
            }

            // 根據userid來自動帶出MachineID
            DataRow[] drUserRowNB = dtMDCalibrationList.Select($@" operator = '{Env.User.UserID}'");
            if (drUserRowNB.Length > 0)
            {
                int row = MyUtility.Convert.GetInt(drUserRowNB[0]["row"]);
                this.comboMDMachineID.SelectedIndex = row;
                this.MachineID = this.comboMDMachineID.Text;
            }
            else
            {
                this.comboMDMachineID.SelectedIndex = 0;
            }
            #endregion

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
                .Text("HaulingScanTime", header: "Hauling Scan Time", width: Widths.AnsiChars(12))
                .Text("PassName", header: "Scanned by", width: Widths.AnsiChars(12))
                .Text("ActCTNWeight", header: "Actual CTN# Weight", width: Widths.AnsiChars(12));

            this.gridScanDetail.DataSource = this.scanDetailBS;
            this.Helper.Controls.Grid.Generator(this.gridScanDetail)
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(15))
                .Numeric("QtyPerCTN", header: "Qty")
                .Text("Barcode", header: "Hangtag Barcode", width: Widths.AnsiChars(15))
                .Numeric("ScanQty", header: "Scan Qty")
                .Text("MDStatus", header: "MD Status", width: Widths.AnsiChars(10))
                ;
            this.Tab_Focus("CARTON");

            // 重啟P18 必須重新判斷校正記錄
            this.txtDest.TextBox1.ReadOnly = true;

            // 啟動計時器
            this.timer1.Start();

            // RFID資料
            this.labRFIDReader.Text = string.Empty;
            this.btnRFIDReader.Visible = MyUtility.Check.Seek($"select 1 from System where RFIDReaderConnection=1", "ManufacturingExecution");
        }

        /// <inheritdoc/>
        protected override void OnFormDispose()
        {
            base.OnFormDispose();

            this.rfidReader.RFIDscanOff();
            this.rfidReader.RFIDdisConnect();
        }

        private void Disable_Carton_Scan()
        {
            if (this.chkAutoCalibration.Checked)
            {
                string machineID = this.MachineID;
                string sqlcmd = $@"
select * from MDCalibrationList where MachineID = '{machineID}' and CalibrationDate = CONVERT(date, GETDATE()) and operator = '{Sci.Env.User.UserID}' order by CalibrationTime desc";

                DataTable dtMDCalibrationList;
                Ict.DualResult result;
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out dtMDCalibrationList)))
                {
                    this.ShowErr(result);
                }

                DataRow[] MDCalibrationListChk = dtMDCalibrationList.Select(@"
   Point1 = 0
or Point2 = 0
or Point3 = 0
or Point4 = 0
or Point5 = 0
or Point6 = 0
or Point7 = 0
or Point8 = 0
or Point9 = 0
");

                if (MDCalibrationListChk.Length > 0)
                {
                    this.txtScanCartonSP.Enabled = false;
                    this.txtScanEAN.Enabled = false;
                    this.btnPackingError.Enabled = false;
                }
                else
                {
                    this.txtScanCartonSP.Enabled = true;
                    this.txtScanEAN.Enabled = true;
                    this.btnPackingError.Enabled = true;
                }
            }
        }

        private void alert_Calibration()
        {
            // 不在掃箱掃碼過程才動作!
            bool canScan = this.tabControlScanArea.SelectedIndex == 0;
            if (this.chkAutoCalibration.Checked && (this.Boolfirst || canScan))
            {
                string machineID = this.MachineID;
                string sqlcmd = $@"
select top 1 * from MDCalibrationList where MachineID = '{machineID}' and CalibrationDate = CONVERT(date, GETDATE()) and operator = '{Sci.Env.User.UserID}' order by CalibrationTime desc";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                {
                    // 全都勾選
                    if (!MyUtility.Check.Empty(dr["Point1"]) &&
                        !MyUtility.Check.Empty(dr["Point2"]) &&
                        !MyUtility.Check.Empty(dr["Point3"]) &&
                        !MyUtility.Check.Empty(dr["Point4"]) &&
                        !MyUtility.Check.Empty(dr["Point5"]) &&
                        !MyUtility.Check.Empty(dr["Point6"]) &&
                        !MyUtility.Check.Empty(dr["Point7"]) &&
                        !MyUtility.Check.Empty(dr["Point8"]) &&
                        !MyUtility.Check.Empty(dr["Point9"]) &&
                        !MyUtility.Check.Empty(dr["CalibrationTime"]))
                    {
                        int hh = MyUtility.Convert.GetInt(dr["CalibrationTime"].ToString().Substring(0, 2));
                        int mm = MyUtility.Convert.GetInt(dr["CalibrationTime"].ToString().Substring(3, 2));

                        this.Display_Calibration(hh, mm);
                        int currHH = DateTime.Now.Hour;
                        int currMM = DateTime.Now.Minute;

                        // 觸發Timer:
                        // 第一次開畫面
                        if (this.Boolfirst)
                        {
                            // 當前時間 > 設定時間一小時以上 代表未做校正記錄
                            if ((currHH - hh > 1) || (currHH - hh == 1 && currMM - mm > 0))
                            {
                                this.timer1.Stop();
                                this.AlterMSg();
                            }

                            this.Boolfirst = false;
                        }

                        // 時間要剛好是一小時整才會觸發
                        else if (currHH - hh == 1 && currMM - mm == 0)
                        {
                            this.timer1.Stop();
                            this.AlterMSg();
                        }
                    }
                }

                this.Boolfirst = false;
            }
        }

        private void AlterMSg()
        {
            MyUtility.Msg.WarningBox("Move to MD Hourly Calibration!");

            foreach (Form form in Application.OpenForms)
            {
                if (form is P18_Calibration_List)
                {
                    form.Activate();
                    return;
                }
            }

            P18_Calibration_List callForm = new P18_Calibration_List(true, string.Empty, string.Empty, string.Empty);
            callForm.ShowDialog(this);
            this.Disable_Carton_Scan();
            this.Display_Calibration(0, 0);
        }

        private void Display_Calibration(int HH, int MM)
        {
            // Machine 沒資料就不用顯示下一次時間
            if (MyUtility.Check.Empty(this.MachineID))
            {
                return;
            }

            if (HH == 0 && MM == 0)
            {
                string sqlcmd = $@"
select top 1 * from MDCalibrationList where MachineID = '{this.MachineID}' and CalibrationDate = CONVERT(date, GETDATE()) and operator = '{Sci.Env.User.UserID}' order by CalibrationTime desc";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                {
                    // 全都勾選
                    if (!MyUtility.Check.Empty(dr["Point1"]) &&
                        !MyUtility.Check.Empty(dr["Point2"]) &&
                        !MyUtility.Check.Empty(dr["Point3"]) &&
                        !MyUtility.Check.Empty(dr["Point4"]) &&
                        !MyUtility.Check.Empty(dr["Point5"]) &&
                        !MyUtility.Check.Empty(dr["Point6"]) &&
                        !MyUtility.Check.Empty(dr["Point7"]) &&
                        !MyUtility.Check.Empty(dr["Point8"]) &&
                       !MyUtility.Check.Empty(dr["CalibrationTime"]))
                    {
                        string hh = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(dr["CalibrationTime"].ToString().Substring(0, 2)) + 1);
                        string mm = dr["CalibrationTime"].ToString().Substring(3, 2);
                        this.lbCalibrationTime.Text = $@"Next Calibration Time : {hh}:{mm}";
                    }
                }
            }
            else
            {
                if (MM < 10)
                {
                    this.lbCalibrationTime.Text = $@"Next Calibration Time : {HH + 1}:0{MM}";
                }
                else
                {
                    this.lbCalibrationTime.Text = $@"Next Calibration Time : {HH + 1}:{MM}";
                }
            }
        }

        private void TxtScanCartonSP_Validating(object sender, CancelEventArgs e)
        {
            DualResult result;
            this.PackingListID = string.Empty;
            this.CTNStarNo = string.Empty;

            // 掃碼階段btnCalibrationList 不能啟用
            this.ShowCalibrationButton();

            if (MyUtility.Check.Empty(this.txtScanCartonSP.Text))
            {
                this.dt_epcData.Clear();
                return;
            }

            // Check MD Machine#
            if (MyUtility.Check.Empty(this.comboMDMachineID.Text))
            {
                MyUtility.Msg.WarningBox("Please select MD Machine#!!");
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
            else
            {
                this.dt_epcData.Clear();
            }

            if (this.txtScanCartonSP.Text.Length > 13)
            {
                this.PackingListID = this.txtScanCartonSP.Text.Substring(0, 13);
                this.CTNStarNo = this.txtScanCartonSP.Text.Substring(13, this.txtScanCartonSP.Text.Length - 13).TrimStart('^');
            }

            // 換箱清空更新barcode字串
            this.upd_sql_barcode = string.Empty;
            this.intTmpNo = 0;

            this.ClearAll("SCAN");
            #region 檢查是否有資料，三個角度
            if (!this.LoadDatas())
            {
                return;
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

                if (this.UseAutoScanPack && MyUtility.Check.Seek($"SELECT 1 FROM PackingList_Detail WHERE ID='{this.PackingListID}' AND CTNStartNo='{this.CTNStarNo}' AND (Barcode = '' OR Barcode IS NULL) "))
                {
                    foreach (DataRow dr in this.dt_scanDetail.Rows)
                    {
                        dr["barcode"] = DBNull.Value;
                    }

                    string cmd = $@"update PackingList_Detail set barcode = null where ID = '{this.PackingListID}' and  CTNStartNo = '{this.CTNStarNo}'";
                    DBProxy.Current.Execute(null, cmd);
                    this.TaskCallWebAPI(this.PackingListID);
                }

                DualResult result_load = this.LoadScanDetail(0);
                if (!result_load)
                {
                    this.ShowErr(result_load);
                }
            }

            if (this.chkAutoScan.Checked)
            {
                DualResult result_load = this.LoadScanDetail(0);
            }

            if (this.btnRFIDReader.Visible && this.selcartonBS.DataSource != null && this.dt_scanDetail.Rows[0]["BrandID"].ToString() == "NIKE")
            {
                List<string> poList = this.dt_scanDetail.AsEnumerable()
                    .Select(s => "'" + s["CustPoNo"].ToString().Substring(0, 10) + "'")
                    .Distinct()
                    .ToList();
                if (!(result = DBProxy.Current.Select("ManufacturingExecution", $"select EPC,UPC from EPCData WITH (NOLOCK) where PO in ({string.Join(",", poList)})", out this.dt_epcData)))
                {
                    this.StopRFIDReaderScan();
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion
        }

        private bool LoadDatas()
        {
            /*注意！  這裡有異動的話，要注意Reset()是否需要同步異動*/

            // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
            // 2.=Orders.ID
            // 3.=Orders.CustPoNo
            string[] aLLwhere = new string[]
            {
                this.txtScanCartonSP.Text.Length > 13 ? $" and  pd.ID = '{this.PackingListID}' and  pd.CTNStartNo = '{this.CTNStarNo}'" : " and 1=0 ",
                $" and  pd.SCICtnNo = '{this.txtScanCartonSP.Text.GetPackScanContent()}'",
                $" and  pd.ID = '{this.txtScanCartonSP.Text}'",
                $" and o.ID = '{this.txtScanCartonSP.Text}' or o.CustPoNo = '{this.txtScanCartonSP.Text}'",
                $" and pd.CustCTN = '{this.txtScanCartonSP.Text}'",
            };

            string scanDetail_sql = $@"
select distinct
    pd.ID,
    pd.CTNStartNo,
    pd.OrderID,
    o.CustPoNo,
    pd.Article,
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
    isnull(iif(ps.name is null, convert(nvarchar(10),pd.ScanEditDate,120), ps.name+'-'+convert(nvarchar(10),pd.ScanEditDate,120)),'') as PassName
    ,p.Remark
	,pd.Ukey
	,[IsFirstTimeScan] = Cast(1 as bit)
    ,o.CustCDID
    ,[MDStatus] = IIF(pd.ScanPackMDDate is null, '1st MD', '2rd MD')
    ,[HaulingScanTime] = IIF(s.RgCode='PH2',Null,(SELECT TOP 1 FORMAT(CTN.AddDate, 'yyyy-MM-dd HH:mm') FROM CTNHauling CTN WITH(NOLOCK) WHERE CTN.PackingListID = pd.ID AND CTN.OrderID = pd.OrderID AND CTN.CTNStartNo = pd.CTNStartNo ORDER BY CTN.AddDate DESC))
    ,pd.Color
from PackingList_Detail pd WITH (NOLOCK)
inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode 
left join pass1 ps WITH (NOLOCK) on pd.ScanName = ps.id
cross join [system] s WITH (NOLOCK)
where p.Type in ('B','L')
";

            foreach (string where in aLLwhere)
            {
                DualResult result = DBProxy.Current.Select(null, scanDetail_sql + where, out this.dt_scanDetail);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                if (this.dt_scanDetail.Rows.Count > 0)
                {
                    break;
                }
            }

            return true;
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
            if (rowidx == 0 && this.chkAutoScan.Checked)
            {
                string nowPKseq = "0";
                if (this.gridSelectCartonDetail != null && this.selcartonBS != null && this.selcartonBS.DataSource != null && this.selcartonBS.Current != null)
                {
                    nowPKseq = MyUtility.Convert.GetString(((SelectCartonDetail)this.selcartonBS.Current).PKseq);
                }

                var scanFirst = this.dt_scanDetail.AsEnumerable()
                       .GroupBy(g => new { ID = MyUtility.Convert.GetString(g["ID"]), CTNStartNo = MyUtility.Convert.GetString(g["CTNStartNo"]) })
                       .Select(s => new
                       {
                           s.Key.ID,
                           s.Key.CTNStartNo,
                           ScanQty = s.Sum(su => MyUtility.Convert.GetInt(su["ScanQty"])),
                           QtyPerCTN = s.Sum(su => MyUtility.Convert.GetInt(su["QtyPerCTN"])),
                           PKseq = s.Min(f => MyUtility.Convert.GetString(f["PKseq"])),
                       })
                       .Where(w => w.QtyPerCTN > w.ScanQty && MyUtility.Convert.GetInt(w.PKseq) >= MyUtility.Convert.GetInt(nowPKseq))
                       .OrderBy(o => o.PKseq)
                       .FirstOrDefault();

                var obj = this.selcartonBS.List.OfType<SelectCartonDetail>().ToList().Find(f => f.CTNStartNo.Equals(scanFirst.CTNStartNo) && f.ID == scanFirst.ID);
                rowidx = this.selcartonBS.IndexOf(obj);
            }

            SelectCartonDetail dr = (SelectCartonDetail)this.gridSelectCartonDetail.GetData(rowidx);
            if (dr == null)
            {
                return result;
            }

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

                if (!this.LoadDatas())
                {
                    return result;
                }

                this.LoadSelectCarton();
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

                string cmd = $@"update PackingList_Detail set barcode = null where ID = '{MyUtility.Convert.GetString(dr.ID)}' AND CTNStartNo='{MyUtility.Convert.GetString(dr.CTNStartNo)}' ";
                DBProxy.Current.Execute(null, cmd);
                this.TaskCallWebAPI(dr.ID);
            }

            if (this.selecedPK == null || dr.CTNStartNo != this.selecedPK.CTNStartNo)
            {
                this.CleanRFIDTgsData();
            }

            this.scanDetailBS.DataSource = dr_scanDetail.OrderBy(s => s["Article"]).ThenBy(s => s["Seq"]).CopyToDataTable();
            this.LoadHeadData(dr);
            this.Tab_Focus("EAN");
            this.selcartonBS.Position = rowidx;

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

            // 換箱清空更新barcode字串
            this.upd_sql_barcode = string.Empty;
            this.intTmpNo = 0;
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
WHERE ID = '{packingListID}' AND CTNStartNo = '{cTNStartNo}' 
";

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
                                                              HaulingScanTime = g.Select(st => st.Field<string>("HaulingScanTime")).FirstOrDefault(),
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
                                                              CustCD = string.Join("/", g.Select(st => st.Field<string>("CustCDID")).Distinct().ToArray()),
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
                this.ShowCalibrationButton();
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
            this.displayCustCD.Text = dr.CustCD;
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
            this.chkVasShas.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"SELECT VasShas FROM Orders WHERE ID = '{dr.OrderID}'"));
            this.chkBrokenneedles.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"SELECT BrokenNeedles FROM Orders WHERE ID = '{dr.OrderID}'"));
            if (this.chkBrokenneedles.Checked)
            {
                MyUtility.Msg.InfoBox($"PO {dr.CustPoNo} ever broken needle in production line");
            }
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
        private int intTmpNo = 0;

        private void TxtScanEAN_Validating(object sender, CancelEventArgs e)
        {
            this.Check_ScanEAN_Validated(e, "Validating");
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
                this.displayCustCD.Text = string.Empty;
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
                this.CleanRFIDTgsData();
            }
            else if (type.Equals("SCAN"))
            {
                this.numBoxScanQty.Text = string.Empty;
                this.numBoxScanTtlQty.Text = string.Empty;
                this.displayPackID.Text = string.Empty;
                this.displayCtnNo.Text = string.Empty;
                this.displaySPNo.Text = string.Empty;
                this.displayCustCD.Text = string.Empty;
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
                this.CleanRFIDTgsData();
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

            /// <summary>
            /// CustCD
            /// </summary>
            public string CustCD { get; set; }

            public string HaulingScanTime { get; set; }
        }

        /// <inheritdoc/>
        public static string P18_PackingError { get; set; }

        /// <inheritdoc/>
        public static int P18_PackingErrorQty { get; set; }

        /// <inheritdoc/>
        public static string P18_PackingAuditQC { get; set; }

        private void BtnLacking_Click(object sender, EventArgs e)
        {
            var frm = new P18_PackingError();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.UpdateLackingStatus();
            }

            P18_PackingError = string.Empty;
            P18_PackingErrorQty = 0;
            P18_PackingAuditQC = string.Empty;
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
ScanQty = {(MyUtility.Check.Empty(dr["ScanQty"]) ? "0" : dr["ScanQty"])} 
, ScanEditDate = GETDATE()
, ScanName = '{Env.User.UserID}'   
, Lacking = 1
, BarCode = '{dr["Barcode"]}'
, PackingReasonERID = '{P18_PackingError}'
, ErrQty = '{P18_PackingErrorQty}'
, AuditQCName = '{P18_PackingAuditQC}'
, ScanPackMDDate = IIF(ScanPackMDDate is null, GETDATE(), ScanPackMDDate)
, ClogScanPackMDDate = IIF(ScanPackMDDate is not null , GETDATE(), ClogScanPackMDDate)
output	inserted.ID
into #tmpUpdatedID
where Ukey={dr["Ukey"]}

";
                }

                upd_sql += @"select distinct ID from #tmpUpdatedID
drop table #tmpUpdatedID
";
                DataTable dtUpdateID;
                sql_result = DBProxy.Current.Select(null, upd_sql, out dtUpdateID);
                if (!sql_result)
                {
                    this.ShowErr(sql_result);
                    return;
                }

                if (dtUpdateID.Rows.Count > 0)
                {
                    List<string> listID = dtUpdateID.AsEnumerable().Select(s => s["ID"].ToString()).ToList();
                    this.TaskCallWebAPI(listID);
                }

                this.AfterCompleteScanCarton();

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
            DataRow drPassName;
            string passName = string.Empty;

            // 掃描完成後要重新撈一次ScanName，存到Form的DataTable
            string scanName = string.Empty;

            string sql = $@"
            select  ScanName, isnull(iif(ps.name is null, convert(nvarchar(10),pd.ScanEditDate,112), ps.name+'-'+convert(nvarchar(10),pd.ScanEditDate,120)),'') as PassName,
                    pd.ScanEditDate
            from PackingList_Detail pd WITH (NOLOCK)
            left join pass1 ps WITH (NOLOCK) on pd.ScanName = ps.id
            where pd.id = '{this.selecedPK.ID}' 
            and pd.CTNStartNo = '{this.selecedPK.CTNStartNo}'";

            using (new MethodWatch(20, "Packing.P18_AfterCompleteScanCarton"))
            {
                if (MyUtility.Check.Seek(sql, out drPassName))
                {
                    passName = MyUtility.Convert.GetString(drPassName["PassName"]);
                    scanName = MyUtility.Convert.GetString(drPassName["ScanName"]);
                }
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
                else if (resultLacking == DialogResult.No)
                {
                    return false;
                }

                this.CleanRFIDTgsData();
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
                this.CTNStarNo = this.txtScanCartonSP.Text.Substring(13, this.txtScanCartonSP.Text.Length - 13).TrimStart('^');
            }

            // 換箱清空更新barcode字串
            this.upd_sql_barcode = string.Empty;
            this.intTmpNo = 0;
            this.ClearAll("SCAN");

            if (this.txtScanCartonSP.Text.Length > 13)
            {
                this.PackingListID = this.txtScanCartonSP.Text.Substring(0, 13);
                this.CTNStarNo = this.txtScanCartonSP.Text.Substring(13, this.txtScanCartonSP.Text.Length - 13).TrimStart('^');
            }

            // 換箱清空更新barcode字串
            this.upd_sql_barcode = string.Empty;
            this.intTmpNo = 0;
            this.ClearAll("SCAN");
            #region 檢查是否有資料，三個角度

            // 1.=PackingList_Detail.ID+PackingList_Detail.CTNStartNo
            // 2.=Orders.ID
            // 3.=Orders.CustPoNo
            string[] aLLwhere = new string[]
            {
                this.txtScanCartonSP.Text.Length > 13 ? $" and  pd.ID = '{this.PackingListID}' and  pd.CTNStartNo = '{this.CTNStarNo}'" : " and 1=0 ",
                $" and  pd.SCICtnNo  = '{this.txtScanCartonSP.Text.GetPackScanContent()}'",
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
                                           isnull(iif(ps.name is null, convert(nvarchar(10),pd.ScanEditDate,120), ps.name+'-'+convert(nvarchar(10),pd.ScanEditDate,120)),'') as PassName
                                           ,p.Remark
										   ,pd.Ukey
										   ,[IsFirstTimeScan] = Cast(1 as bit)
                                           ,o.CustCDID
                                           ,[MDStatus] = IIF(pd.ScanPackMDDate is null, '1st MD', '2rd MD')
                                           ,[HaulingScanTime] = IIF(s.RgCode='PH2',Null,(SELECT TOP 1 FORMAT(CTN.AddDate, 'yyyy-MM-dd HH:mm') FROM CTNHauling CTN WITH(NOLOCK) WHERE CTN.PackingListID = pd.ID AND CTN.OrderID = pd.OrderID AND CTN.CTNStartNo = pd.CTNStartNo ORDER BY CTN.AddDate DESC))
                                from PackingList_Detail pd WITH (NOLOCK)
                                inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
                                inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
                                left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = pd.SizeCode 
                                left join pass1 ps WITH (NOLOCK) on pd.ScanName = ps.id
                                cross join [system] s WITH (NOLOCK)
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

                // 確認該箱都有設定Barcode，只要有缺少，就清空該箱Barcode
                if (this.UseAutoScanPack && MyUtility.Check.Seek($"SELECT 1 FROM PackingList_Detail WHERE ID='{this.PackingListID}' AND CTNStartNo='{this.CTNStarNo}' AND (Barcode = '' OR Barcode IS NULL) "))
                {
                    foreach (DataRow dr in this.dt_scanDetail.Rows)
                    {
                        dr["Barcode"] = DBNull.Value;
                    }

                    string cmd = $@"update PackingList_Detail set barcode = null where ID = '{this.PackingListID}' and  CTNStartNo = '{this.CTNStarNo}'";
                    DBProxy.Current.Execute(null, cmd);
                    this.TaskCallWebAPI(this.PackingListID);
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
            this.Check_ScanEAN_Validated(null, "Leave");
        }

        private void TaskCallWebAPI(string id)
        {
            List<string> listID = new List<string>();
            listID.Add(id);
            this.TaskCallWebAPI(listID);
        }

        private void TaskCallWebAPI(List<string> listID)
        {
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(listID.JoinToString(","), string.Empty))
                       .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            // 因為會傳圖片，拆成單筆 PackingListNo 轉出，避免一次傳出的容量過大超過api大小限制
            foreach (string id in listID)
            {
                #region ISP20201607 資料交換 - Gensong
                if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
                {
                    // 不透過Call API的方式，自己組合，傳送API
                    Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(id, string.Empty))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                }
                #endregion
            }
        }

        private void btnCalibrationList_Click(object sender, EventArgs e)
        {
            // Check MD Machine#
            if (MyUtility.Check.Empty(this.comboMDMachineID.Text))
            {
                MyUtility.Msg.WarningBox("Please select MD Machine#!!");
                this.comboMDMachineID.Select();
                return;
            }

            P18_Calibration_List callForm = new P18_Calibration_List(true, this.comboMDMachineID.Text, string.Empty, string.Empty);
            callForm.ShowDialog(this);
            this.Disable_Carton_Scan();
            this.Display_Calibration(0, 0);

            // 啟動計時器
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 每15秒跑一次
            timer = this.timer1;
            this.timer1.Interval = 1000 * 15;
            this.alert_Calibration();
        }

        private void chkAutoCalibration_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAutoCalibration != null)
            {
                if (this.chkAutoCalibration.Checked == true)
                {
                    this.lbCalibrationTime.Visible = true;
                    this.Display_Calibration(0, 0);
                }
                else
                {
                    this.lbCalibrationTime.Visible = false;
                    this.txtScanCartonSP.Enabled = true;
                    this.txtScanEAN.Enabled = true;
                    this.btnPackingError.Enabled = true;
                }
            }
        }

        private void tabControlScanArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowCalibrationButton();
        }

        private void ShowCalibrationButton()
        {
            // 掃碼階段btnCalibrationList 不能啟用
            if (this.tabControlScanArea.SelectedIndex == 0)
            {
                this.btnCalibrationList.Enabled = true;
            }
            else
            {
                this.btnCalibrationList.Enabled = false;
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
where   a.ID ='{this.selecedPK.ID}'
        AND a.CTNStartNo =  '{this.selecedPK.CTNStartNo}'
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

--抓出有更新的PKID，作為後續call WebAPI 更新廠商資料用
select distinct pd.ID
from  PackingList_Detail pd
where pd.Ukey in (select Ukey from #tmpNeedUpdPackUkeys{this.intTmpNo})

drop table #tmpNeedUpdateGroup{this.intTmpNo}, #tmpNeedUpdPackUkeys{this.intTmpNo}

";
        }

        /// <summary>
        /// 處理RFDI掃描結果，可能是多筆
        /// </summary>
        private void HandleRFIDScanResult(DataTable scannedTags)
        {
            StringBuilder insertScanSql = new StringBuilder();
            bool isEPCNotFound = false;
            bool isEPCVaildError = false;

            if (this.selecedPK == null)
            {
                return;
            }

            try
            {
                foreach (DataRow row in scannedTags.Rows)
                {
                    string tagId = row["tagID"].ToString();
                    string errroMsg = string.Empty;
                    string upc = string.Empty;

                    string binary_LLL = string.Empty;
                    string header_LLL = string.Empty;
                    string productCode_LLL = string.Empty;
                    string serialNumber_LLL = string.Empty;
                    string vendorCode_LLL = string.Empty;

                    // selecedPK
                    string packingID = this.selecedPK.ID;
                    string ctnStartNo = this.selecedPK.CTNStartNo;
                    string brandID = this.selecedPK.BrandId;
                    string sizecode = this.selecedPK.SizeCode;
                    DataTable dt_scanDetail = this.dt_scanDetail.Select($"ID = '{packingID}' and CTNStartNo = '{ctnStartNo}' ").CopyToDataTable();

                    switch (this.selecedPK.BrandId)
                    {
                        case "LLL":
                            tagId = tagId.Substring(0, 24);
                            this.HandleRFIDLLLScanResult(tagId, out upc, out binary_LLL, out header_LLL, out productCode_LLL, out serialNumber_LLL, out vendorCode_LLL);
                            break;
                        case "NIKE":
                            this.HandleRFIDNikeScanResult(tagId, ref upc);
                            break;
                    }

                    if (!string.IsNullOrEmpty(upc))
                    {
                        this.txtScanEAN.Text = upc;
                        errroMsg = this.Check_ScanEAN_Validated(null, "Validating", true);
                        this.txtScanEAN.Text = string.Empty; // 避免觸發到 Leave事件

                        if (!string.IsNullOrEmpty(errroMsg))
                        {
                            isEPCVaildError = true;
                        }
                    }
                    else
                    {
                        isEPCNotFound = true;
                    }

                    // 0 : UPC有對應到Barcode
                    // 1 : UPC沒有對應到Barcode
                    int status = (int)ScanStatusResult.Success;
                    if (string.IsNullOrEmpty(upc) || !string.IsNullOrEmpty(errroMsg))
                    {
                        status = (int)ScanStatusResult.Error_NotFound;
                    }

                    insertScanSql.AppendLine($@"INSERT INTO EPCDataScanRecord
(
BrandID,
EPC,
ScanTime,
UPC,
PackingID,
CTNStartNo,
Colorway,
Color,
SizeCode,
Status,
Binary,
Header,
ProductCode,
SerialNumber,
VendorCode)
 VALUES
 ('{brandID}',
'{tagId}',
GETDATE(),
'{upc}',
'{packingID}',
'{ctnStartNo}',
'{dt_scanDetail.Rows[0]["Article"].ToString()}',
'{dt_scanDetail.Rows[0]["Color"].ToString()}',
'{sizecode}',
{status},
'{binary_LLL}',
'{header_LLL}',
'{productCode_LLL}',
'{serialNumber_LLL}',
'{vendorCode_LLL}');");
                }
            }
            catch (Exception ex)
            {
                this.StopRFIDReaderScan();
                MyUtility.Msg.ErrorBox($"Error processing scan results: {ex.Message}");
            }

            if (isEPCNotFound)
            {
                string msg = this.selecedPK.BrandId == "LLL" ? "Unable to decode this EPC to UPC.\r\nPlease scan the barcode manually." : "EPC Data not found";
                MyUtility.Msg.ErrorBox(msg);
            }

            if (isEPCNotFound || isEPCVaildError)
            {
                this.StopRFIDReaderScan();
            }

            if (insertScanSql.Length > 0)
            {
                Ict.DualResult dbResult = DBProxy.Current.Execute("ManufacturingExecution", insertScanSql.ToString());
                if (!dbResult)
                {
                    MyUtility.Msg.ErrorBox($"Failed to insert AutomationCheckMsg data: {dbResult.ToMessages()}");
                }
            }
        }

        private void HandleRFIDNikeScanResult(string tagId, ref string upc)
        {
            List<DataRow> epcRowList = this.dt_epcData.AsEnumerable()
                    .Where(s => s.Field<string>("EPC").ToUpper() == tagId.ToUpper())
                    .ToList();

            if (epcRowList.Count > 0)
            {
                upc = MyUtility.Convert.GetString(epcRowList[0]["UPC"]);
            }
        }

        private void HandleRFIDLLLScanResult(string tagId, out string upc, out string binary, out string headerBits, out string productBits, out string serialBits, out string vendorCode)
        {
            upc = string.Empty;
            vendorCode = string.Empty;

            // 用 BigInteger 處理 96-bit 大數
            System.Numerics.BigInteger epcValue = System.Numerics.BigInteger.Parse("0" + tagId, System.Globalization.NumberStyles.HexNumber);
            binary = this.ToBinaryString(epcValue, 96);

            // 取三段 binary 欄位
            headerBits = binary.Substring(0, 12);
            productBits = binary.Substring(12, 50);
            serialBits = binary.Substring(62, 34);

            // 驗證 header
            if (headerBits != "001110111101")
            {
                return;
            }
            else
            {
                ulong serialNumber = Convert.ToUInt64(serialBits, 2);
                string serialStr = serialNumber.ToString().PadLeft(11, '0'); // 補滿 11 碼（如果太短）
                vendorCode = serialStr.Substring(0, 2);
                upc = Convert.ToUInt64(productBits, 2).ToString().PadLeft(12, '0');
            }
        }

        /// <summary>
        /// 將value轉換為指定位數的二進位字串表示形式
        /// </summary>
        private string ToBinaryString(System.Numerics.BigInteger value, int bits)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = bits - 1; i >= 0; i--)
            {
                sb.Append((value & (System.Numerics.BigInteger.One << i)) != 0 ? '1' : '0');
            }

            return sb.ToString();
        }

        /// <summary>
        /// 檢查EAN是否有效
        /// </summary>
        /// <param name="fromRFID">是否是從RFID</param>
        private string Check_ScanEAN_Validated(CancelEventArgs e, string eventAction, bool fromRFID = false)
        {
            if (MyUtility.Check.Empty(this.txtScanEAN.Text))
            {
                return string.Empty;
            }
            else
            {
                this.txtScanEAN.Text = this.txtScanEAN.Text.Replace("[", string.Empty).Replace("]", string.Empty);
            }

            if (this.scanDetailBS.DataSource == null)
            {
                return string.Empty;
            }

            // Check MD Machine#
            if (MyUtility.Check.Empty(this.comboMDMachineID.Text))
            {
                MyUtility.Msg.WarningBox("Please select MD Machine#!!");
                return string.Empty;
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

                    msg.Show($"<{this.txtScanEAN.Text}> Invalid barcode !!");
                    this.txtScanEAN.Text = string.Empty;

                    if (e != null)
                    {
                        e.Cancel = true;
                    }

                    return fromRFID ? "NoBarCode" : string.Empty;
                }
                else
                {
                    if (fromRFID)
                    {
                        return fromRFID ? "NoBarCode" : string.Empty;
                    }

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
                            if (e != null)
                            {
                                e.Cancel = true;
                            }

                            return string.Empty;
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
                            dr["ScanQty"] = (short)dr["ScanQty"] + 1;

                            if (eventAction == "Validating")
                            {
                                // 變更是否為第一次掃描的標記
                                dr["IsFirstTimeScan"] = false;
                            }

                            this.UpdScanQty((long)dr["Ukey"], (string)dr["Barcode"]);
                            break;
                        }
                    }
                }
            }
            else
            {
                this.scanDetailBS.Position = barcode_pos;
                DataRowView cur_dr = (DataRowView)this.scanDetailBS.Current;
                int scanQty = (short)cur_dr["ScanQty"];
                int qtyPerCTN = (int)cur_dr["QtyPerCTN"];

                if (eventAction == "Validating")
                {
                    // 判斷該Barcode是否為第一次掃描，是的話傳送指令避免停下
                    bool isFirstTimeScan = ((DataTable)this.scanDetailBS.DataSource).AsEnumerable().Where(s => MyUtility.Convert.GetString(s["Barcode"]) == this.txtScanEAN.Text.Trim() && MyUtility.Convert.GetBool(s["IsFirstTimeScan"])).Any();

                    if (isFirstTimeScan && this.UseAutoScanPack)
                    {
                        // 變更是否為第一次掃描的標記
                        cur_dr["IsFirstTimeScan"] = false;
                    }
                }

                if (scanQty >= qtyPerCTN)
                {
                    AutoClosingMessageBox.Show($"This Size scan is complete,can not scan again!!", "Warning", 3000);
                    this.txtScanEAN.Text = string.Empty;
                    if (e != null)
                    {
                        e.Cancel = true;
                    }

                    return string.Empty;
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
                    DataTable dtUpdateID;

                    if (eventAction == "Validating")
                    {
                        using (new MethodWatch(30, "WH.P18_UpdBarCode"))
                        {
                            if (!(sql_result = DBProxy.Current.Select(null, this.upd_sql_barcode, out dtUpdateID)))
                            {
                                this.ShowErr(sql_result);
                                return string.Empty;
                            }
                        }

                        if (dtUpdateID.Rows.Count > 0)
                        {
                            List<string> listID = dtUpdateID.AsEnumerable().Select(s => s["ID"].ToString()).ToList();
                            using (new MethodWatch(50, "WH.P18_TaskCallWebAPI"))
                            {
                                this.TaskCallWebAPI(listID);
                            }
                        }
                    }
                    else
                    {
                        if (!(sql_result = DBProxy.Current.Select(null, this.upd_sql_barcode, out dtUpdateID)))
                        {
                            this.ShowErr(sql_result);
                            return string.Empty;
                        }

                        if (dtUpdateID.Rows.Count > 0)
                        {
                            List<string> listID = dtUpdateID.AsEnumerable().Select(s => s["ID"].ToString()).ToList();
                            this.TaskCallWebAPI(listID);
                        }
                    }
                }

                bool isNeedShowWeightInputWindow = this.chk_AutoCheckWeight.Checked;

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
                , ScanPackMDDate = IIF(ScanPackMDDate is null, GETDATE(), ScanPackMDDate)
                , ClogScanPackMDDate = IIF(ScanPackMDDate is not null , GETDATE(), ClogScanPackMDDate)
                , MDMachineNo = '{this.comboMDMachineID.Text}'
                where id = '{this.selecedPK.ID}' 
                and CTNStartNo = '{this.selecedPK.CTNStartNo}' 

                ";
                sql_result = DBProxy.Current.Execute(null, upd_sql);
                if (!sql_result)
                {
                    this.ShowErr(sql_result);
                    return string.Empty;
                }

                this.AfterCompleteScanCarton();

                if (eventAction == "Validating")
                {
                    DualResult result_load = this.LoadScanDetail(0);
                    if (!result_load)
                    {
                        this.ShowErr(result_load);
                    }
                }

                if (e != null)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                // 讓遊標停留在原地
                if (e != null)
                {
                    e.Cancel = true;
                }

                if (eventAction == "Leave")
                {
                    this.txtScanEAN.Focus();
                }
            }

            return string.Empty;
        }

        private void StopRFIDReaderScan()
        {
            if (this.rfidReader.IsScanOn)
            {
                this.rfidReader.RFIDscanOff();
                this.btnRFIDReader.Text = "RFID Connect";
                this.labRFIDReader.Text = "RFID Not Connected";
            }
        }

        /// <summary>
        /// 清空RFID 暫存資料
        /// </summary>
        private void CleanRFIDTgsData()
        {
            this.rfidReader.tagTable.Clear();
        }

        private void BtnRFIDReader_Click(object sender, EventArgs e)
        {
            if (this.rfidReader.IsScanOn)
            {
                this.rfidReader.RFIDscanOff();
                this.btnRFIDReader.Text = "RFID Connect";
                this.labRFIDReader.Text = "RFID Not Connected";
                return;
            }

            if (this.rfidReader.IsScanOn)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.comboMDMachineID.Text))
            {
                MyUtility.Msg.WarningBox("Please select MD Machine#!!");
                return;
            }

            this.labRFIDReader.Text = string.Empty;
            this.labRFIDReader.Refresh();

            DualResult result = new DualResult(true);
            if (!this.rfidReader.IsConnect)
            {
                result = this.rfidReader.RFIDConnect();
            }

            if (result)
            {
                result = this.rfidReader.RFIDscanOn();
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToMessages().ToString());
                    this.labRFIDReader.Text = "RFID Not Connected";
                    return;
                }

                this.labRFIDReader.Text = "RFID Connected";
                this.btnRFIDReader.Text = "RFID Disconnect";
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToMessages().ToString());
                this.labRFIDReader.Text = "RFID Not Connected";
            }
        }
    }
}
