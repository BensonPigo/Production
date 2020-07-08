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
using System.Transactions;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P13
    /// </summary>
    public partial class P13 : Win.Tems.QueryForm
    {
        private DataTable gridData;
        private DataGridViewGeneratorTextColumnSettings sewLine = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorDateColumnSettings sewInLine = new DataGridViewGeneratorDateColumnSettings();
        private DataGridViewGeneratorDateColumnSettings sewOffLine = new DataGridViewGeneratorDateColumnSettings();
        private DataGridViewGeneratorDateColumnSettings cutReadyDate = new DataGridViewGeneratorDateColumnSettings();

        /// <summary>
        /// P13
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.displayFactory.Value = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (MyUtility.GetValue.Lookup(string.Format("select UseAPS from Factory WITH (NOLOCK) where ID = '{0}'", Sci.Env.User.Factory)) == "True")
            {
                MyUtility.Msg.WarningBox("Please use APS system to assign schedule.");
                this.Close();
                return;
            }

            this.cutReadyDate.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(dr["CuttingSP"]) != MyUtility.Convert.GetString(dr["ID"]))
                {
                    dr["CutReadyDate"] = DBNull.Value;
                    MyUtility.Msg.WarningBox("This SP is not cutting sp, no need input Cutting Ready Date.");
                    return;
                }

                if (!MyUtility.Check.Empty(e.FormattedValue) && (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["CutReadyDate"])))
                {
                    if (MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-30))
                    {
                        dr["CutReadyDate"] = DBNull.Value;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Cutting Ready Date can't exceed 30 days before today!!");
                        return;
                    }
                }
            };

            this.sewInLine.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["SewInLine"])))
                {
                    if (MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-30))
                    {
                        dr["SewInLine"] = DBNull.Value;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Inline date can't exceed 30 days before today!!");
                        return;
                    }
                }
            };

            this.sewOffLine.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (MyUtility.Check.Empty(dr["SewInLine"]) || (MyUtility.Convert.GetDate(e.FormattedValue) < MyUtility.Convert.GetDate(dr["SewInLine"]))))
                {
                    dr["SewOffLine"] = DBNull.Value;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Offline date can't less than Inline date!!");
                    return;
                }
            };

            this.sewLine.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                        Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("Select ID,Description From SewingLine WITH (NOLOCK) Where FactoryId = '{0}'", Sci.Env.User.Factory), "2,16", MyUtility.Convert.GetString(dr["SewLine"]));
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }

                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            };
            this.sewLine.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(MyUtility.Convert.GetString(e.FormattedValue)))
                {
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", Sci.Env.User.Factory);
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@sewinglineid", MyUtility.Convert.GetString(e.FormattedValue));

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);

                    DataTable sewLineData;
                    string sqlCmd = "Select ID From SewingLine WITH (NOLOCK) Where FactoryId = @factoryid and ID = @sewinglineid";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out sewLineData);

                    if (!result || sewLineData.Rows.Count <= 0)
                    {
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Sewing Line : {0} > not found!!!", MyUtility.Convert.GetString(e.FormattedValue)));
                        }

                        dr["SewLine"] = string.Empty;
                        e.Cancel = true;
                        return;
                    }
                }
            };

            // Grid設定
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.grid1)
                .Date("SCIDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CdCodeID", header: "CD#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", iseditingreadonly: true)
                .Numeric("CPU", header: "CPU", decimal_places: 3, iseditingreadonly: true)
                .Text("SewLine", header: "Line#", width: Widths.AnsiChars(2), settings: this.sewLine)
                .Date("SewInLine", header: "In Line Date", settings: this.sewInLine)
                .Date("SewOffLine", header: "Off Line Date", settings: this.sewOffLine)
                .Date("CutReadyDate", header: "Cutting Ready Date", settings: this.cutReadyDate)
                .Date("LETA", header: "LastMTL ETA", iseditingreadonly: true)
                .Date("ActSewInLine", header: "Act. InLine", iseditingreadonly: true)
                .Date("ActSewOffLine", header: "Act. OffLine", iseditingreadonly: true)
                .Text("ArtworkType", header: "Artwork Type", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SewRemark", header: "Remark", width: Widths.AnsiChars(60))
                .Text("MR", header: "MR", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SMR", header: "SMR", width: Widths.AnsiChars(13), iseditingreadonly: true);

            this.grid1.Columns["SewLine"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["SewInLine"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["SewOffLine"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["CutReadyDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["SewRemark"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["SewLine"].DefaultCellStyle.ForeColor = Color.Red;
            this.grid1.Columns["SewInLine"].DefaultCellStyle.ForeColor = Color.Red;
            this.grid1.Columns["SewOffLine"].DefaultCellStyle.ForeColor = Color.Red;
            this.grid1.Columns["CutReadyDate"].DefaultCellStyle.ForeColor = Color.Red;
            this.grid1.Columns["SewRemark"].DefaultCellStyle.ForeColor = Color.Red;
        }

        // SMR
        private void TxtSMR_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSMR.Text = this.PopUpTPEUser(this.txtSMR.Text);
        }

        // MR
        private void TxtMR_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtMR.Text = this.PopUpTPEUser(this.txtMR.Text);
        }

        private string PopUpTPEUser(string userID)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name,ExtNo from TPEPass1 WITH (NOLOCK) ", "10,23,5", userID);
            item.Width = 510;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return string.Empty;
            }

            return item.GetSelectedString();
        }

        // SMR
        private void TxtSMR_Validating(object sender, CancelEventArgs e)
        {
            string textBox3Value = this.txtSMR.Text;
            if (textBox3Value != string.Empty)
            {
                if (this.TPEUserValidating(textBox3Value))
                {
                    this.txtSMR.Text = textBox3Value;
                    this.displaySMR.Value = this.GetUserName(this.txtSMR.Text);
                }
                else
                {
                    this.txtSMR.Text = string.Empty;
                    this.displaySMR.Value = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textBox3Value));
                    return;
                }
            }

            if (textBox3Value == string.Empty)
            {
                this.txtSMR.Text = string.Empty;
                this.displaySMR.Value = string.Empty;
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

        private string GetUserName(string userID)
        {
            string selectSql = string.Format("Select Name,ExtNo from TPEPass1 WITH (NOLOCK) Where id='{0}'", userID);
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
                return string.Empty;
            }
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPStart.Text) && MyUtility.Check.Empty(this.txtSPStart.Text) && MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                MyUtility.Msg.WarningBox("SP#  and SCI Delivery can't empty!!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"with tmpData
as
(select o.ID,o.StyleID,o.BrandID,o.SciDelivery,o.BuyerDelivery,o.CdCodeID,
 o.OrderTypeID,o.Qty,(o.CPU*o.CPUFactor) as CPU,o.SewLine,o.SewInLine,o.SewOffLine,o.CutReadyDate,
 o.LETA,o.SewRemark,isnull((o.MRHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = o.MRHandle)),o.MRHandle) as MR,
 isnull((o.SMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = o.SMR)),o.SMR) as SMR,
 (select min(s.OutputDate) from SewingOutput s WITH (NOLOCK) , SewingOutput_Detail sd WITH (NOLOCK) where sd.OrderId = o.ID and sd.ID = s.ID and sd.QAQty > 0) as ActSewInLine,
 (select max(s.OutputDate) from SewingOutput s WITH (NOLOCK) , SewingOutput_Detail sd WITH (NOLOCK) where sd.OrderId = o.ID and sd.ID = s.ID and sd.QAQty > 0) as tmpActSewOffLine,
 (select min(a.QAQty) from (select ComboType,sum(QAQty) as QAQty from SewingOutput_Detail WITH (NOLOCK) where OrderId = o.ID group by ComboType) a) as SewOutQty,
 (select '['+CAST(a.Abbreviation as nvarchar)+'],' from (
 select distinct at.Abbreviation from Order_Artwork oa WITH (NOLOCK) ,ArtworkType at WITH (NOLOCK) 
 where at.IsSubprocess = 1
 and oa.ID = o.ID
 and oa.ArtworkTypeID = at.ID) a
 for xml path('')) as tmpArtworkType,o.CuttingSP
 from Orders o WITH (NOLOCK) 
 where o.Finished = 0 and o.category in ('B','S')
 and o.IsForecast = 0
 and o.FtyGroup = '{0}'", Sci.Env.User.Factory));
            if (!MyUtility.Check.Empty(this.txtSPStart.Text))
            {
                sqlCmd.Append(string.Format(" and o.ID >= '{0}'", this.txtSPStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPEnd.Text))
            {
                sqlCmd.Append(string.Format(" and o.ID <= '{0}'", this.txtSPEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.txtstyle.Text))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", this.txtstyle.Text));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.txtbrand.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSMR.Text))
            {
                sqlCmd.Append(string.Format(" and o.SMR = '{0}'", this.txtSMR.Text));
            }

            if (!MyUtility.Check.Empty(this.txtMR.Text))
            {
                sqlCmd.Append(string.Format(" and o.MRHandle = '{0}'", this.txtMR.Text));
            }

            sqlCmd.Append(@")
select *,iif(isnull(SewOutQty,0) >= Qty, tmpActSewOffLine, null) as ActSewOffLine,SUBSTRING(tmpArtworkType,1, LEN(tmpArtworkType)-1) as ArtworkType from tmpData order by SCIDelivery");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty((DataTable)this.listControlBindingSource1.DataSource))
            {
                IList<string> updateCmds = new List<string>();
                this.grid1.ValidateControl();
                this.listControlBindingSource1.EndEdit();
                StringBuilder allSP = new StringBuilder();
                foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        updateCmds.Add(string.Format(
                            @"update Orders set SewLine = '{0}', SewInLine = {1}, SewOffLine = {2}, CutReadyDate = {3}, SewRemark = '{4}' where ID = '{5}'",
                            MyUtility.Convert.GetString(dr["SewLine"]),
                            MyUtility.Check.Empty(dr["SewInLine"]) ? "null" : "'" + Convert.ToDateTime(dr["SewInLine"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["SewOffLine"]) ? "null" : "'" + Convert.ToDateTime(dr["SewOffLine"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["CutReadyDate"]) ? "null" : "'" + Convert.ToDateTime(dr["CutReadyDate"]).ToString("d") + "'",
                            MyUtility.Convert.GetString(dr["SewRemark"]),
                            MyUtility.Convert.GetString(dr["ID"])));
                        allSP.Append(string.Format("'{0}',", MyUtility.Convert.GetString(dr["ID"])));
                    }
                }

                if (allSP.Length != 0)
                {
                    DataTable orderData, sewingData;

                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable(
                            (DataTable)this.listControlBindingSource1.DataSource,
                            "Id,SewInLine,SewOffLine",
                            string.Format("select o.ID,iif(isnull(o.SewInLine,'1900-01-01') <> isnull(t.SewInLine,'1900-01-01'),'1','') as InlineDiff,iif(isnull(o.SewOffLine,'1900-01-01') <> isnull(t.SewOffLine,'1900-01-01'),'1','') as OfflineDiff,o.SewInLine as OInline,t.SewInLine as TInLine,o.SewOffLine as OOffLine,t.SewOffLine as TOffLine from Orders o WITH (NOLOCK) , #tmp t where o.ID in ({0}) and o.ID = t.ID", MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1)),
                            out orderData);
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr("Query order error.", ex);
                        return;
                    }

                    foreach (DataRow dr in orderData.Rows)
                    {
                        if (MyUtility.Convert.GetString(dr["InlineDiff"]) == "1")
                        {
                            updateCmds.Add(string.Format(
                                @"insert into Order_History (ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
values ('{0}','SewInOffLine',{1},{2},'Sewing Inline Update','{3}',GETDATE()) ", MyUtility.Convert.GetString(dr["ID"]),
                                MyUtility.Check.Empty(dr["OInline"]) ? "null" : "'" + Convert.ToDateTime(dr["OInline"]).ToString("d") + "'",
                                MyUtility.Check.Empty(dr["TInLine"]) ? "null" : "'" + Convert.ToDateTime(dr["TInLine"]).ToString("d") + "'",
                                Sci.Env.User.UserID));
                        }

                        if (MyUtility.Convert.GetString(dr["OfflineDiff"]) == "1")
                        {
                            updateCmds.Add(string.Format(
                                @"insert into Order_History (ID,HisType,OldValue,NewValue,Remark,AddName,AddDate)
values ('{0}','SewInOffLine',{1},{2},'Sewing Offline Update','{3}',GETDATE()) ", MyUtility.Convert.GetString(dr["ID"]),
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

                    string sqlCmd = string.Format(
                        @"
with ttlSewTime
as
(select o.ID, isnull(sum(t.TotalSewingTime),0) as ttlSewTime
 from Orders o WITH (NOLOCK) , TimeStudy t WITH (NOLOCK) 
 where t.BrandID = o.BrandID
 and t.StyleID = o.StyleID
 and t.SeasonID = o.SeasonID
 and o.ID in ({0})
 group by o.ID
)

select isnull(s.ID,0) as ID, o.ID as OrderID, ComboType = ''
    , SUBSTRING(o.SewLine,1,2) as SewingLineID,o.Qty as AlloQty,
    o.SewInLine as Inline, o.SewOffLine as Offline,
    o.MDivisionID,
    o.FtyGroup as FactoryID,sl.Sewer,st.ttlSewTime as TotalSewingTime,
    100 as MaxEff,iif(st.ttlSewTime*sl.Sewer = 0,0,(3600.0/st.ttlSewTime*sl.Sewer)) as StandardOutput,
    (select count(SewingLineID) from WorkHour WITH (NOLOCK) where Hours > 0 and FactoryID = o.FtyGroup and Date between o.SewInLine and o.SewOffLine and SewingLineID = o.SewLine) as WorkDay,
    iif((3600.0/st.ttlSewTime*sl.Sewer) = 0,0,floor(o.Qty/(3600.0/st.ttlSewTime*sl.Sewer))) as WorkHour,0 as APSNo, o.Finished as OrderFinished,'{1}'as AddName,GETDATE() as AddDate,EditName = iif(s.id is null, '', '{1}'),EditDate =iif(s.id is null, null, '{2}')
from Orders o WITH (NOLOCK) 
left join SewingSchedule s WITH (NOLOCK) on s.OrderID = o.ID
left join SewingLine sl WITH (NOLOCK) on sl.ID = o.SewLine and sl.FactoryID = o.FtyGroup
left join ttlSewTime st on st.ID = o.ID
where o.ID in ({0})",
                        MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1),
                        Env.User.UserID,
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    DualResult result1 = DBProxy.Current.Select(null, sqlCmd, out sewingData);

                    if (!result1)
                    {
                        MyUtility.Msg.ErrorBox("Query Sewing faill.\r\n" + result1.ToString());
                        return;
                    }

                    if (MyUtility.Tool.CursorUpdateTable(sewingData, "SewingSchedule", null))
                    {
                        // 先刪除已不存在訂單中的資料
                        result1 = DBProxy.Current.Execute(null, string.Format(
                            @"delete from SewingSchedule_Detail where OrderID in ({0})
and not exists (select * from Order_Qty where ID = OrderID and Article = Article and SizeCode = SizeCode)", MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1)));
                        if (!result1)
                        {
                            MyUtility.Msg.ErrorBox("Delete Sewing faill.\r\n" + result1.ToString());
                            return;
                        }

                        // 新增 & 更新資料
                        result1 = DBProxy.Current.Select(
                            null,
                            string.Format(
                            @"
select isnull(s.ID,0) as ID,o.ID as OrderID, ComboType = '',
o.SewLine as SewingLineID,oq.Article,oq.SizeCode,oq.Qty as AlloQty
from Orders o WITH (NOLOCK) 
left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
left join SewingSchedule s WITH (NOLOCK) on oq.ID = s.OrderID
where o.ID in ({0})",
                            MyUtility.Convert.GetString(allSP).Substring(0, MyUtility.Convert.GetString(allSP).Length - 1)),
                            out sewingData);
                        if (!result1)
                        {
                            MyUtility.Msg.ErrorBox("Query Sewing faill.\r\n" + result1.ToString());
                            return;
                        }

                        if (!MyUtility.Tool.CursorUpdateTable(sewingData, "SewingSchedule_Detail", null))
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

                this.Setcuttingdate();
            }
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable excelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSource1.DataSource, "SCIDelivery,BuyerDelivery,ID,BrandID,StyleID,CdCodeID,OrderTypeID,Qty,CPU,SewLine,SewInLine,SewOffLine,CutReadyDate,LETA,ActSewInLine,ActSewOffLine,ArtworkType,SewRemark,MR,SMR", "select * from #tmp", out excelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_P13.xltx");
            MyUtility.Excel.CopyToXls(excelTable, string.Empty, "PPIC_P13.xltx", 1, true, string.Empty, objApp);
        }

        private void Setcuttingdate()
        {
            string sewdate = DateTime.Now.AddDays(45).ToShortDateString();
            DualResult dresult;

            #region 先刪除不在SewingSchedule 內的Cutting 資料
            string sqlcmd = string.Format(
                @"
Delete Cutting 
from Cutting 
join (Select a.id from Cutting a where a.FactoryID = '{1}' and a.Finished = 0 and a.id not in 
(Select distinct c.cuttingsp from orders c, (SELECT orderid
FROM Sewingschedule b 
WHERE Inline <= '{0}' And offline is not null and offline !=''
AND b.FactoryID = '{1}' group by b.orderid) d where c.id = d.orderid and c.FactoryID = '{1}')) f
on cutting.id = f.ID",
                sewdate,
                Env.User.Factory);

            DBProxy.Current.DefaultTimeout = 300;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(dresult = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlcmd, dresult);
                        return;
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
            #endregion
            #region 找出需新增或update 的Cutting
            DataTable cuttingtb;
            string updsql = string.Empty;
            sqlcmd = string.Format(
                @"
SELECT ord.cuttingsp, 
       Min(isnull(ord.sewinline,''))  AS inline, 
       Max(isnull(ord.sewoffline,'')) AS offlinea 
FROM   orders ord WITH (nolock), 
       (SELECT * 
        FROM   (SELECT DISTINCT c.cuttingsp 
                FROM   orders c WITH (nolock), 
                       (SELECT orderid 
                        FROM   sewingschedule b WITH (nolock) 
                        WHERE  inline <= '{0}' 
                               AND offline IS NOT NULL 
                               AND offline != '' 
                               AND b.factoryid = '{1}' 
                        GROUP  BY b.orderid) d 
                WHERE  c.id = d.orderid 
                       AND c.isforecast = 0 
                       AND c.localorder = 0) e 
        WHERE  e.cuttingsp IS NOT NULL 
               AND e.cuttingsp NOT IN (SELECT id 
                                       FROM   cutting WITH (nolock))) cut 
WHERE  ord.cuttingsp = cut.cuttingsp 
       AND ord.FtyGroup = '{1}' 
GROUP  BY ord.cuttingsp 
ORDER  BY ord.cuttingsp ",
                sewdate,
                Env.User.Factory);
            dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            string sewin, sewof;
            foreach (DataRow dr in cuttingtb.Rows)
            {
                if (dr["inline"] == DBNull.Value)
                {
                    sewin = string.Empty;
                }
                else
                {
                    sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
                }

                if (dr["offlinea"] == DBNull.Value)
                {
                    sewof = string.Empty;
                }
                else
                {
                    sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();
                }

                updsql = updsql + string.Format("insert into cutting(ID,sewInline,sewoffline,mDivisionid,FactoryID,AddName,AddDate) Values('{0}','{1}','{2}','{3}','{4}','{5}',GetDate()); ", dr["cuttingsp"].ToString(), sewin, sewof, Sci.Env.User.Keyword, Sci.Env.User.Factory, Sci.Env.User.UserID);
            }

            if (!MyUtility.Check.Empty(updsql))
            {
                dresult = DBProxy.Current.Execute(null, updsql);
                if (!dresult)
                {
                    this.ShowErr(updsql, dresult);
                    return;
                }
            }

            string sqlupdate = string.Format(
                @"
SELECT ord.cuttingsp, 
       Min(isnull(ord.sewinline,''))  AS inline, 
       Max(isnull(ord.sewoffline,'')) AS offlinea 
FROM   orders ord WITH (nolock), 
       (SELECT * 
        FROM   (SELECT DISTINCT c.cuttingsp 
                FROM   orders c WITH (nolock), 
                       (SELECT orderid 
                        FROM   sewingschedule b WITH (nolock) 
                        WHERE  inline <= '{0}' 
                               AND offline IS NOT NULL 
                               AND offline != '' 
                               AND b.factoryid = '{1}' 
                        GROUP  BY b.orderid) d 
                WHERE  c.id = d.orderid 
                       AND c.isforecast = 0 
                       AND c.localorder = 0) e 
        WHERE  e.cuttingsp IS NOT NULL 
               AND e.cuttingsp IN (SELECT id 
                                   FROM   cutting WITH (nolock))) cut 
WHERE  ord.cuttingsp = cut.cuttingsp 
       AND ord.factoryid = '{1}' 
GROUP  BY ord.cuttingsp 
ORDER  BY ord.cuttingsp 

MERGE production.dbo.cutting AS t 
using #updatetemp AS s 
ON t.id = s.cuttingsp 
WHEN matched THEN 
  UPDATE SET t.sewinline = s.inline, 
             t.sewoffline = s.offlinea; 

DROP TABLE #updatetemp  ",
                sewdate,
                Env.User.Factory);
            dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            if (!dresult)
            {
                 this.ShowErr(updsql, dresult);
                 return;
            }
            #endregion
            MyUtility.Msg.InfoBox("Update is Completed!");

            DBProxy.Current.DefaultTimeout = 0;
        }

        // MR
        private void TxtMR_Validating(object sender, CancelEventArgs e)
        {
            string textBox4Value = this.txtMR.Text;
            if (textBox4Value != string.Empty)
            {
                if (this.TPEUserValidating(textBox4Value))
                {
                    this.txtMR.Text = textBox4Value;
                    this.displayMR.Value = this.GetUserName(this.txtMR.Text);
                }
                else
                {
                    this.txtMR.Text = string.Empty;
                    this.displayMR.Value = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< User Id: {0} > not found!!!", textBox4Value));
                    return;
                }
            }

            if (textBox4Value == string.Empty)
            {
                this.txtMR.Text = string.Empty;
                this.displayMR.Value = string.Empty;
            }
        }
    }
}
