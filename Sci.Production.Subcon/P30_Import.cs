using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P30_Import : Sci.Win.Subs.Base
    {
        DataRow dr_localPO;
        DataTable dt_localPODetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        protected DataTable dtlocal;

        public P30_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_localPO = master;
            dt_localPODetail = detail;
            this.Text += string.Format(" ( Categgory:{0} - Supplier:{1} )", dr_localPO["category"].ToString(), dr_localPO["localsuppid"].ToString());
            this.dateApproveDate.Enabled = dr_localPO["category"].ToString().TrimEnd().ToUpper() == "CARTON";
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            String sp_b = this.txtSPNoStart.Text;
            String sp_e = this.txtSPNoEnd.Text;
            String brandid = this.txtbrand.Text;
            String factory = this.txtfactory1.Text;

            string booking_b, booking_e, sewinline_b, sewinline_e, arrived_b, arrived_e, approved_b, approved_e, scidelivery_b, scidelivery_e, sql, tmp;
            booking_b = null;
            booking_e = null;
            sewinline_b = null;
            sewinline_e = null;
            arrived_b= null;
            arrived_e = null;
            approved_b= null;
            approved_e= null;
            scidelivery_b= null;
            scidelivery_e = null;
            

            if (dateEstBookingDate.Value1 != null) booking_b = this.dateEstBookingDate.Text1;
            if (dateEstBookingDate.Value2 != null) { booking_e = this.dateEstBookingDate.Text2; }
            if (dateSewingInlineDate.Value1 != null) sewinline_b = this.dateSewingInlineDate.Text1;
            if (dateSewingInlineDate.Value2 != null) { sewinline_e = this.dateSewingInlineDate.Text2; }
            if (dateEstArrivedDate.Value1 != null) arrived_b = this.dateEstArrivedDate.Text1;
            if (dateEstArrivedDate.Value2 != null) { arrived_e = this.dateEstArrivedDate.Text2; }
            if (dateApproveDate.Value1 != null) approved_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { approved_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) scidelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { scidelivery_e = this.dateSCIDelivery.Text2; }

            

            if ((MyUtility.Check.Empty(booking_b) && MyUtility.Check.Empty(booking_e)) &&
                (MyUtility.Check.Empty(sewinline_b) && MyUtility.Check.Empty(sewinline_e ))  &&
                (MyUtility.Check.Empty(arrived_b) && MyUtility.Check.Empty(arrived_e)) &&
                (MyUtility.Check.Empty(approved_b) && MyUtility.Check.Empty(approved_e)) &&
                (MyUtility.Check.Empty(scidelivery_b) && MyUtility.Check.Empty(scidelivery_e)) &&
                MyUtility.Check.Empty(sp_b) && MyUtility.Check.Empty(sp_e) && MyUtility.Check.Empty(brandid))
            {
                MyUtility.Msg.WarningBox(@"< Booking Date > or < Sci Delivery > or < Arrived Date > or < Sewing Inline >  
                                                or < Approve Date > or  < SP# > or  < Brand > can't be empty!!");
                txtSPNoStart.Focus();
                return;
            }
            else
            {
                #region 組sql語法
                string strSQLCmd = null;
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                // 建立可以符合回傳的Cursor - Carton
                if (dr_localPO["category"].ToString().TrimEnd().ToUpper() == "CARTON")
                {
                    strSQLCmd = string.Format(@"
select distinct 1 as Selected
       , c.POID 
       , b.OrderID 
       , c.StyleID
       , c.SciDelivery
       , c.SeasonID 
       , b.RefNo 
       , dbo.getitemdesc('{2}',b.refno) as description 
       , '' as threadcolorid
       , sum(b.CTNQty) qty
       , d.UnitID
       , d.Price
       , sum(b.CTNQty) * d.Price as amount
       , [std_price] = round(y.order_amt /iif(y.order_qty=0,1,y.order_qty),3) 
       , '' as remark 
       , a.EstCTNArrive etd 
       , a.ID as requestid
       , '' as id
       , c.FactoryID 
       , c.SewInLine
       , delivery = a.EstCTNArrive
       , br.BuyerID
into #tmp
from dbo.PackingList a WITH (NOLOCK) 
inner join PackingList_Detail b WITH (NOLOCK) on a.ID = b.ID
inner join Orders c WITH (NOLOCK) on b.OrderID = c.ID    
inner join Brand br WITH (NOLOCK) on c.BrandID = br.ID
inner join LocalItem d WITH (NOLOCK) on b.RefNo = d.RefNo
inner join factory WITH (NOLOCK) on c.FactoryID = factory.id
--inner join LocalPO_Detail e WITH (NOLOCK) on c.id=e.OrderId
outer apply(
    select o1.POID
           , isnull(sum(o1.qty),0) order_qty
           , sum(o1.qty*ot.Price) order_amt 
    from orders o1 WITH (NOLOCK) 
    inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = o1.ID 
    where o1.poid = c.poid 
          and ot.ArtworkTypeID = '{2}'
    group by o1.poid
) y
where a.ApvToPurchase = 1 
      and a.LocalPOID = ''
      and d.localsuppid = '{3}'
      --and a.factoryid = '{0}'    
      and a.mdivisionid ='{1}'
      and c.Category  in ('B','S')
      and c.Junk = 0
      and factory.IsProduceFty = 1
", Env.User.Factory
                     , Env.User.Keyword
                     , dr_localPO["category"]
                     ,dr_localPO["localsuppid"]
                     );

                    if (!MyUtility.Check.Empty(sp_b)) { strSQLCmd += " and c.id between @sp1 and @sp2"; }
                    if (!MyUtility.Check.Empty(brandid)) { strSQLCmd += " and c.brandid = @brandid"; }
                    if (!MyUtility.Check.Empty(factory)) { strSQLCmd += $" and a.FactoryID = '{factory}'"; }
                    if (!MyUtility.Check.Empty(sewinline_b)) { strSQLCmd += string.Format(" and c.sewinline >= '{0}' ", sewinline_b); }
                    if (!MyUtility.Check.Empty(sewinline_e)) { strSQLCmd += string.Format(" and c.sewinline <= '{0}' ", sewinline_e); }
                    if (!MyUtility.Check.Empty(scidelivery_b)) { strSQLCmd += string.Format(" and c.scidelivery >= '{0}' ", scidelivery_b); }
                    if (!MyUtility.Check.Empty(scidelivery_e)) { strSQLCmd += string.Format(" and c.scidelivery <= '{0}' ", scidelivery_e); }
                    if (!MyUtility.Check.Empty(booking_b)) { strSQLCmd += string.Format(" and a.EstCTNBooking >= '{0}' ", booking_b); }
                    if (!MyUtility.Check.Empty(booking_e)) { strSQLCmd += string.Format(" and a.EstCTNBooking <= '{0}' ", booking_e); }
                    if (!MyUtility.Check.Empty(arrived_b)) { strSQLCmd += string.Format(" and a.EstCTNArrive >= '{0}' ", arrived_b); }
                    if (!MyUtility.Check.Empty(arrived_e)) { strSQLCmd += string.Format(" and a.EstCTNArrive <= '{0}' ", arrived_e); }
                    if (!MyUtility.Check.Empty(approved_b)) { strSQLCmd += string.Format(" and a.ApvToPurchaseDate >= '{0}' ", approved_b); }
                    if (!MyUtility.Check.Empty(approved_e)) { strSQLCmd += string.Format(" and a.ApvToPurchaseDate <= '{0}' ", approved_e); }

                    strSQLCmd += string.Format(@" group by c.POID,b.OrderID,c.StyleID,c.SeasonID,b.RefNo,d.UnitID,d.Price,a.EstCTNArrive,a.ID,c.FactoryID ,c.SewInLine,c.SciDelivery,y.order_amt,y.order_qty,y.POID,br.BuyerID
select * from #tmp a
where  not exists (select orderID 
                      from LocalPo_Detail 
                      where (RequestID = '' or RequestID = a.RequestID)
                      		and Poid = a.POID 
                      		and OrderID = a.OrderID 
                      		and Refno = a.RefNo
                            and ID !='{0}')
", dr_localPO["ID"]);

                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@sp1";
                    sp1.Value = sp_b;

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@sp2";
                    sp2.Value = sp_e;

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@brandid";
                    sp3.Value = brandid;

                    
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    
                    #endregion
                }
                else
                {
                    string wf = string.Empty;
                    if (!MyUtility.Check.Empty(factory)) { wf = $" and c.FactoryID = '{factory}'"; }
                    strSQLCmd = string.Format(@"
select distinct 1 as Selected
       , c.POID 
       , a.OrderID 
       , a.StyleID
       , c.SciDelivery
       , a.SeasonID 
       , b.RefNo 
       , dbo.getitemdesc('{2}',b.refno) as description 
       , b.threadcolorid
       , b.PurchaseQty as qty
       , d.UnitID
       , [Price] = iif(tc.price is null , iif(tc2.price is null , d.Price,tc2.price) ,tc.price)
       , b.PurchaseQty * iif(tc.price is null , iif(tc2.price is null , d.Price,tc2.price) ,tc.price) as amount 
       , [std_price] = round(y.order_amt /iif(y.order_qty=0,1,y.order_qty),3)
       , '' as remark 
       , a.EstArriveDate etd 
       , a.OrderID as requestid
       , '' as id
       , c.FactoryID 
       , c.SewInLine
       , delivery = a.EstArriveDate
       , br.BuyerID
from dbo.ThreadRequisition a WITH (NOLOCK) 
inner join ThreadRequisition_Detail b WITH (NOLOCK) on a.OrderID = b.OrderID
inner join Orders c WITH (NOLOCK) on b.OrderID = c.ID
inner join Brand br WITH (NOLOCK) on c.BrandID = br.ID
inner join LocalItem d WITH (NOLOCK) on b.RefNo = d.RefNo
inner join factory WITH (NOLOCK) on c.FactoryID = factory.id
left join ThreadColor t WITH (NOLOCK) on t.ID = b.ThreadColorID
left join LocalItem_ThreadBuyerColorGroupPrice tc with (nolock) 
    on tc.refno=b.Refno and tc.ThreadColorGroupID=t.ThreadColorGroupID and tc.BuyerID = br.BuyerID
left join LocalItem_ThreadBuyerColorGroupPrice tc2 with (nolock) 
    on tc2.refno=b.Refno and tc2.ThreadColorGroupID=t.ThreadColorGroupID and tc2.BuyerID = ''
--inner join LocalPO_Detail e WITH (NOLOCK) on c.id=e.OrderId
outer apply(
    select o1.POID
           ,isnull(sum(o1.qty),0) order_qty
           ,sum(o1.qty*ot.Price) order_amt 
    from orders o1 WITH (NOLOCK) 
    inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = o1.ID 
    where o1.poid= c.poid 
          and ot.ArtworkTypeID = '{2}'
    group by o1.poid
) y
where a.status = 'Approved' 
      and factory.IsProduceFty = 1
      --and a.factoryid = '{0}'
      and d.localsuppid= '{3}'
      and a.Mdivisionid = '{1}'
      and c.Category  in ('B','S')
      and exists (select id from orders where poid=b.OrderID and junk=0)
      and b.PurchaseQty > 0 
      and b.PoId = ''
      {5}
      and not exists (select orderID 
      				  from LocalPo_Detail 
      				  where RequestID = a.OrderID
      				  		and Poid = c.POID 
      				  		and OrderID = a.OrderID 
      				  		and Refno = b.RefNo 
      				  		and ThreadColorID = b.threadcolorid
                            and ID !='{4}')", Env.User.Factory
                     , Env.User.Keyword
                     , dr_localPO["category"]
                     , dr_localPO["localsuppid"]
                     , dr_localPO["ID"]
                     , wf);

                    if (!MyUtility.Check.Empty(sp_b)) { strSQLCmd += " and c.id between @sp1 and @sp2"; }
                    if (!MyUtility.Check.Empty(brandid)) { strSQLCmd += " and c.brandid = @brandid"; }
                    if (!MyUtility.Check.Empty(sewinline_b)) { strSQLCmd += string.Format(" and c.sewinline >= '{0}' ", sewinline_b); }
                    if (!MyUtility.Check.Empty(sewinline_e)) { strSQLCmd += string.Format(" and c.sewinline <= '{0}' ", sewinline_e); }
                    if (!MyUtility.Check.Empty(scidelivery_b)) { strSQLCmd += string.Format(" and c.scidelivery >= '{0}' ", scidelivery_b); }
                    if (!MyUtility.Check.Empty(scidelivery_e)) { strSQLCmd += string.Format(" and c.scidelivery <= '{0}' ", scidelivery_e); }
                    if (!MyUtility.Check.Empty(booking_b)) { strSQLCmd += string.Format(" and a.EstBookDate >= '{0}' ", booking_b); }
                    if (!MyUtility.Check.Empty(booking_e)) { strSQLCmd += string.Format(" and a.EstBookDate <= '{0}' ", booking_e); }
                    if (!MyUtility.Check.Empty(arrived_b)) { strSQLCmd += string.Format(" and a.EstArriveDate >= '{0}' ", arrived_b); }
                    if (!MyUtility.Check.Empty(arrived_e)) { strSQLCmd += string.Format(" and a.EstArriveDate <= '{0}' ", arrived_e); }
                    //strSQLCmd += " group by c.POID,b.OrderID,c.StyleID,c.SeasonID,b.RefNo,d.UnitID,d.Price,a.EstArriveDate,a.ID ";

                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@sp1";
                    sp1.Value = sp_b;

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@sp2";
                    sp2.Value = sp_e;

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@brandid";
                    sp3.Value = brandid;


                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    #endregion  
                }
                #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd,cmds, out dtlocal))
                {
                    if (dtlocal.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtlocal;
                }
                else 
                    { ShowErr(strSQLCmd, result); }

                #region 加工remark欄位
                string category,Finished;
                decimal price;
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    category = MyUtility.GetValue.Lookup(string.Format(@"select category from orders WITH (NOLOCK) where id = '{0}'", dr["orderid"]), null);
                    if (category == "B")
                    {
                        sql = string.Format(@"select price from order_tmscost WITH (NOLOCK) where id='{0}' and artworktypeid='{1}'"
                                            , dr["orderid"], dr_localPO["category"].ToString().TrimEnd().ToUpper());
                        tmp = MyUtility.GetValue.Lookup(sql);
                        if (MyUtility.Check.Empty(tmp))
                            price = 0;
                        else
                            price = decimal.Parse(tmp);

                        if (MyUtility.Check.Empty(price) || price == 0)
                        {
                            dr["remark"] = "Price is 0, can not be transfered to local purchase!!";
                            dr["selected"] = 0;
                        }
                    }
                    Finished = MyUtility.GetValue.Lookup(string.Format(@"select Finished from orders WITH (NOLOCK) where id = '{0}'", dr["orderid"]), null);
                    if (Finished == "True")
                    {
                        dr["remark"] += "  This orders already finished, can not be transfered to local purchase!!";
                        dr["selected"] = 0;
                    }

                    if (dr_localPO["category"].ToString().EqualString("SP_THREAD")
                        || dr_localPO["category"].ToString().EqualString("EMB_THREAD")
                        || dr_localPO["category"].ToString().EqualString("CARTON")) {
                        #region CARTON
                        var strCheckOrderID = dr["OrderID"];
                        var strCheckRemark = "  This orders already PulloutComplete, can not be transfered to local purchase!!";
                        var strCheckPulloutCompleteSql = $@"
SELECT 1
FROM ORDERS O
outer apply (
	select ShipQty= isnull(sum(ShipQty),0)  from Pullout_Detail where OrderID=O.ID
) pd
outer apply(
	select DiffQty= isnull(SUM(isnull(DiffQty ,0)),0) 
	from InvAdjust I
	left join InvAdjust_Qty IQ on I.ID=IQ.ID
	where OrderID=O.ID
) inv
WHERE O.ID='{strCheckOrderID}' 
AND (O.Qty-pd.ShipQty-inv.DiffQty = 0)";
                        #endregion

                        #region SP_THREAD, EMB_THREAD
                        if (dr_localPO["category"].ToString().EqualString("SP_THREAD")
                            || dr_localPO["category"].ToString().EqualString("EMB_THREAD"))
                        {
                            strCheckOrderID = dr["POID"];
                            strCheckRemark = "  This POID already PulloutComplete, can not be transfered to local purchase!!";
                            strCheckPulloutCompleteSql = $@"
select *
from (
	SELECT Qty = sum (O.qty)
		   , ShipQty = sum(pd.ShipQty)
		   , DiffQty = sum(inv.DiffQty)
	FROM ORDERS O
	outer apply (
		select ShipQty= isnull(sum(ShipQty),0)  
		from Pullout_Detail 
		where OrderID=O.ID
	) pd
	outer apply(
		select DiffQty= isnull(SUM(isnull(DiffQty ,0)),0) 
		from InvAdjust I
		left join InvAdjust_Qty IQ on I.ID=IQ.ID
		where OrderID=O.ID
	) inv
	WHERE O.POID='{strCheckOrderID}' 
) tmp
where Qty - ShipQty - DiffQty = 0";
                        }
                        #endregion

                        if (MyUtility.Check.Seek(strCheckPulloutCompleteSql))
                        {
                            dr["remark"] += strCheckRemark;
                            dr["selected"] = 0;
                        }
                    }
                }
                #endregion  
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow ddr = gridImport.GetDataRow<DataRow>(e.RowIndex);
                    if ((decimal)e.FormattedValue > (decimal)ddr["unpaid"])
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Qty can't be more than unpaid");
                        return;
                    }

                    ddr["qty"] = e.FormattedValue;
                }
            };


            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "Master SP#", iseditingreadonly: true) //0
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))//1
                .Text("styleid", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(13))//2
                .Text("Seasonid", header: "Season", iseditingreadonly: true, width: Widths.AnsiChars(13))//3
                 .Text("refno", header: "Refno", iseditingreadonly: true)      //4
                .Text("description", header: "Description", iseditingreadonly: true)//5
                .Text("threadcolorid", header: "Color Shade", iseditingreadonly: true)//6
                .Numeric("qty", header: "Qty", iseditingreadonly: true)//7
                .Text("Unitid", header: "Unit", iseditingreadonly: true)//8
                .Numeric("Price", header: "Price", iseditable: true, decimal_places: 4, integer_places: 4)  //9
                .Numeric("amount", header: "Amount", iseditable: true, decimal_places: 4, integer_places: 4)  //10
                .Text("remark", header: "Remark", iseditingreadonly: true)//11
                .Date("etd", header: "ETD", iseditingreadonly: true)//12
                 .Text("requestid", header: "Request ID", iseditingreadonly: true)//13
                 .Text("BuyerID", header: "Buyer", iseditingreadonly: true)//14
                ;
            //this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;  //Qty   

        }

        // close
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // import
        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            gridImport.ValidateControl();
            
            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;
            
            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0) return;
            
            DataRow[] dr2 = dtImport.Select("Selected = 1 and remark=''");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow =
                        dt_localPODetail.Select(string.Format(@"orderid = '{0}' and refno = '{1}' and threadcolorid = '{2}'"
                                                                                , tmp["orderid"].ToString()
                                                                                , tmp["refno"].ToString()
                                                                                , tmp["threadcolorid"].ToString()));
                    if (findrow.Length > 0)
                    {
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["qty"] = tmp["qty"];
                    }
                    else
                    {
                        tmp["id"] = dr_localPO["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_localPODetail.ImportRow(tmp);
                    }
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Please select rows with empty remark first!", "Warnning");
                return;
            }
            this.Close();
        }

        // To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {

            DataTable dt = ((DataTable)listControlBindingSource1.DataSource).Copy();
            dt.Columns.RemoveAt(dt.Columns.Count-1);
            Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(dt);
            sdExcel.Save(Sci.Production.Class.MicrosoftFile.GetName("Subcon_P30_Import"));

        }
    }
}
