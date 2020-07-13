using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P05_IrregularQtyReason : Win.Subs.Base
    {
        DataRow _masterData;
        DataTable _detailDatas;
        DataTable dtLoad;
        string _ArtWorkReq_ID = string.Empty;
        Ict.Win.UI.DataGridViewTextBoxColumn txt_SubReason;

        public P05_IrregularQtyReason(string ArtWorkReq_ID, DataRow masterData, DataTable detailDatas)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this._masterData = masterData;
            this._ArtWorkReq_ID = ArtWorkReq_ID;
            this._detailDatas = detailDatas;
        }

        protected override void OnFormLoaded()
        {
            TxtSubconReason.CellSubconReason txtSubReason = (TxtSubconReason.CellSubconReason)TxtSubconReason.CellSubconReason.GetGridtxtCell("SQ");

            // comboSubReason.EditingControlShowing += (s, e) =>
            // {
            //     if (s==null || e == null)
            //     {
            //         return;
            //     }

            // var eventArgs = (Ict.Win.UI.DataGridViewEditingControlShowingEventArgs)e;
            //     ComboBox cb = eventArgs.Control as ComboBox;
            //     if (cb != null)
            //     {
            //         cb.SelectionChangeCommitted -= this.combo_SelectionChangeCommitted;
            //         cb.SelectionChangeCommitted += this.combo_SelectionChangeCommitted;
            //     }
            // };
            this.Query();
            this.gridIrregularQty.IsEditingReadOnly = false;
            this.gridIrregularQty.DataSource = this.listControlBindingSource1;

            #region Grid欄位設定

            this.Helper.Controls.Grid.Generator(this.gridIrregularQty)
                .Text("FactoryID", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("ArtworkTypeID", header: "Artwork Type", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("OrderID", header: "SP", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("StandardQty", header: "Std. Qty", decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("ReqQty", header: "Req. Qty", decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("SubconReasonID", header: "Reason# ", width: Widths.AnsiChars(15), iseditable: false, settings: txtSubReason).Get(out this.txt_SubReason)
                .Text("ReasonDesc", header: "Reason Desc.", iseditingreadonly: true, width: Widths.AnsiChars(15))

                .DateTime("CreateDate", header: "Create" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(16))
                .Text("CreateBy", header: "Create" + Environment.NewLine + "By", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .DateTime("EditBy", header: "Edit" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditDate", header: "Edit" + Environment.NewLine + "By", iseditingreadonly: true, width: Widths.AnsiChars(10))
                ;
            #endregion
            this.gridIrregularQty.Columns["SubconReasonID"].DefaultCellStyle.BackColor = Color.Pink;

            for (int i = 0; i < this.gridIrregularQty.Columns.Count; i++)
            {
                this.gridIrregularQty.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void combo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string newValue = ((Ict.Win.UI.DataGridViewComboBoxEditingControl)sender).EditingControlFormattedValue.ToString();
            DataRow dr = this.gridIrregularQty.GetDataRow(this.listControlBindingSource1.Position);
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            string sqlcmd = $@"select reason from SubconReason where Type='SQ' and id= '{newValue}'";
            DataRow drRow;

            if (MyUtility.Check.Seek(sqlcmd, out drRow))
            {
                dr["ReasonDesc"] = drRow["reason"];
                dr["SubconReasonID"] = newValue;
            }

            dr.EndEdit();
        }

        private void Query()
        {
            this.listControlBindingSource1.DataSource = this.getData();
        }

        public DataTable Check_Irregular_Qty()
        {
            DataTable dt = this.getData();
            this.listControlBindingSource1.DataSource = dt;

            return dt;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                DataTable ModifyTable = (DataTable)this.listControlBindingSource1.DataSource;

                StringBuilder sqlcmd = new StringBuilder();

                var Insert_Or_Update = this.dtLoad.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() != string.Empty);

                var deleteTmp = this.dtLoad.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() == string.Empty);

                foreach (var item in deleteTmp)
                {
                    string OrderID = item["OrderID"].ToString();
                    string ArtworkType = item["ArtworkTypeID"].ToString();
                    sqlcmd.Append($@"delete from ArtworkReq_IrregularQty where OrderID = '{OrderID}' and ArtworkTypeID = '{ArtworkType}'" + Environment.NewLine);
                }

                foreach (var item in Insert_Or_Update)
                {
                    string OrderID = item["OrderID"].ToString();
                    string ArtworkType = item["ArtworkTypeID"].ToString();
                    string SubconReasonID = item["SubconReasonID"].ToString();
                    decimal StandardQty = MyUtility.Convert.GetDecimal(item["StandardQty"]);
                    decimal ReqQty = MyUtility.Convert.GetDecimal(item["ReqQty"]);

                    DataTable dt;

                    DualResult result = DBProxy.Current.Select(null, $@"SELECT * FROM ArtworkReq_IrregularQty WHERE OrderID='{OrderID}' AND ArtworkTypeID='{ArtworkType}'", out dt);
                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SubconReasonID"].ToString() != SubconReasonID && !string.IsNullOrEmpty(SubconReasonID))
                            {
                                sqlcmd.Append($"UPDATE [ArtworkReq_IrregularQty] SET [SubconReasonID]='{SubconReasonID}',EditDate=GETDATE(),EditName='{Env.User.UserID}'" + Environment.NewLine);
                                sqlcmd.Append($"                                  WHERE OrderID='{OrderID}' AND [ArtworkTypeID]='{ArtworkType}'" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            sqlcmd.Append(@"
INSERT INTO [ArtworkReq_IrregularQty]([OrderID],[ArtworkTypeID],[StandardQty],[ReqQty],[SubconReasonID],[AddDate],[AddName])" + Environment.NewLine);
                            sqlcmd.Append($@"
VALUES ('{OrderID}','{ArtworkType}',{StandardQty},{ReqQty},'{SubconReasonID}',GETDATE(),'{Env.User.UserID}')" + Environment.NewLine);
                        }
                    }
                }

                if (!MyUtility.Check.Empty(sqlcmd.ToString()))
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        DualResult upResult;
                        try
                        {
                            upResult = DBProxy.Current.Execute(null, sqlcmd.ToString());
                            if (!upResult)
                            {
                                this.ShowErr(sqlcmd.ToString(), upResult);
                                return;
                            }

                            scope.Complete();
                            scope.Dispose();
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            this.ShowErr("Commit transaction error.", ex);
                        }
                    }
                }
            }

            this.Query();
            this.EditMode = !this.EditMode;
            if (this.EditMode)
            {
                this.txt_SubReason.IsEditable = true;
            }
            else
            {
                this.txt_SubReason.IsEditable = false;
            }

            this.btnEdit.Text = this.EditMode ? "Save" : "Edit";
            this.btnClose.Text = this.EditMode ? "Undo" : "Close";
        }

        private DataTable getData()
        {
            string sqlcmd = string.Empty;

            DualResult result;
            sqlcmd = $@"

-- exists DB
select distinct
o.FactoryID
,ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
,ai.OrderID
,o.StyleID
,o.BrandID
,ai.StandardQty
,ai.ReqQty
,ai.SubconReasonID
,[ReasonDesc] = (select reason from SubconReason where Type='SQ' and id=ai.SubconReasonID)
,[CreateBy] = (select name from Pass1 where id = ai.AddName)
,[CreateDate] = ai.AddDate
,[EditBy] = (select name from pass1 where id=ai.EditName)
,[EditDate] = ai.EditDate
,ArtworkID=''
,PatternCode=''
,PatternDesc=''
into #tmpDB
from ArtworkReq_IrregularQty ai
inner join Orders o on o.ID = ai.OrderID
inner join #tmp s on s.OrderID = ai.OrderID 
where ai.ArtworkTypeID like '{this._masterData["ArtworkTypeID"]}%'

-- not exists DB
select 
o.FactoryID
,ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
,[OrderID] = o.ID
,o.StyleID
,o.BrandID
,[StandardQty] = sum(oq.Qty)
,[ReqQty] = ReqQty.value + PoQty.value + s.ReqQty
,[SubconReasonID] = ''
,[ReasonDesc] = ''
,[CreateBy] = ''
,[CreateDate] = null
,[EditBy] = ''
,[EditDate] = null
,s.ArtworkID,s.PatternCode,s.PatternDesc
into #tmpCurrent
from  orders o WITH (NOLOCK) 
inner join order_qty oq WITH (NOLOCK) on oq.id = o.ID
inner join #tmp s  on s.OrderID = o.ID 
outer apply(
	select value = ISNULL(sum(PoQty),0)
    from ArtworkPO_Detail ad, ArtworkPO a
	where ad.ID = a.ID
    and OrderID = o.ID 
    and ad.PatternCode = isnull(s.PatternCode,'')
	and ad.PatternDesc = isnull(s.PatternDesc,'') 
    and ad.ArtworkID = iif(s.ArtworkID is null,'{this._masterData["ArtworkTypeID"]}',s.ArtworkID)
	and ad.ArtworkReqID =''
	and a.ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
) PoQty
outer apply(
	select value = ISNULL(sum(ReqQty),0)
	from ArtworkReq_Detail ad , ArtworkReq a
	where ad.ID = a.ID
	and OrderID = o.ID and ad.PatternCode= isnull(s.PatternCode,'')
	and ad.PatternDesc = isnull(s.PatternDesc,'') 
    and ad.ArtworkID = iif(s.ArtworkID is null,'{this._masterData["ArtworkTypeID"]}',s.ArtworkID)
	and ad.id !=  '{this._ArtWorkReq_ID}'
	and a.ArtworkTypeID = '{this._masterData["ArtworkTypeID"]}'
    and a.status != 'Closed'
)ReqQty
where not exists(
	select 1 from #tmpDB
	where orderID = o.ID and ArtworkTypeID like '{this._masterData["ArtworkTypeID"]}%'
)
group by o.FactoryID,o.ID,o.StyleID,o.BrandID,ReqQty.value,PoQty.value,s.ReqQty
,s.ArtworkID,s.PatternCode,s.PatternDesc
having ReqQty.value + PoQty.value + s.ReqQty > sum(oq.Qty) 

select * 
,row = ROW_NUMBER() over(partition by orderid,ArtworkTypeID order by  ReqQty desc)
into #tmpFinal
from
(
	select * from #tmpCurrent
	union all 
	select * from #tmpDB
) a

select * from #tmpFinal where row = 1

drop table #tmpCurrent,#tmpFinal,#tmpDB

";

            result = MyUtility.Tool.ProcessWithDatatable(this._detailDatas, string.Empty, sqlcmd, out this.dtLoad);
            if (result == false)
            {
                this.ShowErr(sqlcmd, result);
            }

            return this.dtLoad;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.btnClose.Text == "Close")
            {
                this.Close();
            }
            else
            {
                this.EditMode = !this.EditMode;
                this.btnEdit.Text = "Edit";
                this.btnClose.Text = "Close";
                if (this.EditMode)
                {
                    this.txt_SubReason.IsEditable = true;
                }
                else
                {
                    this.txt_SubReason.IsEditable = false;
                }

                // 回到檢視模式，並且重新取得資料
                this.Query();
            }
        }
    }
}
