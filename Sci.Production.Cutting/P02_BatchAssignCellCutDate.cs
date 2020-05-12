using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Sci.Production.Class;

namespace Sci.Production.Cutting
{
    public partial class P02_BatchAssignCellCutDate : Sci.Win.Subs.Base
    {
        private DataTable curTb;
        private DataTable detailTb;
        private DataTable sp;
        private string Poid;
        private string KeyWord = Sci.Env.User.Keyword;

        public P02_BatchAssignCellCutDate(DataTable cursor)
        {
            InitializeComponent();
            txtCutCell.MDivisionID = this.KeyWord;
            txtCell2.MDivisionID = this.KeyWord;
            detailTb = cursor;
            curTb = cursor.Copy();
            curTb.Columns.Add("Sel", typeof(bool));
            this.txtSpreadingNo.MDivision = this.KeyWord;
            gridsetup();
            btnFilter_Click(null, null);  //1390: CUTTING_P02_BatchAssignCellCutDate，當進去此功能時應直接預帶資料。

            MyUtility.Tool.ProcessWithDatatable(curTb, "orderid", "select distinct orderid from #tmp", out sp);
            if (cursor != null)
            {
                DataTable dtcopy = cursor.Copy();
                dtcopy.AcceptChanges();
                Poid = MyUtility.GetValue.Lookup($@"Select poid from orders WITH (NOLOCK) where id ='{dtcopy.Rows[0]["ID"]}'");
            }
        }

        private void gridsetup()
        {
            DataGridViewGeneratorTextColumnSettings Cell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings SpreadingNo = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Seq1 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Seq2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings EstCutDate = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Shift = cellTextDropDownList.GetGridCell("Pms_WorkOrderShift");
            DataGridViewGeneratorDateColumnSettings WKETA = new DataGridViewGeneratorDateColumnSettings();
            Ict.Win.UI.DataGridViewDateBoxColumn col_wketa = new Ict.Win.UI.DataGridViewDateBoxColumn();

            #region Cell
            bool cellchk = true;
            Cell.EditingMouseDown += (s, e) =>
            {
                DualResult DR; DataTable DT; SelectItem S;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    string CUTCELL = string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", this.KeyWord);
                    DR = DBProxy.Current.Select(null, CUTCELL, out DT);
                    S = new SelectItem(DT, "ID", "10", DT.Columns["id"].ToString(), false, ",");
                    DialogResult result = S.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = S.GetSelectedString();

                    DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);

                    showMsgCheckCuttingWidth(e.EditingControl.Text, dr["SciRefno"].ToString());

                    cellchk = false;
                }
            };
            Cell.CellValidating += (s, e) =>
            {
                DualResult DR; DataTable DT;

                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty()) return;

                string oldvalue = dr["Cutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                string CUTCELL = string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", this.KeyWord);
                DR = DBProxy.Current.Select(null, CUTCELL, out DT);

                DataRow[] seledr = DT.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["Cutcellid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    return;
                }
                dr["Cutcellid"] = newvalue;

