using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Sci.Win.Tools;

namespace Sci.Production.Quality
{
    public partial class R10 : Win.Tems.PrintForm
    {
        private string Type;
        private string Style;
        private string Season;
        private string Brand;
        private string FabricRefNo;
        private string T1SubconName;
        private string T2SubconName;

        private void TxtCombineStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string item_cmd = "select ID,SeasonID,Description,BrandID from Style WITH (NOLOCK) where Junk = 0 order by ID";
            SelectItem item = new SelectItem(item_cmd, string.Empty, string.Empty, string.Empty);
            DialogResult dresult = item.ShowDialog();
            if (dresult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCombineStyle.Text = item.GetSelectedString();
            this.txtbrand.Text = item.GetSelecteds()[0]["BrandID"].ToString();
            this.txtseason.Text = item.GetSelecteds()[0]["SeasonID"].ToString();
        }

        private void TxtCombineStyle_Validating(object sender, CancelEventArgs e)
        {
            string sqlcmd = $@"select 1 from Style WITH (NOLOCK) where Junk = 0 and id ='{this.txtCombineStyle.Text}'";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.txtCombineStyle.Focus();
                this.txtCombineStyle.Text = string.Empty;
                return;
            }
        }

        private DataTable printData;

        private void ComboBoxTypeofPrint_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboBoxTypeofPrint.SelectedValue == null)
            {
                return;
            }

            string typevalue = this.comboBoxTypeofPrint.SelectedValue.ToString();
            if (typevalue == "Mockup Crocking")
            {
                this.txtLocalTPESupp.Enabled = false;
            }
            else
            {
                this.txtLocalTPESupp.Enabled = true;
            }
        }

        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboBoxTypeofPrint, 2, 1, "All,All,Mockup Crocking,Mockup Crocking,Mockup Oven,Mockup Oven,Mockup Wash,Mockup Wash");
            this.comboBoxTypeofPrint.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            this.Type = this.comboBoxTypeofPrint.SelectedValue.ToString();
            this.Style = this.txtCombineStyle.Text;
            this.Season = this.txtseason.Text;
            this.Brand = this.txtbrand.Text;
            this.FabricRefNo = this.txtFabRefno.Text;
            this.T1SubconName = this.txtLocalSupp.TextBox1.Text;
            this.T2SubconName = this.txtLocalTPESupp.TextBox1.Text;

            if (MyUtility.Check.Empty(this.Style) && (MyUtility.Check.Empty(this.Season) || MyUtility.Check.Empty(this.Brand)))
            {
                MyUtility.Msg.WarningBox("[Style] or [Season/Brand] cannot be empty!");
                return false;
            }

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region Filter
            List<string> Filter = new List<string>();
            if (!MyUtility.Check.Empty(this.Type) && this.Type != "All")
            {
                Filter.Add($"and TypeOfPrint='{this.Type}'");
            }

            if (!MyUtility.Check.Empty(this.Style))
            {
                Filter.Add($"and Style = '{this.Style}'");
            }

            if (!MyUtility.Check.Empty(this.Season))
            {
                Filter.Add($"and Season = '{this.Season}'");
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                Filter.Add($"and Brand = '{this.Brand}'");
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                Filter.Add($"and Brand = '{this.Brand}'");
            }

            if (!MyUtility.Check.Empty(this.FabricRefNo))
            {
                Filter.Add($"and FabricRefNo = '{this.FabricRefNo}'");
            }

            if (!MyUtility.Check.Empty(this.T1SubconName))
            {
                Filter.Add($"and T1SubconName = '{this.T1SubconName}'");
            }

            if (!MyUtility.Check.Empty(this.T2SubconName) && this.Type != "Mockup Crocking")
            {
                Filter.Add($"and (T2SubconName = '{this.T2SubconName}' or TypeOfPrint='Mockup Crocking')");
            }
            #endregion

            #region SqlCommand
            string sqlcmd = $@"
