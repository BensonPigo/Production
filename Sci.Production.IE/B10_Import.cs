﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Microsoft.Office.Interop.Excel;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualBasic.PowerPacks;
using static System.Net.Mime.MediaTypeNames;
using static Sci.MyUtility;
using DataTable = System.Data.DataTable;

namespace Sci.Production.IE
{
    /// <summary>
    /// B10_Import
    /// </summary>
    public partial class B10_Import : Win.Subs.Base
    {
        private DataTable grid2Data = new DataTable();
        private DataTable detailData;
        private string destination_path;

        /// <summary>
        /// B10_Import
        /// </summary>
        public B10_Import()
        {
            this.destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK)", null);
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable excelFile = new DataTable();
            excelFile.Columns.Add("Filename", typeof(string));
            excelFile.Columns.Add("Status", typeof(string));
            excelFile.Columns.Add("FullFileName", typeof(string));

            this.listControlBindingSource1.DataSource = excelFile;
            this.gridAttachFile.DataSource = this.listControlBindingSource1;
            this.gridAttachFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridAttachFile)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(15))
                .Text("Status", header: "Status", width: Widths.AnsiChars(100));

            // 取Grid結構
            // string sqlCmd = "select SPACE(13) as OrderID, null as BuyerDelivery,SPACE(10) as ShipmodeID,SPACE(8) as Article,SPACE(6) as ColorID,SPACE(8) as SizeCode,0.0 as Qty,SPACE(100) as ErrMsg";
            // DualResult result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            this.grid2Data.Columns.Add("Attachment Group", typeof(string));
            this.grid2Data.Columns.Add("ID", typeof(string));
            this.grid2Data.Columns.Add("Description", typeof(string));
            this.grid2Data.Columns.Add("Description(Chinese)", typeof(string));
            this.grid2Data.Columns.Add("Machine Master ID", typeof(string));
            this.grid2Data.Columns.Add("Machine Description", typeof(string));
            this.grid2Data.Columns.Add("Type", typeof(string));
            this.grid2Data.Columns.Add("Measurement", typeof(string));
            this.grid2Data.Columns.Add("Direction/Fold Type", typeof(string));
            this.grid2Data.Columns.Add("Direction/Fold Desc", typeof(string));
            this.grid2Data.Columns.Add("Supplier Part#-1", typeof(string));
            this.grid2Data.Columns.Add("Supplier Brand-1", typeof(string));
            this.grid2Data.Columns.Add("Supplier Part#-2", typeof(string));
            this.grid2Data.Columns.Add("Supplier Brand-2", typeof(string));
            this.grid2Data.Columns.Add("Supplier Part#-3", typeof(string));
            this.grid2Data.Columns.Add("Supplier Brand-3", typeof(string));
            this.grid2Data.Columns.Add("Remark", typeof(string));

            this.grid2Data.Columns.Add("Picture1_File", typeof(byte[]));
            this.grid2Data.Columns.Add("Picture2_File", typeof(byte[]));
            this.grid2Data.Columns.Add("Picture1", typeof(string));
            this.grid2Data.Columns.Add("Picture2", typeof(string));
            this.grid2Data.Columns.Add("HasPicture1", typeof(bool));
            this.grid2Data.Columns.Add("HasPicture2", typeof(bool));

            this.grid2Data.Columns.Add("ErrMsg", typeof(string));

            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Attachment Group", header: "Attachment Group", width: Widths.AnsiChars(20))
                .Text("ID", header: "ID", width: Widths.AnsiChars(20))
                .Text("Description", header: "Description", width: Widths.AnsiChars(50))
                .Text("Description(Chinese)", header: "Description(Chinese)", width: Widths.AnsiChars(50))
                .Text("Machine Master ID", header: "Machine Master ID", width: Widths.AnsiChars(20))
                .Text("Machine Description", header: "Machine Description", width: Widths.AnsiChars(20))
                .Text("Type", header: "Type", width: Widths.AnsiChars(15))
                .Text("Measurement", header: "Measurement", width: Widths.AnsiChars(15))
                .Text("Direction/Fold Type", header: "Direction/Fold Type", width: Widths.AnsiChars(15))
                .Text("Direction/Fold Desc", header: "Direction/Fold Desc", width: Widths.AnsiChars(25))
                .Text("Supplier Part#-1", header: "Supplier Part#-1", width: Widths.AnsiChars(15))
                .Text("Supplier Brand-1", header: "Supplier Brand-1", width: Widths.AnsiChars(15))
                .Text("Supplier Part#-2", header: "Supplier Part#-2", width: Widths.AnsiChars(15))
                .Text("Supplier Brand-2", header: "Supplier Brand-2", width: Widths.AnsiChars(15))
                .Text("Supplier Part#-3", header: "Supplier Part#-3", width: Widths.AnsiChars(15))
                .Text("Supplier Brand-3", header: "Supplier Brand-3", width: Widths.AnsiChars(15))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(100))
                .CheckBox("HasPicture1", header: "Has Picture1", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0)
                .CheckBox("HasPicture2", header: "Has Picture2", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0)
                .Text("ErrMsg", header: "Error Message", width: Widths.AnsiChars(100));

            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                //this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void BtnAddExcel_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
                dr["Filename"] = this.openFileDialog1.SafeFileName;
                dr["Status"] = string.Empty;
                dr["FullFileName"] = this.openFileDialog1.FileName;
                ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(dr);
                this.listControlBindingSource1.MoveLast();
            }
        }

        private void BtnRemoveExcel_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.Position != -1)
            {
                this.listControlBindingSource1.RemoveCurrent();
            }
        }

        private void BtnCheckImport_Click(object sender, EventArgs e)
        {
            #region 判斷第一個Grid是否有資料
            if (this.listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No excel data!!");
                return;
            }
            #endregion

            // 清空Grid2資料
            if (this.grid2Data != null)
            {
                this.grid2Data.Clear();
            }

            this.gridDetail.SuspendLayout();
            #region 檢查1. Grid中的檔案是否存在，不存在時顯示於status欄位；2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位；3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。當檢查都沒問題時，就將資料寫入第2個Grid
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (!MyUtility.Check.Empty(dr["Filename"]))
                {
                    if (!System.IO.File.Exists(MyUtility.Convert.GetString(dr["FullFileName"])))
                    {
                        dr["Status"] = string.Format("Excel file not found < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                    }
                    else
                    {
                        Microsoft.Office.Interop.Excel.Application excel;
                        try
                        {
                            excel = new Microsoft.Office.Interop.Excel.Application();
                        }
                        catch (Exception ex)
                        {
                            dr["Status"] = string.Format("Not able to open excel file < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                            dr["ErrMsg"] = ex.Message;
                            continue;
                        }

                        excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                        excel.Visible = false;
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                        // 檢查Excel格式
                        Microsoft.Office.Interop.Excel.Range range = worksheet.Range[string.Format("A{0}:S{0}", 1)];
                        object[,] objCellArray = range.Value;
                        string id = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                        string machineMasterID = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                        string type = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");
                        string measurement = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "C");
                        string directionFoldType = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "C");

                        if (id != "ID" || machineMasterID.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(" ", string.Empty) != "MachineMasterID" || type != "Type" || measurement != "Measurement" || directionFoldType != @"Direction/Fold Type")
                        {
                            #region 將不存在欄位顯示於status
                            StringBuilder columnName = new StringBuilder();
                            if (id != "ID")
                            {
                                columnName.Append("< ID >, ");
                            }

                            if (machineMasterID != "MachineMasterID")
                            {
                                columnName.Append("< Machine Master ID >, ");
                            }

                            if (type != "Type")
                            {
                                columnName.Append("< Type >, ");
                            }

                            if (measurement != "Measurement")
                            {
                                columnName.Append("< Measurement >, ");
                            }

                            if (directionFoldType != @"Direction/Fold Type")
                            {
                                columnName.Append(@"< Direction/Fold Type >, ");
                            }

                            dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                            #endregion
                        }
                        else
                        {
                            int intRowsCount = worksheet.UsedRange.Rows.Count;
                            int intColumnsCount = worksheet.UsedRange.Columns.Count;
                            int intRowsStart = 2;
                            int intRowsRead = intRowsStart - 1;

                            while (intRowsRead < intRowsCount)
                            {
                                intRowsRead++;

                                range = worksheet.Range[string.Format("A{0}:S{0}", intRowsRead)];
                                objCellArray = range.Value;

                                DataRow newRow = this.grid2Data.NewRow();
                                string moldIDVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C").ToString();
                                newRow["Attachment Group"] = moldIDVal.Length > 20 ? moldIDVal.Substring(0, 20) : moldIDVal;

                                string sewingMachineAttachmentIDVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C").ToString();
                                newRow["ID"] = sewingMachineAttachmentIDVal.Length > 200 ? sewingMachineAttachmentIDVal.Substring(0, 200) : sewingMachineAttachmentIDVal;

                                string descriptionVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C").ToString();
                                newRow["Description"] = descriptionVal.Length > 200 ? descriptionVal.Substring(0, 200) : descriptionVal;

                                string descriptionCNVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C").ToString();
                                newRow["Description(Chinese)"] = descriptionCNVal.Length > 200 ? descriptionVal.Substring(0, 200) : descriptionCNVal;

                                string machineMasterIDVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C").ToString();
                                newRow["Machine Master ID"] = machineMasterIDVal.Length > 2 ? machineMasterIDVal.Substring(0, 2) : machineMasterIDVal;

                                newRow["Machine Description"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C").ToString();

                                string typeVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C").ToString();
                                newRow["Type"] = typeVal.Length > 200 ? typeVal.Substring(0, 200) : typeVal;

                                string measurementVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "C").ToString();
                                newRow["Measurement"] = measurementVal.Length > 200 ? measurementVal.Substring(0, 200) : measurementVal;

                                string directionFoldTypeVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "C").ToString();
                                newRow["Direction/Fold Type"] = directionFoldTypeVal.Length > 200 ? directionFoldTypeVal.Substring(0, 200) : directionFoldTypeVal;

                                newRow["Direction/Fold Desc"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "C").ToString();

                                string supplierPart1Val = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 11], "C").ToString();
                                newRow["Supplier Part#-1"] = supplierPart1Val.Length > 60 ? supplierPart1Val.Substring(0, 60) : supplierPart1Val;

                                string supplierBrand1Val = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 12], "C").ToString();
                                newRow["Supplier Brand-1"] = supplierBrand1Val.Length > 60 ? supplierBrand1Val.Substring(0, 60) : supplierBrand1Val;

                                string supplierPart2Val = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 13], "C").ToString();
                                newRow["Supplier Part#-2"] = supplierPart2Val.Length > 60 ? supplierPart2Val.Substring(0, 60) : supplierPart2Val;

                                string supplierBrand2Val = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 14], "C").ToString();
                                newRow["Supplier Brand-2"] = supplierBrand2Val.Length > 60 ? supplierBrand2Val.Substring(0, 60) : supplierBrand2Val;

                                string supplierPart3Val = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 15], "C").ToString();
                                newRow["Supplier Part#-3"] = supplierPart3Val.Length > 60 ? supplierPart3Val.Substring(0, 60) : supplierPart3Val;

                                string supplierBrand3Val = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 16], "C").ToString();
                                newRow["Supplier Brand-3"] = supplierBrand3Val.Length > 60 ? supplierBrand3Val.Substring(0, 60) : supplierBrand3Val;

                                string remarkVal = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 17], "C").ToString();
                                newRow["Remark"] = remarkVal.Length > 3000 ? remarkVal.Substring(0, 3000) : remarkVal;

                                // 必填欄位不為空才可以填入
                                if (!MyUtility.Check.Empty(sewingMachineAttachmentIDVal) && !MyUtility.Check.Empty(machineMasterIDVal) && !MyUtility.Check.Empty(typeVal)
                                    && !MyUtility.Check.Empty(measurementVal) && !MyUtility.Check.Empty(directionFoldTypeVal))
                                {
                                    this.grid2Data.Rows.Add(newRow);
                                }
                            }

                            #region 抓取Excel圖片

                            // 取得Sheet上的圖形集合
                            Shapes shapes = worksheet.Shapes;

                            // 當前Sheet是否有圖片
                            bool hasShape = shapes.Count > 0;

                            foreach (Microsoft.Office.Interop.Excel.Shape currentShape in shapes)
                            {
                                // 取得當前圖片，是在哪一個儲存格
                                Microsoft.Office.Interop.Excel.Range picRange = currentShape.TopLeftCell;

                                // picRange.Row的起點是1，加上Excel第一列是標題，所以要-2才能對應到DataTable第一列
                                int shapeRow = picRange.Row - 2;

                                // 判斷：(1)圖形所在的儲存格，跟目前讀取到的Row是不是一樣的，以及是不是放在第R(18)和S(19)欄  (2) 所有圖片都讀取完，就不再做
                                // Picture1
                                if (picRange.Column == 18)
                                {
                                    // 取得圖形
                                    currentShape.CopyPicture(XlPictureAppearance.xlScreen, XlCopyPictureFormat.xlBitmap);
                                    if (Clipboard.ContainsImage())
                                    {
                                        // 取得二進位資料
                                        byte[] imageData = null;
                                        using (MemoryStream imagestream = new MemoryStream())
                                        {
                                            Clipboard.GetImage().Save(imagestream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            imageData = imagestream.ToArray();
                                        }


                                        this.grid2Data.Rows[shapeRow]["Picture1_File"] = imageData;
                                        this.grid2Data.Rows[shapeRow]["Picture1"] = Guid.NewGuid() + "-1.jpg";
                                        this.grid2Data.Rows[shapeRow]["HasPicture1"] = true;
                                    }
                                }

                                // Picture2
                                if (picRange.Column == 19)
                                {
                                    // 取得圖形
                                    currentShape.CopyPicture(XlPictureAppearance.xlScreen, XlCopyPictureFormat.xlBitmap);
                                    if (Clipboard.ContainsImage())
                                    {
                                        // 取得二進位資料
                                        byte[] imageData = null;
                                        using (MemoryStream imagestream = new MemoryStream())
                                        {
                                            Clipboard.GetImage().Save(imagestream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            imageData = imagestream.ToArray();
                                        }

                                        this.grid2Data.Rows[shapeRow]["Picture2_File"] = imageData;
                                        this.grid2Data.Rows[shapeRow]["Picture2"] = Guid.NewGuid() + "-2.jpg";
                                        this.grid2Data.Rows[shapeRow]["HasPicture2"] = true;
                                    }
                                }
                            }
                            #endregion

                            dr["Status"] = "Check & Import Completed.";
                        }

                        excel.DisplayAlerts = false;
                        excel.Workbooks.Close();
                        excel.Quit();
                        excel = null;
                    }
                }
            }
            #endregion

            this.gridDetail.ResumeLayout();
        }

        // Write in
        private void BtnWriteIn_Click(object sender, EventArgs e)
        {
            if (this.grid2Data.Rows == null || this.grid2Data.Rows.Count == 0)
            {
                return;
            }

            List<SqlParameter> para = new List<SqlParameter>();

            foreach (DataRow dr in this.grid2Data.Rows)
            {
                byte[] picture1 = null;
                if (dr["Picture1_File"] != null && dr["Picture1_File"] != DBNull.Value)
                {
                    string fileName = dr["Picture1"].ToString();
                    picture1 = (byte[])dr["Picture1_File"];

                    // 轉成圖片
                    using (MemoryStream oMemoryStream = new MemoryStream(picture1))
                    {
                        System.Drawing.Image oImage = System.Drawing.Image.FromStream(oMemoryStream);
                        oImage.Save(this.destination_path + fileName);
                    }
                }

                byte[] picture2 = null;
                if (dr["Picture2_File"] != null && dr["Picture2_File"] != DBNull.Value)
                {
                    string fileName = dr["Picture2"].ToString();
                    picture2 = (byte[])dr["Picture2_File"];

                    // 轉成圖片
                    using (MemoryStream oMemoryStream = new MemoryStream(picture2))
                    {
                        System.Drawing.Image oImage = System.Drawing.Image.FromStream(oMemoryStream);
                        oImage.Save(this.destination_path + fileName);
                    }
                }
            }

            this.grid2Data.Columns.Remove("Picture1_File");
            this.grid2Data.Columns.Remove("Picture2_File");

            string sqlCmd = $@"-----選出符合條件可以塞的資料
select [ID]
    ,[Attachment Group]
    ,[Description]
    ,[Description(Chinese)]
    ,[Machine Master ID]
    ,[Type]
    ,[Measurement]
    ,FoldType = ISNULL( (select TOP 1 FoldType from AttachmentFoldType a where a.FoldType = t.[Direction/Fold Type])   ,'')
    ,[Supplier Part#-1]
    ,[Supplier Brand-1]
    ,[Supplier Part#-2]
    ,[Supplier Brand-2]
    ,[Supplier Part#-3]
    ,[Supplier Brand-3]
    ,[Remark]
    ,Picture1 = ISNULL([Picture1] ,'')
    ,Picture2 = ISNULL([Picture2] ,'')
    ,AddName = '{Sci.Env.User.UserID}'
    ,AddDate = GETDATE()
INTO #InsertData
from #tmp t
where not exists(
    select 1 
    from SewingMachineAttachment a
    where a.ID = t.[ID]
)----排除已存在ID
and not exists(
    select 1 from SewingMachineAttachment a
    where a.MachineMasterGroupID = t.[Machine Master ID] and a.AttachmentTypeID = t.[Type] and a.MeasurementID = t.[Measurement] and a.FoldTypeID = t.[Direction/Fold Type]
) ----MachineMasterGroupID + AttachmentTypeID + MeasurementID + FoldTypeID不可重複
AND exists(
    SELECT 1 FROM AttachmentFoldType a WHERE a.FoldType = t.[Direction/Fold Type] and Junk=0
)----排除FoldTypeID 不存在
AND exists(
    SELECT 1 FROM AttachmentMeasurement a WHERE a.Measurement = t.[Measurement] and Junk=0
)----排除Measurement 不存在
AND exists(
    SELECT 1 FROM SciMachine_MachineMasterGroup a WHERE a.ID = t.[Machine Master ID]  and Junk=0
)----排除Machine 不存在
AND exists(
    SELECT 1 FROM AttachmentType a WHERE a.Type = t.[Type]  and Junk=0
)----排除Type 不存在
AND t.[ID] = t.[Machine Master ID] + '-' + t.[Type] + '-' + t.[Measurement] + '-' + t.[Direction/Fold Type]
---- ID 的規則是 MachineMasterGroupID、AttachmentTypeID、MeasurementID、FoldTypeID四個欄位用 - 串起來



-----找出不能塞的資料
select *
    ,ErrMsg1 = CASE WHEN exists(
                        select 1 
                        from SewingMachineAttachment a
                        where a.ID = t.[ID]
                    ) THEN 'ID Existed.' 
                    WHEN exists(
                        select 1 from SewingMachineAttachment a
                        where a.MachineMasterGroupID = t.[Machine Master ID] and a.AttachmentTypeID = t.[Type] and a.MeasurementID = t.[Measurement] and a.FoldTypeID = t.[Direction/Fold Type]
                    ) THEN 'ID Existed.'
                    WHEN not exists(
                        SELECT 1 FROM AttachmentFoldType a WHERE a.FoldType = t.[Direction/Fold Type] and Junk=0
                    ) THEN 'FoldType not Existed.'
                    WHEN not exists(
                        SELECT 1 FROM AttachmentMeasurement a WHERE a.Measurement = t.[Measurement] and Junk=0
                    ) THEN 'Measurement not Existed.'
                    WHEN not exists(
                        SELECT 1 FROM SciMachine_MachineMasterGroup a WHERE a.ID = t.[Machine Master ID]  and Junk=0
                    ) THEN 'Machine not Existed.'
                    WHEN not exists(
                        SELECT 1 FROM AttachmentType a WHERE a.Type = t.[Type]  and Junk=0
                    ) THEN 'Attachment Type not Existed.'

                    WHEN t.[ID] != t.[Machine Master ID] + '-' + t.[Type] + '-' + t.[Measurement] + '-' + t.[Direction/Fold Type] 
                      THEN 'ID does not fit to (machine + type + measurement + fold type)'

                    ELSE '' 
                END
INTO #FailData
from #tmp t



-----開始塞SewingMachineAttachment
INSERT INTO dbo.SewingMachineAttachment
           (ID,MoldID,Description,DescriptionCN,MachineMasterGroupID,AttachmentTypeID,MeasurementID,FoldTypeID,Supplier1PartNo,Supplier1BrandID
           ,Supplier2PartNo,Supplier2BrandID,Supplier3PartNo,Supplier3BrandID,Remark,Picture1,Picture2,AddName,AddDate)
select *
from #InsertData

select *
from #FailData
drop table #InsertData ,#FailData
";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.grid2Data, string.Empty, sqlCmd, out System.Data.DataTable dt);

            dt.Columns.Add("Picture1_File", typeof(byte[]));
            dt.Columns.Add("Picture2_File", typeof(byte[]));
            this.grid2Data.Columns.Add("Picture1_File", typeof(byte[]));
            this.grid2Data.Columns.Add("Picture2_File", typeof(byte[]));
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["ErrMsg"] = row["ErrMsg1"];
                }

                dt.Columns.Remove("ErrMsg1");

                this.grid2Data = dt;
                this.listControlBindingSource2.DataSource = this.grid2Data;
                this.gridDetail.DataSource = this.listControlBindingSource2;
                MyUtility.Msg.InfoBox("Success!!");
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
