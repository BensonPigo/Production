using System.Linq;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using Ict.Data;
using Ict;
using Sci.Win.Tools;
using Sci.Production.PublicPrg;


namespace Sci.Production.Cutting
{
    public partial class P10_Generate : Sci.Win.Subs.Base
    {
        DataRow maindatarow;
        DataTable allpartTb, artTb, qtyTb, sizeTb, patternTb, detailTb, garmentTb,alltmpTb,bundle_detail_artTb;
        string f_code;
        int NoOfBunble;


        public P10_Generate(DataRow maindr,DataTable table_bundle_Detail,DataTable table_bundleallpart, DataTable table_bundleart, DataTable table_bundleqty)
        {

            string cmd_st = "Select 0 as Sel, PatternCode,PatternDesc, '' as annotation,parts from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out allpartTb);

            string pattern_cmd = "Select patternCode,PatternDesc,Parts,'' as art,0 AS parts from Bundle_Detail WITH (NOLOCK) Where 1=0"; //左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out patternTb);


            string cmd_art = "Select PatternCode,subprocessid from Bundle_detail_art WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_art, out artTb);

            InitializeComponent();

            maindatarow = maindr;
            alltmpTb = table_bundleallpart;
            bundle_detail_artTb = table_bundleart;
            
            qtyTb = table_bundleqty;
            detailTb = table_bundle_Detail;
            numericBox_noBundle.Value = (decimal)maindr["Qty"];
            NoOfBunble = Convert.ToInt16(maindr["Qty"]);
            displayBox_pattern.Text = maindr["PatternPanel"].ToString();
            //calsumQty();
            garmentlist(); //排出所有GarmentList

            int totalCutQty = 0;
            #region Size-CutQty
            if (MyUtility.Check.Empty(maindr["cutref"])) //因為無CutRef 就直接抓取Order_Qty 的SizeCode
            {
                label2.Visible = false;
                displayBox_Cutoutput.Visible = false;
                string size_cmd = string.Format("Select distinct sizecode,0  as Qty from order_Qty WITH (NOLOCK) where id='{0}'", maindr["Orderid"]);
                DualResult dResult = DBProxy.Current.Select(null, size_cmd, out sizeTb);

            }
            else
            {
                string size_cmd = string.Format("Select b.sizecode,isnull(sum(b.Qty),0)  as Qty from Workorder a WITH (NOLOCK) ,Workorder_distribute b WITH (NOLOCK) where a.cutref='{0}' and a.ukey = b.workorderukey and b.orderid='{1}' group by sizeCode", maindr["cutref"], maindr["Orderid"]);
                DualResult dResult = DBProxy.Current.Select(null, size_cmd, out sizeTb);
                if (sizeTb.Rows.Count != 0) totalCutQty = Convert.ToInt16(sizeTb.Compute("Sum(Qty)", ""));
                else
                {
                    size_cmd = string.Format("Select distinct sizecode,0  as Qty from order_Qty WITH (NOLOCK) where id='{0}'", maindr["Orderid"]);
                   dResult = DBProxy.Current.Select(null, size_cmd, out sizeTb);

                }
            }
            displayBox_Cutoutput.Value = totalCutQty;
            #endregion

            #region 將qtyTb資料清空，並將sizeTb的資料複製到qtyTb
            qtyTb.Rows.Clear();
            int i = 1;
            foreach (DataRow dr in sizeTb.Rows)
            {
                DataRow row = qtyTb.NewRow();
                row["No"] = i;
                row["SizeCode"] = dr["SizeCode"];
                row["Qty"] = dr["Qty"];
                qtyTb.Rows.Add(row);
                i++;
            }
            #endregion

            calsumQty();

            if (detailTb.Rows.Count != 0) exist_Table_Query();
            else noexist_Table_Query();

            grid_setup();
            calAllPart();
            caltotalpart();
        }

