﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    public partial class StdGSDList : Sci.Win.Subs.Base
    {
        private long styleUkey;
        private DataTable gridData1, gridData2, gridData3, tmpData;
        public StdGSDList(long StyleUKey)
        {
            InitializeComponent();
            styleUkey = StyleUKey;
            MyUtility.Tool.SetupCombox(comboTypeFilter,2,1,"A,All,T,Top,B,Bottom,I,Inner,O,Outer");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 撈Grid資料

            #region Std. GSD
            string sqlCmd = string.Format(@"select id.SEQ,id.Location,id.OperationID,isnull(m.Description,'') as MachineDesc,id.Mold,isnull(o.DescEN,'') as OperationDescEN,id.Annotation,id.Frequency,
isnull(o.MtlFactorID,'') as MtlFactorID,isnull(o.SMV,0) as SMV,isnull(o.SeamLength,0) as SeamLength,
iif(id.Location = 'T','Top',iif(id.Location = 'B','Bottom',iif(id.Location = 'I','Inner',iif(id.Location = 'O','Outer','')))) as Type,
round(isnull(o.smv,0)*id.Frequency*(isnull(mf.Rate,0)/100+1),4) as newSMV,isnull(o.SeamLength,0)*id.Frequency as ttlSeamLength,
round(isnull(o.smv,0)*id.Frequency*(isnull(mf.Rate,0)/100+1)*60,4) as gsdsec
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
left join Operation o WITH (NOLOCK) on id.OperationID = o.ID
left join MachineType m WITH (NOLOCK) on o.MachineTypeID = m.ID
left join MtlFactor mf WITH (NOLOCK) on mf.Type = 'F' and o.MtlFactorID = mf.ID
where s.Ukey = {0} order by id.SEQ", styleUkey);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData1);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query IETMS fail!\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData1;
            #endregion

            #region Summary by artwork
            sqlCmd = string.Format(@"select id.Location,ATD.ArtworkTypeID,
iif(id.Location = 'T','Top',iif(id.Location = 'B','Bottom',iif(id.Location = 'I','Inner',iif(id.Location = 'O','Outer','')))) as Type,
round(sum(isnull(o.smv,0)*id.Frequency*(isnull(mf.Rate,0)/100+1)*60),0) as tms
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
inner join Operation o WITH (NOLOCK) on id.OperationID = o.ID
inner join MachineType m WITH (NOLOCK) on o.MachineTypeID = m.ID
left join MtlFactor mf WITH (NOLOCK) on mf.Type = 'F' and o.MtlFactorID = mf.ID
LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON m.ID=ATD.MachineTypeID
where s.Ukey = {0}
group by id.Location,ATD.ArtworkTypeID", styleUkey);
            result = DBProxy.Current.Select(null, sqlCmd, out gridData2);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Summary by artwork fail!\r\n" + result.ToString());
            }
            listControlBindingSource2.DataSource = gridData2;
            #endregion

            #region Summary by machine
            sqlCmd = string.Format(@"select id.Location,o.MachineTypeID,isnull(m.Description,'') as Description,isnull(m.DescCH,'') as DescCH,
isnull(m.RPM,0) as RPM,isnull(m.Stitches,0.0) as Stitches,
iif(id.Location = 'T','Top',iif(id.Location = 'B','Bottom',iif(id.Location = 'I','Inner',iif(id.Location = 'O','Outer','')))) as Type,
round(sum(isnull(o.smv,0)*id.Frequency*(isnull(mf.Rate,0)/100+1)*60),0) as tms
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
inner join Operation o WITH (NOLOCK) on id.OperationID = o.ID
left join MachineType m WITH (NOLOCK) on o.MachineTypeID = m.ID
left join MtlFactor mf WITH (NOLOCK) on mf.Type = 'F' and o.MtlFactorID = mf.ID
where s.Ukey = {0}
group by id.Location,o.MachineTypeID,isnull(m.Description,''),isnull(m.DescCH,''),isnull(m.RPM,0),isnull(m.Stitches,0.0)
ORDER BY id.Location,o.MachineTypeID", styleUkey);
            result = DBProxy.Current.Select(null, sqlCmd, out gridData3);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Summary by machine fail!\r\n" + result.ToString());
            }
            listControlBindingSource3.DataSource = gridData3;
            #endregion

            #endregion

            if (gridData1.Rows.Count > 0)
            {
                numTotalGSD.Value = Convert.ToDecimal(gridData1.Compute("sum(gsdsec)", ""));
                numTotalSMV.Value = Convert.ToDecimal(gridData1.Compute("sum(newSMV)", ""));
            }
            else
            {
                numTotalGSD.Value = 0;
                numTotalSMV.Value = 0;
            }
            CalculateData();

            //設定Grid1的顯示欄位
            this.gridStdGSD.IsEditingReadOnly = true;
            this.gridStdGSD.DataSource = listControlBindingSource1.DataSource;
            Helper.Controls.Grid.Generator(this.gridStdGSD)
                 .Text("Seq", header: "Seq", width: Widths.AnsiChars(4))
                 .Text("Type", header: "Type", width: Widths.AnsiChars(6))
                .Text("OperationID", header: "Operation code", width: Widths.AnsiChars(13))
                .Text("MachineDesc", header: "M/C", width: Widths.AnsiChars(20))
                .Text("Mold", header: "Mold/Att", width: Widths.AnsiChars(20))
                .EditText("OperationDescEN", header: "Operation Description", width: Widths.AnsiChars(30))
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(30))
                .Numeric("Frequency", header: "Freq.", decimal_places: 2)
                .Text("MtlFactorID", header: "Fac.", width: Widths.AnsiChars(3))
                .Numeric("SMV", header: "Ori. SMV", decimal_places: 4)
                .Numeric("newSMV", header: "S.M.V", decimal_places: 4)
                .Numeric("SeamLength", header: "Seam length", decimal_places: 2)
                .Numeric("ttlSeamLength", header: "Total seam length", decimal_places: 4);

            //設定Grid2的顯示欄位
            this.gridSummaryByArtwork.IsEditingReadOnly = true;
            this.gridSummaryByArtwork.DataSource = listControlBindingSource2.DataSource;
            Helper.Controls.Grid.Generator(this.gridSummaryByArtwork)
                 .Text("Type", header: "Type", width: Widths.AnsiChars(6))
                 .Text("ArtworkTypeID", header: "ArtWork", width: Widths.AnsiChars(20))
                 .Numeric("tms", header: "TMS(sec)", width: Widths.AnsiChars(8));

            //設定Grid3的顯示欄位
            this.gridSummaryByMachine.IsEditingReadOnly = true;
            this.gridSummaryByMachine.DataSource = listControlBindingSource3.DataSource;
            Helper.Controls.Grid.Generator(this.gridSummaryByMachine)
                 .Text("Type", header: "Type", width: Widths.AnsiChars(6))
                 .Text("MachineTypeID", header: "Machine", width: Widths.AnsiChars(10))
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(20))
                 .EditText("DescCH", header: "Description(Chinese)", width: Widths.AnsiChars(20))
                 .Numeric("RPM", header: "RPM", width: Widths.AnsiChars(8))
                 .Numeric("Stitches", header: "Stitches per CM", width: Widths.AnsiChars(8))
                 .Numeric("tms", header: "TMS(sec)", width: Widths.AnsiChars(8));
        }

        private void CalculateData()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select s.ID,s.SeasonID,i.ActFinDate,s.IETMSID,s.IETMSVersion,
 round(sum(isnull(o.SMV,0)*isnull(id.Frequency,0)*(isnull(m.Rate,0)/100+1)*60),4) as ttlTMS
 from Style s WITH (NOLOCK) 
 left join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
 left join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
 left join Operation o WITH (NOLOCK) on id.OperationID = o.ID
 left join MtlFactor m WITH (NOLOCK) on m.Type = 'F' and o.MtlFactorID = m.ID
 left join MachineType mt WITH (NOLOCK) on o.MachineTypeID = mt.ID
 LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON MT.ID=ATD.MachineTypeID
 left join ArtworkType a WITH (NOLOCK) on ATD.ArtworkTypeID = a.ID and a.IsTMS = 1
 where s.Ukey = {0}", styleUkey.ToString()));
            if (comboTypeFilter.SelectedIndex != -1 && comboTypeFilter.SelectedValue.ToString() != "A")
            {
                sqlCmd.Append(string.Format(" and id.Location = '{0}'", comboTypeFilter.SelectedValue.ToString()));
            }
            sqlCmd.Append(" group by s.ID,s.SeasonID,i.ActFinDate,s.IETMSID,s.IETMSVersion");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out tmpData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Style fail!\r\n" + result.ToString());
            }
            if (tmpData.Rows.Count > 0)
            {
                displayStyleNo.Value = tmpData.Rows[0]["ID"].ToString();
                displaySeason.Value = tmpData.Rows[0]["SeasonID"].ToString();
                displayApplyNo.Value = tmpData.Rows[0]["IETMSID"].ToString();
                displayVersion.Value = tmpData.Rows[0]["IETMSVersion"].ToString();
                numTotalCPUTMS.Value = Convert.ToDecimal(tmpData.Rows[0]["ttlTMS"]);
                if (MyUtility.Check.Empty(tmpData.Rows[0]["ActFinDate"]))
                {
                    dateRequireFinish.Value = null;
                }
                else
                {
                    dateRequireFinish.Value = Convert.ToDateTime(tmpData.Rows[0]["ActFinDate"]);
                }
            }
            else
            {
                numTotalCPUTMS.Value = 0;
            }
        }

        //Type Filter
        private void comboTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gridData1 != null)
            {
                if (comboTypeFilter.SelectedIndex != -1 && comboTypeFilter.SelectedValue.ToString() != "A")
                {
                    string filter = "Location = '" + comboTypeFilter.SelectedValue.ToString() + "'";
                    gridData1.DefaultView.RowFilter = filter;
                    gridData2.DefaultView.RowFilter = filter;
                    gridData3.DefaultView.RowFilter = filter;
                    if (gridData1.DefaultView.Count > 0)
                    {
                        numTotalGSD.Value = Convert.ToDecimal(gridData1.Compute("sum(gsdsec)", filter));
                        numTotalSMV.Value = Convert.ToDecimal(gridData1.Compute("sum(newSMV)", filter));
                    }
                    else
                    {
                        numTotalGSD.Value = 0;
                        numTotalSMV.Value = 0;
                    }
                    CalculateData();
                }
                else
                {
                    gridData1.DefaultView.RowFilter = "";
                    gridData2.DefaultView.RowFilter = "";
                    gridData3.DefaultView.RowFilter = "";
                    if (gridData1.Rows.Count > 0)
                    {
                        numTotalGSD.Value = Convert.ToDecimal(gridData1.Compute("sum(gsdsec)", ""));
                        numTotalSMV.Value = Convert.ToDecimal(gridData1.Compute("sum(newSMV)", ""));
                    }
                    else
                    {
                        numTotalGSD.Value = 0;
                        numTotalSMV.Value = 0;
                    }
                    CalculateData();
                }
            }
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
           
            DataTable ExcelTable;
            try
            {
                string sqlCmd = string.Format("select Seq,Type,OperationID,MachineDesc,Mold,OperationDescEN,Annotation,Frequency,MtlFactorID,SMV,newSMV,SeamLength,ttlSeamLength from #tmp {0}",comboTypeFilter.SelectedIndex != -1 && comboTypeFilter.SelectedValue.ToString() != "A"?"where Location = '"+comboTypeFilter.SelectedValue.ToString()+"'":"");

                MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Seq,Type,OperationID,MachineDesc,Mold,OperationDescEN,Annotation,Frequency,MtlFactorID,SMV,newSMV,SeamLength,ttlSeamLength,Location", sqlCmd, out ExcelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            //dlg.FileName = "StdGSDList_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            //{
             
            // Open document
         
                bool result = MyUtility.Excel.CopyToXls(ExcelTable,"", "PPIC_P01_StdGSDList.xltx",headerRow:1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
           // }
            //else
            //{
                return;
            //}
        }
       

    }
}