select * from 
(
	select 
	[TypeOfTesting] ='Mockup Crocking'
	,[Style] = StyleID
	,[Season] = SeasonID
	,[Brand] = BrandID
	,[Article] = a.Article
	,[T1SubconName] = (select Abb from LocalSupp WITH (NOLOCK) where id = a.T1Subcon)
	,[T2SubconName] = ''
	,[ReportNo] = c.ReportNo
	,[SubmitDate] = b.SubmitDate
	,[ReceivedDate] = b.ReceivedDate
	,[ReleaseDate] = b.ReleasedDate
	,TestTemperature=null
	,TestTime=null
	,HTPlate=null
	,HTFlim=null
	,HTTime=null
	,HTPressure=null
	,HTPellOff=null
	,HT2ndPressnoreverse=null
	,HT2ndPressreversed=null
	,HTCoolingTime=null
	,[Artwork] = c.ArtworkTypeID
	,TypeofPrint=null
	,[ArtworkColor] = c.ArtworkColor
	,[FabricRefNo] = c.FabricRefNo
	,[FabricColor] = c.FabricColor
	,[Result] = c.Result
	,c.Remark
	 from MockupCrocking a
	 left join MockupCrocking_Detail b on a.ID=b.ID
	 left join MockupCrocking_Detail_Detail c on a.ID=c.ID

union all 

	select 
	[TypeOfTesting] ='Mockup Oven'
	,[Style] = StyleID
	,[Season] = SeasonID
	,[Brand] = BrandID
	,[Article] = a.Article
	,[T1SubconName] = (select Abb from LocalSupp WITH (NOLOCK) where id = a.T1Subcon)
	,[T2SubconName] = (select top 1 Abb from(select Abb from LocalSupp WITH (NOLOCK) where id = a.T2Supplier  union select [Abb] = AbbEN from Supp WITH (NOLOCK)where id = a.T2Supplier )a)
	,[ReportNo] = c.ReportNo
	,[SubmitDate] = b.SubmitDate
	,[ReceivedDate] = b.ReceivedDate
	,[ReleaseDate] = b.ReleasedDate
	,TestTemperature
	,TestTime
	,HTPlate
	,HTFlim
	,HTTime
	,HTPressure
	,HTPellOff
	,HT2ndPressnoreverse
	,HT2ndPressreversed
	,HTCoolingTime
	,[Artwork] = c.ArtworkTypeID
	,TypeofPrint
	,[ArtworkColor] = c.ArtworkColor
	,[FabricRefNo] = c.FabricRefNo
	,[FabricColor] = c.FabricColor
	,[Result] = c.Result
	,c.Remark
	 from MockupOven a
	 left join MockupOven_Detail b on a.ID=b.ID
	 left join MockupOven_Detail_Detail c on a.ID=c.ID

union all

	select 
	[TypeOfTesting] ='Mockup Wash'
	,[Style] = StyleID
	,[Season] = SeasonID
	,[Brand] = BrandID
	,[Article] = a.Article
	,[T1SubconName] = (select Abb from LocalSupp WITH (NOLOCK) where id = a.T1Subcon)
	,[T2SubconName] =  (select top 1 Abb from(select Abb from LocalSupp WITH (NOLOCK) where id = a.T2Supplier  union select [Abb] = AbbEN from Supp WITH (NOLOCK)where id = a.T2Supplier )a)
	,[ReportNo] = c.ReportNo
	,[SubmitDate] = b.SubmitDate
	,[ReceivedDate] = b.ReceivedDate
	,[ReleaseDate] = b.ReleasedDate
	,TestTemperature=null
	,TestTime=null
	,HTPlate
	,HTFlim
	,HTTime
	,HTPressure
	,HTPellOff
	,HT2ndPressnoreverse
	,HT2ndPressreversed
	,HTCoolingTime
	,[Artwork] = c.ArtworkTypeID
	,TypeofPrint
	,[ArtworkColor] = c.ArtworkColor
	,[FabricRefNo] = c.FabricRefNo
	,[FabricColor] = c.FabricColor
	,[Result] = c.Result
	,c.Remark
	 from MockupWash a
	 left join MockupWash_Detail b on a.ID=b.ID
	 left join MockupWash_Detail_Detail c on a.ID=c.ID
) d
where 1=1
{Filter.JoinToString($"{Environment.NewLine} ")}
";
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlcmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string xltx = "Quality_R10.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltx, 1, true, null, objApp); // 將datatable copy to excel
            return true;
        }
    }
}
