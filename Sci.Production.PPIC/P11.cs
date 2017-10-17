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
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    public partial class P11 : Sci.Win.Tems.Input6
    {

        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "' and FabricType = 'A'";
            txtuserApprove.TextBox1.ReadOnly = true;
            txtuserApprove.TextBox1.IsSupportEditMode = false;
            InsertDetailGridOnDoubleClick = false;
            displayIssueLackDate.ReadOnly = true;          
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select ld.*,(left(ld.Seq1+' ',3)+ld.Seq2) as Seq,isnull(psd.Refno,'') as Refno,dbo.getMtlDesc(l.POID,ld.Seq1,ld.Seq2,1,0) as Description,
isnull(p.Description,'') as PPICReasonDesc
from Lack l WITH (NOLOCK) 
inner join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
left join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
left join PPICReason p WITH (NOLOCK) on p.Type = 'AL' and ld.PPICReasonID = p.ID
where l.ID = '{0}'
order by ld.Seq1,ld.Seq2", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboType, 2, 1, "L,Lacking,R,Replacement");
            MyUtility.Tool.SetupCombox(comboShift, 2, 1, "D,Day,N,Night,O,Subcon-Out");
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings seq = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings refno = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings reason = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings inqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings outqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings requestqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings issueqty = new DataGridViewGeneratorNumericColumnSettings();
            Dictionary<string, string> processSource = new Dictionary<string, string>() { { "Automation", "Automation" }, { "Bonding", "Bonding" }, { "Cutting", "Cutting" }, { "Embroidery", "Embroidery" }, { "Heat transfer", "Heat transfer" }, { "Printing", "Printing" }, { "Sewing", "Sewing" }, { "Loading", "Loading" }, { "PPA", "PPA" } };
            DataGridViewGeneratorComboBoxColumnSettings process = new DataGridViewGeneratorComboBoxColumnSettings() { DataSource = new System.Windows.Forms.BindingSource(processSource, null), ValueMember = "Value", DisplayMember = "Value" }; ;

            inqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            outqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            requestqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            issueqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            #region Seq按右鍵與Validating
            seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = Prgs.SelePoItem(MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(dr["Seq"]), "FabricType = 'A'");
                            DialogResult result = item.ShowDialog();
                            if (result == DialogResult.Cancel) { return; }
                            IList<DataRow> selectData = item.GetSelecteds();
                            dr["Seq"] = item.GetSelectedString();
                            dr["Seq1"] = MyUtility.Convert.GetString(selectData[0]["Seq1"]);
                            dr["Seq2"] = MyUtility.Convert.GetString(selectData[0]["Seq2"]);
                            dr["RefNo"] = MyUtility.Convert.GetString(selectData[0]["RefNo"]);
                            dr["Description"] = MyUtility.Convert.GetString(selectData[0]["Description"]);
                            DataTable WHdata;
                            DualResult whdr = DBProxy.Current.Select(null, string.Format("SELECT m.InQty,m.OutQty FROM MDivisionPoDetail m WITH (NOLOCK) inner join Orders o WITH (NOLOCK) on m.POID=o.ID inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup WHERE m.POID = '{0}' AND m.Seq1 = '{1}' AND m.Seq2 = '{2}' AND f.MDivisionID = '{3}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"]), Sci.Env.User.Keyword), out WHdata);
                            if (whdr)
                            {
                                if (WHdata.Rows.Count > 0)
                                {
                                    dr["WhseInQty"] = MyUtility.Convert.GetDecimal(WHdata.Rows[0]["InQty"]);
                                    dr["FTYInQty"] = MyUtility.Convert.GetDecimal(WHdata.Rows[0]["OutQty"]);
                                }
                            }
                            DateTime? maxIssueDate = MaxIssueDate(MyUtility.Convert.GetString(selectData[0]["Seq1"]), MyUtility.Convert.GetString(selectData[0]["Seq2"]));
                            if (MyUtility.Check.Empty(maxIssueDate))
                            {
                                dr["FTYLastRecvDate"] = DBNull.Value;
                            }
                            else
                            {
                                dr["FTYLastRecvDate"] = maxIssueDate;
                            }
                            dr.EndEdit();
                        }
                    }
                }
            };

            seq.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Seq"]))
                    {
                        if (MyUtility.Check.Empty(CurrentMaintain["POID"]))
                        {
                            ClearGridData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("SP# can't empty!!");
                            return;
                        }

                        if (MyUtility.Convert.GetString(e.FormattedValue).IndexOf("'") != -1)
                        {
                            ClearGridData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                            return;
                        }

                        DataRow poData;

                        //bug fix:直接輸入seq會有錯誤訊息
                        //string sqlCmd = string.Format(@"select left(seq1+' ',3)+seq2 as Seq, Refno,InQty,OutQty,seq1,seq2, 
                        //dbo.getmtldesc(id,seq1,seq2,2,0) as Description 
                        //from dbo.PO_Supp_Detail
                        //where id ='{0}' and seq1 = '{1}' and seq2 = '{2}' and FabricType = 'A'", MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(e.FormattedValue).Substring(0, 3), MyUtility.Convert.GetString(e.FormattedValue).Substring(2, 2));
                        string seq12 = MyUtility.Convert.GetString(e.FormattedValue);
                        char[] ch1 = new Char[] {' '}; 
                        string[] inputString = seq12.Split(ch1,StringSplitOptions.RemoveEmptyEntries);
                        if (inputString.Length < 2 || inputString.Length > 3)
                        {
                            ClearGridData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Please input legal seq#!! example:01 03");
                            return;
                        }
                        string sqlCmd = string.Format(@"select left(psd.seq1+' ',3)+psd.seq2 as Seq, psd.Refno,isnull(m.InQty,0) as InQty,isnull(m.OutQty,0) as OutQty,psd.seq1,psd.seq2, dbo.getmtldesc(psd.id,psd.seq1,psd.seq2,2,0) as Description 
                        from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
                        left join MDivisionPoDetail m WITH (NOLOCK) on m.POID = psd.ID and m.Seq1 = psd.SEQ1 and m.Seq2 = psd.SEQ2
                        inner join dbo.Factory F WITH (NOLOCK) on F.id=psd.factoryid and F.MDivisionID='{3}'
                        where psd.id ='{0}' and psd.seq1 = '{1}' and psd.seq2 = '{2}' and psd.FabricType = 'A'",
                        MyUtility.Convert.GetString(CurrentMaintain["POID"]),inputString[0],inputString[1] , Sci.Env.User.Keyword);

                        if (!MyUtility.Check.Seek(sqlCmd, out poData))
                        {
                            ClearGridData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > not found!!!", MyUtility.Convert.GetString(e.FormattedValue)));
                            return;
                        }
                        else
                        {
                            dr["Seq"] = MyUtility.Convert.GetString(poData["Seq"]);
                            dr["Seq1"] = MyUtility.Convert.GetString(poData["Seq1"]);
                            dr["Seq2"] = MyUtility.Convert.GetString(poData["Seq2"]);
                            dr["RefNo"] = MyUtility.Convert.GetString(poData["RefNo"]);
                            dr["Description"] = MyUtility.Convert.GetString(poData["Description"]);
                            dr["WhseInQty"] = MyUtility.Convert.GetDecimal(poData["InQty"]);
                            dr["FTYInQty"] = MyUtility.Convert.GetDecimal(poData["OutQty"]);
                            DateTime? maxIssueDate = MaxIssueDate(MyUtility.Convert.GetString(poData["Seq1"]), MyUtility.Convert.GetString(poData["Seq2"]));
                            if (MyUtility.Check.Empty(maxIssueDate))
                            {
                                dr["FTYLastRecvDate"] = DBNull.Value;
                            }
                            else
                            {
                                dr["FTYLastRecvDate"] = maxIssueDate;
                            }
                            dr.EndEdit();
                        }
                    }
                }
            };

            #endregion

            #region RefNo的CoubleClick
            refno.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(dr["Description"]), "Description", false, null);
                        callNextForm.ShowDialog(this);

                    }
                }
            };
            #endregion

            #region Request Qty Validating
            requestqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (MyUtility.Convert.GetString(CurrentMaintain["Type"]) == "R")
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                        if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["RequestQty"]))
                        {
                            dr["RequestQty"] = e.FormattedValue;
                            dr["RejectQty"] = e.FormattedValue;
                                dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            #region PPICReasonID按右鍵與Validating
            reason.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID,Description from PPICReason WITH (NOLOCK) where Type = 'AL' and Junk = 0 and TypeForUse = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Type"])), "5,40", MyUtility.Convert.GetString(dr["PPICReasonID"]));
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            IList<DataRow> selectData = item.GetSelecteds();
                            dr["PPICReasonID"] = item.GetSelectedString();
                            dr["PPICReasonDesc"] = MyUtility.Convert.GetString(selectData[0]["Description"]);
                            dr.EndEdit();
                        }
                    }
                }
            };

            reason.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Seq"]))
                    {
                        if (MyUtility.Convert.GetString(e.FormattedValue).IndexOf("'") != -1)
                        {
                            dr["PPICReasonID"] = "";
                            dr["PPICReasonDesc"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                            return;
                        }

                        DataRow reasonData;
                        string sqlCmd = string.Format(@"select ID,Description from PPICReason WITH (NOLOCK) where Type = 'AL' and Junk = 0 and TypeForUse = '{0}' and ID = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["Type"]), MyUtility.Convert.GetString(e.FormattedValue));
                        if (!MyUtility.Check.Seek(sqlCmd, out reasonData))
                        {
                            dr["PPICReasonID"] = "";
                            dr["PPICReasonDesc"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Reason Id: {0} > not found!!!", MyUtility.Convert.GetString(e.FormattedValue)));
                            return;
                        }
                        else
                        {
                            dr["PPICReasonID"] = MyUtility.Convert.GetString(e.FormattedValue);
                            dr["PPICReasonDesc"] = MyUtility.Convert.GetString(reasonData["Description"]);
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(5), settings: seq)
                .Text("RefNo", header: "Refer#", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: refno)
                .Date("FTYLastRecvDate", header: "Prod. Last Rcvd Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("FTYInQty", header: "Prod. Accu. Rcvd Qty", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true, settings: outqty)
                .Numeric("WhseInQty", header: "WH Accu. Rcvd Qty", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true, settings: inqty)  
                .Numeric("RequestQty", header: "Request Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, maximum: 99999999.99M, minimum: 0, settings: requestqty)
                .Numeric("RejectQty", header: "# of pcs rejected")
                .Numeric("IssueQty", header: "Issue Qty upon request", decimal_places: 2, iseditingreadonly: true, settings: issueqty)
                .ComboBox("Process", header: "Process", width: Widths.AnsiChars(15), settings: process)
                .Text("PPICReasonID", header: "Reason Id", width: Widths.AnsiChars(5), settings: reason)
                .EditText("PPICReasonDesc", header: "Reason", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        private void ClearGridData(DataRow dr)
        {
            dr["Seq"] = "";
            dr["Seq1"] = "";
            dr["Seq2"] = "";
            dr["RefNo"] = "";
            dr["Description"] = "";
            dr["WhseInQty"] = 0;
            dr["FTYInQty"] = 0;
            dr["FTYLastRecvDate"] = DBNull.Value;
            dr.EndEdit();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["IssueDate"] = DateTime.Today;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FabricType"] = "A";
            CurrentMaintain["ApplyName"] = Sci.Env.User.UserID;
            CurrentMaintain["Status"] = "New";
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't modify.");
                return false;
            }
            return true;
        }
       
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't delete.");
                return false;
            }
            return true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("Type can't empty");
                comboType.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Shift"]))
            {
                MyUtility.Msg.WarningBox("Shift can't empty");
                comboShift.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("SP# can't empty");
                txtSPNo.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("Sewing Line can't empty");
                txtSewingLine.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ApplyName"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty");
                txtuserHandle.TextBox1.Focus();
                return false;
            }
            #endregion
            int i = 0; //計算表身Grid的總筆數
            foreach (DataRow dr in DetailDatas)
            {
                #region 刪除表身Seq為空白的資料
                if (MyUtility.Check.Empty(dr["Seq"]))
                {
                    dr.Delete();
                    continue;
                }
                #endregion
                i++;
                #region 表身的RequestQty不可小於0、Reason不可為空 、Type='R'時RejectQty不可小(等)於0
                if (MyUtility.Check.Empty(dr["RequestQty"]) || MyUtility.Convert.GetDecimal(dr["RequestQty"]) <= 0)
                {
                    MyUtility.Msg.WarningBox("< Request Qty >  can't equal or less 0!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["PPICReasonID"]))
                {
                    MyUtility.Msg.WarningBox("< Reason Id >  can't empty!");
                    return false;
                }

                if (MyUtility.Convert.GetString(CurrentMaintain["Type"]) == "R" && (MyUtility.Check.Empty(dr["RejectQty"]) || MyUtility.Convert.GetInt(dr["RejectQty"]) <= 0))
                {
                    MyUtility.Msg.WarningBox("< # of pcs rejected >  can't equal or less 0!");
                    return false;
                }

                #endregion
            }

            //表身Grid資料不可為空
            if (i == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }

            //RequestQty不可以超過Warehouse的A倉庫存數量
            DataTable ExceedData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)detailgridbs.DataSource, "Seq,Seq1,Seq2,RequestQty", string.Format(@"select * from (
SELECT l.Seq,l.Seq1,l.Seq2,l.RequestQty,isnull(mpd.InQty-mpd.OutQty+mpd.AdjustQty-mpd.LInvQty,0) as StockQty
FROM #tmp l
left join MDivisionPoDetail mpd WITH (NOLOCK) on mpd.POID = '{0}' and mpd.SEQ1 = l.Seq1 and mpd.SEQ2 = l.Seq2) a
where a.RequestQty > a.StockQty", MyUtility.Convert.GetString(CurrentMaintain["POID"])), out ExceedData);
            }
            catch (Exception ex)
            {
                ShowErr("Save error.", ex);
                return false;
            }
            StringBuilder msg = new StringBuilder();
            if (comboType.Text != "Lacking")
            {
                foreach (DataRow dr in ExceedData.Rows)
                {
                    msg.Append(string.Format("Seq#:{0}  < Request Qty >:{1} exceed stock qty:{2}\r\n", MyUtility.Convert.GetString(dr["Seq"]), MyUtility.Convert.GetString(dr["RequestQty"]), MyUtility.Convert.GetString(dr["StockQty"])));
                }
            }
            if (msg.Length != 0)
            {
                MyUtility.Msg.WarningBox(msg.ToString());
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Factory + "LR", "Lack", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("No Data!!");
                return false;
            }

            string sqlCmd = string.Format(@"select (left(ld.Seq1+' ',3)+'-'+ld.Seq2) as Seq, dbo.getMtlDesc(l.POID,ld.Seq1,ld.Seq2,1,0) as Description,
            ld.FTYLastRecvDate,ld.FTYInQty,ld.WhseInQty,ld.RequestQty,ld.IssueQty
            from Lack l WITH (NOLOCK) 
            left join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
            where l.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DataTable ExcelData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ExcelData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_P11.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = MyUtility.Convert.GetString(CurrentMaintain["MDivisionID"]);
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(CurrentMaintain["ID"]);
            worksheet.Cells[5, 2] = Convert.ToDateTime(CurrentMaintain["IssueDate"]).ToString("d");
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(CurrentMaintain["OrderID"]);
            worksheet.Cells[7, 2] = MyUtility.Convert.GetString(CurrentMaintain["Remark"]);

            worksheet.Cells[4, 4] = MyUtility.Convert.GetString(CurrentMaintain["Type"]) == "R" ? "Replacement" : "Lacking";
            worksheet.Cells[5, 4] = MyUtility.Convert.GetString(CurrentMaintain["Shift"]) == "D" ? "Day" : MyUtility.Convert.GetString(CurrentMaintain["Shift"]) == "N" ? "Night" : "Subcon-Out";
            worksheet.Cells[6, 4] = MyUtility.Convert.GetString(CurrentMaintain["SewingLineID"]);

            worksheet.Cells[4, 6] = txtuserHandle.TextBox1.Text + " " + txtuserHandle.DisplayBox1.Text;
            worksheet.Cells[5, 6] = txtuserApprove.TextBox1.Text + " " + txtuserApprove.DisplayBox1.Text;
            worksheet.Cells[6, 6] = MyUtility.Check.Empty(CurrentMaintain["ApvDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["ApvDate"]).ToString("d");

            int intRowsStart = 10;
            int dataRowCount = ExcelData.Rows.Count;
            int rownum = 0;
            object[,] objArray = new object[1, 7];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = ExcelData.Rows[i];
                rownum = intRowsStart + i;
                objArray[0, 0] = dr["Seq"];
                objArray[0, 1] = dr["Description"];
                objArray[0, 2] = dr["FTYLastRecvDate"];
                objArray[0, 3] = dr["FTYInQty"];
                objArray[0, 4] = dr["WhseInQty"];
                objArray[0, 5] = dr["RequestQty"];
                objArray[0, 6] = dr["IssueQty"];

                worksheet.Range[String.Format("A{0}:G{0}", rownum)].Value2 = objArray;
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P11");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion 
            return base.ClickPrint();
        }

        //撈取最後發料日
        private DateTime? MaxIssueDate(string Seq1, string Seq2)
        {
            DateTime? maxIssueDate = null;
            DataTable issueData;
            string sqlCmd = string.Format(@"select max(i.IssueDate) as IssueDate from Issue i WITH (NOLOCK) , Issue_Detail id WITH (NOLOCK) where i.Id = id.Id and id.PoId = '{0}' and id.Seq1 = '{1}' and id.Seq2 = '{2}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]), Seq1, Seq2);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out issueData);
            if (result)
            {
                maxIssueDate = MyUtility.Convert.GetDate(issueData.Rows[0]["IssueDate"]);
            }

            return maxIssueDate;
        }

        //Type
        private void comboType_Validated(object sender, EventArgs e)
        {
            if (EditMode)
            {
                if (comboType.OldValue != comboType.SelectedValue && detailgridbs.DataSource != null)
                {
                    foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
                    {
                        dr["PPICReasonID"] = "";
                        dr["PPICReasonDesc"] = "";
                    }
                }
            }
        }

        //SP#
        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (txtSPNo.OldValue != txtSPNo.Text)
                {
                    if (!MyUtility.Check.Empty(txtSPNo.Text))
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@poid", txtSPNo.Text);
                        //用登入的Factory 抓取對應的FtyGroup
                        DataTable FtyGroupData;
                        DBProxy.Current.Select(null, string.Format("select FTYGroup from Factory where id='{0}' and IsProduceFty = 1", Sci.Env.User.Factory), out FtyGroupData);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@factoryid", FtyGroupData.Rows[0]["FTYGroup"].ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable OrderPOID;
                        string sqlCmd = "select POID,FtyGroup from Orders WITH (NOLOCK) where POID = @poid and FtyGroup  = @factoryid";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrderPOID);
                        if (result && OrderPOID.Rows.Count > 0)
                        {
                            CurrentMaintain["OrderID"] = txtSPNo.Text;
                            CurrentMaintain["POID"] = OrderPOID.Rows[0]["POID"];
                            CurrentMaintain["FactoryID"] = OrderPOID.Rows[0]["FtyGroup"];
                        }
                        else
                        {
                            CurrentMaintain["OrderID"] = "";
                            CurrentMaintain["POID"] = "";
                            CurrentMaintain["FactoryID"] = "";
                            DeleteAllGridData();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("SP# not exist!!");
                            return;
                        }
                    }
                    else
                    {
                        CurrentMaintain["OrderID"] = "";
                        CurrentMaintain["POID"] = "";
                        CurrentMaintain["FactoryID"] = "";
                    }
                    DeleteAllGridData();
                }
            }
        }

        //刪除表身Grid資料
        private void DeleteAllGridData()
        {
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'Confirmed',ApvName = '{0}',ApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }
           
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!MyUtility.Check.Empty(CurrentMaintain["IssueLackId"]))
            {
                MyUtility.Msg.WarningBox("This order was issued, can't unconfirm!");
                return;
            }
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to unconfirm this data?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'New',ApvName = '',ApvDate = null, EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
           
        }

        protected override void ClickReceive()
        {
            base.ClickReceive();
            if (MyUtility.Check.Empty(CurrentMaintain["IssueLackId"]))
            {
                MyUtility.Msg.WarningBox("< Issue No. > can't empty!");
                return;
            }
            DualResult result;
            string updateCmd = string.Format("update Lack set Status = 'Received', EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Receive fail!\r\n" + result.ToString());
                return;
            }
            
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            lbStatus.Text = CurrentMaintain["status"].ToString().Trim();

            if (!MyUtility.Check.Empty(this.CurrentMaintain["IssueLackDT"]))
            {
                this.displayIssueLackDate.Text = Convert.ToDateTime(this.CurrentMaintain["IssueLackDT"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            this.displayIssueLackDate.Text = "";
        }
        #region -- SelePoItem --
        public static string selePoItemSqlCmd = @"select  p.id,
                                                    concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2) as seq,
                                                    p.Refno, 
                                                    dbo.getmtldesc(p.id,p.seq1,p.seq2,2,0) as Description 
                                                    ,p.ColorID,p.SizeSpec,p.FinalETA
                                                    ,isnull(m.InQty, 0) as InQty
                                                    ,iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
                                                        ff.UsageUnit , 
                                                        iif(mm.IsExtensionUnit > 0 , 
                                                            iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
                                                                ff.UsageUnit , 
                                                                uu.ExtensionUnit), 
                                                            ff.UsageUnit)) as StockUnit
                                                    ,isnull(m.OutQty, 0) as outQty
                                                    ,isnull(m.AdjustQty, 0) as AdjustQty
                                                    ,isnull(m.inqty, 0) - isnull(m.OutQty, 0) + isnull(m.AdjustQty, 0) as balance
                                                    ,isnull(m.LInvQty, 0) as LInvQty
                                                    from dbo.PO_Supp_Detail p WITH (NOLOCK) 
                                                    left join dbo.mdivisionpodetail m WITH (NOLOCK) on m.poid = p.id and m.seq1 = p.seq1 and m.seq2 = p.seq2 
                                                    inner join dbo.Factory F WITH (NOLOCK) on F.id=p.factoryid and F.MDivisionID='{1}'
                                                    inner join [dbo].[Fabric] ff WITH (NOLOCK) on p.SCIRefno= ff.SCIRefno
                                                    inner join [dbo].[MtlType] mm WITH (NOLOCK) on mm.ID = ff.MtlTypeID
                                                    inner join [dbo].[Unit] uu WITH (NOLOCK) on ff.UsageUnit = uu.ID
                                                    inner join View_unitrate v on v.FROM_U = p.POUnit 
	                                                    and v.TO_U = (
	                                                    iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
		                                                    ff.UsageUnit , 
		                                                    iif(mm.IsExtensionUnit > 0 , 
			                                                    iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
				                                                    ff.UsageUnit , 
				                                                    uu.ExtensionUnit), 
			                                                    ff.UsageUnit)))--p.StockUnit
                                                    where p.id ='{0}'";
        /// <summary>
        /// 右鍵開窗選取採購項
        /// </summary>
        /// <param name="poid"></param>
        /// <param name="defaultseq"></param>
        /// <param name="filters"></param>
        /// <returns>Sci.Win.Tools.SelectItem</returns>//
        public static Sci.Win.Tools.SelectItem2 SelePoItem1(string poid, string defaultseq, string filters = null)
        {
            DataTable dt;
            string sqlcmd = string.Format(selePoItemSqlCmd, poid, Sci.Env.User.Keyword);

            if (!(MyUtility.Check.Empty(filters)))
            {
                sqlcmd += string.Format(" And {0}", filters);
            }

            DBProxy.Current.Select(null, sqlcmd, out dt);

            Sci.Win.Tools.SelectItem2 selepoitem = new Win.Tools.SelectItem2(dt, "Seq,refno,description,colorid,SizeSpec,FinalETA,inqty,stockunit,outqty,adjustqty,balance,linvqty"
                            , "Seq,Ref#,Description,Color,Size,ETA,In Qty,Stock Unit,Out Qty,Adqty,Balance,Inventory Qty"
                            , "6,8,35,8,10,6,6,6,6,6,6", defaultseq);
            selepoitem.Width = 1024;

            return selepoitem;
        }
        #endregion

        private void P11_FormLoaded(object sender, EventArgs e)
        {
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.PPIC.P10_P11_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource, "Accessory");
            frm.ShowDialog(this);
            this.RenewData();
        }

    }
}
