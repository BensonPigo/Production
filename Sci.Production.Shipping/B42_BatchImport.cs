using System;
using System.Data;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using Sci.Production.PublicPrg;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B42_BatchImport
    /// </summary>
    public partial class B42_BatchImport : Win.Subs.Base
    {
        private DataTable dtBatchImport;

        /// <summary>
        /// B42_BatchImport
        /// </summary>
        public B42_BatchImport()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlBatchImportColumn =
                @"
select  CustomSP,
        VNContractID,
        ID,
        StyleID,
        SeasonID,
        SizeCode,
        [SCIRefno] = '',
        [Refno] = '',
        [Type] = '',
        [NLCode]='',
        [LocalItem]= 0,
        [FabricBrandID]='',
        [Qty]=0.0000,
        [UserCreate]='',
        [StockQty]=0.0000,
        [StockUnit]='',
        [HSCode]='',
        [UnitID]='',
        Remark='' ,
        checks=0 ,
        [UsageQty]=0.0000,
        [UsageUnit]=''
from VNConsumption where 1=0";

            DBProxy.Current.Select(null, sqlBatchImportColumn, out this.dtBatchImport);

            this.Helper.Controls.Grid.Generator(this.gridBatchImport)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("VNContractID", header: "Contract Id", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ID", header: "ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("UsageUnit", header: "Usage Unit", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Numeric("StockQty", header: "Qty", width: Widths.AnsiChars(9), integer_places: 12, decimal_places: 4, iseditingreadonly: true)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(100), iseditingreadonly: true);
            this.listControlBindingSource1.DataSource = this.dtBatchImport;
        }

        private void Btnselectfile_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xls,*.xlsx)|*.xls;*.xlsx");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            // 刪除表身Grid資料
            if (this.dtBatchImport != null && this.dtBatchImport.Rows.Count > 0)
            {
                this.dtBatchImport.Clear();
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null)
            {
                return;
            }

            StringBuilder errNLCode = new StringBuilder();

            this.ShowWaitMessage("Starting EXCEL...");
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsCount = worksheet.UsedRange.Rows.Count;
            int intColumnsCount = worksheet.UsedRange.Columns.Count;
            int intRowsStart = 2;
            int intRowsRead = intRowsStart - 1;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;

                range = worksheet.Range[string.Format("A{0}:F{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = this.dtBatchImport.NewRow();
                string refno = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"));
                string type = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C"));
                string usageQty = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "N"));
                string customSP = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C"));
                string contractID = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C"));
                string usageUnit = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C"));
                string remark = string.Empty;

                // 匯入的檔案ContractID 是SubconIn 就提示訊息不能匯入
                DataRow drCheck;
                string checkIsSubconIn = $@"select IsSubconIn from VNContract where id ='{contractID}'";
                if (MyUtility.Check.Seek(checkIsSubconIn, out drCheck))
                {
                    if (!MyUtility.Check.Empty(drCheck["IsSubconIn"]))
                    {
                        MyUtility.Msg.WarningBox("Cannot import [Contract ID] which is [Subcon In]");
                        this.HideWaitMessage();
                        return;
                    }
                }

                string b42checkSQL = string.Format(@"select * from VNConsumption  with(nolock) where VNContractID = '{0}' and CustomSP = '{1}'", contractID, customSP);
                DataRow drc;
                bool isExistsB42 = MyUtility.Check.Seek(b42checkSQL, out drc);
                if (isExistsB42)
                {
                    newRow["ID"] = drc["ID"].ToString().Trim();
                    newRow["StyleID"] = drc["StyleID"].ToString().Trim();
                    newRow["SeasonID"] = drc["SeasonID"].ToString().Trim();
                    newRow["SizeCode"] = drc["SizeCode"].ToString().Trim();

                    DataRow drNLCode;

                    if (type != "F" && type != "A" && type != "L" && type != "Misc")
                    {
                        remark += "Type is wrong." + Environment.NewLine;
                    }
                    else
                    {
                        drNLCode = Prgs.GetNLCodeDataByRefno(refno, usageQty, drc["BrandID"].ToString(), type, usageUnit: usageUnit);

                        if (drNLCode == null)
                        {
                            remark += "Refno not found." + Environment.NewLine + "Fabric / Accessory need input usage unit." + Environment.NewLine;
                        }
                        else if (MyUtility.Check.Empty(drNLCode["NLCode"]))
                        {
                            remark += "NLCode is not maintained." + Environment.NewLine;
                        }
                        else if (MyUtility.Check.Empty(drNLCode["StockUnit"]))
                        {
                            remark += "StockUnit is not found." + Environment.NewLine;
                        }
                        else
                        {
                            newRow["NLCode"] = drNLCode["NLCode"];
                            newRow["Qty"] = drNLCode["Qty"];
                            newRow["StockQty"] = drNLCode["StockQty"];
                            newRow["SCIRefno"] = drNLCode["SCIRefno"];
                            newRow["HSCode"] = drNLCode["HSCode"];
                            newRow["UnitID"] = drNLCode["UnitID"];
                            newRow["StockUnit"] = drNLCode["StockUnit"];
                            newRow["FabricBrandID"] = drNLCode["FabricBrandID"];
                            newRow["UsageUnit"] = drNLCode["UsageUnit"];
                        }
                    }
                }
                else
                {
                    remark += "Custom SP & Contract not found." + Environment.NewLine;
                }

                newRow["remark"] = remark;
                newRow["CustomSP"] = customSP.ToString().Trim();
                newRow["VNContractID"] = contractID.ToString().Trim();
                newRow["Type"] = type;
                newRow["Refno"] = refno;
                newRow["UsageQty"] = usageQty;
                newRow["UsageUnit"] = usageUnit;
                newRow["checks"] = 0;

                this.dtBatchImport.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            DataTable distinctCustomSP = this.dtBatchImport.DefaultView.ToTable(true, new string[] { "CustomSP" });
            this.numericttlsp.Value = distinctCustomSP.Rows.Count;
            this.numericdetailrecord.Value = this.dtBatchImport.Rows.Count;

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataRow[] drImportList = this.dtBatchImport.Select("remark = ''");
            DualResult drResult;
            string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            StringBuilder idu = new StringBuilder();

            #region 檢查ID,NLCode,HSCode,UnitID Group後是否有ID,NLCode重複的資料
            bool isVNConsumption_Detail_DetailHasDupData = !Prgs.CheckVNConsumption_Detail_Dup(drImportList, true);
            if (isVNConsumption_Detail_DetailHasDupData)
            {
                return;
            }
            #endregion

            foreach (DataRow dr in drImportList)
            {
                string customSP = MyUtility.Convert.GetString(dr["CustomSP"]);
                string vnContractID = MyUtility.Convert.GetString(dr["VNContractID"]);
                string id = MyUtility.Convert.GetString(dr["ID"]);
                string nLCode = MyUtility.Convert.GetString(dr["NLCode"]);
                string sCIRefno = MyUtility.Convert.GetString(dr["SCIRefno"]).Replace("'", "''");
                string refno = MyUtility.Convert.GetString(dr["Refno"]).Replace("'", "''");
                string localItem = dr["Type"].Equals("L") ? "1" : "0";
                string fabricBrandID = MyUtility.Convert.GetString(dr["FabricBrandID"]);
                string fabricType = MyUtility.Convert.GetString(dr["Type"]);
                string qty = MyUtility.Convert.GetString(dr["Qty"]);
                string stockQty = MyUtility.Convert.GetString(dr["StockQty"]);
                string stockUnit = MyUtility.Convert.GetString(dr["StockUnit"]);
                string hSCode = MyUtility.Convert.GetString(dr["HSCode"]);
                string unitID = MyUtility.Convert.GetString(dr["UnitID"]);
                string usageQty = MyUtility.Convert.GetString(dr["UsageQty"]);
                string usageUnit = MyUtility.Convert.GetString(dr["UsageUnit"]);

                string chk = $@"
select 1    from VNConsumption v 
            inner join VNConsumption_Detail_Detail vdd on v.id = vdd.id 
            where v.CustomSP = '{customSP}' and v.VNContractID = '{vnContractID}' and vdd.Refno = '{refno}' and vdd.SCIRefno = '{sCIRefno}' and vdd.NLCode = '{nLCode}'";

                bool isExistsVNConsumption = MyUtility.Check.Seek(chk);
                if (!isExistsVNConsumption)
                {
                    idu.Append(
                        $@"
insert into VNConsumption_Detail_Detail(ID,NLCode,SCIRefno,Refno,Qty,LocalItem,FabricBrandID,FabricType,SystemQty,UserCreate,StockQty,StockUnit,HSCode,UnitID, UsageQty, UsageUnit)
                values('{id}','{nLCode}','{sCIRefno}','{refno}',{qty},{localItem},'{fabricBrandID}','{fabricType}',0,1,{stockQty},'{stockUnit}','{hSCode}','{unitID}',{usageQty},'{usageUnit}');");

                    dr["checkS"] = 1;
                }
                else
                {
                    idu.Append(
                        $@"
update VNConsumption_Detail_Detail 
set 
StockQty = {stockQty}
,Qty = {qty}
,FabricBrandID = '{fabricBrandID}'
,FabricType = '{fabricType}'
,StockUnit = '{stockUnit}'
,HSCode = '{hSCode}'
,UnitID = '{unitID}'
,UserCreate = 1
,UsageQty = {usageQty}
,UsageUnit = '{usageUnit}'
where   id = '{id}' and 
        Refno = '{refno}';
");

                    idu.Append(string.Format(
                        @"update VNConsumption set EditName = '{0}',EditDate = '{1}' where CustomSP = '{2}' and VNContractID = '{3}' ;",
                        Env.User.UserID,
                        datetime,
                        customSP,
                        vnContractID));

                    dr["checkS"] = 1;
                }
            }

            // 刪除VNConsumption_Detail_Detail不存在於本次import的資料
            DataTable distinct = this.dtBatchImport.DefaultView.ToTable(true, new string[] { "CustomSP", "VNContractID" });
            var srcCheckBatchImport = this.dtBatchImport.AsEnumerable();
            foreach (DataRow dr in distinct.Rows)
            {
                string sqlGetVNConsumption_Detail_Detail = string.Format(
                    @"select v.ID, vdd.Refno,vdd.SCIRefno,vdd.NLCode
from VNConsumption v,VNConsumption_Detail_Detail vdd
where v.id = vdd.id
and v.VNContractID = '{0}' and v.CustomSP = '{1}'",
                    dr["VNContractID"].ToString(),
                    dr["CustomSP"].ToString());

                DataTable dtVNConsumption_Detail_Detail;

                // 根據VNContractID,CustomSP去找DB內detail有的NLCode
                drResult = DBProxy.Current.Select(null, sqlGetVNConsumption_Detail_Detail, out dtVNConsumption_Detail_Detail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                    return;
                }

                // 找Excel匯入資料有沒有這NLCode
                foreach (DataRow dn in dtVNConsumption_Detail_Detail.Rows)
                {
                    // fix Data 不刪除
                    bool isFixData = MyUtility.Check.Seek($"select 1 from VNFixedDeclareItem where Refno = '{dn["Refno"].ToString().Replace("'", "''")}'");
                    if (MyUtility.Check.Empty(dn["SCIRefno"]) && isFixData)
                    {
                        continue;
                    }

                    bool isExistsImportData = srcCheckBatchImport.Where(s => s["ID"].Equals(dn["ID"]) &&
                                                                             s["Refno"].Equals(dn["Refno"]) &&
                                                                             s["SCIRefno"].Equals(dn["SCIRefno"]) &&
                                                                             s["NLCode"].Equals(dn["NLCode"]) &&
                                                                             MyUtility.Check.Empty(s["remark"])).Any();

                    if (!isExistsImportData)
                    {
                        idu.Append(string.Format(
                            "delete VNConsumption_Detail_Detail where id = '{0}' and Refno ='{1}' and SCIRefno = '{2}' and NLCode = '{3}';" + Environment.NewLine,
                            dn["ID"].ToString(),
                            dn["Refno"].ToString().Replace("'", "''"),
                            dn["SCIRefno"].ToString().Replace("'", "''"),
                            dn["NLCode"].ToString()));
                    }
                }
            }

            // 如果沒資料update/insert 就不進去
            if (!MyUtility.Check.Empty(idu.ToString()))
            {
                // 產生VNConsumption_Detail的資料 呼叫CreateVNConsumption_Detail
                string[] distinctID = this.dtBatchImport.AsEnumerable().Where(s => (int)s["checks"] == 1).Select(s => s["ID"].ToString()).Distinct().ToArray();
                foreach (string id in distinctID)
                {
                    idu.Append($"exec CreateVNConsumption_Detail '{id}'");
                }

                drResult = DBProxy.Current.Execute(null, idu.ToString());
                if (!drResult)
                {
                    this.ShowErr("Insert/Update datas error!", drResult);
                }
                else
                {
                    DataRow[] drsf = this.dtBatchImport.Select("remark <> ''");
                    this.numericFail.Value = drsf.Length;
                    DataTable distinctchk = this.dtBatchImport.DefaultView.ToTable(true, new string[] { "CustomSP", "checkS" });
                    DataRow[] drs2 = distinctchk.Select("checkS = 1");
                    this.numericSucessSP.Value = drs2.Length;
                    MyUtility.Msg.InfoBox("Complete!!");
                }
            }
            else
            {
                MyUtility.Msg.InfoBox("No data import success!");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnDownloadExcel_Click(object sender, EventArgs e)
        {
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_B42_Batch Import.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }
    }
}
