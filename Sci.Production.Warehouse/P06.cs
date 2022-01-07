using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P06
    /// </summary>
    public partial class P06 : Sci.Win.Tems.Input6
    {
        private bool IsProduceFty
        {
            get
            {
                if (this.CurrentMaintain == null)
                {
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["FromFactoryID"]))
                {
                    return false;
                }

                List<SqlParameter> listPar = new List<SqlParameter>()
                {
                    new SqlParameter("@FromFactoryID", this.CurrentMaintain["FromFactoryID"]),
                };

                string sqlFtyCheck = @"select 1 from Factory where ID = @FromFactoryID and IsProduceFty = 1";

                return MyUtility.Check.Seek(sqlFtyCheck, listPar);
            }
        }

        private bool IsSent
        {
            get
            {
                if (this.CurrentMaintain == null)
                {
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
                {
                    return false;
                }

                List<SqlParameter> listPar = new List<SqlParameter>()
                {
                    new SqlParameter("@ID", this.CurrentMaintain["ID"]),
                };

                string sqlCheckSent = @"select 1 from TransferExport with (nolock) where ID = @ID and Sent = 1 ";

                return MyUtility.Check.Seek(sqlCheckSent, listPar);
            }
        }

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.RecallChkValue = "Send";
            this.SendChkValue = "New";
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (!this.IsProduceFty)
            {
                MyUtility.Msg.WarningBox("Only from factory can use Edit button.");
                return false;
            }

            if (MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]))
            {
                MyUtility.Msg.WarningBox("This record already Junk can not edit");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void Refresh_AfterToolbarStatusBtnClick()
        {
            base.Refresh_AfterToolbarStatusBtnClick();
            if (!this.EditMode)
            {
                this.toolbar.cmdEdit.Enabled = this.CurrentMaintain["FtyStatus"].ToString() == "New";
                this.toolbar.cmdSend.Enabled = this.CurrentMaintain["FtyStatus"].ToString() == "New";
                this.toolbar.cmdRecall.Enabled = this.CurrentMaintain["FtyStatus"].ToString() == "Send";
            }
        }

        /// <inheritdoc/>
        protected override void ClickSend()
        {
            if (!this.IsProduceFty)
            {
                MyUtility.Msg.WarningBox("Only from factory can use send button.");
                return;
            }

            if (!this.IsSent)
            {
                MyUtility.Msg.WarningBox("TPE shipping not yet send transfer working.");
                return;
            }

            if (MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]))
            {
                MyUtility.Msg.WarningBox("This record already Junk can not edit");
                return;
            }

            string sqlCheckTransferOut = @"
select  distinct ted.POID, [Seq] = Concat (ted.Seq1, ' ', ted.Seq2)
from TransferExport_Detail ted with (nolock)
where   ted.ID = @ID and 
        not exists ( select 1 
                    from TransferOut tfo with (nolock)
                    inner join TransferOut_Detail tfod with (nolock) on tfod.ID = tfo.ID
                    where   tfo.Status != 'New' and
                            tfod.TransferExport_DetailUkey = ted.Ukey
                    )
