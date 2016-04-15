using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using Ict.Data;
using Ict;

namespace Sci.Production.Cutting
{
    public partial class P10 : Sci.Win.Tems.Input6
    {
        string keyword = Sci.Env.User.Keyword;
        string LoginId = Sci.Env.User.UserID;
        DataTable bundle_Detail_allpart_Tb, bundle_Detail_Art_Tb, bundle_Detail_Qty_Tb;
        public P10(ToolStripMenuItem menuitem,string history)
            : base(menuitem)
        {

            InitializeComponent();
            if (history == "0") this.DefaultFilter = string.Format("Orderid in (Select id from orders where finished=0) and mDivisionid='{0}'", keyword);
            else this.DefaultFilter = string.Format("Orderid in (Select id from orders where finished=1) and mDivisionid='{0}'",keyword);

        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (CurrentMaintain == null) return;
            string orderid = CurrentMaintain["OrderID"].ToString();
            string cutref = (CurrentMaintain["Cutref"] == null) ? "" : CurrentMaintain["Cutref"].ToString();
            string cuttingid = "";
            DataTable orders;
            if (DBProxy.Current.Select(null, string.Format("Select * from Orders Where id='{0}'", orderid), out orders))
            {
                if (orders.Rows.Count != 0)
                {
                    displayBox_Season.Text = orders.Rows[0]["Seasonid"].ToString();
                    displayBox_Style.Text = orders.Rows[0]["Styleid"].ToString();
                    cuttingid = orders.Rows[0]["cuttingsp"].ToString();
                }
                else
                {
                    displayBox_Season.Text = "";
                    displayBox_Style.Text = "";
                }
            }
            else
            {
                displayBox_Season.Text = "";
                displayBox_Style.Text = "";
            }
            string estcutdate = MyUtility.GetValue.Lookup(string.Format("Select estcutdate from workorder where id='{0}' and cutref = '{1}'", cuttingid, cutref), null);
            displayBox_EstCutdate.Text = estcutdate;
            
        }
        protected override Ict.DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.*,
            (
                Select Subprocessid+'+' 
                From Bundle_Detail_art c
                Where c.bundleno =a.bundleno and c.id = a.id 
                For XML path('')
            ) as subProcessid, 0 as ukey1
            From Bundle_Detail a,Orders b 
            where a.id = '{0}'
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            #region 先撈出底層其他Table
            string allPart_cmd = string.Format(@"Select b.*, 0 as ukey1 from Bundle_Detail a,Bundle_Detail_Allpart b Where a.id ='{0}' and a.Bundleno = b.bundleno and a.id = b.id", masterID);
            string art_cmd = string.Format(@"Select b.*, 0 as ukey1 from Bundle_Detail a,Bundle_Detail_art b Where a.id ='{0}' and a.Bundleno = b.bundleno and a.id = b.id", masterID);
            string qty_cmd = string.Format(@"Select a.* from Bundle_Detail_qty a Where a.id ='{0}'", masterID);
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out bundle_Detail_allpart_Tb);
            if(!dRes)
            {
                ShowErr(allPart_cmd,dRes);
                return dRes;
            }
            dRes = DBProxy.Current.Select(null, art_cmd, out bundle_Detail_Art_Tb);
            if(!dRes)
            {
                ShowErr(allPart_cmd,dRes);
                return dRes;
            }
            dRes = DBProxy.Current.Select(null, qty_cmd, out bundle_Detail_Qty_Tb);
            if(!dRes)
            {
                ShowErr(allPart_cmd,dRes);
                return dRes;
            }
            #endregion

            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("BundleGroup", header: "Group", width: Widths.AnsiChars(4), integer_places: 5,iseditingreadonly: true)
            .Text("Bundleno", header: "Bundle No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("PatternCode", header: "Cutpart", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("PatternDesc", header: "Cutpart name", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("subProcessid", header: "SubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true)
            .Numeric("Farmout", header: "Farm Out", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true)
            .Numeric("Farmin", header: "Farm In", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true);
            return base.OnGridSetup();
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Cdate"] = DateTime.Now;
            CurrentMaintain["mDivisionid"] = keyword;
            bundle_Detail_allpart_Tb.Clear();
            bundle_Detail_Art_Tb.Clear();
            bundle_Detail_Qty_Tb.Clear();

        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("<SP#> can not be empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PatternPanel"]))
            {
                MyUtility.Msg.WarningBox("<Pattern Panel> can not be empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["sewinglineid"]))
            {
                MyUtility.Msg.WarningBox("<Line> can not be empty!");
                return false;
            }
            if (IsDetailInserting)
            {
                string Keyword = keyword + "BC";
                string cid = MyUtility.GetValue.GetID(Keyword, "Bundle", Convert.ToDateTime(CurrentMaintain["cdate"]));
                if (string.IsNullOrWhiteSpace(cid))
                {
                    return false;
                }
                CurrentMaintain["ID"] = cid;
                foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows)
                {
                    dr["ID"] = cid;
                }
                foreach (DataRow dr in bundle_Detail_Art_Tb.Rows)
                {
                    dr["ID"] = cid;
                }
                foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows)
                {
                    dr["ID"] = cid;
                }
            }

            return base.ClickSaveBefore();
        }
        protected override DualResult ClickSavePost()
        {
            #region 填入Bundleno
            int drcount = DetailDatas.Count;
            IList<string > cListBundleno;
            
            cListBundleno = MyUtility.GetValue.GetBatchID("", "Bundle_Detail", Convert.ToDateTime(CurrentMaintain["cDate"]), 3, batchNumber: drcount);
            if (cListBundleno.Count == 0)
            {
                return new DualResult(false, "Create Bundleno error.");
            }
            int nCount = 0;
            
            DataRow[] roway;
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Bundleno"]) && dr.RowState!=DataRowState.Deleted)
                {
                    CurrentDetailData["Bundleno"] = cListBundleno[nCount];
                    roway = bundle_Detail_allpart_Tb.Select(string.Format("ukey1 = {0}", dr["ukey"]));
                    foreach (DataRow dr2 in roway)
                    {
                        dr2["Bundleno"] = cListBundleno[nCount];
                    }
                    roway = bundle_Detail_Art_Tb.Select(string.Format("ukey1 = {0}", dr["ukey"]));
                    foreach (DataRow dr2 in roway)
                    {
                        dr2["Bundleno"] = cListBundleno[nCount];
                    }
                    nCount++;

                }
            }
            #endregion 
            string allpart_cmd = "", Art_cmd = "",Qty_cmd = "";

            foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    allpart_cmd = allpart_cmd + string.Format(
                    @"Delete table bundle_Detail_allpart where ukey = '{0}';",dr["ukey"]);
                }
                if (dr.RowState == DataRowState.Added)
                {
                    allpart_cmd = allpart_cmd + string.Format(
                    @"insert into bundle_Detail_allpart(ID,Bundleno,PatternCode,PatternDesc,Parts,Ukey) 
                    values('{0}','{1}','{2}','{3}',{4});"
                    ,dr["ID"],dr["Bundleno"],dr["PatternCode"],dr["PatternDesc"],dr["Parts"]);
                }

                if(dr.RowState == DataRowState.Modified)
                {
                    allpart_cmd = allpart_cmd + string.Format(
                    @"update bundle_Detail_allpart set PatterCode ='{0}',PatternDesc = '{1}',
                    Parts ={2} Where ukey ={3};",dr["PatternCode"], dr["PatternDesc"], dr["Parts"], dr["ukey"]);
                }
            }
            foreach (DataRow dr in bundle_Detail_Art_Tb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    Art_cmd = Art_cmd + string.Format(
                    @"Delete table bundle_Detail_Art where ukey = '{0}';", dr["ukey"]);
                }
                if (dr.RowState == DataRowState.Added)
                {
                    Art_cmd = Art_cmd + string.Format(
                    @"insert into bundle_Detail_Art(ID,Bundleno,PatternCode,SubProcessid) 
                    values('{0}','{1}','{2}','{3}');"
                    , dr["ID"], dr["Bundleno"], dr["PatternCode"], dr["SubProcessid"]);
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    Art_cmd = Art_cmd + string.Format(
                    @"update bundle_Detail_Art set PatterCode ='{0}',SubProcessid = '{1}' where
                     Where ukey = {2};", dr["PatternCode"], dr["SubProcessid"], dr["ukey"]);
                }

            }
            foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    Qty_cmd = Qty_cmd + string.Format(
                    @"Delete table bundle_Detail_Qty where ukey = '{0}';", dr["ukey"]);
                }
                if (dr.RowState == DataRowState.Added)
                {
                    Qty_cmd = Qty_cmd + string.Format(
                    @"insert into bundle_Detail_Qty(ID,SizeCode,Qty) 
                    values('{0}','{1}',{2});"
                    , dr["ID"], dr["sizecode"], dr["Qty"]);
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    Qty_cmd = Qty_cmd + string.Format(
                    @"update bundle_Detail_Qty set SizeCode ='{0}',Qty = {1} where
                     Where ukey = {2};", dr["SizeCode"], dr["Qty"], dr["ukey"]);
                }

            }
            return base.ClickSavePost();
        }
        private void textBox_Cutref_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            if (textBox_Cutref.OldValue.ToString() == textBox_Cutref.Text) return;
            string cmd = string.Format(
            @"Select a.*,substring(b.Sewline,1,2) as Sewline ,b.poid,b.seasonid,b.styleid,
             (
                Select d.SizeCode+'/' 
                From Workorder_SizeRatio d
                Where a.ukey =d.workorderukey
                For XML path('')
            ) as SizeCode,
             (
                Select convert(varchar,Qty)+'/' 
                From Workorder_SizeRatio e
                Where a.ukey =e.workorderukey
                For XML path('')
            ) as Ratio,
            (
                Select Top(1)Article
                From Workorder_Distribute f
                Where a.ukey =f.workorderukey and a.orderid=f.orderid
            ) as article,
            (
                Select count(id)
                From Workorder_Distribute g 
                Where a.ukey =g.workorderukey and a.orderid=g.orderid
            ) as Qty
            From workorder a ,orders b 
            Where a.cutref='{0}' and a.mDivisionid = '{1}' and a.orderid = b.id", textBox_Cutref.Text, keyword);
            DataRow cutdr;
            if (!MyUtility.Check.Seek(cmd, out cutdr, null))
            {
                MyUtility.Msg.WarningBox("<Cut Ref#> data not found!");
                displayBox_Season.Text = "";
                displayBox_Style.Text = "";
                displayBox_PrintDate.Text = "";
                CurrentMaintain["Cutno"] = 0;
                CurrentMaintain["sewinglineid"] = "";
                CurrentMaintain["OrderID"] = "";
                CurrentMaintain["POID"] = "";
                CurrentMaintain["PatternPanel"] = "";
                CurrentMaintain["Sizecode"] = "";
                CurrentMaintain["Ratio"] = "";
                CurrentMaintain["Article"] = "";
                CurrentMaintain["Colorid"] = "";
                CurrentMaintain["Qty"] = 0;
                CurrentMaintain["SewingCell"] = "";
                CurrentMaintain["startno"] = 1;
                CurrentMaintain["cutref"] = "";
                CurrentMaintain["ITEM"] = "";

                DetailDatas.Clear();
                bundle_Detail_allpart_Tb.Clear();
                bundle_Detail_Art_Tb.Clear();
                bundle_Detail_Qty_Tb.Clear();
                CurrentMaintain.EndEdit();
                e.Cancel = true;
                return;
            }
            else
            {
                CurrentMaintain["Cutno"] = Convert.ToInt32(cutdr["Cutno"].ToString());
                CurrentMaintain["sewinglineid"] = cutdr["Sewline"].ToString();
                CurrentMaintain["OrderID"] = cutdr["OrderID"].ToString();
                CurrentMaintain["POID"] = cutdr["POID"].ToString();
                CurrentMaintain["PatternPanel"] = cutdr["Fabriccombo"].ToString();
                CurrentMaintain["Sizecode"] = cutdr["Sizecode"].ToString();
                CurrentMaintain["Ratio"] = cutdr["Ratio"].ToString();
                CurrentMaintain["Article"] = cutdr["Article"].ToString();
                CurrentMaintain["Colorid"] = cutdr["Colorid"].ToString();
                CurrentMaintain["Qty"] = cutdr["Qty"];
                displayBox_Season.Text = cutdr["Seasonid"].ToString();
                displayBox_Style.Text = cutdr["Styleid"].ToString();
                displayBox_PrintDate.Text = cutdr["Estcutdate"].ToString();

                string cellid = MyUtility.GetValue.Lookup("SewingCell", cutdr["sewline"].ToString(), "SewingLine", "ID");

                CurrentMaintain["SewingCell"] = cellid;
                string max_cmd = string.Format("Select isnull(Max(startno+Qty),0) as Start from Bundle Where OrderID = '{0}'", cutdr["OrderID"].ToString());
                DataTable max_st;
                if (DBProxy.Current.Select(null, max_cmd, out max_st))
                {
                    if (max_st.Rows.Count != 0) CurrentMaintain["startno"] = max_st.Rows[0]["Start"].ToString();
                    else CurrentMaintain["startno"] = 1;
                }
                else
                {
                    CurrentMaintain["startno"] = 1;
                }
                string item_cmd = string.Format("Select a.Name from Reason a, Style b where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{0}' and b.ApparelType = a.id", cutdr["styleukey"]);
                string item = MyUtility.GetValue.Lookup(item_cmd, null);
                CurrentMaintain["ITEM"] = item;
            }
            
        }
        private void textBox_orderid_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["Cutref"]) || MyUtility.Check.Empty(CurrentMaintain["POID"])) return;
            Sci.Win.Tools.SelectItem item;
            string cuttingid = MyUtility.GetValue.Lookup("Cuttingsp",CurrentMaintain["POID"].ToString(),"Orders","ID");
            string selectCommand = string.Format("select b.orderid from workorder a, workorder_distribute b where a.cutref = '{0}' and a.id = '{1}' and a.ukey = b.workorderukey and a.id = b.id and b.id = '{1}'", CurrentMaintain["Cutref"], cuttingid);

            item = new Sci.Win.Tools.SelectItem(selectCommand, "13", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        private void textBox_orderid_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = textBox_orderid.Text;
            if (textBox_orderid.OldValue.ToString() == newvalue) return;
            string cuttingsp = MyUtility.GetValue.Lookup("Cuttingsp", newvalue, "Orders", "ID");
            if (!MyUtility.Check.Empty(CurrentMaintain["Cutref"]))
            {
                string cuttingid = MyUtility.GetValue.Lookup("id", CurrentMaintain["Cutref"].ToString(), "workorder", "Cutref");
                if (cuttingsp != cuttingid)
                {
                    MyUtility.Msg.WarningBox("<Cutref> is different.");
                    textBox_orderid.Text = "";
                    e.Cancel = true;
                    return;
                }
                string work_cmd = string.Format("Select * from workorder a,workorder_Distribute b Where a.ukey = b.workorderukey and a.cutref = '{0}' and b.orderid ='{1}'",CurrentMaintain["Cutref"],newvalue);
                DataTable articleTb;
                if (DBProxy.Current.Select(null, work_cmd,out articleTb))
                {
                    if (articleTb.Rows.Count ==0 )
                    {
                        MyUtility.Msg.WarningBox("<Cutref> is different.");
                        textBox_orderid.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    CurrentMaintain["Qty"] = articleTb.Rows.Count; //一筆distribute 表示一個bundle
                }
            }
            else//Issue#969 當CutRef# 為空時，SP No 清空時，清空MasterID與Item、Line
            {
                if (MyUtility.Check.Empty(newvalue))
                {
                    CurrentMaintain["POID"] = "";
                    CurrentMaintain["Item"] = "";
                    CurrentMaintain["sewinglineid"] = "";
                    DetailDatas.Clear();
                    bundle_Detail_allpart_Tb.Clear();
                    bundle_Detail_Art_Tb.Clear();
                    bundle_Detail_Qty_Tb.Clear();
                }
                else
                {

                    string selectCommand = string.Format("select a.* from orders a where a.id = '{0}' and mDivisionid='{1}' ", newvalue, keyword);
                    DataRow cutdr;
                    if (MyUtility.Check.Seek(selectCommand, out cutdr, null))
                    {
                        CurrentMaintain["sewinglineid"] = cutdr["Sewline"].ToString();
                        CurrentMaintain["OrderID"] = cutdr["id"].ToString();
                        CurrentMaintain["POID"] = cutdr["POID"].ToString();
                        displayBox_Season.Text = cutdr["Seasonid"].ToString();
                        displayBox_Style.Text = cutdr["Styleid"].ToString();
                        #region Item
                        string item_cmd = string.Format("Select a.Name from Reason a, Style b where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{0}' and b.ApparelType = a.id", cutdr["styleukey"]);
                        string item = MyUtility.GetValue.Lookup(item_cmd, null);
                        CurrentMaintain["ITEM"] = item;
                        #endregion
                        #region Cell
                        if (!MyUtility.Check.Empty(cutdr["sewline"].ToString()))
                        {
                            string cellid = MyUtility.GetValue.Lookup("SewingCell", cutdr["sewline"].ToString().Substring(0, 2), "SewingLine", "ID");

                            CurrentMaintain["SewingCell"] = cellid;
                        }
                        #endregion
                        #region startno
                        string max_cmd = string.Format("Select isnull(Max(startno+Qty),0) as Start from Bundle Where OrderID = '{0}'", cutdr["id"].ToString());
                        DataTable max_st;
                        if (DBProxy.Current.Select(null, max_cmd, out max_st))
                        {
                            if (max_st.Rows.Count != 0) CurrentMaintain["startno"] = max_st.Rows[0]["Start"].ToString();
                            else CurrentMaintain["startno"] = 1;
                        }
                        else
                        {
                            CurrentMaintain["startno"] = 1;
                        }
                        #endregion
                        #region Article colorid
                        if (MyUtility.Check.Empty(CurrentMaintain["PatternPanel"]))
                        {
                            string ColorComb_cmd = string.Format("Select top(1) article,colorid from order_colorcombo where id ='{0}' and patternpanel = '{1}'", cutdr["POID"], CurrentMaintain["PatternPanel"]);
                            DataRow colordr;
                            if(MyUtility.Check.Seek(ColorComb_cmd, out colordr))
                            {
                                CurrentMaintain["Article"] = colordr["Article"];
                                CurrentMaintain["colorid"] = colordr["colorid"];
                            }
                            
                        }
                        #endregion 
                    }
                }
            }
        }
    }
}
