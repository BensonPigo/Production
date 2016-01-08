using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using System.Linq;

namespace Sci.Production.PPIC
{
    public partial class P06 : Sci.Win.Tems.QueryForm
    {
        DataTable gridData;
        Ict.Win.DataGridViewGeneratorDateColumnSettings cutoffDate = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        DataGridViewGeneratorNumericColumnSettings clogctn = new DataGridViewGeneratorNumericColumnSettings();
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            //Exp P/out date預設帶出下個月的最後一天
            dateBox1.Value = (DateTime.Today.AddMonths(2)).AddDays(1 - (DateTime.Today.AddMonths(2)).Day - 1);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            cutoffDate.CellValidating += (s, e) =>
            {

                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["SDPDate"]))
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if ((MyUtility.Convert.GetDate(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddYears(1) || MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddYears(-1)))
                        {
                            MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
                            dr["SDPDate"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };

            //Grid設定
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;

            //當欄位值為0時，顯示空白
            clogctn.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            Helper.Controls.Grid.Generator(this.grid1)
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
                .Date("SDPDate", header: "Cut-off Date", settings: cutoffDate)
                .Date("EstPulloutDate", header: "E. P/Out", iseditingreadonly: true)
                .Text("Seq", header: "Ship Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("ShipmodeID", header: "Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Del", iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Del", iseditingreadonly: true)
                .Date("CRDDate", header: "CRD", iseditingreadonly: true)
                .Text("BuyMonth", header: "Month Buy", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ScanAndPack", header: "S&P", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("RainwearTestPassed", header: "Rainwear Test Passed", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("CTNQty", header: "Ctn Qty", iseditingreadonly: true)
                .EditText("Dimension", header: "Carton Dimension", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ProdRemark", header: "Production Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ShipRemark", header: "Remark", width: Widths.AnsiChars(20))
                .Text("MtlFormA", header: "Mtl. FormA", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("InClogCTN", header: "% in CLOG", iseditingreadonly: true, settings: clogctn)
                .Numeric("CBM", header: "Ttl CBM", decimal_places: 3, iseditingreadonly: true)
                .Text("ClogLocationId", header: "Bin Location", width: Widths.AnsiChars(20), iseditingreadonly: true);

            grid1.Columns[13].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns[27].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns[13].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns[27].DefaultCellStyle.BackColor = Color.Pink;
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            DualResult result;
            if (MyUtility.Check.Empty(dateBox1.Value))
            {
                MyUtility.Msg.WarningBox("Exp P/out Date can't be empty!");
                dateBox1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(txtdropdownlist1.SelectedValue))
            {
                MyUtility.Msg.WarningBox("Category can't be empty!");
                txtdropdownlist1.Focus();
                return;
            }
            StringBuilder sqlCmd = new StringBuilder();
            # region 組SQL
            sqlCmd.Append(string.Format(@"with OrderData
as
(select o.FactoryID,o.BrandID,o.SewLine,o.ID,o.StyleID,o.CustPONo,o.CustCDID,o.Customize2,o.DoxType,
        oq.Qty,o.Dest,o.SewOffLine,oq.SDPDate,oq.EstPulloutDate,oq.Seq,oq.ShipmodeID,oq.BuyerDelivery,
	    o.SciDelivery,o.BuyMonth,o.Customize1,iif(o.ScanAndPack = 1,'Y','') as ScanAndPack,
	    iif(o.RainwearTestPassed = 1,'Y','') as RainwearTestPassed,oq.ProdRemark,oq.ShipRemark,
	    isnull(o.InspDate,iif(oq.EstPulloutDate is null,dateadd(day,2,o.SewOffLine),dateadd(day,-2,oq.EstPulloutDate))) as InspDate,
	    iif(oq.BuyerDelivery > o.CRDDate, o.CRDDate, null) as CRDDate
 from Orders o, Order_QtyShip oq
 where o.MDivisionID = '{0}'
 and o.PulloutComplete = 0
 and o.Finished = 0
 and o.Qty > 0
 and o.ID = oq.Id
 and (oq.EstPulloutDate <= '{1}' or dateadd(day,4,o.SewOffLine) <= '{1}')", Sci.Env.User.Keyword, Convert.ToDateTime(dateBox1.Value).ToString("d")));
            if (txtdropdownlist1.SelectedValue.ToString() == "BS")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
            }
            else
            {
                sqlCmd.Append(string.Format(" and o.Category = '{0}'", txtdropdownlist1.SelectedValue));
            }
            sqlCmd.Append(@"),
distinctOrder
as
(select distinct ID,Seq
 from OrderData
),
CTNData
as
(select a.ID,a.Seq,a.CTNQty,iif(a.CTNQty = 0,0,Round((cast(a.ClogQty as float)/a.CTNQty)*100,0)) as InClogCTN
 from (select do.ID,do.Seq,isnull(sum(pd1.CTNQty),0) as CTNQty,isnull(sum(pd2.CTNQty),0) as ClogQty
       from distinctOrder do
       left join PackingList_Detail pd1 on do.ID = pd1.OrderID and do.Seq = pd1.OrderShipmodeSeq
       left join PackingList_Detail pd2 on pd1.OrderID = pd2.OrderID and pd1.OrderShipmodeSeq = pd2.OrderShipmodeSeq and pd1.CTNStartNo = pd2.CTNStartNo and pd1.Article = pd2.Article and pd1.SizeCode = pd2.SizeCode and pd2.ReceiveDate is not null
       group by do.ID,do.Seq) a
 where a.CTNQty > 0
),
CBM
as
(select do.ID,do.Seq,isnull(sum(p.CBM),0) as CBM
 from distinctOrder do, PackingList p
 where do.ID = p.OrderID
 and do.Seq = p.OrderShipmodeSeq
 group by do.ID,do.Seq
),
RefNo
as
(select distinct do.ID,do.Seq,pd.RefNo,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension
 from distinctOrder do
 left join PackingList_Detail pd on pd.OrderID = do.ID and pd.OrderShipmodeSeq = do.Seq
 left join LocalItem li on li.RefNo = pd.RefNo
 where pd.RefNo is not null and pd.RefNo <> ''
),
CTNDim
as
(select distinct a.ID,a.Seq,(select cast(b.Dimension as nvarchar)+'; ' from RefNo b where a.ID = b.ID and a.Seq = b.Seq for xml path('')) as Dimension
 from RefNo a
),
Location
as
(select distinct do.ID,do.Seq,pd.ClogLocationId
 from distinctOrder do
 left join PackingList_Detail pd on pd.OrderID = do.ID and pd.OrderShipmodeSeq = do.Seq
 where pd.ClogLocationId is not null and pd.ClogLocationId <> ''
),
BinLocation
as
(select distinct a.ID,a.Seq,(select cast(b.ClogLocationId as nvarchar)+'; ' from Location b where a.ID = b.ID and a.Seq = b.Seq for xml path('')) as ClogLocationId
 from Location a
),
MtlFormA
as
(select distinct do.ID,ed.Seq1+'-'+iif(min(ed.FormXReceived) is null,iif(min(ed.FormXReceived)is null, '  /  /    ',convert(char(10),min(ed.FormXReceived),111)) ,iif(max(ed.FormXReceived) is null,'  /  /    ',convert(char(10),max(ed.FormXReceived),111))) as Seq
 from distinctOrder do, Export_Detail ed
 where do.ID = ed.PoID
 group by do.ID,ed.Seq1
),
LastMtlFormA
as
(select distinct mf1.ID, (select CAST(mf2.Seq as nvarchar)+' ; ' from MtlFormA mf2 where mf2.ID = mf1.ID for xml path('')) as MtlFormA
 from MtlFormA mf1
)
select od.*,isnull(cd.CTNQty,0) as CTNQty,isnull(cd.InClogCTN,0) as InClogCTN,isnull(CBM.CBM,0) as CBM,isnull(CTNDim.Dimension,'') as Dimension,isnull(bl.ClogLocationId,'') as ClogLocationId,isnull(lm.MtlFormA,'') as MtlFormA,c.Alias
from OrderData od
left join CTNData cd on od.ID = cd.ID and od.Seq = cd.Seq
left join CBM on od.ID = cbm.ID and od.Seq = cbm.Seq
left join CTNDim on od.ID = CTNDim.ID and od.Seq = CTNDim.Seq
left join BinLocation bl on od.ID = bl.ID and od.Seq = bl.Seq
left join LastMtlFormA lm on od.id = lm.ID
left join Country c on c.ID = od.Dest");
            #endregion

            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData))
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Find Now
        private void button5_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("ID", textBox1.Text.ToString());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }

        //Quit without Save
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Save and Quit
        private void button3_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty((DataTable)listControlBindingSource1.DataSource))
            {
                IList<string> updateCmds = new List<string>();
                this.grid1.ValidateControl();
                listControlBindingSource1.EndEdit();
                StringBuilder allSP = new StringBuilder();
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        updateCmds.Add(string.Format(@"update Order_QtyShip set SDPDate = {0}, ShipRemark = '{1}', EditName = '{2}', EditDate = GETDATE() where ID = '{3}' and Seq = '{4}'",
                            MyUtility.Check.Empty(dr["SDPDate"]) ? "null" : "'" + Convert.ToDateTime(dr["SDPDate"]).ToString("d") + "'",
                            dr["ShipRemark"].ToString(), Sci.Env.User.UserID, dr["ID"].ToString(), dr["Seq"].ToString()));
                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));
                    }
                }
                if (allSP.Length != 0)
                {
                    DataTable GroupData;
                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Id,SDPDate",
                            string.Format("select id,min(SDPDate) as SDPDate from #tmp where Id in ({0}) group by Id", allSP.ToString().Substring(0, allSP.ToString().Length - 1)),
                            out GroupData);
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Save error.", ex);
                        return;
                    }

                    foreach (DataRow dr in GroupData.Rows)
                    {
                        updateCmds.Add(string.Format("update Orders set SDPDate = {0} where ID = '{1}'",
                            MyUtility.Check.Empty(dr["SDPDate"]) ? "null" : "'" + Convert.ToDateTime(dr["SDPDate"]).ToString("d") + "'", dr["Id"].ToString()));
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

        //To Excel
        private void button2_Click(object sender, EventArgs e)
        {
            if ((DataTable)listControlBindingSource1.DataSource == null || ((DataTable)listControlBindingSource1.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            Sci.Production.PPIC.P06_Print callNextForm = new Sci.Production.PPIC.P06_Print((DataTable)listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}