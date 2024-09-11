﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P11_JunkSP : Win.Forms.Base
    {
        private DataTable tmp;

        /// <summary>
        /// Initializes a new instance of the <see cref="P11_JunkSP"/> class.
        /// </summary>
        /// <param name="mdr">Row</param>
        public P11_JunkSP(DataTable mdr)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.tmp = mdr;
        }

        private DataTable dt;

        private void Query()
        {
            string sqlcmd = $@"
Select  
     sel = Cast(0 as Bit),
     tmp.SubConOutFty,
     tmp.ContractNumber,
     tmp.OrderID,
     o.StyleID, 
     tmp.ComboType,
     oq.Article,
     OrderQty = Sum(oq.Qty), 
     tmp.OutputQty,
     tmp.UnitPrice,
     LocalCurrencyID = Isnull(tmp.LocalCurrencyID, ''),
     tmp.LocalUnitPrice, 
     tmp.Vat,
     tmp.KpiRate,
     reason = ''
From #tmp tmp
Left join Orders o On tmp.OrderID = o.ID
Left join Order_Qty oq On oq.ID = tmp.OrderID And oq.article = tmp.article 
Where 1=1
--And tmp.AccuOutPutQty = 0
Group By tmp.OrderID, o.StyleID, tmp.ComboType, oq.Article,tmp.OutputQty,tmp.UnitPrice, 
         tmp.SubConOutFty,tmp.ContractNumber,LocalUnitPrice, tmp.Vat, tmp.KpiRate, tmp.LocalCurrencyID
";
            MyUtility.Tool.ProcessWithDatatable(this.tmp, string.Empty, sqlcmd, out this.dt);
            this.listControlBindingSource1.DataSource = this.dt;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();

            #region -- Grid 設定 --
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("sel", header: string.Empty, width: Widths.AnsiChars(5))
            .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("ComboType", header: "ComboType", iseditingreadonly: true, width: Widths.AnsiChars(10))
            .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(10))
            .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true, width: Widths.AnsiChars(10))
            .Numeric("OutputQty", header: "Subcon Out Qty ", iseditingreadonly: true, width: Widths.AnsiChars(10))
            .Numeric("UnitPrice", header: "Price(Unit)", iseditingreadonly: true, iseditable: false)
            .Text("Reason", header: "Reason", iseditingreadonly: false, width: Widths.AnsiChars(35), iseditable: true)
            ;
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.grid1.IsEditingReadOnly = false;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            DataTable subconOutContract_Junk = new DataTable();
            string sql = "Select * from SubconOutContract_Junk where 1 = 2";
            DBProxy.Current.Select(null, sql, out subconOutContract_Junk);

            #region 新增至 SubconOutContract_Junk

            var selectedRows = this.dt.AsEnumerable()
                                      .Where(row => row.Field<bool>("sel"));

            var groupedData = selectedRows
          .GroupBy(row => new
          {
              SubConOutFty = row.Field<string>("SubConOutFty"),
              ContractNumber = row.Field<string>("ContractNumber"),
              OrderID = row.Field<string>("OrderID"),
              StyleID = row.Field<string>("StyleID"),
              ComboType = row.Field<string>("ComboType"),
              Article = row.Field<string>("Article"),
              OutputQty = row.Field<int>("OutputQty"),
              UnitPrice = row.Field<decimal>("UnitPrice"),
              LocalCurrencyID = row.Field<string>("LocalCurrencyID"),
              LocalUnitPrice = row.Field<decimal>("LocalUnitPrice"),
              Vat = row.Field<decimal>("Vat"),
              KpiRate = row.Field<decimal>("KpiRate"),
              Reason = row.Field<string>("Reason"),
          })
          .Select(group => new
          {
              SubConOutFty = group.Key.SubConOutFty,
              ContractNumber = group.Key.ContractNumber,
              OrderID = group.Key.OrderID,
              ComboType = group.Key.ComboType,
              Article = group.Key.Article,
              OutputQty = group.Key.OutputQty,
              UnitPrice = group.Key.UnitPrice,
              LocalCurrencyID = group.Key.LocalCurrencyID,
              LocalUnitPrice = group.Key.LocalUnitPrice,
              Vat = group.Key.Vat,
              KpiRate = group.Key.KpiRate,
              Reason = group.Key.Reason,
          });

            // 将汇总数据插入到目标表中
            foreach (var item in groupedData)
            {
                System.Collections.Generic.List<SqlParameter> listPar = new System.Collections.Generic.List<SqlParameter>()
               {
               new SqlParameter("@SubConOutFty", item.SubConOutFty),
               new SqlParameter("@ContractNumber", item.ContractNumber),
               new SqlParameter("@OrderID", item.OrderID),
               new SqlParameter("@ComboType", item.ComboType),
               new SqlParameter("@Article", item.Article),
               new SqlParameter("@SubconQty", item.OutputQty),
               new SqlParameter("@UnitPrice", item.UnitPrice),
               new SqlParameter("@LocalCurrencyID", item.LocalCurrencyID),
               new SqlParameter("@LocalUnitPrice", item.LocalUnitPrice),
               new SqlParameter("@Vat", item.Vat),
               new SqlParameter("@KpiRate", item.KpiRate),
               new SqlParameter("@Reason", item.Reason),
               new SqlParameter("@AddName", Env.User.UserID),
               };

                string sqlCmd = @"
if not exists(
select 1 from SubconOutContract_Junk 
where SubConOutFty = @SubConOutFty 
and ContractNumber = @ContractNumber 
and OrderID = @OrderID
and ComboType = @ComboType
and Article = @Article
)
Begin
INSERT INTO [dbo].[SubconOutContract_Junk]
           ([SubConOutFty]
           ,[ContractNumber]
           ,[OrderID]
           ,[ComboType]
           ,[Article]
           ,[SubconQty]
           ,[UnitPrice]
           ,[LocalCurrencyID]
           ,[LocalUnitPrice]
           ,[Vat]
           ,[KpiRate]
           ,[Reason]
           ,[AddName]
           ,[AddDate])
     VALUES
           ( @SubConOutFty
            ,@ContractNumber
            ,@OrderID
            ,@ComboType
            ,@Article
            ,@SubconQty
            ,@UnitPrice
            ,@LocalCurrencyID
            ,@LocalUnitPrice
            ,@Vat
            ,@KpiRate
            ,@Reason
            ,@AddName
            ,GetDate())
End

if exists(
select 1 from SubconOutContract_Detail 
where SubConOutFty = @SubConOutFty 
and ContractNumber = @ContractNumber 
and OrderID = @OrderID
and ComboType = @ComboType
and Article = @Article
)
BEGIN

Delete SubconOutContract_Detail 
Where SubConOutFty = @SubConOutFty 
And ContractNumber = @ContractNumber 
And OrderID = @OrderID
And ComboType = @ComboType
And Article = @Article

END

";

                var result = DBProxy.Current.Execute(string.Empty, sqlCmd, listPar);
            }

            MyUtility.Msg.InfoBox("Complete");
            this.Close();
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
