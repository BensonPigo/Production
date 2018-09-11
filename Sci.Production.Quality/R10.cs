using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Sci.Win.Tools;

namespace Sci.Production.Quality
{
    public partial class R10 : Sci.Win.Tems.PrintForm
    {
        private string Type, Style, Season, Brand, FabricRefNo, T1SubconName, T2SubconName;

        private void txtCombineStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string item_cmd = "select ID,SeasonID,Description,BrandID from Style WITH (NOLOCK) where Junk = 0 order by ID";
            SelectItem item = new SelectItem(item_cmd, "", "", "");
            DialogResult dresult = item.ShowDialog();
            if (dresult == DialogResult.Cancel)
            {
                return;
            }

            txtCombineStyle.Text = item.GetSelectedString();
            txtbrand.Text = item.GetSelecteds()[0]["BrandID"].ToString();
            txtseason.Text = item.GetSelecteds()[0]["SeasonID"].ToString();
        }

        private void txtCombineStyle_Validating(object sender, CancelEventArgs e)
        {
            string sqlcmd = $@"select 1 from Style WITH (NOLOCK) where Junk = 0 and id ='{this.txtCombineStyle.Text}'";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox("Data not found!");
                txtCombineStyle.Focus();
                this.txtCombineStyle.Text = string.Empty;
                return;
            }
        }

        private DataTable printData;

        private void comboBoxTypeofPrint_SelectedValueChanged(object sender, EventArgs e)
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
            InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboBoxTypeofPrint, 2, 1, "All,All,Mockup Crocking,Mockup Crocking,Mockup Oven,Mockup Oven,Mockup Wash,Mockup Wash");
            this.comboBoxTypeofPrint.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            Type = this.comboBoxTypeofPrint.SelectedValue.ToString();
            Style = this.txtCombineStyle.Text;
            Season = this.txtseason.Text;
            Brand = this.txtbrand.Text;
            FabricRefNo = this.txtFabRefno.Text;
            T1SubconName = this.txtLocalSupp.TextBox1.Text;
            T2SubconName = this.txtLocalTPESupp.TextBox1.Text;

            if (MyUtility.Check.Empty(Style))
            {
                MyUtility.Msg.WarningBox("Style cannot be empty!");
                return false;
            }
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region Filter
            List<string> Filter = new List<string>();
            if (!MyUtility.Check.Empty(Type) && Type !="All")
            {
                Filter.Add($"and TypeOfPrint='{Type}'");
            }

            if (!MyUtility.Check.Empty(Style))
            {
                Filter.Add($"and Style = '{Style}'");
            }

            if (!MyUtility.Check.Empty(Season))
            {
                Filter.Add($"and Season = '{Season}'");
            }

            if (!MyUtility.Check.Empty(Brand))
            {
                Filter.Add($"and Brand = '{Brand}'");
            }

            if (!MyUtility.Check.Empty(Brand))
            {
                Filter.Add($"and Brand = '{Brand}'");
            }

            if (!MyUtility.Check.Empty(FabricRefNo))
            {
                Filter.Add($"and FabricRefNo = '{FabricRefNo}'");
            }

            if (!MyUtility.Check.Empty(T1SubconName))
            {
                Filter.Add($"and T1SubconName = '{T1SubconName}'");
            }

            if (!MyUtility.Check.Empty(T2SubconName) && Type != "Mockup Crocking")
            {
                Filter.Add($"and (T2SubconName = '{T2SubconName}' or TypeOfPrint='Mockup Crocking')");
            }
            #endregion

            #region SqlCommand
            string sqlcmd = $@"
select * from 
(
	select 
	[TypeOfPrint] ='Mockup Crocking'
	,[Style] = StyleID
	,[Season] = SeasonID
	,[Brand] = BrandID
	,[Article] = a.Article
	,[T1SubconName] = a.T1Subcon
	,[T2SubconName] = ''
	,[ReportNo] = c.ReportNo
	,[SubmitDate] = b.SubmitDate
	,[ReceivedDate] = b.ReceivedDate
	,[ReleaseDate] = b.ReleasedDate
	,[Artwork] = c.ArtworkTypeID
	,[ArtworkColor] = c.ArtworkColor
	,[FabricRefNo] = c.FabricRefNo
	,[FabricColor] = c.FabricColor
	,[Result] = c.Result
	 from MockupCrocking a
	 left join MockupCrocking_Detail b on a.ID=b.ID
	 left join MockupCrocking_Detail_Detail c on a.ID=c.ID

union all 

	select 
	[TypeOfPrint] ='Mockup Oven'
	,[Style] = StyleID
	,[Season] = SeasonID
	,[Brand] = BrandID
	,[Article] = a.Article
	,[T1SubconName] = a.T1Subcon
	,[T2SubconName] = a.T2Supplier
	,[ReportNo] = c.ReportNo
	,[SubmitDate] = b.SubmitDate
	,[ReceivedDate] = b.ReceivedDate
	,[ReleaseDate] = b.ReleasedDate
	,[Artwork] = c.ArtworkTypeID
	,[ArtworkColor] = c.ArtworkColor
	,[FabricRefNo] = c.FabricRefNo
	,[FabricColor] = c.FabricColor
	,[Result] = c.Result
	 from MockupOven a
	 left join MockupOven_Detail b on a.ID=b.ID
	 left join MockupOven_Detail_Detail c on a.ID=c.ID

union all

	select 
	[TypeOfPrint] ='Mockup Wash'
	,[Style] = StyleID
	,[Season] = SeasonID
	,[Brand] = BrandID
	,[Article] = a.Article
	,[T1SubconName] = a.T1Subcon
	,[T2SubconName] = a.T2Supplier
	,[ReportNo] = c.ReportNo
	,[SubmitDate] = b.SubmitDate
	,[ReceivedDate] = b.ReceivedDate
	,[ReleaseDate] = b.ReleasedDate
	,[Artwork] = c.ArtworkTypeID
	,[ArtworkColor] = c.ArtworkColor
	,[FabricRefNo] = c.FabricRefNo
	,[FabricColor] = c.FabricColor
	,[Result] = c.Result
	 from MockupWash a
	 left join MockupWash_Detail b on a.ID=b.ID
	 left join MockupWash_Detail_Detail c on a.ID=c.ID
) d
where 1=1
{Filter.JoinToString($"{Environment.NewLine} ")}
";
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlcmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            string xltx = "Quality_R10.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltx); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", xltx, 1, true, null, objApp);// 將datatable copy to excel
            return true;
        }

    }
}
