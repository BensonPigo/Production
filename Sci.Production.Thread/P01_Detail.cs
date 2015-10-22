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
        private string styleUkey;
        private DataTable headerTable,gridTable,tbArticle;
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
            button1.Visible = editmode;
            //建立Gird
            generateGrid();

        }
        
        private void generateGrid() //建立Gird
        {
            string articleSql = string.Format("Select Article from Style_Article where styleukey='{0}'", styleUkey);
            string art_col="";
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
                    art_col = art_col + ", '' as '" +dr["article"].ToString().Trim() + "',0 as '"+dr["article"].ToString().Trim() + "Ukey'";
                }
            }
            dResult = DBProxy.Current.Select(null, articleSql, out headerTable);
            if(!dResult)
            {
                ShowErr(articleSql, dResult);
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
                        row["threadcolorid"] = "";
                        row.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
            };
            
            Helper.Controls.Grid.Generator(grid1)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("ThreadLocationName", header: "Thread Location", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("UseRatio", header: "Use Ratio", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), settings: refno_col);

            grid1.Columns[3].DefaultCellStyle.BackColor = Color.Pink;

            for(int i = 0;i < tbArticle.Rows.Count;i++)
            {
                Helper.Controls.Grid.Generator(grid1)
                    .Text(tbArticle.Rows[i]["article"].ToString().Trim(), header: tbArticle.Rows[i]["article"].ToString().Trim(), width: Widths.AnsiChars(2), settings: threadcolor_col);

                grid1.Columns[i+4].Width = 120;
                grid1.Columns[i + 4].DefaultCellStyle.BackColor = Color.Pink;
                

            }
            #endregion
            #region 資料建立
            DataTable cursorTable;
            sql = @"Select a.SEQ,a.ThreadLocation,a.UseRatio,
                    b.Ukey,b.ID,b.Refno,b.Article,b.ThreadColorid,c.name as ThreadLocationName";
            sql = sql + string.Format(@" from MachineType_ThreadRatio a 
                    join Reason c on c.Reasontypeid ='Threadlocation' and c.id = a.threadlocation 
                    left join ThreadColorComb_detail b 
                    on a.id = b.machinetypeid and a.seq = b.seq 
                    and a.ThreadLocation = b.threadlocationid and b.id = '{0}' 
                    and b.machinetypeid = '{1}' and b.threadcombid ='{2}' 
                    where a.id = '{1}' order by ThreadLocationName"
                ,detailRow["id"].ToString(), detailRow["Machinetypeid"].ToString(), detailRow["ThreadCombID"].ToString());

            dResult = DBProxy.Current.Select(null, sql, out cursorTable);
            if (!dResult)
            {
                ShowErr(sql, dResult);
            }
            MyUtility.Tool.ProcessWithDatatable(cursorTable,
                            @"SEQ,ThreadLocation,UseRatio,ID,Refno,ThreadLocationName",
                            @"Select SEQ,ThreadLocation,UseRatio,ID,Refno,ThreadLocationName" + art_col + @" from #tmp group by 
                                SEQ,ThreadLocation,UseRatio,ID,Refno,ThreadLocationName", out gridTable);
            string headerArticle, headerArticleUkey;
            foreach (DataRow dr in cursorTable.Rows)
            {
                if (!MyUtility.Check.Empty(dr["article"]))
                {
                    headerArticle = dr["article"].ToString();
                    headerArticleUkey = dr["article"].ToString() + "Ukey";
                    DataRow[] drArray = gridTable.Select(string.Format("SEQ = '{0}' and ThreadLocation='{1}' and Refno ='{2}' and UseRatio='{3}'",dr["SEQ"],dr["ThreadLocation"],dr["Refno"],dr["UseRatio"]));
                    drArray[0][headerArticle] = dr["threadcolorid"].ToString();
                    drArray[0][headerArticleUkey] = dr["ukey"].ToString();
                }
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
                string article, seekSql, refnoSql;
                string delSql = "", updateSql = "", insertSql = "";
                bool msg = true; //判斷是否要Show error
                bool linsert = true; //判斷若僅輸入Refno 就必須新增一筆Data
                DataRow seekRow;
                DataTable refDT;
                refnoSql = string.Format("Select ukey,refno from ThreadColorComb_Detail where id ='{0}' group by ukey,refno ", detailRow["ID"].ToString());
                DualResult dResult = DBProxy.Current.Select(null, refnoSql, out refDT);
                if (!dResult)
                {
                    ShowErr(refnoSql, dResult);
                }
                foreach (DataRow dr in gridTable.Rows)
                {
                    if (!MyUtility.Check.Empty(dr["Refno"])) linsert = true; //判斷若僅輸入Refno 就必須新增一筆Data
                    else linsert = false;
                    msg = true;
                    if (dr.RowState == DataRowState.Modified)
                    {
                        foreach (DataRow drcol in tbArticle.Rows)
                        {
                            article = drcol["article"].ToString().Trim(); 
                            if (MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr[article]))
                            {
                                msg = false; //有color無Refno 不可產生
                            }
                            if (!MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr[article]))
                            {
                                linsert = false; //只要有Color 就不需要新增
                            }
                            if (MyUtility.Check.Empty(dr[article + "Ukey"]))
                            {
                                if (!MyUtility.Check.Empty(dr[article])) //Color 有值就新增空白Ukey
                                {
                                    insertSql = insertSql + string.Format("Insert into ThreadColorComb_Detail(id,Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID) Values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}');", detailRow["ID"], detailRow["Machinetypeid"], detailRow["Threadcombid"], dr["Refno"], article, dr[article], dr["SEQ"], dr["ThreadLocation"]);
                                }
                            }
                            else
                            {
                                seekSql = string.Format("Select * from ThreadColorComb_Detail where Ukey = {0}", dr[article + "Ukey"]);
                                if (MyUtility.Check.Seek(seekSql, out seekRow)) //存在Ukey 判斷只要有不同的Refno,Color 就update
                                {
                                    if (seekRow["ThreadColorid"].ToString() != dr[article].ToString() || seekRow["Refno"].ToString() != dr["Refno"].ToString())
                                    {
                                        if (MyUtility.Check.Empty(dr["Refno"]) && MyUtility.Check.Empty(dr[article]))
                                        { //只要Refno 跟Color 都是空白就刪除資料 否則就更新
                                            delSql = delSql + string.Format("Delete from ThreadColorComb_Detail where Ukey = {0}", dr[article + "Ukey"]);
                                        }
                                        else
                                        {
                                            updateSql = updateSql + string.Format("Update ThreadColorComb_Detail set threadcolorid ='{0}', Refno='{1}' where Ukey = {2};", dr[article].ToString(), dr["Refno"].ToString(), dr[article + "Ukey"]);
                                        }

                                    }
                                }

                            }
                        }
                    }
                    if (!msg)
                    {
                        MyUtility.Msg.WarningBox("If has thread color, thread refno can't empty.");
                        return;
                    }
                    if (linsert)
                    {
                        seekSql = string.Format(@"Select * from ThreadColorComb_Detail 
                        where id = {0} and Machinetypeid = '{1}' and ThreadCombid='{2}' and Refno = '{3}' and 
                        Article = '{4}' and SEQ ='{5}' and ThreadLocationID= '{6}'", 
                        detailRow["ID"], detailRow["Machinetypeid"], detailRow["Threadcombid"], 
                        dr["Refno"], tbArticle.Rows[0]["article"].ToString().Trim(), dr["SEQ"], dr["ThreadLocation"]);
                        if (!MyUtility.Check.Seek(seekSql)) //存在Ukey 判斷只要有不同的Refno,Color 就update
                        {
                            insertSql = insertSql + string.Format("Insert into ThreadColorComb_Detail(id,Machinetypeid,ThreadCombid,Refno,Article,SEQ,ThreadLocationID) Values({0},'{1}','{2}','{3}','{4}','{5}','{6}');", detailRow["ID"], detailRow["Machinetypeid"], detailRow["Threadcombid"], dr["Refno"], tbArticle.Rows[0]["article"].ToString().Trim(), dr["SEQ"], dr["ThreadLocation"]);
                        }
                    }
                }
                DualResult result;
                TransactionScope _transactionscope = new TransactionScope();
                using (_transactionscope)
                {
                    try
                    {
                        if (!MyUtility.Check.Empty(delSql))
                        {
                            result = DBProxy.Current.Execute(null, delSql);
                            if (!result)
                            {
                                ShowErr(delSql, result);
                                return;
                            }
                        }
                        if (!MyUtility.Check.Empty(updateSql))
                        {
                            result = DBProxy.Current.Execute(null, updateSql);
                            if (!result)
                            {
                                ShowErr(updateSql, result);
                                return;
                            }
                        }
                        if (!MyUtility.Check.Empty(insertSql))
                        {

                            result = DBProxy.Current.Execute(null, insertSql);
                            if (!result)
                            {
                                ShowErr(insertSql, result);
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
