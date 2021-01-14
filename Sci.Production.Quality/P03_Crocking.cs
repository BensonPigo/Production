using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Windows.Forms;
using Sci.Production.PublicPrg;
using System.Diagnostics;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P03_Crocking : Win.Subs.Input4
    {
        private readonly string loginID = Env.User.UserID;
        private readonly string ID;
        private DataRow maindr;
        private int crockingTestOption = 0;
        private string singleCubeRangeSource = $"A1:N36";
        private string doubleCubeRangeSource = $"A39:N110";

        /// <inheritdoc/>
        public P03_Crocking(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.ID = id.Trim();
            string sqlGetCrockingTestOption = $@"
declare @CrockingTestOption tinyint

select @CrockingTestOption = CrockingTestOption 
from QABrandSetting with (nolock) 
where BrandID = ( select  o.BrandID
                  from Fir f with (nolock)
                  inner join Orders o with (nolock) on f.POID = o.ID
                  where f.ID = '{id}')

select [CrockingTestOption] = isnull(@CrockingTestOption, 0)
";
            this.crockingTestOption = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlGetCrockingTestOption));
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.Button_enable();
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery()
        {
            this.MainDBQuery();
            this.Button_enable();

            // 表頭 資料設定
            this.save.Enabled = !MyUtility.Convert.GetBool(this.maindr["CrockingEncode"]);
            string fir_cmd = string.Format(
@"select distinct 
    a.Poid,a.ArriveQty,a.SCIRefno,a.Refno,
	seq = CONCAT(a.SEQ1,a.SEQ2),
    b.styleid,b.BrandID,
    c.ExportId,c.WhseArrival,
    d.ColorID,
    e.CrockingDate,e.Crocking,e.nonCrocking,
    [CrockingInspector] = (select name from pass1 where id= e.CrockingInspector),
    f.SuppID,
    g.DescDetail
from FIR a WITH (NOLOCK) 
left join Orders b WITH (NOLOCK) on a.POID=b.POID
left join Receiving c WITH (NOLOCK) on a.ReceivingID=c.Id
left join PO_Supp_Detail d WITH (NOLOCK) on d.ID=a.POID and a.SEQ1=d.SEQ1 and a.seq2=d.SEQ2
left join FIR_Laboratory e WITH (NOLOCK) on a.ID=e.ID
left join PO_Supp f WITH (NOLOCK) on d.ID=f.ID and d.SEQ1=f.SEQ1
left join Fabric g WITH (NOLOCK) on g.SCIRefno = a.SCIRefno
				where a.ID='{0}'", this.ID);
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
                this.dateLastInspectionDate.Value = MyUtility.Convert.GetDate(fir_dr["CrockingDate"]);
                this.txtResult.Text = fir_dr["Crocking"].ToString();
                this.checkNA.Value = fir_dr["nonCrocking"].ToString();
                this.editDescription.Text = fir_dr["DescDetail"].ToString();
                this.txtCrockingInspector.Text = fir_dr["CrockingInspector"].ToString();
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
                this.txtCrockingInspector.Text = string.Empty;
            }

            return base.OnRequery();
        }

        /// <inheritdoc/>
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
                dr["poid"] = this.maindr["poid"];
                dr["SEQ1"] = this.maindr["SEQ1"];
                dr["SEQ2"] = this.maindr["SEQ2"];
                string name = MyUtility.Check.Empty(datas.Rows[i]["EditName"].ToString()) ? MyUtility.GetValue.Lookup("Name_Extno", datas.Rows[i]["AddName"].ToString(), "View_ShowName", "ID") :
                   MyUtility.GetValue.Lookup("Name_Extno", datas.Rows[i]["EditName"].ToString(), "View_ShowName", "ID");
                string date = MyUtility.Check.Empty(datas.Rows[i]["EditDate"].ToString()) ? datas.Rows[i]["AddDate"].ToString() : datas.Rows[i]["EditDate"].ToString();
                dr["Last update"] = name + " - " + date;

                i++;
            }
        }

        private Ict.Win.UI.DataGridViewTextBoxColumn ResultCell;

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings scaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings labTechCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings inspDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings inspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings dyelotCell = new DataGridViewGeneratorTextColumnSettings();

            #region grid MouseClickEvent
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

            scaleCell.EditingMouseDown += (s, e) =>
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
                    string colName = ((DataGridViewColumn)s).Name;
                    string scalecmd = @"select id from Scale WITH (NOLOCK) where junk!=1";
                    SelectItem item1 = new SelectItem(scalecmd, "15", dr[colName].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr[colName] = item1.GetSelectedString();
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

                    // string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) ";
                    string scalecmd = $@"select DISTINCT Inspector,b.name from FIR_Laboratory_Crocking a WITH (NOLOCK) 
                                         INNER join Pass1 b WITH (NOLOCK) on a.Inspector=b.ID
                                         ";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Inspector"] = item1.GetSelectedString(); // 將選取selectitem value帶入GridView
                    dr["Name"] = item1.GetSelecteds()[0]["Name"];
                }
            };

            resultCell.CellMouseDoubleClick += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                string colName = ((DataGridViewColumn)s).Name;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (dr[colName].ToString().ToUpper() == "PASS")
                {
                    dr[colName] = "Fail";
                }
                else
                {
                    dr[colName] = "Pass";
                }

                this.Resultchange();
            };
            #endregion

            #region Valid 檢驗
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

            labTechCell.CellValidating += (s, e) =>
            {
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
            inspDateCell.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    MyUtility.Msg.WarningBox("<Lab Tech> cannot be empty!");
                    e.Cancel = true;
                    return;
                }
            };

            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), settings: rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), settings: dyelotCell)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true).Get(out this.ResultCell);
            if (this.crockingTestOption == 0)
            {
                this.Helper.Controls.Grid.Generator(this.grid)
                .Text("DryScale", header: "Dry Scale", width: Widths.AnsiChars(5), settings: scaleCell, iseditingreadonly: true)
                .Text("ResultDry", header: "Result(Dry)", width: Widths.AnsiChars(5), settings: resultCell, iseditingreadonly: true)
                .Text("WetScale", header: "Wet Scale", width: Widths.AnsiChars(5), settings: scaleCell, iseditingreadonly: true)
                .Text("ResultWet", header: "Result(Wet)", width: Widths.AnsiChars(5), settings: resultCell, iseditingreadonly: true);
            }
            else
            {
                this.Helper.Controls.Grid.Generator(this.grid)
                .Text("DryScale", header: "Dry Scale (Warp)", width: Widths.AnsiChars(5), settings: scaleCell, iseditingreadonly: true)
                .Text("ResultDry", header: "Dry Result (Warp)", width: Widths.AnsiChars(5), settings: resultCell, iseditingreadonly: true)
                .Text("DryScale_Weft", header: "Dry Scale (Weft)", width: Widths.AnsiChars(5), settings: scaleCell, iseditingreadonly: true)
                .Text("ResultDry_Weft", header: "Dry Result (Warp)", width: Widths.AnsiChars(5), settings: resultCell, iseditingreadonly: true)
                .Text("WetScale", header: "Wet Scale (Warp)", width: Widths.AnsiChars(5), settings: scaleCell, iseditingreadonly: true)
                .Text("ResultWet", header: "Wet Result (Warp)", width: Widths.AnsiChars(5), settings: resultCell, iseditingreadonly: true)
                .Text("WetScale_Weft", header: "Wet Scale (Weft)", width: Widths.AnsiChars(5), settings: scaleCell, iseditingreadonly: true)
                .Text("ResultWet_Weft", header: "Wet Result (Weft)", width: Widths.AnsiChars(5), settings: resultCell, iseditingreadonly: true);
            }

            this.Helper.Controls.Grid.Generator(this.grid)
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10), settings: inspDateCell)
            .Text("Inspector", header: "Lab Tech", width: Widths.AnsiChars(16), settings: labTechCell)
            .Text("Name", header: "Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(16))
            .Text("Last update", header: "Last update", width: Widths.AnsiChars(50), iseditingreadonly: true);

            #region 可編輯欄位變色
            this.grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            return true;
        }

        private void Resultchange()
        {
            if (MyUtility.Convert.GetString(this.CurrentData["ResultDry"]).EqualString("Fail") ||
                MyUtility.Convert.GetString(this.CurrentData["ResultWet"]).EqualString("Fail") ||
                MyUtility.Convert.GetString(this.CurrentData["ResultDry_Weft"]).EqualString("Fail") ||
                MyUtility.Convert.GetString(this.CurrentData["ResultWet_Weft"]).EqualString("Fail"))
            {
                this.CurrentData["Result"] = "Fail";
            }
            else
            {
                this.CurrentData["Result"] = "Pass";
            }

            this.CurrentData.EndEdit();
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
                        MyUtility.Msg.WarningBox($"<Roll>{MyUtility.Convert.GetString(dr["Roll"])}, <Dyelot>{MyUtility.Convert.GetString(dr["Dyelot"])} is already exist ! ");
                        return false;
                    }
                }
            }

            bool isScaleResultEmpty = afterDT.AsEnumerable().Any(row =>
                                                    MyUtility.Check.Empty(row["DryScale"]) ||
                                                    MyUtility.Check.Empty(row["ResultDry"]) ||
                                                    MyUtility.Check.Empty(row["WetScale"]) ||
                                                    MyUtility.Check.Empty(row["ResultWet"]) ||
                                                    (this.crockingTestOption == 1 && (MyUtility.Check.Empty(row["DryScale_Weft"]) ||
                                                                                      MyUtility.Check.Empty(row["ResultDry_Weft"]) ||
                                                                                      MyUtility.Check.Empty(row["WetScale_Weft"]) ||
                                                                                      MyUtility.Check.Empty(row["ResultWet_Weft"])))
                                                    );

            if (isScaleResultEmpty)
            {
                MyUtility.Msg.WarningBox("<Scale> & <Result> cannot be empty.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["Result"])))
            {
                MyUtility.Msg.WarningBox("<Result> can not be empty.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["Resultdry"])))
            {
                MyUtility.Msg.WarningBox("<Result(dry)> can not be empty.");
                return false;
            }

            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["Resultwet"])))
            {
                MyUtility.Msg.WarningBox("<Result(wet)> can not be empty.");
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
                    update_cmd = "Delete From FIR_Laboratory_Crocking Where id =@id and Roll=@roll ";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@roll", dr["Roll", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    if (!upResult)
                    {
                        return upResult;
                    }

                    continue;
                }

                // 轉換時間型態
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
                    update_cmd = @"insert into FIR_Laboratory_Crocking(ID,roll,Dyelot,DryScale,WetScale,Inspdate,Inspector,Result,Remark,AddDate,AddName,ResultDry,ResultWet, DryScale_Weft, ResultDry_Weft, WetScale_Weft, ResultWet_Weft)
                    values(@ID,@roll,@Dyelot,@DryScale,@WetScale,@Inspdate,@Inspector,@Result,@Remark,@AddDate,@AddName,@ResultDry,@ResultWet, isnull(@DryScale_Weft, ''), isnull(@ResultDry_Weft, ''), isnull(@WetScale_Weft, ''), isnull(@ResultWet_Weft, ''))";
                    spamAdd.Add(new SqlParameter("@id", dr["ID"]));
                    spamAdd.Add(new SqlParameter("@roll", dr["roll"]));
                    spamAdd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamAdd.Add(new SqlParameter("@DryScale", dr["DryScale"]));
                    spamAdd.Add(new SqlParameter("@WetScale", dr["WetScale"]));
                    spamAdd.Add(new SqlParameter("@Inspdate", inspdate));
                    spamAdd.Add(new SqlParameter("@Inspector", this.loginID));
                    spamAdd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamAdd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamAdd.Add(new SqlParameter("@AddDate", today));
                    spamAdd.Add(new SqlParameter("@AddName", this.loginID));
                    spamAdd.Add(new SqlParameter("@ResultDry", dr["ResultDry"]));
                    spamAdd.Add(new SqlParameter("@ResultWet", dr["ResultWet"]));
                    spamAdd.Add(new SqlParameter("@DryScale_Weft", dr["DryScale_Weft"]));
                    spamAdd.Add(new SqlParameter("@ResultDry_Weft", dr["ResultDry_Weft"]));
                    spamAdd.Add(new SqlParameter("@WetScale_Weft", dr["WetScale_Weft"]));
                    spamAdd.Add(new SqlParameter("@ResultWet_Weft", dr["ResultWet_Weft"]));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamAdd);
                    if (!upResult)
                    {
                        return upResult;
                    }
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    List<SqlParameter> spamUpd = new List<SqlParameter>();
                    update_cmd = @"update FIR_Laboratory_Crocking
                    set ID=@ID,roll=@roll,Dyelot=@Dyelot,DryScale=@DryScale,WetScale=@WetScale,Inspdate=@Inspdate,Inspector=@Inspector,
                        Result=@Result,Remark=@Remark,EditDate=@EditDate,EditName=@EditName,ResultDry=@ResultDry,ResultWet=@ResultWet,
                        DryScale_Weft = @DryScale_Weft,
                        ResultDry_Weft = @ResultDry_Weft,
                        WetScale_Weft = @WetScale_Weft,
                        ResultWet_Weft = @ResultWet_Weft
                        where id=@id AND roll=@roll AND Dyelot=@Dyelot and roll=@rollbefore";

                    spamUpd.Add(new SqlParameter("@id", dr["ID"]));
                    spamUpd.Add(new SqlParameter("@roll", dr["roll"]));
                    spamUpd.Add(new SqlParameter("@Dyelot", dr["Dyelot"]));
                    spamUpd.Add(new SqlParameter("@DryScale", dr["DryScale"]));
                    spamUpd.Add(new SqlParameter("@WetScale", dr["WetScale"]));
                    spamUpd.Add(new SqlParameter("@Inspdate", inspdate));
                    spamUpd.Add(new SqlParameter("@Inspector", dr["Inspector"]));
                    spamUpd.Add(new SqlParameter("@Result", dr["Result"]));
                    spamUpd.Add(new SqlParameter("@Remark", dr["Remark"]));
                    spamUpd.Add(new SqlParameter("@EditDate", today));
                    spamUpd.Add(new SqlParameter("@EditName", this.loginID));
                    spamUpd.Add(new SqlParameter("@rollbefore", dr["Roll", DataRowVersion.Original]));
                    spamUpd.Add(new SqlParameter("@ResultDry", dr["ResultDry"]));
                    spamUpd.Add(new SqlParameter("@ResultWet", dr["ResultWet"]));
                    spamUpd.Add(new SqlParameter("@DryScale_Weft", dr["DryScale_Weft"]));
                    spamUpd.Add(new SqlParameter("@ResultDry_Weft", dr["ResultDry_Weft"]));
                    spamUpd.Add(new SqlParameter("@WetScale_Weft", dr["WetScale_Weft"]));
                    spamUpd.Add(new SqlParameter("@ResultWet_Weft", dr["ResultWet_Weft"]));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamUpd);
                    if (!upResult)
                    {
                        return upResult;
                    }
                }
            }

            return upResult;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            if (MyUtility.Check.Empty(this.CurrentData) && !MyUtility.Convert.GetBool(this.maindr["nonCrocking"]))
            {
                MyUtility.Msg.WarningBox("Please test one Roll least");
                return;
            }

            if (!MyUtility.Convert.GetBool(this.maindr["CrockingEncode"]))
            {
                // Encode
                #region 判斷Crocking Result
                DataTable gridDt = (DataTable)this.gridbs.DataSource;
                DataRow[] resultAry = gridDt.Select("Result='Fail'");
                string result = "Pass";
                string today = DateTime.Now.ToShortDateString();
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
                    #endregion

                    #region  寫入實體Table
                    updatesql = string.Format(
                    @"Update Fir_Laboratory set CrockingEncode = 1 , Crocking='{0}',CrockingDate ='{2}',CrockingInspector = '{3}' where id ='{1}'",
                    result, this.maindr["ID"], lastDate.ToShortDateString(), Env.User.UserID);

                    updatesql = updatesql + string.Format(@"update FIR_Laboratory_Crocking set inspDate='{1}' where id='{0}'", this.maindr["ID"], today);
                }
                else
                {
                    updatesql = string.Format(
                    @"Update Fir_Laboratory set CrockingEncode = 1,Crocking='{0}',CrockingInspector = '{2}'  where id ='{1}'", result, this.maindr["ID"], Env.User.UserID);
                }

                #endregion

            }
            else
            {
                // Amend
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir_Laboratory set CrockingDate = null,CrockingEncode= 0,Crocking = '',CrockingInspector = '' where id ='{0}'", this.maindr["ID"]);

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
                        MyUtility.Msg.InfoBox("Error Message：" + upResult);
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
            string[] returnstr = Prgs.GetOverallResult_Lab(this.maindr["ID"]);
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

        private void Button_enable()
        {
            if (this.maindr == null)
            {
                return;
            }

            this.btnEncode.Enabled = this.CanEdit && !this.EditMode;
            this.btnEncode.Text = MyUtility.Convert.GetBool(this.maindr["CrockingEncode"]) ? "Amend" : "Encode";
            this.btnToExcel.Enabled = !this.EditMode;
            this.btntoPDF.Enabled = !this.EditMode;
            this.txtsupplierSupp.TextBox1.ReadOnly = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            string[] columnNames;
            string excelName = string.Empty;
            switch (this.crockingTestOption)
            {
                case 0:
                    columnNames = new string[] { "Roll", "Dyelot", "DryScale", "WetScale", "Result", "InspDate", "Inspector", "Remark", "Last update" };
                    excelName = "Quality_P03_Crocking_Test.xltx";
                    break;
                default:
                    columnNames = new string[] { "Roll", "Dyelot", "DryScale", "DryScale_Weft", "WetScale", "WetScale_Weft", "Result", "InspDate", "Inspector", "Remark", "Last update" };
                    excelName = "Quality_P03_Crocking_Weft_Warp_Test.xltx";
                    break;
            }

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

            // 撈取seasonID
            DataTable dtSeason;
            string seasonID;
            DBProxy.Current.Select("Production", string.Format(
            "select C.SeasonID from FIR_Laboratory_Crocking a WITH (NOLOCK) left join FIR_Laboratory b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C WITH (NOLOCK) ON B.POID=C.ID where a.ID='{0}'", this.maindr["ID"]), out dtSeason);
            if (dtSeason.Rows.Count == 0)
            {
                seasonID = string.Empty;
            }
            else
            {
                seasonID = dtSeason.Rows[0]["SeasonID"].ToString();
            }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Workbooks.Add();
            MyUtility.Excel.CopyToXls(ret, xltFileName: excelName, fileName: string.Empty, openfile: false, headerline: 5, excelAppObj: excel);
            Microsoft.Office.Interop.Excel.Worksheet excelSheets = excel.ActiveWorkbook.Worksheets[1]; // 取得工作表
            excel.Cells[2, 2] = this.txtSP.Text.ToString();
            excel.Cells[2, 4] = this.txtSEQ.Text.ToString();
            excel.Cells[2, 6] = this.txtColor.Text.ToString();
            excel.Cells[2, 8] = this.txtStyle.Text.ToString();
            excel.Cells[2, 10] = seasonID;
            excel.Cells[3, 2] = this.txtSCIRefno.Text.ToString();
            excel.Cells[3, 4] = this.txtWkno.Text.ToString();
            excel.Cells[3, 6] = this.txtResult.Text.ToString();
            excel.Cells[3, 8] = this.dateLastInspectionDate.Value;
            excel.Cells[3, 10] = this.txtBrand.Text.ToString();
            excel.Cells[4, 2] = this.txtBrandRefno.Text.ToString();
            excel.Cells[4, 4] = this.txtArriveQty.Text.ToString();
            excel.Cells[4, 6] = this.dateArriveWHDate.Value;
            excel.Cells[4, 8] = this.txtsupplierSupp.DisplayBox1.Text.ToString();
            excel.Cells[4, 10] = this.checkNA.Value.ToString();

            excel.Cells.EntireColumn.AutoFit();    // 自動欄寬
            excel.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(excelSheets);

            strExcelName.OpenFile();
            #endregion
        }

        // maindr where id,poid重新query
        private void MainDBQuery()
        {
            string cmd = @"select a.id,a.poid,(a.SEQ1+a.SEQ2) as seq,a.SEQ1,a.SEQ2,a.Receivingid,Refno,a.SCIRefno,
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
                MyUtility.Msg.InfoBox("Data is empty.");
            }
        }

        private void BtntoPDF_Click(object sender, EventArgs e)
        {
            if (this.crockingTestOption == 0)
            {
                this.CreateExcelOnlyWetDry();
            }
            else
            {
                this.CreateExcelOnlyWEFTandWARP();
            }
        }

        private void CreateExcelOnlyWetDry()
        {
            this.ShowWaitMessage("Data Loading...");
            DataTable dtt = (DataTable)this.gridbs.DataSource;
            if (dtt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            string submitDate = string.Empty;
            if (!MyUtility.Check.Empty(this.maindr["ReceiveSampleDate"]))
            {
                submitDate = ((DateTime)MyUtility.Convert.GetDate(this.maindr["ReceiveSampleDate"])).ToString("yyyy") + "/" + ((DateTime)MyUtility.Convert.GetDate(this.maindr["ReceiveSampleDate"])).ToString("MM") + "/" + ((DateTime)MyUtility.Convert.GetDate(this.maindr["ReceiveSampleDate"])).ToString("dd");
            }

            string sqlcmd = $@"
SELECT distinct oc.article,fd.InspDate,a.Name
FROM Order_BOF bof
inner join PO_Supp_Detail p on p.id=bof.id and bof.SCIRefno=p.SCIRefno
inner join Order_ColorCombo OC on oc.id=p.id and oc.FabricCode=bof.FabricCode
inner join orders o on o.id = bof.id
inner join FIR_Laboratory f on f.poid = o.poid and f.seq1 = p.seq1 and f.seq2 = p.seq2
inner join FIR_Laboratory_Crocking fd on fd.id = f.id
outer apply
(
	select Name = stuff((
		select concat(',',Name)
		from pass1 
		where id = fd.Inspector
		for xml path('')
	),1,1,'')
)a
where bof.id='{this.maindr["POID"]}' and p.seq1='{this.maindr["seq1"]}' and p.seq2='{this.maindr["seq2"]}'
order by fd.InspDate,oc.article
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P03_Crocking_Test_for_PDF.xltx");

            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1];

                worksheet1.Copy(worksheetn);
            }

            int j = 0;
            foreach (DataRow row in dt.Rows)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[j + 1];   // 取得工作表
                worksheet.Cells[4, 3] = submitDate;
                if (!MyUtility.Check.Empty(row["InspDate"]))
                {
                    worksheet.Cells[4, 5] = ((DateTime)row["InspDate"]).ToString("yyyy") + "/" + ((DateTime)row["InspDate"]).ToString("MM") + "/" + ((DateTime)row["InspDate"]).ToString("dd");
                }

                worksheet.Cells[6, 9] = row["article"];
                worksheet.Cells[4, 7] = this.txtSP.Text;
                worksheet.Cells[4, 10] = this.txtBrand.Text;
                worksheet.Cells[6, 3] = this.txtStyle.Text;
                worksheet.Cells[6, 6] = MyUtility.GetValue.Lookup($"select CustPONo from orders where id = '{this.txtSP.Text}'");
                worksheet.Cells[7, 3] = MyUtility.GetValue.Lookup($@"select StyleName from Style s, orders o where o.id = '{this.txtSP.Text}' and  o.StyleUkey = s.ukey");
                worksheet.Cells[7, 9] = this.txtArriveQty.Text;
                worksheet.Cells[14, 8] = row["Name"];

                string sqlcmd2 = $@"
