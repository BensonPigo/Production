﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;

namespace Sci.Production.Subcon
{
    public partial class P03 : Sci.Win.Tems.Input6
    {
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MdivisionID = '" + Sci.Env.User.Keyword + "'";
            txtmfactory.ReadOnly = true;            
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["bundleno"] = "";
            CurrentDetailData["qty"] = 0;
        }

        private void txtartworktype_ftyArtworkType_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;
            //值有變更時表身資料清空,從最後一列
            if ((o.Text != o.OldValue) && this.EditMode)
            {
                DataTable dt = (DataTable)detailgridbs.DataSource;
                for (int i = dt.Rows.Count-1 ; i >= 0  ;i--){
                    DataRow dr  = dt.Rows[i];
                    dr.Delete();
                }
            }
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            CurrentMaintain["Status"] = "New";
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString().ToUpper() != "NEW")
            {
                MyUtility.Msg.WarningBox("Data is Confirmed, can't be deleted.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("farmout", "remark", CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }
            
            return base.ClickEditBefore();
        }
    
        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            detailgridbs.EndEdit();

            #region 必輸檢查
            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateDate.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type >  can't be empty!", "Warning");
                txtartworktype_ftyArtworkType.Focus();
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory.Focus();
                return false;
            }
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("qty = 0"))
            {
                row.Delete();
            }


            if (((DataTable)detailgridbs.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            DataRow[] drchk = ((DataTable)detailgridbs.DataSource).Select("[orderid] ='' or [artworkpoid] ='' or [orderid] is null or [artworkpoid] is null ");
            if (drchk.Length > 0)
            {
                MyUtility.Msg.WarningBox("Detail of SP# & POID can't be empty", "Warning");
                return false;
            }

            //取單號： 
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(factorykeyword + "FO", "FarmOut", (DateTime)CurrentMaintain["issuedate"]);
            }
            #region 加總明細Qty是否=0

            object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(qty)", "");
            if (MyUtility.Convert.GetDecimal(detail_a)==0)
            {
                MessageBox.Show("Farm In# Detail Qty can't all be zero.");
                return false;
            }
            

            #endregion
            #region 加總明細金額至表頭

            object a = ((DataTable)detailgridbs.DataSource).Compute("sum(Qty)", "");
            CurrentMaintain["TotalQty"] = MyUtility.Math.Round((decimal)a);
            #endregion
            return base.ClickSaveBefore();
        }

        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string id = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select 
