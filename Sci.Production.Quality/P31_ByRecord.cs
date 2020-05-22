using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P31_ByRecord : Sci.Win.Forms.Base
    {

        private string _OrderID = "";
        private string _OrderShipmodeSeq = "";

        public P31_ByRecord(string OrderID , string OrderShipmodeSeq)
        {
            InitializeComponent();

            this._OrderID = OrderID;
            this._OrderShipmodeSeq = OrderShipmodeSeq;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DualResult res;
            DataTable dt; 

            string cmd = $@"
SELECt AuditDate
		,Stage
		,ClogReceivedPercentage
		,InspectQty		
		,DefectQty		
		,Result
		,[InspectedCarton]=Carton
		,Remark			
		,Status			
		,AddDate			
		,EditDate
FROM CFAInspectionRecord  
WHERE OrderID= '{this._OrderID}'  and SEQ='{this._OrderShipmodeSeq}'
";

            res = DBProxy.Current.Select(null, cmd, out dt);
            if (!res)
            {
                this.ShowErr(res);
            }
            this.listControlBindingSource.DataSource = dt;

            this.grid.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid)
                 .Date("AuditDate", header: "Audit Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("Stage", header: "Stage", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Numeric("ClogReceivedPercentage", header: "Closed carton Output(%)", width: Widths.AnsiChars(10),decimal_places:0, iseditingreadonly: true)
                 .Numeric("InspectQty", header: "Inspect Qty", width: Widths.AnsiChars(8), decimal_places: 0, iseditingreadonly: true)
                 .Numeric("DefectQty", header: "Defect Qty", width: Widths.AnsiChars(8), decimal_places: 0, iseditingreadonly: true)
                 .Text("Result", header: "Result", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("InspectedCarton", header: "Inspected Carton", width: Widths.AnsiChars(25), iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true)
                 .Text("Status", header: "Staggered result", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;
        }
    }
}
