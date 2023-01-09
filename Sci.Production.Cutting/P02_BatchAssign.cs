using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Sci.Production.Class;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_BatchAssign : Win.Subs.Base
    {
        private DataTable curTb;
        private DataTable detailTb;
        private DataTable sp;
        private string Poid;
        private string KeyWord = Env.User.Keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="P02_BatchAssign"/> class.
        /// </summary>
        /// <param name="cursor">Detail Table</param>
        /// <param name="id">ID</param>
        public P02_BatchAssign(DataTable cursor, string id)
        {
            this.InitializeComponent();
            this.txtCutCell.MDivisionID = this.KeyWord;
            this.txtCell2.MDivisionID = this.KeyWord;
            this.detailTb = cursor;
            this.curTb = cursor.Copy();
            this.curTb.Columns.Add("Sel", typeof(bool));
            this.txtSpreadingNo.MDivision = this.KeyWord;
            this.Gridsetup();
            this.BtnFilter_Click(null, null);  // 1390: CUTTING_P02_BatchAssignCellCutDate，當進去此功能時應直接預帶資料。

            MyUtility.Tool.ProcessWithDatatable(this.curTb, "orderid", "select distinct orderid from #tmp", out this.sp);
            if (cursor != null)
            {
                DataTable dtcopy = cursor.Copy();
                dtcopy.AcceptChanges();
                this.Poid = MyUtility.GetValue.Lookup($@"Select poid from orders WITH (NOLOCK) where id ='{dtcopy.Rows[0]["ID"]}'");
            }
        }

        private void Gridsetup()
        {
            DataGridViewGeneratorTextColumnSettings cell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings spreadingNo = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Seq1 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Seq2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings estCutDate = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Shift = CellTextDropDownList.GetGridCell("Pms_WorkOrderShift");
            DataGridViewGeneratorDateColumnSettings wKETA = new DataGridViewGeneratorDateColumnSettings();
            Ict.Win.UI.DataGridViewDateBoxColumn col_wketa = new Ict.Win.UI.DataGridViewDateBoxColumn();

            #region Cell
            bool cellchk = true;
            cell.EditingMouseDown += (s, e) =>
            {
                DualResult dR;
                SelectItem s1;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    string cUTCELL = string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", this.KeyWord);
                    dR = DBProxy.Current.Select(null, cUTCELL, out DataTable dt);
                    s1 = new SelectItem(dt, "ID", "10", dt.Columns["id"].ToString(), false, ",");
                    DialogResult result = s1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = s1.GetSelectedString();

                    DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);

                    this.ShowMsgCheckCuttingWidth(e.EditingControl.Text, dr["SciRefno"].ToString());

                    cellchk = false;
                }
            };
            cell.CellValidating += (s, e) =>
            {
                DualResult result;

                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty())
                {
                    return;
                }

                string oldvalue = dr["Cutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                string cUTCELL1 = string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", this.KeyWord);
                result = DBProxy.Current.Select(null, cUTCELL1, out DataTable dt);

                DataRow[] seledr = dt.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["Cutcellid"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    return;
                }

                dr["Cutcellid"] = newvalue;

                if (cellchk)
                {
                    this.ShowMsgCheckCuttingWidth(dr["CutcellID"].ToString(), dr["SciRefno"].ToString());
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
            spreadingNo.EditingMouseDown += (s, e) =>
            {
                DualResult result1;
                SelectItem s2;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    string sqlSpreadingNo = $"Select id,CutCell=CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and junk=0";
                    result1 = DBProxy.Current.Select(null, sqlSpreadingNo, out DataTable dt);
                    if (!result1)
                    {
                        this.ShowErr(result1);
                        return;
                    }

                    s2 = new SelectItem(dt, "ID,CutCell", string.Empty, dt.Columns["id"].ToString(), false, ",");
                    DialogResult result = s2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                    dr["SpreadingNoID"] = s2.GetSelectedString();
                    if (!MyUtility.Check.Empty(s2.GetSelecteds()[0]["CutCell"]))
                    {
                        dr["Cutcellid"] = s2.GetSelecteds()[0]["CutCell"];
                        this.ShowMsgCheckCuttingWidth(MyUtility.Convert.GetString(s2.GetSelecteds()[0]["CutCell"]), dr["SciRefno"].ToString());
                    }

                    dr.EndEdit();
                    col_SpreadingNoIDchk = false;
                }
            };
            spreadingNo.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty())
                {
                    return;
                }

                string oldvalue = dr["SpreadingNoID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                string sqlSpreading = $"Select CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and  id = '{newvalue}' and junk=0";
                if (!MyUtility.Check.Seek(sqlSpreading, out DataRow spreadingNodr))
                {
                    dr["SpreadingNoID"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SpreadingNo> : {0} data not found!", newvalue));
                    return;
                }

                dr["SpreadingNoID"] = newvalue;

                if (!MyUtility.Check.Empty(spreadingNodr["CutCellID"]))
                {
                    dr["cutCellid"] = spreadingNodr["CutCellID"];
                }

                if (!col_SpreadingNoIDchk)
                {
                    col_SpreadingNoIDchk = true;
                }
                else
                {
                    this.CheckCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                }

                dr.EndEdit();
            };
            #endregion

            estCutDate.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (e.FormattedValue.ToString() == dr["estcutdate"].ToString())
                    {
                        return;
                    }

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
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            col_Seq1.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
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
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            col_Seq2.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
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
            wKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    P02_WKETA item = new P02_WKETA(dr);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (result == DialogResult.No)
                    {
                        dr["WKETA"] = DBNull.Value;
                    }

                    if (result == DialogResult.Yes)
                    {
                        dr["WKETA"] = Itemx.WKETA;
                    }

                    dr.EndEdit();
                }
            };

            #endregion

            this.gridBatchAssignCellEstCutDate.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridBatchAssignCellEstCutDate)
             .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
             .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
             .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5), iseditingreadonly: true)
             .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(3), settings: spreadingNo, iseditingreadonly: false)
             .Text("Cutcellid", header: "Cell", width: Widths.AnsiChars(2), settings: cell, iseditingreadonly: false)
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
             .Date("WKETA", header: "WK ETA", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: wKETA).Get(out col_wketa)
             .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: estCutDate)
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

            // col_wketa.Width = 97;
            col_wketa.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true;
                    ((Ict.Win.UI.DateBox)e.Control).Enabled = true;
                }
                else
                {
                    ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true;
                    ((Ict.Win.UI.DateBox)e.Control).Enabled = false;
                }
            };
            col_wketa.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
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

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            this.Filter();
        }

        private void Filter()
        {
            string sp = this.txtSPNo.Text;
            string article = this.txtArticle.Text;
            string markername = this.txtMarkerName.Text;
            string sizecode = this.txtSizeCode.Text;
            string cutcell = this.txtCutCell.Text;
            string fabriccombo = this.txtFabricCombo.Text;
            string estcutdate = this.txtEstCutDate.Text.ToString();
            string filter = "(cutref is null or cutref = '') and (cutplanid is null or cutplanid = '') ";
            if (!MyUtility.Check.Empty(sp))
            {
                filter = filter + string.Format(" and OrderID ='{0}'", sp);
            }

            if (!MyUtility.Check.Empty(article))
            {
                filter = filter + string.Format(" and article like '%{0}%'", article);
            }

            if (!MyUtility.Check.Empty(markername))
            {
                filter = filter + string.Format(" and markername ='{0}'", markername);
            }

            if (!MyUtility.Check.Empty(sizecode))
            {
                filter = filter + string.Format(" and sizecode like '%{0}%'", sizecode);
            }

            if (!MyUtility.Check.Empty(cutcell))
            {
                filter = filter + string.Format(" and cutcellid ='{0}'", cutcell);
            }

            if (!MyUtility.Check.Empty(fabriccombo))
            {
                filter = filter + string.Format(" and fabriccombo ='{0}'", fabriccombo);
            }

            if (!MyUtility.Check.Empty(this.numCutNo.Value))
            {
                filter = filter + string.Format(" and cutno ={0}", this.numCutNo.Value);
            }

            if (!MyUtility.Check.Empty(this.txtEstCutDate.Value))
            {
                filter = filter + string.Format(" and estcutdate ='{0}'", estcutdate);
            }

            if (this.checkOnlyShowEmptyEstCutDate.Checked)
            {
                filter = filter + " and estcutdate is null ";
            }

            string orderby = "SORT_NUM ASC,FabricCombo ASC,multisize DESC,Colorid ASC,Order_SizeCode_Seq DESC,MarkerName ASC,Ukey";
            this.curTb.DefaultView.RowFilter = filter;
            this.curTb.DefaultView.Sort = orderby;
            this.gridBatchAssignCellEstCutDate.DataSource = this.curTb;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBatchUpdateEstCutDate_Click(object sender, EventArgs e)
        {
            this.gridBatchAssignCellEstCutDate.ValidateControl();
            string cdate = string.Empty;
            if (!MyUtility.Check.Empty(this.txtBatchUpdateEstCutDate.Value))
            {
                cdate = this.txtBatchUpdateEstCutDate.Text;
            }

            foreach (DataRow dr in this.curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (dr["Sel"].ToString() == "True")
                {
                    if (cdate != string.Empty)
                    {
                        dr["estcutdate"] = cdate;
                    }
                    else
                    {
                        dr["estcutdate"] = DBNull.Value;
                    }
                }
            }

            string strShift = string.Empty;
            if (!MyUtility.Check.Empty(this.txtShift.Text))
            {
                strShift = this.txtShift.Text;
            }

            foreach (DataRow dr in this.curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (dr["Sel"].ToString() == "True")
                {
                    if (strShift != string.Empty)
                    {
                        dr["Shift"] = strShift;
                    }
                    else
                    {
                        dr["Shift"] = string.Empty;
                    }

                    dr.EndEdit();
                }
            }

            string wkETA = string.Empty;
            if (!MyUtility.Check.Empty(this.dateBoxWKETA.Value))
            {
                wkETA = this.dateBoxWKETA.Text;
            }

            DataRow[] drSelect = this.curTb.Select("Sel = 1");
            foreach (DataRow dr in drSelect)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                string sqlcmdChk = $@"
select 1
From Export_Detail ED 
INNER JOIN Export E ON E.ID = ED.ID
where ED.poid='{dr["id"]}' 
and ED.seq1 ='{dr["Seq1"]}' and ED.seq2='{dr["seq2"]}'
and e.ETA = '{wkETA}'
";
                if (MyUtility.Check.Seek(sqlcmdChk))
                {
                    dr["WKETA"] = wkETA;
                }
                else
                {
                    dr["WKETA"] = DBNull.Value;
                }
            }

            this.gridBatchAssignCellEstCutDate.ValidateControl();
            List<string> warningMsg = new List<string>();

            // 不可輸入空白
            if (!MyUtility.Check.Empty(this.txtSpreadingNo.Text))
            {
                string spreadingNoID = this.txtSpreadingNo.Text;
                string cellid = MyUtility.GetValue.Lookup($"Select CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and  id = '{spreadingNoID}' and junk=0");
                foreach (DataRow dr in this.curTb.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dr["Sel"].ToString() == "True")
                    {
                        dr["SpreadingNoID"] = spreadingNoID;
                    }
                }

                if (!MyUtility.Check.Empty(cellid))
                {
                    foreach (DataRow dr in this.curTb.Rows)
                    {
                        if (dr.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        if (dr["Sel"].ToString() == "True")
                        {
                            dr["Cutcellid"] = cellid;
                            dr.EndEdit();
                            string strMsg = this.CheckCuttingWidth(dr["Cutcellid"].ToString(), dr["SciRefno"].ToString());
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

            List<string> warningMsgcell = new List<string>();
            string cell = this.txtCell2.Text;

            // 不可輸入空白
            if (!cell.Empty())
            {
                foreach (DataRow dr in this.curTb.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dr["Sel"].ToString() == "True")
                    {
                        dr["Cutcellid"] = cell;
                        dr.EndEdit();
                        string strMsg = this.CheckCuttingWidth(dr["Cutcellid"].ToString(), dr["SciRefno"].ToString());
                        if (!strMsg.Empty())
                        {
                            warningMsgcell.Add(strMsg);
                        }
                    }
                }

                if (warningMsgcell.Count > 0)
                {
                    MyUtility.Msg.WarningBox(warningMsgcell.Select(x => x).Distinct().ToList().JoinToString("\n"));
                }
            }

            string seq1 = this.txtSeq1.Text;
            string seq2 = this.txtSeq2.Text;
            bool isColorMatch = true;
            List<DataRow> listColorchangedDr = new List<DataRow>();
            List<DataRow> listOriDr = new List<DataRow>();

            // 不可輸入空白
            if (!MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2))
            {
                foreach (DataRow dr in this.curTb.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dr["Sel"].ToString() == "True")
                    {
                        string chk = $@"
select Colorid from (
	select ID,SEQ1,SEQ2,ColorID
	from PO_Supp_Detail 
	where ID = '{this.Poid}'
    and Refno = '{dr["Refno"]}'
	and Junk != 1
)a
where Seq1='{seq1}' and Seq2='{seq2}' 
";
                        if (MyUtility.Check.Seek(chk, out DataRow drCheckColor))
                        {
                            if (!drCheckColor["Colorid"].Equals(dr["Colorid"]))
                            {
                                DataRow oldDr = dr.Table.NewRow();
                                dr.CopyTo(oldDr, "Seq1,Seq2,Colorid");
                                listOriDr.Add(oldDr);
                                isColorMatch = false;
                                listColorchangedDr.Add(dr);
                            }

                            dr["Seq1"] = seq1;
                            dr["Seq2"] = seq2;
                            dr["Colorid"] = drCheckColor["Colorid"];
                        }

                        dr.EndEdit();
                    }
                }

                if (!isColorMatch)
                {
                    DialogResult diaR = MyUtility.Msg.QuestionBox($@"Orignal assign colorID isn't same as locate colorID.
Do you want to continue? ");
                    if (diaR == DialogResult.No)
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
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.gridBatchAssignCellEstCutDate.ValidateControl();
            string spreadingNo = this.txtSpreadingNo.Text;
            string cell = this.txtCell2.Text;
            string cdate = string.Empty;
            string seq1 = this.txtSeq1.Text;
            string seq2 = this.txtSeq2.Text;
            string shift = this.txtShift.Text;
            if (!MyUtility.Check.Empty(this.txtBatchUpdateEstCutDate.Value))
            {
                cdate = this.txtBatchUpdateEstCutDate.Text;
            }

            foreach (DataRow dr in this.curTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (dr["Sel"].ToString() == "True")
                {
                    DataRow[] detaildr;
                    if (MyUtility.Check.Empty(dr["Ukey"]))
                    {
                        detaildr = this.detailTb.Select(string.Format("newkey = '{0}'", dr["newkey"]));
                    }
                    else
                    {
                        detaildr = this.detailTb.Select(string.Format("Ukey = '{0}'", dr["Ukey"]));
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

            this.Close();
        }

        private void TxtBatchUpdateEstCutDate_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtBatchUpdateEstCutDate.Value))
            {
                if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(this.txtBatchUpdateEstCutDate.Value)) > 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                }
            }
        }

        private void TxtSPNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem sele;
            sele = new SelectItem(this.sp, "OrderID", "15@300,400", this.sp.Columns["OrderID"].ToString(), columndecimals: "50");
            DialogResult result = sele.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNo.Text = sele.GetSelectedString();
            this.Filter();
        }

        /// <summary>
        /// 確認【布】寬是否會超過【裁桌】的寬度
        /// 若是 CutCell 取得寬度為 0 ，則不顯示訊息
        /// return Msg
        /// </summary>
        /// <param name="strCutCellID">CutCellID</param>
        /// <param name="strSCIRefno">SciRefno</param>
        private string CheckCuttingWidth(string strCutCellID, string strSCIRefno)
        {
            string chkwidth = MyUtility.GetValue.Lookup(string.Format(
                @"
select width_cm = width*2.54 
from Fabric 
where SCIRefno = '{0}'", strSCIRefno));
            string strCuttingWidth = MyUtility.GetValue.Lookup(string.Format(
                @"
select cuttingWidth = isnull (cuttingWidth, 0) 
from CutCell 
where   id = '{0}'
        and MDivisionID = '{1}'",
                strCutCellID,
                this.KeyWord));
            if (!chkwidth.Empty() && !strCuttingWidth.Empty())
            {
                decimal width_CM = decimal.Parse(chkwidth);
                decimal decCuttingWidth = decimal.Parse(strCuttingWidth);
                if (decCuttingWidth == 0)
                {
                    return string.Empty;
                }

                if (width_CM > decCuttingWidth)
                {
                    return string.Format("fab width greater than cutting cell {0}, please check it.", strCutCellID);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 確認【布】寬是否會超過【裁桌】的寬度
        /// 若是 CutCell 取得寬度為 0 ，則不顯示訊息
        /// Show Msg
        /// </summary>
        /// <param name="strCutCellID">CutCellID</param>
        /// <param name="strSCIRefno">SciRefno</param>
        private void ShowMsgCheckCuttingWidth(string strCutCellID, string strSCIRefno)
        {
            string msg = this.CheckCuttingWidth(strCutCellID, strSCIRefno);
            if (!msg.Empty())
            {
                MyUtility.Msg.WarningBox(msg);
            }
        }

        private void TxtSeq1_Validating(object sender, CancelEventArgs e)
        {
            string seq1 = this.txtSeq1.Text;
            if (!MyUtility.Check.Empty(seq1))
            {
                if (!MyUtility.Check.Seek($@"select 1 from po_Supp_Detail WITH (NOLOCK) where id='{this.Poid}' and Seq1='{seq1}'  and Junk=0"))
                {
                    MyUtility.Msg.WarningBox($@"Seq1: {seq1} data not found!");
                    this.txtSeq1.Text = string.Empty;
                    this.txtSeq1.Focus();
                    return;
                }
            }
            else
            {
                this.txtSeq1.Text = string.Empty;
            }
        }

        private void TxtSeq2_Validating(object sender, CancelEventArgs e)
        {
            string seq2 = this.txtSeq2.Text;
            if (!MyUtility.Check.Empty(seq2))
            {
                if (!MyUtility.Check.Seek($@"select 1 from po_Supp_Detail WITH (NOLOCK) where id='{this.Poid}' and Seq2='{seq2}' and Junk=0"))
                {
                    MyUtility.Msg.WarningBox($@"Seq2: {seq2} data not found!");
                    this.txtSeq2.Text = string.Empty;
                    this.txtSeq2.Focus();
                    return;
                }
            }
        }

        private void TxtSeq1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem item = new SelectItem($@"Select SEQ1,SEQ2,Colorid From PO_Supp_Detail WITH (NOLOCK) Where id='{this.Poid}' and junk=0", "SEQ1,SEQ2,Colorid", this.txtSeq1.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeq1.Text = item.GetSelectedString();
            this.txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();
        }

        private void TxtSeq2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem item = new SelectItem($@"Select SEQ1,SEQ2,Colorid From PO_Supp_Detail WITH (NOLOCK) Where id='{this.Poid}' and junk=0", "SEQ1,SEQ2,Colorid", this.txtSeq2.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeq1.Text = item.GetSelecteds()[0]["Seq1"].ToString();
            this.txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();
        }
    }
}
