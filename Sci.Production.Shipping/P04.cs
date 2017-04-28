﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P04 : Sci.Win.Tems.Input6
    {
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
             string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
             this.DetailSelectCommand = string.Format(@"select ed.*,isnull(o.FactoryID,'') as FactoryID,isnull(o.BrandID,'') as BrandID,o.BuyerDelivery,o.SciDelivery,
(left(ed.Seq1+' ',3)+'-'+ed.Seq2) as Seq,(ed.SuppID+'-'+iif(fe.Type = 4,(select Abb from LocalSupp WITH (NOLOCK) where ID = ed.SuppID),(select AbbEN from Supp WITH (NOLOCK) where ID = ed.SuppID))) as Supp,
ed.RefNo,isnull(iif(fe.Type = 4,(select Description from LocalItem WITH (NOLOCK) where RefNo = ed.RefNo),(select DescDetail from Fabric WITH (NOLOCK) where SCIRefno = ed.SCIRefNo)),'') as Description,
(case when ed.FabricType = 'F' then 'Fabric' when ed.FabricType = 'A' then 'Accessory' else '' end) as Type
from FtyExport_Detail ed WITH (NOLOCK) 
left join FtyExport fe WITH (NOLOCK) on fe.ID = ed.ID
left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
where ed.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FactoryID", header: "Prod. Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Del.", iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Del.", iseditingreadonly: true)
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Import Q'ty", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "N.W.(kg)", decimal_places: 2);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Type"] = 1;
            CurrentMaintain["Handle"] = Sci.Env.User.UserID;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["Forwarder"]))
            {
                MyUtility.Msg.WarningBox("Forwarder can't empty!!");
                txtSubconForwarder.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("ShipMode can't empty!!");
                txtShipmodeShippingMode.Focus();
                return false;
            }

            //提單號碼不可重複
            if (!MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["BLNo"]).IndexOf("'") != -1)
                {
                    MyUtility.Msg.WarningBox("B/L(AWB) No. can not enter the  '  character!!");
                    return false;
                }

                string sqlCmd = string.Format("select ID from FtyExport WITH (NOLOCK) where BLNo = '{0}' and ID != '{1}'", MyUtility.Convert.GetString(CurrentMaintain["BLNo"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                if (MyUtility.Check.Seek(sqlCmd))
                {
                    MyUtility.Msg.WarningBox("B/L(AWB) No. already exist.");
                    txtBLAWBNo.Focus();
                    return false;
                }
            }

            //加總表身欄位回寫表頭
            double nw = 0.0, gw = 0.0;
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    nw = MyUtility.Math.Round(nw + MyUtility.Convert.GetDouble(dr["NetKg"]), 2);
                    gw = MyUtility.Math.Round(gw + MyUtility.Convert.GetDouble(dr["WeightKg"]), 2);
                }
            }
            CurrentMaintain["NetKg"] = nw;
            CurrentMaintain["WeightKg"] = gw;

            if (IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("select RgCode from System WITH (NOLOCK) ").Trim() + "FE", "FtyExport", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = newID;
            }

            return base.ClickSaveBefore();
        }

        protected override bool ClickDeleteBefore()
        {
            //已經有做出口費用分攤就不可以被刪除
            if (MyUtility.Check.Seek(string.Format(@"select ShippingAPID from ShareExpense WITH (NOLOCK) where InvNo = '{0}' or WKNO = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("This record have expense data, can't be deleted!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //Port of Loading按右鍵
        private void txtPortofLoading_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID,CountryID from Port WITH (NOLOCK) where Junk = 0";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "20,3", txtPortofLoading.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            IList<DataRow> portData;
            portData = item.GetSelecteds();
            CurrentMaintain["ExportPort"] = item.GetSelectedString();
            CurrentMaintain["ExportCountry"] = portData[0]["CountryID"];
        }

        //Port of Discharge按右鍵
        private void txtPortofDischarge_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID,CountryID from Port WITH (NOLOCK) where Junk = 0";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "20,3", txtPortofDischarge.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            IList<DataRow> portData;
            portData = item.GetSelecteds();
            CurrentMaintain["ImportPort"] = item.GetSelectedString();
            CurrentMaintain["ImportCountry"] = portData[0]["CountryID"];
        }

        //Radio
        private void radioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (radioPanel1.Value == "3")
            {
                labelArrivePortDate.Text = "Ship Date";
            }
            else
            {
                labelArrivePortDate.Text = "Arrive Port Date";
            }

            if (this.EditMode)
            {
                foreach (DataRow dr in DetailDatas)
                {
                    dr.Delete();
                }
            }
        }

        //Expense Data
        private void btnExpenseData_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ExpenseData callNextForm = new Sci.Production.Shipping.P05_ExpenseData(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["Type"]) == "3"?"InvNo":"WKNo");
            callNextForm.ShowDialog(this);
        }

        //Import Data
        private void btnImportData_Click(object sender, EventArgs e)
        {
            switch (CurrentMaintain["Type"].ToString())
            {
                case "1":
                    Sci.Production.Shipping.P04_Import3rd call3rdForm = new Sci.Production.Shipping.P04_Import3rd((DataTable)detailgridbs.DataSource);
                    call3rdForm.ShowDialog(this);
                    break;
                case "2":
                    Sci.Production.Shipping.P04_ImportTransferIn callTransferInForm = new Sci.Production.Shipping.P04_ImportTransferIn((DataTable)detailgridbs.DataSource);
                    callTransferInForm.ShowDialog(this);
                    break;
                case "3":
                    Sci.Production.Shipping.P04_ImportTransferOut callTransferOutForm = new Sci.Production.Shipping.P04_ImportTransferOut((DataTable)detailgridbs.DataSource);
                    callTransferOutForm.ShowDialog(this);
                    break;
                case "4":
                    Sci.Production.Shipping.P04_ImportLocalPO callLocalPOForm = new Sci.Production.Shipping.P04_ImportLocalPO((DataTable)detailgridbs.DataSource);
                    callLocalPOForm.ShowDialog(this);
                    break;
                default:
                    break;
            }
        }
    }
}
