using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B43_Copy_CustomsSP : Sci.Win.Subs.Base
    {
        private DataRow drMaster;

        public B43_Copy_CustomsSP(DataRow drMain)
        {
            this.InitializeComponent();
            this.drMaster = drMain;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtSubconFromFty.Text = this.drMaster["SubconFromSystem"].ToString();
            this.txtSubconFromContract.Text = this.drMaster["SubconFromContractID"].ToString();

            this.txtSubconInFty.Text = MyUtility.GetValue.Lookup("select RgCode from system");
            this.txtSubconInContractNo.Text = this.drMaster["ID"].ToString();

            this.gridSubconFrom.IsEditingReadOnly = true;
            this.gridSubconFrom.DataSource = this.SubConFromDataBinding;
            this.Helper.Controls.Grid.Generator(this.gridSubconFrom)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8))
                .Text("Version", header: "Ver.", width: Widths.AnsiChars(4))
                .Date("CDate", header: "Date", width: Widths.AnsiChars(10))
                .Text("Category", header: "Category", width: Widths.AnsiChars(4))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Text("ColorWay", header: "Color Way", width: Widths.AnsiChars(16))
                .Text("SizeGroup", header: "Size Group", width: Widths.AnsiChars(16));

            this.gridSubconIn.IsEditingReadOnly = true;
            this.gridSubconIn.DataSource = this.SubConToDataBinding;
            this.Helper.Controls.Grid.Generator(this.gridSubconIn)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8))
                .Text("Version", header: "Ver.", width: Widths.AnsiChars(4))
                .Date("CDate", header: "Date", width: Widths.AnsiChars(10))
                .Text("Category", header: "Category", width: Widths.AnsiChars(4))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Text("ColorWay", header: "Color Way", width: Widths.AnsiChars(16))
                .Text("SizeGroup", header: "Size Group", width: Widths.AnsiChars(16))
                ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            DataTable dt;
            string sqlcmd = $@"
select t.CustomSP
,t.Version
,t.CDate
,t.Category
,t.SizeCode
,Article = Article.value
,SizeGroup = SizeGroup.value
from VNConsumption T
outer apply(
	select value = Stuff((
		select concat(',',Article)
		from (
				select 	distinct Article
				from dbo.VNConsumption_Article s
				where s.id = t.ID
			) s
		for xml path ('')
	) , 1, 1, '')
)Article
outer apply(
	select value = Stuff((
		select concat(',',SizeCode)
		from (
				select 	distinct SizeCode
				from dbo.VNConsumption_SizeCode s
				where s.id = t.ID
			) s
		for xml path ('')
	) , 1, 1, '')
)SizeGroup
where StyleID = '{this.txtStyle.Text}' 
and BrandID='{this.txtbrand.Text}'
and VNContractID = '{this.txtSubconFromContract.Text}'";

            DualResult result;

            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt)))
            {
                this.ShowErr(result);
                return;
            }
            else if (dt != null && dt.Rows.Count > 0)
            {
                this.SubConFromDataBinding.DataSource = dt;
            }

            #region Get WebAPI Data
            DataColumn dc = new DataColumn();
            DataRow dr;
            DataTable dtAPI = new DataTable();
            dtAPI.Columns.Add("StyleID", typeof(string));
            dtAPI.Columns.Add("BrandID", typeof(string));
            dtAPI.Columns.Add("ContractNO", typeof(string));
            dr = dt.NewRow();
            dr["StyleID"] = this.txtStyle.Text;
            dr["BrandID"] = this.txtbrand.Text;
            dr["ContractNO"] = this.txtSubconFromContract.Text;
            dt.Rows.Add(dr);

            Task.Run(() => new Customs_WebAPI().SentCustoms_load(dt)).ContinueWith(Utility_WebAPI.CustomsExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);

            /* Task.Run(() => new Gensong_AutoWHFabric().SentReceive_DetailToGensongAutoWHFabric(dtDetail))
         .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
         */
            #endregion
        }

        private void BtnCopyCustomsSP_Click(object sender, EventArgs e)
        {

        }
    }
}