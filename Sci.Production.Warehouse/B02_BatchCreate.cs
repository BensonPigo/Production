using Ict;
using Sci.Data;
using Sci.Production.Automation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sci.Production.Warehouse
{
    public partial class B02_BatchCreate : Win.Tems.Base
    {
        public B02_BatchCreate()
        {
            this.InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            List<MtlLocation> mtlLocations = new List<MtlLocation>();
            DualResult result;
            if (MyUtility.Check.Empty(this.txtID.Text) && !(this.chkBulk.Checked || this.chkInventory.Checked || this.chkScrap.Checked))
            {
                MyUtility.Msg.InfoBox("<Code> can not be empty and at least one <Stock Type> is checked.");
                return;
            }
            else if (MyUtility.Check.Empty(this.txtID.Text))
            {
                MyUtility.Msg.InfoBox("<Code> can not be empty.");
                return;
            }
            else if (this.txtID.Text.Length >= 4 && this.txtID.Text.Substring(0, 4) == "WMS_")
            {
                MyUtility.Msg.WarningBox("Cannot type in \"WMS_\" words in <Code> !");
                return;
            }
            else if (!(this.chkBulk.Checked || this.chkInventory.Checked || this.chkScrap.Checked))
            {
                MyUtility.Msg.InfoBox("At least one <Stock Type> is checked.");
                return;
            }
            else
            {
                if (this.chkBulk.Checked)
                {
                    mtlLocations.Add(new MtlLocation()
                    {
                        ID = this.txtID.Text,
                        Description = this.txtDescription.Text,
                        StockType = "B",
                        AddName = Env.User.UserID,
                        AddDate = DateTime.Now,
                    });
                }

                if (this.chkInventory.Checked)
                {
                    mtlLocations.Add(new MtlLocation()
                    {
                        ID = this.txtID.Text,
                        Description = this.txtDescription.Text,
                        StockType = "I",
                        AddName = Env.User.UserID,
                        AddDate = DateTime.Now,
                    });
                }

                if (this.chkScrap.Checked)
                {
                    mtlLocations.Add(new MtlLocation()
                    {
                        ID = this.txtID.Text,
                        Description = this.txtDescription.Text,
                        StockType = "O",
                        AddName = Env.User.UserID,
                        AddDate = DateTime.Now,
                    });
                }
            }

            string insertCmd = string.Empty;

            foreach (var item in mtlLocations)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                string chkCmd = $"SELECT * FROM MtlLocation WHERE ID=@ID AND StockType=@StockType";
                paras.Add(new SqlParameter("@ID", item.ID));
                paras.Add(new SqlParameter("@StockType", item.StockType));

                if (!MyUtility.Check.Seek(chkCmd, paras))
                {
                    insertCmd += $@"
INSERT INTO [dbo].[MtlLocation]
           ([ID]
           ,[StockType]
           ,[Junk]
           ,[Description]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
     VALUES
           ('{item.ID}'
           ,'{item.StockType}'
           ,0
           ,'{item.Description}'
           ,'{item.AddName}'
           ,GETDATE()
           ,''
           ,NULL)

";
                }
            }

            if (!MyUtility.Check.Empty(insertCmd))
            {
                result = DBProxy.Current.Execute(null, insertCmd);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            // AutoWHFabric WebAPI for Gensong
//            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
//            {
//                if (this.chkBulk.Checked)
//                {
//                    DataTable dtMain = new DataTable();
//                    string sqlcmd = $@"
//select * from MtlLocation 
//where id ='{this.txtID.Text}'
//and StockType = 'B'
//";
//                    DBProxy.Current.Select(string.Empty, sqlcmd, out dtMain);
//                    if (dtMain != null || dtMain.Rows.Count > 0)
//                    {
//                        Task.Run(() => new Gensong_AutoWHFabric().SentMtlLocationToGensongAutoWHFabric(dtMain))
//                       .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
//                    }
//                }
//            }

            this.txtID.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            MyUtility.Msg.InfoBox("Complete");
        }

        public class MtlLocation
        {
            public string ID { get; set; }

            public string Description { get; set; }

            public string StockType { get; set; }

            public string AddName { get; set; }

            public DateTime AddDate { get; set; }
        }
    }
}
