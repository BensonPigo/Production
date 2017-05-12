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
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Configuration;
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
            
            //DualResult res = DBProxy.Current.Select("RFID", sqlcme, out m);
            //if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return; }

            if (m.Rows.Count < 1) { MyUtility.Msg.ErrorBox("No datas.", ""); return; }

            string xltPath = System.IO.Path.Combine(Sci.Env.Cfg.XltPathDir, "RFIDmiddleware.xltx");
            sxrc sxr = new sxrc(xltPath);
            sxrc.xltRptTable dt = new sxrc.xltRptTable(m);

            dt.ShowHeader = true;
            dt.Borders.AllCellsBorders = true;
            //自動欄位寬度
            dt.boAutoFitColumn = true;
            //凍結窗格
            dt.boFreezePanes = true;
            dt.boAddFilter = true;
            sxr.dicDatas.Add(sxr._v + "tb", dt);
            sxr.boOpenFile = true;
            sxr.Save();

        }
    }
}