SELECT distinct fd.InspDate,oc.article,fd.DryScale,fd.ResultDry,fd.WetScale,fd.ResultWet,fd.Remark,fd.Inspector,fd.Roll,fd.Dyelot
FROM Order_BOF bof
inner join PO_Supp_Detail p on p.id=bof.id and bof.SCIRefno=p.SCIRefno
inner join Order_ColorCombo OC on oc.id=p.id and oc.FabricCode=bof.FabricCode
inner join orders o on o.id = bof.id
inner join FIR_Laboratory f on f.poid = o.poid and f.seq1 = p.seq1 and f.seq2 = p.seq2
inner join FIR_Laboratory_Crocking fd on fd.id = f.id
where bof.id='{this.maindr["POID"]}' and p.seq1='{this.maindr["seq1"]}' and p.seq2='{this.maindr["seq2"]}' 
    and oc.article = '{row["article"]}' and fd.InspDate = '{MyUtility.Convert.GetDate(row["InspDate"]).Value.ToShortDateString()}'
";
                DataTable dt2;
                DBProxy.Current.Select(null, sqlcmd2, out dt2);
                for (int i = 1; i < dt2.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A12:A12", Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }

                int k = 0;
                foreach (DataRow row2 in dt2.Rows)
                {
                    worksheet.Cells[11 + k, 2] = this.txtBrandRefno.Text;
                    worksheet.Cells[11 + k, 3] = this.txtColor.Text;
                    worksheet.Cells[11 + k, 4] = row2["Dyelot"];
                    worksheet.Cells[11 + k, 5] = row2["Roll"];
                    worksheet.Cells[11 + k, 6] = row2["DryScale"];
                    worksheet.Cells[11 + k, 7] = row2["ResultDry"];
                    worksheet.Cells[11 + k, 8] = row2["WetScale"];
                    worksheet.Cells[11 + k, 9] = row2["ResultWet"];
                    worksheet.Cells[11 + k, 10] = row2["Remark"];

                    Microsoft.Office.Interop.Excel.Range rg = worksheet.Range[worksheet.Cells[11 + k, 2], worksheet.Cells[11 + k, 10]];

                    // 加框線
                    rg.Borders.LineStyle = 1;
                    rg.Borders.Weight = 3;
                    rg.WrapText = true; // 自動換列
                    rg.Font.Bold = false;

                    // 水平,垂直置中
                    rg.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rg.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    k++;
                }

                // worksheet.get_Range("B9:J9").Font.Bold = true;
                // worksheet.Cells.EntireColumn.AutoFit();
                #region 開始畫格子

                #region 框框數量計算

                int detailCouunt = dt2.Rows.Count;

                // 超過36筆資料，PDF就會跳到下一頁
                // int onePageLimit = 36;

                // 框框數，最多只要顯示4個
                int cubeCount = 0;

                // 每個框框共7個Row高度，因此每多7筆資料，框框就少一個
                if (detailCouunt < 3)
                {
                    cubeCount = 4;
                }

                if (detailCouunt >= 3 && detailCouunt < 10)
                {
                    cubeCount = 3;
                }

                if (detailCouunt >= 10 && detailCouunt < 17)
                {
                    cubeCount = 2;
                }

                if (detailCouunt >= 17 && detailCouunt < 27)
                {
                    cubeCount = 1;
                }

                // 28~44筆資料，未達2頁，但又塞不下一個框框，因此為0

                // 若超過1頁，但未達3頁，還是要畫，第三頁開始不畫

                // 第二頁上面沒有那一堆表格，因此是原本的4 + 表格的10 = 16
                if (detailCouunt >= 43 && detailCouunt < 50)
                {
                    cubeCount = 4;
                }

                if (detailCouunt >= 50 && detailCouunt < 61)
                {
                    cubeCount = 3;
                }

                if (detailCouunt >= 61 && detailCouunt < 72)
                {
                    cubeCount = 2;
                }

                if (detailCouunt >= 72 && detailCouunt < 83)
                {
                    cubeCount = 1;
                }

                // 超過兩頁，會出現第三頁
                if (detailCouunt >= 83)
                {
                    cubeCount = 0;
                }

                #endregion

                // 開始畫
                if (cubeCount > 0)
                {
                    // 作法：先畫第一個，若框框超過一個，就用複製的

                    // 第一個框框上的文字
                    // 16 = 11 + 5
                    worksheet.Cells[16 + dt2.Rows.Count, 3] = "DRY";
                    worksheet.Cells[16 + dt2.Rows.Count, 8] = "WET";
                    Microsoft.Office.Interop.Excel.Range rg1 = worksheet.Range[worksheet.Cells[16 + dt2.Rows.Count, 3], worksheet.Cells[16 + dt2.Rows.Count, 8]];

                    // 置中
                    rg1.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // 畫框框
                    // 17 = 11 + 6
                    // 24 = 17 + 7
                    rg1 = worksheet.Range[worksheet.Cells[17 + dt2.Rows.Count, 2], worksheet.Cells[24 + dt2.Rows.Count, 4]];

                    // 框線設定
                    rg1.BorderAround2(LineStyle: 1);
                    rg1 = worksheet.Range[worksheet.Cells[17 + dt2.Rows.Count, 7], worksheet.Cells[24 + dt2.Rows.Count, 9]];
                    rg1.BorderAround2(LineStyle: 1);

                    // 框框旁邊的字
                    rg1 = worksheet.Range[worksheet.Cells[18 + dt2.Rows.Count, 5], worksheet.Cells[18 + dt2.Rows.Count, 6]];
                    rg1.Merge(true);
                    worksheet.Cells[18 + dt2.Rows.Count, 5] = "Ref# : _________________";

                    rg1 = worksheet.Range[worksheet.Cells[19 + dt2.Rows.Count, 5], worksheet.Cells[19 + dt2.Rows.Count, 6]];
                    rg1.Merge(true);
                    worksheet.Cells[19 + dt2.Rows.Count, 5] = "Color  : ________________";

                    rg1 = worksheet.Range[worksheet.Cells[20 + dt2.Rows.Count, 5], worksheet.Cells[20 + dt2.Rows.Count, 6]];
                    rg1.Merge(true);
                    worksheet.Cells[20 + dt2.Rows.Count, 5] = "Roll# : _________________";

                    rg1 = worksheet.Range[worksheet.Cells[21 + dt2.Rows.Count, 5], worksheet.Cells[21 + dt2.Rows.Count, 6]];
                    rg1.Merge(true);
                    worksheet.Cells[21 + dt2.Rows.Count, 5] = "Dyelot# : _______________";

                    rg1 = worksheet.Range[worksheet.Cells[24 + dt2.Rows.Count, 5], worksheet.Cells[24 + dt2.Rows.Count, 6]];
                    rg1.Merge(true);
                    worksheet.Cells[24 + dt2.Rows.Count, 5] = "Grade : ________________";
                    worksheet.Cells[24 + dt2.Rows.Count, 10] = "_____________";

                    // 選取要被複製的資料
                    rg1 = worksheet.get_Range($"B{17 + dt2.Rows.Count}:J{24 + dt2.Rows.Count}").EntireRow;

                    // 根據框框數，資料筆數，決定貼在哪個座標
                    switch (cubeCount)
                    {
                        case 2:
                            Microsoft.Office.Interop.Excel.Range rgX = worksheet.get_Range($"B{17 + dt2.Rows.Count + 9}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                            rgX.Insert(rg1.Copy(Type.Missing)); // 貼上
                            break;
                        case 3:
                            rgX = worksheet.get_Range($"B{17 + dt2.Rows.Count + 9}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                            rgX.Insert(rg1.Copy(Type.Missing)); // 貼上
                            Microsoft.Office.Interop.Excel.Range rgY = worksheet.get_Range($"B{17 + dt2.Rows.Count + 18}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                            rgY.Insert(rg1.Copy(Type.Missing)); // 貼上

                            break;
                        case 4:
                            rgX = worksheet.get_Range($"B{17 + dt2.Rows.Count + 9}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                            rgX.Insert(rg1.Copy(Type.Missing)); // 貼上
                            rgY = worksheet.get_Range($"B{17 + dt2.Rows.Count + 18}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                            rgY.Insert(rg1.Copy(Type.Missing)); // 貼上
                            Microsoft.Office.Interop.Excel.Range rgZ = worksheet.get_Range($"B{17 + dt2.Rows.Count + 27}", Type.Missing).EntireRow; // 選擇要被貼上的位置
                            rgZ.Insert(rg1.Copy(Type.Missing)); // 貼上

                            break;
                        default:
                            break;
                    }
                }

                #endregion

                Marshal.ReleaseComObject(worksheet);
                j++;
            }

            #region Save & Show Excel
            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;
            strFileName = Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test_for_PDF");
            strPDFFileName = Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test_for_PDF", Class.PDFFileNameExtension.PDF);
            objApp.ActiveWorkbook.SaveAs(strFileName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            #endregion

            if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                Process.Start(startInfo);
            }

            this.HideWaitMessage();
        }

        private class PageInfoForPDF
        {
            public int StartRow { get; set; }

            public bool IsSingle { get; set; }
        }

        private List<PageInfoForPDF> GetPageInfo(int firstStartRow, int ttlRowCnt)
        {
            List<PageInfoForPDF> infoForPDFs = new List<PageInfoForPDF>();
            int pagestartRow = firstStartRow;
            bool isSingle = true;

            if (firstStartRow >= 35 && firstStartRow <= 71)
            {
                pagestartRow = 73;
                isSingle = false;
            }

            if (firstStartRow > 71 && ((firstStartRow - 71) % 74) > 37)
            {
                pagestartRow = ((((firstStartRow - 71) / 74) + 1) * 74) + 73;
                isSingle = false;
            }

            infoForPDFs.Add(new PageInfoForPDF { StartRow = pagestartRow, IsSingle = isSingle });

            int removeFirstPageRowCnt = isSingle ? ttlRowCnt - 1 : ttlRowCnt - 2;
            int pageCnt = MyUtility.Convert.GetInt(Math.Ceiling(removeFirstPageRowCnt / 2.0));
            isSingle = false;
            for (int i = 0; i < pageCnt; i++)
            {
                if (i == pageCnt - 1 && (removeFirstPageRowCnt % 2) > 0)
                {
                    isSingle = true;
                }

                if (pagestartRow % 74 != 73)
                {
                    pagestartRow = pagestartRow < 72 ? 73 : ((((pagestartRow - 71) / 74) + 1) * 74) + 73;
                }
                else
                {
                    pagestartRow += 74;
                }

                infoForPDFs.Add(new PageInfoForPDF { StartRow = pagestartRow, IsSingle = isSingle });
            }

            return infoForPDFs;
        }

        private void CreateExcelOnlyWEFTandWARP()
        {
            this.ShowWaitMessage("Data Loading...");
            DataTable dtt = (DataTable)this.gridbs.DataSource;
            if (dtt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            string submitDate = string.Empty;
            if (!MyUtility.Check.Empty(this.maindr["ReceiveSampleDate"]))
            {
                submitDate = ((DateTime)MyUtility.Convert.GetDate(this.maindr["ReceiveSampleDate"])).ToString("yyyy") + "/" + ((DateTime)MyUtility.Convert.GetDate(this.maindr["ReceiveSampleDate"])).ToString("MM") + "/" + ((DateTime)MyUtility.Convert.GetDate(this.maindr["ReceiveSampleDate"])).ToString("dd");
            }

            string sqlcmd = $@"
SELECT distinct [Article] = Article.val,fd.InspDate,p1.Name, fd.Inspector
FROM  PO_Supp_Detail p with (nolock)
inner join Order_BOF bof with (nolock) on p.id=bof.id and bof.SCIRefno=p.SCIRefno
outer apply (SELECT val =  Stuff((select distinct concat( ',',oc.Article)   
                                 from Order_ColorCombo oc with (nolock)
                                 where oc.id=p.id and oc.FabricCode=bof.FabricCode and p.ColorID = oc.ColorID
                                 FOR XML PATH('')),1,1,'') ) Article
inner join FIR_Laboratory f on f.poid = p.ID and f.seq1 = p.seq1 and f.seq2 = p.seq2
inner join FIR_Laboratory_Crocking fd on fd.id = f.id
left join pass1 p1 with (nolock) on p1.id = fd.Inspector
where f.ID = '{this.ID}'
order by fd.InspDate
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P03_Crocking_Test_for_PDF_Weft_Warp.xltx");
            Microsoft.Office.Interop.Excel.Worksheet worksheetForCopyCube = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[2];
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1];

                worksheet1.Copy(worksheetn);
            }

            int j = 0;
            foreach (DataRow row in dt.Rows)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[j + 1];   // 取得工作表
                worksheet.Cells[3, 2] = submitDate;
                if (!MyUtility.Check.Empty(row["InspDate"]))
                {
                    worksheet.Cells[3, 5] = ((DateTime)row["InspDate"]).ToString("yyyy") + "/" + ((DateTime)row["InspDate"]).ToString("MM") + "/" + ((DateTime)row["InspDate"]).ToString("dd");
                }

                worksheet.Cells[5, 12] = row["article"];
                worksheet.Cells[3, 9] = this.txtSP.Text;
                worksheet.Cells[3, 13] = this.txtBrand.Text;
                worksheet.Cells[5, 2] = this.txtStyle.Text;
                worksheet.Cells[5, 5] = MyUtility.GetValue.Lookup($"select CustPONo from orders where id = '{this.txtSP.Text}'");
                worksheet.Cells[6, 2] = MyUtility.GetValue.Lookup($@"select StyleName from Style s, orders o where o.id = '{this.txtSP.Text}' and  o.StyleUkey = s.ukey");
                worksheet.Cells[6, 12] = this.txtArriveQty.Text;
                worksheet.Cells[13, 9] = row["Name"];

                string sqlcmd2 = $@"
SELECT  fd.DryScale,
        fd.ResultDry, 
        fd.DryScale_Weft, 
        fd.ResultDry_Weft,
        fd.WetScale_Weft,
        fd.ResultWet_Weft,
        fd.WetScale,
        fd.ResultWet,
        fd.Remark,
        fd.Inspector,
        fd.Roll,
        fd.Dyelot,
        fd.Result
FROM PO_Supp_Detail p with (nolock)
inner join Order_BOF bof with (nolock) on p.id=bof.id and bof.SCIRefno=p.SCIRefno
inner join orders o with (nolock) on o.id = bof.id
inner join FIR_Laboratory f with (nolock) on f.poid = o.poid and f.seq1 = p.seq1 and f.seq2 = p.seq2
inner join FIR_Laboratory_Crocking fd with (nolock) on fd.id = f.id
where bof.id='{this.maindr["POID"]}' and p.seq1='{this.maindr["seq1"]}' and p.seq2='{this.maindr["seq2"]}' 
    and fd.InspDate = '{MyUtility.Convert.GetDate(row["InspDate"]).Value.ToShortDateString()}'
    and fd.Inspector = '{MyUtility.Convert.GetString(row["Inspector"])}'
";
                DataTable dt2;
                DBProxy.Current.Select(null, sqlcmd2, out dt2);
                for (int i = 1; i < dt2.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A11:A11", Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }

                int k = 10;
                foreach (DataRow row2 in dt2.Rows)
                {
                    worksheet.Cells[k, 1] = this.txtBrandRefno.Text;
                    worksheet.Cells[k, 2] = this.txtColor.Text;
                    worksheet.Cells[k, 3] = row2["Dyelot"];
                    worksheet.Cells[k, 4] = row2["Roll"];
                    worksheet.Cells[k, 5] = row2["DryScale"];
                    worksheet.Cells[k, 6] = row2["ResultDry"];
                    worksheet.Cells[k, 7] = row2["DryScale_Weft"];
                    worksheet.Cells[k, 8] = row2["ResultDry_Weft"];
                    worksheet.Cells[k, 9] = row2["WetScale"];
                    worksheet.Cells[k, 10] = row2["ResultWet"];
                    worksheet.Cells[k, 11] = row2["WetScale_Weft"];
                    worksheet.Cells[k, 12] = row2["ResultWet_Weft"];
                    worksheet.Cells[k, 13] = row2["Result"];
                    worksheet.Cells[k, 14] = row2["Remark"];

                    Microsoft.Office.Interop.Excel.Range rg = worksheet.Range[worksheet.Cells[k, 1], worksheet.Cells[k, 14]];

                    // 加框線
                    rg.Borders.LineStyle = 1;
                    rg.Borders.Weight = 3;
                    rg.WrapText = true; // 自動換列
                    rg.Font.Bold = false;

                    // 水平,垂直置中
                    rg.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rg.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    k++;
                }

                // worksheet.get_Range("B9:J9").Font.Bold = true;
                // worksheet.Cells.EntireColumn.AutoFit();
                #region 開始畫格子

                int firstCubeStartRow = 15 + dt2.Rows.Count;

                List<PageInfoForPDF> infoForPDFs = this.GetPageInfo(firstCubeStartRow, dt2.Rows.Count);
                Microsoft.Office.Interop.Excel.Range cubeCopyRange;
                Microsoft.Office.Interop.Excel.Range pastCubeRange;
                foreach (PageInfoForPDF pageInfoForPDF in infoForPDFs)
                {
                    if (pageInfoForPDF.IsSingle)
                    {
                        cubeCopyRange = worksheetForCopyCube.get_Range(this.singleCubeRangeSource, Type.Missing).EntireRow;
                    }
                    else
                    {
                        cubeCopyRange = worksheetForCopyCube.get_Range(this.doubleCubeRangeSource, Type.Missing).EntireRow;
                    }

                    pastCubeRange = worksheet.get_Range($"A{pageInfoForPDF.StartRow}", Type.Missing).EntireRow;
                    pastCubeRange.Insert(cubeCopyRange.Copy(Type.Missing)); // 貼上
                }
                #endregion

                int lastPageRowNum = lastPageRowNum = 71 + (74 * (infoForPDFs.Count - 1));

                worksheet.PageSetup.PrintArea = $"$A$1:$N${lastPageRowNum.ToString()}";
                Marshal.ReleaseComObject(worksheet);
                j++;
            }

            worksheetForCopyCube.Delete();

            #region Save & Show Excel
            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;
            strFileName = Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test_for_PDF_Weft_Warp");
            strPDFFileName = Class.MicrosoftFile.GetName("Quality_P03_Crocking_Test_for_PDF_Weft_Warp", Class.PDFFileNameExtension.PDF);
            objApp.ActiveWorkbook.SaveAs(strFileName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            #endregion

            if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                Process.Start(startInfo);
            }

            this.HideWaitMessage();
        }
    }
}
