using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.PublicPrgTests1
{
    public static class TestInitializer
    {
        public static void Initialize()
        {
            //Ict.Win.App.Init();
            //Env.AppInit();
            DBProxy.Current.DefaultModuleName = "testing_SNP";
            ((DBProxy)DBProxy.Current).OpenConnectionAction = _OpenConnection;
        }

        public static DualResult _OpenConnection(string connname, out SqlConnection connection)
        {
            SqlConnection val;
            val = new SqlConnection("Data Source=testing\\SNP;Initial Catalog=Production;Persist Security Info=True;User ID=scimis;Password=27128299");
            ((DbConnection)val).Open();
            connection = val;
            return Result.True;
        }
    }
}
