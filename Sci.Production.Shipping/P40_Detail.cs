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

namespace Sci.Production.Shipping
{
    public partial class P40_Detail : Sci.Win.Subs.Base
    {
        DataRow masterData;
        string nlCode;
        public P40_Detail(DataRow  MasterData, string NLCode)
        {
            InitializeComponent();
            masterData = MasterData;
            nlCode = NLCode;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(grid1)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 2, iseditingreadonly: true)
                .Text("WKNo", header: "WK No.", width: Widths.AnsiChars(30), iseditingreadonly: true);

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            sqlCmd.Append(@"with tmpExport
as (
");
            if (MyUtility.Convert.GetString(masterData["IsFtyExport"]).ToUpper() == "FALSE")
            {
                    sqlCmd.Append(string.Format(@"select f.Refno,f.BrandID,f.Type,ed.UnitId,sum(ed.Qty+ed.Foc) as Qty,e.ID 
from Export e
inner join Export_Detail ed on e.ID = ed.ID
inner join PO_Supp_Detail psd on ed.PoID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
inner join Fabric f on psd.SCIRefno = f.SCIRefno
where {0}
and f.NLCode = '{1}'
group by f.Refno,f.BrandID,f.Type,ed.UnitId,e.ID",
                                                 MyUtility.Check.Empty(masterData["WKNo"]) ? "e.Blno = '" + MyUtility.Convert.GetString(masterData["BLNo"]) + "'" : "e.ID = '" + MyUtility.Convert.GetString(masterData["WKNo"]) + "'", nlCode));
            }
            else
            {
                if (MyUtility.Convert.GetString(masterData["IsLocalPO"]).ToUpper() == "TRUE")
                {
                    sqlCmd.Append(string.Format(@"select f.Refno,'' as BrandID,f.Category as Type,ed.UnitId,sum(ed.Qty) as Qty,e.ID 
from FtyExport e
inner join FtyExport_Detail ed on e.ID = ed.ID
inner join LocalItem f on ed.Refno = f.Refno
where {0}
and f.NLCode = '{1}'
group by f.Refno,f.Category,ed.UnitId,e.ID",
                        MyUtility.Check.Empty(masterData["WKNo"]) ? "e.Blno = '" + MyUtility.Convert.GetString(masterData["BLNo"]) + "'" : "e.ID = '" + MyUtility.Convert.GetString(masterData["WKNo"]) + "'", nlCode));
                }
                else
                {
                    sqlCmd.Append(string.Format(@"select f.Refno,f.BrandID,f.Type,ed.UnitId,sum(ed.Qty) as Qty,e.ID 
from FtyExport e
inner join FtyExport_Detail ed on e.ID = ed.ID
inner join PO_Supp_Detail psd on ed.PoID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
inner join Fabric f on psd.SCIRefno = f.SCIRefno
where {0}
and f.NLCode = '{1}'
group by f.Refno,f.BrandID,f.Type,ed.UnitId,e.ID",
                        MyUtility.Check.Empty(masterData["WKNo"]) ? "e.Blno = '" + MyUtility.Convert.GetString(masterData["BLNo"]) + "'" : "e.ID = '" + MyUtility.Convert.GetString(masterData["WKNo"]) + "'", nlCode));
                }
            }
            
            sqlCmd.Append(@"
),
tmpWKNo
as (
select RefNo,
(select CONCAT(ID,',') from tmpExport te where te.Refno = t.Refno order by te.ID for XML path('')) as WKNo
from tmpExport t
),
tmpDataSummary
as (
select Refno,BrandID,Type,UnitId,sum(Qty) as Qty
from tmpExport
group by Refno,BrandID,Type,UnitId
)
select distinct ts.*, tw.WKNo
from tmpDataSummary ts
inner join tmpWKNo tw on ts.Refno = tw.Refno");
            #endregion

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
            }

            listControlBindingSource1.DataSource = gridData;
        }
    }
}