        public void noexist_Table_Query() //第一次產生時需全部重新撈值
        {
            //找出相同PatternPanel 的subprocessid
            int npart = 0; //allpart 數量
            //DataRow[] garmentar = garmentTb.Select(string.Format("{0} = '{1}'", f_code,maindatarow["Lectracode"]));
            DataRow[] garmentar = garmentTb.Select(string.Format("{0} = '{1}'", f_code, maindatarow["PatternPanel"]));
            foreach (DataRow dr in garmentar)
            {
                if (MyUtility.Check.Empty(dr["annotation"])) //若無ANNOTATion直接寫入All Parts
                {
                    DataRow ndr = allpartTb.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["parts"] = Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                    allpartTb.Rows.Add(ndr);
                    npart = npart+Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                }
                else
                {
                    //Annotation 
                    string[] ann = dr["annotation"].ToString().Split('+'); //剖析Annotation
                    string art = "";
                    #region Annotation有在Subprocess 內需要寫入bundle_detail_art，寫入Bundle_Detail_pattern
                    if (ann.Length > 0)
                    {
                        
                        bool lallpart;
                        #region 算Subprocess
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(),artTb ,out lallpart);
                        #endregion 
                        if (!lallpart)
                        {
                            if (dr["DV"].ToString() != "0" || dr["Pair"].ToString() != "0")
                            {
                                int count = Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                                for (int i = 0; i < count; i++)
                                {
                                    DataRow ndr2 = patternTb.NewRow();
                                    ndr2["PatternCode"] = dr["PatternCode"];
                                    ndr2["PatternDesc"] = dr["PatternDesc"];
                                    ndr2["Parts"] = 1;
                                    ndr2["art"] = art;
                                    patternTb.Rows.Add(ndr2);
                                }
                            }
                            else
                            {
                                DataRow ndr2 = patternTb.NewRow();
                                ndr2["PatternCode"] = dr["PatternCode"];
                                ndr2["PatternDesc"] = dr["PatternDesc"];
                                ndr2["art"] = art;
                                ndr2["Parts"] = dr["alone"];
                                patternTb.Rows.Add(ndr2);
                            }
                        }
                        else
                        {
                            DataRow ndr = allpartTb.NewRow();
                            ndr["PatternCode"] = dr["PatternCode"];
                            ndr["PatternDesc"] = dr["PatternDesc"];
                            ndr["Annotation"] = dr["Annotation"];
                            ndr["parts"] = Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                            npart = npart + Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                            allpartTb.Rows.Add(ndr);
                        }

                    }
                    else
                    {
                        DataRow ndr = allpartTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["PatternDesc"] = dr["PatternDesc"];
                        ndr["Annotation"] = dr["Annotation"];
                        ndr["parts"] = Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                        npart = npart + Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                        allpartTb.Rows.Add(ndr);
                    }
                    #endregion
                }
            }

            DataRow pdr = patternTb.NewRow(); //預設要有ALLPARTS
            pdr["PatternCode"] = "ALLPARTS";
            pdr["PatternDesc"] = "All Parts";
            pdr["parts"] = npart;
            patternTb.Rows.Add(pdr);
            

            #region Create Qtytb
            //268: CUTTING_P10_Generate_Bundle Card Generate
            //distributeQty(Convert.ToInt16(maindatarow["Qty"]));
            #endregion
        }

        public void exist_Table_Query() //當bundle_allPart, bundle_art 存在時的對應資料
        {
            //將Bundle_Detial_Art distinct PatternCode,
            DataTable tmp;
            MyUtility.Tool.ProcessWithDatatable(detailTb,"PatternCode,PatternDesc,parts","Select distinct PatternCode,PatternDesc,Parts from #tmp",out tmp);
            MyUtility.Tool.ProcessWithDatatable(bundle_detail_artTb, "PatternCode,SubProcessid", "Select distinct PatternCode,SubProcessid from #tmp", out artTb);
            foreach (DataRow dr in tmp.Rows)
            {
                DataRow ndr = patternTb.NewRow();
                ndr["PatternCode"] = dr["PatternCode"];
                ndr["PatternDesc"] = dr["PatternDesc"];
                ndr["Parts"] = dr["Parts"];
                string art ="";
                DataRow[] dray = artTb.Select(string.Format("PatternCode = '{0}'", dr["PatternCode"]));
                if (dray.Length!=0)
                {
                    foreach (DataRow dr2 in dray)
                    {
                        if(art != "") art = art+"+"+dr2["Subprocessid"].ToString();
                        else art = dr2["Subprocessid"].ToString();
                    }
                    ndr["art"] = art;
                }
                patternTb.Rows.Add(ndr);
            }

            MyUtility.Tool.ProcessWithDatatable(alltmpTb, "sel,PatternCode,PatternDesc,parts,annotation", "Select distinct sel,PatternCode,PatternDesc,parts,annotation from #tmp", out allpartTb);
            foreach (DataRow dr in allpartTb.Rows)
            {
                DataRow[] adr = garmentTb.Select(string.Format("PatternCode='{0}'",dr["patternCode"]));
                if(adr.Length>0)
                {
                    dr["annotation"] = adr[0]["annotation"];
                }
            }
        }

