using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Win;
using Sci.Win.Tools;
using Ict;
using Ict.Data;
using Sci.Production.Class;
using Sci.Production.PublicPrg;
using System.Collections;
using System.Transactions;

namespace Sci.Production.Thread
{
    public partial class P01_Detail : Sci.Win.Subs.Base
    {
        private DataRow detail,master;
        private string loginID = Sci.Env.User.UserID;
        private string styleUkey,combdetail_id;
        private DataTable gridTable,tbArticle;
        private DataRow masterRow,detailRow;
        public P01_Detail(DataRow masterrow,DataRow detailrow,bool editmode)
        {
            InitializeComponent();
            this.masterRow = masterrow;
            this.detailRow = detailrow;
            styleUkey = masterrow["Ukey"].ToString();
            detail = detailrow;
            master = masterrow;
            displayBox1.Value = detailrow["Machinetypeid"].ToString();
            displayBox2.Value = masterrow["id"].ToString();
            displayBox3.Value = masterrow["Seasonid"].ToString();
            displayBox4.Value = detailrow["ThreadCombID"].ToString();
            combdetail_id = detailrow["id"].ToString();
            
            button1.Enabled = Sci.Production.PublicPrg.Prgs.GetAuthority(loginID, "P01.Thread Color Combination", "CanEdit");
            button1.Visible = Sci.Production.PublicPrg.Prgs.GetAuthority(loginID, "P01.Thread Color Combination", "CanEdit");
            //建立Gird
            generateGrid();

        }
        
        private void generateGrid() //建立Gird
        {
            string articleSql = string.Format("Select Article from Style_Article where styleukey='{0}'", styleUkey);
            StringBuilder art_col = new StringBuilder();
            if (!art_col.Empty())
            {
                art_col.Clear();
            }
            

            string sql;
            DualResult dResult = DBProxy.Current.Select(null, articleSql, out tbArticle);
            if (!dResult)
            {
                ShowErr(articleSql, dResult);
            }
            else
            {
                if (tbArticle.Rows.Count == 0) 
                {
                    MyUtility.Msg.WarningBox("Article not found");
                    return;
                }
                foreach (DataRow dr in tbArticle.Rows)
                {
                    art_col.Append(string.Format(@",[{0}]", dr["article"].ToString().Trim()));
                }
            }
           
            #region Grid header,Column  建立 使用自行新增另一種寫法
            //grid1.IsEditingReadOnly = false;
            //grid1.Columns.Add("SEQ", "SEQ");
            //grid1.Columns[0].DataPropertyName = "SEQ";
            //grid1.Columns[0].Width = 50;

            //grid1.Columns.Add("ThreadLocationName", "Thread Location");
            //grid1.Columns[1].DataPropertyName = "ThreadLocationName";
            //grid1.Columns[1].Width = 100;

            //grid1.Columns.Add("UseRatio", "Use Ratio");
            //grid1.Columns[2].DataPropertyName = "UseRatio";
            //grid1.Columns[2].Width = 150;

            //Ict.Win.UI.DataGridViewTextBoxColumn refno_col = new Ict.Win.UI.DataGridViewTextBoxColumn();

            //refno_col.HeaderText = "Refno";
            //refno_col.DataPropertyName = "Refno";
            //grid1.Columns.Add(refno_col);

            //grid1.Columns[3].Width = 130;
            //grid1.Columns[3].DefaultCellStyle.BackColor = Color.Pink;
            //refno_col.EditingMouseDown += (s, e) =>
            //{
            //    e.EditingControl.Text = "XXX";
            //};

            //for (int i = 0; i < tbArticle.Rows.Count; i++)
            //{
            //    //grid1.Columns.Add(tbArticle.Rows[i]["article"].ToString().Trim(), tbArticle.Rows[i]["article"].ToString().Trim());
            //    //grid1.Columns[i+4].DataPropertyName = tbArticle.Rows[i]["article"].ToString().Trim();
            //    grid1.Columns[i + 4].Width = 120;
            //    grid1.Columns[i + 4].DefaultCellStyle.BackColor = Color.Pink;

            //}

            #endregion

            #region Grid header,Column  Gerenator建立 可用Setting
            DataGridViewGeneratorTextColumnSettings refno_col = celllocalitem.GetGridCell("Thread", null);
            DataGridViewGeneratorTextColumnSettings threadcolor_col = new DataGridViewGeneratorTextColumnSettings();
            threadcolor_col.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow row = grid1.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele;
                    string header = grid1.Columns[e.ColumnIndex].HeaderText;
                    sele = new SelectItem("Select id, description From ThreadColor where junk=0", "23", row[header].ToString(), false, ",");

                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    row[header] = sele.GetSelectedString();
                    row.EndEdit();
                }
            };
            threadcolor_col.CellValidating += (s, e) =>
            {
                // Parent form 若是非編輯狀態就 return 
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                DataRow row = grid1.GetDataRow<DataRow>(e.RowIndex);
                string header = grid1.Columns[e.ColumnIndex].HeaderText;
                String oldValue = row[header].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                sql = string.Format("Select id, description From ThreadColor where junk=0 and id='{0}'", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql))
                    {
                        MyUtility.Msg.WarningBox(string.Format("<Thread Color> : {0} not found!!!", newValue));
                        row[header] = "";
                        row.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
            };

