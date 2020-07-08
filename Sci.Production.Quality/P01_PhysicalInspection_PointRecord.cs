using Ict;
using Ict.Win;
using System;
using System.Data;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class P01_PhysicalInspection_PointRecord : Sci.Win.Subs.Base
    {
        private DataTable defRecord;
        private string def_num;
        private DataRow def_dr;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_Points;

        public P01_PhysicalInspection_PointRecord(DataRow data_dr, string n_column, bool edit)
        {
            this.InitializeComponent();
            string defect_str = data_dr["def" + n_column].ToString();
            string[] defect = defect_str.Split(new char[] { '/' });
            this.def_num = n_column;
            this.def_dr = data_dr;
            DBProxy.Current.Select(null, "Select ID,Type,DescriptionEN,0 as points From FabricDefect WITH (NOLOCK) ", out this.defRecord);
            string defid;
            int point;
            if (!MyUtility.Check.Empty(defect_str))
            {
                foreach (string dr in defect)
                {
                    // 避免defect值是空值會掛掉
                    if (MyUtility.Check.Empty(dr))
                    {
                        continue;
                    }

                    // 第一碼為ID,第二碼為Points
                    defid = dr.Substring(0, 1);
                    point = MyUtility.Convert.GetInt(dr.Substring(1));
                    DataRow[] Ary = this.defRecord.Select(string.Format("ID = '{0}'", defid));
                    if (Ary.Length > 0)
                    {
                        Ary[0]["Points"] = point;
                    }
                }
            }

            this.gridPhysicalInspection.DataSource = this.defRecord;
            this.gridPhysicalInspection.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridPhysicalInspection)
            .Text("ID", header: "Code", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("DescriptionEN", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("Points", header: "Points", width: Widths.AnsiChars(4), integer_places: 4).Get(out this.col_Points);
            this.GridEditing(edit);
        }

        private void GridEditing(bool isEditing)
        {
            this.col_Points.IsEditingReadOnly = !isEditing;
        }

        private void btnOK_Click(object sender, EventArgs e) // OK
        {
            this.gridPhysicalInspection.ValidateControl();
            int totalPoint = MyUtility.Convert.GetInt(this.defRecord.Compute("Sum(Points)", string.Empty));
            if (totalPoint > 20)
            {
                MyUtility.Msg.WarningBox("According Standard of Inspection, the max. of points per yard is 4,\nso total points can not over 20");
                return;
            }

            string def = string.Empty;
            DataRow[] Ary = this.defRecord.Select("Points<>0");
            foreach (DataRow dr in Ary)
            {
                if (def == string.Empty)
                {
                    def = dr["ID"].ToString() + dr["Points"].ToString();
                }
                else
                {
                    def = def + "/" + dr["ID"].ToString() + dr["Points"].ToString();
                }
            }

            this.def_dr["def" + this.def_num] = def; // 填入DefectRecord
            this.def_dr["Point" + this.def_num] = totalPoint;
            this.Dispose();
        }
    }
}
