using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P06
    /// </summary>
    public partial class P06 : Sci.Win.Tems.QueryForm
    {
        private DataTable gridData;
        private Ict.Win.DataGridViewGeneratorDateColumnSettings cutoffDate = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings clogctn = new DataGridViewGeneratorNumericColumnSettings();

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // Exp P/out date預設帶出下個月的最後一天
            this.dateExpPoutDate.Value = DateTime.Today.AddMonths(2).AddDays(1 - DateTime.Today.AddMonths(2).Day - 1);
            this.comboDropDownListCategory.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.cutoffDate.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridShipmentSchedule.GetDataRow<DataRow>(e.RowIndex);
                if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["SDPDate"]))
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if (MyUtility.Convert.GetDate(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddYears(1) || MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddYears(-1))
                        {
                            dr["SDPDate"] = DBNull.Value;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
                            return;
                        }
                    }
                }
            };

            // Grid設定
            this.gridShipmentSchedule.IsEditingReadOnly = false;
            this.gridShipmentSchedule.DataSource = this.listControlBindingSource1;

            // 當欄位值為0時，顯示空白
            this.clogctn.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            this.Helper.Controls.Grid.Generator(this.gridShipmentSchedule)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SewLine", header: "Sewing Line", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CustCDID", header: "Cust CD", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Customize2", header: "Field2", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("DoxType", header: "Duty Deduction Dox.", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", iseditingreadonly: true)
                .Text("Alias", header: "Dest.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("SewOffLine", header: "Offline", iseditingreadonly: true)
                .Date("InspDate", header: "F-Insp", iseditingreadonly: true)
                .Date("SDPDate", header: "Cut-off Date", settings: this.cutoffDate)
                .Date("EstPulloutDate", header: "Est. Pullout", iseditingreadonly: true)
                .Text("Seq", header: "Ship Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("ShipmodeID", header: "Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Del", iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Del", iseditingreadonly: true)
                .Date("CRDDate", header: "CRD", iseditingreadonly: true)
                .Text("BuyMonth", header: "Month Buy", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ScanAndPack", header: "S&P", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("RainwearTestPassed", header: "Rainwear Test Passed", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("PackingMethod", header: "Packing Method", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Numeric("CTNQty", header: "Ctn Qty", iseditingreadonly: true)
                .EditText("Dimension", header: "Carton Dimension", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ProdRemark", header: "Production Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ShipRemark", header: "Remark", width: Widths.AnsiChars(20))
                .Text("MtlFormA", header: "Mtl. FormA", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("InClogCTN", header: "% in CLOG", iseditingreadonly: true, settings: this.clogctn)
                .Numeric("CBM", header: "Ttl CBM", decimal_places: 3, iseditingreadonly: true)
                .Text("ClogLocationId", header: "Bin Location", width: Widths.AnsiChars(20), iseditingreadonly: true);

            this.gridShipmentSchedule.Columns["SDPDate"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridShipmentSchedule.Columns["ShipRemark"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridShipmentSchedule.Columns["SDPDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridShipmentSchedule.Columns["ShipRemark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            #region 檢核
            DualResult result;
            if (MyUtility.Check.Empty(this.dateExpPoutDate.Value))
            {
                MyUtility.Msg.WarningBox("Exp P/out Date can't be empty!");
                this.dateExpPoutDate.Focus();
                return;
            }

            if (this.comboDropDownListCategory.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Category can't be empty!");
                this.comboDropDownListCategory.Focus();
                return;
            }
            #endregion

            this.ShowWaitMessage("Data processing, please wait...");
            StringBuilder sqlCmd = new StringBuilder();
            string category = this.comboDropDownListCategory.SelectedValue.ToString();
            #region 組SQL
            sqlCmd.Append(string.Format(
                @"
select distinct oq.Id, oq.Seq , pd.ClogLocationId
into #tmpClocationids
from Orders o WITH (NOLOCK) 
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID
left join PackingList_Detail pd WITH (NOLOCK) on pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
where 1=1            
and o.MDivisionID = '{0}'
and o.PulloutComplete = 0
and o.Finished = 0
and (o.Junk=0 or (o.Junk=1 and o.NeedProduction=1))
and (oq.EstPulloutDate <= '{1}' or oq.EstPulloutDate is null or iif(o.PulloutDate is null, dateadd(day,4,o.SewOffLine) , o.PulloutDate) <= '{1}')
and o.Category in ({2})",
                Sci.Env.User.Keyword,
                Convert.ToDateTime(this.dateExpPoutDate.Value).ToString("d"),
                category));

            sqlCmd.Append(@"

select distinct id,seq into #tmpIDSeq from  #tmpClocationids

SELECT distinct t.id ,t.seq  ,oqd.article
INTO #Order_QtyShip_Detail
FROM #tmpIDSeq t
LEFT JOIN Order_QtyShip_Detail oqD WITH (NOLOCK)  ON  t.id = oqD.id and t.seq = oqD.seq

select *
into #tmp1
from #tmpIDSeq t
outer apply(
	select ClogLocationId = stuff(
	(
		select concat('; ',a.ClogLocationId)
		from(
			select distinct pd2.ClogLocationId
			from #tmpClocationids pd2 WITH (NOLOCK) 
			where pd2.id = t.ID and pd2.seq = t.seq
		)a
		for xml path('')
	),1,2,'')
)ClogLocationId
outer apply(  
	select ColorWay = stuff((
		select concat(', ', b.Article) 
		from  #Order_QtyShip_Detail b
		where b.id = t.id and b.seq = t.seq
		FOR XML PATH('')
	),1,2,'') 
)as oqD   

select ed.Seq1, ed.POID ,
	max(ed.FormXReceived) as maxR,	
	min(ed.FormXReceived) as minR,
	count(*) as count_All,
	count(ed.FormXReceived) as count_NoNull
INTO #MtlFormA
from #tmpIDSeq a 
LEFT JOIN Export_Detail ed WITH (NOLOCK) ON ed.POID = a.Id
GROUP BY Seq1, ed.POID 

select distinct
    o.FactoryID,o.BrandID,o.SewLine,o.Id,o.StyleID,o.CustPONo,o.CustCDID,o.Customize2,o.DoxType,oq.Qty,c.Alias,o.SewOffLine
	,InspDate = isnull(o.InspDate,iif(oq.EstPulloutDate is null,dateadd(day,2,o.SewOffLine),dateadd(day,-2,oq.EstPulloutDate)))
	,oq.SDPDate
    ,EstPulloutDate = iif(oq.EstPulloutDate is null , o.BuyerDelivery , oq.EstPulloutDate)
    ,oq.Seq,oq.ShipmodeID,oq.BuyerDelivery,o.SciDelivery
	,CRDDate = iif(oq.BuyerDelivery > o.CRDDate, o.CRDDate, null)
	,o.BuyMonth,o.Customize1
	,ScanAndPack = iif(o.ScanAndPack = 1,'Y','')
	,RainwearTestPassed = iif(o.RainwearTestPassed = 1,'Y','')
	,Dimension = Dimension.Dimension
	,oq.ProdRemark,oq.ShipRemark
	,MtlFormA = MtlFormA.MtlFormA
    ,[PackingMethod]=dd.ID +'-'+dd.Name
	,C3.CTNQty
    ,InClogCTN = iif(isnull(C3.CTNQty,0) = 0,0,Round((cast(isnull(C3.ClogQty,0) as float)/cast(isnull(C3.CTNQty,0) as float)) * 100,0)) 
    ,C3.CBM
	,o.OrderTypeID as OrderType
	,o.SeasonID as Season
	,o.ProgramID as Program 
    ,o.SewInLine
    ,C3.ClogQty
into #tmp2
from #tmpIDSeq a 
inner join Orders o WITH (NOLOCK) on o.id = a.Id
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID
left join country c WITH (NOLOCK) on c.ID = o.Dest
LEFT JOIN DropDownList dd ON dd.Type='PackingMethod' AND dd.ID=o.CtnType
outer apply(
	Select top 1 
		CBM = first_value(p.CBM) over (order by (select 1))
		,CTNQty = isnull(sum(pd2.CTNQty) over (partition by (select 1)),0)
		,ClogQty = isnull(sum(iif(pd2.ReceiveDate is not null ,pd2.CTNQty,0)) over (partition by (select 1)),0)
	from PackingList_Detail pd2 WITH (NOLOCK) 
	inner join PackingList p WITH (NOLOCK) on pd2.id = p.id
	where pd2.OrderID = oq.ID and pd2.OrderShipmodeSeq = oq.Seq
)C3
outer apply(
	select Dimension = stuff((
		select CONCAT( '; ',d.d)
		from(
			select distinct d = concat(L.CtnLength,'*',L.CtnWidth,'*',L.CtnHeight)
			from PackingList_Detail pd2 WITH (NOLOCK) 
			inner join LocalItem L WITH (NOLOCK) on L.RefNo = pd2.RefNo
			where pd2.OrderID = oq.id and pd2.OrderShipmodeSeq = oq.seq and pd2.RefNo is not null and pd2.RefNo <> ''
		)d
		for xml path('')
	),1,2,'')
)Dimension
outer apply(
	select MtlFormA = stuff((
		select concat('; ',s.Seq1,'-',iif( maxR is null ,  '  /  /    ' , format(iif(s.count_All = s.count_NoNull, s.minR, s.maxR),  'yyyy/MM/dd')))
		from #MtlFormA s
		WHERE s.PoID=oq.Id
		order by Seq1
		for xml path('')
	),1,2,'')
)MtlFormA

select FactoryID,BrandID,SewLine,a.Id,StyleID,CustPONo,CustCDID,Customize2,DoxType,Qty,Alias,SewOffLine,InspDate
	,SDPDate,EstPulloutDate,a.Seq,ShipmodeID,BuyerDelivery,SciDelivery,CRDDate,BuyMonth,Customize1,ScanAndPack
	,RainwearTestPassed,Dimension,ProdRemark,ShipRemark, MtlFormA,PackingMethod,CTNQty,InClogCTN,CBM,ClogLocationId
    ,ColorWay,OrderType,Season,Program ,SewInLine,CLOGQty
from #tmp1 a inner join #tmp2 b on a.Id = b.ID and a.Seq = b.Seq
order by FactoryID,BrandID,SewLine,a.Id,StyleID,CustPONo

drop table #tmpClocationids,#tmpIDSeq,#tmp1,#tmp2,#Order_QtyShip_Detail,#MtlFormA
");
            #endregion

            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData))
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.Messages.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;

            this.HideWaitMessage();
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                return;
            }

            int index = this.listControlBindingSource1.Find("ID", this.txtLocateForSP.Text.ToString());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.listControlBindingSource1.Position = index;
            }
        }

        // Quit without Save
        private void BtnQuitWithoutSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Save and Quit
        private void BtnSaveAndQuit_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty((DataTable)this.listControlBindingSource1.DataSource))
            {
                IList<string> updateCmds = new List<string>();
                this.gridShipmentSchedule.ValidateControl();
                this.listControlBindingSource1.EndEdit();
                StringBuilder allSP = new StringBuilder();
                foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        updateCmds.Add(string.Format(
                            @"update Order_QtyShip set SDPDate = {0}, ShipRemark = '{1}', EditName = '{2}', EditDate = GETDATE() where ID = '{3}' and Seq = '{4}'",
                            MyUtility.Check.Empty(dr["SDPDate"]) ? "null" : "'" + Convert.ToDateTime(dr["SDPDate"]).ToString("d") + "'",
                            dr["ShipRemark"].ToString(),
                            Sci.Env.User.UserID,
                            dr["ID"].ToString(),
                            dr["Seq"].ToString()));

                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));
                    }
                }

                if (allSP.Length != 0)
                {
                    DataTable groupData;
                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable(
                            (DataTable)this.listControlBindingSource1.DataSource,
                            "Id,SDPDate",
                            string.Format("select id,min(SDPDate) as SDPDate from #tmp where Id in ({0}) group by Id", allSP.ToString().Substring(0, allSP.ToString().Length - 1)),
                            out groupData);
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr("Save error.", ex);
                        return;
                    }

                    foreach (DataRow dr in groupData.Rows)
                    {
                        updateCmds.Add(string.Format(
                            "update Orders set SDPDate = {0} where ID = '{1}'",
                            MyUtility.Check.Empty(dr["SDPDate"]) ? "null" : "'" + Convert.ToDateTime(dr["SDPDate"]).ToString("d") + "'",
                            dr["Id"].ToString()));
                    }
                }

                if (updateCmds.Count != 0)
                {
                    DualResult result = DBProxy.Current.Executes(null, updateCmds);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Save Fail!" + result.ToString());
                        return;
                    }
                }

                this.Close();
            }
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            if ((DataTable)this.listControlBindingSource1.DataSource == null || ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            Sci.Production.PPIC.P06_Print callNextForm = new Sci.Production.PPIC.P06_Print((DataTable)this.listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}