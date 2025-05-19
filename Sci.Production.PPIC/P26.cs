using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P26 : Win.Tems.QueryForm
    {
        private PPIC_R21_ViewModel model;

        /// <inheritdoc/>
        public P26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("ScanTime", header: "Scan Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Status", header: "Status", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("PackinglistID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Factory", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Dest", header: "Destination", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("SCIDelivery", header: "SCI Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void Find()
        {
            this.listControlBindingSource1.DataSource = null;

            if (MyUtility.Check.Empty(this.txtSP.Text) && MyUtility.Check.Empty(this.txtPO.Text)
                && MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                MyUtility.Msg.WarningBox(@"
Please fill in at least one of the following options:
<SP#>, <PO#>, <Pack ID>, <Buyer Delivery>, <SCI Delivery>
");
                return;
            }

            this.model = new PPIC_R21_ViewModel()
            {
                BuyerDeliveryFrom = this.dateBuyerDelivery.DateBox1.Value,
                BuyerDeliveryTo = this.dateBuyerDelivery.DateBox2.Value,
                SCIDeliveryFrom = this.dateSCIDelivery.DateBox1.Value,
                SCIDeliveryTo = this.dateSCIDelivery.DateBox2.Value,
                MDivisionID = string.Empty,
                ComboProcess = this.txtStatus.Text,
                FactoryID = this.txtfactory.Text,
                PO = this.txtPO.Text,
                OrderID = this.txtSP.Text,
                PackID = this.txtPackID.Text,
                IsBI = false,
                IsPPICP26 = true,
            };

            PPIC_R21 biModel = new PPIC_R21();
            Base_ViewModel resultReport = biModel.GetCartonStatusTrackingList(this.model);
            if (!resultReport.Result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + resultReport.Result.ToString());
                return;
            }

            DataTable dt = resultReport.Dt;

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            DataView view = dt.DefaultView;
            view.Sort = "OrderID ASC, CTNStartNo ASC";  // 或 DESC
            DataTable sortedTable = view.ToTable();

            this.listControlBindingSource1.DataSource = sortedTable;
        }

        private void txtStatus_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            IEnumerable<string> list = PPIC.R21.listProcess;
            list = list.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            SelectItem2 item = new SelectItem2(list, "Status", "30", this.txtStatus.Text);
            item.Width = 455;
            item.Height = 550;
            if (item.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.txtStatus.Text = item.GetSelectedString(); ;
        }
    }
}
