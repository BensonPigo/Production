using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using System.Linq;

namespace Sci.Production.PPIC
{
    public partial class P13 : Sci.Win.Tems.QueryForm
    {
        DataTable gridData;
        DataGridViewGeneratorTextColumnSettings sewLine = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        DataGridViewGeneratorDateColumnSettings sewInLine = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        DataGridViewGeneratorDateColumnSettings sewOffLine = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        DataGridViewGeneratorDateColumnSettings cutReadyDate = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            displayBox3.Value = Sci.Env.User.Factory;
        }

        protected override void OnFormLoaded()
        {
            if (MyUtility.GetValue.Lookup(string.Format("select UseAPS from Factory where ID = '{0}'", Sci.Env.User.Factory)) == "True")
            {
                MyUtility.Msg.WarningBox("Please use APS system to assign schedule.");
                Close();
            }

            base.OnFormLoaded();

            cutReadyDate.CellValidating += (s, e) =>
            {

                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && dr["CuttingSP"].ToString() != dr["ID"].ToString())
                {
                    MyUtility.Msg.WarningBox("This SP is not cutting sp, no need input Cutting Ready Date.");
                    dr["CutReadyDate"] = DBNull.Value;
                    return;
                }

                if ((!MyUtility.Check.Empty(e.FormattedValue) && !MyUtility.Check.Empty(dr["CutReadyDate"]) && Convert.ToDateTime(e.FormattedValue) != Convert.ToDateTime(dr["CutReadyDate"])) || !(MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Check.Empty(dr["CutReadyDate"])))
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if (Convert.ToDateTime(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-30))
                        {
                            MyUtility.Msg.WarningBox("< Cutting Ready Date > is invalid!!");
                            dr["CutReadyDate"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };

            sewInLine.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if ((!MyUtility.Check.Empty(e.FormattedValue) && !MyUtility.Check.Empty(dr["SewInLine"]) && Convert.ToDateTime(e.FormattedValue) != Convert.ToDateTime(dr["SewInLine"])) || !(MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Check.Empty(dr["SewInLine"])))
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if (Convert.ToDateTime(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-30))
                        {
                            MyUtility.Msg.WarningBox("< In Line Date > is invalid!!");
                            dr["SewInLine"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };

            sewOffLine.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (MyUtility.Check.Empty(dr["SewInLine"]) || (!MyUtility.Check.Empty(dr["SewInLine"]) && (Convert.ToDateTime(e.FormattedValue) < Convert.ToDateTime(dr["SewInLine"])))))
                {
                    MyUtility.Msg.WarningBox("< Off Line Date > is invalid!!");
                    dr["SewOffLine"] = DBNull.Value;
                    e.Cancel = true;
                    return;
                }
            };

            sewLine.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("Select ID,Description From SewingLine Where FactoryId = '{0}'", Sci.Env.User.Factory), "2,16", dr["SewLine"].ToString());
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            };
            sewLine.CellValidating += (s, e) =>
            {

                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                {

                    if (MyUtility.Check.Seek(string.Format("Select ID From SewingLine Where FactoryId = '{0}' and ID = '{1}'", Sci.Env.User.Factory, e.FormattedValue.ToString())) == false)
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Sewing Line : {0} > not found!!!", e.FormattedValue.ToString()));
                        dr["SewLine"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            };

            //Grid設定
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.grid1)
                .Date("SCIDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CdCodeID", header: "CD#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", iseditingreadonly: true)
                .Numeric("CPU", header: "CPU", decimal_places: 3, iseditingreadonly: true)
                .Text("SewLine", header: "Line#", width: Widths.AnsiChars(2), settings: sewLine)
                .Date("SewInLine", header: "In Line Date", settings: sewInLine)
                .Date("SewOffLine", header: "Off Line Date", settings: sewOffLine)
                .Date("CutReadyDate", header: "Cutting Ready Date", settings: cutReadyDate)
                .Date("LETA", header: "LastMTL ETA", iseditingreadonly: true)
                .Date("ActSewInLine", header: "Act. InLine", iseditingreadonly: true)
                .Date("ActSewOffLine", header: "Act. OffLine", iseditingreadonly: true)
                .Text("ArtworkType", header: "Artwork Type", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SewRemark", header: "Remark", width: Widths.AnsiChars(60))
                .Text("MR", header: "MR", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SMR", header: "SMR", width: Widths.AnsiChars(13), iseditingreadonly: true);

            grid1.Columns[9].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns[11].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns[12].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns[17].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns[9].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns[10].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns[11].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns[12].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns[17].DefaultCellStyle.ForeColor = Color.Red;
        }

        //SMR
        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            textBox3.Text = PopUpTPEUser(textBox3.Text);
        }

        //MR
        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            textBox4.Text = PopUpTPEUser(textBox4.Text);
        }

        private string PopUpTPEUser(string userID)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1", "15,30,10", userID);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return ""; }
            return item.GetSelectedString();
        }

        //SMR
        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            string textBox3Value = textBox3.Text;
            if (TPEUserValidating(textBox3Value))
            {
                textBox3.Text = textBox3Value;
                displayBox1.Value = GetUserName(textBox3.Text);
            }
            else
            {
                MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textBox3Value));
                textBox3.Text = "";
                displayBox1.Value = "";
                e.Cancel = true;
                return;
            }
        }

        //MR
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            string textBox4Value = textBox4.Text;
            if (TPEUserValidating(textBox4Value))
            {
                textBox4.Text = textBox4Value;
                displayBox2.Value = GetUserName(textBox4.Text);
            }
            else
            {
                MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textBox4Value));
                textBox4.Text = "";
                displayBox2.Value = "";
                e.Cancel = true;
                return;
            }
        }

        private bool TPEUserValidating(string userID)
        {
            if (!MyUtility.Check.Seek(userID, "TPEPass1", "ID"))
            {
                return false;
            }
            return true;
        }

        private string GetUserName(string UserID)
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 Where id='{0}'", UserID);
            DataRow dr;
            if (MyUtility.Check.Seek(selectSql, out dr))
            {
                string userName;
                userName = MyUtility.Check.Empty(dr["extNo"]) ? "" : dr["Name"].ToString();
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    userName = userName + " #" + dr["extNo"].ToString();
                }
                return userName;
            }
            else
            {
                return "";
            }
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(textBox1.Text) && MyUtility.Check.Empty(textBox1.Text) && MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2))
            {
                MyUtility.Msg.WarningBox("SP#  and SCI Delivery can't empty!!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"with tmpData
as
(select o.ID,o.StyleID,o.BrandID,o.SciDelivery,o.BuyerDelivery,o.CdCodeID,
 o.OrderTypeID,o.Qty,(o.CPU*o.CPUFactor) as CPU,o.SewLine,o.SewInLine,o.SewOffLine,o.CutReadyDate,
 o.LETA,o.SewRemark,isnull((o.MRHandle+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = o.MRHandle)),o.MRHandle) as MR,
 isnull((o.SMR+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = o.SMR)),o.SMR) as SMR,
 (select min(s.OutputDate) from SewingOutput s, SewingOutput_Detail sd where sd.OrderId = o.ID and sd.ID = s.ID and sd.QAQty > 0) as ActSewInLine,
 (select max(s.OutputDate) from SewingOutput s, SewingOutput_Detail sd where sd.OrderId = o.ID and sd.ID = s.ID and sd.QAQty > 0) as tmpActSewOffLine,
 (select min(a.QAQty) from (select ComboType,sum(QAQty) as QAQty from SewingOutput_Detail where OrderId = o.ID group by ComboType) a) as SewOutQty,
 (select '['+CAST(a.Abbreviation as nvarchar)+'],' from (
 select distinct at.Abbreviation from Order_Artwork oa,ArtworkType at
 where at.IsSubprocess = 1
 and oa.ID = o.ID
 and oa.ArtworkTypeID = at.ID) a
 for xml path('')) as tmpArtworkType,o.CuttingSP
 from Orders o
 where o.Finished = 0
 and o.IsForecast = 0
 and o.FtyGroup = '{0}'", Sci.Env.User.Factory));
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                sqlCmd.Append(string.Format(" and o.ID >= '{0}'", textBox1.Text));
            }
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                sqlCmd.Append(string.Format(" and o.ID <= '{0}'", textBox2.Text));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(txtstyle1.Text))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", txtstyle1.Text));
            }
            if (!MyUtility.Check.Empty(txtbrand1.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", txtbrand1.Text));
            }
            if (!MyUtility.Check.Empty(textBox3.Text))
            {
                sqlCmd.Append(string.Format(" and o.SMR = '{0}'", textBox3.Text));
            }
            if (!MyUtility.Check.Empty(textBox4.Text))
            {
                sqlCmd.Append(string.Format(" and o.MRHandle = '{0}'", textBox4.Text));
            }
            sqlCmd.Append(@")
select *,iif(isnull(SewOutQty,0) >= Qty, tmpActSewOffLine, null) as ActSewOffLine,SUBSTRING(tmpArtworkType,1, LEN(tmpArtworkType)-1) as ArtworkType from tmpData order by SCIDelivery");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Close
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Save
        private void button3_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty((DataTable)listControlBindingSource1.DataSource))
            {
                IList<string> updateCmds = new List<string>();
                this.grid1.ValidateControl();
                listControlBindingSource1.EndEdit();
                StringBuilder allSP = new StringBuilder();
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        updateCmds.Add(string.Format(@"update Orders set SewLine = '{0}', SewInLine = {1}, SewOffLine = {2}, CutReadyDate = {3}, SewRemark = '{4}' where ID = '{5}'",
                            dr["SewLine"].ToString(),
                            MyUtility.Check.Empty(dr["SewInLine"]) ? "null" : "'" + Convert.ToDateTime(dr["SewInLine"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["SewOffLine"]) ? "null" : "'" + Convert.ToDateTime(dr["SewOffLine"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["CutReadyDate"]) ? "null" : "'" + Convert.ToDateTime(dr["CutReadyDate"]).ToString("d") + "'",
                            dr["SewRemark"].ToString(), dr["ID"].ToString()));
                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));
                    }
                }
                if (allSP.Length != 0)
                {
                    DataTable OrderData, SewingData;

                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Id,SewInLine,SewOffLine",
                            string.Format("select o.ID,iif(isnull(o.SewInLine,'1900-01-01') <> isnull(t.SewInLine,'1900-01-01'),'1','') as InlineDiff,iif(isnull(o.SewOffLine,'1900-01-01') <> isnull(t.SewOffLine,'1900-01-01'),'1','') as OfflineDiff,o.SewInLine as OInline,t.SewInLine as TInLine,o.SewOffLine as OOffLine,t.SewOffLine as TOffLine from Orders o, #tmp t where o.ID in ({0}) and o.ID = t.ID", allSP.ToString().Substring(0, allSP.ToString().Length - 1)),
                            out OrderData);
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Query order error.", ex);
                        return;
                    }

                    foreach (DataRow dr in OrderData.Rows)
                    {
                        if (dr["InlineDiff"].ToString() == "1")
                        {
                            updateCmds.Add(string.Format(@"insert into Order_History (ID,ColumnName,OldValue,NewValue,Remark,AddName,AddDate)
values ('{0}','SewInLine',{1},{2},'Sewing Inline Update','{3}',GETDATE()) ", dr["ID"].ToString(),
                            MyUtility.Check.Empty(dr["OInline"]) ? "null" : "'" + Convert.ToDateTime(dr["OInline"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["TInLine"]) ? "null" : "'" + Convert.ToDateTime(dr["TInLine"]).ToString("d") + "'",
                            Sci.Env.User.UserID));
                        }
                        if (dr["OfflineDiff"].ToString() == "1")
                        {
                            updateCmds.Add(string.Format(@"insert into Order_History (ID,ColumnName,OldValue,NewValue,Remark,AddName,AddDate)
values ('{0}','SewOffLine',{1},{2},'Sewing Offline Update','{3}',GETDATE()) ", dr["ID"].ToString(),
                            MyUtility.Check.Empty(dr["OOffLine"]) ? "null" : "'" + Convert.ToDateTime(dr["OOffLine"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["TOffLine"]) ? "null" : "'" + Convert.ToDateTime(dr["TOffLine"]).ToString("d") + "'",
                            Sci.Env.User.UserID));
                        }
                    }

                    if (updateCmds.Count != 0)
                    {
                        DualResult result = DBProxy.Current.Executes(null, updateCmds);
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Save Fail!" + result.ToString());
                            return;
                        }
                    }

                    DualResult result1 = DBProxy.Current.Select(null, string.Format(@"with ttlSewTime
as
(select o.ID, isnull(sum(t.TotalSewingTime),0) as ttlSewTime
 from Orders o, TimeStudy t
 where t.BrandID = o.BrandID
 and t.StyleID = o.StyleID
 and t.SeasonID = o.SeasonID
 and o.ID in ({0})
 group by ID
)

select isnull(s.ID,0) as ID, o.ID as OrderID, SUBSTRING(o.SewLine,1,2) as SewingLineID,o.Qty as AlloQty,
o.SewInLine as Inline, o.SewOffLine as Offline,o.FtyGroup as FactoryID,sl.Sewer,st.ttlSewTime as TotalSewingTime,
100 as MaxEff,iif(st.ttlSewTime*sl.Sewer = 0,0,(3600.0/st.ttlSewTime*sl.Sewer)) as StandardOutput,
(select count(SewingLineID) from WorkHour where Hours > 0 and FactoryID = o.FtyGroup and Date between o.SewInLine and o.SewOffLine and SewingLineID = o.SewLine) as WorkDay,
iif((3600.0/st.ttlSewTime*sl.Sewer) = 0,0,floor(o.Qty/(3600.0/st.ttlSewTime*sl.Sewer))) as WorkHour,0 as APSNo, o.Finished as OrderFinished,'{1}'as AddName,GETDATE() as AddDate
from Orders o
left join SewingSchedule s on s.OrderID = o.ID
left join SewingLine sl on sl.ID = o.SewLine and sl.FactoryID = o.FtyGroup
left join ttlSewTime st on st.ID = o.ID
where o.ID in ({0})", allSP.ToString().Substring(0, allSP.ToString().Length - 1), Sci.Env.User.UserID), out SewingData);
                    
                    if (!result1)
                    {
                        MyUtility.Msg.ErrorBox("Query Sewing faill.\r\n" + result1.ToString());
                        return;
                    }
                    if (MyUtility.Tool.CursorUpdateTable(SewingData, "SewingSchedule", null))
                    {
                        //先刪除已不存在訂單中的資料
                        result1 = DBProxy.Current.Execute(null,string.Format(@"delete from SewingSchedule_Detail where OrderID in ({0})
and not exists (select * from Order_Qty where ID = OrderID and Article = Article and SizeCode = SizeCode)", allSP.ToString().Substring(0, allSP.ToString().Length - 1)));
                        if (!result1)
                        {
                            MyUtility.Msg.ErrorBox("Delete Sewing faill.\r\n" + result1.ToString());
                            return;
                        }
                        //新增 & 更新資料
                        result1 = DBProxy.Current.Select(null, string.Format(@"select isnull(s.ID,0) as ID,o.ID as OrderID,'' as ComboType,
o.SewLine as SewingLineID,oq.Article,oq.SizeCode,oq.Qty as AlloQty
from Orders o
left join Order_Qty oq on oq.ID = o.ID
left join SewingSchedule s on oq.ID = s.OrderID
where o.ID in ({0})", allSP.ToString().Substring(0, allSP.ToString().Length - 1)), out SewingData);
                        if (!result1)
                        {
                            MyUtility.Msg.ErrorBox("Query Sewing faill.\r\n" + result1.ToString());
                            return;
                        }
                        if (!MyUtility.Tool.CursorUpdateTable(SewingData, "SewingSchedule_Detail", null))
                        {
                            MyUtility.Msg.WarningBox("Save fail! Please try again.");
                            return;
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Save fail! Please try again.");
                        return;
                    }
                }
            }
        }

        //To Excel
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable ExcelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "SCIDelivery,BuyerDelivery,ID,BrandID,StyleID,CdCodeID,OrderTypeID,Qty,CPU,SewLine,SewInLine,SewOffLine,CutReadyDate,LETA,ActSewInLine,ActSewOffLine,ArtworkType,SewRemark,MR,SMR", "select * from #tmp", out ExcelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n"+ex.ToString());
                return;
            }

            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            dlg.FileName = "SampleSewingSchedule_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            // Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            // Get the selected file name and CopyToXls
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                bool result = MyUtility.Excel.CopyToXls(ExcelTable, dlg.FileName, xltfile: "PPIC_P13.xltx");
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            else
            {
                return;
            }

        }
    }
}
