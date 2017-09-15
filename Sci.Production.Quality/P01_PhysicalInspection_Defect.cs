﻿using Ict;
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

namespace Sci.Production.Quality
{
    public partial class P01_PhysicalInspection_Defect : Sci.Win.Subs.Input6A
    {
        DataTable DefectTb, DefectFilterTb;
        private DataTable gridTb;
        public DataRow mainrow;
        
        public P01_PhysicalInspection_Defect(DataTable defectTb,DataRow maindr)
        {
            InitializeComponent();
            DefectTb = defectTb;
            mainrow = maindr;

        }

        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);
            DataView dv = DefectTb.DefaultView;
            dv.RowFilter = string.Format("NewKey = {0}", data["NewKey"]);
            DefectFilterTb = dv.ToTable();
            string cmd = string.Format("Select '' AS yds1, '' AS def1, 0 as point1,'' AS yds2,'' AS def2, 0 as point2,'' AS yds3,'' AS def3, 0 as point3");
            DBProxy.Current.Select(null, cmd, out gridTb);
            gridTb.Clear();
            #region 計算碼長分配入TABLE
            Double actyds = MyUtility.Convert.GetDouble(data["ActualYds"]);
            string cStr = "";
            int j = 1;
            for (int i = 0; i < actyds; i = i + 5) //每5碼為一組
            {
                if (i + 5 >= actyds) //最後超過需-0.01的整數當做最後碼長
                {
                    cStr = MyUtility.Convert.NTOC(i, 3) + "-" + MyUtility.Convert.NTOC(MyUtility.Convert.GetInt(Math.Floor(actyds - 0.01)), 3);
                }
                else
                {
                    cStr = MyUtility.Convert.NTOC(i, 3) + "-" + MyUtility.Convert.NTOC(i + 4, 3);
                }
                DataRow[] Ary = DefectFilterTb.Select(string.Format("DefectLocation ='{0}'", cStr));
                //將存在DefectFilterTb的資料填入Grid
                #region 填入對的位置 % 去找位置
                if (j % 3 == 1) //新增一筆從頭開始
                {
                    DataRow ndr = gridTb.NewRow();
                    ndr["yds1"] = cStr;
                    if (Ary.Length > 0)
                    {
                        ndr["def1"] = Ary[0]["DefectRecord"];
                        ndr["point1"] = Ary[0]["point"];
                    }
                    gridTb.Rows.Add(ndr);
                }
                int rowIndex = (int)Math.Floor((decimal)(j / 3));

                if (j % 3 == 2)
                {
                    DataRow dr = gridTb.Rows[rowIndex];
                    dr["yds2"] = cStr;
                    if (Ary.Length > 0)
                    {
                        dr["def2"] = Ary[0]["DefectRecord"];
                        dr["point2"] = Ary[0]["point"];
                    }
                }
                if (j % 3 == 0)
                {
                    DataRow dr = gridTb.Rows[rowIndex - 1];
                    dr["yds3"] = cStr;
                    if (Ary.Length > 0)
                    {
                        dr["def3"] = Ary[0]["DefectRecord"];
                        dr["point3"] = Ary[0]["point"];
                    }
                }
                #endregion
                j++;
            }

