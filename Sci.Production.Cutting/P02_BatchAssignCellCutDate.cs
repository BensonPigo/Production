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

namespace Sci.Production.Cutting
{
    public partial class P02_BatchAssignCellCutDate : Sci.Win.Subs.Base
    {
        private DataTable curTb;
        private DataTable detailTb;
        private DataTable sp;
        public P02_BatchAssignCellCutDate(DataTable cursor)
        {
            InitializeComponent();
            txtCutCell.FactoryId = Sci.Env.User.Keyword;
            txtCell2.FactoryId = Sci.Env.User.Keyword;
            detailTb = cursor;
            curTb = cursor.Copy();
            curTb.Columns.Add("Sel", typeof(bool));
            gridsetup();
            btnFilter_Click(null, null);  //1390: CUTTING_P02_BatchAssignCellCutDate，當進去此功能時應直接預帶資料。

            MyUtility.Tool.ProcessWithDatatable(curTb, "orderid", "select distinct orderid from #tmp", out sp);
        }


        private void gridsetup()
        {
            DataGridViewGeneratorTextColumnSettings Cell = new DataGridViewGeneratorTextColumnSettings();
            this.gridBatchAssignCellEstCutDate.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Cell.EditingMouseDown += (s, e) =>
            {
                DualResult DR; DataTable DT; SelectItem S;
                if (e.Button == MouseButtons.Right)
                {
                    string keyWord = Sci.Env.User.Keyword;
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    string CUTCELL = string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord);
                    DR = DBProxy.Current.Select(null, CUTCELL, out DT);
                    S = new SelectItem(DT, "ID", "10", DT.Columns["id"].ToString(), false, ",");
                    DialogResult result = S.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = S.GetSelectedString();
                }
            };
            Cell.CellValidating += (s, e) =>
            {
                DualResult DR; DataTable DT;

                string keyWord = Sci.Env.User.Keyword;
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridBatchAssignCellEstCutDate.GetDataRow(e.RowIndex);
                string oldvalue = dr["Cutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                string CUTCELL = string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord);
                DR = DBProxy.Current.Select(null, CUTCELL, out DT);

                DataRow[] seledr = DT.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    dr["Cutcellid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }

                dr["Cutcellid"] = newvalue;
                dr.EndEdit();
            };
            DataGridViewGeneratorDateColumnSettings EstCutDate = new DataGridViewGeneratorDateColumnSettings();
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


            Helper.Controls.Grid.Generator(this.gridBatchAssignCellEstCutDate)
             .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
             .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
             .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5), iseditingreadonly: true)
             .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Text("Cutcellid", header: "Cell", width: Widths.AnsiChars(2), settings: Cell, iseditingreadonly: false)
             .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
             .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
             .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
             .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
             .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: EstCutDate)
             .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.gridBatchAssignCellEstCutDate.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["Cutcellid"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["Cutcellid"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridBatchAssignCellEstCutDate.Columns["estcutdate"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridBatchAssignCellEstCutDate.Columns["estcutdate"].DefaultCellStyle.ForeColor = Color.Red;

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
            string cell = txtCell2.Text;
            foreach (DataRow dr in curTb.Rows)
            {
                if (dr["Sel"].ToString() == "True")
                {
                    DataRow[] detaildr = detailTb.Select(string.Format("Ukey = '{0}'", dr["Ukey"]));
                    //  detaildr[0]["Cutcellid"] = cell;
                    dr["Cutcellid"] = cell;
                }
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
                if (dr["Sel"].ToString() == "True")
                {
                    DataRow[] detaildr = detailTb.Select(string.Format("Ukey = '{0}'", dr["Ukey"]));
                    if (cdate != "")
                    {
                        //  detaildr[0]["estcutdate"] = cdate;
                        dr["estcutdate"] = cdate;
                    }
                    else
                    {
                        //  detaildr[0]["estcutdate"] = DBNull.Value;
                        dr["estcutdate"] = DBNull.Value;
                    }
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.gridBatchAssignCellEstCutDate.ValidateControl();
            string cell = txtCell2.Text; string cdate = "";
            if (!MyUtility.Check.Empty(txtBatchUpdateEstCutDate.Value))
            {
                cdate = txtBatchUpdateEstCutDate.Text;
            };
            foreach (DataRow dr in curTb.Rows)
            {
                if (dr["Sel"].ToString() == "True")
                {
                    DataRow[] detaildr = detailTb.Select(string.Format("Ukey = '{0}'", dr["Ukey"]));
                    if (cell != "")
                    {
                        detaildr[0]["Cutcellid"] = cell;
                        dr["Cutcellid"] = cell;
                    }
                    if (dr["Cutcellid"].ToString() != "")
                    {
                        string CUTCELL = dr["Cutcellid"].ToString();
                        detaildr[0]["Cutcellid"] = CUTCELL;

                    }
                    else
                    {
                        detaildr[0]["Cutcellid"] = DBNull.Value;
                        dr["Cutcellid"] = DBNull.Value;
                    }
                    if (cdate != "")
                    {
                        detaildr[0]["estcutdate"] = cdate;
                        dr["estcutdate"] = cdate;
                    }
                    if (dr["estcutdate"].ToString() != "")
                    {
                        string ESTDATE = dr["estcutdate"].ToString();
                        detaildr[0]["estcutdate"] = ESTDATE;
                    }
                    else
                    {
                        detaildr[0]["estcutdate"] = DBNull.Value;
                        dr["estcutdate"] = DBNull.Value;
                    }
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
    }
}
