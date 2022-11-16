using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P25_PadPrintInUse : Sci.Win.Subs.Base
    {
        private DataRow dataRow;
        private DataTable tmpFindNow;

        public P25_PadPrintInUse(DataRow dr)
        {
            InitializeComponent();
            this.dataRow = dr;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFindNow_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void Find()
        {
            this.ShowWaitMessage("Data Loading...");
            string sqlcmd = $@"
select p.BrandID
,pm.Refno
,pm.MoldID
,pm.Season
,pm.LabelFor
,pm.Gender
,pm.AgeGroup
,pm.Part
,pm.Region
,pms.SizePage
,pms.SourceSize
,pms.CustomerSize
from PadPrint p
left join PadPrint_Mold pm on p.Ukey = pm.PadPrint_ukey
left join PadPrint_Mold_Spec pms on pms.PadPrint_ukey = pm.PadPrint_ukey
where 1=1
and p.BrandID = '{this.dataRow["BrandID"]}'
and pm.Region = '{this.dataRow["Region"]}'
and pm.Refno  = '{this.dataRow["Refno]"]}'
";
            if (this.chkSizePage.Checked)
            {
                switch (this.dataRow["SizePage"].ToString())
                {
                    case "J4":
                        sqlcmd += " and pms.SizePage like '%J4%'";
                        break;
                    case "J5":
                        sqlcmd += " and pms.SizePage like '%J5%'";
                        break;
                    default:
                        sqlcmd += $" and pms.SizePage like '%{this.dataRow["SizePage"]}%'";
                        break;
                }
            }

            if (this.chkSourceSize.Checked)
            {
                sqlcmd += $" and pms.SourceSize = '{this.dataRow["SizeCode"]}'";
            }

            if (this.chkGender.Checked)
            {
                string ID = MyUtility.GetValue.Lookup($@"select Top 1 ID from DropDownList where Type = 'PadPrint_Gender' and Name = '{this.dataRow["BrandGender"].ToString()}'");
                if (ID == null)
                {
                    ID = string.Empty;
                }

                sqlcmd += $" and pm.Gender = '{ID}'";
            }

            if (this.chkAgeGroup.Checked)
            {
                string ID = MyUtility.GetValue.Lookup($@"
select Top 1 ID from DropDownList where Type = 'PadPrint_AgeGroup' and Name = '{this.dataRow["AgeGroup"]}'");
                if (ID == null)
                {
                    ID = string.Empty;
                }

                sqlcmd += $" and pm.AgeGroup = '{ID}'";
            }

            if (this.chkPart.Checked)
            {
                string ID = MyUtility.GetValue.Lookup($@"
select top 1 d.ID 
from DropDownList d
Left join DropDownList dd on dd.Type = 'Location' and dd.id = '{this.dataRow["Location"]}'
where d.Type = 'PadPrint_Part' and d.Name = dd.Name");
                if (ID == null)
                {
                    ID = string.Empty;
                }

                sqlcmd += $" and pm.Part  = '{ID}'";
            }

            if (this.chkCustOrderSize.Checked)
            {
                sqlcmd += $" and pms.CustomerSize  = '{this.dataRow["SizeSpec"]}'";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
            this.grid.AutoResizeColumns();
            this.HideWaitMessage();
        }
    }
}
