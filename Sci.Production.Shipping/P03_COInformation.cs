using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Shipping
{
    public partial class P03_COInformation : Win.Forms.Base
    {
        private string exportID;

        /// <summary>
        /// Initializes a new instance of the <see cref="P03_COInformation"/> class.
        /// </summary>
        /// <param name="exportID"></param>
        public P03_COInformation(string exportID)
        {
            this.InitializeComponent();
            this.exportID = exportID;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridCOInfo)
                .Text("ID", header: "Material C/O ID", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("InvoiceNo", header: "Invoice#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("FormName", header: "Form Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("FormNo", header: "Form#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("FtyReceiveDate", header: "Fty Receive Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("FtyReceiveName", header: "Fty Receive Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .EditText("FtyRemark", header: "Fty Remark", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Date("TPEReceiveDate", header: "TPE Receive Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("TPERemark", header: "TPE Remark", width: Widths.AnsiChars(25), iseditingreadonly: true);

            string sqlGetData = $@"
select  distinct
        md.ID              ,
        md.SuppID          ,
        md.InvoiceNo       ,
        md.FormType        ,
        md.FormNo          ,
        md.TPEReceiveDate  ,
        md.TPERemark       ,
        md.Junk            ,
        md.FtyReceiveDate  ,
        md.FtyReceiveName  ,
        md.FtyRemark       ,
        md.TPEAddName      ,
        md.TPEAddDate      ,
        md.TPEEditName     ,
        md.TPEEditDate     ,
        md.FtyEditName     ,
        md.FtyEditDate,
        [FormName] = ft.Name
from Export_Detail ed
inner join MtlCertificate_Detail md on md.InvoiceNo = ed.FormXPayINV
left join FormType ft with (nolock) on ft.ID = md.FormType
where ed.ID = '{this.exportID}'

";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridCOInfo.DataSource = dtResult;
        }
    }
}
