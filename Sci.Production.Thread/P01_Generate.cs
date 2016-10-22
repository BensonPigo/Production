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
using System.Collections;
using System.Transactions;
using System.Linq;

namespace Sci.Production.Thread
{
    public partial class P01_Generate : Sci.Win.Subs.Base
    {
        private DataTable gridTable, detTable;
        private string styleid, season, id, styleUkey;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P01_Generate(string str_styleukey, string str_styleid, string str_season, string str_brandid)
        {
            InitializeComponent();


            string sql = string.Format(
            @"with a as (
            Select threadcombid,operationid ,isnull(a.id,'') as id,f.id as styleid,f.seasonid,f.brandid
            from threadcolorcomb a, threadcolorcomb_Operation b ,style f
            where a.id = b.id and a.styleukey = f.ukey and 
            f.id = '{0}' and f.seasonid = '{1}' and f.brandid = '{2}'),
            b as(
            Select seq,operationid,annotation,d.SeamLength,d.MachineTypeID,descEN,styleid,seasonid,brandid
            from timestudy c,timestudy_Detail d 
            join operation e on e.id = d.operationid
            where c.id = d.id and c.styleid = '{0}' and c.seasonid = '{1}' and c.brandid = '{2}')
            select 0 as sel,a.id,b.*,a.threadcombid from b left join a 
			on a.operationid = b.operationid and b.styleid = a.styleid 
            and a.seasonid  = b.seasonid and a.brandid = b.brandid order by seq
            ", str_styleid, str_season, str_brandid);
            styleUkey = str_styleukey;
            DualResult dResult = DBProxy.Current.Select(null, sql, out gridTable);
            if (dResult)
            {
                this.grid1.DataSource = gridTable;
            }
            else
            {
                ShowErr(sql);
                return;
            }

            DataGridViewGeneratorTextColumnSettings threadcombcell = cellthreadcomb.GetGridCell(true);
            // DataGridViewGeneratorTextColumnSettings  combCell = new DataGridViewGeneratorTextColumnSettings();
            threadcombcell.CellValidating += (s, e) =>
            {
                string newValue = e.FormattedValue.ToString();
                string operationid = gridTable.DefaultView.ToTable().Rows[e.RowIndex]["operationid"].ToString();
                string machinetypeid = gridTable.DefaultView.ToTable().Rows[e.RowIndex]["machinetypeid"].ToString();
                foreach (DataRowView dr in gridTable.DefaultView)
                {
                    if (dr["operationid"].ToString() == operationid && dr["machinetypeid"].ToString() == machinetypeid)
                    {
                        dr["threadcombid"] = newValue;
                        dr.EndEdit();
                    }
                }

            };

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
            .Text("Seq", header: "SEQ", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("Operationid", header: "Operation Code", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("descEN", header: "Operation Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("Seamlength", header: "Seam Length", width: Widths.AnsiChars(9), integer_places: 9, decimal_places: 2, iseditingreadonly: true)
            .Text("MachineTypeid", header: "Machine Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Threadcombid", header: "Thread Combination", width: Widths.AnsiChars(10), settings: threadcombcell);
            grid1.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            if (MyUtility.Check.Empty(txtmachinetype1.Text))
            {
                gridTable.DefaultView.RowFilter = this.checkBox1.Value == "True" ? "Threadcombid is null" : "";
            }
            else
            {
                gridTable.DefaultView.RowFilter = this.checkBox1.Value == "True" ? string.Format("MachineTypeid = '{0}' and Threadcombid is null", txtmachinetype1.Text) : string.Format("MachineTypeid = '{0}'", txtmachinetype1.Text);
            }
        }

        //close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //save到DB
        private void btn_Generate_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable groupTable, countTable, operTable, gridTable3;
            StringBuilder delesql = new StringBuilder();
            string sql = "", str_Select;
            List<string> SqlList = new List<string>();
            List<string> gridList = new List<string>();

            gridTable3 = gridTable.Clone();//複製結構
            #region 準備有輸入資料的row到gridTable3,並判斷Threadcombid是否符合規則
            if (gridTable.AsEnumerable().Any(row => !MyUtility.Check.Empty(row["Threadcombid"])))//Threadcombid任一筆有值則true
            {
                //Threadcombid欄位有資料複製到gridTable3
                gridTable3 = gridTable.Select("threadcombid is not null and Threadcombid <> ''").CopyToDataTable();

                #region 判斷Threadcombid是否符合規則
                //從gridTable3選threadcombid,Machinetypeid groupby後groupTable
                MyUtility.Tool.ProcessWithDatatable(gridTable3, "threadcombid,Machinetypeid", @"Select threadcombid,Machinetypeid from #tmp where threadcombid is not null and rtrim(threadcombid) <>'' group by threadcombid,machinetypeid", out groupTable);
                //從groupTable計數不同的Machinetypeid確有相同的Threadcombid
                MyUtility.Tool.ProcessWithDatatable(groupTable, "threadcombid,Machinetypeid", @"Select count(Threadcombid) as tt , threadcombid from #tmp  group by threadcombid having count(Threadcombid) > 1", out countTable);
                //countTable不為空則表示Threadcombid不符規則
                StringBuilder overmsg = new StringBuilder();
                if (countTable.Rows.Count != 0)
                {
                    overmsg.Append("There is <Thread Combination > over use two <Machine Type> \n");
                    foreach (DataRow dr in countTable.Rows)
                    {
                        overmsg.Append(dr["ThreadCombid"] + "\n");
                    }
                    MyUtility.Msg.WarningBox(overmsg.ToString());
                    return;
                }
                #endregion
            }
            #endregion
            
            //請三思按下YES就回不了頭
            DialogResult buttonFinished = MyUtility.Msg.QuestionBox("All related data of this style will be clear, please confirm"
                                                                    , "Question", MessageBoxButtons.YesNo);
            if (buttonFinished == DialogResult.No) return;
            
            #region 準備刪除資料字串
            foreach (DataRow dr in gridTable.Rows)
            {
                delesql.Append(string.Format(@"Delete from threadcolorcomb_operation where id='{1}'
                                               Delete from threadcolorcomb_Detail where id='{1}'
                                               Delete from threadcolorcomb where StyleUkey='{0}'"
                                              , styleUkey, dr["ID"].ToString()));
            }
            #endregion

            //sum(seamlength)
            MyUtility.Tool.ProcessWithDatatable(gridTable3, "id,threadcombid,Machinetypeid,seamlength",
                                    @"Select id ,threadcombid,Machinetypeid ,isnull(sum(seamlength),0) as Length
                                    from #tmp where threadcombid is not null and rtrim(threadcombid) <>'' 
                                    group by id,threadcombid,machinetypeid", out groupTable, "#tmp");
            //operationid,threadcombid ,Machinetypeid
            MyUtility.Tool.ProcessWithDatatable(gridTable3, "threadcombid,operationid,Machinetypeid",
                                    @"Select threadcombid,operationid,Machinetypeid
                                    from #tmp where threadcombid is not null and rtrim(threadcombid) <>'' 
                                    group by threadcombid,operationid,Machinetypeid", out operTable, "#tmp");

            #region 準備新增字串
            foreach (DataRow dr in groupTable.Rows)
            {
                //前面全部都刪除不會有updata狀況
                //準備新增字串且取得新增此筆的IDENTITY要用來存入ThreadColorComb_operation
                sql = string.Format(@"Insert into ThreadColorComb 
                                    (threadcombid,machinetypeid,styleukey,length) 
                                    values('{0}','{1}','{2}',{3})
                                    select @@IDENTITY as ii",
                                    dr["threadcombid"].ToString(), dr["machinetypeid"].ToString(), styleUkey, dr["Length"]);
                //用List是為了可各筆執行後分別取回IDENTITY
                SqlList.Add(sql);
                str_Select = string.Format("threadcombid ='{0}' and machinetypeid = '{1}'",
                                            dr["threadcombid"].ToString(), dr["machinetypeid"].ToString());
                gridList.Add(str_Select);
            }
            #endregion

            #region  執行Delete,Update,Insert字串
            DualResult upResult;
            DataTable dt;
            StringBuilder sql2 = new StringBuilder();

            TransactionScope _transactionscope = new TransactionScope();

            using (_transactionscope)
            {
                try
                {
                    if (!MyUtility.Check.Empty(delesql.ToString()))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, delesql.ToString())))
                        {
                            ShowErr(delesql.ToString(), upResult);
                            return;
                        }
                    }

                    if (SqlList.Count>0)
                    {
                        for (int i = 0; i < SqlList.Count; i++)
                        {
                            //執行新增ThreadColorComb,並取回此筆IDENTITY存入dt
                            if (!(upResult = DBProxy.Current.Select(null, SqlList[i], out dt)))
                            {
                                ShowErr(SqlList[i], upResult);
                                return;
                            }
                            else
                            {
                                //新增 ThreadColorComb_operation字串                               
                                if (dt.Rows.Count != 0) //沒有dt 表示為原本就存在的不需要新增Operation
                                {
                                    DataRow[] rowSelect = operTable.Select(gridList[i]);
                                    for (int j = 0; j < rowSelect.Length; j++)
                                    {
                                        if (!MyUtility.Check.Seek(string.Format("Select * from ThreadColorComb_operation where id='{0}' and operationid = '{1}'", dt.Rows[0]["ii"].ToString(), rowSelect[j]["operationid"].ToString())))
                                        {
                                            sql2.Append(string.Format("Insert into ThreadColorComb_operation (id,operationid) values({0},'{1}');", dt.Rows[0]["ii"].ToString(), rowSelect[j]["operationid"].ToString()));
                                        }
                                    }
                                }
                            }
                        }
                        if (!(upResult = DBProxy.Current.Execute(null, sql2.ToString())))
                        {
                            ShowErr(sql2.ToString(), upResult);
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

            #endregion
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            foreach (DataRowView dr in gridTable.DefaultView)
            {
                if (dr["Sel"].ToString() == "1") dr["ThreadCombid"] = txtthreadcomb1.Text;
            }
            grid1.ValidateControl();
        }
    }
}