            #endregion
            gridFabricInspection.DataSource = gridTb;

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            GridSetUp();
 
        }

        public void GridSetUp()
        {

            DataGridViewGeneratorTextColumnSettings DefectsCell1 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings DefectsCell2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings DefectsCell3 = new DataGridViewGeneratorTextColumnSettings();
            DefectsCell1.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1 || !this.EditMode) return;
                
                DataRow dr = gridFabricInspection.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["yds1"])) return;
                var frm = new Sci.Production.Quality.P01_PhysicalInspection_PointRecord(dr,"1");
                frm.ShowDialog(this);
            };
            DefectsCell2.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1 || !this.EditMode) return;
                DataRow dr = gridFabricInspection.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["yds2"])) return;
                var frm = new Sci.Production.Quality.P01_PhysicalInspection_PointRecord(dr,"2");
                frm.ShowDialog(this);
            };
            DefectsCell3.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1 || !this.EditMode) return;
                DataRow dr = gridFabricInspection.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["yds3"])) return;
                var frm = new Sci.Production.Quality.P01_PhysicalInspection_PointRecord(dr,"3");
                frm.ShowDialog(this);
            };
             Helper.Controls.Grid.Generator(gridFabricInspection)
                 .Text("yds1", header: "Yds", width: Widths.AnsiChars(7),iseditingreadonly: true)
                 .Text("def1", header: "Defects", width: Widths.AnsiChars(15), settings: DefectsCell1)
                 .Text("yds2", header: "Yds", width: Widths.AnsiChars(8),iseditingreadonly: true)
                 .Text("def2", header: "Defects", width: Widths.AnsiChars(15), settings: DefectsCell2)
                 .Text("yds3", header: "Yds", width: Widths.AnsiChars(8),iseditingreadonly: true)
                 .Text("def3", header: "Defects", width: Widths.AnsiChars(15), settings: DefectsCell3);
        }

        protected override bool DoSave()
        {

            foreach (DataRow dr in gridTb.Rows)
            {
                #region 回填回Fir_Physical_Defect
                for (int i = 1; i <= 3; i++) //有3個yds,def 所以直接用Forloop 
                {
                    string str_col = i.ToString();
                    DataRow[] Ary = DefectTb.Select(string.Format("NewKey = {0} and DefectLocation = '{1}'", CurrentData["NewKey"], dr["yds" + str_col]));
                    if (MyUtility.Check.Empty(dr["def" + str_col]))
                    {
                        //找出Table是否有存在的Location
                        if (Ary.Length > 0) Ary[0].Delete();//原本有值現在為空需要刪除資料
                    }
                    else
                    {
                        if (Ary.Length > 0)
                        {
                            Ary[0]["DefectRecord"] = dr["def" + str_col];
                            Ary[0]["Point"] = dr["point" + str_col];
                        }
                        else //需新增
                        {
                            DataRow ndr = DefectTb.NewRow();
                            ndr["ID"] = CurrentData["ID"];
                            ndr["NewKey"] = CurrentData["NewKey"];
                            ndr["Fir_PhysicalDetailUkey"] = CurrentData["DetailUkey"];
                            ndr["DefectLocation"] = dr["yds" + str_col];
                            ndr["DefectRecord"] = dr["def" + str_col];
                            ndr["Point"] = dr["point" + str_col];
                            DefectTb.Rows.Add(ndr);
                        }
                    }
                }
                #endregion
            }
//            Double SumPoint = MyUtility.Convert.GetDouble(DefectTb.Compute("Sum(Point)", string.Format("NewKey = {0}", CurrentData["NewKey"])));
//            //PointRate 國際公式每五碼最高20點
//            CurrentData["TotalPoint"] = SumPoint;
//            double double_ActualYds = MyUtility.Convert.GetDouble(CurrentData["ActualYds"]);
//            CurrentData["PointRate"] = (double_ActualYds == 0) ? 0 : Math.Round((SumPoint / double_ActualYds) * 100, 2);
//            #region Grade,Result
//            string WeaveTypeid = MyUtility.GetValue.Lookup("WeaveTypeId", mainrow["SCiRefno"].ToString(), "Fabric", "SciRefno");
//            string grade_cmd = String.Format(@"
//SELECT MIN(GRADE) grade 
//FROM FIR_Grade WITH (NOLOCK) 
//WHERE   WEAVETYPEID = '{0}' 
//        AND PERCENTAGE >= IIF({1} > 100, 100, {1})", WeaveTypeid, CurrentData["PointRate"]);
//            DataRow grade_dr;
//            if (MyUtility.Check.Seek(grade_cmd, out grade_dr))
//            {
//                CurrentData["Grade"] = grade_dr["grade"];
//                CurrentData["Result"] = MyUtility.GetValue.Lookup(string.Format(@"
//Select	[Result] =	case Result	
//						when 'P' then 'Pass'
//						when 'F' then 'Fail'
//					end
//from Fir_Grade WITH (NOLOCK) 
//where	WEAVETYPEID = '{0}' 
//		and Grade = '{1}'", WeaveTypeid, grade_dr["grade"]), null);
//            }
//            #endregion
            return base.DoSave();
        }
    }
}
