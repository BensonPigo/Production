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
using System.Transactions;


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
            base.OnFormLoaded();
            if (MyUtility.GetValue.Lookup(string.Format("select UseAPS from Factory where ID = '{0}'", Sci.Env.User.Factory)) == "True")
            {
                MyUtility.Msg.WarningBox("Please use APS system to assign schedule.");
                Close();
                return;
            }


            cutReadyDate.CellValidating += (s, e) =>
            {

                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(dr["CuttingSP"]) != MyUtility.Convert.GetString(dr["ID"]))
                {
                    MyUtility.Msg.WarningBox("This SP is not cutting sp, no need input Cutting Ready Date.");
                    dr["CutReadyDate"] = DBNull.Value;
                    return;
                }

                if (!MyUtility.Check.Empty(e.FormattedValue) && (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["CutReadyDate"])))
                {
                    if (MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-30))
                    {
                        MyUtility.Msg.WarningBox("Cutting Ready Date can't exceed 30 days before today!!");
                        dr["CutReadyDate"] = DBNull.Value;
                        e.Cancel = true;
                        return;
                    }
                }
            };

            sewInLine.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["SewInLine"])))
                {
                    if (MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-30))
                    {
                        MyUtility.Msg.WarningBox("Inline date can't exceed 30 days before today!!");
                        dr["SewInLine"] = DBNull.Value;
                        e.Cancel = true;
                        return;
                    }
                }
            };

            sewOffLine.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (MyUtility.Check.Empty(dr["SewInLine"]) || (MyUtility.Convert.GetDate(e.FormattedValue) < MyUtility.Convert.GetDate(dr["SewInLine"]))))
                {
                    MyUtility.Msg.WarningBox("Offline date can't less than Inline date!!");
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
                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("Select ID,Description From SewingLine Where FactoryId = '{0}'", Sci.Env.User.Factory), "2,16", MyUtility.Convert.GetString(dr["SewLine"]));
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            };
            sewLine.CellValidating += (s, e) =>
            {

                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(MyUtility.Convert.GetString(e.FormattedValue)))
                {
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", Sci.Env.User.Factory);
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@sewinglineid", MyUtility.Convert.GetString(e.FormattedValue));


                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);

                    DataTable SewLineData;
                    string sqlCmd = "Select ID From SewingLine Where FactoryId = @factoryid and ID = @sewinglineid";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out SewLineData);

                    if (!result || SewLineData.Rows.Count <= 0)
                    {
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Sewing Line : {0} > not found!!!", MyUtility.Convert.GetString(e.FormattedValue)));
                        }
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
            if (textBox3Value!="")
            {
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
                userName = MyUtility.Convert.GetString(dr["Name"]);
                if (!MyUtility.Check.Empty(dr["extNo"]))
                {
                    userName = userName + " #" + MyUtility.Convert.GetString(dr["extNo"]);
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
                            MyUtility.Convert.GetString(dr["SewLine"]),
                            MyUtility.Check.Empty(dr["SewInLine"]) ? "null" : "'" + Convert.ToDateTime(dr["SewInLine"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["SewOffLine"]) ? "null" : "'" + Convert.ToDateTime(dr["SewOffLine"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["CutReadyDate"]) ? "null" : "'" + Convert.ToDateTime(dr["CutReadyDate"]).ToString("d") + "'",
                            MyUtility.Convert.GetString(dr["SewRemark"]), MyUtility.Convert.GetString(dr["ID"])));
                        allSP.Append(string.Format("'{0}',", MyUtility.Convert.GetString(dr["ID"])));
                    }
                }
                if (allSP.Length != 0)
                {
                    DataTable OrderData, SewingData;

                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Id,SewInLine,SewOffLine",
                            string.Format("select o.ID,iif(isnull(o.SewInLine,'1900-01-01') <> isnull(t.SewInLine,'1900-01-01'),'1','') as InlineDiff,iif(isnull(o.SewOffLine,'1900-01-01') <> isnull(t.SewOffLine,'1900-01-01'),'1','') as OfflineDiff,o.SewInLine as OInline,t.SewInLine as TInLine,o.SewOffLine as OOffLine,t.SewOffLine as TOffLine from Orders o, #tmp t where o.ID in ({0}) and o.ID = t.ID", MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1)),
                            out OrderData);
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Query order error.", ex);
                        return;
                    }

                    foreach (DataRow dr in OrderData.Rows)
                    {
                        if (MyUtility.Convert.GetString(dr["InlineDiff"]) == "1")
                        {
                            updateCmds.Add(string.Format(@"insert into Order_History (ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
values ('{0}','SewInLine',{1},{2},'Sewing Inline Update','{3}',GETDATE()) ", MyUtility.Convert.GetString(dr["ID"]),
                            MyUtility.Check.Empty(dr["OInline"]) ? "null" : "'" + Convert.ToDateTime(dr["OInline"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["TInLine"]) ? "null" : "'" + Convert.ToDateTime(dr["TInLine"]).ToString("d") + "'",
                            Sci.Env.User.UserID));
                        }
                        if (MyUtility.Convert.GetString(dr["OfflineDiff"]) == "1")
                        {
                            updateCmds.Add(string.Format(@"insert into Order_History (ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
values ('{0}','SewOffLine',{1},{2},'Sewing Offline Update','{3}',GETDATE()) ", MyUtility.Convert.GetString(dr["ID"]),
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

                    string sqlCmd = string.Format(@"with ttlSewTime
as
(select o.ID, isnull(sum(t.TotalSewingTime),0) as ttlSewTime
 from Orders o, TimeStudy t
 where t.BrandID = o.BrandID
 and t.StyleID = o.StyleID
 and t.SeasonID = o.SeasonID
 and o.ID in ({0})
 group by o.ID
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
where o.ID in ({0})", MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1), Sci.Env.User.UserID);

                    DualResult result1 = DBProxy.Current.Select(null, sqlCmd, out SewingData);

                    if (!result1)
                    {
                        MyUtility.Msg.ErrorBox("Query Sewing faill.\r\n" + result1.ToString());
                        return;
                    }
                    if (MyUtility.Tool.CursorUpdateTable(SewingData, "SewingSchedule", null))
                    {
                        //先刪除已不存在訂單中的資料
                        result1 = DBProxy.Current.Execute(null, string.Format(@"delete from SewingSchedule_Detail where OrderID in ({0})
and not exists (select * from Order_Qty where ID = OrderID and Article = Article and SizeCode = SizeCode)", MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1)));
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
where o.ID in ({0})", MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1)), out SewingData);
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

                setcuttingdate();

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
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            //dlg.FileName = "SampleSewingSchedule_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

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

        private void setcuttingdate()
        {
            string sewdate = DateTime.Now.AddDays(45).ToShortDateString();
            DualResult dresult;

            #region 先刪除不在SewingSchedule 內的Cutting 資料
            string sqlcmd = string.Format(@"Delete Cutting from Cutting join 
            (Select a.id from Cutting a where a.FactoryID = '{1}' and a.Finished = 0 and a.id not in 
            (Select distinct c.cuttingsp from orders c, (SELECT orderid
            FROM Sewingschedule b 
            WHERE Inline <= '{0}' And offline is not null and offline !=''
            AND b.FactoryID = '{1}' group by b.orderid) d where c.id = d.orderid and c.FactoryID = '{1}')) f
            on cutting.id = f.ID", sewdate, Sci.Env.User.Factory);

            DBProxy.Current.DefaultTimeout = 300;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(dresult = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd, dresult);
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
            #endregion

            #region 找出需新增或update 的Cutting
            DataTable cuttingtb;
            string updsql = "";
            sqlcmd = string.Format(@"Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
            from orders ord,
            (Select * from (Select distinct c.cuttingsp from orders c, 
                (SELECT orderid FROM Sewingschedule b 
                WHERE Inline <= '{0}' And offline is not null and offline !=''
               AND b.FactoryID = '{1}' group by b.orderid) d 
            where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 ) e Where e.cuttingsp is not null 
			and e.cuttingsp not in (Select id from cutting)) cut
            where ord.cuttingsp = cut.CuttingSP and ord.FactoryID = '{1}'
          group by ord.CuttingSp order by ord.CuttingSP", sewdate, Sci.Env.User.Factory);
            dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            string sewin, sewof;
            foreach (DataRow dr in cuttingtb.Rows)
            {
                if (dr["inline"] == DBNull.Value) sewin = "";
                else sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
                if (dr["offlinea"] == DBNull.Value) sewof = "";
                else sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();

                updsql = updsql + string.Format("insert into cutting(ID,sewInline,sewoffline,mDivisionid,FactoryID,AddName,AddDate) Values('{0}','{1}','{2}','{3}','{4}',GetDate()); ", dr["cuttingsp"], sewin, sewof, Sci.Env.User.Keyword, Sci.Env.User.Factory, Sci.Env.User.UserID);
            }
            sqlcmd = string.Format(@"Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
            from orders ord,
            (Select * from (Select distinct c.cuttingsp from orders c, 
                (SELECT orderid FROM Sewingschedule b 
                WHERE Inline <= '{0}' And offline is not null and offline !=''
               AND b.FactoryID = '{1}' group by b.orderid) d 
            where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 ) e Where e.cuttingsp is not null 
			and e.cuttingsp in (Select id from cutting)) cut
            where ord.cuttingsp = cut.CuttingSP and ord.FactoryID = '{1}'
          group by ord.CuttingSp order by ord.CuttingSP", sewdate, Sci.Env.User.Factory);
            dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            foreach (DataRow dr in cuttingtb.Rows)
            {
                if (dr["inline"] == DBNull.Value) sewin = "";
                else sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
                if (dr["offlinea"] == DBNull.Value) sewof = "";
                else sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();

                updsql = updsql + string.Format("update cutting set SewInLine ='{0}',sewoffline = '{1}' where id = '{2}'; ", sewin, sewof, dr["cuttingsp"]);
            }
            TransactionScope _transactionscope2 = new TransactionScope();
            using (_transactionscope2)
            {
                try
                {
                    if (!(dresult = DBProxy.Current.Execute(null, updsql)))
                    {
                        _transactionscope2.Dispose();
                        ShowErr(updsql, dresult);
                        return;
                    }
                    _transactionscope2.Complete();
                }
                catch (Exception ex)
                {
                    _transactionscope2.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope2.Dispose();
            _transactionscope2 = null;
            #endregion

            DBProxy.Current.DefaultTimeout = 0;

        }


    }
}
