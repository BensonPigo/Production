using Sci.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Net;
using System.IO;
using Sci;
using System.Diagnostics;
using PostJobLog;
using System.Threading;

namespace MarkerFile.Hourly
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        private string MarkerInputPath;
        private string MarkerOutputPath;
        public Main()
        {
            InitializeComponent();
            isAuto = false;
        }

        public Main(String _isAuto)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(_isAuto))
            {
                isAuto = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ClickExport();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.MarkerInputPath = MyUtility.GetValue.Lookup($@"select MarkerInputPath from system");
            this.MarkerOutputPath = MyUtility.GetValue.Lookup($@"select MarkerOutputPath from system");
            if (isAuto)
            {
                ClickExport();
                this.Close();
            }
        }
        
        private void ClickExport()
        {
            SqlConnection conn;

            if (!Sci.SQL.GetConnection(out conn)) return;

            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result = AsyncHelper.Current.DataProcess(this, () =>
            {
                return AsyncUpdateExport(conn);
            });

            if (!result)
            {
                ShowErr(result);
            }

            conn.Close();
        }


        // Export/Update (非同步)
        private DualResult AsyncUpdateExport(SqlConnection conn)
        {
            try
            {
                string sqlcmd = $@"select 1";
                SqlCommand cmd = new SqlCommand(sqlcmd, conn);
                cmd.CommandTimeout = 7200;  //12分鐘
                cmd.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                return Ict.Result.F(se);
            }
            return Ict.Result.True;
        }

        // 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message; });
        }

        // 1.	刪除舊檔案及資料夾
        private bool DeleteFile()
        {
            return true;
        }

        // 2.	檢查及更新檔案
        private bool CheckUpdateFile()
        {
            return true;
        }
    }
}
