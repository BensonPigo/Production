using Ict.Win;
using System.Data;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Quality
{
    public partial class P05_Detail : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private string  aa = Sci.Env.User.Keyword;
        private DataRow maindr;
        private string PoID,ID;
        private DataTable dtOven;
        private bool newOven=false;


        public P05_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr,string Poid)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            maindr = mainDr;
            PoID = Poid.Trim();
            ID = id.Trim();
            //判斷是否為新資料
            if (MyUtility.Check.Empty(maindr))
            {               
                newOven = true;
            }

            if (MyUtility.Check.Empty(ID) || canedit)
            {
                this.EditMode = true;
            }
                       
        }
        protected override void OnEditModeChanged()
        {
            DataTable dt;
            DBProxy.Current.Select(null,string.Format("select * from oven_detail where id='{0}'",ID),out dt);
            
            if (dt.Rows.Count>=1)
            {
                newOven = false;
            }
            base.OnEditModeChanged();
        }
        protected override Ict.DualResult OnRequery(out System.Data.DataTable datas)
        {
      
            Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
            Result_RowSource.Add("Pass", "Pass");
            Result_RowSource.Add("Fail", "Fail");
            comboBox1.DataSource = new BindingSource(Result_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            #region 表頭設定
                
            Ict.DualResult dResult;
            string cmd = "select * from oven where id=@id";      
                      
            List<SqlParameter> sqm = new List<SqlParameter>();
            sqm.Add(new SqlParameter("@id",ID));
            if (dResult = DBProxy.Current.Select(null, cmd, sqm, out dtOven))
            {
                if (dtOven.Rows.Count > 0)
                {                    
                    this.testno.Text = dtOven.Rows[0]["testno"].ToString();
                    this.poid.Text = dtOven.Rows[0]["POID"].ToString();
                    this.inspdate.Value = Convert.ToDateTime(dtOven.Rows[0]["Inspdate"]);
                    this.article.Text = dtOven.Rows[0]["article"].ToString();
                    this.txtuser1.TextBox1Binding = dtOven.Rows[0]["inspector"].ToString();
                    this.remark.Text = dtOven.Rows[0]["Remark"].ToString();
                    comboBox1.SelectedValue = dtOven.Rows[0]["Result"].ToString();
                    if (dtOven.Rows[0]["Status"].ToString()=="New" || dtOven.Rows[0]["Status"].ToString()=="")
                    {
                        this.encode_btn.Text="Encode";
                        this.save.Enabled = true;
                    }
                    else
                    {
                        this.encode_btn.Text = "Amend";
                        this.save.Enabled = false;
                    }
                       
                }
                else
                {
                    this.testno.Text = "";
                    this.poid.Text = PoID;
                    this.inspdate.Value = null;
                    this.article.Text = "";
                    this.txtuser1.TextBox1Binding = loginID;
                    this.remark.Text = "";
                    this.comboBox1.Text = "";
                }
            }           
          
            #endregion

            return base.OnRequery(out datas);
        }
        // 重組grid view 
        protected override void OnRequeryPost(System.Data.DataTable datas)
        {
            
            base.OnRequeryPost(datas);
            DataTable dtpo,dtsupp;
            // Gridview新增欄位
            datas.Columns.Add("SCIRefno", typeof(string));
            datas.Columns.Add("Colorid", typeof(string));
            datas.Columns.Add("SEQ", typeof(string));
            datas.Columns.Add("LastUpdate", typeof(string));
            datas.Columns.Add("Supplier",typeof(string));
            int i = 0;
            //跑迴圈丟值進去
            foreach (DataRow dr in datas.Rows)
            {
                if (datas.Rows.Count<=0)
                {
                    return;
                }   
                List<SqlParameter> spm = new List<SqlParameter>();
                string cmdd = "select * from PO_Supp_Detail where id=@id and seq1=@seq1 and seq2=@seq2";
                spm.Add(new SqlParameter("@id", PoID));
                spm.Add(new SqlParameter("@seq1", datas.Rows[i]["seq1"]));
                spm.Add(new SqlParameter("@seq2", datas.Rows[i]["seq2"]));
                DBProxy.Current.Select(null, cmdd, spm, out dtpo);
                //Suppliers
                List<SqlParameter> spmSupp = new List<SqlParameter>();
                string cmddSupp =
@"SELECT a.ID,a.SuppID,a.SEQ1,a.SuppID+'-'+b.AbbEN as supplier 
from PO_Supp a
left join supp b on a.SuppID=b.ID
where a.ID=@id
and a.seq1=@seq1";
                spmSupp.Add(new SqlParameter("@id", PoID));
                spmSupp.Add(new SqlParameter("@seq1", datas.Rows[i]["seq1"]));
                DBProxy.Current.Select(null, cmddSupp, spmSupp, out dtsupp);
                if (dtpo.Rows.Count <= 0)
                {
                     dr["SCIRefno"] = "";
                     dr["Colorid"] = "";
                }
                else
                {
                    dr["SEQ"] = datas.Rows[i]["seq1"].ToString() + "- " + datas.Rows[i]["seq2"].ToString();
                    dr["SCIRefno"] = dtpo.Rows[0]["SCIRefno"].ToString();
                    dr["Colorid"] = dtpo.Rows[0]["Colorid"].ToString();
                    dr["Supplier"] = dtsupp.Rows[0]["supplier"].ToString();
                    dr["LastUpdate"] = datas.Rows[i]["EditName"].ToString() + " - " + datas.Rows[i]["EditDate"].ToString();   
                }
                             
                i++;
            }

        }
        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings groupCell = new DataGridViewGeneratorMaskedTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings seqCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings rollCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings chgCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings staCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultCell = new DataGridViewGeneratorTextColumnSettings();
        

            #region MouseClick
            seqCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = string.Format("select seq1 +'-'+ seq2 AS SEQ,scirefno,colorid from PO_Supp_Detail where id='{0}'", PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }          
                    dr["SEQ"] = item.GetSelectedString(); 
                    Char splitChar='-';
                    string[] seqSplit = item.GetSelectedString().Split(splitChar);
                    dr["seq1"] = seqSplit[0];
                    dr["seq2"] = seqSplit[1];
                   
                }                 

            };
            rollCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    
                    DataRow dr = grid.GetDataRow(e.RowIndex);

                    if (newOven) //新資料 不判斷SEQ
                    {                       
                        string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>'' ";
                        List<SqlParameter> spam = new List<SqlParameter>();
                        spam.Add(new SqlParameter("@poid", PoID));
                        SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                        DialogResult dresult = item.ShowDialog();
                        if (dresult == DialogResult.Cancel)
                        {
                            return;
                        }
                        dr["Roll"] = item.GetSelectedString();
                        dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();
                                
                    }
                    else
                    {
                       
                        string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2";
                        List<SqlParameter> spam = new List<SqlParameter>();
                        spam.Add(new SqlParameter("@poid", PoID));
                        spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                        spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                        SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                        DialogResult dresult = item.ShowDialog();
                        if (dresult == DialogResult.Cancel)
                        {
                            return;
                        }
                        dr["Roll"] = item.GetSelectedString();
                        dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();
                    }
                   
                }

            };
            chgCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale where Junk=0 ";
                   
                    SelectItem item = new SelectItem(item_cmd,"10", dr["Changescale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Changescale"] = item.GetSelectedString();
                }

            };
            staCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["StainingScale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["StainingScale"] = item.GetSelectedString();
                }

            };
            #endregion
            #region Valid
           // 個位數需補0
            groupCell.CellValidating+=(s,e)=>
            {
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (e.FormattedValue.ToString().Length!=2)
                {
                    dr["OvenGroup"]="0"+e.FormattedValue.ToString();
                }                            
            };
            rollCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                DataTable dt;
                DataRow dr = grid.GetDataRow(e.RowIndex);

                if (newOven)//新資料 不判斷SEQ
                {
                    string cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>'' ";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@poid", PoID));                   
                    DBProxy.Current.Select(null, cmd, spam, out dt);
                    if (dt.Rows.Count <= 0)
                    {
                        MyUtility.Msg.InfoBox("<Roll> doesn't exist in Data!");
                        dr["Roll"] = "";
                        dr["Dyelot"] = "";
                        return;
                    }
                    else
                    {
                        List<SqlParameter> spamUpdate = new List<SqlParameter>();
                        spamUpdate.Add(new SqlParameter("@dyelot", dr["dyelot"]));
                        DBProxy.Current.Execute(null, "update FtyInventory set dyelot=@dyelot", spamUpdate);
                    }
                }
                else
                {
                    //string cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2";
                    string cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2 and Roll=@Roll ";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@poid", PoID));
                    spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                    spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                    spam.Add(new SqlParameter("@Roll", e.FormattedValue));
                    DBProxy.Current.Select(null, cmd, spam, out dt);
                    if (dt.Rows.Count <= 0)
                    {
                        MyUtility.Msg.InfoBox("<Roll> doesn't exist in Data!");
                        dr["Roll"] = "";
                        dr["Dyelot"] = "";
                        return;
                    }
                    else
                    {
                        //List<SqlParameter> spamUpdate = new List<SqlParameter>();
                        //spamUpdate.Add(new SqlParameter("@dyelot", dr["dyelot"]));
                        //DBProxy.Current.Execute(null, "update FtyInventory set dyelot=@dyelot", spamUpdate);
                        dr["Roll"] = e.FormattedValue;
                        dr["Dyelot"] = dt.Rows[0]["Dyelot"].ToString().Trim();
                    }
                }
                
                                
            };
           chgCell.CellValidating += (s, e) =>
           {
               if (this.EditMode == false) return;
               if (e.FormattedValue == "") return;
               DataTable dt;
               DataRow dr = grid.GetDataRow(e.RowIndex);
               string cmd = "select id from Scale where Junk=0  and id=@ChangeScale";
               List<SqlParameter> spam = new List<SqlParameter>();
               spam.Add(new SqlParameter("@ChangeScale", e.FormattedValue));
               
               DBProxy.Current.Select(null, cmd, spam, out dt);
               if (dt.Rows.Count <= 0)
               {
                   MyUtility.Msg.InfoBox("<Color Change Scal> doesn't exist in Data!");
                   dr["Changescale"] = "";
                   return;
               }
           };
           staCell.CellValidating += (s, e) =>
           {
               if (this.EditMode == false) return;
               if (e.FormattedValue == "") return;
               DataTable dt;
               DataRow dr = grid.GetDataRow(e.RowIndex);
               string cmd = "select id from Scale where Junk=0  and id=@StainingScale";
               List<SqlParameter> spam = new List<SqlParameter>();
               spam.Add(new SqlParameter("@StainingScale", e.FormattedValue));

               DBProxy.Current.Select(null, cmd, spam, out dt);
               if (dt.Rows.Count <= 0)
               {
                   MyUtility.Msg.InfoBox("<Color Staining Scale> doesn't exist in Data!");
                   dr["StainingScale"] = "";
                   return;
               }
           };
           resultCell.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (dr["result"].ToString() == "Pass") { dr["result"] = "Fail"; }
                else { dr["result"] = "Pass"; }
            };
            #endregion           
            Helper.Controls.Grid.Generator(this.grid)               
                .MaskedText("OvenGroup","00","Group",width: Widths.AnsiChars(5), settings:groupCell)
                .Text("SEQ", header: "SEQ#", width: Widths.AnsiChars(10),iseditable:false, settings : seqCell)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(5), settings: rollCell)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5),iseditingreadonly:true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8),iseditingreadonly:true)
                .Text("Changescale", header: "Color Change Scale", width: Widths.AnsiChars(16), settings: chgCell)
                .Text("StainingScale", header: "Color Staining Scale", width: Widths.AnsiChars(10), settings: staCell)
                .Text("Result", header: "Result", width: Widths.AnsiChars(8),settings:resultCell)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(30))
                .Text("LastUpdate", header: "LastUpdate", width: Widths.AnsiChars(30), iseditingreadonly: true);
            return true;
        }
        protected override bool OnSaveBefore()
        {
            if (MyUtility.Check.Empty(this.article.Text))
            {
                MyUtility.Msg.InfoBox("<Article> cannot be empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(this.txtuser1.TextBox1.Text))
            {
                MyUtility.Msg.InfoBox("<Inspector> cannot be empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(this.inspdate.Value))
            {
                MyUtility.Msg.InfoBox("<Test Date> cannot be empty!!");
                return false;
            }
            return base.OnSaveBefore();
        }
        protected override Ict.DualResult OnSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = "";
            foreach (DataRow dr in ((DataTable)gridbs.DataSource).Rows)
            {
                //刪除
                if (dr.RowState == DataRowState.Deleted)
                {
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From Oven_Detail Where id =@id and ovenGroup=@ovenGroup and seq1=@seq1 and seq2=@seq2 ";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@ovenGroup", dr["ovenGroup", DataRowVersion.Original]));

                    string seq1 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[0].Trim();
                    string seq2 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[1].Trim();
                    spamDet.Add(new SqlParameter("@seq1", seq1));
                    spamDet.Add(new SqlParameter("@seq2", seq2));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    continue;
                }

                //ii.刪除空白SEQ 的Oven_Detail資料
                if (MyUtility.Check.Empty(dr["SEQ"]))
                {
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From Oven_Detail Where id =@id and ovenGroup=@ovenGroup and seq1='' and seq2='' ";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@ovenGroup", dr["ovenGroup", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    continue;
                }

                
                string Today = DateTime.Now.ToShortDateString();
                

                //新增
                if (dr.RowState==DataRowState.Added)
                {
                    
                    if (newOven)// insert 新資料進oven
                    {
                        string insCmd = @"
SET IDENTITY_INSERT oven ON
insert into Oven(ID,POID,TestNo,InspDate,Article,Result,Status,Inspector,Remark,addName,addDate)
values(@id ,@poid,'1',GETDATE(),@Article,'Pass','New',@logid,@remark,@logid,GETDATE())
SET IDENTITY_INSERT oven off";
                        List<SqlParameter> spamAddNew = new List<SqlParameter>();
                        spamAddNew.Add(new SqlParameter("@id", ID));//New ID
                        spamAddNew.Add(new SqlParameter("@poid", PoID));
                        spamAddNew.Add(new SqlParameter("@article", this.article.Text));
                        spamAddNew.Add(new SqlParameter("@logid", loginID));
                        spamAddNew.Add(new SqlParameter("@remark", this.remark.Text));
                        upResult= DBProxy.Current.Execute(null, insCmd,spamAddNew);
                    }                
                }
                if (dr.RowState==DataRowState.Modified)
                {
                    string editCmd = @"update oven set inspdate=@insDate,Article=@Article,Inspector=@insor,remark=@remark where id=@id";
                        List<SqlParameter> spamEdit = new List<SqlParameter>();
                        spamEdit.Add(new SqlParameter("@id", ID));//New ID
                        spamEdit.Add(new SqlParameter("@insDate", this.inspdate.Value));
                        spamEdit.Add(new SqlParameter("@article", this.article.Text));
                        spamEdit.Add(new SqlParameter("@insor", loginID));
                        spamEdit.Add(new SqlParameter("@remark", this.remark.Text));
                        upResult= DBProxy.Current.Execute(null, editCmd,spamEdit);
                }
            }

            return base.OnSave();

        }
       
        protected override void OnInsert()
        {
           
            DataTable dt;
            DataTable dtGrid = (DataTable)gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select * from oven_detail where id='{0}'", ID), out dt);
            int rows = dt.Rows.Count;
            base.OnInsert();           
            if (rows<=0)
	        {
                dtGrid.Rows[0]["OvenGroup"] = 01;
	        }
            else
            {
                int group = MyUtility.Convert.GetInt(dtGrid.Rows[rows-1]["ovengroup"]);

                dtGrid.Rows[rows]["ovengroup"] = group + 1;
            }
            
        }
        #region 表頭Article 右鍵事件: 1.右鍵selectItem 2.判斷validated
        private void article_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.EditMode == false) return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string cmd =
@"select a.Article from Order_Qty a
left join Oven b on a.ID=b.POID
where a.id=@poid
group by a.Article";
                List<SqlParameter> spm = new List<SqlParameter>();
                spm.Add(new SqlParameter("@poid", PoID));
                SelectItem item = new SelectItem(cmd, spm, "12", null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                    
                }
                this.article.Text = item.GetSelectedString();
            }
        }
       
        private void article_Validated(object sender, EventArgs e)
        {
            DualResult dresult;
            DataTable dt;
            string cmd = "select * from order_qty where article=@art";
            List<SqlParameter> spm = new List<SqlParameter>();
            spm.Add(new SqlParameter("@art", article.Text));
            if (dresult = DBProxy.Current.Select(null,cmd,spm,out dt))
            {
                if (dt.Rows.Count<=0)
                {
                    MyUtility.Msg.InfoBox("Article doesn't exist in orders");
                    article.Text = "";
                    article.Focus();
                }
            }
            else
            {
                return;
            }
        }
