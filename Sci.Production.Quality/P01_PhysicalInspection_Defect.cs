using Ict;
using Ict.Win;
using System;
using System.Data;
using Sci.Data;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_PhysicalInspection_Defect : Win.Subs.Input6A
    {
        private readonly DataTable DefectTb;
        private readonly bool editm = false;
        private DataTable DefectFilterTb;
        private DataTable gridTb;
        private DataRow mainrow;

        /// <inheritdoc/>
        public P01_PhysicalInspection_Defect(DataTable defectTb, DataRow maindr, bool edit)
        {
            this.InitializeComponent();
            this.DefectTb = defectTb;
            this.mainrow = maindr;
            this.editm = edit;
        }

        /// <inheritdoc/>
        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);
            DataView dv = this.DefectTb.DefaultView;
            dv.RowFilter = string.Format("NewKey = {0}", data["NewKey"]);
            this.DefectFilterTb = dv.ToTable();
            this.DefectFilterTb.ColumnsIntAdd("DefectLocationF");
            this.DefectFilterTb.ColumnsIntAdd("DefectLocationT");

            for (int i = 0; i < this.DefectFilterTb.Rows.Count; i++)
            {
                this.DefectFilterTb.Rows[i]["DefectLocationF"] = this.DefectFilterTb.Rows[i]["DefectLocation"].ToString().Split('-')[0];
                this.DefectFilterTb.Rows[i]["DefectLocationT"] = this.DefectFilterTb.Rows[i]["DefectLocation"].ToString().Split('-')[1];
            }

            string cmd = string.Format("Select '' AS yds1, '' AS def1, 0 as point1,'' AS yds2,'' AS def2, 0 as point2,'' AS yds3,'' AS def3, 0 as point3");
            DBProxy.Current.Select(null, cmd, out this.gridTb);
            this.gridTb.Clear();
            #region 計算碼長分配入TABLE
            double actyds = MyUtility.Convert.GetDouble(data["ActualYds"]);
            string cStr = string.Empty;
            int j = 1;

            // 每5碼為一組
            for (int i = 0; i < actyds; i = i + 5)
            {
                // 最後超過需-0.01的整數當做最後碼長
                if (i + 5 >= actyds)
                {
                    cStr = MyUtility.Convert.NTOC(i, 3) + "-" + MyUtility.Convert.NTOC(MyUtility.Convert.GetInt(Math.Floor(actyds - 0.01)), 3);
                }
                else
                {
                    cStr = MyUtility.Convert.NTOC(i, 3) + "-" + MyUtility.Convert.NTOC(i + 4, 3);
                }

                DataRow[] ary = this.DefectFilterTb.Select(string.Format("DefectLocationF >= {0} and DefectLocationT <= {1}", Convert.ToInt32(cStr.Split('-')[0]), Convert.ToInt32(cStr.Split('-')[1])));

                // 將存在DefectFilterTb的資料填入Grid
                #region 填入對的位置 % 去找位置

                // 新增一筆從頭開始
                if (j % 3 == 1)
                {
                    DataRow ndr = this.gridTb.NewRow();
                    ndr["yds1"] = cStr;
                    if (ary.Length > 0)
                    {
                        ndr["def1"] = ary[0]["DefectRecord"];
                        ndr["point1"] = ary[0]["point"];
                    }

                    this.gridTb.Rows.Add(ndr);
                }

                int rowIndex = (int)Math.Floor((decimal)(j / 3));

                if (j % 3 == 2)
                {
                    DataRow dr = this.gridTb.Rows[rowIndex];
                    dr["yds2"] = cStr;
                    if (ary.Length > 0)
                    {
                        dr["def2"] = ary[0]["DefectRecord"];
                        dr["point2"] = ary[0]["point"];
                    }
                }

                if (j % 3 == 0)
                {
                    DataRow dr = this.gridTb.Rows[rowIndex - 1];
                    dr["yds3"] = cStr;
                    if (ary.Length > 0)
                    {
                        dr["def3"] = ary[0]["DefectRecord"];
                        dr["point3"] = ary[0]["point"];
                    }
                }
                #endregion
                j++;
            }

            #endregion
            this.gridFabricInspection.DataSource = this.gridTb;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetUp();
        }

        /// <inheritdoc/>
        public void GridSetUp()
        {
            DataGridViewGeneratorTextColumnSettings defectsCell1 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings defectsCell2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings defectsCell3 = new DataGridViewGeneratorTextColumnSettings();
            defectsCell1.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridFabricInspection.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["yds1"]))
                {
                    return;
                }

                var frm = new P01_PhysicalInspection_PointRecord(dr, "1", this.editm);
                frm.ShowDialog(this);
            };
            defectsCell2.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridFabricInspection.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["yds2"]))
                {
                    return;
                }

                var frm = new P01_PhysicalInspection_PointRecord(dr, "2", this.editm);
                frm.ShowDialog(this);
            };
            defectsCell3.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridFabricInspection.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["yds3"]))
                {
                    return;
                }

                var frm = new P01_PhysicalInspection_PointRecord(dr, "3", this.editm);
                frm.ShowDialog(this);
            };
            this.Helper.Controls.Grid.Generator(this.gridFabricInspection)
                 .Text("yds1", header: "Yds", width: Widths.AnsiChars(7), iseditingreadonly: true)
                 .Text("def1", header: "Defects", width: Widths.AnsiChars(15), settings: defectsCell1)
                 .Text("yds2", header: "Yds", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("def2", header: "Defects", width: Widths.AnsiChars(15), settings: defectsCell2)
                 .Text("yds3", header: "Yds", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("def3", header: "Defects", width: Widths.AnsiChars(15), settings: defectsCell3);
        }

        /// <inheritdoc/>
        protected override bool DoSave()
        {
            foreach (DataRow dr in this.gridTb.Rows)
            {
                #region 回填回Fir_Physical_Defect
                for (int i = 1; i <= 3; i++)
                {
                    // 有3個yds,def 所以直接用Forloop
                    string str_col = i.ToString();
                    DataRow[] ary = this.DefectTb.Select(string.Format("NewKey = {0} and DefectLocation = '{1}'", this.CurrentData["NewKey"], dr["yds" + str_col]));
                    if (MyUtility.Check.Empty(dr["def" + str_col]))
                    {
                        // 找出Table是否有存在的Location
                        if (ary.Length > 0)
                        {
                            ary[0].Delete(); // 原本有值現在為空需要刪除資料
                        }
                    }
                    else
                    {
                        if (ary.Length > 0)
                        {
                            ary[0]["DefectRecord"] = dr["def" + str_col];
                            ary[0]["Point"] = dr["point" + str_col];
                        }
                        else
                        {
                            // 需新增
                            DataRow ndr = this.DefectTb.NewRow();
                            ndr["ID"] = this.CurrentData["ID"];
                            ndr["NewKey"] = this.CurrentData["NewKey"];
                            ndr["Fir_PhysicalDetailUkey"] = this.CurrentData["DetailUkey"];
                            ndr["DefectLocation"] = dr["yds" + str_col];
                            ndr["DefectRecord"] = dr["def" + str_col];
                            ndr["Point"] = dr["point" + str_col];
                            this.DefectTb.Rows.Add(ndr);
                        }
                    }
                }
                #endregion
            }

// Double SumPoint = MyUtility.Convert.GetDouble(DefectTb.Compute("Sum(Point)", string.Format("NewKey = {0}", CurrentData["NewKey"])));
//            //PointRate 國際公式每五碼最高20點
//            CurrentData["TotalPoint"] = SumPoint;
//            double double_ActualYds = MyUtility.Convert.GetDouble(CurrentData["ActualYds"]);
//            CurrentData["PointRate"] = (double_ActualYds == 0) ? 0 : Math.Round((SumPoint / double_ActualYds) * 100, 2);
//            #region Grade,Result
//            string WeaveTypeid = MyUtility.GetValue.Lookup("WeaveTypeId", mainrow["SCiRefno"].ToString(), "Fabric", "SciRefno");
//            string grade_cmd = String.Format(@"
// SELECT MIN(GRADE) grade
// FROM FIR_Grade WITH (NOLOCK)
// WHERE   WEAVETYPEID = '{0}'
//        AND PERCENTAGE >= IIF({1} > 100, 100, {1})", WeaveTypeid, CurrentData["PointRate"]);
//            DataRow grade_dr;
//            if (MyUtility.Check.Seek(grade_cmd, out grade_dr))
//            {
//                CurrentData["Grade"] = grade_dr["grade"];
//                CurrentData["Result"] = MyUtility.GetValue.Lookup(string.Format(@"
// Select   [Result] =  case Result
// when 'P' then 'Pass'
// when 'F' then 'Fail'
// end
// from Fir_Grade WITH (NOLOCK)
// where    WEAVETYPEID = '{0}'
// and Grade = '{1}'", WeaveTypeid, grade_dr["grade"]), null);
//            }
//            #endregion
            return base.DoSave();
        }
    }
}
