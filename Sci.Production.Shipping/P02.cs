using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Shipping
{
    public partial class P02 : Sci.Win.Tems.Input2
    {
        //宣告Context Menu Item
        ToolStripMenuItem focpl, purchase, poitem, newitem, edit, delete, print, batchprint;

        protected string carrierID = "";
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboFrom, 2, 1, "1,Factory,2,Brand");
            MyUtility.Tool.SetupCombox(comboTO, 2, 1, "1,SCI,2,Factory,3,Sullpier,4,Brand");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            detailgrid.CellToolTipTextNeeded += (s, e) =>
                {
                    e.ToolTipText = "You can show the function form to press the right key under inquiring the state.";
                };

            detailgridmenus.Items.Clear();//清空原有的Menu Item
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import from FOC PL# (Garment FOC)", onclick: (s, e) => ImportFromFOCPL()).Get(out focpl);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import from Purchase (Material)", onclick: (s, e) => ImportFromPurchase()).Get(out purchase);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Add by PO# item (Garment Chargeable)", onclick: (s, e) => AddByPOItem()).Get(out poitem);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Add new Item", onclick: (s, e) => AddNewItem()).Get(out newitem);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Separator();
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record - ", onclick: (s, e) => MenuDelete()).Get(out delete);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail - ", onclick: (s, e) => MenuEdit()).Get(out edit);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Separator();
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Print", onclick: (s, e) => Print()).Get(out print);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Batch Print", onclick: (s, e) => BatchPrint()).Get(out batchprint);
            SetContextMenuStatus(false); //預設先將Context ment設定為disable
        }

        //設定Context Menu的Enable/Disable
        private void SetContextMenuStatus(bool status)
        {
            focpl.Enabled = status;
            purchase.Enabled = status;
            poitem.Enabled = status;
            newitem.Enabled = status;
            delete.Enabled = status;
            edit.Enabled = status;
            print.Enabled = status;
            batchprint.Enabled = status;
        }

        //Context Menu選擇Import from FOC PL# (Garment FOC)
        private void ImportFromFOCPL()
        {
            Sci.Production.Shipping.P02_ImportFromFOCPackingList callFOCPLForm = new Sci.Production.Shipping.P02_ImportFromFOCPackingList(CurrentMaintain);
            callFOCPLForm.ShowDialog(this);
            RenewData();
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
        }

        //Context Menu選擇Import from purchase (Material)
        private void ImportFromPurchase()
        {
            Sci.Production.Shipping.P02_ImportFromPO callPurchaseForm = new Sci.Production.Shipping.P02_ImportFromPO(CurrentMaintain);
            callPurchaseForm.ShowDialog(this);
            RenewData();
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
        }

        //Context Menu選擇Add by PO# item (Garment Chargeable)
        private void AddByPOItem()
        {
            Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
            DataRow dr = ((DataTable)detailgridbs.DataSource).NewRow();
            dr["ID"] = CurrentMaintain["ID"];
            callPOItemForm.SetInsert(dr);
            callPOItemForm.ShowDialog(this);
            RenewData();
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
        }

        //Context Menu選擇Add new Item
        private void AddNewItem()
        {
            Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
            DataRow dr = ((DataTable)detailgridbs.DataSource).NewRow();
            dr["ID"] = CurrentMaintain["ID"];
            callNewItemForm.SetInsert(dr);
            callNewItemForm.ShowDialog(this);
            RenewData();
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
        }

        //Context Menu選擇Edit this Record's detail
        private void MenuEdit()
        {
            if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "1" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "2" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "3")
            {
                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "1")
                {
                    callPOItemForm.Text = "International Air/Express - Import from FOC PL#";
                }
                callPOItemForm.SetUpdate(CurrentDetailData);
                callPOItemForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "4")
            {
                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                callEditPOForm.SetUpdate(CurrentDetailData);
                callEditPOForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "5" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "6" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "7" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "8" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "9")
            {
                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                callNewItemForm.SetUpdate(CurrentDetailData);
                callNewItemForm.ShowDialog(this);
            }
            #region 重新計算ttl Carton Weight
            if (null == CurrentMaintain) return;

            string sqlCmd = string.Format(@"
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
select * from DeleteCtn", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DataTable CTNData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out CTNData);
            if (!result)
            {
                return;
            }

            if (CTNData.Rows.Count > 0)
            {
                IList<string> updateCmds = new List<string>();
                foreach (DataRow dr in CTNData.Rows)
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
where ID = '{0}'", CurrentMaintain["ID"]);

            DBProxy.Current.Execute(null, sqlcmd);
            #endregion
            RenewData();
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
        }

        //Context Menu選擇Delete this Record
        private void MenuDelete()
        {
            if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "1" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "2" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "3")
            {
                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "1")
                {
                    callPOItemForm.Text = "International Air/Express - Import From FOC PL#";
                }
                callPOItemForm.SetDelete(CurrentDetailData);
                callPOItemForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "4")
            {
                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                callEditPOForm.SetDelete(CurrentDetailData);
                callEditPOForm.ShowDialog(this);
            }

            if (MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "5" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "6" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "7" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "8" || MyUtility.Convert.GetString(CurrentDetailData["Category"]) == "9")
            {
                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                callNewItemForm.SetDelete(CurrentDetailData);
                callNewItemForm.ShowDialog(this);
            }
            #region 重新計算vm 體積
            if (null == CurrentMaintain) return;

            string sqlCmd = string.Format(@"
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
select * from DeleteCtn", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DataTable CTNData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out CTNData);
            if (!result)
            {               
                return;
            }

            if (CTNData.Rows.Count > 0)
            {
                IList<string> updateCmds = new List<string>();
                foreach (DataRow dr in CTNData.Rows)
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
            string sqlcmd = string.Format(@"update Express 
set VW= (select isnull(sum((CtnLength*CtnWidth*CtnHeight)/6000),0) as VW from Express_CTNData where id='{0}'),
CTNNW = (select isnull(sum(CTNNW),0) as CTNNW from Express_CTNData where id='{0}')
where id='{0}' ", CurrentMaintain["ID"]);
            DBProxy.Current.Execute(null, sqlcmd);
            #endregion

            RenewData();
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
        }

        //Context Menu選擇Print
        private void Print()
        {
            DualResult result;
            IReportResource reportresource;
            ReportDefinition rd = new ReportDefinition();
            if (!(result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(GetType()), GetType(), "P02_DetailPrint.rdlc", out reportresource)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
            else
            {
                rd.ReportResource = reportresource;
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("barCode", '*' + MyUtility.Convert.GetString(CurrentDetailData["ID"]) + MyUtility.Convert.GetString(CurrentDetailData["OrderID"]) + MyUtility.Convert.GetString(CurrentDetailData["Seq1"]) + MyUtility.Convert.GetString(CurrentDetailData["Seq2"]) + MyUtility.Convert.GetString(CurrentDetailData["Category"]) + '*'));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("from", (MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "1" ? "Factory" : "Brand") + "(" + MyUtility.Convert.GetString(CurrentMaintain["FromSite"]) + ")"));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("to", (MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "1" ? "SCI" : MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "2" ? "Factory" : MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "3" ? "Supplier" : "Brand") + "(" + MyUtility.Convert.GetString(CurrentMaintain["ToSite"]) + ")"));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("hcNo", MyUtility.Convert.GetString(CurrentDetailData["ID"])));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("serialNo", MyUtility.Convert.GetString(CurrentDetailData["OrderID"]) + "-" + MyUtility.Convert.GetString(CurrentDetailData["Seq1"]) + "-" + MyUtility.Convert.GetString(CurrentDetailData["Seq2"])));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("brand", MyUtility.Convert.GetString(CurrentDetailData["BrandID"])));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("incharge", MyUtility.GetValue.Lookup(string.Format("select Name+ iif(ExtNo <> '',' #' + ExtNo,'') as Incharge from Pass1 where ID = '{0}'", MyUtility.Convert.GetString(CurrentDetailData["InCharge"])))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("teamLeader", MyUtility.Convert.GetString(CurrentDetailData["LeaderName"])));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("receiver", MyUtility.Convert.GetString(CurrentDetailData["ReceiverName"])));
                using (var frm = new Sci.Win.Subs.ReportView(rd))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        //Context Menu選擇Batch Print
        private void BatchPrint()
        {
            Sci.Production.Shipping.P02_BatchPrint callPurchaseForm = new Sci.Production.Shipping.P02_BatchPrint(CurrentMaintain);
            callPurchaseForm.ShowDialog(this);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            displayFrom.Value = MyUtility.GetValue.Lookup("NameEN", MyUtility.Convert.GetString(CurrentMaintain["FromSite"]), "Brand", "ID");
            displayTO.Value = "";
            if (MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "3")
            {
                displayTO.Value = MyUtility.GetValue.Lookup("AbbEN", MyUtility.Convert.GetString(CurrentMaintain["ToSite"]), "Supp", "ID");
            }
            else
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "4")
                {
                    displayTO.Value = MyUtility.GetValue.Lookup("NameEN", MyUtility.Convert.GetString(CurrentMaintain["ToSite"]), "Brand", "ID");
                }
            }
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
            displayCarrier.Value = MyUtility.GetValue.Lookup(string.Format("select c.SuppID + '-' + s.AbbEN from Carrier c WITH (NOLOCK) left join Supp s WITH (NOLOCK) on c.SuppID = s.ID where c.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["CarrierID"])));
            if (MyUtility.Check.Empty(CurrentMaintain["StatusUpdateDate"]))
            {
                displayStatupdate.Value = null;
            }
            else
            {
                displayStatupdate.Value = Convert.ToDateTime(CurrentMaintain["StatusUpdateDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SendDate"]))
            {
                displaySendtoSCI.Value = null;
            }
            else
            {
                displaySendtoSCI.Value = Convert.ToDateTime(CurrentMaintain["SendDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }

            btnMailto.Enabled = !EditMode && (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Sent" || MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Approved") && (PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Manager"])));

            if (CurrentDetailData == null)
            {
                //如果狀態是Junk的話，Context Menu要Disable
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Junked")
                {
                    SetContextMenuStatus(false);
                }
                else
                {
                    //先將Menu狀態全打開
                    SetContextMenuStatus(true);

                    delete.Enabled = false;
                    edit.Enabled = false;
                    print.Enabled = false;
                    batchprint.Enabled = false;
                }
            }
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select ed.*,p.Refno,ed.SuppID+'-'+isnull(s.AbbEN,'') as Supplier,ec.CTNNW,dbo.getMtlDesc(ed.OrderID,ed.Seq1,ed.Seq2,1,0) as MtlDesc,
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
from Express_Detail ed WITH (NOLOCK) 
left join PO_Supp_Detail p WITH (NOLOCK) on ed.OrderID = p.ID and ed.Seq1 = p.SEQ1 and ed.Seq2 = p.SEQ2
left join Supp s WITH (NOLOCK) on ed.SuppID = s.ID
left join Express_CTNData ec WITH (NOLOCK) on ed.ID = ec.ID and ed.CTNNo = ec.CTNNo
where ed.ID = '{0}'
Order by ed.CTNNo,ed.Seq1,ed.Seq2", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

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
                            DataRow dr = detailgrid.GetDataRow<DataRow>(e.RowIndex);

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

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: orderid)
                .Text("Seq1", header: "Seq1#", width: Widths.AnsiChars(3))
                .Text("Seq2", header: "Seq2#", width: Widths.AnsiChars(2))
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15))
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(15))
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15))
                .Text("CTNNo", header: "C/No.", width: Widths.AnsiChars(5))
                .Numeric("NW", header: "N.W.", width: Widths.AnsiChars(10), decimal_places: 2)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(10), decimal_places: 4)
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(7), decimal_places: 2)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(5))
                .Text("CategoryName", header: "Category", width: Widths.AnsiChars(14))
                .Text("DutyNo", header: "AirPP#/FOC PL#", width: Widths.AnsiChars(13))
                .Text("InChargeName", header: "In Charge", width: Widths.AnsiChars(25))
                .Text("ReceiverName", header: "Receiver", width: Widths.AnsiChars(13))
                .Text("LeaderName", header: "Team Leader", width: Widths.AnsiChars(30))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(30))
                .EditText("Description", header: "Description", width: Widths.AnsiChars(30))
                .Text("Dimension", header: "Dimension (cm)", width: Widths.AnsiChars(20))
                .Numeric("CTNNW", header: "Carton Weight", width: Widths.AnsiChars(10), decimal_places: 2);
        }

        protected override void OnDetailGridRowChanged()
        {
            base.OnDetailGridRowChanged();
            editDescription.Text = "";

            if (CurrentDetailData != null)
            {
                if (MyUtility.Check.Empty(CurrentDetailData["OrderID"]) || MyUtility.Check.Empty(CurrentDetailData["Seq2"]))
                {
                    editDescription.Text = MyUtility.Convert.GetString(CurrentDetailData["Description"]);
                }
                else
                {
                    editDescription.Text = MyUtility.Convert.GetString(CurrentDetailData["MtlDesc"]);
                }

                //先將Menu狀態全打開
                SetContextMenuStatus(true);

                //只有HC的Handle或此項次的申請人才可以刪除及修改
                bool authority = PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentDetailData["InCharge"]));
                delete.Enabled = authority;
                edit.Enabled = authority;
                //Approve後就不可以再做任何動作(更改Context Menu Item的Enable/Disable)
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Approved")
                {
                    focpl.Enabled = false;
                    purchase.Enabled = false;
                    poitem.Enabled = false;
                    newitem.Enabled = false;
                    delete.Enabled = false;
                    edit.Enabled = false;
                }
                //修改顯示內容
                delete.Text = string.Format("Delete this Record - {0} {1}-{2}", MyUtility.Convert.GetString(CurrentDetailData["OrderID"]), MyUtility.Convert.GetString(CurrentDetailData["Seq1"]), MyUtility.Convert.GetString(CurrentDetailData["Seq2"]));
                edit.Text = string.Format("Edit this Record's detail - {0} {1}-{2}", MyUtility.Convert.GetString(CurrentDetailData["OrderID"]), MyUtility.Convert.GetString(CurrentDetailData["Seq1"]), MyUtility.Convert.GetString(CurrentDetailData["Seq2"]));
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            CurrentMaintain["Manager"] = MyUtility.GetValue.Lookup("Supervisor", Sci.Env.User.UserID, "Pass1", "ID");
            CurrentMaintain["NW"] = 0;
            CurrentMaintain["CTNNW"] = 0;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            txtCarrier.ReadOnly = true; //因為Key Down事件，如果按Delete or Backspace按鍵會真的將字元給移除，如果跳出視窗後按Cancel的話，資料會不正確，所以就把此欄位設定為ReadOnly
        }

        protected override bool ClickEditBefore()
        {
            if (!(MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "New" || MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Sent"))
            {
                MyUtility.Msg.WarningBox("Status is not 'New' or 'Sent', can't modify!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "New")
            {
                txtTO.ReadOnly = MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "1" ? true : false;
                txtCarrier.ReadOnly = true;
            }
            else
            {
                txtFrom.ReadOnly = true;
                txtTO.ReadOnly = true;
                txtShipMark.ReadOnly = true;
                txtPort.ReadOnly = true;
                txtCarrier.ReadOnly = true;
                txtBLNo.ReadOnly = true;
                comboFrom.ReadOnly = true;
                comboTO.ReadOnly = true;
                txtUserHandle.TextBox1.ReadOnly = true;
                txtUserManager.TextBox1.ReadOnly = true;
                dateShipDate.ReadOnly = true;
                dateETD.ReadOnly = true;
                dateETA.ReadOnly = true;
                editRemark.ReadOnly = true;
                txtCountryDestination.TextBox1.ReadOnly = true;

            }
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["FromSite"]))
            {
                MyUtility.Msg.WarningBox("From (Site) can't empty");
                txtFrom.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ToSite"]))
            {
                MyUtility.Msg.WarningBox("To (Site) can't empty");
                txtTO.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                MyUtility.Msg.WarningBox("Destination can't empty");
                txtCountryDestination.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PortAir"]))
            {
                MyUtility.Msg.WarningBox("Port can't empty");
                txtPort.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipDate"]))
            {
                MyUtility.Msg.WarningBox("Ship. Date can't empty");
                dateShipDate.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ETD"]))
            {
                MyUtility.Msg.WarningBox("ETD can't empty");
                dateETD.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ETA"]))
            {
                MyUtility.Msg.WarningBox("ETA can't empty");
                dateETA.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["CarrierID"]))
            {
                MyUtility.Msg.WarningBox("Carrier can't empty");
                txtCarrier.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("B/L No. can't empty");
                txtBLNo.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty");
                txtUserHandle.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Manager"]))
            {
                MyUtility.Msg.WarningBox("Manager can't empty");
                txtUserManager.TextBox1.Focus();
                return false;
            }
            #endregion

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "HC", "Express", Convert.ToDateTime(CurrentMaintain["ShipDate"]), 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
            base.ClickSaveAfter();
        }

        protected override bool ClickPrint()
        {
            Sci.Production.Shipping.P02_Print callPurchaseForm = new Sci.Production.Shipping.P02_Print(CurrentMaintain,(DataTable)detailgridbs.DataSource);
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //From
        private void comboFrom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CurrentMaintain["FromTag"] = comboFrom.SelectedValue;
            CurrentMaintain["FromSite"] = "";
            displayFrom.Value = "";
            CurrentMaintain["CarrierID"] = "";
            displayCarrier.Value = "";
            CurrentMaintain["ExpressACNo"] = "";
        }

        //From Site
        private void txtFrom_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["FromTag"]))
            {
                return;
            }

            string sqlCmd = "";
            Sci.Win.Tools.SelectItem item;
            if (MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "1")
            {
                sqlCmd = "select ID from Factory WITH (NOLOCK) where Junk = 0 and ExpressGroup <> ''";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", txtFrom.Text);
            }
            else
            {
                sqlCmd = "select ID,NameEN from Brand WITH (NOLOCK) where Junk = 0";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,50", txtFrom.Text);
            }

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            CurrentMaintain["FromSite"] = item.GetSelectedString();
            if (MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "2")
            {
                IList<DataRow> brand = item.GetSelecteds();
                displayFrom.Value = MyUtility.Convert.GetString(brand[0]["NameEN"]);
            }
        }

        //From Site
        private void txtFrom_Validating(object sender, CancelEventArgs e)
        {
            if (txtFrom.OldValue != txtFrom.Text)
            {
                if (MyUtility.Check.Empty(txtFrom.Text))
                {
                    CurrentMaintain["FromSite"] = "";
                    displayFrom.Value = "";
                }
                else
                {
                    //sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", txtFrom.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);

                    if (MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "1")
                    {
                        DataTable FactoryData;
                        string sqlCmd = "select ID from Factory WITH (NOLOCK) where Junk = 0 and ExpressGroup <> '' and ID = @id";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out FactoryData);
                        if (!result || FactoryData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("Factory: {0} does not exist.", txtFrom.Text));
                            }
                            CurrentMaintain["FromSite"] = "";
                            displayFrom.Value = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            CurrentMaintain["FromSite"] = txtFrom.Text;
                            CurrentMaintain["ShipMark"] = txtFrom.Text;
                            displayFrom.Value = "";
                        }
                    }
                    else
                    {
                        if (MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "2")
                        {
                            DataTable BrandData;
                            string sqlCmd = "select NameEN from Brand WITH (NOLOCK) where Junk = 0 and ID = @id";
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out BrandData);

                            if (!result || BrandData.Rows.Count <= 0)
                            {
                                if (!result)
                                {
                                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                                }
                                else
                                {
                                    MyUtility.Msg.WarningBox(string.Format("Brand: {0} does not exist.", txtFrom.Text));
                                }

                                CurrentMaintain["FromSite"] = "";
                                displayFrom.Value = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                CurrentMaintain["FromSite"] = txtFrom.Text;
                                displayFrom.Value = MyUtility.Convert.GetString(BrandData.Rows[0]["NameEN"]);
                            }
                        }
                    }
                }
                GetCarrier();
            }
        }

        //To
        private void comboTO_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CurrentMaintain["ToTag"] = comboTO.SelectedValue;
            CurrentMaintain["ToSite"] = MyUtility.Convert.GetString(comboTO.SelectedValue) == "1" ? "SCI" : "";
            displayTO.Value = "";
            CurrentMaintain["CarrierID"] = "";
            displayCarrier.Value = "";
            CurrentMaintain["ExpressACNo"] = "";
            txtTO.ReadOnly = MyUtility.Convert.GetString(comboTO.SelectedValue) == "1" ? true : false;
            if (MyUtility.Convert.GetString(comboTO.SelectedValue) == "1")
            {
                GetCarrier();
            }
        }

        //To Site
        private void txtTO_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ToTag"]))
            {
                return;
            }

            string sqlCmd = "";
            Sci.Win.Tools.SelectItem item;
            if (CurrentMaintain["ToTag"].ToString() == "2")
            {
                sqlCmd = "select ID from SCIFty WITH (NOLOCK) where Junk = 0 AND ExpressGroup <> ''";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", txtTO.Text);
            }
            else
            {
                if (CurrentMaintain["ToTag"].ToString() == "3")
                {
                    sqlCmd = "select ID,AbbCH,AbbEN from Supp WITH (NOLOCK) where Junk = 0";
                    item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,20,20", txtTO.Text);
                }
                else
                {
                    sqlCmd = "select ID,NameEN,CountryID from Brand WITH (NOLOCK) where Junk = 0";
                    item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,20,0", txtTO.Text);
                }
            }

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            CurrentMaintain["ToSite"] = item.GetSelectedString();
            IList<DataRow> brand = item.GetSelecteds();
            if (CurrentMaintain["ToTag"].ToString() == "3")
            {
                displayTO.Value = brand[0]["AbbEN"].ToString();
            }
            if (CurrentMaintain["ToTag"].ToString() == "4")
            {
                displayTO.Value = brand[0]["NameEN"].ToString();
                CurrentMaintain["Dest"] = brand[0]["CountryID"].ToString();
            }
        }

        //To Site
        private void txtTO_Validating(object sender, CancelEventArgs e)
        {
            if (txtTO.OldValue != txtTO.Text)
            {
                if (MyUtility.Check.Empty(txtTO.Text))
                {
                    CurrentMaintain["ToSite"] = "";
                    CurrentMaintain["PortAir"] = "";
                    displayTO.Value = "";
                }
                else
                {
                    //sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", txtTO.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    if (MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "2")
                    {
                        DataTable SCIFtyData;
                        string sqlCmd = "select ID,ExpressGroup,CountryID,PortAir from SCIFty WITH (NOLOCK) where Junk = 0 AND ExpressGroup <> '' and ID = @id";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out SCIFtyData);

                        if (!result || SCIFtyData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("Factory: {0} does not exist.", txtTO.Text));
                            }
                            CurrentMaintain["ToSite"] = "";
                            CurrentMaintain["PortAir"] = "";
                            displayTO.Value = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            CurrentMaintain["ToSite"] = txtTO.Text;
                            CurrentMaintain["PortAir"] = MyUtility.Convert.GetString(SCIFtyData.Rows[0]["PortAir"]);
                            displayTO.Value = "";
                            if (MyUtility.Check.Empty(CurrentMaintain["ShipMark"]))
                            {
                                CurrentMaintain["ShipMark"] = txtTO.Text;
                                CurrentMaintain["Dest"] = MyUtility.Convert.GetString(SCIFtyData.Rows[0]["CountryID"]);
                            }
                        }
                    }
                    else
                    {
                        if (MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "3")
                        {
                            DataTable SuppData;
                            string sqlCmd = "select AbbEN from Supp WITH (NOLOCK) where Junk = 0 and ID = @id";
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out SuppData);

                            if (!result || SuppData.Rows.Count <= 0)
                            {
                                if (!result)
                                {
                                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                                }
                                else
                                {
                                    MyUtility.Msg.WarningBox(string.Format("Supplier: {0} does not exist.", txtTO.Text));
                                }
                                CurrentMaintain["ToSite"] = "";
                                CurrentMaintain["PortAir"] = "";
                                displayTO.Value = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                CurrentMaintain["ToSite"] = txtTO.Text;
                                displayTO.Value = MyUtility.Convert.GetString(SuppData.Rows[0]["AbbEN"]);
                                CurrentMaintain["PortAir"] = "";
                            }
                        }
                        else
                        {
                            if (MyUtility.Convert.GetString(CurrentMaintain["ToTag"]) == "4")
                            {
                                DataTable BrandData;
                                string sqlCmd = "select NameEN,CountryID from Brand WITH (NOLOCK) where Junk = 0 and ID = @id";
                                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out BrandData);

                                if (!result || BrandData.Rows.Count <= 0)
                                {
                                    if (!result)
                                    {
                                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                                    }
                                    else
                                    {
                                        MyUtility.Msg.WarningBox(string.Format("Brand: {0} does not exist.", txtTO.Text));
                                    }
                                    CurrentMaintain["ToSite"] = "";
                                    CurrentMaintain["PortAir"] = "";
                                    displayTO.Value = "";
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    CurrentMaintain["ToSite"] = txtTO.Text;
                                    displayTO.Value = MyUtility.Convert.GetString(BrandData.Rows[0]["NameEN"]);
                                    CurrentMaintain["Dest"] = MyUtility.Convert.GetString(BrandData.Rows[0]["CountryID"]);
                                    CurrentMaintain["PortAir"] = "";
                                }
                            }
                        }
                    }
                }
                GetCarrier();
            }
        }

        private void GetCarrier()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["FromTag"]) && MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "1" && !MyUtility.Check.Empty(CurrentMaintain["FromSite"]) && !MyUtility.Check.Empty(CurrentMaintain["ToSite"]))
            {
                string fromSite = "", toSite = "", fromCountry = "", toCountry = "";
                fromSite = MyUtility.Convert.GetString(CurrentMaintain["FromSite"]);
                fromCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from Factory WITH (NOLOCK) where ID = '{0}'", fromSite));
                toSite = MyUtility.Convert.GetString(CurrentMaintain["ToSite"]);
                switch (MyUtility.Convert.GetString(CurrentMaintain["ToTag"]))
                {
                    case "1":
                        toCountry = "TW";
                        break;
                    case "2":
                        toCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from SCIFty WITH (NOLOCK) where ID = '{0}'", toSite));
                        break;
                    case "3":
                        toCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from Supp WITH (NOLOCK) where ID = '{0}'", toSite));
                        break;
                    case "4":
                        toCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from Brand WITH (NOLOCK) where ID = '{0}'", toSite));
                        break;
                }


                string sqlCmd = string.Format(@"declare @1st varchar(4),@2nd varchar(4),@3rd varchar(4),@4th varchar(4),@5th varchar(4),@6th varchar(4) ;
select @1st=ID from Carrier_FtyRule where FromSite = '{0}' and ToSite = '{2}';
select @2nd=ID from Carrier_FtyRule where FromSite = '{0}' and ToCountry = '{3}';
select @3rd=ID from Carrier_FtyRule where FromCountry = '{1}' and ToSite = '{2}';
select @4th=ID from Carrier_FtyRule where FromCountry = '{1}' and ToCountry = '{3}';
select @5th=ID from Carrier_FtyRule where FromSite = '{0}';
select @6th=ID from Carrier_FtyRule where FromCountry = '{1}';

select c.ID,c.Account,c.SuppID,isnull(s.AbbEN,'') as Abb from Carrier c WITH (NOLOCK) 
left join Supp s WITH (NOLOCK) on c.SuppID = s.ID
where c.ID = (select iif(@1st is null,(iif(@2nd is null,iif(@3rd is null,iif(@4th is null,iif(@5th is null,@6th,@5th),@4th),@3rd),@2nd)),@1st));",
                                                                                                                          fromSite, fromCountry, toSite, toCountry);
                DataTable CarrierData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out CarrierData);
                if (result && CarrierData.Rows.Count > 0)
                {
                    CurrentMaintain["CarrierID"] = CarrierData.Rows[0]["ID"];
                    CurrentMaintain["ExpressACNo"] = CarrierData.Rows[0]["Account"];
                    displayCarrier.Value = MyUtility.Convert.GetString(CarrierData.Rows[0]["SuppID"]) + " " + MyUtility.Convert.GetString(CarrierData.Rows[0]["Abb"]);
                }
                else
                {
                    CurrentMaintain["CarrierID"] = "";
                    CurrentMaintain["ExpressACNo"] = "";
                    displayCarrier.Value = "";
                }
            }
        }

        //ETD
        private void dateETD_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(dateETD.Value) && dateETD.OldValue != dateETD.Value)
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ETA"]) && dateETD.Value > MyUtility.Convert.GetDate(CurrentMaintain["ETA"]))
                {
                    MyUtility.Msg.WarningBox("ETD can't later than ETA!");
                    CurrentMaintain["ETD"] = DBNull.Value;
                    e.Cancel = true;
                    return;
                }
            }
        }

        //ETA
        private void dateETA_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(dateETA.Value) && dateETA.OldValue != dateETA.Value)
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ETD"]))
                {
                    if (dateETA.Value < MyUtility.Convert.GetDate(CurrentMaintain["ETD"]))
                    {
                        MyUtility.Msg.WarningBox("ETA can't early than ETD!");
                        CurrentMaintain["ETA"] = DBNull.Value;
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        if (dateETA.Value > Convert.ToDateTime(CurrentMaintain["ETD"]).AddDays(90))
                        {
                            MyUtility.Msg.WarningBox("ETA can't later than ETD more than 90 days!");
                            CurrentMaintain["ETA"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        //Carrier
        private void txtCarrier_KeyDown(object sender, KeyEventArgs e)
        {
            if (EditMode)
            {
                CarrierPopup();
            }
        }

        //Carrier
        private void txtCarrier_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            CarrierPopup();
        }

        //Carrier按右鍵帶出的資料
        private void CarrierPopup()
        {
            string sqlCmd = @"select c.ID,c.SuppID,isnull(s.AbbEN,'') as Abb,c.Account
from Carrier c WITH (NOLOCK) 
left join Supp s WITH (NOLOCK) on c.SuppID = s.ID";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "5,8,20,20", txtCarrier.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            IList<DataRow> carrier = item.GetSelecteds();
            CurrentMaintain["CarrierID"] = item.GetSelectedString();
            displayCarrier.Value = MyUtility.Convert.GetString(carrier[0]["SuppID"]) + " " + MyUtility.Convert.GetString(carrier[0]["Abb"]);
            CurrentMaintain["ExpressACNo"] = carrier[0]["Account"];
        }

        //B/L No.
        private void txtBLNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(txtBLNo) && txtBLNo.OldValue != txtBLNo.Text)
            {
                for (int i = 0; i < txtBLNo.Text.Trim().Length; i++)
                {
                    var asc = ASCIIEncoding.ASCII.GetBytes(txtBLNo.Text.Substring(i, 1));
                    if (!((asc[0] >= 48 && asc[0] <= 57) || (asc[0] >= 65 && asc[0] <= 90)))
                    {
                        MyUtility.Msg.WarningBox("B/L No. format error, only keyin (0-9,A-Z)!");
                        CurrentMaintain["BLNo"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //Carton Dimension && Weight
        private void btnCartonDimensionWeight_Click(object sender, EventArgs e)
        {
            if (null == CurrentMaintain) return;

            string sqlCmd = string.Format(@"with NewCtn
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
select * from DeleteCtn", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DataTable CTNData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out CTNData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query Ctn data fail, please try again.\r\n" + result.ToString());
                return;
            }

            if (CTNData.Rows.Count > 0)
            {
                IList<string> updateCmds = new List<string>();
                foreach (DataRow dr in CTNData.Rows)
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
(MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "New" || MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Send") && (PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Manager"]))), MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null);
                callNextForm.ShowDialog(this);
                this.RenewData();
                numericBox4.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["NW"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["CTNNW"]);
          


        }

        //Send
        protected override void ClickSend()
        {
            base.ClickSend();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to send.");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("B/L No. can't empty!!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Send > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            SendMail();
            string updateCmd = string.Format("update Express set Status = 'Sent', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
           
        }           

        //Recall
        protected override void ClickRecall()
        {
            base.ClickRecall();
            string updateCmd = string.Format("update Express set Status = 'New', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Recall data faile.\r\n" + result.ToString());
                return;
            }
           
        }

        //Junk
        protected override void ClickJunk()
        {
            base.ClickJunk();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to Junk.");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Junk > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update Express set Status = 'Junk', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            updateCmds.Add(string.Format("update PackingList set ExpressID = '' where ExpressID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Junk data faile.\r\n" + result.ToString());
                return;
            }
            SendMail();
           
        }

        //Approve
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            RenewData();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to approve.");
                return;
            }
            if (!MyUtility.Check.Seek(string.Format("select ID from Express_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("There's no detail data, don't need to approve.");
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["NW"]))
            {
                MyUtility.Msg.WarningBox("< ttl N.W. > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("< B/L No. > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["FtyInvNo"]))
            {
                MyUtility.Msg.WarningBox("< Invoice No. > can't empty!");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_Detail WITH (NOLOCK) where ID = '{0}' and NW <= 0", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("N.W. detail data can't be 0.");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_Detail WITH (NOLOCK) where ID = '' and Qty <= 0", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("Q'ty detail data can't be 0.");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_CTNData WITH (NOLOCK) where ID = '{0}' and (CtnLength <= 0 or CtnWidth <= 0 or CtnHeight <= 0 or CTNNW <= 0)", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("Carton Dimension & Weight data can't empty!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Approve > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string updateCmd = string.Format("update Express set Status = 'Approved', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Approve data faile.\r\n" + result.ToString());
                return;
            }
            
        }

        //Unapprove
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!(PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Handle"])) || PublicPrg.Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["Manager"]))))
            {
                MyUtility.Msg.WarningBox("You don't have permission to unapprove.");
                return;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["PayDate"]))
            {
                MyUtility.Msg.WarningBox("Already have payment, can't unapprove!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unapprove > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string updateCmd = string.Format("update Express set Status = 'Sent', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unapprove data faile.\r\n" + result.ToString());
                return;
            }
            
        }

        // Mail to
        private void btnMailto_Click(object sender, EventArgs e)
        {
            SendMail();
        }

        // Mail to
        private void SendMail()
        {
            DataRow dr;
            if (MyUtility.Check.Seek("select * from MailTo WITH (NOLOCK) where ID = '003'", out dr))
            {
                string mailto = MyUtility.Convert.GetString(dr["ToAddress"]);
                string cc = MyUtility.Convert.GetString(dr["CcAddress"]);
                string subject = string.Format("<{0}-{1}> TO <{2}{3}> HC#({4}) International Express ({5}){6}",
                    MyUtility.Convert.GetString(CurrentMaintain["FromSite"]), MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "1" ? MyUtility.Convert.GetString(CurrentMaintain["FromSite"]) : MyUtility.Convert.GetString(displayFrom.Value),
                    MyUtility.Convert.GetString(CurrentMaintain["ToSite"]), MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "3" ? "-" + MyUtility.Convert.GetString(displayTO.Value) : "",
                    MyUtility.Convert.GetString(CurrentMaintain["ID"]),
                    MyUtility.Check.Empty(CurrentMaintain["ShipDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)),
                    MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Junk" ? " -Cancel" : "");
                StringBuilder content = new StringBuilder();
                #region 組Content
                content.Append(string.Format(@"< {0} > will have EXPRESS shipment from {1}-{2} TO {3}{4} ON {5} 

If anyone who wants to follow this shipment, must fill the detail & sp# in this HC#. And team leader need to approve. 
When first applicant's team leader approval, anyone can not do any modification. Please noted.


---Pls notice the Qty & G.W must coincide w/ shipping document. If found out a great inconsistency between system & document then application team have to be deputy of all express application for one month.
---If the goods belong to BULK ORDER, pls must fill in the system with ICR# or Debit note#.
---Applicant need to notify factory & supplier's attn. to arrange the shipping time by oneself before export the goods.
---Applicant also need to notify factory , supplier & customer's attn. to make sure that they can receive the goods by oneself after shipped out  the goods.
", MyUtility.Convert.GetString(CurrentMaintain["Manager"]) + "-" + MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Manager"]))),
 MyUtility.Convert.GetString(CurrentMaintain["FromSite"]), MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "1" ? MyUtility.Convert.GetString(CurrentMaintain["FromSite"]) : MyUtility.Convert.GetString(displayFrom.Value),
 MyUtility.Convert.GetString(CurrentMaintain["ToSite"]), MyUtility.Convert.GetString(CurrentMaintain["FromTag"]) == "3" ? "-" + MyUtility.Convert.GetString(displayTO.Value) : "",
 MyUtility.Check.Empty(CurrentMaintain["ShipDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat))));
                #endregion

                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, cc, subject, "", content.ToString(), false, false);
                email.ShowDialog(this);
            }
        }
    }
}
