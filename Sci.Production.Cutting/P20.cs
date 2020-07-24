using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P20 : Win.Tems.Input8
    {
        private readonly string LoginID = Env.User.UserID;
        private readonly string KeyWord = Env.User.Keyword;

        /// <inheritdoc/>
        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", this.KeyWord);
            this.DoSubForm = new P20_Detail();
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DualResult result;
            result = DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            if (!result)
            {
                this.ShowErr(result);
            }

            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
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
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
Select a.* , e.FabricCombo, e.FabricPanelCode, 
    (
        Select DISTINCT Orderid+'/' 
        From WorkOrder_Distribute d WITH (NOLOCK) 
        Where d.WorkOrderUkey =a.WorkOrderUkey and Orderid!='EXCESS'
        For XML path('')
    ) as OrderID,
    (
        --Select SizeCode+'/'+convert(varchar,Qty )
        Select SizeCode+'*'+convert(varchar,Qty ) + '/ '
        From CuttingOutput_Detail_Detail c WITH (NOLOCK) 
        Where c.CuttingOutput_DetailUkey =a.Ukey 
        For XML path('')
    ) as SizeRatio      
	, WorkOderLayer = e.Layer
	, AccuCuttingLayer = isnull(acc.AccuCuttingLayer,0)
    , LackingLayers = e.Layer -isnull(acc.AccuCuttingLayer,0)- a.layer
    , e.ConsPC,SRQ.SizeRatioQty
From cuttingoutput_Detail a WITH (NOLOCK)
left join WorkOrder e WITH (NOLOCK) on a.WorkOrderUkey = e.Ukey
outer apply(select AccuCuttingLayer = sum(aa.Layer) from cuttingoutput_Detail aa where aa.WorkOrderUkey = e.Ukey and id <> '{0}')acc
outer apply(select SizeRatioQty = sum(b.Qty) from WorkOrder_SizeRatio b where b.WorkOrderUkey = e.Ukey)SRQ
where a.id = '{0}' 
ORDER BY CutRef
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? string.Empty : e.Detail["Ukey"].ToString();

            this.SubDetailSelectCommand = string.Format(
            @"Select a.*
            from Cuttingoutput_Detail_Detail a WITH (NOLOCK) 
            where a.Cuttingoutput_DetailUkey = '{0}'", masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label9.Text = this.CurrentMaintain["Status"].ToString();

            // 去除表身[sizeRatio]最後一個/ 字元
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["sizeRatio"] = dr["sizeRatio"].ToString().Trim().TrimEnd('/');
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings sizeratio = new DataGridViewGeneratorTextColumnSettings();

            #region Cutref column
            DataGridViewGeneratorTextColumnSettings cutref = new DataGridViewGeneratorTextColumnSettings();
            cutref.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Cutref"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (newvalue == oldvalue || newvalue.Trim() == string.Empty)
                {
                    return;
                }

                string cutrefsql = $@"
Select
	a.ID
	,a.CutRef
	,a.FabricCombo
	,a.MarkerName
	,a.Cons
	,a.FabricPanelCode
	,a.cutno
	,a.MarkerLength
	,a.colorID
	,a.Ukey
	,Cuttingid = a.id
	,OrderID = stuff((
		Select orderid+'/' 
		From WorkOrder_Distribute c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey and orderid!='EXCESS'
		For XML path('')
    ),1,1,'')
	,SizeRatio = stuff((
        Select  '/ '+SizeCode+'*'+convert(varchar,Qty ) 
		From WorkOrder_SizeRatio c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
    ),1,1,'')
	,WorkOderLayer = a.Layer
	,AccuCuttingLayer = isnull(acc.AccuCuttingLayer,0)
	,CuttingLayer = case when cl.CuttingLayer > a.Layer - isnull(acc.AccuCuttingLayer,0) then a.Layer - isnull(acc.AccuCuttingLayer,0)
                    when cl.CuttingLayer < 0 then 0
                    else cl.CuttingLayer
                    end
	,LackingLayers = 0
	,a.ConsPC
	,SRQ.SizeRatioQty
	,a.Layer
from WorkOrder a WITH (NOLOCK)
outer apply(select AccuCuttingLayer = sum(b.Layer) from cuttingoutput_Detail b where b.WorkOrderUkey = a.Ukey and id <> '{this.CurrentMaintain["ID"]}')acc
outer apply(select SizeRatioQty = sum(b.Qty) from WorkOrder_SizeRatio b where b.WorkOrderUkey = a.Ukey)SRQ
outer apply(
	select Qty = min(x2.Qty) -- 正常狀況,在同裁次內 每個size計算出來要一樣, 取min只是個保險
	from(
		select Qty = CEILING(iif( ws.Qty = 0, 0, cast(min(x.qty)as float) / ws.Qty)) -- 正常狀況,在同裁次內 每個size計算出來要一樣, 取min只是個保險
		from(
			select Qty=SUM(bd.Qty),bd.SizeCode
			from Bundle b with(nolock)
			inner join Bundle_Detail bd with(nolock) on bd.Id = b.ID
			where b.CutRef = a.CutRef
			group by bd.SizeCode, bd.Patterncode, bd.PatternDesc
		)x
		left join WorkOrder_SizeRatio ws with(nolock) on x.SizeCode = ws.SizeCode and ws.WorkOrderUkey = a.Ukey
		group by x.SizeCode,ws.Qty
	)x2
)x3
outer apply(select CuttingLayer = isnull(x3.Qty,0)-isnull(acc.AccuCuttingLayer,0)) cl
where a.CutRef = '{e.FormattedValue}'
and a.CutRef != ''
--and a.Layer > isnull(acc.AccuCuttingLayer,0)
and a.MDivisionId = '{Env.User.Keyword}'
";
                DualResult result = DBProxy.Current.Select(null, cutrefsql, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dr["Orderid"] = string.Empty;
                dr["Cuttingid"] = string.Empty;
                dr["Cutref"] = string.Empty;
                dr["FabricCombo"] = string.Empty;
                dr["MarkerName"] = string.Empty;
                dr["WorkOderLayer"] = 0;
                dr["AccuCuttingLayer"] = 0;
                dr["Layer"] = 0;
                dr["LackingLayers"] = 0;
                dr["Cons"] = 0;
                dr["FabricPanelCode"] = string.Empty;
                dr["Workorderukey"] = 0;
                dr["SizeRatio"] = string.Empty;
                dr["ConsPC"] = 0;
                dr["SizeRatioQty"] = 0;
                dr.EndEdit();

                // 找不到資料
                if (dt.Rows.Count == 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("<Cut Ref> data not found.");
                    return;
                }
                else
                {
                    DataTable dtCamWork = new DataTable();
                    DataRow seldr;

                    var nowOutputRow = this.DetailDatas.Where(x => x["CutRef"].EqualString(e.FormattedValue)).GroupBy(x => x["CutRef"]).Select(x => new { OutputLayer = x.Sum(y => MyUtility.Convert.GetInt(y["Layer"])) });
                    var nowOutputRow_Layer = 0;
                    foreach (var nowOutputRowVal in nowOutputRow)
                    {
                        nowOutputRow_Layer += nowOutputRowVal.OutputLayer;
                    }

                    var delRow = this.GetDetailGridDatasByDeleted().FirstOrDefault(x => x["CutRef", DataRowVersion.Original].EqualString(e.FormattedValue));
                    int delRow_Layer = delRow == null ? 0 : MyUtility.Convert.GetInt(delRow["Layer", DataRowVersion.Original]);
                    var rows = dt.AsEnumerable().Where(x => MyUtility.Convert.GetInt(x["WorkOderLayer"]) > MyUtility.Convert.GetInt(x["AccuCuttingLayer"]) + nowOutputRow_Layer - delRow_Layer);
                    if (rows.Any())
                    {
                        dtCamWork = rows.CopyToDataTable();
                    }

                    if (dtCamWork.Rows.Count == 0)
                    {
                        // WorkOderLayer <= AccuCuttingLayer
                        cutrefsql =
                            $@"select [Cutting Output ID] = b.ID
                                        , b.Layer
                                    from WorkOrder a WITH (NOLOCK)
                                    inner join cuttingoutput_Detail b WITH (NOLOCK) on b.WorkOrderUkey = a.Ukey
                                    where a.CutRef =  '{e.FormattedValue}'
                                    and a.CutRef != ''
                                    and a.MDivisionId = '{Env.User.Keyword}'";
                        result = DBProxy.Current.Select(null, cutrefsql, out dt);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return;
                        }

                        e.Cancel = true;

                        var m = new Win.UI.MsgGridForm(
                            dt,
                            "this <Cut Ref#(" + e.FormattedValue + ")> already output finished \r\nPlease refer to the following information",
                            "Warning",
                            null,
                            MessageBoxButtons.OK);
                        m.grid1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        m.grid1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        m.Width = 630;
                        m.text_Find.Width = 140;
                        m.btn_Find.Location = new Point(150, 6);
                        m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        m.ShowDialog();

                        return;
                    }
                    else if (dtCamWork.Rows.Count > 1)
                    {
                        SelectItem sele;
                        sele = new SelectItem(dtCamWork, "cutref,Orderid,FabricCombo,WorkOderLayer,AccuCuttingLayer,CuttingLayer,LackingLayers,Cons", string.Empty, dr["cutref"].ToString(), false, ",");
                        DialogResult resultShowDialog = sele.ShowDialog();
                        if (resultShowDialog == DialogResult.Cancel)
                        {
                            return;
                        }

                        e.FormattedValue = sele.GetSelectedString();
                        seldr = sele.GetSelecteds()[0];
                        if (((DataTable)this.detailgridbs.DataSource).Select($"Workorderukey = '{seldr["Ukey"]}'").Length > 0)
                        {
                            MyUtility.Msg.WarningBox($"CutRefno：{seldr["cutref"]}, WorkOrderUkey：{seldr["Ukey"]} can't duplicate");
                            dr["cutref"] = DBNull.Value;
                            return;
                        }
                    }
                    else
                    {
                        seldr = dtCamWork.Rows[0];
                    }

                    dr["Orderid"] = seldr["Orderid"];
                    dr["Cuttingid"] = seldr["ID"];
                    dr["Cutref"] = seldr["cutref"];
                    dr["FabricCombo"] = seldr["FabricCombo"];
                    dr["MarkerName"] = seldr["MarkerName"];
                    dr["WorkOderLayer"] = seldr["WorkOderLayer"];
                    dr["AccuCuttingLayer"] = seldr["AccuCuttingLayer"];
                    dr["Layer"] = MyUtility.Convert.GetInt(seldr["CuttingLayer"]) - nowOutputRow_Layer;
                    dr["LackingLayers"] = MyUtility.Convert.GetInt(seldr["WorkOderLayer"]) - MyUtility.Convert.GetInt(seldr["AccuCuttingLayer"]) - (MyUtility.Convert.GetInt(dr["Layer"]) + nowOutputRow_Layer);
                    dr["Cons"] = seldr["Cons"];
                    dr["FabricPanelCode"] = seldr["FabricPanelCode"];
                    dr["cutno"] = seldr["cutno"];
                    dr["MarkerLength"] = seldr["MarkerLength"];
                    dr["colorID"] = seldr["colorID"];
                    dr["Workorderukey"] = seldr["Ukey"];
                    dr["SizeRatio"] = seldr["SizeRatio"];
                    dr["ConsPC"] = seldr["ConsPC"];
                    dr["SizeRatioQty"] = seldr["SizeRatioQty"];
                    dr.EndEdit();
                }
            };
            #endregion
            #region SizeRatio Dobule Click entery next form
            sizeratio.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && !this.EditMode)
                {
                    this.OpenSubDetailPage();
                }
            };
            #endregion
            #region Cutting Layer
            DataGridViewGeneratorNumericColumnSettings layer = new DataGridViewGeneratorNumericColumnSettings();
            layer.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                dr["Layer"] = e.FormattedValue;
                dr["Cons"] = MyUtility.Convert.GetDecimal(e.FormattedValue) * MyUtility.Convert.GetDecimal(dr["ConsPC"]) * MyUtility.Convert.GetDecimal(dr["SizeRatioQty"]);
                dr["LackingLayers"] = MyUtility.Convert.GetInt(dr["WorkOderLayer"]) - MyUtility.Convert.GetInt(dr["AccuCuttingLayer"]) - MyUtility.Convert.GetInt(dr["Layer"]);

                dr.EndEdit();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Cutref", header: "Cut Ref#", width: Widths.AnsiChars(6), settings: cutref)
            .Text("Cuttingid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerLength", header: "Marker Length", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("WorkOderLayer", header: "WorkOrder\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Numeric("AccuCuttingLayer", header: "Accu. Cutting\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Numeric("Layer", header: "Cutting\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, maximum: 99999, minimum: 0, settings: layer)
            .Numeric("LackingLayers", header: "Lacking\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 7, decimal_places: 2, iseditingreadonly: true)
            .Text("sizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: sizeratio);
            this.detailgrid.Columns["Cutref"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Layer"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["cDate"] = DateTime.Today.AddDays(-1);
            this.CurrentMaintain["mDivisionid"] = this.KeyWord;
            this.CurrentMaintain["Status"] = "New";
            this.txtfactoryByM1.Text = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("<Status> is not New, you can not modify.");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("<Status> is not New, you can not delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value))
            {
                MyUtility.Msg.WarningBox("<Date> can not be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.numManPower.Text))
            {
                MyUtility.Msg.WarningBox("<Man Power> can not be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtfactoryByM1.Text))
            {
                MyUtility.Msg.WarningBox("<Factory> can not be empty.");
                return false;
            }

            // 檢查表身不能裁0層
            DataRow[] chkzero = ((DataTable)this.detailgridbs.DataSource).Select("Layer = '0'");
            if (chkzero.Length > 0)
            {
                List<string> chkZeroItemList = new List<string>();
                foreach (DataRow item in chkzero)
                {
                    chkZeroItemList.Add(MyUtility.Convert.GetString(item["Cutref"]));
                }

                string chkZeroItemMsg = $"Cutref: [{string.Join(",", chkZeroItemList)}] layers can not be zero.";
                MyUtility.Msg.WarningBox(chkZeroItemMsg);
                return false;
            }

            if (this.IsDetailInserting)
            {
                string date = this.dateDate.Text;
                string sql = string.Format("Select * from Cuttingoutput WITH (NOLOCK) Where cdate = '{0}' and id !='{1}' and FactoryID ='{2}'", date, this.CurrentMaintain["ID"], this.txtfactoryByM1.Text);
                if (MyUtility.Check.Seek(sql, null))
                {
                    MyUtility.Msg.WarningBox("The <Date> had been existed already.");

                    // CurrentMaintain["cDate"] = "";
                    return false;
                }
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DataView dv = ((DataTable)this.detailgridbs.DataSource).DefaultView;
            if (dv.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can not be empty.");
                return false;
            }

            MyUtility.Tool.ProcessWithDatatable(dt, "Layer,cutref,workorderukey,cuttingid,id", "Select sum(Layer) as tlayer,cutref,workorderukey,cuttingid,id from #tmp group by cutref,workorderukey,cuttingid,id", out DataTable sumTb);
            string cutref, id, cuttingid, sumsql;
            int layer_cutting, layer_work;
            foreach (DataRow dr in sumTb.Rows)
            {
                cutref = dr["Cutref"].ToString();
                id = dr["id"].ToString();
                cuttingid = dr["cuttingid"].ToString();
                sumsql = string.Format("Select sum(Layer) as tlayer from Workorder WITH (NOLOCK) where id='{0}' and cutref = '{1}'", cuttingid, cutref);
                string str = MyUtility.GetValue.Lookup(sumsql, null);
                if (MyUtility.Check.Empty(str))
                {
                    layer_work = 0;
                }
                else
                {
                    layer_work = Convert.ToInt32(str);
                }

                sumsql = string.Format("Select sum(Layer) as cuttingL from Cuttingoutput_Detail WITH (NOLOCK) where id!='{0}' and cutref = '{1}' and cuttingid= '{2}'", id, cutref, cuttingid);
                str = MyUtility.GetValue.Lookup(sumsql, null);
                if (MyUtility.Check.Empty(str))
                {
                    layer_cutting = 0;
                }
                else
                {
                    layer_cutting = Convert.ToInt32(str);
                }

                layer_cutting += Convert.ToInt32(dr["tlayer"]);
                if (layer_cutting > layer_work)
                {
                    MyUtility.Msg.WarningBox("<Refno>:" + cutref + ", the total layer cant not excess the total layer in workorder");
                    return false;
                }
            }

            decimal cons = Convert.ToDecimal(((DataTable)this.detailgridbs.DataSource).Compute("Sum(cons)", string.Empty));
            this.CurrentMaintain["Actoutput"] = cons;
            if (this.IsDetailInserting)
            {
                string cid = MyUtility.GetValue.GetID(this.KeyWord + "CD", "CuttingOutput");
                this.CurrentMaintain["id"] = cid;
            }
            #region 掃表身找出有改變的寫入第三層
            DataTable detailTb = (DataTable)this.detailgridbs.DataSource;
            MyUtility.Tool.ProcessWithDatatable(detailTb, "WorkorderUkey", "Select b.* from #tmp a, workorder_SizeRatio b WITH (NOLOCK) where a.workorderukey = b.workorderukey", out DataTable sizeTb);
            DataRow[] dray;
            foreach (DataRow dr in this.DetailDatas)
            {
                this.GetSubDetailDatas(dr, out DataTable subtb);

                // 變更需刪除第三層資料
                if (dr.RowState == DataRowState.Modified)
                {
                    foreach (DataRow sdr in subtb.Rows)
                    {
                        sdr.Delete();
                    }
                }

                // 新增與變更需增加第三層
                if ((dr.RowState == DataRowState.Added) || (dr.RowState == DataRowState.Modified))
                {
                    dray = sizeTb.Select(string.Format("WorkorderUkey ='{0}'", dr["WorkorderUkey"]));
                    if (dray.Length != 0)
                    {
                        foreach (DataRow dr2 in dray)
                        {
                            DataRow ndr = subtb.NewRow();
                            ndr["CuttingID"] = dr2["id"];
                            ndr["SizeCode"] = dr2["SizeCode"];
                            ndr["Qty"] = dr2["Qty"];
                            ndr["ID"] = this.CurrentMaintain["ID"];
                            subtb.Rows.Add(ndr);
                        }
                    }
                }
            }
            #endregion
            int cutrefCount = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => MyUtility.Convert.
   GetString(s["Cutref"])).Distinct().Count();
            this.CurrentMaintain["ActCutRefQty"] = cutrefCount;
            this.CurrentMaintain.EndEdit();
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string cdate = ((DateTime)this.CurrentMaintain["cdate"]).ToString("yyyy/MM/dd");
            #region 若前面的單子有尚未Confrim 則不可Confirm
            string sqlcmd = $"Select * from Cuttingoutput WITH (NOLOCK) where cdate<'{cdate}' and Status='New' and mDivisionid = '{this.KeyWord}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (!this.CheckMsg(dt, "The record not yet confirm, you can not confirm.Please see below list."))
            {
                return;
            }

            #endregion

            if (!this.CheckAfterCdate("confirm"))
            {
                return;
            }

            string update = $@"update Cuttingoutput set status='Confirmed',editDate=getdate(),editname ='{this.LoginID}' where id='{this.CurrentMaintain["ID"]}';
                        EXEC Cutting_P20_CFM_Update '{this.CurrentMaintain["ID"]}','{((DateTime)this.CurrentMaintain["cdate"]).ToString("yyyy/MM/dd")}',{this.CurrentMaintain["ManPower"]},{this.CurrentMaintain["ManHours"]},'Confirm';
";

            #region transaction
            DualResult upResult;
            TransactionOptions oTranOpt = default;
            oTranOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            oTranOpt.Timeout = new TimeSpan(0, 5, 0);
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.RequiresNew, oTranOpt))
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, update)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(upResult);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }

                transactionscope.Complete();
            }

            MyUtility.Msg.InfoBox("Successfully");
            #endregion
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!this.CheckAfterCdate("Unconfirm"))
            {
                return;
            }

            string update = $@"update Cuttingoutput set status='New',editDate=getdate(),editname ='{this.LoginID}' where id='{this.CurrentMaintain["ID"]}';
                        EXEC Cutting_P20_CFM_Update '{this.CurrentMaintain["ID"]}','{((DateTime)this.CurrentMaintain["cdate"]).ToString("yyyy/MM/dd")}',{this.CurrentMaintain["ManPower"]},{this.CurrentMaintain["ManHours"]},'UnConfirm';
";

            #region transaction
            DualResult upResult;
            TransactionOptions oTranOpt = default;
            oTranOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            oTranOpt.Timeout = new TimeSpan(0, 5, 0);
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.RequiresNew, oTranOpt))
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, update)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(upResult);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }

                transactionscope.Complete();
            }

            MyUtility.Msg.InfoBox("Successfully");
            #endregion
        }

        private bool CheckAfterCdate(string c)
        {
            // 該日期之後的單據是否有狀態不在 New 的資料
            string cdate = ((DateTime)this.CurrentMaintain["cdate"]).ToString("yyyy/MM/dd");
            string sqlcmd = $@"Select * from Cuttingoutput WITH (NOLOCK) where cdate>'{cdate}' and Status<>'New' and mDivisionid = '{this.KeyWord}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (!this.CheckMsg(dt, $"The record not yet Unconfirm, you can not {c}.Please see below list."))
            {
                return false;
            }

            return true;
        }

        private bool CheckMsg(DataTable dt, string headermag)
        {
            string msg = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                msg = msg + "<ID>:" + dr["ID"] + ",<Date>:" + ((DateTime)dr["cdate"]).ToString("yyyy/MM/dd") + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(msg))
            {
                MyUtility.Msg.WarningBox(headermag + Environment.NewLine + msg);
                return false;
            }

            return true;
        }

        private void BtnImportfromWorkOrder_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            var frm = new P20_Import_Workorder(this.CurrentMaintain, dt);
            frm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        private void BtnImportfromRealtimeBundleTracking_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            var frm = new P20_Import_RFID(this.CurrentMaintain, dt);
            frm.ShowDialog(this);
        }

        private int index;
        private string find = string.Empty;
        private DataRow[] find_dr;

        private void BtnFind_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)this.detailgridbs.DataSource;

            // 移到指定那筆
            string cutref = this.txtCutref.Text;
            string find_new = string.Empty;

            if (!MyUtility.Check.Empty(cutref))
            {
                find_new = string.Format("Cutref='{0}'", cutref);
            }

            if (this.find != find_new)
            {
                this.find = find_new;
                this.find_dr = detDtb.Select(find_new);
                if (this.find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Data not Found.");
                    return;
                }
                else
                {
                    this.index = 0;
                }
            }
            else
            {
                this.index++;
                if (this.find_dr == null)
                {
                    return;
                }

                if (this.index >= this.find_dr.Length)
                {
                    this.index = 0;
                }
            }

            this.detailgridbs.Position = this.DetailDatas.IndexOf(this.find_dr[this.index]);
        }

        private void BtnCopyBal_Click(object sender, EventArgs e)
        {
            foreach (DataRow item in this.DetailDatas.AsEnumerable().Where(w => MyUtility.Check.Empty(w["Layer"])))
            {
                item["Layer"] = MyUtility.Convert.GetInt(item["WorkOderLayer"]) - MyUtility.Convert.GetInt(item["AccuCuttingLayer"]);
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            foreach (DataRow item in ((DataTable)this.detailgridbs.DataSource).Select("Layer = 0"))
            {
                item.Delete();
            }
        }
    }
}
