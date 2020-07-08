using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class B02_BatchCreate : Sci.Win.Tems.Base
    {
        public B02_BatchCreate()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            List<MtlLocation> MtlLocations = new List<MtlLocation>();
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
            else if (!(this.chkBulk.Checked || this.chkInventory.Checked || this.chkScrap.Checked))
            {
                MyUtility.Msg.InfoBox("At least one <Stock Type> is checked.");
                return;
            }
            else
            {
                if (this.chkBulk.Checked)
                {
                    MtlLocations.Add(new MtlLocation()
                    {
                        ID = this.txtID.Text,
                        Description = this.txtDescription.Text,
                        StockType = "B",
                        AddName = Sci.Env.User.UserID,
                        AddDate = DateTime.Now,
                    });
                }

                if (this.chkInventory.Checked)
                {
                    MtlLocations.Add(new MtlLocation()
                    {
                        ID = this.txtID.Text,
                        Description = this.txtDescription.Text,
                        StockType = "I",
                        AddName = Sci.Env.User.UserID,
                        AddDate = DateTime.Now,
                    });
                }

                if (this.chkScrap.Checked)
                {
                    MtlLocations.Add(new MtlLocation()
                    {
                        ID = this.txtID.Text,
                        Description = this.txtDescription.Text,
                        StockType = "O",
                        AddName = Sci.Env.User.UserID,
                        AddDate = DateTime.Now,
                    });
                }
            }

            string insertCmd = string.Empty;

            foreach (var item in MtlLocations)
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
