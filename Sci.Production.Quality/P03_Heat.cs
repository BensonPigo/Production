﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Linq;

namespace Sci.Production.Quality
{
    public partial class P03_Heat : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;
        private string ID;

        public P03_Heat(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            maindr = mainDr;
            ID = id.Trim();
            
        }
        //編輯事件觸發     

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            button_enable();
        }
   
        //設定表頭資料
        protected override DualResult OnRequery()
        {
            mainDBQuery();//重新query maindr
            #region Encode Enable
            button_enable();
            encode_button.Text = MyUtility.Convert.GetBool(maindr["HeatEncode"]) ? "Amend" : "Encode";

            #endregion
            //表頭 資料設定           
            this.save.Enabled = !MyUtility.Convert.GetBool(maindr["HeatEncode"]);
           
            string fir_cmd = string.Format(
                @"select distinct a.Poid,a.SEQ1+a.SEQ2 as seq,a.ArriveQty,
				b.styleid,b.BrandID,c.ExportId,c.WhseArrival,f.SuppID,a.SCIRefno,a.Refno,d.ColorID,
				e.HeatDate,e.Heat,e.nonHeat												
				 from FIR a WITH (NOLOCK) 
				left join Orders b WITH (NOLOCK) on a.POID=b.POID
				left join Receiving c WITH (NOLOCK) on a.ReceivingID=c.Id
				left join PO_Supp_Detail d WITH (NOLOCK) on d.ID=a.POID and a.SEQ1=d.SEQ1 and a.seq2=d.SEQ2
				left join FIR_Laboratory e WITH (NOLOCK) on a.ID=e.ID
                left join PO_Supp f WITH (NOLOCK) on d.ID=f.ID and d.SEQ1=f.SEQ1
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
                LIDate.Value = MyUtility.Convert.GetDate(fir_dr["HeatDate"]);
                ResultText.Text = fir_dr["Heat"].ToString();
                checkBox1.Value = fir_dr["nonHeat"].ToString();
            }
            else
            {
                sptext.Text = ""; SEQtext.Text = ""; AQtytext.Text = ""; Wknotext.Text = ""; Arrdate.Text = ""; Styletext.Text = ""; Brandtext.Text = "";
                Supptext.Text = ""; SRnotext.Text = ""; BRnotext.Text = ""; Colortext.Text = "";
            }


            return base.OnRequery();
        }
        //Grid View DataTable 新增欄位資料供撈取顯示
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("Poid", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));
            datas.Columns.Add("NewKey", typeof(int));
            datas.Columns.Add("Horizontal_Average", typeof(decimal));
            datas.Columns.Add("Vertical_Average", typeof(decimal));
            datas.Columns.Add("Last update", typeof(string));
            decimal avgHorizontal = 0; decimal avgVertical = 0;
            int i = 0;
                     
            foreach (DataRow dr in datas.Rows)
            {
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["NewKey"] = i;
                dr["poid"] = maindr["poid"];
                dr["SEQ1"] = maindr["SEQ1"];
                dr["SEQ2"] = maindr["SEQ2"];
                if (datas.Rows[i].IsNull("HorizontalTest1") || datas.Rows[i].IsNull("HorizontalTest2") || datas.Rows[i].IsNull("HorizontalTest3"))
                {
                    avgHorizontal = 0;
                }
                else
                {

                    avgHorizontal = ((decimal)datas.Rows[i]["HorizontalTest1"] + (decimal)datas.Rows[i]["HorizontalTest2"] + (decimal)datas.Rows[i]["HorizontalTest3"]) / 3;                    
                    dr["Horizontal_Average"] =  Math.Round(avgHorizontal, 2);
                }
                if (datas.Rows[i].IsNull("VerticalTest1") || datas.Rows[i].IsNull("VerticalTest2") || datas.Rows[i].IsNull("VerticalTest3"))
                {
                    avgVertical = 0;
                }
                else
                {
                    avgVertical = ((decimal)datas.Rows[i]["VerticalTest1"] + (decimal)datas.Rows[i]["VerticalTest2"] + (decimal)datas.Rows[i]["VerticalTest3"]) / 3;
                    dr["Vertical_Average"] =Math.Round(avgVertical,2);
                }   

                dr["Last update"] = datas.Rows[i]["EditName"].ToString() + " - " + datas.Rows[i]["EditDate"].ToString();
                i++;
            }

        }
        //GridView 設定
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings Rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings orlHorCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings orlVirCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings HorTest1Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings HorTest2Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings HorTest3Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings VirTest1Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings VirTest2Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings VirTest3Cell = new DataGridViewGeneratorNumericColumnSettings();
            

            
            DataGridViewGeneratorTextColumnSettings LabTechCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ResultCell = new DataGridViewGeneratorTextColumnSettings();

            #region 設定GridMouse Click 事件
            Rollcell.EditingMouseDown += (s, e) =>
            {

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
                    e.EditingControl.Text = item.GetSelectedString();//將選取selectitem value帶入GridView
                }
            };
            LabTechCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) where Resign is null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["DryScale"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    e.EditingControl.Text = item1.GetSelectedString(); //將選取selectitem value帶入GridView
                }
            };          
            ResultCell.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (dr["Result"].ToString() == "PASS") dr["Result"] = "FAIL";
                else dr["Result"] = "PASS";
            };
            #endregion
            #region 設定Grid Valid事件
            Rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (this.EditMode == false) return;
                if (oldvalue == newvalue) return;
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
                    MyUtility.Msg.WarningBox("<Roll> data not found!");
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
            };
            orlHorCell.CellValidating += (s, e) =>
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["HorizontalOriginal"]))
                    {
                        if (MyUtility.Convert.GetDecimal(e.FormattedValue)==0)
                        {
                            dr["HorizontalOriginal"] = dr["HorizontalOriginal"];
                            return;
                        }
                        if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                        {
                            MyUtility.Msg.InfoBox("<Original Horizontal > cannot over than 100 !");
                            dr["HorizontalOriginal"] = MyUtility.Convert.GetDecimal(dr["HorizontalOriginal"]);
                        }
                        else
                        {
                            dr["HorizontalOriginal"] = e.FormattedValue;
                        }
                    }
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue)!=0)
                    {
                        decimal newValue = (((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3) / (decimal)dr["HorizontalOriginal"];
                        dr["HorizontalRate"] = Math.Round(newValue, 2);
                    }
                    else
                    {
                        return;
                    }
                   
                };
            orlVirCell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);

                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["VerticalOriginal"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) == 0)
                    {
                        dr["VerticalOriginal"] = dr["VerticalOriginal"];
                        return;
                    }
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<VerticalOriginal > cannot over than 100 !");
                        dr["VerticalOriginal"] = MyUtility.Convert.GetDecimal(dr["VerticalOriginal"]);
                    }
                    else
                    {
                        dr["VerticalOriginal"] = e.FormattedValue;
                    }
                }
                if (MyUtility.Convert.GetDecimal(e.FormattedValue)!=0)
                {
                    decimal newValue = (((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3) / (decimal)dr["VerticalOriginal"];
                    dr["VerticalRate"] = Math.Round(newValue, 2);
                }
                else
                {
                    return;
                }
            
            };
            HorTest1Cell.CellValidating += (s, e) =>
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["HorizontalTest1"]))
                    {
                        if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                        {
                            MyUtility.Msg.InfoBox("<Horizontal 1> cannot over than 100 !");
                            dr["HorizontalTest1"] = MyUtility.Convert.GetDecimal(dr["HorizontalTest1"]);
                        }
                        else
                        {
                            dr["HorizontalTest1"] = e.FormattedValue;
                        }                       
                    }
                    if ((decimal)dr["HorizontalOriginal"]!=0)
                    {
                        decimal newValue = (((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3) / (decimal)dr["HorizontalOriginal"];
                        dr["HorizontalRate"] = Math.Round(newValue, 2);
                        decimal newAvgValue = (((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3);
                        dr["Horizontal_Average"] = Math.Round(newAvgValue, 2);
                        
                    }
                    else
                    {
                        return;
                    }

                };
            HorTest2Cell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["HorizontalTest2"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Horizontal 2> cannot over than 100 !");
                        dr["HorizontalTest2"] = MyUtility.Convert.GetDecimal(dr["HorizontalTest2"]);
                    }
                    else
                    {
                        dr["HorizontalTest2"] = e.FormattedValue;
                    }                   
                }
                if ((decimal)dr["HorizontalOriginal"] != 0)
                {
                    decimal newValue = (((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3) / (decimal)dr["HorizontalOriginal"];
                    dr["HorizontalRate"] = Math.Round(newValue, 2);
                    decimal newAvgValue = (((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3);
                    dr["Horizontal_Average"] = Math.Round(newAvgValue, 2);
                    
                }
                else
                {
                    return;
                }
            };
            HorTest3Cell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["HorizontalTest3"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Horizontal 3> cannot over than 100 !");
                        dr["HorizontalTest3"] = MyUtility.Convert.GetDecimal(dr["HorizontalTest3"]);
                    }
                    else
                    {
                        dr["HorizontalTest3"] = e.FormattedValue;
                    }
                }
                if ((decimal)dr["HorizontalOriginal"] != 0)
                {
                    decimal newValue = (((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3) / (decimal)dr["HorizontalOriginal"];
                    dr["HorizontalRate"] = Math.Round(newValue, 2);
                    decimal newAvgValue = (((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3);
                    dr["Horizontal_Average"] = Math.Round(newAvgValue, 2);
                    
                }
                else
                {
                    return;
                }
            };
            
          
            VirTest1Cell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["VerticalTest1"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Vertical 1> cannot over than 100 !");
                        dr["VerticalTest1"] = MyUtility.Convert.GetDecimal(dr["VerticalTest1"]);

                    }
                    else
                    {
                        dr["VerticalTest1"] = e.FormattedValue;
                    }
                }
                if ((decimal)dr["VerticalOriginal"] != 0)
                {
                    decimal newValue = (((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3) / (decimal)dr["VerticalOriginal"];
                    dr["VerticalRate"] = Math.Round(newValue, 2);
                    decimal newAvgValue = (((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3);
                    dr["Vertical_Average"] = Math.Round(newAvgValue, 2);
                    
                }
                else
                {
                    return;
                }
            };
            VirTest2Cell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["VerticalTest2"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Vertical 2> cannot over than 100 !");
                        dr["VerticalTest2"] = MyUtility.Convert.GetDecimal(dr["VerticalTest2"]);

                    }
                    else
                    {
                        dr["VerticalTest2"] = e.FormattedValue;
                    }
                }
                if ((decimal)dr["VerticalOriginal"] != 0)
                {
                    decimal newValue = (((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3) / (decimal)dr["VerticalOriginal"];
                    dr["VerticalRate"] = Math.Round(newValue, 2);
                    decimal newAvgValue = (((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3);
                    dr["Vertical_Average"] = Math.Round(newAvgValue, 2);
                    
                }
                else
                {
                    return;
                }
            };           
            VirTest3Cell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["VerticalTest3"]))
                {       
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Vertical 3> cannot over than 100 !");
                        dr["VerticalTest3"] = MyUtility.Convert.GetDecimal(dr["VerticalTest3"]);

                    }
                    else
                    {
                        dr["VerticalTest3"] = e.FormattedValue;
                    }
                }
                if ((decimal)dr["VerticalOriginal"] != 0)
                {
                    decimal newValue = (((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3) / (decimal)dr["VerticalOriginal"];
                    dr["VerticalRate"] = Math.Round(newValue, 2);
                    decimal newAvgValue = (((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3) ;
                    dr["Vertical_Average"] = Math.Round(newAvgValue, 2);
                    
                }
                else
                {
                    return;
                }
            };
            LabTechCell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["inspector"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (this.EditMode == false) return;
                if (oldvalue == newvalue) return;
                string dryScale_cmd = string.Format(@"select Inspector from FIR_Laboratory_Crocking a WITH (NOLOCK) left join Pass1 b WITH (NOLOCK) on a.Inspector=b.ID and b.Resign is null where a.id ='{0}'", maindr["id"]);
                DataRow roll_dr;
                if (MyUtility.Check.Seek(dryScale_cmd, out roll_dr))
                {
                    dr["Inspector"] = roll_dr["Inspector"];
                    dr.EndEdit();
                }
                else
                {
                    MyUtility.Msg.WarningBox("<Inspector> data not found!");
                    dr["Inspector"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.grid)
              .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), settings: Rollcell)
              .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true)
              .Numeric("HorizontalOriginal", header: "Original Horizontal", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings: orlHorCell)//s
              .Numeric("VerticalOriginal", header: "Original Vertical", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings:orlVirCell)
              .Text("Result", header: "Result", width: Widths.AnsiChars(5), settings: ResultCell,iseditingreadonly:true)
              .Numeric("HorizontalTest1", header: "Horizontal 1", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings: HorTest1Cell)
              .Numeric("HorizontalTest2", header: "Horizontal 2", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings: HorTest2Cell)
              .Numeric("HorizontalTest3", header: "Horizontal 3", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings: HorTest3Cell)
              .Numeric("Horizontal_Average", header: "Horizontal_Average", width: Widths.AnsiChars(8), integer_places:10, decimal_places: 2, iseditingreadonly: true)                
              .Numeric("HorizontalRate", header: "Horizontal Shrinkage rate", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
              .Numeric("VerticalTest1", header: "Vertical 1", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings: VirTest1Cell)
              .Numeric("VerticalTest2", header: "Vertical 2", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings: VirTest2Cell)
              .Numeric("VerticalTest3", header: "Vertical 3", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, settings: VirTest3Cell)
              .Numeric("Vertical_Average", header: "Vertical Average", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
              .Numeric("VerticalRate", header: "Vertical Shrinkage Rate", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
              .Date("InspDate", header: "Test Date", width: Widths.AnsiChars(10))
              .Text("Inspector", header: "Lab Tech", width: Widths.AnsiChars(16), settings: LabTechCell)
              .CellUser("Inspector", header: "Name", width: Widths.AnsiChars(10),iseditingreadonly:true)
              .Text("Remark", header: "Remark", width: Widths.AnsiChars(16))
              .Text("Last update", header: "Last update", width: Widths.AnsiChars(50), iseditingreadonly: true);
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
        //判斷Grid View有無空白
        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)gridbs.DataSource;
            DataTable afterDT = new DataTable();
            //將刪除資料過的grid 重新丟進新datatable 並將資料以完全刪除來做判斷! 
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
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalOriginal"])))
            {
                 MyUtility.Msg.WarningBox("<Original Vertical> can not be empty.");
                return false;
            } 
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalOriginal"])))
            {
                MyUtility.Msg.WarningBox("<Original Horizontal> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalTest1"])))
            {
                MyUtility.Msg.WarningBox("<Horizontal 1 > can not be empty.");
                return false;
            }    
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalTest2"])))
            {
                MyUtility.Msg.WarningBox("<Horizontal 2> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalTest3"])))
            {
                MyUtility.Msg.WarningBox("<Horizontal 3> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalTest1"])))
            {
                MyUtility.Msg.WarningBox("<Vertical 1> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalTest2"])))
            {
                MyUtility.Msg.WarningBox("<Vertical 2> can not be empty.");
                return false;
            }
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalTest3"])))
            {
                MyUtility.Msg.WarningBox("<Vertical 3> can not be empty.");
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
            
            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From FIR_Laboratory_Heat Where id =@id and roll=@roll";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@roll", dr["roll", DataRowVersion.Original])); 
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    if (!upResult) { return upResult; }
                    continue;
                }
                string inspdate;
                if (MyUtility.Check.Empty(dr["InspDate"])) inspdate = ""; //判斷Inspect Date
                else inspdate = Convert.ToDateTime(dr["InspDate"]).ToShortDateString();
                DateTime Today = DateTime.Now;
                if (dr.RowState == DataRowState.Added)
                {
                    List<SqlParameter> spamAdd = new List<SqlParameter>();
                     update_cmd =
                        @"insert into FIR_Laboratory_Heat
                       (ID,roll,Dyelot,Inspdate,Inspector,Result,Remark,AddDate,AddName,HorizontalRate,HorizontalOriginal,
                        HorizontalTest1,HorizontalTest2,HorizontalTest3,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2,VerticalTest3)
                       values
                        (@ID,@roll,@Dyelot,@Inspdate,@Inspector,@Result,@Remark,@AddDate,@AddName,@HorizontalRate,@HorizontalOriginal,
                        @HorizontalTest1,@HorizontalTest2,@HorizontalTest3,@VerticalRate,@VerticalOriginal,@VerticalTest1,@VerticalTest2,@VerticalTest3)";
                    spamAdd.Add(new SqlParameter("@id", maindr["ID"]));
                    spamAdd.Add(new SqlParameter("@roll", dr["Roll"]));
                    spamAdd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamAdd.Add(new SqlParameter("@Inspdate", inspdate.Replace("/", "-")));
                    spamAdd.Add(new SqlParameter("@Inspector", dr["Inspector"]));
                    spamAdd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamAdd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamAdd.Add(new SqlParameter("@AddDate", Today));
                    spamAdd.Add(new SqlParameter("@AddName", loginID));
                    spamAdd.Add(new SqlParameter("@HorizontalRate", dr["HorizontalRate"]));
                    spamAdd.Add(new SqlParameter("@HorizontalOriginal", dr["HorizontalOriginal"]));
                    spamAdd.Add(new SqlParameter("@HorizontalTest1", dr["HorizontalTest1"]));
                    spamAdd.Add(new SqlParameter("@HorizontalTest2", dr["HorizontalTest2"]));
                    spamAdd.Add(new SqlParameter("@HorizontalTest3", dr["HorizontalTest3"]));
                    spamAdd.Add(new SqlParameter("@VerticalRate", dr["VerticalRate"]));
                    spamAdd.Add(new SqlParameter("@VerticalOriginal", dr["VerticalOriginal"]));
                    spamAdd.Add(new SqlParameter("@VerticalTest1", dr["VerticalTest1"]));
                    spamAdd.Add(new SqlParameter("@VerticalTest2", dr["VerticalTest2"]));
                    spamAdd.Add(new SqlParameter("@VerticalTest3", dr["VerticalTest3"]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamAdd);
                    if (!upResult) { return upResult; }
                }
                if (dr.RowState == DataRowState.Modified)
                {
                    List<SqlParameter> spamUpd = new List<SqlParameter>();                    
                    update_cmd =
                    @"update FIR_Laboratory_Heat 
                    set ID=@id,roll=@roll,Dyelot=@Dyelot,Inspdate=@Inspdate,Inspector=@Inspector,
                    Result=@Result,Remark=@Remark,EditDate=@EditDate,EditName=@EditName,
                    HorizontalRate=@HorizontalRate,HorizontalOriginal=@HorizontalOriginal,
                    HorizontalTest1=@HorizontalTest1,HorizontalTest2=@HorizontalTest2,HorizontalTest3=@HorizontalTest3,
                    VerticalRate=@VerticalRate,VerticalOriginal=@VerticalOriginal,VerticalTest1=@VerticalTest1,
                    VerticalTest2=@VerticalTest2,VerticalTest3=@VerticalTest3
                    where id=@sid and Dyelot=@DyelotOrl";
                    spamUpd.Add(new SqlParameter("@id", dr["ID"]));
                    spamUpd.Add(new SqlParameter("@roll", dr["Roll"]));
                    spamUpd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamUpd.Add(new SqlParameter("@Inspdate", inspdate.Replace("/","-")));
                    spamUpd.Add(new SqlParameter("@Inspector", dr["Inspector"]));
                    spamUpd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamUpd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamUpd.Add(new SqlParameter("@EditDate", Today));
                    spamUpd.Add(new SqlParameter("@EditName", loginID));
                    spamUpd.Add(new SqlParameter("@HorizontalRate", dr["HorizontalRate"]));
                    spamUpd.Add(new SqlParameter("@HorizontalOriginal", dr["HorizontalOriginal"]));
                    spamUpd.Add(new SqlParameter("@HorizontalTest1", dr["HorizontalTest1"]));
                    spamUpd.Add(new SqlParameter("@HorizontalTest2", dr["HorizontalTest2"]));
                    spamUpd.Add(new SqlParameter("@HorizontalTest3", dr["HorizontalTest3"]));
                    spamUpd.Add(new SqlParameter("@VerticalRate", dr["VerticalRate"]));
                    spamUpd.Add(new SqlParameter("@VerticalOriginal", dr["VerticalOriginal"]));
                    spamUpd.Add(new SqlParameter("@VerticalTest1", dr["VerticalTest1"]));
                    spamUpd.Add(new SqlParameter("@VerticalTest2", dr["VerticalTest2"]));
                    spamUpd.Add(new SqlParameter("@VerticalTest3", dr["VerticalTest3"]));
                    spamUpd.Add(new SqlParameter("@sid", maindr["ID"]));
                    spamUpd.Add(new SqlParameter("@DyelotOrl", dr["Dyelot", DataRowVersion.Original]));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamUpd);
                    if (!upResult) { return upResult; }
                }
            }
            return upResult;
        }
        private void encode_button_Click(object sender, EventArgs e)
        {
            string updatesql = "";
            if (!MyUtility.Convert.GetBool(maindr["HeatEncode"]))
            {
                if (!MyUtility.Convert.GetBool(maindr["nonHeat"]))//判斷有勾選可Encode
                {
                    //至少檢驗一卷 並且出現在Fir_Continuity.Roll
                    DataTable rolldt;
                    string cmd = string.Format(
                        @"Select roll from Receiving_Detail a WITH (NOLOCK) where 
                        a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
                        and exists 
                        (Select distinct dyelot from FIR_Laboratory_Heat b WITH (NOLOCK) where b.id='{1}' and a.roll = b.roll)"
                        , maindr["receivingid"], maindr["id"], maindr["POID"], maindr["seq1"], maindr["seq2"]);
                    DualResult dResult;
                    if (dResult = DBProxy.Current.Select(null, cmd, out rolldt))
                    {
                        if (rolldt.Rows.Count < 1)
                        {
                            MyUtility.Msg.WarningBox("Each Roll must be in Physical Contiunity");
                        }
                    }
                }
                #region 判斷Heat Result
                DataTable gridDt = (DataTable)gridbs.DataSource;
                DataRow[] ResultAry = gridDt.Select("Result='Fail'");
                string result = "Pass";
                if (ResultAry.Length > 0) result = "Fail";
                #endregion
               string Today = DateTime.Now.ToShortDateString();
                #region 判斷表身最晚時間
                DataTable dt = (DataTable)gridbs.DataSource;
                if (MyUtility.Check.Empty(dt)) return;
                if (dt.Rows.Count == 0) return;
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
                #region 寫入實體Table
                updatesql = string.Format(
                @"Update Fir_Laboratory set HeatDate = '{0}',HeatEncode = 1,Heat='{1}' where id ='{2}'", lastDate.ToShortDateString(), result, maindr["ID"]);

                updatesql = updatesql + string.Format(@"update FIR_Laboratory_Heat set editName='{0}',inspdate='{2}' where id='{1}'", loginID, maindr["ID"], Today);
                #endregion
                
                }
            
            else//Amend
            {                       
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir_Laboratory set HeatDate = null,HeatEncode= 0 where id ='{0}'", maindr["ID"]);

               // updatesql = updatesql + string.Format(@"update FIR_Laboratory_Heat set editName='{0}',editDate=Getdate() where id='{1}'", loginID, maindr["ID"]);
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
        //編輯權限設定
        private void button_enable()
        {
            //return;
            if (maindr == null) return;
            encode_button.Enabled = this.CanEdit && !this.EditMode;
            this.ToExcel.Enabled =  !this.EditMode;
            this.Supptext.TextBox1.ReadOnly = true;
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P03", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 WITH (NOLOCK) Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            int lApprove = 0; //有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }

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
            if (!MyUtility.Check.Seek(cmd,spam,out maindr))
            {
                MyUtility.Msg.InfoBox("Data is empty");            
            }
          
        }

        private void ToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridbs.DataSource;
            string[] columnNames = new string[] 
            { "Roll", "Dyelot", "HorizontalOriginal", "VerticalOriginal", "Result", "HorizontalTest1", "HorizontalTest2", "HorizontalTest3", "Vertical_Average","Horizontalrate",
                "VerticalTest1","VerticalTest2","VerticalTest3","Vertical_Average","VerticalRate","InspDate", "Inspector", "Inspector", "Remark", "Last update" };
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, grid.Columns.Count) as object[,];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                for (int j = 0; j < columnNames.Length; j++)
                {
                    ret[i, j] = row[columnNames[j]];
                }
            }
            if (dt.Rows.Count==0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }
            // 撈seasonID 
            DataTable dtSeason;
            string SeasonID;
            DBProxy.Current.Select("Production", string.Format(
            "select C.SeasonID from FIR_Laboratory_Heat a WITH (NOLOCK) left join FIR_Laboratory b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C WITH (NOLOCK) ON B.POID=C.ID where a.ID='{0}'", maindr["ID"]), out dtSeason);
            if (dtSeason.Rows.Count == 0) { SeasonID = ""; }
            else { SeasonID = dtSeason.Rows[0]["SeasonID"].ToString(); }
          
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();

            MyUtility.Excel.CopyToXls(ret, xltFileName: "Quality_P03_Heat_Test.xltx", fileName: "Quality_P03_Heat_Test", headerline: 5, openfile: true, excelAppObj: excel);
            Microsoft.Office.Interop.Excel.Worksheet excelSheets = excel.ActiveWorkbook.Worksheets[1];// 取得工作表            
            excelSheets.Cells[2, 2] = sptext.Text.ToString();
            excelSheets.Cells[2, 4] = SEQtext.Text.ToString();
            excelSheets.Cells[2, 6] = Colortext.Text.ToString();
            excelSheets.Cells[2, 8] = Styletext.Text.ToString();
            excelSheets.Cells[2, 10] = SeasonID;
            excelSheets.Cells[3, 2] = SRnotext.Text.ToString();
            excelSheets.Cells[3, 4] = Wknotext.Text.ToString();
            excelSheets.Cells[3, 6] = ResultText.Text.ToString();
            excelSheets.Cells[3, 8] = LIDate.Value;
            excelSheets.Cells[3, 10] = Brandtext.Text.ToString();
            excelSheets.Cells[4, 2] = BRnotext.Text.ToString();
            excelSheets.Cells[4, 4] = AQtytext.Text.ToString();
            excelSheets.Cells[4, 6] = Arrdate.Value;
            excelSheets.Cells[4, 8] = Supptext.DisplayBox1.Text.ToString();
            excelSheets.Cells[4, 10] = checkBox1.Value.ToString();

            excel.Cells.EntireColumn.AutoFit();    //自動欄寬
            excel.Cells.EntireRow.AutoFit();       ////自動欄高

            if (excelSheets != null) Marshal.FinalReleaseComObject(excelSheets);//釋放sheet
            if (excel != null) Marshal.FinalReleaseComObject(excel);          //釋放objApp
                

        }
   
    }
}
