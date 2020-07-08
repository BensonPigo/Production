using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ProductionKit
    /// </summary>
    public partial class P01_ProductionKit : Sci.Win.Subs.Input4
    {
        private string dataFilter1 = string.Empty;

        /// <summary>
        /// DataFilter1
        /// </summary>
        protected string DataFilter1
        {
            get
            {
                return this.dataFilter1;
            }

            set
            {
                this.dataFilter1 = value;
            }
        }

        /// <summary>
        /// P01_ProductionKit
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        /// <param name="styleid">string styleid</param>
        public P01_ProductionKit(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string styleid)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Text = "Production Kit: " + styleid;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable factoryData, mrData, smrData;
            string sqlcmd = string.Format("select FactoryID,left(FactoryID+'        ',8) + cast(Count(FactoryID) as varchar(3)) from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0} group by FactoryID", this.KeyValue1);
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out factoryData);
            MyUtility.Tool.SetupCombox(this.comboFactory, 2, factoryData);

            sqlcmd = string.Format("select MRHandle,left(MRHandle+'          ',10) + cast(Count(MRHandle) as varchar(3)) from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0} group by MRHandle", this.KeyValue1);
            result = DBProxy.Current.Select(null, sqlcmd, out mrData);
            MyUtility.Tool.SetupCombox(this.comboMR, 2, mrData);

            sqlcmd = string.Format("select SMR,left(SMR+'          ',10) + cast(Count(SMR) as varchar(3)) from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0} group by SMR", this.KeyValue1);
            result = DBProxy.Current.Select(null, sqlcmd, out smrData);
            MyUtility.Tool.SetupCombox(this.comboSMR, 2, smrData);

            this.comboFactory.SelectedValue = string.Empty;
            this.comboMR.SelectedValue = string.Empty;
            this.comboSMR.SelectedValue = string.Empty;
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery()
        {
            string selectCommand = string.Format(
                @"select sp.*, iif(sp.IsPF = 1,'Y','N') as CPF, iif(sp.ReasonID = '','N','Y') as Reason, r.Name as ReasonName,
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
            DataTable artworkTable = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out artworkTable);
            if (!returnResult)
            {
                return returnResult;
            }

            this.SetGrid(artworkTable);
            this.DataFilter();
            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
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

            this.grid.Columns["SendToQA"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["QAReceived"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        /// <inheritdoc/>
        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }

        // Factory
        private void ComboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DataFilter();
        }

        // MR
        private void ComboMR_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DataFilter();
        }

        // SMR
        private void ComboSMR_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DataFilter();
        }

        // MR not send yet
        private void CheckMRnotSendYet_CheckedChanged(object sender, EventArgs e)
        {
            this.DataFilter();
        }

        // Factory not received
        private void CheckFactorynotReceived_CheckedChanged(object sender, EventArgs e)
        {
            this.DataFilter();
        }

        // 組Filter
        private void DataFilter()
        {
            this.DataFilter1 = string.Empty;
            StringBuilder filter = new StringBuilder();
            if (!MyUtility.Check.Empty(this.comboFactory.SelectedValue))
            {
                filter.Append(string.Format(" FactoryID = '{0}' and", this.comboFactory.SelectedValue.ToString()));
            }

            if (!MyUtility.Check.Empty(this.comboMR.SelectedValue))
            {
                filter.Append(string.Format(" MRHandle = '{0}' and", this.comboMR.SelectedValue.ToString()));
            }

            if (!MyUtility.Check.Empty(this.comboSMR.SelectedValue))
            {
                filter.Append(string.Format(" SMR = '{0}' and", this.comboSMR.SelectedValue.ToString()));
            }

            if (this.checkMRnotSendYet.Checked)
            {
                filter.Append(" SendDate is null and");
            }

            if (this.checkFactorynotReceived.Checked)
            {
                filter.Append(" SendDate is not null and ReceiveDate is null and");
            }

            if (filter.Length > 0)
            {
                this.DataFilter1 = filter.ToString().Substring(0, filter.Length - 3);
            }

            ((DataTable)this.gridbs.DataSource).DefaultView.RowFilter = this.DataFilter1;
        }

        // View Detail
        private void BtnViewDetail_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P03_Detail doForm = new Sci.Production.PPIC.P03_Detail();
            doForm.Set(false, new List<DataRow>(((DataTable)this.gridbs.DataSource).Select(this.DataFilter1)), this.grid.GetDataRow(this.grid.GetSelectedRowIndex()));
            doForm.ShowDialog(this);
        }
    }
}
