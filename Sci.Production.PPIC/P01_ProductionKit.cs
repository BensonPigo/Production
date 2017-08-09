using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P01_ProductionKit : Sci.Win.Subs.Input4
    {
        protected string dataFilter = "";
        public P01_ProductionKit(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string styleid)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.Text = "Production Kit: " + styleid;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable FactoryData, MRData, SMRData;
            string sqlcmd = string.Format("select FactoryID,left(FactoryID+'        ',8) + cast(Count(FactoryID) as varchar(3)) from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0} group by FactoryID", KeyValue1);
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out FactoryData);
            MyUtility.Tool.SetupCombox(comboFactory, 2, FactoryData);

            sqlcmd = string.Format("select MRHandle,left(MRHandle+'          ',10) + cast(Count(MRHandle) as varchar(3)) from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0} group by MRHandle", KeyValue1);
            result = DBProxy.Current.Select(null, sqlcmd, out MRData);
            MyUtility.Tool.SetupCombox(comboMR, 2, MRData);

            sqlcmd = string.Format("select SMR,left(SMR+'          ',10) + cast(Count(SMR) as varchar(3)) from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0} group by SMR", KeyValue1);
            result = DBProxy.Current.Select(null, sqlcmd, out SMRData);
            MyUtility.Tool.SetupCombox(comboSMR, 2, SMRData);

            comboFactory.SelectedValue = "";
            comboMR.SelectedValue = "";
            comboSMR.SelectedValue = "";
        }

        protected override DualResult OnRequery()
        {
            string selectCommand = string.Format(@"select sp.*, iif(sp.IsPF = 1,'Y','N') as CPF, iif(sp.ReasonID = '','N','Y') as Reason, r.Name as ReasonName,
isnull((sp.MRHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.MRHandle)),sp.MRHandle) as MRName,
isnull((sp.PoHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.PoHandle)),sp.PoHandle) as POHName,
isnull((sp.SMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.SMR)),sp.SMR) as SMRName,
isnull((sp.POSMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.POSMR)),sp.POSMR) as POSMRName,
s.ID as StyleID, s.SeasonID
from Style_ProductionKits sp WITH (NOLOCK) 
left join Reason r WITH (NOLOCK) on r.ID = sp.DOC and r.ReasonTypeID = 'ProductionKits'
left join Style s WITH (NOLOCK) on sp.StyleUkey = s.Ukey
where sp.StyleUkey = {0} order by sp.ProductionKitsGroup", this.KeyValue1);
            Ict.DualResult returnResult;
            DataTable ArtworkTable = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out ArtworkTable);
            if (!returnResult)
            {
                return returnResult;
            }
            SetGrid(ArtworkTable);
            DataFilter();
            return Result.True;
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .EditText("Article", header: "ColorWay", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .EditText("ReasonName", header: "DOC", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("SendDate", header: "TW send date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ReceiveDate", header: "FTY MR rcv date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("SendToQA", header: "Fty send to QA", width: Widths.AnsiChars(10))
                .Date("QAReceived", header: "QA rcv date", width: Widths.AnsiChars(10))
                .Date("ProvideDate", header: "Provide date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("OrderId", header: "SPNO", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CPF", header: "Pull forward", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MRName", header: "MR", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("POHName", header: "PO Handle", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("SMRName", header: "SMR", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("POSMRName", header: "PO SMR", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("Reason", header: "Unnecessary to send", width: Widths.AnsiChars(1), iseditingreadonly: true);

            grid.Columns["SendToQA"].DefaultCellStyle.BackColor = Color.Pink;
            grid.Columns["QAReceived"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            append.Visible = false;
            revise.Visible = false;
            delete.Visible = false;
        }

        //Factory
        private void comboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        //MR
        private void comboMR_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        //SMR
        private void comboSMR_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        //MR not send yet
        private void checkMRnotSendYet_CheckedChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        //Factory not received
        private void checkFactorynotReceived_CheckedChanged(object sender, EventArgs e)
        {
            DataFilter();
        }

        //組Filter
        private void DataFilter()
        {
            dataFilter = "";
            StringBuilder filter = new StringBuilder();
            if (!MyUtility.Check.Empty(comboFactory.SelectedValue))
            {
                filter.Append(string.Format(" FactoryID = '{0}' and", comboFactory.SelectedValue.ToString()));
            }
            if (!MyUtility.Check.Empty(comboMR.SelectedValue))
            {
                filter.Append(string.Format(" MRHandle = '{0}' and", comboMR.SelectedValue.ToString()));
            }
            if (!MyUtility.Check.Empty(comboSMR.SelectedValue))
            {
                filter.Append(string.Format(" SMR = '{0}' and", comboSMR.SelectedValue.ToString()));
            }

            if (checkMRnotSendYet.Checked)
            {
                filter.Append(" SendDate is null and");
            }

            if (checkFactorynotReceived.Checked)
            {
                filter.Append(" SendDate is not null and ReceiveDate is null and");
            }

            if (filter.Length > 0)
            {
                dataFilter = filter.ToString().Substring(0, filter.Length - 3);
            }

            ((DataTable)gridbs.DataSource).DefaultView.RowFilter = dataFilter;
        }

        //View Detail
        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P03_Detail DoForm = new Sci.Production.PPIC.P03_Detail();
            DoForm.Set(false, new List<DataRow>(((DataTable)gridbs.DataSource).Select(dataFilter)), grid.GetDataRow(grid.GetSelectedRowIndex())); DoForm.ShowDialog(this);
        }

        
    }
}
