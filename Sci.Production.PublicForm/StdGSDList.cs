using System;
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
            MyUtility.Tool.SetupCombox(comboBox1,2,1,"A,All,T,Top,B,Bottom,I,Inner,O,Outer");
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
from Style s
inner join IETMS i on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id on i.IETMSUkey = id.IETMSUkey
left join Operation o on id.OperationID = o.ID
left join MachineType m on o.MachineTypeID = m.ID
left join MtlFactor mf on mf.Type = 'F' and o.MtlFactorID = mf.ID
where s.Ukey = {0}", styleUkey);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData1);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query IETMS fail!\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData1;
            #endregion

            #region Summary by artwork
            sqlCmd = string.Format(@"select id.Location,m.ArtworkTypeID,
iif(id.Location = 'T','Top',iif(id.Location = 'B','Bottom',iif(id.Location = 'I','Inner',iif(id.Location = 'O','Outer','')))) as Type,
round(sum(isnull(o.smv,0)*id.Frequency*(isnull(mf.Rate,0)/100+1)*60),0) as tms
from Style s
inner join IETMS i on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id on i.IETMSUkey = id.IETMSUkey
inner join Operation o on id.OperationID = o.ID
inner join MachineType m on o.MachineTypeID = m.ID
left join MtlFactor mf on mf.Type = 'F' and o.MtlFactorID = mf.ID
where s.Ukey = {0}
group by id.Location,m.ArtworkTypeID", styleUkey);
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
from Style s
inner join IETMS i on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id on i.IETMSUkey = id.IETMSUkey
inner join Operation o on id.OperationID = o.ID
left join MachineType m on o.MachineTypeID = m.ID
left join MtlFactor mf on mf.Type = 'F' and o.MtlFactorID = mf.ID
where s.Ukey = {0}
group by id.Location,o.MachineTypeID,isnull(m.Description,''),isnull(m.DescCH,''),isnull(m.RPM,0),isnull(m.Stitches,0.0)", styleUkey);
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
                numericBox1.Value = Convert.ToDecimal(gridData1.Compute("sum(gsdsec)", ""));
                numericBox3.Value = Convert.ToDecimal(gridData1.Compute("sum(newSMV)", ""));
            }
            else
            {
                numericBox1.Value = 0;
                numericBox3.Value = 0;
            }
            CalculateData();

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1.DataSource;
            Helper.Controls.Grid.Generator(this.grid1)
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
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2.DataSource;
            Helper.Controls.Grid.Generator(this.grid2)
                 .Text("Type", header: "Type", width: Widths.AnsiChars(6))
                 .Text("ArtworkTypeID", header: "ArtWork", width: Widths.AnsiChars(20))
                 .Numeric("tms", header: "TMS(sec)", width: Widths.AnsiChars(8));

            //設定Grid3的顯示欄位
            this.grid3.IsEditingReadOnly = true;
            this.grid3.DataSource = listControlBindingSource3.DataSource;
            Helper.Controls.Grid.Generator(this.grid3)
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
 from Style s
 left join IETMS i on s.IETMSID = i.ID and s.IETMSVersion = i.Version
 left join IETMS_Detail id on i.IETMSUkey = id.IETMSUkey
 left join Operation o on id.OperationID = o.ID
 left join MtlFactor m on m.Type = 'F' and o.MtlFactorID = m.ID
 left join MachineType mt on o.MachineTypeID = mt.ID
 left join ArtworkType a on mt.ArtworkTypeID = a.ID and a.IsTMS = 1
 where s.Ukey = {0}", styleUkey.ToString()));
            if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedValue.ToString() != "A")
            {
                sqlCmd.Append(string.Format(" and id.Location = '{0}'", comboBox1.SelectedValue.ToString()));
            }
            sqlCmd.Append(" group by s.ID,s.SeasonID,i.ActFinDate,s.IETMSID,s.IETMSVersion");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out tmpData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Style fail!\r\n" + result.ToString());
            }
            if (tmpData.Rows.Count > 0)
            {
                displayBox1.Value = tmpData.Rows[0]["ID"].ToString();
                displayBox3.Value = tmpData.Rows[0]["SeasonID"].ToString();
                displayBox2.Value = tmpData.Rows[0]["IETMSID"].ToString();
                displayBox4.Value = tmpData.Rows[0]["IETMSVersion"].ToString();
                numericBox2.Value = Convert.ToDecimal(tmpData.Rows[0]["ttlTMS"]);
                if (MyUtility.Check.Empty(tmpData.Rows[0]["ActFinDate"]))
                {
                    dateBox1.Value = null;
                }
                else
                {
                    dateBox1.Value = Convert.ToDateTime(tmpData.Rows[0]["ActFinDate"]);
                }
            }
            else
            {
                numericBox2.Value = 0;
            }
        }

        //Type Filter
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gridData1 != null)
            {
                if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedValue.ToString() != "A")
                {
                    string filter = "Location = '" + comboBox1.SelectedValue.ToString() + "'";
                    gridData1.DefaultView.RowFilter = filter;
                    gridData2.DefaultView.RowFilter = filter;
                    gridData3.DefaultView.RowFilter = filter;
                    if (gridData1.DefaultView.Count > 0)
                    {
                        numericBox1.Value = Convert.ToDecimal(gridData1.Compute("sum(gsdsec)", filter));
                        numericBox3.Value = Convert.ToDecimal(gridData1.Compute("sum(newSMV)", filter));
                    }
                    else
                    {
                        numericBox1.Value = 0;
                        numericBox3.Value = 0;
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
                        numericBox1.Value = Convert.ToDecimal(gridData1.Compute("sum(gsdsec)", ""));
                        numericBox3.Value = Convert.ToDecimal(gridData1.Compute("sum(newSMV)", ""));
                    }
                    else
                    {
                        numericBox1.Value = 0;
                        numericBox3.Value = 0;
                    }
                    CalculateData();
                }
            }
        }

        //To Excel
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable ExcelTable;
            try
            {
                string sqlCmd = string.Format("select Seq,Type,OperationID,MachineDesc,Mold,OperationDescEN,Annotation,Frequency,MtlFactorID,SMV,newSMV,SeamLength,ttlSeamLength from #tmp {0}",comboBox1.SelectedIndex != -1 && comboBox1.SelectedValue.ToString() != "A"?"where Location = '"+comboBox1.SelectedValue.ToString()+"'":"");

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
            dlg.FileName = "StdGSDList_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                DualResult result = MyUtility.Excel.CopyToXls(ExcelTable, dlg.FileName);
                if (result) { MyUtility.Excel.XlsAutoFit(dlg.FileName, "PPIC_P01_StdGSDList.xltx", 2); }
                else { MyUtility.Msg.WarningBox(result.ToMessages().ToString(), "Warning"); }
            }
            else
            {
                return;
            }
        }
    }
}
