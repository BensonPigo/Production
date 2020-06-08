using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Win.Tools;
using Sci.Win;
using System.Reflection;
using System.Linq;
using System.Drawing;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02
    /// </summary>
    public partial class P02 : Sci.Win.Tems.Input2
    {
        // 宣告Context Menu Item
        private ToolStripMenuItem bulkpl;
        private ToolStripMenuItem samplepl;
        private ToolStripMenuItem focpl;
        private ToolStripMenuItem purchase;
        private ToolStripMenuItem poitem;
        private ToolStripMenuItem newitem;
        private ToolStripMenuItem edit;
        private ToolStripMenuItem delete;
        private ToolStripMenuItem print;
        private ToolStripMenuItem batchprint;

        private string carrierID = string.Empty;

        /// <summary>
        /// CarrierID
        /// </summary>
        protected string CarrierID
        {
            get
            {
                return this.carrierID;
            }

            set
            {
                this.carrierID = value;
            }
        }

        /// <summary>
        /// P02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboFrom, 2, 1, "1,Factory,2,Brand");
            MyUtility.Tool.SetupCombox(this.comboTO, 2, 1, "1,SCI,2,Factory,3,Supplier,4,Brand");
            MyUtility.Tool.SetupCombox(this.cmbPayer, 2, 1, "3RD,Freight Collect by 3rd Party,CUST,Freight Collect by Customer,FTY,Freight Pre-Paid by Fty,HAND,Hand Carry");

            Sci.Win.UI.Button btnSendingSchedule = new Win.UI.Button();
            Point btnSendingScheduleloc = new Point(720, this.lbl_queryfor.Location.Y);
            btnSendingSchedule.Text = "FTY Regular Sending Schedule";
            btnSendingSchedule.Location = btnSendingScheduleloc;
            btnSendingSchedule.Size = new Size(250, 30);
            btnSendingSchedule.Click += (s, e) =>
            {
                using (var sendingSchedule = new P02_FTYRegularSendingSchedule())
                {
                    sendingSchedule.ShowDialog();
                }
            };
            this.browsetop.Controls.Add(btnSendingSchedule);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.detailgrid.CellToolTipTextNeeded += (s, e) =>
                {
                    e.ToolTipText = "You can show the function form to press the right key under inquiring the state.";
                };

            this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import Bulk PL#", onclick: (s, e) => this.ImportBulkPL()).Get(out this.bulkpl);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import from Sample PL#", onclick: (s, e) => this.ImportFromSamplePL()).Get(out this.samplepl);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import from FOC PL# (Garment FOC)", onclick: (s, e) => this.ImportFromFOCPL()).Get(out this.focpl);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Add by PO# item (Garment Chargeable)", onclick: (s, e) => this.AddByPOItem()).Get(out this.poitem);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import from Purchase (Material)", onclick: (s, e) => this.ImportFromPurchase()).Get(out this.purchase);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Add new Item", onclick: (s, e) => this.AddNewItem()).Get(out this.newitem);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Separator();
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record - ", onclick: (s, e) => this.MenuDelete()).Get(out this.delete);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail - ", onclick: (s, e) => this.MenuEdit()).Get(out this.edit);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Separator();
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Print", onclick: (s, e) => this.Print(new DataRow[] { this.CurrentDetailData }, false)).Get(out this.print);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Batch Print", onclick: (s, e) => this.BatchPrint()).Get(out this.batchprint);
            this.SetContextMenuStatus(false); // 預設先將Context ment設定為disable
        }

        // 設定Context Menu的Enable/Disable
        private void SetContextMenuStatus(bool status)
        {
            this.bulkpl.Enabled = status;
            this.samplepl.Enabled = status;
            this.focpl.Enabled = status;
            this.purchase.Enabled = status;
            this.poitem.Enabled = status;
            this.newitem.Enabled = status;
            this.delete.Enabled = status;
            this.edit.Enabled = status;
            this.print.Enabled = status;
            this.batchprint.Enabled = status;
        }

        // 比對detail table差異
        private void CompareDetailPrint(DataTable after_dt, DataTable before_dt)
        {
            DataTable result_dt;
            if (before_dt.Rows.Count > 0)
            {
                var idsNotInB = after_dt.AsEnumerable().Select(r => r.Field<string>("ID") + r.Field<string>("OrderID") + r.Field<string>("Seq1") + r.Field<string>("Seq2") + r.Field<string>("Category"))
              .Except(before_dt.AsEnumerable().Select(r => r.Field<string>("ID") + r.Field<string>("OrderID") + r.Field<string>("Seq1") + r.Field<string>("Seq2") + r.Field<string>("Category"))).ToList();
                if (idsNotInB.Count == 0)
                {
                    return;
                }

                result_dt = (from r in after_dt.AsEnumerable()
                                 join id in idsNotInB
                                 on r.Field<string>("ID") + r.Field<string>("OrderID") + r.Field<string>("Seq1") + r.Field<string>("Seq2") + r.Field<string>("Category") equals id
                                 select r).CopyToDataTable();
            }
            else
            {
                result_dt = after_dt;
            }

            if (result_dt.Rows.Count > 0)
            {
                this.Print(result_dt.Select(), true);
            }
        }

        // Context Menu選擇Import from FOC PL# (Garment FOC)
        private void ImportFromFOCPL()
        {
            Sci.Production.Shipping.P02_ImportFromFOCPackingList callFOCPLForm = new Sci.Production.Shipping.P02_ImportFromFOCPackingList(this.CurrentMaintain);
            DataTable before_dt = ((DataTable)this.detailgridbs.DataSource).Copy();
            callFOCPLForm.ShowDialog(this);
            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);

            this.CompareDetailPrint((DataTable)this.detailgridbs.DataSource, before_dt);
        }

        // Context Menu選擇Import Bulk PL#
        private void ImportBulkPL()
        {
            Sci.Production.Shipping.P02_ImportFromBulkPackingList callFOCPLForm = new Sci.Production.Shipping.P02_ImportFromBulkPackingList(this.CurrentMaintain);
            DataTable before_dt = ((DataTable)this.detailgridbs.DataSource).Copy();
            callFOCPLForm.ShowDialog(this);
            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);

            this.CompareDetailPrint((DataTable)this.detailgridbs.DataSource, before_dt);
        }

        // Context Menu選擇Import from FOC PL# (Garment FOC)
        private void ImportFromSamplePL()
        {
            Sci.Production.Shipping.P02_ImportFromSamplePackingList callFOCPLForm = new Sci.Production.Shipping.P02_ImportFromSamplePackingList(this.CurrentMaintain);
            DataTable before_dt = ((DataTable)this.detailgridbs.DataSource).Copy();
            callFOCPLForm.ShowDialog(this);
            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);

            this.CompareDetailPrint((DataTable)this.detailgridbs.DataSource, before_dt);
        }

        // Context Menu選擇Import from purchase (Material)
        private void ImportFromPurchase()
        {
            Sci.Production.Shipping.P02_ImportFromPO callPurchaseForm = new Sci.Production.Shipping.P02_ImportFromPO(this.CurrentMaintain);
            DataTable before_dt = ((DataTable)this.detailgridbs.DataSource).Copy();
            callPurchaseForm.ShowDialog(this);
            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);
            this.CompareDetailPrint((DataTable)this.detailgridbs.DataSource, before_dt);
        }

        // Context Menu選擇Add by PO# item (Garment Chargeable)
        private void AddByPOItem()
        {
            Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
            DataRow dr = ((DataTable)this.detailgridbs.DataSource).NewRow();
            dr["ID"] = this.CurrentMaintain["ID"];
            DataTable before_dt = ((DataTable)this.detailgridbs.DataSource).Copy();
            callPOItemForm.SetInsert(dr);
            callPOItemForm.ShowDialog(this);

            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);
            this.CompareDetailPrint((DataTable)this.detailgridbs.DataSource, before_dt);
        }

        // Context Menu選擇Add new Item
        private void AddNewItem()
        {
            Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
            DataRow dr = ((DataTable)this.detailgridbs.DataSource).NewRow();
            dr["ID"] = this.CurrentMaintain["ID"];
            DataTable before_dt = ((DataTable)this.detailgridbs.DataSource).Copy();
            callNewItemForm.SetInsert(dr);
            callNewItemForm.ShowDialog(this);
            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);
            this.CompareDetailPrint((DataTable)this.detailgridbs.DataSource, before_dt);
        }

        // Context Menu選擇Edit this Record's detail
        private void MenuEdit()
        {
            string before_orderid = this.CurrentDetailData["OrderId"].ToString();
            string before_seq1 = this.CurrentDetailData["Seq1"].ToString();
            string before_seq2 = this.CurrentDetailData["Seq2"].ToString();
            string before_category = this.CurrentDetailData["Category"].ToString();
            DialogResult edit_result = DialogResult.No;
            if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "1" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "2" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "3")
            {
                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "1")
                {
                    callPOItemForm.Text = "International Air/Express - Import from FOC PL#";
                }

                callPOItemForm.SetUpdate(this.CurrentDetailData);
                edit_result = callPOItemForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "4")
            {
                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                callEditPOForm.SetUpdate(this.CurrentDetailData);
                edit_result = callEditPOForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "5" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "6" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "7" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "8" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "9")
            {
                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                callNewItemForm.SetUpdate(this.CurrentDetailData);
                edit_result = callNewItemForm.ShowDialog(this);
            }
            #region 重新計算ttl Carton Weight
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string sqlCmd = string.Format(
                @"
with NewCtn
as
(
select distinct ed.ID,ed.CTNNo,'N' as Status 
from Express_Detail ed WITH (NOLOCK) 
where ed.ID = '{0}'
and not exists (select 1 from Express_CTNData ec WITH (NOLOCK) where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
),
DeleteCtn
as
(
select distinct ec.ID,ec.CTNNo,'D' as Status 
from Express_CTNData ec WITH (NOLOCK) 
where ec.ID = '{0}'
and not exists (select 1 from Express_Detail ed WITH (NOLOCK) where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
)
select * from NewCtn
union
select * from DeleteCtn", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable cTNData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out cTNData);
            if (!result)
            {
                return;
            }

            if (cTNData.Rows.Count > 0)
            {
                IList<string> updateCmds = new List<string>();
                foreach (DataRow dr in cTNData.Rows)
                {
                    if (MyUtility.Convert.GetString(dr["Status"]) == "N")
                    {
                        updateCmds.Add(string.Format("insert into Express_CTNData(ID,CTNNo,AddName, AddDate) values ('{0}','{1}','{2}',GETDATE());", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["CTNNo"]), Sci.Env.User.UserID));
                    }
                    else
                    {
                        updateCmds.Add(string.Format("delete from Express_CTNData where ID = '{0}' and CTNNo = '{1}';", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["CTNNo"])));
                    }
                }

                result = DBProxy.Current.Executes(null, updateCmds);

                if (!result)
                {
                    return;
                }
            }

            string sqlcmd = string.Format(
@"update Express set NW = (select SUM(NW) from Express_Detail where ID = '{0}'),
CTNQty = (select COUNT(distinct CTNNo) from Express_Detail where ID = '{0}'),
CTNNW = (SELECT SUM(CTNNW) FROM Express_CTNData WHERE ID='{0}' ),
VW = (select sum(vw) from (
select CTNNo,(CtnLength*CtnWidth*CtnHeight)/6000 as vw from Express_CTNData
where id='{0}') a)
where ID = '{0}'", this.CurrentMaintain["ID"]);

            DBProxy.Current.Execute(null, sqlcmd);
            #endregion
            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);

            DataRow[] after_row = ((DataTable)this.detailgridbs.DataSource).Select(string.Format(" OrderId = '{0}' and Seq1 = '{1}' and Seq2 = '{2}' and Category = '{3}' ", before_orderid, before_seq1, before_seq2, before_category));
            if (after_row.Length == 0)
            {
                return;
            }

            if (edit_result == DialogResult.OK)
            {
                this.Print(after_row, true);
            }
        }

        // Context Menu選擇Delete this Record
        private void MenuDelete()
        {
            if (!this.CheckPullexists())
            {
                return;
            }

            if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "1" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "2" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "3")
            {
                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "1")
                {
                    callPOItemForm.Text = "International Air/Express - Import From FOC PL#";
                }

                callPOItemForm.SetDelete(this.CurrentDetailData);
                callPOItemForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "4")
            {
                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                callEditPOForm.SetDelete(this.CurrentDetailData);
                callEditPOForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "5" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "6" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "7" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "8" || MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) == "9")
            {
                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                callNewItemForm.SetDelete(this.CurrentDetailData);
                callNewItemForm.ShowDialog(this);
            }
            #region 重新計算vm 體積
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string sqlCmd = string.Format(
                @"
with NewCtn
as
(
select distinct ed.ID,ed.CTNNo,'N' as Status 
from Express_Detail ed WITH (NOLOCK) 
where ed.ID = '{0}'
and not exists (select 1 from Express_CTNData ec WITH (NOLOCK) where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
),
DeleteCtn
as
(
select distinct ec.ID,ec.CTNNo,'D' as Status 
from Express_CTNData ec WITH (NOLOCK) 
where ec.ID = '{0}'
and not exists (select 1 from Express_Detail ed WITH (NOLOCK) where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
)
select * from NewCtn
union
select * from DeleteCtn", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable cTNData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out cTNData);
            if (!result)
            {
                return;
            }

            if (cTNData.Rows.Count > 0)
            {
                IList<string> updateCmds = new List<string>();
                foreach (DataRow dr in cTNData.Rows)
                {
                    if (MyUtility.Convert.GetString(dr["Status"]) == "N")
                    {
                        updateCmds.Add(string.Format("insert into Express_CTNData(ID,CTNNo,AddName, AddDate) values ('{0}','{1}','{2}',GETDATE());", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["CTNNo"]), Sci.Env.User.UserID));
                    }
                    else
                    {
                        updateCmds.Add(string.Format("delete from Express_CTNData where ID = '{0}' and CTNNo = '{1}';", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["CTNNo"])));
                    }
                }

                result = DBProxy.Current.Executes(null, updateCmds);

                if (!result)
                {
                    return;
                }
            }

            string sqlcmd = string.Format(
                @"update Express 
set VW= (select isnull(sum((CtnLength*CtnWidth*CtnHeight)/6000),0) as VW from Express_CTNData where id='{0}'),
CTNNW = (select isnull(sum(CTNNW),0) as CTNNW from Express_CTNData where id='{0}')
where id='{0}' ", this.CurrentMaintain["ID"]);
            DBProxy.Current.Execute(null, sqlcmd);
            #endregion

            this.RenewData();
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);
        }

        // Context Menu選擇Print
        private void Print(DataRow[] drlist, bool print_flag)
        {
            DualResult result;
            IReportResource reportresource;
            ReportDefinition rd = new ReportDefinition();
            if (!(result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(this.GetType()), this.GetType(), "P02_DetailPrint.rdlc", out reportresource)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
            else
            {
                rd.ReportResource = reportresource;

                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("barCode", '*' + MyUtility.Convert.GetString(this.CurrentDetailData["ID"]) + MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]) + MyUtility.Convert.GetString(this.CurrentDetailData["Seq1"]) + MyUtility.Convert.GetString(this.CurrentDetailData["Seq2"]) + MyUtility.Convert.GetString(this.CurrentDetailData["Category"]) + '*'));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("from", (MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "1" ? "Factory" : "Brand") + "(" + MyUtility.Convert.GetString(this.CurrentMaintain["FromSite"]) + ")"));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("to", (MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "1" ? "SCI" : MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "2" ? "Factory" : MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "3" ? "Supplier" : "Brand") + "(" + MyUtility.Convert.GetString(this.CurrentMaintain["ToSite"]) + ")"));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("hcNo", MyUtility.Convert.GetString(this.CurrentDetailData["ID"])));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("serialNo", MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]) + "-" + MyUtility.Convert.GetString(this.CurrentDetailData["Seq1"]) + "-" + MyUtility.Convert.GetString(this.CurrentDetailData["Seq2"])));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("brand", MyUtility.Convert.GetString(this.CurrentDetailData["BrandID"])));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("incharge", MyUtility.GetValue.Lookup(string.Format("select Name+ iif(ExtNo <> '',' #' + ExtNo,'') as Incharge from Pass1 where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentDetailData["InCharge"])))));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("teamLeader", MyUtility.Convert.GetString(this.CurrentDetailData["LeaderName"])));
                // rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("receiver", MyUtility.Convert.GetString(this.CurrentDetailData["ReceiverName"])));
                List<P02_PrintData> data = new List<P02_PrintData>();
                P02_PrintData data_item;
                foreach (DataRow dr in drlist)
                {
                    data_item = new P02_PrintData();
                    data_item.BarCode = '*' + MyUtility.Convert.GetString(dr["ID"]) + MyUtility.Convert.GetString(dr["OrderID"]) + MyUtility.Convert.GetString(dr["Seq1"]) + MyUtility.Convert.GetString(dr["Seq2"]) + MyUtility.Convert.GetString(dr["Category"]) + '*';
                    data_item.From = (MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "1" ? "Factory" : "Brand") + "(" + MyUtility.Convert.GetString(this.CurrentMaintain["FromSite"]) + ")";
                    data_item.To = (MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "1" ? "SCI" : MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "2" ? "Factory" : MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "3" ? "Supplier" : "Brand") + "(" + MyUtility.Convert.GetString(this.CurrentMaintain["ToSite"]) + ")";
                    data_item.HcNo = MyUtility.Convert.GetString(dr["ID"]);
                    data_item.SerialNo = MyUtility.Convert.GetString(dr["OrderID"]) + "-" + MyUtility.Convert.GetString(dr["Seq1"]) + "-" + MyUtility.Convert.GetString(dr["Seq2"]);
                    data_item.Brand = MyUtility.Convert.GetString(dr["BrandID"]);
                    data_item.Incharge = MyUtility.GetValue.Lookup(string.Format("select Name+ iif(ExtNo <> '',' #' + ExtNo,'') as Incharge from Pass1 where ID = '{0}'", MyUtility.Convert.GetString(dr["InCharge"])));
                    data_item.TeamLeader = MyUtility.Convert.GetString(dr["LeaderName"]);
                    data_item.Receiver = MyUtility.Convert.GetString(dr["ReceiverName"]);
                    data.Add(data_item);
                }

                rd.ReportDataSource = data;

                using (var frm = new Sci.Win.Subs.ReportView(rd))
                {
                    frm.DirectPrint = print_flag;
                    frm.ShowDialog(this);
                }
            }
        }

        // Context Menu選擇Batch Print
        private void BatchPrint()
        {
            Sci.Production.Shipping.P02_BatchPrint callPurchaseForm = new Sci.Production.Shipping.P02_BatchPrint(this.CurrentMaintain);
            callPurchaseForm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayFrom.Value = MyUtility.GetValue.Lookup("NameEN", MyUtility.Convert.GetString(this.CurrentMaintain["FromSite"]), "Brand", "ID");
            this.displayTO.Value = string.Empty;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "3")
            {
                this.displayTO.Value = MyUtility.GetValue.Lookup("AbbEN", MyUtility.Convert.GetString(this.CurrentMaintain["ToSite"]), "Supp", "ID");
            }
            else
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "4")
                {
                    this.displayTO.Value = MyUtility.GetValue.Lookup("NameEN", MyUtility.Convert.GetString(this.CurrentMaintain["ToSite"]), "Brand", "ID");
                }
            }

            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);
            this.displayCarrier.Value = MyUtility.GetValue.Lookup(string.Format("select c.SuppID + '-' + s.AbbEN from Carrier c WITH (NOLOCK) left join Supp s WITH (NOLOCK) on c.SuppID = s.ID where c.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["CarrierID"])));
            if (MyUtility.Check.Empty(this.CurrentMaintain["StatusUpdateDate"]))
            {
                this.displayStatupdate.Value = null;
            }
            else
            {
                this.displayStatupdate.Value = Convert.ToDateTime(this.CurrentMaintain["StatusUpdateDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SendDate"]))
            {
                this.displaySendtoSCI.Value = null;
            }
            else
            {
                this.displaySendtoSCI.Value = Convert.ToDateTime(this.CurrentMaintain["SendDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }

            this.btnMailto.Enabled = !this.EditMode && (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Sent" || MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved") && (PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Manager"])));

            if (this.CurrentDetailData == null)
            {
                // 如果狀態是Junk的話，Context Menu要Disable
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Junked")
                {
                    this.SetContextMenuStatus(false);
                }
                else
                {
                    // 先將Menu狀態全打開
                    this.SetContextMenuStatus(true);

                    this.delete.Enabled = false;
                    this.edit.Enabled = false;
                    this.print.Enabled = false;
                    this.batchprint.Enabled = false;
                }
            }

            string sqlsupp = $@"select Abb from LocalSupp with(nolock) where Junk = 0 and id ='{this.CurrentMaintain["ByFtyCarrier"]}' ";
            this.disSuppabb.Value = MyUtility.GetValue.Lookup(sqlsupp);

            this.CarrierbyEnable();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
SELECT OrderNumber = ROW_NUMBER() over (order by   TRY_CONVERT (int, CTNNo )
										    , (RIGHT (REPLICATE ('0', 10) + rtrim (ltrim (CTNNo)), 10))
										    , DropDownListSeq
							    )        
      ,allData.* 
FROM(
    select ed.*
	    ,case when ed.Category in ('4','9') then ed.MtlDesc else ed.Description end nDescription
	    ,AirPPno=iif(isnull(ed.PackingListID,'') = '',ed.DutyNo , airpp.AirPPno)
    from
    (
	    select ed.*,p.Refno,ed.SuppID+'-'+isnull(s.AbbEN,'') as Supplier,ec.CTNNW,
		    dbo.getMtlDesc(ed.OrderID,ed.Seq1,ed.Seq2,1,0) as MtlDesc,
		    isnull(cast(ec.CtnLength as varchar),'')+'*'+isnull(cast(ec.CtnWidth as varchar),'')+'*'+isnull(cast(ec.CtnHeight as varchar),'') as Dimension,
		    isnull((ed.InCharge+' '+(select Name+' #'+ExtNo from Pass1 WITH (NOLOCK) where ID = ed.InCharge)),ed.InCharge) as InChargeName,
		    isnull((ed.Receiver+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = ed.Receiver)),ed.Receiver) as ReceiverName,
		    isnull((ed.Leader+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = ed.Leader)),ed.Leader) as LeaderName,
		    CASE ed.Category
			    WHEN '1' THEN N'Sample'
			    WHEN '2' THEN N'SMS'
			    WHEN '3' THEN N'Bulk'
			    WHEN '4' THEN N'Material'
			    WHEN '5' THEN N'Dox'
			    WHEN '6' THEN N'Machine/Parts'
			    WHEN '7' THEN N'Mock Up'
			    WHEN '8' THEN N'Other Sample'
			    WHEN '9' THEN N'Other Material'
			    ELSE N''
		    END as CategoryName
		    ,[DropDownListSeq]=dp.Seq
			,[CategoryNameFromDD]=dp.Description
            ,pl.PulloutID
            ,pl.Type
	    from Express_Detail ed WITH (NOLOCK) 
	    left join PO_Supp_Detail p WITH (NOLOCK) on ed.OrderID = p.ID and ed.Seq1 = p.SEQ1 and ed.Seq2 = p.SEQ2
	    left join Supp s WITH (NOLOCK) on ed.SuppID = s.ID
	    left join Express_CTNData ec WITH (NOLOCK) on ed.ID = ec.ID and ed.CTNNo = ec.CTNNo
        left join DropDownList dp ON dp.Type='Pms_Sort_HC_DHL_Cate' AND ed.Category = dp.ID
		left join PackingList pl WITH (NOLOCK) on ed.PackingListID = pl.ID
	    where ed.ID = '{0}'
    )ed
    outer apply(
	    select top 1 AirPPno = AirPP.ID
	    from PackingList_Detail pld with(nolock)
	    inner join AirPP with(nolock) on AirPP.OrderID = pld.OrderID and AirPP.OrderShipmodeSeq = pld.OrderShipmodeSeq
	    where pld.id = ed.PackingListID and pld.OrderID = ed.OrderID
	    order by AirPP.AddDate desc
    )airpp
) allData
Order by CTNNo,Seq1,Seq2", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            orderid.CellMouseDoubleClick += (s, e) =>
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                            if (MyUtility.Convert.GetString(dr["Category"]) == "1" || MyUtility.Convert.GetString(dr["Category"]) == "2" || MyUtility.Convert.GetString(dr["Category"]) == "3")
                            {
                                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                                callPOItemForm.SetView(dr);
                                callPOItemForm.ShowDialog(this);
                            }

                            if (MyUtility.Convert.GetString(dr["Category"]) == "4")
                            {
                                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                                callEditPOForm.SetView(dr);
                                callEditPOForm.ShowDialog(this);
                            }

                            if (MyUtility.Convert.GetString(dr["Category"]) == "5" || MyUtility.Convert.GetString(dr["Category"]) == "6" || MyUtility.Convert.GetString(dr["Category"]) == "7" || MyUtility.Convert.GetString(dr["Category"]) == "8" || MyUtility.Convert.GetString(dr["Category"]) == "9")
                            {
                                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                                callNewItemForm.SetView(dr);
                                callNewItemForm.ShowDialog(this);
                            }
                        }
                    }
                };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: orderid)
                .Text("Seq1", header: "Seq1#", width: Widths.AnsiChars(3))
                .Text("Seq2", header: "Seq2#", width: Widths.AnsiChars(2))
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15))
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(15))
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15))
                .Text("CTNNo", header: "C/No.", width: Widths.AnsiChars(5))
                .Numeric("NW", header: "N.W.", width: Widths.AnsiChars(10), decimal_places: 3)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(10), decimal_places: 4)
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(7), decimal_places: 2)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(5))
                .Text("CategoryName", header: "Category", width: Widths.AnsiChars(14))
                .Text("PackingListID", header: "PL#", width: Widths.AnsiChars(13))
                .Text("PulloutID", header: "Pullout ID", width: Widths.AnsiChars(13))
                .Text("AirPPno", header: "AirPP#", width: Widths.AnsiChars(13))
                .Text("InChargeName", header: "In Charge", width: Widths.AnsiChars(25))
                .Text("ReceiverName", header: "Receiver", width: Widths.AnsiChars(13))
                .Text("LeaderName", header: "Team Leader", width: Widths.AnsiChars(30))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(30))
                .EditText("nDescription", header: "Description", width: Widths.AnsiChars(30))
                .Text("Dimension", header: "Dimension (cm)", width: Widths.AnsiChars(20))
                .Numeric("CTNNW", header: "Carton Weight", width: Widths.AnsiChars(10), decimal_places: 2);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            base.OnDetailGridRowChanged();
            this.editDescription.Text = string.Empty;
            if (this.CurrentDetailData != null)
            {
                if (MyUtility.Check.Empty(this.CurrentDetailData["OrderID"]) || MyUtility.Check.Empty(this.CurrentDetailData["Seq2"]))
                {
                    this.editDescription.Text = MyUtility.Convert.GetString(this.CurrentDetailData["Description"]);
                }
                else
                {
                    this.editDescription.Text = MyUtility.Convert.GetString(this.CurrentDetailData["MtlDesc"]);
                }

                // 先將Menu狀態全打開
                this.SetContextMenuStatus(true);

                // 只有HC的Handle或此項次的申請人才可以刪除及修改
                bool authority = PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentDetailData["InCharge"]));
                this.delete.Enabled = authority;
                this.edit.Enabled = authority;

                // Approve後就不可以再做任何動作(更改Context Menu Item的Enable/Disable)
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved")
                {
                    this.bulkpl.Enabled = false;
                    this.samplepl.Enabled = false;
                    this.focpl.Enabled = false;
                    this.purchase.Enabled = false;
                    this.poitem.Enabled = false;
                    this.newitem.Enabled = false;
                    this.delete.Enabled = false;
                    this.edit.Enabled = false;
                }

                // 修改顯示內容
                this.delete.Text = string.Format("Delete this Record - {0} {1}-{2}", MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]), MyUtility.Convert.GetString(this.CurrentDetailData["Seq1"]), MyUtility.Convert.GetString(this.CurrentDetailData["Seq2"]));
                this.edit.Text = string.Format("Edit this Record's detail - {0} {1}-{2}", MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]), MyUtility.Convert.GetString(this.CurrentDetailData["Seq1"]), MyUtility.Convert.GetString(this.CurrentDetailData["Seq2"]));
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["Manager"] = MyUtility.GetValue.Lookup("Supervisor", Sci.Env.User.UserID, "Pass1", "ID");
            this.CurrentMaintain["NW"] = 0;
            this.CurrentMaintain["CTNNW"] = 0;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.txtCarrier.ReadOnly = true; // 因為Key Down事件，如果按Delete or Backspace按鍵會真的將字元給移除，如果跳出視窗後按Cancel的話，資料會不正確，所以就把此欄位設定為ReadOnly
            this.txtCarrierbyCustomer.ReadOnly = true;
            this.txtCarrierbyFty.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (!( MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "New" ||
                   MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Sent" ||
                   MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved"))
            {
                MyUtility.Msg.WarningBox("Status is not 'New' or 'Sent', can't modify!");
                return false;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved" && !MyUtility.Check.Empty(this.CurrentMaintain["PayDate"]))
            {
                MyUtility.Msg.WarningBox("Already have payment, can't modify!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "New")
            {
                this.txtTO.ReadOnly = MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "1" ? true : false;
                this.txtCarrier.ReadOnly = true;
                this.txtCarrierbyCustomer.ReadOnly = true;
                this.txtCarrierbyFty.ReadOnly = true;
            }
            else if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved")
            {
                this.txtFrom.ReadOnly = true;
                this.txtTO.ReadOnly = true;
                this.txtShipMark.ReadOnly = true;
                this.txtPort.ReadOnly = true;
                this.txtBLNo.ReadOnly = true;
                this.comboFrom.ReadOnly = true;
                this.comboTO.ReadOnly = true;
                this.txtUserHandle.TextBox1.ReadOnly = true;
                this.txtUserManager.TextBox1.ReadOnly = true;
                this.dateShipDate.ReadOnly = true;
                this.dateETD.ReadOnly = true;
                this.dateETA.ReadOnly = true;
                this.editRemark.ReadOnly = true;
                this.txtCountryDestination.TextBox1.ReadOnly = true;
                this.txtInvoiceNo.ReadOnly = true;
            }
            else
            {
                this.txtFrom.ReadOnly = true;
                this.txtTO.ReadOnly = true;
                this.txtShipMark.ReadOnly = true;
                this.txtPort.ReadOnly = true;
                this.txtCarrier.ReadOnly = true;
                this.txtCarrierbyCustomer.ReadOnly = true;
                this.txtCarrierbyFty.ReadOnly = true;
                this.txtBLNo.ReadOnly = true;
                this.comboFrom.ReadOnly = true;
                this.comboTO.ReadOnly = true;
                this.txtUserHandle.TextBox1.ReadOnly = true;
                this.txtUserManager.TextBox1.ReadOnly = true;
                this.dateShipDate.ReadOnly = true;
                this.dateETD.ReadOnly = true;
                this.dateETA.ReadOnly = true;
                this.editRemark.ReadOnly = true;
                this.txtCountryDestination.TextBox1.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["FromSite"]))
            {
                this.txtFrom.Focus();
                MyUtility.Msg.WarningBox("From (Site) can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ToSite"]))
            {
                this.txtTO.Focus();
                MyUtility.Msg.WarningBox("To (Site) can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Dest"]))
            {
                this.txtCountryDestination.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Destination can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["PortAir"]))
            {
                this.txtPort.Focus();
                MyUtility.Msg.WarningBox("Port can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipDate"]))
            {
                this.dateShipDate.Focus();
                MyUtility.Msg.WarningBox("Ship. Date can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ETD"]))
            {
                this.dateETD.Focus();
                MyUtility.Msg.WarningBox("ETD can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ETA"]))
            {
                this.dateETA.Focus();
                MyUtility.Msg.WarningBox("ETA can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FreightBy"]))
            {
                this.cmbPayer.Select();
                MyUtility.Msg.WarningBox("Payer cannot be empty.");
                return false;
            }

            if (this.EditMode && this.cmbPayer.Text.ToLower() == "Freight Collect By 3rd Party".ToLower() && MyUtility.Check.Empty(this.CurrentMaintain["CarrierID"]))
            {
                this.txtCarrier.Focus();
                MyUtility.Msg.WarningBox("Carrier can't empty");
                return false;
            }
            else if (this.EditMode && this.cmbPayer.Text.ToLower() == "Freight Collect By Customer".ToLower() && MyUtility.Check.Empty(this.CurrentMaintain["ByCustomerCarrier"]))
            {
                this.txtCarrierbyCustomer.Focus();
                MyUtility.Msg.WarningBox("Carrier can't empty");
                return false;
            }
            else if (this.EditMode && this.cmbPayer.Text.ToLower() == "Freight Pre-Paid by Fty".ToLower() && MyUtility.Check.Empty(this.CurrentMaintain["ByFtyCarrier"]))
            {
                this.txtCarrierbyFty.Focus();
                MyUtility.Msg.WarningBox("Carrier can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
            {
                this.txtBLNo.Focus();
                MyUtility.Msg.WarningBox("B/L No. can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Handle"]))
            {
                this.txtUserHandle.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Handle can't empty");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Manager"]))
            {
                this.txtUserManager.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Manager can't empty");
                return false;
            }
            #endregion

            #region set Special Sending Value
            if ((this.CurrentMaintain["FromTag"].ToString() == "1") &&
                    (this.CurrentMaintain["ToTag"].ToString() == "1" ||
                     this.CurrentMaintain["ToTag"].ToString() == "2"))
            {
                string fromRegion = MyUtility.GetValue.Lookup($@"select NegoRegion from Factory where id='{this.CurrentMaintain["FromSite"]}'");
                string countryCode = string.Empty;
                string dateShipDate = ((DateTime)this.CurrentMaintain["ShipDate"]).ToString("yyyy/MM/dd");

                switch (this.CurrentMaintain["ToTag"].ToString())
                {
                    case "1":
                        countryCode = MyUtility.GetValue.Lookup($@"select CountryID from Supp where AbbEN='sci'");
                        break;
                    case "2":
                        countryCode = MyUtility.GetValue.Lookup($@"select CountryID from SCIFty where id='{this.CurrentMaintain["ToSite"]}'");
                        break;
                    default:
                        break;
                }

                string sqlcmd = string.Empty;
                DataRow dr;

                // 存在FactoryExpress_SendingSchedule
                sqlcmd = $@"
select 
[IsSpecialSending] = 
case when Date.value = '1' and s.MON=1 then 0 
	 when Date.value = '2' and s.TUE=1 then 0 
     when Date.value = '3' and s.WED=1 then 0 
     when Date.value = '4' and s.THU=1 then 0 
     when Date.value = '5' and s.FRI=1 then 0 
     when Date.value = '6' and s.SAT=1 then 0 
     when Date.value = '7' and s.SUN=1 then 0 
else 1 end
,S.* 
from FactoryExpress_SendingSchedule S
outer apply(SELECT value = CONVERT(CHAR(1),DATEPART(WEEKDAY, CONVERT(datetime,'{dateShipDate}') +6))) Date
where RegionCode = '{fromRegion}' 
and ToID='{countryCode}' 
and s.BeginDate <= CONVERT(datetime,'{dateShipDate}')
and s.junk = 0
";
                if (MyUtility.Check.Seek(sqlcmd, out dr))
                {
                    this.CurrentMaintain["IsSpecialSending"] = dr["IsSpecialSending"];
                }
                else
                {
                    // FactoryExpress_SendingScheduleHistory
                    sqlcmd = $@"
select 
[IsSpecialSending] = 
case when Date.value = '1' and regular.MON >= 1 then 0 
	 when Date.value = '2' and regular.TUE >= 1 then 0 
     when Date.value = '3' and regular.WED >= 1 then 0 
     when Date.value = '4' and regular.THU >= 1 then 0 
     when Date.value = '5' and regular.FRI >= 1 then 0 
     when Date.value = '6' and regular.SAT >= 1 then 0 
     when Date.value = '7' and regular.SUN >= 1 then 0
     when FoundHis = 0 then 0 
     else 1 
end	
from (
	select MON = count (case when s.MON = 1 then 1 end)
			, TUE = count (case when s.TUE = 1 then 1 end)
			, WED = count (case when s.WED = 1 then 1 end)
			, THU = count (case when s.THU = 1 then 1 end)
			, FRI = count (case when s.FRI = 1 then 1 end)
			, SAT = count (case when s.SAT = 1 then 1 end)
			, SUN = count (case when s.SUN = 1 then 1 end)
            , FoundHis = count(1)
	from FactoryExpress_SendingScheduleHistory s
    where RegionCode = '{fromRegion}' 
          and ToID='{countryCode}' 
          and ('{dateShipDate}' between s.BeginDate and s.EndDate
               or (s.EndDate is null
                   and s.BeginDate <= '{dateShipDate}'
                  )
              )
) regular
outer apply (
    SELECT value = CONVERT(CHAR(1),DATEPART(WEEKDAY, CONVERT(datetime,'{dateShipDate}') +6))
) Date
";
                    if (MyUtility.Check.Seek(sqlcmd, out dr))
                    {
                        this.CurrentMaintain["IsSpecialSending"] = dr["IsSpecialSending"];
                    }
                    else
                    {
                        this.CurrentMaintain["IsSpecialSending"] = false;
                    }
                }

            }
            else
            {
                this.CurrentMaintain["IsSpecialSending"] = false;
            }
            #endregion

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "HC", "Express", Convert.ToDateTime(this.CurrentMaintain["ShipDate"]), 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);
            base.ClickSaveAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            Sci.Production.Shipping.P02_Print callPurchaseForm = new Sci.Production.Shipping.P02_Print(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        // From
        private void ComboFrom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.CurrentMaintain["FromTag"] = this.comboFrom.SelectedValue;
            this.CurrentMaintain["FromSite"] = string.Empty;
            this.displayFrom.Value = string.Empty;
            this.ClearCarrier();
        }

        // From Site
        private void TxtFrom_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["FromTag"]))
            {
                return;
            }

            string sqlCmd = string.Empty;
            Sci.Win.Tools.SelectItem item;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "1")
            {
                sqlCmd = "select ID from Factory WITH (NOLOCK) where Junk = 0 and ExpressGroup <> ''";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", this.txtFrom.Text);
            }
            else
            {
                sqlCmd = "select ID,NameEN from Brand WITH (NOLOCK) where Junk = 0";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,50", this.txtFrom.Text);
            }

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["FromSite"] = item.GetSelectedString();
            if (MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "2")
            {
                IList<DataRow> brand = item.GetSelecteds();
                this.displayFrom.Value = MyUtility.Convert.GetString(brand[0]["NameEN"]);
            }

            this.ClearCarrier();
        }

        // From Site
        private void TxtFrom_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtFrom.OldValue != this.txtFrom.Text)
            {
                if (MyUtility.Check.Empty(this.txtFrom.Text))
                {
                    this.CurrentMaintain["FromSite"] = string.Empty;
                    this.displayFrom.Value = string.Empty;
                }
                else
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", this.txtFrom.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);

                    if (MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "1")
                    {
                        DataTable factoryData;
                        string sqlCmd = "select ID from Factory WITH (NOLOCK) where Junk = 0 and ExpressGroup <> '' and ID = @id";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out factoryData);
                        if (!result || factoryData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("Factory: {0} does not exist.", this.txtFrom.Text));
                            }

                            this.CurrentMaintain["FromSite"] = string.Empty;
                            this.displayFrom.Value = string.Empty;
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            this.CurrentMaintain["FromSite"] = this.txtFrom.Text;
                            this.CurrentMaintain["ShipMark"] = this.txtFrom.Text;
                            this.displayFrom.Value = string.Empty;
                        }
                    }
                    else
                    {
                        if (MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "2")
                        {
                            DataTable brandData;
                            string sqlCmd = "select NameEN from Brand WITH (NOLOCK) where Junk = 0 and ID = @id";
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out brandData);

                            if (!result || brandData.Rows.Count <= 0)
                            {
                                if (!result)
                                {
                                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                                }
                                else
                                {
                                    MyUtility.Msg.WarningBox(string.Format("Brand: {0} does not exist.", this.txtFrom.Text));
                                }

                                this.CurrentMaintain["FromSite"] = string.Empty;
                                this.displayFrom.Value = string.Empty;
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                this.CurrentMaintain["FromSite"] = this.txtFrom.Text;
                                this.displayFrom.Value = MyUtility.Convert.GetString(brandData.Rows[0]["NameEN"]);
                            }
                        }
                    }
                }

                this.ClearCarrier();
            }
        }

        // To
        private void ComboTO_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.CurrentMaintain["ToTag"] = this.comboTO.SelectedValue;
            this.CurrentMaintain["ToSite"] = MyUtility.Convert.GetString(this.comboTO.SelectedValue) == "1" ? "SCI" : string.Empty;
            this.displayTO.Value = string.Empty;
            this.txtTO.ReadOnly = MyUtility.Convert.GetString(this.comboTO.SelectedValue) == "1" ? true : false;

            this.ClearCarrier();
        }

        // To Site
        private void TxtTO_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ToTag"]))
            {
                return;
            }

            string sqlCmd = string.Empty;
            Sci.Win.Tools.SelectItem item;
            if (this.CurrentMaintain["ToTag"].ToString() == "2")
            {
                sqlCmd = "select ID from SCIFty WITH (NOLOCK) where Junk = 0 AND ExpressGroup <> ''";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", this.txtTO.Text);
            }
            else
            {
                if (this.CurrentMaintain["ToTag"].ToString() == "3")
                {
                    sqlCmd = @"
select ID,AbbCH,AbbEN from Supp s WITH (NOLOCK) where s.Junk = 0 
and not exists(select 1 from SCIFty where id=s.id )
    union all 
select ID,AbbCH = Abb,AbbEN = Abb 
from LocalSupp WITH (NOLOCK) where Junk = 0
and IsFactory=0";
                    item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,20,20", this.txtTO.Text);
                }
                else
                {
                    sqlCmd = "select ID,NameEN,CountryID from Brand WITH (NOLOCK) where Junk = 0";
                    item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,20,0", this.txtTO.Text);
                }
            }

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["ToSite"] = item.GetSelectedString();
            IList<DataRow> brand = item.GetSelecteds();
            if (this.CurrentMaintain["ToTag"].ToString() == "3")
            {
                this.displayTO.Value = brand[0]["AbbEN"].ToString();
            }

            if (this.CurrentMaintain["ToTag"].ToString() == "4")
            {
                this.displayTO.Value = brand[0]["NameEN"].ToString();
                this.CurrentMaintain["Dest"] = brand[0]["CountryID"].ToString();
            }

            this.ClearCarrier();
        }

        // To Site
        private void TxtTO_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtTO.OldValue != this.txtTO.Text)
            {
                if (MyUtility.Check.Empty(this.txtTO.Text))
                {
                    this.CurrentMaintain["ToSite"] = string.Empty;
                    this.CurrentMaintain["PortAir"] = string.Empty;
                    this.displayTO.Value = string.Empty;
                }
                else
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", this.txtTO.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    if (MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "2")
                    {
                        DataTable sCIFtyData;
                        string sqlCmd = "select ID,ExpressGroup,CountryID,PortAir from SCIFty WITH (NOLOCK) where Junk = 0 AND ExpressGroup <> '' and ID = @id";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out sCIFtyData);

                        if (!result || sCIFtyData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("Factory: {0} does not exist.", this.txtTO.Text));
                            }

                            this.CurrentMaintain["ToSite"] = string.Empty;
                            this.CurrentMaintain["PortAir"] = string.Empty;
                            this.displayTO.Value = string.Empty;
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            this.CurrentMaintain["ToSite"] = this.txtTO.Text;
                            this.CurrentMaintain["PortAir"] = MyUtility.Convert.GetString(sCIFtyData.Rows[0]["PortAir"]);
                            this.displayTO.Value = string.Empty;
                            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipMark"]))
                            {
                                this.CurrentMaintain["ShipMark"] = this.txtTO.Text;
                                this.CurrentMaintain["Dest"] = MyUtility.Convert.GetString(sCIFtyData.Rows[0]["CountryID"]);
                            }
                        }
                    }
                    else
                    {
                        if (MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "3")
                        {
                            DataTable suppData;
                            string sqlCmd = @"
select ID,AbbCH,AbbEN 
from Supp s WITH (NOLOCK) where s.Junk = 0 
and not exists(select * from SCIFty where id=s.id )
and ID = @id
union all 
select ID,AbbCH = Abb,AbbEN = Abb 
from LocalSupp WITH (NOLOCK) where Junk = 0
and IsFactory=0
and ID = @id ";
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out suppData);

                            if (!result || suppData.Rows.Count <= 0)
                            {
                                if (!result)
                                {
                                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                                }
                                else
                                {
                                    MyUtility.Msg.WarningBox(string.Format("Supplier: {0} does not exist.", this.txtTO.Text));
                                }

                                this.CurrentMaintain["ToSite"] = string.Empty;
                                this.CurrentMaintain["PortAir"] = string.Empty;
                                this.displayTO.Value = string.Empty;
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                this.CurrentMaintain["ToSite"] = this.txtTO.Text;
                                this.displayTO.Value = MyUtility.Convert.GetString(suppData.Rows[0]["AbbEN"]);
                                this.CurrentMaintain["PortAir"] = string.Empty;
                            }
                        }
                        else
                        {
                            if (MyUtility.Convert.GetString(this.CurrentMaintain["ToTag"]) == "4")
                            {
                                DataTable brandData;
                                string sqlCmd = "select NameEN,CountryID from Brand WITH (NOLOCK) where Junk = 0 and ID = @id";
                                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out brandData);

                                if (!result || brandData.Rows.Count <= 0)
                                {
                                    if (!result)
                                    {
                                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                                    }
                                    else
                                    {
                                        MyUtility.Msg.WarningBox(string.Format("Brand: {0} does not exist.", this.txtTO.Text));
                                    }

                                    this.CurrentMaintain["ToSite"] = string.Empty;
                                    this.CurrentMaintain["PortAir"] = string.Empty;
                                    this.displayTO.Value = string.Empty;
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    this.CurrentMaintain["ToSite"] = this.txtTO.Text;
                                    this.displayTO.Value = MyUtility.Convert.GetString(brandData.Rows[0]["NameEN"]);
                                    this.CurrentMaintain["Dest"] = MyUtility.Convert.GetString(brandData.Rows[0]["CountryID"]);
                                    this.CurrentMaintain["PortAir"] = string.Empty;
                                }
                            }
                        }
                    }
                }

                this.ClearCarrier();
            }
        }

        private void DateETD_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.dateETD.Value) && this.dateETD.OldValue != this.dateETD.Value)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["ETA"]) && this.dateETD.Value > MyUtility.Convert.GetDate(this.CurrentMaintain["ETA"]))
                {
                    this.CurrentMaintain["ETD"] = DBNull.Value;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("ETD can't later than ETA!");
                    return;
                }
            }
        }

        // ETA
        private void DateETA_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.dateETA.Value) && this.dateETA.OldValue != this.dateETA.Value)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["ETD"]))
                {
                    if (this.dateETA.Value < MyUtility.Convert.GetDate(this.CurrentMaintain["ETD"]))
                    {
                        this.CurrentMaintain["ETA"] = DBNull.Value;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("ETA can't early than ETD!");
                        return;
                    }
                    else
                    {
                        if (this.dateETA.Value > Convert.ToDateTime(this.CurrentMaintain["ETD"]).AddDays(90))
                        {
                            this.CurrentMaintain["ETA"] = DBNull.Value;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("ETA can't later than ETD more than 90 days!");
                            return;
                        }
                    }
                }
            }
        }

        // Carrier
        private void TxtCarrier_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.EditMode)
            {
                this.CarrierPopup();
            }
        }

        // Carrier
        private void TxtCarrier_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CarrierPopup();
        }

        // Carrier按右鍵帶出的資料
        private void CarrierPopup()
        {
            string fromTag = this.NametoTag(this.comboFrom.Text);
            string toTag = this.NametoTag(this.comboTO.Text);
            if (MyUtility.Check.Empty(this.txtFrom.Text))
            {
                MyUtility.Msg.WarningBox($"Please select < From {this.comboFrom.Text} >");
                return;
            }

            if (MyUtility.Check.Empty(this.txtTO.Text))
            {
                MyUtility.Msg.WarningBox($"Please select < To {this.comboTO.Text} >");
                return;
            }

            string fromCountry = this.ConverToCountry(this.comboFrom.Text, this.txtFrom.Text);
            string toCountry = this.ConverToCountry(this.comboTO.Text, this.txtTO.Text);
            string sqlCmd = $@"
select CarrierID=ca.id,SuppID=su.ID,su.AbbEN,Account
from Carrier ca
inner join Carrier_Detail_Freight cd on  cd.id=ca.id
inner join Supp su on su.ID = ca.SuppID
where FromTag='{fromTag}' 
and ToTag='{toTag}' 
and (FromInclude like'%{fromCountry}%' or FromInclude = '')
and (ToInclude like'%{toCountry}%' or ToInclude = '')";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,8,10,20", this.txtCarrier.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> carrier = item.GetSelecteds();
            this.CurrentMaintain["CarrierID"] = item.GetSelectedString();
            this.displayCarrier.Value = MyUtility.Convert.GetString(carrier[0]["SuppID"]) + " " + MyUtility.Convert.GetString(carrier[0]["AbbEN"]);
            this.CurrentMaintain["ExpressACNo"] = carrier[0]["Account"];
        }

        // B/L No.
        private void TxtBLNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtBLNo) && this.txtBLNo.OldValue != this.txtBLNo.Text)
            {
                for (int i = 0; i < this.txtBLNo.Text.Trim().Length; i++)
                {
                    var asc = ASCIIEncoding.ASCII.GetBytes(this.txtBLNo.Text.Substring(i, 1));
                    if (!((asc[0] >= 48 && asc[0] <= 57) || (asc[0] >= 65 && asc[0] <= 90)))
                    {
                        this.CurrentMaintain["BLNo"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("B/L No. format error, only keyin (0-9,A-Z)!");
                        return;
                    }
                }
            }
        }

        // Carton Dimension && Weight
        private void BtnCartonDimensionWeight_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string sqlCmd = string.Format(
                @"with NewCtn
as
(
select distinct ed.ID,ed.CTNNo,'N' as Status 
from Express_Detail ed WITH (NOLOCK) 
where ed.ID = '{0}'
and not exists (select 1 from Express_CTNData ec WITH (NOLOCK) where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
),
DeleteCtn
as
(
select distinct ec.ID,ec.CTNNo,'D' as Status 
from Express_CTNData ec WITH (NOLOCK) 
where ec.ID = '{0}'
and not exists (select 1 from Express_Detail ed WITH (NOLOCK) where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
)
select * from NewCtn
union
select * from DeleteCtn", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable cTNData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out cTNData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query Ctn data fail, please try again.\r\n" + result.ToString());
                return;
            }

            if (cTNData.Rows.Count > 0)
            {
                IList<string> updateCmds = new List<string>();
                foreach (DataRow dr in cTNData.Rows)
                {
                    if (MyUtility.Convert.GetString(dr["Status"]) == "N")
                    {
                        updateCmds.Add(string.Format("insert into Express_CTNData(ID,CTNNo,AddName, AddDate) values ('{0}','{1}','{2}',GETDATE());", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["CTNNo"]), Sci.Env.User.UserID));
                    }
                    else
                    {
                        updateCmds.Add(string.Format("delete from Express_CTNData where ID = '{0}' and CTNNo = '{1}';", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["CTNNo"])));
                    }
                }

                result = DBProxy.Current.Executes(null, updateCmds);

                if (!result)
                {
                    MyUtility.Msg.WarningBox("Update Ctn data fail, please try again.\r\n" + result.ToString());
                    return;
                }
            }

                Sci.Production.Shipping.P02_CTNDimensionAndWeight callNextForm = new Sci.Production.Shipping.P02_CTNDimensionAndWeight(
(MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "New" || MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Sent") && (PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Manager"]))), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), null, null);
                callNextForm.ShowDialog(this);
                this.RenewData();
                this.numericBoxttlGW.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNNW"]);
        }

        /// <inheritdoc/>
        protected override void ClickSend()
        {
            base.ClickSend();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to send.");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("B/L No. can't empty!!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Send > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            this.SendMail();
            string updateCmd = string.Format(
                @"
insert into Express_History([ID], [OldValue], [NewValue], [AddName], [AddDate])
select ID, Status, 'Sent', '{0}', GETDATE()
from Express
where ID = '{1}';

update Express set Status = 'Sent', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickRecall()
        {
            base.ClickRecall();
            string updateCmd = string.Format(
                @"
insert into Express_History([ID], [OldValue], [NewValue], [AddName], [AddDate])
select ID, Status, 'New', '{0}', GETDATE()
from Express
where ID = '{1}';

update Express set Status = 'New', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}';",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Recall data faile.\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to Junk.");
                return;
            }

            if (!this.CheckPullexists(false))
            {
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Junk > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format(
                @"
insert into Express_History([ID], [OldValue], [NewValue], [AddName], [AddDate])
select ID, Status, 'Junk', '{0}', GETDATE()
from Express
where ID = '{1}';

update Express set Status = 'Junk', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            updateCmds.Add(string.Format("update PackingList set pulloutdate=null where ExpressID = '{0}' and Type = 'F'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            updateCmds.Add(string.Format("update PackingList set ExpressID = '' where ExpressID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Junk data faile.\r\n" + result.ToString());
                return;
            }

            this.SendMail();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            this.RenewData();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to approve.");
                return;
            }

            if (!MyUtility.Check.Seek(string.Format("select ID from Express_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("There's no detail data, don't need to approve.");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["NW"]))
            {
                MyUtility.Msg.WarningBox("< ttl N.W. > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("< B/L No. > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FtyInvNo"]))
            {
                MyUtility.Msg.WarningBox("< Invoice No. > can't empty!");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_Detail WITH (NOLOCK) where ID = '{0}' and NW <= 0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("N.W. detail data can't be 0.");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_Detail WITH (NOLOCK) where ID = '' and Qty <= 0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("Q'ty detail data can't be 0.");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_CTNData WITH (NOLOCK) where ID = '{0}' and (CtnLength <= 0 or CtnWidth <= 0 or CtnHeight <= 0 or CTNNW <= 0)", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("Carton Dimension & Weight data can't empty!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Approve > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string updateCmd = string.Format(
                @"
insert into Express_History([ID], [OldValue], [NewValue], [AddName], [AddDate])
select ID, Status, 'Approved', '{0}', GETDATE()
from Express
where ID = '{1}';

update Express set Status = 'Approved', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}';",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            string shipDate = MyUtility.Check.Empty(this.CurrentMaintain["ShipDate"]) ? "NULL" : "'" + ((DateTime)this.CurrentMaintain["ShipDate"]).ToString("d") + "'";
            updateCmd += $" update PackingList set PulloutDate = {shipDate} where ExpressID = '{this.CurrentMaintain["ID"]}' and type = 'F'";

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Approve data faile.\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to unapprove.");
                return;
            }

                string sqlchk = $@"
select PackingListID = concat('FOC PL : ',ed.PackingListID)
from Express_Detail ed
inner join PackingList pl on pl.ID = ed.PackingListID and pl.Type = 'F'
inner join Pullout p on p.ID = pl.PulloutID
where ed.ID = '{this.CurrentMaintain["ID"]}' and p.Status in ('Confirmed','Locked')
";
            DataTable dtchk;
            DualResult result1 = DBProxy.Current.Select(null, sqlchk, out dtchk);
            if (!result1)
            {
                this.ShowErr(result1);
                return;
            }

            if (dtchk.Rows.Count > 0)
            {
                var x = dtchk.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["PackingListID"])).ToList();
                MyUtility.Msg.WarningBox($"Below record's pullout report already confirmed, HC cannot unconfirm.\r\n" + string.Join("\r\n", x));
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["PayDate"]))
            {
                MyUtility.Msg.WarningBox("Already have payment, can't unapprove!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unapprove > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            IList<string> updateCmds = new List<string>();

            updateCmds.Add(string.Format(
                @"
insert into Express_History([ID], [OldValue], [NewValue], [AddName], [AddDate])
select ID, Status, 'Sent', '{0}', GETDATE()
from Express
where ID = '{1}';

update Express set Status = 'Sent', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            updateCmds.Add(string.Format("update PackingList set pulloutdate=null where ExpressID = '{0}' and Type = 'F'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unapprove data faile.\r\n" + result.ToString());
                return;
            }
        }

        // Mail to
        private void BtnMailto_Click(object sender, EventArgs e)
        {
            this.SendMail();
        }

        // Mail to
        private void SendMail()
        {
            DataRow dr;
            if (MyUtility.Check.Seek("select * from MailTo WITH (NOLOCK) where ID = '003'", out dr))
            {
                string mailto = MyUtility.Convert.GetString(dr["ToAddress"]);
                string cc = MyUtility.Convert.GetString(dr["CcAddress"]);
                string subject = string.Format(
                    "<{0}-{1}> TO <{2}{3}> HC#({4}) International Express ({5}){6}",
                    MyUtility.Convert.GetString(this.CurrentMaintain["FromSite"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "1" ? MyUtility.Convert.GetString(this.CurrentMaintain["FromSite"]) : MyUtility.Convert.GetString(this.displayFrom.Value),
                    MyUtility.Convert.GetString(this.CurrentMaintain["ToSite"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "3" ? "-" + MyUtility.Convert.GetString(this.displayTO.Value) : string.Empty,
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    MyUtility.Check.Empty(this.CurrentMaintain["ShipDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)),
                    MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Junk" ? " -Cancel" : string.Empty);

                StringBuilder content = new StringBuilder();
                #region 組Content
                content.Append(string.Format(
                    @"< {0} > will have EXPRESS shipment from {1}-{2} TO {3}{4} ON {5} 

If anyone who wants to follow this shipment, must fill the detail & sp# in this HC#. And team leader need to approve. 
When first applicant's team leader approval, anyone can not do any modification. Please noted.


---Pls notice the Qty & G.W must coincide w/ shipping document. If found out a great inconsistency between system & document then application team have to be deputy of all express application for one month.
---If the goods belong to BULK ORDER, pls must fill in the system with ICR# or Debit note#.
---Applicant need to notify factory & supplier's attn. to arrange the shipping time by oneself before export the goods.
---Applicant also need to notify factory , supplier & customer's attn. to make sure that they can receive the goods by oneself after shipped out  the goods.",
                    MyUtility.Convert.GetString(this.CurrentMaintain["Manager"]) + "-" + MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Manager"]))),
                    MyUtility.Convert.GetString(this.CurrentMaintain["FromSite"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "1" ? MyUtility.Convert.GetString(this.CurrentMaintain["FromSite"]) : MyUtility.Convert.GetString(this.displayFrom.Value),
                    MyUtility.Convert.GetString(this.CurrentMaintain["ToSite"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["FromTag"]) == "3" ? "-" + MyUtility.Convert.GetString(this.displayTO.Value) : string.Empty,
                    MyUtility.Check.Empty(this.CurrentMaintain["ShipDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat))));
                #endregion

                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, cc, subject, string.Empty, content.ToString(), false, false);
                email.ShowDialog(this);
            }
        }

        /// <summary>
        /// 檢查 這張HC底下所有的PackingList/單筆PackingList  是否已經出貨
        /// </summary>
        /// <param name="isSingelCheck"> false=檢查這張HC底下所有的PackingList、true=檢查單筆PackingList </param>
        /// <returns>是否存在</returns>
        private bool CheckPullexists(bool isSingelCheck = true)
        {
            #region 若該HC#下的Packing已經被拉到Pullout Report中且Pullout Status為confirm則不能刪除
            string sqlchk = string.Empty;

            if (isSingelCheck)
            {
                string packingListID = this.CurrentDetailData["PackingListID"].ToString();
                sqlchk = $@"
SELECt [PackingListID]=p.ID 
FROM PackingList p 
INNER JOIN Pullout pu ON p.PulloutID=pu.ID 
WHERE p.ID='{packingListID}' 
AND Type In ('B','S','F') 
AND pu.Status <> 'New'
";
            }
            else
            {
                sqlchk = $@"
select distinct [PackingListID]= pl.ID
from Pullout_Detail pd
inner join PackingList pl on pl.id = pd.PackingListID
inner join Pullout p on p.id = pd.id
where p.status = 'confirmed'
and pl.ExpressID = '{this.CurrentMaintain["ID"]}'
";
            }

            DataTable pkdt;
            DualResult result = DBProxy.Current.Select(null, sqlchk, out pkdt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (pkdt.Rows.Count > 0)
            {
                string msg = "These PackingList# are existed in Pullout Report so it can't be delete!\r\nPackingList#: " + string.Join(Environment.NewLine + "PackingList#: ", pkdt.AsEnumerable().Select(row => row["PackingListID"]).ToList());
                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            return true;
            #endregion
        }

        private void CarrierbyEnable()
        {
            if (this.EditMode && this.cmbPayer.Text.ToLower() == "Freight Collect By 3rd Party".ToLower())
            {
                this.txtCarrier.Enabled = true;
                this.txtCarrierbyCustomer.Enabled = false;
                this.txtCarrierbyFty.Enabled = false;
            }
            else if (this.EditMode && this.cmbPayer.Text.ToLower() == "Freight Collect By Customer".ToLower())
            {
                this.txtCarrier.Enabled = false;
                this.txtCarrierbyCustomer.Enabled = true;
                this.txtCarrierbyFty.Enabled = false;
            }
            else if (this.EditMode && this.cmbPayer.Text.ToLower() == "Freight Pre-Paid by Fty".ToLower())
            {
                this.txtCarrier.Enabled = false;
                this.txtCarrierbyCustomer.Enabled = false;
                this.txtCarrierbyFty.Enabled = true;
            }
            else
            {
                this.txtCarrier.Enabled = false;
                this.txtCarrierbyCustomer.Enabled = false;
                this.txtCarrierbyFty.Enabled = false;
            }
        }

        private void TxtCarrierbyCustomer_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CarrierbyCustomerPopup();
        }

        private void TxtCarrierbyCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.EditMode)
            {
                this.CarrierbyCustomerPopup();
            }
        }

        // Carrier按右鍵帶出的資料
        private void CarrierbyCustomerPopup()
        {
            string sqlCmd = $@"
select CarrierID,Account
from FreightCollectByCustomer
where BrandID='{this.txtTO.Text}'
and Dest='{this.CurrentMaintain["Dest"]}'
";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,10", this.txtCarrier.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["ByCustomerCarrier"] = item.GetSelectedString();
            this.CurrentMaintain["ByCustomerAccountID"] = item.GetSelecteds()[0]["Account"];
        }

        private void TxtCarrierbyFty_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CarrierbyFtyPopup();
        }

        private void TxtCarrierbyFty_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.EditMode)
            {
                this.CarrierbyFtyPopup();
            }
        }

        // Carrier按右鍵帶出的資料
        private void CarrierbyFtyPopup()
        {
            string sqlCmd = @"select id,Abb from LocalSupp with(nolock) where Junk = 0";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,15", this.txtCarrier.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["ByFtyCarrier"] = item.GetSelectedString();
            this.disSuppabb.Value = item.GetSelecteds()[0]["abb"];
        }

        private string NametoTag(string n)
        {
            if (n == "Brand") // 因為trade來的設定資料是Buyer
            {
                n = "Buyer";
            }

            string sql = $@"select id from DropDownList where Type = 'ShipPlan_HC_To' and name ='{n}'";
            return MyUtility.GetValue.Lookup(sql);
        }

        private string ConverToCountry(string tag, string id)
        {
            string c = string.Empty;
            switch (tag)
            {
                case "SCI":
                    c = "TW";
                    break;
                case "Factory":
                    c = MyUtility.GetValue.Lookup($"select countryID from factory with(nolock) where id = '{id}'");
                    break;
                case "Supplier":
                    c = MyUtility.GetValue.Lookup($"select countryID from Supp with(nolock) where id = '{id}'");
                    break;
                case "Brand":
                    c = MyUtility.GetValue.Lookup($"select countryID from brand with(nolock) where id = '{id}'");
                    break;
                default:
                    break;
            }

            return c;
        }

        private void ClearCarrier()
        {
            if (this.CurrentMaintain == null || !this.EditMode)
            {
                return;
            }

            this.CurrentMaintain["CarrierID"] = string.Empty;
            this.CurrentMaintain["ByCustomerCarrier"] = string.Empty;
            this.CurrentMaintain["ByCustomerAccountID"] = string.Empty;
            this.CurrentMaintain["ByFtyCarrier"] = string.Empty;
            this.CurrentMaintain["ExpressACNo"] = string.Empty;
            this.displayCarrier.Text = string.Empty;
            this.disSuppabb.Text = string.Empty;
        }

        private void CmbPayer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.CurrentMaintain["FreightBy"] = this.cmbPayer.SelectedValue;
            this.ClearCarrier();
            this.CurrentMaintain.EndEdit();
        }

        private void CmbPayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CarrierbyEnable();
        }

        private void BtnPaymentDetail_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new P02_PaymentDetail(this.CurrentMaintain);
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new P02_History(this.CurrentMaintain["ID"].ToString());
            frm.ShowDialog(this);
            frm.Dispose();
        }
    }
}
