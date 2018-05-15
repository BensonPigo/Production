using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using System.Data.SqlClient;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P05 : Sci.Win.Tems.QueryForm
    {
        public P05(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            this.EditMode = true;
            InitializeComponent();
            this.grid1.IsEditingReadOnly = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorCheckBoxColumnSettings col_chk = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_chk.CellValidating += (s, e) =>
            {
                DataRow dr = grid1.GetDataRow<DataRow>(e.RowIndex);
                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
                calEstUsageAndBalance();
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
            qty.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = grid1.GetDataRow<DataRow>(e.RowIndex);
                Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(dr["ID"]),txtSPNo.Text);
                callNextForm.ShowDialog(this);
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings qty2 = new DataGridViewGeneratorNumericColumnSettings();
            qty2.CellMouseDoubleClick += (s, e) =>
            {

                if (callP03 != null && callP03.Visible == true)
                {
                    callP03.P03Data(txtSPNo.Text);
                    callP03.Activate();
                }
                else
                {
                    P03FormOpen();
                }
            };
            #region Set Grid1
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0,settings: col_chk)
            .Text("ID", header: "SP# (Child SP)", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true, width: Widths.AnsiChars(13),settings:qty)
            .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Date("SciDelivery", header: "Sci Delivery", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("SewInLine", header: "Sewing Inline", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("SewLine", header: "Sewing Line#", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("VasShas", header: "VAS/SHAS", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("CustCDID", header: "CustCD", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("Alias", header: "Destination", iseditingreadonly: true, width: Widths.AnsiChars(13))
            ;
            #endregion

            #region Set Grid2
            Helper.Controls.Grid.Generator(this.grid2)
            .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("ColorID", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("ETA", header: "ETA", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Numeric("Qty", header: "Purchase Qty", iseditingreadonly: true, width: Widths.AnsiChars(13),decimal_places:2, settings: qty2)
            .Numeric("Wqty", header: "On Warehouse Qty", iseditingreadonly: true, width: Widths.AnsiChars(13), decimal_places: 2)
            .Numeric("bqty", header: "On board Qty", iseditingreadonly: true, width: Widths.AnsiChars(13), decimal_places: 2)
            .Numeric("Uqty", header: "Usable Qty", iseditingreadonly: true, width: Widths.AnsiChars(13), decimal_places: 2)
            .Numeric("EstUsageQty", header: "Est. Usage Qty", iseditingreadonly: true, width: Widths.AnsiChars(13), decimal_places: 2)
            .Numeric("BlanceQty", header: "Blance Qty", iseditingreadonly: true, width: Widths.AnsiChars(13), decimal_places: 2,minimum:-999999)
            ;
            #endregion
        }

        Sci.Production.Warehouse.P03 callP03 = null;
        private void P03FormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Sci.Production.Warehouse.P03)
                {
                    form.Activate();
                    Sci.Production.Warehouse.P03 activateForm = (Sci.Production.Warehouse.P03)form;
                    activateForm.setTxtSPNo(txtSPNo.Text);
                    activateForm.Query();
                    return;
                }
            }

            ToolStripMenuItem P03MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P03. Material Status"))
                            {
                                P03MenuItem = ((ToolStripMenuItem)subMenuItem);
                                break;
                            }
                        }
                    }
                }
            }

            callP03 = new P03(txtSPNo.Text, P03MenuItem);
            callP03.MdiParent = MdiParent;
            callP03.Show();
            callP03.P03Data(txtSPNo.Text);
            callP03.ChangeDetailColor();
        }

        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            Query();
            calBalance();
        }

        DataTable dt;
        DataTable dt2;
        DataTable[] dt3;

        private void Query()
        {
            #region 1
            string sqlcmd = $@"
select 	selected = 0,	o.ID,	oq.Article,	OrderQty = sum(oq.Qty),	o.BuyerDelivery,	o.SciDelivery,	o.SewInLine,	o.SewLine,VasShas = iif(o.VasShas=1,'Y',''),o.CustCDID,	c.Alias,
	dtkey = ROW_NUMBER() OVER(ORDER BY BuyerDelivery) -1
from Orders o with(nolock)
inner join Order_Qty oq with(nolock) on o.ID = oq.ID
left join Country c with(nolock) on o.Dest = c.ID
where o.poid = '{txtSPNo.Text}'
group by o.ID,oq.Article,o.BuyerDelivery,o.SciDelivery,o.SewInLine,o.SewLine,o.VasShas,o.CustCDID,c.Alias
order by BuyerDelivery
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            this.listControlBindingSource1.DataSource = dt;
            this.grid1.AutoResizeColumns();
            #endregion
            #region 2
            string sqlcmd2 = $@"
select Refno,ColorID,FabricType,id,Qty = Sum(Po_Supp_Detail.Qty)
into #tmp 
from Po_Supp_Detail with(nolock)
where FabricType = 'F' and id = '{txtSPNo.Text}'
group by Refno,ColorID,FabricType,id

select 
	t.Refno,
	f.Description,	
	t.ColorID,	a.ETA,	t.Qty,
	Wqty = isnull(b.InQty,0)+isnull(c.InQty,0),
	bqty = isnull(b.bqty,0),
	Uqty = iif('{checkBox1.Checked}'='True',isnull(b.InQty,0)+isnull(c.InQty,0), isnull(b.InQty,0)+isnull(c.InQty,0)+isnull(b.bqty,0)),
	Uqty2 = iif('{checkBox1.Checked}'='True',isnull(b.InQty,0)+isnull(c.InQty,0), isnull(b.InQty,0)+isnull(c.InQty,0)+isnull(b.bqty,0)),
	EstUsageQty = 0.00,
	BlanceQty = 0.00
from #tmp t
outer apply(
	select ETA = stuff((
		select concat(',',format( ETA,'yyyy/MM/dd'))
		from(
			select distinct ETA
			from Po_Supp_Detail with(nolock) 
			where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID and ETA is not null
		)x
		for xml path('')),
	1,1,'')
)a
outer apply(
	select Description = stuff((
		select concat(',',Description)
		from(
			select distinct Description 
			from Fabric f with(nolock) 
			where f.SCIRefno in (
				select distinct SCIRefno 
				from Po_Supp_Detail with(nolock) 
				where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID)
		)a
		for xml path('')),
	1,1,'')
)f
outer apply(
	select InQty = sum(b.InQty), bqty = sum(a.ShipQty + a.ShipFOC - b.InQty)
	from Po_Supp_Detail a with(nolock) 
	left join MDivisionPoDetail b with(nolock) on a.id = b.POID and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2
	where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID and a.seq1 not like '7%'
)b
outer apply(
	select InQty = sum(InQty)
	from Po_Supp_Detail a with(nolock) 
	inner join MDivisionPoDetail b with(nolock) on a.id = b.POID and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2
	where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID and a.seq1 like '7%' and ShipETA <=GETDATE()
)c
order by  t.Refno
drop table #tmp
";
            DualResult result2 = DBProxy.Current.Select(null, sqlcmd2, out dt2);
            if (!result2)
            {
                this.ShowErr(result2);
                return;
            }
            this.listControlBindingSource2.DataSource = dt2;
            this.grid2.AutoResizeColumns();
            this.grid2.Columns["Description"].Width = 200;
            #endregion
            #region 3
            if (dt.Rows.Count == 0)
            {
                return;
            }

            string sqlcmd3 = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                sqlcmd3 += $@"
select ob.refno,oec.ColorID,TtlConsPC = avg(oe.conspc)*{dr["OrderQty"]},dtkey={dr["dtkey"]}
from Order_EachCons oe
inner join dbo.Order_BOF ob on oe.Id = ob.Id and oe.FabricCode = ob.FabricCode
inner join dbo.Order_EachCons_Color oec on oe.Id = oec.Id and oe.Ukey = oec.Order_EachConsUkey
where oe.ID='{txtSPNo.Text}'
and (0 = iif(exists (select 1 from Order_EachCons_Article where Order_EachConsUkey = oe.Ukey),1,0) --若Order_EachCons_Article有資料,要確認Article是否存在於Order_EachCons_Article
	or '{dr["Article"]}' in(select Article from Order_EachCons_Article oea where oea.Order_EachConsUkey = oe.Ukey))
group by oe.ID,ob.refno,oec.ColorID
order by ob.refno,oec.ColorID
";
            }
            DualResult result3 = DBProxy.Current.Select(null, sqlcmd3, out dt3);
            if (!result3)
            {
                this.ShowErr(result3);
                return;
            }
            #endregion
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataRow dr2 in dt2.Rows)
            {
                dr2["Uqty"] = checkBox1.Checked ? dr2["Wqty"] : MyUtility.Convert.GetDecimal(dr2["Wqty"]) + MyUtility.Convert.GetDecimal(dr2["bqty"]);
                dr2["Uqty2"] = checkBox1.Checked ? dr2["Wqty"] : MyUtility.Convert.GetDecimal(dr2["Wqty"]) + MyUtility.Convert.GetDecimal(dr2["bqty"]);
            }

            calBalance();
        }

        private void btnAutoCal_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr["selected"] = false;
            }

            foreach (DataRow dr2 in dt2.Rows)
            {
                dr2["Uqty2"] = dr2["Uqty"];
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataTable dt3c = dt3[MyUtility.Convert.GetInt(dr["dtkey"])];
                bool flag = true;
                foreach (DataRow dr2 in dt2.Rows)
                {
                    DataRow[] dr3s = dt3c.Select($"Refno = '{dr2["Refno"]}' and ColorID = '{dr2["ColorID"]}'");
                    if (dr3s.Length > 0)
                    {
                        if (MyUtility.Convert.GetDecimal(dr2["Uqty2"]) - MyUtility.Convert.GetDecimal(dr3s[0]["TtlConsPC"]) < 0)
                        {
                            flag = false;
                        }
                    }
                }

                if (flag)
                {
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        DataRow[] dr3s = dt3c.Select($"Refno = '{dr2["Refno"]}' and ColorID = '{dr2["ColorID"]}'");
                        if (dr3s.Length > 0)
                        {
                            dr2["Uqty2"] = MyUtility.Convert.GetDecimal(dr2["Uqty2"]) - MyUtility.Convert.GetDecimal(dr3s[0]["TtlConsPC"]);
                        }
                    }
                    dr["selected"] = true;
                }
            }
            calEstUsageAndBalance();
            this.grid1.ValidateControl();
            this.grid2.ValidateControl();
        }

        private void calEstUsageAndBalance()
        {
            foreach (DataRow dr2 in dt2.Rows)
            {
                dr2["EstUsageQty"] = 0;
            }

            foreach (DataRow dr in dt.Rows)
            {
                if (MyUtility.Convert.GetInt(dr["selected"])==1)
                {
                    DataTable dt3c = dt3[MyUtility.Convert.GetInt(dr["dtkey"])];
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        DataRow[] dr3s = dt3c.Select($"Refno = '{dr2["Refno"]}' and ColorID = '{dr2["ColorID"]}'");
                        if (dr3s.Length > 0)
                        {
                            dr2["EstUsageQty"] = MyUtility.Convert.GetDecimal(dr2["EstUsageQty"]) + MyUtility.Convert.GetDecimal(dr3s[0]["TtlConsPC"]);
                        }
                    }
                }
            }
            calBalance();
        }

        private void calBalance()
        {
            foreach (DataRow dr2 in dt2.Rows)
            {
                dr2["BlanceQty"] = MyUtility.Convert.GetDecimal(dr2["Uqty"]) - MyUtility.Convert.GetDecimal(dr2["EstUsageQty"]);
            }
        }
    }
}
