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
using Sci.Production.PublicPrg;
using System.Diagnostics;

namespace Sci.Production.Quality
{
    public partial class P03_Crocking : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;
        private string ID;

        public P03_Crocking(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr)
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
            button_enable();

            //表頭 資料設定
            this.save.Enabled = !MyUtility.Convert.GetBool(maindr["CrockingEncode"]);
            string fir_cmd = string.Format(
@"select distinct 
    a.Poid,a.ArriveQty,a.SCIRefno,a.Refno,
	seq = CONCAT(a.SEQ1,a.SEQ2),
    b.styleid,b.BrandID,
    c.ExportId,c.WhseArrival,
    d.ColorID,
    e.CrockingDate,e.Crocking,e.nonCrocking,
    f.SuppID,
    g.DescDetail
from FIR a WITH (NOLOCK) 
left join Orders b WITH (NOLOCK) on a.POID=b.POID
left join Receiving c WITH (NOLOCK) on a.ReceivingID=c.Id
left join PO_Supp_Detail d WITH (NOLOCK) on d.ID=a.POID and a.SEQ1=d.SEQ1 and a.seq2=d.SEQ2
left join FIR_Laboratory e WITH (NOLOCK) on a.ID=e.ID
left join PO_Supp f WITH (NOLOCK) on d.ID=f.ID and d.SEQ1=f.SEQ1
left join Fabric g WITH (NOLOCK) on g.SCIRefno = a.SCIRefno
				where a.ID='{0}'", ID);
            DataRow fir_dr;
            if (MyUtility.Check.Seek(fir_cmd, out fir_dr))
            {
                txtSP.Text = fir_dr["Poid"].ToString();
                txtSEQ.Text = fir_dr["SEQ"].ToString();
                txtArriveQty.Text = fir_dr["ArriveQty"].ToString();
                txtWkno.Text = fir_dr["exportid"].ToString();
                dateArriveWHDate.Value = MyUtility.Convert.GetDate(fir_dr["WhseArrival"]);
                txtStyle.Text = fir_dr["styleid"].ToString();
                txtBrand.Text = fir_dr["Brandid"].ToString();
                txtsupplierSupp.TextBox1.Text = fir_dr["SuppID"].ToString();
                txtSCIRefno.Text = fir_dr["Scirefno"].ToString();
                txtBrandRefno.Text = fir_dr["Refno"].ToString();
                txtColor.Text = fir_dr["colorid"].ToString();
                dateLastInspectionDate.Value = MyUtility.Convert.GetDate(fir_dr["CrockingDate"]);
                txtResult.Text = fir_dr["Crocking"].ToString();
                checkNA.Value = fir_dr["nonCrocking"].ToString();
                editDescription.Text = fir_dr["DescDetail"].ToString();
            }
            else
            {
                txtSP.Text = ""; txtSEQ.Text = ""; txtArriveQty.Text = ""; txtWkno.Text = ""; dateArriveWHDate.Text = ""; txtStyle.Text = ""; txtBrand.Text = "";
                txtsupplierSupp.Text = ""; txtSCIRefno.Text = ""; txtBrandRefno.Text = ""; txtColor.Text = ""; editDescription.Text = "";
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
                dr["Name"] = MyUtility.GetValue.Lookup("Name_Extno", dr["Inspector"].ToString(), "View_ShowName", "ID");
                dr["NewKey"] = i;
                dr["poid"] = maindr["poid"];
                dr["SEQ1"] = maindr["SEQ1"];
                dr["SEQ2"] = maindr["SEQ2"];
                string name = MyUtility.Check.Empty(datas.Rows[i]["EditName"].ToString()) ? MyUtility.GetValue.Lookup("Name_Extno", datas.Rows[i]["AddName"].ToString(), "View_ShowName", "ID") :
                   MyUtility.GetValue.Lookup("Name_Extno", datas.Rows[i]["EditName"].ToString(), "View_ShowName", "ID");
                string Date=MyUtility.Check.Empty(datas.Rows[i]["EditDate"].ToString()) ? datas.Rows[i]["AddDate"].ToString() : datas.Rows[i]["EditDate"].ToString();
                dr["Last update"] = name + " - " + Date;
            
                i++;
            }

        }

