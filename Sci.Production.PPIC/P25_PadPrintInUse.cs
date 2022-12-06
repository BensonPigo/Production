using Ict;
using Ict.Win;
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
            this.EditMode = true;
            this.dataRow = dr;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.DataSource = this.listControlBindingSource1.DataSource;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MoldID", header: "Mold#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Season", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("LabelFor", header: "Label For", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Gender", header: "Gender", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("AgeGroup", header: "AgeGroup", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Part", header: "Part", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Region", header: "Region", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("SizePage", header: "Size Page", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SourceSize", header: "Source Size", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CustomerSize", header: "Cust Order Size", width: Widths.AnsiChars(5), iseditingreadonly: true);
            this.Find();
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
select distinct
p.BrandID
,pm.Refno
,pm.MoldID
,pm.Season
,[LabelFor] = case pm.LabelFor 
				when 'O' then 'One Asia' 
				when 'E' then 'EMEA' else pm.LabelFor end
,pm.Gender
,pm.AgeGroup
,pm.Part
,pm.Region
,pms.SizePage
,pms.SourceSize
,pms.CustomerSize
from PadPrint p
left join PadPrint_Mold pm on p.Ukey = pm.PadPrint_ukey
left join PadPrint_Mold_Spec pms on pms.PadPrint_ukey = pm.PadPrint_ukey and pm.MoldID = pms.MoldID 
where 1=1
and p.junk = 0
and p.BrandID = '{this.dataRow["BrandID"]}'
and pm.Region = '{this.dataRow["Region"]}'
and pm.Refno  = '{this.dataRow["Refno"].ToString().Replace("-PAD PRINT", string.Empty)}'
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
                string iD = MyUtility.GetValue.Lookup($@"select Top 1 ID from DropDownList where Type = 'PadPrint_Gender' and Name = '{this.dataRow["BrandGender"]}'");
                if (iD == null)
                {
                    iD = string.Empty;
                }

                sqlcmd += $" and pm.Gender = '{iD}'";
            }

            if (this.chkAgeGroup.Checked)
            {
                string iD = MyUtility.GetValue.Lookup($@"
select Top 1 ID from DropDownList where Type = 'PadPrint_AgeGroup' and Name = '{this.dataRow["AgeGroup"]}'");
                if (iD == null)
                {
                    iD = string.Empty;
                }

                sqlcmd += $" and pm.AgeGroup = '{iD}'";
            }

            if (this.chkPart.Checked)
            {
                string iD = MyUtility.GetValue.Lookup($@"
select top 1 d.ID 
from DropDownList d
Left join DropDownList dd on dd.Type = 'Location' and dd.id = '{this.dataRow["Location"]}'
where d.Type = 'PadPrint_Part' and d.Name = dd.Name");
                if (iD == null)
                {
                    iD = string.Empty;
                }

                sqlcmd += $" and pm.Part  = '{iD}'";
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
            this.grid.DataSource = this.listControlBindingSource1.DataSource;
            this.HideWaitMessage();
        }
    }
}
