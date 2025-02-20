using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P68 : Win.Tems.QueryForm
    {
        private List<SqlParameter> listsqlParameter = new List<SqlParameter>();
        private string strSQLWher = string.Empty;
        private DataTable dt_Head = new DataTable();
        private DataTable dt_Detail = new DataTable();

        /// <inheritdoc/>
        public P68(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.txtmultifactory1.MDivisionID = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.cbStatus, 1, 1, "All,Preparing,Unroll,Relaxation,Dispatched,Received");
            this.grid2.DataSource = this.bindingDetail;
            this.cutingDate.Value1 = DateTime.Now;
            this.cutingDate.Value2 = DateTime.Now;
            this.txtmultifactory1.Text = Sci.Env.User.Factory;

            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region 上面的Grid
            DataGridViewGeneratorTextColumnSettings refno = new DataGridViewGeneratorTextColumnSettings();
            refno.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                var frm = new P68_Refno(dr);
                frm.ShowDialog(this);
            };

            DataGridViewGeneratorTextColumnSettings whRemark = new DataGridViewGeneratorTextColumnSettings();
            whRemark.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                var frm = new P68_WHReamrk(dr);
                frm.ShowDialog(this);
            };
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Factory", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CutCell", header: "Cut\r\nCell", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Date("EditDate", header: "Cutplan\r\nEdit Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ID", header: "Cutplan ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("EstCutdate", header: "Cutting Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: refno)
                .Text("Color", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Date("ActETA", header: "Act ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("FabricRelaxationID", header: "Relaxation", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("NeedUnroll", header: "Need\r\nUnroll", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Relaxtime", header: "Relaxtime\r\n(Hours)", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Status", header: "Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Numeric("RequestCons", header: "Request\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                .Numeric("PreparingCons", header: "Preparing\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                .Numeric("BalanceQty", header: "Balance\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                .Numeric("UnrollCons", header: "Unroll\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                .Numeric("RelaxationCons", header: "Relaxation\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                .Numeric("DispatchedCons", header: "Dispatched\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                .Numeric("ReceivedCons", header: "Received\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                .EditText("RequestorRemark", header: "Requestor\r\nRemark", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("WHRemark", header: "W/H Remark", width: Widths.AnsiChars(9), iseditingreadonly: true, settings: whRemark)
                ;
            #endregion

            #region 下面的Grid
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Shade\r\nBand/Group", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Date("LockDate", header: "Unlock Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("IssueQty", header: "Issue Qty", width: Widths.AnsiChars(9), decimal_places: 2, iseditingreadonly: true)
            .DateTime("MINDReleaseDate", header: "Pick Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("UnrollStartTime", header: "Unroll\r\nStart Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("UnrollMachine", header: "Unroll\r\nMachine", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("RelaxationStartTime", header: "Relax\r\nStart Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("RelaxationEndTime", header: "Relax\r\nEnd Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("DispatchTime", header: "Dispatch Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("FactoryReceivedTime", header: "Factory\r\nReceive Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .Text("RackLocationID", header: "Location Dispatched", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listsqlParameter.Clear();
            this.strSQLWher = string.Empty;
            if (MyUtility.Check.Empty(this.cutingDate.Value1) && MyUtility.Check.Empty(this.txtCutplanID.Text) && MyUtility.Check.Empty(this.dateCutPlanEditDate.Value1))
            {
                MyUtility.Msg.WarningBox("<Cutting Date> and <Cutplan ID> and <Cut Plan EditDate> cannot all be empty.");
                return;
            }

            if (!MyUtility.Check.Empty(this.cutingDate.Value1))
            {
                this.listsqlParameter.Add(new SqlParameter("@CuttingDateStart", this.cutingDate.Value1));
                this.listsqlParameter.Add(new SqlParameter("@CuttingDateEnd", this.cutingDate.Value2));
                this.strSQLWher += " and EstCutdate.EstCutdate BETWEEN CAST(@CuttingDateStart AS DATE) AND CAST(@CuttingDateEnd AS DATE)";
            }

            if (!MyUtility.Check.Empty(this.txtCutplanID.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@CutPlanID", this.txtCutplanID.Text));
                this.strSQLWher += " and cp.ID = @CutPlanID";
            }

            if (!MyUtility.Check.Empty(this.txtCutCell_Value1.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@CutCellStart", this.txtCutCell_Value1.Text));
                this.strSQLWher += " and cp.CutCellID >= @CutCellStart";
            }

            if (!MyUtility.Check.Empty(this.txtCutCell_Value2.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@CutCellEnd", this.txtCutCell_Value2.Text));
                this.strSQLWher += " and cp.CutCellID <= @CutCellEnd";
            }

            if (!MyUtility.Check.Empty(this.txtmultifactory1.Text))
            {
                this.strSQLWher += $" and o.FactoryID in('{string.Join("','", this.txtmultifactory1.Text.Split(',').ToList())}')";
            }

            if (!MyUtility.Check.Empty(this.txtPOID.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@POID", this.txtPOID.Text));
                this.strSQLWher += " and cp.POID  = @POID";
            }

            if (!MyUtility.Check.Empty(this.txtstyle1.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@StyleID", this.txtstyle1.Text));
                this.strSQLWher += " and o.StyleID  = @StyleID";
            }

            if (!MyUtility.Check.Empty(this.dateCutPlanEditDate.Value1))
            {
                this.listsqlParameter.Add(new SqlParameter("@CutPlanEditDate1", this.dateCutPlanEditDate.Value1));
                this.listsqlParameter.Add(new SqlParameter("@CutPlanEditDate2", this.dateCutPlanEditDate.Value2));
                this.strSQLWher += " and CAST(EditDate.EditDate as DATE) BETWEEN @CutPlanEditDate1 AND @CutPlanEditDate2";
            }

            string sqlCmd = $@"
			select cp.ID
					, o.FactoryID
                    , o.StyleID
					, cp.POID
					, cp.CutCellID
					, EstCutdate.EstCutdate
                    , EditDate.EditDate
					, psd.Refno
					, [Color] = psdsC.SpecValue
					, rfrt.FabricRelaxationID
					, frlx.NeedUnroll
					, frlx.Relaxtime
					, [Request Cons] = sum (cpdc.Cons)
                    , FinalETA.ActETA
                    ,psd.SCIRefno
                    ,cpi.RequestorRemark
			into #CutList
			from Cutplan cp
			inner join Cutplan_Detail_Cons cpdc on cp.ID = cpdc.ID
			inner join PO_Supp_Detail psd on cpdc.POID = psd.ID
											and cpdc.SEQ1 = psd.SEQ1
											and cpdc.SEQ2 = psd.SEQ2
			inner join PO_Supp_Detail_Spec psdsC on psd.ID = psdsC.ID
													and psd.SEQ1 = psdsC.Seq1
													and psd.SEQ2 = psdsC.Seq2
													and psdsC.SpecColumnID = 'Color'
			left join SciMES_RefnoRelaxtime rfrt on psd.Refno = rfrt.Refno
			left join SciMES_FabricRelaxation frlx on rfrt.FabricRelaxationID = frlx.ID
			left join Orders o on cp.POID = o.ID
            LEFT JOIN CutPlan_IssueCutDate cpi WITH(NOLOCK) ON cpi.ID = cp.id AND cpi.Refno = psd.Refno AND cpi.Colorid = psdsC.SpecValue
            OUTER APPLY(SELECT EstCutdate = IIF(cpi.EstCutDate IS NOT NULL, cpi.EstCutDate, cp.EstCutdate))EstCutdate
            OUTER APPLY(SELECT EditDate = IIF(cpi.EditDate IS NOT NULL, cpi.EditDate, cp.EditDate))EditDate
            outer apply
            (
                select ActETA = Max(p3.FinalETA) 
                from PO_Supp_Detail p3 with (nolock) 
                inner join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = p3.id and psdsC2.seq1 = p3.seq1 and psdsC2.seq2 = p3.seq2 and psdsC2.SpecColumnID = 'Color'
                where p3.id = psd.ID
                and p3.SCIRefno = psd.SCIRefno
                and psdsC2.SpecValue = psdsC.SpecValue
                and p3.Junk = 0
                and p3.Seq1 not like 'A%'
            ) FinalETA
			where cp.Status = 'Confirmed'
			{this.strSQLWher}
			group by cp.ID, o.FactoryID, cp.POID, cp.CutCellID
                ,EstCutdate.EstCutdate
                ,psd.Refno
                ,psdsC.SpecValue
                ,rfrt.FabricRelaxationID
                ,frlx.NeedUnroll
                ,frlx.Relaxtime
                ,o.StyleID
                ,EditDate.EditDate
                ,FinalETA.ActETA
                ,psd.SCIRefno
                ,cpi.RequestorRemark
			/*
				發料準備清單
			*/
			select cl.ID, cl.Refno, cl.Color, cl.NeedUnroll, cl.Relaxtime
					, isud.POID, isud.Seq1, isud.Seq2, isud.Roll, isud.Dyelot, isud.Qty
                    , isud.MINDReleaseDate
					, fur.UnrollStartTime, [UnrollMachine] = MIOT.MachineID
					, [UnrollDone] = IIF (cl.NeedUnroll = 1 and UnrollStatus != '', 1, 0)
					, fur.RelaxationStartTime, fur.RelaxationEndTime
					, [RelaxationDone] = IIF (cl.Relaxtime > 0 and fur.RelaxationStartTime is not null, 1, 0)
					, mmd.DispatchTime, mmd.FactoryReceivedTime
                    , f.Tone
                    , LockDate = IIF(f.Lock = 0, f.LockDate, Null)
                    , [Location] = dbo.Getlocation(f.ukey)
                    ,cl.SCIRefno
                    ,[issueid] = isu.id
                    ,mmd.RackLocationID
			into #issueDtl
			from #CutList cl
			inner join Issue isu on cl.ID = isu.CutplanID
			inner join Issue_Detail isud on isu.Id = isud.Id
			inner join PO_Supp_Detail psd on isud.POID = psd.ID
											and isud.Seq1= psd.SEQ1
											and isud.Seq2 = psd.SEQ2
											and cl.Refno = psd.Refno
			inner join PO_Supp_Detail_Spec psdsC on psd.ID = psdsC.ID
													and psd.SEQ1 = psdsC.Seq1
													and psd.SEQ2 = psdsC.Seq2
													and psdsC.SpecColumnID = 'Color'
													and cl.Color = psdsC.SpecValue
			inner join WHBarcodeTransaction wbt on isud.Id = wbt.TransactionID
													and isud.ukey = wbt.TransactionUkey
													and wbt.Action = 'Confirm'
			left join Fabric_UnrollandRelax fur on wbt.To_NewBarcode = fur.Barcode
            left join [ExtendServer].ManufacturingExecution.dbo.MachineIoT MIOT with (nolock) on MIOT.Ukey = fur.MachineIoTUkey and MIOT.MachineIoTType= 'unroll'
			left join M360MINDDispatch mmd on isud.M360MINDDispatchUkey = mmd.Ukey
            left join FtyInventory f on f.POID = isud.POID
                                    and f.Seq1 = isud.Seq1
                                    and f.Seq2 = isud.Seq2
                                    and f.Roll = isud.Roll
                                    and f.Dyelot = isud.Dyelot
                                    and f.StockType = isud.StockType
			where isu.Status = 'Confirmed'


			/*
				計算各狀態已完成數
				此為彙整總表
			*/
			select cl.ID
					, [Factory] = cl.FactoryID
					, cl.POID
					, [CutCell] = cl.CutCellID
					, cl.EstCutdate
					, cl.Refno
					, cl.Color
					, cl.FabricRelaxationID
					, [NeedUnroll] = IIF (cl.NeedUnroll = 1, 'Y', '')
					, cl.Relaxtime
					, [Status] = case
									when Received.Cons >= Floor (cl.[Request Cons]) then 'Received'
									when Dispatched.Cons >= Floor (cl.[Request Cons]) then 'Dispatched'
									when Relaxation.Cons >= Floor (cl.[Request Cons]) then 'Relaxation'
									when Unroll.Cons >= Floor (cl.[Request Cons]) then 'Unroll'
									else 'Preparing'
								end
					, [RequestCons] = cl.[Request Cons]
					, [PreparingCons] = isnull (Preparing.Cons, 0)
					, [UnrollCons] = isnull (Unroll.Cons, 0)
					, [RelaxationCons] = isnull (Relaxation.Cons, 0)
					, [DispatchedCons] = isnull (Dispatched.Cons, 0)
					, [ReceivedCons] = isnull (Received.Cons, 0)
					, [BalanceQty] = cl.[Request Cons] - isnull (Preparing.Cons, 0)
                    , cl.ActETA
                    , cl.StyleID
                    , cl.EditDate
                    , cl.SCIRefno
                    , IssueSummary.WHRemark
                    , cl.RequestorRemark
			from #CutList cl
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
			) Preparing
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.UnrollDone = 1
			) Unroll
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.RelaxationDone = 1
			) Relaxation
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.DispatchTime is not null
			) Dispatched
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.FactoryReceivedTime is not null
			) Received
            OUTER APPLY (
                SELECT WHRemark = 
	            REPLACE(
		            REPLACE(
			            (select stuff((
				            select n=Remark
				            from(
                                SELECT DISTINCT iss.Remark
                                FROM issue i
                                INNER JOIN Issue_Summary iss ON iss.Id = i.ID
                                WHERE cl.ID = i.CutplanID
                                AND cl.POID = iss.POID
                                AND cl.SCIRefno = iss.SCIRefno
                                AND cl.Color = iss.Colorid
                                AND iss.Remark <> ''
				            )d  order by Remark
				            for xml path('')
			            ),1,3,''))
		            ,'</n>','')
	            ,'<n>',',' + CHAR(13) + char(10)) -- EditText 開窗換行方式
            ) IssueSummary　 --串Issue Summary取Remark
			order by cl.ID, cl.Refno, cl.Color
			/*
				顯示清單
			*/
			select 
			idt.ID, 
			idt.Refno, 
			idt.Color, 
			[Seq] = concat (idt.Seq1, ' ', idt.Seq2),
			idt.Roll,
			idt.Dyelot,
			[IssueQty] = idt.Qty, 
			idt.UnrollStartTime,
            idt.UnrollMachine,
			idt.RelaxationStartTime, 
			idt.RelaxationEndTime, 
			idt.DispatchTime,
			idt.FactoryReceivedTime
            ,idt.MINDReleaseDate
            ,idt.Tone
            ,idt.LockDate
            ,idt.[Location]
            ,idt.RackLocationID
			from #issueDtl idt
			order by idt.ID, idt.Refno, idt.Color, idt.Seq1, idt.Seq2, idt.Roll, idt.Dyelot

			drop table #CutList, #issueDtl";

            DualResult dualResult = DBProxy.Current.Select(null, sqlCmd, this.listsqlParameter, out DataTable[] dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.dt_Head = dt[0];
            this.dt_Detail = dt[1];
            if (this.dt_Head.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
            }

            this.grid1.DataSource = this.cbStatus.Text == "All" ? this.dt_Head : this.dt_Head.Select($"Status = '{this.cbStatus.Text}'").TryCopyToDataTable(this.dt_Head);
            this.bindingDetail.DataSource = this.dt_Detail;
        }

        private void Grid1_SelectionChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.grid1.DataSource))
            {
                return;
            }

            if (this.dt_Head == null || this.dt_Head.Rows.Count == 0)
            {
                return;
            }

            if (this.dt_Detail == null || this.dt_Detail.Rows.Count == 0)
            {
                return;
            }

            DataRow dr_Value = this.grid1.GetDataRow(this.grid1.GetSelectedRowIndex());

            if (dr_Value != null)
            {
                string strID = string.IsNullOrEmpty(dr_Value["ID"].ToString()) ? string.Empty : dr_Value["ID"].ToString();
                string strRefno = string.IsNullOrEmpty(dr_Value["Refno"].ToString()) ? string.Empty : dr_Value["Refno"].ToString();
                string strColor = string.IsNullOrEmpty(dr_Value["Color"].ToString()) ? string.Empty : dr_Value["Color"].ToString();
                this.bindingDetail.Filter = $"ID = '{strID}' and Refno = '{strRefno}' and Color = '{strColor}'";
            }
            else
            {
                this.bindingDetail.Filter = $"ID = '' and Refno = '' and Color = ''";
            }
        }

        private void CbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dt_Head.Rows.Count == 0)
            {
                return;
            }

            if (this.cbStatus.Text != "All")
            {
                this.grid1.DataSource = this.dt_Head.Select($"Status = '{this.cbStatus.Text}'").TryCopyToDataTable(this.dt_Head);
            }
            else
            {
                this.grid1.DataSource = this.dt_Head;
            }
        }
    }
}
