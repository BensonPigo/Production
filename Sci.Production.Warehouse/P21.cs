﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P21 : Win.Tems.QueryForm
    {
        private DataTable dtReceiving = new DataTable();

        /// <inheritdoc/>
        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add(string.Empty, "All");
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessory");

            this.cmbBarcoedType.SelectedIndex = 0;
            string country = MyUtility.GetValue.Lookup($"select CountryID from Factory where ID = '{Env.User.Factory}'");
            if (country != "PH")
            {
                this.cmbBarcoedType.SelectedIndex = 1;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.dateTimePicker.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.dateTimeFabric2LabBy.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimeFabric2LabBy.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker2.Format = DateTimePickerFormat.Custom;
            this.ChangeCutShadebandTime();

            DataGridViewGeneratorNumericColumnSettings cellActWeight = new DataGridViewGeneratorNumericColumnSettings();
            cellActWeight.CellValidating += (s, e) =>
            {
                DataRow curDr = this.gridReceiving.GetDataRow(e.RowIndex);
                if (curDr["ReceivingSource"].ToString() != "Receiving")
                {
                    return;
                }

                curDr["Differential"] = (decimal)e.FormattedValue - (decimal)curDr["Weight"];
                curDr["ActualWeight"] = e.FormattedValue;
                curDr.EndEdit();

                this.DifferentialColorChange(e.RowIndex);
                this.SelectModify(e.RowIndex);
                this.gridReceiving.RefreshEdit();
            };

            DataGridViewGeneratorTextColumnSettings cellLocation = new DataGridViewGeneratorTextColumnSettings();
            cellLocation.CellMouseDoubleClick += (s, e) =>
            {
                this.GridLocationCellPop(e.RowIndex);
            };

            cellLocation.EditingMouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.GridLocationCellPop(e.RowIndex);
                }
            };

            cellLocation.CellValidating += (s, e) =>
            {
                DataRow curDr = this.gridReceiving.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    curDr["Location"] = string.Empty;
                    this.SelectModify(e.RowIndex);
                    return;
                }

                string[] locationList = e.FormattedValue.ToString().Split(',');

                string notLocationExistsList = locationList.Where(a => !Prgs.CheckLocationExists(curDr["StockType"].ToString(), a)).JoinToString(",");

                if (!MyUtility.Check.Empty(notLocationExistsList))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"Location<{notLocationExistsList}> not Found");
                    return;
                }
                else
                {
                    curDr["Location"] = e.FormattedValue.ToString();
                    curDr.EndEdit();
                }

                this.SelectModify(e.RowIndex);
                this.gridReceiving.RefreshEdit();
            };

            DataGridViewGeneratorCheckBoxColumnSettings col_Select = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_Select.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridReceiving.GetDataRow(e.RowIndex);
                bool isCheck = MyUtility.Convert.GetBool(e.FormattedValue);
                dr["select"] = isCheck;
                dr.EndEdit();
                DataTable dt = (DataTable)this.gridReceiving.DataSource;
                if (dt != null || dt.Rows.Count > 0)
                {
                    int cnt = MyUtility.Convert.GetInt(dt.Compute("count(select)", "select = 1")); // + (isCheck ? 1 : -1);
                    this.numSelectCnt.Value = cnt;
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridReceiving)
                 .CheckBox("select", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Select)
                 .Text("ExportID", header: "WK#", width: Widths.AnsiChars(14), iseditingreadonly: true)

                 // .Text("ID", header: "Receiving ID", width: Widths.AnsiChars(14), iseditingreadonly: true)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("Seq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("WeaveTypeID", header: "Weave Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Button(propertyname: "Barcode", header: "Print Barcode", width: Widths.AnsiChars(16), onclick: this.PrintBarcode)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("StockQty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                 .Text("StockTypeDesc", header: "Stock Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .DateTime("CutShadebandTime", header: "Cut Shadeband Time", width: Widths.AnsiChars(20), iseditingreadonly: false)
                 .Text("CutBy", header: "Cut Shadeband By", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .DateTime("Fabric2LabTime", header: "Fabric to Lab Time", width: Widths.AnsiChars(20), iseditingreadonly: false)
                 .Text("Fabric2LabBy", header: "Fabric to Lab By", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Location", header: "Location", width: Widths.AnsiChars(12), settings: cellLocation)
                 .Text("Checker", header: "Checker", width: Widths.AnsiChars(12))
                 .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                 .Numeric("ActualWeight", header: "Act.(kg)", width: Widths.AnsiChars(8), decimal_places: 2, settings: cellActWeight)
                 .Numeric("Differential", header: "Differential", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15))
                 .Text("LastRemark", header: "Last P26 Remark data", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .DateTime("LastEditDate", header: "Last Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("rid", header: "rid", width: Widths.AnsiChars(8), decimal_places: 0, iseditingreadonly: true)
                 ;

            this.gridReceiving.Columns["Location"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["Checker"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["ActualWeight"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["CutShadebandTime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["Fabric2LabTime"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["rid"].Visible = false;
        }

        private void PrintBarcode(object sender, EventArgs e)
        {
            if (this.dtReceiving != null && this.dtReceiving.Rows.Count > 0 && this.gridReceiving.CurrentDataRow != null)
            {
                DataRow dr = this.gridReceiving.CurrentDataRow;

                if (MyUtility.Check.Empty(dr["Barcode"]))
                {
                    return;
                }

                string sp = "SP:" + MyUtility.Convert.GetString(dr["poid"]);
                string seq = "SEQ:" + MyUtility.Convert.GetString(dr["Seq"]);
                string refno = "Ref:" + MyUtility.Convert.GetString(dr["refno"]);
                string dyelot = "Lot:" + MyUtility.Convert.GetString(dr["dyelot"]);
                string color = "Color:" + MyUtility.Convert.GetString(dr["color"]);
                string roll = "Roll:" + MyUtility.Convert.GetString(dr["Roll"]);
                string qty = "Qty:" + MyUtility.Convert.GetString(dr["StockQty"]);

                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SP", sp));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("seq", seq));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("refno", refno));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("dyelot", dyelot));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("color", color));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("roll", roll));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("qty", qty));

                #region QRCode 參數
                int qrCodeWidth = 90;
                string rdlcName = "P21_PrintBarcode5.rdlc";
                if (this.cmbBarcoedType.Text == "10X10")
                {
                    qrCodeWidth = 180;
                    rdlcName = "P21_PrintBarcode10.rdlc";
                }

                Bitmap oriBitmap = MyUtility.Convert.GetString(dr["Barcode"]).ToBitmapQRcode(qrCodeWidth, qrCodeWidth);
                string paramValue;
                using (var b = new Bitmap(oriBitmap))
                {
                    using (var ms = new MemoryStream())
                    {
                        b.Save(ms, ImageFormat.Png);
                        paramValue = Convert.ToBase64String(ms.ToArray());
                    }
                }

                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Image", paramValue));
                #endregion

                #region  指定是哪個 RDLC
                DualResult result = ReportResources.ByEmbeddedResource(typeof(P21_PrintBarcode_Data), rdlcName, out IReportResource reportresource);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                report.ReportDataSource = new List<P21_PrintBarcode_Data>(); // 其實不用, 但寫法需要, 給個空的即可
                report.ReportResource = reportresource;
                #endregion

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                    DirectPrint = true,
                };

                frm.Show();
            }
        }

        private void DifferentialColorChange(int rowIndex)
        {
            if ((decimal)this.gridReceiving.Rows[rowIndex].Cells["Differential"].Value < 0)
            {
                this.gridReceiving.Rows[rowIndex].Cells["Differential"].Style.ForeColor = Color.Red;
            }
            else
            {
                this.gridReceiving.Rows[rowIndex].Cells["Differential"].Style.ForeColor = Color.Black;
            }
        }

        private void GridFormatChange()
        {
            foreach (DataGridViewRow item in this.gridReceiving.Rows)
            {
                this.DifferentialColorChange(item.Index);
            }
        }

        private void GridLocationCellPop(int rowIndex)
        {
            if (rowIndex == -1)
            {
                return;
            }

            DataRow curDr = this.gridReceiving.GetDataRow(rowIndex);
            SelectItem2 selectItem2 = Prgs.SelectLocation(curDr["StockType"].ToString());
            selectItem2.ShowDialog();
            if (selectItem2.DialogResult == DialogResult.OK)
            {
                curDr["Location"] = selectItem2.GetSelecteds().Select(s => s["ID"].ToString()).JoinToString(",");
                this.gridReceiving.Rows[rowIndex].Cells["Location"].Value = curDr["Location"];
            }

            curDr.EndEdit();
        }

        private void Query()
        {
            string sqlWhere = string.Empty;
            string sqlWhere2 = string.Empty;

            if (!this.txtSeq.CheckSeq1Empty() && this.txtSeq.CheckSeq2Empty())
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.Seq1}'";
                sqlWhere2 += $" and td.seq1 = '{this.txtSeq.Seq1}'";
            }
            else if (!this.txtSeq.CheckEmpty(showErrMsg: false))
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.Seq1}' and rd.seq2 = '{this.txtSeq.Seq2}'";
                sqlWhere2 += $" and td.seq1 = '{this.txtSeq.Seq1}' and td.seq2 = '{this.txtSeq.Seq2}'";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                sqlWhere += $" and o.BrandID = '{this.txtBrand.Text}'";
                sqlWhere2 += $" and o.BrandID = '{this.txtBrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtRef.Text))
            {
                sqlWhere += $" and psd.refno = '{this.txtRef.Text}'";
                sqlWhere2 += $" and psd.refno = '{this.txtRef.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                sqlWhere += $" and (psd.SuppColor = '{this.txtColor.Text}' or psd.ColorID = '{this.txtColor.Text}')";
                sqlWhere2 += $" and (psd.SuppColor = '{this.txtColor.Text}' or psd.ColorID = '{this.txtColor.Text}')";
            }

            if (!MyUtility.Check.Empty(this.txtRoll.Text))
            {
                sqlWhere += $" and rd.roll like '%{this.txtRoll.Text}%'";
                sqlWhere2 += $" and td.roll like '%{this.txtRoll.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.txtDyelot.Text))
            {
                sqlWhere += $" and rd.dyelot = '{this.txtDyelot.Text}'";
                sqlWhere2 += $" and td.dyelot = '{this.txtDyelot.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtRecivingID.Text))
            {
                sqlWhere += $" and r.ID = '{this.txtRecivingID.Text}'";
                sqlWhere2 += $" and t.ID = '{this.txtRecivingID.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtWK.Text))
            {
                sqlWhere += $" and r.ExportID = '{this.txtWK.Text}'";
                sqlWhere2 += $" and 1=0 ";
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlWhere += $" and rd.POID like '%{this.txtSP.Text}%'";
                sqlWhere2 += $" and td.POID like '%{this.txtSP.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.dateBoxArriveWH.Value1))
            {
                sqlWhere += $" and r.WhseArrival >= '{Convert.ToDateTime(this.dateBoxArriveWH.Value1).ToString("yyyy/MM/dd")}'";
                sqlWhere2 += $" and t.IssueDate >= '{Convert.ToDateTime(this.dateBoxArriveWH.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateBoxArriveWH.Value2))
            {
                sqlWhere += $" and r.WhseArrival <= '{Convert.ToDateTime(this.dateBoxArriveWH.Value2).ToString("yyyy/MM/dd")}'";
                sqlWhere2 += $" and t.IssueDate <= '{Convert.ToDateTime(this.dateBoxArriveWH.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtMtlLocation.Text))
            {
                sqlWhere += $@"
and exists(
	select 1 from FtyInventory_Detail fid 
	where fid.Ukey = fi.Ukey
	and fid.MtlLocationID = '{this.txtMtlLocation.Text}'
)";
                sqlWhere2 += $@"
and exists(
	select 1 from FtyInventory_Detail fid 
	where fid.Ukey = fi.Ukey
	and fid.MtlLocationID = '{this.txtMtlLocation.Text}'
)";
            }

            if (this.dateTimePicker1.Checked)
            {
                sqlWhere += $@"
and cutTime.cutTime between '{this.dateTimePicker1.Text}' and '{this.dateTimePicker2.Text}'";
                sqlWhere2 += $@"
and cutTime.cutTime between '{this.dateTimePicker1.Text}' and '{this.dateTimePicker2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtuserCutShadeband.TextBox1.Text))
            {
                sqlWhere += $@"
and cutTime.CutBy = '{this.txtuserCutShadeband.TextBox1.Text}'";
                sqlWhere2 += $@"
and cutTime.CutBy = '{this.txtuserCutShadeband.TextBox1.Text}'";
            }

            if (MyUtility.Check.Empty(sqlWhere))
            {
                MyUtility.Msg.WarningBox("The criteria can't all be empty.");
                return;
            }

            string sqlQuery = $@"
select
[ID] = REPLACE(ID,'''',''),
Name
into #tmpStockType
from DropDownList WITH (NOLOCK) where Type = 'Pms_StockType'

select r.*
     , [rid] = cast(ROW_NUMBER() over(order by  OrderSeq, OrderSeq2) as int)
into #tmpFinal
from
(
    select
        [select] = 0
        ,r.ExportID
        ,rd.Id
        ,rd.PoId
        ,[Seq] = rd.Seq1 + ' ' + rd.Seq2
        ,rd.Roll
        ,rd.Dyelot
        ,fi.Barcode
        ,rd.StockQty
        ,[StockTypeDesc] = st.Name
        ,rd.StockType
        --,rd.Location
        --,[OldLocation] = rd.Location
        ,[Location]=Location.MtlLocationID
        ,[OldLocation] = Location.MtlLocationID
        ,rd.Weight
        ,rd.ActualWeight
        ,[OldActualWeight] = rd.ActualWeight
        ,[Differential] = rd.ActualWeight - rd.Weight
        ,[FtyInventoryUkey] = fi.Ukey
        ,[FtyInventoryQty] = fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty
        ,rd.Seq1
        ,rd.Seq2
        ,psd.refno
        ,[Remark]=''
        ,[ReceivingSource]='Receiving'
        ,[CutShadebandTime]=cutTime.CutTime
        ,[OldCutShadebandTime]=cutTime.CutTime
        ,cutTime.CutBy
        ,o.BrandID
        ,fb.WeaveTypeID
        ,[OldFabric2LabTime]=rd.Fabric2LabTime
        ,rd.Fabric2LabTime
        ,rd.Fabric2LabBy
        ,[ReceivingTransferInUkey] = rd.Ukey
        ,rd.Ukey
        ,rd.Checker
        ,[OldChecker] = rd.Checker
		,[OrderSeq] = 7
		,[OrderSeq2] = cast(ROW_NUMBER() over(order by r.ExportID, rd.Id, rd.EncodeSeq, rd.PoId, rd.Seq1, rd.Seq2, rd.Roll, rd.Dyelot) as int)	
        ,fb.MtlTypeID
		,psd.SuppColor
		,psd.ColorID
    from  Receiving r with (nolock)
    inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
    inner join View_WH_Orders o with (nolock) on o.ID = rd.POID 
    inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
    inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
    inner join Ftyinventory  fi with (nolock) on    rd.POID = fi.POID and
                                                    rd.Seq1 = fi.Seq1 and
                                                    rd.Seq2 = fi.Seq2 and
                                                    rd.Roll = fi.Roll and
                                                    rd.Dyelot  = fi.Dyelot and
                                                    rd.StockType = fi.StockType
    left join #tmpStockType st with (nolock) on st.ID = rd.StockType
    OUTER APPLY(

	    SELECT [MtlLocationID] = STUFF(
			    (
			    SELECT DISTINCT IIF(fid.MtlLocationID IS NULL OR fid.MtlLocationID = '' ,'' , ','+fid.MtlLocationID)
			    FROM FtyInventory_Detail fid
			    WHERE fid.Ukey = fi.Ukey
			    FOR XML PATH('') )
			    , 1, 1, '')
    )Location
    OUTER APPLY(
	    SELECT  fs.CutTime,fs.CutBy
	    FROM FIR f
	    INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
	    WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
    )cutTime

    where r.MDivisionID  = '{Env.User.Keyword}'
    AND psd.FabricType ='F'
    {sqlWhere}

    UNION 

    SELECT 
        [select] = 0
        ,ExportID=''
        ,ID=t.ID
        ,td.PoId
        ,[Seq] = td.Seq1 + ' ' + td.Seq2
        ,td.Roll
        ,td.Dyelot
        ,fi.Barcode
        ,[StockQty]=td.Qty
        ,[StockTypeDesc] = st.Name
        ,td.StockType
        ,[Location]=Location.MtlLocationID 
        ,[OldLocation] = Location.MtlLocationID 
        ,[Weight]=td.Weight
        ,ActualWeight=td.ActualWeight
        ,[OldActualWeight] = td.ActualWeight
        ,[Differential] = td.ActualWeight - td.Weight
        ,[FtyInventoryUkey] = fi.Ukey
        ,[FtyInventoryQty] = fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty
        ,td.Seq1
        ,td.Seq2
        ,psd.refno
        ,[Remark]=''
        ,[ReceivingSource]='TransferIn'
        ,[CutShadebandTime]=cutTime.CutTime
        ,[OldCutShadebandTime]=cutTime.CutTime
        ,cutTime.CutBy
        ,o.BrandID
        ,fb.WeaveTypeID
        ,[OldFabric2LabTime]=td.Fabric2LabTime
        ,td.Fabric2LabTime
        ,td.Fabric2LabBy
        ,[ReceivingTransferInUkey] = td.Ukey
        ,td.Ukey
        ,td.Checker
        ,[OldChecker] = td.Checker
		,[OrderSeq] = 18
		,[OrderSeq2] = cast(ROW_NUMBER() over(order by t.ID, td.PoId, td.Seq1, td.Seq2, td.Roll, td.Dyelot) as int)
        ,fb.MtlTypeID
		,psd.SuppColor
		,psd.ColorID
    FROM TransferIn t with (nolock)
    INNER JOIN TransferIn_Detail td with (nolock) ON t.ID = td.ID
    INNER JOIN View_WH_Orders o with (nolock) ON o.ID = td.POID
    INNER JOIN PO_Supp_Detail psd with (nolock) on td.PoId = psd.ID and td.Seq1 = psd.SEQ1 and td.Seq2 = psd.SEQ2
    INNER JOIN Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
    INNER JOIN Ftyinventory  fi with (nolock) on    td.POID = fi.POID and
                                                    td.Seq1 = fi.Seq1 and
                                                    td.Seq2 = fi.Seq2 and
                                                    td.Roll = fi.Roll and
                                                    td.Dyelot  = fi.Dyelot and
                                                    td.StockType = fi.StockType
    INNER JOIN #tmpStockType st with (nolock) on st.ID = td.StockType
    OUTER APPLY(

	    SELECT [MtlLocationID] = STUFF(
			    (
			    SELECT DISTINCT IIF(fid.MtlLocationID IS NULL OR fid.MtlLocationID = '' ,'' , ','+fid.MtlLocationID)
			    FROM FtyInventory_Detail fid
			    WHERE fid.Ukey = fi.Ukey
			    FOR XML PATH('') )
			    , 1, 1, '')
    )Location
    OUTER APPLY(
	    SELECT  fs.CutTime,fs.CutBy
	    FROM FIR f
	    INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
	    WHERE  t.id = f.ReceivingID and td.PoId = F.POID and td.Seq1 = F.SEQ1 and td.Seq2 = F.SEQ2 AND td.Roll = fs.Roll and td.Dyelot = fs.Dyelot
    )cutTime
    WHERE t.Status='Confirmed' 
    AND t.MDivisionID  = '{Env.User.Keyword}'
    AND psd.FabricType ='F'
    {sqlWhere2}
)r

--因為部分欄位抓取速度較慢，改先產生tmp table後再另外抓取
select  t.*
        ,[LastRemark] = LastEditDate.Remark
        ,[LastEditDate]=LastEditDate.EditDate
		,[Description] = dbo.getmtldesc(t.POID, t.Seq1, t.Seq2, 2, 0)
        ,[Color] = case when t.MtlTypeID = 'EMB THREAD' OR t.MtlTypeID = 'SP THREAD' OR t.MtlTypeID = 'THREAD' and t.SuppColor = '' then dbo.GetColorMultipleID (t.BrandID, t.ColorID)
						when t.MtlTypeID = 'EMB THREAD' OR t.MtlTypeID = 'SP THREAD' OR t.MtlTypeID = 'THREAD' and t.SuppColor <> '' then t.SuppColor
						else t.ColorID end
from #tmpFinal t
OUTER APPLY(
	    SELECT top 1 lt.EditDate, lt.Remark
	    FROM LocationTrans lt
	    INNER JOIN LocationTrans_detail ltd ON lt.ID=ltd.ID
	    WHERE lt.Status='Confirmed' AND ltd.FtyInventoryUkey=t.FtyInventoryUkey 
        order by lt.EditDate desc
        )LastEditDate
order by t.rid

DROP TABLE #tmpStockType, #tmpFinal
";

            DualResult result = DBProxy.Current.Select(null, sqlQuery, out this.dtReceiving);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtReceiving.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                this.gridReceiving.DataSource = this.dtReceiving;
                return;
            }

            this.gridReceiving.DataSource = this.dtReceiving;
            this.GridFormatChange();
            this.numSelectCnt.Value = 0;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.dateTimePicker.Value = DateTime.Now;
            this.dateTimeFabric2LabBy.Value = DateTime.Now;
            this.Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridReceiving_Sorted(object sender, EventArgs e)
        {
            this.GridFormatChange();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.dateTimePicker.Value = DateTime.Now;
            this.dateTimeFabric2LabBy.Value = DateTime.Now;

            var selectedReceiving = this.dtReceiving.AsEnumerable().Where(s => (int)s["select"] == 1);
            if (!selectedReceiving.Any())
            {
                MyUtility.Msg.WarningBox("Please select rows first!");
                return;
            }

            if (selectedReceiving.Any(s => MyUtility.Check.Empty(s["Location"])
                                    && !MyUtility.Check.Empty(s["OldLocation"])))
            {
                MyUtility.Msg.WarningBox("Location can not be empty");
                return;
            }

            #region 排除Location 包含WMS & 非WMS資料

            string sqlcmd = @"
select * from
(
select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct t.POID,t.Seq1,t.Seq2,t.Roll,t.Dyelot,IsWMS = isnull( ml.IsWMS,0),t.Location
		from #tmp t
		outer apply(
			select ml.IsWMS
			from MtlLocation ml
			inner join dbo.SplitString(t.Location,',') sp on sp.Data = ml.ID
		)ml
	) a
) final
where rowCnt = 2

drop table #tmp
";
            DualResult result1;
            DataTable dt;
            string errmsg = string.Empty;

            if (!(result1 = MyUtility.Tool.ProcessWithDatatable(selectedReceiving.CopyToDataTable(), string.Empty, sqlcmd, out dt)))
            {
                MyUtility.Msg.WarningBox(result1.Messages.ToString());
                return;
            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow tmp in dt.Rows)
                    {
                        errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} Location: {tmp["Location"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("These material exists in WMS Location and non-WMS location in same time , please revise below detail location column data." + Environment.NewLine + errmsg, "Warning");
                    return;
                }
            }
            #endregion

            // 排除Location沒有修改的資料
            DataRow[] drArryExistRemark = this.dtReceiving.AsEnumerable().Where(x => x.Field<int>("select") == 1
                                                                             && !MyUtility.Check.Empty(x.Field<string>("Remark"))
                                                                             && !x.Field<string>("Location").EqualString(x.Field<string>("OldLocation"))).ToArray();
            DataRow[] drArryNotExistRemark = this.dtReceiving.AsEnumerable().Where(x => x.Field<int>("select") == 1
                                                                             && MyUtility.Check.Empty(x.Field<string>("Remark"))
                                                                             && !x.Field<string>("Location").EqualString(x.Field<string>("OldLocation"))).ToArray();
            var listUpdateReceivingTransferIn = this.dtReceiving.AsEnumerable().Where(x => x.Field<int>("select") == 1
                                                                             && (x.Field<decimal>("ActualWeight") != x.Field<decimal>("OldActualWeight") ||
                                                                                 !MyUtility.Convert.GetDate(x["Fabric2LabTime"]).EqualString(MyUtility.Convert.GetDate(x["OldFabric2LabTime"])) ||
                                                                                 x.Field<string>("Checker") != x.Field<string>("OldChecker")))
                                                                                .Select(s => new
                                                                                {
                                                                                    ReceivingSource = s["ReceivingSource"].ToString(),
                                                                                    ActualWeight = MyUtility.Convert.GetDecimal(s["ActualWeight"]),
                                                                                    Ukey = s["ReceivingTransferInUkey"],
                                                                                    Fabric2LabTime = s["Fabric2LabTime"],
                                                                                    IsNeedUpdateFabric2LabBy = !MyUtility.Convert.GetDate(s["Fabric2LabTime"]).EqualString(MyUtility.Convert.GetDate(s["OldFabric2LabTime"])),
                                                                                    Checker = s["Checker"],
                                                                                });

            DataRow[] drArryCutShadebandTime = this.dtReceiving.AsEnumerable().Where(x => x.Field<int>("select") == 1
                                                                             && !MyUtility.Convert.GetDate(x["CutShadebandTime"]).EqualString(MyUtility.Convert.GetDate(x["OldCutShadebandTime"]))).ToArray();

            // Remark沒資料則統一合併後寫入P26 同ID，排除Location沒有修改的資料
            var selectedReceivingSummary = drArryNotExistRemark
                                        .Where(s => s["Location"].ToString() != s["OldLocation"].ToString())
                                        .GroupBy(s => new
                                        {
                                            POID = s["POID"].ToString(),
                                            Seq1 = s["Seq1"].ToString(),
                                            Seq2 = s["Seq2"].ToString(),
                                            Roll = s["Roll"].ToString(),
                                            Dyelot = s["Dyelot"].ToString(),
                                            StockType = s["StockType"].ToString(),
                                            FtyInventoryQty = (decimal)s["FtyInventoryQty"],
                                            FtyInventoryUkey = (long)s["FtyInventoryUkey"],
                                        })
                                        .Select(s => new
                                        {
                                            s.Key.POID,
                                            s.Key.Seq1,
                                            s.Key.Seq2,
                                            s.Key.Roll,
                                            s.Key.Dyelot,
                                            s.Key.StockType,
                                            s.Key.FtyInventoryQty,
                                            s.Key.FtyInventoryUkey,
                                            Location = s.Select(d => d["Location"].ToString()).Distinct().JoinToString(","),
                                            OldLocation = s.Select(d => d["OldLocation"].ToString()).Distinct().JoinToString(","),
                                        });

            int cntID = ((selectedReceivingSummary.Count() >= 1) ? 1 : 0) + drArryExistRemark.Length; // 產生表頭數

            string sqlInsertLocationTrans = string.Empty;
            List<string> id_list = MyUtility.GetValue.GetBatchID(Env.User.Keyword + "LH", "LocationTrans", batchNumber: cntID, sequenceMode: 2); // 批次產生ID
            int idcnt = 0;

            if (id_list.Count == 0 && listUpdateReceivingTransferIn.Count() == 0 && drArryCutShadebandTime.Length == 0)
            {
                MyUtility.Msg.WarningBox("There is no Location, Act.(kg), Cut Shadeband Time, Checker changed.");
                return;
            }

            if (id_list.Count > 0)
            {
                // Remark有資料要分開寫入到P26 不同ID
                foreach (var item in drArryExistRemark)
                {
                    // 預設要填入---Create from P21.，因此要扣掉這個文字長度
                    if (item["Remark"].ToString().Length >= (60 - 19))
                    {
                        MyUtility.Msg.WarningBox("Remark is too long!");
                        return;
                    }

                    sqlInsertLocationTrans += $@"
Insert into LocationTrans(ID,MDivisionID,FactoryID,IssueDate,Status,Remark,AddName,AddDate,EditName,EditDate)
            values( '{id_list[idcnt]}',
                    '{Env.User.Keyword}',
                    '{Env.User.Factory}',
                    GetDate(),
                    'Confirmed',
                    '{item["Remark"]}---Create from P21.',
                    '{Env.User.UserID}',
                    GetDate(),
                    '{Env.User.UserID}',
                    GetDate()
                )
";

                    sqlInsertLocationTrans += $@"
Insert into LocationTrans_Detail(   ID,
                                    FtyInventoryUkey,
                                    POID,
                                    Seq1,
                                    Seq2,
                                    Roll,
                                    Dyelot,
                                    FromLocation,
                                    ToLocation,
                                    Qty,
                                    StockType)
                values('{id_list[idcnt]}',
                       {item["FtyInventoryUkey"]},
                       '{item["POID"]}',
                       '{item["Seq1"]}',
                       '{item["Seq2"]}',
                       '{item["Roll"]}',
                       '{item["Dyelot"]}',
                       '{item["OldLocation"]}',
                       '{item["Location"]}',
                       {item["FtyInventoryQty"]},
                       '{item["StockType"]}')
";
                    idcnt++;
                }

                if (selectedReceivingSummary.Any())
                {
                    sqlInsertLocationTrans += $@"
Insert into LocationTrans(ID,MDivisionID,FactoryID,IssueDate,Status,Remark,AddName,AddDate,EditName,EditDate)
            values( '{id_list[idcnt]}',
                    '{Env.User.Keyword}',
                    '{Env.User.Factory}',
                    GetDate(),
                    'Confirmed',
                    '---Create from P21.',
                    '{Env.User.UserID}',
                    GetDate(),
                    '{Env.User.UserID}',
                    GetDate()
                )
";

                    foreach (var receivingItem in selectedReceivingSummary)
                    {
                        sqlInsertLocationTrans += $@"
Insert into LocationTrans_Detail(   ID,
                                    FtyInventoryUkey,
                                    POID,
                                    Seq1,
                                    Seq2,
                                    Roll,
                                    Dyelot,
                                    FromLocation,
                                    ToLocation,
                                    Qty,
                                    StockType)
                values('{id_list[idcnt]}',
                       {receivingItem.FtyInventoryUkey},
                       '{receivingItem.POID}',
                       '{receivingItem.Seq1}',
                       '{receivingItem.Seq2}',
                       '{receivingItem.Roll}',
                       '{receivingItem.Dyelot}',
                       '{receivingItem.OldLocation}',
                       '{receivingItem.Location}',
                       {receivingItem.FtyInventoryQty},
                       '{receivingItem.StockType}')
";
                    }
                }

                // 重新撈取新增ID資料
                string idList = id_list.Count <= 1 ? id_list[0].ToString() : id_list.JoinToString("','");
                sqlInsertLocationTrans += $@"select * from LocationTrans_Detail where ID in ('{idList}')";
            }

            string sqlUpdateReceiving_Detail = string.Empty;
            foreach (var updateItem in listUpdateReceivingTransferIn)
            {
                string updateFabric2Lab = string.Empty;

                if (updateItem.IsNeedUpdateFabric2LabBy)
                {
                    string fabric2LabTime = MyUtility.Convert.GetDate(updateItem.Fabric2LabTime).HasValue ? "'" + MyUtility.Convert.GetDate(updateItem.Fabric2LabTime).Value.ToString("yyyy/MM/dd HH:mm:ss") + "'" : "NULL";
                    updateFabric2Lab = $",Fabric2LabTime = {fabric2LabTime}, Fabric2LabBy = '{Env.User.UserID}' ";
                }

                if (updateItem.ReceivingSource == "Receiving")
                {
                    sqlUpdateReceiving_Detail += $@"update Receiving_Detail set ActualWeight  = {updateItem.ActualWeight} {updateFabric2Lab}, Checker = '{updateItem.Checker}'
                                                    where   UKey = '{updateItem.Ukey}'
";
                }

                if (updateItem.ReceivingSource == "TransferIn")
                {
                    sqlUpdateReceiving_Detail += $@"update TransferIn_Detail set ActualWeight  = {updateItem.ActualWeight} {updateFabric2Lab}, Checker = '{updateItem.Checker}'
                                                    where   UKey = '{updateItem.Ukey}'
";
                }
            }

            string sqlUpdateFIR_Shadebone = string.Empty;
            foreach (var updateItem in drArryCutShadebandTime)
            {
                string cutTime = MyUtility.Convert.GetDate(updateItem["CutShadebandTime"]).HasValue ? "'" + MyUtility.Convert.GetDate(updateItem["CutShadebandTime"]).Value.ToString("yyyy/MM/dd HH:mm:ss") + "'" : "NULL";
                sqlUpdateFIR_Shadebone += $@"
UPDATE fs
SET  fs.CutTime = {cutTime}, Cutby = iif({cutTime} is null, Cutby, '{Sci.Env.User.UserID}')
FROM FIR f
INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
WHERE  f.ReceivingID='{updateItem["ID"]}'
AND f.PoId = '{updateItem["PoId"]}'
AND f.Seq1 = '{updateItem["Seq1"]}'
AND f.Seq2 ='{updateItem["Seq2"]}'
AND fs.Roll = '{updateItem["Roll"]}'
AND fs.Dyelot = '{updateItem["Dyelot"]}'
;
";
            }

            DataTable dtToWMS = this.dtReceiving.AsEnumerable().Where(s => (int)s["select"] == 1).CopyToDataTable().Clone();
            DataTable dtcopy = this.dtReceiving.AsEnumerable().Where(s => (int)s["select"] == 1).CopyToDataTable();
            foreach (DataRow dr in dtcopy.Rows)
            {
                string sqlchk = $@"
select 1 from MtlLocation m
inner join SplitString('{dr["Location"]}',',') sp on m.ID = sp.Data
where m.IsWMS = 0";
                if (MyUtility.Check.Seek(sqlchk) && string.Compare(dr["Location"].ToString(), dr["OldLocation"].ToString()) != 0)
                {
                    dtToWMS.ImportRow(dr);
                }
            }

            if (!Prgs_WMS.LockNotWMS(dtToWMS))
            {
                return;
            }

            DualResult result;
            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    if (!MyUtility.Check.Empty(sqlInsertLocationTrans))
                    {
                        if (!(result = DBProxy.Current.Select(null, sqlInsertLocationTrans, out DataTable dtLocationTransDetail)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = Prgs.UpdateFtyInventoryMDivisionPoDetail(dtLocationTransDetail.AsEnumerable().ToList())))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!MyUtility.Check.Empty(sqlUpdateReceiving_Detail))
                    {
                        if (!(result = DBProxy.Current.Execute(null, sqlUpdateReceiving_Detail)))
                        {
                            throw result.GetException();
                        }
                    }

                    if (!MyUtility.Check.Empty(sqlUpdateFIR_Shadebone))
                    {
                        if (!(result = DBProxy.Current.Execute(null, sqlUpdateFIR_Shadebone)))
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

            if (!MyUtility.Check.Empty(errMsg))
            {
                // 找出要撤回的 P07 Ukey
                DataTable dt07 = Prgs.GetWHDetailUkey(dtToWMS, "P07");

                // 找出要撤回的 P18 Ukey
                DataTable dt18 = Prgs.GetWHDetailUkey(dtToWMS, "P18");

                Gensong_AutoWHFabric.Sent(true, dt07, "P07", EnumStatus.UnLock, EnumStatus.Unconfirm);
                Gensong_AutoWHFabric.Sent(true, dt18, "P18", EnumStatus.UnLock, EnumStatus.Unconfirm);
                this.ShowErr(errMsg);
                return;
            }

            // 調整後 Tolocation 不是自動倉, 要發給 WMS 要求撤回(Delete) P07/P18
            Prgs_WMS.DeleteNotWMS(dtToWMS);

            // 將當前所選位置記錄起來後, 待資料重整後定位回去!
            int currentRowIndexInt = this.gridReceiving.CurrentRow.Index;
            int currentColumnIndexInt = this.gridReceiving.CurrentCell.ColumnIndex;
            this.Query();
            this.gridReceiving.CurrentCell = this.gridReceiving[currentColumnIndexInt, currentRowIndexInt];
            this.gridReceiving.FirstDisplayedScrollingRowIndex = currentRowIndexInt;

            MyUtility.Msg.InfoBox("Complete");
        }

        /// <summary>
        /// 檢查表身Location,ActualWeight是否有被修改過(跟DB資料比較)
        /// 有被修改過,就自動勾選資料
        /// </summary>
        /// <inheritdoc/>
        private void SelectModify(int rowIndex)
        {
            DataRow dr = this.gridReceiving.GetDataRow(rowIndex);
            bool chg_ActWeight = false;
            bool chg_Location = false;
            bool chg_CutShadebandTime = false;

            decimal oldActualWeight = MyUtility.Convert.GetDecimal(dr["OldActualWeight"]);
            decimal newActualWeight = MyUtility.Convert.GetDecimal(dr["ActualWeight"]);
            if (!oldActualWeight.Equals(newActualWeight))
            {
                chg_ActWeight = true;
            }
            else
            {
                chg_ActWeight = false;
            }

            decimal oldCutShadebandTime = MyUtility.Convert.GetDecimal(dr["OldCutShadebandTime"]);
            decimal cutShadebandTime = MyUtility.Convert.GetDecimal(dr["CutShadebandTime"]);
            if (!oldCutShadebandTime.Equals(cutShadebandTime))
            {
                chg_CutShadebandTime = true;
            }
            else
            {
                chg_CutShadebandTime = false;
            }

            // 判斷Location 有變更資料就自動勾選
            string oldvalue = dr["OldLocation"].ToString();
            string newvalue = dr["Location"].ToString();
            if (!oldvalue.Equals(newvalue))
            {
                chg_Location = true;
            }
            else
            {
                chg_Location = false;
            }

            if (chg_Location || chg_ActWeight || chg_CutShadebandTime)
            {
                dr["select"] = 1;
            }
            else
            {
                dr["select"] = 0;
            }
        }

        private void GridReceiving_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.gridReceiving.ValidateControl();
                DataTable dt = (DataTable)this.gridReceiving.DataSource;
                if (dt != null || dt.Rows.Count > 0)
                {
                    int cnt = MyUtility.Convert.GetInt(dt.Compute("count(select)", "select = 1"));
                    this.numSelectCnt.Value = cnt;
                }
            }
        }

        private void BtnUpdateTime_Click(object sender, EventArgs e)
        {
            if (this.dtReceiving != null && this.dtReceiving.Rows.Count > 0)
            {
                var selectedReceiving = this.dtReceiving.AsEnumerable().Where(s => (int)s["select"] == 1);
                DateTime dateTime = this.dateTimePicker.Value;

                DataRow[] dataRows = this.dtReceiving.Select("select=1");

                foreach (var item in dataRows)
                {
                    item["CutShadebandTime"] = dateTime;
                }
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.gridReceiving.DataSource))
            {
                return;
            }

            if (this.txtLocateSP.Text.Empty() &&
                this.txtLocateSeq.Seq1.Empty() &&
                this.txtLocateSeq.Seq2.Empty() &&
                this.txtLocateRef.Text.Empty() &&
                this.txtLocateColor.Text.Empty() &&
                this.txtLocateRoll.Text.Empty() &&
                this.txtLocateDyelot.Text.Empty())
            {
                return;
            }

            DataTable dt = (DataTable)this.gridReceiving.DataSource;
            List<DataRow> drs = dt.AsEnumerable().ToList();
            if (!this.txtLocateSP.Text.Empty() && drs.Any())
            {
                drs = drs.Where(x => x.Field<string>("poid").Contains(this.txtLocateSP.Text.ToString())).ToList();
            }

            if (!this.txtLocateSeq.Seq1.Empty() && drs.Any())
            {
                drs = drs.Where(x => x.Field<string>("Seq1").EqualString(this.txtLocateSeq.Seq1.ToString())).ToList();
            }

            if (!this.txtLocateSeq.Seq2.Empty() && drs.Any())
            {
                drs = drs.Where(x => x.Field<string>("Seq2").EqualString(this.txtLocateSeq.Seq2.ToString())).ToList();
            }

            if (!this.txtLocateRef.Text.Empty() && drs.Any())
            {
                drs = drs.Where(x => x.Field<string>("refno").EqualString(this.txtLocateRef.Text.ToString())).ToList();
            }

            if (!this.txtLocateColor.Text.Empty() && drs.Any())
            {
                drs = drs.Where(x => x.Field<string>("Color").EqualString(this.txtLocateColor.Text.ToString())).ToList();
            }

            if (!this.txtLocateRoll.Text.Empty() && drs.Any())
            {
                drs = drs.Where(x => x.Field<string>("Roll").Contains(this.txtLocateRoll.Text.ToString())).ToList();
            }

            if (!this.txtLocateDyelot.Text.Empty() && drs.Any())
            {
                drs = drs.Where(x => x.Field<string>("Dyelot").Contains(this.txtLocateDyelot.Text.ToString())).ToList();
            }

            if (drs.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
                return;
            }

            int index = drs.Select(x => x.Field<int>("rid")).FirstOrDefault();
            DataGridViewRow row = this.gridReceiving.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["rid"].Value.ToString().Equals(index.ToString())).FirstOrDefault();

            if (index <= 0 || row == null)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.gridReceiving.CurrentCell = this.gridReceiving.Rows[row.Index].Cells["POID"];
            }
        }

        private bool oldDateChecked = false;

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).Checked != this.oldDateChecked)
            {
                this.oldDateChecked = ((DateTimePicker)sender).Checked;
                this.ChangeCutShadebandTime(((DateTimePicker)sender).Checked);
            }
        }

        private void ChangeCutShadebandTime(bool chk = false)
        {
            this.dateTimePicker1.Checked = this.dateTimePicker2.Checked = chk;
            this.ChangeDateTimepickCheck(this.dateTimePicker1);
            this.ChangeDateTimepickCheck(this.dateTimePicker2);
            if (chk)
            {
                this.dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                this.dateTimePicker2.Text = DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
            }
        }

        private void ChangeDateTimepickCheck(DateTimePicker dateTimePicker)
        {
            if (dateTimePicker.Checked)
            {
                dateTimePicker.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            }
            else
            {
                dateTimePicker.CustomFormat = " ";
            }
        }

        private void BtnUpdateFabric2LabTime_Click(object sender, EventArgs e)
        {
            if (this.dtReceiving != null && this.dtReceiving.Rows.Count > 0)
            {
                var selectedReceiving = this.dtReceiving.AsEnumerable().Where(s => (int)s["select"] == 1);
                DateTime dateTime = this.dateTimeFabric2LabBy.Value;

                DataRow[] dataRows = this.dtReceiving.Select("select=1");

                foreach (var item in dataRows)
                {
                    item["Fabric2LabTime"] = dateTime;
                }
            }
        }
    }
}
