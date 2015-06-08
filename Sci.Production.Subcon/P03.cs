using System;
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
            this.DefaultFilter = string.Format("FactoryID = '{0}'" , Sci.Env.User.Factory);
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["bundleno"] = "";
            CurrentDetailData["qty"] =0;
        }

        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();
            }
        }

        // 新增時預設資料
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            CurrentMaintain["Encode"] = 0;
            
        }

        // delete前檢查

        protected override bool OnDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (!myUtility.Empty(dr["encode"].ToString()) || dr["encode"].ToString().ToUpper() == "TRUE")
            {
                MessageBox.Show("Data is encoded, can't be deleted.", "Warning");
                return false;
            }

            return base.OnDeleteBefore();
        }

        // edit前檢查
        protected override bool OnEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["encode"].ToString().ToUpper() == "TRUE")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("farmout", "remark", dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.OnEditBefore();
        }

        // save前檢查 & 取id
        protected override bool OnSaveBefore()
        {
            detailgridbs.EndEdit();

            #region 必輸檢查
            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MessageBox.Show("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MessageBox.Show("< Artwork Type >  can't be empty!", "Warning");
                txtartworktype_fty1.Focus();
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MessageBox.Show("< Handle >  can't be empty!", "Warning");
                txtuser1.TextBox1.Focus();
                return false;
            }
            #endregion

            foreach( DataRow row in ((DataTable)detailgridbs.DataSource).Select("qty = 0" ))
            {
                ((DataTable)detailgridbs.DataSource).Rows.Remove(row);
            }


            if (((DataTable)detailgridbs.DataSource).Rows.Count == 0)
            {
                MessageBox.Show("Detail can't be empty", "Warning");
                return false;
            }

            DataRow[] drchk = ((DataTable)detailgridbs.DataSource).Select("[orderid] ='' or [artworkpoid] ='' or [orderid] is null or [artworkpoid] is null ");
            if (drchk.Length > 0)
            {
                MessageBox.Show("Detail of SP# & POID can't be empty", "Warning");
                return false;
            }


            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                CurrentMaintain["id"] = Sci.myUtility.GetID(ProductionEnv.Keyword + "FO", "FarmOut", (DateTime)CurrentMaintain["issuedate"]);
            }

            #region 加總明細Qty至表頭
            
            object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(qty)", "");
            CurrentMaintain["totalqty"] = (decimal)detail_a;
           
            #endregion

            return base.OnSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                (e.Details).Columns.Add("StyleId", typeof(String));
                (e.Details).Columns.Add("Variance", typeof(decimal));
                (e.Details).Columns.Add("BalQty", typeof(decimal));
                
                foreach (DataRow dr in e.Details.Rows)
                {
                    dr["Variance"] = (decimal)dr["artworkpoqty"] - (decimal)dr["onhand"];
                    dr["BalQty"] = (decimal)dr["artworkpoqty"] - (decimal)dr["onhand"] - (decimal)dr["qty"];
                    DataTable order_dt;
                    DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders where id='{0}'", dr["orderid"].ToString()), out order_dt);
                    if (order_dt.Rows.Count == 0)
                        continue;
                    dr["StyleId"] = order_dt.Rows[0]["styleid"].ToString();
                    
                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            button1.Enabled = !this.EditMode &&
                                (Prgs.GetAuthority(Env.User.UserID) ||
                                CurrentMaintain["handle"].ToString() == Env.User.UserID);

            if (myUtility.Empty(CurrentMaintain["encode"]) || CurrentMaintain["encode"].ToString().ToUpper() == "False")
            {
                button1.Text = "Encode";
                button1.ForeColor = Color.Black;
            }
            else
            {
                button1.Text = "Amend";
                button1.ForeColor = Color.Blue;
            }
                              
            txtartworktype_fty1.Enabled = !this.EditMode || IsDetailInserting;

            #region Batch Import
            button2.Enabled = this.EditMode;
            #endregion
            
        }
       
        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region orderid帶styleid
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellValidating += (s, e) =>
            {

                if (!(this.EditMode) || this.IsDetailInserting) return;
                if (!(myUtility.Empty(e.FormattedValue)) && e.FormattedValue.ToString().TrimEnd() != ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex)["orderid"].ToString().TrimEnd())
                {
                    CurrentDetailData["styleid"] = myUtility.Seek(string.Format("select styleid from orders where id ='{0}'", e.FormattedValue));
                    CurrentDetailData["artworkid"] = "";
                    CurrentDetailData["patterndesc"] = "";
                    CurrentDetailData["artworkpoqty"] = 0;
                    CurrentDetailData["onhand"] = 0;
                    CurrentDetailData["ArtworkPo_DetailUkey"] = DBNull.Value;
                    CurrentDetailData["qty"] = 0;
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
                    if (myUtility.Empty(CurrentMaintain["artworktypeid"]))
                    {
                        MessageBox.Show("Please fill Artwork Type first");
                        this.txtartworktype_fty1.Focus();
                        return;
                    }
                    
                    if (myUtility.Empty(dr["OrderID"]))
                    {
                        MessageBox.Show("Please fill SP# first");
                        return;
                    }
                    if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1 )
                    {
                        
                        string sqlcmd = string.Format(@"SELECT B.id
                                                        ,b.artworktypeid
                                                        ,b.artworkid
                                                        ,b.patterncode
                                                        ,b.patterndesc
                                                        ,b.poqty
                                                        ,b.farmout
                                                        ,b.ukey
                                                        FROM ArtworkPO A, ArtworkPO_Detail B
                                                        WHERE A.ID = B.ID
                                                        AND A.ApvName IS NOT NULL
                                                        AND A.Closed = 0
                                                        AND B.OrderID ='{0}'
                                                        AND B.ArtworkTypeID = '{1}' order by B.ID",dr["OrderID"].ToString(), CurrentMaintain["artworktypeid"].ToString());
                        Sci.Win.Tools.SelectItem item
                            = new Sci.Win.Tools.SelectItem(sqlcmd, "13,13,13,13,15,5,5,0", dr["artworkpoID"].ToString(), "POID,Artwork Type,Artwork,Cutpart,Cutpart Name,PoQty,FarmOut,Ukey");
                        item.Width = 1024;
                        item.Height = 480;

                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        dr["artworkpoID"] = item.GetSelectedString();
                        IList<DataRow> selectedData = item.GetSelecteds();
                        if (selectedData.Count > 0)
                        {
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
            .CellOrderId("OrderID", header: "SP#", width: Widths.AnsiChars(13),settings:ts)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(6), iseditingreadonly: false)    //2
            .Text("ArtworkPoID", header: "POID", settings: ts4, iseditingreadonly: true, width: Widths.AnsiChars(13))   //3
            .Text("ArtworkId", header: "Artwork", iseditingreadonly: false)   //4
            .Text("PatternCode", header: "Cutpart ID", iseditingreadonly: false)    //5
            .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: false)//6
            .Numeric("ArtworkPoQty", header: "P/O Qty", width: Widths.AnsiChars(5), iseditingreadonly: false)    //7
            .Numeric("OnHand", header: "Accum. Qty", width: Widths.AnsiChars(5), iseditingreadonly: false) //8
            .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(5), iseditingreadonly: false)   //9
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5) )     //10
            .Numeric("BalQty", header: "Bal. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true);  //11
            
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns[1].DefaultCellStyle.BackColor = Color.Pink; 
            detailgrid.Columns[3].DefaultCellStyle.BackColor = Color.Pink;  
            detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink; 
            #endregion
        }

        // Encode
        private void button1_Click(object sender, EventArgs e)
        {
            String sqlcmd, sqlcmd2 = "", sqlcmd3 = "";
            DualResult result,result2;
            DataTable datacheck;
            sqlcmd = string.Format(@"select a.id from artworkpo a, farmout_detail b 
                            where a.id = b.artworkpoid and a.closed = 1 and b.id = '{0}'",CurrentMaintain["id"]);


            string ids = "", bundlenos="";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                }
                MessageBox.Show(String.Format("These POID <{0}> already closed, can't encode/amend", ids));
                return;
            }

            sqlcmd = string.Format(@"select b.id,b.bundleno 
                                    from farmin a,farmin_detail b, farmout_detail c 
                                    where a.id = b.id 
                                    and b.bundleno = c.bundleno 
                                    and c.id = '{0}'
                                    and a.artworktypeid = '{1}'", CurrentMaintain["id"], CurrentMaintain["artworktypeid"]);


            ids = "";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                    bundlenos += dr[1].ToString() + ",";
                }
                MessageBox.Show(String.Format("These Bundle# <{0}> already exist in farm-in data <{1}> , can't encode/amend",bundlenos, ids));
                return;
            }

            // Encode提示是否超過po qty ， amend
            if (CurrentMaintain["Encode"].ToString().ToUpper() == "FALSE")
            {
                ids = "";
                foreach (var dr in DetailDatas)
                {
                    sqlcmd = string.Format("select * from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"]);
                    if (!(result = DBProxy.Current.Select(null, sqlcmd,out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {
                        if ((decimal)dr["qty"] + (decimal)datacheck.Rows[0]["farmout"] > (decimal)datacheck.Rows[0]["poqty"])
                        {
                            ids += string.Format("{0}-{1}-{2}-{3}-{4} is over PO Qty", datacheck.Rows[0]["id"], datacheck.Rows[0]["orderid"], datacheck.Rows[0]["artworktypeid"], datacheck.Rows[0]["artworkid"], datacheck.Rows[0]["patterncode"]) + Environment.NewLine;
                        }
                    }
                }
                if (!myUtility.Empty(ids))
                {
                    MessageBox.Show(ids);
                }
            }
            else
            {
                ids = "";
                foreach (var dr in DetailDatas)
                {
                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {
                        if ((decimal)datacheck.Rows[0]["farmout"] - (decimal)dr["qty"] < (decimal)datacheck.Rows[0]["farmin"])
                        {
                            ids += string.Format("{0}-{1}-{2}-{3}-{4} can't less farm in qty {5}", datacheck.Rows[0]["id"], datacheck.Rows[0]["orderid"], datacheck.Rows[0]["artworktypeid"], datacheck.Rows[0]["artworkid"], datacheck.Rows[0]["patterncode"], datacheck.Rows[0]["farmin"]) + Environment.NewLine;
                        }
                    }
                }
                if (!myUtility.Empty(ids))
                {
                    MessageBox.Show(ids);
                }
            }

            

            // update farmout status
            if (CurrentMaintain["Encode"].ToString().ToUpper()=="FALSE")
            {
                sqlcmd3 = string.Format("update Farmout set encode = 1 , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }
            else
            {
                DialogResult dResult = MessageBox.Show("Do you want to amend it?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dResult.ToString().ToUpper() == "NO") return;
                sqlcmd3 = string.Format("update Farmout set encode = 0, editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }

            // update artworkpo_detail farmout
            foreach (var dr in DetailDatas)
            {
                sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmout a, farmout_detail b
                                    where a.id = b.id  and a.encode =1 and b.artworkpo_detailukey ='{0}'
                                    group by b.artworkpo_detailukey ", dr["artworkpo_detailukey"]);
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }
                if (datacheck.Rows.Count > 0)
                {
                    if (CurrentMaintain["Encode"].ToString().ToUpper() == "FALSE")
                    {
                        sqlcmd2 += string.Format("update artworkpo_detail set farmout = {0} where ukey = '{1}';" 
                            + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] + (decimal)dr["qty"], dr["artworkpo_detailukey"]);
                    }
                    else
                    {
                        sqlcmd2 += string.Format("update artworkpo_detail set farmout = {0} where ukey = '{1}';" 
                            + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] - (decimal)dr["qty"], dr["artworkpo_detailukey"]);
                    }
                }
                else
                {
                    if (CurrentMaintain["Encode"].ToString().ToUpper() == "FALSE")  // encode
                    {
                        sqlcmd2 += string.Format("update artworkpo_detail set farmout = {0} where ukey = '{1}';" 
                            + Environment.NewLine, (decimal)dr["qty"], dr["artworkpo_detailukey"]);
                    }
                    else//amend
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
                        ShowErr(sqlcmd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlcmd2)))
                    {
                        ShowErr(sqlcmd2, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MessageBox.Show("Encode successful");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            RenewData();
            OnDetailEntered();
        }
    }
}
