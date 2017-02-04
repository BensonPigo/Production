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
            detailgrid.DataSource = gridbs;
            gridbs.DataSource = detailTb;
            setDetailGrid();

        }
        public void setDetailGrid()
        {
            this.detailgrid.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            DataGridViewGeneratorTextColumnSettings col_cutreason = cellcutreason.GetGridCell("RC");
            DataGridViewGeneratorCheckBoxColumnSettings col_check = new DataGridViewGeneratorCheckBoxColumnSettings();
            #region set grid
            Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_check)
                .Text("ID", header: "Cutting SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Date("estcutdate", header: "Org.Est.\nCut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("newestcutdate", header: "New.Est.\nCut Date", width: Widths.AnsiChars(10)).Get(out col_estcutdate)
                .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CutReasonid", header: "Reason", width: Widths.AnsiChars(6), settings: col_cutreason)
                .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true);
                this.detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns[8].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            col_estcutdate.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                if (Convert.ToDateTime(e.FormattedValue) < DateTime.Now)
                {
                    MyUtility.Msg.WarningBox("<Est Cut Date> can not early today.");
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    dr["newestcutdate"] = "";
                    dr.EndEdit();
                }
            };
            col_estcutdate.CellFormatting += (s, e) =>
            {
                e.CellStyle.BackColor = Color.Pink;
                e.CellStyle.ForeColor = Color.Red; 
            };            
        }

        private void button_Query_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            string cutsp = textBox_Cutsp.Text;
            string sp = textBox_sp.Text;
            string seq = textBox_seq.Text;
            string estcutdate = dateBox_estcutdate.Text;
            string sewinginline = dateBox_sewinginline.Text;
            string cutref = textBox_cutref.Text;
            if (MyUtility.Check.Empty(cutsp) && MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(dateBox_estcutdate.Value))
            {
                MyUtility.Msg.WarningBox("You must be entry conditions <Cutting SP#> or <SP#> or <Est. Cut Date>");
                return;
            }
            string sql =
            @"
            Select * 
            From
            (
            Select a.*,
            (
                Select distinct Article+'/ ' 
			    From dbo.WorkOrder_Distribute b
			    Where b.workorderukey = a.Ukey and b.article!=''
                For XML path('')
            ) as article,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
                From WorkOrder_SizeRatio c 
                Where c.WorkOrderUkey =a.Ukey 
                For XML path('')
            ) as SizeCode,
            (
                Select PatternPanel+'+ ' 
                From WorkOrder_PatternPanel c
                Where c.WorkOrderUkey =a.Ukey 
                For XML path('')
            ) as PatternPanel,
			(
				Select  isnull(CONVERT (DATE, Min(sew.Inline),101),'')  as inline 
				From SewingSchedule sew ,SewingSchedule_detail sew_b,WorkOrder_Distribute h
				Where h.WorkOrderUkey = a.ukey and sew.id=sew_b.id and h.orderid = sew_b.OrderID 
						and h.Article = sew_b.Article and h.SizeCode = h.SizeCode and h.orderid = sew.orderid
			)  as Sewinline,
			(
				Select isnull(Min(cut.cdate),'')
				From cuttingoutput cut ,cuttingoutput_detail cut_b
				Where cut_b.workorderukey = a.Ukey and cut.id = cut_b.id
			)  as actcutdate,
            '' as NewestcutDate, '' as cutreasonid ,0 as sel
			from Workorder a";
            string where = string.Format(" Where cutplanid!='' and MDivisionId = '{0}'", keyWord);
            if (!MyUtility.Check.Empty(cutsp)) where = where + string.Format(" and id='{0}'",cutsp);
            if (!MyUtility.Check.Empty(sp)) where = where + string.Format(" and OrderID='{0}'", sp);
            if (!MyUtility.Check.Empty(seq)) where = where + string.Format(" and Seq1+SEQ2='{0}'", seq);
            if (!MyUtility.Check.Empty(dateBox_estcutdate.Value)) where = where + string.Format("and estcutdate='{0}'",estcutdate);
            if (!MyUtility.Check.Empty(cutref)) where = where + string.Format(" and cutref='{0}'", cutref);

            sql = sql + where + " ) as #tmp ";
            where = " Where 1=1 and actcutdate =''";
            if (!MyUtility.Check.Empty(dateBox_sewinginline.Value)) where = where + string.Format(" and Sewinline='{0}'", sewinginline);
            sql = sql + where;
            DualResult dResult = DBProxy.Current.Select(null, sql, out detailTb);
            if (detailTb == null || detailTb.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found!!");
                return ;
            }
            detailgrid.DataSource = gridbs;
            gridbs.DataSource = detailTb;
            this.detailgrid.AutoResizeColumns();
        }

        private void dateBox_newestcutdate_Validating(object sender, CancelEventArgs e)
        {
            if (dateBox_newestcutdate.Value != null && dateBox_newestcutdate.Value < DateTime.Now)
            {
                MyUtility.Msg.WarningBox("<Est Cut Date> can not early today.");
                dateBox_newestcutdate.Value = null;
            }
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            if (detailTb == null) return;
            string estcutdate = dateBox_newestcutdate.Text;
            string reason = txtcutReason1.TextBox1.Text;
            foreach (DataRow dr in detailTb.Rows)
            {
                if(dr["Sel"].ToString()=="1")
                {
                    dr["newestcutdate"] = estcutdate;
                    dr["cutreasonid"] = reason;
                }
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            string update = "";
            if (detailTb.Rows.Count == 0) return;
            foreach (DataRow dr in detailTb.Rows)
            {
                if (dr["EstCutDate"] != dr["NewEstCutDate"] && !MyUtility.Check.Empty(dr["NewEstCutDate"]))
                {
                    if (MyUtility.Check.Empty(dr["CutReasonid"]))
                    {
                        MyUtility.Msg.WarningBox("<Reason> can not be empty.");
                        return;
                    }
                    update = update + string.Format("Update Workorder Set estcutdate ='{0}' where Ukey = {1}; ", dr["newestcutdate"],dr["Ukey"]);
                    update = update + string.Format("Insert into Workorder_EstCutdate(WorkOrderUkey,orgEstCutDate,NewEstCutDate,CutReasonid,ID) Values({0},'{1}','{2}','{3}','{4}');", dr["Ukey"], Convert.ToDateTime(dr["EstCutDate"]).ToShortDateString(), dr["NewEstCutDate"], dr["CutReasonid"], dr["ID"]);
                }
            }
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
