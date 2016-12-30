﻿using Ict;
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


namespace Sci.Production.Cutting
{
    public partial class P20 : Sci.Win.Tems.Input8
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", keyWord);
            DoSubForm = new P20_Detail();
        }
        protected override Ict.DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.* , e.FabricCombo, e.LectraCode, 
            (
                Select DISTINCT Orderid+'/' 
                From WorkOrder_Distribute d
                Where d.WorkOrderUkey =a.WorkOrderUkey and Orderid!='EXCESS'
                For XML path('')
            ) as OrderID,
            (
                --Select SizeCode+'/'+convert(varchar,Qty )
                Select SizeCode+'*'+convert(varchar,Qty ) + '/ '
                From CuttingOutput_Detail_Detail c
                Where c.CuttingOutput_DetailUkey =a.Ukey 
                For XML path('')
            ) as SizeRatio      
            From cuttingoutput_Detail a, WorkOrder e
            where a.id = '{0}' and a.WorkOrderUkey = e.Ukey
            ORDER BY CutRef
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override DualResult OnSubDetailSelectCommandPrepare(Win.Tems.Input8.PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "" : e.Detail["Ukey"].ToString();

            this.SubDetailSelectCommand = string.Format(
            @"Select a.*
            from Cuttingoutput_Detail_Detail a
            where a.Cuttingoutput_DetailUkey = '{0}'", masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        } 
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            label9.Text = CurrentMaintain["Status"].ToString();

            //去除表身[sizeRatio]最後一個/ 字元
            foreach (DataRow dr in DetailDatas) dr["sizeRatio"] = dr["sizeRatio"].ToString().Trim().TrimEnd('/');

        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings sizeratio = new DataGridViewGeneratorTextColumnSettings();

            #region Cutref column
            DataGridViewGeneratorTextColumnSettings cutref = new DataGridViewGeneratorTextColumnSettings();
            cutref.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if(e.RowIndex==-1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue =  dr["Cutref"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (newvalue == oldvalue || newvalue.Trim()=="") return;
                DataTable dt;
                if (DBProxy.Current.Select(null, string.Format(@"Select * from WorkOrder a where a.cutref = '{0}'", e.FormattedValue.ToString()), out dt))
                {
                    if (dt.Rows.Count==0)
                    {
                        MyUtility.Msg.WarningBox("<Cut Ref> data not found.");
                        dr["Orderid"] = "";
                        dr["Cuttingid"] = "";
                        dr["Cutref"] = "";
                        dr["FabricCombo"] = "";
                        dr["MarkerName"] = "";
                        dr["Layer"] =0;
                        dr["Cons"] = 0;
                        dr["LectraCode"] = "";
                        dr["Workorderukey"] = 0;
                        dr["SizeRatio"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
                DataRow seldr;
                if (dt.Rows.Count > 1)
                {
                    SelectItem sele;

                    sele = new SelectItem(dt, "cutref,Colorid,fabriccombo,Cutno,MarkerName,Layer,ukey", "8,2,2,5,5,10", dr["cutref"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.FormattedValue = sele.GetSelectedString();
                    seldr = sele.GetSelecteds()[0];
                }
                else
                {
                    seldr = dt.Rows[0];
                }
                dr["Orderid"] = "";
                dr["Cuttingid"] = seldr["ID"];
                dr["Cutref"] = seldr["cutref"];
                dr["FabricCombo"] = seldr["FabricCombo"];
                dr["MarkerName"] = seldr["MarkerName"];
                dr["Layer"] = seldr["Layer"];
                dr["Cons"] = seldr["Cons"];
                dr["LectraCode"] = seldr["LectraCode"];
                dr["cutno"] = seldr["cutno"];
                dr["MarkerLength"] = seldr["MarkerLength"];
                dr["colorID"] = seldr["colorID"];
                dr["Workorderukey"] = seldr["Ukey"];
                dr["SizeRatio"] = "";
                DataTable workorderTb;
                string str = "";
                if (DBProxy.Current.Select(null, string.Format("Select * from Workorder_SizeRatio a where a.workorderukey = '{0}'", dr["Workorderukey"]), out workorderTb))
                {
                    if (workorderTb.Rows.Count != 0)
                    {
                        foreach (DataRow ddr in workorderTb.Rows)
                        {
                            if (str != "")
                            {
                                str = str + "/ " + ddr["SizeCode"].ToString() + "*" + ddr["Qty"].ToString();
                            }
                            else
                            {
                                str = ddr["SizeCode"].ToString() + "*" + ddr["Qty"].ToString();
                            }
                        }
                    }
                    dr["sizeRatio"] = str;
                }
                str = "";
                if (DBProxy.Current.Select(null, string.Format("Select * from Workorder_distribute a where a.workorderukey = '{0}'", dr["Workorderukey"]), out workorderTb))
                {
                    if (workorderTb.Rows.Count!=0)
                    {
                        foreach (DataRow ddr in workorderTb.Rows)
                        {
                            if (ddr["Orderid"].ToString()!="EXCESS")
                            {
                                if (str != "")
                                {
                                    str = str + "/" + ddr["Orderid"].ToString();
                                }
                                else
                                {
                                    str = ddr["Orderid"].ToString();
                                }
                            }
                        }
                    }
                    dr["Orderid"] = str;
                }
                dr.EndEdit();     
            };
            #endregion
            #region SizeRatio Dobule Click entery next form
            sizeratio.CellMouseDoubleClick += (s, e) =>
            {

                if (e.Button == System.Windows.Forms.MouseButtons.Left && !this.EditMode)
                {
                    OpenSubDetailPage();
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
             .Text("Cutref", header: "Cut Ref#", width: Widths.AnsiChars(6), settings: cutref)
            .Text("Cuttingid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("LectraCode", header: "Lectra code", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerLength", header: "Marker Length", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 7, decimal_places: 2, iseditingreadonly: true)
            .Text("sizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: sizeratio);
            this.detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["cDate"] = DateTime.Today.AddDays(-1);
            CurrentMaintain["mDivisionid"] = keyWord;
            CurrentMaintain["Status"] = "New";

        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("<Status> is not New, you can not modify.");
                return false;
            }
            return base.ClickEditBefore();
        }
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("<Status> is not New, you can not delete.");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(dateBox1.Value))
            {
                MyUtility.Msg.WarningBox("<Date> can not be empty.");
                return false;
            }
            if (MyUtility.Check.Empty(numericBox2.Text))
            {
                MyUtility.Msg.WarningBox("<Man Power> can not be empty.");
                return false;
            }
            if (this.IsDetailInserting)
            {
                string date = dateBox1.Text;
                string sql = string.Format("Select * from Cuttingoutput Where cdate = '{0}' and id !='{1}'", date,CurrentMaintain["ID"]);
                if (MyUtility.Check.Seek(sql, null))
                {
                    MyUtility.Msg.WarningBox("The <Date> had been existed already.");
                    //CurrentMaintain["cDate"] = "";
                    return false;
                }
            }
            DataTable dt= ((DataTable)detailgridbs.DataSource);
            
            DataView dv= ((DataTable)detailgridbs.DataSource).DefaultView;
            if (dv.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can not be empty.");
                return false;
            }
            DataTable sumTb;

            MyUtility.Tool.ProcessWithDatatable(dt, "Layer,cutref,workorderukey,cuttingid,id", "Select sum(Layer) as tlayer,cutref,workorderukey,cuttingid,id from #tmp group by cutref,workorderukey,cuttingid,id", out sumTb);
            string cutref, id, cuttingid,sumsql;
            int layer_cutting, layer_work;
            foreach (DataRow dr in sumTb.Rows)
            {
                cutref = dr["Cutref"].ToString();
                id = dr["id"].ToString();
                cuttingid = dr["cuttingid"].ToString();
                sumsql = string.Format("Select sum(Layer) as tlayer from Workorder where id='{0}' and cutref = '{1}'", cuttingid, cutref);
                string str = MyUtility.GetValue.Lookup(sumsql, null);
                if (MyUtility.Check.Empty(str)) layer_work = 0;
                else layer_work = Convert.ToInt32(str);

                sumsql = string.Format("Select sum(Layer) as cuttingL from Cuttingoutput_Detail where id!='{0}' and cutref = '{1}' and cuttingid= '{2}'", id, cutref,cuttingid);
                str = MyUtility.GetValue.Lookup(sumsql, null);
                if (MyUtility.Check.Empty(str)) layer_cutting = 0;
                else layer_cutting = Convert.ToInt32(str);

                layer_cutting = layer_cutting + Convert.ToInt32(dr["tlayer"]);
                if (layer_cutting > layer_work)
                {
                    MyUtility.Msg.WarningBox("<Refno>:" + cutref + ", the total layer cant not excess the total layer in workorder");
                    return false;
                }
            }
            decimal cons =Convert.ToDecimal(((DataTable)detailgridbs.DataSource).Compute("Sum(cons)",""));
            CurrentMaintain["Actoutput"] = cons;
            if (IsDetailInserting)
            {
                string cid = MyUtility.GetValue.GetID(keyWord + "CD", "CuttingOutput");
                CurrentMaintain["id"] = cid;
            }
            #region 掃表身找出有改變的寫入第三層
            DataTable detailTb = ((DataTable)detailgridbs.DataSource);
            DataTable subtb, SizeTb;
            MyUtility.Tool.ProcessWithDatatable(detailTb, "WorkorderUkey", "Select b.* from #tmp a, workorder_SizeRatio b where a.workorderukey = b.workorderukey", out SizeTb);
            DataRow[] dray;
            foreach (DataRow dr in DetailDatas)
            {
                GetSubDetailDatas(dr, out subtb);
                //變更需刪除第三層資料
                if ((dr.RowState == DataRowState.Modified))
                {
                    foreach (DataRow sdr in subtb.Rows)
                    {
                        sdr.Delete();
                    }
                }
                //新增與變更需增加第三層
                if ((dr.RowState == DataRowState.Added) || (dr.RowState == DataRowState.Modified))
                {
                    dray = SizeTb.Select(string.Format("WorkorderUkey ='{0}'", dr["WorkorderUkey"]));
                    if (dray.Length != 0)
                    {
                        foreach(DataRow dr2 in dray)
                        {
                            DataRow ndr = subtb.NewRow();
                            ndr["CuttingID"] = dr2["id"];
                            ndr["SizeCode"] = dr2["SizeCode"];
                            ndr["Qty"] = dr2["Qty"];
                            ndr["ID"] = CurrentMaintain["ID"];
                            subtb.Rows.Add(ndr);
                        }
                    }
                }
            }
            #endregion

            return base.ClickSaveBefore();
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            #region 若前面的單子有尚未Confrim 則不可Confirm
            
            string sql = string.Format("Select * from Cuttingoutput where cdate<'{0}' and Status='New' and mDivisionid = '{1}'",CurrentMaintain["cdate"],keyWord);
            string msg = "";
            DataTable Dt;
            if (DBProxy.Current.Select(null,sql, out Dt))
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    msg = msg + "<Date>:" + dr["cdate"].ToString()+"\\n";
                }
                if(MyUtility.Check.Empty(msg))
                {
                    MyUtility.Msg.WarningBox("The record not yet confirm, you can not confirm.Please see below list.\\n"+msg);
                    return;
                }
            }
            #endregion
            #region GMT SQL
            string sql1 = string.Format(@"
            Select distinct d.*,e.Colorid,e.PatternPanel
            into #tmp1
            from 
            (Select b.POID,c.ID,c.Article,c.SizeCode,c.Qty from (Select distinct a.id,POID ,w.article
            from Orders a,CuttingOutput_Detail cu,WorkOrder_Distribute w where a.id = w.OrderID and cu.id='{0}' 
            and w.WorkOrderUkey = cu.WorkOrderUkey) as b,
            order_Qty c where c.id = b.id and b.Article = c.Article) d,Order_ColorCombo e,order_Eachcons cons
            where d.POID=e.id and d.Article = e.Article and e.FabricCode is not null and e.FabricCode !='' and cons.id =e.id and d.poid = cons.id and cons.CuttingPiece='0' and  cons.FabricCombo = e.PatternPanel

            Select  b.orderid,b.Article,b.SizeCode,c.PatternPanel,isnull(sum(b.qty),0) as cutqty 
            into #tmp2
            from CuttingOutput ma,CuttingOutput_Detail a ,WorkOrder_Distribute b , WorkOrder_PatternPanel c , Orders O
            Where ma.cdate<'{1}' and ma.ID = a.id and ma.Status!='New' 
            and a.WorkOrderUkey = b.WorkOrderUkey and a.WorkOrderUkey = c.WorkOrderUkey   and O.ID=b.OrderID
            and O.POID in (select CuttingID from CuttingOutput_Detail where CuttingOutput_Detail.ID = '{0}')
            group by b.orderid,b.Article,b.SizeCode,c.PatternPanel

            Select a.poid,a.id,a.article,a.sizecode,min(isnull(b.cutqty,0)) as cutqty 
            from #tmp1 a 
            left join #tmp2 b on a.id = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
            group by a.poid,a.id,a.article,a.sizecode"
            , CurrentMaintain["ID"], Convert.ToDateTime(CurrentMaintain["cdate"]).ToShortDateString());

            string sql2 = string.Format(@"
			Select distinct d.*,e.Colorid,e.PatternPanel
            into #tmp1
            from 
            (Select b.POID,c.ID,c.Article,c.SizeCode,c.Qty from (Select distinct a.id,POID ,w.article
            from Orders a,CuttingOutput_Detail cu,WorkOrder_Distribute w where a.id = w.OrderID and cu.id='{0}' 
            and w.WorkOrderUkey = cu.WorkOrderUkey) as b,
            order_Qty c where c.id = b.id and b.Article = c.Article) d,Order_ColorCombo e,order_Eachcons cons
            where d.POID=e.id and d.Article = e.Article and e.FabricCode is not null and e.FabricCode !='' and cons.id =e.id and d.poid = cons.id and cons.CuttingPiece='0' and  cons.FabricCombo = e.PatternPanel

            Select  b.orderid,b.Article,b.SizeCode,c.PatternPanel,isnull(sum(b.qty),0) as cutqty 
            into #tmp2
            from CuttingOutput ma,CuttingOutput_Detail a ,WorkOrder_Distribute b , WorkOrder_PatternPanel c , Orders O
            Where ma.cdate<='{1}' and ma.ID = a.id and ma.Status!='New' 
            and a.WorkOrderUkey = b.WorkOrderUkey and a.WorkOrderUkey = c.WorkOrderUkey   and O.ID=b.OrderID
            and O.POID in (select CuttingID from CuttingOutput_Detail where CuttingOutput_Detail.ID = '{0}')
            group by b.orderid,b.Article,b.SizeCode,c.PatternPanel

            Select a.poid,a.id,a.article,a.sizecode,min(isnull(b.cutqty,0)) as cutqty 
            from #tmp1 a 
            left join #tmp2 b on a.id = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
            group by a.poid,a.id,a.article,a.sizecode"
            , CurrentMaintain["ID"], Convert.ToDateTime(CurrentMaintain["cdate"]).ToShortDateString());

            DataTable t1, t2;
            DBProxy.Current.Select(null, sql1, out t1); //今天日期之前的gmt數
            DBProxy.Current.Select(null, sql2, out t2); //包含今天之前的gmt 數
            int t1gmt,t2gmt;
            if (t1.Rows.Count == 0) t1gmt = 0;
            else t1gmt = Convert.ToInt32(t1.Compute("sum(cutqty)", ""));
            if (t2.Rows.Count == 0) t2gmt = 0;
            else t2gmt = Convert.ToInt32(t2.Compute("sum(cutqty)", ""));
            int gmt = t1gmt - t2gmt; //相減為當天的GMT數
            #endregion
            string update = "";
            DataRow wipRow;
            decimal ncpu=0,var;
            decimal cpu;

            foreach (DataRow dr in t2.Rows)
            {
                if (MyUtility.Check.Seek(string.Format("Select * from CuttingOutput_WIP where Orderid='{0}' and article ='{1}' and size = '{2}'", dr["id"], dr["article"], dr["sizecode"]), out wipRow, null))
                {
                    update = update + string.Format("update CuttingOutput_WIP set Qty = {0} where Orderid='{1}' and article ='{2}' and size = '{3}';", dr["cutqty"], dr["id"], dr["article"], dr["sizecode"]);
                    cpu = Convert.ToDecimal(MyUtility.GetValue.Lookup("CPU", dr["id"].ToString(), "Orders", "ID"));
                    var = Convert.ToDecimal(dr["cutqty"]) - Convert.ToDecimal(wipRow["Qty"]);
                    ncpu = ncpu + var * cpu;
                }
                else
                {
                    update = update + string.Format("Insert into CuttingOutput_WIP(orderid,article,size,qty) values('{0}','{1}','{2}',{3});", dr["id"], dr["Article"], dr["SizeCode"], dr["cutqty"]);
                    cpu = Convert.ToDecimal(MyUtility.GetValue.Lookup("CPU", dr["id"].ToString(), "Orders", "ID"));
                    ncpu = ncpu + Convert.ToDecimal(dr["cutqty"]) * cpu;
                }
            }
            decimal pph;
            if (MyUtility.Check.Empty(CurrentMaintain["ManPower"]) || MyUtility.Check.Empty(CurrentMaintain["ManHours"]))
            {
                pph = 0;
            }
            else
            {
                if (Convert.ToDecimal(MyUtility.Check.Empty(CurrentMaintain["ManHours"])) > 0)
                {
                    pph = Math.Round(ncpu / Convert.ToDecimal(CurrentMaintain["ManPower"]) / Convert.ToDecimal(MyUtility.Check.Empty(CurrentMaintain["ManHours"])), 2);
                }
                else {
                    pph = 0;
                }
            }

            update = update + string.Format("update Cuttingoutput set status='Confirmed',editDate=getdate(),editname ='{0}',actgarment ='{2}',pph='{3}' where id='{1}'", loginID, CurrentMaintain["ID"], gmt, pph);

            #region transaction
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, update)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
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

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
            #endregion

        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 若前面的單子有UnConfrim 則UnConfirm

            string sql = string.Format("Select * from Cuttingoutput where cdate>'{0}' and Status!='New' and mDivisionid = '{1}'", CurrentMaintain["cdate"], keyWord);
            string msg = "";
            DataTable Dt;
            if (DBProxy.Current.Select(null, sql, out Dt))
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    msg = msg + "<Date>:" + dr["cdate"].ToString() + "\\n";
                }
                if (MyUtility.Check.Empty(msg))
                {
                    MyUtility.Msg.WarningBox("The record not yet Unconfirm, you can not Unconfirm.Please see below list.\\n" + msg);
                    return;
                }
            }
            #endregion
            #region GMT SQL
            string sql1 = string.Format(@"
            Select distinct d.*,e.Colorid,e.PatternPanel
            into #tmp1
            from 
            (Select b.POID,c.ID,c.Article,c.SizeCode,c.Qty from (Select distinct a.id,POID ,w.article
            from Orders a,CuttingOutput_Detail cu,WorkOrder_Distribute w where a.id = w.OrderID and cu.id='{0}' 
            and w.WorkOrderUkey = cu.WorkOrderUkey) as b,
            order_Qty c where c.id = b.id and b.Article = c.Article) d,Order_ColorCombo e,order_Eachcons cons
            where d.POID=e.id and d.Article = e.Article and e.FabricCode is not null and e.FabricCode !='' and cons.id =e.id and d.poid = cons.id and cons.CuttingPiece='0' and  cons.FabricCombo = e.PatternPanel

            Select  b.orderid,b.Article,b.SizeCode,c.PatternPanel,isnull(sum(b.qty),0) as cutqty 
            into #tmp2
            from CuttingOutput ma,CuttingOutput_Detail a ,WorkOrder_Distribute b , WorkOrder_PatternPanel c , Orders O
            Where ma.cdate<'{1}' and ma.ID = a.id and ma.Status!='New' 
            and a.WorkOrderUkey = b.WorkOrderUkey and a.WorkOrderUkey = c.WorkOrderUkey   and O.ID=b.OrderID
            and O.POID in (select CuttingID from CuttingOutput_Detail where CuttingOutput_Detail.ID = '{0}')
            group by b.orderid,b.Article,b.SizeCode,c.PatternPanel

            Select a.poid,a.id,a.article,a.sizecode,min(isnull(b.cutqty,0)) as cutqty 
            from #tmp1 a 
            left join #tmp2 b on a.id = b.orderid and a.Article = b.Article and a.PatternPanel = b.PatternPanel and a.SizeCode = b.SizeCode
            group by a.poid,a.id,a.article,a.sizecode"
            , CurrentMaintain["ID"], Convert.ToDateTime(CurrentMaintain["cdate"]).ToShortDateString());

            DataTable t1;
            DBProxy.Current.Select(null, sql1, out t1);
            #endregion
            string update = "";
            foreach (DataRow dr in t1.Rows)
            {
                if (MyUtility.Check.Seek(string.Format("Select * from CuttingOutput_WIP where Orderid='{0}' and article ='{1}' and size = '{2}'", dr["id"], dr["article"], dr["sizecode"]), null))
                {
                    update = update + string.Format("update CuttingOutput_WIP set Qty = {0}  where Orderid='{1}' and article ='{2}' and size = '{3}';", dr["CutQty"], dr["id"], dr["article"], dr["sizecode"]);
                }
                else
                {
                    update = update + string.Format("Insert into CuttingOutput_WIP(orderid,article,size,qty) values('{0}','{1}','{2}',{3});", dr["ID"], dr["Article"], dr["SizeCode"], dr["CutQty"]);
                }
            }
            update = update + string.Format("update Cuttingoutput set status='New',editDate=getdate(),editname ='{0}',actgarment =0,pph=0  where id='{1}'", loginID, CurrentMaintain["ID"]);


            #region transaction
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, update)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
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

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
            #endregion
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            DataTable dt = (DataTable)detailgridbs.DataSource;
            var frm = new Sci.Production.Cutting.P20_Import_Workorder(CurrentMaintain, dt);
            frm.ShowDialog(this);
        }

//        private void Gmtrequery()
//        {
//            #region 找出有哪些部位
//            DataTable fabcodetb;
//            string fabcodesql = string.Format(@"Select distinct a.PatternPanel
//            from Order_ColorCombo a ,Order_EachCons b,CuttingOutput_Detail c
//            where a.id = c.cuttingid and a.FabricCode is not null and a.FabricCode !='' 
//            and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel and b.id = c.cuttingid and c.id ='{0}'
//            order by patternpanel", CurrentMaintain["ID"]);
//            DualResult fabresult = DBProxy.Current.Select("Production", fabcodesql, out fabcodetb);
//            #endregion
//            #region 建立Grid
//            string settbsql = "Select distinct a.id,article,sizecode,a.qty,'' as complete"; //寫SQL建立Table
//            foreach (DataRow dr in fabcodetb.Rows) //組動態欄位
//            {
//                settbsql = settbsql + ", 0 as " + dr["PatternPanel"];
//            }
//            settbsql = settbsql + string.Format(" From Order_Qty a,orders b,CuttingOutput_Detail c Where b.cuttingsp =c.cuttingid and c.id = '{0}' and a.id = b.id order by id,article,sizecode", CurrentMaintain["ID"]);

//            DataTable gridtb;
//            DualResult gridResult = DBProxy.Current.Select(null, settbsql, out gridtb);

//            #endregion
//            #region 寫入部位數量
//            string getqtysql = string.Format("Select b.article,b.sizecode,b.qty,c.PatternPanel,b.orderid from CuttingOutput_Detail a, workorder_Distribute b, workorder_PatternPanel c Where a.cdate <= '{0}' and a.mdivisionid = '{1}' and a.workorderukey = b.workorderukey and a.workorderukey = c.workorderukey and b.workorderukey = c.workorderukey and b.article !=''", CurrentMaintain["cdate"], keyWord);
//            DataTable getqtytb;

//            gridResult = DBProxy.Current.Select(null, getqtysql, out getqtytb);
//            foreach (DataRow dr in getqtytb.Rows)
//            {
//                DataRow[] gridselect = gridtb.Select(string.Format("id = '{0}' and article = '{1}' and sizecode = '{2}'", dr["orderid"], dr["article"], dr["sizecode"], dr["PatternPanel"], dr["Qty"]));
//                if (gridselect.Length != 0)
//                {
//                    gridselect[0][dr["PatternPanel"].ToString()] = MyUtility.Convert.GetDecimal((gridselect[0][dr["PatternPanel"].ToString()])) + MyUtility.Convert.GetDecimal(dr["Qty"]);
//                }


//            }
//            #endregion
//            #region 判斷是否Complete
//            bool complete = true;
//            DataTable panneltb;
//            fabcodesql = string.Format(@"Select distinct a.Article,a.PatternPanel
//            from Order_ColorCombo a ,Order_EachCons b,(Select distinct orders.id,POID ,article
//			from Orders,CuttingOutput_Detail ,WorkOrder_Distribute where Orders.id = WorkOrder_Distribute.OrderID and               
//            CuttingOutput_Detail.id='{0}' 
//			and WorkOrder_Distribute.WorkOrderUkey = CuttingOutput_Detail.WorkOrderUkey) as f
//            where a.id = f.poid and a.FabricCode is not null 
//            and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel
//            and a.FabricCode !='' order by Article,PatternPanel", CurrentMaintain["ID"]);
//            gridResult = DBProxy.Current.Select(null, fabcodesql, out panneltb);
//            foreach (DataRow dr in gridtb.Rows)
//            {
//                complete = true;
//                DataRow[] sel = panneltb.Select(string.Format("Article = '{0}'", dr["Article"]));
//                foreach (DataRow pdr in sel)
//                {

//                    if (MyUtility.Convert.GetDecimal(dr["Qty"]) > MyUtility.Convert.GetDecimal(dr[pdr["Patternpanel"].ToString()])) complete = false;

//                }
//                if (complete) dr["Complete"] = "Y";

//            }
//            #endregion
//            grid1.DataSource = gridtb;
//        }
    }
}
