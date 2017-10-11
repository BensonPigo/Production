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
    public partial class B42_BatchImport : Sci.Win.Subs.Base
    {
        DataTable dt;
        public B42_BatchImport()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DBProxy.Current.Select(null, "select CustomSP,VNContractID,ID,StyleID,SeasonID,SizeCode,NLCode='',Qty,Remark='' from VNConsumption where 1=0", out dt);
            Helper.Controls.Grid.Generator(this.gridBatchImport)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("VNContractID", header: "Contract Id", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ID", header: "ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("NLCode", header: "NLCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(9), decimal_places: 9999, iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true);
            listControlBindingSource1.DataSource = dt;
        }
        private void btnselectfile_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xls)|*.xls");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            //刪除表身Grid資料
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null) return;

            StringBuilder errNLCode = new StringBuilder();

            this.ShowWaitMessage("Starting EXCEL...");
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsCount = worksheet.UsedRange.Rows.Count;
            int intColumnsCount = worksheet.UsedRange.Columns.Count;
            int intRowsStart = 2;
            int intRowsRead = intRowsStart - 0;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;

                range = worksheet.Range[String.Format("A{0}:F{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = dt.NewRow();
                string NLCode = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"));
                string ContractID = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C"));
                string B43check = string.Format("select 1 from VNContract_Detail with(nolock) where id = '{0}' and NLCode = '{1}'", ContractID, NLCode);
                string remark = "";
                if (!MyUtility.Check.Seek(B43check))
                {
                    remark = "NLCode not found in Contract";
                }
                string CustomSP = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C"));
                string B42check = string.Format(@"select * from VNConsumption  with(nolock) where VNContractID = '{0}' and CustomSP = '{1}'", ContractID, CustomSP);
                DataRow drc;
                if (!MyUtility.Check.Seek(B42check, out drc))
                {
                    remark += "Custom SP & Contract not found.";
                }
                else
                {
                    newRow["ID"] = drc["ID"];
                    newRow["StyleID"] = drc["StyleID"];
                    newRow["SeasonID"] = drc["SeasonID"];
                    newRow["SizeCode"] = drc["SizeCode"];
                }
                newRow["remark"] = remark;
                newRow["CustomSP"] = CustomSP;
                newRow["VNContractID"] = ContractID;
                newRow["NLCode"] = NLCode;
                newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "N");

                dt.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            DataTable DistinctCustomSP = dt.DefaultView.ToTable(true, new string[] { "CustomSP" });
            numericttlsp.Value = DistinctCustomSP.Rows.Count;
            numericdetailrecord.Value = dt.Rows.Count;

            this.HideWaitMessage();
            /////
            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //DataTable dt2, dt3;
            DataRow[] drs = dt.Select("remark = ''");
            DualResult drt;
            //dt2 = dt.Clone();
            //foreach (DataRow drr in drs)
            //{
            //    dt2.ImportRow(drr);
            //}
            //           dr = MyUtility.Tool.ProcessWithDatatable(dt2, "", @"
            //merge VNConsumption_Detail t
            //using(
            //	select t.*,vd.HSCode,vd.UnitID
            //	from #tmp t,VNContract_Detail vd
            //	where vd.ID = t.VNContractID and vd.NLCode = t.NLCode
            //)s
            //on t.id = s.id and t.NLCode = s.NLCode
            //when matched then
            //	update set
            //	t.qty = s.qty
            //when not matched then
            //	insert(id,NLCode,HSCode,UnitID,Qty,UserCreate,SystemQty)
            //	values(s.id,s.NLCode,s.HSCode,s.UnitID,s.Qty,1,0)
            //when not matched by source and t.id = s.id then
            //	delete;
            //", out dt3);
            //            if (!dr)
            //            {
            //                MyUtility.Msg.ErrorBox("Insert/Update datas error!");
            //            }
            //            else
            //            {
            //                MyUtility.Msg.InfoBox("Complete!!");
            //            }

            StringBuilder idu = new StringBuilder();
            int c = 0,c2=0;
            foreach (DataRow dr in drs)
            {
                string CustomSP = MyUtility.Convert.GetString(dr["CustomSP"]);
                string VNContractID = MyUtility.Convert.GetString(dr["VNContractID"]);
                string NLCode = MyUtility.Convert.GetString(dr["NLCode"]);
                string chk = string.Format("select 1 from VNConsumption v inner join VNConsumption_Detail vd on v.id = vd.id where v.CustomSP = '{0}' and v.VNContractID = '{1}' and vd.NLCode = '{2}'", CustomSP, VNContractID, NLCode);
                if (!MyUtility.Check.Seek(chk))
                {
                    idu.Append(string.Format(@"
insert into VNConsumption_Detail
select '{2}','{1}',HSCode,UnitID,'{3}',1,0
from VNContract_Detail
where ID = '{0}' and NLCode = '{1}'
;"
                        , VNContractID, NLCode, dr["id"].ToString(), dr["Qty"].ToString()));
                    c++;
                }
                else
                {
                    idu.Append(string.Format(@"update VNConsumption_Detail set qty = '{0}' where id = '{1}' and NLCode = '{2}';", dr["Qty"].ToString(), dr["id"].ToString(), dr["NLCode"].ToString()));
                    c++;
                }
            }

            drt = DBProxy.Current.Execute(null, idu.ToString());
            if (!drt)
            {
                MyUtility.Msg.ErrorBox("Insert/Update datas error!");
            }
            else
            {
                MyUtility.Msg.InfoBox("Complete!!");
                DataRow[] drsf = dt.Select("remark <> ''");
                numericFail.Value = drsf.Length;
                numericSucessSP.Value = c;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
