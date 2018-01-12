using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B42_BatchImport
    /// </summary>
    public partial class B42_BatchImport : Sci.Win.Subs.Base
    {
        private DataTable dt;

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
            DBProxy.Current.Select(null, "select CustomSP,VNContractID,ID,StyleID,SeasonID,SizeCode,NLCode='',Qty=0.0000,Remark='' ,checkS=0 from VNConsumption where 1=0", out this.dt);
            this.Helper.Controls.Grid.Generator(this.gridBatchImport)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("VNContractID", header: "Contract Id", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ID", header: "ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("NLCode", header: "NLCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(9), integer_places: 12, decimal_places: 4, iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true);
            this.listControlBindingSource1.DataSource = this.dt;
        }

        private void Btnselectfile_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xls)|*.xls");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            // 刪除表身Grid資料
            if (this.dt != null && this.dt.Rows.Count > 0)
            {
                this.dt.Clear();
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

                DataRow newRow = this.dt.NewRow();
                string nLCode = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"));
                string contractID = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C"));
                string b43check = string.Format("select 1 from VNContract_Detail with(nolock) where id = '{0}' and NLCode = '{1}'", contractID, nLCode);
                string remark = string.Empty;
                if (!MyUtility.Check.Seek(b43check))
                {
                    remark = "NLCode not found in Contract. ";
                }

                string customSP = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C"));
                string b42check = string.Format(@"select * from VNConsumption  with(nolock) where VNContractID = '{0}' and CustomSP = '{1}'", contractID, customSP);
                DataRow drc;
                if (!MyUtility.Check.Seek(b42check, out drc))
                {
                    remark += "Custom SP & Contract not found.";
                }
                else
                {
                    newRow["ID"] = drc["ID"].ToString().Trim();
                    newRow["StyleID"] = drc["StyleID"].ToString().Trim();
                    newRow["SeasonID"] = drc["SeasonID"].ToString().Trim();
                    newRow["SizeCode"] = drc["SizeCode"].ToString().Trim();
                }

                newRow["remark"] = remark;
                newRow["CustomSP"] = customSP.ToString().Trim();
                newRow["VNContractID"] = contractID.ToString().Trim();
                newRow["NLCode"] = nLCode.ToString().Trim();
                newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "N");

                this.dt.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            DataTable distinctCustomSP = this.dt.DefaultView.ToTable(true, new string[] { "CustomSP" });
            this.numericttlsp.Value = distinctCustomSP.Rows.Count;
            this.numericdetailrecord.Value = this.dt.Rows.Count;

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataRow[] drs = this.dt.Select("remark = ''");
            DualResult drt;
            string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            StringBuilder idu = new StringBuilder();
            foreach (DataRow dr in drs)
            {
                string customSP = MyUtility.Convert.GetString(dr["CustomSP"]);
                string vnContractID = MyUtility.Convert.GetString(dr["VNContractID"]);
                string nLCode = MyUtility.Convert.GetString(dr["NLCode"]);
                string chk = string.Format("select 1 from VNConsumption v inner join VNConsumption_Detail vd on v.id = vd.id where v.CustomSP = '{0}' and v.VNContractID = '{1}' and vd.NLCode = '{2}'", customSP, vnContractID, nLCode);
                if (!MyUtility.Check.Seek(chk))
                {
                    idu.Append(string.Format(
                        @"
insert into VNConsumption_Detail(ID,NLCode,HSCode,UnitID,Qty,UserCreate,SystemQty,Waste)
select '{2}','{1}',a.HSCode,a.UnitID,'{3}',1,'{3}',
Waste = (select Waste = (select MAX( [dbo].[getWaste]( v.StyleID,v.BrandID,v.SeasonID,v.VNContractID, '{1}')))
		from VNConsumption v  WITH (NOLOCK)
		where v.ID	   = '{2}')
from VNContract_Detail a WITH (NOLOCK)
where a.ID = '{0}' and a.NLCode = '{1}';",
                        vnContractID,
                        nLCode,
                        dr["id"].ToString(),
                        dr["Qty"].ToString()));

                    dr["checkS"] = 1;
                }
                else
                {
                    idu.Append(string.Format(
                        @"
update VNConsumption_Detail 
set qty = '{0}'
,UserCreate = 1
where id = '{1}' and NLCode = '{2}';",
                        dr["Qty"].ToString(),
                        dr["id"].ToString(),
                        dr["NLCode"].ToString(),
                        customSP));

                    idu.Append(string.Format(
                        @"update VNConsumption set EditName = '{0}',EditDate = '{1}' where CustomSP = '{2}' and VNContractID = '{3}' ;",
                        Sci.Env.User.UserID,
                        datetime,
                        customSP,
                        vnContractID));

                    dr["checkS"] = 1;
                }
            }

            DataTable distinct = this.dt.DefaultView.ToTable(true, new string[] { "CustomSP", "VNContractID" });
            foreach (DataRow dr in distinct.Rows)
            {
                string d = string.Format(
                    @"select vd.NLCode 
from VNConsumption v,VNConsumption_Detail vd
where v.id = vd.id
and v.VNContractID = '{0}' and v.CustomSP = '{1}'",
                    dr["VNContractID"].ToString(),
                    dr["CustomSP"].ToString());

                DataTable dlt;

                // 根據VNContractID,CustomSP去找DB內detail有的NLCode
                drt = DBProxy.Current.Select(null, d, out dlt);
                if (!drt)
                {
                    MyUtility.Msg.ErrorBox("Insert/Update datas error!");
                    return;
                }

                // 找Excel匯入資料有沒有這NLCode
                foreach (DataRow dn in dlt.Rows)
                {
                    DataRow[] drN = this.dt.Select(string.Format(
                        "remark = ''and VNContractID = '{0}' and CustomSP = '{1}' and NLCode = '{2}'",
                        dr["VNContractID"].ToString(),
                        dr["CustomSP"].ToString(),
                        dn["NLCode"].ToString()));

                    if (drN.Length == 0)
                    {
                        DataRow[] dra = this.dt.Select(string.Format(
                            "remark = '' and VNContractID = '{0}' and CustomSP = '{1}'",
                            dr["VNContractID"].ToString(),
                            dr["CustomSP"].ToString()));

                        if (dra.Length != 0)
                        {
                            idu.Append(string.Format(
                                "delete VNConsumption_Detail where id = '{0}' and NLCode ='{1}';",
                                dra[0]["ID"].ToString(),
                                dn["NLCode"].ToString()));
                        }
                    }
                }
            }

            // 如果沒資料update/insert 就不進去
            if (!MyUtility.Check.Empty(idu.ToString()))
            {
                drt = DBProxy.Current.Execute(null, idu.ToString());
                if (!drt)
                {
                    MyUtility.Msg.ErrorBox("Insert/Update datas error!");
                }
                else
                {
                    DataRow[] drsf = this.dt.Select("remark <> ''");
                    this.numericFail.Value = drsf.Length;
                    DataTable distinctchk = this.dt.DefaultView.ToTable(true, new string[] { "CustomSP", "checkS" });
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
    }
}
