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
            string sqlcme = string.Format("select * from [{0}].[{1}].dbo.[{2}]", txtServerName.Text, txtDatabaseName.Text, txtTable.Text);
            DataTable m;
            DualResult res = DBProxy.Current.Select(null, sqlcme, out m);
            if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return; }
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
