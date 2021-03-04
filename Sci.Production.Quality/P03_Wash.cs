using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Windows.Forms;
using System.Linq;
using System.Collections;       // file使用Hashtable時，必須引入這個命名空間

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P03_Wash : Win.Subs.Input4
    {
        private readonly string loginID = Env.User.UserID;
        private DataRow maindr;
        private string ID;
        private Hashtable ht = new Hashtable();

        /// <inheritdoc/>
        public P03_Wash(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.ID = id.Trim();

            // 抓取當下.exe執行位置路徑
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Resources\");
            this.ht.Add("Picture1", path + "QA_Skewness1.png");
            this.ht.Add("Picture2", path + "QA_Skewness2.png");
            this.ht.Add("Picture3", path + "QA_Skewness3.png");
        }

        // 編輯事件觸發

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.Button_enable();
        }

        // 設定表頭資料

        /// <inheritdoc/>
        protected override DualResult OnRequery()
        {
            this.MainDBQuery(); // 重新query maindr
            this.Button_enable();

            // 表頭 資料設定
            this.save.Enabled = !MyUtility.Convert.GetBool(this.maindr["WashEncode"]);

            string fir_cmd = string.Format(
@"select distinct 
	a.Poid,a.SCIRefno,a.Refno,
	seq = CONCAT(a.SEQ1,a.SEQ2),a.ArriveQty,
	b.styleid,b.BrandID,
	c.ExportId,c.WhseArrival,
	d.ColorID,	
	e.WashDate,e.Wash,e.nonWash,e.SkewnessOptionID
    ,[WashInspector] = (select name from pass1 where id= e.WashInspector),
	f.SuppID,
	g.DescDetail										
from FIR a WITH (NOLOCK) 
left join Orders b WITH (NOLOCK) on a.POID=b.POID
left join Receiving c WITH (NOLOCK) on a.ReceivingID=c.Id
left join PO_Supp_Detail d WITH (NOLOCK) on d.ID=a.POID and a.SEQ1=d.SEQ1 and a.seq2=d.SEQ2
left join FIR_Laboratory e WITH (NOLOCK) on a.ID=e.ID
left join PO_Supp f WITH (NOLOCK) on d.ID=f.ID and d.SEQ1=f.SEQ1
left join Fabric g WITH (NOLOCK) on g.SCIRefno = a.SCIRefno
where a.ID='{0}'",
this.ID);
            DataRow fir_dr;
            if (MyUtility.Check.Seek(fir_cmd, out fir_dr))
            {
                this.txtSP.Text = fir_dr["Poid"].ToString();
                this.txtSEQ.Text = fir_dr["SEQ"].ToString();
                this.txtArriveQty.Text = fir_dr["ArriveQty"].ToString();
                this.txtWkno.Text = fir_dr["exportid"].ToString();
                this.dateArriveWHDate.Value = MyUtility.Convert.GetDate(fir_dr["WhseArrival"]);
                this.txtStyle.Text = fir_dr["styleid"].ToString();
                this.txtBrand.Text = fir_dr["Brandid"].ToString();
                this.txtsupplierSupp.TextBox1.Text = fir_dr["SuppID"].ToString();
                this.txtSCIRefno.Text = fir_dr["Scirefno"].ToString();
                this.txtBrandRefno.Text = fir_dr["Refno"].ToString();
                this.txtColor.Text = fir_dr["colorid"].ToString();
                this.dateLastInspectionDate.Value = MyUtility.Convert.GetDate(fir_dr["WashDate"]);
                this.txtResult.Text = fir_dr["Wash"].ToString();
                this.checkNA.Value = fir_dr["nonWash"].ToString();
                this.editDescription.Text = fir_dr["DescDetail"].ToString();
                this.radioPanel1.Value = fir_dr["SkewnessOptionID"].ToString();
                this.txtWashInspector.Text = fir_dr["WashInspector"].ToString();
                if (fir_dr["SkewnessOptionID"].ToString() == "1")
                {
                    this.radioOption1.Checked = true;
                    this.radioOption2.Checked = false;
                    this.radioOption3.Checked = false;
                }
                else if (fir_dr["SkewnessOptionID"].ToString() == "2")
                {
                    this.radioOption1.Checked = false;
                    this.radioOption2.Checked = true;
                    this.radioOption3.Checked = false;
                }
                else if (fir_dr["SkewnessOptionID"].ToString() == "3")
                {
                    this.radioOption1.Checked = false;
                    this.radioOption2.Checked = false;
                    this.radioOption3.Checked = true;
                }

                switch (this.radioPanel1.Value.ToString())
                {
                    case "1":
                        this.pictureBox1.ImageLocation = this.ht["Picture1"].ToString();
                        break;
                    case "2":
                        this.pictureBox1.ImageLocation = this.ht["Picture2"].ToString();
                        break;
                    case "3":
                        this.pictureBox1.ImageLocation = this.ht["Picture3"].ToString();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                this.txtSP.Text = string.Empty;
                this.txtSEQ.Text = string.Empty;
                this.txtArriveQty.Text = string.Empty;
                this.txtWkno.Text = string.Empty;
                this.dateArriveWHDate.Text = string.Empty;
                this.txtStyle.Text = string.Empty;
                this.txtBrand.Text = string.Empty;
                this.txtsupplierSupp.Text = string.Empty;
                this.txtSCIRefno.Text = string.Empty;
                this.txtBrandRefno.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.editDescription.Text = string.Empty;
                this.txtWashInspector.Text = string.Empty;
            }

            this.GridView_Visable();

            return base.OnRequery();
        }

        // Grid View DataTable 新增欄位資料供撈取顯示

        /// <inheritdoc/>
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
            decimal avgHorizontal = 0;
            decimal avgVertical = 0;
            int i = 0;

            foreach (DataRow dr in datas.Rows)
            {
                dr["Name"] = MyUtility.GetValue.Lookup("Name_Extno", dr["Inspector"].ToString(), "View_ShowName", "ID");
                dr["NewKey"] = i;
                dr["poid"] = this.maindr["poid"];
                dr["SEQ1"] = this.maindr["SEQ1"];
                dr["SEQ2"] = this.maindr["SEQ2"];
                if (datas.Rows[i].IsNull("HorizontalTest1") || datas.Rows[i].IsNull("HorizontalTest2") || datas.Rows[i].IsNull("HorizontalTest3"))
                {
                    avgHorizontal = 0;
                }
                else
                {
                    avgHorizontal = ((decimal)datas.Rows[i]["HorizontalTest1"] + (decimal)datas.Rows[i]["HorizontalTest2"] + (decimal)datas.Rows[i]["HorizontalTest3"]) / 3;
                    dr["Horizontal_Average"] = Math.Round(avgHorizontal, 2);
                }

                if (datas.Rows[i].IsNull("VerticalTest1") || datas.Rows[i].IsNull("VerticalTest2") || datas.Rows[i].IsNull("VerticalTest3"))
                {
                    avgVertical = 0;
                }
                else
                {
                    avgVertical = ((decimal)datas.Rows[i]["VerticalTest1"] + (decimal)datas.Rows[i]["VerticalTest2"] + (decimal)datas.Rows[i]["VerticalTest3"]) / 3;
                    dr["Vertical_Average"] = Math.Round(avgVertical, 2);
                }

                string name = MyUtility.Check.Empty(datas.Rows[i]["EditName"].ToString()) ? MyUtility.GetValue.Lookup("Name_Extno", datas.Rows[i]["AddName"].ToString(), "View_ShowName", "ID") :
                   MyUtility.GetValue.Lookup("Name_Extno", datas.Rows[i]["EditName"].ToString(), "View_ShowName", "ID");
                string date = MyUtility.Check.Empty(datas.Rows[i]["EditDate"].ToString()) ? datas.Rows[i]["AddDate"].ToString() : datas.Rows[i]["EditDate"].ToString();
                dr["Last update"] = name + " - " + date;
                i++;
            }
        }

        // GridView 設定

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings orlHorCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings orlVirCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings horTest1Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings horTest2Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings horTest3Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings virTest1Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings virTest2Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings virTest3Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings skeTest1Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings skeTest2Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings skeTest3Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings skeTest4Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings dyelotCell = new DataGridViewGeneratorTextColumnSettings();

            DataGridViewGeneratorTextColumnSettings labTechCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultCell = PublicPrg.Prgs.CellResult.GetGridCell();

            #region 設定GridMouse Click 事件
            rollcell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string selectedDyelot = dr["Dyelot"].ToString();

                    string sqlcmd = string.Empty;

                    if (MyUtility.Check.Empty(selectedDyelot))
                    {
                        sqlcmd = $@" Select roll,dyelot 
                                        from dbo.View_AllReceivingDetail WITH (NOLOCK) 
                                        Where id='{this.maindr["Receivingid"]}'
                                                and poid ='{this.maindr["Poid"]}' 
                                                and seq1 = '{this.maindr["seq1"]}' 
                                                and seq2 ='{this.maindr["seq2"]}'";
                    }
                    else
                    {
                        sqlcmd = $@" Select roll,dyelot 
                                        from dbo.View_AllReceivingDetail WITH (NOLOCK) 
                                        Where id='{this.maindr["Receivingid"]}'
                                                and poid ='{this.maindr["Poid"]}' 
                                                and seq1 = '{this.maindr["seq1"]}' 
                                                and seq2 ='{this.maindr["seq2"]}'
                                                AND Dyelot='{selectedDyelot}'";
                    }

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

            dyelotCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string selectedRoll = dr["Roll"].ToString();

                    string sqlcmd = string.Empty;
                    if (MyUtility.Check.Empty(selectedRoll))
                    {
                        sqlcmd = $@" Select roll,dyelot 
                                        from dbo.View_AllReceivingDetail WITH (NOLOCK) 
                                        Where id='{this.maindr["Receivingid"]}'
                                                and poid ='{this.maindr["Poid"]}' 
                                                and seq1 = '{this.maindr["seq1"]}' 
                                                and seq2 ='{this.maindr["seq2"]}'";
                    }
                    else
                    {
                        sqlcmd = $@" Select roll,dyelot 
                                        from dbo.View_AllReceivingDetail WITH (NOLOCK) 
                                        Where id='{this.maindr["Receivingid"]}'
                                                and poid ='{this.maindr["Poid"]}' 
                                                and seq1 = '{this.maindr["seq1"]}' 
                                                and seq2 ='{this.maindr["seq2"]}'
                                                AND Roll='{selectedRoll}'";
                    }

                    SelectItem item = new SelectItem(sqlcmd, "15,12", dr["dyelot"].ToString(), false, ",");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Roll"] = item.GetSelecteds()[0]["Roll"].ToString();
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString();
                }
            };

            labTechCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);

                    // string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) where Resign is null";
                    string sqlCmd = $@"select DISTINCT Inspector,b.name from FIR_Laboratory_Wash a WITH (NOLOCK) 
                                         INNER join Pass1 b WITH (NOLOCK) on a.Inspector=b.ID
                                         ";
                    SelectItem item1 = new SelectItem(sqlCmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Inspector"] = item1.GetSelectedString(); // 將選取selectitem value帶入GridView
                    dr["Name"] = item1.GetSelecteds()[0]["Name"];
                }
            };

            #endregion
            #region 設定Grid Valid事件
            rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (oldvalue.Equals(newvalue))
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    // 沒填入資料,清空dyelot
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    return;
                }

                if (dr.RowState != DataRowState.Added)
                {
                    if (oldvalue == newvalue)
                    {
                        return;
                    }
                }

                string roll_cmd = string.Format("Select roll,Poid,seq1,seq2,dyelot from dbo.View_AllReceivingDetail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);
                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr.EndEdit();
                }
                else
                {
                    dr["Roll"] = string.Empty;

                    // dr["Dyelot"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> data not found!", e.FormattedValue));
                    return;
                }
            };

            dyelotCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Dyelot"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (oldvalue.Equals(newvalue))
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    // 沒填入資料,清空dyelot
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    return;
                }

                if (dr.RowState != DataRowState.Added)
                {
                    if (oldvalue == newvalue)
                    {
                        return;
                    }
                }

                string roll_cmd = string.Format("Select roll,Poid,seq1,seq2,dyelot from dbo.View_AllReceivingDetail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and Dyelot='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);
                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr.EndEdit();
                }
                else
                {
                    // dr["Roll"] = "";
                    dr["Dyelot"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Dyelot: {0}> data not found!", e.FormattedValue));
                    return;
                }
            };

            orlHorCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["HorizontalOriginal"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) == 0)
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

                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != 0)
                {
                    this.CalHorRate(dr);
                }
                else
                {
                    dr["HorizontalRate"] = 0;
                    return;
                }
            };
            orlVirCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);

                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["VerticalOriginal"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) == 0)
                    {
                        dr["VerticalOriginal"] = MyUtility.Convert.GetDecimal(dr["VerticalOriginal"]);
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

                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != 0)
                {
                    this.CalVerRate(dr);
                }
                else
                {
                    dr["VerticalRate"] = 0;
                    return;
                }
            };
            horTest1Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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

                this.CalHorRate(dr);
            };
            horTest2Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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

                this.CalHorRate(dr);
            };
            horTest3Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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

                this.CalHorRate(dr);
            };

            virTest1Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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

                this.CalVerRate(dr);
            };
            virTest2Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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

                this.CalVerRate(dr);
            };
            virTest3Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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

                this.CalVerRate(dr);
            };

            skeTest1Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["SkewnessTest1"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        dr["SkewnessTest1"] = MyUtility.Convert.GetDecimal(dr["SkewnessTest1"]);
                        MyUtility.Msg.InfoBox("<Skewness 1> cannot over than 100 !");
                        return;
                    }
                    else
                    {
                        dr["SkewnessTest1"] = e.FormattedValue;
                    }
                }

                this.CalSkeValue(dr);
            };

            skeTest2Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["SkewnessTest2"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Skewness 2> cannot over than 100 !");
                        dr["SkewnessTest2"] = MyUtility.Convert.GetDecimal(dr["SkewnessTest2"]);
                    }
                    else
                    {
                        dr["SkewnessTest2"] = e.FormattedValue;
                    }
                }

                this.CalSkeValue(dr);
            };
            skeTest3Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["SkewnessTest3"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Skewness 3> cannot over than 100 !");
                        dr["SkewnessTest3"] = MyUtility.Convert.GetDecimal(dr["SkewnessTest3"]);
                    }
                    else
                    {
                        dr["SkewnessTest3"] = e.FormattedValue;
                    }
                }

                this.CalSkeValue(dr);
            };
            skeTest4Cell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["SkewnessTest4"]))
                {
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) >= 100)
                    {
                        MyUtility.Msg.InfoBox("<Skewness 4> cannot over than 100 !");
                        dr["SkewnessTest4"] = MyUtility.Convert.GetDecimal(dr["SkewnessTest4"]);
                    }
                    else
                    {
                        dr["SkewnessTest4"] = e.FormattedValue;
                    }
                }

                this.CalSkeValue(dr);
            };

            labTechCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return; // 沒資料 return
                }

                string sqlCmd = $"SELECT ID,Name FROM Pass1 WHERE ID='{e.FormattedValue}'";
                DataRow userDt;

                if (!MyUtility.Check.Seek(sqlCmd, out userDt))
                {
                    MyUtility.Msg.WarningBox(string.Format("<Lab Tech: {0}> data not found!", e.FormattedValue));

                    dr["inspector"] = string.Empty;
                    dr["Name"] = string.Empty;
                    return;
                }
                else
                {
                    dr["inspector"] = userDt["ID"].ToString();
                    dr["Name"] = userDt["Name"].ToString();
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
              .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), settings: rollcell)
              .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), settings: dyelotCell)
              .Numeric("HorizontalOriginal", header: "Original Horizontal", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: orlHorCell)
              .Numeric("VerticalOriginal", header: "Original Vertical", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: orlVirCell)
              .Text("Result", header: "Result", width: Widths.AnsiChars(5), settings: resultCell, iseditingreadonly: true)
              .Numeric("HorizontalTest1", header: "Horizontal 1", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: horTest1Cell)
              .Numeric("HorizontalTest2", header: "Horizontal 2", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: horTest2Cell)
              .Numeric("HorizontalTest3", header: "Horizontal 3", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, maximum: 99, settings: horTest3Cell)
              .Numeric("Horizontal_Average", header: "Horizontal_Average", width: Widths.AnsiChars(6), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
              .Numeric("Horizontalrate", header: "Horizontal Shrinkage rate", width: Widths.AnsiChars(6), integer_places: 4, decimal_places: 2, iseditingreadonly: true)
              .Numeric("VerticalTest1", header: "Vertical 1", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: virTest1Cell)
              .Numeric("VerticalTest2", header: "Vertical 2", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: virTest2Cell)
              .Numeric("VerticalTest3", header: "Vertical 3", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: virTest3Cell)
              .Numeric("Vertical_Average", header: "Vertical Average", width: Widths.AnsiChars(6), integer_places: 2, decimal_places: 2, iseditingreadonly: true)
              .Numeric("VerticalRate", header: "Vertical Shrinkage Rate", width: Widths.AnsiChars(6), integer_places: 4, decimal_places: 2, iseditingreadonly: true)

                // 注意,SkewnessTest Header Name 會隨Option變動的,Function =GridView_Visable()
              .Numeric("SkewnessTest1", header: "SkewnessTest1", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: skeTest1Cell)
              .Numeric("SkewnessTest2", header: "SkewnessTest2", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: skeTest2Cell)
              .Numeric("SkewnessTest3", header: "SkewnessTest3", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: skeTest3Cell)
              .Numeric("SkewnessTest4", header: "SkewnessTest4", width: Widths.AnsiChars(4), integer_places: 2, decimal_places: 2, settings: skeTest4Cell)

              // ********end**************
              .Numeric("SkewnessRate", header: "Skewness Rate", width: Widths.AnsiChars(6), integer_places: 4, decimal_places: 2, iseditingreadonly: true)
              .Date("InspDate", header: "Test Date", width: Widths.AnsiChars(10))
              .Text("Inspector", header: "Lab Tech", width: Widths.AnsiChars(16), settings: labTechCell)
              .Text("Name", header: "Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
              .Text("Remark", header: "Remark", width: Widths.AnsiChars(16))
              .Text("Last update", header: "Last update", width: Widths.AnsiChars(50), iseditingreadonly: true);

            #region 可編輯欄位變色
            this.grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["HorizontalOriginal"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["VerticalOriginal"].DefaultCellStyle.BackColor = Color.Pink;

            this.grid.Columns["HorizontalTest1"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["HorizontalTest2"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["HorizontalTest3"].DefaultCellStyle.BackColor = Color.Pink;

            this.grid.Columns["VerticalTest1"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["VerticalTest2"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["VerticalTest3"].DefaultCellStyle.BackColor = Color.Pink;

            this.grid.Columns["SkewnessTest1"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["SkewnessTest2"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["SkewnessTest3"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["SkewnessTest4"].DefaultCellStyle.BackColor = Color.Pink;

            this.grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            this.GridView_Visable();
            return true;
        }

        private void GridView_Visable()
        {
            if (this.radioOption1.Checked)
            {
                this.grid.Columns[15].HeaderText = "AC";
                this.grid.Columns[16].HeaderText = "BD";
                this.grid.Columns[17].Visible = false;
                this.grid.Columns[18].Visible = false;
            }
            else if (this.radioOption2.Checked)
            {
                this.grid.Columns[15].HeaderText = "AA’";
                this.grid.Columns[16].HeaderText = "DD’";
                this.grid.Columns[17].HeaderText = "AB";
                this.grid.Columns[18].HeaderText = "CD";
                this.grid.Columns[17].Visible = true;
                this.grid.Columns[18].Visible = true;
            }
            else if (this.radioOption3.Checked)
            {
                this.grid.Columns[15].HeaderText = "AA’";
                this.grid.Columns[16].HeaderText = "AB";
                this.grid.Columns[17].Visible = false;
                this.grid.Columns[18].Visible = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnInsert()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;

            int maxi = MyUtility.Convert.GetInt(dt.Compute("Max(NewKey)", string.Empty));
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = this.loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", this.loginID, "Pass1", "ID");
            selectDr["NewKey"] = maxi + 1;
            selectDr["poid"] = this.maindr["poid"];
            selectDr["SEQ1"] = this.maindr["SEQ1"];
            selectDr["SEQ2"] = this.maindr["SEQ2"];
        }

        // 判斷Grid View有無空白

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)this.gridbs.DataSource;

            DataTable afterDT = new DataTable();

            // 將將刪除資料過的grid 重新丟進新datatable 並將資料完全刪除來做判斷!
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
                    DataRow[] drArray = afterDT.Select(string.Format("Roll='{0}' and Dyelot = '{1}'", MyUtility.Convert.GetString(dr["Roll"]), MyUtility.Convert.GetString(dr["Dyelot"])));
                    if (drArray.Length > 1)
                    {
                        MyUtility.Msg.WarningBox("<Roll>" + MyUtility.Convert.GetString(dr["Roll"]) + ", <Dyelot> " + MyUtility.Convert.GetString(dr["Dyelot"]) + " is already exist ! ");
                        return false;
                    }
                }
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalOriginal"])))
            {
                MyUtility.Msg.WarningBox("<Original Vertical> can not be 0");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalOriginal"])))
            {
                MyUtility.Msg.WarningBox("<Original Horizontal> can not be 0");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalOriginal"])))
            {
                MyUtility.Msg.WarningBox("<Horizontal 1 > can not be 0.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalTest2"])))
            {
                MyUtility.Msg.WarningBox("<Horizontal 2> can not be 0.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["HorizontalTest3"])))
            {
                MyUtility.Msg.WarningBox("<Horizontal 3> can not be 0.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalTest1"])))
            {
                MyUtility.Msg.WarningBox("<Vertical 1> can not be 0.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalTest2"])))
            {
                MyUtility.Msg.WarningBox("<Vertical 2> can not be 0.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["VerticalTest3"])))
            {
                MyUtility.Msg.WarningBox("<Vertical 3> can not be 0.");
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
                MyUtility.Msg.WarningBox("<Lab Tech> can not be empty.");
                return false;
            }
            #endregion

            return base.OnSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = string.Empty;

            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From FIR_Laboratory_Wash Where id =@id and roll=@roll";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@roll", dr["roll", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    if (!upResult)
                    {
                        return upResult;
                    }

                    continue;
                }

                string inspdate;
                if (MyUtility.Check.Empty(dr["InspDate"]))
                {
                    inspdate = string.Empty; // 判斷Inspect Date
                }
                else
                {
                    inspdate = string.Format("{0:yyyy-MM-dd}", dr["InspDate"]);
                }

                DateTime today = DateTime.Now;
                if (dr.RowState == DataRowState.Added)
                {
                    List<SqlParameter> spamAdd = new List<SqlParameter>();
                    update_cmd =
                       @"insert into FIR_Laboratory_Wash
                       (ID,roll,Dyelot,Inspdate,Inspector,Result,Remark,AddDate,AddName,HorizontalRate,HorizontalOriginal,
                        HorizontalTest1,HorizontalTest2,HorizontalTest3,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2,VerticalTest3,
                        SkewnessTest1,SkewnessTest2,SkewnessTest3,SkewnessTest4,SkewnessRate)
                       values
                        (@ID,@roll,@Dyelot,@Inspdate,@Inspector,@Result,@Remark,@AddDate,@AddName,@HorizontalRate,@HorizontalOriginal,
                        @HorizontalTest1,@HorizontalTest2,@HorizontalTest3,@VerticalRate,@VerticalOriginal,@VerticalTest1,@VerticalTest2,@VerticalTest3,@SkewnessTest1,@SkewnessTest2,@SkewnessTest3,@SkewnessTest4,@SkewnessRate)";
                    spamAdd.Add(new SqlParameter("@id", this.maindr["ID"]));
                    spamAdd.Add(new SqlParameter("@roll", dr["Roll"]));
                    spamAdd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamAdd.Add(new SqlParameter("@Inspdate", inspdate));
                    spamAdd.Add(new SqlParameter("@Inspector", dr["Inspector"]));
                    spamAdd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamAdd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamAdd.Add(new SqlParameter("@AddDate", today));
                    spamAdd.Add(new SqlParameter("@AddName", this.loginID));
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
                    spamAdd.Add(new SqlParameter("@SkewnessTest1", dr["SkewnessTest1"]));
                    spamAdd.Add(new SqlParameter("@SkewnessTest2", dr["SkewnessTest2"]));
                    spamAdd.Add(new SqlParameter("@SkewnessTest3", dr["SkewnessTest3"]));
                    spamAdd.Add(new SqlParameter("@SkewnessTest4", dr["SkewnessTest4"]));
                    spamAdd.Add(new SqlParameter("@SkewnessRate", dr["SkewnessRate"]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamAdd);
                    if (!upResult)
                    {
                        return upResult;
                    }
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    List<SqlParameter> spamUpd = new List<SqlParameter>();
                    update_cmd =
                    @"update FIR_Laboratory_Wash 
                    set ID=@id,roll=@roll,Dyelot=@Dyelot,Inspdate=@Inspdate,Inspector=@Inspector,
                    Result=@Result,Remark=@Remark,EditDate=@EditDate,EditName=@EditName,
                    HorizontalRate=@HorizontalRate,HorizontalOriginal=@HorizontalOriginal,
                    HorizontalTest1=@HorizontalTest1,HorizontalTest2=@HorizontalTest2,HorizontalTest3=@HorizontalTest3,
                    VerticalRate=@VerticalRate,VerticalOriginal=@VerticalOriginal,VerticalTest1=@VerticalTest1,
                    VerticalTest2=@VerticalTest2,VerticalTest3=@VerticalTest3,
                    SkewnessTest1=@SkewnessTest1,SkewnessTest2=@SkewnessTest2,
                    SkewnessTest3=@SkewnessTest3,SkewnessTest4=@SkewnessTest4,
                    SkewnessRate=@SkewnessRate
                    where id=@sid and Roll=@RollBefore";
                    spamUpd.Add(new SqlParameter("@id", dr["ID"]));
                    spamUpd.Add(new SqlParameter("@roll", dr["Roll"]));
                    spamUpd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamUpd.Add(new SqlParameter("@Inspdate", inspdate));
                    spamUpd.Add(new SqlParameter("@Inspector", dr["Inspector"]));
                    spamUpd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamUpd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamUpd.Add(new SqlParameter("@EditDate", today));
                    spamUpd.Add(new SqlParameter("@EditName", this.loginID));
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
                    spamUpd.Add(new SqlParameter("@SkewnessTest1", dr["SkewnessTest1"]));
                    spamUpd.Add(new SqlParameter("@SkewnessTest2", dr["SkewnessTest2"]));
                    spamUpd.Add(new SqlParameter("@SkewnessTest3", dr["SkewnessTest3"]));
                    spamUpd.Add(new SqlParameter("@SkewnessTest4", dr["SkewnessTest4"]));
                    spamUpd.Add(new SqlParameter("@SkewnessRate", dr["SkewnessRate"]));
                    spamUpd.Add(new SqlParameter("@sid", this.maindr["ID"]));
                    spamUpd.Add(new SqlParameter("@RollBefore", dr["Roll", DataRowVersion.Original]));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamUpd);
                    if (!upResult)
                    {
                        return upResult;
                    }
                }

            }

            List<SqlParameter> spamOption = new List<SqlParameter>();
            string updateOptionID =
            @"update FIR_Laboratory
                set SkewnessOptionID=@OptionID
                where id=@ID";
            spamOption.Add(new SqlParameter("@id", this.ID));
            spamOption.Add(new SqlParameter("@OptionID", this.radioPanel1.Value));
            upResult = DBProxy.Current.Execute(null, updateOptionID, spamOption);
            if (!upResult)
            {
                return upResult;
            }

            return upResult;
        }

        // 編輯權限設定
        private void Button_enable()
        {
            if (this.maindr == null)
            {
                return;
            }

            this.btnEncode.Enabled = this.CanEdit && !this.EditMode;
            this.btnEncode.Text = MyUtility.Convert.GetBool(this.maindr["WashEncode"]) ? "Amend" : "Encode";
            this.btnToExcel.Enabled = !this.EditMode;
            this.radioPanel1.ReadOnly = !this.EditMode;
            this.txtsupplierSupp.TextBox1.ReadOnly = true;
        }

        // maindr where id,poid重新query
        private void MainDBQuery()
        {
            string cmd = @"select a.id,a.poid,(a.SEQ1+a.SEQ2) as seq,a.SEQ1,a.SEQ2,a.ReceivingID,Refno,a.SCIRefno,
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
            spam.Add(new SqlParameter("@id", this.ID));
            spam.Add(new SqlParameter("@poid", this.maindr["poid"]));
            if (!MyUtility.Check.Seek(cmd, spam, out this.maindr))
            {
                MyUtility.Msg.WarningBox("Data is empty");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            if (!MyUtility.Convert.GetBool(this.maindr["WashEncode"]))
            {
                // 判斷有勾選可Encode
                if (!MyUtility.Convert.GetBool(this.maindr["nonWash"]))
                {
                    // 至少檢驗一卷 並且出現在Fir_Continuity.Roll
                    DataTable rolldt;
                    string cmd = string.Format(
                        @"Select roll from dbo.View_AllReceivingDetail a WITH (NOLOCK) where 
                        a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
                        and exists 
                        (Select distinct dyelot from FIR_Laboratory_Wash b WITH (NOLOCK) where b.id='{1}' and a.roll = b.roll)",
                        this.maindr["receivingid"], this.maindr["id"], this.maindr["POID"], this.maindr["seq1"], this.maindr["seq2"]);
                    DualResult dResult;
                    if (dResult = DBProxy.Current.Select(null, cmd, out rolldt))
                    {
                        if (rolldt.Rows.Count < 1)
                        {
                            MyUtility.Msg.WarningBox("Must inspect at least 1 roll data.");
                            return;
                        }
                    }
                }
                #region 判斷Wash Result
                DataTable gridDt = (DataTable)this.gridbs.DataSource;
                DataRow[] resultAry = gridDt.Select("Result='Fail'");
                string result = "Pass";
                if (resultAry.Length > 0)
                {
                    result = "Fail";
                }
                #endregion
                #region 判斷表身最晚時間
                DataTable dt = (DataTable)this.gridbs.DataSource;

                if (dt.Rows.Count != 0)
                {
                    DateTime lastDate = Convert.ToDateTime(dt.Rows[0]["inspDate"]);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime newDate = Convert.ToDateTime(dt.Rows[i]["inspDate"]);

                        // 代表newDate 比  lastDate還晚 就取代lastDate
                        if (DateTime.Compare(newDate, lastDate) > 0)
                        {
                            lastDate = newDate;
                        }
                    }

                    updatesql = string.Format(
                    @"Update Fir_Laboratory set WashDate = '{2}',WashEncode = 1,Wash='{0}',WashInspector = '{3}' where id ='{1}'", result, this.maindr["ID"], lastDate.ToShortDateString(), Env.User.UserID);
                }
                else
                {
                    updatesql = string.Format(
                    @"Update Fir_Laboratory set WashEncode = 1,Wash='{0}',WashInspector='{2}' where id ='{1}'", result, this.maindr["ID"], Env.User.UserID);
                }
                #endregion
            }
            else
            {
                // Amend
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir_Laboratory set WashDate = null,WashEncode= 0,Wash = '', WashInspector='' where id ='{0}'", this.maindr["ID"]);

                // updatesql = updatesql + string.Format(@"update FIR_Laboratory_Wash set editName='{0}',editDate=Getdate() where id='{1}'", loginID, maindr["ID"]);
                #endregion
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #region Over All Result 寫入
            string[] returnstr = PublicPrg.Prgs.GetOverallResult_Lab(this.maindr["ID"]);
            this.maindr["Result"] = returnstr[0];
            string cmdResult = @"update Fir_Laboratory set Result=@Result where id=@id";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@Result", returnstr[0]));
            spam.Add(new SqlParameter("@id", this.maindr["ID"]));
            DBProxy.Current.Execute(null, cmdResult, spam);

            // 更新PO.FIRLabInspPercent
            if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIRLab','{this.maindr["POID"]}'")))
            {
                this.ShowErr(upResult);
                return;
            }
            #endregion

            this.OnRequery();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            string[] columnNames = new string[]
            {
                "Roll", "Dyelot", "HorizontalOriginal", "VerticalOriginal", "Result", "HorizontalTest1", "HorizontalTest2", "HorizontalTest3", "Vertical_Average", "Horizontalrate",
                "VerticalTest1", "VerticalTest2", "VerticalTest3", "Vertical_Average", "VerticalRate", "SkewnessTest1", "SkewnessTest2", "SkewnessTest3", "SkewnessTest4", "SkewnessRate", "InspDate", "Inspector", "Inspector", "Remark", "Last update",
            };

            string skewnessOption = this.radioPanel1.Value;

            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, this.grid.Columns.Count) as object[,];
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

            // 撈seasonID
            DataTable dtSeason;
            string seasonID;
            DBProxy.Current.Select("Production", string.Format(
            "select C.SeasonID from FIR_Laboratory_Wash a WITH (NOLOCK) left join FIR_Laboratory b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C WITH (NOLOCK) ON B.POID=C.ID where a.ID='{0}'", this.maindr["ID"]), out dtSeason);
            if (dtSeason.Rows.Count == 0)
            {
                seasonID = string.Empty;
            }
            else
            {
                seasonID = dtSeason.Rows[0]["SeasonID"].ToString();
            }

            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            MyUtility.Excel.CopyToXls(ret, xltFileName: "Quality_P03_Wash_Test.xltx", headerline: 5, openfile: false, excelAppObj: excel);
            Microsoft.Office.Interop.Excel.Worksheet excelSheets = excel.ActiveWorkbook.Worksheets[1]; // 取得工作表
            excelSheets.Cells[2, 2] = this.txtSP.Text.ToString();
            excelSheets.Cells[2, 4] = this.txtSEQ.Text.ToString();
            excelSheets.Cells[2, 6] = this.txtColor.Text.ToString();
            excelSheets.Cells[2, 8] = this.txtStyle.Text.ToString();
            excelSheets.Cells[2, 10] = seasonID;
            excelSheets.Cells[3, 2] = this.txtSCIRefno.Text.ToString();
            excelSheets.Cells[3, 4] = this.txtWkno.Text.ToString();
            excelSheets.Cells[3, 6] = this.txtResult.Text.ToString();
            excelSheets.Cells[3, 8] = this.dateLastInspectionDate.Value;
            excelSheets.Cells[3, 10] = this.txtBrand.Text.ToString();
            excelSheets.Cells[4, 2] = this.txtBrandRefno.Text.ToString();
            excelSheets.Cells[4, 4] = this.txtArriveQty.Text.ToString();
            excelSheets.Cells[4, 6] = this.dateArriveWHDate.Value;
            excelSheets.Cells[4, 8] = this.txtsupplierSupp.DisplayBox1.Text.ToString();
            excelSheets.Cells[4, 10] = this.checkNA.Value.ToString();

            // SkewnessOption欄位名稱改變
            switch (skewnessOption)
            {
                case "1":
                    excelSheets.Cells[5, 16] = "AC";
                    excelSheets.Cells[5, 17] = "BD";
                    break;
                case "2":
                    excelSheets.Cells[5, 16] = "AA’";
                    excelSheets.Cells[5, 17] = "DD’";
                    excelSheets.Cells[5, 18] = "AB";
                    excelSheets.Cells[5, 19] = "CD";
                    break;
                case "3":
                    excelSheets.Cells[5, 16] = "AA’";
                    excelSheets.Cells[5, 17] = "AB";
                    break;
                default:
                    break;
            }

            // 只有2 有4欄，因此1和3藏起來
            if (skewnessOption != "2")
            {
                excelSheets.get_Range("R:R").EntireColumn.Hidden = true;
                excelSheets.get_Range("S:S").EntireColumn.Hidden = true;
            }

            excel.Cells.EntireColumn.AutoFit();    // 自動欄寬
            excel.Cells.EntireRow.AutoFit();      ////自動欄高

            // 只有2 有4欄，因此1和3藏起來
            if (skewnessOption != "2")
            {
                excelSheets.get_Range("R:R").EntireColumn.Hidden = true;
                excelSheets.get_Range("S:S").EntireColumn.Hidden = true;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Quality_P03_Wash_Test");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(excelSheets);

            strExcelName.OpenFile();
            #endregion
        }

        /// <summary>
        /// Calculation Skewness value
        /// </summary>
        /// <param name="dr">current dataRow</param>
        private void CalSkeValue(DataRow dr)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (this.radioOption1.Checked && (dr["SkewnessTest1"] != DBNull.Value && dr["SkewnessTest2"] != DBNull.Value))
            {
                if (!MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(dr["SkewnessTest1"]) + MyUtility.Convert.GetDecimal(dr["SkewnessTest2"])))
                {
                    decimal skewnessRate = (Math.Abs((decimal)dr["SkewnessTest1"] - (decimal)dr["SkewnessTest2"]) / ((decimal)dr["SkewnessTest1"] + (decimal)dr["SkewnessTest2"])) * 2 * 100;
                    dr["SkewnessRate"] = Math.Round(skewnessRate, 2);
                }
                else
                {
                    dr["SkewnessRate"] = 0;
                    return;
                }
            }

            if (this.radioOption2.Checked && (dr["SkewnessTest1"] != DBNull.Value && dr["SkewnessTest2"] != DBNull.Value && dr["SkewnessTest3"] != DBNull.Value && dr["SkewnessTest4"] != DBNull.Value))
            {
                if (!MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(dr["SkewnessTest3"]) + MyUtility.Convert.GetDecimal(dr["SkewnessTest4"])))
                {
                    decimal skewnessRate = (Math.Abs((decimal)dr["SkewnessTest1"] + (decimal)dr["SkewnessTest2"]) / ((decimal)dr["SkewnessTest3"] + (decimal)dr["SkewnessTest4"])) * 100;
                    dr["SkewnessRate"] = Math.Round(skewnessRate, 2);
                }
                else
                {
                    dr["SkewnessRate"] = 0;
                    return;
                }
            }

            if (this.radioOption3.Checked && (dr["SkewnessTest1"] != DBNull.Value && dr["SkewnessTest2"] != DBNull.Value))
            {
                if (!MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(dr["SkewnessTest2"])))
                {
                    decimal skewnessRate = Math.Abs((decimal)dr["SkewnessTest1"] / (decimal)dr["SkewnessTest2"]) * 100;
                    dr["SkewnessRate"] = Math.Round(skewnessRate, 2);
                }
                else
                {
                    dr["SkewnessRate"] = 0;
                    return;
                }
            }
        }

        /// <summary>
        /// Calculation HorizontalRate Value
        /// </summary>
        /// <param name="dr">current dataRow</param>
        private void CalHorRate(DataRow dr)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (!MyUtility.Check.Empty(dr["HorizontalOriginal"]))
            {
                decimal newValue = ((((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3) - (decimal)dr["HorizontalOriginal"]) / (decimal)dr["HorizontalOriginal"] * 100;
                dr["HorizontalRate"] = Math.Round(newValue, 2);
            }
            else
            {
                dr["HorizontalRate"] = 0;
                return;
            }

            decimal newAvgValue = ((decimal)dr["HorizontalTest1"] + (decimal)dr["HorizontalTest2"] + (decimal)dr["HorizontalTest3"]) / 3;
            dr["Horizontal_Average"] = Math.Round(newAvgValue, 2);
        }

        /// <summary>
        /// Calculation VerticalRate Value
        /// </summary>
        /// <param name="dr">current dataRow</param>
        private void CalVerRate(DataRow dr)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (!MyUtility.Check.Empty(dr["VerticalOriginal"]))
            {
                decimal newValue = ((((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3) - (decimal)dr["VerticalOriginal"]) / (decimal)dr["VerticalOriginal"] * 100;
                dr["VerticalRate"] = Math.Round(newValue, 2);
            }
            else
            {
                dr["VerticalRate"] = 0;
                return;
            }

            decimal newAvgValue = ((decimal)dr["VerticalTest1"] + (decimal)dr["VerticalTest2"] + (decimal)dr["VerticalTest3"]) / 3;
            dr["Vertical_Average"] = Math.Round(newAvgValue, 2);
        }

        private void RadioPanel1_ValueChanged(object sender, EventArgs e)
        {
            switch (this.radioPanel1.Value.ToString())
            {
                case "1":
                    this.pictureBox1.ImageLocation = this.ht["Picture1"].ToString();
                    break;
                case "2":
                    this.pictureBox1.ImageLocation = this.ht["Picture2"].ToString();
                    break;
                case "3":
                    this.pictureBox1.ImageLocation = this.ht["Picture3"].ToString();
                    break;
                default:
                    break;
            }

            this.GridView_Visable();
            if (!((DataTable)this.gridbs.DataSource).Empty() && ((DataTable)this.gridbs.DataSource).Rows.Count > 0)
            {
                foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                {
                    this.CalSkeValue(dr);
                }
            }
        }
    }
}
