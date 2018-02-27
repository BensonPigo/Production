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
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DualResult result;
            result = DBProxy.Current.Select(null, querySql, out queryDT);
            if (!result)
            {
                ShowErr(result);
            }
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
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
        protected override Ict.DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
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
            From cuttingoutput_Detail a WITH (NOLOCK) , WorkOrder e WITH (NOLOCK) 
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
            from Cuttingoutput_Detail_Detail a WITH (NOLOCK) 
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
                if (DBProxy.Current.Select(null, string.Format(@"Select * from WorkOrder a WITH (NOLOCK) where a.cutref = '{0}'", e.FormattedValue.ToString()), out dt))
                {
                    if (dt.Rows.Count==0)
                    {
                        dr["Orderid"] = "";
                        dr["Cuttingid"] = "";
                        dr["Cutref"] = "";
                        dr["FabricCombo"] = "";
                        dr["MarkerName"] = "";
                        dr["Layer"] =0;
                        dr["Cons"] = 0;
                        dr["FabricPanelCode"] = "";
                        dr["Workorderukey"] = 0;
                        dr["SizeRatio"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("<Cut Ref> data not found.");
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
                dr["FabricPanelCode"] = seldr["FabricPanelCode"];
                dr["cutno"] = seldr["cutno"];
                dr["MarkerLength"] = seldr["MarkerLength"];
                dr["colorID"] = seldr["colorID"];
                dr["Workorderukey"] = seldr["Ukey"];
                dr["SizeRatio"] = "";
                DataTable workorderTb;
                string str = "";
                if (DBProxy.Current.Select(null, string.Format("Select * from Workorder_SizeRatio a WITH (NOLOCK) where a.workorderukey = '{0}'", dr["Workorderukey"]), out workorderTb))
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
                if (DBProxy.Current.Select(null, string.Format("Select * from Workorder_distribute a WITH (NOLOCK) where a.workorderukey = '{0}'", dr["Workorderukey"]), out workorderTb))
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
            .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerLength", header: "Marker Length", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 7, decimal_places: 2, iseditingreadonly: true)
            .Text("sizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: sizeratio);
            this.detailgrid.Columns["Cutref"].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["cDate"] = DateTime.Today.AddDays(-1);
            CurrentMaintain["mDivisionid"] = keyWord;
            CurrentMaintain["Status"] = "New";
            txtfactoryByM1.Text = Sci.Env.User.Factory;
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
            if (MyUtility.Check.Empty(dateDate.Value))
            {
                MyUtility.Msg.WarningBox("<Date> can not be empty.");
                return false;
            }
            if (MyUtility.Check.Empty(numManPower.Text))
            {
                MyUtility.Msg.WarningBox("<Man Power> can not be empty.");
                return false;
            }
            if (MyUtility.Check.Empty(txtfactoryByM1.Text))
            {
                MyUtility.Msg.WarningBox("<Factory> can not be empty.");
                return false;
            }
            if (this.IsDetailInserting)
            {
                string date = dateDate.Text;
                string sql = string.Format("Select * from Cuttingoutput WITH (NOLOCK) Where cdate = '{0}' and id !='{1}' and FactoryID ='{2}'", date, CurrentMaintain["ID"], txtfactoryByM1.Text);
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
                sumsql = string.Format("Select sum(Layer) as tlayer from Workorder WITH (NOLOCK) where id='{0}' and cutref = '{1}'", cuttingid, cutref);
                string str = MyUtility.GetValue.Lookup(sumsql, null);
                if (MyUtility.Check.Empty(str)) layer_work = 0;
                else layer_work = Convert.ToInt32(str);

                sumsql = string.Format("Select sum(Layer) as cuttingL from Cuttingoutput_Detail WITH (NOLOCK) where id!='{0}' and cutref = '{1}' and cuttingid= '{2}'", id, cutref, cuttingid);
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
            MyUtility.Tool.ProcessWithDatatable(detailTb, "WorkorderUkey", "Select b.* from #tmp a, workorder_SizeRatio b WITH (NOLOCK) where a.workorderukey = b.workorderukey", out SizeTb);
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
            DualResult result;
            #region 若前面的單子有尚未Confrim 則不可Confirm

            string sql = string.Format("Select * from Cuttingoutput WITH (NOLOCK) where cdate<'{0}' and Status='New' and mDivisionid = '{1}'", ((DateTime)CurrentMaintain["cdate"]).ToString("yyyy/MM/dd"), keyWord);
            string msg = "";
            DataTable Dt;
            result = DBProxy.Current.Select(null, sql, out Dt);
            if (result)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    msg = msg + "<ID>:" + dr["ID"] +  ",<Date>:" + ((DateTime)dr["cdate"]).ToString("yyyy/MM/dd") + Environment.NewLine;
                }
                if (!MyUtility.Check.Empty(msg))
                {
                    MyUtility.Msg.WarningBox("The record not yet confirm, you can not confirm.Please see below list." + Environment.NewLine + msg);
                    return;
                }
            }
            else
            {
                ShowErr(result);
                return;
            }

            #endregion

            string update = "";
            update = $@"update Cuttingoutput set status='Confirmed',editDate=getdate(),editname ='{loginID}' where id='{CurrentMaintain["ID"]}';
                        EXEC Cutting_P20_CFM_Update '{CurrentMaintain["ID"]}','{((DateTime)CurrentMaintain["cdate"]).ToString("yyyy/MM/dd")}',{CurrentMaintain["ManPower"]},{CurrentMaintain["ManHours"]},'Confirm';";

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
                        ShowErr(upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
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
            
            #endregion
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;
            #region 若前面的單子有UnConfrim 則UnConfirm

            string sql = string.Format("Select * from Cuttingoutput  WITH (NOLOCK) where cdate>'{0}' and Status!='New' and mDivisionid = '{1}'", ((DateTime)CurrentMaintain["cdate"]).ToString("yyyy/MM/dd"), keyWord);
            string msg = "";
            DataTable Dt;
            result = DBProxy.Current.Select(null, sql, out Dt);
            if (result)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    msg = msg + "<ID>:" + dr["ID"] + ",<Date>:" + ((DateTime)dr["cdate"]).ToString("yyyy/MM/dd") + Environment.NewLine;
                }
                if (!MyUtility.Check.Empty(msg))
                {
                    MyUtility.Msg.WarningBox("The record not yet Unconfirm, you can not Unconfirm.Please see below list." + Environment.NewLine + msg);
                    return;
                }
            }
            else
            {
                ShowErr(result);
                return;
            }
            #endregion
            string update = "";
            update = $@"update Cuttingoutput set status='New',editDate=getdate(),editname ='{loginID}' where id='{CurrentMaintain["ID"]}';
                        EXEC Cutting_P20_CFM_Update '{CurrentMaintain["ID"]}','{((DateTime)CurrentMaintain["cdate"]).ToString("yyyy/MM/dd")}',{CurrentMaintain["ManPower"]},{CurrentMaintain["ManHours"]},'UnConfirm';";

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
                        ShowErr(upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
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

            #endregion

        }

        private void btnImportfromWorkOrder_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            DataTable dt = (DataTable)detailgridbs.DataSource;
            var frm = new Sci.Production.Cutting.P20_Import_Workorder(CurrentMaintain, dt);
            frm.ShowDialog(this);
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
        }

        private void btnImportfromRealtimeBundleTracking_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            DataTable dt = (DataTable)detailgridbs.DataSource;
            var frm = new P20_Import_RFID(CurrentMaintain, dt);
            frm.ShowDialog(this);
        }
    }
}