#endregion

        private void encode_btn_Click(object sender, EventArgs e)
        {
            DataTable dt =(DataTable)gridbs.DataSource;            
            bool result = true;

            if (this.encode_btn.Text == "Encode")
            {
                if (dt.Rows.Count <= 0)
	            {
                    MyUtility.Msg.InfoBox("Data is empty please Append first!");
                    return;
	            }

                else
                {
                    foreach (DataRow dr in ((DataTable)gridbs.DataSource).Rows)
                    {
                        if (MyUtility.Check.Empty(dr["ovenGroup"]))
                        {
                            MyUtility.Msg.InfoBox("<Group> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["Seq"]))
                        {
                            MyUtility.Msg.InfoBox("<SEQ> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["Changescale"]))
                        {
                            MyUtility.Msg.InfoBox("<Color Change Scale> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["StainingScale"]))
                        {
                            MyUtility.Msg.InfoBox("<Color Staining Scale> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["Result"]))
                        {
                            MyUtility.Msg.InfoBox("<Result> can not be empty!");
                            return;
                        }
                        if (dr["Result"].ToString().Trim() == "Fail")
                        {
                            result = false;
                            DBProxy.Current.Execute(null, string.Format("update oven set result='Fail',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));

                        }
                        if (dr["Result"].ToString().Trim()== "Pass" && result)
                        {
                            DBProxy.Current.Execute(null, string.Format("update oven set result='Pass',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));
                            
                        }
                    }
                }
              
            }
            // Amend
            else
            {
                DBProxy.Current.Execute(null, string.Format("update oven set result='',status='New',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dt.Rows[0]["id"]));
            }
            OnRequery();
        }

        private void save_Click(object sender, EventArgs e)
        {

        }
         
        private void ToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridbs.DataSource;
            string[] columnNames = new string[] { "OvenGroup", "SEQ", "Roll", "Dyelot", "SCIRefno", "Colorid", "Supplier", "Changescale", "StainingScale", "Result","Remark" };
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, grid.Columns.Count) as object[,];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                for (int j = 0; j < columnNames.Length; j++)
                {
                    ret[i, j] = row[columnNames[j]];
                }
            }
            DataTable dtPo;
            DualResult sResult;
            if (sResult = DBProxy.Current.Select(null, string.Format("select * from PO where id='{0}'", PoID), out dtPo))
            {
                if (dtPo.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\P05_Detail_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 2] = this.poid.Text.ToString();
            worksheet.Cells[1, 4] = dtPo.Rows[0]["StyleID"].ToString();
            worksheet.Cells[1, 6] = dtPo.Rows[0]["SeasonID"].ToString();
            worksheet.Cells[1, 8] = this.article.Text.ToString();
            worksheet.Cells[1, 10] = this.testno.Text.ToString();
            worksheet.Cells[2, 2] = dtOven.Rows[0]["status"].ToString();
            worksheet.Cells[2, 4] = this.comboBox1.Text;
            worksheet.Cells[2, 6] = this.inspdate.Value;
            worksheet.Cells[2, 8] = this.txtuser1.TextBox1.Text.ToString();
            worksheet.Cells[2, 10] = dtPo.Rows[0]["BrandID"].ToString();

            int StartRow = 4;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet.Cells[StartRow + i, 1] = ret[i, 0];
                worksheet.Cells[StartRow + i, 2] = ret[i, 1];
                worksheet.Cells[StartRow + i, 3] = ret[i, 2];
                worksheet.Cells[StartRow + i, 4] = ret[i, 3];
                worksheet.Cells[StartRow + i, 5] = ret[i, 4];
                worksheet.Cells[StartRow + i, 6] = ret[i, 5];
                worksheet.Cells[StartRow + i, 7] = ret[i, 6];
                worksheet.Cells[StartRow + i, 8] = ret[i, 7];
                worksheet.Cells[StartRow + i, 9] = ret[i, 8];
                worksheet.Cells[StartRow + i, 10] = ret[i, 9];
                worksheet.Cells[StartRow + i, 11] = ret[i, 10];
            }

            worksheet.Cells.EntireColumn.AutoFit();
            worksheet.Cells.EntireRow.AutoFit();

            worksheet.Select();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;

            if (excel != null) Marshal.FinalReleaseComObject(excel); //釋放sheet
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet); //釋放objApp

        }
              
    }
}
