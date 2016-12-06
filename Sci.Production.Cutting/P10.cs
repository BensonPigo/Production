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
using Sci.Win.Tools;

namespace Sci.Production.Cutting
{
    public partial class P10 : Sci.Win.Tems.Input6
    {
        string keyword = Sci.Env.User.Keyword;
        string LoginId = Sci.Env.User.UserID;
        DataTable bundle_Detail_allpart_Tb, bundle_Detail_Art_Tb, bundle_Detail_Qty_Tb;
        string WorkOrder_Ukey;

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
            queryTable();
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
            if (!MyUtility.Check.Empty(estcutdate))  displayBox_EstCutdate.Text = Convert.ToDateTime(estcutdate).ToString("yyyy/MM/dd");
            
            int qty = 0;
            if(bundle_Detail_Qty_Tb.Rows.Count==0)  qty =0;
            else  qty = Convert.ToInt16(bundle_Detail_Qty_Tb.Compute("Sum(Qty)",""));
           
            numericBox_GroupQty.Value = qty;

            string factoryid = MyUtility.GetValue.Lookup(string.Format("SELECT o.FactoryID FROM Bundle b inner join orders o on b.Orderid=o.ID where b.Orderid='{0}'",orderid), null);

            //txtsewingline1.factoryobjectName = (Control)factoryid;

        }
        public void queryTable()
        {
            string masterID = (CurrentMaintain == null) ? "" : CurrentMaintain["id"].ToString();
            string allPart_cmd = string.Format(@"Select 0 as sel,b.*, 0 as ukey1,'' as annotation from Bundle_Detail a,Bundle_Detail_Allpart b Where a.id ='{0}' and a.id = b.id", masterID);
            string art_cmd = string.Format(@"Select b.*, 0 as ukey1 from Bundle_Detail a,Bundle_Detail_art b Where a.id ='{0}' and a.Bundleno = b.bundleno and a.id = b.id", masterID);
            string qty_cmd = string.Format(@"Select 0 as No,a.* from Bundle_Detail_qty a Where a.id ='{0}'", masterID);
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out bundle_Detail_allpart_Tb);
            if (!dRes)
            {
                ShowErr(allPart_cmd, dRes);
            }
            dRes = DBProxy.Current.Select(null, art_cmd, out bundle_Detail_Art_Tb);
            if (!dRes)
            {
                ShowErr(art_cmd, dRes);

            }
            dRes = DBProxy.Current.Select(null, qty_cmd, out bundle_Detail_Qty_Tb);
            if (!dRes)
            {
                ShowErr(qty_cmd, dRes);

            }
            int ukey = 1;
            foreach (DataRow dr in DetailDatas)
            {
                dr["ukey1"] = ukey;
                //DataRow[] allar = bundle_Detail_allpart_Tb.Select(string.Format("Bundleno='{0}'", dr["Bundleno"]));
                DataRow[] allar = bundle_Detail_allpart_Tb.Select(string.Format("id='{0}'", dr["ID"]));
                if (allar.Length > 0)
                {
                    foreach (DataRow dr2 in allar)
                    {
                        dr2["ukey1"] = ukey;
                    }
                }
                DataRow[] Artar = bundle_Detail_Art_Tb.Select(string.Format("Bundleno='{0}'", dr["Bundleno"]));
                if (allar.Length > 0)
                {
                    foreach (DataRow dr2 in Artar)
                    {
                        dr2["ukey1"] = ukey;
                    }
                }
            }
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
            From Bundle_Detail a
            where a.id = '{0}' order by bundlegroup
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            #region 先撈出底層其他Table
            if (!IsDetailInsertByCopy)
            {
                string allPart_cmd = string.Format(@"Select 0 as sel,b.*, 0 as ukey1,'' as annotation from Bundle_Detail a,Bundle_Detail_Allpart b Where a.id ='{0}' and a.id = b.id", masterID);
                string art_cmd = string.Format(@"Select b.*, 0 as ukey1 from Bundle_Detail a,Bundle_Detail_art b Where a.id ='{0}' and a.Bundleno = b.bundleno and a.id = b.id", masterID);
                string qty_cmd = string.Format(@"Select 0 as No,a.* from Bundle_Detail_qty a Where a.id ='{0}'", masterID);
                DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out bundle_Detail_allpart_Tb);
                if (!dRes)
                {
                    ShowErr(allPart_cmd, dRes);
                    return dRes;
                }
                dRes = DBProxy.Current.Select(null, art_cmd, out bundle_Detail_Art_Tb);
                if (!dRes)
                {
                    ShowErr(allPart_cmd, dRes);
                    return dRes;
                }
                dRes = DBProxy.Current.Select(null, qty_cmd, out bundle_Detail_Qty_Tb);
                if (!dRes)
                {
                    ShowErr(allPart_cmd, dRes);
                    return dRes;
                }

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
            CurrentMaintain["Cdate"] = DateTime.Today;
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
        protected override DualResult ClickSavePre()
        {
            #region 填入Bundleno
            int drcount = DetailDatas.Count;
            IList<string> cListBundleno;

            cListBundleno = MyUtility.GetValue.GetBatchID("", "Bundle_Detail", Convert.ToDateTime(CurrentMaintain["cDate"]), 3,"Bundleno", batchNumber: drcount,sequenceMode:2);
            if (cListBundleno.Count == 0)
            {
                return new DualResult(false, "Create Bundleno error.");
            }
            int nCount = 0;

            DataRow[] roway;
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Bundleno"]) && dr.RowState != DataRowState.Deleted)
                {
                    dr["Bundleno"] = cListBundleno[nCount];
                    //roway = bundle_Detail_allpart_Tb.Select(string.Format("ukey1 = {0}", dr["ukey1"]));
                    //foreach (DataRow dr2 in roway)
                    //{
                    //    dr2["Bundleno"] = cListBundleno[nCount];
                    //}
                    roway = bundle_Detail_Art_Tb.Select(string.Format("ukey1 = {0}", dr["ukey1"]));
                    foreach (DataRow dr2 in roway)
                    {
                        dr2["Bundleno"] = cListBundleno[nCount];
                    }
                    nCount++;

                }
            }
            #endregion  
            //DataTable dt = (DataTable)detailgridbs.DataSource;
            return base.ClickSavePre();
        }
        protected override DualResult ClickSavePost()
        {

            string allpart_cmd = "", Art_cmd = "",Qty_cmd = "";
            #region 先撈出實體Table 為了平行判斷筆數 DataTable allparttmp, arttmp, qtytmp
            DataTable allparttmp, arttmp, qtytmp;
            string masterID = (CurrentMaintain == null) ? "" : CurrentMaintain["id"].ToString();
            string allPart_cmd = string.Format(@"Select b.* from Bundle_Detail_Allpart b left join Bundle_Detail a on a.id = b.id where b.id='{0}' ", masterID);
            string art_cmd = string.Format(@"Select b.* from Bundle_Detail_art b left join Bundle_Detail a on a.Bundleno = b.bundleno and a.id = b.id where b.id='{0}'", masterID);
            string qty_cmd = string.Format(@"Select a.* from Bundle_Detail_qty a Where a.id ='{0}'", masterID);
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out allparttmp);
            if (!dRes)
            {
                ShowErr(allPart_cmd, dRes);
            }
            dRes = DBProxy.Current.Select(null, art_cmd, out arttmp);
            if (!dRes)
            {
                ShowErr(art_cmd, dRes);

            }
            dRes = DBProxy.Current.Select(null, qty_cmd, out qtytmp);
            if (!dRes)
            {
                ShowErr(qty_cmd, dRes);

            }
            #endregion
            int row = 0;
            #region 處理Bundle_Detail_AllPart
            int allpart_old_rowCount = allparttmp.Rows.Count;
            
            foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows) //處理Bundle_Detail_AllPart
            {

                if (row >= allpart_old_rowCount) //新增
                {
                    allpart_cmd = allpart_cmd + string.Format(
                    @"insert into bundle_Detail_allpart(ID,PatternCode,PatternDesc,Parts) values('{0}','{1}','{2}','{3}');"
                    , CurrentMaintain["ID"], dr["PatternCode"], dr["PatternDesc"], dr["Parts"]);
                }
                else //覆蓋
                {
                    allpart_cmd = allpart_cmd + string.Format(
                @"update bundle_Detail_allpart set PatternCode ='{0}',PatternDesc = '{1}',
                Parts ={2}  Where ukey ={3};", dr["PatternCode"], dr["PatternDesc"], dr["Parts"], allparttmp.Rows[row]["ukey"]);
                }
                row++;
                
            }
            bundle_Detail_allpart_Tb.AcceptChanges();//變更Row Status 狀態
            int allpart_new_rowCount = bundle_Detail_allpart_Tb.Rows.Count;
            if (allpart_old_rowCount > allpart_new_rowCount) //舊的筆數小於新的筆數 表示要先覆蓋再刪除多餘的
            {
                for (int i = allpart_new_rowCount; i < allpart_old_rowCount; i++)
                {
                    allpart_cmd = allpart_cmd + string.Format(@"Delete from bundle_Detail_allpart where ukey ='{0}';", allparttmp.Rows[i]["ukey"]);
                }
            }
            #endregion
            #region 處理Bundle_Detail_Art
            row = 0;
            int art_old_rowCount = arttmp.Rows.Count;
            
            foreach (DataRow dr in bundle_Detail_Art_Tb.Rows) //處理Bundle_Detail_Art
            {

                if (row >= art_old_rowCount) //新增
                {
                    Art_cmd = Art_cmd + string.Format(
                   @"insert into bundle_Detail_Art(ID,Bundleno,PatternCode,SubProcessid) 
                    values('{0}','{1}','{2}','{3}');"
                    , CurrentMaintain["ID"], dr["Bundleno"], dr["PatternCode"], dr["SubProcessid"]);
                }
                else //覆蓋
                {
                    Art_cmd = Art_cmd + string.Format(
                    @"update bundle_Detail_Art set PatternCode ='{0}',SubProcessid = '{1}',bundleno ='{2}' 
                    Where ukey ={3};", dr["PatternCode"], dr["SubProcessid"], dr["Bundleno"], arttmp.Rows[row]["ukey"]);
                }
                row++;
            }
            bundle_Detail_Art_Tb.AcceptChanges();//變更Row Status 狀態
            int art_new_rowCount = bundle_Detail_Art_Tb.Rows.Count;
            if (art_old_rowCount > art_new_rowCount) //舊的筆數小於新的筆數 表示要先覆蓋再刪除多餘的
            {
                for (int i = art_new_rowCount; i < art_old_rowCount; i++)
                {
                    Art_cmd = Art_cmd + string.Format(@"Delete from bundle_Detail_Art where ukey ='{0}';", arttmp.Rows[i]["ukey"]);
                }
            }
            #endregion

            #region 處理Bundle_Detail_Qty
            row = 0;
            int Qty_old_rowCount = qtytmp.Rows.Count;
            
            foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows) //處理Bundle_Detail_Art
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (row >= Qty_old_rowCount) //新增
                    {
                        Qty_cmd = Qty_cmd + string.Format(
                        @"insert into bundle_Detail_Qty(ID,SizeCode,Qty) 
                    values('{0}','{1}',{2});"
                        , CurrentMaintain["ID"], dr["sizecode"], dr["Qty"]);
                    }
                    else //覆蓋
                    {
                        Qty_cmd = Qty_cmd + string.Format(
                        @"update bundle_Detail_Qty set SizeCode ='{0}',Qty = {1} 
                    Where ukey = {2};", dr["SizeCode"], dr["Qty"], qtytmp.Rows[row]["ukey"]);
                    }
                }
                row++;
            }
            bundle_Detail_Qty_Tb.AcceptChanges();//變更Row Status 狀態
            int Qty_new_rowCount = bundle_Detail_Qty_Tb.Rows.Count;
            if (Qty_old_rowCount > Qty_new_rowCount) //舊的筆數小於新的筆數 表示要先覆蓋再刪除多餘的
            {
                for (int i = Qty_new_rowCount; i < Qty_old_rowCount; i++)
                {
                    Qty_cmd = Qty_cmd + string.Format(@"Delete from bundle_Detail_Qty where ukey ={0};", qtytmp.Rows[i]["ukey"]);
                }
            }
            #endregion
            DualResult upResult;
            if (!MyUtility.Check.Empty(allpart_cmd))
            {
                if (!(upResult = DBProxy.Current.Execute(null, allpart_cmd)))
                {
                    return upResult;
                }
            }
            if (!MyUtility.Check.Empty(Art_cmd))
            {
                if (!(upResult = DBProxy.Current.Execute(null, Art_cmd)))
                {
                    return upResult;
                }
            }
            if (!MyUtility.Check.Empty(Qty_cmd))
            {
                if (!(upResult = DBProxy.Current.Execute(null, Qty_cmd)))
                {
                    return upResult;
                }
            }
            return base.ClickSavePost();
        }
        private void textBox_Cutref_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = textBox_Cutref.Text;
            if (textBox_Cutref.OldValue.ToString() == newvalue) return;
            string cmd = string.Format(
            @"Select a.*,substring(b.Sewline,1,charindex(',',b.Sewline,1)) as Sewline ,b.poid,b.seasonid,b.styleid,b.styleukey,b.factoryid,

            (
               Select Top(1) OrderID
                From Workorder_Distribute WD
                Where a.ukey =WD.workorderukey --and a.orderid=WD.orderid
            ) as Workorder_Distribute_OrderID,

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
                Select Top(1) Article
                From Workorder_Distribute f
                Where a.ukey =f.workorderukey --and a.orderid=f.orderid
            ) as article,
            (
                Select count(id)
                From Workorder_Distribute g 
                Where a.ukey =g.workorderukey and g.OrderID=(Select Top(1) OrderID From Workorder_Distribute WD Where a.ukey =WD.workorderukey)
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
                textBox_LectraCode.Text = "";

                DetailDatas.Clear();
                bundle_Detail_allpart_Tb.Clear();
                bundle_Detail_Art_Tb.Clear();
                bundle_Detail_Qty_Tb.Clear();
                WorkOrder_Ukey = "";
                CurrentMaintain.EndEdit();
                e.Cancel = true;
                return;
            }
            else
            {
                CurrentMaintain["Cutno"] = Convert.ToInt32(cutdr["Cutno"].ToString());
                CurrentMaintain["sewinglineid"] = cutdr["Sewline"].ToString();
                CurrentMaintain["OrderID"] = cutdr["Workorder_Distribute_OrderID"].ToString();    //cutdr["OrderID"].ToString()
                CurrentMaintain["POID"] = cutdr["POID"].ToString();
                CurrentMaintain["PatternPanel"] = cutdr["Fabriccombo"].ToString();
                CurrentMaintain["Sizecode"] = cutdr["Sizecode"].ToString().Substring(0,cutdr["Sizecode"].ToString().Length-1);
                CurrentMaintain["Ratio"] = cutdr["Ratio"].ToString().Substring(0, cutdr["Ratio"].ToString().Length - 1);
                CurrentMaintain["Article"] = cutdr["Article"].ToString();
                CurrentMaintain["Colorid"] = cutdr["Colorid"].ToString();
                CurrentMaintain["Qty"] = cutdr["Qty"];

                CurrentMaintain["LectraCode"] = cutdr["LectraCode"].ToString();
                displayBox_Season.Text = cutdr["Seasonid"].ToString();
                displayBox_Style.Text = cutdr["Styleid"].ToString();
                displayBox_PrintDate.Text = cutdr["Estcutdate"].ToString();

                string cellid = MyUtility.GetValue.Lookup("SewingCell", cutdr["sewline"].ToString()+cutdr["factoryid"].ToString(), "SewingLine", "ID+factoryid");
                CurrentMaintain["SewingCell"] = cellid;

                #region Startno
                int startno = startNo_Function(CurrentMaintain["OrderID"].ToString());
                CurrentMaintain["startno"] = startno;
                #endregion

                string item_cmd = string.Format("Select a.Name from Reason a, Style b where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{0}' and b.ApparelType = a.id", cutdr["styleukey"]);
                string item = MyUtility.GetValue.Lookup(item_cmd, null);
                CurrentMaintain["ITEM"] = item;
                CurrentMaintain["Cutref"] = newvalue;
                WorkOrder_Ukey = cutdr["Ukey"].ToString(); ;
                CurrentMaintain.EndEdit();
            }
            
        }

        private void textBox_orderid_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["Cutref"]) || MyUtility.Check.Empty(CurrentMaintain["POID"])) return;
            Sci.Win.Tools.SelectItem item;
            string cuttingid = MyUtility.GetValue.Lookup("Cuttingsp",CurrentMaintain["POID"].ToString(),"Orders","ID");
            //string selectCommand = string.Format("select b.orderid from workorder a, workorder_distribute b where a.cutref = '{0}' and a.id = '{1}' and a.ukey = b.workorderukey and a.id = b.id and b.id = '{1}'", CurrentMaintain["Cutref"], cuttingid);
            string selectCommand = string.Format(@"select distinct b.orderid 
                                                from workorder a, workorder_distribute b 
                                                where a.cutref = '{0}' and a.id = '{1}' and a.ukey = b.workorderukey"
                                                , CurrentMaintain["Cutref"], cuttingid);
            item = new Sci.Win.Tools.SelectItem(selectCommand, "20", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox_orderid.Text = item.GetSelectedString();
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
                if (cuttingsp.Trim() != cuttingid.Trim())
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
                CurrentMaintain["OrderID"] = newvalue;
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
                            string cellid = MyUtility.GetValue.Lookup("SewingCell", cutdr["sewline"].ToString().Substring(0, 2)+cutdr["Factoryid"].ToString(), "SewingLine", "ID+factoryid");

                            CurrentMaintain["SewingCell"] = cellid;
                        }
                        #endregion
                        #region startno
                        int startno = startNo_Function(CurrentMaintain["OrderID"].ToString());
                        CurrentMaintain["startno"] = startno;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["poid"].ToString(), "Orders", "ID");
            Sci.Production.PublicForm.GarmentList callNextForm =
    new Sci.Production.PublicForm.GarmentList(ukey);
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }

        public int startNo_Function(string orderid) //Start No 計算
        {
            #region startno
            string max_cmd = string.Format("Select isnull(Max(startno+Qty),0) as Start from Bundle Where OrderID = '{0}'", orderid);
            DataTable max_st;
            if (DBProxy.Current.Select(null, max_cmd, out max_st))
            {
                if (max_st.Rows.Count != 0) return Convert.ToInt16(max_st.Rows[0]["Start"]);
                else return 1;
            }
            else
            {
                return 1;
            }
            #endregion

        }
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["ID"] = "";
            CurrentMaintain["Cdate"] = DateTime.Now;
            int startno = startNo_Function(CurrentMaintain["OrderID"].ToString());
            CurrentMaintain["startno"] = startno;
            #region 清除Detail Bundleno,ID 
            foreach (DataRow dr in DetailDatas)
            {
                dr["Bundleno"] = "";
                dr["ID"] = "";
                dr["BundleGroup"] = 0;
            }
            foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows)
            {
                //dr["Bundleno"] = "";
                dr["ID"] = "";
            }
            foreach (DataRow dr in bundle_Detail_Art_Tb.Rows)
            {
                dr["Bundleno"] = "";
                dr["ID"] = "";
            }
            foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows)
            {
                dr["ID"] = "";
            }
            #endregion 
        }

        private void numericBox_Group_Validated(object sender, EventArgs e)
        {
            decimal no = (decimal)numericBox_Group.Value;
            decimal oldvalue = (decimal)numericBox_Group.OldValue;
            decimal nGroup = no - oldvalue;
            foreach (DataRow dr in DetailDatas)
            {
                dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + nGroup;
            }
        }

        private void Generate_Button_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)detailgridbs.DataSource;
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P10_Generate(CurrentMaintain,dt,bundle_Detail_allpart_Tb,bundle_Detail_Art_Tb,bundle_Detail_Qty_Tb);
            frm.ShowDialog(this);
        }
        protected override bool ClickPrint()
        {
            P10_Print p = new P10_Print(this.CurrentDataRow);
            p.ShowDialog();

            return true;

        }

        private void textBox_Article_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            string selectCommand;
            Sci.Win.Tools.SelectItem item;
            if (!EditMode) return;
            if (!MyUtility.Check.Empty(CurrentMaintain["cutref"]))
            {
                selectCommand = string.Format(@"select distinct Article from Workorder_Distribute 
                                                where Article!='' and WorkorderUkey={0}", WorkOrder_Ukey);
                item = new Sci.Win.Tools.SelectItem(selectCommand, "20", this.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                textBox_Article.Text = item.GetSelectedString();
            }
            else
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["Orderid"]))
                {
                    selectCommand = string.Format(@"select distinct article from Order_Qty  where Id='{0}'", CurrentMaintain["Orderid"]);
                    item = new Sci.Win.Tools.SelectItem(selectCommand, "20", this.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    textBox_Article.Text = item.GetSelectedString();
                }
            }
        }

        private void textBox_Article_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = textBox_Article.Text;
            if (textBox_Article.OldValue.ToString() == newvalue) return;
            string sql;
            DataTable dtTEMP;
            if (!MyUtility.Check.Empty(CurrentMaintain["cutref"]))
            {
                sql = string.Format(@"select Article from Workorder_Distribute 
                                                where Article!='' and WorkorderUkey={0} and Article='{1}'", WorkOrder_Ukey, newvalue);
                if (DBProxy.Current.Select(null, sql, out dtTEMP))
                {
                    if (dtTEMP.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("<Article> can't find !!");
                        textBox_Article.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["Orderid"]))
                {
                    sql = string.Format(@"select article from Order_Qty  where Id='{0}' and Article='{1}'", CurrentMaintain["Orderid"], newvalue);
                    if (DBProxy.Current.Select(null, sql, out dtTEMP))
                    {
                        if (dtTEMP.Rows.Count == 0)
                        {
                            MyUtility.Msg.WarningBox("<Article> can't find !!");
                            textBox_Article.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }

        }

        private void txtLine_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode) return;
            string sql = string.Format(@"Select ID,FactoryID,Description  From SewingLine  
                                        where FactoryID in (select ID from Factory where MDivisionID='{0}')", Sci.Env.User.Keyword);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "2,8,16", this.Text, false, ",");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtLine.Text = item.GetSelectedString();
        }

        private void txtLine_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = txtLine.Text;
            if (txtLine.OldValue.ToString() == newvalue) return;
            string sql = string.Format(@"Select ID  From SewingLine  
                    where FactoryID in (select ID from Factory where MDivisionID='{0}') and ID='{1}'", Sci.Env.User.Keyword, newvalue);
            string tmp = MyUtility.GetValue.Lookup(sql);
            if (string.IsNullOrWhiteSpace(tmp))
            {
                txtLine.Text = "";
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Sewing Line> : {0} not found!!!", newvalue));
                return;
            }
        }


        //private void textBox_Line_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (!this.EditMode) return;
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    {

        //        string sqlcmd1 = "語法需有文件確認才可以填上去";
        //        SelectItem item1 = new SelectItem(sqlcmd1, "30", textBox_Line.Text.ToString());
        //        DialogResult result1 = item1.ShowDialog();
        //        if (result1 == DialogResult.Cancel) { return; }
        //        //
        //        textBox_Line.Text = item1.GetSelectedString();
        //    }
        //}
    }
}
