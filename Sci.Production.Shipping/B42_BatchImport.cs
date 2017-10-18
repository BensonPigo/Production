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
            DBProxy.Current.Select(null, "select CustomSP,VNContractID,ID,StyleID,SeasonID,SizeCode,NLCode='',Qty=0.0000,Remark='' ,checkS=0 from VNConsumption where 1=0", out dt);
            Helper.Controls.Grid.Generator(this.gridBatchImport)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("VNContractID", header: "Contract Id", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ID", header: "ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("NLCode", header: "NLCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(9), integer_places: 12, decimal_places: 4, iseditingreadonly: true)
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
            if (dt !=null && dt.Rows.Count>0)
            {
                dt.Clear();
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
            int intRowsRead = intRowsStart - 1;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;

                range = worksheet.Range[String.Format("A{0}:F{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = dt.NewRow();
                string NLCode = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"));
                string ContractID = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C"));
                string B43check = string.Format("select 1 from VNContract_Detail with(nolock) where id = '{0}' and NLCode = '{1}'", ContractID, NLCode);
                string remark = "";
                if (!MyUtility.Check.Seek(B43check))
                {
                    remark = "NLCode not found in Contract. ";
                }
                string CustomSP = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C"));
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
            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            DataRow[] drs = dt.Select("remark = ''");
            DualResult drt;
            string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            StringBuilder idu = new StringBuilder();
            foreach (DataRow dr in drs)
            {
                string CustomSP = MyUtility.Convert.GetString(dr["CustomSP"]);
                string VNContractID = MyUtility.Convert.GetString(dr["VNContractID"]);
                string NLCode = MyUtility.Convert.GetString(dr["NLCode"]);
                string chk = string.Format("select 1 from VNConsumption v inner join VNConsumption_Detail vd on v.id = vd.id where v.CustomSP = '{0}' and v.VNContractID = '{1}' and vd.NLCode = '{2}'", CustomSP, VNContractID, NLCode);
                if (!MyUtility.Check.Seek(chk))
                {
                    idu.Append(string.Format(@"
insert into VNConsumption_Detail(ID,NLCode,HSCode,UnitID,Qty,UserCreate,SystemQty,Waste)
select '{2}','{1}',a.HSCode,a.UnitID,'{3}',1,'{3}',isnull(b.Waste,0)
from VNContract_Detail a WITH (NOLOCK)
left join View_VNNLCodeWaste b  WITH (NOLOCK) on a.NLCode = b.NLCode
where a.ID = '{0}' and a.NLCode = '{1}'
;"
                        , VNContractID, NLCode, dr["id"].ToString(), dr["Qty"].ToString()));
                    dr["checkS"] = 1;
                }
                else
                {
                    idu.Append(string.Format(@"update VNConsumption_Detail set qty = '{0}',UserCreate = 1,Waste = isnull((select Waste from View_VNNLCodeWaste where  NLCode = '{2}' ),0)
                                                where id = '{1}' and NLCode = '{2}';", dr["Qty"].ToString(), dr["id"].ToString(), dr["NLCode"].ToString()));
                    idu.Append(string.Format(@"update VNConsumption set EditName = '{0}',EditDate = '{1}' where CustomSP = '{2}' and VNContractID = '{3}' ;", Sci.Env.User.UserID, datetime, CustomSP, VNContractID));
                    dr["checkS"] = 1;
                }
            }
            DataTable Distinct = dt.DefaultView.ToTable(true, new string[] { "CustomSP", "VNContractID" });
            foreach (DataRow dr in Distinct.Rows)
            {
                string d = string.Format(@"select vd.NLCode 
from VNConsumption v,VNConsumption_Detail vd
where v.id = vd.id
and v.VNContractID = '{0}' and v.CustomSP = '{1}'", dr["VNContractID"].ToString(), dr["CustomSP"].ToString());
                DataTable dlt;
                drt = DBProxy.Current.Select(null, d, out dlt);//根據VNContractID,CustomSP去找DB內detail有的NLCode
                if (!drt)
                {
                    MyUtility.Msg.ErrorBox("Insert/Update datas error!");
                    return;
                }
                foreach (DataRow dn in dlt.Rows)//找Excel匯入資料有沒有這NLCode
                {
                    DataRow[] drN = dt.Select(string.Format("remark = ''and VNContractID = '{0}' and CustomSP = '{1}' and NLCode = '{2}'"
                        , dr["VNContractID"].ToString(), dr["CustomSP"].ToString(), dn["NLCode"].ToString()));
                    if (drN.Length == 0)
                    {
                        DataRow[] dra = dt.Select(string.Format("remark = '' and VNContractID = '{0}' and CustomSP = '{1}'"
                            , dr["VNContractID"].ToString(), dr["CustomSP"].ToString()));
                        if (dra.Length !=0)
                        {
                            idu.Append(string.Format("delete VNConsumption_Detail where id = '{0}' and NLCode ='{1}';"
                            , dra[0]["ID"].ToString(), dn["NLCode"].ToString()));
                        }                        
                    }
                }
            }

            drt = DBProxy.Current.Execute(null, idu.ToString());
            if (!drt)
            {
                MyUtility.Msg.ErrorBox("Insert/Update datas error!");
            }
            else
            {
                DataRow[] drsf = dt.Select("remark <> ''");
                numericFail.Value = drsf.Length;
                DataTable Distinctchk = dt.DefaultView.ToTable(true, new string[] { "CustomSP", "checkS" });
                DataRow[] drs2 = Distinctchk.Select("checkS = 1");
                numericSucessSP.Value = drs2.Length;
                MyUtility.Msg.InfoBox("Complete!!");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
