using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using Ict.Data;
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class P03 : Sci.Win.Tems.QueryForm
    {
        DataTable detailTb;
        Ict.Win.UI.DataGridViewDateBoxColumn col_estcutdate;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            gridDetail.DataSource = gridbs;
            gridbs.DataSource = detailTb;
            setDetailGrid();
            txtCell1.MDivisionID = keyWord;
        }

        Ict.Win.UI.DataGridViewTextBoxColumn col_cutcell;
        public void setDetailGrid()
        {
            this.gridDetail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            DataGridViewGeneratorTextColumnSettings col_cutreason = cellcutreason.GetGridCell("RC");
            DataGridViewGeneratorCheckBoxColumnSettings col_check = new DataGridViewGeneratorCheckBoxColumnSettings();
            #region set grid
            Helper.Controls.Grid.Generator(this.gridDetail)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_check)
            .Text("factoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ID", header: "Cutting SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Date("estcutdate", header: "Org.Est.\nCut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("newestcutdate", header: "New Est.\nCut Date", width: Widths.AnsiChars(10)).Get(out col_estcutdate)
            .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), iseditingreadonly: true)

            .Text("Cutcellid", header: "Org. Cell", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("NewCutcellid", header: "New Cell", width: Widths.AnsiChars(2)).Get(out col_cutcell)

            .Text("CutReasonid", header: "Reason", width: Widths.AnsiChars(6), settings: col_cutreason)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true);
            this.gridDetail.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["CutReasonid"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["NewCutcellid"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            col_estcutdate.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                if (Convert.ToDateTime(e.FormattedValue) < DateTime.Now)
                {
                    MyUtility.Msg.WarningBox("<Est Cut Date> can not early today.");
                    DataRow dr = gridDetail.GetDataRow(e.RowIndex);
                    dr["newestcutdate"] = "";
                    dr.EndEdit();
                }
            };
            col_estcutdate.CellFormatting += (s, e) =>
            {
                e.CellStyle.BackColor = Color.Pink;
                e.CellStyle.ForeColor = Color.Red; 
            };
            changeeditable();
        }

        private void changeeditable()// Grid Cell 物件設定
        {
            #region cutcell
            col_cutcell.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    DataRow dr = gridDetail.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    DataTable cellTb;
                    DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);
                    sele = new SelectItem(cellTb, "ID", "10@300,300", dr["CutCellid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["NewCutcellid"] = sele.GetSelectedString();
                    dr.EndEdit();                    
                }
            };
            col_cutcell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridDetail.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty()) return;

                string oldvalue = dr["NewCutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();

                if (oldvalue == newvalue) return;

                DataTable cellTb;
                DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);

                DataRow[] seledr = cellTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["cutCellid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    return;
                }

                dr["cutCellid"] = newvalue;
                dr.EndEdit();
            };
            #endregion
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (detailTb != null) detailTb.Clear();
            string cutsp = txtCuttingSPNo.Text;
            string sp = txtSPNo.Text;
            string seq = txtSEQ.Text;
            string estcutdate = dateEstCutDate.Text;
            string sewinginline = dateSewingInline.Text;
            string cutref = txtCutRefNo.Text;
            if (MyUtility.Check.Empty(cutsp) && MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(dateEstCutDate.Value))
            {
                MyUtility.Msg.WarningBox("You must be entry conditions <Cutting SP#> or <SP#> or <Est. Cut Date>");
                return;
            }
            string sql = @"
