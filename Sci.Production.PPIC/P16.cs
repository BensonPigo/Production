using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Reflection;

namespace Sci.Production.PPIC
{
    public partial class P16 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_balance;

        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.EditMode = true;
            this.InitializeComponent();
            this.grid1.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorCheckBoxColumnSettings col_chk = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_chk.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
                this.calEstUsageAndBalance();
            };

            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
            qty.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                P01_QtyShip callNextForm = new P01_QtyShip(MyUtility.Convert.GetString(dr["ID"]), this.txtSPNo.Text);
                callNextForm.ShowDialog(this);
            };

            DataGridViewGeneratorNumericColumnSettings qty2 = new DataGridViewGeneratorNumericColumnSettings();
            qty2.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = this.grid2.GetDataRow<DataRow>(e.RowIndex);

                string fullpath = System.Windows.Forms.Application.StartupPath + ".\\Sci.Production.Warehouse.dll";
                var assemblys = Assembly.LoadFile(fullpath);
                var types = assemblys.GetTypes().ToList();
                var myClass = types.Where(x => x.FullName == "Sci.Production.Warehouse.P03").First();

                if (myClass != null)
                {
                    var callMethod = myClass.GetMethod("P05Filter");
                    callMethod.Invoke(null, new object[] { this.txtSPNo.Text, dr["Refno"].ToString(), "F", dr["ColorID"].ToString(), this.MdiParent });
                }
            };
            #region Set Grid1
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.Auto(), iseditable: true, trueValue: 1, falseValue: 0, settings: col_chk)
            .Text("ID", header: "SP# (Child SP)", iseditingreadonly: true, width: Widths.Auto())
            .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.Auto())
            .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true, width: Widths.Auto(), settings: qty)
            .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true, width: Widths.Auto())
            .Date("SciDelivery", header: "Sci Delivery", iseditingreadonly: true, width: Widths.Auto())
            .Text("SewInLine", header: "Sewing Inline", iseditingreadonly: true, width: Widths.Auto())
            .Text("SewLine", header: "Sewing Line#", iseditingreadonly: true, width: Widths.Auto())
            .Text("VasShas", header: "VAS/SHAS", iseditingreadonly: true, width: Widths.Auto())
            .Text("CustCDID", header: "CustCD", iseditingreadonly: true, width: Widths.Auto())
            .Text("Alias", header: "Destination", iseditingreadonly: true, width: Widths.Auto())
            ;
            #endregion

            #region Set Grid2
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.Auto())
            .Text("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(22))
            .Text("ColorID", header: "Color", iseditingreadonly: true, width: Widths.Auto())
            .Text("ETA", header: "ETA", iseditingreadonly: true, width: Widths.Auto())
            .Numeric("Qty", header: "Purchase Qty", iseditingreadonly: true, width: Widths.Auto(), decimal_places: 2, settings: qty2)
            .Numeric("Wqty", header: "On Warehouse Qty", iseditingreadonly: true, width: Widths.Auto(), decimal_places: 2)
            .Numeric("bqty", header: "On board Qty", iseditingreadonly: true, width: Widths.Auto(), decimal_places: 2)
            .Numeric("Uqty", header: "Usable Qty", iseditingreadonly: true, width: Widths.Auto(), decimal_places: 2)
            .Numeric("EstUsageQty", header: "Est. Usage Qty", iseditingreadonly: true, width: Widths.Auto(), decimal_places: 2)
            .Numeric("BlanceQty", header: "Blance Qty", iseditingreadonly: true, width: Widths.Auto(), decimal_places: 2, minimum: -999999).Get(out this.col_balance)
            ;
            this.grid2.Columns["Description"].Width = 200;
            #endregion
        }

        private void Change_Color()
        {
            this.col_balance.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid2.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetDecimal(dr["BlanceQty"]) < 0)
                {
                    this.grid2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
                }
                else
                {
                    this.grid2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            };
        }

        private DataTable dt;
        private DataTable dt2;
        private DataTable[] dt3;

        private void Query()
        {
            #region 1
            string sqlcmd = $@"
select 	selected = 0,	o.ID,	oq.Article,	OrderQty = sum(oq.Qty),	o.BuyerDelivery,	o.SciDelivery,	o.SewInLine,	o.SewLine,VasShas = iif(o.VasShas=1,'Y',''),o.CustCDID,	c.Alias,
	dtkey = ROW_NUMBER() OVER(ORDER BY BuyerDelivery) -1
from Orders o with(nolock)
inner join Order_Qty oq with(nolock) on o.ID = oq.ID
left join Country c with(nolock) on o.Dest = c.ID
where o.poid = '{this.txtSPNo.Text}'
group by o.ID,oq.Article,o.BuyerDelivery,o.SciDelivery,o.SewInLine,o.SewLine,o.VasShas,o.CustCDID,c.Alias
order by o.BuyerDelivery,o.ID
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = this.dt;
            ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.Sort = "ID";
            this.grid1.AutoResizeColumns();
            #endregion
            #region 2
            string sqlcmd2 = $@"
select Refno,ColorID,FabricType,id,
Qty = Sum(Round(dbo.getUnitQty(Po_Supp_Detail.POUnit, Po_Supp_Detail.StockUnit, isnull(Po_Supp_Detail.Qty, 0)), 2)) +
      Sum(Round(dbo.getUnitQty(Po_Supp_Detail.POUnit, Po_Supp_Detail.StockUnit, isnull(Po_Supp_Detail.FOC, 0)), 2))
into #tmp 
from Po_Supp_Detail with(nolock)
where FabricType = 'F' and id = '{this.txtSPNo.Text}' and junk=0
group by Refno,ColorID,FabricType,id

select 
	t.Refno,
	f.Description,	
	t.ColorID,	a.ETA,	t.Qty,t.FabricType,
	Wqty = isnull(b.InQty,0)+isnull(c.InQty,0),
	bqty = IIF(isnull(b.bqty,0)<0,0,isnull(b.bqty,0)),
	Uqty = iif('{this.checkBox1.Checked}'='True', isnull(b.InQty,0)+isnull(c.InQty,0)+isnull(b.bqty,0),isnull(b.InQty,0)+isnull(c.InQty,0)),
	Uqty2 = iif('{this.checkBox1.Checked}'='True', isnull(b.InQty,0)+isnull(c.InQty,0)+isnull(b.bqty,0),isnull(b.InQty,0)+isnull(c.InQty,0)),
	EstUsageQty = 0.00,
	BlanceQty = 0.00
from #tmp t
outer apply(
	select ETA = stuff((
		select concat(',',format( ETA,'yyyy/MM/dd'))
		from(
			select distinct ETA
			from Po_Supp_Detail with(nolock) 
			where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID and ETA is not null  and junk=0
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
				where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID)  and junk=0
		)a
		for xml path('')),
	1,1,'')
)f
outer apply(
	select InQty = sum(b.InQty), 
		bqty = sum(Round(dbo.getUnitQty(POUnit, StockUnit, isnull(ShipQty, 0)), 2)) +
			   sum(Round(dbo.getUnitQty(POUnit, StockUnit, isnull(ShipFOC, 0)), 2)) - 
			   sum(b.InQty)
	from Po_Supp_Detail a with(nolock) 
	left join MDivisionPoDetail b with(nolock) on a.id = b.POID and a.SEQ1 = b.Seq1 and a.SEQ2 = b.Seq2
	where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID and a.seq1 not like '7%'
	and a.seq1 <> 'A1'and a.seq1 <> 'A2'  and a.junk=0
)b
outer apply(
	select InQty = SUM(Round(dbo.getUnitQty(POUnit, StockUnit, isnull(QTY, 0)), 2))
	from Po_Supp_Detail a with(nolock) 
	where FabricType = t.FabricType and id = t.ID and Refno = t.Refno and ColorID = t.ColorID and a.seq1 like '7%'  and a.junk=0 and
	(select ShipETA from Po_Supp_Detail with(nolock) where id = a.StockPOID and Seq1 = a.StockSeq1 and Seq2 = A.StockSeq2)<=GETDATE()
)c
order by  t.Refno
drop table #tmp
";
            DualResult result2 = DBProxy.Current.Select(null, sqlcmd2, out this.dt2);
            if (!result2)
            {
                this.ShowErr(result2);
                return;
            }

            this.listControlBindingSource2.DataSource = this.dt2;
            this.grid2.AutoResizeColumns();

            #endregion
            #region 3
            if (this.dt.Rows.Count == 0)
            {
                return;
            }

            string sqlcmd3 = string.Empty;
            foreach (DataRow dr in this.dt.Rows)
            {
                sqlcmd3 += $@"
select refno,ColorID,TtlConsPC = avg(ConsPC)*{dr["OrderQty"]},dtkey={dr["dtkey"]}
from (
    select ob.refno,oec.ColorID,oes.SizeCode,ConsPC = sum(oe.conspc)
    from Order_EachCons oe
    inner join dbo.Order_BOF ob on oe.Id = ob.Id and oe.FabricCode = ob.FabricCode
    inner join dbo.Order_EachCons_Color oec on oe.Id = oec.Id and oe.Ukey = oec.Order_EachConsUkey
    inner join dbo.Order_EachCons_SizeQty oes on oe.Ukey = oes.Order_EachConsUkey
    inner join dbo.Order_ColorCombo oc on oc.Id=oe.Id and oc.Article = '{dr["Article"]}' and oc.FabricPanelCode = oe.FabricPanelCode and oc.colorID=oec.ColorID
    where oe.ID='{this.txtSPNo.Text}'
    and (0 = iif(exists (select 1 from Order_EachCons_Article where Order_EachConsUkey = oe.Ukey),1,0) --若Order_EachCons_Article有資料,要確認Article是否存在於Order_EachCons_Article
	    or '{dr["Article"]}' in(select Article from Order_EachCons_Article oea where oea.Order_EachConsUkey = oe.Ukey))
    group by ob.refno ,oec.ColorID,oes.SizeCode
)a
group by refno,ColorID
";
            }

            DualResult result3 = DBProxy.Current.Select(null, sqlcmd3, out this.dt3);
            if (!result3)
            {
                this.ShowErr(result3);
                return;
            }
            #endregion
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dt == null)
            {
                return;
            }

            if (this.dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow dr2 in this.dt2.Rows)
            {
                dr2["Uqty"] = !this.checkBox1.Checked ? dr2["Wqty"] : MyUtility.Convert.GetDecimal(dr2["Wqty"]) + MyUtility.Convert.GetDecimal(dr2["bqty"]);
                dr2["Uqty2"] = !this.checkBox1.Checked ? dr2["Wqty"] : MyUtility.Convert.GetDecimal(dr2["Wqty"]) + MyUtility.Convert.GetDecimal(dr2["bqty"]);
            }

            this.calBalance();
        }

        private void btnAutoCal_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in this.dt.Rows)
            {
                dr["selected"] = false;
            }

            foreach (DataRow dr2 in this.dt2.Rows)
            {
                dr2["Uqty2"] = dr2["Uqty"];
            }

            foreach (DataRow dr in this.dt.Rows)
            {
                DataTable dt3c = this.dt3[MyUtility.Convert.GetInt(dr["dtkey"])];
                bool flag = true;
                foreach (DataRow dr2 in this.dt2.Rows)
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
                    foreach (DataRow dr2 in this.dt2.Rows)
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

            this.calEstUsageAndBalance();
            this.grid1.ValidateControl();
            this.grid2.ValidateControl();
        }

        private void calEstUsageAndBalance()
        {
            foreach (DataRow dr2 in this.dt2.Rows)
            {
                dr2["EstUsageQty"] = 0;
            }

            foreach (DataRow dr in this.dt.Rows)
            {
                if (MyUtility.Convert.GetInt(dr["selected"]) == 1)
                {
                    DataTable dt3c = this.dt3[MyUtility.Convert.GetInt(dr["dtkey"])];
                    foreach (DataRow dr2 in this.dt2.Rows)
                    {
                        DataRow[] dr3s = dt3c.Select($"Refno = '{dr2["Refno"]}' and ColorID = '{dr2["ColorID"]}'");
                        if (dr3s.Length > 0)
                        {
                            dr2["EstUsageQty"] = MyUtility.Convert.GetDecimal(dr2["EstUsageQty"]) + MyUtility.Convert.GetDecimal(dr3s[0]["TtlConsPC"]);
                        }
                    }
                }
            }

            this.calBalance();
        }

        private void calBalance()
        {
            foreach (DataRow dr2 in this.dt2.Rows)
            {
                dr2["BlanceQty"] = MyUtility.Convert.GetDecimal(dr2["Uqty"]) - MyUtility.Convert.GetDecimal(dr2["EstUsageQty"]);
            }

            this.Change_Color();
        }

        private void grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Countselectcount();
        }

        private void Countselectcount()
        {
            this.grid1.ValidateControl();
            DataGridViewColumn column = this.grid1.Columns["Selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                this.calEstUsageAndBalance();
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;

            string chk2 = $@"select Category from orders where poid = '{this.txtSPNo.Text}' and Category in ('B','S')";
            if (!MyUtility.Check.Seek(chk2))
            {
                return;
            }

            string chk = $@"select 1 from orders where finished = 1 and id = '{this.txtSPNo.Text}'";
            if (MyUtility.Check.Seek(chk))
            {
                MyUtility.Msg.WarningBox($"{this.txtSPNo.Text} PPIC already close !! ");
                return;
            }

            this.Query();
            this.calBalance();
        }

        /// <inheritdoc/>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.txtSPNo.Focused)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        this.Query();
                        this.calBalance();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnNewSearch_Click(object sender, EventArgs e)
        {
            this.txtSPNo.ResetText();
            this.txtSPNo.Select();
        }

        private void btnAutoCalc_Click(object sender, EventArgs e)
        {
            if (this.dt == null)
            {
                return;
            }

            if (this.dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow dr in this.dt.Rows)
            {
                dr["selected"] = false;
            }

            foreach (DataRow dr2 in this.dt2.Rows)
            {
                dr2["Uqty2"] = dr2["Uqty"];
            }

            foreach (DataRow dr in this.dt.Rows)
            {
                DataTable dt3c = this.dt3[MyUtility.Convert.GetInt(dr["dtkey"])];
                bool flag = true;
                foreach (DataRow dr2 in this.dt2.Rows)
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
                    foreach (DataRow dr2 in this.dt2.Rows)
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

            this.calEstUsageAndBalance();
            this.grid1.ValidateControl();
            this.grid2.ValidateControl();
        }
    }
}
