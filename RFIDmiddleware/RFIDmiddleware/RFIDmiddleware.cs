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

namespace RFIDmiddleware
{
    public partial class RFIDmiddleware : Sci.Win.Tems.QueryForm
    {
        private string RFIDLoginId, RFIDLoginPwd;
        public RFIDmiddleware()
        {
            InitializeComponent();
            DataTable s;
            DBProxy.Current.Select(null,"select s.RFIDServerName,s.RFIDDatabaseName,s.RFIDTable,s.RFIDLoginId,s.RFIDLoginPwd from System s", out s);
            txtServerName.Text = MyUtility.Convert.GetString(s.Rows[0]["RFIDServerName"]);
            txtDatabaseName.Text = MyUtility.Convert.GetString(s.Rows[0]["RFIDDatabaseName"]);
            txtTable.Text = MyUtility.Convert.GetString(s.Rows[0]["RFIDTable"]);
            RFIDLoginId = MyUtility.Convert.GetString(s.Rows[0]["RFIDLoginId"]);
            RFIDLoginPwd = MyUtility.Convert.GetString(s.Rows[0]["RFIDLoginPwd"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlexc = string.Format(@"
--若不存在則新增連線	
IF NOT EXISTS (SELECT * FROM sys.servers WHERE name = '{0}')
BEGIN
	--新建連線
	EXEC master.dbo.sp_addlinkedserver @server = [{0}], @srvproduct=N'SQL Server'
	--設定連線登入資訊
	EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname = [{0}], @locallogin = NULL , @useself = N'False', @rmtuser = [{1}], @rmtpassword = [{2}]
END
", txtServerName.Text, RFIDLoginId, RFIDLoginPwd);
            DBProxy.Current.Execute(null, sqlexc);

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