BundleNo, OrderID, styleID, ArtworkPoID, ArtworkID, PatternCode, PatternDesc, ArtworkPoQty, OnHand, 
(artworkpoqty - onhand) Variance, 
Qty, 
(artworkpoqty - onhand - qty) BalQty ,
Ukey ,id, artworkpo_detailukey
from FarmOut_Detail WITH (NOLOCK) 
outer apply(
	select styleID
	from orders WITH (NOLOCK) 
	where orders.id = FarmOut_Detail.Orderid	
) styleID where id='{0}'", id);

            return base.OnDetailSelectCommandPrepare(e);
        }

        // grid 加工填值
        //protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        //{
        //    if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
        //    {
        //        (e.Details).Columns.Add("StyleId", typeof(String));
        //        (e.Details).Columns.Add("Variance", typeof(decimal));
        //        (e.Details).Columns.Add("BalQty", typeof(decimal));

        //        foreach (DataRow dr in e.Details.Rows)
        //        {
        //            dr["Variance"] = (decimal)dr["artworkpoqty"] - (decimal)dr["onhand"];
        //            dr["BalQty"] = (decimal)dr["artworkpoqty"] - (decimal)dr["onhand"] - (decimal)dr["qty"];
        //            DataTable order_dt;
        //            DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders where id='{0}'", dr["orderid"].ToString()), out order_dt);
        //            if (order_dt.Rows.Count == 0)
        //                continue;
        //            dr["StyleId"] = order_dt.Rows[0]["styleid"].ToString();

        //        }
        //    }
        //    return base.OnRenewDataDetailPost(e);
        //}

        //refresh

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            
            txtartworktype_ftyArtworkType.Enabled = !this.EditMode || IsDetailInserting;
           
            this.txtmfactory.ReadOnly = true;

            #region -- 加總明細金額，顯示於表頭 --
            if (!(CurrentMaintain == null))
            {
                decimal x = 0;
                foreach (DataRow drr in ((DataTable)detailgridbs.DataSource).Rows)
                {
                    x += (decimal)drr["qty"];
                }
               
                Console.WriteLine("get {0}", x);
              
                numTotalQty.Text= x.ToString();
            }
            #endregion
            #region Status Label
            labelConfirmed.Text = CurrentMaintain["Status"].ToString();
            #endregion
            #region Batch Import
            btnImportFromRealTime.Enabled = this.EditMode;
            #endregion

        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region orderid帶styleid
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellValidating += (s, e) =>
            {
                if (e.FormattedValue.ToString() == CurrentDetailData["OrderID"].ToString()) return;
                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    CurrentDetailData["OrderID"] = e.FormattedValue;
                    CurrentDetailData["StyleID"] = MyUtility.GetValue.Lookup(string.Format("select styleid from orders WITH (NOLOCK) where id ='{0}'", e.FormattedValue));
                    CurrentDetailData["ArtworkPoID"] = "";
                    CurrentDetailData["ArtworkId"] = "";
                    CurrentDetailData["PatternCode"] = "";
                    CurrentDetailData["PatternCode"] = "";
                    CurrentDetailData["PatternDesc"] = "";
                    CurrentDetailData["ArtworkPoQty"] = 0;
                    CurrentDetailData["OnHand"] = 0;
                    CurrentDetailData["Variance"] = 0;
                    CurrentDetailData["qty"] = 0;
                    CurrentDetailData["BalQty"] = 0;
                    CurrentDetailData["ArtworkPo_DetailUkey"] = DBNull.Value;
                }
            };
            #endregion

            #region POID#右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(CurrentMaintain["artworktypeid"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill Artwork Type first");
                        this.txtartworktype_ftyArtworkType.Focus();
                        return;
                    }

                    if (MyUtility.Check.Empty(dr["OrderID"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill SP# first");
                        return;
                    }

                   

                    if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
                    {
                        
                        string sqlcmd = string.Format(@"SELECT B.id
                                                        ,b.artworktypeid
                                                        ,b.artworkid
                                                        ,b.patterncode
                                                        ,b.patterndesc
                                                        ,b.poqty
                                                        ,b.farmout
                                                        ,b.ukey
                                                        ,A.Status
                                                        FROM ArtworkPO A WITH (NOLOCK) inner join ArtworkPO_Detail B WITH (NOLOCK) on A.ID = B.ID
                                                        WHERE a.mdivisionid = '{2}'
                                                        AND A.ApvName IS NOT NULL
                                                        AND A.Closed = 0
                                                        AND B.OrderID ='{0}'
                                                        AND B.ArtworkTypeID = '{1}' order by B.ID", dr["OrderID"], CurrentMaintain["artworktypeid"],Sci.Env.User.Keyword);
                        Sci.Win.Tools.SelectItem item
                            = new Sci.Win.Tools.SelectItem(sqlcmd, "13,13,13,13,15,5,5,0", dr["artworkpoID"].ToString(), "POID,Artwork Type,Artwork,Cutpart,Cutpart Name,PoQty,FarmOut,Ukey");
                        item.Width = 1024;
                        item.Height = 480;

                        DialogResult returnResult = item.ShowDialog();
                        IList<DataRow> selectedData = item.GetSelecteds();
                        if (returnResult == DialogResult.Cancel) { return; }
                        if ((selectedData[0])["Status"].ToString() =="New")
                        {
                            MyUtility.Msg.WarningBox("PO is not confirmed yet");
                        } 
                        else if (selectedData.Count > 0)
                        {
                            dr["artworkpoID"] = item.GetSelectedString();
                            dr["artworkid"] = (selectedData[0])["artworkid"].ToString();
                            dr["patterncode"] = (selectedData[0])["patterncode"].ToString();
                            dr["patterndesc"] = (selectedData[0])["patterndesc"].ToString();
                            dr["artworkpoqty"] = (selectedData[0])["poqty"].ToString();
                            dr["onhand"] = (selectedData[0])["farmout"];
                            dr["ArtworkPo_DetailUkey"] = (selectedData[0])["ukey"].ToString();
                            dr["qty"] = (decimal)(selectedData[0])["poqty"] - (decimal)(selectedData[0])["farmout"];
                        }
                        
                    }
                }
            };
            #endregion
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("BundleNo", header: "Bundle No", width: Widths.AnsiChars(10), iseditingreadonly: true)  //0
            .CellOrderId("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: ts)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(6), iseditingreadonly: true)    //2
            .Text("ArtworkPoID", header: "POID", settings: ts4, iseditingreadonly: true, width: Widths.AnsiChars(13))   //3
            .Text("ArtworkId", header: "Artwork", iseditingreadonly: true)   //4
            .Text("PatternCode", header: "Cutpart ID", iseditingreadonly: true)    //5
            .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: true)//6
            .Numeric("ArtworkPoQty", header: "P/O Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)    //7
            .Numeric("OnHand", header: "Accum. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true) //8
            .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(5), iseditingreadonly: true)   //9
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5))     //10
            .Numeric("BalQty", header: "Bal. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true);  //11

            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns["OrderID"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["ArtworkPoID"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if ((!(Prgs.GetAuthority(Env.User.UserID)) && CurrentMaintain["handle"].ToString() != Env.User.UserID))
            {
                MyUtility.Msg.WarningBox("Only Handle or Leader have authority to Confirm!");
                return;
            }
            String sqlcmd, sqlcmd2 = "", sqlcmd3 = "";
            DualResult result, result2;
            DataTable datacheck;
            sqlcmd = string.Format(@"select a.id from artworkpo a WITH (NOLOCK) , farmout_detail b WITH (NOLOCK) 
                            where a.id = b.artworkpoid and a.Status = 'Closed' and b.id = '{0}'", CurrentMaintain["id"]);


            string ids = "", bundlenos = "";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These POID <{0}> already closed, can't Confirmed", ids));
                return;
            }

            sqlcmd = string.Format(@"select b.id,b.bundleno 
from farmin a WITH (NOLOCK) inner join farmin_detail c WITH (NOLOCK) on a.id = c.id
inner join farmout_detail b WITH (NOLOCK) on b.bundleno = c.bundleno and b.ArtworkID = c.ArtworkID
where b.bundleno !='' and b.id = '{0}'and a.artworktypeid = '{1}'", CurrentMaintain["id"], CurrentMaintain["artworktypeid"]);

            ids = "";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                    bundlenos += dr[1].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These Bundle# <{0}> already exist in farm in data <{1}> , can't Confirmed", bundlenos, ids));
                return;
            }

            #region Confirmed 提示是否超過po qty
            if (CurrentMaintain["status"].ToString().ToUpper() == "NEW")
            {
                ids = "";
                foreach (var dr in DetailDatas)
                {
                    sqlcmd = string.Format("select * from artworkpo_detail WITH (NOLOCK) where ukey = '{0}'", dr["artworkpo_detailukey"]);
                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {
                        if ((decimal)dr["qty"] + (decimal)datacheck.Rows[0]["farmout"] > (decimal)datacheck.Rows[0]["poqty"])
                        {
                            ids += string.Format("{0}-{1}-{2}-{3}-{4} is over PO Qty", datacheck.Rows[0]["id"], datacheck.Rows[0]["orderid"],
                                datacheck.Rows[0]["artworktypeid"], datacheck.Rows[0]["artworkid"], datacheck.Rows[0]["patterncode"]) + Environment.NewLine;
                        }
                    }
                }
                if (!MyUtility.Check.Empty(ids))
                {
                    MyUtility.Msg.WarningBox(ids);
                }
            }
            #endregion

            // update farmout status
            if (CurrentMaintain["Status"].ToString().ToUpper() == "NEW")
            {
                sqlcmd3 = string.Format("update Farmout set Status = 'Confirmed' , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }

            // update artworkpo_detail farmout
            DataTable detailgroup;
            sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmout_detail b WITH (NOLOCK) 
                                    where b.id ='{0}'
                                    group by b.artworkpo_detailukey ", CurrentMaintain["id"]);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out detailgroup)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

            if (detailgroup.Rows.Count > 0)
            {
                foreach (DataRow dr in detailgroup.Rows)
                {

                    sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmout a WITH (NOLOCK) , farmout_detail b WITH (NOLOCK) 
                                    where a.id = b.id  and a.Status ='Confirmed' and b.artworkpo_detailukey ='{0}'
                                    group by b.artworkpo_detailukey ", dr["artworkpo_detailukey"]);

                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {
                         sqlcmd2 += string.Format("update artworkpo_detail set farmout = {0} where ukey = '{1}';"
                                + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] + (decimal)dr["qty"], dr["artworkpo_detailukey"]);
                        
                    }
                    else
                    {
                         sqlcmd2 += string.Format("update artworkpo_detail set farmout = {0} where ukey = '{1}';"
                                + Environment.NewLine, (decimal)dr["qty"], dr["artworkpo_detailukey"]);
                    }
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlcmd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlcmd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirm successful");
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
         
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if ((!(Prgs.GetAuthority(Env.User.UserID)) && CurrentMaintain["handle"].ToString() != Env.User.UserID))
            {
                MyUtility.Msg.WarningBox("Only Handle & Leader have authority to UnConfirm!");
                return;
            }

            String sqlcmd, sqlcmd2 = "", sqlcmd3 = "";
            DualResult result, result2;
            DataTable datacheck;
            sqlcmd = string.Format(@"select a.id from artworkpo a WITH (NOLOCK) , farmout_detail b WITH (NOLOCK) 
                            where a.id = b.artworkpoid and a.Status = 'Closed' and b.id = '{0}'", CurrentMaintain["id"]);

            string ids = "", bundlenos = "";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These POID <{0}> already closed, can't UnConfirmed", ids));
                return;
            }

            sqlcmd = string.Format(@"select b.id,b.bundleno 
                                    from farmin a WITH (NOLOCK) inner join farmin_detail b WITH (NOLOCK) on b.id = a.id 
                                    inner join farmout_detail c WITH (NOLOCK) on b.bundleno = c.bundleno and b.artworkid = c.artworkid
                                    where c.bundleno !='' and c.id = '{0}'
                                    and a.artworktypeid = '{1}'", 
                                    CurrentMaintain["id"], CurrentMaintain["artworktypeid"]);

            ids = "";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                    bundlenos += dr[1].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These Bundle# <{0}> already exist in farm-in data <{1}> , can't UnConfirmed", bundlenos, ids));
                return;
            }

            string sqlcmdio = string.Format(@"
                            select c.FarmOut,c.FarmIn,b.id,b.orderid,a.artworktypeid,b.artworkid,b.patterncode,c.Farmin
                            from FarmOut a WITH (NOLOCK) 
                            inner join FarmOut_Detail b WITH (NOLOCK) on a.id=b.id
                            inner join ArtworkPO_Detail c WITH (NOLOCK) on b.ArtworkPo_DetailUkey=c.Ukey
                            where a.Id='{0}' and a.artworktypeid = '{1}'"
                            , CurrentMaintain["id"], CurrentMaintain["artworktypeid"]);
            StringBuilder mids = new StringBuilder();
            if (!(result = DBProxy.Current.Select(null, sqlcmdio, out datacheck)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            if (datacheck.Rows.Count > 0)
            {
                DataRow[] Srows;
                foreach (var dr in DetailDatas)
                {
                    Srows = datacheck.Select(string.Format("OrderID = '{0}'",dr["orderid"].ToString()));
                    if ((decimal)Srows[0]["farmout"] - (decimal)dr["qty"] < (decimal)Srows[0]["farmin"])
                    {
                        mids.Append(string.Format("{0}-{1}-{2}-{3}-{4} can't less farm in qty {5} \n", Srows[0]["id"], Srows[0]["orderid"], Srows[0]["artworktypeid"], Srows[0]["artworkid"], Srows[0]["patterncode"], Srows[0]["farmin"]));
                    }
                }

                if (!MyUtility.Check.Empty(mids.ToString()))
                {
                    MyUtility.Msg.WarningBox(mids.ToString());
                    return;
                }
            }

            // update farmout status
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unconfirm it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            sqlcmd3 = string.Format("update Farmout set Status = 'New', editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            // update artworkpo_detail farmout
            DataTable detailgroup;
            sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmout_detail b WITH (NOLOCK) 
                                    where b.id ='{0}'
                                    group by b.artworkpo_detailukey ", CurrentMaintain["id"]);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out detailgroup)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

            if (detailgroup.Rows.Count > 0)
            {
                foreach (DataRow dr in detailgroup.Rows)
                {

                    sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmout a WITH (NOLOCK) , farmout_detail b WITH (NOLOCK) 
                                    where a.id = b.id  and a.Status ='Confirmed' and b.artworkpo_detailukey ='{0}'
                                    group by b.artworkpo_detailukey ", dr["artworkpo_detailukey"]);

                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {

                        sqlcmd2 += string.Format("update artworkpo_detail set farmout = {0} where ukey = '{1}';"
                            + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] - (decimal)dr["qty"], dr["artworkpo_detailukey"]);

                    }
                    else
                    {

                        sqlcmd2 += string.Format("update artworkpo_detail set farmout = {0} where ukey = '{1}';"
                            + Environment.NewLine, 0m, dr["artworkpo_detailukey"]);

                    }
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlcmd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlcmd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirm successful");
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
           
        }

        // P03_ImportFrom Real Time
        private void btnImportFromRealTime_Click(object sender, EventArgs e)
        {
            //P03_Import TrimCardPrint = new P03_Import();
            //P03_Import.ShowDialog(this);
        }

    }
}
