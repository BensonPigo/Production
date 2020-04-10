using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P08
    /// </summary>
    public partial class P08 : Sci.Win.Tems.Input6
    {
        private string excelFile;

        /// <summary>
        /// P08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type = 'F' and MDivisionID = '{0}'", Env.User.Keyword);
            this.InsertDetailGridOnDoubleClick = false;
            MyUtility.Tool.SetupCombox(this.comboDefectResponsibilityExplanation, 2, 1, "F,Factory,M,Mill,S,Subcon in Local,T,SCI dep. (purchase/s. mrs/sample room)");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };

            this.btnBatch.Enabled = this.Perm.Confirm;
            // GridReplacement 欄位設定
            this.Helper.Controls.Grid.Generator(this.gridReplacement)
          .Text("SP", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
          .Text("NewSeq1", header: "New Seq1", width: Widths.AnsiChars(7), iseditingreadonly: true)
          .Text("NewSeq2", header: "New Seq2", width: Widths.AnsiChars(5), iseditingreadonly: true)
          .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(7), iseditingreadonly: true)
          .Text("Seq", header: "Seq2", width: Widths.AnsiChars(5), iseditingreadonly: true)
          .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
          .Numeric("FinalNeededQty", header: "Final Needed Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
          .Numeric("PoQty", header: "P/O Q’ty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
          .Numeric("PoFoc", header: "P/O FOC", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
          .Numeric("ShipQty", header: "Ship Q’ty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
          .Numeric("ShipFoc", header: "Ship FOC", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
          .CheckBox("Complete", header: "Complete", width: Widths.AnsiChars(10), iseditable: false)
          .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
          ;

        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"select rd.*,(rd.Seq1+' '+rd.Seq2) as Seq, f.Description, [dbo].[getMtlDesc](r.POID,rd.Seq1,rd.Seq2,2,0) as Description,
isnull((select top(1) ExportId from Receiving WITH (NOLOCK) where InvNo = rd.INVNo),'') as ExportID
from ReplacementReport r WITH (NOLOCK) 
inner join ReplacementReport_Detail rd WITH (NOLOCK) on rd.ID = r.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = r.POID and psd.SEQ1 = rd.Seq1 and psd.SEQ2 = rd.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
where r.ID = '{0}'
order by rd.Seq1,rd.Seq2", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            this.toolbar.cmdJunk.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "Junked" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["ApplyName"])) && MyUtility.Check.Empty(this.CurrentMaintain["ApplyDate"]) ? true : false;
            this.toolbar.cmdCheck.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "New" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["ApplyName"])) ? true : false;
            this.toolbar.cmdUncheck.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Checked" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["ApplyName"])) ? true : false;
            this.toolbar.cmdConfirm.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Checked" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["ApvName"])) ? true : false;
            this.toolbar.cmdUnconfirm.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved" && PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["ApvName"])) && MyUtility.Check.Empty(this.CurrentMaintain["TPECFMDate"]) ? true : false;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.btnMailto.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "Junked" && !MyUtility.Check.Empty(this.CurrentMaintain["ApvDate"]) ? true : false;
            this.displayStyleNo.Value = MyUtility.GetValue.Lookup("StyleID", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), "Orders", "ID");
            this.displayPreparedby.Value = MyUtility.Check.Empty(this.CurrentMaintain["ApplyDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ApplyDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat));
            this.displayPPICFactorymgr.Value = MyUtility.Check.Empty(this.CurrentMaintain["ApvDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ApvDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat));
            this.displayConfirmby.Value = MyUtility.Check.Empty(this.CurrentMaintain["TPECFMDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["TPECFMDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat));
            this.displayTPELasteditDate.Value = MyUtility.Check.Empty(this.CurrentMaintain["TPEEditDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["TPEEditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            this.label8.Visible = !MyUtility.Check.Empty(this.CurrentMaintain["RespDeptConfirmDate"]);
            this.numTotalUS.Value =
                MyUtility.Convert.GetDecimal(this.CurrentMaintain["RMtlAmt"]) +
                MyUtility.Convert.GetDecimal(this.CurrentMaintain["ActFreight"]) +
                MyUtility.Convert.GetDecimal(this.CurrentMaintain["EstFreight"]) +
                MyUtility.Convert.GetDecimal(this.CurrentMaintain["SurchargeAmt"]);
            DataRow pOData;
            if (MyUtility.Check.Seek(string.Format("select POSMR,POHandle,PCSMR,PCHandle from PO WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"])), out pOData))
            {
                this.txttpeuserPOSMR.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["POSMR"]);
                this.txttpeuserPOHandle.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["POHandle"]);
                this.txttpeuserPCSMR.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["PCSMR"]);
                this.txttpeuserPCHandle.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["PCHandle"]);
            }
            else
            {
                this.txttpeuserPOSMR.DisplayBox1Binding = string.Empty;
                this.txttpeuserPOHandle.DisplayBox1Binding = string.Empty;
                this.txttpeuserPCSMR.DisplayBox1Binding = string.Empty;
                this.txttpeuserPCHandle.DisplayBox1Binding = string.Empty;
            }

            string sqlcmd = $@"select 1
from Clip where TableName = 'ReplacementReport' AND UniqueKey = '{this.CurrentMaintain["ID"]}'";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                this.btnDownload.ForeColor = Color.Blue;
            }
            else
            {
                this.btnDownload.ForeColor = Color.Black;
            }

            #region 取得Grid Replacement 資料
            DataTable dtRelp;
            string cmdRelp = $@"
select 
[SP] = psd.ID
,[NewSeq1] = psd.SEQ1
,[NewSeq2] = psd.SEQ2
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = psd.Refno
,[FinalNeededQty] = psd.Qty
,[PoQty] = psd.Qty
,[PoFoc] = psd.FOC
,[ShipQty] = psd.ShipQty
,[ShipFoc] = psd.ShipFOC
,[Complete] = psd.Complete
,[Remark] = psd.Remark
from ReplacementReport_Detail rd
inner join PO_Supp_Detail psd on rd.PurchaseID = psd.ID
and rd.SCIRefno = psd.SCIRefno
and rd.ColorID = psd.ColorID
and (psd.SEQ1 = rd.NewSeq1 or psd.OutputSeq1 = rd.NewSeq1)
and (psd.SEQ2 = rd.NewSeq2 or psd.OutputSeq2 = rd.NewSeq2)
where rd.id = '{this.CurrentMaintain["ID"]}'
";

            if (!(result = DBProxy.Current.Select(null,cmdRelp,out dtRelp)))
            {
                this.ShowErr(result);
                return;
            }

            this.gridReplacement.DataSource = dtRelp;

            #endregion

            this.btnResponsibilitydept.ForeColor = MyUtility.Check.Seek($@"
select 1 from ICR_ResponsibilityDept 
where id = '{this.CurrentMaintain["id"]}'") ? Color.Blue : Color.Black;

            this.ChangeRowColor();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings estinqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings actinqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings ttlrequest = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cturequest = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings occurcost = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings desc = new DataGridViewGeneratorTextColumnSettings();

            desc.CharacterCasing = CharacterCasing.Normal;
            estinqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            actinqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            ttlrequest.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            cturequest.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            occurcost.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("Junk", header: "Junk", iseditable: false)
                .Text("Seq", header: "SEQ#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("RefNo", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: desc)
                .Text("INVNo", header: "Invoice#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ETA", header: "ETA", iseditingreadonly: true)
                .Text("ColorID", header: "Color Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("EstInQty", header: "Est. Rced\r\nQ'ty", decimal_places: 2, width: Widths.AnsiChars(7), settings: estinqty, iseditingreadonly: true)
                .Numeric("ActInQty", header: "Actual Rced\r\nQ'ty", decimal_places: 2, width: Widths.AnsiChars(7), settings: actinqty, iseditingreadonly: true)
                .Numeric("TotalRequest", header: "Inspection Total\r\nReplacement Request\r\nQty", decimal_places: 2, width: Widths.AnsiChars(7), settings: ttlrequest, iseditingreadonly: true)
                .Numeric("AfterCuttingRequest", header: "After Cutting\r\nReplacement\r\nRequest Qty", decimal_places: 2, width: Widths.AnsiChars(7), settings: cturequest, iseditingreadonly: true)
                .Numeric("FinalNeedQty", header: "Final Needed Q'ty", decimal_places: 2, width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("ReplacementUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("DamageSendDate", header: "Damage\r\nSample Sent\r\nDate", iseditingreadonly: true)
                .Text("AWBNo", header: "AWB# Of\r\nDamage\r\nSample", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ReplacementETA", header: "Replacement\r\nETA", iseditingreadonly: true)
                .Numeric("OccurCost", header: "Cost Occurred", decimal_places: 3, width: Widths.AnsiChars(7), settings: occurcost, iseditingreadonly: true)
                .EditText("ResponsibilityReason", header: "Reason", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .EditText("Suggested", header: "Factory Suggested Solution", width: Widths.AnsiChars(30), iseditingreadonly: true);

            this.detailgrid.CellDoubleClick += (s, e) =>
            {
                if (e.ColumnIndex == 1)
                {
                    Sci.Production.PPIC.P08_InputData callInputDataForm = new Sci.Production.PPIC.P08_InputData(this.CurrentMaintain);
                    callInputDataForm.Set(this.EditMode, this.DetailDatas, this.CurrentDetailData);
                    callInputDataForm.ShowDialog(this);
                }
            };

            this.detailgrid.Sorted += (s, e) =>
            {
                this.ChangeRowColor();
            };
        }

        private void ChangeRowColor()
        {
            foreach (DataGridViewRow gridRow in this.detailgrid.Rows)
            {
                if (MyUtility.Convert.GetBool(gridRow.Cells["Junk"].Value))
                {
                    gridRow.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "F";
            this.CurrentMaintain["SendToTrade"] = 0;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["ApvDate"]))
            {
                MyUtility.Msg.WarningBox("This record is approved, can't be modified!!");
                return false;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Junked")
            {
                MyUtility.Msg.WarningBox("This record is junked, can't be modified!!");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["POID"]))
            {
                MyUtility.Msg.WarningBox("SP No. can't empty");
                this.txtSPNo.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ApplyName"]))
            {
                MyUtility.Msg.WarningBox("Prepared by can't empty");
                this.txtuserPreparedby.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ApvName"]))
            {
                MyUtility.Msg.WarningBox("PPIC/Factory mgr can't empty");
                this.txtuserPPICFactorymgr.TextBox1.Focus();
                return false;
            }

            #endregion

            if (!this.CheckRequestQty())
            {
                return false;
            }

            int count = 0; // 紀錄表身筆數
            // 刪除表身Grid的Seq為空資料
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Seq"]))
                {
                    dr.Delete();
                    continue;
                }

                count++;
            }

            if (count == 0)
            {
                MyUtility.Msg.WarningBox("Deatil can't empty!!");
                return false;
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Factory + MyUtility.Convert.GetString(this.CurrentMaintain["POID"]).Substring(0, 8), "ReplacementReport", DateTime.Today, 6, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            // 將表頭的Responsibility存至detail
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["Responsibility"] = this.CurrentMaintain["Responsibility"];
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            List<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()));
            string sqlcmd = "Select r.ApvDate From ReplacementReport r WITH (NOLOCK) where r.ID = @ID";
            DataTable apvDate;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, cmds, out apvDate);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (MyUtility.Check.Empty(apvDate.Rows[0][0]))
            {
                MyUtility.Msg.WarningBox("It dosen't approve.");
                return false;
            }

            this.ToExcel(false);

            return base.ClickPrint();
        }

        private bool ToExcel(bool isSendMail)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("No data!!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_P08.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            DataTable gridData = (DataTable)this.detailgridbs.DataSource;

            // 計算總輸出頁數
            int totalPage = (int)Math.Ceiling(MyUtility.Convert.GetDecimal(gridData.Rows.Count) / 2);

            if (totalPage > 1)
            {
                // 選取要被複製的資料
                Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A1:A35").EntireRow;
                for (int i = 2; i <= totalPage; i++)
                {
                    // 選擇要被貼上的位置
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString((35 * (i - 1)) + 1)), Type.Missing).EntireRow;

                    // 貼上
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                }
            }

            string attention = MyUtility.GetValue.Lookup(string.Format("select Name from TPEPass1 WITH (NOLOCK) where ID = '{0}'", this.txttpeuserPCHandle.DisplayBox1Binding));
            string apply = MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ApplyName"])));
            string approve = MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ApvName"])));
            string style = MyUtility.GetValue.Lookup(string.Format("select top 1 StyleID from Orders WITH (NOLOCK) where POID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"])));
            string confirm = MyUtility.GetValue.Lookup(string.Format("select Name from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["TPECFMName"])));
            int j = 0;
            int n = 0;

            string responsibility = MyUtility.Convert.GetString(this.CurrentMaintain["Responsibility"]);
            foreach (DataRow dr in gridData.Rows)
            {
                j++;
                int row = 36 * ((int)Math.Ceiling(MyUtility.Convert.GetDecimal(j) / 2) - 1);
                int column = 7;

                // 填表頭資料
                if (j % 2 == 1)
                {
                    n++;
                    worksheet.Cells[row + 3, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]) + " - " + n + "/" + MyUtility.Convert.GetString(totalPage);
                    worksheet.Cells[row + 4, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]);
                    worksheet.Cells[row + 4, 6] = MyUtility.Check.Empty(this.CurrentMaintain["ApvDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ApvDate"]).ToString("yyyy/MM/dd");
                    worksheet.Cells[row + 4, 8] = MyUtility.Check.Empty(this.CurrentMaintain["ApplyDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ApplyDate"]).ToString("yyyy/MM/dd");
                    worksheet.Cells[row + 5, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["POID"]);
                    worksheet.Cells[row + 5, 6] = MyUtility.Check.Empty(this.CurrentMaintain["TPECFMDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["TPECFMDate"]).ToString("yyyy/MM/dd");
                    worksheet.Cells[row + 5, 8] = attention;
                    worksheet.Cells[row + 6, 2] = apply + "/" + approve;
                    worksheet.Cells[row + 6, 8] = style;
                    column = 5;
                }

                worksheet.Cells[row + 7, column] = "#" + MyUtility.Convert.GetString(dr["RefNo"]) + " " + MyUtility.Convert.GetString(dr["Description"]) + "(" + MyUtility.Convert.GetString(dr["Seq1"]) + " -" + MyUtility.Convert.GetString(dr["Seq2"]) + ")";
                worksheet.Cells[row + 8, column] = MyUtility.Convert.GetString(dr["INVNo"]) + ", " + (MyUtility.Check.Empty(dr["ETA"]) ? string.Empty : "ETA" + Convert.ToDateTime(dr["ETA"]).ToString("d"));
                worksheet.Cells[row + 9, column] = MyUtility.Convert.GetString(dr["ColorID"]);
                worksheet.Cells[row + 10, column] = MyUtility.Convert.GetString(dr["EstInQty"]) + "/" + MyUtility.Convert.GetString(dr["ActInQty"]);
                worksheet.Cells[row + 12, column] = MyUtility.Convert.GetString(dr["AGradeDefect"]);
                worksheet.Cells[row + 12, column + 1] = MyUtility.Convert.GetString(dr["AGradeRequest"]);
                worksheet.Cells[row + 14, column] = MyUtility.Convert.GetString(dr["BGradeDefect"]);
                worksheet.Cells[row + 14, column + 1] = MyUtility.Convert.GetString(dr["BGradeRequest"]);
                worksheet.Cells[row + 16, column] = MyUtility.Convert.GetString(dr["NarrowWidth"]);
                worksheet.Cells[row + 16, column + 1] = MyUtility.Convert.GetString(dr["NarrowRequest"]);
                worksheet.Cells[row + 17, column] = MyUtility.Check.Empty(dr["Other"]) ? MyUtility.Convert.GetString(dr["OtherReason"]) : MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Damage Reason' and ID = '{0}'", MyUtility.Convert.GetString(dr["Other"])));
                worksheet.Cells[row + 17, column + 1] = MyUtility.Convert.GetString(dr["OtherRequest"]);
                worksheet.Cells[row + 18, column] = MyUtility.Convert.GetString(dr["TotalRequest"]);
                worksheet.Cells[row + 19, column] = MyUtility.Check.Empty(dr["AfterCutting"]) ? MyUtility.Convert.GetString(dr["AfterCuttingReason"]) : MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Damage Reason' and ID = '{0}'", MyUtility.Convert.GetString(dr["AfterCutting"])));
                worksheet.Cells[row + 20, column] = MyUtility.Convert.GetString(dr["AfterCuttingRequest"]);
                string finalNeedQty = string.Format("{0:N2}", MyUtility.Convert.GetDecimal(dr["FinalNeedQty"])) + (MyUtility.Check.Empty(dr["ReplacementUnit"]) ? string.Empty : "  " + dr["ReplacementUnit"].ToString());
                worksheet.Cells[row + 21, column] = finalNeedQty;
                worksheet.Cells[row + 22, column] = MyUtility.Check.Empty(dr["DamageSendDate"]) ? string.Empty : Convert.ToDateTime(dr["DamageSendDate"]).ToString("d");
                worksheet.Cells[row + 23, column] = MyUtility.Convert.GetString(dr["AWBNo"]);
                worksheet.Cells[row + 24, column] = MyUtility.Check.Empty(dr["ReplacementETA"]) ? string.Empty : Convert.ToDateTime(dr["ReplacementETA"]).ToString("d");
                worksheet.Cells[row + 24, 3] = responsibility == "M" ? "V" : string.Empty;
                worksheet.Cells[row + 25, column] = responsibility == "M" ? MyUtility.Convert.GetString(dr["ResponsibilityReason"]) : string.Empty;
                worksheet.Cells[row + 25, 3] = responsibility == "T" ? "V" : string.Empty;
                worksheet.Cells[row + 26, column] = responsibility == "T" ? MyUtility.Convert.GetString(dr["ResponsibilityReason"]) : string.Empty;
                worksheet.Cells[row + 26, 3] = responsibility == "F" ? "V" : string.Empty;
                worksheet.Cells[row + 27, column] = responsibility == "F" ? MyUtility.Convert.GetString(dr["ResponsibilityReason"]) : string.Empty;
                worksheet.Cells[row + 27, 3] = responsibility == "S" ? "V" : string.Empty;
                worksheet.Cells[row + 28, column] = responsibility == "S" ? MyUtility.Convert.GetString(dr["ResponsibilityReason"]) : string.Empty;
                worksheet.Cells[row + 29, column] = MyUtility.Convert.GetString(dr["Suggested"]);
                worksheet.Cells[row + 29, column] = apply;
                worksheet.Cells[row + 31, column + 1] = approve;
                worksheet.Cells[row + 32, column] = MyUtility.Convert.GetString(dr["OccurCost"]);
                worksheet.Cells[row + 33, column] = confirm;
            }

            worksheet.Protect(Password: "Sport2006");

            #region Save Excel
            this.excelFile = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P08");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(this.excelFile);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            #endregion
            if (!isSendMail)
            {
                this.excelFile.OpenFile();
            }

            return true;
        }

        // SP No.
        private void TxtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(this.txtSPNo.Text))
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@poid", this.txtSPNo.Text);

                    // 用登入的Factory 抓取對應的FtyGroup
                     DataTable ftyGroupData;
                     DBProxy.Current.Select(null, string.Format("select FTYGroup from Factory where id='{0}' and IsProduceFty = 1", Sci.Env.User.Factory), out ftyGroupData);
                    if (ftyGroupData.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("SP No. not found!!");
                        this.CurrentMaintain["POID"] = string.Empty;
                        this.CurrentMaintain["FactoryID"] = string.Empty;
                        this.CurrentMaintain.EndEdit();
                        return;
                    }

                     System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@factoryid", ftyGroupData.Rows[0]["FTYGroup"].ToString());

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    string sqlCmd = "select ID,FtyGroup from Orders WITH (NOLOCK) where POID = @poid and FtyGroup  = @factoryid";
                    DataTable ordersData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out ordersData);

                    if (!result || ordersData.Rows.Count <= 0)
                    {
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("SP No. not found!!");
                        }

                        this.CurrentMaintain["POID"] = string.Empty;
                        this.CurrentMaintain["FactoryID"] = string.Empty;
                        this.CurrentMaintain.EndEdit();
                        return;
                    }
                    else
                    {
                        this.CurrentMaintain["POID"] = this.txtSPNo.Text;
                        this.CurrentMaintain["FactoryID"] = ordersData.Rows[0]["FtyGroup"];
                        this.CurrentMaintain.EndEdit();
                    }
                }
            }
        }

        // SP No.
        private void TxtSPNo_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                // 清空表身Grid資料
                foreach (DataRow dr in this.DetailDatas)
                {
                    dr.Delete();
                }

                string sqlCmd = string.Format(
                    @"select f.Seq1,f.Seq2, left(f.Seq1+' ',3)+f.Seq2 as Seq,f.Refno,
[dbo].getMtlDesc(f.POID,f.Seq1,f.Seq2,2,0) as Description,
isnull(psd.ColorID,'') as ColorID,isnull(r.InvNo,'') as InvNo,iif(e.Eta is null,r.ETA,e.ETA) as ETA,isnull(r.ExportId,'') as ExportId,
isnull(sum(fp.TicketYds),0) as EstInQty, isnull(sum(fp.ActualYds),0) as ActInQty
from FIR f WITH (NOLOCK) 
left join FIR_Physical fp WITH (NOLOCK) on f.ID = fp.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on f.POID = psd.ID and f.Seq1 = psd.SEQ1 and f.Seq2 = psd.SEQ2
left join Receiving r WITH (NOLOCK) on f.ReceivingID = r.Id
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
where f.POID = '{0}' and f.Result = 'F'
group by f.Seq1,f.Seq2, left(f.Seq1+' ',3)+f.Seq2,f.Refno,[dbo].getMtlDesc(f.POID,f.Seq1,f.Seq2,2,0),psd.ColorID,r.InvNo,iif(e.Eta is null,r.ETA,e.ETA),isnull(r.ExportId,'')", this.txtSPNo.Text);
                DataTable firData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out firData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Query FIR fail!\r\n" + result.ToString());
                }
                else
                {
                    foreach (DataRow dr in firData.Rows)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
                    }
                }

                this.displayStyleNo.Value = MyUtility.GetValue.Lookup("StyleID", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), "Orders", "ID");
                DataRow poData;
                if (MyUtility.Check.Seek(string.Format("select POSMR,POHandle,PCSMR,PCHandle from PO WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"])), out poData))
                {
                    this.txttpeuserPOSMR.DisplayBox1Binding = MyUtility.Convert.GetString(poData["POSMR"]);
                    this.txttpeuserPOHandle.DisplayBox1Binding = MyUtility.Convert.GetString(poData["POHandle"]);
                    this.txttpeuserPCSMR.DisplayBox1Binding = MyUtility.Convert.GetString(poData["PCSMR"]);
                    this.txttpeuserPCHandle.DisplayBox1Binding = MyUtility.Convert.GetString(poData["PCHandle"]);
                }
                else
                {
                    this.txttpeuserPOSMR.DisplayBox1Binding = string.Empty;
                    this.txttpeuserPOHandle.DisplayBox1Binding = string.Empty;
                    this.txttpeuserPCSMR.DisplayBox1Binding = string.Empty;
                    this.txttpeuserPCHandle.DisplayBox1Binding = string.Empty;
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            DualResult result;
            string updateCmd = string.Format("update ReplacementReport set Status = 'Junked', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Junk fail!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickCheck()
        {
            base.ClickCheck();
            StringBuilder check = new StringBuilder();
            IList<string> updateCmds = new List<string>();
            if (!this.CheckRequestQty())
            {
                return;
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(dr["Responsibility"]) || MyUtility.Check.Empty(dr["ResponsibilityReason"]) || MyUtility.Check.Empty(dr["Suggested"]))
                {
                    check.Append(string.Format("SEQ# {0}\r\n", MyUtility.Convert.GetString(dr["Seq"])));
                }

                updateCmds.Add(string.Format(
                    @"update FIR set ReplacementReportID = '{0}' where ReceivingID in (select distinct r.Id
from ReplacementReport rr 
inner join ReplacementReport_Detail rrd on rr.ID = rrd.ID
inner join Receiving r on rrd.INVNo = r.InvNo
inner join Receiving_Detail rd on rd.Id = r.Id and rr.POID = rd.PoId and rrd.Seq1 = rd.Seq1 and rrd.Seq2 = rd.Seq2
where rr.ID = '{0}') and POID = '{1}' and Seq1 = '{2}' and Seq2 = '{3}' and ReplacementReportID = '';",
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["POID"]),
                    MyUtility.Convert.GetString(dr["Seq1"]),
                    MyUtility.Convert.GetString(dr["Seq2"])));
            }

            if (check.Length > 0)
            {
                MyUtility.Msg.WarningBox(check.ToString() + "<Defect Responsibility> and <Reason> and <Factory Suggested Solution> can't empty!!");
                return;
            }

            updateCmds.Add(string.Format("update ReplacementReport set Status = 'Checked', ApplyDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}';", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check fail!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUncheck()
        {
            base.ClickUncheck();
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to uncheck this data?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update ReplacementReport set Status = 'New', ApplyDate = Null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}';", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            updateCmds.Add(string.Format(
                @"update FIR 
set ReplacementReportID = isnull((select top(1) rr.ID
						   from ReplacementReport rr
						   inner join ReplacementReport_Detail rrd on rr.ID = rrd.ID
						   where rr.CDate = (select MIN(r.CDate)
											 from ReplacementReport r
											 inner join ReplacementReport_Detail rd on r.ID = rd.ID
											 inner join Receiving rc on rd.INVNo = rc.InvNo
											 where r.ID != '{0}' and r.ApplyDate is not null and r.POID = FIR.POID and rd.Seq1 = FIR.Seq1 and rd.Seq2 = FIR.Seq2 and rc.Id = FIR.ReceivingID)
						   and rr.ID != '{0}' and rr.ApplyDate is not null and rr.POID = FIR.POID and rrd.Seq1 = FIR.Seq1 and rrd.Seq2 = FIR.Seq2),'') 
where ReplacementReportID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Uncheck fail!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult result;
            if (!this.CheckRequestQty())
            {
                return;
            }

            this.SendMail();

            result = Prgs.PostReplacementReportToTrade(this.CurrentMaintain["ID"].ToString());
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string updateCmd = string.Format("update ReplacementReport set SendToTrade = 1,Status = 'Approved', ApvDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }

            this.RenewData();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to unconfirm this data?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            DualResult result;
            string updateCmd = string.Format("update ReplacementReport set Status = 'Checked', ApvDate = null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
        }

        // Mail to
        private void BtnMailto_Click(object sender, EventArgs e)
        {
            this.SendMail();
        }

        // Mail to
        private void SendMail()
        {
                DataTable allMail;
                string sqlCmd = string.Format(
                    @"select isnull((select EMail from Pass1 WITH (NOLOCK) where ID = r.ApplyName),'') as ApplyName,
isnull((select Name from Pass1 WITH (NOLOCK) where ID = r.ApvName),'') as ApvName,
isnull((select Email from Pass1 WITH (NOLOCK) where ID = r.ApvName),'') as CCMAIL,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = o.MRHandle),'') as MRHandle,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = o.SMR),'') as SMR,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = p.POHandle),'') as POHandle,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = p.POSMR),'') as POSMR,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = p.PCHandle),'') as PCHandle,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = p.PCSMR),'') as PCSMR 
from ReplacementReport r WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = r.POID
left join PO p WITH (NOLOCK) on p.ID = o.POID
where r.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out allMail);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query mail list fail.\r\n" + result.ToString());
                    return;
                }

                string mailto = MyUtility.Convert.GetString(allMail.Rows[0]["POSMR"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["POHandle"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["PCSMR"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["PCHandle"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["SMR"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["MRHandle"]) + ";";
                string cc = MyUtility.Convert.GetString(allMail.Rows[0]["ApplyName"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["CCMAIL"]) + ";";
                string subject = string.Format("{0} - Fabric Replacement report", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                StringBuilder content = new StringBuilder();
                #region 組Content
                content.Append(@"Hi PO Handle,

Please refer attached replacement report and confirm rcvd in reply. The defect sample will send via courier AWB#      on     .please clarify with supplier and advise the result. Thanks.
If the replacement report can be accept and cfm to proceed, please approve it through system

");
                #endregion

                // 產生Excel
                this.ToExcel(true);

                // 帶出夾檔的檔案
                sqlCmd = string.Format("select *, YEAR(AddDate) as Year, MONTH(AddDate) as Month from Clip WITH (NOLOCK) where TableName = '{0}' and UniqueKey = '{1}'", "ReplacementReport", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                DataTable clipData;
                double totalSize = 0;
                string totalFile;
                StringBuilder allFile = new StringBuilder();
                result = DBProxy.Current.Select(null, sqlCmd, out clipData);
                if (result && clipData.Rows.Count > 0)
                {
                    foreach (DataRow dr in clipData.Rows)
                    {
                        // var dataFile = dr;
                        // string targetFile;
                        // Sci.Win.PrivUtils.GetClipFileName(dataFile, out targetFile);
                        string targetFile = Env.Cfg.ClipDir + "\\" + MyUtility.Convert.GetString(dr["Year"]) + Convert.ToString(dr["Month"]).PadLeft(2, '0') + "\\" + MyUtility.Convert.GetString(dr["TableName"]) + MyUtility.Convert.GetString(dr["PKey"]) + MyUtility.Convert.GetString(dr["SourceFile"]).Substring(MyUtility.Convert.GetString(dr["SourceFile"]).LastIndexOf('.'));
                        allFile.Append(string.Format(",{0}", targetFile));
                        System.IO.FileInfo fi = new System.IO.FileInfo(targetFile);
                        totalSize += (double)fi.Length;
                    }
                }

                if (totalSize > 10485760)
                {
                    // 當To Excel的檔案與迴紋針裡的檔案加起來超過10MB的話，就在信件中顯示下面訊息，附件只夾To Excel的檔案
                    content.Append("Due to the attach files is more than 10MB, please ask factory's related person to provide the attach file.");
                    totalFile = this.excelFile;
                }
                else
                {
                    totalFile = this.excelFile + allFile.ToString();
                }

                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, cc, subject, totalFile, content.ToString(), false, true);
                email.ShowDialog(this);

                // 刪除Excel File
                if (System.IO.File.Exists(this.excelFile))
                {
                    try
                    {
                        System.IO.File.Delete(this.excelFile);
                    }
                    catch (System.IO.IOException)
                    {
                        MyUtility.Msg.WarningBox("Delete excel file fail!!");
                    }
                }
        }

        private void P08_FormLoaded(object sender, EventArgs e)
        {
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
        }

        /// <summary>
        /// 確認表身Request Qty 是否皆為0
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckRequestQty()
        {
            this.detailgridbs.EndEdit();
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(dr["TotalRequest"]) && MyUtility.Check.Empty(dr["AfterCuttingRequest"]))
                    {
                        MyUtility.Msg.WarningBox("The request Qty cannot all be 0!");
                        return false;
                    }
                }
            }

            return true;
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            P08_Download callInputDataForm = new P08_Download(this.CurrentMaintain);
            callInputDataForm.ShowDialog(this);
        }

        private void BtnFreightList_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new P08_FreightList(this.CurrentMaintain);
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void BtnResponsibilitydept_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            bool canEdit = this.Perm.Edit && this.Perm.Send && MyUtility.Check.Empty(this.CurrentMaintain["RespDeptConfirmDate"]) && MyUtility.Check.Empty(this.CurrentMaintain["VoucherDate"]);
            var frm = new P21_ResponsibilityDept(canEdit, this.CurrentMaintain["ID"].ToString(), null, null, "Replacement");
            frm.ShowDialog(this);
            frm.Dispose();
            this.OnRefreshClick();
            this.OnDetailEntered();
        }

        private void BtnBatch_Click(object sender, EventArgs e)
        {
            var frm = new P08_BatchConfirmRespDept("F");
            frm.ShowDialog(this);
            frm.Dispose();
            this.ReloadDatas();
        }
    }
}