";

            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@ID", this.CurrentMaintain["ID"]) };

            DataTable dtCheckTransferOut;

            DualResult result = DBProxy.Current.Select(null, sqlCheckTransferOut, listPar, out dtCheckTransferOut);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtCheckTransferOut.Rows.Count > 0)
            {
                string msgCheckTransferOut = dtCheckTransferOut.AsEnumerable()
                                                .Select(s => $"SP# : {s["POID"]}, Seq : {s["Seq"]}")
                                                .JoinToString(Environment.NewLine);

                MyUtility.Msg.WarningBox("Those material not inculde in transfer out, please refer below list." + Environment.NewLine + msgCheckTransferOut);
                return;
            }

            // 檢查是否有漏填Reason
            bool isNeedInputReason = this.DetailDatas.Any(s => MyUtility.Convert.GetDecimal(s["PoQty"]) > MyUtility.Convert.GetDecimal(s["BalanceQty"]) && MyUtility.Check.Empty(s["TransferExportReason"]));
            if (isNeedInputReason)
            {
                MyUtility.Msg.WarningBox("Export q'ty less than PO q'ty must input the reason.");
                return;
            }

            base.ClickSend();

            string sqlUpdateStatus = @"update TransferExport set FtyStatus = 'Send' where ID = @ID";
            List<SqlParameter> listParUpdateStatus = new List<SqlParameter>() { new SqlParameter("@ID", this.CurrentMaintain["ID"]) };

            result = DBProxy.Current.Execute(null, sqlUpdateStatus, listParUpdateStatus);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Send success");
        }

        /// <inheritdoc/>
        protected override void ClickRecall()
        {
            if (!this.IsProduceFty)
            {
                MyUtility.Msg.WarningBox("Only from factory can use recall button.");
                return;
            }

            base.ClickRecall();

            string sqlUpdateStatus = @"update TransferExport set FtyStatus = 'New' where ID = @ID";
            List<SqlParameter> listParUpdateStatus = new List<SqlParameter>() { new SqlParameter("@ID", this.CurrentMaintain["ID"]) };

            DualResult result = DBProxy.Current.Execute(null, sqlUpdateStatus, listParUpdateStatus);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Recall success");
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.lblJunk.Visible = MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]);
            if (!this.EditMode)
            {
                this.toolbar.cmdEdit.Enabled = this.CurrentMaintain["FtyStatus"].ToString() == "New";
                this.toolbar.cmdSend.Enabled = this.CurrentMaintain["FtyStatus"].ToString() == "New";
                this.toolbar.cmdRecall.Enabled = this.CurrentMaintain["FtyStatus"].ToString() == "Send";
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select	ted.InventoryPOID,
		[FromSeq] = Concat (ted.InventorySeq1, ' ', ted.InventorySeq2),
		ted.PoID,
		[ToSEQ] = Concat (ted.Seq1, ' ', ted.Seq2),
		ted.SuppID,
		ted.Description,
		ted.UnitID,
		[ColorID] = isnull(psdinv.ColorID, psd.ColorID),
		[SizeSpec] = isnull(psdinv.SizeSpec, psd.SizeSpec),
		ted.PoQty,
		ExportCarton.ExportQty,
		ExportCarton.Foc,
		[BalanceQty] = ExportCarton.ExportQty + ExportCarton.Foc,
		ted.TransferExportReason,
		[ReasonDesc] = (select wr.Description from WhseReason wr with (nolock) where wr.Type = 'TE' and ID = ted.TransferExportReason),
		ted.NetKg,
		ted.WeightKg,
		ted.CBM,
		[ContainerType] =	(SELECT [Val] = STUFF((
							    SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
							    FROM TransferExport_ShipAdvice_Container esc
							    WHERE esc.TransferExport_DetailUkey = ted.Ukey and
							          esc.ContainerType <> '' AND 
									  esc.ContainerNo <> ''
							    FOR XML PATH('')
							     ),1,1,'')
							),
        o.FactoryID,
        o.ProjectID,
        [SCIDlv] = (select min(SciDelivery) 
                    from Orders WITH (NOLOCK) 
                    where POID = ted.PoID and (Category = 'B' or Category = o.Category)),
        [Category] = (case  when o.Category = 'B' then 'Bulk' 
                            when o.Category = 'S' then 'Sample' 
                            when o.Category = 'M' then 'Material' 
                            when o.Category = 'T' then 'Material' 
                            else '' 
                      end),
        [InspDate] = iif(o.PFOrder = 1, dateadd(day,-10,o.SciDelivery)
                        , iif(( select CountryID 
                                from Factory WITH (NOLOCK) 
                                where ID = o.factoryID)='PH'
                               , iif((  select MrTeam 
                                        from Brand WITH (NOLOCK) 
                                        where ID = o.BrandID) = '01'
                                    , dateadd(day,-15,o.SciDelivery)
                                    , dateadd(day,-24,o.SciDelivery))
                                , dateadd(day,-34
                        , o.SciDelivery))),
        [Preshrink] = iif(f.Preshrink = 1, 'V' ,''),
        [Supp] = (ted.SuppID+'-'+s.AbbEN),
        [Color] = Color.Value,
        ted.Ukey,
        PoidSeq1 = rtrim(ted.InventoryPOID) + Ltrim(Rtrim(ted.InventorySeq1)),
        PoidSeq = rtrim(ted.InventoryPOID)+(Ltrim(Rtrim(ted.InventorySeq1)) + ' ' + ted.InventorySeq2),
        ted.Refno
from TransferExport_Detail ted with (nolock) 
left join Orders o with (nolock) on ted.PoID = o.ID
left join PO_Supp_Detail psdInv with (nolock) on	ted.InventoryPOID = psdInv.ID and 
													ted.InventorySeq1 = psdInv.SEQ1 and
													ted.InventorySeq2 = psdinv.SEQ2
left join PO_Supp_Detail psd with (nolock) on	ted.PoID = psd.ID and 
												ted.Seq1 = psd.SEQ1 and
												ted.Seq2 = psd.SEQ2
left join Supp s WITH (NOLOCK) on s.id = ted.SuppID 
left join Fabric f WITH (NOLOCK) on f.SCIRefno = ted.SCIRefno
outer apply(select	[ExportQty] = isnull(sum(isnull(tdc.Qty, 0)), 0) ,
					[Foc] = isnull(sum(isnull(tdc.Foc, 0)), 0) 
			from TransferExport_Detail_Carton tdc with (nolock) where tdc.TransferExport_DetailUkey = ted.Ukey) ExportCarton
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null,dbo.GetColorMultipleID(psd.BrandID,psd.ColorID),psd.SuppColor)
		 ELSE dbo.GetColorMultipleID(psd.BrandID,psd.ColorID)
	 END
) Color
where ted.ID = '{0}'
", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings settingReason = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings settingExportQty = new DataGridViewGeneratorNumericColumnSettings();

            settingReason.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["ReasonDesc"] = string.Empty;
                    return;
                }

                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@ID", e.FormattedValue) };
                string reasonDesc = MyUtility.GetValue.Lookup("select Description from WhseReason with (nolock) where Type = 'TE' and ID = @ID", listPar);

                if (MyUtility.Check.Empty(reasonDesc))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Reason not found");
                    return;
                }

                this.CurrentDetailData["TransferExportReason"] = e.FormattedValue;
                this.CurrentDetailData["ReasonDesc"] = reasonDesc;
            };

            settingReason.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                SelectItem selectItem = new SelectItem("select [Reason] = ID, [ReasonDesc] = Description from WhseReason with (nolock) where type = 'TE'", null, string.Empty);

                DialogResult dialogResult = selectItem.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                this.CurrentDetailData["TransferExportReason"] = selectItem.GetSelecteds()[0]["Reason"];
                this.CurrentDetailData["ReasonDesc"] = selectItem.GetSelecteds()[0]["ReasonDesc"];
            };

            settingExportQty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                new P06_ExportDetail(this.CurrentDetailData).ShowDialog();
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("ColorID", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("PoQty", header: "Po Q'ty", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ExportQty", header: "Export Q'ty", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true, settings: settingExportQty)
                .Numeric("Foc", header: "F.O.C.", decimal_places: 2, width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("BalanceQty", header: "Balance", decimal_places: 2, width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("TransferExportReason", header: "Reason", width: Widths.AnsiChars(8), settings: settingReason)
                .Text("ReasonDesc", header: "Reason Desc", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2, iseditingreadonly: true)
                .Numeric("WeightKg", header: "G.W.(kg)", decimal_places: 2, iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 2, iseditingreadonly: true)
                .Text("ContainerType", header: "ContainerType & No", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.detailgrid.Columns["TransferExportReason"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            string strXltName = Env.Cfg.XltPathDir + $"\\Warehouse_P06_Detail.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            string sqlCmd = string.Format("select * from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Handle"]));
            DataTable tPEPass1;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out tPEPass1);
            string handle = string.Empty;
            string ext = string.Empty;
            string email = string.Empty;

            if (!result || tPEPass1.Rows.Count <= 0)
            {
                handle = string.Empty;
                ext = string.Empty;
                email = string.Empty;
            }
            else
            {
                handle = MyUtility.Convert.GetString(tPEPass1.Rows[0]["Name"]);
                ext = MyUtility.Convert.GetString(tPEPass1.Rows[0]["ExtNo"]);
                email = MyUtility.Convert.GetString(tPEPass1.Rows[0]["EMail"]);
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            worksheet.Cells[2, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["INVNo"]);
            worksheet.Cells[3, 2] = MyUtility.Check.Empty(this.CurrentMaintain["Eta"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["Eta"]).ToString("yyyy/MM/dd");
            worksheet.Cells[3, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Payer"]);
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Consignee"]);
            worksheet.Cells[4, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Blno"]);
            worksheet.Cells[5, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Packages"]);
            worksheet.Cells[5, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Vessel"]);
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["CYCFS"]);
            worksheet.Cells[6, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["NetKg"]) + " / " + MyUtility.Convert.GetString(this.CurrentMaintain["WeightKg"]);
            worksheet.Cells[7, 6] = MyUtility.Convert.GetString(this.DetailDatas.Sum(s => MyUtility.Convert.GetDecimal(s["CBM"])));
            worksheet.Cells[8, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ImportPort"] + "-" + this.CurrentMaintain["ImportCountry"]);
            worksheet.Cells[8, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Remark"]);
            worksheet.Cells[9, 2] = handle;
            worksheet.Cells[9, 6] = ext;
            worksheet.Cells[9, 8] = email;

            int rownum = 11;
            object[,] objArray = new object[1, 20];
            foreach (DataRow dr in this.DetailDatas)
            {
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["ProjectID"];
                objArray[0, 2] = dr["POID"];
                objArray[0, 3] = dr["SCIDlv"];
                objArray[0, 4] = dr["Category"];
                objArray[0, 5] = dr["InspDate"];
                objArray[0, 6] = dr["ToSEQ"];
                objArray[0, 7] = dr["Preshrink"];
                objArray[0, 8] = dr["Supp"];
                objArray[0, 9] = dr["Description"];
                objArray[0, 10] = dr["UnitId"];
                objArray[0, 11] = dr["Color"];
                objArray[0, 12] = dr["SizeSpec"];
                objArray[0, 13] = dr["ExportQty"];
                objArray[0, 14] = dr["Foc"];
                objArray[0, 15] = dr["BalanceQty"];
                objArray[0, 16] = dr["NetKg"];
                objArray[0, 17] = dr["WeightKg"];

                worksheet.Range[string.Format("A{0}:R{0}", rownum)].Value2 = objArray;

                rownum++;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P06_Detail");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion

            return base.ClickPrint();
        }

        private void BtnShippingMark_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["ShipMarkDesc"]), "Shipping Mark", false, null);
            callNextForm.ShowDialog(this);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = -1;

            // 判斷 Poid
            if (this.txtSeq1.CheckEmpty(showErrMsg: false))
            {
                index = this.detailgridbs.Find("InventoryPOID", this.txtLocateSP.Text.TrimEnd());
            }

            // 判斷 Poid + Seq1
            else if (this.txtSeq1.CheckSeq2Empty())
            {
                index = this.detailgridbs.Find("PoidSeq1", this.txtLocateSP.Text.TrimEnd() + this.txtSeq1.Seq1);
            }

            // 判斷 Poid + Seq1 + Seq2
            else
            {
                index = this.detailgridbs.Find("PoidSeq", this.txtLocateSP.Text.TrimEnd() + this.txtSeq1.GetSeq());
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
    }
}
