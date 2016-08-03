using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R25 : Sci.Win.Tems.PrintForm
    {
        public R25()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }
        protected override bool ValidateInput()
        {
            if (!this.dateRange1.Text1.Empty())
            {
            
                MyUtility.Msg.ErrorBox("[Receive Date]  one of the inputs must be selected");
                dateRange1.Focus();
                return false;
            }
            if (!this.dateRange1.Text2.Empty())
            {

                MyUtility.Msg.ErrorBox("[Receive Date]  one of the inputs must be selected");
                dateRange1.Focus();
                return false;
            }
            if (!this.textBox1.Text.Empty())
            {

                MyUtility.Msg.ErrorBox("[sp]  one of the inputs must be selected");
                textBox1.Focus();
                return false;
            }

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DateTime? ReceiveDate = dateRange1.Value1;
            DateTime? ReceiveDate2 = dateRange1.Value2;
            string SP = textBox1.Text.ToString();
            string Refno = textBox2.Text.ToString();
            string Category = txtartworktype_fty1.Text.ToString();
            string Supplier = txtsubcon1.TextBox1.Text;
            string Factory = comboBox1.SelectedItem.ToString();

            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string order = "";
            List<string> sqlWheres = new List<string>();
            if (!this.dateRange1.Value1.Empty() && !this.dateRange1.Value2.Empty())
            {
                sqlWheres.Add("lr.issuedate between @ReceiveDate and @ReceiveDate2");
                lis.Add(new SqlParameter("@ReceiveDate", ReceiveDate));
                lis.Add(new SqlParameter("@ReceiveDate2", ReceiveDate2));
            }
            if (!this.textBox1.Text.Empty())
            {
                sqlWheres.Add("lrd.orderid=@SP");
                lis.Add(new SqlParameter("@SP", SP));
            }
            if (!this.textBox2.Text.Empty())
            {
                sqlWheres.Add("lrd.refno=@Refno");
                lis.Add(new SqlParameter("@Refno", Refno));
            }
            if (!this.txtartworktype_fty1.Text.Empty())
            {
                sqlWheres.Add("lrd.category=@Category");
                lis.Add(new SqlParameter("@Category", Category));
            }
            if (!this.txtsubcon1.Text.Empty())
            {
                sqlWheres.Add("lr.localsuppid=@Supplier");
                lis.Add(new SqlParameter("@Supplier", Supplier));
            }
            if (!this.comboBox1.Text.Empty())
            {
                sqlWheres.Add("lr.factoryid =@Factory");
                lis.Add(new SqlParameter("@Factory", Factory));
            }
            order = "order by lr.issuedate,lr.id";
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            DualResult result;
            DataTable dtt;
            string sqlcmd = string.Format(@"select lr.FactoryId [Factory] 
                    ,lr.Id [ID]
                    ,lr.IssueDate [Receive_Date]
                    ,lr.LocalSuppID [Supplier]
                    ,lr.InvNo [Invoice]
                    ,lrd.OrderId [SP]
                    ,lrd.Category [Category]
                    ,lrd.Refno [Refno]
                    ,[Description]=dbo.getItemDesc(lrd.Category,lrd.Refno)
                    ,lrd.ThreadColorID [Color_Shade]
                    ,c.UnitId [Unit]
                    ,c.Qty [PO_Qty]
                    ,lrd.Qty [Qty]
                    ,c.Qty-lrd.Qty [On_Road]
                    ,lrd.Location [Location]
                    ,lr.Remark [Remark]
           from dbo.LocalReceiving lr
           left join dbo.LocalReceiving_Detail lrd on  lr.id=lrd.Id
           left join dbo.LocalPO_Detail c on lrd.LocalPo_detailukey=c.Ukey  " + sqlWhere +" "+order);
            result = DBProxy.Current.Select("", sqlcmd, out dtt);
           

            return base.OnAsyncDataLoad(e);
        }


        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }
            string xlt = @"Subcon_R25.xltx";
            SaveXltReportCls xl = new SaveXltReportCls(xlt);
            xl.dicDatas.Add("##Factory", false);
            xl.Save(outpath);
            return false;
        }
    }
 }