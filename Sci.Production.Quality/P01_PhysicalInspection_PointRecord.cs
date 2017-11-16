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
            InitializeComponent();
            string defect_str = data_dr["def" + n_column].ToString();
            string[] defect = defect_str.Split(new char[] { '/' });
            def_num = n_column;
            def_dr = data_dr;
            DBProxy.Current.Select(null, "Select ID,Type,DescriptionEN,0 as points From FabricDefect WITH (NOLOCK) ", out defRecord);
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
                    DataRow[] Ary = defRecord.Select(string.Format("ID = '{0}'", defid));
                    if (Ary.Length > 0)
                    {
                        Ary[0]["Points"] = point;
                    }
                }
            }
            gridPhysicalInspection.DataSource = defRecord;
            gridPhysicalInspection.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(gridPhysicalInspection)
            .Text("ID", header: "Code", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("DescriptionEN", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("Points", header: "Points", width: Widths.AnsiChars(4), integer_places: 4).Get(out col_Points);
            GridEditing(edit);

        }
        private void GridEditing(bool isEditing)
        {
            col_Points.IsEditingReadOnly = !isEditing;
        }
        private void btnOK_Click(object sender, EventArgs e) //OK
        {
            gridPhysicalInspection.ValidateControl();
            int totalPoint = MyUtility.Convert.GetInt(defRecord.Compute("Sum(Points)", ""));
            if (totalPoint > 20)
            {
                MyUtility.Msg.WarningBox("According Standard of Inspection, the max. of points per yard is 4,\nso total points can not over 20");
                return;
            }
            string def = "";
            DataRow[] Ary = defRecord.Select("Points<>0");
            foreach (DataRow dr in Ary)
            {
                if (def == "") def = dr["ID"].ToString() + dr["Points"].ToString();
                else def = def + "/" + dr["ID"].ToString() + dr["Points"].ToString();
            }
            def_dr["def" + def_num] = def; //填入DefectRecord
            def_dr["Point" + def_num] = totalPoint;
            this.Dispose();
        }
    }
}
