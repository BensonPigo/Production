using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// P01_BatchUpdateIDD
    /// </summary>
    public partial class P01_BatchUpdateIDD : Sci.Win.Tems.QueryForm
    {
        private DataTable dtIDD;

        private DataTable FilterIDDResult
        {
            get
            {
                if (this.dtIDD == null)
                {
                    return null;
                }

                var filterResult = this.dtIDD.AsEnumerable();
                if (this.chkExcludeExistsGB.Checked)
                {
                    filterResult = this.dtIDD.AsEnumerable().Where(s => MyUtility.Convert.GetInt(s["AutoLock"]) == 0);
                }

                return filterResult.Any() ? filterResult.CopyToDataTable() : null;
            }
        }

        /// <summary>
        /// P01_BatchUpdateIDD
        /// </summary>
        public P01_BatchUpdateIDD()
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridIDD)
                .CheckBox("select", header: "Sel", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(4), iseditable: true)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SewOffLine", header: "Sewing Offline Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Qty", header: "Total Q'ty", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .EditText("Invoice", header: "Invoice#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .CheckBox("AutoLock", header: "Auto Lock", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(4), iseditable: false)
                .Date("IDD", header: "Intended" + Environment.NewLine + "Delivery", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("IDDEditName", header: "Edit By", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("IDDEditDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        private void QueryData()
        {

            if (!this.dateSewingOffline.HasValue &&
                !this.dateBuyerdelivery.HasValue &&
                MyUtility.Check.Empty(this.txtSPFrom.Text) &&
                MyUtility.Check.Empty(this.txtSPTo.Text))
            {
                MyUtility.Msg.WarningBox("Sewing Offline, Buyer Delivery, SP# At least one must be entered");
                return;
            }

            string sqlWhere = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.txtFactory.Text))
            {
                sqlWhere += $" and o.FactoryID = '{this.txtFactory.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                sqlWhere += $" and o.BrandID = '{this.txtBrand.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtStyle.Text))
            {
                sqlWhere += $" and o.StyleID = '{this.txtStyle.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSPFrom.Text))
            {
                sqlWhere += $" and o.ID >= '{this.txtSPFrom.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSPTo.Text))
            {
                sqlWhere += $" and o.ID <= '{this.txtSPTo.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtDest.TextBox1.Text))
            {
                sqlWhere += $" and o.Dest = '{this.txtDest.TextBox1.Text}' ";
            }

            if (this.dateSewingOffline.HasValue)
            {
                sqlWhere += $" and o.SewOffLine between @SewingOfflineFrom and @SewingOfflineTo ";
                listPar.Add(new SqlParameter("@SewingOfflineFrom", this.dateSewingOffline.DateBox1.Value));
                listPar.Add(new SqlParameter("@SewingOfflineTo", this.dateSewingOffline.DateBox2.Value));
            }

            if (this.dateBuyerdelivery.HasValue)
            {
                sqlWhere += $" and oqs.BuyerDelivery between @BuyerdeliveryFrom and @BuyerdeliveryTo ";
                listPar.Add(new SqlParameter("@BuyerdeliveryFrom", this.dateBuyerdelivery.DateBox1.Value));
                listPar.Add(new SqlParameter("@BuyerdeliveryTo", this.dateBuyerdelivery.DateBox2.Value));
            }

            string sqlGetData = $@"
select  [select] = 0,
        oqs.Id,
        o.FactoryID,
        o.BrandID,
        o.StyleID,
        o.SewOffLine,
        c.Alias,
        oqs.Seq,
        oqs.ShipModeID,
        oqs.BuyerDelivery,
        oqs.Qty,
        [Invoice] = Invoice.val,
        [AutoLock] = iif(isnull(Invoice.val, '') = '', 0, 1),
        oqs.IDD,
        oqs.IDDEditName,
        oqs.IDDEditDate
from    Orders o with (nolock)
inner join  Order_QtyShip oqs with (nolock) on o.ID = oqs.ID
left join Country c with (nolock) on c.ID = o.Dest
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
where   o.MDivisionID = '{Env.User.Keyword}' {sqlWhere}
";
            DualResult result = DBProxy.Current.Select(null, sqlGetData, listPar, out this.dtIDD);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtIDD.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found");
            }

            this.gridIDD.DataSource = this.FilterIDDResult;
            this.FormatGrid();
        }

        private void FormatGrid()
        {
            foreach (DataGridViewRow gridRow in this.gridIDD.Rows)
            {
                int autoLock = MyUtility.Convert.GetInt(gridRow.Cells["AutoLock"].Value);
                gridRow.DefaultCellStyle.BackColor = autoLock == 1 ? Color.LightGray : Color.White;
                gridRow.Cells["IDD"].ReadOnly = autoLock == 1 ? true : false;
                gridRow.Cells["select"].ReadOnly = autoLock == 1 ? true : false;
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.QueryData();
        }

        private void BtnUpdateGridIDD_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateIDD.Value))
            {
                return;
            }

            this.gridIDD.ValidateControl();
            DataTable dtGridIDD = (DataTable)this.gridIDD.DataSource;

            foreach (DataRow drIDD in dtGridIDD.Select("select = 1"))
            {
                drIDD["IDD"] = this.dateIDD.Value;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChkExcludeExistsGB_CheckedChanged(object sender, EventArgs e)
        {
            this.gridIDD.DataSource = this.FilterIDDResult;
            this.FormatGrid();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (this.gridIDD.DataSource == null)
            {
                return;
            }

            var listUpdateData = ((DataTable)this.gridIDD.DataSource)
                .AsEnumerable().Where(s => (int)s["select"] == 1);

            if (!listUpdateData.Any())
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
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
                this.ShowErr(result);
                return;
            }

            if (dtExistsGB.Rows.Count > 0)
            {
                string msg = $@"Orders – ShipmodeSeq already create in Garment Booking.
{dtExistsGB.AsEnumerable().Select(s => s["Id"].ToString() + " - " + s["Seq"].ToString()).JoinToString(Environment.NewLine)}";
                MyUtility.Msg.WarningBox(msg);
            }

            this.QueryData();
        }

        private void GridIDD_Sorted(object sender, EventArgs e)
        {
            this.FormatGrid();
        }
    }

}
