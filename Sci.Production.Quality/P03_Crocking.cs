using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P03_Crocking : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;
        private string ID;
        

        public P03_Crocking(bool canedit, string id, string keyvalue2, string keyvalue3,DataRow mainDr)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            maindr = mainDr;
            ID = id.Trim();
            
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            button_enable();
        }
        protected override DualResult OnRequery()
        {
            mainDBQuery();
            #region Encode Enable
            button_enable();
            encode_button.Text = MyUtility.Convert.GetBool(maindr["CrockingEncode"]) ? "Amend" : "Encode";
            
            #endregion
            //表頭 資料設定
            this.save.Enabled = !MyUtility.Convert.GetBool(maindr["CrockingEncode"]);
            string fir_cmd = string.Format(
                @"select distinct a.Poid,a.SEQ1+a.SEQ2 as seq,a.ArriveQty,
				b.styleid,b.BrandID,c.ExportId,c.WhseArrival,f.SuppID,a.SCIRefno,a.Refno,d.ColorID,
				e.CrockingDate,e.Crocking,e.nonCrocking												
				 from FIR a
				left join Orders b on a.POID=b.POID
				left join Receiving c on a.ReceivingID=c.Id
				left join PO_Supp_Detail d on d.ID=a.POID and a.SEQ1=d.SEQ1 and a.seq2=d.SEQ2
				left join FIR_Laboratory e on a.ID=e.ID
                left join PO_Supp f on d.ID=f.ID and d.SEQ1=f.SEQ1
				where a.ID='{0}'", ID);
            DataRow fir_dr;
            if (MyUtility.Check.Seek(fir_cmd, out fir_dr))
            {
                sptext.Text = fir_dr["Poid"].ToString();
                SEQtext.Text = fir_dr["SEQ"].ToString();
                AQtytext.Text = fir_dr["ArriveQty"].ToString();
                Wknotext.Text = fir_dr["exportid"].ToString();
                Arrdate.Value = MyUtility.Convert.GetDate(fir_dr["WhseArrival"]);
                Styletext.Text = fir_dr["styleid"].ToString();
                Brandtext.Text = fir_dr["Brandid"].ToString();
                Supptext.TextBox1.Text = fir_dr["SuppID"].ToString();
                SRnotext.Text = fir_dr["Scirefno"].ToString();
                BRnotext.Text = fir_dr["Refno"].ToString();
                Colortext.Text = fir_dr["colorid"].ToString();
                LIDate.Value=MyUtility.Convert.GetDate(fir_dr["CrockingDate"]);
                ResultText.Text = fir_dr["Crocking"].ToString();
                checkBox1.Value = fir_dr["nonCrocking"].ToString();
            }
            else
            {
                sptext.Text = "";  SEQtext.Text = ""; AQtytext.Text = ""; Wknotext.Text = ""; Arrdate.Text = ""; Styletext.Text = ""; Brandtext.Text = "";
                Supptext.Text = ""; SRnotext.Text = ""; BRnotext.Text = ""; Colortext.Text = "";
            }         
      

            return base.OnRequery();
        }
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("Poid", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));
            datas.Columns.Add("NewKey", typeof(int));
            datas.Columns.Add("Last update", typeof(string));
            int i = 0;
            foreach (DataRow dr in datas.Rows)
            {
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["NewKey"] = i;
                dr["poid"] = maindr["poid"];
                dr["SEQ1"] = maindr["SEQ1"];
                dr["SEQ2"] = maindr["SEQ2"];
                dr["Last update"] = datas.Rows[i]["EditName"].ToString() +" - "+ datas.Rows[i]["EditDate"].ToString();
                i++;
            }
           
        }
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings Rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings dryScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings wetScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings LabTechCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ResultCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings InspDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings InspectorCell = new DataGridViewGeneratorTextColumnSettings();

            #region grid MouseClickEvent
            Rollcell.EditingMouseDown += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    if (this.EditMode == false) return;
                    if (e.Button==System.Windows.Forms.MouseButtons.Right)
                    {
                        DataRow dr = grid.GetDataRow(e.RowIndex);
                        string sqlcmd = string.Format(@"Select roll,dyelot from Receiving_Detail Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"]);
                        SelectItem item = new SelectItem(sqlcmd, "15,12", dr["roll"].ToString(),false,",");
                        DialogResult result = item.ShowDialog();
                        if (result==DialogResult.Cancel)
                        {
                            return;
                        }
                        e.EditingControl.Text = item.GetSelectedString();//將選取selectitem value帶入GridView
                    }
                };

                dryScaleCell.EditingMouseDown += (s, e) =>
                    {
                        if (e.RowIndex == -1) return;
                        if (this.EditMode == false) return;
                        if (e.Button==System.Windows.Forms.MouseButtons.Right)
                        {
                            DataRow dr = grid.GetDataRow(e.RowIndex);
                            string scalecmd = @"select id from Scale where junk!=1";
                            SelectItem item1 = new SelectItem(scalecmd, "15", dr["DryScale"].ToString());
                            DialogResult result = item1.ShowDialog();
                            if (result==DialogResult.Cancel)
                            {
                                return;
                            }
                            e.EditingControl.Text = item1.GetSelectedString();
                            dr["DryScale"] = item1.GetSelectedString(); 
                        }
                    
                    };

              
                wetScaleCell.EditingMouseDown += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    if (this.EditMode == false) return;
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        DataRow dr = grid.GetDataRow(e.RowIndex);
                        string scalecmd = @"select id from Scale where junk!=1";
                        SelectItem item1 = new SelectItem(scalecmd, "15", dr["WetScale"].ToString());
                        DialogResult result = item1.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }
                        e.EditingControl.Text = item1.GetSelectedString(); //將選取selectitem value帶入GridView
                        dr["WetScale"] = item1.GetSelectedString();
                    }

                };
                LabTechCell.EditingMouseDown += (s, e) =>
                { 
                    if (e.RowIndex == -1) return;
                    if (this.EditMode == false) return;
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        DataRow dr = grid.GetDataRow(e.RowIndex);
                        string scalecmd = @"select id,name from Pass1 ";
                        SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                        DialogResult result = item1.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }
                        e.EditingControl.Text = item1.GetSelectedString(); //將選取selectitem value帶入GridView
                        dr["Inspector"] = item1.GetSelectedString();
                    }
                };
                              
                ResultCell.CellMouseDoubleClick+=(s,e)=>
                {
                    if (!this.EditMode) return;
                    DataRow dr = grid.GetDataRow(e.RowIndex);

                    if (dr["Result"].ToString() == "PASS") dr["Result"] = "FAIL";
                    else dr["Result"] = "PASS";
                };
           
            #endregion
                #region Valid 檢驗
                Rollcell.CellValidating += (s, e) =>
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string oldvalue = dr["Roll"].ToString();
                    string newvalue = e.FormattedValue.ToString();
                    if (this.EditMode == false) return;
                    if (dr.RowState!=DataRowState.Added) {
                        if (oldvalue == newvalue) return;
                    }                   
                    
                    string roll_cmd = string.Format("Select roll,Poid,seq1,seq2,dyelot from Receiving_Detail Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"], e.FormattedValue);
                    DataRow roll_dr;
                    if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                    {
                        dr["Roll"] = roll_dr["Roll"];
                        dr["Dyelot"] = roll_dr["Dyelot"];
                        dr.EndEdit();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("<Roll> data not found!");
                        dr["Roll"] = "";
                        dr["Dyelot"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                };

                dryScaleCell.CellValidating += (s, e) =>
                    {
                        DataRow dr = grid.GetDataRow(e.RowIndex);
                        string oldvalue = dr["DryScale"].ToString();
                        string newvalue = e.FormattedValue.ToString();
                        if (this.EditMode == false) return;
                        if (dr.RowState != DataRowState.Added)
                        {
                            if (oldvalue == newvalue) return;
                        }  
                        
                        string dryScale_cmd = string.Format(@"	select DryScale from FIR_Laboratory_Crocking a left join Scale b on a.DryScale=b.id where a.id ='{0}'", maindr["id"]);
                        DataRow roll_dr;
                        if (!MyUtility.Check.Seek(dryScale_cmd, out roll_dr))
                        {
                             MyUtility.Msg.WarningBox("<Dry Scale> data not found!");
                             dr["DryScale"] = "";
                             dr.EndEdit();
                             e.Cancel = true;
                             return;
                        }
                     
                    };
                wetScaleCell.CellValidating += (s, e) =>
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string oldvalue = dr["wetScale"].ToString();
                    string newvalue = e.FormattedValue.ToString();
                    if (this.EditMode == false) return;
                    if (dr.RowState != DataRowState.Added)
                    {
                        if (oldvalue == newvalue) return;
                    }  
                    
                    string dryScale_cmd = string.Format(@"select wetScale from FIR_Laboratory_Crocking a left join Scale b on a.DryScale=b.id where a.id ='{0}'", maindr["id"]);
                    DataRow roll_dr;
                    if (!MyUtility.Check.Seek(dryScale_cmd, out roll_dr))
                    {
                        MyUtility.Msg.WarningBox("<Wet Scale> data not found!");
                        dr["wetScale"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                
                };
                LabTechCell.CellValidating += (s, e) =>
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string oldvalue = dr["inspector"].ToString();
                    string newvalue = e.FormattedValue.ToString();
                    if (this.EditMode == false) return;
                    if (dr.RowState != DataRowState.Added)
                    {
                        if (oldvalue == newvalue) return;
                    }                      
                    string dryScale_cmd = string.Format(@"select Inspector from FIR_Laboratory_Crocking a	left join Pass1 b on a.Inspector=b.ID and b.Resign is not null where a.id ='{0}'", maindr["id"]);
                    DataRow roll_dr;
                    if (!MyUtility.Check.Seek(dryScale_cmd, out roll_dr))
                    {

                        MyUtility.Msg.WarningBox("<Inspector> data not found!");
                        dr["Inspector"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                   
                };
                ResultCell.CellValidating += (s, e) =>
                {
                    string result_cmd = string.Format(@"select result from FIR_Laboratory_Crocking where id ='{0}'", maindr["id"]);
                    DataRow drResult;
                    if (!MyUtility.Check.Seek(result_cmd,out drResult))
                    {
                        MyUtility.Msg.WarningBox("<Result> cannot be empty!");
                        return;
                    }
                };
                InspDateCell.CellValidating += (s, e) =>
                {
                    string result_cmd = string.Format(@"select inspdate from FIR_Laboratory_Crocking where id ='{0}'", maindr["id"]);
                    DataRow drResult;
                    if (!MyUtility.Check.Seek(result_cmd, out drResult))
                    {
                        MyUtility.Msg.WarningBox("<inspdate> cannot be empty!");
                        return;
                    }
                };                               
                
                #endregion

                Helper.Controls.Grid.Generator(this.grid)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), settings: Rollcell)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("DryScale", header: "Dry Scale", width: Widths.AnsiChars(5), settings: dryScaleCell,iseditingreadonly:true)
                .Text("WetScale", header: "Wet Scale", width: Widths.AnsiChars(5), settings: wetScaleCell,iseditingreadonly:true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(5), settings: ResultCell,iseditingreadonly:true)
                .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10), settings: InspDateCell)
                .Text("Inspector", header: "Lab Tech", width: Widths.AnsiChars(16),iseditingreadonly:true,settings:LabTechCell)
                .CellUser("Inspector", header: "Name", width: Widths.AnsiChars(10), userNamePropertyName: "Name",iseditingreadonly:true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(16))
                .Text("Last update", header: "Last update",width: Widths.AnsiChars(50), iseditingreadonly: true);
                       

            return true;
        }

        
        protected override void OnInsert()
        {
            DataTable dt = (DataTable)gridbs.DataSource;
            
            int Maxi = MyUtility.Convert.GetInt(dt.Compute("Max(NewKey)", ""));
            base.OnInsert();

            DataRow selectDr = ((DataRowView)grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", loginID, "Pass1", "ID");
            selectDr["NewKey"] = Maxi + 1;
            selectDr["poid"] = maindr["poid"];
            selectDr["SEQ1"] = maindr["SEQ1"];
            selectDr["SEQ2"] = maindr["SEQ2"];
        }
        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)gridbs.DataSource;

            DataTable afterDT = new DataTable();
            //將將刪除資料過的grid 重新丟進新datatable 並將資料完全刪除來做判斷! 
            afterDT.Merge(gridTb, true);
            afterDT.AcceptChanges();
         
            
            #region 判斷空白不可存檔
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["Roll"])))
            {
                MyUtility.Msg.WarningBox("<Roll> can not be empty.");
                return false;
            }
            else
            {
                foreach (DataRow dr in afterDT.Rows)
                {                                  
                    DataRow[] drArray = afterDT.Select(string.Format("Roll='{0}'", MyUtility.Convert.GetString(dr["Roll"])));
                    if (drArray.Length > 1)
                    {
                        MyUtility.Msg.WarningBox("<Roll>" + MyUtility.Convert.GetString(dr["Roll"]) + " is already exist ! ");
                        return false;
                    }                                    
                }       
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["DryScale"])))
            {
                MyUtility.Msg.WarningBox("<Dry Scale> can not be empty.");
                return false;

            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["WetScale"])))
            {
                MyUtility.Msg.WarningBox("<WetScale> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["Result"])))
            {
                MyUtility.Msg.WarningBox("<Result> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["Inspdate"])))
            {
                MyUtility.Msg.WarningBox("<Insection Date> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["inspector"])))
            {
                MyUtility.Msg.WarningBox("<Inspector> can not be empty.");
                return false;
            }
          
            #endregion
            return base.OnSaveBefore();
        }
        protected override DualResult OnSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = "";
          
            foreach (DataRow dr in ((DataTable)gridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From FIR_Laboratory_Crocking Where id =@id and Roll=@roll ";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@roll", dr["Roll", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    if (!upResult) { return upResult; }
                    continue;
                }
                //轉換時間型態
                string inspdate;
                if (MyUtility.Check.Empty(dr["InspDate"])) inspdate = ""; //判斷Inspect Date
                else inspdate = string.Format("{0:yyyy-MM-dd}", dr["InspDate"]);
                DateTime Today =  DateTime.Now;
                if (dr.RowState == DataRowState.Added)
                {
                    List<SqlParameter> spamAdd = new List<SqlParameter>();
                    update_cmd = @"insert into FIR_Laboratory_Crocking(ID,roll,Dyelot,DryScale,WetScale,Inspdate,Inspector,Result,Remark,AddDate,AddName)
                    values(@ID,@roll,@Dyelot,@DryScale,@WetScale,@Inspdate,@Inspector,@Result,@Remark,@AddDate,@AddName)";
                    spamAdd.Add(new SqlParameter("@id", dr["ID"]));
                    spamAdd.Add(new SqlParameter("@roll", dr["roll"]));
                    spamAdd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamAdd.Add(new SqlParameter("@DryScale", dr["DryScale"]));
                    spamAdd.Add(new SqlParameter("@WetScale", dr["WetScale"]));
                    spamAdd.Add(new SqlParameter("@Inspdate", inspdate));
                    spamAdd.Add(new SqlParameter("@Inspector", loginID));
                    spamAdd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamAdd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamAdd.Add(new SqlParameter("@AddDate", Today));
                    spamAdd.Add(new SqlParameter("@AddName", loginID));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamAdd);
                    if (!upResult) { return upResult; }
                }
                if (dr.RowState == DataRowState.Modified)
                {
                    List<SqlParameter> spamUpd = new List<SqlParameter>();
                    update_cmd = @"update FIR_Laboratory_Crocking
                    set ID=@ID,roll=@roll,Dyelot=@Dyelot,DryScale=@DryScale,WetScale=@WetScale,Inspdate=@Inspdate,Inspector=@Inspector,
                        Result=@Result,Remark=@Remark,EditDate=@EditDate,EditName=@EditName
                        where id=@id and roll=@rollbefore";

                    spamUpd.Add(new SqlParameter("@id", dr["ID"]));
                    spamUpd.Add(new SqlParameter("@roll", dr["roll"]));
                    spamUpd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamUpd.Add(new SqlParameter("@DryScale", dr["DryScale"]));
                    spamUpd.Add(new SqlParameter("@WetScale", dr["WetScale"]));
                    spamUpd.Add(new SqlParameter("@Inspdate", inspdate));
                    spamUpd.Add(new SqlParameter("@Inspector", dr["Inspector"]));
                    spamUpd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamUpd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamUpd.Add(new SqlParameter("@EditDate", Today));
                    spamUpd.Add(new SqlParameter("@EditName", loginID));
                    spamUpd.Add(new SqlParameter("@rollbefore", dr["Roll", DataRowVersion.Original]));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamUpd);
                    if (!upResult) { return upResult; }

                }
            }
          
            return upResult;
           
        }   

        private void encode_button_Click(object sender, EventArgs e)
        {
            string updatesql = "";
            if (!MyUtility.Convert.GetBool(maindr["CrockingEncode"]))//Encode
            {
                if (!MyUtility.Convert.GetBool(maindr["nonCrocking"]))//判斷有勾選可Encode
                {
                    //至少檢驗一卷 並且出現在Fir_Continuity.Roll
                    DataTable rolldt;
                    string cmd= string.Format(
                        @"Select roll from Receiving_Detail a where 
                        a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
                        and exists 
                        (Select distinct dyelot from FIR_Continuity b where b.id='{1}' and a.roll = b.roll)"
                        , maindr["receivingid"], maindr["id"], maindr["POID"], maindr["seq1"], maindr["seq2"]);
                    DualResult dResult;
                    if (dResult =  DBProxy.Current.Select(null, cmd, out rolldt))
                    {
                        if (rolldt.Rows.Count < 1)
                        {
                            MyUtility.Msg.WarningBox("Each Roll must be in Physical Contiunity");
                            return;
                        }
                    }
                }
                #region 判斷Crocking Result
                DataTable gridDt = (DataTable)gridbs.DataSource;
                DataRow[] ResultAry = gridDt.Select("Result='Fail'");
                string result = "Pass";                
                string Today = DateTime.Now.ToShortDateString();
                if (ResultAry.Length > 0) result = "Fail";
                #endregion

                #region 判斷表身最晚時間
                DataTable dt = (DataTable)gridbs.DataSource;
                DateTime lastDate = Convert.ToDateTime(dt.Rows[0]["inspDate"]);
                for (int i = 0; i < dt.Rows.Count; i++)
                {                    
                    DateTime newDate = Convert.ToDateTime(dt.Rows[i]["inspDate"]);
                    //代表newDate 比  lastDate還晚 就取代lastDate
                    if (DateTime.Compare(newDate, lastDate) > 0)
                    {
                        lastDate = newDate;
                    }                    
                }
                #endregion
               
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir_Laboratory set CrockingEncode=1,Crocking='{0}',CrockingDate ='{2}' where id ='{1}'",
                result, maindr["ID"], lastDate.ToShortDateString());

                updatesql = updatesql + string.Format(@"update FIR_Laboratory_Crocking set inspDate='{1}' where id='{0}'", maindr["ID"], Today);
                #endregion                           

            }
            else //Amend
            {                              
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir_Laboratory set CrockingDate = null,CrockingEncode= 0 where id ='{0}'",  maindr["ID"]);
              
                #endregion
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.InfoBox("Error Message：" + upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #region Over All Result 寫入
            string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Lab(maindr["ID"]);
            maindr["Result"] = returnstr[0];
            string cmdResult = @"update Fir_Laboratory set Result=@Result where id=@id";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@Result", returnstr[0]));
            spam.Add(new SqlParameter("@id", maindr["ID"]));
            DBProxy.Current.Execute(null, cmdResult, spam);
            #endregion

            OnRequery();
           
        }

        private void button_enable()
        {
            //return;
            if (maindr == null) return;
            encode_button.Enabled = this.CanEdit && !this.EditMode;
            this.ToExcelBtn.Enabled = this.CanEdit && !this.EditMode;
            this.Supptext.TextBox1.ReadOnly = true;
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P03", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            int lApprove = 0; //有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }

        }

        private void ToExcelBtn_Click(object sender, EventArgs e)
        {           
            DataTable dt = (DataTable)gridbs.DataSource;
            string[] columnNames = new string[] { "Roll","Dyelot","DryScale","WetScale","Result","InspDate","Inspector","Remark","Last update" };
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, grid.Columns.Count) as object[,];
            for( int i = 0;i <dt.Rows.Count; i++) {
                DataRow row = dt.Rows[i];           
                for (int j=0; j<columnNames.Length; j++) {
                    ret[i,j] = row[columnNames[j]];
                }
            }
          
            DataTable dtSeason;
            DualResult sResult;         
            if (sResult = DBProxy.Current.Select("Production", string.Format(
            "select C.SeasonID from FIR_Shadebone a left join FIR b on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{0}'", maindr["ID"]), out dtSeason))
            {
                if (dtSeason.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }

            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            MyUtility.Excel.CopyToXls(ret, xltFileName:"P03_Crocking_Test.xltx", headerline:5, excelAppObj:excel);
            Microsoft.Office.Interop.Excel.Worksheet excelSheets = excel.ActiveWorkbook.Worksheets[1];// 取得工作表      
            excel.Cells[2, 2] = sptext.Text.ToString();
            excel.Cells[2, 4] = SEQtext.Text.ToString();
            excel.Cells[2, 6] = Colortext.Text.ToString();
            excel.Cells[2, 8] = Styletext.Text.ToString();
            excel.Cells[2, 10] = dtSeason.Rows[0]["SeasonID"];
            excel.Cells[3, 2] = SRnotext.Text.ToString();
            excel.Cells[3, 4] = Wknotext.Text.ToString();
            excel.Cells[3, 6] = ResultText.Text.ToString();
            excel.Cells[3, 8] = LIDate.Value;
            excel.Cells[3, 10] = Brandtext.Text.ToString();
            excel.Cells[4, 2] = BRnotext.Text.ToString();
            excel.Cells[4, 4] = AQtytext.Text.ToString();
            excel.Cells[4, 6] = Arrdate.Value;
            excel.Cells[4, 8] = Supptext.DisplayBox1.Text.ToString();
            excel.Cells[4, 10] = checkBox1.Value.ToString();

            excel.Cells.EntireColumn.AutoFit();    //自動欄寬
            excel.Cells.EntireRow.AutoFit();       ////自動欄高


            if (excelSheets != null) Marshal.FinalReleaseComObject(excelSheets);//釋放sheet
            if (excel != null) Marshal.FinalReleaseComObject(excel);          //釋放objApp
           
        }
        //maindr where id,poid重新query 
        private void mainDBQuery()
        {

            string cmd = @"select a.id,a.poid,(a.SEQ1+a.SEQ2) as seq,a.SEQ1,a.SEQ2,Receivingid,Refno,a.SCIRefno,
                b.CrockingEncode,b.HeatEncode,b.WashEncode,
                ArriveQty,
				 (
                Select d.colorid from PO_Supp_Detail d Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as Colorid,
				(
				select Suppid+f.AbbEN as supplier from Supp f where a.Suppid=f.ID
				) as Supplier,
				b.ReceiveSampleDate,b.InspDeadline,b.Result,b.Crocking,b.nonCrocking,b.CrockingDate,b.nonHeat,Heat,b.HeatDate,
				b.nonWash,b.Wash,b.WashDate
				from FIR a 
				left join FIR_Laboratory b on a.ID=b.ID
				left join Receiving c on c.id = a.receivingid
				Where a.poid=@poid  and a.id=@id order by a.seq1,a.seq2,Refno ";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@id", ID));
            spam.Add(new SqlParameter("@poid", maindr["poid"]));
            if (!MyUtility.Check.Seek(cmd, spam, out maindr))
            {
                MyUtility.Msg.InfoBox("Data is empty");
            }

        }


    
     
    }
}
