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
            MyUtility.Tool.SetupCombox(comboTypeFilter, 2, 1, "A,All,T,Top,B,Bottom,I,Inner,O,Outer");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 撈Grid資料

            #region Std. GSD
            string sqlCmd = string.Format(@"
select 
        id.SEQ
        ,id.Location
        ,id.OperationID
        ,isnull(m.Description,'') as MachineDesc
        ,id.Mold
        ,isnull(o.DescEN,'') as OperationDescEN
        ,id.Annotation
        ,id.Frequency
        ,isnull(id.MtlFactorID,'') as MtlFactorID
        ,isnull(o.SMV,0) as SMV
        ,isnull(o.SMV,0) * id.Frequency as newSMV
        ,isnull(o.SeamLength,0) as SeamLength
        ,iif(id.Location = 'T','Top',iif(id.Location = 'B','Bottom',iif(id.Location = 'I','Inner',iif(id.Location = 'O','Outer','')))) as Type
        ,isnull(o.SeamLength,0)*id.Frequency as ttlSeamLength

from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
left join Operation o WITH (NOLOCK) on id.OperationID = o.ID
left join MachineType m WITH (NOLOCK) on o.MachineTypeID = m.ID
--left join MtlFactor mf WITH (NOLOCK) on mf.Type = 'F' and o.MtlFactorID = mf.ID
where s.Ukey = {0} order by id.SEQ", styleUkey);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData1);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query IETMS fail!\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData1;
            #endregion

            #region Summary by artwork
            sqlCmd = $@"
select  i.Location, i.ArtworkTypeID,
	type = iif(i.Location = 'T','Top',iif(i.Location = 'B','Bottom',iif(i.Location = 'I','Inner',iif(i.Location = 'O','Outer','')))),
	tms = CEILING(sum(i.ProSMV) * 60)
from IETMS_Summary i
where i.IETMSUkey = (select distinct i.Ukey from Style s WITH (NOLOCK) inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version where s.ukey = '{styleUkey}')
group by i.Location,i.ArtworkTypeID
";
            result = DBProxy.Current.Select(null, sqlCmd, out gridData2);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Summary by artwork fail!\r\n" + result.ToString());
            }
            if (gridData2.Rows.Count == 0) // 若舊資料IETMS_Summary沒有資料
            {
                sqlCmd = $@"
select id.Location,M.ArtworkTypeID,
iif(id.Location = 'T','Top',iif(id.Location = 'B','Bottom',iif(id.Location = 'I','Inner',iif(id.Location = 'O','Outer','')))) as Type,
round(sum(isnull(o.smv,0)*id.Frequency*(isnull(id.MtlFactorRate,0)/100+1)*60),0) as tms
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
inner join Operation o WITH (NOLOCK) on id.OperationID = o.ID
inner join MachineType m WITH (NOLOCK) on o.MachineTypeID = m.ID
where s.Ukey = {styleUkey}
group by id.Location,M.ArtworkTypeID
";
                result = DBProxy.Current.Select(null, sqlCmd, out gridData2);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query Summary by artwork fail!\r\n" + result.ToString());
                }
            }
            listControlBindingSource2.DataSource = gridData2;
            #endregion

            #region Summary by machine
            sqlCmd = string.Format(@"
select ies.location
, Type=iif(ies.Location = 'T','Top',iif(ies.Location = 'B','Bottom',iif(ies.Location = 'I','Inner',iif(ies.Location = 'O','Outer',''))))
, MachineTypeID = mt.ID
, mt.Description
, mt.DescCH
, mt.RPM
, mt.Stitches
, TMS = CEILING(sum(ies.ProSMV) * 60)
from IETMS_Summary ies
Left join MachineType mt on ies.MachineTypeID =mt.ID
where IETMSUkey = (select distinct i.Ukey from Style s WITH (NOLOCK) inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version where s.ukey = '{0}')
and ies.Location != ''
Group by mt.ID,mt.Description,mt.DescCH,mt.RPM,mt.Stitches,ies.location", styleUkey);
            result = DBProxy.Current.Select(null, sqlCmd, out gridData3);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Summary by machine fail!\r\n" + result.ToString());
            }
            listControlBindingSource3.DataSource = gridData3;
            #endregion

            #endregion

            if (gridData2.Rows.Count > 0)
            {
                numTotalGSD.Value = Convert.ToDecimal(gridData2.Compute("sum(tms)", ""));
            }
            else
            {
                numTotalGSD.Value = 0;
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

        private void btnCIPF_Click(object sender, EventArgs e)
        {
            string ietmsUKEY = MyUtility.GetValue.Lookup($@"
select distinct i.Ukey
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
where s.ukey = '{styleUkey}'
                ");
            var dlg = new CIPF(MyUtility.Convert.GetLong(ietmsUKEY));
            dlg.Show();
        }

        private void CalculateData()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select s.ID,s.SeasonID,i.ActFinDate,s.IETMSID,s.IETMSVersion
 from Style s WITH (NOLOCK) 
 left join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
 left join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
 left join Operation o WITH (NOLOCK) on id.OperationID = o.ID
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
                if (MyUtility.Check.Empty(tmpData.Rows[0]["ActFinDate"]))
                {
                    dateRequireFinish.Value = null;
                }
                else
                {
                    dateRequireFinish.Value = Convert.ToDateTime(tmpData.Rows[0]["ActFinDate"]);
                }
            }

            #region TTLCpuTms
            string sqlTTLCpuTms = $@"
select ProductionCpuTms =  CEILING(sum(iif(a.IsTTLTMS = 1, IES.ProSMV,0))*60)
, ProductionSMV = Round(sum(IES.ProSMV),3)
from Style s WITH (NOLOCK) 
left join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
left join IETMS_Summary IES on IES.IETMSUkey = i.Ukey
left join ArtworkType a WITH (NOLOCK) on ies.ArtworkTypeID = a.ID
where s.Ukey = {styleUkey} 
";
            if (comboTypeFilter.SelectedIndex != -1 && comboTypeFilter.SelectedValue.ToString() != "A")
            {
                sqlTTLCpuTms += string.Format(" and IES.Location = '{0}'", comboTypeFilter.SelectedValue.ToString());
            }
            sqlTTLCpuTms += " group by s.ID,s.SeasonID,i.ActFinDate,i.ID,i.Version ";
            DataRow dr;
            if (MyUtility.Check.Seek(sqlTTLCpuTms, out dr))
            {
                numTotalCPUTMS.Value = MyUtility.Convert.GetDecimal(dr["ProductionCpuTms"]);
                numTotalSMV.Value = MyUtility.Convert.GetDecimal(dr["ProductionSMV"]);
            }
            else
            {
                numTotalCPUTMS.Value = 0;
                numTotalSMV.Value = 0;
            }
            #endregion

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
                }
                else
                {
                    gridData1.DefaultView.RowFilter = "";
                    gridData2.DefaultView.RowFilter = "";
                    gridData3.DefaultView.RowFilter = "";
                }
            }
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {

            DataTable ExcelTable;
            try
            {
                string ietmsUKEY = MyUtility.GetValue.Lookup($@"
select distinct i.Ukey
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
where s.ukey = '{styleUkey}'
                ");

                string sqlCmd = string.Empty;
                sqlCmd = $@"
select 
    seq = '0',	Type = null,	OperationID = '--CUTTING',	MachineDesc = null,Mold = null,OperationDescEN = null,Annotation = null,
	Frequency = round(ProTMS, 4),	MtlFactorID = null,	SMV = round(ProTMS, 4),	newSMV = round(ProTMS, 4),
	SeamLength = null,	ttlSeamLength = null
from[IETMS_Summary] where location = '' and[IETMSUkey] = {ietmsUKEY} and ArtworkTypeID = 'Cutting'
union all
select
    seq = '0', Type = null, OperationID = 'SIOCIPF00001', MachineDesc = 'CUT', Mold = null, OperationDescEN = '**Cutting', Annotation = null,
    Frequency = round(ProTMS, 4), MtlFactorID = 'ENL', SMV = round(ProTMS, 4), newSMV = round(ProTMS, 4),
    SeamLength = null, ttlSeamLength = null
from[IETMS_Summary] where location = '' and[IETMSUkey] = {ietmsUKEY} and ArtworkTypeID = 'Cutting'
union all
";
                sqlCmd += string.Format("select Seq,Type,OperationID,MachineDesc,Mold,OperationDescEN,Annotation,Frequency,MtlFactorID,SMV,newSMV,SeamLength,ttlSeamLength from #tmp {0}", comboTypeFilter.SelectedIndex != -1 && comboTypeFilter.SelectedValue.ToString() != "A" ? "where Location = '" + comboTypeFilter.SelectedValue.ToString() + "'" : "");

                sqlCmd += $@"
union all
select 
	seq = '9960',	Type = null,	OperationID = '--IPF',	MachineDesc=null,Mold=null,OperationDescEN=null,Annotation=null,
	Frequency = sum(round(ProTMS,4)),	MtlFactorID=null,	SMV = sum(round(ProTMS,4)),	newSMV = sum(round(ProTMS,4)),
	SeamLength = null,	ttlSeamLength = null
from [IETMS_Summary] where location = '' and [IETMSUkey] = {ietmsUKEY} and ArtworkTypeID <> 'Cutting'
union all
select 
	seq = '9970',Type = null,OperationID = 'SIOCIPF00002',MachineDesc='M',Mold=null,OperationDescEN='**Inspection',Annotation=null,
	Frequency = round(ProTMS,4),	MtlFactorID='ENL',	SMV = round(ProTMS,4),	newSMV = round(ProTMS,4),
	SeamLength = null,	ttlSeamLength = null
from [IETMS_Summary] where location = '' and [IETMSUkey] = {ietmsUKEY} and ArtworkTypeID = 'Inspection'
union all
select 
	seq = '9980',Type = null,OperationID = 'SIOCIPF00004',MachineDesc='MM2',Mold=null,OperationDescEN='**Pressing',Annotation=null,
	Frequency = round(ProTMS,4),	MtlFactorID='ENL',	SMV = round(ProTMS,4),	newSMV = round(ProTMS,4),
	SeamLength = null,	ttlSeamLength = null
from [IETMS_Summary] where location = '' and [IETMSUkey] = {ietmsUKEY} and ArtworkTypeID = 'Pressing'
union all
select 
	seq = '9990',Type = null,OperationID = 'SIOCIPF00003',MachineDesc='MM2',Mold=null,OperationDescEN='**Packing',Annotation=null,
	Frequency = round(ProTMS,4),	MtlFactorID='ENL',	SMV = round(ProTMS,4),	newSMV = round(ProTMS,4),
	SeamLength = null,	ttlSeamLength = null
from [IETMS_Summary] where location = '' and [IETMSUkey] = {ietmsUKEY} and ArtworkTypeID = 'Packing'
order by seq
";
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

            bool result = MyUtility.Excel.CopyToXls(ExcelTable, "", "PPIC_P01_StdGSDList.xltx", headerRow: 1);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            // }
            //else
            //{
            return;
            //}
        }


    }
}
