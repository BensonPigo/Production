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
             string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
             this.DetailSelectCommand = string.Format(@"select ed.ID,isnull(o.FactoryID,'') as FactoryID,ed.PoID,isnull(o.BrandID,'') as BrandID,o.BuyerDelivery,o.SciDelivery,
(left(ed.Seq1+' ',3)+'-'+ed.Seq2) as Seq,(ed.SuppID+'-'+iif(fe.Type = 4,(select Abb from LocalSupp where ID = ed.SuppID),(select AbbEN from Supp where ID = ed.SuppID))) as Supp,
ed.RefNo,isnull(iif(fe.Type = 4,(select Description from LocalItem where RefNo = ed.RefNo),(select DescDetail from Fabric where SCIRefno = ed.SCIRefNo)),'') as Description,
(case when ed.FabricType = 'F' then 'Fabric' when ed.FabricType = 'A' then 'Accessory' else '' end) as Type,ed.FabricType,
ed.MtlTypeID,ed.UnitId,ed.Qty,ed.NetKg,ed.WeightKg,ed.Seq1,ed.Seq2
from FtyExport_Detail ed
left join FtyExport fe on fe.ID = ed.ID
left join Orders o on o.ID = ed.PoID
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
                txtsubcon1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("ShipMode can't empty!!");
                txtshipmode1.Focus();
                return false;
            }

            //提單號碼不可重複
            if (!MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                string sqlCmd = string.Format("select ID from FtyExport where BLNo = '{0}' and ID != '{1}'", CurrentMaintain["BLNo"].ToString(), CurrentMaintain["ID"].ToString());
                if (MyUtility.Check.Seek(sqlCmd))
                {
                    MyUtility.Msg.WarningBox("B/L(AWB) No. already exist.");
                    textBox5.Focus();
                    return false;
                }
            }

            //加總表身欄位回寫表頭
            double nw = 0.0, gw = 0.0;
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    nw = MyUtility.Math.Round(nw + Convert.ToDouble(dr["NetKg"]), 2);
                    gw = MyUtility.Math.Round(gw + Convert.ToDouble(dr["WeightKg"]), 2);
                }
            }
            CurrentMaintain["NetKg"] = nw;
            CurrentMaintain["WeightKg"] = gw;

            if (IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("select RgCode from System").Trim() + "FE", "FtyExport", DateTime.Today, 2, "Id", null);
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
            if (MyUtility.Check.Seek(string.Format(@"select ShippingAPID from ShareExpense where InvNo = '{0}' or WKNO = '{0}'", CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("This record have expense data, can't be deleted!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //Port of Loading按右鍵
        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID,CountryID from Port where Junk = 0";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "20,3", textBox3.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            IList<DataRow> portData;
            portData = item.GetSelecteds();
            CurrentMaintain["ExportPort"] = item.GetSelectedString();
            CurrentMaintain["ExportCountry"] = portData[0]["CountryID"].ToString();
        }

        //Port of Discharge按右鍵
        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID,CountryID from Port where Junk = 0";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "20,3", textBox4.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            IList<DataRow> portData;
            portData = item.GetSelecteds();
            CurrentMaintain["ImportPort"] = item.GetSelectedString();
            CurrentMaintain["ImportCountry"] = portData[0]["CountryID"].ToString();
        }

        //Radio
        private void radioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (radioPanel1.Value == "3")
            {
                label16.Text = "Ship Date";
            }
            else
            {
                label16.Text = "Arrive Port Date";
            }

            if (radioPanel1.Value == "1" || radioPanel1.Value == "2")
            {
                DataTable dt = (DataTable)txtshipmode1.DataSource;
                dt.DefaultView.RowFilter = "UseFunction like '%ORDER%'";
            }
            if (radioPanel1.Value == "3" || radioPanel1.Value == "4")
            {
                DataTable dt = (DataTable)txtshipmode1.DataSource;
                dt.DefaultView.RowFilter = "UseFunction like '%WK%'";
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
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ExpenseData callNextForm = new Sci.Production.Shipping.P05_ExpenseData(CurrentMaintain["ID"].ToString(), CurrentMaintain["Type"].ToString() == "3"?"InvNo":"WKNo");
            callNextForm.ShowDialog(this);
        }

        //Import Data
        private void button2_Click(object sender, EventArgs e)
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
