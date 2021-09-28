using Ict;
using Ict.Resources;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_PhysicalInspection_PointRecord : Win.Subs.Base
    {
        private readonly DataTable defRecord;
        private readonly string def_num;
        private readonly DataRow def_dr;
        private readonly Ict.Win.UI.DataGridViewNumericBoxColumn col_Points;
        private readonly bool b_edit;
        private List<Endline_Camera_Schema> picList = new List<Endline_Camera_Schema>();

        /// <inheritdoc/>
        public P01_PhysicalInspection_PointRecord(DataRow data_dr, string n_column, bool edit)
        {
            this.InitializeComponent();
            string defect_str = data_dr["def" + n_column].ToString();
            string[] defect = defect_str.Split(new char[] { '/' });
            this.def_num = n_column;
            this.def_dr = data_dr;
            this.b_edit = edit;
            string where = edit ? "where fd.junk = 0" : string.Empty;
            string sqlcmd = $@"
Select distinct fd.ID,Type,DescriptionEN,0 as points 
From FabricDefect fd WITH (NOLOCK) 
{where}
order by fd.ID ";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out this.defRecord);

            if (!dualResult)
            {
                this.ShowErr(dualResult);
                return;
            }

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
                    defid = Prgs.SplitDefectNum(dr);
                    point = MyUtility.Convert.GetInt(Prgs.SplitDefectNum(dr, 1));
                    DataRow[] ary = this.defRecord.Select(string.Format("ID = '{0}'", defid));
                    if (ary.Length > 0)
                    {
                        ary[0]["Points"] = point;
                    }
                }
            }

            DataGridViewGeneratorImageColumnSettings col_img = new DataGridViewGeneratorImageColumnSettings();

            col_img.CellClick += (s, a) =>
            {
                // 小心連點太快會直接刪除新開窗資料
                if (a.RowIndex != -1)
                {
                    DataRow dr = this.gridPhysicalInspection.GetDataRow(a.RowIndex);
                    var tempShow = this.picList.Where(t => t.FabricdefectID == dr["ID"].ToString());
                    if (tempShow.Count() != 0)
                    {
                        var frm = new Camera_ShowNew(dr["ID"].ToString(), tempShow.ToList());
                        frm.ShowDialog();
                    }

                    foreach (var item in tempShow)
                    {
                        if (Camera_Prg.MasterSchemas.Where(r => r.Pkey.Equals(item.Pkey) && r.ID.Equals(item.ID)).Any() == false)
                        {
                            // 刪除檔案
                            if (System.IO.File.Exists(item.imgPath))
                            {
                                try
                                {
                                    item.image.Dispose();
                                    System.IO.File.Delete(item.imgPath);
                                }
                                catch (Exception ex)
                                {
                                    this.ShowErr(ex.Message.ToString());
                                    break;
                                }
                            }

                            string sqlcmdDelete = $@"
delete from ManufacturingExecution.dbo.Clip 
where PKey = '{item.Pkey}'
";
                            DualResult result;
                            if (!(result = DBProxy.Current.Execute(string.Empty, sqlcmdDelete)))
                            {
                                this.ShowErr(result);
                                break;
                            }
                        }
                    }

                    this.ReLoadImage();
                }
            };

            this.gridPhysicalInspection.DataSource = this.defRecord;
            this.gridPhysicalInspection.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridPhysicalInspection)
            .Text("ID", header: "Code", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("DescriptionEN", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("Points", header: "Points", width: Widths.AnsiChars(4), integer_places: 4).Get(out this.col_Points)
            .Image("showPic", header: "Defect" + Environment.NewLine + "Picture", width: Widths.AnsiChars(5), settings: col_img)
            ;
            this.GridEditing(edit);

        }

        private void ReLoadImage()
        {
            DataTable tmp_dt = (DataTable)this.gridPhysicalInspection.DataSource;

            #region 存放照片在暫存檔

            // 重新載入照片
            if (this.picList != null && this.picList.Count > 0)
            {
                this.picList.Clear();
            }

            DataTable dt;
            string sqlcmd = $@"
select  c.SourceFile,
        c.UniqueKey,
        c.Description,
        c.Pkey,
        [yyyyMM] = format(c.AddDate,'yyyyMM')
into    #MES_Clip        
from    [ExtendServer].ManufacturingExecution.dbo.Clip c
where   c.TableName = 'FIR_Physical_Defect_Realtime' and
        c.UniqueKey in (select ID from FIR_Physical_Defect_Realtime with (nolock) where FIR_PhysicalDetailUkey = {this.def_dr["DetailUkey"]})


select  r.FabricdefectID,
        r.FIR_PhysicalDetailUkey,
        c.SourceFile,
        c.UniqueKey,
        c.Description,
        c.Pkey,
        c.yyyyMM
from FIR_Physical_Defect_Realtime r 
inner join #MES_Clip  c on c.UniqueKey = r.Id
where r.FIR_PhysicalDetailUkey = {this.def_dr["DetailUkey"]}
order by r.FabricdefectID,r.FIR_PhysicalDetailUkey,c.SourceFile";

            DBProxy.Current.Select(null, sqlcmd, out dt);

            foreach (DataRow item in dt.Rows)
            {
                string strpath = Path.Combine(Camera_Prg.GetCameraPath(item["yyyyMM"].ToString()), item["SourceFile"].ToString());
                if (!File.Exists(strpath))
                {
                    continue;
                }

                using (var fs = new System.IO.FileStream(strpath, System.IO.FileMode.Open))
                {
                    var temp1 = new Endline_Camera_Schema()
                    {
                        Pkey = item["Pkey"].ToString(),
                        ID = item["UniqueKey"].ToString(),
                        desc = item["Description"].ToString(),
                        image = new Bitmap(fs),
                        FabricdefectID = item["FabricdefectID"].ToString(),
                        imgPath = strpath,
                    };
                    this.picList.Add(temp1);
                }
            }

            Camera_Prg.MasterSchemas = this.picList.ToList();
            #endregion

            // 暫存的照片存在就顯示圖片
            foreach (DataGridViewRow drFabricDefect in this.gridPhysicalInspection.Rows)
            {
                if (this.picList.Any(s => s.FabricdefectID == drFabricDefect.Cells["ID"].Value.ToString()))
                {
                    drFabricDefect.Cells["showPic"].Value = Resource.image_icon1;
                }
                else
                {
                    drFabricDefect.Cells["showPic"].Value = null;
                }
            }

            this.gridPhysicalInspection.ValidateControl();
        }

        private void GridEditing(bool isEditing)
        {
            this.col_Points.IsEditingReadOnly = !isEditing;
        }

        private void BtnOK_Click(object sender, EventArgs e) // OK
        {
            this.gridPhysicalInspection.ValidateControl();
            int totalPoint = MyUtility.Convert.GetInt(this.defRecord.Compute("Sum(Points)", string.Empty));
            if (totalPoint > 20)
            {
                MyUtility.Msg.WarningBox("According Standard of Inspection, the max. of points per yard is 4,\nso total points can not over 20");
                return;
            }

            string def = string.Empty;
            DataRow[] ary = this.defRecord.Select("Points<>0");
            foreach (DataRow dr in ary)
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

        private void P01_PhysicalInspection_PointRecord_FormLoaded(object sender, EventArgs e)
        {
            this.ReLoadImage();
        }
    }
}
