using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Quality
{
    public partial class P01 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        private bool boolFromP01;

        int index;
        string find = string.Empty;
        DataRow[] find_dr;

        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgridmenus.Items.Remove(this.appendmenu);
            this.detailgridmenus.Items.Remove(this.modifymenu);
            this.detailgridmenus.Items.Remove(this.deletemenu);
            this.detailgridmenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.modifyPhysicalInspectionToolStripMenuItem,
                this.modifyWeightTestToolStripMenuItem,
                this.modifyShadeBondTestToolStripMenuItem,
                this.modifyContinuityTestToolStripMenuItem,
                this.modifyOdorTestToolStripMenuItem,
            });

            // 關閉表身Grid DoubleClick 會新增row的問題
            this.InsertDetailGridOnDoubleClick = false;
            this.boolFromP01 = false;
        }

        public P01(string Poid) // for Form直接call form
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("ID = '{0}'", Poid);
            this.InsertDetailGridOnDoubleClick = false;
            this.IsSupportEdit = false;
            this.boolFromP01 = true;
        }

        protected override void OnFormLoaded()
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, ",last two years data");
            if (this.boolFromP01)
            {
                this.ExpressQuery = false;
            }
            else
            {
                this.queryfors.SelectedIndex = 1;
                this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                this.ExpressQuery = true;
            }

            base.OnFormLoaded();

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    case 1:
                        this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                        break;
                }

                this.ReloadDatas();
            };
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmd = string.Format(
@"Select a.id,a.poid,a.SEQ1,a.SEQ2,Receivingid,a.Refno,a.SCIRefno,Suppid,
ArriveQty,InspDeadline,Result,
PhysicalEncode,WeightEncode,ShadeBondEncode,ContinuityEncode,
NonPhysical,Physical,TotalInspYds,PhysicalDate,Physical,
NonWeight, Weight,WeightDate,Weight,
NonShadebond,Shadebond,ShadebondDate,shadebond,
NonContinuity,Continuity,ContinuityDate,Continuity,
a.Status,ReplacementReportID,(a.seq1+a.seq2) as seq,
(Select weavetypeid from Fabric b WITH (NOLOCK) where b.SCIRefno =a.SCIrefno) as weavetypeid,
c.Exportid,c.whseArrival,dbo.getPass1(a.Approve) as approve1,approveDate,approve,
d.ColorID,
(Select ID+' - '+ AbbEn From Supp WITH (NOLOCK) Where a.suppid = supp.id) as SuppEn,
c.ExportID as Wkno
,cn.name
,a.nonOdor
,a.Odor
,a.OdorEncode
,a.OdorDate
,[PhysicalInspector] = (select name from pass1 where id = a.PhysicalInspector)
,[WeightInspector] = (select name from pass1 where id = a.WeightInspector)
,[ShadeboneInspector] = (select name from pass1 where id = a.ShadeboneInspector)
,[ContinuityInspector] = (select name from pass1 where id = a.ContinuityInspector)
,[OdorInspector] = (select name from pass1 where id = a.OdorInspector)
From FIR a WITH (NOLOCK) Left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
inner join PO_Supp_Detail d WITH (NOLOCK) on d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
outer apply(select name from color WITH (NOLOCK) where color.id = d.colorid and color.BrandId = d.BrandId)cn
Where a.poid='{0}' order by a.seq1,a.seq2", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup() // Grid 設定
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorCheckBoxColumnSettings nonPhy = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonWei = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonSha = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonCon = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings nonOdor = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings phy = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings phyD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorNumericColumnSettings phyYds = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings Wei = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings WeiD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings sha = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings shaD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings Con = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings ConD = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings Odor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorDateColumnSettings OdorD = new DataGridViewGeneratorDateColumnSettings();
            #region ClickEvent
            phy.CellMouseDoubleClick += (s, e) =>
            {
                    var dr = this.CurrentDetailData;
                    if (dr == null)
                {
                    return;
                }

                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                    frm.ShowDialog(this);
                    frm.Dispose();
                    this.RenewData();
            };
            phyD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_PhysicalInspection(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            phyYds.CellFormatting += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string sqlcmd = $@"
select 1 from FIR_Physical with(nolock)
where  id = {dr["id"]}
and TicketYds <> ActualYds
and ActualYds > 0
";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LemonChiffon;
                }
            };
            phyYds.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_PhysicalInspection(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            Wei.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_Weight(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            WeiD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_Weight(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            sha.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_ShadeBond(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            shaD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_ShadeBond(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            Con.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_Continuity(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            ConD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_Continuity(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            Odor.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_Odor(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            OdorD.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P01_Odor(false, this.CurrentDetailData["ID"].ToString(), null, null, dr);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            #endregion
            #region Validat & Editable
            nonPhy.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved")
                {
                    e.IsEditable = false;
                }
            };
            nonPhy.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["NonPhysical"] = e.FormattedValue;
                dr.EndEdit();
                DataTable dt = (DataTable)this.detailgridbs.DataSource;
                this.FinalResult(dr);
            };
            nonWei.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved")
                {
                    e.IsEditable = false;
                }
            };
            nonWei.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["NonWeight"] = e.FormattedValue;
                dr.EndEdit();
                this.FinalResult(dr);
            };
            nonSha.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved")
                {
                    e.IsEditable = false;
                }
            };
            nonSha.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["NonShadeBond"] = e.FormattedValue;
                dr.EndEdit();
                this.FinalResult(dr);
            };
            nonCon.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved")
                {
                    e.IsEditable = false;
                }
            };
            nonCon.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["NonContinuity"] = e.FormattedValue;
                dr.EndEdit();
                this.FinalResult(dr);
            };
            nonOdor.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["Status"].ToString() == "Approved")
                {
                    e.IsEditable = false;
                }
            };
            nonOdor.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["nonOdor"] = e.FormattedValue;
                dr.EndEdit();
                this.FinalResult(dr);
            };
            #endregion
            #region set grid
            this.detailgrid.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("WKNO", header: "Wkno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(26), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("name", header: "Color Name", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Text("weavetypeid", header: "Weave Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Result", header: "Over all\n Result", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .CheckBox("NonPhysical", header: "Physical N/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonPhy)
                .Text("Physical", header: "Physical\n Inspection", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: phy)
                .Numeric("TotalInspYds", header: "Act. Ttl Ysd\nInspection", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true, settings: phyYds)
                .Date("PhysicalDate", header: "Last Phy.\nInsp. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: phyD)
                .CheckBox("NonWeight", header: "Weight N/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonWei)
                .Text("Weight", header: "Weight\n Test", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: Wei)
                .Date("WeightDate", header: "Last Wei.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: WeiD)
                .CheckBox("NonShadeBond", header: "Shade\nBandN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonSha)
                .Text("Shadebond", header: "Shade\nBand", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: sha)
                .Date("ShadeBondDate", header: "Last Shade.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: shaD)
                .CheckBox("NonContinuity", header: "Continuity \nN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonCon)
                .Text("Continuity", header: "Continuity", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: Con)
                .Date("ContinuityDate", header: "Last Cont.\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: ConD)

                .CheckBox("nonOdor", header: "Odor \nN/A", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: nonOdor)
                .Text("Odor", header: "Odor", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: Odor)
                .Date("OdorDate", header: "Last Odor\nTest. Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: OdorD)

                .Text("Approve1", header: "Approve", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ReplacementReportID", header: "1st Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Receivingid", header: "Receiving ID", width: Widths.AnsiChars(13), iseditingreadonly: true);
            this.detailgrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.detailgrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);

            // 紅粉佳人組
            this.detailgrid.Columns["NonPhysical"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["NonWeight"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["NonShadeBond"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["NonContinuity"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.detailgrid.Columns["nonOdor"].DefaultCellStyle.BackColor = Color.MistyRose;

            // 檸檬薄紗組
            this.detailgrid.Columns["Physical"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["TotalInspYds"].DefaultCellStyle.BackColor = Color.LemonChiffon;
            this.detailgrid.Columns["PhysicalDate"].DefaultCellStyle.BackColor = Color.LemonChiffon;

            // 青藍組
            this.detailgrid.Columns["Weight"].DefaultCellStyle.BackColor = Color.LightCyan;
            this.detailgrid.Columns["WeightDate"].DefaultCellStyle.BackColor = Color.LightCyan;

            // 青綠組
            this.detailgrid.Columns["Shadebond"].DefaultCellStyle.BackColor = Color.LightGreen;
            this.detailgrid.Columns["ShadeBondDate"].DefaultCellStyle.BackColor = Color.LightGreen;

            // 復古色組
            this.detailgrid.Columns["Continuity"].DefaultCellStyle.BackColor = Color.AntiqueWhite;
            this.detailgrid.Columns["ContinuityDate"].DefaultCellStyle.BackColor = Color.AntiqueWhite;

            // Lavender
            this.detailgrid.Columns["Odor"].DefaultCellStyle.BackColor = Color.Lavender;
            this.detailgrid.Columns["OdorDate"].DefaultCellStyle.BackColor = Color.Lavender;
            #endregion

        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            DataRow queryDr;
            DualResult dResult = PublicPrg.Prgs.QueryQaInspectionHeader(this.CurrentMaintain["ID"].ToString(), out queryDr);
            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            DataTable sciTb;
            string query_cmd = string.Format(" select * from [dbo].[Getsci]('{0}','{1}')", this.CurrentMaintain["ID"], MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Category"]);
            DBProxy.Current.Select(null, query_cmd, out sciTb);
            if (!dResult)
            {
                this.ShowErr(query_cmd, dResult);
                return;
            }

            if (sciTb.Rows.Count > 0)
            {
                // if (sciTb.Rows[0]["MinSciDelivery"] == DBNull.Value) dateEarliestSCIDel.Text = "";
                // else dateEarliestSCIDel.Text = Convert.ToDateTime(sciTb.Rows[0]["MinSciDelivery"]).ToShortDateString();
            }
            else
            {
                // dateEarliestSCIDel.Text = "";
                this.dateEarliestEstCutDate.Text = string.Empty;
            }

            DateTime? targT;
            if (MyUtility.Check.Empty(queryDr))
            {
                targT = null;
                this.dateEarliestEstCutDate.Text = string.Empty;
            }
            else
            {
                 targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["CUTINLINE"], sciTb.Rows[0]["MinSciDelivery"]);
                 if (queryDr["cutinline"] == DBNull.Value)
                {
                    this.dateEarliestEstCutDate.Text = string.Empty;
                }
                else
                {
                    this.dateEarliestEstCutDate.Text = Convert.ToDateTime(queryDr["cutinline"]).ToShortDateString();
                }
            }

            if (targT != null)
            {
                this.dateTargetLeadTime.Text = ((DateTime)targT).ToShortDateString();
            }
            else
            {
                this.dateTargetLeadTime.Text = string.Empty;
            }

            this.displayStyle.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Styleid"].ToString();
            this.displaySeason.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["Seasonid"].ToString();
            this.displayBrand.Text = MyUtility.Check.Empty(queryDr) ? string.Empty : queryDr["brandid"].ToString();

            this.displayMTLCmlpt.Text = (bool)this.CurrentMaintain["Complete"] ? "Y" : string.Empty;
            decimal detailRowCount = this.DetailDatas.Count;
            string inspnum = "0";
            DataTable detailTb = (DataTable)this.detailgridbs.DataSource;
            if (detailRowCount != 0)
            {
                if (detailTb.Rows.Count != 0)
                {
                    DataRow[] inspectAry = detailTb.Select("Result<>'' or (nonphysical and nonweight and nonshadebond and noncontinuity and nonOdor)");
                    if (inspectAry.Length > 0)
                    {
                        inspnum = Math.Round(((decimal)inspectAry.Length / detailRowCount) * 100, 2).ToString();
                    }
                }
            }

            // displayofInspection.Text = inspnum;
            DateTime? completedate, Physicalcompletedate, Weightcompletedate, ShadeBondcompletedate, Continuitycompletedate;
            if (inspnum == "100")
            {
                Physicalcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(PhysicalDate)", string.Empty));
                Weightcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(WeightDate)", string.Empty));
                ShadeBondcompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(ShadeBondDate)", string.Empty));
                Continuitycompletedate = MyUtility.Convert.GetDate(detailTb.Compute("Max(ContinuityDate)", string.Empty));
                if (MyUtility.Math.DateMinus(Physicalcompletedate, Weightcompletedate).TotalSeconds < 0)
                {
                    completedate = Weightcompletedate;
                }
                else
                {
                    completedate = Physicalcompletedate;
                }

                if (MyUtility.Math.DateMinus(completedate, ShadeBondcompletedate).TotalSeconds < 0)
                {
                    completedate = ShadeBondcompletedate;
                }

                if (MyUtility.Math.DateMinus(completedate, Continuitycompletedate).TotalSeconds < 0)
                {
                    completedate = Continuitycompletedate;
                }

                this.dateCompletionDate.Text = completedate == null ? string.Empty : ((DateTime)completedate).ToShortDateString();
            }
            else
            {
                this.dateCompletionDate.Text = string.Empty;
            }

            #region Box 顏色
            this.dispLengthofdifference.BackColor = Color.Pink;
            this.displayavailablemodified.BackColor = Color.MistyRose;
            this.displayPhysicalInsp.BackColor = Color.LemonChiffon;
            this.displayWeightTest.BackColor = Color.LightCyan;
            this.displayShadeBond.BackColor = Color.LightGreen;
            this.displayContinuity.BackColor = Color.AntiqueWhite;
            this.displayOdor.BackColor = Color.Lavender;
            #endregion
            this.detailgrid.AutoResizeColumns();
        }

        protected override DualResult ClickSave()
        {
            // 因為表頭是PO不能覆蓋其他資料，必需自行存檔
            string save_po_cmd = string.Format("update po set FIRRemark = '{0}' where id = '{1}';", this.editRemark.Text.ToString(), this.CurrentMaintain["ID"]);

            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    int nonph = dr["NonPhysical"].ToString() == "True" ? 1 : 0;
                    int nonwei = dr["NonWeight"].ToString() == "True" ? 1 : 0;
                    int nonsha = dr["NonShadeBond"].ToString() == "True" ? 1 : 0;
                    int noncon = dr["NonContinuity"].ToString() == "True" ? 1 : 0;
                    int nonOdor = dr["NonOdor"].ToString() == "True" ? 1 : 0;
                    save_po_cmd = save_po_cmd + string.Format(
                    @"Update FIR Set Result = '{0}',NonPhysical = {1},NonWeight = {2},
                    NonShadeBond = {3},NonContinuity = {4},Status = '{6}',NonOdor={7}
                    Where ID = '{5}';",
                    dr["Result"], nonph, nonwei, nonsha, noncon, dr["ID"], dr["Status"], nonOdor);
                }
                #region 重新判斷AllResult

                // 重新判斷AllResult
                // string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(dr);
                // dr["Result"] = returnstr[0];
                // dr["Status"] = returnstr[1];
                #endregion
            }

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, save_po_cmd)))
                    {
                        _transactionscope.Dispose();
                        return upResult;
                    }

                    // 更新PO.FIRInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{this.CurrentMaintain["ID"]}'")))
                    {
                        _transactionscope.Dispose();
                        return upResult;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return Result.True;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;

            return Result.True;
        }

        // 判斷並回寫Physical OverallResult, Status string[0]=Result, string[1]=status
        public void FinalResult(DataRow dr)
        {
            if (this.EditMode) // Status = Confirm 才會判斷
            {
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(dr);

                dr["Result"] = returnstr[0];
                dr["Status"] = returnstr[1];
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            DataTable detDtb = (DataTable)this.detailgridbs.DataSource;

            // 移到指定那筆
            string wk = this.txtLocateforWK.Text;
            string seq1 = this.txtSEQ1.Text;
            string seq2 = this.txtSEQ2.Text;
            string find_new = string.Empty;

            if (!MyUtility.Check.Empty(wk))
            {
                find_new = string.Format("Exportid='{0}'", wk);
            }

            if (!MyUtility.Check.Empty(seq1))
            {
               if (!MyUtility.Check.Empty(find_new))
               {
                   find_new = find_new + string.Format(" and SEQ1 = '{0}'", seq1);
               }
               else
               {
                   find_new = string.Format("SEQ1 = '{0}'", seq1);
               }
            }

            if (!MyUtility.Check.Empty(seq2))
            {
               if (!MyUtility.Check.Empty(find_new))
               {
                   find_new = find_new + string.Format(" and SEQ2 = '{0}'", seq2);
               }
               else
               {
                   find_new = string.Format("SEQ2 = '{0}'", seq2);
               }
            }

            if (this.find != find_new)
            {
                this.find = find_new;
                this.find_dr = detDtb.Select(find_new);
                if (this.find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Data not Found.");
                    return;
                }
                else
                {
                    this.index = 0;
                }
            }
            else
            {
                this.index++;
                if (this.find_dr == null)
                {
                    return;
                }

                if (this.index >= this.find_dr.Length)
                {
                    this.index = 0;
                }
            }

            this.detailgridbs.Position = this.DetailDatas.IndexOf(this.find_dr[this.index]);
        }

        override protected DetailGridContextMenuMode CurrentDetailGridContextMenuMode()
        {
            // 非編輯狀態不顯示
            if (!this.EditMode)
            {
                return DetailGridContextMenuMode.Editable;
            }

            return DetailGridContextMenuMode.None;
        }

        private void modifyPhysicalInspectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_PhysicalInspection(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            this.OnDetailEntered();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        private void modifyWeightTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            var currentID = this.CurrentDetailData["ID"].ToString();

            var frm = new Sci.Production.Quality.P01_Weight(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            this.OnDetailEntered();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        private void modifyShadeBondTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_ShadeBond(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();
            this.OnDetailEntered();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        private void modifyContinuityTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_Continuity(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();

            // 重新計算表頭資料
            this.OnDetailEntered();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        private void modifyOdorTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            var currentID = this.CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P01_Odor(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr);
            frm.ShowDialog(this);
            frm.Dispose();
            this.RenewData();

            // 重新計算表頭資料
            this.OnDetailEntered();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }
    }
}