                if (cellchk)
                {
                    showMsgCheckCuttingWidth(dr["CutcellID"].ToString(), dr["SciRefno"].ToString());
                }
                else
                {
                    cellchk = true;
                }
                dr.EndEdit();
            };
            #endregion
            #region Cell
            bool col_SpreadingNoIDchk = true;
            SpreadingNo.EditingMouseDown += (s, e) =>
            {
                DualResult Result;
                DataTable DT;
                SelectItem S;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    string sqlSpreadingNo = $"Select id,CutCell=CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and junk=0";
                    Result = DBProxy.Current.Select(null, sqlSpreadingNo, out DT);
                    if (!Result)
                    {
                        this.ShowErr(Result);
                        return;
                    }
                    S = new SelectItem(DT, "ID,CutCell", string.Empty, DT.Columns["id"].ToString(), false, ",");
                    DialogResult result = S.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                    dr["SpreadingNoID"] = S.GetSelectedString();
                    if (!MyUtility.Check.Empty(S.GetSelecteds()[0]["CutCell"]))
                    {
                        dr["Cutcellid"] = S.GetSelecteds()[0]["CutCell"];
                        showMsgCheckCuttingWidth(MyUtility.Convert.GetString(S.GetSelecteds()[0]["CutCell"]), dr["SciRefno"].ToString());
                    }
                    dr.EndEdit();
                    col_SpreadingNoIDchk = false;
                }
            };
            SpreadingNo.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (e.RowIndex == -1) return;
                DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty()) return;
                string oldvalue = dr["SpreadingNoID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                DataRow SpreadingNodr;
                string sqlSpreading = $"Select CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and  id = '{newvalue}' and junk=0";
                if (!MyUtility.Check.Seek(sqlSpreading, out SpreadingNodr))
                {
                    dr["SpreadingNoID"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SpreadingNo> : {0} data not found!", newvalue));
                    return;
                }

                dr["SpreadingNoID"] = newvalue;

                if (!MyUtility.Check.Empty(SpreadingNodr["CutCellID"]))
                    dr["cutCellid"] = SpreadingNodr["CutCellID"];
                if (!col_SpreadingNoIDchk)
                {
                    col_SpreadingNoIDchk = true;
                }
                else
                {
                    checkCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                }
                dr.EndEdit();
            };
            #endregion

            EstCutDate.CellValidating += (s, e) =>
            {
                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (e.FormattedValue.ToString() == dr["estcutdate"].ToString()) { return; }
                    if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(e.FormattedValue)) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                    }
                }
            };

            #region Seq1
            col_Seq1.EditingMouseDown += (s, e) =>
            {
                P02_PublicFunction.Seq1EditingMouseDown(s, e, this, this.gridBatchAssignCellEstCutDate, this.Poid);
            };
            col_Seq1.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
            };
            col_Seq1.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_Seq1.CellValidating += (s, e) =>
            {
                P02_PublicFunction.Seq1CellValidating(s, e, this, this.gridBatchAssignCellEstCutDate, this.Poid);
            };
            #endregion

            #region Seq2
            col_Seq2.EditingMouseDown += (s, e) =>
            {
                P02_PublicFunction.Seq2EditingMouseDown(s, e, this, this.gridBatchAssignCellEstCutDate, this.Poid);
            };
            col_Seq2.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_Seq2.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_Seq2.CellValidating += (s, e) =>
            {
                P02_PublicFunction.Seq2CellValidating(s, e, this, this.gridBatchAssignCellEstCutDate, this.Poid);
            };
            #endregion

            #region WKETA
            WKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    P02_WKETA item = new P02_WKETA(dr);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    if (result == DialogResult.No) { dr["WKETA"] = DBNull.Value; }
                    if (result == DialogResult.Yes) { dr["WKETA"] = itemx.WKETA; }
                    dr.EndEdit();
                }
            };
            
            #endregion

            this.gridBatchAssignCellEstCutDate.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(this.gridBatchAssignCellEstCutDate)
             .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
             .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
             .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5), iseditingreadonly: true)
             .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(3), settings: SpreadingNo, iseditingreadonly: false)
             .Text("Cutcellid", header: "Cell", width: Widths.AnsiChars(2), settings: Cell, iseditingreadonly: false)
             .Text("Shift", header: "Shift", width: Widths.AnsiChars(2), iseditingreadonly: false, settings: col_Shift)
             .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
             .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
             .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), settings: col_Seq1)
             .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), settings: col_Seq2)
             .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Date("WKETA", header: "WK ETA", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: WKETA).Get(out col_wketa)
             .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: EstCutDate)
             .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.gridBatchAssignCellEstCutDate.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["SpreadingNoID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["SpreadingNoID"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridBatchAssignCellEstCutDate.Columns["Cutcellid"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["Cutcellid"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridBatchAssignCellEstCutDate.Columns["estcutdate"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["estcutdate"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridBatchAssignCellEstCutDate.Columns["Shift"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["Shift"].DefaultCellStyle.ForeColor = Color.Red;
            //col_wketa.Width = 97;

            col_wketa.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                { ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true; ((Ict.Win.UI.DateBox)e.Control).Enabled = true; }
                else { ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true; ((Ict.Win.UI.DateBox)e.Control).Enabled = false; }

            };
            col_wketa.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            filter();
        }

        private void filter()
        {
            string sp = txtSPNo.Text;
            string article = txtArticle.Text;
            string markername = txtMarkerName.Text;
            string sizecode = txtSizeCode.Text;
            string cutcell = txtCutCell.Text;
            string fabriccombo = txtFabricCombo.Text;
            string estcutdate = txtEstCutDate.Text.ToString();
            string filter = "(cutref is null or cutref = '') and (cutplanid is null or cutplanid = '') ";
            if (!MyUtility.Check.Empty(sp)) filter = filter + string.Format(" and OrderID ='{0}'", sp);
            if (!MyUtility.Check.Empty(article)) filter = filter + string.Format(" and article like '%{0}%'", article);
            if (!MyUtility.Check.Empty(markername)) filter = filter + string.Format(" and markername ='{0}'", markername);
            if (!MyUtility.Check.Empty(sizecode)) filter = filter + string.Format(" and sizecode like '%{0}%'", sizecode);
            if (!MyUtility.Check.Empty(cutcell)) filter = filter + string.Format(" and cutcellid ='{0}'", cutcell);
            if (!MyUtility.Check.Empty(fabriccombo)) filter = filter + string.Format(" and fabriccombo ='{0}'", fabriccombo);
            if (!MyUtility.Check.Empty(numCutNo.Value)) filter = filter + string.Format(" and cutno ={0}", numCutNo.Value);
            if (!MyUtility.Check.Empty(txtEstCutDate.Value)) filter = filter + string.Format(" and estcutdate ='{0}'", estcutdate);
            if (checkOnlyShowEmptyEstCutDate.Value == "True") filter = filter + " and estcutdate is null ";
            string orderby = "SORT_NUM ASC,FabricCombo ASC,multisize DESC,Colorid ASC,Order_SizeCode_Seq DESC,MarkerName ASC,Ukey";
            curTb.DefaultView.RowFilter = filter;
            curTb.DefaultView.Sort = orderby;
            gridBatchAssignCellEstCutDate.DataSource = curTb;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBatchUpdateEstCutCell_Click(object sender, EventArgs e)
        {
            List<string> warningMsg = new List<string>();
            string cell = txtCell2.Text;

            // 不可輸入空白
            if (cell.Empty())
            {
                MyUtility.Msg.WarningBox("Cell can't be empty.");
                return;
            }

            foreach (DataRow dr in curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;

                if (dr["Sel"].ToString() == "True")
                {
                    dr["Cutcellid"] = cell;
                    dr.EndEdit();
                    string strMsg = checkCuttingWidth(dr["Cutcellid"].ToString(), dr["SciRefno"].ToString());
                    if (!strMsg.Empty())
                    {
                        warningMsg.Add(strMsg);
                    }
                }
            }
            if (warningMsg.Count > 0)
            {
                MyUtility.Msg.WarningBox(warningMsg.Select(x => x).Distinct().ToList().JoinToString("\n"));
            }
        }

        private void btnBatchUpdateEstCutDate_Click(object sender, EventArgs e)
        {
            string cdate = "";
            if (!MyUtility.Check.Empty(txtBatchUpdateEstCutDate.Value))
            {
                cdate = txtBatchUpdateEstCutDate.Text;
            };
            foreach (DataRow dr in curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;

                if (dr["Sel"].ToString() == "True")
                {
                    if (cdate != "")
                    {
                        dr["estcutdate"] = cdate;
                    }
                    else
                    {
                        dr["estcutdate"] = DBNull.Value;
                    }
                }
            }
        }

        private void btnBatchUpdSeq_Click(object sender, EventArgs e)
        {
            string Seq1 = txtSeq1.Text;
            string Seq2 = txtSeq2.Text;
            DataRow drCheckColor;
            bool isColorMatch = true;
            List<DataRow> listColorchangedDr = new List<DataRow>();
            List<DataRow> listOriDr = new List<DataRow>();

            // 不可輸入空白
            if (MyUtility.Check.Empty(Seq1) || MyUtility.Check.Empty(Seq2))
            {
                MyUtility.Msg.WarningBox("Seq1 and Seq2 can't be empty.");
                return;
            }

            foreach (DataRow dr in curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;

                if (dr["Sel"].ToString() == "True")
                {
                    string chk = $@"
select Colorid from (
	select ID,SEQ1,SEQ2,ColorID
	from PO_Supp_Detail 
	where ID = '{Poid}'
    and Refno = '{dr["Refno"]}'
	and Junk != 1
)a
where Seq1='{Seq1}' and Seq2='{Seq2}' 
";
                    if (MyUtility.Check.Seek(chk, out drCheckColor))
                    {
                        if (!drCheckColor["Colorid"].Equals(dr["Colorid"]))
                        {
                            DataRow OldDr = dr.Table.NewRow();
                            dr.CopyTo(OldDr, "Seq1,Seq2,Colorid");
                            listOriDr.Add(OldDr);
                            isColorMatch = false;
                            listColorchangedDr.Add(dr);
                        }
                        dr["Seq1"] = Seq1;
                        dr["Seq2"] = Seq2;
                        dr["Colorid"] = drCheckColor["Colorid"];
                    }
                    dr.EndEdit();
                }
            }

            if (!isColorMatch)
            {
                DialogResult DiaR = MyUtility.Msg.QuestionBox($@"Orignal assign colorID isn't same as locate colorID.
Do you want to continue? ");
                if (DiaR == DialogResult.No)
                {
                    for (int i = 0; i < listColorchangedDr.Count; i++)
                    {
                        listColorchangedDr[i]["Seq1"] = listOriDr[i]["Seq1"];
                        listColorchangedDr[i]["Seq2"] = listOriDr[i]["Seq2"];
                        listColorchangedDr[i]["Colorid"] = listOriDr[i]["Colorid"];
                    }
                    return;
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.gridBatchAssignCellEstCutDate.ValidateControl();
            string SpreadingNo = txtSpreadingNo.Text;
            string cell = txtCell2.Text;
            string cdate = "";
            string Seq1 = txtSeq1.Text;
            string Seq2 = txtSeq2.Text;
            string Shift = txtShift.Text;
            if (!MyUtility.Check.Empty(txtBatchUpdateEstCutDate.Value))
            {
                cdate = txtBatchUpdateEstCutDate.Text;
            };
            foreach (DataRow dr in curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;
                if (dr["Sel"].ToString() == "True")
                {
                    DataRow[] detaildr;
                    if (MyUtility.Check.Empty(dr["Ukey"]))
                    {
                        detaildr = detailTb.Select(string.Format("newkey = '{0}'", dr["newkey"]));
                    }
                    else
                    {
                        detaildr = detailTb.Select(string.Format("Ukey = '{0}'", dr["Ukey"]));
                    }                    

                    detaildr[0]["SpreadingNoID"] = dr["SpreadingNoID"];
                    detaildr[0]["Cutcellid"] = dr["Cutcellid"];
                    detaildr[0]["estcutdate"] = dr["estcutdate"];
                    detaildr[0]["WKETA"] = dr["WKETA"];
                    detaildr[0]["Seq1"] = dr["Seq1"];
                    detaildr[0]["Seq2"] = dr["Seq2"];
                    detaildr[0]["shift"] = dr["shift"];

                }
            }
            Close();
        }

        private void txtBatchUpdateEstCutDate_Validating(object sender, CancelEventArgs e)
        {
            if (!(MyUtility.Check.Empty(txtBatchUpdateEstCutDate.Value)))
            {
                if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(txtBatchUpdateEstCutDate.Value)) > 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                }
            }
        }

        private void txtSPNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem sele;
            sele = new SelectItem(sp, "OrderID", "15@300,400", sp.Columns["OrderID"].ToString(), columndecimals: "50");
            DialogResult result = sele.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtSPNo.Text = sele.GetSelectedString();
            filter();
        }

        /// <summary>
        /// 確認【布】寬是否會超過【裁桌】的寬度
        /// 若是 CutCell 取得寬度為 0 ，則不顯示訊息
        /// return Msg
        /// </summary>
        /// <param name="strCutCellID">CutCellID</param>
        /// <param name="strSCIRefno">SciRefno</param>
        private string checkCuttingWidth(string strCutCellID, string strSCIRefno)
        {
            string chkwidth = MyUtility.GetValue.Lookup(string.Format(@"
select width_cm = width*2.54 
from Fabric 
where SCIRefno = '{0}'", strSCIRefno));
            string strCuttingWidth = MyUtility.GetValue.Lookup(string.Format(@"
select cuttingWidth = isnull (cuttingWidth, 0) 
from CutCell 
where   id = '{0}'
        and MDivisionID = '{1}'", strCutCellID, this.KeyWord));
            if (!chkwidth.Empty() && !strCuttingWidth.Empty())
            {
                decimal width_CM = decimal.Parse(chkwidth);
                decimal decCuttingWidth = decimal.Parse(strCuttingWidth);
                if (decCuttingWidth == 0)
                {
                    return "";
                }

                if (width_CM > decCuttingWidth)
                {
                    return string.Format("fab width greater than cutting cell {0}, please check it.", strCutCellID);
                }
            }
            return "";
        }

        /// <summary>
        /// 確認【布】寬是否會超過【裁桌】的寬度
        /// 若是 CutCell 取得寬度為 0 ，則不顯示訊息
        /// Show Msg
        /// </summary>
        /// <param name="strCutCellID">CutCellID</param>
        /// <param name="strSCIRefno">SciRefno</param>
        private void showMsgCheckCuttingWidth(string strCutCellID, string strSCIRefno)
        {
            string Msg = checkCuttingWidth(strCutCellID, strSCIRefno);
            if (!Msg.Empty())
            {
                MyUtility.Msg.WarningBox(Msg);
            }
        }

        private void txtSeq1_Validating(object sender, CancelEventArgs e)
        {
            string Seq1 = txtSeq1.Text;
            if (!MyUtility.Check.Empty(Seq1))
            {
                if (!MyUtility.Check.Seek($@"select 1 from po_Supp_Detail WITH (NOLOCK) where id='{Poid}' and Seq1='{Seq1}'  and Junk=0"))
                {
                    MyUtility.Msg.WarningBox($@"Seq1: {Seq1} data not found!");
                    txtSeq1.Text = string.Empty;
                    txtSeq1.Focus();
                    return;
                }
            }
            else
            {
                txtSeq1.Text = string.Empty;
            }
        }

        private void txtSeq2_Validating(object sender, CancelEventArgs e)
        {
            string Seq2 = txtSeq2.Text;
            if (!MyUtility.Check.Empty(Seq2))
            {
                if (!MyUtility.Check.Seek($@"select 1 from po_Supp_Detail WITH (NOLOCK) where id='{Poid}' and Seq2='{Seq2}' and Junk=0"))
                {
                    MyUtility.Msg.WarningBox($@"Seq2: {Seq2} data not found!");
                    txtSeq2.Text = string.Empty;
                    txtSeq2.Focus();
                    return;
                }
            }
        }

        private void txtSeq1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem item = new SelectItem($@"Select SEQ1,SEQ2,Colorid From PO_Supp_Detail WITH (NOLOCK) Where id='{Poid}' and junk=0", "SEQ1,SEQ2,Colorid", txtSeq1.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtSeq1.Text = item.GetSelectedString();
            txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();
        }

        private void txtSeq2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem item = new SelectItem($@"Select SEQ1,SEQ2,Colorid From PO_Supp_Detail WITH (NOLOCK) Where id='{Poid}' and junk=0", "SEQ1,SEQ2,Colorid", txtSeq2.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtSeq1.Text = item.GetSelecteds()[0]["Seq1"].ToString();
            txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();
        }

        private void btnBatchUpdateSpreadingNo_Click(object sender, EventArgs e)
        {
            this.gridBatchAssignCellEstCutDate.ValidateControl();
            List<string> warningMsg = new List<string>();
            // 不可輸入空白
            if (MyUtility.Check.Empty(txtSpreadingNo.Text))
            {
                MyUtility.Msg.WarningBox("SpreadingNo can't be empty.");
                return;
            }
            string SpreadingNoID = txtSpreadingNo.Text;
            string cell = MyUtility.GetValue.Lookup($"Select CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and  id = '{SpreadingNoID}' and junk=0");
            foreach (DataRow dr in curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;

                if (dr["Sel"].ToString() == "True")
                {
                    dr["SpreadingNoID"] = SpreadingNoID;
                }
            }

            if (!MyUtility.Check.Empty(cell))
            {
                foreach (DataRow dr in curTb.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                        continue;

                    if (dr["Sel"].ToString() == "True")
                    {
                        dr["Cutcellid"] = cell;
                        dr.EndEdit();
                        string strMsg = checkCuttingWidth(dr["Cutcellid"].ToString(), dr["SciRefno"].ToString());
                        if (!strMsg.Empty())
                        {
                            warningMsg.Add(strMsg);
                        }
                    }
                }
                if (warningMsg.Count > 0)
                {
                    MyUtility.Msg.WarningBox(warningMsg.Select(x => x).Distinct().ToList().JoinToString("\n"));
                }
            }
        }

        private void BtnBatchUpdateShift_Click(object sender, EventArgs e)
        {
            string strShift = "";
            if (!MyUtility.Check.Empty(txtShift.Text))
            {
                strShift = txtShift.Text;
            };
            foreach (DataRow dr in curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;

                if (dr["Sel"].ToString() == "True")
                {
                    if (strShift != "")
                    {
                        dr["Shift"] = strShift;
                    }
                    else
                    {
                        dr["Shift"] = DBNull.Value;
                    }
                    dr.EndEdit();
                }
            }
        }

        private void BtnBatchWKETA_Click(object sender, EventArgs e)
        {
            this.gridBatchAssignCellEstCutDate.ValidateControl();
            string cdate = "";
            if (!MyUtility.Check.Empty(dateBoxWKETA.Value))
            {
                cdate = dateBoxWKETA.Text;
            };

            DataRow[] drSelect = curTb.Select("Sel = 1");
            foreach (DataRow dr in drSelect)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;
                string sqlcmdChk = $@"
select 1
From Export_Detail ED 
INNER JOIN Export E ON E.ID = ED.ID
where ED.poid='{dr["id"]}' 
and ED.seq1 ='{dr["Seq1"]}' and ED.seq2='{dr["seq2"]}'
and e.ETA = '{cdate}'
";
                if (MyUtility.Check.Seek(sqlcmdChk))
                {
                    dr["WKETA"] = cdate;
                }
                else
                {
                    dr["WKETA"] = DBNull.Value;
                }
            }
        }
    }
}
