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

        Ict.Win.UI.DataGridViewTextBoxColumn col_SpreadingNo;
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
            .Text("SpreadingNoID", header: "Org.\nSpreading No", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("NewSpreadingNoID", header: "New\nSpreading No", width: Widths.AnsiChars(2)).Get(out col_SpreadingNo)
            .Text("Cutcellid", header: "Org.\nCut Cell", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("NewCutcellid", header: "New\nCut Cell", width: Widths.AnsiChars(2)).Get(out col_cutcell)
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
            this.gridDetail.Columns["NewSpreadingNoID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDetail.Columns["NewCutcellid"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            col_estcutdate.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                DataRow dr = gridDetail.GetDataRow(e.RowIndex);
                if ((DateTime)e.FormattedValue < DateTime.Today)
                {
                    MyUtility.Msg.WarningBox("<Est Cut Date> can not early today.");
                    dr["newestcutdate"] = DBNull.Value;
                    dr.EndEdit();
                }
                else
                {
                    dr["newestcutdate"] = (DateTime)e.FormattedValue;
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
            #region col_SpreadingNo
            col_SpreadingNo.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    DataRow dr = gridDetail.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    DataTable SpreadingNoTb;
                    DBProxy.Current.Select(null, $"Select id from SpreadingNo WITH (NOLOCK) where mDivisionid = '{keyWord}' and junk=0", out SpreadingNoTb);
                    sele = new SelectItem(SpreadingNoTb, "ID", "10@400,300", dr["NewSpreadingNoID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["NewSpreadingNoID"] = sele.GetSelectedString();
                    dr.EndEdit();
                }
            };
            col_SpreadingNo.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                if (e.RowIndex == -1) return;
                DataRow dr = gridDetail.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty()) return;
                string oldvalue = dr["NewSpreadingNoID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                DataRow SpreadingNodr;
                string sqlSpreading = $"Select 1 from SpreadingNo WITH (NOLOCK) where mDivisionid = '{keyWord}' and  id = '{newvalue}' and junk=0";
                if (!MyUtility.Check.Seek(sqlSpreading, out SpreadingNodr))
                {
                    dr["NewSpreadingNoID"] = "";
                    dr.EndEdit();
                    MyUtility.Msg.WarningBox(string.Format("<SpreadingNo> : {0} data not found!", newvalue));
                    return;
                }

                dr["NewSpreadingNoID"] = newvalue;
                
                dr.EndEdit();
            };
            #endregion
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
                    dr["NewCutcellid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    return;
                }

                dr["NewCutcellid"] = newvalue;
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
            string cutplanID = txtCutplanID.Text;
            if (MyUtility.Check.Empty(cutsp) && MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(dateEstCutDate.Value) && MyUtility.Check.Empty(cutplanID))
            {
                MyUtility.Msg.WarningBox("You must be entry conditions <Cutting SP#> or <SP#> or <Est. Cut Date> or <CutplanID>");
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
        NewestcutDate = cast(null as date) , '' as cutreasonid ,0 as sel
        ,NewCutcellid=''
        ,NewSpreadingNoID=''
    from Workorder a";
            string where = string.Format(" Where cutplanid!='' and MDivisionId = '{0}'", keyWord);
            if (!MyUtility.Check.Empty(cutsp)) where = where + string.Format(" and id='{0}'",cutsp);
            if (!MyUtility.Check.Empty(sp)) where = where + string.Format(" and OrderID='{0}'", sp);
            if (!MyUtility.Check.Empty(seq)) where = where + string.Format(" and Seq1+SEQ2='{0}'", seq);
            if (!MyUtility.Check.Empty(dateEstCutDate.Value)) where = where + string.Format("and estcutdate='{0}'",estcutdate);
            if (!MyUtility.Check.Empty(cutref)) where = where + string.Format(" and cutref='{0}'", cutref);
            if (!MyUtility.Check.Empty(txtfactoryByM.Text)) where = where + string.Format(" and a.Factoryid='{0}'", txtfactoryByM.Text);
            if (!MyUtility.Check.Empty(cutplanID)) where = where + string.Format(" and a.CutplanID='{0}'", cutplanID);

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
            if (dateNewEstCutDate.Value != null && dateNewEstCutDate.Value < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("<Est Cut Date> can not early today.");
                dateNewEstCutDate.Value = null;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            gridDetail.ValidateControl();
            if (detailTb == null) return;
            string reason = txtcutReason.TextBox1.Text;
            string SpreadingNo = txtSpreadingNo1.Text;
            string cell = txtCell1.Text;
            foreach (DataRow dr in detailTb.Rows)
            {
                if(dr["Sel"].ToString()=="1")
                {
                    if (MyUtility.Check.Empty(dateNewEstCutDate.Value))
                    {
                        dr["newestcutdate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["newestcutdate"] = ((DateTime)dateNewEstCutDate.Value).ToShortDateString();
                    }
                    dr["cutreasonid"] = reason;
                    dr["NewSpreadingNoID"] = SpreadingNo;
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
                if ((dr["EstCutDate"] != dr["NewEstCutDate"] && !MyUtility.Check.Empty((dr["NewEstCutDate"].ToString().Replace("/", "")))) || 
                    (dr["Cutcellid"] != dr["NewCutcellid"] && !MyUtility.Check.Empty(dr["NewCutcellid"])) ||
                    (dr["SpreadingNoID"] != dr["NewSpreadingNoID"] && !MyUtility.Check.Empty(dr["NewSpreadingNoID"]))
                    )
                {
                    if (MyUtility.Check.Empty(dr["CutReasonid"]))
                    {
                        MyUtility.Msg.WarningBox("<Reason> can not be empty.");
                        return;
                    }

                    string newestcutdate = MyUtility.Check.Empty(dr["newestcutdate"]) ? "Null" : $"'{((DateTime)dr["newestcutdate"]).ToShortDateString()}'";
                    string NewCutcellid = MyUtility.Check.Empty(dr["NewCutcellid"]) ? "Null" : $"'{dr["NewCutcellid"]}'";
                    string NewSpreadingNoID = MyUtility.Check.Empty(dr["NewSpreadingNoID"]) ? "Null" : $"'{dr["NewSpreadingNoID"]}'";
                    string orgEstCutDate =((DateTime)dr["EstCutDate"]).ToShortDateString();
                    if (!MyUtility.Check.Empty(dr["newestcutdate"]))
                    {
                        update = update + $"Update Workorder Set estcutdate ='{((DateTime)dr["newestcutdate"]).ToShortDateString()}' where Ukey = {dr["Ukey"]}; ";
                    }
                    if (!MyUtility.Check.Empty(dr["NewCutcellid"]))
                    {
                        update = update + $"Update Workorder Set CutCellid = '{dr["NewCutcellid"]}' where Ukey = {dr["Ukey"]}; ";
                    }
                    if (!MyUtility.Check.Empty(dr["NewSpreadingNoID"]))
                    {
                        update = update + $"Update Workorder Set SpreadingNoID='{dr["NewSpreadingNoID"]}' where Ukey = {dr["Ukey"]}; ";
                    }

                    update = update + $@"
Insert into Workorder_EstCutdate(WorkOrderUkey,orgEstCutDate      ,NewEstCutDate  ,CutReasonid              ,ID             ,OrgCutCellid       ,NewCutCellid   ,OrgSpreadingNoID         ,NewSpreadingNoID) 
Values                                       ({dr["Ukey"]}    ,'{orgEstCutDate}',{newestcutdate},'{dr["CutReasonid"]}','{dr["ID"]}','{dr["Cutcellid"]}',{NewCutcellid},'{dr["SpreadingNoID"]}',{NewSpreadingNoID});";
                }
            }

            var distnct_id = detailTb.AsEnumerable().
                Where(w => w.Field<int>("Sel") == 1 && w.Field<object>("EstCutDate") != w.Field<object>("NewEstCutDate") && !MyUtility.Check.Empty(w.Field < object >("NewEstCutDate"))).
                Select(row => row.Field<string>("ID")).Distinct();
            foreach (string tmp_id in distnct_id)
            {
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

            #region 檢查相同M、CutRef#，spreading No、Cut Cell、Est.CutDate, 更新必須都一樣
            var distnct_List = detailTb.AsEnumerable().
                Select(m => new
                {
                    MDivisionId = m.Field<string>("MDivisionId"),                    
                    CutRef = m.Field<string>("CutRef")
                }).Distinct();
            string inUkey = "'" + string.Join("','", detailTb.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ukey"]))) + "'";

            foreach (var item in distnct_List)
            {
                // 檢查已撈出資料
                DataRow[] chkdrs = detailTb.Select($@" MDivisionId = '{item.MDivisionId}' and CutRef = '{item.CutRef}'");

                if (chkdrs.Length > 1)
                {
                    var chksame = chkdrs.Select(m => new
                    {
                        NewSpreadingNoID = m.Field<string>("NewSpreadingNoID"),
                        NewCutcellid = m.Field<string>("NewCutcellid"),
                        NewEstcutdate = m.Field<DateTime?>("NewEstcutdate"),
                        CutReasonid = m.Field<string>("CutReasonid")
                    }).Distinct().ToList();
                    // 更新的欄位不能合併表示不一樣
                    if (chksame.Count> 1)
                    {
                        MyUtility.Msg.WarningBox("You can't set different [Est.CutDate] or [CutCell] or [Spreading No.] or [reason] in same M and CutRef#");
                        return;
                    }
                }

                // 檢查DB(不包含撈出資料)有沒有同裁次, 若有則要檢查此次更新的欄位是否與DB內相同
                string csqlhk = $@"select MDivisionId,CutRef,SpreadingNoID,Cutcellid,estcutdate
from workOrder w with(nolock) 
where ukey not in ({inUkey})
and MDivisionId = '{item.MDivisionId}'
and CutRef = '{item.CutRef}'
";
                DataTable chkdt;
                DualResult result;
                result = DBProxy.Current.Select(null, csqlhk, out chkdt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                foreach (DataRow dr in chkdt.Rows)
                {
                    bool isNotsame = false;
                    if (!MyUtility.Check.Empty(chkdrs[0]["NewEstcutdate"]) &&
                        MyUtility.Convert.GetDate(dr["estcutdate"]) != MyUtility.Convert.GetDate(chkdrs[0]["NewEstcutdate"]))
                    {
                        isNotsame = true;
                    }
                    if (!MyUtility.Check.Empty(chkdrs[0]["NewCutcellid"]) &&
                        !MyUtility.Convert.GetString(dr["Cutcellid"]).EqualString(MyUtility.Convert.GetString(chkdrs[0]["NewCutcellid"])))
                    {
                        isNotsame = true;
                    }
                    if (!MyUtility.Check.Empty(chkdrs[0]["NewSpreadingNoID"]) &&
                        !MyUtility.Convert.GetString(dr["SpreadingNoID"]).EqualString(MyUtility.Convert.GetString(chkdrs[0]["NewSpreadingNoID"])))
                    {
                        isNotsame = true;
                    }
                    if (isNotsame)
                    {
                        MyUtility.Msg.WarningBox("You can't set different [Est.CutDate] or [CutCell] or [Spreading No.] or [reason] in same M and CutRef#");
                        return;
                    }
                }
            }
            #endregion

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
