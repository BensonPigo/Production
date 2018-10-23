using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using Sci;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace RFIDmiddleware
{
    public partial class RFIDmiddleware : Sci.Win.Tems.QueryForm
    {
        private string RFIDLoginId, RFIDLoginPwd;

        public RFIDmiddleware()
        {
            InitializeComponent();
            
            txtServerName.Text = ConfigurationManager.AppSettings["RFIDServerName"];;
            txtDatabaseName.Text = ConfigurationManager.AppSettings["RFIDDatabaseName"]; ;
            txtTable.Text = ConfigurationManager.AppSettings["RFIDTable"]; ;
            RFIDLoginId = ConfigurationManager.AppSettings["RFIDLoginId"]; ;
            RFIDLoginPwd = ConfigurationManager.AppSettings["RFIDLoginPwd"]; ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlcme = string.Format("select * from [{0}].dbo.[{1}]",  txtDatabaseName.Text, txtTable.Text);
            DataTable m = new DataTable();
            SqlConnection conn = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", txtServerName.Text, txtDatabaseName.Text, RFIDLoginId, RFIDLoginPwd));

            using (conn)
            {
                SqlCommand command = new SqlCommand(sqlcme, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(m);
                da.Dispose();
            }
            
            if (m.Rows.Count < 1) { MyUtility.Msg.ErrorBox("No datas.", ""); return; }

            
            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\RFIDmiddleware.xltx", objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            com.WriteTable(m, 1, 1, true);
            com.ColumnsAutoFit = true;
            worksheet.Columns[5].NumberFormat = "yyyy/mm/dd hh:mm:ss";

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
        }
    }
}
