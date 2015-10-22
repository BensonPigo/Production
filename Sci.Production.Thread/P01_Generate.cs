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

namespace Sci.Production.Thread
{
    public partial class P01_Generate : Sci.Win.Subs.Base
    {
        private DataTable gridTable,detTable;
        private string styleid,season,id,styleUkey;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P01_Generate(string str_styleukey, string str_styleid,string str_season,string str_brandid)
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable groupTable,countTable,operTable;
            string sql="",str_Select,delesql="";
            List<string> SqlList = new List<string>();
            List<string> gridList = new List<string>();

            MyUtility.Tool.ProcessWithDatatable(gridTable, "threadcombid,Machinetypeid",
                                                @"Select threadcombid,Machinetypeid from #tmp where threadcombid is not null and rtrim(threadcombid) <>''  group by threadcombid,machinetypeid"
                                                , out groupTable);
            MyUtility.Tool.ProcessWithDatatable(groupTable, "threadcombid,Machinetypeid",
                                    @"Select count(Threadcombid) as tt , threadcombid from #tmp  group by threadcombid having count(Threadcombid) > 1"
                                    , out countTable);
            string msg = "There is <Thread Combination > over use two <Machine Type> \n";
            if (countTable.Rows.Count != 0) 
            {
                foreach (DataRow dr in countTable.Rows)
                {
                    msg = msg+"\n"+dr["ThreadCombid"];
                }
                MyUtility.Msg.WarningBox(msg);
                return;
            }
            foreach (DataRow dr in gridTable.Rows) //先刪除資料
            {
                if (dr.RowState == DataRowState.Modified && MyUtility.Check.Empty(dr["threadcombid"]))
                {
                    delesql = delesql + string.Format("Delete from threadcolorcomb where id='{0}';delete from threadcolorcomb_operation where id='{0}' and operationid = '{1}';delete from threadcolorcomb_Detail where id='{0}'", dr["ID"].ToString(),dr["operationid"].ToString());
                }
            }
            MyUtility.Tool.ProcessWithDatatable(gridTable, "id,threadcombid,Machinetypeid,seamlength",
                                    @"Select id ,threadcombid,Machinetypeid ,isnull(sum(seamlength),0) as Length
                                    from #tmp where threadcombid is not null and rtrim(threadcombid) <>'' group
                                    by id,threadcombid,machinetypeid", out groupTable,"#tmp");

            //by operationid,threadcombid group
            MyUtility.Tool.ProcessWithDatatable(gridTable, "threadcombid,operationid,Machinetypeid",
                                    @"Select threadcombid,operationid,Machinetypeid
                                    from #tmp where threadcombid is not null and rtrim(threadcombid) <>'' group
                                    by threadcombid,operationid,Machinetypeid", out operTable, "#tmp");
            foreach (DataRow dr in groupTable.Rows)
            {
                
                if (MyUtility.Check.Empty(dr["id"])) //不存在ID 表示新增
                {
                    sql = string.Format("Insert into ThreadColorComb (threadcombid,machinetypeid,styleukey,length) values('{0}','{1}','{2}',{3})", dr["threadcombid"].ToString(), dr["machinetypeid"].ToString(), styleUkey, dr["Length"]) + "; select @@IDENTITY as ii";
                }
                else //Update 存在ID 時update
                {
                    sql = string.Format("update ThreadColorComb set length = {0},threadcombid = '{1}' where id = '{2}' ;select ID as ii from threadcolorcomb where id='{2}'", dr["Length"], dr["threadcombid"].ToString(), dr["ID"].ToString());
                }
                SqlList.Add(sql);
                str_Select = (string.Format("threadcombid ='{0}' and machinetypeid = '{1}'",dr["threadcombid"].ToString(), dr["machinetypeid"].ToString()));
                gridList.Add(str_Select);
            }
            #region update Insert
            DualResult upResult;
            DataTable dt;
            string sql2=" ";
            
            TransactionScope _transactionscope = new TransactionScope();
            
            using (_transactionscope)
            {
                try
                {
                    if(!MyUtility.Check.Empty(delesql))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, delesql)))
                        {
                            ShowErr(delesql, upResult);
                            return;
                        }
                    }
                    for (int i = 0; i < SqlList.Count; i++)
                    {
                        if (!(upResult = DBProxy.Current.Select(null, SqlList[i], out dt)))
                        {
                            ShowErr(SqlList[i], upResult);
                            return;
                        }
                        else
                        {
                            if (dt.Rows.Count != 0) //沒有dt 表示為原本就存在的不需要新增Operation
                            {
                                DataRow[] rowSelect = operTable.Select(gridList[i]);
                                for (int j = 0; j < rowSelect.Length; j++)
                                {
                                    if (!MyUtility.Check.Seek(string.Format("Select * from ThreadColorComb_operation where id='{0}' and operationid = '{1}'", dt.Rows[0]["ii"].ToString(), rowSelect[j]["operationid"].ToString())))
                                    {
                                        sql2 = sql2 + string.Format("Insert into ThreadColorComb_operation (id,operationid) values({0},'{1}');", dt.Rows[0]["ii"].ToString(), rowSelect[j]["operationid"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, sql2)))
                    {
                        ShowErr(sql2, upResult);
                        return;
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