        public void grid_setup()
        {
            DataGridViewGeneratorNumericColumnSettings qtyCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings subcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell1 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell2 = new DataGridViewGeneratorNumericColumnSettings();

            qtyCell.CellValidating += (s, e) =>
            {
                DataRow dr = grid_qty.GetDataRow(listControlBindingSource1.Position);
                string oldvalue = dr["qty"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["qty"] = newvalue;
                dr.EndEdit();
                calsumQty();
            };
            partsCell1.CellValidating += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calAllPart();
                caltotalpart();
            };
            partsCell2.CellValidating += (s, e) =>
            {
                DataRow dr = grid_allpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calAllPart();
                caltotalpart();
            };
            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;

                    sele = new SelectItem(garmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    dr["PatternDesc"] = (sele.GetSelecteds()[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (sele.GetSelecteds()[0]["PatternCode"]).ToString();
                    string[] ann = (sele.GetSelecteds()[0]["Annotation"]).ToString().Split('+'); //剖析Annotation
                    string art = "";
                    bool lallpart;
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), artTb, out lallpart);
                    }
                    #endregion 
                    dr["art"] = art;
                    dr["parts"] = 1;
                    dr.EndEdit();
                    calAllPart();
                    caltotalpart();
                }
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                string oldvalue = dr["PatternCode"].ToString();
                if (oldvalue == patcode) return;
                DataRow[] gemdr = garmentTb.Select(string.Format("PatternCode ='{0}'", patcode),"");
                if (gemdr.Length > 0)
                {
                    dr["PatternDesc"] = (gemdr[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (gemdr[0]["PatternCode"]).ToString();
                    string[] ann = (gemdr[0]["Annotation"]).ToString().Split('+'); //剖析Annotation
                    string art = "";
                    bool lallpart;
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), artTb, out lallpart);
                    }
                    #endregion
                    dr["art"] = art;
                    dr["parts"] = 1;

                }
                dr.EndEdit(); 
            };
            patterncell2.EditingMouseDown += (s, e) =>
            {
                DataRow dr = grid_allpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {
                    
                    SelectItem sele;

                    sele = new SelectItem(garmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    dr["PatternDesc"] = (sele.GetSelecteds()[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (sele.GetSelecteds()[0]["PatternCode"]).ToString();
                    dr["Annotation"] = (sele.GetSelecteds()[0]["Annotation"]).ToString();
                    dr["parts"] = 1;
                    dr.EndEdit();
                    calAllPart();
                    caltotalpart();
                }
            };
            
            subcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem2 sele;
                    sele = new SelectItem2("Select id from subprocess WITH (NOLOCK) where junk=0 and IsRfidProcess=1", "Subprocess", "23", dr["PatternCode"].ToString());
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    string subpro = sele.GetSelectedString().Replace(",","+");

                    e.EditingControl.Text = subpro;
                    dr["art"] = subpro;
                    dr.EndEdit();
                    DataRow[] artdr = artTb.Select(string.Format("PatternCode='{0}'", dr["PatternCode"]));
                    foreach (DataRow adr in artdr)
                    {
                        adr.Delete();
                    }
                    foreach(DataRow dt in sele.GetSelecteds())
                    {
                        DataRow ndr = artTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["subprocessid"] = dt["id"];
                        artTb.Rows.Add(ndr);
                    }
                }
            };

            listControlBindingSource1.DataSource = qtyTb;
            grid_qty.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid_qty)
            .Numeric("No", header: "No", width: Widths.AnsiChars(4), integer_places: 5, iseditingreadonly: true)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 5, settings: qtyCell);
            grid_qty.Columns[2].DefaultCellStyle.BackColor = Color.Pink;

            grid_art.DataSource = patternTb;
            grid_art.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid_art)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15))
            .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true,settings: subcell)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partsCell1);
            grid_art.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            grid_art.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            grid_art.Columns[2].DefaultCellStyle.BackColor = Color.Pink;
            grid_art.Columns[3].DefaultCellStyle.BackColor = Color.Pink;

            grid_allpart.DataSource = allpartTb;
            this.grid_allpart.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(this.grid_allpart)
            .CheckBox("Sel", header: "Chk", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10),settings:patterncell2)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13))
            .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partsCell2);
            grid_allpart.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            grid_allpart.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            grid_allpart.Columns[2].DefaultCellStyle.BackColor = Color.Pink;
            grid_allpart.Columns[4].DefaultCellStyle.BackColor = Color.Pink;


            grid_Size.DataSource = sizeTb;
            grid_Size.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid_Size)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8))
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void distributeQty(int Qtycount)
        {
            #region Qty 的筆數分配
            //輸入的數量必須超過[No of Bundle]
            if (Qtycount <= NoOfBunble)
            {
                MyUtility.Msg.WarningBox(string.Format("[No of Bundle] must exceed {0} !!", NoOfBunble));
                return;
            }

            int rowindex = grid_qty.CurrentRow.Index;
            string SizeCode = grid_qty.Rows[rowindex].Cells["SizeCode"].Value.ToString();
            int NowCount = qtyTb.Select(string.Format("SizeCode='{0}'", SizeCode)).Length;  //現在有幾筆
            int BeforeAddCount = NowCount - 1;  //之前新增筆數
            int AddCount = Qtycount - NoOfBunble;  //想要新增幾筆  6-4=2
            int Modify = AddCount - BeforeAddCount;
            if (Modify > 0)  //新增
            {
                for (int i = 0; i < Modify; i++)
                {
                    DataRow ndr = qtyTb.NewRow();
                    ndr["SizeCode"] = SizeCode;
                    qtyTb.Rows.Add(ndr);
                }
            }
            else if (Modify < 0)  //刪除(從後面)
            {
                for (int i = qtyTb.Rows.Count-1; i > 0; i--)
                {
                    if (Modify >= 0) break;
                    if (qtyTb.Rows[i]["SizeCode"].ToString() == SizeCode)
                    {
                        qtyTb.Rows[i].Delete();
                        Modify++;
                    } 
                }
            }

            //賦予流水號
            int serial = 1;
            foreach (DataRow dr in qtyTb.Rows)
            {
                dr["No"] = serial;
                serial++;
            }



            //int i = 1;
            //foreach (DataRow dr in sizeTb.Rows) //依照Size產生
            //{
            //    int rowcount = qtyTb.Select(string.Format("SizeCode='{0}'", dr["SizeCode"]), "").Length;

            //    if (rowcount <= Qtycount) //缺少需先新增
            //    {
            //        for (int j = 0; j < Qtycount - rowcount; j++)
            //        {
            //            DataRow ndr = qtyTb.NewRow();
            //            ndr["SizeCode"] = dr["SizeCode"];
            //            qtyTb.Rows.Add(ndr);
            //        }
            //    }
            //    i = 1;
            //    DataRow[] qtyArr = qtyTb.Select(string.Format("SizeCode='{0}'", dr["SizeCode"]), ""); //重新撈取
            //    foreach (DataRow dr2 in qtyArr)
            //    {
            //        if (dr2.RowState != DataRowState.Deleted)
            //        {
            //            dr2["No"] = i;
            //        }
            //        if (i > Qtycount) dr2.Delete(); //多餘的筆數要刪掉

            //        i++;
            //    }
            //}
            
            calQty();

            #endregion
        }

        private void numericBox_noBundle_Validated(object sender, EventArgs e)
        {
            int newvalue = (int)numericBox_noBundle.Value;
            int oldvalue = (int)numericBox_noBundle.OldValue;
            //if (newvalue == oldvalue) return;
            distributeQty(newvalue);
        }

        public void calQty() //分配Qty
        {
            foreach (DataRow dr in sizeTb.Rows)
            {
                Double TotalCutQty = Convert.ToDouble(dr["Qty"]);
                DataRow[] qtyarry = qtyTb.Select(string.Format("SizeCode='{0}'", dr["SizeCode"]), "");
                Double rowcount = qtyarry.Length;
                if (TotalCutQty % rowcount == 0)
                {
                    int qty = (int)(TotalCutQty / rowcount); //每一個數量是多少
                    foreach (DataRow dr2 in qtyarry)
                    {
                        dr2["Qty"] = qty;
                    }
                }
                else
                {
                    int eachqty = (int)(Math.Floor(TotalCutQty / rowcount));
                    int modqty = (int)(TotalCutQty % rowcount); //剩餘數

                    foreach (DataRow dr2 in qtyarry)
                    {
                        if (eachqty != 0)
                        {
                            if (modqty > 0) dr2["Qty"] = eachqty + 1;//每組分配一個Qty 當分配完表示沒了
                            else dr2["Qty"] = eachqty;
                            modqty--; //剩餘數一定小於rowcount所以會有筆數沒有拿到
                        }
                        else
                        {
                            if (modqty > 0)
                            {
                                dr2["Qty"] = 1;
                                modqty--;
                            }
                            else dr2.Delete();
                        }
                    }
                    
                }
            }
            calsumQty();
        }

        public void calsumQty()
        {
            int qty=0;
            if(qtyTb.Rows.Count>0) qty = Convert.ToInt16(qtyTb.Compute("sum(Qty)", ""));
            displayBox1.Value = qty;
        }

        private void button_Qty_Click(object sender, EventArgs e)
        {
            DataRow selectSizeDr = ((DataRowView)grid_Size.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow selectQtyeDr = ((DataRowView)grid_qty.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectQtyeDr["SizeCode"] = selectSizeDr["SizeCode"];
            qtyTb.DefaultView.Sort="SizeCode,No";
            foreach(DataRow dr in sizeTb.Rows)
            {
                int i = 1;
                DataRow[] qtyArr = qtyTb.Select(string.Format("SizeCode='{0}'", dr["SizeCode"]), ""); //重新撈取
                foreach (DataRow dr2 in qtyArr)
                {
                    dr2["No"] = i;
                    i++;
                }
            }
            calQty();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", maindatarow["poid"].ToString(), "Orders", "ID");
            Sci.Production.PublicForm.GarmentList callNextForm = new Sci.Production.PublicForm.GarmentList(ukey);
            callNextForm.ShowDialog(this);
        }

        public void garmentlist() //產生GarmentList Table   garmentTb
        {
            PublicPrg.Prgs.GetGarmentListTable(maindatarow["poid"].ToString(), out garmentTb);
            
            #region 撈取Pattern Ukey  找最晚Edit且Status 為Completed
            string Styleyukey = MyUtility.GetValue.Lookup("Styleukey", maindatarow["poid"].ToString(), "Orders", "ID");
            string patidsql = String.Format(
                            @"SELECT ukey
                              FROM [Production].[dbo].[Pattern] WITH (NOLOCK) 
                              WHERE STYLEUKEY = '{0}'  and Status = 'Completed' 
                              AND EDITdATE = 
                              (
                                SELECT MAX(EditDate) 
                                from pattern WITH (NOLOCK) 
                                where styleukey = '{0}' and Status = 'Completed'
                              )
             ", Styleyukey);
           string patternukey = MyUtility.GetValue.Lookup(patidsql);
           #endregion
           string sqlcmd = String.Format(
            @"Select a.ArticleGroup
            from Pattern_GL_Article a WITH (NOLOCK) 
            Where a.PatternUkey = '{0}' and article = '{1}'", patternukey,maindatarow["Article"]);
            f_code = MyUtility.GetValue.Lookup(sqlcmd, null);
            if (f_code == "") f_code = "F_Code";
        }

        private void button_LefttoRight_Click(object sender, EventArgs e)
        {
            grid_allpart.ValidateControl();
            grid_art.ValidateControl();
            grid_qty.ValidateControl();
            DataRow selectartDr = ((DataRowView)grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            string pattern = selectartDr["PatternCode"].ToString();
            if (pattern == "ALLPARTS") return;
            string art = selectartDr["art"].ToString();
            //移動此筆
            DataRow ndr = allpartTb.NewRow();
            ndr["PatternCode"] = selectartDr["PatternCode"];
            ndr["PatternDesc"] = selectartDr["PatternDesc"];
            ndr["Parts"] = selectartDr["Parts"];
            //Annotation
            DataRow[] adr = garmentTb.Select(string.Format("PatternCode='{0}'", selectartDr["patternCode"]));
            if (adr.Length > 0)
            {
                ndr["annotation"] = adr[0]["annotation"];
            }
            allpartTb.Rows.Add(ndr);
            selectartDr.Delete(); //刪除此筆

            DataRow[] patterndr = patternTb.Select(string.Format("PatternCode='{0}'", pattern));
            DataRow[] artdr = artTb.Select(string.Format("PatternCode='{0}'", pattern));
            if (patterndr.Length > 0) //刪除後還有相同Pattern 需要判斷是否Subprocess都存在
            {

                foreach (DataRow dr in patterndr)
                {
                    if (artdr.Length > 0)
                    {
                        foreach (DataRow dr2 in artdr)
                        {
                            if (dr["art"].ToString().IndexOf(dr2["subprocessid"].ToString()) == -1) dr2.Delete();
                        }
                    }
                }
            }
            else
            { //直接刪除全部同PatternCode 的Subprocessid
                foreach (DataRow dr in patterndr)
                {
                    dr.Delete();
                }
            }
            calAllPart();
            caltotalpart();
        }

        private void button_RighttoLeft_Click(object sender, EventArgs e)
        {
            grid_allpart.ValidateControl();
            grid_art.ValidateControl();
            grid_qty.ValidateControl();
            if (patternTb.Rows.Count == 0) return;
            if (grid_allpart.RowCount == 0) return;
            DataRow selectartDr = ((DataRowView)grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow selectallparteDr = ((DataRowView)grid_allpart.GetSelecteds(SelectedSort.Index)[0]).Row;

            DataRow[] checkdr = allpartTb.Select("sel=1");
            #region 確認有勾選
            if (checkdr.Length > 0)
            {
                foreach (DataRow chdr in checkdr)
                {
                    string art = "";
                    string[] ann = chdr["annotation"].ToString().Split('+'); //剖析Annotation
                    if (ann.Length > 0)
                    {
                        bool lallpart;
                        #region 算Subprocess
                        art = Prgs.BundleCardCheckSubprocess(ann, chdr["PatternCode"].ToString(), artTb, out lallpart);
                        #endregion 
                    }
                    //新增PatternTb
                    DataRow ndr2 = patternTb.NewRow();
                    ndr2["PatternCode"] = chdr["PatternCode"];
                    ndr2["PatternDesc"] = chdr["PatternDesc"];
                    ndr2["Parts"] = chdr["Parts"]; ;
                    ndr2["art"] = art;
                    patternTb.Rows.Add(ndr2);
                    chdr.Delete(); //刪除
                }
            }
            else if (checkdr.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data !!");
            }
            #endregion
            calAllPart();
            caltotalpart();
        }

        public void caltotalpart() //計算total part
        {
            decimal totalpart = 0;
            if (patternTb.Rows.Count > 0) totalpart = Convert.ToDecimal(patternTb.Compute("Sum(Parts)", ""));
            totalpart_numericBox.Value = totalpart;
        }

        public void calAllPart() //計算all part
        {
            int allpart = 0;
            if (allpartTb.AsEnumerable().Count(row => row.RowState != DataRowState.Deleted) > 0)
                allpart = allpartTb.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted)
                    .Sum(row => Convert.ToInt16(row["Parts"]));
            DataRow[] dr = patternTb.Select("PatternCode='ALLPARTS'");
            if (dr.Length > 0)  dr[0]["Parts"] = allpart;
        }

        private void insertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = patternTb.NewRow();
            patternTb.Rows.Add();
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow selectartDr = ((DataRowView)grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            if (selectartDr["PatternCode"].ToString() == "ALLPARTS")
            {
                MyUtility.Msg.WarningBox("Please remove all right grid's parts to instead of removeing ALLPARTS directly!");
                return;
            }
            selectartDr.Delete();
            caltotalpart();
        }

        private void allpart_insert_Click(object sender, EventArgs e)
        {
            allpartTb.NewRow();
            allpartTb.Rows.Add();
        }

        private void allpart_delete_Click(object sender, EventArgs e)
        {
            DataRow selectartDr = ((DataRowView)grid_allpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectartDr.Delete();
            calAllPart(); 
            caltotalpart();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            DataTable at = artTb.Copy();
            #region 判斷Pattern的Artwork  不可為空
            DataRow[] findr= patternTb.Select("PatternCode<>'ALLPARTS' and (art='' or art is null)","");
            if(findr.Length>0)
            {
                MyUtility.Msg.WarningBox("<Art> can not be empty!");
                return;
            }
            #endregion 

            maindatarow["Qty"] = numericBox_noBundle.Value;
            DataTable bundle_detail_tmp;
            DBProxy.Current.Select(null, "Select *,0 as ukey1,'' as subprocessid from bundle_Detail WITH (NOLOCK) where 1=0", out bundle_detail_tmp);
            int bundlegroup = Convert.ToInt16(maindatarow["startno"]);
            int ukey = 1;
            foreach (DataRow dr in qtyTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    foreach (DataRow dr2 in patternTb.Rows)
                    {
                        if (Convert.ToInt16(dr2["Parts"]) == 0) continue;  //若Parts=0，則不需產生資料至Bundle card明細
                        DataRow nDetail = bundle_detail_tmp.NewRow();
                        nDetail["PatternCode"] = dr2["PatternCode"];
                        nDetail["PatternDesc"] = dr2["PatternDesc"];
                        nDetail["Parts"] = dr2["Parts"];
                        nDetail["Qty"] = dr["Qty"];
                        nDetail["SizeCode"] = dr["SizeCode"];
                        nDetail["bundlegroup"] = bundlegroup;
                        nDetail["ukey1"] = ukey;
                        
                        if (dr2["PatternCode"].ToString() != "ALLPARTS") //為了跟外面相同多加一個"+"
                        {
                            nDetail["subprocessid"] = dr2["art"].ToString()+"+";
                        }
                        ukey++;

                        bundle_detail_tmp.Rows.Add(nDetail);
                    }
                    bundlegroup++;
                }
                
            }
            alltmpTb.Clear();
            bundle_detail_artTb.Clear();
            //平行覆蓋資料
            int j = 0;
            int detailRow = 0;
            int tmpRow = bundle_detail_tmp.Rows.Count;
            foreach (DataRow dr in detailTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    
                    if (j < tmpRow)
                    {
                        DataRow tmpdr = bundle_detail_tmp.Rows[j];
                        dr["bundlegroup"] = tmpdr["bundlegroup"];
                        dr["PatternCode"] = tmpdr["PatternCode"];
                        dr["PatternDesc"] = tmpdr["PatternDesc"];
                        dr["subprocessid"] = tmpdr["subprocessid"];
                        dr["Parts"] = tmpdr["Parts"];
                        dr["Qty"] = tmpdr["Qty"];
                        dr["SizeCode"] = tmpdr["SizeCode"];
                        dr["ukey1"] = tmpdr["ukey1"];
                        j++;
                        if (tmpdr["PatternCode"].ToString() == "ALLPARTS")
                        {

                            #region 讓Bundle_Detail_Allpart只產生一份資料
                            if (allpartTb.Rows.Count > alltmpTb.Rows.Count)
                            {
                                foreach (DataRow aldr in allpartTb.Rows)
                                {
                                    if (aldr.RowState != DataRowState.Deleted)
                                    {
                                        DataRow allpart_ndr = alltmpTb.NewRow();
                                        //allpart_ndr["Bundleno"] = dr["Bundleno"];
                                        allpart_ndr["PatternCode"] = aldr["PatternCode"];
                                        allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                                        allpart_ndr["Parts"] = aldr["Parts"];
                                        allpart_ndr["ukey1"] = dr["ukey1"];
                                        alltmpTb.Rows.Add(allpart_ndr);
                                    }
                                }
                            }
                            #endregion

                        }
                        DataRow[] artary = artTb.Select(string.Format("PatternCode='{0}'", tmpdr["PatternCode"]));
                        foreach (DataRow artdr in artary)
                        {
                            if (artdr.RowState != DataRowState.Deleted)
                            {
                                DataRow art_ndr = bundle_detail_artTb.NewRow();
                                art_ndr["Bundleno"] = dr["Bundleno"];
                                art_ndr["PatternCode"] = artdr["PatternCode"];
                                art_ndr["Subprocessid"] = artdr["Subprocessid"];
                                art_ndr["ukey1"] = dr["ukey1"];
                                bundle_detail_artTb.Rows.Add(art_ndr);
                            }
                        }
                    }
                }
                detailRow++;
            }
            if (tmpRow < detailTb.Rows.Count) //當舊的比較多就要刪除
            {
                int detailrow = detailTb.Rows.Count;
                for (int i = detailrow - 1; i >= tmpRow; i--) //因為delete時Rowcount 會改變所以要重後面往前刪
                {
                     detailTb.Rows[i].Delete();
                }
            }
            if (tmpRow > j) //表示新增的比較多需要Insert
            {
                for (int i = 0; i < tmpRow - j; i++)
                {
                    DataRow ndr = detailTb.NewRow();
                    DataRow tmpdr = bundle_detail_tmp.Rows[j + i];
                    ndr["bundlegroup"] = tmpdr["bundlegroup"];
                    ndr["PatternCode"] = tmpdr["PatternCode"];
                    ndr["PatternDesc"] = tmpdr["PatternDesc"];
                    ndr["subprocessid"] = tmpdr["subprocessid"];
                    ndr["Parts"] = tmpdr["Parts"];
                    ndr["Qty"] = tmpdr["Qty"];
                    ndr["SizeCode"] = tmpdr["SizeCode"];
                    ndr["ukey1"] = tmpdr["ukey1"];
                    detailTb.Rows.Add(ndr);
                    if (tmpdr["PatternCode"].ToString() == "ALLPARTS")
                    {

                        #region 讓Bundle_Detail_Allpart只產生一份資料
                        if (allpartTb.Rows.Count > alltmpTb.Rows.Count)
                        {
                            foreach (DataRow aldr in allpartTb.Rows)
                            {
                                if (aldr.RowState != DataRowState.Deleted)
                                {
                                    DataRow allpart_ndr = alltmpTb.NewRow();

                                    allpart_ndr["PatternCode"] = aldr["PatternCode"];
                                    allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                                    allpart_ndr["Parts"] = aldr["Parts"];
                                    allpart_ndr["ukey1"] = tmpdr["ukey1"];
                                    alltmpTb.Rows.Add(allpart_ndr);
                                }
                            }
                        }
                        #endregion

                    }
                    DataRow[] artary = artTb.Select(string.Format("PatternCode='{0}'", tmpdr["PatternCode"]));
                    foreach (DataRow artdr in artary)
                    {
                        if (artdr.RowState != DataRowState.Deleted)
                        {
                            DataRow art_ndr = bundle_detail_artTb.NewRow();

                            art_ndr["PatternCode"] = artdr["PatternCode"];
                            art_ndr["Subprocessid"] = artdr["Subprocessid"];
                            art_ndr["ukey1"] = tmpdr["ukey1"];
                            bundle_detail_artTb.Rows.Add(art_ndr);
                        }
                    }
                }
            }
            this.listControlBindingSource1.DataSource = null;
            this.Close();
        }

        private void grid_Size_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button_Qty_Click(sender, e);
        }

    }
}
