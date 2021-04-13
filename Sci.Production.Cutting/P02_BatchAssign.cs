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
        /// <param name="distqtyTb">DistqtyTb</param>
        public P02_BatchAssign(DataTable cursor, string id, DataTable distqtyTb)
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

            this.ID = id;
            this.DistqtyTb = distqtyTb;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.bgWorkerUpdateInfo.RunWorkerAsync();
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

        private string ID;
        private DataTable DistqtyTb;
        private DataTable detailData;
        private DataTable summaryData;
        private DataTable dtG;
        private DataTable dtF;
        private DateTime MinInLine;
        private DateTime MaxOffLine;
        private List<string> FtyFroup = new List<string>();
        private List<InOffLineList> AllDataTmp = new List<InOffLineList>();
        private List<InOffLineList> AllData = new List<InOffLineList>();
        private List<InOffLineList_byFabricPanelCode> AllDataTmp2 = new List<InOffLineList_byFabricPanelCode>();
        private List<InOffLineList_byFabricPanelCode> AllData2 = new List<InOffLineList_byFabricPanelCode>();
        private List<PublicPrg.Prgs.Day> Days = new List<PublicPrg.Prgs.Day>();
        private List<PublicPrg.Prgs.Day> Days2 = new List<PublicPrg.Prgs.Day>();
        private List<LeadTime> LeadTimeList = new List<LeadTime>();

        private bool Query1()
        {
            List<string> orderIDs = this.DistqtyTb.AsEnumerable()
                .Select(s => new { ID = MyUtility.Convert.GetString(s["OrderID"]) })
                .Where(w => w.ID != "EXCESS")
                .Distinct()
                .Select(s => s.ID)
                .ToList();

            if (!this.Check_Subprocess_LeadTime(orderIDs))
            {
                return false;
            }

            #region 起手資料
            string cmd = $@"SELECT s.* FROM SewingSchedule s INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID WHERE s.OrderID in ('{string.Join("','", orderIDs)}')";
            DualResult result = DBProxy.Current.Select(null, cmd, out DataTable dt_Schedule);
            if (!result)
            {
                return false;
            }

            if (dt_Schedule.Rows.Count == 0)
            {
                return false;
            }
            #endregion

            #region 時間軸
            this.Days.Clear();

            // 取出最早InLine / 最晚OffLine
            this.MinInLine = dt_Schedule.AsEnumerable().Min(o => Convert.ToDateTime(o["Inline"]));
            this.MaxOffLine = dt_Schedule.AsEnumerable().Max(o => Convert.ToDateTime(o["offline"]));

            int maxLeadTime = this.LeadTimeList.Max(o => o.LeadTimeDay);
            int minLeadTime = this.LeadTimeList.Min(o => o.LeadTimeDay);

            // 起點 = (最早Inline - 最大Lead Time)、終點 = (最晚Offline - 最小Lead Time)
            DateTime start_where = this.MinInLine;
            DateTime end_where = this.MaxOffLine;

            List<PublicPrg.Prgs.Day> daylist1 = GetDays(maxLeadTime, start_where.Date, this.FtyFroup);
            List<PublicPrg.Prgs.Day> daylist2 = GetDays(minLeadTime, end_where.Date, this.FtyFroup);
            foreach (var item in daylist1)
            {
                this.Days.Add(item);
            }

            foreach (var item in daylist2)
            {
                this.Days.Add(item);
            }

            DateTime d2 = daylist2.Select(s => s.Date).Min(m => m.Date);

            // 若 daylist1 是1/1~1/3, daylist2 是1/10~1/12, 中間也要補上
            if (start_where < d2)
            {
                List<PublicPrg.Prgs.Day> daylist3 = GetRangeHoliday(start_where, d2, this.FtyFroup);
                foreach (var item in daylist3)
                {
                    this.Days.Add(item);
                }
            }

            this.Days = this.Days
                .Select(s => new { s.Date.Date, s.IsHoliday }).Distinct()
                .Select(s => new PublicPrg.Prgs.Day { Date = s.Date.Date, IsHoliday = s.IsHoliday })
                .OrderBy(o => o.Date).ToList();
            #endregion

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(5);

            this.AllData = GetInOffLineList(dt_Schedule, this.Days, startdate: DateTime.Today.Date, bw: this.bgWorkerUpdateInfo);

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(90);

            List<DataTable> leadTimeList = GetCutting_WIP_DataTable(this.Days, this.AllData.OrderBy(o => o.OrderID).ToList());

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(95);

            this.summaryData = leadTimeList[0];
            this.detailData = leadTimeList[1];

            this.dtG = this.summaryData.Clone();
            foreach (DataRow item in this.summaryData.Rows)
            {
                DataRow drdStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Desc./Sewing Date] = 'Std. Qty'")[0];
                this.dtG.ImportRow(drdStdQty);
                DataRow drdAccStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Desc./Sewing Date] = 'Accu. Std. Qty'")[0];
                DataRow drdAccCutPlan = this.detailData.Select($"SP='{item["SP"]}' and [Desc./Sewing Date] = 'Accu. Cut Plan Qty'")[0];

                // 2 是日期欄位開始
                for (int i = 2; i < this.detailData.Columns.Count; i++)
                {
                    string bal = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(drdAccCutPlan[i]) - MyUtility.Convert.GetDecimal(drdAccStdQty[i]), 0));
                    if (MyUtility.Convert.GetString(drdAccCutPlan[i]) == string.Empty && MyUtility.Convert.GetString(drdAccStdQty[i]) == string.Empty)
                    {
                        bal = string.Empty;
                    }

                    string wip = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(item[i - 1]), 2));
                    if (MyUtility.Convert.GetString(item[i - 1]) == string.Empty)
                    {
                        wip = string.Empty;
                    }

                    string srow2 = string.Empty;
                    if (!(MyUtility.Convert.GetString(bal) == string.Empty && MyUtility.Convert.GetString(wip) == string.Empty))
                    {
                        srow2 = wip + " / " + bal;
                    }

                    drdAccCutPlan[i] = srow2;
                }

                drdAccCutPlan["SP"] = string.Empty;
                this.dtG.ImportRow(drdAccCutPlan);
            }

            return true;
        }

        private bool Query2()
        {
            List<string> orderIDs = this.DistqtyTb.AsEnumerable()
                .Select(s => new { ID = MyUtility.Convert.GetString(s["OrderID"]) })
                .Where(w => w.ID != "EXCESS")
                .Distinct()
                .Select(s => s.ID)
                .ToList();

            if (!this.Check_Subprocess_LeadTime(orderIDs))
            {
                return false;
            }

            #region 起手資料
            string cmd = $@"SELECT s.* FROM SewingSchedule s INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID WHERE s.OrderID in ('{string.Join("','", orderIDs)}')";
            DualResult result = DBProxy.Current.Select(null, cmd, out DataTable dt_Schedule);
            if (!result)
            {
                return false;
            }

            if (dt_Schedule.Rows.Count == 0)
            {
                return false;
            }
            #endregion

            #region 時間軸
            this.Days2.Clear();

            // 取出最早InLine / 最晚OffLine
            this.MinInLine = dt_Schedule.AsEnumerable().Min(o => Convert.ToDateTime(o["Inline"]));
            this.MaxOffLine = dt_Schedule.AsEnumerable().Max(o => Convert.ToDateTime(o["offline"]));

            int maxLeadTime = this.LeadTimeList.Max(o => o.LeadTimeDay);
            int minLeadTime = this.LeadTimeList.Min(o => o.LeadTimeDay);

            // 起點 = (最早Inline - 最大Lead Time)、終點 = (最晚Offline - 最小Lead Time)
            DateTime start_where = this.MinInLine;
            DateTime end_where = this.MaxOffLine;

            List<PublicPrg.Prgs.Day> daylist1 = GetDays(maxLeadTime, start_where, this.FtyFroup);
            List<PublicPrg.Prgs.Day> daylist2 = GetDays(minLeadTime, end_where, this.FtyFroup);
            foreach (var item in daylist1)
            {
                this.Days2.Add(item);
            }

            foreach (var item in daylist2)
            {
                this.Days2.Add(item);
            }

            DateTime d2 = daylist2.Select(s => s.Date).Min(m => m.Date);

            // 若 daylist1 是1/1~1/3, daylist2 是1/10~1/12, 中間也要補上
            if (start_where < d2)
            {
                List<PublicPrg.Prgs.Day> daylist3 = GetRangeHoliday(start_where, d2, this.FtyFroup);
                foreach (var item in daylist3)
                {
                    this.Days2.Add(item);
                }
            }

            this.Days2 = this.Days2
                .Select(s => new { s.Date, s.IsHoliday }).Distinct() // start和end加入日期有重複
                .Select(s => new PublicPrg.Prgs.Day { Date = s.Date, IsHoliday = s.IsHoliday })
                .OrderBy(o => o.Date).ToList();
            #endregion

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(5);

            this.AllData2 = GetInOffLineList_byFabricPanelCode(dt_Schedule, this.Days2, startdate: DateTime.Today.Date, bw: this.bgWorkerUpdateInfo);

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(90);

            List<DataTable> leadTimeList = GetCutting_WIP_DataTable(this.Days2, this.AllData2.OrderBy(o => o.OrderID).ToList());

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(95);

            this.summaryData = leadTimeList[0];
            this.detailData = leadTimeList[1];

            this.dtF = this.summaryData.Clone();
            foreach (DataRow item in this.summaryData.Rows)
            {
                DataRow drdStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Fab. Panel Code] = '{item["Fab. Panel Code"]}' and [Desc./Sewing Date] = 'Std. Qty'")[0];
                this.dtF.ImportRow(drdStdQty);
                DataRow drdAccStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Fab. Panel Code] = '{item["Fab. Panel Code"]}' and [Desc./Sewing Date] = 'Accu. Std. Qty'")[0];
                DataRow drdAccCutPlan = this.detailData.Select($"SP='{item["SP"]}' and [Fab. Panel Code] = '{item["Fab. Panel Code"]}' and [Desc./Sewing Date] = 'Accu. Cut Plan Qty'")[0];

                // 2 是日期欄位開始
                for (int i = 3; i < this.detailData.Columns.Count; i++)
                {
                    string bal = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(drdAccCutPlan[i]) - MyUtility.Convert.GetDecimal(drdAccStdQty[i]), 0));
                    if (MyUtility.Convert.GetString(drdAccCutPlan[i]) == string.Empty && MyUtility.Convert.GetString(drdAccStdQty[i]) == string.Empty)
                    {
                        bal = string.Empty;
                    }

                    string wip = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(item[i - 1]), 2));
                    if (MyUtility.Convert.GetString(item[i - 1]) == string.Empty)
                    {
                        wip = string.Empty;
                    }

                    string srow2 = string.Empty;
                    if (!(MyUtility.Convert.GetString(bal) == string.Empty && MyUtility.Convert.GetString(wip) == string.Empty))
                    {
                        srow2 = wip + " / " + bal;
                    }

                    drdAccCutPlan[i] = srow2;
                }

                drdAccCutPlan["SP"] = string.Empty;
                this.dtF.ImportRow(drdAccCutPlan);
            }

            return true;
        }

        private void AfterQuery1()
        {
            this.listControlBindingSource1.DataSource = this.dtG;
            foreach (DataColumn item in this.dtG.Columns)
            {
                this.Helper.Controls.Grid.Generator(this.gridGarment)
                .Text(item.ColumnName, header: item.ColumnName, width: Widths.Auto(), iseditingreadonly: true)
                ;
            }

            this.gridGarment.AutoResizeColumns();

            int columnIndex = 1;
            foreach (var day in this.Days)
            {
                // string dateStr = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                // gridGarment.Columns[ColumnIndex].Name = dateStr;
                // 假日的話粉紅色
                if (day.IsHoliday)
                {
                    this.gridGarment.Columns[columnIndex].HeaderCell.Style.BackColor = Color.FromArgb(255, 199, 206);
                }

                columnIndex++;
            }

            // 扣除無產出的日期
            List<PublicPrg.Prgs.Day> removeDays = new List<PublicPrg.Prgs.Day>();

            foreach (var day in this.Days)
            {
                // 如果該日期，不是「有資料」，則刪掉
                if (!this.AllData.Where(x => x.InOffLines.Where(y => y.DateWithLeadTime == day.Date
                                                                && (MyUtility.Convert.GetInt(y.CutQty) > 0 || MyUtility.Convert.GetInt(y.StdQty) > 0)) // 不同於R01,是因此只有顯示CutQty,StdQty資料
                                                                .Any())
                                       .Any())
                {
                    removeDays.Add(day);
                }
                else if (!this.AllData.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                    && MyUtility.Convert.GetDecimal(y.WIP) > 0)
                                                                .Any())
                                       .Any()
                    && day.IsHoliday)
                {
                    removeDays.Add(day);
                }
                else if (day.Date < DateTime.Today)
                {
                    removeDays.Add(day);
                }
            }

            columnIndex = 1;
            foreach (var day in this.Days)
            {
                if (removeDays.Where(o => o.Date == day.Date).Any())
                {
                    this.gridGarment.Columns[columnIndex].Visible = false; // 隱藏,但還在 ColumnIndex不會變
                }

                columnIndex++;
            }

            if (removeDays.Count == columnIndex - 1)
            {
                this.gridGarment.DataSource = null;

                // gridGarment.Columns[0].Visible = false;
            }

            #region 關閉排序功能
            for (int i = 0; i < this.gridGarment.ColumnCount; i++)
            {
                this.gridGarment.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
        }

        private void AfterQuery2()
        {
            this.listControlBindingSource2.DataSource = this.dtF;
            foreach (DataColumn item in this.dtF.Columns)
            {
                this.Helper.Controls.Grid.Generator(this.gridFabric_Panel_Code)
                .Text(item.ColumnName, header: item.ColumnName, width: Widths.Auto(), iseditingreadonly: true)
                ;
            }

            int columnIndex = 2;
            foreach (var day in this.Days2)
            {
                // string dateStr = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                // gridFabric_Panel_Code.Columns[ColumnIndex].Name = dateStr;
                // 假日的話粉紅色
                if (day.IsHoliday)
                {
                    this.gridFabric_Panel_Code.Columns[columnIndex].HeaderCell.Style.BackColor = Color.FromArgb(255, 199, 206);
                }

                columnIndex++;
            }

            // 扣除無產出的日期
            List<PublicPrg.Prgs.Day> removeDays = new List<PublicPrg.Prgs.Day>();

            foreach (var day in this.Days2)
            {
                // 如果該日期，不是「有資料」，則刪掉
                if (!this.AllData2.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                    && (MyUtility.Convert.GetInt(y.CutQty) > 0 || MyUtility.Convert.GetInt(y.StdQty) > 0)) // 不同於R01,是因此只有顯示CutQty,StdQty資料
                                                                .Any())
                                       .Any())
                {
                    removeDays.Add(day);
                }
                else if (!this.AllData.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                    && MyUtility.Convert.GetDecimal(y.WIP) > 0)
                                                                .Any())
                                       .Any()
                    && day.IsHoliday)
                {
                    removeDays.Add(day);
                }
                else if (day.Date < DateTime.Today)
                {
                    removeDays.Add(day);
                }
            }

            columnIndex = 2;
            foreach (var day in this.Days2)
            {
                if (removeDays.Where(o => o.Date == day.Date).Any())
                {
                    this.gridFabric_Panel_Code.Columns[columnIndex].Visible = false; // 隱藏,但還在 ColumnIndex不會變
                }

                columnIndex++;
            }

            this.gridFabric_Panel_Code.Columns[0].Width = 115;

            if (removeDays.Count == columnIndex - 2)
            {
                this.gridFabric_Panel_Code.DataSource = null;

                // gridFabric_Panel_Code.Columns[0].Visible = false;
                // gridFabric_Panel_Code.Columns[1].Visible = false;
            }

            #region 關閉排序功能
            for (int i = 0; i < this.gridFabric_Panel_Code.ColumnCount; i++)
            {
                this.gridFabric_Panel_Code.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
            return;
        }

        /// <summary>
        /// Check Subprocess LeadTime
        /// </summary>
        /// <param name="orderIDs">List Order ID</param>
        /// <returns>bool</returns>
        public bool Check_Subprocess_LeadTime(List<string> orderIDs)
        {
            DualResult result;
            string cmd = $@"
SELECT  DISTINCT OrderID, s.MDivisionID, s.FactoryID
INTO #OrderList
FROM SewingSchedule s WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID
WHERE o.LocalOrder = 0
AND OrderID in ('{string.Join("','", orderIDs)}')
";

            cmd += $@"
SELECT DIStINCT  b.POID ,a.OrderID ,b.FtyGroup, a.MDivisionID, a.FactoryID
FROM #OrderList a
INNER JOIN Orders b ON a.OrderID= b.ID 

drop table #OrderList
";
            result = DBProxy.Current.Select(null, cmd, out DataTable poID_dt);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            List<string> poID_List = poID_dt.AsEnumerable().Select(o => o["POID"].ToString()).Distinct().ToList();
            this.FtyFroup = poID_dt.AsEnumerable().Select(o => o["FtyGroup"].ToString()).Distinct().ToList();
            List<string> msg = new List<string>();

            foreach (DataRow dr in poID_dt.Rows)
            {
                string pOID = dr["POID"].ToString();
                string orderID = dr["OrderID"].ToString();
                string mDivisionID = dr["MDivisionID"].ToString();
                string factoryID = dr["FactoryID"].ToString();

                GetGarmentListTable(string.Empty, pOID, "''", out DataTable garmentTb, out DataTable articleGroupDT);

                List<string> annotationList = garmentTb.AsEnumerable().Where(o => !MyUtility.Check.Empty(o["Annotation"].ToString())).Select(o => o["Annotation"].ToString()).Distinct().ToList();

                List<string> annotationList_Final = new List<string>();

                foreach (string annotation in annotationList)
                {
                    foreach (var item in annotation.Split('+'))
                    {
                        string input = string.Empty;
                        for (int i = 0; i <= item.Length - 1; i++)
                        {
                            // 排除掉數字
                            if (!int.TryParse(item[i].ToString(), out int x))
                            {
                                input += item[i].ToString();
                            }
                        }

                        if (!annotationList_Final.Contains(input) && MyUtility.Check.Seek($"SELECT 1 FROM Subprocess WHERE ID='{input}' "))
                        {
                            annotationList_Final.Add(input);
                        }
                    }
                }

                string annotationStr = annotationList_Final.OrderBy(o => o.ToString()).JoinToString("+");
                string chk_LeadTime = $@"
SELECT DISTINCT SD.ID
                ,Subprocess.IDs
                ,LeadTime= s.LeadTime
FROM SubprocessLeadTime s WITH(NOLOCK)
INNER JOIN SubprocessLeadTime_Detail SD WITH(NOLOCK) on s.ID = sd.ID
OUTER APPLY(
	SELECT IDs=STUFF(
	 (
		SELECT '+'+SubprocessID
		FROM SubprocessLeadTime_Detail WITH(NOLOCK)
		WHERE ID = SD.ID
		FOR XML PATH('')
	)
	,1,1,'')
)Subprocess
WHERE Subprocess.IDs = '{annotationStr}'
and s.MDivisionID = '{mDivisionID}'
";
                result = DBProxy.Current.Select(null, chk_LeadTime, out DataTable leadTime_dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                // 收集需要顯示訊息的Subprocess ID
                if (leadTime_dt.Rows.Count == 0 && annotationStr != string.Empty)
                {
                    msg.Add(mDivisionID + ";" + annotationStr);
                }
                else
                {
                    // 記錄下加工段的Lead Time
                    LeadTime o = new LeadTime()
                    {
                        OrderID = orderID,
                        LeadTimeDay = MyUtility.Check.Empty(annotationStr) ? 0 : Convert.ToInt32(leadTime_dt.Rows[0]["LeadTime"]), // 加工段為空，LeadTimeDay = 0
                    };
                    this.LeadTimeList.Add(o);
                }
            }

            if (msg.Count > 0)
            {
                string message = "<" + msg.Distinct().OrderBy(o => o).JoinToString(">" + Environment.NewLine + "<") + ">";
                message = message.Replace(";", "><");
                message += Environment.NewLine + @"Please set cutting lead time in [Cutting_B09. Subprocess Lead Time].
When the settings are complete, can be use this function!!
";

                // MyUtility.Msg.InfoBox(message);
                return false;
            }

            return true;
        }

        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex > 0)
            {
                return;
            }

            if (e.RowIndex > 0)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }

            if (!this.IsEmptyCellValue(e.RowIndex, (Win.UI.Grid)sender))
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Bottom = ((Win.UI.Grid)sender).AdvancedCellBorderStyle.Bottom;
            }
        }

        private bool IsEmptyCellValue(int row, Win.UI.Grid grid)
        {
            if (row == grid.Rows.Count - 1)
            {
                return true;
            }

            DataGridViewCell cell1 = grid["SP", row];

            return MyUtility.Check.Empty(cell1.Value);
        }

        private int Qend = 0;
        private int Qrun = 1;

        private void BgWorkerUpdateInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
            {
                this.Qend = 0;
                if (!this.Query1())
                {
                    this.Qend = 3;
                    this.bgWorkerUpdateInfo.CancelAsync();
                    this.bgWorkerUpdateInfo.ReportProgress(0);
                    return;
                }

                this.Qend = 1;

                if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
                {
                    return;
                }

                this.bgWorkerUpdateInfo.ReportProgress(100);

                this.Qrun = 2;
                if (!this.Query2())
                {
                    this.Qend = 3;
                    this.bgWorkerUpdateInfo.CancelAsync();
                    this.bgWorkerUpdateInfo.ReportProgress(0);
                    return;
                }

                this.Qend = 2;
                this.bgWorkerUpdateInfo.ReportProgress(100);
            }
        }

        private void BgWorkerUpdateInfo_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return;
            }

            if (this.Qrun == 1)
            {
                this.progressBar1.Value = e.ProgressPercentage;
            }
            else
            {
                this.progressBar2.Value = e.ProgressPercentage;
            }

            try
            {
                if (this.Qend == 1)
                {
                    this.progressBar1.Visible = false;
                    this.AfterQuery1();
                    this.Qend = 0;
                }
                else if (this.Qend == 2)
                {
                    this.progressBar2.Visible = false;
                    this.AfterQuery2();
                    this.Qend = 0;
                }
                else if (this.Qend == 3)
                {
                    this.progressBar1.Visible = false;
                    this.progressBar2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void P02_BatchAssign_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bgWorkerUpdateInfo.IsBusy)
            {
                this.bgWorkerUpdateInfo.CancelAsync();
            }
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex > -1)
            {
                if (e.RowIndex % 4 > 1)
                {
                    e.CellStyle.BackColor = Color.FromArgb(128, 255, 255);
                }
            }
        }
    }
}
