using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using Sci.Production.Quality;
using System.Runtime.InteropServices;
using Sci.Production.PublicPrg;
using Sci.Utility.Excel;

namespace Sci.Production.Quality
{
    public partial class P01_PhysicalInspection : Sci.Win.Subs.Input4
    {
        private DataRow maindr;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string excelFile = "";
        DataTable Fir_physical_Defect;
        int addline = 0;
        public P01_PhysicalInspection(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3,DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)

        {
            InitializeComponent();

            txtsupplier.TextBox1.IsSupportEditMode = false;
            txtsupplier.TextBox1.ReadOnly = true;
            txtuserApprover.TextBox1.IsSupportEditMode = false;
            txtuserApprover.TextBox1.ReadOnly = true;
            maindr = mainDr;
            this.textID.Text = keyvalue1;
            QueryHeader();
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            button_enable();
        }

        protected override DualResult OnRequery()
        {
            QueryHeader();
            return base.OnRequery();
        }

        private void QueryHeader()
        {
            #region Encode/Approve Enable
            button_enable();
            btnEncode.Text = MyUtility.Convert.GetBool(maindr["PhysicalEncode"]) ? "Amend" : "Encode";
            btnApprove.Text = maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion

            this.save.Enabled = !MyUtility.Convert.GetBool(maindr["PhysicalEncode"]);

            // SendMail 
            this.btnSendMail.Enabled = MyUtility.Convert.GetBool(maindr["PhysicalEncode"]);

            string order_cmd = string.Format("Select * from orders WITH (NOLOCK) where id='{0}'", maindr["POID"]);
            DataRow order_dr;
            if (MyUtility.Check.Seek(order_cmd, out order_dr))
            {
                displayBrand.Text = order_dr["Brandid"].ToString();
                displayStyle.Text = order_dr["Styleid"].ToString();
            }
            else
            {
                displayBrand.Text = "";
                displayStyle.Text = "";
            }
            string Receiving_cmd = string.Format("select a.exportid,a.WhseArrival ,b.Refno from Receiving a WITH (NOLOCK) inner join FIR b WITH (NOLOCK) on a.Id=b.Receivingid where b.id='{0}'", maindr["id"]);
            DataRow rec_dr;
            if (MyUtility.Check.Seek(Receiving_cmd, out rec_dr))
            {
                displayBrandRefno.Text = rec_dr["Refno"].ToString();
            }
            else
            {
                displayBrandRefno.Text = "";
            }
            string po_cmd = string.Format("Select * from po_supp WITH (NOLOCK) where id='{0}' and seq1 = '{1}'", maindr["POID"], maindr["seq1"]);
            DataRow po_dr;
            if (MyUtility.Check.Seek(po_cmd, out po_dr))
            {
                txtsupplier.TextBox1.Text = po_dr["suppid"].ToString();

            }
            else
            {
                txtsupplier.TextBox1.Text = "";
            }
            string po_supp_detail_cmd = string.Format("select SCIRefno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1='{1}' and seq2='{2}'", maindr["POID"], maindr["seq1"], maindr["seq2"]);
            DataRow po_supp_detail_dr;
            if (MyUtility.Check.Seek(po_supp_detail_cmd, out po_supp_detail_dr))
            {
                displayColor.Text = po_supp_detail_dr["colorid"].ToString();
            }
            else
            {
                displayColor.Text = "";
            }
            displaySCIRefno.Text = maindr["SCIRefno"].ToString();
            displayApprover.Text = maindr["ApproveDate"].ToString();
            displayArriveQty.Text = maindr["arriveQty"].ToString();
            dateArriveWHDate.Value = MyUtility.Convert.GetDate(maindr["whseArrival"]);
            dateLastInspectionDate.Value = MyUtility.Convert.GetDate(maindr["physicalDate"]);
            displaySCIRefno1.Text = MyUtility.GetValue.Lookup("Description", maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            displaydescDetail.Text = MyUtility.GetValue.Lookup("descDetail", maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            displaySEQ.Text = maindr["Seq1"].ToString() + "-" + maindr["Seq2"].ToString();
            displaySP.Text = maindr["POID"].ToString();
            displayWKNo.Text = maindr["Exportid"].ToString();
            checkNonInspection.Value = maindr["nonphysical"].ToString();
            displayResult.Text = maindr["physical"].ToString();
            txtuserApprover.TextBox1.Text = maindr["Approve"].ToString();
            txtPhysicalInspector.Text = maindr["PhysicalInspector"].ToString();
        }

        DataTable datas2;

        protected override void OnRequeryPost(DataTable datas)
        {
            datas2 = datas;
            if (!datas.Columns.Contains("CutWidth"))
            {
                datas.ColumnsDecimalAdd("CutWidth");
                foreach (DataRow dr in datas.Rows)
                {
                    dr["CutWidth"] = MyUtility.GetValue.Lookup(string.Format(@"
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
                dr["poid"] = maindr["poid"];
                dr["SEQ1"] = maindr["SEQ1"];
                dr["SEQ2"] = maindr["SEQ2"];
                dr["LthOfDiff"] = MyUtility.Convert.GetDecimal(dr["ActualYds"]) - MyUtility.Convert.GetDecimal(dr["TicketYds"]);

                i++;
            }
            #region 撈取下一層資料Defect
            string str_defect = string.Format("Select a.* ,b.NewKey from Fir_physical_Defect a WITH (NOLOCK) ,#tmp b Where a.id = b.id and a.FIR_PhysicalDetailUKey = b.DetailUkey");

            MyUtility.Tool.ProcessWithDatatable(datas, "ID,NewKey,DetailUkey", str_defect, out Fir_physical_Defect);
            #endregion
        }

        /// <summary>
        /// 刪除第三層資料
        /// By Ukey
        /// </summary>
        /// <param name="strNewKey">需要刪除第三層所對應的 NewUkey</param>
        private void cleanDefect(string strNewKey)
        {
            for (int i = Fir_physical_Defect.Rows.Count - 1; i >= 0; i--)
            {
                if (Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted 
                    && Fir_physical_Defect.Rows[i]["NewKey"].EqualString(strNewKey))
                {
                    Fir_physical_Defect.Rows[i].Delete();
                }
            }     

            get_total_point();
        }

        protected void get_total_point()
        {
            double double_ActualYds = MyUtility.Convert.GetDouble(CurrentData["ActualYds"]);            
            double ActualYdsT = Math.Floor(MyUtility.Convert.GetDouble(CurrentData["ActualYds"])  -0.01);
            double ActualWidth = MyUtility.Convert.GetDouble(CurrentData["actualwidth"]);
            double ActualYdsF = ActualYdsT - (ActualYdsT % 5);
            double def_locT = 0d;
            double def_locF = 0d;
            //Act.Yds Inspected更動時剔除Fir_physical_Defect不在範圍的資料

            //foreach (DataRow dr in Fir_physical_Defect)
            for (int i = 0; i <= Fir_physical_Defect.Rows.Count - 1; i++)
            {
                if (Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted
                    && Fir_physical_Defect.Rows[i]["NewKey"].EqualString(CurrentData["NewKey"]))
                {
                    // if (dr.RowState != DataRowState.Deleted)
                    //{
                    def_locF = MyUtility.Convert.GetDouble(Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[0]);
                    def_locT = MyUtility.Convert.GetDouble(Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[1]);
                    if (def_locF >= double_ActualYds && Fir_physical_Defect.Rows[i]["NewKey"].ToString() == CurrentData["NewKey"].ToString())
                    {
                        Fir_physical_Defect.Rows[i].Delete();
                    }
                    // }

                    //DefectLocation的範圍如果與目前ActualYds界限值不符的話就update DefectLocation
                    if (def_locF == ActualYdsF && def_locT != ActualYdsT)
                    {
                        Fir_physical_Defect.Rows[i]["DefectLocation"] = ActualYdsF.ToString().PadLeft(3, '0') + "-" + ActualYdsT.ToString().PadLeft(3, '0');
                    }

                    if (def_locT < ActualYdsF && def_locT - def_locF != 4)
                    {
                        /*  
                         *  如果DefectLocation的相同範圍內有２筆資料,就必須把舊的給刪除
                         *  ex: 080-083 汙點D1, 080-084 汙點A1/D1 ,就必須要080-083給刪除
                         *  不然將080-083 改為080-084 Pkey就會重複
                        */
                        DataRow[] Ary = Fir_physical_Defect.Select(string.Format(@"NewKey = {0} and DefectLocation like '%{1}%'", CurrentData["NewKey"].ToString(), def_locF));
                        if (Ary.Length > 1)
                        {
                            Fir_physical_Defect.Rows[i].Delete();
                        }
                        else
                        {
                            Fir_physical_Defect.Rows[i]["DefectLocation"] = def_locF.ToString().PadLeft(3, '0') + "-" + (def_locF + 4).ToString().PadLeft(3, '0');
                        }
                    }
                }
            }

            Double SumPoint = MyUtility.Convert.GetDouble(Fir_physical_Defect.Compute("Sum(Point)", string.Format("NewKey = {0}", CurrentData["NewKey"])));
            //PointRate 國際公式每五碼最高20點
            CurrentData["TotalPoint"] = SumPoint;

            #region 依dbo.PointRate 來判斷新的PointRate計算公式
            DataRow drPoint;
            string PointRateID = (MyUtility.Check.Seek($@"select * from PointRate where Brandid='{displayBrand.Text}'", out drPoint)) ? drPoint["id"].ToString() : "1";

            CurrentData["PointRate"] = (PointRateID == "2") ?
                ((double_ActualYds == 0 || ActualWidth == 0) ? 0 : Math.Round((SumPoint * 3600) / (double_ActualYds * ActualWidth), 2)) :
                (double_ActualYds == 0) ? 0 : Math.Round((SumPoint / double_ActualYds) * 100, 2);
         
            #endregion

            #region Grade,Result
            string WeaveTypeid = MyUtility.GetValue.Lookup("WeaveTypeId", maindr["SCiRefno"].ToString(), "Fabric", "SciRefno");

            string grade_cmd = $@"

---- 1. 取得預設的布種的等級
SELECT [Grade]=MIN(Grade)
INTO #default
FROM FIR_Grade f WITH (NOLOCK) 
WHERE f.WeaveTypeID = '{WeaveTypeid}' 
AND f.Percentage >= IIF({CurrentData["PointRate"]} > 100, 100, {CurrentData["PointRate"]})
AND BrandID=''

---- 2. 取得該品牌布種的等級
SELECT [Grade]=MIN(Grade)
INTO #withBrandID
FROM FIR_Grade WITH (NOLOCK) 
WHERE WeaveTypeID = '{WeaveTypeid}' 
AND Percentage >= IIF({CurrentData["PointRate"]} > 100, 100, {CurrentData["PointRate"]})
AND BrandID='{displayBrand.Text}'

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
WHERE WeaveTypeID = '{WeaveTypeid}' 
AND Grade = (
	SELECT ISNULL(Brand.Grade, (SELECT Grade FROM #default)) 
	FROM #withBrandID brand
)
AND BrandID = IIF((SELECT IsFromBrand FROM #BrandInfo) = 1  , '{displayBrand.Text}' ,'') 

DROP TABLE #default,#withBrandID ,#BrandInfo

";

            DataTable[] dts;
            DBProxy.Current.Select(null, grade_cmd, out dts);

            if (dts != null && dts[0].Rows.Count > 0 && dts[1].Rows.Count > 0)
            {
                CurrentData["Grade"] = dts[0].Rows[0]["Grade"];
                CurrentData["Result"] = dts[1].Rows[0]["Result"];

            }

#endregion
        }

        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings Rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings Ydscell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings TotalPointcell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings ResulCell = Sci.Production.PublicPrg.Prgs.cellResult.GetGridCell();
            #region TotalPoint Double Click
            TotalPointcell.EditingMouseDoubleClick += (s, e) =>
            {
                grid.ValidateControl();
                P01_PhysicalInspection_Defect frm = new P01_PhysicalInspection_Defect(Fir_physical_Defect,maindr,this.EditMode);
                frm.Set(EditMode, Datas, grid.GetDataRow(e.RowIndex));
                frm.ShowDialog(this);
                if (EditMode)
                {
                    get_total_point();
                }
            };
            #endregion
            #region Roll
            Rollcell.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (e.RowIndex == -1) return;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string originRoll = dr["Roll"].ToString();
                    string strNewKey = dr["NewKey"].ToString();

                    SelectItem sele;
                    string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' order by dyelot", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"]);

                    if (!MyUtility.Check.Seek(roll_cmd))
                    {
                        roll_cmd = string.Format("Select roll,dyelot,StockQty=qty from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' order by dyelot", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"]);
                    }
                    sele = new SelectItem(roll_cmd, "15,10,10",dr["roll"].ToString(), false, ",",columndecimals:"0,0,2");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) {
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
                    dr["CutWidth"] = dr["CutWidth"] = MyUtility.GetValue.Lookup(string.Format(@"
                                                                    select width
                                                                    from Fabric 
                                                                    inner join Fir on Fabric.SCIRefno = fir.SCIRefno
                                                                    where Fir.ID = '{0}'", dr["id"]), null, null);
                    dr["Result"] = "Pass";
                    dr["Grade"] = "A";
                    dr["totalpoint"] = 0.00;
                    cleanDefect(strNewKey);
                    dr.EndEdit();
                   
                }
            };
            Rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string strNewKey = dr["NewKey"].ToString();
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue))//沒填入資料,清空dyelot
                {
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr["Ticketyds"] = 0.00;
                    dr["Actualyds"] = 0.00;
                    dr["CutWidth"] = 0.00;
                    dr["fullwidth"] = 0.00;
                    dr["actualwidth"] = 0.00;
                    dr["totalpoint"] = 0.00;
                    dr["pointRate"] = 0.00;
                    dr["Result"] = "";
                    dr["Grade"] = "";
                    dr["moisture"] = 0;
                    dr["Remark"] = "";
                    cleanDefect(strNewKey);
                    dr.EndEdit();
                    return;
                }
                if (oldvalue == newvalue) return;
                string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"], e.FormattedValue.ToString().Replace("'", "''"));

                if (!MyUtility.Check.Seek(roll_cmd))
                {
                    roll_cmd = string.Format("Select roll,dyelot,StockQty=qty from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"], e.FormattedValue.ToString().Replace("'", "''"));
                }

                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr["Ticketyds"] = roll_dr["StockQty"];
                    dr["CutWidth"] = dr["CutWidth"] = MyUtility.GetValue.Lookup(string.Format(@"
                                                                        select width
                                                                        from Fabric 
                                                                        inner join Fir on Fabric.SCIRefno = fir.SCIRefno
                                                                        where Fir.ID = '{0}'", dr["id"]), null, null);
                    dr["Result"] = "Pass";
                    dr["Grade"] = "A";
                    dr["totalpoint"] = 0.00;
                    cleanDefect(strNewKey);
                    dr.EndEdit();
                }
                else
                {
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr["Ticketyds"] = 0.00;
                    dr["Actualyds"] = 0.00;
                    dr["CutWidth"] = 0.00;
                    dr["fullwidth"] = 0.00;
                    dr["actualwidth"] = 0.00;
                    dr["totalpoint"] = 0.00;
                    dr["pointRate"] = 0.00;
                    dr["moisture"] = 0;
                    dr["Remark"] = "";
                    dr.EndEdit();
                    cleanDefect(strNewKey);
                    dr["Result"] = "";
                    dr["Grade"] = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> data not found!", e.FormattedValue));
                    return;
                }  
            };
            #endregion
            #region Act Yds
            Ydscell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Actualyds"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                //判斷Actualyds調整範圍後Fir_physical_Defect會被清掉的話需要提示
                double double_ActualYds = Convert.ToDouble( newvalue);
                double def_loc = 0d;
                StringBuilder hintmsg = new StringBuilder();
                for (int i = 0; i <= Fir_physical_Defect.Rows.Count - 1; i++)
                {
                    if (Fir_physical_Defect.Rows[i].RowState != DataRowState.Deleted)
                    {
                        def_loc = MyUtility.Convert.GetDouble(Fir_physical_Defect.Rows[i]["DefectLocation"].ToString().Split('-')[0]);
                        if (def_loc >= double_ActualYds && Fir_physical_Defect.Rows[i]["NewKey"].ToString() == CurrentData["NewKey"].ToString())
                        {
                            hintmsg.Append("Yds : " + Fir_physical_Defect.Rows[i]["DefectLocation"].ToString() + " , Defects : " + Fir_physical_Defect.Rows[i]["DefectRecord"].ToString() +
                                                " , Point : " + Fir_physical_Defect.Rows[i]["Point"].ToString() + "\n");
                        }

                    }
                }

                if (hintmsg.Length > 0) {
                    hintmsg.Insert(0, String.Format("Position greater than {0} is defective\nChanging yds will remove the following defects\nDo you wish to change ?\n", newvalue));
                    if (MyUtility.Msg.WarningBox(hintmsg.ToString(), buttons: MessageBoxButtons.YesNo) == DialogResult.No) {
                        e.FormattedValue = oldvalue;
                        dr["Actualyds"] = oldvalue;
                        dr.EndEdit();
                        return;
                    }

                }

                // 若新的 Act.Yds\nInspected = 0，則第三層必須清空
                if (newvalue.EqualDecimal(0))
                {
                    cleanDefect(dr["NewKey"].ToString());
                }

                dr["Actualyds"] = e.FormattedValue;
                dr.EndEdit();
                get_total_point();
            };
            #endregion
           
            Helper.Controls.Grid.Generator(this.grid)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: Rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Actualyds", header: "Act.Yds\nInspected", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, settings: Ydscell)
            .Numeric("LthOfDiff", header: "Lth. Of Diff", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Text("TransactionID", header: "TransactionID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("CutWidth", header: "Cut. Width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2,iseditingreadonly:true)
            .Numeric("fullwidth", header: "Full width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2)
            .Numeric("actualwidth", header: "Actual Width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2)
            .Numeric("totalpoint", header: "Total Points", width: Widths.AnsiChars(7), integer_places: 6, iseditingreadonly: true, settings: TotalPointcell)
            .Numeric("pointRate", header: "Point Rate \nper 100yds", width: Widths.AnsiChars(5), iseditingreadonly: true, integer_places: 6,decimal_places:2)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Grade", header: "Grade", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .CheckBox("moisture", header: "Moisture", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector",header:"Inspector", width: Widths.AnsiChars(10),userNamePropertyName:"Name")
            .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true);

            grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["Actualyds"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["CutWidth"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["fullwidth"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["actualwidth"].DefaultCellStyle.BackColor = Color.LightYellow;
            grid.Columns["moisture"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.MistyRose;

            return true;

        }

        protected override void OnInsert()
        {
            DataTable Dt = (DataTable)gridbs.DataSource;
            int Maxi = MyUtility.Convert.GetInt(Dt.Compute("Max(NewKey)",""));
            base.OnInsert();
            
            DataRow selectDr = ((DataRowView)grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", loginID, "Pass1", "ID");
            selectDr["NewKey"] = Maxi + 1;
            selectDr["Moisture"] =0;
            selectDr["poid"] = maindr["poid"];
            selectDr["SEQ1"] = maindr["SEQ1"];
            selectDr["SEQ2"] = maindr["SEQ2"];
        }

        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)gridbs.DataSource;
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

        protected override DualResult OnSave()
        {
            DualResult upResult = new DualResult(true) ;
            #region Fir_Physical //因為要抓取DetailUkey 所以要自己Overridr
            string update_cmd = "";
            List<string> append_cmd = new List<string>();
            DataTable idenDt;
            string iden;
            DataTable gridTb = (DataTable)gridbs.DataSource;


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
                if (MyUtility.Check.Empty(dr["InspDate"])) inspdate = ""; //判斷Inspect Date
                else inspdate = Convert.ToDateTime(dr["InspDate"]).ToShortDateString();
                
                if (MyUtility.Convert.GetBool(dr["Moisture"])) bolMoisture = 1; 
                if (dr.RowState == DataRowState.Added)
                {
                    string add_cmd = "";
                    
                    add_cmd =string.Format(
                    @"Insert into Fir_Physical
(ID,Roll,Dyelot,TicketYds,ActualYds,FullWidth,ActualWidth,TotalPoint,PointRate,Grade,Result,Remark,InspDate,Inspector,Moisture,AddName,AddDate) 
Values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},'{9}','{10}','{11}','{12}','{13}',{14},'{15}',GetDate()) ;",
                            dr["ID"], dr["roll"].ToString().Replace("'","''"), dr["Dyelot"], dr["TicketYds"], dr["ActualYds"], dr["FullWidth"], dr["ActualWidth"], dr["TotalPoint"], dr["PointRate"], dr["Grade"], dr["Result"], dr["Remark"].ToString().Replace("'", "''"), inspdate, dr["Inspector"], bolMoisture, loginID);
                    add_cmd = add_cmd + "select @@IDENTITY as ii";
                    #region 先存入Table 撈取Idenitiy
                    upResult = DBProxy.Current.Select(null, add_cmd, out idenDt);
                    if (upResult)
                    {
                        iden = idenDt.Rows[0]["ii"].ToString(); //取出Identity

                        DataRow[] Ary = Fir_physical_Defect.Select(string.Format("NewKey={0}", dr["NewKey"]));
                        if (Ary.Length > 0)
                        {
                            foreach (DataRow Ary_dr in Ary)
                            {
                                Ary_dr["FIR_PhysicalDetailUKey"] = iden;
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
                           dr["roll"].ToString().Replace("'", "''"), dr["Dyelot"], dr["TicketYds"], dr["ActualYds"], dr["FullWidth"], dr["ActualWidth"], dr["TotalPoint"], dr["PointRate"], dr["Grade"], dr["Result"], dr["Remark"].ToString().Replace("'", "''"), inspdate, dr["Inspector"], bolMoisture, loginID,dr["DetailUkey"]);
                }
            }
            if (update_cmd != "")
            {
                upResult = DBProxy.Current.Execute(null, update_cmd);
                if (!upResult) return upResult;
            }
            #endregion 

            #region Fir_Physical_Defect
            string update_cmd1 = "";
            foreach (DataRow dr in Fir_physical_Defect.Rows)
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
                            dr["ID"], dr["FIR_PhysicalDetailUKey"], dr["DefectLocation",DataRowVersion.Current], dr["DefectRecord"], dr["Point"], dr["DefectLocation",DataRowVersion.Original]);
                }
            }
            if (update_cmd1 != "")
            {
                upResult = DBProxy.Current.Execute(null, update_cmd1);
            }
            #endregion
            return upResult;
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            string updatesql ="";
            if (MyUtility.Check.Empty(CurrentData) && this.btnEncode.Text=="Encode")
            {
                MyUtility.Msg.WarningBox("Data not found! ");
                return;
            }
            if (!MyUtility.Convert.GetBool(maindr["PhysicalEncode"])) //Encode
            {
                if (!MyUtility.Convert.GetBool(maindr["nonPhysical"])) //只要沒勾選就要判斷，有勾選就可直接Encode
                {
                    //至少收料的每ㄧ缸都要有檢驗紀錄 ,找尋有收料的缸沒在檢驗出現
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
"
                        , maindr["receivingid"], maindr["id"], maindr["POID"], maindr["seq1"], maindr["seq2"]);
                    DualResult dResult = DBProxy.Current.Select(null, cmd, out dyeDt);
                    if (dResult)
                    {
                        if (dyeDt != null && dyeDt.Rows.Count > 0)
                        {
                            string dye = "";
                            foreach (DataRow dr in dyeDt.Rows)
                            {
                                dye = dye + dr["Dyelot"].ToString() + ",";
                            }
                            MyUtility.Msg.WarningBox("<Dyelot>:" + dye + " Each Dyelot must be inspected!");
                            return;
                        }
                    }
                }
                DataTable gridTb = (DataTable)gridbs.DataSource;
                DataRow[] ResultAry = gridTb.Select("Result = 'Fail'");
                string result = "Pass";
                if (ResultAry.Length > 0) result = "Fail";
                #region  寫入虛擬欄位
                maindr["Physical"] = result;
                //maindr["PhysicalDate"] = DateTime.Now.ToShortDateString();
                maindr["PhysicalDate"] = gridTb.Compute("Max(InspDate)", "");
                maindr["PhysicalEncode"] = true;
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                maindr["PhysicalInspector"] = loginID;
                int sumPoint = MyUtility.Convert.GetInt(gridTb.Compute("Sum(totalpoint)", ""));
                decimal sumTotalYds = MyUtility.Convert.GetDecimal(gridTb.Compute("Sum(actualyds)", ""));
                maindr["TotalDefectPoint"] = sumPoint;
                maindr["TotalInspYds"] = sumTotalYds;
                #endregion 
                #region 判斷Result 是否要寫入
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr);
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = '{7}',PhysicalEncode=1,EditName='{0}',EditDate = GetDate(),Physical = '{1}',Result ='{2}',TotalDefectPoint = {4},TotalInspYds = {5},Status='{6}' ,PhysicalInspector = '{0}'
                  where id ={3}", loginID, result, returnstr[0], maindr["ID"], sumPoint, sumTotalYds, returnstr[1],Convert.ToDateTime(gridTb.Compute("Max(InspDate)", "")).ToString("yyyy/MM/dd"));
                #endregion
                
                maindr["Result"] = returnstr[0];
                maindr["Status"] = returnstr[1];
            }
            else //Amend
            {                
                #region  寫入虛擬欄位
                maindr["Physical"] = "";
                maindr["PhysicalDate"] = DBNull.Value;
                maindr["PhysicalEncode"] = false;                                
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                maindr["TotalDefectPoint"] = 0;
                maindr["TotalInspYds"] = 0;
                maindr["PhysicalInspector"] = string.Empty;

                //判斷Result and Status 必須先確認Physical="",判斷才會正確
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr);
                maindr["Result"] = returnstr[0];
                maindr["Status"] = returnstr[1];
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = null,PhysicalEncode=0,EditName='{0}',EditDate = GetDate(),Physical = '',Result ='{2}',TotalDefectPoint = 0,TotalInspYds = 0,Status='{3}' ,PhysicalInspector = ''  where id ={1}", loginID, maindr["ID"], returnstr[0], returnstr[1]);
                #endregion
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    //更新PO.FIRInspPercent和AIRInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{maindr["POID"].ToString()}'; ")))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            OnRequery();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            string updatesql = "";

            if (maindr["Status"].ToString() == "Confirmed")
            {
                maindr["Status"] = "Approved";
                maindr["Approve"] = loginID;
                maindr["ApproveDate"] = DateTime.Now.ToShortDateString();
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Approved',Approve='{0}',EditName='{0}',EditDate = GetDate(),ApproveDate = GetDate() where id ={1}", loginID, maindr["ID"]);
                #endregion
            }
            else
            {
                maindr["Status"] = "Confirmed";
                maindr["Approve"] = "";
                maindr["ApproveDate"] = DBNull.Value;
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Confirmed',Approve='',EditName='{0}',EditDate = GetDate(),ApproveDate = null where id ={1}", loginID, maindr["ID"]);
                #endregion
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            
            OnRequery();
        }

        private void button_enable()
        {
            if (maindr == null) return;
            btnEncode.Enabled = this.CanEdit && !this.EditMode && maindr["Status"].ToString() != "Approved";
            this.btnToExcel.Enabled = !this.EditMode;
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P01", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 WITH (NOLOCK) Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            int lApprove = 0; //有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }
            if (maindr["Result"].ToString() == "Pass")
            {
                btnApprove.Enabled = this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(maindr["Result"]) && !MyUtility.Check.Empty(maindr["physical"]);
            }
            else
            {
                btnApprove.Enabled = this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(maindr["Result"]) && !MyUtility.Check.Empty(maindr["physical"]);
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            ToExcel(false, "Regular");            
        }

        private void btnToExcel_defect_Click(object sender, EventArgs e)
        {
            ToExcel(false, "DefectYds");
        }

        private bool ToExcel(bool isSendMail,string Type)
        {
            #region DataTables && 共用變數
            //FabricDefect 基本資料 DB
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
            //FIR_Physical_Defect DB
            DataTable dt;
            if (result = DBProxy.Current.Select("Production", string.Format("select * from FIR_Physical A WITH (NOLOCK) left JOIN FIR_Physical_Defect B WITH (NOLOCK) ON A.DetailUkey = B.FIR_PhysicalDetailUKey WHERE A.ID='{0}'", textID.Text), out dt))
            {
                if (dt.Rows.Count < 1)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }
            DataTable dt1;
            if (result = DBProxy.Current.Select("Production", string.Format(
               "select Roll,Dyelot,A.Result,A.Inspdate,Inspector,B.ContinuityEncode,C.SeasonID,B.TotalInspYds,B.ArriveQty,B.PhysicalEncode  from FIR_Physical a WITH (NOLOCK) left join FIR b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{0}'", textID.Text), out dt1))
            {
                if (dt1.Rows.Count <= 0)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }
            DataTable dtSupvisor;
            if (result = DBProxy.Current.Select("Production", string.Format("select * from Pass1 WITH (NOLOCK) where id='{0}'", loginID), out dtSupvisor))
            {
                if (dtSupvisor.Rows.Count <= 0)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false;
                }
            }
            DataTable dtSumQty;
            if (result = DBProxy.Current.Select("Production", string.Format("select sum(ticketYds) as TotalTicketYds from FIR_Physical WITH (NOLOCK) where id='{0}'", textID.Text), out dtSumQty))
            {
                if (dtSumQty.Rows.Count <= 0)
                {
                    MessageBox.Show("Data not found!", "Warring!");
                    return false ;
                }
            }

            #endregion
            int addline = 0;
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Quality_P01_Physical_Inspection_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region FabricDefect 基本資料
            int counts = dtBasic.Rows.Count;
            int int_X = 6;
            int int_Y = 2;
            int int_z = 0;//判斷是否為第一次

            string typeColumn = "typeColumn";
            this.ShowWaitMessage("Starting EXCEL...");

            for (int i = 0; i < counts; i++)
            {
                if (dtBasic.Rows[i]["type"].ToString() != typeColumn && dtBasic.Rows[i]["type"].ToString() != null)
                {
                    int_X = 6;
                    if (int_Y == 2 && int_z == 0) //first time
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
                if (int_Y==2)
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

            excel.Cells[1, 3] = this.displaySP.Text + "-" + displaySEQ.Text;
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
            excel.Cells[4, 7] = dtSumQty.Rows[0]["TotalTicketYds"];//Inspected Qty
            excel.Cells[4, 9] = dt1.Rows[0]["PhysicalEncode"].ToString() == "1" ? "Y" : "N";


            #endregion

            DataTable dtGrid;
            int gridCounts = 0;
            if (gridbs.DataSource == null || grid == null)
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
                    dr["CutWidth"] = MyUtility.GetValue.Lookup(string.Format(@"
                                                                        select width
                                                                        from Fabric 
                                                                        inner join Fir on Fabric.SCIRefno = fir.SCIRefno
                                                                        where Fir.ID = '{0}'", dr["id"]), null, null);
                    dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                    dr["poid"] = maindr["poid"];
                    dr["SEQ1"] = maindr["SEQ1"];
                    dr["SEQ2"] = maindr["SEQ2"];
                }

            }
            else
            {
                dtGrid = (DataTable)gridbs.DataSource;
                gridCounts = grid.RowCount;
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

                excel.Cells[14 + (i * 8)+addline, 4] = dtGrid.Rows[rowcount]["Actualyds"].ToString();
                excel.Cells[14 + (i * 8)+addline, 5] = dtGrid.Rows[rowcount]["Cutwidth"].ToString();
                excel.Cells[14 + (i * 8)+addline, 6] = dtGrid.Rows[rowcount]["fullwidth"].ToString();
                excel.Cells[14 + (i * 8)+addline, 7] = dtGrid.Rows[rowcount]["actualwidth"].ToString();
                excel.Cells[14 + (i * 8)+addline, 8] = dtGrid.Rows[rowcount]["totalpoint"].ToString();
                excel.Cells[14 + (i * 8)+addline, 9] = dtGrid.Rows[rowcount]["pointRate"].ToString();
                excel.Cells[14 + (i * 8)+addline, 10] = dtGrid.Rows[rowcount]["Grade"].ToString();
                excel.Cells[14 + (i * 8)+addline, 11] = dtGrid.Rows[rowcount]["Result"].ToString();
                excel.Cells[14 + (i * 8) + addline, 12] = dtGrid.Rows[rowcount]["Remark"].ToString();
                rowcount++;


                #region FIR_Physical_Defect
                //變色 titile
                worksheet.Range[excel.Cells[15 + (i * 8) + addline, 1], excel.Cells[15 + (i * 8) + addline, 10]].Interior.colorindex = 38;
                worksheet.Range[excel.Cells[15 + (i * 8) + addline, 1], excel.Cells[15 + (i * 8) + addline, 10]].Borders.LineStyle = 1;
                worksheet.Range[excel.Cells[15 + (i * 8) + addline, 1], excel.Cells[15 + (i * 8) + addline, 10]].Font.Bold = true;

                DataTable dtRealTime;

                if ((!(result = DBProxy.Current.Select("Production", $@"
SELECT Yards,FabricdefectID,count(1) cnt
FROM [Production].[dbo].[FIR_Physical_Defect_Realtime] 
where FIR_PhysicalDetailUkey={dtGrid.Rows[rowcount - 1]["detailUkey"]} 
group by Yards,FabricdefectID
order by Yards
", out dtRealTime))))
                {
                    ShowErr(result);
                    return false;
                }

                // 依照Type來匯出Excel
                if (Type == "DefectYds" && dtRealTime.Rows.Count > 0)
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
                    int PDrowcount = 0;
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
                            excel.Cells[16 + (i * 8) + addline, ii - (nextline * 10)] = dtDefect.Rows[PDrowcount]["DefectLocation"];
                            PDrowcount++;
                        }
                        else
                        {
                            excel.Cells[16 + (i * 8) + addline, ii - (nextline * 10)] = dtDefect.Rows[PDrowcount - 1]["DefectRecord"];
                        }
                    }
                }

                worksheet.Range[excel.Cells[17 + (i * 8) + addline, 1], excel.Cells[17 + (i * 8) + addline, 10]].Font.Bold = true;
                #endregion
                DataTable dtcombo;
            

                if (result = DBProxy.Current.Select("Production", string.Format(@"
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
where a.ID='{0}' and a.Roll='{1}' ORDER BY A.Roll", textID.Text, dtGrid.Rows[rowcount-1]["Roll"].ToString()), out dtcombo))
                {
                    if (dtcombo.Rows.Count < 1)
                    {
                        excel.Cells[17 + (i * 8)+addline, 2] = "Result";
                        excel.Cells[17 + (i * 8)+addline, 3] = "Comment";
                        excel.Cells[17 + (i * 8)+addline, 4] = "Inspector";
                        excel.Cells[18 + (i * 8)+addline, 1] = "Contiunity ";
                        excel.Cells[19 + (i * 8)+addline, 1] = "Shad band";
                        excel.Cells[20 + (i * 8)+addline, 1] = "Weight";
                        excel.Cells[21 + (i * 8)+addline, 1] = "Moisture";

                    }
                    else
                    {
                        excel.Cells[17 + (i * 8)+addline, 2] = "Result";
                        excel.Cells[17 + (i * 8)+addline, 3] = "Comment";
                        excel.Cells[17 + (i * 8)+addline, 4] = "Inspector";
                        excel.Cells[18 + (i * 8)+addline, 1] = "Contiunity ";
                        excel.Cells[19 + (i * 8)+addline, 1] = "Shad band";
                        excel.Cells[20 + (i * 8)+addline, 1] = "Weight";
                        excel.Cells[21 + (i * 8) + addline, 1] = "Moisture";


                        excel.Cells[18 + (i * 8)+addline, 2] = dtcombo.Rows[0]["Result_c"].ToString();
                        excel.Cells[18 + (i * 8)+addline, 3] = "'" + dtcombo.Rows[0]["Remark_c"].ToString();
                        excel.Cells[18 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_c"].ToString();

                        excel.Cells[19 + (i * 8)+addline, 2] = dtcombo.Rows[0]["Result_s"].ToString();
                        //開頭加單引號防止特殊字元使excel產生失敗
                        excel.Cells[19 + (i * 8)+addline, 3] = "'" + dtcombo.Rows[0]["Remark_s"].ToString();
                        excel.Cells[19 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_s"].ToString();

                        excel.Cells[20 + (i * 8)+addline, 2] = dtcombo.Rows[0]["Result_w"].ToString();
                        excel.Cells[20 + (i * 8)+addline, 3] = dtcombo.Rows[0]["Remark_w"].ToString();
                        excel.Cells[20 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_w"].ToString();

                        if ((bool)dtcombo.Rows[0]["Moisture"]) {
                            excel.Cells[21 + (i * 8)+addline, 2] = dtcombo.Rows[0]["Result_m"].ToString();
                        }
                        
                        excel.Cells[21 + (i * 8)+addline, 3] = dtcombo.Rows[0]["Remark_m"].ToString();
                        excel.Cells[21 + (i * 8) + addline, 4] = dtcombo.Rows[0]["Name_m"].ToString();

                    }                   
                    worksheet.Range[excel.Cells[17 + (i * 8) + addline, 1], excel.Cells[17 + (i * 8) + addline , 4]].Interior.colorindex = 38;
                    worksheet.Range[excel.Cells[17 + (i * 8) + addline, 1], excel.Cells[17 + (i * 8) + addline, 4]].Borders.LineStyle = 1;

                    worksheet.Range[excel.Cells[17 + (i * 8)+addline, 1], excel.Cells[17 + (i * 8) + addline, 4]].Font.Bold = true;
                    worksheet.Range[excel.Cells[18 + (i * 8) + addline, 1], excel.Cells[21 + (i * 8) + addline, 1]].Font.Bold = true;                    
                }
            }

            #endregion
            worksheet.Range[excel.Cells[13, 1], excel.Cells[13, 11]].Borders.LineStyle = 1;
            worksheet.Range[excel.Cells[13, 1], excel.Cells[13, 11]].Borders.Weight = 3;
            excel.Cells.EntireColumn.AutoFit();    //自動欄寬
            excel.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save Excel
            excelFile = Sci.Production.Class.MicrosoftFile.GetName("QA_P01_PhysicalInspection");
            excel.ActiveWorkbook.SaveAs(excelFile);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            #endregion

            if (!isSendMail)
            {
                excelFile.OpenFile();
            }
            this.HideWaitMessage();
            return true;
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            
            if (MyUtility.Convert.GetBool(maindr["PhysicalEncode"]) && maindr["Status"].ToString() != "Approved")
            {
                // Excel Email 需寄給Encoder的Teamleader 與 Supervisor*****
                DataRow dr;
                string cmd_leader = $@"
select ToAddress = stuff ((select concat (';', tmp.email)
from (
	select distinct email from pass1
	where id in (select Supervisor from pass1 where  id='{Sci.Env.User.UserID}')
			or id in (select Manager from Pass1 where id = '{Sci.Env.User.UserID}')
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
                    ToExcel(true, "Regular");
                    var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, ccAddress, subject, excelFile, content, false, true);
                    email.ShowDialog(this);
                }
            }

            if (MyUtility.Convert.GetBool(maindr["PhysicalEncode"]) && maindr["Status"].ToString() == "Approved")
            {
                // *****Send Excel Email 完成 需寄給Factory MC*****
                string strToAddress = MyUtility.GetValue.Lookup("ToAddress", "007", "MailTo", "ID");
                string mailto = strToAddress;
                string mailCC = MyUtility.GetValue.Lookup("CCAddress", "007", "MailTo", "ID");
                string subject = string.Format(MyUtility.GetValue.Lookup("Subject", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text);
                string content = string.Format(MyUtility.GetValue.Lookup("content", "007", "MailTo", "ID"), this.displaySP.Text, this.displayBrandRefno.Text, this.displayColor.Text);

                ToExcel(true, "Regular");
                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailCC, subject, excelFile, content, false, true);
                email.ShowDialog(this);

            }
        }
    }
}
