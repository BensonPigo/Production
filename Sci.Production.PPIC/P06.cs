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
                .Numeric("CTNQty", header: "Ctn Qty", iseditingreadonly: true)
                .EditText("Dimension", header: "Carton Dimension", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ProdRemark", header: "Production Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ShipRemark", header: "Remark", width: Widths.AnsiChars(20))
                .Text("MtlFormA", header: "Mtl. FormA", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("InClogCTN", header: "% in CLOG", iseditingreadonly: true, settings: clogctn)
                .Numeric("CBM", header: "Ttl CBM", decimal_places: 3, iseditingreadonly: true)
                .Text("ClogLocationId", header: "Bin Location", width: Widths.AnsiChars(20), iseditingreadonly: true);

            grid1.Columns["SDPDate"].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns["ShipRemark"].DefaultCellStyle.ForeColor = Color.Red;
            grid1.Columns["SDPDate"].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns["ShipRemark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            #region 檢核
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
            #endregion

            this.ShowWaitMessage("Data processing, please wait...");
            StringBuilder sqlCmd = new StringBuilder();
            # region 組SQL
            sqlCmd.Append(string.Format(@"select DISTINCT
            o.FactoryID,o.BrandID,o.SewLine,o.Id,o.StyleID,o.CustPONo,o.CustCDID,o.Customize2,o.DoxType,oq.Qty,c.Alias,o.SewOffLine,
            isnull(o.InspDate,iif(oq.EstPulloutDate is null,dateadd(day,2,o.SewOffLine),dateadd(day,-2,oq.EstPulloutDate))) as InspDate,
            oq.SDPDate
            --,oq.EstPulloutDate
            ,iif(oq.EstPulloutDate is null , o.BuyerDelivery , oq.EstPulloutDate) EstPulloutDate
            ,oq.Seq,oq.ShipmodeID,oq.BuyerDelivery,o.SciDelivery,
            iif(oq.BuyerDelivery > o.CRDDate, o.CRDDate, null) as CRDDate,
            o.BuyMonth,o.Customize1,
            iif(o.ScanAndPack = 1,'Y','') as ScanAndPack,
            iif(o.RainwearTestPassed = 1,'Y','') as RainwearTestPassed,
            stuff(Dimension.Dimension,1,2,'') as Dimension,
            oq.ProdRemark,oq.ShipRemark,
            stuff(MtlFormA.MtlFormA,1,2,'') as MtlFormA,
            isnull(CTNQty.CTNQty,0) as CTNQty,
            iif(CTNQty.CTNQty=0,0,Round((cast(ClogQty.ClogQty as float)/cast(CTNQty.CTNQty as float)) * 100,0)) as InClogCTN,
            CBM.CBM,
            isnull(stuff(ClogLocationId.ClogLocationId,1,1,''),'') as ClogLocationId

            from Orders o WITH (NOLOCK) 
            left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID
            left join country c WITH (NOLOCK) on c.ID = o.Dest
            left join PackingList_Detail pd WITH (NOLOCK) on pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
            outer apply(
	            select isnull(sum(pd.CTNQty),0) as CTNQty
	            from PackingList_Detail pd WITH (NOLOCK) 
	            where pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
            )CTNQty
            outer apply(
	            select isnull(sum(pd.CTNQty),0) as ClogQty
	            from PackingList_Detail pd WITH (NOLOCK) 
	            where pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq 
	            and pd.ReceiveDate is not null
            )ClogQty
            outer apply(
	            select ClogLocationId = (
		            select distinct concat('; ',pd.ClogLocationId)
		            from PackingList_Detail pd WITH (NOLOCK) 
		            where pd.OrderID = oq.ID and pd.OrderShipmodeSeq = oq.seq
		            for xml path('')
	            )
            )ClogLocationId
            outer apply(
	            select Dimension = (
		            select distinct concat('; ',L.CtnLength,'*',L.CtnWidth,'*',L.CtnHeight)
		            from PackingList_Detail pd WITH (NOLOCK) 
		            inner join LocalItem L WITH (NOLOCK) on  pd.RefNo = L.RefNo 
		            where pd.OrderID = oq.id and pd.OrderShipmodeSeq = oq.seq
		            and pd.RefNo is not null and pd.RefNo <> ''
		            for xml path('')
	            )
            )Dimension
            outer apply(
	            select CBM=(
		               Select top 1 isnull(p.CBM,0)
		            from PackingList p WITH (NOLOCK) 
					inner join PackingList_Detail pd WITH (NOLOCK) on p.id=pd.id
		            where p.OrderID = oq.ID and pd.OrderShipmodeSeq = oq.Seq
	            )
            )CBM
            outer apply(
	            select MtlFormA =(
		            select concat('; ',s.Seq1,'-',iif( maxR is null ,  '  /  /    ' , format(s2.FormXRev,  'yyyy/MM/dd')))
		            from (
			            select ed.Seq1,
				            max(ed.FormXReceived) as maxR,	
				            min(ed.FormXReceived) as minR,
				            count(*) as count_All,
				            count(ed.FormXReceived) as count_NoNull
			            from Export_Detail ed WITH (NOLOCK) 
			            where ed.PoID = oq.Id
			            GROUP BY Seq1
		            ) as s
		            outer apply(
			            select iif(s.count_All = s.count_NoNull, s.minR, s.maxR) as FormXRev
		            ) as s2
		            order by Seq1
		            for xml path('')
	            )
            )MtlFormA

            where 1=1            
            and o.MDivisionID = '{0}'
            and o.PulloutComplete = 0
            and o.Finished = 0
            and o.Qty > 0
            and (oq.EstPulloutDate <= '{1}' or oq.EstPulloutDate is null or iif(o.PulloutDate is null, dateadd(day,4,o.SewOffLine) , o.PulloutDate) <= '{1}') ",
            Sci.Env.User.Keyword, Convert.ToDateTime(dateBox1.Value).ToString("d")));
            if (txtdropdownlist1.SelectedValue.ToString() == "BS")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
            }
            else
            {
                sqlCmd.Append(string.Format(" and o.Category = '{0}'", txtdropdownlist1.SelectedValue));
            }
            # endregion

            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData))
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
            this.grid1.AutoResizeColumns(); //先註解，會很慢
            this.HideWaitMessage();
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