            Helper.Controls.Grid.Generator(grid1)
                .Text("SEQ", header: "SEQ", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("ThreadLocation", header: "Thread Location", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("UseRatioNumeric", header: "UseRatioNumeric", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Allowance", header: "Allowance", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.Auto(true), settings: refno_col);
                //前4個是MachineType_ThreadRatio,後一個Refno是ThreadColorComb_Detail

            grid1.Columns[4].DefaultCellStyle.BackColor = Color.Pink;

            for(int i = 0;i < tbArticle.Rows.Count;i++)
            {
                Helper.Controls.Grid.Generator(grid1)
                    .Text(tbArticle.Rows[i]["article"].ToString().Trim(), header: tbArticle.Rows[i]["article"].ToString().Trim(), width: Widths.Auto(true), settings: threadcolor_col);

                grid1.Columns[i + 5].DefaultCellStyle.BackColor = Color.Pink;
            }
            #endregion

            #region 資料建立
            StringBuilder op = new StringBuilder();
            for (int i = 0; i < tbArticle.Rows.Count; i++)
            {
                op.Append(string.Format(@"
                        outer apply (
	                        select t.ThreadColorid as '{0}'
	                        from ThreadColorComb_Detail t
	                        where Machinetypeid=MT.ID
                            and SEQ = MT.SEQ
	                        and Article='{0}'
                            and t.id ='{2}'
                        )TC{1}", tbArticle.Rows[i][0].ToString().Trim(), i, combdetail_id));
            }
            sql = string.Format(@"
                    select
                        MT.SEQ,
                        MT.ThreadLocation,
                        MT.UseRatioNumeric,
                        MT.Allowance,
                        TD.id,
                        TD.Refno
                        {1}
                    from MachineType_ThreadRatio MT 
                    outer apply (
	                    select t.Refno,t.id
	                    from ThreadColorComb_Detail t
	                    where Machinetypeid=MT.ID
						and SEQ = MT.SEQ
                        and t.id ='{3}'
                        group by t.Refno,t.id
                    )TD
					{2}
                    where MT.ID='{0}'", detailRow["Machinetypeid"].ToString(), art_col, op.ToString(),combdetail_id);
            dResult = DBProxy.Current.Select(null, sql, out gridTable);
            if (!dResult)
            {
                ShowErr(sql, dResult);
            }
            #endregion

            grid1.DataSource = gridTable;           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (!EditMode)
            {
                button1.Text = "Save";
                EditMode = true;
                grid1.IsEditingReadOnly = !EditMode;
            }
            else
            {
                grid1.ValidateControl();
                //string article, seekSql, refnoSql;
                //string delSql = "", updateSql = "", insertSql = "";
                //bool msg = true; //判斷是否要Show error
                //bool linsert = true; //判斷若僅輸入Refno 就必須新增一筆Data
                //DataRow seekRow;
                DataTable refDT;
                StringBuilder insertSql = new StringBuilder();
                StringBuilder updateSql = new StringBuilder();
                StringBuilder delSql = new StringBuilder();
                string refnoSql;

                foreach (DataRow dr in gridTable.Rows)
                {
                    //判斷Ref是否為空  以及後面的column是否全為空值
                    if (MyUtility.Check.Empty(dr["Refno"]))
                    {
                        int i;
                        for (i = 0; i < tbArticle.Rows.Count; i++)
                        {
                            if (!MyUtility.Check.Empty(dr[i + 6])) break;//只要有任一筆不為空,跳出for迴圈
                        }
                        if (i == tbArticle.Rows.Count)//相等表示判斷剩下的column全為空
                        {
                            delSql.Append(string.Format("Delete from ThreadColorComb_Detail where id = '{0}' and seq='{1}' and ThreadLocationID='{2}'", detailRow["id"].ToString(), dr["SEQ"].ToString(), dr["ThreadLocation"].ToString()));
                            continue;
                        }
                    }

                    refnoSql = string.Format("Select * from ThreadColorComb_Detail where id = '{0}' and seq='{1}' and ThreadLocationID='{2}'", detailRow["id"].ToString(), dr["SEQ"].ToString(), dr["ThreadLocation"].ToString());
                    DualResult dResult = DBProxy.Current.Select(null, refnoSql, out refDT);
                    if (!dResult)
                    {
                        ShowErr(refnoSql, dResult);
                    }

                    if (refDT.Rows.Count != 0)
                    {
                        for (int i = 0; i < tbArticle.Rows.Count; i++)
                        {
                            updateSql.Append(string.Format("Update ThreadColorComb_Detail set Refno='{0}', threadcolorid ='{1}' where id = '{2}' and Article = '{3}' and SEQ = '{4}' and ThreadLocationID='{5}'", dr["Refno"].ToString(), dr[tbArticle.Rows[i]["article"].ToString().Trim()].ToString(), detailRow["id"].ToString(), tbArticle.Rows[i]["article"].ToString().Trim(), dr["SEQ"].ToString(), dr["ThreadLocation"].ToString()));
                        }
                    }
                    if (refDT.Rows.Count == 0)
                    {
                        //Ref有值且無資料
                        for (int i = 0; i < tbArticle.Rows.Count; i++)
                        {
                            insertSql.Append(string.Format(@"Insert into ThreadColorComb_Detail(id,Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID) Values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}');", detailRow["ID"], detailRow["Machinetypeid"], detailRow["Threadcombid"], dr["Refno"], tbArticle.Rows[i]["article"].ToString().Trim(), dr[tbArticle.Rows[i]["article"].ToString().Trim()], dr["SEQ"], dr["ThreadLocation"]));
                        }
                    }
                }
                #region 舊做法
//                foreach (DataRow dr in gridTable.Rows)
//                {
//                    if (!MyUtility.Check.Empty(dr["Refno"])) linsert = true; //判斷若僅輸入Refno 就必須新增一筆Data
//                    else linsert = false;
//                    msg = true;
//                    if (dr.RowState == DataRowState.Modified)
//                    {
//                        foreach (DataRow drcol in tbArticle.Rows)
//                        {
//                            article = drcol["article"].ToString().Trim(); 
//                            if (MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr[article]))
//                            {
//                                msg = false; //有color無Refno 不可產生
//                            }
//                            if (!MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr[article]))
//                            {
//                                linsert = false; //只要有Color 就不需要新增
//                            }
//                            if (MyUtility.Check.Empty(dr[article + "Ukey"]))
//                            {
//                                if (!MyUtility.Check.Empty(dr[article])) //Color 有值就新增空白Ukey
//                                {
//                                    insertSql = insertSql + string.Format("Insert into ThreadColorComb_Detail(id,Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID) Values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}');", detailRow["ID"], detailRow["Machinetypeid"], detailRow["Threadcombid"], dr["Refno"], article, dr[article], dr["SEQ"], dr["ThreadLocation"]);
//                                }
//                            }
//                            else
//                            {
//                                seekSql = string.Format("Select * from ThreadColorComb_Detail where Ukey = {0}", dr[article + "Ukey"]);
//                                if (MyUtility.Check.Seek(seekSql, out seekRow)) //存在Ukey 判斷只要有不同的Refno,Color 就update
//                                {
//                                    if (seekRow["ThreadColorid"].ToString() != dr[article].ToString() || seekRow["Refno"].ToString() != dr["Refno"].ToString())
//                                    {
//                                        if (MyUtility.Check.Empty(dr["Refno"]) && MyUtility.Check.Empty(dr[article]))
//                                        { //只要Refno 跟Color 都是空白就刪除資料 否則就更新
//                                            delSql = delSql + string.Format("Delete from ThreadColorComb_Detail where Ukey = {0}", dr[article + "Ukey"]);
//                                        }
//                                        else
//                                        {
//                                            updateSql = updateSql + string.Format("Update ThreadColorComb_Detail set threadcolorid ='{0}', Refno='{1}' where Ukey = {2};", dr[article].ToString(), dr["Refno"].ToString(), dr[article + "Ukey"]);
//                                        }

//                                    }
//                                }

//                            }
//                        }
//                    }
//                    if (!msg)
//                    {
//                        MyUtility.Msg.WarningBox("If has thread color, thread refno can't empty.");
//                        return;
//                    }
//                    if (linsert)
//                    {
//                        seekSql = string.Format(@"Select * from ThreadColorComb_Detail 
//                        where id = {0} and Machinetypeid = '{1}' and ThreadCombid='{2}' and Refno = '{3}' and 
//                        Article = '{4}' and SEQ ='{5}' and ThreadLocationID= '{6}'", 
//                        detailRow["ID"], detailRow["Machinetypeid"], detailRow["Threadcombid"], 
//                        dr["Refno"], tbArticle.Rows[0]["article"].ToString().Trim(), dr["SEQ"], dr["ThreadLocation"]);
//                        if (!MyUtility.Check.Seek(seekSql)) //存在Ukey 判斷只要有不同的Refno,Color 就update
//                        {
//                            insertSql = insertSql + string.Format(@"Insert into ThreadColorComb_Detail(id,Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID) Values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}');", detailRow["ID"], detailRow["Machinetypeid"], detailRow["Threadcombid"], dr["Refno"], tbArticle.Rows[i]["article"].ToString().Trim(), dr[tbArticle.Rows[i]["article"].ToString().Trim()], dr["SEQ"], dr["ThreadLocation"]);
//                        }
//                    }
//                }
#endregion
                DualResult result;
                TransactionScope _transactionscope = new TransactionScope();
                using (_transactionscope)
                {
                    try
                    {
                        if (!MyUtility.Check.Empty(delSql.ToString()))
                        {
                            result = DBProxy.Current.Execute(null, delSql.ToString());
                            if (!result)
                            {
                                ShowErr(delSql.ToString(), result);
                                return;
                            }
                        }
                        if (!MyUtility.Check.Empty(updateSql.ToString()))
                        {
                            result = DBProxy.Current.Execute(null, updateSql.ToString());
                            if (!result)
                            {
                                ShowErr(updateSql.ToString(), result);
                                return;
                            }
                        }
                        if (!MyUtility.Check.Empty(insertSql.ToString()))
                        {

                            result = DBProxy.Current.Execute(null, insertSql.ToString());
                            if (!result)
                            {
                                ShowErr(insertSql.ToString(), result);
                                return;
                            }
                        }
                        _transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }

                _transactionscope.Dispose();
                _transactionscope = null;

                button1.Text = "Edit";
                EditMode = false;
                grid1.IsEditingReadOnly = !EditMode;
            }
        }

        
    }
}
