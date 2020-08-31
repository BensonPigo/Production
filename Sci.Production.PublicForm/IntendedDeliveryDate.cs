using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// IntendedDeliveryDate
    /// </summary>
    public partial class IntendedDeliveryDate : Sci.Win.Tems.QueryForm
    {
        private string orderID;

        /// <summary>
        /// IntendedDeliveryDate
        /// </summary>
        /// /// <param name="orderID">orderID</param>
        /// <param name="canEdit">canEdit</param>
        public IntendedDeliveryDate(string orderID, bool canEdit)
        {
            this.InitializeComponent();
            this.EditMode = false;

            this.orderID = orderID;
            if (!canEdit)
            {
                this.btnEdit.Visible = false;
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                DualResult result = this.SaveData();
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.EditMode = false;
                this.btnEdit.Text = "Edit";
                this.QueryData();
            }
            else
            {
                this.EditMode = true;
                this.btnEdit.Text = "Save";
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridIDD)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Qty", header: "Total Q'ty", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .EditText("Invoice", header: "Invoice#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .CheckBox("AutoLock", header: "Auto Lock", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(4), iseditable: false)
                .Date("IDD", header: "Intended" + Environment.NewLine + "Delivery", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("IDDEditName", header: "Edit By", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("IDDEditDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true);
            this.QueryData();
        }

        private void QueryData()
        {
            string sqlGetData = $@"
select  oqs.Id,
        oqs.Seq,
        oqs.ShipModeID,
        oqs.BuyerDelivery,
        oqs.Qty,
        [Invoice] = Invoice.val,
        [AutoLock] = iif(isnull(Invoice.val, '') = '', 0, 1),
        oqs.IDD,
        oqs.IDDEditName,
        oqs.IDDEditDate
from    Order_QtyShip oqs with (nolock)
outer apply (SELECT val =  Stuff((  select  concat( ',',pl.INVNo)   
                                    from PackingList pl with (nolock) 
                                    where   exists(   select 1 
                                                    from PackingList_Detail pld with (nolock) 
                                                    where   pld.ID = pl.ID and
                                                            pld.OrderID = oqs.ID and
                                                            pld.OrderShipmodeSeq = oqs.Seq
                                                ) and
                                            pl.INVNo <> ''
                                    FOR XML PATH('')),1,1,'') ) Invoice
where   oqs.ID = '{this.orderID}'
";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridIDD.DataSource = dtResult;
            this.FormatGrid();
        }

        private void FormatGrid()
        {
            foreach (DataGridViewRow gridRow in this.gridIDD.Rows)
            {
                int autoLock = MyUtility.Convert.GetInt(gridRow.Cells["AutoLock"].Value);
                gridRow.Cells["IDD"].ReadOnly = autoLock == 1 ? true : false;
            }
        }

        private void GridIDD_Sorted(object sender, EventArgs e)
        {
            this.FormatGrid();
        }

        private DualResult SaveData()
        {
            var listUpdateData = ((DataTable)this.gridIDD.DataSource)
                .AsEnumerable().Where(s => s.RowState == DataRowState.Modified && s.CompareDataRowVersionValue("IDD"));

            if (!listUpdateData.Any())
            {
                return new DualResult(true);
            }

            DataTable dtUpdate = listUpdateData.CopyToDataTable();
            string sqlUpdate = $@"
Declare @IDDEditName varchar(10) = '{Env.User.UserID}'
Declare @IDDEditDate datetime = GetDate()

Alter table #tmp alter column Id varchar(13)
Alter table #tmp alter column Seq varchar(2)

select  t.Id, t.Seq
into    #tmpExistsGB
from #tmp t
where exists( select  1 from PackingList pl with (nolock) 
                        where   exists(   select 1 
                                          from PackingList_Detail pld with (nolock) 
                                          where   pld.ID = pl.ID and
                                                  pld.OrderID = t.ID and
                                                  pld.OrderShipmodeSeq = t.Seq
                                      ) and
                                 pl.INVNo <> '')

update  oqs set oqs.IDD = t.IDD, oqs.IDDEditName = @IDDEditName, oqs.IDDEditDate = @IDDEditDate
from Order_QtyShip oqs
inner join #tmp t on t.Id = oqs.ID and t.Seq = oqs.Seq
where   not exists(select 1 from #tmpExistsGB tegb where tegb.Id = oqs.ID and tegb.Seq = oqs.Seq)

select * from   #tmpExistsGB
";
            DataTable dtExistsGB;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtUpdate, "Id,Seq,IDD", sqlUpdate, out dtExistsGB);
            if (!result)
            {
                return result;
            }

            if (dtExistsGB.Rows.Count > 0)
            {
                string msg = $@"Orders – ShipmodeSeq already create in Garment Booking.
{dtExistsGB.AsEnumerable().Select(s => s["Id"].ToString() + " - " + s["Seq"].ToString()).JoinToString(Environment.NewLine)}";
                MyUtility.Msg.WarningBox(msg);
            }

            return result;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