        Ict.Win.UI.DataGridViewTextBoxColumn ResultCell;
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings Rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings dryScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings wetScaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings LabTechCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings InspDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings InspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings Resultdry = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings Resultwet = new DataGridViewGeneratorTextColumnSettings();

            #region grid MouseClickEvent
            Rollcell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string sqlcmd = string.Format(@"Select roll,dyelot from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"]);
                    SelectItem item = new SelectItem(sqlcmd, "15,12", dr["roll"].ToString(), false, ",");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Roll"] = item.GetSelecteds()[0]["Roll"].ToString();
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString();
                }
            };

            dryScaleCell.EditingMouseDown += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    if (this.EditMode == false) return;
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        DataRow dr = grid.GetDataRow(e.RowIndex);
                        string scalecmd = @"select id from Scale WITH (NOLOCK) where junk!=1";
                        SelectItem item1 = new SelectItem(scalecmd, "15", dr["DryScale"].ToString());
                        DialogResult result = item1.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }                        
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
                    string scalecmd = @"select id from Scale WITH (NOLOCK) where junk!=1";
                    SelectItem item1 = new SelectItem(scalecmd, "15", dr["WetScale"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }                    
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
                    string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) ";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }                    
                    dr["Inspector"] = item1.GetSelectedString();
                }
            };

            Resultdry.CellMouseDoubleClick += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (dr["Resultdry"].ToString().ToUpper() == "PASS")
                {
                    dr["Resultdry"] = "Fail";
                }
                else dr["Resultdry"] = "Pass";
                resultchange();
            };
            Resultwet.CellMouseDoubleClick += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (dr["Resultwet"].ToString().ToUpper() == "PASS")
                {
                    dr["Resultwet"] = "Fail";
                }
                else dr["Resultwet"] = "Pass";
                resultchange();
            };
            #endregion

            #region Valid 檢驗
            Rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return 
                if (MyUtility.Check.Empty(e.FormattedValue))//沒填入資料,清空dyelot
                {
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    return;
                }
                if (dr.RowState != DataRowState.Added)
                {
                    if (oldvalue == newvalue) return;
                }

                string roll_cmd = string.Format("Select roll,Poid,seq1,seq2,dyelot from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"], e.FormattedValue);
                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr.EndEdit();
                }
                else
                {
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> data not found!", e.FormattedValue));
                    return;
                }                
            };

            dryScaleCell.CellValidating += (s, e) =>
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string oldvalue = dr["DryScale"].ToString();
                    string newvalue = e.FormattedValue.ToString();
                    if (!this.EditMode) return;//非編輯模式 
                    if (e.RowIndex == -1) return; //沒東西 return
                    if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return
                    if (dr.RowState != DataRowState.Added)
                    {
                        if (oldvalue == newvalue) return;
                    }

                    string dryScale_cmd = string.Format(@"	select DryScale from FIR_Laboratory_Crocking a WITH (NOLOCK) left join Scale b WITH (NOLOCK) on a.DryScale=b.id where a.id ='{0}'", maindr["id"]);
                    DataRow roll_dr;
                    if (!MyUtility.Check.Seek(dryScale_cmd, out roll_dr))
                    {
                        dr["DryScale"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<Dry Scale: {0}> data not found!", e.FormattedValue));
                        return;
                    }

                };
            wetScaleCell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["wetScale"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return
                if (dr.RowState != DataRowState.Added)
                {
                    if (oldvalue == newvalue) return;
                }

                string dryScale_cmd = string.Format(@"select wetScale from FIR_Laboratory_Crocking a WITH (NOLOCK) left join Scale b WITH (NOLOCK) on a.DryScale=b.id where a.id ='{0}'", maindr["id"]);
                DataRow roll_dr;
                if (!MyUtility.Check.Seek(dryScale_cmd, out roll_dr))
                {
                    dr["wetScale"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Wet Scale: {0}> data not found!", e.FormattedValue));
                    return;
                }

            };
            LabTechCell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["inspector"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return
                if (dr.RowState != DataRowState.Added)
                {
                    if (oldvalue == newvalue) return;
                }
                string dryScale_cmd = string.Format(@"select Inspector from FIR_Laboratory_Crocking a WITH (NOLOCK) left join Pass1 b WITH (NOLOCK) on a.Inspector=b.ID and b.Resign is not null where a.id ='{0}'", maindr["id"]);
                DataRow roll_dr;
                if (!MyUtility.Check.Seek(dryScale_cmd, out roll_dr))
                {
                    dr["Inspector"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Inspector: {0}> data not found!", e.FormattedValue));
                    return;
                }

            };        
            InspDateCell.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    MyUtility.Msg.WarningBox("<inspdate> cannot be empty!");
                    e.Cancel = true;
                    return;
                }
            };

            #endregion

            Helper.Controls.Grid.Generator(this.grid)
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), settings: Rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true).Get(out ResultCell)
            .Text("DryScale", header: "Dry Scale", width: Widths.AnsiChars(5), settings: dryScaleCell, iseditingreadonly: true)
            .Text("ResultDry", header: "Result(Dry)", width: Widths.AnsiChars(5), settings: Resultdry, iseditingreadonly: true)
            .Text("WetScale", header: "Wet Scale", width: Widths.AnsiChars(5), settings: wetScaleCell, iseditingreadonly: true)
            .Text("ResultWet", header: "Result(Wet)", width: Widths.AnsiChars(5), settings: Resultwet, iseditingreadonly: true)
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10), settings: InspDateCell)
            .Text("Inspector", header: "Lab Tech", width: Widths.AnsiChars(16), iseditingreadonly: true, settings: LabTechCell)
            .CellUser("Name", header: "Name", width: Widths.AnsiChars(25), userNamePropertyName: "Name", iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(16))
            .Text("Last update", header: "Last update", width: Widths.AnsiChars(50), iseditingreadonly: true);
            return true;
        }

        private void resultchange()
        {
            if (MyUtility.Convert.GetString(this.CurrentData["ResultDry"]).EqualString("Pass") && MyUtility.Convert.GetString(this.CurrentData["ResultWet"]).EqualString("Pass"))
            {
                this.CurrentData["Result"] = "Pass";
            }
            else
            {
                this.CurrentData["Result"] = "Fail";
            }
            this.CurrentData.EndEdit();
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
                DateTime Today = DateTime.Now;
                if (dr.RowState == DataRowState.Added)
                {
                    List<SqlParameter> spamAdd = new List<SqlParameter>();
                    update_cmd = @"insert into FIR_Laboratory_Crocking(ID,roll,Dyelot,DryScale,WetScale,Inspdate,Inspector,Result,Remark,AddDate,AddName,ResultDry,ResultWet)
                    values(@ID,@roll,@Dyelot,@DryScale,@WetScale,@Inspdate,@Inspector,@Result,@Remark,@AddDate,@AddName,@ResultDry,@ResultWet)";
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
                    spamAdd.Add(new SqlParameter("@ResultDry", dr["ResultDry"]));
                    spamAdd.Add(new SqlParameter("@ResultWet", dr["ResultWet"]));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamAdd);
                    if (!upResult) { return upResult; }
                }
                if (dr.RowState == DataRowState.Modified)
                {
                    List<SqlParameter> spamUpd = new List<SqlParameter>();
                    update_cmd = @"update FIR_Laboratory_Crocking
                    set ID=@ID,roll=@roll,Dyelot=@Dyelot,DryScale=@DryScale,WetScale=@WetScale,Inspdate=@Inspdate,Inspector=@Inspector,
                        Result=@Result,Remark=@Remark,EditDate=@EditDate,EditName=@EditName,ResultDry=@ResultDry,ResultWet=@ResultWet
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
                    spamUpd.Add(new SqlParameter("@ResultDry", dr["ResultDry"]));
                    spamUpd.Add(new SqlParameter("@ResultWet", dr["ResultWet"]));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamUpd);
                    if (!upResult) { return upResult; }

                }
            }

            return upResult;

        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = "";
            if (MyUtility.Check.Empty(CurrentData) && !MyUtility.Convert.GetBool(maindr["nonCrocking"]))
            {
                MyUtility.Msg.WarningBox("Please test one Roll least");
                return;
            }
            if (!MyUtility.Convert.GetBool(maindr["CrockingEncode"]))//Encode
            {
                if (!MyUtility.Convert.GetBool(maindr["nonCrocking"]))//判斷有勾選可Encode
                {
                    //至少檢驗一卷 並且出現在FIR_Laboratory_Crocking.Roll
                    DataTable rolldt;
                    string cmd = string.Format(
                        @"Select roll from Receiving_Detail a WITH (NOLOCK) where 
                        a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
                        and exists 
                        (Select distinct dyelot from FIR_Laboratory_Crocking b WITH (NOLOCK) where b.id='{1}' and a.roll = b.roll)"
                        , maindr["receivingid"], maindr["id"], maindr["POID"], maindr["seq1"], maindr["seq2"]);
                    DualResult dResult;
                    if (dResult = DBProxy.Current.Select(null, cmd, out rolldt))
                    {
                        if (rolldt.Rows.Count < 1)
                        {
                            MyUtility.Msg.WarningBox("Each Roll must be in Physical Contiunity.");
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
                if (dt.Rows.Count != 0)
                {
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
                }
                else
                {

                    updatesql = string.Format(
                    @"Update Fir_Laboratory set CrockingEncode = 1,Crocking='{0}' where id ='{1}'", result, maindr["ID"]);

                }

                    #endregion

            }
            else //Amend
            {
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir_Laboratory set CrockingDate = null,CrockingEncode= 0,Crocking = '' where id ='{0}'", maindr["ID"]);

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
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
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
            if (maindr == null) return;
            btnEncode.Enabled = this.CanEdit && !this.EditMode;
            btnEncode.Text = MyUtility.Convert.GetBool(maindr["CrockingEncode"]) ? "Amend" : "Encode";
            this.btnToExcel.Enabled = !this.EditMode;
            this.btntoPDF.Enabled = !this.EditMode;
            this.txtsupplierSupp.TextBox1.ReadOnly = true;
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridbs.DataSource;
            string[] columnNames = new string[] { "Roll", "Dyelot", "DryScale", "WetScale", "Result", "InspDate", "Inspector", "Remark", "Last update" };
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, grid.Columns.Count) as object[,];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                for (int j = 0; j < columnNames.Length; j++)
                {
                    ret[i, j] = row[columnNames[j]];
                }
            }
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            //撈取seasonID
            DataTable dtSeason;
            string SeasonID;
            DBProxy.Current.Select("Production", string.Format(
            "select C.SeasonID from FIR_Laboratory_Crocking a WITH (NOLOCK) left join FIR_Laboratory b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C WITH (NOLOCK) ON B.POID=C.ID where a.ID='{0}'", maindr["ID"]), out dtSeason);
            if (dtSeason.Rows.Count == 0) { SeasonID = ""; }
            else { SeasonID = dtSeason.Rows[0]["SeasonID"].ToString(); }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Workbooks.Add();
            MyUtility.Excel.CopyToXls(ret, xltFileName: "Quality_P03_Crocking_Test.xltx", fileName: "", openfile: false, headerline: 5, excelAppObj: excel);
            Microsoft.Office.Interop.Excel.Worksheet excelSheets = excel.ActiveWorkbook.Worksheets[1];// 取得工作表      
            excel.Cells[2, 2] = txtSP.Text.ToString();
            excel.Cells[2, 4] = txtSEQ.Text.ToString();
            excel.Cells[2, 6] = txtColor.Text.ToString();
            excel.Cells[2, 8] = txtStyle.Text.ToString();
            excel.Cells[2, 10] = SeasonID;
            excel.Cells[3, 2] = txtSCIRefno.Text.ToString();
            excel.Cells[3, 4] = txtWkno.Text.ToString();
            excel.Cells[3, 6] = txtResult.Text.ToString();
            excel.Cells[3, 8] = dateLastInspectionDate.Value;
            excel.Cells[3, 10] = txtBrand.Text.ToString();
            excel.Cells[4, 2] = txtBrandRefno.Text.ToString();
            excel.Cells[4, 4] = txtArriveQty.Text.ToString();
            excel.Cells[4, 6] = dateArriveWHDate.Value;
            excel.Cells[4, 8] = txtsupplierSupp.DisplayBox1.Text.ToString();
            excel.Cells[4, 10] = checkNA.Value.ToString();

            excel.Cells.EntireColumn.AutoFit();    //自動欄寬
            excel.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(excelSheets);

            strExcelName.OpenFile();
            #endregion 
        }

        //maindr where id,poid重新query 
        private void mainDBQuery()
        {

            string cmd = @"select a.id,a.poid,(a.SEQ1+a.SEQ2) as seq,a.SEQ1,a.SEQ2,Receivingid,Refno,a.SCIRefno,
                b.CrockingEncode,b.HeatEncode,b.WashEncode,
                ArriveQty,
				 (
                Select d.colorid from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as Colorid,
				(
				select Suppid+f.AbbEN as supplier from Supp f WITH (NOLOCK) where a.Suppid=f.ID
				) as Supplier,
				b.ReceiveSampleDate,b.InspDeadline,b.Result,b.Crocking,b.nonCrocking,b.CrockingDate,b.nonHeat,Heat,b.HeatDate,
				b.nonWash,b.Wash,b.WashDate
				from FIR a WITH (NOLOCK) 
				left join FIR_Laboratory b WITH (NOLOCK) on a.ID=b.ID
				left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
				Where a.poid=@poid  and a.id=@id order by a.seq1,a.seq2,Refno ";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@id", ID));
            spam.Add(new SqlParameter("@poid", maindr["poid"]));
            if (!MyUtility.Check.Seek(cmd, spam, out maindr))
            {
                MyUtility.Msg.InfoBox("Data is empty.");
            }

        }

        private void btntoPDF_Click(object sender, EventArgs e)
        {
            string submitDate = MyUtility.Convert.GetString(this.maindr["ReceiveSampleDate"]);
            string sqlcmd = $@"
SELECT distinct oc.article,fd.InspDate,fd.DryScale,fd.ResultDry,fd.WetScale,fd.ResultWet,fd.Remark,fd.Inspector
FROM Order_BOF bof
inner join PO_Supp_Detail p on p.id=bof.id and bof.SCIRefno=p.SCIRefno
inner join Order_ColorCombo OC on oc.id=p.id and oc.FabricCode=bof.FabricCode
inner join orders o on o.id = bof.id
inner join FIR_Laboratory f on f.poid = o.poid and f.seq1 = p.seq1 and f.seq2 = p.seq2
inner join FIR_Laboratory_Crocking fd on fd.id = f.id
where bof.id='{maindr["POID"]}' and p.seq1='{maindr["seq1"]}' and p.seq2='{maindr["seq2"]}'
order by fd.InspDate,oc.article
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null,sqlcmd,out dt);

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_P03_Crocking_Test_for_PDF.xltx");
           
            objApp.DisplayAlerts = false;//設定Excel的警告視窗是否彈出
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                worksheet1.Copy(worksheetn);
            }

            int j = 0;
            foreach (DataRow row in dt.Rows)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[j + 1];   // 取得工作表
                worksheet.Cells[4, 3] = submitDate;
                worksheet.Cells[4, 5] = row["InspDate"];
                worksheet.Cells[6, 9] = row["article"];

                worksheet.Cells[4, 7] = txtSP.Text;
                worksheet.Cells[4, 10] = txtBrand.Text;
                worksheet.Cells[6, 3] = txtStyle.Text;
                worksheet.Cells[6, 6] = MyUtility.GetValue.Lookup($"select CustPONo from orders where id = '{txtSP.Text}'");
                worksheet.Cells[7, 3] = MyUtility.GetValue.Lookup($@"select StyleName from Style s, orders o where o.id = '{txtSP.Text}' and o.brandid = s.brandid and o.StyleID = s.id");
                worksheet.Cells[7, 6] = txtArriveQty.Text;
                worksheet.Cells[11, 2] = txtBrandRefno.Text;
                worksheet.Cells[11, 3] = txtColor.Text;
                worksheet.Cells[11, 4] = row["DryScale"];
                worksheet.Cells[11, 5] = row["ResultDry"];
                worksheet.Cells[11, 6] = row["WetScale"];
                worksheet.Cells[11, 7] = row["ResultWet"];
                worksheet.Cells[11, 8] = row["Remark"];

                worksheet.Cells[22, 8] = MyUtility.GetValue.Lookup("Name", row["Inspector"].ToString(), "Pass1", "ID");

                Marshal.ReleaseComObject(worksheet);
                j++;
            }

            #region Save & Show Excel
            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;
            strFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test_for_PDF");
            strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test_for_PDF", Sci.Production.Class.PDFFileNameExtension.PDF);
            objApp.ActiveWorkbook.SaveAs(strFileName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            #endregion

            if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                Process.Start(startInfo);
            }
        }
    }
}
