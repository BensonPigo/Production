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
            displayMachineType.Value = detailrow["Machinetypeid"].ToString();
            displayStyleNo.Value = masterrow["id"].ToString();
            displaySeason.Value = masterrow["Seasonid"].ToString();
            displayThreadCombination.Value = detailrow["ThreadCombID"].ToString();
            combdetail_id = detailrow["id"].ToString();
            displayBoxEdit.Text = MyUtility.Convert.GetString(masterrow["ThreadEditname"])+" "+ (MyUtility.Convert.GetString(masterrow["ThreadEditdate"])=="" ?"":((DateTime)MyUtility.Convert.GetDate(masterrow["ThreadEditdate"])).ToString("yyyy/MM/dd HH:mm:ss"));
            btnEdit.Enabled = Sci.Production.PublicPrg.Prgs.GetAuthority(loginID, "P01.Thread Color Combination", "CanEdit");
            btnEdit.Visible = Sci.Production.PublicPrg.Prgs.GetAuthority(loginID, "P01.Thread Color Combination", "CanEdit");
            //建立Gird
            generateGrid();

        }
        
        private void generateGrid() //建立Gird
        {
            string articleSql = string.Format("Select Article from Style_Article WITH (NOLOCK) where styleukey='{0}'", styleUkey);
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
            DataGridViewGeneratorTextColumnSettings refno_col = celllocalitem.GetGridCell("SP_THREAD", null);
            DataGridViewGeneratorTextColumnSettings threadcolor_col = new DataGridViewGeneratorTextColumnSettings();
            threadcolor_col.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow row = gridDetail.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele;
                    string header = gridDetail.Columns[e.ColumnIndex].HeaderText;
                    sele = new SelectItem("Select id, description From ThreadColor WITH (NOLOCK) where junk=0", "23", row[header].ToString(), false, ",");

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
                DataRow row = gridDetail.GetDataRow<DataRow>(e.RowIndex);
                string header = gridDetail.Columns[e.ColumnIndex].HeaderText;
                String oldValue = row[header].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                sql = string.Format("Select id, description From ThreadColor WITH (NOLOCK) where junk=0 and id='{0}'", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql))
                    {
                        row[header] = "";
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format(@"<Thread Color: {0}> not found!!!", newValue));
                        return;
                    }
                }
            };

            Helper.Controls.Grid.Generator(gridDetail)
                .Text("SEQ", header: "SEQ", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("ThreadLocation", header: "Thread Location", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("UseRatioNumeric", header: "UseRatioNumeric", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Allowance", header: "Start End Loss", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(16), settings: refno_col);
                //前4個是MachineType_ThreadRatio,後一個Refno是ThreadColorComb_Detail

            gridDetail.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;

            for(int i = 0;i < tbArticle.Rows.Count;i++)
            {
                Helper.Controls.Grid.Generator(gridDetail)
                    .Text(tbArticle.Rows[i]["article"].ToString().Trim(), header: tbArticle.Rows[i]["article"].ToString().Trim(), width: Widths.AnsiChars(10), settings: threadcolor_col);

                gridDetail.Columns[tbArticle.Rows[i]["article"].ToString().Trim()].DefaultCellStyle.BackColor = Color.Pink;
            }
            #endregion

            #region 資料建立
            StringBuilder op = new StringBuilder();
            for (int i = 0; i < tbArticle.Rows.Count; i++)
            {
                op.Append(string.Format(@"
                        outer apply (
	                        select t.ThreadColorid as '{0}'
	                        from ThreadColorComb_Detail t WITH (NOLOCK) 
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
                    from MachineType_ThreadRatio MT WITH (NOLOCK) 
                    outer apply (
	                    select t.Refno,t.id
	                    from ThreadColorComb_Detail t WITH (NOLOCK) 
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

            gridDetail.DataSource = gridTable;           
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            
            if (!EditMode)
            {
                btnEdit.Text = "Save";
                EditMode = true;
                gridDetail.IsEditingReadOnly = !EditMode;
            }
            else
            {
                gridDetail.ValidateControl();
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

                    refnoSql = string.Format("Select * from ThreadColorComb_Detail WITH (NOLOCK) where id = '{0}' and seq='{1}' and ThreadLocationID='{2}'", detailRow["id"].ToString(), dr["SEQ"].ToString(), dr["ThreadLocation"].ToString());
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
                string StyleEdit = string.Format(@"
update s set 
    ThreadEditname ='{3}',ThreadEditdate='{4}' 
from style s 
where id = '{0}' and BrandID ='{1}' and SeasonID = '{2}'",
masterRow["id"].ToString(), 
masterRow["BrandID"].ToString(), 
masterRow["SeasonID"].ToString(),
Sci.Env.User.UserID,
DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
);

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
                                _transactionscope.Dispose();
                                ShowErr(delSql.ToString(), result);
                                return;
                            }
                        }
                        if (!MyUtility.Check.Empty(updateSql.ToString()))
                        {
                            result = DBProxy.Current.Execute(null, updateSql.ToString());
                            if (!result)
                            {
                                _transactionscope.Dispose();
                                ShowErr(updateSql.ToString(), result);
                                return;
                            }
                        }
                        if (!MyUtility.Check.Empty(insertSql.ToString()))
                        {
                            result = DBProxy.Current.Execute(null, insertSql.ToString());
                            if (!result)
                            {
                                _transactionscope.Dispose();
                                ShowErr(insertSql.ToString(), result);
                                return;
                            }
                        }
                        result = DBProxy.Current.Execute(null, StyleEdit);
                        if (!result)
                        {
                            _transactionscope.Dispose();
                            ShowErr(StyleEdit, result);
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

                btnEdit.Text = "Edit";
                EditMode = false;
                gridDetail.IsEditingReadOnly = !EditMode;
                displayBoxEdit.Text = MyUtility.GetValue.Lookup(string.Format(@"
select ThreadEditname = concat(ThreadEditname ,' ',format(ThreadEditdate,'yyyy/MM/dd HH:mm:ss' ))
from style s 
where id = '{0}' and BrandID ='{1}' and SeasonID = '{2}'",
masterRow["id"].ToString(),
masterRow["BrandID"].ToString(),
masterRow["SeasonID"].ToString()));
            }
        }
    }
}
