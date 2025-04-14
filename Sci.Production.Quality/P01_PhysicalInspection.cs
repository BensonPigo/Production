using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using Sci.Utility.Excel;
using System.IO;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_PhysicalInspection : Win.Subs.Input4
    {
        private readonly DataRow maindr;
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private string excelFile = string.Empty;
        private DataTable Fir_physical_Defect;
        private string FirID;
        private bool needCheckInspectionGroup = false;
        private bool isColorFormat = false;
        private bool isFormatInP01 = false;
        private string weaveTypeid = string.Empty;
        private double cutWidth = 0;

        /// <inheritdoc/>
        public P01_PhysicalInspection(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();

            this.txtsupplier.TextBox1.IsSupportEditMode = false;
            this.txtsupplier.TextBox1.ReadOnly = true;
            this.txtuserApprover.TextBox1.IsSupportEditMode = false;
            this.txtuserApprover.TextBox1.ReadOnly = true;
            this.maindr = mainDr;
            this.FirID = keyvalue1;
            this.QueryHeader();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.Button_enable();
        }

        /// <inheritdoc/>
        protected override void OnMaintainEntered()
        {
            base.OnMaintainEntered();
            if ((this.isColorFormat || this.isFormatInP01) && this.gridbs.DataSource != null)
            {
                DataTable dtFir_Physical = (DataTable)this.gridbs.DataSource;
                foreach (DataRow rowFir_Physical in dtFir_Physical.Rows)
                {
                    this.RefreshGrade(rowFir_Physical);
                }
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery()
        {
            this.QueryHeader();
            return base.OnRequery();
        }

        /// <inheritdoc/>
        protected override void OnRequired()
        {
            base.OnRequired();
            if (this.isColorFormat || this.isFormatInP01)
            {
                DataTable dtFir_Physical = (DataTable)this.gridbs.DataSource;
                foreach (DataRow rowFir_Physical in dtFir_Physical.Rows)
                {
                    this.RefreshGrade(rowFir_Physical);
                }
            }
        }

        private void QueryHeader()
        {
            #region Encode/Approve Enable
            this.Button_enable();
            this.btnEncode.Text = MyUtility.Convert.GetBool(this.maindr["PhysicalEncode"]) ? "Amend" : "Encode";
            this.btnApprove.Text = this.maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion

            this.save.Enabled = !MyUtility.Convert.GetBool(this.maindr["PhysicalEncode"]);

            // SendMail
            this.btnSendMail.Enabled = MyUtility.Convert.GetBool(this.maindr["PhysicalEncode"]);

            string order_cmd = string.Format("Select * from View_WH_Orders WITH (NOLOCK) where id='{0}'", this.maindr["POID"]);
            DataRow order_dr;
            if (MyUtility.Check.Seek(order_cmd, out order_dr))
            {
                this.displayBrand.Text = order_dr["Brandid"].ToString();
                this.displayStyle.Text = order_dr["Styleid"].ToString();
            }
            else
            {
                this.displayBrand.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
            }

            string receiving_cmd = string.Format("select a.exportid,a.WhseArrival ,b.Refno from Receiving a WITH (NOLOCK) inner join FIR b WITH (NOLOCK) on a.Id=b.Receivingid where b.id='{0}'", this.maindr["id"]);
            DataRow rec_dr;
            if (MyUtility.Check.Seek(receiving_cmd, out rec_dr))
            {
                this.displayBrandRefno.Text = rec_dr["Refno"].ToString();
            }
            else
            {
                this.displayBrandRefno.Text = string.Empty;
            }

            string po_cmd = string.Format("Select * from po_supp WITH (NOLOCK) where id='{0}' and seq1 = '{1}'", this.maindr["POID"], this.maindr["seq1"]);
            DataRow po_dr;
            if (MyUtility.Check.Seek(po_cmd, out po_dr))
            {
                this.txtsupplier.TextBox1.Text = po_dr["suppid"].ToString();
            }
            else
            {
                this.txtsupplier.TextBox1.Text = string.Empty;
            }

            string po_supp_detail_cmd = $"select ColorID = isnull(SpecValue ,'') from PO_Supp_Detail_Spec WITH (NOLOCK) where id='{this.maindr["POID"]}' and seq1='{this.maindr["seq1"]}' and seq2='{this.maindr["seq2"]}' and SpecColumnID = 'Color'";
            if (MyUtility.Check.Seek(po_supp_detail_cmd, out DataRow po_supp_detail_dr))
            {
                this.displayColor.Text = po_supp_detail_dr["colorid"].ToString();
            }
            else
            {
                this.displayColor.Text = string.Empty;
            }

            this.displaySCIRefno.Text = this.maindr["SCIRefno"].ToString();
            this.displayApprover.Text = this.maindr["ApproveDate"].ToString();
            this.displayArriveQty.Text = this.maindr["arriveQty"].ToString();
            this.dateArriveWHDate.Value = MyUtility.Convert.GetDate(this.maindr["whseArrival"]);
            this.dateLastInspectionDate.Value = MyUtility.Convert.GetDate(this.maindr["physicalDate"]);
            this.displaySEQ.Text = this.maindr["Seq1"].ToString() + "-" + this.maindr["Seq2"].ToString();
            this.displaySP.Text = this.maindr["POID"].ToString();
            this.displayWKNo.Text = this.maindr["Exportid"].ToString();
            this.checkNonInspection.Value = this.maindr["nonphysical"].ToString();
            this.displayResult.Text = this.maindr["physical"].ToString();
            this.txtuserApprover.TextBox1.Text = this.maindr["Approve"].ToString();
            this.txtPhysicalInspector.Text = this.maindr["PhysicalInspector"].ToString();
            this.txtCustInspNumber.Text = this.maindr["CustInspNumber"].ToString();

            string sqlCmd = $@"
select  InspectionGroup,
        WeaveTypeId,
        Description,
        descDetail,
        Width
from Fabric with (nolock) where SCIRefno = '{this.maindr["SCIRefno"].ToString()}'";
            DataRow drFabricInfo;
            if (MyUtility.Check.Seek(sqlCmd, out drFabricInfo))
            {
                this.txtGroup.Text = drFabricInfo["InspectionGroup"].ToString();
                this.weaveTypeid = drFabricInfo["WeaveTypeId"].ToString();
                this.displaySCIRefno1.Text = drFabricInfo["Description"].ToString();
                this.displaydescDetail.Text = drFabricInfo["descDetail"].ToString();
                this.cutWidth = MyUtility.Convert.GetDouble(drFabricInfo["Width"]);
            }
            else
            {
                this.txtGroup.Text = string.Empty;
                this.weaveTypeid = string.Empty;
                this.displaySCIRefno1.Text = string.Empty;
                this.displaydescDetail.Text = string.Empty;
                this.cutWidth = 0;
            }

            List<SqlParameter> listNeedCheckInspectionGroupPar = new List<SqlParameter>
            {
                new SqlParameter("@BrandID", this.displayBrand.Text),
                new SqlParameter("@WeaveTypeID", this.weaveTypeid),
            };

            this.needCheckInspectionGroup = MyUtility.Check.Seek(
                $"select 1 from FIR_Grade with (nolock) where BrandID = @BrandID and WeaveTypeID = @WeaveTypeID and InspectionGroup <> ''",
                listNeedCheckInspectionGroupPar);

            this.isColorFormat = MyUtility.Check.Seek(
                $"select 1 from FIR_Grade with (nolock) where BrandID = @BrandID and WeaveTypeID = @WeaveTypeID and IsColorFormat = 1",
                listNeedCheckInspectionGroupPar);

            this.isFormatInP01 = MyUtility.Check.Seek(
                $"select 1 from FIR_Grade with (nolock) where BrandID = @BrandID and WeaveTypeID = @WeaveTypeID and isFormatInP01 = 1",
                listNeedCheckInspectionGroupPar);

            string sqlcmd_TotalRoll = $@"select count(TotalRollnumberTEST.Roll) as TotalRollnumber
                                        from(
                                        select rd.Roll
	                                    from Receiving_Detail rd with(nolock)
                                        where 
                                        rd.poid = '{this.maindr["POID"].ToString()}' and
                                        rd.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                        rd.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                        rd.ID = '{this.maindr["ReceivingID"].ToString()}'
                                        union
	                                    select td.Roll
	                                    from TransferIn_Detail td with(nolock)
                                        where 
                                        td.poid = '{this.maindr["POID"].ToString()}' and
                                        td.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                        td.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                        td.ID = '{this.maindr["ReceivingID"].ToString()}'
                                        ) TotalRollnumberTEST";
            this.displayTotlalRoll.Text = MyUtility.GetValue.Lookup(sqlcmd_TotalRoll, "Production");

            string sqlcmd_InspectedRoll = $@"select count(InspectedRollnumberTEST.Roll) as InspectedRollnumber
                                            from(
                                            select rd.Roll
	                                        from Receiving_Detail rd
	                                        inner join FIR f on f.ReceivingID = rd.Id and f.POID = rd.PoId and f.SEQ1 = rd.Seq1 and f.SEQ2 = rd.Seq2
	                                        inner join FIR_Physical fp on f.id = fp.ID and fp.Roll = rd.Roll and fp.Dyelot = rd.Dyelot
                                            where 
                                            rd.poid = '{this.maindr["POID"].ToString()}' and
                                            rd.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                            rd.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                            rd.ID = '{this.maindr["ReceivingID"].ToString()}' and
                                            fp.Result<>'' 
                                            union
	                                        select td.Roll
	                                        from TransferIn_Detail td
	                                        inner join FIR f on f.ReceivingID = td.Id and f.POID = td.PoId and f.SEQ1 = td.Seq1 and f.SEQ2 = td.Seq2
	                                        inner join FIR_Physical fp on f.id = fp.ID and fp.Roll = td.Roll and fp.Dyelot = td.Dyelot
                                            where 
                                            td.poid = '{this.maindr["POID"].ToString()}' and
                                            td.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                            td.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                            td.ID = '{this.maindr["ReceivingID"].ToString()}' and
                                            fp.Result<>''
                                            ) InspectedRollnumberTEST";
            this.displayInspectedRoll.Text = MyUtility.GetValue.Lookup(sqlcmd_InspectedRoll, "Production");

            string sqlcmd_TotalNumber = $@"select Count(distinct Dyelot) as TotalLotNumber
                                            from(
                                            select rd.Dyelot
                                            from Receiving_Detail rd with(nolock)
                                            where 
                                            rd.poid = '{this.maindr["POID"].ToString()}' and
                                            rd.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                            rd.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                            rd.ID = '{this.maindr["ReceivingID"].ToString()}'
                                            union
	                                        select td.Dyelot
	                                        from TransferIn_Detail td with(nolock)
	                                        where 
                                            td.poid = '{this.maindr["POID"].ToString()}' and
                                            td.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                            td.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                            td.ID = '{this.maindr["ReceivingID"].ToString()}') TotalLotNumberTEST";
            this.displayTotalLot.Text = MyUtility.GetValue.Lookup(sqlcmd_TotalNumber, "Production");

            string sqlcmd_InspectedLot = $@"select Count(distinct Dyelot) as InspectedLotNumber
                                            from(
	                                        select rd.Dyelot
	                                        from Receiving_Detail rd
	                                        inner join FIR f on f.ReceivingID = rd.Id and f.POID = rd.PoId and f.SEQ1 = rd.Seq1 and f.SEQ2 = rd.Seq2
	                                        inner join FIR_Physical fp on f.id = fp.ID and fp.Roll = rd.Roll and fp.Dyelot = rd.Dyelot
	                                        where 
                                            rd.poid = '{this.maindr["POID"].ToString()}' and
                                            rd.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                            rd.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                            rd.ID = '{this.maindr["ReceivingID"].ToString()}' and
                                            fp.Result<>''
                                            union
	                                        select td.Dyelot
	                                        from TransferIn_Detail td
	                                        inner join FIR f on f.ReceivingID = td.Id and f.POID = td.PoId and f.SEQ1 = td.Seq1 and f.SEQ2 = td.Seq2
	                                        inner join FIR_Physical fp on f.id = fp.ID and fp.Roll = td.Roll and fp.Dyelot = td.Dyelot
	                                        where  
                                            td.poid = '{this.maindr["POID"].ToString()}' and
                                            td.seq1 = '{this.maindr["Seq1"].ToString()}' and
                                            td.Seq2 = '{this.maindr["Seq2"].ToString()}' and
                                            td.ID = '{this.maindr["ReceivingID"].ToString()}' and
                                            fp.Result<>'') InspectedLotNumberTEST";
            this.displayInspectedLot.Text = MyUtility.GetValue.Lookup(sqlcmd_InspectedLot, "Production");
        }

        private DataTable datas2;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnRequeryPost(DataTable datas)
        {
            this.datas2 = datas;
            if (!datas.Columns.Contains("CutWidth"))
            {
                datas.ColumnsDecimalAdd("CutWidth");
                foreach (DataRow dr in datas.Rows)
                {
                    dr["CutWidth"] = this.cutWidth;
                }
            }

            base.OnRequeryPost(datas);
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("POID", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));
            datas.Columns.Add("NewKey", typeof(int));
            datas.Columns.Add("LthOfDiff", typeof(decimal));
            datas.Columns.Add("Weight", typeof(decimal));
            datas.Columns.Add("ActualWeight", typeof(decimal));
            datas.Columns.Add("Differential", typeof(decimal));
            datas.Columns.Add("ShowGrade", typeof(string));
            int i = 0;
            foreach (DataRow dr in datas.Rows)
            {
                dr["ShowGrade"] = dr["Grade"];
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["NewKey"] = i;
                dr["poid"] = this.maindr["poid"];
                dr["SEQ1"] = this.maindr["SEQ1"];
                dr["SEQ2"] = this.maindr["SEQ2"];
                dr["LthOfDiff"] = MyUtility.Convert.GetDecimal(dr["ActualYds"]) - MyUtility.Convert.GetDecimal(dr["TicketYds"]);

                // 新增Receiving_Detail & TransferIn_Detail來源
                string sqlcmd = $@"
select [Weight] = IIF(ISNULL(rd.weight,0) = 0 ,ISNULL(td.weight,0),ISNULL(rd.weight,0))
,[ActualWeight] = IIF(ISNULL(rd.ActualWeight,0) = 0 ,ISNULL(td.ActualWeight,0),ISNULL(rd.ActualWeight,0))
,[Differential] = IIF(ISNULL(rd.ActualWeight,0) = 0 ,ISNULL(td.ActualWeight,0),ISNULL(rd.ActualWeight,0))-IIF(ISNULL(rd.weight,0) = 0 ,ISNULL(td.weight,0),ISNULL(rd.weight,0))
from FIR_Physical fp
inner join fir f on fp.ID = f.ID
left join Receiving_Detail rd on rd.PoId = f.POID
	and rd.Seq1 = f.SEQ1 and rd.Seq2 = f.SEQ2
	and rd.Roll = fp.Roll and rd.Dyelot = fp.Dyelot
	and rd.Id = f.ReceivingID
left join TransferIn_Detail td on td.PoId = f.POID
	and td.Seq1 = f.SEQ1 and td.Seq2 = f.SEQ2
	and td.Roll = fp.Roll and td.Dyelot = fp.Dyelot
	and td.Id = f.ReceivingID
where fp.DetailUkey = '{dr["DetailUkey"]}'
";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow dr_R))
                {
                    dr["Weight"] = dr_R["Weight"];
                    dr["ActualWeight"] = dr_R["ActualWeight"];
                    dr["Differential"] = dr_R["Differential"];
                }
                else
                {
                    dr["Weight"] = 0;
                    dr["ActualWeight"] = 0;
                    dr["Differential"] = 0;
                }

                i++;
            }
            #region 撈取下一層資料Defect
            string str_defect = string.Format("Select a.* ,b.NewKey from Fir_physical_Defect a WITH (NOLOCK) ,#tmp b Where a.id = b.id and a.FIR_PhysicalDetailUKey = b.DetailUkey");

            MyUtility.Tool.ProcessWithDatatable(datas, "ID,NewKey,DetailUkey", str_defect, out this.Fir_physical_Defect);
            #endregion
        }

        /// <summary>
        /// 刪除第三層資料
        /// By Ukey
        /// </summary>
        /// <param name="strNewKey">需要刪除第三層所對應的 NewUkey</param>
        private void CleanDefect(string strNewKey)
        {
            for (int i = this.Fir_physical_Defect.Rows.Count - 1; i >= 0; i--)
            {
                if (this.Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted
                    && this.Fir_physical_Defect.Rows[i]["NewKey"].EqualString(strNewKey))
                {
                    this.Fir_physical_Defect.Rows[i].Delete();
                }
            }

            this.Get_total_point(this.CurrentData);
        }

        /// <inheritdoc/>
        protected void Get_total_point(DataRow rowFir_Physical)
        {
            double double_ActualYds = MyUtility.Convert.GetDouble(rowFir_Physical["ActualYds"]);
            double double_TicketYds = MyUtility.Convert.GetDouble(rowFir_Physical["TicketYds"]);
            double double_Totalpoint = MyUtility.Convert.GetDouble(rowFir_Physical["Totalpoint"]);
            double actualYdsT = Math.Floor(MyUtility.Convert.GetDouble(rowFir_Physical["ActualYds"]));
            double actualWidth = MyUtility.Convert.GetDouble(rowFir_Physical["actualwidth"]);
            double actualYdsF = actualYdsT - (actualYdsT % 5);
            double def_locT = 0d;
            double def_locF = 0d;

            // Act.Yds Inspected更動時剔除Fir_physical_Defect不在範圍的資料

            // foreach (DataRow dr in Fir_physical_Defect)
            for (int i = 0; i <= this.Fir_physical_Defect.Rows.Count - 1; i++)
            {
                if (this.Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted
                    && this.Fir_physical_Defect.Rows[i]["NewKey"].EqualString(rowFir_Physical["NewKey"]))
                {
                    // if (dr.RowState != DataRowState.Deleted)
                    // {
                    def_locF = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[0]);
                    def_locT = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[1]);
                    if (def_locF >= double_ActualYds && this.Fir_physical_Defect.Rows[i]["NewKey"].ToString() == rowFir_Physical["NewKey"].ToString())
                    {
                        this.Fir_physical_Defect.Rows[i].Delete();
                    }

                    // }

                    // DefectLocation的範圍如果與目前ActualYds界限值不符的話就update DefectLocation
                    if (def_locF == actualYdsF && def_locT != actualYdsT)
                    {
                        this.Fir_physical_Defect.Rows[i]["DefectLocation"] = actualYdsF.ToString().PadLeft(3, '0') + "-" + actualYdsT.ToString().PadLeft(3, '0');
                    }

                    if (def_locT < actualYdsF && def_locT - def_locF != 4)
                    {
                        /*
                         *  如果DefectLocation的相同範圍內有２筆資料,就必須把舊的給刪除
                         *  ex: 080-083 汙點D1, 080-084 汙點A1/D1 ,就必須要080-083給刪除
                         *  不然將080-083 改為080-084 Pkey就會重複
                        */
                        DataRow[] ary = this.Fir_physical_Defect.Select(string.Format(@"NewKey = {0} and DefectLocation like '%{1}%'", rowFir_Physical["NewKey"].ToString(), def_locF));
                        if (ary.Length > 1)
                        {
                            this.Fir_physical_Defect.Rows[i].Delete();
                        }
                        else
                        {
                            this.Fir_physical_Defect.Rows[i]["DefectLocation"] = def_locF.ToString().PadLeft(3, '0') + "-" + (def_locF + 4).ToString().PadLeft(3, '0');
                        }
                    }
                }
            }

            double sumPoint = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Compute("Sum(Point)", string.Format("NewKey = {0}", rowFir_Physical["NewKey"])));

            // PointRate 國際公式每五碼最高20點
            rowFir_Physical["TotalPoint"] = sumPoint;
            #region 依dbo.PointRate 來判斷新的PointRate計算公式
            DataRow drPoint;
            string pointRateID = MyUtility.Check.Seek($@"select * from QABrandSetting where Brandid='{this.displayBrand.Text}'", out drPoint) ? drPoint["PointRateOption"].ToString() : "1";
            decimal pointRate = 0;
            switch (pointRateID)
            {
                case "1":
                    pointRate = (double_ActualYds == 0) ? 0 : MyUtility.Convert.GetDecimal(Math.Round((sumPoint / double_ActualYds) * 100, 2));
                    break;
                case "2":
                    pointRate = (double_ActualYds == 0 || actualWidth == 0) ? 0 : MyUtility.Convert.GetDecimal(Math.Round((sumPoint * 3600) / (double_ActualYds * actualWidth), 2));
                    break;
                case "3":
                    if (this.weaveTypeid == "KNIT")
                    {
                        pointRate = (double_TicketYds == 0 || this.cutWidth == 0) ? 0 : MyUtility.Convert.GetDecimal(Math.Round((sumPoint * 3600) / (double_TicketYds * this.cutWidth), 2));
                    }
                    else
                    {
                        pointRate = (double_ActualYds == 0 || this.cutWidth == 0) ? 0 : MyUtility.Convert.GetDecimal(Math.Round((sumPoint * 3600) / (double_ActualYds * this.cutWidth), 2));
                    }

                    break;
                case "4":
                    if (MyUtility.Check.Seek($@"
select * from FIR_PointRateFormula
where SuppID = '{this.txtsupplier.TextBox1.Text}'
and WeaveTypeID = '{this.weaveTypeid}'
and BrandID = '{this.displayBrand.Text}'
"))
                    {
                        pointRate = (double_ActualYds == 0 || this.cutWidth == 0) ? 0 : MyUtility.Convert.GetDecimal(Math.Round((sumPoint * 3600) / (double_ActualYds * this.cutWidth), 2));
                    }
                    else
                    {
                        pointRate = (double_ActualYds == 0) ? 0 : MyUtility.Convert.GetDecimal(Math.Round((sumPoint / double_ActualYds) * 100, 2));
                    }

                    break;

                default:
                    pointRate = 0;
                    break;
            }

            rowFir_Physical["PointRate"] = pointRate;
            this.RefreshGrade(rowFir_Physical);
            #endregion
        }

        private void RefreshGrade(DataRow rowFir_Physical)
        {
            #region Grade,Result
            string grade_cmd = string.Empty;

            string strTMP = string.Empty;

            List<string> list = new List<string>();

            grade_cmd = $@"
                DECLARE @Point as NUMERIC(10, 2) = {rowFir_Physical["PointRate"]}
                DECLARE @BrandID as varchar(15) = '{this.displayBrand.Text}'
                DECLARE @WeaveTypeID as varchar(15) = '{this.weaveTypeid}'
                DECLARE @InspectionGroup varchar(15) = '{this.txtGroup.Text}'
                Declare @Grade as varchar(10) = ''
                Declare @Result as varchar(10) = ''

                select Grade, Result, isFormatInP01, ShowGrade, IsColorFormat from dbo.GetFirPhysicalGrade(@Point, @BrandID, @WeaveTypeID, @InspectionGroup)";

            DataTable dtGradeResult;
            DualResult result = DBProxy.Current.Select(null, grade_cmd, out dtGradeResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 沒有設定Grade標準，就不做任何事
            if (dtGradeResult.Rows.Count == 0)
            {
                return;
            }

            if (this.needCheckInspectionGroup && MyUtility.Check.Empty(this.txtGroup.Text))
            {
                rowFir_Physical["ShowGrade"] = string.Empty;
                rowFir_Physical["Grade"] = string.Empty;
                rowFir_Physical["Result"] = string.Empty;
            }
            else
            {
                if (MyUtility.Convert.GetBool(rowFir_Physical["ColorToneCheck"]))
                {
                    rowFir_Physical["Result"] = "Fail";
                }
                else
                {
                    rowFir_Physical["Result"] = dtGradeResult.Rows[0]["Result"].ToString();
                }

                rowFir_Physical["Grade"] = dtGradeResult.Rows[0]["Grade"];

                if (MyUtility.Convert.GetBool(dtGradeResult.Rows[0]["isFormatInP01"]))
                {
                    rowFir_Physical["ShowGrade"] = dtGradeResult.Rows[0]["ShowGrade"];
                }
                else
                {
                    rowFir_Physical["ShowGrade"] = dtGradeResult.Rows[0]["Grade"];
                }

                int gridRowIndex = this.GetRowIndex(rowFir_Physical);
                if (MyUtility.Convert.GetBool(dtGradeResult.Rows[0]["IsColorFormat"]))
                {
                    this.grid.Rows[gridRowIndex].Cells["ShowGrade"].Style.BackColor = Color.Yellow;
                }
                else
                {
                    this.grid.Rows[gridRowIndex].Cells["ShowGrade"].Style.BackColor = Color.White;
                }
            }
            #endregion
        }

        private int GetRowIndex(DataRow rowFir_Physical)
        {
            for (int i = 0; i < this.grid.Rows.Count; i++)
            {
                if (this.grid.GetDataRow(i) == rowFir_Physical)
                {
                    return i;
                }
            }

            return -1; // 如果找不到，返回-1
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings ydscell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings totalPointcell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings resulCell = PublicPrg.Prgs.CellResult.GetGridCell();
            DataGridViewGeneratorCheckBoxColumnSettings tonecell = new DataGridViewGeneratorCheckBoxColumnSettings();

            tonecell.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                bool colorToneCheck = (bool)e.FormattedValue;
                string grade_cmd = string.Empty;
                grade_cmd = $@"
                    DECLARE @Point as NUMERIC(10, 2) = {dr["PointRate"]}
                    DECLARE @BrandID as varchar(15) = '{this.displayBrand.Text}'
                    DECLARE @WeaveTypeID as varchar(15) = '{this.weaveTypeid}'
                    DECLARE @InspectionGroup varchar(15) = '{this.txtGroup.Text}'

                    select Grade, Result from dbo.GetFirPhysicalGrade(@Point, @BrandID, @WeaveTypeID, @InspectionGroup)

                    ";

                DataTable dtGradeResult;
                DualResult result = DBProxy.Current.Select(null, grade_cmd, out dtGradeResult);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dr["ColorToneCheck"] = colorToneCheck;
                dr["Result"] = colorToneCheck ? "Fail" : dtGradeResult.Rows[0]["Result"].ToString();
            };

            #region TotalPoint Double Click
            totalPointcell.EditingMouseDoubleClick += (s, e) =>
            {
                this.grid.ValidateControl();
                P01_PhysicalInspection_Defect frm = new P01_PhysicalInspection_Defect(this.Fir_physical_Defect, this.maindr, this.EditMode);
                frm.Set(this.EditMode, this.Datas, this.grid.GetDataRow(e.RowIndex));
                frm.ShowDialog(this);
                if (this.EditMode)
                {
                    this.Get_total_point(this.CurrentData);
                }
            };
            #endregion
            #region Roll
            rollcell.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string originRoll = dr["Roll"].ToString();
                    string strNewKey = dr["NewKey"].ToString();

                    SelectItem sele;
                    string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' order by dyelot", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"]);

                    if (!MyUtility.Check.Seek(roll_cmd))
                    {
                        roll_cmd = string.Format("Select roll,dyelot,StockQty=qty from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' order by dyelot", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"]);
                    }

                    sele = new SelectItem(roll_cmd, "15,10,10", dr["roll"].ToString(), false, ",", columndecimals: "0,0,2");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (originRoll.EqualString(sele.GetSelecteds()[0]["Roll"].ToString().Trim()))
                    {
                        dr["Roll"] = sele.GetSelecteds()[0]["Roll"].ToString().Trim();
                        return;
                    }

                    dr["Roll"] = sele.GetSelecteds()[0]["Roll"].ToString().Trim();
                    dr["Dyelot"] = sele.GetSelecteds()[0]["Dyelot"].ToString().Trim();
                    dr["Ticketyds"] = sele.GetSelecteds()[0]["StockQty"].ToString().Trim();
                    dr["CutWidth"] = this.cutWidth;

                    if (this.needCheckInspectionGroup && MyUtility.Check.Empty(this.txtGroup.Text))
                    {
                        dr["Grade"] = string.Empty;
                        dr["Result"] = string.Empty;
                    }
                    else
                    {
                        dr["Result"] = "Pass";
                        dr["Grade"] = "A";
                        dr["ShowGrade"] = "A";
                    }

                    dr["totalpoint"] = 0.00;
                    this.CleanDefect(strNewKey);
                    dr.EndEdit();
                }
            };
            rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string strNewKey = dr["NewKey"].ToString();
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    // 沒填入資料,清空dyelot
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr["Ticketyds"] = 0.00;
                    dr["Actualyds"] = 0.00;
                    dr["CutWidth"] = 0.00;
                    dr["fullwidth"] = 0.00;
                    dr["actualwidth"] = 0.00;
                    dr["totalpoint"] = 0.00;
                    dr["pointRate"] = 0.00;
                    dr["Result"] = string.Empty;
                    dr["Grade"] = string.Empty;
                    dr["moisture"] = 0;
                    dr["Remark"] = string.Empty;
                    this.CleanDefect(strNewKey);
                    dr.EndEdit();
                    return;
                }

                if (oldvalue == newvalue)
                {
                    return;
                }

                string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue.ToString().Replace("'", "''"));

                if (!MyUtility.Check.Seek(roll_cmd))
                {
                    roll_cmd = string.Format("Select roll,dyelot,StockQty=qty from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue.ToString().Replace("'", "''"));
                }

                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr["Ticketyds"] = roll_dr["StockQty"];
                    dr["CutWidth"] = this.cutWidth;
                    if ((this.displayBrand.Text != "LLL") && this.txtGroup.Text != string.Empty)
                    {
                        dr["Result"] = "Pass";
                        dr["Grade"] = "A";
                        dr["ShowGrade"] = "A";
                    }

                    dr["totalpoint"] = 0.00;
                    this.CleanDefect(strNewKey);
                    dr.EndEdit();
                }
                else
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr["Ticketyds"] = 0.00;
                    dr["Actualyds"] = 0.00;
                    dr["CutWidth"] = 0.00;
                    dr["fullwidth"] = 0.00;
                    dr["actualwidth"] = 0.00;
                    dr["totalpoint"] = 0.00;
                    dr["pointRate"] = 0.00;
                    dr["moisture"] = 0;
                    dr["Remark"] = string.Empty;
                    dr.EndEdit();
                    this.CleanDefect(strNewKey);
                    dr["Result"] = string.Empty;
                    dr["Grade"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> data not found!", e.FormattedValue));
                    return;
                }
            };
            #endregion
            #region Act Yds
            ydscell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Actualyds"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                // 判斷Actualyds調整範圍後Fir_physical_Defect會被清掉的話需要提示
                double double_ActualYds = Convert.ToDouble(newvalue);
                double actualYdsF = double_ActualYds - (double_ActualYds % 5);
                StringBuilder hintmsg = new StringBuilder();

                // check FIR_Physical_Defect
                for (int i = 0; i <= this.Fir_physical_Defect.Rows.Count - 1; i++)
                {
                    if (this.Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted)
                    {
                        double def_locF = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[0]);
                        double def_locT = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[1]);
                        if ((def_locF >= double_ActualYds || def_locT > double_ActualYds) && this.Fir_physical_Defect.Rows[i]["NewKey"].ToString() == this.CurrentData["NewKey"].ToString())
                        {
                            hintmsg.Append("Yds : " + this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString() + " , Defects : " + this.Fir_physical_Defect.Rows[i]["DefectRecord"].ToString() +
                                                " , Point : " + this.Fir_physical_Defect.Rows[i]["Point"].ToString() + "\n");
                        }
                    }
                }

                string sqlCheck = $@"select * from dbo.FIR_Physical_Defect_Realtime where FIR_PhysicalDetailUkey = {dr["DetailUkey"]} and  Yards > {double_ActualYds}";
                DualResult result = DBProxy.Current.Select("", sqlCheck, out DataTable dtCheck);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dtCheck != null)
                {
                    for (int i = 0; i <= dtCheck.Rows.Count - 1; i++)
                    {
                        if (i == 0)
                        {
                            hintmsg.Append("-----------------------------------------------\n");
                        }

                        hintmsg.Append("Yds : " + dtCheck.Rows[i]["Yards"].ToString() + " , Defects : " + dtCheck.Rows[i]["FabricdefectID"].ToString() + "\n");
                    }
                }

                if (hintmsg.Length > 0)
                {
                    hintmsg.Insert(0, string.Format("Position greater than {0} is defective\nChanging yds will remove the following defects\nDo you wish to change ?\n", newvalue));
                    if (MyUtility.Msg.WarningBox(hintmsg.ToString(), buttons: MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        e.FormattedValue = oldvalue;
                        dr["Actualyds"] = oldvalue;
                        dr.EndEdit();
                        return;
                    }
                    else
                    {
                        string update_cmd = $@" Delete From FIR_Physical_Defect_Realtime Where FIR_PhysicalDetailUKey = {dr["DetailUkey"]} and Yards > {double_ActualYds};";
                        DualResult upResult = DBProxy.Current.Execute("Production", update_cmd);
                        if (!upResult)
                        {
                            this.ShowErr(upResult);
                            return;
                        }

                        // 判斷是否需要刪除 FIR_Physical_Defect
                        for (int i = 0; i <= this.Fir_physical_Defect.Rows.Count - 1; i++)
                        {
                            if (this.Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted)
                            {
                                double def_locF = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[0]);
                                double def_locT = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[1]);

                                // 判斷是否需要刪除, 代表區間最小值 > TotalYDS 那這區間資料就要刪除
                                if (def_locF > double_ActualYds && this.Fir_physical_Defect.Rows[i]["NewKey"].ToString() == this.CurrentData["NewKey"].ToString())
                                {
                                    this.Fir_physical_Defect.Rows[i].Delete();
                                }

                                // 檢查此區間內 FIR_Physical_Defect_Realtime是否有資料，沒資料也要被delete
                                string sqlCheck2 = $@"select id from FIR_Physical_Defect_Realtime where 
                        FIR_PhysicalDetailUkey = {dr["DetailUkey"]} and Yards >= {def_locF} and Yards <= {def_locT}";
                                if (!MyUtility.Check.Seek(sqlCheck2, "Production"))
                                {
                                    this.Fir_physical_Defect.Rows[i].Delete();
                                }
                            }
                        }
                    }
                }

                // 若新的 Act.Yds\nInspected = 0，則第三層必須清空
                if (newvalue.EqualDecimal(0))
                {
                    this.CleanDefect(dr["NewKey"].ToString());
                }

                dr["Actualyds"] = e.FormattedValue;
                dr.EndEdit();
                this.Get_total_point(this.CurrentData);
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Actualyds", header: "Act.Yds\nInspected", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, settings: ydscell)
            .Numeric("LthOfDiff", header: "Lth. Of Diff", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Text("TransactionID", header: "TransactionID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("CutWidth", header: "Cut. Width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
            .Numeric("fullwidth", header: "Full width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2)
            .Numeric("actualwidth", header: "Actual Width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2)
            .Numeric("totalpoint", header: "Total Points", width: Widths.AnsiChars(7), integer_places: 6, iseditingreadonly: true, settings: totalPointcell)
            .Numeric("pointRate", header: "Point Rate \nper 100yds", width: Widths.AnsiChars(5), iseditingreadonly: true, integer_places: 6, decimal_places: 2)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .CheckBox("ColorToneCheck", header: "Shade in roll fail", width: Widths.AnsiChars(5), iseditable: true, trueValue: 1, falseValue: 0, settings: tonecell)
            .Text("ShowGrade", header: "Grade", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .CheckBox("IsGrandCCanUse", header: "Fail But Can Use", width: Widths.AnsiChars(5), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("GrandCCanUseReason", header: "Fail But Can Use Reason", width: Widths.AnsiChars(20))
            .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(5), iseditingreadonly: true, integer_places: 7, decimal_places: 2)
            .Numeric("ActualWeight", header: "Act.(kg)", width: Widths.AnsiChars(5), iseditingreadonly: true, integer_places: 7, decimal_places: 2)
            .Numeric("Differential", header: "Differential", width: Widths.AnsiChars(5), iseditingreadonly: true, integer_places: 7, decimal_places: 2)
            .CheckBox("moisture", header: "Moisture", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name")
            .Text("EditName", header: "Edit Name", width: Widths.AnsiChars(10))
            .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(12))
            .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true);

            this.grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Actualyds"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["CutWidth"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["fullwidth"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["actualwidth"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.grid.Columns["moisture"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.MistyRose;

            return true;
        }

        /// <inheritdoc/>
        protected override void OnInsert()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            int maxi = MyUtility.Convert.GetInt(dt.Compute("Max(NewKey)", string.Empty));
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = this.loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", this.loginID, "Pass1", "ID");
            selectDr["NewKey"] = maxi + 1;
            selectDr["Moisture"] = 0;
            selectDr["poid"] = this.maindr["poid"];
            selectDr["SEQ1"] = this.maindr["SEQ1"];
            selectDr["SEQ2"] = this.maindr["SEQ2"];
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)this.gridbs.DataSource;
            #region 判斷空白不可存檔
            DataRow[] drArray;
            drArray = gridTb.Select("Roll=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Roll> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("actualyds=0");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Actual Yds Inspected> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("actualyds>99999");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Actual Yds Inspected> can not more than 99999.");
                return false;
            }

            drArray = gridTb.Select("fullwidth=0");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Full Width> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("fullwidth>999");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Full Width> can not more than 999");
                return false;
            }

            drArray = gridTb.Select("actualwidth=0");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Actual Width> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("actualwidth>999");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Actual Width> can not more than 999");
                return false;
            }

            drArray = gridTb.Select("Inspdate is null");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Insection Date> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("inspector=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Inspector> can not be empty.");
                return false;
            }
            #endregion

            return base.OnSaveBefore();
        }

        private void Get_All_point(DataTable dt)
        {
            foreach (DataRow dataRow in dt.Rows)
            {
                this.Get_total_point(dataRow);
            }

            this.gridbs.DataSource = dt;
            this.OnSave();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override DualResult OnSave()
        {
            DualResult upResult = new DualResult(true);
            #region Fir_Physical //因為要抓取DetailUkey 所以要自己Overridr
            string update_cmd = string.Empty;
            List<string> append_cmd = new List<string>();
            DataTable idenDt;
            string iden;
            DataTable gridTb = (DataTable)this.gridbs.DataSource;

            foreach (DataRow dr in gridTb.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    string delete_cmd = string.Format(
                    @"
Delete From Fir_physical Where DetailUkey = {0} ;
Delete From FIR_Physical_Defect Where FIR_PhysicalDetailUKey = {0} ; 
Delete From FIR_Physical_Defect_Realtime Where FIR_PhysicalDetailUKey = {0} ;",
                    dr["DetailUKey", DataRowVersion.Original]);

                    #region 先刪除資料
                    upResult = DBProxy.Current.Select(null, delete_cmd, out idenDt);
                    if (!upResult)
                    {
                        return upResult;
                    }
                    #endregion
                    continue;
                }

                int bolMoisture = 0;
                string inspdate;
                if (MyUtility.Check.Empty(dr["InspDate"]))
                {
                    inspdate = string.Empty; // 判斷Inspect Date
                }
                else
                {
                    inspdate = Convert.ToDateTime(dr["InspDate"]).ToShortDateString();
                }

                if (MyUtility.Convert.GetBool(dr["Moisture"]))
                {
                    bolMoisture = 1;
                }

                if (dr.RowState == DataRowState.Added)
                {
                    string add_cmd = string.Empty;

                    var isGrandCCanUse = MyUtility.Convert.GetBool(dr["IsGrandCCanUse"]) ? 1 : 0;

                    // 防止相同ID+Roll+Dyelot 存入Fir_Physical
                    string sqlChk = $@"
select 1 
from Fir_Physical 
where id = '{dr["ID"]}'
and Roll = '{dr["roll"].ToString().Replace("'", "''")}'
and Dyelot = '{dr["Dyelot"]}'
";
                    if (MyUtility.Check.Seek(sqlChk))
                    {
                        MyUtility.Msg.WarningBox($@"Roll = {dr["Roll"]} and Dyelit = {dr["Dyelot"]} cannot be duplicate!");
                        return new DualResult(false, "Roll and Dyelot cannot be duplicate!");
                    }

                    add_cmd = string.Format(
                    @"Insert into Fir_Physical
(ID,Roll,Dyelot,TicketYds,ActualYds,FullWidth,ActualWidth,TotalPoint,PointRate,Grade,Result,Remark,InspDate,Inspector,Moisture,AddName,AddDate,IsGrandCCanUse,GrandCCanUseReason,ColorToneCheck) 
Values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},'{9}','{10}','{11}','{12}','{13}',{14},'{15}',GetDate(),{16},'{17}','{18}') ;",
                    dr["ID"], dr["roll"].ToString().Replace("'", "''"), dr["Dyelot"], dr["TicketYds"], dr["ActualYds"], dr["FullWidth"], dr["ActualWidth"], dr["TotalPoint"], dr["PointRate"], dr["Grade"], dr["Result"], dr["Remark"].ToString().Replace("'", "''"), inspdate, dr["Inspector"], bolMoisture, this.loginID, isGrandCCanUse, dr["GrandCCanUseReason"], dr["ColorToneCheck"]);
                    add_cmd = add_cmd + "select @@IDENTITY as ii";
                    #region 先存入Table 撈取Idenitiy
                    upResult = DBProxy.Current.Select(null, add_cmd, out idenDt);
                    if (upResult)
                    {
                        iden = idenDt.Rows[0]["ii"].ToString(); // 取出Identity

                        DataRow[] ary = this.Fir_physical_Defect.Select(string.Format("NewKey={0}", dr["NewKey"]));
                        if (ary.Length > 0)
                        {
                            foreach (DataRow ary_dr in ary)
                            {
                                ary_dr["FIR_PhysicalDetailUKey"] = iden;
                            }
                        }
                    }
                    else
                    {
                        return upResult;
                    }
                    #endregion
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    var isGrandCCanUse_val = MyUtility.Convert.GetBool(dr["IsGrandCCanUse"]) == false ? 0 : 1;

                    update_cmd = update_cmd + $@"
Update Fir_Physical 
set Roll = '{dr["roll"].ToString().Replace("'", "''")}' ,Dyelot='{dr["Dyelot"]}',TicketYds = {dr["TicketYds"]}
    ,ActualYds = {dr["ActualYds"]},FullWidth ={dr["FullWidth"]}
    ,ActualWidth={dr["ActualWidth"]} ,TotalPoint ={dr["TotalPoint"]} ,PointRate={dr["PointRate"]}
    ,Grade='{dr["Grade"]}',Result='{dr["Result"]}' ,Remark='{dr["Remark"].ToString().Replace("'", "''")}' ,InspDate='{inspdate}' 
    ,Inspector='{dr["Inspector"]}'
    ,Moisture={bolMoisture} ,EditName = '{this.loginID}' ,EditDate = GetDate() ,IsGrandCCanUse = {isGrandCCanUse_val} 
    ,GrandCCanUseReason = '{dr["GrandCCanUseReason"]}' ,ColorToneCheck = '{dr["ColorToneCheck"]}'
Where DetailUkey = {dr["DetailUkey"]};
";
                }
            }

            if (update_cmd != string.Empty)
            {
                upResult = DBProxy.Current.Execute(null, update_cmd);
                if (!upResult)
                {
                    return upResult;
                }
            }
            #endregion

            #region Fir_Physical_Defect
            string update_cmd1 = string.Empty;
            foreach (DataRow dr in this.Fir_physical_Defect.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    update_cmd1 = update_cmd1 + string.Format(
                    @"Delete From Fir_physical_Defect Where ID = {0} and FIR_PhysicalDetailUKey = {1} and DefectLocation ='{2}';",
                    dr["ID", DataRowVersion.Original], dr["FIR_PhysicalDetailUKey", DataRowVersion.Original], dr["DefectLocation", DataRowVersion.Original]);
                }

                if (dr.RowState == DataRowState.Added)
                {
                    update_cmd1 = update_cmd1 + string.Format(
                        @"Insert into Fir_Physical_Defect(ID,FIR_PhysicalDetailUKey,DefectLocation,DefectRecord,Point) 
                            Values({0},{1},'{2}','{3}',{4});",
                        dr["ID"], dr["FIR_PhysicalDetailUKey"], dr["DefectLocation"], dr["DefectRecord"], dr["Point"]);
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    update_cmd1 = update_cmd1 + string.Format(
                        @"Update Fir_Physical_Defect set DefectRecord = '{3}',Point = {4},DefectLocation = '{2}'
                            Where ID = {0} and FIR_PhysicalDetailUKey = {1} and DefectLocation = '{5}';",
                        dr["ID"], dr["FIR_PhysicalDetailUKey"], dr["DefectLocation", DataRowVersion.Current], dr["DefectRecord"], dr["Point"], dr["DefectLocation", DataRowVersion.Original]);
                }
            }

            update_cmd1 += " update FIR set CustInspNumber = @CustInspNumber where ID = @FIR_ID";
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@CustInspNumber", this.txtCustInspNumber.Text),
                new SqlParameter("@FIR_ID", this.maindr["ID"]),
            };

            if (update_cmd1 != string.Empty)
            {
                upResult = DBProxy.Current.Execute(null, update_cmd1, listPar);
            }

            if (upResult)
            {
                this.maindr["CustInspNumber"] = this.txtCustInspNumber.Text;
            }

            #endregion
            return upResult;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            if (MyUtility.Check.Empty(this.CurrentData) && this.btnEncode.Text == "Encode")
            {
                MyUtility.Msg.WarningBox("Data not found! ");
                return;
            }

            string sqlCmd = $"select InspectionGroup from Fabric where SCIRefno = '{this.maindr["SCIRefno"].ToString()}'";
            this.txtGroup.Text = MyUtility.GetValue.Lookup(sqlCmd, "Production");

            if (MyUtility.Check.Empty(this.txtGroup.Text) && this.btnEncode.Text == "Encode" && this.needCheckInspectionGroup)
            {
                MyUtility.Msg.WarningBox("<Group> can not be empty, please ask the maintenance of relevant personnel.");
                return;
            }

            if (this.btnEncode.Text == "Encode")
            {
                var dataTable = (DataTable)this.gridbs.DataSource;
                if (dataTable.Rows.Count > 0 && this.needCheckInspectionGroup)
                {
                    this.Get_All_point((DataTable)this.gridbs.DataSource);
                }
            }

            if (!MyUtility.Convert.GetBool(this.maindr["PhysicalEncode"]))
            {
                // Encode
                if (!MyUtility.Convert.GetBool(this.maindr["nonPhysical"]))
                {
                    // 只要沒勾選就要判斷，有勾選就可直接Encode
                    // 至少收料的每ㄧ缸都要有檢驗紀錄 ,找尋有收料的缸沒在檢驗出現
                    DataTable dyeDt;
                    string cmd = string.Format(
                        @"
Select distinct dyelot from Receiving_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_Physical b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
union
Select distinct dyelot from TransferIn_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_Physical b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
",
                        this.maindr["receivingid"], this.maindr["id"], this.maindr["POID"], this.maindr["seq1"], this.maindr["seq2"]);
                    DualResult dResult = DBProxy.Current.Select(null, cmd, out dyeDt);
                    if (dResult)
                    {
                        if (dyeDt != null && dyeDt.Rows.Count > 0)
                        {
                            string dye = string.Empty;
                            foreach (DataRow dr in dyeDt.Rows)
                            {
                                dye = dye + dr["Dyelot"].ToString() + ",";
                            }

                            MyUtility.Msg.WarningBox("<Dyelot>:" + dye + " Each Dyelot must be inspected!");
                            return;
                        }
                    }
                }

                string result = string.Empty;
                string lLLResult = string.Empty;
                DataTable gridTb = (DataTable)this.gridbs.DataSource;
                DataRow[] resultAry = gridTb.Select("Result = 'Fail'");
                result = "Pass";
                if (resultAry.Length > 0)
                {
                    result = "Fail";
                }

                if (this.displayBrand.Text == "LLL")
                {
                    if (this.txtGroup.Text == "5")
                    {
                        result = "LLL G5";
                    }
                }

                #region  寫入虛擬欄位
                this.maindr["Physical"] = result;

                // maindr["PhysicalDate"] = DateTime.Now.ToShortDateString();
                this.maindr["PhysicalDate"] = gridTb.Compute("Max(InspDate)", string.Empty);
                this.maindr["PhysicalEncode"] = true;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["PhysicalInspector"] = this.loginID;
                int sumPoint = MyUtility.Convert.GetInt(gridTb.Compute("Sum(totalpoint)", string.Empty));
                decimal sumTotalYds = MyUtility.Convert.GetDecimal(gridTb.Compute("Sum(actualyds)", string.Empty));
                this.maindr["TotalDefectPoint"] = sumPoint;
                this.maindr["TotalInspYds"] = sumTotalYds;
                #endregion
                #region 判斷Result 是否要寫入
                string[] returnstr = PublicPrg.Prgs.GetOverallResult_Status(this.maindr);
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = '{7}',PhysicalEncode=1,EditName='{0}',EditDate = GetDate(),Physical = '{1}',Result ='{2}',TotalDefectPoint = {4},TotalInspYds = {5},Status='{6}' ,PhysicalInspector = '{0}'
                  where id ={3}", this.loginID, result, returnstr[0], this.maindr["ID"], sumPoint, sumTotalYds, returnstr[1], Convert.ToDateTime(gridTb.Compute("Max(InspDate)", string.Empty)).ToString("yyyy/MM/dd"));
                #endregion
                this.maindr["Result"] = returnstr[0];
                this.maindr["Status"] = returnstr[1];
            }
            else
            {
                // Amend
                #region  寫入虛擬欄位
                this.maindr["Physical"] = string.Empty;
                this.maindr["PhysicalDate"] = DBNull.Value;
                this.maindr["PhysicalEncode"] = false;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["TotalDefectPoint"] = 0;
                this.maindr["TotalInspYds"] = 0;
                this.maindr["PhysicalInspector"] = string.Empty;

                // 判斷Result and Status 必須先確認Physical="",判斷才會正確
                string[] returnstr = PublicPrg.Prgs.GetOverallResult_Status(this.maindr);
                this.maindr["Result"] = returnstr[0];
                this.maindr["Status"] = returnstr[1];
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = null,PhysicalEncode=0,EditName='{0}',EditDate = GetDate(),Physical = '',Result ='{2}',TotalDefectPoint = 0,TotalInspYds = 0,Status='{3}' ,PhysicalInspector = ''  where id ={1}", this.loginID, this.maindr["ID"], returnstr[0], returnstr[1]);
                #endregion
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    // 更新PO.FIRInspPercent和AIRInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{this.maindr["POID"].ToString()}'; ")))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            this.OnRequery();
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;

            if (this.maindr["Status"].ToString() == "Confirmed")
            {
                this.maindr["Status"] = "Approved";
                this.maindr["Approve"] = this.loginID;
                this.maindr["ApproveDate"] = DateTime.Now.ToShortDateString();
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Approved',Approve='{0}',EditName='{0}',EditDate = GetDate(),ApproveDate = GetDate() where id ={1}", this.loginID, this.maindr["ID"]);
                #endregion
            }
            else
            {
                this.maindr["Status"] = "Confirmed";
                this.maindr["Approve"] = string.Empty;
                this.maindr["ApproveDate"] = DBNull.Value;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Confirmed',Approve='',EditName='{0}',EditDate = GetDate(),ApproveDate = null where id ={1}", this.loginID, this.maindr["ID"]);
                #endregion
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            this.OnRequery();
        }

        private void Button_enable()
        {
            if (this.maindr == null)
            {
                return;
            }

            this.btnEncode.Enabled = this.CanEdit && !this.EditMode && this.maindr["Status"].ToString() != "Approved";
            this.btnToExcel.Enabled = !this.EditMode;
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P01", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", this.loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 WITH (NOLOCK) Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            int lApprove = 0; // 有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }

            if (this.maindr["Result"].ToString() == "Pass")
            {
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["physical"]);
            }
            else
            {
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["physical"]);
            }
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.ToExcel(false, "Regular");
        }

        private void BtnToExcel_defect_Click(object sender, EventArgs e)
        {
            this.ToExcel(false, "DefectYds");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private bool ToExcel(bool isSendMail, string type)
        {
            #region DataTables && 共用變數
            // FabricDefect 基本資料 DB
            DataTable dtBasic;
            DualResult result;
            if (result = DBProxy.Current.Select("Production", "SELECT id,type,DescriptionEN FROM FabricDefect WITH (NOLOCK) where id <>'' order by ID", out dtBasic))
            {
                if (dtBasic.Rows.Count < 1)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }

            // FIR_Physical_Defect DB
            DataTable dt;
            if (result = DBProxy.Current.Select("Production", string.Format("select * from FIR_Physical A WITH (NOLOCK) left JOIN FIR_Physical_Defect B WITH (NOLOCK) ON A.DetailUkey = B.FIR_PhysicalDetailUKey WHERE A.ID='{0}'", this.FirID), out dt))
            {
                if (dt.Rows.Count < 1)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }

            DataTable dt1;
            if (result = DBProxy.Current.Select("Production", string.Format(
               @"
select Roll,Dyelot,A.Result,A.Inspdate,Inspector,B.ContinuityEncode,C.SeasonID,B.TotalInspYds,B.ArriveQty,B.PhysicalEncode  
from FIR_Physical a WITH (NOLOCK) 
left join FIR b WITH (NOLOCK) on a.ID=b.ID 
LEFT JOIN View_WH_Orders C ON B.POID=C.ID 
where a.ID='{0}'", this.FirID), out dt1))
            {
                if (dt1.Rows.Count <= 0)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }

            DataTable dtSupvisor;
            if (result = DBProxy.Current.Select("Production", string.Format("select * from Pass1 WITH (NOLOCK) where id='{0}'", this.loginID), out dtSupvisor))
            {
                if (dtSupvisor.Rows.Count <= 0)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }

            DataTable dtSumQty;
            if (result = DBProxy.Current.Select("Production", string.Format("select sum(ticketYds) as TotalTicketYds from FIR_Physical WITH (NOLOCK) where id='{0}'", this.FirID), out dtSumQty))
            {
                if (dtSumQty.Rows.Count <= 0)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }

            #endregion
            int addline = 0;
            string strXltName = Env.Cfg.XltPathDir + "\\Quality_P01_Physical_Inspection_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region FabricDefect 基本資料
            int counts = dtBasic.Rows.Count;
            int int_X = 6;
            int int_Y = 2;
            int int_z = 0; // 判斷是否為第一次

            string typeColumn = "typeColumn";
            this.ShowWaitMessage("Starting EXCEL...");

            for (int i = 0; i < counts; i++)
            {
                if (dtBasic.Rows[i]["type"].ToString() != typeColumn && dtBasic.Rows[i]["type"].ToString() != null)
                {
                    int_X = 6;
                    if (int_Y == 2 && int_z == 0)
                    {
                        // first time
                        worksheet.Cells[6, int_Y - 1] = "Code";
                        worksheet.Cells[6, int_Y - 1].Interior.Color = Color.FromArgb(153, 204, 255);
                        typeColumn = dtBasic.Rows[i]["type"].ToString();
                        worksheet.Cells[6, int_Y + 1] = typeColumn.ToString();
                        int_X++;
                        int_z = 1;
                    }
                    else
                    {
                        int_X++;
                        int_Y += 2;
                        worksheet.Cells[6, int_Y] = "Code".ToString();
                        worksheet.Cells[6, int_Y].Interior.Color = Color.FromArgb(153, 204, 255);
                        typeColumn = dtBasic.Rows[i]["type"].ToString();
                        worksheet.Cells[6, int_Y + 1] = typeColumn.ToString();
                    }

                    worksheet.Cells[6, int_Y + 1].Interior.Color = Color.FromArgb(153, 204, 255);
                }

                if (int_Y == 2)
                {
                    worksheet.Cells[int_X, int_Y - 1] = dtBasic.Rows[i]["id"].ToString();
                    worksheet.Cells[int_X, int_Y - 1].Interior.Color = Color.FromArgb(153, 204, 255);
                }
                else
                {
                    worksheet.Cells[int_X, int_Y] = dtBasic.Rows[i]["id"].ToString();
                    worksheet.Cells[int_X, int_Y].Interior.Color = Color.FromArgb(153, 204, 255);
                }

                worksheet.Cells[int_X, int_Y + 1] = dtBasic.Rows[i]["DescriptionEN"].ToString();
                int_X++;
            }

            worksheet.Range[worksheet.Cells[6, 1], worksheet.Cells[17, int_Y + 1]].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            #endregion
            #region FIR_Physical
            #region 表頭

            excel.Cells[1, 3] = this.displaySP.Text + "-" + this.displaySEQ.Text;
            excel.Cells[1, 6] = this.displayColor.Text;
            excel.Cells[1, 8] = this.displayStyle.Text;
            excel.Cells[1, 10] = dt1.Rows[0]["SeasonID"];
            excel.Cells[1, 12] = dt1.Rows[0]["Inspector"];
            excel.Cells[2, 3] = this.displaySCIRefno.Text;
            excel.Cells[2, 6] = this.displayResult.Text;
            excel.Cells[2, 8] = this.dateLastInspectionDate.Value;
            excel.Cells[2, 10] = this.displayBrand.Text;
            excel.Cells[2, 12] = dt1.Rows[0]["TotalInspYds"];
            excel.Cells[3, 3] = this.displayBrandRefno.Text;
            excel.Cells[3, 6] = this.txtsupplier.DisplayBox1.Text.ToString();
            excel.Cells[3, 8] = this.displayWKNo.Text;
            excel.Cells[3, 10] = dtSupvisor.Rows[0]["Supervisor"];
            excel.Cells[4, 3] = this.dateArriveWHDate.Value;

            excel.Cells[4, 6] = this.displayArriveQty.Text;
            excel.Cells[4, 8] = dtSumQty.Rows[0]["TotalTicketYds"]; // Inspected Qty
            excel.Cells[4, 10] = dt1.Rows[0]["PhysicalEncode"].ToString().ToUpper() == "TRUE" ? "Y" : "N";

            #endregion

            DataTable dtGrid;
            int gridCounts = 0;
            if (this.gridbs.DataSource == null || this.grid == null)
            {
                string sqlcmd = $@"
select rd.FullRoll, rd.FullDyelot,fp.* 
from Production.dbo.FIR_Physical fp
left join Receiving_Detail rd on rd.PoId = '{this.maindr["POID"]}' 
and rd.Seq1 = '{this.maindr["Seq1"]}' and rd.Seq2= '{this.maindr["Seq2"]}'
and fp.Roll = rd.Roll and fp.Dyelot = rd.Dyelot
where fp.id = {this.FirID}";
                if (result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtGrid))
                {
                    gridCounts = dtGrid.Rows.Count;
                }

                dtGrid.ColumnsDecimalAdd("CutWidth");
                dtGrid.Columns.Add("Name", typeof(string));
                dtGrid.Columns.Add("POID", typeof(string));
                dtGrid.Columns.Add("SEQ1", typeof(string));
                dtGrid.Columns.Add("SEQ2", typeof(string));
                foreach (DataRow dr in dtGrid.Rows)
                {
                    dr["CutWidth"] = MyUtility.GetValue.Lookup(
                        string.Format(
                        @"
                                                                        select width
                                                                        from Fabric 
                                                                        inner join Fir on Fabric.SCIRefno = fir.SCIRefno
                                                                        where Fir.ID = '{0}'", dr["id"]), null, null);
                    dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                    dr["poid"] = this.maindr["poid"];
                    dr["SEQ1"] = this.maindr["SEQ1"];
                    dr["SEQ2"] = this.maindr["SEQ2"];
                }
            }
            else
            {
                dtGrid = (DataTable)this.gridbs.DataSource;

                if (dtGrid.Columns.Contains("FullRoll") == false)
                {
                    dtGrid.Columns.Add("FullRoll", typeof(string));
                }

                if (dtGrid.Columns.Contains("FullDyelot") == false)
                {
                    dtGrid.Columns.Add("FullDyelot", typeof(string));
                }

                string sqlFull = $@"
select distinct Roll,Dyelot,FullRoll,FullDyelot 
from Receiving_Detail 
where PoId = '{this.maindr["POID"]}' 
and Seq1 = '{this.maindr["Seq1"]}' and Seq2= '{this.maindr["Seq2"]}'
and id = '{this.maindr["Receivingid"]}'
";
                if (result = DBProxy.Current.Select(string.Empty, sqlFull, out DataTable dtFull))
                {
                    if (dtFull != null || dtFull.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtGrid.Rows)
                        {
                            DataRow[] drFilter = dtFull.Select($@"roll = '{dr["roll"]}' and Dyelot = '{dr["Dyelot"]}'");
                            if (drFilter.Length > 0)
                            {
                                DataTable dtFilter = drFilter.CopyToDataTable();
                                dr["FullRoll"] = dtFilter.Rows[0]["FullRoll"];
                                dr["FullDyelot"] = dtFilter.Rows[0]["FullDyelot"];
                            }
                        }
                    }
                }

                gridCounts = this.grid.RowCount;
            }

            int rowcount = 0;

            for (int i = 0; i < gridCounts; i++)
            {
                excel.Cells[19 + (i * 10) + addline, 1] = dtGrid.Rows[rowcount]["Roll"].ToString();
                excel.Cells[19 + (i * 10) + addline, 2] = dtGrid.Rows[rowcount]["FullRoll"].ToString();
                excel.Cells[19 + (i * 10) + addline, 3] = dtGrid.Rows[rowcount]["Dyelot"].ToString();
                excel.Cells[19 + (i * 10) + addline, 4] = dtGrid.Rows[rowcount]["FullDyelot"].ToString();
                excel.Cells[19 + (i * 10) + addline, 5] = dtGrid.Rows[rowcount]["Ticketyds"].ToString();

                // 指定欄位轉型
                Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[19 + (i * 10) + addline, 5];
                worksheet.get_Range(cell, cell).NumberFormat = "0.00";

                excel.Cells[19 + (i * 10) + addline, 6] = dtGrid.Rows[rowcount]["Actualyds"].ToString();
                excel.Cells[19 + (i * 10) + addline, 7] = dtGrid.Rows[rowcount]["Cutwidth"].ToString();
                excel.Cells[19 + (i * 10) + addline, 8] = dtGrid.Rows[rowcount]["fullwidth"].ToString();
                excel.Cells[19 + (i * 10) + addline, 9] = dtGrid.Rows[rowcount]["actualwidth"].ToString();
                excel.Cells[19 + (i * 10) + addline, 10] = dtGrid.Rows[rowcount]["totalpoint"].ToString();
                excel.Cells[19 + (i * 10) + addline, 11] = dtGrid.Rows[rowcount]["pointRate"].ToString();
                excel.Cells[19 + (i * 10) + addline, 12] = dtGrid.Rows[rowcount]["Grade"].ToString();
                excel.Cells[19 + (i * 10) + addline, 13] = dtGrid.Rows[rowcount]["Result"].ToString();
                excel.Cells[19 + (i * 10) + addline, 14] = dtGrid.Rows[rowcount]["Remark"].ToString();
                rowcount++;

                #region FIR_Physical_Defect

                // 變色 titile
                worksheet.Range[excel.Cells[20 + (i * 10) + addline, 1], excel.Cells[20 + (i * 10) + addline, 10]].Interior.colorindex = 38;
                worksheet.Range[excel.Cells[20 + (i * 10) + addline, 1], excel.Cells[20 + (i * 10) + addline, 10]].Borders.LineStyle = 1;
                worksheet.Range[excel.Cells[20 + (i * 10) + addline, 1], excel.Cells[20 + (i * 10) + addline, 10]].Font.Bold = true;

                DataTable dtRealTime;

                string cmd = $@"
SELECT Yards, FabricdefectID, T2 = iif(T2 = 1,'-T2',''), [cnt] = count(1)
FROM [Production].[dbo].[FIR_Physical_Defect_Realtime] with (nolock)
where FIR_PhysicalDetailUkey={dtGrid.Rows[rowcount - 1]["detailUkey"]}
group by Yards,FabricdefectID, T2
order by Yards
";

                if (!(result = DBProxy.Current.Select("Production", cmd, out dtRealTime)))
                {
                    this.ShowErr(result);
                    return false;
                }

                // 依照Type來匯出Excel
                if (type == "DefectYds" && dtRealTime.Rows.Count > 0)
                {
                    int cntRealTime = 0;
                    int cntDtRealTime = dtRealTime.Rows.Count;
                    int cntnextline = 0;
                    int cntX = 3;
                    for (int ii = 1; ii <= cntDtRealTime * 2; ii++)
                    {
                        if (ii % 2 == 1)
                        {
                            if (ii > 10 * (cntnextline + 1))
                            {
                                cntnextline++;
                                addline++;
                            }

                            if (cntnextline == 0)
                            {
                                excel.Cells[20 + (i * 10) + addline, ii] = "Yards";
                            }

                            excel.Cells[21 + (i * 10) + addline, ii - (cntnextline * 10)] = dtRealTime.Rows[cntRealTime]["Yards"];
                            cntRealTime++;
                        }
                        else
                        {
                            if (cntnextline == 0)
                            {
                                excel.Cells[20 + (i * 10) + addline, ii] = "Defect";
                            }

                            excel.Cells[21 + (i * 10) + addline, ii - (cntnextline * 10)] = dtRealTime.Rows[cntRealTime - 1]["FabricdefectID"].ToString() + dtRealTime.Rows[cntRealTime - 1]["cnt"].ToString() + dtRealTime.Rows[cntRealTime - 1]["T2"].ToString();

                            Microsoft.Office.Interop.Excel.Range formatRange = worksheet.get_Range($"{MyExcelPrg.GetExcelColumnName(cntX - (cntnextline * 10))}{21 + (i * 10) + addline}");
                            formatRange.NumberFormat = "0.00";
                            formatRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                            cntX += 2;
                        }
                    }
                }
                else
                {
                    DataTable dtDefect;
                    DBProxy.Current.Select("Production", string.Format("select * from  FIR_Physical_Defect WITH (NOLOCK) WHERE FIR_PhysicalDetailUKey='{0}'", dtGrid.Rows[rowcount - 1]["detailUkey"]), out dtDefect);
                    int pDrowcount = 0;
                    int dtRowCount = (int)dtDefect.Rows.Count;
                    int nextLineCount = 1;
                    for (int c = 1; c < dtRowCount; c++)
                    {
                        if (c == 6 * nextLineCount)
                        {
                            nextLineCount++;
                        }
                    }

                    for (int ii = 1; ii < 11; ii++)
                    {
                        if (ii % 2 == 1)
                        {
                            excel.Cells[20 + (i * 10) + addline, ii] = "Yards";
                        }
                        else
                        {
                            excel.Cells[20 + (i * 10) + addline, ii] = "Defect";
                        }
                    }

                    int nextline = 0;
                    for (int ii = 1; ii <= dtRowCount * 2; ii++)
                    {
                        if (ii % 2 == 1)
                        {
                            if (ii > 10 * (nextline + 1))
                            {
                                nextline++;
                                addline++;
                            }

                            excel.Cells[21 + (i * 10) + addline, ii - (nextline * 10)] = dtDefect.Rows[pDrowcount]["DefectLocation"];
                            pDrowcount++;
                        }
                        else
                        {
                            string sqlcmd = $@"
select 1 
from FIR_Physical_Defect_Realtime t
where FIR_PhysicalDetailUkey = {dtGrid.Rows[rowcount - 1]["detailUkey"]}
and CONVERT(int, t.Yards) between (select Data from SplitString('{dtDefect.Rows[pDrowcount - 1]["DefectLocation"]}','-') where no = '1')　
and (select Data from SplitString('{dtDefect.Rows[pDrowcount - 1]["DefectLocation"]}','-') where no = '2')　
and t.T2 = 1";
                            DataTable dtSelect = dtDefect.Clone();
                            dtSelect.ImportRow(dtDefect.Rows[pDrowcount - 1]);
                            string strT2 = MyUtility.Check.Seek(sqlcmd) ? "-T2" : string.Empty;
                            if (dtDefect.Rows[pDrowcount - 1]["DefectRecord"].ToString().IndexOf('/') != -1)
                            {
                                excel.Cells[21 + (i * 10) + addline, ii - (nextline * 10)] = P01_PhysicalInspection_Defect.GetNewDefectRecord_T2(dtSelect);
                            }
                            else
                            {
                                excel.Cells[21 + (i * 10) + addline, ii - (nextline * 10)] = dtDefect.Rows[pDrowcount - 1]["DefectRecord"] + strT2;
                            }
                        }
                    }
                }

                string sql = $@"
select FabricdefectID, FabricdefectCount = count(fdr.FabricdefectID) 
from FIR f
inner join FIR_Physical fp on fp.ID = f.ID
inner join FIR_Physical_Defect_Realtime fdr on fdr.FIR_PhysicalDetailUkey = fp.DetailUkey
where PoId = '{this.maindr["POID"]}' 
and Seq1 = '{this.maindr["Seq1"]}' and Seq2= '{this.maindr["Seq2"]}'
and fp.Roll ='{dtGrid.Rows[i]["Roll"].ToString()}' 
and fp.Dyelot ='{dtGrid.Rows[i]["Dyelot"].ToString()}'
group by FabricdefectID
";
                DataTable tmpFabricdefect;
                DBProxy.Current.Select(null, sql, out tmpFabricdefect);

                if (tmpFabricdefect.Rows.Count > 0)
                {
                    for (int tc = 0; tc < tmpFabricdefect.Rows.Count; tc++) // 修正: tc 從 0 開始
                    {
                        int col = (tc * 2) + 1; // 修正: 確保欄位對應正確 (Excel欄位從1開始)

                        excel.Cells[22 + (i * 10) + addline, col] = "Defect";
                        excel.Cells[23 + (i * 10) + addline, col] = tmpFabricdefect.Rows[tc]["FabricdefectID"].ToString();

                        excel.Cells[22 + (i * 10) + addline, col + 1] = "Total";
                        excel.Cells[23 + (i * 10) + addline, col + 1] = tmpFabricdefect.Rows[tc]["FabricdefectCount"].ToString();
                    }

                    int ttlc = tmpFabricdefect.Rows.Count * 2;

                    worksheet.Range[excel.Cells[22 + (i * 10) + addline, 1], excel.Cells[22 + (i * 10) + addline, ttlc]].Interior.colorindex = 38;
                    worksheet.Range[excel.Cells[22 + (i * 10) + addline, 1], excel.Cells[22 + (i * 10) + addline, ttlc]].Borders.LineStyle = 1;
                    worksheet.Range[excel.Cells[22 + (i * 10) + addline, 1], excel.Cells[22 + (i * 10) + addline, ttlc]].Font.Bold = true;
                }

                if (tmpFabricdefect.Rows.Count == 0)
                {
                    excel.Cells[22 + (i * 10) + addline, 1] = "Defect";
                    excel.Cells[22 + (i * 10) + addline, 2] = "Total";
                    worksheet.Range[excel.Cells[22 + (i * 10) + addline, 1], excel.Cells[22 + (i * 10) + addline, 2]].Interior.colorindex = 38;
                    worksheet.Range[excel.Cells[22 + (i * 10) + addline, 1], excel.Cells[22 + (i * 10) + addline, 2]].Borders.LineStyle = 1;
                    worksheet.Range[excel.Cells[22 + (i * 10) + addline, 1], excel.Cells[22 + (i * 10) + addline, 2]].Font.Bold = true;
                }

                worksheet.Range[excel.Cells[24 + (i * 10) + addline, 1], excel.Cells[24 + (i * 10) + addline, 10]].Font.Bold = true;
                #endregion
                DataTable dtcombo;

                if (result = DBProxy.Current.Select("Production", string.Format(
                    @"
select  a.ID
        ,a.Roll
        ,type_c = 'Continuity' 
        ,Result_c = d.Result 
        ,Remark_c = d.Remark 
        ,Inspector_c = d.Inspector 
        ,Name_c = (select Concat(Pass1.Name, ' Ext.', Pass1.ExtNo) 
                   from Pass1
                   where Pass1.ID = d.Inspector) 
        ,type_s = 'Shadebond' 
        ,Result_s = b.Result 
        ,Remark_s = b.Remark 
        ,Inspector_s = b.Inspector 
        ,Name_s = (select Concat(Pass1.Name, ' Ext.', Pass1.ExtNo) 
                   from Pass1
                   where Pass1.ID = b.Inspector) 
        ,type_w = 'Weight' 
        ,Result_w = c.Result 
        ,Remark_w = c.Remark 
        ,Inspector_w = c.Inspector 
        ,Name_w = (select Concat(Pass1.Name, ' Ext.', Pass1.ExtNo) 
                   from Pass1
                   where Pass1.ID = c.Inspector) 
        ,type_m = 'Moisture' 
        ,Result_m = IIF(a.Moisture=0,'Fail','Pass')
        ,Remark_m = ''
        ,Inspector_m = a.Inspector 
        ,Name_m = (select Concat(Pass1.Name, ' Ext.', Pass1.ExtNo) 
                   from Pass1
                   where Pass1.ID = a.Inspector) 
        ,Comment = '' 
        ,Moisture = a.Moisture
from FIR_Physical a WITH (NOLOCK) 
left join FIR_Shadebone b WITH (NOLOCK) on a.ID=b.ID and a.Roll=b.Roll
left join FIR_Weight c WITH (NOLOCK) on a.ID=c.ID and a.Roll=c.Roll
left join FIR_Continuity d WITH (NOLOCK) on a.id=d.ID and a.Roll=d.roll
where a.ID='{0}' and a.Roll='{1}' ORDER BY A.Roll", this.FirID, dtGrid.Rows[rowcount - 1]["Roll"].ToString()), out dtcombo))
                {
                    if (dtcombo.Rows.Count < 1)
                    {
                        excel.Cells[24 + (i * 10) + addline, 2] = "Result";
                        excel.Cells[24 + (i * 10) + addline, 3] = "Comment";
                        excel.Cells[24 + (i * 10) + addline, 4] = "Inspector";
                        excel.Cells[25 + (i * 10) + addline, 1] = "Contiunity ";
                        excel.Cells[26 + (i * 10) + addline, 1] = "Shad band";
                        excel.Cells[27 + (i * 10) + addline, 1] = "Weight";
                        excel.Cells[28 + (i * 10) + addline, 1] = "Moisture";
                    }
                    else
                    {
                        excel.Cells[24 + (i * 10) + addline, 2] = "Result";
                        excel.Cells[24 + (i * 10) + addline, 3] = "Comment";
                        excel.Cells[24 + (i * 10) + addline, 4] = "Inspector";
                        excel.Cells[25 + (i * 10) + addline, 1] = "Contiunity ";
                        excel.Cells[26 + (i * 10) + addline, 1] = "Shad band";
                        excel.Cells[27 + (i * 10) + addline, 1] = "Weight";
                        excel.Cells[28 + (i * 10) + addline, 1] = "Moisture";

                        excel.Cells[25 + (i * 10) + addline, 2] = dtcombo.Rows[0]["Result_c"].ToString();
                        excel.Cells[25 + (i * 10) + addline, 3] = "'" + dtcombo.Rows[0]["Remark_c"].ToString();
                        excel.Cells[25 + (i * 10) + addline, 4] = dtcombo.Rows[0]["Name_c"].ToString();

                        excel.Cells[26 + (i * 10) + addline, 2] = dtcombo.Rows[0]["Result_s"].ToString();

                        // 開頭加單引號防止特殊字元使excel產生失敗
                        excel.Cells[26 + (i * 10) + addline, 3] = "'" + dtcombo.Rows[0]["Remark_s"].ToString();
                        excel.Cells[26 + (i * 10) + addline, 4] = dtcombo.Rows[0]["Name_s"].ToString();

                        excel.Cells[27 + (i * 10) + addline, 2] = dtcombo.Rows[0]["Result_w"].ToString();
                        excel.Cells[27 + (i * 10) + addline, 3] = dtcombo.Rows[0]["Remark_w"].ToString();
                        excel.Cells[27 + (i * 10) + addline, 4] = dtcombo.Rows[0]["Name_w"].ToString();

                        if ((bool)dtcombo.Rows[0]["Moisture"])
                        {
                            excel.Cells[28 + (i * 10) + addline, 2] = dtcombo.Rows[0]["Result_m"].ToString();
                        }

                        excel.Cells[28 + (i * 10) + addline, 3] = dtcombo.Rows[0]["Remark_m"].ToString();
                        excel.Cells[28 + (i * 10) + addline, 4] = dtcombo.Rows[0]["Name_m"].ToString();
                    }

                    worksheet.Range[excel.Cells[24 + (i * 10) + addline, 1], excel.Cells[24 + (i * 10) + addline, 4]].Interior.colorindex = 38;
                    worksheet.Range[excel.Cells[24 + (i * 10) + addline, 1], excel.Cells[24 + (i * 10) + addline, 4]].Borders.LineStyle = 1;

                    worksheet.Range[excel.Cells[24 + (i * 10) + addline, 1], excel.Cells[24 + (i * 10) + addline, 4]].Font.Bold = true;
                    worksheet.Range[excel.Cells[25 + (i * 10) + addline, 1], excel.Cells[28 + (i * 10) + addline, 1]].Font.Bold = true;
                }
            }

            #endregion
            worksheet.Range[excel.Cells[18, 1], excel.Cells[18, 12]].Borders.LineStyle = 1;
            worksheet.Range[excel.Cells[18, 1], excel.Cells[18, 12]].Borders.Weight = 3;
            excel.Cells.EntireColumn.AutoFit();    // 自動欄寬
            excel.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save Excel
            this.excelFile = Class.MicrosoftFile.GetName("QA_P01_PhysicalInspection");
            excel.ActiveWorkbook.SaveAs(this.excelFile);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            #endregion

            if (!isSendMail)
            {
                this.excelFile.OpenFile();
            }

            this.HideWaitMessage();
            return true;
        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetBool(this.maindr["PhysicalEncode"]) && this.maindr["Status"].ToString() != "Approved")
            {
                // Excel Email 需寄給Encoder的Teamleader 與 Supervisor*****
                string cmd_leader = $@"
select ToAddress = stuff ((select concat (';', tmp.email)
from (
    select distinct email 
    from (
        select email = iif( isnull(p.email,'') = '', isnull(m.ToAddress,''),isnull(p.email,''))
		from View_WH_Orders o
		left join Pass1 p on o.MCHandle = p.ID
		left join MailGroup m on m.Code = '007' and o.FactoryID = m.FactoryID
        where o.ID='{this.displaySP.Text}'
    ) a
) tmp
for xml path('')
), 1, 1, '')";

                if (MyUtility.Check.Seek(cmd_leader, out DataRow dr))
                {
                    string mailto = dr["ToAddress"].ToString();

                    string ccMailGroup = MyUtility.GetValue.Lookup($@"
select m.CCAddress 
from View_WH_Orders o 
inner join MailGroup m on m.Code = '007' and o.FactoryID = m.FactoryID
where o.ID = '{this.displaySP.Text}'");

                    string ccAddress = ccMailGroup;
                    string subject = string.Format(MyUtility.GetValue.Lookup("Subject", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text);
                    string content = string.Format(MyUtility.GetValue.Lookup("content", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text)
                                     + Environment.NewLine
                                     + "Please Approve and Check Fabric Inspection";
                    this.ToExcel(true, "Regular");
                    var email = new MailTo(Env.Cfg.MailFrom, mailto, ccAddress, subject, this.excelFile, content, false, true);
                    email.ShowDialog(this);
                }
            }

            if (MyUtility.Convert.GetBool(this.maindr["PhysicalEncode"]) && this.maindr["Status"].ToString() == "Approved")
            {
                // *****Send Excel Email 完成 需寄給Factory MC*****
                string sqlcmd = $@"select EMail from Pass1 p inner join View_WH_Orders o on o.MCHandle = p.id where o.id='{this.maindr["POID"]}'";
                string mailto = MyUtility.GetValue.Lookup(sqlcmd);
                if (MyUtility.Check.Empty(mailto))
                {
                    sqlcmd = $@"select EMail from TPEPass1 p inner join View_WH_Orders o on o.MCHandle = p.id where o.id='{this.maindr["POID"]}'";
                    mailto = MyUtility.GetValue.Lookup(sqlcmd);
                }

                string strToAddress = MyUtility.GetValue.Lookup("ToAddress", "007", "MailTo", "ID");
                mailto = mailto + ";" + strToAddress;
                string mailCC = MyUtility.GetValue.Lookup("CCAddress", "007", "MailTo", "ID");
                string subject = string.Format(MyUtility.GetValue.Lookup("Subject", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text);
                string content = string.Format(MyUtility.GetValue.Lookup("content", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text);

                List<string> attachment = new List<string>();
                List<string> attachmentPic = this.GetAttachment();
                if (attachmentPic != null && attachmentPic.Count > 0)
                {
                    attachment = attachmentPic;
                }

                this.ToExcel(true, "Regular");
                attachment.Add(this.excelFile);

                var email = new MailTo(Env.Cfg.MailFrom, mailto, mailCC, subject, content, attachment, false, true);
                email.ShowDialog(this);

                foreach (var item in attachment)
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        private List<string> GetAttachment()
        {
            List<string> attachment = new List<string>();
            string sqlcmd = $@"
select fdr.Id, f.Roll, f.Dyelot, fdr.FabricDefectID, fd.DefectLocation
from FIR_Physical_Defect_Realtime fdr
inner join FIR_Physical f on fdr.FIR_PhysicalDetailUKey = f.DetailUkey
inner join FIR_Physical_Defect fd on fd.FIR_PhysicalDetailUKey = fdr.FIR_PhysicalDetailUkey
where f.id =  '{this.maindr["ID"]}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return null;
            }

            // 找到所有圖檔名稱
            string ukeys = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["id"])).JoinToString(",");
            if (!MyUtility.Check.Empty(ukeys))
            {
                sqlcmd = $@"
select distinct SourceFile, AddDate = format(AddDate,'yyyyMM'),UniqueKey
from Clip
where TableName = 'FIR_Physical_Defect_Realtime'
and UniqueKey in ({ukeys})
order by UniqueKey
";
                result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt2);
                if (!result)
                {
                    this.ShowErr(result);
                    return null;
                }

                int virtualSeqnum = 1;
                foreach (DataRow dr2 in dt2.Rows)
                {
                    string roll = MyUtility.Convert.GetString(dt.Select($"ID = {dr2["UniqueKey"]}")[0]["Roll"]);
                    string dyelot = MyUtility.Convert.GetString(dt.Select($"ID = {dr2["UniqueKey"]}")[0]["dyelot"]);
                    string defectLocation = MyUtility.Convert.GetString(dt.Select($"ID = {dr2["UniqueKey"]}")[0]["DefectLocation"]);
                    string fabricDefectID = MyUtility.Convert.GetString(dt.Select($"ID = {dr2["UniqueKey"]}")[0]["FabricDefectID"]);
                    string virtualSeq = virtualSeqnum.ToString().PadLeft(2, '0');
                    string newFileName = $"{this.maindr["POID"]}_{this.maindr["seq1"]}{this.maindr["seq2"]}_{roll}_{dyelot}_{defectLocation}_{fabricDefectID}_{virtualSeq}.png";
                    virtualSeqnum++;
                    string oripath = Path.Combine(Env.Cfg.ClipDir, MyUtility.Convert.GetString(dr2["AddDate"]), MyUtility.Convert.GetString(dr2["SourceFile"]));
                    string newpath = Path.Combine(Env.Cfg.ReportTempDir, newFileName);
                    File.Copy(oripath, newpath, true);
                    attachment.Add(newpath);
                }
            }

            return attachment;
        }
    }
}
