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

namespace Sci.Production.Quality
{
    public partial class P01_PhysicalInspection : Win.Subs.Input4
    {
        private readonly DataRow maindr;
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private string excelFile = string.Empty;
        private DataTable Fir_physical_Defect;

        public P01_PhysicalInspection(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();

            this.txtsupplier.TextBox1.IsSupportEditMode = false;
            this.txtsupplier.TextBox1.ReadOnly = true;
            this.txtuserApprover.TextBox1.IsSupportEditMode = false;
            this.txtuserApprover.TextBox1.ReadOnly = true;
            this.maindr = mainDr;
            this.textID.Text = keyvalue1;
            this.QueryHeader();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.Button_enable();
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery()
        {
            this.QueryHeader();
            return base.OnRequery();
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

            string order_cmd = string.Format("Select * from orders WITH (NOLOCK) where id='{0}'", this.maindr["POID"]);
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

            string po_supp_detail_cmd = string.Format("select SCIRefno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1='{1}' and seq2='{2}'", this.maindr["POID"], this.maindr["seq1"], this.maindr["seq2"]);
            DataRow po_supp_detail_dr;
            if (MyUtility.Check.Seek(po_supp_detail_cmd, out po_supp_detail_dr))
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
            this.displaySCIRefno1.Text = MyUtility.GetValue.Lookup("Description", this.maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            this.displaydescDetail.Text = MyUtility.GetValue.Lookup("descDetail", this.maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            this.displaySEQ.Text = this.maindr["Seq1"].ToString() + "-" + this.maindr["Seq2"].ToString();
            this.displaySP.Text = this.maindr["POID"].ToString();
            this.displayWKNo.Text = this.maindr["Exportid"].ToString();
            this.checkNonInspection.Value = this.maindr["nonphysical"].ToString();
            this.displayResult.Text = this.maindr["physical"].ToString();
            this.txtuserApprover.TextBox1.Text = this.maindr["Approve"].ToString();
            this.txtPhysicalInspector.Text = this.maindr["PhysicalInspector"].ToString();
        }

        private DataTable datas2;

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            this.datas2 = datas;
            if (!datas.Columns.Contains("CutWidth"))
            {
                datas.ColumnsDecimalAdd("CutWidth");
                foreach (DataRow dr in datas.Rows)
                {
                    dr["CutWidth"] = MyUtility.GetValue.Lookup(
                        string.Format(
                        @"
                                                                        select width
                                                                        from Fabric 
                                                                        inner join Fir on Fabric.SCIRefno = fir.SCIRefno
                                                                        where Fir.ID = '{0}'", dr["id"]), null, null);
                }
            }

            base.OnRequeryPost(datas);
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("POID", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));
            datas.Columns.Add("NewKey", typeof(int));
            datas.Columns.Add("LthOfDiff", typeof(decimal));
            int i = 0;
            foreach (DataRow dr in datas.Rows)
            {
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["NewKey"] = i;
                dr["poid"] = this.maindr["poid"];
                dr["SEQ1"] = this.maindr["SEQ1"];
                dr["SEQ2"] = this.maindr["SEQ2"];
                dr["LthOfDiff"] = MyUtility.Convert.GetDecimal(dr["ActualYds"]) - MyUtility.Convert.GetDecimal(dr["TicketYds"]);

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

            this.Get_total_point();
        }

        protected void Get_total_point()
        {
            double double_ActualYds = MyUtility.Convert.GetDouble(this.CurrentData["ActualYds"]);
            double actualYdsT = Math.Floor(MyUtility.Convert.GetDouble(this.CurrentData["ActualYds"]) - 0.01);
            double actualWidth = MyUtility.Convert.GetDouble(this.CurrentData["actualwidth"]);
            double actualYdsF = actualYdsT - (actualYdsT % 5);
            double def_locT = 0d;
            double def_locF = 0d;

            // Act.Yds Inspected更動時剔除Fir_physical_Defect不在範圍的資料

            // foreach (DataRow dr in Fir_physical_Defect)
            for (int i = 0; i <= this.Fir_physical_Defect.Rows.Count - 1; i++)
            {
                if (this.Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted
                    && this.Fir_physical_Defect.Rows[i]["NewKey"].EqualString(this.CurrentData["NewKey"]))
                {
                    // if (dr.RowState != DataRowState.Deleted)
                    // {
                    def_locF = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[0]);
                    def_locT = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[1]);
                    if (def_locF >= double_ActualYds && this.Fir_physical_Defect.Rows[i]["NewKey"].ToString() == this.CurrentData["NewKey"].ToString())
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
                        DataRow[] ary = this.Fir_physical_Defect.Select(string.Format(@"NewKey = {0} and DefectLocation like '%{1}%'", this.CurrentData["NewKey"].ToString(), def_locF));
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

            double sumPoint = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Compute("Sum(Point)", string.Format("NewKey = {0}", this.CurrentData["NewKey"])));

            // PointRate 國際公式每五碼最高20點
            this.CurrentData["TotalPoint"] = sumPoint;

            #region 依dbo.PointRate 來判斷新的PointRate計算公式
            DataRow drPoint;
            string pointRateID = MyUtility.Check.Seek($@"select * from PointRate where Brandid='{this.displayBrand.Text}'", out drPoint) ? drPoint["id"].ToString() : "1";

            this.CurrentData["PointRate"] = (pointRateID == "2") ?
                ((double_ActualYds == 0 || actualWidth == 0) ? 0 : Math.Round((sumPoint * 3600) / (double_ActualYds * actualWidth), 2)) :
                (double_ActualYds == 0) ? 0 : Math.Round((sumPoint / double_ActualYds) * 100, 2);

            #endregion

            #region Grade,Result
            string weaveTypeid = MyUtility.GetValue.Lookup("WeaveTypeId", this.maindr["SCiRefno"].ToString(), "Fabric", "SciRefno");

            string grade_cmd = $@"

---- 1. 取得預設的布種的等級
SELECT [Grade]=MIN(Grade)
INTO #default
FROM FIR_Grade f WITH (NOLOCK) 
WHERE f.WeaveTypeID = '{weaveTypeid}' 
AND f.Percentage >= IIF({this.CurrentData["PointRate"]} > 100, 100, {this.CurrentData["PointRate"]})
AND BrandID=''

---- 2. 取得該品牌布種的等級
SELECT [Grade]=MIN(Grade)
INTO #withBrandID
FROM FIR_Grade WITH (NOLOCK) 
WHERE WeaveTypeID = '{weaveTypeid}' 
AND Percentage >= IIF({this.CurrentData["PointRate"]} > 100, 100, {this.CurrentData["PointRate"]})
AND BrandID='{this.displayBrand.Text}'

---- 若該品牌有另外設定等級，就用該設定，不然用預設（主索引鍵是WeaveTypeID + Percentage + BrandID，因此不會找到多筆預設的Grade）
SELECT [Grade] = ISNULL(Brand.Grade, ISNULL((SELECT Grade FROM #default),'') ) 
FROM #withBrandID brand

SELECT [Grade] = ISNULL(Brand.Grade, ISNULL((SELECT Grade FROM #default),'') ) 
,[IsFromBrand] = IIF(Brand.Grade IS NULL, 0,1 ) 
INTO #BrandInfo
FROM #withBrandID brand

---- 結果也區分品牌
Select TOP 1 [Result] =	case Result	
						when 'P' then 'Pass'
						when 'F' then 'Fail'
					end
from Fir_Grade WITH (NOLOCK) 
WHERE WeaveTypeID = '{weaveTypeid}' 
AND Grade = (
	SELECT ISNULL(Brand.Grade, (SELECT Grade FROM #default)) 
	FROM #withBrandID brand
)
AND BrandID = IIF((SELECT IsFromBrand FROM #BrandInfo) = 1  , '{this.displayBrand.Text}' ,'') 

DROP TABLE #default,#withBrandID ,#BrandInfo

";

            DataTable[] dts;
            DBProxy.Current.Select(null, grade_cmd, out dts);

            if (dts != null && dts[0].Rows.Count > 0 && dts[1].Rows.Count > 0)
            {
                this.CurrentData["Grade"] = dts[0].Rows[0]["Grade"];
                this.CurrentData["Result"] = dts[1].Rows[0]["Result"];
            }

#endregion
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings ydscell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings totalPointcell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings resulCell = PublicPrg.Prgs.CellResult.GetGridCell();
            #region TotalPoint Double Click
            totalPointcell.EditingMouseDoubleClick += (s, e) =>
            {
                this.grid.ValidateControl();
                P01_PhysicalInspection_Defect frm = new P01_PhysicalInspection_Defect(this.Fir_physical_Defect, this.maindr, this.EditMode);
                frm.Set(this.EditMode, this.Datas, this.grid.GetDataRow(e.RowIndex));
                frm.ShowDialog(this);
                if (this.EditMode)
                {
                    this.Get_total_point();
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
                    dr["CutWidth"] = dr["CutWidth"] = MyUtility.GetValue.Lookup(
                        string.Format(
                        @"
                                                                    select width
                                                                    from Fabric 
                                                                    inner join Fir on Fabric.SCIRefno = fir.SCIRefno
                                                                    where Fir.ID = '{0}'", dr["id"]), null, null);
                    dr["Result"] = "Pass";
                    dr["Grade"] = "A";
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

                if (MyUtility.Check.Empty(e.FormattedValue)) // 沒填入資料,清空dyelot
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
                    string cmd = string.Format(
                        @"
                                                                        select width
                                                                        from Fabric 
                                                                        inner join Fir on Fabric.SCIRefno = fir.SCIRefno
                                                                        where Fir.ID = '{0}'", dr["id"]);
                    dr["CutWidth"] = dr["CutWidth"] = MyUtility.GetValue.Lookup(cmd, null, null);
                    dr["Result"] = "Pass";
                    dr["Grade"] = "A";
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

                // 判斷Actualyds調整範圍後Fir_physical_Defect會被清掉的話需要提示
                double double_ActualYds = Convert.ToDouble(newvalue);
                double def_loc = 0d;
                StringBuilder hintmsg = new StringBuilder();
                for (int i = 0; i <= this.Fir_physical_Defect.Rows.Count - 1; i++)
                {
                    if (this.Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted)
                    {
                        def_loc = MyUtility.Convert.GetDouble(this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[0]);
                        if (def_loc >= double_ActualYds && this.Fir_physical_Defect.Rows[i]["NewKey"].ToString() == this.CurrentData["NewKey"].ToString())
                        {
                            hintmsg.Append("Yds : " + this.Fir_physical_Defect.Rows[i]["DefectLocation"].ToString() + " , Defects : " + this.Fir_physical_Defect.Rows[i]["DefectRecord"].ToString() +
                                                " , Point : " + this.Fir_physical_Defect.Rows[i]["Point"].ToString() + "\n");
                        }
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
                }

                // 若新的 Act.Yds\nInspected = 0，則第三層必須清空
                if (newvalue.EqualDecimal(0))
                {
                    this.CleanDefect(dr["NewKey"].ToString());
                }

                dr["Actualyds"] = e.FormattedValue;
                dr.EndEdit();
                this.Get_total_point();
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
            .Text("Grade", header: "Grade", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .CheckBox("moisture", header: "Moisture", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name")
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

        /// <inheritdoc/>
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
                    update_cmd = update_cmd + string.Format(
                    @"Delete From Fir_physical Where DetailUkey = {0} ;Delete From FIR_Physical_Defect Where FIR_PhysicalDetailUKey = {0} ; Delete From FIR_Physical_Defect_Realtime Where FIR_PhysicalDetailUKey = {0} ;",
                    dr["DetailUKey", DataRowVersion.Original]);
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

                    add_cmd = string.Format(
                    @"Insert into Fir_Physical
(ID,Roll,Dyelot,TicketYds,ActualYds,FullWidth,ActualWidth,TotalPoint,PointRate,Grade,Result,Remark,InspDate,Inspector,Moisture,AddName,AddDate) 
Values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},'{9}','{10}','{11}','{12}','{13}',{14},'{15}',GetDate()) ;",
                    dr["ID"], dr["roll"].ToString().Replace("'", "''"), dr["Dyelot"], dr["TicketYds"], dr["ActualYds"], dr["FullWidth"], dr["ActualWidth"], dr["TotalPoint"], dr["PointRate"], dr["Grade"], dr["Result"], dr["Remark"].ToString().Replace("'", "''"), inspdate, dr["Inspector"], bolMoisture, this.loginID);
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
                    update_cmd = update_cmd + string.Format(
                        @"Update Fir_Physical set Roll = '{0}' ,Dyelot='{1}',TicketYds = {2},ActualYds = {3},FullWidth ={4},ActualWidth={5},TotalPoint ={6},PointRate={7},Grade='{8}',Result='{9}',Remark='{10}',InspDate='{11}',Inspector='{12}',Moisture={13},EditName = '{14}',EditDate = GetDate() 
Where DetailUkey = {15};",
                        dr["roll"].ToString().Replace("'", "''"), dr["Dyelot"], dr["TicketYds"], dr["ActualYds"], dr["FullWidth"], dr["ActualWidth"], dr["TotalPoint"], dr["PointRate"], dr["Grade"], dr["Result"], dr["Remark"].ToString().Replace("'", "''"), inspdate, dr["Inspector"], bolMoisture, this.loginID, dr["DetailUkey"]);
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

            if (update_cmd1 != string.Empty)
            {
                upResult = DBProxy.Current.Execute(null, update_cmd1);
            }
            #endregion
            return upResult;
        }

        private void BtnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            if (MyUtility.Check.Empty(this.CurrentData) && this.btnEncode.Text == "Encode")
            {
                MyUtility.Msg.WarningBox("Data not found! ");
                return;
            }

            if (!MyUtility.Convert.GetBool(this.maindr["PhysicalEncode"])) // Encode
            {
                if (!MyUtility.Convert.GetBool(this.maindr["nonPhysical"])) // 只要沒勾選就要判斷，有勾選就可直接Encode
                {
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

                DataTable gridTb = (DataTable)this.gridbs.DataSource;
                DataRow[] resultAry = gridTb.Select("Result = 'Fail'");
                string result = "Pass";
                if (resultAry.Length > 0)
                {
                    result = "Fail";
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
            else // Amend
            {
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

        private bool ToExcel(bool isSendMail, string type)
        {
            #region DataTables && 共用變數
            // FabricDefect 基本資料 DB
            DataTable dtBasic;
            DualResult result;
            if (result = DBProxy.Current.Select("Production", "SELECT id,type,DescriptionEN FROM FabricDefect WITH (NOLOCK) order by ID", out dtBasic))
            {
                if (dtBasic.Rows.Count < 1)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }

            // FIR_Physical_Defect DB
            DataTable dt;
            if (result = DBProxy.Current.Select("Production", string.Format("select * from FIR_Physical A WITH (NOLOCK) left JOIN FIR_Physical_Defect B WITH (NOLOCK) ON A.DetailUkey = B.FIR_PhysicalDetailUKey WHERE A.ID='{0}'", this.textID.Text), out dt))
            {
                if (dt.Rows.Count < 1)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }

            DataTable dt1;
            if (result = DBProxy.Current.Select("Production", string.Format(
               "select Roll,Dyelot,A.Result,A.Inspdate,Inspector,B.ContinuityEncode,C.SeasonID,B.TotalInspYds,B.ArriveQty,B.PhysicalEncode  from FIR_Physical a WITH (NOLOCK) left join FIR b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{0}'", this.textID.Text), out dt1))
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
            if (result = DBProxy.Current.Select("Production", string.Format("select sum(ticketYds) as TotalTicketYds from FIR_Physical WITH (NOLOCK) where id='{0}'", this.textID.Text), out dtSumQty))
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
                    if (int_Y == 2 && int_z == 0) // first time
                    {
                        worksheet.Cells[6, int_Y - 1] = "Code";
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
                        typeColumn = dtBasic.Rows[i]["type"].ToString();
                        worksheet.Cells[6, int_Y + 1] = typeColumn.ToString();
                    }
                }

                if (int_Y == 2)
                {
                    worksheet.Cells[int_X, int_Y - 1] = dtBasic.Rows[i]["id"].ToString();
                }
                else
                {
                    worksheet.Cells[int_X, int_Y] = dtBasic.Rows[i]["id"].ToString();
                }

                worksheet.Cells[int_X, int_Y + 1] = dtBasic.Rows[i]["DescriptionEN"].ToString();
                int_X++;
            }
            #endregion
            #region FIR_Physical
            #region 表頭

            excel.Cells[1, 3] = this.displaySP.Text + "-" + this.displaySEQ.Text;
            excel.Cells[1, 5] = this.displayColor.Text;
            excel.Cells[1, 7] = this.displayStyle.Text;
            excel.Cells[1, 9] = dt1.Rows[0]["SeasonID"];
            excel.Cells[1, 11] = dt1.Rows[0]["Inspector"];
            excel.Cells[2, 3] = this.displaySCIRefno.Text;
            excel.Cells[2, 5] = this.displayResult.Text;
            excel.Cells[2, 7] = this.dateLastInspectionDate.Value;
            excel.Cells[2, 9] = this.displayBrand.Text;
            excel.Cells[2, 11] = dt1.Rows[0]["TotalInspYds"];
            excel.Cells[3, 3] = this.displayBrandRefno.Text;
            excel.Cells[3, 5] = this.txtsupplier.DisplayBox1.Text.ToString();
            excel.Cells[3, 7] = this.displayWKNo.Text;
            excel.Cells[3, 9] = dtSupvisor.Rows[0]["Supervisor"];
            excel.Cells[4, 3] = this.dateArriveWHDate.Value;

            excel.Cells[4, 5] = this.displayArriveQty.Text;
            excel.Cells[4, 7] = dtSumQty.Rows[0]["TotalTicketYds"]; // Inspected Qty
            excel.Cells[4, 9] = dt1.Rows[0]["PhysicalEncode"].ToString() == "1" ? "Y" : "N";

            #endregion

            DataTable dtGrid;
            int gridCounts = 0;
            if (this.gridbs.DataSource == null || this.grid == null)
            {
                string sqlcmd = $@"select * from Production.dbo.FIR_Physical where id = {this.textID.Text}";
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
                gridCounts = this.grid.RowCount;
            }

            int rowcount = 0;

            for (int i = 0; i < gridCounts; i++)
            {
                excel.Cells[14 + (i * 8) + addline, 1] = dtGrid.Rows[rowcount]["Roll"].ToString();
                excel.Cells[14 + (i * 8) + addline, 2] = dtGrid.Rows[rowcount]["Dyelot"].ToString();
                excel.Cells[14 + (i * 8) + addline, 3] = dtGrid.Rows[rowcount]["Ticketyds"].ToString();

                // 指定欄位轉型
                Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[14 + (i * 8) + addline, 3];
                worksheet.get_Range(cell, cell).NumberFormat = "0.00";

                excel.Cells[14 + (i * 8) + addline, 4] = dtGrid.Rows[rowcount]["Actualyds"].ToString();
                excel.Cells[14 + (i * 8) + addline, 5] = dtGrid.Rows[rowcount]["Cutwidth"].ToString();
                excel.Cells[14 + (i * 8) + addline, 6] = dtGrid.Rows[rowcount]["fullwidth"].ToString();
                excel.Cells[14 + (i * 8) + addline, 7] = dtGrid.Rows[rowcount]["actualwidth"].ToString();
                excel.Cells[14 + (i * 8) + addline, 8] = dtGrid.Rows[rowcount]["totalpoint"].ToString();
                excel.Cells[14 + (i * 8) + addline, 9] = dtGrid.Rows[rowcount]["pointRate"].ToString();
                excel.Cells[14 + (i * 8) + addline, 10] = dtGrid.Rows[rowcount]["Grade"].ToString();
                excel.Cells[14 + (i * 8) + addline, 11] = dtGrid.Rows[rowcount]["Result"].ToString();
                excel.Cells[14 + (i * 8) + addline, 12] = dtGrid.Rows[rowcount]["Remark"].ToString();
                rowcount++;

                #region FIR_Physical_Defect

                // 變色 titile
                worksheet.Range[excel.Cells[15 + (i * 8) + addline, 1], excel.Cells[15 + (i * 8) + addline, 10]].Interior.colorindex = 38;
                worksheet.Range[excel.Cells[15 + (i * 8) + addline, 1], excel.Cells[15 + (i * 8) + addline, 10]].Borders.LineStyle = 1;
                worksheet.Range[excel.Cells[15 + (i * 8) + addline, 1], excel.Cells[15 + (i * 8) + addline, 10]].Font.Bold = true;

                DataTable dtRealTime;

                if (!(result = DBProxy.Current.Select("Production", $@"
SELECT Yards,FabricdefectID,count(1) cnt
FROM [Production].[dbo].[FIR_Physical_Defect_Realtime] 
where FIR_PhysicalDetailUkey={dtGrid.Rows[rowcount - 1]["detailUkey"]} 
group by Yards,FabricdefectID
order by Yards
", out dtRealTime)))
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
                                excel.Cells[15 + (i * 8) + addline, ii] = "Yards";
                            }

                            excel.Cells[16 + (i * 8) + addline, ii - (cntnextline * 10)] = dtRealTime.Rows[cntRealTime]["Yards"];
                            cntRealTime++;
                        }
                        else
                        {
                            if (cntnextline == 0)
                            {
                                excel.Cells[15 + (i * 8) + addline, ii] = "Defect";
                            }

                            excel.Cells[16 + (i * 8) + addline, ii - (cntnextline * 10)] = dtRealTime.Rows[cntRealTime - 1]["FabricdefectID"].ToString() + dtRealTime.Rows[cntRealTime - 1]["cnt"].ToString();

                            Microsoft.Office.Interop.Excel.Range formatRange = worksheet.get_Range($"{MyExcelPrg.GetExcelColumnName(cntX - (cntnextline * 10))}{16 + (i * 8) + addline}");
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
                            excel.Cells[15 + (i * 8) + addline, ii] = "Yards";
                        }
                        else
                        {
                            excel.Cells[15 + (i * 8) + addline, ii] = "Defect";
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

                            excel.Cells[16 + (i * 8) + addline, ii - (nextline * 10)] = dtDefect.Rows[pDrowcount]["DefectLocation"];
                            pDrowcount++;
                        }
                        else
                        {
                            excel.Cells[16 + (i * 8) + addline, ii - (nextline * 10)] = dtDefect.Rows[pDrowcount - 1]["DefectRecord"];
                        }
                    }
                }

                worksheet.Range[excel.Cells[17 + (i * 8) + addline, 1], excel.Cells[17 + (i * 8) + addline, 10]].Font.Bold = true;
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
where a.ID='{0}' and a.Roll='{1}' ORDER BY A.Roll", this.textID.Text, dtGrid.Rows[rowcount - 1]["Roll"].ToString()), out dtcombo))
                {
                    if (dtcombo.Rows.Count < 1)
                    {
                        excel.Cells[17 + (i * 8) + addline, 2] = "Result";
                        excel.Cells[17 + (i * 8) + addline, 3] = "Comment";
                        excel.Cells[17 + (i * 8) + addline, 4] = "Inspector";
                        excel.Cells[18 + (i * 8) + addline, 1] = "Contiunity ";
                        excel.Cells[19 + (i * 8) + addline, 1] = "Shad band";
                        excel.Cells[20 + (i * 8) + addline, 1] = "Weight";
                        excel.Cells[21 + (i * 8) + addline, 1] = "Moisture";
                    }
                    else
                    {
                        excel.Cells[17 + (i * 8) + addline, 2] = "Result";
                        excel.Cells[17 + (i * 8) + addline, 3] = "Comment";
                        excel.Cells[17 + (i * 8) + addline, 4] = "Inspector";
                        excel.Cells[18 + (i * 8) + addline, 1] = "Contiunity ";
                        excel.Cells[19 + (i * 8) + addline, 1] = "Shad band";
                        excel.Cells[20 + (i * 8) + addline, 1] = "Weight";
                        excel.Cells[21 + (i * 8) + addline, 1] = "Moisture";

                        excel.Cells[18 + (i * 8) + addline, 2] = dtcombo.Rows[0]["Result_c"].ToString();
                        excel.Cells[18 + (i * 8) + addline, 3] = "'" + dtcombo.Rows[0]["Remark_c"].ToString();
                        excel.Cells[18 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_c"].ToString();

                        excel.Cells[19 + (i * 8) + addline, 2] = dtcombo.Rows[0]["Result_s"].ToString();

                        // 開頭加單引號防止特殊字元使excel產生失敗
                        excel.Cells[19 + (i * 8) + addline, 3] = "'" + dtcombo.Rows[0]["Remark_s"].ToString();
                        excel.Cells[19 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_s"].ToString();

                        excel.Cells[20 + (i * 8) + addline, 2] = dtcombo.Rows[0]["Result_w"].ToString();
                        excel.Cells[20 + (i * 8) + addline, 3] = dtcombo.Rows[0]["Remark_w"].ToString();
                        excel.Cells[20 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_w"].ToString();

                        if ((bool)dtcombo.Rows[0]["Moisture"])
                        {
                            excel.Cells[21 + (i * 8) + addline, 2] = dtcombo.Rows[0]["Result_m"].ToString();
                        }

                        excel.Cells[21 + (i * 8) + addline, 3] = dtcombo.Rows[0]["Remark_m"].ToString();
                        excel.Cells[21 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_m"].ToString();
                    }

                    worksheet.Range[excel.Cells[17 + (i * 8) + addline, 1], excel.Cells[17 + (i * 8) + addline, 4]].Interior.colorindex = 38;
                    worksheet.Range[excel.Cells[17 + (i * 8) + addline, 1], excel.Cells[17 + (i * 8) + addline, 4]].Borders.LineStyle = 1;

                    worksheet.Range[excel.Cells[17 + (i * 8) + addline, 1], excel.Cells[17 + (i * 8) + addline, 4]].Font.Bold = true;
                    worksheet.Range[excel.Cells[18 + (i * 8) + addline, 1], excel.Cells[21 + (i * 8) + addline, 1]].Font.Bold = true;
                }
            }

            #endregion
            worksheet.Range[excel.Cells[13, 1], excel.Cells[13, 11]].Borders.LineStyle = 1;
            worksheet.Range[excel.Cells[13, 1], excel.Cells[13, 11]].Borders.Weight = 3;
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
                DataRow dr;
                string cmd_leader = $@"
select ToAddress = stuff ((select concat (';', tmp.email)
from (
	select distinct email from pass1
	where id in (select Supervisor from pass1 where  id='{Env.User.UserID}')
			or id in (select Manager from Pass1 where id = '{Env.User.UserID}')
) tmp
for xml path('')
), 1, 1, '')";

                if (MyUtility.Check.Seek(cmd_leader, out dr))
                {
                    string mailto = dr["ToAddress"].ToString();
                    string ccAddress = Env.User.MailAddress;
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
                string strToAddress = MyUtility.GetValue.Lookup("ToAddress", "007", "MailTo", "ID");
                string mailto = strToAddress;
                string mailCC = MyUtility.GetValue.Lookup("CCAddress", "007", "MailTo", "ID");
                string subject = string.Format(MyUtility.GetValue.Lookup("Subject", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text);
                string content = string.Format(MyUtility.GetValue.Lookup("content", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text);

                this.ToExcel(true, "Regular");
                var email = new MailTo(Env.Cfg.MailFrom, mailto, mailCC, subject, this.excelFile, content, false, true);
                email.ShowDialog(this);
            }
        }
    }
}