Select * 
From
(
    Select a.*,
    (
        Select distinct Article+'/ ' 
	    From dbo.WorkOrder_Distribute b WITH (NOLOCK) 
	    Where b.workorderukey = a.Ukey and b.article!=''
        For XML path('')
    ) as article,
    (
        Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
        From WorkOrder_SizeRatio c WITH (NOLOCK) 
        Where c.WorkOrderUkey =a.Ukey 
        For XML path('')
    ) as SizeCode,
    (
        Select PatternPanel+'+ ' 
        From WorkOrder_PatternPanel c WITH (NOLOCK) 
        Where c.WorkOrderUkey =a.Ukey 
        For XML path('')
    ) as PatternPanel,
    (
	    Select  isnull(CONVERT (DATE, Min(sew.Inline),101),'')  as inline 
	    From SewingSchedule sew WITH (NOLOCK) ,SewingSchedule_detail sew_b,WorkOrder_Distribute h WITH (NOLOCK) 
	    Where h.WorkOrderUkey = a.ukey and sew.id=sew_b.id and h.orderid = sew_b.OrderID 
			    and h.Article = sew_b.Article and h.SizeCode = h.SizeCode and h.orderid = sew.orderid
    )  as Sewinline,
    (
	    Select isnull(Min(cut.cdate),'')
	    From cuttingoutput cut WITH (NOLOCK) ,cuttingoutput_detail cut_b WITH (NOLOCK) 
	    Where cut_b.workorderukey = a.Ukey and cut.id = cut_b.id
    )  as actcutdate,
    '' as NewestcutDate, '' as cutreasonid ,0 as sel
    ,NewCutcellid=''
    from Workorder a";
            string where = string.Format(" Where cutplanid!='' and MDivisionId = '{0}'", keyWord);
            if (!MyUtility.Check.Empty(cutsp)) where = where + string.Format(" and id='{0}'",cutsp);
            if (!MyUtility.Check.Empty(sp)) where = where + string.Format(" and OrderID='{0}'", sp);
            if (!MyUtility.Check.Empty(seq)) where = where + string.Format(" and Seq1+SEQ2='{0}'", seq);
            if (!MyUtility.Check.Empty(dateEstCutDate.Value)) where = where + string.Format("and estcutdate='{0}'",estcutdate);
            if (!MyUtility.Check.Empty(cutref)) where = where + string.Format(" and cutref='{0}'", cutref);
            if (!MyUtility.Check.Empty(txtfactoryByM.Text)) where = where + string.Format(" and a.Factoryid='{0}'", txtfactoryByM.Text);

            sql = sql + where + " ) as #tmp ";
            where = " Where 1=1 and actcutdate =''";
            if (!MyUtility.Check.Empty(dateSewingInline.Value)) where = where + string.Format(" and Sewinline='{0}'", sewinginline);
            sql = sql + where;
            DualResult dResult = DBProxy.Current.Select(null, sql, out detailTb);
            if (detailTb == null || detailTb.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found!!");
                return ;
            }
            gridDetail.DataSource = gridbs;
            gridbs.DataSource = detailTb;
        }

        private void dateNewEstCutDate_Validating(object sender, CancelEventArgs e)
        {
            if (dateNewEstCutDate.Value != null && dateNewEstCutDate.Value < DateTime.Now)
            {
                MyUtility.Msg.WarningBox("<Est Cut Date> can not early today.");
                dateNewEstCutDate.Value = null;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            gridDetail.ValidateControl();
            if (detailTb == null) return;
            string estcutdate = dateNewEstCutDate.Text;
            string reason = txtcutReason.TextBox1.Text;
            string cell = txtCell1.Text;
            foreach (DataRow dr in detailTb.Rows)
            {
                if(dr["Sel"].ToString()=="1")
                {
                    dr["newestcutdate"] = estcutdate;
                    dr["cutreasonid"] = reason;
                    dr["NewCutcellid"] = cell;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            gridDetail.ValidateControl();
            string update = "";
            if (MyUtility.Check.Empty(detailTb))return;
            if (detailTb.Rows.Count == 0) return;

            foreach (DataRow dr in detailTb.Rows)
            {

                if ((dr["EstCutDate"] != dr["NewEstCutDate"] && !MyUtility.Check.Empty((dr["NewEstCutDate"].ToString().Replace("/", "")))) || (dr["Cutcellid"] != dr["NewCutcellid"] && !MyUtility.Check.Empty(dr["NewCutcellid"])))
                {
                    if (MyUtility.Check.Empty(dr["CutReasonid"]))
                    {
                        MyUtility.Msg.WarningBox("<Reason> can not be empty.");
                        return;
                    }

                    if (MyUtility.Check.Empty(dr["NewCutcellid"]))
                    {
                        update = update + $"Update Workorder Set estcutdate ='{dr["newestcutdate"]}' where Ukey = {dr["Ukey"]}; ";
                    }
                    else if (!MyUtility.Check.Empty((dr["NewEstCutDate"].ToString().Replace("/", ""))))
                    {
                        update = update + $"Update Workorder Set CutCellid = '{dr["NewCutcellid"]}' where Ukey = {dr["Ukey"]}; ";
                    }
                    else
                    {
                        update = update + $"Update Workorder Set estcutdate ='{dr["newestcutdate"]}',CutCellid = '{dr["NewCutcellid"]}' where Ukey = {dr["Ukey"]}; ";
                    }


                    update = update + string.Format("Insert into Workorder_EstCutdate(WorkOrderUkey,orgEstCutDate,NewEstCutDate,CutReasonid,ID,OrgCutCellid,NewCutCellid) Values({0},'{1}','{2}','{3}','{4}','{5}','{6}');", dr["Ukey"], Convert.ToDateTime(dr["EstCutDate"]).ToShortDateString(), dr["NewEstCutDate"], dr["CutReasonid"], dr["ID"], dr["Cutcellid"], dr["NewCutcellid"]);
                }
            }

            var distnct_id = detailTb.AsEnumerable().
                Where(w => w.Field<int>("Sel") == 1 && w.Field<object>("EstCutDate") != w.Field<object>("NewEstCutDate") && !MyUtility.Check.Empty(w.Field < object >("NewEstCutDate"))).
                Select(row => row.Field<string>("ID")).Distinct();
            foreach (string tmp_id in distnct_id) {
                // Mantis_9252 連帶更新Cutting資料表的CutInLine及CutOffLine,CutInLine-->MIN(Workorder.estcutdate),CutOffLine-->MAX(Workorder.estcutdate)
                update = update + string.Format(@"update cutting set CutInLine =  wk.Min_Wk_estcutdate,CutOffLine = wk.Max_Wk_estcutdate
from dbo.cutting WITH (NOLOCK)
left join (select id,Min_Wk_estcutdate =  min(estcutdate), Max_Wk_estcutdate = max(estcutdate) 
			from dbo.WorkOrder  WITH (NOLOCK) where id = '{0}' group by id) wk on wk.id = cutting.ID
where cutting.ID = '{0}';", tmp_id);

                //orders.CutInLine及CutOffLine也要連帶更新_Wk_estcutdate
                                               
                update = update + string.Format(@"update orders set CutInLine =  wk.Min_Wk_estcutdate, CutOffLine = wk.Max_Wk_estcutdate 
                                                 from dbo.orders WITH (NOLOCK)
                                                left join (select id,Min_Wk_estcutdate =  min(estcutdate), Max_Wk_estcutdate = max(estcutdate) 
			                                                from dbo.WorkOrder  WITH (NOLOCK) where id = '{0}' group by id) wk on wk.id = orders.POID
                                                where orders.POID = '{0}';", tmp_id);

            }

            if (update == "") return;
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, update)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(update, upResult);
                        return;
                    }
                    _transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            MyUtility.Msg.InfoBox("Finished");
        }
    }
}
